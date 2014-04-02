using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Controls;
using FTI.Shared.Win32;

namespace FTI.Trialmax.Panes
{
	/// <summary>This class creates and manages the TrialMax properties viewer pane</summary>
	public class CPropertiesPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants
		
		protected const int ERROR_INITIALIZE_GRID_EX		= (ERROR_BASE_PANE_MAX + 1);
		protected const int ERROR_ON_BEFORE_GRID_UPDATE_EX	= (ERROR_BASE_PANE_MAX + 2);
		protected const int ERROR_ON_CLICK_EDITOR_BUTTON_EX = (ERROR_BASE_PANE_MAX + 3);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Local member collection to store the properties being displayed</summary>
		private CTmaxProperties m_tmaxProperties = new CTmaxProperties();
		
		/// <summary>Interface to the record that owns the current set of properties</summary>
		private ITmaxMediaRecord m_tmaxRecord = null;
		
		/// <summary>Interface to the record that is to be activated when the pane gets activated</summary>
		private ITmaxMediaRecord m_tmaxActivate = null;

		/// <summary>The property being updated</summary>
		private CTmaxProperty m_tmaxUpdating = null;

		/// <summary>The child property grid control</summary>
		private FTI.Trialmax.Controls.CTmaxPropGridCtrl m_ctrlGrid;

		#endregion Private Members
		
		#region Public Members
		
