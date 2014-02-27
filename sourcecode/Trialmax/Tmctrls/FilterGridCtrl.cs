using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class creates a grid-style control for viewing the terms in a TrialMax database filter</summary>
	public class CTmaxFilterGridCtrl : CTmaxBaseCtrl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_COLUMNS_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_ADD_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_SET_FILTER_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_UPDATE_OPERATOR_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_UPDATE_TERM_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_REMOVE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 6;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		protected System.ComponentModel.IContainer components = null;
		
		/// <summary>Standard ListView control used to display messages</summary>
		private System.Windows.Forms.ListView m_ctrlListView = null;
		
		/// <summary>List view column to display filter logic</summary>
		private System.Windows.Forms.ColumnHeader m_colLogic;
		
		/// <summary>List view column to display a term's field name</summary>
		private System.Windows.Forms.ColumnHeader m_colFieldName;
		
		/// <summary>List view column to display a term's modifier</summary>
		private System.Windows.Forms.ColumnHeader m_colNOT;
		
		/// <summary>List view column to display the term's comparison</summary>
		private System.Windows.Forms.ColumnHeader m_colComparison;
		
		/// <summary>List view column to display the term's value</summary>
		private System.Windows.Forms.ColumnHeader m_colValue;

		/// <summary>Local member to keep track of filter used to initialize the control</summary>
		private CTmaxFilter m_tmaxFilter = null;
		
		/// <summary>Local member to suppress selection change notifications</summary>
		private bool m_bSuppressNotifications = false;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxFilterGridCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();
		}
		
		/// <summary>Called to remove all rows from the list box</summary>
		public void Clear()
		{
			try 
			{ 
				m_ctrlListView.Items.Clear(); 
				m_tmaxFilter = null;
			}
			catch 
			{
			}
			
		}// public void Clear()
		
		/// <summary>This method will set the filter to be displayed in the control</summary>
		/// <param name="tmaxFilter">The database filter to be displayed</param>
		/// <returns>true if successful</returns>
		public bool SetFilter(CTmaxFilter tmaxFilter)
		{
			bool bSuccessful = false;
			
			if(m_ctrlListView == null) return false;
			
			try
			{
				//	Clear the current contents
				Clear();
				
				//	Assign the new filter
				if((m_tmaxFilter = tmaxFilter) == null) return true;
			
				//	Now add each of the code terms
				foreach(CTmaxFilterTerm O in m_tmaxFilter.Terms)
					Add(O, false);
					
				bSuccessful = true;
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetFilter", m_tmaxErrorBuilder.Message(ERROR_SET_FILTER_EX), Ex);
			}
			
			//	Make sure all columns are properly sized
			ResizeColumns();
			
			return bSuccessful;
			
		}// public bool SetFilter(CTmaxFilter tmaxFilter)
		
		/// <summary>This method is called when a term is added to the filter</summary>
		/// <param name="tmaxTerm">The term added to the filter</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxFilterTerm tmaxTerm)
		{
			return Add(tmaxTerm, true);
		}
		
		/// <summary>This method is called when the filter operator changes</summary>
		/// <returns>true if successful</returns>
		public bool UpdateOperator()
		{
			bool bSuccessful = false;
			
			//	Do we have a valid list control and filter?
			if(m_ctrlListView == null) return false;
			if(m_tmaxFilter == null) return false;
		
			try
			{
				SuspendLayout();
				
				for(int i = 1; i < m_ctrlListView.Items.Count; i++)
				{
					m_ctrlListView.Items[i].Text = m_tmaxFilter.Operator.ToString();
				}
				
				m_colLogic.Width = -2;
				
				ResumeLayout();
			
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "UpdateOperator", m_tmaxErrorBuilder.Message(ERROR_UPDATE_OPERATOR_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool UpdateOperator()
		
		/// <summary>This method is called when a term is added to the filter</summary>
		/// <param name="tmaxTerm">The term added to the filter</param>
		/// <returns>true if successful</returns>
		public bool Remove(CTmaxFilterTerm tmaxTerm)
		{
			ListViewItem	lvItem = null;
			bool			bSuccessful = false;
			
			//	Do we have a valid list control and filter?
			if(m_ctrlListView == null) return false;
			if(m_tmaxFilter == null) return false;
		
			try
			{
				//	Get the list view item for this term
				if((lvItem = GetItem(tmaxTerm)) == null)
					return false;
					
				//	Remove this item from the view
				m_ctrlListView.Items.Remove(lvItem);
				
				//	Make sure the Filter logic is cleared out
				//	in the first item
				if(m_ctrlListView.Items.Count > 0)
				{
					m_ctrlListView.Items[0].SubItems[0].Text = "";
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Remove", m_tmaxErrorBuilder.Message(ERROR_REMOVE_EX), Ex);
			}
			
			//	Automatically resize the columns to fit the text
			ResizeColumns();
			
			return bSuccessful;
		
		}// public bool OnAdded(CTmaxFilterTerm tmaxTerm)
		
		/// <summary>This method is called when a term has been updated</summary>
		/// <param name="tmaxTerm">The term being updated</param>
		/// <returns>true if successful</returns>
		public bool Update(CTmaxFilterTerm tmaxTerm)
		{
			ListViewItem	lvItem = null;
			bool			bSuccessful = false;
			
			//	Do we have a valid list control and filter?
			if(m_ctrlListView == null) return false;
			if(m_tmaxFilter == null) return false;
		
			try
			{
				//	Get the item for this term
				if((lvItem = GetItem(tmaxTerm)) == null) 
					return false;
				
				//	Update the column text
				lvItem.SubItems[1].Text = tmaxTerm.Name;
				
				if(tmaxTerm.Modifier != TmaxFilterModifiers.None)
					lvItem.SubItems[2].Text = tmaxTerm.Modifier.ToString();
				else
					lvItem.SubItems[2].Text = "";
					
				if(tmaxTerm.Comparison == TmaxFilterComparisons.AnyValue)
				{
					lvItem.SubItems[3].Text = "";
					lvItem.SubItems[4].Text = "ANY VALUE";
				}
				else
				{
					lvItem.SubItems[3].Text = tmaxTerm.Comparison.ToString();
					lvItem.SubItems[4].Text = tmaxTerm.Value;
				}
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			}
			
			//	Resize the columns
			if(bSuccessful == true)
				ResizeColumns();
			
			return bSuccessful;
		
		}// public bool Update(CTmaxFilterTerm tmaxTerm)
		
		/// <summary>This method is called to get the index of the current selection</summary>
		/// <returns>The index of the current (or first) selection</returns>
		public int GetSelectedIndex()
		{
			int iSelected = -1;
			
			if((m_ctrlListView != null) && (m_ctrlListView.SelectedIndices != null))
			{
				if(m_ctrlListView.SelectedIndices.Count > 0)
					iSelected = m_ctrlListView.SelectedIndices[0];
			}
			
			return iSelected;
				
		}// public int GetSelectedIndex()
		
		/// <summary>This method is called to get the current selection</summary>
		/// <returns>The current selection</returns>
		public CTmaxFilterTerm GetSelected()
		{
			int iSelected = -1;
			
			if((iSelected = GetSelectedIndex()) >= 0)
				return (CTmaxFilterTerm)(m_ctrlListView.Items[iSelected].Tag);
			else
				return null;
				
		}// public CTmaxFilterTerm GetSelected()
		
		/// <summary>This method is called to set the index of the current selection</summary>
		/// <param name="iIndex">The index of the current selection</param>
		/// <param name="bSuppress">true to suppress change notifications</param>
		/// <returns>True if successful</returns>
		public bool SetSelectedIndex(int iIndex, bool bSuppress)
		{
			bool bSuccessful = false;
			
			if(m_ctrlListView == null) return false;
			if(m_ctrlListView.Items == null) return false;
			
			try
			{
				m_bSuppressNotifications = bSuppress;
				
				//	Clear the current selections
				if(m_ctrlListView.SelectedItems != null)
					m_ctrlListView.SelectedItems.Clear();
					
				if((iIndex >= 0) && (iIndex < m_ctrlListView.Items.Count))
				{
					m_ctrlListView.Items[iIndex].Selected = true;
					m_ctrlListView.EnsureVisible(iIndex);
				}
						
				bSuccessful = true;

			}
			catch
			{
			}
			
			m_bSuppressNotifications = false;
			return bSuccessful;
				
		}// public bool SetSelectedIndex(int iIndex)
		
		/// <summary>This method is called to set the current selection</summary>
		/// <param name="tmaxTerm">The object used to create the entry in the list box</param>
		/// <param name="bSuppress">true to suppress change notifications</param>
		/// <returns>True if successful</returns>
		public bool SetSelected(CTmaxFilterTerm tmaxTerm, bool bSuppress)
		{
			bool			bSuccessful = false;
			ListViewItem	lvi = null;
			
			if(m_ctrlListView == null) return false;
			if(m_ctrlListView.Items == null) return false;
			
			try
			{
				m_bSuppressNotifications = bSuppress;
				
				//	Clear the current selections
				if(m_ctrlListView.SelectedItems != null)
					m_ctrlListView.SelectedItems.Clear();
					
				if(tmaxTerm != null)
				{
					if((lvi = GetItem(tmaxTerm)) != null)
					{
						lvi.Selected = true;
						m_ctrlListView.EnsureVisible(lvi.Index);
						bSuccessful = true;
					}
						
				}
				else
				{
					bSuccessful = true;
				}

			}
			catch
			{
			}
			
			m_bSuppressNotifications = false;
			return bSuccessful;
				
		}// public bool SetSelected(CTmaxFilterTerm tmaxTerm)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Initialize the list view columns
			SetColumns();
			
			//	Perform the base class processing first
			base.OnLoad(e);
		}

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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the columns");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding a filter term to the list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding set the active filter.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while updating the filter operator.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while updating the filter term: name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while removing the filter term.");
			
		}// override protected void SetErrorStrings()

		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected override void RecalcLayout()
		{
			//	Resize the columns
			ResizeColumns();
		}
			
		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			this.m_ctrlListView = new System.Windows.Forms.ListView();
			this.m_colLogic = new System.Windows.Forms.ColumnHeader();
			this.m_colFieldName = new System.Windows.Forms.ColumnHeader();
			this.m_colNOT = new System.Windows.Forms.ColumnHeader();
			this.m_colComparison = new System.Windows.Forms.ColumnHeader();
			this.m_colValue = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// m_ctrlListView
			// 
			this.m_ctrlListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.m_colLogic,
																							 this.m_colFieldName,
																							 this.m_colNOT,
																							 this.m_colComparison,
																							 this.m_colValue});
			this.m_ctrlListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlListView.FullRowSelect = true;
			this.m_ctrlListView.HideSelection = false;
			this.m_ctrlListView.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlListView.MultiSelect = false;
			this.m_ctrlListView.Name = "m_ctrlListView";
			this.m_ctrlListView.Size = new System.Drawing.Size(150, 150);
			this.m_ctrlListView.TabIndex = 0;
			this.m_ctrlListView.View = System.Windows.Forms.View.Details;
			this.m_ctrlListView.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
			// 
			// m_colLogic
			// 
			this.m_colLogic.Text = "Logic";
			this.m_colLogic.Width = 38;
			// 
			// m_colFieldName
			// 
			this.m_colFieldName.Text = "Field Name";
			this.m_colFieldName.Width = 65;
			// 
			// m_colNOT
			// 
			this.m_colNOT.Text = "NOT";
			this.m_colNOT.Width = 35;
			// 
			// m_colComparison
			// 
			this.m_colComparison.Text = "Condition";
			this.m_colComparison.Width = 56;
			// 
			// m_colValue
			// 
			this.m_colValue.Text = "Value";
			this.m_colValue.Width = 39;
			// 
			// CTmaxFilterGridCtrl
			// 
			this.Controls.Add(this.m_ctrlListView);
			this.Name = "CTmaxFilterGridCtrl";
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Fired by the control when the user clicks on the Edit button</summary>
		public event System.EventHandler SelectedIndexChanged;	
	
		private void OnSelectedIndexChanged(object sender, System.EventArgs e)
		{
			//	When the user selects a new item, .NET fires a de-select and
			//	then a select event. We want to ignore the de-select
			if(m_ctrlListView.SelectedIndices.Count == 0)
				if(m_ctrlListView.Items.Count > 0) return; // Ignore
				
			//	Bubble the event
			if((m_bSuppressNotifications == false) && (SelectedIndexChanged != null))
				SelectedIndexChanged(this, e);
			
		}// private void OnSelectedIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method is called when a term is added to the filter</summary>
		/// <param name="tmaxTerm">The term added to the filter</param>
		/// <param name="bResize">true to resize the columns</param>
		/// <returns>true if successful</returns>
		private bool Add(CTmaxFilterTerm tmaxTerm, bool bResize)
		{
			ListViewItem	lvItem = null;
			bool			bSuccessful = false;
			
			//	Do we have a valid list control and filter?
			if(m_ctrlListView == null) return false;
			if(m_tmaxFilter == null) return false;
		
			try
			{
				//	Allocate the new item
				lvItem = new ListViewItem();
				lvItem.Tag = tmaxTerm;
				
				//	Set the text for the first column
				if(m_ctrlListView.Items.Count == 0)
					lvItem.Text = "";
				else
					lvItem.Text = m_tmaxFilter.Operator.ToString();
				
				//	Add the field name
				lvItem.SubItems.Add(tmaxTerm.Name);
				
				//	Add the modifier
				if(tmaxTerm.Modifier != TmaxFilterModifiers.None)
					lvItem.SubItems.Add(tmaxTerm.Modifier.ToString());
				else
					lvItem.SubItems.Add("");
					
				//	Add the comparison and value
				if(tmaxTerm.Comparison == TmaxFilterComparisons.AnyValue)
				{			
					lvItem.SubItems.Add("");
					lvItem.SubItems.Add("ANY VALUE");
				}
				else
				{
					lvItem.SubItems.Add(tmaxTerm.Comparison.ToString());
					lvItem.SubItems.Add(tmaxTerm.Value.ToString());
				}
				
				//	Add to the list view		
				m_ctrlListView.Items.Add(lvItem);
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			}
			
			//	Resize the columns
			if((bSuccessful == true) && (bResize == true))
				ResizeColumns();
			
			return bSuccessful;
		
		}// private bool Add(CTmaxFilterTerm tmaxTerm, bool bResize)
		
		/// <summary>This method is called to get the item created from the specified object</summary>
		/// <param name="O">The item used to create the row item</param>
		/// <returns>The associated list view item if found</returns>
		private ListViewItem GetItem(object O)
		{
			//	Do we have a valid list control and item collection?
			if(m_ctrlListView == null) return null;
			if(m_ctrlListView.Items == null) return null;
			if(m_ctrlListView.Items.Count == 0) return null;
						
			foreach(ListViewItem lvi in m_ctrlListView.Items)
			{
				if(lvi.Tag != null)
				{
					if(ReferenceEquals(lvi.Tag, O) == true)
						return lvi;
				}
				
			}
			return null;

		}// private ListViewItem GetItem(object O)
		
		/// <summary>This method sets the columns used to display the filter values</summary>
		/// <returns>true if successful</returns>
		private bool SetColumns()
		{	
			bool bSuccessful = false;
			
			try
			{
				// =============================================================
				//	NOTE:	The columns have been added via the design editor
				// =============================================================
				
				SuspendLayout();
				
				//	Automatically resize the columns to fit the text
				m_colLogic.Width = -2;
				m_colFieldName.Width = -2;
				m_colNOT.Width = -2;
				m_colComparison.Width = -2;
				m_colValue.Width = -2;
			
				//	Clear the header text in the NOT column
				m_colNOT.Text = "";
			
				ResumeLayout();

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetColumns", m_tmaxErrorBuilder.Message(ERROR_SET_COLUMNS_EX), Ex);
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(CTmaxFilterTerm tmaxTerm)
		
		/// <summary>This method will resize the columns</summary>
		private void ResizeColumns()
		{
			try
			{
				SuspendLayout();
				
				//	Automatically resize the adjustable columns to fit the text
				m_colFieldName.Width = -2;
				m_colNOT.Width = -2;
				m_colComparison.Width = -2;
				m_colValue.Width = -2;
			
				ResumeLayout();
			}
			catch
			{
			}
		
		}// public void ResizeColumns()
		
		#endregion Private Methods

		#region Properties

		/// <summary>This property exposes the child list view control</summary>
		public System.Windows.Forms.ListView ListView
		{
			get { return m_ctrlListView; }
		}
		
		/// <summary>The filter being displayed by the control</summary>
		public CTmaxFilter Filter
		{
			get { return m_tmaxFilter; }
			set { SetFilter(value); }
		}
		
		#endregion Properties
		
	}// class CTmaxFilterGridCtrl
	
}// namespace FTI.Trialmax.Controls

