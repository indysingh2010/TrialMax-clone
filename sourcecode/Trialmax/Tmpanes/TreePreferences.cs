using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// Summary description for TreePreferences.
	/// </summary>
	public class CFTreePreferences : System.Windows.Forms.Form
	{
		#region Local Enumerations
		
		private enum TmaxSortOptions
		{
			Ascending = 0,
			CaseSensitive,
		}
		
		#endregion Local Enumerations
		
		#region Private Members

		/// <summary>Component collection required by designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Image list used for buttons</summary>
		private System.Windows.Forms.ImageList m_ctrlSmallImages;
		
		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Primary text modes group box</summary>
		private System.Windows.Forms.GroupBox m_ctrlPrimaryTextGroup;
		
		/// <summary>Local member bound to PrimaryTextMode property</summary>
		private FTI.Shared.Trialmax.TmaxTextModes m_ePrimaryTextMode = TmaxTextModes.MediaId;
		
		/// <summary>Local array of primary text mode selections</summary>
		private ArrayList m_aPrimaryTextModes = new ArrayList();
		
		/// <summary>Primary text modes radio list box</summary>
		private FTI.Trialmax.Controls.CRadioListCtrl m_ctrlPrimaryTextModes;
		
		/// <summary>Secondary text modes group box</summary>
		private System.Windows.Forms.GroupBox m_ctrlSecondaryTextGroup;
		
		/// <summary>Local member bound to SecondaryTextMode property</summary>
		private FTI.Shared.Trialmax.TmaxTextModes m_eSecondaryTextMode = TmaxTextModes.Barcode;
		
		/// <summary>Local array of secondary text mode selections</summary>
		private ArrayList m_aSecondaryTextModes = new ArrayList();
		
		/// <summary>Secondary text modes radio list box</summary>
		private FTI.Trialmax.Controls.CRadioListCtrl m_ctrlSecondaryTextModes;
		
		/// <summary>Tertiary text modes group box</summary>
		private System.Windows.Forms.GroupBox m_ctrlTertiaryTextGroup;
		
		/// <summary>Local member bound to TertiaryTextMode property</summary>
		private FTI.Shared.Trialmax.TmaxTextModes m_eTertiaryTextMode = TmaxTextModes.Barcode;
		
		/// <summary>Local member bound to SuperNodeSize property</summary>
		private int m_iSuperNodeSize = 0;
		
		/// <summary>Local array of tertiary text mode selections</summary>
		private ArrayList m_aTertiaryTextModes = new ArrayList();
		
		/// <summary>Tertiary text modes radio list box</summary>
		private FTI.Trialmax.Controls.CRadioListCtrl m_ctrlTertiaryTextModes;
		
		/// <summary>Quaternary text modes group box</summary>
		private System.Windows.Forms.GroupBox m_ctrlQuaternaryTextGroup;
		
		/// <summary>Local member bound to QuaternaryTextMode property</summary>
		private FTI.Shared.Trialmax.TmaxTextModes m_eQuaternaryTextMode = TmaxTextModes.Barcode;
		
		/// <summary>Local array of tertiary text mode selections</summary>
		private ArrayList m_aQuaternaryTextModes = new ArrayList();
		
		/// <summary>Quaternary text modes radio list box</summary>
		private FTI.Trialmax.Controls.CRadioListCtrl m_ctrlQuaternaryTextModes;

		/// <summary>Primary sort fields group box</summary>
		private System.Windows.Forms.GroupBox m_ctrlPrimarySortFieldsGroup;
		
		/// <summary>Primary sort fields radio list box</summary>
		private FTI.Trialmax.Controls.CRadioListCtrl m_ctrlPrimarySortFields;
		
		/// <summary>Local array of primary sort field selections</summary>
		private ArrayList m_aPrimarySortFields = new ArrayList();
		
		/// <summary>Local array of primary sort options</summary>
		private ArrayList m_aPrimarySortOptions = new ArrayList();
		
		/// <summary>Local member bounded to PrimarySorter property</summary>
		private FTI.Trialmax.Controls.CTmaxBaseTreeSorter m_tmaxPrimarySorter = null;

        /// <summary> Set Binder's Sorting order </summary>
        private FTI.Trialmax.Controls.CTmaxBaseTreeSorter m_tmaxBinderSorter = null;

	    private bool m_isSortEnable = true;

        /// <summary>Local member bound to CaseCodes property</summary>
        private FTI.Shared.Trialmax.CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bounded to Primary sort options group box</summary>
		private System.Windows.Forms.GroupBox m_ctrlPrimarySortOptionsGroup;
		private System.Windows.Forms.TextBox m_ctrlSuperNodeSize;
		private System.Windows.Forms.Label m_ctrlSuperNodeSizeLabel;
		private System.Windows.Forms.GroupBox m_ctrlMiscGroup;

		/// <summary>Primary sort options check list box</summary>
		private System.Windows.Forms.CheckedListBox m_ctrlPrimarySortOptions;
        private CRadioListCtrl m_ctrlFilededSortFields;
        private GroupBox m_ctrlSortFieldedGroup;

		/// <summary>Local member bound to know if this is the virtual tree or physical tree</summary>
		private bool m_bVirtualTree = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFTreePreferences()
		{
			InitializeComponent();
		}

        /// <summary>This method is called when the form is loaded the first time</summary>
		/// <param name="e">system event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            FillCodes();
            base.OnLoad(e);
            
            
        }

	    /// <summary>Called to populate the list of case codes</summary>
        /// <returns>true if successful</returns>
        private bool FillCodes()
        {
            m_ctrlFilededSortFields.DataSource = m_tmaxCaseCodes;
	        return true;

        }// private bool FillCodes()

		/// <summary>This method is called to initialize the form</summary>
		public void Initialize(FTI.Trialmax.Panes.CTreePane tmaxTree)
		{
			int iAdjustment = 0;
			
			//	Copy the values of the host tree
			if(tmaxTree == null) return;
			m_ePrimaryTextMode    = tmaxTree.PrimaryTextMode;
			m_eSecondaryTextMode  = tmaxTree.SecondaryTextMode;
			m_eTertiaryTextMode   = tmaxTree.TertiaryTextMode;
			m_eQuaternaryTextMode = tmaxTree.QuaternaryTextMode;
			m_iSuperNodeSize      = tmaxTree.SuperNodeSize;
            if(tmaxTree.IsBinder)
                m_ctrlMiscGroup.Enabled = false;
		    m_tmaxCaseCodes = tmaxTree.CaseCodes;
			//	Initialize the Primary Text Mode options
			if(m_aPrimaryTextModes != null)
			{
				m_aPrimaryTextModes.Add(new CRadioListItem(TmaxTextModes.MediaId, "Media ID"));
				m_aPrimaryTextModes.Add(new CRadioListItem(TmaxTextModes.Name, "Name"));
				//m_aPrimaryTextModes.Add(new CRadioListItem(TmaxTextModes.Exhibit, "Exhibit"));
			
				//	Assign this array as the data source for the list box
				if(m_ctrlPrimaryTextModes != null)
					m_ctrlPrimaryTextModes.DataSource = m_aPrimaryTextModes;
			}

			//	Initialize the Secondary Text Mode options
			if(m_aSecondaryTextModes != null)
			{
				m_aSecondaryTextModes.Add(new CRadioListItem(TmaxTextModes.DisplayOrder, "Display Order"));
				m_aSecondaryTextModes.Add(new CRadioListItem(TmaxTextModes.Barcode, "Barcode"));
				m_aSecondaryTextModes.Add(new CRadioListItem(TmaxTextModes.Name, "Name"));
				m_aSecondaryTextModes.Add(new CRadioListItem(TmaxTextModes.Filename, "Filename"));
			
				//	Assign this array as the data source for the list box
				if(m_ctrlSecondaryTextModes != null)
					m_ctrlSecondaryTextModes.DataSource = m_aSecondaryTextModes;
			}

			//	Initialize the Tertiary Text Mode options
			if(m_aTertiaryTextModes != null)
			{
				m_aTertiaryTextModes.Add(new CRadioListItem(TmaxTextModes.DisplayOrder, "Display Order"));
				m_aTertiaryTextModes.Add(new CRadioListItem(TmaxTextModes.Barcode, "Barcode"));
				m_aTertiaryTextModes.Add(new CRadioListItem(TmaxTextModes.Name, "Name"));
				m_aTertiaryTextModes.Add(new CRadioListItem(TmaxTextModes.Filename, "Filename"));
			
				//	Assign this array as the data source for the list box
				if(m_ctrlTertiaryTextModes != null)
					m_ctrlTertiaryTextModes.DataSource = m_aTertiaryTextModes;
			}
		
			//	Initialize the Quaternary Text Mode options
			if(m_aQuaternaryTextModes != null)
			{
				m_aQuaternaryTextModes.Add(new CRadioListItem(TmaxTextModes.DisplayOrder, "Display Order"));
				m_aQuaternaryTextModes.Add(new CRadioListItem(TmaxTextModes.Barcode, "Barcode"));
				m_aQuaternaryTextModes.Add(new CRadioListItem(TmaxTextModes.Name, "Name"));
			
				//	Assign this array as the data source for the list box
				if(m_ctrlQuaternaryTextModes != null)
					m_ctrlQuaternaryTextModes.DataSource = m_aQuaternaryTextModes;
			}

            // Fill PrimarySortField/FieldedData ListITem
            if (m_aPrimarySortFields != null)
            {
                m_aPrimarySortFields.Add(new CRadioListItem(TmaxTreeSortFields.Text, "Text"));
                m_aPrimarySortFields.Add(new CRadioListItem(TmaxTreeSortFields.MediaId, "Media ID"));
                m_aPrimarySortFields.Add(new CRadioListItem(TmaxTreeSortFields.MediaName, "Media Name"));
                m_aPrimarySortFields.Add(new CRadioListItem(TmaxTreeSortFields.Created, "Date Created"));
                m_aPrimarySortFields.Add(new CRadioListItem(TmaxTreeSortFields.Modified, "Last Modified"));
                m_aPrimarySortFields.Add(new CRadioListItem(TmaxTreeSortFields.FiledData, "Fielded Data"));

                //	Assign this array as the data source for the list box
                if (m_ctrlPrimarySortFields != null)
                    m_ctrlPrimarySortFields.DataSource = m_aPrimarySortFields;
            }

            //	Initialize the Primary sort fields options
            if (m_aPrimarySortOptions != null)
            {
                m_aPrimarySortOptions.Add(new CRadioListItem(TmaxSortOptions.Ascending, "Ascending"));
                m_aPrimarySortOptions.Add(new CRadioListItem(TmaxSortOptions.CaseSensitive, "Case Sensitive"));

                //	Assign this array as the data source for the list box
                if (m_ctrlPrimarySortOptions != null && m_ctrlPrimarySortOptions.Enabled)
                    m_ctrlPrimarySortOptions.DataSource = m_aPrimarySortOptions;

            }

			//	Is this the physical tree?
			//
			//	NOTE:	The virtual tree does not sort primary records
			//			We can't to a normal == comparison for null because
			//			the sorter overloads the equality operator
            if (m_isSortEnable)
            {
                // binder's individual sort order, not from tmaxTree
                if (ReferenceEquals(m_tmaxBinderSorter, null) == false)
                    m_tmaxPrimarySorter = m_tmaxBinderSorter;
                else
                    m_tmaxPrimarySorter = new CTmaxBaseTreeSorter(tmaxTree.PrimarySorter);
                if (m_tmaxPrimarySorter.CaseCode == null && m_tmaxCaseCodes != null && m_tmaxCaseCodes.Count > 0)
                    m_tmaxPrimarySorter.CaseCode = m_tmaxCaseCodes.Find(m_tmaxPrimarySorter.CaseCodeId);

                //	Initialize the Primary sort fields options
                

            }
            else
            {
                m_bVirtualTree = true;

                if (m_ctrlPrimarySortFieldsGroup != null)
                {
                    m_ctrlPrimarySortFieldsGroup.Enabled = false;
                }
                if (m_ctrlPrimarySortFields != null)
                {
                    m_ctrlPrimarySortFields.Enabled = false;
                }
                if (m_ctrlSortFieldedGroup != null)
                {
                    m_ctrlSortFieldedGroup.Enabled = false;
                }
                if (m_ctrlFilededSortFields != null)
                {
                    m_ctrlFilededSortFields.Enabled = false;
                }

                if (m_ctrlPrimarySortOptions != null)
                {
                    m_ctrlPrimarySortOptions.Enabled = false;
                }
                if (m_ctrlPrimarySortOptionsGroup != null)
                {
                    m_ctrlPrimarySortOptionsGroup.Enabled = false;
                }
                if (m_ctrlSuperNodeSize != null)
                {
                    m_ctrlSuperNodeSize.Enabled = false;
                }
                if (m_ctrlSuperNodeSizeLabel != null)
                {
                    m_ctrlSuperNodeSizeLabel.Enabled = false;
                }
                if (m_ctrlMiscGroup != null)
                {
                    m_ctrlMiscGroup.Enabled = false;
                }

               
            }
	
		}// Initialize()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFTreePreferences));
            this.m_ctrlSmallImages = new System.Windows.Forms.ImageList(this.components);
            this.m_ctrlCancel = new System.Windows.Forms.Button();
            this.m_ctrlOk = new System.Windows.Forms.Button();
            this.m_ctrlPrimaryTextModes = new FTI.Trialmax.Controls.CRadioListCtrl();
            this.m_ctrlPrimaryTextGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlSecondaryTextModes = new FTI.Trialmax.Controls.CRadioListCtrl();
            this.m_ctrlSecondaryTextGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlTertiaryTextModes = new FTI.Trialmax.Controls.CRadioListCtrl();
            this.m_ctrlTertiaryTextGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlPrimarySortFields = new FTI.Trialmax.Controls.CRadioListCtrl();
            this.m_ctrlPrimarySortFieldsGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlQuaternaryTextModes = new FTI.Trialmax.Controls.CRadioListCtrl();
            this.m_ctrlQuaternaryTextGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlPrimarySortOptionsGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlPrimarySortOptions = new System.Windows.Forms.CheckedListBox();
            this.m_ctrlSuperNodeSize = new System.Windows.Forms.TextBox();
            this.m_ctrlSuperNodeSizeLabel = new System.Windows.Forms.Label();
            this.m_ctrlMiscGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlFilededSortFields = new FTI.Trialmax.Controls.CRadioListCtrl();
            this.m_ctrlSortFieldedGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlTertiaryTextGroup.SuspendLayout();
            this.m_ctrlPrimarySortOptionsGroup.SuspendLayout();
            this.m_ctrlMiscGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlSmallImages
            // 
            this.m_ctrlSmallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlSmallImages.ImageStream")));
            this.m_ctrlSmallImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlSmallImages.Images.SetKeyName(0, "");
            this.m_ctrlSmallImages.Images.SetKeyName(1, "");
            // 
            // m_ctrlCancel
            // 
            this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlCancel.ImageList = this.m_ctrlSmallImages;
            this.m_ctrlCancel.Location = new System.Drawing.Point(482, 312);
            this.m_ctrlCancel.Name = "m_ctrlCancel";
            this.m_ctrlCancel.Size = new System.Drawing.Size(71, 23);
            this.m_ctrlCancel.TabIndex = 8;
            this.m_ctrlCancel.Text = "  &Cancel";
            // 
            // m_ctrlOk
            // 
            this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlOk.ImageList = this.m_ctrlSmallImages;
            this.m_ctrlOk.Location = new System.Drawing.Point(407, 312);
            this.m_ctrlOk.Name = "m_ctrlOk";
            this.m_ctrlOk.Size = new System.Drawing.Size(71, 23);
            this.m_ctrlOk.TabIndex = 7;
            this.m_ctrlOk.Text = "&OK";
            this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
            // 
            // m_ctrlPrimaryTextModes
            // 
            this.m_ctrlPrimaryTextModes.CheckOnClick = true;
            this.m_ctrlPrimaryTextModes.IntegralHeight = false;
            this.m_ctrlPrimaryTextModes.Location = new System.Drawing.Point(14, 24);
            this.m_ctrlPrimaryTextModes.Name = "m_ctrlPrimaryTextModes";
            this.m_ctrlPrimaryTextModes.SingleSelection = false;
            this.m_ctrlPrimaryTextModes.Size = new System.Drawing.Size(165, 136);
            this.m_ctrlPrimaryTextModes.TabIndex = 33;
            // 
            // m_ctrlPrimaryTextGroup
            // 
            this.m_ctrlPrimaryTextGroup.Location = new System.Drawing.Point(6, 8);
            this.m_ctrlPrimaryTextGroup.Name = "m_ctrlPrimaryTextGroup";
            this.m_ctrlPrimaryTextGroup.Size = new System.Drawing.Size(179, 160);
            this.m_ctrlPrimaryTextGroup.TabIndex = 34;
            this.m_ctrlPrimaryTextGroup.TabStop = false;
            this.m_ctrlPrimaryTextGroup.Text = "Primary Media Text";
            // 
            // m_ctrlSecondaryTextModes
            // 
            this.m_ctrlSecondaryTextModes.CheckOnClick = true;
            this.m_ctrlSecondaryTextModes.IntegralHeight = false;
            this.m_ctrlSecondaryTextModes.Location = new System.Drawing.Point(195, 24);
            this.m_ctrlSecondaryTextModes.Name = "m_ctrlSecondaryTextModes";
            this.m_ctrlSecondaryTextModes.SingleSelection = false;
            this.m_ctrlSecondaryTextModes.Size = new System.Drawing.Size(165, 136);
            this.m_ctrlSecondaryTextModes.TabIndex = 35;
            // 
            // m_ctrlSecondaryTextGroup
            // 
            this.m_ctrlSecondaryTextGroup.Location = new System.Drawing.Point(189, 8);
            this.m_ctrlSecondaryTextGroup.Name = "m_ctrlSecondaryTextGroup";
            this.m_ctrlSecondaryTextGroup.Size = new System.Drawing.Size(179, 160);
            this.m_ctrlSecondaryTextGroup.TabIndex = 36;
            this.m_ctrlSecondaryTextGroup.TabStop = false;
            this.m_ctrlSecondaryTextGroup.Text = "Secondary Media Text";
            // 
            // m_ctrlTertiaryTextModes
            // 
            this.m_ctrlTertiaryTextModes.CheckOnClick = true;
            this.m_ctrlTertiaryTextModes.IntegralHeight = false;
            this.m_ctrlTertiaryTextModes.Location = new System.Drawing.Point(7, 18);
            this.m_ctrlTertiaryTextModes.Name = "m_ctrlTertiaryTextModes";
            this.m_ctrlTertiaryTextModes.SingleSelection = false;
            this.m_ctrlTertiaryTextModes.Size = new System.Drawing.Size(165, 136);
            this.m_ctrlTertiaryTextModes.TabIndex = 37;
            // 
            // m_ctrlTertiaryTextGroup
            // 
            this.m_ctrlTertiaryTextGroup.Controls.Add(this.m_ctrlTertiaryTextModes);
            this.m_ctrlTertiaryTextGroup.Location = new System.Drawing.Point(375, 8);
            this.m_ctrlTertiaryTextGroup.Name = "m_ctrlTertiaryTextGroup";
            this.m_ctrlTertiaryTextGroup.Size = new System.Drawing.Size(179, 160);
            this.m_ctrlTertiaryTextGroup.TabIndex = 38;
            this.m_ctrlTertiaryTextGroup.TabStop = false;
            this.m_ctrlTertiaryTextGroup.Text = "Tertiary Media Text";
            // 
            // m_ctrlPrimarySortFields
            // 
            this.m_ctrlPrimarySortFields.CheckOnClick = true;
            this.m_ctrlPrimarySortFields.IntegralHeight = false;
            this.m_ctrlPrimarySortFields.Location = new System.Drawing.Point(14, 192);
            this.m_ctrlPrimarySortFields.Name = "m_ctrlPrimarySortFields";
            this.m_ctrlPrimarySortFields.SingleSelection = true;
            this.m_ctrlPrimarySortFields.Size = new System.Drawing.Size(115, 136);
            this.m_ctrlPrimarySortFields.TabIndex = 39;
            this.m_ctrlPrimarySortFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.m_ctrlPrimarySortFields_ItemCheck);
            // 
            // m_ctrlPrimarySortFieldsGroup
            // 
            this.m_ctrlPrimarySortFieldsGroup.Location = new System.Drawing.Point(6, 176);
            this.m_ctrlPrimarySortFieldsGroup.Name = "m_ctrlPrimarySortFieldsGroup";
            this.m_ctrlPrimarySortFieldsGroup.Size = new System.Drawing.Size(130, 160);
            this.m_ctrlPrimarySortFieldsGroup.TabIndex = 40;
            this.m_ctrlPrimarySortFieldsGroup.TabStop = false;
            this.m_ctrlPrimarySortFieldsGroup.Text = "Primary Sort Field";
            // 
            // m_ctrlQuaternaryTextModes
            // 
            this.m_ctrlQuaternaryTextModes.CheckOnClick = true;
            this.m_ctrlQuaternaryTextModes.IntegralHeight = false;
            this.m_ctrlQuaternaryTextModes.Location = new System.Drawing.Point(36, 128);
            this.m_ctrlQuaternaryTextModes.Name = "m_ctrlQuaternaryTextModes";
            this.m_ctrlQuaternaryTextModes.SingleSelection = false;
            this.m_ctrlQuaternaryTextModes.Size = new System.Drawing.Size(28, 24);
            this.m_ctrlQuaternaryTextModes.TabIndex = 41;
            this.m_ctrlQuaternaryTextModes.Visible = false;
            // 
            // m_ctrlQuaternaryTextGroup
            // 
            this.m_ctrlQuaternaryTextGroup.Location = new System.Drawing.Point(36, 143);
            this.m_ctrlQuaternaryTextGroup.Name = "m_ctrlQuaternaryTextGroup";
            this.m_ctrlQuaternaryTextGroup.Size = new System.Drawing.Size(68, 20);
            this.m_ctrlQuaternaryTextGroup.TabIndex = 42;
            this.m_ctrlQuaternaryTextGroup.TabStop = false;
            this.m_ctrlQuaternaryTextGroup.Text = "Quaternary Media Text";
            this.m_ctrlQuaternaryTextGroup.Visible = false;
            // 
            // m_ctrlPrimarySortOptionsGroup
            // 
            this.m_ctrlPrimarySortOptionsGroup.Controls.Add(this.m_ctrlQuaternaryTextModes);
            this.m_ctrlPrimarySortOptionsGroup.Controls.Add(this.m_ctrlQuaternaryTextGroup);
            this.m_ctrlPrimarySortOptionsGroup.Location = new System.Drawing.Point(273, 176);
            this.m_ctrlPrimarySortOptionsGroup.Name = "m_ctrlPrimarySortOptionsGroup";
            this.m_ctrlPrimarySortOptionsGroup.Size = new System.Drawing.Size(130, 160);
            this.m_ctrlPrimarySortOptionsGroup.TabIndex = 44;
            this.m_ctrlPrimarySortOptionsGroup.TabStop = false;
            this.m_ctrlPrimarySortOptionsGroup.Text = "Primary Sort Options";
            // 
            // m_ctrlPrimarySortOptions
            // 
            this.m_ctrlPrimarySortOptions.CheckOnClick = true;
            this.m_ctrlPrimarySortOptions.IntegralHeight = false;
            this.m_ctrlPrimarySortOptions.Location = new System.Drawing.Point(281, 192);
            this.m_ctrlPrimarySortOptions.Name = "m_ctrlPrimarySortOptions";
            this.m_ctrlPrimarySortOptions.Size = new System.Drawing.Size(115, 136);
            this.m_ctrlPrimarySortOptions.TabIndex = 45;
            // 
            // m_ctrlSuperNodeSize
            // 
            this.m_ctrlSuperNodeSize.Location = new System.Drawing.Point(93, 29);
            this.m_ctrlSuperNodeSize.Name = "m_ctrlSuperNodeSize";
            this.m_ctrlSuperNodeSize.Size = new System.Drawing.Size(48, 20);
            this.m_ctrlSuperNodeSize.TabIndex = 46;
            this.m_ctrlSuperNodeSize.WordWrap = false;
            // 
            // m_ctrlSuperNodeSizeLabel
            // 
            this.m_ctrlSuperNodeSizeLabel.Location = new System.Drawing.Point(2, 16);
            this.m_ctrlSuperNodeSizeLabel.Name = "m_ctrlSuperNodeSizeLabel";
            this.m_ctrlSuperNodeSizeLabel.Size = new System.Drawing.Size(85, 45);
            this.m_ctrlSuperNodeSizeLabel.TabIndex = 47;
            this.m_ctrlSuperNodeSizeLabel.Text = "Max Nodes per Super Node:";
            this.m_ctrlSuperNodeSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlMiscGroup
            // 
            this.m_ctrlMiscGroup.Controls.Add(this.m_ctrlSuperNodeSizeLabel);
            this.m_ctrlMiscGroup.Controls.Add(this.m_ctrlSuperNodeSize);
            this.m_ctrlMiscGroup.Location = new System.Drawing.Point(407, 176);
            this.m_ctrlMiscGroup.Name = "m_ctrlMiscGroup";
            this.m_ctrlMiscGroup.Size = new System.Drawing.Size(146, 131);
            this.m_ctrlMiscGroup.TabIndex = 48;
            this.m_ctrlMiscGroup.TabStop = false;
            // 
            // m_ctrlFilededSortFields
            // 
            this.m_ctrlFilededSortFields.CheckOnClick = true;
            this.m_ctrlFilededSortFields.Enabled = false;
            this.m_ctrlFilededSortFields.IntegralHeight = false;
            this.m_ctrlFilededSortFields.Location = new System.Drawing.Point(149, 192);
            this.m_ctrlFilededSortFields.Name = "m_ctrlFilededSortFields";
            this.m_ctrlFilededSortFields.SingleSelection = true;
            this.m_ctrlFilededSortFields.Size = new System.Drawing.Size(115, 136);
            this.m_ctrlFilededSortFields.TabIndex = 49;
            // 
            // m_ctrlSortFieldedGroup
            // 
            this.m_ctrlSortFieldedGroup.Location = new System.Drawing.Point(138, 176);
            this.m_ctrlSortFieldedGroup.Name = "m_ctrlSortFieldedGroup";
            this.m_ctrlSortFieldedGroup.Size = new System.Drawing.Size(130, 160);
            this.m_ctrlSortFieldedGroup.TabIndex = 50;
            this.m_ctrlSortFieldedGroup.TabStop = false;
            this.m_ctrlSortFieldedGroup.Text = "Fielded Data";
            // 
            // CFTreePreferences
            // 
            this.AcceptButton = this.m_ctrlOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_ctrlCancel;
            this.ClientSize = new System.Drawing.Size(563, 351);
            this.Controls.Add(this.m_ctrlFilededSortFields);
            this.Controls.Add(this.m_ctrlSortFieldedGroup);
            this.Controls.Add(this.m_ctrlPrimarySortOptions);
            this.Controls.Add(this.m_ctrlPrimarySortOptionsGroup);
            this.Controls.Add(this.m_ctrlPrimarySortFields);
            this.Controls.Add(this.m_ctrlPrimarySortFieldsGroup);
            this.Controls.Add(this.m_ctrlSecondaryTextModes);
            this.Controls.Add(this.m_ctrlSecondaryTextGroup);
            this.Controls.Add(this.m_ctrlPrimaryTextModes);
            this.Controls.Add(this.m_ctrlPrimaryTextGroup);
            this.Controls.Add(this.m_ctrlCancel);
            this.Controls.Add(this.m_ctrlOk);
            this.Controls.Add(this.m_ctrlTertiaryTextGroup);
            this.Controls.Add(this.m_ctrlMiscGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFTreePreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Tree Preferences";
            this.Load += new System.EventHandler(this.OnLoad);
            this.m_ctrlTertiaryTextGroup.ResumeLayout(false);
            this.m_ctrlPrimarySortOptionsGroup.ResumeLayout(false);
            this.m_ctrlMiscGroup.ResumeLayout(false);
            this.m_ctrlMiscGroup.PerformLayout();
            this.ResumeLayout(false);

		}

        void m_ctrlPrimarySortFields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CRadioListItem cRadioListItem = (CRadioListItem)m_ctrlPrimarySortFields.SelectedItems[0];
            if (e.NewValue.ToString().ToLower().Equals("checked") && (TmaxTreeSortFields)cRadioListItem.Value == TmaxTreeSortFields.FiledData)
            {
                m_ctrlFilededSortFields.Enabled = true;
            }
            else
            {
                // Disable FieldedList and Uncheck all item(s) of the list
                DisableFilededSortList();
            }

        }

        

	    #endregion
		
		/// <summary>
		/// This method is called to set the current selections using the
		/// current tree pane object
		/// </summary>
		protected void SetSelections()
		{
			SetSelection(m_ctrlPrimaryTextModes, m_ePrimaryTextMode);
			SetSelection(m_ctrlSecondaryTextModes, m_eSecondaryTextMode);
			SetSelection(m_ctrlTertiaryTextModes, m_eTertiaryTextMode);
			SetSelection(m_ctrlQuaternaryTextModes, m_eQuaternaryTextMode);
		
			if(m_bVirtualTree == false)
			{
				m_ctrlSuperNodeSize.Text = m_iSuperNodeSize.ToString();
			
				if(ReferenceEquals(m_tmaxPrimarySorter,null) == false)
					SetSelections(m_tmaxPrimarySorter, m_ctrlPrimarySortFields, m_ctrlPrimarySortOptions);
			}
				
		}// SetSelections()
		
		/// <summary>
		/// This method is called to get the current selections and use the
		///	values to update the tree pane properties
		/// </summary>
		protected void GetSelections()
		{
			m_ePrimaryTextMode    = GetSelection(m_ctrlPrimaryTextModes, m_ePrimaryTextMode);
			m_eSecondaryTextMode  = GetSelection(m_ctrlSecondaryTextModes, m_eSecondaryTextMode);
			m_eTertiaryTextMode   = GetSelection(m_ctrlTertiaryTextModes, m_eTertiaryTextMode);
			m_eQuaternaryTextMode = GetSelection(m_ctrlQuaternaryTextModes, m_eQuaternaryTextMode);
		
			if(m_bVirtualTree == false)
			{
				if(m_ctrlSuperNodeSize.Text.Length > 0)
				{
					try
					{
						m_iSuperNodeSize = Int32.Parse(m_ctrlSuperNodeSize.Text);
					}
					catch
					{
					}
				}
				
				if(m_tmaxPrimarySorter != null)
					GetSelections(m_tmaxPrimarySorter, m_ctrlPrimarySortFields, m_ctrlPrimarySortOptions);
			}
				
		}// GetSelections()
		
		/// <summary>
		/// This method is called to set the selection in the specified list box
		/// </summary>
		/// <param name="ctrlListBox">The list box containing the selections</param>
		/// <param name="eMode">The desired selection</param>
		protected void SetSelection(FTI.Trialmax.Controls.CRadioListCtrl ctrlListBox, FTI.Shared.Trialmax.TmaxTextModes eMode)
		{
			int				iIndex;
			CRadioListItem	oItem;

			if((ctrlListBox != null) && (ctrlListBox.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)
				{
					if((oItem = (CRadioListItem)ctrlListBox.Items[iIndex]) != null)
					{
						if((eMode & (TmaxTextModes)oItem.Value) != 0)
						{
							ctrlListBox.SetItemChecked(iIndex, true);
						}
						else
						{
							ctrlListBox.SetItemChecked(iIndex, false);
						}
					}
				
				}// for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)
			
			}// if((ctrlListBox != null) && (ctrlListBox.Items != null))
			
		}// SetSelection()
		
		/// <summary>
		/// This method is called to set the selections in the specified list boxes
		/// </summary>
		/// <param name="tmaxSorter">The sorter object used to set the selections</param>
		/// <param name="ctrlFields">The list box containing the sort fields</param>
		/// <param name="ctrlOptions">The sort options check list control</param>
		protected void SetSelections(FTI.Trialmax.Controls.CTmaxBaseTreeSorter tmaxSorter, FTI.Trialmax.Controls.CRadioListCtrl ctrlFields, System.Windows.Forms.CheckedListBox ctrlOptions)
		{
			int				iIndex;
			CRadioListItem	oItem;

			//	Set the sort field selection
			if((ctrlFields != null) && (ctrlFields.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlFields.Items.Count; iIndex++)
				{
					if((oItem = (CRadioListItem)ctrlFields.Items[iIndex]) != null)
					{
						if((TmaxTreeSortFields)oItem.Value == tmaxSorter.SortBy)
						{
							ctrlFields.SetItemChecked(iIndex, true);
						}
						else
						{
							ctrlFields.SetItemChecked(iIndex, false);
						}
					}
				
				}// for(iIndex = 0; iIndex < ctrlFields.Items.Count; iIndex++)
			
			}// if((ctrlFields != null) && (ctrlFields.Items != null))


            // Set the sort fielded data selection
            SelectSortedFieldedData(tmaxSorter);

			//	Set the options 
			if((ctrlOptions != null) && (ctrlOptions.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlOptions.Items.Count; iIndex++)
				{
					if((oItem = (CRadioListItem)ctrlOptions.Items[iIndex]) != null)
					{
						//	Which option is this?
						switch((TmaxSortOptions)oItem.Value)
						{
							case TmaxSortOptions.Ascending:
							
								ctrlOptions.SetItemChecked(iIndex, tmaxSorter.Ascending);
								break;
								
							case TmaxSortOptions.CaseSensitive:
							
								ctrlOptions.SetItemChecked(iIndex, tmaxSorter.CaseSensitive);
								break;
						}

					}// if((oItem = (CRadioListItem)ctrlOptions.Items[iIndex]) != null)
				
				}// for(iIndex = 0; iIndex < ctrlOptions.Items.Count; iIndex++)
			
			}// if((ctrlOptions != null) && (ctrlOptions.Items != null))
			
		}

	    

