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
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Panes
{
	/// <summary>This control implements the pane used to tune recording clips</summary>
	public class CTunePane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants
		
		protected const int ERROR_NO_SCENE_SOURCE		= (ERROR_BASE_PANE_MAX + 1);
		protected const int ERROR_NO_EXTENTS			= (ERROR_BASE_PANE_MAX + 2);
		protected const int ERROR_NO_XML_DESIGNATION	= (ERROR_BASE_PANE_MAX + 3);
		protected const int ERROR_NO_VIDEO_FILE			= (ERROR_BASE_PANE_MAX + 4);
		protected const int ERROR_VIDEO_NOT_FOUND		= (ERROR_BASE_PANE_MAX + 5);
		protected const int ERROR_EDIT_TEXT_EX			= (ERROR_BASE_PANE_MAX + 6);
		protected const int ERROR_ON_DELETED_EX			= (ERROR_BASE_PANE_MAX + 7);
		
		protected const string KEY_PREVIEW_PERIOD		= "PreviewPeriod";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Local member used to manage the list of scripts stored in the database</summary>
		private CDxPrimaries m_dxScripts = new CDxPrimaries();

		/// <summary>Local member used to manage the list of scenes associated with the active script</summary>
		private CDxSecondaries m_dxScenes = new CDxSecondaries();

		/// <summary>Local member used to keep the scenes list properly sorted</summary>
		private CTmaxRecordSorter m_tmaxScenesSorter = new CTmaxRecordSorter();

		/// <summary>Local member used to track the current script selection</summary>
		private CDxPrimary m_dxScript = null;

		/// <summary>Local member used to track the source record for the current scene</summary>
		private CDxTertiary m_dxTertiary = null;

		/// <summary>Local member used to manage the list of scenes associated with the active script</summary>
		private CDxSecondary m_dxScene = null;

		/// <summary>Local member used to keep track of active scene during script playback</summary>
		private int m_iScriptScene = -1;

		/// <summary>Local member used to keep track of whether or not script should be played to the end</summary>
		private bool m_bScriptToEnd = false;

		/// <summary>Local member used to keep track of the active link</summary>
		private CDxQuaternary m_dxLink = null;

		/// <summary>Local member used to store item waiting for activation</summary>
		private CTmaxItem m_tmaxActivate = null;

		/// <summary>Local member to manage the active designation</summary>
		private CXmlDesignation m_xmlDesignation = null;

		/// <summary>Local member to manage the active link</summary>
		private CXmlLink m_xmlLink = null;

		/// <summary>Custom TrialMax script selection combobox control</summary>
		private FTI.Trialmax.Controls.CTmaxScriptBoxCtrl m_ctrlScripts;

		/// <summary>Panel used as filler for client area not used by the scripts combobox</summary>
		private System.Windows.Forms.Panel m_ctrlFillScripts;

		/// <summary>Custom TrialMax video properties control</summary>
		private FTI.Trialmax.Controls.CTmaxVideoPropsCtrl m_ctrlVideoProps;

		/// <summary>Splitter bar attached to the bottom of the video properties control</summary>
		private System.Windows.Forms.Splitter m_crtlVideoPropsSplitter;

		/// <summary>Custom TrialMax links editing control</summary>
		private FTI.Trialmax.Controls.CTmaxVideoLinksCtrl m_ctrlLinks;

		/// <summary>Splitter bar attached to the top of the links editor control</summary>
		private System.Windows.Forms.Splitter m_ctrlLinksSplitter;

		/// <summary>Custom TrialMax video playback/tuner control</summary>
		private FTI.Trialmax.Controls.CTmaxVideoTunerCtrl m_ctrlTuner;

		/// <summary>Local member to store flag to automatically update when changed</summary>
		private bool m_bApplyAutomatic = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTunePane()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			//	Attach the sorter to the local scenes collection
			m_dxScenes.Comparer = m_tmaxScenesSorter;
			m_dxScenes.KeepSorted = false;
			
			//	Connect the child controls event sources
			if(m_ctrlScripts != null)
			{
				m_tmaxEventSource.Attach(m_ctrlScripts.EventSource);
			}
			if(m_ctrlLinks != null)
			{
				m_tmaxEventSource.Attach(m_ctrlLinks.EventSource);
				m_ctrlLinks.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrlEvent);
			}
			if(m_ctrlTuner != null)
			{
				m_ctrlTuner.EnableSimulation = true;
				m_tmaxEventSource.Attach(m_ctrlTuner.EventSource);
				m_ctrlTuner.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrlEvent);
			}
			if(m_ctrlVideoProps != null)
			{
				m_tmaxEventSource.Attach(m_ctrlVideoProps.EventSource);
				m_ctrlVideoProps.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrlEvent);
			}

		}// public CTunePane()

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

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.m_ctrlScripts = new FTI.Trialmax.Controls.CTmaxScriptBoxCtrl();
			this.m_ctrlFillScripts = new System.Windows.Forms.Panel();
			this.m_ctrlTuner = new FTI.Trialmax.Controls.CTmaxVideoTunerCtrl();
			this.m_ctrlLinksSplitter = new System.Windows.Forms.Splitter();
			this.m_ctrlLinks = new FTI.Trialmax.Controls.CTmaxVideoLinksCtrl();
			this.m_crtlVideoPropsSplitter = new System.Windows.Forms.Splitter();
			this.m_ctrlVideoProps = new FTI.Trialmax.Controls.CTmaxVideoPropsCtrl();
			this.m_ctrlFillScripts.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlScripts
			// 
			this.m_ctrlScripts.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_ctrlScripts.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlScripts.Name = "m_ctrlScripts";
			this.m_ctrlScripts.Scene = null;
			this.m_ctrlScripts.Scenes = null;
			this.m_ctrlScripts.Script = null;
			this.m_ctrlScripts.Scripts = null;
			this.m_ctrlScripts.ShowScripts = true;
			this.m_ctrlScripts.Size = new System.Drawing.Size(312, 52);
			this.m_ctrlScripts.TabIndex = 0;
			this.m_ctrlScripts.ScriptChangedEvent += new FTI.Trialmax.Controls.CTmaxScriptBoxCtrl.ScriptChangedHandler(this.OnScriptChanged);
			this.m_ctrlScripts.PlayEvent += new FTI.Trialmax.Controls.CTmaxScriptBoxCtrl.PlayHandler(this.OnScriptsCmdPlay);
			this.m_ctrlScripts.SceneChangedEvent += new FTI.Trialmax.Controls.CTmaxScriptBoxCtrl.SceneChangedHandler(this.OnSceneChanged);
			// 
			// m_ctrlFillScripts
			// 
			this.m_ctrlFillScripts.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlFillScripts.Controls.Add(this.m_ctrlTuner);
			this.m_ctrlFillScripts.Controls.Add(this.m_ctrlLinksSplitter);
			this.m_ctrlFillScripts.Controls.Add(this.m_ctrlLinks);
			this.m_ctrlFillScripts.Controls.Add(this.m_crtlVideoPropsSplitter);
			this.m_ctrlFillScripts.Controls.Add(this.m_ctrlVideoProps);
			this.m_ctrlFillScripts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlFillScripts.Location = new System.Drawing.Point(0, 52);
			this.m_ctrlFillScripts.Name = "m_ctrlFillScripts";
			this.m_ctrlFillScripts.Size = new System.Drawing.Size(312, 516);
			this.m_ctrlFillScripts.TabIndex = 14;
			// 
			// m_ctrlTuner
			// 
			this.m_ctrlTuner.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlTuner.EnableLinks = true;
			this.m_ctrlTuner.EnableSimulation = false;
			this.m_ctrlTuner.LinkPosition = -1;
			this.m_ctrlTuner.Location = new System.Drawing.Point(0, 83);
			this.m_ctrlTuner.Name = "m_ctrlTuner";
			this.m_ctrlTuner.PreviewPeriod = 2;
			this.m_ctrlTuner.SimulationText = "";
			this.m_ctrlTuner.Size = new System.Drawing.Size(312, 292);
			this.m_ctrlTuner.StartPosition = -1;
			this.m_ctrlTuner.StopPosition = -1;
			this.m_ctrlTuner.TabIndex = 31;
			// 
			// m_ctrlLinksSplitter
			// 
			this.m_ctrlLinksSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_ctrlLinksSplitter.Location = new System.Drawing.Point(0, 375);
			this.m_ctrlLinksSplitter.Name = "m_ctrlLinksSplitter";
			this.m_ctrlLinksSplitter.Size = new System.Drawing.Size(312, 3);
			this.m_ctrlLinksSplitter.TabIndex = 30;
			this.m_ctrlLinksSplitter.TabStop = false;
			// 
			// m_ctrlLinks
			// 
			this.m_ctrlLinks.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_ctrlLinks.LinkPosition = -1;
			this.m_ctrlLinks.Location = new System.Drawing.Point(0, 378);
			this.m_ctrlLinks.Name = "m_ctrlLinks";
			this.m_ctrlLinks.Size = new System.Drawing.Size(312, 138);
			this.m_ctrlLinks.StartPosition = -1;
			this.m_ctrlLinks.StopPosition = -1;
			this.m_ctrlLinks.TabIndex = 29;
			// 
			// m_crtlVideoPropsSplitter
			// 
			this.m_crtlVideoPropsSplitter.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_crtlVideoPropsSplitter.Location = new System.Drawing.Point(0, 80);
			this.m_crtlVideoPropsSplitter.MinSize = 2;
			this.m_crtlVideoPropsSplitter.Name = "m_crtlVideoPropsSplitter";
			this.m_crtlVideoPropsSplitter.Size = new System.Drawing.Size(312, 3);
			this.m_crtlVideoPropsSplitter.TabIndex = 22;
			this.m_crtlVideoPropsSplitter.TabStop = false;
			// 
			// m_ctrlVideoProps
			// 
			this.m_ctrlVideoProps.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlVideoProps.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_ctrlVideoProps.LinkPosition = -1;
			this.m_ctrlVideoProps.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlVideoProps.Name = "m_ctrlVideoProps";
			this.m_ctrlVideoProps.Size = new System.Drawing.Size(312, 80);
			this.m_ctrlVideoProps.StartPosition = -1;
			this.m_ctrlVideoProps.StopPosition = -1;
			this.m_ctrlVideoProps.TabIndex = 21;
			// 
			// CTunePane
			// 
			this.Controls.Add(this.m_ctrlFillScripts);
			this.Controls.Add(this.m_ctrlScripts);
			this.Name = "CTunePane";
			this.Size = new System.Drawing.Size(312, 568);
			this.m_ctrlFillScripts.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		/// <summary>This function is called when the PresentationOptions property changes</summary>
		protected override void OnPresentationOptionsChanged()
		{
			bool bClassicLinks = false;
			
			try
			{
				if(this.PresentationOptions != null)
					bClassicLinks = this.PresentationOptions.ClassicLinks;
					
				if(m_ctrlLinks != null)
					m_ctrlLinks.ClassicLinks = bClassicLinks;
				if(m_ctrlTuner != null)
					m_ctrlTuner.ClassicLinks = bClassicLinks;
				if(m_ctrlVideoProps != null)
					m_ctrlVideoProps.ClassicLinks = bClassicLinks;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnPresentationOptionsChanged", Ex);
			}

		}// protected override void OnPresentationOptionsChanged()

		/// <summary>This function is called when the the user has closed the Presentation Options form</summary>
		/// <param name="bCancelled">true if the user cancelled the operation</param>
		public override void OnAfterSetPresentationOptions(bool bCancelled)
		{
			//	Update the child controls
			if(bCancelled == false)
				OnPresentationOptionsChanged();
		}

		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Unload the viewer whenever the database changes
			Unload();
			
			//	Reset the script selection list
			FillScripts();
						
		}// OnDatabaseChanged()
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			bool			bRefresh = false;
			CDxSecondary	dxScene = null;
			
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxChildren != null);
			
			//	All new media should be in the child collection
			Debug.Assert(tmaxChildren.Count > 0);
			if(tmaxChildren.Count == 0) return;
		
			//	Are we adding new primary media?
			if(tmaxParent.MediaLevel == TmaxMediaLevels.None)
			{ 
				foreach(CTmaxItem tmaxChild in tmaxChildren)
				{
					if(tmaxChild.MediaType == TmaxMediaTypes.Script)
					{
						//	Add to the local scripts collection
						if(tmaxChild.GetMediaRecord() != null)
						{
							m_dxScripts.AddList((CDxMediaRecord)tmaxChild.GetMediaRecord());
							bRefresh = true;
						}
					}
				
				}
				
				//	Do we need to refresh the script selection control?
				if(bRefresh == true)
					m_ctrlScripts.RefreshScripts(false);

			}
			//	New scenes?
			else if(tmaxParent.MediaType == TmaxMediaTypes.Script)
			{
				//	Are we adding scenes to the active script?
				if((m_dxScript != null) &&
				   (ReferenceEquals(m_dxScript, tmaxParent.GetMediaRecord()) == true))
				{
					//	Check each of the new children
					foreach(CTmaxItem tmaxChild in tmaxChildren)
					{
						if((dxScene = (CDxSecondary)tmaxChild.GetMediaRecord()) != null)
						{
							//	Add to the scenes collection if this is a designation or clip
							if((dxScene.SourceType == TmaxMediaTypes.Clip) ||
							   (dxScene.SourceType == TmaxMediaTypes.Designation))
							{
								m_dxScenes.AddList(dxScene);
								bRefresh = true;
							}
							
						}
							
					}// foreach(CTmaxItem tmaxChild in tmaxChildren)
					
					//	Do we need to refresh the scenes?
					if(bRefresh == true)
					{
						m_dxScenes.Sort();
						m_ctrlScripts.RefreshScenes(false);
					}
				
				}
				
			}
			//	New links ?
			else if(ReferenceEquals(tmaxParent.GetMediaRecord(), m_dxTertiary) == true)
			{
				//	Add each new link to the box
				foreach(CTmaxItem tmaxChild in tmaxChildren)
				{
					if((tmaxChild.GetMediaRecord() != null) &&
					   (tmaxChild.XmlLink != null))
					{
						m_ctrlLinks.Add(tmaxChild.XmlLink, false);
					}
					
				}
				
				//	Select the first new link
				if(tmaxChildren.Count > 0)
				{
					if(tmaxChildren[0].XmlLink != null)
						m_ctrlLinks.SetLink(tmaxChildren[0].XmlLink);
				}
			
			}
			
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called when the user has deleted children from the specified parent</summary>
		/// <param name="tmaxItem">The parent of the deleted records</param>
		protected void OnDeleted(CTmaxItem tmaxParent)
		{
			bool		bRefresh = false;
			CXmlLink	xmlLink = null;
			
			//	Don't bother if this is a pick list item
			if(tmaxParent.DataType == TmaxDataTypes.PickItem) return;
			
			//	Have primary records been deleted?
			if(tmaxParent.MediaLevel == TmaxMediaLevels.None)
			{
				//	Has the active script been deleted
				foreach(CTmaxItem O in tmaxParent.SubItems)
				{
					//	Check to see if any scripts have been deleted
					if(O.MediaType == TmaxMediaTypes.Script)
					{
						//	Remove this script from the local collection
						if(m_dxScripts.Contains(O.GetMediaRecord()) == true)
						{
							m_dxScripts.Remove(O.GetMediaRecord());
							bRefresh = true;
						}
						
					}// if(O.MediaType == TmaxMediaTypes.Script)
					
				}// foreach(CTmaxItem O in tmaxParent.SubItems)
				
				//	Do we need to refresh the scripts?
				if(bRefresh == true)
				{
					// NOTE: If the active script has been deleted this
					//		 will cause a ScriptChange event to get fired
					m_ctrlScripts.RefreshScripts(false);
				}
				
			}
			
			//	Have secondary records been deleted?
			else if(tmaxParent.MediaLevel == TmaxMediaLevels.Primary)
			{
				//	Have they been deleted from the active script?
				if((m_dxScript != null) && (m_dxScenes != null) &&
				   (ReferenceEquals(m_dxScript, tmaxParent.GetMediaRecord()) == true))
				{
					//	Check to see if any of the scenes are in our local collection
					foreach(CTmaxItem O in tmaxParent.SubItems)
					{
						if(m_dxScenes.Contains(O.GetMediaRecord()) == true)
						{
							m_dxScenes.Remove(O.GetMediaRecord());
							bRefresh = true;
						}
					
					}// foreach(CTmaxItem O in tmaxParent.SubItems)

				}// if(ReferenceEquals(ReferenceEquals(m_dxScript, tmaxParent.GetMediaRecord()) == true)
				
				//	Do we need to refresh the scenes
				if(bRefresh == true)
				{
					m_dxScenes.Sort();
					m_ctrlScripts.RefreshScenes(false);
					
					//	Has the active scene been deleted?
					if((m_dxScene != null) && (m_tmaxDatabase.IsValidRecord(m_dxScene) == false))
						SetScene(null, true);
				}
				
			}
			
			//	Have quaternary records been deleted?
			else if(tmaxParent.MediaLevel == TmaxMediaLevels.Tertiary)
			{
				//	Have links from our active designation/clip been deleted?
				if(ReferenceEquals(tmaxParent.ITertiary, m_dxTertiary) == true)
				{
					Debug.Assert(m_xmlDesignation != null);
					if(m_xmlDesignation == null) return;
					
					foreach(CTmaxItem O in tmaxParent.SubItems)
					{
						xmlLink = m_xmlDesignation.Links.Find(O.GetUniqueId());
						if(xmlLink != null)
						{
							//	Notify the tune control and remove from the XML designation
							m_ctrlLinks.Delete(xmlLink);
							m_xmlDesignation.Links.Remove(xmlLink);
						}
					
					}
				
				}
				
				//	Do we need to refresh the scenes
				if(bRefresh == true)
				{
					m_dxScenes.Sort();
					m_ctrlScripts.RefreshScenes(false);
				}
				
			}

		}// protected void OnDeleted(CTmaxItem tmaxParent)
		
		/// <summary>This method handles all video events fired by one of the child controls</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
		{
			CDxMediaRecord dxRecord = null;
			
			//	Which event?
			switch(e.EventId)
			{
				case TmaxVideoCtrlEvents.Apply:

					OnApply();
					break;
					
				case TmaxVideoCtrlEvents.EditDesignationExtents:

					Debug.Assert(e.XmlDesignation != null);
					Debug.Assert(ReferenceEquals(e.XmlDesignation, m_xmlDesignation) == true);
					
					OnEditDesignationExtents();
					break;
					
				case TmaxVideoCtrlEvents.EditDesignationText:
				
					Debug.Assert(e.XmlDesignation != null);
					Debug.Assert(ReferenceEquals(e.XmlDesignation, m_xmlDesignation) == true);
					
					OnEditDesignationText();
					break;
					
				case TmaxVideoCtrlEvents.QueryCanContinue:
				
					e.QueryHandled = true;
					e.CanContinue = CanContinue(e.CheckDesignation, e.CheckLink);
					break;
					
				case TmaxVideoCtrlEvents.ScriptDesignationComplete:
				
					OnScriptDesignationComplete();
					break;
					
				case TmaxVideoCtrlEvents.ScriptLinkChanged:
				
					OnScriptLinkChanged(e.XmlLink);
					break;
					
				case TmaxVideoCtrlEvents.AddLink:
				
					Debug.Assert(e.XmlDesignation != null);
					Debug.Assert(ReferenceEquals(e.XmlDesignation, m_xmlDesignation) == true);
					
					OnAddLink(e.XmlDesignation, e.XmlLink);
					break;
					
				case TmaxVideoCtrlEvents.QueryLinkSourceDbId:
				
					Debug.Assert(e.LinkSourceMediaId != null);
					Debug.Assert(e.LinkSourceMediaId.Length > 0);
					
					if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(e.LinkSourceMediaId, true, false)) != null)
					{
						//	Should we switch to the source record?
						if(dxRecord.MediaType == TmaxMediaTypes.Scene)
						{
							dxRecord = ((CDxSecondary)dxRecord).GetSource();
							e.LinkSourceMediaId = dxRecord.GetBarcode(false);
						}
						else if(dxRecord.MediaType == TmaxMediaTypes.Link)
						{
							dxRecord = ((CDxQuaternary)dxRecord).GetSource();
							e.LinkSourceMediaId = dxRecord.GetBarcode(false);
						}
						
					}
					
					if(dxRecord != null)
					{
						//	Can the media be linked?
						switch(dxRecord.MediaType)
						{
							case TmaxMediaTypes.Page:
							case TmaxMediaTypes.Slide:

								e.LinkSourceDbId = dxRecord.GetUniqueId();
								break;

							case TmaxMediaTypes.Treatment:
							
								//	Don't permit split screen treatment links
								if(((CDxTertiary)dxRecord).SplitScreen == false)
								{
									e.LinkSourceDbId = dxRecord.GetUniqueId();
								}
								else
								{
									MessageBox.Show("Unable to link to split screen treatments", "Error",
													MessageBoxButtons.OK, MessageBoxIcon.Warning);
									e.LinkSourceDbId = "";
								}
								break;
								
							default:
							
								MessageBox.Show("Unable to link media of type: " + dxRecord.MediaType.ToString() + "\n\nThe link must reference a page, treatment, or slide.", "Error",
												 MessageBoxButtons.OK, MessageBoxIcon.Warning);
								e.LinkSourceDbId = "";
								break;
								
						}
					
					}
					else
					{
						MessageBox.Show("Unable to locate record for the specified link barcode: " + e.LinkSourceMediaId, "Error",
										MessageBoxButtons.OK, MessageBoxIcon.Warning);
						e.LinkSourceDbId = "";
					}
					
					e.QueryHandled = true;
					break;
					
				case TmaxVideoCtrlEvents.SetLink:
				
					//	Notify the tuner
					m_ctrlTuner.SetProperties(e.XmlLink);
					
					//	Update the local member
					m_xmlLink = e.XmlLink;
					
					//	Get the database record
					if(m_xmlLink != null)
					{
						if((m_dxLink = (CDxQuaternary)m_tmaxDatabase.GetRecordFromId(m_xmlLink.DatabaseId, false)) == null)
						{
							//MessageBox.Show("cant find db record for the link");
						}
					}
					else
					{
						m_dxLink = null;
					}
					break;
					
				case TmaxVideoCtrlEvents.QueryLinkDropBarcode:
				
					//	Initialize the return values
					e.QueryHandled = true;
					e.LinkDropBarcode = "";
					
					//	Is the user dragging registered media?
					if((m_eDragState == PaneDragStates.Records) &&
					   (m_tmaxDragData.SourceItems != null) &&
					   (m_tmaxDragData.SourceItems.Count == 1) &&
					   (m_tmaxDragData.SourceItems[0].GetMediaRecord() != null))
					{
						dxRecord = (CDxMediaRecord)(m_tmaxDragData.SourceItems[0].GetMediaRecord());
						
						//	Should we use the source record?
						if(dxRecord.MediaType == TmaxMediaTypes.Scene)
						{
							dxRecord = ((CDxSecondary)dxRecord).GetSource();
						}
						else if(dxRecord.MediaType == TmaxMediaTypes.Link)
						{
							dxRecord = ((CDxQuaternary)dxRecord).GetSource();
						}
						
						if(dxRecord != null)
						{
							//	Can the media be linked?
							switch(dxRecord.MediaType)
							{
								case TmaxMediaTypes.Page:
								case TmaxMediaTypes.Slide:

									e.LinkDropBarcode = dxRecord.GetBarcode(false);
									break;
								
								case TmaxMediaTypes.Treatment:
								
									//	Don't permit split screen treatment links
									if(((CDxTertiary)dxRecord).SplitScreen == false)
										e.LinkDropBarcode = dxRecord.GetBarcode(false);
									break;

							}// switch(dxRecord.MediaType)
						
						}// if(dxRecord != null)
						
					}
					break;
					
				default:
				
					break;
					
			}
		
		}// protected void OnTuneBarCommand(object objSender, FTI.Trialmax.Controls.CTmaxVideoTuneBarCtrl.TmaxTuneBarCommands eCommand)
	
		/// <summary>This method handles all events where the user requests editing of the extents for the active designation</summary>
		protected void OnEditDesignationExtents()
		{
			bool bModified = false;

			Debug.Assert(m_dxScript != null);
			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_dxTertiary != null);
			Debug.Assert(m_xmlDesignation != null);
			
			if(m_dxScript == null) return;
			if(m_dxScene == null) return;
			if(m_dxTertiary == null) return;
			if(m_xmlDesignation == null) return;
			
			if(m_dxTertiary.MediaType == TmaxMediaTypes.Designation)
			{
				CFAddDesignations EditDesigation = new CFAddDesignations();
				
				EditDesigation.PaneId = PaneId;
				m_tmaxEventSource.Attach(EditDesigation.EventSource);
				EditDesigation.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
				EditDesigation.Script = m_dxScript;
				EditDesigation.Scene = m_dxScene;
				EditDesigation.Deposition = m_dxTertiary.Secondary.Primary;
				EditDesigation.Database = m_tmaxDatabase;
				EditDesigation.EditExtents = true;
				
				bModified = (EditDesigation.ShowDialog() == DialogResult.OK);
			}
			else if(m_dxTertiary.MediaType == TmaxMediaTypes.Clip)
			{
				CFAddClips EditClip = new CFAddClips();
				
				EditClip.PaneId = PaneId;
				m_tmaxEventSource.Attach(EditClip.EventSource);
				EditClip.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
				EditClip.Script = m_dxScript;
				EditClip.Scene = m_dxScene;
				EditClip.Clip = m_dxTertiary;
				EditClip.XmlDesignation = m_xmlDesignation;
				EditClip.Database = m_tmaxDatabase;
				EditClip.Edit = true;
				
				bModified = (EditClip.ShowDialog() == DialogResult.OK);
			}
			else
			{
				Debug.Assert(false);
			}
			
			if(bModified == true)
			{
				m_ctrlTuner.SetTuneMode();
			}
			
		}// protected void OnEditDesignationExtents()
		
		/// <summary>This method handles all events where the user requests editing of the text for the active designation</summary>
		protected void OnEditDesignationText()
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
					//	Update the designation and scene records
					FireCommand(TmaxCommands.Update, new CTmaxItem(m_dxTertiary));
					FireCommand(TmaxCommands.Update, new CTmaxItem(m_dxScene));
					
					//	Save the designation
					m_xmlDesignation.ModifiedBy = m_tmaxDatabase.GetUserName(m_dxTertiary.ModifiedBy);
					m_xmlDesignation.ModifiedOn = m_dxTertiary.ModifiedOn.ToString();
					m_xmlDesignation.Save();
					
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnEditDesignationText", m_tmaxErrorBuilder.Message(ERROR_EDIT_TEXT_EX, m_xmlDesignation), Ex);
			}
			
		}// protected void OnEditDesignationText()
		
		/// <summary>This method handles all events where the user is requesting to apply changes</summary>
		protected void OnApply()
		{
			bool bError = false;
			
			//	Update the active objects
			if(m_xmlDesignation != null)
			{
				//	Is there an active link?
				if((m_xmlLink != null) && (IsModified(m_xmlLink, null) == true))
				{
					if(Update(m_xmlDesignation, m_xmlLink) == false)
						bError = true;
				}
				
				if(IsModified(m_xmlDesignation, null) == true)
				{
					if(Update(m_xmlDesignation, m_dxTertiary) == false)
						bError = true;
				}
				
				if((bError == true) && (m_dxScene != null))
					RefreshScene();			
			}
		
		}// protected void OnApply()
	
		/// <summary>This method handles video events requesting the addition of a new link</summary>
		/// <param name="xmlDesignation">The parent designation</param>
		/// <param name="xmlLink">The new link</param>
		protected void OnAddLink(CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		{
			CTmaxItem tmaxItem = null;
			
			Debug.Assert(xmlDesignation != null);
			Debug.Assert(xmlLink != null);
			Debug.Assert(m_dxTertiary != null);
			Debug.Assert(m_dxScript != null);
			Debug.Assert(m_dxScene != null);
			Debug.Assert(ReferenceEquals(xmlDesignation, m_xmlDesignation) == true);

			if(m_dxScene == null) return;
			if(xmlLink == null) return;
			
			//	Construct the item needed to add the link as a child of the designation	
			tmaxItem = new CTmaxItem(m_dxTertiary);
			tmaxItem.XmlDesignation = m_xmlDesignation;
			
			if(tmaxItem.SourceItems == null)
				tmaxItem.SourceItems = new CTmaxItems();
				
			tmaxItem.SourceItems.Add(new CTmaxItem());
			tmaxItem.SourceItems[0].XmlLink = xmlLink;
			
			FireCommand(TmaxCommands.Add, tmaxItem);
		
		}// protected void OnAddLink(CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The items that have been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	Iterate the collection of parent items
			foreach(CTmaxItem O in tmaxItems)
			{
				try
				{
					//	Don't bother processing pick list items
					if(O.DataType == TmaxDataTypes.PickItem)
						break;
						
					OnDeleted(O);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_DELETED_EX), Ex);
				}
				
			}

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			Debug.Assert(tmaxItem != null);
			if(tmaxItem == null) return;
			if(tmaxItem.GetMediaRecord() == null) return;

			//	What type of media has been updated?
			switch(tmaxItem.GetMediaRecord().GetMediaType())
			{
				case TmaxMediaTypes.Scene:
				
					//	Is this our active scene?
					if((m_dxScene != null) && 
					   (ReferenceEquals(m_dxScene, tmaxItem.GetMediaRecord()) == true))
					{
						RefreshScene();
					}
					break;
					
				case TmaxMediaTypes.Designation:

					//	Is this our active tertiary record?
					if((m_dxTertiary != null) && 
					   (ReferenceEquals(m_dxTertiary, tmaxItem.GetMediaRecord()) == true))
					{
						//	Our active XML designation may no longer be valid if
						//	the extents have been modified by another pane or form
						if((m_xmlDesignation != null) && (m_dxScene != null) && (m_dxTertiary != null))
						{
							//	Have the extents changed
							if((m_xmlDesignation.FirstPL != m_dxTertiary.GetExtent().StartPL) ||
							   (m_xmlDesignation.LastPL != m_dxTertiary.GetExtent().StopPL))
							{
								//	Reset the scene
								SetScene(m_dxScene, false);
							}
							else
							{
								//	The split text option may have been modified by the
								//	tree or the script builder pane
								if(m_dxTertiary.ScrollText != m_xmlDesignation.ScrollText)
									m_xmlDesignation.ScrollText = m_dxTertiary.ScrollText;
									
								//	Just do a normal refresh
								RefreshScene();
							}
							
						}
						else
						{
							RefreshScene();
						}
					
					}
					break;
					
				case TmaxMediaTypes.Clip:
				
					//	Is this our active tertiary record?
					if((m_dxTertiary != null) && 
						(ReferenceEquals(m_dxTertiary, tmaxItem.GetMediaRecord()) == true))
					{
						RefreshScene();
					}
					break;
					
				case TmaxMediaTypes.Link:
				
					//	Is this our active link?
					if((m_dxLink != null) && 
					   (ReferenceEquals(m_dxLink, tmaxItem.GetMediaRecord()) == true))
					{
						RefreshScene();
					}
					break;
					
				default:
				
					break;
					
			}
					
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			Debug.Assert(tmaxItem != null);
			if(tmaxItem == null) return;
			if(tmaxItem.DataType == TmaxDataTypes.CaseCode) return;
			if(tmaxItem.GetMediaRecord() == null) return;
				
			//	What type of media has been updated?
			switch(tmaxItem.GetMediaRecord().GetMediaType())
			{
				case TmaxMediaTypes.Script:
				
					//	Have the scenes in the active script been reordered?
					if((m_dxScript != null) && (m_dxScenes != null) &&
					   (ReferenceEquals(m_dxScript, tmaxItem.GetMediaRecord()) == true))
					{
						m_dxScenes.Sort();
						m_ctrlScripts.RefreshScenes(false);
					
					}
					
					break;
					
				case TmaxMediaTypes.Designation:
				case TmaxMediaTypes.Clip:
				
					//	Is this our active tertiary record?
					if((m_dxTertiary != null) && 
					   (ReferenceEquals(m_dxTertiary, tmaxItem.GetMediaRecord()) == true))
					{
						RefreshScene();
					}
					break;
					
				default:
				
					break;
					
			}
					
		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		///<summary>This method is called when the active scene or its source is updated by the user</summary>
		public void RefreshScene()
		{
			//	Notify each of the child controls
			if((m_ctrlVideoProps != null) && (m_ctrlVideoProps.IsDisposed == false))
			{
				if(m_xmlDesignation != null)
					m_ctrlVideoProps.OnAttributesChanged(m_xmlDesignation);
				if(m_xmlLink != null)
					m_ctrlVideoProps.OnAttributesChanged(m_xmlLink);
			}
			
			if((m_ctrlTuner != null) && (m_ctrlTuner.IsDisposed  == false))
			{
				if(m_xmlDesignation != null)
					m_ctrlTuner.OnAttributesChanged(m_xmlDesignation);
				if(m_xmlLink != null)
					m_ctrlTuner.OnAttributesChanged(m_xmlLink);
			}
			
			if((m_ctrlLinks != null) && (m_ctrlLinks.IsDisposed == false))
			{
				if(m_xmlDesignation != null)
					m_ctrlLinks.OnAttributesChanged(m_xmlDesignation);
				if(m_xmlLink != null)
					m_ctrlLinks.OnAttributesChanged(m_xmlLink);
			}
			
		}// public void RefreshScene()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			base.SetErrorStrings();
			
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to set the active scene. No source record could be found. scene = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to set the active scene. No extents record could be found. tertiary = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to set the active scene. Unable to retrieve XML designation filename. tertiary = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to set the active scene. Unable to retrieve the path to the video file. tertiary = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the video file for the designation. \nfilename = %1\ndesignation = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the text associated with the active designation: designation = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to process the delete notification");
		}

		/// <summary>This method is called to populate scripts combo box</summary>
		protected void FillScripts()
		{
			//	Make sure the existing scripts are flushed
			m_dxScripts.Clear();
			
			//	Don't bother if no database
			if(m_tmaxDatabase == null) return;
			
			//	Get all the scripts from the database
			foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
			{
				//	Is this a script?
				if(dxPrimary.MediaType == TmaxMediaTypes.Script)
				{
					m_dxScripts.AddList(dxPrimary);
				}
						
			}
				
			//	Give the script box control the collection of scripts in the database
			if((m_ctrlScripts != null) && (m_ctrlScripts.IsDisposed == false))
			{
				m_ctrlScripts.SetScripts(m_dxScripts);
			}
		
		}// protected void FillScripts()

		/// <summary>This method is called to build a collection of XML designations from the current scenes collection</summary>
		///	<returns>A collection of designations that defines a script</returns>
		protected CXmlDesignations GetDesignations()
		{
			CXmlDesignations xmlDesignations = null;
			CXmlDesignation  xmlDesignation = null;
			
			//	Do we have any scenes to play?
			if((m_dxScenes != null) && (m_dxScenes.Count > 0))
			{
				//	Allocate a new collection
				xmlDesignations = new CXmlDesignations();
				
				//	Create a designation for each scene
				foreach(CDxSecondary dxScene in m_dxScenes)
				{
					if((xmlDesignation = GetDesignation(dxScene)) != null)
						xmlDesignations.Add(xmlDesignation);
				}
				
				//	Did we get any designations?
				if(xmlDesignations.Count == 0)
					xmlDesignations = null;			
			}
			
			return xmlDesignations;
		
		}// protected CXmlDesignations GetScriptFromScenes()

		/// <summary>This method is called to get the XML designation associated with the scene</summary>
		/// <param name="dxScene">The scene used to retrieve the designation</param>
		///	<returns>An eqivalent XML designationi</returns>
		protected CXmlDesignation GetDesignation(CDxSecondary dxScene)
		{
			CXmlDesignation xmlDesignation = null;
			CDxTertiary		dxTertiary = null;
			string			strFileSpec = "";
			
			Debug.Assert((dxScene.SourceType == TmaxMediaTypes.Designation) || (dxScene.SourceType == TmaxMediaTypes.Clip));
			if((dxScene.SourceType != TmaxMediaTypes.Designation) && 
			   (dxScene.SourceType != TmaxMediaTypes.Clip)) return null;
				   
			//	Were we unable to get the source?
			if(dxScene.GetSource() == null)
			{
				m_tmaxEventSource.FireError(this, "GetDesignation", m_tmaxErrorBuilder.Message(ERROR_NO_SCENE_SOURCE, dxScene));
				return null;
			}

			dxTertiary = (CDxTertiary)dxScene.GetSource();
			if(dxTertiary.GetExtent() == null)
			{
				m_tmaxEventSource.FireError(this, "GetDesignation", m_tmaxErrorBuilder.Message(ERROR_NO_EXTENTS, dxTertiary));
				return null;
			}

			//	Get the XML designation for the tertiary record
			if((xmlDesignation = m_tmaxDatabase.GetXmlDesignation(dxTertiary, true, true,true)) != null)
			{
				m_tmaxEventSource.Attach(xmlDesignation.EventSource);
			}
			else
			{
				m_tmaxEventSource.FireError(this, "GetDesignation", m_tmaxErrorBuilder.Message(ERROR_NO_XML_DESIGNATION, dxTertiary));
				return null;
			}	
				
			//	Get the video file
			Debug.Assert(dxTertiary.Secondary != null);
			strFileSpec = m_tmaxDatabase.GetFileSpec(dxTertiary.Secondary);
			if((strFileSpec == null) || (strFileSpec.Length == 0))
			{
				m_tmaxEventSource.FireError(this, "SetScene", m_tmaxErrorBuilder.Message(ERROR_NO_VIDEO_FILE, dxTertiary));
				return null;
			}
				
			//	Check to see if the video exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				//	Set the simulation text
				m_ctrlTuner.SimulationText = (System.IO.Path.GetFileName(strFileSpec) + " not found");
				
				//m_tmaxEventSource.FireError(this, "SetScene", m_tmaxErrorBuilder.Message(ERROR_VIDEO_NOT_FOUND, strFileSpec, xmlDesignation.Name));
				//return null;
			}
				
			// Store the path to the recording
			xmlDesignation.Recording = strFileSpec;
			
			//	Maintain a link back to the scene
			xmlDesignation.UserData = dxScene;
			
			return xmlDesignation;
		
		}// protected protected CXmlDesignations GetDesignation(CDxSecondary dxScene)

		/// <summary>This function is called when the Active property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			//	Is this pane being activated by the user?
			if(m_bPaneVisible == true)
			{
				//	Is there a record waiting for activation?
				if(m_tmaxActivate != null)
				{
					Activate(m_tmaxActivate);
				}
			}
			else
			{
				//	Make sure we are not playing anything
				StopScript();
				m_ctrlTuner.Player.Stop();
			}

		}// protected override void OnPaneVisibleChanged()
		
		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			// Don't bother if not valid media
			if(tmaxItem.GetMediaRecord() == null) return false;
			
			//	Don't bother if playing a script
			if(m_iScriptScene >= 0) return false;
			
			//	Only process scripts, scenes and links
			switch(tmaxItem.MediaType)
			{
				case TmaxMediaTypes.Script:
				case TmaxMediaTypes.Scene:
				case TmaxMediaTypes.Link:
				
					m_tmaxActivate = tmaxItem;
					
					break;
					
				default:
				
					//	Don't bother
					return false;
			}

			//	Go ahead and activate this record if the pane is visible
			if(PaneVisible == true)
			{
				Activate(m_tmaxActivate);
			}
			
			return true;
						
		}// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called to activate the specified record</summary>
		/// <param name="tmaxActivate">The item to be activated</param>
		/// <returns>true if successful</returns>
		public bool Activate(CTmaxItem tmaxItem)
		{
			CDxPrimary		dxScript = null;
			CDxSecondary	dxScene = null;
			CDxQuaternary	dxLink = null;
			bool			bScriptChanged = false;
			
			Debug.Assert(tmaxItem != null);
			Debug.Assert(tmaxItem.GetMediaRecord() != null);
			
			//	Make sure it's ok to continue
			if(CanContinue() == false) return false;
			
			//	Get the record interfaces
			if(tmaxItem.MediaType == TmaxMediaTypes.Script)
			{
				dxScript = (CDxPrimary)tmaxItem.GetMediaRecord();
			}
			else if(tmaxItem.MediaType == TmaxMediaTypes.Scene)
			{
				dxScene = (CDxSecondary)tmaxItem.GetMediaRecord();
				dxScript = dxScene.Primary;
				Debug.Assert(dxScript != null);
			}
			else if(tmaxItem.MediaType == TmaxMediaTypes.Link)
			{
				dxLink   = (CDxQuaternary)tmaxItem.GetMediaRecord();
				
				Debug.Assert(tmaxItem.ParentItem != null);
				Debug.Assert(tmaxItem.ParentItem.GetMediaRecord() != null);
				Debug.Assert(tmaxItem.ParentItem.MediaType == TmaxMediaTypes.Scene);
				
				dxScene  = (CDxSecondary)tmaxItem.ParentItem.GetMediaRecord();
				Debug.Assert(dxScene != null);
				dxScript = dxScene.Primary;
				Debug.Assert(dxScript != null);
			}
			else
			{
				//	This shouldn't happen
				Debug.Assert(dxScript != null);
				return false;
			}
			
			//	Do we have to change the script?
			if((m_dxScript == null) || 
			   (ReferenceEquals(dxScript, m_dxScript) == false))
			{
				SetScript(dxScript, true);
				bScriptChanged = true;
			}
			
			//	Is the user selecting a tuneable scene?
			if(dxScene != null) 
			{
				if((dxScene.SourceType == TmaxMediaTypes.Designation) ||
				   (dxScene.SourceType == TmaxMediaTypes.Clip))
				{
				}
				else
				{
					//	Force selection of first tuneable scene if the script
					//	has changed
					dxScene = null;
				}
				
			}
			
			//	Should we automatically select the first scene?
			if((dxScene == null) && (bScriptChanged == true) && (m_dxScenes.Count > 0))
			{
				dxScene = m_dxScenes[0];
			}
			
			//	Should we set the active scene?
			if((dxScene != null) && (ReferenceEquals(dxScene, m_dxScene) == false))
			{
				SetScene(dxScene, true);
			}
			
			//	Should we set the active link?
			if((dxLink != null) && (ReferenceEquals(dxLink, m_dxLink) == false))
			{
				SetLink(dxLink, true);
			}
			
			//	Clear the pending item
			m_tmaxActivate = null;
			
			return true;
		
		}// public bool Activate(CDxMediaRecord dxRecord)
			
		/// <summary>
		/// This method is called by the application when a new search result has been activated
		/// </summary>
		/// <param name="tmaxResult">The search result to be activated</param>
		public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{
			CTmaxItem tmaxItem = null;
			
			//	Don't bother if not visible
			if(PaneVisible == false) return;
			
			//	Don't bother if this is not a scene
			if(tmaxResult.IScene == null) return;
			
			try
			{
				//	Activate this record
				tmaxItem = new CTmaxItem(tmaxResult.IScene);
				Activate(tmaxItem, TmaxAppPanes.Results);
			}
			catch
			{
			}
		
		}// public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		
		/// <summary>
		/// This method is called by the application to initialize the pane
		/// </summary>
		/// <returns>true if successful</returns>
		/// <remarks>Derived classes should override for custom runtime initialization</remarks>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class processing first
			if(base.Initialize(xmlINI) == false)
				return false;
				
			//	Get the preferences from the ini file
			if(xmlINI.SetSection(m_strPaneName) == true)
			{
				if(m_ctrlTuner != null)
					m_ctrlTuner.PreviewPeriod = xmlINI.ReadDouble(KEY_PREVIEW_PERIOD, 2.0);
			}
			
			return true;
		}
		
		/// <summary>
		/// This method is called by the application when it is about to terminate
		/// </summary>
		/// <remarks>Derived classes should override for custom shutdown</remarks>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Save the preferences to the ini file
			if(xmlINI.SetSection(m_strPaneName) == true)
			{
				if(m_ctrlTuner != null)
					xmlINI.Write(KEY_PREVIEW_PERIOD, m_ctrlTuner.PreviewPeriod);
			}

		}
		
		/// <summary>This method is called to see if it's ok to continue to operation in progress</summary>
		/// <param name="bDesignation">true to check the active designation</param>
		/// <param name="bLink">true to check the active link</param>
		/// <returns>true if ok to continue</returns>
		protected bool CanContinue(bool bDesignation, bool bLink)
		{
			bool		bContinue = true;
			bool		bModified = false;
			ArrayList	aModifications = new ArrayList();

			//	Do we need to check the active designation?
			if((bDesignation == true) && (m_xmlDesignation != null))
			{
				if(IsModified(m_xmlDesignation, aModifications) == true)
					bModified = true;
			}
			
			//	Do we need to check the active link?
			if((bLink == true) && (m_xmlLink != null))
			{
				if(IsModified(m_xmlLink, aModifications) == true)
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
			
		}// protected bool CanContinue(bool bDesignation, bool bLink)
		
		/// <summary>This method is called to see if it's ok to continue to operation in progress</summary>
		/// <returns>true if ok to continue</returns>
		protected bool CanContinue()
		{
			return CanContinue(true,true);	
		}	
		
		/// <summary>This method is called to determine if the control properties match those of the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be compared</param>
		///	<param name="aModifications">Array in which to store a list of the modifications</param>
		/// <returns>true if there are modifications</returns>
		protected bool IsModified(CXmlDesignation xmlDesignation, ArrayList aModifications)
		{
			bool bModified = false;
			
			//	Do we have an active designation?
			if(xmlDesignation == null) return false;

			//	Check each of the controls that edit designation properties
			if(m_ctrlTuner.IsModified(aModifications) == true)
				bModified = true;
				
			if(m_ctrlVideoProps.IsModified(aModifications) == true)
				bModified = true;
			
			return bModified;
		
		}// protected bool IsModified(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to determine if the control properties match those of the specified link</summary>
		/// <param name="xmlDesignation">The link to be compared</param>
		///	<param name="aModifications">Array in which to store a list of the modifications</param>
		/// <returns>true if there are modifications</returns>
		protected bool IsModified(CXmlLink xmlLink, ArrayList aModifications)
		{
			bool bModified = false;
			
			//	Do we have an active link?
			if(xmlLink == null) return false;

			//	Check each of the controls that edit link properties
			if(m_ctrlTuner.IsModified(xmlLink, aModifications) == true)
				bModified = true;

			if(m_ctrlLinks.IsModified(xmlLink, aModifications) == true)
				bModified = true;
			
			return bModified;
		
		}// protected bool IsModified(CXmlLink xmlLink)
		
		/// <summary>This method is called to update the specified designation</summary>
		/// <param name="xmlDesignation">The XML designation to be updated</param>
		/// <param name="dxDesignation">The designation's record exchange interface</param>
		/// <returns>true if successful</returns>
		protected bool Update(CXmlDesignation xmlDesignation, CDxTertiary dxDesignation)
		{
			Debug.Assert(dxDesignation != null);
			if(dxDesignation == null) return false;
			
			//	Make sure we have all the changes
			m_ctrlTuner.SetAttributes(xmlDesignation);
			m_ctrlVideoProps.SetAttributes(xmlDesignation);
					
			//	Update the designation exchange object
			dxDesignation.SetProperties(xmlDesignation);
			
			//	If the user tuned the clip the name may have changed
			if(dxDesignation.MediaType == TmaxMediaTypes.Clip)
			{
				if(dxDesignation.Secondary.Name.Length > 0)
					xmlDesignation.SetNameFromExtents(dxDesignation.Secondary.Name);
				else
					xmlDesignation.SetNameFromExtents(dxDesignation.Secondary.Filename);
				
				dxDesignation.Name = xmlDesignation.Name;
				
			}// if(dxDesignation.MediaType == TmaxMediaTypes.Clip)
			
			//	Fire the command
			if(FireCommand(TmaxCommands.Update, new CTmaxItem(dxDesignation)) == true)
			{
				return xmlDesignation.Save();
			}
			else
			{			
				return false;
			}
		
		}// protected bool Update(CXmlDesignation xmlDesignation, CDxTertiary dxDesignation)
			
		/// <summary>This method is called to update the specified link</summary>
		/// <param name="xmlDesignation">The XML designation that owns the link</param>
		/// <param name="dxDesignation">The XML link to be updated</param>
		/// <returns>true if successful</returns>
		protected bool Update(CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		{
			CDxQuaternary	dxLink = null;
			CDxQuaternaries	dxSorted = null;
			CDxMediaRecord	dxSource = null;
			CTmaxItem		tmaxItem    = null;
			bool			bSuccessful = false;
			bool			bReorder = false;
			
			//	Get the database record associated with this link
			if((dxLink = (CDxQuaternary)m_tmaxDatabase.GetRecordFromId(xmlLink.DatabaseId, false)) == null)
			{
				Debug.Assert(dxLink != null);
				return false;
			}
			
			//	Get the database record associated with this link's source media
			if(xmlLink.Hide == false)
			{
				if((dxSource = m_tmaxDatabase.GetRecordFromId(xmlLink.SourceDbId, false)) == null)
				{
					Debug.Assert(dxSource != null);
					return false;
				}
			}
			
			while(bSuccessful == false)
			{
				//	Get the tuner modifications
				if(m_ctrlTuner.SetAttributes(xmlLink) == false)
					break;
					
				//	Get the links control modifications
				if(m_ctrlLinks.SetAttributes(xmlLink) == false)
					break;

				//	Update the record properties
				dxLink.SetProperties(xmlLink);
				
				//	Do we need to reorder the collection
				for(int i = 1; i < dxLink.Tertiary.Quaternaries.Count; i++)
				{
					if(dxLink.Tertiary.Quaternaries[i - 1].Start > 
					   dxLink.Tertiary.Quaternaries[i].Start)
					{
						bReorder = true;
						break;
					}
					
				}
				
				//	Update the database
				if(bReorder == true)
				{
					//	Create the event item required for the update
					tmaxItem = new CTmaxItem(dxLink.Tertiary);
					
					//	Create a collection we can use to sort the records
					dxSorted = new CDxQuaternaries();
					if(dxSorted.Comparer == null)
					{
						dxSorted.Comparer = new CTmaxRecordSorter();
						dxSorted.KeepSorted = false;
					}
					
					//	Put in correct sort order
					foreach(CDxQuaternary O in dxLink.Tertiary.Quaternaries)
						dxSorted.AddList(O);
					
					dxSorted.Sort();
					
					//	Now build the event item collection to define the new sort order
					foreach(CDxQuaternary O in dxSorted)
						tmaxItem.SubItems.Add(new CTmaxItem(O));
					
					dxSorted.Clear();
					dxSorted = null;
					
					//	Fire the command
					if(FireCommand(TmaxCommands.Reorder, tmaxItem) == false)
						break;
				}
				else
				{
					//	Create the event item required for the update
					tmaxItem = new CTmaxItem(dxLink);
					
					//	Fire the command
					if(FireCommand(TmaxCommands.Update, tmaxItem) == false)
						break;
				}
					
				//	Update went ok
				bSuccessful = true;
				
			}
			
			//	We're we successful?
			if(bSuccessful == true)
			{
				xmlDesignation.Save();
			}
			else
			{
				//	Make sure we stay in sync
				dxLink.SetAttributes(xmlLink);
			}	
			
			return bSuccessful;
		
		}// protected bool Update(CXmlDesignation xmlDesignation, CXmlLink xmlLink)
			
		/// <summary>This method is called to unload the child controls</summary>
		protected void Unload()
		{
			try
			{
				//	Reset the local members
				m_dxScripts.Clear();
				m_dxScenes.Clear();
				
				m_dxScript = null;
				m_dxScene = null;
				m_dxLink = null;
				m_dxTertiary = null;
				m_iScriptScene = -1;
				m_tmaxActivate = null;
				m_xmlDesignation = null;
				m_xmlLink = null;
				
				ResetScene(true);
			
				//	Notify the controls
				if((m_ctrlScripts != null) && (m_ctrlScripts.IsDisposed == false))
				{
					m_ctrlScripts.SetScripts(null);
				}
			
			}
			catch
			{
			}
			
		}// protected void Unload()

		/// <summary>This method is called to reset the local references associated with the active scene</summary>
		protected void ResetScene(bool bNotify)
		{
			try
			{
				//	Reset the local members
				m_dxScene    = null;
				m_dxTertiary = null;
				m_xmlLink    = null;
				m_dxLink     = null;
				
				//	Close the XML file
				if(m_xmlDesignation != null)
				{
					m_xmlDesignation.Close();
					m_xmlDesignation = null;
				}
			
				//	Notify the controls
				if(bNotify == true)
				{
					if((m_ctrlVideoProps != null) && (m_ctrlVideoProps.IsDisposed == false))
					{
						m_ctrlVideoProps.SetProperties("", null);
					}
					if((m_ctrlTuner != null) && (m_ctrlTuner.IsDisposed == false))
					{
						m_ctrlTuner.SetProperties("", null);
					}
					if((m_ctrlLinks != null) && (m_ctrlLinks.IsDisposed == false))
					{
						m_ctrlLinks.SetProperties("", null);
					}
				}
			
			}
			catch
			{
			}
			
		}// protected void ResetScene()

		/// <summary>This method handles events fired by the Scripts combo box when the user selects a new scene</summary>
		/// <param name="dxScene">The scene to be activated</param>
		/// <param name="bSynchronize">True to synchronize the script selection control</param>
		/// <returns>true if successful</returns>
		protected bool SetScene(CDxSecondary dxScene, bool bSynchronize)
		{
			CXmlDesignation xmlDesignation = null;
			
			//	Are we clearing the current scene?
			if(dxScene == null)
			{
				ResetScene(true);
			}
			else
			{
				//	Get the designation for this scene
				if((xmlDesignation = GetDesignation(dxScene)) == null)
				{
					return false;
				}

				//	Clear the current scene
				ResetScene(false);

				//	Update the local reference
				m_dxScene = dxScene;
				m_dxTertiary = (CDxTertiary)m_dxScene.GetSource();
				m_xmlDesignation = xmlDesignation;
				
				//	Notify the child controls
				if((m_ctrlVideoProps != null) && (m_ctrlVideoProps.IsDisposed == false))
				{
					m_ctrlVideoProps.SetProperties(xmlDesignation.Recording, xmlDesignation);
				}
				if((m_ctrlLinks != null) && (m_ctrlLinks.IsDisposed == false))
				{
					m_ctrlLinks.SetProperties(xmlDesignation.Recording, xmlDesignation);
				}
				
				//	NOTE:	Always load the tuner last because it's the one that's
				//			going to be firing realtime events if we are playing a script
				if((m_ctrlTuner != null) && (m_ctrlTuner.IsDisposed == false))
				{
					m_ctrlTuner.SetProperties(xmlDesignation.Recording, xmlDesignation);
				}
			
			}// if(dxScene == null)
			
			//	Should we notify the script selection control?
			if(bSynchronize == true)
			{
				if((m_ctrlScripts != null) && (m_ctrlScripts.IsDisposed == false))
				{
					m_ctrlScripts.SetScene(dxScene, true);
				}
				
			}
			
			return true;

		}// protected void SetScene(CDxSecondary dxScene, bool bSynchronize)
		
		/// <summary>This method is called to activate the specified link</summary>
		/// <param name="dxLink">The link to be activated</param>
		/// <param name="bSynchronize">True to synchronize the script selection control</param>
		/// <returns>true if successful</returns>
		protected bool SetLink(CDxQuaternary dxLink, bool bSynchronize)
		{
			CXmlLink xmlLink = null;

			//	We should have an active designation
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return false;
			
			if(dxLink != null)
			{
				xmlLink = m_xmlDesignation.Links.Find(m_tmaxDatabase.GetUniqueId(dxLink));
			}

			//	Notify the links control
			m_ctrlLinks.SetLink(xmlLink);			
			
			return true;

		}// protected void SetScene(CDxSecondary dxScene, bool bSynchronize)
		
		/// <summary>This method handles events fired by the Scripts combo box when the user selects a new scene</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="objScript">The current script selection</param>
		/// <param name="objScene">The new scene selection</param>
		protected void OnSceneChanged(object objSender, object objScript, object objScene)
		{
			Debug.Assert(objScript != null);
			Debug.Assert(ReferenceEquals(objScript, m_dxScript) == true);
		
			//	Don't bother if playing a script
			if(m_iScriptScene >= 0) return;
			
			if(CanContinue() == true)
			{
				//	Set the scene
				SetScene((CDxSecondary)objScene, false);
			}
			else
			{
				m_ctrlScripts.SetScene(m_dxScene, true);
			}
		
		}// protected void OnSceneChanged(object objSender, object objScript, object objScene)
		
		/// <summary>This method is called to set the active script</summary>
		/// <param name="dxScript">The script to be activated</param>
		/// <param name="bSynchronize">True to synchronize the script selection control</param>
		protected void SetScript(CDxPrimary dxScript, bool bSynchronize)
		{
			//	Reset the local members
			m_dxScenes.Clear();
			m_dxScript = null;
			ResetScene(true);
			
			//	Do we have a new script?
			if(dxScript != null)
			{
				Debug.Assert(m_dxScripts.Contains(dxScript) == true);
				
				//	Update the local reference
				m_dxScript = dxScript;
				
				//	Do we need to fill the child collection?
				if((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
					dxScript.Fill();
					
				//	Fill the list of scenes
				if((dxScript.Secondaries != null) && (dxScript.Secondaries.Count > 0))
				{
					foreach(CDxSecondary O in dxScript.Secondaries)
					{
						if((O.SourceType == TmaxMediaTypes.Clip) ||
						   (O.SourceType == TmaxMediaTypes.Designation))
						{
							m_dxScenes.AddList(O);
						}
						
					}
				
				}// if((dxScript.Secondaries != null) && (dxScript.Secondaries.Count > 0))
			
			}
			
			//	Should we notify the script selection control?
			if(bSynchronize == true)
			{
				if((m_ctrlScripts != null) && (m_ctrlScripts.IsDisposed == false))
				{
					m_ctrlScripts.SetScript(m_dxScript, true);
				}
				
			}

			//	Give the script selector the new list of scenes
			m_ctrlScripts.SetScenes(m_dxScenes);
		
		}// protected void SetScript(CDxPrimary dxScript, bool bSynchronize)
		
		/// <summary>This method handles events fired by the Scripts combo box when the user selects a new script</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="objScript">The new script selection</param>
		protected void OnScriptChanged(object objSender, object objScript)
		{
			//	Don't bother if playing a script
			if(m_iScriptScene >= 0) return;
			
			if(CanContinue() == true)
			{
				//	Set the new script
				SetScript((CDxPrimary)objScript, false);
				
				//	Select the first scene
				if(m_dxScenes.Count > 0)
					SetScene(m_dxScenes[0], true);
			}
			else
			{
				m_ctrlScripts.SetScript(m_dxScript, true);
				m_ctrlScripts.SetScenes(m_dxScenes);
				m_ctrlScripts.SetScene(m_dxScene, true);
			}
		
		}// protected void OnScriptChanged(object objSender, object objScript)
		
		/// <summary>This method handles events fired by the Scripts combo box when the user wants to play the script</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="objScript">The current script selection</param>
		/// <param name="objScript">The scene to start the playback from</param>
		/// <param name="bPlayToEnd">True to play to the end of the script</param>
		protected void OnScriptsCmdPlay(object objSender, object objScript, object objScene, bool bPlayToEnd)
		{
			//	Are we canceling playback?
			if(objScript == null)
			{
				StopScript();
			}
			else
			{
				Debug.Assert(ReferenceEquals(objScript, m_dxScript) == true);
				if(ReferenceEquals(objScript, m_dxScript) == false) return;
				
				StartScript((CDxSecondary)objScene, bPlayToEnd);
			}
		
		}// protected void OnScriptsCmdPlay(object objSender, object objScript, object objScene, bool bPlayToEnd)

		/// <summary>This method is called to start playing the active script</summary>
		/// <param name="dxScene">The scene to start the playback from</param>
		/// <param name="bPlayToEnd">True to play to the end of the script</param>
		protected void StartScript(CDxSecondary dxScene, bool bPlayToEnd)
		{
			//	Make sure we have a collection of scenes
			if((m_dxScenes == null) || (m_dxScenes.Count == 0)) return;
			
			//	Make sure it's ok to run the script
			if(CanContinue() == false)
			{
				m_ctrlScripts.StopScript();
				return;
			}
			
			//	Are we supposed to start with a specific scene?
			if(dxScene != null)
			{
				//	Locate the specified scene
				if((m_iScriptScene = m_dxScenes.IndexOf(dxScene)) < 0)
				{
					if(MessageBox.Show("Unable to start playback at the specified scene. Playback will start at the beginning.", "Error",
					   MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
					{
						m_iScriptScene = 0;
					}
					
				}
			
			}// if(dxScene != null)
			else
			{
				m_iScriptScene = 0;
			}
			
			if((m_iScriptScene < 0) || (m_iScriptScene >= m_dxScenes.Count)) return;
						
			//	Put the video controls in script playback mode
			m_ctrlScripts.StartScript();
			m_ctrlVideoProps.StartScript();
			m_ctrlLinks.StartScript();
			m_ctrlTuner.StartScript();		
			
			//	Save this flag to control playback
			m_bScriptToEnd = bPlayToEnd;
			
			//	Now set the playback scene
			SetScene(m_dxScenes[m_iScriptScene], false);	
		
		}// protected void StartScript(CDxSecondary dxScene, bool bPlayToEnd)

		/// <summary>This method is called to stop playing the active script</summary>
		protected void StopScript()
		{
			//	Don't bother if not active
			if(m_iScriptScene < 0) return;

			m_iScriptScene = -1;
			
			//	Stop the playback
			m_ctrlTuner.StopScript();			
			m_ctrlVideoProps.StopScript();
			m_ctrlLinks.StopScript();
			m_ctrlScripts.StopScript();
			
		}// protected void StopScript()

		/// <summary>This method handles events fired by the player when the active link changes</summary>
		protected void OnScriptLinkChanged(CXmlLink xmlLink)
		{
			//	Notify the links control
			m_ctrlLinks.SetLink(xmlLink);			
		}
		
		/// <summary>This method handles events fired by the player when the designation is complete</summary>
		protected void OnScriptDesignationComplete()
		{
			//	Have we canceled the playback
			if(m_iScriptScene < 0) return;
			
			//	Go to the next scene
			m_iScriptScene++;
			
			//	Should we keep playing?
			if((m_bScriptToEnd == true) && (m_iScriptScene < m_dxScenes.Count))
			{
				SetScene(m_dxScenes[m_iScriptScene], true);
			}
			else
			{
				StopScript();
			}
		
		}// protected void OnScriptDesignationComplete()
		
		#endregion Protected Methods


	}// public class CTunePane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
