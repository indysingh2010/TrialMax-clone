using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.Drawing;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class creates a grid-style control for viewing the terms in a TrialMax database filter</summary>
	public class CTmaxResultsGridCtrl : CTmaxBaseCtrl
	{
		#region Constants

		//	Column  identifiers
		private const string RESULT_COLUMN_SCENE		= "Scene";
		private const string RESULT_COLUMN_TRANSCRIPT	= "Transcript";
		private const string RESULT_COLUMN_PAGE			= "Page";
		private const string RESULT_COLUMN_LINE			= "Line";
		private const string RESULT_COLUMN_TEXT			= "Text";
		private const string RESULT_COLUMN_TAG			= "Tag";

		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX = ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_SET_COLUMNS_EX = ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_ADD_EX = ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_CREATE_DATA_SOURCE_EX = ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_CREATE_DATA_TABLE_EX = ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_ON_INITIALIZE_LAYOUT_EX = ERROR_TMAX_BASE_CONTROL_MAX + 6;
		private const int ERROR_ADD_COLLECTION_EX = ERROR_TMAX_BASE_CONTROL_MAX + 7;
		private const int ERROR_GET_SELECTED_EX = ERROR_TMAX_BASE_CONTROL_MAX + 8;
		private const int ERROR_SET_SELECTION_EX = ERROR_TMAX_BASE_CONTROL_MAX + 9;
		private const int ERROR_UPDATE_EX = ERROR_TMAX_BASE_CONTROL_MAX + 10;
		private const int ERROR_DELETE_DATA_ROW_EX = ERROR_TMAX_BASE_CONTROL_MAX + 11;
		private const int ERROR_DELETE_GRID_ROW_EX = ERROR_TMAX_BASE_CONTROL_MAX + 12;
		private const int ERROR_SET_COLUMN_PROPS_EX = ERROR_TMAX_BASE_CONTROL_MAX + 13;
		private const int ERROR_CHOOSE_COLUMNS_EX = ERROR_TMAX_BASE_CONTROL_MAX + 14;

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		protected System.ComponentModel.IContainer components = null;

		/// <summary>Infragistics grid control used to display objections</summary>
		private Infragistics.Win.UltraWinGrid.UltraGrid m_ctrlGrid;

		/// <summary>Local member used as the data source for the grid</summary>
		private System.Data.DataSet m_dataSet = null;

		/// <summary>Local member used to store objections</summary>
		private System.Data.DataTable m_dataTable = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Fired by the control when the user selects a new row</summary>
		public event System.EventHandler SelectionChanged;

		/// <summary>Fired by the control when the user selects a new row</summary>
		public event System.EventHandler DblClick;

		/// <summary>Constructor</summary>
		public CTmaxResultsGridCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();

			//	Initialize the event managers
			m_tmaxEventSource.Name = "Search Results Grid";

		}// public CTmaxResultsGridCtrl() : base()

		/// <summary>This method is called to initialize the grid to display properties associated with the specified owner type</summary>
		/// <param name="xmlFile">The configuration file containing the configuration</param>
		/// <returns>true if successful</returns>
		public bool Initialize(CXmlIni xmlFile)
		{
			try
			{
				//	Create the data source
				CreateDataSource();

				//	Assign the data source to the grid
				if(m_dataSet != null)
					m_ctrlGrid.DataSource = m_dataSet;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);

				//	Clean up
				FreeDataSource();
			}

			return (m_dataSet != null);

		}// public bool Initialize()

		/// <summary>This method is called to add an object to the grid</summary>
		/// <param name="tmaxResult">The object to be added</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxSearchResult tmaxResult)
		{
			bool bSuccessful = false;
			DataRow row = null;

			Debug.Assert(tmaxResult != null);
			if(tmaxResult == null) return false;

			Debug.Assert(m_dataTable != null);
			if(m_dataTable == null) return false;

			try
			{
				//	Create a new row for this property
				row = m_dataTable.NewRow();

				//	Set the column values
				SetColumns(row, tmaxResult);

				//	Add to the table
				m_dataTable.Rows.Add(row);

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			}

			return bSuccessful;

		}// public bool Add(CTmaxSearchResult tmaxResult)

		/// <summary>Called to add the collection of objects to the grid</summary>
		/// <param name="tmaxResults">The objects to be added</param>
		/// <param name="bClear">true to clear before adding</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxSearchResults tmaxResults, bool bClear)
		{
			bool bSuccessful = true;

			//	Are we supposed to clear the existing rows?
			if(bClear)
				Clear();

			//	Now add the new rows
			if((tmaxResults != null) && (tmaxResults.Count > 0))
			{
				//	Add each of the properties
				foreach(CTmaxSearchResult O in tmaxResults)
				{
					if(Add(O) == false)
						bSuccessful = false;
				}

			}// if((tmaxResults != null) && (tmaxResults.Count > 0))

			return bSuccessful;

		}// public bool Add(CTmaxSearchResults tmaxResults, bool bClear)

		/// <summary>This method will delete the row bound to the specified objection</summary>
		/// <param name="tmaxResult">The object bound to the row</param>
		/// <returns>the index of the row containing the object that was deleted</returns>
		public int Delete(CTmaxSearchResult tmaxResult)
		{
			UltraGridRow row = null;
			int iIndex = -1;

			//	Get the requested row in the data source
			if((row = GetRow(tmaxResult, null)) != null)
			{
				iIndex = row.Index;
				Delete(row);
			}
			else
			{
				m_tmaxEventSource.FireDiagnostic(this, "Delete", "Unable to locate data row with specified result");
			}

			return iIndex;

		}// public bool Delete(CTmaxSearchResult tmaxResult)

		/// <summary>Called to remove all rows from the list box</summary>
		public void Clear()
		{
			try
			{
				if(m_dataSet != null)
					m_dataSet.Clear();
			}
			catch
			{
			}

		}// public void Clear()

		/// <summary>This method is called to get the object that is selected in the grid</summary>
		/// <returns>The selected object, null if none or more than one selected</returns>
		public CTmaxSearchResult GetSelection()
		{
			CTmaxSearchResult tmaxSelection = null;

			try
			{
				if(m_ctrlGrid.Selected != null)
				{
					if((m_ctrlGrid.Selected.Rows != null) && (m_ctrlGrid.Selected.Rows.Count > 0))
					{
						tmaxSelection = (CTmaxSearchResult)(m_ctrlGrid.Selected.Rows[0].Cells[RESULT_COLUMN_TAG].Value);
					}

				}// if(m_ctrlGrid.Selected != null)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetSelection", Ex);
			}

			return tmaxSelection;

		}// public CTmaxSearchResult GetSelection()

		/// <summary>This method is called to get the collection of selected objects</summary>
		/// <param name="tmaxSelected">The collection in which to store the objects</param>
		/// <returns>The total number of selected objects</returns>
		public int GetSelected(CTmaxSearchResults tmaxSelected)
		{
			return GetSelected(tmaxSelected, null);
		}

		/// <summary>This method is called to get the collection of selected objects</summary>
		/// <returns>The collection of selected objects if any</returns>
		public CTmaxSearchResults GetSelected()
		{
			CTmaxSearchResults tmaxSelected = new CTmaxSearchResults();

			if(GetSelected(tmaxSelected) > 0)
				return tmaxSelected;
			else
				return null;

		}// public CTmaxSearchResults GetSelected()

		/// <summary>This method is called to get the total number of selections</summary>
		public int GetSelectedCount()
		{
			return GetSelected(null);
		}

		/// <summary>This method is called to get the total number of rows</summary>
		public int GetTotalCount()
		{
			if(m_ctrlGrid.Rows != null)
				return m_ctrlGrid.Rows.Count;
			else
				return 0;

		}// public int GetTotalCount()

		/// <summary>This method is called to get the index of the row bound to the specified object</summary>
		/// <param name="tmaxResult">The object associated with the row</param>
		///	<returns>The index of the row</returns>
		public int GetIndex(CTmaxSearchResult tmaxResult)
		{
			UltraGridCell cell = null;

			try
			{
				for(int i = 0; i < m_ctrlGrid.Rows.Count; i++)
				{
					//	Get the cell containing the object reference
					if((cell = GetCell(m_ctrlGrid.Rows[i], RESULT_COLUMN_TAG)) != null)
					{
						//	Is this the one we're looking for?
						if(ReferenceEquals(cell.Value, tmaxResult) == true)
							return i;
					}

				}// for(int i = 0; i < m_ctrlGrid.Rows.Count; i++)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetIndex", Ex);
			}

			return -1;

		}// public UltraGridRow GetIndex(CTmaxSearchResult tmaxResult)

		/// <summary>Called to get the search result associated with the specified row in the grid</summary>
		/// <param name="row">The row bound to the desired object</param>
		/// <returns>The search result object</returns>
		public CTmaxSearchResult GetResult(UltraGridRow row)
		{
			UltraGridCell cell = null;
			CTmaxSearchResult tmaxResult = null;

			if((row != null) && (row.Cells != null))
			{
				if((cell = GetCell(row, RESULT_COLUMN_TAG)) != null)
				{
					try { tmaxResult = ((CTmaxSearchResult)(cell.Value)); }
					catch { }

				}// if((cell = GetCell(row, RESULT_COLUMN_TAG)) != null)

			}// if((row != null) && (row.Cells != null))

			return tmaxResult;

		}// public CTmaxSearchResult GetResult(UltraGridRow row)

		/// <summary>Called to get the search result object associated with the specified cell</summary>
		/// <param name="cell">The cell in the row bound to the result object</param>
		/// <returns>The search result object</returns>
		public CTmaxSearchResult GetResult(UltraGridCell cell)
		{
			if((cell != null) && (cell.Row != null))
				return GetResult(cell.Row);
			else
				return null;

		}// public CTmaxSearchResult GetResult(UltraGridCell cell)

		/// <summary>Called to get the search result associated with the specified row in the grid</summary>
		/// <param name="iIndex">The index of the desired row</param>
		/// <returns>The associated search result</returns>
		public CTmaxSearchResult GetResult(int iIndex)
		{
			if((iIndex >= 0) && (iIndex < m_ctrlGrid.Rows.Count))
				return GetResult(m_ctrlGrid.Rows[iIndex]);
			else
				return null;

		}// public CTmaxSearchResult GetResult(int iIndex)

		/// <summary>This method is called to select the row bound to the specified result</summary>
		/// <param name="tmaxResult">The result to be selected</param>
		/// <returns>true if successful</returns>
		public bool SetSelection(CTmaxSearchResult tmaxResult)
		{
			UltraGridRow row = null;
			bool bSuccessful = false;

			try
			{
				if((row = GetRow(tmaxResult, null)) != null)
				{
					bSuccessful = SetSelection(row);
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}

			return bSuccessful;

		}// public bool SetSelection(CTmaxSearchResult tmaxResult)

		/// <summary>This method is called to select the specified row</summary>
		/// <param name="lIndex">The index of the row to be selected</param>
		/// <returns>true if successful</returns>
		public bool SetSelection(int iIndex)
		{
			bool bSuccessful = false;

			try
			{
				if(m_ctrlGrid.Rows != null)
				{
					if((iIndex >= 0) && (iIndex < m_ctrlGrid.Rows.Count))
					{
						bSuccessful = SetSelection(m_ctrlGrid.Rows[iIndex]);

					}// if((iIndex >= 0) && (iIndex < m_ctrlGrid.Rows.Count))

				}// if(m_ctrlGrid.Rows != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}

			return bSuccessful;

		}// public bool SetSelection(int iIndex)

		/// <summary>This method will update the row bound to the specified object</summary>
		/// <param name="tmaxResult">The result bound to the row</param>
		/// <returns>true if successful</returns>
		public bool Update(CTmaxSearchResult tmaxResult)
		{
			DataRow dr = null;
			UltraGridRow gr = null;
			bool bSuccessful = true;

			if(m_dataTable == null) return false;
			if(m_dataTable.Rows == null) return false;

			//	Does the caller want to update all objections?
			if(tmaxResult == null)
			{
				foreach(DataRow O in m_dataTable.Rows)
				{
					if((tmaxResult = (CTmaxSearchResult)(O[RESULT_COLUMN_TAG])) != null)
					{
						if(SetColumns(dr, tmaxResult) == false)
							bSuccessful = false;

					}// if((tmaxResult = (CTmaxSearchResult)(O[RESULT_COLUMN_TAG])) != null)

				}// foreach(DataRow O in m_dataTable.Rows)

			}
			else
			{
				if((dr = GetSourceRow(tmaxResult)) != null)
				{
					if((bSuccessful = SetColumns(dr, tmaxResult)) == true)
					{
						if((gr = GetRow(tmaxResult, null)) != null)
							gr.PerformAutoSize();

					}// if((bSuccessful = SetColumns(dr, tmaxResult)) == true)

				}// if((dr = GetSourceRow(tmaxResult)) != null)

			}// if(tmaxResult == null)

			return bSuccessful;

		}// public bool Update(CTmaxSearchResult tmaxResult)

		/// <summary>This method is called by the application when it is about to terminate</summary>
		/// <param name="xmlFile">The configuration file containing the configuration</param>
		public void Save(CXmlIni xmlFile)
		{

		}// public void Save(CXmlIni xmlFile)

		#endregion Public Methods

		#region Protected Methods

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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the control");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the columns");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding an objection)");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while creating the data source");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while creating the data table");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the display layout");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding a collection of objections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the current selections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the current selections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while updating an objection");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while deleting the data row from the source table");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while deleting the row from the grid");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the column properties: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while choosing the display columns.");

		}// override protected void SetErrorStrings()

		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		override protected void RecalcLayout()
		{
			//	Resize the columns
			ResizeColumns();
		}

		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			this.m_ctrlGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlGrid
			// 
			appearance1.BackColor = System.Drawing.SystemColors.Window;
			appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.m_ctrlGrid.DisplayLayout.Appearance = appearance1;
			this.m_ctrlGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGrid.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
			appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
			appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance2.BorderColor = System.Drawing.SystemColors.Window;
			this.m_ctrlGrid.DisplayLayout.GroupByBox.Appearance = appearance2;
			appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
			this.m_ctrlGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
			this.m_ctrlGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.m_ctrlGrid.DisplayLayout.GroupByBox.Hidden = true;
			appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
			appearance4.BackColor2 = System.Drawing.SystemColors.Control;
			appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
			this.m_ctrlGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
			this.m_ctrlGrid.DisplayLayout.MaxColScrollRegions = 1;
			this.m_ctrlGrid.DisplayLayout.MaxRowScrollRegions = 1;
			appearance5.BackColor = System.Drawing.SystemColors.Window;
			appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_ctrlGrid.DisplayLayout.Override.ActiveCellAppearance = appearance5;
			this.m_ctrlGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
			this.m_ctrlGrid.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
			this.m_ctrlGrid.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
			this.m_ctrlGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
			appearance6.BackColor = System.Drawing.SystemColors.Window;
			this.m_ctrlGrid.DisplayLayout.Override.CardAreaAppearance = appearance6;
			appearance7.BorderColor = System.Drawing.Color.Silver;
			appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.m_ctrlGrid.DisplayLayout.Override.CellAppearance = appearance7;
			this.m_ctrlGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.m_ctrlGrid.DisplayLayout.Override.CellPadding = 0;
			appearance8.BackColor = System.Drawing.SystemColors.Control;
			appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance8.BorderColor = System.Drawing.SystemColors.Window;
			this.m_ctrlGrid.DisplayLayout.Override.GroupByRowAppearance = appearance8;
			appearance9.TextHAlignAsString = "Left";
			this.m_ctrlGrid.DisplayLayout.Override.HeaderAppearance = appearance9;
			this.m_ctrlGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
			this.m_ctrlGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance10.BackColor = System.Drawing.SystemColors.Window;
			appearance10.BorderColor = System.Drawing.Color.Silver;
			this.m_ctrlGrid.DisplayLayout.Override.RowAppearance = appearance10;
			this.m_ctrlGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
			this.m_ctrlGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
			appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
			this.m_ctrlGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
			this.m_ctrlGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.m_ctrlGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.m_ctrlGrid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.m_ctrlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlGrid.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlGrid.Name = "m_ctrlGrid";
			this.m_ctrlGrid.Size = new System.Drawing.Size(150, 150);
			this.m_ctrlGrid.TabIndex = 0;
			this.m_ctrlGrid.Text = "SearchResultsGrid";
			this.m_ctrlGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.OnGridInitializeRow);
			this.m_ctrlGrid.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.OnGridBeforeSelectChange);
			this.m_ctrlGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
			this.m_ctrlGrid.DoubleClick += new System.EventHandler(this.OnGridDblClick);
			this.m_ctrlGrid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.OnGridAfterSelectChange);
			this.m_ctrlGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.OnGridInitializeLayout);
			// 
			// CTmaxResultsGridCtrl
			// 
			this.Controls.Add(this.m_ctrlGrid);
			this.Name = "CTmaxResultsGridCtrl";
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGrid)).EndInit();
			this.ResumeLayout(false);

		}// protected void InitializeComponent()

		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

		}// protected override void Dispose(bool disposing)

		#endregion Protected Methods

		#region Private Methods

		/// <summary>This method is called to select the specified row</summary>
		/// <param name="row">The row being selected</param>
		/// <returns>true if successful</returns>
		private bool SetSelection(UltraGridRow row)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlGrid.Selected.Rows.Clear();
				m_ctrlGrid.ActiveRow = null;

				if(row != null)
				{
					row.Selected = true;
					m_ctrlGrid.ActiveRow = row;
				}

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetSelection(UltraGridRow row)

		/// <summary>This method is called to update the columns of the specified data row</summary>
		/// <param name="dr">The data row to be updated</param>
		/// <param name="tmaxResult">The object bound to the row</param>
		/// <returns>true if successful</returns>
		private bool SetColumns(DataRow dr, CTmaxSearchResult tmaxResult)
		{
			bool bSuccessful = false;

			Debug.Assert(dr != null);
			if(dr == null) return false;

			Debug.Assert(tmaxResult != null);
			if(tmaxResult == null) return false;

			Debug.Assert(m_dataTable != null);
			if(m_dataTable == null) return false;

			try
			{
				//	Set the column values
				dr[RESULT_COLUMN_SCENE] = (tmaxResult.IScene != null) ? tmaxResult.IScene.GetBarcode(false) : "";
				dr[RESULT_COLUMN_TRANSCRIPT] = tmaxResult.Transcript;
				dr[RESULT_COLUMN_PAGE] = tmaxResult.Page;
				dr[RESULT_COLUMN_LINE] = tmaxResult.Line;
				dr[RESULT_COLUMN_TEXT] = tmaxResult.Text;
				dr[RESULT_COLUMN_TAG] = tmaxResult;

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetColumns", m_tmaxErrorBuilder.Message(ERROR_SET_COLUMNS_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetColumns(DataRow dr, CTmaxSearchResult tmaxObjection)

		/// <summary>This method will resize the columns</summary>
		private void ResizeColumns()
		{
			try
			{
				SuspendLayout();


				ResumeLayout();
			}
			catch
			{
			}

		}// public void ResizeColumns()

		/// <summary>This method is called to get the specified row from the Properties table</summary>
		/// <param name="IProperty">The Interface of the desired row</param>
		///	<returns>The row object with the specified Id</returns>
		private System.Data.DataRow GetSourceRow(CTmaxSearchResult tmaxObjection)
		{
			if(m_dataTable == null) return null;
			if(m_dataTable.Rows == null) return null;
			if(m_dataTable.Rows.Count == 0) return null;

			foreach(DataRow O in m_dataTable.Rows)
			{
				if(ReferenceEquals(O[RESULT_COLUMN_TAG], tmaxObjection) == true)
					return O;
			}

			return null;

		}// private System.Data.DataRow GetSourceRow(CTmaxSearchResult tmaxObjection)

		/// <summary>This method is called to get the specified row</summary>
		/// <param name="tmaxObjection">The object associated with the row</param>
		///	<param name="rows">The collection of rows to be searched</param>
		///	<returns>The row object with the specified objection</returns>
		private UltraGridRow GetRow(CTmaxSearchResult tmaxObjection, RowsCollection rows)
		{
			UltraGridCell cell = null;

			//	Use the root collection if not specified by the caller
			if(rows == null)
				rows = this.m_ctrlGrid.Rows;

			Debug.Assert(rows != null);
			if(rows == null) return null;

			try
			{
				foreach(UltraGridRow O in rows)
				{
					if((cell = GetCell(O, RESULT_COLUMN_TAG)) != null)
					{
						//	Is this the one we're looking for?
						if(ReferenceEquals(cell.Value, tmaxObjection) == true)
							return O;
					}

				}// foreach(UltraGridRow O in rows)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetRow", Ex);
			}

			return null;

		}// private UltraGridRow GetRow(CTmaxSearchResult tmaxObjection, RowsCollection rows)

		/// <summary>This method is called to get the specified column</summary>
		/// <param name="strName">The name of the desired column</param>
		///	<returns>The column object with the specified name</returns>
		private UltraGridColumn GetColumn(string strName)
		{
			UltraGridColumn column = null;

			try
			{
				Debug.Assert(strName != null);
				Debug.Assert(strName.Length > 0);

				if(m_ctrlGrid == null) return null;
				if(m_ctrlGrid.DisplayLayout == null) return null;
				if(m_ctrlGrid.DisplayLayout.Bands == null) return null;
				if(m_ctrlGrid.DisplayLayout.Bands.Count == 0) return null;

				if(m_ctrlGrid.DisplayLayout.Bands[0] != null)
				{
					column = m_ctrlGrid.DisplayLayout.Bands[0].Columns[strName];
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetColumn", "Exception: name = " + strName, Ex);
			}

			return column;

		}// private UltraGridColumn GetColumn(string strName)

		/// <summary>This method will delete the specified row in the Properties table</summary>
		/// <param name="dr">The data row to be deleted</param>
		/// <returns>true if successful</returns>
		private bool Delete(DataRow dr)
		{
			bool bSuccessful = false;

			try
			{
				//m_ctrlUltraGrid.PerformAction(UltraGridAction.ExitEditMode);
				dr.Delete();
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Delete", m_tmaxErrorBuilder.Message(ERROR_DELETE_DATA_ROW_EX), Ex);
			}

			return bSuccessful;

		}// private bool Delete(DataRow dr)

		/// <summary>This method will delete the specified row in the grid</summary>
		/// <param name="row">The row to be deleted</param>
		/// <returns>true if successful</returns>
		private bool Delete(UltraGridRow row)
		{
			bool bSuccessful = false;

			try
			{
				//m_ctrlUltraGrid.PerformAction(UltraGridAction.ExitEditMode);
				row.Delete(false);
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Delete", m_tmaxErrorBuilder.Message(ERROR_DELETE_GRID_ROW_EX), Ex);
			}

			return bSuccessful;

		}// private bool Delete(UltraGridRow row)

		/// <summary>This method will release the data source and reset the local members</summary>
		private void FreeDataSource()
		{
			try
			{
				m_ctrlGrid.DataSource = null;

				if(m_dataSet != null)
					m_dataSet.Clear();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FreeDataSource", Ex);
			}
			finally
			{
				m_dataSet = null;
				m_dataTable = null;
			}

		}// private void FreeDataSource()

		/// <summary>This method will create a new data source for the grid</summary>
		/// <returns>The data set to be assigned as the source for the grid control</returns>
		private System.Data.DataSet CreateDataSource()
		{
			//	Release the existing data source
			FreeDataSource();

			try
			{
				//	Create the data table
				CreateDataTable();

				//	Nothing to do if no objections
				if(m_dataTable == null) return null;

				//	Create the source data set
				m_dataSet = new System.Data.DataSet();

				//	Add the data table to the set
				m_dataSet.Tables.Add(m_dataTable);

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDataSource", m_tmaxErrorBuilder.Message(ERROR_CREATE_DATA_SOURCE_EX), Ex);
				FreeDataSource();
			}

			return m_dataSet;

		}// private System.Data.DataSet CreateDataSource()

		/// <summary>This method will create the data table used to store the results displayed in the grid</summary>
		/// <returns>The new Properties table</returns>
		private System.Data.DataTable CreateDataTable()
		{
			Debug.Assert(m_dataTable == null);

			try
			{
				//	Create a data table for the objections
				m_dataTable = new DataTable("SearchResults");

				//	Add the columns
				//
				//	NOTE:	These MUST be added in the order in which the columns are enumerated
				m_dataTable.Columns.Add(RESULT_COLUMN_SCENE, typeof(string));
				m_dataTable.Columns.Add(RESULT_COLUMN_TRANSCRIPT, typeof(string));
				m_dataTable.Columns.Add(RESULT_COLUMN_PAGE, typeof(long));
				m_dataTable.Columns.Add(RESULT_COLUMN_LINE, typeof(Int32));
				m_dataTable.Columns.Add(RESULT_COLUMN_TEXT, typeof(string));
				m_dataTable.Columns.Add(RESULT_COLUMN_TAG, typeof(object));

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDataTable", m_tmaxErrorBuilder.Message(ERROR_CREATE_DATA_TABLE_EX), Ex);
			}

			return m_dataTable;

		}// private System.Data.DataTable CreateDataTable()

		/// <summary>This method handles events fired by the grid when it attempts to initialize its layout</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridInitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraGridColumn column = null;

			try
			{
				if((column = GetColumn(RESULT_COLUMN_TAG)) != null)
					column.Hidden = true;

				m_ctrlGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;

				e.Layout.Override.RowSizing = RowSizing.AutoFree;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnGridInitializeLayout", m_tmaxErrorBuilder.Message(ERROR_ON_INITIALIZE_LAYOUT_EX), Ex);
			}

		}// private void OnGridInitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)

		/// <summary>This method handles events fired by the grid when it initializes a new row</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridInitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
				e.Row.Activation = Activation.NoEdit;

				//e.Row.Cells[PROPERTIES_COLUMN_VALUE].Activation = Activation.ActivateOnly;
				//e.Row.Cells[PROPERTIES_COLUMN_NAME].Appearance.ForeColor = System.Drawing.SystemColors.ControlDark;
				//e.Row.Cells[PROPERTIES_COLUMN_VALUE].Appearance.ForeColor = System.Drawing.SystemColors.ControlDark;

				//if(m_bResizeColumns == true)
				//    SetColumnWidths();

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridInitializeRow", Ex);
			}

		}// private void OnGridInitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)

		/// <summary>This method handles AfterSelectChange events fired by the grid</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridAfterSelectChange(object sender, AfterSelectChangeEventArgs e)
		{
			try
			{
				if(this.SelectionChanged != null)
				{
					if(e.Type == typeof(UltraGridRow))
					{
						SelectionChanged(this, System.EventArgs.Empty);
					}

				}// if(this.SelectionChanged != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridAfterSelectChange", Ex);
			}

		}// private void OnGridAfterSelectChange(object sender, AfterSelectChangeEventArgs e)

		/// <summary>This method handles BeforeSelectChange events fired by the grid</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridBeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
		{
			try
			{
				//	Prevent the user from selecting a column header
				if(e.Type == typeof(Infragistics.Win.UltraWinGrid.ColumnHeader))
				{
					e.Cancel = true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridBeforeSelectChange", Ex);
			}

		}// private void OnGridBeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)

		/// <summary>This method will handle DoubleClick events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnGridDblClick(object sender, System.EventArgs e)
		{
			if(DblClick != null)
				DblClick(this, System.EventArgs.Empty);
		}

		/// <summary>This method handles MouseDown events fired by the grid control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UltraGridCell cell = null;

			//	Is this the right mouse button?
			if(e.Button == MouseButtons.Right)
			{
				if((cell = GetCell(e.X, e.Y)) != null)
				{
					if(IsSelected(cell.Row) == false)
					{
						m_ctrlGrid.Selected.Rows.Clear();
						cell.Row.Selected = true;
					}

				}// if(cell != null)

			}// if(e.Button = MouseButtons.Right)	

			//	Bubble the event
			try { OnMouseDown(e); }
			catch(System.Exception Ex) { m_tmaxEventSource.FireDiagnostic(this, "OnMouseDown", Ex); }

		}// private void OnGridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This gets the cell at the specified location within the client area</summary>
		/// <param name="iX">The X position in client coordinates</param>
		/// <param name="iY">The Y position in client coordinates</param>
		private UltraGridCell GetCell(int iX, int iY)
		{
			UIElement uiElement = null;
			UltraGridCell cell = null;

			try
			{
				//	Retrieve the UIElement at the specified location
				uiElement = m_ctrlGrid.DisplayLayout.UIElement.ElementFromPoint(new Point(iX, iY));

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

		/// <summary>This method is called to get the collection of selected objections</summary>
		/// <param name="tmaxSelected">The collection in which to store the objections</param>
		/// <param name="rows">The collection of rows to be searched</param>
		/// <returns>The total number of selected objections</returns>
		private int GetSelected(CTmaxSearchResults tmaxSelected, RowsCollection rows)
		{
			if(m_ctrlGrid == null) return 0;
			if(m_ctrlGrid.IsDisposed == true) return 0;
			if(m_ctrlGrid.Rows == null) return 0;
			if(m_ctrlGrid.Rows.Count == 0) return 0;
			if(m_dataTable == null) return 0;
			if(m_dataTable.Rows == null) return 0;
			if(m_dataTable.Rows.Count == 0) return 0;

			try
			{
				//	Do we need to allocate a collection?
				if(tmaxSelected == null)
					tmaxSelected = new CTmaxSearchResults();
				else
					tmaxSelected.Clear();

				//	Should we use the root collection?
				if(rows == null)
					rows = m_ctrlGrid.Rows;

				foreach(UltraGridRow O in rows)
				{
					//	Is this row selected?
					if(IsSelected(O) == true)
						tmaxSelected.Add((CTmaxSearchResult)(O.Cells[RESULT_COLUMN_TAG].Value));

				}// foreach(UltraGridRow O in rows)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSelected", m_tmaxErrorBuilder.Message(ERROR_GET_SELECTED_EX), Ex);
			}

			if(tmaxSelected != null)
				return tmaxSelected.Count;
			else
				return 0;

		}// private int GetSelected(CTmaxSearchResults tmaxSelected, RowCollection rows)

		/// <summary>This method is called to determine if the specified property row is selected</summary>
		/// <param name="aSelected">The row being checked</param>
		/// <returns>true if selected</returns>
		private bool IsSelected(UltraGridRow row)
		{
			bool bSelected = false;

			if(m_ctrlGrid == null) return false;
			if(m_ctrlGrid.IsDisposed == true) return false;
			if(m_ctrlGrid.Rows == null) return false;
			if(m_ctrlGrid.Rows.Count == 0) return false;
			if(m_dataTable == null) return false;
			if(m_dataTable.Rows == null) return false;
			if(m_dataTable.Rows.Count == 0) return false;
			if(row == null) return false;

			try
			{
				//	Is this row selected?
				if(row.Selected == true)
					bSelected = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IsSelected", Ex);
			}

			return bSelected;

		}

		/// <summary>Called to get the cell in the specified column position</summary>
		/// <param name="row">The row that contains the cell</param>
		/// <param name="iColumn">The column name</param>
		/// <returns>The cell in the requested column</returns>
		public UltraGridCell GetCell(UltraGridRow row, string strColumn)
		{
			UltraGridCell cell = null;

			Debug.Assert(strColumn.Length > 0);

			try
			{
				//	Get the cell with the matching name
				if((row != null) && (row.Cells != null))
					cell = row.Cells[strColumn];
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetCell", Ex);
			}

			return cell;

		}

		#endregion Private Methods

	}// class CTmaxResultsGridCtrl

}// namespace FTI.Trialmax.Controls

