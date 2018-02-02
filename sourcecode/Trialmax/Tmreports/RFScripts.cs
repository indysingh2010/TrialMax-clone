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
	public class CRFScripts : CRFBase
	{
		#region Constants
		
		private const int MAX_HIGHLIGHTERS = 7;
		private const int DEFAULT_HIGHLIGHTER = 0x000000;
		
		private const string INFORMATION_TABLE_NAME = "Information";
		private const string TRANSCRIPT_TABLE_NAME  = "Transcript";
		private const string CLIP_TABLE_NAME  = "Clip";
        private const string HIGHLIGHTER_TABLE_NAME = "HighlighterDetail";
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_ADD_EX					= (ERROR_RFBASE_MAX + 1);
		protected const int ERROR_EXCHANGE_EX				= (ERROR_RFBASE_MAX + 2);
		protected const int ERROR_GET_ITEM_SCRIPTS_EX		= (ERROR_RFBASE_MAX + 3);
		protected const int ERROR_GET_BINDER_SCRIPTS_EX		= (ERROR_RFBASE_MAX + 4);
		protected const int ERROR_FILL_SOURCE_EX			= (ERROR_RFBASE_MAX + 5);
		protected const int ERROR_XML_NOT_FOUND				= (ERROR_RFBASE_MAX + 6);
		protected const int ERROR_PREPARE_EX				= (ERROR_RFBASE_MAX + 7);
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Exchange interfaces for all scripts in the report</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxScripts = new CTmaxItems();
		
		/// <summary>Options for generating the report</summary>
		private CROScripts m_reportOptions = null;
		
		/// <summary>Image list for browse button image</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Check box bound to PageBreak option</summary>
		private System.Windows.Forms.CheckBox m_ctrlPageBreak;
		
		/// <summary>Check box bound to IncludeSubBinders option</summary>
		private System.Windows.Forms.CheckBox m_ctrlIncludeSubBinders;
		
		/// <summary>Check box bound to HighlightText option</summary>
		private System.Windows.Forms.CheckBox m_ctrlHighlightText;
		
		/// <summary>Check box bound to IndicateLinked option</summary>
		private System.Windows.Forms.CheckBox m_ctrlIndicateLinked;
		
		/// <summary>Check box bound to IndicateEdited option</summary>
		private System.Windows.Forms.CheckBox m_ctrlIndicateEdited;
		
		/// <summary>Group box for controls related to report content</summary>
		private System.Windows.Forms.GroupBox m_ctrlContentGroup;
		
		/// <summary>Check box bound to IncludeTranscriptText option</summary>
		private System.Windows.Forms.CheckBox m_ctrlIncludeTranscriptText;
		
		/// <summary>Group box for controls related to report options</summary>
		private System.Windows.Forms.GroupBox m_ctrlOptionsGroup;
		
		/// <summary>Check box bound to IncludeCustomShows option</summary>
		private System.Windows.Forms.CheckBox m_ctrlIncludeCustomShows;
		
		/// <summary>Check box bound to IncludePlaylists option</summary>
		private System.Windows.Forms.CheckBox m_ctrlIncludePlaylists;
		
		/// <summary>Group box for controls related to report style</summary>
		private System.Windows.Forms.GroupBox m_ctrlStyleGroup;
		
		/// <summary>Static text label for alternate template box</summary>
		private System.Windows.Forms.Label m_ctrlAlternateLabel;
		
		/// <summary>Combobox for list of available templates</summary>
		private System.Windows.Forms.ComboBox m_ctrlTemplates;
		
		/// <summary>Text box for path to alternate template</summary>
		private System.Windows.Forms.TextBox m_ctrlAlternate;
		
		/// <summary>Pushbutton to browse for alternate template</summary>
		private System.Windows.Forms.Button m_ctrlBrowse;
		
		/// <summary>Check box bound to BarcodeGraphic option</summary>
		private System.Windows.Forms.CheckBox m_ctrlBarcodeGraphic;
		
		/// <summary>Check box bound to SaveData option (normally hidden)</summary>
		private System.Windows.Forms.CheckBox m_ctrlSaveData;
		
		/// <summary>Check box bound to Duration option</summary>
		private System.Windows.Forms.CheckBox m_ctrlDuration;
		
		/// <summary>Check box bound to Barcode Text option</summary>
		private System.Windows.Forms.CheckBox m_ctrlBarcodeText;
		
		/// <summary>Check box bound to ElapsedTime option</summary>
		private System.Windows.Forms.CheckBox m_ctrlElapsed;
		
		/// <summary>Check box bound to RemainingTime option</summary>
		private System.Windows.Forms.CheckBox m_ctrlRemaining;
		
		/// <summary>Check box bound to TimeTotals option</summary>
		private System.Windows.Forms.CheckBox m_ctrlTimeTotals;
		
		/// <summary>Check box bound to MediaFile option</summary>
		private System.Windows.Forms.CheckBox m_ctrlMediaFile;
		
		/// <summary>Group box for controls related to export options</summary>
		private System.Windows.Forms.GroupBox m_ctrlExportGroup;
		
		/// <summary>Check box bound to SplitExports option</summary>
		private System.Windows.Forms.CheckBox m_ctrlSplitExports;
		
		/// <summary>Check box bound to PreviewExports option</summary>
		private System.Windows.Forms.CheckBox m_ctrlPreviewExports;
		
		/// <summary>Static text label for Export Formats combobox</summary>
		private System.Windows.Forms.Label m_ctrlFormatsLabel;
		
		/// <summary>Combobox for drop list of available export formats</summary>
		private System.Windows.Forms.ComboBox m_ctrlExportFormats;
		
		/// <summary>Array of highlighter colors</summary>
		private int [] m_aHighlighters = new int[MAX_HIGHLIGHTERS];

        /// <summary>Array of highlighter name</summary>
        private string[] m_aHighlightersNames = new string[MAX_HIGHLIGHTERS];
	
		/// <summary>Local member to keep track of total time</summary>
		private long m_lAccumulatedTime = 0;
	
		/// <summary>Local member to keep track of total time for the script</summary>
		private long m_lScriptTime = 0;
	
		/// <summary>Local member to keep track of elapsed time for the script</summary>
		private long m_lScriptElapsed = 0;
	
		/// <summary>Local member to keep track of the current script index</summary>
		private int m_iScriptIndex = 0;
	
		/// <summary>Static text label for export folder text box</summary>
		private System.Windows.Forms.Label m_ctrlExportFolderLabel;
	
		/// <summary>Text box to specify path to the export folder</summary>
		private System.Windows.Forms.TextBox m_ctrlExportFolder;
	
		/// <summary>Pushbutton to open folder browser for the export folder path</summary>
		private System.Windows.Forms.Button m_ctrlBrowseExportFolder;
	
		/// <summary>Check box to set the AddMediaName option for the report</summary>
		private System.Windows.Forms.CheckBox m_ctrlAddMediaName;

		/// <summary>Check box bound to SceneNumber option</summary>
		private CheckBox m_ctrlSceneNumber;
        private CheckBox m_ctrlHighlighterIndex;
	
		/// <summary>Local member to keep track of the current clip index</summary>
		private int m_iClipIndex = 0;
	
		#endregion Private Members
		
		#region Public Methods

		/// <summary>Constructor</summary>
		public CRFScripts() : base()
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
			
			Debug.Assert(m_tmaxScripts != null);
			if(m_tmaxScripts == null) return false;
			
			//	The caller must have provided an item collection
			if((m_tmaxItems == null) || (m_tmaxItems.Count == 0))
			{
				MessageBox.Show("No scripts available for the report", "Error", 
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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the script to the queue: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the report options");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the scripts for an event item.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the scripts in a binder: name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding a script to the report source: mediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the XML file containing the transcript text for %1: filename = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prepare the report for execution");
		
		}// protected override void SetErrorStrings()

		/// <summary>Overloaded base class member to do custom initialization when the form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			Debug.Assert(m_reportOptions != null);
			
			//	Initialize the controls
			Exchange(true);
			
			//	Get the highlighter colors
            for (int i = 0; i < MAX_HIGHLIGHTERS; i++)
            {
                m_aHighlighters[i] = GetOleHighlighter(i);
                m_aHighlightersNames[i] = GetHighlighterName(i);
            }
			//	Do the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method handles the Click event fired by the export</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnBrowseExportFolder(object sender, System.EventArgs e)
		{
			string strFolder = m_ctrlExportFolder.Text;
			
			if(BrowseForFolder("Choose Export Folder", ref strFolder) == true)
				m_ctrlExportFolder.Text = strFolder;
		
		}// protected void OnBrowseExportFolder(object sender, System.EventArgs e)
		
		/// <summary>This method handles the SelectedIndexChanged event fired by the Export Formats combobox</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnExportFormatsSelChanged(object sender, System.EventArgs e)
		{
			TmaxExportReportFormats eFormat = GetExportFormat(m_ctrlExportFormats);
			
			m_ctrlExportFolder.Enabled = (eFormat != TmaxExportReportFormats.None);
			m_ctrlExportFolderLabel.Enabled = (eFormat != TmaxExportReportFormats.None);
			m_ctrlSplitExports.Enabled = (eFormat != TmaxExportReportFormats.None);
			m_ctrlPreviewExports.Enabled = (eFormat != TmaxExportReportFormats.None);
			m_ctrlBrowseExportFolder.Enabled = (eFormat != TmaxExportReportFormats.None);
			m_ctrlAddMediaName.Enabled = (eFormat != TmaxExportReportFormats.None);
		
		}// protected void OnExportFormatsSelChanged(object sender, System.EventArgs e)
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				
				if(m_tmaxScripts != null)
				{
					m_tmaxScripts.Clear();
					m_tmaxScripts = null;
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
					m_ctrlIncludeCustomShows.Checked = Options.IncludeCustomShows;
					m_ctrlIncludePlaylists.Checked = Options.IncludePlaylists;
					m_ctrlIncludeTranscriptText.Checked = Options.IncludeTranscriptText;
					m_ctrlPageBreak.Checked = Options.PageBreak;
					m_ctrlAlternate.Text = Options.Alternate;
					m_ctrlHighlightText.Checked = Options.HighlightText;
                    m_ctrlHighlighterIndex.Checked = Options.HighlighterIndex;
					m_ctrlIndicateLinked.Checked = Options.IndicateLinks;
					m_ctrlIndicateEdited.Checked = Options.IndicateEdited;
					m_ctrlBarcodeGraphic.Checked = Options.BarcodeGraphic;
					m_ctrlBarcodeText.Checked = Options.BarcodeText;
					m_ctrlMediaFile.Checked = Options.MediaFile;
					m_ctrlDuration.Checked = Options.Duration;
					m_ctrlElapsed.Checked = Options.Elapsed;
					m_ctrlRemaining.Checked = Options.Remaining;
					m_ctrlTimeTotals.Checked = Options.TimeTotals;
					m_ctrlSaveData.Checked = Options.SaveData;
					m_ctrlSceneNumber.Checked = Options.SceneNumber;
					
					m_ctrlHighlightText.Enabled = m_ctrlIncludeTranscriptText.Checked;
                    m_ctrlHighlighterIndex.Enabled = m_ctrlIncludeTranscriptText.Checked;
					m_ctrlExportFolder.Text = Options.ExportFolder;
					m_ctrlPreviewExports.Checked = Options.PreviewExports;
					m_ctrlSplitExports.Checked = Options.SplitExports;
					m_ctrlAddMediaName.Checked = Options.AddMediaName;
					
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
				
					FillExportFormats(m_ctrlExportFormats, Options.ExportFormat);
					OnExportFormatsSelChanged(m_ctrlExportFormats, EventArgs.Empty);
					
					m_ctrlSaveData.Visible = Options.ShowSaveData;
				}
				else
				{
					//	Make sure the export folder is valid
					if(CheckExportFolder(GetExportFormat(m_ctrlExportFormats), m_ctrlExportFolder.Text) == false)
					{
						m_ctrlExportFolder.Focus();
						return false;
					}
					
					Options.IncludeSubBinders = m_ctrlIncludeSubBinders.Checked;
					Options.IncludeCustomShows = m_ctrlIncludeCustomShows.Checked;
					Options.IncludePlaylists = m_ctrlIncludePlaylists.Checked;
					Options.IncludeTranscriptText = m_ctrlIncludeTranscriptText.Checked;
					Options.PageBreak = m_ctrlPageBreak.Checked;
					Options.HighlightText = m_ctrlHighlightText.Checked;
                    Options.HighlighterIndex = m_ctrlHighlighterIndex.Checked;
					Options.IndicateLinks = m_ctrlIndicateLinked.Checked;
					Options.IndicateEdited = m_ctrlIndicateEdited.Checked;
					Options.BarcodeGraphic = m_ctrlBarcodeGraphic.Checked;
					Options.BarcodeText = m_ctrlBarcodeText.Checked;
					Options.MediaFile = m_ctrlMediaFile.Checked;
					Options.TimeTotals = m_ctrlTimeTotals.Checked;
					Options.Duration = m_ctrlDuration.Checked;
					Options.Elapsed = m_ctrlElapsed.Checked;
					Options.Remaining = m_ctrlRemaining.Checked;
					Options.SceneNumber = m_ctrlSceneNumber.Checked;
					Options.Template = m_ctrlTemplates.SelectedIndex + 1;
					Options.Alternate = m_ctrlAlternate.Text;
					Options.SaveData = m_ctrlSaveData.Checked;
					Options.ExportFormat = GetExportFormat(m_ctrlExportFormats);
					Options.ExportFolder = m_ctrlExportFolder.Text;
					Options.PreviewExports = m_ctrlPreviewExports.Checked;
					Options.SplitExports = m_ctrlSplitExports.Checked;
					Options.AddMediaName = m_ctrlAddMediaName.Checked;

					//	The use must select either playlists or custom shows
					if((m_reportOptions.IncludeCustomShows == false) && (m_reportOptions.IncludePlaylists == false))
					{
							MessageBox.Show("You must include either Custom Shows, Playlists, or both.", "Error",
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
						m_ctrlIncludePlaylists.Focus();
						return false;
					}
					
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
			
		}// private bool Exchange(bool bSetControls)
		
		/// <summary>This method is called to get the options for the report</summary>
		/// <returns>The options object associated with the report</returns>
		/// <remarks>This MUST be overridden by the derived class</remarks>
		protected override CROBase GetOptions()
		{
			return (m_reportOptions as CROBase);

		}// protected override CROBase GetOptions()
		
		/// <summary>This method uses the items provided by the caller to populate the collection of scripts to be included in the report</summary>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>the number of reports to be run</returns>
		protected override int GetSourceRecords(bool bOnePerPrimary)
		{
			int iReports = 0;
			
			Debug.Assert(m_tmaxItems != null);
			Debug.Assert(m_tmaxItems.Count > 0);
			if(m_tmaxItems == null) return 0;
			if(m_tmaxItems.Count == 0) return 0;
			
			//	Clear the existing collection
			m_tmaxScripts.Clear();
			
			try
			{
				//	Check each item provided by the caller
				foreach(CTmaxItem O in m_tmaxItems)
				{
					//	Is this item associated with a media record?
					if(O.IPrimary != null)
					{
						//	Is this a script?
						if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Script)
						{
							//	Is this a playlist?
							if(((CDxPrimary)(O.IPrimary)).Playlist == true)
							{
								if(m_reportOptions.IncludePlaylists == true)
									Add((CDxPrimary)(O.IPrimary));
							}
							else
							{
								if(m_reportOptions.IncludeCustomShows == true)
									Add((CDxPrimary)(O.IPrimary));
							}
							
						}// if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Script)
					
					}// if(O.IPrimary != null)
					
						//	Is it a binder?
					else if(O.IBinderEntry != null)
					{
						//	Get the scripts in this binder
						GetScripts((CDxBinderEntry)(O.IBinderEntry));
					}
					
				}// foreach(CTmaxItem O in m_tmaxItems)
				
				//	Do we have any scripts to report on?
				if(m_tmaxScripts.Count == 0)
					MessageBox.Show("No scripts found using the current selections.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceRecords", m_tmaxErrorBuilder.Message(ERROR_GET_ITEM_SCRIPTS_EX), Ex);
			}
			
			//	How many reports do we want to run?
			if((m_tmaxScripts != null) && (m_tmaxScripts.Count > 0))
				iReports = (bOnePerPrimary == true) ? m_tmaxScripts.Count : 1;
				
			return iReports;
			
		}// protected override int GetSourceRecords()
		
		/// <summary>This method is called to populate the source data set</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected override bool FillDataSet(int iReportIndex, bool bOnePerPrimary)
		{
			string strFileSpec = "";
			
			Debug.Assert(m_dsReportSource != null);
			if(m_dsReportSource == null) return false;
			
			Debug.Assert(m_tmaxScripts != null);
			Debug.Assert(m_tmaxScripts.Count > 0);
			if(m_tmaxScripts == null) return false;
			if(m_tmaxScripts.Count == 0) return false;
			
			//	Reset the report counters
			m_iScriptIndex = 1;
			m_iClipIndex = 1;
			m_lAccumulatedTime = 0;
			m_lScriptTime = 0;
			
			//	Are we doing individual reports?
			if(bOnePerPrimary == true)
			{
				//	Is the caller specified value within range?
				if((iReportIndex >= 0) && (iReportIndex < m_tmaxScripts.Count))
				{
					try
					{
						AddSource(m_tmaxScripts[iReportIndex]);
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireError(this, "FillSource", m_tmaxErrorBuilder.Message(ERROR_FILL_SOURCE_EX, m_tmaxScripts[iReportIndex].IPrimary.GetBarcode(false)), Ex);
						return false;
					}

				}// if((iReportIndex >= 0) && (iReportIndex < m_tmaxScripts.Count))
				
			}
			else
			{
				//	Add each script to the data set
				foreach(CTmaxItem O in m_tmaxScripts)
				{
					try
					{
						AddSource(O);
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireError(this, "FillSource", m_tmaxErrorBuilder.Message(ERROR_FILL_SOURCE_EX, O.IPrimary.GetBarcode(false)), Ex);
						return false;
					}
				
				}// foreach(CTmaxItem O in m_tmaxScripts)

			}// if(bOnePerPrimary == true)
			
			if((Options.SaveData == true) && (Options.ShowSaveData == true))
			{
				try
				{
					if(bOnePerPrimary == true)
					{
						strFileSpec = System.IO.Path.GetDirectoryName(m_strTemplate);
						if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
							strFileSpec += "\\";
						strFileSpec += System.IO.Path.GetFileNameWithoutExtension(m_strTemplate);
						strFileSpec += ("_" + iReportIndex.ToString() + ".xml");
					}
					else
					{
						strFileSpec = System.IO.Path.ChangeExtension(m_strTemplate, "xml");
					}
					
					m_dsReportSource.WriteXml(strFileSpec);

				}
				catch
				{
				}
				
			}
				
			return true;

		}// protected override bool FillDataSet()
		
		/// <summary>This method is called to get the path to the export file for the specified report</summary>
		/// <param name="iReportIndex">The index of the report being exported</param>
		/// <param name="strFolder">The path to the folder where exported reports should be stored</param>
		/// <param name="eFormat">The format of the file being exported</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>The fully qualified path</returns>
		protected override string GetExportFileSpec(int iReportIndex, string strFolder, TmaxExportReportFormats eFormat, bool bOnePerPrimary)
		{
			ITmaxMediaRecord	tmaxRecord = null;
			string				strFileSpec = "";
			string				strFileName = "";
			int					iIndex = 0;
			
			if((m_tmaxScripts != null) && (m_tmaxScripts.Count > 0))
			{
				//	Are we doing individual reports?
				if(bOnePerPrimary == true)
				{
					//	Is the caller specified value within range?
					if((iReportIndex >= 0) && (iReportIndex < m_tmaxScripts.Count))
						iIndex = iReportIndex;
				}
			
				if((tmaxRecord = m_tmaxScripts[iIndex].GetMediaRecord()) != null)
				{
					strFileName += tmaxRecord.GetMediaId();
					
					//	Should we include the media name?
					if((Options.AddMediaName == true) && (tmaxRecord.GetName().Length > 0))
						strFileName += ("-" + tmaxRecord.GetName());
						
					strFileName += ("." + CROBase.GetExtension(eFormat));
					
					strFileSpec = strFolder;
					if(strFileSpec.EndsWith("\\") == false)
						strFileSpec += "\\";
					strFileSpec += CTmaxToolbox.CleanFilename(strFileName, false);

				}// if((tmaxRecord = m_tmaxScripts[iIndex].GetMediaRecord()) != null)
				
			}// if((m_tmaxScripts != null) && (m_tmaxScripts.Count > 0))
			
			return strFileSpec;
		
		}// protected virtual string GetExportFileSpec(int iReportIndex, string strFolder, TmaxExportReportFormats eFormat)
		
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
				m_ctrlViewer.Title = "Script Report";
				m_ctrlViewer.Template = m_strTemplate;
				m_ctrlViewer.ReportSource = m_dsReportSource;
				
				//	Should we enable the groups pane in the viewer?
				m_ctrlViewer.EnableGroups = (m_tmaxScripts.Count > 1);
				
				//	Should we suppress the transcript text?
				if(Options.IncludeTranscriptText == false)
					m_ctrlViewer.SuppressedSections.Add("DetailSection1");
					
				//	Attach to the form's event source
				m_ctrlViewer.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
				m_ctrlViewer.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
				m_ctrlViewer.TemplateLoaded += new System.EventHandler(this.OnTemplateLoaded);
				
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
		
		/// <summary>This method is called to add the specified script to the report queue</summary>
		/// <param name="dxScript">The script to be added</param>
		/// <returns>true if successful</returns>
		private bool Add(CDxPrimary dxScript)
		{
			CTmaxItem	tmaxScript = null;
			CTmaxItem	tmaxScene = null;
			double		dScriptDuration = 0;
			double		dSceneDuration = 0;
			
			try
			{
				Debug.Assert(dxScript != null);
				Debug.Assert(dxScript.MediaType == TmaxMediaTypes.Script);
				
				//	Make sure this script has been filled
				if((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
					dxScript.Fill();
							
				//	Create a new item using this script
				tmaxScript = new CTmaxItem(dxScript);
				
				//	Use the subitem collection to store references to all designations
				Debug.Assert(tmaxScript.SubItems != null);
				foreach(CDxSecondary dxScene in dxScript.Secondaries)
				{
					//	Make sure we can get to the source record
					if(dxScene.GetSource() == null) continue;
					
					//	Add a new item for this scene
					tmaxScene = new CTmaxItem(dxScene);
					tmaxScript.SubItems.Add(tmaxScene);
					
					//	Calculate the time required for this scene
					switch(dxScene.GetSource().MediaType)
					{
						case TmaxMediaTypes.Designation:
						case TmaxMediaTypes.Clip:
						case TmaxMediaTypes.Segment:
						
							dSceneDuration = dxScene.GetSource().GetDuration();
							break;
							
						default:
						
							dSceneDuration = 0;
							break;
					}
					
					if((dxScript.Playlist == false) &&
					   (dxScene.AutoTransition == true) && 
					   (dxScene.TransitionTime > 0))
					{
						dSceneDuration += dxScene.TransitionTime;
					}
					
					//	Save the duration for this scene so we can use it in the data source
					tmaxScene.UserData1 = System.Convert.ToInt64(dSceneDuration);
					
					//	keep track of time required for this script
					if(dSceneDuration > 0)
						dScriptDuration += dSceneDuration;
						
				}// foreach(CDxSecondary dxScene in dxScript.Secondaries)
						
				//	Should we add this script to the report?
				if(tmaxScript.SubItems.Count > 0)
				{
					m_tmaxScripts.Add(tmaxScript);
					
					//	Store the duration so we can use it later to generate
					//	the report
					tmaxScript.UserData1 = System.Convert.ToInt64(dScriptDuration);
				}

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX, dxScript.GetBarcode(false)), Ex);
				return false;
			}
			
		}// private bool Add(CDxPrimary dxScript)
		
		/// <summary>This called to get all scripts contained in the specified binder</summary>
		/// <returns>true if successful</returns>
		private bool GetScripts(CDxBinderEntry dxBinder)
		{
			try
			{
				//	Has it's child collection been filled
				if((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
					dxBinder.Fill();
							
				foreach(CDxBinderEntry O in dxBinder.Contents)
				{
					//	Is this a media node?
					if(O.IsMedia() == true)
					{
						//	Is this a script?
						if((O.GetSource(false) != null) && (O.GetSource(false).MediaType == TmaxMediaTypes.Script))
							Add((CDxPrimary)(O.GetSource(false)));
					}
					else
					{
						//	Should we recurse subfolders?
						if(Options.IncludeSubBinders == true)
							GetScripts(O);	
					}
							
				}// foreach(CDxBinderEntry O in dxBinder.Contents)
				
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetScripts", m_tmaxErrorBuilder.Message(ERROR_GET_BINDER_SCRIPTS_EX, dxBinder.Name), Ex);
				return false;
			}
			
		}// private bool GetScripts(CDxBinderEntry dxBinder)
		
		/// <summary>This method is called to add a script to the source data set</summary>
		/// <param name="tmaxScript">The item that identifies the script record</param>
		/// <returns>true if successful</returns>
		private bool AddSource(CTmaxItem tmaxScript)
		{
			Debug.Assert(tmaxScript.IPrimary != null);
			Debug.Assert(tmaxScript.SubItems != null);
			
			//	Set the times
			m_lScriptElapsed = 0;
			m_lScriptTime = (long)(tmaxScript.UserData1);
			m_lAccumulatedTime += m_lScriptTime;
			
			//	Create a new row for this script
			DataRow dr = m_dsReportSource.Tables[INFORMATION_TABLE_NAME].NewRow();
			
			//	Set the values
			dr["ScriptIndex"] = m_iScriptIndex;
			dr["Total_Time"] = CTmaxToolbox.SecondsToString(m_lScriptTime, 0);
			dr["Accumulated_Total_Time"] = CTmaxToolbox.SecondsToString(m_lAccumulatedTime, 0);

			if(tmaxScript.IPrimary.GetName().Length > 0)
				dr["Name"] = tmaxScript.IPrimary.GetName();
			else
				dr["Name"] = tmaxScript.IPrimary.GetBarcode(false); // MediaID
			
			//	Add the row
			m_dsReportSource.Tables[INFORMATION_TABLE_NAME].Rows.Add(dr);

			//	Add all designations
			foreach(CTmaxItem O in tmaxScript.SubItems)
			{
				Debug.Assert(O.MediaType == TmaxMediaTypes.Scene);
				Debug.Assert(O.ISecondary != null);
				Debug.Assert(O.UserData1 != null);
				
				if(AddSource((CDxSecondary)(O.ISecondary), (long)(O.UserData1)) == false)
					return false;
				
			}// foreach(CTmaxItem O in tmaxScript.SubItems)
			
			//	Bump the script index
			m_iScriptIndex++;
			
			return true;
			
		}// private bool AddSource(CTmaxItem tmaxScript)
		
		/// <summary>This method is called to add a scene to the source data set</summary>
		/// <param name="dxScene">The record exchange object for the scene to be added</param>
		/// <param name="lDuration">The previously computed time required for the scene</param>
		/// <returns>true if successful</returns>
		private bool AddSource(CDxSecondary dxScene, long lDuration)
		{
			CDxPrimary			dxScript = null;
			CDxTertiary			dxDesignation = null;
			CDxPrimary			dxDeposition = null;
			long				lRemaining = 0;
			string				strFilename = "";
			string				strStartPL = "";
			string				strStopPL = "";
			string				strTranscript = "";
				
			Debug.Assert(dxScene != null);
			
			//	Get the parent script
			Debug.Assert(dxScene.Primary != null);
			dxScript = dxScene.Primary;
			
			//	Update the time counts
			if((lRemaining = m_lScriptTime - m_lScriptElapsed) < lDuration)
			{
				//	This is just in case of rounding errors
				lRemaining = lDuration;
				m_lScriptElapsed = m_lScriptTime - lRemaining;
			}
			
			//	What type of source media are we dealing with?
			Debug.Assert(dxScene.GetSource() != null);
			switch(dxScene.GetSource().MediaType)
			{
				case TmaxMediaTypes.Designation:
				
					//	Get the source designation
					dxDesignation = (CDxTertiary)(dxScene.GetSource());
			
					//	Get the associated deposition
					Debug.Assert(dxDesignation.Secondary != null);
					Debug.Assert(dxDesignation.Secondary.Primary != null);
					dxDeposition = dxDesignation.Secondary.Primary;
			
					strTranscript = dxDeposition.Name;
					strStartPL = CTmaxToolbox.PLToString(dxDesignation.GetExtent().StartPL);
					strStopPL = CTmaxToolbox.PLToString(dxDesignation.GetExtent().StopPL);
					strFilename = dxDesignation.Secondary.GetFileName();
					break;
					
				case TmaxMediaTypes.Clip:
				
					strTranscript = dxScene.GetSource().GetBarcode(false);
					strStartPL = CTmaxToolbox.SecondsToString(((CDxTertiary)(dxScene.GetSource())).Start, 1);
					strStopPL = CTmaxToolbox.SecondsToString(((CDxTertiary)(dxScene.GetSource())).Stop, 1);
					strFilename = ((CDxTertiary)(dxScene.GetSource())).Secondary.GetFileName();
					break;
					
				default:
				
					strTranscript = dxScene.GetSource().GetBarcode(false);
					strFilename = dxScene.GetSource().GetFileName();
					break;
					
			}// switch(dxScene.GetSource().MediaType)
			
			//	Create a new row
			DataRow dr = m_dsReportSource.Tables[CLIP_TABLE_NAME].NewRow();
			
			//	Set the values
			dr["ClipIndex"] = m_iClipIndex;
			dr["Script"] = m_iScriptIndex;
			dr["Clip"] = dxScene.DisplayOrder;
			dr["Transcript"] = strTranscript;
			dr["StartPL"] = strStartPL;
			dr["StopPL"] = strStopPL;
			
			if(Options.Elapsed == true)
				dr["Elapsed"] = CTmaxToolbox.SecondsToString(m_lScriptElapsed, 0);
			else
				dr["Elapsed"] = "";
				
			if(Options.Remaining == true)
				dr["Remaining"] = CTmaxToolbox.SecondsToString(lRemaining, 0);
			else
				dr["Remaining"] = "";
			
			if(Options.BarcodeText == true)
				dr["Barcode"] = dxScene.GetBarcode(false);
			else
				dr["Barcode"] = "";
			
			if(Options.BarcodeGraphic == true)
				dr["Graphic"] = ("X" + (dxScene.GetBarcode(true)).ToUpper());
			else
				dr["Graphic"] = "";
			
			if(Options.MediaFile == true)
				dr["Filename"] = strFilename;
			else
				dr["Filename"] = "";
				
			if(Options.Duration == true)
				dr["Time"] = (lDuration > 0) ? CTmaxToolbox.SecondsToString(lDuration, 0) : "00:00:00";
			else
				dr["Time"] = "";
			
			//	Only show the transition indicators for custom shows
			if(dxScene.Primary.Playlist == false)
				dr["Transition"] = (dxScene.AutoTransition == true) ? "A" : "M";
			
			//	Add the row
			m_dsReportSource.Tables[CLIP_TABLE_NAME].Rows.Add(dr);
			
			//	Now add the transcript text
			if(dxDesignation != null)
			{
				AddSource(dxScene, dxDesignation);
			}
			else
			{
				//	This report requires us to add at least one line of 
				//	transcript text for each scene
				AddSource(null, false, null, -1);
			}
			
			//	Adjust the counters
			m_lScriptElapsed += lDuration;
			m_iClipIndex++;
			
			return true;
		
		}// private double AddSource(CDxSecondary dxScene)
		
		/// <summary>This method is called to add the text for the specified designation to the data set</summary>
		/// <param name="dxScene">The scene that references the designation</param>
		/// <param name="dxDesignation">The record exchange object for the designation</param>
		/// <returns>true if successful</returns>
		private bool AddSource(CDxSecondary dxScene, CDxTertiary dxDesignation)
		{
			CXmlDesignation	xmlDesignation = null;
			string			strFileSpec = "";
			string			strLinkBarcode = "";
			long			lPageNumber = -1;
			bool			bSetPage = false;
			int				iHighlighter = 0;
			CDxQuaternary	dxLink = null;
				
			//	What highlighter is assigned to this designation?
			//
			//	NOTE:	HighlighterId = (ZeroIndex + 1)
			iHighlighter = ((int)(dxDesignation.GetExtent().HighlighterId - 1));
			
			//	Get the fully qualified path to the XML file containing the transcript text
			strFileSpec = m_tmaxDatabase.GetFileSpec(dxDesignation);
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_XML_NOT_FOUND, dxScene.GetBarcode(false), strFileSpec));
				return false;
			}
			
			//	Load the file
			xmlDesignation = new CXmlDesignation();
			xmlDesignation.FastFill(strFileSpec, true, true);
				
			if((m_reportOptions.IncludeTranscriptText == true) &&
			   (xmlDesignation.Transcripts != null) && 
			   (xmlDesignation.Transcripts.Count > 0))
			{
				foreach(CXmlTranscript O in xmlDesignation.Transcripts)
				{
					//	Should we include the page number?
					if((lPageNumber < 0) || (lPageNumber != O.Page))
					{
						bSetPage = true;
						lPageNumber = O.Page;
					}
					
					//	Is this line linked?
					strLinkBarcode = "";
					if((dxLink = dxDesignation.GetLink(O.PL)) != null)
					{
						//	Is this a hidden link?
						if(dxLink.HideLink == true)
						{
							strLinkBarcode = "Hide";
						}
						else
						{
							//	Get the source record from the database
							if(dxLink.GetSource() != null)
								strLinkBarcode = dxLink.GetSource().GetBarcode(false);
						}
						
					}// if((iLink = xmlDesignation.Links.Locate(O.PL)) >= 0)

					if(AddSource(O, bSetPage, strLinkBarcode, iHighlighter) == false)
						return false;
						
				}// foreach(CXmlTranscript O in xmlDesignation.Transcripts)
			
			}
			else
			{
				//	This report requires us to add at least one line of 
				//	transcript text for each scene
				AddSource(null, false, null, iHighlighter);
			}
				
			//	Clean up
			xmlDesignation.Clear();
			xmlDesignation = null;
			
			return true;
		
		}// private bool AddSource(CDxSecondary dxScene, CDxTertiary dxDesignation)
		
		/// <summary>This method is called to add transcript text to the data set</summary>
		/// <param name="xmlTranscript">The transcript text to be added</param>
		/// <param name="bSetPage">true to assign the page number</param>
		/// <param name="strLinkBarcode">barcode of the link that occurs at this line</param>
		/// <param name="iHighlighter">index of the highlighter</param>
		/// <returns>true if successful</returns>
		private bool AddSource(CXmlTranscript xmlTranscript, bool bSetPage, string strLinkBarcode, int iHighlighter)
		{
			//	Create a new row for this script
			DataRow dr = m_dsReportSource.Tables[TRANSCRIPT_TABLE_NAME].NewRow();           
			
			//	Set the values
			dr["Clip"] = m_iClipIndex;
			
			if(xmlTranscript != null)
			{
				dr["PL"] = xmlTranscript.PL;
				dr["Line"] = xmlTranscript.Line;
				dr["LineText"] = xmlTranscript.GetFormattedText();
			}
			else
			{
				dr["LineText"] = "";
			}

            if ((Options.HighlightText == true) && (iHighlighter >= 0) && (iHighlighter < MAX_HIGHLIGHTERS))
            {
                dr["HColor"] = m_aHighlighters[iHighlighter];             
            }
            else
            {
                dr["HColor"] = DEFAULT_HIGHLIGHTER;              
            }

            if ((xmlTranscript != null) && (Options.HighlighterIndex == true) && (iHighlighter >= 0) && (iHighlighter < MAX_HIGHLIGHTERS))
            {
                dr["HName"] = m_aHighlightersNames[iHighlighter];
            }
            else
            {
                dr["HName"] = "";
            }

			if((xmlTranscript != null) && (bSetPage == true))
				dr["Page"] = (xmlTranscript.Page.ToString() + ":");
			else
				dr["Page"] = "";
			
			if((Options.IndicateLinks == true) && (xmlTranscript != null) && 
			   (strLinkBarcode != null) && (strLinkBarcode.Length > 0))
			{
				dr["Link"] = "L";
				dr["LinkBarcode"] = strLinkBarcode;
			}
			else
			{
				dr["Link"] = "";
				dr["LinkBarcode"] = "";
			}
			
			if((Options.IndicateEdited == true) && (xmlTranscript != null) && (xmlTranscript.Edited == true))
				dr["Edit"] = "E";
			else
				dr["Edit"] = "";
			
			//	Add the rows
			m_dsReportSource.Tables[TRANSCRIPT_TABLE_NAME].Rows.Add(dr);

            if ((xmlTranscript != null) && (Options.HighlighterIndex == true) && (iHighlighter >= 0) && (iHighlighter < MAX_HIGHLIGHTERS))
            {
                DataRow[] existingHighlighter = m_dsReportSource.Tables[HIGHLIGHTER_TABLE_NAME].Select("HName = '" + m_aHighlightersNames[iHighlighter] + "'");
                if (existingHighlighter == null || existingHighlighter.Length == 0)
                {
                    DataRow drHighlighter = m_dsReportSource.Tables[HIGHLIGHTER_TABLE_NAME].NewRow();
             //       drHighlighter["Clip"] = m_iClipIndex;
                    drHighlighter["HName"] = m_aHighlightersNames[iHighlighter];
                    drHighlighter["HColor"] = m_aHighlighters[iHighlighter];
                    m_dsReportSource.Tables[HIGHLIGHTER_TABLE_NAME].Rows.Add(drHighlighter);
                }
            }

			return true;
			
		}// private bool AddSource(CXmlTranscript xmlTranscript)
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRFScripts));
            this.m_ctrlOK = new System.Windows.Forms.Button();
            this.m_ctrlCancel = new System.Windows.Forms.Button();
            this.m_ctrlContentGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlIncludePlaylists = new System.Windows.Forms.CheckBox();
            this.m_ctrlIncludeCustomShows = new System.Windows.Forms.CheckBox();
            this.m_ctrlIncludeSubBinders = new System.Windows.Forms.CheckBox();
            this.m_ctrlIncludeTranscriptText = new System.Windows.Forms.CheckBox();
            this.m_ctrlIndicateEdited = new System.Windows.Forms.CheckBox();
            this.m_ctrlIndicateLinked = new System.Windows.Forms.CheckBox();
            this.m_ctrlHighlightText = new System.Windows.Forms.CheckBox();
            this.m_ctrlPageBreak = new System.Windows.Forms.CheckBox();
            this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
            this.m_ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlHighlighterIndex = new System.Windows.Forms.CheckBox();
            this.m_ctrlSceneNumber = new System.Windows.Forms.CheckBox();
            this.m_ctrlTimeTotals = new System.Windows.Forms.CheckBox();
            this.m_ctrlMediaFile = new System.Windows.Forms.CheckBox();
            this.m_ctrlRemaining = new System.Windows.Forms.CheckBox();
            this.m_ctrlElapsed = new System.Windows.Forms.CheckBox();
            this.m_ctrlBarcodeText = new System.Windows.Forms.CheckBox();
            this.m_ctrlBarcodeGraphic = new System.Windows.Forms.CheckBox();
            this.m_ctrlDuration = new System.Windows.Forms.CheckBox();
            this.m_ctrlStyleGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlAlternateLabel = new System.Windows.Forms.Label();
            this.m_ctrlTemplates = new System.Windows.Forms.ComboBox();
            this.m_ctrlAlternate = new System.Windows.Forms.TextBox();
            this.m_ctrlBrowse = new System.Windows.Forms.Button();
            this.m_ctrlSaveData = new System.Windows.Forms.CheckBox();
            this.m_ctrlExportGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlAddMediaName = new System.Windows.Forms.CheckBox();
            this.m_ctrlExportFolderLabel = new System.Windows.Forms.Label();
            this.m_ctrlExportFolder = new System.Windows.Forms.TextBox();
            this.m_ctrlBrowseExportFolder = new System.Windows.Forms.Button();
            this.m_ctrlFormatsLabel = new System.Windows.Forms.Label();
            this.m_ctrlExportFormats = new System.Windows.Forms.ComboBox();
            this.m_ctrlPreviewExports = new System.Windows.Forms.CheckBox();
            this.m_ctrlSplitExports = new System.Windows.Forms.CheckBox();
            this.m_ctrlContentGroup.SuspendLayout();
            this.m_ctrlOptionsGroup.SuspendLayout();
            this.m_ctrlStyleGroup.SuspendLayout();
            this.m_ctrlExportGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlOK
            // 
            this.m_ctrlOK.Location = new System.Drawing.Point(212, 478);
            this.m_ctrlOK.Name = "m_ctrlOK";
            this.m_ctrlOK.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlOK.TabIndex = 0;
            this.m_ctrlOK.Text = "&OK";
            // 
            // m_ctrlCancel
            // 
            this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_ctrlCancel.Location = new System.Drawing.Point(292, 478);
            this.m_ctrlCancel.Name = "m_ctrlCancel";
            this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlCancel.TabIndex = 1;
            this.m_ctrlCancel.Text = "&Cancel";
            // 
            // m_ctrlContentGroup
            // 
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludePlaylists);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeCustomShows);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeSubBinders);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeTranscriptText);
            this.m_ctrlContentGroup.Location = new System.Drawing.Point(8, 8);
            this.m_ctrlContentGroup.Name = "m_ctrlContentGroup";
            this.m_ctrlContentGroup.Size = new System.Drawing.Size(368, 64);
            this.m_ctrlContentGroup.TabIndex = 0;
            this.m_ctrlContentGroup.TabStop = false;
            this.m_ctrlContentGroup.Text = "Content";
            // 
            // m_ctrlIncludePlaylists
            // 
            this.m_ctrlIncludePlaylists.Checked = true;
            this.m_ctrlIncludePlaylists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlIncludePlaylists.Location = new System.Drawing.Point(8, 20);
            this.m_ctrlIncludePlaylists.Name = "m_ctrlIncludePlaylists";
            this.m_ctrlIncludePlaylists.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlIncludePlaylists.TabIndex = 0;
            this.m_ctrlIncludePlaylists.Text = "Include Playlists";
            // 
            // m_ctrlIncludeCustomShows
            // 
            this.m_ctrlIncludeCustomShows.Checked = true;
            this.m_ctrlIncludeCustomShows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlIncludeCustomShows.Location = new System.Drawing.Point(8, 40);
            this.m_ctrlIncludeCustomShows.Name = "m_ctrlIncludeCustomShows";
            this.m_ctrlIncludeCustomShows.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlIncludeCustomShows.TabIndex = 1;
            this.m_ctrlIncludeCustomShows.Text = "Include Custom Shows";
            // 
            // m_ctrlIncludeSubBinders
            // 
            this.m_ctrlIncludeSubBinders.Checked = true;
            this.m_ctrlIncludeSubBinders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlIncludeSubBinders.Location = new System.Drawing.Point(188, 40);
            this.m_ctrlIncludeSubBinders.Name = "m_ctrlIncludeSubBinders";
            this.m_ctrlIncludeSubBinders.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlIncludeSubBinders.TabIndex = 3;
            this.m_ctrlIncludeSubBinders.Text = "Include Sub-binders";
            // 
            // m_ctrlIncludeTranscriptText
            // 
            this.m_ctrlIncludeTranscriptText.Checked = true;
            this.m_ctrlIncludeTranscriptText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlIncludeTranscriptText.Location = new System.Drawing.Point(188, 20);
            this.m_ctrlIncludeTranscriptText.Name = "m_ctrlIncludeTranscriptText";
            this.m_ctrlIncludeTranscriptText.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlIncludeTranscriptText.TabIndex = 2;
            this.m_ctrlIncludeTranscriptText.Text = "Include Transcript Text";
            this.m_ctrlIncludeTranscriptText.Click += new System.EventHandler(this.OnClickShowText);
            // 
            // m_ctrlIndicateEdited
            // 
            this.m_ctrlIndicateEdited.Location = new System.Drawing.Point(8, 84);
            this.m_ctrlIndicateEdited.Name = "m_ctrlIndicateEdited";
            this.m_ctrlIndicateEdited.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlIndicateEdited.TabIndex = 3;
            this.m_ctrlIndicateEdited.Text = "Show edited text indicators";
            // 
            // m_ctrlIndicateLinked
            // 
            this.m_ctrlIndicateLinked.Location = new System.Drawing.Point(8, 64);
            this.m_ctrlIndicateLinked.Name = "m_ctrlIndicateLinked";
            this.m_ctrlIndicateLinked.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlIndicateLinked.TabIndex = 2;
            this.m_ctrlIndicateLinked.Text = "Show link indicators";
            // 
            // m_ctrlHighlightText
            // 
            this.m_ctrlHighlightText.Location = new System.Drawing.Point(8, 24);
            this.m_ctrlHighlightText.Name = "m_ctrlHighlightText";
            this.m_ctrlHighlightText.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlHighlightText.TabIndex = 0;
            this.m_ctrlHighlightText.Text = "Highlight transcript text";
            // 
            // m_ctrlPageBreak
            // 
            this.m_ctrlPageBreak.Location = new System.Drawing.Point(8, 44);
            this.m_ctrlPageBreak.Name = "m_ctrlPageBreak";
            this.m_ctrlPageBreak.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlPageBreak.TabIndex = 1;
            this.m_ctrlPageBreak.Text = "Break page after each script";
            // 
            // m_ctrlImages
            // 
            this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
            this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlImages.Images.SetKeyName(0, "");
            // 
            // m_ctrlOptionsGroup
            // 
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHighlighterIndex);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSceneNumber);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlTimeTotals);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlMediaFile);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlRemaining);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlElapsed);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlBarcodeText);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlBarcodeGraphic);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlIndicateEdited);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlIndicateLinked);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHighlightText);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlPageBreak);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlDuration);
            this.m_ctrlOptionsGroup.Location = new System.Drawing.Point(8, 180);
            this.m_ctrlOptionsGroup.Name = "m_ctrlOptionsGroup";
            this.m_ctrlOptionsGroup.Size = new System.Drawing.Size(368, 164);
            this.m_ctrlOptionsGroup.TabIndex = 2;
            this.m_ctrlOptionsGroup.TabStop = false;
            this.m_ctrlOptionsGroup.Text = "Options";
            // 
            // m_ctrlHighlighterIndex
            // 
            this.m_ctrlHighlighterIndex.Location = new System.Drawing.Point(8, 144);
            this.m_ctrlHighlighterIndex.Name = "m_ctrlHighlighterIndex";
            this.m_ctrlHighlighterIndex.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlHighlighterIndex.TabIndex = 12;
            this.m_ctrlHighlighterIndex.Text = "Show Highlighter Index";
            // 
            // m_ctrlSceneNumber
            // 
            this.m_ctrlSceneNumber.Location = new System.Drawing.Point(188, 124);
            this.m_ctrlSceneNumber.Name = "m_ctrlSceneNumber";
            this.m_ctrlSceneNumber.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlSceneNumber.TabIndex = 11;
            this.m_ctrlSceneNumber.Text = "Show scene numbers";
            // 
            // m_ctrlTimeTotals
            // 
            this.m_ctrlTimeTotals.Location = new System.Drawing.Point(188, 104);
            this.m_ctrlTimeTotals.Name = "m_ctrlTimeTotals";
            this.m_ctrlTimeTotals.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlTimeTotals.TabIndex = 10;
            this.m_ctrlTimeTotals.Text = "Show time totals";
            // 
            // m_ctrlMediaFile
            // 
            this.m_ctrlMediaFile.Location = new System.Drawing.Point(188, 24);
            this.m_ctrlMediaFile.Name = "m_ctrlMediaFile";
            this.m_ctrlMediaFile.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlMediaFile.TabIndex = 6;
            this.m_ctrlMediaFile.Text = "Show filename";
            // 
            // m_ctrlRemaining
            // 
            this.m_ctrlRemaining.Location = new System.Drawing.Point(188, 84);
            this.m_ctrlRemaining.Name = "m_ctrlRemaining";
            this.m_ctrlRemaining.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlRemaining.TabIndex = 9;
            this.m_ctrlRemaining.Text = "Show remaining time";
            // 
            // m_ctrlElapsed
            // 
            this.m_ctrlElapsed.Location = new System.Drawing.Point(188, 64);
            this.m_ctrlElapsed.Name = "m_ctrlElapsed";
            this.m_ctrlElapsed.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlElapsed.TabIndex = 8;
            this.m_ctrlElapsed.Text = "Show elapsed time";
            // 
            // m_ctrlBarcodeText
            // 
            this.m_ctrlBarcodeText.Location = new System.Drawing.Point(8, 104);
            this.m_ctrlBarcodeText.Name = "m_ctrlBarcodeText";
            this.m_ctrlBarcodeText.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlBarcodeText.TabIndex = 4;
            this.m_ctrlBarcodeText.Text = "Show barcode text";
            // 
            // m_ctrlBarcodeGraphic
            // 
            this.m_ctrlBarcodeGraphic.Location = new System.Drawing.Point(8, 124);
            this.m_ctrlBarcodeGraphic.Name = "m_ctrlBarcodeGraphic";
            this.m_ctrlBarcodeGraphic.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlBarcodeGraphic.TabIndex = 5;
            this.m_ctrlBarcodeGraphic.Text = "Show barcode graphic";
            // 
            // m_ctrlDuration
            // 
            this.m_ctrlDuration.Location = new System.Drawing.Point(188, 44);
            this.m_ctrlDuration.Name = "m_ctrlDuration";
            this.m_ctrlDuration.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlDuration.TabIndex = 7;
            this.m_ctrlDuration.Text = "Show duration";
            // 
            // m_ctrlStyleGroup
            // 
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlAlternateLabel);
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlTemplates);
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlAlternate);
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlBrowse);
            this.m_ctrlStyleGroup.Location = new System.Drawing.Point(8, 80);
            this.m_ctrlStyleGroup.Name = "m_ctrlStyleGroup";
            this.m_ctrlStyleGroup.Size = new System.Drawing.Size(368, 92);
            this.m_ctrlStyleGroup.TabIndex = 1;
            this.m_ctrlStyleGroup.TabStop = false;
            this.m_ctrlStyleGroup.Text = "Style";
            // 
            // m_ctrlAlternateLabel
            // 
            this.m_ctrlAlternateLabel.Location = new System.Drawing.Point(8, 48);
            this.m_ctrlAlternateLabel.Name = "m_ctrlAlternateLabel";
            this.m_ctrlAlternateLabel.Size = new System.Drawing.Size(312, 16);
            this.m_ctrlAlternateLabel.TabIndex = 3;
            this.m_ctrlAlternateLabel.Text = "Alternate Report Template";
            this.m_ctrlAlternateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // m_ctrlTemplates
            // 
            this.m_ctrlTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ctrlTemplates.Location = new System.Drawing.Point(8, 20);
            this.m_ctrlTemplates.Name = "m_ctrlTemplates";
            this.m_ctrlTemplates.Size = new System.Drawing.Size(352, 21);
            this.m_ctrlTemplates.TabIndex = 0;
            // 
            // m_ctrlAlternate
            // 
            this.m_ctrlAlternate.Location = new System.Drawing.Point(6, 64);
            this.m_ctrlAlternate.Name = "m_ctrlAlternate";
            this.m_ctrlAlternate.Size = new System.Drawing.Size(326, 20);
            this.m_ctrlAlternate.TabIndex = 1;
            // 
            // m_ctrlBrowse
            // 
            this.m_ctrlBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.m_ctrlBrowse.ImageIndex = 0;
            this.m_ctrlBrowse.ImageList = this.m_ctrlImages;
            this.m_ctrlBrowse.Location = new System.Drawing.Point(336, 64);
            this.m_ctrlBrowse.Name = "m_ctrlBrowse";
            this.m_ctrlBrowse.Size = new System.Drawing.Size(24, 20);
            this.m_ctrlBrowse.TabIndex = 2;
            this.m_ctrlBrowse.Click += new System.EventHandler(this.OnClickBrowse);
            // 
            // m_ctrlSaveData
            // 
            this.m_ctrlSaveData.Location = new System.Drawing.Point(20, 482);
            this.m_ctrlSaveData.Name = "m_ctrlSaveData";
            this.m_ctrlSaveData.Size = new System.Drawing.Size(92, 16);
            this.m_ctrlSaveData.TabIndex = 7;
            this.m_ctrlSaveData.Text = "Save Data";
            // 
            // m_ctrlExportGroup
            // 
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlAddMediaName);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlExportFolderLabel);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlExportFolder);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlBrowseExportFolder);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlFormatsLabel);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlExportFormats);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlPreviewExports);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlSplitExports);
            this.m_ctrlExportGroup.Location = new System.Drawing.Point(8, 350);
            this.m_ctrlExportGroup.Name = "m_ctrlExportGroup";
            this.m_ctrlExportGroup.Size = new System.Drawing.Size(368, 120);
            this.m_ctrlExportGroup.TabIndex = 8;
            this.m_ctrlExportGroup.TabStop = false;
            this.m_ctrlExportGroup.Text = "Export";
            // 
            // m_ctrlAddMediaName
            // 
            this.m_ctrlAddMediaName.Location = new System.Drawing.Point(188, 60);
            this.m_ctrlAddMediaName.Name = "m_ctrlAddMediaName";
            this.m_ctrlAddMediaName.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlAddMediaName.TabIndex = 7;
            this.m_ctrlAddMediaName.Text = "Add Media Name to filename";
            // 
            // m_ctrlExportFolderLabel
            // 
            this.m_ctrlExportFolderLabel.Location = new System.Drawing.Point(6, 76);
            this.m_ctrlExportFolderLabel.Name = "m_ctrlExportFolderLabel";
            this.m_ctrlExportFolderLabel.Size = new System.Drawing.Size(342, 16);
            this.m_ctrlExportFolderLabel.TabIndex = 6;
            this.m_ctrlExportFolderLabel.Text = "Target Folder";
            this.m_ctrlExportFolderLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // m_ctrlExportFolder
            // 
            this.m_ctrlExportFolder.Location = new System.Drawing.Point(6, 92);
            this.m_ctrlExportFolder.Name = "m_ctrlExportFolder";
            this.m_ctrlExportFolder.Size = new System.Drawing.Size(326, 20);
            this.m_ctrlExportFolder.TabIndex = 4;
            // 
            // m_ctrlBrowseExportFolder
            // 
            this.m_ctrlBrowseExportFolder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.m_ctrlBrowseExportFolder.ImageIndex = 0;
            this.m_ctrlBrowseExportFolder.ImageList = this.m_ctrlImages;
            this.m_ctrlBrowseExportFolder.Location = new System.Drawing.Point(336, 92);
            this.m_ctrlBrowseExportFolder.Name = "m_ctrlBrowseExportFolder";
            this.m_ctrlBrowseExportFolder.Size = new System.Drawing.Size(24, 20);
            this.m_ctrlBrowseExportFolder.TabIndex = 5;
            this.m_ctrlBrowseExportFolder.Click += new System.EventHandler(this.OnBrowseExportFolder);
            // 
            // m_ctrlFormatsLabel
            // 
            this.m_ctrlFormatsLabel.Location = new System.Drawing.Point(8, 20);
            this.m_ctrlFormatsLabel.Name = "m_ctrlFormatsLabel";
            this.m_ctrlFormatsLabel.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlFormatsLabel.TabIndex = 3;
            this.m_ctrlFormatsLabel.Text = "Format";
            // 
            // m_ctrlExportFormats
            // 
            this.m_ctrlExportFormats.Location = new System.Drawing.Point(8, 36);
            this.m_ctrlExportFormats.Name = "m_ctrlExportFormats";
            this.m_ctrlExportFormats.Size = new System.Drawing.Size(172, 21);
            this.m_ctrlExportFormats.TabIndex = 0;
            this.m_ctrlExportFormats.Text = "comboBox1";
            this.m_ctrlExportFormats.SelectedIndexChanged += new System.EventHandler(this.OnExportFormatsSelChanged);
            // 
            // m_ctrlPreviewExports
            // 
            this.m_ctrlPreviewExports.Location = new System.Drawing.Point(188, 40);
            this.m_ctrlPreviewExports.Name = "m_ctrlPreviewExports";
            this.m_ctrlPreviewExports.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlPreviewExports.TabIndex = 2;
            this.m_ctrlPreviewExports.Text = "Preview";
            // 
            // m_ctrlSplitExports
            // 
            this.m_ctrlSplitExports.Location = new System.Drawing.Point(188, 20);
            this.m_ctrlSplitExports.Name = "m_ctrlSplitExports";
            this.m_ctrlSplitExports.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlSplitExports.TabIndex = 1;
            this.m_ctrlSplitExports.Text = "One file per script";
            // 
            // CRFScripts
            // 
            this.AcceptButton = this.m_ctrlOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_ctrlCancel;
            this.ClientSize = new System.Drawing.Size(382, 508);
            this.ControlBox = false;
            this.Controls.Add(this.m_ctrlExportGroup);
            this.Controls.Add(this.m_ctrlSaveData);
            this.Controls.Add(this.m_ctrlStyleGroup);
            this.Controls.Add(this.m_ctrlOptionsGroup);
            this.Controls.Add(this.m_ctrlContentGroup);
            this.Controls.Add(this.m_ctrlCancel);
            this.Controls.Add(this.m_ctrlOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CRFScripts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scripts Report";
            this.m_ctrlContentGroup.ResumeLayout(false);
            this.m_ctrlOptionsGroup.ResumeLayout(false);
            this.m_ctrlStyleGroup.ResumeLayout(false);
            this.m_ctrlStyleGroup.PerformLayout();
            this.m_ctrlExportGroup.ResumeLayout(false);
            this.m_ctrlExportGroup.PerformLayout();
            this.ResumeLayout(false);

		}
		
		/// <summary>This method handles the event fired when the user clicks on the Show Text check box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickShowText(object sender, System.EventArgs e)
		{
			m_ctrlHighlightText.Enabled = m_ctrlIncludeTranscriptText.Checked;
            m_ctrlHighlighterIndex.Enabled = m_ctrlIncludeTranscriptText.Checked;
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

		/// <summary>This method handles the event fired when the preview form loads the report template</summary>
		/// <param name="sender">The preview form sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnTemplateLoaded(object sender, System.EventArgs e)
		{
			try
			{
				CRFViewer			preview = (CRFViewer)sender;
				Area				area = null;
//				Section				section = null;
//				FieldObject			foLinkBarcode = null;
//				FieldObject			foLineText = null;
				TextObject			column = null;
				FieldObject			field = null;
				FieldHeadingObject	fieldHeading = null;

				//	Do we need to turn off the page break?
				if((Options.PageBreak == false) || (Options.TimeTotals == false))
				{
					if((area = preview.GetArea(preview.Document, "GroupFooterArea1")) != null)
					{
						if(area.AreaFormat != null)
						{
							if(Options.PageBreak == false)
								area.AreaFormat.EnableNewPageAfter = false;
							if(Options.TimeTotals == false)
								area.AreaFormat.EnableSuppress = true;
						}

					}// if((area = preview.GetArea(preview.Document, "GroupFooterArea1")) != null)

				}// if((Options.PageBreak == false) || (Options.TimeTotals == false))
			
				//	Has the user turned off the barcode graphic
				if(Options.BarcodeGraphic == false)
				{
					if((area = preview.GetArea(preview.Document, "GroupHeaderArea2")) != null)
					{
						foreach(Section O in area.Sections)
						{
							if(String.Compare(O.Name, "GroupHeaderSection3", true) == 0)
							{
								O.SectionFormat.EnableSuppress = true;
								break;
							}
			
						}
					
					}// if((area = preview.GetArea(preview.Document, "GroupHeaderArea2")) != null)
					
				}// if(Options.BarcodeGraphic == false)

				//	Do we need to suppress the scene number field?
				//if(Options.SceneNumber == false)
				//{
				//    if((area = preview.GetArea(preview.Document, "GroupHeaderArea2")) != null)
				//    {
				//        foreach(Section O in area.Sections)
				//        {
				//            if(String.Compare(O.Name, "GroupHeaderSection2", true) == 0)
				//            {
				//                foreach(CrystalDecisions.CrystalReports.Engine.ReportObject ro in O.ReportObjects)
				//                {
				//                    if(ro.Kind == ReportObjectKind.FieldObject)
				//                    {
				//                        field = (FieldObject)ro;
				//                        switch(field.Name)
				//                        {
				//                            case "Clip1":
												
				//                                field.ObjectFormat.EnableSuppress = true;
				//                                break;
				//                        }
										
				//                    }
				//                }
								
				//                break;
				//            }

				//        }

				//    }// if((area = preview.GetArea(preview.Document, "GroupHeaderArea2")) != null)

				//}// if(Options.BarcodeGraphic == false)

				if(Options.SceneNumber == false)
				{
					if((field = preview.GetField("GroupHeaderSection2", "Clip1")) != null)
					{
						field.ObjectFormat.EnableSuppress = true;

					}

				}// iif(Options.SceneNumber == false)

				//	Make sure only the required column headers are turned on
				if((area = preview.GetArea(preview.Document, "GroupHeaderArea1")) != null)
				{
					foreach(Section O in area.Sections)
					{
						if(String.Compare(O.Name, "GroupHeaderSection1", true) == 0)
						{
							foreach(CrystalDecisions.CrystalReports.Engine.ReportObject ro in O.ReportObjects)
							{
								if(ro.Kind == ReportObjectKind.TextObject)
								{
									try
									{
										column = (CrystalDecisions.CrystalReports.Engine.TextObject)ro;

										switch(column.Name)
										{
											case "DurationColumn":
											
												if(Options.Duration == false)
													column.Text = "";
												break;
												
											case "ElapsedColumn":
											
												if(Options.Elapsed == false)
													column.Text = "";
												break;
											
											case "RemainsColumn":
											
												if(Options.Remaining == false)
													column.Text = "";
												break;
												
											case "MediaFileColumn":
											
												if(Options.MediaFile == false)
													column.Text = "";
												break;
												
											case "BarcodeColumn":
											
												if(Options.BarcodeText == false)
													column.Text = "";
												break;

										}// switch(column.Name)

									}
									catch
									{
									}
									
								}// if(ro.Kind == ReportObjectKind.TextObject)
							
								else if(ro.Kind == ReportObjectKind.FieldHeadingObject)
								{
									try
									{
										fieldHeading = (CrystalDecisions.CrystalReports.Engine.FieldHeadingObject)ro;

										switch(fieldHeading.Name)
										{
											case "SceneNumberColumn":
												
												if(Options.SceneNumber == false)
												{
													fieldHeading.Text = "";
													fieldHeading.Width = 0;
												}
												break;

										}// switch(fieldHeading.Name)

									}
									catch
									{
									}
								
								}// if(ro.Kind == ReportObjectKind.FieldHeadingObject)
								
							}// foreach(CrystalDecisions.CrystalReports.Engine.ReportObject ro in O.ReportObjects)
							
							break;
							
						}// if(String.Compare(O.Name, "GroupHeaderSection1", true) == 0)
					
					}// foreach(Section O in area.Sections)
					
				}// if(Options.BarcodeGraphic == false)

//				//	Do we need to turn off the link indicators
//				if(Options.IndicateLinks == false)
//				{
//					if((section = preview.GetSection(preview.Document, "DetailSection1")) != null)
//					{
//						foreach(CrystalDecisions.CrystalReports.Engine.FieldObject fo in section.ReportObjects)
//						{
//							if(fo.Name == "LinkBarcode1")
//							{
//								foLinkBarcode = fo;
//								
//							}
//							else if(fo.Name == "LineText1")
//							{
//								foLineText = fo;
//							}
//							
//							if((foLineText != null) && (foLinkBarcode != null))
//								break;
//						}
//						
//						if((foLineText != null) && (foLinkBarcode != null))
//						{
//							foLineText.Left = foLinkBarcode.Left;
//							foLinkBarcode.ObjectFormat.EnableSuppress = true;
//						}	
//					}
//					
//				}
			
			}
			catch
			{
			}
			
		}// private void OnTemplateLoaded(object document, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>User defined options for generating the report</summary>
		public CROScripts Options
		{
			get { return m_reportOptions; }
			set { m_reportOptions = value; }
		}
		
		#endregion Properties

	}// public class CRFScripts : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Reports
