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
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Reports
{
	/// <summary>This form is the base class for all report forms</summary>
	public class CRFBase : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_RFBASE_NO_SCHEMA				= (ERROR_TMAX_FORM_MAX + 1);
		protected const int ERROR_RFBASE_SCHEMA_NOT_FOUND		= (ERROR_TMAX_FORM_MAX + 2);
		protected const int ERROR_RFBASE_CREATE_DATA_SET_EX		= (ERROR_TMAX_FORM_MAX + 3);
		protected const int ERROR_RFBASE_EXECUTE_EX				= (ERROR_TMAX_FORM_MAX + 4);
		protected const int ERROR_RFBASE_PREVIEW_EX				= (ERROR_TMAX_FORM_MAX + 5);
		protected const int ERROR_RFBASE_EXPORT_EX				= (ERROR_TMAX_FORM_MAX + 6);
		protected const int ERROR_RFBASE_RESERVED_7				= (ERROR_TMAX_FORM_MAX + 7);
		protected const int ERROR_RFBASE_RESERVED_8				= (ERROR_TMAX_FORM_MAX + 8);
		
		protected const int ERROR_RFBASE_MAX = ERROR_RFBASE_RESERVED_8;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>The viewer used to execute the report</summary>
		protected CRFViewer m_ctrlViewer = null;
		
		/// <summary>Local member bound to Items property</summary>
		protected FTI.Shared.Trialmax.CTmaxItems m_tmaxItems = null;
		
		/// <summary>Local member bound to Database property</summary>
		protected FTI.Trialmax.Database.CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member bound to Template property</summary>
		protected string m_strTemplate = "";
		
		/// <summary>Local member bound to XmlSchema property</summary>
		protected string m_strXmlSchema = "";
		
		/// <summary>Local member bound to SourceFolder property</summary>
		protected string m_strSourceFolder = "";
		
		/// <summary>Local member bound to ReportSource property</summary>
		protected System.Data.DataSet m_dsReportSource = null;
		
		/// <summary>The active template</summary>
		protected CROTemplate m_roTemplate = null;
		
		/// <summary>Folder to keep track of exported files</summary>
		protected CTmaxSourceFolder m_tmaxExported = new CTmaxSourceFolder();
		
		/// <summary>Status window used for export operations</summary>
		protected CRFExportStatus m_wndStatus = null;
		
		#endregion Protected Members
		
		#region Public Methods

		/// <summary>Constructor</summary>
		public CRFBase() : base()
		{
		}

		/// <summary>This method is called by the manager to verify that the form can be opened</summary>
		/// <returns>true if the form has what it needs to run the report</returns>
		public virtual bool CanExecute()
		{
			return true;
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method will run the report using the current options</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected virtual bool Execute(int iReportIndex, bool bOnePerPrimary)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Prepare the report for execution
				if(Prepare(iReportIndex, bOnePerPrimary) != false)
				{
					//	Are we supposed to export this report?
					if(GetOptions().ExportFormat != TmaxExportReportFormats.None)
					{
						//	Are we supposed to preview before exporting?
						if(GetOptions().PreviewExports == true)
						{
							if(Preview(iReportIndex, bOnePerPrimary, true) != DialogResult.Abort)
							{
								bSuccessful = Export(iReportIndex, bOnePerPrimary);
							}
							
						}
						else
						{
							bSuccessful = Export(iReportIndex, bOnePerPrimary);
						
						}// if(GetOptions().PreviewExports == true)
						
					}
					else
					{
						//	Default execution is to preview the report
						if(Preview(iReportIndex, bOnePerPrimary, false) != DialogResult.Abort)
							bSuccessful = true;
					}
					
				}// if(Prepare(iReportIndex) != false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Execute", m_tmaxErrorBuilder.Message(ERROR_RFBASE_EXECUTE_EX), Ex);
			}

			//	Notify the derived class that we are done
			OnComplete(iReportIndex, bOnePerPrimary);
				
			return bSuccessful;
		
		}// protected virtual bool Execute()
		
		/// <summary>This method is called to prepare the viewer to run the report</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected virtual bool Prepare(int iReportIndex, bool bOnePerPrimary)
		{
			return true;
						
		}// protected virtual bool Prepare(int iReportIndex, bool bOnePerPrimary)
		
		/// <summary>Called to notify the derived class when finished with a report</summary>
		/// <param name="iReportIndex">The index of the report that is complete</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		protected virtual void OnComplete(int iReportIndex, bool bOnePerPrimary)
		{
			try
			{
				//	Clean up
				m_dsReportSource = null;
				m_ctrlViewer = null;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnComplete", Ex);
			}

		}// protected virtual void OnComplete(int iReportIndex, bool bOnePerPrimary)
		
		/// <summary>This method is called to exchange values between the form control and the local options object</summary>
		/// <param name="bSetControls">true to set the control values</param>
		/// <returns>true if successful</returns>
		protected virtual bool Exchange(bool bSetControls)
		{
			return true;
						
		}// private bool Exchange(bool bSetControls)
		
		/// <summary>This method is called to get the records used to populate the source data set</summary>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>the number of reports to be run</returns>
		protected virtual int GetSourceRecords(bool bOnePerPrimary)
		{
			return 1;
		
		}// protected virtual bool GetSourceRecords()
		
		/// <summary>This method is called to populate the report's data set</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected virtual bool FillDataSet(int iReportIndex, bool bOnePerPrimary)
		{
			return true;
		
		}// protected virtual bool FillDataSet()
		
		/// <summary>Called to populate the specified drop list with the export format options</summary>
		/// <param name="comboBox">The list to be populated</param>
		/// <param name="eSelection">The initial selection</param>
		protected virtual void FillExportFormats(ComboBox comboBox, TmaxExportReportFormats eSelection)
		{
			int iIndex = 0;
			
			try
			{
				//	Clear the existing list
				comboBox.Items.Clear();
				
				foreach(TmaxExportReportFormats O in Enum.GetValues(typeof(TmaxExportReportFormats)))
				{
					comboBox.Items.Add(CROBase.GetDisplayString(O));
				
				}// foreach(TmaxCodedProperties O in Enum.GetValues(typeof(TmaxCodedProperties)))
			
				if(comboBox.Items.Count > 0)
					iIndex = comboBox.FindStringExact(CROBase.GetDisplayString(eSelection));
				if(iIndex >= 0)
					comboBox.SelectedIndex = iIndex;
				else
					comboBox.SelectedIndex = 0;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FillExportFormats", Ex);
			}
			
		}// protected virtual void FillExportFormats(ComboBox comboBox, TmaxExportReportFormats eSelection)
		
		/// <summary>Called to get the export format selected in the specified drop list</summary>
		/// <param name="comboBox">The list populated with the export formats</param>
		/// <param name="eSelection">The selected format</param>
		protected virtual TmaxExportReportFormats GetExportFormat(ComboBox comboBox)
		{
			try
			{
				// NOTE:	We could cast the selected index to the enumerated value
				//			but I might want to sort this list box in the
				//			future so I'll just assume the box is sorted now
				if(comboBox.SelectedIndex >= 0)
				{
					foreach(TmaxExportReportFormats O in Enum.GetValues(typeof(TmaxExportReportFormats)))
					{
						if(comboBox.Items[comboBox.SelectedIndex].ToString() == CROBase.GetDisplayString(O))
							return O;
				
					}// foreach(TmaxCodedProperties O in Enum.GetValues(typeof(TmaxCodedProperties)))
			
				}// if(comboBox.SelectedText.Length > 0)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetExportFormat", Ex);
			}
			
			//	No selection
			return TmaxExportReportFormats.None;
			
		}// protected virtual TmaxExportReportFormats GetExportFormat(ComboBox comboBox)
		
		/// <summary>Called to verify that the export folder specified by the user is valid for the operation</summary>
		/// <param name="eFormat">The export format to be used</param>
		/// <param name="strFolder">The folder specified by the user</param>
		/// <returns>True if successful</returns>
		protected virtual bool CheckExportFolder(TmaxExportReportFormats eFormat, string strFolder)
		{
			//	Don't bother if not exporting the report(s)
			if(eFormat == TmaxExportReportFormats.None) return true;
			
			//	Assume using the working directory if no folder specified
			if(strFolder.Length == 0) return true;
			
			//	Does this folder exist?
			if(System.IO.Directory.Exists(strFolder) == false)
			{
				//	Attempt to create the folder
				try
				{
					System.IO.Directory.CreateDirectory(strFolder);
				}
				catch
				{
					return Warn("Unable to create the export format folder: " + strFolder);
				}
				
			}
			
			return true; // All is good ...
		
		}// protected virtual bool CheckExportFolder(TmaxExportReportFormats eFormat, string strFolder)
		
		/// <summary>This method is called to set the the report properties based on the user options</summary>
		/// <returns>true if successful</returns>
		protected virtual bool SetReportProps()
		{
			string	strMsg = "";
			CROBase	roOptions = GetOptions();
			
			Debug.Assert(roOptions != null);
			if(roOptions == null) return false;
			
			//	Did the user specify an alternate report template?
			if(roOptions.Alternate.Length > 0)
			{
				m_strTemplate  = GetFileSpec(roOptions.Alternate);
				m_strXmlSchema = GetFileSpec(roOptions.GetDefaultXmlSchema());
			}
			else
			{
				Debug.Assert(m_roTemplate != null);
				if(m_roTemplate == null) return false;
				
				m_strTemplate = GetFileSpec(m_roTemplate.Filename);
				m_strXmlSchema = GetFileSpec(m_roTemplate.XmlSchema);
			}
				
			//	Does the template exist
			if(System.IO.File.Exists(m_strTemplate) == false)
			{
				strMsg = ("Unable to locate the report template: " + m_strTemplate);
				MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			
			//	Does the data source file exist
			if(System.IO.File.Exists(m_strXmlSchema) == false)
			{
				strMsg = ("Unable to locate the report data source file: " + m_strXmlSchema);
				MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			return true;
		
		}// protected virtual bool SetReportProps()
		
		/// <summary>This method is called to create the report's data set</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected virtual bool CreateDataSet(int iReportIndex, bool bOnePerPrimary)
		{
			try
			{
				Debug.Assert(m_strXmlSchema.Length > 0);
				if(m_strXmlSchema.Length == 0) 
				{
					m_tmaxEventSource.FireElapsed(this, "CreateDataSet", m_tmaxErrorBuilder.Message(ERROR_RFBASE_NO_SCHEMA));
					return false;
				}
							
				//	Make sure the schema file exists
				if(System.IO.File.Exists(m_strXmlSchema) == false)
				{
					m_tmaxEventSource.FireElapsed(this, "CreateDataSet", m_tmaxErrorBuilder.Message(ERROR_RFBASE_SCHEMA_NOT_FOUND, m_strXmlSchema));
					return false;
				}
				
				//	Create the new data set
				m_dsReportSource = new DataSet(System.IO.Path.GetFileNameWithoutExtension(m_strXmlSchema));
			
				m_dsReportSource.ReadXmlSchema(m_strXmlSchema);
				
				m_dsReportSource.EnforceConstraints = false;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDataSet", m_tmaxErrorBuilder.Message(ERROR_RFBASE_CREATE_DATA_SET_EX), Ex);
				return false;
			}
		
		}// protected virtual bool CreateDataSet()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("No XML schema file has been specified to create the report's data set");
			m_tmaxErrorBuilder.FormatStrings.Add("The report's data schema file could not be found:\nFilename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the report's data set.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while executing the report.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while previewing the report.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the report to %1  <format = %2>");
			m_tmaxErrorBuilder.FormatStrings.Add("CRFBase reserved error 7");
			m_tmaxErrorBuilder.FormatStrings.Add("CRFBase reserved error 8");
		
		}// protected override void SetErrorStrings()

		/// <summary>This method is called to get the highlighter color as an OLE_COLOR value</summary>
		/// <param name="iIndex">Index of the desired highlighter</param>
		/// <returns>The color as an OLE_COLOR value</returns>
		protected int GetOleHighlighter(int iIndex)
		{
			int	iOleColor = 0;
			
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			{
				if((iIndex >= 0) && (iIndex < m_tmaxDatabase.Highlighters.Count))
				{
					iOleColor = m_tmaxDatabase.Highlighters[iIndex].OleColor;
				}
				
			}

			return iOleColor;
			
		}// protected int GetOleHighlighter(int iIndex)
		
		/// <summary>This method handles the OK button's click event</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected virtual void OnClickOK(object sender, System.EventArgs e)
		{
			bool	bSuccessful = false;
			bool	bOnePerPrimary = false;
			bool	bExporting = false;
			int		iReports = 0;
			int		iIndex = 0;
			string	strMsg = "";
			
			try
			{
				while(true)
				{
					Cursor.Current = Cursors.WaitCursor;

					//	Get the control values
					if(Exchange(false) == false) break;
					
					//	Use the operator selections to set the report properties
					if(SetReportProps() == false) break;
					
					//	Are we splitting up the records for multiple reports?
					if(GetOptions().ExportFormat != TmaxExportReportFormats.None)
					{
						bExporting = true;
						m_tmaxExported.Initialize(GetOptions().ExportFolder);
						m_tmaxExported.Files.Clear();
						
						if(GetOptions().SplitExports == true)
							bOnePerPrimary = true;
							
						//	Display the status form if not previewing
						if(GetOptions().PreviewExports == false)
						{
							try
							{
								m_wndStatus = new CRFExportStatus();
								m_wndStatus.Message = "Exporting reports ...";
								m_wndStatus.Show();
							}
							catch{}
						}
						
					}// if(GetOptions().ExportFormat != TmaxExportReportFormats.None)
					
					//	Get the records to be used for the report(s)
					if((iReports = GetSourceRecords(bOnePerPrimary)) == 0) break;
					
					Cursor.Current = Cursors.Default;

					//	Run the reports
					for(iIndex = 0; iIndex < iReports; iIndex++)
					{
						Cursor.Current = Cursors.WaitCursor;

						//	Create the data set used to populate the report source
						if(CreateDataSet(iIndex, bOnePerPrimary) == false) 
							break;
					
						//	Fill the source data set
						if(FillDataSet(iIndex, bOnePerPrimary) == false) 
							break;
					
						Cursor.Current = Cursors.Default;
					
						//	Run the report
						if(Execute(iIndex, bOnePerPrimary) == false) 
							break;
							
					}// for(int iIndex = 0; iIndex < iReports; iIndex++)
					
					//	Did we run all the reports?
					if(iIndex >= iReports)
						bSuccessful = true;
						
					//	All done
					break;
					
				}// while(true)
				
			}
			catch(System.Exception Ex)
			{
				Cursor.Current = Cursors.Default;
				m_tmaxEventSource.FireError(this, "OnClickOK", m_tmaxErrorBuilder.Message(ERROR_RFBASE_EXECUTE_EX), Ex);
			}
			
			if(m_wndStatus != null)
			{
				try
				{
					m_wndStatus.Close();
					m_wndStatus = null;
				}
				catch{}
			}
			
			if(bSuccessful == true)
			{
				//	Should we display a summary message?
				if((bExporting == true) && (m_tmaxExported.Files.Count > 0))
				{
					strMsg  = ("Exported " + m_tmaxExported.Files.Count.ToString() + " report(s) to: \n\n");
					strMsg += (m_tmaxExported.Path + "\n\n");
					
					foreach(CTmaxSourceFile O in m_tmaxExported.Files)
						strMsg += (O.Name + "\n");
						
					CRFExportSummary summary = new CRFExportSummary();
					summary.Exported = m_tmaxExported;
					summary.ShowDialog();
					
					//MessageBox.Show(strMsg, "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				
				// Close the form if successful
				DialogResult = DialogResult.OK;
				this.Close();
			}
		
		}// protected virtual void OnClickOK(object sender, System.EventArgs e)
		
		/// <summary>Called to allow the user to browse for a folder</summary>
		/// <param name="strPrompt">The prompt displayed in the selection form</param>
		/// <param name="rFolder">The string containing the initial folder and the folder selection on return</param>
		/// <returns>true if the dialog result is OK</returns>
		protected virtual bool BrowseForFolder(string strPrompt, ref string rFolder)
		{
			FTI.Shared.CBrowseForFolder bff = new CBrowseForFolder();
			bool						bSuccessful = false;
			
			bff.Folder = rFolder;
			bff.Prompt = strPrompt;
			bff.NoNewFolder = false;
			
			if(bff.ShowDialog(this.Handle) == DialogResult.OK)
			{
				rFolder = bff.Folder.ToLower();
				if((rFolder.EndsWith("\\") == false) &&
				   (rFolder.EndsWith("/") == false))
				{
					rFolder += "\\";
				}
				
				bSuccessful = true;
			}
			
			return bSuccessful;
			
		}// protected virtual bool BrowseForFolder(string strPrompt, ref string rFolder)
		
		/// <summary>This method handles the event fired when the user clicks on the Browse button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected virtual bool BrowseForTemplate(ref string strFilename)
		{
			OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
			
			//	Initialize the file selection dialog
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.Multiselect = false;
			dlg.Title = "Select Report Template";
			dlg.Filter = "Crystal Reports (*.rpt)|*.rpt|All Files (*.*)|*.*";
			dlg.FileName = strFilename;
			
			//	Open the dialog box
			if(dlg.ShowDialog() == DialogResult.OK) 
			{
				strFilename = dlg.FileName.ToLower();
				return true;
			}
			else
			{
				return false; // User canceled
			}
			
		}// protected virtual bool BrowseForTemplate(ref string strFilename)

		/// <summary>This method is called to open the report in the viewer window</summary>
		/// <param name="iReportIndex">The index of the report being executed</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <param name="bExporting">true if previewing for export</param>
		/// <returns>true if successful</returns>
		protected virtual DialogResult Preview(int iReportIndex, bool bOnePerPrimary, bool bExporting)
		{
			DialogResult result = DialogResult.Abort;
			
			try
			{
				if(m_ctrlViewer != null)
				{
					//	Turn on the OK/Cancel buttons if previewing for export
					m_ctrlViewer.ShowButtons = bExporting;
					
					if((result = m_ctrlViewer.ShowDialog()) == DialogResult.Cancel)
					{
						//	Abort the operation if exporting
						if(bExporting == true)
							result = DialogResult.Abort;
					}
					
				}// if(m_ctrlViewer != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Preview", m_tmaxErrorBuilder.Message(ERROR_RFBASE_PREVIEW_EX), Ex);
			}
			
			return result;

		}// protected virtual DialogResult Preview(int iReportIndex, bool bOnePerPrimary)
		
		/// <summary>This method is called to export the specified report</summary>
		/// <param name="iReportIndex">The index of the report being exported</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>true if successful</returns>
		protected virtual bool Export(int iReportIndex, bool bOnePerPrimary)
		{
			bool	bSuccessful = false;
			string	strFileSpec = "";
			
			try
			{
				//	Assume the viewer has been prepared with the appropriate report
				if(m_ctrlViewer != null)
				{
					//	Get the path to the export file
					strFileSpec = GetExportFileSpec(iReportIndex, GetOptions().ExportFolder, GetOptions().ExportFormat, bOnePerPrimary);
					
					if((strFileSpec != null) && (strFileSpec.Length > 0))
					{
						if(m_wndStatus != null)
							m_wndStatus.Message = ("Exporting " + strFileSpec);
						
						bSuccessful = m_ctrlViewer.Export(strFileSpec, GetOptions().ExportFormat);
				
					}// if((strFileSpec != null) && (strFileSpec.Length > 0))
					
					//	Should we add this file to our list?
					if((bSuccessful == true) && (m_tmaxExported != null))
					{
						m_tmaxExported.Files.Add(new CTmaxSourceFile(strFileSpec));
					}
					
				}// if(m_ctrlViewer != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Export", m_tmaxErrorBuilder.Message(ERROR_RFBASE_EXPORT_EX, strFileSpec, GetOptions().ExportFormat), Ex);
			}
			
			return bSuccessful;

		}// protected virtual bool Export(int iReportIndex, bool bOnePerPrimary)
		
		/// <summary>This method is called to get the path to the export file for the specified report</summary>
		/// <param name="iReportIndex">The index of the report being exported</param>
		/// <param name="strFolder">The path to the folder where exported reports should be stored</param>
		/// <param name="eFormat">The format of the file being exported</param>
		/// <param name="bOnePerPrimary">true if one report per primary record</param>
		/// <returns>The fully qualified path</returns>
		protected virtual string GetExportFileSpec(int iReportIndex, string strFolder, TmaxExportReportFormats eFormat, bool bOnePerPrimary)
		{
			return "";
		}
		
		/// <summary>This method is called to get the options for the report</summary>
		/// <returns>The options object associated with the report</returns>
		/// <remarks>This MUST be overridden by the derived class</remarks>
		protected virtual CROBase GetOptions()
		{
			return null;

		}// protected virtual CROBase GetOptions()
		
		/// <summary>This method is called to get the fully qualified path to the specified file</summary>
		///	<param name="strFilename">The name of the file</param>
		/// <returns>The fully qualified path to the file</returns>
		protected virtual string GetFileSpec(string strFilename)
		{
			string strFileSpec = "";
			
			//	If no path specified assume it's in the reports folder
			if(System.IO.Path.IsPathRooted(strFilename) == false)
			{
				strFileSpec = m_strSourceFolder;
				if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
					strFileSpec += "\\";
				strFileSpec += strFilename;
			}
			else
			{
				//	File is already fully qualified
				strFileSpec = strFilename;
			}
			
			return strFileSpec;
		
		}// protected virtual string GetFileSpec(string strFilename)

		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>The active database</summary>
		public FTI.Trialmax.Database.CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
			
		}
		
		/// <summary>Items to define records to be included in the report</summary>
		public FTI.Shared.Trialmax.CTmaxItems Items
		{
			get { return m_tmaxItems; }
			set { m_tmaxItems = value; }
			
		}
		
		/// <summary>Crystal reports template used to generate the report</summary>
		public string Template
		{
			get { return m_strTemplate; }
			set { m_strTemplate = value; }
			
		}
		
		/// <summary>Path to the file containing the XML schema used to create the data set</summary>
		public string XmlSchema
		{
			get { return m_strXmlSchema; }
			set { m_strXmlSchema = value; }
			
		}
		
		/// <summary>The folder containing the report source files</summary>
		public string SourceFolder
		{
			get { return m_strSourceFolder; }
			set { m_strSourceFolder = value; }
			
		}
		
		/// <summary>The folder containing the report source files</summary>
		public System.Data.DataSet ReportSource
		{
			get { return m_dsReportSource; }
			set { m_dsReportSource = value; }
			
		}
		
		#endregion Properties
	
	}// public class RFTranscript : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Reports
