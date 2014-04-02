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

namespace FTI.Trialmax.Panes
{
	/// <summary>This class manages a form that allows the user to add new movie clips</summary>
	public class CFAddClips : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants
		
		protected const int ERROR_FILL_RECORDINGS_EX	= (ERROR_TMAX_FORM_MAX + 1);
		protected const int ERROR_FILL_SEGMENTS_EX		= (ERROR_TMAX_FORM_MAX + 2);
		protected const int ERROR_INVALID_START_HOURS	= (ERROR_TMAX_FORM_MAX + 3);
		protected const int ERROR_INVALID_START_MINUTES	= (ERROR_TMAX_FORM_MAX + 4);
		protected const int ERROR_INVALID_START_SECONDS	= (ERROR_TMAX_FORM_MAX + 5);
		protected const int ERROR_INVALID_STOP_HOURS	= (ERROR_TMAX_FORM_MAX + 6);
		protected const int ERROR_INVALID_STOP_MINUTES	= (ERROR_TMAX_FORM_MAX + 7);
		protected const int ERROR_INVALID_STOP_SECONDS	= (ERROR_TMAX_FORM_MAX + 8);
		protected const int ERROR_START_OUT_OF_RANGE	= (ERROR_TMAX_FORM_MAX + 9);
		protected const int ERROR_STOP_OUT_OF_RANGE		= (ERROR_TMAX_FORM_MAX + 10);
		protected const int ERROR_REVERSED_POSITIONS	= (ERROR_TMAX_FORM_MAX + 11);
		protected const int ERROR_ADD_CLIP_EX			= (ERROR_TMAX_FORM_MAX + 12);
		protected const int ERROR_ADD_SCENE_EX			= (ERROR_TMAX_FORM_MAX + 13);
		protected const int ERROR_EDIT_NO_SCRIPT		= (ERROR_TMAX_FORM_MAX + 14);
		protected const int ERROR_EDIT_NO_SCENE			= (ERROR_TMAX_FORM_MAX + 15);
		protected const int ERROR_EDIT_NO_SOURCE		= (ERROR_TMAX_FORM_MAX + 16);
		protected const int ERROR_EDIT_NO_CLIP			= (ERROR_TMAX_FORM_MAX + 17);
		protected const int ERROR_EDIT_NO_SEGMENT		= (ERROR_TMAX_FORM_MAX + 18);
		protected const int ERROR_EDIT_NO_RECORDING		= (ERROR_TMAX_FORM_MAX + 19);
		protected const int ERROR_EDIT_NO_EXTENT		= (ERROR_TMAX_FORM_MAX + 20);
		protected const int ERROR_EDIT_NO_DESIGNATION	= (ERROR_TMAX_FORM_MAX + 21);
		protected const int ERROR_SAVE_DESIGNATION_EX	= (ERROR_TMAX_FORM_MAX + 22);
		protected const int ERROR_UPDATE_CLIP_EX		= (ERROR_TMAX_FORM_MAX + 23);
		protected const int ERROR_UPDATE_SCENE_EX		= (ERROR_TMAX_FORM_MAX + 24);
	
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Combobox to display available recordings</summary>
		private System.Windows.Forms.ComboBox m_ctrlRecordings;
		
		/// <summary>Static label for recordings combobox</summary>
		private System.Windows.Forms.Label m_ctrlRecordingLabel;
		
		/// <summary>Edit box for stop position minutes</summary>
		private System.Windows.Forms.TextBox m_ctrlStopMinutes;
		
		/// <summary>Edit box for start position minutes</summary>
		private System.Windows.Forms.TextBox m_ctrlStartMinutes;
		
		/// <summary>Edit box for stop position hours</summary>
		private System.Windows.Forms.TextBox m_ctrlStopHours;
		
		/// <summary>Edit box for start position hours</summary>
		private System.Windows.Forms.TextBox m_ctrlStartHours;
		
		/// <summary>Static label for stop position</summary>
		private System.Windows.Forms.Label m_ctrlStopLabel;
		
		/// <summary>Static label for start position</summary>
		private System.Windows.Forms.Label m_ctrlStartLabel;
		
		/// <summary>Static text to display maximum time allowed</summary>
		private System.Windows.Forms.Label m_ctrlMaximum;
		
		/// <summary>Static label for maximum time</summary>
		private System.Windows.Forms.Label m_ctrlMaximumLabel;
		
		/// <summary>Combobox to display all available segments</summary>
		private System.Windows.Forms.ComboBox m_ctrlSegments;
		
		/// <summary>Static label for segments combobox</summary>
		private System.Windows.Forms.Label m_ctrlSegmentLabel;
		
		/// <summary>Static text to display the name of the active script</summary>
		private System.Windows.Forms.Label m_ctrlScript;
		
		/// <summary>Static label for active script</summary>
		private System.Windows.Forms.Label m_ctrlScriptLabel;
		
		/// <summary>Pushbutton to close the form</summary>
		private System.Windows.Forms.Button m_ctrlDone;
		
		/// <summary>Pushbutton to add/edit designation(s)</summary>
		private System.Windows.Forms.Button m_ctrlAdd;
		
		/// <summary>Edit box for start position seconds</summary>
		private System.Windows.Forms.TextBox m_ctrlStartSeconds;
		
		/// <summary>Edit box for stop position seconds</summary>
		private System.Windows.Forms.TextBox m_ctrlStopSeconds;
		
		/// <summary>Static label for position minutes</summary>
		private System.Windows.Forms.Label m_ctrlMinutesLabel;
		
		/// <summary>Static label for position hours</summary>
		private System.Windows.Forms.Label m_ctrlHoursLabel;
		
		/// <summary>Static label for position seconds</summary>
		private System.Windows.Forms.Label m_ctrlSecondsLabel;

		/// <summary>Local member bound to Database property</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member bound to Script property</summary>
		private CDxPrimary m_dxScript = null;
		
		/// <summary>Local member bound to Scene property</summary>
		private CDxSecondary m_dxScene = null;
		
