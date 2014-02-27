using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.Xml.XPath;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;

using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.ViewerObjectModel;

namespace FTI.Trialmax.Reports
{
	/// <summary>This form allows the user to create a Transcript report</summary>
	public class CRFExhibits : CRFBase
	{
		#region Constants
		
		private const string INFORMATION_TABLE_NAME = "Information";
		private const string MEDIA_TABLE_NAME  = "Media";
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_GET_SOURCE_RECORDS_EX		= (ERROR_RFBASE_MAX + 1);
		protected const int ERROR_EXCHANGE_EX				= (ERROR_RFBASE_MAX + 2);
		protected const int ERROR_ADD_MEDIA_RECORD_EX		= (ERROR_RFBASE_MAX + 3);
		protected const int ERROR_ADD_MEDIA_BINDER_EX		= (ERROR_RFBASE_MAX + 4);
		protected const int ERROR_FILL_SOURCE_EX			= (ERROR_RFBASE_MAX + 5);
		protected const int ERROR_PREVIEW_EX				= (ERROR_RFBASE_MAX + 6);
		protected const int ERROR_ADD_SOURCE_EX				= (ERROR_RFBASE_MAX + 7);
		protected const int ERROR_PREPARE_EX				= (ERROR_RFBASE_MAX + 8);
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Collection of entries to be added to the report source media table</summary>
		private CRSExhibitsMedias m_sourceMedias = new CRSExhibitsMedias();
		
		/// <summary>Options for generating the report</summary>
		private CROExhibits m_reportOptions = null;
	
		/// <summary>Index used to sort records in the media table</summary>
		private int m_iSortIndex = 0;
	
		/// <summary>Index used for primary records in the media table</summary>
		private int m_iPrimaryIndex = 0;
	
		/// <summary>Index used for secondary records in the media table</summary>
		private int m_iSecondaryIndex = 0;
		private System.Windows.Forms.GroupBox m_ctrlOptionsGroup;
		private System.Windows.Forms.TextBox m_ctrlSubTitle;
		private System.Windows.Forms.Label m_ctrlSubTitleLabel;
		private System.Windows.Forms.Label m_ctrlSortOrdersLabel;
		private System.Windows.Forms.ComboBox m_ctrlSortOrders;
		private System.Windows.Forms.GroupBox m_ctrlStyleGroup;
		private System.Windows.Forms.Label m_ctrlAlternateLabel;
		private System.Windows.Forms.ComboBox m_ctrlTemplates;
		private System.Windows.Forms.TextBox m_ctrlAlternate;
		private System.Windows.Forms.Button m_ctrlBrowse;
		private System.Windows.Forms.GroupBox m_ctrlContentGroup;
		private System.Windows.Forms.CheckBox m_ctrlIncludePages;
		private System.Windows.Forms.CheckBox m_ctrlIncludeTreatments;
		private System.Windows.Forms.CheckBox m_ctrlIncludeSubBinders;
		private System.Windows.Forms.Button m_ctrlCancel;
		private System.Windows.Forms.Button m_ctrlOK;
		private System.Windows.Forms.CheckBox m_ctrlSaveData;
		private System.Windows.Forms.ImageList m_ctrlImages;
		private System.Windows.Forms.CheckBox m_ctrlIncludeOnlyMapped;
		private System.Windows.Forms.CheckBox m_ctrlIncludeOnlyAdmitted;
	
		/// <summary>Index used for tertiary records in the media table</summary>
		private int m_iTertiaryIndex = 0;
	
		#endregion Private Members
		
		#region Public Methods

		/// <summary>Constructor</summary>
		public CRFExhibits() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
		}

