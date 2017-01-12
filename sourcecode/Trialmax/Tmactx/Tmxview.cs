using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>This is the .NET container control for the Trialmax Tmview ActiveX control</summary>
	public class CTmxView : FTI.Trialmax.ActiveX.CTmxBase
	{
		#region Constants
		
		private const double DEFAULT_SCREEN_RATIO = 1.3333333; // 4/3 ratio
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Local member required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Infragistics library toolbar/menu manager left-side docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxView_Toolbars_Dock_Area_Left;
		
		/// <summary>Infragistics library toolbar/menu manager right-side docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxView_Toolbars_Dock_Area_Right;
		
		/// <summary>Infragistics library toolbar/menu manager top docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxView_Toolbars_Dock_Area_Top;
		
		/// <summary>Infragistics library toolbar/menu manager bottom docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxView_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Infragistics library toolbar/menu manager</summary>
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlUltraToolbarManager;
		
		/// <summary>Background panel to act as container for child controls</summary>
		private System.Windows.Forms.Panel m_ctrlFillPanel;
		
		/// <summary>Local member bound to UseScreenRatio property</summary>
		private bool m_bUseScreenRatio = false;
		
		/// <summary>Local member bound to ZapSourceFile property</summary>
		private string m_strZapSourceFile = "";

		/// <summary>Local member bound to TextEditorActive property</summary>
		private bool m_bTextEditorActive = false;

		/// <summary>Local flag to keep track of whether or not a treatment is loaded</summary>
		private bool m_bIsTreatment = false;

        /// <summary>Local member that stores the total angle the current loaded image is rotated by</summary>
        private short m_sTotalRotation = 0;

        private AxTM_VIEW6Lib.AxTm_view6 m_ctrlTmview;

        /// <summary>Local member that stores the total nudge the current loaded image is rotated by</summary>
        private short m_sTotalNudge = 0;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmxView() : base()
		{
			m_strDescription = "TrialMax Image Viewer (tmview)";
			m_tmaxEventSource.Name = m_strDescription;
			
			//	Initialize the child components
			InitializeComponent();
			
			InitializeUltraToolbar();
		}
		
		/// <summary>
		/// This method is called to determine if the specified file is viewable
		/// by the derived control
		/// </summary>
		/// <param name="strFilename">Name of file to be viewed if possible</param>
		/// <returns>true if viewable</returns>
		public override bool IsViewable(string strFilename)
		{
			string strExtension = System.IO.Path.GetExtension(strFilename);
			return CTmxView.CheckExtension(strExtension);
		}
		
		/// <summary>
		/// This method is called to determine if the specified file is a treatment
		/// </summary>
		/// <param name="strFilename">Name of file to be checked</param>
		/// <returns>true if it is a treatment</returns>
		public bool IsTreatment(string strFilename)
		{
			bool	bTreatment = false;
			string	strExtension = System.IO.Path.GetExtension(strFilename);
			
			if((strExtension != null) && (strExtension.Length > 0))
			{
				if(String.Compare(strExtension, ".zap", true) == 0)
					bTreatment = true;
			}
			
			return bTreatment;
			
		}// public bool IsTreatment(string strFilename)
		
		/// <summary>This method handles all KeyDown notifications from the application</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>true if processed</returns>
		public override bool OnKeyDown(Keys eKey, Keys eModifiers)
		{
			TmaxMediaBarCommands eCommand = TmaxMediaBarCommands.Invalid;

			//	Don't process hotkeys unless the toolbar is active
			if(m_bShowToolbar == false) return false;

			//	Don't process hotkeys if the user is editing a text annotation
			if(m_bTextEditorActive == true) return false;

			//	Don't allow the delete key to get translated
			//
			//	We want the default behavior for the page navigator
			if(IsGoToActive() == true) return false;
			
			//	Translate the keystroke to a media bar command
			if((eCommand = CTmaxMediaBar.GetCommand(eKey, eModifiers)) != TmaxMediaBarCommands.Invalid)
			{
				ProcessCommand(eCommand);
				return true;
			}
			else
			{
				//	These hotkeys do not translate to media bar commands
				switch(eKey)
				{
					case Keys.PageDown:
					
						try { m_ctrlTmview.Pan(3, -1); }
						catch { }
						
						return true;
						
					case Keys.PageUp:
					
						try { m_ctrlTmview.Pan(2, -1); }
						catch { }
						
						return true;
						
					default:
						
						m_tmaxEventSource.FireDiagnostic(this, "OnKeyDown", eKey.ToString() + " -> FALSE");			
						return false;
						
				}// switch(eKey)
			
			}
			
		}// public override bool OnKeyDown(Keys eKey, Keys eModifiers)
		
		/// <summary>This function is called to determine if files with the specified extension can be rendered by this control</summary>
		/// <param name="strExtension">File extension to be checked</param>
		/// <returns>true if viewable</returns>
		public static bool CheckExtension(string strExtension)
		{
			if((strExtension != null) && (strExtension.Length > 0))
			{
				//	Strip the leading period if found
				if(strExtension.StartsWith("."))
					strExtension = strExtension.Remove(0,1);
					
				switch(strExtension.ToLower())
				{
					case "tif":
					case "tiff":
					case "bmp":
					case "zap":
					case "png":
					case "pcx":
					case "jpg":
					case "jpeg":
					case "gif":
					
						return true;
					
					default:
					
						break;
				}
				
			}
			
			return false;

		}
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="lStart">Not used by this viewer</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public override bool View(string strFilename, long lStart)
		{
			//	Are we actually unloading?
			if((strFilename == null) || (strFilename.Length == 0))
			{
				Unload();
				return true;
			}
			
			//	Do we have a valid control?
			if((m_ctrlTmview == null) || (m_bInitialized == false)) return false;
			
			//	Make sure the rotation property is reset
			m_ctrlTmview.Rotation = 0;
            m_sTotalRotation = 0;

			if(IsTreatment(strFilename) == true)
			{
				m_bIsTreatment = true;
				m_sAxError = m_ctrlTmview.LoadZap(strFilename, 1, 1, 0, -1, m_strZapSourceFile);
			}
			else
			{
				m_bIsTreatment = false;
				m_sAxError = m_ctrlTmview.LoadFile(strFilename, -1);
			}
			
			if(m_sAxError == 0)
			{ 
				SetUltraToolEnabled(TmaxMediaBarCommands.UpdateZap, m_bIsTreatment);
                SetUltraToolEnabled(TmaxMediaBarCommands.NudgeLeft, !m_bIsTreatment);
                SetUltraToolEnabled(TmaxMediaBarCommands.NudgeRight, !m_bIsTreatment);
                SetUltraToolEnabled(TmaxMediaBarCommands.SaveNudge, !m_bIsTreatment);
				
				m_strFilename = strFilename;
				return true;
			}
			else
			{
				m_bIsTreatment = false;
				SetUltraToolEnabled(TmaxMediaBarCommands.UpdateZap, false);
                SetUltraToolEnabled(TmaxMediaBarCommands.NudgeLeft, !m_bIsTreatment);
                SetUltraToolEnabled(TmaxMediaBarCommands.NudgeRight, !m_bIsTreatment);
                SetUltraToolEnabled(TmaxMediaBarCommands.SaveNudge, !m_bIsTreatment);
				
				return false;
			}
				
		}//	Load(string strFilename)
		
		/// <summary>
		/// This method is called to perform the ActiveX initialization of the control
		/// </summary>
		/// <returns>true if successful</returns>
		public override bool AxInitialize()
		{
			//	Do the base class initialization first
			if(base.AxInitialize() == false) return false;
			
			//	Initialize the control properties
			AxSetProperties();
			
			//	Update the toolbar
			SetShadedCallouts();
			SetColorImage();
			SetToolButton(m_ctrlTmview.AnnTool);
			
			return true;
		}
				
		/// <summary>This method is called to save the ActiveX control properties to the IniFilename / IniSection</summary>
		/// <returns>true if successful</returns>
		public override bool AxSaveProperties()
		{
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				if((m_strIniFilename.Length > 0) && (m_strIniSection.Length > 0))
				{
					try 
					{ 
						if(m_ctrlTmview.SaveProperties(m_strIniFilename, m_strIniSection) == 0)
							return true;
					}
					catch 
					{ 
					}
				}
				
			}
			
			return false;
		
		}// public override bool AxSaveProperties()
		
		/// <summary>This method is called to set the ActiveX control properties stored in the IniFilename / IniSection</summary>
		/// <returns>true if successful</returns>
		public override bool AxSetProperties()
		{
			bool bSuccessful = false;
			
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				if((m_strIniFilename.Length > 0) && (m_strIniSection.Length > 0))
				{
					try 
					{ 
						if(m_ctrlTmview.SetProperties(m_strIniFilename, m_strIniSection) == 0)
						{
							bSuccessful = true;
						}
					}
					catch 
					{ 
					}
				}
				
			}
			
			//	Make sure the toolbar is in sync if visible
			if(m_bShowToolbar == true)
			{
				//	Is the user drawing?
				if(m_ctrlTmview.Action == (short)TmxViewActions.Draw)
				{
					//	The tool may have changed
					SetToolButton(m_ctrlTmview.AnnTool);
				}
				
				//	Make sure the correct color is highlighted
				SetColorImage();
				
				//	The user may have changed the shaded callouts option
				SetShadedCallouts();				
			}
			
			return bSuccessful;
		
		}// public virtual bool AxSetProperties()
		
		/// <summary>This method is called to rotate the active image</summary>
		/// <param name="bClockwise">true for clockwise, false for counter-clockwise</param>
		/// <returns>true if successful</returns>
		public bool Rotate(bool bClockwise)
		{
			bool bSuccessful = false;
			
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				if(m_ctrlTmview.IsLoaded(-1) == -1)
				{
					if(bClockwise == true)
						m_ctrlTmview.RotateCw(1, -1);
					else
						m_ctrlTmview.RotateCcw(1, -1);
					
					bSuccessful = true;
				}
				
			}
			
			return bSuccessful;
		
		}// public bool Rotate(bool bClockwise)
		
		/// <summary>This method is called to deskew the active image</summary>
		/// <returns>true if successful</returns>
		public bool Deskew()
		{
			bool bSuccessful = false;
			
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				if(m_ctrlTmview.IsLoaded(-1) == -1)
					bSuccessful = (m_ctrlTmview.Deskew(-1) == 0);
			}
			
			return bSuccessful;
		
		}// public bool Deskew()
		
		/// <summary>This method is called to despeckle the active image</summary>
		/// <returns>true if successful</returns>
		public bool Despeckle()
		{
			bool bSuccessful = false;
			
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				if(m_ctrlTmview.IsLoaded(-1) == -1)
					bSuccessful = (m_ctrlTmview.Despeckle(-1) == 0);
			}
			
			return bSuccessful;
		
		}// public bool Despeckle()
		
		/// <summary>This method is called to remove hole punches in the active image</summary>
		/// <param name="iMinHoles">The minimum number of holes that should appear</param>
		/// <param name="iMaxHoles">The maximum number of holes that should appear</param>
		/// <param name="iLocations">The locations (edges) to check for the punches</param>
		/// <returns>true if successful</returns>
		public bool RemoveHolePunches(int iMinHoles, int iMaxHoles, int iLocations)
		{
			bool bSuccessful = false;
			
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				if(m_ctrlTmview.IsLoaded(-1) == -1)
				{
					//	Assume success
					bSuccessful = true;
					
					if((iLocations & (int)TmaxHolePunchLocations.Left) != 0)
					{
						if(m_ctrlTmview.HolePunchRemove2(-1, iMinHoles, iMaxHoles, 1) != 0)
							bSuccessful = false;
					}
					
					if((iLocations & (int)TmaxHolePunchLocations.Right) != 0)
					{
						if(m_ctrlTmview.HolePunchRemove2(-1, iMinHoles, iMaxHoles, 2) != 0)
							bSuccessful = false;
					}
					
					if((iLocations & (int)TmaxHolePunchLocations.Top) != 0)
					{
						if(m_ctrlTmview.HolePunchRemove2(-1, iMinHoles, iMaxHoles, 3) != 0)
							bSuccessful = false;
					}
					
					if((iLocations & (int)TmaxHolePunchLocations.Bottom) != 0)
					{
						if(m_ctrlTmview.HolePunchRemove2(-1, iMinHoles, iMaxHoles, 4) != 0)
							bSuccessful = false;
					}
					
				}// if(m_ctrlTmview.IsLoaded(-1) == -1)
			
			}
			
			return bSuccessful;
		
		}// public bool RemoveHolePunches(int iMinHoles, int iMaxHoles, TmaxHolePunchLocations iLocations)
		
		/// <summary>This method is called to save the active image to the specified file</summary>
		/// <returns>true if successful</returns>
		public bool Save(string strFilename)
		{
			bool	bSuccessful = false;
			string	strSaveAs = "";
			string	strTemp = "";

			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				if(m_ctrlTmview.IsLoaded(-1) == -1)
				{
					//	Are we supposed to use the existing filename?
					if((strFilename == null) || (strFilename.Length == 0))
						strSaveAs = this.Filename;
					else
						strSaveAs = strFilename;
					
					//	Does this file already exist?
					if(System.IO.File.Exists(strSaveAs) == true)
					{
						try 
						{ 
							strTemp = System.IO.Path.ChangeExtension(strSaveAs, ".bak");
							System.IO.File.Move(strSaveAs, strTemp); 
						}
						catch
						{ 
							strTemp = "";
						}
					
					}
					
					//	Save the file
					if((m_sAxError = m_ctrlTmview.Save(strSaveAs, -1)) == 0)
					{
						bSuccessful = true;
                        m_sTotalNudge = 0;
                        m_sTotalRotation = 0;
						//	Delete the backup
						if((strTemp.Length > 0) && (System.IO.File.Exists(strTemp) == true))
						{
							try { System.IO.File.Delete(strTemp); }
							catch {}
						}
					
					}
					else
					{
						//	Restore the original
						if((strTemp.Length > 0) && (System.IO.File.Exists(strTemp) == true))
						{
							try { System.IO.File.Move(strTemp, strSaveAs); }
							catch {}
						}
						
					}
					
					
				}// if(m_ctrlTmview.IsLoaded(-1) == -1)
			
			}
			
			return bSuccessful;
		
		}// public bool Save(string strFilename)
		
		/// <summary>This method is called to save the pages in the specified file to the folder</summary>
		/// <param name="strFilename">The fully qualified path to the source file</param>
		/// <param name="strFolder">The path to the output folder</param>
		/// <param name="strPrefix">The prefix applied to the output filename</param>
		/// <returns>The total number of saved pages, Zero if error</returns>
		public int SavePages(string strFilename, string strFolder, string strPrefix)
		{
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				return m_ctrlTmview.SavePages(strFilename, strFolder, strPrefix);
			}
			else
			{
				return 0;
			}
		
		}// public int SavePages(string strFilename, string strFolder, string strPrefix)
		
		/// <summary>This function is called to process the specified request</summary>
		/// <param name="tmxRequest">The request parameters</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public override bool ProcessRequest(CTmxRequest tmxRequest)
		{
			if(m_bInitialized == false) return false;
			
			//	What action is being requested?
			switch(tmxRequest.Action)
			{
				case TmxActions.ShowCallouts:
				
					m_ctrlTmview.ShowCallouts(1, -1);
					break;
					
				case TmxActions.HideCallouts:
				
					m_ctrlTmview.ShowCallouts(0, -1);
					break;
					
			}
			
			return true;
		}
		
		/// <summary>This method is called to process a media bar command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <returns>true if processed</returns>
		public override bool ProcessCommand(TmaxMediaBarCommands eCommand)
		{
			//	Which command?
			switch(eCommand)
			{
				case TmaxMediaBarCommands.RotateCW:
				
					OnRotateImage(90);
					break;
					
				case TmaxMediaBarCommands.RotateCCW:
				
					OnRotateImage(-90);
					break;
					
				case TmaxMediaBarCommands.Zoom:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Zoom;
					SetToolButton(eCommand, false);
					break;
					
				case TmaxMediaBarCommands.Callout:

					DialogResult result = MessageBox.Show("Do you want to fixed aspect ratio?", "Aspect Ratio", MessageBoxButtons.YesNoCancel);
                    if(result == DialogResult.Yes)
                    {
                        m_ctrlTmview.Action = (short)TmxViewActions.Callout;
                        m_ctrlTmview.KeepAspect = 1;
                        SetToolButton(eCommand, false);
                        break;
                    }
                    else if (result == DialogResult.No)
                    {
                        m_ctrlTmview.Action = (short)TmxViewActions.Callout;
                        m_ctrlTmview.KeepAspect = 0;
                        SetToolButton(eCommand, false);
                        break;
                    }
                    else
                    {
                        break;
                    }
					
				case TmaxMediaBarCommands.Highlight:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Highlight;
					SetToolButton(eCommand, false);
					break;
					
				case TmaxMediaBarCommands.Redact:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Redact;
					SetToolButton(eCommand, false);
					break;
					
				case TmaxMediaBarCommands.Pan:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Pan;
					SetToolButton(eCommand, false);
					break;
					
				case TmaxMediaBarCommands.Select:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Select;
					SetToolButton(eCommand, false);
					break;
					
				case TmaxMediaBarCommands.Draw:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					SetToolButton(m_ctrlTmview.AnnTool);
					break;
					
				case TmaxMediaBarCommands.Freehand:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Freehand;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.Ellipse:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Ellipse;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.Line:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Line;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.Arrow:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Arrow;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.Polygon:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Polyline;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.Rectangle:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Rectangle;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.Text:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Text;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.FilledEllipse:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.FilledEllipse;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.FilledPolygon:
				
					m_ctrlTmview.Action = (short)TmxViewActions.Draw;
					m_ctrlTmview.AnnTool = (short)TmxViewTools.Polygon;
					SetToolButton(eCommand, AxAutoSave);
					break;
					
				case TmaxMediaBarCommands.Normal:
				
					m_ctrlTmview.ResetZoom(-1);
					break;
					
				case TmaxMediaBarCommands.ZoomWidth:
				
					m_ctrlTmview.ZoomFullWidth(-1);
					break;
					
				case TmaxMediaBarCommands.Erase:
				
					m_ctrlTmview.Erase(-1);
					break;
					
				case TmaxMediaBarCommands.EraseLast:
				
					OnDeleteAnnotations();
					break;
					
				case TmaxMediaBarCommands.SaveZap:
				
					OnSaveZap();
					break;

				case TmaxMediaBarCommands.UpdateZap:
				
					OnUpdateZap();
					break;

				case TmaxMediaBarCommands.Next:
				case TmaxMediaBarCommands.Previous:
				
					OnNavigatorCommand(eCommand);
					break;

				//	We assigned PureBlack as the command for invoking
				//	the color picker
				case TmaxMediaBarCommands.PureBlack:
				
					OnSetColor();
					break;

				//	Toggle the state of the shaded callouts option
				case TmaxMediaBarCommands.ShadedCallouts:
				
					m_ctrlTmview.ShadeOnCallout = (sbyte)(m_ctrlTmview.ShadeOnCallout == 1 ? 0 : 1);
					SetShadedCallouts();
					if(AxAutoSave == true)
						AxSaveProperties();
					break;

                case TmaxMediaBarCommands.BlankPresentation:
                    break;

                case TmaxMediaBarCommands.NudgeRight:

                    OnNudge(true);
                    break;

                case TmaxMediaBarCommands.NudgeLeft:

                    OnNudge(false);
                    break;

                case TmaxMediaBarCommands.SaveNudge:

                    SaveNudgeImage(m_strFilename);
                    break;

				default:
				
					return false;
					
			}// switch(eCommand)
		
			return true;
		
		}

        private void SaveNudgeImage(string strFilename)
        {
            if (m_bIsTreatment)
                return;
            bool bSuccessful = false;
            string strSaveAs = "";
            string strTemp = "";

            if ((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
            {
                if (m_ctrlTmview.IsLoaded(-1) == -1)
                {
                    // Are we supposed to use the existing filename?
                    if ((strFilename == null) || (strFilename.Length == 0))
                        strSaveAs = this.Filename;
                    else
                        strSaveAs = strFilename;

                    // Does this file already exist?
                    if (System.IO.File.Exists(strSaveAs) == true)
                    {
                        try
                        {
                            strTemp = System.IO.Path.ChangeExtension(strSaveAs, ".bak");
                            System.IO.File.Move(strSaveAs, strTemp);
                        }
                        catch
                        {
                            strTemp = "";
                        }

                    }
                    // Save the file
                    if ((m_sAxError = m_ctrlTmview.Save(strSaveAs, -1)) == 0)
                    {
                        bSuccessful = true;
                        m_sTotalRotation = 0;
                        m_sTotalNudge = 0;
                        // Delete the backup
                        if ((strTemp.Length > 0) && (System.IO.File.Exists(strTemp) == true))
                        {
                            try { System.IO.File.Delete(strTemp); }
                            catch { }
                        }
                    }
                    else
                    {
                        // Restore the original
                        if ((strTemp.Length > 0) && (System.IO.File.Exists(strTemp) == true))
                        {
                            try { System.IO.File.Move(strTemp, strSaveAs); }
                            catch { }
                        }
                    }
                }// if(m_ctrlTmview.IsLoaded(-1) == -1)
            }
        }

        private void OnNudge(bool direction)
        {
            if (m_bIsTreatment || Math.Abs(m_sTotalNudge + (direction == true ? 1 : -1)) > 20)
                return;
            //	Give the owner a chance to cancel the operation
            if (FireQueryContinue(TmxActions.Nudge, m_strFilename, CalloutCount) == true)
            {
                m_sTotalRotation += (short)(direction == true ? 1 : -1);
                m_sTotalNudge += (short)(direction == true ? 1 : -1);
                string tempFileName = m_strFilename;
                m_bIsTreatment = false;

                m_ctrlTmview.Rotation = m_sTotalRotation;
                m_sAxError = m_ctrlTmview.LoadFile(tempFileName, -1);

            }
        }// public override bool ProcessCommand(TmaxMediaBarCommands eCommand)
		
		/// <summary>This method is called to unload the viewer</summary>
		public override void Unload()
		{
			//	Do we have a valid control?
			if((m_ctrlTmview != null) && (m_bInitialized == true))
			{
				m_ctrlTmview.LoadFile("", -1);
			}
			
			m_strFilename = "";
			m_bIsTreatment = false;
			m_bTextEditorActive = false;
			
			if(m_bInitialized == true)
				FireStateChange(TmxStates.Unloaded);
			
		}//	Unload()
		
		/// <summary>This method is called to rescale the callouts to match the window size</summary>
		/// <returns>True if successful</returns>
		public bool RescaleCallouts()
		{
			if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
			{
				m_sAxError = m_ctrlTmview.RescaleZapCallouts();
				
				return (m_sAxError == 0);
			}
			else
			{
				return false;
			}
			
		}
		
		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("GoTo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("Total");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Zoom", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomWidth");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Normal");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveZap");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UpdateZap");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Callout", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Highlight", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Redact", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Pan", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Freehand", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Arrow", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool8 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Line", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Rectangle", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Ellipse", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Polygon", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("FilledEllipse", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("FilledPolygon", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool14 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Text", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool15 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Select", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Erase");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCw");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCcw");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool16 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShadedCallouts", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PureBlack");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonToolShowPresentation1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BlankPresentation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NudgeLeft");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NudgeRight");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveNudge");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Erase");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool17 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Callout", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool18 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Highlight", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool19 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Redact", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool20 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Freehand", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool21 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Arrow", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool22 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Line", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool23 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Rectangle", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool24 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Ellipse", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool25 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Polygon", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool26 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("FilledEllipse", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool27 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("FilledPolygon", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool28 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Text", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool29 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Select", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCw");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCcw");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool30 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Zoom", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomWidth");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Normal");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveZap");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PureBlack");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UpdateZap");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool31 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShadedCallouts", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool32 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Pan", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("GoTo");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("Total");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonToolShowPresentation2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BlankPresentation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NudgeLeft");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NudgeRight");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveNudge");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTmxView));
            this.m_ctrlUltraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.m_ctrlFillPanel = new System.Windows.Forms.Panel();
            this._CTmxView_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CTmxView_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CTmxView_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CTmxView_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.m_ctrlTmview = new AxTM_VIEW6Lib.AxTm_view6();
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).BeginInit();
            this.m_ctrlFillPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmview)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ctrlUltraToolbarManager
            // 
            this.m_ctrlUltraToolbarManager.DesignerFlags = 1;
            this.m_ctrlUltraToolbarManager.DockWithinContainer = this;
            this.m_ctrlUltraToolbarManager.ImageSizeSmall = new System.Drawing.Size(24, 18);
            this.m_ctrlUltraToolbarManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlUltraToolbarManager.LockToolbars = true;
            this.m_ctrlUltraToolbarManager.ShowFullMenusDelay = 500;
            this.m_ctrlUltraToolbarManager.ShowQuickCustomizeButton = false;
            this.m_ctrlUltraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            textBoxTool1.InstanceProps.Width = 56;
            labelTool1.InstanceProps.Width = 13;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            textBoxTool1,
            buttonTool2,
            labelTool1,
            stateButtonTool1,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            stateButtonTool2,
            stateButtonTool3,
            stateButtonTool4,
            stateButtonTool5,
            stateButtonTool6,
            stateButtonTool7,
            stateButtonTool8,
            stateButtonTool9,
            stateButtonTool10,
            stateButtonTool11,
            stateButtonTool12,
            stateButtonTool13,
            stateButtonTool14,
            stateButtonTool15,
            buttonTool7,
            buttonTool8,
            buttonTool9,
            stateButtonTool16,
            buttonTool10,
            buttonToolShowPresentation1,
            buttonTool21,
            buttonTool22,
            buttonTool23});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.ToolSpacing = -4;
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
            buttonTool11.SharedProps.Caption = "Erase";
            buttonTool11.SharedProps.ToolTipText = "Erase All - (Shortcut = E)";
            stateButtonTool17.SharedProps.Caption = "Callout";
            stateButtonTool17.SharedProps.ToolTipText = "Callout (Shortcut = C)";
            stateButtonTool18.SharedProps.Caption = "Highlight";
            stateButtonTool18.SharedProps.ToolTipText = "Highlight (Shortcut = H)";
            stateButtonTool19.SharedProps.Caption = "Redact";
            stateButtonTool19.SharedProps.ToolTipText = "Redact (Shortcut = R)";
            stateButtonTool20.SharedProps.Caption = "Freehand";
            stateButtonTool20.SharedProps.ToolTipText = "Freehand (Shortcut = Shift+F)";
            stateButtonTool21.SharedProps.Caption = "Arrow";
            stateButtonTool21.SharedProps.ToolTipText = "Arrow (Shortcut = Shift+A)";
            stateButtonTool22.SharedProps.Caption = "Line";
            stateButtonTool22.SharedProps.ToolTipText = "Line (Shortcut = Shift+L)";
            stateButtonTool23.SharedProps.Caption = "Rectangle";
            stateButtonTool23.SharedProps.ToolTipText = "Rectangle (Shortcut = Shift+S)";
            stateButtonTool24.SharedProps.Caption = "Ellipse";
            stateButtonTool24.SharedProps.ToolTipText = "Ellipse (Shortcut = Shift+C)";
            stateButtonTool25.SharedProps.Caption = "Polygon";
            stateButtonTool25.SharedProps.ToolTipText = "Polygon (Shortcut = Shift+H)";
            stateButtonTool26.SharedProps.Caption = "FilledEllipse";
            stateButtonTool26.SharedProps.ToolTipText = "Filled Ellipse (Shortcut = Ctrl+Shift+C)";
            stateButtonTool27.SharedProps.Caption = "FilledPolygon";
            stateButtonTool27.SharedProps.ToolTipText = "Filled Polygon (Shortcut = Ctrl+Shift+H)";
            stateButtonTool28.SharedProps.Caption = "Text";
            stateButtonTool28.SharedProps.ToolTipText = "Text (Shortcut = Shift+T)";
            stateButtonTool29.SharedProps.Caption = "Select";
            stateButtonTool29.SharedProps.ToolTipText = "Select (Shortcut = A)";
            buttonTool12.SharedProps.Caption = "Rotate Clockwise";
            buttonTool12.SharedProps.ToolTipText = "Rotate Clockwise (Shortcut = ])";
            buttonTool13.SharedProps.Caption = "Rotate Counter Clockwise";
            buttonTool13.SharedProps.ToolTipText = "Rotate CounterClockwise (Shortcut = [)";
            stateButtonTool30.SharedProps.Caption = "Zoom In";
            stateButtonTool30.SharedProps.ToolTipText = "Zoom (Shortcut = Z)";
            buttonTool14.SharedProps.Caption = "Full Width";
            buttonTool14.SharedProps.ToolTipText = "Full Width (Shortcut = W)";
            buttonTool15.SharedProps.Caption = "Fit to Window";
            buttonTool15.SharedProps.ToolTipText = "1:1 (Shortcut = F)";
            buttonTool16.SharedProps.Caption = "Save Treatment";
            buttonTool16.SharedProps.ToolTipText = "Save Treatment (Shortcut = M)";
            buttonTool17.SharedProps.Caption = "Set Color";
            buttonTool18.SharedProps.Caption = "Update Treatment";
            buttonTool18.SharedProps.ToolTipText = "Update Treatment (Shortcut = U)";
            stateButtonTool31.SharedProps.Caption = "Shaded Callouts";
            stateButtonTool32.SharedProps.Caption = "Pan";
            stateButtonTool32.SharedProps.ToolTipText = "Pan - (Shortcut = S)";
            buttonTool19.SharedProps.Caption = "Next";
            buttonTool19.SharedProps.ToolTipText = "Next Page - (Shortcut = .)";
            buttonTool20.SharedProps.Caption = "Previous";
            buttonTool20.SharedProps.ToolTipText = "Prev Page - (Shortcut = ,)";
            appearance1.TextHAlignAsString = "Right";
            textBoxTool2.EditAppearance = appearance1;
            textBoxTool2.SharedProps.ToolTipText = "Go To Page";
            appearance2.TextHAlignAsString = "Left";
            labelTool2.SharedProps.AppearancesSmall.Appearance = appearance2;
            labelTool2.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonToolShowPresentation2.SharedProps.Caption = "TrailMax Presentation";
            buttonToolShowPresentation2.SharedProps.ToolTipText = "TrailMax Presentation (Shortcut = Ctrl+F5)";
            buttonTool24.SharedProps.Caption = "Nudge Left";
            buttonTool24.SharedProps.ToolTipText = "Nudge Left - (Shortcut = Shift+[)";
            buttonTool25.SharedProps.Caption = "Nudge Right";
            buttonTool25.SharedProps.ToolTipText = "Nudge Right - (Shortcut = Shift+])";
            buttonTool26.SharedProps.Caption = "Save Nudge";
            this.m_ctrlUltraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool11,
            stateButtonTool17,
            stateButtonTool18,
            stateButtonTool19,
            stateButtonTool20,
            stateButtonTool21,
            stateButtonTool22,
            stateButtonTool23,
            stateButtonTool24,
            stateButtonTool25,
            stateButtonTool26,
            stateButtonTool27,
            stateButtonTool28,
            stateButtonTool29,
            buttonTool12,
            buttonTool13,
            stateButtonTool30,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            stateButtonTool31,
            stateButtonTool32,
            buttonTool19,
            buttonTool20,
            textBoxTool2,
            labelTool2,
            buttonToolShowPresentation2,
            buttonTool24,
            buttonTool25,
            buttonTool26});
            this.m_ctrlUltraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
            this.m_ctrlUltraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
            this.m_ctrlUltraToolbarManager.ToolKeyDown += new Infragistics.Win.UltraWinToolbars.ToolKeyEventHandler(this.OnUltraToolKeyDown);
            // 
            // m_ctrlFillPanel
            // 
            this.m_ctrlFillPanel.Controls.Add(this.m_ctrlTmview);
            this.m_ctrlFillPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.m_ctrlFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ctrlFillPanel.Location = new System.Drawing.Point(0, 30);
            this.m_ctrlFillPanel.Name = "m_ctrlFillPanel";
            this.m_ctrlFillPanel.Size = new System.Drawing.Size(540, 120);
            this.m_ctrlFillPanel.TabIndex = 0;
            this.m_ctrlFillPanel.Resize += new System.EventHandler(this.OnPanelResize);
            // 
            // _CTmxView_Toolbars_Dock_Area_Left
            // 
            this._CTmxView_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTmxView_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._CTmxView_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._CTmxView_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTmxView_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 30);
            this._CTmxView_Toolbars_Dock_Area_Left.Name = "_CTmxView_Toolbars_Dock_Area_Left";
            this._CTmxView_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 120);
            this._CTmxView_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlUltraToolbarManager;
            // 
            // _CTmxView_Toolbars_Dock_Area_Right
            // 
            this._CTmxView_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTmxView_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._CTmxView_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._CTmxView_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTmxView_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(540, 30);
            this._CTmxView_Toolbars_Dock_Area_Right.Name = "_CTmxView_Toolbars_Dock_Area_Right";
            this._CTmxView_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 120);
            this._CTmxView_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlUltraToolbarManager;
            // 
            // _CTmxView_Toolbars_Dock_Area_Top
            // 
            this._CTmxView_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTmxView_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._CTmxView_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._CTmxView_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTmxView_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CTmxView_Toolbars_Dock_Area_Top.Name = "_CTmxView_Toolbars_Dock_Area_Top";
            this._CTmxView_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(540, 30);
            this._CTmxView_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlUltraToolbarManager;
            // 
            // _CTmxView_Toolbars_Dock_Area_Bottom
            // 
            this._CTmxView_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTmxView_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._CTmxView_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._CTmxView_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTmxView_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 150);
            this._CTmxView_Toolbars_Dock_Area_Bottom.Name = "_CTmxView_Toolbars_Dock_Area_Bottom";
            this._CTmxView_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(540, 0);
            this._CTmxView_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlUltraToolbarManager;
            // 
            // m_ctrlTmview
            // 
            this.m_ctrlTmview.Enabled = true;
            this.m_ctrlTmview.Location = new System.Drawing.Point(115, 36);
            this.m_ctrlTmview.Name = "m_ctrlTmview";
            this.m_ctrlTmview.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_ctrlTmview.OcxState")));
            this.m_ctrlTmview.Size = new System.Drawing.Size(262, 43);
            this.m_ctrlTmview.TabIndex = 0;
            // 
            // CTmxView
            // 
            this.Controls.Add(this.m_ctrlFillPanel);
            this.Controls.Add(this._CTmxView_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._CTmxView_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._CTmxView_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._CTmxView_Toolbars_Dock_Area_Bottom);
            this.Name = "CTmxView";
            this.Size = new System.Drawing.Size(540, 150);
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).EndInit();
            this.m_ctrlFillPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmview)).EndInit();
            this.ResumeLayout(false);

		}// protected override void InitializeComponent()

		/// <summary>Clean up any resources being used</summary>
		/// <param name="disposing">true if disposing of the object</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>This method is called to get the Infragistics toolbar manager</summary>
		/// <returns>The viewer's toolbar manager</returns>
		/// <remarks>This method should be overridden by derived class that want to use the toolbar helper methods</remarks>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraManager()
		{
			return m_ctrlUltraToolbarManager;
		}

		/// <summary>This method is called to get the object's component container</summary>
		/// <returns>The viewer's component container</returns>
		protected override System.ComponentModel.IContainer GetComponentContainer()
		{
			return components;
		}

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		protected virtual void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		/// <summary>This function handles events fired by the toolbar manager when the user releases a key in one of the tools</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		protected virtual void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)
		{
			if((e.Tool.Key == TmaxMediaBarCommands.GoTo.ToString()) && (e.KeyCode == Keys.Enter))
			{
				//	Mark the event as handled
				e.Handled = true;

				OnNavigatorCommand(TmaxMediaBarCommands.GoTo);
			}

		}// private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)

		/// <summary>This method traps events fired when the user clicks on a toolbar button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">Infragistics event object</param>
		protected override void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			//	Call the base class handler
			//
			//	NOTE:	We have to put this wrapper in the derived class otherwise the
			//			form designer in VS 2005 will raise an exception when we attempt
			//			to edit the form
			base.OnUltraToolClick(sender, e);

		}// protected override void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>Called by the base class when any navigator changes</summary>
		protected override void OnNavigatorPropChanged()
		{
			TextBoxTool goTo = null;
			LabelTool	total = null;
			
			SetUltraToolEnabled(TmaxMediaBarCommands.Next, ((m_iNavigatorPosition >= 1) && (m_iNavigatorPosition < m_iNavigatorTotal)));
			SetUltraToolEnabled(TmaxMediaBarCommands.Previous, ((m_iNavigatorPosition > 1) && (m_iNavigatorPosition <= m_iNavigatorTotal)));

			//	Make sure we have the correct text in the GO TO box
			if((goTo = (TextBoxTool)GetUltraTool(TmaxMediaBarCommands.GoTo.ToString())) != null)
			{
				goTo.SharedProps.Enabled = (m_iNavigatorTotal > 0);
	
				if(m_iNavigatorPosition >= 1)
					goTo.Text = m_iNavigatorPosition.ToString();
				else
					goTo.Text = "";
			
			}
			
			//	Make sure we have the correct text in the GO TO box
			if((total = (LabelTool)GetUltraTool("Total")) != null)
			{
				if(m_iNavigatorTotal > 0)
					total.SharedProps.Caption = ("of " + m_iNavigatorTotal.ToString());
				else
					total.SharedProps.Caption = "";
			}
			
		}// protected override void OnNavigatorPropChanged()
		
		/// <summary>This function is called to set the states of the toolbar buttons</summary>
		protected override void SetUltraToolStates()
		{
			//	Do the base class processing
			base.SetUltraToolStates();
			
			if((m_bShowToolbar == true) && (m_bEnableToolbar == true))
			{
				OnNavigatorPositionChanged();
			}
		
		}//	protected override void SetUltraToolStates()
		
		/// <summary>This method is called when the window gets created</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(System.EventArgs e)
		{
			if(m_bUseScreenRatio == true)
				m_ctrlTmview.BackColor = System.Drawing.Color.Black;
			else
				m_ctrlTmview.BackColor = System.Drawing.SystemColors.Control;
				
			//	Initialize the navigator controls
			OnNavigatorPositionChanged();
			
			//	Make sure the TMView control is in position
			RecalcLayout();
			
			base.OnLoad(e);
		}

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method traps MouseDown events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseDown(object sender, AxTM_VIEW6Lib._DTm_view6Events_MouseDownEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseDown(this, TranslateMouseButton(e.button), e.x, e.y);	
		}

		/// <summary>This method traps MouseDblClick events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseDblClick(object sender, AxTM_VIEW6Lib._DTm_view6Events_MouseDblClickEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseDblClick(this, TranslateMouseButton(e.button), 0, 0, 2);	
		}

		/// <summary>This method traps MouseUp events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseUp(object sender, AxTM_VIEW6Lib._DTm_view6Events_MouseUpEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseUp(this, TranslateMouseButton(e.button), e.x, e.y);	
		}

		/// <summary>This method traps MouseMove events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseMove(object sender, AxTM_VIEW6Lib._DTm_view6Events_MouseMoveEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseMove(this, TranslateMouseButton(e.button), e.x, e.y);	
		}

		/// <summary>This method traps AxError events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxError(object sender, AxTM_VIEW6Lib._DTm_view6Events_AxErrorEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireError(this, "TMView", e.lpszMessage);	
		}

		/// <summary>This method traps AxDiagnostic events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxDiagnostic(object sender, AxTM_VIEW6Lib._DTm_view6Events_AxDiagnosticEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireDiagnostic(this, e.lpszMethod, e.lpszMessage);	
		}
		
		/// <summary>This method traps CreateCallout events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxCreateCallout(object sender, AxTM_VIEW6Lib._DTm_view6Events_CreateCalloutEvent e)
		{
			FireEvent(new CTmxEventArgs(TmxEvents.CreatedCallout, this));
		}

		/// <summary>This method traps SavedPage events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxSavedPage(object sender, AxTM_VIEW6Lib._DTm_view6Events_SavedPageEvent e)
		{
			CTmxEventArgs Args = null;
			
			try
			{
				Args = new CTmxEventArgs(TmxEvents.SavedPage);
				
				Args.Filename = e.lpszSourceFile;
				Args.PageFilename = e.lpszPageFile;
				Args.PageNumber = e.sPage;
				Args.TotalPages = e.sTotal;
				
				FireEvent(Args);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAxSavedPage", Ex);
			}
		
		}// private void OnAxSavedPage(object sender, AxTM_VIEW6Lib._DTm_view6Events_SavedPageEvent e)

		/// <summary>This method traps StartTextEdit events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxStartTextEdit(object sender, AxTM_VIEW6Lib._DTm_view6Events_StartTextEditEvent e)
		{
			m_bTextEditorActive = true;
			FireEvent(new CTmxEventArgs(TmxEvents.StartEditAnnText, this));

		}// private void OnAxStartTextEdit(object sender, AxTM_VIEW6Lib._DTm_view6Events_StartTextEditEvent e)

		/// <summary>This method traps StopTextEdit events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxStopTextEdit(object sender, AxTM_VIEW6Lib._DTm_view6Events_StopTextEditEvent e)
		{
			m_bTextEditorActive = false;
			FireEvent(new CTmxEventArgs(TmxEvents.FinishEditAnnText, this));

		}// private void OnAxStopTextEdit(object sender, AxTM_VIEW6Lib._DTm_view6Events_StopTextEditEvent e)

		/// <summary>This method traps OpenTextBox events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxOpenTextBox(object sender, AxTM_VIEW6Lib._DTm_view6Events_OpenTextBoxEvent e)
		{
			m_bTextEditorActive = true;
			FireEvent(new CTmxEventArgs(TmxEvents.StartEditAnnText, this));
		}

		/// <summary>This method traps CloseTextBox events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxCloseTextBox(object sender, AxTM_VIEW6Lib._DTm_view6Events_CloseTextBoxEvent e)
		{
			m_bTextEditorActive = false;
			FireEvent(new CTmxEventArgs(TmxEvents.FinishEditAnnText, this));
		}

		/// <summary>This method will recalculate the size and position of the Tmview control</summary>
		private void RecalcLayout()
		{
			if(m_ctrlFillPanel == null) return;
			if(m_ctrlFillPanel.IsDisposed == true) return;
			if(m_ctrlTmview == null) return;
			if(m_ctrlTmview.IsDisposed == true) return;
			
			if((m_ctrlFillPanel.Width <= 1) || 
			   (m_ctrlFillPanel.Height <= 1)) return;
			
			//	Are we using fixed screen ratio?
			if(UseScreenRatio == true)
			{
				//	Get the size of the panel
				double dWidth  = (double)m_ctrlFillPanel.Width;
				double dHeight = (double)m_ctrlFillPanel.Height;
				
				//	Compute the center point of the panel
				double dCx = (dWidth / 2.0);
				double dCy = (dHeight / 2.0);
				
				//	Adjust the height or width to the default aspect ratio
				//
				//	Can we use the full available height?
				if((dHeight * DEFAULT_SCREEN_RATIO) <= dWidth)
					dWidth = dHeight * DEFAULT_SCREEN_RATIO;
				else
					dHeight = dWidth / DEFAULT_SCREEN_RATIO;
					
				m_ctrlTmview.SetBounds((int)(dCx - (dWidth / 2.0)),
									   (int)(dCy - (dHeight / 2.0)),
									   (int)(dWidth + 0.5), (int)(dHeight + 0.5), BoundsSpecified.All);					
			}
			else
			{
				//	Use all the available client area
				m_ctrlTmview.SetBounds(0, 0, m_ctrlFillPanel.Width, m_ctrlFillPanel.Height, BoundsSpecified.All);
			}
		
		}
		
		/// <summary>This method is called when the filler panel gets resized</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnPanelResize(object sender, System.EventArgs e)
		{
			//	Resize the TMView control
			RecalcLayout();
		
		}// private void OnPanelResize(object sender, System.EventArgs e)
		
		/// <summary>This method is called to set the check states of the annotation tool buttons</summary>
		/// <param name="eCommand">The command identifier used to determine the button states</param>
		/// <param name="bSaveProperties">true to save the Tmview properties to file</param>
		private void SetToolButton(TmaxMediaBarCommands eCommand, bool bSaveProperties)
		{
			try
			{
				m_bProcessToolClicks = false;
				
				SetUltraToolChecked(TmaxMediaBarCommands.Zoom, eCommand == TmaxMediaBarCommands.Zoom);
				SetUltraToolChecked(TmaxMediaBarCommands.Highlight, eCommand == TmaxMediaBarCommands.Highlight);
				SetUltraToolChecked(TmaxMediaBarCommands.Redact, eCommand == TmaxMediaBarCommands.Redact);
				SetUltraToolChecked(TmaxMediaBarCommands.Select, eCommand == TmaxMediaBarCommands.Select);
				SetUltraToolChecked(TmaxMediaBarCommands.Callout, eCommand == TmaxMediaBarCommands.Callout);
				SetUltraToolChecked(TmaxMediaBarCommands.Pan, eCommand == TmaxMediaBarCommands.Pan);
				
				SetUltraToolChecked(TmaxMediaBarCommands.Freehand, eCommand == TmaxMediaBarCommands.Freehand);
				SetUltraToolChecked(TmaxMediaBarCommands.Ellipse, eCommand == TmaxMediaBarCommands.Ellipse);
				SetUltraToolChecked(TmaxMediaBarCommands.Line, eCommand == TmaxMediaBarCommands.Line);
				SetUltraToolChecked(TmaxMediaBarCommands.Arrow, eCommand == TmaxMediaBarCommands.Arrow);
				SetUltraToolChecked(TmaxMediaBarCommands.Polygon, eCommand == TmaxMediaBarCommands.Polygon);
				SetUltraToolChecked(TmaxMediaBarCommands.Rectangle, eCommand == TmaxMediaBarCommands.Rectangle);
				SetUltraToolChecked(TmaxMediaBarCommands.Text, eCommand == TmaxMediaBarCommands.Text);
				SetUltraToolChecked(TmaxMediaBarCommands.FilledEllipse, eCommand == TmaxMediaBarCommands.FilledEllipse);
				SetUltraToolChecked(TmaxMediaBarCommands.FilledPolygon, eCommand == TmaxMediaBarCommands.FilledPolygon);
			
				SetColorImage();
			}
			catch
			{
			}
			finally
			{
				m_bProcessToolClicks = true;
				
				//	Update the INI file
				if(bSaveProperties == true)
					AxSaveProperties();
			}
		
		}// private void SetToolButton(TmaxMediaBarCommands eCommand)

		/// <summary>This method is called to set the appropriate tool button</summary>
		/// <param name="sAnnTool">The tool identifier</param>
		private void SetToolButton(short sAnnTool)
		{
			TmxViewTools eTool = TmxViewTools.Freehand;
			
			//	Translate the tool identifier to a tool enumerator
			try { eTool = (TmxViewTools)sAnnTool; }
			catch{ eTool = TmxViewTools.Freehand; }
			
			//	Now use a media bar command to set the button state
			switch(eTool)
			{
				case TmxViewTools.Arrow:		
					
					SetToolButton(TmaxMediaBarCommands.Arrow, false);
					break;
					
				case TmxViewTools.Ellipse:		
				
					SetToolButton(TmaxMediaBarCommands.Ellipse, false);
					break;
					
				case TmxViewTools.Line:			
				
					SetToolButton(TmaxMediaBarCommands.Line, false);
					break;
					
				case TmxViewTools.Rectangle:	
				
					SetToolButton(TmaxMediaBarCommands.Rectangle, false);
					break;
					
				case TmxViewTools.FilledEllipse:			
				
					SetToolButton(TmaxMediaBarCommands.FilledEllipse, false);
					break;
					
				case TmxViewTools.Polyline:	
				
					SetToolButton(TmaxMediaBarCommands.Polygon, false);
					break;
					
				case TmxViewTools.Polygon:	
				
					SetToolButton(TmaxMediaBarCommands.FilledPolygon, false);
					break;
					
				case TmxViewTools.Text:	
				
					SetToolButton(TmaxMediaBarCommands.Text, false);
					break;
					
				case TmxViewTools.Freehand:
				default:						
				
					SetToolButton(TmaxMediaBarCommands.Freehand, false);
					break;
			}
		
		}// private void SetToolButton(short sAnnTool)

		/// <summary>This method is called when the user wants to set the color for the current annotation tool</summary>
		private void OnSetColor()
		{
			CTmxViewColorPicker Picker = new CTmxViewColorPicker();
			
			Picker.Color = (TmxViewColors)m_ctrlTmview.GetColor();
			
			if(Picker.ShowDialog(this) == DialogResult.OK)
			{
				m_ctrlTmview.SetColor((short)Picker.Color);
				
				SetColorImage();
				
				//	Update the INI file
				if(AxAutoSave == true)
					AxSaveProperties();
				
			}
			
		}// private void OnSetColor()
		
		/// <summary>This method is called to set the check state of the ShadedCallouts button</summary>
		private void SetShadedCallouts()
		{
			StateButtonTool shadedCallouts = null;
			
			try
			{
				m_bProcessToolClicks = false;
				
				if((shadedCallouts = (StateButtonTool)GetUltraTool("ShadedCallouts")) != null)
				{
					shadedCallouts.Checked = m_ctrlTmview.ShadeOnCallout == 1 ? true : false;
				}
			}
			catch
			{
			}
			finally
			{
				m_bProcessToolClicks = true;
			}
		
		}// private void SetShadedCallouts(bool bShaded)

		/// <summary>This method is called when the user clicks on the Save Zap button</summary>
		private void OnSaveZap()
		{
			string strFilename = System.IO.Path.GetTempFileName();
			
			//	Give the owner a chance to cancel the operation
			if(FireQueryContinue(TmxActions.SaveZap, strFilename, CalloutCount) == true)
			{
				//	Save the current image using a temporary filename
				if((m_sAxError = m_ctrlTmview.SaveZap(strFilename, -1)) == 0)
				{
					FireAction(TmxActions.SaveZap, strFilename);
				}
			}
			
		}// protected void OnSaveZap()
		
		/// <summary>This method is called when the user clicks on the Save Zap button</summary>
		private void OnUpdateZap()
		{
			//	Don't bother if this is not a treatment
			if(m_bIsTreatment == true)
			{
				if(FireQueryContinue(TmxActions.UpdateZap, m_strFilename, CalloutCount) == true)
				{
					//	Save the treatment using the current filename
					if((m_sAxError = m_ctrlTmview.SaveZap(m_strFilename, -1)) == 0)
					{
						FireAction(TmxActions.UpdateZap, m_strFilename);
					}
				}
			
			}
			
		}// private void OnUpdateZap()
		
		/// <summary>This method is called when the user clicks on one of the rotate buttons</summary>
		private void OnRotateImage(short sRotation)
		{
			//	Give the owner a chance to cancel the operation
			if(FireQueryContinue(TmxActions.RotateImage, m_strFilename, CalloutCount) == true)
			{
				m_ctrlTmview.Rotation = sRotation;
				m_ctrlTmview.Rotate(1, -1);

				//	Save the file unless this is a treatment
				if(IsTreatment(m_strFilename) == false)
				{
					//	Save the current image after it's been rotated
					if(Save(m_strFilename) == true)
					{
						FireAction(TmxActions.RotateImage, m_strFilename);
					}
					else
					{
						//	Restore the image
                        m_ctrlTmview.Rotation = (short)(-1 * sRotation);
                        m_ctrlTmview.Rotate(1, -1);
					}
				}
				
			}

		}// private void OnRotateImage(short sRotation)
		
		/// <summary>This method is called when the user clicks on one of the navigator command buttons</summary>
		private void OnNavigatorCommand(TmaxMediaBarCommands eCommand)
		{
			int				iPosition = -1;
			TextBoxTool		goTo = null;
			TmxActions		eAction = TmxActions.None;
			CTmxEventArgs	Args = null;
			
			switch(eCommand)
			{
				case TmaxMediaBarCommands.Next:
				
					if((m_iNavigatorPosition >= 1) && (m_iNavigatorPosition < m_iNavigatorTotal))
					{
						eAction = TmxActions.Next;
						iPosition = m_iNavigatorPosition + 1;
					}
					break;
					
				case TmaxMediaBarCommands.Previous:
				
					if((m_iNavigatorPosition > 1) && (m_iNavigatorPosition <= m_iNavigatorTotal))
					{
						eAction = TmxActions.Previous;
						iPosition = m_iNavigatorPosition - 1;
					}
					break;
					
				case TmaxMediaBarCommands.GoTo:
				
					if(m_iNavigatorTotal > 0)
					{
						if((goTo = (TextBoxTool)GetUltraTool(TmaxMediaBarCommands.GoTo.ToString())) != null)
						{
							if(goTo.Text.Length > 0)
							{
								try
								{
									iPosition = System.Convert.ToInt32(goTo.Text);
									
									if((iPosition < 1) || (iPosition > m_iNavigatorTotal))
									{
										MessageBox.Show("The page number must be between 1 and " + m_iNavigatorTotal.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
									}
									else
									{	
										eAction = TmxActions.GoTo;
									}
								
								}
								catch
								{
									MessageBox.Show("You must enter a valid page number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								}
								
							}
							else
							{
								MessageBox.Show("You must enter the page number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							}
				
						}
					
					}// if(m_iNavigatorTotal > 0)
					break;
					
			}// switch(eCommand)
			
			if(eAction != TmxActions.None)
			{
				//	Initialize the arguments
				Args = new CTmxEventArgs(TmxEvents.Action, this);
				Args.Action = eAction;
				Args.GoTo = iPosition;
				
				FireEvent(Args);
			}
			
		}// private void OnNavigator(TmaxMediaBarCommands eCommand)

		/// <summary>This method is called when the user attempts to delete one or more annotations</summary>
		private void OnDeleteAnnotations()
		{
			//	Is the user selecting annotations?
			if((m_ctrlTmview.Action == (int)(TmxViewActions.Select)) && (m_ctrlTmview.GetSelectCount(-1) > 0))
			{
			    //	Delete the selected annotations
			    m_ctrlTmview.DeleteSelections(-1);
			}
			else
			{
				//	Delete the last annotation drawn by the user
				m_ctrlTmview.DeleteLastAnn(-1);
			}

		}// protected void OnDeleteAnnotations()

		/// <summary>This method is called to set the appropriate image index for the color selection button</summary>
		private void SetColorImage()
		{
			ButtonTool	colorButton = null;
			int			iImage = -1;
			
			if((colorButton = GetUltraButton(TmaxMediaBarCommands.PureBlack)) != null)
			{
				try
				{
					switch((TmxViewColors)m_ctrlTmview.GetColor())
					{
						case TmxViewColors.Red:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.Red);
							break;
								
						case TmxViewColors.Green:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.Green);
							break;
								
						case TmxViewColors.Blue:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.Blue);
							break;
					
						case TmxViewColors.DarkRed:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.DarkRed);
							break;
								
						case TmxViewColors.DarkGreen:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.DarkGreen);
							break;
								
						case TmxViewColors.DarkBlue:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.DarkBlue);
							break;
								
						case TmxViewColors.LightRed:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.LightRed);
							break;
								
						case TmxViewColors.LightGreen:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.LightGreen);
							break;
								
						case TmxViewColors.LightBlue:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.LightBlue);
							break;
								
						case TmxViewColors.Yellow:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.Yellow);
							break;
					
						case TmxViewColors.White:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.PureWhite);
							break;
								
						case TmxViewColors.Black:
							
							iImage = m_tmaxMediaBar.GetImageIndex(TmaxMediaBarCommands.PureBlack);
							break;
								
					}// switch((TmxViewColors)m_ctrlTmview.GetColor())
					
					if(iImage >= 0)
						colorButton.SharedProps.AppearancesSmall.Appearance.Image = iImage;
			
				}
				catch
				{
				
				}
				
			}// private void SetColorImage()
		
		}
		
		/// <summary>Called to determine if the GoTo text box is active</summary>
		private bool IsGoToActive()
		{
			TextBoxTool goTo = null;
			bool		bActive = false;
			
			//	Make sure we have the correct text in the GO TO box
			if((goTo = (TextBoxTool)GetUltraTool(TmaxMediaBarCommands.GoTo.ToString())) != null)
			{
				bActive = goTo.IsInEditMode;
			}
			
			return bActive;
			
		}// private bool IsGoToActive()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>True to lock viewer to 4/3 screen aspect ratio</summary>
		public bool UseScreenRatio
		{
			get 
			{ 
				return m_bUseScreenRatio; 
			}
			set
			{
				if(m_bUseScreenRatio != value)
				{
					m_bUseScreenRatio = value;
				
					if(m_ctrlTmview != null)
					{
						if(m_bUseScreenRatio == true)
							m_ctrlTmview.BackColor = System.Drawing.Color.Black;
						else
							m_ctrlTmview.BackColor = System.Drawing.SystemColors.Control;
					}
					
					RecalcLayout();
				}
			}
			
		}// UseScreenRatio properties
		
		/// <summary>Number of callouts owned by the pane</summary>
		public short CalloutCount
		{
			get 
			{ 
				if((m_ctrlTmview != null) && (m_ctrlTmview.IsDisposed == false))
					return m_ctrlTmview.GetCalloutCount(-1);
				else
					return 0;
			}
			
		}// CalloutCount properties
		
		/// <summary>Source filename used when loading a treatment</summary>
		public string ZapSourceFile
		{
			get { return m_strZapSourceFile; }
			set { m_strZapSourceFile = value; } 
		}

		/// <summary>Flag to idicate that the user is editing a text annotation</summary>
		public bool TextEditorActive
		{
			get { return m_bTextEditorActive; }
			set { m_bTextEditorActive = value; }
		}

		#endregion Properties

	}// public class CTmxView : FTI.Trialmax.ActiveX.CTmxBase

}// namespace FTI.Trialmax.ActiveX

