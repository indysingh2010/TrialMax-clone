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

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class manages a form that allows the user to add new video designations</summary>
	public class CFTmaxVideoSetProps : CFTmaxVideoForm
	{
		#region Constants
		
		protected const int ERROR_EXCHANGE_EX	= (ERROR_TMAX_VIDEO_FORM_MAX + 1);
		protected const int ERROR_SET_SOURCE_EX	= (ERROR_TMAX_VIDEO_FORM_MAX + 2);
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Button to add new designations</summary>
		private System.Windows.Forms.Button m_ctrlOK;

		/// <summary>Static text label for script name</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;

		/// <summary>Edit box for script name</summary>
		private System.Windows.Forms.TextBox m_ctrlName;
		
		/// <summary>Button to close the form</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Static text label for source deposition</summary>
		private System.Windows.Forms.Label m_ctrlSourceLabel;
		
		/// <summary>Static text to display name of the source deposition</summary>
		private System.Windows.Forms.Label m_ctrlSourceName;
		
		/// <summary>Image list used for browse button image</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Local member bound to XmlScript property</summary>
		private CXmlScript m_xmlScript = null;
		
		/// <summary>Button to allow the user to get a new source deposition</summary>
		private System.Windows.Forms.Button m_ctrlSetSource;
		
		/// <summary>Local member bound to SetSource property</summary>
		private bool m_bSetSource = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFTmaxVideoSetProps()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Set Script Props";
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

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFTmaxVideoSetProps));
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.TextBox();
			this.m_ctrlSourceLabel = new System.Windows.Forms.Label();
			this.m_ctrlSourceName = new System.Windows.Forms.Label();
			this.m_ctrlSetSource = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.Location = new System.Drawing.Point(208, 80);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 1;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(296, 80);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.TabStop = false;
			this.m_ctrlCancel.Text = "&Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(8, 48);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(101, 20);
			this.m_ctrlNameLabel.TabIndex = 8;
			this.m_ctrlNameLabel.Text = "Script Name:";
			this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlName.Location = new System.Drawing.Point(112, 48);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(272, 20);
			this.m_ctrlName.TabIndex = 0;
			this.m_ctrlName.Text = "";
			// 
			// m_ctrlSourceLabel
			// 
			this.m_ctrlSourceLabel.Location = new System.Drawing.Point(8, 16);
			this.m_ctrlSourceLabel.Name = "m_ctrlSourceLabel";
			this.m_ctrlSourceLabel.Size = new System.Drawing.Size(101, 20);
			this.m_ctrlSourceLabel.TabIndex = 10;
			this.m_ctrlSourceLabel.Text = "Source Deposition:";
			this.m_ctrlSourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSourceName
			// 
			this.m_ctrlSourceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSourceName.Location = new System.Drawing.Point(112, 16);
			this.m_ctrlSourceName.Name = "m_ctrlSourceName";
			this.m_ctrlSourceName.Size = new System.Drawing.Size(248, 20);
			this.m_ctrlSourceName.TabIndex = 12;
			this.m_ctrlSourceName.Text = "source name";
			this.m_ctrlSourceName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSetSource
			// 
			this.m_ctrlSetSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSetSource.ImageIndex = 1;
			this.m_ctrlSetSource.ImageList = this.m_ctrlImages;
			this.m_ctrlSetSource.Location = new System.Drawing.Point(360, 16);
			this.m_ctrlSetSource.Name = "m_ctrlSetSource";
			this.m_ctrlSetSource.Size = new System.Drawing.Size(24, 24);
			this.m_ctrlSetSource.TabIndex = 3;
			this.m_ctrlSetSource.Click += new System.EventHandler(this.OnClickSetSource);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// CFTmaxVideoSetProps
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(394, 111);
			this.Controls.Add(this.m_ctrlSetSource);
			this.Controls.Add(this.m_ctrlSourceName);
			this.Controls.Add(this.m_ctrlName);
			this.Controls.Add(this.m_ctrlSourceLabel);
			this.Controls.Add(this.m_ctrlNameLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFTmaxVideoSetProps";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Set Script Properties";
			this.ResumeLayout(false);

		}

		/// <summary>Called when the form's Load event is trapped</summary>
		/// <param name="e">system event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Initialize the child controls
			Exchange(true);
			
			base.OnLoad (e);

		}// protected void OnLoad(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Private Methods
		
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Do the base class processing first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the property values.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the source deposition.");

		}// protected override void SetErrorStrings()

		/// <summary>This method is called to exchange values between the form control and the class members</summary>
		/// <param name="bSetControls">true to set the control values</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetControls)
		{
			try
			{
				//	Are we setting the child controls?
				if(bSetControls == true)
				{
					if(m_xmlScript != null)
					{
						m_ctrlName.Text = m_xmlScript.Name;
						
						if(m_xmlScript.XmlDeposition != null)
							m_ctrlSourceName.Text = m_xmlScript.XmlDeposition.Name;
					}
						
					//	Are we allowing the user to set the deposition?
					m_ctrlSetSource.Visible = m_bSetSource;
			
				}
				else
				{
					//	User must have a source deposition
					if(m_xmlScript == null)
						return Warn("You must specify a source script or deposition", null);
			
					m_xmlScript.Name = m_ctrlName.Text;
				
				}// if(bSetControls == true)
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX), Ex);
				return false;
			}
						
		}// private bool Exchange(bool bSetControls)
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			if(Exchange(false) == true)
			{			
				DialogResult = DialogResult.OK;
				this.Close();
			}
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>Called when the user clicks on the Done button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		/// <summary>Called when the user clicks on the Set Source button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickSetSource(object sender, System.EventArgs e)
		{
			OpenFileDialog	dlg = null;
			CXmlScript		xmlScript = null;
			
			try
			{
				dlg = new System.Windows.Forms.OpenFileDialog();

				//	Initialize the file selection dialog
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Title = "Open ...";
				dlg.Filter = "Scripts (*.xmls)|*.xmls|Transcripts (*.xmlt)|*.xmlt|All Files (*.*)|*.*";
				dlg.FilterIndex = 1;
			
				//	Set the initial directory
				if(m_xmlScript != null)
					dlg.InitialDirectory = System.IO.Path.GetDirectoryName(m_xmlScript.FileSpec);

				//	Open the dialog box
				if(dlg.ShowDialog() == DialogResult.OK) 
				{
					xmlScript = new CXmlScript();
					
					//	Load this new source
					if(xmlScript.FastFill(dlg.FileName) == true)
					{
						//	Get rid of any designations
						if(xmlScript.XmlDesignations != null)
							xmlScript.XmlDesignations.Clear();
							
						m_xmlScript = xmlScript;
						m_ctrlSourceName.Text = m_xmlScript.XmlDeposition.Name;
					}
					else
					{
						//	Warn the user
						Warn("Unable to load " + dlg.FileName, null);
					}
				
				}// if(dlg.ShowDialog() == DialogResult.OK)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickSetSource", m_tmaxErrorBuilder.Message(ERROR_SET_SOURCE_EX), Ex);
			}
						
		}// private void OnClickSetSource(object sender, System.EventArgs e)

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The active script</summary>
		public CXmlScript XmlScript
		{
			get { return m_xmlScript; }
			set { m_xmlScript = value; }
		}
		
		/// <summary>True to allow user to set the source deposition</summary>
		public bool SetSource
		{
			get { return m_bSetSource; }
			set { m_bSetSource = value; }
		}
		
		#endregion Properties
		
	}// public class CFTmaxVideoSetProps : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.TMVV.Tmvideo