		/// <summary>Local member to store reference to active recording record</summary>
		private CDxPrimary m_dxRecording = null;
		
		/// <summary>Local member to store reference to active segment record</summary>
		private CDxSecondary m_dxSegment = null;
		
		/// <summary>Local member to store reference to active extents record</summary>
		private CDxExtent m_dxExtent = null;
		
		/// <summary>Local member bound to Clip property</summary>
		private CDxTertiary m_dxClip = null;
		
		/// <summary>Local member bound to XmlDesignation property</summary>
		private CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member bound to Edit property</summary>
		private bool m_bEdit = false;
		
		/// <summary>Local member bound to InsertBefore property</summary>
		private bool m_bInsertBefore = false;

		/// <summary>Local member bound to Modified property</summary>
		private bool m_bModified = false;
		
		/// <summary>Local flag to inhibit event processing</summary>
		private bool m_bIgnoreEvents = false;
		
		/// <summary>Local member to store the start position</summary>
		private double m_dStartPosition = -1;
		
		/// <summary>Local member to store the start hours</summary>
		private double m_dStartHours = -1;
		
		/// <summary>Local member to store the start minutes</summary>
		private double m_dStartMinutes = -1;
		
		/// <summary>Local member to store the start seconds</summary>
		private double m_dStartSeconds = -1;
		
		/// <summary>Local member to store the stop position</summary>
		private double m_dStopPosition = -1;
		
		/// <summary>Local member to store the stop hours</summary>
		private double m_dStopHours = -1;
		
		/// <summary>Local member to store the stop minutes</summary>
		private double m_dStopMinutes = -1;
		
