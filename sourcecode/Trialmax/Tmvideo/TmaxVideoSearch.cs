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

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class creates the view used to display the deposition transcript</summary>
	public class CTmaxVideoSearch : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_FIND_EX						= ERROR_BASE_VIEW_MAX + 1;
		protected const int ERROR_ON_APP_OPENED_EX				= ERROR_BASE_VIEW_MAX + 2;

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component container required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>The list of search results</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlResults;
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatus;

		/// <summary>Local member bound to Results property</summary>
		private CTmaxVideoResults m_tmaxResults = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoSearch() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			//	Initialize the list view control
			m_ctrlResults.Initialize(new CTmaxVideoResult());
		}

		/// <summary>This function is called by the application when the user opens a new XML script</summary>
		/// <param name="xmlScript">The new XML script</param>
		/// <returns>true if successful</returns>
		public override bool OnAppOpened(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			//	Perform the base class processing first
			//
			//	NOTE: This updates the m_xmlScript member
			if(base.OnAppOpened(xmlScript) == false) return false;
			
			try
			{
				//	Clear the active results
				if(m_tmaxResults != null)
				{
					m_tmaxResults.Clear();
					m_tmaxResults = null;
				}
				
				//	Clear the previous search selections
				if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.SearchOptions != null))
					m_tmaxAppOptions.SearchOptions.PreviousItems = null;
				
				//	Clear the results list
				if((m_ctrlResults != null) && (m_ctrlResults.IsDisposed == false))
					m_ctrlResults.Clear();
					
				//	Reset the status bar text
				SetStatus();
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppOpened", m_tmaxErrorBuilder.Message(ERROR_ON_APP_OPENED_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public override bool OnAppOpened(CXmlScript xmlScript)
		
		/// <summary>This method is called by the application when it processes a Delete command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoDelete(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			CTmaxVideoResults tmaxResults = null;
			
			//	Do we have any active results?
			if(m_tmaxResults == null) return true;
			if(m_tmaxResults.Count == 0) return true;
			
			//	Designations are stored in the SubItems collection
			if(tmaxItem.SubItems == null) return false;
			if(tmaxItem.SubItems.Count == 0) return false;
			
			//	Check each designation
			foreach(CTmaxItem O in tmaxItem.SubItems)
			{
				if(O.XmlDesignation == null) continue;
					
				//	Get the results that reference this designation
				if((tmaxResults = m_tmaxResults.FindAll(O.XmlDesignation)) != null)
				{
					//	Remove each result
					foreach(CTmaxVideoResult tmaxResult in tmaxResults)
					{
						try
						{
							m_tmaxResults.Remove(tmaxResult);
							m_ctrlResults.Remove(tmaxResult);
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireDiagnostic(this, "OnTmaxVideoDelete", Ex);
						}
							
					}// foreach(CTmaxVideoResult tmaxResult in tmaxResults)
						
					tmaxResults.Clear();
					tmaxResults = null;
					
				}// if((tmaxResults = m_tmaxResults.FindAll(O.XmlDesignation)) != null)
					
			}// foreach(CTmaxItem O in tmaxItem.SubItems)
			
			//	Update the status bar
			SetStatus();
					
			return true;
		
		}// public override bool OnTmaxVideoDelete(CTmaxItem tmaxItem, TmaxVideoViews eView)
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			CTmaxVideoResults	tmaxResults = null;
			CTmaxItem			tmaxAdded = null;
			
			//	Do we have any active results?
			if(m_tmaxResults == null) return true;
			if(m_tmaxResults.Count == 0) return true;
			
			//	Should be updating a designation
			if(tmaxItem == null) return false;
			if(tmaxItem.XmlDesignation == null) return false;
			
			//	Get the results that reference this designation
			if((tmaxResults = m_tmaxResults.FindAll(tmaxItem.XmlDesignation)) != null)
			{
				//	Remove each result
				foreach(CTmaxVideoResult tmaxResult in tmaxResults)
				{
					try
					{
						//	Make sure this result is still within the designation range
						if((tmaxResult.PL < tmaxItem.XmlDesignation.FirstPL) || (tmaxResult.PL > tmaxItem.XmlDesignation.LastPL))
						{
							//	See if this result fits one that was added as a result of the operation
							tmaxAdded = null;
							foreach(CTmaxItem O in tmaxItem.SubItems)
							{
								if(O.XmlDesignation != null)
								{
									if((tmaxResult.PL >= O.XmlDesignation.FirstPL) && (tmaxResult.PL <= O.XmlDesignation.LastPL))
									{
										tmaxAdded = O;
										break;
									}
								}
								
							}// foreach(CTmaxItem O in tmaxItem.SubItems)
							
							//	Did we find a new designation?
							if(tmaxAdded != null)
							{
								tmaxResult.XmlDesignation = tmaxAdded.XmlDesignation;
								m_ctrlResults.Update(tmaxResult);
							}
							else
							{
								//	Remove this result
								m_tmaxResults.Remove(tmaxResult);
								m_ctrlResults.Remove(tmaxResult);
							}
							
						}
						else
						{
							//	Update the text because the extents may have changed
							m_ctrlResults.Update(tmaxResult);
						}
					
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireDiagnostic(this, "OnTmaxVideoUpdate", Ex);
					}
								
				}// foreach(CTmaxVideoResult tmaxResult in tmaxResults)
						
				tmaxResults.Clear();
				tmaxResults = null;
					
			}// if((tmaxResults = m_tmaxResults.FindAll(O.XmlDesignation)) != null)

			return true;
		
		}// public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, int iId)
		
		/// <summary>This method is called by the application to initiate a search operation</summary>
		/// <param name="tmaxItem">The items to be searched</param>
		/// <param name="eView">The view initiating the request</param>
		/// <returns>true if successful</returns>
		public bool Find(CTmaxItems tmaxItems, TmaxVideoViews eView)
		{
			bool bSuccessful = false;
			
			try
			{
				Debug.Assert(m_xmlScript != null);
				Debug.Assert(m_tmaxAppOptions != null);
				if(m_xmlScript == null) return false;
				if(m_tmaxAppOptions == null) return false;
				
				//	Create and initialize a new search form
				CFTmaxVideoFind tmaxSearch = new CFTmaxVideoFind();
				
				tmaxSearch.XmlScript	 = m_xmlScript;
				tmaxSearch.View			 = eView;
				tmaxSearch.SearchOptions = m_tmaxAppOptions.SearchOptions;
				tmaxSearch.Selections    = tmaxItems;
				tmaxSearch.Results		 = new CTmaxVideoResults();
				
				//	Connect to the form's events
				m_tmaxEventSource.Attach(tmaxSearch.EventSource);
				m_tmaxEventSource.Attach(tmaxSearch.Results.EventSource);
				
				//	Did the user cancel the operation?
				if(tmaxSearch.ShowDialog() == DialogResult.OK)
				{
					//m_tmaxEventSource.InitElapsed();
					
					//	Clear the existing results
					if(m_tmaxResults != null)
					{
						m_tmaxResults.Clear();
						m_tmaxResults = null;
					}
					
					//	Set the new results
					if((m_tmaxResults = tmaxSearch.Results) != null)
					{					
						m_ctrlResults.Add(m_tmaxResults, true);
					}
					else
					{
						//	Make sure the list is empty
						m_ctrlResults.Clear();
					}
					
					//	Refresh the status bar
					SetStatus();
					
					//m_tmaxEventSource.FireElapsed(this, "Search", "Time to load results: ");
				
					//	Select the first result
					if(m_tmaxResults.Count > 0)
						m_ctrlResults.SetSelected(m_tmaxResults[0], false);
						
					bSuccessful = true;
				}
					
				tmaxSearch.Dispose();
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Find", m_tmaxErrorBuilder.Message(ERROR_FIND_EX), Ex);
			}

			return bSuccessful;
			
		}// public bool Find(CTmaxItems tmaxItems, int iSource)
		
		/// <summary>This method is called to select the next result in the list</summary>
		public bool FindNext()
		{
			CTmaxVideoResult	tmaxResult = null;
			bool				bSuccessful = false;
			
			try
			{
				//	Get the next search result in the list
				if((tmaxResult = GetNext()) != null)
				{
					m_ctrlResults.SetSelected(tmaxResult, false);
					bSuccessful = true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FindNext", Ex);
			}
			
			return bSuccessful;
		
		}// public bool FindNext()
		
		/// <summary>This method is called to determine if the FindNext command should be enabled</summary>
		/// <returns>true if the command should be enabled</returns>
		public bool CanFindNext()
		{
			return (GetNext() != null);

		}// public override bool CanFindNext()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called the when the window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Perform the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>Used by form designer to lay out child controls</summary> 
		protected override void InitializeComponent()
		{
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			this.m_ctrlResults = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlStatus = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
			this.SuspendLayout();
			// 
			// m_ctrlResults
			// 
			this.m_ctrlResults.AddTop = false;
			this.m_ctrlResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlResults.AutoResizeColumns = false;
			this.m_ctrlResults.ClearOnDblClick = false;
			this.m_ctrlResults.DisplayMode = 0;
			this.m_ctrlResults.HideSelection = false;
			this.m_ctrlResults.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlResults.MaxRows = 0;
			this.m_ctrlResults.Name = "m_ctrlResults";
			this.m_ctrlResults.OwnerImages = null;
			this.m_ctrlResults.SelectedIndex = -1;
			this.m_ctrlResults.ShowHeaders = true;
			this.m_ctrlResults.ShowImage = false;
			this.m_ctrlResults.Size = new System.Drawing.Size(152, 128);
			this.m_ctrlResults.TabIndex = 1;
			this.m_ctrlResults.DoubleClick += new System.EventHandler(this.OnDblClickResult);
			this.m_ctrlResults.SelectedIndexChanged += new System.EventHandler(this.OnSelChanged);
			// 
			// m_ctrlStatus
			// 
			this.m_ctrlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			appearance1.BackColor = System.Drawing.SystemColors.Control;
			appearance1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_ctrlStatus.Appearance = appearance1;
			this.m_ctrlStatus.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
			this.m_ctrlStatus.Dock = System.Windows.Forms.DockStyle.None;
			this.m_ctrlStatus.Location = new System.Drawing.Point(0, 128);
			this.m_ctrlStatus.Name = "m_ctrlStatus";
			this.m_ctrlStatus.Size = new System.Drawing.Size(150, 23);
			this.m_ctrlStatus.TabIndex = 2;
			this.m_ctrlStatus.WrapText = false;
			// 
			// CTmaxVideoSearch
			// 
			this.Controls.Add(this.m_ctrlStatus);
			this.Controls.Add(this.m_ctrlResults);
			this.Name = "CTmaxVideoSearch";
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
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to invoke the search engine.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the new script.");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods
		
		#region Private Methods
	
		/// <summary>This method is called to get the next result in the list</summary>
		private CTmaxVideoResult GetNext()
		{
			CTmaxVideoResult	tmaxResult = null;
			int					iIndex = 0;
			
			if(m_tmaxResults == null) return null;
			if(m_tmaxResults.Count == 0) return null;
			
			//	Get the current selection
			if((tmaxResult = (CTmaxVideoResult)(m_ctrlResults.GetSelected())) != null)
			{
				//	Is this the last one in the list?
				if((iIndex = m_tmaxResults.IndexOf(tmaxResult)) >= 0)
				{
					if(iIndex < (m_tmaxResults.Count - 1))
						tmaxResult = m_tmaxResults[iIndex + 1];
					else
						tmaxResult = null;
				}
				else
				{
					tmaxResult = null;
				}
			}
			else
			{
				if(m_tmaxResults.Count > 0)
					tmaxResult = m_tmaxResults[0];
											
			}// if((tmaxResult = (CTmaxVideoResult)(m_ctrlResults.GetSelected())) != null)
					
			return tmaxResult;
			
		}// private CTmaxVideoResult GetNext()
		
		/// <summary>This method is called to set the text displayed in the status bar</summary>
		private void SetStatus()
		{
			if((m_ctrlStatus != null) && (m_ctrlStatus.IsDisposed == false))
			{
				if((m_tmaxResults != null) && (m_tmaxResults.Count > 0))
				{					
					m_ctrlStatus.Text = (m_tmaxResults.Count.ToString() + " search results");
				}
				else
				{
					m_ctrlStatus.Text = "";
				}
			
			}
		
		}// private void SetStatus()
		
		/// <summary>This method will fire the LoadResult command event</summary>
		/// <param name="bOpen">true to open the transcript viewer</param>
		/// <returns>true if successful</returns>
		private bool FireLoadResult(bool bOpen)
		{
			CTmaxItem			tmaxItem = null;
			CTmaxVideoResult	tmaxResult = null;
			CTmaxParameters		tmaxParameters = null;
			bool				bSuccessful = false;
			
			if(m_tmaxResults == null) return false;
			if(m_tmaxResults.Count == 0) return false;
			
			//	Get the current selection
			if((tmaxResult = (CTmaxVideoResult)(m_ctrlResults.GetSelected())) != null)
			{
				if(tmaxResult.XmlDesignation != null)
				{
					//	Make sure this designation is still valid
					if(m_xmlScript.XmlDesignations.Contains(tmaxResult.XmlDesignation) == false)
						return false;
				}
						
				//	Allocate an item for the event
				tmaxItem = new CTmaxItem(m_xmlScript, tmaxResult.XmlDesignation);
				tmaxItem.UserData1 = tmaxResult;

				if(bOpen == true)
				{
					//	Create the required parameters required to open the transcript viewer
					tmaxParameters = new CTmaxParameters();
					tmaxParameters.Add(TmaxCommandParameters.Viewer, true);
				}
							
				if(FireCommand(TmaxVideoCommands.LoadResult, tmaxItem, tmaxParameters) != null)
					bSuccessful = true;
						
			}// if((tmaxResult = (CTmaxVideoResult)(m_ctrlResults.GetSelected())) != null)
					
			return bSuccessful;
			
		}// private void FireSetSearchResult(bool bOpen)
		
		/// <summary>Called when the user selects a new result</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnSelChanged(object sender, System.EventArgs e)
		{
			FireLoadResult(false);
								
		}// private void OnSelChanged(object sender, System.EventArgs e)
		
		/// <summary>Called when the user double clicks in the results list</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnDblClickResult(object sender, System.EventArgs e)
		{
			FireLoadResult(true);
								
		}// private void OnDblClickResult(object sender, System.EventArgs e)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The results of the search operation</summary>
		public CTmaxVideoResults Results
		{
			get { return m_tmaxResults; }
			set { m_tmaxResults = value; }
		}
		
		#endregion Properties
	
	}//  public class CTmaxVideoSearch : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView

}//  namespace FTI.Trialmax.TMVV.Tmvideo
