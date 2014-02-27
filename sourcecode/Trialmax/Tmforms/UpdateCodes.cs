using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Forms
{
	/// <summary>Form used to execute a bulk update of fielded data codes</summary>
	public class CFUpdateCodes : CFTmaxBaseForm
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		private const int ERROR_FILL_UPDATES_EX		= ERROR_TMAX_FORM_MAX + 1;
		private const int ERROR_SET_UPDATE_EX		= ERROR_TMAX_FORM_MAX + 2;
		private const int ERROR_EXCHANGE_EX			= ERROR_TMAX_FORM_MAX + 3;
		private const int ERROR_QUEUE_ALL_EX		= ERROR_TMAX_FORM_MAX + 4;
		private const int ERROR_QUEUE_TYPE_EX		= ERROR_TMAX_FORM_MAX + 5;
		private const int ERROR_FILL_ACTIONS_EX		= ERROR_TMAX_FORM_MAX + 6;

		#endregion Constants
		
		#region Private Members

		/// <summary>Form designer component container</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Local member bound to CaseCodes property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to PickLists property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickLists = null;
		
		/// <summary>Custom TrialMax list view control to display the code updates</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlCodeUpdates;
		
		/// <summary>Image list bound to the TrialMax list view control</summary>
		private System.Windows.Forms.ImageList m_ctrlUpdateImages;
		
		/// <summary>Private member to store all available code updates</summary>
		private FTI.Shared.Trialmax.CTmaxCodeUpdates m_tmaxUpdates = new CTmaxCodeUpdates();
		
		/// <summary>Private member bound to Actions property</summary>
		private FTI.Shared.Trialmax.CTmaxCodeUpdates m_tmaxActions = new CTmaxCodeUpdates();
		
		/// <summary>Custom TrialMax editor to assign a value to the code update</summary>
		private FTI.Trialmax.Controls.CTmaxEditorCtrl m_ctrlEditor;
		
		/// <summary>Check box to allow caller to request that all records be deleted</summary>
		private System.Windows.Forms.CheckBox m_ctrlDeleteAll;
		
		/// <summary>Pushbutton to allow caller to reset the active update</summary>
		private System.Windows.Forms.Button m_ctrlReset;
		
		/// <summary>Pushbutton to allow caller to reset all updates</summary>
		private System.Windows.Forms.Button m_ctrlResetAll;
		
		/// <summary>Group box for source record options</summary>
		private System.Windows.Forms.GroupBox m_ctrlSourceGroup;
		
		/// <summary>List box of media types when using the All source selection</summary>
		private System.Windows.Forms.CheckedListBox m_ctrlMediaTypes;
		
		/// <summary>Radio button to use filtered source records</summary>
		private System.Windows.Forms.RadioButton m_ctrlSourceFiltered;
		
		/// <summary>Radio button to use selected source records</summary>
		private System.Windows.Forms.RadioButton m_ctrlSourceSelected;
		
		/// <summary>Radio button to use all source records</summary>
		private System.Windows.Forms.RadioButton m_ctrlSourceAll;
		
		/// <summary>Static text label for the editor control</summary>
		private System.Windows.Forms.Label m_ctrlEditorLabel;
		
		/// <summary>Group box for action options</summary>
		private System.Windows.Forms.GroupBox m_ctrlActionGroup;
		
		/// <summary>Private member keep track of the current selection</summary>
		private FTI.Shared.Trialmax.CTmaxCodeUpdate m_tmaxUpdate = null;
		
		/// <summary>Private member bound to Source property</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxSource = new CTmaxItems();
		
		/// <summary>Private member bound to Primaries property</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxPrimaries = null;
		
		/// <summary>Private member bound to Filtered property</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxFiltered = null;
		
		/// <summary>Private member bound to Selected property</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxSelected = null;
		
		/// <summary>Private member bound to AllPrimaries property</summary>
		private bool m_bAllPrimaries = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFUpdateCodes() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Bulk Update Form";
			m_tmaxEventSource.Attach(m_tmaxUpdates.EventSource);
		
		}// public CFUpdateCodes() : base()

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

		/// <summary>Called when the form gets displayed</summary>
		/// <param name="e">Load event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Initialize the list view to display code updates
			m_ctrlCodeUpdates.Initialize(new CTmaxCodeUpdate());
			
			//	Populate the list boxes
			FillMediaTypes();
			FillUpdates();
			
			//	Enable / disable the source options
			if((m_tmaxPrimaries == null) || (m_tmaxPrimaries.Count == 0))
				m_ctrlSourceAll.Enabled = false;
			if((m_tmaxFiltered == null) || (m_tmaxFiltered.Count == 0))
				m_ctrlSourceFiltered.Enabled = false;
			if((m_tmaxSelected == null) || (m_tmaxSelected.Count == 0))
				m_ctrlSourceSelected.Enabled = false;
				
			if(m_ctrlSourceSelected.Enabled == true)
				m_ctrlSourceSelected.Checked = true;
			else
				m_ctrlSourceAll.Checked = true;
				
			//	Must have at least one source option
			if((m_ctrlSourceAll.Enabled == false) &&
				(m_ctrlSourceFiltered.Enabled == false) &&
				(m_ctrlSourceSelected.Enabled == false))
				m_ctrlOk.Enabled = false;

			//	Set the initial control states
			SetControlStates();
			
			//	Perform the bases class processing
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the updates collection.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to change the active update.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange values: SetMembers = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to queue all source records.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to queue all source records of the specified type: type = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the actioned updates.");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFUpdateCodes));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlEditorLabel = new System.Windows.Forms.Label();
			this.m_ctrlCodeUpdates = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlUpdateImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlEditor = new FTI.Trialmax.Controls.CTmaxEditorCtrl();
			this.m_ctrlDeleteAll = new System.Windows.Forms.CheckBox();
			this.m_ctrlReset = new System.Windows.Forms.Button();
			this.m_ctrlResetAll = new System.Windows.Forms.Button();
			this.m_ctrlSourceGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlMediaTypes = new System.Windows.Forms.CheckedListBox();
			this.m_ctrlSourceAll = new System.Windows.Forms.RadioButton();
			this.m_ctrlSourceFiltered = new System.Windows.Forms.RadioButton();
			this.m_ctrlSourceSelected = new System.Windows.Forms.RadioButton();
			this.m_ctrlActionGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlSourceGroup.SuspendLayout();
			this.m_ctrlActionGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(368, 376);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 6;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(280, 376);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 5;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlEditorLabel
			// 
			this.m_ctrlEditorLabel.Location = new System.Drawing.Point(8, 56);
			this.m_ctrlEditorLabel.Name = "m_ctrlEditorLabel";
			this.m_ctrlEditorLabel.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlEditorLabel.TabIndex = 5;
			this.m_ctrlEditorLabel.Text = "Assign :";
			this.m_ctrlEditorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCodeUpdates
			// 
			this.m_ctrlCodeUpdates.AddTop = false;
			this.m_ctrlCodeUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCodeUpdates.AutoResizeColumns = true;
			this.m_ctrlCodeUpdates.ClearOnDblClick = false;
			this.m_ctrlCodeUpdates.DisplayMode = 0;
			this.m_ctrlCodeUpdates.HideSelection = false;
			this.m_ctrlCodeUpdates.Location = new System.Drawing.Point(8, 176);
			this.m_ctrlCodeUpdates.MaxRows = 0;
			this.m_ctrlCodeUpdates.Name = "m_ctrlCodeUpdates";
			this.m_ctrlCodeUpdates.OwnerImages = this.m_ctrlUpdateImages;
			this.m_ctrlCodeUpdates.PaneId = 0;
			this.m_ctrlCodeUpdates.SelectedIndex = -1;
			this.m_ctrlCodeUpdates.ShowHeaders = true;
			this.m_ctrlCodeUpdates.ShowImage = true;
			this.m_ctrlCodeUpdates.Size = new System.Drawing.Size(448, 190);
			this.m_ctrlCodeUpdates.TabIndex = 4;
			this.m_ctrlCodeUpdates.SelectedIndexChanged += new System.EventHandler(this.OnUpdateSelChanged);
			// 
			// m_ctrlUpdateImages
			// 
			this.m_ctrlUpdateImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlUpdateImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlUpdateImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlUpdateImages.ImageStream")));
			this.m_ctrlUpdateImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlEditor
			// 
			this.m_ctrlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlEditor.DropListValues = null;
			this.m_ctrlEditor.FalseText = "False";
			this.m_ctrlEditor.Location = new System.Drawing.Point(56, 56);
			this.m_ctrlEditor.MaxTextLength = 255;
			this.m_ctrlEditor.MemoAsText = false;
			this.m_ctrlEditor.MultiLevel = null;
			this.m_ctrlEditor.MultiLevelSelection = null;
			this.m_ctrlEditor.Name = "m_ctrlEditor";
			this.m_ctrlEditor.PaneId = 0;
			this.m_ctrlEditor.Size = new System.Drawing.Size(200, 56);
			this.m_ctrlEditor.TabIndex = 1;
			this.m_ctrlEditor.TrueText = "True";
			this.m_ctrlEditor.Type = FTI.Trialmax.Controls.TmaxEditorCtrlTypes.Text;
			this.m_ctrlEditor.UserAdditions = false;
			this.m_ctrlEditor.Value = "";
			// 
			// m_ctrlDeleteAll
			// 
			this.m_ctrlDeleteAll.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlDeleteAll.Name = "m_ctrlDeleteAll";
			this.m_ctrlDeleteAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_ctrlDeleteAll.TabIndex = 0;
			this.m_ctrlDeleteAll.Text = "Delete All";
			this.m_ctrlDeleteAll.Click += new System.EventHandler(this.OnClickDeleteAll);
			// 
			// m_ctrlReset
			// 
			this.m_ctrlReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlReset.Location = new System.Drawing.Point(280, 136);
			this.m_ctrlReset.Name = "m_ctrlReset";
			this.m_ctrlReset.TabIndex = 2;
			this.m_ctrlReset.Text = "  &Reset";
			this.m_ctrlReset.Click += new System.EventHandler(this.OnClickReset);
			// 
			// m_ctrlResetAll
			// 
			this.m_ctrlResetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlResetAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlResetAll.Location = new System.Drawing.Point(368, 136);
			this.m_ctrlResetAll.Name = "m_ctrlResetAll";
			this.m_ctrlResetAll.TabIndex = 3;
			this.m_ctrlResetAll.Text = " Reset &All";
			this.m_ctrlResetAll.Click += new System.EventHandler(this.OnClickResetAll);
			// 
			// m_ctrlSourceGroup
			// 
			this.m_ctrlSourceGroup.Controls.Add(this.m_ctrlMediaTypes);
			this.m_ctrlSourceGroup.Controls.Add(this.m_ctrlSourceAll);
			this.m_ctrlSourceGroup.Controls.Add(this.m_ctrlSourceFiltered);
			this.m_ctrlSourceGroup.Controls.Add(this.m_ctrlSourceSelected);
			this.m_ctrlSourceGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlSourceGroup.Name = "m_ctrlSourceGroup";
			this.m_ctrlSourceGroup.Size = new System.Drawing.Size(176, 160);
			this.m_ctrlSourceGroup.TabIndex = 0;
			this.m_ctrlSourceGroup.TabStop = false;
			this.m_ctrlSourceGroup.Text = "Source Records";
			// 
			// m_ctrlMediaTypes
			// 
			this.m_ctrlMediaTypes.CheckOnClick = true;
			this.m_ctrlMediaTypes.IntegralHeight = false;
			this.m_ctrlMediaTypes.Location = new System.Drawing.Point(56, 72);
			this.m_ctrlMediaTypes.Name = "m_ctrlMediaTypes";
			this.m_ctrlMediaTypes.Size = new System.Drawing.Size(112, 80);
			this.m_ctrlMediaTypes.TabIndex = 3;
			// 
			// m_ctrlSourceAll
			// 
			this.m_ctrlSourceAll.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlSourceAll.Name = "m_ctrlSourceAll";
			this.m_ctrlSourceAll.Size = new System.Drawing.Size(48, 16);
			this.m_ctrlSourceAll.TabIndex = 2;
			this.m_ctrlSourceAll.Text = "All :";
			this.m_ctrlSourceAll.Click += new System.EventHandler(this.OnClickSource);
			// 
			// m_ctrlSourceFiltered
			// 
			this.m_ctrlSourceFiltered.Location = new System.Drawing.Point(8, 48);
			this.m_ctrlSourceFiltered.Name = "m_ctrlSourceFiltered";
			this.m_ctrlSourceFiltered.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSourceFiltered.TabIndex = 1;
			this.m_ctrlSourceFiltered.Text = "Filtered";
			this.m_ctrlSourceFiltered.Click += new System.EventHandler(this.OnClickSource);
			// 
			// m_ctrlSourceSelected
			// 
			this.m_ctrlSourceSelected.Checked = true;
			this.m_ctrlSourceSelected.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlSourceSelected.Name = "m_ctrlSourceSelected";
			this.m_ctrlSourceSelected.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlSourceSelected.TabIndex = 0;
			this.m_ctrlSourceSelected.TabStop = true;
			this.m_ctrlSourceSelected.Text = "Selections";
			this.m_ctrlSourceSelected.Click += new System.EventHandler(this.OnClickSource);
			// 
			// m_ctrlActionGroup
			// 
			this.m_ctrlActionGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlActionGroup.Controls.Add(this.m_ctrlDeleteAll);
			this.m_ctrlActionGroup.Controls.Add(this.m_ctrlEditor);
			this.m_ctrlActionGroup.Controls.Add(this.m_ctrlEditorLabel);
			this.m_ctrlActionGroup.Location = new System.Drawing.Point(192, 8);
			this.m_ctrlActionGroup.Name = "m_ctrlActionGroup";
			this.m_ctrlActionGroup.Size = new System.Drawing.Size(264, 120);
			this.m_ctrlActionGroup.TabIndex = 1;
			this.m_ctrlActionGroup.TabStop = false;
			this.m_ctrlActionGroup.Text = "Action";
			// 
			// CFUpdateCodes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(466, 405);
			this.Controls.Add(this.m_ctrlActionGroup);
			this.Controls.Add(this.m_ctrlSourceGroup);
			this.Controls.Add(this.m_ctrlResetAll);
			this.Controls.Add(this.m_ctrlReset);
			this.Controls.Add(this.m_ctrlCodeUpdates);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFUpdateCodes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Update Fielded Data";
			this.m_ctrlSourceGroup.ResumeLayout(false);
			this.m_ctrlActionGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			bool		bSuccessful = false;
			CTmaxItems	tmaxSource = null;
			
			Debug.Assert(m_tmaxSource != null);
			Debug.Assert(m_tmaxActions != null);
			
			//	There might have been a previous attempt
			m_tmaxSource.Clear();
			m_tmaxActions.Clear();
			m_bAllPrimaries = false;
			
			Cursor.Current = Cursors.WaitCursor;
			
			while(bSuccessful == false)
			{
				//	Make sure we update the current selection
				if(Exchange(true) == false)
					break;
					
				//	Are there any updates to be performed?
				if(FillActions() == false)
					break;
				if(m_tmaxActions.Count == 0)
				{
					MessageBox.Show("No updates have been defined.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}
				
				//	Are we queuing all source records
				if(m_ctrlSourceAll.Checked == true)
				{
					if(QueueAll() == false)
						break;
				}
				else
				{
					m_tmaxSource.Clear();
					
					//	Which source collection should we use?
					if(m_ctrlSourceFiltered.Checked == true)
						tmaxSource = m_tmaxFiltered;
					else
						tmaxSource = m_tmaxSelected;
						
					//	Transfer to the operation collection
					foreach(CTmaxItem O in tmaxSource)
						m_tmaxSource.Add(O);
					
				}// if(m_ctrlSourceAll.Checked == true)
				
				//	Are there any source records?
				if(m_tmaxSource.Count == 0)
				{
					MessageBox.Show("No source records to perform the update.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}
				
				//	Are all primary records to be updated?
				if((m_ctrlSourceAll.Enabled == true) && (m_tmaxSource.Count == m_tmaxPrimaries.Count))
				{
					m_bAllPrimaries = true;
				}
				
				//	All is good
				bSuccessful = true;
					
			}//	while(bSuccessful == false)
			
			Cursor.Current = Cursors.Default;
			
			if(bSuccessful == true)
			{
				DialogResult = DialogResult.OK;
				this.Close();
			
			}// if(bSuccessful == true)
			
		}// private void OnClickOK(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on DeleteAll</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickDeleteAll(object sender, System.EventArgs e)
		{
			SetControlStates();
		}
		
		/// <summary>Called to enable / disable the child controls</summary>
		private void SetControlStates()
		{
			if(m_tmaxUpdate != null)
			{
				m_ctrlEditor.Enabled = (m_ctrlDeleteAll.Checked == false);
				m_ctrlDeleteAll.Enabled = true;	
				m_ctrlReset.Enabled = true;
			}
			else
			{
				m_ctrlEditor.Value = "";
				m_ctrlEditor.Enabled = false;
				m_ctrlDeleteAll.Checked = false;
				m_ctrlDeleteAll.Enabled = false;	
				m_ctrlReset.Enabled = false;
			}
			
			m_ctrlMediaTypes.Enabled = (m_ctrlSourceAll.Enabled && m_ctrlSourceAll.Checked);
			if(m_ctrlMediaTypes.Enabled == true)
			{
				m_ctrlMediaTypes.BackColor = System.Drawing.SystemColors.Window;
				m_ctrlMediaTypes.ForeColor = System.Drawing.SystemColors.WindowText;
			}
			else
			{
				m_ctrlMediaTypes.BackColor = System.Drawing.SystemColors.Control;
				m_ctrlMediaTypes.ForeColor = System.Drawing.SystemColors.GrayText;
			}
			
		}// private void SetControlStates()

		/// <summary>This method is called when the user clicks on Reset</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickReset(object sender, System.EventArgs e)
		{
			if(m_tmaxUpdate != null)
			{
				ResetUpdate(m_tmaxUpdate);
				SetControlStates();
			}
			
		}// private void OnClickReset(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on ResetAll</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickResetAll(object sender, System.EventArgs e)
		{
			foreach(CTmaxCodeUpdate O in m_tmaxUpdates)
				ResetUpdate(O);
				
			SetControlStates();
			
		}// private void OnClickResetAll(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on one of the Source radio buttons</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSource(object sender, System.EventArgs e)
		{
			SetControlStates();
			
		}// private void OnClickSource(object sender, System.EventArgs e)

		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			bool bSuccessful = true;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					if(m_tmaxUpdate != null)
					{
						//	Is the case code bound to a pick list?
						if(m_tmaxUpdate.CaseCode.Type == TmaxCodeTypes.PickList)
						{
							//	Make sure we have the pick list bound to the case code
							if((m_tmaxUpdate.CaseCode.PickList == null) && (m_tmaxPickLists != null))
								m_tmaxUpdate.CaseCode.PickList = m_tmaxPickLists.FindList(m_tmaxUpdate.CaseCode.PickListId);
								
							if(m_tmaxUpdate.CaseCode.PickList != null)
							{
								if(m_tmaxUpdate.CaseCode.IsMultiLevel == true)
								{
									m_tmaxUpdate.MultiLevelSelection = m_ctrlEditor.MultiLevelSelection;
									if(m_tmaxUpdate.MultiLevelSelection != null)
										m_tmaxUpdate.Value = m_tmaxUpdate.MultiLevelSelection.Name;
									else
										m_tmaxUpdate.Value = "";
																			
								}
								else
								{
									m_tmaxUpdate.MultiLevelSelection = null;
									m_tmaxUpdate.Value = m_ctrlEditor.Value;
								}
							
							}
							else
							{
								m_tmaxUpdate.MultiLevelSelection = null;
								m_tmaxUpdate.Value = m_ctrlEditor.Value;
							
							}// if(m_tmaxUpdate.CaseCode.PickList != null)
						
						}
						else
						{
							m_tmaxUpdate.MultiLevelSelection = null;
							m_tmaxUpdate.Value = m_ctrlEditor.Value;
						}

						//	Set the update action
						if(m_ctrlDeleteAll.Checked == true)
							m_tmaxUpdate.Action = TmaxCodeActions.Delete;
						else if(m_tmaxUpdate.Value.Length > 0)
							m_tmaxUpdate.Action = TmaxCodeActions.Add;
						else
							m_tmaxUpdate.Action = TmaxCodeActions.Unknown;
						
					}// if(m_tmaxUpdate != null)

				}
				else
				{
					if(m_tmaxUpdate != null)
					{
						//	Make sure we have the pick list bound to the case code
						if(m_tmaxUpdate.CaseCode.Type == TmaxCodeTypes.PickList)
						{
							if((m_tmaxUpdate.CaseCode.PickList == null) && (m_tmaxPickLists != null))
								m_tmaxUpdate.CaseCode.PickList = m_tmaxPickLists.FindList(m_tmaxUpdate.CaseCode.PickListId);
						}
								
						//	Set the editor's type and value
						m_ctrlEditor.SetType(m_tmaxUpdate.CaseCode);
				
						//	Is this a pick list type code?
						if(m_tmaxUpdate.CaseCode.Type == TmaxCodeTypes.PickList)
						{
							if(m_tmaxUpdate.CaseCode.IsMultiLevel == true)
							{		
								m_ctrlEditor.SetDropListValues(null);
								m_ctrlEditor.MultiLevel = m_tmaxUpdate.CaseCode.PickList;
								m_ctrlEditor.MultiLevelSelection = m_tmaxUpdate.MultiLevelSelection;
							}
							else
							{
								m_ctrlEditor.MultiLevel = null;
								m_ctrlEditor.MultiLevelSelection = null;
								m_ctrlEditor.SetDropListValues(m_tmaxUpdate.CaseCode.PickList.Children);
								m_ctrlEditor.Value = m_tmaxUpdate.Value;
							
							}// if(m_tmaxUpdate.CaseCode.IsMultiLevel == true)

						}// if(m_tmaxUpdate.CaseCode.Type == TmaxCodeTypes.PickList)
						else
						{
							m_ctrlEditor.SetDropListValues(null);
							m_ctrlEditor.MultiLevel = null;
							m_ctrlEditor.MultiLevelSelection = null;
							m_ctrlEditor.Value = m_tmaxUpdate.Value;
						}
						
						m_ctrlDeleteAll.Checked = (m_tmaxUpdate.Action == TmaxCodeActions.Delete);

					}
					else
					{
						m_ctrlEditor.SetDropListValues(null);
						m_ctrlEditor.SetType(TmaxEditorCtrlTypes.Text);
						m_ctrlEditor.MultiLevelSelection = null;
						m_ctrlEditor.Value = "";
						m_ctrlDeleteAll.Checked = false;
					
					}// if(m_tmaxUpdate != null)
				
				}// if(bSetMembers == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful; 
			
		}// private bool Exchange(bool bSetMembers)
		
		/// <summary>This method will populate the Media Types list box</summary>
		private void FillMediaTypes()
		{
			m_ctrlMediaTypes.Items.Add("Documents", true);
			m_ctrlMediaTypes.Items.Add("PowerPoints", true);
			m_ctrlMediaTypes.Items.Add("Recordings", true);
			m_ctrlMediaTypes.Items.Add("Depositions", true);
			m_ctrlMediaTypes.Items.Add("Scripts", true);
		
		}// private void FillMediaTypes()
		
		/// <summary>Called to populate the list of code updates</summary>
		private void FillUpdates()
		{
			try
			{
				if(m_tmaxCaseCodes != null)
				{
					foreach(CTmaxCaseCode O in m_tmaxCaseCodes)
					{
						//	Only support single-instance case codes
						if(O.AllowMultiple == false)
							m_tmaxUpdates.Add(new CTmaxCodeUpdate(O));
					
					}// foreach(CTmaxCaseCode O in m_tmaxCaseCodes)
				
				}// if(m_tmaxCaseCodes != null)
				
				m_ctrlCodeUpdates.Add(m_tmaxUpdates, true);
				m_ctrlCodeUpdates.ResizeColumns();
				
				//	Set the initial selection
				if(m_tmaxUpdates.Count > 0)
					m_ctrlCodeUpdates.SetSelectedIndex(0, false);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillUpdates", m_tmaxErrorBuilder.Message(ERROR_FILL_UPDATES_EX), Ex);
			}
		
		}// private void FillUpdates()
		
		/// <summary>Called to populate the list of actioned code updates</summary>
		/// <returns>True if successful</returns>
		private bool FillActions()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxActions != null);
			Debug.Assert(m_tmaxUpdates != null);
			
			try
			{
				foreach(CTmaxCodeUpdate O in m_tmaxUpdates)
				{
					//	Has an action been assigned to this update?
					if(O.Action != TmaxCodeActions.Unknown)
						m_tmaxActions.Add(O);
					
				}// foreach(CTmaxCodeUpdate O in m_tmaxUpdates)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillActions", m_tmaxErrorBuilder.Message(ERROR_FILL_ACTIONS_EX), Ex);
			}
			
			return bSuccessful;
		
		}// private void FillActions()
		
		/// <summary>Handles events fired when the user selects a new code update</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The arguments passed with the event</param>
		private void OnUpdateSelChanged(object sender, System.EventArgs e)
		{
			//	Set the new selection
			SetUpdate((CTmaxCodeUpdate)(m_ctrlCodeUpdates.GetSelected()), false);
		
		}// private void OnUpdateSelChanged(object sender, System.EventArgs e)

		/// <summary>Called to set the active update</summary>
		/// <param name="tmaxUpdate">The update to be activated</param>
		/// <param name="bSynchronize">True to synchronize the list view selection</param>
		/// <returns>true if successful</returns>
		private bool SetUpdate(CTmaxCodeUpdate tmaxUpdate, bool bSynchronize)
		{
			bool bSuccessful = true;
			
			try
			{
				//	Should we update the current selection first?
				if(m_tmaxUpdate != null)
				{
					bSuccessful = Exchange(true);
					m_ctrlCodeUpdates.Update(m_tmaxUpdate);
				}
				
				//	Should we make the switch?
				if(bSuccessful == true)
				{
					m_tmaxUpdate = tmaxUpdate;
					Exchange(false);
				}
				
				//	Are we supposed to synchronize the list view?
				if((bSynchronize == true) || (bSuccessful == false))
					m_ctrlCodeUpdates.SetSelected(m_tmaxUpdate, true);
				
				//	Enable/disable the child controls
				SetControlStates();
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetUpdate", m_tmaxErrorBuilder.Message(ERROR_SET_UPDATE_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private void SetUpdate(CTmaxCodeUpdate tmaxUpdate, bool bSynchronize)

		/// <summary>Called to reset the values associated with the specified update</summary>
		/// <param name="tmaxUpdate">The update to be reset</param>
		private void ResetUpdate(CTmaxCodeUpdate tmaxUpdate)
		{
			Debug.Assert(tmaxUpdate != null);
			
			try
			{
				tmaxUpdate.Action = TmaxCodeActions.Unknown;
				tmaxUpdate.Value = "";
				tmaxUpdate.MultiLevelSelection = null;
				
				//	Update the updates list
				m_ctrlCodeUpdates.Update(tmaxUpdate);
				
				//	Is this the active update?
				if(ReferenceEquals(tmaxUpdate, m_tmaxUpdate) == true)
				{
					//	Clear the action controls
					m_ctrlEditor.SetValue("");
					m_ctrlEditor.SetMultiLevelSelection(null);
					m_ctrlDeleteAll.Checked = false;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ResetUpdate", Ex);
			}
			
		}// private void ResetUpdate(CTmaxCodeUpdate tmaxUpdate)

		/// <summary>This method is called to populate the source queue using the media types selected by the user</summary>
		/// <returns>True to continue with the operation</returns>
		private bool QueueAll()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxSource != null);
		
			//	Flush the existing records
			m_tmaxSource.Clear();
			
			try
			{
				//m_tmaxEventSource.InitElapsed();
				
				//	Has the user selected all media types?
				if(m_ctrlMediaTypes.CheckedIndices.Count == m_ctrlMediaTypes.Items.Count)
				{
					QueueAll(TmaxMediaTypes.Unknown);					
				}
				else
				{				
					//	Which selections have been checked
					foreach(int iIndex in m_ctrlMediaTypes.CheckedIndices)
					{
						// See FillMediaTypes() to verify the case values
						switch(iIndex)
						{
							case 0:		QueueAll(TmaxMediaTypes.Document);
										break;
							case 1:		QueueAll(TmaxMediaTypes.Powerpoint);
										break;
							case 2:		QueueAll(TmaxMediaTypes.Recording);
										break;
							case 3:		QueueAll(TmaxMediaTypes.Deposition);
										break;
							case 4:		QueueAll(TmaxMediaTypes.Script);
										break;
						
						}// switch(iIndex)
						
					}// foreach(int iIndex in m_ctrlMediaTypes.CheckedIndices)
				
				}// if(m_ctrlMediaTypes.CheckedIndices.Count == m_ctrlMediaTypes.Items.Count)
				
				//m_tmaxEventSource.FireElapsed(this, "QueueAll", "Time to queue media types: ");
				
				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "QueueAll", m_tmaxErrorBuilder.Message(ERROR_QUEUE_ALL_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool QueueAll()
		
		/// <summary>Called to queue all records of the specified type</summary>
		/// <param name="eType">The media type to be queued</param>
		/// <returns>True to continue with the operation</returns>
		private bool QueueAll(TmaxMediaTypes eType)
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxSource != null);
		
			try
			{
				//	Locate all records of the specified type
				foreach(CTmaxItem O in m_tmaxPrimaries)
				{
					if((eType == TmaxMediaTypes.Unknown) || (O.MediaType == eType))
						m_tmaxSource.Add(O);
				}
				
				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "QueueAll", m_tmaxErrorBuilder.Message(ERROR_QUEUE_TYPE_EX, eType), Ex);
			}
			
			return bSuccessful;
			
		}// private bool QueueAll(TmaxMediaTypes eType)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Collection of updates that combine to define the operation</summary>
		public CTmaxCodeUpdates Actions
		{
			get { return m_tmaxActions; }
		}
		
		/// <summary>Collection of event items to represent the source records for the operation</summary>
		public CTmaxItems Source
		{
			get { return m_tmaxSource; }
		}
		
		/// <summary>Collection of event items to represent all primary records</summary>
		public CTmaxItems Primaries
		{
			get { return m_tmaxPrimaries; }
			set { m_tmaxPrimaries = value; }
		}
		
		/// <summary>Collection of event items to represent all filtered primary records</summary>
		public CTmaxItems Filtered
		{
			get { return m_tmaxFiltered; }
			set { m_tmaxFiltered = value; }
		}
		
		/// <summary>Collection of event items to represent all selected primary records</summary>
		public CTmaxItems Selected
		{
			get { return m_tmaxSelected; }
			set { m_tmaxSelected = value; }
		}
		
		/// <summary>The application's collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
			set { m_tmaxCaseCodes = value; }
		}
		
		/// <summary>Flag to indicate that all primaries are included in the operation</summary>
		public bool AllPrimaries
		{
			get { return m_bAllPrimaries; }
		}
		
		/// <summary>The application's collection of pick lists</summary>
		public CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
			set { m_tmaxPickLists = value; }
		}
		
		#endregion Properties
		
	}// public class CFUpdateCodes : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
