using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class displays a rich text file or resource in a read-only label</summary>
	public class CTmaxRichLabelCtrl : System.Windows.Forms.UserControl
	{
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Control used to display the rich text</summary>
		private System.Windows.Forms.RichTextBox m_ctrlRichText;

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Local member bound to FileStream property</summary>
		private System.IO.Stream m_ioStream = null;

		#endregion Private Members
		
		#region Public Methods

		/// <summary>Constructor</summary>
		public CTmaxRichLabelCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>This method will load the specified file in the rich text control</summary>
		/// <param name="strFileSpec">Fully qualified path to the file to be loaded</param>
		/// <returns>true if successful</returns>
		public bool LoadFromFile(string strFileSpec)
		{
			bool bSuccessful = true;
			
			//	Update the FileSpec property
			if(strFileSpec == null)
				m_strFileSpec = "";
			else
				m_strFileSpec = strFileSpec;

			//	Should we update the text in the control?
			if((m_ctrlRichText != null) && (m_ctrlRichText.IsDisposed == false))
			{
				//	Are we loading a file?
				if((m_strFileSpec.Length >  0) && (System.IO.File.Exists(strFileSpec) == true))
				{
					try
					{
						m_ctrlRichText.LoadFile(m_strFileSpec, RichTextBoxStreamType.RichText);
					}
					catch
					{
						bSuccessful = false;
					}
					
				}
				else
				{
					//	Clear out the control since no file is available
					m_ctrlRichText.Clear();
				}
				
			}// if((m_ctrlRichText != null) && (m_ctrlRichText.IsDisposed == false))
			
			return bSuccessful;
						
		}// public bool Load(string strFileSpec)

		/// <summary>This method will load the specified file in the rich text control</summary>
		/// <param name="strFileSpec">Fully qualified path to the file to be loaded</param>
		/// <returns>true if successful</returns>
		public bool LoadFromStream(System.IO.Stream ioStream)
		{
			bool bSuccessful = true;
			
			//	Update the FileStream property
			m_ioStream = ioStream;

			//	Should we update the text in the control?
			if((m_ctrlRichText != null) && (m_ctrlRichText.IsDisposed == false))
			{
				//	Are we loading a file?
				if(m_ioStream != null)
				{
					try
					{
						m_ctrlRichText.LoadFile(m_ioStream, RichTextBoxStreamType.RichText);
					}
					catch
					{
						bSuccessful = false;
					}
					
				}
				else
				{
					//	Clear out the control since no file is available
					m_ctrlRichText.Clear();
				}
				
			}// if((m_ctrlRichText != null) && (m_ctrlRichText.IsDisposed == false))
			
			return bSuccessful;
						
		}// public bool LoadFromStream(System.IO.Stream ioStream)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Called when the control's window gets created</summary>
		/// <param name="e">the system event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			m_ctrlRichText.BackColor = this.BackColor;
			
			//	Do the base class processing
			base.OnLoad(e);
		}

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
		
		}// protected override void Dispose( bool disposing )
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.m_ctrlRichText = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// m_ctrlRichText
			// 
			this.m_ctrlRichText.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlRichText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_ctrlRichText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlRichText.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlRichText.Name = "m_ctrlRichText";
			this.m_ctrlRichText.ReadOnly = true;
			this.m_ctrlRichText.Size = new System.Drawing.Size(188, 176);
			this.m_ctrlRichText.TabIndex = 0;
			this.m_ctrlRichText.Text = "";
			this.m_ctrlRichText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.OnClickLink);
			// 
			// CTmaxRichLabelCtrl
			// 
			this.Controls.Add(this.m_ctrlRichText);
			this.Name = "CTmaxRichLabelCtrl";
			this.Size = new System.Drawing.Size(188, 176);
			this.BackColorChanged += new System.EventHandler(this.OnBackColorChanged);
			this.ResumeLayout(false);

		}
		
		/// <summary>Handles BackColorChange events fired by the control</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnBackColorChanged(object sender, System.EventArgs e)
		{
			if((m_ctrlRichText != null) && (m_ctrlRichText.IsDisposed == false))
			{
				m_ctrlRichText.BackColor = this.BackColor;
			}
			
		}// private void OnBackColorChanged(object sender, System.EventArgs e)
		
		/// <summary>Handles events fired when the user click on a link embedded in the RTF</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">the sysetm event parameters</param>
		private void OnClickLink(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			try 
			{
				System.Diagnostics.ProcessStartInfo  StartInfo = new System.Diagnostics.ProcessStartInfo(e.LinkText);
				StartInfo.UseShellExecute = true;
				System.Diagnostics.Process.Start(StartInfo);
			} 
			catch
			{
			}
		
		}// private void OnClickLink(object sender, System.Windows.Forms.LinkClickedEventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>Fully qualified path to the RTF file to be displayed in the control</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { LoadFromFile(value); }
		}
		
		/// <summary>File stream used to load the RTF text into the control</summary>
		public System.IO.Stream IOStream
		{
			get { return m_ioStream; }
			set { LoadFromStream(value); }
		}
		
		/// <summary>Border style to be applied to the control</summary>
		[Category("Appearance")]
		new public BorderStyle BorderStyle
		{
			get { return this.m_ctrlRichText.BorderStyle; }
			set { this.m_ctrlRichText.BorderStyle = value; }
		}

		#endregion Properties
		
	}//  public class CTmaxRichLabelCtrl : System.Windows.Forms.UserControl
	
}//  namespace FTI.Trialmax.Controls
