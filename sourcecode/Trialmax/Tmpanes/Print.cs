using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;


namespace FTI.Trialmax.Panes
{
    /// <summary>This form allow the user to print media records</summary>
    public class CFPrint : FTI.Trialmax.Forms.CFTmaxBaseForm
    {
        #region Constants

        //	Template field bitmask identifiers
        private const int TEMPLATE_BITMASK_NAME = 0x00000001;
        private const int TEMPLATE_BITMASK_BARCODE = 0x00000002;
        private const int TEMPLATE_BITMASK_GRAPHIC = 0x00000004;
        private const int TEMPLATE_BITMASK_FILENAME = 0x00000008;
        private const int TEMPLATE_BITMASK_PAGENUM = 0x00000010;
        private const int TEMPLATE_BITMASK_DEPONENT = 0x00000020;
        private const int TEMPLATE_BITMASK_IMAGE = 0x00000040;
        private const int TEMPLATE_BITMASK_CELL_BORDER = 0x00000080;
        private const int TEMPLATE_BITMASK_AUTO_ROTATE = 0x00000100;
        private const int TEMPLATE_BITMASK_FOREIGN_BARCODE = 0x00000200;
        private const int TEMPLATE_BITMASK_SOURCE_BARCODE = 0x00000400;

        //	Split screen treatment flag bitmask identifiers
        private const int SPLITSCREEN_BITFLAG_RIGHT = 0x00000001;
        private const int SPLITSCREEN_BITFLAG_HORIZONTAL = 0x00000002;

        private const int ERROR_FILL_PRINTERS_EX = (ERROR_TMAX_FORM_MAX + 1);
        private const int ERROR_FILL_TEXT_FIELDS_EX = (ERROR_TMAX_FORM_MAX + 2);
        private const int ERROR_FILL_SUBSTITUTIONS_EX = (ERROR_TMAX_FORM_MAX + 3);

        private const string FIELD_SUBSTITUTE_DATE = "<CD>";
        private const string FIELD_SUBSTITUTE_TIME = "<CT>";
        private const string FIELD_SUBSTITUTE_DATE_TIME = "<DT>";

        #endregion Constants

        #region Private Members

        /// <summary>Required designer variable</summary>
        private System.ComponentModel.Container components = null;

        /// <summary>Button to close the form</summary>
        private System.Windows.Forms.Button m_ctrlDone;

        /// <summary>Button to request printing</summary>
        private System.Windows.Forms.Button m_ctrlPrint;

        /// <summary>Group containing print range controls</summary>
        private System.Windows.Forms.GroupBox m_ctrlRangeGroup;

        /// <summary>Radio button to request printing of current selections</summary>
        private System.Windows.Forms.RadioButton m_ctrlPrintSelections;

        /// <summary>Radio button to request printing of specific barcodes</summary>
        private System.Windows.Forms.RadioButton m_ctrlPrintBarcodes;

        /// <summary>Barcodes to be printed</summary>
        private System.Windows.Forms.TextBox m_ctrlBarcodes;

        /// <summary>Include treatments in print job</summary>
        private System.Windows.Forms.CheckBox m_ctrlIncludeTreatments;

        /// <summary>Only treaments in print job</summary>
        private System.Windows.Forms.CheckBox m_ctrlOnlyTreatments;

        /// <summary>Only first page of each document in print job</summary>
        private CheckBox m_ctrlOnlyFirstPage;

        /// <summary>Only document links in print job</summary>
        private System.Windows.Forms.CheckBox m_ctrlOnlyLinks;

        /// <summary>Include document links in print job</summary>
        private CheckBox m_ctrlIncludeLinks;

        /// <summary>Include subbinders in print job</summary>
        private System.Windows.Forms.CheckBox m_ctrlIncludeSubBinders;

        /// <summary>Local member bound to Items property</summary>
        private FTI.Shared.Trialmax.CTmaxItems m_tmaxItems = null;

        /// <summary>Local member bound to Database property</summary>
        private FTI.Trialmax.Database.CTmaxCaseDatabase m_tmaxDatabase = null;

        /// <summary>Label for Copies text entry control</summary>
        private System.Windows.Forms.Label m_ctrlCopiesLabel;

        /// <summary>Text box to enter number of copies being printed</summary>
        private System.Windows.Forms.TextBox m_ctrlCopies;

        /// <summary>Check box to set the Collate option</summary>
        private System.Windows.Forms.CheckBox m_ctrlCollate;

        /// <summary>Local member bound to PrintOptions property</summary>
        private FTI.Shared.Trialmax.CTmaxPrintOptions m_tmaxPrintOptions = null;

        /// <summary>Local member bound to PresentationOptions property</summary>
        private FTI.Shared.Trialmax.CTmaxPresentationOptions m_tmaxPresentationOptions = null;

        /// <summary>List box of available printers</summary>
        private System.Windows.Forms.ComboBox m_ctrlPrinters;

        /// <summary>List box of available templates</summary>
        private System.Windows.Forms.ComboBox m_ctrlTemplates;

        /// <summary>Print full file path check box</summary>
        private System.Windows.Forms.CheckBox m_ctrlPrintPath;

        /// <summary>Print treatment callouts check box</summary>
        private System.Windows.Forms.CheckBox m_ctrlPrintCallouts;

        /// <summary>Print page total in page number field check box</summary>
        private System.Windows.Forms.CheckBox m_ctrlPrintPageTotal;

        /// <summary>Force new page on each document check box</summary>
        private System.Windows.Forms.CheckBox m_ctrlForceNewPage;

        /// <summary>Printer is configured for double sided printing</summary>
        private System.Windows.Forms.CheckBox m_ctrlDoubleSided;

        /// <summary>Group box for printer setup options</summary>
        private System.Windows.Forms.GroupBox m_ctrlPrinterGroup;

        /// <summary>Group box for print job options</summary>
        private System.Windows.Forms.GroupBox m_ctrlOptionalGroup;

        /// <summary>Group box for print job content options</summary>
        private GroupBox m_ctrlContentGroup;

        /// <summary>Check box to request printing of callout borders</summary>
        private System.Windows.Forms.CheckBox m_ctrlPrintCalloutBorders;

        /// <summary>Group box for template setup controls</summary>
        private System.Windows.Forms.GroupBox m_ctrlTemplateGroup;

        /// <summary>Check box to select barcode text field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateText;

        /// <summary>Check box to select page number field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplatePageNumber;

        /// <summary>Check box to select cell border field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateBorder;

        /// <summary>Check box to select media image field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateImage;

        /// <summary>Check box to select barcode graphic field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateGraphic;

        /// <summary>Check box to select filename field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateFilename;

        /// <summary>Check box to select deponent field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateDeponent;

        /// <summary>Check box to select name field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateName;

        /// <summary>Check box to select foreign barcode field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateForeign;

        /// <summary>Check box to select source barcode field</summary>
        private System.Windows.Forms.CheckBox m_ctrlTemplateSource;

        /// <summary>Check box to select auto rotation option</summary>
        private System.Windows.Forms.CheckBox m_ctrlAutoRotate;

        /// <summary>Check box to select Slip Sheet option</summary>
        private CheckBox m_ctrlInsertSlipSheet;

        /// <summary>List of available field substitutions</summary>
        private System.Windows.Forms.ComboBox m_ctrlFieldSubstitutions;

        /// <summary>Text box to edit a field substitution</summary>
        private System.Windows.Forms.TextBox m_ctrlFieldText;

        /// <summary>Label for field substitution text box</summary>
        private System.Windows.Forms.Label m_ctrlSubstitutionsLabel;

        /// <summary>Barcodes example line 1</summary>
        private System.Windows.Forms.Label m_ctrlBarcodeExample2;

        /// <summary>Local member bound to Template filename property</summary>
        private string m_strTemplateFilename = "fti.ini";

        /// <summary>Control to display the current selections</summary>
        private System.Windows.Forms.Label m_ctrlSelections;

        /// <summary>Button to open printer properties dialog</summary>
        private System.Windows.Forms.Button m_ctrlPrinterProperties;

        /// <summary>Flag to indicate that the form has been initialized</summary>
        private bool m_bInitialized = false;

        /// <summary>Local member bound to store records to be printed</summary>
        private CDxMediaRecords m_dxRecords = new CDxMediaRecords();

        /// <summary>TMPrint ActiveX control used to implement printing operation</summary>
        private AxTM_PRINT6Lib.AxTMPrint6 m_tmxPrint = null;

        /// <summary>Check box to display text fields in the active template</summary>
        private System.Windows.Forms.CheckedListBox m_ctrlTextFieldsList;

        /// <summary>Label for text fields list box</summary>
        private System.Windows.Forms.Label m_ctrlTextFieldsLabel;

        /// <summary>Label for field text edit box</summary>
        private System.Windows.Forms.Label m_ctrlFieldTextLabel;

        /// <summary>Class member used to assign a name to the print job</summary>
        private string m_strJobName = "";

        /// <summary>Class member used to keep track of the current text field selection</summary>
        private CTmaxPrintField m_tmaxTextField = null;

        /// <summary>Class member used to keep track of the initial set of text fields</summary>
        private CTmaxPrintFields m_tmaxInitialTextFields = null;

        /// <summary>Class member used to group Media on the basis of BinderEntryid</summary>
        private MediaPrintJobs m_MediaPrintJobs = null;

        #endregion Private Members

        #region Public Members

        /// <summary>Constructor</summary>
        public CFPrint()
        {
            // Initialize the child controls
            InitializeComponent();

        }// public CFPrint()

        /// <summary>This method is called once to initialize the form</summary>
        /// <returns>true if successful</returns>
        public bool Initialize()
        {
            Debug.Assert(m_tmaxPrintOptions != null);
            if (m_tmaxPrintOptions == null) return false;

            Debug.Assert(m_tmxPrint != null);
            if (m_tmxPrint == null) return false;

            //	Initialize the TMPrint control
            m_bInitialized = InitAxPrinter();

            return m_bInitialized;

        }// public bool Initialize()

        #endregion Public Methods

        #region Protected Methods

        /// <summary>Clean up any resources being used</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>The method is called when the form gets loaded</summary>
        /// <param name="e">The system event arguments</param>
        protected override void OnLoad(System.EventArgs e)
        {
            string strSelections = "";
            bool bEnable = false;

            //	Do the base class processing
            base.OnLoad(e);

            //	Clear out previous values
            m_ctrlBarcodes.Text = "";

            //	Initalize the control values
            if ((m_tmaxPrintOptions != null) && (m_bInitialized == true))
            {
                //	Use the print options to initialize the controls
                ExchangeOptions(true);

                //	Do we have a media collection available for printing?
                if ((m_tmaxItems != null) && (m_tmaxItems.Count > 0))
                {
                    m_ctrlPrintSelections.Checked = true;
                    m_ctrlPrintSelections.Enabled = true;
                    m_ctrlPrintBarcodes.Checked = false;

                    //	Build the string to represent the current selections
                    foreach (CTmaxItem O in m_tmaxItems)
                    {
                        if (O.GetMediaRecord() != null)
                        {
                            if (strSelections.Length > 0)
                                strSelections += ", ";
                            strSelections += O.GetMediaRecord().GetBarcode(false);
                        }
                        else if (O.IBinderEntry != null)
                        {
                            if (strSelections.Length > 0)
                                strSelections += ", ";
                            strSelections += O.IBinderEntry.GetName();
                        }
                        else if (O.MediaType != TmaxMediaTypes.Unknown)
                        {
                            if (strSelections.Length > 0)
                                strSelections += ", ";
                            strSelections += O.MediaType.ToString();
                            if (strSelections.EndsWith("s") == false)
                                strSelections += "s";
                        }

                    }

                    //	Set the selections text
                    m_ctrlSelections.Text = CTmaxToolbox.FitPathToWidth(strSelections, m_ctrlSelections);

                }
                else
                {
                    m_ctrlPrintSelections.Checked = false;
                    m_ctrlPrintSelections.Enabled = false;
                    m_ctrlPrintBarcodes.Checked = true;
                    m_ctrlSelections.Text = "";
                }

                //	Fill the combolist of available text field substitutions
                FillFieldSubstitutions();

                //	Get the list of printers
                bEnable = FillPrinters();
            }

            //	Disable all if we failed to initialize TMPrint
            if (bEnable == false)
            {
                m_ctrlPrint.Enabled = false;
                m_ctrlPrintSelections.Enabled = false;
                m_ctrlPrintBarcodes.Enabled = false;
                m_ctrlBarcodes.Enabled = false;
            }
            else
            {
                m_ctrlPrint.Enabled = true;
                m_ctrlPrintBarcodes.Enabled = true;
                m_ctrlBarcodes.Enabled = true;
                m_ctrlBarcodes.Focus();
            }

        }// protected override void OnLoad(System.EventArgs e)

        /// <summary>This method is called to populate the error builder's format string collection</summary>
        protected override void SetErrorStrings()
        {
            if (m_tmaxErrorBuilder == null) return;
            if (m_tmaxErrorBuilder.FormatStrings == null) return;

            //	Let the base class add its strings first
            base.SetErrorStrings();

            //	Add our custom strings
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while filling the list of available printers");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while filling the list of available text fields");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while filling the list of available text field substitutions");

        }// protected override void SetErrorStrings()

        #endregion Protected Methods

        #region Private Methods

