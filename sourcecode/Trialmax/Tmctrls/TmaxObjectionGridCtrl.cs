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
	public class CTmaxObjectionGridCtrl : CTmaxBaseCtrl
	{
		#region Constants

		//	Column  identifiers
		private const int OBJECTION_COLUMN_SIDE			= 0;
		private const int OBJECTION_COLUMN_STATE		= 1;
		private const int OBJECTION_COLUMN_DEPOSITION	= 2;
		private const int OBJECTION_COLUMN_FIRST		= 3;
		private const int OBJECTION_COLUMN_LAST			= 4;
		private const int OBJECTION_COLUMN_ARGUMENT		= 5;
		private const int OBJECTION_COLUMN_RULING		= 6;
		private const int OBJECTION_COLUMN_RULING_TEXT	= 7;
		private const int OBJECTION_COLUMN_RESPONSE_1	= 8;
		private const int OBJECTION_COLUMN_RESPONSE_2	= 9;
		private const int OBJECTION_COLUMN_RESPONSE_3	= 10;
		private const int OBJECTION_COLUMN_WORK_PRODUCT = 11;
		private const int OBJECTION_COLUMN_COMMENTS		= 12;
		private const int OBJECTION_COLUMN_ID			= 13;
		private const int OBJECTION_COLUMN_CASE_NAME	= 14;
		private const int OBJECTION_COLUMN_CASE_ID		= 15;
		private const int OBJECTION_COLUMN_OBJECT		= 16;
		private const int OBJECTION_MAX_COLUMNS			= 17;

		private const string XMLINI_KEY_ASCENDING_DEPOSITIONS	= "ascendingDepositions";
		private const string XMLINI_KEY_SORT_COLUMN				= "sortColumn";
		private const string XMLINI_KEY_SORT_ASCENDING			= "sortAscending";
		private const string XMLINI_COLUMN_ATTRIBUTE_WIDTH		= "width";
		private const string XMLINI_COLUMN_ATTRIBUTE_VISIBLE	= "visible";
		private const string XMLINI_COLUMN_ATTRIBUTE_POSITION	= "position";
	
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_SET_COLUMNS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_ADD_EX					= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_CREATE_DATA_SOURCE_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_CREATE_DATA_TABLE_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_ON_INITIALIZE_LAYOUT_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 6;
		private const int ERROR_ADD_COLLECTION_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 7;
		private const int ERROR_GET_SELECTED_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 8;
		private const int ERROR_SET_SELECTION_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 9;
		private const int ERROR_UPDATE_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 10;
		private const int ERROR_DELETE_DATA_ROW_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 11;
		private const int ERROR_DELETE_GRID_ROW_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 12;
		private const int ERROR_SET_COLUMN_PROPS_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 13;
		private const int ERROR_CHOOSE_COLUMNS_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 14;

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		protected System.ComponentModel.IContainer components = null;

		/// <summary>Infragistics grid control used to display objections</summary>
		private Infragistics.Win.UltraWinGrid.UltraGrid m_ctrlGrid;

		/// <summary>Local member used as the data source for the grid</summary>
		private System.Data.DataSet m_dsSource = null;

		/// <summary>Local member used to store objections</summary>
		private System.Data.DataTable m_dtObjections = null;

		/// <summary>Local member to sort the Deposition column</summary>
		private CTmaxObjectionGridComparer m_tmaxDepositionComparer = new CTmaxObjectionGridComparer();

		/// <summary>Local member to sort the FirstPL column</summary>
		private CTmaxObjectionGridComparer m_tmaxFirstPLComparer = new CTmaxObjectionGridComparer();

		/// <summary>Local member to sort the LastPL column</summary>
		private CTmaxObjectionGridComparer m_tmaxLastPLComparer = new CTmaxObjectionGridComparer();

		/// <summary>Local member to sort the column by text</summary>
		private CTmaxObjectionGridComparer m_tmaxTextComparer = new CTmaxObjectionGridComparer();

		private CTmaxObjectionGridColumn[] m_aColumns = new CTmaxObjectionGridColumn[OBJECTION_MAX_COLUMNS];

		/// <summary>Local member to keep track of deposition sort order</summary>
		private bool m_bAscendingDepositions = true;

		/// <summary>Local member to keep track of current sort column</summary>
		private int m_iSortColumn = OBJECTION_COLUMN_DEPOSITION;

		private bool m_bSortAscending = true;
		
		#endregion Private Members

		#region Public Methods

		/// <summary>This is the delegate used to handle SelectionChanging events fired by this control</summary>
		/// <param name="sender">Object firing the event</param>
		/// <return>true to cancel the change</return>
		public delegate bool SelectionChangingHandler(object sender);

		/// <summary>Fired by the control when the current selection is about to change</summary>
		public event SelectionChangingHandler SelectionChanging;

		/// <summary>Fired by the control when the user selects a new row</summary>
		public event System.EventHandler SelectionChanged;

		/// <summary>Fired by the control when the user changes the sort order</summary>
		public event System.EventHandler SortChanged;

		/// <summary>Fired by the control when the user selects a new row</summary>
		public event System.EventHandler DblClick;
		
		/// <summary>Constructor</summary>
		public CTmaxObjectionGridCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();
			
			//	Initialize the event managers
			m_tmaxEventSource.Name = "Objection Grid Control";
			m_tmaxEventSource.Attach(m_tmaxDepositionComparer.EventSource);
			m_tmaxEventSource.Attach(m_tmaxFirstPLComparer.EventSource);
			m_tmaxEventSource.Attach(m_tmaxLastPLComparer.EventSource);
			m_tmaxEventSource.Attach(m_tmaxTextComparer.EventSource);
			
		}// public CTmaxObjectionGridCtrl() : base()

		/// <summary>This method is called to initialize the grid to display properties associated with the specified owner type</summary>
		/// <param name="xmlFile">The configuration file containing the configuration</param>
		/// <returns>true if successful</returns>
		public bool Initialize(CXmlIni xmlFile)
		{
			//	NOTE:	We assume the file has already been set to the correct section

			try
			{
				//	Initialize the sort comparers
				m_tmaxDepositionComparer.GridCtrl = this;
				m_tmaxFirstPLComparer.GridCtrl = this;
				m_tmaxLastPLComparer.GridCtrl = this;
				m_tmaxTextComparer.GridCtrl = this;
				m_tmaxDepositionComparer.SortColumn = CTmaxObjectionGridComparer.SORT_COLUMN_DEPOSITION;
				m_tmaxFirstPLComparer.SortColumn = CTmaxObjectionGridComparer.SORT_COLUMN_FIRST_PL;
				m_tmaxLastPLComparer.SortColumn = CTmaxObjectionGridComparer.SORT_COLUMN_LAST_PL;
				m_tmaxTextComparer.SortColumn = CTmaxObjectionGridComparer.SORT_COLUMN_TEXT;

				//	Fill the columns collection
				FillColumns();

				//	Retrieve the column properties stored in the file
				SetColumnProps(xmlFile);
				
				//	Create the data source
				CreateDataSource();

				//	Assign the data source to the grid
				if(m_dsSource != null)
					m_ctrlGrid.DataSource = m_dsSource;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);

				//	Clean up
				FreeDataSource();
			}

			return (m_dsSource != null);

		}// public bool Initialize()

		/// <summary>This method is called to add an objection to the grid</summary>
		/// <param name="tmaxObjection">The objection to be added</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxObjection tmaxObjection)
		{
			bool	bSuccessful = false;
			DataRow row = null;

			Debug.Assert(tmaxObjection != null);
			if(tmaxObjection == null) return false;

			Debug.Assert(m_dtObjections != null);
			if(m_dtObjections == null) return false;

			try
			{
				//	Create a new row for this property
				row = m_dtObjections.NewRow();

				//	Set the column values
				SetColumns(row, tmaxObjection);

				//	Add to the table
				m_dtObjections.Rows.Add(row);

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			}
			
			return bSuccessful;

		}// public bool Add(CTmaxObjection tmaxObjection)

		/// <summary>Called to add the collection of objections</summary>
		/// <param name="tmaxObjections">The objections to be added</param>
		/// <param name="bClear">true to clear before adding</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxObjections tmaxObjections, bool bClear)
		{
			bool bSuccessful = true;

			//	Are we supposed to clear the existing rows?
			if(bClear)
				Clear();

			//	Now add the new rows
			if((tmaxObjections != null) && (tmaxObjections.Count > 0))
			{
				//	Add each of the properties
				foreach(CTmaxObjection O in tmaxObjections)
				{
					if(Add(O) == false)
						bSuccessful = false;
				}

			}// if((tmaxObjections != null) && (tmaxObjections.Count > 0))

			return bSuccessful;

		}// public bool Add(CTmaxObjections tmaxObjections, bool bClear)

		/// <summary>This method will delete the row bound to the specified objection</summary>
		/// <param name="tmaxObjection">The objection bound to the row</param>
		/// <returns>the index of the row containing the objection that was deleted</returns>
		public int Delete(CTmaxObjection tmaxObjection)
		{
			UltraGridRow	row = null;
			int				iIndex = -1;

			//	Get the requested row in the data source
			if((row = GetRow(tmaxObjection, null)) != null)
			{
				iIndex = row.Index;
				Delete(row);
			}
			else
			{
				m_tmaxEventSource.FireDiagnostic(this, "Delete", "Unable to locate data row with specified objection");
			}

			return iIndex;

		}// public bool Delete(CTmaxObjection tmaxObjection)

		/// <summary>Called to remove all rows from the list box</summary>
		public void Clear()
		{
			try
			{
				if(m_dsSource != null)
					m_dsSource.Clear();
			}
			catch
			{
			}

		}// public void Clear()

		/// <summary>This method is called to get the objection that is selected in the grid</summary>
		/// <returns>The selected objection, null if none or more than one selected</returns>
		public CTmaxObjection GetSelection()
		{
			CTmaxObjections tmaxSelected = new CTmaxObjections();
			CTmaxObjection	tmaxObjection = null;

			if(GetSelected(tmaxSelected) == 1)
			{
				tmaxObjection = tmaxSelected[0];
			}

			return tmaxObjection;

		}// public CTmaxObjection GetSelection()

		/// <summary>This method is called to get the collection of selected objections</summary>
		/// <param name="tmaxSelected">The collection in which to store the objections</param>
		/// <returns>The total number of selected objections</returns>
		public int GetSelected(CTmaxObjections tmaxSelected)
		{
			return GetSelected(tmaxSelected, null);
		}

		/// <summary>This method is called to get the collection of selected objections</summary>
		/// <returns>The collection of selected objections if any</returns>
		public CTmaxObjections GetSelected()
		{
			CTmaxObjections tmaxSelected = new CTmaxObjections();

			if(GetSelected(tmaxSelected) > 0)
				return tmaxSelected;
			else
				return null;

		}// public CTmaxObjections GetSelected()

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

		/// <summary>This method is called to get the index of the row bound to the specified objection</summary>
		/// <param name="tmaxObjection">The object associated with the row</param>
		///	<returns>The index of the row</returns>
		public int GetIndex(CTmaxObjection tmaxObjection)
		{
			UltraGridCell cell = null;
			
			try
			{
				for(int i = 0; i < m_ctrlGrid.Rows.Count; i++)
				{
					//	Get the cell containing the object reference
					if((cell = GetCell(m_ctrlGrid.Rows[i], OBJECTION_COLUMN_OBJECT)) != null)
					{
						//	Is this the one we're looking for?
						if(ReferenceEquals(cell.Value, tmaxObjection) == true)
							return i;
						
					}

				}// for(int i = 0; i < m_ctrlGrid.Rows.Count; i++)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetIndex", Ex);
			}

			return -1;

		}// public UltraGridRow GetIndex(CTmaxObjection tmaxObjection)

		/// <summary>Called to get the application objection object associated with the specified row in the grid</summary>
		/// <param name="row">The row that contains the objection</param>
		/// <returns>The application objection object</returns>
		public CTmaxObjection GetObjection(UltraGridRow row)
		{
			UltraGridCell	cell = null;
			CTmaxObjection	tmaxObjection = null;

			if((row != null) && (row.Cells != null))
			{
				if((cell = GetCell(row, OBJECTION_COLUMN_OBJECT)) != null)
				{
					try { tmaxObjection = ((CTmaxObjection)(cell.Value)); }
					catch { }

				}// if((cell = GetCell(row, OBJECTION_COLUMN_OBJECT)) != null)

			}// if((row != null) && (row.Cells != null))

			return tmaxObjection;

		}// public CTmaxObjection GetObjection(UltraGridRow row)

		/// <summary>Called to get the application objection object associated with the specified cell</summary>
		/// <param name="cell">The cell in the row bound to the objection</param>
		/// <returns>The application objection object</returns>
		public CTmaxObjection GetObjection(UltraGridCell cell)
		{
			if((cell != null) && (cell.Row != null))
				return GetObjection(cell.Row);
			else
				return null;

		}// public CTmaxObjection GetObjection(UltraGridCell cell)

		/// <summary>Called to get the application objection object associated with the specified row in the grid</summary>
		/// <param name="iIndex">The index of the desired row</param>
		/// <returns>The application objection object</returns>
		public CTmaxObjection GetObjection(int iIndex)
		{
			if((iIndex >= 0) && (iIndex < m_ctrlGrid.Rows.Count))
				return GetObjection(m_ctrlGrid.Rows[iIndex]);
			else
				return null;

		}// public CTmaxObjection GetObjection(int iIndex)

		/// <summary>This method is called to make the objection the selected row</summary>
		/// <param name="tmaxObjection">The interface to the objection being selected</param>
		/// <returns>true if successful</returns>
		public bool SetSelection(CTmaxObjection tmaxObjection)
		{
			UltraGridRow	row = null;
			bool			bSuccessful = false;

			try
			{
				if((row = GetRow(tmaxObjection, null)) != null)
				{
					bSuccessful = SetSelection(row);
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}
			
			return bSuccessful;

		}// public bool SetSelection(CTmaxObjection tmaxObjection)

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

		/// <summary>This method will update the row bound to the specified interface</summary>
		/// <param name="IProperty">The interface bound to the row</param>
		/// <returns>true if successful</returns>
		public bool Update(CTmaxObjection tmaxObjection)
		{
			DataRow			dr = null;
			UltraGridRow	gr = null;
			bool			bSuccessful = true;

			if(m_dtObjections == null) return false;
			if(m_dtObjections.Rows == null) return false;

			//	Does the caller want to update all objections?
			if(tmaxObjection == null)
			{
				foreach(DataRow O in m_dtObjections.Rows)
				{
					if((tmaxObjection = (CTmaxObjection)(O[OBJECTION_COLUMN_OBJECT])) != null)
					{
						if(SetColumns(dr, tmaxObjection) == false)
							bSuccessful = false;

					}// if((tmaxObjection = (CTmaxObjection)(O[OBJECTION_COLUMN_OBJECT])) != null)

				}// foreach(DataRow O in m_dtObjections.Rows)

			}
			else
			{
				if((dr = GetSourceRow(tmaxObjection)) != null)
				{
					if((bSuccessful = SetColumns(dr, tmaxObjection)) == true)
					{
						if((gr = GetRow(tmaxObjection, null)) != null)
							gr.PerformAutoSize();

					}// if((bSuccessful = SetColumns(dr, tmaxObjection)) == true)

				}// if((dr = GetSourceRow(tmaxObjection)) != null)

			}// if(tmaxObjection == null)

			return bSuccessful;

		}// public bool Update(CTmaxObjection tmaxObjection)

		/// <summary>This method is called by the application when it is about to terminate</summary>
		/// <param name="xmlFile">The configuration file containing the configuration</param>
		/// <param name="strParentName">The path to the parent's section in the file</param>
		public void Save(CXmlIni xmlFile)
		{
			UltraGridColumn column = null;
			string			strKey = "";
			
			//	NOTE: We assume the file is positioned on the correct section

			xmlFile.Write(XMLINI_KEY_ASCENDING_DEPOSITIONS, m_bAscendingDepositions);
			xmlFile.Write(XMLINI_KEY_SORT_ASCENDING, m_bSortAscending);
			xmlFile.Write(XMLINI_KEY_SORT_COLUMN, m_iSortColumn);

			//	Store each of the columns to file
			foreach(CTmaxObjectionGridColumn O in m_aColumns)
			{
				if(O.Configurable == true)
				{
					//	Get the column with this name
					if((column = GetColumn(O.Name)) != null)
					{
						//	Update the column properties
						O.Width = column.Width;
						O.Visible = (column.Hidden == false);

						strKey = O.Name.Replace(' ', '_');

						xmlFile.Write(strKey, XMLINI_COLUMN_ATTRIBUTE_WIDTH, O.Width);
						xmlFile.Write(strKey, XMLINI_COLUMN_ATTRIBUTE_VISIBLE, O.Visible);
						xmlFile.Write(strKey, XMLINI_COLUMN_ATTRIBUTE_POSITION, column.Header.VisiblePosition);
					}

				}// if(O.Configurable == true)

			}// foreach(CTmaxObjectionGridColumn O in m_aColumns)

		}// public void Save(CXmlIni xmlFile)

		/// <summary>Called to allow the user to choose the columns to be displayed</summary>
		/// <returns>True if changes were made</returns>
		public bool ChooseColumns()
		{
			CFObjectionGridColumns	wndColumns = null;
			UltraGridColumn			column = null;
			bool					bSuccessful = false;

			try
			{
				wndColumns = new CFObjectionGridColumns();

				//	Initialize the form
				wndColumns.GridCtrl = this;

				//	Open the form
				if(wndColumns.ShowDialog() == DialogResult.OK)
				{
					//	Update the grid if it's loaded
					if(m_dsSource != null)
					{
						//	The only thing that can be changed with the preferences form is the column visibility
						foreach(CTmaxObjectionGridColumn O in m_aColumns)
						{
							if(O.Configurable == true)
							{
								//	Get the grid column
								if((column = GetColumn(O.Name)) != null)
								{
									//	Has the visibility changed?
									if(O.Visible == column.Hidden)
									{
										column.Hidden = !O.Visible;
									}

								}// if((column = GetColumn(O.Name)) != null)

							}// if(O.Configurable == true)

						}// foreach(CTmaxObjectionGridColumn O in m_aColumns)

					}// if(m_dsSource != null)
					
					bSuccessful = true;

				}// if(wndColumns.ShowDialog() == DialogResult.OK)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ChooseColumns", m_tmaxErrorBuilder.Message(ERROR_CHOOSE_COLUMNS_EX), Ex);
			}
			
			return bSuccessful;

		}// public bool ChooseColumns()
		
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
			this.m_ctrlGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.m_ctrlGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
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
			this.m_ctrlGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
			this.m_ctrlGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance10.BackColor = System.Drawing.SystemColors.Window;
			appearance10.BorderColor = System.Drawing.Color.Silver;
			this.m_ctrlGrid.DisplayLayout.Override.RowAppearance = appearance10;
			this.m_ctrlGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
			this.m_ctrlGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
			this.m_ctrlGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.m_ctrlGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.m_ctrlGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.m_ctrlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlGrid.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlGrid.Name = "m_ctrlGrid";
			this.m_ctrlGrid.Size = new System.Drawing.Size(150, 150);
			this.m_ctrlGrid.TabIndex = 0;
			this.m_ctrlGrid.Text = "ObjectionsGrid";
			this.m_ctrlGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.OnGridInitializeRow);
			this.m_ctrlGrid.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.OnGridBeforeSelectChange);
			this.m_ctrlGrid.BeforeSortChange += new Infragistics.Win.UltraWinGrid.BeforeSortChangeEventHandler(this.OnGridBeforeSortChange);
			this.m_ctrlGrid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.OnGridAfterSelectChange);
			this.m_ctrlGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.OnGridInitializeLayout);
			this.m_ctrlGrid.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.OnGridAfterSortChange);
			this.m_ctrlGrid.DoubleClick += new System.EventHandler(this.OnGridDblClick);
			this.m_ctrlGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
			// 
			// CTmaxObjectionGridCtrl
			// 
			this.Controls.Add(this.m_ctrlGrid);
			this.Name = "CTmaxObjectionGridCtrl";
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

		/// <summary>Called to fill the array of column descriptors</summary>
		private void FillColumns()
		{
			Debug.Assert(m_aColumns != null);

			//	Populate the array
			for(int i = 0; i <= m_aColumns.GetUpperBound(0); i++)
				m_aColumns[i] = new CTmaxObjectionGridColumn();
		
			m_aColumns[OBJECTION_COLUMN_SIDE].Name = "Party";
			m_aColumns[OBJECTION_COLUMN_STATE].Name = "Status";
			m_aColumns[OBJECTION_COLUMN_DEPOSITION].Name = "Deposition";
			m_aColumns[OBJECTION_COLUMN_FIRST].Name = "First";
			m_aColumns[OBJECTION_COLUMN_LAST].Name = "Last";
			m_aColumns[OBJECTION_COLUMN_ARGUMENT].Name = "Objection";
			m_aColumns[OBJECTION_COLUMN_RULING].Name = "Ruling";
			m_aColumns[OBJECTION_COLUMN_RULING_TEXT].Name = "Ruling Text";
			m_aColumns[OBJECTION_COLUMN_RESPONSE_1].Name = "Response";
			m_aColumns[OBJECTION_COLUMN_RESPONSE_2].Name = "Response 2";
			m_aColumns[OBJECTION_COLUMN_RESPONSE_3].Name = "Response 3";
			m_aColumns[OBJECTION_COLUMN_WORK_PRODUCT].Name = "Work Product";
			m_aColumns[OBJECTION_COLUMN_COMMENTS].Name = "Comments";
			m_aColumns[OBJECTION_COLUMN_ID].Name = "UniqueId";
			m_aColumns[OBJECTION_COLUMN_CASE_NAME].Name = "Case Name";
			m_aColumns[OBJECTION_COLUMN_CASE_ID].Name = "Case Id";
			m_aColumns[OBJECTION_COLUMN_OBJECT].Name = "Object";

			//	Set these columns to default to invisible
			m_aColumns[OBJECTION_COLUMN_DEPOSITION].Visible = false;
			m_aColumns[OBJECTION_COLUMN_DEPOSITION].Width = 100;
			m_aColumns[OBJECTION_COLUMN_ID].Visible = false;
			m_aColumns[OBJECTION_COLUMN_ID].Width = 100;
			m_aColumns[OBJECTION_COLUMN_CASE_NAME].Visible = false;
			m_aColumns[OBJECTION_COLUMN_CASE_NAME].Width = 100;
			m_aColumns[OBJECTION_COLUMN_CASE_ID].Visible = false;
			m_aColumns[OBJECTION_COLUMN_CASE_ID].Width = 100;

			m_aColumns[OBJECTION_COLUMN_OBJECT].Configurable = false;

		}// private void FillColumns()

		/// <summary>This method is called to set the column properties using the values stored in the configuration file</summary>
		/// <param name="xmlFile">The file containing the column attributes</param>
		/// <returns>true if successful</returns>
		public bool SetColumnProps(CXmlIni xmlFile)
		{
			int		iWidth = 0;
			bool	bSuccessful = false;
			string	strKey = "";

			try
			{
				Debug.Assert(xmlFile != null);
				if(xmlFile == null) return false;

				m_bAscendingDepositions = xmlFile.ReadBool(XMLINI_KEY_ASCENDING_DEPOSITIONS, m_bAscendingDepositions);
				m_bSortAscending = xmlFile.ReadBool(XMLINI_KEY_SORT_ASCENDING, m_bSortAscending);
				m_iSortColumn = xmlFile.ReadInteger(XMLINI_KEY_SORT_COLUMN, m_iSortColumn);
				if((m_iSortColumn < 0) || (m_iSortColumn >= OBJECTION_MAX_COLUMNS))
					m_iSortColumn = OBJECTION_COLUMN_DEPOSITION;

				m_tmaxDepositionComparer.AscendingDepositions = m_bAscendingDepositions;
				m_tmaxTextComparer.AscendingDepositions = m_bAscendingDepositions;
				m_tmaxFirstPLComparer.AscendingDepositions = m_bAscendingDepositions;
				m_tmaxLastPLComparer.AscendingDepositions  = m_bAscendingDepositions;

				foreach(CTmaxObjectionGridColumn O in m_aColumns)
				{
					//	We only store information about user selectable columns
					if(O.Configurable == true)
					{
						strKey = O.Name.Replace(' ', '_');
						
						//	Read the attributes stored in the file
						iWidth = xmlFile.ReadInteger(strKey, XMLINI_COLUMN_ATTRIBUTE_WIDTH, -1);

						if(iWidth > 0)
						{
							O.Width = iWidth;
							O.Visible = xmlFile.ReadBool(strKey, XMLINI_COLUMN_ATTRIBUTE_VISIBLE, O.Visible);
							O.Position = xmlFile.ReadInteger(strKey, XMLINI_COLUMN_ATTRIBUTE_POSITION, -1);
						}

					}// if(O.Configurable == true)

				}// foreach(CTmaxObjectionGridColumn O in m_aColumns)
				
				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetColumnProps", m_tmaxErrorBuilder.Message(ERROR_SET_COLUMN_PROPS_EX, xmlFile.FileSpec), Ex);
			}

			return bSuccessful;

		}// public bool SetColumnProps(CXmlIni xmlFile)

		/// <summary>This method is called to update the columns of the specified data row</summary>
		/// <param name="dr">The data row to be updated</param>
		/// <param name="tmaxObjection">The object bound to the row</param>
		/// <returns>true if successful</returns>
		private bool SetColumns(DataRow dr, CTmaxObjection tmaxObjection)
		{
			bool bSuccessful = false;
			
			Debug.Assert(dr != null);
			if(dr == null) return false;

			Debug.Assert(tmaxObjection != null);
			if(tmaxObjection == null) return false;
			
			Debug.Assert(m_dtObjections != null);
			if(m_dtObjections == null) return false;

			try
			{
				//	Set the column values
				dr[OBJECTION_COLUMN_STATE] = tmaxObjection.State;
				dr[OBJECTION_COLUMN_SIDE] = tmaxObjection.Plaintiff ? "Plaintiff" : "Defendant";
				dr[OBJECTION_COLUMN_DEPOSITION] = tmaxObjection.Deposition;
				dr[OBJECTION_COLUMN_FIRST] = CTmaxToolbox.PLToString(tmaxObjection.FirstPL);
				dr[OBJECTION_COLUMN_LAST] = CTmaxToolbox.PLToString(tmaxObjection.LastPL);
				dr[OBJECTION_COLUMN_ARGUMENT] = tmaxObjection.Argument;
				dr[OBJECTION_COLUMN_RULING] = tmaxObjection.Ruling;
				dr[OBJECTION_COLUMN_RULING_TEXT] = tmaxObjection.RulingText;
				dr[OBJECTION_COLUMN_RESPONSE_1] = tmaxObjection.Response1;
				dr[OBJECTION_COLUMN_RESPONSE_2] = tmaxObjection.Response2;
				dr[OBJECTION_COLUMN_RESPONSE_3] = tmaxObjection.Response3;
				dr[OBJECTION_COLUMN_WORK_PRODUCT] = tmaxObjection.WorkProduct;
				dr[OBJECTION_COLUMN_COMMENTS] = tmaxObjection.Comments;
				dr[OBJECTION_COLUMN_ID] = tmaxObjection.UniqueId;
				dr[OBJECTION_COLUMN_CASE_ID] = tmaxObjection.CaseId;
				dr[OBJECTION_COLUMN_CASE_NAME] = tmaxObjection.CaseName;
				dr[OBJECTION_COLUMN_OBJECT] = tmaxObjection;

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetColumns", m_tmaxErrorBuilder.Message(ERROR_SET_COLUMNS_EX), Ex);
			}
			
			return bSuccessful;

		}// private bool SetColumns(DataRow dr, CTmaxObjection tmaxObjection)

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
		private System.Data.DataRow GetSourceRow(CTmaxObjection tmaxObjection)
		{
			if(m_dtObjections == null) return null;
			if(m_dtObjections.Rows == null) return null;
			if(m_dtObjections.Rows.Count == 0) return null;

			foreach(DataRow O in m_dtObjections.Rows)
			{
				if(ReferenceEquals(O[OBJECTION_COLUMN_OBJECT], tmaxObjection) == true)
					return O;
			}

			return null;

		}// private System.Data.DataRow GetSourceRow(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to get the specified row</summary>
		/// <param name="tmaxObjection">The object associated with the row</param>
		///	<param name="rows">The collection of rows to be searched</param>
		///	<returns>The row object with the specified objection</returns>
		private UltraGridRow GetRow(CTmaxObjection tmaxObjection, RowsCollection rows)
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
					if((cell = GetCell(O, OBJECTION_COLUMN_OBJECT)) != null)
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

		}// private UltraGridRow GetRow(CTmaxObjection tmaxObjection, RowsCollection rows)

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
				
				//	Get the band containing the properties
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

		/// <summary>This method is called to get the specified column</summary>
		/// <param name="iColumn">The index of the desired column</param>
		///	<returns>The column object with the specified index</returns>
		private UltraGridColumn GetColumn(int iColumn)
		{
			if(m_aColumns != null)
				return GetColumn(m_aColumns[iColumn].Name);
			else
				return null;

		}// private UltraGridColumn GetColumn(int iColumn)

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
				m_dtObjections = null;
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
				if(m_dtObjections == null) return null;

				//	Create the source data set
				m_dsSource = new System.Data.DataSet();

				//	Add the data table to the set
				m_dsSource.Tables.Add(m_dtObjections);

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDataSource", m_tmaxErrorBuilder.Message(ERROR_CREATE_DATA_SOURCE_EX), Ex);
				FreeDataSource();
			}

			return m_dsSource;

		}// private System.Data.DataSet CreateDataSource()

		/// <summary>This method will create the table of properties to be included in the data set</summary>
		/// <returns>The new Properties table</returns>
		private System.Data.DataTable CreateDataTable()
		{
			Debug.Assert(m_dtObjections == null);

			try
			{
				//	Create a data table for the objections
				m_dtObjections = new DataTable("Objections");

				//	Add the columns
				//
				//	NOTE:	These MUST be added in the order in which the columns are enumerated
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_SIDE].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_STATE].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_DEPOSITION].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_FIRST].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_LAST].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_ARGUMENT].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_RULING].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_RULING_TEXT].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_RESPONSE_1].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_RESPONSE_2].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_RESPONSE_3].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_WORK_PRODUCT].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_COMMENTS].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_ID].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_CASE_NAME].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_CASE_ID].Name, typeof(string));
				m_dtObjections.Columns.Add(m_aColumns[OBJECTION_COLUMN_OBJECT].Name, typeof(object));

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDataTable", m_tmaxErrorBuilder.Message(ERROR_CREATE_DATA_TABLE_EX), Ex);
			}

			return m_dtObjections;

		}// private System.Data.DataTable CreateDataTable()

		/// <summary>This method handles events fired by the grid when it attempts to initialize its layout</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridInitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraGridColumn column = null;
			//UltraGridBand band = null;

			//this.Columns[0].AutoEdit = false;
			//this.Columns[0].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

			try
			{
				//	Set the column properties
				foreach(CTmaxObjectionGridColumn O in m_aColumns)
				{
					if((column = GetColumn(O.Name)) != null)
					{
						//	Has this column been initialized?
						if(O.Width > 0)
						{
							column.Width = O.Width;
							column.Hidden = ((O.Configurable == false) || (O.Visible == false));
							
							if(O.Position >= 0)
								column.Header.VisiblePosition = O.Position;

						}// if(O.Width > 0)
						else
						{
							//	Make sure non-configurable columns are hidden
							if(O.Configurable == false)
								column.Hidden = true;
						}

					}// if((column = GetColumn(O.Name)) != null)

				}// foreach(CTmaxObjectionGridColumn O in m_aColumns)
				
				m_ctrlGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;

				//	Set the multi-line columns
				if((column = GetColumn(OBJECTION_COLUMN_ARGUMENT)) != null)
					column.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
				if((column = GetColumn(OBJECTION_COLUMN_RULING_TEXT)) != null)
					column.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
				if((column = GetColumn(OBJECTION_COLUMN_RESPONSE_1)) != null)
					column.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
				if((column = GetColumn(OBJECTION_COLUMN_RESPONSE_2)) != null)
					column.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
				if((column = GetColumn(OBJECTION_COLUMN_RESPONSE_3)) != null)
					column.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
				if((column = GetColumn(OBJECTION_COLUMN_WORK_PRODUCT)) != null)
					column.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
				if((column = GetColumn(OBJECTION_COLUMN_COMMENTS)) != null)
					column.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;

				//	Attach the appropriate sort comparer to each column
				if((column = GetColumn(OBJECTION_COLUMN_STATE)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_SIDE)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_FIRST)) != null)
					column.SortComparer = m_tmaxFirstPLComparer;
				if((column = GetColumn(OBJECTION_COLUMN_LAST)) != null)
					column.SortComparer = m_tmaxLastPLComparer;
				if((column = GetColumn(OBJECTION_COLUMN_ARGUMENT)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_RESPONSE_1)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_RESPONSE_2)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_RESPONSE_3)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_WORK_PRODUCT)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_RULING)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_RULING_TEXT)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_COMMENTS)) != null)
					column.SortComparer = m_tmaxTextComparer;
				if((column = GetColumn(OBJECTION_COLUMN_DEPOSITION)) != null)
				{
					column.SortComparer = m_tmaxDepositionComparer;

					//	Always include the deposition column in the sorted collection
					try
					{
						if(m_bAscendingDepositions == false)
							column.SortIndicator = SortIndicator.Descending;
						else
							column.SortIndicator = SortIndicator.Ascending;
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireDiagnostic(this, "OnGridInitializeLayout", Ex);
					}
					
				}// if((column = GetColumn(OBJECTION_COLUMN_DEPOSITION)) != null)
				
				//	Set the active sort column
				if(m_iSortColumn != OBJECTION_COLUMN_DEPOSITION)
				{
					if((column = GetColumn(m_iSortColumn)) != null)
					{
						try
						{
							if(m_bSortAscending == false)
								column.SortIndicator = SortIndicator.Descending;
							else
								column.SortIndicator = SortIndicator.Ascending;
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireDiagnostic(this, "OnGridInitializeLayout", Ex);
						}

					}// if((column = GetColumn(m_iSortColumn)) != null)

				}// if(m_iSortColumn != OBJECTION_COLUMN_DEPOSITION)
				
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
				if(this.SelectionChanging != null)
				{
					if(e.Type == typeof(UltraGridRow))
					{
						//	Notify the owner and give it a chance to cancel the change
						if(SelectionChanging(this) == true)
						{
							//	Cancel the change
							e.Cancel = true;
							
							//	Make the previous selection the active row
							if(m_ctrlGrid.Selected.Rows.Count > 0)
								m_ctrlGrid.ActiveRow = m_ctrlGrid.Selected.Rows[0];
							else
								m_ctrlGrid.ActiveRow = null;

						}// if(SelectionChanging(this) == true)

					}// if(e.Type == typeof(UltraGridRow))

				}// if(this.SelectionChanging != null)
				
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
		private int GetSelected(CTmaxObjections tmaxSelected, RowsCollection rows)
		{
			if(m_ctrlGrid == null) return 0;
			if(m_ctrlGrid.IsDisposed == true) return 0;
			if(m_ctrlGrid.Rows == null) return 0;
			if(m_ctrlGrid.Rows.Count == 0) return 0;
			if(m_dtObjections == null) return 0;
			if(m_dtObjections.Rows == null) return 0;
			if(m_dtObjections.Rows.Count == 0) return 0;

			try
			{
				//	Do we need to allocate a collection?
				if(tmaxSelected == null)
					tmaxSelected = new CTmaxObjections();
				else
					tmaxSelected.Clear();

				//	Should we use the root collection?
				if(rows == null)
					rows = m_ctrlGrid.Rows;

				foreach(UltraGridRow O in rows)
				{
					//	Is this row selected?
					if(IsSelected(O) == true)
						tmaxSelected.Add((CTmaxObjection)(O.Cells[m_aColumns[OBJECTION_COLUMN_OBJECT].Name].Value));

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

		}// private int GetSelected(CTmaxObjections tmaxSelected, RowCollection rows)

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
			if(m_dtObjections == null) return false;
			if(m_dtObjections.Rows == null) return false;
			if(m_dtObjections.Rows.Count == 0) return false;
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

		/// <summary>Called to get the in the specified column position</summary>
		/// <param name="row">The row that contains the cell</param>
		/// <param name="iColumn">The column identifier</param>
		/// <returns>The cell in the requested column</returns>
		public UltraGridCell GetCell(UltraGridRow row, int iColumn)
		{
			UltraGridCell	cell = null;
			string			strName = "";

			//	Get the name of the desired column
			if(m_aColumns != null)
				strName = m_aColumns[iColumn].Name;
			Debug.Assert(strName.Length > 0);
			
			//	Get the cell with the matching name
			if((row != null) && (row.Cells != null))
			{
				cell = row.Cells[strName];

			}

			return cell;

		}// public UltraGridCell GetCell(UltraGridRow row, int iColumn)

		/// <summary>Handles event fired by the grid before the sort action occurs</summary>
		/// <param name="sender">the grid firing the event</param>
		/// <param name="e">the event specific parameters</param>
		private void OnGridBeforeSortChange(object sender, BeforeSortChangeEventArgs e)
		{
			m_tmaxEventSource.FireDiagnostic(this, "OnGridBeforeSortChange", "BeforeSortChange Event");

			//	Cancel this action if the user is pressing the shift or control keys
			if((GetShiftPressed() == true) || (GetControlPressed() == true))
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridBeforeSortChange", "BeforeSortChange Canceled");
				e.Cancel = true;
			}

		}// private void OnGridBeforeSortChange(object sender, BeforeSortChangeEventArgs e)

		/// <summary>Handles event fired by the grid after the sort action occurs</summary>
		/// <param name="sender">the grid firing the event</param>
		/// <param name="e">the event specific parameters</param>
		private void OnGridAfterSortChange(object sender, BandEventArgs e)
		{
			//	Are there any sorted columns?
			if((e.Band.SortedColumns != null) && (e.Band.SortedColumns.Count > 0))
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridAfterSortChange", "AfterSortChange - " + e.Band.SortedColumns.Count.ToString() + " columns");

				m_iSortColumn = e.Band.SortedColumns[0].Index;
				if(e.Band.SortedColumns[0].SortIndicator == SortIndicator.Descending)
					m_bSortAscending = false;
				else
					m_bSortAscending = true;
					
				if(m_iSortColumn == OBJECTION_COLUMN_DEPOSITION)
				{
					m_bAscendingDepositions = m_bSortAscending;
					m_tmaxDepositionComparer.AscendingDepositions = m_bAscendingDepositions;
					m_tmaxTextComparer.AscendingDepositions = m_bAscendingDepositions;
					m_tmaxFirstPLComparer.AscendingDepositions = m_bAscendingDepositions;
					m_tmaxLastPLComparer.AscendingDepositions = m_bAscendingDepositions;
				}

				if(this.SortChanged != null)
				{
					SortChanged(this, System.EventArgs.Empty);

				}// if(this.SortChanged != null)
			}
			else
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridAfterSortChange", "AfterSortChange - SortColumns Empty");

			}// if((e.Band.SortedColumns != null) && (e.Band.SortedColumns.Count > 0))

		}// private void OnGridAfterSortChange(object sender, BandEventArgs e)

		#endregion Private Methods

		#region Properties

		/// <summary>The collection of column descriptors</summary>
		public CTmaxObjectionGridColumn [] Columns
		{
			get { return m_aColumns; }
		}

		#endregion Properties

	}// class CTmaxObjectionGridCtrl

	/// <summary>Objects of this class are used to sort columns in the grid</summary>
	public class CTmaxObjectionGridComparer : IComparer
	{
		#region Constants

		public const int SORT_COLUMN_DEPOSITION = 0;
		public const int SORT_COLUMN_FIRST_PL	= 1;
		public const int SORT_COLUMN_LAST_PL	= 2;
		public const int SORT_COLUMN_TEXT		= 3;
		
		#endregion Constants

		#region Private Members

		/// <summary>Private member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Private member bound to GridCtrl property</summary>
		private CTmaxObjectionGridCtrl m_tmaxGridCtrl = null;
		
		/// <summary>Private member bound to SortColumn property</summary>
		private int m_iSortColumn = SORT_COLUMN_TEXT;

		/// <summary>Private member bound to AscendingDepositions property</summary>
		private bool m_bAscendingDepositions = true;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxObjectionGridComparer()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Objections Grid Comparer";
		}

		/// <summary>Called to get a hash code to represent this object</summary>
		/// <returns>the hash code</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary> This function is called to compare two cells in a column</summary>
		/// <param name="x">The first cell being compared</param>
		/// <param name="y">The second cell being compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		int IComparer.Compare(object x, object y)
		{
			return Compare((UltraGridCell)x, (UltraGridCell)y);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>This function is called to compare two cells in the column</summary>
		/// <param name="Cell1">First cell to be compared</param>
		/// <param name="Cell2">Second cell to be compared</param>
		/// <returns>-1 if Cell1 less than Cell2, 0 if equal, 1 if greater than</returns>
		private int Compare(UltraGridCell Cell1, UltraGridCell Cell2)
		{
			int iReturn = -1;

			try
			{
				//	Check for equality first
				//
				//	NOTE:	.NET raises and exception if we don't return 0 for
				//			equal objects
				if(ReferenceEquals(Cell1, Cell2) == false)
				{
					CTmaxObjection tmaxObjection1 = m_tmaxGridCtrl.GetObjection(Cell1);
					CTmaxObjection tmaxObjection2 = m_tmaxGridCtrl.GetObjection(Cell2);

					//	We want to group by deposition
					if((iReturn = String.Compare(tmaxObjection1.Deposition, tmaxObjection2.Deposition)) == 0)
					{
						if(m_iSortColumn == SORT_COLUMN_FIRST_PL)
						{
							if(tmaxObjection1.FirstPL == tmaxObjection2.FirstPL)
								iReturn = 0;
							else
								iReturn = (tmaxObjection1.FirstPL < tmaxObjection2.FirstPL) ? -1 : 1;
						}
						else if(m_iSortColumn == SORT_COLUMN_LAST_PL)
						{
							if(tmaxObjection1.LastPL == tmaxObjection2.LastPL)
								iReturn = 0;
							else
								iReturn = (tmaxObjection1.LastPL < tmaxObjection2.LastPL) ? -1 : 1;
						}
						else
						{
							//	Perform a standard text comparison
							iReturn = String.Compare(Cell1.Text, Cell2.Text);
						}

					}// if((iReturn = String.Compare(tmaxObjection1.Deposition, tmaxObjection2.Deposition)) == 0)
					else
					{
						if(Cell1.Column.SortIndicator == SortIndicator.Descending)
						{
							if(m_bAscendingDepositions == true)
							{
								m_tmaxEventSource.FireDiagnostic(this, "SORT", "Reverse Return Value");
								iReturn *= -1;
							}
						}
						else
						{
							if(m_bAscendingDepositions == false)
							{
								m_tmaxEventSource.FireDiagnostic(this, "SORT", "Reverse Return Value");
								iReturn *= -1;
							}
						}

					}

				}// if(ReferenceEquals(Cell1, Cell2) == false)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Compare", "Exception:", Ex);
			}

			return iReturn;

		}//	private int Compare(UltraGridCell Cell1, UltraGridCell Cell2)

		#endregion Private Methods

		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>The grid control that owns the comparer</summary>
		public CTmaxObjectionGridCtrl GridCtrl
		{
			get { return m_tmaxGridCtrl; }
			set { m_tmaxGridCtrl = value; }
		}

		/// <summary>Identifies the column to sort on</summary>
		public int SortColumn
		{
			get { return m_iSortColumn; }
			set { m_iSortColumn = value; }
		}

		/// <summary>True if depositions are to be sorted in ascending order</summary>
		public bool AscendingDepositions
		{
			get { return m_bAscendingDepositions; }
			set { m_bAscendingDepositions = value; }
		}

		#endregion Properties

	}// class CTmaxObjectionGridComparer
	
}// namespace FTI.Trialmax.Controls

