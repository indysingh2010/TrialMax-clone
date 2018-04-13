using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.Data.OleDb;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Reports
{
	/// <summary>This form allows the user to create a Transcript report</summary>
	public class CRFTranscript : CRFBase
	{
		#region Constants
		
		private const int MAX_HIGHLIGHTERS = 7;
		
		private const int SMALL_FONT_SIZE = 8;
		private const int MEDIUM_FONT_SIZE = 9;
		private const int DEFAULT_HIGHLIGHTER = 0xFFFFFF;
		
		private const string TRANSCRIPT_TABLE_NAME  = "Transcript";
		private const string INFORMATION_TABLE_NAME = "Information";
        private const string HIGHLIGHTER_TABLE_NAME = "HighlighterDetail";
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_SET_TRANSCRIPT_EX			= (ERROR_RFBASE_MAX + 1);
		protected const int ERROR_PREPARE_EX				= (ERROR_RFBASE_MAX + 2);
		protected const int ERROR_ADD_SOURCE_TEXT_EX		= (ERROR_RFBASE_MAX + 3);
		protected const int ERROR_FILL_INFORMATION_EX		= (ERROR_RFBASE_MAX + 4);
		protected const int ERROR_FILL_OPTIONS_EX			= (ERROR_RFBASE_MAX + 5);
		protected const int ERROR_FILL_TRANSCRIPT_EX		= (ERROR_RFBASE_MAX + 6);
		protected const int ERROR_SET_SOURCE_HIGHLIGHT_EX	= (ERROR_RFBASE_MAX + 7);
		protected const int ERROR_EXCHANGE_EX				= (ERROR_RFBASE_MAX + 8);
		protected const int ERROR_GET_SOURCE_FROM_MEDIA_EX	= (ERROR_RFBASE_MAX + 9);
		protected const int ERROR_GET_SOURCE_FROM_BINDER_EX	= (ERROR_RFBASE_MAX + 10);
		protected const int ERROR_GET_SOURCE_RECORDS_EX		= (ERROR_RFBASE_MAX + 11);
		
		#endregion Constants
		
		#region Private Members

		private System.ComponentModel.IContainer components;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Exchange interfaces for all scenes in the script</summary>
		private FTI.Trialmax.Database.CDxSecondaries m_dxScenes = new CDxSecondaries();
		
		/// <summary>Exchange interfaces for all scripts in use</summary>
		private FTI.Trialmax.Database.CDxPrimaries m_dxScripts = new CDxPrimaries();
		
		/// <summary>Exchange interfaces for all depositions referenced by the script</summary>
		private FTI.Trialmax.Database.CDxPrimaries m_dxDepositions = new CDxPrimaries();
		
		/// <summary>The active deposition</summary>
		private FTI.Trialmax.Database.CDxPrimary m_dxDeposition = null;
		
		/// <summary>The active transcript</summary>
		private FTI.Trialmax.Database.CDxTranscript m_dxTranscript = null;
		
		/// <summary>The source groups used to build the reports</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxSourceGroups = new CTmaxItems();
		
		/// <summary>The source groups actually used for the reports</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxReportGroups = new CTmaxItems();
		
		/// <summary>The source XML transcript</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = null;
		
		/// <summary>Group box for report options</summary>
		private System.Windows.Forms.GroupBox m_ctrlOptionsGroup;
		
		/// <summary>Check box to select bold designations</summary>
		private System.Windows.Forms.CheckBox m_ctrlBoldDesignations;
		
		/// <summary>Check box to select vertical highlights</summary>
		private System.Windows.Forms.CheckBox m_ctrlVerticalHighlights;
		
		/// <summary>Check box to select header on first page</summary>
		private System.Windows.Forms.CheckBox m_ctrlPageHeaderFirst;
		
		/// <summary>Check box to select header on subsequent pages</summary>
		private System.Windows.Forms.CheckBox m_ctrlPageHeaderSubsequent;
		
		/// <summary>Check box to select page footers</summary>
		private System.Windows.Forms.CheckBox m_ctrlPageFooter;
		
		/// <summary>Check box to select medium font</summary>
		private System.Windows.Forms.CheckBox m_ctrlMediumFont;
		
		/// <summary>Local member to store the font size for the report</summary>
		private int m_iFontSize = SMALL_FONT_SIZE;
		
		/// <summary>Image list for browse button</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Array of highlighter colors</summary>
		private int [] m_aHighlighters = new int[MAX_HIGHLIGHTERS];

        /// <summary>Array of highlighter name</summary>
        private string[] m_aHighlightersNames = new string[MAX_HIGHLIGHTERS];
		
		/// <summary>Debugging check box to save data set to file</summary>
		private System.Windows.Forms.CheckBox m_ctrlSaveData;
		
		/// <summary>Group box for options that control report content</summary>
		private System.Windows.Forms.GroupBox m_ctrlContentGroup;
		
		/// <summary>Static text label for transcripts check list</summary>
		private System.Windows.Forms.Label m_ctrlTranscriptsLabel;
		
		/// <summary>Static text label to display the scripts being processed</summary>
		private System.Windows.Forms.Label m_ctrlScript;
		
		/// <summary>Static text label for script id string</summary>
		private System.Windows.Forms.Label m_ctrlScriptLabel;
		
		/// <summary>Group box for list of available report styles</summary>
		private System.Windows.Forms.GroupBox m_ctrlStyleGroup;
		
		/// <summary>Static text label for alternate edit box</summary>
		private System.Windows.Forms.Label m_ctrlAlternateLabel;
		
		/// <summary>Drop list of available report templates</summary>
		private System.Windows.Forms.ComboBox m_ctrlTemplates;
		
		/// <summary>Edit box to enter path to an alternate report template</summary>
		private System.Windows.Forms.TextBox m_ctrlAlternate;
		
		/// <summary>Browse button for alternate template</summary>
		private System.Windows.Forms.Button m_ctrlBrowse;
		
		/// <summary>Check list of available transcripts</summary>
		private System.Windows.Forms.CheckedListBox m_ctrlTranscripts;
		private System.Windows.Forms.GroupBox m_ctrlExportGroup;
		private System.Windows.Forms.Label m_ctrlExportFolderLabel;
		private System.Windows.Forms.TextBox m_ctrlExportFolder;
		private System.Windows.Forms.Button m_ctrlBrowseExportFolder;
		private System.Windows.Forms.ComboBox m_ctrlExportFormats;
		private System.Windows.Forms.CheckBox m_ctrlPreviewExports;
        private CheckBox m_ctrlShowEditText;
        private CheckBox m_ctrlShowHighlighterLabel;
		
		/// <summary>Options for generating the report</summary>
		private CROTranscript m_reportOptions = null;
		
		#endregion Private Members
		
		#region Public Methods

		/// <summary>Constructor</summary>
		public CRFTranscript() : base()
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
			
			Debug.Assert(m_tmaxSourceGroups != null);
			Debug.Assert(m_dxDepositions != null);
			Debug.Assert(m_dxScripts != null);
			if(m_tmaxSourceGroups == null) return false;
			if(m_dxDepositions == null) return false;
			if(m_dxScripts == null) return false;
			
			m_dxDepositions.Clear();
			m_dxScripts.Clear();
			m_tmaxSourceGroups.Clear();
			m_tmaxReportGroups.Clear();
			
			//	Get the records we need to run the report
			if((m_tmaxItems != null) && (m_tmaxItems.Count > 0))
			{
				foreach(CTmaxItem O in m_tmaxItems)
				{
					//	Is this a media record?
					if(O.IPrimary != null)
					{
						//	Only process scripts and depositions
						switch(O.MediaType)
						{
							case TmaxMediaTypes.Script:
							case TmaxMediaTypes.Deposition:
							
								GetSourceFromMedia((CDxPrimary)(O.IPrimary));
								break;
						}
						
					}
					else if(O.IBinderEntry != null)
					{
						GetSourceFromBinder((CDxBinderEntry)(O.IBinderEntry));
					}

				}// foreach(CTmaxItem O in m_tmaxItems)

			}// if((m_tmaxItems != null) && (m_tmaxItems.Count > 0))
			else
			{
				MessageBox.Show("No records have been selected for the report");
				return false;
			}
			
			//	Do we have any depositions to report on?
			if((m_dxDepositions == null) || (m_dxDepositions.Count == 0))
			{
				MessageBox.Show("The selected records do not reference any registered depositions.", "Error", 
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
		
		/// <summary>This method is called to get the options for the report</summary>
		/// <returns>The options object associated with the report</returns>
		/// <remarks>This MUST be overridden by the derived class</remarks>
		protected override CROBase GetOptions()
		{
			return (m_reportOptions as CROBase);

		}// protected override CROBase GetOptions()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the report transcript");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prepare the report for execution");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding a row to the source Transcript table");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the Information table");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the Options table");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the Transcript table");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to highlight a designation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exchanging the report options");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving source groups for the primary media: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving source groups for the binder: name = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the source records for the report");

		}// protected override void SetErrorStrings()

		/// <summary>Overloaded base class member to do custom initialization when the form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Get the highlighter colors
            for (int i = 0; i < MAX_HIGHLIGHTERS; i++)
            {
                m_aHighlighters[i] = GetOleHighlighter(i);
                m_aHighlightersNames[i] = GetHighlighterName(i);
            }	
			//	Set the control values
			Exchange(true);
			
			//	Populate the transcripts list box
			FillTranscripts();
			
			//	Set the script name
			if((m_dxScripts != null) && (m_dxScripts.Count > 0))
			{
				m_ctrlScript.Text = "";
				foreach(CDxPrimary O in m_dxScripts)
				{
					if(m_ctrlScript.Text.Length > 0)
						m_ctrlScript.Text += ",";
					m_ctrlScript.Text += O.MediaId;
				}

			}
			else
			{
				m_ctrlScript.Text = "";
				m_ctrlScriptLabel.Enabled = false;
				m_ctrlVerticalHighlights.Enabled = false;
				m_ctrlVerticalHighlights.Checked = false;
				m_ctrlBoldDesignations.Enabled = false;
				m_ctrlBoldDesignations.Checked = false;
			}
			
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
				
				if(m_dxDepositions != null)
					m_dxDepositions.Clear();
					
				if(m_dxScenes != null)
					m_dxScenes.Clear();
					
			}
			base.Dispose( disposing );
		
		}// protected override void Dispose( bool disposing )
		
		/// <summary>This method uses the items provided by the caller to populate the collection of scripts to be included in the report</summary>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>the number of reports to be run</returns>
		protected override int GetSourceRecords(bool bOnePerPrimary)
		{
			CDxPrimaries dxDepositions = null;
			
			Debug.Assert(m_tmaxSourceGroups != null);
			Debug.Assert(m_tmaxSourceGroups.Count > 0);
			if(m_tmaxSourceGroups == null) return 0;
			if(m_tmaxSourceGroups.Count == 0) return 0;
			
			Debug.Assert(m_dxDepositions != null);
			Debug.Assert(m_dxDepositions.Count > 0);
			if(m_dxDepositions == null) return 0;
			if(m_dxDepositions.Count == 0) return 0;
			
			try
			{
				//	Clear the reports collection
				m_tmaxReportGroups.Clear();
				
				//	Get the collection of selected depositions
				dxDepositions = new CDxPrimaries();
				for(int i = 0; i < m_ctrlTranscripts.Items.Count; i++)
				{
					if(m_ctrlTranscripts.GetItemChecked(i) == true)
						dxDepositions.AddList(m_dxDepositions[i]);
				}
				
				if(dxDepositions.Count > 0)
				{
					//	Now extract the groups that match the selected transcripts
					foreach(CTmaxItem O in m_tmaxSourceGroups)
					{
						//	Does this group use one of the selected depositions?
						if(dxDepositions.Contains((CDxPrimary)(O.UserData1)) == true)
							m_tmaxReportGroups.Add(O);
					}
					
					//	Warn the user if no report groups (this shouldn't happen but ...)
					if(m_tmaxReportGroups.Count == 0)
						MessageBox.Show("No records are available for the report", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					
				}
				else
				{
					//	Warn the user that no transcripts have been selected
					MessageBox.Show("You must select one or more transcripts for the report", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceRecords", m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_RECORDS_EX), Ex);
			}
			
			return m_tmaxReportGroups.Count;
			
		}// protected override int GetSourceRecords()
		
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
				
					m_ctrlBoldDesignations.Checked = Options.BoldDesignations;
					m_ctrlVerticalHighlights.Checked = Options.VerticalHighlights;
					m_ctrlPageHeaderFirst.Checked = Options.PageHeaderFirst;
					m_ctrlPageHeaderSubsequent.Checked = Options.PageHeaderSubsequent;
					m_ctrlPageFooter.Checked = Options.PageFooter;
					m_ctrlMediumFont.Checked = Options.MediumFont;
                    m_ctrlShowEditText.Checked = Options.ShowEditedText;
					m_ctrlSaveData.Checked = Options.SaveData;
					m_ctrlAlternate.Text = Options.Alternate;
					
					m_ctrlExportFolder.Text = Options.ExportFolder;
					m_ctrlPreviewExports.Checked = Options.PreviewExports;
                    m_ctrlShowHighlighterLabel.Checked = Options.ShowHighlighterLabel;
					
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
				
					m_ctrlPageHeaderSubsequent.Enabled = m_ctrlPageHeaderFirst.Checked;
				
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
					
					Options.BoldDesignations = m_ctrlBoldDesignations.Checked;
					Options.VerticalHighlights = m_ctrlVerticalHighlights.Checked;
					Options.PageHeaderFirst = m_ctrlPageHeaderFirst.Checked;
					Options.PageHeaderSubsequent = m_ctrlPageHeaderSubsequent.Checked;
					Options.PageFooter = m_ctrlPageFooter.Checked;
					Options.MediumFont = m_ctrlMediumFont.Checked;
                    Options.ShowEditedText = m_ctrlShowEditText.Checked;
					Options.Template = m_ctrlTemplates.SelectedIndex + 1;
					Options.SaveData = m_ctrlSaveData.Checked;
					Options.Alternate = m_ctrlAlternate.Text;
					Options.ExportFormat = GetExportFormat(m_ctrlExportFormats);
					Options.ExportFolder = m_ctrlExportFolder.Text;
					Options.PreviewExports = m_ctrlPreviewExports.Checked;
                    Options.ShowHighlighterLabel = m_ctrlShowHighlighterLabel.Checked;
					
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
				
				m_iFontSize = (Options.MediumFont == true) ? MEDIUM_FONT_SIZE : SMALL_FONT_SIZE;
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX), Ex);
				return false;
			}
			
		}// protected override bool Exchange(bool bSetControls)
		
		/// <summary>This method is called to populate the source data set</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected override bool FillDataSet(int iReportIndex, bool bOnePerPrimary)
		{
			CTmaxItem tmaxSource = null;
			
			m_dxDeposition = null;
			m_dxTranscript = null;
			m_xmlDeposition = null;
			m_dxScenes.Clear();
			
			//	Get the specified source group
			if((tmaxSource = m_tmaxReportGroups[iReportIndex]) == null) 
				return false;
				
			//	Set the active deposition
			if(SetTranscript((CDxPrimary)(tmaxSource.UserData1)) == false)
				return false;

			//	Add the scenes if there are any
			if(tmaxSource.SubItems.Count > 0)
			{
				foreach(CTmaxItem O in tmaxSource.SubItems)
				{
					m_dxScenes.AddList((CDxSecondary)(O.GetMediaRecord()));
				}
			}
			
			//	Fill the information table
			if(FillSourceInformation() == false) return false;
				
			//	Fill the transcript table
			if(FillSourceTranscript() == false) return false;
				
			//	Highlight the scenes
			if(SetSourceHighlights() == false) return false;

            //Show any edited text in transcript
            if (Options.ShowEditedText)
            {
                if (SetEditedTranscriptText() == false) return false;
            }

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
				m_ctrlViewer.Title = m_dxDeposition.Name;
				m_ctrlViewer.Template = m_strTemplate;
				m_ctrlViewer.ReportSource = m_dsReportSource;
				m_ctrlViewer.EnableGroups = false;
				
				if(Options.PageHeaderFirst == false)
				{
					//	Suppress the report header and page header sections
					m_ctrlViewer.SuppressedSections.Add("ReportHeaderSection1");
					m_ctrlViewer.SuppressedSections.Add("PageHeaderSection1");
				}
				else
				{
					if(Options.PageHeaderSubsequent == true)
					{
						//	Suppress the report header
						m_ctrlViewer.SuppressedSections.Add("ReportHeaderSection1");
					}
					else
					{
						//	Suppress the page header section
						m_ctrlViewer.SuppressedSections.Add("PageHeaderSection1");
					}
					
				}
				
				//	Should we suppress the footers?
				if(Options.PageFooter == false)
					m_ctrlViewer.SuppressedSections.Add("PageFooterSection1");
					
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
			m_ctrlPreviewExports.Enabled = (eFormat != TmaxExportReportFormats.None);
			m_ctrlBrowseExportFolder.Enabled = (eFormat != TmaxExportReportFormats.None);
		
		}// protected void OnExportFormatsSelChanged(object sender, System.EventArgs e)
		
		/// <summary>This method is called to get the path to the export file for the specified report</summary>
		/// <param name="iReportIndex">The index of the report being exported</param>
		/// <param name="strFolder">The path to the folder where exported reports should be stored</param>
		/// <param name="eFormat">The format of the file being exported</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>The fully qualified path</returns>
		protected override string GetExportFileSpec(int iReportIndex, string strFolder, TmaxExportReportFormats eFormat, bool bOnePerPrimary)
		{
			CDxPrimary	dxScript = null;
			CDxPrimary	dxDeposition = null;
			string		strFileSpec = "";
			string		strFileName = "";
			
			Debug.Assert(iReportIndex >= 0);
			Debug.Assert(iReportIndex < m_tmaxReportGroups.Count);
			
			//	Get the deposition this report is based on
			if((iReportIndex >= 0) && (iReportIndex < m_tmaxReportGroups.Count))
				dxDeposition = (CDxPrimary)(m_tmaxReportGroups[iReportIndex].UserData1);
			
			//	There should always be a deposition
			Debug.Assert(dxDeposition != null);
			if(dxDeposition == null)
				return "";
				
			//	There may or may not be a script
			dxScript = (CDxPrimary)(m_tmaxReportGroups[iReportIndex].GetMediaRecord());
			
			//	Assemble the filename
			if(dxScript != null)
				strFileName += (dxScript.MediaId + " ");
				
			if(dxDeposition.Name.Length > 0)
			{
				strFileName += dxDeposition.Name;
			}
			else
			{
				strFileName += dxDeposition.MediaId;
				
				if(dxDeposition.GetTranscript() != null)
				{
					strFileName += (" " + dxDeposition.GetTranscript().DeposedOn);
				}
				
			}
			
			strFileName += ("." + CROBase.GetExtension(eFormat));
			
			//	Assemble the filename
			strFileSpec = strFolder;
			if(strFileSpec.EndsWith("\\") == false)
				strFileSpec += "\\";
			strFileSpec += CTmaxToolbox.CleanFilename(strFileName, false);
				
			return strFileSpec;
		
		}// protected virtual string GetExportFileSpec(int iReportIndex, string strFolder, TmaxExportReportFormats eFormat)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Called to get the source groups required for the specified primary media</summary>
		/// <param name="dxPrimary">The primary media record</param>
		/// <returns>true if successful</returns>
		private bool GetSourceFromMedia(CDxPrimary dxPrimary)
		{
			bool		bFilled = false;
			bool		bSuccessful = true;
			CTmaxItem	tmaxGroup = null;
			CDxPrimary	dxDeposition = null;
			
			Debug.Assert(dxPrimary != null);
			
			try
			{
				//	Is this a script we're processing?
				if(dxPrimary.MediaType == TmaxMediaTypes.Script)
				{
					//	Do we need to get the secondary scenes collection?
					if((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
					{
						dxPrimary.Fill();
						bFilled = true;
					}
					
					//	Iterate the scenes and locate those associated with a designation
					foreach(CDxSecondary O in dxPrimary.Secondaries)
					{
						if((O.GetSource() != null) && (O.GetSource().MediaType == TmaxMediaTypes.Designation))
						{
							//	Get the source deposition for this designation
							if((dxDeposition = GetDeposition((CDxTertiary)(O.GetSource()))) != null)
							{
								//	Do we need to find the group item?
								if((tmaxGroup == null) || (ReferenceEquals(tmaxGroup.UserData1, dxDeposition) == false))
									tmaxGroup = GetSourceGroup(dxPrimary, dxDeposition);

								//	Do we need to add a group?
								if(tmaxGroup == null)
								{
									tmaxGroup = new CTmaxItem(dxPrimary);
									tmaxGroup.UserData1 = dxDeposition;
									m_tmaxSourceGroups.Add(tmaxGroup);
								}
								
								//	Add this scene to the group collection
								tmaxGroup.SubItems.Add(new CTmaxItem(O));
							
								//	Add the deposition if not already in the local collection
								if(m_dxDepositions.Contains(dxDeposition) == false)
									m_dxDepositions.AddList(dxDeposition);

							}	
					
						}// if((O.GetSource() != null) && (O.GetSource().MediaType == TmaxMediaTypes.Designation))
					
					}// foreach(CDxSecondary O in dxPrimary.Secondaries)
				
					//	Did we add any scenes from this script?
					if((tmaxGroup != null) && (tmaxGroup.SubItems.Count > 0))
					{
						if(m_dxScripts.Contains(dxPrimary) == false)
							m_dxScripts.AddList(dxPrimary);
					}
					
				}
				
				//	Is it a deposition?
				else if(dxPrimary.MediaType == TmaxMediaTypes.Deposition)
				{
					//	Do we already have a group for this deposition?
					if((tmaxGroup = GetSourceGroup(null, dxPrimary)) == null)
					{
						//	Add a group for this deposition
						tmaxGroup = new CTmaxItem();
						tmaxGroup.UserData1 = dxPrimary;
						m_tmaxSourceGroups.Add(tmaxGroup);

						//	Add the deposition if not already in the local collection
						if(m_dxDepositions.Contains(dxPrimary) == false)
							m_dxDepositions.AddList(dxPrimary);
					}
				
				}
				
				else
				{
					//	This should only be getting called for scripts and depositions
					bSuccessful = false;
				}
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceFromMedia", m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_FROM_MEDIA_EX, dxPrimary.GetBarcode(false)), Ex);
				bSuccessful = false;
			}
			finally
			{
				//	Should we flush the collection of child contents?
				if(bFilled == true)
					dxPrimary.Secondaries.Clear();
			}
			
			return bSuccessful;
			
		}// private bool GetSourceFromMedia(CDxPrimary dxPrimary)
		
		/// <summary>Called to get the source groups required for the specified binder</summary>
		/// <param name="dxBinder">The binder record</param>
		/// <returns>true if successful</returns>
		private bool GetSourceFromBinder(CDxBinderEntry dxBinder)
		{
			bool	bFilled = false;
			bool	bSuccessful = true;
			
			Debug.Assert(dxBinder != null);
			
			try
			{
				//	Do we need to populate the child collection?
				if((dxBinder.Contents.Count == 0) && (dxBinder.ChildCount > 0))
				{
					dxBinder.Fill();
					bFilled = true;
				}
				
				//	Search for primary media in the binder
				foreach(CDxBinderEntry O in dxBinder.Contents)
				{
					if(O.IsMedia() == true)
					{
						if(O.GetSource(true) != null)
						{
							switch(O.GetSource(true).MediaType)
							{
								case TmaxMediaTypes.Script:
								case TmaxMediaTypes.Deposition:
								
									GetSourceFromMedia((CDxPrimary)(O.GetSource(true)));
									break;
							}
							
						}// if(O.GetSource() != null)
						
					}
					else
					{
						//	Drill down into subbinders
						GetSourceFromBinder(O);
					
					}// if(O.IsMedia() == true)
					
				}// foreach(CDxBinderEntry O in dxBinder.Contents)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceFromBinder", m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_FROM_BINDER_EX, dxBinder.Name), Ex);
				bSuccessful = false;
			}
			finally
			{
				//	Should we flush the collection of child contents?
				if(bFilled == true)
					dxBinder.Contents.Clear();
			}
			
			return bSuccessful;
			
		}// private bool GetSourceFromBinder(CDxBinderEntry dxBinder)
		
		/// <summary>This method is called to get the deposition record from which the specified designation was created</summary>
		/// <param name="dxDesignation">The designation created from the desired deposition</param>
		/// <returns>The source deposition for the specified designation</returns>
		private CDxPrimary GetDeposition(CDxTertiary dxDesignation)
		{
			Debug.Assert(dxDesignation != null);
			
			if(dxDesignation != null)
			{
				Debug.Assert(dxDesignation.Secondary != null);
				if(dxDesignation.Secondary != null)
				{
					Debug.Assert(dxDesignation.Secondary.Primary != null);
					return dxDesignation.Secondary.Primary;
				}
				
			}
			return null;
			
		}// private CDxPrimary GetDeposition(CDxTertiary dxDesignation)
		
		/// <summary>This method is called to add the transcript text to the source data set</summary>
		/// <returns>true if successful</returns>
		private bool FillSourceTranscript()
		{
			long lPage = -1;

			Debug.Assert(m_dsReportSource != null);
			if(m_dsReportSource == null) return false;
			
			Debug.Assert(m_xmlDeposition != null);
			if(m_xmlDeposition == null) return false;
			
			//m_tmaxEventSource.InitElapsed();			
			
			//	Iterate the transcript text in the XML file and add each
			//	line to the data set
			foreach(CXmlTranscript O in m_xmlDeposition.Transcripts)
			{
				if(AddSourceText(O, (lPage != O.Page)) == false)
					return false;
					
				lPage = O.Page;
			
			}// foreach(CXmlTranscript O in m_xmlDeposition.Transcripts)
			
			//m_tmaxEventSource.FireElapsed(this, "FillSourceTranscript", "Time to fill transcript table: ");			
			return true;

		}// private bool SetSourceTranscript()
		
		/// <summary>This method is called to set the highlights for all scenes in the active script</summary>
		/// <returns>true if successful</returns>
		private bool SetSourceHighlights()
		{
			CDxTertiary dxDesignation = null;
            Hashtable highlightDurationTable = new Hashtable();

			Debug.Assert(m_dxScenes != null);
			if(m_dxScenes == null) return false;
			
			//	Don't bother if not bolding or highlighting
			if((Options.VerticalHighlights == false) && (Options.BoldDesignations == false))
				return true;
				
			//m_tmaxEventSource.InitElapsed();
			
			//	Iterate the collection of scenes and highlight each designation
			foreach(CDxSecondary dxScene in m_dxScenes)
			{
				//	Only designations should be in the collection
				Debug.Assert(dxScene.GetSource() != null);
				Debug.Assert(dxScene.GetSource().MediaType == TmaxMediaTypes.Designation);
				
				if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
				{
					dxDesignation = (CDxTertiary)(dxScene.GetSource());
					
					//	Is this designation created from the selected deposition?
					if((m_dxDeposition == null) || (ReferenceEquals(dxDesignation.Secondary.Primary, m_dxDeposition) == true))
					{
                        CDxExtent cExtent = dxDesignation.GetExtent();
                        if (cExtent != null)
						{                                                  
							//	Highlight this designation
                            SetSourceHighlight(cExtent);

                            //Save higlighter duration in Hash table
                            RecordSourceHighlighterDuration(cExtent, highlightDurationTable);                           
						}					
					}					
				}
			
			}// foreach(CDxSecondary dxScene in m_dxScenes)

            //calculate time duration and update data table
            CalculateHighlightedTextDuration(highlightDurationTable);           

			//m_tmaxEventSource.FireElapsed(this, "SetSourceHighlights", "Time to highlight transcript: ");
			return true;
		
		}// private bool SetSourceHighlights()
		
		/// <summary>This method is called to set the highlights for the specified designation</summary>
		/// <returns>true if successful</returns>
        private bool SetSourceHighlight(CDxExtent cExtent)
		{
			long lPL = 0;
            long lFirstPL = cExtent.StartPL;
            long lLastPL = cExtent.StopPL;
            long lHighlighter = cExtent.HighlighterId;
            long lDuration = 0;            
			try
			{
				//	Locate the rows that fall within this range
				foreach(DataRow dr in m_dsReportSource.Tables[TRANSCRIPT_TABLE_NAME].Rows)
				{
					lPL = (long)((int)(dr["PL"]));
					
					//	Is this row within range?
					if((lPL >= lFirstPL) && (lPL <= lLastPL))
					{
						if(Options.BoldDesignations == true)
						{
							dr["FontStyle"] = 1;
						}
						
						if(Options.VerticalHighlights == true)
						{
							//	Assume the highlighter id is really the index
							switch(lHighlighter)
							{
								case 1:		dr["H1"] = m_aHighlighters[0];
											break;
								case 2:		dr["H2"] = m_aHighlighters[1];
											break;
								case 3:		dr["H3"] = m_aHighlighters[2];
											break;
								case 4:		dr["H4"] = m_aHighlighters[3];
											break;
								case 5:		dr["H5"] = m_aHighlighters[4];
											break;
								case 6:		dr["H6"] = m_aHighlighters[5];
											break;
								case 7:		dr["H7"] = m_aHighlighters[6];
											break;
											
							}// switch(lHighlighter)

                            if (Options.ShowHighlighterLabel)
                            {
                                DataRow[] existingHighlighter = m_dsReportSource.Tables[HIGHLIGHTER_TABLE_NAME].Select("HName = '" + m_aHighlightersNames[lHighlighter - 1] + "'");
                                if (existingHighlighter == null || existingHighlighter.Length == 0)
                                {
                                    DataRow drHighlighter = m_dsReportSource.Tables[HIGHLIGHTER_TABLE_NAME].NewRow();
                                    //       drHighlighter["Clip"] = m_iClipIndex;
                                    drHighlighter["HName"] = m_aHighlightersNames[lHighlighter - 1];
                                    drHighlighter["HColor"] = m_aHighlighters[lHighlighter - 1];

                                    m_dsReportSource.Tables[HIGHLIGHTER_TABLE_NAME].Rows.Add(drHighlighter);
                                }
                            }
						}
						
					}// if((lPL >= lFirstPL) && (lPL <= lLastPL))
					
					//	Have we gone beyond the last PL?
					else if(lPL > lLastPL)
					{
						//	We are assuming the table is sorted
						break;
					}
					
				}// foreach(DataRow O in m_dsReportSource.Tables[TRANSCRIPT_TABLE_NAME].Rows)

                
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSourceHighlight", m_tmaxErrorBuilder.Message(ERROR_SET_SOURCE_HIGHLIGHT_EX), Ex);
				return false;
			}
			
		}// private bool SetSourceHighlight(long lFirstPL, long lLastPL, long lHighlighter)

        /// <summary>This method is called to record the Highlighted text duration on hashtable</summary>
        /// <returns>true if successful</returns>
        private void RecordSourceHighlighterDuration(CDxExtent cExtent, Hashtable highlightDurationTable)
        {
            double lDuration = cExtent.Stop - cExtent.Start;
            if (!highlightDurationTable.ContainsKey(m_aHighlightersNames[cExtent.HighlighterId - 1]))
            {
                highlightDurationTable.Add(m_aHighlightersNames[cExtent.HighlighterId - 1], lDuration);
            }
            else
            {
                lDuration = cExtent.Stop - cExtent.Start;
                if (lDuration > 0)
                {
                    double existingDuration = (double)highlightDurationTable[m_aHighlightersNames[cExtent.HighlighterId - 1]];
                    existingDuration = existingDuration + lDuration;
                    highlightDurationTable[m_aHighlightersNames[cExtent.HighlighterId - 1]] = existingDuration;
                }
            }
        }//private void RecordSourceHighlighterDuration(CDxExtent cExtent, Hashtable highlightDurationTable)


        /// <summary>This method is called to calculate time duration of the Highlighted text</summary>
        /// <returns>true if successful</returns>
        private void CalculateHighlightedTextDuration(Hashtable highlightDurationTable)
        {
            foreach (string key in highlightDurationTable.Keys)
            {
                double duration = (double)highlightDurationTable[key];
                DataRow[] existingHighlighter = m_dsReportSource.Tables[HIGHLIGHTER_TABLE_NAME].Select("HName = '" + key + "'");
                if (existingHighlighter != null && existingHighlighter.Length > 0)
                {
                    if (duration > 0)
                    {
                        existingHighlighter[0]["HDuration"] = CTmaxToolbox.SecondsToString(Math.Round(duration), 0);
                    }
                    else
                    {
                        existingHighlighter[0]["HDuration"] = "00:00:00";
                    }
                }
            }
        }//private void CalculateHighlightedTextDuration(Hashtable highlightDurationTable)

        /// <summary>This method is called to show edited text in the transcript</summary>
        /// <returns>true if successful</returns>
        private bool SetEditedTranscriptText()
        {
            CDxTertiary dxDesignation = null;

            Debug.Assert(m_dxScenes != null);
            if (m_dxScenes == null) return false;

            //	Iterate the collection of scenes and highlight each designation
            foreach (CDxSecondary dxScene in m_dxScenes)
            {
                //	Only designations should be in the collection
                Debug.Assert(dxScene.GetSource() != null);
                Debug.Assert(dxScene.GetSource().MediaType == TmaxMediaTypes.Designation);

                if ((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
                {
                    dxDesignation = (CDxTertiary)(dxScene.GetSource());

                    //	Is this designation created from the selected deposition?
                    if ((m_dxDeposition == null) || (ReferenceEquals(dxDesignation.Secondary.Primary, m_dxDeposition) == true))
                    {
                        //	Get the fully qualified path to the XML file containing the transcript text
                        string strFileSpec = m_tmaxDatabase.GetFileSpec(dxDesignation);
                        if (System.IO.File.Exists(strFileSpec) == false)
                        {
                            m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_FILL_TRANSCRIPT_EX, dxScene.GetBarcode(false), strFileSpec));
                            return false;
                        }

                        //	Load the file
                        CXmlDesignation xmlDesignation = new CXmlDesignation();
                        xmlDesignation.FastFill(strFileSpec, true, true);

                        foreach (CXmlTranscript O in xmlDesignation.Transcripts)
                        {
                        //	Locate the rows that fall within this range
                            if (O.Edited)
                            {
                                var filteredRow = m_dsReportSource.Tables[TRANSCRIPT_TABLE_NAME].Select("PL = " + O.PL + " and Line = " + O.Line);
                                if (filteredRow != null && filteredRow.Length > 0)
                                {
                                    filteredRow[0]["LineText"] = O.Text;
                                    filteredRow[0]["Edit"] = "E";
                                }
                            }
                           
                        }

                        //	Clean up
                        xmlDesignation.Clear();
                        xmlDesignation = null;
                    }

                }

            }// foreach(CDxSecondary dxScene in m_dxScenes)

            //m_tmaxEventSource.FireElapsed(this, "SetSourceHighlights", "Time to highlight transcript: ");
            return true;

        }// private bool SetEditedTranscriptText()()

		/// <summary>This method is called to add a line to the transcript table</summary>
		/// <param name="xmlTranscript">The XML transcript line descriptor</param>
		/// <param name="bPageNumber">True to include the page number</param>
		/// <returns>true if successful</returns>
		private bool AddSourceText(CXmlTranscript xmlTranscript, bool bPageNumber)
		{
			string strPage = "";
			
			if(bPageNumber == true)
				strPage = xmlTranscript.Page.ToString() + ":";
			
			try
			{
				//	Create a new row
				DataRow dr = m_dsReportSource.Tables[TRANSCRIPT_TABLE_NAME].NewRow();
				
				//	Set the values
				dr["PL"] = (int)xmlTranscript.PL;
				dr["Page"] = strPage;
				dr["Line"] = (short)xmlTranscript.Line;
				dr["LineText"] = xmlTranscript.GetFormattedText();
				dr["FontStyle"] = 0;
				dr["FontSize"] = m_iFontSize;
				dr["H1"] = DEFAULT_HIGHLIGHTER;
				dr["H2"] = DEFAULT_HIGHLIGHTER;
				dr["H3"] = DEFAULT_HIGHLIGHTER;
				dr["H4"] = DEFAULT_HIGHLIGHTER;
				dr["H5"] = DEFAULT_HIGHLIGHTER;
				dr["H6"] = DEFAULT_HIGHLIGHTER;
				dr["H7"] = DEFAULT_HIGHLIGHTER;
                dr["Edit"] = "";
				
				//	Add the row
				m_dsReportSource.Tables[TRANSCRIPT_TABLE_NAME].Rows.Add(dr);
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSourceText", m_tmaxErrorBuilder.Message(ERROR_ADD_SOURCE_TEXT_EX), Ex);
				return false;
			}
			
		}// private bool AddSourceText(CXmlTranscript xmlTranscript, bool bPageNumber)
		
		/// <summary>This method is called to populate the Information table</summary>
		/// <returns>true if successful</returns>
		private bool FillSourceInformation()
		{
			string	strMediaID = "";
			string	strName = "";
			string	strDeponent = "";
			
			//	The data set must be available
			Debug.Assert(m_dsReportSource != null);
			if(m_dsReportSource == null) return false;
			
			//	We must have a deposition
			Debug.Assert(m_dxDeposition != null);
			if(m_dxDeposition == null) return false;
			
			//	We must have a transcript
			Debug.Assert(m_dxTranscript != null);
			if(m_dxTranscript == null) return false;
			
			try
			{
				//m_tmaxEventSource.InitElapsed();
				
				//	Do we have a script?
				if(m_dxScenes.Count > 0)
				{
					strMediaID = m_dxScenes[0].Primary.MediaId;
					strName = m_dxScenes[0].Primary.Name;
					if(strName.Length == 0)
						strName = strMediaID;
				}
				else
				{
					strMediaID = "";
					strName = "";
				}
			
				strDeponent = m_dxDeposition.Name;
				if(strDeponent.Length == 0)
					strDeponent = m_dxDeposition.MediaId;

				//	Create a new row
				DataRow dr = m_dsReportSource.Tables[INFORMATION_TABLE_NAME].NewRow();
				
				//	Set the values
				dr["Deponent"] = strDeponent;
				dr["Deposition_Date"] = m_dxTranscript.DeposedOn;
				dr["MediaID"] = strMediaID;
				dr["Script_Name"] = strName;

				//	Add the row
				m_dsReportSource.Tables[INFORMATION_TABLE_NAME].Rows.Add(dr);
				
				//m_tmaxEventSource.FireElapsed(this, "FillSourceInformation", "Time to fill Information table: ");
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillSourceInformation", m_tmaxErrorBuilder.Message(ERROR_FILL_INFORMATION_EX), Ex);
				return false;
			}
			
		}// private bool FillSourceInformation()
		
		/// <summary>Called to populate the transcripts list box</summary>
		private void FillTranscripts()
		{
			Debug.Assert(m_ctrlTranscripts != null);
			Debug.Assert(m_ctrlTranscripts.IsDisposed == false);
			if(m_ctrlTranscripts == null) return;
			if(m_ctrlTranscripts.IsDisposed == true) return;
			
			//	Flush the existing list
			m_ctrlTranscripts.Items.Clear();
			
			//	Add an entry for each deposition
			if((m_dxDepositions != null) && (m_dxDepositions.Count > 0))
			{
				foreach(CDxPrimary O in m_dxDepositions)
				{
					m_ctrlTranscripts.Items.Add(O);
				}
				
			}
			
			// Initialize the check states
			if(m_ctrlTranscripts.Items.Count > 0)
			{
				//	Pre-select all the transcripts
				for(int i = 0; i < m_ctrlTranscripts.Items.Count; i++)
					m_ctrlTranscripts.SetItemCheckState(i, CheckState.Checked);
				
			}// if(m_ctrlTranscripts.Items.Count > 0)
			
		}// protected void FillTranscripts()

		/// <summary>This method is called to set the transcript used for the report</summary>
		/// <param name="dxDeposition">The deposition being activated</param>
		/// <returns>true if successful</returns>
		private bool SetTranscript(CDxPrimary dxDeposition)
		{
			string	strFileSpec = "";
			string	strMsg = "";
			
			try
			{
				//m_tmaxEventSource.InitElapsed();
				
				//	Set the selected deposition
				if((m_dxDeposition = dxDeposition) == null)
					return false;
				
				//	Get the transcript information for this deposition
				if((m_dxTranscript = m_dxDeposition.GetTranscript()) == null)
				{
					MessageBox.Show("Unable to retrieve the transcript record for the selected deposition", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}
				
				//	Get the path to the transcript file from the database
				strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxTranscript);
				
				//	Make sure the file exists
				if(System.IO.File.Exists(strFileSpec) == false)
				{
					strMsg = String.Format("Unable to locate the XML transcript: {0}", strFileSpec);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}
				
				//	Load the transcript text
				m_xmlDeposition = new CXmlDeposition();
				if(m_xmlDeposition.FastFill(strFileSpec, false, true, false) == false)
				{
					strMsg = String.Format("Unable to load transcript text from {0}", strFileSpec);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}
				
				//m_tmaxEventSource.FireElapsed(this, "SetTranscript", "Time to load XML transcript: ");
				
				return true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetTranscript", m_tmaxErrorBuilder.Message(ERROR_SET_TRANSCRIPT_EX), Ex);
				return false;
			}

		}// private bool SetTranscript()
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRFTranscript));
            this.m_ctrlOK = new System.Windows.Forms.Button();
            this.m_ctrlCancel = new System.Windows.Forms.Button();
            this.m_ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlShowHighlighterLabel = new System.Windows.Forms.CheckBox();
            this.m_ctrlShowEditText = new System.Windows.Forms.CheckBox();
            this.m_ctrlMediumFont = new System.Windows.Forms.CheckBox();
            this.m_ctrlPageFooter = new System.Windows.Forms.CheckBox();
            this.m_ctrlPageHeaderSubsequent = new System.Windows.Forms.CheckBox();
            this.m_ctrlPageHeaderFirst = new System.Windows.Forms.CheckBox();
            this.m_ctrlVerticalHighlights = new System.Windows.Forms.CheckBox();
            this.m_ctrlBoldDesignations = new System.Windows.Forms.CheckBox();
            this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
            this.m_ctrlSaveData = new System.Windows.Forms.CheckBox();
            this.m_ctrlContentGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlTranscripts = new System.Windows.Forms.CheckedListBox();
            this.m_ctrlTranscriptsLabel = new System.Windows.Forms.Label();
            this.m_ctrlScript = new System.Windows.Forms.Label();
            this.m_ctrlScriptLabel = new System.Windows.Forms.Label();
            this.m_ctrlStyleGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlAlternateLabel = new System.Windows.Forms.Label();
            this.m_ctrlTemplates = new System.Windows.Forms.ComboBox();
            this.m_ctrlAlternate = new System.Windows.Forms.TextBox();
            this.m_ctrlBrowse = new System.Windows.Forms.Button();
            this.m_ctrlExportGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlExportFolderLabel = new System.Windows.Forms.Label();
            this.m_ctrlExportFolder = new System.Windows.Forms.TextBox();
            this.m_ctrlBrowseExportFolder = new System.Windows.Forms.Button();
            this.m_ctrlExportFormats = new System.Windows.Forms.ComboBox();
            this.m_ctrlPreviewExports = new System.Windows.Forms.CheckBox();
            this.m_ctrlOptionsGroup.SuspendLayout();
            this.m_ctrlContentGroup.SuspendLayout();
            this.m_ctrlStyleGroup.SuspendLayout();
            this.m_ctrlExportGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlOK
            // 
            this.m_ctrlOK.Location = new System.Drawing.Point(416, 300);
            this.m_ctrlOK.Name = "m_ctrlOK";
            this.m_ctrlOK.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlOK.TabIndex = 3;
            this.m_ctrlOK.Text = "&OK";
            // 
            // m_ctrlCancel
            // 
            this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_ctrlCancel.Location = new System.Drawing.Point(504, 300);
            this.m_ctrlCancel.Name = "m_ctrlCancel";
            this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlCancel.TabIndex = 4;
            this.m_ctrlCancel.Text = "&Cancel";
            // 
            // m_ctrlOptionsGroup
            // 
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlShowHighlighterLabel);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlShowEditText);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlMediumFont);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlPageFooter);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlPageHeaderSubsequent);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlPageHeaderFirst);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlVerticalHighlights);
            this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlBoldDesignations);
            this.m_ctrlOptionsGroup.Location = new System.Drawing.Point(304, 8);
            this.m_ctrlOptionsGroup.Name = "m_ctrlOptionsGroup";
            this.m_ctrlOptionsGroup.Size = new System.Drawing.Size(288, 184);
            this.m_ctrlOptionsGroup.TabIndex = 2;
            this.m_ctrlOptionsGroup.TabStop = false;
            this.m_ctrlOptionsGroup.Text = "Options";
            // 
            // m_ctrlShowHighlighterLabel
            // 
            this.m_ctrlShowHighlighterLabel.Location = new System.Drawing.Point(12, 158);
            this.m_ctrlShowHighlighterLabel.Name = "m_ctrlShowHighlighterLabel";
            this.m_ctrlShowHighlighterLabel.Size = new System.Drawing.Size(252, 20);
            this.m_ctrlShowHighlighterLabel.TabIndex = 7;
            this.m_ctrlShowHighlighterLabel.Text = "Show Highlighter Labels";
            // 
            // m_ctrlShowEditText
            // 
            this.m_ctrlShowEditText.Location = new System.Drawing.Point(12, 140);
            this.m_ctrlShowEditText.Name = "m_ctrlShowEditText";
            this.m_ctrlShowEditText.Size = new System.Drawing.Size(252, 16);
            this.m_ctrlShowEditText.TabIndex = 6;
            this.m_ctrlShowEditText.Text = "Show Edited Text";
            // 
            // m_ctrlMediumFont
            // 
            this.m_ctrlMediumFont.Location = new System.Drawing.Point(12, 121);
            this.m_ctrlMediumFont.Name = "m_ctrlMediumFont";
            this.m_ctrlMediumFont.Size = new System.Drawing.Size(252, 16);
            this.m_ctrlMediumFont.TabIndex = 5;
            this.m_ctrlMediumFont.Text = "Medium Font";
            // 
            // m_ctrlPageFooter
            // 
            this.m_ctrlPageFooter.Checked = true;
            this.m_ctrlPageFooter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlPageFooter.Location = new System.Drawing.Point(12, 100);
            this.m_ctrlPageFooter.Name = "m_ctrlPageFooter";
            this.m_ctrlPageFooter.Size = new System.Drawing.Size(252, 16);
            this.m_ctrlPageFooter.TabIndex = 4;
            this.m_ctrlPageFooter.Text = "Include page footer";
            // 
            // m_ctrlPageHeaderSubsequent
            // 
            this.m_ctrlPageHeaderSubsequent.Checked = true;
            this.m_ctrlPageHeaderSubsequent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlPageHeaderSubsequent.Location = new System.Drawing.Point(12, 79);
            this.m_ctrlPageHeaderSubsequent.Name = "m_ctrlPageHeaderSubsequent";
            this.m_ctrlPageHeaderSubsequent.Size = new System.Drawing.Size(252, 16);
            this.m_ctrlPageHeaderSubsequent.TabIndex = 3;
            this.m_ctrlPageHeaderSubsequent.Text = "Include page header on subsequent pages";
            // 
            // m_ctrlPageHeaderFirst
            // 
            this.m_ctrlPageHeaderFirst.Checked = true;
            this.m_ctrlPageHeaderFirst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlPageHeaderFirst.Location = new System.Drawing.Point(12, 58);
            this.m_ctrlPageHeaderFirst.Name = "m_ctrlPageHeaderFirst";
            this.m_ctrlPageHeaderFirst.Size = new System.Drawing.Size(252, 16);
            this.m_ctrlPageHeaderFirst.TabIndex = 2;
            this.m_ctrlPageHeaderFirst.Text = "Include page header on first page";
            this.m_ctrlPageHeaderFirst.Click += new System.EventHandler(this.OnClickPageHeaderFirst);
            // 
            // m_ctrlVerticalHighlights
            // 
            this.m_ctrlVerticalHighlights.Checked = true;
            this.m_ctrlVerticalHighlights.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlVerticalHighlights.Location = new System.Drawing.Point(12, 37);
            this.m_ctrlVerticalHighlights.Name = "m_ctrlVerticalHighlights";
            this.m_ctrlVerticalHighlights.Size = new System.Drawing.Size(252, 16);
            this.m_ctrlVerticalHighlights.TabIndex = 1;
            this.m_ctrlVerticalHighlights.Text = "Vertical Highlights";
            // 
            // m_ctrlBoldDesignations
            // 
            this.m_ctrlBoldDesignations.Checked = true;
            this.m_ctrlBoldDesignations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ctrlBoldDesignations.Location = new System.Drawing.Point(12, 16);
            this.m_ctrlBoldDesignations.Name = "m_ctrlBoldDesignations";
            this.m_ctrlBoldDesignations.Size = new System.Drawing.Size(252, 16);
            this.m_ctrlBoldDesignations.TabIndex = 0;
            this.m_ctrlBoldDesignations.Text = "Bold Designations";
            // 
            // m_ctrlImages
            // 
            this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
            this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlImages.Images.SetKeyName(0, "");
            // 
            // m_ctrlSaveData
            // 
            this.m_ctrlSaveData.Location = new System.Drawing.Point(292, 300);
            this.m_ctrlSaveData.Name = "m_ctrlSaveData";
            this.m_ctrlSaveData.Size = new System.Drawing.Size(92, 16);
            this.m_ctrlSaveData.TabIndex = 6;
            this.m_ctrlSaveData.Text = "Save Data";
            // 
            // m_ctrlContentGroup
            // 
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlTranscripts);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlTranscriptsLabel);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlScript);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlScriptLabel);
            this.m_ctrlContentGroup.Location = new System.Drawing.Point(7, 8);
            this.m_ctrlContentGroup.Name = "m_ctrlContentGroup";
            this.m_ctrlContentGroup.Size = new System.Drawing.Size(288, 184);
            this.m_ctrlContentGroup.TabIndex = 0;
            this.m_ctrlContentGroup.TabStop = false;
            this.m_ctrlContentGroup.Text = "Content";
            // 
            // m_ctrlTranscripts
            // 
            this.m_ctrlTranscripts.Location = new System.Drawing.Point(8, 68);
            this.m_ctrlTranscripts.Name = "m_ctrlTranscripts";
            this.m_ctrlTranscripts.Size = new System.Drawing.Size(272, 79);
            this.m_ctrlTranscripts.TabIndex = 4;
            // 
            // m_ctrlTranscriptsLabel
            // 
            this.m_ctrlTranscriptsLabel.Location = new System.Drawing.Point(8, 48);
            this.m_ctrlTranscriptsLabel.Name = "m_ctrlTranscriptsLabel";
            this.m_ctrlTranscriptsLabel.Size = new System.Drawing.Size(192, 16);
            this.m_ctrlTranscriptsLabel.TabIndex = 2;
            this.m_ctrlTranscriptsLabel.Text = "Transcript(s):";
            // 
            // m_ctrlScript
            // 
            this.m_ctrlScript.Location = new System.Drawing.Point(64, 24);
            this.m_ctrlScript.Name = "m_ctrlScript";
            this.m_ctrlScript.Size = new System.Drawing.Size(216, 16);
            this.m_ctrlScript.TabIndex = 1;
            // 
            // m_ctrlScriptLabel
            // 
            this.m_ctrlScriptLabel.Location = new System.Drawing.Point(8, 24);
            this.m_ctrlScriptLabel.Name = "m_ctrlScriptLabel";
            this.m_ctrlScriptLabel.Size = new System.Drawing.Size(52, 16);
            this.m_ctrlScriptLabel.TabIndex = 0;
            this.m_ctrlScriptLabel.Text = "Script(s):";
            // 
            // m_ctrlStyleGroup
            // 
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlAlternateLabel);
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlTemplates);
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlAlternate);
            this.m_ctrlStyleGroup.Controls.Add(this.m_ctrlBrowse);
            this.m_ctrlStyleGroup.Location = new System.Drawing.Point(7, 193);
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
            // m_ctrlExportGroup
            // 
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlExportFolderLabel);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlExportFolder);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlBrowseExportFolder);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlExportFormats);
            this.m_ctrlExportGroup.Controls.Add(this.m_ctrlPreviewExports);
            this.m_ctrlExportGroup.Location = new System.Drawing.Point(304, 193);
            this.m_ctrlExportGroup.Name = "m_ctrlExportGroup";
            this.m_ctrlExportGroup.Size = new System.Drawing.Size(288, 92);
            this.m_ctrlExportGroup.TabIndex = 9;
            this.m_ctrlExportGroup.TabStop = false;
            this.m_ctrlExportGroup.Text = "Export";
            // 
            // m_ctrlExportFolderLabel
            // 
            this.m_ctrlExportFolderLabel.Location = new System.Drawing.Point(10, 48);
            this.m_ctrlExportFolderLabel.Name = "m_ctrlExportFolderLabel";
            this.m_ctrlExportFolderLabel.Size = new System.Drawing.Size(210, 16);
            this.m_ctrlExportFolderLabel.TabIndex = 6;
            this.m_ctrlExportFolderLabel.Text = "Target Folder";
            this.m_ctrlExportFolderLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // m_ctrlExportFolder
            // 
            this.m_ctrlExportFolder.Location = new System.Drawing.Point(6, 64);
            this.m_ctrlExportFolder.Name = "m_ctrlExportFolder";
            this.m_ctrlExportFolder.Size = new System.Drawing.Size(244, 20);
            this.m_ctrlExportFolder.TabIndex = 4;
            // 
            // m_ctrlBrowseExportFolder
            // 
            this.m_ctrlBrowseExportFolder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.m_ctrlBrowseExportFolder.ImageIndex = 0;
            this.m_ctrlBrowseExportFolder.ImageList = this.m_ctrlImages;
            this.m_ctrlBrowseExportFolder.Location = new System.Drawing.Point(255, 64);
            this.m_ctrlBrowseExportFolder.Name = "m_ctrlBrowseExportFolder";
            this.m_ctrlBrowseExportFolder.Size = new System.Drawing.Size(24, 20);
            this.m_ctrlBrowseExportFolder.TabIndex = 5;
            this.m_ctrlBrowseExportFolder.Click += new System.EventHandler(this.OnBrowseExportFolder);
            // 
            // m_ctrlExportFormats
            // 
            this.m_ctrlExportFormats.Location = new System.Drawing.Point(8, 20);
            this.m_ctrlExportFormats.Name = "m_ctrlExportFormats";
            this.m_ctrlExportFormats.Size = new System.Drawing.Size(172, 21);
            this.m_ctrlExportFormats.TabIndex = 0;
            this.m_ctrlExportFormats.Text = "comboBox1";
            this.m_ctrlExportFormats.SelectedIndexChanged += new System.EventHandler(this.OnExportFormatsSelChanged);
            // 
            // m_ctrlPreviewExports
            // 
            this.m_ctrlPreviewExports.Location = new System.Drawing.Point(205, 25);
            this.m_ctrlPreviewExports.Name = "m_ctrlPreviewExports";
            this.m_ctrlPreviewExports.Size = new System.Drawing.Size(72, 16);
            this.m_ctrlPreviewExports.TabIndex = 2;
            this.m_ctrlPreviewExports.Text = "Preview";
            // 
            // CRFTranscript
            // 
            this.AcceptButton = this.m_ctrlOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_ctrlCancel;
            this.ClientSize = new System.Drawing.Size(598, 334);
            this.ControlBox = false;
            this.Controls.Add(this.m_ctrlExportGroup);
            this.Controls.Add(this.m_ctrlStyleGroup);
            this.Controls.Add(this.m_ctrlContentGroup);
            this.Controls.Add(this.m_ctrlOptionsGroup);
            this.Controls.Add(this.m_ctrlCancel);
            this.Controls.Add(this.m_ctrlOK);
            this.Controls.Add(this.m_ctrlSaveData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CRFTranscript";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transcript Report";
            this.m_ctrlOptionsGroup.ResumeLayout(false);
            this.m_ctrlContentGroup.ResumeLayout(false);
            this.m_ctrlStyleGroup.ResumeLayout(false);
            this.m_ctrlStyleGroup.PerformLayout();
            this.m_ctrlExportGroup.ResumeLayout(false);
            this.m_ctrlExportGroup.PerformLayout();
            this.ResumeLayout(false);

		}
		
		/// <summary>This method handles the event fired when the user clicks on the PageHeaderFirst check box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickPageHeaderFirst(object sender, System.EventArgs e)
		{
			m_ctrlPageHeaderSubsequent.Enabled = m_ctrlPageHeaderFirst.Checked;
		
		}// private void OnClickPageHeaderFirst(object sender, System.EventArgs e)

		/// <summary>This method handles the event fired when the user clicks on the Browse button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickBrowse(object sender, System.EventArgs e)
		{
			string strFilename = m_ctrlAlternate.Text;
			
			if(BrowseForTemplate(ref strFilename) == true)
				m_ctrlAlternate.Text = strFilename;

		}// private void OnClickBrowse(object sender, System.EventArgs e)

		/// <summary>Called to locate the matching source group</summary>
		/// <param name="dxScript">The script bound to the source group</param>
		/// <param name="dxDeposition">The deposition bound to the source group</param>
		/// <returns>The group item if found</returns>
		CTmaxItem GetSourceGroup(CDxPrimary dxScript, CDxPrimary dxDeposition)
		{
			//	Must have a valid deposition
			Debug.Assert(dxDeposition != null);
			if(dxDeposition == null) return null;
			
			//	Search the groups collection for a matching group
			if((m_tmaxSourceGroups != null) && (m_tmaxSourceGroups.Count > 0))
			{
				foreach(CTmaxItem O in m_tmaxSourceGroups)
				{
					//	The depositions must match
					if(ReferenceEquals(O.UserData1, dxDeposition) == true)
					{
						//	Do the scripts match?
						if((O.IPrimary == null) && (dxScript == null))
							return O;
						else if(ReferenceEquals(O.IPrimary, dxScript) == true)
							return O;
					
					}//	if(ReferenceEquals(O.UserData1, dxDeposition) == true)				
				
				}// foreach(CTmaxItem O in m_tmaxSourceGroups)
				
			}// if((m_tmaxSourceGroups != null) && (m_tmaxSourceGroups.Count > 0))
			
			return null; // not found...
		
		}// CTmaxItem GetSourceGroup(CDxPrimary dxScript, CDxPrimary dxDeposition)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>User defined options for generating the report</summary>
		public CROTranscript Options
		{
			get { return m_reportOptions; }
			set { m_reportOptions = value; }
		}
		
		#endregion Properties
	
	}// public class RFTranscript : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Reports
