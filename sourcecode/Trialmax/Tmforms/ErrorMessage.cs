using System;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>
	/// 
	/// </summary>
	public class CFErrorMessage : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Local member accessed by Items property</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorArgs.CErrorItems m_aItems;
			
		/// <summary>Local member accessed by Message property</summary>
		protected string m_strMessage;
			
		/// <summary>Local member accessed by Source property</summary>
		protected string m_strSource;
			
		/// <summary>Local member accessed by Title property</summary>
		protected string m_strTitle = "Error";
			
		/// <summary>Local member accessed by Details property</summary>
		protected string m_strDetails;
		
		/// <summary>Local member accessed by Date property</summary>
		protected string m_strDate;
			
		/// <summary>Local member accessed by Time property</summary>
		protected string m_strTime;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Expand button</summary>
		private System.Windows.Forms.Button m_ctrlExpand;
		
		/// <summary>Form control used to display error message</summary>
		private System.Windows.Forms.RichTextBox m_ctrlMsg;
		
		/// <summary>Local member to keep track of whether or not the form is expanded</summary>
		private bool m_bExpanded = true;
		
		/// <summary>Height of client area when expanded</summary>
		private int m_iExpanded;
		
		private System.Windows.Forms.Label m_ctrlSourceLabel;
		private System.Windows.Forms.Label m_ctrlTimeLabel;
		private System.Windows.Forms.Label m_ctrlSource;
		private System.Windows.Forms.Label m_ctrlTime;
		private System.Windows.Forms.ListView m_ctrlItems;
		private System.Windows.Forms.Label m_ctrlItemsLabel;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colValue;
		private System.Windows.Forms.RichTextBox m_ctrlDetails;
		private System.Windows.Forms.Label m_ctrlDetailsLabel;
		
		/// <summary>Height of client area when collapsed</summary>
		private int m_iCollapsed;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default contructor</summary>
		public CFErrorMessage()
		{
			//	Initialize the child controls
			InitializeComponent();
			
			//	Initialize the control values
			SetControls();
		}

		/// <summary>
		/// This method will update the form's controls using the current property values
		/// </summary>
		public void SetControls()
		{
			ListViewItem lvItem;
			
			this.Text = m_strTitle;
			
			if(m_ctrlTime != null)
				m_ctrlTime.Text = m_strDate + "  " + m_strTime;
				
			if(m_ctrlMsg != null)
				m_ctrlMsg.Text = m_strMessage;
				
			if(m_ctrlSource != null)
				m_ctrlSource.Text = m_strSource;
			
			if(m_ctrlDetails != null)
				m_ctrlDetails.Text = m_strDetails;
			
			//	Populate the list of error items
			m_ctrlItems.Items.Clear();
			if((m_aItems != null) && (m_aItems.Count > 0))
			{
				foreach(CTmaxErrorArgs.CErrorItem objItem in m_aItems)
				{
					lvItem = new ListViewItem();
				
					//	Set the text for each column
					lvItem.Text = objItem.Name;
					lvItem.SubItems.Add(objItem.Value);
					
					//	Add to the list
					m_ctrlItems.Items.Add(lvItem);				
				}
				
				//	Automatically resize the columns to fit the text
				m_ctrlItems.SuspendLayout();
				for(int i = 0; i < m_ctrlItems.Columns.Count; i++)
				{
					if(m_ctrlItems.Columns[i] != null)
						m_ctrlItems.Columns[i].Width = -2;
				}
				m_ctrlItems.ResumeLayout();
			
			}//if((m_aItems != null) && (m_aItems.Count > 0))
				
		}// SetControls()
		
		/// <summary>
		/// This method will update the form's controls using the current property values
		/// </summary>
		public void SetControls(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		{
			if(Args == null) return;
			
			//	Transfer the property values
			m_strTitle   = Args.Title;
			m_strDate    = Args.Date;
			m_strTime    = Args.Time;
			m_strMessage = Args.Message;
			m_strDetails = Args.Details;
			m_strSource  = Args.Source;
			m_aItems     = Args.Items;
			
			//	Append the Exception text to the error message
			if((Args.Exception != null) && (Args.Exception.Length > 0))
			{
				m_strMessage += "\n\nException: ";
				m_strMessage += Args.Exception;
			}
			
			//	Refresh the controls
			SetControls();				
		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFErrorMessage));
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlMsg = new System.Windows.Forms.RichTextBox();
			this.m_ctrlExpand = new System.Windows.Forms.Button();
			this.m_ctrlSourceLabel = new System.Windows.Forms.Label();
			this.m_ctrlTimeLabel = new System.Windows.Forms.Label();
			this.m_ctrlSource = new System.Windows.Forms.Label();
			this.m_ctrlTime = new System.Windows.Forms.Label();
			this.m_ctrlItems = new System.Windows.Forms.ListView();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colValue = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlItemsLabel = new System.Windows.Forms.Label();
			this.m_ctrlDetails = new System.Windows.Forms.RichTextBox();
			this.m_ctrlDetailsLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.Location = new System.Drawing.Point(252, 128);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "OK";
			// 
			// m_ctrlMsg
			// 
			this.m_ctrlMsg.BackColor = System.Drawing.SystemColors.ControlLight;
			this.m_ctrlMsg.Location = new System.Drawing.Point(9, 8);
			this.m_ctrlMsg.Name = "m_ctrlMsg";
			this.m_ctrlMsg.ReadOnly = true;
			this.m_ctrlMsg.Size = new System.Drawing.Size(317, 112);
			this.m_ctrlMsg.TabIndex = 4;
			this.m_ctrlMsg.Text = "richTextBox1";
			// 
			// m_ctrlExpand
			// 
			this.m_ctrlExpand.Location = new System.Drawing.Point(172, 128);
			this.m_ctrlExpand.Name = "m_ctrlExpand";
			this.m_ctrlExpand.TabIndex = 5;
			this.m_ctrlExpand.Text = "Details";
			this.m_ctrlExpand.Click += new System.EventHandler(this.OnClickExpand);
			// 
			// m_ctrlSourceLabel
			// 
			this.m_ctrlSourceLabel.Location = new System.Drawing.Point(8, 152);
			this.m_ctrlSourceLabel.Name = "m_ctrlSourceLabel";
			this.m_ctrlSourceLabel.Size = new System.Drawing.Size(48, 16);
			this.m_ctrlSourceLabel.TabIndex = 6;
			this.m_ctrlSourceLabel.Text = "Source:";
			// 
			// m_ctrlTimeLabel
			// 
			this.m_ctrlTimeLabel.Location = new System.Drawing.Point(8, 136);
			this.m_ctrlTimeLabel.Name = "m_ctrlTimeLabel";
			this.m_ctrlTimeLabel.Size = new System.Drawing.Size(48, 16);
			this.m_ctrlTimeLabel.TabIndex = 7;
			this.m_ctrlTimeLabel.Text = "Time:";
			// 
			// m_ctrlSource
			// 
			this.m_ctrlSource.Location = new System.Drawing.Point(56, 152);
			this.m_ctrlSource.Name = "m_ctrlSource";
			this.m_ctrlSource.Size = new System.Drawing.Size(276, 16);
			this.m_ctrlSource.TabIndex = 8;
			this.m_ctrlSource.Text = "FTI.Trialmax.Database";
			// 
			// m_ctrlTime
			// 
			this.m_ctrlTime.Location = new System.Drawing.Point(56, 136);
			this.m_ctrlTime.Name = "m_ctrlTime";
			this.m_ctrlTime.Size = new System.Drawing.Size(276, 16);
			this.m_ctrlTime.TabIndex = 9;
			this.m_ctrlTime.Text = "01-01-2003 12:00:00";
			// 
			// m_ctrlItems
			// 
			this.m_ctrlItems.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.colName,
																						  this.colValue});
			this.m_ctrlItems.FullRowSelect = true;
			this.m_ctrlItems.Location = new System.Drawing.Point(9, 292);
			this.m_ctrlItems.Name = "m_ctrlItems";
			this.m_ctrlItems.Size = new System.Drawing.Size(317, 84);
			this.m_ctrlItems.TabIndex = 10;
			this.m_ctrlItems.View = System.Windows.Forms.View.Details;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			// 
			// colValue
			// 
			this.colValue.Text = "Value";
			// 
			// m_ctrlItemsLabel
			// 
			this.m_ctrlItemsLabel.Location = new System.Drawing.Point(9, 276);
			this.m_ctrlItemsLabel.Name = "m_ctrlItemsLabel";
			this.m_ctrlItemsLabel.Size = new System.Drawing.Size(120, 16);
			this.m_ctrlItemsLabel.TabIndex = 11;
			this.m_ctrlItemsLabel.Text = "Error Items:";
			// 
			// m_ctrlDetails
			// 
			this.m_ctrlDetails.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlDetails.Location = new System.Drawing.Point(9, 192);
			this.m_ctrlDetails.Name = "m_ctrlDetails";
			this.m_ctrlDetails.ReadOnly = true;
			this.m_ctrlDetails.Size = new System.Drawing.Size(317, 80);
			this.m_ctrlDetails.TabIndex = 13;
			this.m_ctrlDetails.Text = "";
			this.m_ctrlDetails.WordWrap = false;
			// 
			// m_ctrlDetailsLabel
			// 
			this.m_ctrlDetailsLabel.Location = new System.Drawing.Point(9, 176);
			this.m_ctrlDetailsLabel.Name = "m_ctrlDetailsLabel";
			this.m_ctrlDetailsLabel.Size = new System.Drawing.Size(120, 16);
			this.m_ctrlDetailsLabel.TabIndex = 14;
			this.m_ctrlDetailsLabel.Text = "Details:";
			// 
			// CFErrorMessage
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(334, 415);
			this.Controls.Add(this.m_ctrlDetailsLabel);
			this.Controls.Add(this.m_ctrlDetails);
			this.Controls.Add(this.m_ctrlItemsLabel);
			this.Controls.Add(this.m_ctrlItems);
			this.Controls.Add(this.m_ctrlExpand);
			this.Controls.Add(this.m_ctrlMsg);
			this.Controls.Add(this.m_ctrlOk);
			this.Controls.Add(this.m_ctrlTime);
			this.Controls.Add(this.m_ctrlSource);
			this.Controls.Add(this.m_ctrlTimeLabel);
			this.Controls.Add(this.m_ctrlSourceLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFErrorMessage";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
	
		#endregion Private Methods

		
		/// <summary>
		/// This method traps the event fired when the user clicks on the Expand button
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickExpand(object sender, System.EventArgs e)
		{
			// Toggle the view state
			if(m_bExpanded == true)
				Collapse();
			else
				Expand();
		}

		/// <summary>
		/// This method traps the Load event fired when the window gets created
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			//	Get the heights to be used for expanded/collapsed views
			m_iExpanded  = this.ClientSize.Height;
			m_iCollapsed = m_ctrlOk.Bottom + 4;
		
			//	Collapse the initial view
			Collapse();
		}
		
		/// <summary>
		/// This method is called to collapse the form
		/// </summary>
		private void Collapse()
		{
			System.Drawing.Point topLeft = new System.Drawing.Point();
			System.Drawing.Size  sizeClient = new System.Drawing.Size();
			
			//	Resize the client area
			sizeClient.Width  = this.ClientSize.Width;
			sizeClient.Height = m_iCollapsed;
			this.ClientSize   = sizeClient;
			
			//	Move the OK button
			topLeft.X = m_ctrlOk.Left;
			topLeft.Y = m_iCollapsed - m_ctrlOk.Height - 4;
			m_ctrlOk.Location = topLeft;

			//	Move the Expand button
			topLeft.X = m_ctrlExpand.Left;
			topLeft.Y = m_iCollapsed - m_ctrlExpand.Height - 4;
			m_ctrlExpand.Location = topLeft;
			
			m_ctrlExpand.Text = "Details";
			m_ctrlTimeLabel.Visible = false;
			m_ctrlTime.Visible = false;
			m_ctrlSourceLabel.Visible = false;
			m_ctrlSource.Visible = false;
			
			//	Set the flag
			m_bExpanded = false;
		}
		
		/// <summary>
		/// This method is called to expand the form
		/// </summary>
		private void Expand()
		{
			System.Drawing.Point topLeft = new System.Drawing.Point();
			System.Drawing.Size  sizeClient = new System.Drawing.Size();
			
			//	Resize the client area
			sizeClient.Width  = this.ClientSize.Width;
			sizeClient.Height = m_iExpanded;
			this.ClientSize   = sizeClient;
			
			//	Move the OK button
			topLeft.X = m_ctrlOk.Left;
			topLeft.Y = m_iExpanded - m_ctrlOk.Height - 4;
			m_ctrlOk.Location = topLeft;

			//	Move the Expand button
			topLeft.X = m_ctrlExpand.Left;
			topLeft.Y = m_iExpanded - m_ctrlExpand.Height - 4;
			m_ctrlExpand.Location = topLeft;
			
			m_ctrlExpand.Text = "Collapse";
			m_ctrlTimeLabel.Visible = true;
			m_ctrlTime.Visible = true;
			m_ctrlSourceLabel.Visible = true;
			m_ctrlSource.Visible = true;
			
			//	Set the flag
			m_bExpanded = true;
		}
		
		
		#region Properties
		
		/// <summary>
		/// This property contains the form's title
		/// </summary>
		public string Title
		{
			get
			{
				return m_strTitle;
			}
			set
			{
				m_strTitle = value;
			}
		}
		
		/// <summary>
		/// This property contains the error message
		/// </summary>
		public string Message
		{
			get
			{
				return m_strMessage;
			}
			set
			{
				m_strMessage = value;
			}
		}
		
		/// <summary>he property exposes the Date associated with the event</summary>
		public string Date
		{
			get
			{
				return m_strDate;
			}
			set
			{
				m_strDate = value;
			}
			
		} // Date property

		/// <summary>he property exposes the Time associated with the event</summary>
		public string Time
		{
			get
			{
				return m_strTime;
			}
			set
			{
				m_strTime = value;
			}
			
		} // Time property

		/// <summary>This property contains details about the error</summary>
		public string Details
		{
			get
			{
				return m_strDetails;
			}
			set
			{
				m_strDetails = value;
			}
			
		} // Details property
		
		/// <summary>This is the array of items associated with this error</summary>
		public FTI.Shared.Trialmax.CTmaxErrorArgs.CErrorItems Items
		{
			get
			{
				return m_aItems;
			}
			
		} // Items property

		/// <summary>This property exposes the source of the error</summary>
		public string Source
		{
			get
			{
				return m_strSource;
			}
			set
			{
				m_strSource = value;
			}
			
		} // Source property

		#endregion Properties

	}
}