		//	Default constructor
		public CPropertiesPane() : base()
		{
			//	Initialize the child windows
			InitializeComponent();
			
			m_tmaxEventSource.Attach(m_ctrlGrid.EventSource);
			m_tmaxEventSource.Name = "Properties Pane Events";
			
		}// public CPropertiesPane()
		
		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Clear out the existing information
			m_tmaxActivate = null;
			SetRecord(null);
		}
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	Has the item to be activated been deleted?
			if((m_tmaxActivate != null) && (m_tmaxDatabase.IsValidRecord((CDxMediaRecord)m_tmaxActivate) == false))
				m_tmaxActivate = null;
				
			//	Nothing else to do if not displaying anything
			if(m_tmaxRecord == null) return;
			
			//	Has the active record been deleted?
			if(m_tmaxDatabase.IsValidRecord((CDxMediaRecord)m_tmaxRecord) == false)
			{
				SetRecord(null);
			}
			else
			{
				//	Check each parent and update the properties if we are
				//	displaying the parent record
				foreach(CTmaxItem tmaxItem in tmaxItems)
				{
					//	Don't bother with pick list notifications
					if(tmaxItem.DataType == TmaxDataTypes.PickItem)
						break;
						
					//	Are we displaying the parent?
					if(Update(tmaxItem) == true) break;
				}
				
			} // if(m_tmaxDatabase.Validate(m_tmaxRecord) == false)
			
		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			//	Ignore case codes
			if(tmaxItem.DataType == TmaxDataTypes.CaseCode) return;
			
			//	Nothing else to do if not displaying anything
			if(m_tmaxRecord == null) return;
			
			Debug.Assert(tmaxItem != null);
			if(tmaxItem == null) return;
			
			//	Check this item first
			if(Update(tmaxItem) == false)
			{
				//	Check each of the children
				if(tmaxItem.SubItems != null)
				{
					foreach(CTmaxItem O in tmaxItem.SubItems)
					{
						if(Update(O) == true)
							break;
					}
					
				}
			
			}// if(Update(tmaxItem) == false)
		
		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to notify the panes to refresh their text</summary>
		public override void RefreshText()
		{
			if(m_tmaxRecord != null)
				Update(m_tmaxRecord);
		}
		
		/// <summary>This method is called by the application when a new search result has been activated</summary>
		/// <param name="tmaxResult">The search result to be activated</param>
		public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{
			CTmaxItem tmaxItem = null;
			
			//	Don't bother if not active
			if(PaneVisible == false) return;
			
			//	Is this result associated with a record?
			if(tmaxResult.IScene != null)
				tmaxItem = new CTmaxItem(tmaxResult.IScene);
			else if(tmaxResult.IDeposition != null)
				tmaxItem = new CTmaxItem(tmaxResult.IDeposition);
				
			//	Should we activate the record
			if(tmaxItem != null)
			{
				Activate(tmaxItem, TmaxAppPanes.Results);
			}
		
		}// public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			//	Nothing else to do if not displaying anything
			if(m_tmaxRecord == null) return;
			
			//	Update the values if we are displaying the parent
			if(Update(tmaxParent) == false)
			{
				//	We may be displaying a sibling
				Update(m_tmaxRecord);
			}

		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			//	Are we updating from this pane?
			if(m_tmaxUpdating != null)
			{
				//	Don't worry about doing anything. We'll keep the
				//	grid in sync in the OnGridAfterUpdate handler
				m_tmaxUpdating = null;
			
			}
			else if(tmaxItem != null)
			{
				//	Check this item
				if(Update(tmaxItem) == true) return;
				
				//	Check the children
				if(tmaxItem.SubItems != null)
				{
					foreach(CTmaxItem O in tmaxItem.SubItems)
					{
						if(Update(O) == true) return;
					}
					
				}
				
			}// if((m_bUpdating == false) && (tmaxItem != null))
		
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			if(tmaxItem.IBinderEntry != null)
				m_tmaxActivate = tmaxItem.IBinderEntry;
			else
				m_tmaxActivate = tmaxItem.GetMediaRecord();

			if((m_bPaneVisible == true) && (ReferenceEquals(m_tmaxRecord, m_tmaxActivate) == false))
			{
				SetRecord(m_tmaxActivate);
			}

			return true;
						
		}// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

		/// <summary>This method is called by the application prior to closing the active database</summary>
		/// <returns>True if OK to close the database</returns>
		public override bool CanCloseDatabase()
		{
			try
			{
				//	Make sure all changes have been committed
				if(m_tmaxRecord != null)
					m_ctrlGrid.EndUserUpdate(false);
			}
			catch
			{
			}

			return true;

		}// public override bool CanCloseDatabase()

		/// <summary>This method is called by the application when it is about to terminate</summary>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Clear the property grid
			m_ctrlGrid.EndUserUpdate(true);
			m_ctrlGrid.Clear(true);
			
			//	Do the base class cleanup
			base.Terminate(xmlINI);
		}
		
		/// <summary>This method is called by the application to open the specified item</summary>
		/// <param name="tmaxItem">The item to be opened</param>
		///	<param name="ePane">The pane making the request</param>
		/// <returns>true if successful</returns>
		public override bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			return Activate(tmaxItem, ePane);
						
		}// public override bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		#endregion Public Members
		
		#region Protected Methods
		
		/// <summary>Required method for Designer support</summary>
		protected override void InitializeComponent()
		{
			this.m_ctrlGrid = new FTI.Trialmax.Controls.CTmaxPropGridCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlGrid
			// 
			this.m_ctrlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlGrid.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlGrid.Name = "m_ctrlGrid";
			this.m_ctrlGrid.TabIndex = 0;
			this.m_ctrlGrid.ClickEditorButton += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnClickEditorButton);
			this.m_ctrlGrid.AfterUpdate += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridAfterUpdate);
			this.m_ctrlGrid.BeforeUpdate += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridBeforeUpdate);
			// 
			// CPropertiesPane
			// 
			this.Controls.Add(this.m_ctrlGrid);
			this.Name = "CPropertiesPane";
			this.ResumeLayout(false);

		}// protected override void InitializeComponent()

		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			if(m_bPaneVisible == true)
			{
				if(ReferenceEquals(m_tmaxRecord, m_tmaxActivate) == false)
				{
					SetRecord(m_tmaxActivate);
				}
				
			}

		}// protected override void OnPaneVisibleChanged()
		
		/// <summary>This method is called to display the properties for the specified record</summary>
		/// <param name="tmaxRecord">The record interface used to populate the grid</param>
		protected void SetRecord(ITmaxMediaRecord tmaxRecord)
		{
			//	Clear the current collection
			m_tmaxProperties.Clear();
				
			//	Get the properties associated with this record
			if(tmaxRecord != null)
				tmaxRecord.GetProperties(m_tmaxProperties);
			
			//	Refresh the grid control
			m_ctrlGrid.Add(m_tmaxProperties, true);
			
			//	Update the local member
			m_tmaxRecord = tmaxRecord;
		
		}// protected void SetRecord(ITmaxMediaRecord tmaxRecord)
		
		/// <summary>This method is called when one or more properties associated with the specified item changes</summary>
		/// <param name="tmaxItem">TrialMax event item that identifies the record</param>
		///	<returns>true if this record is associated with the active item</returns>
		protected bool Update(CTmaxItem tmaxItem)
		{
			if(tmaxItem.GetMediaRecord() != null)
				return Update(tmaxItem.GetMediaRecord());
			else if(tmaxItem.IBinderEntry != null)
				return Update(tmaxItem.IBinderEntry);
			else
				return false;
			
		}// protected bool Update(CTmaxItem tmaxItem)
		
		/// <summary>This method is called when one or more properties associated with the specified item changes</summary>
		/// <param name="IRecord">Interface that identifies the record</param>
		///	<returns>true if this record is associated with the active item</returns>
		protected bool Update(ITmaxMediaRecord IRecord)
		{
			if((m_tmaxRecord == null) || (IRecord == null)) 
				return false;
			
			//	Is this related to the active record?
			if(m_tmaxRecord.GetRelationship(IRecord) != TmaxMediaRelationships.None)
			{
				//	Is the pane active?
				if(m_bPaneVisible == true)
				{
					//	Refresh the property bag
					((CDxMediaRecord)m_tmaxRecord).RefreshProperties(m_tmaxProperties);

					//	Update all the properties
					m_ctrlGrid.Update(null);
				}
				else
				{
					//	Force an update when the pane goes active
					if(ReferenceEquals(m_tmaxRecord, m_tmaxActivate) == true)
					{
						m_tmaxRecord = null;
					}
				
				}// if(m_bActive == true)
				
				return true;
			}
			else // if(ReferenceEquals(m_tmaxRecord, IRecord) == true)
			{
				return false;
			}
			
		}// protected bool Update(CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application after the database processes a Move command</summary>
		/// <param name="tmaxMoved">Event item that identifies the records that have been moved</param>
		public override void OnMoved(CTmaxItem tmaxMoved)
		{
			//	Just assume we need an update if displaying a binder
			if((m_tmaxRecord != null) && (m_tmaxRecord.GetDataType() == TmaxDataTypes.Binder))
			{
				Update(m_tmaxRecord);
			}
		
		}// public override void OnMoved(CTmaxItem tmaxMoved)
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			base.SetErrorStrings();
			
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the property grid control.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to change the value for the %1 property to %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when the user clicked on the custom property editor button.");
			
		}// protected override void SetErrorStrings()

		/// <summary>This method is called when the window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Initialize the property grid
			InitializeGridCtrl();
			
			// Perform the base class processing
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method handles events fired by the grid when it has updated a code's value</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnGridAfterUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			try
			{
				CTmaxProperty tmaxProperty = null;
				
				//	Make sure we have the property interface
				Debug.Assert(e.IProperty != null);
				if((tmaxProperty = (CTmaxProperty)(e.IProperty)) == null) return;
				
				//	This keeps the grid in sync just in case the value
				//	was not accepted when the Update command was fired
				if(m_tmaxRecord != null)
				{
					((CDxMediaRecord)m_tmaxRecord).RefreshProperty(tmaxProperty);
					m_ctrlGrid.Update(tmaxProperty);
				}
				
			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridAfterUpdate", "Exception");
			}
			
		}// private void OnGridAfterUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		
		/// <summary>This method handles events fired by the grid before it updates a code's value</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnGridBeforeUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			CTmaxSetPropertyArgs	Args = null;
			string					strMsg = "";
			CTmaxItem				tmaxItem = null;
			CTmaxProperty			tmaxProperty = null;
			
			try
			{
				//	Must have an active record
				if(m_tmaxRecord == null) return;
				
				//	Make sure we have the property interface
				Debug.Assert(e.IProperty != null);
				if((tmaxProperty = (CTmaxProperty)(e.IProperty)) == null) return;
				
				Args = new CTmaxSetPropertyArgs(tmaxProperty, e.Value);
				Args.Confirmed = false;

				if(m_tmaxRecord.SetProperty(Args) == true)
				{
					//	Set up the event item
					//
					//	NOTE:	This could be a binder entry
					if(m_tmaxRecord.GetMediaType() == TmaxMediaTypes.Unknown)
					{
						if(((CDxBinderEntry)m_tmaxRecord).Source != null)
						{
							tmaxItem = new CTmaxItem(((CDxBinderEntry)m_tmaxRecord).Source);
						}
						else
						{
							tmaxItem = new CTmaxItem();
							tmaxItem.IBinderEntry = m_tmaxRecord;
						}

					}
					else
					{
		
						tmaxItem = new CTmaxItem(m_tmaxRecord);
					}
					
					//	Prevent processing of OnUpdated notifications
					m_tmaxUpdating = tmaxProperty;
					
					//	Request the update
					FireCommand(TmaxCommands.Update, tmaxItem);
					
					//	Allow the change
					e.Cancel = false;
				}
				else
				{
					//	Cancel the change
					e.Cancel = true;
				}
				
				if(Args.Message.Length > 0)
				{
					strMsg = "Unable to set the new value for ";
					strMsg += tmaxProperty.Name;
					strMsg += ":\n\n";
					strMsg += Args.Message;
					
					FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONEXCLAMATION);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnGridBeforeUpdate", m_tmaxErrorBuilder.Message(ERROR_ON_BEFORE_GRID_UPDATE_EX, tmaxProperty.Name, e.Value), Ex);
			}
			finally
			{			
				//	Make sure to clear this member
				m_tmaxUpdating = null;
			}
			
		}// private void OnGridBeforeUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		
		/// <summary>This method is called to initialize the grid control</summary>
		/// <returns>true if successful</returns>
		bool InitializeGridCtrl()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Attach the event handler
				m_tmaxEventSource.Attach(m_ctrlGrid.EventSource);
				
				//	Populate a collecion of property categories
				ArrayList aCategories = new System.Collections.ArrayList();
				aCategories.Add(TmaxPropertyCategories.Media);
				aCategories.Add(TmaxPropertyCategories.Database);
				
				//	Initialize the control
				bSuccessful = m_ctrlGrid.Initialize(aCategories);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeGridCtrl", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_GRID_EX), Ex);
			}
			
			return bSuccessful;
			
		}// bool InitializeGridCtrl()
		
		/// <summary>This method handles events fired by the grid before it updates a code's value</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnClickEditorButton(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			CTmaxProperty tmaxProperty = null;
			
			try
			{
				//	Must have an active record
				if(m_tmaxRecord == null) return;
				
				//	Make sure we have the property interface
				Debug.Assert(e.IProperty != null);
				if((tmaxProperty = (CTmaxProperty)(e.IProperty)) == null) return;
				
				//	The only custom editor we have right now is the path editor
				CFPathEditor pathEditor = new CFPathEditor();
					
				pathEditor.Database = m_tmaxDatabase;
				pathEditor.Record = (CDxMediaRecord)m_tmaxRecord;
				pathEditor.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
				m_tmaxEventSource.Attach(pathEditor.EventSource);
					
				pathEditor.ShowDialog(this);

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickEditorButton", m_tmaxErrorBuilder.Message(ERROR_ON_CLICK_EDITOR_BUTTON_EX), Ex);
			}
			
		}// private void OnClickEditorButton(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)

		#endregion Private Methods

	}// public class CPropertiesPane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes

