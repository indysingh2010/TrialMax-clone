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
	public class CTmaxMessageCtrl : System.Windows.Forms.UserControl
	{
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Standard ListView control used to display messages</summary>
		private System.Windows.Forms.ListView m_ctrlMessages;

		/// <summary>Local member bound to Columns property</summary>
		private CTmaxMessageCtrlColumns m_tmaxColumns = new CTmaxMessageCtrlColumns();
		
		/// <summary>Local member bound to Format property</summary>
		private TmaxMessageFormats m_eFormat = TmaxMessageFormats.UserDefined;
		
		/// <summary>Maximum number of rows contained in the list box</summary>
		private int m_iMaxRows = 0;
		
		/// <summary>True to add messages to the top of the list</summary>
		private bool m_bAddTop = false;
		
		/// <summary>Local member bound to ClearOnDblClick</summary>
		private bool m_bClearOnDblClick = false;
		
		/// <summary>Local image list bound to list view control</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Local member bound to ShowImage property</summary>
		private bool m_bShowImage = false;
		
		/// <summary>Local member bound to ShowHeaders property</summary>
		private bool m_bShowHeaders = true;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxMessageCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();
		}
		
		/// <summary>Called to remove all rows from the list box</summary>
		public void Clear()
		{
			try { m_ctrlMessages.Items.Clear(); }
			catch {}
			
		}// public void Clear()
		
		/// <summary>This method adds a new row to the list using the text defined in the columns collection</summary>
		/// <returns>true if successful</returns>
		public bool Add()
		{
			return Add((object)null);
		}
		
		/// <summary>This method allows the caller to add a row to the list box using the specified CBaseVersion object</summary>
		/// <param name="tmVersion">Version object containing the information needed to populate the row</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Version</remarks>
		public bool Add(FTI.Shared.CBaseVersion tmVersion)
		{	
			if(SetColumns(tmVersion) == true)
				return Add((object)tmVersion);
			else
				return false;
		
		}// Add(FTI.Shared.CBaseVersion tmVersion)
		
		/// <summary>This method updates the row created with the specified object</summary>
		/// <param name="tmVersion">The object used to create the row</param>
		/// <returns>true if successful</returns>
		public bool Update(FTI.Shared.CBaseVersion tmVersion)
		{	
			if(SetColumns(tmVersion) == true)
				return Update((object)tmVersion);
			else
				return false;
		
		}// public bool Update(FTI.Shared.CBaseVersion tmVersion)
		
		/// <summary>This method allows the caller to add a row to the list box using the specified record interface</summary>
		/// <param name="tmaxRecord">Record object containing the information needed to populate the row</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Record</remarks>
		public bool Add(FTI.Shared.Trialmax.ITmaxMediaRecord tmaxRecord)
		{	
			if(SetColumns(tmaxRecord) == true)
				return Add((object)tmaxRecord);
			else
				return false;
		
		}// public bool Add(FTI.Shared.Trialmax.ITmaxRecord tmaxRecord)
		
		/// <summary>This method allows the caller to update a row to the list box using the specified record interface</summary>
		/// <param name="tmaxRecord">Record object used to create the row</param>
		/// <returns>true if successful</returns>
		public bool Update(FTI.Shared.Trialmax.ITmaxMediaRecord tmaxRecord)
		{	
			if(SetColumns(tmaxRecord) == true)
				return Update((object)tmaxRecord);
			else
				return false;
		
		}// public bool Update(FTI.Shared.Trialmax.ITmaxRecord tmaxRecord)
		
		/// <summary>This method allows the caller to add a row to the list box using the specified search result</summary>
		/// <param name="tmaxResult">Object containing the information needed to populate the row</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == SearchResult</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{	
			if(SetColumns(tmaxResult) == true)
				return Add((object)tmaxResult);
			else
				return false;
		
		}// public bool Add(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		
		/// <summary>This method allows the caller to update a row to the list box using the specified search result</summary>
		/// <param name="tmaxResult">Object used to create the row</param>
		/// <returns>true if successful</returns>
		public bool Update(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{	
			if(SetColumns(tmaxResult) == true)
				return Update((object)tmaxResult);
			else
				return false;
		
		}// public bool Update(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		
		/// <summary>This method allows the caller to add an entire collection of search results</summary>
		/// <param name="tmaxResults">The collection of objects to be added</param>
		/// <param name="bClear">true to clear the existing results first</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == SearchResult</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxSearchResults tmaxResults, bool bClear)
		{	
			bool bSuccessful = true;
			
			m_ctrlMessages.SuspendLayout();
			
			if(bClear == true)
				m_ctrlMessages.Items.Clear();
				
			foreach(CTmaxSearchResult O in tmaxResults)
			{
				if(Add(O) == false)
					bSuccessful = false;
			}
			
			m_ctrlMessages.ResumeLayout();
			
			return bSuccessful;
						
		}// Add(FTI.Shared.Trialmax.CTmaxSearchResults tmaxResults, bool bClear)
		
		/// <summary>This method allows the caller to add a row to the list box using the specified import message</summary>
		/// <param name="tmaxMessage">Object containing the information needed to populate the row</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == ImportMessage</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxImportMessage tmaxMessage)
		{	
			if(SetColumns(tmaxMessage) == true)
				return Add((object)tmaxMessage);
			else
				return false;
		
		}// public bool Add(FTI.Shared.Trialmax.CTmaxImportMessage tmaxMessage)
		
		/// <summary>This method allows the caller to update a row to the list box using the specified import message</summary>
		/// <param name="tmaxMessage">Object used to create the row</param>
		/// <returns>true if successful</returns>
		public bool Update(FTI.Shared.Trialmax.CTmaxImportMessage tmaxMessage)
		{	
			if(SetColumns(tmaxMessage) == true)
				return Update((object)tmaxMessage);
			else
				return false;
		
		}// public bool Update(FTI.Shared.Trialmax.CTmaxImportMessage tmaxMessage)
		
		/// <summary>This method allows the caller to add an entire collection of import messages</summary>
		/// <param name="tmaxMessages">The collection of objects to be added</param>
		/// <param name="bClear">true to clear the existing rows first</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == ImportMessage</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxImportMessages tmaxMessages, bool bClear)
		{	
			bool bSuccessful = true;
			
			m_ctrlMessages.SuspendLayout();
			
			if(bClear == true)
				m_ctrlMessages.Items.Clear();
				
			foreach(CTmaxImportMessage O in tmaxMessages)
			{
				if(Add(O) == false)
					bSuccessful = false;
			}
			
			m_ctrlMessages.ResumeLayout();
			
			return bSuccessful;
						
		}// public bool Add(FTI.Shared.Trialmax.CTmaxImportMessages tmaxMessages, bool bClear)
		
		/// <summary>This method allows the caller to add a row to the list box using the specified system message</summary>
		/// <param name="tmaxMessage">Object containing the information needed to populate the row</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Message</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxMessage tmaxMessage)
		{	
			if(SetColumns(tmaxMessage) == true)
				return Add((object)tmaxMessage);
			else
				return false;
		
		}// public bool Add(FTI.Shared.Trialmax.CTmaxMessage tmaxMessage)
		
		/// <summary>This method allows the caller to add an entire collection of system messages</summary>
		/// <param name="tmaxMessages">The collection of objects to be added</param>
		/// <param name="bClear">true to clear the existing rows first</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Message</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxMessages tmaxMessages, bool bClear)
		{	
			bool bSuccessful = true;
			
			m_ctrlMessages.SuspendLayout();
			
			if(bClear == true)
				m_ctrlMessages.Items.Clear();
				
			foreach(CTmaxMessage O in tmaxMessages)
			{
				if(Add(O) == false)
					bSuccessful = false;
			}
			
			m_ctrlMessages.ResumeLayout();
			
			return bSuccessful;
						
		}// public bool Add(FTI.Shared.Trialmax.CTmaxMessages tmaxMessages, bool bClear)
		
		/// <summary>This method allows the caller to add a row to the list box using the specified CTmaxErrorArgs object</summary>
		/// <param name="Args">Error argument object containing the error information</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Error</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		{	
			if(SetColumns(Args) == true)
				return Add((object)Args);
			else
				return false;
		
		}// public bool Add(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		
		/// <summary>This method allows the caller to add a row to the list box using the specified CTmaxDiagnosticArgs object</summary>
		/// <param name="Args">Diagnostic event argument object</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Diagnostics</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxDiagnosticArgs Args)
		{	
			if(SetColumns(Args) == true)
				return Add((object)Args);
			else
				return false;
		
		}// public bool Add(FTI.Shared.Trialmax.CTmaxDiagnosticArgs Args)
		
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

			Debug.Assert(m_ctrlMessages != null);
			if(m_ctrlMessages == null) return false;
			Debug.Assert(m_tmaxColumns != null);
			if(m_tmaxColumns == null) return false;

			//	Is there anything to be saved?
			if((m_ctrlMessages.Items == null) || (m_ctrlMessages.Items.Count == 0))
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
				foreach(ListViewItem O in m_ctrlMessages.Items)
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
						
				}// foreach(ListViewItem O in m_ctrlMessages.Items)
				
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
		
		/// <summary>This method will rebuild the columns using the current collection</summary>
		public void Rebuild()
		{
			if(m_ctrlMessages == null) return;
			if(m_tmaxColumns == null) return;

			//	Clear the current contents
			m_ctrlMessages.Clear();
			
			if(m_bShowImage == true)
				m_ctrlMessages.SmallImageList = this.m_ctrlImages;
			else
				m_ctrlMessages.SmallImageList = null;

			//	Populate the columns collection
			if(m_eFormat != TmaxMessageFormats.UserDefined)
				CreateColumns();
			
			//	Do we have any columns to add?
			if(m_tmaxColumns.Count > 0)
			{
				//	Add each column
				foreach(CTmaxMessageCtrlColumn objColumn in m_tmaxColumns)
				{
					if(objColumn.Header != null)
					{
						m_ctrlMessages.Columns.Add(objColumn.Header);
					}
				}
				
				ShowColumnHeaders(m_bShowHeaders);
			
			}// if(m_tmaxColumns.Count > 0)
			
		}// public void Rebuild()
		
		/// <summary>This method shows / hides the column headers</summary>
		/// <param name="bVisible">True to make the headers visible</param>
		public void ShowColumnHeaders(bool bVisible)
		{
			m_bShowHeaders = bVisible;
			
			if(m_bShowHeaders == true)
				m_ctrlMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
			else
				m_ctrlMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		
		}// public void ShowColumnHeaders(bool bVisible)
		
		/// <summary>This method is called to get the index of the current selection</summary>
		/// <returns>The index of the current (or first) selection</returns>
		public int GetSelectedIndex()
		{
			int iSelected = -1;
			
			if((m_ctrlMessages != null) && (m_ctrlMessages.SelectedIndices != null))
			{
				if(m_ctrlMessages.SelectedIndices.Count > 0)
					iSelected = m_ctrlMessages.SelectedIndices[0];
			}
			
			return iSelected;
				
		}// public int GetSelectedIndex()
		
		/// <summary>This method is called to set the index of the current selection</summary>
		/// <param name="iIndex">The index of the current selection</param>
		/// <returns>True if successful</returns>
		public bool SetSelectedIndex(int iIndex)
		{
			bool bSuccessful = false;
			
			try
			{
				if((m_ctrlMessages != null) && (m_ctrlMessages.Items != null))
				{
					//	Clear the current selections
					if(m_ctrlMessages.SelectedItems != null)
						m_ctrlMessages.SelectedItems.Clear();
					
					if((iIndex >= 0) && (iIndex < m_ctrlMessages.Items.Count))
						m_ctrlMessages.Items[iIndex].Selected = true;
						
					bSuccessful = true;
				}

			}
			catch
			{
			}
			
			return bSuccessful;
				
		}// public bool SetSelectedIndex(int iIndex)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Required by form designer</summary>
		protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxMessageCtrl));
			this.m_ctrlMessages = new System.Windows.Forms.ListView();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// m_ctrlMessages
			// 
			this.m_ctrlMessages.AllowColumnReorder = true;
			this.m_ctrlMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlMessages.FullRowSelect = true;
			this.m_ctrlMessages.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlMessages.MultiSelect = false;
			this.m_ctrlMessages.Name = "m_ctrlMessages";
			this.m_ctrlMessages.Size = new System.Drawing.Size(150, 150);
			this.m_ctrlMessages.TabIndex = 0;
			this.m_ctrlMessages.View = System.Windows.Forms.View.Details;
			this.m_ctrlMessages.DoubleClick += new System.EventHandler(this.OnDoubleClick);
			this.m_ctrlMessages.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// CTmaxMessageCtrl
			// 
			this.Controls.Add(this.m_ctrlMessages);
			this.Name = "CTmaxMessageCtrl";
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
	
		private void OnSelectedIndexChanged(object sender, System.EventArgs e)
		{
			//	Bubble the event
			if(SelectedIndexChanged != null)
				SelectedIndexChanged(this, e);
			
		}// private void OnSelectedIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method sets the text for each column using the CBaseVersion object provided by the caller</summary>
		/// <param name="tmVersion">The object used to set the column text</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Version</remarks>
		private bool SetColumns(FTI.Shared.CBaseVersion tmVersion)
		{	
			CTmaxMessageCtrlColumn	mpColumn = null;
			bool				bSuccessful = false;
			
			Debug.Assert(tmVersion != null);
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				//	Update the column text
				if((mpColumn = m_tmaxColumns["Assembly"]) != null)
					mpColumn.Text = tmVersion.Title;
				if((mpColumn = m_tmaxColumns["Version"]) != null)
					mpColumn.Text = tmVersion.ShortVersion;
				if((mpColumn = m_tmaxColumns["Build"]) != null)
					mpColumn.Text = tmVersion.BuildDate;
				if((mpColumn = m_tmaxColumns["Location"]) != null)
					mpColumn.Text = tmVersion.Location.ToLower();
				if((mpColumn = m_tmaxColumns["Description"]) != null)
					mpColumn.Text = tmVersion.Description;
				
				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(FTI.Shared.CBaseVersion tmVersion)
		
		/// <summary>This method sets the text for each column using the ITmaxRecord object provided by the caller</summary>
		/// <param name="tmaxRecord">The object used to set the column text</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Record</remarks>
		private bool SetColumns(FTI.Shared.Trialmax.ITmaxMediaRecord tmaxRecord)
		{	
			CTmaxMessageCtrlColumn	mpColumn = null;
			bool					bSuccessful = false;
			
			Debug.Assert(tmaxRecord != null);
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				if(tmaxRecord.GetDataType() == TmaxDataTypes.Binder)
				{
					if((mpColumn = m_tmaxColumns["Type"]) != null)
						mpColumn.Text = "Binder";
					if((mpColumn = m_tmaxColumns["Barcode"]) != null)
						mpColumn.Text = ""; // No barcode for binders
					if((mpColumn = m_tmaxColumns["Name"]) != null)
						mpColumn.Text = tmaxRecord.GetName();
					if((mpColumn = m_tmaxColumns["Description"]) != null)
						mpColumn.Text = tmaxRecord.GetDescription();
				}
				else
				{
					switch(tmaxRecord.GetMediaType())
					{
						case TmaxMediaTypes.Link:
					
							if((mpColumn = m_tmaxColumns["Type"]) != null)
								mpColumn.Text = "Link";
							if((mpColumn = m_tmaxColumns["Barcode"]) != null)
								mpColumn.Text = ""; // No barcode for links
							if((mpColumn = m_tmaxColumns["Name"]) != null)
								mpColumn.Text = tmaxRecord.GetName();
							
							if((mpColumn = m_tmaxColumns["Description"]) != null)
							{
								if(tmaxRecord.GetParent() != null)
								{
									mpColumn.Text = ("Parent: " + tmaxRecord.GetParent().GetName());
								}
							}
							break;

						case TmaxMediaTypes.Treatment:
							
							if((mpColumn = m_tmaxColumns["Type"]) != null)
							{
								if(tmaxRecord.GetSplitScreen() == true)
									mpColumn.Text = "Split Treatment";
								else
									mpColumn.Text = "Treatment";
							}
							if((mpColumn = m_tmaxColumns["Barcode"]) != null)
								mpColumn.Text = tmaxRecord.GetBarcode(false);
							if((mpColumn = m_tmaxColumns["Name"]) != null)
								mpColumn.Text = tmaxRecord.GetName();
							if((mpColumn = m_tmaxColumns["Description"]) != null)
								mpColumn.Text = tmaxRecord.GetDescription();
							break;

						case TmaxMediaTypes.Scene:
					
							if((mpColumn = m_tmaxColumns["Type"]) != null)
								mpColumn.Text = "Scene";
							if((mpColumn = m_tmaxColumns["Barcode"]) != null)
								mpColumn.Text = tmaxRecord.GetBarcode(false);
							if((mpColumn = m_tmaxColumns["Name"]) != null)
								mpColumn.Text = "";
							if((mpColumn = m_tmaxColumns["Description"]) != null)
								mpColumn.Text = "";
							break;

						default:
					
							if((mpColumn = m_tmaxColumns["Type"]) != null)
								mpColumn.Text = tmaxRecord.GetMediaType().ToString();
							if((mpColumn = m_tmaxColumns["Barcode"]) != null)
								mpColumn.Text = tmaxRecord.GetBarcode(false);
							if((mpColumn = m_tmaxColumns["Name"]) != null)
								mpColumn.Text = tmaxRecord.GetName();
							if((mpColumn = m_tmaxColumns["Description"]) != null)
								mpColumn.Text = tmaxRecord.GetDescription();
							break;
						
					}// switch(tmaxRecord.GetMediaType())
				
				}// private bool SetColumns(FTI.Shared.Trialmax.ITmaxRecord tmaxRecord)

				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(FTI.Shared.CBaseVersion tmVersion)
		
		/// <summary>This method sets the text for each column using the CTmaxSearchResult object provided by the caller</summary>
		/// <param name="tmaxResult">The object used to set the column text</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == SearchResult</remarks>
		private bool SetColumns(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{	
			CTmaxMessageCtrlColumn	mpColumn = null;
			bool				bSuccessful = false;
			
			Debug.Assert(tmaxResult != null);
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				if((mpColumn = m_tmaxColumns["Script"]) != null)
				{
					if(tmaxResult.IScene != null)
						mpColumn.Text = tmaxResult.IScene.GetBarcode(false);
					else
						mpColumn.Text = "";
				}
				if((mpColumn = m_tmaxColumns["Transcript"]) != null)
					mpColumn.Text = tmaxResult.Transcript;
				if((mpColumn = m_tmaxColumns["Page"]) != null)
					mpColumn.Text = tmaxResult.Page.ToString();
				if((mpColumn = m_tmaxColumns["Line"]) != null)
					mpColumn.Text = tmaxResult.Line.ToString();
				if((mpColumn = m_tmaxColumns["Text"]) != null)
					mpColumn.Text = tmaxResult.Text;
				
				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		
		/// <summary>This method sets the text for each column using the CTmaxImportMessage object provided by the caller</summary>
		/// <param name="tmaxMessage">The object used to set the column text</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == ImportMessage</remarks>
		private bool SetColumns(FTI.Shared.Trialmax.CTmaxImportMessage tmaxMessage)
		{	
			CTmaxMessageCtrlColumn	mpColumn = null;
			bool				bSuccessful = false;
			
			Debug.Assert(tmaxMessage != null);
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				//	Set the image index
				//
				//	NOTE:	The first column is always used to set the row image
				if((mpColumn = m_tmaxColumns[0]) != null)
					mpColumn.Image = GetImageIndex(tmaxMessage.Level);

				if((mpColumn = m_tmaxColumns["Filename"]) != null)
					mpColumn.Text = tmaxMessage.Filename;
				if((mpColumn = m_tmaxColumns["Line"]) != null)
				{
					if(tmaxMessage.LineNumber > 0)
						mpColumn.Text = tmaxMessage.LineNumber.ToString();
					else
						mpColumn.Text = "";
				}
				if((mpColumn = m_tmaxColumns["Message"]) != null)
					mpColumn.Text = tmaxMessage.Message;

				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(FTI.Shared.Trialmax.CTmaxImportMessage tmaxMessage)
		
		/// <summary>This method sets the text for each column using the CTmaxErrorArgs object provided by the caller</summary>
		/// <param name="Args">The object used to set the column text</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Error</remarks>
		private bool SetColumns(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		{	
			CTmaxMessageCtrlColumn	mpColumn = null;
			bool				bSuccessful = false;
			
			Debug.Assert(Args != null);
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				//	Update the column text
				if((mpColumn = m_tmaxColumns["Message"]) != null)
				{
					if((Args.Exception != null) && (Args.Exception.Length > 0))
						mpColumn.Text = Args.Exception;
					else
						mpColumn.Text = Args.Message;
				}

				if((mpColumn = m_tmaxColumns["Time"]) != null)
					mpColumn.Text = Args.Time;
				if((mpColumn = m_tmaxColumns["Source"]) != null)
					mpColumn.Text = Args.Source;
				if((mpColumn = m_tmaxColumns["Items"]) != null)
					mpColumn.Text = Args.Items.ToString();
				
				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		
		/// <summary>This method sets the text for each column using the CTmaxMessage object provided by the caller</summary>
		/// <param name="tmaxMessage">The object used to set the column text</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Message</remarks>
		private bool SetColumns(FTI.Shared.Trialmax.CTmaxMessage tmaxMessage)
		{	
			CTmaxMessageCtrlColumn	mpColumn = null;
			bool					bSuccessful = false;
			
			Debug.Assert(tmaxMessage != null);
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				//	Set the image index
				//
				//	NOTE:	The first column is always used to set the row image
				if((mpColumn = m_tmaxColumns[0]) != null)
					mpColumn.Image = GetImageIndex(tmaxMessage.Level);

				if((mpColumn = m_tmaxColumns["Text"]) != null)
					mpColumn.Text = tmaxMessage.Text;
				
				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(FTI.Shared.Trialmax.CTmaxMessage tmaxMessage)
		
		/// <summary>This method sets the text for each column using the CTmaxDiagnosticArgs object provided by the caller</summary>
		/// <param name="Args">The object used to set the column text</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Diagnostic</remarks>
		private bool SetColumns(FTI.Shared.Trialmax.CTmaxDiagnosticArgs Args)
		{	
			CTmaxMessageCtrlColumn	mpColumn = null;
			bool				bSuccessful = false;
			
			Debug.Assert(Args != null);
			
			if(m_tmaxColumns == null) return false;
			if(m_tmaxColumns.Count == 0) return false;
			
			try
			{
				if((mpColumn = m_tmaxColumns["Time"]) != null)
					mpColumn.Text = Args.Time;
				if((mpColumn = m_tmaxColumns["Message"]) != null)
				{
					mpColumn.Text = Args.Message;
					
					if((Args.Exception != null) && (Args.Exception.Length > 0))
						mpColumn.Text += (" -> " + Args.Exception);
				}
				if((mpColumn = m_tmaxColumns["Name"]) != null)
					mpColumn.Text = Args.Name;
				if((mpColumn = m_tmaxColumns["Source"]) != null)
					mpColumn.Text = Args.Source;
				if((mpColumn = m_tmaxColumns["Items"]) != null)
					mpColumn.Text = Args.Items.ToString();
				
				bSuccessful = true;
			}
			catch
			{
			}
				
			return bSuccessful;
		
		}// private bool SetColumns(FTI.Shared.Trialmax.CTmaxDiagnosticArgs Args)
		
		/// <summary>This method will fill the column collection based on the current Format</summary>
		private void CreateColumns()
		{
			if(m_tmaxColumns == null) return;

			//	Clear the current collection
			m_tmaxColumns.Clear();
			
			//	What format are we using?
			switch(m_eFormat)
			{
				case TmaxMessageFormats.Version:
			
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Assembly"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Version"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Build"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Location"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Description"));
					break;
					
				case TmaxMessageFormats.SearchResult:
			
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Script"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Transcript"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Page"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Line"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Text"));
					break;
					
				case TmaxMessageFormats.Record:
			
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Type"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Barcode"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Name"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Description"));
					break;
					
				case TmaxMessageFormats.ErrorArgs:
			
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Message"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Source"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Time"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Items"));
					break;
					
				case TmaxMessageFormats.DiagnosticArgs:
			
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Time"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Message"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Name"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Source"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Items"));
					break;
					
				case TmaxMessageFormats.TextMessage:
			
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Text"));
					break;
					
				case TmaxMessageFormats.ImportMessage:
			
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Filename"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Line"));
					m_tmaxColumns.Add(new CTmaxMessageCtrlColumn("Message"));
					break;
					
				case TmaxMessageFormats.UserDefined:
				default:
				
					break;
			}
			
		}// private void CreateColumns()
		
		/// <summary>This method traps DoubleClick events sent to this window</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">system event arguments</param>
		private void OnDoubleClick(object sender, System.EventArgs e)
		{
			//	What format are we using?
			switch(m_eFormat)
			{
				case TmaxMessageFormats.ErrorArgs:
			
					foreach(ListViewItem O in m_ctrlMessages.SelectedItems)
					{
						if(O.Tag != null) 
						{
							try { m_tmaxEventSource.FireError(this, (CTmaxErrorArgs)(O.Tag)); }
							catch { }
						}
						
					}
					break;
					
				case TmaxMessageFormats.DiagnosticArgs:
			
					foreach(ListViewItem O in m_ctrlMessages.SelectedItems)
					{
						if(O.Tag != null) 
						{
							try { m_tmaxEventSource.FireDiagnostic(this, (CTmaxDiagnosticArgs)(O.Tag)); }
							catch { }
						}
						
					}
					break;
					
				case TmaxMessageFormats.Version:
				case TmaxMessageFormats.UserDefined:
				case TmaxMessageFormats.SearchResult:
				case TmaxMessageFormats.ImportMessage:
				default:
				
					break;
			}
			
			//	Clear the current contents
			if((m_bClearOnDblClick == true) && 
			   (m_ctrlMessages != null) && 
			   (m_ctrlMessages.Items != null))
				m_ctrlMessages.Items.Clear();
		
		}// private void OnDoubleClick(object sender, System.EventArgs e)

		/// <summary>This method adds a new row to the list</summary>
		/// <param name="Tag">The object to be assigned to the new row's Tag property</param>
		/// <returns>true if successful</returns>
		private bool Add(object Tag)
		{
			ListViewItem lvItem;
			
			//	Do we have any valid columns?
			if((m_tmaxColumns == null) || (m_tmaxColumns.Count == 0)) return false;
			
			//	Do we have a valid list control?
			if(m_ctrlMessages == null) return false;
		
			lvItem = new ListViewItem();
			
			if(Tag != null)
				lvItem.Tag = Tag;
			
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
				m_ctrlMessages.Items.Insert(0, lvItem);

				//	Do we need to make room?
				if(m_iMaxRows > 0)
				{
					while(m_ctrlMessages.Items.Count > m_iMaxRows)
					{
						try
						{
							m_ctrlMessages.Items.RemoveAt(m_ctrlMessages.Items.Count - 1);
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
				m_ctrlMessages.Items.Add(lvItem);

				//	Do we need to make room?
				if(m_iMaxRows > 0)
				{
					while(m_ctrlMessages.Items.Count > m_iMaxRows)
					{
						try
						{
							m_ctrlMessages.Items.RemoveAt(0);
						}
						catch
						{
							break;
						}
						
					}
					
				}
			}
			
			//	Automatically resize the columns to fit the text
			SuspendLayout();
			for(int i = 0; i < (int)m_tmaxColumns.Count; i++)
			{
				if(m_tmaxColumns[i].Header != null)
					m_tmaxColumns[i].Header.Width = -2;
			}
			ResumeLayout();
			
			return true;
		}
		
		/// <summary>This method is called to get the item created from the specified object</summary>
		/// <param name="O">The item used to create the row item</param>
		/// <returns>The associated list view item if found</returns>
		private ListViewItem GetItem(object O)
		{
			//	Do we have a valid list control and item collection?
			if(m_ctrlMessages == null) return null;
			if(m_ctrlMessages.Items == null) return null;
			if(m_ctrlMessages.Items.Count == 0) return null;
			
			foreach(ListViewItem lvi in m_ctrlMessages.Items)
			{
				if(lvi.Tag != null)
				{
					if(ReferenceEquals(lvi.Tag, O) == true)
						return lvi;
				}
				
			}
			
			return null;

		}// private ListViewItem GetItem(object O)
		
		/// <summary>This method updates the row created from the specified object</summary>
		/// <param name="O">The object used to create the row</param>
		/// <returns>true if successful</returns>
		private bool Update(object O)
		{
			ListViewItem	lvi = null;
			bool			bSuccessful = false;

			//	Get the row item created from this object
			if((lvi = GetItem(O)) != null)
			{
				try
				{
					//	Set the text for the first column
					lvi.Text = m_tmaxColumns[0].Text;
			
					//	Now add the sub items
					for(int i = 1; i < (int)m_tmaxColumns.Count; i++)
					{
						lvi.SubItems[i].Text = m_tmaxColumns[i].Text;
					}
					
					bSuccessful = true;

					//	Automatically resize the columns to fit the text
					SuspendLayout();
					for(int i = 0; i < (int)m_tmaxColumns.Count; i++)
					{
						if(m_tmaxColumns[i].Header != null)
							m_tmaxColumns[i].Header.Width = -2;
					}
					ResumeLayout();
				}
				catch
				{
				}
				
			}// if((lvi = GetItem(O)) != null)
			
			return bSuccessful;
			
		}// private bool Update(object O)
		
		/// <summary>Called to get the index of the image for the message to be added</summary>
		/// <param name="eLevel">Enumerated message error level identifier</param>
		/// <returns>The index of the image assigned to messages of this type</returns>
		private int GetImageIndex(TmaxMessageLevels eLevel)
		{
			int iIndex = -1;

			switch(eLevel)
			{
				case TmaxMessageLevels.FatalError:
					iIndex = 3;
					break;

				case TmaxMessageLevels.CriticalError:
					iIndex = 2;
					break;

				case TmaxMessageLevels.Warning:
					iIndex = 1;
					break;
				
				case TmaxMessageLevels.Information:
					iIndex = 0;
					break;

			}
			
			return iIndex;

		}// private int GetImageIndex(TmaxMessageLevels eLevel)
		
		#endregion Private Methods

		#region Properties

		/// <summary>This property exposes the list view control</summary>
		public System.Windows.Forms.ListView ListView
		{
			get { return m_ctrlMessages; }
		}
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
		}		
		
		/// <summary>The number of rows in the list</summary>
		public int Count
		{
			get 
			{ 
				if((m_ctrlMessages != null) && (m_ctrlMessages.Items != null))
					return m_ctrlMessages.Items.Count;
				else
					return 0;
			}
			
		}
		
		/// <summary>This property indicates the maximum number of rows allowed</summary>
		public int MaxRows
		{
			get { return m_iMaxRows; }
			set { m_iMaxRows = value; }
		}

		/// <summary>This property indicates if new items should be added to the top of the list
		///	instead of the bottom
		///</summary>
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

		/// <summary>This is the collection of columns used to build the pane</summary>
		public CTmaxMessageCtrlColumns Columns
		{
			get { return m_tmaxColumns; }
		}

		/// <summary>This property sets up the columns in the local collection</summary>
		public TmaxMessageFormats Format
		{
			get{ return m_eFormat; }
			set
			{
				//	Has the value changed?
				if(value != m_eFormat)
				{
					m_eFormat = value;
					
					Rebuild();
				}
				
			}
			
		}

		/// <summary>True to display the image column</summary>
		public bool ShowImage
		{
			get{ return m_bShowImage; }
			set
			{
				//	Has the value changed?
				if(value != m_bShowImage)
				{
					m_bShowImage = value;
					Rebuild();
				}
				
			}
			
		}

		/// <summary>True to show the column headers</summary>
		public bool ShowHeaders
		{
			get{ return m_bShowHeaders; }
			set
			{
				//	Has the value changed?
				if(value != m_bShowHeaders)
				{
					m_bShowHeaders = value;
					ShowColumnHeaders(m_bShowHeaders);
				}
				
			}
			
		}

		/// <summary>This method is called to get the index of the current selection</summary>
		public int SelectedIndex
		{
			get { return GetSelectedIndex(); }
			set { SetSelectedIndex(value); }
		}
		
		#endregion Properties
		
	}// class CTmaxMessageCtrl
	
	/// <summary>This class is used to define a column in the message pane</summary>
	public class CTmaxMessageCtrlColumn
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
		public CTmaxMessageCtrlColumn()
		{
			Initialize("", -2, HorizontalAlignment.Left);
		}
			
		/// <summary>Constructor</summary>
		/// <param name="strName">Name that appears in column header</param>
		public CTmaxMessageCtrlColumn(string strName)
		{
			Initialize(strName, -2, HorizontalAlignment.Left);
		}
			
		/// <summary>Constructor</summary>
		/// <param name="strName">Name that appears in column header</param>
		/// <param name="iWidth">Width of the column</param>
		public CTmaxMessageCtrlColumn(string strName, int iWidth)
		{
			Initialize(strName, iWidth, HorizontalAlignment.Left);
		}
			
		/// <summary>Constructor</summary>
		/// <param name="strName">Name that appears in column header</param>
		/// <param name="iWidth">Width of the column</param>
		/// <param name="eHorizontal">Horizontal text alignment</param>
		public CTmaxMessageCtrlColumn(string strName, int iWidth, HorizontalAlignment eHorizontal)
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

	}// public class CTmaxMessageCtrlColumn
		
	/// <summary>Objects of this class are used to manage a dynamic array of CTmaxMessageCtrlColumn objects</summary>
	public class CTmaxMessageCtrlColumns : CollectionBase
	{
			#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxMessageCtrlColumns()
		{
		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="mpColumn">CTmaxMessageCtrlColumn object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxMessageCtrlColumn Add(CTmaxMessageCtrlColumn mpColumn)
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
			
		}// Add(CTmaxMessageCtrlColumn mpColumn)

		/// <summary>
		/// This method is called to remove the requested filter from the collection
		/// </summary>
		/// <param name="mpColumn">The filter object to be removed</param>
		public void Remove(CTmaxMessageCtrlColumn mpColumn)
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
		public bool Contains(CTmaxMessageCtrlColumn mpColumn)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(mpColumn as object);
		}

		/// <summary>
		/// Called to locate the object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxMessageCtrlColumn Find(string strName)
		{
			// Search for the object with the same name
			foreach(CTmaxMessageCtrlColumn obj in base.List)
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
		public CTmaxMessageCtrlColumn this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (base.List[index] as CTmaxMessageCtrlColumn);
			}
		}

		/// <summary>
		/// Overloaded [] operator to locate the parameter object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxMessageCtrlColumn this[string strName]
		{
			get 
			{
				// Search for the object with the same name
				foreach(CTmaxMessageCtrlColumn obj in base.List)
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
		public int IndexOf(CTmaxMessageCtrlColumn value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

			#endregion Public Methods
		
	}//	CTmaxMessageCtrlColumns
			
	public enum TmaxMessageFormats
	{
		UserDefined = 0,
		ErrorArgs,
		DiagnosticArgs,
		Version,
		Record,
		SearchResult,
		TextMessage,
		ImportMessage,
	}
	
}// namespace FTI.Trialmax.Controls

