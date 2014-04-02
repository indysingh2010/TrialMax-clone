using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;

using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.ViewerObjectModel;

namespace FTI.Trialmax.Reports
{
	/// <summary>This form is used to preview a report</summary>
	public class CRFViewer : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_ON_LOAD_EX		= (ERROR_TMAX_FORM_MAX + 1);
		private const int ERROR_PREPARE_EX		= (ERROR_TMAX_FORM_MAX + 2);
		private const int ERROR_PREVIEW_EX		= (ERROR_TMAX_FORM_MAX + 3);
		private const int ERROR_EXPORT_EX		= (ERROR_TMAX_FORM_MAX + 4);
		private const int ERROR_INVALID_FORMAT	= (ERROR_TMAX_FORM_MAX + 5);
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Local member bound to Title property</summary>
		private string m_strTitle = "";
		
		/// <summary>Local member bound to Template property</summary>
		private string m_strTemplate = "";
		
		/// <summary>Local member bound to DataSourceSet property</summary>
		private System.Data.DataSet m_dsReportSource = null;
		
		/// <summary>Crystal Reports Viewer control</summary>
		private CrystalDecisions.Windows.Forms.CrystalReportViewer m_ctrlViewer;
		
		/// <summary>Local member bound to Document property</summary>
		private CrystalDecisions.CrystalReports.Engine.ReportDocument m_cdrDocument = null;
		
		/// <summary>Local member bound to EnableGroups property</summary>
		private bool m_bEnableGroups = false;
		
		/// <summary>Local member bound to ShowButtons property</summary>
		private bool m_bShowButtons = false;
		private System.Windows.Forms.Button m_ctrlOK;
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Local member bound to SuppressedSections property</summary>
		private ArrayList m_aSuppressedSections = new ArrayList();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This event is fired after the template is loaded but before the data is set</summary>
		///
		///	<remarks>This event gives users the opprotunity to set report options before loading the data</remarks>
		public event System.EventHandler TemplateLoaded;
		
