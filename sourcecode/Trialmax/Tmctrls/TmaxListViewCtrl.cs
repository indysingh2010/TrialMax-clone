using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>
	/// This class implements a Trialmax pane that can be used to display
	///	system messages
	/// </summary>
	public class CTmaxListViewCtrl : CTmaxBaseCtrl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		//protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_0	= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Standard ListView control used to display messages</summary>
		private CExListViewCtrl m_ctrlListView = null;

		/// <summary>Local member to keep track of object used to initialize the control</summary>
		private ITmaxListViewCtrl m_IOwner = null;
		
		/// <summary>Local member bound to Columns property</summary>
		private CTmaxListViewColumns m_tmaxColumns = new CTmaxListViewColumns();
		
		/// <summary>Maximum number of rows contained in the list box</summary>
		private int m_iMaxRows = 0;
		
		/// <summary>True to add messages to the top of the list</summary>
		private bool m_bAddTop = false;
		
		/// <summary>Local member bound to ClearOnDblClick</summary>
		private bool m_bClearOnDblClick = false;
		
		/// <summary>Local image list bound to list view control</summary>
		private System.Windows.Forms.ImageList m_ctrlImages = null;
		
		/// <summary>Local member bound to OwnerImages property</summary>
		private System.Windows.Forms.ImageList m_ctrlOwnerImages = null;
		
		/// <summary>Local member bound to ShowImage property</summary>
		private bool m_bShowImage = false;
		
		/// <summary>Local member bound to ShowHeaders property</summary>
		private bool m_bShowHeaders = true;
		
		/// <summary>Local member bound to AutoResizeColumns property</summary>
		private bool m_bAutoResizeColumns = false;
		
		/// <summary>Local member to bound to DisplayMode property</summary>
		private int m_iDisplayMode = 0;
		
		/// <summary>Local member to suppress selection change notifications</summary>
		private bool m_bSuppressNotifications = false;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxListViewCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();
			
			m_ctrlListView.ExListKeyDown += new KeyEventHandler(OnExListKeyDown);
			m_ctrlListView.ExListKeyUp += new KeyEventHandler(OnExListKeyUp);
		}
		
		/// <summary>Called to remove all rows from the list box</summary>
		public void Clear()
		{
			try { m_ctrlListView.Items.Clear(); }
			catch {}
			
		}// public void Clear()
		
		/// <summary>This method will rebuild the columns using the current collection</summary>
		public bool Initialize(ITmaxListViewCtrl IOwner)
		{
			if(m_ctrlListView == null) return false;
			if(m_tmaxColumns == null) return false;
			if(IOwner == null) return false;

			//	Keep track of the object used to initialize the control
			m_IOwner = IOwner;
			
			//	Clear the current contents
			m_ctrlListView.Clear();
			
			if(m_bShowImage == true)
			{
				if(m_ctrlOwnerImages != null)
					m_ctrlListView.SmallImageList = m_ctrlOwnerImages;
				else
					m_ctrlListView.SmallImageList = this.m_ctrlImages;
			}
			else
			{
				m_ctrlListView.SmallImageList = null;
			}

			//	Create the list view control columns
			if(CreateColumns(IOwner) == false) return false;
				
			//	Did the owner define any columns?
			if(m_tmaxColumns.Count > 0)
			{
				//	Add each column
				foreach(CTmaxListViewColumn O in m_tmaxColumns)
				{
					if(O.Header != null)
					{
						m_ctrlListView.Columns.Add(O.Header);
					}
				}

				SetHeadersVisible(m_bShowHeaders);
				
			}// if(m_tmaxColumns.Count > 0)

			return true;
			
		}// public bool Initialize(ITmaxListViewCtrl IOwner)
		
		/// <summary>This method adds a new row to the list</summary>
		/// <param name="IObject">The object to be assigned to the new row's Tag property</param>
		/// <returns>true if successful</returns>
		public bool Add(ITmaxListViewCtrl IObject)
		{
			ListViewItem lvItem;
			
			//	Do we have any valid columns?
			if((m_tmaxColumns == null) || (m_tmaxColumns.Count == 0))
			{
				//if(m_tmaxColumns == null)
				//    MessageBox.Show("NULL COLUMNS");
				//else
				//    MessageBox.Show("EMPTY COLUMNS");
				return false;
			}
			
			//	Do we have a valid list control?
			if(m_ctrlListView == null)
			{
				//MessageBox.Show("NO LIST VIEW");
				return false;
			}
		
			//	Get the values for the new row
			if(SetColumns(IObject) == false)
			{
				//MessageBox.Show("SET COLUMNS FAILED");
				return false;
			}
			
			lvItem = new ListViewItem();
			lvItem.Tag = IObject;
			
			//	Set the text for the first column
			lvItem.Text = m_tmaxColumns[0].Text;
		
			//	Set the image for this row
			if(m_bShowImage)
				lvItem.ImageIndex = m_tmaxColumns[0].Image;
			
			//	Now add the sub items
			for(int i = 1; i < (int)m_tmaxColumns.Count; i++)
			{
				lvItem.SubItems.Add(m_tmaxColumns[i].Text);
			}

			//	Are we adding to the top?
			if(m_bAddTop)
			{
				//	Add to the top of the list box
				m_ctrlListView.Items.Insert(0, lvItem);

				//	Do we need to make room?
				if(m_iMaxRows > 0)
				{
					while(m_ctrlListView.Items.Count > m_iMaxRows)
					{
						try
						{
							m_ctrlListView.Items.RemoveAt(m_ctrlListView.Items.Count - 1);
						}
						catch
						{
							break;
						}
						
					}
				}
			}
			else
			{
				//	Add the row to the bottom			
				m_ctrlListView.Items.Add(lvItem);

				//	Do we need to make room?
				if(m_iMaxRows > 0)
				{
					while(m_ctrlListView.Items.Count > m_iMaxRows)
					{
						try
						{
							m_ctrlListView.Items.RemoveAt(0);
						}
						catch
						{
							break;
						}
						
					}
					
				}
			}
			
			//	Automatically resize the columns to fit the text
			ResizeColumns();
			
			return true;
		
		}// public bool Add(ITmaxListViewCtrl IObject)
		
		/// <summary>This method allows the caller to add an entire collection of objects</summary>
		/// <param name="ICollection">The collection of objects to be added</param>
		/// <param name="bClear">true to clear the existing rows first</param>
		/// <returns>true if successful</returns>
		public bool Add(IList ICollection, bool bClear)
		{	
			bool bSuccessful = true;

			m_ctrlListView.SuspendLayout();
			
			try
			{
				if(bClear == true)
					m_ctrlListView.Items.Clear();
					
				if(ICollection != null)
				{
					foreach(ITmaxListViewCtrl O in ICollection)
					{
						if(Add(O) == false)
							bSuccessful = false;
					}
				
				}// if(ICollection != null)
			
			}
			catch
			{
				bSuccessful = false;
			}
			
			m_ctrlListView.ResumeLayout();

			return bSuccessful;
						
		}// public bool Add(IList ICollection, bool bClear)
		
		/// <summary>This method updates the row created from the specified object</summary>
		/// <param name="IObject">The object used to create the row</param>
		/// <returns>true if successful</returns>
		public bool Update(ITmaxListViewCtrl IObject)
		{
			ListViewItem	lvi = null;
			bool			bSuccessful = false;

			//	Set the column data
			if(SetColumns(IObject) == false) return false;
			
			//	Get the row item created from this object
			if((lvi = GetItem(IObject)) != null)
			{
				try
				{
					//	Set the text for the first column
					lvi.Text = m_tmaxColumns[0].Text;
			
					if(m_bShowImage)
						lvi.ImageIndex = m_tmaxColumns[0].Image;

					//	Now add the sub items
					for(int i = 1; i < (int)m_tmaxColumns.Count; i++)
					{
						lvi.SubItems[i].Text = m_tmaxColumns[i].Text;
					}
					
					bSuccessful = true;

					//	Automatically resize the columns to fit the text
					ResizeColumns();
				}
				catch
				{
				}
				
			}// if((lvi = GetItem(IObject)) != null)
			
			return bSuccessful;
			
		}// public bool Update(ITmaxListViewCtrl IObject)
		
		/// <summary>This method updates the row created from the specified object</summary>
		/// <param name="IObject">The object used to create the row</param>
		/// <returns>true if successful</returns>
		public bool Remove(ITmaxListViewCtrl IObject)
		{
			ListViewItem	lvi = null;
			bool			bSuccessful = false;

			//	Get the row item created from this object
			if((lvi = GetItem(IObject)) != null)
			{
				m_ctrlListView.Items.Remove(lvi);
				bSuccessful = true;
				
			}// if((lvi = GetItem(IObject)) != null)
			
			return bSuccessful;
			
		}// public bool Remove(ITmaxListViewCtrl IObject)
		
		/// <summary>This method will save the current messages to file</summary>
		/// <param name="strFilename">Path to the file where messages should be stored</param>
		/// <param name="bGetFilename">True if the user should be prompted for the filename (strFilename initializes the dialog)</param>
		/// <param name="bOverwrite">True to automatically overwrite the file if it exists</param>
		/// <returns>True if successful</returns>
		public bool Save(string strFilename, bool bGetFilename, bool bOverwrite)
		{
			System.IO.StreamWriter	streamWriter = null;
			SaveFileDialog			saveFile = null;
			string					strMsg = "";
			string					strLine = "";
			bool					bIsFirst = true;
			bool					bSuccessful = true;

			Debug.Assert(m_ctrlListView != null);
			if(m_ctrlListView == null) return false;
			Debug.Assert(m_tmaxColumns != null);
			if(m_tmaxColumns == null) return false;

			//	Is there anything to be saved?
			if((m_ctrlListView.Items == null) || (m_ctrlListView.Items.Count == 0))
				return false; // Message list is empty

			//	Are we supposed to prompt the user for the filename?
			if(bGetFilename == true)
			{
				//	Initialize the file selection dialog
				saveFile = new SaveFileDialog();
				saveFile.AddExtension = true;
				saveFile.CheckPathExists = true;
				saveFile.OverwritePrompt = false;
				saveFile.FileName = strFilename;
				saveFile.DefaultExt = "txt";
				saveFile.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			
				if(strFilename.Length > 0)		
					saveFile.InitialDirectory = System.IO.Path.GetDirectoryName(strFilename);

				//	Open the dialog box
				if(saveFile.ShowDialog() == DialogResult.OK)
					strFilename = saveFile.FileName;
				else
					return false; // User cancelled
					
				//	Does the file already exist?
				if(System.IO.File.Exists(strFilename) == true)
				{
					strMsg = (strFilename + " already exists.\n\n");
					strMsg += "Do you want to overwrite the existing file (Yes) or append to the existing file (No)?";
					
					switch(MessageBox.Show(strMsg, "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
					{
						case DialogResult.Yes:
						
							bOverwrite = true; // Force an overwrite
							break;
							
						case DialogResult.No:
						
							bOverwrite = false; // Prevent an overwrite
							break;
							
						case DialogResult.Cancel:
						
							return false;
					}
				
				}// if(System.IO.File.Exists(strFilename) == true)
			
			}// if(bGetFilename == true)
			
			//	Open the file
			try
			{
				streamWriter = new System.IO.StreamWriter(strFilename, (bOverwrite == false));
			}
			catch
			{
				//	Just use a message box to notify the user
				strMsg = String.Format("Unable to open {0} to save the messages", strFilename);
				MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return false;
			}
			
			try
			{
				foreach(ListViewItem O in m_ctrlListView.Items)
				{
					//	Prepare for a new line
					strLine = "";
					bIsFirst = true;
					
					//	Iterate the subitems collection to build the line
					//
					//	NOTE:	The first subitem is also the main item that
					//			owns all other subitems
					if((O.SubItems != null) && (O.SubItems.Count > 0))
					{
						foreach(ListViewItem.ListViewSubItem si in O.SubItems)
						{
							if(bIsFirst == true)
								bIsFirst = false;
							else
								strLine += "\t";

							strLine += si.Text;
						}
						
					}// if((O.SubItems != null) && (O.SubItems.Count > 0))
					
					streamWriter.WriteLine(strLine);
						
				}// foreach(ListViewItem O in m_ctrlListView.Items)
				
				//	Add a line break in case the user appends more messages
				streamWriter.WriteLine("");
				
				bSuccessful = true;				
			
			}
			catch(System.Exception Ex)
			{
				strMsg = String.Format("An exception was raised while writing messages to {0}", strFilename);
				m_tmaxEventSource.FireError(this, "Save", strMsg, Ex);
			}
			finally
			{
				if(streamWriter != null)
					streamWriter.Close();
			}
			
			//	Give the user the opprotunity to view the file
			if(bSuccessful == true)
			{
				strMsg = String.Format("{0} messages saved to {1}. Would you like to view the file?", this.Count, strFilename);
				if(MessageBox.Show(strMsg, "View?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					try
					{
						FTI.Shared.Win32.Shell.ShellExecute(this.Handle, "open", strFilename, "", "", FTI.Shared.Win32.User.SW_NORMAL);
					}
					catch
					{
						MessageBox.Show("Unable to launch the viewer for " + strFilename);
					}
					
				}
				
			}
			
			return bSuccessful;
			
		}// Save(string strFilename, bool bGetFilename, bool bOverwrite)
		
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
		public ITmaxListViewCtrl GetSelected()
		{
			int iSelected = -1;
			
			if((iSelected = GetSelectedIndex()) >= 0)
				return (ITmaxListViewCtrl)(m_ctrlListView.Items[iSelected].Tag);
			else
				return null;
				
		}// public ITmaxListViewCtrl GetSelected()
		
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
		/// <param name="ISelected">The object used to create the entry in the list box</param>
		/// <param name="bSuppress">true to suppress change notifications</param>
		/// <returns>True if successful</returns>
		public bool SetSelected(ITmaxListViewCtrl ISelected, bool bSuppress)
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
					
				if(ISelected != null)
				{
					if((lvi = GetItem(ISelected)) != null)
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
				
		}// public bool SetSelected(ITmaxListViewCtrl ISelected)
		
		/// <summary>This method will resize the columns</summary>
		public void ResizeColumns()
		{
			//	Do we have any valid columns?
			if((m_tmaxColumns == null) || (m_tmaxColumns.Count == 0)) return;
			
			//	Do we have a valid list control?
			if(m_ctrlListView == null) return;
			
			//	Automatically resize the columns to fit the text
			SuspendLayout();
			for(int i = 0; i < (int)m_tmaxColumns.Count; i++)
			{
				if(m_tmaxColumns[i].Header != null)
				{
					m_tmaxColumns[i].Header.Width = -2;
					
					if((i == 0) && (m_bShowImage == true))
					{
						m_tmaxColumns[i].Header.Width += 16;
					}
					
				}
			}
			ResumeLayout();
	
		}// public void ResizeColumns()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Called when the window size changes</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
		
			if(m_bAutoResizeColumns == true)
				ResizeColumns();

		}// protected override void OnSizeChanged(EventArgs e)

		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxListViewCtrl));
			this.m_ctrlListView = new CExListViewCtrl();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// m_ctrlListView
			// 
			this.m_ctrlListView.AllowColumnReorder = true;
			this.m_ctrlListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlListView.FullRowSelect = true;
			this.m_ctrlListView.HideSelection = false;
			this.m_ctrlListView.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlListView.MultiSelect = false;
			this.m_ctrlListView.Name = "m_ctrlListView";
			this.m_ctrlListView.Size = new System.Drawing.Size(150, 150);
			this.m_ctrlListView.TabIndex = 0;
			this.m_ctrlListView.View = System.Windows.Forms.View.Details;
			this.m_ctrlListView.DoubleClick += new System.EventHandler(this.OnDoubleClick);
			this.m_ctrlListView.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// CTmaxListViewCtrl
			// 
			this.Controls.Add(this.m_ctrlListView);
			this.Name = "CTmaxListViewCtrl";
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			//	Clear the columns collection
			if(m_tmaxColumns != null)
				m_tmaxColumns.Clear();
				
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Fired by the control when the user clicks on the Edit button</summary>
		public event System.EventHandler SelectedIndexChanged;	
		public event System.EventHandler DblClick;
		public event System.Windows.Forms.KeyEventHandler TmaxListKeyDown;
		public event System.Windows.Forms.KeyEventHandler TmaxListKeyUp;	
	
		private void OnSelectedIndexChanged(object sender, System.EventArgs e)
		{
			//	When the user selects a new item, .NET fires a de-select and
			//	then a select event. We want to ignore the de-select
			if(m_ctrlListView.SelectedIndices.Count == 0)
				if(m_ctrlListView.Items.Count > 0) return; // Ignore
				
			//	Bubble the event
			if((m_bSuppressNotifications == false) && (SelectedIndexChanged != null))
			{
				SelectedIndexChanged(this, e);
			}
			
		}// private void OnSelectedIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method will populate the local columns collection</summary>
		/// <param name="IOwner">The interface to an object that will be displayed in the list view</param>
		/// <returns>true if successful</returns>
		private bool CreateColumns(ITmaxListViewCtrl IOwner)
		{
			string[] aNames = null;
			
			//	We must have a columns collection and owner object
			if(m_tmaxColumns == null) return false;
			if(IOwner == null) return false;

			//	Clear the current collection
			m_tmaxColumns.Clear();
			
			//	Get the array of names that represent the columns
			if((aNames = IOwner.GetColumnNames(this.DisplayMode)) == null) return false;
			
			//	Fill the columns collection
			for(int i = 0; i <= aNames.GetUpperBound(0); i++)
			{
				try { m_tmaxColumns.Add(new CTmaxListViewColumn(aNames[i])); }
				catch { return false; }
			}
			
			//	All columns have been added
			return true;
			
		}// private bool CreateColumns(ITmaxListViewCtrl IOwner)
 		
		/// <summary>This method traps DoubleClick events sent to this window</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">system event arguments</param>
		private void OnDoubleClick(object sender, System.EventArgs e)
		{
			//	Clear the current contents
			if(m_bClearOnDblClick == true)
			{
				if(m_ctrlListView.Items != null)
					m_ctrlListView.Items.Clear();
			}
			else
			{	
				if(DblClick != null)
					DblClick(this, System.EventArgs.Empty);
			}
		
		}// private void OnDoubleClick(object sender, System.EventArgs e)

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
		
		/// <summary>This method set the value of the ShowImage property</summary>
		private bool SetShowImage(bool bShowImage)
		{
			bool bSuccessful = true;
			
			//	Has the value changed?
			if(m_bShowImage != bShowImage)
			{
				m_bShowImage = bShowImage;
				
				//	Has the control been initialized?
				if(m_IOwner != null)
					bSuccessful = Initialize(m_IOwner);
			}
			
			return bSuccessful;
			
		}// private bool SetShowImage(bool bShowImage)
		
		/// <summary>This method set the value of the ShowHeaders property</summary>
		private bool SetShowHeaders(bool bShowHeaders)
		{
			bool bSuccessful = true;
			
			//	Has the value changed?
			if(m_bShowHeaders != bShowHeaders)
				SetHeadersVisible(bShowHeaders);
			
			return bSuccessful;
			
		}// private bool SetShowHeaders(bool bShowHeaders)
		
		/// <summary>This method shows / hides the column headers</summary>
		/// <param name="bVisible">True to make the headers visible</param>
		private void SetHeadersVisible(bool bVisible)
		{
			m_bShowHeaders = bVisible;

			if(m_bShowHeaders == true)
				m_ctrlListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
			else
				m_ctrlListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		
		}// private void SetHeadersVisible(bool bVisible)
		
		/// <summary>This method sets the text for each column using the interface object supplied by the caller</summary>
		/// <param name="IObject">The object that will supplied the values</param>
		/// <returns>true if successful</returns>
		private bool SetColumns(ITmaxListViewCtrl IObject)
		{	
			string[]	aValues = null;
			bool		bSuccessful = false;
			
			Debug.Assert(IObject != null);
			if(IObject == null) return false;
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				//	Set the image index
				//
				//	NOTE:	The first column is always used to set the row image
				if((this.ShowImage == true) && (m_ctrlListView.SmallImageList != null))
				{
					if(m_tmaxColumns[0] != null)
						m_tmaxColumns[0].Image = IObject.GetImageIndex(this.DisplayMode);
				}

				//	Get the values for each column
				if((aValues = IObject.GetValues(this.DisplayMode)) == null) return false;
				
				//	The array sizes should be the same
				Debug.Assert(aValues.GetUpperBound(0) == (m_tmaxColumns.Count - 1));
				if(aValues.GetUpperBound(0) != (m_tmaxColumns.Count - 1)) return false;
				
				//	Assume the columns are in the correct order
				for(int i = 0; i <= aValues.GetUpperBound(0); i++)
				{
					if((m_tmaxColumns[i] != null) && (aValues[i] != null))
						m_tmaxColumns[i].Text = aValues[i];
				}

				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(ITmaxListViewCtrl IObject)
		
		/// <summary>Handles events fired by the child list view control when the user presses a key</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">system key event arguments</param>
		private void OnExListKeyDown(object sender, KeyEventArgs e)
		{
			//	Bubble the event
			if(TmaxListKeyDown != null)
				TmaxListKeyDown(this, e);
		}

		/// <summary>Handles events fired by the child list view control when the user presses a key</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">system key event arguments</param>
		private void OnExListKeyUp(object sender, KeyEventArgs e)
		{
			//	Bubble the event
			if(TmaxListKeyUp != null)
				TmaxListKeyUp(this, e);
		}

		#endregion Private Methods

		#region Properties

		/// <summary>This property exposes the child list view control</summary>
		public System.Windows.Forms.ListView ListView
		{
			get { return m_ctrlListView; }
		}
		
		/// <summary>The number of rows in the list</summary>
		public int Count
		{
			get { return m_ctrlListView.Items.Count; }
		}
		
		/// <summary>Image list used to replace the default image list</summary>
		public System.Windows.Forms.ImageList OwnerImages
		{
			get { return m_ctrlOwnerImages; }
			set { m_ctrlOwnerImages = value; }
		}

		/// <summary>This property indicates the maximum number of rows allowed</summary>
		public int MaxRows
		{
			get { return m_iMaxRows; }
			set { m_iMaxRows = value; }
		}

		/// <summary>This property indicates if new items should be added at the top of the list</summary>
		public bool AddTop
		{
			get { return m_bAddTop; }
			set { m_bAddTop = value; }
		}

		/// <summary>True to clear the contents on a double click</summary>
		public bool ClearOnDblClick
		{
			get { return m_bClearOnDblClick; }
			set { m_bClearOnDblClick = value; }
		}

		/// <summary>User defined value to allow for multiple display modes</summary>
		public int DisplayMode
		{
			get { return m_iDisplayMode; }
			set { m_iDisplayMode = value; }
		}

		/// <summary>This is the collection of columns used to build the pane</summary>
		public CTmaxListViewColumns Columns
		{
			get { return m_tmaxColumns; }
		}

		/// <summary>True to display an image in the first column</summary>
		public bool ShowImage
		{
			get{ return m_bShowImage; }
			set { SetShowImage(value); }
		}

		/// <summary>True to automatically resize columns when the window size changes</summary>
		public bool AutoResizeColumns
		{
			get{ return m_bAutoResizeColumns; }
			set 
			{ 
				m_bAutoResizeColumns = value; 
				if(m_bAutoResizeColumns == true) 
					ResizeColumns();
			}
		
		}

		/// <summary>True to show the column headers</summary>
		public bool ShowHeaders
		{
			get { return m_bShowHeaders; }
			set { SetShowHeaders(value); }
		}

		/// <summary>This method is called to get the index of the current selection</summary>
		public int SelectedIndex
		{
			get { return GetSelectedIndex(); }
			set { SetSelectedIndex(value, false); }
		}
		
		/// <summary>This method is called to set the HideSelection property of the child list view control</summary>
		public bool HideSelection
		{
			get { return (m_ctrlListView != null) ? m_ctrlListView.HideSelection : false; }
			set { if(m_ctrlListView != null) m_ctrlListView.HideSelection = value; }
		}
		
		#endregion Properties
		
	}// class CTmaxListViewCtrl
	
	/// <summary>This class is used to define a column in the message pane</summary>
	public class CTmaxListViewColumn
	{
		#region Private Members
			
		/// <summary>Local member bound to Header property</summary>
		private ColumnHeader m_ctrlHeader = new ColumnHeader();
		
		/// <summary>Local member  bound to the Text property</summary>
		private string m_strText = "";
			
		/// <summary>Local member bound to the Image property</summary>
		private int m_iImage = -1;
			
		#endregion Private Members
			
		#region Public Methods
			
		/// <summary>Constructor</summary>
		public CTmaxListViewColumn()
		{
			Initialize("", -2, HorizontalAlignment.Left);
		}
			
		/// <summary>Constructor</summary>
		/// <param name="strName">Name that appears in column header</param>
		public CTmaxListViewColumn(string strName)
		{
			Initialize(strName, -2, HorizontalAlignment.Left);
		}
			
		/// <summary>Constructor</summary>
		/// <param name="strName">Name that appears in column header</param>
		/// <param name="iWidth">Width of the column</param>
		public CTmaxListViewColumn(string strName, int iWidth)
		{
			Initialize(strName, iWidth, HorizontalAlignment.Left);
		}
			
		/// <summary>Constructor</summary>
		/// <param name="strName">Name that appears in column header</param>
		/// <param name="iWidth">Width of the column</param>
		/// <param name="eHorizontal">Horizontal text alignment</param>
		public CTmaxListViewColumn(string strName, int iWidth, HorizontalAlignment eHorizontal)
		{
			Initialize(strName, iWidth, eHorizontal);
		}
			
		/// <summary>Called to initialize the column</summary>
		/// <param name="strName">Name that appears in the column header</param>
		/// <param name="iWidth">Width of the column</param>
		/// <param name="eHorizontal">Horizontal text alignment</param>
		public void Initialize(string strName, int iWidth, HorizontalAlignment eHorizontal)
		{
			if(strName != null)
				m_ctrlHeader.Text = strName;
			m_ctrlHeader.Width = iWidth;
			m_ctrlHeader.TextAlign = eHorizontal;
		
		}// public void Initialize(string strName, int iWidth, HorizontalAlignment eHorizontal)
			
		#endregion Public Methods
			
		#region Properties
			
		/// <summary>This property exposes the header control associated with this column</summary>
		public ColumnHeader Header 
		{
			get { return m_ctrlHeader; }
		}
			
		/// <summary>This property is used to set the text displayed in the column</summary>
		public string Text
		{
			get { return m_strText; }
			set { m_strText = value; }
		}
			
		/// <summary>This property is used to set the jmage displayed in the column</summary>
		public int Image
		{
			get { return m_iImage; }
			set { m_iImage = value; }
		}
			
		/// <summary>This property is used to set the Name displayed in the column</summary>
		public string Name
		{
			get
			{
				if(m_ctrlHeader != null)
					return m_ctrlHeader.Text;
				else
					return "";
			}
			set
			{
				if(m_ctrlHeader != null)
					m_ctrlHeader.Text = value;
			}
			
		}// Name property
			
		/// <summary>This property is used to set the width of the column in pixels</summary>
		public int Width
		{
			get
			{
				if(m_ctrlHeader != null)
					return m_ctrlHeader.Width;
				else
					return 0;
			}
			set
			{
				if(m_ctrlHeader != null)
					m_ctrlHeader.Width = value;
			}
			
		}// Width property
			
		/// <summary>This property is used to set the alignment of text in the column</summary>
		public System.Windows.Forms.HorizontalAlignment Alignment
		{
			get
			{
				if(m_ctrlHeader != null)
					return m_ctrlHeader.TextAlign;
				else
					return 0;
			}
			set
			{
				if(m_ctrlHeader != null)
					m_ctrlHeader.TextAlign = value;
			}
			
		}// Alignment property
			
		#endregion Properties

	}// public class CTmaxListViewColumn
		
	/// <summary>Objects of this class are used to manage a dynamic array of CTmaxListViewColumn objects</summary>
	public class CTmaxListViewColumns : CollectionBase
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxListViewColumns()
		{
		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="mpColumn">CTmaxListViewColumn object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxListViewColumn Add(CTmaxListViewColumn mpColumn)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(mpColumn as object);

				return mpColumn;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxListViewColumn mpColumn)

		/// <summary>
		/// This method is called to remove the requested filter from the collection
		/// </summary>
		/// <param name="mpColumn">The filter object to be removed</param>
		public void Remove(CTmaxListViewColumn mpColumn)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(mpColumn as object);
			}
			catch
			{
			}
		}

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="mpColumn">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxListViewColumn mpColumn)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(mpColumn as object);
		}

		/// <summary>
		/// Called to locate the object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxListViewColumn Find(string strName)
		{
			// Search for the object with the same name
			foreach(CTmaxListViewColumn obj in base.List)
			{
				if(String.Compare(obj.Name, strName, true) == 0)
				{
					return obj;
				}
			}
			return null;

		}//	Find(string strName)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public CTmaxListViewColumn this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (base.List[index] as CTmaxListViewColumn);
			}
		}

		/// <summary>
		/// Overloaded [] operator to locate the parameter object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxListViewColumn this[string strName]
		{
			get 
			{
				// Search for the object with the same name
				foreach(CTmaxListViewColumn obj in base.List)
				{
					if(String.Compare(obj.Name, strName, true) == 0)
					{
						return obj;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// This method is called to retrieve the index of the specified object
		/// </summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxListViewColumn value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	CTmaxListViewColumns
			
	/// <summary>Extended list view control to add key down/up notifications</summary>
	public class CExListViewCtrl : System.Windows.Forms.ListView
	{
			
		#region Public Methods
			
		public event System.Windows.Forms.KeyEventHandler ExListKeyDown;		
		public event System.Windows.Forms.KeyEventHandler ExListKeyUp;		

		/// <summary>Constructor</summary>
		public CExListViewCtrl()
		{
		
		}
		
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(ExListKeyDown != null)
			{
				ExListKeyDown(this, e);
	
				if(e.Handled == false)
					base.OnKeyDown(e);
			}
			else
			{
				base.OnKeyDown(e);
			}
			
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if(ExListKeyUp != null)
			{
				ExListKeyUp(this, e);
	
				if(e.Handled == false)
					base.OnKeyUp(e);
			}
			else
			{
				base.OnKeyUp(e);
			}

		}

		
		#endregion Public Methods
			
	}// public class CExListViewCtrl : System.Windows.Forms.ListView
	

}// namespace FTI.Trialmax.Controls

