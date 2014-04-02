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

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class manages a form that allows the user to add new video designations</summary>
	public class CFTmaxVideoDesignations : CFTmaxVideoForm
	{
		#region Constants
		
		protected const int	ERROR_INVALID_START_PAGE			= (ERROR_TMAX_VIDEO_FORM_MAX + 1);
		protected const int	ERROR_INVALID_START_LINE			= (ERROR_TMAX_VIDEO_FORM_MAX + 2);
		protected const int	ERROR_INVALID_STOP_PAGE				= (ERROR_TMAX_VIDEO_FORM_MAX + 3);
		protected const int	ERROR_INVALID_STOP_LINE				= (ERROR_TMAX_VIDEO_FORM_MAX + 4);
		protected const int ERROR_START_LINE_OUT_OF_RANGE		= (ERROR_TMAX_VIDEO_FORM_MAX + 5);
		protected const int ERROR_STOP_LINE_OUT_OF_RANGE		= (ERROR_TMAX_VIDEO_FORM_MAX + 6);
		protected const int ERROR_START_PL_OUT_OF_RANGE			= (ERROR_TMAX_VIDEO_FORM_MAX + 7);
		protected const int ERROR_STOP_PL_OUT_OF_RANGE			= (ERROR_TMAX_VIDEO_FORM_MAX + 8);
		protected const int ERROR_PL_REVERSED					= (ERROR_TMAX_VIDEO_FORM_MAX + 9);
		protected const int ERROR_ADD_DESIGNATIONS_EX			= (ERROR_TMAX_VIDEO_FORM_MAX + 10);
		protected const int ERROR_NO_EDIT_SCRIPT				= (ERROR_TMAX_VIDEO_FORM_MAX + 11);
		protected const int ERROR_NO_EDIT_DESIGNATION			= (ERROR_TMAX_VIDEO_FORM_MAX + 12);
		protected const int ERROR_INVALID_EDIT_EXTENTS			= (ERROR_TMAX_VIDEO_FORM_MAX + 13);
		protected const int ERROR_IMPORT_EX						= (ERROR_TMAX_VIDEO_FORM_MAX + 14);
		
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
		
		/// <summary>Text control to display deposition name</summary>
		private System.Windows.Forms.Label m_ctrlDeposition;
		
		/// <summary>Deposition name label</summary>
		private System.Windows.Forms.Label m_ctrlDepositionLabel;
		
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
		
		/// <summary>Local member bound to XmlScript property</summary>
		private CXmlScript m_xmlScript = null;
		
		/// <summary>Local member bound to XmlInsert property</summary>
		private CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member bound to Highlighter property</summary>
		private CTmaxHighlighter m_tmaxHighlighter = null;
		
		/// <summary>Private member bound to VideoOptions property</summary>
		private CTmaxVideoOptions m_tmaxVideoOptions = null;
		
		/// <summary>Local member bound to EditExtents property</summary>
		private bool m_bEditExtents = false;
		
		/// <summary>Local member bound to InsertBefore property</summary>
		private bool m_bInsertBefore = false;
		
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
		
		/// <summary>Local member to store the minimum PL</summary>
		private long m_lMinimumPL = -1;
		
		/// <summary>Local member to store the maximum PL</summary>
		private long m_lMaximumPL = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFTmaxVideoDesignations()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Designation Editor";
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
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlScriptLabel = new System.Windows.Forms.Label();
			this.m_ctrlScript = new System.Windows.Forms.Label();
			this.m_ctrlDepositionLabel = new System.Windows.Forms.Label();
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
			this.m_ctrlDeposition = new System.Windows.Forms.Label();
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
			this.m_ctrlDepositionLabel.Location = new System.Drawing.Point(8, 32);
			this.m_ctrlDepositionLabel.Name = "m_ctrlDepositionLabel";
			this.m_ctrlDepositionLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlDepositionLabel.TabIndex = 4;
			this.m_ctrlDepositionLabel.Text = "Deposition:";
			this.m_ctrlDepositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.m_ctrlImport.TabIndex = 5;
			this.m_ctrlImport.TabStop = false;
			this.m_ctrlImport.Text = "&Import";
			this.m_ctrlImport.Click += new System.EventHandler(this.OnClickImport);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlHighlighters
			// 
			this.m_ctrlHighlighters.DisplayMember = "DisplayString";
			this.m_ctrlHighlighters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlHighlighters.IntegralHeight = false;
			this.m_ctrlHighlighters.Location = new System.Drawing.Point(111, 56);
			this.m_ctrlHighlighters.Name = "m_ctrlHighlighters";
			this.m_ctrlHighlighters.Size = new System.Drawing.Size(177, 21);
			this.m_ctrlHighlighters.TabIndex = 8;
			this.m_ctrlHighlighters.TabStop = false;
			// 
			// m_ctrlHighlightersLabel
			// 
			this.m_ctrlHighlightersLabel.Location = new System.Drawing.Point(7, 64);
			this.m_ctrlHighlightersLabel.Name = "m_ctrlHighlightersLabel";
			this.m_ctrlHighlightersLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlHighlightersLabel.TabIndex = 21;
			this.m_ctrlHighlightersLabel.Text = "Highlighter:";
			this.m_ctrlHighlightersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDeposition
			// 
			this.m_ctrlDeposition.Location = new System.Drawing.Point(112, 32);
			this.m_ctrlDeposition.Name = "m_ctrlDeposition";
			this.m_ctrlDeposition.Size = new System.Drawing.Size(176, 12);
			this.m_ctrlDeposition.TabIndex = 22;
			this.m_ctrlDeposition.Text = "deposition name here";
			this.m_ctrlDeposition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFTmaxVideoDesignations
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(294, 251);
			this.Controls.Add(this.m_ctrlDeposition);
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
			this.Controls.Add(this.m_ctrlDepositionLabel);
			this.Controls.Add(this.m_ctrlScript);
			this.Controls.Add(this.m_ctrlScriptLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFTmaxVideoDesignations";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add Designations";
			this.ResumeLayout(false);

		}

		/// <summary>Called when the form's Load event is trapped</summary>
		/// <param name="e">system event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if((m_xmlScript != null) && (m_xmlScript.XmlDeposition != null))
			{
				if(m_xmlScript.Name.Length > 0)
					m_ctrlScript.Text = m_xmlScript.Name;
				else
					m_ctrlScript.Text = "Unnamed";
					
				m_ctrlDeposition.Text = m_xmlScript.XmlDeposition.Name;
				
				//	Populate the highlighters
				FillHighlighters();
				
				//	Set the range allowed by the deposition
				SetDepositionLimits();
				
				//	Are we editing extents?
				if(EditExtents == true)
				{
					if(InitializeEditor() == false)
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
			
			}// if((m_xmlScript != null) && (m_xmlScript.XmlDeposition != null))
		
			base.OnLoad (e);

		}// protected void OnLoad(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Private Methods
		
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Do the base class processing first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid start page value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid start line value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid stop page value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid stop line value.");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified start line is out of range. The maximum lines per page is %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("The specified stop line is out of range. The maximum lines per page is %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified start page/line is outside the range allowed for the transcript");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified stop page/line is outside the range allowed for the transcript");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified stop page/line must appear after the specified start page/line values");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the new designations to the database.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to edit the extents. No script has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to edit the extents. No designation has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to initilize the extents controls using the specified designation: FirstPL = %1 LastPL = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to import new designations.");

		}// protected override void SetErrorStrings()

		/// <summary>Called when the user clicks on the Add button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			//	Are we editing extents?
			if(EditExtents == true)
			{
				OnEdit();
			}
			else
			{
				OnAdd();
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
		private void OnAdd()
		{
			CXmlDesignations	xmlDesignations = null;
			string				strMsg = "";
			bool				bSuccessful = false;
			int					iHighlighter = 0;
			
			//	Do we have an active deposition?
			Debug.Assert(m_xmlScript != null);
			Debug.Assert(m_xmlScript.XmlDeposition != null);
			if((m_xmlScript == null) || (m_xmlScript.XmlDeposition == null)) 
				return;
				
			Cursor.Current = Cursors.WaitCursor;
		
			while(bSuccessful == false)
			{
				//	Check the user supplied values
				if(CheckRange() == false) 
					break;
			
				//	Get the highlighter to be used for the new designations
				if((m_tmaxHighlighter = GetHighlighter()) != null)
				{
					iHighlighter = (int)(m_tmaxHighlighter.Id);
				}
				else if((m_tmaxVideoOptions.Highlighters != null) && (m_tmaxVideoOptions.Highlighters.Count > 0))
				{
					iHighlighter = (int)(m_tmaxVideoOptions.Highlighters[0].Id);
				}
				
				//	Create the designations
				xmlDesignations = new CXmlDesignations();
				if(m_xmlScript.XmlDeposition.CreateDesignations(xmlDesignations, m_lStartPL, m_lStopPL, iHighlighter) == false)
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
				
				//	Add the designations to the script
				AddDesignations(xmlDesignations, true);
				
				//	Looks good
				bSuccessful = true;
				
			}// while(bSuccessful == false)
					
			Cursor.Current = Cursors.Default;
			
			Select(m_ctrlStartPage);
			
		}// private void OnAdd()

		/// <summary>This method will populate the list of highlighters</summary>
		private void FillHighlighters()
		{
			Debug.Assert(m_tmaxVideoOptions != null);
			Debug.Assert(m_tmaxVideoOptions.Highlighters != null);
			
			if((m_tmaxVideoOptions != null) && (m_tmaxVideoOptions.Highlighters != null))
			{
				foreach(CTmaxHighlighter O in m_tmaxVideoOptions.Highlighters)
				{
					//	Has this highlighter been assigned?
					if((O.Label != null) && (O.Label.Length > 0))
					{
						m_ctrlHighlighters.Items.Add(O);
					}
					
				}
				
				//	Select the first highlighter
				if((EditExtents == false) && (m_ctrlHighlighters.Items.Count > 0))
				{
					SetHighlighter(m_tmaxHighlighter);
				}
				
			}
			
		}// private void FillHighlighters()
		
		/// <summary>This method will set the current selection in the highlighter's list box</summary>
		/// <param name="tmaxHighlighter">the highlighter to be selected</param>
		private void SetHighlighter(CTmaxHighlighter tmaxHighlighter)
		{
			try
			{
				if((m_ctrlHighlighters.Items != null) && (m_ctrlHighlighters.Items.Count > 0))
				{
					if(tmaxHighlighter != null)
					{
						if(m_ctrlHighlighters.Items.Contains(tmaxHighlighter) == true)
						{
							m_ctrlHighlighters.SelectedItem = tmaxHighlighter;
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
				
				}// if((m_ctrlHighlighters.Items != null) && (m_ctrlHighlighters.Items.Count > 0))
			
			}
			catch
			{
			}

		}// private void SetHighlighter(CTmaxHighlighter tmaxHighlighter)
		
		/// <summary>This method will retrieve selected highlighter</summary>
		private CTmaxHighlighter GetHighlighter()
		{
			if(m_ctrlHighlighters == null) return null;
			if(m_ctrlHighlighters.SelectedItem == null) return null;
			
			try		{ return ((CTmaxHighlighter)(m_ctrlHighlighters.SelectedItem)); }
			catch	{ return null; }

		}// private CTmaxHighlighter GetHighlighter()
		
		/// <summary>This method is called to add the specified designations</summary>
		///	<param name="xmlDesignations">The collection of designations to be added</param>
		/// <param name="bActivate">true to activate the first new scene</param>
		///	<returns>true if successful</returns>
		private bool AddDesignations(CXmlDesignations xmlDesignations, bool bActivate)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxScript = null;
			CTmaxItem		tmaxDesignation = null;
			CTmaxVideoArgs	Args = null;
			int				iIndex = 0;
			
			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return false;
				
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				//	Allocate an item to represent the parent script
				tmaxScript = new CTmaxItem();
				tmaxScript.XmlScript = m_xmlScript;
				tmaxScript.MediaType = TmaxMediaTypes.Script;
				
				//	Add each of the designations
				foreach(CXmlDesignation O in xmlDesignations)
				{
					//	Create an event item to represent this designation
					tmaxDesignation = new CTmaxItem();
					tmaxDesignation.MediaType = TmaxMediaTypes.Designation;
					tmaxDesignation.XmlDesignation = O;
					
					//	Add to the collection of source items
					tmaxScript.SourceItems.Add(tmaxDesignation);
					
				}// foreach(CXmlDesignation O in xmlDesignations)
				
				//	Did we add any designations?
				if(tmaxScript.SourceItems.Count > 0)
				{
					//	Allocate the parameter collection for the event
					tmaxParameters = new CTmaxParameters();
				
					//	Are we inserting into the script
					if(this.XmlDesignation != null)
					{
						//	Allocate an item to represent the insertion point
						tmaxDesignation = new CTmaxItem();
						tmaxDesignation.MediaType = TmaxMediaTypes.Designation;
						tmaxDesignation.XmlDesignation = this.XmlDesignation;
						
						//	Add to the subitems collection
						tmaxScript.SubItems.Add(tmaxDesignation);
						
						tmaxParameters.Add(TmaxCommandParameters.Before, this.InsertBefore);
						
					}	
					else
					{
						//	Request activation of the first new record when adding to the end of the script
						tmaxParameters.Add(TmaxCommandParameters.Activate, true);
					}	
				
					//	Fire the command event
					Args = FireCommand(TmaxVideoCommands.Add, tmaxScript, tmaxParameters);
				
					//	Were we successful?
					if((Args.Successful == true) && (Args.Result != null) && (Args.Result.SubItems.Count > 0))
					{
						//	We need to adjust the insertion point if inserting After
						if((this.XmlDesignation != null) && (this.InsertBefore == false))
						{
							//	Use the last new designation
							iIndex = Args.Result.SubItems.Count - 1;
							
							this.XmlDesignation = Args.Result.SubItems[iIndex].XmlDesignation;
							
							tmaxParameters = new CTmaxParameters();
							tmaxParameters.Add(TmaxCommandParameters.SyncMediaTree, true);
							
							FireCommand(TmaxVideoCommands.Activate, Args.Result.SubItems[iIndex], tmaxParameters);
						
						}// if((this.XmlDesignation != null) && (this.InsertBefore == false))
					
					}// if((Args.Successful == true) && (Args.Result != null) && (Args.Result.SubItems.Count > 0))
					
				}// if(this.XmlInsert != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddDesignations", m_tmaxErrorBuilder.Message(ERROR_ADD_DESIGNATIONS_EX), Ex);
			}

			Cursor.Current = Cursors.Default;

			return ((Args != null) && (Args.Successful == true));
			
		}// private bool AddDesignations(CXmlDesignations xmlDesignations)
		
		/// <summary>This method is called to use the specified designations to edit the active designation</summary>
		private void OnEdit()
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem			tmaxScript = null;
			
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return;
			
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
				//	Allocate and initialize the event item to represent the script and designation
				tmaxScript = new CTmaxItem(m_xmlScript, m_xmlDesignation);
				
				//	Populate the parameters collection
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.EditMethod, (int)(TmaxDesignationEditMethods.Extents));
				tmaxParameters.Add(TmaxCommandParameters.StartPL, m_lStartPL);
				tmaxParameters.Add(TmaxCommandParameters.StopPL, m_lStopPL);
				
				if(m_tmaxHighlighter != null)
					tmaxParameters.Add(TmaxCommandParameters.Highlighter, m_tmaxHighlighter.Id);

				//	Fire the command event
				FireCommand(TmaxVideoCommands.EditDesignation, tmaxScript, tmaxParameters);
				
				//	Close the form
				DialogResult = DialogResult.OK;
				this.Close();
			}
			
		}// private void OnEdit()
		
		/// <summary>This method is called in edit mode to determine if the active designation has been modified</summary>
		/// <returns>true if modified</returns>
		private bool IsModified()
		{
			bool bModified = false;
			
			//	Do the extents match?
			//
			//	NOTE:	The call to CheckRange() updated the local PL values
			if(m_lStartPL != m_xmlDesignation.FirstPL)
				bModified = true;
			if(m_lStopPL != m_xmlDesignation.LastPL)
				bModified = true;
				
			//	Has the highlighter changed?
			if(ReferenceEquals(m_tmaxHighlighter, GetHighlighter()) == false)
			{
				m_tmaxHighlighter = GetHighlighter();
				bModified = true;
			}
			
			return bModified;
			
		}// private bool IsModified()
		
		/// <summary>This method is called to initialize the form for edit mode operation</summary>
		/// <returns>true if successful</returns>
		private bool InitializeEditor()
		{
			//	Set the title
			this.Text = "Designation Extents Editor";
			
			//	Hide the import button
			m_ctrlImport.Visible = false;
			
			//	Make sure we have the required objects
			if(m_xmlScript == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_NO_EDIT_SCRIPT), null);
			if(m_xmlDesignation == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_NO_EDIT_DESIGNATION), null);
			
			//	Initialize the extents controls
			try
			{
				m_ctrlStartPage.Text = (CTmaxToolbox.PLToPage(m_xmlDesignation.FirstPL)).ToString();
				m_ctrlStartLine.Text = (CTmaxToolbox.PLToLine(m_xmlDesignation.FirstPL)).ToString();
				m_ctrlStopPage.Text = (CTmaxToolbox.PLToPage(m_xmlDesignation.LastPL)).ToString();
				m_ctrlStopLine.Text = (CTmaxToolbox.PLToLine(m_xmlDesignation.LastPL)).ToString();
			}
			catch
			{
				return Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_EDIT_EXTENTS, m_xmlDesignation.FirstPL, m_xmlDesignation.LastPL), null);
			}
			
			//	Set the highlighter
			if((m_tmaxVideoOptions != null) && (m_tmaxVideoOptions.Highlighters != null))
			{
				m_tmaxHighlighter = m_tmaxVideoOptions.Highlighters.Find(m_xmlDesignation.Highlighter);
				
				SetHighlighter(m_tmaxHighlighter);
			}
				
			return true;
			
		}// private bool InitializeEditor()
		
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
				
				if((m_xmlScript.XmlDeposition.LinesPerPage > 0) && (m_iStartLine > m_xmlScript.XmlDeposition.LinesPerPage))
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_START_LINE_OUT_OF_RANGE, m_xmlScript.XmlDeposition.LinesPerPage), m_ctrlStartLine);
					break;
				}
				
				if((m_xmlScript.XmlDeposition.LinesPerPage > 0) && (m_iStopLine > m_xmlScript.XmlDeposition.LinesPerPage))
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_STOP_LINE_OUT_OF_RANGE, m_xmlScript.XmlDeposition.LinesPerPage), m_ctrlStopLine);
					break;
				}
				
				m_lStartPL = CTmaxToolbox.GetPL(m_lStartPage, m_iStartLine);
				m_lStopPL = CTmaxToolbox.GetPL(m_lStopPage, m_iStopLine);
				
				if(m_lStartPL < m_lMinimumPL)
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_START_PL_OUT_OF_RANGE), m_ctrlStartPage);
					break;
				}
				
				if((m_lMaximumPL > 0) && (m_lStopPL > m_lMaximumPL))
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
			CTmaxVideoArgs	Args = null;
			CTmaxItem		tmaxTarget = null;
			CTmaxParameters tmaxParameters = null;
			int				iIndex = -1;
			
			try
			{
				tmaxTarget = new CTmaxItem(m_xmlScript);
				tmaxParameters = new CTmaxParameters();

				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.AsciiMedia));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.MergeImported, true));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Before, InsertBefore));
				
				//	Are we inserting?
				if(m_xmlDesignation != null)
				{
					tmaxTarget.SubItems.Add(new CTmaxItem(m_xmlScript, m_xmlDesignation));
				}
				else
				{
					//	Request activation of the first new designation
					tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Activate, true));
				}
				
				Args = FireCommand(TmaxVideoCommands.Import, tmaxTarget, tmaxParameters);
			
				//	Were we successful?
				if((Args.Successful == true) && (Args.Result != null) && (Args.Result.SubItems.Count > 0))
				{
					//	We need to adjust the insertion point if inserting After
					if((this.XmlDesignation != null) && (this.InsertBefore == false))
					{
						//	Use the last new designation
						iIndex = Args.Result.SubItems.Count - 1;
							
						this.XmlDesignation = Args.Result.SubItems[iIndex].XmlDesignation;
							
						tmaxParameters = new CTmaxParameters();
						tmaxParameters.Add(TmaxCommandParameters.SyncMediaTree, true);
							
						FireCommand(TmaxVideoCommands.Activate, Args.Result.SubItems[iIndex], tmaxParameters);
						
					}// if((this.XmlDesignation != null) && (this.InsertBefore == false))
					
				}// if((Args.Successful == true) && (Args.Result != null) && (Args.Result.SubItems.Count > 0))
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickImport", m_tmaxErrorBuilder.Message(ERROR_IMPORT_EX), Ex);
			}
			
		}// private void OnClickImport(object sender, System.EventArgs e)
		
		/// <summary>This method will set the limits defined by the deposition</summary>
		private void SetDepositionLimits()
		{
			bool bValid = false;
			
			if((m_xmlScript != null) && (m_xmlScript.XmlDeposition != null))
			{
				m_xmlScript.XmlDeposition.GetPLRange(ref m_lMinimumPL, ref m_lMaximumPL);
				bValid = true;
			}
			else
			{
				m_lMinimumPL = -1;
				m_lMaximumPL = -1;
			}
			
			//	Do we have the required objects?
			m_ctrlStartLabel.Enabled = (bValid == true);
			m_ctrlStopLabel.Enabled = (bValid == true);
			m_ctrlMinimumLabel.Enabled = (bValid == true);
			m_ctrlMaximumLabel.Enabled = (bValid == true);
			m_ctrlDepositionPageLabel.Enabled = (bValid == true);
			m_ctrlDepositionLineLabel.Enabled = (bValid == true);
			
			m_ctrlMinimumPage.Enabled = (bValid == true);
			m_ctrlMinimumLine.Enabled = (bValid == true);
			m_ctrlMaximumPage.Enabled = (bValid == true);
			m_ctrlMaximumLine.Enabled = (bValid == true);

			m_ctrlStartPage.Enabled = (bValid == true);
			m_ctrlStartLine.Enabled = (bValid == true);
			m_ctrlStopPage.Enabled = (bValid == true);
			m_ctrlStopLine.Enabled = (bValid == true);
		
			m_ctrlOK.Enabled = (bValid == true);

			if(bValid == true)
			{
				m_ctrlMinimumPage.Text = (CTmaxToolbox.PLToPage(m_lMinimumPL)).ToString();
				m_ctrlMinimumLine.Text = (CTmaxToolbox.PLToLine(m_lMinimumPL)).ToString();
				m_ctrlMaximumPage.Text = (CTmaxToolbox.PLToPage(m_lMaximumPL)).ToString();
				m_ctrlMaximumLine.Text = (CTmaxToolbox.PLToLine(m_lMaximumPL)).ToString();
			}
			else
			{
				m_ctrlMinimumPage.Text = "";
				m_ctrlMaximumPage.Text = "";
				m_ctrlMaximumPage.Text = "";
				m_ctrlMaximumLine.Text = "";
			}	
				
		}// private void SetDepositionLimits()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The active script</summary>
		public CXmlScript XmlScript
		{
			get { return m_xmlScript; }
			set { m_xmlScript = value; }
		}
		
		/// <summary>The insertion point for Add mode / Active designation for Edit mode</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return m_xmlDesignation; }
			set { m_xmlDesignation = value; }
		}
		
		/// <summary>The highlighter used to initialize the form</summary>
		public CTmaxHighlighter Highlighter
		{
			get { return m_tmaxHighlighter; }
			set { m_tmaxHighlighter = value; }
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
		
		/// <summary>Global TmaxVideo application options</summary>
		public CTmaxVideoOptions VideoOptions
		{
			get { return m_tmaxVideoOptions; }
			set { m_tmaxVideoOptions = value; }
		}
		
		#endregion Properties
		
	}// public class CFTmaxVideoDesignations : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.TMVV.Tmvideo
