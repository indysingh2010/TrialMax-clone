using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Panes
{
	/// <summary>Form used to clean scanned images</summary>
	public class CFCleanScanned : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants

		protected const int ERROR_EXCHANGE_OPTIONS_EX		= (ERROR_TMAX_FORM_MAX + 1);
		protected const int ERROR_CLEAN_EX					= (ERROR_TMAX_FORM_MAX + 2);

		#endregion Constants
		
		#region Private Members

		/// <summary>Control collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Pushbutton to request cleaning</summary>
		private System.Windows.Forms.Button m_ctrlClean;
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Local member bound to Items property</summary>
		private FTI.Shared.Trialmax.CTmaxItems m_tmaxItems = null;
		
		/// <summary>The list of selected items</summary>
		private System.Windows.Forms.Label m_ctrlSelections;
		
		/// <summary>The selections list label</summary>
		private System.Windows.Forms.Label m_ctrlSelectionsLabel;
		
		/// <summary>Local member bound to CleanOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxCleanOptions m_tmaxCleanOptions = null;
		
		/// <summary>Local member bound to Database property</summary>
		private FTI.Trialmax.Database.CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local flag to indicate that cleaning is under way</summary>
		private bool m_bCleaning = false;
		
		/// <summary>Local flag to indicate that the user canceled the operation</summary>
		private bool m_bCanceled = false;
		
		/// <summary>Local flag to indicate that the user has confirmed rotation of treated pages</summary>
		private bool m_bTreatedConfirmed = false;
		
		/// <summary>Local flag to indicate that the treated pages should be rotated</summary>
		private bool m_bRotateTreated = false;
		
		/// <summary>Label for control that displays the path to the image being cleaned</summary>
		private System.Windows.Forms.Label m_ctrlCleaningLabel;
		
		/// <summary>Label to display the path to the image being cleaned</summary>
		private System.Windows.Forms.Label m_ctrlCleaning;
		
		/// <summary>Group box for error messages</summary>
		private System.Windows.Forms.GroupBox m_ctrlErrorsGroup;
		
		/// <summary>List box to display error messages</summary>
		private System.Windows.Forms.ListView m_ctrlErrors;
		
		/// <summary>Errors list box image column</summary>
		private System.Windows.Forms.ColumnHeader I;
		
		/// <summary>Errors list box barcode column</summary>
		private System.Windows.Forms.ColumnHeader BARCODE;
		
		/// <summary>Errors list box description column</summary>
		private System.Windows.Forms.ColumnHeader DESCRIPTION;
		
		/// <summary>Errors list box image list</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Group box for cleaning options</summary>
		private System.Windows.Forms.GroupBox m_ctrlOptionsGroup;
		
		/// <summary>Text control to define maximum number of hole punches to search for</summary>
		private System.Windows.Forms.TextBox m_ctrlMaxHoleCount;
		
		/// <summary>Label for maximum hole count text control</summary>
		private System.Windows.Forms.Label m_ctrlMaxHoleCountLabel;
		
		/// <summary>Text control to define minimum number of hole punches to search for</summary>
		private System.Windows.Forms.TextBox m_ctrlMinHoleCount;
		
		/// <summary>Label for minimum hole count text control</summary>
		private System.Windows.Forms.Label m_ctrlMinHoleCountLabel;
		
		/// <summary>Check box to request searching for hole punches on top edge</summary>
		private System.Windows.Forms.CheckBox m_ctrlHolesTop;
		
		/// <summary>Check box to request searching for hole punches on right edge</summary>
		private System.Windows.Forms.CheckBox m_ctrlHolesRight;
		
		/// <summary>Check box to request searching for hole punches on left edge</summary>
		private System.Windows.Forms.CheckBox m_ctrlHolesLeft;
		
		/// <summary>Check box to request removal of hole punches during cleaning</summary>
		private System.Windows.Forms.CheckBox m_ctrlRemoveHolePunches;
		
		/// <summary>Check box to request despeckling during cleaning</summary>
		private System.Windows.Forms.CheckBox m_ctrlDespeckle;
		
		/// <summary>Check box to request deskewing during cleaning</summary>
		private System.Windows.Forms.CheckBox m_ctrlDeskew;
		
		/// <summary>Check box to request confirmation before saving the cleaned image</summary>
		private System.Windows.Forms.CheckBox m_ctrlPromptBeforeSave;
		
		/// <summary>Check box to request searching for hole punches on bottom edge</summary>
		private System.Windows.Forms.CheckBox m_ctrlHolesBottom;
		
		/// <summary>Group box to frame the image viewer</summary>
		private System.Windows.Forms.GroupBox m_ctrlViewerGroup;
		
		/// <summary>Image viewer used to display and clean the images</summary>
		private FTI.Trialmax.ActiveX.CTmxView m_tmxView;
		
		/// <summary>Radio button for no rotation</summary>
		private System.Windows.Forms.RadioButton m_ctrlRotateNone;
		
		/// <summary>Radio button for clockwise rotation</summary>
		private System.Windows.Forms.RadioButton m_ctrlRotateCW;
		
		/// <summary>Radio button for counter-clockwise rotation</summary>
		private System.Windows.Forms.RadioButton m_ctrlRotateCCW;
		
		/// <summary>Group box to contain rotation radio buttons</summary>
		private System.Windows.Forms.GroupBox m_ctrlRotateGroup;
		
		/// <summary>Local member to keep track of barcode being cleaned</summary>
		private string m_strActiveBarcode = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFCleanScanned() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			//	Initialize the Tmview control
			m_tmxView.AxInitialize();
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
		
		/// <summary>Overridden base class member called when the form gets displayed</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Initialize the controls
			ExchangeOptions(true);
			ResetErrorMessages();
			OnClickRemoveHolePunches(m_ctrlRemoveHolePunches, System.EventArgs.Empty);
			
			//	Do we have any active selections?
			if((m_tmaxItems != null) && (m_tmaxItems.Count > 0))
			{
				m_ctrlSelections.Text = CTmaxToolbox.FitPathToWidth(m_tmaxItems.ToString(), m_ctrlSelections);
			}
			else
			{
				m_ctrlClean.Enabled = false;
			}

			// Do the base class processing
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exchanging the job options");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while cleaning the record: barcode = %1");
	
		}// protected override void SetErrorStrings()

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to determine if the user has canceled the operation</summary>
		/// <returns>True if canceled</returns>
		private bool CheckCanceled()
		{
			Application.DoEvents();
			return m_bCanceled;
		}
		
		/// <summary>This method is called to add an error to the list box</summary>
		/// <param name="strBarcode">The barcode of the associated record</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="bCritical">True if this is a critical error</param>
		private void AddError(string strBarcode, string strMessage, bool bCritical)
		{
			ListViewItem lvItem = null;
			
			if(m_ctrlErrors == null) return;
			if(m_ctrlErrors.IsDisposed == true) return;
			
			try
			{
				lvItem = new ListViewItem();
							
				lvItem.ImageIndex = bCritical ? 0 : 1;
				lvItem.SubItems.Add(strBarcode);
				lvItem.SubItems.Add(strMessage);
							
				m_ctrlErrors.Items.Add(lvItem);

				//	Automatically resize the columns to fit the text
				SuspendLayout();
				m_ctrlErrors.Columns[0].Width = -2;
				m_ctrlErrors.Columns[1].Width = -2;
				m_ctrlErrors.Columns[2].Width = -2;
				ResumeLayout();

			}
			catch
			{
			}

		}// AddError(string strBarcode, string strMessage, bool bCritical)
		
		/// <summary>This method is called to add an error to the list box</summary>
		/// <param name="strBarcode">The barcode of the associated record</param>
		/// <param name="strMessage">The error message</param>
		private void AddError(string strBarcode, string strMessage)
		{
			AddError(strBarcode, strMessage, true);
			
		}// AddError(string strBarcode, string strMessage)
		
		/// <summary>This method is called to add an error warning to the list box</summary>
		/// <param name="strBarcode">The barcode of the associated record</param>
		/// <param name="strMessage">The error message</param>
		private void AddWarning(string strBarcode, string strMessage)
		{
			AddError(strBarcode, strMessage, false);
			
		}// AddWarning(string strBarcode, string strMessage)
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFCleanScanned));
			this.m_ctrlClean = new System.Windows.Forms.Button();
			this.m_ctrlSelections = new System.Windows.Forms.Label();
			this.m_ctrlSelectionsLabel = new System.Windows.Forms.Label();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlCleaningLabel = new System.Windows.Forms.Label();
			this.m_ctrlCleaning = new System.Windows.Forms.Label();
			this.m_ctrlErrorsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlErrors = new System.Windows.Forms.ListView();
			this.I = new System.Windows.Forms.ColumnHeader();
			this.BARCODE = new System.Windows.Forms.ColumnHeader();
			this.DESCRIPTION = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlPromptBeforeSave = new System.Windows.Forms.CheckBox();
			this.m_ctrlHolesTop = new System.Windows.Forms.CheckBox();
			this.m_ctrlMinHoleCountLabel = new System.Windows.Forms.Label();
			this.m_ctrlMinHoleCount = new System.Windows.Forms.TextBox();
			this.m_ctrlRemoveHolePunches = new System.Windows.Forms.CheckBox();
			this.m_ctrlMaxHoleCountLabel = new System.Windows.Forms.Label();
			this.m_ctrlMaxHoleCount = new System.Windows.Forms.TextBox();
			this.m_ctrlDespeckle = new System.Windows.Forms.CheckBox();
			this.m_ctrlHolesBottom = new System.Windows.Forms.CheckBox();
			this.m_ctrlHolesRight = new System.Windows.Forms.CheckBox();
			this.m_ctrlHolesLeft = new System.Windows.Forms.CheckBox();
			this.m_ctrlDeskew = new System.Windows.Forms.CheckBox();
			this.m_ctrlViewerGroup = new System.Windows.Forms.GroupBox();
			this.m_tmxView = new FTI.Trialmax.ActiveX.CTmxView();
			this.m_ctrlRotateNone = new System.Windows.Forms.RadioButton();
			this.m_ctrlRotateCW = new System.Windows.Forms.RadioButton();
			this.m_ctrlRotateCCW = new System.Windows.Forms.RadioButton();
			this.m_ctrlRotateGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlErrorsGroup.SuspendLayout();
			this.m_ctrlOptionsGroup.SuspendLayout();
			this.m_ctrlViewerGroup.SuspendLayout();
			this.m_ctrlRotateGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlClean
			// 
			this.m_ctrlClean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlClean.Location = new System.Drawing.Point(484, 460);
			this.m_ctrlClean.Name = "m_ctrlClean";
			this.m_ctrlClean.TabIndex = 3;
			this.m_ctrlClean.Text = "&Clean";
			this.m_ctrlClean.Click += new System.EventHandler(this.OnClickClean);
			// 
			// m_ctrlSelections
			// 
			this.m_ctrlSelections.Location = new System.Drawing.Point(76, 8);
			this.m_ctrlSelections.Name = "m_ctrlSelections";
			this.m_ctrlSelections.Size = new System.Drawing.Size(264, 16);
			this.m_ctrlSelections.TabIndex = 9;
			this.m_ctrlSelections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSelectionsLabel
			// 
			this.m_ctrlSelectionsLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlSelectionsLabel.Name = "m_ctrlSelectionsLabel";
			this.m_ctrlSelectionsLabel.Size = new System.Drawing.Size(64, 16);
			this.m_ctrlSelectionsLabel.TabIndex = 10;
			this.m_ctrlSelectionsLabel.Text = "Selections:";
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.Location = new System.Drawing.Point(568, 460);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "Cl&ose";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlCleaningLabel
			// 
			this.m_ctrlCleaningLabel.Location = new System.Drawing.Point(348, 8);
			this.m_ctrlCleaningLabel.Name = "m_ctrlCleaningLabel";
			this.m_ctrlCleaningLabel.Size = new System.Drawing.Size(64, 16);
			this.m_ctrlCleaningLabel.TabIndex = 13;
			this.m_ctrlCleaningLabel.Text = "Cleaning:";
			// 
			// m_ctrlCleaning
			// 
			this.m_ctrlCleaning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCleaning.Location = new System.Drawing.Point(416, 8);
			this.m_ctrlCleaning.Name = "m_ctrlCleaning";
			this.m_ctrlCleaning.Size = new System.Drawing.Size(232, 16);
			this.m_ctrlCleaning.TabIndex = 12;
			this.m_ctrlCleaning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlErrorsGroup
			// 
			this.m_ctrlErrorsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlErrorsGroup.Controls.Add(this.m_ctrlErrors);
			this.m_ctrlErrorsGroup.Location = new System.Drawing.Point(8, 368);
			this.m_ctrlErrorsGroup.Name = "m_ctrlErrorsGroup";
			this.m_ctrlErrorsGroup.Size = new System.Drawing.Size(332, 116);
			this.m_ctrlErrorsGroup.TabIndex = 2;
			this.m_ctrlErrorsGroup.TabStop = false;
			this.m_ctrlErrorsGroup.Text = "Errors:";
			// 
			// m_ctrlErrors
			// 
			this.m_ctrlErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.I,
																						   this.BARCODE,
																						   this.DESCRIPTION});
			this.m_ctrlErrors.FullRowSelect = true;
			this.m_ctrlErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.m_ctrlErrors.HideSelection = false;
			this.m_ctrlErrors.LabelWrap = false;
			this.m_ctrlErrors.Location = new System.Drawing.Point(8, 16);
			this.m_ctrlErrors.MultiSelect = false;
			this.m_ctrlErrors.Name = "m_ctrlErrors";
			this.m_ctrlErrors.Size = new System.Drawing.Size(316, 92);
			this.m_ctrlErrors.TabIndex = 0;
			this.m_ctrlErrors.View = System.Windows.Forms.View.Details;
			// 
			// I
			// 
			this.I.Text = "";
			this.I.Width = 16;
			// 
			// BARCODE
			// 
			this.BARCODE.Text = "Barcode";
			this.BARCODE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// DESCRIPTION
			// 
			this.DESCRIPTION.Text = "Description";
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlOptionsGroup
			// 
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlPromptBeforeSave);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHolesTop);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlMinHoleCountLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlMinHoleCount);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlRemoveHolePunches);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlMaxHoleCountLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlMaxHoleCount);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlDespeckle);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHolesBottom);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHolesRight);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHolesLeft);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlDeskew);
			this.m_ctrlOptionsGroup.Location = new System.Drawing.Point(8, 32);
			this.m_ctrlOptionsGroup.Name = "m_ctrlOptionsGroup";
			this.m_ctrlOptionsGroup.Size = new System.Drawing.Size(332, 224);
			this.m_ctrlOptionsGroup.TabIndex = 0;
			this.m_ctrlOptionsGroup.TabStop = false;
			this.m_ctrlOptionsGroup.Text = "Options";
			// 
			// m_ctrlPromptBeforeSave
			// 
			this.m_ctrlPromptBeforeSave.Location = new System.Drawing.Point(12, 24);
			this.m_ctrlPromptBeforeSave.Name = "m_ctrlPromptBeforeSave";
			this.m_ctrlPromptBeforeSave.Size = new System.Drawing.Size(248, 20);
			this.m_ctrlPromptBeforeSave.TabIndex = 0;
			this.m_ctrlPromptBeforeSave.Text = "Prompt Before Save";
			// 
			// m_ctrlHolesTop
			// 
			this.m_ctrlHolesTop.Location = new System.Drawing.Point(28, 172);
			this.m_ctrlHolesTop.Name = "m_ctrlHolesTop";
			this.m_ctrlHolesTop.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlHolesTop.TabIndex = 6;
			this.m_ctrlHolesTop.Text = "Check Top Edge";
			// 
			// m_ctrlMinHoleCountLabel
			// 
			this.m_ctrlMinHoleCountLabel.Location = new System.Drawing.Point(172, 124);
			this.m_ctrlMinHoleCountLabel.Name = "m_ctrlMinHoleCountLabel";
			this.m_ctrlMinHoleCountLabel.Size = new System.Drawing.Size(116, 20);
			this.m_ctrlMinHoleCountLabel.TabIndex = 8;
			this.m_ctrlMinHoleCountLabel.Text = "Minimum Hole Count:";
			this.m_ctrlMinHoleCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMinHoleCount
			// 
			this.m_ctrlMinHoleCount.Location = new System.Drawing.Point(296, 124);
			this.m_ctrlMinHoleCount.Name = "m_ctrlMinHoleCount";
			this.m_ctrlMinHoleCount.Size = new System.Drawing.Size(28, 20);
			this.m_ctrlMinHoleCount.TabIndex = 8;
			this.m_ctrlMinHoleCount.Text = "";
			this.m_ctrlMinHoleCount.WordWrap = false;
			// 
			// m_ctrlRemoveHolePunches
			// 
			this.m_ctrlRemoveHolePunches.Location = new System.Drawing.Point(12, 100);
			this.m_ctrlRemoveHolePunches.Name = "m_ctrlRemoveHolePunches";
			this.m_ctrlRemoveHolePunches.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlRemoveHolePunches.TabIndex = 3;
			this.m_ctrlRemoveHolePunches.Text = "Remove Hole Punches";
			this.m_ctrlRemoveHolePunches.Click += new System.EventHandler(this.OnClickRemoveHolePunches);
			// 
			// m_ctrlMaxHoleCountLabel
			// 
			this.m_ctrlMaxHoleCountLabel.Location = new System.Drawing.Point(172, 148);
			this.m_ctrlMaxHoleCountLabel.Name = "m_ctrlMaxHoleCountLabel";
			this.m_ctrlMaxHoleCountLabel.Size = new System.Drawing.Size(116, 20);
			this.m_ctrlMaxHoleCountLabel.TabIndex = 9;
			this.m_ctrlMaxHoleCountLabel.Text = "Maximum Hole Count:";
			this.m_ctrlMaxHoleCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMaxHoleCount
			// 
			this.m_ctrlMaxHoleCount.Location = new System.Drawing.Point(296, 148);
			this.m_ctrlMaxHoleCount.Name = "m_ctrlMaxHoleCount";
			this.m_ctrlMaxHoleCount.Size = new System.Drawing.Size(28, 20);
			this.m_ctrlMaxHoleCount.TabIndex = 9;
			this.m_ctrlMaxHoleCount.Text = "";
			this.m_ctrlMaxHoleCount.WordWrap = false;
			// 
			// m_ctrlDespeckle
			// 
			this.m_ctrlDespeckle.Location = new System.Drawing.Point(12, 76);
			this.m_ctrlDespeckle.Name = "m_ctrlDespeckle";
			this.m_ctrlDespeckle.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlDespeckle.TabIndex = 2;
			this.m_ctrlDespeckle.Text = "Despeckle";
			// 
			// m_ctrlHolesBottom
			// 
			this.m_ctrlHolesBottom.Location = new System.Drawing.Point(28, 196);
			this.m_ctrlHolesBottom.Name = "m_ctrlHolesBottom";
			this.m_ctrlHolesBottom.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlHolesBottom.TabIndex = 7;
			this.m_ctrlHolesBottom.Text = "Check Bottom Edge";
			// 
			// m_ctrlHolesRight
			// 
			this.m_ctrlHolesRight.Location = new System.Drawing.Point(28, 148);
			this.m_ctrlHolesRight.Name = "m_ctrlHolesRight";
			this.m_ctrlHolesRight.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlHolesRight.TabIndex = 5;
			this.m_ctrlHolesRight.Text = "Check Right Edge";
			// 
			// m_ctrlHolesLeft
			// 
			this.m_ctrlHolesLeft.Location = new System.Drawing.Point(28, 124);
			this.m_ctrlHolesLeft.Name = "m_ctrlHolesLeft";
			this.m_ctrlHolesLeft.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlHolesLeft.TabIndex = 4;
			this.m_ctrlHolesLeft.Text = "Check Left Edge";
			// 
			// m_ctrlDeskew
			// 
			this.m_ctrlDeskew.Location = new System.Drawing.Point(12, 52);
			this.m_ctrlDeskew.Name = "m_ctrlDeskew";
			this.m_ctrlDeskew.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlDeskew.TabIndex = 1;
			this.m_ctrlDeskew.Text = "Deskew";
			// 
			// m_ctrlViewerGroup
			// 
			this.m_ctrlViewerGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlViewerGroup.Controls.Add(this.m_tmxView);
			this.m_ctrlViewerGroup.Location = new System.Drawing.Point(348, 32);
			this.m_ctrlViewerGroup.Name = "m_ctrlViewerGroup";
			this.m_ctrlViewerGroup.Size = new System.Drawing.Size(300, 420);
			this.m_ctrlViewerGroup.TabIndex = 17;
			this.m_ctrlViewerGroup.TabStop = false;
			// 
			// m_tmxView
			// 
			this.m_tmxView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_tmxView.AxAutoSave = false;
			this.m_tmxView.AxError = ((short)(0));
			this.m_tmxView.EnableToolbar = true;
			this.m_tmxView.IniFilename = "";
			this.m_tmxView.IniSection = "";
			this.m_tmxView.Location = new System.Drawing.Point(8, 16);
			this.m_tmxView.Name = "m_tmxView";
			this.m_tmxView.NavigatorPosition = -1;
			this.m_tmxView.NavigatorTotal = 0;
			this.m_tmxView.ShowToolbar = false;
			this.m_tmxView.Size = new System.Drawing.Size(284, 396);
			this.m_tmxView.TabIndex = 18;
			this.m_tmxView.UseScreenRatio = false;
			this.m_tmxView.ZapSourceFile = "";
			// 
			// m_ctrlRotateNone
			// 
			this.m_ctrlRotateNone.Location = new System.Drawing.Point(12, 20);
			this.m_ctrlRotateNone.Name = "m_ctrlRotateNone";
			this.m_ctrlRotateNone.Size = new System.Drawing.Size(143, 24);
			this.m_ctrlRotateNone.TabIndex = 0;
			this.m_ctrlRotateNone.Text = "None";
			// 
			// m_ctrlRotateCW
			// 
			this.m_ctrlRotateCW.Location = new System.Drawing.Point(12, 45);
			this.m_ctrlRotateCW.Name = "m_ctrlRotateCW";
			this.m_ctrlRotateCW.Size = new System.Drawing.Size(138, 24);
			this.m_ctrlRotateCW.TabIndex = 1;
			this.m_ctrlRotateCW.Text = "Clockwise";
			// 
			// m_ctrlRotateCCW
			// 
			this.m_ctrlRotateCCW.Location = new System.Drawing.Point(12, 70);
			this.m_ctrlRotateCCW.Name = "m_ctrlRotateCCW";
			this.m_ctrlRotateCCW.Size = new System.Drawing.Size(145, 24);
			this.m_ctrlRotateCCW.TabIndex = 2;
			this.m_ctrlRotateCCW.Text = "Counter Clockwise";
			// 
			// m_ctrlRotateGroup
			// 
			this.m_ctrlRotateGroup.Controls.Add(this.m_ctrlRotateNone);
			this.m_ctrlRotateGroup.Controls.Add(this.m_ctrlRotateCW);
			this.m_ctrlRotateGroup.Controls.Add(this.m_ctrlRotateCCW);
			this.m_ctrlRotateGroup.Location = new System.Drawing.Point(8, 264);
			this.m_ctrlRotateGroup.Name = "m_ctrlRotateGroup";
			this.m_ctrlRotateGroup.Size = new System.Drawing.Size(332, 100);
			this.m_ctrlRotateGroup.TabIndex = 1;
			this.m_ctrlRotateGroup.TabStop = false;
			this.m_ctrlRotateGroup.Text = "Rotation";
			// 
			// CFCleanScanned
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 489);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlRotateGroup);
			this.Controls.Add(this.m_ctrlViewerGroup);
			this.Controls.Add(this.m_ctrlOptionsGroup);
			this.Controls.Add(this.m_ctrlErrorsGroup);
			this.Controls.Add(this.m_ctrlCleaningLabel);
			this.Controls.Add(this.m_ctrlCleaning);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlSelectionsLabel);
			this.Controls.Add(this.m_ctrlSelections);
			this.Controls.Add(this.m_ctrlClean);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFCleanScanned";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Clean Scanned Images";
			this.m_ctrlErrorsGroup.ResumeLayout(false);
			this.m_ctrlOptionsGroup.ResumeLayout(false);
			this.m_ctrlViewerGroup.ResumeLayout(false);
			this.m_ctrlRotateGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method exchanges values between the search options and from controls</summary>
		/// <param name="bSetControls">true if options are to be used to set control values</param>
		/// <returns>true if successful</returns>
		private bool ExchangeOptions(bool bSetControls)
		{
			Debug.Assert(m_tmaxCleanOptions != null);
			if(m_tmaxCleanOptions == null) return false;
			
			try
			{
				//	Are we setting the control states?
				if(bSetControls == true)
				{
					m_ctrlPromptBeforeSave.Checked = m_tmaxCleanOptions.PromptBeforeSave;
					m_ctrlDeskew.Checked = m_tmaxCleanOptions.Deskew;
					m_ctrlDespeckle.Checked = m_tmaxCleanOptions.Despeckle;
					m_ctrlRemoveHolePunches.Checked = m_tmaxCleanOptions.RemoveHolePunches;
					
					m_ctrlHolesLeft.Checked = ((m_tmaxCleanOptions.HolePunchLocations & (int)TmaxHolePunchLocations.Left) != 0);
					m_ctrlHolesRight.Checked = ((m_tmaxCleanOptions.HolePunchLocations & (int)TmaxHolePunchLocations.Right) != 0);
					m_ctrlHolesTop.Checked = ((m_tmaxCleanOptions.HolePunchLocations & (int)TmaxHolePunchLocations.Top) != 0);
					m_ctrlHolesBottom.Checked = ((m_tmaxCleanOptions.HolePunchLocations & (int)TmaxHolePunchLocations.Bottom) != 0);
				
					m_ctrlMinHoleCount.Text = m_tmaxCleanOptions.MinimumHolePunchCount.ToString();
					m_ctrlMaxHoleCount.Text = m_tmaxCleanOptions.MaximumHolePunchCount.ToString();
				
					m_ctrlRotateNone.Checked = (m_tmaxCleanOptions.Rotation == TmaxRotations.None);
					m_ctrlRotateCW.Checked = (m_tmaxCleanOptions.Rotation == TmaxRotations.Clockwise);
					m_ctrlRotateCCW.Checked = (m_tmaxCleanOptions.Rotation == TmaxRotations.CounterClockwise);
				}
				else
				{
					m_tmaxCleanOptions.PromptBeforeSave = m_ctrlPromptBeforeSave.Checked;
					m_tmaxCleanOptions.Deskew = m_ctrlDeskew.Checked;
					m_tmaxCleanOptions.Despeckle = m_ctrlDespeckle.Checked;
					m_tmaxCleanOptions.RemoveHolePunches = m_ctrlRemoveHolePunches.Checked;
					
					m_tmaxCleanOptions.HolePunchLocations = (int)TmaxHolePunchLocations.None;
					if(m_ctrlHolesLeft.Checked == true)
						m_tmaxCleanOptions.HolePunchLocations |= (int)TmaxHolePunchLocations.Left;
					if(m_ctrlHolesRight.Checked == true)
						m_tmaxCleanOptions.HolePunchLocations |= (int)TmaxHolePunchLocations.Right;
					if(m_ctrlHolesBottom.Checked == true)
						m_tmaxCleanOptions.HolePunchLocations |= (int)TmaxHolePunchLocations.Bottom;
					if(m_ctrlHolesTop.Checked == true)
						m_tmaxCleanOptions.HolePunchLocations |= (int)TmaxHolePunchLocations.Top;
						
					try
					{
						m_tmaxCleanOptions.MinimumHolePunchCount = System.Convert.ToInt32(m_ctrlMinHoleCount.Text);
					}
					catch
					{
						m_tmaxCleanOptions.MinimumHolePunchCount = 2;
						m_ctrlMinHoleCount.Text = "2";
					}
					
					try
					{
						m_tmaxCleanOptions.MaximumHolePunchCount = System.Convert.ToInt32(m_ctrlMaxHoleCount.Text);
					}
					catch
					{
						m_tmaxCleanOptions.MaximumHolePunchCount = m_tmaxCleanOptions.MinimumHolePunchCount + 1;
						m_ctrlMaxHoleCount.Text = m_tmaxCleanOptions.MaximumHolePunchCount.ToString();
					}
					
					//	Make sure we have appropriate values if removing hole punches
					if(m_tmaxCleanOptions.RemoveHolePunches == true)
					{
						if(m_tmaxCleanOptions.HolePunchLocations == (int)TmaxHolePunchLocations.None)
						{
							m_tmaxCleanOptions.HolePunchLocations = (int)TmaxHolePunchLocations.Left;
							m_ctrlHolesLeft.Checked = true;
						}
						
						if(m_tmaxCleanOptions.MinimumHolePunchCount < 2)
						{
							m_tmaxCleanOptions.MinimumHolePunchCount = 2;
							m_ctrlMinHoleCount.Text = "2";
						}
						
						if(m_tmaxCleanOptions.MaximumHolePunchCount < m_tmaxCleanOptions.MinimumHolePunchCount)
						{
							m_tmaxCleanOptions.MaximumHolePunchCount = m_tmaxCleanOptions.MinimumHolePunchCount;
							m_ctrlMaxHoleCount.Text = m_tmaxCleanOptions.MaximumHolePunchCount.ToString();
						}
						
					}// if(m_tmaxCleanOptions.RemoveHolePunches == true)
					
					if(m_ctrlRotateCW.Checked == true)
						m_tmaxCleanOptions.Rotation = TmaxRotations.Clockwise;
					else if(m_ctrlRotateCCW.Checked == true)
						m_tmaxCleanOptions.Rotation = TmaxRotations.CounterClockwise;
					else
						m_tmaxCleanOptions.Rotation = TmaxRotations.None;
					
					//	Make sure the user has selected one of the cleaning options
					if((m_tmaxCleanOptions.Deskew == false) &&
					   (m_tmaxCleanOptions.Despeckle == false) &&
					   (m_tmaxCleanOptions.RemoveHolePunches == false) &&
					   (m_tmaxCleanOptions.Rotation == TmaxRotations.None))
					{
						MessageBox.Show("You must select one of the available cleaning or rotation options", "",
										MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
					
				}// if(bSetControls == true)
				
				return true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ExchangeOptions", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_OPTIONS_EX), Ex);
				return false;
			}
			
		}// private void ExchangeOptions(bool bSetControls)
		
		/// <summary>This method traps the event fired when the user clicks on OK</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickClean(object sender, System.EventArgs e)
		{
			//	Get the user defined options
			if(ExchangeOptions(false) == false) return;
						
			//	Set the local flags
			m_bCanceled = false;
			m_bCleaning = true;
			m_bTreatedConfirmed = false;
			m_bRotateTreated = false;
			
			m_ctrlClean.Enabled = false;
			m_ctrlCancel.Text = "C&ancel";
			ResetErrorMessages();
			
			foreach(CTmaxItem O in m_tmaxItems)
			{
				//	Must identify a media record
				if(O.GetMediaRecord() != null)
				{
					//	Has the user canceled?
					if(CheckCanceled() == true)
						break;
					
					//	Clean this record
					if(Clean((CDxMediaRecord)(O.GetMediaRecord())) == false)
						break;
				
				}
			
			}// foreach(CTmaxItem O in m_tmaxItems)
			
			m_tmxView.Unload();
			m_ctrlCleaning.Text = "";
			m_ctrlClean.Enabled = true;
			m_ctrlCancel.Text = "Cl&ose";
			m_bCleaning = false;
			
		}// private void OnClickClean(object sender, System.EventArgs e)
		
		/// <summary>This method will clean the file associated with the specified item</summary>
		/// <param name="dxRecord">The record that owns the file to be cleaned</param>
		///	<returns>True to continue</returns>
		private bool Clean(CDxMediaRecord dxRecord)
		{
			CDxPrimary	dxDocument = null;
			string		strFilename = "";
			
			Debug.Assert(dxRecord != null);

			try
			{
				//	What type of media
				switch(dxRecord.MediaType)
				{
					case TmaxMediaTypes.Document:
					
						dxDocument = (CDxPrimary)dxRecord;
						
						//	Do we need to fill the pages?
						if((dxDocument.Secondaries == null) || (dxDocument.Secondaries.Count == 0))
							dxDocument.Fill();
							
						foreach(CDxSecondary O in dxDocument.Secondaries)
						{
							if(Clean(O) == false)
								return false;
								
							//	Has the user canceled?
							if(CheckCanceled() == true)
								return false;	//	Stop cleaning
						}
							
						//	All pages processed ok
						return true;
						
					case TmaxMediaTypes.Page:
					
						//	Are we rotating the image?
						if(m_tmaxCleanOptions.Rotation != TmaxRotations.None)
						{
							//	Make sure it's OK to rotate
							if(ConfirmRotation((CDxSecondary)dxRecord) == false)
								return true; //	Don't treat this as an error			
						}
			
						//	Get the filename from the database
						strFilename = dxRecord.GetFileSpec();
						
						if((strFilename != null) && (strFilename.Length > 0))
						{
							m_strActiveBarcode = dxRecord.GetBarcode(false);
							
							if(Clean(strFilename) == false)
								return false;
						}
						else
						{
							//	Add an error message
							AddError(dxRecord.GetBarcode(false), "Unable to retrieve valid file path");
						}
						
						return true;
						
					case TmaxMediaTypes.Treatment:
					
						//	Clean the parent page
						if(((CDxTertiary)dxRecord).Secondary != null)
						{
							return Clean(((CDxTertiary)dxRecord).Secondary);
						}
						else
						{
							AddError(dxRecord.GetBarcode(false), "Unable to retrieve parent record");
							return true; //	Don't stop processing
						}
							
					default:
					
						return true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Clean", m_tmaxErrorBuilder.Message(ERROR_CLEAN_EX, dxRecord.GetBarcode(false)), Ex);
				return true; // Don't cancel the operation
			}
			
		}// private bool Clean(CDxMediaRecord dxRecord)
		
		/// <summary>This method will clean the specified file</summary>
		/// <param name="strFilename">The file to be cleaned</param>
		///	<returns>True to continue</returns>
		private bool Clean(string strFilename)
		{
			string	strMsg = "";
			bool	bScanned = false;
			
			Debug.Assert(strFilename != null);
			Debug.Assert(strFilename.Length > 0);

			m_ctrlCleaning.Text = CTmaxToolbox.FitPathToWidth(strFilename.ToLower(), m_ctrlCleaning);
			
			//	Is this a scanned image?
			bScanned = IsScanned(strFilename);
			
			//	Don't bother if this is not a scanned image and not rotating
			if((bScanned == false) && (m_tmaxCleanOptions.Rotation == TmaxRotations.None))
				return true; // Don't stop the operation because of this
				
			//	Make sure this file exists
			if(System.IO.File.Exists(strFilename) == false)
			{
				AddError(m_strActiveBarcode, "Unable to locate " + strFilename);
				return true;
			}
			
			//	Load the file
			if(m_tmxView.View(strFilename, 1) == false)
			{
				AddError(m_strActiveBarcode, "Unable to load " + strFilename + " in the viewer: AxError #" + m_tmxView.AxError.ToString());
				return true;
			}
			
			//	Should we deskew?
			if((bScanned == true) && (m_tmaxCleanOptions.Deskew == true))
				m_tmxView.Deskew();
				
			//	Should we despeckle?
			if((bScanned == true) && (m_tmaxCleanOptions.Despeckle == true))
				m_tmxView.Despeckle();
				
			//	Should we remove the holes?
			if((bScanned == true) && (m_tmaxCleanOptions.RemoveHolePunches == true))
				m_tmxView.RemoveHolePunches(m_tmaxCleanOptions.MinimumHolePunchCount, 
											m_tmaxCleanOptions.MaximumHolePunchCount,
											m_tmaxCleanOptions.HolePunchLocations);
			
			//	Are we supposed to rotate the image?
			if(m_tmaxCleanOptions.Rotation == TmaxRotations.Clockwise)
				m_tmxView.Rotate(true);
			else if(m_tmaxCleanOptions.Rotation == TmaxRotations.CounterClockwise)
				m_tmxView.Rotate(false);
			
			//	Are we supposed to prompt the user?
			if(m_tmaxCleanOptions.PromptBeforeSave == true)
			{
				strMsg = String.Format("Cleaned {0}\n\nDo you want to save the changes?\n\nYes = Save Changes\nNo = Don't Save\nCancel = Cancel the operation", m_strActiveBarcode);
				switch(MessageBox.Show(strMsg, "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
				{
					case DialogResult.Cancel:
					
						return false;
						
					case DialogResult.No:
					
						return true; //	Don't save but continue the operation
				}
				
			}//	if(m_tmaxCleanOptions.PromptBeforeSave == true)
			
			//	Save the file
			if(m_tmxView.Save(strFilename) == false)
			{
				AddError(m_strActiveBarcode, "Unable to save changes to " + strFilename);
			}
			
			return true;
			
		}// private bool Clean(string strFilename)
		
		/// <summary>This method is called to determine if the specified file is a scanned image</summary>
		/// <param name="strFilename">The file to be checked</param>
		///	<returns>True if this is a scanned image</returns>
		private bool IsScanned(string strFilename)
		{
			string strExtension = "";
			
			Debug.Assert(strFilename != null);
			Debug.Assert(strFilename.Length > 0);

			strExtension = System.IO.Path.GetExtension(strFilename);
			
			if((strExtension == null) || (strExtension.Length == 0))
				return false;
				
			if(String.Compare(strExtension, ".tif", true) == 0)
				return true;
			
			if(String.Compare(strExtension, ".tiff", true) == 0)
				return true;

            if (String.Compare(strExtension, ".png", true) == 0)
                return true;

			return false;
			
		}// private bool IsScanned(string strFilename)
			
		/// <summary>This method resets the error messages list box</summary>
		private void ResetErrorMessages()
		{
			//	Clear the existing messages
			if((m_ctrlErrors.Items != null) && (m_ctrlErrors.Items.Count > 0))
				m_ctrlErrors.Items.Clear();
				
			//	Automatically resize the columns to fit the column text
			SuspendLayout();
			m_ctrlErrors.Columns[1].Width = -2;
			m_ctrlErrors.Columns[2].Width = -2;
			ResumeLayout();
		
		}// private void ResetErrorMessages()
		
		/// <summary>This method traps the event fired when the user clicks on Cancel</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			//	Are we cleaning?
			if(m_bCleaning == true)
			{
				//	Cancel the operation
				m_bCanceled = true;
			}
			else
			{
				//	Close the form
				DialogResult = DialogResult.Cancel;
				this.Close();
			}
			
		}// private void OnClickCancel(object sender, System.EventArgs e)
		
		/// <summary>This method traps the event fired when the user clicks on the Remove Hole Punches check box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickRemoveHolePunches(object sender, System.EventArgs e)
		{
			//	Enable/disable the remove hole punch options
			m_ctrlHolesLeft.Enabled = m_ctrlRemoveHolePunches.Checked;
			m_ctrlHolesRight.Enabled = m_ctrlRemoveHolePunches.Checked;
			m_ctrlHolesTop.Enabled = m_ctrlRemoveHolePunches.Checked;
			m_ctrlHolesBottom.Enabled = m_ctrlRemoveHolePunches.Checked;
			m_ctrlMinHoleCountLabel.Enabled = m_ctrlRemoveHolePunches.Checked;
			m_ctrlMinHoleCount.Enabled = m_ctrlRemoveHolePunches.Checked;
			m_ctrlMaxHoleCountLabel.Enabled = m_ctrlRemoveHolePunches.Checked;
			m_ctrlMaxHoleCount.Enabled = m_ctrlRemoveHolePunches.Checked;
			
		}// private void OnClickRemoveHolePunches(object sender, System.EventArgs e)

		/// <summary>Called to prompt for confirmation before rotating a page</summary>
		/// <param name="dxPage">The page to be rotated</param>
		/// <returns>true if successful</returns>
		private bool ConfirmRotation(CDxSecondary dxPage)
		{
			CFGetConfirmation	wndGetConfirmation = null;
			bool				bContinue = true;
			
			Debug.Assert(dxPage != null);
			
			//	It's OK to rotate if the user has not treated this page
			if((dxPage.Tertiaries.Count == 0) && (dxPage.ChildCount == 0))
				return true;
				
			//	Have treated pages already been confirmed by the user?
			if(m_bTreatedConfirmed == true)
				return m_bRotateTreated;

			wndGetConfirmation = new CFGetConfirmation();
			m_tmaxEventSource.Attach(wndGetConfirmation.EventSource);
			wndGetConfirmation.Message = String.Format("{0} has one or more treatments. Do you still want to rotate this page?", dxPage.GetBarcode(false));

			bContinue = (wndGetConfirmation.ShowDialog() == DialogResult.Yes);

			if(wndGetConfirmation.ApplyAll == true)
			{
				m_bTreatedConfirmed = true;
				m_bRotateTreated = bContinue;
			}
			
			return bContinue;
		
		}// private bool ConfirmRotation(CDxSecondary dxPage)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		/// <summary>Items that represent the images to be cleaned</summary>
		public CTmaxItems Items
		{
			get { return m_tmaxItems; }
			set { m_tmaxItems = value; }
		}
		
		/// <summary>User defined cleaning options</summary>
		public CTmaxCleanOptions CleanOptions
		{
			get { return m_tmaxCleanOptions; }
			set { m_tmaxCleanOptions = value; }
			
		}
		
		#endregion Properties
	
	}// public class CFCleanScanned : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Panes
