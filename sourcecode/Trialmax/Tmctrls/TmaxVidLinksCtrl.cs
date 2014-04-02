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
	/// <summary>This control creates an interface for managing video links</summary>
	public class CTmaxVideoLinksCtrl : CTmaxVideoBaseCtrl
	{
		#region Constants
		
		protected int DEFAULT_SPACING = 4;
		protected int DEFAULT_BORDER = 4;
		
		protected const int ERROR_FILL_EX				= 0;
		protected const int ERROR_FIRE_VIDEO_EVENT_EX	= 1;
		protected const int ERROR_GET_CLIP_POSITION_EX	= 2;
		protected const int ERROR_GET_PL_EX				= 3;
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Pushbutton to add new link</summary>
		private System.Windows.Forms.Button m_ctrlAddLink;

		/// <summary>Checkbox to select split screen option</summary>
		private System.Windows.Forms.CheckBox m_ctrlSplitLink;

		/// <summary>Checkbox to indicate link removal</summary>
		private System.Windows.Forms.CheckBox m_ctrlHideLink;

		/// <summary>Seconds associated with link position</summary>
		private System.Windows.Forms.TextBox m_ctrlSeconds;

		/// <summary>Minutes associated with link position</summary>
		private System.Windows.Forms.TextBox m_ctrlMinutes;

		/// <summary>Hours associated with link position</summary>
		private System.Windows.Forms.TextBox m_ctrlHours;

		/// <summary>Label for Seconds control</summary>
		private System.Windows.Forms.Label m_ctrlSecondsLabel;

		/// <summary>Label for Minutes control</summary>
		private System.Windows.Forms.Label m_ctrlMinutesLabel;

		/// <summary>Label for Hours control</summary>
		private System.Windows.Forms.Label m_ctrlHoursLabel;

		/// <summary>Barcode for new link</summary>
		private System.Windows.Forms.TextBox m_ctrlBarcode;

		/// <summary>Label for Barcode control</summary>
		private System.Windows.Forms.Label m_ctrlBarcodeLabel;

		/// <summary>Line number used to define link position</summary>
		private System.Windows.Forms.TextBox m_ctrlLine;

		/// <summary>Page number used to define link position</summary>
		private System.Windows.Forms.TextBox m_ctrlPage;

		/// <summary>Label for Line Number control</summary>
		private System.Windows.Forms.Label m_ctrlLineLabel;

		/// <summary>Label for Pane Number control</summary>
		private System.Windows.Forms.Label m_ctrlPageLabel;

		/// <summary>List of links associated with the active designation</summary>
		private System.Windows.Forms.ListView m_ctrlLinks;

		/// <summary>List box columns used to display links</summary>
		private System.Windows.Forms.ColumnHeader T;
		private System.Windows.Forms.ColumnHeader S;
		private System.Windows.Forms.ColumnHeader B;

		/// <summary>Image list bound to the list of links</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;

		/// <summary>Panels docked to bottom of control to group the link options</summary>
		private Panel m_ctrlBottomPanel;

		/// <summary>Check box to select the Hide Video options for a visible link</summary>
		private CheckBox m_ctrlHideVideo;

		/// <summary>Check box to select the Hide Text options for a visible link</summary>
		private CheckBox m_ctrlHideText;
		
		/// <summary>Local flag to inhibit event processing</summary>
		private bool m_bIgnoreEvents = false;
		
		/// <summary>Local member to prevent reentrancy while restoring the list view selection</summary>
		private bool m_bRestoringSelection = false;
		
		/// <summary>Local member to store Hours</summary>
		private int m_iHours = -1;
		
		/// <summary>Local member to store Minutes</summary>
		private int m_iMinutes = -1;
		
		/// <summary>Local member to store Seconds</summary>
		private double m_dSeconds = -1;		
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoLinksCtrl() : base()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Video Links Control";
			
			//	Initialize the child controls
			InitializeComponent();
			
			//	Keep the list view sorted based on link start time
			m_ctrlLinks.ListViewItemSorter = new CLinksViewSorter();
		}

		/// <summary>This method is called to get the derived class property values and use them to set the link attributes</summary>
		/// <param name="xmlLink">The link to be updated with the current property values</param>
		/// <returns>true if successful</returns>
		public override bool SetAttributes(CXmlLink xmlLink)
		{
			CXmlLink xmlBackup = null;
			
			Debug.Assert(xmlLink != null);
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return false;
			
			//	Create a backup first
			xmlBackup = new CXmlLink(xmlLink);
			
			//	Get the new values
			if(Validate(xmlLink, false) == false)
			{
				//	Restore the original values
				xmlLink.Copy(xmlBackup);
				return false;
			}
			else
			{
				return true;
			}

		}// public override bool SetAttributes(CXmlLink xmlLink)

		/// <summary>This method is called to set the control properties</summary>
		/// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
		/// <param name="xmlDesignation">The designation used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		{
			//	Save the new values
			m_strFileSpec    = strFileSpec;
			m_xmlDesignation = xmlDesignation;

			//	Local reference is no longer valid
			m_xmlLink = null;
			SetHMS();
			
			//	Refill the list box
			Fill();
			
			//	Set the control states using the new values
			SetControlStates(true);
			
			return true;
		
		}// public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="xmlLink">The link used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(CXmlLink xmlLink)
		{
			SetLink(m_xmlLink);
			return true;
		}
		
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

			//	Has the hide option changed?
			if(m_ctrlHideLink.Checked != m_xmlLink.Hide)
			{
				if(aModifications != null)
					aModifications.Add("Link hide option has changed");
				bModified = true;
			}
					
			//	Has the split screen option changed?
			if(m_ctrlSplitLink.Checked != m_xmlLink.Split)
			{
				if(aModifications != null)
					aModifications.Add("Link split screen option has changed");
				bModified = true;
			}
					
			//	Has the barcode changed?
			if(m_xmlLink.Hide == false)
			{
				if(String.Compare(m_ctrlBarcode.Text, m_xmlLink.SourceMediaId, true) != 0)
				{
					if(aModifications != null)
						aModifications.Add("Link barcode has changed");
					bModified = true;
				}
			}

			//	Has the Hide video option changed?
			if(m_ctrlHideVideo.Checked != m_xmlLink.HideVideo)
			{
				if(aModifications != null)
					aModifications.Add("Disable video option has changed");
				bModified = true;
			}

			//	Has the Hide text option changed?
			if(m_ctrlHideText.Checked != m_xmlLink.HideText)
			{
				if(aModifications != null)
					aModifications.Add("Disable text option has changed");
				bModified = true;
			}

			//	Has the position changed?
			if(IsPositionModified() == true)
			{
				if(IsMovieClip() == true)
					strMsg = "Link HH:MM:SS position has changed";
				else
					strMsg = "Link Page / Line position has changed";
				
				if(aModifications != null)
					aModifications.Add(strMsg);
				
				bModified = true;
			}
					
			return bModified;
			
		}// public override bool IsModified(CXmlLink xmlLink)
		
		/// <summary>This method is called when the attributes associated with the active link have changed</summary>
		/// <param name="xmlLink">The link who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public override bool OnAttributesChanged(CXmlLink xmlLink)
		{
			//	Refresh the control values
			SetControlStates(true);
			return true;
		}
		
		/// <summary>This method is called when the attributes associated with the active designation have changed</summary>
		/// <param name="xmlLink">The designation who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public override bool OnAttributesChanged(CXmlDesignation xmlDesignation)
		{
			//	Refresh the control values
			SetControlStates(true);
			return true;
		}
		
		/// <summary>This method is called to determine if it is OK to change the active designation and/or link</summary>
		/// <returns>true if OK to continue</returns>
		public override bool CanContinue()
		{
			//	By default we are only concerned with the active link
			return CanContinue(false,true);	
		}
			
		/// <summary>This method is called to add a link to the list</summary>
		/// <param name="xmlLink">The link to be added</param>
		/// <returns>true if successful</returns>
		public bool Add(CXmlLink xmlLink, bool bSelect)
		{
			ListViewItem lvItem = new ListViewItem();
			
			//	Make sure this link exists in the active designation
			//
			//	NOTE:	We have to do this check because it may be that
			//			the link was added without using the designation
			//			assigned to this control
			if(m_xmlDesignation.Links.Contains(xmlLink) == false)
				m_xmlDesignation.Links.Add(xmlLink);
				
			//	Set the item values
			SetListItemProps(lvItem, xmlLink, true);
			
			//	Add it to the list
			m_ctrlLinks.Items.Add(lvItem);
			m_ctrlLinks.Sort();
			
			//	Automatically resize the columns to fit the text
			SuspendLayout();
			m_ctrlLinks.Columns[1].Width = -2;
			m_ctrlLinks.Columns[2].Width = -2;
			ResumeLayout();

			//	Should we select the new link
			if(bSelect)
				SetLink(xmlLink, false);
			
			return true;
		
		}// public bool Add(CXmlLink xmlLink, bool bSelect)
		
		/// <summary>This method is called to delete a link in the list</summary>
		/// <param name="xmlLink">The link to be deleted</param>
		/// <returns>true if successful</returns>
		public bool Delete(CXmlLink xmlLink)
		{
			ListViewItem lvItem = null;
			
			if((lvItem = GetListItem(xmlLink)) != null)
			{
				//	Is this the selected link?
				if(lvItem.Selected == true)
					SetLink(null);
					
				m_ctrlLinks.Items.Remove(lvItem);
			}
			
			return true;
		
		}// public bool Add(CXmlLink xmlLink, bool bSelect)
		
		/// <summary>This method is called to set the active link</summary>
		/// <param name="xmlLink">The link to be activated</param>
		/// <param name="bSilent">true to prevent firing SetLink event</param>
		/// <returns>true if successful</returns>
		public bool SetLink(CXmlLink xmlLink, bool bSilent)
		{
			if(m_xmlDesignation == null) return false;
			if(m_xmlDesignation.Links == null) return false;
			
			if(xmlLink != null)
			{
				if(m_xmlDesignation.Links.Contains(xmlLink) == false)
				{
					Debug.Assert(false);
					return false;
				}
			}

			//	Select this item in the list box
			SetListSelection(xmlLink);
			
			//	Reset the local members and update the controls
			m_xmlLink = xmlLink;
			OnAttributesChanged(m_xmlLink);

			//	Should we fire the event?
			if(bSilent == false)
			{
				FireLinkChanged(m_xmlLink);
			}
			
			return true;
			
		}// public bool SetLink(object oScene)

		/// <summary>This method is called to set the value of the ClassicLinks property</summary>
		public override void SetClassicLinks(bool bClassicLinks)
		{
			//	Perform the base class processing
			base.SetClassicLinks(bClassicLinks);
			
			//	Update the control states
			SetControlStates(false);

		}// public override void SetClassicLinks(bool bClassicLinks)

		/// <summary>This method is called to set the active link</summary>
		/// <param name="xmlLink">The link to be activated</param>
		/// <returns>true if successful</returns>
		public bool SetLink(CXmlLink xmlLink)
		{
			return SetLink(xmlLink, false);
		}
		
		/// <summary>This method is called when the user wants to start playing a script</summary>
		/// <returns>true if successful</returns>
		public override bool StartScript()
		{
			m_bPlayingScript = true;
			SetControlStates(true);
			return true;
							
		}// public virtual bool StartScript()
		
		/// <summary>This method is called when the user wants to stop playing a script</summary>
		/// <returns>true if successful</returns>
		public override bool StopScript()
		{
			m_bPlayingScript = false;
			SetControlStates(true);
			return true;
			
		}// public virtual bool StopScript()
		
		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);

			SetControlStates(true);
		}

		/// <summary>This method is called to set the selection in the list box</summary>
		/// <param name="xmlLink">The link to be selected</param>
		/// <returns>true if successful</returns>
		public bool SetListSelection(CXmlLink xmlLink)
		{
			ListViewItem lvItem = null;
			bool		 bSuccessful = true;
			
			try
			{
				m_bIgnoreEvents = true;
				
				//	Clear the current selections
				m_ctrlLinks.SelectedItems.Clear();
				
				if(xmlLink != null)
				{
					if((lvItem = GetListItem(xmlLink)) != null)
					{
						lvItem.Selected = true;
						m_ctrlLinks.EnsureVisible(lvItem.Index);
					}
				}

			}
			catch
			{
				bSuccessful = false;
			}
			finally
			{
				m_bIgnoreEvents = false;
			}
			
			return bSuccessful;
			
		}// public bool SetListSelection(CXmlLink xmlLink)
		
		/// <summary>This method is called to validate the user settings and store the values in the specified object</summary>
		/// <param name="xmlLink">The object in which to store the validated values</param>
		/// <param name="bInitialize">True if this link is being initialized for the first time</param>
		/// <returns>true if successful</returns>
		public bool Validate(CXmlLink xmlLink, bool bInitialize)
		{
			string	strSourceMediaId = "";
			string	strSourceDbId = "";
			bool	bHide = false;
			double	dPosition = -1;
			long	lPL = -1;
			long	lPage = -1;
			int		iLine = -1;
			string  strMsg = "";
			bool	bPositionChanged = false;
			int		iHours = 0;
			int		iMinutes = 0;
			double	dSeconds = 0;
			
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return false;
			
			//	Is this a visible link?
			if((bHide = m_ctrlHideLink.Checked) == false)
			{
				//	What is the media id (barcode) ?
				strSourceMediaId = m_ctrlBarcode.Text;
				
				//	Did the user supply a barcode?
				if(strSourceMediaId.Length == 0)
				{
					return Warn("You must supply a barcode for visible links");
				}
				
				//	Get the source database id
				strSourceDbId = GetSourceDbId(ref strSourceMediaId);
				
				//	Did we get the database record?
				if(strSourceDbId.Length == 0)
				{
					//	Let the tune page handle the warnings
					//return Warn("Unable to locate a record for the specified barcode."); 
					
					return false;
				}
				else
				{
					//	Just in case the source media id changed
					m_ctrlBarcode.Text = strSourceMediaId;
				}
				
			}
			
			//	Is the active designation a movie clip?
			if(IsMovieClip() == true)
			{	
				//	Get the hours
				try
				{
					if(m_ctrlHours.Text.Length > 0)
						iHours = System.Convert.ToInt32(m_ctrlHours.Text);
				}
				catch
				{
					return Warn("You must supply a valid number of hours");
				}
			
				//	Get the minutes
				try
				{
					if(m_ctrlMinutes.Text.Length > 0)
						iMinutes = System.Convert.ToInt32(m_ctrlMinutes.Text);
				}
				catch
				{
					return Warn("You must supply a valid number of minutes");
				}
			
				//	Get the seconds
				try
				{
					if(m_ctrlSeconds.Text.Length > 0)
						dSeconds = System.Convert.ToDouble(m_ctrlSeconds.Text);
				}
				catch
				{
					return Warn("You must supply a valid number of seconds");
				}
			
				//	Has the position changed?
				if((bInitialize == true) || (iHours != m_iHours) || 
				   (iMinutes != m_iMinutes) || (dSeconds != m_dSeconds))
				{
					//	Calculate the new position
					dPosition = ((iHours * 3600.0) + (iMinutes * 60.0) + dSeconds);
					bPositionChanged = true;
									
					//	Is this within the range allowed for the designation?
					if((dPosition < m_xmlDesignation.Start) || (dPosition > m_xmlDesignation.Stop))
					{
						strMsg = String.Format("The specified position of {0} is outside the range defined by the clip. start = {1} stop = {2}",
											CTmaxToolbox.SecondsToString(dPosition),
											CTmaxToolbox.SecondsToString(m_xmlDesignation.Start),
											CTmaxToolbox.SecondsToString(m_xmlDesignation.Stop));
						return Warn(strMsg);
					}
					
				}
				
			}
			else // if(IsMovieClip() == true)
			{
				//	Get the page number
				try
				{
					lPage = System.Convert.ToInt64(m_ctrlPage.Text);
				}
				catch
				{
					return Warn("You must supply a valid page number");
				}
			
				//	Get the line number
				try
				{
					iLine = System.Convert.ToInt32(m_ctrlLine.Text);
				}
				catch
				{
					return Warn("You must supply a valid line number");
				}
				
				//	Get the composite PL value
				lPL = CTmaxToolbox.GetPL(lPage, iLine);
				
				//	Has the position changed?
				if((bInitialize == true) || (m_xmlLink == null) || 
				   (lPL != m_xmlLink.PL))
				{
					//	Is this value within range?
					if((lPL < m_xmlDesignation.FirstPL) || (lPL > m_xmlDesignation.LastPL))
					{
						strMsg = String.Format("The specified page/line is outside the range defined for the designation. {0}:{1} to {2}:{3}",
							CTmaxToolbox.PLToPage(m_xmlDesignation.FirstPL),
							CTmaxToolbox.PLToLine(m_xmlDesignation.FirstPL),
							CTmaxToolbox.PLToPage(m_xmlDesignation.LastPL),
							CTmaxToolbox.PLToLine(m_xmlDesignation.LastPL));
						return Warn(strMsg);
					}
					
					//	Get the position associated with this PL value
					if((dPosition = GetPosition(lPL)) < 0)
					{
						return Warn("Unable to translate the specified page and line to a time position within the designation.");	
					}
					
					//	The position has changed
					bPositionChanged = true;
					
				}
				
			}// if(IsMovieClip() == true)
			
			//	Set the caller's object properties
			if(xmlLink != null)
			{
				xmlLink.SourceDbId    = strSourceDbId;
				xmlLink.SourceMediaId = strSourceMediaId;
				xmlLink.Hide          = bHide;
				xmlLink.Split         = m_ctrlSplitLink.Checked;
				xmlLink.HideText	  = m_ctrlHideText.Checked;
				xmlLink.HideVideo	  = m_ctrlHideVideo.Checked;
				
				//	Has the position changed?
				if(bPositionChanged == true)
				{
					if(IsMovieClip() == false)
					{
						xmlLink.PL   = lPL;
						xmlLink.Page = lPage;
						xmlLink.Line = iLine;
					}
					
					xmlLink.Start = dPosition;
					xmlLink.StartTuned = false;
				}
				
			}
			
			return true;
		
		}// public bool Validate(CXmlLink xmlLink)

		/// <summary>This method is called to warn the user when an invalid setting is encountered</summary>
		/// <param name="strMsg">The message to be displayed</param>
		/// <returns>Always false - just makes for some cleaner code</returns>
		protected bool Warn(string strMsg)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		
		/// <summary>This method is called to determine if the hour controls should be enabled</summary>
		/// <returns>true if they should be enabled</returns>
		protected bool HoursEnabled()
		{
			if(IsMovieClip() == false) return false;
			if(m_xmlDesignation == null) return false;
			if(m_xmlDesignation.Stop < 3600.0) return false;
			
			return true;
		
		}// protected bool HoursEnabled()
		
		/// <summary>This method is called to determine if the minute controls should be enabled</summary>
		/// <returns>true if they should be enabled</returns>
		protected bool MinutesEnabled()
		{
			if(IsMovieClip() == false) return false;
			if(m_xmlDesignation == null) return false;
			if(m_xmlDesignation.Stop < 60.0) return false;
			
			return true;
		
		}// protected bool MinutesEnabled()
		
		/// <summary>This method is called to set the enable/disable the controls</summary>
		/// <param name="bRefresh">True to refresh the control values</param>
		protected void SetControlStates(bool bRefresh)
		{
			bool bClip = IsMovieClip();
			bool bSynchronized = false;

			//	Check to make sure the text is synchronized if this is not a clip
			if(m_xmlDesignation != null)
			{
				if(bClip == false)
					bSynchronized = m_xmlDesignation.GetSynchronized(false);
				else
					bSynchronized = true;// Assume synchronized for clips

			}// if(m_xmlDesignation != null)
				
			//	Movie clip controls
			m_ctrlHours.Enabled = ((HoursEnabled() == true) && (m_bPlayingScript == false));
			m_ctrlHours.Visible = (bClip == true);
			m_ctrlHoursLabel.Enabled = m_ctrlHours.Enabled;
			m_ctrlHoursLabel.Visible = m_ctrlHours.Visible;
				
			m_ctrlMinutes.Enabled = ((MinutesEnabled() == true) && (m_bPlayingScript == false));
			m_ctrlMinutes.Visible = (bClip == true);
			m_ctrlMinutesLabel.Enabled = m_ctrlMinutes.Enabled;
			m_ctrlMinutesLabel.Visible = m_ctrlMinutes.Visible;
				
			m_ctrlSeconds.Enabled = ((bClip == true) && (m_bPlayingScript == false));
			m_ctrlSeconds.Visible = (bClip == true);
			m_ctrlSecondsLabel.Enabled = m_ctrlSeconds.Enabled;
			m_ctrlSecondsLabel.Visible = m_ctrlSeconds.Visible;
			
			//	Transcript designation controls
			m_ctrlPage.Enabled = ((bClip == false) && (bSynchronized == true) && (m_bPlayingScript == false));
			m_ctrlPage.Visible = !m_ctrlHours.Visible;
			m_ctrlPageLabel.Enabled = m_ctrlPage.Enabled;
			m_ctrlPageLabel.Visible = m_ctrlPage.Visible;

			m_ctrlLine.Enabled = ((bClip == false) && (bSynchronized == true) && (m_bPlayingScript == false));
			m_ctrlLine.Visible = !m_ctrlHours.Visible;
			m_ctrlLineLabel.Enabled = m_ctrlLine.Enabled;
			m_ctrlLineLabel.Visible = m_ctrlLine.Visible;
				
			m_ctrlBarcode.Enabled = ((bSynchronized == true) && (m_bPlayingScript == false));
			m_ctrlBarcodeLabel.Enabled = ((bSynchronized == true) && (m_bPlayingScript == false));
			
			m_ctrlAddLink.Enabled = ((bSynchronized == true) && (m_bPlayingScript == false));
			
			m_ctrlHideLink.Enabled = ((bSynchronized == true) && (m_bPlayingScript == false));

			m_ctrlLinks.Enabled = (bSynchronized == true);
			
			//	Refresh the control values if requested
			if(bRefresh == true)
				RefreshAll();

			//	Do these last so that we can use the check state of the Hide Link check box
			m_ctrlHideText.Enabled = ((bClip == false) && (m_bClassicLinks == false) && (m_ctrlHideLink.Enabled == true));
			m_ctrlHideVideo.Enabled = ((m_ctrlHideLink.Enabled == true));

			m_ctrlSplitLink.Enabled = ((m_ctrlHideLink.Enabled == true) && (m_ctrlHideLink.Checked == false) && (m_ctrlHideVideo.Checked == false));

		}// protected void SetControlStates()
		
		/// <summary>This method is called to refresh all child controls</summary>
		protected void RefreshAll()
		{
			bool			bClip = IsMovieClip();
			ListViewItem	lvItem = null;
			
			//	Update the local H:M:S values
			SetHMS();

			if(m_xmlLink != null)
			{
				if((lvItem = GetListItem(m_xmlLink)) != null)
					SetListItemProps(lvItem, m_xmlLink, false);
					
				m_ctrlBarcode.Text = m_xmlLink.SourceMediaId;
				m_ctrlHideLink.Checked = m_xmlLink.Hide;
				m_ctrlSplitLink.Checked = m_xmlLink.Split;
				m_ctrlHideText.Checked = m_xmlLink.HideText;
				m_ctrlHideVideo.Checked = m_xmlLink.HideVideo;
				
				if(bClip == true)
				{
					System.TimeSpan ts = System.TimeSpan.FromSeconds(m_xmlLink.Start);
					m_ctrlHours.Text = ts.Hours.ToString();
					m_ctrlMinutes.Text = ts.Minutes.ToString();
					m_ctrlSeconds.Text = ts.Seconds.ToString();
					m_ctrlPage.Text = "";
					m_ctrlLine.Text = "";
				}
				else
				{
					m_ctrlHours.Text = "";
					m_ctrlMinutes.Text = "";
					m_ctrlSeconds.Text = "";
					m_ctrlPage.Text = m_xmlLink.Page.ToString();
					m_ctrlLine.Text = m_xmlLink.Line.ToString();
				}
				
			}
			else
			{
				m_ctrlBarcode.Text = "";
				m_ctrlPage.Text = "";
				m_ctrlLine.Text = "";
				m_ctrlHours.Text = "";
				m_ctrlMinutes.Text = "";
				m_ctrlSeconds.Text = "";
				m_ctrlBarcode.Text = "";
				m_ctrlHideLink.Checked = false;
				m_ctrlSplitLink.Checked = false;
				m_ctrlHideVideo.Checked = false;
				
				if(m_xmlDesignation != null)
					m_ctrlHideText.Checked = !m_xmlDesignation.ScrollText;
				else
					m_ctrlHideText.Checked = false;
				
			}
		
			if(m_ctrlLinks.Items.Count > 0)
			{
				//	Make sure sort order is correct
				m_ctrlLinks.Sort();
				
				//	Automatically resize the columns to fit the text
				SuspendLayout();
				m_ctrlLinks.Columns[1].Width = -2;
				m_ctrlLinks.Columns[2].Width = -2;
				ResumeLayout();
			}

		}// protected void RefreshAll()
		
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
		}

		/// <summary>This method is called to determine if the link position has been modified</summary>
		/// <param name="xmlLink">The link to be checked</param>
		/// <returns>true if modified</returns>
		public bool IsPositionModified()
		{
			if(m_xmlLink == null) return false;

			if(IsMovieClip() == true)
			{
				try
				{
					if(System.Convert.ToInt32(m_ctrlHours.Text) != m_iHours)
						return true;
					if(System.Convert.ToInt32(m_ctrlMinutes.Text) != m_iMinutes)
						return true;
					if(System.Convert.ToDouble(m_ctrlSeconds.Text) != m_dSeconds)
						return true;
				}
				catch
				{
					//	NOTE:	We don't alert the user of the problem unless
					//			they actually try to commit the change
					return true;
				}
			
			}
			else
			{
				//	Has the PL value changed?
				try
				{
					if(GetPL() != m_xmlLink.PL)
						return true;
				}
				catch
				{
					//	NOTE:	We don't alert the user of the problem unless
					//			they actually try to commit the change
					return true;
				}
			
			}
					
			return false;
			
		}// public bool IsPositionModified()
		
		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected override void RecalcLayout()
		{
			//int iWidth = 0;
			//int iHeight = 0;
			
			//if(this.Width <= 0) return;
			//if(m_ctrlAddLink == null) return;
			//if(m_ctrlAddLink.IsDisposed == true) return;
			//if(m_ctrlBarcodeLabel == null) return;
			//if(m_ctrlBarcodeLabel.IsDisposed == true) return;
			//if(m_ctrlBarcode == null) return;
			//if(m_ctrlBarcode.IsDisposed == true) return;
			
			////	Put the add link button in the bottom right corner
			//m_ctrlAddLink.SetBounds(this.Width - m_ctrlAddLink.Width - DEFAULT_BORDER,
			//                        this.Height - m_ctrlAddLink.Height - DEFAULT_BORDER,
			//                        0, 0, BoundsSpecified.Location);
									
			////	Put the barcode label in the lower left corner
			//m_ctrlBarcodeLabel.SetBounds(DEFAULT_BORDER,
			//                             this.Height - m_ctrlBarcodeLabel.Height - DEFAULT_BORDER,
			//                             0, 0, BoundsSpecified.Location);
									
			////	Put the barcode edit box between the label and the add button
			//m_ctrlBarcode.SetBounds(m_ctrlBarcodeLabel.Right + DEFAULT_SPACING,
			//                        this.Height - m_ctrlBarcode.Height - DEFAULT_BORDER,
			//                        m_ctrlAddLink.Left - m_ctrlBarcodeLabel.Right - (2 * DEFAULT_SPACING),
			//                        0, BoundsSpecified.Location | BoundsSpecified.Width);
									
			////	Reposition the controls used to define the link position
			//m_ctrlHoursLabel.SetBounds(0, m_ctrlAddLink.Top - m_ctrlHoursLabel.Height - DEFAULT_SPACING,
			//                           0, 0, BoundsSpecified.Y);
			//m_ctrlHours.SetBounds(0, m_ctrlAddLink.Top - m_ctrlHours.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlMinutesLabel.SetBounds(0, m_ctrlAddLink.Top - m_ctrlMinutesLabel.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlMinutes.SetBounds(0, m_ctrlAddLink.Top - m_ctrlMinutes.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlSecondsLabel.SetBounds(0, m_ctrlAddLink.Top - m_ctrlSecondsLabel.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlSeconds.SetBounds(0, m_ctrlAddLink.Top - m_ctrlSeconds.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlLineLabel.SetBounds(0, m_ctrlAddLink.Top - m_ctrlLineLabel.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlLine.SetBounds(0, m_ctrlAddLink.Top - m_ctrlLine.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlPageLabel.SetBounds(0, m_ctrlAddLink.Top - m_ctrlPageLabel.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlPage.SetBounds(0, m_ctrlAddLink.Top - m_ctrlPage.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
									   
			////	Reposition the check boxes
			//m_ctrlHideLink.SetBounds(0, m_ctrlAddLink.Top - m_ctrlHideLink.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
			//m_ctrlSplitLink.SetBounds(0, m_ctrlAddLink.Top - m_ctrlSplitLink.Height - DEFAULT_SPACING,
			//    0, 0, BoundsSpecified.Y);
				
			////	Fill up the rest of the area at the top with the list box
			//iHeight = (m_ctrlPage.Top - DEFAULT_SPACING - DEFAULT_BORDER);
			//if(iHeight < 0) iHeight = 0;
			
			//iWidth  = this.Width - (2 * DEFAULT_BORDER);
			//if(iWidth < 0) iWidth = 0;
			
			//m_ctrlLinks.SetBounds(DEFAULT_BORDER, DEFAULT_BORDER, iWidth, iHeight);
									   
		}// protected void RecalcLayout()
		
		/// <summary>This method is called to get the position defined by the PL value</summary>
		/// <returns>The position at which the page/line occurs</returns>
		protected double GetPosition(long lPL)
		{
			double	dPosition = -1;
			
			Debug.Assert(m_xmlDesignation != null);
			Debug.Assert(m_xmlDesignation.Transcripts != null);
			
			if(m_xmlDesignation != null)
			{
				dPosition = m_xmlDesignation.GetPosition(lPL);
			}
			
			return dPosition;
			
		}// protected double GetPosition(long lPL)
		
		/// <summary>This method is called to get the database identifier for the link's source media</summary>
		/// <param name="strSourceMediaId">The media id of the source object</param>
		/// <returns>The unique PST identifier assigned by the databse</returns>
		protected string GetSourceDbId(ref string strSourceMediaId)
		{
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			Args.EventId = TmaxVideoCtrlEvents.QueryLinkSourceDbId;
			Args.LinkSourceMediaId = strSourceMediaId;
			
			FireTmaxVideoCtrlEvent(Args);
			
			//	Was the event handled
			if(Args.QueryHandled == true)
			{
				//	The handler may have changed the source media id
				strSourceMediaId = Args.LinkSourceMediaId;
				
				return Args.LinkSourceDbId;
			}
			else
			{
				return "";
			}
			
		}// protected string GetSourceDbId(string strSourceMediaId)
		
		/// <summary>This method is called to get the composite PL identifier for the link position</summary>
		/// <returns>The PL value specified for the link</returns>
		protected long GetPL()
		{
			long lPage = 0;
			int  iLine = 0;
			
			lPage = System.Convert.ToInt64(m_ctrlPage.Text);
			iLine = System.Convert.ToInt32(m_ctrlLine.Text);
			
			return CTmaxToolbox.GetPL(lPage, iLine);
			
		}// protected long GetPL()
		
		/// <summary>This method is called to set the local HMS values</summary>
		protected void SetHMS()
		{
			//	Reset hte members
			m_iHours = -1;
			m_iMinutes = -1;
			m_dSeconds = -1;
			
			if(m_xmlLink != null)
			{
				try
				{
					System.TimeSpan ts = System.TimeSpan.FromSeconds(m_xmlLink.Start);
					
					m_iHours   = ts.Hours;
					m_iMinutes = ts.Minutes;
					m_dSeconds = (double)ts.Seconds + ts.Milliseconds / 1000;
				}
				catch
				{
				}
				
			}
			
		}// protected void SetHMS()
		
		/// <summary>This method is called to populate the list box</summary>
		/// <returns>true if successful</returns>
		protected bool Fill()
		{
			bool			bSuccessful = false;
			ListViewItem	lvItem = null;
			
			Debug.Assert(m_ctrlLinks != null);
			Debug.Assert(m_ctrlLinks.IsDisposed == false);
			if(m_ctrlLinks == null) return false;
			if(m_ctrlLinks.IsDisposed == true) return false;
			
			try
			{
				//	Clear the existing links
				m_ctrlLinks.Items.Clear();
				
				//	Do we have an active list of links?
				if((m_xmlDesignation != null) && (m_xmlDesignation.Links != null))
				{
					foreach(CXmlLink O in m_xmlDesignation.Links)
					{
						lvItem = new ListViewItem();
						
						SetListItemProps(lvItem, O, true);
						
						m_ctrlLinks.Items.Add(lvItem);
					}
				
				}
				
				//	Automatically resize the columns to fit the text
				SuspendLayout();
				m_ctrlLinks.Columns[1].Width = -2;
				m_ctrlLinks.Columns[2].Width = -2;
				ResumeLayout();
			
				//	Done
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Fill", m_tmaxErrorBuilder.Message(ERROR_FILL_EX), Ex);
			}
			finally
			{
			}

			return bSuccessful;
		
		}// protected bool Fill()
		
		/// <summary>This method is called to get the list view item associated with the specified XML link</summary>
		///	<param name="xmlLink">The link associated with the item</param>
		/// <returns>the list item if found</returns>
		protected ListViewItem GetListItem(CXmlLink xmlLink)
		{
			Debug.Assert(xmlLink != null);
			
			foreach(ListViewItem O in m_ctrlLinks.Items)
			{
				if(ReferenceEquals(xmlLink, O.Tag) == true)
					return O;
			}
			
			return null;
		
		}// protected ListViewItem GetListItem(CXmlLink xmlLink)
		
		/// <summary>This method is called to get the barcode to be dropped during a drag & drop operation</summary>
		///	<remarks>the drop barcode if available</remarks>
		protected string QueryLinkDropBarcode()
		{
			CTmaxVideoCtrlEventArgs Args = null;
			string				strBarcode = "";
			
			try
			{
				//	Construct the argument object
				Args = new CTmaxVideoCtrlEventArgs();
				Args.EventId = TmaxVideoCtrlEvents.QueryLinkDropBarcode;
				
				//	Fire the event
				FireTmaxVideoCtrlEvent(Args);
				
				//	Was it processed?
				if(Args.QueryHandled == true)
				{
					if(Args.LinkDropBarcode != null)
						strBarcode = Args.LinkDropBarcode;
				}
				
			}
			catch
			{
			}
			
			return strBarcode;
						
		}// protected string QueryDropBarcode()
	
		/// <summary>This method is called to set the properties for the list item</summary>
		///	<param name="lvItem">The list item being set</param>
		///	<param name="xmlLink">The link associated with the item</param>
		/// <param name="bInitialize">true if the item is being initialized</param>
		/// <returns>the updated item</returns>
		protected ListViewItem SetListItemProps(ListViewItem lvItem, CXmlLink xmlLink, bool bInitialize)
		{
			string strText = "";
			
			Debug.Assert(lvItem != null);
			Debug.Assert(xmlLink != null);
			if(lvItem == null) return null;
			if(xmlLink == null) return null;
			
			//	Initialize the item
			lvItem.Tag = xmlLink;
			
			//	Set the image for the first column
			lvItem.Text = "";
			lvItem.ImageIndex = xmlLink.StartTuned ? 0 : -1;
							
			if(xmlLink.PL > 0)
				strText = CTmaxToolbox.PLToString(xmlLink.PL);
			else
				strText = CTmaxToolbox.SecondsToString(xmlLink.Start);
			
			if(bInitialize == true)
				lvItem.SubItems.Add(strText);
			else
				lvItem.SubItems[1].Text = strText;
								
			if(xmlLink.Hide == true)
				strText = "Hide";
			else
				strText = xmlLink.SourceMediaId;
			
			if(bInitialize == true)
				lvItem.SubItems.Add(strText);
			else
				lvItem.SubItems[2].Text = strText;
								
			return lvItem;
		
		}// protected ListViewItem SetListItemProps(ListViewItem lvItem, CXmlLink xmlLink)
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the links list box.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing a video event. event = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to calculate the link position. The HH:MM:SS values are not valid");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to calculate the link position. The PAGE/LINE values are not valid");

		}// protected void SetErrorStrings()

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected new void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTmaxVideoLinksCtrl));
			this.m_ctrlBarcode = new System.Windows.Forms.TextBox();
			this.m_ctrlBarcodeLabel = new System.Windows.Forms.Label();
			this.m_ctrlAddLink = new System.Windows.Forms.Button();
			this.m_ctrlSplitLink = new System.Windows.Forms.CheckBox();
			this.m_ctrlHideLink = new System.Windows.Forms.CheckBox();
			this.m_ctrlSeconds = new System.Windows.Forms.TextBox();
			this.m_ctrlMinutes = new System.Windows.Forms.TextBox();
			this.m_ctrlHours = new System.Windows.Forms.TextBox();
			this.m_ctrlSecondsLabel = new System.Windows.Forms.Label();
			this.m_ctrlMinutesLabel = new System.Windows.Forms.Label();
			this.m_ctrlHoursLabel = new System.Windows.Forms.Label();
			this.m_ctrlLine = new System.Windows.Forms.TextBox();
			this.m_ctrlPage = new System.Windows.Forms.TextBox();
			this.m_ctrlLineLabel = new System.Windows.Forms.Label();
			this.m_ctrlPageLabel = new System.Windows.Forms.Label();
			this.m_ctrlLinks = new System.Windows.Forms.ListView();
			this.T = new System.Windows.Forms.ColumnHeader();
			this.S = new System.Windows.Forms.ColumnHeader();
			this.B = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlBottomPanel = new System.Windows.Forms.Panel();
			this.m_ctrlHideVideo = new System.Windows.Forms.CheckBox();
			this.m_ctrlHideText = new System.Windows.Forms.CheckBox();
			this.m_ctrlBottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlBarcode
			// 
			this.m_ctrlBarcode.AllowDrop = true;
			this.m_ctrlBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBarcode.Location = new System.Drawing.Point(55, 61);
			this.m_ctrlBarcode.Name = "m_ctrlBarcode";
			this.m_ctrlBarcode.Size = new System.Drawing.Size(136, 20);
			this.m_ctrlBarcode.TabIndex = 47;
			this.m_ctrlBarcode.WordWrap = false;
			this.m_ctrlBarcode.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
			this.m_ctrlBarcode.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			// 
			// m_ctrlBarcodeLabel
			// 
			this.m_ctrlBarcodeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlBarcodeLabel.Location = new System.Drawing.Point(3, 60);
			this.m_ctrlBarcodeLabel.Name = "m_ctrlBarcodeLabel";
			this.m_ctrlBarcodeLabel.Size = new System.Drawing.Size(53, 23);
			this.m_ctrlBarcodeLabel.TabIndex = 46;
			this.m_ctrlBarcodeLabel.Text = "Barcode :";
			this.m_ctrlBarcodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAddLink
			// 
			this.m_ctrlAddLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAddLink.Enabled = false;
			this.m_ctrlAddLink.Location = new System.Drawing.Point(197, 60);
			this.m_ctrlAddLink.Name = "m_ctrlAddLink";
			this.m_ctrlAddLink.Size = new System.Drawing.Size(72, 23);
			this.m_ctrlAddLink.TabIndex = 45;
			this.m_ctrlAddLink.Text = "Add Link";
			this.m_ctrlAddLink.Click += new System.EventHandler(this.OnAddLink);
			// 
			// m_ctrlSplitLink
			// 
			this.m_ctrlSplitLink.Location = new System.Drawing.Point(8, 33);
			this.m_ctrlSplitLink.Name = "m_ctrlSplitLink";
			this.m_ctrlSplitLink.Size = new System.Drawing.Size(93, 24);
			this.m_ctrlSplitLink.TabIndex = 44;
			this.m_ctrlSplitLink.Text = "Split Screen";
			// 
			// m_ctrlHideLink
			// 
			this.m_ctrlHideLink.Location = new System.Drawing.Point(179, 7);
			this.m_ctrlHideLink.Name = "m_ctrlHideLink";
			this.m_ctrlHideLink.Size = new System.Drawing.Size(74, 24);
			this.m_ctrlHideLink.TabIndex = 43;
			this.m_ctrlHideLink.Text = "Hide Link";
			this.m_ctrlHideLink.Click += new System.EventHandler(this.OnClickHideLink);
			// 
			// m_ctrlSeconds
			// 
			this.m_ctrlSeconds.Location = new System.Drawing.Point(127, 7);
			this.m_ctrlSeconds.Name = "m_ctrlSeconds";
			this.m_ctrlSeconds.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlSeconds.TabIndex = 42;
			this.m_ctrlSeconds.Text = "88";
			this.m_ctrlSeconds.Visible = false;
			this.m_ctrlSeconds.WordWrap = false;
			// 
			// m_ctrlMinutes
			// 
			this.m_ctrlMinutes.Location = new System.Drawing.Point(76, 7);
			this.m_ctrlMinutes.Name = "m_ctrlMinutes";
			this.m_ctrlMinutes.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlMinutes.TabIndex = 41;
			this.m_ctrlMinutes.Visible = false;
			this.m_ctrlMinutes.WordWrap = false;
			// 
			// m_ctrlHours
			// 
			this.m_ctrlHours.Location = new System.Drawing.Point(26, 7);
			this.m_ctrlHours.Name = "m_ctrlHours";
			this.m_ctrlHours.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlHours.TabIndex = 40;
			this.m_ctrlHours.Visible = false;
			this.m_ctrlHours.WordWrap = false;
			// 
			// m_ctrlSecondsLabel
			// 
			this.m_ctrlSecondsLabel.Location = new System.Drawing.Point(103, 7);
			this.m_ctrlSecondsLabel.Name = "m_ctrlSecondsLabel";
			this.m_ctrlSecondsLabel.Size = new System.Drawing.Size(20, 20);
			this.m_ctrlSecondsLabel.TabIndex = 39;
			this.m_ctrlSecondsLabel.Text = "S:";
			this.m_ctrlSecondsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlSecondsLabel.Visible = false;
			// 
			// m_ctrlMinutesLabel
			// 
			this.m_ctrlMinutesLabel.Location = new System.Drawing.Point(53, 7);
			this.m_ctrlMinutesLabel.Name = "m_ctrlMinutesLabel";
			this.m_ctrlMinutesLabel.Size = new System.Drawing.Size(20, 20);
			this.m_ctrlMinutesLabel.TabIndex = 38;
			this.m_ctrlMinutesLabel.Text = "M:";
			this.m_ctrlMinutesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlMinutesLabel.Visible = false;
			// 
			// m_ctrlHoursLabel
			// 
			this.m_ctrlHoursLabel.Location = new System.Drawing.Point(4, 6);
			this.m_ctrlHoursLabel.Name = "m_ctrlHoursLabel";
			this.m_ctrlHoursLabel.Size = new System.Drawing.Size(20, 20);
			this.m_ctrlHoursLabel.TabIndex = 37;
			this.m_ctrlHoursLabel.Text = "H:";
			this.m_ctrlHoursLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlHoursLabel.Visible = false;
			// 
			// m_ctrlLine
			// 
			this.m_ctrlLine.Location = new System.Drawing.Point(115, 7);
			this.m_ctrlLine.Name = "m_ctrlLine";
			this.m_ctrlLine.Size = new System.Drawing.Size(36, 20);
			this.m_ctrlLine.TabIndex = 51;
			this.m_ctrlLine.WordWrap = false;
			// 
			// m_ctrlPage
			// 
			this.m_ctrlPage.Location = new System.Drawing.Point(32, 7);
			this.m_ctrlPage.Name = "m_ctrlPage";
			this.m_ctrlPage.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlPage.TabIndex = 50;
			this.m_ctrlPage.WordWrap = false;
			// 
			// m_ctrlLineLabel
			// 
			this.m_ctrlLineLabel.Location = new System.Drawing.Point(84, 7);
			this.m_ctrlLineLabel.Name = "m_ctrlLineLabel";
			this.m_ctrlLineLabel.Size = new System.Drawing.Size(28, 20);
			this.m_ctrlLineLabel.TabIndex = 49;
			this.m_ctrlLineLabel.Text = "LN:";
			this.m_ctrlLineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_ctrlPageLabel
			// 
			this.m_ctrlPageLabel.Location = new System.Drawing.Point(4, 6);
			this.m_ctrlPageLabel.Name = "m_ctrlPageLabel";
			this.m_ctrlPageLabel.Size = new System.Drawing.Size(26, 20);
			this.m_ctrlPageLabel.TabIndex = 48;
			this.m_ctrlPageLabel.Text = "PG:";
			this.m_ctrlPageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_ctrlLinks
			// 
			this.m_ctrlLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlLinks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.T,
            this.S,
            this.B});
			this.m_ctrlLinks.FullRowSelect = true;
			this.m_ctrlLinks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.m_ctrlLinks.HideSelection = false;
			this.m_ctrlLinks.LabelWrap = false;
			this.m_ctrlLinks.Location = new System.Drawing.Point(3, 0);
			this.m_ctrlLinks.MultiSelect = false;
			this.m_ctrlLinks.Name = "m_ctrlLinks";
			this.m_ctrlLinks.Size = new System.Drawing.Size(269, 50);
			this.m_ctrlLinks.SmallImageList = this.m_ctrlImages;
			this.m_ctrlLinks.TabIndex = 52;
			this.m_ctrlLinks.UseCompatibleStateImageBehavior = false;
			this.m_ctrlLinks.View = System.Windows.Forms.View.Details;
			this.m_ctrlLinks.SelectedIndexChanged += new System.EventHandler(this.OnLinksSelChanged);
			// 
			// T
			// 
			this.T.Text = "T";
			this.T.Width = 16;
			// 
			// S
			// 
			this.S.Text = "S";
			// 
			// B
			// 
			this.B.Text = "B";
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlImages.Images.SetKeyName(0, "");
			// 
			// m_ctrlBottomPanel
			// 
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlHideVideo);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlHideText);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlAddLink);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlBarcodeLabel);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlLine);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlBarcode);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlPage);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlHideLink);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlLineLabel);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlHoursLabel);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlPageLabel);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlMinutesLabel);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlSplitLink);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlSecondsLabel);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlHours);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlSeconds);
			this.m_ctrlBottomPanel.Controls.Add(this.m_ctrlMinutes);
			this.m_ctrlBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_ctrlBottomPanel.Location = new System.Drawing.Point(0, 51);
			this.m_ctrlBottomPanel.Name = "m_ctrlBottomPanel";
			this.m_ctrlBottomPanel.Size = new System.Drawing.Size(275, 86);
			this.m_ctrlBottomPanel.TabIndex = 53;
			// 
			// m_ctrlHideVideo
			// 
			this.m_ctrlHideVideo.Location = new System.Drawing.Point(179, 33);
			this.m_ctrlHideVideo.Name = "m_ctrlHideVideo";
			this.m_ctrlHideVideo.Size = new System.Drawing.Size(94, 24);
			this.m_ctrlHideVideo.TabIndex = 53;
			this.m_ctrlHideVideo.Text = "Disable Video";
			this.m_ctrlHideVideo.Click += new System.EventHandler(this.OnClickHideVideo);
			// 
			// m_ctrlHideText
			// 
			this.m_ctrlHideText.Location = new System.Drawing.Point(94, 33);
			this.m_ctrlHideText.Name = "m_ctrlHideText";
			this.m_ctrlHideText.Size = new System.Drawing.Size(93, 24);
			this.m_ctrlHideText.TabIndex = 52;
			this.m_ctrlHideText.Text = "Disable Text";
			// 
			// CTmaxVideoLinksCtrl
			// 
			this.Controls.Add(this.m_ctrlBottomPanel);
			this.Controls.Add(this.m_ctrlLinks);
			this.Name = "CTmaxVideoLinksCtrl";
			this.Size = new System.Drawing.Size(275, 137);
			this.m_ctrlBottomPanel.ResumeLayout(false);
			this.m_ctrlBottomPanel.PerformLayout();
			this.ResumeLayout(false);

		}
		
		/// <summary>Traps the event fired when the user selects a script</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		protected void OnLinksSelChanged(object sender, System.EventArgs e)
		{
			if(m_bIgnoreEvents == false)
			{
				//	Make this the active link
				if(m_ctrlLinks.SelectedItems.Count > 0)
				{
					//	Are we restoring the previous selection?
					if(m_bRestoringSelection == true)
					{
						//	MUST call this again to put the selection back to where we
						//	want it to be (don't understand this event chain)
						SetListSelection(m_xmlLink);
						
						m_bRestoringSelection = false;
						return;
					}
					
					//	Is it ok to change the active link
					if((CanContinue() == true) && (m_bPlayingScript == false))
					{
						SetLink((CXmlLink)(m_ctrlLinks.SelectedItems[0].Tag));
					}
					else
					{
						m_bRestoringSelection = true;
						SetListSelection(m_xmlLink);
					}
					
				}
				else if(m_ctrlLinks.Items.Count == 0)
				{
					SetLink(null);
				}
				
			}
			
		}// protected void OnLinksSelChanged(object sender, System.EventArgs e)

		/// <summary>Traps the event fired when the user clicks on Add Link</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		protected void OnAddLink(object sender, System.EventArgs e)
		{
			CXmlLink xmlLink = null;
			CTmaxVideoCtrlEventArgs Args = null;
			
			Debug.Assert(m_xmlDesignation != null);
			if(m_xmlDesignation == null) return;
			
			//	Create a new link
			xmlLink = new CXmlLink();
			
			//	Get the values
			if(Validate(xmlLink, true) == true)
			{
				//	Fire an event to request the addition
				Args = new CTmaxVideoCtrlEventArgs();
				Args.EventId = TmaxVideoCtrlEvents.AddLink;
				Args.XmlDesignation = m_xmlDesignation;
				Args.XmlLink = xmlLink;
				
				FireTmaxVideoCtrlEvent(Args);
			}
		
		}// protected void OnAddLink(object sender, System.EventArgs e)

		/// <summary>This method fires the event to inform the system when the transcript associated with the player position changes</summary>
		/// <param name="xmlLink">The new link object</param>
		protected void FireLinkChanged(CXmlLink xmlLink)
		{
			int iIndex = -1;
			
			//	Get the index of this link in the designation's collection
			if((xmlLink != null) && (m_xmlDesignation != null) && (m_xmlDesignation.Links != null))
				iIndex = m_xmlDesignation.Links.IndexOf(xmlLink);

			//	Create the event argument
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			Args.EventId         = TmaxVideoCtrlEvents.SetLink;
			Args.XmlDesignation  = m_xmlDesignation;
			Args.LinkIndex       = iIndex;
			Args.XmlLink         = xmlLink;
			
			//	Fire the event
			FireTmaxVideoCtrlEvent(Args);
		
		}// protected void FireLinkChanged(CXmlLink xmlLink)

		/// <summary>Fires the event to apply the changes</summary>
		protected void FireApply()
		{
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			//	Fire the video event
			Args.EventId = TmaxVideoCtrlEvents.Apply;
			FireTmaxVideoCtrlEvent(Args);
			
		}// protected void FireApply()

		/// <summary>Traps the event fired when the user drags over the barcode edit box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		protected void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if(QueryLinkDropBarcode().Length > 0)
				e.Effect = 	DragDropEffects.Link;
		}

		/// <summary>Traps the event fired when the user drops on the barcode edit box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		protected void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			string strBarcode = "";
			
			strBarcode = QueryLinkDropBarcode();
			
			if(strBarcode.Length > 0)
			{
				e.Effect = DragDropEffects.Link;
				m_ctrlBarcode.Text = strBarcode;
			}

		}// protected void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)

		/// <summary>Traps the event fired when the user clicks the Hide Link check box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		protected void OnClickHideLink(object sender, EventArgs e)
		{
			SetControlStates(false);
		}

		/// <summary>Traps the event fired when the user clicks the Hide Video check box</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		protected void OnClickHideVideo(object sender, EventArgs e)
		{
			SetControlStates(false);
		}

		#endregion Protected Methods

		/// <summary>This class implements a sorter interface that can be used to maintain sorted links</summary>
		protected class CLinksViewSorter : IComparer
		{
			#region Public Methods
		
			/// <summary>Default constructor</summary>
			public CLinksViewSorter()
			{
			}
		
			/// <summary>This method is called to compare two links</summary>
			/// <param name="x">First link to be compared</param>
			/// <param name="y">Second link to be compared</param>
			/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
			int IComparer.Compare(object x, object y) 
			{
				return Compare((ListViewItem)x, (ListViewItem)y);
			}

			#endregion Public Methods
		
			#region Protected Methods
		
			/// <summary>This function is called to compare two links</summary>
			/// <param name="xmlLink1">First link to be compared</param>
			/// <param name="xmlLink2">Second link to be compared</param>
			/// <returns>-1 if lvItem1 less than lvItem2, 0 if equal, 1 if lvItem1 greater than lvItem2</returns>
			protected int Compare(ListViewItem lvItem1, ListViewItem lvItem2)
			{
				int	iReturn = -1;
				CXmlLink xmlLink1 = null;
				CXmlLink xmlLink2 = null;
			
				try
				{
					if((xmlLink1 = (CXmlLink)lvItem1.Tag) == null)
						return -1;
					if((xmlLink2 = (CXmlLink)lvItem2.Tag) == null)
						return -1;
						
					//	Check for equality first
					//
					//	NOTE:	.NET raises and exception if we don't return 0 for
					//			equal objects
					if(ReferenceEquals(xmlLink1, xmlLink2) == true)
					{
						iReturn = 0;
					}
					else
					{
						if(xmlLink1.Start == xmlLink2.Start)
							iReturn = 0;
						else if(xmlLink1.Start > xmlLink2.Start)
							iReturn = 1;
						else
							iReturn = -1;
					
					}// if(ReferenceEquals(xmlLink1, xmlLink2) == true)
		
				}
				catch
				{
					iReturn = -1;
				}
			
				return iReturn;
		
			}// protected virtual int Compare(CXmlLink xmlLink1, CXmlLink xmlLink2)
		
		
			#endregion Protected Methods

		}// public class CLinksViewSorter : IComparer

	}// public class CTmaxVideoLinksCtrl

}// namespace FTI.Trialmax.Controls
