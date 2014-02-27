using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class creates the view used to display the deposition transcript</summary>
	public class CTmaxVideoTuner : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_ON_APP_OPENED_EX			= ERROR_BASE_VIEW_MAX + 1;
		private const int ERROR_ON_TMAXVIDEO_ACTIVATE_EX	= ERROR_BASE_VIEW_MAX + 2;
		private const int ERROR_SET_SCRIPT_EX				= ERROR_BASE_VIEW_MAX + 3;
		private const int ERROR_SET_DESIGNATION_EX			= ERROR_BASE_VIEW_MAX + 4;
		private const int ERROR_UPDATE_EX					= ERROR_BASE_VIEW_MAX + 5;
		private const int ERROR_ON_EDIT_TRANSCRIPT_EX		= ERROR_BASE_VIEW_MAX + 6;
		private const int ERROR_ON_EDIT_EXTENTS_EX			= ERROR_BASE_VIEW_MAX + 7;
		private const int ERROR_FIRE_ACTIVATE_EX			= ERROR_BASE_VIEW_MAX + 8;

		private const string KEY_PREVIEW_PERIOD				= "PreviewPeriod";

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component container required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>TrialMax control to navigate designations in the active script</summary>
		private FTI.Trialmax.Controls.CTmaxScriptBoxCtrl m_ctrlScriptBar;

		/// <summary>TrialMax child video properties control</summary>
		private FTI.Trialmax.Controls.CTmaxVideoPropsCtrl m_ctrlProperties;

		/// <summary>TrialMax child video tuner control</summary>
		private FTI.Trialmax.Controls.CTmaxVideoTunerCtrl m_ctrlTuner;

		/// <summary>The active designation</summary>
		private FTI.Shared.Xml.CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>The pending designation</summary>
		private FTI.Shared.Xml.CXmlDesignation m_xmlActivate = null;
		
		/// <summary>Collection used to load the script into the TrialMax script navigator</summary>
		private ArrayList m_aXmlScripts = new ArrayList();
		
		/// <summary>Local member to store flag to automatically update when changed</summary>
		private bool m_bApplyAutomatic = true;

		/// <summary>Local member to inhibit processing of video control events</summary>
		private bool m_bIgnoreCtrlEvents = false;

		/// <summary>Local member used to keep track of active designation during script playback</summary>
		private int m_iPlayerIndex = -1;

		/// <summary>Local member used to determine stopping point during script playback</summary>
		private bool m_bPlayToEnd = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoTuner() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			if(m_ctrlTuner != null)
			{
				m_ctrlTuner.EnableSimulation = true;
				m_tmaxEventSource.Attach(m_ctrlTuner.EventSource);
				m_ctrlTuner.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrl);
			}
			if(m_ctrlProperties != null)
			{
				m_tmaxEventSource.Attach(m_ctrlProperties.EventSource);
				m_ctrlProperties.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrl);
			}
			if(m_ctrlScriptBar != null)
			{
				m_tmaxEventSource.Attach(m_ctrlScriptBar.EventSource);
				m_ctrlScriptBar.SceneChangedEvent += new FTI.Trialmax.Controls.CTmaxScriptBoxCtrl.SceneChangedHandler(this.OnScriptBarDesignationChanged);
				m_ctrlScriptBar.PlayEvent += new FTI.Trialmax.Controls.CTmaxScriptBoxCtrl.PlayHandler(this.OnScriptBarPlayCommand);
			}
		
		}// public CTmaxVideoTuner() : base()

		/// <summary>This function is called when the Active property changes</summary>
		protected override void OnActiveChanged()
		{
			//	Is this pane being activated by the user?
			if(m_bActive == true)
			{
				//	Is there an active script?
				if(m_xmlScript != null)
				{
					//	Do we need to activate this script?
					if(m_aXmlScripts.Count == 0)
						SetScript(m_xmlScript);
					
					//	Is there a designation waiting for activation?
					if(m_xmlActivate != null)
					{
						SetDesignation(m_xmlActivate, true);
					}
				
				}// if(m_xmlScript != null)

			}
			else
			{
				//	Make sure we are not playing anything
				Stop();
				m_ctrlTuner.Player.Stop();
			}
			
		}// protected override void OnActiveChanged()
		
		/// <summary>This function is called by the application when the user opens a new XML script</summary>
		/// <param name="xmlScript">The new XML script</param>
		/// <returns>true if successful</returns>
		public override bool OnAppOpened(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Clear the existing script
				Unload();
				
				//	Is this view visible?
				if(m_bActive == true)
					SetScript(xmlScript);
				else
					m_xmlScript = xmlScript; // Keep track for later
				
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
			//	Ignore the request if playing a script
			if(m_iPlayerIndex >= 0) return false;
			
			//	Activate the designation bound to the caller's event item
			if(tmaxItem.XmlDesignation != null)
			{
				//	Has the designation changed?
				if(ReferenceEquals(tmaxItem.XmlDesignation, m_xmlDesignation) == false)
				{
					SetDesignation(tmaxItem.XmlDesignation, true);
				}
			
			}// if(tmaxItem.XmlDesignation != null)
			
			return true;
		}
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoLoadResult(CTmaxVideoResult tmaxResult)
		{
			bool	bSuccessful = true;
//			int		iTranscript = -1;
			
			try
			{
				//	Is a designation bound to the result?
				if(tmaxResult.XmlDesignation != null)
				{
					//	Ignore events while we set up the video
					m_bIgnoreCtrlEvents = true;
					
					//	Activate this designation
					bSuccessful = OnTmaxVideoActivate(new CTmaxItem(m_xmlScript, tmaxResult.XmlDesignation), TmaxVideoViews.Results);
				
					//	If we decide to synchronize the tuner we will put this code back in
//					if(bSuccessful == true)
//					{
//						//	See if we can locate the transcript line at this position
//						if(tmaxResult.XmlDesignation.Transcripts != null)
//							iTranscript = tmaxResult.XmlDesignation.Transcripts.Locate(tmaxResult.PL);
//							
//						//	Reposition the video if this is anything other than the first line
//						if(iTranscript > 0)
//						{
//							//	Prevent attempts to apply changes
//							//
//							//	NOTE:	We bump the start position by 1ms to make sure the control selects the correct line
//							m_ctrlTuner.SetTuneMode(TmaxVideoCtrlTuneModes.None);
//							m_ctrlTuner.SetPosition(tmaxResult.XmlDesignation.Transcripts[iTranscript].Start + 0.001);
//						}
//					
//					}
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnTmaxLoadResult", Ex);
			}
			
			//	Make sure event processing is enabled
			m_bIgnoreCtrlEvents = false;
			
			return bSuccessful;
		
		}// public override bool OnTmaxVideoLoadResult(CTmaxVideoResult tmaxResult)
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			if(tmaxItem.XmlDesignation != null)
			{
				//	Is this our active designation?
				if(ReferenceEquals(tmaxItem.XmlDesignation, m_xmlDesignation) == true)
				{
					//	Update the child controls
					if((m_ctrlTuner != null) && (m_ctrlTuner.IsDisposed  == false))
					{
						m_ctrlTuner.OnAttributesChanged(m_xmlDesignation);
						m_ctrlTuner.SetTuneMode();
					}
					if((m_ctrlProperties != null) && (m_ctrlProperties.IsDisposed  == false))
					{
						m_ctrlProperties.OnAttributesChanged(m_xmlDesignation);
					}
					
				}
				
			}// if(tmaxItem.XmlDesignation != null)
			
			return true;
		
		}// public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, int iId)
		
		/// <summary>This method handles events fired by the Scripts combo box when the user selects a new scene</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="objScript">The current script selection</param>
		/// <param name="objScene">The new scene selection</param>
		private void OnScriptBarDesignationChanged(object objSender, object objScript, object objScene)
		{
			Debug.Assert(objScript != null);
			Debug.Assert(ReferenceEquals(m_xmlScript, objScript) == true);
		
			//	Don't bother if playing a script
			if(m_iPlayerIndex >= 0) return;
			
			if(CanContinue() == true)
			{
				//	Set the designation
				if(SetDesignation((CXmlDesignation)objScene, false) == true)
					FireActivate((CXmlDesignation)objScene);
			}
			else
			{
				m_ctrlScriptBar.SetScene(m_xmlDesignation, true);
			}
		
		}// private void OnScriptBarDesignationChanged(object objSender, object objScript, object objScene)
		
		/// <summary>This method handles events fired by the Scripts combo box when the user wants to play the script</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="objScript">The current script selection</param>
		/// <param name="objScript">The scene to start the playback from</param>
		/// <param name="bPlayToEnd">True to play to the end of the script</param>
		private void OnScriptBarPlayCommand(object objSender, object objScript, object objScene, bool bPlayToEnd)
		{
			//	Are we canceling playback?
			if(objScript == null)
			{
				Stop();
			}
			else
			{
				Debug.Assert(ReferenceEquals(objScript, m_xmlScript) == true);
				if(ReferenceEquals(objScript, m_xmlScript) == false) return;
				
				Play((CXmlDesignation)objScene, bPlayToEnd);
			}
		
		}// private void OnScriptBarPlayCommand(object objSender, object objScript, object objScene, bool bPlayToEnd)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when the application opened a new script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active designation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the tuner's active script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the tuner's active designation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the tuner's active designation.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the transcript for the tuner's active designation: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the extents of the tuner's active designation: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to activate the designation: designation = %1");

		}// protected override void SetErrorStrings()

		/// <summary>This method is called the when the window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			
			//	Perform the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called by the application to initialize the view</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class processing first
			if(base.Initialize(xmlINI) == false)
				return false;
				
			//	Get the preferences from the ini file
			if(xmlINI.SetSection(m_strViewName) == true)
			{
				if(m_ctrlTuner != null)
					m_ctrlTuner.PreviewPeriod = xmlINI.ReadDouble(KEY_PREVIEW_PERIOD, 2.0);
			}
			
			return true;
		}
		
		/// <summary>This method is called by the application when it is about to terminate</summary>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Save the preferences to the ini file
			if(xmlINI.SetSection(m_strViewName) == true)
			{
				if(m_ctrlTuner != null)
					xmlINI.Write(KEY_PREVIEW_PERIOD, m_ctrlTuner.PreviewPeriod);
			}

		}
		
		/// <summary>Used by form designer to lay out child controls</summary> 
		protected override void InitializeComponent()
		{
			this.m_ctrlScriptBar = new FTI.Trialmax.Controls.CTmaxScriptBoxCtrl();
			this.m_ctrlProperties = new FTI.Trialmax.Controls.CTmaxVideoPropsCtrl();
			this.m_ctrlTuner = new FTI.Trialmax.Controls.CTmaxVideoTunerCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlScriptBar
			// 
			this.m_ctrlScriptBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlScriptBar.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlScriptBar.Name = "m_ctrlScriptBar";
			this.m_ctrlScriptBar.Scene = null;
			this.m_ctrlScriptBar.Scenes = null;
			this.m_ctrlScriptBar.Script = null;
			this.m_ctrlScriptBar.Scripts = null;
			this.m_ctrlScriptBar.ShowScripts = false;
			this.m_ctrlScriptBar.Size = new System.Drawing.Size(336, 32);
			this.m_ctrlScriptBar.TabIndex = 0;
			// 
			// m_ctrlProperties
			// 
			this.m_ctrlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlProperties.LinkPosition = -1;
			this.m_ctrlProperties.Location = new System.Drawing.Point(0, 32);
			this.m_ctrlProperties.Name = "m_ctrlProperties";
			this.m_ctrlProperties.Size = new System.Drawing.Size(336, 80);
			this.m_ctrlProperties.StartPosition = -1;
			this.m_ctrlProperties.StopPosition = -1;
			this.m_ctrlProperties.TabIndex = 3;
			// 
			// m_ctrlTuner
			// 
			this.m_ctrlTuner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTuner.EnableLinks = false;
			this.m_ctrlTuner.EnableSimulation = false;
			this.m_ctrlTuner.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_ctrlTuner.LinkPosition = -1;
			this.m_ctrlTuner.Location = new System.Drawing.Point(0, 112);
			this.m_ctrlTuner.Name = "m_ctrlTuner";
			this.m_ctrlTuner.PreviewPeriod = 2;
			this.m_ctrlTuner.SimulationText = "";
			this.m_ctrlTuner.Size = new System.Drawing.Size(336, 296);
			this.m_ctrlTuner.StartPosition = -1;
			this.m_ctrlTuner.StopPosition = -1;
			this.m_ctrlTuner.TabIndex = 2;
			// 
			// CTmaxVideoTuner
			// 
			this.Controls.Add(this.m_ctrlProperties);
			this.Controls.Add(this.m_ctrlTuner);
			this.Controls.Add(this.m_ctrlScriptBar);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Name = "CTmaxVideoTuner";
			this.Size = new System.Drawing.Size(336, 408);
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
	
		/// <summary>This method is called to unload the child controls</summary>
		private void Unload()
		{
			try
			{
				//	Reset the local members
				m_aXmlScripts.Clear();
				m_xmlScript = null;
				m_xmlActivate = null;
				m_iPlayerIndex = -1;
				SetDesignation(null, false);
				
				//	Notify the controls
				if((m_ctrlScriptBar != null) && (m_ctrlScriptBar.IsDisposed == false))
				{
					m_ctrlScriptBar.SetScripts(null);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Unload", Ex);
			}
			
		}// private void Unload()

		/// <summary>This method is called to load the specified script</summary>
		/// <param name="xmlScript">The script to be loaded</param>
		/// <returns>true if successful</returns>
		private bool SetScript(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			try
			{
				m_xmlScript = xmlScript;
				
				//	Populate the collection that we use to initialize the script navigation toolbar
				m_aXmlScripts.Clear();
				m_aXmlScripts.Add(m_xmlScript);
				
				//	Notify the script toolbar
				m_ctrlScriptBar.SetScripts(m_aXmlScripts);
				m_ctrlScriptBar.SetScript(m_xmlScript, true);
				m_ctrlScriptBar.SetScenes(m_xmlScript.XmlDesignations);
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScript", m_tmaxErrorBuilder.Message(ERROR_SET_SCRIPT_EX), Ex);
			}
			
			return bSuccessful;
		
		}// private void SetScript(CXmlScript xmlScript)
		
		/// <summary>This method is called to activate the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be activated</param>
		/// <param name="bSynchronize">True to synchronize the script selection control</param>
		/// <returns>true if successful</returns>
		private bool SetDesignation(CXmlDesignation xmlDesignation, bool bSynchronize)
		{
			string	strVideoFileSpec = "";
			bool	bSuccessful = false;
			
			try
			{
				//	Make sure it's ok to continue
				if(CanContinue() == false) return false;
				
				//	Store the reference to the active designation
				m_xmlDesignation = xmlDesignation;
				
				//	Clear the pending activation
				m_xmlActivate = null;
				
				if(m_xmlDesignation != null)
				{
					//	Get the path to the video bound to this designation
					strVideoFileSpec = GetVideoFileSpec(xmlDesignation, false, true);
					if((strVideoFileSpec == null) || (strVideoFileSpec.Length == 0)) return false;
					
					//	Does this file exist?
					if(System.IO.File.Exists(strVideoFileSpec) == false)
						m_ctrlTuner.SimulationText = strVideoFileSpec.ToLower() + " not found";
					else
						m_ctrlTuner.SimulationText = "";

					//	Load this designation in the child controls
					m_ctrlProperties.SetProperties(strVideoFileSpec, xmlDesignation);
					m_ctrlTuner.SetProperties(strVideoFileSpec, xmlDesignation);

				}
				else
				{
					//	Clear the child controls
					m_ctrlProperties.SetProperties("", null);
					m_ctrlTuner.SetProperties("", null);
				}
				
				//	Should we notify the script selection control?
				if(bSynchronize == true)
				{
					if((m_ctrlScriptBar != null) && (m_ctrlScriptBar.IsDisposed == false))
					{
						m_ctrlScriptBar.SetScene(m_xmlDesignation, true);
					}
				
				}

				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDesignation", m_tmaxErrorBuilder.Message(ERROR_SET_DESIGNATION_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private void SetDesignation(CXmlDesignation xmlDesignation)
		
		/// <summary>This method handles all events where the user is requesting to apply changes</summary>
		private void OnApply()
		{
			//	Update the active designation
			if(m_xmlDesignation != null)
				Update(m_xmlDesignation);
				
		}// private void OnApply()
	
		/// <summary>This method is called to see if it's ok to continue to operation in progress</summary>
		/// <returns>true if ok to continue</returns>
		private bool CanContinue()
		{
			bool		bContinue = true;
			bool		bModified = false;
			ArrayList	aModifications = new ArrayList();

			//	Do we need to check the active designation?
			if(m_xmlDesignation != null)
			{
				if(IsModified(m_xmlDesignation, aModifications) == true)
					bModified = true;
			}
			
			//	Has something changed?
			if(bModified == true)
			{
				//	Are we automatically applying changes?
				if(m_bApplyAutomatic)
				{
					//	Make it look like the user clicked on apply
					OnApply();
				}
				else
				{
					//	Prompt for response
					CTmaxVideoPrompt prompt = new CTmaxVideoPrompt();
					
					prompt.Modifications = aModifications;
					
					switch(prompt.ShowDialog(this))
					{
						case DialogResult.Yes:
						
							OnApply();
							break;
							
						case DialogResult.Cancel:
						
							bContinue = false;
							break;
					
					}// switch(prompt.ShowDialog(this))
					
					if(prompt.ApplyAutomatic == true)
					{
						m_bApplyAutomatic = true;
					}
				
				}
			
			}
			
			//	Clear the temporary collection
			if(aModifications != null)
				aModifications.Clear();
				
			return bContinue;
			
		}// private bool CanContinue()
		
		/// <summary>This method is called to determine if the control properties match those of the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be compared</param>
		///	<param name="aModifications">Array in which to store a list of the modifications</param>
		/// <returns>true if there are modifications</returns>
		private bool IsModified(CXmlDesignation xmlDesignation, ArrayList aModifications)
		{
			bool bModified = false;
			
			//	Do we have an active designation?
			if(xmlDesignation == null) return false;

			//	Check each of the controls that edit designation properties
			if(m_ctrlTuner.IsModified(aModifications) == true)
				bModified = true;
				
			if(m_ctrlProperties.IsModified(aModifications) == true)
				bModified = true;
			
			return bModified;
		
		}// private bool IsModified(CXmlDesignation xmlDesignation)
		
		/// <summary>This method handles all video events fired by one of the child controls</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnTmaxVideoCtrl(object sender, CTmaxVideoCtrlEventArgs e)
		{
			//	Which event?
			switch(e.EventId)
			{
				case TmaxVideoCtrlEvents.Apply:

					OnApply();
					break;
					
				case TmaxVideoCtrlEvents.EditDesignationExtents:

					Debug.Assert(e.XmlDesignation != null);
					Debug.Assert(ReferenceEquals(e.XmlDesignation, m_xmlDesignation) == true);
					
					OnEditExtents();
					break;
					
				case TmaxVideoCtrlEvents.EditDesignationText:
				
					Debug.Assert(e.XmlDesignation != null);
					Debug.Assert(ReferenceEquals(e.XmlDesignation, m_xmlDesignation) == true);
					
					OnEditTranscript();
					break;
					
				case TmaxVideoCtrlEvents.QueryCanContinue:
				
					e.QueryHandled = true;
					if(e.CheckDesignation == true)
						e.CanContinue = CanContinue();
					break;
					
				case TmaxVideoCtrlEvents.ScriptDesignationComplete:
				
					OnPlayerDesignationComplete();
					break;
					
				case TmaxVideoCtrlEvents.ScriptTranscriptChanged:
				case TmaxVideoCtrlEvents.PlayerTranscriptChanged:
				
					if(m_bIgnoreCtrlEvents == false)
						OnPlayerTranscriptChanged(e.XmlDesignation, e.TranscriptIndex);
					break;

				case TmaxVideoCtrlEvents.ScriptLinkChanged:
				case TmaxVideoCtrlEvents.AddLink:
				case TmaxVideoCtrlEvents.QueryLinkSourceDbId:
				case TmaxVideoCtrlEvents.SetLink:
				case TmaxVideoCtrlEvents.QueryLinkDropBarcode:
				default:
				
					break;
					
			}// switch(e.EventId)
		
		}// private void OnTmaxVideoCtrl(object objSender, FTI.Trialmax.Controls.CTmaxVideoTuneBarCtrl.TmaxTuneBarCommands eCommand)
	
		/// <summary>This method is called to update the specified designation</summary>
		/// <param name="xmlDesignation">The XML designation to be updated</param>
		/// <returns>true if successful</returns>
		private bool Update(CXmlDesignation xmlDesignation)
		{
			CTmaxVideoArgs	Args = null;
			bool			bSuccessful = false;
			
			Debug.Assert(xmlDesignation != null);
			if(xmlDesignation == null) return false;
			
			try
			{
				//	Update the attributes using the child controls
				m_ctrlTuner.SetAttributes(xmlDesignation);
				m_ctrlProperties.SetAttributes(xmlDesignation);

				//	Fire the command to update the script
				if((Args = FireCommand(TmaxVideoCommands.Update, new CTmaxItem(m_xmlScript, xmlDesignation))) != null)
					bSuccessful = Args.Successful;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Update", m_tmaxErrorBuilder.Message(ERROR_UPDATE_EX), Ex);
			}
		
			return bSuccessful;
			
		}// private bool Update(CXmlDesignation xmlDesignation)
			
		/// <summary>This method handles all events where the user requests editing of the extents for the active designation</summary>
		private void OnEditExtents()
		{
			bool bModified = false;

			Debug.Assert(m_xmlScript != null);
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlScript == null) return;
			if(m_xmlDesignation == null) return;
			
			try
			{
				CFTmaxVideoDesignations editor = new CFTmaxVideoDesignations();
				
				editor.View = this.AppId;
				editor.XmlScript = m_xmlScript;
				editor.XmlDesignation = m_xmlDesignation;
				editor.VideoOptions = m_tmaxAppOptions;
				editor.EditExtents = true;
				m_tmaxEventSource.Attach(editor.EventSource);
				editor.TmaxVideoCommandEvent += new TmaxVideoHandler(this.OnTmaxCommand);
				
				bModified = (editor.ShowDialog() == DialogResult.OK);
			
				if(bModified == true)
				{
					m_ctrlTuner.SetTuneMode();
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnEditExtents", m_tmaxErrorBuilder.Message(ERROR_ON_EDIT_EXTENTS_EX, m_xmlDesignation.Name), Ex);
			}
			
		}// private void OnEditExtents()
		
		/// <summary>This method handles all events where the user requests editing of the text for the active designation</summary>
		private void OnEditTranscript()
		{
			CFEditTranscript Editor = null;
			
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return;
			
			try
			{
				//	Allocate and initialize the form
				Editor = new CFEditTranscript();
				Editor.Transcripts = m_xmlDesignation.Transcripts;
				
				//	Open the editor
				Editor.ShowDialog();
				
				//	Was any text modified?
				if(Editor.Modified == true)
				{
					//	Notify the application
					FireCommand(TmaxVideoCommands.Update, new CTmaxItem(m_xmlScript, m_xmlDesignation));
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnEditTranscript", m_tmaxErrorBuilder.Message(ERROR_ON_EDIT_TRANSCRIPT_EX, m_xmlDesignation.Name), Ex);
			}
			
		}// private void OnEditTranscript()
		
		/// <summary>This method is called to start playing the active script</summary>
		/// <param name="xmlDesignation">The designation to start the playback from</param>
		/// <param name="bPlayToEnd">True to play to the end of the script</param>
		/// <returns>true if successful</returns>
		private bool Play(CXmlDesignation xmlDesignation, bool bPlayToEnd)
		{
			//	Make sure we have a collection of designations
			if(m_xmlScript == null) return false;
			if(m_xmlScript.XmlDesignations == null) return false;
			if(m_xmlScript.XmlDesignations.Count == 0) return false;
			
			//	Make sure it's ok to run the script
			if(CanContinue() == false)
			{
				m_ctrlScriptBar.StopScript();
				return false;
			}
			
			//	Get the index of the designation to start the playback from
			if(xmlDesignation != null)
			{
				//	Locate the specified designation
				if((m_iPlayerIndex = m_xmlScript.XmlDesignations.IndexOf(xmlDesignation)) < 0)
				{
					if(MessageBox.Show("Unable to start playback at the specified designation. Playback will start at the beginning.", "Error",
					   MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
					{
						m_iPlayerIndex = 0;
					}
					
				}
			
			}
			else
			{
				m_iPlayerIndex = 0;
			}
			
			//	Do we have a valid start position?
			if((m_iPlayerIndex < 0) || (m_iPlayerIndex >= m_xmlScript.XmlDesignations.Count))
				return false;
						
			//	Put the video controls in script playback mode
			m_ctrlScriptBar.StartScript();
			m_ctrlProperties.StartScript();
			m_ctrlTuner.StartScript();		
			
			//	Save this flag to control playback
			m_bPlayToEnd = bPlayToEnd;
			
			//	Now set the active designation
			if(SetDesignation(m_xmlScript.XmlDesignations[m_iPlayerIndex], false) == true)
				FireActivate(m_xmlScript.XmlDesignations[m_iPlayerIndex]);
		
			return true;
			
		}// private bool Play(CXmlDesignation xmlDesignation, bool bPlayToEnd)

		/// <summary>This method is called to stop playing the active script</summary>
		private void Stop()
		{
			try
			{
				//	Is the user playing the active script?
				if(m_iPlayerIndex >= 0)
				{
					m_iPlayerIndex = -1;
					
					//	Stop the playback
					m_ctrlTuner.StopScript();			
					m_ctrlProperties.StopScript();
					m_ctrlScriptBar.StopScript();
				
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Stop", Ex);
			}
			
		}// private void Stop()

		/// <summary>This method handles events fired by the video player it changes the transcript text</summary>
		/// <param name="xmlDesignation">The designation being previewed</param>
		///	<param name="iTranscript">The index of the active transcript</param>
		private void OnPlayerTranscriptChanged(CXmlDesignation xmlDesignation, int iTranscript)
		{
			try
			{
				if((xmlDesignation != null) && (xmlDesignation.Transcripts != null))
				{
					if((iTranscript >= 0) && (iTranscript < xmlDesignation.Transcripts.Count))
					{
						FireActivate(xmlDesignation, xmlDesignation.Transcripts[iTranscript]);
					}
					
				}
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnPlayerTranscriptChanged", Ex);
			}

		}// private void OnPlayerTranscriptChanged(object sender, CXmlDesignation xmlDesignation, int iTranscript)
		
		/// <summary>This method handles events fired by the player when the designation is complete</summary>
		private void OnPlayerDesignationComplete()
		{
			//	Have we canceled the playback
			if(m_iPlayerIndex < 0) return;
			
			//	Go to the next designation
			m_iPlayerIndex++;
			
			//	Should we keep playing?
			if((m_bPlayToEnd == true) && (m_iPlayerIndex < m_xmlScript.XmlDesignations.Count))
			{
				if(SetDesignation(m_xmlScript.XmlDesignations[m_iPlayerIndex], true) == true)
					FireActivate(m_xmlScript.XmlDesignations[m_iPlayerIndex]);
			}
			else
			{
				Stop();
			}
		
		}// private void OnPlayerDesignationComplete()
		
		/// <summary>This method is called to fire the application command to activate the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be activated</param>
		/// <param name="xmlTranscript">The transcript to be activated</param>
		/// <returns>true if successful</returns>
		private bool FireActivate(CXmlDesignation xmlDesignation, CXmlTranscript xmlTranscript)
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;
			bool			bSuccessful = false;
			
			//	Make sure we have the required objects
			if(m_xmlScript == null) return false;
			if(xmlDesignation == null) return false;
			
			try
			{
				tmaxItem = new CTmaxItem(m_xmlScript, xmlDesignation, xmlTranscript);
				
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.SyncMediaTree, true);

				FireCommand(TmaxVideoCommands.Activate, tmaxItem, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireActivate", m_tmaxErrorBuilder.Message(ERROR_FIRE_ACTIVATE_EX, xmlDesignation != null ? xmlDesignation.Name : "NULL"), Ex);
			}
			
			return bSuccessful;
			
		}// private bool FireActivate(CXmlDesignation xmlDesignation)

		/// <summary>This method is called to fire the application command to activate the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be activated</param>
		/// <returns>true if successful</returns>
		private bool FireActivate(CXmlDesignation xmlDesignation)
		{
			return FireActivate(xmlDesignation, null);
		}

		#endregion Private Methods
	
	}//  public class CTmaxVideoTuner : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView

}//  namespace FTI.Trialmax.TMVV.Tmvideo
