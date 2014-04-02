using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class creates the view used to display the deposition transcript</summary>
	public class CTmaxVideoTranscript : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView
	{
		#region Constants
		
		/// <summary>View specific command identifiers</summary>
		private enum TmaxVideoTranscriptCommands
		{
			Invalid = 0,
			Preview,
			Add,
			InsertBefore,
			InsertAfter,
			SplitBefore,
			SplitAfter,
			Exclude,
			EditExtents,
			Find,
			GoTo,
			SetHighlighter,
			SetHighlighter1,
			SetHighlighter2,
			SetHighlighter3,
			SetHighlighter4,
			SetHighlighter5,
			SetHighlighter6,
			SetHighlighter7,
			AssignHighlighters,
		
		}// private enum TmaxVideoTranscriptCommands
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_SET_SCRIPT_EX						= ERROR_BASE_VIEW_MAX + 1;
		protected const int ERROR_ON_APP_OPENED_EX					= ERROR_BASE_VIEW_MAX + 2;
		protected const int ERROR_HIGHLIGHT_SCRIPT_EX				= ERROR_BASE_VIEW_MAX + 3;
		protected const int ERROR_ON_COMMAND_EX						= ERROR_BASE_VIEW_MAX + 4;
		protected const int ERROR_ON_CMD_PREVIEW_EX					= ERROR_BASE_VIEW_MAX + 5;
		protected const int ERROR_ON_PREVIEW_TRANSCRIPT_CHANGE_EX	= ERROR_BASE_VIEW_MAX + 6;
		protected const int ERROR_CREATE_DESIGNATIONS_EX			= ERROR_BASE_VIEW_MAX + 7;
		protected const int ERROR_ADD_SELECTION_EX					= ERROR_BASE_VIEW_MAX + 8;
		protected const int ERROR_ON_CMD_EDIT_EX					= ERROR_BASE_VIEW_MAX + 9;
		protected const int ERROR_FILL_HIGHLIGHTERS_EX				= ERROR_BASE_VIEW_MAX + 10;
		protected const int ERROR_SET_HIGHLIGHTER_EX				= ERROR_BASE_VIEW_MAX + 11;
		protected const int ERROR_ON_CMD_ASSIGN_HIGHLIGHTERS_EX		= ERROR_BASE_VIEW_MAX + 12;
		protected const int ERROR_GO_TO_EX							= ERROR_BASE_VIEW_MAX + 13;
		protected const int ERROR_ON_CMD_FIND_EX					= ERROR_BASE_VIEW_MAX + 14;
		protected const int ERROR_FIRE_EDIT_COMMAND_EX				= ERROR_BASE_VIEW_MAX + 15;

		private const string ULTRA_HIGHLIGHTERS_KEY = "Highlighters";
		private const string ULTRA_SET_HIGHLIGHTER_KEY = "SetHighlighter";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Private controls collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Image list bound to toolbar manager for toolbar and submenu images</summary>
		private System.Windows.Forms.ImageList m_ctrlToolbarImages;

		/// <summary>Image list bound to grid control</summary>
		private System.Windows.Forms.ImageList m_ctrlGridImages;

		/// <summary>Toolbar manager used to create main toolbar and context menu</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlToolbarManager;

		/// <summary>Infragistics docking area for toolbar management</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTranscript_Toolbars_Dock_Area_Left;

		/// <summary>Infragistics docking area for toolbar management</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTranscript_Toolbars_Dock_Area_Right;

		/// <summary>Infragistics docking area for toolbar management</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTranscript_Toolbars_Dock_Area_Top;

		/// <summary>Infragistics docking area for toolbar management</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom;

		/// <summary>Grid control used to display the transcript text</summary>
		private FTI.Trialmax.Controls.CTmaxTransGridCtrl m_ctrlTransGrid;
		
		/// <summary>The active designation</summary>
		private FTI.Shared.Xml.CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Array of selected rows being previewed</summary>
		private Array m_aPreviewSelections = null;
		
		/// <summary>Designation being previewed</summary>
		private CXmlDesignation m_xmlPreviewDesignation = null;
		
		/// <summary>Flag to inhibit processing of grid events</summary>
		private bool m_bIgnoreGridEvents = false;
		
		/// <summary>Flag to inhibit processing of toolbar events</summary>
		private bool m_bIgnoreToolbarEvents = false;
		
		/// <summary>The active highlighter</summary>
		private CTmaxHighlighter m_tmaxHighlighter = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoTranscript() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>This method is called by the application to initialize the view</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class initialization
			if(base.Initialize(xmlINI) == false)
				return false;
				
			return true;
		
		}// public override bool Initialize(CXmlIni xmlINI)
		
		/// <summary>This method is called by the application when it is about to terminate</summary>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Clear the grid
			Clear();
			
			//	Do the base class cleanup
			base.Terminate(xmlINI);
			
		}// public override void Terminate(CXmlIni xmlINI)
		
		/// <summary>This function is called by the application when the user is is finished setting the preferences</summary>
		/// <param name="bApplied">true if changes have been applied</param>
		public override void OnAppAfterSetPreferences(bool bApplied)
		{
			//	Has the user changed the values?
			if((bApplied == true) && (this.AppOptions != null))
			{
				//	Has the Pause Threshold changed?
				if(this.AppOptions.PauseThreshold != m_ctrlTransGrid.PauseThreshold)
					m_ctrlTransGrid.PauseThreshold = this.AppOptions.PauseThreshold;
			
				//	Update the highlighters 
				FillHighlighters();
				
				//	The user may have changed highlighter colors
				if(m_xmlScript != null)
					Highlight(m_xmlScript);
					
			}// if((bApplied == true) && (this.AppOptions != null))
		
		}// public override void OnAppAfterSetPreferences(bool bApplied)
		
		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the view</returns>
		public override bool OnAppHotkey(TmaxHotkeys eHotkey)
		{
			long						lSelections = 0;
			long						lStartPL = -1;
			long						lStopPL = -1;
			TmaxVideoTranscriptCommands	eCommand = TmaxVideoTranscriptCommands.Invalid;
						
			//	Get the current number of row selections
			if((m_ctrlTransGrid != null) && (m_ctrlTransGrid.IsDisposed == false))
				lSelections = m_ctrlTransGrid.GetSelectionRange(ref lStartPL, ref lStopPL);
			
			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.Find:
				
					eCommand = TmaxVideoTranscriptCommands.Find;
					break;
					
				case TmaxHotkeys.GoTo:
				
					eCommand = TmaxVideoTranscriptCommands.GoTo;
					break;
					
				case TmaxHotkeys.AddToScript:
				
					eCommand = TmaxVideoTranscriptCommands.Add;
					break;
					
				default:
				
					break;
			}
		
			//	Did this hotkey translate to a command?
			if(eCommand != TmaxVideoTranscriptCommands.Invalid)
			{
				//	Is this command visible?
				if(GetCommandVisible(eCommand) == true)
				{
					//	Is this command enabled
					if(GetCommandEnabled(eCommand, lSelections, lStartPL, lStopPL) == true)
					{
						OnCommand(eCommand);
					}
					
				}

			}// if(eCommand != TmaxVideoTranscriptCommands.Invalid)
			
			return (eCommand != TmaxVideoTranscriptCommands.Invalid);
			
		}// public override bool OnAppHotkey(TmaxHotkeys eHotkey)
		
		/// <summary>This function is called when the application options object changes</summary>
		public override void OnAppOptionsChanged()
		{
			if(this.AppOptions != null)
			{
				//	Has the Pause Threshold changed?
				if(this.AppOptions.PauseThreshold != m_ctrlTransGrid.PauseThreshold)
					m_ctrlTransGrid.PauseThreshold = this.AppOptions.PauseThreshold;
			
				//	Update the highlighters 
				FillHighlighters();
				
				//	The user may have changed highlighter colors
				if(m_xmlScript != null)
					Highlight(m_xmlScript);
					
			}// if(this.AppOptions != null)

		}// public override void OnAppOptionsChanged()
		
		/// <summary>This function is called by the application when the user opens a new XML script</summary>
		/// <param name="xmlScript">The new XML script</param>
		/// <returns>true if successful</returns>
		public override bool OnAppOpened(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			try
			{
				if(xmlScript != null)
				{
					bSuccessful = SetScript(xmlScript);
				}
				else
				{
					Clear();
					
					bSuccessful = true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppOpened", m_tmaxErrorBuilder.Message(ERROR_ON_APP_OPENED_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public override bool OnAppOpened(CXmlScript xmlScript)
		
		/// <summary>This method is called by the application when it processes a Select command event</summary>
		/// <param name="tmaxItem">The item to be selected</param>
		/// <param name="eView">The view requesting selection</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoActivate(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			if(tmaxItem.XmlDesignation != null)
			{
				//	Do we need to change the active designation?
				if((tmaxItem.XmlTranscript == null) || (ReferenceEquals(tmaxItem.XmlDesignation, m_xmlDesignation) == false))
				{
					m_ctrlTransGrid.Activate(tmaxItem.XmlDesignation, true);
					m_xmlDesignation = tmaxItem.XmlDesignation;
				}
				
				//	Are we supposed to highlight a particular line?
				if(tmaxItem.XmlTranscript != null)
				{
					try
					{
						m_ctrlTransGrid.SetSelection(tmaxItem.XmlTranscript.PL, true, true);
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireDiagnostic(this, "OnTmaxVideoActivate", Ex);
					}
				
				}// if(tmaxItem.XmlTranscript != null)
			
			
			}// if(tmaxItem.XmlDesignation != null)
			
			SetToolStates(null, false);
			return true;
		}
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoLoadResult(CTmaxVideoResult tmaxResult)
		{
			//	Is a designation bound to the result?
			if(tmaxResult.XmlDesignation != null)
			{
				//	Activate this designation
				OnTmaxVideoActivate(new CTmaxItem(m_xmlScript, tmaxResult.XmlDesignation), TmaxVideoViews.Results);
			}
			
			m_ctrlTransGrid.SetSelection(tmaxResult.PL, true, true);
				
			return true;			
		
		}// public override bool OnTmaxVideoLoadResult(CTmaxVideoResult tmaxResult)
		
		/// <summary>This method is called by the application when it processes a Select command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that added the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoAdd(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			//	New designations are stored in the SubItems collection
			if((tmaxItem.SubItems != null) && (tmaxItem.SubItems.Count > 0))
			{
				foreach(CTmaxItem O in tmaxItem.SubItems)
				{
					if(O.XmlDesignation != null)
						Highlight(O.XmlDesignation);
				}
			
			}
			
			return true;
			
		}// public virtual bool OnTmaxVideoAdd(CTmaxItem tmaxItem, int iId)
		
		/// <summary>This method is called by the application when it processes a Delete command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoDelete(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			//	Deleted designations are stored in the SubItems collection
			if((tmaxItem.SubItems != null) && (tmaxItem.SubItems.Count > 0))
			{
				if(m_xmlScript != null)
				{
					m_ctrlTransGrid.Erase();
					Highlight(m_xmlScript);	
					
					//	Has the active designation been deleted?
					if(m_xmlDesignation != null)
					{
						if(m_xmlScript.XmlDesignations.Contains(m_xmlDesignation) == false)
						{
							m_ctrlTransGrid.Activate(null, false);
							m_xmlDesignation = null;
						}
					
					}
					
				}		
			
			}
			
			return true;
		
		}// public override bool OnTmaxVideoDelete(CTmaxItem tmaxItem, int iId)
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			if((m_xmlScript != null) && (tmaxItem != null) && (tmaxItem.XmlDesignation != null))
			{
				//	Make sure higlights are drawn correctly
				m_ctrlTransGrid.Erase();
				Highlight(m_xmlScript);	
					
				//	Has the active designation been modified?
				if((m_xmlDesignation != null) && (ReferenceEquals(m_xmlDesignation, tmaxItem.XmlDesignation) == true))
				{
					//	Redraw the selection indicators
					m_ctrlTransGrid.Activate(m_xmlDesignation, false);
				}
			
			}
			
			return true;
		
		}// public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, int iId)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when the application opened a new script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to highlight the designations in the script: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to process the view command: command = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to preview the selected transcript text.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to handle the preview transcript change event. transcriptIndex = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create designations using the selected transcript text.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the selected text to the active script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the active designation: method = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of available highlighters.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active highlighter.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to assign the application highlighters.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to go to the specified line.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to search the active transcript.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the edit command: Method = %1");

		}// protected override void SetErrorStrings()

		/// <summary>This method is called the when the window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			SetToolStates(null, false);
			
			//	Perform the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>Used by form designer to lay out child controls</summary> 
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxVideoTranscript));
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preview");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GoTo");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertAfter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitAfter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exclude");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditExtents");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("HighlighterLabel");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Highlighters");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preview");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBefore");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertAfter");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertAfter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preview");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitAfter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exclude");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditExtents");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GoTo");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AssignHighlighters");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitBefore");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitAfter");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exclude");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditExtents");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Highlighters");
			Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("HighlighterLabel");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter1", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter2", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter3", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter4", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter5", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter6", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter7", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool8 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter1", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter2", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter3", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter4", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter5", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter6", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool14 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter7", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AssignHighlighters");
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GoTo");
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			this.m_ctrlTransGrid = new FTI.Trialmax.Controls.CTmaxTransGridCtrl();
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlGridImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlTransGrid
			// 
			this.m_ctrlTransGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlTransGrid.Location = new System.Drawing.Point(0, 74);
			this.m_ctrlTransGrid.Name = "m_ctrlTransGrid";
			this.m_ctrlTransGrid.PauseThreshold = 0;
			this.m_ctrlTransGrid.Size = new System.Drawing.Size(416, 76);
			this.m_ctrlTransGrid.TabIndex = 0;
			this.m_ctrlTransGrid.XmlDeposition = null;
			this.m_ctrlTransGrid.DblClick += new System.EventHandler(this.OnGridDblClick);
			this.m_ctrlTransGrid.SelChanged += new System.EventHandler(this.OnGridSelChanged);
			// 
			// m_ctrlToolbarImages
			// 
			this.m_ctrlToolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlToolbarImages.ImageStream")));
			this.m_ctrlToolbarImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlGridImages
			// 
			this.m_ctrlGridImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlGridImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlGridImages.ImageStream")));
			this.m_ctrlGridImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlToolbarManager
			// 
			this.m_ctrlToolbarManager.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
			this.m_ctrlToolbarManager.DesignerFlags = 1;
			this.m_ctrlToolbarManager.DockWithinContainer = this;
			this.m_ctrlToolbarManager.ImageListSmall = this.m_ctrlToolbarImages;
			this.m_ctrlToolbarManager.LockToolbars = true;
			this.m_ctrlToolbarManager.ShowFullMenusDelay = 500;
			this.m_ctrlToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.Text = "MainToolbar";
			buttonTool4.InstanceProps.IsFirstInGroup = true;
			buttonTool7.InstanceProps.IsFirstInGroup = true;
			labelTool1.InstanceProps.IsFirstInGroup = true;
			labelTool1.InstanceProps.Width = 72;
			ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  buttonTool1,
																							  buttonTool2,
																							  buttonTool3,
																							  buttonTool4,
																							  buttonTool5,
																							  buttonTool6,
																							  buttonTool7,
																							  buttonTool8,
																							  buttonTool9,
																							  buttonTool10,
																							  labelTool1,
																							  comboBoxTool1});
			this.m_ctrlToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																												 ultraToolbar1});
			appearance1.Image = 0;
			buttonTool11.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool11.SharedProps.Caption = "&Preview ...";
			appearance2.Image = 4;
			buttonTool12.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool12.SharedProps.Caption = "Add Selection";
			appearance3.Image = 5;
			buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool13.SharedProps.Caption = "Insert Selection Before";
			appearance4.Image = 6;
			buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool14.SharedProps.Caption = "Insert Selection After";
			popupMenuTool1.SharedProps.Caption = "ContextMenu";
			buttonTool18.InstanceProps.IsFirstInGroup = true;
			buttonTool19.InstanceProps.IsFirstInGroup = true;
			buttonTool23.InstanceProps.IsFirstInGroup = true;
			popupMenuTool2.InstanceProps.IsFirstInGroup = true;
			popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							   buttonTool15,
																							   buttonTool16,
																							   buttonTool17,
																							   buttonTool18,
																							   buttonTool19,
																							   buttonTool20,
																							   buttonTool21,
																							   buttonTool22,
																							   buttonTool23,
																							   buttonTool24,
																							   popupMenuTool2,
																							   buttonTool25});
			appearance5.Image = 14;
			buttonTool26.SharedProps.AppearancesSmall.Appearance = appearance5;
			buttonTool26.SharedProps.Caption = "Split &Before";
			appearance6.Image = 13;
			buttonTool27.SharedProps.AppearancesSmall.Appearance = appearance6;
			buttonTool27.SharedProps.Caption = "Split &After";
			appearance7.Image = 12;
			buttonTool28.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool28.SharedProps.Caption = "E&xclude";
			appearance8.Image = 11;
			buttonTool29.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool29.SharedProps.Caption = "&Edit Extents";
			comboBoxTool2.SharedProps.Caption = "Highlighters";
			comboBoxTool2.SharedProps.Spring = true;
			comboBoxTool2.ValueList = valueList1;
			labelTool2.SharedProps.Caption = "Highlighter:";
			labelTool2.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool2.SharedProps.ShowInCustomizer = false;
			appearance9.Image = 1;
			popupMenuTool3.SharedProps.AppearancesSmall.Appearance = appearance9;
			popupMenuTool3.SharedProps.Caption = "Set Highlighter";
			stateButtonTool1.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool2.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool3.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool4.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool5.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool6.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool7.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							   stateButtonTool1,
																							   stateButtonTool2,
																							   stateButtonTool3,
																							   stateButtonTool4,
																							   stateButtonTool5,
																							   stateButtonTool6,
																							   stateButtonTool7});
			stateButtonTool8.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool8.SharedProps.Caption = "SetHighlighter1";
			stateButtonTool9.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool9.SharedProps.Caption = "SetHighlighter2";
			stateButtonTool10.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool10.SharedProps.Caption = "SetHighlighter3";
			stateButtonTool11.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool11.SharedProps.Caption = "SetHighlighter4";
			stateButtonTool12.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool12.SharedProps.Caption = "SetHighlighter5";
			stateButtonTool13.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool13.SharedProps.Caption = "SetHighlighter6";
			stateButtonTool14.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool14.SharedProps.Caption = "SetHighlighter7";
			appearance10.Image = 2;
			buttonTool30.SharedProps.AppearancesSmall.Appearance = appearance10;
			buttonTool30.SharedProps.Caption = "Assign Highlighters ...";
			appearance11.Image = 8;
			buttonTool31.SharedProps.AppearancesSmall.Appearance = appearance11;
			buttonTool31.SharedProps.Caption = "&Find ...";
			appearance12.Image = 9;
			buttonTool32.SharedProps.AppearancesSmall.Appearance = appearance12;
			buttonTool32.SharedProps.Caption = "&Go To ...";
			this.m_ctrlToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																										  buttonTool11,
																										  buttonTool12,
																										  buttonTool13,
																										  buttonTool14,
																										  popupMenuTool1,
																										  buttonTool26,
																										  buttonTool27,
																										  buttonTool28,
																										  buttonTool29,
																										  comboBoxTool2,
																										  labelTool2,
																										  popupMenuTool3,
																										  stateButtonTool8,
																										  stateButtonTool9,
																										  stateButtonTool10,
																										  stateButtonTool11,
																										  stateButtonTool12,
																										  stateButtonTool13,
																										  stateButtonTool14,
																										  buttonTool30,
																										  buttonTool31,
																										  buttonTool32});
			this.m_ctrlToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterPopup);
			this.m_ctrlToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			this.m_ctrlToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ctrlToolbarManager.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.OnUltraToolValueChanged);
			// 
			// _CTmaxVideoTranscript_Toolbars_Dock_Area_Left
			// 
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 74);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.Name = "_CTmaxVideoTranscript_Toolbars_Dock_Area_Left";
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 76);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxVideoTranscript_Toolbars_Dock_Area_Right
			// 
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(416, 74);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.Name = "_CTmaxVideoTranscript_Toolbars_Dock_Area_Right";
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 76);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxVideoTranscript_Toolbars_Dock_Area_Top
			// 
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.Name = "_CTmaxVideoTranscript_Toolbars_Dock_Area_Top";
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(416, 74);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom
			// 
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 150);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.Name = "_CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom";
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(416, 0);
			this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// CTmaxVideoTranscript
			// 
			this.m_ctrlToolbarManager.SetContextMenuUltra(this, "ContextMenu");
			this.Controls.Add(this.m_ctrlTransGrid);
			this.Controls.Add(this._CTmaxVideoTranscript_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTmaxVideoTranscript_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTmaxVideoTranscript_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTmaxVideoTranscript_Toolbars_Dock_Area_Bottom);
			this.Name = "CTmaxVideoTranscript";
			this.Size = new System.Drawing.Size(416, 150);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).EndInit();
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>Clean up</summary>
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
	
		/// <summary>This method is called to save the current selection as a designation(s)</summary>
		/// <param name="bInsert">true to insert designations into the current scene</param>
		/// <param name="bBefore">true to insert designations before the current scene</param>
		private bool AddSelection(bool bInsert, bool bBefore)
		{
			CXmlDesignations		xmlDesignations = null;
			string					strMsg = "";
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem			tmaxScript = null;
			CTmaxItem			tmaxDesignation = null;
			CTmaxVideoArgs			Args = null;
			bool					bSuccessful = false;
			
			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return false;
			
			try
			{
				//	Create the designations for the current selection
				xmlDesignations = CreateDesignations();
				if((xmlDesignations == null) || (xmlDesignations.Count == 0))
					return false;
					
				//	Did the selections break across segments?
				if(xmlDesignations.Count > 1)
				{
					strMsg = String.Format("The current selection will result in {0} designations\n\n", xmlDesignations.Count);
					
					foreach(CXmlDesignation O in xmlDesignations)
						strMsg += (O.Name + "\n");
						
					strMsg += "\nDo you want to continue?";
					
					//	Prompt the user for confirmation before continuing
					if(MessageBox.Show(strMsg, "Please Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						return false;
				}
					
				//	Allocate the parameter collection for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Activate, true);
				
				//	Allocate an item to represent the parent script
				tmaxScript = new CTmaxItem();
				tmaxScript.XmlScript = m_xmlScript;
				tmaxScript.MediaType = TmaxMediaTypes.Script;
				
				//	Add each of the designations
				foreach(CXmlDesignation O in xmlDesignations)
				{
					//	Create an event item to represent this designation
					tmaxDesignation = new CTmaxItem();
					tmaxDesignation.MediaType = TmaxMediaTypes.Designation;
					tmaxDesignation.XmlDesignation = O;
					
					//	Add to the collection of source items
					tmaxScript.SourceItems.Add(tmaxDesignation);
					
				}// foreach(CXmlDesignation O in xmlDesignations)
				
				//	Did we add any designations?
				if(tmaxScript.SourceItems.Count == 0) return false;
				
				//	Are we inserting into the script
				if((bInsert == true) && (m_xmlDesignation != null))
				{
					//	Allocate an item to represent the insertion point
					tmaxDesignation = new CTmaxItem();
					tmaxDesignation.MediaType = TmaxMediaTypes.Designation;
					tmaxDesignation.XmlDesignation = m_xmlDesignation;
					
					//	Add to the subitems collection
					tmaxScript.SubItems.Add(tmaxDesignation);
					
					tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
					
				}		
			
				//	Fire the command event
				Args = FireCommand(TmaxVideoCommands.Add, tmaxScript, tmaxParameters);
			
				if(Args != null)
					bSuccessful = Args.Successful;
					
				//	Clear the current selection if successful
				if(bSuccessful == true)
					ClearSelections(true);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSelection", m_tmaxErrorBuilder.Message(ERROR_ADD_SELECTION_EX), Ex);
			}				
			
			return bSuccessful;
			
		}// private bool AddSelection(bool bInsert, bool bBefore)
		
		/// <summary>Called to clear the grid</summary>
		private void Clear()
		{
			try
			{
				//	Notify the child grid
				m_ctrlTransGrid.Clear();
			}
			catch
			{
			}
			finally
			{
				//	Reset the local reference
				m_xmlScript = null;
				m_xmlDesignation = null;
			}
			
		}// private void Clear()
		
		/// <summary>This method is called to set the active script</summary>
		/// <param name="xmlScript">The script to be displayed</param>
		/// <returns>true if successful</returns>
		private bool SetScript(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			//	Is the caller attempting to clear the view?
			if(xmlScript == null)
			{
				Clear();
				return true;
			}
			
			try
			{
				//	Update the class reference
				m_xmlScript = xmlScript;
				
				//	Load the transcript text into the grid
				bSuccessful = m_ctrlTransGrid.SetDeposition(m_xmlScript.XmlDeposition, null);
				
				//	Were we successful?
				if(bSuccessful == true)
				{
					//	Highlight the scenes contained in this script
					Highlight(m_xmlScript);
				
				}// if(bSuccessful == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScript", m_tmaxErrorBuilder.Message(ERROR_SET_SCRIPT_EX), Ex);
			}
			finally
			{
				SetToolStates(null, false);
			}
			
			return bSuccessful;
			
		}// private bool SetScript(CXmlScript xmlScript)
		
		/// <summary>This method is called to highlight all designation in the specified script</summary>
		/// <param name="xmlScript">The script that owns the designations</param>
		/// <returns>true if successful</returns>
		private bool Highlight(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			Debug.Assert(xmlScript != null);
			if(xmlScript == null) return false;
			
			try
			{
				//	Highlight the scenes contained in this script
				if((xmlScript.XmlDesignations != null) && (xmlScript.XmlDesignations.Count > 0))
				{
					foreach(CXmlDesignation O in xmlScript.XmlDesignations)
						Highlight(O);
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Highlight", m_tmaxErrorBuilder.Message(ERROR_HIGHLIGHT_SCRIPT_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Highlight(CXmlScript xmlScript)
		
		/// <summary>This method is called to highlight the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be highlighted</param>
		/// <returns>true if successful</returns>
		private bool Highlight(CXmlDesignation xmlDesignation)
		{
			bool bSuccessful = false;
			
			try
			{
				m_ctrlTransGrid.Highlight(xmlDesignation.FirstPL, xmlDesignation.LastPL, GetHighlighter(xmlDesignation));
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Highlight", Ex);
			}
			
			return bSuccessful;
			
		}// private bool Highlight(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to get the highlight color for the specified designation</summary>
		/// <param name="xmlDesignation">The designation being highlighted</param>
		/// <returns>The associated highlighter color</returns>
		private System.Drawing.Color GetHighlighter(CXmlDesignation xmlDesignation)
		{
			System.Drawing.Color	sysColor = System.Drawing.Color.Yellow;
			CTmaxHighlighter	tmaxHighlighter = null;

			//	Do we have the application highlighters?
			if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.Highlighters != null))
			{
				//	Get the highlighter bound to this designation
				if((tmaxHighlighter = m_tmaxAppOptions.Highlighters.Find(xmlDesignation.Highlighter)) != null)
					sysColor = tmaxHighlighter.Color;
			}
			
			return sysColor;
		}
		
		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			TmaxVideoTranscriptCommands eCommand = TmaxVideoTranscriptCommands.Invalid;
			
			if(m_bIgnoreToolbarEvents == false)
			{
				if((eCommand = GetCommand(e.Tool.Key)) != TmaxVideoTranscriptCommands.Invalid)
					OnCommand(eCommand);
			}
		
		}// private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		
		/// <summary>Traps the ToolValueChanged event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
		{
			if(e.Tool.Key == ULTRA_HIGHLIGHTERS_KEY)
			{
				OnHighlighterSelChanged();
			}
		
		}// private void OnUltraToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterPopup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			TmaxVideoTranscriptCommands	eCommand = TmaxVideoTranscriptCommands.Invalid;
			PopupMenuTool				popupMenu = null;
			
			try { popupMenu = (PopupMenuTool)(e.Tool); }
			catch { return; }
			
			//	Check each tool in the menu's collection
			foreach(ToolBase O in popupMenu.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != TmaxVideoTranscriptCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)
				
		}// private void OnUltraAfterPopup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)

		/// <summary>This function handles events fired by the toolbar manager when it is about to display a menu</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			PopupMenuTool popupMenu = null;
			
			try
			{
				//	Enable/disable the commands in this submenu
				if((popupMenu = (PopupMenuTool)(e.Tool)) != null)
				{
					if(e.Tool.Key == ULTRA_SET_HIGHLIGHTER_KEY)
					{
						OnSetHighlighter(popupMenu);
					}
					else
					{
						if(popupMenu.Tools != null)
							SetToolStates(popupMenu.Tools, true);
					}
						
				}

			}
			catch
			{
			}
					
		}// private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

		/// <summary>This method handles events fired by the preview form when it changes the transcript text</summary>
		/// <param name="xmlDesignation">The designation being previewed</param>
		///	<param name="iTranscript">The index of the active transcript</param>
		private void OnPreviewTranscriptChanged(CXmlDesignation xmlDesignation, int iTranscript)
		{
			CTmaxTransGridRow gridRow = null;
			
			Debug.Assert(xmlDesignation != null);
			Debug.Assert(ReferenceEquals(m_xmlPreviewDesignation,xmlDesignation) == true);
			Debug.Assert(m_aPreviewSelections != null);
			Debug.Assert(m_xmlPreviewDesignation.Transcripts != null);
			
			if(xmlDesignation == null) return;
			if(ReferenceEquals(m_xmlPreviewDesignation,xmlDesignation) == false) return;
			if(m_aPreviewSelections == null) return;
			if(m_xmlPreviewDesignation.Transcripts == null) return;
			
			try
			{
				//	Stop here if transcript is out of range
				if((iTranscript < 0) || (iTranscript > m_aPreviewSelections.GetUpperBound(0)))
				{
					//	Make sure nothing is selected
					ClearSelections(false);
				}
				else
				{
					//	Get the row object associated with the specified transcript
					if((gridRow = (CTmaxTransGridRow)m_aPreviewSelections.GetValue(iTranscript)) != null)
						m_ctrlTransGrid.SetSelection(gridRow, true);
				}
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnPreviewTranscriptChanged", m_tmaxErrorBuilder.Message(ERROR_ON_PREVIEW_TRANSCRIPT_CHANGE_EX, iTranscript.ToString()), Ex);
			}

		}// private void OnPreviewTranscriptChanged(object sender, CXmlDesignation xmlDesignation, int iTranscript)
		
		/// <summary>This function is called to retrieve the menu / toolbar tool with the specified key</summary>
		/// <param name="strKey">Alpha-numeric key identifier</param>
		/// <returns>Infragistics base class tool object</returns>
		private ToolBase GetUltraTool(string strKey)
		{
			ToolBase Tool = null;
			
			if(m_ctrlToolbarManager != null)
				Tool = m_ctrlToolbarManager.Tools[strKey];

			return Tool;
		
		}// private ToolBase GetUltraTool(string strKey)
		
		/// <summary>This function is called to get the tool associated with the specified command</summary>
		/// <param name="eCommand">The application command enumeration</param>
		/// <returns>Infragistics base class tool object</returns>
		private ToolBase GetUltraTool(TmaxVideoTranscriptCommands eCommand)
		{
			return GetUltraTool(eCommand.ToString());
		}
		
		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(TmaxVideoTranscriptCommands eCommand)
		{
			switch(eCommand)
			{
				case TmaxVideoTranscriptCommands.Find:
				
					return Shortcut.CtrlF;
				
				case TmaxVideoTranscriptCommands.GoTo:
				
					return Shortcut.CtrlG;
				
				case TmaxVideoTranscriptCommands.Add:
				
					return Shortcut.CtrlS;
				
				default:
				
					return Shortcut.None;
			
			}// switch(eCommand)

		}// private Shortcut GetCommandShortcut(TmaxVideoTranscriptCommands eCommand)
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The transcript view command enumeration</param>
		/// <param name="lSelections">The number of lines in the current selection</param>
		/// <param name="lSelectedStart">The composite PL value of the first selected line</param>
		/// <param name="lSelectedStop">The composite PL value of the last selected line</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(TmaxVideoTranscriptCommands eCommand, long lSelections, long lSelectedStart, long lSelectedStop)
		{
			//	Do we have a valid grid
			if(m_ctrlTransGrid == null) return false;
			
			//	We have to have a script
			if(m_xmlScript == null) return false;
				
			//	What is the command?
			switch(eCommand)
			{
				case TmaxVideoTranscriptCommands.Add:
				
					if(lSelections <= 0) return false;
					if(m_xmlScript == null) return false;
					
					//	All conditions have been met
					return true;
					
				case TmaxVideoTranscriptCommands.InsertBefore:
				case TmaxVideoTranscriptCommands.InsertAfter:
				
					if(lSelections <= 0) return false;
					if(m_xmlScript == null) return false;
					if(m_xmlDesignation == null) return false;
					
					//	All conditions have been met
					return true;
					
				case TmaxVideoTranscriptCommands.Preview:
	
					return (lSelections > 0);
					
				case TmaxVideoTranscriptCommands.EditExtents:

					if(lSelections <= 0) return false;
					if(m_xmlScript == null) return false;
					if(m_xmlDesignation == null) return false;
					return true;
					
				case TmaxVideoTranscriptCommands.Exclude:
				
					if(lSelections <= 0) return false;
					if(m_xmlScript == null) return false;
					if(m_xmlDesignation == null) return false;

					//	Must be a portion of the designation selected
					if(lSelectedStart <= m_xmlDesignation.FirstPL)
					{
						return ((lSelectedStop >= m_xmlDesignation.FirstPL) && (lSelectedStop < m_xmlDesignation.LastPL));
					}
					else
					{
						return (lSelectedStart <= m_xmlDesignation.LastPL);
					}
						
				case TmaxVideoTranscriptCommands.SplitBefore:
				
					if(lSelections <= 0) return false;
					if(m_xmlScript == null) return false;
					if(m_xmlDesignation == null) return false;

					//	Must be a portion selected after the start of the designation
					return ((lSelectedStart > m_xmlDesignation.FirstPL) && (lSelectedStart <= m_xmlDesignation.LastPL));
						
				case TmaxVideoTranscriptCommands.SplitAfter:
				
					if(lSelections <= 0) return false;
					if(m_xmlScript == null) return false;
					if(m_xmlDesignation == null) return false;

					//	Must be a portion selected before the end of the designation
					return ((lSelectedStop < m_xmlDesignation.LastPL) && (lSelectedStop >= m_xmlDesignation.FirstPL));
						
				case TmaxVideoTranscriptCommands.SetHighlighter:
				case TmaxVideoTranscriptCommands.SetHighlighter1:
				case TmaxVideoTranscriptCommands.SetHighlighter2:
				case TmaxVideoTranscriptCommands.SetHighlighter3:
				case TmaxVideoTranscriptCommands.SetHighlighter4:
				case TmaxVideoTranscriptCommands.SetHighlighter5:
				case TmaxVideoTranscriptCommands.SetHighlighter6:
				case TmaxVideoTranscriptCommands.SetHighlighter7:
				case TmaxVideoTranscriptCommands.AssignHighlighters:
				
					if(m_tmaxAppOptions == null) return false;
					if(m_tmaxAppOptions.Highlighters == null) return false;
					if(m_tmaxAppOptions.Highlighters.Count == 0) return false;

					return true;
						
				case TmaxVideoTranscriptCommands.GoTo:
				case TmaxVideoTranscriptCommands.Find:
				
					if(m_xmlScript == null) return false;
					if(m_xmlScript.XmlDeposition == null) return false;

					return true;
						
				default:
				
					break;
			}	
			
			return false;
		
		}// private bool GetCommandEnabled(TmaxVideoTranscriptCommands eCommand, CTmaxTreeNodes tmaxNodes)
		
		/// <summary>This method is called to determine if the specified command should be visible</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>true if command should be visible</returns>
		private bool GetCommandVisible(TmaxVideoTranscriptCommands eCommand)
		{
			return true;
		}
		
		/// <summary>This method is called to convert the specified key to its associated command enumeration</summary>
		/// <returns>The associated tree command</returns>
		private TmaxVideoTranscriptCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TmaxVideoTranscriptCommands));
				
				foreach(TmaxVideoTranscriptCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TmaxVideoTranscriptCommands.Invalid;
		
		}// private TmaxVideoTranscriptCommands GetCommand(string strKey)
		
		/// <summary>This function enables / disables the command tools</summary>
		/// <param name="ultraTools">The collection of tools to be enabled/disabled</param>
		/// <param name="bShortcuts">true to apply shortcuts to the tools in the collection</param>
		private void SetToolStates(Infragistics.Win.UltraWinToolbars.ToolsCollectionBase ultraTools, bool bShortcuts)
		{
			TmaxVideoTranscriptCommands eCommand = TmaxVideoTranscriptCommands.Invalid;
			long						lSelections = 0;
			long						lStartPL = -1;
			long						lStopPL = -1;
			Shortcut					eShortcut = Shortcut.None;
						
			//	Get the current number of row selections
			Debug.Assert(m_ctrlTransGrid != null);
			Debug.Assert(m_ctrlTransGrid.IsDisposed == false);
			if((m_ctrlTransGrid != null) && (m_ctrlTransGrid.IsDisposed == false))
				lSelections = m_ctrlTransGrid.GetSelectionRange(ref lStartPL, ref lStopPL);
			
			try
			{
				//	Should we use the root collection?
				if(ultraTools == null)
					ultraTools = m_ctrlToolbarManager.Tools;
					
				//	Iterate the master tools collection
				foreach(ToolBase O in ultraTools)
				{
					//	Get the command associated with this tool
					if((eCommand = GetCommand(O.Key)) != TmaxVideoTranscriptCommands.Invalid)
					{
						//	Should this tool be visible and enabled?
						O.SharedProps.Visible = GetCommandVisible(eCommand);
						O.SharedProps.Enabled = GetCommandEnabled(eCommand, lSelections, lStartPL, lStopPL);

						//	Set shortcuts for commands on this menu
						if((bShortcuts == true) && (O.SharedProps.Visible == true))
						{
							if((eShortcut = GetCommandShortcut(eCommand)) != Shortcut.None)
								O.SharedProps.Shortcut = eShortcut;
						}
					}
						
				}
						
			}
			catch
			{
					
			}
					
		}// private void SetToolStates()

		/// <summary>This is called when the Set Highlighter submenu gets displayed</summary>
		/// <param name="popupMenu">The Set Highlighter popup menu tool</param>
		private void OnSetHighlighter(PopupMenuTool popupMenu)
		{
			string					strKey = "";
			StateButtonTool			toolHighlighter = null;
			CTmaxHighlighter	tmaxHighlighter = null;
			
			if(m_tmaxAppOptions == null) return;
			if(m_tmaxAppOptions.Highlighters == null) return;
			
			try
			{
				//	Inhibit toolbar events while we initialize the menu
				//
				//	NOTE:	Setting the check state to True will trigger
				//			a click event
				m_bIgnoreToolbarEvents = true;
				
				for(int i = 0; i < m_tmaxAppOptions.Highlighters.Count; i++)
				{
					// Format the key for the menu tool
					strKey = String.Format("SetHighlighter{0}", i + 1);
					
					if((toolHighlighter = (StateButtonTool)GetUltraTool(strKey)) != null)
					{
						tmaxHighlighter = m_tmaxAppOptions.Highlighters[i];
						
						if((tmaxHighlighter != null) && (tmaxHighlighter.Label.Length > 0))
						{
							toolHighlighter.SharedProps.Caption = tmaxHighlighter.Label;
							
							if(ReferenceEquals(tmaxHighlighter, m_tmaxHighlighter) == true)
								toolHighlighter.Checked = true;
							else
								toolHighlighter.Checked = false;
							
							toolHighlighter.SharedProps.Visible = true;
						}
						else
						{
							toolHighlighter.SharedProps.Visible = false;
						}
					
					}// if((toolHighlighter = (StateButtonTool)GetUltraTool(strKey)) != null)
				
				}// for(int i = 0; i < m_tmaxAppOptions.Highlighters.Count; i++)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnSetHighlighter", Ex);
			}
			finally
			{
				m_bIgnoreToolbarEvents = false;
			}			
			
		}// private void OnSetHighlighter(PopupMenuTool popupMenu)

		/// <summary>This method will process the specified command</summary>
		/// <param name="eCommand">The command to be processed</param>
		private void OnCommand(TmaxVideoTranscriptCommands eCommand)
		{
			long lSelections = 0;
			
			//	Get the current number of row selections
			if((m_ctrlTransGrid != null) && (m_ctrlTransGrid.IsDisposed == false))
				lSelections = m_ctrlTransGrid.GetSelectedCount();
			
			try
			{	
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case TmaxVideoTranscriptCommands.Preview:
					
						OnCmdPreview();
						break;
						
					case TmaxVideoTranscriptCommands.Add:
					
						if(lSelections > 0)
							OnCmdAdd(false, false);
						break;
						
					case TmaxVideoTranscriptCommands.InsertBefore:
					
						if(lSelections > 0)
							OnCmdAdd(true, true);
						break;
						
					case TmaxVideoTranscriptCommands.InsertAfter:
					
						if(lSelections > 0)
							OnCmdAdd(true, false);
						break;
						
					case TmaxVideoTranscriptCommands.EditExtents:
					case TmaxVideoTranscriptCommands.Exclude:
					case TmaxVideoTranscriptCommands.SplitBefore:
					case TmaxVideoTranscriptCommands.SplitAfter:
					
						if(lSelections > 0)
							OnCmdEditDesignation(eCommand);
						break;
						
					case TmaxVideoTranscriptCommands.SetHighlighter1:
					case TmaxVideoTranscriptCommands.SetHighlighter2:
					case TmaxVideoTranscriptCommands.SetHighlighter3:
					case TmaxVideoTranscriptCommands.SetHighlighter4:
					case TmaxVideoTranscriptCommands.SetHighlighter5:
					case TmaxVideoTranscriptCommands.SetHighlighter6:
					case TmaxVideoTranscriptCommands.SetHighlighter7:
					
						OnCmdSetHighlighter((int)eCommand);
						break;
						
					case TmaxVideoTranscriptCommands.AssignHighlighters:
					
						OnCmdAssignHighlighters();
						break;
						
					case TmaxVideoTranscriptCommands.GoTo:
					
						OnCmdGoTo();
						break;
						
					case TmaxVideoTranscriptCommands.Find:
					
						OnCmdFind();
						break;
						
					default:
					
						Debug.Assert(false, "Unknown command identifier: " + eCommand.ToString());
						break;
				
				}// switch(eCommand)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCommand", m_tmaxErrorBuilder.Message(ERROR_ON_COMMAND_EX, eCommand), Ex);
			}
		
		}// private void OnCommand(TmaxVideoTranscriptCommands eCommand)

		/// <summary>Toolbar event handler for Add command</summary>
		/// <param name="bInsert">true to insert the current selection</param>
		/// <param name="bBefore">true to save before the current scene</param>
		private void OnCmdAdd(bool bInsert, bool bBefore)
		{
			Debug.Assert(m_xmlScript != null);
		
			AddSelection(bInsert, bBefore);
			
			//	Make sure this pane retains the focus
			this.Select();
			
		}// private void OnCmdAdd(bool bInsert, bool bBefore)
		
		/// <summary>Toolbar event handler for SetHighlighter commands</summary>
		/// <param name="iCommand">The command identifier</param>
		private void OnCmdSetHighlighter(int iCommand)
		{
			int iIndex = 0;
			
			if(m_tmaxAppOptions == null) return;
			if(m_tmaxAppOptions.Highlighters == null) return;
			if(m_tmaxAppOptions.Highlighters.Count == 0) return;
			
			iIndex = (iCommand - (int)TmaxVideoTranscriptCommands.SetHighlighter1);
			
			if(iIndex < 0) return;
			if(iIndex >= m_tmaxAppOptions.Highlighters.Count) return;
			
			if(m_tmaxAppOptions.Highlighters[iIndex] != null)
			{
				SetHighlighter(m_tmaxAppOptions.Highlighters[iIndex], true);
			}		
		
		}// private void OnCmdSetHighlighter(int iCommand)
		
		/// <summary>This method handles the event fired when the user clicks on Find from the context menu</summary>
		private void OnCmdFind()
		{
			try
			{
				//	Fire the application command
				FireCommand(TmaxVideoCommands.Find);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdFind", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_FIND_EX), Ex);
			}
						
		}// private void OnCmdFind(CTmaxTreeNode tmaxNodes)
		
		/// <summary>This method handles the event fired when the user clicks on GoTo from the context menu</summary>
		private void OnCmdGoTo()
		{
			CFGoTranscript goTranscript = null;
			
			//	Command should be disabled if no deposition available
			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return;
			Debug.Assert(m_xmlScript.XmlDeposition != null);
			if(m_xmlScript.XmlDeposition == null) return;
			
			try
			{
				//	Allocate a new form
				goTranscript = new CFGoTranscript();
				
				//	Set the form properties
				goTranscript.TranscriptName = m_xmlScript.XmlDeposition.Name;
				goTranscript.FirstPL = m_xmlScript.XmlDeposition.GetFirstPL();
				goTranscript.LastPL = m_xmlScript.XmlDeposition.GetLastPL();
				goTranscript.LinesPerPage = m_xmlScript.XmlDeposition.LinesPerPage;
				
				//	Show the form
				if(goTranscript.ShowDialog() == DialogResult.OK)
				{
					//	Select the user specified page/line
					m_ctrlTransGrid.SetSelection(goTranscript.GoPL, true, false);
				}
				
				goTranscript.Dispose();
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdGoTo", m_tmaxErrorBuilder.Message(ERROR_GO_TO_EX), Ex);
			}
			
		}// private void OnCmdGoTo()
		
		/// <summary>This method handles the event fired when the user clicks on Preview from the context menu</summary>
		private void OnCmdPreview()
		{
			CXmlDesignations	xmlDesignations = null;
			CXmlDesignation		xmlDesignation = null;
			string				strMsg = "";
			string				strFileSpec = "";

			try
			{
				while(true)
				{
					//	Create the designations for the current selection
					xmlDesignations = CreateDesignations();
					if((xmlDesignations == null) || (xmlDesignations.Count == 0))
						break;
					
					//	Did the selections break across segments?
					if(xmlDesignations.Count > 1)
					{
						strMsg = String.Format("The current selection will result in {0} designations. Preview will only show the first designation\n\n", xmlDesignations.Count);
					
						foreach(CXmlDesignation O in xmlDesignations)
							strMsg += (O.Name + "\n");
						
						strMsg += "\nDo you want to continue?";
					
						//	Prompt the user for confirmation before continuing
						if(MessageBox.Show(strMsg, "Please Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							break;
					}
					
					//	Set the playback designation
					xmlDesignation = xmlDesignations[0];
					Debug.Assert(xmlDesignation != null);
					
					//	Make sure the designation contains synchronized text
					if(xmlDesignation.GetSynchronized(false) == false)
					{
						strMsg = String.Format("Unable to start the preview. The selection does not contain synchronized text.");
						Warn(strMsg);
						break;
					}
					
					//	Get the path to the video file
					strFileSpec = GetVideoFileSpec(xmlDesignation, true, false);
					if((strFileSpec == null) || (strFileSpec.Length == 0))
						break;
						
	
					//	Store these members locally so that we can process events from the preview form
					m_xmlPreviewDesignation = xmlDesignation;
					m_aPreviewSelections = m_ctrlTransGrid.GetSelectedRows();

					//	Inhibit grid events until we're done
					m_bIgnoreGridEvents = true;
					
					//	Clear the current selections
					ClearSelections(false);

					//	Create a new preview form
					CFPreview preview = new FTI.Trialmax.Forms.CFPreview();
					m_tmaxEventSource.Attach(preview.EventSource);
					preview.Player.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrl);
				
					preview.SetProperties(strFileSpec, xmlDesignation);
					preview.ShowDialog();
				
					//	Restore the selections
					if(m_aPreviewSelections != null)
						m_ctrlTransGrid.SetSelections(m_aPreviewSelections);
						
					//	We're done
					break;
				
				}// while(true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "PreviewSelection", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_PREVIEW_EX), Ex);
			}
			finally
			{
				m_xmlPreviewDesignation = null;
				m_aPreviewSelections = null;
				m_bIgnoreGridEvents = false;
			}				

		}// private void OnCmdPreview()
		
		/// <summary>Toolbar event handler for designation editing commands</summary>
		/// <param name="eCommand">The enumerated editing command identifier (split,exclude,extents)</param>
		private void OnCmdEditDesignation(TmaxVideoTranscriptCommands eCommand)
		{
			TmaxDesignationEditMethods	eMethod = TmaxDesignationEditMethods.Unknown;
			long						lStartPL = 0;
			long						lStopPL = 0;
			
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return;

			try
			{
				//	Get the appropriate edit method
				switch(eCommand)
				{
					case TmaxVideoTranscriptCommands.EditExtents:	
						eMethod = TmaxDesignationEditMethods.Extents;
						break;
					case TmaxVideoTranscriptCommands.Exclude:		
						eMethod = TmaxDesignationEditMethods.Exclude;
						break;
					case TmaxVideoTranscriptCommands.SplitBefore:	
						eMethod = TmaxDesignationEditMethods.SplitBefore;
						break;
					case TmaxVideoTranscriptCommands.SplitAfter:		
						eMethod = TmaxDesignationEditMethods.SplitAfter;
						break;
					default:									
						Debug.Assert(false, "Invalid Edit Command: " + eCommand.ToString());
						return;
		
				}// switch(eCommand)
			
				//	Get the selection range
				if(m_ctrlTransGrid.GetSelectionRange(ref lStartPL, ref lStopPL) == 0)
				{
					Warn("You must select some text within the active designation");
					return;
				}
			
				//	Fire the required command
				FireEditCommand(eMethod, lStartPL, lStopPL);

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdEditDesignation", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_EDIT_EX, eMethod.ToString()), Ex);
			}
			
		}// private void OnCmdEditDesignation(TmaxDesignationEditMethods eMethod)
		
		/// <summary>Toolbar event handler for AssignHighlighters command</summary>
		private void OnCmdAssignHighlighters()
		{
			CTmaxParameters tmaxParameters = null;
			
			try
			{
				//	Populate the parameters collection
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.InitialPage, (int)TmaxVideoPreferencesPages.Highlighters);
				
				//	Fire the command event
				FireCommand(TmaxVideoCommands.SetPreferences, (CTmaxItems)null, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdAssignHighlighters", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_ASSIGN_HIGHLIGHTERS_EX), Ex);
			}
			
		}// private void OnCmdAssignHighlighters()
		
		/// <summary>Handles events fired by the grid when the selected rows change</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnGridSelChanged(object sender, System.EventArgs e)
		{
			//	Should we process the notification?
			if(m_bIgnoreGridEvents == false)
			{
				try { SetToolStates(null, false); }
				catch {};
			}
		
		}// private void OnGridSelChanged(object sender, System.EventArgs e)
	
		/// <summary>Handles events fired by the grid when the selected rows change</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnGridDblClick(object sender, System.EventArgs e)
		{
			CXmlTranscript xmlTranscript = null;
			
			while(true)
			{
				//	Are we ignoring grid events?
				if(m_bIgnoreGridEvents == true)
					break;
					
				//	We must have an active designation
				if(m_xmlDesignation == null)
					break;
				Debug.Assert(m_xmlScript != null);
				if(m_xmlScript == null) break;
				
				//	Get the XML transcript at the click location
				if((xmlTranscript = m_ctrlTransGrid.GetCursorTranscript()) == null)
					break;
					
				//	Is this transcript within range for the active designation
				if(xmlTranscript.PL <= m_xmlDesignation.FirstPL)
					break;
				if(xmlTranscript.PL > m_xmlDesignation.LastPL)
					break;

				//	Don't bother if not synchronized
				if(xmlTranscript.Synchronized == false)
					break;
					
				//	Ignore this line if it does not exceed the Pause period
				if((xmlTranscript.Stop - xmlTranscript.Start) < m_tmaxAppOptions.PauseThreshold)
					break;
						
				//	Fire the command to split the designation
				FireEditCommand(TmaxDesignationEditMethods.SplitBefore, xmlTranscript.PL, xmlTranscript.PL);
				
				break;
						
			}
		
		}// private void OnGridDblClick(object sender, System.EventArgs e)
	
		/// <summary>This method is called to fire the requested edit command</summary>
		/// <param name="eMethod">The enumerated editing method (split,exclude,extents)</param>
		/// <param name="lStartPL">The start position</param>
		/// <param name="lStopPL">The stop position</param>
		/// <returns>true if successful</returns>
		private bool FireEditCommand(TmaxDesignationEditMethods eMethod, long lStartPL, long lStopPL)
		{
			CTmaxVideoArgs				Args = null;
			CTmaxParameters				tmaxParameters = null;
			CTmaxItem					tmaxScript = null;
			bool						bSuccessful = false;
			
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return false;

			try
			{
				//	Allocate and initialize the event item to represent the script and designation
				tmaxScript = new CTmaxItem(m_xmlScript, m_xmlDesignation);
				
				//	Populate the parameters collection
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.EditMethod, (int)eMethod);
				tmaxParameters.Add(TmaxCommandParameters.StartPL, lStartPL);
				tmaxParameters.Add(TmaxCommandParameters.StopPL, lStopPL);
				
				//	Fire the command event
				if((Args = FireCommand(TmaxVideoCommands.EditDesignation, tmaxScript, tmaxParameters)) != null)
				{
					//	We're we successful?
					if(Args.Successful == true)
					{
						ClearSelections(true);
						bSuccessful = true;
					}
					
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireEditCommand", m_tmaxErrorBuilder.Message(ERROR_FIRE_EDIT_COMMAND_EX, eMethod.ToString()), Ex);
			}
			
			return bSuccessful;
			
		}// private bool FireEditCommand(TmaxVideoTranscriptCommands eCommand, long lStartPL, long lStopPL)
		
		/// <summary>This method creates the designations that represent the current selection</summary>
		/// <returns>The collection of designations that match the rows selected in the grid</returns>
		private CXmlDesignations CreateDesignations()
		{
			CXmlDesignations		xmlDesignations = null;
			CTmaxHighlighter	tmaxHighlighter = null;
			long					lHighlighter = 1;
			long					lStartPL = 0;
			long					lStopPL = 0;

			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return null;
			Debug.Assert(m_xmlScript.XmlDeposition != null);
			if(m_xmlScript.XmlDeposition == null) return null;
			
			try
			{
				//	Get the current selections
				if(m_ctrlTransGrid.GetSelectionRange(ref lStartPL, ref lStopPL) > 0)
				{
					//	Get the highlighter identifier
					if((tmaxHighlighter = GetHighlighter()) != null)
						lHighlighter = tmaxHighlighter.Id;
						
					//	Get the designations that will result from the selection
					xmlDesignations = new CXmlDesignations();
					m_xmlScript.XmlDeposition.CreateDesignations(xmlDesignations, lStartPL, lStopPL, (int)lHighlighter);
					if((xmlDesignations == null) || (xmlDesignations.Count == 0))
					{
						Warn("Unable to create the designations required for playback.");
						xmlDesignations = null;
					}
					
				}
				else
				{
					Warn("You must select one or more rows of transcript text.");
				
				}// if(m_ctrlTransGrid.GetSelectionRange(ref lStartPL, ref lStopPL) > 0)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDesignations", m_tmaxErrorBuilder.Message(ERROR_CREATE_DESIGNATIONS_EX), Ex);
			}
			
			return xmlDesignations;

		}// private CXmlDesignations CreateDesignations()
		
		/// <summary>Clears the current selections in the grid</summary>
		/// <param name="bInhibitEvents">True to inhibit grid events</param>
		private void ClearSelections(bool bInhibitEvents)
		{
			if((m_ctrlTransGrid != null) && (m_ctrlTransGrid.IsDisposed == false))
			{
				try
				{
					if(bInhibitEvents)
						m_bIgnoreGridEvents = true;
						
					m_ctrlTransGrid.SetSelections(null);
					
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "ClearSelections", Ex);
				}
				finally
				{
					//	Should we re-enable the grid events?
					if(bInhibitEvents == true)
						m_bIgnoreGridEvents = false;
						
					//	Make sure commands are properly enabled/disabled
					SetToolStates(null, false);
				}
				
			}// if((m_ctrlTransGrid != null) && (m_ctrlTransGrid.IsDisposed == false))
			
		}// private void ClearSelections(bool bInhibitEvents)
		
		/// <summary>This function is called to get the active highlighter</summary>
		/// <returns>The active highlighter</returns>
		private CTmaxHighlighter GetHighlighter()
		{
			//	Do we need to activate a highlighter
			if(m_tmaxHighlighter == null)
			{
				if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.Highlighters != null))
				{
					if(m_tmaxAppOptions.Highlighters.Count > 0)
					{
						//	Make the first one active
						SetHighlighter(m_tmaxAppOptions.Highlighters[0], true);
					}
					
				}// if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.Highlighters != null))
				
			}// if(m_tmaxHighlighter == null)
			
			return m_tmaxHighlighter;
			
		}// private CTmaxHighlighter GetHighlighter()
		
		/// <summary>This function is called to set the active highlighter</summary>
		/// <param name="tmaxHighlighter">The highlighter to be activated</param>
		/// <param name="bSynchronize">True to synchronize the selection in the drop down list</param>
		/// <returns>True if successsful</returns>
		private bool SetHighlighter(CTmaxHighlighter tmaxHighlighter, bool bSynchronize)
		{
			ComboBoxTool	ctrlHighlighters = null;
			ValueListItem	vlItem = null;
		
			//	Prevent responding to events while we make the change
			//
			//	NOTE:	This may be getting called as a result of the user
			//			making a new selection in the combobox
			if(m_bIgnoreToolbarEvents == false)
				m_bIgnoreToolbarEvents = true;
			else
				return false;	//	Must be selecting locally
			
			//	Update the application options
			if((m_tmaxAppOptions != null) && (tmaxHighlighter != null))
				m_tmaxAppOptions.LastHighlighter = tmaxHighlighter.Id;
			
			//	Update the local reference
			m_tmaxHighlighter = tmaxHighlighter;
			
			//	Should we synchronize the selection in the drop down list?
			if(bSynchronize == true)
			{
				//	Make sure we have the required objects
				if((ctrlHighlighters = GetUltraHighlighters()) == null) return false;
				if(ctrlHighlighters.ValueList == null) return false;
				if(ctrlHighlighters.ValueList.ValueListItems == null) return false;
				
				try
				{
					//	Clear the current selection
					ctrlHighlighters.SelectedItem = null;
					
					if(tmaxHighlighter != null)
					{	
						foreach(Infragistics.Win.ValueListItem O in ctrlHighlighters.ValueList.ValueListItems)
						{
							if(ReferenceEquals(O.DataValue, tmaxHighlighter) == true)
							{
								ctrlHighlighters.SelectedItem = O;
								vlItem = O;
								break;
							}
							
						}
					
					}// if(tmaxHighlighter != null)
				
					//	Move to the top of the list
					if(vlItem != null)
					{
						ctrlHighlighters.ValueList.ValueListItems.Remove(vlItem);
						ctrlHighlighters.ValueList.ValueListItems.Insert(0, vlItem);
					}
				
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "SetHighlighterSelection", m_tmaxErrorBuilder.Message(ERROR_SET_HIGHLIGHTER_EX), Ex);
				}
				
			}//	if(bSynchronize == true)
			
			//	Clear the flag that inhibits the event processing
			m_bIgnoreToolbarEvents = false;
			
			return true;
			
		}// private bool SetHighlighter(CTmaxHighlighter tmaxHighlighter)
		
		/// <summary>This method is retrieve the highlighters combobox</summary>
		private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetUltraHighlighters()
		{
			try
			{
				return ((ComboBoxTool)GetUltraTool(ULTRA_HIGHLIGHTERS_KEY));
			}
			catch
			{
				return null;
			}
	
		}// private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetHighlightersBox()
		
		/// <summary>This method is called to fill the highlighters combobox</summary>
		/// <returns>true if successful</returns>
		private bool FillHighlighters()
		{
			ComboBoxTool			ctrlHighlighters = GetUltraHighlighters();
			CTmaxHighlighters	tmaxHighlighters = null;
			CTmaxHighlighter	tmaxHighlighter = null;
			bool					bSuccessful = false;
			
			//	Clear the active highlighter
			m_tmaxHighlighter = null;
			
			Debug.Assert(ctrlHighlighters != null);
			if(ctrlHighlighters == null) return false;
			if(ctrlHighlighters.ValueList == null) return false;
			if(ctrlHighlighters.ValueList.ValueListItems == null) return false;
			
			try
			{
				//	Clear the existing highlighters
				ctrlHighlighters.ValueList.ValueListItems.Clear();
				
				//	Get the application's highlighter collection
				if((tmaxHighlighters = m_tmaxAppOptions.Highlighters) != null)
				{
					//	Add each assigned highlighter
					foreach(CTmaxHighlighter O in tmaxHighlighters)
					{
						if((O.Label != null) && (O.Label.Length > 0))
							ctrlHighlighters.ValueList.ValueListItems.Add(O, O.Label);
				
					}// foreach(CTmaxHighlighter O in tmaxHighlighters)
			
					//	Get the last highlighter that was used
					if(m_tmaxAppOptions.LastHighlighter > 0)
						tmaxHighlighter = tmaxHighlighters.Find(m_tmaxAppOptions.LastHighlighter);
					if((tmaxHighlighter == null) && (tmaxHighlighters.Count > 0))
						tmaxHighlighter = tmaxHighlighters[0];
						
					//	Make this the active highlighter
					SetHighlighter(tmaxHighlighter, true);
						
					bSuccessful = true;
					
				}// if((tmaxHighlighters = m_tmaxAppOptions.Highlighters) != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillHighlighters", m_tmaxErrorBuilder.Message(ERROR_FILL_HIGHLIGHTERS_EX), Ex);
			}
			
			return bSuccessful;
		
		}// private bool FillHighlighters()
		
		/// <summary>This method is called when the user selects a new highlighter from the drop down list</summary>
		private void OnHighlighterSelChanged()
		{
			CTmaxHighlighter	tmaxHighlighter = null;
			ComboBoxTool			ctrlHighlighters = null;
			ValueListItem			vlItem = null;
			
			if(m_bIgnoreToolbarEvents == true) return;
			
			//	Get the combo box
			if((ctrlHighlighters = GetUltraHighlighters()) == null) return;
			
			//	Is there a selection?
			if((vlItem = (ValueListItem)ctrlHighlighters.SelectedItem) != null)
			{
				tmaxHighlighter = (CTmaxHighlighter)(vlItem.DataValue);
				
				//	Move this item to the top of the list
				m_bIgnoreToolbarEvents = true;
				ctrlHighlighters.ValueList.ValueListItems.Remove(vlItem);
				ctrlHighlighters.ValueList.ValueListItems.Insert(0, vlItem);
				ctrlHighlighters.SelectedItem = vlItem;
				m_bIgnoreToolbarEvents = false;
			}

			//	Update the current selection
			SetHighlighter(tmaxHighlighter, false);
			
		}// private void OnHighlightersSelChanged()

		/// <summary>This method is called to handle a TrialMax video playback (TmxVideoEvent) event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">event arguments</param>
		private void OnTmaxVideoCtrl(object sender, CTmaxVideoCtrlEventArgs e)
		{
			//	Which event?
			switch(e.EventId)
			{
				case TmaxVideoCtrlEvents.PlayerTranscriptChanged:
				
					OnPreviewTranscriptChanged(e.XmlDesignation, e.TranscriptIndex);
					break;

				case TmaxVideoCtrlEvents.PlayerPositionChanged:
				case TmaxVideoCtrlEvents.PlayerStateChanged:
				case TmaxVideoCtrlEvents.SetMode:
				case TmaxVideoCtrlEvents.SetPreviewPeriod:
				case TmaxVideoCtrlEvents.Apply:
				case TmaxVideoCtrlEvents.QueryPlayerPosition:
				default:
				
					break;
					
			}
			
		}// private virtual void OnTmaxVideoCtrl(object sender, CTmaxVideoCtrlEventArgs e)
	
		#endregion Private Methods

	}//  public class CTmaxVideoTranscript : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView

}//  namespace FTI.Trialmax.TMVV.Tmvideo
