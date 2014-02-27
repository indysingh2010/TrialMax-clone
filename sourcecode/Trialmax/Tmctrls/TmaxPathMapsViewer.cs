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
	public class CTmaxPathMapsViewerCtrl : System.Windows.Forms.UserControl
	{
		#region Constants
		
		private const int ERROR_FILL_MAPS_EX		= 0;
		private const int ERROR_FILL_OVERRIDES_EX	= 1;
		private const int ERROR_EDIT_EX				= 2;
		private const int ERROR_ADD_EX				= 3;
		private const int ERROR_MAP_CHANGED_EX		= 4;
		private const int ERROR_APPLY_EX			= 5;
		private const int ERROR_ADD_OVERRIDE_EX		= 6;
		private const int ERROR_DELETE_EX			= 7;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Button to request addition of new path map</summary>
		private System.Windows.Forms.Button m_ctrlAdd;
		
		/// <summary>Button to request editing of existing map</summary>
		private System.Windows.Forms.Button m_ctrlEdit;
		
		/// <summary>Button to request removal of selected map</summary>
		private System.Windows.Forms.Button m_ctrlDelete;

		/// <summary>List box to display available path maps</summary>
		private System.Windows.Forms.ListBox m_ctrlPathMaps;

		/// <summary>Local member bound to CaseOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseOptions m_tmaxCaseOptions = null;

		/// <summary>Group box for path maps list box</summary>
		private System.Windows.Forms.GroupBox m_ctrlMapsGroup;

		/// <summary>Group box for path overrides list box</summary>
		private System.Windows.Forms.GroupBox m_ctrlPathsGroup;

		/// <summary>List box for paths associated with current map</summary>
		private System.Windows.Forms.ListView m_ctrlPathOverrides;

		/// <summary>Local member bound to Modified property</summary>
		private bool m_bModified = false;

		/// <summary>The collection of available path maps</summary>
		private FTI.Shared.Trialmax.CTmaxPathMaps m_tmaxPathMaps = null;

		/// <summary>The image list assigned to the path overrides list view control</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;

		/// <summary>The path overrides list view image column</summary>
		private System.Windows.Forms.ColumnHeader m_lvcImage;

		/// <summary>The path overrides list view media type column</summary>
		private System.Windows.Forms.ColumnHeader m_lvcMedia;

		/// <summary>The path overrides list view override column</summary>
		private System.Windows.Forms.ColumnHeader m_lvcOverride;

		/// <summary>The current map selection</summary>
		private FTI.Shared.Trialmax.CTmaxPathMap m_tmaxPathMap = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Fired by the control when the user clicks on the Edit button</summary>
		public event System.EventHandler ClickEdit;	
	
		/// <summary>Fired by the control when the user clicks on the Add button</summary>
		public event System.EventHandler ClickAdd;	
	
		/// <summary>Fired by the control when the user clicks on the Delete button</summary>
		public event System.EventHandler ClickDelete;	
	
		/// <summary>Constructor</summary>
		public CTmaxPathMapsViewerCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		
			//	Add the error builder's format strings
			SetErrorStrings();
			
		}// public CTmaxPathMapsViewerCtrl()

		/// <summary>This method is called to apply the user defined values</summary>
		/// <returns>true if changes have been made</returns>
		public bool Apply()
		{
			try
			{
				//	Has the map selection changed?
				if(m_tmaxPathMap != null)
				{
					if(m_tmaxCaseOptions.PathMap != null)
						m_bModified = (ReferenceEquals(m_tmaxPathMap, m_tmaxCaseOptions.PathMap) == false);
					else
						m_bModified = true;				
				}
				
				//	Update the map if modified
				if(m_bModified == true)
				{
					//	Leave it up to the host form to control whether or not the file gets saved
					m_tmaxCaseOptions.SetPathMap(m_tmaxPathMap.Id, false, true);
				}
				
				return true;
			
			}
			catch(System.Exception Ex)
			{
				
				m_tmaxEventSource.FireError(this, "Apply", m_tmaxErrorBuilder.Message(ERROR_APPLY_EX), Ex);
			
				return true; // Don't prevent the form from closing
			}
		
		}// public bool Apply()
		
		/// <summary>This method refreshes the list and sets the selection to the desired map</summary>
		/// <param name="iSelection">The id of the map to be selected</param>
		///	<returns>True if successful</returns>
		public bool Reload(int iSelection)
		{
			if(m_tmaxPathMaps == null) return false;
			
			//	Do we want to use the caller's selection or our current selection?
			if((iSelection <= 0) && (m_tmaxPathMap != null))
				iSelection = m_tmaxPathMap.Id;
				
			//	Rebuild the list of maps
			FillPathMaps();
				
			//	Set the selection
			SetMapSelection(iSelection);
			
			return (m_tmaxPathMap != null);
			
		}// public bool Reload(int iSelection)
		
		/// <summary>This method is called by the parent form when the user cancels</summary>
		/// <returns>true if ok to close the form</returns>
		public bool Cancel()
		{
		
			return true;

		}// private bool Cancel()

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
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxPathMapsViewerCtrl));
			this.m_ctrlAdd = new System.Windows.Forms.Button();
			this.m_ctrlEdit = new System.Windows.Forms.Button();
			this.m_ctrlPathMaps = new System.Windows.Forms.ListBox();
			this.m_ctrlMapsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlPathsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlPathOverrides = new System.Windows.Forms.ListView();
			this.m_lvcImage = new System.Windows.Forms.ColumnHeader();
			this.m_lvcMedia = new System.Windows.Forms.ColumnHeader();
			this.m_lvcOverride = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlDelete = new System.Windows.Forms.Button();
			this.m_ctrlMapsGroup.SuspendLayout();
			this.m_ctrlPathsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlAdd
			// 
			this.m_ctrlAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlAdd.Location = new System.Drawing.Point(4, 156);
			this.m_ctrlAdd.Name = "m_ctrlAdd";
			this.m_ctrlAdd.Size = new System.Drawing.Size(64, 23);
			this.m_ctrlAdd.TabIndex = 1;
			this.m_ctrlAdd.Text = "&Add";
			this.m_ctrlAdd.Click += new System.EventHandler(this.OnClickAdd);
			// 
			// m_ctrlEdit
			// 
			this.m_ctrlEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlEdit.Location = new System.Drawing.Point(76, 156);
			this.m_ctrlEdit.Name = "m_ctrlEdit";
			this.m_ctrlEdit.Size = new System.Drawing.Size(64, 23);
			this.m_ctrlEdit.TabIndex = 2;
			this.m_ctrlEdit.Text = "&Edit";
			this.m_ctrlEdit.Click += new System.EventHandler(this.OnClickEdit);
			// 
			// m_ctrlPathMaps
			// 
			this.m_ctrlPathMaps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlPathMaps.DisplayMember = "Name";
			this.m_ctrlPathMaps.IntegralHeight = false;
			this.m_ctrlPathMaps.Location = new System.Drawing.Point(8, 20);
			this.m_ctrlPathMaps.Name = "m_ctrlPathMaps";
			this.m_ctrlPathMaps.Size = new System.Drawing.Size(192, 120);
			this.m_ctrlPathMaps.TabIndex = 2;
			this.m_ctrlPathMaps.DoubleClick += new System.EventHandler(this.OnDblClickMap);
			this.m_ctrlPathMaps.SelectedIndexChanged += new System.EventHandler(this.OnMapSelChanged);
			// 
			// m_ctrlMapsGroup
			// 
			this.m_ctrlMapsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlMapsGroup.Controls.Add(this.m_ctrlPathMaps);
			this.m_ctrlMapsGroup.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlMapsGroup.Name = "m_ctrlMapsGroup";
			this.m_ctrlMapsGroup.Size = new System.Drawing.Size(208, 148);
			this.m_ctrlMapsGroup.TabIndex = 0;
			this.m_ctrlMapsGroup.TabStop = false;
			this.m_ctrlMapsGroup.Text = "Maps";
			// 
			// m_ctrlPathsGroup
			// 
			this.m_ctrlPathsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPathsGroup.Controls.Add(this.m_ctrlPathOverrides);
			this.m_ctrlPathsGroup.Location = new System.Drawing.Point(220, 4);
			this.m_ctrlPathsGroup.Name = "m_ctrlPathsGroup";
			this.m_ctrlPathsGroup.Size = new System.Drawing.Size(172, 176);
			this.m_ctrlPathsGroup.TabIndex = 4;
			this.m_ctrlPathsGroup.TabStop = false;
			this.m_ctrlPathsGroup.Text = "Paths";
			// 
			// m_ctrlPathOverrides
			// 
			this.m_ctrlPathOverrides.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPathOverrides.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								  this.m_lvcImage,
																								  this.m_lvcMedia,
																								  this.m_lvcOverride});
			this.m_ctrlPathOverrides.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.m_ctrlPathOverrides.Location = new System.Drawing.Point(12, 20);
			this.m_ctrlPathOverrides.MultiSelect = false;
			this.m_ctrlPathOverrides.Name = "m_ctrlPathOverrides";
			this.m_ctrlPathOverrides.Size = new System.Drawing.Size(152, 148);
			this.m_ctrlPathOverrides.SmallImageList = this.m_ctrlImages;
			this.m_ctrlPathOverrides.TabIndex = 4;
			this.m_ctrlPathOverrides.View = System.Windows.Forms.View.Details;
			// 
			// m_lvcImage
			// 
			this.m_lvcImage.Text = "";
			this.m_lvcImage.Width = 18;
			// 
			// m_lvcMedia
			// 
			this.m_lvcMedia.Text = "Media";
			// 
			// m_lvcOverride
			// 
			this.m_lvcOverride.Text = "";
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlDelete
			// 
			this.m_ctrlDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlDelete.Location = new System.Drawing.Point(148, 156);
			this.m_ctrlDelete.Name = "m_ctrlDelete";
			this.m_ctrlDelete.Size = new System.Drawing.Size(64, 23);
			this.m_ctrlDelete.TabIndex = 3;
			this.m_ctrlDelete.Text = "&Delete";
			this.m_ctrlDelete.Click += new System.EventHandler(this.OnClickDelete);
			// 
			// CTmaxPathMapsViewerCtrl
			// 
			this.Controls.Add(this.m_ctrlDelete);
			this.Controls.Add(this.m_ctrlPathsGroup);
			this.Controls.Add(this.m_ctrlMapsGroup);
			this.Controls.Add(this.m_ctrlEdit);
			this.Controls.Add(this.m_ctrlAdd);
			this.Name = "CTmaxPathMapsViewerCtrl";
			this.Size = new System.Drawing.Size(396, 188);
			this.Load += new System.EventHandler(this.OnLoad);
			this.m_ctrlMapsGroup.ResumeLayout(false);
			this.m_ctrlPathsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>This method handles the control's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			//	Get the collection of PathMap objects
			if(m_tmaxCaseOptions != null)
				m_tmaxPathMaps = m_tmaxCaseOptions.GetPathMaps(true, false);
				
			//	Must have a valid collection of maps to do anything
			if(m_tmaxPathMaps != null)
			{
				//	Populate the list box
				FillPathMaps();

				//	Set the selection in the list box
				if(m_tmaxCaseOptions.PathMap != null)
					SetMapSelection(m_tmaxCaseOptions.PathMap.Id);
				else
					SetMapSelection(1);
			
			}
			else
			{
				m_ctrlEdit.Enabled = false;
				m_ctrlAdd.Enabled = false;
			}
		}// private void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on the Edit button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickEdit(object sender, System.EventArgs e)
		{
			//	Propagate the event
			if(ClickEdit != null)
			{
				try
				{
					//	Fire the event to open the map editor
					ClickEdit(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickEdit", m_tmaxErrorBuilder.Message(ERROR_EDIT_EX, m_tmaxPathMap.Name), Ex);
				}
				
			}
			
		}// private void OnClickEdit(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on the Delete button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickDelete(object sender, System.EventArgs e)
		{
			//	Propagate the event
			if(ClickDelete != null)
			{
				try
				{
					//	Fire the event to open the map editor
					ClickDelete(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickDelete", m_tmaxErrorBuilder.Message(ERROR_DELETE_EX, m_tmaxPathMap.Name), Ex);
				}
				
			}
			
		}// private void OnClickDelete(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on the Add button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickAdd(object sender, System.EventArgs e)
		{
			//	Propagate the event
			if(ClickAdd != null)
			{
				try
				{
					//	Fire the event to open the map editor
					ClickAdd(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickAdd", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
				}
				
			}
		
		}// private void OnClickAdd(object sender, System.EventArgs e)
		
		/// <summary>This method will populate the list of path maps</summary>
		private void FillPathMaps()
		{
			try
			{
				//	Clear the existing list contents
				m_ctrlPathMaps.Items.Clear();
				
				//	Add an entry for each path map
				if(m_tmaxPathMaps != null)
				{
					foreach(CTmaxPathMap O in m_tmaxPathMaps)
					{
						//	Only add to the list if not deleted
						if(O.IsDeleted == false)
							m_ctrlPathMaps.Items.Add(O);
					}
					
				}// if(m_tmaxPathMaps != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillPathMaps", m_tmaxErrorBuilder.Message(ERROR_FILL_MAPS_EX), Ex);
			}
			
		}// private void FillPathMaps()
		
		/// <summary>This method will select the map with the specified id</summary>
		/// <param name="iId">The id of the desired map</param>
		/// <returns>True if the desired map was selected</returns>
		private bool SetMapSelection(int iId)
		{
			CTmaxPathMap tmaxPathMap = null;
			
			if(m_tmaxPathMaps == null) return false;
			if(m_ctrlPathMaps == null) return false;
			if(m_ctrlPathMaps.IsDisposed == true) return false;
			if(m_ctrlPathMaps.Items == null) return false;
			if(m_tmaxPathMaps.Count == 0) return false;
			if(m_ctrlPathMaps.Items.Count == 0) return false;
			
			//	Locate the requested map
			for(int i = 0; i < m_ctrlPathMaps.Items.Count; i++)
			{
				//	Is this the requested map?
				if((tmaxPathMap = ((CTmaxPathMap)(m_ctrlPathMaps.Items[i]))) != null)
				{
					if((tmaxPathMap.Id == iId) && (tmaxPathMap.IsDeleted == false))
					{
						m_ctrlPathMaps.SelectedIndex = i;
						return true;
					}
				
				}
				
			}// for(int i = 0; i < m_ctrlPathMaps.Items.Count; i++)
			
			//	Select the first in the list if the requested was not found
			m_ctrlPathMaps.SelectedIndex = 0;
			return false;
			
		}// private bool SetMapSelection(int iId)
		
		/// <summary>This method will populate the list of path overrides associated with the selected path map</summary>
		private void FillPathOverrides()
		{
			CTmaxOption tmaxCasePath = null;
			
			try
			{
				//	Clear the existing list contents
				m_ctrlPathOverrides.Items.Clear();
				
				//	Must have an active map
				if((m_tmaxPathMap != null) && (m_tmaxPathMap.CasePaths != null))
				{
					//	Add a row for case path the user has control over
					if((tmaxCasePath = m_tmaxPathMap.CasePaths.Find(TmaxCaseFolders.Documents.ToString())) != null)
						AddPathOverride(tmaxCasePath);
					if((tmaxCasePath = m_tmaxPathMap.CasePaths.Find(TmaxCaseFolders.PowerPoints.ToString())) != null)
						AddPathOverride(tmaxCasePath);
					if((tmaxCasePath = m_tmaxPathMap.CasePaths.Find(TmaxCaseFolders.Recordings.ToString())) != null)
						AddPathOverride(tmaxCasePath);
					if((tmaxCasePath = m_tmaxPathMap.CasePaths.Find(TmaxCaseFolders.Videos.ToString())) != null)
						AddPathOverride(tmaxCasePath);
                    if((tmaxCasePath = m_tmaxPathMap.CasePaths.Find(TmaxCaseFolders.Objections.ToString())) != null)
                        AddPathOverride(tmaxCasePath);
						
//					foreach(CTmaxOption O in m_tmaxPathMap.CasePaths)
//					{
//						AddPathOverride(O);
//					}
					
					//	Automatically resize the columns to fit the text
					m_ctrlPathOverrides.Columns[1].Width = -2;
					m_ctrlPathOverrides.Columns[2].Width = -2;

				}// if((m_tmaxPathMap != null) && (m_tmaxPathMap.CasePaths != null))
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillPathOverrides", m_tmaxErrorBuilder.Message(ERROR_FILL_OVERRIDES_EX), Ex);
			}

		}// private void FillPathOverrides()
		
		/// <summary>This method is called to add an entry to the path overrides list box</summary>
		/// <param name="tmaxOverride">The override to be added</param>
		private void AddPathOverride(CTmaxOption tmaxOverride)
		{
			ListViewItem	lvItem = null;
			string			strPath = "";
			
			try
			{
				//	Get the override path
				if(tmaxOverride.Value != null)
					strPath = tmaxOverride.Value.ToString();
					
				//	Allocate a new item for the list box
				lvItem = new ListViewItem();
				
				//	Set the image for the first column
				lvItem.Text = "";
				
				if(strPath.Length > 0)
				{
					if(System.IO.Directory.Exists(strPath) == true)
						lvItem.ImageIndex = 0;
					else
						lvItem.ImageIndex = 1;
				}
				else
				{
					lvItem.ImageIndex = 2;
				}
							
				//	Add the subitems
				lvItem.SubItems.Add(tmaxOverride.Text);
				lvItem.SubItems.Add(strPath);
			
				//	Add to the list
				m_ctrlPathOverrides.Items.Add(lvItem);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddPathOverride", m_tmaxErrorBuilder.Message(ERROR_ADD_OVERRIDE_EX, tmaxOverride.Text, strPath), Ex);
			}

		}// private void AddPathOverride(CTmaxOption tmaxOverride)
		
		/// <summary>This member handles events fired when the user selects a new path map</summary>
		/// <param name="sender">The path maps list box</param>
		/// <param name="e">System event arguments</param>
		private void OnMapSelChanged(object sender, System.EventArgs e)
		{
			int iIndex = -1;
			
			try
			{
				if((iIndex = m_ctrlPathMaps.SelectedIndex) >= 0)
					m_tmaxPathMap = ((CTmaxPathMap)(m_ctrlPathMaps.Items[iIndex]));
				else
					m_tmaxPathMap = null;
					
				//	Refresh the path overrides list box
				FillPathOverrides();
				
				//	Do we have a map to edit?
				m_ctrlEdit.Enabled = (m_tmaxPathMap != null);
				
				//	Don't allow removal of the default path map
				if(m_tmaxPathMap != null)
					m_ctrlDelete.Enabled = (m_tmaxPathMap.Id != 1);
				else
					m_ctrlDelete.Enabled = false;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnMapSelChanged", m_tmaxErrorBuilder.Message(ERROR_MAP_CHANGED_EX), Ex);
			}
			
		}// private void OnMapSelChanged(object sender, System.EventArgs e)

		/// <summary>This member handles events fired when the user double clicks a path map</summary>
		/// <param name="sender">The path maps list box</param>
		/// <param name="e">System event arguments</param>
		private void OnDblClickMap(object sender, System.EventArgs e)
		{
			if(m_ctrlPathMaps.SelectedIndex >= 0)
				OnClickEdit(m_ctrlEdit, System.EventArgs.Empty);
		
		}// private void OnDblClickMap(object sender, System.EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the path maps list box");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the path overrides list box");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the selected path map: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new path map");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while changing the path map selection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding a path override to the list: %1 = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the selected path map: Name = %1");
			
		}// protected void SetErrorStrings()

		#endregion Private Methods

		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
		}		
		
		/// <summary>Case options containing the case paths to be configured</summary>
		public FTI.Shared.Trialmax.CTmaxCaseOptions CaseOptions
		{
			get	{ return m_tmaxCaseOptions; }
			set	{ m_tmaxCaseOptions = value; }
		}		
		
		/// <summary>True if values have been modified</summary>
		public bool Modified
		{
			get	{ return m_bModified; }
		}
				
		/// <summary>The current path map selection</summary>
		public CTmaxPathMap PathMap
		{
			get	{ return m_tmaxPathMap; }
		}
				
		#endregion Properties
		
	}// public class CTmaxPathMapsViewerCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls
