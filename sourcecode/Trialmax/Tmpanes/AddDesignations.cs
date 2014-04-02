using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Shared.Win32;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Panes
{
	/// <summary>This class manages a form that allows the user to add new video designations</summary>
	public class CFAddDesignations : FTI.Trialmax.Forms.CFTmaxBaseForm 
	{
		#region Constants
		
		protected const int ERROR_TRANSCRIPT_NOT_FOUND			= (ERROR_TMAX_FORM_MAX + 1);
		protected const int ERROR_INVALID_XML_FILESPEC			= (ERROR_TMAX_FORM_MAX + 2);
		protected const int ERROR_GET_XML_DEPOSITION_EX			= (ERROR_TMAX_FORM_MAX + 3);
		protected const int	ERROR_INVALID_START_PAGE			= (ERROR_TMAX_FORM_MAX + 4);
		protected const int	ERROR_INVALID_START_LINE			= (ERROR_TMAX_FORM_MAX + 5);
		protected const int	ERROR_INVALID_STOP_PAGE				= (ERROR_TMAX_FORM_MAX + 6);
		protected const int	ERROR_INVALID_STOP_LINE				= (ERROR_TMAX_FORM_MAX + 7);
		protected const int ERROR_START_LINE_OUT_OF_RANGE		= (ERROR_TMAX_FORM_MAX + 8);
		protected const int ERROR_STOP_LINE_OUT_OF_RANGE		= (ERROR_TMAX_FORM_MAX + 9);
		protected const int ERROR_START_PL_OUT_OF_RANGE			= (ERROR_TMAX_FORM_MAX + 10);
		protected const int ERROR_STOP_PL_OUT_OF_RANGE			= (ERROR_TMAX_FORM_MAX + 11);
		protected const int ERROR_PL_REVERSED					= (ERROR_TMAX_FORM_MAX + 12);
		protected const int ERROR_ADD_DESIGNATION_EX			= (ERROR_TMAX_FORM_MAX + 13);
		protected const int ERROR_ADD_DESIGNATIONS_EX			= (ERROR_TMAX_FORM_MAX + 14);
		protected const int ERROR_ADD_SCENES_EX					= (ERROR_TMAX_FORM_MAX + 15);
		protected const int ERROR_SEGMENT_NOT_FOUND				= (ERROR_TMAX_FORM_MAX + 16);
		protected const int ERROR_NO_EDIT_SCRIPT				= (ERROR_TMAX_FORM_MAX + 17);
		protected const int ERROR_NO_EDIT_SCENE					= (ERROR_TMAX_FORM_MAX + 18);
		protected const int ERROR_NO_EDIT_DESIGNATION			= (ERROR_TMAX_FORM_MAX + 19);
		protected const int ERROR_NO_EDIT_XML_DESIGNATION		= (ERROR_TMAX_FORM_MAX + 20);
		protected const int ERROR_INVALID_EDIT_DEPOSITION		= (ERROR_TMAX_FORM_MAX + 21);
		protected const int ERROR_INVALID_EDIT_EXTENTS			= (ERROR_TMAX_FORM_MAX + 22);
		protected const int ERROR_IMPORT_EX						= (ERROR_TMAX_FORM_MAX + 23);
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Button to add new designations</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Button to close the form</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Script text descriptor label</summary>
		private System.Windows.Forms.Label m_ctrlScriptLabel;
		
		/// <summary>Text control to display script description/summary>
		private System.Windows.Forms.Label m_ctrlScript;
		
		/// <summary>Deposition combobox label</summary>
		private System.Windows.Forms.Label m_ctrlDepositionLabel;
		
		/// <summary>Deposition combobox</summary>
		private System.Windows.Forms.ComboBox m_ctrlDepositions;
		
		/// <summary>Minimum extents text control label</summary>
		private System.Windows.Forms.Label m_ctrlMinimumLabel;
		
		/// <summary>Maximum extents text control label</summary>
		private System.Windows.Forms.Label m_ctrlMaximumLabel;
		
		/// <summary>Deposition line number text label</summary>
		private System.Windows.Forms.Label m_ctrlDepositionLineLabel;
		
		/// <summary>Deposition page number text label</summary>
		private System.Windows.Forms.Label m_ctrlDepositionPageLabel;
		
		/// <summary>Minimum line number text label</summary>
		private System.Windows.Forms.Label m_ctrlMinimumLine;
		
		/// <summary>Minimum page number text label</summary>
		private System.Windows.Forms.Label m_ctrlMinimumPage;
		
		/// <summary>Maximum line number text label</summary>
		private System.Windows.Forms.Label m_ctrlMaximumLine;
		
		/// <summary>Maximum page number text label</summary>
		private System.Windows.Forms.Label m_ctrlMaximumPage;
		
		/// <summary>Stop extents text label</summary>
		private System.Windows.Forms.Label m_ctrlStopLabel;
		
		/// <summary>Start extents text label</summary>
		private System.Windows.Forms.Label m_ctrlStartLabel;
		
		/// <summary>Start page number control</summary>
		private System.Windows.Forms.TextBox m_ctrlStartPage;
		
		/// <summary>Stop page number control</summary>
		private System.Windows.Forms.TextBox m_ctrlStopPage;
		
		/// <summary>Stop line number control</summary>
		private System.Windows.Forms.TextBox m_ctrlStopLine;
		
		/// <summary>Start line number control</summary>
		private System.Windows.Forms.TextBox m_ctrlStartLine;
		
		/// <summary>Import button</summary>
		private System.Windows.Forms.Button m_ctrlImport;
		
		/// <summary>Image list used for form buttons</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Combobox to display available highlighters</summary>
		private System.Windows.Forms.ComboBox m_ctrlHighlighters;
		
		/// <summary>Highlighters text label control</summary>
		private System.Windows.Forms.Label m_ctrlHighlightersLabel;
		
		/// <summary>Local member bound to Database property</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member bound to Script property</summary>
		private CDxPrimary m_dxScript = null;
		
		/// <summary>Local member to store reference to active deposition record</summary>
		private CDxPrimary m_dxDeposition = null;
		
		/// <summary>Local member to store reference to active transcript record</summary>
		private CDxTranscript m_dxTranscript = null;
		
		/// <summary>Local member bound to Scene property</summary>
		private CDxSecondary m_dxScene = null;
		
		/// <summary>Local member to keep track of the active Edit designation</summary>
		private CDxTertiary m_dxDesignation = null;
		
		/// <summary>Local member bound to Highlighter property</summary>
		private CDxHighlighter m_dxHighlighter = null;
		
		/// <summary>Local member bound to EditExtents property</summary>
		private bool m_bEditExtents = false;
		
		/// <summary>Local member bound to InsertBefore property</summary>
		private bool m_bInsertBefore = false;
		
		/// <summary>Local member to keep track of XML deposition success</summary>
		private bool m_bXmlFailed = false;
		
		/// <summary>Local member to store the start page number</summary>
		private long m_lStartPage = -1;
		
		/// <summary>Local member to store the start line number</summary>
		private int m_iStartLine = -1;
		
		/// <summary>Local member to store the stop page number</summary>
		private long m_lStopPage = -1;
		
		/// <summary>Local member to store the stop line number</summary>
		private int m_iStopLine = -1;
		
		/// <summary>Local member to store the start PL</summary>
		private long m_lStartPL = -1;
		
		/// <summary>Local member to store the stop PL</summary>
		private long m_lStopPL = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFAddDesignations()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			//	Populate the error builder's format strings
			SetErrorStrings();
		}

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

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFAddDesignations));
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlScriptLabel = new System.Windows.Forms.Label();
			this.m_ctrlScript = new System.Windows.Forms.Label();
			this.m_ctrlDepositionLabel = new System.Windows.Forms.Label();
			this.m_ctrlDepositions = new System.Windows.Forms.ComboBox();
			this.m_ctrlMinimumLabel = new System.Windows.Forms.Label();
			this.m_ctrlMaximumLabel = new System.Windows.Forms.Label();
			this.m_ctrlDepositionLineLabel = new System.Windows.Forms.Label();
			this.m_ctrlDepositionPageLabel = new System.Windows.Forms.Label();
			this.m_ctrlMinimumLine = new System.Windows.Forms.Label();
			this.m_ctrlMinimumPage = new System.Windows.Forms.Label();
			this.m_ctrlMaximumLine = new System.Windows.Forms.Label();
			this.m_ctrlMaximumPage = new System.Windows.Forms.Label();
			this.m_ctrlStopLabel = new System.Windows.Forms.Label();
			this.m_ctrlStartLabel = new System.Windows.Forms.Label();
			this.m_ctrlStartPage = new System.Windows.Forms.TextBox();
			this.m_ctrlStopPage = new System.Windows.Forms.TextBox();
			this.m_ctrlStopLine = new System.Windows.Forms.TextBox();
			this.m_ctrlStartLine = new System.Windows.Forms.TextBox();
			this.m_ctrlImport = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlHighlighters = new System.Windows.Forms.ComboBox();
			this.m_ctrlHighlightersLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Location = new System.Drawing.Point(110, 220);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 6;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(212, 220);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 7;
			this.m_ctrlCancel.TabStop = false;
			this.m_ctrlCancel.Text = "&Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlScriptLabel
			// 
			this.m_ctrlScriptLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlScriptLabel.Name = "m_ctrlScriptLabel";
			this.m_ctrlScriptLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlScriptLabel.TabIndex = 2;
			this.m_ctrlScriptLabel.Text = "Script:";
			this.m_ctrlScriptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlScript
			// 
			this.m_ctrlScript.Location = new System.Drawing.Point(112, 8);
			this.m_ctrlScript.Name = "m_ctrlScript";
			this.m_ctrlScript.Size = new System.Drawing.Size(176, 12);
			this.m_ctrlScript.TabIndex = 3;
			this.m_ctrlScript.Text = "script name here";
			this.m_ctrlScript.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDepositionLabel
			// 
			this.m_ctrlDepositionLabel.Location = new System.Drawing.Point(8, 64);
			this.m_ctrlDepositionLabel.Name = "m_ctrlDepositionLabel";
			this.m_ctrlDepositionLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlDepositionLabel.TabIndex = 4;
			this.m_ctrlDepositionLabel.Text = "Deposition:";
			this.m_ctrlDepositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDepositions
			// 
			this.m_ctrlDepositions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlDepositions.IntegralHeight = false;
			this.m_ctrlDepositions.Location = new System.Drawing.Point(112, 60);
			this.m_ctrlDepositions.Name = "m_ctrlDepositions";
			this.m_ctrlDepositions.Size = new System.Drawing.Size(176, 21);
			this.m_ctrlDepositions.TabIndex = 9;
			this.m_ctrlDepositions.TabStop = false;
			this.m_ctrlDepositions.SelectedIndexChanged += new System.EventHandler(this.OnDepositionSelChanged);
			// 
			// m_ctrlMinimumLabel
			// 
			this.m_ctrlMinimumLabel.Location = new System.Drawing.Point(8, 116);
			this.m_ctrlMinimumLabel.Name = "m_ctrlMinimumLabel";
			this.m_ctrlMinimumLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlMinimumLabel.TabIndex = 6;
			this.m_ctrlMinimumLabel.Text = "Minimum:";
			this.m_ctrlMinimumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMaximumLabel
			// 
			this.m_ctrlMaximumLabel.Location = new System.Drawing.Point(8, 136);
			this.m_ctrlMaximumLabel.Name = "m_ctrlMaximumLabel";
			this.m_ctrlMaximumLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlMaximumLabel.TabIndex = 7;
			this.m_ctrlMaximumLabel.Text = "Maximum:";
			this.m_ctrlMaximumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDepositionLineLabel
			// 
			this.m_ctrlDepositionLineLabel.Location = new System.Drawing.Point(212, 96);
			this.m_ctrlDepositionLineLabel.Name = "m_ctrlDepositionLineLabel";
			this.m_ctrlDepositionLineLabel.Size = new System.Drawing.Size(75, 12);
			this.m_ctrlDepositionLineLabel.TabIndex = 9;
			this.m_ctrlDepositionLineLabel.Text = "Line";
			this.m_ctrlDepositionLineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlDepositionPageLabel
			// 
			this.m_ctrlDepositionPageLabel.Location = new System.Drawing.Point(116, 96);
			this.m_ctrlDepositionPageLabel.Name = "m_ctrlDepositionPageLabel";
			this.m_ctrlDepositionPageLabel.Size = new System.Drawing.Size(75, 12);
			this.m_ctrlDepositionPageLabel.TabIndex = 8;
			this.m_ctrlDepositionPageLabel.Text = "Page";
			this.m_ctrlDepositionPageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlMinimumLine
			// 
			this.m_ctrlMinimumLine.Location = new System.Drawing.Point(212, 116);
			this.m_ctrlMinimumLine.Name = "m_ctrlMinimumLine";
			this.m_ctrlMinimumLine.Size = new System.Drawing.Size(75, 12);
			this.m_ctrlMinimumLine.TabIndex = 11;
			this.m_ctrlMinimumLine.Text = "0";
			this.m_ctrlMinimumLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlMinimumPage
			// 
			this.m_ctrlMinimumPage.Location = new System.Drawing.Point(116, 116);
			this.m_ctrlMinimumPage.Name = "m_ctrlMinimumPage";
			this.m_ctrlMinimumPage.Size = new System.Drawing.Size(75, 12);
			this.m_ctrlMinimumPage.TabIndex = 10;
			this.m_ctrlMinimumPage.Text = "0";
			this.m_ctrlMinimumPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlMaximumLine
			// 
			this.m_ctrlMaximumLine.Location = new System.Drawing.Point(212, 136);
			this.m_ctrlMaximumLine.Name = "m_ctrlMaximumLine";
			this.m_ctrlMaximumLine.Size = new System.Drawing.Size(75, 12);
			this.m_ctrlMaximumLine.TabIndex = 13;
			this.m_ctrlMaximumLine.Text = "0";
			this.m_ctrlMaximumLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlMaximumPage
			// 
			this.m_ctrlMaximumPage.Location = new System.Drawing.Point(116, 136);
			this.m_ctrlMaximumPage.Name = "m_ctrlMaximumPage";
			this.m_ctrlMaximumPage.Size = new System.Drawing.Size(75, 12);
			this.m_ctrlMaximumPage.TabIndex = 12;
			this.m_ctrlMaximumPage.Text = "0";
			this.m_ctrlMaximumPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlStopLabel
			// 
			this.m_ctrlStopLabel.Location = new System.Drawing.Point(8, 192);
			this.m_ctrlStopLabel.Name = "m_ctrlStopLabel";
			this.m_ctrlStopLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlStopLabel.TabIndex = 15;
			this.m_ctrlStopLabel.Text = "Stop:";
			this.m_ctrlStopLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStartLabel
			// 
			this.m_ctrlStartLabel.Location = new System.Drawing.Point(8, 164);
			this.m_ctrlStartLabel.Name = "m_ctrlStartLabel";
			this.m_ctrlStartLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlStartLabel.TabIndex = 14;
			this.m_ctrlStartLabel.Text = "Start:";
			this.m_ctrlStartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStartPage
			// 
			this.m_ctrlStartPage.AcceptsReturn = true;
			this.m_ctrlStartPage.Location = new System.Drawing.Point(116, 160);
			this.m_ctrlStartPage.Name = "m_ctrlStartPage";
			this.m_ctrlStartPage.Size = new System.Drawing.Size(75, 20);
			this.m_ctrlStartPage.TabIndex = 0;
			this.m_ctrlStartPage.Text = "";
			this.m_ctrlStartPage.WordWrap = false;
			this.m_ctrlStartPage.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStopPage
			// 
			this.m_ctrlStopPage.AcceptsReturn = true;
			this.m_ctrlStopPage.Location = new System.Drawing.Point(116, 188);
			this.m_ctrlStopPage.Name = "m_ctrlStopPage";
			this.m_ctrlStopPage.Size = new System.Drawing.Size(75, 20);
			this.m_ctrlStopPage.TabIndex = 2;
			this.m_ctrlStopPage.Text = "";
			this.m_ctrlStopPage.WordWrap = false;
			this.m_ctrlStopPage.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStopLine
			// 
			this.m_ctrlStopLine.AcceptsReturn = true;
			this.m_ctrlStopLine.Location = new System.Drawing.Point(212, 188);
			this.m_ctrlStopLine.Name = "m_ctrlStopLine";
			this.m_ctrlStopLine.Size = new System.Drawing.Size(75, 20);
			this.m_ctrlStopLine.TabIndex = 3;
			this.m_ctrlStopLine.Text = "";
			this.m_ctrlStopLine.WordWrap = false;
			this.m_ctrlStopLine.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStartLine
			// 
			this.m_ctrlStartLine.AcceptsReturn = true;
			this.m_ctrlStartLine.Location = new System.Drawing.Point(212, 160);
			this.m_ctrlStartLine.Name = "m_ctrlStartLine";
			this.m_ctrlStartLine.Size = new System.Drawing.Size(75, 20);
			this.m_ctrlStartLine.TabIndex = 1;
			this.m_ctrlStartLine.Text = "";
			this.m_ctrlStartLine.WordWrap = false;
			this.m_ctrlStartLine.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlImport
			// 
			this.m_ctrlImport.Location = new System.Drawing.Point(8, 220);
			this.m_ctrlImport.Name = "m_ctrlImport";
			this.m_ctrlImport.Size = new System.Drawing.Size(64, 23);
			this.m_ctrlImport.TabIndex = 5;
			this.m_ctrlImport.TabStop = false;
			this.m_ctrlImport.Text = "&Import";
			this.m_ctrlImport.Click += new System.EventHandler(this.OnClickImport);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlHighlighters
			// 
			this.m_ctrlHighlighters.DisplayMember = "DisplayString";
			this.m_ctrlHighlighters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlHighlighters.IntegralHeight = false;
			this.m_ctrlHighlighters.Location = new System.Drawing.Point(111, 28);
			this.m_ctrlHighlighters.Name = "m_ctrlHighlighters";
			this.m_ctrlHighlighters.Size = new System.Drawing.Size(177, 21);
			this.m_ctrlHighlighters.TabIndex = 8;
			this.m_ctrlHighlighters.TabStop = false;
			// 
			// m_ctrlHighlightersLabel
			// 
			this.m_ctrlHighlightersLabel.Location = new System.Drawing.Point(7, 32);
			this.m_ctrlHighlightersLabel.Name = "m_ctrlHighlightersLabel";
			this.m_ctrlHighlightersLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlHighlightersLabel.TabIndex = 21;
			this.m_ctrlHighlightersLabel.Text = "Highlighter:";
			this.m_ctrlHighlightersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFAddDesignations
			// 
			this.AcceptButton = null;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(294, 251);
			this.Controls.Add(this.m_ctrlHighlighters);
			this.Controls.Add(this.m_ctrlHighlightersLabel);
			this.Controls.Add(this.m_ctrlStopLine);
			this.Controls.Add(this.m_ctrlStartLine);
			this.Controls.Add(this.m_ctrlStopPage);
			this.Controls.Add(this.m_ctrlStartPage);
			this.Controls.Add(this.m_ctrlImport);
			this.Controls.Add(this.m_ctrlStopLabel);
			this.Controls.Add(this.m_ctrlStartLabel);
			this.Controls.Add(this.m_ctrlMaximumLine);
			this.Controls.Add(this.m_ctrlMaximumPage);
			this.Controls.Add(this.m_ctrlMinimumLine);
			this.Controls.Add(this.m_ctrlMinimumPage);
			this.Controls.Add(this.m_ctrlDepositionLineLabel);
			this.Controls.Add(this.m_ctrlDepositionPageLabel);
			this.Controls.Add(this.m_ctrlMaximumLabel);
			this.Controls.Add(this.m_ctrlMinimumLabel);
			this.Controls.Add(this.m_ctrlDepositions);
			this.Controls.Add(this.m_ctrlDepositionLabel);
			this.Controls.Add(this.m_ctrlScript);
			this.Controls.Add(this.m_ctrlScriptLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddDesignations";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add Designations";
			this.ResumeLayout(false);

		}

		/// <summary>Called when the form's Load event is trapped</summary>
		/// <param name="e">system event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if(m_dxScript != null)
				m_ctrlScript.Text = m_dxScript.GetText();
				
			//	Populate the comboboxes
			FillHighlighters();
			FillDepositions();
			
			//	Are we editing extents?
			if(EditExtents == true)
			{
				if(PrepareForEdit() == false)
				{
					//	Close the form
					DialogResult = DialogResult.Abort;
					this.Close();
				}
				
			}
			else
			{
				//	Rename the action buttons
				m_ctrlOK.Text = "&Add";
				m_ctrlCancel.Text = "&Done";
				this.AcceptButton = m_ctrlOK;
			}
		
			base.OnLoad (e);

		}// protected void OnLoad(object sender, System.EventArgs e)

		/// <summary>Called when the form's Closing event is trapped</summary>
		/// <param name="e">cancellable system event arguments</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			//	Make sure all XML depositions allocated by this form get closed
			foreach(CDxPrimary dxDeposition in m_ctrlDepositions.Items)
			{
				if((dxDeposition.Transcript != null) && (dxDeposition.Transcript.XmlDeposition != null))
				{
					//	Was this transcript allocated by this form?
					if(ReferenceEquals(this, dxDeposition.Transcript.XmlDeposition.UserData) == true)
					{
						dxDeposition.Transcript.XmlDeposition.Close(true);
						dxDeposition.Transcript.XmlDeposition = null;
					}
				}	
			}
			
			base.OnClosing (e);

		}// protected void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve the transcript record associated with the primary deposition: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve a valid transcript file specification for the primary deposition: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to open the XML deposition: MediaId = %1 Filename = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid start page value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid start line value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid stop page value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid stop line value.");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified start line is out of range. The maximum lines per page is %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified stop line is out of range. The maximum lines per page is %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified start page/line is outside the range allowed for the transcript");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified stop page/line is outside the range allowed for the transcript");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified stop page/line must appear after the specified start page/line values");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the new designation: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the new designations to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the new script scenes.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to add the designation. It's segment record could not be found. %1 - segment key = %2");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to edit the extents. No script has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to edit the extents. No script scene has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to edit the extents. No designation has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to edit the extents. No XML designation has been provided.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to edit the extents. Could not locate the parent deposition: deposition = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to initilize the extents controls using the specified designation: FirstPL = %1 LastPL = %2");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the import operation");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Called when the user clicks on the Add button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			//	Are we editing extents?
			if(EditExtents == true)
			{
				OnClickEdit();
			}
			else
			{
				OnClickAdd();
			}
			
		}// private void OnClickOK(object sender, System.EventArgs e)

		/// <summary>Called when the user clicks on the Done button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		/// <summary>Called when the user clicks on the Add button</summary>
		private void OnClickAdd()
		{
			CXmlDesignations	xmlDesignations = null;
			string				strMsg = "";
			bool				bSuccessful = false;
			int					iHighlighter = 0;
			
			//	Do we have an active deposition?
			Debug.Assert(m_dxDeposition != null);
			Debug.Assert(m_dxTranscript != null);
			if((m_dxDeposition == null) || (m_dxTranscript == null)) 
				return;
				
			Cursor.Current = Cursors.WaitCursor;
		
			while(bSuccessful == false)
			{
				//	Check the user supplied values
				if(CheckRange() == false) 
					break;
			
				//	Make sure we have the xml deposition
				if(GetXmlDeposition() == null)
					break;
			
				//	Get the highlighter to be used for the new designations
				if(GetHighlighter() != null)
				{
					iHighlighter = (int)(GetHighlighter().AutoId);
				}
				else if(m_tmaxDatabase.Highlighters.Count > 0)
				{
					iHighlighter = (int)(m_tmaxDatabase.Highlighters[0].AutoId);
				}
				
				//	Create the designations
				xmlDesignations = new CXmlDesignations();
				if(m_dxTranscript.XmlDeposition.CreateDesignations(xmlDesignations, m_lStartPL, m_lStopPL, iHighlighter) == false)
				{
					xmlDesignations.Clear();
					break;
				}
			
				//	Prompt for confirmation if more than one designation
				if(xmlDesignations.Count > 1)
				{
					strMsg = String.Format("The current selection will result in {0} designations\n\n", xmlDesignations.Count);
						
					foreach(CXmlDesignation O in xmlDesignations)
						strMsg += (O.Name + "\n");
							
					strMsg += "\nDo you want to continue?";
						
					//	Prompt the user for confirmation before continuing
					if(MessageBox.Show(strMsg, "Please Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						xmlDesignations.Clear();
						xmlDesignations = null;
						break;
					}
			
				}// if(xmlDesignations.Count > 1)
				
				//	Add the designations to the database
				AddDesignations(xmlDesignations, true);
				
				//	Looks good
				bSuccessful = true;
				
			}// while(bSuccessful == false)
					
			Cursor.Current = Cursors.Default;
			
			Select(m_ctrlStartPage);
			
		}// private void OnClickAdd()

		/// <summary>This method will populate the list of highlighters</summary>
		private void FillHighlighters()
		{
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Highlighters != null);
			
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			{
				foreach(CDxHighlighter O in m_tmaxDatabase.Highlighters)
				{
					//	Has this highlighter been assigned?
					if((O.Name != null) && (O.Name.Length > 0))
					{
						m_ctrlHighlighters.Items.Add(O);
					}
					
				}
				
				//	Select the first highlighter
				if((EditExtents == false) && (m_ctrlHighlighters.Items.Count > 0))
				{
					if(m_dxHighlighter != null)
					{
						if(m_ctrlHighlighters.Items.Contains(m_dxHighlighter) == true)
						{
							m_ctrlHighlighters.SelectedItem = m_dxHighlighter;
						}
						else
						{
							m_ctrlHighlighters.SelectedIndex = 0;
						}
						
					}
					else
					{
						m_ctrlHighlighters.SelectedIndex = 0;
					}
				
				}
				
			}
			
		}// private void FillHighlighters()
		
		/// <summary>This method will locate the segment in the active deposition with the specified key</summary>
		/// <param name="strKey">The xml key identifier</param>
		/// <returns>the specified segment</returns>
		private CDxSecondary GetSegment(string strKey)
		{
			long lId = 0;
			
			Debug.Assert(m_dxDeposition != null);
			Debug.Assert(m_dxDeposition.Secondaries != null);
			if(m_dxDeposition == null) return null;
			if(m_dxDeposition.Secondaries == null) return null;
			
			try
			{
				lId = System.Convert.ToInt64(strKey);
			}
			catch
			{
				return null;
			}
			
			//	Locate the appropriate segment
			foreach(CDxSecondary O in m_dxDeposition.Secondaries)
			{
				if((O.GetExtent() != null) && (O.GetExtent().XmlSegmentId == lId))
				{
					return O;
				}
							
			}
			
			//	Not found
			return null;
			
		}// private CDxSecondary GetSegment(string strKey)
		
		/// <summary>This method will retrieve selected highlighter</summary>
		private CDxHighlighter GetHighlighter()
		{
			if(m_ctrlHighlighters == null) return null;
			if(m_ctrlHighlighters.SelectedItem == null) return null;
			
			try		{ return ((CDxHighlighter)(m_ctrlHighlighters.SelectedItem)); }
			catch	{ return null; }

		}// private CDxHighlighter GetHighlighter()
		
		/// <summary>This method will populate the list of highlighters</summary>
		private void FillDepositions()
		{
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
			{
				foreach(CDxPrimary O in m_tmaxDatabase.Primaries)
				{
					//	Is this a deposition?
					if(O.MediaType == TmaxMediaTypes.Deposition)
					{
						m_ctrlDepositions.Items.Add(O);
					}
					
				}
				
				//	Select the transcript
				if((m_ctrlDepositions.Items.Count > 0) && (EditExtents == false))
				{
					if(m_dxDeposition != null)
					{
						if(m_ctrlDepositions.Items.Contains(m_dxDeposition) == true)
						{
							m_ctrlDepositions.SelectedItem = m_dxDeposition;
						}
						else
						{
							m_ctrlDepositions.SelectedIndex = 0;
						}
						
					}
					else
					{
						m_ctrlDepositions.SelectedIndex = 0;
					}
				}
			}
			
		}// private void FillDepositions()
		
		/// <summary>Called when the user selects a new deposition</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnDepositionSelChanged(object sender, System.EventArgs e)
		{
			SetDeposition((CDxPrimary)m_ctrlDepositions.SelectedItem);
		}
		
		/// <summary>Called to set the active deposition</summary>
		/// <param name="dxDeposition">The deposition to be activated</param>
		/// <returns>true if successful</returns>
		private bool SetDeposition(CDxPrimary dxDeposition)
		{
			CDxTranscript	dxTranscript = null;
			bool			bSuccessful = false;
			
			//	Clear the current values
			m_dxDeposition = null;
			m_dxTranscript = null;
			m_bXmlFailed = false;
			
			//	Did the caller specify a deposition?
			if(dxDeposition == null)
			{
				SetControlStates();
				return true;
			}
			
			//	Retrieve the information associated with the new deposition
			while(bSuccessful == false)
			{
				//	Get the transcript information for this deposition
				if((dxTranscript = dxDeposition.GetTranscript()) == null)
				{
					m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_TRANSCRIPT_NOT_FOUND, dxDeposition.MediaId));
					break;
				}
			
				//	Make sure we have the segments
				if((dxDeposition.Secondaries == null) || (dxDeposition.Secondaries.Count == 0))
				{	
					if(dxDeposition.Fill() == false)
						return false;
				}
				
				//	Everything is ok
				bSuccessful = true;
				
			}// while(bSuccessful == false)
		
			//	Should we update the local values?
			if(bSuccessful == true)
			{
				m_dxDeposition  = dxDeposition;
				m_dxTranscript  = dxTranscript;
			}
			
			//	Update the control states
			SetControlStates();
			
			return bSuccessful;
		
		}// private bool SetDeposition(CDxPrimary dxDeposition)
		
		/// <summary>This method will enable/disable the child controls based on current property values</summary>
		private void SetControlStates()
		{
			//	Do we have the required objects?
			m_ctrlStartLabel.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlStopLabel.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlMinimumLabel.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlMaximumLabel.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlDepositionPageLabel.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlDepositionLineLabel.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			
			m_ctrlMinimumPage.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlMinimumLine.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlMaximumPage.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlMaximumLine.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));

			m_ctrlStartPage.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlStartLine.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlStopPage.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
			m_ctrlStopLine.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));
		
			m_ctrlOK.Enabled = ((m_dxTranscript != null) && (m_bXmlFailed == false));

			if(m_dxTranscript != null)
			{
				m_ctrlMinimumPage.Text = (CTmaxToolbox.PLToPage(m_dxTranscript.FirstPL)).ToString();
				m_ctrlMinimumLine.Text = (CTmaxToolbox.PLToLine(m_dxTranscript.FirstPL)).ToString();
				m_ctrlMaximumPage.Text = (CTmaxToolbox.PLToPage(m_dxTranscript.LastPL)).ToString();
				m_ctrlMaximumLine.Text = (CTmaxToolbox.PLToLine(m_dxTranscript.LastPL)).ToString();

			}
			else
			{
				m_ctrlMinimumPage.Text = "";
				m_ctrlMaximumPage.Text = "";
				m_ctrlMaximumPage.Text = "";
				m_ctrlMaximumLine.Text = "";
			}	
		
		}// private void SetControlStates()
		
		/// <summary>Called to open the XML deposition associated with the active deposition</summary>
		/// <returns>The XML deposition associated with the active deposition record</returns>
		private CXmlDeposition GetXmlDeposition()
		{
			string strFileSpec = "";
			
			//	Do we have an active deposition?
			Debug.Assert(m_dxDeposition != null);
			Debug.Assert(m_dxTranscript != null);
			if((m_dxDeposition == null) || (m_dxTranscript == null)) 
				return null;
			
			//	Have we already retrieved the xml file?
			if(m_dxTranscript.XmlDeposition != null)
				return m_dxTranscript.XmlDeposition;
				
			//	Get the path to the xml file from the database
			strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxTranscript);
		
			if((strFileSpec == null) || (strFileSpec.Length == 0))
			{
				m_tmaxEventSource.FireError(this, "GetXmlDeposition", m_tmaxErrorBuilder.Message(ERROR_INVALID_XML_FILESPEC, m_dxDeposition.MediaId));
				return null;
			}

			//	Allocate and open a new transcript
			try
			{
				m_dxTranscript.XmlDeposition = new CXmlDeposition();
				m_tmaxEventSource.Attach(m_dxTranscript.XmlDeposition.EventSource);
				
				//	Mark this deposition so we know if it has to be closed
				m_dxTranscript.XmlDeposition.UserData = this;

				if(m_dxTranscript.XmlDeposition.FastFill(strFileSpec, true, true, false) == false)
				{
					m_dxTranscript.XmlDeposition = null;
				}
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetXmlDeposition", m_tmaxErrorBuilder.Message(ERROR_GET_XML_DEPOSITION_EX, m_dxDeposition.MediaId, strFileSpec), Ex);
				m_dxTranscript.XmlDeposition = null;
			}
			
			return m_dxTranscript.XmlDeposition;
		
		}// private CXmlDeposition GetXmlDeposition()
		
		/// <summary>This method is called to add a designation to the database</summary>
		///	<param name="dxSegment">The segment that owns the designation</param>
		/// <param name="xmlDesignation">The xml designation to be added</param>
		/// <returns>The new designation record exchange object</returns>
		private CDxTertiary AddDesignation(CDxSecondary dxSegment, CXmlDesignation xmlDesignation)
		{
			CTmaxItem	tmaxParent = null;
			CTmaxItem	tmaxDesignation = null;
			
			Debug.Assert(xmlDesignation != null);
			Debug.Assert(dxSegment != null);
			if(xmlDesignation == null) return null;
			if(dxSegment == null) return null;
				
			try
			{
				//	Create a parent item for the segment
				tmaxParent = new CTmaxItem(dxSegment);
				Debug.Assert(tmaxParent.MediaType == TmaxMediaTypes.Segment);
				
				//	Create an item to represent the designation and add it
				//	to the source items collection
				tmaxDesignation = new CTmaxItem();
				tmaxDesignation.XmlDesignation = xmlDesignation;
				
				if(tmaxParent.SourceItems == null)
					tmaxParent.SourceItems = new CTmaxItems();
				tmaxParent.SourceItems.Add(tmaxDesignation);
				
				//	Fire the command to add the designation
				FireCommand(TmaxCommands.Add, tmaxParent);
				
				//	The database should have set the record interface
				if((tmaxDesignation.GetMediaRecord() != null) &&
					(tmaxDesignation.MediaType == TmaxMediaTypes.Designation))
				{
					return (CDxTertiary)tmaxDesignation.GetMediaRecord();
				}
				else
				{
					return null;
				}				

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddDesignation", m_tmaxErrorBuilder.Message(ERROR_ADD_DESIGNATION_EX, xmlDesignation.Name), Ex);
				return null;
			}
			
		}// private CDxTertiary AddDesignation(CDxSecondary dxSegment, CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to add the specified designations</summary>
		///	<param name="xmlDesignations">The collection of designations to be added</param>
		/// <param name="bActivate">true to activate the first new scene</param>
		///	<returns>true if successful</returns>
		private bool AddDesignations(CXmlDesignations xmlDesignations, bool bActivate)
		{
			CTmaxItems			tmaxAdded = null;
			CDxTertiary			dxDesignation = null;
			CDxSecondary		dxSegment = null;
			bool				bSuccessful = false;
			
			Debug.Assert(m_dxScript != null);
			if(m_dxScript == null) return false;
				
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				//	Allocate a temporary collection to hold event items that
				//	represent the new designations
				tmaxAdded = new CTmaxItems();
				
				//	Add each of the designations
				foreach(CXmlDesignation O in xmlDesignations)
				{
					//	Hook the TrialMax events if not already hooked
					if(O.EventSource.ErrorHooked == false)
						O.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
					if(O.EventSource.DiagnosticHooked == false)
						O.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);

					//	Try to locate the secondary segment
					if((dxSegment = GetSegment(O.Segment)) == null)
					{
						Warn(m_tmaxErrorBuilder.Message(ERROR_SEGMENT_NOT_FOUND, O.Name, O.Segment), null);
					}
					else
					{
						if((dxDesignation = AddDesignation(dxSegment, O)) != null)
						{
							tmaxAdded.Add(new CTmaxItem(dxDesignation));
							
							//	Close the designation file
							O.Close(true);
						}
					}
					
				}
				
				//	Did we add any designations?
				if(tmaxAdded.Count > 0)
				{
					//	Add new scenes to the script
					AddScenes(tmaxAdded, bActivate);
				}
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddDesignations", m_tmaxErrorBuilder.Message(ERROR_ADD_DESIGNATIONS_EX), Ex);
			}				
			
			//	Clean up
			if(tmaxAdded != null)
			{
				tmaxAdded.Clear();
				tmaxAdded = null;
			}
			
			Cursor.Current = Cursors.Default;

			return bSuccessful;
			
		}// private bool AddDesignations(CXmlDesignations xmlDesignations)
		
		/// <summary>This method is called to use the specified designations to edit the active designation</summary>
		private void OnClickEdit()
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;
			
			//	There must always be a scene
			Debug.Assert(m_dxScene != null);
			if(m_dxScene == null) return;
			
			//	Check the user supplied values
			if(CheckRange() == false) return;
			
			//	Have any changes been made
			if(IsModified() == false)
			{
				//	Make it look like the user cancelled
				OnClickCancel(m_ctrlCancel, null);
			}
			else
			{
				//	Allocate and initialize the event item
				tmaxItem = new CTmaxItem(m_dxScene);
				
				//	Populate the parameters collection
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.EditMethod, (int)TmaxDesignationEditMethods.Extents);
				tmaxParameters.Add(TmaxCommandParameters.StartPL, m_lStartPL);
				tmaxParameters.Add(TmaxCommandParameters.StopPL, m_lStopPL);
				
				if(m_dxHighlighter != null)
					tmaxParameters.Add(TmaxCommandParameters.Highlighter, m_dxHighlighter.AutoId);

				//	Fire the command event
				FireCommand(TmaxCommands.EditDesignation, tmaxItem, tmaxParameters);
				
				//	Close the form
				DialogResult = DialogResult.OK;
				this.Close();
			}
			
		}// private void OnClickEdit()
		
		/// <summary>This method is called in edit mode to determine if the active designation has been modified</summary>
		/// <returns>true if modified</returns>
		private bool IsModified()
		{
			bool bModified = false;
			
			//	Do the extents match?
			//
			//	NOTE:	The call to CheckRange() updated the local PL values
			if(m_lStartPL != m_dxDesignation.GetExtent().StartPL)
				bModified = true;
			if(m_lStopPL != m_dxDesignation.GetExtent().StopPL)
				bModified = true;
				
			//	Has the highlighter changed?
			if(ReferenceEquals(m_dxHighlighter, GetHighlighter()) == false)
			{
				m_dxHighlighter = GetHighlighter();
				bModified = true;
			}
			
			return bModified;
			
		}// private bool IsModified()
		
		/// <summary>This method is called to initialize the form for edit mode operation</summary>
		/// <returns>true if successful</returns>
		private bool PrepareForEdit()
		{
			CDxPrimary	dxDeposition = null;
			long		lHighlighter = 0;
			
			//	Hide the import button
			m_ctrlImport.Visible = false;
			
			//	Make sure we have the required objects
			if(m_dxScript == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_NO_EDIT_SCRIPT), null);
			if(m_dxScene == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_NO_EDIT_SCENE), null);
			
			//	Get the source designation
			if((m_dxScene.GetSource() != null) && (m_dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
				m_dxDesignation = (CDxTertiary)(m_dxScene.GetSource());
			if(m_dxDesignation == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_NO_EDIT_DESIGNATION), null);
				
			//	Get the parent deposition
			if((m_dxDesignation != null) && (m_dxDesignation.Secondary != null))
			{
				dxDeposition = m_dxDesignation.Secondary.Primary;
				
				Debug.Assert(dxDeposition != null);
				Debug.Assert(dxDeposition.MediaType == TmaxMediaTypes.Deposition);
				
				//	Is this deposition in our local list?
				if(m_ctrlDepositions.Items.Contains(dxDeposition) == true)
				{
					//	Make this the active deposition
					m_ctrlDepositions.SelectedItem = dxDeposition;
				}
				else
				{
					return Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_EDIT_DEPOSITION, dxDeposition.DisplayString), null);
				}
			}
			
			//	Were we successful?
			if(dxDeposition == null)
			{
				return Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_EDIT_DEPOSITION, "unknown"), null);
			}
			if(m_dxTranscript == null)
			{
				//	User already got warned
				return false;
			}
			
			//	Don't allow the user to change the deposition
			m_ctrlDepositions.Enabled = false;
			
			//	Initialize the extents controls
			try
			{
				m_ctrlStartPage.Text = (CTmaxToolbox.PLToPage(m_dxDesignation.GetExtent().StartPL)).ToString();
				m_ctrlStartLine.Text = (CTmaxToolbox.PLToLine(m_dxDesignation.GetExtent().StartPL)).ToString();
				m_ctrlStopPage.Text = (CTmaxToolbox.PLToPage(m_dxDesignation.GetExtent().StopPL)).ToString();
				m_ctrlStopLine.Text = (CTmaxToolbox.PLToLine(m_dxDesignation.GetExtent().StopPL)).ToString();
			}
			catch
			{
				return Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_EDIT_EXTENTS, m_dxDesignation.GetExtent().StartPL, m_dxDesignation.GetExtent().StopPL), null);
			}
			
			//	Set the highlighter
			try
			{
				if(m_dxHighlighter != null)
					lHighlighter = m_dxHighlighter.AutoId;
				else
					lHighlighter = m_dxDesignation.GetExtent().HighlighterId;
					
				foreach(CDxHighlighter O in m_ctrlHighlighters.Items)
				{
					if(O.AutoId == lHighlighter)
					{
						m_ctrlHighlighters.SelectedItem = O;
						break;
					}
				}
			
			}
			catch
			{
			}
			return true;
			
		}// private bool InitEditExtents()
		
		/// <summary>This method is called to add new scenes to the active script</summary>
		/// <param name="tmaxDesignations">the collection of designations to be added as new scenes</param>
		/// <param name="bActivate">true to activate the first new scene</param>
		/// <returns>true if successful</returns>
		private bool AddScenes(CTmaxItems tmaxDesignations, bool bActivate)
		{
			CTmaxItem			tmaxScript = null;
			CTmaxItem			tmaxLastDesignation = null;
			CTmaxParameters		tmaxParameters = null;
			
			Debug.Assert(tmaxDesignations != null);
			Debug.Assert(tmaxDesignations.Count > 0);
			Debug.Assert(m_dxScript != null);
			if(tmaxDesignations == null) return false;
			if(tmaxDesignations.Count == 0) return false;
			if(m_dxScript == null) return false;
			
			try
			{
				//	Create an event item for the parent script
				tmaxScript = new CTmaxItem(m_dxScript);
					
				//	Assign the source items
				tmaxScript.SourceItems = tmaxDesignations;
					
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Activate, bActivate);
					
				//	Are we inserting into the script
				if(m_dxScene != null)
				{
					Debug.Assert(ReferenceEquals(m_dxScene.Primary, m_dxScript) == true);
					
					//	Create the required parameters for the event
					tmaxParameters.Add(TmaxCommandParameters.Before, m_bInsertBefore);
					
					//	Put the insertion point in the subitem collection
					tmaxScript.SubItems.Add(new CTmaxItem(m_dxScene));
					
					//	Save a reference to the last designation if we are inserting
					//	after
					if(m_bInsertBefore == false)
						tmaxLastDesignation = tmaxDesignations[tmaxDesignations.Count - 1];
				}

				//	Fire the event
				FireCommand(TmaxCommands.Add, tmaxScript, tmaxParameters);
				
				//	We need to adjust the active scene if we were inserting after
				//	in order to keep the designations in the correct order
				if(tmaxLastDesignation != null)
				{
					//	Make the new scene the active scene.
					//
					//	The database returns the new record in the original item
					if((tmaxLastDesignation.ReturnItem != null) && (tmaxLastDesignation.ReturnItem.GetMediaRecord() != null))
					{
						Debug.Assert(tmaxLastDesignation.ReturnItem.MediaType == TmaxMediaTypes.Scene);
						m_dxScene = (CDxSecondary)(tmaxLastDesignation.ReturnItem.GetMediaRecord());
					}
					
				}
					
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddScenes", m_tmaxErrorBuilder.Message(ERROR_ADD_SCENES_EX), Ex);
				return false;
			}				

		}// private bool AddScenes(CTmaxItems tmaxDesignations)
		
		/// <summary>This method is called to check the range specified by the user</summary>
		/// <returns>true if the specified range is ok</returns>
		private bool CheckRange()
		{
			bool bOk = false;
			
			while(bOk == false)
			{
				try
				{
					m_lStartPage = System.Convert.ToInt64(m_ctrlStartPage.Text);
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_START_PAGE), m_ctrlStartPage);
					break;
				}
				
				try
				{
					m_iStartLine = System.Convert.ToInt32(m_ctrlStartLine.Text);
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_START_LINE), m_ctrlStartLine);
					break;
				}
				
				try
				{
					m_lStopPage = System.Convert.ToInt64(m_ctrlStopPage.Text);
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_STOP_PAGE), m_ctrlStopPage);
					break;
				}
				
				try
				{
					m_iStopLine = System.Convert.ToInt32(m_ctrlStopLine.Text);
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_STOP_LINE), m_ctrlStopLine);
					break;
				}
				
				if((m_dxTranscript.LinesPerPage > 0) && (m_iStartLine > m_dxTranscript.LinesPerPage))
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_START_LINE_OUT_OF_RANGE, m_dxTranscript.LinesPerPage), m_ctrlStartLine);
					break;
				}
				
				if((m_dxTranscript.LinesPerPage > 0) && (m_iStopLine > m_dxTranscript.LinesPerPage))
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_STOP_LINE_OUT_OF_RANGE, m_dxTranscript.LinesPerPage), m_ctrlStopLine);
					break;
				}
				
				m_lStartPL = CTmaxToolbox.GetPL(m_lStartPage, m_iStartLine);
				m_lStopPL = CTmaxToolbox.GetPL(m_lStopPage, m_iStopLine);
				
				if(m_lStartPL < m_dxTranscript.FirstPL)
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_START_PL_OUT_OF_RANGE), m_ctrlStartPage);
					break;
				}
				
				if((m_dxTranscript.LastPL > 0) && (m_lStopPL > m_dxTranscript.LastPL))
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_STOP_PL_OUT_OF_RANGE), m_ctrlStopPage);
					break;
				}
				
				if(m_lStopPL < m_lStartPL)
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_PL_REVERSED), m_ctrlStartPage);
					break;
				}
				
				bOk = true;
			}
			
			return bOk;
		
		}// private bool CheckRange()
		
		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		private bool Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation);
			
			if(ctrlSelect != null)
				Select(ctrlSelect);	
				
			return false; // allows for cleaner code						
		
		}// private void Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>Called when one of the pane/line edit boxes gets focus</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnEnterExtent(object sender, System.EventArgs e)
		{
			try
			{
				((System.Windows.Forms.TextBox)sender).SelectAll();
			}
			catch
			{
			}
			
		}

		/// <summary>Called when the user clicks on the Import button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickImport(object sender, System.EventArgs e)
		{
			CTmaxItem		tmaxTarget = null;
			CTmaxParameters tmaxParameters = null;
			
			try
			{
				tmaxTarget = new CTmaxItem(m_dxScript);
				tmaxParameters = new CTmaxParameters();

				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.AsciiMedia));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Activate, true));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.MergeImported, true));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Before, InsertBefore));
				
				if(m_dxScene != null)
					tmaxTarget.SubItems.Add(new CTmaxItem(m_dxScene));
					
				FireCommand(TmaxCommands.Import, tmaxTarget, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickImport", m_tmaxErrorBuilder.Message(ERROR_IMPORT_EX), Ex);
			}
			
		}// private void OnClickImport(object sender, System.EventArgs e)
		
		/// <summary>This method is called to select the specified control</summary>
		/// <param name="ctrlTextBox">the desired text box control to be selected</param>
		private void Select(System.Windows.Forms.TextBox ctrlTextBox)
		{
			ctrlTextBox.Focus();
			ctrlTextBox.SelectAll();
		}
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		/// <summary>The active script record</summary>
		public CDxPrimary Script
		{
			get { return m_dxScript; }
			set { m_dxScript = value; }
		}
		
		/// <summary>The active deposition</summary>
		public CDxPrimary Deposition
		{
			get { return m_dxDeposition; }
			set { m_dxDeposition = value; }
		}
		
		/// <summary>The highlighter used to initialize the form</summary>
		public CDxHighlighter Highlighter
		{
			get { return m_dxHighlighter; }
			set { m_dxHighlighter = value; }
		}
		
		/// <summary>The active scene record</summary>
		public CDxSecondary Scene
		{
			get { return m_dxScene; }
			set { m_dxScene = value; }
		}
		
		/// <summary>True to insert designations before the specified Scene</summary>
		public bool InsertBefore
		{
			get { return m_bInsertBefore; }
			set { m_bInsertBefore = value; }
		}
		
		/// <summary>True to edit the extents of Designation instead of add new designations</summary>
		public bool EditExtents
		{
			get { return m_bEditExtents; }
			set { m_bEditExtents = value; }
		}
		
		#endregion Properties
		
	}// public class CFAddDesignations : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Panes