		/// <summary>This method is called by the manager to verify that the form can be opened</summary>
		/// <returns>true if the form has what it needs to run the report</returns>
		public override bool CanExecute()
		{
			if(base.CanExecute() == false)
				return false;
			
			Debug.Assert(m_sourceMedias != null);
			if(m_sourceMedias == null) return false;
			
			//	The caller must have provided an item collection
			if((m_tmaxItems == null) || (m_tmaxItems.Count == 0))
			{
				MessageBox.Show("No records specified for the report", "Error", 
								 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			else
			{
				return true;
			}
		
		}// public override bool CanExecute()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to locate the source records.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the report options");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a record to the report queue. barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a binder to the report queue: name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the report's source data set.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to preview the report");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a record to the data set's Media table: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prepare the report for execution.");
		
		}// protected override void SetErrorStrings()

		/// <summary>Overloaded base class member to do custom initialization when the form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			Debug.Assert(m_reportOptions != null);
			
			//	Initialize the controls
			Exchange(true);
			
			//	Do the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				
				if(m_sourceMedias != null)
				{
					m_sourceMedias.Clear();
					m_sourceMedias = null;
				}
					
			}
			base.Dispose( disposing );
		
		}// protected override void Dispose( bool disposing )
		
		/// <summary>This method is called to exchange values between the form control and the local options object</summary>
		/// <param name="bSetControls">true to set the control values</param>
		/// <returns>true if successful</returns>
		protected override bool Exchange(bool bSetControls)
		{
			try
			{
				//	Are we setting the control values?
				if(bSetControls == true)
				{
					//	Populate the templates combo box
					m_ctrlTemplates.DataSource = Options.Templates;
				
					m_ctrlIncludeSubBinders.Checked = Options.IncludeSubBinders;
					m_ctrlIncludePages.Checked = Options.IncludePages;
					m_ctrlIncludeTreatments.Checked = Options.IncludeTreatments;
					m_ctrlIncludeOnlyMapped.Checked = Options.IncludeOnlyMapped;
					m_ctrlIncludeOnlyAdmitted.Checked = Options.IncludeOnlyAdmitted;
					m_ctrlSaveData.Checked = Options.SaveData;
					m_ctrlAlternate.Text = Options.Alternate;
					
					//	Set the sort order selection
					if((Options.SortOrder >= 0) && (Options.SortOrder < m_ctrlSortOrders.Items.Count))
						m_ctrlSortOrders.SelectedIndex = Options.SortOrder;
					else
						m_ctrlSortOrders.SelectedIndex = 0;
						
					//	Set the template selection
					if(Options.Templates.Count > 0)
					{
						if((Options.Template > 0) && (Options.Template <= Options.Templates.Count))
						{
							m_ctrlTemplates.SelectedIndex = Options.Template - 1;
						}
						else
						{
							m_ctrlTemplates.SelectedIndex = 0;
						}
					}
				
					m_ctrlIncludeTreatments.Enabled = m_ctrlIncludePages.Checked;
				
					m_ctrlSaveData.Visible = Options.ShowSaveData;
				}
				else
				{
					Options.IncludeSubBinders = m_ctrlIncludeSubBinders.Checked;
					Options.IncludePages = m_ctrlIncludePages.Checked;
					Options.IncludeTreatments = m_ctrlIncludeTreatments.Checked;
					Options.IncludeOnlyMapped = m_ctrlIncludeOnlyMapped.Checked;
					Options.IncludeOnlyAdmitted = m_ctrlIncludeOnlyAdmitted.Checked;
					Options.SortOrder = m_ctrlSortOrders.SelectedIndex;
					Options.SubTitle = m_ctrlSubTitle.Text;
					Options.Template = m_ctrlTemplates.SelectedIndex + 1;
					Options.SaveData = m_ctrlSaveData.Checked;
					Options.Alternate = m_ctrlAlternate.Text;
					
					if((Options.Template < 1) && (Options.Alternate.Length == 0))
					{
						MessageBox.Show("You must select a report style or specify an alternate template", "Error",
										MessageBoxButtons.OK, MessageBoxIcon.Warning);
						m_ctrlTemplates.Focus();
						return false;
					}
					else
					{
						m_roTemplate = (CROTemplate)(Options.Templates[Options.Template - 1]);
					}

				}// if(bSetControls == true)
				
				return true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX), Ex);
				return false;
			}
			
		}// protected override bool Exchange(bool bSetControls)
		
		/// <summary>This method is called to get the options for the report</summary>
		/// <returns>The options object associated with the report</returns>
		/// <remarks>This MUST be overridden by the derived class</remarks>
		protected override CROBase GetOptions()
		{
			return (m_reportOptions as CROBase);

		}// protected override CROBase GetOptions()
		
		/// <summary>This method is called to populate the media collection used to fill the report source</summary>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>the number of reports to be run</returns>
		protected override int GetSourceRecords(bool bOnePerPrimary)
		{
			Debug.Assert(m_sourceMedias != null);
			if(m_sourceMedias == null) return 0;
			
			Debug.Assert(m_tmaxItems != null);
			if(m_tmaxItems == null) return 0;
			
			try
			{
				//	Clear all existing records
				m_sourceMedias.Clear();
				
				//	Iterate the collection of event items 
				foreach(CTmaxItem O in m_tmaxItems)
				{
					//	Is this a media record?
					if(O.IPrimary != null)
					{
						AddMedia(m_sourceMedias, (CDxMediaRecord)(O.GetMediaRecord()));
					}
					else if(O.IBinderEntry != null)
					{
						AddMedia(m_sourceMedias, (CDxBinderEntry)(O.IBinderEntry));
					}
					
				}// foreach(CTmaxItem O in m_tmaxItems)
				
				if(m_sourceMedias.Count == 0)
				{
					MessageBox.Show("Unable to locate any records for the report using the current selections", "",
									MessageBoxButtons.OK, MessageBoxIcon.Information);
					return 0;
				}
				else
				{
					return 1;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceRecords", m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_RECORDS_EX), Ex);
				return 0;
			}
		
		}// protected override bool GetSourceRecords()
		
		/// <summary>This method is called to populate the source data set</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected override bool FillDataSet(int iReportIndex, bool bOnePerPrimary)
		{
			Debug.Assert(m_dsReportSource != null);
			if(m_dsReportSource == null) return false;
		
			Debug.Assert(m_sourceMedias != null);
			Debug.Assert(m_sourceMedias.Count > 0);
			if(m_sourceMedias == null) return false;
			if(m_sourceMedias.Count == 0) return false;
			
			try
			{
				//	Add the report information
				DataRow dr = m_dsReportSource.Tables[INFORMATION_TABLE_NAME].NewRow();
				
				if(Options.Alternate.Length > 0)
					dr["Title"] = "Custom";
				else if(m_roTemplate != null)
					dr["Title"] = m_roTemplate.Name;
				else
					dr["Title"] = "";
				
				dr["SubTitle"] = Options.SubTitle;
				dr["CaseName"] = m_tmaxDatabase.GetShortCaseName();
				dr["CasePath"] = m_tmaxDatabase.Folder;
				
				//	Add the row
				m_dsReportSource.Tables[INFORMATION_TABLE_NAME].Rows.Add(dr);

				//	Sort the records
				m_sourceMedias.SetSortOrder(Options.SortOrder);
				m_sourceMedias.Sort();
				
				//	Reset the sort index for media records
				m_iSortIndex = 0;
				m_iPrimaryIndex = 0;
				m_iSecondaryIndex = 0;
				m_iTertiaryIndex = 0;
				
				//	Add each of the media records to the report
				foreach(CRSExhibitsMedia O in m_sourceMedias)
				{
					switch(O.Source.GetMediaLevel())
					{
						case TmaxMediaLevels.Primary:
						
							m_iPrimaryIndex++;
							m_iSecondaryIndex = 0;
							m_iTertiaryIndex = 0;
							break;
							
						case TmaxMediaLevels.Secondary:
						
							m_iPrimaryIndex++;
							m_iSecondaryIndex++;
							m_iTertiaryIndex = 0;
							break;
							
						case TmaxMediaLevels.Tertiary:
						
							m_iPrimaryIndex++;
							m_iSecondaryIndex++;
							m_iTertiaryIndex++;
							break;
							
						default:
						
							Debug.Assert(false);
							continue;
							
					}
					
					AddSource(O);
					
				}// foreach(CRSExhibitsMedia O in m_sourceMedias)
			
				if((Options.SaveData == true) && (Options.ShowSaveData == true))
				{
					try
					{
						m_dsReportSource.WriteXml(System.IO.Path.ChangeExtension(m_strTemplate, "xml"));
					}
					catch
					{
					}
				
				}
				
				return true;			
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillSource", m_tmaxErrorBuilder.Message(ERROR_FILL_SOURCE_EX), Ex);
				return false;
			}

		}// protected override bool FillDataSet()
		
		/// <summary>This method is called to prepare the viewer to run the report</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected override bool Prepare(int iReportIndex, bool bOnePerPrimary)
		{
			try
			{
				//	Create a viewer form
				m_ctrlViewer = new CRFViewer();
				
				//	Initialize the form
				m_ctrlViewer.Title			= "Exhibits Report";
				m_ctrlViewer.Template		= m_strTemplate;
				m_ctrlViewer.ReportSource	= m_dsReportSource;
				m_ctrlViewer.EnableGroups	= false;
				
				//	Attach to the form's event source
				m_ctrlViewer.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
				m_ctrlViewer.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
				
				return m_ctrlViewer.Prepare();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Prepare", m_tmaxErrorBuilder.Message(ERROR_PREPARE_EX), Ex);
				return false;
			}

		}// protected override bool Prepare(int iReportIndex)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method will add an entry for the specified record to the media collection</summary>
		/// <param name="aMedias">The collection in which to add the new object</param>
		/// <param name="dxRecord">The record used as the source for the new media object</param>
		/// <returns>true if added to the specified collection</returns>
		private bool AddMedia(CRSExhibitsMedias aMedias, CDxMediaRecord dxRecord)
		{
			CDxPrimary			dxPrimary = null;
			CDxSecondary		dxSecondary = null;
			CRSExhibitsMedia	rsMedia = null;
			
			Debug.Assert(dxRecord != null);
			if(dxRecord == null) return false;
			
			//	Are we only supposed to be adding admitted media
			if(Options.IncludeOnlyAdmitted == true)
				if(dxRecord.Admitted == false) return false;
				
			try
			{
				rsMedia = new CRSExhibitsMedia();
				
				//	What type of media are we attempting to add?
				switch(dxRecord.MediaType)
				{
					case TmaxMediaTypes.Document:
					case TmaxMediaTypes.Powerpoint:
					case TmaxMediaTypes.Recording:
					case TmaxMediaTypes.Deposition:
					case TmaxMediaTypes.Script:
					
						dxPrimary = (CDxPrimary)dxRecord;
						
						//	Add children if requested
						if(m_reportOptions.IncludePages == true)
						{
							if((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
								dxPrimary.Fill();
								
							foreach(CDxSecondary O in dxPrimary.Secondaries)
							{
								AddMedia(rsMedia.Children, O);
							}
						
						}
							
						//	Should we add this record to the collection?
						if((rsMedia.Children.Count > 0) || 
						(m_reportOptions.IncludeOnlyMapped == false) ||
						((dxRecord.Mapped == true) && (dxRecord.ForeignBarcode.Length > 0)))
						{
							SetMediaProps(rsMedia, dxRecord);
							aMedias.Add(rsMedia);
							return true;
						}
						break;
					
					case TmaxMediaTypes.Page:
					
						dxSecondary = (CDxSecondary)dxRecord;
						
						//	Add children if requested
						if(m_reportOptions.IncludeTreatments == true)
						{
							if((dxSecondary.Tertiaries == null) || (dxSecondary.Tertiaries.Count == 0))
								dxSecondary.Fill();
								
							foreach(CDxTertiary O in dxSecondary.Tertiaries)
							{
								AddMedia(rsMedia.Children, O);
							}
							
						}
							
						//	Should we add this record to the collection?
						if((rsMedia.Children.Count > 0) || 
							(m_reportOptions.IncludeOnlyMapped == false) ||
							((dxRecord.Mapped == true) && (dxRecord.ForeignBarcode.Length > 0)))
						{
							SetMediaProps(rsMedia, dxRecord);
							aMedias.Add(rsMedia);
							return true;
						}
						break;
					
					case TmaxMediaTypes.Scene:
					case TmaxMediaTypes.Slide:
					case TmaxMediaTypes.Segment:
					case TmaxMediaTypes.Treatment:
					
						//	Should we add this record to the collection?
						if((m_reportOptions.IncludeOnlyMapped == false) ||
							((dxRecord.Mapped == true) && (dxRecord.ForeignBarcode.Length > 0)))
						{
							SetMediaProps(rsMedia, dxRecord);
							aMedias.Add(rsMedia);
							return true;
						}
						break;
						
					case TmaxMediaTypes.Designation:
					case TmaxMediaTypes.Clip:
					case TmaxMediaTypes.Link:
					default:
					
						break;	
				
				}// switch(dxRecord.MediaType)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddMedia", m_tmaxErrorBuilder.Message(ERROR_ADD_MEDIA_RECORD_EX, dxRecord.GetBarcode(false)), Ex);
			}
			
			//	Must not have been added to the collection
			return false;
		
		}// private bool AddMedia(CRSExhibitsMedias aMedias, CDxMediaRecord dxRecord)
		
		/// <summary>This method will add the contents of the specified binder to the collection</summary>
		/// <param name="aMedias">The collection in which to add the new objects</param>
		/// <param name="dxRecord">The binder to be iterated</param>
		/// <returns>true if any records have been added</returns>
		private bool AddMedia(CRSExhibitsMedias aMedias, CDxBinderEntry dxBinder)
		{
			long lExisting = aMedias.Count;
			
			Debug.Assert(dxBinder != null);
			if(dxBinder == null) return false;
			
			try
			{
				//	Do we need to fill the child collection
				if((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
					dxBinder.Fill();
					
				//	Iterate the collection
				foreach(CDxBinderEntry O in dxBinder.Contents)
				{
					//	Is this a media reference?
					if(O.IsMedia() == true)
					{
						if(O.Source != null)
							AddMedia(aMedias, O.Source);
					}
					else if(m_reportOptions.IncludeSubBinders == true)
					{
						AddMedia(aMedias, O);
					}
		
				}// foreach(CDxBinderEntry O in dxBinder.Contents)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddMedia", m_tmaxErrorBuilder.Message(ERROR_ADD_MEDIA_BINDER_EX, dxBinder.Name), Ex);
			}
			
			return (aMedias.Count > lExisting);
		
		}// private bool AddMedia(CRSExhibitsMedias aMedias, CDxBinderEntry dxBinder)
		
		/// <summary>This method will set the specified media object properties using the record provided by the caller</summary>
		/// <param name="rsMedia">The media object to be initialized</param>
		/// <param name="dxRecord">The source record</param>
		private void SetMediaProps(CRSExhibitsMedia rsMedia, CDxMediaRecord dxRecord)
		{
			rsMedia.Source = dxRecord;
			rsMedia.MediaType = (short)(dxRecord.MediaType);
			rsMedia.LastModified = dxRecord.ModifiedOn;
			rsMedia.Admitted = dxRecord.Admitted;
			rsMedia.Barcode = dxRecord.GetBarcode(true);
			rsMedia.ForeignBarcode = dxRecord.ForeignBarcode;
						
			//	What type of media are we dealing with?
			switch(dxRecord.MediaType)
			{
				case TmaxMediaTypes.Document:
				case TmaxMediaTypes.Recording:
				case TmaxMediaTypes.Deposition:
				case TmaxMediaTypes.Script:
				
					rsMedia.Name = dxRecord.Name;
					rsMedia.Description = dxRecord.Description;
					
					if(rsMedia.Name.Length == 0)
						rsMedia.Name = ((CDxPrimary)dxRecord).MediaId;
						
					if(dxRecord.MediaType != TmaxMediaTypes.Script)
					{
						rsMedia.Path = m_tmaxDatabase.GetRelativePath(dxRecord);
					}
					break;
					
				case TmaxMediaTypes.Powerpoint:
				
					rsMedia.Name = dxRecord.Name;
					
					try
					{
						rsMedia.Path = m_tmaxDatabase.GetRelativePath(dxRecord);
					}
					catch{}
					
					if(dxRecord.Description.Length > 0)
						rsMedia.Description = dxRecord.Description;
					else
						rsMedia.Description = dxRecord.GetFileName();
					break;
					
				case TmaxMediaTypes.Page:
				case TmaxMediaTypes.Treatment:
				case TmaxMediaTypes.Segment:
				
					rsMedia.Name = dxRecord.Name;
					rsMedia.Description = dxRecord.Description;
					
					if(rsMedia.Name.Length == 0)
						rsMedia.Name = dxRecord.GetFileName();
					
					try
					{
						rsMedia.Path = m_tmaxDatabase.GetRelativePath(dxRecord);
					}
					catch{}

					break;
					
				case TmaxMediaTypes.Slide:
				
					rsMedia.Name = dxRecord.Name;
					rsMedia.Description = dxRecord.Description;

					try
					{
						if(dxRecord.GetParent() != null)
						{
							rsMedia.Path = m_tmaxDatabase.GetRelativePath(dxRecord.GetParent());
						}
					}
					catch{}
					break;
					
				case TmaxMediaTypes.Scene:
					
					if(((CDxSecondary)dxRecord).GetSource() != null)
					{
						rsMedia.Name = ((CDxSecondary)dxRecord).GetBarcode(false);
					}

					try
					{
						if(((CDxSecondary)dxRecord).GetSource() != null)
						{
							rsMedia.Path = m_tmaxDatabase.GetRelativePath(((CDxSecondary)dxRecord).GetSource());
						}
					}
					catch{}
					
					break;
						
				case TmaxMediaTypes.Designation:
				case TmaxMediaTypes.Clip:
				case TmaxMediaTypes.Link:
				default:
					
					break;	
				
			}// switch(dxRecord.MediaType)
			
			//	Strip the trailing backslash from the path if it exists
			if(rsMedia.Path.Length > 0)
			{
				if(rsMedia.Path.EndsWith("\\") == true)
					rsMedia.Path = rsMedia.Path.Substring(0, rsMedia.Path.Length - 1);
			}

		}// private void SetMediaProps(CRSExhibitsMedia rsMedia, CDxMediaRecord dxRecord)
		
		/// <summary>This method is called to add a row to the Media table in the data set</summary>
		/// <param name="rsMedia">The media object used to initialize the row values</param>
		/// <returns>true if successful</returns>
		private bool AddSource(CRSExhibitsMedia rsMedia)
		{
			Debug.Assert(rsMedia != null);
			
			try
			{
				//	Increment the sort index
				m_iSortIndex++;

				//	Create a new row for this script
				DataRow dr = m_dsReportSource.Tables[MEDIA_TABLE_NAME].NewRow();
				
				//	Set the values
				dr["SortOrder"] = m_iSortIndex;
				dr["PrimaryIndex"] = m_iPrimaryIndex;
				dr["SecondaryIndex"] = m_iSecondaryIndex;
				dr["TertiaryIndex"] = m_iTertiaryIndex;
				dr["Barcode"] = rsMedia.Barcode;
				dr["Type"] = rsMedia.MediaType;
				dr["Name"] = rsMedia.Name;
				dr["Description"] = rsMedia.Description;
				dr["Updated"] = rsMedia.LastModified;
				dr["Admitted"] = rsMedia.Admitted;
				dr["ForeignBarcode"] = rsMedia.ForeignBarcode;
				dr["Path"] = rsMedia.Path;

				//	Add the row
				m_dsReportSource.Tables[MEDIA_TABLE_NAME].Rows.Add(dr);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_ADD_SOURCE_EX, rsMedia.Barcode), Ex);
				return false;
			}
			
			//	Add a row for each child
			foreach(CRSExhibitsMedia O in rsMedia.Children)
			{
				switch(O.Source.GetMediaLevel())
				{
					case TmaxMediaLevels.Secondary:
						
						m_iSecondaryIndex++;
						m_iTertiaryIndex = 0;
						break;
						
					case TmaxMediaLevels.Tertiary:
					
						m_iTertiaryIndex++;
						break;
						
					default:
					
						Debug.Assert(false);
						continue;
						
				}
				
				AddSource(O);
			
			}// foreach(CRSExhibitsMedia O in rsMedia.Children) 
			
			return true;
			
		}// private bool AddSource(CRSExhibitsMedia rsMedia)
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CRFExhibits));
			this.m_ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlSubTitle = new System.Windows.Forms.TextBox();
			this.m_ctrlSubTitleLabel = new System.Windows.Forms.Label();
			this.m_ctrlSortOrdersLabel = new System.Windows.Forms.Label();
			this.m_ctrlSortOrders = new System.Windows.Forms.ComboBox();
			this.m_ctrlStyleGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlAlternateLabel = new System.Windows.Forms.Label();
			this.m_ctrlTemplates = new System.Windows.Forms.ComboBox();
			this.m_ctrlAlternate = new System.Windows.Forms.TextBox();
			this.m_ctrlBrowse = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlContentGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlIncludeOnlyAdmitted = new System.Windows.Forms.CheckBox();
			this.m_ctrlIncludePages = new System.Windows.Forms.CheckBox();
			this.m_ctrlIncludeTreatments = new System.Windows.Forms.CheckBox();
			this.m_ctrlIncludeSubBinders = new System.Windows.Forms.CheckBox();
			this.m_ctrlIncludeOnlyMapped = new System.Windows.Forms.CheckBox();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlSaveData = new System.Windows.Forms.CheckBox();
			this.m_ctrlOptionsGroup.SuspendLayout();
			this.m_ctrlStyleGroup.SuspendLayout();
			this.m_ctrlContentGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlOptionsGroup
			// 
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSubTitle);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSubTitleLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSortOrdersLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSortOrders);
			this.m_ctrlOptionsGroup.Location = new System.Drawing.Point(7, 236);
			this.m_ctrlOptionsGroup.Name = "m_ctrlOptionsGroup";
			this.m_ctrlOptionsGroup.Size = new System.Drawing.Size(288, 76);
			this.m_ctrlOptionsGroup.TabIndex = 2;
			this.m_ctrlOptionsGroup.TabStop = false;
			this.m_ctrlOptionsGroup.Text = "Options";
			// 
			// m_ctrlSubTitle
			// 
			this.m_ctrlSubTitle.Location = new System.Drawing.Point(80, 48);
			this.m_ctrlSubTitle.Name = "m_ctrlSubTitle";
			this.m_ctrlSubTitle.Size = new System.Drawing.Size(200, 20);
			this.m_ctrlSubTitle.TabIndex = 1;
			this.m_ctrlSubTitle.Text = "";
			// 
			// m_ctrlSubTitleLabel
			// 
			this.m_ctrlSubTitleLabel.Location = new System.Drawing.Point(8, 48);
			this.m_ctrlSubTitleLabel.Name = "m_ctrlSubTitleLabel";
			this.m_ctrlSubTitleLabel.Size = new System.Drawing.Size(68, 20);
			this.m_ctrlSubTitleLabel.TabIndex = 6;
			this.m_ctrlSubTitleLabel.Text = "SubTitle:";
			this.m_ctrlSubTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSortOrdersLabel
			// 
			this.m_ctrlSortOrdersLabel.Location = new System.Drawing.Point(8, 20);
			this.m_ctrlSortOrdersLabel.Name = "m_ctrlSortOrdersLabel";
			this.m_ctrlSortOrdersLabel.Size = new System.Drawing.Size(68, 20);
			this.m_ctrlSortOrdersLabel.TabIndex = 5;
			this.m_ctrlSortOrdersLabel.Text = "Sort By:";
			this.m_ctrlSortOrdersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSortOrders
			// 
			this.m_ctrlSortOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlSortOrders.Items.AddRange(new object[] {
																  "Barcode",
																  "Name",
																  "Foreign Barcode",
																  "Media Type",
																  "Last Modified"});
			this.m_ctrlSortOrders.Location = new System.Drawing.Point(80, 20);
			this.m_ctrlSortOrders.Name = "m_ctrlSortOrders";
			this.m_ctrlSortOrders.Size = new System.Drawing.Size(200, 21);
			this.m_ctrlSortOrders.TabIndex = 0;
			// 
			// m_ctrlStyleGroup
			// 
			this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlAlternateLabel);
			this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlTemplates);
			this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlAlternate);
			this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlBrowse);
			this.m_ctrlStyleGroup.Location = new System.Drawing.Point(7, 136);
			this.m_ctrlStyleGroup.Name = "m_ctrlStyleGroup";
			this.m_ctrlStyleGroup.Size = new System.Drawing.Size(288, 92);
			this.m_ctrlStyleGroup.TabIndex = 1;
			this.m_ctrlStyleGroup.TabStop = false;
			this.m_ctrlStyleGroup.Text = "Style";
			// 
			// m_ctrlAlternateLabel
			// 
			this.m_ctrlAlternateLabel.Location = new System.Drawing.Point(8, 48);
			this.m_ctrlAlternateLabel.Name = "m_ctrlAlternateLabel";
			this.m_ctrlAlternateLabel.Size = new System.Drawing.Size(248, 16);
			this.m_ctrlAlternateLabel.TabIndex = 3;
			this.m_ctrlAlternateLabel.Text = "Alternate Report Template";
			this.m_ctrlAlternateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_ctrlTemplates
			// 
			this.m_ctrlTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlTemplates.Location = new System.Drawing.Point(8, 20);
			this.m_ctrlTemplates.Name = "m_ctrlTemplates";
			this.m_ctrlTemplates.Size = new System.Drawing.Size(272, 21);
			this.m_ctrlTemplates.TabIndex = 0;
			// 
			// m_ctrlAlternate
			// 
			this.m_ctrlAlternate.Location = new System.Drawing.Point(8, 64);
			this.m_ctrlAlternate.Name = "m_ctrlAlternate";
			this.m_ctrlAlternate.Size = new System.Drawing.Size(240, 20);
			this.m_ctrlAlternate.TabIndex = 1;
			this.m_ctrlAlternate.Text = "";
			// 
			// m_ctrlBrowse
			// 
			this.m_ctrlBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowse.ImageIndex = 0;
			this.m_ctrlBrowse.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowse.Location = new System.Drawing.Point(256, 64);
			this.m_ctrlBrowse.Name = "m_ctrlBrowse";
			this.m_ctrlBrowse.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowse.TabIndex = 2;
			this.m_ctrlBrowse.Click += new System.EventHandler(this.OnClickBrowse);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlContentGroup
			// 
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeOnlyAdmitted);
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludePages);
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeTreatments);
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeSubBinders);
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeOnlyMapped);
			this.m_ctrlContentGroup.Location = new System.Drawing.Point(7, 8);
			this.m_ctrlContentGroup.Name = "m_ctrlContentGroup";
			this.m_ctrlContentGroup.Size = new System.Drawing.Size(288, 124);
			this.m_ctrlContentGroup.TabIndex = 0;
			this.m_ctrlContentGroup.TabStop = false;
			this.m_ctrlContentGroup.Text = "Content";
			// 
			// m_ctrlIncludeOnlyAdmitted
			// 
			this.m_ctrlIncludeOnlyAdmitted.Checked = true;
			this.m_ctrlIncludeOnlyAdmitted.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlIncludeOnlyAdmitted.Location = new System.Drawing.Point(8, 100);
			this.m_ctrlIncludeOnlyAdmitted.Name = "m_ctrlIncludeOnlyAdmitted";
			this.m_ctrlIncludeOnlyAdmitted.Size = new System.Drawing.Size(276, 16);
			this.m_ctrlIncludeOnlyAdmitted.TabIndex = 4;
			this.m_ctrlIncludeOnlyAdmitted.Text = "Include Only If Admitted";
			// 
			// m_ctrlIncludePages
			// 
			this.m_ctrlIncludePages.Checked = true;
			this.m_ctrlIncludePages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlIncludePages.Location = new System.Drawing.Point(8, 20);
			this.m_ctrlIncludePages.Name = "m_ctrlIncludePages";
			this.m_ctrlIncludePages.Size = new System.Drawing.Size(276, 16);
			this.m_ctrlIncludePages.TabIndex = 0;
			this.m_ctrlIncludePages.Text = "Include Pages";
			this.m_ctrlIncludePages.Click += new System.EventHandler(this.OnClickIncludePages);
			// 
			// m_ctrlIncludeTreatments
			// 
			this.m_ctrlIncludeTreatments.Checked = true;
			this.m_ctrlIncludeTreatments.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlIncludeTreatments.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlIncludeTreatments.Name = "m_ctrlIncludeTreatments";
			this.m_ctrlIncludeTreatments.Size = new System.Drawing.Size(276, 16);
			this.m_ctrlIncludeTreatments.TabIndex = 1;
			this.m_ctrlIncludeTreatments.Text = "Include Treatments";
			// 
			// m_ctrlIncludeSubBinders
			// 
			this.m_ctrlIncludeSubBinders.Checked = true;
			this.m_ctrlIncludeSubBinders.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlIncludeSubBinders.Location = new System.Drawing.Point(8, 60);
			this.m_ctrlIncludeSubBinders.Name = "m_ctrlIncludeSubBinders";
			this.m_ctrlIncludeSubBinders.Size = new System.Drawing.Size(276, 16);
			this.m_ctrlIncludeSubBinders.TabIndex = 2;
			this.m_ctrlIncludeSubBinders.Text = "Include Sub-binders";
			// 
			// m_ctrlIncludeOnlyMapped
			// 
			this.m_ctrlIncludeOnlyMapped.Checked = true;
			this.m_ctrlIncludeOnlyMapped.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlIncludeOnlyMapped.Location = new System.Drawing.Point(8, 80);
			this.m_ctrlIncludeOnlyMapped.Name = "m_ctrlIncludeOnlyMapped";
			this.m_ctrlIncludeOnlyMapped.Size = new System.Drawing.Size(276, 16);
			this.m_ctrlIncludeOnlyMapped.TabIndex = 3;
			this.m_ctrlIncludeOnlyMapped.Text = "Include Only Mapped (valid foreign barcode)";
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(219, 320);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 5;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Location = new System.Drawing.Point(135, 320);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 4;
			this.m_ctrlOK.Text = "&OK";
			// 
			// m_ctrlSaveData
			// 
			this.m_ctrlSaveData.Location = new System.Drawing.Point(23, 324);
			this.m_ctrlSaveData.Name = "m_ctrlSaveData";
			this.m_ctrlSaveData.Size = new System.Drawing.Size(76, 16);
			this.m_ctrlSaveData.TabIndex = 3;
			this.m_ctrlSaveData.Text = "Save Data";
			this.m_ctrlSaveData.Visible = false;
			// 
			// CRFExhibits
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(302, 351);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlOptionsGroup);
			this.Controls.Add(this.m_ctrlStyleGroup);
			this.Controls.Add(this.m_ctrlContentGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Controls.Add(this.m_ctrlSaveData);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CRFExhibits";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Exhibits Report";
			this.m_ctrlOptionsGroup.ResumeLayout(false);
			this.m_ctrlStyleGroup.ResumeLayout(false);
			this.m_ctrlContentGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method handles the event fired when the user clicks on the IncludePages check box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickIncludePages(object sender, System.EventArgs e)
		{
			m_ctrlIncludeTreatments.Enabled = m_ctrlIncludePages.Checked;
		}

		/// <summary>This method handles the event fired when the user clicks on the Browse button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickBrowse(object sender, System.EventArgs e)
		{
			string strFilename = m_ctrlAlternate.Text;
			
			if(BrowseForTemplate(ref strFilename) == true)
				m_ctrlAlternate.Text = strFilename;

		}// private void OnClickBrowse(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>User defined options for generating the report</summary>
		public CROExhibits Options
		{
			get { return m_reportOptions; }
			set { m_reportOptions = value; }
		}
		
		#endregion Properties

	}// public class CRFExhibits : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Reports
