using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.Controls
{
	/// <summary>This control is used to select and navigate a collection of scripts</summary>
	public class CTmaxScriptBoxCtrl : System.Windows.Forms.UserControl
	{
		#region Constants
		
		//	Toolbar command enumerations
		protected enum TmaxScriptBoxCommands
		{
			Invalid = 0,
			Play,
			PlayThrough,
			PlayAll,
			First,
			Previous,
			Next,
			Last,
		}
		
		//	Error identifiers
		protected const int ERROR_SET_SCRIPTS_EX = 0;
		protected const int ERROR_SET_SCENES_EX = 1;
		protected const int ERROR_SCRIPT_CHANGED_EVENT_EX = 2;
		protected const int ERROR_SCENE_CHANGED_EVENT_EX = 3;
		protected const int ERROR_PLAY_EVENT_EX = 4;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Image list used for toolbar buttons</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		///	<summary>Local flag to inhibit event processing</summary>
		private bool m_bIgnoreEvents = false;

		///	<summary>Local flag to keep track of script playback</summary>
		private bool m_bPlayingScript = false;

		/// <summary>Infragistics toolbar manager control for creating the toolbar</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;

		/// <summary>Infragistics toolbar manager docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxScriptCtrl_Toolbars_Dock_Area_Left;

		/// <summary>Infragistics toolbar manager docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxScriptCtrl_Toolbars_Dock_Area_Right;

		/// <summary>Infragistics toolbar manager docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxScriptCtrl_Toolbars_Dock_Area_Top;

		/// <summary>Infragistics toolbar manager docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom;

		/// <summary>Infragistics combo box control to list available scripts</summary>
		Infragistics.Win.UltraWinToolbars.ComboBoxTool m_ctrlScripts = null;

		/// <summary>Infragistics label for scripts combobox</summary>
		Infragistics.Win.UltraWinToolbars.LabelTool m_ctrlScriptsLabel = null;

		/// <summary>Background fill panel to support docking panes</summary>
		private System.Windows.Forms.Panel m_ctrlFillPanel;

		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>List of scripts displayed in the combo box</summary>
		private IList m_IScripts = null;
		
		/// <summary>List of scenes associated with the current script</summary>
		private IList m_IScenes = null;
		
		/// <summary>The active script</summary>
		private object m_oScript = null;
		
		/// <summary>The active scene</summary>
		private object m_oScene = null;
		
		/// <summary>Local member bound to ShowScripts property</summary>
		private bool m_bShowScripts = true;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxScriptBoxCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			//	Set the default event source name
			m_tmaxEventSource.Name = "Scripts Combobox";
			
			m_ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts");
			m_ctrlScriptsLabel = (LabelTool)GetUltraTool("ScriptsLabel");

			//	Initialize the error builder
			SetErrorStrings();
			
			//	Initilize the toolbar commands
			SetShowScripts(m_bShowScripts);
			SetCommandStates();
		}

		/// <summary>This method is called to populate the combo box using the specified list of scripts</summary>
		/// <param name="IScripts">The new list of script objects</param>
		/// <returns>true if successful</returns>
		public bool SetScripts(IList IScripts)
		{
			bool bComplete = false;
			
			Debug.Assert(m_ctrlScripts != null);
			if(m_ctrlScripts == null) return false;
			
			try
			{
				//	Clear the existing scripts
				m_ctrlScripts.ValueList.ValueListItems.Clear();
				
				//	Reset the local members
				m_oScript = null;
				m_IScenes = null;
				m_oScene  = null;
				
				//	Now repopulate with the new list
				if((IScripts != null) && (IScripts.Count > 0))
				{
					foreach(ITmaxScriptBoxCtrl O in IScripts)
					{
						m_ctrlScripts.ValueList.ValueListItems.Add(O, GetDisplayText(O));
					}
				
				}
				
				//	Store the new reference
				m_IScripts = IScripts;
				
				bComplete = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScripts", m_tmaxErrorBuilder.Message(ERROR_SET_SCRIPTS_EX), Ex);
				m_IScripts = null; // disables commands
			}
			finally
			{
				SetCommandStates();
			}

			return bComplete;
		
		}// public bool SetScripts(IList IScripts)
		
		/// <summary>This method is called to set the list of scenes associated with the current script</summary>
		/// <param name="IScripts">The new list of scene objects</param>
		/// <returns>true if successful</returns>
		public bool SetScenes(IList IScenes)
		{
			bool bComplete = false;

			try
			{
				//	Reset the local members
				m_IScenes = IScenes;
				m_oScene  = null;
				
				bComplete = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScenes", m_tmaxErrorBuilder.Message(ERROR_SET_SCENES_EX), Ex);
				m_IScenes = null; // disables commands
				m_oScene = null;
			}
			finally
			{
				SetCommandStates();
			}
			
			return bComplete;
			
		}// public bool SetScenes(IList IScenes)
		
		/// <summary>This method is called to set the active scene</summary>
		/// <param name="oScene">The scene to be activated</param>
		/// <param name="bSilent">true to prevent firing ScriptChanged event</param>
		/// <returns>true if successful</returns>
		public bool SetScene(object oScene, bool bSilent)
		{
			if(m_IScenes == null) return false;
			if(m_IScenes.Count == 0) return false;

			if(oScene != null)
			{
				if(m_IScenes.Contains(oScene) == false)
					return false;
			}

			m_oScene = oScene;
			SetCommandStates();
			
			//	Should we fire the event?
			if((bSilent == false) && (SceneChangedEvent != null))
			{
				try
				{
					SceneChangedEvent(this, m_oScript, m_oScene);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "SetScene", m_tmaxErrorBuilder.Message(ERROR_SCENE_CHANGED_EVENT_EX), Ex);
				}
				
			}
			
			return true;
			
		}// public bool SetScene(object oScene)
		
		/// <summary>This method is called to set the active scene</summary>
		/// <param name="oScene">The scene to be activated</param>
		/// <returns>true if successful</returns>
		public bool SetScene(object oScene)
		{
			return SetScene(oScene, false);	
		}
			
		/// <summary>This method is called to set the active script</summary>
		/// <param name="oScript">The script to be activated</param>
		/// <param name="bSilent">true to prevent firing ScriptChanged event</param>
		/// <returns>true if successful</returns>
		public bool SetScript(object oScript, bool bSilent)
		{
			int iIndex = -1;
			
			if(m_IScripts == null) return false;
			if(m_IScripts.Count == 0) return false;

			if(oScript != null)
			{
				if((iIndex = m_IScripts.IndexOf(oScript)) < 0)
					return false;
			}

			//	Select this item in the list box
			try
			{
				m_bIgnoreEvents = true;
				m_ctrlScripts.SelectedIndex = iIndex;
			}
			catch
			{
			}
			
			m_bIgnoreEvents = false;
			
			//	Reset the local members
			m_oScript = oScript;
			m_IScenes = null;
			m_oScene  = null;
				
			SetCommandStates();

			//	Should we fire the event?
			if((bSilent == false) && (ScriptChangedEvent != null))
			{
				try
				{
					ScriptChangedEvent(this, m_oScript);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "SetScript", m_tmaxErrorBuilder.Message(ERROR_SCRIPT_CHANGED_EVENT_EX), Ex);
				}
				
			}
			
			return true;
			
		}// public bool SetScene(object oScene)
		
		/// <summary>This method is called to set the active script</summary>
		/// <param name="oScript">The script to be activated</param>
		/// <returns>true if successful</returns>
		public bool SetScript(object oScript)
		{
			return SetScript(oScript, false);
		}
		
		/// <summary>This method is called to refresh the scripts collection</summary>
		/// <param name="bSilent">true to inhibit script change events</param>
		/// <returns>true if successful</returns>
		public bool RefreshScripts(bool bSilent)
		{
			bool bScriptChanged = false;
			int	 iIndex = -1;
			
			if(m_IScripts == null) return false;
			if(m_ctrlScripts == null) return false;
			
			try
			{
				//	Ignore events while we do this processing
				m_bIgnoreEvents = true;
			
				//	Clear the existing scripts
				m_ctrlScripts.ValueList.ValueListItems.Clear();
				
				//	Now rebuild the list
				if(m_IScripts.Count > 0)
				{
					foreach(ITmaxScriptBoxCtrl O in m_IScripts)
					{
						m_ctrlScripts.ValueList.ValueListItems.Add(O, GetDisplayText(O));
					}
					
				}
				
				//	Is there an active script selection?
				if(m_oScript != null)
				{
					//	Is this script still in the list?
					if((iIndex = m_IScripts.IndexOf(m_oScript)) >= 0)
					{
						//	Make sure it is selected
						m_ctrlScripts.SelectedIndex = iIndex;
					}
					else
					{
						//	Clear the selection
						m_ctrlScripts.SelectedIndex = -1;
						bScriptChanged = true;
					}
						
				}

			}
			catch
			{
			}
			finally
			{			
				m_bIgnoreEvents = false;
			
				SetCommandStates();
			}

			//	Do we have to reset the script?
			if(bScriptChanged == true)
				SetScript(null, bSilent);
				
			return true;
			
		}// public bool RefreshScripts(bool bSilent)
		
		/// <summary>This method is called when the user wants to start playing a script</summary>
		/// <returns>true if successful</returns>
		public bool StartScript()
		{
			m_bPlayingScript = true;
			SetCommandStates();
			return true;
							
		}// public virtual bool StartScript()
		
		/// <summary>This method is called when the user wants to stop playing a script</summary>
		/// <returns>true if successful</returns>
		public bool StopScript()
		{
			m_bPlayingScript = false;
			SetCommandStates();
			SetPlayButtonStates(TmaxScriptBoxCommands.Invalid);
			return true;
			
		}// public virtual bool StopScript()
		
		/// <summary>This method is called to refresh the scripts collection</summary>
		/// <returns>true if successful</returns>
		public bool RefreshScripts()
		{
			return RefreshScripts(false);
		}
		
		/// <summary>This method is called to refresh the scenes collection</summary>
		/// <param name="bSilent">true to inhibit script change events</param>
		/// <returns>true if successful</returns>
		public bool RefreshScenes(bool bSilent)
		{
			if(m_IScenes == null) return false;
			
			try
			{
				//	Is there an active scene selection?
				if(m_oScene != null)
				{
					//	Is this scene no longer in the list?
					if(m_IScenes.Contains(m_oScene) == false)
					{
						//	Clear the current selection
						SetScene(null, bSilent);
					}
					else
					{
						//	Make sure the command buttons are updated
						SetCommandStates();
					}
						
				}

			}
			catch
			{
			}
				
			return true;
			
		}// public bool RefreshScenes(bool bSilent)
		
		/// <summary>This method is called to refresh the scenes collection</summary>
		/// <returns>true if successful</returns>
		public bool RefreshScenes()
		{
			return RefreshScenes(false);
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to set the check states of the playback buttons</summary>
		/// <returns>true if successful</returns>
		protected void SetPlayButtonStates(TmaxScriptBoxCommands eCommand)
		{
			StateButtonTool playButton = null;
			
			try
			{
				m_bIgnoreEvents = true;
				
				//	Uncheck the playback buttons
				if((playButton = (StateButtonTool)GetUltraTool(TmaxScriptBoxCommands.Play.ToString())) != null)
					playButton.Checked = (eCommand == TmaxScriptBoxCommands.Play);
				if((playButton = (StateButtonTool)GetUltraTool(TmaxScriptBoxCommands.PlayAll.ToString())) != null)
					playButton.Checked = (eCommand == TmaxScriptBoxCommands.PlayAll);
				if((playButton = (StateButtonTool)GetUltraTool(TmaxScriptBoxCommands.PlayThrough.ToString())) != null)
					playButton.Checked = (eCommand == TmaxScriptBoxCommands.PlayThrough);
			}
			catch
			{
			}
			finally
			{
				m_bIgnoreEvents = false;
			}
			
		}// public void SetPlayButtonStates(TmaxScriptBoxCommands eCommand)
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				
				if(m_tmaxErrorBuilder != null)
				{
					if(m_tmaxErrorBuilder.FormatStrings != null)
						m_tmaxErrorBuilder.FormatStrings.Clear();
						
					m_tmaxErrorBuilder = null;
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>This method is called to enable/disable the command buttons</summary>
		protected void SetCommandStates()
		{
			TmaxScriptBoxCommands eCommand;
			
			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) ||( m_ultraToolbarManager.Tools == null)) return;
			
			//	Enable / disable the combo box
			if(m_ctrlScripts != null)
			{
				m_ctrlScripts.SharedProps.Enabled = ((m_IScripts != null) && (m_bPlayingScript == false));
				m_ctrlScripts.SharedProps.Visible = m_bShowScripts;
			}
			if(m_ctrlScriptsLabel != null)
			{
				m_ctrlScriptsLabel.SharedProps.Enabled = ((m_IScripts != null) && (m_bPlayingScript == false));
				m_ctrlScriptsLabel.SharedProps.Visible = m_bShowScripts;
			}
			
			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				if(O.Key == null) 
					continue;

				if((eCommand = GetCommand(O.Key)) == TmaxScriptBoxCommands.Invalid)
					continue;
					
				try
				{
					//	Should the command be enabled?
					O.SharedProps.Enabled = GetCommandEnabled(eCommand);
				}
				catch
				{
				}
				
			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)

		}// protected void SetCommandStates()

		/// <summary>This method is called to set the ShowScripts property</summary>
		/// <param name="bShowScripts">True to show the scripts combobox</param>
		public void SetShowScripts(bool bShowScripts)
		{
			Infragistics.Win.UltraWinToolbars.UltraToolbar scriptBar = null;
			
			//	Update the class member
			m_bShowScripts = bShowScripts;
			
			try
			{
				//	Get the scripts toolbar
				if((scriptBar = m_ultraToolbarManager.Toolbars["ScriptsBar"]) != null)
					scriptBar.Visible = m_bShowScripts;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetShowScripts", Ex);
			}
				
			SetCommandStates();
		}
			
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the scripts list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the scenes list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the script changed event.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the scene changed event.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the play changed event.");

		}// protected void SetErrorStrings()

		/// <summary>
		/// This method is called internally to convert the key passed in
		///	an Infragistics event to its associated command enumeration
		/// </summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated command</returns>
		protected TmaxScriptBoxCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TmaxScriptBoxCommands));
				
				foreach(TmaxScriptBoxCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TmaxScriptBoxCommands.Invalid;
		}
		
		/// <summary>This function will retrieve the tool with the specified key from the toolbar manager</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		protected ToolBase GetUltraTool(string strKey)
		{
			ToolBase Tool = null;
					
			try
			{
				if(m_ultraToolbarManager != null)
				{
					Tool = m_ultraToolbarManager.Tools[strKey];
				}
			
			}
			catch(System.Exception )
			{
				//FireError("GetUltraTool", m_tmaxErrorBuilder.Message(ERROR_GET_ULTRA_TOOL_EX, strKey), Ex);
			}
			
			return Tool;
		
		}// GetUltraTool()
				
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The command enumeration</param>
		/// <returns>true if command should be enabled</returns>
		protected virtual bool GetCommandEnabled(TmaxScriptBoxCommands eCommand)
		{
			//	Do we have any scripts?
			if(m_ctrlScripts == null) return false;
			if(m_ctrlScripts.ValueList == null) return false;
			if(m_ctrlScripts.ValueList.ValueListItems.Count == 0) return false;
	
			//	What is the command?
			switch(eCommand)
			{
				case TmaxScriptBoxCommands.PlayAll:
				
					//	Do we have any scenes to be played?
					return ((m_IScenes != null) && (m_IScenes.Count > 0));
					
				case TmaxScriptBoxCommands.Play:
				case TmaxScriptBoxCommands.PlayThrough:
				
					return (m_oScene != null);
					
				case TmaxScriptBoxCommands.First:
				
					if(m_bPlayingScript == true) return false;
					if(m_IScenes == null) return false;
					if(m_IScenes.Count == 0) return false;
					if((m_oScene != null) && 
					   (m_IScenes.IndexOf(m_oScene) == 0)) return false;
					
					return true;
					
				case TmaxScriptBoxCommands.Last:
				
					if(m_bPlayingScript == true) return false;
					if(m_IScenes == null) return false;
					if(m_IScenes.Count == 0) return false;
					if((m_oScene != null) && 
						(m_IScenes.IndexOf(m_oScene) >= (m_IScenes.Count - 1))) return false;
					
					return true;
					
				case TmaxScriptBoxCommands.Previous:
				
					if(m_bPlayingScript == true) return false;
					if(m_IScenes == null) return false;
					if(m_IScenes.Count == 0) return false;
					if(m_oScene == null) return false;
					
					return (m_IScenes.IndexOf(m_oScene) > 0);
					
				case TmaxScriptBoxCommands.Next:
				
					if(m_bPlayingScript == true) return false;
					if(m_IScenes == null) return false;
					if(m_IScenes.Count == 0) return false;
					if(m_oScene == null) return false;
					
					return (m_IScenes.IndexOf(m_oScene) < (m_IScenes.Count - 1));
					
				default:
				
					break;
			}	
			
			return false;
		
		}// protected virtual bool GetCommandEnabled(TmaxScriptBoxCommands eCommand, int iSelections)
		
		/// <summary>Traps the event fired when the user selects a script</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		protected void OnUltraValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
		{
			int iIndex = -1;
			
			if((e.Tool.Key == "Scripts") && (m_bIgnoreEvents == false))
			{
				if((iIndex = m_ctrlScripts.SelectedIndex) >= 0)
					SetScript(m_IScripts[iIndex]);
				else
					SetScript(null);
			}
		
		}// private void OnUltraValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		protected void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		protected void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			TmaxScriptBoxCommands eCommand = TmaxScriptBoxCommands.Invalid;
			
			//	Is event processing disabled?
			if(m_bIgnoreEvents == true) return;
			
			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);
				
			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case TmaxScriptBoxCommands.PlayAll:
					
						if(((StateButtonTool)e.Tool).Checked == true)
						{
							OnCmdPlay(null, true);
							SetPlayButtonStates(eCommand);
						}
						else
							OnCmdStop();
						break;
						
					case TmaxScriptBoxCommands.PlayThrough:
					
						if(((StateButtonTool)e.Tool).Checked == true)
						{
							if(m_oScene != null)
							{
								OnCmdPlay(m_oScene, true);
								SetPlayButtonStates(eCommand);
							}
							else
							{
								SetPlayButtonStates(TmaxScriptBoxCommands.Invalid);
							}
						
						}
						else
						{
							OnCmdStop();
						}
						break;
						
					case TmaxScriptBoxCommands.Play:
					
						if(((StateButtonTool)e.Tool).Checked == true)
						{
							if(m_oScene != null)
							{
								OnCmdPlay(m_oScene, false);
								SetPlayButtonStates(eCommand);
							}
							else
							{
								SetPlayButtonStates(TmaxScriptBoxCommands.Invalid);
							}
						}
						else
						{
							OnCmdStop();
						}
						break;
						
					case TmaxScriptBoxCommands.First:
					case TmaxScriptBoxCommands.Previous:
					case TmaxScriptBoxCommands.Next:
					case TmaxScriptBoxCommands.Last:
					
						OnCmdTraverse(eCommand);
						break;
						
					default:
					
						break;
				
				}// switch(eCommand)

			}
			catch
			{
			}
		
		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxScriptBoxCtrl));
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("PlayerBar");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Play", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PlayThrough", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PlayAll", "");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("ScriptsBar");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("ScriptsLabel");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Scripts");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Play", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PlayThrough", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PlayAll", "");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Scripts");
			Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("ScriptsLabel");
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlFillPanel = new System.Windows.Forms.Panel();
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(24, 18);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ultraToolbarManager
			// 
			this.m_ultraToolbarManager.DockWithinContainer = this;
			this.m_ultraToolbarManager.ImageListSmall = this.m_ctrlImages;
			this.m_ultraToolbarManager.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ultraToolbarManager.LockToolbars = true;
			this.m_ultraToolbarManager.ShowFullMenusDelay = 500;
			this.m_ultraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 1;
			ultraToolbar1.ShowInToolbarList = false;
			ultraToolbar1.Text = "PlayerBar";
			stateButtonTool1.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool2.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  stateButtonTool1,
																							  stateButtonTool2,
																							  stateButtonTool3,
																							  labelTool1,
																							  buttonTool1,
																							  buttonTool2,
																							  buttonTool3,
																							  buttonTool4});
			ultraToolbar2.DockedColumn = 0;
			ultraToolbar2.DockedRow = 0;
			ultraToolbar2.Text = "ScriptsBar";
			comboBoxTool1.InstanceProps.Width = 151;
			ultraToolbar2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  labelTool2,
																							  comboBoxTool1});
			this.m_ultraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																												  ultraToolbar1,
																												  ultraToolbar2});
			this.m_ultraToolbarManager.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			this.m_ultraToolbarManager.ToolbarSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			this.m_ultraToolbarManager.ToolbarSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			this.m_ultraToolbarManager.ToolbarSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			this.m_ultraToolbarManager.ToolbarSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			this.m_ultraToolbarManager.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			this.m_ultraToolbarManager.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			this.m_ultraToolbarManager.ToolbarSettings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			this.m_ultraToolbarManager.ToolbarSettings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
			appearance1.Image = 0;
			buttonTool5.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool5.SharedProps.Caption = "First";
			appearance2.Image = 1;
			buttonTool6.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool6.SharedProps.Caption = "Previous";
			appearance3.Image = 2;
			buttonTool7.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool7.SharedProps.Caption = "Next";
			appearance4.Image = 3;
			buttonTool8.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool8.SharedProps.Caption = "Last";
			labelTool3.SharedProps.MinWidth = 0;
			labelTool3.SharedProps.Spring = true;
			stateButtonTool4.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool4.SharedProps.Caption = "Play";
			stateButtonTool4.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			stateButtonTool5.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool5.SharedProps.Caption = "P-Through";
			stateButtonTool5.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			stateButtonTool6.SharedProps.Caption = "P-All";
			stateButtonTool6.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			comboBoxTool2.SharedProps.ShowInCustomizer = false;
			comboBoxTool2.SharedProps.Spring = true;
			comboBoxTool2.SharedProps.ToolTipText = "Scripts";
			comboBoxTool2.ValueList = valueList1;
			labelTool4.SharedProps.Caption = "Scripts:";
			labelTool4.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool4.SharedProps.ToolTipText = "Scripts";
			this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																										   buttonTool5,
																										   buttonTool6,
																										   buttonTool7,
																										   buttonTool8,
																										   labelTool3,
																										   stateButtonTool4,
																										   stateButtonTool5,
																										   stateButtonTool6,
																										   comboBoxTool2,
																										   labelTool4});
			this.m_ultraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ultraToolbarManager.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.OnUltraValueChanged);
			// 
			// m_ctrlFillPanel
			// 
			this.m_ctrlFillPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_ctrlFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlFillPanel.Location = new System.Drawing.Point(0, 54);
			this.m_ctrlFillPanel.Name = "m_ctrlFillPanel";
			this.m_ctrlFillPanel.Size = new System.Drawing.Size(248, 0);
			this.m_ctrlFillPanel.TabIndex = 0;
			// 
			// _CTmaxScriptCtrl_Toolbars_Dock_Area_Left
			// 
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(215)), ((System.Byte)(215)), ((System.Byte)(229)));
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 54);
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left.Name = "_CTmaxScriptCtrl_Toolbars_Dock_Area_Left";
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CTmaxScriptCtrl_Toolbars_Dock_Area_Right
			// 
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(215)), ((System.Byte)(215)), ((System.Byte)(229)));
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(248, 54);
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right.Name = "_CTmaxScriptCtrl_Toolbars_Dock_Area_Right";
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CTmaxScriptCtrl_Toolbars_Dock_Area_Top
			// 
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(215)), ((System.Byte)(215)), ((System.Byte)(229)));
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top.Name = "_CTmaxScriptCtrl_Toolbars_Dock_Area_Top";
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(248, 54);
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom
			// 
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(215)), ((System.Byte)(215)), ((System.Byte)(229)));
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 52);
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom.Name = "_CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom";
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(248, 0);
			this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// CTmaxScriptBoxCtrl
			// 
			this.Controls.Add(this.m_ctrlFillPanel);
			this.Controls.Add(this._CTmaxScriptCtrl_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTmaxScriptCtrl_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTmaxScriptCtrl_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTmaxScriptCtrl_Toolbars_Dock_Area_Bottom);
			this.Name = "CTmaxScriptBoxCtrl";
			this.Size = new System.Drawing.Size(248, 52);
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called when the user wants to start playback</summary>
		/// <param name="oScene">The scene from which to start the playback</param>
		/// <param name="bPlayToEnd">True to play to the end of the script</param>
		protected void OnCmdPlay(object oScene, bool bPlayToEnd)
		{
			//	Must have an active script
			if(m_oScript == null) return;
			
			try
			{
				if(PlayEvent != null)
					PlayEvent(this, m_oScript, oScene, bPlayToEnd);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdPlay", m_tmaxErrorBuilder.Message(ERROR_PLAY_EVENT_EX), Ex);
			}
		
		}// protected void OnCmdPlay(object oScene)
		
		/// <summary>Called to get the text used to display a script in the combobox</summary>
		/// <param name="tmaxScript">Interface to the script object</param>
		/// <returns>The associated display text</returns>
		protected string GetDisplayText(ITmaxScriptBoxCtrl tmaxScript)
		{
			string strText = "";
			
			if(tmaxScript.GetName().Length > 0)
				strText = tmaxScript.GetName();
			
			if((strText.Length > 0) && (tmaxScript.GetMediaId().Length > 0))
				strText += " - ";
				
			if(tmaxScript.GetMediaId().Length > 0)
				strText += tmaxScript.GetMediaId();
				
			if(strText.Length == 0)
				strText = "Unnamed";
				
			return strText;
			
		}// protected string GetDisplayText(ITmaxScriptBoxCtrl tmaxScript)
		
		/// <summary>This method is called when the user wants to stop playback</summary>
		protected void OnCmdStop()
		{
			try
			{
				if(PlayEvent != null)
					PlayEvent(this, null, null, false);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdStop", m_tmaxErrorBuilder.Message(ERROR_PLAY_EVENT_EX), Ex);
			}
		
		}// protected void OnCmdStop()
		
		/// <summary>This method is called when the user wants to select a new scene</summary>
		/// <param name="eCommand">The command identifier triggered by the user</param>
		protected void OnCmdTraverse(TmaxScriptBoxCommands eCommand)
		{
			object oScene = null;
			
			//	What is the command?
			switch(eCommand)
			{
				case TmaxScriptBoxCommands.First:
				
					if((m_IScenes != null) && (m_IScenes.Count > 0))
						oScene = m_IScenes[0];
					break;
										
				case TmaxScriptBoxCommands.Last:
				
					if((m_IScenes != null) && (m_IScenes.Count > 0))
						oScene = m_IScenes[m_IScenes.Count - 1];
					break;
										
				case TmaxScriptBoxCommands.Previous:
				
					if((m_IScenes != null) && (m_IScenes.Count > 0))
					{
						if((m_oScene != null) && (m_IScenes.IndexOf(m_oScene) > 0))
						{
							oScene = m_IScenes[m_IScenes.IndexOf(m_oScene) - 1];
						}
						
					}
					break;
										
				case TmaxScriptBoxCommands.Next:
				
					if((m_IScenes != null) && (m_IScenes.Count > 0))
					{
						if((m_oScene != null) && (m_IScenes.IndexOf(m_oScene) < (m_IScenes.Count - 1)))
						{
							oScene = m_IScenes[m_IScenes.IndexOf(m_oScene) + 1];
						}
						
					}
					break;
										
				default:
				
					break;
			}
			
			//	Has the scene actually changed?
			if((oScene != null) && (ReferenceEquals(oScene, m_oScene) == false))
				SetScene(oScene);	
			
		}// protected void OnCmdTraverse(TmaxScriptBoxCommands eCommand)
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>The list of scripts being displayed in the combo box</summary>
		public IList Scripts
		{
			get { return m_IScripts; }
			set { SetScripts(value); }
		}
		
		/// <summary>The list of scenes associated with the current script</summary>
		public IList Scenes
		{
			get { return m_IScenes; }
			set { SetScenes(value); }
		}
		
		/// <summary>The current script selection</summary>
		public object Script
		{
			get { return m_oScript; }
			set { SetScript(value); }
		}
		
		/// <summary>The current scene selection</summary>
		public object Scene
		{
			get { return m_oScene; }
			set { SetScene(value); }
		}
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get{ return m_tmaxEventSource; }
			
		}// EventSource property

		/// <summary>True to show the scripts combobox</summary>
		public bool ShowScripts
		{
			get { return m_bShowScripts; }
			set { SetShowScripts(value); }
		}
		
		#endregion Properties
		
		#region Events
		
		/// <summary>This is the delegate used to handle events fired by this control when the user selects a new script</summary>
		/// <param name="objSender">Object firing the event</param>
		/// <param name="objScript">The new script selection</param>
		public delegate void ScriptChangedHandler(object objSender, object objScript);
		public event ScriptChangedHandler ScriptChangedEvent;
		
		/// <summary>This is the delegate used to handle events fired by this control when the user selects a new scene</summary>
		/// <param name="objSender">Object firing the event</param>
		/// <param name="objScript">The current script selection</param>
		/// <param name="objScene">The new scene selection</param>
		public delegate void SceneChangedHandler(object objSender, object objScript, object objScene);
		public event SceneChangedHandler SceneChangedEvent;
		
		/// <summary>This is the delegate used to handle events fired by this control when the user wants to start the playback</summary>
		/// <param name="objSender">Object firing the event</param>
		/// <param name="objScript">The current script selection</param>
		/// <param name="objScene">The start scene for playback</param>
		/// <param name="bPlayToEnd">True to play to the end of the script</param>
		public delegate void PlayHandler(object objSender, object objScript, object objScene, bool bPlayToEnd);
		public event PlayHandler PlayEvent;
		
		#endregion Events
	
	}// public class CTmaxScriptBoxCtrl : System.Windows.Forms.UserControl
	
}// namespace FTI.Trialmax.Controls