		/// <summary>Constructor</summary>
		public CRFViewer()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			m_tmaxEventSource.Name = "Report Viewer";
			
		}// public CRFViewer()

		/// <summary>This method is called to prepare the report</summary>
		/// <returns>True if successful</returns>
		public bool Prepare()
		{
			Section reportSection = null;
			bool	bSuccessful = false;
			
			try
			{
				//	Create a new report document
				m_cdrDocument = new ReportDocument();
					
				//	Load the report template
				m_cdrDocument.Load(m_strTemplate);
					
				//	Notify the owner
				if(TemplateLoaded != null)
					TemplateLoaded(this, System.EventArgs.Empty);
					
				//	Suppress any sections that have been requested
				foreach(string O in m_aSuppressedSections)
				{
					if((reportSection = GetSection(m_cdrDocument, O)) != null)
					{
						if(reportSection.SectionFormat != null)
						{
							try { reportSection.SectionFormat.EnableSuppress = true; }
							catch {}
						}
					
					}
				}
				
				//	Set the report data
				m_cdrDocument.SetDataSource(ReportSource);
				
				bSuccessful = true; // ready to go
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Prepare", m_tmaxErrorBuilder.Message(ERROR_PREPARE_EX), Ex);
			}
			
			return bSuccessful;

		}// public bool Prepare()
		
		/// <summary>This method is called to export the report</summary>
		/// <param name="strFileSpec">The path to the export file</param>
		/// <param name="eFormat">The format of the exported file</param>
		/// <returns>True if successful</returns>
		public bool Export(string strFileSpec, TmaxExportReportFormats eFormat)
		{
			bool				bSuccessful = false;
			ExportFormatType	eType = ExportFormatType.NoFormat;

			try
			{
				//	The document should have been prepared before calling this method
				if(m_cdrDocument == null)
					return false;
					
				//	Get the Crystal Reports format identifier
				if((eType = GetExportType(eFormat)) != ExportFormatType.NoFormat)
				{
					//	Delete the file if it already exists
					if(System.IO.File.Exists(strFileSpec) == true)
					{
						try { System.IO.File.Delete(strFileSpec); }
						catch {}
					}
					
					//	Export the report to disk
					m_cdrDocument.ExportToDisk(eType, strFileSpec);
					bSuccessful = true;
				}
				else
				{
					m_tmaxEventSource.FireError(this, "Export", m_tmaxErrorBuilder.Message(ERROR_INVALID_FORMAT, eFormat));
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Export", m_tmaxErrorBuilder.Message(ERROR_EXPORT_EX, strFileSpec, eFormat), Ex);
			}
			
			return bSuccessful;

		}// public bool Export(string strFileSpec, TmaxExportReportFormats eFormat)
		
		/// <summary>This method is called to get the section with the specified name</summary>
		/// <param name="reportDoc">The report document to be searched</param>
		/// <param name="strName">The name of the section to be located</param>
		/// <returns>The section object with the specified name</returns>
		public Section GetSection(ReportDocument reportDoc, string strName)
		{
			try
			{
				//	Should we use the active report document?
				if(reportDoc == null)
					reportDoc = this.Document;

				Debug.Assert(reportDoc != null);
				if(reportDoc == null) return null;

				if(reportDoc.ReportDefinition == null) return null;
				if(reportDoc.ReportDefinition.Sections == null) return null;
				
				//	Iterate the sections collection and locate the specified section
				foreach(Section O in reportDoc.ReportDefinition.Sections)
				{
					if(String.Compare(O.Name, strName, true) == 0)
						return O;
				}
				
			}
			catch
			{
			}
			
			//	If we made it this far either we couldn't find it or an exception
			//	was raised
			return null;

		}// public Section GetSection(ReportDocument reportDoc, string strName)

		/// <summary>This method is called to get the section with the specified name</summary>
		/// <param name="strName">The name of the section to be located</param>
		/// <returns>The section object with the specified name</returns>
		public Section GetSection(string strName)
		{
			return GetSection(null, strName);
		}
		
		/// <summary>This method is called to get the area with the specified name</summary>
		/// <param name="reportDoc">The report document to be searched</param>
		/// <param name="strName">The name of the area to be located</param>
		/// <returns>The area object with the specified name</returns>
		public Area GetArea(ReportDocument reportDoc, string strName)
		{
			try
			{
				//	Should we use the active report document?
				if(reportDoc == null)
					reportDoc = this.Document;
					
				Debug.Assert(reportDoc != null);				
				if(reportDoc == null) return null;
				
				if(reportDoc.ReportDefinition == null) return null;
				if(reportDoc.ReportDefinition.Areas == null) return null;
				
				//	Iterate the sections collection and locate the specified section
				foreach(Area O in reportDoc.ReportDefinition.Areas)
				{
					if(String.Compare(O.Name, strName, true) == 0)
						return O;
				}
				
			}
			catch
			{
			}
			
			//	If we made it this far either we couldn't find it or an exception
			//	was raised
			return null;

		}// public Area GetArea(ReportDocument reportDoc, string strName)

		/// <summary>This method is called to get the field with the specified name</summary>
		/// <param name="reportDoc">The report document to be searched</param>
		/// <param name="strSection">The name of the section containing the field</param>
		/// <param name="strName">The name of the area to be located</param>
		/// <returns>The field object with the specified name</returns>
		public FieldObject GetField(ReportDocument reportDoc, string strSection, string strName)
		{
			Section section = null;
			
			try
			{
				//	Get the specified section
				if((section = GetSection(reportDoc, strSection)) != null)
				{
					//	Search all objects contained in this section
					foreach(CrystalDecisions.CrystalReports.Engine.ReportObject O in section.ReportObjects)
					{
						if(O.Kind == ReportObjectKind.FieldObject)
						{
							if(O.Name == strName)
								return ((FieldObject)O);
						}

					}// foreach(CrystalDecisions.CrystalReports.Engine.ReportObject O in section.ReportObjects)				

				}// if((section = GetSection(reportDoc, strSection)) != null)

			}
			catch
			{
			}

			return null; // Not found

		}// public FieldObject GetField(ReportDocument reportDoc, string strSection, string strName)

		/// <summary>This method is called to get the field with the specified name</summary>
		/// <param name="strSection">The name of the section containing the field</param>
		/// <param name="strName">The name of the area to be located</param>
		/// <returns>The field object with the specified name</returns>
		public FieldObject GetField(string strSection, string strName)
		{
			return GetField(null, strSection, strName);
		}

		/// <summary>This method is called to get the area with the specified name</summary>
		/// <param name="strName">The name of the area to be located</param>
		/// <returns>The area object with the specified name</returns>
		public Area GetArea(string strName)
		{
			return GetArea(null, strName);
		}

		/// <summary>This method is a debugging aide to view the sections in a document</summary>
		public void ShowSections()
		{
			if(m_cdrDocument == null)
			{
				MessageBox.Show("NO DOCUMENT AVAILABLE");
				return;
			}
			
			try
			{
				//	Iterate the sections collection and locate the specified section
				foreach(Section O in m_cdrDocument.ReportDefinition.Sections)
				{
					MessageBox.Show(O.Name);
				}
				
			}
			catch(System.Exception Ex)
			{
				MessageBox.Show(Ex.ToString());
			}

		}// public void ShowSections()
		
		/// <summary>This method is a debugging aide to view the sections in a document</summary>
		public void ShowAreas()
		{
			if(m_cdrDocument == null)
			{
				MessageBox.Show("NO DOCUMENT AVAILABLE");
				return;
			}
			
			try
			{
				//	Iterate the sections collection and locate the specified section
				foreach(Area O in m_cdrDocument.ReportDefinition.Areas)
				{
					MessageBox.Show(O.Name);
				}
				
			}
			catch(System.Exception Ex)
			{
				MessageBox.Show(Ex.ToString());
			}

		}// public void ShowAreas()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function handles all Resize events</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnResize(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnResize(e);
			
			//	Make sure the controls are properly sized
			RecalcLayout();
			
		}// protected override void OnResize(System.EventArgs e)

		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected void RecalcLayout()
		{
			//	Are the buttons invisible?
			if(m_bShowButtons == false)
			{
				m_ctrlViewer.Size = this.ClientSize;			
			}
			else
			{
				m_ctrlViewer.Size = new Size(this.Width, m_ctrlOK.Top - 4);
			}
			
		}
			
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				
				if(m_aSuppressedSections != null)
					m_aSuppressedSections.Clear();
					
			}
			base.Dispose( disposing );
		}
		
		/// <summary>Overridden base class member called when the form gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);
			
			//	Should we hide the buttons?
			if(m_bShowButtons == false)
			{
				m_ctrlOK.Visible = false;
				m_ctrlCancel.Visible = false;
			}
			
			//	Make sure the controls are properly sized
			RecalcLayout();

			try
			{
				//	Set the form title
				this.Text = m_strTitle;
				
				//	Are groups enabled for this report?
				if(m_bEnableGroups == false)
				{
					m_ctrlViewer.DisplayGroupTree = false;
					m_ctrlViewer.ShowGroupTreeButton = false;
				}
				
				//	Make this report the source for the viewer
				if(m_cdrDocument != null)
					m_ctrlViewer.ReportSource = m_cdrDocument;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnLoad", m_tmaxErrorBuilder.Message(ERROR_ON_LOAD_EX), Ex);
			}
		
		}// protected override void OnLoad(EventArgs e)
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to load the report viewer.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prepare the report for execution.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to preview the report.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the report to %1  <format = %2>");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to export the report. <%1> is not a valid export format");
		
		}// protected override void SetErrorStrings()

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CRFViewer));
			this.m_ctrlViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlViewer
			// 
			this.m_ctrlViewer.ActiveViewIndex = -1;
			this.m_ctrlViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlViewer.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlViewer.Name = "m_ctrlViewer";
			this.m_ctrlViewer.ReportSource = null;
			this.m_ctrlViewer.ShowCloseButton = false;
			this.m_ctrlViewer.ShowRefreshButton = false;
			this.m_ctrlViewer.Size = new System.Drawing.Size(528, 324);
			this.m_ctrlViewer.TabIndex = 6;
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOK.Location = new System.Drawing.Point(352, 332);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 7;
			this.m_ctrlOK.Text = "&OK";
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(436, 332);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 8;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// CRFViewer
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(528, 361);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Controls.Add(this.m_ctrlViewer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CRFViewer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CRFViewer";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called to preview the report</summary>
		private void Preview()
		{
			Section reportSection = null;
			
			try
			{
				//	Create a new report document
				m_cdrDocument = new ReportDocument();
					
				//	Load the report template
				m_cdrDocument.Load(m_strTemplate);
					
				//	Notify the owner
				if(TemplateLoaded != null)
					TemplateLoaded(this, System.EventArgs.Empty);
					
				//	Suppress any sections that have been requested
				foreach(string O in m_aSuppressedSections)
				{
					if((reportSection = GetSection(m_cdrDocument, O)) != null)
					{
						if(reportSection.SectionFormat != null)
						{
							try { reportSection.SectionFormat.EnableSuppress = true; }
							catch {}
						}
					
					}
				}
				
				//	Set the report data
				m_cdrDocument.SetDataSource(ReportSource);
						
				//	Make this report the source for the viewer
				m_ctrlViewer.ReportSource = m_cdrDocument;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Preview", m_tmaxErrorBuilder.Message(ERROR_PREVIEW_EX), Ex);
			}

		}// private void Preview()
		
		/// <summary>Called to translate the TrialMax format to Crystal Reports format type enumeration</summary>
		/// <param name="eFormat">the TrialMax enumerated format type</param>
		/// <returns>the Crystal Reports enumerated format type</returns>
		ExportFormatType GetExportType(TmaxExportReportFormats eFormat)
		{
			ExportFormatType eType = ExportFormatType.PortableDocFormat;
			
			switch(eFormat)
			{
				case TmaxExportReportFormats.Adobe:
					eType = ExportFormatType.PortableDocFormat;
					break;
					
				case TmaxExportReportFormats.Word:
					eType = ExportFormatType.WordForWindows;
					break;
					
				case TmaxExportReportFormats.Excel:
					eType = ExportFormatType.Excel;
					break;
					
				case TmaxExportReportFormats.HTML:
					eType = ExportFormatType.HTML40;
					break;
					
				case TmaxExportReportFormats.None:
					Debug.Assert(false, "No export format specified");
					eType = ExportFormatType.NoFormat;
					break;
					
				default:
					Debug.Assert(false, "Unhandled export format type: " + eFormat.ToString());
					eType = ExportFormatType.NoFormat;
					break;
					
			}// switch(eFormat)
			
			return eType;
		
		}// ExportFormatType GetExportType(TmaxExportReportFormats eFormat)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Collection containing the names of sections to be suppressed</summary>
		public ArrayList SuppressedSections
		{
			get { return m_aSuppressedSections; }
		}
		
		/// <summary>Report document obtained after loading the template</summary>
		public CrystalDecisions.CrystalReports.Engine.ReportDocument Document
		{
			get { return m_cdrDocument; }
		}
		
		/// <summary>Data set object contain report source data</summary>
		public System.Data.DataSet ReportSource
		{
			get { return m_dsReportSource; }
			set { m_dsReportSource = value; }
		}
		
		/// <summary>Fully qualified path to the report template</summary>
		public string Template
		{
			get { return m_strTemplate; }
			set { m_strTemplate = value; }
		}
		
		/// <summary>Title to be displayed in the report preview form</summary>
		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value; }
		}
		
		/// <summary>True to enable the groups capabilities of the Crystal Reports viewer</summary>
		public bool EnableGroups
		{
			get { return m_bEnableGroups; }
			set { m_bEnableGroups = value; }
		}
		
		/// <summary>True to show the OK/Cancel buttons</summary>
		public bool ShowButtons
		{
			get { return m_bShowButtons; }
			set { m_bShowButtons = value; }
		}
		
		#endregion Properties
		
	}// public class CRFViewer : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Reports
