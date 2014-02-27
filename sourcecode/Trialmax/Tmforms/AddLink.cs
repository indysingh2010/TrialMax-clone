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

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFAddLink : CFTmaxBaseForm
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		private const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 1;
		private const int ERROR_FIRE_VALIDATE_EX = ERROR_TMAX_FORM_MAX + 2;
		private const int ERROR_SET_CONTROL_STATES_EX = ERROR_TMAX_FORM_MAX + 3;

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Check box to allow the user to set the Hide option for the link</summary>
		private CheckBox m_ctrlHideLink;

		/// <summary>Label for the barcode text entry box</summary>
		private Label m_ctrlBarcodeLabel;

		/// <summary>Text box to allow the user to set the link's source barcode</summary>
		private TextBox m_ctrlBarcode;

		/// <summary>Local member bound to XmlLink property</summary>
		private FTI.Shared.Xml.CXmlLink m_xmlLink = null;

		/// <summary>Check box to allow the user to set the Split Screen option for the link</summary>
		private CheckBox m_ctrlSplitScreen;

		/// <summary>Check box to allow the user to set the Hide Text option for the link</summary>
		private CheckBox m_ctrlHideText;

		/// <summary>Check box to allow the user to set the Hide Video for the link</summary>
		private CheckBox m_ctrlHideVideo;

		/// <summary>Static text control to display transcript text</summary>
		private Label m_ctrlPosition;

		/// <summary>Group box for linked media</summary>
		private GroupBox m_ctrlMediaGroup;

		/// <summary>Group box for linked position</summary>
		private GroupBox m_ctrlPositionGroup;

		/// <summary>Group box for link options</summary>
		private GroupBox m_ctrlOptionsGroup;

		/// <summary>Local member bound to XmlTranscript property</summary>
		private FTI.Shared.Xml.CXmlTranscript m_xmlTranscript = null;

		/// <summary>Local member bound to ClassicLinks property</summary>
		private bool m_bClassicLinks = true;

		#endregion Private Members

		#region Public Methods

		/// <summary>This event is fired to validate the barcode supplied by the user</summary>
		public delegate bool ValidateBarcodeHandler(object sender, CXmlLink xmlLink);
		public event ValidateBarcodeHandler ValidateBarcode;		

		/// <summary>Constructor</summary>
		public CFAddLink() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			m_tmaxEventSource.Name = "Add Link";

		}// public CFAddLink() : base()

		#endregion Public Methods

		#region Protected Methods

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Let the base class add its strings first
			base.SetErrorStrings();

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the control values: bSetMembers = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to validate the user selections.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the control states.");

		}// protected override void SetErrorStrings()

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

		}// protected override void Dispose( bool disposing )

		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Owner MUST provide the link to be configured
			Debug.Assert(m_xmlLink != null);

			//	Initialize the control values
			Exchange(false);
			
			//	Set the initial control states
			SetControlStates();
				
			//	Do the base class processing
			base.OnLoad(e);

		}// private void OnLoad(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Private Methods

		/// <summary>This method enables / disables the child controls</summary>
		private void SetControlStates()
		{
			try
			{
				if(m_xmlLink != null)
				{
					m_ctrlOk.Enabled			= true;
					m_ctrlHideLink.Enabled		= true;
					m_ctrlHideVideo.Enabled		= true;
					m_ctrlOptionsGroup.Enabled	= true;
					m_ctrlHideText.Enabled		= (m_bClassicLinks == false);

					m_ctrlBarcode.Enabled = (m_ctrlHideLink.Checked == false);
					m_ctrlBarcodeLabel.Enabled = m_ctrlBarcode.Enabled;
					m_ctrlSplitScreen.Enabled = ((m_ctrlHideLink.Checked == false) && (m_ctrlHideVideo.Checked == false));
				}
				else
				{
					m_ctrlOk.Enabled = false;
					m_ctrlHideLink.Enabled = false;
					m_ctrlBarcode.Enabled = false;
					m_ctrlBarcodeLabel.Enabled = false;
					m_ctrlOptionsGroup.Enabled = false;
					m_ctrlHideText.Enabled = false;
					m_ctrlHideVideo.Enabled = false;
					m_ctrlSplitScreen.Enabled = false;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetControlStates", m_tmaxErrorBuilder.Message(ERROR_SET_CONTROL_STATES_EX), Ex);
			}

		}// private void SetControlStates()

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFAddLink));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlHideLink = new System.Windows.Forms.CheckBox();
			this.m_ctrlBarcodeLabel = new System.Windows.Forms.Label();
			this.m_ctrlBarcode = new System.Windows.Forms.TextBox();
			this.m_ctrlSplitScreen = new System.Windows.Forms.CheckBox();
			this.m_ctrlHideText = new System.Windows.Forms.CheckBox();
			this.m_ctrlHideVideo = new System.Windows.Forms.CheckBox();
			this.m_ctrlPosition = new System.Windows.Forms.Label();
			this.m_ctrlMediaGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlPositionGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlMediaGroup.SuspendLayout();
			this.m_ctrlPositionGroup.SuspendLayout();
			this.m_ctrlOptionsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(271, 227);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "&Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(186, 227);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlHideLink
			// 
			this.m_ctrlHideLink.Location = new System.Drawing.Point(6, 19);
			this.m_ctrlHideLink.Name = "m_ctrlHideLink";
			this.m_ctrlHideLink.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.m_ctrlHideLink.Size = new System.Drawing.Size(77, 22);
			this.m_ctrlHideLink.TabIndex = 0;
			this.m_ctrlHideLink.Text = "Hide";
			this.m_ctrlHideLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlHideLink.UseVisualStyleBackColor = true;
			this.m_ctrlHideLink.Click += new System.EventHandler(this.OnClickHideLink);
			// 
			// m_ctrlBarcodeLabel
			// 
			this.m_ctrlBarcodeLabel.Location = new System.Drawing.Point(8, 46);
			this.m_ctrlBarcodeLabel.Name = "m_ctrlBarcodeLabel";
			this.m_ctrlBarcodeLabel.Size = new System.Drawing.Size(64, 16);
			this.m_ctrlBarcodeLabel.TabIndex = 1;
			this.m_ctrlBarcodeLabel.Text = "Barcode";
			this.m_ctrlBarcodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlBarcode
			// 
			this.m_ctrlBarcode.Location = new System.Drawing.Point(69, 45);
			this.m_ctrlBarcode.Name = "m_ctrlBarcode";
			this.m_ctrlBarcode.Size = new System.Drawing.Size(263, 20);
			this.m_ctrlBarcode.TabIndex = 2;
			// 
			// m_ctrlSplitScreen
			// 
			this.m_ctrlSplitScreen.AutoSize = true;
			this.m_ctrlSplitScreen.Location = new System.Drawing.Point(162, 22);
			this.m_ctrlSplitScreen.Name = "m_ctrlSplitScreen";
			this.m_ctrlSplitScreen.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_ctrlSplitScreen.Size = new System.Drawing.Size(83, 17);
			this.m_ctrlSplitScreen.TabIndex = 2;
			this.m_ctrlSplitScreen.Text = "Split Screen";
			this.m_ctrlSplitScreen.UseVisualStyleBackColor = true;
			// 
			// m_ctrlHideText
			// 
			this.m_ctrlHideText.AutoSize = true;
			this.m_ctrlHideText.Location = new System.Drawing.Point(11, 45);
			this.m_ctrlHideText.Name = "m_ctrlHideText";
			this.m_ctrlHideText.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_ctrlHideText.Size = new System.Drawing.Size(85, 17);
			this.m_ctrlHideText.TabIndex = 1;
			this.m_ctrlHideText.Text = "Disable Text";
			this.m_ctrlHideText.UseVisualStyleBackColor = true;
			// 
			// m_ctrlHideVideo
			// 
			this.m_ctrlHideVideo.AutoSize = true;
			this.m_ctrlHideVideo.Location = new System.Drawing.Point(11, 22);
			this.m_ctrlHideVideo.Name = "m_ctrlHideVideo";
			this.m_ctrlHideVideo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_ctrlHideVideo.Size = new System.Drawing.Size(91, 17);
			this.m_ctrlHideVideo.TabIndex = 0;
			this.m_ctrlHideVideo.Text = "Disable Video";
			this.m_ctrlHideVideo.UseVisualStyleBackColor = true;
			this.m_ctrlHideVideo.Click += new System.EventHandler(this.OnClickHideVideo);
			// 
			// m_ctrlPosition
			// 
			this.m_ctrlPosition.AutoEllipsis = true;
			this.m_ctrlPosition.Location = new System.Drawing.Point(8, 21);
			this.m_ctrlPosition.Name = "m_ctrlPosition";
			this.m_ctrlPosition.Size = new System.Drawing.Size(324, 13);
			this.m_ctrlPosition.TabIndex = 0;
			this.m_ctrlPosition.Text = "8888:88";
			this.m_ctrlPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMediaGroup
			// 
			this.m_ctrlMediaGroup.Controls.Add(this.m_ctrlBarcode);
			this.m_ctrlMediaGroup.Controls.Add(this.m_ctrlHideLink);
			this.m_ctrlMediaGroup.Controls.Add(this.m_ctrlBarcodeLabel);
			this.m_ctrlMediaGroup.Location = new System.Drawing.Point(8, 59);
			this.m_ctrlMediaGroup.Name = "m_ctrlMediaGroup";
			this.m_ctrlMediaGroup.Size = new System.Drawing.Size(338, 76);
			this.m_ctrlMediaGroup.TabIndex = 1;
			this.m_ctrlMediaGroup.TabStop = false;
			this.m_ctrlMediaGroup.Text = "Media";
			// 
			// m_ctrlPositionGroup
			// 
			this.m_ctrlPositionGroup.Controls.Add(this.m_ctrlPosition);
			this.m_ctrlPositionGroup.Location = new System.Drawing.Point(8, 3);
			this.m_ctrlPositionGroup.Name = "m_ctrlPositionGroup";
			this.m_ctrlPositionGroup.Size = new System.Drawing.Size(338, 50);
			this.m_ctrlPositionGroup.TabIndex = 0;
			this.m_ctrlPositionGroup.TabStop = false;
			this.m_ctrlPositionGroup.Text = "Position";
			// 
			// m_ctrlOptionsGroup
			// 
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHideVideo);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSplitScreen);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlHideText);
			this.m_ctrlOptionsGroup.Location = new System.Drawing.Point(8, 141);
			this.m_ctrlOptionsGroup.Name = "m_ctrlOptionsGroup";
			this.m_ctrlOptionsGroup.Size = new System.Drawing.Size(338, 76);
			this.m_ctrlOptionsGroup.TabIndex = 2;
			this.m_ctrlOptionsGroup.TabStop = false;
			this.m_ctrlOptionsGroup.Text = "Options";
			// 
			// CFAddLink
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(355, 261);
			this.Controls.Add(this.m_ctrlOptionsGroup);
			this.Controls.Add(this.m_ctrlPositionGroup);
			this.Controls.Add(this.m_ctrlMediaGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddLink";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Add Link";
			this.m_ctrlMediaGroup.ResumeLayout(false);
			this.m_ctrlMediaGroup.PerformLayout();
			this.m_ctrlPositionGroup.ResumeLayout(false);
			this.m_ctrlOptionsGroup.ResumeLayout(false);
			this.m_ctrlOptionsGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	Get the user selections
			if(Exchange(true) == true)
			{
				//	Make sure the user values are valid
				if(FireValidate() == true)
				{
					DialogResult = DialogResult.OK;
					this.Close();

				}// if(FireValidate() == true)

			}// if(Exchange(true) == true)

		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Hide Link</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickHideLink(object sender, EventArgs e)
		{
			//	Update the control states
			SetControlStates();

		}// private void OnClickHideLink(object sender, EventArgs e)

		/// <summary>This method is called when the user clicks on Hide Video</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickHideVideo(object sender, EventArgs e)
		{
			//	Update the control states
			SetControlStates();

		}// private void OnClickHideVideo(object sender, EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();

		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			bool bSuccessful = false;

			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					if(m_xmlLink != null)
					{
						m_xmlLink.SourceMediaId = m_ctrlBarcode.Text;
						m_xmlLink.Hide = m_ctrlHideLink.Checked;
						m_xmlLink.HideText = m_ctrlHideText.Checked;
						m_xmlLink.HideVideo = m_ctrlHideVideo.Checked;
						m_xmlLink.Split = m_ctrlSplitScreen.Checked;

					}// if(m_xmlLink != null)
					
				}
				else
				{
					if(m_xmlLink != null)
					{
						m_ctrlBarcode.Text = m_xmlLink.SourceMediaId;
						m_ctrlHideLink.Checked = m_xmlLink.Hide;
						m_ctrlHideText.Checked = m_xmlLink.HideText;
						m_ctrlHideVideo.Checked = m_xmlLink.HideVideo;
						m_ctrlSplitScreen.Checked = m_xmlLink.Split;
					}
					else
					{
						m_ctrlBarcode.Text = "";
						m_ctrlHideLink.Checked = false;
						m_ctrlHideText.Checked = false;
						m_ctrlHideVideo.Checked = false;
						m_ctrlSplitScreen.Checked = false;
					}
					
					if(m_xmlTranscript != null)
					{
						m_ctrlPosition.Text = String.Format("{0}  {1}", CTmaxToolbox.PLToString(m_xmlTranscript.PL), m_xmlTranscript.Text);
					}
					else
					{
						m_ctrlPosition.Text = "";
					}

				}// if(bSetMembers == true)

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
			}

			return bSuccessful;

		}// private bool Exchange(bool bSetMembers)

		/// <summary>Called to fire the event to validate the user selections</summary>
		/// <returns>true if the values are valid</returns>
		private bool FireValidate()
		{
			bool bSuccessful = true;

			try
			{
				//	Is this a hidden link?
				if(m_xmlLink.Hide == false)
				{
					//	Must supply a source barcode
					if(m_xmlLink.SourceMediaId.Length > 0)
					{
						//	Fire the event to validate the barcode
						if(ValidateBarcode != null)
							bSuccessful = ValidateBarcode(this, m_xmlLink);
					}
					else
					{
						Warn("You must supply a barcode for the linked media if the link is not hidden", m_ctrlBarcode);
						bSuccessful = false;
					}
				
				}
				else
				{
					//	Don't need a barcode for hidden links
					m_xmlLink.SourceMediaId = "";
					m_xmlLink.SourceDbId = "";

				}// if(m_xmlLink.Hide == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireValidate", m_tmaxErrorBuilder.Message(ERROR_FIRE_VALIDATE_EX), Ex);
			}

			return bSuccessful;

		}// private bool FireValidate()

		#endregion Private Methods

		#region Properties

		/// <summary>The new XML link</summary>
		public FTI.Shared.Xml.CXmlLink XmlLink
		{
			get { return m_xmlLink; }
			set { m_xmlLink = value; }
		}

		/// <summary>The line where the link will be inserted</summary>
		public FTI.Shared.Xml.CXmlTranscript XmlTranscript
		{
			get { return m_xmlTranscript; }
			set { m_xmlTranscript = value; }
		}

		/// <summary>Flag to indicate if system is configured for class video deposition links</summary>
		public bool ClassicLinks
		{
			get { return m_bClassicLinks; }
			set { m_bClassicLinks = value; }
		}

		#endregion Properties

	}// public class CFAddLink : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
