using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This control allows the user to set the overrides for the default media paths used by the database</summary>
	public class CTmaxExportColumnsCtrl : FTI.Trialmax.Controls.CTmaxBaseCtrl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_FILL_AVAILABLE_EX	=	ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_FILL_EXPORTED_EX	=	ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_EXCHANGE_EX			=	ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_INSERT_EX			=	ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_REMOVE_EX			=	ERROR_TMAX_BASE_CONTROL_MAX + 5;
	
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>The form's tool tip control</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTips;

		/// <summary>The button used to insert a column before the selected column</summary>
		private System.Windows.Forms.Button m_ctrlInsertBefore;
		
		/// <summary>The button used to insert a column after the selected column</summary>
		private System.Windows.Forms.Button m_ctrlInsertAfter;
		
		/// <summary>The button used to remove a column from the Columns list box</summary>
		private System.Windows.Forms.Button m_ctrlRemove;
		
		/// <summary>The list of available fields</summary>
		private System.Windows.Forms.ListBox m_ctrlAvailable;
		
		/// <summary>Label for the fields list box</summary>
		private System.Windows.Forms.Label m_ctrlAvailableLabel;
		
		/// <summary>The label for the exported list box</summary>
		private System.Windows.Forms.Label m_ctrlExportedLabel;
		
		/// <summary>The list of exported columns</summary>
		private System.Windows.Forms.ListBox m_ctrlExported;

		/// <summary>Pushbutton to remove all entries in Columns list</summary>
		private System.Windows.Forms.Button m_ctrlClearAll;
		
		/// <summary>Pushbutton to move the column selection down in the list</summary>
		private System.Windows.Forms.Button m_ctrlMoveDown;
		
		/// <summary>Pushbutton to move the column selection up in the list</summary>
		private System.Windows.Forms.Button m_ctrlMoveUp;
		
		/// <summary>Local member to store a collection of available fields</summary>
		private CTmaxExportColumns m_tmaxAvailable = new CTmaxExportColumns();

		/// <summary>Local member bound to CaseCodes property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to ExportColumns property</summary>
		private FTI.Shared.Trialmax.CTmaxExportColumns m_tmaxExportColumns = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxExportColumnsCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		
			m_tmaxEventSource.Name = "Export Columns Control";
			
			//	Add the error builder's format strings
			SetErrorStrings();
			
		}// public CTmaxExportColumnsCtrl()


		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		public bool Exchange(bool bSetMembers)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					//	Refresh the exported columns
					if(this.ExportColumns != null)
					{
						this.ExportColumns.Clear();
						foreach(CTmaxExportColumn O in m_ctrlExported.Items)
							this.ExportColumns.Add(O);
					}

				}
				else
				{
					FillAvailable();
					FillExported();
				
				}// if(bSetMembers == true)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
			}
			
			return bSuccessful; 
			
		}// public bool Exchange(bool bSetMembers)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Overloaded member called when the window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Do the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

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
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		override protected void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Let the base class add it's strings first
			base.SetErrorStrings();
			
			//	Now add the strings for this control
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of available columns.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of export columns.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the control's properties: SetMembers = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to insert the selected field.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to remove the selected field.");

		}// override protected void SetErrorStrings()

		/// <summary>Required method for Designer support</summary>
		protected new void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_ctrlToolTips = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlMoveDown = new System.Windows.Forms.Button();
			this.m_ctrlMoveUp = new System.Windows.Forms.Button();
			this.m_ctrlClearAll = new System.Windows.Forms.Button();
			this.m_ctrlRemove = new System.Windows.Forms.Button();
			this.m_ctrlInsertAfter = new System.Windows.Forms.Button();
			this.m_ctrlInsertBefore = new System.Windows.Forms.Button();
			this.m_ctrlExportedLabel = new System.Windows.Forms.Label();
			this.m_ctrlExported = new System.Windows.Forms.ListBox();
			this.m_ctrlAvailableLabel = new System.Windows.Forms.Label();
			this.m_ctrlAvailable = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// m_ctrlMoveDown
			// 
			this.m_ctrlMoveDown.Location = new System.Drawing.Point(216, 136);
			this.m_ctrlMoveDown.Name = "m_ctrlMoveDown";
			this.m_ctrlMoveDown.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlMoveDown.TabIndex = 12;
			this.m_ctrlMoveDown.Text = "Move &Down";
			this.m_ctrlMoveDown.Click += new System.EventHandler(this.OnClickMoveDown);
			// 
			// m_ctrlMoveUp
			// 
			this.m_ctrlMoveUp.Location = new System.Drawing.Point(216, 104);
			this.m_ctrlMoveUp.Name = "m_ctrlMoveUp";
			this.m_ctrlMoveUp.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlMoveUp.TabIndex = 11;
			this.m_ctrlMoveUp.Text = "Move &Up";
			this.m_ctrlMoveUp.Click += new System.EventHandler(this.OnClickMoveUp);
			// 
			// m_ctrlClearAll
			// 
			this.m_ctrlClearAll.Location = new System.Drawing.Point(216, 208);
			this.m_ctrlClearAll.Name = "m_ctrlClearAll";
			this.m_ctrlClearAll.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlClearAll.TabIndex = 15;
			this.m_ctrlClearAll.Text = "&Clear All";
			this.m_ctrlClearAll.Click += new System.EventHandler(this.OnClickClearAll);
			// 
			// m_ctrlRemove
			// 
			this.m_ctrlRemove.Location = new System.Drawing.Point(216, 176);
			this.m_ctrlRemove.Name = "m_ctrlRemove";
			this.m_ctrlRemove.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlRemove.TabIndex = 14;
			this.m_ctrlRemove.Text = "&Remove";
			this.m_ctrlRemove.Click += new System.EventHandler(this.OnClickRemove);
			// 
			// m_ctrlInsertAfter
			// 
			this.m_ctrlInsertAfter.Location = new System.Drawing.Point(216, 64);
			this.m_ctrlInsertAfter.Name = "m_ctrlInsertAfter";
			this.m_ctrlInsertAfter.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlInsertAfter.TabIndex = 10;
			this.m_ctrlInsertAfter.Text = "Insert &After";
			this.m_ctrlInsertAfter.Click += new System.EventHandler(this.OnClickInsert);
			// 
			// m_ctrlInsertBefore
			// 
			this.m_ctrlInsertBefore.Location = new System.Drawing.Point(216, 40);
			this.m_ctrlInsertBefore.Name = "m_ctrlInsertBefore";
			this.m_ctrlInsertBefore.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlInsertBefore.TabIndex = 9;
			this.m_ctrlInsertBefore.Text = "Insert &Before";
			this.m_ctrlInsertBefore.Click += new System.EventHandler(this.OnClickInsert);
			// 
			// m_ctrlExportedLabel
			// 
			this.m_ctrlExportedLabel.Location = new System.Drawing.Point(360, 8);
			this.m_ctrlExportedLabel.Name = "m_ctrlExportedLabel";
			this.m_ctrlExportedLabel.Size = new System.Drawing.Size(144, 16);
			this.m_ctrlExportedLabel.TabIndex = 16;
			this.m_ctrlExportedLabel.Text = "Exported Columns";
			// 
			// m_ctrlExported
			// 
			this.m_ctrlExported.Location = new System.Drawing.Point(360, 24);
			this.m_ctrlExported.Name = "m_ctrlExported";
			this.m_ctrlExported.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.m_ctrlExported.Size = new System.Drawing.Size(168, 212);
			this.m_ctrlExported.TabIndex = 17;
			this.m_ctrlExported.SelectedIndexChanged += new System.EventHandler(this.OnSelIndexChanged);
			// 
			// m_ctrlAvailableLabel
			// 
			this.m_ctrlAvailableLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlAvailableLabel.Name = "m_ctrlAvailableLabel";
			this.m_ctrlAvailableLabel.Size = new System.Drawing.Size(144, 16);
			this.m_ctrlAvailableLabel.TabIndex = 13;
			this.m_ctrlAvailableLabel.Text = "Available Columns";
			// 
			// m_ctrlAvailable
			// 
			this.m_ctrlAvailable.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlAvailable.Name = "m_ctrlAvailable";
			this.m_ctrlAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.m_ctrlAvailable.Size = new System.Drawing.Size(168, 212);
			this.m_ctrlAvailable.TabIndex = 8;
			this.m_ctrlAvailable.SelectedIndexChanged += new System.EventHandler(this.OnSelIndexChanged);
			// 
			// CTmaxExportColumnsCtrl
			// 
			this.Controls.Add(this.m_ctrlMoveDown);
			this.Controls.Add(this.m_ctrlMoveUp);
			this.Controls.Add(this.m_ctrlClearAll);
			this.Controls.Add(this.m_ctrlRemove);
			this.Controls.Add(this.m_ctrlInsertAfter);
			this.Controls.Add(this.m_ctrlInsertBefore);
			this.Controls.Add(this.m_ctrlExportedLabel);
			this.Controls.Add(this.m_ctrlExported);
			this.Controls.Add(this.m_ctrlAvailableLabel);
			this.Controls.Add(this.m_ctrlAvailable);
			this.Name = "CTmaxExportColumnsCtrl";
			this.Size = new System.Drawing.Size(536, 248);
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to populate the list of available columns</summary>
		/// <returns>true if successful</returns>
		private bool FillAvailable()
		{
			bool				bSuccessful = false;
			CTmaxExportColumn	tmaxColumn = null;
			
			try
			{
				//	Start by adding predefined export columns
				Array aPredefined = Enum.GetValues(typeof(TmaxExportColumns));
				foreach(TmaxExportColumns O in aPredefined)
				{
					if(O != TmaxExportColumns.Invalid)
					{
						//	Create a column and add to the list
						tmaxColumn = new CTmaxExportColumn();
						tmaxColumn.CaseCodeId = 0;
						tmaxColumn.TmaxEnumId = O;
						tmaxColumn.Name = O.ToString();
						m_tmaxAvailable.Add(tmaxColumn);
					}
				}
				
				//	Now add columns for each of the case codes
				if(m_tmaxCaseCodes != null)
				{
					foreach(CTmaxCaseCode O in m_tmaxCaseCodes)
					{
						tmaxColumn = new CTmaxExportColumn();
						tmaxColumn.CaseCode = O;
						m_tmaxAvailable.Add(tmaxColumn);	
					}
				
				}// if(m_tmaxCaseCodes != null)
				
				//	Assign this as the data source for the list box
				m_ctrlAvailable.DataSource = m_tmaxAvailable;
			
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillAvailable", m_tmaxErrorBuilder.Message(ERROR_FILL_AVAILABLE_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool FillAvailable()
		
		/// <summary>This method is called to populate the list of exported columns</summary>
		/// <returns>true if successful</returns>
		private bool FillExported()
		{
			bool				bSuccessful = false;
			CTmaxExportColumn	tmaxAvailable = null;
			
			if(this.ExportColumns == null) return false;
			if(this.ExportColumns.Count == 0) return true;
			
			try
			{
				//	Iterate the existing collection of exported columns
				foreach(CTmaxExportColumn O in this.ExportColumns)
				{
					if((tmaxAvailable = GetAvailable(O)) != null)
					{
						m_ctrlExported.Items.Add(tmaxAvailable);
					}
				}
			
				if(m_ctrlExported.Items.Count > 0)
					m_ctrlExported.SelectedIndex = 0;
					
				SetControlStates();

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillExported", m_tmaxErrorBuilder.Message(ERROR_FILL_EXPORTED_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool FillExported()
		
		/// <summary>This method is called to get the column in the Available list that matches the specified exported column</summary>
		/// <param name="tmaxExported">The column to be matched</param>
		/// <returns>the matching column if found</returns>
		private CTmaxExportColumn GetAvailable(CTmaxExportColumn tmaxExported)
		{
			CTmaxExportColumn tmaxAvailable = null;
			
			if(m_tmaxAvailable == null) return null;
			if(m_tmaxAvailable.Count == 0) return null;
			
			foreach(CTmaxExportColumn O in m_tmaxAvailable)
			{
				//	Do the case codes match?
				if(O.CaseCodeId == tmaxExported.CaseCodeId)
				{
					//	If case code is zero the names must match also
					if(O.CaseCodeId == 0)
					{
						if(String.Compare(O.Name, tmaxExported.Name, true) == 0)
							return O;
					}
					else
					{
						return O;
					}
					
				}// if(O.CaseCodeId == tmaxExported.CaseCodeId)
			
			}// foreach(CTmaxExportColumn O in m_tmaxAvailable)
			
			return tmaxAvailable;
			
		}// private CTmaxExportColumn GetAvailable(CTmaxExportColumn tmaxExported)
		
		/// <summary>This method is called to get the current selection in the specified list box</summary>
		/// <param name="ctrlColumns">The list box containing the columns</param>
		/// <returns>The current selection in the list box if only one column is selected</returns>
		private CTmaxExportColumn GetSelection(ListBox ctrlColumns)
		{
			CTmaxExportColumns tmaxSelections = null;
			
			tmaxSelections = GetSelections(ctrlColumns);
			if((tmaxSelections != null) && (tmaxSelections.Count == 1))
				return tmaxSelections[0];
			else
				return null;//	Only allow single select
		
		}// private CTmaxExportColumn GetSelection(ListBox ctrlColumns)
		
		/// <summary>This method is called to get the index of the current selection in the specified list box</summary>
		/// <param name="ctrlColumns">The list box containing the columns</param>
		/// <returns>The index of the current selection if only one column is selected</returns>
		private int GetSelectionIndex(ListBox ctrlColumns)
		{
			CTmaxExportColumn tmaxSelection = null;
			
			if((tmaxSelection = GetSelection(ctrlColumns)) != null)
				return ctrlColumns.Items.IndexOf(tmaxSelection);
			else
				return -1;
		
		}// private CTmaxExportColumn GetSelectionIndex(ListBox ctrlColumns)
		
		/// <summary>This method is called to get the current selectionS in the specified list box</summary>
		/// <param name="ctrlColumns">The list box containing the columns</param>
		/// <returns>The collection of selected columns</returns>
		private CTmaxExportColumns GetSelections(ListBox ctrlColumns)
		{
			CTmaxExportColumns tmaxColumns = null;
			
			try
			{
				if((ctrlColumns.SelectedItems != null) && (ctrlColumns.SelectedItems.Count > 0))
				{
					tmaxColumns = new CTmaxExportColumns();
					
					foreach(CTmaxExportColumn O in ctrlColumns.SelectedItems)
						tmaxColumns.Add(O);
				}
			
			}
			catch
			{
			}
			
			return tmaxColumns;
			
		}// private CTmaxExportColumns GetSelections(ListBox ctrlColumns)
		
		/// <summary>This method inserts the current selection in the Available list box into the Exported list box</summary>
		/// <param name="bAfter">true to insert after the current selection</param>
		/// <returns>true if successful</returns>
		private bool Insert(bool bAfter)
		{
			CTmaxExportColumns	tmaxAvailable = null;
			bool				bSuccessful = false;
			int					iIndex = -1;
			
			//	Get the selection in the list of Available columns
			if((tmaxAvailable = GetSelections(m_ctrlAvailable)) == null)
			{
				MessageBox.Show("You must select a field to be exported from the list of Available fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			
			try
			{
				//	Get the index of the current selection in the export box
				iIndex = GetSelectionIndex(m_ctrlExported);

				//	Add this column if there is no current selection
				if(iIndex < 0)
				{
					foreach(CTmaxExportColumn O in tmaxAvailable)
						m_ctrlExported.Items.Add(O);
				}
				else
				{
					//	Are we adding to the end of the list?
					if((iIndex == (m_ctrlExported.Items.Count - 1)) && (bAfter == true))
					{
						foreach(CTmaxExportColumn O in tmaxAvailable)
							m_ctrlExported.Items.Add(O);
					}
					else
					{
						//	Adjust the index if inserting AFTER
						if(bAfter == true) iIndex++;
					
						foreach(CTmaxExportColumn O in tmaxAvailable)
						{
							m_ctrlExported.Items.Insert(iIndex, O);
							iIndex++;
						}
							
					}
				
				}// if(m_ctrlExported.Items.Count == 0)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Insert", m_tmaxErrorBuilder.Message(ERROR_INSERT_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Insert(bool bAfter)
		
		/// <summary>This method is called when the user clicks on Clear All</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickClearAll(object sender, System.EventArgs e)
		{
			m_ctrlExported.Items.Clear();
			SetControlStates();
		
		}// private void OnClickClearAll(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on one of the Insert buttons</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickInsert(object sender, System.EventArgs e)
		{
			if(ReferenceEquals(sender, m_ctrlInsertBefore) == true)
				Insert(false);
			else
				Insert(true);
		
		}// private void OnClickInsert(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks the Move Up button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickMoveUp(object sender, System.EventArgs e)
		{
			CTmaxExportColumns	tmaxExported = GetSelections(m_ctrlExported);
			
			if(tmaxExported != null)
			{
				//	Get the current index of each object in the collection
				int [] aIndexes = new int[tmaxExported.Count];
				
				for(int i = 0; i < tmaxExported.Count; i++)
					aIndexes[i] = m_ctrlExported.Items.IndexOf(tmaxExported[i]);
						
				for(int i = 0; i < tmaxExported.Count; i++)
				{
					if((tmaxExported[i] != null) && (aIndexes[i] >= 0))
					{
						m_ctrlExported.Items.RemoveAt(aIndexes[i]);
				
						m_ctrlExported.Items.Insert(aIndexes[i] - 1, tmaxExported[i]);
						m_ctrlExported.SelectedIndex = aIndexes[i] - 1;
					}
					
				}
				
			}// if(tmaxExported != null)
			
		}// private void OnClickMoveUp(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks the Move Down button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickMoveDown(object sender, System.EventArgs e)
		{
			CTmaxExportColumns	tmaxExported = GetSelections(m_ctrlExported);
			
			if(tmaxExported != null)
			{
				//	Get the current index of each object in the collection
				int [] aIndexes = new int[tmaxExported.Count];
				
				for(int i = 0; i < tmaxExported.Count; i++)
					aIndexes[i] = m_ctrlExported.Items.IndexOf(tmaxExported[i]);
						
				//	Work in reverse so we don't have to adjust the indexes as we go
				for(int i = tmaxExported.Count - 1; i >= 0; i--)
				{
					if((tmaxExported[i] != null) && (aIndexes[i] >= 0))
					{
						m_ctrlExported.Items.RemoveAt(aIndexes[i]);
				
						if((aIndexes[i] + 1) < m_ctrlExported.Items.Count)
						{
							m_ctrlExported.Items.Insert(aIndexes[i] + 1, tmaxExported[i]);
							m_ctrlExported.SelectedIndex = aIndexes[i] + 1;
						}
						else
						{
							m_ctrlExported.Items.Add(tmaxExported[i]);
							m_ctrlExported.SelectedIndex = m_ctrlExported.Items.Count - 1;
						}
					
					}// if((tmaxExported[i] != null) && (aIndexes[i] >= 0))
					
				}
				
			}// if(tmaxExported != null)

		}// private void OnClickMoveDown(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Remove</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickRemove(object sender, System.EventArgs e)
		{
			int					iIndex = -1;
			CTmaxExportColumns	tmaxRemove = GetSelections(m_ctrlExported);
			
			if(tmaxRemove != null)
			{
				foreach(CTmaxExportColumn O in tmaxRemove)
				{
					//	Get the index of the current selection
					if((iIndex = m_ctrlExported.Items.IndexOf(O)) >= 0)
					{
						m_ctrlExported.Items.RemoveAt(iIndex);
						
						if(m_ctrlExported.Items.Count > 0)
						{
							if(iIndex >= m_ctrlExported.Items.Count)
								iIndex = m_ctrlExported.Items.Count - 1;
								
							m_ctrlExported.SelectedIndex = iIndex;
						}
					}
				
				}// foreach(CTmaxExportColumn O in tmaxRemove)
			
			}// if(tmaxRemove != null)
		
		}// private void OnClickInsert(object sender, System.EventArgs e)

		/// <summary>This method handles events fired when the user changes the selection in one of the list boxes</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnSelIndexChanged(object sender, System.EventArgs e)
		{
			//	Make sure the appropriate controls are enabled / disabled
			SetControlStates();
			
		}// private void OnSelIndexChanged(object sender, System.EventArgs e)

		/// <summary>This member updates the child controls based on the current selection</summary>
		private void SetControlStates()
		{
			CTmaxExportColumns tmaxAvailable = GetSelections(m_ctrlAvailable);
			CTmaxExportColumns tmaxExported = GetSelections(m_ctrlExported);
			
			m_ctrlInsertBefore.Enabled = ((tmaxAvailable != null) && (tmaxExported != null) && (tmaxExported.Count == 1));
			m_ctrlInsertAfter.Enabled = ((tmaxAvailable != null) && ((tmaxExported == null) || (tmaxExported.Count == 1)));
			m_ctrlClearAll.Enabled = (m_ctrlExported.Items.Count > 0);

			if(tmaxExported != null)
			{
				m_ctrlMoveUp.Enabled = (m_ctrlExported.Items.IndexOf(tmaxExported[0]) > 0);
				m_ctrlMoveDown.Enabled = (m_ctrlExported.Items.IndexOf(tmaxExported[tmaxExported.Count - 1]) < (m_ctrlExported.Items.Count - 1));
				
				m_ctrlRemove.Enabled = true;
			}
			else
			{
				m_ctrlMoveUp.Enabled = false;
				m_ctrlMoveDown.Enabled = false;
				m_ctrlRemove.Enabled = false;
			}
			
		}// private void SetControlStates()

		#endregion Private Methods

		#region Properties
		
		/// <summary>The collection of columns to be exported</summary>
		public FTI.Shared.Trialmax.CTmaxExportColumns ExportColumns
		{
			get	{ return m_tmaxExportColumns; }
			set	{ m_tmaxExportColumns = value; }
		}		
		
		/// <summary>The application's collection of case codes</summary>
		public FTI.Shared.Trialmax.CTmaxCaseCodes CaseCodes
		{
			get	{ return m_tmaxCaseCodes; }
			set	{ m_tmaxCaseCodes = value; }
		}		
		
		/// <summary>The total number of columns in the export list</summary>
		public int ExportCount
		{
			get	{ return m_ctrlExported.Items.Count; }
		}		
		
		#endregion Properties
		
	}// public class CTmaxExportColumnsCtrl : CTmaxBaseCtrl

}// namespace FTI.Trialmax.Controls