        /// <summary>This method is called to print the user selections</summary>
        /// <returns>true if successful</returns>
        private bool PrintSelections()
        {
            Debug.Assert(m_tmaxItems != null);
            Debug.Assert(m_tmaxItems.Count > 0);
            if (m_tmaxItems == null) return false;
            if (m_tmaxItems.Count == 0) return false;

            //	Build the list of records to be printed
            if (QueueSelections() == false) return false;

            //	Do we have anything to print?
            if (m_dxRecords.Count == 0)
            {
                MessageBox.Show("Unable to locate any records for printing among the current selections", "",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }

            //	Print the records in the queue
            return PrintQueue();

        }// private void PrintSelections()

        /// <summary>This method is called to print the user defined barcodes</summary>
        /// <returns>true if successful</returns>
        private bool PrintBarcodes()
        {
            //	Build the list of records to be printed
            if (QueueBarcodes() == false) return false;

            //	Do we have anything to print?
            if (m_dxRecords.Count == 0)
            {
                MessageBox.Show("Unable to locate any records for printing", "",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //	Print the records in the queue
            return PrintQueue();

        }// private void PrintBarcodes()

        /// <summary>This method is called to print the contents of the record queue</summary>
        /// <returns>true if successful</returns>
        private bool PrintQueue()
        {
            try
            {
                string strAxString = "";
                string strMediaId = "";
                ArrayList aJobs = null;
                CPrintJob aJob = null;
                bool bSuccessful = true;
                Debug.Assert(m_dxRecords != null);
                Debug.Assert(m_dxRecords.Count > 0);
                if (m_dxRecords == null) return false;
                if (m_dxRecords.Count == 0) return false;

                //	Are we doing double sided printing?
                if (m_tmaxPrintOptions.DoubleSided == true)
                {
                    //	We need to organize the queue into a collection
                    //	of jobs where there is one job for each new primary record
                    aJobs = new ArrayList();

                    if (m_MediaPrintJobs != null && m_MediaPrintJobs.Count > 0)
                        AddBinderPrintJob(aJobs, aJob);
                    else
                        AddMediaPrintJob(aJobs, aJob);


                    //	Now print each job
                    for (int i = 1; i <= m_tmaxPrintOptions.Copies; i++)
                    {
                        foreach (CPrintJob J in aJobs)
                        {
                            //	Clear the print control's queue
                            m_tmxPrint.Clear();
                            m_tmxPrint.JobName = J.Name;

                            //	Add each string to the control's queue
                            foreach (string AxString in J)
                            {
                                m_tmxPrint.Add(AxString);
                            }

                            //	Print this job
                            if (m_tmxPrint.Print() != 0)
                                bSuccessful = false;

                        }

                    } // for(int i = 1; i <= m_tmaxPrintOptions.Copies; i++)

                }
                else
                {
                    //	Clear the existing queue
                    m_tmxPrint.Clear();
                    m_tmxPrint.JobName = m_strJobName;

                    //	Add each record to the TMPrint queue
                    foreach (CDxMediaRecord O in m_dxRecords)
                    {
                        strAxString = GetAxString(O);

                        if (strAxString.Length > 0)
                        {
                            m_tmxPrint.Add(strAxString);
                        }

                    } // foreach(CDxMediaRecord O in m_dxRecords)

                    bSuccessful = (m_tmxPrint.Print() == 0);
                }

                return bSuccessful;
            }
            catch (System.Exception Ex)
            {
                m_tmxPrint.Clear();
                m_tmaxEventSource.FireDiagnostic(this, "Print", Ex);
                return false;
            }
            finally
            {
                m_tmxPrint.Clear();
                m_MediaPrintJobs = null;
            }
        }

        /// <summary> Add Media PrintJob in aJobs List, in the case of binder </summary>
        private void AddBinderPrintJob(ArrayList aJobs, CPrintJob aJob)
        {
            string strAxString;
            if (m_MediaPrintJobs == null) return;
            m_MediaPrintJobs.Sort(new CustomComparer());
            foreach (MediaPrintJob mediaPrintJob in m_MediaPrintJobs)
            {
                if (mediaPrintJob.CBaseRecords != null)
                {
                    aJob = new CFPrint.CPrintJob();
                    //aJob.Name =  O.GetMediaId();
                    aJobs.Add(aJob);
                    foreach (CDxMediaRecord O in mediaPrintJob.CBaseRecords)
                    {


                        //	Get the print string for this record and add it to the job
                        strAxString = GetAxString(O);
                        if (strAxString.Length > 0)
                            aJob.Add(strAxString);
                    }
                }
            }
        }

        /// <summary> Add Media PrintJob in aJobs List, when print call raises from media/Filed tree </summary>
        private void AddMediaPrintJob(ArrayList aJobs, CPrintJob aJob)
        {
            string strMediaId = string.Empty;
            string strAxString = string.Empty;
            foreach (CDxMediaRecord O in m_dxRecords)
            {
                //	Do we need to create a new job?
                if ((aJob == null) || (String.Compare(O.GetMediaId(), strMediaId) != 0))
                {
                    aJob = new CFPrint.CPrintJob();
                    aJob.Name = O.GetMediaId();
                    aJobs.Add(aJob);
                    strMediaId = O.GetMediaId();
                }

                //	Get the print string for this record and add it to the job
                strAxString = GetAxString(O);
                if (strAxString.Length > 0)
                    aJob.Add(strAxString);

            }// foreach(CDxMediaRecord O in m_dxRecords)
        }


        /// <summary>This method is called to populate the record queue using the current selections</summary>
        /// <returns>True to continue with print job</returns>
        private bool QueueSelections()
        {
            bool bContinue = true;

            Debug.Assert(m_tmaxItems != null);
            Debug.Assert(m_tmaxItems.Count > 0);
            if (m_tmaxItems == null) return false;
            if (m_tmaxItems.Count == 0) return false;

            //	Flush the existing records
            m_dxRecords.Clear();

            //	Asssign a job name if only one record selected by the user
            m_strJobName = "";
            if (m_tmaxItems.Count == 1)
            {
                //	Use the barcode as the default job name if this is a media record
                if (m_tmaxItems[0].GetMediaRecord() != null)
                    m_strJobName = m_tmaxItems[0].GetMediaRecord().GetBarcode(false);
                else if (m_tmaxItems[0].IBinderEntry != null)
                    m_strJobName = m_tmaxItems[0].IBinderEntry.GetName();
            }

            //	Iterate the item collection and queue each associated record
            foreach (CTmaxItem O in m_tmaxItems)
            {
                //	Is this a media record?
                if (O.IPrimary != null)
                    bContinue = AddToQueue((CDxMediaRecord)(O.GetMediaRecord()), null);
                else if (O.IBinderEntry != null)
                {
                    m_MediaPrintJobs = new MediaPrintJobs();
                    bContinue = AddToQueue((CDxBinderEntry)(O.IBinderEntry));
                }
                //	Should we stop here?
                if (bContinue == false)
                    break;

            }// foreach(CTmaxItem O in m_tmaxItems)

            return bContinue;

        }// private bool QueueSelections()

        /// <summary>This method is called to populate the barcodes specified by the user</summary>
        /// <returns>True to continue with print job</returns>
        private bool QueueBarcodes()
        {
            bool bContinue = true;
            ArrayList aInvalid = new ArrayList();
            CDxMediaRecords dxBarcodes = new CDxMediaRecords();
            string strMsg = "";

            Debug.Assert(m_ctrlBarcodes != null);
            Debug.Assert(m_ctrlBarcodes.IsDisposed == false);
            Debug.Assert(m_ctrlBarcodes.Text != null);
            if (m_ctrlBarcodes == null) return false;
            if (m_ctrlBarcodes.IsDisposed == true) return false;
            if (m_ctrlBarcodes.Text == null) return false;

            //	Flush the existing queue
            m_dxRecords.Clear();

            //	Make sure the user has specified a barcode string
            if (m_ctrlBarcodes.Text.Length == 0)
            {
                MessageBox.Show("You must specify the barcodes to be printed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                m_ctrlBarcodes.Focus();
                return false;
            }

            //	Let the database parse the string and retrieve the records
            m_tmaxDatabase.GetRecordsFromBarcodes(m_ctrlBarcodes.Text, dxBarcodes, aInvalid, ",;", true);

            //	Were there any errors?
            foreach (string O in aInvalid)
            {
                strMsg = String.Format("Unable to locate the record for this barcode: {0}\n\nDo you want to continue with the print job?", O);

                if (MessageBox.Show(strMsg, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }

            //	Use the barcode of the record as the default job
            //	name if only one record being printed
            if (dxBarcodes.Count == 1)
                m_strJobName = dxBarcodes[0].GetBarcode(false);
            else
                m_strJobName = "";

            //	Iterate the collection of records returned by the database
            foreach (CDxMediaRecord O in dxBarcodes)
            {
                if ((bContinue = AddToQueue(O, null)) == false)
                    break;

            }// foreach(CDxMediaRecord O in dxBarcodes)

            return bContinue;

        }// private bool QueueBarcodes()

        /// <summary>This method is called to queue the specified media record</summary>
        /// <returns>True to continue with print job</returns>
        private bool AddToQueue(CDxMediaRecord dxMedia, MediaPrintJob mediaPrintJob)
        {
            CDxPrimary dxPrimary = null;
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;

            Debug.Assert(dxMedia != null);
            Debug.Assert(dxMedia.MediaType != TmaxMediaTypes.Unknown);
            if (dxMedia == null) return true;
            if (dxMedia.MediaType == TmaxMediaTypes.Unknown) return true;

            //	What type of media is this?
            switch (dxMedia.MediaType)
            {
                case TmaxMediaTypes.Document:
                case TmaxMediaTypes.Powerpoint:
                case TmaxMediaTypes.Recording:
                case TmaxMediaTypes.Script:

                    //	This is a primary record
                    dxPrimary = (CDxPrimary)dxMedia;

                    //	Make sure the child collection is populated
                    if ((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
                        dxPrimary.Fill();

                    //	Do we have at least one record?
                    if ((dxPrimary.Secondaries != null) && (dxPrimary.Secondaries.Count > 0))
                    {
                        //	Should we only be adding the first page?
                        if ((dxPrimary.MediaType == TmaxMediaTypes.Document) && (m_tmaxPrintOptions.OnlyFirstPage == true))
                        {
                            AddToQueue(dxPrimary.Secondaries[0], mediaPrintJob);
                        }
                        else
                        {
                            //	Add each of the children to the queue
                            foreach (CDxSecondary O in dxPrimary.Secondaries)
                                AddToQueue(O, mediaPrintJob);

                        }// if((dxPrimary.MediaType == TmaxMediaTypes.Document) && (m_tmaxPrintOptions.OnlyFirstPage == true))

                    }// if((dxPrimary.Secondaries != null) && (dxPrimary.Secondaries.Count > 0))

                    break;

                case TmaxMediaTypes.Slide:
                case TmaxMediaTypes.Treatment:

                    m_dxRecords.AddList(dxMedia);
                    if (mediaPrintJob != null)
                        mediaPrintJob.AddMediaRecord(dxMedia);
                    break;

                case TmaxMediaTypes.Scene:

                    //	Should we add the scene to the job?
                    if (m_tmaxPrintOptions.OnlyLinks == false)
                    {
                        m_dxRecords.AddList(dxMedia);
                        if (mediaPrintJob != null)
                            mediaPrintJob.AddMediaRecord(dxMedia);
                    }

                    //	Make sure we have a source record
                    dxSecondary = (CDxSecondary)dxMedia;
                    if (dxSecondary.GetSource() != null)
                    {
                        //	What type of source record are we dealing with?
                        switch (dxSecondary.GetSource().MediaType)
                        {
                            case TmaxMediaTypes.Designation:
                            case TmaxMediaTypes.Clip:

                                //	Does the user want to add links to the job?
                                if ((m_tmaxPrintOptions.IncludeLinks == true) ||
                                   (m_tmaxPrintOptions.OnlyLinks == true))
                                {
                                    dxTertiary = (CDxTertiary)(dxSecondary.GetSource());

                                    //	Make sure the child collection is populated
                                    if ((dxTertiary.Quaternaries == null) || (dxTertiary.Quaternaries.Count == 0))
                                        dxTertiary.Fill();

                                    //	Add each of the children to the queue
                                    foreach (CDxQuaternary O in dxTertiary.Quaternaries)
                                    {
                                        m_dxRecords.AddList(O);
                                        if (mediaPrintJob != null)
                                            mediaPrintJob.AddMediaRecord(O);
                                    }

                                }// if(m_tmaxPrintOptions.IncludeLinks == true)
                                break;

                            default:

                                break;

                        }// switch(dxSecondary.GetSource().MediaType)

                    }// if(dxSecondary.GetSource() != null)
                    break;

                case TmaxMediaTypes.Segment:

                    if (((CDxSecondary)dxMedia).Primary.MediaType != TmaxMediaTypes.Deposition)
                    {
                        m_dxRecords.AddList(dxMedia);
                        if (mediaPrintJob != null)
                            mediaPrintJob.AddMediaRecord(dxMedia);
                    }
                    break;

                case TmaxMediaTypes.Link:

                    if (((CDxQuaternary)dxMedia).GetSource() != null)
                    {
                        m_dxRecords.AddList(((CDxQuaternary)dxMedia).GetSource());
                        if (mediaPrintJob != null)
                            mediaPrintJob.AddMediaRecord(dxMedia);
                    }
                    break;

                case TmaxMediaTypes.Page:

                    //	Add this record to the queue unless we're only allowed to add treatments
                    if (m_tmaxPrintOptions.OnlyTreatments == false)
                    {
                        m_dxRecords.AddList(dxMedia);
                        if (mediaPrintJob != null)
                            mediaPrintJob.AddMediaRecord(dxMedia);
                    }


                    //	Are we supposed to be adding treatments?
                    if ((m_tmaxPrintOptions.IncludeTreatments == true) ||
                       (m_tmaxPrintOptions.OnlyTreatments == true))
                    {
                        dxSecondary = (CDxSecondary)dxMedia;

                        //	Make sure the child collection is full
                        if ((dxSecondary.Tertiaries == null) || (dxSecondary.Tertiaries.Count == 0))
                            dxSecondary.Fill();

                        foreach (CDxTertiary O in dxSecondary.Tertiaries)
                            AddToQueue(O, mediaPrintJob);
                    }

                    break;

                //	User can not directly select these records for printing
                case TmaxMediaTypes.Designation:
                case TmaxMediaTypes.Clip:
                case TmaxMediaTypes.Deposition:
                default:

                    break;

            }// switch(dxMedia.MediaType)

            return true;

        }// private bool QueueSelection(CDxMediaRecord dxSelection)


        /// <summary>This method is called to queue the specified binder record</summary>
        /// <returns>True to continue with print job</returns>
        private bool AddToQueue(CDxBinderEntry dxBinder)
        {

            Debug.Assert(dxBinder != null);
            if (dxBinder == null) return false;

            try
            {
                //	Fill the child collection if necessary
                if ((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
                    dxBinder.Fill();

                MediaPrintJob mediaPrintJob = new MediaPrintJob(dxBinder.AutoId);
                m_MediaPrintJobs.Add(mediaPrintJob);

                //	Add each child to the queue
                foreach (CDxBinderEntry O in dxBinder.Contents)
                {
                    //	Is this a media entry
                    if (O.IsMedia() == true)
                    {
                        //	Add this media to the queue
                        if (O.Source != null)
                        {
                            AddToQueue(O.Source, mediaPrintJob);
                        }
                    }
                    else
                    {
                        //	Recurse if requested
                        if (m_tmaxPrintOptions.IncludeSubBinders == true)
                            AddToQueue(O);
                    }

                }// foreach(CDxBinderEntry O in dxBinder.Contents)

                
            }
            catch
            {
            }

            return true;

        }// private bool QueueSelection(CDxMediaRecord dxSelection)

        /// <summary>This method converts the record to a string to be added to the TMPrint queue</summary>
        /// <param name="dxRecord">The record being added to the job</param>
        /// <returns>The appropriate string</returns>
        private string GetAxString(CDxMediaRecord dxRecord)
        {
            CDxPrimary dxPrimary = null;
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;
            string strAxString = "";
            string strFilename = "";
            string strName = "";
            string strPath = "";
            string strDeponent = "";
            string strType = "";
            string strPage = "";
            string strPages = "";
            string strTreatmentImage = "";
            string strBarcode = "";
            string strGraphic = "";
            string strForeign = "";
            string strSource = "";
            string strPrimary = "";
            string strTertiary = "";
            string strTextField = "";
            string strSiblingFileSpec = "";
            string strSiblingImage = "";
            int iFlags = 0;

            //	Switch to the source record before doing anything if this is a
            //	linked document
            if (dxRecord.MediaType == TmaxMediaTypes.Link)
            {
                if ((dxRecord = ((CDxQuaternary)dxRecord).GetSource()) == null)
                    return "";

            }//	if(dxRecord.MediaType == TmaxMediaTypes.Link)

            strBarcode = dxRecord.GetBarcode(false);
            strForeign = dxRecord.GetForeignBarcode();
            strGraphic = dxRecord.GetBarcode(true);
            strPrimary = dxRecord.GetMediaId();
            strPage = dxRecord.GetDisplayOrder().ToString();
            if (dxRecord.GetParent() != null)
                strPages = dxRecord.GetParent().GetChildCount().ToString();

            //	Switch to the source record if this is a scene
            if (dxRecord.MediaType == TmaxMediaTypes.Scene)
            {
                if ((dxRecord = ((CDxSecondary)dxRecord).GetSource()) != null)
                {
                    //	Make the source barcode available for the report for some types
                    switch (dxRecord.MediaType)
                    {
                        case TmaxMediaTypes.Document:
                        case TmaxMediaTypes.Powerpoint:
                        case TmaxMediaTypes.Recording:
                        case TmaxMediaTypes.Page:
                        case TmaxMediaTypes.Segment:
                        case TmaxMediaTypes.Slide:
                        case TmaxMediaTypes.Treatment:

                            strSource = dxRecord.GetBarcode(false);
                            break;

                        case TmaxMediaTypes.Clip:
                        case TmaxMediaTypes.Designation:

                            strSource = strBarcode; // Use the scene barcode
                            break;

                        default:

                            break;

                    }// switch(dxRecord.MediaType)

                }
                else
                {
                    return "";
                }

            }// if(dxRecord.MediaType == TmaxMediaTypes.Scene)

            //	Get the record interfaces
            switch (dxRecord.GetMediaLevel())
            {
                case TmaxMediaLevels.Secondary:

                    dxSecondary = (CDxSecondary)dxRecord;
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

                case TmaxMediaLevels.Tertiary:

                    dxTertiary = (CDxTertiary)dxRecord;
                    dxSecondary = dxTertiary.Secondary;
                    Debug.Assert(dxSecondary != null);
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

                default:

                    //	Primaries get expanded and quaternaries use the source record
                    Debug.Assert(false);
                    return "";
            }

            //	Get the type specific values
            switch (dxRecord.MediaType)
            {
                case TmaxMediaTypes.Page:

                    if (dxSecondary.HighResolution == true)
                        strType = "G";
                    else
                        strType = "T";

                    strFilename = System.IO.Path.GetFileName(dxSecondary.GetFileSpec());
                    strPath = System.IO.Path.GetDirectoryName(dxSecondary.GetFileSpec());

                    if (dxSecondary.Name.Length > 0)
                        strName = dxSecondary.Name;
                    else
                        strName = dxPrimary.Name;
                    break;

                case TmaxMediaTypes.Treatment:

                    if (dxSecondary.HighResolution == true)
                        strType = "G";
                    else
                        strType = "T";

                    strFilename = System.IO.Path.GetFileName(m_tmaxDatabase.GetFileSpec(dxTertiary));
                    strPath = System.IO.Path.GetDirectoryName(m_tmaxDatabase.GetFileSpec(dxTertiary));
                    strTertiary = dxTertiary.AutoId.ToString();
                    strTreatmentImage = m_tmaxDatabase.GetFileSpec(dxSecondary);

                    //	Is this a split screen treatment?
                    if (dxTertiary.SplitScreen == true)
                    {
                        //	Do we have a valid sibling treatment?
                        if ((dxTertiary.Sibling != null) && (dxTertiary.Sibling.GetParent() != null))
                        {
                            strSiblingFileSpec = m_tmaxDatabase.GetFileSpec(dxTertiary.Sibling);
                            strSiblingImage = m_tmaxDatabase.GetFileSpec(dxTertiary.Sibling.GetParent());

                            if (dxTertiary.SplitRight == true)
                                iFlags |= SPLITSCREEN_BITFLAG_RIGHT;
                            if (dxTertiary.SplitHorizontal == true)
                                iFlags |= SPLITSCREEN_BITFLAG_HORIZONTAL;

                        }// if(dxTertiary.Sibling != null)

                    }// if(dxTertiary.SplitScreen == true)

                    if (dxTertiary.Name.Length > 0)
                        strName = dxTertiary.Name;
                    else if (dxSecondary.Name.Length > 0)
                        strName = dxSecondary.Name;
                    else
                        strName = dxPrimary.Name;
                    break;

                case TmaxMediaTypes.Slide:

                    strType = "P";
                    strFilename = System.IO.Path.GetFileName(m_tmaxDatabase.GetFileSpec(dxPrimary));
                    strPath = System.IO.Path.GetDirectoryName(m_tmaxDatabase.GetFileSpec(dxPrimary));

                    if (dxSecondary.Name.Length > 0)
                        strName = dxSecondary.Name;
                    else
                        strName = dxPrimary.Name;

                    //	Specify printing of the exported image if available
                    if (m_tmaxDatabase.CheckSlide(dxSecondary) == true)
                    {
                        strTreatmentImage = m_tmaxDatabase.GetFileSpec(dxSecondary);
                    }
                    else
                    {
                        MessageBox.Show("Unable to export slide");
                    }
                    break;

                case TmaxMediaTypes.Segment:

                    strType = "M";
                    strFilename = System.IO.Path.GetFileName(m_tmaxDatabase.GetFileSpec(dxSecondary));
                    strPath = System.IO.Path.GetDirectoryName(m_tmaxDatabase.GetFileSpec(dxSecondary));
                    if (dxSecondary.Name.Length > 0)
                        strName = dxSecondary.Name;
                    else
                        strName = dxPrimary.Name;
                    break;

                case TmaxMediaTypes.Clip:

                    strType = "M";
                    strFilename = System.IO.Path.GetFileName(m_tmaxDatabase.GetFileSpec(dxSecondary));
                    strPath = System.IO.Path.GetDirectoryName(m_tmaxDatabase.GetFileSpec(dxSecondary));
                    strDeponent = dxTertiary.Name;
                    if (dxTertiary.Name.Length > 0)
                        strName = dxTertiary.Name;
                    else if (dxSecondary.Name.Length > 0)
                        strName = dxSecondary.Name;
                    else
                        strName = dxPrimary.Name;
                    break;

                case TmaxMediaTypes.Designation:

                    strType = "D";
                    strFilename = System.IO.Path.GetFileName(m_tmaxDatabase.GetFileSpec(dxSecondary));
                    strPath = System.IO.Path.GetDirectoryName(m_tmaxDatabase.GetFileSpec(dxSecondary));
                    strDeponent = dxTertiary.Name;
                    if (dxTertiary.Name.Length > 0)
                        strName = dxTertiary.Name;
                    else if (dxSecondary.Name.Length > 0)
                        strName = dxSecondary.Name;
                    else
                        strName = dxPrimary.Name;
                    break;

                default:

                    Debug.Assert(false);
                    return "";

            }// switch(dxRecord.MediaType)

            //	Build the string
            strAxString = ("Barcode=" + strBarcode);
            strAxString += ("~Graphic=" + strGraphic);
            strAxString += ("~Primary=" + strPrimary);
            strAxString += ("~Page=" + strPage);

            if (strForeign.Length > 0)
                strAxString += ("~Foreign=" + strForeign);

            if (strSource.Length > 0)
                strAxString += ("~Source=" + strSource);

            if (strPages.Length > 0)
                strAxString += ("~Pages=" + strPages);
            if (strTertiary.Length > 0)
                strAxString += ("~Tertiary=" + strTertiary);
            if (strFilename.Length > 0)
                strAxString += ("~Filename=" + strFilename);
            if (strPath.Length > 0)
                strAxString += ("~Path=" + strPath);
            if (strName.Length > 0)
                strAxString += ("~Name=" + strName);
            if (strDeponent.Length > 0)
                strAxString += ("~Deponent=" + strDeponent);
            if (strType.Length > 0)
                strAxString += ("~Type=" + strType);
            if (strTreatmentImage.Length > 0)
                strAxString += ("~TreatmentImage=" + strTreatmentImage);
            if (strSiblingFileSpec.Length > 0)
                strAxString += ("~SiblingPath=" + strSiblingFileSpec);
            if (strSiblingImage.Length > 0)
                strAxString += ("~SiblingImage=" + strSiblingImage);
            if (iFlags > 0)
                strAxString += ("~Flags=" + iFlags.ToString());

            //	Now append the user defined text fields
            for (int i = 0; i < m_tmaxPrintOptions.TextFields.Count; i++)
            {
                //	Are we printing this field?
                if (m_tmaxPrintOptions.TextFields[i].PrintText.Length > 0)
                {
                    strTextField = String.Format("~TF{0}={1}", m_tmaxPrintOptions.TextFields[i].Id, m_tmaxPrintOptions.TextFields[i].PrintText);
                    strAxString += strTextField;

                }// if(m_tmaxPrintOptions.TextFields[i].PrintText.Length > 0)

            }// for(int i = 0; i < m_tmaxPrintOptions.TextFields.Count; i++)

            return strAxString;

        }// private string GetAxString(CDxMediaRecord dxRecord)

        /// <summary>This method sets the TMPrint properties for the print job</summary>
        /// <returns>true if successful</returns>
        private bool SetAxProps()
        {
            try
            {
                m_tmxPrint.Printer = m_tmaxPrintOptions.Printer;
                m_tmxPrint.TemplateName = m_tmaxPrintOptions.Template;
                m_tmxPrint.IncludePageTotal = m_tmaxPrintOptions.PrintPageTotal;
                m_tmxPrint.IncludePathInFileName = m_tmaxPrintOptions.PrintPath;
                m_tmxPrint.ForceNewPage = m_tmaxPrintOptions.ForceNewPage;
                m_tmxPrint.InsertSlipSheet = m_tmaxPrintOptions.InsertSlipSheet;
                m_tmxPrint.PrintCallouts = m_tmaxPrintOptions.PrintCallouts;
                m_tmxPrint.PrintCalloutBorders = m_tmaxPrintOptions.PrintCalloutBorders;
                m_tmxPrint.PrintBarcodeGraphic = m_ctrlTemplateGraphic.Checked;
                m_tmxPrint.PrintBarcodeText = m_ctrlTemplateText.Checked;
                m_tmxPrint.PrintForeignBarcode = m_ctrlTemplateForeign.Checked;
                m_tmxPrint.PrintSourceBarcode = m_ctrlTemplateSource.Checked;
                m_tmxPrint.PrintCellBorder = m_ctrlTemplateBorder.Checked;
                m_tmxPrint.PrintDeponent = m_ctrlTemplateDeponent.Checked;
                m_tmxPrint.PrintName = m_ctrlTemplateName.Checked;
                m_tmxPrint.PrintFileName = m_ctrlTemplateFilename.Checked;
                m_tmxPrint.PrintImage = m_ctrlTemplateImage.Checked;
                m_tmxPrint.PrintPageNumber = m_ctrlTemplatePageNumber.Checked;
                m_tmxPrint.AutoRotate = m_ctrlAutoRotate.Checked;

                if (m_tmaxPresentationOptions != null)
                    m_tmxPrint.CalloutFrameColor = (short)(m_tmaxPresentationOptions.CalloutFrameColor);

                //	Are we doing double sided printing?
                if (m_tmaxPrintOptions.DoubleSided == true)
                {
                    //	Copies will always be 1 because each document is a new job
                    m_tmxPrint.Copies = 1;
                    m_tmxPrint.Collate = true;
                }
                else
                {
                    m_tmxPrint.Collate = m_tmaxPrintOptions.Collate;
                    m_tmxPrint.Copies = (short)m_tmaxPrintOptions.Copies;
                }

                //	Enable / disable the text fields
                for (int i = 0; i < m_tmaxPrintOptions.TextFields.Count; i++)
                {
                    //	Are we printing this field?
                    if ((m_tmaxPrintOptions.TextFields[i].Print == true) &&
                       (m_tmaxPrintOptions.TextFields[i].PrintText.Length > 0))
                    {
                        m_tmxPrint.SetTextFieldEnabled(m_tmaxPrintOptions.TextFields[i].Id, m_tmaxPrintOptions.TextFields[i].Name, (short)1);
                    }
                    else
                    {
                        m_tmxPrint.SetTextFieldEnabled(m_tmaxPrintOptions.TextFields[i].Id, m_tmaxPrintOptions.TextFields[i].Name, 0);
                    }

                }// for(int i = 0; i < m_tmaxPrintOptions.TextFields.Count; i++)

                return true;
            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "SetAxProps", Ex);
                return false;
            }

        }// private bool SetAxProps()

        /// <summary>This method is called to get the bitmask that represents the current template options</summary>
        /// <returns>The packed bitfield mask</returns>
        private int GetTemplateFlags()
        {
            int iMask = 0;

            if ((m_ctrlTemplateName.Enabled == true) && (m_ctrlTemplateName.Checked == true))
                iMask |= TEMPLATE_BITMASK_NAME;

            if ((m_ctrlTemplateText.Enabled == true) && (m_ctrlTemplateText.Checked == true))
                iMask |= TEMPLATE_BITMASK_BARCODE;

            if ((m_ctrlTemplateGraphic.Enabled == true) && (m_ctrlTemplateGraphic.Checked == true))
                iMask |= TEMPLATE_BITMASK_GRAPHIC;

            if ((m_ctrlTemplateForeign.Enabled == true) && (m_ctrlTemplateForeign.Checked == true))
                iMask |= TEMPLATE_BITMASK_FOREIGN_BARCODE;

            if ((m_ctrlTemplateSource.Enabled == true) && (m_ctrlTemplateSource.Checked == true))
                iMask |= TEMPLATE_BITMASK_SOURCE_BARCODE;

            if ((m_ctrlTemplateFilename.Enabled == true) && (m_ctrlTemplateFilename.Checked == true))
                iMask |= TEMPLATE_BITMASK_FILENAME;

            if ((m_ctrlTemplatePageNumber.Enabled == true) && (m_ctrlTemplatePageNumber.Checked == true))
                iMask |= TEMPLATE_BITMASK_PAGENUM;

            if ((m_ctrlTemplateDeponent.Enabled == true) && (m_ctrlTemplateDeponent.Checked == true))
                iMask |= TEMPLATE_BITMASK_DEPONENT;

            if ((m_ctrlTemplateImage.Enabled == true) && (m_ctrlTemplateImage.Checked == true))
                iMask |= TEMPLATE_BITMASK_IMAGE;

            if ((m_ctrlTemplateBorder.Enabled == true) && (m_ctrlTemplateBorder.Checked == true))
                iMask |= TEMPLATE_BITMASK_CELL_BORDER;

            //	Auto rotate is not really a field but it is a template option
            if ((m_ctrlAutoRotate.Enabled == true) && (m_ctrlAutoRotate.Checked == true))
                iMask |= TEMPLATE_BITMASK_AUTO_ROTATE;

            return iMask;

        }// private int GetTemplateFlags()

        /// <summary>This method is called to set the check state of the template field controls using the specified bitmask</summary>
        /// <param name="iMask">The mask used to define the fields that should be enabled</param>
        private void SetFlagsEnabled(int iMask)
        {
            if ((iMask & TEMPLATE_BITMASK_NAME) == TEMPLATE_BITMASK_NAME)
                m_ctrlTemplateName.Enabled = true;
            else
                m_ctrlTemplateName.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_BARCODE) == TEMPLATE_BITMASK_BARCODE)
                m_ctrlTemplateText.Enabled = true;
            else
                m_ctrlTemplateText.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_GRAPHIC) == TEMPLATE_BITMASK_GRAPHIC)
                m_ctrlTemplateGraphic.Enabled = true;
            else
                m_ctrlTemplateGraphic.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_FOREIGN_BARCODE) == TEMPLATE_BITMASK_FOREIGN_BARCODE)
                m_ctrlTemplateForeign.Enabled = true;
            else
                m_ctrlTemplateForeign.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_SOURCE_BARCODE) == TEMPLATE_BITMASK_SOURCE_BARCODE)
                m_ctrlTemplateSource.Enabled = true;
            else
                m_ctrlTemplateSource.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_FILENAME) == TEMPLATE_BITMASK_FILENAME)
                m_ctrlTemplateFilename.Enabled = true;
            else
                m_ctrlTemplateFilename.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_PAGENUM) == TEMPLATE_BITMASK_PAGENUM)
                m_ctrlTemplatePageNumber.Enabled = true;
            else
                m_ctrlTemplatePageNumber.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_DEPONENT) == TEMPLATE_BITMASK_DEPONENT)
                m_ctrlTemplateDeponent.Enabled = true;
            else
                m_ctrlTemplateDeponent.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_IMAGE) == TEMPLATE_BITMASK_IMAGE)
                m_ctrlTemplateImage.Enabled = true;
            else
                m_ctrlTemplateImage.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_CELL_BORDER) == TEMPLATE_BITMASK_CELL_BORDER)
                m_ctrlTemplateBorder.Enabled = true;
            else
                m_ctrlTemplateBorder.Enabled = false;

            if ((iMask & TEMPLATE_BITMASK_AUTO_ROTATE) == TEMPLATE_BITMASK_AUTO_ROTATE)
                m_ctrlAutoRotate.Enabled = true;
            else
                m_ctrlAutoRotate.Enabled = false;

        }// private void SetFlagsEnabled(int iMask)

        /// <summary>This method is called to set the check state of the template field controls using the specified bitmask</summary>
        /// <param name="iMask">The mask used to define the fields that should be enabled</param>
        private void SetFieldsChecked(int iMask)
        {
            if ((iMask & TEMPLATE_BITMASK_NAME) == TEMPLATE_BITMASK_NAME)
                m_ctrlTemplateName.Checked = true;
            else
                m_ctrlTemplateName.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_BARCODE) == TEMPLATE_BITMASK_BARCODE)
                m_ctrlTemplateText.Checked = true;
            else
                m_ctrlTemplateText.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_GRAPHIC) == TEMPLATE_BITMASK_GRAPHIC)
                m_ctrlTemplateGraphic.Checked = true;
            else
                m_ctrlTemplateGraphic.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_FOREIGN_BARCODE) == TEMPLATE_BITMASK_FOREIGN_BARCODE)
                m_ctrlTemplateForeign.Checked = true;
            else
                m_ctrlTemplateForeign.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_SOURCE_BARCODE) == TEMPLATE_BITMASK_SOURCE_BARCODE)
                m_ctrlTemplateSource.Checked = true;
            else
                m_ctrlTemplateSource.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_FILENAME) == TEMPLATE_BITMASK_FILENAME)
                m_ctrlTemplateFilename.Checked = true;
            else
                m_ctrlTemplateFilename.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_PAGENUM) == TEMPLATE_BITMASK_PAGENUM)
                m_ctrlTemplatePageNumber.Checked = true;
            else
                m_ctrlTemplatePageNumber.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_DEPONENT) == TEMPLATE_BITMASK_DEPONENT)
                m_ctrlTemplateDeponent.Checked = true;
            else
                m_ctrlTemplateDeponent.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_IMAGE) == TEMPLATE_BITMASK_IMAGE)
                m_ctrlTemplateImage.Checked = true;
            else
                m_ctrlTemplateImage.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_CELL_BORDER) == TEMPLATE_BITMASK_CELL_BORDER)
                m_ctrlTemplateBorder.Checked = true;
            else
                m_ctrlTemplateBorder.Checked = false;

            if ((iMask & TEMPLATE_BITMASK_AUTO_ROTATE) == TEMPLATE_BITMASK_AUTO_ROTATE)
                m_ctrlAutoRotate.Checked = true;
            else
                m_ctrlAutoRotate.Checked = false;

        }// private void SetFieldsChecked(int iMask)

        /// <summary>This method handles error events fired by the TMPrint ActiveX control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxError(object sender, AxTM_PRINT6Lib._DTMPrint6Events_AxErrorEvent e)
        {
            //	Propagate the event
            m_tmaxEventSource.FireError(this, "OnAxError", e.lpszMessage);

        }// private void OnAxError(object sender, AxTM_PRINT6Lib._DTMPrint6Events_AxErrorEvent e)

        /// <summary>This method handles diagnostic events fired by the TMPrint ActiveX control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxDiagnostic(object sender, AxTM_PRINT6Lib._DTMPrint6Events_AxDiagnosticEvent e)
        {
            //	Propagate the event
            m_tmaxEventSource.FireDiagnostic(this, e.lpszMethod, e.lpszMessage);
        }

        /// <summary>This method is called to initialize the ActiveX control used to run the print job</summary>
        /// <returns>true if successful</returns>
        private bool InitAxPrinter()
        {
            bool bSuccessful = false;
            int iIndex = 0;

            Debug.Assert(m_tmxPrint != null);
            if (m_tmxPrint == null) return false;

            try
            {
                while (bSuccessful == false)
                {
                    m_tmxPrint.FirstTemplate += new AxTM_PRINT6Lib._DTMPrint6Events_FirstTemplateEventHandler(this.OnAxFirstTemplate);
                    m_tmxPrint.AxError += new AxTM_PRINT6Lib._DTMPrint6Events_AxErrorEventHandler(this.OnAxError);
                    m_tmxPrint.NextPrinter += new AxTM_PRINT6Lib._DTMPrint6Events_NextPrinterEventHandler(this.OnAxNextPrinter);
                    m_tmxPrint.NextTemplate += new AxTM_PRINT6Lib._DTMPrint6Events_NextTemplateEventHandler(this.OnAxNextTemplate);
                    m_tmxPrint.FirstPrinter += new AxTM_PRINT6Lib._DTMPrint6Events_FirstPrinterEventHandler(this.OnAxFirstPrinter);
                    m_tmxPrint.AxDiagnostic += new AxTM_PRINT6Lib._DTMPrint6Events_AxDiagnosticEventHandler(this.OnAxDiagnostic);
                    m_tmxPrint.NextTextField += new AxTM_PRINT6Lib._DTMPrint6Events_NextTextFieldEventHandler(this.OnAxNextTextField);
                    m_tmxPrint.FirstTextField += new AxTM_PRINT6Lib._DTMPrint6Events_FirstTextFieldEventHandler(this.OnAxFirstTextField);

                    //	Set the path to the file containing the template descriptors
                    m_tmxPrint.IniFile = m_strTemplateFilename;

                    //	Initialize the TMPrint control
                    if (m_tmxPrint.Initialize() != 0)
                        break;

                    //	Transfer the initial text field descriptors to a temporary collection
                    if (m_tmaxPrintOptions.TextFields.Count > 0)
                    {
                        m_tmaxInitialTextFields = new CTmaxPrintFields();
                        foreach (CTmaxPrintField O in m_tmaxPrintOptions.TextFields)
                            m_tmaxInitialTextFields.Add(O);

                        m_tmaxPrintOptions.TextFields.Clear();

                    }// if(m_tmaxPrintOptions.TextFields.Count > 0)

                    //	Get the list of available templates
                    if (m_tmxPrint.EnumerateTemplates() != 0)
                        break;

                    //	Are any templates available?
                    if (m_ctrlTemplates.Items.Count > 0)
                    {
                        //	Look for the last used template
                        if (m_tmaxPrintOptions.Template.Length > 0)
                            iIndex = m_ctrlTemplates.FindStringExact(m_tmaxPrintOptions.Template, -1);

                        //	Set the selection in the list box
                        if (iIndex >= 0)
                        {
                            m_ctrlTemplates.SelectedIndex = iIndex;

                            //	Set the field selections
                            if (m_tmaxPrintOptions.Fields > 0)
                                SetFieldsChecked(m_tmaxPrintOptions.Fields);

                            //	We should have processed the initial collection by now
                            //
                            //	NOTE:	We have to wait to do this until AFTER we set
                            //			the template selection index
                            m_tmaxInitialTextFields = null;
                        }
                        else
                        {
                            //	Prevent any attempt to use the initial text fields 
                            //	since the templates do not match
                            //
                            //	NOTE:	We have to do this BEFORE we set the index
                            m_tmaxInitialTextFields = null;

                            m_ctrlTemplates.SelectedIndex = 0;
                        }

                    }
                    else
                    {
                        MessageBox.Show("No templates descriptors could be found in " + m_strTemplateFilename, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                    }

                    bSuccessful = true;

                }// while(bSuccessful == false)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "InitAxPrinter", "An exception was raised while initializing the TMPrint control", Ex);
            }

            return bSuccessful;

        }// private bool InitAxPrinter()

        /// <summary>This method is called to populate the printer selection box</summary>
        /// <returns>true if successful</returns>
        private bool FillPrinters()
        {
            bool bSuccessful = false;
            int iIndex = 0;

            try
            {
                while (bSuccessful == false)
                {
                    //	Clear the existing list
                    if (m_ctrlPrinters.Items != null)
                        m_ctrlPrinters.Items.Clear();

                    //	Get the TMPrint control to enumerate the printers
                    if (m_tmxPrint.EnumeratePrinters() != 0)
                        break;

                    //	Are any printers available?
                    if (m_ctrlPrinters.Items.Count > 0)
                    {
                        //	Look for the printer used for the last job. Use the default if not
                        //	available or not found
                        if (m_tmaxPrintOptions.Printer.Length == 0)
                            m_tmaxPrintOptions.Printer = m_tmxPrint.GetDefaultPrinter();
                        if ((iIndex = m_ctrlPrinters.FindStringExact(m_tmaxPrintOptions.Printer, -1)) < 0)
                            iIndex = m_ctrlPrinters.FindStringExact(m_tmxPrint.GetDefaultPrinter());

                        //	Set the selection in the list box
                        if (iIndex >= 0)
                            m_ctrlPrinters.SelectedIndex = iIndex;
                        else
                            m_ctrlPrinters.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("No printers could be found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                    }

                    bSuccessful = true;

                }// while(bSuccessful == false)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "FillPrinters", m_tmaxErrorBuilder.Message(ERROR_FILL_PRINTERS_EX), Ex);
            }

            return bSuccessful;

        }// private bool FillPrinters()

        /// <summary>This method is called to populate the list of available text fields</summary>
        /// <returns>true if successful</returns>
        private bool FillTextFields()
        {
            bool bSuccessful = false;

            try
            {
                //	Clear the existing print options fields
                m_tmaxPrintOptions.TextFields.Clear();
                m_tmaxTextField = null;

                //	Clear the child controls
                if (m_ctrlTextFieldsList.Items != null)
                    m_ctrlTextFieldsList.Items.Clear();
                m_ctrlFieldText.Text = "";

                //	Enumerate the fields in the active template
                if (m_ctrlTemplates.Text.Length > 0)
                    m_tmxPrint.EnumerateTextFields(m_ctrlTemplates.Text);					//	Get the TMPrint control to enumerate the printers

                //	Do we have any fields in this template?
                if (m_tmaxPrintOptions.TextFields.Count > 0)
                {
                    foreach (CTmaxPrintField O in m_tmaxPrintOptions.TextFields)
                    {
                        m_ctrlTextFieldsList.Items.Add(O, O.Print);
                    }

                    //	Set the initial selection in the list box
                    if (m_ctrlTextFieldsList.Items.Count > 0)
                        m_ctrlTextFieldsList.SelectedIndex = 0;

                }// if(m_tmaxPrintOptions.TextFields.Count > 0)

                m_ctrlTextFieldsLabel.Enabled = (m_ctrlTextFieldsList.Items.Count > 0);
                m_ctrlTextFieldsList.Enabled = (m_ctrlTextFieldsList.Items.Count > 0);
                m_ctrlFieldTextLabel.Enabled = (m_ctrlTextFieldsList.Items.Count > 0);
                m_ctrlFieldText.Enabled = (m_ctrlTextFieldsList.Items.Count > 0);
                m_ctrlFieldSubstitutions.Enabled = (m_ctrlTextFieldsList.Items.Count > 0);

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "FillTextFields", m_tmaxErrorBuilder.Message(ERROR_FILL_TEXT_FIELDS_EX), Ex);
            }

            return bSuccessful;

        }// private bool FillTextFields()

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFPrint));
            this.m_ctrlDone = new System.Windows.Forms.Button();
            this.m_ctrlPrint = new System.Windows.Forms.Button();
            this.m_ctrlRangeGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlSelections = new System.Windows.Forms.Label();
            this.m_ctrlBarcodeExample2 = new System.Windows.Forms.Label();
            this.m_ctrlBarcodes = new System.Windows.Forms.TextBox();
            this.m_ctrlPrintBarcodes = new System.Windows.Forms.RadioButton();
            this.m_ctrlPrintSelections = new System.Windows.Forms.RadioButton();
            this.m_ctrlIncludeSubBinders = new System.Windows.Forms.CheckBox();
            this.m_ctrlIncludeLinks = new System.Windows.Forms.CheckBox();
            this.m_ctrlOnlyFirstPage = new System.Windows.Forms.CheckBox();
            this.m_ctrlOnlyTreatments = new System.Windows.Forms.CheckBox();
            this.m_ctrlIncludeTreatments = new System.Windows.Forms.CheckBox();
            this.m_ctrlCopiesLabel = new System.Windows.Forms.Label();
            this.m_ctrlCopies = new System.Windows.Forms.TextBox();
            this.m_ctrlCollate = new System.Windows.Forms.CheckBox();
            this.m_ctrlPrinters = new System.Windows.Forms.ComboBox();
            this.m_ctrlTemplates = new System.Windows.Forms.ComboBox();
            this.m_ctrlPrintPath = new System.Windows.Forms.CheckBox();
            this.m_ctrlPrintCallouts = new System.Windows.Forms.CheckBox();
            this.m_ctrlPrintPageTotal = new System.Windows.Forms.CheckBox();
            this.m_ctrlForceNewPage = new System.Windows.Forms.CheckBox();
            this.m_ctrlDoubleSided = new System.Windows.Forms.CheckBox();
            this.m_ctrlPrinterGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlPrinterProperties = new System.Windows.Forms.Button();
            this.m_ctrlOptionalGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlInsertSlipSheet = new System.Windows.Forms.CheckBox();
            this.m_ctrlAutoRotate = new System.Windows.Forms.CheckBox();
            this.m_ctrlPrintCalloutBorders = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlSubstitutionsLabel = new System.Windows.Forms.Label();
            this.m_ctrlFieldText = new System.Windows.Forms.TextBox();
            this.m_ctrlFieldSubstitutions = new System.Windows.Forms.ComboBox();
            this.m_ctrlFieldTextLabel = new System.Windows.Forms.Label();
            this.m_ctrlTextFieldsLabel = new System.Windows.Forms.Label();
            this.m_ctrlTextFieldsList = new System.Windows.Forms.CheckedListBox();
            this.m_ctrlTemplateSource = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateForeign = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateFilename = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateDeponent = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateName = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateText = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplatePageNumber = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateBorder = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateImage = new System.Windows.Forms.CheckBox();
            this.m_ctrlTemplateGraphic = new System.Windows.Forms.CheckBox();
            this.m_ctrlContentGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlOnlyLinks = new System.Windows.Forms.CheckBox();
            this.m_ctrlRangeGroup.SuspendLayout();
            this.m_ctrlPrinterGroup.SuspendLayout();
            this.m_ctrlOptionalGroup.SuspendLayout();
            this.m_ctrlTemplateGroup.SuspendLayout();
            this.m_ctrlContentGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlDone
            // 
            this.m_ctrlDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_ctrlDone.Location = new System.Drawing.Point(575, 460);
            this.m_ctrlDone.Name = "m_ctrlDone";
            this.m_ctrlDone.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlDone.TabIndex = 6;
            this.m_ctrlDone.Text = "&Cancel";
            // 
            // m_ctrlPrint
            // 
            this.m_ctrlPrint.Location = new System.Drawing.Point(491, 460);
            this.m_ctrlPrint.Name = "m_ctrlPrint";
            this.m_ctrlPrint.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlPrint.TabIndex = 5;
            this.m_ctrlPrint.Text = "&OK";
            this.m_ctrlPrint.Click += new System.EventHandler(this.OnClickPrint);
            // 
            // m_ctrlRangeGroup
            // 
            this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSelections);
            this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlBarcodeExample2);
            this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlBarcodes);
            this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlPrintBarcodes);
            this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlPrintSelections);
            this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlIncludeSubBinders);
            this.m_ctrlRangeGroup.Location = new System.Drawing.Point(8, 8);
            this.m_ctrlRangeGroup.Name = "m_ctrlRangeGroup";
            this.m_ctrlRangeGroup.Size = new System.Drawing.Size(322, 134);
            this.m_ctrlRangeGroup.TabIndex = 0;
            this.m_ctrlRangeGroup.TabStop = false;
            this.m_ctrlRangeGroup.Text = "Print Range";
            // 
            // m_ctrlSelections
            // 
            this.m_ctrlSelections.Location = new System.Drawing.Point(92, 28);
            this.m_ctrlSelections.Name = "m_ctrlSelections";
            this.m_ctrlSelections.Size = new System.Drawing.Size(184, 12);
            this.m_ctrlSelections.TabIndex = 8;
            this.m_ctrlSelections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlBarcodeExample2
            // 
            this.m_ctrlBarcodeExample2.Location = new System.Drawing.Point(92, 68);
            this.m_ctrlBarcodeExample2.Name = "m_ctrlBarcodeExample2";
            this.m_ctrlBarcodeExample2.Size = new System.Drawing.Size(192, 16);
            this.m_ctrlBarcodeExample2.TabIndex = 7;
            this.m_ctrlBarcodeExample2.Text = "e.g. 1.2, 3.4, 2.2.3,4";
            // 
            // m_ctrlBarcodes
            // 
            this.m_ctrlBarcodes.Location = new System.Drawing.Point(94, 44);
            this.m_ctrlBarcodes.Name = "m_ctrlBarcodes";
            this.m_ctrlBarcodes.Size = new System.Drawing.Size(219, 20);
            this.m_ctrlBarcodes.TabIndex = 2;
            this.m_ctrlBarcodes.WordWrap = false;
            // 
            // m_ctrlPrintBarcodes
            // 
            this.m_ctrlPrintBarcodes.Location = new System.Drawing.Point(12, 44);
            this.m_ctrlPrintBarcodes.Name = "m_ctrlPrintBarcodes";
            this.m_ctrlPrintBarcodes.Size = new System.Drawing.Size(76, 20);
            this.m_ctrlPrintBarcodes.TabIndex = 1;
            this.m_ctrlPrintBarcodes.Text = "Barcodes :";
            // 
            // m_ctrlPrintSelections
            // 
            this.m_ctrlPrintSelections.Checked = true;
            this.m_ctrlPrintSelections.Location = new System.Drawing.Point(12, 20);
            this.m_ctrlPrintSelections.Name = "m_ctrlPrintSelections";
            this.m_ctrlPrintSelections.Size = new System.Drawing.Size(76, 20);
            this.m_ctrlPrintSelections.TabIndex = 0;
            this.m_ctrlPrintSelections.TabStop = true;
            this.m_ctrlPrintSelections.Text = "Selections";
            // 
            // m_ctrlIncludeSubBinders
            // 
            this.m_ctrlIncludeSubBinders.Location = new System.Drawing.Point(13, 94);
            this.m_ctrlIncludeSubBinders.Name = "m_ctrlIncludeSubBinders";
            this.m_ctrlIncludeSubBinders.Size = new System.Drawing.Size(226, 20);
            this.m_ctrlIncludeSubBinders.TabIndex = 3;
            this.m_ctrlIncludeSubBinders.Text = "Include SubBinders";
            // 
            // m_ctrlIncludeLinks
            // 
            this.m_ctrlIncludeLinks.Location = new System.Drawing.Point(10, 118);
            this.m_ctrlIncludeLinks.Name = "m_ctrlIncludeLinks";
            this.m_ctrlIncludeLinks.Size = new System.Drawing.Size(284, 20);
            this.m_ctrlIncludeLinks.TabIndex = 4;
            this.m_ctrlIncludeLinks.Text = "Print all document links for selected script scenes";
            // 
            // m_ctrlOnlyFirstPage
            // 
            this.m_ctrlOnlyFirstPage.Location = new System.Drawing.Point(10, 21);
            this.m_ctrlOnlyFirstPage.Name = "m_ctrlOnlyFirstPage";
            this.m_ctrlOnlyFirstPage.Size = new System.Drawing.Size(284, 20);
            this.m_ctrlOnlyFirstPage.TabIndex = 0;
            this.m_ctrlOnlyFirstPage.Text = "Only use the first page of selected documents";
            // 
            // m_ctrlOnlyTreatments
            // 
            this.m_ctrlOnlyTreatments.Location = new System.Drawing.Point(10, 43);
            this.m_ctrlOnlyTreatments.Name = "m_ctrlOnlyTreatments";
            this.m_ctrlOnlyTreatments.Size = new System.Drawing.Size(224, 20);
            this.m_ctrlOnlyTreatments.TabIndex = 1;
            this.m_ctrlOnlyTreatments.Text = "Only print treatments, nothing else";
            this.m_ctrlOnlyTreatments.Click += new System.EventHandler(this.OnClickOnlyTreatments);
            // 
            // m_ctrlIncludeTreatments
            // 
            this.m_ctrlIncludeTreatments.Location = new System.Drawing.Point(10, 65);
            this.m_ctrlIncludeTreatments.Name = "m_ctrlIncludeTreatments";
            this.m_ctrlIncludeTreatments.Size = new System.Drawing.Size(284, 20);
            this.m_ctrlIncludeTreatments.TabIndex = 2;
            this.m_ctrlIncludeTreatments.Text = "Print all treatments for any selected page/document";
            // 
            // m_ctrlCopiesLabel
            // 
            this.m_ctrlCopiesLabel.Location = new System.Drawing.Point(146, 75);
            this.m_ctrlCopiesLabel.Name = "m_ctrlCopiesLabel";
            this.m_ctrlCopiesLabel.Size = new System.Drawing.Size(48, 20);
            this.m_ctrlCopiesLabel.TabIndex = 1;
            this.m_ctrlCopiesLabel.Text = "Copies :";
            this.m_ctrlCopiesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlCopies
            // 
            this.m_ctrlCopies.Location = new System.Drawing.Point(201, 75);
            this.m_ctrlCopies.Name = "m_ctrlCopies";
            this.m_ctrlCopies.Size = new System.Drawing.Size(39, 20);
            this.m_ctrlCopies.TabIndex = 4;
            this.m_ctrlCopies.WordWrap = false;
            // 
            // m_ctrlCollate
            // 
            this.m_ctrlCollate.Location = new System.Drawing.Point(9, 75);
            this.m_ctrlCollate.Name = "m_ctrlCollate";
            this.m_ctrlCollate.Size = new System.Drawing.Size(104, 20);
            this.m_ctrlCollate.TabIndex = 3;
            this.m_ctrlCollate.Text = "Collate";
            // 
            // m_ctrlPrinters
            // 
            this.m_ctrlPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ctrlPrinters.DropDownWidth = 300;
            this.m_ctrlPrinters.Location = new System.Drawing.Point(8, 22);
            this.m_ctrlPrinters.Name = "m_ctrlPrinters";
            this.m_ctrlPrinters.Size = new System.Drawing.Size(212, 21);
            this.m_ctrlPrinters.Sorted = true;
            this.m_ctrlPrinters.TabIndex = 0;
            // 
            // m_ctrlTemplates
            // 
            this.m_ctrlTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ctrlTemplates.Location = new System.Drawing.Point(8, 20);
            this.m_ctrlTemplates.Name = "m_ctrlTemplates";
            this.m_ctrlTemplates.Size = new System.Drawing.Size(305, 21);
            this.m_ctrlTemplates.TabIndex = 0;
            this.m_ctrlTemplates.SelectedIndexChanged += new System.EventHandler(this.OnTemplateChanged);
            // 
            // m_ctrlPrintPath
            // 
            this.m_ctrlPrintPath.Location = new System.Drawing.Point(169, 24);
            this.m_ctrlPrintPath.Name = "m_ctrlPrintPath";
            this.m_ctrlPrintPath.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlPrintPath.TabIndex = 4;
            this.m_ctrlPrintPath.Text = "Print Full File Path";
            // 
            // m_ctrlPrintCallouts
            // 
            this.m_ctrlPrintCallouts.Location = new System.Drawing.Point(12, 24);
            this.m_ctrlPrintCallouts.Name = "m_ctrlPrintCallouts";
            this.m_ctrlPrintCallouts.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlPrintCallouts.TabIndex = 0;
            this.m_ctrlPrintCallouts.Text = "Print Callouts";
            // 
            // m_ctrlPrintPageTotal
            // 
            this.m_ctrlPrintPageTotal.Location = new System.Drawing.Point(169, 48);
            this.m_ctrlPrintPageTotal.Name = "m_ctrlPrintPageTotal";
            this.m_ctrlPrintPageTotal.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlPrintPageTotal.TabIndex = 5;
            this.m_ctrlPrintPageTotal.Text = "Print Page Total";
            // 
            // m_ctrlForceNewPage
            // 
            this.m_ctrlForceNewPage.Location = new System.Drawing.Point(12, 72);
            this.m_ctrlForceNewPage.Name = "m_ctrlForceNewPage";
            this.m_ctrlForceNewPage.Size = new System.Drawing.Size(142, 20);
            this.m_ctrlForceNewPage.TabIndex = 2;
            this.m_ctrlForceNewPage.Text = "Force New Page";
            this.m_ctrlForceNewPage.Click += new System.EventHandler(this.OnClickForceNewPage);
            // 
            // m_ctrlDoubleSided
            // 
            this.m_ctrlDoubleSided.Location = new System.Drawing.Point(9, 50);
            this.m_ctrlDoubleSided.Name = "m_ctrlDoubleSided";
            this.m_ctrlDoubleSided.Size = new System.Drawing.Size(276, 20);
            this.m_ctrlDoubleSided.TabIndex = 2;
            this.m_ctrlDoubleSided.Text = "New job per document (use for double sided)";
            this.m_ctrlDoubleSided.Click += new System.EventHandler(this.OnClickDoubleSided);
            // 
            // m_ctrlPrinterGroup
            // 
            this.m_ctrlPrinterGroup.Controls.Add(this.m_ctrlPrinterProperties);
            this.m_ctrlPrinterGroup.Controls.Add(this.m_ctrlPrinters);
            this.m_ctrlPrinterGroup.Controls.Add(this.m_ctrlCopiesLabel);
            this.m_ctrlPrinterGroup.Controls.Add(this.m_ctrlCopies);
            this.m_ctrlPrinterGroup.Controls.Add(this.m_ctrlCollate);
            this.m_ctrlPrinterGroup.Controls.Add(this.m_ctrlDoubleSided);
            this.m_ctrlPrinterGroup.Location = new System.Drawing.Point(337, 8);
            this.m_ctrlPrinterGroup.Name = "m_ctrlPrinterGroup";
            this.m_ctrlPrinterGroup.Size = new System.Drawing.Size(322, 107);
            this.m_ctrlPrinterGroup.TabIndex = 3;
            this.m_ctrlPrinterGroup.TabStop = false;
            this.m_ctrlPrinterGroup.Text = "Printer Setup";
            // 
            // m_ctrlPrinterProperties
            // 
            this.m_ctrlPrinterProperties.Location = new System.Drawing.Point(226, 21);
            this.m_ctrlPrinterProperties.Name = "m_ctrlPrinterProperties";
            this.m_ctrlPrinterProperties.Size = new System.Drawing.Size(87, 23);
            this.m_ctrlPrinterProperties.TabIndex = 1;
            this.m_ctrlPrinterProperties.Text = "Properties ...";
            this.m_ctrlPrinterProperties.Click += new System.EventHandler(this.OnClickPrinterProperties);
            // 
            // m_ctrlOptionalGroup
            // 
            this.m_ctrlOptionalGroup.Controls.Add(this.m_ctrlInsertSlipSheet);
            this.m_ctrlOptionalGroup.Controls.Add(this.m_ctrlAutoRotate);
            this.m_ctrlOptionalGroup.Controls.Add(this.m_ctrlPrintCalloutBorders);
            this.m_ctrlOptionalGroup.Controls.Add(this.m_ctrlForceNewPage);
            this.m_ctrlOptionalGroup.Controls.Add(this.m_ctrlPrintPath);
            this.m_ctrlOptionalGroup.Controls.Add(this.m_ctrlPrintCallouts);
            this.m_ctrlOptionalGroup.Controls.Add(this.m_ctrlPrintPageTotal);
            this.m_ctrlOptionalGroup.Location = new System.Drawing.Point(8, 308);
            this.m_ctrlOptionalGroup.Name = "m_ctrlOptionalGroup";
            this.m_ctrlOptionalGroup.Size = new System.Drawing.Size(322, 143);
            this.m_ctrlOptionalGroup.TabIndex = 2;
            this.m_ctrlOptionalGroup.TabStop = false;
            this.m_ctrlOptionalGroup.Text = "Options";
            // 
            // m_ctrlInsertSlipSheet
            // 
            this.m_ctrlInsertSlipSheet.Location = new System.Drawing.Point(12, 96);
            this.m_ctrlInsertSlipSheet.Name = "m_ctrlInsertSlipSheet";
            this.m_ctrlInsertSlipSheet.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlInsertSlipSheet.TabIndex = 3;
            this.m_ctrlInsertSlipSheet.Text = "Insert Slip Sheet";
            this.m_ctrlInsertSlipSheet.Click += new System.EventHandler(this.OnClickInsertSlipSheet);
            // 
            // m_ctrlAutoRotate
            // 
            this.m_ctrlAutoRotate.Location = new System.Drawing.Point(169, 72);
            this.m_ctrlAutoRotate.Name = "m_ctrlAutoRotate";
            this.m_ctrlAutoRotate.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlAutoRotate.TabIndex = 6;
            this.m_ctrlAutoRotate.Text = "Auto Rotate Images";
            // 
            // m_ctrlPrintCalloutBorders
            // 
            this.m_ctrlPrintCalloutBorders.Location = new System.Drawing.Point(12, 48);
            this.m_ctrlPrintCalloutBorders.Name = "m_ctrlPrintCalloutBorders";
            this.m_ctrlPrintCalloutBorders.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlPrintCalloutBorders.TabIndex = 1;
            this.m_ctrlPrintCalloutBorders.Text = "Print Callout Borders";
            // 
            // m_ctrlTemplateGroup
            // 
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlSubstitutionsLabel);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlFieldText);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlFieldSubstitutions);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlFieldTextLabel);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTextFieldsLabel);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTextFieldsList);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateSource);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateForeign);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateFilename);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateDeponent);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateName);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateText);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplatePageNumber);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplates);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateBorder);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateImage);
            this.m_ctrlTemplateGroup.Controls.Add(this.m_ctrlTemplateGraphic);
            this.m_ctrlTemplateGroup.Location = new System.Drawing.Point(337, 120);
            this.m_ctrlTemplateGroup.Name = "m_ctrlTemplateGroup";
            this.m_ctrlTemplateGroup.Size = new System.Drawing.Size(322, 332);
            this.m_ctrlTemplateGroup.TabIndex = 4;
            this.m_ctrlTemplateGroup.TabStop = false;
            this.m_ctrlTemplateGroup.Text = "Template Setup";
            // 
            // m_ctrlSubstitutionsLabel
            // 
            this.m_ctrlSubstitutionsLabel.Location = new System.Drawing.Point(9, 297);
            this.m_ctrlSubstitutionsLabel.Name = "m_ctrlSubstitutionsLabel";
            this.m_ctrlSubstitutionsLabel.Size = new System.Drawing.Size(76, 21);
            this.m_ctrlSubstitutionsLabel.TabIndex = 17;
            this.m_ctrlSubstitutionsLabel.Text = "Substitutions :";
            this.m_ctrlSubstitutionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlFieldText
            // 
            this.m_ctrlFieldText.Location = new System.Drawing.Point(89, 270);
            this.m_ctrlFieldText.Name = "m_ctrlFieldText";
            this.m_ctrlFieldText.Size = new System.Drawing.Size(224, 20);
            this.m_ctrlFieldText.TabIndex = 11;
            // 
            // m_ctrlFieldSubstitutions
            // 
            this.m_ctrlFieldSubstitutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ctrlFieldSubstitutions.Location = new System.Drawing.Point(89, 297);
            this.m_ctrlFieldSubstitutions.Name = "m_ctrlFieldSubstitutions";
            this.m_ctrlFieldSubstitutions.Size = new System.Drawing.Size(224, 21);
            this.m_ctrlFieldSubstitutions.TabIndex = 12;
            this.m_ctrlFieldSubstitutions.SelectedIndexChanged += new System.EventHandler(this.OnFieldSubstitutionChanged);
            // 
            // m_ctrlFieldTextLabel
            // 
            this.m_ctrlFieldTextLabel.Location = new System.Drawing.Point(8, 270);
            this.m_ctrlFieldTextLabel.Name = "m_ctrlFieldTextLabel";
            this.m_ctrlFieldTextLabel.Size = new System.Drawing.Size(76, 21);
            this.m_ctrlFieldTextLabel.TabIndex = 14;
            this.m_ctrlFieldTextLabel.Text = "Text to Print :";
            this.m_ctrlFieldTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlTextFieldsLabel
            // 
            this.m_ctrlTextFieldsLabel.Location = new System.Drawing.Point(7, 166);
            this.m_ctrlTextFieldsLabel.Name = "m_ctrlTextFieldsLabel";
            this.m_ctrlTextFieldsLabel.Size = new System.Drawing.Size(152, 16);
            this.m_ctrlTextFieldsLabel.TabIndex = 12;
            this.m_ctrlTextFieldsLabel.Text = "Text Fields";
            // 
            // m_ctrlTextFieldsList
            // 
            this.m_ctrlTextFieldsList.Location = new System.Drawing.Point(9, 183);
            this.m_ctrlTextFieldsList.Name = "m_ctrlTextFieldsList";
            this.m_ctrlTextFieldsList.Size = new System.Drawing.Size(304, 79);
            this.m_ctrlTextFieldsList.TabIndex = 10;
            this.m_ctrlTextFieldsList.SelectedIndexChanged += new System.EventHandler(this.OnTextFieldChanged);
            // 
            // m_ctrlTemplateSource
            // 
            this.m_ctrlTemplateSource.Location = new System.Drawing.Point(9, 139);
            this.m_ctrlTemplateSource.Name = "m_ctrlTemplateSource";
            this.m_ctrlTemplateSource.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateSource.TabIndex = 4;
            this.m_ctrlTemplateSource.Text = "Source Barcode";
            // 
            // m_ctrlTemplateForeign
            // 
            this.m_ctrlTemplateForeign.Location = new System.Drawing.Point(9, 117);
            this.m_ctrlTemplateForeign.Name = "m_ctrlTemplateForeign";
            this.m_ctrlTemplateForeign.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateForeign.TabIndex = 3;
            this.m_ctrlTemplateForeign.Text = "Foreign Barcode";
            // 
            // m_ctrlTemplateFilename
            // 
            this.m_ctrlTemplateFilename.Location = new System.Drawing.Point(170, 117);
            this.m_ctrlTemplateFilename.Name = "m_ctrlTemplateFilename";
            this.m_ctrlTemplateFilename.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateFilename.TabIndex = 8;
            this.m_ctrlTemplateFilename.Text = "File Name";
            // 
            // m_ctrlTemplateDeponent
            // 
            this.m_ctrlTemplateDeponent.Location = new System.Drawing.Point(170, 139);
            this.m_ctrlTemplateDeponent.Name = "m_ctrlTemplateDeponent";
            this.m_ctrlTemplateDeponent.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateDeponent.TabIndex = 9;
            this.m_ctrlTemplateDeponent.Text = "Video Information";
            // 
            // m_ctrlTemplateName
            // 
            this.m_ctrlTemplateName.Location = new System.Drawing.Point(170, 95);
            this.m_ctrlTemplateName.Name = "m_ctrlTemplateName";
            this.m_ctrlTemplateName.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateName.TabIndex = 7;
            this.m_ctrlTemplateName.Text = "Name";
            // 
            // m_ctrlTemplateText
            // 
            this.m_ctrlTemplateText.Location = new System.Drawing.Point(9, 95);
            this.m_ctrlTemplateText.Name = "m_ctrlTemplateText";
            this.m_ctrlTemplateText.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateText.TabIndex = 2;
            this.m_ctrlTemplateText.Text = "Barcode Text";
            // 
            // m_ctrlTemplatePageNumber
            // 
            this.m_ctrlTemplatePageNumber.Location = new System.Drawing.Point(170, 72);
            this.m_ctrlTemplatePageNumber.Name = "m_ctrlTemplatePageNumber";
            this.m_ctrlTemplatePageNumber.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplatePageNumber.TabIndex = 6;
            this.m_ctrlTemplatePageNumber.Text = "Page Number";
            // 
            // m_ctrlTemplateBorder
            // 
            this.m_ctrlTemplateBorder.Location = new System.Drawing.Point(170, 50);
            this.m_ctrlTemplateBorder.Name = "m_ctrlTemplateBorder";
            this.m_ctrlTemplateBorder.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateBorder.TabIndex = 5;
            this.m_ctrlTemplateBorder.Text = "Cell Border";
            // 
            // m_ctrlTemplateImage
            // 
            this.m_ctrlTemplateImage.Location = new System.Drawing.Point(9, 50);
            this.m_ctrlTemplateImage.Name = "m_ctrlTemplateImage";
            this.m_ctrlTemplateImage.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateImage.TabIndex = 0;
            this.m_ctrlTemplateImage.Text = "Image";
            // 
            // m_ctrlTemplateGraphic
            // 
            this.m_ctrlTemplateGraphic.Location = new System.Drawing.Point(9, 72);
            this.m_ctrlTemplateGraphic.Name = "m_ctrlTemplateGraphic";
            this.m_ctrlTemplateGraphic.Size = new System.Drawing.Size(128, 20);
            this.m_ctrlTemplateGraphic.TabIndex = 1;
            this.m_ctrlTemplateGraphic.Text = "Barcode Graphic";
            // 
            // m_ctrlContentGroup
            // 
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlOnlyLinks);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeLinks);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeTreatments);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlOnlyFirstPage);
            this.m_ctrlContentGroup.Controls.Add(this.m_ctrlOnlyTreatments);
            this.m_ctrlContentGroup.Location = new System.Drawing.Point(8, 148);
            this.m_ctrlContentGroup.Name = "m_ctrlContentGroup";
            this.m_ctrlContentGroup.Size = new System.Drawing.Size(322, 154);
            this.m_ctrlContentGroup.TabIndex = 1;
            this.m_ctrlContentGroup.TabStop = false;
            this.m_ctrlContentGroup.Text = "Print What";
            // 
            // m_ctrlOnlyLinks
            // 
            this.m_ctrlOnlyLinks.Location = new System.Drawing.Point(10, 96);
            this.m_ctrlOnlyLinks.Name = "m_ctrlOnlyLinks";
            this.m_ctrlOnlyLinks.Size = new System.Drawing.Size(295, 20);
            this.m_ctrlOnlyLinks.TabIndex = 3;
            this.m_ctrlOnlyLinks.Text = "Only print document links for selected script scenes";
            this.m_ctrlOnlyLinks.Click += new System.EventHandler(this.OnClickOnlyLinks);
            // 
            // CFPrint
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(668, 491);
            this.Controls.Add(this.m_ctrlContentGroup);
            this.Controls.Add(this.m_ctrlTemplateGroup);
            this.Controls.Add(this.m_ctrlOptionalGroup);
            this.Controls.Add(this.m_ctrlPrinterGroup);
            this.Controls.Add(this.m_ctrlRangeGroup);
            this.Controls.Add(this.m_ctrlDone);
            this.Controls.Add(this.m_ctrlPrint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFPrint";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Print";
            this.m_ctrlRangeGroup.ResumeLayout(false);
            this.m_ctrlRangeGroup.PerformLayout();
            this.m_ctrlPrinterGroup.ResumeLayout(false);
            this.m_ctrlPrinterGroup.PerformLayout();
            this.m_ctrlOptionalGroup.ResumeLayout(false);
            this.m_ctrlTemplateGroup.ResumeLayout(false);
            this.m_ctrlTemplateGroup.PerformLayout();
            this.m_ctrlContentGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>This method exchanges values between the print options and from controls</summary>
        /// <param name="bSetControls">true if options are to be used to set control values</param>
        private void ExchangeOptions(bool bSetControls)
        {
            System.DateTime dtNow = System.DateTime.Now;
            string strPrint = "";

            Debug.Assert(m_tmaxPrintOptions != null);
            if (m_tmaxPrintOptions == null) return;

            //	Are we setting the control states?
            if (bSetControls == true)
            {
                m_ctrlCopies.Text = m_tmaxPrintOptions.Copies.ToString();
                m_ctrlCollate.Checked = m_tmaxPrintOptions.Collate;
                m_ctrlDoubleSided.Checked = m_tmaxPrintOptions.DoubleSided;
                m_ctrlOnlyTreatments.Checked = m_tmaxPrintOptions.OnlyTreatments;
                m_ctrlOnlyFirstPage.Checked = m_tmaxPrintOptions.OnlyFirstPage;
                m_ctrlOnlyLinks.Checked = m_tmaxPrintOptions.OnlyLinks;
                m_ctrlIncludeTreatments.Checked = m_tmaxPrintOptions.IncludeTreatments;
                m_ctrlIncludeSubBinders.Checked = m_tmaxPrintOptions.IncludeSubBinders;
                m_ctrlIncludeLinks.Checked = m_tmaxPrintOptions.IncludeLinks;
                m_ctrlPrintCallouts.Checked = m_tmaxPrintOptions.PrintCallouts;
                m_ctrlPrintCalloutBorders.Checked = m_tmaxPrintOptions.PrintCalloutBorders;
                m_ctrlPrintPageTotal.Checked = m_tmaxPrintOptions.PrintPageTotal;
                m_ctrlPrintPath.Checked = m_tmaxPrintOptions.PrintPath;
                m_ctrlInsertSlipSheet.Checked = m_tmaxPrintOptions.InsertSlipSheet;
                m_ctrlForceNewPage.Checked = (m_tmaxPrintOptions.ForceNewPage || m_tmaxPrintOptions.InsertSlipSheet);

                //	Postpone setting the printer or template until after the list boxes
                //	have been populated

                OnClickOnlyTreatments(m_ctrlOnlyTreatments, System.EventArgs.Empty);
                OnClickDoubleSided(m_ctrlDoubleSided, System.EventArgs.Empty);
                OnClickOnlyLinks(m_ctrlOnlyLinks, System.EventArgs.Empty);
            }
            else
            {
                try
                {
                    m_tmaxPrintOptions.Copies = System.Convert.ToInt32(m_ctrlCopies.Text);
                    if (m_tmaxPrintOptions.Copies <= 0)
                    {
                        m_tmaxPrintOptions.Copies = 1;
                        m_ctrlCopies.Text = "1";
                    }

                }
                catch
                {
                    m_tmaxPrintOptions.Copies = 1;
                }

                m_tmaxPrintOptions.Collate = m_ctrlCollate.Checked;
                m_tmaxPrintOptions.DoubleSided = m_ctrlDoubleSided.Checked;
                m_tmaxPrintOptions.OnlyTreatments = m_ctrlOnlyTreatments.Checked;
                m_tmaxPrintOptions.OnlyFirstPage = m_ctrlOnlyFirstPage.Checked;
                m_tmaxPrintOptions.OnlyLinks = m_ctrlOnlyLinks.Checked;
                m_tmaxPrintOptions.IncludeTreatments = m_ctrlIncludeTreatments.Checked;
                m_tmaxPrintOptions.IncludeSubBinders = m_ctrlIncludeSubBinders.Checked;
                m_tmaxPrintOptions.IncludeLinks = m_ctrlIncludeLinks.Checked;
                m_tmaxPrintOptions.PrintCallouts = m_ctrlPrintCallouts.Checked;
                m_tmaxPrintOptions.PrintCalloutBorders = m_ctrlPrintCalloutBorders.Checked;
                m_tmaxPrintOptions.PrintPageTotal = m_ctrlPrintPageTotal.Checked;
                m_tmaxPrintOptions.PrintPath = m_ctrlPrintPath.Checked;

                if (m_ctrlInsertSlipSheet.Checked == true)
                {
                    m_tmaxPrintOptions.InsertSlipSheet = true;
                    m_tmaxPrintOptions.ForceNewPage = true;

                    if (m_ctrlForceNewPage.Checked == false)
                        m_ctrlForceNewPage.Checked = true;
                }
                else
                {
                    m_tmaxPrintOptions.ForceNewPage = m_ctrlForceNewPage.Checked;
                    m_tmaxPrintOptions.InsertSlipSheet = false;
                }

                m_tmaxPrintOptions.Fields = GetTemplateFlags();

                if ((m_ctrlPrinters.Text != null) && (m_ctrlPrinters.Text.Length > 0))
                    m_tmaxPrintOptions.Printer = m_ctrlPrinters.Text;

                if ((m_ctrlTemplates.Text != null) && (m_ctrlTemplates.Text.Length > 0))
                    m_tmaxPrintOptions.Template = m_ctrlTemplates.Text;

                //	Should we update the current text field?
                if (m_tmaxTextField != null)
                    m_tmaxTextField.Text = m_ctrlFieldText.Text;

                //	Set the print option to match the check states
                for (int i = 0; i < m_tmaxPrintOptions.TextFields.Count; i++)
                {
                    m_tmaxPrintOptions.TextFields[i].Print = m_ctrlTextFieldsList.GetItemChecked(i);

                    //	Are we printing this field?
                    if (m_tmaxPrintOptions.TextFields[i].Print == true)
                    {
                        //	Set the print text
                        strPrint = m_tmaxPrintOptions.TextFields[i].Text;

                        if (strPrint.Length > 0)
                        {
                            //	Format the text for printing
                            strPrint = strPrint.Replace(FIELD_SUBSTITUTE_DATE, dtNow.ToShortDateString());
                            strPrint = strPrint.Replace(FIELD_SUBSTITUTE_TIME, dtNow.ToLongTimeString());
                            strPrint = strPrint.Replace(FIELD_SUBSTITUTE_DATE_TIME, dtNow.ToString());
                        }

                        m_tmaxPrintOptions.TextFields[i].PrintText = strPrint;

                    }
                    else
                    {
                        m_tmaxPrintOptions.TextFields[i].PrintText = "";
                    }

                }// for(int i = 0; i < m_tmaxPrintOptions.TextFields.Count; i++)

            }// if(bSetControls == true)

        }// private void ExchangeOptions(bool bSetControls)

        /// <summary>This method is called when the user clicks on the Print button</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Event arguments</param>
        private void OnClickPrint(object sender, System.EventArgs e)
        {
            bool bSuccessful = false;

            Debug.Assert(m_tmaxPrintOptions != null);
            if (m_tmaxPrintOptions == null) return;

            //	Update the options using the current control settings
            ExchangeOptions(false);

            //	Set the TMPrint properties
            if (SetAxProps() == false) return;

            //	Is the user printing the current selections?
            if (m_ctrlPrintSelections.Checked == true)
                bSuccessful = PrintSelections();
            else
                bSuccessful = PrintBarcodes();

            if (bSuccessful == true)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }

        }// private void OnClickPrint(object sender, System.EventArgs e)

        /// <summary>This methods handles the FirstPrinter event fired by the TMPrint control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxFirstPrinter(object sender, AxTM_PRINT6Lib._DTMPrint6Events_FirstPrinterEvent e)
        {
            if ((m_ctrlPrinters != null) && (m_ctrlPrinters.IsDisposed == false))
            {
                m_ctrlPrinters.Items.Clear();
                m_ctrlPrinters.Items.Add(e.lpszPrinter);
            }

        }// private void OnAxFirstPrinter(object sender, AxTM_PRINT6Lib._DTMPrint6Events_FirstPrinterEvent e)

        /// <summary>This methods handles the NextPrinter event fired by the TMPrint control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxNextPrinter(object sender, AxTM_PRINT6Lib._DTMPrint6Events_NextPrinterEvent e)
        {
            if ((m_ctrlPrinters != null) && (m_ctrlPrinters.IsDisposed == false))
            {
                m_ctrlPrinters.Items.Add(e.lpszPrinter);
            }

        }// private void OnAxNextPrinter(object sender, AxTM_PRINT6Lib._DTMPrint6Events_NextPrinterEvent e)

        /// <summary>This methods handles the FirstTemplate event fired by the TMPrint control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxFirstTemplate(object sender, AxTM_PRINT6Lib._DTMPrint6Events_FirstTemplateEvent e)
        {
            if ((m_ctrlTemplates != null) && (m_ctrlTemplates.IsDisposed == false))
            {
                m_ctrlTemplates.Items.Clear();
                m_ctrlTemplates.Items.Add(e.lpszTemplate);
            }

        }// private void OnAxFirstTemplate(object sender, AxTM_PRINT6Lib._DTMPrint6Events_FirstTemplateEvent e)

        /// <summary>This methods handles the NextTemplate event fired by the TMPrint control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxNextTemplate(object sender, AxTM_PRINT6Lib._DTMPrint6Events_NextTemplateEvent e)
        {
            if ((m_ctrlTemplates != null) && (m_ctrlTemplates.IsDisposed == false))
            {
                m_ctrlTemplates.Items.Add(e.lpszTemplate);
            }

        }// private void OnAxNextTemplate(object sender, AxTM_PRINT6Lib._DTMPrint6Events_NextTemplateEvent e)

        /// <summary>This methods handles the FirstTextField event fired by the TMPrint control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxFirstTextField(object sender, AxTM_PRINT6Lib._DTMPrint6Events_FirstTextFieldEvent e)
        {
            CTmaxPrintField tmaxField = null;

            if ((m_ctrlTextFieldsList != null) && (m_ctrlTextFieldsList.IsDisposed == false))
            {
                //	Search the initial collection if available
                if (m_tmaxInitialTextFields != null)
                {
                    if ((tmaxField = m_tmaxInitialTextFields.Find(e.lId, e.lpszName)) != null)
                    {
                        //	Remove from the initial collection
                        m_tmaxInitialTextFields.Remove(tmaxField);
                    }

                }// if(m_tmaxInitialTextFields != null)

                //	Do we need to allocate a new field?
                if (tmaxField == null)
                {
                    tmaxField = new CTmaxPrintField();

                    tmaxField.Id = e.lId;
                    tmaxField.Name = e.lpszName;
                    tmaxField.Text = e.lpszText;
                    tmaxField.Print = (e.bPrint != 0);
                }

                m_tmaxPrintOptions.TextFields.Add(tmaxField);

            }// if((m_ctrlTextFieldsList != null) && (m_ctrlTextFieldsList.IsDisposed == false))

        }// private void OnAxFirstTextField(object sender, AxTM_PRINT6Lib._DTMPrint6Events_FirstTextFieldEvent e)

        /// <summary>This methods handles the NextTextField event fired by the TMPrint control</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnAxNextTextField(object sender, AxTM_PRINT6Lib._DTMPrint6Events_NextTextFieldEvent e)
        {
            CTmaxPrintField tmaxField = null;

            if ((m_ctrlTextFieldsList != null) && (m_ctrlTextFieldsList.IsDisposed == false))
            {
                //	Search the initial collection if available
                if (m_tmaxInitialTextFields != null)
                {
                    if ((tmaxField = m_tmaxInitialTextFields.Find(e.lId, e.lpszName)) != null)
                    {
                        //	Remove from the initial collection
                        m_tmaxInitialTextFields.Remove(tmaxField);
                    }

                }// if(m_tmaxInitialTextFields != null)

                //	Do we need to allocate a new field?
                if (tmaxField == null)
                {
                    tmaxField = new CTmaxPrintField();

                    tmaxField.Id = e.lId;
                    tmaxField.Name = e.lpszName;
                    tmaxField.Text = e.lpszText;
                    tmaxField.Print = (e.bPrint != 0);
                }

                m_tmaxPrintOptions.TextFields.Add(tmaxField);

            }// if((m_ctrlTextFieldsList != null) && (m_ctrlTextFieldsList.IsDisposed == false))

        }// private void axTMPrint61_NextTextField(object sender, AxTM_PRINT6Lib._DTMPrint6Events_NextTextFieldEvent e)

        /// <summary>This methods handles events fired when the user changes the template selection</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnTemplateChanged(object sender, System.EventArgs e)
        {
            if (m_ctrlTemplates.SelectedIndex >= 0)
            {
                SetFlagsEnabled(m_tmxPrint.GetFieldEnabledMask(m_ctrlTemplates.Text));
                SetFieldsChecked(m_tmxPrint.GetFieldDefaultMask(m_ctrlTemplates.Text));
            }
            else
            {
                SetFieldsChecked(0);
                SetFlagsEnabled(0);
            }

            //	Fill the text fields list
            FillTextFields();

        }// private void OnTemplateChanged(object sender, System.EventArgs e)

        /// <summary>This method is called when the user clicks on the OnlyTreatments check box</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Event arguments</param>
        private void OnClickOnlyTreatments(object sender, System.EventArgs e)
        {
            m_ctrlIncludeTreatments.Enabled = (m_ctrlOnlyTreatments.Checked == false);
        }

        /// <summary>This method is called when the user clicks on the OnlyLinks check box</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Event arguments</param>
        private void OnClickOnlyLinks(object sender, System.EventArgs e)
        {
            m_ctrlIncludeLinks.Enabled = (m_ctrlOnlyLinks.Checked == false);
        }

        /// <summary>This method is called when the user clicks on the Double Sided check box</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Event arguments</param>
        private void OnClickDoubleSided(object sender, System.EventArgs e)
        {
            m_ctrlForceNewPage.Enabled = (m_ctrlDoubleSided.Checked == false);
            m_ctrlInsertSlipSheet.Enabled = (m_ctrlDoubleSided.Checked == false);
            m_ctrlCollate.Enabled = (m_ctrlDoubleSided.Checked == false);
        }

        /// <summary>This method is called when the user clicks on the Insert Slip Sheet check box</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Event arguments</param>
        private void OnClickInsertSlipSheet(object sender, System.EventArgs e)
        {
            if (m_ctrlInsertSlipSheet.Checked == true)
                m_ctrlForceNewPage.Checked = true;
        }

        /// <summary>This method is called when the user clicks on the Force New Page check box</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Event arguments</param>
        private void OnClickForceNewPage(object sender, System.EventArgs e)
        {
            if (m_ctrlForceNewPage.Checked == false)
                m_ctrlInsertSlipSheet.Checked = false;
        }

        /// <summary>This method is called when the user clicks on the Properties button</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Event arguments</param>
        private void OnClickPrinterProperties(object sender, System.EventArgs e)
        {
            if ((m_ctrlPrinters.Text != null) && (m_ctrlPrinters.Text.Length > 0))
            {
                //	Get the current control values
                ExchangeOptions(false);

                //	Set the TMPrint properties
                m_tmxPrint.Printer = m_tmaxPrintOptions.Printer;
                m_tmxPrint.Copies = (short)m_tmaxPrintOptions.Copies;
                m_tmxPrint.Collate = m_tmaxPrintOptions.Collate;
                m_tmxPrint.TemplateName = m_tmaxPrintOptions.Template;

                if (m_tmxPrint.SetPrinterProperties((int)(this.Handle)) == true)
                {
                    m_tmaxPrintOptions.Printer = m_tmxPrint.Printer;
                    m_tmaxPrintOptions.Copies = m_tmxPrint.Copies;
                    m_tmaxPrintOptions.Collate = m_tmxPrint.Collate;

                    ExchangeOptions(true);
                }

            }
            else
            {
                FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONEXCLAMATION);
            }

        }// private void OnClickPrinterProperties(object sender, System.EventArgs e)

        /// <summary>Called when the user changes the selection in the list of text fields</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event arguments</param>
        private void OnTextFieldChanged(object sender, System.EventArgs e)
        {
            int iIndex = -1;

            //	Should we update the current text field?
            if (m_tmaxTextField != null)
            {
                m_tmaxTextField.Text = m_ctrlFieldText.Text;
                m_tmaxTextField = null;
            }

            if ((iIndex = m_ctrlTextFieldsList.SelectedIndex) >= 0)
            {
                if (iIndex < m_tmaxPrintOptions.TextFields.Count)
                    m_tmaxTextField = m_tmaxPrintOptions.TextFields[iIndex];
            }

            if (m_tmaxTextField != null)
            {
                m_ctrlFieldText.Text = m_tmaxTextField.Text;
                m_ctrlFieldText.Enabled = true;
                m_ctrlFieldSubstitutions.Enabled = true;
            }
            else
            {
                m_ctrlFieldText.Text = "";
                m_ctrlFieldText.Enabled = false;
                m_ctrlFieldSubstitutions.Enabled = false;
            }

        }// private void OnTextFieldChanged(object sender, System.EventArgs e)

        /// <summary>Called when the user changes the selection in the list of text substitutions</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event arguments</param>
        private void OnFieldSubstitutionChanged(object sender, System.EventArgs e)
        {
            int iIndex = -1;
            string strText = "";

            try
            {
                if ((iIndex = m_ctrlFieldSubstitutions.SelectedIndex) >= 0)
                    strText = ((CFieldSubstitution)(m_ctrlFieldSubstitutions.Items[iIndex])).PlaceHolder;

                if (strText.Length > 0)
                {
                    m_ctrlFieldText.Text = strText;
                    m_ctrlFieldText.Focus();
                    m_ctrlFieldText.SelectAll();
                }
            }
            catch
            {
            }

        }// private void OnFieldSubstitutionChanged(object sender, System.EventArgs e)

        /// <summary>Called to populate the drop list of available text field substitutions</summary>
        private void FillFieldSubstitutions()
        {
            ArrayList aSubstitutions = null;

            try
            {
                //	Populate the list of available substitutions
                aSubstitutions = new ArrayList();
                aSubstitutions.Add(new CFieldSubstitution(FIELD_SUBSTITUTE_DATE, "Current Date"));
                aSubstitutions.Add(new CFieldSubstitution(FIELD_SUBSTITUTE_TIME, "Current Time"));
                aSubstitutions.Add(new CFieldSubstitution(FIELD_SUBSTITUTE_DATE_TIME, "Date & Time"));

                //	Attach to the drop list control
                m_ctrlFieldSubstitutions.DataSource = aSubstitutions;
                m_ctrlFieldSubstitutions.DisplayMember = "DisplayAs";
                m_ctrlFieldSubstitutions.ValueMember = "";

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "FillFieldSubstitutions", m_tmaxErrorBuilder.Message(ERROR_FILL_SUBSTITUTIONS_EX), Ex);
            }

        }// private void FillFieldSubstitutions()

        #endregion Private Methods

        #region Properties

        /// <summary>TMPrint ActiveX control used to implement printing operation</summary>
        ///
        ///	<remarks>
        ///			We could make the TMPrint control a child of this form but then we 
        ///			would have to make the form non-modal. By making TMPrint a child of
        ///			the main form we can retain the printer properties and use this
        ///			as a modal form
        /// </remarks>
        public AxTM_PRINT6Lib.AxTMPrint6 TmxPrint
        {
            get { return m_tmxPrint; }
            set { m_tmxPrint = value; }
        }

        /// <summary>The application's active database</summary>
        public FTI.Trialmax.Database.CTmaxCaseDatabase Database
        {
            get { return m_tmaxDatabase; }
            set { m_tmaxDatabase = value; }
        }

        /// <summary>Event items that represent the current selections</summary>
        public FTI.Shared.Trialmax.CTmaxItems Items
        {
            get { return m_tmaxItems; }
            set { m_tmaxItems = value; }
        }

        /// <summary>Print options to use to initialize the form</summary>
        public FTI.Shared.Trialmax.CTmaxPrintOptions PrintOptions
        {
            get { return m_tmaxPrintOptions; }
            set { m_tmaxPrintOptions = value; }
        }

        /// <summary>The collection of user-defined options used by TmaxPresentation</summary>
        public FTI.Shared.Trialmax.CTmaxPresentationOptions PresentationOptions
        {
            get { return m_tmaxPresentationOptions; }
            set { m_tmaxPresentationOptions = value; }
        }

        /// <summary>Name of the file containing the template descriptors</summary>
        public string TemplateFilename
        {
            get { return m_strTemplateFilename; }
            set { m_strTemplateFilename = value; }
        }

        #endregion Properties

        #region Nested Classes

        //	This class is used to manage multiple print jobs
        public class CPrintJob : System.Collections.ArrayList
        {
            #region Private Members

            private string m_strName = "";

            #endregion Private Members

            #region Public Methods

            /// <summary>Constructor</summary>
            public CPrintJob()
                : base()
            {
            }

            #endregion Public Methods

            #region Properties

            /// <summary>Name used to identify the job in the print queue</summary>
            public string Name
            {
                get { return m_strName; }
                set { m_strName = value; }
            }

            #endregion Properties

        }// public class CPrintJob : System.Collections.ArrayList

        //	This class is used to display the available text field substitutions
        public class CFieldSubstitution
        {
            #region Private Members

            private string m_strPlaceHolder = "";

            private string m_strDescription = "";

            #endregion Private Members

            #region Public Methods

            /// <summary>Constructor</summary>
            public CFieldSubstitution(string strPlaceHolder, string strDescription)
            {
                m_strPlaceHolder = strPlaceHolder;
                m_strDescription = strDescription;
            }

            public override string ToString()
            {
                return (this.PlaceHolder);
            }

            #endregion Public Methods

            #region Properties

            /// <summary>PlaceHolder characters</summary>
            public string PlaceHolder
            {
                get { return m_strPlaceHolder; }
            }

            /// <summary>Description of the substitution</summary>
            public string Description
            {
                get { return m_strDescription; }
            }

            /// <summary>Display string</summary>
            public string DisplayAs
            {
                get { return (this.PlaceHolder + " - " + this.Description); }
            }

            #endregion Properties

        }

        #endregion Nested Classes

    }// public class CFPrint : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Panes