		/// <summary>Local member to store the stop seconds</summary>
		private double m_dStopSeconds = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFAddClips() : base()
		{
			// Initialize the child controls
			InitializeComponent();
		}
		
		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>This method is called to initialize the form for edit mode operation</summary>
		/// <returns>true if successful</returns>
		protected bool InitEditMode()
		{
			CDxMediaRecord	dxSource = null;
			double		dHours = 0;
			double		dMinutes = 0;
			double		dSeconds = 0;
			
			//	Rename the action buttons
			m_ctrlAdd.Text = "&OK";
			m_ctrlDone.Text = "&Cancel";
			this.AcceptButton = null;
			
			//	Disable the recording and segment selection boxes
			m_ctrlRecordings.Enabled = false;
			m_ctrlSegments.Enabled = false;
			
			//	Make sure we have the required objects
			if(m_dxScript == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_SCRIPT), null);
			if(m_dxScene == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_SCENE), null);
			if(m_xmlDesignation == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_DESIGNATION), null);
				
			//	Get the source for the active scene
			if((dxSource = m_dxScene.GetSource()) == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_SOURCE), null);
			
			//	The source should be a movie clip
			if(dxSource.MediaType == TmaxMediaTypes.Clip)
				m_dxClip = (CDxTertiary)dxSource;
			else
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_CLIP), null);

			//	Get the segment and recording records
			if(m_dxClip.Secondary != null)
				m_dxSegment = m_dxClip.Secondary;
			else
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_SEGMENT), null);
			
			if(m_dxSegment.Primary != null)
				m_dxRecording = m_dxSegment.Primary;
			else
			{
				m_dxSegment = null; //	Causes all controls to be disabled
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_RECORDING), null);
			}
			
			//	Get the segment extent record
			if((m_dxExtent = m_dxSegment.GetExtent()) == null)
				return Warn(m_tmaxErrorBuilder.Message(ERROR_EDIT_NO_EXTENT), null);
			
			//	Inhibit event processing while we set all this up
			m_bIgnoreEvents = true;
			
			//	Set the recording selection
			try
			{
				foreach(CDxPrimary O in m_ctrlRecordings.Items)
				{
					if(ReferenceEquals(O, m_dxRecording) == true)
					{
						m_ctrlRecordings.SelectedItem = O;
						break;
					}
					
				}
			}
			catch
			{
			}

			//	Populate the segments list box using the current recording
			FillSegments();
			
			//	Match up the segment selection
			try
			{
				foreach(CDxSecondary O in m_ctrlSegments.Items)
				{
					if(ReferenceEquals(O, m_dxSegment) == true)
					{
						m_ctrlSegments.SelectedItem = O;
						break;
					}
					
				}
			}
			catch
			{
			}

			//	Initialize the start selection
			dHours = (double)((int)(m_xmlDesignation.Start / 3600.0));
			dMinutes = (double)((int)((m_xmlDesignation.Start - (dHours * 3600)) / 60));
			dSeconds = m_xmlDesignation.Start - (dHours * 3600) - (dMinutes * 60);
			
			if((HoursEnabled() == false) || (dHours <= 0))
				m_ctrlStartHours.Text = "";
			else
				m_ctrlStartHours.Text = dHours.ToString();
				
			if((MinutesEnabled() == false) || (dMinutes <= 0))
				m_ctrlStartMinutes.Text = "";
			else
				m_ctrlStartMinutes.Text = dMinutes.ToString();
				
			m_ctrlStartSeconds.Text = dSeconds.ToString();
				
			//	Initialize the stop selection
			dHours = (double)((int)(m_xmlDesignation.Stop / 3600.0));
			dMinutes = (double)((int)((m_xmlDesignation.Stop - (dHours * 3600)) / 60));
			dSeconds = m_xmlDesignation.Stop - (dHours * 3600) - (dMinutes * 60);
			
			if((HoursEnabled() == false) || (dHours <= 0))
				m_ctrlStopHours.Text = "";
			else
				m_ctrlStopHours.Text = dHours.ToString();
				
			if((MinutesEnabled() == false) || (dMinutes <= 0))
				m_ctrlStopMinutes.Text = "";
			else
				m_ctrlStopMinutes.Text = dMinutes.ToString();
				
			m_ctrlStopSeconds.Text = dSeconds.ToString();
				
			//	Put focus on the first edit box
			Select(GetFirstStartControl());
			
			//	Enable event processing
			m_bIgnoreEvents = false;
			
			return true;
			
		}// protected bool InitEditMode()
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		protected void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFAddClips));
			this.m_ctrlRecordings = new System.Windows.Forms.ComboBox();
			this.m_ctrlRecordingLabel = new System.Windows.Forms.Label();
			this.m_ctrlStopMinutes = new System.Windows.Forms.TextBox();
			this.m_ctrlStartMinutes = new System.Windows.Forms.TextBox();
			this.m_ctrlStopHours = new System.Windows.Forms.TextBox();
			this.m_ctrlStartHours = new System.Windows.Forms.TextBox();
			this.m_ctrlStopLabel = new System.Windows.Forms.Label();
			this.m_ctrlStartLabel = new System.Windows.Forms.Label();
			this.m_ctrlMaximum = new System.Windows.Forms.Label();
			this.m_ctrlMaximumLabel = new System.Windows.Forms.Label();
			this.m_ctrlSegments = new System.Windows.Forms.ComboBox();
			this.m_ctrlSegmentLabel = new System.Windows.Forms.Label();
			this.m_ctrlScript = new System.Windows.Forms.Label();
			this.m_ctrlScriptLabel = new System.Windows.Forms.Label();
			this.m_ctrlDone = new System.Windows.Forms.Button();
			this.m_ctrlAdd = new System.Windows.Forms.Button();
			this.m_ctrlStartSeconds = new System.Windows.Forms.TextBox();
			this.m_ctrlStopSeconds = new System.Windows.Forms.TextBox();
			this.m_ctrlMinutesLabel = new System.Windows.Forms.Label();
			this.m_ctrlHoursLabel = new System.Windows.Forms.Label();
			this.m_ctrlSecondsLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlRecordings
			// 
			this.m_ctrlRecordings.DisplayMember = "DisplayString";
			this.m_ctrlRecordings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlRecordings.IntegralHeight = false;
			this.m_ctrlRecordings.Location = new System.Drawing.Point(112, 28);
			this.m_ctrlRecordings.Name = "m_ctrlRecordings";
			this.m_ctrlRecordings.Size = new System.Drawing.Size(176, 21);
			this.m_ctrlRecordings.TabIndex = 8;
			this.m_ctrlRecordings.TabStop = false;
			this.m_ctrlRecordings.SelectedIndexChanged += new System.EventHandler(this.OnRecordingSelChanged);
			// 
			// m_ctrlRecordingLabel
			// 
			this.m_ctrlRecordingLabel.Location = new System.Drawing.Point(8, 32);
			this.m_ctrlRecordingLabel.Name = "m_ctrlRecordingLabel";
			this.m_ctrlRecordingLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlRecordingLabel.TabIndex = 37;
			this.m_ctrlRecordingLabel.Text = "Recording:";
			this.m_ctrlRecordingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStopMinutes
			// 
			this.m_ctrlStopMinutes.AcceptsReturn = true;
			this.m_ctrlStopMinutes.Location = new System.Drawing.Point(174, 164);
			this.m_ctrlStopMinutes.Name = "m_ctrlStopMinutes";
			this.m_ctrlStopMinutes.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlStopMinutes.TabIndex = 4;
			this.m_ctrlStopMinutes.Text = "";
			this.m_ctrlStopMinutes.WordWrap = false;
			this.m_ctrlStopMinutes.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStartMinutes
			// 
			this.m_ctrlStartMinutes.AcceptsReturn = true;
			this.m_ctrlStartMinutes.Location = new System.Drawing.Point(174, 136);
			this.m_ctrlStartMinutes.Name = "m_ctrlStartMinutes";
			this.m_ctrlStartMinutes.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlStartMinutes.TabIndex = 1;
			this.m_ctrlStartMinutes.Text = "";
			this.m_ctrlStartMinutes.WordWrap = false;
			this.m_ctrlStartMinutes.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStopHours
			// 
			this.m_ctrlStopHours.AcceptsReturn = true;
			this.m_ctrlStopHours.Location = new System.Drawing.Point(112, 164);
			this.m_ctrlStopHours.Name = "m_ctrlStopHours";
			this.m_ctrlStopHours.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlStopHours.TabIndex = 3;
			this.m_ctrlStopHours.Text = "";
			this.m_ctrlStopHours.WordWrap = false;
			this.m_ctrlStopHours.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStartHours
			// 
			this.m_ctrlStartHours.AcceptsReturn = true;
			this.m_ctrlStartHours.Location = new System.Drawing.Point(112, 136);
			this.m_ctrlStartHours.Name = "m_ctrlStartHours";
			this.m_ctrlStartHours.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlStartHours.TabIndex = 0;
			this.m_ctrlStartHours.Text = "";
			this.m_ctrlStartHours.WordWrap = false;
			this.m_ctrlStartHours.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStopLabel
			// 
			this.m_ctrlStopLabel.Location = new System.Drawing.Point(8, 168);
			this.m_ctrlStopLabel.Name = "m_ctrlStopLabel";
			this.m_ctrlStopLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlStopLabel.TabIndex = 36;
			this.m_ctrlStopLabel.Text = "Stop:";
			this.m_ctrlStopLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStartLabel
			// 
			this.m_ctrlStartLabel.Location = new System.Drawing.Point(8, 140);
			this.m_ctrlStartLabel.Name = "m_ctrlStartLabel";
			this.m_ctrlStartLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlStartLabel.TabIndex = 35;
			this.m_ctrlStartLabel.Text = "Start:";
			this.m_ctrlStartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMaximum
			// 
			this.m_ctrlMaximum.Location = new System.Drawing.Point(112, 92);
			this.m_ctrlMaximum.Name = "m_ctrlMaximum";
			this.m_ctrlMaximum.Size = new System.Drawing.Size(176, 12);
			this.m_ctrlMaximum.TabIndex = 34;
			this.m_ctrlMaximum.Text = "0";
			this.m_ctrlMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMaximumLabel
			// 
			this.m_ctrlMaximumLabel.Location = new System.Drawing.Point(8, 92);
			this.m_ctrlMaximumLabel.Name = "m_ctrlMaximumLabel";
			this.m_ctrlMaximumLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlMaximumLabel.TabIndex = 30;
			this.m_ctrlMaximumLabel.Text = "Maximum Time:";
			this.m_ctrlMaximumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSegments
			// 
			this.m_ctrlSegments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlSegments.IntegralHeight = false;
			this.m_ctrlSegments.Location = new System.Drawing.Point(112, 60);
			this.m_ctrlSegments.Name = "m_ctrlSegments";
			this.m_ctrlSegments.Size = new System.Drawing.Size(176, 21);
			this.m_ctrlSegments.TabIndex = 9;
			this.m_ctrlSegments.TabStop = false;
			this.m_ctrlSegments.SelectedIndexChanged += new System.EventHandler(this.OnSegmentSelChanged);
			// 
			// m_ctrlSegmentLabel
			// 
			this.m_ctrlSegmentLabel.Location = new System.Drawing.Point(8, 64);
			this.m_ctrlSegmentLabel.Name = "m_ctrlSegmentLabel";
			this.m_ctrlSegmentLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlSegmentLabel.TabIndex = 28;
			this.m_ctrlSegmentLabel.Text = "Segment:";
			this.m_ctrlSegmentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlScript
			// 
			this.m_ctrlScript.Location = new System.Drawing.Point(112, 8);
			this.m_ctrlScript.Name = "m_ctrlScript";
			this.m_ctrlScript.Size = new System.Drawing.Size(176, 12);
			this.m_ctrlScript.TabIndex = 26;
			this.m_ctrlScript.Text = "script name here";
			this.m_ctrlScript.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlScriptLabel
			// 
			this.m_ctrlScriptLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlScriptLabel.Name = "m_ctrlScriptLabel";
			this.m_ctrlScriptLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlScriptLabel.TabIndex = 24;
			this.m_ctrlScriptLabel.Text = "Script:";
			this.m_ctrlScriptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDone
			// 
			this.m_ctrlDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlDone.Location = new System.Drawing.Point(213, 200);
			this.m_ctrlDone.Name = "m_ctrlDone";
			this.m_ctrlDone.TabIndex = 7;
			this.m_ctrlDone.TabStop = false;
			this.m_ctrlDone.Text = "&Done";
			this.m_ctrlDone.Click += new System.EventHandler(this.OnDone);
			// 
			// m_ctrlAdd
			// 
			this.m_ctrlAdd.Location = new System.Drawing.Point(128, 200);
			this.m_ctrlAdd.Name = "m_ctrlAdd";
			this.m_ctrlAdd.TabIndex = 6;
			this.m_ctrlAdd.Text = "&Add";
			this.m_ctrlAdd.Click += new System.EventHandler(this.OnAdd);
			// 
			// m_ctrlStartSeconds
			// 
			this.m_ctrlStartSeconds.AcceptsReturn = true;
			this.m_ctrlStartSeconds.Location = new System.Drawing.Point(236, 136);
			this.m_ctrlStartSeconds.Name = "m_ctrlStartSeconds";
			this.m_ctrlStartSeconds.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlStartSeconds.TabIndex = 2;
			this.m_ctrlStartSeconds.Text = "";
			this.m_ctrlStartSeconds.WordWrap = false;
			this.m_ctrlStartSeconds.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlStopSeconds
			// 
			this.m_ctrlStopSeconds.AcceptsReturn = true;
			this.m_ctrlStopSeconds.Location = new System.Drawing.Point(236, 164);
			this.m_ctrlStopSeconds.Name = "m_ctrlStopSeconds";
			this.m_ctrlStopSeconds.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlStopSeconds.TabIndex = 5;
			this.m_ctrlStopSeconds.Text = "";
			this.m_ctrlStopSeconds.WordWrap = false;
			this.m_ctrlStopSeconds.Enter += new System.EventHandler(this.OnEnterExtent);
			// 
			// m_ctrlMinutesLabel
			// 
			this.m_ctrlMinutesLabel.Location = new System.Drawing.Point(174, 116);
			this.m_ctrlMinutesLabel.Name = "m_ctrlMinutesLabel";
			this.m_ctrlMinutesLabel.Size = new System.Drawing.Size(52, 12);
			this.m_ctrlMinutesLabel.TabIndex = 41;
			this.m_ctrlMinutesLabel.Text = "Minutes";
			this.m_ctrlMinutesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlHoursLabel
			// 
			this.m_ctrlHoursLabel.Location = new System.Drawing.Point(112, 116);
			this.m_ctrlHoursLabel.Name = "m_ctrlHoursLabel";
			this.m_ctrlHoursLabel.Size = new System.Drawing.Size(52, 12);
			this.m_ctrlHoursLabel.TabIndex = 40;
			this.m_ctrlHoursLabel.Text = "Hours";
			this.m_ctrlHoursLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlSecondsLabel
			// 
			this.m_ctrlSecondsLabel.Location = new System.Drawing.Point(236, 116);
			this.m_ctrlSecondsLabel.Name = "m_ctrlSecondsLabel";
			this.m_ctrlSecondsLabel.Size = new System.Drawing.Size(52, 12);
			this.m_ctrlSecondsLabel.TabIndex = 42;
			this.m_ctrlSecondsLabel.Text = "Seconds";
			this.m_ctrlSecondsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CFAddClips
			// 
			this.AcceptButton = this.m_ctrlAdd;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlDone;
			this.ClientSize = new System.Drawing.Size(296, 229);
			this.Controls.Add(this.m_ctrlSecondsLabel);
			this.Controls.Add(this.m_ctrlMinutesLabel);
			this.Controls.Add(this.m_ctrlHoursLabel);
			this.Controls.Add(this.m_ctrlStopSeconds);
			this.Controls.Add(this.m_ctrlStartSeconds);
			this.Controls.Add(this.m_ctrlStopMinutes);
			this.Controls.Add(this.m_ctrlStartMinutes);
			this.Controls.Add(this.m_ctrlStopHours);
			this.Controls.Add(this.m_ctrlStartHours);
			this.Controls.Add(this.m_ctrlRecordingLabel);
			this.Controls.Add(this.m_ctrlStopLabel);
			this.Controls.Add(this.m_ctrlStartLabel);
			this.Controls.Add(this.m_ctrlMaximum);
			this.Controls.Add(this.m_ctrlMaximumLabel);
			this.Controls.Add(this.m_ctrlSegments);
			this.Controls.Add(this.m_ctrlSegmentLabel);
			this.Controls.Add(this.m_ctrlScript);
			this.Controls.Add(this.m_ctrlScriptLabel);
			this.Controls.Add(this.m_ctrlDone);
			this.Controls.Add(this.m_ctrlAdd);
			this.Controls.Add(this.m_ctrlRecordings);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddClips";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Add Clips";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while filling the list of available recordings");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while filling the list of available segments");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid start hours value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid start minutes value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid start seconds value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid stop hours value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid stop minutes value.");
			m_tmaxErrorBuilder.FormatStrings.Add("You must supply a valid stop seconds value.");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified start position is out of range: position = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The specified stop position is out of range: position = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The stop position must be greater than the start position:\n\nstart = %1\nstop = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the new clip to the database");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the new script scene to the database");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. No script has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. No scene has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. Can not retrieve the source record for the active script scene.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. The active script scene is not associated with a valid movie clip.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. Can not retrieve the record for the active segment.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. Can not retrieve the record for the active recording.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. Can not retrieve the extents record for the active segment.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to enter edit mode. No XML designation has been specified.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to save the XML designation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the clip record.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the scene record.");
		
		}// protected override void SetErrorStrings()

		/// <summary>This method is called to select the specified control</summary>
		/// <param name="ctrlTextBox">the desired text box control to be selected</param>
		protected void Select(System.Windows.Forms.TextBox ctrlTextBox)
		{
			ctrlTextBox.Focus();
			ctrlTextBox.SelectAll();
		}
		
		/// <summary>This method will enable/disable the child controls based on current property values</summary>
		protected void SetControlStates()
		{
			//	Do we have the required objects?
			m_ctrlStartLabel.Enabled = (m_dxExtent != null);
			m_ctrlStopLabel.Enabled = (m_dxExtent != null);
			m_ctrlHoursLabel.Enabled = (HoursEnabled() == true);
			m_ctrlMinutesLabel.Enabled = (MinutesEnabled() == true);
			m_ctrlSecondsLabel.Enabled = (m_dxExtent != null);
			
			m_ctrlMaximumLabel.Enabled = (m_dxExtent != null);
			m_ctrlMaximum.Enabled = (m_dxExtent != null);

			m_ctrlStartHours.Enabled = (HoursEnabled() == true);
			m_ctrlStartMinutes.Enabled = (MinutesEnabled() == true);
			m_ctrlStartSeconds.Enabled = (m_dxExtent != null);
			m_ctrlStopHours.Enabled = (HoursEnabled() == true);
			m_ctrlStopMinutes.Enabled = (MinutesEnabled() == true);
			m_ctrlStopSeconds.Enabled = (m_dxExtent != null);
			
			m_ctrlAdd.Enabled = (m_dxExtent != null);

			if(m_dxExtent != null)
			{
				m_ctrlMaximum.Text = CTmaxToolbox.SecondsToString(m_dxExtent.Stop);

			}
			else
			{
				m_ctrlMaximum.Text = "";
			}	
		
		}// protected void SetControlStates()
		
		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		protected bool Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
				            MessageBoxIcon.Exclamation);
			
			if(ctrlSelect != null)
				Select(ctrlSelect);	
				
			return false; // allows for cleaner code						
		
		}// protected void Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>This method will populate the list of recordings</summary>
		protected void FillRecordings()
		{
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			try
			{
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				{
					foreach(CDxPrimary O in m_tmaxDatabase.Primaries)
					{
						//	Is this a recording?
						if(O.MediaType == TmaxMediaTypes.Recording)
						{
							m_ctrlRecordings.Items.Add(O);
						}
						
					}
					
					//	Select the first recording if not in edit mode
					if((Edit == false) && (m_ctrlRecordings.Items.Count > 0))
						m_ctrlRecordings.SelectedIndex = 0;
					
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillRecordings", m_tmaxErrorBuilder.Message(ERROR_FILL_RECORDINGS_EX), Ex);
			}
			
		}// protected void FillRecordings()
		
		/// <summary>This method will populate the list of segments</summary>
		protected void FillSegments()
		{
			Debug.Assert(m_ctrlSegments != null);
			Debug.Assert(m_ctrlSegments.IsDisposed == false);
			Debug.Assert(m_ctrlSegments.Items != null);
			if(m_ctrlSegments == null) return;
			if(m_ctrlSegments.IsDisposed == true) return;
			if(m_ctrlSegments.Items == null) return;
			
			try
			{
				//	Clear the existing segments
				m_ctrlSegments.Items.Clear();
				
				//	ONLY RESET IF NOT IN EDIT MODE
				if(m_bEdit == false)
				{
					m_dxSegment = null;
					m_dxExtent = null;
				}
				
				//	Do we have an active recording?
				if((m_dxRecording != null) && (m_dxRecording.Secondaries != null))
				{
					//	Make sure the child collection has been populated
					if(m_dxRecording.Secondaries.Count == 0)
						m_dxRecording.Fill();
						
					foreach(CDxSecondary O in m_dxRecording.Secondaries)
					{
						m_ctrlSegments.Items.Add(O);
					}
					
					//	Select the first segment if not in edit mode
					if((Edit == false) && (m_ctrlSegments.Items.Count > 0))
						m_ctrlSegments.SelectedIndex = 0;
					
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillSegments", m_tmaxErrorBuilder.Message(ERROR_FILL_SEGMENTS_EX), Ex);
			}
			
		}// protected void FillSegments()
		
		/// <summary>Called when the form's Load event is trapped</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			if(m_dxScript != null)
				m_ctrlScript.Text = m_dxScript.GetText();
				
			//	Populate the recordings combobox
			FillRecordings();
			
			//	Are we editing extents?
			if(Edit == true)
			{
				if(InitEditMode() == false)
				{
					//	Close the form
					DialogResult = DialogResult.Abort;
					this.Close();
				}
				
			}
			
			//	Make sure all children are properly initialized
			SetControlStates();
		
		}// protected void OnLoad(object sender, System.EventArgs e)

		/// <summary>Called when the user selects a new recording</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnRecordingSelChanged(object sender, System.EventArgs e)
		{
			if(m_bIgnoreEvents == false)
			{
				//	Update the local member
				m_dxRecording = (CDxPrimary)m_ctrlRecordings.SelectedItem;
				
				//	Fill the segments
				FillSegments();
			}
		
		}// private void OnRecordingSelChanged(object sender, System.EventArgs e)

		/// <summary>Called when the user selects a new segment</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnSegmentSelChanged(object sender, System.EventArgs e)
		{
			if(m_bIgnoreEvents == false)
			{
				//	Update the local member
				m_dxSegment = (CDxSecondary)m_ctrlSegments.SelectedItem;
				
				if(m_dxSegment != null)
					m_dxExtent = m_dxSegment.GetExtent();
				else
					m_dxExtent = null;
					
				//	Update the controls
				SetControlStates();
				
				//	Put focus on the first edit box
				Select(GetFirstStartControl());
			}
		
		}// private void OnSegmentSelChanged(object sender, System.EventArgs e)
		
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

		/// <summary>Called when the user clicks on the Done button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnDone(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		/// <summary>Called to edit the active clip using the values specified by the user</summary>
		/// <returns>True if successful</returns>
		protected bool EditClip()
		{
			//	Do we have an active recording and segment?
			Debug.Assert(m_dxScript != null);
			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_dxClip != null);
			Debug.Assert(m_dxClip.GetExtent() != null);
			Debug.Assert(m_xmlDesignation != null);
			if(m_dxScript == null) return false;
			if(m_dxScene == null) return false;
			if(m_dxClip == null) return false;
			if(m_dxClip.GetExtent() == null) return false;
			if(m_xmlDesignation == null) return false;
			
			m_bModified = false;
			
			//	Has the start position changed?
			if(m_dStartPosition != m_dxClip.GetExtent().Start)
			{
				m_xmlDesignation.Start = m_dStartPosition;
				m_xmlDesignation.StartTuned = false;
				m_bModified = true;
			}
			if(m_dStopPosition != m_dxClip.GetExtent().Stop)
			{
				m_xmlDesignation.Stop = m_dStopPosition;
				m_xmlDesignation.StopTuned = false;
				m_bModified = true;
			}
			
			//	Nothing to do if nothing changed
			if(m_bModified == false) return true;
			
			//	Update the xml information
			m_xmlDesignation.ModifiedBy = m_tmaxDatabase.GetUserName();
			m_xmlDesignation.ModifiedOn = System.DateTime.Now.ToString();
			if(m_dxClip.Secondary.Name.Length > 0)
				m_xmlDesignation.SetNameFromExtents(m_dxClip.Secondary.Name);
			else
				m_xmlDesignation.SetNameFromExtents(m_dxClip.Secondary.Filename);
			
			//	Save the XML file
			try
			{
				if(m_xmlDesignation.Save() == false)
					m_bModified = false;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "EditClip", m_tmaxErrorBuilder.Message(ERROR_SAVE_DESIGNATION_EX), Ex);
				m_bModified = false;
			}
			
			//	Did our attempt to save the XML fail?
			if(m_bModified == false)
			{
				//	Restore the XML file
				m_xmlDesignation.Start = m_dxClip.GetExtent().Start;
				m_xmlDesignation.StartTuned = m_dxClip.GetExtent().StartTuned;
				m_xmlDesignation.Stop = m_dxClip.GetExtent().Stop;
				m_xmlDesignation.StopTuned = m_dxClip.GetExtent().StopTuned;
				m_xmlDesignation.ModifiedBy = m_tmaxDatabase.GetUserName(m_dxClip.ModifiedBy);
				m_xmlDesignation.ModifiedOn = m_dxClip.ModifiedOn.ToString();
				
				return false;
			}
			
			//	Update the clip record
			try
			{
				m_dxClip.GetExtent().Start = m_xmlDesignation.Start;
				m_dxClip.GetExtent().StartTuned = m_xmlDesignation.StartTuned;
				m_dxClip.GetExtent().Stop = m_xmlDesignation.Stop;
				m_dxClip.GetExtent().StopTuned = m_xmlDesignation.StopTuned;
				m_dxClip.Name = m_xmlDesignation.Name; // Name built with new extents
				
				FireCommand(TmaxCommands.Update, new CTmaxItem(m_dxClip));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "EditClip", m_tmaxErrorBuilder.Message(ERROR_UPDATE_CLIP_EX), Ex);
				return false;
			}
			
			//	Update the scene record
			try
			{
				FireCommand(TmaxCommands.Update, new CTmaxItem(m_dxScene));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "EditClip", m_tmaxErrorBuilder.Message(ERROR_UPDATE_SCENE_EX), Ex);
				return false;
			}
			
			return true;		
		
		}// protected bool EditClip()
		
		/// <summary>Called to create and add a new clip to the database</summary>
		/// <returns>A reference to the new record exchange object for the clip</returns>
		protected CDxTertiary AddClip()
		{
			CXmlDesignation xmlDesignation = null;
			CTmaxItem		tmaxParent = null;
			CTmaxItem		tmaxDesignation = null;
			
			try
			{
				//	Create a new XML designation to represent the clip
				xmlDesignation = new CXmlDesignation();
				if(xmlDesignation.EventSource.ErrorHooked == false)
					xmlDesignation.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
				if(xmlDesignation.EventSource.DiagnosticHooked == false)
					xmlDesignation.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
				
				//	Intialize the properties
				xmlDesignation.Start = m_dStartPosition;
				xmlDesignation.Stop = m_dStopPosition;
				xmlDesignation.StartTuned = false;
				xmlDesignation.StopTuned = false;
				xmlDesignation.HasText = false;
				xmlDesignation.ScrollText = false;
				if(m_dxSegment.Name.Length > 0)
					xmlDesignation.SetNameFromExtents(m_dxSegment.Name);
				else
					xmlDesignation.SetNameFromExtents(m_dxSegment.Filename);
				
				//	Create a parent item for the segment
				tmaxParent = new CTmaxItem(m_dxSegment);
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
				   (tmaxDesignation.MediaType == TmaxMediaTypes.Clip))
				{
					return (CDxTertiary)tmaxDesignation.GetMediaRecord();
				}
				else
				{
					return null;
				}				

			}
			catch
			{
				return null;
			}
			
		}// protected bool AddClip()
		
		/// <summary>This method is called to add a new scene to the active script</summary>
		/// <param name="dxClip">the clip record that will be the source for the scene</param>
		/// <returns>true if successful</returns>
		protected bool AddScene(CDxTertiary dxClip)
		{
			CTmaxItem			tmaxScript = null;
			CTmaxItem			tmaxClip = null;
			CTmaxParameters		tmaxParameters = null;
			
			Debug.Assert(dxClip != null);
			Debug.Assert(m_dxScript != null);
			if(dxClip == null) return false;
			if(m_dxScript == null) return false;
			
			try
			{
				//	Create an event item for the parent script
				tmaxScript = new CTmaxItem(m_dxScript);
					
				//	Assign the source items
				if(tmaxScript.SourceItems == null)
					tmaxScript.SourceItems = new CTmaxItems();
					
				tmaxClip = new CTmaxItem(dxClip);
				tmaxScript.SourceItems.Add(tmaxClip);
					
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Activate, true);
					
				//	Are we inserting into the script
				if(m_dxScene != null)
				{
					Debug.Assert(ReferenceEquals(m_dxScene.Primary, m_dxScript) == true);
					
					//	Create the required parameters for the event
					tmaxParameters.Add(TmaxCommandParameters.Before, m_bInsertBefore);
					
					//	Put the insertion point in the subitem collection
					tmaxScript.SubItems.Add(new CTmaxItem(m_dxScene));
				}

				//	Fire the event
				FireCommand(TmaxCommands.Add, tmaxScript, tmaxParameters);
				
				//	We need to adjust the active scene if we were inserting after
				//	in order to keep the clips in the correct order
				if((m_dxScene != null) && (m_bInsertBefore == false))
				{
					//	Make the new scene the active scene.
					//
					//	The database returns the new record in the original item
					if((tmaxClip.ReturnItem != null) && (tmaxClip.ReturnItem.GetMediaRecord() != null))
					{
						Debug.Assert(tmaxClip.ReturnItem.MediaType == TmaxMediaTypes.Scene);
						m_dxScene = (CDxSecondary)(tmaxClip.ReturnItem.GetMediaRecord());
					}
					
				}
					
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddScene", m_tmaxErrorBuilder.Message(ERROR_ADD_SCENE_EX), Ex);
				return false;
			}				

		}// protected bool AddScenes(CTmaxItems tmaxDesignations)
		
		/// <summary>Called when the user clicks on the Add button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnAdd(object sender, System.EventArgs e)
		{
			CDxTertiary	dxClip = null;
			
			//	Do we have an active recording and segment?
			Debug.Assert(m_dxRecording != null);
			Debug.Assert(m_dxSegment != null);
			Debug.Assert(m_dxExtent != null);
			if(m_dxRecording == null) return;
			if(m_dxSegment == null) return;
			if(m_dxExtent == null) return;
		
			//	Check the user supplied values
			if(CheckRange() == false) return; 
					
			Cursor.Current = Cursors.WaitCursor;
			
			//	Are we in edit mode?
			if(m_bEdit == true)
			{
				EditClip();
			}
			else
			{
				//	Add the clip to the database
				if((dxClip = AddClip()) != null)
				{
					//	Add the script scene
					AddScene(dxClip);
				}
				
			}// if(m_bEdit == true)
					
			Cursor.Current = Cursors.Default;
				
			//	Set up for user to start entering another clip or close if editing
			if(m_bEdit == true)
			{
				DialogResult = (m_bModified == true) ? DialogResult.OK : DialogResult.Cancel;
				this.Close();
			}
			else
			{
				Select(GetFirstStartControl());
			}
			
		}// protected void OnAdd(object sender, System.EventArgs e)

		/// <summary>This method is called to determine if the hour controls should be enabled</summary>
		/// <returns>true if they should be enabled</returns>
		protected bool HoursEnabled()
		{
			if(m_dxExtent != null)
			{
				if((m_dxExtent.Stop <= 0) || (m_dxExtent.Stop >= 3600.0))
				{
					return true;
				}
				
			}
			
			return false;
		
		}// protected bool HoursEnabled()
		
		/// <summary>This method is called to determine if the minute controls should be enabled</summary>
		/// <returns>true if they should be enabled</returns>
		protected bool MinutesEnabled()
		{
			if(m_dxExtent != null)
			{
				if((m_dxExtent.Stop <= 0) || (m_dxExtent.Stop >= 60.0))
				{
					return true;
				}
				
			}
			
			return false;
		
		}// protected bool MinutesEnabled()
		
		/// <summary>This method is called to get the first enabled start position control</summary>
		/// <returns>The first start position edit box that is enabled</returns>
		protected System.Windows.Forms.TextBox GetFirstStartControl()
		{
			if(HoursEnabled() == true)
				return m_ctrlStartHours;
			else if(MinutesEnabled() == true)
				return m_ctrlStartMinutes;
			else
				return m_ctrlStartSeconds;
		
		}// protected System.Windows.Forms.TextBox GetFirstStartControl()
		
		/// <summary>This method is called to get the first enabled stop position control</summary>
		/// <returns>The first start position edit box that is enabled</returns>
		protected System.Windows.Forms.TextBox GetFirstStopControl()
		{
			if(HoursEnabled() == true)
				return m_ctrlStopHours;
			else if(MinutesEnabled() == true)
				return m_ctrlStopMinutes;
			else
				return m_ctrlStopSeconds;
		
		}// protected System.Windows.Forms.TextBox GetFirstStopControl()
		
		/// <summary>This method is called to verify the start / stop values specified by the user</summary>
		/// <returns>true if the specified range is ok</returns>
		protected bool CheckRange()
		{
			bool bOk = false;
			
			while(bOk == false)
			{
				try
				{
					if((HoursEnabled() == true) && (m_ctrlStartHours.Text.Length > 0))
						m_dStartHours = System.Convert.ToDouble(m_ctrlStartHours.Text);
					else
						m_dStartHours = 0.0;
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_START_HOURS), m_ctrlStartHours);
					break;
				}
				
				try
				{
					if((MinutesEnabled() == true) && (m_ctrlStartMinutes.Text.Length > 0))
						m_dStartMinutes = System.Convert.ToDouble(m_ctrlStartMinutes.Text);
					else
						m_dStartMinutes = 0.0;
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_START_MINUTES), m_ctrlStartMinutes);
					break;
				}
				
				try
				{
					if(m_ctrlStartSeconds.Text.Length > 0)
						m_dStartSeconds = System.Convert.ToDouble(m_ctrlStartSeconds.Text);
					else
						m_dStartSeconds = 0.0;
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_START_SECONDS), m_ctrlStartSeconds);
					break;
				}
				
				try
				{
					if((HoursEnabled() == true) && (m_ctrlStopHours.Text.Length > 0))
						m_dStopHours = System.Convert.ToDouble(m_ctrlStopHours.Text);
					else
						m_dStopHours = 0.0;
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_STOP_HOURS), m_ctrlStopHours);
					break;
				}
				
				try
				{
					if((MinutesEnabled() == true) && (m_ctrlStopMinutes.Text.Length > 0))
						m_dStopMinutes = System.Convert.ToDouble(m_ctrlStopMinutes.Text);
					else
						m_dStopMinutes = 0.0;
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_STOP_MINUTES), m_ctrlStopMinutes);
					break;
				}
				
				try
				{
					if(m_ctrlStopSeconds.Text.Length > 0)
						m_dStopSeconds = System.Convert.ToDouble(m_ctrlStopSeconds.Text);
					else
						m_dStopSeconds = 0.0;
				}
				catch
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_STOP_SECONDS), m_ctrlStopSeconds);
					break;
				}
				
				m_dStartPosition = (((m_dStartHours * 3600.0) + (m_dStartMinutes * 60)) + m_dStartSeconds);
				m_dStopPosition = (((m_dStopHours * 3600.0) + (m_dStopMinutes * 60)) + m_dStopSeconds);
				
				//	Are the positions within range?
				if(m_dStartPosition < 0)
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_START_OUT_OF_RANGE, CTmaxToolbox.SecondsToString(m_dStartPosition)), GetFirstStartControl());
					break;
				}
				if((m_dxExtent.Stop > 0) && (m_dxExtent.Stop < m_dStartPosition))
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_START_OUT_OF_RANGE, CTmaxToolbox.SecondsToString(m_dStartPosition)), GetFirstStartControl());
					break;
				}
				if(m_dStopPosition <= 0)
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_STOP_OUT_OF_RANGE, CTmaxToolbox.SecondsToString(m_dStopPosition)), GetFirstStopControl());
					break;
				}
				if((m_dxExtent.Stop > 0) && (m_dxExtent.Stop < m_dStopPosition))
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_STOP_OUT_OF_RANGE, CTmaxToolbox.SecondsToString(m_dStopPosition)), GetFirstStopControl());
					break;
				}
				
				//	Are the positions reversed?
				if(m_dStopPosition <= m_dStartPosition)
				{
					Warn(m_tmaxErrorBuilder.Message(ERROR_REVERSED_POSITIONS, CTmaxToolbox.SecondsToString(m_dStartPosition), CTmaxToolbox.SecondsToString(m_dStopPosition)), GetFirstStartControl());
					break;
				}
				
				bOk = true;
			}
			
			return bOk;
		
		}// protected bool CheckRange()
		
		#endregion Protected Methods
		
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
		
		/// <summary>The active scene record</summary>
		public CDxSecondary Scene
		{
			get { return m_dxScene; }
			set { m_dxScene = value; }
		}
		
		/// <summary>The active recording</summary>
		public CDxPrimary Recording
		{
			get { return m_dxRecording; }
			set { m_dxRecording = value; }
		}
		
		/// <summary>The active segment record</summary>
		public CDxSecondary Segment
		{
			get { return m_dxSegment; }
			set { m_dxSegment = value; }
		}
		
		/// <summary>The movie clip record being edited if operating in Edit mode</summary>
		public CDxTertiary Clip
		{
			get { return m_dxClip; }
			set { m_dxClip = value; }
		}
		
		/// <summary>The XML designation being edited if operating in Edit mode</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return m_xmlDesignation; }
			set { m_xmlDesignation = value; }
		}
		
		/// <summary>True if the user changes an extent in Edit mode</summary>
		public bool Modified
		{
			get { return m_bModified; }
		}
		
		/// <summary>True to insert designations before the specified Scene</summary>
		public bool InsertBefore
		{
			get { return m_bInsertBefore; }
			set { m_bInsertBefore = value; }
		}
		
		/// <summary>True to edit the extents of Designation instead of add new designations</summary>
		public bool Edit
		{
			get { return m_bEdit; }
			set { m_bEdit = value; }
		}
		
		#endregion Properties
		
	}// public class CFAddClips : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Panes
