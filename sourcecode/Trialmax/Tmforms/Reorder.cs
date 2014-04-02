using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace FTI.Trialmax.Forms
{
	/// <summary>Generic form used to reorder an arraylist of objects</summary>
	public class CFReorder : CFTmaxBaseForm
	{
		#region Private Members

		/// <summary>Form designer component container</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>List box to display caller's objects</summary>
		private System.Windows.Forms.ListBox m_ctrlListBox;

		/// <summary>Control to display user prompt</summary>
		private System.Windows.Forms.Label m_ctrlPrompt;

		/// <summary>Image list used for button images</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;

		/// <summary>Button to move current selection down</summary>
		private System.Windows.Forms.Button m_ctrlMoveDown;

		/// <summary>Button to move current selection up</summary>
		private System.Windows.Forms.Button m_ctrlMoveUp;

		/// <summary>Local member bound to Prompt property</summary>
		private string m_strPrompt = "";
		
		/// <summary>Local member bound to Title property</summary>
		private string m_strTitle = "";
		
		/// <summary>Local member bound to Collection property</summary>
		private ArrayList m_aCollection = null;
		
		/// <summary>Local member bound to Reordered property</summary>
		private ArrayList m_aReordered = null;

		/// <summary>Local member bound to ReorderCollection property</summary>
		private bool m_bReorderCollection = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFReorder() : base()
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

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFReorder));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlListBox = new System.Windows.Forms.ListBox();
			this.m_ctrlPrompt = new System.Windows.Forms.Label();
			this.m_ctrlMoveUp = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlMoveDown = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(236, 212);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(148, 212);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlListBox
			// 
			this.m_ctrlListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlListBox.Location = new System.Drawing.Point(8, 52);
			this.m_ctrlListBox.Name = "m_ctrlListBox";
			this.m_ctrlListBox.Size = new System.Drawing.Size(276, 147);
			this.m_ctrlListBox.TabIndex = 0;
			this.m_ctrlListBox.SelectedIndexChanged += new System.EventHandler(this.OnSelChanged);
			// 
			// m_ctrlPrompt
			// 
			this.m_ctrlPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPrompt.Location = new System.Drawing.Point(12, 8);
			this.m_ctrlPrompt.Name = "m_ctrlPrompt";
			this.m_ctrlPrompt.Size = new System.Drawing.Size(308, 40);
			this.m_ctrlPrompt.TabIndex = 8;
			// 
			// m_ctrlMoveUp
			// 
			this.m_ctrlMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMoveUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlMoveUp.ImageIndex = 1;
			this.m_ctrlMoveUp.ImageList = this.m_ctrlImages;
			this.m_ctrlMoveUp.Location = new System.Drawing.Point(292, 136);
			this.m_ctrlMoveUp.Name = "m_ctrlMoveUp";
			this.m_ctrlMoveUp.Size = new System.Drawing.Size(26, 26);
			this.m_ctrlMoveUp.TabIndex = 1;
			this.m_ctrlMoveUp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlMoveUp.Click += new System.EventHandler(this.OnClickMoveUp);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlMoveDown
			// 
			this.m_ctrlMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMoveDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlMoveDown.ImageIndex = 0;
			this.m_ctrlMoveDown.ImageList = this.m_ctrlImages;
			this.m_ctrlMoveDown.Location = new System.Drawing.Point(292, 172);
			this.m_ctrlMoveDown.Name = "m_ctrlMoveDown";
			this.m_ctrlMoveDown.Size = new System.Drawing.Size(26, 26);
			this.m_ctrlMoveDown.TabIndex = 2;
			this.m_ctrlMoveDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlMoveDown.Click += new System.EventHandler(this.OnClickMoveDown);
			// 
			// CFReorder
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(328, 241);
			this.Controls.Add(this.m_ctrlMoveDown);
			this.Controls.Add(this.m_ctrlMoveUp);
			this.Controls.Add(this.m_ctrlPrompt);
			this.Controls.Add(this.m_ctrlListBox);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFReorder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Reorder";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			m_ctrlPrompt.Text = m_strPrompt;
			this.Text = m_strTitle;
			
			if(m_aCollection != null)
			{
				foreach(object O in m_aCollection)
				{
					m_ctrlListBox.Items.Add(O);
				}
				
				if(m_ctrlListBox.Items.Count > 0)
					m_ctrlListBox.SelectedIndex = 0;
					
				SetButtonStates();

			}
				
		}
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			//	Do we have a valid collection?
			if(m_aCollection != null)
			{
				//	Should we reorder the owner's source collection
				if(m_bReorderCollection == true)
				{
					m_aReordered = m_aCollection;
					m_aCollection.Clear();
				}
				else
				{
					//	Create a new list for the reordered objects
					m_aReordered = new ArrayList();
				}
				
				foreach(object O in m_ctrlListBox.Items)
					m_aReordered.Add(O);
			}
			
				
			DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>This method is called when the user clicks on Move Up</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickMoveUp(object sender, System.EventArgs e)
		{
			int		iIndex = -1;
			object	lbItem = null;
			
			if((iIndex = m_ctrlListBox.SelectedIndex) > 0)
			{
				Debug.Assert(m_ctrlListBox.SelectedItem != null);
				lbItem = m_ctrlListBox.SelectedItem;
				
				m_ctrlListBox.Items.RemoveAt(iIndex);
				m_ctrlListBox.Items.Insert(iIndex - 1, lbItem);
				m_ctrlListBox.SelectedItem = lbItem;
				
				SetButtonStates();
			}
		
		}

		/// <summary>This method is called when the user clicks on Move Down</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickMoveDown(object sender, System.EventArgs e)
		{
			int		iIndex = -1;
			object	lbItem = null;
			
			if((iIndex = m_ctrlListBox.SelectedIndex) < (m_ctrlListBox.Items.Count - 1))
			{
				Debug.Assert(m_ctrlListBox.SelectedItem != null);
				lbItem = m_ctrlListBox.SelectedItem;
				
				m_ctrlListBox.Items.RemoveAt(iIndex);
				m_ctrlListBox.Items.Insert(iIndex + 1, lbItem);
				m_ctrlListBox.SelectedItem = lbItem;
				
				SetButtonStates();
			}
		
		}

		/// <summary>This method is called to enable/disable the Move buttons</summary>
		private void SetButtonStates()
		{
			int iIndex = m_ctrlListBox.SelectedIndex;
			
			m_ctrlMoveUp.Enabled = (iIndex > 0);
			m_ctrlMoveDown.Enabled = (iIndex < (m_ctrlListBox.Items.Count - 1));
		}
		
		/// <summary>This method is called when the user selects a new object in the list box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnSelChanged(object sender, System.EventArgs e)
		{
			SetButtonStates();
		}

		#endregion Private Methods

		#region Properties
		
		/// <summary>Owner's collection of objects to be reordered</summary>
		public ArrayList Collection
		{
			get { return m_aCollection; }
			set { m_aCollection = value; }
		}
		
		/// <summary>Reordered collection of objects</summary>
		public ArrayList Reordered
		{
			get { return m_aReordered; }
			set { m_aReordered = value; }
		}
		
		/// <summary>True to reorder the Collection objects on OK</summary>
		public bool ReorderCollection
		{
			get { return m_bReorderCollection; }
			set { m_bReorderCollection = value; }
		}
		
		/// <summary>Text to be displayed in title bar</summary>
		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value; }
		}
		
		/// <summary>Text to be displayed in prompt control</summary>
		public string Prompt
		{
			get { return m_strPrompt; }
			set { m_strPrompt = value; }
		}
		
		#endregion Properties
		
	}// public class CFReorder : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
