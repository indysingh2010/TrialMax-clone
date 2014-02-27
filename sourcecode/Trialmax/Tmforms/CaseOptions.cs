using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Forms
{
	/// <summary>Form that allows the user to configure the case options</summary>
	public class CFCaseOptions : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_ADD_MAP_EX				= ERROR_TMAX_FORM_MAX + 1;
		private const int ERROR_EDIT_MAP_EX				= ERROR_TMAX_FORM_MAX + 2;
		private const int ERROR_INSERT_AFTER_FIELD_EX	= ERROR_TMAX_FORM_MAX + 3;
		private const int ERROR_EDIT_FIELD_EX			= ERROR_TMAX_FORM_MAX + 4;
		private const int ERROR_EXCHANGE_EX				= ERROR_TMAX_FORM_MAX + 5;
		private const int ERROR_ON_ADDED_EX				= ERROR_TMAX_FORM_MAX + 6;
		private const int ERROR_ON_DELETED_EX			= ERROR_TMAX_FORM_MAX + 7;
		private const int ERROR_ON_UPDATED_EX			= ERROR_TMAX_FORM_MAX + 8;
		private const int ERROR_DELETE_FIELD_EX			= ERROR_TMAX_FORM_MAX + 9;
		private const int ERROR_ON_REORDERED_EX			= ERROR_TMAX_FORM_MAX + 10;
		private const int ERROR_MOVE_FIELD_UP_EX		= ERROR_TMAX_FORM_MAX + 11;
		private const int ERROR_MOVE_FIELD_DOWN_EX		= ERROR_TMAX_FORM_MAX + 12;
		private const int ERROR_INSERT_BEFORE_FIELD_EX	= ERROR_TMAX_FORM_MAX + 13;
		private const int ERROR_DELETE_MAP_EX			= ERROR_TMAX_FORM_MAX + 14;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Local member bound to PathsModified property</summary>
		private bool m_bPathsModified = false;
		
		/// <summary>Local member bound to CodesModified property</summary>
		private bool m_bCodesModified = false;
		
		/// <summary>Local member bound to StationsModified property</summary>
		private bool m_bStationsModified = false;
		
		/// <summary>Local member bound to EnableCodeEditors property</summary>
		private bool m_bEnableCodeEditors = false;
		
		/// <summary>Local member bound to CaseOptions property</summary>
		private CTmaxCaseOptions m_tmaxCaseOptions = null;
		
		/// <summary>Local member bound to CodesManager property</summary>
		private CTmaxCodesManager m_tmaxCodesManager = null;
		
		/// <summary>Local member bound to StationOptions property</summary>
		private CTmaxStationOptions m_tmaxStationOptions = null;
		
		/// <summary>The form's tab control</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabControl m_ultraTabCtrl;
		
		/// <summary>Tab page for shared controls</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage m_ctrlSharedCtrlsPage;
		
		/// <summary>Tab page for case paths control</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlMapsPage;
		
		/// <summary>Tab page for case codes control</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlCaseCodesPage;
		
		/// <summary>Tab page for setting media options</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlMediaPage;

		/// <summary>Property page for managing the application's pick lists</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlPickListPage;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>The tab used to select the drive aliases page</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlAliasesEditorCtrlPage;
		
		/// <summary>Custom TrialMax control for editing aliases</summary>
		private FTI.Trialmax.Controls.CTmaxAliasesCtrl m_ctrlAliasesEditorCtrl;
		
		/// <summary>Custom TrialMax control for viewing/selecting path maps</summary>
		private FTI.Trialmax.Controls.CTmaxPathMapsViewerCtrl m_ctrlPathMapsCtrl;
		
		/// <summary>Custom TrialMax control for setting meta data types</summary>
		private FTI.Trialmax.Controls.CTmaxCaseCodesViewerCtrl m_ctrlCaseCodes;
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Text box to allow user to set the transcript pause threshold</summary>
		private System.Windows.Forms.TextBox m_ctrlPauseThreshold;

		/// <summary>Label for text box to allow user to set the transcript pause threshold</summary>
		private System.Windows.Forms.Label m_ctrlPauseThresholdLabel;

		/// <summary>Label for text box to allow user to set the case name</summary>
		private Label m_ctrlCaseNameLabel;

		/// <summary>Text box to allow user to set the case name</summary>
		private TextBox m_ctrlCaseName;

		/// <summary>Label for text box to display seconds units</summary>
		private Label m_ctrlSecondsLabel;

		/// <summary>Label for text box to allow user to set the short case name</summary>
		private Label m_ctrlShortCaseNameLabel;

		/// <summary>Text box to allow user to set the short case name</summary>
		private TextBox m_ctrlShortCaseName;

		/// <summary>Static text control to display the path to the active case folder</summary>
		private Label m_ctrlCaseFolder;

		/// <summary>Label for the control used to display the active case path</summary>
		private Label m_ctrlCaseFolderLabel;
		
		/// <summary>TrialMax custom control for editing pick lists</summary>
		private FTI.Trialmax.Controls.CTmaxPickListViewerCtrl m_ctrlPickListsEditor;

		/// <summary>Local member bound to CaseName property</summary>
		private string m_strCaseName = "";

		/// <summary>Local member bound to ShortCaseName property</summary>
		private string m_strShortCaseName = "";

		/// <summary>Local member bound to CaseFolder property</summary>
		private string m_strCaseFolder = "";

		#endregion Private Members
		
		#region Public Methods
		
		public CFCaseOptions()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		
			//	Attach to the child control event sources
			m_tmaxEventSource.Attach(m_ctrlPathMapsCtrl.EventSource);
			m_tmaxEventSource.Attach(m_ctrlAliasesEditorCtrl.EventSource);
			m_tmaxEventSource.Attach(m_ctrlCaseCodes.EventSource);
			m_tmaxEventSource.Attach(m_ctrlPickListsEditor.EventSource);
			
		}// public CFCaseOptions()

		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			try
			{
				if((this.CaseCodes != null) && (m_ctrlCaseCodes != null))
					m_ctrlCaseCodes.OnAdded(tmaxParent, tmaxChildren);

				if((this.PickLists != null) && (m_ctrlPickListsEditor != null))
					m_ctrlPickListsEditor.OnAdded(tmaxParent, tmaxChildren);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAdded", m_tmaxErrorBuilder.Message(ERROR_ON_ADDED_EX), Ex);
			}
			
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				if((this.CaseCodes != null) && (m_ctrlCaseCodes != null))
					m_ctrlCaseCodes.OnDeleted(tmaxItems);

				if((this.PickLists != null) && (m_ctrlPickListsEditor != null))
					m_ctrlPickListsEditor.OnDeleted(tmaxItems);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_DELETED_EX), Ex);
			}
			
		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				if((this.CaseCodes != null) && (m_ctrlCaseCodes != null))
					m_ctrlCaseCodes.OnUpdated(tmaxItems);

				if((this.PickLists != null) && (m_ctrlPickListsEditor != null))
					m_ctrlPickListsEditor.OnUpdated(tmaxItems);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnUpdated", m_tmaxErrorBuilder.Message(ERROR_ON_DELETED_EX), Ex);
			}
			
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			try
			{
				//	Have the case codes been reordered?
				if((tmaxItem != null) && (tmaxItem.DataType == TmaxDataTypes.CaseCode))
				{
					if((this.CaseCodes != null) && (m_ctrlCaseCodes != null))
						m_ctrlCaseCodes.OnReordered(tmaxItem);

				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnReordered", m_tmaxErrorBuilder.Message(ERROR_ON_REORDERED_EX), Ex);
			}
			
		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
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
		
		}// protected override void Dispose( bool disposing )
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Clear the modified flags
			m_bPathsModified = false;
			m_bCodesModified = false;
			m_bStationsModified = false;
			
			//	Do we have the application case options?
			if(m_tmaxCaseOptions == null)
			{
				m_ctrlOk.Enabled = false;
				m_ctrlPathMapsCtrl.Enabled = false;
				m_ctrlAliasesEditorCtrl.Enabled = false;
				m_ctrlAliasesEditorCtrlPage.Tab.Visible = false;
			}
			else
			{
				m_ctrlAliasesEditorCtrlPage.Tab.Visible = m_tmaxCaseOptions.AllowEditAliases;
			}
			
			if((m_tmaxCodesManager != null) && (m_tmaxCodesManager.CaseCodes != null))
			{
				m_ctrlCaseCodes.Enabled = true;

				m_ctrlPickListsEditor.Enabled = m_tmaxCodesManager.PickListsEnabled;
				m_ctrlPickListPage.Tab.Enabled = m_tmaxCodesManager.PickListsEnabled;
				m_ctrlPickListsEditor.PaneId = this.PaneId;
			}
			else
			{
				m_ctrlCaseCodes.Enabled = false;
				m_ctrlPickListsEditor.Enabled = false;
				m_ctrlPickListPage.Tab.Enabled = false;
			}
			
			if(m_strCaseFolder.Length > 0)
				m_ctrlCaseFolder.Text = CTmaxToolbox.FitPathToWidth(m_strCaseFolder, m_ctrlCaseFolder);
				
			if(m_tmaxStationOptions != null)
			{
				Exchange(false);
			}
			else
			{
				m_ctrlPauseThreshold.Enabled = false;
				m_ctrlPauseThresholdLabel.Enabled = false;
			}
		
			//	Request application notifications
			SetAppNotifications(true);
			
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called when the form is closing</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			//	Stop application notifications
			m_tmaxCodesManager = null;
			SetAppNotifications(false);
			
			//	Do the base class processing
			base.OnClosing (e);
			
		}// protected override void OnClosing(CancelEventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the new path map.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the path map: Id = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to insert a new data field.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the data field: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the class members with the child controls: SetMembers = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling an application notification that records have been added to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling an application notification that records have been deleted from the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling an application notification that records have been updated in the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete a data field.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to reorder the fields.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to move the data field up: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to move the data field down: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to insert a new data field.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the path map: Id = %1");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Called to assign the active work station options</summary>
		/// <param name="tmaxOptions">The active station options object</param>
		private void SetStationOptions(CTmaxStationOptions tmaxStationOptions)
		{
			m_tmaxStationOptions = tmaxStationOptions;
		}
		
		/// <summary>Called to assign the case options</summary>
		/// <param name="tmaxOptions">The active case options object</param>
		private void SetCaseOptions(CTmaxCaseOptions tmaxCaseOptions)
		{
			m_tmaxCaseOptions = tmaxCaseOptions;

			if(m_ctrlPathMapsCtrl != null)
				m_ctrlPathMapsCtrl.CaseOptions = m_tmaxCaseOptions;

			if(m_ctrlAliasesEditorCtrl != null)
				m_ctrlAliasesEditorCtrl.CaseOptions = m_tmaxCaseOptions;
		
		}// private void SetCaseOptions(CTmaxCaseOptions tmaxCaseOptions)
		
		/// <summary>Called to assign the case code manager</summary>
		/// <param name="tmaxCodesManager">The active case code manager</param>
		private void SetCodesManager(CTmaxCodesManager tmaxCodesManager)
		{
			m_tmaxCodesManager = tmaxCodesManager;

			if(m_ctrlCaseCodes != null)
			{
				m_ctrlCaseCodes.CaseCodes = this.CaseCodes;
				m_ctrlCaseCodes.PickLists = this.PickLists;
			}
			
			if(m_ctrlPickListsEditor != null)
				m_ctrlPickListsEditor.PickLists = this.PickLists;
		
		}// private void SetCodesManager(CTmaxCodesManager tmaxCodesManager)
		
		/// <summary>This method handles the event fired by the path map control when the user clicks on its Edit button</summary>
		/// <param name="sender">The path map control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnEditPathMap(object sender, System.EventArgs e)
		{
			try
			{
				OpenPathMapEditor(false);
			}
			catch(System.Exception Ex)
			{
				if(m_ctrlPathMapsCtrl.PathMap != null)
					m_tmaxEventSource.FireError(this, "OnEditPathMap", m_tmaxErrorBuilder.Message(ERROR_EDIT_MAP_EX, m_ctrlPathMapsCtrl.PathMap.Id.ToString()), Ex);
				else
					m_tmaxEventSource.FireError(this, "OnEditPathMap", m_tmaxErrorBuilder.Message(ERROR_EDIT_MAP_EX, "-1"), Ex);
			}			
		
		}// private void OnEditPathMap(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired by the path map control when the user clicks on its Add button</summary>
		/// <param name="sender">The path map control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnAddPathMap(object sender, System.EventArgs e)
		{
			try
			{
				OpenPathMapEditor(true);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAddPathMap", m_tmaxErrorBuilder.Message(ERROR_ADD_MAP_EX), Ex);
			}			
		
		}// private void OnAddPathMap(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired by the path map control when the user clicks on its Delete button</summary>
		/// <param name="sender">The path map control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnDeletePathMap(object sender, System.EventArgs e)
		{
			CTmaxPathMaps	tmaxPathMaps = null;
			CTmaxPathMap	tmaxPathMap = null;
			ArrayList		tmaxMachines = null;
			string			strMachines = "";
			string			strMsg = "";
			bool			bRemove = true;
			bool			bSave = false;
			
			Debug.Assert(m_tmaxCaseOptions != null, "NULL Case Options");

			//	Refresh the path maps collection and lock them for editing
			if(m_tmaxCaseOptions != null) 
				tmaxPathMaps = m_tmaxCaseOptions.GetPathMaps(true, true);
			if(tmaxPathMaps == null) return;
					
			try
			{
				//	Use the map selected on the path maps page
				//
				//	NOTE:	We use the Id instead of the object itself
				//			because we have refreshed the collection
				if(m_ctrlPathMapsCtrl.PathMap != null)
					tmaxPathMap = tmaxPathMaps.Find(m_ctrlPathMapsCtrl.PathMap.Id);
				
				//	We must have something to delete
				if(tmaxPathMap != null)
				{
					//	Get the list of machines that reference this map
					if((tmaxMachines = m_tmaxCaseOptions.GetTmaxMachines()) != null)
					{
						//	Build the list of machine names
						foreach(CTmaxMachine O in tmaxMachines)
						{
							if(tmaxPathMap.Id == O.PathMap)
								strMachines += (O.Name + "\n");
						}
						
					}// if((tmaxMachines = m_tmaxCaseOptions.GetTmaxMachines()) != null)
					
					//	Should we prompt the user for confirmation?
					if(strMachines.Length > 0)
					{
						strMsg = String.Format("{0} is in use by these machines:\n\n{1}\n\nDo you want to continue ?", tmaxPathMap.Name, strMachines);
						if(MessageBox.Show(strMsg, "Delete ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							bRemove = false;	
					}
					
					//	Should we continue with the removal process?
					if(bRemove == true)
					{
						//	Remove the map
						if((m_tmaxStationOptions != null) && (m_tmaxStationOptions.User.Length > 0))
							tmaxPathMap.DeletedBy = m_tmaxStationOptions.User;
						else
							tmaxPathMap.DeletedBy = "Unknown User";
						
						//	Rebuild the list of maps because we've modified the collection
						m_ctrlPathMapsCtrl.Reload((m_tmaxCaseOptions.PathMap != null) ? m_tmaxCaseOptions.PathMap.Id : -1);
					
						//	Set the flag to indicate that the paths have been modified
						m_bPathsModified = true;
						bSave = true;
						
					}//	if(bRemove == true)
					else
					{
						//	Restore the correct selection
						m_ctrlPathMapsCtrl.Reload((m_tmaxCaseOptions.PathMap != null) ? m_tmaxCaseOptions.PathMap.Id : -1);
					}
					
				}
				else
				{
					MessageBox.Show("Unable to locate the selected path map", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}

			}
			catch(System.Exception Ex)
			{
				if(m_ctrlPathMapsCtrl.PathMap != null)
					m_tmaxEventSource.FireError(this, "OnDeletePathMap", m_tmaxErrorBuilder.Message(ERROR_DELETE_MAP_EX, m_ctrlPathMapsCtrl.PathMap.Id.ToString()), Ex);
				else
					m_tmaxEventSource.FireError(this, "OnDeletePathMap", m_tmaxErrorBuilder.Message(ERROR_DELETE_MAP_EX, "-1"), Ex);
			}			

			//	Unlock the collection
			m_tmaxCaseOptions.ReleasePathMaps(bSave);

		}// private void OnDeletePathMap(object sender, System.EventArgs e)

		/// <summary>This method will invoke the path map editor</summary>
		/// <param name="bAdd">True to add a new map for editing</param>
		///	<returns>True if changes were made and accepted</returns>
		private bool OpenPathMapEditor(bool bAdd)
		{
			CFPathMapEditor	pathMapEditor = null;
			CTmaxPathMaps	tmaxPathMaps = null;
			CTmaxPathMap	tmaxPathMap = null;
			bool			bModified = false;
			
			Debug.Assert(m_tmaxCaseOptions != null, "NULL Case Options");
			if(m_tmaxCaseOptions == null) return false;
			
			//	Refresh the path maps collection and lock them for editing
			if((tmaxPathMaps = m_tmaxCaseOptions.GetPathMaps(true, true)) == null)
				return false;
					
			//	Are we supposed to add a new map?
			if(bAdd == true)
			{
				//	Add a new map to the collection
				if((tmaxPathMap = tmaxPathMaps.Add()) == null)
					MessageBox.Show("Unable to add map for editing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			else
			{
				//	Use the map selected on the path maps page
				//
				//	NOTE:	We use the Id instead of the object itself
				//			because we have refreshed the collection
				if(m_ctrlPathMapsCtrl.PathMap != null)
					tmaxPathMap = tmaxPathMaps.Find(m_ctrlPathMapsCtrl.PathMap.Id);
				
				//	We must have something to edit
				if(tmaxPathMap == null)
					MessageBox.Show("Unable to locate map for editing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					
			}// if(bAdd == true)

			//	Do we have a map to work with?
			if(tmaxPathMap != null)
			{
				//	Create the map editor
				pathMapEditor = new CFPathMapEditor();
				pathMapEditor.CaseFolder  = m_tmaxCaseOptions.CaseFolder;
				pathMapEditor.ShowWarning = (bAdd == false);
				pathMapEditor.PathMaps    = tmaxPathMaps;
				pathMapEditor.PathMap     = tmaxPathMap;
				
				pathMapEditor.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.m_tmaxEventSource.OnError);
				pathMapEditor.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.m_tmaxEventSource.OnDiagnostic);
				
				//	Open the form to allow the user to edit the paths
				if(pathMapEditor.ShowDialog() == DialogResult.OK)
				{
					bModified = true;
				}
				else
				{
					//	If we added a new map remove it from the collection
					if(bAdd == true)
						tmaxPathMaps.Remove(tmaxPathMap);
				}

			}//	while(bComplete == false);
			
			//	Rebuild the list of maps because we've refreshed the collection
			m_ctrlPathMapsCtrl.Reload((tmaxPathMap != null) ? tmaxPathMap.Id : -1);
				
			//	Unlock the collection
			m_tmaxCaseOptions.ReleasePathMaps(bModified);
			
			//	Keep track of whether or not a change was made
			if(bModified == true)
				m_bPathsModified = true;
				
			return bModified;
			
		}// private bool OpenPathMapEditor(bool bEdit)

		/// <summary>This method handles the event fired by the meta fields control when the user clicks on its Edit button</summary>
		/// <param name="sender">The meta fields control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnCaseCodesEdit(object sender, System.EventArgs e)
		{
			try
			{
				OpenCaseCodeEditor(false, false);
			}
			catch(System.Exception Ex)
			{
				if(m_ctrlCaseCodes.CaseCode != null)
					m_tmaxEventSource.FireError(this, "OnCaseCodesEdit", m_tmaxErrorBuilder.Message(ERROR_EDIT_FIELD_EX, m_ctrlCaseCodes.CaseCode.Name), Ex);
				else
					m_tmaxEventSource.FireError(this, "OnCaseCodesEdit", m_tmaxErrorBuilder.Message(ERROR_EDIT_FIELD_EX, "N/A"), Ex);
			}			
		
		}// private void OnCaseCodesEdit(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired by the meta fields control when the user clicks on its Add button</summary>
		/// <param name="sender">The meta fields control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnCaseCodesInsertBefore(object sender, System.EventArgs e)
		{
			try
			{
				OpenCaseCodeEditor(true, true);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCaseCodesInsertBefore", m_tmaxErrorBuilder.Message(ERROR_INSERT_BEFORE_FIELD_EX), Ex);
			}			
		
		}// private void OnCaseCodesInsertBefore(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired by the meta fields control when the user clicks on its Add button</summary>
		/// <param name="sender">The meta fields control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnCaseCodesInsertAfter(object sender, System.EventArgs e)
		{
			try
			{
				OpenCaseCodeEditor(true, false);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCaseCodesInsertAfter", m_tmaxErrorBuilder.Message(ERROR_INSERT_AFTER_FIELD_EX), Ex);
			}			
		
		}// private void OnCaseCodesInsertAfter(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired by the meta fields control when the user clicks on its Delete button</summary>
		/// <param name="sender">The meta fields control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnCaseCodesDelete(object sender, System.EventArgs e)
		{
			try
			{
				if(m_ctrlCaseCodes.CaseCode != null)
					FireCommand(m_ctrlCaseCodes.CaseCode, TmaxCommands.Delete, null);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCaseCodesDelete", m_tmaxErrorBuilder.Message(ERROR_DELETE_FIELD_EX), Ex);
			}			
		
		}// private void OnCaseCodesDelete(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired by the meta fields control when the user clicks on its MoveUp button</summary>
		/// <param name="sender">The meta fields control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnCaseCodesMoveUp(object sender, System.EventArgs e)
		{
			try
			{
				MoveCaseCode(false);
			}
			catch(System.Exception Ex)
			{
				if(m_ctrlCaseCodes.CaseCode != null)
					m_tmaxEventSource.FireError(this, "OnCaseCodesMoveUp", m_tmaxErrorBuilder.Message(ERROR_MOVE_FIELD_UP_EX, m_ctrlCaseCodes.CaseCode.Name), Ex);
				else
					m_tmaxEventSource.FireError(this, "OnCaseCodesMoveUp", m_tmaxErrorBuilder.Message(ERROR_MOVE_FIELD_UP_EX, "N/A"), Ex);
			}			
		
		}// private void OnCaseCodesMoveUp(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired by the meta fields control when the user clicks on its MoveDown button</summary>
		/// <param name="sender">The meta fields control</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnCaseCodesMoveDown(object sender, System.EventArgs e)
		{
			try
			{
				MoveCaseCode(true);
			}
			catch(System.Exception Ex)
			{
				if(m_ctrlCaseCodes.CaseCode != null)
					m_tmaxEventSource.FireError(this, "OnCaseCodesMoveDown", m_tmaxErrorBuilder.Message(ERROR_MOVE_FIELD_DOWN_EX, m_ctrlCaseCodes.CaseCode.Name), Ex);
				else
					m_tmaxEventSource.FireError(this, "OnCaseCodesMoveDown", m_tmaxErrorBuilder.Message(ERROR_MOVE_FIELD_DOWN_EX, "N/A"), Ex);
			}			
		
		}// private void OnCaseCodesMoveDown(object sender, System.EventArgs e)

		/// <summary>This method will invoke the meta field editor</summary>
		/// <param name="bAdd">True to add a new field for editing</param>
		/// <param name="bBefore">True if inserting before the current selection</param>
		///	<returns>True if changes were made and accepted</returns>
		private bool OpenCaseCodeEditor(bool bAdd, bool bBefore)
		{
			CFCaseCodeEditor	editor = null;
			CTmaxCaseCode		tmaxCaseCode = null;
			bool				bModified = false;
			CTmaxParameters		tmaxParameters = null;

			Debug.Assert(m_tmaxCodesManager != null, "NULL Case Codes Manager");
			if(m_tmaxCodesManager == null) return false;
			
			//	Do we have the application's case codes collection?
			if(this.CaseCodes == null) return false;
					
			//	Are we supposed to add a new code?
			if(bAdd == true)
			{
				//	Create a new case code to work with
				tmaxCaseCode = new CTmaxCaseCode();
				tmaxCaseCode.UniqueId = this.CaseCodes.GetNextId();
				tmaxCaseCode.Name = "New Code";
				tmaxCaseCode.Type = TmaxCodeTypes.Text;
				tmaxCaseCode.AllowMultiple = false;
			}
			else
			{
				//	Use the selected code
				//
				//	NOTE:	We use the Id instead of the object itself
				//			because we may have refreshed the collection
				if(m_ctrlCaseCodes.CaseCode != null)
					tmaxCaseCode = this.CaseCodes.Find(m_ctrlCaseCodes.CaseCode.UniqueId);
				
				//	We must have something to edit
				if(tmaxCaseCode == null)
					MessageBox.Show("Unable to locate data field for editing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
						
			}// if(bAdd == true)

			//	Do we have a code to work with?
			if(tmaxCaseCode != null)
			{
				//	Create the editor
				editor = new CFCaseCodeEditor();
				m_tmaxEventSource.Attach(editor.EventSource);
				
				editor.EditMode = (bAdd == false);
				editor.CaseCodes = this.CaseCodes;
				editor.CaseCode = tmaxCaseCode;
				editor.PickLists = this.PickLists;
				editor.PickListsEnabled = m_tmaxCodesManager.PickListsEnabled;
				
				//	Open the form to allow the user to edit the paths
				if(editor.ShowDialog() == DialogResult.OK)
				{
					//	Notify the application
					if(bAdd == true)
					{
						tmaxParameters = new CTmaxParameters();
						tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
						
						FireCommand(tmaxCaseCode, TmaxCommands.Add, tmaxParameters);
					}
					else
					{
						FireCommand(tmaxCaseCode, TmaxCommands.Update, null);
					}
							
					bModified = true;
				
				}// if(editor.ShowDialog() == DialogResult.OK)

			}//	while(bComplete == false);
			
			//	Keep track of whether or not a change was made
			if(bModified == true)
				m_bCodesModified = true;
				
			return bModified;
			
		}// private bool OpenCaseCodeEditor(bool bEdit)

		/// <summary>This method will move the selected code up or down</summary>
		/// <param name="bDown">True to move the selected code down in the list</param>
		///	<returns>True if changes were made and accepted</returns>
		private bool MoveCaseCode(bool bDown)
		{
			CTmaxCaseCode		tmaxCaseCode = null;
			CTmaxItem			tmaxReorder = null;
			CTmaxCommandArgs	Args = null;
			bool				bModified = false;
			int					iIndex = 0;

			Debug.Assert(m_tmaxCodesManager != null, "NULL Case Codes Manager");
			if(m_tmaxCodesManager == null) return false;
			
			//	Do we have the application's case codes collection?
			if(this.CaseCodes == null) return false;
			if(this.CaseCodes.Count < 2) return false;
					
			//	Use the selected code
			//
			//	NOTE:	We use the Id instead of the object itself
			//			because we may have refreshed the collection
			if(m_ctrlCaseCodes.CaseCode != null)
				tmaxCaseCode = this.CaseCodes.Find(m_ctrlCaseCodes.CaseCode.UniqueId);
				
			//	We must have something to move
			if(tmaxCaseCode == null)
				MessageBox.Show("You must select a data code to be moved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

			//	Do we have a code to work with?
			if(tmaxCaseCode != null)
			{
				if((iIndex = this.CaseCodes.IndexOf(tmaxCaseCode)) >= 0)
				{
					tmaxReorder = new CTmaxItem();
					tmaxReorder.DataType = TmaxDataTypes.CaseCode;
					
					for(int i = 0; i < this.CaseCodes.Count; i++)
					{
						if(i != iIndex)
							tmaxReorder.SubItems.Add(new CTmaxItem(this.CaseCodes[i]));
					}
					
					//	Adjust the index if moving down
					if(bDown == true)
						iIndex++;
					else if(iIndex > 0)
						iIndex--;

					if(iIndex >= this.CaseCodes.Count)
						tmaxReorder.SubItems.Add(new CTmaxItem(tmaxCaseCode));
					else
						tmaxReorder.SubItems.Insert(new CTmaxItem(tmaxCaseCode), iIndex);
					
					//	Fire the command to reorder the codes
					if((Args = FireCommand(TmaxCommands.Reorder, tmaxReorder)) != null)
						bModified = Args.Successful;
					
				}// if((iIndex = this.CaseCodes.IndexOf(tmaxCaseCode)) >= 0)
				
			}//	if(tmaxCaseCode != null)
			
			//	Keep track of whether or not a change was made
			if(bModified == true)
				m_bCodesModified = true;
				
			return bModified;
			
		}// private bool MoveCaseCode(bool bDown)

		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			int		iPauseThreshold = 0;
			bool	bSuccessful = false;
			
			Debug.Assert(m_tmaxStationOptions != null);
			if(m_tmaxStationOptions == null) return false;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					m_strCaseName = m_ctrlCaseName.Text;
					m_strShortCaseName = m_ctrlShortCaseName.Text;
					
					if(m_ctrlPauseThreshold.Text.Length > 0)
					{
						try { iPauseThreshold = System.Convert.ToInt32(m_ctrlPauseThreshold.Text); }
						catch {}
					}
					if(iPauseThreshold < 0) iPauseThreshold = 0;
					
					//	Has the value changed?
					if(iPauseThreshold != m_tmaxStationOptions.PauseThreshold)
					{
						m_tmaxStationOptions.PauseThreshold = iPauseThreshold;
						m_bStationsModified = true;
					}

				}
				else
				{
					m_ctrlCaseName.Text = m_strCaseName;
					m_ctrlShortCaseName.Text = m_strShortCaseName;
					m_ctrlPauseThreshold.Text = m_tmaxStationOptions.PauseThreshold.ToString();
						
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
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFCaseOptions));
			this.m_ctrlMediaPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlSecondsLabel = new System.Windows.Forms.Label();
			this.m_ctrlShortCaseNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlShortCaseName = new System.Windows.Forms.TextBox();
			this.m_ctrlCaseNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlCaseName = new System.Windows.Forms.TextBox();
			this.m_ctrlPauseThreshold = new System.Windows.Forms.TextBox();
			this.m_ctrlPauseThresholdLabel = new System.Windows.Forms.Label();
			this.m_ctrlMapsPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlPathMapsCtrl = new FTI.Trialmax.Controls.CTmaxPathMapsViewerCtrl();
			this.m_ctrlAliasesEditorCtrlPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlAliasesEditorCtrl = new FTI.Trialmax.Controls.CTmaxAliasesCtrl();
			this.m_ctrlCaseCodesPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlCaseCodes = new FTI.Trialmax.Controls.CTmaxCaseCodesViewerCtrl();
			this.m_ctrlPickListPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlPickListsEditor = new FTI.Trialmax.Controls.CTmaxPickListViewerCtrl();
			this.m_ultraTabCtrl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.m_ctrlSharedCtrlsPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlCaseFolderLabel = new System.Windows.Forms.Label();
			this.m_ctrlCaseFolder = new System.Windows.Forms.Label();
			this.m_ctrlMediaPage.SuspendLayout();
			this.m_ctrlMapsPage.SuspendLayout();
			this.m_ctrlAliasesEditorCtrlPage.SuspendLayout();
			this.m_ctrlCaseCodesPage.SuspendLayout();
			this.m_ctrlPickListPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ultraTabCtrl)).BeginInit();
			this.m_ultraTabCtrl.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlMediaPage
			// 
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlCaseFolder);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlCaseFolderLabel);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlSecondsLabel);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlShortCaseNameLabel);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlShortCaseName);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlCaseNameLabel);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlCaseName);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlPauseThreshold);
			this.m_ctrlMediaPage.Controls.Add(this.m_ctrlPauseThresholdLabel);
			this.m_ctrlMediaPage.Location = new System.Drawing.Point(2, 24);
			this.m_ctrlMediaPage.Name = "m_ctrlMediaPage";
			this.m_ctrlMediaPage.Size = new System.Drawing.Size(428, 234);
			// 
			// m_ctrlSecondsLabel
			// 
			this.m_ctrlSecondsLabel.Location = new System.Drawing.Point(176, 120);
			this.m_ctrlSecondsLabel.Name = "m_ctrlSecondsLabel";
			this.m_ctrlSecondsLabel.Size = new System.Drawing.Size(100, 20);
			this.m_ctrlSecondsLabel.TabIndex = 6;
			this.m_ctrlSecondsLabel.Text = "(sec)";
			this.m_ctrlSecondsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlShortCaseNameLabel
			// 
			this.m_ctrlShortCaseNameLabel.Location = new System.Drawing.Point(7, 66);
			this.m_ctrlShortCaseNameLabel.Name = "m_ctrlShortCaseNameLabel";
			this.m_ctrlShortCaseNameLabel.Size = new System.Drawing.Size(100, 20);
			this.m_ctrlShortCaseNameLabel.TabIndex = 5;
			this.m_ctrlShortCaseNameLabel.Text = "Short Case Name";
			this.m_ctrlShortCaseNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlShortCaseName
			// 
			this.m_ctrlShortCaseName.Location = new System.Drawing.Point(113, 66);
			this.m_ctrlShortCaseName.Name = "m_ctrlShortCaseName";
			this.m_ctrlShortCaseName.Size = new System.Drawing.Size(170, 20);
			this.m_ctrlShortCaseName.TabIndex = 1;
			// 
			// m_ctrlCaseNameLabel
			// 
			this.m_ctrlCaseNameLabel.Location = new System.Drawing.Point(7, 39);
			this.m_ctrlCaseNameLabel.Name = "m_ctrlCaseNameLabel";
			this.m_ctrlCaseNameLabel.Size = new System.Drawing.Size(100, 20);
			this.m_ctrlCaseNameLabel.TabIndex = 3;
			this.m_ctrlCaseNameLabel.Text = "Case Name";
			this.m_ctrlCaseNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCaseName
			// 
			this.m_ctrlCaseName.Location = new System.Drawing.Point(113, 39);
			this.m_ctrlCaseName.Name = "m_ctrlCaseName";
			this.m_ctrlCaseName.Size = new System.Drawing.Size(304, 20);
			this.m_ctrlCaseName.TabIndex = 0;
			// 
			// m_ctrlPauseThreshold
			// 
			this.m_ctrlPauseThreshold.Location = new System.Drawing.Point(114, 120);
			this.m_ctrlPauseThreshold.Name = "m_ctrlPauseThreshold";
			this.m_ctrlPauseThreshold.Size = new System.Drawing.Size(56, 20);
			this.m_ctrlPauseThreshold.TabIndex = 2;
			// 
			// m_ctrlPauseThresholdLabel
			// 
			this.m_ctrlPauseThresholdLabel.Location = new System.Drawing.Point(8, 120);
			this.m_ctrlPauseThresholdLabel.Name = "m_ctrlPauseThresholdLabel";
			this.m_ctrlPauseThresholdLabel.Size = new System.Drawing.Size(100, 32);
			this.m_ctrlPauseThresholdLabel.TabIndex = 0;
			this.m_ctrlPauseThresholdLabel.Text = "Transcript Pause Indicators";
			// 
			// m_ctrlMapsPage
			// 
			this.m_ctrlMapsPage.Controls.Add(this.m_ctrlPathMapsCtrl);
			this.m_ctrlMapsPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlMapsPage.Name = "m_ctrlMapsPage";
			this.m_ctrlMapsPage.Size = new System.Drawing.Size(428, 234);
			// 
			// m_ctrlPathMapsCtrl
			// 
			this.m_ctrlPathMapsCtrl.CaseOptions = null;
			this.m_ctrlPathMapsCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPathMapsCtrl.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlPathMapsCtrl.Name = "m_ctrlPathMapsCtrl";
			this.m_ctrlPathMapsCtrl.Size = new System.Drawing.Size(428, 234);
			this.m_ctrlPathMapsCtrl.TabIndex = 0;
			this.m_ctrlPathMapsCtrl.ClickEdit += new System.EventHandler(this.OnEditPathMap);
			this.m_ctrlPathMapsCtrl.ClickDelete += new System.EventHandler(this.OnDeletePathMap);
			this.m_ctrlPathMapsCtrl.ClickAdd += new System.EventHandler(this.OnAddPathMap);
			// 
			// m_ctrlAliasesEditorCtrlPage
			// 
			this.m_ctrlAliasesEditorCtrlPage.Controls.Add(this.m_ctrlAliasesEditorCtrl);
			this.m_ctrlAliasesEditorCtrlPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlAliasesEditorCtrlPage.Name = "m_ctrlAliasesEditorCtrlPage";
			this.m_ctrlAliasesEditorCtrlPage.Size = new System.Drawing.Size(428, 234);
			// 
			// m_ctrlAliasesEditorCtrl
			// 
			this.m_ctrlAliasesEditorCtrl.CaseOptions = null;
			this.m_ctrlAliasesEditorCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlAliasesEditorCtrl.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlAliasesEditorCtrl.Name = "m_ctrlAliasesEditorCtrl";
			this.m_ctrlAliasesEditorCtrl.Size = new System.Drawing.Size(428, 234);
			this.m_ctrlAliasesEditorCtrl.TabIndex = 0;
			// 
			// m_ctrlCaseCodesPage
			// 
			this.m_ctrlCaseCodesPage.Controls.Add(this.m_ctrlCaseCodes);
			this.m_ctrlCaseCodesPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlCaseCodesPage.Name = "m_ctrlCaseCodesPage";
			this.m_ctrlCaseCodesPage.Size = new System.Drawing.Size(428, 234);
			// 
			// m_ctrlCaseCodes
			// 
			this.m_ctrlCaseCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseCodes.CaseCodes = null;
			this.m_ctrlCaseCodes.Location = new System.Drawing.Point(0, 4);
			this.m_ctrlCaseCodes.Name = "m_ctrlCaseCodes";
			this.m_ctrlCaseCodes.PaneId = 0;
			this.m_ctrlCaseCodes.PickLists = null;
			this.m_ctrlCaseCodes.Size = new System.Drawing.Size(428, 234);
			this.m_ctrlCaseCodes.TabIndex = 0;
			this.m_ctrlCaseCodes.ClickInsertBefore += new System.EventHandler(this.OnCaseCodesInsertBefore);
			this.m_ctrlCaseCodes.ClickMoveDown += new System.EventHandler(this.OnCaseCodesMoveDown);
			this.m_ctrlCaseCodes.ClickMoveUp += new System.EventHandler(this.OnCaseCodesMoveUp);
			this.m_ctrlCaseCodes.ClickDelete += new System.EventHandler(this.OnCaseCodesDelete);
			this.m_ctrlCaseCodes.ClickInsertAfter += new System.EventHandler(this.OnCaseCodesInsertAfter);
			this.m_ctrlCaseCodes.ClickEdit += new System.EventHandler(this.OnCaseCodesEdit);
			// 
			// m_ctrlPickListPage
			// 
			this.m_ctrlPickListPage.Controls.Add(this.m_ctrlPickListsEditor);
			this.m_ctrlPickListPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlPickListPage.Name = "m_ctrlPickListPage";
			this.m_ctrlPickListPage.Size = new System.Drawing.Size(428, 234);
			// 
			// m_ctrlPickListsEditor
			// 
			this.m_ctrlPickListsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPickListsEditor.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlPickListsEditor.Name = "m_ctrlPickListsEditor";
			this.m_ctrlPickListsEditor.PaneId = 0;
			this.m_ctrlPickListsEditor.PickLists = null;
			this.m_ctrlPickListsEditor.Size = new System.Drawing.Size(420, 228);
			this.m_ctrlPickListsEditor.TabIndex = 0;
			this.m_ctrlPickListsEditor.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
			// 
			// m_ultraTabCtrl
			// 
			this.m_ultraTabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ultraTabCtrl.Controls.Add(this.m_ctrlSharedCtrlsPage);
			this.m_ultraTabCtrl.Controls.Add(this.m_ctrlMapsPage);
			this.m_ultraTabCtrl.Controls.Add(this.m_ctrlAliasesEditorCtrlPage);
			this.m_ultraTabCtrl.Controls.Add(this.m_ctrlCaseCodesPage);
			this.m_ultraTabCtrl.Controls.Add(this.m_ctrlMediaPage);
			this.m_ultraTabCtrl.Controls.Add(this.m_ctrlPickListPage);
			this.m_ultraTabCtrl.Location = new System.Drawing.Point(8, 8);
			this.m_ultraTabCtrl.Name = "m_ultraTabCtrl";
			this.m_ultraTabCtrl.SharedControlsPage = this.m_ctrlSharedCtrlsPage;
			this.m_ultraTabCtrl.Size = new System.Drawing.Size(432, 260);
			this.m_ultraTabCtrl.TabIndex = 0;
			ultraTab1.Key = "General";
			ultraTab1.TabPage = this.m_ctrlMediaPage;
			ultraTab1.Text = "General";
			ultraTab1.ToolTipText = "Set General Options";
			ultraTab2.TabPage = this.m_ctrlMapsPage;
			ultraTab2.Text = "Path Maps";
			ultraTab2.ToolTipText = "Set Path Map";
			ultraTab3.TabPage = this.m_ctrlAliasesEditorCtrlPage;
			ultraTab3.Text = "Aliases";
			ultraTab3.ToolTipText = "Set Drive Aliases";
			ultraTab4.TabPage = this.m_ctrlCaseCodesPage;
			ultraTab4.Text = "Data Fields";
			ultraTab4.ToolTipText = "Manage Data Fields";
			ultraTab5.TabPage = this.m_ctrlPickListPage;
			ultraTab5.Text = "Pick Lists";
			ultraTab5.ToolTipText = "Manage Pick Lists";
			this.m_ultraTabCtrl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3,
            ultraTab4,
            ultraTab5});
			// 
			// m_ctrlSharedCtrlsPage
			// 
			this.m_ctrlSharedCtrlsPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlSharedCtrlsPage.Name = "m_ctrlSharedCtrlsPage";
			this.m_ctrlSharedCtrlsPage.Size = new System.Drawing.Size(428, 234);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(336, 276);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlOk.TabIndex = 9;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlCaseFolderLabel
			// 
			this.m_ctrlCaseFolderLabel.Location = new System.Drawing.Point(7, 12);
			this.m_ctrlCaseFolderLabel.Name = "m_ctrlCaseFolderLabel";
			this.m_ctrlCaseFolderLabel.Size = new System.Drawing.Size(100, 20);
			this.m_ctrlCaseFolderLabel.TabIndex = 7;
			this.m_ctrlCaseFolderLabel.Text = "Case Folder";
			this.m_ctrlCaseFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCaseFolder
			// 
			this.m_ctrlCaseFolder.Location = new System.Drawing.Point(113, 12);
			this.m_ctrlCaseFolder.Name = "m_ctrlCaseFolder";
			this.m_ctrlCaseFolder.Size = new System.Drawing.Size(304, 20);
			this.m_ctrlCaseFolder.TabIndex = 8;
			this.m_ctrlCaseFolder.Text = "c:\\program files\\fti\\trialmax 7\\cases\\moore";
			this.m_ctrlCaseFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFCaseOptions
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 305);
			this.Controls.Add(this.m_ctrlOk);
			this.Controls.Add(this.m_ultraTabCtrl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFCaseOptions";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Case Options";
			this.m_ctrlMediaPage.ResumeLayout(false);
			this.m_ctrlMediaPage.PerformLayout();
			this.m_ctrlMapsPage.ResumeLayout(false);
			this.m_ctrlAliasesEditorCtrlPage.ResumeLayout(false);
			this.m_ctrlCaseCodesPage.ResumeLayout(false);
			this.m_ctrlPickListPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ultraTabCtrl)).EndInit();
			this.m_ultraTabCtrl.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			Debug.Assert(m_ctrlPathMapsCtrl != null);
			if(m_ctrlPathMapsCtrl == null) return;
			Debug.Assert(m_ctrlPathMapsCtrl.IsDisposed == false);
			if(m_ctrlPathMapsCtrl.IsDisposed == true) return;
			Debug.Assert(m_ctrlAliasesEditorCtrl != null);
			if(m_ctrlAliasesEditorCtrl == null) return;
			Debug.Assert(m_ctrlAliasesEditorCtrl.IsDisposed == false);
			if(m_ctrlAliasesEditorCtrl.IsDisposed == true) return;
			
			if(m_tmaxCaseOptions != null)
			{
				//	Apply the changes
				if(m_ctrlPathMapsCtrl.Apply() == false) return;
				
				if(m_tmaxCaseOptions.AllowEditAliases == true)
				{
					if(m_ctrlAliasesEditorCtrl.Apply() == false) return;
					
					if(m_ctrlAliasesEditorCtrl.Modified == true)
						m_bPathsModified = true;
				}
				
				//	Do we need to save the changes?
				//
				//	NOTE:	The aliases page has already saved it's changes if necessary
				if(m_ctrlPathMapsCtrl.Modified == true)
				{
					m_tmaxCaseOptions.Save(false, false);
					m_bPathsModified = true;
				}
					
			}// if(m_tmaxCaseOptions != null)
			
			if(m_tmaxCodesManager != null)
			{
				//	Apply the changes
				if(m_ctrlCaseCodes.Apply() == false) return;
			
				//	Do we need to save the changes?
				if(m_ctrlCaseCodes.Modified == true)
				{
					//	The changes have already been saved
					m_bCodesModified = true;
				}
					
			}// if(m_tmaxCodesManager != null)
			
			if(m_tmaxStationOptions != null)
				Exchange(true);
				
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method handles command events fired by one of the child controls</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">The event arguments</param>
		private void OnTmaxCommand(object objSender, FTI.Shared.Trialmax.CTmaxCommandArgs Args)
		{
			//	Propagate the event
			FireCommand(Args);
		}

		/// <summary>This method will fire the application command for the specified case code</summary>
		/// <param name="tmaxCaseCode">The case code for the event</param>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <param name="tmaxParameters">Optional parameters collection to be provided with the event</param>
		///	<returns>True if successful</returns>
		private bool FireCommand(CTmaxCaseCode tmaxCaseCode, TmaxCommands eCommand, CTmaxParameters tmaxParameters)
		{
			CTmaxItem			tmaxItem = null;
			CTmaxCommandArgs	Args = null;
			CTmaxCaseCode		tmaxSelection = null;

			try
			{
				switch(eCommand)
				{
					case TmaxCommands.Add:
					
						tmaxItem = new CTmaxItem();
						tmaxItem.DataType = TmaxDataTypes.CaseCode;
						
						if(tmaxItem.SourceItems == null)
							tmaxItem.SourceItems = new CTmaxItems();
							
						//	Get the current selection in the list box
						if((tmaxSelection = m_ctrlCaseCodes.GetSelection()) != null)
							tmaxItem.SubItems.Add(new CTmaxItem(tmaxSelection));
							
						tmaxItem.SourceItems.Add(new CTmaxItem(tmaxCaseCode));
						break;
					
					case TmaxCommands.Update:
					
						tmaxItem = new CTmaxItem();
						tmaxItem.DataType = TmaxDataTypes.CaseCode;
						tmaxItem.CaseCode = tmaxCaseCode;
						break;
					
					case TmaxCommands.Delete:
					
						tmaxItem = new CTmaxItem();
						tmaxItem.DataType = TmaxDataTypes.CaseCode;
						tmaxItem.SubItems.Add(new CTmaxItem(tmaxCaseCode));
						break;
					
				}// switch(eCommand)
				
				if(tmaxItem != null)
					Args = FireCommand(eCommand, tmaxItem, tmaxParameters);				
			}
			catch
			{
			}
			
			return (Args != null ? Args.Successful : false);
			
		}// private bool FireCommand(CTmaxCaseCode tmaxCaseCode, TmaxCommands eCommand)

		#endregion Private Methods

		#region Properties

		/// <summary>The path to the folder containing the active case</summary>
		public string CaseFolder
		{
			get { return m_strCaseFolder; }
			set { m_strCaseFolder = value; }
		}

		/// <summary>The name assigned to the active case</summary>
		public string CaseName
		{
			get { return m_strCaseName; }
			set { m_strCaseName = value; } 
		}

		/// <summary>The short name assigned to the active case</summary>
		public string ShortCaseName
		{
			get { return m_strShortCaseName; }
			set { m_strShortCaseName = value; }
		}

		/// <summary>The active set of case options</summary>
		public CTmaxCaseOptions CaseOptions
		{
			get { return m_tmaxCaseOptions; }
			set { SetCaseOptions(value); }
		}

		/// <summary>The active case codes manager</summary>
		public FTI.Shared.Trialmax.CTmaxCodesManager CodesManager
		{
			get { return m_tmaxCodesManager; }
			set { SetCodesManager(value); }
		}
		
		/// <summary>The active set of work station options</summary>
		public FTI.Shared.Trialmax.CTmaxStationOptions StationOptions
		{
			get { return m_tmaxStationOptions; }
			set { SetStationOptions(value); }
		}
		
		/// <summary>True if the user has modified one or more case paths or the active path selection</summary>
		public bool PathsModified
		{
			get { return m_bPathsModified; }
		}
		
		/// <summary>True if the user has modified one or more case codes</summary>
		public bool CodesModified
		{
			get { return m_bCodesModified; }
		}
		
		/// <summary>True if the user has modified one or more station options</summary>
		public bool StationsModified
		{
			get { return m_bStationsModified; }
		}
		
		/// <summary>True to enable editing of case codes and pick lists</summary>
		public bool EnableCodeEditors
		{
			get { return m_bEnableCodeEditors; }
			set { m_bEnableCodeEditors = value; }
		}
		
		/// <summary>Private property to provide smart access to the application's CaseCodes collection</summary>
		private CTmaxCaseCodes CaseCodes
		{
			get { return (m_tmaxCodesManager != null ? m_tmaxCodesManager.CaseCodes : null); }
		}
		
		/// <summary>Private property to provide smart access to the application's PickLists collection</summary>
		private CTmaxPickItem PickLists
		{
			get { return (m_tmaxCodesManager != null ? m_tmaxCodesManager.PickLists : null); }
		}
		
		#endregion Properties
	
	}// public class CFCaseOptions : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
