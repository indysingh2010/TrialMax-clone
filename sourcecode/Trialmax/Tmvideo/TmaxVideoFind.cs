using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This form is used to specify the criteria used to search the database</summary>
	public class CFTmaxVideoFind : CFTmaxVideoForm
	{
		#region Constants

		private const int ERROR_EXCHANGE_EX					= (ERROR_TMAX_VIDEO_FORM_MAX + 1);
		private const int ERROR_SPLIT_SEARCH_STRING_EX		= (ERROR_TMAX_VIDEO_FORM_MAX + 2);
		private const int ERROR_SET_INITIAL_SOURCE_EX		= (ERROR_TMAX_VIDEO_FORM_MAX + 3);
		private const int ERROR_FILL_DESIGNATIONS_EX		= (ERROR_TMAX_VIDEO_FORM_MAX + 4);
		private const int ERROR_SEARCH_DESIGNATIONS_EX		= (ERROR_TMAX_VIDEO_FORM_MAX + 5);
		private const int ERROR_SEARCH_TRANSCRIPTS_EX		= (ERROR_TMAX_VIDEO_FORM_MAX + 6);
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Group box for controls to define the search range</summary>
		private System.Windows.Forms.GroupBox m_ctrlRangeGroup;
		
		/// <summary>Static text control to display the current selections</summary>
		private System.Windows.Forms.RadioButton m_ctrlSearchSelections;

		/// <summary>Radio button to request searching of current selections</summary>
		private System.Windows.Forms.Label m_ctrlSelections;
		
		/// <summary>Radio button to request searching of previous search items</summary>
		private System.Windows.Forms.RadioButton m_ctrlSearchPrevious;

		/// <summary>Group box for search criteria group</summary>
		private System.Windows.Forms.GroupBox m_ctrlCriteriaGroup;
		
		/// <summary>Local member bound to Selections property</summary>
		private CTmaxItems m_tmaxSelections = null;
		
		/// <summary>Local member bound to XmlScript property</summary>
		private FTI.Shared.Xml.CXmlScript m_xmlScript = null;
		
		/// <summary>Local member to store selected designations</summary>
		private FTI.Shared.Xml.CXmlDesignations m_xmlSelections = new CXmlDesignations();
		
		/// <summary>Local member to store previously selected designations</summary>
		private FTI.Shared.Xml.CXmlDesignations m_xmlPrevious = new CXmlDesignations();
		
		/// <summary>Static text control to display the previous selections</summary>
		private System.Windows.Forms.Label m_ctrlPrevious;
		
		/// <summary>Combobox used to display the search string</summary>
		private System.Windows.Forms.ComboBox m_ctrlSearchStrings;
		
		/// <summary>Static text label for search strings combobox</summary>
		private System.Windows.Forms.Label m_ctrlSearchStringLabel;
		
		/// <summary>Local member bound to Options property</summary>
		private CTmaxSearchOptions m_tmaxOptions = null;
		
		/// <summary>Local member bound to Results property</summary>
		private CTmaxVideoResults m_tmaxResults = null;
		
		/// <summary>Search string specified by the user</summary>
		private string m_strSearchString = "";
		
		/// <summary>Check box to enable case sensitive searching</summary>
		private System.Windows.Forms.CheckBox m_ctrlCaseSensitive;
		
		/// <summary>Radio button to request all words</summary>
		private System.Windows.Forms.RadioButton m_ctrlFindAnyWord;
		
		/// <summary>Radio button to request any word</summary>
		private System.Windows.Forms.RadioButton m_ctrlFindAllWords;
		
		/// <summary>Array of words to be searched for</summary>
		private Array m_aSearchWords = null;
		private System.Windows.Forms.RadioButton m_ctrlSearchScript;
		private System.Windows.Forms.RadioButton m_ctrlSearchTranscript;
		private System.Windows.Forms.Label m_ctrlScript;
		private System.Windows.Forms.Label m_ctrlTranscript;
		
		/// <summary>Check box to enable whole word matching</summary>
		private System.Windows.Forms.CheckBox m_ctrlWholeWord;
		
		#endregion Private Members
		
		#region Public Members

		/// <summary>Constructor</summary>
		public CFTmaxVideoFind()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Search Form";
				
		}// public CFTmaxVideoFind()

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
		
		/// <summary>Overridden base class member called when the form gets displayed</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;
			
			//	Initialize the child controls
			while(bSuccessful == false)
			{
				//	Set the initial search range selection
				if(SetInitialSource() == false)
					break;
					
				//	Set the initial options
				if(Exchange(true) == false)
					break;
					
				//	All done
				bSuccessful = true;
			
			}
							
			m_ctrlOK.Enabled = bSuccessful;
			
			// Do the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>Overridden base class member called when the form size changes</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
		
			try
			{
				if((m_ctrlSelections != null) && (m_ctrlSelections.IsDisposed == false) && 
				   (m_ctrlSelections.Text.Length > 0))
				{
					m_ctrlSelections.Text = CTmaxToolbox.FitPathToWidth(ToString(m_xmlSelections), m_ctrlSelections);
				}
				
				if((m_ctrlPrevious != null) && (m_ctrlPrevious.IsDisposed == false) && 
				   (m_ctrlPrevious.Text.Length > 0))
				{
					m_ctrlPrevious.Text = CTmaxToolbox.FitPathToWidth(ToString(m_xmlPrevious), m_ctrlPrevious);
				}
			
			}
			catch
			{
			}
			
		}// protected override void OnSizeChanged(EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exchanging the search options");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while splitting the search string: string = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the form's initial source selection.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to populate the collection of XML designations.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to search the specified designations.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to search the specified transcript collection.");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Initializes the source selection using values provided by the owner</summary>
		/// <param name="eView">The view initiating the request</param>
		/// <returns>True if successful</returns>
		private bool SetInitialSource()
		{
			bool bSuccessful = false;
			
			//	Must have an active script
			if(m_xmlScript == null) return false;
			
			try
			{
				//	Set the script and transcript names
				m_ctrlScript.Text = m_xmlScript.Name;
				m_ctrlTranscript.Text = m_xmlScript.XmlDeposition.Name;
				
				//	Get the current and previous selections
				FillDesignations(m_xmlSelections, m_tmaxSelections);
				FillDesignations(m_xmlPrevious, m_tmaxOptions.PreviousItems);
					
				//	Enable/disable the selections options
				if(m_xmlSelections.Count > 0)
					m_ctrlSelections.Text = CTmaxToolbox.FitPathToWidth(ToString(m_xmlSelections), m_ctrlSelections);
				else
					m_ctrlSearchSelections.Enabled = false;
				
				if(m_xmlPrevious.Count > 0)
					m_ctrlPrevious.Text = CTmaxToolbox.FitPathToWidth(ToString(m_xmlPrevious), m_ctrlPrevious);
				else
					m_ctrlSearchPrevious.Enabled = false;
					
				if(m_xmlScript.XmlDesignations.Count == 0)
					m_ctrlSearchScript.Enabled = false;
					
				//	Which view has initiated the request
				switch(this.View)
				{
					case TmaxVideoViews.Transcript:
						
						m_ctrlSearchTranscript.Checked = true;
						break;
						
					case TmaxVideoViews.Tree:
						
						if(m_xmlSelections.Count > 0)
							m_ctrlSearchSelections.Checked = true;
						else if(m_xmlScript.XmlDesignations.Count > 0)
							m_ctrlSearchScript.Checked = true;
						else
							m_ctrlSearchTranscript.Checked = true;
						break;
						
					default:
					
						if(m_xmlScript.XmlDesignations.Count > 0)
							m_ctrlSearchScript.Checked = true;
						else
							m_ctrlSearchTranscript.Checked = true;
						break;
						
				}// switch(this.View)
				
				bSuccessful = true;			
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetInitialSource", m_tmaxErrorBuilder.Message(ERROR_SET_INITIAL_SOURCE_EX), Ex);
			}
			
			return bSuccessful;
		
		}// private bool SetInitialSource()

		/// <summary>Builds the string representation of the items in the collection</summary>
		/// <returns>A string that identifies the items</returns>
		private string ToString(CXmlDesignations xmlDesignations)
		{
			string strString = "";
			string strDesignation = "";
			
			foreach(CXmlDesignation O in xmlDesignations)
			{
				strDesignation = String.Format("{0}-{1}", CTmaxToolbox.PLToString(O.FirstPL), CTmaxToolbox.PLToString(O.LastPL));
				
				if(strDesignation.Length > 0)
				{
					if(strString.Length > 0)
						strString += ", ";
						
					strString += strDesignation;
				}
			}
			
			return strString;
			
		}// private string ToString(CXmlDesignations xmlDesignations)

		/// <summary>Populates the collection of designations using the specified event items</summary>
		/// <param name="xmlDesignations">The collection in which to store the desired designations</param>
		/// <param name="tmaxItems">The items that identify the desired designations</param>
		/// <returns>True if successful</returns>
		private bool FillDesignations(CXmlDesignations xmlDesignations, CTmaxItems tmaxItems)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Clear the existing designations
				xmlDesignations.Clear();
				
				if(tmaxItems != null)
				{
					foreach(CTmaxItem O in tmaxItems)
					{
						//	Is this a designation?
						if(O.XmlDesignation != null)
						{
							//	Is this valid for the active script?
							if(m_xmlScript.XmlDesignations.Contains(O.XmlDesignation) == true)
								xmlDesignations.Add(O.XmlDesignation);
								
						}// if(O.XmlDesignation != null)
						
					}// foreach(CTmaxItem O in tmaxItems)
				
				}// if(tmaxItems != null)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillDesignations", m_tmaxErrorBuilder.Message(ERROR_FILL_DESIGNATIONS_EX), Ex);
			}
			
			return bSuccessful;
		
		}// private bool FillDesignations(CXmlDesignations xmlDesignations, CTmaxItems tmaxItems)

		/// <summary>This method is called to split the search string into individual words</summary>
		/// <param name="strSearchString">The string to be split</param>
		/// <returns>true if successful</returns>
		private bool SplitSearchString(string strSearchString)
		{
			char []	aDelimiters = new char[]{ ' ' };
			
			//	Clear the existing words
			m_aSearchWords = null;
			
			try
			{
				m_aSearchWords = strSearchString.Split(aDelimiters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SplitSearchString", m_tmaxErrorBuilder.Message(ERROR_SPLIT_SEARCH_STRING_EX, strSearchString), Ex);
			}
			
			return ((m_aSearchWords != null) && (m_aSearchWords.Length > 0));
			
		}// private bool SplitSearchString(string strSearchString)
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFTmaxVideoFind));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlRangeGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlTranscript = new System.Windows.Forms.Label();
			this.m_ctrlScript = new System.Windows.Forms.Label();
			this.m_ctrlSearchTranscript = new System.Windows.Forms.RadioButton();
			this.m_ctrlSearchScript = new System.Windows.Forms.RadioButton();
			this.m_ctrlPrevious = new System.Windows.Forms.Label();
			this.m_ctrlSelections = new System.Windows.Forms.Label();
			this.m_ctrlSearchPrevious = new System.Windows.Forms.RadioButton();
			this.m_ctrlSearchSelections = new System.Windows.Forms.RadioButton();
			this.m_ctrlCriteriaGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlWholeWord = new System.Windows.Forms.CheckBox();
			this.m_ctrlFindAnyWord = new System.Windows.Forms.RadioButton();
			this.m_ctrlFindAllWords = new System.Windows.Forms.RadioButton();
			this.m_ctrlCaseSensitive = new System.Windows.Forms.CheckBox();
			this.m_ctrlSearchStringLabel = new System.Windows.Forms.Label();
			this.m_ctrlSearchStrings = new System.Windows.Forms.ComboBox();
			this.m_ctrlRangeGroup.SuspendLayout();
			this.m_ctrlCriteriaGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(264, 288);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 1;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.Location = new System.Drawing.Point(184, 288);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 0;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlRangeGroup
			// 
			this.m_ctrlRangeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlTranscript);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlScript);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSearchTranscript);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSearchScript);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlPrevious);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSelections);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSearchPrevious);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSearchSelections);
			this.m_ctrlRangeGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlRangeGroup.Name = "m_ctrlRangeGroup";
			this.m_ctrlRangeGroup.Size = new System.Drawing.Size(336, 120);
			this.m_ctrlRangeGroup.TabIndex = 0;
			this.m_ctrlRangeGroup.TabStop = false;
			this.m_ctrlRangeGroup.Text = "Search Range";
			// 
			// m_ctrlTranscript
			// 
			this.m_ctrlTranscript.Location = new System.Drawing.Point(104, 96);
			this.m_ctrlTranscript.Name = "m_ctrlTranscript";
			this.m_ctrlTranscript.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlTranscript.TabIndex = 7;
			// 
			// m_ctrlScript
			// 
			this.m_ctrlScript.Location = new System.Drawing.Point(104, 72);
			this.m_ctrlScript.Name = "m_ctrlScript";
			this.m_ctrlScript.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlScript.TabIndex = 6;
			// 
			// m_ctrlSearchTranscript
			// 
			this.m_ctrlSearchTranscript.Location = new System.Drawing.Point(8, 96);
			this.m_ctrlSearchTranscript.Name = "m_ctrlSearchTranscript";
			this.m_ctrlSearchTranscript.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSearchTranscript.TabIndex = 3;
			this.m_ctrlSearchTranscript.Text = "Transcript:";
			// 
			// m_ctrlSearchScript
			// 
			this.m_ctrlSearchScript.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlSearchScript.Name = "m_ctrlSearchScript";
			this.m_ctrlSearchScript.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSearchScript.TabIndex = 2;
			this.m_ctrlSearchScript.Text = "Script:";
			// 
			// m_ctrlPrevious
			// 
			this.m_ctrlPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPrevious.Location = new System.Drawing.Point(104, 48);
			this.m_ctrlPrevious.Name = "m_ctrlPrevious";
			this.m_ctrlPrevious.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlPrevious.TabIndex = 5;
			this.m_ctrlPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSelections
			// 
			this.m_ctrlSelections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSelections.Location = new System.Drawing.Point(104, 24);
			this.m_ctrlSelections.Name = "m_ctrlSelections";
			this.m_ctrlSelections.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlSelections.TabIndex = 4;
			this.m_ctrlSelections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSearchPrevious
			// 
			this.m_ctrlSearchPrevious.Location = new System.Drawing.Point(8, 48);
			this.m_ctrlSearchPrevious.Name = "m_ctrlSearchPrevious";
			this.m_ctrlSearchPrevious.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSearchPrevious.TabIndex = 1;
			this.m_ctrlSearchPrevious.Text = "Previous :";
			this.m_ctrlSearchPrevious.Click += new System.EventHandler(this.OnClickSearchRange);
			// 
			// m_ctrlSearchSelections
			// 
			this.m_ctrlSearchSelections.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlSearchSelections.Name = "m_ctrlSearchSelections";
			this.m_ctrlSearchSelections.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSearchSelections.TabIndex = 0;
			this.m_ctrlSearchSelections.Text = "Selections :";
			this.m_ctrlSearchSelections.Click += new System.EventHandler(this.OnClickSearchRange);
			// 
			// m_ctrlCriteriaGroup
			// 
			this.m_ctrlCriteriaGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCriteriaGroup.Controls.Add(this.m_ctrlWholeWord);
			this.m_ctrlCriteriaGroup.Controls.Add(this.m_ctrlFindAnyWord);
			this.m_ctrlCriteriaGroup.Controls.Add(this.m_ctrlFindAllWords);
			this.m_ctrlCriteriaGroup.Controls.Add(this.m_ctrlCaseSensitive);
			this.m_ctrlCriteriaGroup.Controls.Add(this.m_ctrlSearchStringLabel);
			this.m_ctrlCriteriaGroup.Controls.Add(this.m_ctrlSearchStrings);
			this.m_ctrlCriteriaGroup.Location = new System.Drawing.Point(8, 136);
			this.m_ctrlCriteriaGroup.Name = "m_ctrlCriteriaGroup";
			this.m_ctrlCriteriaGroup.Size = new System.Drawing.Size(336, 144);
			this.m_ctrlCriteriaGroup.TabIndex = 1;
			this.m_ctrlCriteriaGroup.TabStop = false;
			this.m_ctrlCriteriaGroup.Text = "Search Criteria";
			// 
			// m_ctrlWholeWord
			// 
			this.m_ctrlWholeWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlWholeWord.Location = new System.Drawing.Point(8, 120);
			this.m_ctrlWholeWord.Name = "m_ctrlWholeWord";
			this.m_ctrlWholeWord.Size = new System.Drawing.Size(160, 16);
			this.m_ctrlWholeWord.TabIndex = 4;
			this.m_ctrlWholeWord.Text = "Match Whole Word Only";
			// 
			// m_ctrlFindAnyWord
			// 
			this.m_ctrlFindAnyWord.Checked = true;
			this.m_ctrlFindAnyWord.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlFindAnyWord.Name = "m_ctrlFindAnyWord";
			this.m_ctrlFindAnyWord.Size = new System.Drawing.Size(112, 16);
			this.m_ctrlFindAnyWord.TabIndex = 1;
			this.m_ctrlFindAnyWord.TabStop = true;
			this.m_ctrlFindAnyWord.Text = "Find Any Word";
			// 
			// m_ctrlFindAllWords
			// 
			this.m_ctrlFindAllWords.Location = new System.Drawing.Point(124, 72);
			this.m_ctrlFindAllWords.Name = "m_ctrlFindAllWords";
			this.m_ctrlFindAllWords.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlFindAllWords.TabIndex = 2;
			this.m_ctrlFindAllWords.Text = "Find All Words";
			// 
			// m_ctrlCaseSensitive
			// 
			this.m_ctrlCaseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlCaseSensitive.Location = new System.Drawing.Point(8, 96);
			this.m_ctrlCaseSensitive.Name = "m_ctrlCaseSensitive";
			this.m_ctrlCaseSensitive.Size = new System.Drawing.Size(140, 16);
			this.m_ctrlCaseSensitive.TabIndex = 3;
			this.m_ctrlCaseSensitive.Text = "Match Case";
			// 
			// m_ctrlSearchStringLabel
			// 
			this.m_ctrlSearchStringLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSearchStringLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlSearchStringLabel.Name = "m_ctrlSearchStringLabel";
			this.m_ctrlSearchStringLabel.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlSearchStringLabel.TabIndex = 11;
			this.m_ctrlSearchStringLabel.Text = "Search Text:";
			this.m_ctrlSearchStringLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSearchStrings
			// 
			this.m_ctrlSearchStrings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSearchStrings.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlSearchStrings.MaxDropDownItems = 10;
			this.m_ctrlSearchStrings.Name = "m_ctrlSearchStrings";
			this.m_ctrlSearchStrings.Size = new System.Drawing.Size(320, 21);
			this.m_ctrlSearchStrings.TabIndex = 0;
			// 
			// CFTmaxVideoFind
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 317);
			this.Controls.Add(this.m_ctrlCriteriaGroup);
			this.Controls.Add(this.m_ctrlRangeGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFTmaxVideoFind";
			this.Text = " Find Text";
			this.m_ctrlRangeGroup.ResumeLayout(false);
			this.m_ctrlCriteriaGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>Handles the Click event fired by one of the Search Range radio buttons</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">System event arguments</param>
		private void OnClickSearchRange(object sender, System.EventArgs e)
		{
		}
		
		/// <summary>Handles the Click event fired by the OK button</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">System event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxOptions != null);
			if(m_tmaxOptions == null) return;
			
			Cursor.Current = Cursors.WaitCursor;
			
			//	Update the options using the current control settings
			if(Exchange(false) == false) return;
			
			if(m_ctrlSearchSelections.Checked == true)
			{
				if((bSuccessful = Search(m_xmlSelections)) == true)
				{
					//	Update the collection of previous items
					m_tmaxOptions.PreviousItems = m_tmaxSelections;
				}
				
			}
			else if(m_ctrlSearchPrevious.Checked == true)
			{
				bSuccessful = Search(m_xmlPrevious);
			}
			else if(m_ctrlSearchTranscript.Checked == true)
			{
				bSuccessful = Search(m_xmlScript.XmlDeposition.Transcripts, null);
			}
			else
			{
				bSuccessful = Search(m_xmlScript.XmlDesignations);
			}
				
			Cursor.Current = Cursors.Default;
			
			if(bSuccessful == true)
			{
				//	Were there any hits?
				if(m_tmaxResults.Count > 0)
				{
					DialogResult = DialogResult.OK;
					this.Close();
				}
				else
				{
					MessageBox.Show("The specified text was not found.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				} 
			
			}// if(bSuccessful == true)

		}// private void OnClickOK(object sender, System.EventArgs e)

		/// <summary>This method exchanges values between the search options and from controls</summary>
		/// <param name="bSetControls">true if options are to be used to set control values</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetControls)
		{
			Debug.Assert(m_tmaxOptions != null);
			if(m_tmaxOptions == null) return false;
			
			try
			{
				//	Are we setting the control states?
				if(bSetControls == true)
				{
					m_ctrlCaseSensitive.Checked = m_tmaxOptions.CaseSensitive;
					m_ctrlWholeWord.Checked = m_tmaxOptions.WholeWords;
				
					m_ctrlFindAllWords.Checked = (m_tmaxOptions.FindAllWords == true);
					m_ctrlFindAnyWord.Checked  = (m_tmaxOptions.FindAllWords == false);

					//	Fill the search strings list
					foreach(string O in m_tmaxOptions.SearchStrings)
					{
						m_ctrlSearchStrings.Items.Add(O);
					}
					
					//	Select the first search string
					if(m_ctrlSearchStrings.Items.Count > 0)
						m_ctrlSearchStrings.SelectedIndex = 0;
				}
				else
				{
					//m_tmaxEventSource.InitElapsed();
					
					m_tmaxOptions.FindAllWords = m_ctrlFindAllWords.Checked;
					m_tmaxOptions.CaseSensitive = m_ctrlCaseSensitive.Checked;
					m_tmaxOptions.WholeWords = m_ctrlWholeWord.Checked;
					
					//	Get the string used for the search
					m_strSearchString = m_ctrlSearchStrings.Text;
					if(m_strSearchString.Length == 0)
					{
						MessageBox.Show("You must supply a valid search string", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						m_ctrlSearchStrings.Focus();
						return false;
					}
					
					//	Parse the search string
					if(SplitSearchString(m_strSearchString) == true)
					{
						//	Store this search string
						m_tmaxOptions.AddSearchString(m_strSearchString);
						
						//	Refresh the combobox
						//
						//	WE ONLY NEED THIS IF WE GO NON-MODAL
						//	m_ctrlSearchStrings.Items.Clear();
						//	foreach(string O in m_tmaxOptions.SearchStrings)
						//		m_ctrlSearchStrings.Items.Add(O);
						//	if(m_ctrlSearchStrings.Items.Count > 0)
						//		m_ctrlSearchStrings.SelectedIndex = 0;
							
					}
					else
					{
						return false;
					}
				
					//m_tmaxEventSource.FireElapsed(this, "ExchangeOptions", "Time to exchange options: ");
					
				}// if(bSetControls == true)
				
				return true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX), Ex);
				return false;
			}
			
		}// private void Exchange(bool bSetControls)
		
		/// <summary>This method is called to search the designations in the specified collection</summary>
		/// <returns>True if successful</returns>
		private bool Search(CXmlDesignations xmlDesignations)
		{
			bool bSuccessful = false;
			
			try
			{
				foreach(CXmlDesignation O in xmlDesignations)
				{
					//	Search the transcript text for each designation
					if((O.Transcripts != null) && (O.Transcripts.Count > 0))
						Search(O.Transcripts, O);
				}
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Search", m_tmaxErrorBuilder.Message(ERROR_SEARCH_DESIGNATIONS_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Search(CXmlDesignations xmlDesignations)
		
		/// <summary>This method executes the search against the specified deposition</summary>
		/// <param name="xmlDeposition">the deposition associated with the search</param>
		/// <param name="xmlDesignation">The owner designation (if there is one)</param>
		/// <returns>true if successful</returns>
		private bool Search(CXmlTranscripts xmlTranscripts, CXmlDesignation xmlDesignation)
		{
			CTmaxVideoResult	tmaxResult = null;
			bool				bSuccessful = false;
			bool				bMatch = false;
			
			try
			{
				foreach(CXmlTranscript O in xmlTranscripts)
				{
					//	Perform whole word matching if required
					if(m_tmaxOptions.WholeWords == true)
						bMatch = MatchWholeWords(O.GetFormattedText());
					else
						bMatch = MatchPartialWords(O.GetFormattedText());
							
					//	Is this line a match?
					if(bMatch == true)
					{
						//	Add a result for this line
						tmaxResult = new CTmaxVideoResult();
							
						tmaxResult.XmlScript = XmlScript;
						tmaxResult.XmlDesignation = xmlDesignation;
						tmaxResult.PL   = O.PL;
						tmaxResult.Page = O.Page;
						tmaxResult.Line = O.Line;
						tmaxResult.Text = O.GetFormattedText();

						m_tmaxResults.Add(tmaxResult);
					}
						
				}// foreach(CXmlTranscript O in xmlTranscripts)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Search", m_tmaxErrorBuilder.Message(ERROR_SEARCH_TRANSCRIPTS_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Search(CXmlTranscripts xmlTranscripts)

/*		
		/// <summary>This method executes the search against the specified script scene</summary>
		/// <param name="dxScene">the scene to be searched</param>
		/// <returns>true if successful</returns>
		private bool Search(CDxSecondary dxScene)
		{
			CDxTertiary dxDesignation = null;
			
			if((dxDesignation = (CDxTertiary)(dxScene.GetSource())) != null)
			{
				if(dxDesignation.Secondary != null)
					return Search(dxDesignation.Secondary.Primary, dxScene);
			}
			
			//	Must have been a problem
			return false;
				
		}// private bool Search(CDxSecondary dxScene)
*/
/*		
		/// <summary>This method will retrieve the fully qualifed path to the XML deposition</summary>
		/// <param name="dxDeposition">The deposition that owns the file</param>
		/// <param name="rCancel">flag to be set if the user cancels</param>
		/// <returns>The fully qualified path to the deposition file</returns>
		private string GetXmlFileSpec(CDxPrimary dxDeposition, ref bool bCancel)
		{
			CDxTranscript	dxTranscript = null;
			string			strFileSpec = "";
			
			//	Get the transcript for this deposition
			if((dxTranscript = dxDeposition.GetTranscript()) == null)
			{
				if(MessageBox.Show("Unable to retrieve the transcript record for " + dxDeposition.Name, "Error",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					bCancel = false;
				}
				else
				{
					bCancel = true;
				}
				return null;
			}
				
			//	Get the file path from the database
			strFileSpec = m_tmaxDatabase.GetFileSpec(dxTranscript);
			
			if((strFileSpec == null) || (strFileSpec.Length == 0))
			{
				if(MessageBox.Show("Unable to retrieve the transcript path for " + dxDeposition.Name, "Error",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					bCancel = false;
				}
				else
				{
					bCancel = true;
				}
				return null;
			}
			
			//	Does the file exist?
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				if(MessageBox.Show("Unable to locate the transcript file for " + dxDeposition.Name + "\n\n" + strFileSpec, "Error",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					bCancel = false;
				}
				else
				{
					bCancel = true;
				}
				return null;
			}
			
			bCancel = false;
			return strFileSpec;
			
		}// private string GetXmlFileSpec(CDxPrimary dxDeposition, ref bool rCancel)
*/
/*
		/// <summary>This method will retrieve the fully qualifed path to the XML designation associated with the specified scene</summary>
		/// <param name="dxScene">The scene that is sourced by the desired designation</param>
		/// <param name="rCancel">flag to be set if the user cancels</param>
		/// <returns>The fully qualified path to the designation file</returns>
		private string GetXmlFileSpec(CDxSecondary dxScene, ref bool bCancel)
		{
			CDxTertiary		dxDesignation = null;
			string			strFileSpec = "";
			
			// Just in case...
			Debug.Assert(dxScene.SourceType == TmaxMediaTypes.Designation);
			if(dxScene.SourceType != TmaxMediaTypes.Designation)
			{
				bCancel = false;
				return null;
			}
			
			//	Get the designation for this scene
			if((dxDesignation = ((CDxTertiary)(dxScene.GetSource()))) == null)
			{
				if(MessageBox.Show("Unable to retrieve the source designation for " + dxScene.GetBarcode(false), "Error",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					bCancel = false;
				}
				else
				{
					bCancel = true;
				}
				return null;
			}
				
			//	Get the file path from the database
			strFileSpec = m_tmaxDatabase.GetFileSpec(dxDesignation);
			if((strFileSpec == null) || (strFileSpec.Length == 0))
			{
				if(MessageBox.Show("Unable to retrieve the designation file path for " + dxScene.GetBarcode(false), "Error",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					bCancel = false;
				}
				else
				{
					bCancel = true;
				}
				return null;
			}
			
			//	Does the file exist?
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				if(MessageBox.Show("Unable to locate the XML designation file for " + dxScene.GetBarcode(false) + "\n\n" + strFileSpec, "Error",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
				{
					bCancel = false;
				}
				else
				{
					bCancel = true;
				}
				return null;
			}
			
			bCancel = false;
			return strFileSpec;
			
		}// private string GetXmlFileSpec(CDxPrimary dxDeposition, ref bool rCancel)
*/		
		/// <summary>This method is called to determine if the specified text contains the search words as whole words</summary>
		/// <param name="strText">The line of text to be searched</param>
		/// <returns>true if successful</returns>
		private bool MatchWholeWords(string strText)
		{
			Array	aWords = strText.Split(' ');
			bool	bWordFound = false;
			
			//	Iterate the collection of search words
			foreach(string S in m_aSearchWords)
			{
				bWordFound = false;
				
				//	Attempt to locate this search word among the specified text
				foreach(string word in aWords)
				{
					if(String.Compare(word, S, !m_tmaxOptions.CaseSensitive) == 0)
					{
						bWordFound = true;
					}
				
				}// foreach(string word in aWords)
				
				//	Did we locate this search word?
				if(bWordFound == true)
				{
					//	Stop here if we only need one of the words
					if(m_tmaxOptions.FindAllWords == false)
						return true;
				}
				else
				{
					//	Stop here if we need all the words
					if(m_tmaxOptions.FindAllWords == true)
						return false;
				}
				
			}// foreach(string S in m_aSearchWords)
			
			//	If we're looking for all words and we made it this 
			//	far then we succeeded. If only looking for one of the
			//	search words then we failed
			return (m_tmaxOptions.FindAllWords == true);
			
		}// private bool MatchWholeWords(string strText)
		
		/// <summary>This method is called to determine if the specified text contains the search words as whole words</summary>
		/// <param name="strText">The line of text to be searched</param>
		/// <returns>true if successful</returns>
		private bool MatchPartialWords(string strText)
		{
			int	iIndex = 0;
			
			//	Iterate the collection of search words
			foreach(string S in m_aSearchWords)
			{
				//	Attempt to locate this search word among the specified text
				if(m_tmaxOptions.CaseSensitive == true)
					iIndex = strText.IndexOf(S);
				else
					iIndex = (strText.ToLower()).IndexOf(S.ToLower());
				
				//	Did we locate this search word?
				if(iIndex >= 0)
				{
					//	Stop here if we only need one of the words
					if(m_tmaxOptions.FindAllWords == false)
						return true;
				}
				else
				{
					//	Stop here if we need all the words
					if(m_tmaxOptions.FindAllWords == true)
						return false;
				}
				
			}// foreach(string S in m_aSearchWords)
			
			//	If we're looking for all words and we made it this 
			//	far then we succeeded. If only looking for one of the
			//	search words then we failed
			return (m_tmaxOptions.FindAllWords == true);
			
		}// private bool MatchPartialWords(string strText)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The application's active script</summary>
		public FTI.Shared.Xml.CXmlScript XmlScript
		{
			get { return m_xmlScript; }
			set { m_xmlScript = value; }
		}
		
		/// <summary>Event items that represent the current selections</summary>
		public CTmaxItems Selections
		{
			get { return m_tmaxSelections; }
			set { m_tmaxSelections = value; }
		}
		
		/// <summary>Application text searching options</summary>
		public CTmaxSearchOptions SearchOptions
		{
			get { return m_tmaxOptions; }
			set { m_tmaxOptions = value; }
		}
		
		/// <summary>The results of the search operation</summary>
		public CTmaxVideoResults Results
		{
			get { return m_tmaxResults; }
			set { m_tmaxResults = value; }
		}
		
		#endregion Properties
		
	}// public class CFTmaxVideoFind : FTI.Trialmax.Forms.CFTmaxBaseForm
	
}// namespace FTI.Trialmax.TMVV.Tmvideo
