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
	/// <summary>Control used to add tuner bar to the CTmaxVideoCtrl object</summary>
	public class CTmaxVideoTuneBarCtrl : CTmaxVideoBaseCtrl
	{
		#region Contants
		
		protected System.Drawing.Color TUNED_BACK_COLOR   = System.Drawing.Color.LightGreen;
		protected System.Drawing.Color TUNED_FORE_COLOR   = System.Drawing.Color.Black;
		protected System.Drawing.Color UNTUNED_BACK_COLOR = System.Drawing.Color.LightCoral;
		protected System.Drawing.Color UNTUNED_FORE_COLOR = System.Drawing.Color.Black;
		
		protected const int ERROR_FIRE_COMMAND_EX			= 0;
		protected const int	ERROR_INVALID_START_POSITION	= 1;
		protected const int	ERROR_INVALID_STOP_POSITION		= 2;
		protected const int	ERROR_INVALID_LINK_POSITION		= 3;
		protected const int	ERROR_LINK_OUT_OF_RANGE			= 4;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Static text control to display current start time</summary>
		private System.Windows.Forms.Label m_ctrlStartTime;
		
		/// <summary>Pushbutton used to apply changes</summary>
		private System.Windows.Forms.Button m_ctrlApply;
		
		/// <summary>Static text control to display current stop time</summary>
		private System.Windows.Forms.Label m_ctrlStopTime;
		
		/// <summary>Static text control to display current link time</summary>
		private System.Windows.Forms.Label m_ctrlLinkTime;
		
		/// <summary>Pushbutton used to enter Start mode</summary>
		private System.Windows.Forms.RadioButton m_ctrlEditStart;
		
		/// <summary>Pushbutton used to enter Link mode</summary>
		private System.Windows.Forms.RadioButton m_ctrlEditLink;
		
		/// <summary>Pushbutton used to enter Stop mode</summary>
		private System.Windows.Forms.RadioButton m_ctrlEditStop;
		
		/// <summary>Horizontal border size</summary>
		private int m_iHorzBorder = 0;
		
		/// <summary>Horizontal spacing between controls</summary>
		private int m_iHorzSpacing = 0;
		
		/// <summary>Minimum mode buttons width</summary>
		private int m_iMinButtonWidth = 0;
		
		/// <summary>Minimum width required to display the controls without clipping</summary>
		private int m_iMinClientWidth = 0;
		
		/// <summary>Member used to indicate that the Tuned flag should be set for the current mode</summary>
		private bool m_bSetTuned = false;
		
		/// <summary>Local member bound to TuneMode property</summary>
		private TmaxVideoCtrlTuneModes m_eTuneMode = TmaxVideoCtrlTuneModes.None;
		
		/// <summary>Local member bound to Position property</summary>
		protected double m_dPosition = (double)-1;
		
		/// <summary>Local member bound to EnableLinks property</summary>
		protected bool m_bEnableLinks = true;
		
		/// <summary>Local member bound to PreviewPeriod property</summary>
		protected double m_dPreviewPeriod = 2.0;
		
		/// <summary>Static text label for the preview time edit control</summary>
		private System.Windows.Forms.Label m_ctrlPreviewLabel;
		
		/// <summary>Static text control to display warning when the active designation is not synchronized</summary>
		private System.Windows.Forms.Label m_ctrlNoSync;
		
		/// <summary>Text box control to allow user to define the preview period</summary>
		private System.Windows.Forms.TextBox m_ctrlPreviewPeriod;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoTuneBarCtrl() : base()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Video Tuner Bar Control";
			
			//	Initialize the child controls
			InitializeComponent();
			
			//	Initialize the controls
			OnAttributesChanged(m_xmlDesignation);
			OnAttributesChanged(m_xmlLink);
			
			m_ctrlPreviewPeriod.Text = m_dPreviewPeriod.ToString();
			
		}// public CTmaxTunerCtrl()

		/// <summary>This method is called to determine if modifications have been made to the active designation</summary>
		/// <param name="xmlDesignation">The active designation</param>
		///	<param name="aModifications">An array in which to put the description of all modifications</param>
		/// <returns>true if modified</returns>
		public override bool IsModified(CXmlDesignation xmlDesignation, ArrayList aModifications)
		{
			bool	bModified = false;
			string	strMsg = "";
			
			if(m_xmlDesignation == null) return false;
			
			Debug.Assert(ReferenceEquals(xmlDesignation, m_xmlDesignation) == true);
			if(ReferenceEquals(xmlDesignation, m_xmlDesignation) == false) return false;
			
			//	Check for this special case where the parent is calling this
			//	method because the user is attempting to apply changes from 
			//	some other place besides clicking on this control's Apply button
			//
			//	NOTE:	The only time the caller does not provide a modifications array
			//			is if it is trying to determine if there are any changes to Apply
			if(aModifications == null)
			{
				if(GetTuned() == false)
					return true;
			}
			
			//	Are we going to be setting the tuned state?
			if((m_bSetTuned == true)&& (m_eTuneMode != TmaxVideoCtrlTuneModes.Link))
			{ 
				if(aModifications != null)
				{
					if(m_eTuneMode == TmaxVideoCtrlTuneModes.Start)
						aModifications.Add("Designation Start Tuned flag has changed");
					else if(m_eTuneMode == TmaxVideoCtrlTuneModes.Stop)
						aModifications.Add("Designation Stop Tuned flag has changed");
				}
			 
				bModified = true;
			}
			
			//	Make sure we have the latest position
			QueryPlayerPosition();

			//	What is the current tune mode
			switch(m_eTuneMode)
			{
				case TmaxVideoCtrlTuneModes.Start:
				
					if(ComparePositions(m_dPosition, StartPosition) != 0)
					{
						if(aModifications != null)
						{
							strMsg = String.Format("Designation start position has changed from {0} to {1}", CTmaxToolbox.SecondsToString(StartPosition, 3), CTmaxToolbox.SecondsToString(m_dPosition, 3));
							aModifications.Add(strMsg);
						}
						
						bModified = true;
					}
					break;

				case TmaxVideoCtrlTuneModes.Stop:
				
					if(ComparePositions(m_dPosition, StopPosition) != 0)
					{
						if(aModifications != null)
						{
							strMsg = String.Format("Designation stop position has changed from {0} to {1}", CTmaxToolbox.SecondsToString(StopPosition, 3), CTmaxToolbox.SecondsToString(m_dPosition, 3));
							aModifications.Add(strMsg);
						}
						
						bModified = true;
					}
					break;
					
			}
			
			return bModified;
			
		}// public override bool IsModified(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to determine if modifications have been made to the active link</summary>
		/// <param name="xmlLink">The active link</param>
		///	<param name="aModifications">An array in which to put the description of all modifications</param>
		/// <returns>true if modified</returns>
		public override bool IsModified(CXmlLink xmlLink, ArrayList aModifications)
		{
			bool	bModified = false;
			string	strMsg = "";
			
			if(m_xmlLink == null) return false;
			
			Debug.Assert(ReferenceEquals(xmlLink, m_xmlLink) == true);
			if(ReferenceEquals(xmlLink, m_xmlLink) == false) return false;
			
			//	Check for this special case where the parent is calling this
			//	method because the user is attempting to apply changes from 
			//	some other place besides clicking on this control's Apply button
			//
			//	NOTE:	The only time the caller does not provide a modifications array
			//			is if it is trying to determine if there are any changes to Apply
			if(aModifications == null)
			{
				if(GetTuned() == false)
					return true;
			}
			
			//	Are we going to be setting the tuned state?
			if((m_bSetTuned == true) && (m_eTuneMode == TmaxVideoCtrlTuneModes.Link)) 
			{ 
				if(aModifications != null)
					aModifications.Add("Link Tuned flag has changed");
			 
				bModified = true;
			}
			
			//	Make sure we have the latest position
			QueryPlayerPosition();

			//	Is the user tuning the link?
			if(m_eTuneMode == TmaxVideoCtrlTuneModes.Link)
			{
				//	Has the position changed?
				if(ComparePositions(m_dPosition, LinkPosition) != 0)
				{
					if(aModifications != null)
					{
						strMsg = String.Format("Link tuned position has changed from {0} to {1}", CTmaxToolbox.SecondsToString(LinkPosition, 3), CTmaxToolbox.SecondsToString(m_dPosition, 3));
						aModifications.Add(strMsg);
					}
						
					bModified = true;
				}
			}
			
			return bModified;
			
		}// public override bool IsModified(CXmlLink xmlLink)
		
		/// <summary>This method is called to get the derived class property values and use them to set the designation attributes</summary>
		/// <param name="xmlDesignation">The designation to be updated with the current property values</param>
		/// <returns>true if successful</returns>
		public override bool SetAttributes(CXmlDesignation xmlDesignation)
		{
			//	Make sure we have the latest position
			QueryPlayerPosition();
			
			//	What is the current tune mode
			switch(m_eTuneMode)
			{
				case TmaxVideoCtrlTuneModes.Start:
				
					//	Check the  position to make sure it's valid
					if(CheckValues(true,false) == false) return false;
					
					xmlDesignation.Start = m_dPosition;
					xmlDesignation.StartTuned = true;
					break;

				case TmaxVideoCtrlTuneModes.Stop:
				
					//	Check the  position to make sure it's valid
					if(CheckValues(true,false) == false) return false;
					
					xmlDesignation.Stop = m_dPosition;
					xmlDesignation.StopTuned = true;
					break;
			}
			
			return true;
			
		}// public override bool SetAttributes(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to set the link attributes</summary>
		/// <param name="xmlDesignation">The link to be updated with the current property values</param>
		/// <returns>true if successful</returns>
		public override bool SetAttributes(CXmlLink xmlLink)
		{
			//	Make sure we have the latest position
			QueryPlayerPosition();
			
			//	Are we tuning a link?
			if(m_eTuneMode == TmaxVideoCtrlTuneModes.Link)
			{
				Debug.Assert(ReferenceEquals(m_xmlLink, xmlLink) == true);
				if(ReferenceEquals(m_xmlLink, xmlLink) == true)
				{
					//	Check the link position to make sure it's valid
					if(CheckValues(false, true) == false) return false;
					
					//	Store the new position
					m_xmlLink.Start = m_dPosition;
					m_xmlLink.StartTuned = true;
				}
					
			}
						
			return true;
			
		}// public override bool SetAttributes(CXmlLink xmlLink)
		
		/// <summary>This method is called when the attributes associated with the active designation have changed</summary>
		/// <param name="xmlDesignation">The designation who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public override bool OnAttributesChanged(CXmlDesignation xmlDesignation)
		{
			return RefreshAll();			
		}
		
		/// <summary>This method is called when the attributes associated with the active link have changed</summary>
		/// <param name="xmlLink">The link who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public override bool OnAttributesChanged(CXmlLink xmlLink)
		{
			return RefreshAll();			
		}
		
		/// <summary>This method is called to set the active tune mode</summary>
		/// <param name="eMode">The desired mode</param>
		/// <param name="bSilent">true to inhibit mode change events</param>
		/// <returns>The new tune mode</returns>
		public TmaxVideoCtrlTuneModes SetTuneMode(TmaxVideoCtrlTuneModes eMode, bool bSilent)
		{
			//	Set the appropriate check state of the buttons
			SetCheckStates(eMode);
		
			//	Update the property value
			m_eTuneMode = eMode;
			
			RefreshAll();
			
			//	Fire the event if requested
			if(bSilent == false)
				FireSetMode(m_eTuneMode);
				
			return TuneMode;
			
		}// public TmaxVideoCtrlTuneModes SetTuneMode(TmaxVideoCtrlTuneModes eMode, bool bSilent)
		
		/// <summary>This method is called to set the active tune mode</summary>
		/// <param name="eMode">The desired mode</param>
		/// <returns>The new tune mode</returns>
		public TmaxVideoCtrlTuneModes SetTuneMode(TmaxVideoCtrlTuneModes eMode)
		{
			//	Set the mode and fire the event
			return SetTuneMode(eMode, false);
		}
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
		/// <param name="xmlDesignation">The designation used to set property values</param>
		/// <param name="xmlLink">The link used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		{
			m_strFileSpec    = strFileSpec;
			m_xmlDesignation = xmlDesignation;
			m_xmlLink		 = xmlLink;
			
			//	Set the appropriate tune mode if not playing a script
			if(m_bPlayingScript == false)
			{
				if((StartPosition >= 0) && (StopPosition >= 0) && (StartPosition < StopPosition) && (IsSynchronized() == true))
					SetTuneMode(TmaxVideoCtrlTuneModes.Start, false);
				else
					SetTuneMode(TmaxVideoCtrlTuneModes.None, false);
			}	
			else
			{
				RefreshAll();
			}
			
			return true;
		}
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
		/// <param name="xmlDesignation">The designation used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		{
			return SetProperties(strFileSpec, xmlDesignation, null);
		
		}// public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="xmlLink">The link used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(CXmlLink xmlLink)
		{
			m_xmlLink = xmlLink;
		
			return RefreshAll();
		
		}// public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		
		/// <summary>This method handles all video events fired by the system</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		///	<remarks>The application must register this handler with an event source for this method to be called</remarks>
		public override void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
		{
			//	Which event?
			switch(e.EventId)
			{
				case TmaxVideoCtrlEvents.PlayerPositionChanged:
				
					//	Refresh the player position property
					PlayerPosition = e.Position;
					break;
					
				default:
				
					break;
					
			}
			
		}// public void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
	
		/// <summary>This method is called when the user wants to start playing a script</summary>
		/// <returns>true if successful</returns>
		public override bool StartScript()
		{
			m_bPlayingScript = true;

			SetTuneMode(TmaxVideoCtrlTuneModes.None);
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
		
		/// <summary>This method is called to refresh the control properties</summary>
		/// <returns>true if successful</returns>
		protected bool RefreshAll()
		{
			//	Do we have a valid start position?
			if((StartPosition >= 0) && (IsSynchronized() == true))
			{
				m_ctrlEditStart.Enabled = (m_bPlayingScript == false);
				m_ctrlStartTime.Text = CTmaxToolbox.SecondsToString(StartPosition);
				m_ctrlStartTime.BackColor = (m_xmlDesignation.StartTuned == true) ? TUNED_BACK_COLOR : UNTUNED_BACK_COLOR;
				m_ctrlStartTime.ForeColor = (m_xmlDesignation.StartTuned == true) ? TUNED_FORE_COLOR : UNTUNED_FORE_COLOR;
			}
			else
			{
				m_ctrlEditStart.Checked = false;
				m_ctrlEditStart.Enabled = false;
				m_ctrlStartTime.Text = "";
				m_ctrlStartTime.BackColor = this.BackColor;
				m_ctrlStartTime.ForeColor = this.ForeColor;
			}
		
			//	Do we have a valid stop position?
			if((StopPosition >= 0) && (IsSynchronized() == true))
			{
				m_ctrlEditStop.Enabled = (m_bPlayingScript == false);
				m_ctrlStopTime.Text = CTmaxToolbox.SecondsToString(StopPosition);
				m_ctrlStopTime.BackColor = (m_xmlDesignation.StopTuned == true) ? TUNED_BACK_COLOR : UNTUNED_BACK_COLOR;
				m_ctrlStopTime.ForeColor = (m_xmlDesignation.StopTuned == true) ? TUNED_FORE_COLOR : UNTUNED_FORE_COLOR;
			}
			else
			{
				m_ctrlEditStop.Checked = false;
				m_ctrlEditStop.Enabled = false;
				m_ctrlStopTime.Text = "";
				m_ctrlStopTime.BackColor = this.BackColor;
				m_ctrlStopTime.ForeColor = this.ForeColor;
			}
		
			//	Do we have a valid link position?
			if((LinkPosition >= 0) && (IsSynchronized() == true))
			{
				m_ctrlEditLink.Enabled = (m_bPlayingScript == false);
				m_ctrlLinkTime.Text = CTmaxToolbox.SecondsToString(LinkPosition);
				m_ctrlLinkTime.BackColor = (m_xmlLink.StartTuned == true) ? TUNED_BACK_COLOR : UNTUNED_BACK_COLOR;
				m_ctrlLinkTime.ForeColor = (m_xmlLink.StartTuned == true) ? TUNED_FORE_COLOR : UNTUNED_FORE_COLOR;
			}
			else
			{
				m_ctrlEditLink.Checked = false;
				m_ctrlEditLink.Enabled = false;
				m_ctrlLinkTime.Text = "";
				m_ctrlLinkTime.BackColor = this.BackColor;
				m_ctrlLinkTime.ForeColor = this.ForeColor;
			}
		
			m_ctrlApply.Enabled = ((m_bPlayingScript == false) && (IsSynchronized() == true));
			m_ctrlPreviewPeriod.Enabled = (m_bPlayingScript == false);
			m_ctrlPreviewLabel.Enabled = (m_bPlayingScript == false);
			
			if((m_xmlDesignation != null) && (IsSynchronized() == false))
			{
				m_ctrlNoSync.BackColor = UNTUNED_BACK_COLOR;
				m_ctrlNoSync.ForeColor = UNTUNED_FORE_COLOR;
				m_ctrlNoSync.Visible = true;
				m_ctrlNoSync.Enabled = true;
				m_ctrlNoSync.BringToFront();
			}
			else
			{
				m_ctrlNoSync.Visible = false;
				m_ctrlNoSync.Enabled = false;
				m_ctrlNoSync.SendToBack();
			}
				
			return true;
			
		}// protected bool RefreshAll()
		
		/// <summary>This method is called to make sure we have the latest playback position</summary>
		///	<remarks>true if successful</remarks>
		protected bool QueryPlayerPosition()
		{
			CTmaxVideoCtrlEventArgs Args = null;
			
			try
			{
				//	Construct the argument object
				Args = new CTmaxVideoCtrlEventArgs();
				Args.EventId = TmaxVideoCtrlEvents.QueryPlayerPosition;
				
				//	Fire the event
				FireTmaxVideoCtrlEvent(Args);
				
				//	Was it processed?
				if(Args.QueryHandled == true)
				{
					//	Update the position
					PlayerPosition = Args.Position;
					return true;
				}
				
			}
			catch
			{
			}
			
			return false;
						
		}// protected bool QueryPlayerPosition()
	
		/// <summary>Handles events fired when user changes the preview period</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnPreviewPeriodChanged(object sender, System.EventArgs e)
		{
			double				dPeriod = 0;
			CTmaxVideoCtrlEventArgs	Args = null;
			
			if(m_ctrlPreviewPeriod.Text.Length == 0) return;
			
			//	Try to convert the text to decimal seconds
			try
			{
				dPeriod = System.Convert.ToDouble(m_ctrlPreviewPeriod.Text);
			}
			catch
			{
				FTI.Shared.Win32.User.MessageBeep(0);
				return;
			}
			
			//	Update the local property and notify the owner
			if(dPeriod > 0)
			{
				m_dPreviewPeriod = dPeriod;
				
				//	Notify the system
				Args = new CTmaxVideoCtrlEventArgs();
				
				Args.EventId = TmaxVideoCtrlEvents.SetPreviewPeriod;
				Args.PreviewPeriod = m_dPreviewPeriod;
				
				FireTmaxVideoCtrlEvent(Args);
			}
		
		}// private void OnPreviewPeriodChanged(object sender, System.EventArgs e)

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

		/// <summary>This method is called to set the button check states</summary>
		/// <param name="eMode">The desired mode</param>
		public void SetCheckStates(TmaxVideoCtrlTuneModes eMode)
		{
			//	Set the appropriate check state of the buttons
			m_ctrlEditStart.Checked = (eMode == TmaxVideoCtrlTuneModes.Start);
			m_ctrlEditStop.Checked  = (eMode == TmaxVideoCtrlTuneModes.Stop);
			m_ctrlEditLink.Checked  = (eMode == TmaxVideoCtrlTuneModes.Link);
		}
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing a command event: command = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The start position is invalid. It must be before the existing stop position.");
			m_tmaxErrorBuilder.FormatStrings.Add("The stop position is invalid. It must be after the existing start position.");
			m_tmaxErrorBuilder.FormatStrings.Add("The link position is invalid. It must be between the start and stop positions.");
			m_tmaxErrorBuilder.FormatStrings.Add("The new %1 position will leave one or more links out of range. Do you still want to change the position?");

		}// protected void SetErrorStrings()

		/// <summary>This method is called to check the current values to see if they are valid</summary>
		/// <param name="bDesignation">true to check the designation extents</param>
		/// <param name="bLink">true to check the link position</param>
		/// <returns>true if values are ok</returns>
		protected bool CheckValues(bool bDesignation, bool bLink)
		{
			if(m_xmlDesignation == null) return true;
			
			//	Assume the position has been updated prior to calling
			//	this method
			
			//	What is the current mode
			switch(m_eTuneMode)
			{
				case TmaxVideoCtrlTuneModes.Start:
				
					if(bDesignation == true)
					{
						if(m_dPosition >= m_xmlDesignation.Stop)
						{
							return Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_START_POSITION));
						}
						else if(m_xmlDesignation.CheckLinkPositions(m_dPosition, m_xmlDesignation.Stop) == false)
						{
							if(MessageBox.Show(m_tmaxErrorBuilder.Message(ERROR_LINK_OUT_OF_RANGE, "start"), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
								return false;
						}
					
					}
					break;
					
				case TmaxVideoCtrlTuneModes.Stop:
				
					if(bDesignation == true)
					{
						if(m_dPosition <= m_xmlDesignation.Start)
						{
							return Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_STOP_POSITION));
						}
						else if(m_xmlDesignation.CheckLinkPositions(m_xmlDesignation.Start, m_dPosition) == false)
						{
							if(MessageBox.Show(m_tmaxErrorBuilder.Message(ERROR_LINK_OUT_OF_RANGE, "stop"), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
								return false;
						}
						
					}
					break;
					
				case TmaxVideoCtrlTuneModes.Link:
				
					if(bLink == true)
					{
						if((m_dPosition < (m_xmlDesignation.Start - TMAXVIDEO_MAX_POSITION_TOLERANCE)) ||
						   (m_dPosition > (m_xmlDesignation.Stop + TMAXVIDEO_MAX_POSITION_TOLERANCE)))
						{
							return Warn(m_tmaxErrorBuilder.Message(ERROR_INVALID_LINK_POSITION));
						}
					}
					
					break;
					
				default:
				
					break;
					
			}
			
			//	All's well if we reached this point
			return true;
			
		}// public bool CheckValues()
		
		/// <summary>This method is called to display a warning message</summary>
		/// <param name="strMsg">The warning message</param>
		/// <returns>always false (reduces code)</returns>
		protected bool Warn(string strMsg)
		{
			MessageBox.Show(strMsg, "Tune Error", 
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation);
			return false;
			
		}// protected bool Warn(string strMsg)
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected new void InitializeComponent()
		{
			this.m_ctrlStopTime = new System.Windows.Forms.Label();
			this.m_ctrlLinkTime = new System.Windows.Forms.Label();
			this.m_ctrlStartTime = new System.Windows.Forms.Label();
			this.m_ctrlApply = new System.Windows.Forms.Button();
			this.m_ctrlEditStart = new System.Windows.Forms.RadioButton();
			this.m_ctrlEditLink = new System.Windows.Forms.RadioButton();
			this.m_ctrlEditStop = new System.Windows.Forms.RadioButton();
			this.m_ctrlPreviewLabel = new System.Windows.Forms.Label();
			this.m_ctrlPreviewPeriod = new System.Windows.Forms.TextBox();
			this.m_ctrlNoSync = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlStopTime
			// 
			this.m_ctrlStopTime.Location = new System.Drawing.Point(176, 28);
			this.m_ctrlStopTime.Name = "m_ctrlStopTime";
			this.m_ctrlStopTime.Size = new System.Drawing.Size(82, 16);
			this.m_ctrlStopTime.TabIndex = 7;
			this.m_ctrlStopTime.Text = "00:00:00.0";
			this.m_ctrlStopTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlLinkTime
			// 
			this.m_ctrlLinkTime.Location = new System.Drawing.Point(91, 28);
			this.m_ctrlLinkTime.Name = "m_ctrlLinkTime";
			this.m_ctrlLinkTime.Size = new System.Drawing.Size(82, 16);
			this.m_ctrlLinkTime.TabIndex = 6;
			this.m_ctrlLinkTime.Text = "00:00:00.0";
			this.m_ctrlLinkTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlStartTime
			// 
			this.m_ctrlStartTime.Location = new System.Drawing.Point(4, 28);
			this.m_ctrlStartTime.Name = "m_ctrlStartTime";
			this.m_ctrlStartTime.Size = new System.Drawing.Size(82, 16);
			this.m_ctrlStartTime.TabIndex = 5;
			this.m_ctrlStartTime.Text = "00:00:00.0";
			this.m_ctrlStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlApply
			// 
			this.m_ctrlApply.Location = new System.Drawing.Point(176, 44);
			this.m_ctrlApply.Name = "m_ctrlApply";
			this.m_ctrlApply.Size = new System.Drawing.Size(82, 23);
			this.m_ctrlApply.TabIndex = 4;
			this.m_ctrlApply.Text = "Apply";
			this.m_ctrlApply.Click += new System.EventHandler(this.OnApply);
			// 
			// m_ctrlEditStart
			// 
			this.m_ctrlEditStart.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_ctrlEditStart.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlEditStart.Name = "m_ctrlEditStart";
			this.m_ctrlEditStart.Size = new System.Drawing.Size(82, 23);
			this.m_ctrlEditStart.TabIndex = 0;
			this.m_ctrlEditStart.Text = "Tune Start";
			this.m_ctrlEditStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_ctrlEditStart.Click += new System.EventHandler(this.OnModeClick);
			// 
			// m_ctrlEditLink
			// 
			this.m_ctrlEditLink.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_ctrlEditLink.Location = new System.Drawing.Point(91, 4);
			this.m_ctrlEditLink.Name = "m_ctrlEditLink";
			this.m_ctrlEditLink.Size = new System.Drawing.Size(82, 23);
			this.m_ctrlEditLink.TabIndex = 1;
			this.m_ctrlEditLink.Text = "Tune Link";
			this.m_ctrlEditLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_ctrlEditLink.Click += new System.EventHandler(this.OnModeClick);
			// 
			// m_ctrlEditStop
			// 
			this.m_ctrlEditStop.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_ctrlEditStop.Location = new System.Drawing.Point(176, 4);
			this.m_ctrlEditStop.Name = "m_ctrlEditStop";
			this.m_ctrlEditStop.Size = new System.Drawing.Size(82, 23);
			this.m_ctrlEditStop.TabIndex = 2;
			this.m_ctrlEditStop.Text = "Tune Stop";
			this.m_ctrlEditStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_ctrlEditStop.Click += new System.EventHandler(this.OnModeClick);
			// 
			// m_ctrlPreviewLabel
			// 
			this.m_ctrlPreviewLabel.Location = new System.Drawing.Point(4, 52);
			this.m_ctrlPreviewLabel.Name = "m_ctrlPreviewLabel";
			this.m_ctrlPreviewLabel.Size = new System.Drawing.Size(76, 12);
			this.m_ctrlPreviewLabel.TabIndex = 8;
			this.m_ctrlPreviewLabel.Text = "Preview (sec)";
			this.m_ctrlPreviewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlPreviewPeriod
			// 
			this.m_ctrlPreviewPeriod.Location = new System.Drawing.Point(84, 47);
			this.m_ctrlPreviewPeriod.Name = "m_ctrlPreviewPeriod";
			this.m_ctrlPreviewPeriod.Size = new System.Drawing.Size(44, 20);
			this.m_ctrlPreviewPeriod.TabIndex = 9;
			this.m_ctrlPreviewPeriod.Text = "5";
			this.m_ctrlPreviewPeriod.WordWrap = false;
			this.m_ctrlPreviewPeriod.TextChanged += new System.EventHandler(this.OnPreviewPeriodChanged);
			// 
			// m_ctrlNoSync
			// 
			this.m_ctrlNoSync.BackColor = System.Drawing.Color.Red;
			this.m_ctrlNoSync.ForeColor = System.Drawing.Color.White;
			this.m_ctrlNoSync.Location = new System.Drawing.Point(64, 28);
			this.m_ctrlNoSync.Name = "m_ctrlNoSync";
			this.m_ctrlNoSync.Size = new System.Drawing.Size(44, 16);
			this.m_ctrlNoSync.TabIndex = 10;
			this.m_ctrlNoSync.Text = "No time sync information";
			this.m_ctrlNoSync.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CTmaxVideoTuneBarCtrl
			// 
			this.Controls.Add(this.m_ctrlNoSync);
			this.Controls.Add(this.m_ctrlPreviewPeriod);
			this.Controls.Add(this.m_ctrlPreviewLabel);
			this.Controls.Add(this.m_ctrlEditStop);
			this.Controls.Add(this.m_ctrlEditLink);
			this.Controls.Add(this.m_ctrlEditStart);
			this.Controls.Add(this.m_ctrlStopTime);
			this.Controls.Add(this.m_ctrlLinkTime);
			this.Controls.Add(this.m_ctrlStartTime);
			this.Controls.Add(this.m_ctrlApply);
			this.Name = "CTmaxVideoTuneBarCtrl";
			this.Size = new System.Drawing.Size(264, 72);
			this.ResumeLayout(false);

		}

		/// <summary>Stores points required to calculate the layout</summary>
		protected void GetCoordinates()
		{
			//	Get the values we need to perform layout
			m_iHorzBorder     = m_ctrlEditStart.Left;
			m_iHorzSpacing    = m_ctrlEditLink.Left - m_ctrlEditStart.Right;
			m_iMinButtonWidth = m_ctrlEditStart.Width;
			
			m_iMinClientWidth = ((2 * m_iHorzBorder) + (3 * m_iMinButtonWidth) + (2 * m_iHorzSpacing));

		}// private void GetCoordinates()
		
		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected override void RecalcLayout()
		{
			int iCtrlWidth = 0;
			int iLeft = 0;
			
			//	Do we need to get the coordinates?
			if(m_iMinClientWidth <= 0)
				GetCoordinates();
				
			//	Should we clip?
			if(this.Width <= m_iMinClientWidth)
			{
				iCtrlWidth = m_iMinButtonWidth;
			}
			else
			{
				iCtrlWidth = ((this.Width - (2 * m_iHorzBorder) - (2 * m_iHorzSpacing)) / 3);
				if(iCtrlWidth < m_iMinButtonWidth)
				{
					Debug.Assert(iCtrlWidth >= m_iMinButtonWidth);
					iCtrlWidth = m_iMinButtonWidth;
				}
			}
			
			iLeft = m_iHorzBorder;
			m_ctrlEditStart.SetBounds(iLeft, 0, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Width);
			m_ctrlStartTime.SetBounds(iLeft, 0, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Width);
			
			//	Move to next column
			iLeft = m_ctrlEditStart.Right + m_iHorzSpacing;
			
			m_ctrlEditLink.SetBounds(iLeft, 0, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Width);
			m_ctrlLinkTime.SetBounds(iLeft, 0, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Width);
				
			//	Move the Apply button into the Links position if not in use
			if(m_bEnableLinks == false)
				m_ctrlApply.SetBounds(iLeft, m_ctrlEditStop.Top, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);
			
			//	Move to next column
			iLeft = m_ctrlEditLink.Right + m_iHorzSpacing;
			
			m_ctrlEditStop.SetBounds(iLeft, 0, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Width);
			m_ctrlStopTime.SetBounds(iLeft, 0, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Width);
			if(m_bEnableLinks == true)
				m_ctrlApply.SetBounds(iLeft, 0, iCtrlWidth, 0, BoundsSpecified.X | BoundsSpecified.Width);
				
			m_ctrlNoSync.SetBounds(m_ctrlStartTime.Left, 0,
								  (m_ctrlStopTime.Right - m_ctrlStartTime.Left), 0,
								   BoundsSpecified.X | BoundsSpecified.Width);
		}// private void RecalcLayout()
		
		/// <summary>Handles events fired when user clicks on one of the mode buttons</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		protected void OnModeClick(object sender, System.EventArgs e)
		{
			TmaxVideoCtrlTuneModes eMode = TmaxVideoCtrlTuneModes.None;
			
			//	Which button?
			if(ReferenceEquals(sender, m_ctrlEditStart))
			{
				eMode = TmaxVideoCtrlTuneModes.Start;
			}
			else if(ReferenceEquals(sender, m_ctrlEditStop))
			{
				eMode = TmaxVideoCtrlTuneModes.Stop;
			}
			else if(ReferenceEquals(sender, m_ctrlEditLink))
			{
				eMode = TmaxVideoCtrlTuneModes.Link;
			}
			
			//	Is the mode changing?
			if(eMode != m_eTuneMode)
			{
				//	Is it ok to make the change?
				//
				//	NOTE:	Since we are not changing the designation
				//			or link object we only prompt for confirmation
				//			if we are about to lose a tuning modification
				if((IsModified(null) == true) && (CanContinue() == false))
				{
					//	Put the buttons back where they were
					SetCheckStates(m_eTuneMode);
					return;
				}
			
			}
			
			//	Change the mode
			SetTuneMode(eMode, false);
			
		}// protected void OnModeClick(object sender, System.EventArgs e)

		/// <summary>Handles events fired when user clicks on the Apply button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		protected void OnApply(object sender, System.EventArgs e)
		{
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			//	Are we going to need to set the Tuned state?
			if(GetTuned() == false)
				m_bSetTuned = true;
				
			//	Fire the video event
			Args.EventId = TmaxVideoCtrlEvents.Apply;
			FireTmaxVideoCtrlEvent(Args);
			
			m_bSetTuned = false;
			
		}// protected void OnApply(object sender, System.EventArgs e)

		/// <summary>Called to determine if the current location has been tuned</summary>
		/// <returns>true if the location has been tuned</returns>
		protected bool GetTuned()
		{
			if(m_xmlDesignation == null) return true;
			
			//	What is the current mode
			switch(m_eTuneMode)
			{
				case TmaxVideoCtrlTuneModes.Start:
				
					return m_xmlDesignation.StartTuned;
					
				case TmaxVideoCtrlTuneModes.Stop:
				
					return m_xmlDesignation.StopTuned;
					
				case TmaxVideoCtrlTuneModes.Link:
				
					if(m_xmlLink != null)
						return m_xmlLink.StartTuned;
					else
						return true; // This prevents additional processing
						
				default:
				
					return true;
					
			}
			
		}// protected bool GetTuned()

		/// <summary>This method is called to fire a TrialMax video event to set the active tune mode</summary>
		/// <param name="Args">The event arguments</param>
		///	<returns>true if successful</returns>
		protected bool FireSetMode(TmaxVideoCtrlTuneModes eMode)
		{
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			//	Initialize the arguments
			Args.EventId = TmaxVideoCtrlEvents.SetMode;
			Args.TuneMode = eMode;
			
			//	Fire the event
			return FireTmaxVideoCtrlEvent(Args);
			
		}// protected bool FireSetMode(TmaxVideoCtrlTuneModes eMode)
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>Current tune mode as reflected by the button state</summary>
		public TmaxVideoCtrlTuneModes TuneMode
		{
			get	{ return m_eTuneMode; }
			set { SetTuneMode(value); }
		}

		/// <summary>Current playback position</summary>
		public double PlayerPosition
		{
			get { return m_dPosition; }
			set { m_dPosition = value; }
		}
		
		/// <summary>Time for previewing video from current position</summary>
		public double PreviewPeriod
		{
			get { return m_dPreviewPeriod; }
			set 
			{
				m_dPreviewPeriod = value;
				if(m_ctrlPreviewPeriod != null)
					m_ctrlPreviewPeriod.Text = m_dPreviewPeriod.ToString();
			}
		
		}// PreviewPeriod
		
		/// <summary>Enable link editing</summary>
		public bool EnableLinks
		{
			get { return m_bEnableLinks; }
			set 
			{ 
				m_bEnableLinks = value; 
				if((m_ctrlEditLink != null) && (m_ctrlEditLink.IsDisposed == false))
				{
					try { m_ctrlEditLink.Visible = m_bEnableLinks; }
					catch {}
				}
			}
		}
		
		#endregion Properties
		
	}// public class CTmaxTunerCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls
