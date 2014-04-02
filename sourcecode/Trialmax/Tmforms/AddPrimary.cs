using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to provide the values required to add a primary media record</summary>
	public class CFAddPrimary : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Button to cancel the operation</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Button to accept the operation</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Local member bound to MediaId property</summary>
		private string m_strMediaId = "";

		/// <summary>Label for media id edit box</summary>
		private System.Windows.Forms.Label m_ctrlMediaIdLabel;

		/// <summary>Edit box allowing user to specify the media id</summary>
		private System.Windows.Forms.TextBox m_ctrlMediaId;

		/// <summary>Local member bound to MediaType property</summary>
		private TmaxMediaTypes m_eMediaType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;

		/// <summary>Local member bound to AutoId property</summary>
		private long m_lAutoId = 0;

		/// <summary>Local member bound to MediaName property</summary>
		private string m_strMediaName = "";

		/// <summary>Label for media name edit box</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;

		/// <summary>Edit box allowing user to specify the media name</summary>
		private System.Windows.Forms.TextBox m_ctrlName;

		/// <summary>Local member bound to Exhibit property</summary>
		private string m_strExhibit = "";

		/// <summary>Label for exhibit name edit box</summary>
		private System.Windows.Forms.Label m_ctrlExhibitLabel;

		/// <summary>Edit box allowing user to specify the exhibit name</summary>
		private System.Windows.Forms.TextBox m_ctrlExhibit;

		/// <summary>Local member bound to Description property</summary>
		private string m_strDescription = "";

		/// <summary>Label for database id text</summary>
		private System.Windows.Forms.Label m_ctrlDatabaseIdLabel;

		/// <summary>Static text control used to display database id</summary>
		private System.Windows.Forms.Label m_ctrlDatabaseId;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFAddPrimary()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		/// <param name="disposing">true if the object is being disposed</param>
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
		protected void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFAddPrimary));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlMediaId = new System.Windows.Forms.TextBox();
			this.m_ctrlMediaIdLabel = new System.Windows.Forms.Label();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.TextBox();
			this.m_ctrlExhibitLabel = new System.Windows.Forms.Label();
			this.m_ctrlExhibit = new System.Windows.Forms.TextBox();
			this.m_ctrlDatabaseIdLabel = new System.Windows.Forms.Label();
			this.m_ctrlDatabaseId = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(210, 148);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 5;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(86, 148);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 4;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlMediaId
			// 
			this.m_ctrlMediaId.Location = new System.Drawing.Point(88, 40);
			this.m_ctrlMediaId.Name = "m_ctrlMediaId";
			this.m_ctrlMediaId.Size = new System.Drawing.Size(276, 20);
			this.m_ctrlMediaId.TabIndex = 0;
			this.m_ctrlMediaId.Text = "textBox1";
			// 
			// m_ctrlMediaIdLabel
			// 
			this.m_ctrlMediaIdLabel.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlMediaIdLabel.Name = "m_ctrlMediaIdLabel";
			this.m_ctrlMediaIdLabel.Size = new System.Drawing.Size(76, 20);
			this.m_ctrlMediaIdLabel.TabIndex = 13;
			this.m_ctrlMediaIdLabel.Text = "Media Id: ";
			this.m_ctrlMediaIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(76, 20);
			this.m_ctrlNameLabel.TabIndex = 15;
			this.m_ctrlNameLabel.Text = "Name: ";
			this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Location = new System.Drawing.Point(88, 72);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(276, 20);
			this.m_ctrlName.TabIndex = 1;
			this.m_ctrlName.Text = "textBox2";
			// 
			// m_ctrlExhibitLabel
			// 
			this.m_ctrlExhibitLabel.Location = new System.Drawing.Point(8, 104);
			this.m_ctrlExhibitLabel.Name = "m_ctrlExhibitLabel";
			this.m_ctrlExhibitLabel.Size = new System.Drawing.Size(76, 20);
			this.m_ctrlExhibitLabel.TabIndex = 17;
			this.m_ctrlExhibitLabel.Text = "Exhibit: ";
			this.m_ctrlExhibitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlExhibit
			// 
			this.m_ctrlExhibit.Location = new System.Drawing.Point(88, 104);
			this.m_ctrlExhibit.Name = "m_ctrlExhibit";
			this.m_ctrlExhibit.Size = new System.Drawing.Size(276, 20);
			this.m_ctrlExhibit.TabIndex = 2;
			this.m_ctrlExhibit.Text = "textBox1";
			// 
			// m_ctrlDatabaseIdLabel
			// 
			this.m_ctrlDatabaseIdLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlDatabaseIdLabel.Name = "m_ctrlDatabaseIdLabel";
			this.m_ctrlDatabaseIdLabel.Size = new System.Drawing.Size(76, 20);
			this.m_ctrlDatabaseIdLabel.TabIndex = 20;
			this.m_ctrlDatabaseIdLabel.Text = "Database Id: ";
			this.m_ctrlDatabaseIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDatabaseId
			// 
			this.m_ctrlDatabaseId.Location = new System.Drawing.Point(88, 8);
			this.m_ctrlDatabaseId.Name = "m_ctrlDatabaseId";
			this.m_ctrlDatabaseId.Size = new System.Drawing.Size(272, 20);
			this.m_ctrlDatabaseId.TabIndex = 21;
			this.m_ctrlDatabaseId.Text = "0000";
			this.m_ctrlDatabaseId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFAddPrimary
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(370, 183);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlDatabaseId,
																		  this.m_ctrlDatabaseIdLabel,
																		  this.m_ctrlExhibitLabel,
																		  this.m_ctrlExhibit,
																		  this.m_ctrlNameLabel,
																		  this.m_ctrlName,
																		  this.m_ctrlMediaIdLabel,
																		  this.m_ctrlMediaId,
																		  this.m_ctrlCancel,
																		  this.m_ctrlOk});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddPrimary";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Add Primary";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>Traps the event fired when the user clicks on the Ok button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			//	The user must specify a Media Id value
			if(m_ctrlMediaId.Text.Length == 0)
			{
				MessageBox.Show("You must provide a valid media id", "Error", 
								MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			
			//	Update the property values
			m_strMediaId = m_ctrlMediaId.Text;
			m_strMediaName = m_ctrlName.Text;
			m_strExhibit = m_ctrlExhibit.Text;
			//m_strDescription = m_ctrlDescription.Text;
		
			DialogResult = DialogResult.OK;
			Close();
		}
		
		/// <summary>Traps the event fired when the form gets loaded for the first time</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			//	Set the control values
			if(m_strMediaId.Length > 0)
				m_ctrlMediaId.Text = m_strMediaId;
			else
				m_ctrlMediaId.Text = m_lAutoId.ToString();
			
			m_ctrlDatabaseId.Text = m_lAutoId.ToString();
			m_ctrlName.Text = m_strMediaName;
			m_ctrlExhibit.Text = m_strExhibit;
			//m_ctrlDescription.Text = m_strDescription;
			
			if(m_eMediaType != TmaxMediaTypes.Unknown)
				this.Text = ("New Primary " + m_eMediaType.ToString());
		
		}
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>MediaType associated with the new primary record</summary>
		public TmaxMediaTypes MediaType
		{
			get	{	return m_eMediaType;	}
			set	{	m_eMediaType = value;	}
		}
		
		/// <summary>AutoId assigned to the record by the database</summary>
		public long AutoId
		{
			get	{	return m_lAutoId;	}
			set	{	m_lAutoId = value;	}
		}
		
		/// <summary>MediaId to be assigned to the primary media record</summary>
		public string MediaId
		{
			get	{	return m_strMediaId;	}
			set	{	m_strMediaId = value;	}
		}
		
		/// <summary>Name to be assigned to the primary media record</summary>
		public string MediaName
		{
			get	{	return m_strMediaName;	}
			set	{	m_strMediaName = value;	}
		}
		
		/// <summary>Exhibit to be assigned to the primary media record</summary>
		public string Exhibit
		{
			get	{	return m_strExhibit;	}
			set	{	m_strExhibit = value;	}
		}
		
		/// <summary>Description to be assigned to the primary media record</summary>
		public string Description
		{
			get	{	return m_strDescription;	}
			set	{	m_strDescription = value;	}
		}
		
		#endregion Properties
		
	}// public class CFAddPrimary : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Forms
