using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinTabControl;

namespace FTI.Trialmax.Panes
{
	/// <summary>This form implements a wizard for importing load files from non-TrialMax applications</summary>
	public class CFImportWizard : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants
	
		private const int ERROR_CONVERT_LAUNCH_FAIL			= (ERROR_TMAX_FORM_MAX + 1);
		private const int ERROR_CONVERT_NO_XML				= (ERROR_TMAX_FORM_MAX + 2);
		private const int ERROR_CONVERT_EX					= (ERROR_TMAX_FORM_MAX + 3);
		private const int ERROR_FILL_CONVERTERS_EX			= (ERROR_TMAX_FORM_MAX + 4);
		private const int ERROR_ON_CLICK_BACK_EX			= (ERROR_TMAX_FORM_MAX + 5);
		private const int ERROR_ON_LEAVE_PAGE_EX			= (ERROR_TMAX_FORM_MAX + 6);
		private const int ERROR_ON_LOAD_PAGE_EX				= (ERROR_TMAX_FORM_MAX + 7);
		private const int ERROR_ON_CLICK_NEXT_EX			= (ERROR_TMAX_FORM_MAX + 8);
		private const int ERROR_PREVIEW_XML_EX				= (ERROR_TMAX_FORM_MAX + 9);
		private const int ERROR_PREVIEW_TEXT_EX				= (ERROR_TMAX_FORM_MAX + 10);
		private const int ERROR_CONVERTER_CHANGED_EX		= (ERROR_TMAX_FORM_MAX + 11);
		private const int ERROR_OPEN_LOAD_FILE_EX			= (ERROR_TMAX_FORM_MAX + 12);
		private const int ERROR_NO_SOURCE_FILE				= (ERROR_TMAX_FORM_MAX + 13);
		private const int ERROR_SOURCE_FILE_NOT_FOUND		= (ERROR_TMAX_FORM_MAX + 14);
		private const int ERROR_NO_SOURCE_FOLDER			= (ERROR_TMAX_FORM_MAX + 15);
		private const int ERROR_SOURCE_FOLDER_NOT_FOUND		= (ERROR_TMAX_FORM_MAX + 16);
		private const int ERROR_NO_ALTERNATE_CONVERTER		= (ERROR_TMAX_FORM_MAX + 17);
		private const int ERROR_ALTERNATE_NOT_FOUND			= (ERROR_TMAX_FORM_MAX + 18);
		private const int ERROR_CONVERTER_FILE_NOT_FOUND	= (ERROR_TMAX_FORM_MAX + 19);
		private const int ERROR_CROSS_REFERENCE_NOT_FOUND	= (ERROR_TMAX_FORM_MAX + 20);
		private const int ERROR_NO_CROSS_REFERENCE			= (ERROR_TMAX_FORM_MAX + 21);
		private const int ERROR_CREATE_TRANSFER_FOLDER		= (ERROR_TMAX_FORM_MAX + 22);
		private const int ERROR_CREATE_PARAMETERS_FILE		= (ERROR_TMAX_FORM_MAX + 23);
		private const int ERROR_WRITE_PARAMETERS_EX			= (ERROR_TMAX_FORM_MAX + 24);
		private const int ERROR_CONVERTER_EXE_NOT_FOUND		= (ERROR_TMAX_FORM_MAX + 25);
		private const int ERROR_TRANSFER_ALL_EX				= (ERROR_TMAX_FORM_MAX + 26);
		private const int ERROR_TRANSFER_PRIMARY_EX			= (ERROR_TMAX_FORM_MAX + 27);
		private const int ERROR_TRANSFER_SECONDARY_EX		= (ERROR_TMAX_FORM_MAX + 28);
		private const int ERROR_START_TRANSFER_THREAD_EX	= (ERROR_TMAX_FORM_MAX + 29);
		private const int ERROR_NO_REGISTRATION_SOURCE		= (ERROR_TMAX_FORM_MAX + 30);
		private const int ERROR_ON_SAVE_ERRORS_EX			= (ERROR_TMAX_FORM_MAX + 31);
		private const int ERROR_CREATE_GROUP_FOLDERS_EX		= (ERROR_TMAX_FORM_MAX + 32);
		
		//	Property page identifiers (indexes)
		private const int WIZARD_PAGE_CONVERT	= 0;
		private const int WIZARD_PAGE_TRANSFER	= 1;
		private const int WIZARD_PAGE_REGISTER	= 2;
		
		//	Default filenames
		private const string CONVERT_EXECUTABLE_FILENAME    = "importwiz.exe";
		private const string CONVERT_PARAMETERS_FILENAME    = "_tmax_importwiz_execute.cfg";
		private const string CONVERT_XML_RESULTS_FILENAME   = "_tmax_importwiz_results.xml";
		private const string CONVERT_TEXT_RESULTS_FILENAME  = "_tmax_importwiz_results.txt";
		private const string DEFAULT_CREATE_SOURCE_FILESPEC = "c:\\fakeload.tif";
		
		#endregion Constants
	
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Infragistics tab control used to manage the property pages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabControl m_ctrlUltraTab;
		
		/// <summary>Infragistics property page displayed when user is converting the source load file</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlConvertPage;

		/// <summary>Infragistics property page displayed when user is transferring source media to the database</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlTransferPage;

		/// <summary>Infragistics property page displayed when user is requesting source registration</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlRegisterPage;

		/// <summary>Infragistics property page containing controls common to all pages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage m_ctrlSharedPage;
		
		/// <summary>The form's Back button</summary>
		private System.Windows.Forms.Button m_ctrlBack;

		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>The form's Next button</summary>
		private System.Windows.Forms.Button m_ctrlNext;
		
		/// <summary>Image list used for the form's buttons</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Group box for source converters controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlConvertersGroup;
		
		/// <summary>List box to display converters known to the system</summary>
		private System.Windows.Forms.ListBox m_ctrlConverters;
		
		/// <summary>Label for configuration file static text control</summary>
		private System.Windows.Forms.Label m_ctrlConfigurationFileLabel;
		
		/// <summary>Static text control to display the name of the active converter configuration file</summary>
		private System.Windows.Forms.Label m_ctrlConfigurationFile;
		
		/// <summary>Text box to allow the user to point to an alternate converter configuration file</summary>
		private System.Windows.Forms.TextBox m_ctrlAlternateFile;
		
		/// <summary>Button to allow the user to browse to an alternate converter configuration file</summary>
		private System.Windows.Forms.Button m_ctrlBrowseAlternate;
		
		/// <summary>Label for the alternate configuration file edit box control</summary>
		private System.Windows.Forms.Label m_ctrlAlternateFileLabel;

		/// <summary>Group box for source file controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlSourceFilesGroup;
		
		/// <summary>Edit box to allow user to set the path to the source load file</summary>
		private System.Windows.Forms.TextBox m_ctrlSourceFile;
		
		/// <summary>Static text label for the source file edit box</summary>
		private System.Windows.Forms.Label m_ctrlSourceFileLabel;
		
		/// <summary>Browse button for the source load file</summary>
		private System.Windows.Forms.Button m_ctrlBrowseSource;
		
		/// <summary>Static text label for the cross reference file edit box</summary>
		private System.Windows.Forms.Label m_ctrlCrossReferenceFileLabel;
		
		/// <summary>Edit box to allow user to set the path to the cross reference file</summary>
		private System.Windows.Forms.TextBox m_ctrlCrossReferenceFile;
		
		/// <summary>Browse button for the cross reference file</summary>
		private System.Windows.Forms.Button m_ctrlBrowseCrossReference;
		
		/// <summary>Static text label for the source root folder edit box</summary>
		private System.Windows.Forms.Label m_ctrlSourceFolderLabel;
		
		/// <summary>Edit box to allow user to set the path to the source root folder</summary>
		private System.Windows.Forms.TextBox m_ctrlRootFolder;
		
		/// <summary>Browse button for the root folder path</summary>
		private System.Windows.Forms.Button m_ctrlBrowseRootFolder;
		
		/// <summary>Pushbutton to allow user to request preview of XML conversion</summary>
		private System.Windows.Forms.Button m_ctrlPreviewXml;
		
		/// <summary>Pushbutton to allow user to request preview of text conversion</summary>
		private System.Windows.Forms.Button m_ctrlPreviewText;

		/// <summary>Group box for conversion summary controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlConversionSummary;
		
		/// <summary>Label for control that displays the number of primary/secondary nodes in the converted XML file</summary>
		private System.Windows.Forms.Label m_ctrlConversionCountLabel;
		
		/// <summary>Static text control to display the number of primary/secondary nodes in the converted XML file</summary>
		private System.Windows.Forms.Label m_ctrlConversionCount;
		
		/// <summary>Static text control to display the number of errors in the converted XML load file</summary>
		private System.Windows.Forms.Label m_ctrlConversionErrorCount;
		
		/// <summary>Label for the conversion error count control</summary>
		private System.Windows.Forms.Label m_ctrlConversionErrorCountLabel;
		
		/// <summary>Group box for transfer page options</summary>
		private System.Windows.Forms.GroupBox m_ctrlTransferOptions;
		
		/// <summary>Check box bound to RenameFiles import option</summary>
		private System.Windows.Forms.CheckBox m_ctrlRenameFiles;
		
		/// <summary>Radio button to disable the MoveFiles import option</summary>
		private System.Windows.Forms.RadioButton m_ctrlCopyFiles;
		
		/// <summary>Radio button to enable the MoveFiles import option</summary>
		private System.Windows.Forms.RadioButton m_ctrlMoveFiles;
		
		/// <summary>Check box used for debugging to force inclusion of an error in the XML load file</summary>
		private System.Windows.Forms.CheckBox m_ctrlAddConvertError;
		
		/// <summary>Static text control to display path to the transfer root folder</summary>
		private System.Windows.Forms.Label m_ctrlTransferFolder;
		
		/// <summary>Label for static text control to display path to the transfer root folder</summary>
		private System.Windows.Forms.Label m_ctrlTransferFolderLabel;
		
		/// <summary>Static text control to display the number of errors during a transfer operation</summary>
		private System.Windows.Forms.Label m_ctrlTransferErrorCount;
		
		/// <summary>Label fo static text control that displays the number of errors during a transfer operation</summary>
		private System.Windows.Forms.Label m_ctrlTransferErrorCountLabel;
		
		/// <summary>Group box control for transfer summary controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlTransferSummaryGroup;
		
		/// <summary>Static text control to display the status message during a transfer operation</summary>
		private System.Windows.Forms.Label m_ctrlTransferStatus;
		
		/// <summary>Label for the text control to display the status message during a transfer operation</summary>
		private System.Windows.Forms.Label m_ctrlTransferStatusLabel;
		
		/// <summary>Debugging check box to request creation of the source tree</summary>
		private System.Windows.Forms.CheckBox m_ctrlCreateSource;

		/// <summary>Local member associated with the WizardOptions property</summary>
		protected CTmaxImportWizard m_tmaxWizardOptions = null;
		
		/// <summary>Local member associated with the RegisterOptions property</summary>
		protected CTmaxRegOptions m_tmaxRegisterOptions = null;
		
		///	<summary>Local member bound to Database property</summary>
		protected CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member to keep track of the active property page</summary>
		private int m_iActivePage = WIZARD_PAGE_CONVERT;
		
		/// <summary>Local member bound to keep track of the active converter</summary>
		private CTmaxLoadFileConverter m_tmaxConverter = null;
		
		/// <summary>Local member to store the path to the converter executable</summary>
		private string m_strConverterExe = "";
		
		/// <summary>Local member to store the path to the converter parameters file</summary>
		private string m_strParametersFileSpec = "";
		
		/// <summary>Local member to store the path to the converter XML output</summary>
		private string m_strXmlFileSpec = "";
		
		/// <summary>Local member to store the path to the converter text output</summary>
		private string m_strTextFileSpec = "";
		
		/// <summary>Local member to store the path to the transfer root folder</summary>
		private string m_strTransferFolder = "";
		
		/// <summary>XML load file containing the source media information</summary>
		private FTI.Shared.Xml.CXmlLoadFile m_xmlLoadFile = null;

		/// <summary>Local member to keep track of secondaries count in the XML load file</summary>
		private long m_lSecondaries = 0;
		
		/// <summary>Local member to store a custom cursor for transfer operations</summary>
		private System.Windows.Forms.Cursor m_waitArrow = null;
		
		/// <summary>Local member bound to RegisterSource property</summary>
		private FTI.Shared.Trialmax.CTmaxSourceFolder m_tmaxRegisterSource = null;
		
		/// <summary>Local member to store a flag to indicate that the transfer operation has been cancelled</summary>
		private bool m_bTransferCancelled = false;
		
		/// <summary>Progress bar to display completion status during transfer operation</summary>
		private Infragistics.Win.UltraWinProgressBar.UltraProgressBar m_ctrlTransferProgressBar;
		
		/// <summary>Button control to allow user to set registration options</summary>
		private System.Windows.Forms.Button m_ctrlRegistrationOptions;
		
		/// <summary>Button control to allow user to save conversion errors to file</summary>
		private System.Windows.Forms.Button m_ctrlSaveConversionErrors;
		
		/// <summary>Button control to allow user to save transfer errors to file</summary>
		private System.Windows.Forms.Button m_ctrlSaveTransferErrors;
		
		/// <summary>TrialMax message list control to display conversion errors</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlConversionErrors;
		
		/// <summary>TrialMax message list control to display transfer errors</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlTransferErrors;
		
		/// <summary>The form's tool tips control</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTips;
		
		/// <summary>Static text control to display description of Next operation</summary>
		private System.Windows.Forms.Label m_ctrlNextDescription;
		
		/// <summary>Static text label for transfer progress bar</summary>
		private System.Windows.Forms.Label m_ctrlTransferProgressBarLabel;

		/// <summary>Text box to allow user to set the MaxDocsPerFolder property value</summary>
		private Label m_ctrlMaxDocsPerFolderLabel;

		/// <summary>Static text label for Maximum Docs per Folder text box</summary>
		private TextBox m_ctrlMaxDocsPerFolder;
		
