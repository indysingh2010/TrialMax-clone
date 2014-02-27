using System;
using Infragistics.Win.UltraWinDock;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// 
	/// </summary>
	public class CGroupPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Error Identifiers
		
		const int ERROR_ADD_EX					= (ERROR_BASE_PANE_MAX + 1);
		const int ERROR_INITIALIZE_DOCK_EX		= (ERROR_BASE_PANE_MAX + 2);
		
		#endregion Error Identifiers
		
		/// <summary>Standard compnent container</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Infragistics inserted unpinned tab area</summary>
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CGroupPaneUnpinnedTabAreaLeft;
		
		/// <summary>Infragistics inserted unpinned tab area</summary>
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CGroupPaneUnpinnedTabAreaRight;
		
		/// <summary>Infragistics inserted unpinned tab area</summary>
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CGroupPaneUnpinnedTabAreaTop;
		
		/// <summary>Infragistics inserted unpinned tab area</summary>
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _CGroupPaneUnpinnedTabAreaBottom;

		/// <summary>Infragistics inserted autohide component</summary>
		private Infragistics.Win.UltraWinDock.AutoHideControl _CGroupPaneAutoHideControl;

		/// <summary>Infragistics library docking manager</summary>
		private Infragistics.Win.UltraWinDock.UltraDockManager m_ctrlUltraDockManager;
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			this.m_ctrlUltraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
			this._CGroupPaneUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CGroupPaneUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CGroupPaneUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CGroupPaneUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CGroupPaneAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraDockManager)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlUltraDockManager
			// 
			this.m_ctrlUltraDockManager.DefaultGroupSettings.TabSizing = Infragistics.Win.UltraWinTabs.TabSizing.Justified;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.True;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowMinimize = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowPin = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowResize = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.DoubleClickAction = Infragistics.Win.UltraWinDock.PaneDoubleClickAction.None;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.ShowCaption = Infragistics.Win.DefaultableBoolean.False;
			appearance1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.TabAppearance = appearance1;
			this.m_ctrlUltraDockManager.HostControl = this;
			this.m_ctrlUltraDockManager.ShowCloseButton = false;
			this.m_ctrlUltraDockManager.ShowDisabledButtons = false;
			this.m_ctrlUltraDockManager.ShowPinButton = false;
			// 
			// _CGroupPaneUnpinnedTabAreaLeft
			// 
			this._CGroupPaneUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this._CGroupPaneUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._CGroupPaneUnpinnedTabAreaLeft.Name = "_CGroupPaneUnpinnedTabAreaLeft";
			this._CGroupPaneUnpinnedTabAreaLeft.Owner = this.m_ctrlUltraDockManager;
			this._CGroupPaneUnpinnedTabAreaLeft.TabIndex = 0;
			// 
			// _CGroupPaneUnpinnedTabAreaRight
			// 
			this._CGroupPaneUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
			this._CGroupPaneUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._CGroupPaneUnpinnedTabAreaRight.Location = new System.Drawing.Point(150, 0);
			this._CGroupPaneUnpinnedTabAreaRight.Name = "_CGroupPaneUnpinnedTabAreaRight";
			this._CGroupPaneUnpinnedTabAreaRight.Owner = this.m_ctrlUltraDockManager;
			this._CGroupPaneUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 150);
			this._CGroupPaneUnpinnedTabAreaRight.TabIndex = 1;
			// 
			// _CGroupPaneUnpinnedTabAreaTop
			// 
			this._CGroupPaneUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
			this._CGroupPaneUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._CGroupPaneUnpinnedTabAreaTop.Name = "_CGroupPaneUnpinnedTabAreaTop";
			this._CGroupPaneUnpinnedTabAreaTop.Owner = this.m_ctrlUltraDockManager;
			this._CGroupPaneUnpinnedTabAreaTop.Size = new System.Drawing.Size(150, 0);
			this._CGroupPaneUnpinnedTabAreaTop.TabIndex = 2;
			// 
			// _CGroupPaneUnpinnedTabAreaBottom
			// 
			this._CGroupPaneUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._CGroupPaneUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._CGroupPaneUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 150);
			this._CGroupPaneUnpinnedTabAreaBottom.Name = "_CGroupPaneUnpinnedTabAreaBottom";
			this._CGroupPaneUnpinnedTabAreaBottom.Owner = this.m_ctrlUltraDockManager;
			this._CGroupPaneUnpinnedTabAreaBottom.Size = new System.Drawing.Size(150, 0);
			this._CGroupPaneUnpinnedTabAreaBottom.TabIndex = 3;
			// 
			// _CGroupPaneAutoHideControl
			// 
			this._CGroupPaneAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._CGroupPaneAutoHideControl.Name = "_CGroupPaneAutoHideControl";
			this._CGroupPaneAutoHideControl.Owner = this.m_ctrlUltraDockManager;
			this._CGroupPaneAutoHideControl.TabIndex = 4;
			// 
			// CGroupPane
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._CGroupPaneAutoHideControl,
																		  this._CGroupPaneUnpinnedTabAreaTop,
																		  this._CGroupPaneUnpinnedTabAreaBottom,
																		  this._CGroupPaneUnpinnedTabAreaLeft,
																		  this._CGroupPaneUnpinnedTabAreaRight});
			this.Name = "CGroupPane";
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraDockManager)).EndInit();
			this.ResumeLayout(false);

		}
	
		/// <summary>Default constructor</summary>
		public CGroupPane() : base()
		{
			//	Initialize the child windows
			InitializeComponent();
			
			//	Initialize the group and dock zone
			InitializeDockArea();
		}

		/// <summary>This function is called to initialize the docking area used to house the panes</summary>
		private void InitializeDockArea()
		{ 
			DockAreaPane		dcpArea;
			DockableGroupPane	dcpGroup;
			
			//	First attempt to load the information from the persistant file
			//if(LoadPaneLayout() == true) return;
			
			//	Do we have a valid manager?
			if(m_ctrlUltraDockManager == null)
			{
				//m_tmaxEventSource.FireError(this, "InitializeDockArea", "Invalid docking manager");
				return;
			}
			
			try
			{
				//	Create the docking area
				dcpArea  = new DockAreaPane(DockedLocation.DockedLeft);
			
				//	Create the docking group
				dcpGroup = new DockableGroupPane();
				dcpGroup.ChildPaneStyle = ChildPaneStyle.TabGroup;
				
				//	Add the group to the area and the area to the manager
				dcpArea.Panes.Add(dcpGroup);
				m_ctrlUltraDockManager.DockAreas.Add(dcpArea);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeDockArea", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_DOCK_EX), Ex);
			} 
		
		}// InitializeDockArea()

		/// <summary>This method is called to add a pane to the group </summary>
		/// <param name="strKey">Alpha numeric key to be assigned to the pane</param>
		/// <param name="strTitle">Title to be displayed on the pane's tab</param>
		/// <param name="Pane">CBasePane object to be added to the group</param>
		/// <returns>true if successful</returns>
		public bool Add(string strKey, string strTitle, CBasePane Pane)
		{
			DockAreaPane		dcpArea;
			DockableGroupPane	dcpGroup;
			
			//	Make sure we have a valid manager
			if(m_ctrlUltraDockManager == null) return false;
			
			try
			{
				//	Does the manager have any valid areas?
				if(m_ctrlUltraDockManager.DockAreas.Count > 0)
				{
					if((dcpArea = m_ctrlUltraDockManager.DockAreas[0]) != null)
					{
						//	Does this area have any groups?
						if(dcpArea.Panes.Count > 0)
						{
							if((dcpGroup = (DockableGroupPane)dcpArea.Panes[0]) != null)
							{
								dcpGroup.Panes.Add(new DockableControlPane(strKey, strTitle, Pane));
								return true;
							}
						
						}// if(dcpArea.Panes.Count > 0)
					
					}// if((dcpArea = m_ctrlUltraDockManager.DockAreas[0]) != null)
				
				}// if(m_ctrlUltraDockManager.DockAreas.Count > 0)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX, strKey, strTitle, Pane.Name), Ex);
			}
			
			return false;
		
		}// Add()
		
		/// <summary>
		/// This function is called to resize and reposition the group pane to use the full client area
		/// </summary>
		protected override void RecalcLayout()
		{
			//	Make sure we have a valid manager
			if(m_ctrlUltraDockManager != null)
			{
				try
				{
					//	Does the manager have any valid areas?
					if(m_ctrlUltraDockManager.DockAreas.Count > 0)
					{
						if(m_ctrlUltraDockManager.DockAreas[0] != null)
						{
							(m_ctrlUltraDockManager.DockAreas[0]).Size = new System.Drawing.Size(this.Width, this.Height);
						}
						
					}// if(m_ctrlUltraDockManager.DockAreas.Count > 0)
				
				}
				catch
				{
				}
		
			} // if(m_ctrlUltraDockManager != null)
		
		}// RecalcLayout()

		/// <summary>This function is called to retrieve the pane object with the specified key</summary>
		/// <param name="strKey">Alpha-numeric key assigned when the pane was added to the group</param>
		/// <returns>Infragistics DockablePaneBase object with the specified key if found</returns>
		public DockablePaneBase GetPane(string strKey)
		{
			DockablePaneBase Pane = null;
			
			if((m_ctrlUltraDockManager != null) && (strKey.Length > 0))
			{
				try
				{
					Pane = m_ctrlUltraDockManager.PaneFromKey(strKey);
				}
				catch
				{
				}
			}

			return Pane;
		
		}// GetPane(string strKey)
		
		/// <summary>This function is called to make all panes in the group visible</summary>
		public void ShowAll()
		{
			try
			{
				if(m_ctrlUltraDockManager != null)
					m_ctrlUltraDockManager.ShowAll();
			}
			catch
			{
			}
		
		}// ShowAll(string strKey)
		
		/// <summary>
		/// This method is called to populate the error builder's format string collection
		/// </summary>
		/// <remarks>The strings should be added to the collection in the same order in which they are enumerated</remarks>
		protected override void SetErrorStrings()
		{
			//	Do the base class first
			base.SetErrorStrings();
			
			if((m_tmaxErrorBuilder != null) && (m_tmaxErrorBuilder.FormatStrings != null))
			{
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding a new pane: Key = %1 Title = %2 Pane = %3");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the docking manager");
			}
			
		}// SetErrorStrings()

	}//	CGroupPane

}// namespace FTI.Trialmax.Panes
