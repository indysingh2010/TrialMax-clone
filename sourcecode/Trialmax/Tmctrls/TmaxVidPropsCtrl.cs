using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Controls
{
	/// <summary>This control allows the user to view and edit designation extents</summary>
	/// </summary>
	public class CTmaxVideoPropsCtrl : CTmaxVideoBaseCtrl
	{
		#region Private Members
		
		/// <summary>Static text control to display media type</summary>
		private System.Windows.Forms.Label m_ctrlMediaType;
		
		/// <summary>Static text control for Filename label</summary>
		private System.Windows.Forms.Label m_ctrlFilenameLabel;
		
		/// <summary>Static text control for Duration label</summary>
		private System.Windows.Forms.Label m_ctrlDurationLabel;
		
		/// <summary>Static text control to display media name</summary>
		private System.Windows.Forms.Label m_ctrlName;
		
		/// <summary>Static text control to display filename</summary>
		private System.Windows.Forms.Label m_ctrlFilename;
		
		/// <summary>Static text control to display duration in seconds</summary>
		private System.Windows.Forms.Label m_ctrlDuration;
		
		/// <summary>Static text control to display stop position</summary>
		private System.Windows.Forms.Label m_ctrlStop;
		
		/// <summary>Static text control to display start position</summary>
		private System.Windows.Forms.Label m_ctrlStart;
		
		/// <summary>Static text control to display stop position label</summary>
		private System.Windows.Forms.Label m_ctrlStopLabel;
		
		/// <summary>Static text control to display start position label</summary>
		private System.Windows.Forms.Label m_ctrlStartLabel;
		
		/// <summary>Pushbutton to invoke the extents editor</summary>
		private System.Windows.Forms.Button m_ctrlEditExtents;
		
		/// <summary>Pushbutton to invoke the text editor</summary>
		private System.Windows.Forms.Button m_ctrlEditText;
		
		/// <summary>Check box to select scrolling text option</summary>
		private System.Windows.Forms.CheckBox m_ctrlScrollText;
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoPropsCtrl() : base()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Video Properties Control";
			
			//	Initialize the child controls
			InitializeComponent();
			
			//	Initilize the controls
			OnAttributesChanged(m_xmlDesignation);
		}

		/// <summary>This method is called to determine if modifications have been made to the active designation</summary>
		/// <param name="xmlDesignation">The active designation</param>
		///	<param name="aModifications">An array in which to put the description of all modifications</param>
		/// <returns>true if modified</returns>
		public override bool IsModified(CXmlDesignation xmlDesignation, ArrayList aModifications)
		{
			bool bModified = true;
			
			if(m_xmlDesignation == null) return false;
			
			Debug.Assert(ReferenceEquals(xmlDesignation, m_xmlDesignation) == true);
			if(ReferenceEquals(xmlDesignation, m_xmlDesignation) == false) return false;
			
			while(bModified == true)
			{
				//	Has scroll text flag changed?
				if(m_xmlDesignation.ScrollText != ScrollText)
				{
					if(aModifications != null)
						aModifications.Add("Designation scroll text option has changed");
						
					break;
				}
					
				//	Nothing changed
				bModified = false;
			}
			
			return bModified;
				
		}// public override bool IsModified(CXmlDesignation xmlDesignation)
			
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
		/// <param name="xmlDesignation">The designation used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		{
			//	Update the property values
			m_strFileSpec = strFileSpec;
			m_xmlDesignation = xmlDesignation;
			
			//	Refresh the controls
			RefreshAll();
			
			return true;
		
		}// public virtual bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to get the derived class property values and use them to set the designation attributes</summary>
		/// <param name="xmlDesignation">The designation to be updated with the current property values</param>
		/// <returns>true if successful</returns>
		public override bool SetAttributes(CXmlDesignation xmlDesignation)
		{
			xmlDesignation.ScrollText = ScrollText;
			return true;
			
		}// protected bool GetModifications(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called when the attributes associated with the active designation have changed</summary>
		/// <param name="xmlDesignation">The designation who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public override bool OnAttributesChanged(CXmlDesignation xmlDesignation)
		{
			Debug.Assert(ReferenceEquals(m_xmlDesignation, xmlDesignation) == true);
			if(ReferenceEquals(m_xmlDesignation, xmlDesignation) == false) return false;
			
			//	Refresh the controls
			RefreshAll();
			
			return true;
			
		}// OnAttributesChanged()
		
		/// <summary>This method is called when the user wants to start playing a script</summary>
		/// <returns>true if successful</returns>
		public override bool StartScript()
		{
			m_bPlayingScript = true;
			RefreshAll();
			return true;
							
		}// public virtual bool StartScript()
		
		/// <summary>This method is called when the user wants to stop playing a script</summary>
		/// <returns>true if successful</returns>
		public override bool StopScript()
		{
			m_bPlayingScript = false;
			RefreshAll();
			return true;
			
		}// public virtual bool StopScript()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called when the user clicks on Edit Extents</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		protected void OnEditExtents(object sender, System.EventArgs e)
		{
			if(m_xmlDesignation != null)
			{
				CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
				
				//	Fire the video event
				Args.EventId = TmaxVideoCtrlEvents.EditDesignationExtents;
				Args.XmlDesignation = m_xmlDesignation;
				
				FireTmaxVideoCtrlEvent(Args);
			}
		
		}// protected void OnEditExtents(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Edit Text</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		protected void OnEditText(object sender, System.EventArgs e)
		{
			if(m_xmlDesignation != null)
			{
				CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
				//	Fire the video event
				Args.EventId = TmaxVideoCtrlEvents.EditDesignationText;
				Args.XmlDesignation = m_xmlDesignation;
			
				FireTmaxVideoCtrlEvent(Args);
			}
		
		}// protected void OnEditText(object sender, System.EventArgs e)
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected override void RecalcLayout()
		{
			if(this.Width <= 0) return;
			if(m_ctrlFilename == null) return;
			if(m_ctrlFilename.IsDisposed == true) return;
			if(m_ctrlDuration == null) return;
			if(m_ctrlDuration.IsDisposed == true) return;
			
			//	Make the name control as wide as possible
			m_ctrlName.SetBounds(0, 0,
								 this.Width - m_ctrlName.Left - 2, 0,
								 BoundsSpecified.Width);
									   
			//	Make the filename control as wide as possible
			m_ctrlFilename.SetBounds(0, 0,
									 this.Width - m_ctrlFilename.Left - 2, 0,
									 BoundsSpecified.Width);
									   
			//	Make the duration control as wide as possible
			m_ctrlDuration.SetBounds(0, 0,
									 this.Width - m_ctrlDuration.Left - 2, 0,
									 BoundsSpecified.Width);
									   
		}
		
		/// <summary>This method will update the control values</summary>
		protected void RefreshAll()
		{
			if(m_ctrlEditExtents == null) return;
			
			m_ctrlEditExtents.Enabled = ((m_xmlDesignation != null) && (m_bPlayingScript == false));
			
			m_ctrlEditText.Visible = ((m_xmlDesignation == null) || (m_xmlDesignation.HasText == true));
			m_ctrlEditText.Enabled = ((m_xmlDesignation != null) && (m_bPlayingScript == false));
			
			m_ctrlScrollText.Visible = ((m_xmlDesignation == null) || (m_xmlDesignation.HasText == true));
			m_ctrlScrollText.Enabled = ((m_xmlDesignation != null) && (m_bPlayingScript == false));
			m_ctrlScrollText.Checked = ((m_xmlDesignation != null) && (m_xmlDesignation.ScrollText));
			
			if(m_xmlDesignation != null)
			{
				m_ctrlMediaType.Text = (m_xmlDesignation.HasText ? "Designation:" : "Recording:");
				m_ctrlMediaType.Enabled = true;
				
				m_ctrlName.Text = m_xmlDesignation.Name;
				m_ctrlName.Enabled = true;

				if(m_xmlDesignation.HasText == true)
					m_ctrlStart.Text = CTmaxToolbox.PLToString(m_xmlDesignation.FirstPL);
				else
					m_ctrlStart.Text = CTmaxToolbox.SecondsToString(m_xmlDesignation.Start);
				m_ctrlStartLabel.Enabled = true;
				
				if(m_xmlDesignation.HasText == true)
					m_ctrlStop.Text = CTmaxToolbox.PLToString(m_xmlDesignation.LastPL);
				else
					m_ctrlStop.Text = CTmaxToolbox.SecondsToString(m_xmlDesignation.Stop);
				m_ctrlStopLabel.Enabled = true;
				
				m_ctrlDuration.Text = CTmaxToolbox.SecondsToString(m_xmlDesignation.Stop - m_xmlDesignation.Start);
				m_ctrlDurationLabel.Enabled = true;
				
			}
			else
			{
				m_ctrlMediaType.Text = "Media:";
				m_ctrlMediaType.Enabled = false;
				
				m_ctrlName.Text = "No selection";
				m_ctrlName.Enabled = false;
				
				m_ctrlStart.Text = "";
				m_ctrlStartLabel.Enabled = false;
				
				m_ctrlStop.Text = "";
				m_ctrlStopLabel.Enabled = false;
				
				m_ctrlDuration.Text = "";
				m_ctrlDurationLabel.Enabled = false;
				
			}
			
			if((m_ctrlFilename != null) && (m_ctrlFilename.IsDisposed == false))
			{
				if(m_strFileSpec != null && (m_strFileSpec.Length > 0))
					m_ctrlFilename.Text = System.IO.Path.GetFileName(m_strFileSpec);
				else
					m_ctrlFilename.Text = "";
			}
			
			if((m_ctrlFilenameLabel != null) && (m_ctrlFilenameLabel.IsDisposed == false))
			{
				if(m_strFileSpec != null && (m_strFileSpec.Length > 0))
					m_ctrlFilenameLabel.Enabled = true;
				else
					m_ctrlFilenameLabel.Enabled = false;
			}
			
		}// protected void RefreshFilename()
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected new void InitializeComponent()
		{
			this.m_ctrlMediaType = new System.Windows.Forms.Label();
			this.m_ctrlFilenameLabel = new System.Windows.Forms.Label();
			this.m_ctrlDurationLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.Label();
			this.m_ctrlFilename = new System.Windows.Forms.Label();
			this.m_ctrlDuration = new System.Windows.Forms.Label();
			this.m_ctrlStop = new System.Windows.Forms.Label();
			this.m_ctrlStart = new System.Windows.Forms.Label();
			this.m_ctrlStopLabel = new System.Windows.Forms.Label();
			this.m_ctrlStartLabel = new System.Windows.Forms.Label();
			this.m_ctrlEditExtents = new System.Windows.Forms.Button();
			this.m_ctrlEditText = new System.Windows.Forms.Button();
			this.m_ctrlScrollText = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// m_ctrlMediaType
			// 
			this.m_ctrlMediaType.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlMediaType.Name = "m_ctrlMediaType";
			this.m_ctrlMediaType.Size = new System.Drawing.Size(68, 12);
			this.m_ctrlMediaType.TabIndex = 0;
			this.m_ctrlMediaType.Text = "Deposition:";
			this.m_ctrlMediaType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlFilenameLabel
			// 
			this.m_ctrlFilenameLabel.Location = new System.Drawing.Point(140, 20);
			this.m_ctrlFilenameLabel.Name = "m_ctrlFilenameLabel";
			this.m_ctrlFilenameLabel.Size = new System.Drawing.Size(64, 12);
			this.m_ctrlFilenameLabel.TabIndex = 1;
			this.m_ctrlFilenameLabel.Text = "Filename:";
			this.m_ctrlFilenameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDurationLabel
			// 
			this.m_ctrlDurationLabel.Location = new System.Drawing.Point(140, 36);
			this.m_ctrlDurationLabel.Name = "m_ctrlDurationLabel";
			this.m_ctrlDurationLabel.Size = new System.Drawing.Size(64, 12);
			this.m_ctrlDurationLabel.TabIndex = 2;
			this.m_ctrlDurationLabel.Text = "Duration:";
			this.m_ctrlDurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Location = new System.Drawing.Point(72, 4);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(204, 12);
			this.m_ctrlName.TabIndex = 3;
			this.m_ctrlName.Text = "Ken Moore 11-18-2003";
			this.m_ctrlName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlFilename
			// 
			this.m_ctrlFilename.Location = new System.Drawing.Point(204, 20);
			this.m_ctrlFilename.Name = "m_ctrlFilename";
			this.m_ctrlFilename.Size = new System.Drawing.Size(72, 12);
			this.m_ctrlFilename.TabIndex = 4;
			this.m_ctrlFilename.Text = "km001.mpg";
			this.m_ctrlFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDuration
			// 
			this.m_ctrlDuration.Location = new System.Drawing.Point(204, 36);
			this.m_ctrlDuration.Name = "m_ctrlDuration";
			this.m_ctrlDuration.Size = new System.Drawing.Size(72, 12);
			this.m_ctrlDuration.TabIndex = 5;
			this.m_ctrlDuration.Text = "00:00:00.0";
			this.m_ctrlDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStop
			// 
			this.m_ctrlStop.Location = new System.Drawing.Point(72, 36);
			this.m_ctrlStop.Name = "m_ctrlStop";
			this.m_ctrlStop.Size = new System.Drawing.Size(64, 12);
			this.m_ctrlStop.TabIndex = 9;
			this.m_ctrlStop.Text = "00:00:00.0";
			this.m_ctrlStop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStart
			// 
			this.m_ctrlStart.Location = new System.Drawing.Point(72, 20);
			this.m_ctrlStart.Name = "m_ctrlStart";
			this.m_ctrlStart.Size = new System.Drawing.Size(64, 12);
			this.m_ctrlStart.TabIndex = 8;
			this.m_ctrlStart.Text = "00:00:00.0";
			this.m_ctrlStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStopLabel
			// 
			this.m_ctrlStopLabel.Location = new System.Drawing.Point(4, 36);
			this.m_ctrlStopLabel.Name = "m_ctrlStopLabel";
			this.m_ctrlStopLabel.Size = new System.Drawing.Size(68, 12);
			this.m_ctrlStopLabel.TabIndex = 7;
			this.m_ctrlStopLabel.Text = "Stop:";
			this.m_ctrlStopLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlStartLabel
			// 
			this.m_ctrlStartLabel.Location = new System.Drawing.Point(4, 20);
			this.m_ctrlStartLabel.Name = "m_ctrlStartLabel";
			this.m_ctrlStartLabel.Size = new System.Drawing.Size(68, 12);
			this.m_ctrlStartLabel.TabIndex = 6;
			this.m_ctrlStartLabel.Text = "Start:";
			this.m_ctrlStartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlEditExtents
			// 
			this.m_ctrlEditExtents.Location = new System.Drawing.Point(4, 52);
			this.m_ctrlEditExtents.Name = "m_ctrlEditExtents";
			this.m_ctrlEditExtents.TabIndex = 10;
			this.m_ctrlEditExtents.Text = "Edit Extents";
			this.m_ctrlEditExtents.Click += new System.EventHandler(this.OnEditExtents);
			// 
			// m_ctrlEditText
			// 
			this.m_ctrlEditText.Location = new System.Drawing.Point(84, 52);
			this.m_ctrlEditText.Name = "m_ctrlEditText";
			this.m_ctrlEditText.TabIndex = 11;
			this.m_ctrlEditText.Text = "Edit Text";
			this.m_ctrlEditText.Click += new System.EventHandler(this.OnEditText);
			// 
			// m_ctrlScrollText
			// 
			this.m_ctrlScrollText.Location = new System.Drawing.Point(172, 52);
			this.m_ctrlScrollText.Name = "m_ctrlScrollText";
			this.m_ctrlScrollText.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlScrollText.TabIndex = 12;
			this.m_ctrlScrollText.Text = "Scroll Text";
			// 
			// CTmaxVideoPropsCtrl
			// 
			this.Controls.Add(this.m_ctrlScrollText);
			this.Controls.Add(this.m_ctrlEditText);
			this.Controls.Add(this.m_ctrlEditExtents);
			this.Controls.Add(this.m_ctrlStop);
			this.Controls.Add(this.m_ctrlStart);
			this.Controls.Add(this.m_ctrlStopLabel);
			this.Controls.Add(this.m_ctrlStartLabel);
			this.Controls.Add(this.m_ctrlDuration);
			this.Controls.Add(this.m_ctrlFilename);
			this.Controls.Add(this.m_ctrlName);
			this.Controls.Add(this.m_ctrlDurationLabel);
			this.Controls.Add(this.m_ctrlFilenameLabel);
			this.Controls.Add(this.m_ctrlMediaType);
			this.Name = "CTmaxVideoPropsCtrl";
			this.Size = new System.Drawing.Size(280, 80);
			this.ResumeLayout(false);

		}
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>The current scroll text check state</summary>
		public bool ScrollText
		{
			get  
			{
				if(m_ctrlScrollText != null)
				{
					return m_ctrlScrollText.Checked;
				}
				else if(m_xmlDesignation != null)
				{
					return m_xmlDesignation.ScrollText;
				}
				else
				{
					return false;
				}
				
			}
		
		}// ScrollText
		
		#endregion Properties

	}// public class CTmaxDesignationCtrl : System.Windows.Forms.UserControl
	 
}// namespace FTI.Trialmax.Controls
