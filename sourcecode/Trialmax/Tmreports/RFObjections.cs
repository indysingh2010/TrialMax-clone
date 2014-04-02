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
using System.Threading;

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
	public class CRFObjections : CRFBase
	{
		#region Constants

		private const string XML_ORE_SETUP_FILENAME		= "tmax_ore_setup.xml";
		private const string XML_ORE_PROGRESS_FILENAME	= "tmax_ore_progress.txt";
		private const string XML_TEMPLATE_SECTION_NAME	= "ORE TEMPLATE";
		
		private const string TEMPLATE_FORMAT_LINE				= "Format";
		private const string TEMPLATE_FLAGS_ENABLED_LINE		= "FlagsEnabled";
		private const string TEMPLATE_EXTENSION_LINE			= "Extension";
		private const string TEMPLATE_VISIBLE_LINE				= "Visible";
		private const string TEMPLATE_SINGLE_DEPOSITION_LINE	= "SingleDeposition";

		/// <summary>Filter identifiers (must match order in drop list)</summary>
		private const int OBJECTIONS_FILTER_ALL			= 0;
		private const int OBJECTIONS_FILTER_DEFENSE		= 1;
		private const int OBJECTIONS_FILTER_PLAINTIFF	= 2;
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_EXCHANGE_EX				= (ERROR_RFBASE_MAX + 1);
		private const int ERROR_EXECUTE_EX				= (ERROR_RFBASE_MAX + 2);
		private const int ERROR_PREPARE_EX				= (ERROR_RFBASE_MAX + 3);
		private const int ERROR_ADD_SOURCE_EX			= (ERROR_RFBASE_MAX + 4);
		private const int ERROR_CREATE_XML_SOURCE_EX	= (ERROR_RFBASE_MAX + 5);
		private const int ERROR_ADD_XML_SCENE_EX		= (ERROR_RFBASE_MAX + 6);
		private const int ERROR_SET_XML_FOLDER_FAILED	= (ERROR_RFBASE_MAX + 7);
		private const int ERROR_SET_XML_FOLDER_EX		= (ERROR_RFBASE_MAX + 8);
		private const int ERROR_SAVE_XML_SCRIPT_FAILED	= (ERROR_RFBASE_MAX + 9);
		private const int ERROR_CREATE_XML_SETUP_FAILED = (ERROR_RFBASE_MAX + 10);
		private const int ERROR_SET_ORE_OPTIONS_FAILED	= (ERROR_RFBASE_MAX + 11);
		private const int ERROR_SAVE_ORE_SETUP_EX		= (ERROR_RFBASE_MAX + 12);
		private const int ERROR_GENERATE_EX				= (ERROR_RFBASE_MAX + 13);
		private const int ERROR_OPEN_ORE_TEMPLATE_EX	= (ERROR_RFBASE_MAX + 14);
		private const int ERROR_SET_FORMAT_EX			= (ERROR_RFBASE_MAX + 15);

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;

		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Options for generating the report</summary>
		private CROObjections m_reportOptions = null;

		/// <summary>Check box bound to IncludeSubBinders option</summary>
		private System.Windows.Forms.CheckBox m_ctrlIncludeSubBinders;

		/// <summary>Group box for controls related to report content</summary>
		private System.Windows.Forms.GroupBox m_ctrlContentGroup;

		/// <summary>Group box for controls related to report style</summary>
		private System.Windows.Forms.GroupBox m_ctrlFilesGroup;

		/// <summary>Combobox for list of available templates</summary>
		private System.Windows.Forms.ComboBox m_ctrlTemplates;

		/// <summary>Static text label for Templates drop-list</summary>
		private Label m_ctrlTemplatesLabel;

		/// <summary>Static text label for export folder text box</summary>
		private System.Windows.Forms.Label m_ctrlOutputFolderLabel;

		/// <summary>Text box to specify path to the export folder</summary>
		private System.Windows.Forms.TextBox m_ctrlOutputFolder;

		/// <summary>Pushbutton to open folder browser for the export folder path</summary>
		private System.Windows.Forms.Button m_ctrlBrowseOutputFolder;

		/// <summary>Image list used for button images</summary>
		private ImageList m_ctrlImages;

		/// <summary>Check box to allow user to set the Add Media Name option</summary>
		private CheckBox m_ctrlAddMediaName;

		/// <summary>Form used to display operation status</summary>
		new private CRFObjectionsStatus m_wndStatus = null;

		/// <summary>Local member associated with the ProductManager property</summary>
		private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;

		/// <summary>Exchange interfaces for all scripts in the report</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxScripts = new CTmaxItems();

		/// <summary>Member to store the fully qualified path to the ORE executable</summary>
		private string m_strOREFileSpec = "";

		/// <summary>Member to store the fully qualified path to the ORE progress file</summary>
		private string m_strOREProgressFileSpec = "";

		/// <summary>Local member to store the path to the temporary folder where XML scripts get stored</summary>
		private string m_strXmlFolder = "";

		/// <summary>Local member to store the path to the temporary ORE XML configuration file</summary>
		private string m_strXmlORESetup = "";

		/// <summary>Flag to indicate if the user has aborted the operation</summary>
		private bool m_bAborted = false;

		/// <summary>Group box for report options controls</summary>
		private GroupBox m_ctrlOptionsGroup;

		/// <summary>Collection of XML scripts used to generate the reports</summary>
		private CXmlScripts m_xmlScripts = new CXmlScripts();

		/// <summary>Check list of available option flags</summary>
		private CheckedListBox m_ctrlFlags;

		/// <summary>Static text label for boolean report options</summary>
		private Label m_ctrlFlagsLabel;

		/// <summary>Static text label for short case name edit box</summary>
		private Label m_ctrlShortCaseNameLabel;

		/// <summary>Edit box for short case name</summary>
		private TextBox m_ctrlShortCaseName;

		/// <summary>Static text label for report title edit box</summary>
		private Label m_ctrlTitleLabel;

		/// <summary>Edit box for report title</summary>
		private TextBox m_ctrlTitle;

		/// <summary>Static text label for long case name edit box</summary>
		private Label m_ctrlLongCaseNameLabel;

		/// <summary>Edit box for long case name</summary>
		private TextBox m_ctrlLongCaseName;

		/// <summary>Group box for filter options</summary>
		private GroupBox m_ctrlFilterGroup;

		/// <summary>Combo-box to allow user to select the filter mode</summary>
		private ComboBox m_ctrlFilter;

		/// <summary>Total number of reports generated during the operation</summary>
		private long m_lGenerated = 0;

		/// <summary>Local flag to indicate that only one deposition per report</summary>
		private bool m_bOneDeposition = false;

		/// <summary>Local flag to indicate how the progress bar should be updated</summary>
		private bool m_bStepProgress = false;

		/// <summary>The active ORE template</summary>
		private CTmaxORETemplate m_tmaxORETemplate = null;
		
		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CRFObjections() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
		}

		/// <summary>This method is called by the manager to verify that the form can be opened</summary>
		/// <returns>true if the form has what it needs to run the report</returns>
		public override bool CanExecute()
		{
			bool	bSuccessful = false;
			string	strMsg = "";

			Debug.Assert(m_tmaxDatabase != null);
			if(m_tmaxDatabase == null) return false;

			while(bSuccessful == false)
			{
				//	Do the base class processing
				if(base.CanExecute() == false)
					break;

				//	Make sure we have a collection of objections to work with
				if((m_tmaxDatabase.Objections == null) || (m_tmaxDatabase.Objections.Count == 0))
				{
					strMsg = "No objections are available for the report.";
					break;
				}
			
				//	The caller must have provided an item collection
				if((m_tmaxItems == null) || (m_tmaxItems.Count == 0))
				{
					strMsg = "No records have been specified for the report";
					break;
				}
			
				//	Must be able to get to the Objections Report Engine executable
				if(GetOREFileSpec() == false) break;

				bSuccessful = true; // It's all good ...

			}// while(bSuccessful == false)

			//	Should we display an error message?
			if(strMsg.Length > 0)
			{
				MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return bSuccessful;
			
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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the report options");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to execute the report.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prepare the report.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while processing a source record: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create an XML script for the source record.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a scene to the XML script: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the temporary folder for the XML scripts: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the folder for the temporary XML files.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to save the XML script to generate the report: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the XML setup file to configure the report engine: %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to save the XML setup file to configure the report engine: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the ORE configuration options.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while generating the report for %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the ORE template configuration file: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the report format");

		}// protected override void SetErrorStrings()

		/// <summary>Overloaded base class member to do custom initialization when the form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			Debug.Assert(m_reportOptions != null);

			//	Initialize the controls
			Exchange(true);

			//	Do the base class processing
			base.OnLoad(e);

		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method handles the Click event fired by the export</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnBrowseOutputFolder(object sender, System.EventArgs e)
		{
			string strFolder = m_ctrlOutputFolder.Text;

			if(BrowseForFolder("Choose Output Folder", ref strFolder) == true)
				m_ctrlOutputFolder.Text = strFolder;

		}// protected void OnBrowseOutputFolder(object sender, System.EventArgs e)

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
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
			base.Dispose(disposing);

		}// protected override void Dispose( bool disposing )

		/// <summary>This method is called to exchange values between the form control and the local options object</summary>
		/// <param name="bSetControls">true to set the control values</param>
		/// <returns>true if successful</returns>
		protected override bool Exchange(bool bSetControls)
		{
			CTmaxOREOption tmaxOREOption = null;
			
			try
			{
				//	Are we setting the control values?
				if(bSetControls == true)
				{
					//	Force a refresh of the ORE template files
					Options.ORETemplates.Clear();
						
					//	Populate the templates combo box
					m_ctrlTemplates.DataSource = Options.Templates;

					m_ctrlIncludeSubBinders.Checked = Options.IncludeSubBinders;
					m_ctrlAddMediaName.Checked = Options.AddMediaName;
					m_ctrlOutputFolder.Text = Options.ExportFolder;
					m_ctrlFilter.SelectedIndex = Options.FilterIndex;
					
					if(Options.LastORETemplate != null)
						m_ctrlTitle.Text = Options.LastORETemplate.GetValue(CTmaxORETemplate.ORE_COMMON_TITLE);

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

					//	Set the case names
					//
					//	NOTE:	This MUST be done AFTER setting the active template
					SetCaseNames();
					
					//	Should we initialize the flags?
					if((m_tmaxORETemplate != null) && (Options.LastORETemplate != null))
					{
						foreach(CTmaxOREOption O in Options.LastORETemplate.Flags)
						{
							if((tmaxOREOption = m_tmaxORETemplate.Flags.Find(O.ORELabel, true)) != null)
								tmaxOREOption.Value = O.Value;
						}
						
						//	Update the list box
						SetSelections(m_ctrlFlags);

					}// if((m_tmaxORETemplate != null) && (Options.LastORETemplate != null))
					
				}
				else
				{
					//	Make sure the output folder is valid
					if(CheckOutputFolder(m_ctrlOutputFolder.Text) == false)
					{
						m_ctrlOutputFolder.Focus();
						return false;
					}

					Options.CaseId = m_tmaxDatabase.Detail.UniqueId;
					Options.IncludeSubBinders = m_ctrlIncludeSubBinders.Checked;
					Options.AddMediaName = m_ctrlAddMediaName.Checked;
					Options.Template = m_ctrlTemplates.SelectedIndex + 1;
					Options.ExportFolder = m_ctrlOutputFolder.Text;
					Options.FilterIndex = m_ctrlFilter.SelectedIndex;
					if(Options.FilterIndex < 0)
						Options.FilterIndex = OBJECTIONS_FILTER_ALL;
					
					if(m_tmaxORETemplate != null)
					{
						if(m_ctrlTitle.Enabled == true)
							m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_COMMON_TITLE, m_ctrlTitle.Text);
							
						if(m_ctrlShortCaseName.Enabled == true)
							m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_COMMON_SHORT_CASE_NAME, m_ctrlShortCaseName.Text);

						if(m_ctrlLongCaseName.Enabled == true)
							m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_COMMON_LONG_CASE_NAME, m_ctrlLongCaseName.Text);

						//	Get the flags selected by the user
						GetSelections(m_ctrlFlags);

					}// if(m_tmaxORETemplate != null)
					
					//	Make sure these settings get saved with the options
					Options.LastORETemplate = m_tmaxORETemplate;
	
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

		/// <summary>This method handles the OK button's click event</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected override void OnClickOK(object sender, System.EventArgs e)
		{
			System.Threading.Thread executionThread = null;
			int						iAttempts = 0;
			
			//	Get the user selections
			if(Exchange(false) == false) return;

			//	Create the status form
			if(CreateStatusForm() == true)
			{
				try
				{
					//	Start the operation
					executionThread = new Thread(new ThreadStart(this.Execute));
					executionThread.Start();

					//	Move this form off screen
					//
					//	NOTE:	Making it invisible messes with the focus when the operation
					//			finishes. Sometimes the application's main window minimizes
					this.Location = new Point(10000,10000);
					
					//	Block the caller until operation is complete or the user aborts
					while(m_bAborted == false)
					{
						Thread.Sleep(500);
						Application.DoEvents();

						//	Has the operation finished?
						if(m_wndStatus.Finished == true)
							break;
						else
							m_bAborted = m_wndStatus.Aborted;

					}// while(m_bAborted == false)
					
					//	Make sure the thread has terminated
					while(executionThread.ThreadState == System.Threading.ThreadState.Running)
					{
						//	Crude test for timeout
						if(iAttempts < 20)
						{
							iAttempts++;
						}
						else
						{
							//	Attempt to terminate the thread
							try { executionThread.Abort(); }
							catch {}
							
							break;
						}

					}// while(executionThread.ThreadState == System.Threading.ThreadState.Running)

				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickOK", m_tmaxErrorBuilder.Message(ERROR_RFBASE_EXECUTE_EX), Ex);
				}

				//	Make sure the status window is closed
				if(m_wndStatus.Visible == true)
				{
					//	Close the form
					try { m_wndStatus.Close(); }
					catch { };

				}// if(m_wndStatus.Visible == true)
				
			}
			else
			{
				Cursor.Current = Cursors.WaitCursor;

				//	Generate the report without a separate thread
				Execute();

				Cursor.Current = Cursors.Default;

			}// if(CreateStatusForm() == true)

			//	Close the form
			DialogResult = DialogResult.OK;
			this.Close();

		}// protected override void OnClickOK(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Private Methods

		/// <summary>This method is called to execute the report operation</summary>
		private void Execute()
		{
			try
			{
				SetStatusVisible(true, "Generating Objections Reports ...");

				//	Prepare the source records specified by the user
				if(Prepare() == true)
				{
					//	Generate a report for each XML script
					foreach(CXmlScript O in m_xmlScripts)
					{
						if(SaveORESetup(O) == false)
							break;
							
						if(m_bAborted == true)
							break;
							
						if(Generate(O) == false)
							break;

					}// foreach(CXmlScript O in m_xmlScripts)

				}// if(Prepare() == true)
				
				//	Close the status form if it's still visible
				if(m_bAborted == false)
				{
					//	Indicate that the operation is finished
					m_wndStatus.Finished = true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Execute", m_tmaxErrorBuilder.Message(ERROR_EXECUTE_EX), Ex);
			}

		}// private void Execute()

		/// <summary>This method is called to prepare to execute the Objections Report Engine</summary>
		/// <returns>True to continue with the report generation</returns>
		private bool Prepare()
		{
			try
			{
			    SetStatus("Preparing report generator ...");

			    //	Reset the class members
			    m_xmlScripts.Clear();
			    m_lGenerated = 0;
			    m_bStepProgress = true;

			    //	Get a temporary folder for the XML scripts
			    if(SetXmlFolder() == false)
			        return false;
				
			    //	Set the report format
			    if(SetFormat() == false)
					return false;

				//	Should there just be one deposition per file?
				m_bOneDeposition = CTmaxToolbox.StringToBool(m_tmaxORETemplate.GetValue(CTmaxORETemplate.ORE_MANAGER_SINGLE_DEPOSITION));
					
			    //	Prepare the source records specified by the caller
			    foreach(CTmaxItem O in m_tmaxItems)
			    {
			        if(O.GetMediaRecord() != null)
			        {
			            switch(O.GetMediaRecord().GetMediaType())
			            {
			                case TmaxMediaTypes.Script:
			                case TmaxMediaTypes.Scene:
			                case TmaxMediaTypes.Deposition:

			                    //	Add this record to the collection
			                    AddSource((CDxMediaRecord)(O.GetMediaRecord()));
			                    break;

			            }// switch(O.GetMediaRecord().GetMediaType())

			        }
			        else if(O.IBinderEntry != null)
			        {
			            AddSource((CDxBinderEntry)(O.IBinderEntry));
			        }

			        // Has the user aborted the operation?
			        if(m_bAborted == true)
			            break;

			    }// foreach(CTmaxItem O in m_tmaxItems)
			
			}
			catch(System.Exception Ex)
			{
			    m_tmaxEventSource.FireError(this, "Prepare", m_tmaxErrorBuilder.Message(ERROR_PREPARE_EX), Ex);
			}
			
			//	Has the user aborted the operation?
			if(m_bAborted == true)
			{
				SetStatus("Deleting temporary files ...");
				
				m_xmlScripts.Clear();

			}// if(m_bAborted == true)
			
			if(m_xmlScripts.Count == 0)
				Warn("There are no scripts or depositions in the current selection(s)");
				
			return (m_xmlScripts.Count > 0);

		}// private bool Prepare()

		/// <summary>This method is called to create an XML script for the specified sourcr media record</summary>
		/// <param name="dxSource">The source record used to create the script</param>
		/// <returns>The number of scripts added to the collection</returns>
		private int AddSource(CDxMediaRecord dxSource)
		{
			CXmlScript		xmlScript = null;
			CDxPrimary		dxScript = null;
			CDxPrimaries	dxDepositions = null;
			CDxSecondaries	dxScenes = null;
			CDxTertiary		dxDesignation = null;
			int				iInitialCount = 0;
			
			try
			{
				SetStatus("Processing " + dxSource.GetBarcode(false));
				
				//	How many scripts are currently in the collection?
				iInitialCount = m_xmlScripts.Count;
				
				//	Allocate the record collections
				dxDepositions = new CDxPrimaries();
				dxScenes = new CDxScenes();
				
				//	Is this a script record?
				if(dxSource.MediaType == TmaxMediaTypes.Script)
				{
					dxScript = (CDxPrimary)dxSource;
					
					//	Make sure the child collection has been filled
					if((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
						dxScript.Fill();
						
					//	Locate all designation scenes
					foreach(CDxSecondary O in dxScript.Secondaries)
					{
						if((O.GetSource() != null) && (O.GetSource().MediaType == TmaxMediaTypes.Designation))
						{
							dxScenes.AddList(O);
						}
						
					}// foreach(CDxSecondary O in dxScript.Secondaries)
						
				}
				else if(dxSource.MediaType == TmaxMediaTypes.Scene)
				{
					if(((CDxSecondary)dxSource).GetSource() != null)
					{
						if(((CDxSecondary)dxSource).GetSource().MediaType == TmaxMediaTypes.Designation)
						{
							dxScenes.AddList((CDxSecondary)dxSource);
							dxScript = (CDxPrimary)(dxSource.GetParent());
						}

					}// if(((CDxSecondary)dxSource).GetSource() != null)	
				
				}
				else if(dxSource.MediaType == TmaxMediaTypes.Deposition)
				{
					//	Add this deposition to the temporary collection
					dxDepositions.AddList(dxSource);
				}
				else
				{
					Debug.Assert(false, "Invalid source media type");
					return 0;
				}
				
				//	Do we have a script related to the source record?
				if(dxScript != null)
				{
					//	Don't bother if not at least one designation
					if(dxScenes.Count > 0)
					{
						//	Add the required depositions to the local collection
						foreach(CDxSecondary O in dxScenes)
						{
							if((dxDesignation = ((CDxTertiary)(O.GetSource()))) != null)
							{
								if((dxDesignation.Secondary != null) && (dxDesignation.Secondary.Primary != null))
								{
									if(dxDepositions.Contains(dxDesignation.Secondary.Primary) == false)
										dxDepositions.AddList(dxDesignation.Secondary.Primary);
								}

							}// if((dxDesignation = ((CDxTertiary)(O.GetSource()))) != null)
							
						}// foreach(CDxSecondary O in dxScenes)

						//	Are we limited to one deposition per report?
						if((m_bOneDeposition == true) && (dxDepositions.Count > 1))
						{
							//	Create one XML script for each deposition
							foreach(CDxPrimary O in dxDepositions)
							{
								if((xmlScript = CreateXmlSource(dxScript, dxScenes, O)) != null)
									m_xmlScripts.Add(xmlScript);
							}
							
						}
						else
						{
							if((xmlScript = CreateXmlSource(dxScript, dxScenes, dxDepositions, false)) != null)
								m_xmlScripts.Add(xmlScript);
						}

					}// if(dxScenes.Count > 0)
					else
					{
						//	No point in processing a script with no designations
					}
					
				}
				else if(dxDepositions.Count == 1)
				{
					if((xmlScript = CreateXmlSource(null, null, dxDepositions, false)) != null)
						m_xmlScripts.Add(xmlScript);
				}
				else
				{
					//	We shouldn't reach this point
					Debug.Assert(false, "Invalid source media");
				}			
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_ADD_SOURCE_EX, dxSource.GetBarcode(false)), Ex);
			}
			
			return (m_xmlScripts.Count - iInitialCount);

		}// private int AddSource(CDxMediaRecord dxSource)

		/// <summary>This method is called to add the valid media in the specified binder</summary>
		/// <param name="dxBinder">The source binder</param>
		private void AddSource(CDxBinderEntry dxBinder)
		{
			CDxMediaRecord	dxMedia = null;
			CDxSecondary	dxScene = null;
			
			//	Make sure the contents have been populated
			if((dxBinder.Contents == null) ||(dxBinder.Contents.Count == 0))
				dxBinder.Fill();
				
			foreach(CDxBinderEntry O in dxBinder.Contents)
			{
				//	Is this a media reference?
				if(O.IsMedia() == true)
				{
					if((dxMedia = O.GetSource(true)) != null)
					{
						switch(dxMedia.MediaType)
						{
							case TmaxMediaTypes.Script:
							case TmaxMediaTypes.Deposition:

								//	Add this record to the collection
								AddSource(dxMedia);
								break;

							case TmaxMediaTypes.Scene:

								//	Is this scene bound to a designation?
								dxScene = (CDxSecondary)(dxMedia);
								if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
									AddSource(dxMedia);
								break;

						}// switch(O.GetMediaRecord().GetMediaType())

					}// if((dxMedia = O.GetSource(true)) != null)
										
				}
				else
				{
					//	Should we drill sub-binders?
					if(Options.IncludeSubBinders == true)
						AddSource(O);
				}

			}// foreach(CDxBinderEntry O in dxBinder.Contents)

		}// private void AddSource(CDxBinderEntry dxBinder)
		
		/// <summary>This method is called to create an XML script for the specified sourcr media record</summary>
		/// <param name="dxScript">The owner script records</param>
		/// <param name="dxScenes">The collection of scenes to be included in the XML script</param>
		/// <param name="dxDepositions">The collection of depositions to be included in the XML script</param>
		/// <param name="bAddDepositionId">true to add the deposition MediaId to the XML filename</param>
		/// <returns>The XML script to represent the source record</returns>
		private CXmlScript CreateXmlSource(CDxPrimary dxScript, CDxSecondaries dxScenes, CDxPrimaries dxDepositions, bool bAddDepositionId)
		{
			CXmlScript	xmlScript = null;
			string		strFileSpec = "";
			
			try
			{
				xmlScript = new CXmlScript();
				xmlScript.XmlScriptFormat = TmaxXmlScriptFormats.Manager;
				
				if(dxScript != null)
				{
					xmlScript.MediaId = dxScript.GetBarcode(false);
					xmlScript.Name = dxScript.Name.Length > 0 ? dxScript.Name : dxScript.GetBarcode(false);
				}
				
				//	Do we have any scenes to be added?
				if((dxScenes != null) && (dxScenes.Count > 0))
				{
					foreach(CDxSecondary O in dxScenes)
					{
						if(AddXmlScene(xmlScript, O) == null)
							return null;
					}

				}// if((dxScenes != null) && (dxScenes.Count > 0))

				//	Do we have any depositions to be added?
				if((dxDepositions != null) && (dxDepositions.Count > 0))
				{
					foreach(CDxPrimary O in dxDepositions)
					{
						if(AddXmlDeposition(xmlScript, O) == null)
							return null;
					}

				}// if((dxDepositions != null) && (dxDepositions.Count > 0))

				//	Get the path used to save the file
				if(dxScript != null)
				{
					if((bAddDepositionId == true) && (dxDepositions.Count == 1))
						strFileSpec = GetXmlFileSpec(dxScript, dxDepositions[0]);
					else
						strFileSpec = GetXmlFileSpec(dxScript, null);
				}
				else
				{
					strFileSpec = GetXmlFileSpec(dxDepositions[0], null);
				}
								
				//	Save the file to the temporary folder
				if(xmlScript.Save(strFileSpec) == false)
				{
					m_tmaxEventSource.FireError(this, "CreateXmlSource", m_tmaxErrorBuilder.Message(ERROR_SAVE_XML_SCRIPT_FAILED, strFileSpec));
					xmlScript = null;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateXmlSource", m_tmaxErrorBuilder.Message(ERROR_CREATE_XML_SOURCE_EX), Ex);
				xmlScript = null;
			}
			
			return xmlScript;

		}// private CXmlScript CreateXmlSource(CDxPrimary dxScript, CDxSecondaries dxScenes, CDxPrimaries dxDepositions)
		
		/// <summary>This method is called to create an XML script for the specified source media record</summary>
		/// <param name="dxScript">The owner script record</param>
		/// <param name="dxScenes">The collection of scenes created from the specified depositon</param>
		/// <param name="dxDeposition">The deposition from which all scenes have been created</param>
		/// <returns>The XML script to represent the source record</returns>
		private CXmlScript CreateXmlSource(CDxPrimary dxScript, CDxSecondaries dxScenes, CDxPrimary dxDeposition)
		{
			CXmlScript		xmlScript = null;
			CDxSecondaries	dxSourceScenes = null;
			CDxPrimaries	dxSourceDepositions = null;
			CDxTertiary		dxDesignation = null;
			
			//	Just checking...
			Debug.Assert(dxScript != null);
			Debug.Assert(dxScenes != null);
			Debug.Assert(dxDeposition != null);
			
			try
			{
				//	Create the collections we need to perform the operation
				dxSourceScenes = new CDxSecondaries();
				dxSourceDepositions = new CDxPrimaries();
				
				//	Locate all scenes associated with the specified deposition
				foreach(CDxSecondary O in dxScenes)
				{
					if((dxDesignation = (CDxTertiary)(O.GetSource())) != null)
					{
						if(dxDesignation.Secondary != null)
						{
							if(ReferenceEquals(dxDesignation.Secondary.Primary, dxDeposition) == true)
								dxSourceScenes.AddList(O);
						}

					}// if((dxDesignation = (CDxTertiary)(O.GetSource())) != null)

				}// foreach(CDxSecondary O in dxScenes)
				
				//	Do we have any scenes for this deposition?
				if(dxSourceScenes.Count > 0)
				{
					dxSourceDepositions.AddList(dxDeposition);
										
					xmlScript = CreateXmlSource(dxScript, dxSourceScenes, dxSourceDepositions, true);

				}// if(dxSourceScenes.Count > 0)
								
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateXmlSource", m_tmaxErrorBuilder.Message(ERROR_CREATE_XML_SOURCE_EX), Ex);
				xmlScript = null;
			}
			finally
			{
				//	Clean up
				if(dxSourceDepositions != null)
				{
					dxSourceDepositions.Clear();
					dxSourceDepositions = null;
				}
				if(dxSourceScenes != null)
				{
					dxSourceScenes.Clear();
					dxSourceScenes = null;
				}
				
			}

			return xmlScript;

		}// private CXmlScript CreateXmlSource(CDxPrimary dxScript, CDxSecondaries dxScenes, CDxPrimaries dxDepositions)
		
		/// <summary>This method is called to add a scene descriptor to the specified XML script</summary>
		/// <param name="xmlScript">The owner XML script</param>
		/// <param name="dxScene">The scene to be added</param>
		/// <returns>The XML scene that was added to the collection</returns>
		private CXmlScene AddXmlScene(CXmlScript xmlScript, CDxSecondary dxScene)
		{
			CXmlScene		xmlScene = null;
			CXmlDesignation	xmlDesignation = null;
			CDxTertiary		dxDesignation = null;
			
			try
			{
				//	Must have a valid source record
				if((dxDesignation = ((CDxTertiary)(dxScene.GetSource()))) == null) 
					return null;

				//	Create and initialize a new scene object
				xmlScene = new CXmlScene();
				xmlScene.SourceId = dxDesignation.GetBarcode(true);
				xmlScene.SourceType = dxDesignation.MediaType;
				xmlScene.Hidden = dxScene.Hidden;
				xmlScene.AutoTransition = dxScene.AutoTransition;
				xmlScene.TransitionTime = dxScene.TransitionTime;
				xmlScene.BarcodeId = dxScene.BarcodeId;

				if((xmlDesignation = m_tmaxDatabase.GetXmlDesignation(dxDesignation, true, false, false)) != null)
				{
					//	Make sure we can link back to the primary source
					if((dxDesignation.Secondary != null) && (dxDesignation.Secondary.Primary != null))
						xmlDesignation.PrimaryId = dxDesignation.Secondary.Primary.MediaId;

					xmlDesignation.Segment = dxDesignation.GetExtent().XmlSegmentId.ToString();

					xmlScript.Add(xmlScene, xmlDesignation);

				}// if((xmlDesignation = m_tmaxDatabase.GetXmlDesignation(dxDesignation, true, false, false)) != null)
				else
				{
					//	Can't add the scene without the source designation
					xmlScene = null;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddXmlScene", m_tmaxErrorBuilder.Message(ERROR_ADD_XML_SCENE_EX, dxScene.GetBarcode(false)), Ex);
				xmlScene = null;
			}

			return xmlScene;

		}// private CXmlScene AddXmlScene(CXmlScript xmlScript, CDxSecondary dxScene)

		/// <summary>This method is called to add a scene descriptor to the specified XML script</summary>
		/// <param name="xmlScript">The owner XML script</param>
		/// <param name="dxDeposition">The scene to be added</param>
		/// <returns>The XML scene that was added to the collection</returns>
		private CXmlDeposition AddXmlDeposition(CXmlScript xmlScript, CDxPrimary dxDeposition)
		{
			CXmlDeposition	xmlDeposition = null;
			CTmaxObjections	tmaxObjections = null;
			bool			bFlushObjections = false;

			try
			{
				//	Get the XML deposition from the database
				if((xmlDeposition = m_tmaxDatabase.GetXmlDeposition(dxDeposition, true, true, false)) != null)
				{
					//	Add to the script's collection
					xmlDeposition.MediaId = dxDeposition.MediaId;
					xmlScript.XmlDepositions.Add(xmlDeposition);

					if(m_tmaxDatabase.Objections != null)
					{
						//	Should we filter the objections first?
						if(Options.FilterIndex == OBJECTIONS_FILTER_DEFENSE || (Options.FilterIndex == OBJECTIONS_FILTER_PLAINTIFF))
						{
							tmaxObjections = new CTmaxObjections();
							bFlushObjections = true;
							
							foreach(CTmaxObjection O in m_tmaxDatabase.Objections)
							{
								if(O.Plaintiff == true)
								{
									if(Options.FilterIndex == OBJECTIONS_FILTER_PLAINTIFF)
										tmaxObjections.Add(O);
								}
								else
								{
									if(Options.FilterIndex == OBJECTIONS_FILTER_DEFENSE)
										tmaxObjections.Add(O);
								}

							}// foreach(CTmaxObjection O in m_tmaxDatabase.Objections)
							
						}
						else
						{
							//	Use the master collection
							tmaxObjections = m_tmaxDatabase.Objections;
						}
						
						xmlDeposition.AddObjections(m_tmaxDatabase.Detail.TmaxCase, tmaxObjections);
					
						if((tmaxObjections != null) && (bFlushObjections == true))
							tmaxObjections.Clear();
						tmaxObjections = null;

					}// if(m_tmaxDatabase.Objections != null)	

				}// if((xmlDeposition = m_tmaxDatabase.GetXmlDeposition(dxDeposition, true, true, false)) != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddXmlDeposition", m_tmaxErrorBuilder.Message(ERROR_ADD_XML_SCENE_EX, dxDeposition.GetBarcode(false)), Ex);
				xmlDeposition = null;
			}

			return xmlDeposition;

		}// private CXmlDeposition AddXmlDeposition(CXmlScript xmlScript, CDxPrimary dxDeposition)

		/// <summary>This called to get the fully qualified path to the Objections Report Engine executable</summary>
		/// <returns>true if successful</returns>
		private bool GetOREFileSpec()
		{
			CTmaxComponent	tmaxOREComponent = null;
			string			strWarning = "";
			
			if((m_tmaxProductManager != null) && (m_tmaxProductManager.Components != null))
				tmaxOREComponent = m_tmaxProductManager.Components.Find(TmaxComponents.FTIORE);
			
			if(tmaxOREComponent != null)
			{
				m_strOREFileSpec = tmaxOREComponent.GetFileSpec();

				//	Make sure the file exists
				if(System.IO.File.Exists(m_strOREFileSpec) == false)
					strWarning = String.Format("The TrialMax Objections Report Engine could not be found in it's registered location {0}\n\nSelect \"Check For Updates ...\" from the application's Help menu to download and update the TrialMax ORE plug-in. ", m_strOREFileSpec);
			}
			else
			{
				strWarning = "The TrialMax Objections Report Engine has not been installed. Select \"Check For Updates ...\" from the application's Help menu to download and install the TrialMax ORE plug-in. ";
			}
			
			//	Should we display a warning message?
			if(strWarning.Length > 0)
				MessageBox.Show(strWarning, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

			return (strWarning.Length == 0);

		}// private bool GetOREFileSpec()

		/// <summary>Called to get the fully qualified path for the XML script</summary>
		/// <param name="dxP1">The primary record used to create the name</param>
		/// <param name="dxP2">The primary record used to create the name</param>
		/// <returns>The fully qualified path</returns>
		private string GetXmlFileSpec(CDxPrimary dxP1, CDxPrimary dxP2)
		{	
			string	strXmlFileSpec = "";
			string	strFilename = "";

			//	Start with the prefix if defined
			strFilename = m_tmaxORETemplate.GetValue(CTmaxORETemplate.ORE_MANAGER_PREFIX);
			
			//	Add the script's MediaID
			strFilename += dxP1.MediaId;

			//	Should we also include the name?
			if((Options.AddMediaName == true) && (dxP1.Name.Length > 0))
				strFilename += (" " + dxP1.Name);
				
			//	Should we append another media id
			if(dxP2 != null)
				strFilename += ("_" + dxP2.MediaId);
				
			//	Make sure the filename doesn't contain any invalid characters
			strFilename = CTmaxToolbox.CleanFilename(strFilename, false);

			//	Build the fully qualified path
			strXmlFileSpec = String.Format("{0}{1}.xmls", m_strXmlFolder, strFilename);

			return strXmlFileSpec;

		}// private string GetXmlFileSpec(CDxPrimary dxP1, CDxPrimary dxP2)

		/// <summary>This method is called to set the path to the folder where the XML scripts will be stored</summary>
		/// <returns>True if successful</returns>
		private bool SetXmlFolder()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Get a temporary folder for the XML files
				m_strXmlFolder = System.IO.Path.GetTempPath();
				if(m_strXmlFolder.Length == 0)
					m_strXmlFolder = m_strSourceFolder;
				if(m_strXmlFolder.EndsWith("\\") == false)
					m_strXmlFolder += "\\";
					
				m_strXmlFolder += "_tmax_ore_files_\\";

				if(System.IO.Directory.Exists(m_strXmlFolder) == false)
				{
					try
					{
						System.IO.Directory.CreateDirectory(m_strXmlFolder);
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireError(this, "SetXmlFolder", m_tmaxErrorBuilder.Message(ERROR_SET_XML_FOLDER_FAILED, m_strXmlFolder), Ex);
						return false;
					}

				}// if(System.IO.Directory.Exists(m_strXmlFolder) == false)

				//	Build the path to the progress file
				m_strOREProgressFileSpec = m_strXmlFolder + XML_ORE_PROGRESS_FILENAME;
				try { System.IO.File.Delete(m_strOREProgressFileSpec); }
				catch {}
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetXmlFolder", m_tmaxErrorBuilder.Message(ERROR_SET_XML_FOLDER_EX), Ex);
			}
			
			return bSuccessful;

		}// private bool SetXmlFolder()

		/// <summary>This method is called to set the format for the ORE generator</summary>
		/// <returns>True if successful</returns>
		private bool SetFormat()
		{
			string strFormat = "";
			
			try
			{
			    //	Get the report format stored in the ORE template
			    strFormat = m_tmaxORETemplate.GetValue(CTmaxORETemplate.ORE_CONSTANT_FORMAT);
			    
			    //	Do we need to set the value using the TrialMax template?
			    if(strFormat.Length == 0)
			    {
					m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_CONSTANT_FORMAT, m_roTemplate.Name);
			
					//	Now read it back to make sure everything is OK
					strFormat = m_tmaxORETemplate.GetValue(CTmaxORETemplate.ORE_CONSTANT_FORMAT);
				}

				if(strFormat.Length == 0)
					Warn("No format definition defined in the ORE template: " + m_tmaxORETemplate.FileSpec);
			}
			catch(System.Exception Ex)
			{
			    m_tmaxEventSource.FireError(this, "SetFormat", m_tmaxErrorBuilder.Message(ERROR_SET_FORMAT_EX), Ex);
				strFormat = "";
			}
				
			return (strFormat.Length > 0);

		}// private bool SetFormat()

		/// <summary>Called to generate the report for the specified XML script</summary>
		/// <param name="xmlScript">The source XML script</param>
		/// <returns>true if OK to continue</returns>
		private bool Generate(CXmlScript xmlScript)
		{
			System.Diagnostics.Process oreProcess = null;
			bool	bContinue = false;
			string	strGenerating = "";
			string	strStatus = "";

			Debug.Assert(m_strOREFileSpec.Length > 0);
			if(m_strOREFileSpec.Length == 0) return false;

			try
			{
				strGenerating = ("Generating " + System.IO.Path.GetFileName(m_tmaxORETemplate.GetValue(CTmaxORETemplate.ORE_RUNTIME_SAVE_AS)) + " ");
				strStatus = strGenerating;

				SetStatus(strStatus);
			
				//	Create the process for launching the converter
				oreProcess = new Process();

				//	Initialize the startup information
				//
				//	NOTE:	All command line parameters are converted to lower case
				//			because the exporter has trouble with some upper case
				//			values. 
				oreProcess.StartInfo.FileName = m_strOREFileSpec.ToLower();
				oreProcess.StartInfo.Arguments = String.Format(" -in \"{0}\"", m_strXmlORESetup.ToLower());
				oreProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

				//	Start the conversion process
				if(oreProcess.Start() == false)
				{
					MessageBox.Show("Unable to start: " + m_strOREFileSpec, oreProcess.StartInfo.Arguments);
					return false;
				}

				//	Block this thread until the process completes
				//
				//	NOTE:	We could just call oreProcess.WaitForExit() but this
				//			approach allows us to do some processing while the 
				//			converter is active
				while(oreProcess.HasExited == false)
				{
					Thread.Sleep(250);
					
					//	Update the status form
					CheckOREProgress();
					
					//	Has the user aborted?
					if(m_bAborted == true)
					{
						//	Changed our mind on this one because it might leave
						//	open instances of Excel or Word hanging around
						
						//	Try to abort the generator process
						//try { oreProcess.Kill(); }
						//catch {};
						
						break;
					}

				}// while(pdfConverter.HasExited == false)

				//	Did the conversion fail?
				if(m_bAborted == false)
				{
					if(oreProcess.ExitCode != 0)
					{
						MessageBox.Show("ORE failed with exit code = " + oreProcess.ExitCode.ToString());
					}
					else
					{
						m_lGenerated++;
						bContinue = true;
					}

				}// if(m_bAborted == false)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Generate", m_tmaxErrorBuilder.Message(ERROR_GENERATE_EX, xmlScript.Filename), Ex);
			}

			//	Make sure the converter process is closed
			if(oreProcess != null)
			{
				try { oreProcess.Close(); }
				catch {}
				oreProcess = null;
			}

			return bContinue;

		}// private bool Generate(CXmlScript xmlScript)

		/// <summary>This method is set the ORE configuration options</summary>
		/// <param name="xmlScript">the script being used to generate the report</param>
		/// <returns>True if successful</returns>
		private bool SaveORESetup(CXmlScript xmlScript)
		{
			bool					bSuccessful = false;
			string					strSelectedDeponent = "";
			CXmlORESetup			xmlSetup = null;
			CTmaxOREOption			tmaxOREOption = null;
			System.Drawing.Color	sysColor = System.Drawing.Color.Red;

			try
			{
				//	This shouldn't happen but just in case...
				if((m_roTemplate == null) || (m_tmaxORETemplate == null))
				{
					Warn("No template available to perform the report configuration");
					return false;
				}

				//	Build the SelectedDeponent value
				if((xmlScript.XmlDepositions != null) && (xmlScript.XmlDepositions.Count > 0))
				{
					foreach(CXmlDeposition O in xmlScript.XmlDepositions)
					{
						if(strSelectedDeponent.Length > 0)
							strSelectedDeponent += ", ";
						strSelectedDeponent += O.MediaId;
					}

				}// if((xmlScript.XmlDepositions != null) && (xmlScript.XmlDepositions.Count > 0))

				//	Set the runtime option values
				m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_RUNTIME_XMLS_PATH, xmlScript.FileSpec.ToLower());
				m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_RUNTIME_SAVE_AS, GetOutputFileSpec(xmlScript));
				m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_RUNTIME_SELECTED_DEPONENT, strSelectedDeponent);
				m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_RUNTIME_PROGRESS_FILESPEC, m_strOREProgressFileSpec);
				m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_RUNTIME_DEFENDANT_COLOR, CTmaxORETemplate.ToString(m_tmaxDatabase.GetObjectionsColor(TmaxHighlighterGroups.Defendant)));
				m_tmaxORETemplate.SetValue(CTmaxORETemplate.ORE_RUNTIME_PLAINTIFF_COLOR, CTmaxORETemplate.ToString(m_tmaxDatabase.GetObjectionsColor(TmaxHighlighterGroups.Plaintiff)));
			
				//	Fill the colors collection
				m_tmaxORETemplate.Colors.Clear();
				if(m_tmaxDatabase.Highlighters != null)
				{
					for(int i = 0; i < m_tmaxDatabase.Highlighters.Count; i++)
					{
						tmaxOREOption = new CTmaxOREOption();
						tmaxOREOption.ORELabel = String.Format("{0}{1}", CTmaxORETemplate.ORE_COLOR_PREFIX, (i + 1));
						tmaxOREOption.Value = CTmaxORETemplate.ToString(m_tmaxDatabase.Highlighters[i].SysColor);
																		   
						if(m_tmaxDatabase.Highlighters[i].Group == TmaxHighlighterGroups.Plaintiff)
							tmaxOREOption.Party = "P";
						else
							tmaxOREOption.Party = "D";
							
						m_tmaxORETemplate.Colors.Add(tmaxOREOption);
					}

				}// if(m_tmaxDatabase.Highlighters != null)
				
				//	Create the setup file and fill it's collection
				xmlSetup = new CXmlORESetup();
				xmlSetup.Fill(m_tmaxORETemplate);

				//	Build the path to the ORE configuration file
				m_strXmlORESetup = m_strXmlFolder + XML_ORE_SETUP_FILENAME;

				//	Save the file
				if((bSuccessful = xmlSetup.Save(m_strXmlORESetup)) == false)
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_CREATE_XML_SETUP_FAILED, m_strXmlORESetup));
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SaveORESetup", m_tmaxErrorBuilder.Message(ERROR_SAVE_ORE_SETUP_EX), Ex);
			}
			finally
			{
				if(xmlSetup != null)
				{
					xmlSetup.Close();
					xmlSetup = null;
				}
				
			}
			
			return bSuccessful;

		}// private bool SaveORESetup()

		/// <summary>Called to open the XML configuration file for the specified template</summary>
		/// <param name="roTemplate">The template that specifies the XML filename</param>
		/// <param name="bRefresh">True to force a refresh of the XML file</param>
		/// <returns>True if successful</returns>
		private bool OpenORETemplate(CROTemplate roTemplate, bool bRefresh)
		{
			bool	bSuccessful = false;
			string	strMsg = "";

			try
			{
				Debug.Assert(roTemplate != null);

				//	Has the file already been opened?
				m_tmaxORETemplate = Options.GetORETemplate(roTemplate);
				if((m_tmaxORETemplate != null) && (bRefresh == false))
					return true; // Nothing to do

				while(bSuccessful == false)
				{
					//	Make sure we have a filename for this template
					if(roTemplate.Filename.Length == 0)
					{
						strMsg = "No filename for this template's configuration file has been provided";
						break;
					}

					//	Get the path to the ORE template file
					m_strTemplate = GetFileSpec(roTemplate.Filename);

					//	Make sure the file exists
					if(System.IO.File.Exists(m_strTemplate) == false)
					{
						strMsg = String.Format("Unable to locate {0} to configure the report options", m_strTemplate);
						break;
					}

					//	Open the configuration file
					m_tmaxORETemplate = new CTmaxORETemplate(roTemplate.Name);
					if(m_tmaxORETemplate.Load(m_strTemplate) == true)
					{
						//	Add to the Options collection
						this.Options.AddORETemplate(m_tmaxORETemplate);
						
						bSuccessful = true;
					}
					else
					{
						m_tmaxORETemplate = null;
						strMsg = String.Format("Unable to open {0} to configure the report options", m_strTemplate);
						break;
					}

				}// while(bSuccessful == false)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenORETemplate", m_tmaxErrorBuilder.Message(ERROR_OPEN_ORE_TEMPLATE_EX, roTemplate.Filename), Ex);
			}

			//	Should we display a warning message?
			if(strMsg.Length > 0)
				Warn(strMsg);

			return bSuccessful;

		}// private bool OpenORETemplate(CROTemplate roTemplate, bool bRefresh)

		/// <summary>This method is called to create the status form for the operation</summary>
		/// <returns>true if successful</returns>
		private bool CreateStatusForm()
		{
			try
			{
				//	Clear the Aborted flag
				m_bAborted = false;

				//	Make sure the previous instance is disposed
				if(m_wndStatus != null)
				{
					if(m_wndStatus.IsDisposed == false)
						m_wndStatus.Dispose();
					m_wndStatus = null;
				}

				//	Create a new instance
				m_wndStatus = new CRFObjectionsStatus();
				m_wndStatus.Title = "Objections Report Status";

				//	Set the initial status message
				SetStatus("Initializing operation ...");
			}
			catch
			{
				m_wndStatus = null;
			}

			return (m_wndStatus != null);

		}// private bool CreateStatusForm()

		/// <summary>This method is called to update the status text on the status form</summary>
		/// <param name="strStatus">The new status message</param>
		private void SetStatus(string strStatus)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					m_wndStatus.Status = strStatus;
					m_wndStatus.Refresh();
				}

			}
			catch
			{
			}

		}// private void SetStatus(string strStatus)

		/// <summary>This method is called to show/hide the status form</summary>
		/// <param name="bVisible">true if visible</param>
		/// <param name="strStatus">optional status message </param>
		private void SetStatusVisible(bool bVisible, string strStatus)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					if(strStatus != null)
						m_wndStatus.Status = strStatus;

					if((bVisible == true) && (m_wndStatus.Visible == false))
					{
						m_wndStatus.Show();
						m_wndStatus.Refresh();
					}
					else
					{
						m_wndStatus.Hide();
					}

				}

			}
			catch
			{
			}

		}// private void SetStatusVisible(bool bVisible, string strStatus)

		/// <summary>This method is called to show/hide the status form</summary>
		/// <param name="bVisible">true if visible</param>
		private void SetStatusVisible(bool bVisible)
		{
			SetStatusVisible(bVisible, null);
		}

		/// <summary>This method handles the SelectedIndexChanged event fired by the Templates combobox</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnTemplateSelChanged(object sender, System.EventArgs e)
		{
			int iIndex = 0;
			
			//	Save the flag selections when we switch between templates
			GetSelections(m_ctrlFlags);
			
			//	Get the new template selection
			if((iIndex = m_ctrlTemplates.SelectedIndex) >= 0)
				m_roTemplate = (CROTemplate)(Options.Templates[iIndex]);
			else
				m_roTemplate = null;

			if(m_roTemplate != null)
			{
				OpenORETemplate(m_roTemplate, false);
				
				//	Populate the flags list
				FillFlags();
			}
			else
			{
				m_tmaxORETemplate = null;
			}
				
			SetControlStates();

		}// private void OnTemplateSelChanged(object sender, System.EventArgs e)

		/// <summary>Called to enable/disable the child controls</summary>
		private void SetControlStates()
		{
			if((m_roTemplate != null) && (m_tmaxORETemplate != null))
			{
				m_ctrlTitle.Enabled = m_tmaxORETemplate.GetEnabled(CTmaxORETemplate.ORE_COMMON_TITLE);
				m_ctrlTitleLabel.Enabled = m_ctrlTitle.Enabled;

				m_ctrlShortCaseName.Enabled = m_tmaxORETemplate.GetEnabled(CTmaxORETemplate.ORE_COMMON_SHORT_CASE_NAME);
				m_ctrlShortCaseNameLabel.Enabled = m_ctrlShortCaseName.Enabled;

				m_ctrlLongCaseName.Enabled = m_tmaxORETemplate.GetEnabled(CTmaxORETemplate.ORE_COMMON_LONG_CASE_NAME);
				m_ctrlLongCaseNameLabel.Enabled = m_ctrlLongCaseName.Enabled;
				
				m_ctrlFlags.Enabled = (m_ctrlFlags.Items.Count > 0);
				m_ctrlFlagsLabel.Enabled = m_ctrlFlags.Enabled;
				
				m_ctrlOK.Enabled = true;
			}
			else
			{
				m_ctrlTitle.Enabled = false;
				m_ctrlTitleLabel.Enabled = false;
				m_ctrlShortCaseName.Enabled = false;
				m_ctrlShortCaseNameLabel.Enabled = false;
				m_ctrlLongCaseName.Enabled = false;
				m_ctrlLongCaseNameLabel.Enabled = false;
				m_ctrlFlags.Enabled = false;
				m_ctrlFlagsLabel.Enabled = false;
				m_ctrlOK.Enabled = false;
			}
			
			if(m_ctrlFlags.Enabled == true)
			{
				m_ctrlFlags.BackColor = System.Drawing.SystemColors.Window;
			}
			else
			{
				m_ctrlFlags.BackColor = System.Drawing.SystemColors.Control;
			}

		}// private void SetControlStates()

		/// <summary>Called to fill the flags list</summary>
		private void FillFlags()
		{
			m_ctrlFlags.Items.Clear();
			
			if((m_tmaxORETemplate != null) && (m_tmaxORETemplate.Flags != null))
			{
				foreach(CTmaxOREOption O in m_tmaxORETemplate.Flags)
				{
					if(O.Enabled == true)
						m_ctrlFlags.Items.Add(O);

				}// foreach(CTmaxOREOption O in m_tmaxORETemplate.Flags)

			}// if((m_tmaxORETemplate != null) && (m_tmaxORETemplate.Flags != null))

			//	Set the check states
			SetSelections(m_ctrlFlags);

		}// private void FillFlags()

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRFObjections));
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlContentGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlIncludeSubBinders = new System.Windows.Forms.CheckBox();
			this.m_ctrlFilesGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlAddMediaName = new System.Windows.Forms.CheckBox();
			this.m_ctrlOutputFolderLabel = new System.Windows.Forms.Label();
			this.m_ctrlOutputFolder = new System.Windows.Forms.TextBox();
			this.m_ctrlBrowseOutputFolder = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlTemplatesLabel = new System.Windows.Forms.Label();
			this.m_ctrlTemplates = new System.Windows.Forms.ComboBox();
			this.m_ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlLongCaseNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlLongCaseName = new System.Windows.Forms.TextBox();
			this.m_ctrlShortCaseNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlShortCaseName = new System.Windows.Forms.TextBox();
			this.m_ctrlTitleLabel = new System.Windows.Forms.Label();
			this.m_ctrlTitle = new System.Windows.Forms.TextBox();
			this.m_ctrlFlags = new System.Windows.Forms.CheckedListBox();
			this.m_ctrlFlagsLabel = new System.Windows.Forms.Label();
			this.m_ctrlFilterGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlFilter = new System.Windows.Forms.ComboBox();
			this.m_ctrlContentGroup.SuspendLayout();
			this.m_ctrlFilesGroup.SuspendLayout();
			this.m_ctrlOptionsGroup.SuspendLayout();
			this.m_ctrlFilterGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Location = new System.Drawing.Point(220, 436);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOK.TabIndex = 4;
			this.m_ctrlOK.Text = "&OK";
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(300, 436);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 5;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlContentGroup
			// 
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeSubBinders);
			this.m_ctrlContentGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlContentGroup.Name = "m_ctrlContentGroup";
			this.m_ctrlContentGroup.Size = new System.Drawing.Size(368, 49);
			this.m_ctrlContentGroup.TabIndex = 0;
			this.m_ctrlContentGroup.TabStop = false;
			this.m_ctrlContentGroup.Text = "Source";
			// 
			// m_ctrlIncludeSubBinders
			// 
			this.m_ctrlIncludeSubBinders.Checked = true;
			this.m_ctrlIncludeSubBinders.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlIncludeSubBinders.Location = new System.Drawing.Point(11, 21);
			this.m_ctrlIncludeSubBinders.Name = "m_ctrlIncludeSubBinders";
			this.m_ctrlIncludeSubBinders.Size = new System.Drawing.Size(172, 16);
			this.m_ctrlIncludeSubBinders.TabIndex = 0;
			this.m_ctrlIncludeSubBinders.Text = "Include Sub-binders";
			// 
			// m_ctrlFilesGroup
			// 
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlAddMediaName);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlOutputFolderLabel);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlOutputFolder);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlBrowseOutputFolder);
			this.m_ctrlFilesGroup.Location = new System.Drawing.Point(8, 344);
			this.m_ctrlFilesGroup.Name = "m_ctrlFilesGroup";
			this.m_ctrlFilesGroup.Size = new System.Drawing.Size(368, 79);
			this.m_ctrlFilesGroup.TabIndex = 3;
			this.m_ctrlFilesGroup.TabStop = false;
			this.m_ctrlFilesGroup.Text = "Files";
			// 
			// m_ctrlAddMediaName
			// 
			this.m_ctrlAddMediaName.AutoSize = true;
			this.m_ctrlAddMediaName.Location = new System.Drawing.Point(80, 53);
			this.m_ctrlAddMediaName.Name = "m_ctrlAddMediaName";
			this.m_ctrlAddMediaName.Size = new System.Drawing.Size(165, 17);
			this.m_ctrlAddMediaName.TabIndex = 1;
			this.m_ctrlAddMediaName.Text = "Add Media Name to Filename";
			this.m_ctrlAddMediaName.UseVisualStyleBackColor = true;
			// 
			// m_ctrlOutputFolderLabel
			// 
			this.m_ctrlOutputFolderLabel.Location = new System.Drawing.Point(8, 23);
			this.m_ctrlOutputFolderLabel.Name = "m_ctrlOutputFolderLabel";
			this.m_ctrlOutputFolderLabel.Size = new System.Drawing.Size(51, 16);
			this.m_ctrlOutputFolderLabel.TabIndex = 6;
			this.m_ctrlOutputFolderLabel.Text = "Folder";
			this.m_ctrlOutputFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlOutputFolder
			// 
			this.m_ctrlOutputFolder.Location = new System.Drawing.Point(81, 23);
			this.m_ctrlOutputFolder.Name = "m_ctrlOutputFolder";
			this.m_ctrlOutputFolder.Size = new System.Drawing.Size(249, 20);
			this.m_ctrlOutputFolder.TabIndex = 0;
			// 
			// m_ctrlBrowseOutputFolder
			// 
			this.m_ctrlBrowseOutputFolder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowseOutputFolder.ImageIndex = 0;
			this.m_ctrlBrowseOutputFolder.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowseOutputFolder.Location = new System.Drawing.Point(336, 23);
			this.m_ctrlBrowseOutputFolder.Name = "m_ctrlBrowseOutputFolder";
			this.m_ctrlBrowseOutputFolder.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowseOutputFolder.TabIndex = 5;
			this.m_ctrlBrowseOutputFolder.Click += new System.EventHandler(this.OnBrowseOutputFolder);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlImages.Images.SetKeyName(0, "");
			// 
			// m_ctrlTemplatesLabel
			// 
			this.m_ctrlTemplatesLabel.Location = new System.Drawing.Point(8, 21);
			this.m_ctrlTemplatesLabel.Name = "m_ctrlTemplatesLabel";
			this.m_ctrlTemplatesLabel.Size = new System.Drawing.Size(51, 16);
			this.m_ctrlTemplatesLabel.TabIndex = 7;
			this.m_ctrlTemplatesLabel.Text = "Format";
			this.m_ctrlTemplatesLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// m_ctrlTemplates
			// 
			this.m_ctrlTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlTemplates.Location = new System.Drawing.Point(81, 20);
			this.m_ctrlTemplates.Name = "m_ctrlTemplates";
			this.m_ctrlTemplates.Size = new System.Drawing.Size(279, 21);
			this.m_ctrlTemplates.TabIndex = 0;
			this.m_ctrlTemplates.SelectedIndexChanged += new System.EventHandler(this.OnTemplateSelChanged);
			// 
			// m_ctrlOptionsGroup
			// 
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlLongCaseNameLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlTemplatesLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlLongCaseName);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlTemplates);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlShortCaseNameLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlShortCaseName);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlTitleLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlTitle);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlFlags);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlFlagsLabel);
			this.m_ctrlOptionsGroup.Location = new System.Drawing.Point(8, 125);
			this.m_ctrlOptionsGroup.Name = "m_ctrlOptionsGroup";
			this.m_ctrlOptionsGroup.Size = new System.Drawing.Size(368, 212);
			this.m_ctrlOptionsGroup.TabIndex = 2;
			this.m_ctrlOptionsGroup.TabStop = false;
			this.m_ctrlOptionsGroup.Text = "Report";
			// 
			// m_ctrlLongCaseNameLabel
			// 
			this.m_ctrlLongCaseNameLabel.Location = new System.Drawing.Point(8, 105);
			this.m_ctrlLongCaseNameLabel.Name = "m_ctrlLongCaseNameLabel";
			this.m_ctrlLongCaseNameLabel.Size = new System.Drawing.Size(67, 20);
			this.m_ctrlLongCaseNameLabel.TabIndex = 15;
			this.m_ctrlLongCaseNameLabel.Text = "Long Case";
			this.m_ctrlLongCaseNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlLongCaseName
			// 
			this.m_ctrlLongCaseName.Location = new System.Drawing.Point(81, 105);
			this.m_ctrlLongCaseName.Name = "m_ctrlLongCaseName";
			this.m_ctrlLongCaseName.Size = new System.Drawing.Size(279, 20);
			this.m_ctrlLongCaseName.TabIndex = 3;
			// 
			// m_ctrlShortCaseNameLabel
			// 
			this.m_ctrlShortCaseNameLabel.Location = new System.Drawing.Point(8, 77);
			this.m_ctrlShortCaseNameLabel.Name = "m_ctrlShortCaseNameLabel";
			this.m_ctrlShortCaseNameLabel.Size = new System.Drawing.Size(67, 20);
			this.m_ctrlShortCaseNameLabel.TabIndex = 13;
			this.m_ctrlShortCaseNameLabel.Text = "Short Case";
			this.m_ctrlShortCaseNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlShortCaseName
			// 
			this.m_ctrlShortCaseName.Location = new System.Drawing.Point(81, 77);
			this.m_ctrlShortCaseName.Name = "m_ctrlShortCaseName";
			this.m_ctrlShortCaseName.Size = new System.Drawing.Size(279, 20);
			this.m_ctrlShortCaseName.TabIndex = 2;
			// 
			// m_ctrlTitleLabel
			// 
			this.m_ctrlTitleLabel.Location = new System.Drawing.Point(8, 49);
			this.m_ctrlTitleLabel.Name = "m_ctrlTitleLabel";
			this.m_ctrlTitleLabel.Size = new System.Drawing.Size(51, 20);
			this.m_ctrlTitleLabel.TabIndex = 11;
			this.m_ctrlTitleLabel.Text = "Title";
			this.m_ctrlTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTitle
			// 
			this.m_ctrlTitle.Location = new System.Drawing.Point(81, 49);
			this.m_ctrlTitle.Name = "m_ctrlTitle";
			this.m_ctrlTitle.Size = new System.Drawing.Size(279, 20);
			this.m_ctrlTitle.TabIndex = 1;
			// 
			// m_ctrlFlags
			// 
			this.m_ctrlFlags.CheckOnClick = true;
			this.m_ctrlFlags.FormattingEnabled = true;
			this.m_ctrlFlags.Location = new System.Drawing.Point(81, 136);
			this.m_ctrlFlags.Name = "m_ctrlFlags";
			this.m_ctrlFlags.Size = new System.Drawing.Size(279, 64);
			this.m_ctrlFlags.TabIndex = 4;
			// 
			// m_ctrlFlagsLabel
			// 
			this.m_ctrlFlagsLabel.Location = new System.Drawing.Point(8, 136);
			this.m_ctrlFlagsLabel.Name = "m_ctrlFlagsLabel";
			this.m_ctrlFlagsLabel.Size = new System.Drawing.Size(51, 16);
			this.m_ctrlFlagsLabel.TabIndex = 8;
			this.m_ctrlFlagsLabel.Text = "Options";
			this.m_ctrlFlagsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlFilterGroup
			// 
			this.m_ctrlFilterGroup.Controls.Add(this.m_ctrlFilter);
			this.m_ctrlFilterGroup.Location = new System.Drawing.Point(8, 63);
			this.m_ctrlFilterGroup.Name = "m_ctrlFilterGroup";
			this.m_ctrlFilterGroup.Size = new System.Drawing.Size(368, 56);
			this.m_ctrlFilterGroup.TabIndex = 1;
			this.m_ctrlFilterGroup.TabStop = false;
			this.m_ctrlFilterGroup.Text = "Filter";
			// 
			// m_ctrlFilter
			// 
			this.m_ctrlFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlFilter.FormattingEnabled = true;
			this.m_ctrlFilter.Items.AddRange(new object[] {
            "All Objections",
            "Defense Objections",
            "Plaintiff Objections"});
			this.m_ctrlFilter.Location = new System.Drawing.Point(11, 20);
			this.m_ctrlFilter.Name = "m_ctrlFilter";
			this.m_ctrlFilter.Size = new System.Drawing.Size(349, 21);
			this.m_ctrlFilter.TabIndex = 0;
			// 
			// CRFObjections
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(382, 468);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlFilterGroup);
			this.Controls.Add(this.m_ctrlOptionsGroup);
			this.Controls.Add(this.m_ctrlFilesGroup);
			this.Controls.Add(this.m_ctrlContentGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CRFObjections";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Objections Report";
			this.m_ctrlContentGroup.ResumeLayout(false);
			this.m_ctrlFilesGroup.ResumeLayout(false);
			this.m_ctrlFilesGroup.PerformLayout();
			this.m_ctrlOptionsGroup.ResumeLayout(false);
			this.m_ctrlOptionsGroup.PerformLayout();
			this.m_ctrlFilterGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		/// <summary>Called to verify that the output folder specified by the user is valid for the operation</summary>
		/// <param name="strFolder">The folder specified by the user</param>
		/// <returns>True if successful</returns>
		private bool CheckOutputFolder(string strFolder)
		{
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
					return Warn("Unable to create the output folder: " + strFolder);
				}

			}

			return true; // All is good ...

		}// private bool CheckOutputFolder(string strFolder)

		/// <summary>Called to fully qualified path to the output report file for the specified script</summary>
		/// <param name="xmlScript">The source XML script</param>
		/// <returns>True if successful</returns>
		private string GetOutputFileSpec(CXmlScript xmlScript)
		{
			string	strFileSpec = "";
			string	strExtension = "";

			if(Options.ExportFolder.Length > 0)
				strFileSpec = Options.ExportFolder;
			else
				strFileSpec = GetDefaultFolder();
			if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
				strFileSpec += "\\";
				
			//	Use the same name as the XMLS file
			strFileSpec += System.IO.Path.GetFileNameWithoutExtension(xmlScript.FileSpec);
			if(strFileSpec.EndsWith(".") == false)
				strFileSpec += ".";
				
			//	Get the default extension for this format
			if(m_tmaxORETemplate != null)
			{
				strExtension = m_tmaxORETemplate.GetValue(CTmaxORETemplate.ORE_MANAGER_EXTENSION);
				if(strExtension.Length == 0)
					strExtension = m_tmaxORETemplate.GetOREExtension();
			}
			else
			{
				//	This shouldn't happen but just in case...
				Debug.Assert(false, "No report template available");
				strExtension = "xls";
			}
			
			//	Now add the extension
			strFileSpec += strExtension;
			
			return strFileSpec;

		}// private string GetOutputFileSpec(CXmlScript xmlScript)

		/// <summary>This method will set the check state of all items in the specified list box</summary>
		/// <param name="ctrlListBox">List box containing the items to be checked</param>
		private void SetSelections(CheckedListBox ctrlListBox)
		{
			int				iIndex;
			CTmaxOREOption	tmaxOREOption;

			if((ctrlListBox != null) && (ctrlListBox.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)
				{
					if((tmaxOREOption = (CTmaxOREOption)ctrlListBox.Items[iIndex]) != null)
					{
						ctrlListBox.SetItemChecked(iIndex, (CTmaxToolbox.StringToBool(tmaxOREOption.Value) == true));
					}

				}// for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)

			}// if((ctrlListBox != null) && (ctrlListBox.Items != null))

		}// private void SetSelections(CheckedListBox ctrlListBox)

		/// <summary>This method will set the long and short case name values</summary>
		private void SetCaseNames()
		{
			string	strShortName = "";
			string	strLongName = "";
			
			//	We should have an active database
			if(m_tmaxDatabase != null)
			{
				//	Has the active case changes?
				if(m_tmaxDatabase.Detail.UniqueId != Options.CaseId)
				{
					//	Reset the last names
					strShortName = m_tmaxDatabase.ShortCaseName;
					strLongName  = m_tmaxDatabase.CaseName;
				}
				else
				{
					if(Options.LastORETemplate != null)
					{
						strLongName = Options.LastORETemplate.GetValue(CTmaxORETemplate.ORE_COMMON_LONG_CASE_NAME);
						strShortName = Options.LastORETemplate.GetValue(CTmaxORETemplate.ORE_COMMON_SHORT_CASE_NAME);
					}
					
					if(strLongName.Length == 0)
						strLongName = m_tmaxDatabase.CaseName;

				}// if(m_tmaxDatabase.Detail.MasterId != Options.CaseId)

			}// if(m_tmaxDatabase != null)
			
			//	Set the control values
			m_ctrlLongCaseName.Text  = strLongName;
			m_ctrlShortCaseName.Text = strShortName;

		}// private void SetCaseNames()

		/// <summary>This method will use the selections in the list box to set the Selected property of all registration options</summary>
		/// <param name="ctrlListBox">List box containing the items to be queried</param>
		private void GetSelections(CheckedListBox ctrlListBox)
		{
			int				iIndex;
			CTmaxOREOption	tmaxOREOption;

			//	Set the file transfer selection
			if((ctrlListBox != null) && (ctrlListBox.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)
				{
					if((tmaxOREOption = (CTmaxOREOption)ctrlListBox.Items[iIndex]) != null)
					{
						if(ctrlListBox.GetItemChecked(iIndex) == true)
							tmaxOREOption.Value = "1";
						else
							tmaxOREOption.Value = "0";

					}// if((tmaxOREOption = (CTmaxOREOption)ctrlListBox.Items[iIndex]) != null)

				}// for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)

			}// if((ctrlListBox != null) && (ctrlListBox.Items != null))

		}// private void GetSelections(CheckedListBox ctrlListBox)

		/// <summary>Called to get the path to the default output folder</summary>
		/// <returns>The default folder path</returns>
		private string GetDefaultFolder()
		{
			string strFolder = "";

			try
			{
				//	The user desktop is the default
				strFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				if(strFolder.Length == 0)
					strFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					
				if((strFolder.Length > 0) && (strFolder.EndsWith("\\") == false))
					strFolder += "\\";
			}
			catch
			{
				strFolder = "";
			}

			return strFolder;

		}// private string GetDefaultFolder()

		/// <summary>Called to get the progress being reported by the ORE application</summary>
		private void CheckOREProgress()
		{
			int						iProgress = -1;
			string					strProgress = "";
			System.IO.StreamReader	oreFile = null;

			if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false) && (m_wndStatus.Visible == true))
			{
				try
				{
					//	Does the file exist?
					if(System.IO.File.Exists(m_strOREProgressFileSpec) == true)
					{
						//	Open the file stream				
						oreFile = System.IO.File.OpenText(m_strOREProgressFileSpec);

						//	Read the progress from the file
						strProgress = oreFile.ReadLine();
						strProgress.Trim();

						//	Convert to numeric value
						try { iProgress = System.Convert.ToInt32(strProgress); }
						catch { }

						//	Update the progress bar
						if(iProgress >= 0)
						{
							m_wndStatus.SetProgress(iProgress);

							//	Stop stepping the progress bar after the first update
							m_bStepProgress = false;

						}// if(iProgress > 0)

						//	Delete the file
						try
						{
							oreFile.Close();
							oreFile = null;
							System.IO.File.Delete(m_strOREProgressFileSpec);
						}
						catch
						{
						}
						
					}
					else
					{
						//	Are we still waiting on the first update from the ORE?
						if(m_bStepProgress == true)
							m_wndStatus.StepProgressBar();

					}// if(System.IO.File.Exists(m_strOREProgressFileSpec) == true)						

				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "CheckOREProgress", Ex);
				}
				finally
				{
					//	Clean up
					if(oreFile != null)
					{
						try { oreFile.Close(); }
						catch {}
						
						oreFile = null;
					}
					
				}

			}// if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false) && (m_wndStatus.Visible == true))

		}// private void CheckOREProgress()

		#endregion Private Methods

		#region Properties

		/// <summary>User defined options for generating the report</summary>
		public CROObjections Options
		{
			get { return m_reportOptions; }
			set { m_reportOptions = value; }
		}

		/// <summary>TrialMax application product descriptor</summary>
		public FTI.Shared.Trialmax.CTmaxProductManager ProductManager
		{
			get { return m_tmaxProductManager; }
			set { m_tmaxProductManager = value; }
		}

		#endregion Properties

	}// public class CRFObjections : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Reports
