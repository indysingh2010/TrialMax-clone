using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form is used to prompt the user before deleting a record</summary>
	public class CFConfirmMediaDelete : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Yes button</summary>
		private System.Windows.Forms.Button m_ctrlYes;

		/// <summary>No button</summary>
		private System.Windows.Forms.Button m_ctrlNo;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Static text control to display prompt message</summary>
		private System.Windows.Forms.Label m_ctrlMessage;

		/// <summary>Static text control to display prompt question</summary>
		private System.Windows.Forms.Label m_ctrlContinueText;

		/// <summary>Label for barcode text control</summary>
		private System.Windows.Forms.Label m_ctrlBarcodeLabel;

		/// <summary>Static text control to display record barcode</summary>
		private System.Windows.Forms.Label m_ctrlBarcode;

		/// <summary>Static text control to display record name</summary>
		private System.Windows.Forms.Label m_ctrlName;

		/// <summary>Label for record name text control</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;

		/// <summary>Static text control to display record description</summary>
		private System.Windows.Forms.Label m_ctrlDescription;

		/// <summary>Label for record description text control</summary>
		private System.Windows.Forms.Label m_ctrlDescriptionLabel;

		/// <summary>Interface to the record being deleted</summary>
		private FTI.Shared.Trialmax.ITmaxMediaRecord m_tmaxRecord = null;

		/// <summary>List box used to display reference records</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlReferences;

		/// <summary>Yes To All pushbutton</summary>
		private System.Windows.Forms.Button m_ctrlYesToAll;

		/// <summary>Collection of interfaces to records that reference the one being deleted</summary>
		private ArrayList m_aReferences = new ArrayList();

		/// <summary>Static text label for Yes To All warning message</summary>
		private System.Windows.Forms.Label m_ctrlWarningLabel;

		/// <summary>Static text control to display warning message for Yes To All button</summary>
		private System.Windows.Forms.Label m_ctrlWarningMessage;

		/// <summary>Private member bound to ManagerOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxManagerOptions m_tmaxManagerOptions = null;

		/// <summary>Local member bound to YesToAll property</summary>
		private bool m_bYesToAll = false;

		#endregion Private Members
		
		#region Public Methods

		/// <summary>Default constructor</summary>
		public CFConfirmMediaDelete()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		/// <param name="disposing">true if disposing of the form</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				
				if(m_aReferences != null)
				{
					m_aReferences.Clear();
					m_aReferences = null;
				}
			}
			base.Dispose( disposing );
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		protected void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFConfirmMediaDelete));
			this.m_ctrlYes = new System.Windows.Forms.Button();
			this.m_ctrlNo = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.m_ctrlContinueText = new System.Windows.Forms.Label();
			this.m_ctrlBarcodeLabel = new System.Windows.Forms.Label();
			this.m_ctrlBarcode = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.Label();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlDescription = new System.Windows.Forms.Label();
			this.m_ctrlDescriptionLabel = new System.Windows.Forms.Label();
			this.m_ctrlReferences = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.m_ctrlYesToAll = new System.Windows.Forms.Button();
			this.m_ctrlWarningLabel = new System.Windows.Forms.Label();
			this.m_ctrlWarningMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlYes
			// 
			this.m_ctrlYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.m_ctrlYes.Location = new System.Drawing.Point(27, 286);
			this.m_ctrlYes.Name = "m_ctrlYes";
			this.m_ctrlYes.TabIndex = 0;
			this.m_ctrlYes.Text = "&Yes";
			// 
			// m_ctrlNo
			// 
			this.m_ctrlNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNo.Location = new System.Drawing.Point(215, 286);
			this.m_ctrlNo.Name = "m_ctrlNo";
			this.m_ctrlNo.TabIndex = 1;
			this.m_ctrlNo.Text = "&No";
			this.m_ctrlNo.Click += new System.EventHandler(this.OnClickNo);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(309, 286);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMessage.Location = new System.Drawing.Point(7, 8);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.Size = new System.Drawing.Size(386, 12);
			this.m_ctrlMessage.TabIndex = 3;
			this.m_ctrlMessage.Text = "You are about to delete a record";
			this.m_ctrlMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlContinueText
			// 
			this.m_ctrlContinueText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlContinueText.Location = new System.Drawing.Point(7, 80);
			this.m_ctrlContinueText.Name = "m_ctrlContinueText";
			this.m_ctrlContinueText.Size = new System.Drawing.Size(386, 32);
			this.m_ctrlContinueText.TabIndex = 5;
			this.m_ctrlContinueText.Text = "Records shown in this list reference the one you are attempting to delete. If you" +
				" delete this record they will also be deleted.";
			this.m_ctrlContinueText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlBarcodeLabel
			// 
			this.m_ctrlBarcodeLabel.Location = new System.Drawing.Point(7, 28);
			this.m_ctrlBarcodeLabel.Name = "m_ctrlBarcodeLabel";
			this.m_ctrlBarcodeLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlBarcodeLabel.TabIndex = 6;
			this.m_ctrlBarcodeLabel.Text = "Barcode:";
			this.m_ctrlBarcodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlBarcode
			// 
			this.m_ctrlBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBarcode.Location = new System.Drawing.Point(112, 28);
			this.m_ctrlBarcode.Name = "m_ctrlBarcode";
			this.m_ctrlBarcode.Size = new System.Drawing.Size(278, 12);
			this.m_ctrlBarcode.TabIndex = 7;
			this.m_ctrlBarcode.Text = "barcode goes here";
			this.m_ctrlBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlName.Location = new System.Drawing.Point(112, 44);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(278, 12);
			this.m_ctrlName.TabIndex = 9;
			this.m_ctrlName.Text = "name goes here";
			this.m_ctrlName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(7, 44);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlNameLabel.TabIndex = 8;
			this.m_ctrlNameLabel.Text = "Name:";
			this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDescription
			// 
			this.m_ctrlDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlDescription.Location = new System.Drawing.Point(112, 60);
			this.m_ctrlDescription.Name = "m_ctrlDescription";
			this.m_ctrlDescription.Size = new System.Drawing.Size(278, 12);
			this.m_ctrlDescription.TabIndex = 11;
			this.m_ctrlDescription.Text = "description goes here";
			this.m_ctrlDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDescriptionLabel
			// 
			this.m_ctrlDescriptionLabel.Location = new System.Drawing.Point(7, 60);
			this.m_ctrlDescriptionLabel.Name = "m_ctrlDescriptionLabel";
			this.m_ctrlDescriptionLabel.Size = new System.Drawing.Size(100, 12);
			this.m_ctrlDescriptionLabel.TabIndex = 10;
			this.m_ctrlDescriptionLabel.Text = "Description:";
			this.m_ctrlDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlReferences
			// 
			this.m_ctrlReferences.AddTop = false;
			this.m_ctrlReferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlReferences.ClearOnDblClick = false;
			this.m_ctrlReferences.Format = FTI.Trialmax.Controls.TmaxMessageFormats.Record;
			this.m_ctrlReferences.Location = new System.Drawing.Point(7, 112);
			this.m_ctrlReferences.MaxRows = 0;
			this.m_ctrlReferences.Name = "m_ctrlReferences";
			this.m_ctrlReferences.Size = new System.Drawing.Size(386, 132);
			this.m_ctrlReferences.TabIndex = 12;
			// 
			// m_ctrlYesToAll
			// 
			this.m_ctrlYesToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlYesToAll.Location = new System.Drawing.Point(121, 286);
			this.m_ctrlYesToAll.Name = "m_ctrlYesToAll";
			this.m_ctrlYesToAll.TabIndex = 13;
			this.m_ctrlYesToAll.Text = "Yes To &All";
			this.m_ctrlYesToAll.Click += new System.EventHandler(this.OnClickYesToAll);
			// 
			// m_ctrlWarningLabel
			// 
			this.m_ctrlWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlWarningLabel.Location = new System.Drawing.Point(12, 252);
			this.m_ctrlWarningLabel.Name = "m_ctrlWarningLabel";
			this.m_ctrlWarningLabel.Size = new System.Drawing.Size(40, 16);
			this.m_ctrlWarningLabel.TabIndex = 14;
			this.m_ctrlWarningLabel.Text = "NOTE:";
			// 
			// m_ctrlWarningMessage
			// 
			this.m_ctrlWarningMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlWarningMessage.Location = new System.Drawing.Point(56, 252);
			this.m_ctrlWarningMessage.Name = "m_ctrlWarningMessage";
			this.m_ctrlWarningMessage.Size = new System.Drawing.Size(334, 28);
			this.m_ctrlWarningMessage.TabIndex = 15;
			this.m_ctrlWarningMessage.Text = "Yes To All will delete all remaining selections without prompting for confirmatio" +
				"n if not referenced by other records";
			// 
			// CFConfirmMediaDelete
			// 
			this.AcceptButton = this.m_ctrlYes;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(400, 317);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlWarningMessage);
			this.Controls.Add(this.m_ctrlWarningLabel);
			this.Controls.Add(this.m_ctrlYesToAll);
			this.Controls.Add(this.m_ctrlReferences);
			this.Controls.Add(this.m_ctrlDescription);
			this.Controls.Add(this.m_ctrlDescriptionLabel);
			this.Controls.Add(this.m_ctrlName);
			this.Controls.Add(this.m_ctrlNameLabel);
			this.Controls.Add(this.m_ctrlBarcode);
			this.Controls.Add(this.m_ctrlBarcodeLabel);
			this.Controls.Add(this.m_ctrlContinueText);
			this.Controls.Add(this.m_ctrlMessage);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlNo);
			this.Controls.Add(this.m_ctrlYes);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFConfirmMediaDelete";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Confirm Removal";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}

		/// <summary>This method is called when the user clicks on NO</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnClickNo(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();
		}
		
		/// <summary>This method is called when the user clicks on Yes To All</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickYesToAll(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			this.m_bYesToAll = true;
			this.Close();
		}

		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			if(m_tmaxRecord != null)
			{
				m_ctrlMessage.Text = ("You are about to delete this " + m_tmaxRecord.GetMediaType().ToString() + ":");
				m_ctrlBarcode.Text = m_tmaxRecord.GetBarcode(false);
				m_ctrlName.Text = m_tmaxRecord.GetName();
				m_ctrlDescription.Text = m_tmaxRecord.GetDescription();
			}
			
			//	Add the refrences
			foreach(ITmaxMediaRecord O in m_aReferences)
				m_ctrlReferences.Add(O);
				
			//	Set the appropriate warning message for the Yes To All button
			if((m_tmaxManagerOptions != null) && (m_tmaxManagerOptions.ConfirmDeleteReferences == true))
				m_ctrlWarningMessage.Text = "Yes To All will delete all remaining selections without prompting for confirmation if not referenced by other records";
			else
				m_ctrlWarningMessage.Text = "Yes To All will delete all remaining selections and records that reference the selections without prompting for confirmation";

			//	Get their attention
			FTI.Shared.Win32.User.MessageBeep(0);
			
		}// protected void OnLoad(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Properties

		/// <summary>Global TmaxManager application options</summary>
		public CTmaxManagerOptions ManagerOptions
		{
			get { return m_tmaxManagerOptions; }
			set { m_tmaxManagerOptions = value; }
		}

		/// <summary>True to confirm all subsequent deletions</summary>
		public bool YesToAll
		{
			get { return m_bYesToAll;	}
			set { m_bYesToAll = value;	}
			
		}// YesToAll property
			
		/// <summary>Interface to the record being deleted</summary>
		public ITmaxMediaRecord Record
		{
			get { return m_tmaxRecord;	}
			set { m_tmaxRecord = value;	}
			
		}// Record property
			
		/// <summary>Collection of interfaces to records that reference the one being deleted</summary>
		public ArrayList References
		{
			get { return m_aReferences;	}
			
		}// References property
			
		#endregion Properties

	
	}// public class CFConfirmMediaDelete : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms

