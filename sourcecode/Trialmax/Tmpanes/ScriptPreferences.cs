using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Panes
{
	/// <summary>Form that allows users to set the script builder options</summary>
	public class CFScriptPreferences : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Local member bound to Columns property</summary>
		private int m_iColumns = 1;
		
		/// <summary>Local member bound to SceneTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eSceneTextMode = TmaxTextModes.Barcode;
		
		/// <summary>Edit box for Columns value</summary>
		private System.Windows.Forms.TextBox m_ctrlColumns;
		
		/// <summary>Static text to label the Columns edit box</summary>
		private System.Windows.Forms.Label m_ctrlColumnsLabel;
		
		/// <summary>List box for selecting SceneTextMode value</summary>
		private FTI.Trialmax.Controls.CRadioListCtrl m_ctrlSceneTextModes;
		
		/// <summary>Group box for SceneTextMode selections</summary>
		private System.Windows.Forms.GroupBox m_ctrlSceneTextGroup;

		/// <summary>List box for selecting StatusTextMode value</summary>
		private FTI.Trialmax.Controls.CRadioListCtrl m_ctrlStatusTextModes;

		/// <summary>Group box for StatusTextMode selections</summary>
		private System.Windows.Forms.GroupBox m_ctrlStatusTextGroup;

		/// <summary>Local member bound to StatusTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eStatusTextMode = TmaxTextModes.Barcode;

		/// <summary>Local array of status text mode selections</summary>
		private ArrayList m_aStatusTextModes = new ArrayList();
		
		/// <summary>Local array of scene text mode selections</summary>
		private ArrayList m_aSceneTextModes = new ArrayList();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFScriptPreferences()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

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

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFScriptPreferences));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlColumns = new System.Windows.Forms.TextBox();
			this.m_ctrlColumnsLabel = new System.Windows.Forms.Label();
			this.m_ctrlSceneTextModes = new FTI.Trialmax.Controls.CRadioListCtrl();
			this.m_ctrlSceneTextGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlStatusTextModes = new FTI.Trialmax.Controls.CRadioListCtrl();
			this.m_ctrlStatusTextGroup = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(212, 208);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 10;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(132, 208);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 9;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlColumns
			// 
			this.m_ctrlColumns.Location = new System.Drawing.Point(76, 172);
			this.m_ctrlColumns.Name = "m_ctrlColumns";
			this.m_ctrlColumns.Size = new System.Drawing.Size(56, 20);
			this.m_ctrlColumns.TabIndex = 11;
			this.m_ctrlColumns.Text = "";
			// 
			// m_ctrlColumnsLabel
			// 
			this.m_ctrlColumnsLabel.Location = new System.Drawing.Point(8, 172);
			this.m_ctrlColumnsLabel.Name = "m_ctrlColumnsLabel";
			this.m_ctrlColumnsLabel.Size = new System.Drawing.Size(68, 20);
			this.m_ctrlColumnsLabel.TabIndex = 12;
			this.m_ctrlColumnsLabel.Text = "Columns: ";
			this.m_ctrlColumnsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSceneTextModes
			// 
			this.m_ctrlSceneTextModes.CheckOnClick = true;
			this.m_ctrlSceneTextModes.IntegralHeight = false;
			this.m_ctrlSceneTextModes.Location = new System.Drawing.Point(156, 24);
			this.m_ctrlSceneTextModes.Name = "m_ctrlSceneTextModes";
			this.m_ctrlSceneTextModes.SingleSelection = false;
			this.m_ctrlSceneTextModes.Size = new System.Drawing.Size(120, 136);
			this.m_ctrlSceneTextModes.TabIndex = 39;
			// 
			// m_ctrlSceneTextGroup
			// 
			this.m_ctrlSceneTextGroup.Location = new System.Drawing.Point(148, 8);
			this.m_ctrlSceneTextGroup.Name = "m_ctrlSceneTextGroup";
			this.m_ctrlSceneTextGroup.Size = new System.Drawing.Size(136, 160);
			this.m_ctrlSceneTextGroup.TabIndex = 40;
			this.m_ctrlSceneTextGroup.TabStop = false;
			this.m_ctrlSceneTextGroup.Text = "Scene Text";
			// 
			// m_ctrlStatusTextModes
			// 
			this.m_ctrlStatusTextModes.CheckOnClick = true;
			this.m_ctrlStatusTextModes.IntegralHeight = false;
			this.m_ctrlStatusTextModes.Location = new System.Drawing.Point(12, 24);
			this.m_ctrlStatusTextModes.Name = "m_ctrlStatusTextModes";
			this.m_ctrlStatusTextModes.SingleSelection = false;
			this.m_ctrlStatusTextModes.Size = new System.Drawing.Size(120, 136);
			this.m_ctrlStatusTextModes.TabIndex = 37;
			// 
			// m_ctrlStatusTextGroup
			// 
			this.m_ctrlStatusTextGroup.Location = new System.Drawing.Point(4, 8);
			this.m_ctrlStatusTextGroup.Name = "m_ctrlStatusTextGroup";
			this.m_ctrlStatusTextGroup.Size = new System.Drawing.Size(136, 160);
			this.m_ctrlStatusTextGroup.TabIndex = 38;
			this.m_ctrlStatusTextGroup.TabStop = false;
			this.m_ctrlStatusTextGroup.Text = "Status Bar Text";
			// 
			// CFScriptPreferences
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 239);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlSceneTextModes,
																		  this.m_ctrlSceneTextGroup,
																		  this.m_ctrlStatusTextModes,
																		  this.m_ctrlStatusTextGroup,
																		  this.m_ctrlColumnsLabel,
																		  this.m_ctrlColumns,
																		  this.m_ctrlCancel,
																		  this.m_ctrlOk});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFScriptPreferences";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Script Builder Preferences";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		#endregion Protected Methods

		/// <summary>
		/// This method is called when the user clicks on OK
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event argument object</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			int iPrevColumns = m_iColumns;
			
			try
			{
				m_iColumns = System.Int32.Parse(m_ctrlColumns.Text.ToString());
			}
			catch
			{
				m_iColumns = iPrevColumns;
			}
			
			m_eStatusTextMode = GetSelection(m_ctrlStatusTextModes, m_eStatusTextMode);
			m_eSceneTextMode = GetSelection(m_ctrlSceneTextModes, m_eSceneTextMode);
		}
		
		/// <summary>This method is called when the form is displayed the first time</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			//	Initialize the Status Text Mode options
			if(m_aStatusTextModes != null)
			{
				m_aStatusTextModes.Add(new CRadioListItem(TmaxTextModes.MediaId, "Media ID"));
				m_aStatusTextModes.Add(new CRadioListItem(TmaxTextModes.Name, "Name"));
				//m_aStatusTextModes.Add(new CRadioListItem(TmaxTextModes.Exhibit, "Exhibit"));
			
				//	Assign this array as the data source for the list box
				if(m_ctrlStatusTextModes != null)
					m_ctrlStatusTextModes.DataSource = m_aStatusTextModes;
			}

			//	Initialize the Scene Text Mode options
			if(m_aSceneTextModes != null)
			{
				m_aSceneTextModes.Add(new CRadioListItem(TmaxTextModes.DisplayOrder, "Display Order"));
				m_aSceneTextModes.Add(new CRadioListItem(TmaxTextModes.Barcode, "Barcode"));
				m_aSceneTextModes.Add(new CRadioListItem(TmaxTextModes.Name, "Name"));
				m_aSceneTextModes.Add(new CRadioListItem(TmaxTextModes.Filename, "Filename"));
			
				//	Assign this array as the data source for the list box
				if(m_ctrlSceneTextModes != null)
					m_ctrlSceneTextModes.DataSource = m_aSceneTextModes;
			}
			
			m_ctrlColumns.Text = m_iColumns.ToString();
			SetSelection(m_ctrlStatusTextModes, m_eStatusTextMode);
			SetSelection(m_ctrlSceneTextModes, m_eSceneTextMode);

		}// private void OnLoad(object sender, System.EventArgs e)
		
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
			
			}// if((ctrlListBox != null) && (ctrlListBox.Items != null))
			
			if(tmaxMode == 0)
				tmaxMode = eDefault;
				
			return tmaxMode;
			
		}// GetSelection(FTI.Trialmax.Controls.CRadioListCtrl ctrlListBox, FTI.Shared.Trialmax.TmaxTextModes eDefault)
		
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
		
		#region Properties
		
		/// <summary>Number of columns of scene rendering controls</summary>
		public int Columns
		{
			get
			{
				return m_iColumns;
			}
			set
			{
				m_iColumns = value;
			}
			
		}// Columns property
		
		/// <summary>Text mode used to display text in each scene's status bar</summary>
		public FTI.Shared.Trialmax.TmaxTextModes SceneTextMode
		{
			get
			{
				return m_eSceneTextMode;
			}
			set
			{
				m_eSceneTextMode = value;

			}
			
		}// SceneTextMode Property
		
		/// <summary>Text mode used to display text in the builder's status bar</summary>
		public FTI.Shared.Trialmax.TmaxTextModes StatusTextMode
		{
			get
			{
				return m_eStatusTextMode;
			}
			set
			{
				m_eStatusTextMode = value;

			}
			
		}// StatusTextMode Property
		
		#endregion Properties

	}// public class CFScriptPreferences

}// namespace FTI.Trialmax.Panes

