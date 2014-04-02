using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Win32;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Panes
{
	/// <summary>This form allows users to add media to a script or binder by specifying barcodes</summary>
	public class CFAddBarcodes : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants
		
		protected const int ERROR_ADD_SCRIPT_EX = (ERROR_TMAX_FORM_MAX + 1);
		protected const int ERROR_ADD_BINDER_EX = (ERROR_TMAX_FORM_MAX + 2);
		protected const int ERROR_IMPORT_EX		= (ERROR_TMAX_FORM_MAX + 3);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Text control to display target media type</summary>
		private System.Windows.Forms.Label m_ctrlTargetType;

		/// <summary>Text control to display target name or barcode</summary>
		private System.Windows.Forms.Label m_ctrlTarget;

		/// <summary>Pushbutton to allow user to request importing of records from file</summary>
		private System.Windows.Forms.Button m_ctrlImport;

		/// <summary>Pushbutton to allow user to request adding the specified barcode</summary>
		private System.Windows.Forms.Button m_ctrlAdd;

		/// <summary>Pushbutton to close the form</summary>
		private System.Windows.Forms.Button m_ctrlDone;
		
		/// <summary>Local member bound to Database property</summary>
		private FTI.Trialmax.Database.CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member bound to Target property</summary>
		private FTI.Trialmax.Database.CDxMediaRecord m_dxTarget = null;
		
		/// <summary>Local member bound to InsertAt property</summary>
		private FTI.Trialmax.Database.CDxMediaRecord m_dxInsertAt = null;
		
		/// <summary>Local member to store the collection of records to be added</summary>
		private FTI.Trialmax.Database.CDxMediaRecords m_dxSource = new CDxMediaRecords();
		
		/// <summary>Local member bound to InsertBefore property</summary>
		private bool m_bInsertBefore = false;
		
		/// <summary>Local member to keep track of import format</summary>
		private TmaxImportFormats m_eImportFormat = TmaxImportFormats.Unknown;
		
		/// <summary>Label for barcode entry edit box</summary>
		private System.Windows.Forms.Label m_ctrlBarcodeLabel;
		
		/// <summary>Edit box to enter a new barcode to be added</summary>
		private System.Windows.Forms.TextBox m_ctrlBarcode;
		
		/// <summary>Image list for form pushbuttons</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFAddBarcodes()
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
				
				if(m_dxSource != null)
				{
					m_dxSource.Clear();
					m_dxSource = null;
				}
				
			}
			base.Dispose( disposing );
		}
		
		/// <summary>Overloaded base class member called when form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if((m_tmaxDatabase != null) && (m_dxTarget != null))
			{
				if(m_dxTarget.GetDataType() == TmaxDataTypes.Binder)
				{
					m_ctrlTargetType.Text = "Binder: ";
					m_ctrlTarget.Text = ((CDxBinderEntry)m_dxTarget).Name;
					m_eImportFormat = TmaxImportFormats.AsciiBinder;
				}
				else
				{
					Debug.Assert(m_dxTarget.MediaType == TmaxMediaTypes.Script);
					
					m_ctrlTargetType.Text = "Script: ";
					m_ctrlTarget.Text = m_dxTarget.GetBarcode(false);
					m_eImportFormat = TmaxImportFormats.AsciiMedia;
				}
				
			}
			else
			{
				m_ctrlTargetType.Text = "";
				m_ctrlTarget.Text = "no target record";
				m_ctrlAdd.Enabled = false;
				m_ctrlImport.Enabled = false;
			}
			
			base.OnLoad (e);
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			//	Add our custom strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding new scenes to the script: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding new binder entries: binder name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the import operation");
			
		}// protected override void SetErrorStrings()

		#endregion Protected Methods

		#region Private Members
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFAddBarcodes));
			this.m_ctrlBarcode = new System.Windows.Forms.TextBox();
			this.m_ctrlImport = new System.Windows.Forms.Button();
			this.m_ctrlBarcodeLabel = new System.Windows.Forms.Label();
			this.m_ctrlTarget = new System.Windows.Forms.Label();
			this.m_ctrlTargetType = new System.Windows.Forms.Label();
			this.m_ctrlDone = new System.Windows.Forms.Button();
			this.m_ctrlAdd = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// m_ctrlBarcode
			// 
			this.m_ctrlBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBarcode.Location = new System.Drawing.Point(80, 36);
			this.m_ctrlBarcode.Name = "m_ctrlBarcode";
			this.m_ctrlBarcode.Size = new System.Drawing.Size(281, 20);
			this.m_ctrlBarcode.TabIndex = 0;
			this.m_ctrlBarcode.WordWrap = false;
			this.m_ctrlBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnBarcodeKeyPress);
			// 
			// m_ctrlImport
			// 
			this.m_ctrlImport.Location = new System.Drawing.Point(80, 65);
			this.m_ctrlImport.Name = "m_ctrlImport";
			this.m_ctrlImport.Size = new System.Drawing.Size(68, 23);
			this.m_ctrlImport.TabIndex = 1;
			this.m_ctrlImport.Text = "&Import";
			this.m_ctrlImport.Click += new System.EventHandler(this.OnClickImport);
			// 
			// m_ctrlBarcodeLabel
			// 
			this.m_ctrlBarcodeLabel.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlBarcodeLabel.Name = "m_ctrlBarcodeLabel";
			this.m_ctrlBarcodeLabel.Size = new System.Drawing.Size(68, 12);
			this.m_ctrlBarcodeLabel.TabIndex = 23;
			this.m_ctrlBarcodeLabel.Text = "Barcode:";
			this.m_ctrlBarcodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTarget
			// 
			this.m_ctrlTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTarget.Location = new System.Drawing.Point(80, 12);
			this.m_ctrlTarget.Name = "m_ctrlTarget";
			this.m_ctrlTarget.Size = new System.Drawing.Size(281, 12);
			this.m_ctrlTarget.TabIndex = 17;
			this.m_ctrlTarget.Text = "target name here";
			this.m_ctrlTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTargetType
			// 
			this.m_ctrlTargetType.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlTargetType.Name = "m_ctrlTargetType";
			this.m_ctrlTargetType.Size = new System.Drawing.Size(68, 12);
			this.m_ctrlTargetType.TabIndex = 16;
			this.m_ctrlTargetType.Text = "Type:";
			this.m_ctrlTargetType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDone
			// 
			this.m_ctrlDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlDone.Location = new System.Drawing.Point(286, 65);
			this.m_ctrlDone.Name = "m_ctrlDone";
			this.m_ctrlDone.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlDone.TabIndex = 3;
			this.m_ctrlDone.Text = "&Done";
			// 
			// m_ctrlAdd
			// 
			this.m_ctrlAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAdd.Location = new System.Drawing.Point(200, 65);
			this.m_ctrlAdd.Name = "m_ctrlAdd";
			this.m_ctrlAdd.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlAdd.TabIndex = 2;
			this.m_ctrlAdd.Text = "&Add";
			this.m_ctrlAdd.Click += new System.EventHandler(this.OnClickAdd);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlImages.Images.SetKeyName(0, "");
			// 
			// CFAddBarcodes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(373, 95);
			this.Controls.Add(this.m_ctrlBarcode);
			this.Controls.Add(this.m_ctrlImport);
			this.Controls.Add(this.m_ctrlBarcodeLabel);
			this.Controls.Add(this.m_ctrlTarget);
			this.Controls.Add(this.m_ctrlTargetType);
			this.Controls.Add(this.m_ctrlDone);
			this.Controls.Add(this.m_ctrlAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddBarcodes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Add Barcodes";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		
		/// <summary>Called when the user clicks on the Add button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickAdd(object sender, System.EventArgs e)
		{
			CDxMediaRecord dxSource = null;
			
			Debug.Assert(m_dxTarget != null);
			Debug.Assert(m_dxSource != null);
			if(m_dxTarget == null) return;
			if(m_dxSource == null) return;
		
			//	Clear the source collection
			m_dxSource.Clear();
			
			//	Has the user provided a barcode?
			if(m_ctrlBarcode.Text.Length == 0)
			{
				MessageBox.Show("You must provide a valid media barcode", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				m_ctrlBarcode.Focus();
				return;
			}
			
			//	Get the source record to be added
			if((dxSource = m_tmaxDatabase.GetRecordFromBarcode(m_ctrlBarcode.Text, true, false)) == null)
			{
				MessageBox.Show(m_ctrlBarcode.Text + " is not a valid barcode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				m_ctrlBarcode.Focus();
				m_ctrlBarcode.SelectAll();
				return;
			}
			
			//	Add to the source collection
			m_dxSource.AddList(dxSource);
			
			//	Add the records
			Add();
			
			m_ctrlBarcode.Focus();
			m_ctrlBarcode.SelectAll();
				
		}// private void OnClickAdd(object sender, System.EventArgs e)

		/// <summary>This method will add the source records to the target</summary>
		private void Add()
		{
			CTmaxItem		tmaxParent = null;
			CTmaxParameters	tmaxParameters = null;
			CDxMediaRecord	dxLastAdded = null;
			CDxMediaRecord	dxAdded = null;
				
			Debug.Assert(m_dxTarget != null);
			Debug.Assert(m_dxSource != null);
			Debug.Assert(m_dxSource.Count > 0);
			if(m_dxTarget == null) return;
			if(m_dxSource == null) return;
			if(m_dxSource.Count == 0) return;
		
			try
			{
				//	Create a parent item to represent the target
				tmaxParent = new CTmaxItem(m_dxTarget);
					
				//	Make sure we have a collection to put the source records in
				if(tmaxParent.SourceItems == null)
					tmaxParent.SourceItems = new CTmaxItems();
					
				//	Add items for each of the source records
				foreach(CDxMediaRecord O in m_dxSource)
					tmaxParent.SourceItems.Add(new CTmaxItem(O));
					
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Activate, true);
						
				//	Are we inserting into the target
				if(m_dxInsertAt != null)
				{
					//	Create the required parameters for the event
					tmaxParameters.Add(TmaxCommandParameters.Before, m_bInsertBefore);
						
					//	Put the insertion point in the subitem collection
					tmaxParent.SubItems.Add(new CTmaxItem(m_dxInsertAt));
				}

				//	Fire the command to add the record
				FireCommand(TmaxCommands.Add, tmaxParent, tmaxParameters);
					
				//	We need to adjust the insertion point if we are inserting
				//	after the specified position
				if((m_dxInsertAt != null) && (m_bInsertBefore == false))
				{
					//	Were any records added?
					if((tmaxParent.ReturnItem != null) && (tmaxParent.ReturnItem.SourceItems != null) &&
					   (tmaxParent.ReturnItem.SourceItems.Count > 0))
					{
						//	Get the last record added that was parented by the active target
						foreach(CTmaxItem O in tmaxParent.ReturnItem.SourceItems)
						{
							//	Is the parent a binder?
							if(m_dxTarget.GetDataType() == TmaxDataTypes.Binder)
							{
								if((dxAdded = (CDxMediaRecord)(O.IBinderEntry)) != null)
								{
									if(ReferenceEquals(dxAdded.GetParent(), m_dxTarget) == true)
										dxLastAdded = dxAdded;
								}
							}
							else
							{
								if((dxAdded = (CDxMediaRecord)(O.GetMediaRecord())) != null)
								{
									if(ReferenceEquals(dxAdded.GetParent(), m_dxTarget) == true)
										dxLastAdded = dxAdded;
								}
							}
							
						}
						
						//	Change the insertion point
						if(dxLastAdded != null)
						{
							m_dxInsertAt = dxLastAdded;
						}
						else
						{
//							MessageBox.Show("NULL");
						}
					
					}// foreach(CTmaxItem O in tmaxParent.ReturnItem.SourceItems)
					
				}
			
			}
			catch(System.Exception Ex)
			{
				if(m_dxTarget.GetDataType() == TmaxDataTypes.Binder)
					m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_BINDER_EX, m_dxTarget.GetName()), Ex);
				else
					m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_SCRIPT_EX, m_dxTarget.GetBarcode(false)), Ex);
			}
					
		}// private void Add()

		/// <summary>Called when the user clicks on the Import button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickImport(object sender, System.EventArgs e)
		{
			CTmaxItem		tmaxTarget = null;
			CTmaxParameters tmaxParameters = null;
			
			try
			{
				tmaxTarget = new CTmaxItem(Target);
				tmaxParameters = new CTmaxParameters();

				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Activate, true));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.MergeImported, true));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Before, InsertBefore));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)m_eImportFormat));
				
				if(m_dxInsertAt != null)
					tmaxTarget.SubItems.Add(new CTmaxItem(m_dxInsertAt));
					
				FireCommand(TmaxCommands.Import, tmaxTarget, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickImport", m_tmaxErrorBuilder.Message(ERROR_IMPORT_EX), Ex);
			}
			
		}// private void OnClickImport(object sender, System.EventArgs e)

		/// <summary>Called when the presses the Enter key in the barcode edit box</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnBarcodeKeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				//	Is the user pressing the Enter key?
				if(e.KeyChar == (char)(Keys.Return))
				{
					//	Make it look like the user clicked on the Add button
					OnClickAdd(m_ctrlAdd, System.EventArgs.Empty);

					//	Mark the keystroke as having been handled
					e.Handled = true;

				}// if(e.KeyChar == (char)(Keys.Return))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnBarcodeKeyPress", Ex);
			}
			
		}// private void OnBarcodeKeyPress(object sender, KeyPressEventArgs e)

		#endregion Private Members

		#region Properties
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		/// <summary>The target record</summary>
		public CDxMediaRecord Target
		{
			get { return m_dxTarget; }
			set { m_dxTarget = value; }
		}
		
		/// <summary>The desired insertion point</summary>
		public CDxMediaRecord InsertAt
		{
			get { return m_dxInsertAt; }
			set { m_dxInsertAt = value; }
		}
		
		/// <summary>True to insert before the specified InsertAt</summary>
		public bool InsertBefore
		{
			get { return m_bInsertBefore; }
			set { m_bInsertBefore = value; }
		}
		
		#endregion Properties

	}// public class CFAddBarcodes : CTmaxBaseForm

}// namespace FTI.Trialmax.Panes
