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
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Panes
{
	/// <summary>This form is used to specify the criteria used to search the database</summary>
	public class CFSearch : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants

		protected const int ERROR_EXCHANGE_OPTIONS_EX		= (ERROR_TMAX_FORM_MAX + 1);
		protected const int ERROR_QUEUE_SELECTIONS_EX		= (ERROR_TMAX_FORM_MAX + 2);
		protected const int ERROR_QUEUE_PREVIOUS_EX			= (ERROR_TMAX_FORM_MAX + 3);
		protected const int ERROR_QUEUE_ALL_EX				= (ERROR_TMAX_FORM_MAX + 4);
		protected const int ERROR_SPLIT_SEARCH_STRING_EX	= (ERROR_TMAX_FORM_MAX + 5);
		protected const int ERROR_SEARCH_SCENE_EX			= (ERROR_TMAX_FORM_MAX + 6);
		protected const int ERROR_SEARCH_DEPOSITION_EX		= (ERROR_TMAX_FORM_MAX + 7);

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

		/// <summary>Check box to request inclusion of sub binders in search range</summary>
		private System.Windows.Forms.CheckBox m_ctrlIncludeSubBinders;

		/// <summary>Radio button to request searching of all records</summary>
		private System.Windows.Forms.RadioButton m_ctrlSearchAll;

		/// <summary>List box to allow user to select records of specific media types</summary>
		private System.Windows.Forms.CheckedListBox m_ctrlMediaTypes;
		
		/// <summary>Group box for search criteria group</summary>
		private System.Windows.Forms.GroupBox m_ctrlCriteriaGroup;
		
		/// <summary>Local member bound to Database property</summary>
		private FTI.Trialmax.Database.CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member bound to Selections property</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxSelections = null;
		
		/// <summary>Static text control to display the previous selections</summary>
		private System.Windows.Forms.Label m_ctrlPrevious;
		
		/// <summary>Combobox used to display the search string</summary>
		private System.Windows.Forms.ComboBox m_ctrlSearchStrings;
		
		/// <summary>Static text label for search strings combobox</summary>
		private System.Windows.Forms.Label m_ctrlSearchStringLabel;
		
		/// <summary>Local member bound to Options property</summary>
		private FTI.Shared.Trialmax.CTmaxSearchOptions m_tmaxOptions = null;
		
		/// <summary>Local member to store records to be searched</summary>
		private CDxMediaRecords m_dxQueue = new FTI.Trialmax.Database.CDxMediaRecords();	
		
		/// <summary>Local member bound to Results property</summary>
		private FTI.Shared.Trialmax.CTmaxSearchResults m_tmaxSearchResults = null;
		
		/// <summary>Search string specified by the user</summary>
		private string m_strSearchString = "";
		
		/// <summary>XPath query string used for the search operation</summary>
		private string m_strXPath = "";
		
		/// <summary>Check box to enable case sensitive searching</summary>
		private System.Windows.Forms.CheckBox m_ctrlCaseSensitive;
		
		/// <summary>Radio button to request all words</summary>
		private System.Windows.Forms.RadioButton m_ctrlFindAnyWord;
		
		/// <summary>Radio button to request any word</summary>
		private System.Windows.Forms.RadioButton m_ctrlFindAllWords;
		
		/// <summary>Array of words to be searched for</summary>
		private Array m_aSearchWords = null;
		
		/// <summary>Check box to enable whole word matching</summary>
		private System.Windows.Forms.CheckBox m_ctrlWholeWord;
		
		private System.Windows.Forms.CheckBox m_ctrlXPath;
		
		#endregion Private Members
		
		#region Public Members

		/// <summary>Constructor</summary>
		public CFSearch()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Search Form";
				
		}// public CFSearch()

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
			//	Do we have any active selections?
			if((m_tmaxSelections != null) && (m_tmaxSelections.Count > 0))
			{
				m_ctrlSelections.Text = CTmaxToolbox.FitPathToWidth(m_tmaxSelections.ToString(), m_ctrlSelections);
				m_ctrlSearchSelections.Checked = true;
			}
			else
			{
				m_ctrlSearchSelections.Checked = false;
				m_ctrlSearchSelections.Enabled = false;
			}
			
			//	Do we have any previous items?
			if((m_tmaxOptions.PreviousItems != null) && (m_tmaxOptions.PreviousItems.Count > 0))
			{
				m_ctrlPrevious.Text =  CTmaxToolbox.FitPathToWidth(m_tmaxOptions.PreviousItems.ToString(), m_ctrlPrevious);
				
				if(m_ctrlSearchSelections.Checked == false)
					m_ctrlSearchPrevious.Checked = true;
					
			}
			else
			{
				m_ctrlSearchPrevious.Enabled = false;
			}
			
			//	Fill the media types list box
			FillMediaTypes();
			
			//	Select the SearchAll option if both the SearchSelections and
			//	SearchPrevious options are disabled
			if((m_ctrlSearchSelections.Enabled == false) && (m_ctrlSearchPrevious.Enabled == false))
			{
				m_ctrlSearchAll.Checked = true;
				m_ctrlIncludeSubBinders.Enabled = false;
			}
			
			//	Set the initial options
			ExchangeOptions(true);
							
			m_ctrlMediaTypes.Enabled = m_ctrlSearchAll.Checked;

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
					m_ctrlSelections.Text = CTmaxToolbox.FitPathToWidth(m_tmaxSelections.ToString(), m_ctrlSelections);
				}
				
				if((m_ctrlPrevious != null) && (m_ctrlPrevious.IsDisposed == false) && 
				   (m_ctrlPrevious.Text.Length > 0))
				{
					m_ctrlPrevious.Text = CTmaxToolbox.FitPathToWidth(m_tmaxOptions.PreviousItems.ToString(), m_ctrlPrevious);
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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding the current selections to the search queue");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding the previous selections to the search queue");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding the media records to the search queue");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while splitting the search string: string = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while searching for text in the script scene: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while searching for text in the deposition: name = %1");
	
		}// protected override void SetErrorStrings()

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to split the search string into individual words</summary>
		/// <param name="strSearchString">The string to be split</param>
		/// <returns>true if successful</returns>
		private bool SplitSearchString(string strSearchString)
		{
			char []	aDelimiters = new char[]{ ' ' };
			bool	bFirstWord = true;
			
			//	Clear the existing words
			m_aSearchWords = null;
			
			try
			{
				m_aSearchWords = strSearchString.Split(aDelimiters);
				
				//	Build the XPath statement required for the search
				if(m_tmaxOptions.UseXPath == true)
				{
					m_strXPath = "//transcript[";
					foreach(string O in m_aSearchWords)
					{
						//	Insert the boolean operator
						if(bFirstWord == false)
						{
							if(m_tmaxOptions.FindAllWords == true)
								m_strXPath += " and ";
							else
								m_strXPath += " or ";
						}
						else
						{
							bFirstWord = false;
						}
						
						//	Now add the XPath to get this word
						m_strXPath += GetXPath(O);
					}
					m_strXPath += "]";
				
				}
				else
				{
					//	Just pull all transcripts if XPath mode not selected
					m_strXPath = "//transcript";
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SplitSearchString", m_tmaxErrorBuilder.Message(ERROR_SPLIT_SEARCH_STRING_EX, strSearchString), Ex);
			}
			
			return ((m_aSearchWords != null) && (m_aSearchWords.Length > 0));
			
		}// private bool SplitSearchString(string strSearchString)
		
		/// <summary>Called to get the portion of the XPath query required for the specified search word</summary>
		/// <param name="strWord">The word to be located</param>
		/// <returns>The XPath required to get the specified word</returns>
		private string GetXPath(string strWord)
		{
			string strXPath = "";
			
			//	Are we doing case insensitive searching?
			if(m_tmaxOptions.CaseSensitive == false)
			{
				strXPath = String.Format("contains(translate(@text, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), {0})", CXmlBase.GetXPathString(strWord.ToLower()));
			}
			else
			{
				strXPath = String.Format("contains(@text, {0})", CXmlBase.GetXPathString(strWord));
			}
			
			return strXPath;
			
		}// private string GetXPath(string strWord)
		
		/// <summary>This method will populate the Media Types list box</summary>
		private void FillMediaTypes()
		{
			m_ctrlMediaTypes.Items.Add("Depositions", true);
			m_ctrlMediaTypes.Items.Add("Scripts", true);
		
		}// private void FillMediaTypes()
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFSearch));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlRangeGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlMediaTypes = new System.Windows.Forms.CheckedListBox();
			this.m_ctrlPrevious = new System.Windows.Forms.Label();
			this.m_ctrlSearchAll = new System.Windows.Forms.RadioButton();
			this.m_ctrlSelections = new System.Windows.Forms.Label();
			this.m_ctrlSearchPrevious = new System.Windows.Forms.RadioButton();
			this.m_ctrlSearchSelections = new System.Windows.Forms.RadioButton();
			this.m_ctrlIncludeSubBinders = new System.Windows.Forms.CheckBox();
			this.m_ctrlCriteriaGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlWholeWord = new System.Windows.Forms.CheckBox();
			this.m_ctrlFindAnyWord = new System.Windows.Forms.RadioButton();
			this.m_ctrlFindAllWords = new System.Windows.Forms.RadioButton();
			this.m_ctrlCaseSensitive = new System.Windows.Forms.CheckBox();
			this.m_ctrlSearchStringLabel = new System.Windows.Forms.Label();
			this.m_ctrlSearchStrings = new System.Windows.Forms.ComboBox();
			this.m_ctrlXPath = new System.Windows.Forms.CheckBox();
			this.m_ctrlRangeGroup.SuspendLayout();
			this.m_ctrlCriteriaGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(264, 308);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 7;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.Location = new System.Drawing.Point(184, 308);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 6;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlRangeGroup
			// 
			this.m_ctrlRangeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlMediaTypes);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlPrevious);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSearchAll);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSelections);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSearchPrevious);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlSearchSelections);
			this.m_ctrlRangeGroup.Controls.Add(this.m_ctrlIncludeSubBinders);
			this.m_ctrlRangeGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlRangeGroup.Name = "m_ctrlRangeGroup";
			this.m_ctrlRangeGroup.Size = new System.Drawing.Size(336, 140);
			this.m_ctrlRangeGroup.TabIndex = 0;
			this.m_ctrlRangeGroup.TabStop = false;
			this.m_ctrlRangeGroup.Text = "Search Range";
			// 
			// m_ctrlMediaTypes
			// 
			this.m_ctrlMediaTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMediaTypes.CheckOnClick = true;
			this.m_ctrlMediaTypes.IntegralHeight = false;
			this.m_ctrlMediaTypes.Location = new System.Drawing.Point(104, 72);
			this.m_ctrlMediaTypes.Name = "m_ctrlMediaTypes";
			this.m_ctrlMediaTypes.Size = new System.Drawing.Size(224, 40);
			this.m_ctrlMediaTypes.TabIndex = 11;
			// 
			// m_ctrlPrevious
			// 
			this.m_ctrlPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPrevious.Location = new System.Drawing.Point(104, 48);
			this.m_ctrlPrevious.Name = "m_ctrlPrevious";
			this.m_ctrlPrevious.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlPrevious.TabIndex = 10;
			this.m_ctrlPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSearchAll
			// 
			this.m_ctrlSearchAll.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlSearchAll.Name = "m_ctrlSearchAll";
			this.m_ctrlSearchAll.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSearchAll.TabIndex = 9;
			this.m_ctrlSearchAll.Text = "All :";
			this.m_ctrlSearchAll.Click += new System.EventHandler(this.OnClickSearchRange);
			// 
			// m_ctrlSelections
			// 
			this.m_ctrlSelections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSelections.Location = new System.Drawing.Point(104, 24);
			this.m_ctrlSelections.Name = "m_ctrlSelections";
			this.m_ctrlSelections.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlSelections.TabIndex = 8;
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
			this.m_ctrlSearchSelections.Checked = true;
			this.m_ctrlSearchSelections.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlSearchSelections.Name = "m_ctrlSearchSelections";
			this.m_ctrlSearchSelections.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSearchSelections.TabIndex = 0;
			this.m_ctrlSearchSelections.TabStop = true;
			this.m_ctrlSearchSelections.Text = "Selections :";
			this.m_ctrlSearchSelections.Click += new System.EventHandler(this.OnClickSearchRange);
			// 
			// m_ctrlIncludeSubBinders
			// 
			this.m_ctrlIncludeSubBinders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlIncludeSubBinders.Location = new System.Drawing.Point(8, 116);
			this.m_ctrlIncludeSubBinders.Name = "m_ctrlIncludeSubBinders";
			this.m_ctrlIncludeSubBinders.Size = new System.Drawing.Size(140, 16);
			this.m_ctrlIncludeSubBinders.TabIndex = 4;
			this.m_ctrlIncludeSubBinders.Text = "Include SubBinders";
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
			this.m_ctrlCriteriaGroup.Location = new System.Drawing.Point(8, 156);
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
			this.m_ctrlWholeWord.TabIndex = 15;
			this.m_ctrlWholeWord.Text = "Match Whole Word Only";
			// 
			// m_ctrlFindAnyWord
			// 
			this.m_ctrlFindAnyWord.Checked = true;
			this.m_ctrlFindAnyWord.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlFindAnyWord.Name = "m_ctrlFindAnyWord";
			this.m_ctrlFindAnyWord.Size = new System.Drawing.Size(112, 16);
			this.m_ctrlFindAnyWord.TabIndex = 14;
			this.m_ctrlFindAnyWord.TabStop = true;
			this.m_ctrlFindAnyWord.Text = "Find Any Word";
			// 
			// m_ctrlFindAllWords
			// 
			this.m_ctrlFindAllWords.Location = new System.Drawing.Point(124, 72);
			this.m_ctrlFindAllWords.Name = "m_ctrlFindAllWords";
			this.m_ctrlFindAllWords.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlFindAllWords.TabIndex = 13;
			this.m_ctrlFindAllWords.Text = "Find All Words";
			// 
			// m_ctrlCaseSensitive
			// 
			this.m_ctrlCaseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlCaseSensitive.Location = new System.Drawing.Point(8, 96);
			this.m_ctrlCaseSensitive.Name = "m_ctrlCaseSensitive";
			this.m_ctrlCaseSensitive.Size = new System.Drawing.Size(140, 16);
			this.m_ctrlCaseSensitive.TabIndex = 12;
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
			// m_ctrlXPath
			// 
			this.m_ctrlXPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlXPath.Location = new System.Drawing.Point(16, 312);
			this.m_ctrlXPath.Name = "m_ctrlXPath";
			this.m_ctrlXPath.Size = new System.Drawing.Size(140, 16);
			this.m_ctrlXPath.TabIndex = 13;
			this.m_ctrlXPath.Text = "Use XPath";
			this.m_ctrlXPath.Visible = false;
			// 
			// CFSearch
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 337);
			this.Controls.Add(this.m_ctrlXPath);
			this.Controls.Add(this.m_ctrlCriteriaGroup);
			this.Controls.Add(this.m_ctrlRangeGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFSearch";
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
			m_ctrlMediaTypes.Enabled = m_ctrlSearchAll.Checked;
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
			if(ExchangeOptions(false) == false) return;
			
			//	Is the user searching the current selections?
			if(m_ctrlSearchSelections.Checked == true)
			{
				if((bSuccessful = SearchSelections()) == true)
				{
					//	Update the collection of previous items
					m_tmaxOptions.PreviousItems = m_tmaxSelections;
					m_ctrlSearchPrevious.Enabled = true;
				
				}
				
			}
			else if(m_ctrlSearchPrevious.Checked == true)
			{
				bSuccessful = SearchPrevious();
			}
			else
			{
				bSuccessful = SearchAll();
			}
				
			Cursor.Current = Cursors.Default;
			
			if(bSuccessful == true)
			{
				//	Were there any hits?
				if(m_tmaxSearchResults.Count > 0)
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
		private bool ExchangeOptions(bool bSetControls)
		{
			Debug.Assert(m_tmaxOptions != null);
			if(m_tmaxOptions == null) return false;
			
			try
			{
				//	Are we setting the control states?
				if(bSetControls == true)
				{
					m_ctrlIncludeSubBinders.Checked = m_tmaxOptions.IncludeSubBinders;
					m_ctrlCaseSensitive.Checked = m_tmaxOptions.CaseSensitive;
					m_ctrlWholeWord.Checked = m_tmaxOptions.WholeWords;
					m_ctrlXPath.Checked = m_tmaxOptions.UseXPath;
				
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
					
					m_tmaxOptions.IncludeSubBinders = m_ctrlIncludeSubBinders.Checked;
					m_tmaxOptions.FindAllWords = m_ctrlFindAllWords.Checked;
					m_tmaxOptions.CaseSensitive = m_ctrlCaseSensitive.Checked;
					m_tmaxOptions.WholeWords = m_ctrlWholeWord.Checked;
					
					//m_tmaxOptions.UseXPath = m_ctrlXPath.Checked;
					m_tmaxOptions.UseXPath = true;
					
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
//						m_ctrlSearchStrings.Items.Clear();
//						foreach(string O in m_tmaxOptions.SearchStrings)
//							m_ctrlSearchStrings.Items.Add(O);
//						if(m_ctrlSearchStrings.Items.Count > 0)
//							m_ctrlSearchStrings.SelectedIndex = 0;
							
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
				m_tmaxEventSource.FireError(this, "ExchangeOptions", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_OPTIONS_EX), Ex);
				return false;
			}
			
		}// private void ExchangeOptions(bool bSetControls)
		
		/// <summary>This method is called to search the user selections</summary>
		/// <returns>true if successful</returns>
		private bool SearchSelections()
		{
			Debug.Assert(m_tmaxSelections != null);
			Debug.Assert(m_tmaxSelections.Count > 0);
			if(m_tmaxSelections == null) return false;
			if(m_tmaxSelections.Count == 0) return false;
			
			//	Build the list of records to be searched
			if(QueueSelections() == false) return false;
			
			//	Do we have anything to search?
			if(m_dxQueue.Count == 0)
			{
				MessageBox.Show("Unable to locate any records to search among the current selections", "",
					            MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
				
			}
			
			//	Search the records in the queue
			return SearchQueue();

		}// private void SearchSelections()
		
		/// <summary>This method is called to search the user selections</summary>
		/// <returns>true if successful</returns>
		private bool SearchPrevious()
		{
			Debug.Assert(m_tmaxOptions.PreviousItems != null);
			Debug.Assert(m_tmaxOptions.PreviousItems.Count > 0);
			if(m_tmaxOptions.PreviousItems == null) return false;
			if(m_tmaxOptions.PreviousItems.Count == 0) return false;
			
			//	Build the list of records to be searched
			if(QueuePrevious() == false) return false;
			
			//	Do we have anything to search?
			if(m_dxQueue.Count == 0)
			{
				MessageBox.Show("Unable to locate any records to search among the previous selections", "",
					             MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
				
			}
			
			//	Search the records in the queue
			return SearchQueue();

		}// private void SearchPrevious()
		
		/// <summary>This method is called to search all records of the selected media types</summary>
		/// <returns>true if successful</returns>
		private bool SearchAll()
		{
			//	Build the list of records to be searched
			if(QueueAll() == false) return false;
			
			//	Do we have anything to search?
			if(m_dxQueue.Count == 0)
			{
				MessageBox.Show("Unable to locate any records to search among the selected media types", "",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			
			//	Search the records in the queue
			return SearchQueue();

		}// private void SearchAll()
		
		/// <summary>This method is called to populate the record queue using the current selections</summary>
		/// <returns>True to continue with the operatioin</returns>
		private bool QueueSelections()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxSelections != null);
			Debug.Assert(m_tmaxSelections.Count > 0);
			if(m_tmaxSelections == null) return false;
			if(m_tmaxSelections.Count == 0) return false;
		
			//m_tmaxEventSource.InitElapsed();
			
			//	Flush the existing records
			m_dxQueue.Clear();
			
			try
			{
				bSuccessful = AddToQueue(m_tmaxSelections, false);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "QueueSelections", m_tmaxErrorBuilder.Message(ERROR_QUEUE_SELECTIONS_EX), Ex);
			}
			
			//m_tmaxEventSource.FireElapsed(this, "QueueSelections", "Time to queue selections: ");
			
			return bSuccessful;
			
		}// private bool QueueSelections()
		
		/// <summary>This method is called to populate the record queue using the items from the previous search operation</summary>
		/// <returns>True to continue with the operatioin</returns>
		private bool QueuePrevious()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxOptions != null);
			Debug.Assert(m_tmaxOptions.PreviousItems != null);
			Debug.Assert(m_tmaxOptions.PreviousItems.Count > 0);
			if(m_tmaxOptions == null) return false;
			if(m_tmaxOptions.PreviousItems == null) return false;
			if(m_tmaxOptions.PreviousItems.Count == 0) return false;
		
			//m_tmaxEventSource.InitElapsed();
			
			//	Flush the existing records
			m_dxQueue.Clear();
			
			try
			{
				bSuccessful = AddToQueue(m_tmaxOptions.PreviousItems, true);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "QueuePrevious", m_tmaxErrorBuilder.Message(ERROR_QUEUE_PREVIOUS_EX), Ex);
			}
			
			//m_tmaxEventSource.FireElapsed(this, "QueuePrevious", "Time to queue previous: ");
			
			return bSuccessful;
			
		}// private bool QueuePrevious()
		
		/// <summary>This method is called to populate the record queue using the media types selected by the user</summary>
		/// <returns>True to continue with the operation</returns>
		private bool QueueAll()
		{
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;
		
			//	Flush the existing records
			m_dxQueue.Clear();
			
			try
			{
				//m_tmaxEventSource.InitElapsed();
				
				//	Which selections have been checked
				foreach(int iIndex in m_ctrlMediaTypes.CheckedIndices)
				{
					switch(iIndex)
					{
						case 0:
						
							//	Add all depositions
							AddToQueue(TmaxMediaTypes.Deposition);
							break;
							
						case 1:
						
							//	Add all scripts
							AddToQueue(TmaxMediaTypes.Script);
							break;
							
					}// switch(iIndex)
					
				}// foreach(int iIndex in m_ctrlMediaTypes.CheckedIndices)
				
				//m_tmaxEventSource.FireElapsed(this, "QueueAll", "Time to queue media types: ");
				
				return true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "QueueAll", m_tmaxErrorBuilder.Message(ERROR_QUEUE_ALL_EX), Ex);
				return false;
			}
			
		}// private bool QueueAll()
		
		/// <summary>This method is called to add all records in the specified collection to the search queue</summary>
		///	<param name="tmaxItems">The collection of items to be added</param>
		/// <returns>True to continue with the search operation</returns>
		private bool AddToQueue(CTmaxItems tmaxItems, bool bValidate)
		{
			bool bContinue = true;
			
			Debug.Assert(tmaxItems != null);
			Debug.Assert(tmaxItems.Count > 0);
									
			//	Add each item
			foreach(CTmaxItem O in tmaxItems)
			{
				//	Is this a media record?
				if(O.IPrimary != null)
				{
					if((bValidate == false) || (m_tmaxDatabase.IsValidRecord(O) == true))
						bContinue = AddToQueue((CDxMediaRecord)(O.GetMediaRecord()));
				}
				else if(O.IBinderEntry != null)
				{
					if((bValidate == false) || (m_tmaxDatabase.IsValidRecord(O) == true))
						bContinue = AddToQueue((CDxBinderEntry)(O.IBinderEntry));
				}
				else if(O.MediaType != TmaxMediaTypes.Unknown)
				{
					bContinue = AddToQueue(O.MediaType);
				}
					
				//	Should we stop here?
				if(bContinue == false)
					break;
					
			}// foreach(CTmaxItem O in tmaxItems)
			
			return bContinue;
			
		}// private bool AddToQueue(CTmaxItems tmaxItems)
		
		/// <summary>This method is called to add all records of the specified type to the queue</summary>
		///	<param name="eType">The enumerated media type identifier</param>
		/// <returns>True to continue with the search operation</returns>
		private bool AddToQueue(TmaxMediaTypes eType)
		{
			bool bContinue = true;
			
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;
									
			//	Add each primary record of the specified type
			foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
			{
				//	Is this the right type
				if(dxPrimary.MediaType == eType)
					bContinue = AddToQueue((CDxMediaRecord)dxPrimary);
					
				//	Should we stop here?
				if(bContinue == false)
					break;
					
			}// foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
			
			return bContinue;
			
		}// private bool AddToQueue(CTmaxItems tmaxItems)
		
		/// <summary>This method is called to queue the specified media record</summary>
		/// <param name="dxMedia">The record to be queued</param>
		/// <returns>True to continue with the search operation</returns>
		private bool AddToQueue(CDxMediaRecord dxMedia)
		{
			CDxPrimary		dxScript = null;
			CDxSecondary	dxScene = null;
			
			Debug.Assert(dxMedia != null);
			if(dxMedia == null) return true;
			
			//	Is this a deposition?
			if(dxMedia.MediaType == TmaxMediaTypes.Deposition)
			{
				//	We must be able to get the transcript for this record
				if(((CDxPrimary)dxMedia).GetTranscript() != null)
				{
					//	Add it to the queue
					m_dxQueue.AddList(dxMedia);
				}
			
			}
			
				//	Is this a script?
			else if(dxMedia.MediaType == TmaxMediaTypes.Script)
			{
				dxScript = (CDxPrimary)dxMedia;
				
				//	Do we need to get the children?
				if((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
					dxScript.Fill();
					
				//	Add each scene
				foreach(CDxSecondary O in dxScript.Secondaries)
					AddToQueue((CDxMediaRecord)O);
					
			}
			
				//	Is it a scene?
			else if(dxMedia.MediaType == TmaxMediaTypes.Scene)
			{
				//	Is the source for this scene a designation?
				dxScene = (CDxSecondary)dxMedia;
				if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
					m_dxQueue.AddList(dxScene);
			}
						
			return true;
			
		}// private bool AddToQueue(CDxMediaRecord dxMedia)
		
		/// <summary>This method is called to queue the specified binder record</summary>
		/// <returns>True to continue with print job</returns>
		private bool AddToQueue(CDxBinderEntry dxBinder)
		{
			Debug.Assert(dxBinder != null);
			if(dxBinder == null) return false;
		
			//	Fill the child collection if necessary
			if((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
				dxBinder.Fill();
			
			//	Add each child to the queue
			foreach(CDxBinderEntry O in dxBinder.Contents)
			{
				//	Is this a media entry
				if(O.IsMedia() == true)
				{
					//	Add this media to the queue
					if(O.Source != null)
						AddToQueue(O.Source);
				}
				else
				{
					//	Recurse if requested
					if(m_tmaxOptions.IncludeSubBinders == true)
						AddToQueue(O);
				}
				
			}// foreach(CDxBinderEntry O in dxBinder.Contents)
		
			return true;
			
		}// private bool AddToQueue(CDxBinderEntry dxBinder)
		
		/// <summary>This method is called to search the records in the queue</summary>
		/// <returns>True if successful</returns>
		private bool SearchQueue()
		{
			Debug.Assert(m_dxQueue.Count > 0);
			
			//	Clear the existing results
			m_tmaxSearchResults.Clear();
			
			//m_tmaxEventSource.InitElapsed();
			
			foreach(CDxMediaRecord O in m_dxQueue)
			{
				if(O.MediaType == TmaxMediaTypes.Deposition)
				{
					if(Search((CDxPrimary)O, null) == false)
						return false;
				}
				else if(O.MediaType == TmaxMediaTypes.Scene)
				{
					if(Search((CDxSecondary)O) == false)
						return false;
				}
				else
				{
					//	We should have already filtered out everything else
					Debug.Assert(false);
				}
				
			}// foreach(CDxMediaRecord O in m_dxQueue)
			
			//m_tmaxEventSource.FireElapsed(this, "SearchQueue", "Time to search: ");

			return true;
			
		}// private bool SearchQueue()
		
		/// <summary>This method executes the search against the specified designation</summary>
		/// <param name="dxDeposition">the deposition associated with the search</param>
		/// <param name="dxScene">the scene associated with the search</param>
		/// <returns>true if successful</returns>
		private bool Search(CDxPrimary dxDeposition, CDxSecondary dxScene)
		{
			CXmlTranscript		xmlTranscript = new CXmlTranscript();
			CTmaxSearchResult	tmaxResult    = null;
			XPathDocument		xpDocument    = null;
			XPathNavigator		xpNavigator   = null;
			XPathNodeIterator	xpIterator    = null;
			string				strFileSpec	  = "";
			bool				bCancel = false;
			bool				bSuccessful = false;
			
			//	Get the path to the file containing the transcript
			if(dxScene != null)
			{
				strFileSpec = GetXmlFileSpec(dxScene, ref bCancel);
			}
			else
			{
				strFileSpec = GetXmlFileSpec(dxDeposition, ref bCancel);
			}
			
			//	Was there a problem?
			if((strFileSpec == null) || (strFileSpec.Length == 0))
			{
				//	Should we force a cancelation?
				return (bCancel == false);
			}
			
			try
			{
				//	Load the file
				xpDocument = new System.Xml.XPath.XPathDocument(strFileSpec);
				
				//	Get all transcript elements that meet our search criteria
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				
				if((xpIterator = xpNavigator.Select(m_strXPath)) == null) return false;
				
				//	Add a result for each line returned by the query
				while(xpIterator.MoveNext() == true)
				{
					if(xmlTranscript.SetProperties(xpIterator.Current) == true)
					{
						//	Perform whole word matching if required
						if(m_tmaxOptions.WholeWords == true)
							bSuccessful = MatchWholeWords(xmlTranscript.GetFormattedText());
						else
							bSuccessful = MatchPartialWords(xmlTranscript.GetFormattedText());
							
						//	Is this line a match?
						if(bSuccessful == true)
						{
							//	Add a result for this line
							tmaxResult = new CTmaxSearchResult();
							
							tmaxResult.IDeposition = (ITmaxMediaRecord)dxDeposition;
							tmaxResult.IScene = (ITmaxMediaRecord)dxScene;
							tmaxResult.PL = xmlTranscript.PL;
							tmaxResult.Page = xmlTranscript.Page;
							tmaxResult.Line = xmlTranscript.Line;
							tmaxResult.Text = xmlTranscript.GetFormattedText();
							tmaxResult.Transcript = dxDeposition.Name;
							
							m_tmaxSearchResults.Add(tmaxResult);
						}
						
					}// if(xmlTranscript.SetProperties(xpIterator.Current) == true)
					
				}// while(xpIterator.MoveNext)
				
				return true;
			}
			catch(System.Exception Ex)
			{
				if(dxScene != null)
					m_tmaxEventSource.FireError(this, "Search", m_tmaxErrorBuilder.Message(ERROR_SEARCH_SCENE_EX, dxScene.GetBarcode(false)), Ex);
				else
					m_tmaxEventSource.FireError(this, "Search", m_tmaxErrorBuilder.Message(ERROR_SEARCH_DEPOSITION_EX, dxDeposition.Name), Ex);
				
				return false;
			}
				
		}// private bool Search(CDxPrimary dxDeposition)
		
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
			
			//	The query has already performed this match if we are using
			//	XPath
			if(m_tmaxOptions.UseXPath == true) return true;

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
		
		/// <summary>The application's active database</summary>
		public FTI.Trialmax.Database.CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		/// <summary>Event items that represent the current selections</summary>
		public FTI.Shared.Trialmax.CTmaxItems Selections
		{
			get { return m_tmaxSelections; }
			set { m_tmaxSelections = value; }
		}
		
		/// <summary>Application search options</summary>
		public FTI.Shared.Trialmax.CTmaxSearchOptions Options
		{
			get { return m_tmaxOptions; }
			set { m_tmaxOptions = value; }
		}
		
		/// <summary>The results of the search operation</summary>
		public FTI.Shared.Trialmax.CTmaxSearchResults SearchResults
		{
			get { return m_tmaxSearchResults; }
			set { m_tmaxSearchResults = value; }
		}
		
		#endregion Properties
		
	}// public class CFSearch : FTI.Trialmax.Forms.CFTmaxBaseForm
	
}// namespace FTI.Trialmax.Panes