// SetSelections()
		
		/// <summary>
		/// This method is called to set the selection in the specified list box
		/// </summary>
		/// <param name="ctrlListBox">The list box containing the selections</param>
		/// <param name="eMode">The desired selection</param>
		protected TmaxTextModes GetSelection(FTI.Trialmax.Controls.CRadioListCtrl ctrlListBox, FTI.Shared.Trialmax.TmaxTextModes eDefault)
		{
			int				iIndex;
			CRadioListItem	oItem;
			TmaxTextModes	tmaxMode = 0;

			if((ctrlListBox != null) && (ctrlListBox.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)
				{
					if((oItem = (CRadioListItem)ctrlListBox.Items[iIndex]) != null)
					{
						if(ctrlListBox.GetItemChecked(iIndex))
						{
							tmaxMode |= (TmaxTextModes)oItem.Value;
						}
					
					}
				
				}// for(iIndex = 0; iIndex < ctrlListBox.Items.Count; iIndex++)
			
				if(tmaxMode == 0)
					tmaxMode = eDefault;
					
				return tmaxMode;
				
			}// if((ctrlListBox != null) && (ctrlListBox.Items != null))
			
			return eDefault;
			
		}// GetSelection(FTI.Trialmax.Controls.CRadioListCtrl ctrlListBox, FTI.Shared.Trialmax.TmaxTextModes eDefault)
		
		/// <summary>
		/// This method is called to set the sorter properties using the current selections in the specified list boxes
		/// </summary>
		/// <param name="tmaxSorter">The sorter object to be updated with the current selections</param>
		/// <param name="ctrlFields">The list box containing the sort fields</param>
		/// <param name="ctrlOptions">The sort options check list control</param>
		protected void GetSelections(FTI.Trialmax.Controls.CTmaxBaseTreeSorter tmaxSorter, FTI.Trialmax.Controls.CRadioListCtrl ctrlFields, System.Windows.Forms.CheckedListBox ctrlOptions)
		{
			int				iIndex;
			CRadioListItem	oItem;

			//	Set the sort field
			if((ctrlFields != null) && (ctrlFields.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlFields.Items.Count; iIndex++)
				{
					if((oItem = (CRadioListItem)ctrlFields.Items[iIndex]) != null)
					{
						if(ctrlFields.GetItemChecked(iIndex))
						{
							tmaxSorter.SortBy = (TmaxTreeSortFields)oItem.Value;
						}
					}
				
				}// for(iIndex = 0; iIndex < ctrlFields.Items.Count; iIndex++)
			
			}// if((ctrlFields != null) && (ctrlFields.Items != null))

            if (tmaxSorter.SortBy == TmaxTreeSortFields.FiledData && m_ctrlFilededSortFields.SelectedItem is CTmaxCaseCode)
            {
                tmaxSorter.CaseCode = (CTmaxCaseCode)m_ctrlFilededSortFields.SelectedItem;
                tmaxSorter.CaseCodeId =tmaxSorter.CaseCode!=null? tmaxSorter.CaseCode.UniqueId:0;
            }
                

			//	Set the sort options
			if((ctrlOptions != null) && (ctrlOptions.Items != null))
			{
				for(iIndex = 0; iIndex < ctrlOptions.Items.Count; iIndex++)
				{
					if((oItem = (CRadioListItem)ctrlOptions.Items[iIndex]) != null)
					{
						//	Which option is this?
						switch((TmaxSortOptions)oItem.Value)
						{
							case TmaxSortOptions.Ascending:
							
								tmaxSorter.Ascending = ctrlOptions.GetItemChecked(iIndex);
								break;
								
							case TmaxSortOptions.CaseSensitive:
							
								tmaxSorter.CaseSensitive = ctrlOptions.GetItemChecked(iIndex);
								break;
								
						}

					}
				
				}// for(iIndex = 0; iIndex < ctrlOptions.Items.Count; iIndex++)
			
			}// if((ctrlOptions != null) && (ctrlOptions.Items != null))
			
		}// GetSelections()
		
		/// <summary>
		/// This method is called when the user clicks on OK
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event argument object</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			//	Update the tree pane properties
			GetSelections();			
		}
		
		/// <summary>
		/// This method is called when the form is loaded for the first time
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event argument object</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			//	Set the control selections
			SetSelections();
		}

		#endregion Protected Methods

        #region Private Methods
        private void DisableFilededSortList()
        {
            m_ctrlFilededSortFields.Enabled = false;
            if (m_ctrlFilededSortFields!=null &&
                m_ctrlFilededSortFields.CheckedItems != null && m_ctrlFilededSortFields.CheckedItems.Count > 0)
            {
                int selectedIndex = m_ctrlFilededSortFields.Items.IndexOf(m_ctrlFilededSortFields.CheckedItems[0]);
                if (selectedIndex > -1 && selectedIndex < m_ctrlFilededSortFields.Items.Count)
                    m_ctrlFilededSortFields.SetItemCheckState(selectedIndex, CheckState.Unchecked);
            }
        }

        private void SelectSortedFieldedData(CTmaxBaseTreeSorter tmaxSorter)
        {
            if (tmaxSorter.SortBy == TmaxTreeSortFields.FiledData)
            {
                if (tmaxSorter != null && tmaxSorter.CaseCode != null)
                {
                    m_ctrlFilededSortFields.Enabled = true;
                    if (m_ctrlFilededSortFields.Items != null && m_ctrlFilededSortFields.Items.Count > 0)
                    {

                        int index = m_tmaxCaseCodes.IndexOf(m_tmaxCaseCodes.Find(tmaxSorter.CaseCode.UniqueId));
                        if (index > -1 && index < m_ctrlFilededSortFields.Items.Count)
                        {
                            m_ctrlFilededSortFields.SetSelected(index, true);
                            m_ctrlFilededSortFields.SetItemChecked(index, true);
                        }
                        else
                        {
                            m_ctrlPrimarySortFields.SetSelected(0, true);
                            m_ctrlPrimarySortFields.SetItemChecked(0, true);
                        }

                    }
                }
                else
                {
                    m_ctrlPrimarySortFields.SetSelected(0, true);
                    m_ctrlPrimarySortFields.SetItemChecked(0, true);
                }

            }
        }
        #endregion

        #region Properties

        /// <summary>current Primary text mode selection</summary>
		public FTI.Shared.Trialmax.TmaxTextModes PrimaryTextMode
		{
			get
			{
				return m_ePrimaryTextMode;
			}
			
		}// PrimaryTextMode property
		
		/// <summary>current Secondary text mode selection</summary>
		public FTI.Shared.Trialmax.TmaxTextModes SecondaryTextMode
		{
			get
			{
				return m_eSecondaryTextMode;
			}
			
		}// SecondaryTextMode property
		
		/// <summary>current Tertiary text mode selection</summary>
		public FTI.Shared.Trialmax.TmaxTextModes TertiaryTextMode
		{
			get
			{
				return m_eTertiaryTextMode;
			}
			
		}// TertiaryTextMode property
		
		/// <summary>current Quaternary text mode selection</summary>
		public FTI.Shared.Trialmax.TmaxTextModes QuaternaryTextMode
		{
			get
			{
				return m_eQuaternaryTextMode;
			}
			
		}// QuaternaryTextMode property
		
		/// <summary>Primary sort options</summary>
		public FTI.Trialmax.Controls.CTmaxBaseTreeSorter PrimarySorter
		{
			get
			{
				return m_tmaxPrimarySorter;
			}

		}// PrimarySorter property

        public FTI.Trialmax.Controls.CTmaxBaseTreeSorter BinderSorter
        {
            set
            {
                 m_tmaxBinderSorter=value;
            }
        }// PrimarySorter property

        public bool IsSortEnable
        {
            set
            {
                m_isSortEnable = value;
            }
        }

		
		/// <summary>Maximum number of child nodes in a super node</summary>
		public int SuperNodeSize
		{
			get
			{
				return m_iSuperNodeSize;
			}
			set
			{
				m_iSuperNodeSize = value;
			}
			
		}//	SuperNodeSize property
			
         
	#endregion Properties
		
	}// class CFTreePreferences()

}// namespace FTI.Trialmax.Panes