		/// <summary>Local member to store a flag to indicate that file transfer is in progress</summary>
		private bool m_bTransferring = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFImportWizard() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Attach(m_ctrlConversionErrors.EventSource);
			m_tmaxEventSource.Attach(m_ctrlTransferErrors.EventSource);
		
		}// public CFImportWizard()

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
		}

		/// <summary>Overridden base class member called when the form gets shown the first time</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			int iIndex = 0;
			
			if((m_tmaxWizardOptions != null) && (m_tmaxDatabase != null) &&
			   (SetConversionFileSpecs() == true))
			{
				//	Load our custom wait cursor
				try	{ m_waitArrow = new Cursor(GetType(), "Waitarrow.cur"); }
				catch {}
				
				//	Are we operating in development mode?
				if(m_tmaxWizardOptions.Development == true)
				{
					m_ctrlAddConvertError.Visible = true;
					m_ctrlCreateSource.Visible = true;
				}
				
				//	Fill the list box of source converters
				FillConverters();
			
				//	Do we have the collection of source converters?
				if(m_tmaxWizardOptions.Converters != null)
				{
					//	We want to initialize to the last used if available
					if(m_tmaxWizardOptions.LastUsed.Length > 0)
						iIndex = m_tmaxWizardOptions.Converters.IndexOf(m_tmaxWizardOptions.LastUsed);
					
					//	Initialize the file specifications
					if(m_tmaxWizardOptions.SourceFileSpec.Length > 0)
						m_ctrlSourceFile.Text = m_tmaxWizardOptions.SourceFileSpec.ToLower();	
			
					if(m_tmaxWizardOptions.CrossReferenceFileSpec.Length > 0)
						m_ctrlCrossReferenceFile.Text = m_tmaxWizardOptions.CrossReferenceFileSpec.ToLower();	
			
					if(m_tmaxWizardOptions.AlternateFileSpec.Length > 0)
						m_ctrlAlternateFile.Text = m_tmaxWizardOptions.AlternateFileSpec.ToLower();	
					
					if(m_tmaxWizardOptions.RootFolder.Length > 0)
						m_ctrlRootFolder.Text = m_tmaxWizardOptions.RootFolder.ToLower();	
			
				}
				
				//	Set the current converter selection
				if(iIndex >= 0)
					m_ctrlConverters.SelectedIndex = iIndex;
				else
					m_ctrlConverters.SelectedIndex = 0;
					
				//	Update the controls
				OnConverterChanged(m_ctrlConverters, System.EventArgs.Empty);
				
				//	Initialize the child controls
				OnLoadPage(m_iActivePage);
			}
			else
			{
				m_ctrlNextDescription.Enabled = false;
				m_ctrlNext.Enabled = false;
				m_ctrlBack.Enabled = false;
				m_ctrlConvertPage.Enabled = false;
				m_ctrlTransferPage.Enabled = false;
				m_ctrlRegisterPage.Enabled = false;
			}
			
			//	Perform the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called to display a warning message</summary>
		/// <param name="iError">Error identifier used to construct the message</param>
		/// <param name="param1">Optional error message parameter</param>
		/// <param name="param2">Optional error message parameter</param>
		private void ShowWarning(int iError, object param1, object param2)
		{
			MessageBox.Show(m_tmaxErrorBuilder.Message(iError, param1, param2), "Error",
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to launch %1 to perform the conversion.");
			m_tmaxErrorBuilder.FormatStrings.Add("The converter was unable to create the XML load file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the conversion.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the converters list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to go back to the %1 page.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to leave the %1 page.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to load the %1 page.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to go to the next page: page = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to preview the XML file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to preview the text file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to change the load file converter.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the XML load file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("You must provide a valid source file for the operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the specified source file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("You must provide a path to the source root folder.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the specified source root folder: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("You must provide an alternate configuration file if no source format is selected");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the specified alternate conversion file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the configuration file for the selected converter: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the specified cross reference file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a cross reference for the selected conversion format.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the transfer folder: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the converter parameters file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the converter parameters file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the executable used to perform the conversions: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the transfer operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to transfer the primary media source files: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to transfer the secondary source from %1 to %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to start the transfer thread.");
			m_tmaxErrorBuilder.FormatStrings.Add("No source files were transferred for registration.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to save the %1 errors to file.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the document group subfolders.");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Handles events fired when the user clicks one of the Save Errors buttons</summary>
		/// <param name="sender">The button firing the event</param>
		/// <param name="e">System event arguments</param>
		private void OnSaveErrors(object sender, System.EventArgs e)
		{
			string strSaveAs = "";
			
			try
			{
				//	Build a default path for the save file
				strSaveAs = CTmaxToolbox.GetApplicationFolder();
				if(strSaveAs.EndsWith("\\") == false)
					strSaveAs += "\\";
				strSaveAs += System.IO.Path.GetFileNameWithoutExtension(m_ctrlSourceFile.Text);
				strSaveAs += "_errors.txt";
				
				//	Save conversion errors?
				if(ReferenceEquals(m_ctrlSaveConversionErrors, sender) == true)
				{
					m_ctrlConversionErrors.Save(strSaveAs, true, true);
				}
				else
				{
					m_ctrlTransferErrors.Save(strSaveAs, true, true);
				}
			
			}
			catch(System.Exception Ex)
			{
				if(ReferenceEquals(m_ctrlSaveConversionErrors, sender) == true)
				{
					m_tmaxEventSource.FireError(this, "SaveConversionErrors", m_tmaxErrorBuilder.Message(ERROR_ON_SAVE_ERRORS_EX, "conversion"), Ex);
				}
				else
				{
					m_tmaxEventSource.FireError(this, "SaveTransferErrors", m_tmaxErrorBuilder.Message(ERROR_ON_SAVE_ERRORS_EX, "transfer"), Ex);
				}
			
			}
		
		}// private void OnSaveErrors(object sender, System.EventArgs e)

		/// <summary>This method is called to set the status message displayed during a transfer operation</summary>
		/// <param name="strStatus">The message to be displayed</param>
		private void SetTransferStatus(string strStatus)
		{
			try
			{
				m_ctrlTransferStatus.Text = CTmaxToolbox.FitPathToWidth(strStatus, m_ctrlTransferStatus);
			}
			catch
			{
			}
			
		}// private void SetTransferStatus(string strStatus)
		
		/// <summary>This method is called to add an error message during a transfer operation</summary>
		/// <param name="strMessage">The message to be added</param>
		private void AddTransferError(string strMessage, TmaxMessageLevels eLevel)
		{
			CTmaxMessage tmaxMessage = null;
			
			try
			{
				tmaxMessage = new CTmaxMessage();
				
				tmaxMessage.Text = strMessage.Trim();
				tmaxMessage.Level = eLevel;
			
				//	Add it to the list
				m_ctrlTransferErrors.Add(tmaxMessage);
			}
			catch
			{
			}
		
		}// private void AddTransferError(string strMessage)
		
		/// <summary>This method is called to add an error message during a conversion operation</summary>
		/// <param name="strMessage">The message to be added</param>
		private void AddConversionError(CXmlError xmlError)
		{
			CTmaxMessage tmaxMessage = null;
			
			try
			{
				tmaxMessage = new CTmaxMessage();
				
				tmaxMessage.Text = xmlError.Text.Trim();
				if(xmlError.Fatal == true)
					tmaxMessage.Level = TmaxMessageLevels.FatalError;
				else
					tmaxMessage.Level = TmaxMessageLevels.Warning;
			
				//	Add it to the list
				m_ctrlConversionErrors.Add(tmaxMessage);
			}
			catch
			{
			}
		
		}// private void AddConversionError(CXmlError xmlError)
		
		/// <summary>This method is called to populate the list box of load file converters</summary>
		private void FillConverters()
		{
			Debug.Assert(m_ctrlConverters != null);
			Debug.Assert(m_ctrlConverters.IsDisposed == false);
			if(m_ctrlConverters == null) return;
			if(m_ctrlConverters.IsDisposed == true) return;
			
			try
			{
				//	Clear the existing contents
				m_ctrlConverters.Items.Clear();
				
				if((m_tmaxWizardOptions != null) && (m_tmaxWizardOptions.Converters != null))
				{
					foreach(CTmaxLoadFileConverter O in m_tmaxWizardOptions.Converters)
						m_ctrlConverters.Items.Add(O);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillConverters", m_tmaxErrorBuilder.Message(ERROR_FILL_CONVERTERS_EX), Ex);
			}
			
		}// private void FillConverters()
		
		/// <summary>Handles the event fired when the user selects a new converter</summary>
		/// <param name="sender">The converters list box</param>
		/// <param name="e">System event arguements</param>
		private void OnConverterChanged(object sender, System.EventArgs e)
		{
			try
			{
				//	Get the converter selected by the user
				if(m_ctrlConverters.SelectedItem != null)
					m_tmaxConverter = (CTmaxLoadFileConverter)(m_ctrlConverters.SelectedItem);
				else
					m_tmaxConverter = null;
							
				//	Do we have an active converter?
				if(m_tmaxConverter != null)
				{
					m_ctrlConfigurationFile.Text = m_tmaxConverter.ConfigurationFilename;
						
				}
				else
				{
					m_ctrlConfigurationFile.Text = "";
					m_ctrlConfigurationFileLabel.Enabled = false;
							
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnConverterChanged", m_tmaxErrorBuilder.Message(ERROR_CONVERTER_CHANGED_EX), Ex);
			}
		
		}// private void OnConverterChanged(object sender, System.EventArgs e)
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFImportWizard));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			this.m_ctrlConvertPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlSourceFilesGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlSourceFolderLabel = new System.Windows.Forms.Label();
			this.m_ctrlRootFolder = new System.Windows.Forms.TextBox();
			this.m_ctrlBrowseRootFolder = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlSourceFile = new System.Windows.Forms.TextBox();
			this.m_ctrlBrowseSource = new System.Windows.Forms.Button();
			this.m_ctrlCrossReferenceFileLabel = new System.Windows.Forms.Label();
			this.m_ctrlCrossReferenceFile = new System.Windows.Forms.TextBox();
			this.m_ctrlBrowseCrossReference = new System.Windows.Forms.Button();
			this.m_ctrlSourceFileLabel = new System.Windows.Forms.Label();
			this.m_ctrlConvertersGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlAlternateFileLabel = new System.Windows.Forms.Label();
			this.m_ctrlAlternateFile = new System.Windows.Forms.TextBox();
			this.m_ctrlBrowseAlternate = new System.Windows.Forms.Button();
			this.m_ctrlConverters = new System.Windows.Forms.ListBox();
			this.m_ctrlConfigurationFileLabel = new System.Windows.Forms.Label();
			this.m_ctrlConfigurationFile = new System.Windows.Forms.Label();
			this.m_ctrlTransferPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlTransferOptions = new System.Windows.Forms.GroupBox();
			this.m_ctrlTransferFolder = new System.Windows.Forms.Label();
			this.m_ctrlTransferFolderLabel = new System.Windows.Forms.Label();
			this.m_ctrlMoveFiles = new System.Windows.Forms.RadioButton();
			this.m_ctrlCopyFiles = new System.Windows.Forms.RadioButton();
			this.m_ctrlRenameFiles = new System.Windows.Forms.CheckBox();
			this.m_ctrlConversionSummary = new System.Windows.Forms.GroupBox();
			this.m_ctrlConversionErrors = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.m_ctrlSaveConversionErrors = new System.Windows.Forms.Button();
			this.m_ctrlConversionErrorCount = new System.Windows.Forms.Label();
			this.m_ctrlConversionErrorCountLabel = new System.Windows.Forms.Label();
			this.m_ctrlConversionCount = new System.Windows.Forms.Label();
			this.m_ctrlConversionCountLabel = new System.Windows.Forms.Label();
			this.m_ctrlPreviewText = new System.Windows.Forms.Button();
			this.m_ctrlPreviewXml = new System.Windows.Forms.Button();
			this.m_ctrlRegisterPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlRegistrationOptions = new System.Windows.Forms.Button();
			this.m_ctrlTransferSummaryGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlTransferErrors = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.m_ctrlSaveTransferErrors = new System.Windows.Forms.Button();
			this.m_ctrlTransferProgressBarLabel = new System.Windows.Forms.Label();
			this.m_ctrlTransferProgressBar = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
			this.m_ctrlTransferErrorCount = new System.Windows.Forms.Label();
			this.m_ctrlTransferErrorCountLabel = new System.Windows.Forms.Label();
			this.m_ctrlTransferStatus = new System.Windows.Forms.Label();
			this.m_ctrlTransferStatusLabel = new System.Windows.Forms.Label();
			this.m_ctrlAddConvertError = new System.Windows.Forms.CheckBox();
			this.m_ctrlUltraTab = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.m_ctrlSharedPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.m_ctrlBack = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlNext = new System.Windows.Forms.Button();
			this.m_ctrlCreateSource = new System.Windows.Forms.CheckBox();
			this.m_ctrlToolTips = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlNextDescription = new System.Windows.Forms.Label();
			this.m_ctrlMaxDocsPerFolder = new System.Windows.Forms.TextBox();
			this.m_ctrlMaxDocsPerFolderLabel = new System.Windows.Forms.Label();
			this.m_ctrlConvertPage.SuspendLayout();
			this.m_ctrlSourceFilesGroup.SuspendLayout();
			this.m_ctrlConvertersGroup.SuspendLayout();
			this.m_ctrlTransferPage.SuspendLayout();
			this.m_ctrlTransferOptions.SuspendLayout();
			this.m_ctrlConversionSummary.SuspendLayout();
			this.m_ctrlRegisterPage.SuspendLayout();
			this.m_ctrlTransferSummaryGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTab)).BeginInit();
			this.m_ctrlUltraTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlConvertPage
			// 
			this.m_ctrlConvertPage.Controls.Add(this.m_ctrlSourceFilesGroup);
			this.m_ctrlConvertPage.Controls.Add(this.m_ctrlConvertersGroup);
			this.m_ctrlConvertPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlConvertPage.Name = "m_ctrlConvertPage";
			this.m_ctrlConvertPage.Size = new System.Drawing.Size(384, 320);
			// 
			// m_ctrlSourceFilesGroup
			// 
			this.m_ctrlSourceFilesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlSourceFolderLabel);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlRootFolder);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlBrowseRootFolder);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlSourceFile);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlBrowseSource);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlCrossReferenceFileLabel);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlCrossReferenceFile);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlBrowseCrossReference);
			this.m_ctrlSourceFilesGroup.Controls.Add(this.m_ctrlSourceFileLabel);
			this.m_ctrlSourceFilesGroup.Location = new System.Drawing.Point(6, 200);
			this.m_ctrlSourceFilesGroup.Name = "m_ctrlSourceFilesGroup";
			this.m_ctrlSourceFilesGroup.Size = new System.Drawing.Size(372, 116);
			this.m_ctrlSourceFilesGroup.TabIndex = 0;
			this.m_ctrlSourceFilesGroup.TabStop = false;
			this.m_ctrlSourceFilesGroup.Text = "Source Files";
			// 
			// m_ctrlSourceFolderLabel
			// 
			this.m_ctrlSourceFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlSourceFolderLabel.Location = new System.Drawing.Point(8, 84);
			this.m_ctrlSourceFolderLabel.Name = "m_ctrlSourceFolderLabel";
			this.m_ctrlSourceFolderLabel.Size = new System.Drawing.Size(100, 20);
			this.m_ctrlSourceFolderLabel.TabIndex = 29;
			this.m_ctrlSourceFolderLabel.Text = "Root Folder:";
			this.m_ctrlSourceFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlRootFolder
			// 
			this.m_ctrlRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlRootFolder.Location = new System.Drawing.Point(112, 84);
			this.m_ctrlRootFolder.Name = "m_ctrlRootFolder";
			this.m_ctrlRootFolder.Size = new System.Drawing.Size(224, 20);
			this.m_ctrlRootFolder.TabIndex = 4;
			// 
			// m_ctrlBrowseRootFolder
			// 
			this.m_ctrlBrowseRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBrowseRootFolder.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowseRootFolder.ImageIndex = 0;
			this.m_ctrlBrowseRootFolder.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowseRootFolder.Location = new System.Drawing.Point(340, 84);
			this.m_ctrlBrowseRootFolder.Name = "m_ctrlBrowseRootFolder";
			this.m_ctrlBrowseRootFolder.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowseRootFolder.TabIndex = 5;
			this.m_ctrlBrowseRootFolder.Click += new System.EventHandler(this.OnClickBrowseRootFolder);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlImages.Images.SetKeyName(0, "");
			this.m_ctrlImages.Images.SetKeyName(1, "");
			this.m_ctrlImages.Images.SetKeyName(2, "");
			this.m_ctrlImages.Images.SetKeyName(3, "");
			this.m_ctrlImages.Images.SetKeyName(4, "");
			this.m_ctrlImages.Images.SetKeyName(5, "");
			// 
			// m_ctrlSourceFile
			// 
			this.m_ctrlSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSourceFile.Location = new System.Drawing.Point(112, 28);
			this.m_ctrlSourceFile.Name = "m_ctrlSourceFile";
			this.m_ctrlSourceFile.Size = new System.Drawing.Size(224, 20);
			this.m_ctrlSourceFile.TabIndex = 0;
			// 
			// m_ctrlBrowseSource
			// 
			this.m_ctrlBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBrowseSource.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowseSource.ImageIndex = 0;
			this.m_ctrlBrowseSource.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowseSource.Location = new System.Drawing.Point(340, 28);
			this.m_ctrlBrowseSource.Name = "m_ctrlBrowseSource";
			this.m_ctrlBrowseSource.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowseSource.TabIndex = 1;
			this.m_ctrlBrowseSource.Click += new System.EventHandler(this.OnClickBrowse);
			// 
			// m_ctrlCrossReferenceFileLabel
			// 
			this.m_ctrlCrossReferenceFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlCrossReferenceFileLabel.Location = new System.Drawing.Point(8, 56);
			this.m_ctrlCrossReferenceFileLabel.Name = "m_ctrlCrossReferenceFileLabel";
			this.m_ctrlCrossReferenceFileLabel.Size = new System.Drawing.Size(100, 20);
			this.m_ctrlCrossReferenceFileLabel.TabIndex = 0;
			this.m_ctrlCrossReferenceFileLabel.Text = "Cross Reference:";
			this.m_ctrlCrossReferenceFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCrossReferenceFile
			// 
			this.m_ctrlCrossReferenceFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCrossReferenceFile.Location = new System.Drawing.Point(112, 56);
			this.m_ctrlCrossReferenceFile.Name = "m_ctrlCrossReferenceFile";
			this.m_ctrlCrossReferenceFile.Size = new System.Drawing.Size(224, 20);
			this.m_ctrlCrossReferenceFile.TabIndex = 2;
			// 
			// m_ctrlBrowseCrossReference
			// 
			this.m_ctrlBrowseCrossReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBrowseCrossReference.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowseCrossReference.ImageIndex = 0;
			this.m_ctrlBrowseCrossReference.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowseCrossReference.Location = new System.Drawing.Point(340, 56);
			this.m_ctrlBrowseCrossReference.Name = "m_ctrlBrowseCrossReference";
			this.m_ctrlBrowseCrossReference.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowseCrossReference.TabIndex = 3;
			this.m_ctrlBrowseCrossReference.Click += new System.EventHandler(this.OnClickBrowse);
			// 
			// m_ctrlSourceFileLabel
			// 
			this.m_ctrlSourceFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlSourceFileLabel.Location = new System.Drawing.Point(8, 28);
			this.m_ctrlSourceFileLabel.Name = "m_ctrlSourceFileLabel";
			this.m_ctrlSourceFileLabel.Size = new System.Drawing.Size(100, 20);
			this.m_ctrlSourceFileLabel.TabIndex = 28;
			this.m_ctrlSourceFileLabel.Text = "Source Load File:";
			this.m_ctrlSourceFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlConvertersGroup
			// 
			this.m_ctrlConvertersGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConvertersGroup.Controls.Add(this.m_ctrlAlternateFileLabel);
			this.m_ctrlConvertersGroup.Controls.Add(this.m_ctrlAlternateFile);
			this.m_ctrlConvertersGroup.Controls.Add(this.m_ctrlBrowseAlternate);
			this.m_ctrlConvertersGroup.Controls.Add(this.m_ctrlConverters);
			this.m_ctrlConvertersGroup.Controls.Add(this.m_ctrlConfigurationFileLabel);
			this.m_ctrlConvertersGroup.Controls.Add(this.m_ctrlConfigurationFile);
			this.m_ctrlConvertersGroup.Location = new System.Drawing.Point(6, 8);
			this.m_ctrlConvertersGroup.Name = "m_ctrlConvertersGroup";
			this.m_ctrlConvertersGroup.Size = new System.Drawing.Size(372, 180);
			this.m_ctrlConvertersGroup.TabIndex = 3;
			this.m_ctrlConvertersGroup.TabStop = false;
			this.m_ctrlConvertersGroup.Text = "Source Format";
			// 
			// m_ctrlAlternateFileLabel
			// 
			this.m_ctrlAlternateFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlAlternateFileLabel.Location = new System.Drawing.Point(12, 156);
			this.m_ctrlAlternateFileLabel.Name = "m_ctrlAlternateFileLabel";
			this.m_ctrlAlternateFileLabel.Size = new System.Drawing.Size(96, 16);
			this.m_ctrlAlternateFileLabel.TabIndex = 33;
			this.m_ctrlAlternateFileLabel.Text = "Alternate File:";
			this.m_ctrlAlternateFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAlternateFile
			// 
			this.m_ctrlAlternateFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAlternateFile.Location = new System.Drawing.Point(108, 152);
			this.m_ctrlAlternateFile.Name = "m_ctrlAlternateFile";
			this.m_ctrlAlternateFile.Size = new System.Drawing.Size(228, 20);
			this.m_ctrlAlternateFile.TabIndex = 1;
			// 
			// m_ctrlBrowseAlternate
			// 
			this.m_ctrlBrowseAlternate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBrowseAlternate.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlBrowseAlternate.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowseAlternate.ImageIndex = 0;
			this.m_ctrlBrowseAlternate.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowseAlternate.Location = new System.Drawing.Point(340, 152);
			this.m_ctrlBrowseAlternate.Name = "m_ctrlBrowseAlternate";
			this.m_ctrlBrowseAlternate.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowseAlternate.TabIndex = 2;
			this.m_ctrlBrowseAlternate.UseVisualStyleBackColor = false;
			this.m_ctrlBrowseAlternate.Click += new System.EventHandler(this.OnClickBrowse);
			// 
			// m_ctrlConverters
			// 
			this.m_ctrlConverters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConverters.IntegralHeight = false;
			this.m_ctrlConverters.Location = new System.Drawing.Point(8, 20);
			this.m_ctrlConverters.Name = "m_ctrlConverters";
			this.m_ctrlConverters.Size = new System.Drawing.Size(356, 100);
			this.m_ctrlConverters.TabIndex = 0;
			this.m_ctrlConverters.SelectedIndexChanged += new System.EventHandler(this.OnConverterChanged);
			// 
			// m_ctrlConfigurationFileLabel
			// 
			this.m_ctrlConfigurationFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlConfigurationFileLabel.Location = new System.Drawing.Point(12, 132);
			this.m_ctrlConfigurationFileLabel.Name = "m_ctrlConfigurationFileLabel";
			this.m_ctrlConfigurationFileLabel.Size = new System.Drawing.Size(96, 16);
			this.m_ctrlConfigurationFileLabel.TabIndex = 30;
			this.m_ctrlConfigurationFileLabel.Text = "Configuration File:";
			this.m_ctrlConfigurationFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlConfigurationFile
			// 
			this.m_ctrlConfigurationFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConfigurationFile.Location = new System.Drawing.Point(112, 132);
			this.m_ctrlConfigurationFile.Name = "m_ctrlConfigurationFile";
			this.m_ctrlConfigurationFile.Size = new System.Drawing.Size(252, 16);
			this.m_ctrlConfigurationFile.TabIndex = 29;
			this.m_ctrlConfigurationFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTransferPage
			// 
			this.m_ctrlTransferPage.Controls.Add(this.m_ctrlTransferOptions);
			this.m_ctrlTransferPage.Controls.Add(this.m_ctrlConversionSummary);
			this.m_ctrlTransferPage.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlTransferPage.Name = "m_ctrlTransferPage";
			this.m_ctrlTransferPage.Size = new System.Drawing.Size(384, 320);
			// 
			// m_ctrlTransferOptions
			// 
			this.m_ctrlTransferOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTransferOptions.Controls.Add(this.m_ctrlMaxDocsPerFolderLabel);
			this.m_ctrlTransferOptions.Controls.Add(this.m_ctrlMaxDocsPerFolder);
			this.m_ctrlTransferOptions.Controls.Add(this.m_ctrlTransferFolder);
			this.m_ctrlTransferOptions.Controls.Add(this.m_ctrlTransferFolderLabel);
			this.m_ctrlTransferOptions.Controls.Add(this.m_ctrlMoveFiles);
			this.m_ctrlTransferOptions.Controls.Add(this.m_ctrlCopyFiles);
			this.m_ctrlTransferOptions.Controls.Add(this.m_ctrlRenameFiles);
			this.m_ctrlTransferOptions.Location = new System.Drawing.Point(6, 204);
			this.m_ctrlTransferOptions.Name = "m_ctrlTransferOptions";
			this.m_ctrlTransferOptions.Size = new System.Drawing.Size(372, 112);
			this.m_ctrlTransferOptions.TabIndex = 1;
			this.m_ctrlTransferOptions.TabStop = false;
			this.m_ctrlTransferOptions.Text = "Transfer Options";
			// 
			// m_ctrlTransferFolder
			// 
			this.m_ctrlTransferFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTransferFolder.Location = new System.Drawing.Point(80, 28);
			this.m_ctrlTransferFolder.Name = "m_ctrlTransferFolder";
			this.m_ctrlTransferFolder.Size = new System.Drawing.Size(284, 12);
			this.m_ctrlTransferFolder.TabIndex = 1;
			this.m_ctrlTransferFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTransferFolderLabel
			// 
			this.m_ctrlTransferFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlTransferFolderLabel.Location = new System.Drawing.Point(8, 28);
			this.m_ctrlTransferFolderLabel.Name = "m_ctrlTransferFolderLabel";
			this.m_ctrlTransferFolderLabel.Size = new System.Drawing.Size(68, 12);
			this.m_ctrlTransferFolderLabel.TabIndex = 0;
			this.m_ctrlTransferFolderLabel.Text = "Transfer To:";
			this.m_ctrlTransferFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMoveFiles
			// 
			this.m_ctrlMoveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlMoveFiles.Location = new System.Drawing.Point(12, 76);
			this.m_ctrlMoveFiles.Name = "m_ctrlMoveFiles";
			this.m_ctrlMoveFiles.Size = new System.Drawing.Size(114, 24);
			this.m_ctrlMoveFiles.TabIndex = 3;
			this.m_ctrlMoveFiles.Text = "Move to case";
			// 
			// m_ctrlCopyFiles
			// 
			this.m_ctrlCopyFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlCopyFiles.Location = new System.Drawing.Point(12, 52);
			this.m_ctrlCopyFiles.Name = "m_ctrlCopyFiles";
			this.m_ctrlCopyFiles.Size = new System.Drawing.Size(105, 24);
			this.m_ctrlCopyFiles.TabIndex = 2;
			this.m_ctrlCopyFiles.Text = "Copy to case";
			// 
			// m_ctrlRenameFiles
			// 
			this.m_ctrlRenameFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlRenameFiles.Location = new System.Drawing.Point(132, 53);
			this.m_ctrlRenameFiles.Name = "m_ctrlRenameFiles";
			this.m_ctrlRenameFiles.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.m_ctrlRenameFiles.Size = new System.Drawing.Size(221, 24);
			this.m_ctrlRenameFiles.TabIndex = 4;
			this.m_ctrlRenameFiles.Text = "Rename files to match page sequence";
			this.m_ctrlRenameFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_ctrlConversionSummary
			// 
			this.m_ctrlConversionSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlConversionErrors);
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlSaveConversionErrors);
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlConversionErrorCount);
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlConversionErrorCountLabel);
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlConversionCount);
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlConversionCountLabel);
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlPreviewText);
			this.m_ctrlConversionSummary.Controls.Add(this.m_ctrlPreviewXml);
			this.m_ctrlConversionSummary.Location = new System.Drawing.Point(6, 8);
			this.m_ctrlConversionSummary.Name = "m_ctrlConversionSummary";
			this.m_ctrlConversionSummary.Size = new System.Drawing.Size(372, 192);
			this.m_ctrlConversionSummary.TabIndex = 0;
			this.m_ctrlConversionSummary.TabStop = false;
			this.m_ctrlConversionSummary.Text = "Conversion Summary";
			// 
			// m_ctrlConversionErrors
			// 
			this.m_ctrlConversionErrors.AddTop = false;
			this.m_ctrlConversionErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConversionErrors.ClearOnDblClick = false;
			this.m_ctrlConversionErrors.Format = FTI.Trialmax.Controls.TmaxMessageFormats.TextMessage;
			this.m_ctrlConversionErrors.Location = new System.Drawing.Point(8, 60);
			this.m_ctrlConversionErrors.MaxRows = 0;
			this.m_ctrlConversionErrors.Name = "m_ctrlConversionErrors";
			this.m_ctrlConversionErrors.SelectedIndex = -1;
			this.m_ctrlConversionErrors.ShowHeaders = false;
			this.m_ctrlConversionErrors.ShowImage = true;
			this.m_ctrlConversionErrors.Size = new System.Drawing.Size(356, 92);
			this.m_ctrlConversionErrors.TabIndex = 4;
			// 
			// m_ctrlSaveConversionErrors
			// 
			this.m_ctrlSaveConversionErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSaveConversionErrors.Location = new System.Drawing.Point(272, 160);
			this.m_ctrlSaveConversionErrors.Name = "m_ctrlSaveConversionErrors";
			this.m_ctrlSaveConversionErrors.Size = new System.Drawing.Size(88, 23);
			this.m_ctrlSaveConversionErrors.TabIndex = 7;
			this.m_ctrlSaveConversionErrors.Text = "Save Errors";
			this.m_ctrlSaveConversionErrors.Click += new System.EventHandler(this.OnSaveErrors);
			// 
			// m_ctrlConversionErrorCount
			// 
			this.m_ctrlConversionErrorCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConversionErrorCount.Location = new System.Drawing.Point(80, 40);
			this.m_ctrlConversionErrorCount.Name = "m_ctrlConversionErrorCount";
			this.m_ctrlConversionErrorCount.Size = new System.Drawing.Size(284, 16);
			this.m_ctrlConversionErrorCount.TabIndex = 3;
			this.m_ctrlConversionErrorCount.Text = "None";
			// 
			// m_ctrlConversionErrorCountLabel
			// 
			this.m_ctrlConversionErrorCountLabel.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlConversionErrorCountLabel.Name = "m_ctrlConversionErrorCountLabel";
			this.m_ctrlConversionErrorCountLabel.Size = new System.Drawing.Size(68, 16);
			this.m_ctrlConversionErrorCountLabel.TabIndex = 2;
			this.m_ctrlConversionErrorCountLabel.Text = "Errors: ";
			this.m_ctrlConversionErrorCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlConversionCount
			// 
			this.m_ctrlConversionCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConversionCount.Location = new System.Drawing.Point(80, 20);
			this.m_ctrlConversionCount.Name = "m_ctrlConversionCount";
			this.m_ctrlConversionCount.Size = new System.Drawing.Size(284, 16);
			this.m_ctrlConversionCount.TabIndex = 1;
			// 
			// m_ctrlConversionCountLabel
			// 
			this.m_ctrlConversionCountLabel.Location = new System.Drawing.Point(8, 20);
			this.m_ctrlConversionCountLabel.Name = "m_ctrlConversionCountLabel";
			this.m_ctrlConversionCountLabel.Size = new System.Drawing.Size(68, 16);
			this.m_ctrlConversionCountLabel.TabIndex = 0;
			this.m_ctrlConversionCountLabel.Text = "Source: ";
			this.m_ctrlConversionCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlPreviewText
			// 
			this.m_ctrlPreviewText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlPreviewText.Location = new System.Drawing.Point(108, 160);
			this.m_ctrlPreviewText.Name = "m_ctrlPreviewText";
			this.m_ctrlPreviewText.Size = new System.Drawing.Size(88, 23);
			this.m_ctrlPreviewText.TabIndex = 6;
			this.m_ctrlPreviewText.Text = "Preview Text";
			this.m_ctrlPreviewText.Click += new System.EventHandler(this.OnClickPreviewText);
			// 
			// m_ctrlPreviewXml
			// 
			this.m_ctrlPreviewXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlPreviewXml.Location = new System.Drawing.Point(8, 160);
			this.m_ctrlPreviewXml.Name = "m_ctrlPreviewXml";
			this.m_ctrlPreviewXml.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlPreviewXml.TabIndex = 5;
			this.m_ctrlPreviewXml.Text = "Preview XML";
			this.m_ctrlPreviewXml.Click += new System.EventHandler(this.OnClickPreviewXml);
			// 
			// m_ctrlRegisterPage
			// 
			this.m_ctrlRegisterPage.Controls.Add(this.m_ctrlRegistrationOptions);
			this.m_ctrlRegisterPage.Controls.Add(this.m_ctrlTransferSummaryGroup);
			this.m_ctrlRegisterPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlRegisterPage.Name = "m_ctrlRegisterPage";
			this.m_ctrlRegisterPage.Size = new System.Drawing.Size(384, 320);
			// 
			// m_ctrlRegistrationOptions
			// 
			this.m_ctrlRegistrationOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlRegistrationOptions.Location = new System.Drawing.Point(248, 284);
			this.m_ctrlRegistrationOptions.Name = "m_ctrlRegistrationOptions";
			this.m_ctrlRegistrationOptions.Size = new System.Drawing.Size(124, 23);
			this.m_ctrlRegistrationOptions.TabIndex = 3;
			this.m_ctrlRegistrationOptions.Text = "Registration Options";
			this.m_ctrlRegistrationOptions.Click += new System.EventHandler(this.OnClickRegistrationOptions);
			// 
			// m_ctrlTransferSummaryGroup
			// 
			this.m_ctrlTransferSummaryGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlTransferErrors);
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlSaveTransferErrors);
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlTransferProgressBarLabel);
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlTransferProgressBar);
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlTransferErrorCount);
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlTransferErrorCountLabel);
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlTransferStatus);
			this.m_ctrlTransferSummaryGroup.Controls.Add(this.m_ctrlTransferStatusLabel);
			this.m_ctrlTransferSummaryGroup.Location = new System.Drawing.Point(6, 8);
			this.m_ctrlTransferSummaryGroup.Name = "m_ctrlTransferSummaryGroup";
			this.m_ctrlTransferSummaryGroup.Size = new System.Drawing.Size(372, 248);
			this.m_ctrlTransferSummaryGroup.TabIndex = 2;
			this.m_ctrlTransferSummaryGroup.TabStop = false;
			this.m_ctrlTransferSummaryGroup.Text = "Transfer Summary";
			// 
			// m_ctrlTransferErrors
			// 
			this.m_ctrlTransferErrors.AddTop = false;
			this.m_ctrlTransferErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTransferErrors.ClearOnDblClick = false;
			this.m_ctrlTransferErrors.Format = FTI.Trialmax.Controls.TmaxMessageFormats.TextMessage;
			this.m_ctrlTransferErrors.Location = new System.Drawing.Point(8, 96);
			this.m_ctrlTransferErrors.MaxRows = 0;
			this.m_ctrlTransferErrors.Name = "m_ctrlTransferErrors";
			this.m_ctrlTransferErrors.SelectedIndex = -1;
			this.m_ctrlTransferErrors.ShowHeaders = false;
			this.m_ctrlTransferErrors.ShowImage = true;
			this.m_ctrlTransferErrors.Size = new System.Drawing.Size(356, 112);
			this.m_ctrlTransferErrors.TabIndex = 16;
			// 
			// m_ctrlSaveTransferErrors
			// 
			this.m_ctrlSaveTransferErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSaveTransferErrors.Location = new System.Drawing.Point(276, 216);
			this.m_ctrlSaveTransferErrors.Name = "m_ctrlSaveTransferErrors";
			this.m_ctrlSaveTransferErrors.Size = new System.Drawing.Size(88, 23);
			this.m_ctrlSaveTransferErrors.TabIndex = 15;
			this.m_ctrlSaveTransferErrors.Text = "Save Errors";
			this.m_ctrlSaveTransferErrors.Click += new System.EventHandler(this.OnSaveErrors);
			// 
			// m_ctrlTransferProgressBarLabel
			// 
			this.m_ctrlTransferProgressBarLabel.Location = new System.Drawing.Point(8, 52);
			this.m_ctrlTransferProgressBarLabel.Name = "m_ctrlTransferProgressBarLabel";
			this.m_ctrlTransferProgressBarLabel.Size = new System.Drawing.Size(68, 16);
			this.m_ctrlTransferProgressBarLabel.TabIndex = 14;
			this.m_ctrlTransferProgressBarLabel.Text = "Progress:";
			this.m_ctrlTransferProgressBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTransferProgressBar
			// 
			this.m_ctrlTransferProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			appearance1.BackColor = System.Drawing.SystemColors.Window;
			this.m_ctrlTransferProgressBar.Appearance = appearance1;
			this.m_ctrlTransferProgressBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
			appearance2.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.m_ctrlTransferProgressBar.FillAppearance = appearance2;
			this.m_ctrlTransferProgressBar.Location = new System.Drawing.Point(76, 52);
			this.m_ctrlTransferProgressBar.Name = "m_ctrlTransferProgressBar";
			this.m_ctrlTransferProgressBar.Size = new System.Drawing.Size(288, 16);
			this.m_ctrlTransferProgressBar.Step = 1;
			this.m_ctrlTransferProgressBar.TabIndex = 13;
			this.m_ctrlTransferProgressBar.Text = "[Formatted]";
			// 
			// m_ctrlTransferErrorCount
			// 
			this.m_ctrlTransferErrorCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTransferErrorCount.Location = new System.Drawing.Point(80, 76);
			this.m_ctrlTransferErrorCount.Name = "m_ctrlTransferErrorCount";
			this.m_ctrlTransferErrorCount.Size = new System.Drawing.Size(284, 16);
			this.m_ctrlTransferErrorCount.TabIndex = 12;
			this.m_ctrlTransferErrorCount.Text = "None";
			// 
			// m_ctrlTransferErrorCountLabel
			// 
			this.m_ctrlTransferErrorCountLabel.Location = new System.Drawing.Point(8, 76);
			this.m_ctrlTransferErrorCountLabel.Name = "m_ctrlTransferErrorCountLabel";
			this.m_ctrlTransferErrorCountLabel.Size = new System.Drawing.Size(68, 16);
			this.m_ctrlTransferErrorCountLabel.TabIndex = 11;
			this.m_ctrlTransferErrorCountLabel.Text = "Errors: ";
			this.m_ctrlTransferErrorCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTransferStatus
			// 
			this.m_ctrlTransferStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTransferStatus.Location = new System.Drawing.Point(80, 24);
			this.m_ctrlTransferStatus.Name = "m_ctrlTransferStatus";
			this.m_ctrlTransferStatus.Size = new System.Drawing.Size(284, 24);
			this.m_ctrlTransferStatus.TabIndex = 10;
			// 
			// m_ctrlTransferStatusLabel
			// 
			this.m_ctrlTransferStatusLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlTransferStatusLabel.Name = "m_ctrlTransferStatusLabel";
			this.m_ctrlTransferStatusLabel.Size = new System.Drawing.Size(68, 16);
			this.m_ctrlTransferStatusLabel.TabIndex = 9;
			this.m_ctrlTransferStatusLabel.Text = "Status:";
			this.m_ctrlTransferStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAddConvertError
			// 
			this.m_ctrlAddConvertError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlAddConvertError.Location = new System.Drawing.Point(8, 356);
			this.m_ctrlAddConvertError.Name = "m_ctrlAddConvertError";
			this.m_ctrlAddConvertError.Size = new System.Drawing.Size(128, 20);
			this.m_ctrlAddConvertError.TabIndex = 1;
			this.m_ctrlAddConvertError.Text = "Add conversion error";
			this.m_ctrlAddConvertError.Visible = false;
			// 
			// m_ctrlUltraTab
			// 
			this.m_ctrlUltraTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUltraTab.BackColorInternal = System.Drawing.SystemColors.Control;
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlSharedPage);
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlConvertPage);
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlTransferPage);
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlRegisterPage);
			this.m_ctrlUltraTab.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlUltraTab.Name = "m_ctrlUltraTab";
			this.m_ctrlUltraTab.SharedControlsPage = this.m_ctrlSharedPage;
			this.m_ctrlUltraTab.Size = new System.Drawing.Size(384, 320);
			this.m_ctrlUltraTab.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Wizard;
			this.m_ctrlUltraTab.TabIndex = 0;
			ultraTab1.TabPage = this.m_ctrlConvertPage;
			ultraTab1.Text = "Convert";
			ultraTab1.ToolTipText = "Select Conversion Format";
			ultraTab2.TabPage = this.m_ctrlTransferPage;
			ultraTab2.Text = "Transfer";
			ultraTab2.ToolTipText = "Transfer Source Files";
			ultraTab3.TabPage = this.m_ctrlRegisterPage;
			ultraTab3.Text = "Register";
			ultraTab3.ToolTipText = "Register Source Files";
			this.m_ctrlUltraTab.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
			// 
			// m_ctrlSharedPage
			// 
			this.m_ctrlSharedPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlSharedPage.Name = "m_ctrlSharedPage";
			this.m_ctrlSharedPage.Size = new System.Drawing.Size(384, 320);
			// 
			// m_ctrlBack
			// 
			this.m_ctrlBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBack.Location = new System.Drawing.Point(224, 370);
			this.m_ctrlBack.Name = "m_ctrlBack";
			this.m_ctrlBack.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlBack.TabIndex = 4;
			this.m_ctrlBack.Text = "<  &Back";
			this.m_ctrlBack.Click += new System.EventHandler(this.OnClickBack);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.Location = new System.Drawing.Point(140, 370);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "&Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlNext
			// 
			this.m_ctrlNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNext.Location = new System.Drawing.Point(304, 370);
			this.m_ctrlNext.Name = "m_ctrlNext";
			this.m_ctrlNext.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlNext.TabIndex = 5;
			this.m_ctrlNext.Text = "&Next  >";
			this.m_ctrlNext.Click += new System.EventHandler(this.OnClickNext);
			// 
			// m_ctrlCreateSource
			// 
			this.m_ctrlCreateSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlCreateSource.Location = new System.Drawing.Point(8, 376);
			this.m_ctrlCreateSource.Name = "m_ctrlCreateSource";
			this.m_ctrlCreateSource.Size = new System.Drawing.Size(128, 20);
			this.m_ctrlCreateSource.TabIndex = 2;
			this.m_ctrlCreateSource.Text = "Create source tree";
			this.m_ctrlCreateSource.Visible = false;
			// 
			// m_ctrlNextDescription
			// 
			this.m_ctrlNextDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNextDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_ctrlNextDescription.Location = new System.Drawing.Point(10, 330);
			this.m_ctrlNextDescription.Name = "m_ctrlNextDescription";
			this.m_ctrlNextDescription.Size = new System.Drawing.Size(372, 23);
			this.m_ctrlNextDescription.TabIndex = 0;
			this.m_ctrlNextDescription.Text = "Action descriptor goes here";
			this.m_ctrlNextDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMaxDocsPerFolder
			// 
			this.m_ctrlMaxDocsPerFolder.Location = new System.Drawing.Point(293, 80);
			this.m_ctrlMaxDocsPerFolder.Name = "m_ctrlMaxDocsPerFolder";
			this.m_ctrlMaxDocsPerFolder.Size = new System.Drawing.Size(60, 20);
			this.m_ctrlMaxDocsPerFolder.TabIndex = 6;
			// 
			// m_ctrlMaxDocsPerFolderLabel
			// 
			this.m_ctrlMaxDocsPerFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlMaxDocsPerFolderLabel.Location = new System.Drawing.Point(134, 80);
			this.m_ctrlMaxDocsPerFolderLabel.Name = "m_ctrlMaxDocsPerFolderLabel";
			this.m_ctrlMaxDocsPerFolderLabel.Size = new System.Drawing.Size(156, 18);
			this.m_ctrlMaxDocsPerFolderLabel.TabIndex = 5;
			this.m_ctrlMaxDocsPerFolderLabel.Text = "Max documents per folder";
			this.m_ctrlMaxDocsPerFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFImportWizard
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 401);
			this.Controls.Add(this.m_ctrlNextDescription);
			this.Controls.Add(this.m_ctrlCreateSource);
			this.Controls.Add(this.m_ctrlNext);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlBack);
			this.Controls.Add(this.m_ctrlUltraTab);
			this.Controls.Add(this.m_ctrlAddConvertError);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFImportWizard";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Import Wizard";
			this.m_ctrlConvertPage.ResumeLayout(false);
			this.m_ctrlSourceFilesGroup.ResumeLayout(false);
			this.m_ctrlSourceFilesGroup.PerformLayout();
			this.m_ctrlConvertersGroup.ResumeLayout(false);
			this.m_ctrlConvertersGroup.PerformLayout();
			this.m_ctrlTransferPage.ResumeLayout(false);
			this.m_ctrlTransferOptions.ResumeLayout(false);
			this.m_ctrlTransferOptions.PerformLayout();
			this.m_ctrlConversionSummary.ResumeLayout(false);
			this.m_ctrlRegisterPage.ResumeLayout(false);
			this.m_ctrlTransferSummaryGroup.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTab)).EndInit();
			this.m_ctrlUltraTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>This method is called when the user clicks on the Registration options button</summary>
		/// <param name="sender">The registration options button</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnClickRegistrationOptions(object sender, System.EventArgs e)
		{
			CFRegOptions regOptions = null;
			
			
			if(m_tmaxRegisterOptions != null)
			{
				try
				{
					regOptions = new CFRegOptions();

					//	Set the options used by the form
					regOptions.Options = m_tmaxRegisterOptions;
				
					regOptions.ShowDialog(this);
				}
				catch
				{
				}
				
				//	Force cleanup of the form
				if(regOptions != null)
					regOptions.Dispose();
			
			}// if(m_tmaxRegisterOptions != null)
			
		}// private void OnClickRegistrationOptions(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on the Back button</summary>
		/// <param name="sender">The back button</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnClickBack(object sender, System.EventArgs e)
		{
			string strBack = "";
			
			try
			{
				//	Which tab is active?
				switch(m_iActivePage)
				{ 
					case WIZARD_PAGE_CONVERT:
					
						break;
						
					case WIZARD_PAGE_TRANSFER:
					
						strBack = "Convert";
						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectPreviousTab);
						m_iActivePage = WIZARD_PAGE_CONVERT;
						break;
						
					case WIZARD_PAGE_REGISTER:
					
						strBack = "Transfer";
						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectPreviousTab);
						m_iActivePage = WIZARD_PAGE_TRANSFER;
						break;
						
				}// switch(m_iActivePage)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickBack", m_tmaxErrorBuilder.Message(ERROR_ON_CLICK_BACK_EX, strBack), Ex);
			}
			
			//	Make sure the child controls are initialized
			OnLoadPage(m_iActivePage);
			
		}// private void OnClickBack(object sender, System.EventArgs e)
	
		/// <summary>This method is called when the user clicks on the Next button</summary>
		/// <param name="sender">The next button</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnClickNext(object sender, System.EventArgs e)
		{
			string strNext = "";
			
			//	Make sure it's OK to leave the active page
			if(OnLeavePage(m_iActivePage) == false) return;
			
			try
			{
				//	Which tab is active?
				switch(m_iActivePage)
				{ 
					case WIZARD_PAGE_CONVERT:

						strNext = "Transfer";
						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectNextTab);
						m_iActivePage = WIZARD_PAGE_TRANSFER;
			
						//	Make sure the child controls are initialized
						OnLoadPage(m_iActivePage);
						break;
						
					case WIZARD_PAGE_TRANSFER:
					
						strNext = "Register";
						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectNextTab);
						m_iActivePage = WIZARD_PAGE_REGISTER;
			
						//	Make sure the child controls are initialized
						OnLoadPage(m_iActivePage);
						break;
						
					case WIZARD_PAGE_REGISTER:
					
						//	Perform the registration
						//
						//	NOTE:	This will close the form
						Register();
						break;
				
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickNext", m_tmaxErrorBuilder.Message(ERROR_ON_CLICK_NEXT_EX, strNext), Ex);
			}
			
		}// private void OnClickNext(object sender, System.EventArgs e)
	
		/// <summary>This method is called when the user clicks on the Cancel button</summary>
		/// <param name="sender">The Cancel button</param>
		/// <param name="e">System event arguments - not used</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			//	Are we transferring files?
			if(m_bTransferring == true)
			{
				//	Cancel the operation
				m_bTransferCancelled = true;
			}
			else
			{
				//	Close the form
				DialogResult = DialogResult.Cancel;
				this.Close();
			}
		
		}// private void OnClickCancel(object sender, System.EventArgs e)
		
		/// <summary>Called when the user is about to leave a property page and move to a new one</summary>
		/// <param name="iPage">The id of the page being closed</param>
		/// <returns>True if OK to leave the page</returns>
		private bool OnLeavePage(int iPage)
		{
			string strLeaving = "";
			
			try
			{
				//	Which page is active?
				switch(iPage)
				{ 
					case WIZARD_PAGE_CONVERT:

						strLeaving = "Convert";
						
						//	Retrieve the values specified by the user
						if(PrepareConversion() == false) 
							return false;
						
						//	Perform the conversion
						if(Convert() == false)
							return false;
						
						//	Open the XML load file
						if(OpenLoadFile() == false)
							return false;
							
						break;
						
					case WIZARD_PAGE_TRANSFER:
					
						strLeaving = "Transfer";
						
						//	Prepare to transfer the source files
						if(PrepareTransfer() == false) 
							return false;
						
						break;
						
					case WIZARD_PAGE_REGISTER:

						strLeaving = "Register";
						break;
						
				}// switch(iPage)
			
				return true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnLeavePage", m_tmaxErrorBuilder.Message(ERROR_ON_LEAVE_PAGE_EX, strLeaving), Ex);
				return false;
			}
		
		}// private bool OnLeavePage(int iPage)
		
		/// <summary>Called when the user has transitioned to a new page</summary>
		/// <param name="iPage">The id of the page being loaded</param>
		private void OnLoadPage(int iPage)
		{
			string strLoading = "";

			try
			{
				//	Which page is active?
				switch(iPage)
				{ 
					case WIZARD_PAGE_CONVERT:

						strLoading = "Convert";
						
						m_ctrlNextDescription.Text = " Click Next to convert the load file ...";
						m_ctrlBack.Enabled = false;
						
						m_ctrlNextDescription.Enabled = true;
						m_ctrlNext.Enabled = true;
						
						m_ctrlToolTips.SetToolTip(m_ctrlCancel, "Cancel the operation");
						m_ctrlToolTips.SetToolTip(m_ctrlNext, "Perform the conversion");
						m_ctrlToolTips.SetToolTip(m_ctrlBack, "");
						m_ctrlToolTips.SetToolTip(m_ctrlConverters, "Select a pre-defined converter");
						m_ctrlToolTips.SetToolTip(m_ctrlAlternateFile, "Specify an alternate converter configuration file");
						m_ctrlToolTips.SetToolTip(m_ctrlAlternateFileLabel, "Specify an alternate converter configuration file");
						m_ctrlToolTips.SetToolTip(m_ctrlSourceFile, "Specify the load file to be converted");
						m_ctrlToolTips.SetToolTip(m_ctrlSourceFileLabel, "Specify the load file to be converted");
						m_ctrlToolTips.SetToolTip(m_ctrlCrossReferenceFile, "Specify the cross reference file");
						m_ctrlToolTips.SetToolTip(m_ctrlCrossReferenceFileLabel, "Specify the cross reference file");
						m_ctrlToolTips.SetToolTip(m_ctrlRootFolder, "Specify the root folder containing the source files");
						m_ctrlToolTips.SetToolTip(m_ctrlSourceFolderLabel, "Specify the root folder containing the source files");
						
						break;
						
					case WIZARD_PAGE_TRANSFER:
					
						strLoading = "Transfer";
						
						//	Set the transfer root folder
						SetTransferFolder();
						
						m_ctrlNextDescription.Text = " Click Next to start transferring files ...";
						m_ctrlBack.Enabled = true;
						
						m_ctrlNextDescription.Enabled = (m_lSecondaries > 0);
						m_ctrlNext.Enabled = (m_lSecondaries > 0);
						
						m_ctrlPreviewXml.Enabled = System.IO.File.Exists(m_strXmlFileSpec);
						m_ctrlPreviewText.Enabled = System.IO.File.Exists(m_strTextFileSpec);
						
						m_ctrlTransferFolder.Text = CTmaxToolbox.FitPathToWidth(m_strTransferFolder, m_ctrlTransferFolder);
						
						m_ctrlCopyFiles.Checked = (m_tmaxWizardOptions.MoveFiles == false);
						m_ctrlMoveFiles.Checked = (m_tmaxWizardOptions.MoveFiles == true);
						m_ctrlRenameFiles.Checked = (m_tmaxWizardOptions.RenameFiles == true);
						m_ctrlMaxDocsPerFolder.Text = m_tmaxWizardOptions.MaxDocsPerFolder.ToString();
				
						if(m_lSecondaries > 0)
						{
							m_ctrlConversionCount.Text = (m_lSecondaries.ToString() + " files in " + m_xmlLoadFile.Primaries.Count.ToString() + " documents");
						}
						else
						{
							m_ctrlConversionCount.Text = "No source files found";
						}
						
						//	Populate the error messages
						m_ctrlConversionErrors.Clear();
						if((m_xmlLoadFile != null) && (m_xmlLoadFile.Errors != null))
						{
							foreach(CXmlError O in m_xmlLoadFile.Errors)
							{
								AddConversionError(O);
							}

						}// if((m_xmlLoadFile != null) && (m_xmlLoadFile.Errors != null))
						
						//	Were there any errors?
						if(m_ctrlConversionErrors.Count > 0)
						{
							m_ctrlConversionErrorCount.Text = m_ctrlConversionErrors.Count.ToString();
							m_ctrlSaveConversionErrors.Enabled = true;
						}
						else
						{
							m_ctrlConversionErrorCount.Text = "None";
							m_ctrlSaveConversionErrors.Enabled = false;
						}
						
						//	Were there any fatal errors?
						if((m_xmlLoadFile != null) && (m_xmlLoadFile.Errors != null) && (m_xmlLoadFile.Errors.GetFatalCount() > 0))
						{
							m_ctrlNextDescription.Enabled = false;
							m_ctrlNext.Enabled = false;
							
							//	Warn the user
							MessageBox.Show("The conversion process has reported " + m_xmlLoadFile.Errors.GetFatalCount().ToString() + " fatal errors", "Error", 
											MessageBoxButtons.OK, MessageBoxIcon.Stop);
						}
						
						m_ctrlToolTips.SetToolTip(m_ctrlNext, "Start transferring files");
						m_ctrlToolTips.SetToolTip(m_ctrlBack, "Go back to conversion page");
						m_ctrlToolTips.SetToolTip(m_ctrlPreviewXml, "Preview conversion as XML");
						m_ctrlToolTips.SetToolTip(m_ctrlPreviewText, "Preview conversion as text");
						m_ctrlToolTips.SetToolTip(m_ctrlSaveConversionErrors, "Save conversion errors to file");
						m_ctrlToolTips.SetToolTip(m_ctrlCopyFiles, "Copy files to the transfer folder");
						m_ctrlToolTips.SetToolTip(m_ctrlMoveFiles, "Move files to the transfer folder");
						m_ctrlToolTips.SetToolTip(m_ctrlMaxDocsPerFolder, "Maximum number of documents allowed per target database folder");
						m_ctrlToolTips.SetToolTip(m_ctrlMaxDocsPerFolderLabel, "Maximum number of documents allowed per target database folder");
						m_ctrlToolTips.SetToolTip(m_ctrlRenameFiles, "Rename files to indicate page number (0001.tif, 0002.tif, ...)");
						m_ctrlToolTips.SetToolTip(m_ctrlTransferFolder, m_strTransferFolder);
		
						break;
						
					case WIZARD_PAGE_REGISTER:
					
						strLoading = "Register";
						
						m_ctrlNextDescription.Text = " Click Next to register the source files ...";
						
						m_ctrlToolTips.SetToolTip(m_ctrlNext, "Register the files");
						m_ctrlToolTips.SetToolTip(m_ctrlBack, "Go back to transfer page");
						m_ctrlToolTips.SetToolTip(m_ctrlRegistrationOptions, "Set options for registering source files");
						m_ctrlToolTips.SetToolTip(m_ctrlSaveTransferErrors, "Save transfer errors to file");

						//	Clear the error information
						m_ctrlTransferErrors.Clear();
						m_ctrlTransferErrorCount.Text = "None";
						
						//	Don't allow the user to go back or forward until finished
						m_ctrlNextDescription.Enabled = false;
						m_ctrlNext.Enabled = false;
						m_ctrlBack.Enabled = false;
						m_ctrlSaveTransferErrors.Enabled = false;
				
						//	Perform the source transfer
						StartTransferThread();
						break;
						
				}// switch(iPage)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnLoadPage", m_tmaxErrorBuilder.Message(ERROR_ON_LOAD_PAGE_EX, strLoading), Ex);
			}
			
		}// private void OnLoadPage(int iPage)
		
		/// <summary>Called when the file transfer operation has finished</summary>
		private void OnTransferComplete()
		{
			long	lFolders = 0;
			long	lFiles = 0;

			//	Transfer operation is no longer active
			m_bTransferring = false;
            EnableBackCtrl();
            //m_ctrlBack.Enabled = true;
			
			//	Was the operation cancelled?
			if(m_bTransferCancelled == true)
			{
				SetTransferStatus("transfer cancelled");
				m_ctrlNextDescription.Enabled = false;
				m_ctrlNext.Enabled = false;
			
				//	Clear this flag so that the user can close the form
				m_bTransferCancelled = false;
			}
			else
			{
				if((m_tmaxRegisterSource == null) || (m_tmaxRegisterSource.GetFileCount(true) == 0))
				{
					SetTransferStatus("no files transferred");
					m_ctrlNextDescription.Enabled = false;
					m_ctrlNext.Enabled = false;
				}
				else
				{
					//	Get the file count
					foreach(CTmaxSourceFolder O in m_tmaxRegisterSource.SubFolders)
					{
						if((O.Files != null) && (O.Files.Count > 0))
						{
							lFolders += 1;
							lFiles += O.Files.Count;
						}
						//	We may be using document group folders
						else if((O.SubFolders != null) && (O.SubFolders.Count > 0))
						{
							foreach(CTmaxSourceFolder S in O.SubFolders)
							{
								if((S.Files != null) && (S.Files.Count > 0))
								{
									lFolders += 1;
									lFiles += S.Files.Count;
								}

							}// foreach(CTmaxSourceFolder S in O.SubFolders)


						}// else if((O.SubFolders != null) && (O.SubFolders.Count > 0))
						
					}
					
					SetTransferStatus("transferred " + lFiles.ToString() + " files in " + lFolders.ToString() + " folders");
					m_ctrlNextDescription.Enabled = true;
					m_ctrlNext.Enabled = true;
				}
				
				//	Alert the user if there were any errors
				if(m_ctrlTransferErrors.Count > 0)
				{
					MessageBox.Show("The transfer process reported " + m_ctrlTransferErrors.Count.ToString() + " errors", "Warning",
									MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    SetTransferError();
                    //m_ctrlSaveTransferErrors.Enabled = true;
				}
				else
				{
					m_ctrlSaveTransferErrors.Enabled = false;
				}
			
				//	Make sure the progress bar is 100%
				//m_ctrlTransferProgressBar.Value = m_ctrlTransferProgressBar.Maximum;
                SetProgressBarValue();
				
			}// if(m_bTransferCancelled == true)
			
		}// private void OnTransferComplete()


        delegate void ProgressBarValueCallback();
        private void SetProgressBarValue()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.m_ctrlTransferProgressBar.InvokeRequired)
            {
                ProgressBarValueCallback d = new ProgressBarValueCallback(SetProgressBarValue);
                m_ctrlTransferProgressBar.Invoke(d, new object[] { });
                
            }
            else
            {
                m_ctrlTransferProgressBar.Value = m_ctrlTransferProgressBar.Maximum;
            }
        }

        delegate void SetTransferErrorCallback();
        private void SetTransferError()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.m_ctrlSaveTransferErrors.InvokeRequired)
            {
                SetTransferErrorCallback d = new SetTransferErrorCallback(SetTransferError);
                m_ctrlSaveTransferErrors.Invoke(d, new object[] { });

            }
            else
            {
                m_ctrlSaveTransferErrors.Enabled = true;
            }
        }
			


        delegate void EnableBackCtrlCallback();
        private void EnableBackCtrl()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.

            if (this.m_ctrlBack.InvokeRequired)
            {
                EnableBackCtrlCallback d = new EnableBackCtrlCallback(EnableBackCtrl);
                m_ctrlBack.Invoke(d, new object[] { });

            }
            else
            {
                m_ctrlBack.Enabled = true;
            }
        }

		
		/// <summary>Handles the event fired when the user clicks on one of the Browse buttons</summary>
		/// <param name="sender">The button firing the event</param>
		/// <param name="e">System event arguements</param>
		private void OnClickBrowse(object sender, System.EventArgs e)
		{
			System.Windows.Forms.TextBox ctrlPath = null;
			string strFilter = "";
			
			//	Which browse button was clicked
			if(ReferenceEquals(m_ctrlBrowseSource, sender) == true)
			{
				ctrlPath = m_ctrlSourceFile;
				if(m_tmaxConverter != null)
					strFilter = m_tmaxConverter.GetFilterString();
				else
					strFilter = "All Files (*.*)|*.*";
			}
			else if(ReferenceEquals(m_ctrlBrowseCrossReference, sender) == true)
			{
				ctrlPath = m_ctrlCrossReferenceFile;
				strFilter = "Default Files (*.txt;*.csv)|*.txt;*.csv|All Files (*.*)|*.*";
			}
			else if(ReferenceEquals(m_ctrlBrowseAlternate, sender) == true)
			{
				ctrlPath = m_ctrlAlternateFile;
				strFilter = "Import Wizard (*.cfg)|*.cfg|All Files (*.*)|*.*";
			}
			else
			{
				Debug.Assert(false);
				return;
			}
			
			OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
			
			//	Initialize the file selection dialog
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.Multiselect = false;
			dlg.Title = "Select File";
			dlg.Filter = strFilter;
			dlg.FileName = ctrlPath.Text;
			
			//	Open the dialog box
			if(dlg.ShowDialog() == DialogResult.OK) 
			{
				ctrlPath.Text = dlg.FileName.ToLower();
			}
		
		}// private void OnClickBrowse(object sender, System.EventArgs e)
		
		/// <summary>Retrieves the values specified by the user and prepares the conversion propcess</summary>
		/// <returns>true if successful</returns>
		private bool PrepareConversion()
		{
			bool	bSuccessful = false;
			string	strConfigFile = "";
			string	strCrossReference = "";
			
			while(bSuccessful == false)
			{
				//	Make sure the user provided a source file
				if(m_ctrlSourceFile.Text.Length == 0)
				{
					ShowWarning(ERROR_NO_SOURCE_FILE, null, null);
					m_ctrlSourceFile.Focus();
					break;
				}
				
				//	Make sure the source file exists
				if(System.IO.File.Exists(m_ctrlSourceFile.Text) == false)
				{
					ShowWarning(ERROR_SOURCE_FILE_NOT_FOUND, m_ctrlSourceFile.Text, null);
					m_ctrlSourceFile.Focus();
					m_ctrlSourceFile.SelectAll();
					break;
				}
				
				//	Make sure the user provided a root folder
				if(m_ctrlRootFolder.Text.Length == 0)
				{
					ShowWarning(ERROR_NO_SOURCE_FOLDER, null, null);
					m_ctrlRootFolder.Focus();
					break;
				}
				
				//	Make sure the source file exists
				if(System.IO.Directory.Exists(m_ctrlRootFolder.Text) == false)
				{
					ShowWarning(ERROR_SOURCE_FOLDER_NOT_FOUND, m_ctrlRootFolder.Text, null);
					m_ctrlRootFolder.Focus();
					m_ctrlRootFolder.SelectAll();
					break;
				}
				
				//	Are we going to be using an alternate configuration file?
				if((m_tmaxConverter == null) || (m_ctrlAlternateFile.Text.Length > 0))
				{
					//	Must have an alternate if no converter is selected
					if((m_tmaxConverter == null) && (m_ctrlAlternateFile.Text.Length == 0))
					{
						ShowWarning(ERROR_NO_ALTERNATE_CONVERTER, null, null);
						m_ctrlAlternateFile.Focus();
						break;
					}
					
					//	Make sure the alternate configuration file exist
					if(System.IO.File.Exists(m_ctrlAlternateFile.Text) == false)
					{
						ShowWarning(ERROR_ALTERNATE_NOT_FOUND, m_ctrlAlternateFile.Text, null);
						m_ctrlAlternateFile.Focus();
						m_ctrlAlternateFile.SelectAll();
						break;
					}
					
					//	Use the alternate as the configuration file
					strConfigFile = m_ctrlAlternateFile.Text;
					
					//	Set the cross reference (we'll verify existance a little later)
					strCrossReference = m_ctrlCrossReferenceFile.Text;
				
				}
				else
				{
					//	Build the path to the configuration file
					strConfigFile = CTmaxToolbox.GetApplicationFolder();
					if(strConfigFile.EndsWith("\\") == false)
						strConfigFile += "\\";
					strConfigFile += m_tmaxConverter.ConfigurationFilename;
					
					//	Make sure the  configuration file exist
					if(System.IO.File.Exists(strConfigFile) == false)
					{
						ShowWarning(ERROR_CONVERTER_FILE_NOT_FOUND, strConfigFile, null);
						break;
					}
				
					//	Get the cross reference selected by the user
					strCrossReference = m_ctrlCrossReferenceFile.Text;
					
					//	Does the active converter require a cross reference?
					if(m_tmaxConverter.UseCrossReference == true)
					{
						//	Make sure the user provided a cross reference file
						if(strCrossReference.Length == 0)
						{
							ShowWarning(ERROR_NO_CROSS_REFERENCE, null, null);
							m_ctrlCrossReferenceFile.Focus();
							break;
						}
					
					}// if(m_tmaxConverter.UseCrossReference == true)
					
				}// if((m_tmaxConverter == null) || (m_ctrlAlternateFile.Text.Length > 0))
				
				//	Make sure the  cross reference file exists
				if(strCrossReference.Length > 0)
				{
					if(System.IO.File.Exists(strCrossReference) == false)
					{
						ShowWarning(ERROR_CROSS_REFERENCE_NOT_FOUND, strCrossReference, null);
						m_ctrlCrossReferenceFile.Focus();
						m_ctrlCrossReferenceFile.SelectAll();
						break;
					}
				}
				
				//	Write the parameters to file
				if(WriteParameters(m_ctrlSourceFile.Text, strConfigFile, strCrossReference, m_ctrlRootFolder.Text) == false)
					break;
				
				bSuccessful = true;
				
			}
			
			//	Did the user provide all the required values?
			if(bSuccessful == true)
			{
				//	Update the collection properties
				m_tmaxWizardOptions.SourceFileSpec = m_ctrlSourceFile.Text.ToLower();
				m_tmaxWizardOptions.CrossReferenceFileSpec = m_ctrlCrossReferenceFile.Text.ToLower();
				m_tmaxWizardOptions.AlternateFileSpec = m_ctrlAlternateFile.Text.ToLower();
				m_tmaxWizardOptions.RootFolder = m_ctrlRootFolder.Text.ToLower();
				
				if(m_tmaxConverter != null)
					m_tmaxWizardOptions.LastUsed = m_tmaxConverter.Name;
				else
					m_tmaxWizardOptions.LastUsed = "";
			}
			
			return bSuccessful;
			
		}// private bool PrepareConversion()
		
		/// <summary>Retrieves the values specified by the user and prepares the transfer operation</summary>
		/// <returns>true if successful</returns>
		private bool PrepareTransfer()
		{
			bool bSuccessful = false;
			
			while(bSuccessful == false)
			{
				//	Make sure the transfer top level folder exists
				if(System.IO.Directory.Exists(m_strTransferFolder) == false)
				{
					//	Create the transfer folder
					try
					{
						System.IO.Directory.CreateDirectory(m_strTransferFolder);
					}
					catch
					{
						ShowWarning(ERROR_CREATE_TRANSFER_FOLDER, m_strTransferFolder, null);
						break;
					}

				}// if(System.IO.Directory.Exists(m_strTransferFolder) == false)
				
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			//	Did the user provide all the required values?
			if(bSuccessful == true)
			{
				//	Update the wizard options
				m_tmaxWizardOptions.MoveFiles = m_ctrlMoveFiles.Checked;
				m_tmaxWizardOptions.RenameFiles = m_ctrlRenameFiles.Checked;
			
				if(m_ctrlMaxDocsPerFolder.Text.Length > 0)
				{
					try { m_tmaxWizardOptions.MaxDocsPerFolder = System.Convert.ToInt64(m_ctrlMaxDocsPerFolder.Text); }
					catch { m_tmaxWizardOptions.MaxDocsPerFolder = 0; }
				}
				else
				{
					m_tmaxWizardOptions.MaxDocsPerFolder = 0;
				}

			}// if(bSuccessful == true)
			
			return bSuccessful;
			
		}// private bool PrepareTransfer()
		
		/// <summary>This method will create the parameters file for the conversion operation</summary>
		/// <param name="strSource">The path to the source media file</param>
		/// <param name="strConfiguration">The path to the configuration file</param>
		/// <param name="strCrossReference">The path to the cross reference file</param>
		/// <returns>True if successful</returns>
		private bool WriteParameters(string strSource, string strConfiguration, 
									 string strCrossReference, string strRootFolder)
		{
			System.IO.StreamWriter	parameters = null;
			bool					bSuccessful = false;
			
			//	Delete the existing parameters file
			if(System.IO.File.Exists(m_strParametersFileSpec) == true)
			{
				try { System.IO.File.Delete(m_strParametersFileSpec); }
				catch {}
			}
			
			try
			{
				parameters = System.IO.File.CreateText(m_strParametersFileSpec);
			}
			catch
			{
				ShowWarning(ERROR_CREATE_PARAMETERS_FILE, m_strParametersFileSpec, null);
				return false;
			}
			
			//	Write the parameters to file
			try
			{
				parameters.WriteLine("# Loadfile Conversion Command File\n");

				parameters.WriteLine("# Conversion configuration");
				parameters.WriteLine("CONFIG_FILE = " + strConfiguration + "\n");

				parameters.WriteLine("# Media Source File");
				parameters.WriteLine("SOURCE_FILE = " + strSource + "\n");

				parameters.WriteLine("# Cross Reference File");
				parameters.WriteLine("XREF_FILE = " + strCrossReference + "\n");

				parameters.WriteLine("# Output XML File");
				parameters.WriteLine("XML_FILE = " + m_strXmlFileSpec + "\n");

				parameters.WriteLine("# Output Intermediate Output File");
				parameters.WriteLine("TXT_FILE = " + m_strTextFileSpec + "\n");

				parameters.WriteLine("# Source Document Root path ");
				parameters.WriteLine("SRC_ROOT = " + strRootFolder + "\n");

				//	For testing purposes
				if(m_ctrlAddConvertError.Checked == true)
				{
					parameters.WriteLine("# Force creation of an error message ");
					parameters.WriteLine("ERROR = This was Phil's crazy idea - not mine\n");
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "WriteParameters", m_tmaxErrorBuilder.Message(ERROR_WRITE_PARAMETERS_EX, m_strParametersFileSpec), Ex);
			}
			finally
			{
				if(parameters != null)
				{
					parameters.Close();
					parameters = null;
				}
				
			}
			
			return bSuccessful;
		
		}// private bool WriteParameters(string strSource, string strConfiguration, string strCrossReference)
		
		/// <summary>This method is called to start the transfer operation in its own thread</summary>
		private void StartTransferThread()
		{
			try
			{
				Thread transfer = new Thread(new ThreadStart(this.TransferThreadProc));
				transfer.Start();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "StartTransferThread", m_tmaxErrorBuilder.Message(ERROR_START_TRANSFER_THREAD_EX), Ex);
				m_ctrlBack.Enabled = false;
			}
		
		}// private void StartTransferThread()
		
		/// <summary>This is the service procedure for the transfer thread</summary>
		private void TransferThreadProc()
		{
			//	Start the transfer operation
			//
			//	NOTE:	I could call Transfer() directly from StartTransferThread()
			//			but I want to have Transfer() return a bool just in case
			//			I decide not to run it in a thread
			Transfer();

			//	The operation is finished
			OnTransferComplete();
		}
		
		/// <summary>This method is called to transfer source files listed in the XML load file to the case folder</summary>
		private bool Transfer()
		{
			CTmaxSourceFolder	tmaxTransfer = null;
			CTmaxSourceFolder	tmaxPrimary = null;
			CTmaxSourceFolder	tmaxTarget = null;
			int					iTotalGroups = 0;
			int					iGroupIndex = 0;
			
			Debug.Assert(m_xmlLoadFile != null);
			Debug.Assert(m_xmlLoadFile.Primaries != null);
			Debug.Assert(m_xmlLoadFile.Primaries.Count > 0);
			if(m_xmlLoadFile == null) return false;
			if(m_xmlLoadFile.Primaries == null) return false;
			if(m_xmlLoadFile.Primaries.Count == 0) return false;
			
			//	Clear the existing source folder
			if(m_tmaxRegisterSource != null)
				m_tmaxRegisterSource = null;

			//	The top level transfer folder should already exist
			Debug.Assert(System.IO.Directory.Exists(m_strTransferFolder));
			if(System.IO.Directory.Exists(m_strTransferFolder) == false) 
			{
				AddTransferError("The transfer root folder does not exist: " + m_strTransferFolder, TmaxMessageLevels.FatalError);
				return false;
			}
			
			//	Allocate the folder where the transferred file specifications
			//	get stored
			tmaxTransfer = new CTmaxSourceFolder(m_strTransferFolder);
			
			//	Reset the local transfer flags
			m_bTransferCancelled = false;
			m_bTransferring = true;
			m_ctrlTransferProgressBar.Maximum = (int)m_lSecondaries;
			m_ctrlTransferProgressBar.Value = 0;
			
			//	Set the wait cursor for the operation
			if(m_waitArrow != null)
				Cursor.Current = m_waitArrow;
			else
				Cursor.Current = Cursors.WaitCursor;
				
			try
			{ 
				//	Create the document group folders
				if(CreateGroupFolders(tmaxTransfer) == true)
				{
					//	How many total document groups do we have?
					if((iTotalGroups = tmaxTransfer.SubFolders.Count) > 0)
					{
						iGroupIndex = 0;
						tmaxTarget = tmaxTransfer.SubFolders[0];
					}
					else
					{
						tmaxTarget = tmaxTransfer; // Use the top-level folder for all documents
					}
					
					//	Transfer the files associated with each primary
					foreach(CXmlPrimary O in m_xmlLoadFile.Primaries)
					{
						//	Check to see if the user has cancelled
						Application.DoEvents();
						if(m_bTransferCancelled == true)
							break;

						//	Should we switch target folders?
						if((iTotalGroups > 0) && ((iGroupIndex + 1) < iTotalGroups))
						{
							//	Is this group full?
							if(tmaxTarget.SubFolders.Count >= m_tmaxWizardOptions.MaxDocsPerFolder)
							{	
								iGroupIndex += 1; // Switch to the next group
								tmaxTarget = tmaxTransfer.SubFolders[iGroupIndex];
							}

						}// if((iTotalGroups > 0) && ((iGroupIndex + 1) < iTotalGroups))

						if((tmaxPrimary = Transfer(tmaxTarget, O)) != null)
							tmaxTarget.SubFolders.Add(tmaxPrimary);
					
					}// foreach(CXmlPrimary O in m_xmlLoadFile.Primaries)

				}// if(CreateGroupFolders(tmaxTransfer) == true)
				
				//	No longer transferring files
				m_bTransferring = false;
				
				//	Was the operation cancelled?
				if(m_bTransferCancelled == true)
				{
					tmaxTransfer = null;
				}
				else
				{
					// Did we transfer any primaries
					if(tmaxTransfer.SubFolders.Count == 0)
						tmaxTransfer = null;
				}
					
				//	Expose the collection for registration
				m_tmaxRegisterSource = tmaxTransfer;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Transfer", m_tmaxErrorBuilder.Message(ERROR_TRANSFER_ALL_EX), Ex);
			}
			
			Cursor.Current = Cursors.Default;
			
			//	Do we have anything to register?
			return (m_tmaxRegisterSource != null);
					
		}// private CTmaxSourceFolder Transfer()
		
		/// <summary>This method is called to transfer source files associated with the specified primary node</summary>
		/// <param name="tmaxParent">The parent folder where the document should be stored</param>
		/// <param name="xmlPrimary">The parent primary node</param>
		/// <returns>A source folder that represents the new storage location</returns>
		private CTmaxSourceFolder Transfer(CTmaxSourceFolder tmaxParent, CXmlPrimary xmlPrimary)
		{
			string				strFolder = "";
			CTmaxSourceFolder	tmaxSource = null;
			
			Debug.Assert(xmlPrimary != null);
			if(xmlPrimary == null) return null;
			Debug.Assert(tmaxParent != null);
			if(tmaxParent == null) return null;
			
			//	MUST have a media id
			if(xmlPrimary.MediaId == null || (xmlPrimary.MediaId.Length == 0))
			{
				AddTransferError("Unable to transfer primary files - no media id provided", TmaxMessageLevels.CriticalError);
				return null;
			}
			
			//	MUST have secondary children
			if(xmlPrimary.XmlSecondaries == null || (xmlPrimary.XmlSecondaries.Count == 0))
			{
				if(m_xmlLoadFile.GetSecondaries(xmlPrimary) == false)
				{
					AddTransferError("Unable to retrieve secondary XML nodes for primary: MediaId = " + xmlPrimary.MediaId, TmaxMessageLevels.CriticalError);
					return null;
				}
				else
				{
					//	Don't bother if no children
					if(xmlPrimary.XmlSecondaries.Count == 0) return null;
				}
				
			}
			
			//	Build the path to the folder where the secondaries should be transferred
			strFolder = tmaxParent.Path;
			if(strFolder.EndsWith("\\") == false)
				strFolder += "\\";
			strFolder += xmlPrimary.MediaId;
			
			//	The folder should not already exist
			if(System.IO.Directory.Exists(strFolder) == false)
			{
				//	Create the folder
				try
				{
					//	Check to see if the user has cancelled
					Application.DoEvents();
					if(m_bTransferCancelled == true)
						return null;
						
					System.IO.Directory.CreateDirectory(strFolder);

				}
				catch
				{
					AddTransferError("Unable to create transfer folder for " + xmlPrimary.MediaId + " : " + strFolder, TmaxMessageLevels.CriticalError);
					m_ctrlTransferProgressBar.IncrementValue(xmlPrimary.XmlSecondaries.Count);
					return null;
				}
				
			}// if(System.IO.Directory.Exists(strFolder) == false)
			
			SetTransferStatus("created " + strFolder);
					
			//	Allocate the source folder
			tmaxSource = new CTmaxSourceFolder(strFolder);
			tmaxSource.XmlPrimary = xmlPrimary;
			tmaxSource.Files.KeepSorted = false;
			
			//	Transfer each of the files
			foreach(CXmlSecondary O in xmlPrimary.XmlSecondaries)
			{
				//	Check to see if the user has cancelled
				Application.DoEvents();
				if(m_bTransferCancelled == true)
					break;
						
				Transfer(xmlPrimary, tmaxSource, O);
				
				//	Update the progress bar
				m_ctrlTransferProgressBar.PerformStep();
			}
			
			//	Were we able to transfer any files?
			if(tmaxSource.Files.Count == 0)
				tmaxSource = null;
				
			//	Flush the secondaries to prevent accumulating memory
			xmlPrimary.XmlSecondaries.Clear();
			
			return tmaxSource;
		
		}// private bool Transfer(CXmlPrimary xmlPrimary)
		
		/// <summary>This method is called to transfer the specified secondary source file</summary>
		/// <param name="xmlPrimary">The parent primary node</param>
		///	<param name="tmaxFolder">The source folder object where the file reference should be stored</param>
		/// <param name="xmlSecondary">The secondary node that owns the file</param>
		/// <returns>True if successful</returns>
		private bool Transfer(CXmlPrimary xmlPrimary, CTmaxSourceFolder tmaxFolder, CXmlSecondary xmlSecondary)
		{
			string			strTarget = "";
			string			strFolder = "";
			CTmaxSourceFile	tmaxFile = null;
			
			Debug.Assert(xmlPrimary != null);
			Debug.Assert(xmlSecondary != null);
			if(xmlPrimary == null) return false;
			if(xmlSecondary == null) return false;
			
			//	Make sure the path to the secondary source has been provided
			if((xmlSecondary.Path == null) || (xmlSecondary.Path.Length == 0))
			{
				AddTransferError("No path provided for " + xmlPrimary.MediaId + " - page " + xmlSecondary.Page.ToString(), TmaxMessageLevels.CriticalError);
				return false;
			}
			
			//	Make sure the source file exists
			if(System.IO.File.Exists(xmlSecondary.Path) == false)
			{
				//	Should we create the source file?
				//
				//	NOTE:	This is done to help us test various load formats
				if(m_ctrlCreateSource.Checked == true)
				{
					//	Does our bogus source file exist?
					if(System.IO.File.Exists(DEFAULT_CREATE_SOURCE_FILESPEC) == true)
					{
						try
						{
							if(System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(xmlSecondary.Path)) == false)
								System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(xmlSecondary.Path));
							System.IO.File.Copy(DEFAULT_CREATE_SOURCE_FILESPEC, xmlSecondary.Path, true);
						}
						catch
						{
						}
					}
					else
					{
						MessageBox.Show("Unable to locate " + DEFAULT_CREATE_SOURCE_FILESPEC + " to create source tree", "Error");
						m_ctrlCreateSource.Checked = false;
						return false;
					}
				
				}
				else
				{				
					AddTransferError(xmlPrimary.MediaId + "." + xmlSecondary.Page.ToString() + " file not found: " + xmlSecondary.Path, TmaxMessageLevels.CriticalError);
					return false;
				}
				
			}
			
			//	Build the target path
			strFolder = tmaxFolder.Path;
			if(strFolder.EndsWith("\\") == false)
				strFolder += "\\";
			if(m_tmaxWizardOptions.RenameFiles == true)
			{
				strTarget = String.Format("{0}{1:000000}{2}", strFolder, xmlSecondary.Page, System.IO.Path.GetExtension(xmlSecondary.Path));
			}
			else
			{
				strTarget = String.Format("{0}{1}", strFolder, System.IO.Path.GetFileName(xmlSecondary.Path));
			} 
			
			try
			{
				//	Are we moving the file?
				if(m_tmaxWizardOptions.MoveFiles == true)
				{
					System.IO.File.Move(xmlSecondary.Path, strTarget);
					SetTransferStatus("moved " + xmlSecondary.Path);
				}
				else
				{
					System.IO.File.Copy(xmlSecondary.Path, strTarget, true);
					SetTransferStatus("copied " + xmlSecondary.Path);
				}
				
				//	Add a new file object to the caller's folder
				tmaxFile = new CTmaxSourceFile(strTarget);
				tmaxFile.XmlSecondary = xmlSecondary;
				tmaxFolder.Files.Add(tmaxFile);

				return true;
			
			}
			catch
			{
				//	Are we moving the file?
				if(m_tmaxWizardOptions.MoveFiles == true)
				{
					AddTransferError("Unable to move " + xmlSecondary.Path + " to " + strTarget, TmaxMessageLevels.CriticalError);
				}
				else
				{
					AddTransferError("Unable to copy " + xmlSecondary.Path + " to " + strTarget, TmaxMessageLevels.CriticalError);
				}
				return false;
				
			}
		
		}// private bool Transfer(CXmlPrimary xmlPrimary, CXmlSecondary xmlSecondary)

		/// <summary>Called to create the group folders used to store the documents</summary>
		/// <param name="tmaxParent">The parent source folder</param>
		/// <returns>true if successful</returns>
		private bool CreateGroupFolders(CTmaxSourceFolder tmaxParent)
		{
			bool	bSuccessful = true;
			int		iGroups = 0;
			string	strFolderName = "";
			string	strFolderPath = "";

			Debug.Assert(tmaxParent != null);
			Debug.Assert(m_xmlLoadFile != null);
			Debug.Assert(m_xmlLoadFile.Primaries != null);
			if(tmaxParent == null) return false;
			if(m_xmlLoadFile == null) return false;
			if(m_xmlLoadFile.Primaries == null) return false;

			try
			{
				//	Shouldn't be any subfolders in the parent collection
				Debug.Assert(tmaxParent.SubFolders.Count == 0);
				if(tmaxParent.SubFolders.Count > 0)
					tmaxParent.SubFolders.Clear();
					
				//	How many group folders are we going to require?
				if(m_tmaxWizardOptions.MaxDocsPerFolder > 1)
				{
					iGroups = (int)(m_xmlLoadFile.Primaries.Count / m_tmaxWizardOptions.MaxDocsPerFolder);

					if(m_xmlLoadFile.Primaries.Count > (m_tmaxWizardOptions.MaxDocsPerFolder * iGroups))
						iGroups += 1;

				}// if(m_tmaxWizardOptions.MaxDocsPerFolder > 1)
				
				//	Do we need to create any subfolders?
				if(iGroups > 1)
				{
					//	Create the group subfolders
					for(int i = 0; i < iGroups; i++)
					{
						//	Build the path to the group folder
						strFolderName = String.Format("{0:00000000}", i * m_tmaxWizardOptions.MaxDocsPerFolder);
						
						strFolderPath = tmaxParent.Path;
						if(strFolderPath.EndsWith("\\") == false)
							strFolderPath += "\\";
						strFolderPath += strFolderName;
						
						//	Make sure the folder exists
						if(System.IO.Directory.Exists(strFolderPath) == false)
						{
							//	Create the folder
							try
							{
								System.IO.Directory.CreateDirectory(strFolderPath);
							}
							catch
							{
								AddTransferError("Unable to create document group folder: " + strFolderPath, TmaxMessageLevels.FatalError);
								bSuccessful = false;
								break;
							}

						}// if(System.IO.Directory.Exists(strFolderPath) == false)
						
						//	Add a descriptor for this folder to the parent collection
						tmaxParent.SubFolders.Add(new CTmaxSourceFolder(strFolderPath));

					}// for(int i = 0; i < iGroups; i++)

				}// if(iGroups > 1)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateGroupFolders", m_tmaxErrorBuilder.Message(ERROR_CREATE_GROUP_FOLDERS_EX), Ex);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool CreateGroupFolders(CTmaxSourceFolder tmaxTargetFolder)
		
		/// <summary>This method is called to perform the conversion</summary>
		/// <returns>True if successful</returns>
		private bool Convert()
		{
			bool bSuccessful = false;
			
			try
			{
				System.Diagnostics.Process process = new Process();
				
				process.StartInfo.FileName = m_strConverterExe;
				process.StartInfo.Arguments = (" \"" + m_strParametersFileSpec + "\"");
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

				if(process.Start() == false)
				{
					MessageBox.Show(m_tmaxErrorBuilder.Message(ERROR_CONVERT_LAUNCH_FAIL, m_strConverterExe), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				else
				{
					Cursor.Current = Cursors.WaitCursor;
					
					//	Wait for the process to complete
					process.WaitForExit();
					
					Cursor.Current = Cursors.Default;
					
					//	Was the conversion successful?
					if(process.ExitCode == 0)
					{
						bSuccessful = true;
					}
					else
					{
						MessageBox.Show(m_tmaxErrorBuilder.Message(ERROR_CONVERT_NO_XML, m_strXmlFileSpec), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					}
					
					//	Close the process
					process.Close();
				
				}// if(process.Start() == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Convert", m_tmaxErrorBuilder.Message(ERROR_CONVERT_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Convert()
		
		//	This method is called when the user wants to register the media
		private void Register()
		{
			//	This should not happen but just in case...
			if((m_tmaxRegisterSource == null) || (m_tmaxRegisterSource.GetFileCount(true) == 0))
			{
				ShowWarning(ERROR_NO_REGISTRATION_SOURCE, null, null);
				return;
			}
			
			//	Get confirmation from the user if there were transfer errors
			if(m_ctrlTransferErrors.Count > 0)
			{
				string strPrompt = String.Format("There are {0} transfer errors. Are you sure you want to register now?", m_ctrlTransferErrors.Count);
				if(MessageBox.Show(strPrompt, "Register", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
			}
			
			//	Copy the conversion results to the root folder in case we need
			//	them later on for some reason
			try { System.IO.File.Copy(m_strXmlFileSpec, m_strTransferFolder + CONVERT_XML_RESULTS_FILENAME, true); }
			catch{};
			try { System.IO.File.Copy(m_strTextFileSpec, m_strTransferFolder + CONVERT_TEXT_RESULTS_FILENAME, true); }
			catch{};
			
			//	Make sure the folder is the anchor for the operation
			m_tmaxRegisterSource.Anchor = true;
			
			//	Close the form
			DialogResult = DialogResult.OK;
			this.Close();
			
		}// private void Register()
		
		/// <summary>Handles the event fired when the user clicks on the Browse for root folder button</summary>
		/// <param name="sender">The button firing the event</param>
		/// <param name="e">System event arguements</param>
		private void OnClickBrowseRootFolder(object sender, System.EventArgs e)
		{
			FTI.Shared.CBrowseForFolder bff = new CBrowseForFolder();
			
			bff.Folder = m_ctrlRootFolder.Text;
			bff.Prompt = "Select root folder : ";
			bff.NoNewFolder = false;
			
			if(bff.ShowDialog(this.Handle) == DialogResult.OK)
			{
				m_ctrlRootFolder.Text = bff.Folder.ToLower();
			}
			
		}// private void OnClickBrowseRootFolder(object sender, System.EventArgs e)
		
		/// <summary>This method will build the path to the transfer root folder</summary>
		private void SetTransferFolder()
		{
			string strSource = "";
			
			//	Get the root document folder
			m_strTransferFolder = m_tmaxDatabase.GetCasePath(TmaxMediaTypes.Document);
			
			if(m_strTransferFolder.Length > 0)
			{
				if(m_strTransferFolder.EndsWith("\\") == false)
				{
					m_strTransferFolder += "\\";
				}
			}
			
			//	Use the source filename to create the top level folder
			strSource = System.IO.Path.GetFileNameWithoutExtension(m_tmaxWizardOptions.SourceFileSpec);
			m_strTransferFolder += strSource;
			
			//	Make sure it does not already exist
			if(System.IO.Directory.Exists(m_strTransferFolder) == true)
			{
				//	Start incrementing until we get a unique folder name
				for(int i = 1; i < 10000; i++)
				{
					if(System.IO.Directory.Exists(m_strTransferFolder + "_" + i.ToString()) == false)
					{
						m_strTransferFolder += ("_" + i.ToString());
						break;
					}
				}
				
			}// if(System.IO.Directory.Exists(m_strTransferFolder) == true)
		
			if(m_strTransferFolder.EndsWith("\\") == false)
			{
				m_strTransferFolder += "\\";
			}

		}// private void SetTransferFolder()
		
		/// <summary>This method constructs the paths to the conversion files</summary>
		/// <returns>true if successful</returns>
		private bool SetConversionFileSpecs()
		{
			string strFolder = "";
			
			//	Assume all defaults go in the app folder
			strFolder = CTmaxToolbox.GetApplicationFolder();
			if(strFolder.EndsWith("\\") == false)
				strFolder += "\\";

			//	Build the path to the converter utility
			m_strConverterExe = (strFolder + CONVERT_EXECUTABLE_FILENAME);
			
			//	Make sure the converter exists
			if(System.IO.File.Exists(m_strConverterExe) == false)
			{
				ShowWarning(ERROR_CONVERTER_EXE_NOT_FOUND, m_strConverterExe, null);
				return false;
			}
			
			//	Build the default paths
			m_strParametersFileSpec = (strFolder + CONVERT_PARAMETERS_FILENAME);
			m_strXmlFileSpec = (strFolder + CONVERT_XML_RESULTS_FILENAME);
			m_strTextFileSpec = (strFolder + CONVERT_TEXT_RESULTS_FILENAME);
	
			return true;
		
		}// private bool SetConversionFileSpecs()
		
		/// <summary>Handles the event fired when the user clicks on the Preview XML button</summary>
		/// <param name="sender">The button firing the event</param>
		/// <param name="e">System event arguements</param>
		private void OnClickPreviewXml(object sender, System.EventArgs e)
		{
			try
			{
				FTI.Shared.Win32.Shell.ShellExecute(this.Handle, 
													"open",
													m_strXmlFileSpec,
													"",
													"",
													FTI.Shared.Win32.User.SW_SHOWDEFAULT);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickPreviewXml", m_tmaxErrorBuilder.Message(ERROR_PREVIEW_XML_EX, m_strXmlFileSpec), Ex);
			}
												
		}// private void OnClickPreviewXml(object sender, System.EventArgs e)

		/// <summary>Handles the event fired when the user clicks on the Preview Text button</summary>
		/// <param name="sender">The button firing the event</param>
		/// <param name="e">System event arguements</param>
		private void OnClickPreviewText(object sender, System.EventArgs e)
		{
			try
			{
				FTI.Shared.Win32.Shell.ShellExecute(this.Handle, 
					"open",
					m_strTextFileSpec,
					"",
					"",
					FTI.Shared.Win32.User.SW_SHOWDEFAULT);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickPreviewText", m_tmaxErrorBuilder.Message(ERROR_PREVIEW_XML_EX, m_strTextFileSpec), Ex);
			}
												
		}// private void OnClickPreviewText(object sender, System.EventArgs e)

		/// <summary>This method is called to open the load file used to transfer the source media</summary>
		/// <returns>True if successful</returns>
		private bool OpenLoadFile()
		{
			bool bSuccessful = false;

			//	Get rid of the existing load file
			if(m_xmlLoadFile != null)
			{
				m_xmlLoadFile.Close(true);
				m_xmlLoadFile = null;
			}
			
			m_lSecondaries = 0;
			
//m_tmaxEventSource.InitElapsed();	
		
			try
			{
				m_xmlLoadFile = new CXmlLoadFile();
				m_tmaxEventSource.Attach(m_xmlLoadFile.EventSource);
				
//				if(m_xmlLoadFile.Open(m_strXmlFileSpec, false) == true)
//				{				
//					//	Get the primary nodes
//					if(m_xmlLoadFile.GetPrimaries(false) == true)
//					{
//						//	How many secondary files to be transferred?
//						foreach(CXmlPrimary O in m_xmlLoadFile.Primaries)
//							m_lSecondaries += O.Children;
//							
//						//	Get the errors
//						m_xmlLoadFile.GetErrors();
//						
//						bSuccessful = true;
//						
//					}// if(m_xmlLoadFile.GetPrimaries(false) == true)
//					
//				}// if(m_xmlLoadFile.Open(m_strFileSpec, false) == true)
							
				if(m_xmlLoadFile.OpenXmlReader(m_strXmlFileSpec, true, false, true) == true)
				{				
					//	How many secondary files to be transferred?
					foreach(CXmlPrimary O in m_xmlLoadFile.Primaries)
						m_lSecondaries += O.Children;
							
					bSuccessful = true;
						
				}// if(m_xmlLoadFile.OpenXmlReader(m_strXmlFileSpec, true, false, true, true) == true)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenLoadFile", m_tmaxErrorBuilder.Message(ERROR_OPEN_LOAD_FILE_EX, m_strXmlFileSpec), Ex);
			}

//m_tmaxEventSource.FireElapsed(this, "OpenLoadFile", "time to open load file");
			
			return bSuccessful;
				
		}// private bool OpenLoadFile()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Persistant import wizard options</summary>
		public FTI.Shared.Trialmax.CTmaxImportWizard WizardOptions
		{
			get { return m_tmaxWizardOptions; }
			set { m_tmaxWizardOptions = value; }
		}
		
		/// <summary>TrialMax source registration options</summary>
		public CTmaxRegOptions RegisterOptions
		{
			get { return m_tmaxRegisterOptions; }
			set { m_tmaxRegisterOptions = value; }
		}
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		/// <summary>The collection of source files to be registered</summary>
		public FTI.Shared.Trialmax.CTmaxSourceFolder RegisterSource
		{
			get { return m_tmaxRegisterSource; }
		}
		
		#endregion Properties

	}// public class CFImportWizard : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
