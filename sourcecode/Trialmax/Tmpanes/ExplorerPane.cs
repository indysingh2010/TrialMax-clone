using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win.UltraWinDock;

using plasmatech.scpax.interop;

using FTI.Shared.Win32;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// Summary description for ExplorerPane.
	/// </summary>
	public class CExplorerPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Error Identifiers
		
		const int ERROR_ADD_FILTER_EX			= (ERROR_BASE_PANE_MAX + 1);
		const int ERROR_SELECT_FILTER_EX		= (ERROR_BASE_PANE_MAX + 2);
		const int ERROR_GET_LIST_SELECTED_EX	= (ERROR_BASE_PANE_MAX + 3);
		const int ERROR_GET_TREE_SELECTED_EX	= (ERROR_BASE_PANE_MAX + 4);
		const int ERROR_GET_FILTER_SELECTION_EX	= (ERROR_BASE_PANE_MAX + 5);
		const int ERROR_GET_TOOL_EX				= (ERROR_BASE_PANE_MAX + 6);
		const int ERROR_TOOL_CLICK_EX			= (ERROR_BASE_PANE_MAX + 7);
		const int ERROR_TOOL_AFTER_DROP_EX		= (ERROR_BASE_PANE_MAX + 8);
		const int ERROR_TOOL_BEFORE_DROP_EX		= (ERROR_BASE_PANE_MAX + 9);
		const int ERROR_TOOL_CHECK_STATE_EX		= (ERROR_BASE_PANE_MAX + 10);
		const int ERROR_TOOL_ENABLE_EX			= (ERROR_BASE_PANE_MAX + 11);
		const int ERROR_SHOW_TREE_EX			= (ERROR_BASE_PANE_MAX + 12);
		const int ERROR_GET_COMMAND_PARAMS_EX	= (ERROR_BASE_PANE_MAX + 13);
		
		#endregion Error Identifiers
		
		#region Private Members
		
		/// <summary>Standard component container</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Infragistics library toolbar/menu manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlUltraToolbarManager;
		
		/// <summary>Infragistics library toolbar/menu manager left-side docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CExplorerPane_Toolbars_Dock_Area_Left;
		
		/// <summary>Infragistics library toolbar/menu manager right-side docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CExplorerPane_Toolbars_Dock_Area_Right;
		
		/// <summary>Infragistics library toolbar/menu manager top docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CExplorerPane_Toolbars_Dock_Area_Top;
		
		/// <summary>Infragistics library toolbar/menu manager bottom docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CExplorerPane_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Local member bound to Filters property</summary>
		private CExplorerPaneFilters m_aFilters = new CExplorerPaneFilters();
		
		/// <summary>Local member bound to SourceType property</summary>
		private FTI.Shared.Trialmax.RegSourceTypes m_eSourceType = FTI.Shared.Trialmax.RegSourceTypes.Document;
		
		/// <summary>Image list used for toolbar buttons</summary>
		private System.Windows.Forms.ImageList m_ctrlSmallImages;

		/// <summary>Flag used to indicate if action was initiated via the folder tree</summary>
		private bool m_bTreeAction = false;

		/// <summary>Flag used to indicate if hidden folders and files should be displayed</summary>
		private bool m_bShowHidden = false;

		/// <summary>Flag used to indicate if the split screen view should be used</summary>
		private bool m_bShowTree = true;

		/// <summary>Local flag to control file/folder searching</summary>
		private bool m_bTreeSelection = true;

		/// <summary>Member used to store the ratio of the tree and list widths</summary>
		private float m_fCtrlWidthRatio = 0.50F;

		/// <summary>Member used to store the list of items associated with the last event fired by the control</summary>
		private CTmaxSourceFolder m_tmaxSourceFolder = new CTmaxSourceFolder();

		/// <summary>Member used to store the collection of items representing the user selections pending registration</summary>
		private CTmaxSourceFolder m_tmaxSelections = new CTmaxSourceFolder();

		/// <summary>Drive/Folder combobox control</summary>
		private plasmatech.scpax.AxPTxShCombo m_ptxShComboBox;

		/// <summary>Panel used to dock the folder tree control</summary>
		private System.Windows.Forms.Panel m_ctrlShTreePanel;

		/// <summary>Shell control pack folder tree control</summary>
		private plasmatech.scpax.AxPTxShTree m_ptxShTree;

		/// <summary>Splitter control separating the folder tree and files list controls</summary>
		private System.Windows.Forms.Splitter m_ctrlSplitter;

		/// <summary>Shell control pack files list control</summary>
		private plasmatech.scpax.AxPTxShList m_ptxShList;

		/// <summary>Panel control used to dock the drive/folders combobox</summary>
		private System.Windows.Forms.Panel m_ctrlShComboPanel;
		
		/// <summary>Command identifier for Trialmax-Morph command added to the shell menu</summary>
		private int m_iShellCommandId;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CExplorerPane() : base()
		{
			//	Initialize the child windows
			InitializeComponent();

			//	Link the files list box to the folder tree
			m_ptxShTree.ShList = (plasmatech.scpax.interop.PTxShList)m_ptxShList.GetOcx();
			m_ptxShComboBox.ShTree = (plasmatech.scpax.interop.PTxShTree)m_ptxShTree.GetOcx();
		
			//m_ptxShTree.SelectedFolder.PathName = "d:\\";
			
			SetShowHidden(m_bShowHidden);
			
		}

		/// <summary>
		/// This method is called to add a filter specification to the files filter drop down
		/// </summary>
		/// <param name="objFilter">CExplorerPaneFilter object used to define the filter</param>
		/// <returns>true if successful</returns>
		public bool AddFilter(CExplorerPaneFilter objFilter)
		{
			ComboBoxTool	ctrlFilters;
			ValueListItem	vlItem;
			
			//	Do we have a valid object and collection?
			if((objFilter == null) || (m_aFilters == null)) return false;
			
			try
			{
				//	Do we have a valid filter control?
				if((ctrlFilters = (ComboBoxTool)GetTool("FileFilter")) != null)
				{
					if((ctrlFilters.ValueList != null) && (ctrlFilters.ValueList.ValueListItems != null))
					{
						if((vlItem = ctrlFilters.ValueList.ValueListItems.Add(objFilter, objFilter.Text)) != null)
						{
							vlItem.Appearance.Image = objFilter.Image;
							m_aFilters.Add(objFilter);
							return true;
						}

					}
				
				}// if((ctrlFilters = (ComboBoxTool)GetTool("FileFilter")) != null)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddFilter", m_tmaxErrorBuilder.Message(ERROR_ADD_FILTER_EX, objFilter.Text), Ex);
			}
			
			return false;
		}
		
		/// <summary>
		/// This method is called to add a filter specification to the files filter drop down
		/// </summary>
		/// <param name="eSourceType">Source type identifier</param>
		/// <param name="strText">Display text</param>
		/// <param name="strExtensions">Delimited extensions string</param>
		/// <param name="iImage">Image index</param>
		/// <returns>true if successful</returns>
		public bool AddFilter(RegSourceTypes eSourceType, string strText, string strExtensions, int iImage)
		{
			CExplorerPaneFilter Filter = new CExplorerPaneFilter(eSourceType, strText, strExtensions, iImage);
			
			if(Filter != null)
			{
				if(AddFilter(Filter) == true)
				{
					//	Is this the current SourceType?
					if(Filter.SourceType == m_eSourceType)
						SetActiveFilter(m_eSourceType);
				
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>This method is set the files filter selection</summary>
		/// <param name="eSourceType">Source type identifier assocaited with the filter</param>
		/// <returns>true if successful</returns>
		public bool SetActiveFilter(RegSourceTypes eSourceType)
		{
			CExplorerPaneFilter	objFilter = null;
			
			//	Locate the specified filter
			if(m_aFilters != null)
				objFilter = m_aFilters.Find(eSourceType);

			if(objFilter != null) 
				return SetActiveFilter(objFilter);
			else
				return false;
		}
		
		/// <summary>
		/// This method is set the files filter selection
		/// </summary>
		/// <param name="objFilter">Filter object to be activated</param>
		/// <returns>true if successful</returns>
		public bool SetActiveFilter(CExplorerPaneFilter objFilter)
		{
			ComboBoxTool		ctrlFilters = (ComboBoxTool)GetTool("FileFilter");
			CExplorerPaneFilter objCurrent = null;
			ValueListItem		vlItem;
			
			//	Do we have a valid combo box?
			if(ctrlFilters == null) return false;
			
			//	Use the first filter in the list if none specified
			if(objFilter == null)
			{
				if(m_aFilters.Count > 0)
				{
					if((objFilter = m_aFilters[0]) == null)
						return false;
				}
				else
				{
					return false;
				}
			}
			
			try
			{
				//	Get the current selection
				if((vlItem = (ValueListItem)ctrlFilters.SelectedItem) != null)
				{
					objCurrent = (CExplorerPaneFilter)vlItem.DataValue;
				}
				
				//	Has the value actually changed?
				if((objCurrent == null) || (objCurrent.SourceType != objFilter.SourceType))
				{
					foreach(ValueListItem O in ctrlFilters.ValueList.ValueListItems)
					{
						if(((CExplorerPaneFilter)O.DataValue).SourceType == objFilter.SourceType)
						{
							ctrlFilters.SelectedItem = O;
							break;
						}
					}
					
				}
	
				//	Set the filter in the list box
				if(m_ptxShList != null)
				{
//					if(objFilter.SourceType == RegSourceTypes.AllFiles)
//					{
//						m_ptxShList.FileFilter = m_aFilters.Extensions;
//						m_tmaxEventSource.FireDiagnostic(this, "SAF", m_ptxShList.FileFilter);
//					}
//					else
						m_ptxShList.FileFilter = objFilter.Extensions;
						
				}
					
				//	Update the SourceType property
				m_eSourceType = objFilter.SourceType;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetActiveFilter", m_tmaxErrorBuilder.Message(ERROR_SELECT_FILTER_EX, objFilter.Text), Ex);
			}
			
			return false;
		
		}// SetActiveFilter(CExplorerPaneFilter objFilter)
		
		/// <summary>This method handles notifications from the application when it is about to load a new screen layout</summary>
		public override void OnBeforeLoadLayout()
		{
			//	Make sure the shell controls are invisible for this action
			//
			//	See remarks for SetAxVisible() method
			SetAxVisible(false);
			
		}// public virtual void OnBeforeLoadLayout()
		
		/// <summary>
		/// This method is called to get the current filter selection
		/// </summary>
		/// <returns>CExplorerPaneFilter associated with the current selection</returns>
		public CExplorerPaneFilter GetFilterSelection()
		{
			ComboBoxTool	ctrlFilters;
			ValueListItem	vlItem;
			
			try
			{
				//	Do we have a valid filter control?
				if((ctrlFilters = (ComboBoxTool)GetTool("FileFilter")) != null)
				{
					if((vlItem = (ValueListItem)ctrlFilters.SelectedItem) != null)
					{
						return (CExplorerPaneFilter)vlItem.DataValue;
					}
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetFilterSelection", m_tmaxErrorBuilder.Message(ERROR_GET_FILTER_SELECTION_EX), Ex);
			}
			
			return null;
			
		}//GetFilterSelection()
		
		#endregion Public Methods

		#region Private Methods
		
		/// <summary>
		/// This method will retrieve the current file list selections and use them to populate
		/// the local collection of event items
		/// </summary>
		/// <returns>true if successful</returns>
		private bool GetListSelections()
		{
			IPTxShListSearch ShListSearch = null;
			
			if((m_tmaxSelections == null) || (m_ptxShList == null)) return false;
			
			//	Do we have a folder selection?
			if((m_ptxShList.Folder == null) || (m_ptxShList.Folder.PathName.Length == 0)) return false;
				
			//	Clear the existing selections
			m_tmaxSelections.Reset();
			
			//	Prevent searching for files and subfolders
			m_bTreeSelection = false;
				
			try
			{
				//	Initialize the parent folder
				m_tmaxSelections.Initialize(m_ptxShList.Folder.PathName);
			
				//	Search for all selected items
				ShListSearch = m_ptxShList.StartSearch();
				ShListSearch.Selected = true;
				
				//	Add an item for each selection
				foreach(IPTxShListItem ShListItem in ShListSearch)  
				{
					//	Make sure this item represents a valid file or folder
					if((ShListItem.IsFileSystem == true) && (ShListItem.PathName.Length > 0))
					{
						//	Is this a subfolder?
						if(ShListItem.IsFolder == true)
						{
							m_tmaxSelections.SubFolders.Add(new CTmaxSourceFolder(ShListItem.PathName));
						}
						else
						{
							m_tmaxSelections.Files.Add(new CTmaxSourceFile(ShListItem.PathName));
						}
					}
				
				}// foreach(IPTxShListItem ShListItem in ShListSearch)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetListSelections", m_tmaxErrorBuilder.Message(ERROR_GET_LIST_SELECTED_EX), Ex);
			}
			
			return ((m_tmaxSelections.SubFolders.Count > 0) || (m_tmaxSelections.Files.Count > 0));
			
		}// GetListSelections()
		
		/// <summary>
		/// This method sets the local selection folder to match the current
		///	selection in the folder tree
		/// </summary>
		/// <returns>true if successful</returns>
		private bool GetTreeSelection()
		{
			if((m_tmaxSelections == null) || (m_ptxShTree == null)) return false;
			
			//	Clear the existing items
			m_tmaxSelections.Reset();
			
			//	Enable searching for files and subfolders
			m_bTreeSelection = true;
			
			try
			{
				IPTxShTreeNode ShTreeNode = m_ptxShTree.SelectedNode;
				
				//	Make sure this item represents a valid file or folder
				if((ShTreeNode.IsFileSystem == true) && (ShTreeNode.PathName.Length > 0))
				{
					//	Allocate a new event item
					//
					//	NOTE:	We do not display files in the folder tree so all
					//			selections must represent folders
					Debug.Assert(ShTreeNode.IsFolder == true);
					m_tmaxSelections.Initialize(ShTreeNode.PathName);
					
					return true;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTreeSelected", m_tmaxErrorBuilder.Message(ERROR_GET_TREE_SELECTED_EX), Ex);
			}
			
			return false;
		
		}// GetTreeSelected()
		
		/// <summary>
		/// This function is called to set the current selection to match 
		///	the folder path specified by the caller
		/// </summary>
		/// <param name="Files">List of path selections provided by shell</param>
		/// <returns>true if successful</returns>
		private bool GetTreeSelection(IPTxStrings Files)
		{
			//	Make sure we have all the necessary objects
			if(m_tmaxSelections == null) return false;
			if(Files == null) return false;
			if(Files.Count() == 0) return false;
			if(m_ptxShTree == null) return false;
			if(m_ptxShTree.Nodes == null) return false;
		
			//	Clear the existing selection folder
			m_tmaxSelections.Reset();
			
			//	Enable searching for files and subfolders
			m_bTreeSelection = true;
			
			//	There should only be one selection in the list
			Debug.Assert(Files.Count() == 1);
			
			//	Locate this path in the nodes list
			foreach(IPTxShTreeNode ShTreeNode in m_ptxShTree.Nodes)
			{
				if((ShTreeNode.PathName == (string)Files[0]) && (ShTreeNode.IsFileSystem == true) && (ShTreeNode.IsFolder == true))
				{
					//	Initialize the selection
					m_tmaxSelections.Initialize(ShTreeNode.PathName);
					return true;
					
				}// if((ShTreeNode.PathName == strFile) && (ShTreeNode.IsFileSystem == true))
			}

			return false;
		}
		
		/// <summary>This method is called to get the parameters required for the specified command event</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <returns>The collection of parameters required for the event</returns>
		private CTmaxParameters GetCommandParameters(TmaxCommands eCommand)
		{
			CExplorerPaneFilter	Filter = GetFilterSelection();
			CTmaxParameters		tmaxParameters = null;
			
			try
			{
				tmaxParameters = new CTmaxParameters();
				
				//	Add the media type associated with the current filter
				if(Filter != null)
					tmaxParameters.Add(TmaxCommandParameters.RegSourceType, (int)Filter.SourceType); 
				else
					tmaxParameters.Add(TmaxCommandParameters.RegSourceType, (int)RegSourceTypes.AllFiles); 
			
				//	Add the command specified parameters
				switch(eCommand)
				{
					case TmaxCommands.RegisterSource:
					
						tmaxParameters.Add(TmaxCommandParameters.RegSourceCount, m_tmaxSourceFolder.GetFileCount(true));
						break;
						
					default:
					
						break;
						
				}// switch(eCommand)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetCommandParameters", m_tmaxErrorBuilder.Message(ERROR_GET_COMMAND_PARAMS_EX, eCommand), Ex);
			}
			
			return tmaxParameters;
				
		}// GetCommandParameters(TmaxCommands eCommand)
		
		/// <summary>This method is called to get the items required for the specified command event</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <returns>The collection of items required for the event</returns>
		private CTmaxItems GetCommandItems(TmaxCommands eCommand)
		{
			CTmaxItems	tmaxItems = null;
			CTmaxItem	tmaxItem = null;
	
			//	Build the list of event items
			if((tmaxItems = new CTmaxItems()) != null)
			{
				if((tmaxItem = new CTmaxItem()) != null)
				{
					//	Assign the appropriate source folder
					if(eCommand == TmaxCommands.RegisterSource)
					{
						m_tmaxSourceFolder.Anchor = m_bTreeSelection;
						tmaxItem.SourceFolder = m_tmaxSourceFolder;
					}
					else
					{
						tmaxItem.SourceFolder = m_tmaxSelections;
					}
					
					tmaxItems.Add(tmaxItem);
				}
			
			}// if((tmaxItems = new CTmaxItems()) != null)
			
			return tmaxItems;
	
		}// GetCommandItems(TmaxCommands eCommand)

		/// <summary>
		/// This method will expand the current selection(s) to include all 
		///	files that should be registered
		/// </summary>
		/// <return>The total number of files to be registered</return>
		private long GetSourceFiles()
		{
			CExplorerPaneFilter Filter = GetFilterSelection();
			CTmaxSourceFiles	tmaxFiles = null;
			string				strFilter = null;
			string []			aExtensions = null;
			
			//	Make sure we have valid collections
			if((m_tmaxSelections == null) || (m_tmaxSourceFolder == null)) return 0;
			
			//	Get the list of valid extensions
			if((Filter != null) && (Filter.SourceType != RegSourceTypes.AllFiles))
				strFilter = Filter.Extensions.ToLower();
			else
				strFilter = m_aFilters.Extensions;// Use all valid extensions

			if(strFilter != null)
			{
				aExtensions = strFilter.Split(';');
			}
			
			//	Display the wait cursor
			Cursor.Current = Cursors.WaitCursor;
			
			//	Clear the existing source folder
			m_tmaxSourceFolder.Reset();
			
			//	Start by verifying that all current file selections pass the filter test
			if(m_tmaxSelections.Files.Count > 0)
			{
				//	Transfer the files to a temporary collection
				tmaxFiles = new CTmaxSourceFiles();
				foreach(CTmaxSourceFile O in m_tmaxSelections.Files)
					tmaxFiles.Add(O);
				m_tmaxSelections.Files.Clear();
				
				//	Now check each file against the filter before putting it back in the selected collection
				foreach(CTmaxSourceFile O in tmaxFiles)
				{
					if(CheckExtension(O.Name, aExtensions) == true)
						m_tmaxSelections.Files.Add(O);
				}
				
				//	Flush the temporary collection
				tmaxFiles.Clear();				
			}
				
			//	Should we search the parent folder?
			//
			//	NOTE:	If the current selections were retrieved from the files list
			//			control then we do not bother to search the parent
			if(m_bTreeSelection == true)
			{
				//	Locate all files and subfolders
				Search(m_tmaxSelections, aExtensions);
			}
			else
			{
				//	Always search the subfolders when selected from the list control
				foreach(CTmaxSourceFolder tmaxFolder in m_tmaxSelections.SubFolders)
				{
					//	Search for any files and/or subfolders
					Search(tmaxFolder, aExtensions);
				}
			
			}
			
			//	Reset the selections
			m_tmaxSourceFolder = m_tmaxSelections;
			m_tmaxSelections = new CTmaxSourceFolder();

			//	Clear the wait cursor
			Cursor.Current = Cursors.Default;
			
			return m_tmaxSourceFolder.GetFileCount(true);
				
		}// GetSourceFiles()
		
		/// <summary>This method is called to get the combobox containing the file filters</summary>
		/// <returns>The filters combobox tool</returns>
		public ComboBoxTool GetFiltersBox()
		{
			ComboBoxTool ctrlFilters = null;
			
			try
			{
				ctrlFilters = (ComboBoxTool)GetTool("FileFilter");
			}
			catch
			{
			}
			
			return ctrlFilters;
		}
		
		/// <summary>
		/// This method will search for files and subfolders in the specified folder
		/// </summary>
		/// <param name="tmaxFolder">The source folder to be searched</param>
		/// <param name="strFilter">The file extensions allowed for the current registration type</param>
		/// <returns>true if successful</returns>
		private bool Search(CTmaxSourceFolder tmaxSearch, string [] aExtensions)
		{
			CTmaxSourceFolder	tmaxSubFolder;
			CTmaxSourceFile		tmaxFile;
			string				strSearchPath;
			bool				bSubFolders = false;
			
			//	Make sure we have valid collections
			if((tmaxSearch == null) || (tmaxSearch.Files == null) || 
			   (tmaxSearch.SubFolders == null)) return false;
		
			//	Are we supposed to search for subfolders?
			if((m_tmaxRegOptions != null) && (m_tmaxRegOptions.GetFlag(RegFlags.IncludeSubfolders) == true))
				bSubFolders = true;
				
			//	Construct the search path
			//
			//	NOTE:	We have to add a trailing backslash because of
			//			a problem in .NET If you pass it C: and ask for all
			//			directories, it will return values without the backslash
			//			in between the folder and drive (e.g. c:temp instead of c:\temp)
			strSearchPath = tmaxSearch.Path;
			if(strSearchPath.EndsWith("\\") == false)
				strSearchPath += "\\";
				
			//	Get the collection of files
			string [] aFiles = System.IO.Directory.GetFiles(strSearchPath);
			foreach(string strFilename in aFiles)
			{
				//	Make sure this file has a valid extension
				if(CheckExtension(strFilename, aExtensions) == true)
				{
					//	Allocate a new file and add it to the collection
					tmaxFile = new CTmaxSourceFile(strFilename);
					tmaxSearch.Files.Add(tmaxFile);
				}
			
			}
			
			//	Do we want to iterate subfolders also?
			if(bSubFolders == true)
			{
				string [] aFolders = System.IO.Directory.GetDirectories(strSearchPath);
				foreach(string strFolder in aFolders)
				{
					//	Allocate and initialize a new subfolder
					if((tmaxSubFolder = new CTmaxSourceFolder(strFolder)) != null)
					{
						//	Search for this folder's children
						if(Search(tmaxSubFolder, aExtensions) == true)
						{				
							//	Add it to the subfolders collection
							tmaxSearch.SubFolders.Add(tmaxSubFolder);
						}
						
					}

				}// foreach(string strFolder in aFolders)
				
			}// if(bSubFolders == true)                   
			
			return true;	
				
		}// Search(CTmaxItem tmaxFolder, string strExtension)
		
		/// <summary>This method will determine if the specified file has a valid extension</summary>
		/// <param name="strFilename">The file to be checked</param>
		/// <param name="aExtensions">The array of valid file extentsions</param>
		/// <returns>true if the file passes the test</returns>
		private bool CheckExtension(string strFilename, string [] aExtensions)
		{
			string strExtension;
			
			Debug.Assert(strFilename != null);
		
			//	Make sure this file has a valid extension
			strExtension = System.IO.Path.GetExtension(strFilename);
			if(strExtension.Length == 0) return false;
				
			//	Did the caller specify an array of allowed extensions?
			if((aExtensions != null) && (aExtensions.GetUpperBound(0) >= 0))
			{
				//	Our filter extensions include the wildcard character
				strExtension = ("*" + strExtension.ToLower());

				foreach(string s in aExtensions)
				{
					//	Does this extension match?
					if(strExtension == s)
						return true;
				}
				
				//	Must not have been able to find the extension
				return false;
			
			}// if((aExtensions != null) && (aExtensions.GetUpperBound(0) >= 0))
			else
			{
				//	No extensions to test against
				return true;
			}
				
		}// private bool CheckExtension(string strFilename, string [] aExtensions)
		
		/// <summary>This method is called to fire a command event</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		protected override bool FireCommand(TmaxCommands eCommand)
		{
			return FireCommand(eCommand, GetCommandItems(eCommand), GetCommandParameters(eCommand));
		}
		
		/// <summary>This method is called to fire a RegisterSource command event</summary>
		/// <returns>true if successful</returns>
		protected bool FireRegisterSource()
		{
			//	Make sure we have valid selections
			if(m_tmaxSelections != null)
			{
				//	Build the hiearchy of source files
				if(GetSourceFiles() > 0)
				{				
					//	Fire the register request
					return FireCommand(TmaxCommands.RegisterSource);
				}
				else
				{
					MessageBox.Show("Unable to locate any valid media files that match the selected registration type", "Error",
								    MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				
			}
			
			return false;
		}
		
		/// <summary>This method retrieves the image list index associated with the specified source type</summary>
		/// <param name="tmaxSourceType">The desired registration source type</param>
		/// <returns>The index of the associated image</returns>
		private int GetFilterImage(CTmaxSourceType tmaxSourceType)
		{
			switch(tmaxSourceType.RegSourceType)
			{
				case RegSourceTypes.Document:	return 10;
				case RegSourceTypes.Powerpoint:	return 11;
				case RegSourceTypes.Recording:	return 12;
				case RegSourceTypes.Deposition:	return 16;
				case RegSourceTypes.Adobe:		return 17;
				case RegSourceTypes.AllFiles:	return 9;
				default:						return -1;
			}
			
		}// private int GetFilterImage(CTmaxSourceType tmaxSourceType)
		
		/// <summary>This function will retrieve the tool with the specified key from the toolbar manager</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		private ToolBase GetTool(string strKey)
		{
			ToolBase Tool = null;
					
			try
			{
				if(m_ctrlUltraToolbarManager != null)
				{
					Tool = m_ctrlUltraToolbarManager.Tools[strKey];
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTool", m_tmaxErrorBuilder.Message(ERROR_GET_TOOL_EX, strKey), Ex);
			}
			return Tool;
		
		}// GetTool()
				
		/// <summary>
		/// This method handles events fired by the file list control when the use selects an item in the file list shell context menu
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Shell Control Pack file list event object</param>
		private void OnListAfterShellMenuSelection(object sender, plasmatech.scpax.IPTxShListEvents_OnAfterShellMenuSelectionEvent e)
		{
			if(e.aSelectedMenuItemId == m_iShellCommandId)
			{
				ShowTrialmaxPopup(false);
			}
			
		}// OnListAfterShellMenuSelection

		/// <summary>
		/// This method handles events fired by the folder tree control when the use selects an item in the file list shell context menu
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Shell Control Pack folder tree event object</param>
		private void OnTreeAfterShellMenuSelection(object sender, plasmatech.scpax.IPTxShTreeEvents_OnAfterShellMenuSelectionEvent e)
		{
			if(e.aSelectedMenuId == m_iShellCommandId)
				ShowTrialmaxPopup(true);
		
		}// OnTreeAfterShellMenuSelection

		/// <summary>
		/// This method handles events fired by the file list control when the use brings up the shell's context menu
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Shell Control Pack file list event object</param>
		private void OnListBeforeShellMenuPopup(object sender, plasmatech.scpax.IPTxShListEvents_OnBeforeShellMenuPopupEvent e)
		{
			IPTxMenuItem subMenu;
			
			// Add a Trialmax selection to the shell menu
			m_iShellCommandId = e.aNextCustomMenuId;
			subMenu = e.aMenu.MenuItems.InsertItem(0, m_iShellCommandId, "Trialmax");
			e.aMenu.MenuItems.InsertItem(1,0,"-"); // special caption "-" means insert a seperator
		
		}// OnListBeforeShellMenuPopup
		
		/// <summary>
		/// This method handles events fired by the folder tree control when the use brings up the shell's context menu
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Shell Control Pack folder tree event object</param>
		private void OnTreeBeforeShellMenuPopup(object sender, plasmatech.scpax.IPTxShTreeEvents_OnBeforeShellMenuPopupEvent e)
		{
			IPTxMenuItem subMenu;
			
			// Add a Trialmax selection to the menu
			m_iShellCommandId = e.aNextCustomMenuId;
			subMenu = e.aMenu.MenuItems.InsertItem(0, m_iShellCommandId, "Trialmax");
			e.aMenu.MenuItems.InsertItem(1,0,"-"); // special caption "-" means insert a seperator

		}// OnTreeBeforeShellMenuPopup

		/// <summary>
		/// This function handles events fired by the list view control when the
		///	user clicks within its client area
		/// </summary>
		/// <param name="sender">Object (listview) firing the event</param>
		/// <param name="e">System event parameter object</param>
		private void OnListMouseDown(object sender, plasmatech.scpax.IPTxShListEvents_OnMouseDownEvent e)
		{
			bool bNoItem;

			try
			{
				//	Is the mouse on an item in the list?
				bNoItem = m_ptxShList.HitTest(e.ax, e.ay) == null  ||  m_ptxShList.HitTestInfo(e.ax, e.ay).Nowhere;
	      
				//	Must be right click with no item and/or selection
				if((e.aButton == 2)  &&  (bNoItem == true) &&  (m_ptxShList.Selected == null))
				{
					//	Display the menu at the current mouse position
					ShowTrialmaxPopup(false);
				}
			}
			catch
			{
			}
		
		}//	OnListMouseDown()

		/// <summary>
		/// This function handles events fired by the list view control when the
		///	user starts dragging an item
		/// </summary>
		/// <param name="sender">Object (listview) firing the event</param>
		/// <param name="e">System event parameter object</param>
		private void OnListStartDrag(object sender, plasmatech.scpax.IPTxShListEvents_OnOleStartDragEvent e)
		{
			//	Can't drag unless the database is open
			if((m_tmaxSelections != null) && (m_tmaxDatabase != null))
			{
				//	Are any items selected?
				if(GetListSelections() == true)
				{
					//	Let the drop target set the effect
					e.aAllowedEffects = (int)(DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move);
					
					//	Fire the start drag event
					FireCommand(TmaxCommands.StartDrag);			
				}
				else
				{
					e.aAllowedEffects = (int)DragDropEffects.None;			
				}
				
			}// if(m_tmaxItems != null)
			else
			{
				e.aAllowedEffects = (int)DragDropEffects.None;			
			}
			
		}// OnListStartDrag(object sender, plasmatech.scpax.IPTxShListEvents_OnOleStartDragEvent e)

		/// <summary>
		/// This function handles events fired by the list view control when the
		///	user stops dragging an item
		/// </summary>
		/// <param name="sender">Object (listview) firing the event</param>
		/// <param name="e">System event parameter object</param>
		private void OnListCompleteDrag(object sender, plasmatech.scpax.IPTxShListEvents_OnOleCompleteDragEvent e)
		{
			OnCompleteDrag((DragDropEffects)e.aEffect);
						
		}// OnListCompleteDrag(object sender, plasmatech.scpax.IPTxShListEvents_OnOleCompleteDragEvent e)

		/// <summary>
		/// This method is called to request registration of the current selections
		/// </summary>
		/// <param name="bIgnoreFiles">true to ignore the file selections and use only the tree selection</param>
		private void OnRegister(bool bIgnoreFiles)
		{
			//	Flush the existing selections
			m_tmaxSelections.Reset();
				
			//	Are we looking only at the active folder?
			if(bIgnoreFiles)
			{
				//	Get the user selections from the folder tree control
				GetTreeSelection();
			}
			else
			{
				//	Get the file selections
				if(GetListSelections() == false)
				{
					GetTreeSelection();
				}
			
			}
		
			//	Should we display the registration options?
			if((m_tmaxRegOptions != null) && 
			   (m_tmaxRegOptions.GetFlag(RegFlags.ShowOnDrop) == true))
			{
				if(ShowRegistrationOptions() == false)
					return;
			}
			
			//	Fire the event
			FireRegisterSource();

		}// OnRegister(bool bIgnoreFiles)

		/// <summary>
		/// This method is called when the user stops dragging source files
		/// </summary>
		/// <param name="eEffects">drag drop effects returned by system</param>
		private void OnCompleteDrag(DragDropEffects eEffects)
		{
			DragDropEffects eMask = (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move);
			
			//	Notify the application that the user has stopped dragging
			FireCommand(TmaxCommands.CompleteDrag);
			
			//	Did the user cancel the operation?
			if((eEffects & eMask) == 0)
			{
				//	Clear the drag selections
				m_tmaxSelections.Reset();
			}
			else
			{		
				//	Should we invoke the options dialog?
				if((m_tmaxRegOptions != null) && (m_tmaxRegOptions.GetFlag(RegFlags.ShowOnDrop) == true))
				{
					if(ShowRegistrationOptions() == true)
						FireRegisterSource();
					else
						m_tmaxSelections.Reset();
				}
				else
				{
					// Fire the event
					FireRegisterSource();
				}
			}
			
		}// OnCompleteDrag()

		/// <summary>This method is called to show the form that allows the user to select the registration options</summary>
		///	<returns>true if the user accepts the changes</returns>
		private bool ShowRegistrationOptions()
		{
			CFRegOptions	regOptions = null;
			bool			bAccepted = false;
			
			//	Don't bother if no options are available
			if(m_tmaxRegOptions == null) return false;
			
			try
			{
				regOptions = new CFRegOptions();

				//	Set the options used by the form
				regOptions.Options = m_tmaxRegOptions;
			
				if(regOptions.ShowDialog(this) == DialogResult.OK)
					bAccepted = true;			
			}
			catch
			{
			}
			
			//	Force cleanup of the form
			if(regOptions != null)
				regOptions.Dispose();
				
			return bAccepted;
			
		}// ShowRegistrationOptions()

		/// <summary>This function handles events fired when the user drags the splitter bar</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">event arguments</param>
		private void OnMoveSplitter(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			try
			{
				if((m_ctrlShTreePanel != null) && (m_ctrlShTreePanel.IsDisposed == false))
				{
					if((m_ctrlShTreePanel.Width > 0) && (e.X > 0))
					{
						m_fCtrlWidthRatio = ((float)(m_ptxShTree.Width)) / ((float)(m_ctrlShTreePanel.Width));
					
					m_tmaxEventSource.FireDiagnostic(this, "MS", "OMS: " + m_fCtrlWidthRatio.ToString());
					}
				}
				
			}
			catch
			{
			}
			
		}
		
		/// <summary>
		/// This function handles events fired by the folder tree control when the
		///	user starts dragging an item
		/// </summary>
		/// <param name="sender">Object (folder tree) firing the event</param>
		/// <param name="e">System event parameter object</param>
		private void OnTreeStartDrag(object sender, plasmatech.scpax.IPTxShTreeEvents_OnOleStartDragEvent e)
		{
			bool bSelection = false;
			
			//	Can't drag unless the database is open
			if((m_tmaxSelections != null) && (m_tmaxDatabase != null))
			{
				//	Use files specified in the event if available
				//
				//	NOTE:	If the user drags a folder that is not currently selected
				//			the control does NOT change the selection. Therefore, it is 
				//			not going to work if we just retrieve the current selections
				if((e.aData == null) || (e.aData.Files == null) || (e.aData.Files.Count() == 0))
				{
					//	Get the user selections from the folder tree control
					bSelection = GetTreeSelection();
				}
				else
				{
					//	Get the drag selections 
					//
					//	NOTE:	These might not be the same as the actual selection in the tree
					bSelection = GetTreeSelection(e.aData.Files);
				}
				
				//	Are any items selected?
				if(bSelection == true)
				{
					//	Let the drop target set the effect
					e.aAllowedEffects = (int)(DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move);
					
					//	Fire the start drag event
					FireCommand(TmaxCommands.StartDrag);			
				}
				else
				{
					e.aAllowedEffects = (int)DragDropEffects.None;			
				}
				
			}// if(m_tmaxSelections != null)
			else
			{
				e.aAllowedEffects = (int)DragDropEffects.None;			
			}
	
		}// OnTreeStartDrag()

		/// <summary>
		/// This function handles events fired by the folder tree control when the
		///	user stops dragging an item
		/// </summary>
		/// <param name="sender">Object (folder tree) firing the event</param>
		/// <param name="e">System event parameter object</param>
		private void OnTreeCompleteDrag(object sender, plasmatech.scpax.IPTxShTreeEvents_OnOleCompleteDragEvent e)
		{
			OnCompleteDrag((DragDropEffects)e.aEffect);
						
		}// OnTreeCompleteDrag()

		/// <summary>
		/// This function traps events fired when the user clicks on a 
		///	toolbar button or menu button
		/// </summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Tool click event parameters</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			//	Are we supposed to ignore this click?
			if(m_bIgnoreUltraEvents) return;
			
			try
			{
				//	Which tool?
				switch(e.Tool.Key)
				{
					case "MoveUp": 

						m_ptxShList.GoUp(1);
						break;

					case "ViewIcons":   
						
						m_ptxShList.ViewStyle = plasmatech.scpax.interop.TPTxViewStyle.ptvsIcon;
						break;

					case "ViewList":
						
						m_ptxShList.ViewStyle = plasmatech.scpax.interop.TPTxViewStyle.ptvsList;
						break;

					case "ViewDetails":
						
						m_ptxShList.ViewStyle = plasmatech.scpax.interop.TPTxViewStyle.ptvsReport;
						break;

					case "ViewThumbnails":
						
						m_ptxShList.ViewStyle = plasmatech.scpax.interop.TPTxViewStyle.ptvsThumbnails;
						break;

					case "FileListProperties":

						m_ptxShList.DoCommandForFolder("properties");
						break;

					case "FileListPaste": 

						m_ptxShList.DoCommandForFolder("paste");
						break;

					case "FileNewFolder":
						
						//	Does the tree have the focus?
						if(m_ptxShTree.Focused)
							m_ptxShTree.CreateNewFolder(true);
						else
							m_ptxShList.CreateNewFolder(true); 
						break;

					case "Register":    
						
						OnRegister(m_bTreeAction);
						break;

					case "RegistrationOptions":    
						
						ShowRegistrationOptions();
						break;

					case "ShowTree":   
						
						//	Toggle the view state
						SetShowTree(!m_bShowTree);
						break;

					case "ShowHidden":
						
						//	Toggle the visibility of hidden files and folders
						SetShowHidden(!m_bShowHidden);
						break;

				}// switch(e.Tool.Key)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnUltraToolClick", m_tmaxErrorBuilder.Message(ERROR_TOOL_CLICK_EX, e.Tool.Key), Ex);
			}
			
		}//	OnToolClick()

		/// <summary>
		/// This function handles events fired by the toolbar/menu manager after it displays a popup menu
		/// </summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Event notification paramaters</param>
		private void OnUltraAfterToolDropdown(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			try
			{
				//	Which tool?
				switch(e.Tool.Key)
				{

					case "FileListMenu":
						
						//	Clear the action flag
						m_bTreeAction = false;
						break;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnUltraAfterToolDropdown", m_tmaxErrorBuilder.Message(ERROR_TOOL_AFTER_DROP_EX, e.Tool.Key), Ex);
			}
		
		}
		
		/// <summary>
		/// This function handles events fired by the toolbar/menu manager when it is about to display
		///	a popup menu
		/// </summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Event notification paramaters</param>
		private void OnUltraBeforeToolDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			try
			{
				//	Which tool?
				switch(e.Tool.Key)
				{
					case "ViewStylesMenu": 
						
						//	Inhibit tool click processing in case we change the check state in code
						m_bIgnoreUltraEvents = true;
						
						//	Set the check states of the buttons
						SetCheckState("ViewIcons", m_ptxShList.ViewStyle == plasmatech.scpax.interop.TPTxViewStyle.ptvsIcon);
						SetCheckState("ViewList", m_ptxShList.ViewStyle == plasmatech.scpax.interop.TPTxViewStyle.ptvsList);
						SetCheckState("ViewDetails", m_ptxShList.ViewStyle == plasmatech.scpax.interop.TPTxViewStyle.ptvsReport);
						SetCheckState("ViewThumbnails", m_ptxShList.ViewStyle == plasmatech.scpax.interop.TPTxViewStyle.ptvsThumbnails);
						
						//	Reenable tool click processing
						m_bIgnoreUltraEvents = false;
						break;

					case "FileListMenu":
						
						//	Disable the paste command if nothing is in the clipboard to paste
						SetEnabled("FileListPaste", Clipboard.GetDataObject().GetDataPresent(DataFormats.FileDrop, true));
						break;

					case "NewMenu":
						
						break;
					
					case "PreferencesMenu":   
						
						break;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnUltraBeforeToolDropdown", m_tmaxErrorBuilder.Message(ERROR_TOOL_BEFORE_DROP_EX, e.Tool.Key), Ex);
			}
		
		}//	OnUltraBeforeToolDropdown()

		/// <summary>
		/// This function traps events fired by the toolbar when the user
		///	selects a new file filter.
		/// </summary>
		/// <param name="sender">The tool firing the event</param>
		/// <param name="e">Event notification paramaters</param>
		private void OnUltraToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
		{
			ValueListItem	vlItem;
			ComboBoxTool	objComboBox;
			
			if(m_bIgnoreUltraEvents == true) return;
			
			//	Is this the file filter combobox
			if(e.Tool.Key == "FileFilter")
			{
				if((objComboBox = (ComboBoxTool)e.Tool) != null)
				{
					if((vlItem = (ValueListItem)objComboBox.SelectedItem) != null)
					{
						if(vlItem.DataValue != null)
						{
							SetActiveFilter((CExplorerPaneFilter)vlItem.DataValue);
						}
					}
					
				}
			}
		}

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		private void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		/// <summary>
		/// This function is called to set the check state of a toolbar/menu tool
		/// </summary>
		/// <param name="strKey">Key of the tool to be set</param>
		/// <param name="bChecked">New check state</param>
		private void SetCheckState(string strKey, bool bChecked)
		{
			StateButtonTool Tool = null;
					
			try
			{
				if((Tool = (StateButtonTool)GetTool(strKey)) != null)
					Tool.Checked = bChecked;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetCheckState", m_tmaxErrorBuilder.Message(ERROR_TOOL_CHECK_STATE_EX, strKey, bChecked), Ex);
			}

		}//	SetCheckState()
		
		/// <summary>
		///	This function is called to set the control width ratio to be used for
		///	calculating the size and position of the folder tree and file list controls
		/// </summary>
		/// <param name="fCtrlWidthRatio">the ratio of the file list width to host control width</param>
		private void SetCtrlWidthRatio(float fCtrlWidthRatio)
		{
			//	Check the range
			if(fCtrlWidthRatio <= 0.0F)
				fCtrlWidthRatio = 0.1F;
			else if(fCtrlWidthRatio > 1.0F)
				fCtrlWidthRatio = 1.0F;
				
			//	Set the new value
			m_fCtrlWidthRatio = fCtrlWidthRatio;
			
			//	Recalculate the control positions
			RecalcLayout();

		}// SetCtrlWidthRatio(float fCtrlWidthRatio)
		
		/// <summary>This function will enable/disable the specified toolbar or menu tool</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <param name="bEnabled">true to enable, false to disable</param>
		private void SetEnabled(string strKey, bool bEnabled)
		{
			ToolBase Tool = null;
					
			try
			{
				if((Tool = GetTool(strKey)) != null)
					Tool.SharedProps.Enabled = bEnabled;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetEnabled", m_tmaxErrorBuilder.Message(ERROR_TOOL_ENABLE_EX, strKey, bEnabled), Ex);
			}

		}

		/// <summary>
		///	This function is called to set the option that controls the visibility of  
		///	hidden files and folders in the list and tree
		/// </summary>
		/// <param name="bShowHidden">true to display hidden files and folders</param>
		private void SetShowHidden(bool bShowHidden)
		{
			try
			{
				//	Set the folder and list box properties
				if(m_ptxShTree != null)
					m_ptxShTree.ShowHidden = bShowHidden;
				if(m_ptxShList != null)
					m_ptxShList.ShowHidden = bShowHidden;
					
				//	Force a refresh of the controls
				m_ptxShTree.CtlRefresh();
				m_ptxShList.CtlRefresh();
				
				//	Set the toolbar/menu check state
				m_bIgnoreUltraEvents = true;
				SetCheckState("ShowHidden", bShowHidden);
				m_bIgnoreUltraEvents = false;

				//	Save the new value
				m_bShowHidden = bShowHidden;
			}
			catch
			{
			}
		}
		
		/// <summary>
		///	This function is called to set the option that controls the visibility of  
		///	the folder tree
		/// </summary>
		/// <param name="bShowTree">true to display the folder tree</param>
		private void SetShowTree(bool bShowTree)
		{
			//	Set the desired display state
			try
			{
				//	Set the visibility of the panes
				if(bShowTree)
				{
					m_ptxShTree.Visible = true;
					m_ctrlSplitter.Visible = true;
					m_ptxShComboBox.Visible = false;
					RecalcLayout();
				}
				else
				{
					m_ptxShTree.Visible = false;
					m_ctrlSplitter.Visible = false;
					m_ptxShComboBox.Visible = true;
				}
				
				//	This forces a resizing of the pane to make the appropriate controls visible
				try
				{
					this.SetBounds(this.Left, this.Top, this.Width + 1, this.Height + 1);
					this.SetBounds(this.Left, this.Top, this.Width - 1, this.Height - 1);
				}
				catch
				{
				}
				
				//	Set the toolbar/menu check state
				m_bIgnoreUltraEvents = true;
				SetCheckState("ShowTree", bShowTree);
				m_bIgnoreUltraEvents = false;

				//	Save the new value
				m_bShowTree = bShowTree;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetShowTree", m_tmaxErrorBuilder.Message(ERROR_SHOW_TREE_EX, bShowTree), Ex);
			}
		
		}//	SetShowTree()
		
		/// <summary>This method is called to display the Trialmax popup window</summary>
		/// <param name="bTreeAction">true if the folder tree is the source of the request</param>
		private void ShowTrialmaxPopup(bool bTreeAction)
		{
			//	Display the Trialmax popup menu
			if(m_ctrlUltraToolbarManager != null)
			{
				m_ctrlUltraToolbarManager.ShowPopup("FileListMenu", Control.MousePosition);
	
				//	Set the appropriate flag to indicate the source of the request
				m_bTreeAction = bTreeAction;
			}
		
		}//	ShowTrialmaxPopup(bool bListAction)
		
		/// <summary>This method is called to set the visibility of the ActiveX shell controls</summary>
		/// <param name="bVisible">true to make the controls visible</param>
		/// <remarks>
		///		We make sure the controls are invisible when the screen layout is set because this pane will
		///		activate itself if we leave them visible. For some reason if there is more than one ActiveX
		//		control on a pane when the layout is set that pane will come to the top of its group and
		//		go active.
		/// </remarks>
		private void SetAxVisible(bool bVisible)
		{
			if((m_ptxShComboBox != null) && (m_ptxShComboBox.IsDisposed == false))
				m_ptxShComboBox.Visible = (bVisible & !m_bShowTree);
			if((m_ptxShList != null) && (m_ptxShList.IsDisposed == false))
				m_ptxShList.Visible = bVisible;
			if((m_ptxShTree != null) && (m_ptxShTree.IsDisposed == false))
				m_ptxShTree.Visible = (bVisible & m_bShowTree);
		
		}// private void SetAxVisible(bool bVisible)
		
		#endregion Private Methods
				
		#region Protected Methods

		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			//	Has the user activated this pane?
			if(PaneVisible == true)
			{
				//	Make sure the shell controls are visible
				//
				//	See remarks for SetAxVisible() method
				SetAxVisible(true);
			
			}

		}// protected override void OnPaneVisibleChanged()
		
		/// <summary>Clean up any resources being used </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				if(m_tmaxSourceFolder != null)
				{
					m_tmaxSourceFolder.Reset();
				}
				if(m_tmaxSelections != null)
				{
					m_tmaxSelections.Reset();
				}
				if(m_aFilters != null)
				{
					m_aFilters.Clear();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CExplorerPane));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RegistrationOptions");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Register");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("FileFilter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveUp");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowTree", "");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ViewStylesMenu");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveUp");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ViewStylesMenu");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewIcons", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewList", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewDetails", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewThumbnails", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewIcons", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewList", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool8 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewDetails", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ViewThumbnails", "");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("FileListMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveUp");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RegistrationOptions");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Register");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FileListPaste");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("NewMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FileListProperties");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool5 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PreferencesMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FileListProperties");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FileListPaste");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool6 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("NewMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FileNewFolder");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FileNewFolder");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Register");
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowTree", "");
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowHidden", "");
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool7 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PreferencesMenu");
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ViewStylesMenu");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowTree", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowHidden", "");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("FileFilter");
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RegistrationOptions");
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			this.m_ctrlSmallImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlUltraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlShComboPanel = new System.Windows.Forms.Panel();
			this.m_ctrlShTreePanel = new System.Windows.Forms.Panel();
			this.m_ptxShList = new plasmatech.scpax.AxPTxShList();
			this.m_ctrlSplitter = new System.Windows.Forms.Splitter();
			this.m_ptxShTree = new plasmatech.scpax.AxPTxShTree();
			this.m_ptxShComboBox = new plasmatech.scpax.AxPTxShCombo();
			this._CExplorerPane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CExplorerPane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CExplorerPane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CExplorerPane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).BeginInit();
			this.m_ctrlShComboPanel.SuspendLayout();
			this.m_ctrlShTreePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ptxShList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ptxShTree)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ptxShComboBox)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlSmallImages
			// 
			this.m_ctrlSmallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlSmallImages.ImageStream")));
			this.m_ctrlSmallImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlSmallImages.Images.SetKeyName(0, "");
			this.m_ctrlSmallImages.Images.SetKeyName(1, "");
			this.m_ctrlSmallImages.Images.SetKeyName(2, "");
			this.m_ctrlSmallImages.Images.SetKeyName(3, "");
			this.m_ctrlSmallImages.Images.SetKeyName(4, "");
			this.m_ctrlSmallImages.Images.SetKeyName(5, "");
			this.m_ctrlSmallImages.Images.SetKeyName(6, "");
			this.m_ctrlSmallImages.Images.SetKeyName(7, "");
			this.m_ctrlSmallImages.Images.SetKeyName(8, "");
			this.m_ctrlSmallImages.Images.SetKeyName(9, "");
			this.m_ctrlSmallImages.Images.SetKeyName(10, "");
			this.m_ctrlSmallImages.Images.SetKeyName(11, "");
			this.m_ctrlSmallImages.Images.SetKeyName(12, "");
			this.m_ctrlSmallImages.Images.SetKeyName(13, "");
			this.m_ctrlSmallImages.Images.SetKeyName(14, "");
			this.m_ctrlSmallImages.Images.SetKeyName(15, "");
			this.m_ctrlSmallImages.Images.SetKeyName(16, "");
			this.m_ctrlSmallImages.Images.SetKeyName(17, "");
			// 
			// m_ctrlUltraToolbarManager
			// 
			
			this.m_ctrlUltraToolbarManager.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
			this.m_ctrlUltraToolbarManager.DesignerFlags = 1;
			this.m_ctrlUltraToolbarManager.DockWithinContainer = this;
			this.m_ctrlUltraToolbarManager.ImageListSmall = this.m_ctrlSmallImages;
			this.m_ctrlUltraToolbarManager.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlUltraToolbarManager.LockToolbars = false;
			this.m_ctrlUltraToolbarManager.MdiMergeable = false;
			appearance1.BackColor = System.Drawing.Color.Transparent;
			this.m_ctrlUltraToolbarManager.MenuSettings.ToolAppearance = appearance1;
			this.m_ctrlUltraToolbarManager.ShowFullMenusDelay = 500;
			this.m_ctrlUltraToolbarManager.ShowQuickCustomizeButton = false;
			this.m_ctrlUltraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			buttonTool1.InstanceProps.IsFirstInGroup = true;
			comboBoxTool1.InstanceProps.IsFirstInGroup = true;
			comboBoxTool1.InstanceProps.Width = 231;
			buttonTool3.InstanceProps.IsFirstInGroup = true;
			stateButtonTool1.InstanceProps.IsFirstInGroup = true;
			appearance2.Image = 1;
			popupMenuTool1.InstanceProps.AppearancesSmall.Appearance = appearance2;
			appearance3.Image = 1;
			popupMenuTool1.InstanceProps.AppearancesSmall.AppearanceOnToolbar = appearance3;
			popupMenuTool1.InstanceProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
			ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            comboBoxTool1,
            buttonTool3,
            stateButtonTool1,
            popupMenuTool1});
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.BorderStyleDocked = Infragistics.Win.UIElementBorderStyle.None;
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.ShowInToolbarList = false;
			ultraToolbar1.Text = "MainToolbar";
			this.m_ctrlUltraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
			appearance4.Image = 0;
			buttonTool4.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool4.SharedProps.Caption = "&Move Up One Level";
			buttonTool4.SharedProps.Category = "Toolbar";
			popupMenuTool2.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
			popupMenuTool2.Settings.PopupStyle = Infragistics.Win.UltraWinToolbars.PopupStyle.Menu;
			appearance5.BackColor = System.Drawing.Color.Transparent;
			popupMenuTool2.Settings.ToolAppearance = appearance5;
			popupMenuTool2.Settings.ToolDisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageOnlyOnToolbars;
			appearance6.Image = 1;
			popupMenuTool2.SharedProps.AppearancesSmall.Appearance = appearance6;
			popupMenuTool2.SharedProps.Caption = "&View Styles";
			popupMenuTool2.SharedProps.Category = "Toolbar";
			popupMenuTool2.SharedProps.ToolTipText = "Select file view style";
			stateButtonTool2.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool3.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool4.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool5.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool2,
            stateButtonTool3,
            stateButtonTool4,
            stateButtonTool5});
			stateButtonTool6.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool6.SharedProps.Caption = "&Icons";
			stateButtonTool6.SharedProps.Category = "View";
			stateButtonTool6.SharedProps.ToolTipText = "Show Iconic View";
			stateButtonTool7.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool7.SharedProps.Caption = "&List";
			stateButtonTool7.SharedProps.Category = "View";
			stateButtonTool7.SharedProps.ToolTipText = "List Files";
			stateButtonTool8.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool8.SharedProps.Caption = "&Details";
			stateButtonTool8.SharedProps.Category = "View";
			stateButtonTool8.SharedProps.ToolTipText = "Show File Details";
			stateButtonTool9.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool9.SharedProps.Caption = "&Thumbnails";
			stateButtonTool9.SharedProps.Category = "View";
			stateButtonTool9.SharedProps.ToolTipText = "Display Thumbnail Images";
			popupMenuTool3.SharedProps.Caption = "FileListMenu";
			popupMenuTool3.SharedProps.Visible = false;
			buttonTool6.InstanceProps.IsFirstInGroup = true;
			buttonTool8.InstanceProps.IsFirstInGroup = true;
			popupMenuTool5.InstanceProps.IsFirstInGroup = true;
			popupMenuTool3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            popupMenuTool4,
            buttonTool9,
            popupMenuTool5});
			appearance7.Image = 3;
			buttonTool10.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool10.SharedProps.Caption = "P&roperties ...";
			buttonTool10.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool10.SharedProps.ToolTipText = "Show Properties";
			appearance8.Image = 2;
			buttonTool11.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool11.SharedProps.Caption = "&Paste";
			buttonTool11.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool11.SharedProps.ToolTipText = "Paste From Clipboard";
			popupMenuTool6.SharedProps.Caption = "&New ...";
			popupMenuTool6.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool12});
			appearance9.Image = 4;
			buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance9;
			buttonTool13.SharedProps.Caption = "&Folder";
			buttonTool13.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool13.SharedProps.ToolTipText = "Create a new folder";
			appearance10.Image = 7;
			buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance10;
			buttonTool14.SharedProps.Caption = "Register &Selections";
			appearance11.Image = 5;
			stateButtonTool10.SharedProps.AppearancesSmall.Appearance = appearance11;
			stateButtonTool10.SharedProps.Caption = "&Show Folder Tree";
			stateButtonTool10.SharedProps.ToolTipText = "Show Files";
			appearance12.Image = 15;
			stateButtonTool11.SharedProps.AppearancesSmall.Appearance = appearance12;
			stateButtonTool11.SharedProps.Caption = "Show &Hidden";
			stateButtonTool11.SharedProps.ToolTipText = "Show Hidden";
			appearance13.Image = 6;
			popupMenuTool7.SharedProps.AppearancesSmall.Appearance = appearance13;
			popupMenuTool7.SharedProps.Caption = "Preferences ...";
			popupMenuTool7.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool8,
            stateButtonTool12,
            stateButtonTool13});
			appearance14.BackColor = System.Drawing.SystemColors.Window;
			appearance14.TextTrimming = Infragistics.Win.TextTrimming.EllipsisWord;
			comboBoxTool2.EditAppearance = appearance14;
			appearance15.BorderColor = System.Drawing.Color.Red;
			appearance15.BorderColor3DBase = System.Drawing.Color.Lime;
			comboBoxTool2.SharedProps.AppearancesSmall.Appearance = appearance15;
			appearance16.BorderColor = System.Drawing.Color.Black;
			appearance16.BorderColor3DBase = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			comboBoxTool2.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance16;
			comboBoxTool2.SharedProps.Caption = "File Filter";
			comboBoxTool2.SharedProps.Spring = true;
			comboBoxTool2.SharedProps.ToolTipText = "Select File Filter";
			comboBoxTool2.Text = "Filter";
			appearance17.BackColor = System.Drawing.SystemColors.Control;
			appearance17.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.HorizontalBump;
			valueList1.Appearance = appearance17;
			valueList1.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayTextAndPicture;
			comboBoxTool2.ValueList = valueList1;
			appearance18.Image = 14;
			buttonTool15.SharedProps.AppearancesSmall.Appearance = appearance18;
			buttonTool15.SharedProps.Caption = "&Registration Options";
			this.m_ctrlUltraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            popupMenuTool2,
            stateButtonTool6,
            stateButtonTool7,
            stateButtonTool8,
            stateButtonTool9,
            popupMenuTool3,
            buttonTool10,
            buttonTool11,
            popupMenuTool6,
            buttonTool13,
            buttonTool14,
            stateButtonTool10,
            stateButtonTool11,
            popupMenuTool7,
            comboBoxTool2,
            buttonTool15});
			this.m_ctrlUltraToolbarManager.AfterToolDropdown += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterToolDropdown);
			this.m_ctrlUltraToolbarManager.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.OnUltraToolValueChanged);
			this.m_ctrlUltraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeToolDropdown);
			this.m_ctrlUltraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ctrlUltraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			// 
			// m_ctrlShComboPanel
			// 
			this.m_ctrlShComboPanel.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlShComboPanel.Controls.Add(this.m_ctrlShTreePanel);
			this.m_ctrlShComboPanel.Controls.Add(this.m_ptxShComboBox);
			this.m_ctrlShComboPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_ctrlShComboPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlShComboPanel.Location = new System.Drawing.Point(0, 73);
			this.m_ctrlShComboPanel.Name = "m_ctrlShComboPanel";
			this.m_ctrlShComboPanel.Size = new System.Drawing.Size(440, 239);
			this.m_ctrlShComboPanel.TabIndex = 0;
			// 
			// m_ctrlShTreePanel
			// 
			this.m_ctrlShTreePanel.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlShTreePanel.Controls.Add(this.m_ptxShList);
			this.m_ctrlShTreePanel.Controls.Add(this.m_ctrlSplitter);
			this.m_ctrlShTreePanel.Controls.Add(this.m_ptxShTree);
			this.m_ctrlShTreePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlShTreePanel.Location = new System.Drawing.Point(0, 22);
			this.m_ctrlShTreePanel.Name = "m_ctrlShTreePanel";
			this.m_ctrlShTreePanel.Size = new System.Drawing.Size(440, 217);
			this.m_ctrlShTreePanel.TabIndex = 12;
			// 
			// m_ptxShList
			// 
			this.m_ptxShList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ptxShList.Location = new System.Drawing.Point(211, 0);
			this.m_ptxShList.Name = "m_ptxShList";
			this.m_ptxShList.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_ptxShList.OcxState")));
			this.m_ptxShList.Size = new System.Drawing.Size(229, 217);
			this.m_ptxShList.TabIndex = 13;
			this.m_ptxShList.Visible = false;
			this.m_ptxShList.OnOleStartDrag += new plasmatech.scpax.IPTxShListEvents_OnOleStartDragEventHandler(this.OnListStartDrag);
			this.m_ptxShList.OnMouseDown += new plasmatech.scpax.IPTxShListEvents_OnMouseDownEventHandler(this.OnListMouseDown);
			this.m_ptxShList.OnAfterShellMenuSelection += new plasmatech.scpax.IPTxShListEvents_OnAfterShellMenuSelectionEventHandler(this.OnListAfterShellMenuSelection);
			this.m_ptxShList.OnBeforeShellMenuPopup += new plasmatech.scpax.IPTxShListEvents_OnBeforeShellMenuPopupEventHandler(this.OnListBeforeShellMenuPopup);
			this.m_ptxShList.OnOleCompleteDrag += new plasmatech.scpax.IPTxShListEvents_OnOleCompleteDragEventHandler(this.OnListCompleteDrag);
			// 
			// m_ctrlSplitter
			// 
			this.m_ctrlSplitter.Location = new System.Drawing.Point(208, 0);
			this.m_ctrlSplitter.Name = "m_ctrlSplitter";
			this.m_ctrlSplitter.Size = new System.Drawing.Size(3, 217);
			this.m_ctrlSplitter.TabIndex = 12;
			this.m_ctrlSplitter.TabStop = false;
			this.m_ctrlSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnMoveSplitter);
			// 
			// m_ptxShTree
			// 
			this.m_ptxShTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.m_ptxShTree.Location = new System.Drawing.Point(0, 0);
			this.m_ptxShTree.Name = "m_ptxShTree";
			this.m_ptxShTree.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_ptxShTree.OcxState")));
			this.m_ptxShTree.Size = new System.Drawing.Size(208, 217);
			this.m_ptxShTree.TabIndex = 11;
			this.m_ptxShTree.Visible = false;
			this.m_ptxShTree.OnMouseDown += new plasmatech.scpax.IPTxShTreeEvents_OnMouseDownEventHandler(this.OnTreeMouseDown);
			this.m_ptxShTree.OnOleStartDrag += new plasmatech.scpax.IPTxShTreeEvents_OnOleStartDragEventHandler(this.OnTreeStartDrag);
			this.m_ptxShTree.OnAfterShellMenuSelection += new plasmatech.scpax.IPTxShTreeEvents_OnAfterShellMenuSelectionEventHandler(this.OnTreeAfterShellMenuSelection);
			this.m_ptxShTree.OnBeforeShellMenuPopup += new plasmatech.scpax.IPTxShTreeEvents_OnBeforeShellMenuPopupEventHandler(this.OnTreeBeforeShellMenuPopup);
			this.m_ptxShTree.OnOleCompleteDrag += new plasmatech.scpax.IPTxShTreeEvents_OnOleCompleteDragEventHandler(this.OnTreeCompleteDrag);
			// 
			// m_ptxShComboBox
			// 
			this.m_ptxShComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_ptxShComboBox.Enabled = true;
			this.m_ptxShComboBox.Location = new System.Drawing.Point(0, 0);
			this.m_ptxShComboBox.Name = "m_ptxShComboBox";
			this.m_ptxShComboBox.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_ptxShComboBox.OcxState")));
			this.m_ptxShComboBox.Size = new System.Drawing.Size(440, 22);
			this.m_ptxShComboBox.TabIndex = 11;
			this.m_ptxShComboBox.Visible = false;
			// 
			// _CExplorerPane_Toolbars_Dock_Area_Left
			// 
			this._CExplorerPane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CExplorerPane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CExplorerPane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CExplorerPane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CExplorerPane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 73);
			this._CExplorerPane_Toolbars_Dock_Area_Left.Name = "_CExplorerPane_Toolbars_Dock_Area_Left";
			this._CExplorerPane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 239);
			this._CExplorerPane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CExplorerPane_Toolbars_Dock_Area_Right
			// 
			this._CExplorerPane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CExplorerPane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CExplorerPane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CExplorerPane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CExplorerPane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(440, 73);
			this._CExplorerPane_Toolbars_Dock_Area_Right.Name = "_CExplorerPane_Toolbars_Dock_Area_Right";
			this._CExplorerPane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 239);
			this._CExplorerPane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CExplorerPane_Toolbars_Dock_Area_Top
			// 
			this._CExplorerPane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CExplorerPane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CExplorerPane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CExplorerPane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CExplorerPane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CExplorerPane_Toolbars_Dock_Area_Top.Name = "_CExplorerPane_Toolbars_Dock_Area_Top";
			this._CExplorerPane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(440, 73);
			this._CExplorerPane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CExplorerPane_Toolbars_Dock_Area_Bottom
			// 
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 312);
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.Name = "_CExplorerPane_Toolbars_Dock_Area_Bottom";
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(440, 0);
			this._CExplorerPane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// CExplorerPane
			// 
			this.Controls.Add(this.m_ctrlShComboPanel);
			this.Controls.Add(this._CExplorerPane_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CExplorerPane_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CExplorerPane_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CExplorerPane_Toolbars_Dock_Area_Bottom);
			this.Name = "CExplorerPane";
			this.Size = new System.Drawing.Size(440, 312);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).EndInit();
			this.m_ctrlShComboPanel.ResumeLayout(false);
			this.m_ctrlShTreePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ptxShList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ptxShTree)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ptxShComboBox)).EndInit();
			this.ResumeLayout(false);

		}

		/// <summary>This function is called when the SourceTypes property changes</summary>
		protected override void OnSourceTypesChanged()
		{
			ComboBoxTool ctrlFilters = null;
			
			if((ctrlFilters = GetFiltersBox()) != null)
			{
				ctrlFilters.ValueList.ValueListItems.Clear();
				
				//	Add the default filter
				AddFilter(RegSourceTypes.AllFiles, "All Files *.*", "*.*", 9);

				//	Add a filter for each source type
				if((m_tmaxSourceTypes != null) && (m_tmaxSourceTypes.Count > 0))
				{
					foreach(CTmaxSourceType O in m_tmaxSourceTypes)
					{
						AddFilter(O.RegSourceType, O.GetFileSelectionString(), O.GetFileFilterString(), GetFilterImage(O));
					}
					
				}
				
				SetActiveFilter(RegSourceTypes.AllFiles);
				
			}// if((ctrlFilters = GetFiltersBox()) != null)
			
		}// protected virtual void OnSourceTypesChanged()
		
		/// <summary>
		/// This method is called to populate the error builder's format string collection
		/// </summary>
		/// <remarks>The strings should be added to the collection in the same order in which they are enumerated</remarks>
		protected override void SetErrorStrings()
		{
			//	Let the base class populate first
			base.SetErrorStrings();
			
			if((m_tmaxErrorBuilder != null) && (m_tmaxErrorBuilder.FormatStrings != null))
			{
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add %1 to the file filter list");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to make %1 the active file filter");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to get selections in the file list");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to get selections in the folder tree");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the current filter selection");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the tool with the specified key: Key = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while processing the tool click event: Key = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised after creating the drop down: Key = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised before creating the drop down: Key = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised in the attempt to set the tool's check state: Key = %1 Checked = %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised in the attempt to enable/disable the tool: Key = %1 Enabled = %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while changing the visibility of the folder tree: Show = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while building the command event parameters: Command = %1");
			}
			
		}// SetErrorStrings()

		/// <summary>Overridden base class member called when the pane window gets created</summary>
		/// <param name="e">The load event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			try
			{
				base.OnLoad(e);
			}
			catch
			{
				//	This traps a bug introduced in the Infragistics toolbar
				//	library version 7.2
				//
				//	UltraToolbarsManager::OnFormLoad() added a fix for a DWM problem
				//	but at the same time introduced a but because no test for form==nll
				//MessageBox.Show(Ex.ToString(), "trialmax");
			}
			
			//	Make sure the appropriate controls are visible
			SetShowTree(m_bShowTree);
		}

		/// <summary>This function is called to resize and reposition the folder tree and file list box</summary>
		protected override void RecalcLayout()
		{
			try
			{
				if((m_ptxShTree != null) && (m_ptxShTree.IsDisposed == false))
				{
					if((m_fCtrlWidthRatio > 0.0F) && (m_ctrlShTreePanel.Width > 0))
					{
						float fWidth = ((float)(m_ctrlShTreePanel.Width)) * m_fCtrlWidthRatio;
						m_ptxShTree.Size = new Size((int)fWidth, m_ptxShTree.Height);
					}
				
				}

			}
			catch
			{
			}
		
		}// RecalcLayout()

		#endregion Protected Methods

		private void OnTreeMouseDown(object sender, plasmatech.scpax.IPTxShTreeEvents_OnMouseDownEvent e)
		{
			bool bNoItem;
			
			try
			{
				//	Is the mouse on an item in the tree?
				bNoItem = m_ptxShTree.HitTest(e.ax, e.ay) == null  ||  m_ptxShTree.HitTestInfo(e.ax, e.ay).Nowhere;
	      
				//	Must be right click with no item and/or selection
				if((e.aButton == 2)  &&  (bNoItem == true))
				{
					//	Display the menu at the current mouse position
					ShowTrialmaxPopup(false);
				}
			}
			catch
			{
			}
		
		}

		#region Properties
		
		/// <summary>
		/// This property is used to control whether or not hidden folders and files are
		/// displayed in the folder tree and files list box
		/// </summary>
		public bool ShowHidden
		{
			get
			{
				return m_bShowHidden;
			}
			set
			{
				SetShowHidden(value);
			}
		}

		/// <summary>
		/// This property is used to control whether or not split screen view is used
		/// </summary>
		public bool ShowTree
		{
			get
			{
				return m_bShowTree;
			}
			set
			{
				SetShowTree(value);
			}
		}

		/// <summary>
		/// This property exposes the media files filter collection
		/// </summary>
		public CExplorerPaneFilters Filters
		{
			get
			{
				return m_aFilters;
			}
		}

		/// <summary>
		/// This property exposes the current source type selected for registration
		/// </summary>
		public FTI.Shared.Trialmax.RegSourceTypes SourceType
		{
			get
			{
				return m_eSourceType;
			}
			set
			{
				m_eSourceType = value;
				
				//	Set the new file filter selection
				SetActiveFilter(m_eSourceType);
			}
			
		}

		/// <summary>
		/// This property is used to control the ratio of the file list control
		///	width to the folder tree control width
		/// </summary>
		public float CtrlWidthRatio
		{
			get
			{
				return m_fCtrlWidthRatio;
			}
			set
			{
				SetCtrlWidthRatio(value);
			}
		}

		/// <summary>
		/// This is the parent folder containing all files associated with
		///	the latest event
		/// </summary>
		public CTmaxSourceFolder SourceFolder
		{
			get
			{
				return m_tmaxSourceFolder;
			}
		}

		#endregion Properties
		
		#region Nested Classes
		
		/// <summary>This class is used to define a file filter for a Trialmax media type</summary>
		public class CExplorerPaneFilter
		{
			#region Private Members
			
			/// <summary>Local member bound to Text property</summary>
			private string m_strText;
			
			/// <summary>Local member bound to Extensions property</summary>
			private string m_strExtensions;
			
			/// <summary>Local member bound to SourceType property</summary>
			private RegSourceTypes m_eSourceType;
			
			/// <summary>Local member bound to Image property</summary>
			private int m_iImage = -1;
			
			#endregion
			
			#region Public Methods
			
			/// <summary>Default constructor</summary>
			public CExplorerPaneFilter()
			{
			}
			
			/// <summary>
			/// Overloaded constructor
			/// </summary>
			/// <param name="iSourceType">Media type identifier</param>
			/// <param name="strText">Display text</param>
			/// <param name="strExtensions">Delimited extensions string</param>
			/// <param name="iImage">Image index</param>
			public CExplorerPaneFilter(RegSourceTypes eSourceType, string strText, string strExtensions)
			{
				m_eSourceType = eSourceType;
				m_strText = strText;
				m_strExtensions = strExtensions;
			}
			
			/// <summary>
			/// Overloaded constructor
			/// </summary>
			/// <param name="iSourceType">Media type identifier</param>
			/// <param name="strText">Display text</param>
			/// <param name="strExtensions">Delimited extensions string</param>
			/// <param name="iImage">Image index</param>
			public CExplorerPaneFilter(RegSourceTypes eSourceType, string strText, string strExtensions, int iImage)
			{
				m_eSourceType = eSourceType;
				m_strText = strText;
				m_strExtensions = strExtensions;
				m_iImage = iImage;	
			}
			
			/// <summary>
			/// Overloaded version of this method to return the extensions assocaited with this filter
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return m_strExtensions;
			}
			
			#endregion Public Methods
			
			#region Properties
			
			/// <summary>Text displayed in the list box</summary>
			public string Text
			{
				get
				{
					return m_strText;
				}
				set
				{
					m_strText = value;
				}
			}

			/// <summary>Semicolon delimited extensions string</summary>
			public string Extensions
			{
				get
				{
					return m_strExtensions;
				}
				set
				{
					m_strExtensions = value;
				}
			}

			/// <summary>Source type identifier for this filter</summary>
			public RegSourceTypes SourceType
			{
				get
				{
					return m_eSourceType;
				}
				set
				{
					m_eSourceType = value;
				}
			}

			/// <summary>Zero-based index of the image to be used with this filter</summary>
			public int Image
			{
				get
				{
					return m_iImage;
				}
				set
				{
					m_iImage = value;
				}
			}

			#endregion Properties
			
		}
		
		/// <summary>
		/// Objects of this class are used to manage a dynamic array of CExplorerPaneFilter objects
		/// </summary>
		public class CExplorerPaneFilters : CollectionBase
		{
		#region Public Members
		
			/// <summary>Default constructor</summary>
			public CExplorerPaneFilters()
			{
			}

			/// <summary>This method allows the caller to add a new filter to the list</summary>
			/// <param name="objFilter">CExplorerPaneFilter object to be added to the list</param>
			/// <returns>The object just added if successful, null otherwise</returns>
			public CExplorerPaneFilter Add(CExplorerPaneFilter objFilter)
			{
				try
				{
					// Use base class to perform actual collection operation
					base.List.Add(objFilter as object);

					return objFilter;
				}
				catch
				{
					return null;
				}
			
			}// Add(CExplorerPaneFilter objFilter)

			/// <summary>
			/// This method is called to remove the requested filter from the collection
			/// </summary>
			/// <param name="objFilter">The filter object to be removed</param>
			public void Remove(CExplorerPaneFilter objFilter)
			{
				try
				{
					// Use base class to process actual collection operation
					base.List.Remove(objFilter as object);
				}
				catch
				{
				}
			}

			/// <summary>
			/// This method is called to determine if the specified object exists in the collection
			/// </summary>
			/// <param name="objFilter">The parameter object to be checked</param>
			/// <returns>true if found in the collection</returns>
			public bool Contains(CExplorerPaneFilter objFilter)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(objFilter as object);
			}

			/// <summary>
			/// Called to locate the filter object with the specified text
			/// </summary>
			/// <returns>The object with the specified text</returns>
			public CExplorerPaneFilter Find(string strText)
			{
				// Search for the object with the same name
				foreach(CExplorerPaneFilter obj in base.List)
				{
					if(String.Compare(obj.Text, strText, true) == 0)
					{
						return obj;
					}
				}

				return null;

			}//	Find(string strText)

			/// <summary>
			/// Called to locate the filter object with the specified media type
			/// </summary>
			/// <returns>The object with the specified media type</returns>
			public CExplorerPaneFilter Find(RegSourceTypes eSourceType)
			{
				// Search for the object with the same name
				foreach(CExplorerPaneFilter obj in base.List)
				{
					if(obj.SourceType == eSourceType)
					{
						return obj;
					}
				}

				return null;
			
			}//	Find(int iSourceType)

			/// <summary>
			/// Overloaded version of [] operator to return the filter object at the desired index
			/// </summary>
			/// <returns>Filter object at the specified index</returns>
			public CExplorerPaneFilter this[int index]
			{
				// Use base class to process actual collection operation
				get 
				{ 
					return (base.List[index] as CExplorerPaneFilter);
				}
			}

			/// <summary>
			/// Composite filter string representing all filters in the collection
			/// </summary>
			public string Extensions
			{
				// Use base class to process actual collection operation
				get 
				{ 
					string strExtensions = "";
					
					foreach(CExplorerPaneFilter O in this)
					{
						if(O.SourceType != RegSourceTypes.AllFiles)
							strExtensions += (O.Extensions + ";");
					}
						
					return strExtensions;
				}
			
			}

			/// <summary>
			/// This method is called to retrieve the index of the specified object
			/// </summary>
			/// <param name="value">Object to be located</param>
			/// <returns>The zero-based index of the specified object</returns>
			public int IndexOf(CExplorerPaneFilter value)
			{
				// Find the 0 based index of the requested entry
				return base.List.IndexOf(value);
			}

			#endregion Public Methods
		
		}//	CExplorerPaneFilters
		
		#endregion Nested Classes
		
	}//	CExplorerPane		

}//	FTI.Trialmax.Panes
