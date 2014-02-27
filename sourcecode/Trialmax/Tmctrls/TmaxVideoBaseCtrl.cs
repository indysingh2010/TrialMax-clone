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
	#region Globals
		
	//	Video tuning mode identifiers
	public enum TmaxVideoCtrlTuneModes
	{
		None = 0,
		Start,
		Link,
		Stop,
	}
		
	//	Video playback states
	public enum TmaxVideoCtrlStates
	{
		Invalid = 0,
		NotReady,
		Loaded,
		Playing,
		Paused,
		Stopped,
	}
	
	/// <summary>Event identifiers</summary>
	public enum TmaxVideoCtrlEvents
	{
		Invalid = 0,
		PlayerPositionChanged,
		PlayerStateChanged,
		PlayerDesignationComplete,
		PlayerTranscriptChanged,
		PlayerLinkChanged,
		ScriptDesignationComplete,
		ScriptTranscriptChanged,
		ScriptLinkChanged,
		SetMode,
		SetPreviewPeriod,
		SetLink,
		AddLink,
		Apply,
		QueryPlayerPosition,
		QueryLinkSourceDbId,
		QueryCanContinue,
		QueryLinkDropBarcode,
		EditDesignationExtents,
		EditDesignationText,
	}
		
	/// <summary>This is the delegate used to handle command events fired by this control</summary>
	/// <param name="sender">Object firing the event</param>
	/// <param name="e">Event arguments</param>
	public delegate void TmaxVideoCtrlHandler(object sender, CTmaxVideoCtrlEventArgs e);
		
	#endregion Globals
	
	/// <summary>This is the base class for TrialMax video controls</summary>
	public class CTmaxVideoBaseCtrl : System.Windows.Forms.UserControl
	{
		/// <summary>Fired by the control to set the active link</summary>
		public event TmaxVideoCtrlHandler TmaxVideoCtrlEvent;		
	
		#region Constants
		
		protected const double TMAXVIDEO_MAX_POSITION_TOLERANCE = 0.02;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bound to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to ClassicLinks property</summary>
		protected bool m_bClassicLinks = true;

		/// <summary>Local member bound to FileSpec property</summary>
		protected string m_strFileSpec = "";
		
		/// <summary>Local member bound to XmlDesignation property</summary>
		protected CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member bound to XmlLink property</summary>
		protected CXmlLink m_xmlLink = null;
		
		/// <summary>Local member to reference the collection of script designations</summary>
		protected CXmlDesignations m_xmlScriptDesignations = null;
		
		/// <summary>Local member to indicate if script should be played to the end</summary>
		protected bool m_bPlayScriptToEnd = false;
		
		/// <summary>Local member to indicate if script is being played</summary>
		protected bool m_bPlayingScript = false;
		
		/// <summary>Local member to store the index of the active script designation</summary>
		protected int m_iScriptIndex = -1;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoBaseCtrl()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Video Base Control";
			
			//	Populate the error builder's format string collection
			SetErrorStrings();
		
			//	NOTE:	It is up to the derived class to call InitializeComponent()
			//			in it's constructor. This ensures that the derived constructor
			//			gets called before InitializeComponent()
		}
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
		/// <param name="xmlDesignation">The designation used to set property values</param>
		/// <param name="xmlLink">The link used to set property values</param>
		/// <returns>true if successful</returns>
		public virtual bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		{
			bool bSuccessful = true;
			
			if(SetProperties(strFileSpec, xmlDesignation) == false)
				bSuccessful = false;
				
			if(SetProperties(xmlLink) == false)
				bSuccessful = false;
			
			return bSuccessful;
		}
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
		/// <param name="xmlDesignation">The designation used to set property values</param>
		/// <returns>true if successful</returns>
		public virtual bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		{
			m_strFileSpec = strFileSpec;
			m_xmlDesignation = xmlDesignation;
			return true;
		}
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="xmlLink">The link used to set property values</param>
		/// <returns>true if successful</returns>
		public virtual bool SetProperties(CXmlLink xmlLink)
		{
			m_xmlLink = xmlLink;
			return true;
		}
		
		/// <summary>This method is called to get the derived class property values and use them to set the designation attributes</summary>
		/// <param name="xmlDesignation">The designation to be updated with the current property values</param>
		/// <param name="xmlLink">The link to be updated with the current property values</param>
		/// <returns>true if successful</returns>
		public virtual bool SetAttributes(CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		{
			bool bSuccessful = true;
			
			if(SetAttributes(xmlDesignation) == false)
				bSuccessful = false;
			if(SetAttributes(xmlLink) == false)
				bSuccessful = false;
				
			return bSuccessful;
			
		}// public virtual bool SetAttributes(CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		
		/// <summary>This method is called to get the derived class property values and use them to set the designation attributes</summary>
		/// <param name="xmlDesignation">The designation to be updated with the current property values</param>
		/// <returns>true if successful</returns>
		public virtual bool SetAttributes(CXmlDesignation xmlDesignation)
		{
			if(m_xmlDesignation != null)
			{
				Debug.Assert(ReferenceEquals(xmlDesignation, m_xmlDesignation) == true);
				if(ReferenceEquals(xmlDesignation, m_xmlDesignation) == false) return false;
			}
			return true;
		}
		
		/// <summary>This method is called to get the derived class property values and use them to set the link attributes</summary>
		/// <param name="xmlLink">The link to be updated with the current property values</param>
		/// <returns>true if successful</returns>
		public virtual bool SetAttributes(CXmlLink xmlLink)
		{
			return true;
		}
		
		/// <summary>This method is called when the attributes associated with the active designation have changed</summary>
		/// <param name="xmlDesignation">The designation who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public virtual bool OnAttributesChanged(CXmlDesignation xmlDesignation)
		{
			if(m_xmlDesignation != null)
			{
				Debug.Assert(ReferenceEquals(xmlDesignation, m_xmlDesignation) == true);
				if(ReferenceEquals(xmlDesignation, m_xmlDesignation) == false) return false;
			}

			return true;
		}
		
		/// <summary>This method is called when the attributes associated with the active link have changed</summary>
		/// <param name="xmlLink">The link who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public virtual bool OnAttributesChanged(CXmlLink xmlLink)
		{
			if(m_xmlLink != null)
			{
				Debug.Assert(ReferenceEquals(m_xmlLink, xmlLink) == true);
			}
			return true;
		}
		
		/// <summary>This method is called to determine if modifications have been made to the active designation</summary>
		/// <param name="xmlDesignation">The active designation</param>
		///	<param name="aModifications">An array in which to put the description of all modifications</param>
		/// <returns>true if modified</returns>
		public virtual bool IsModified(CXmlDesignation xmlDesignation, ArrayList aModifications)
		{
			if(m_xmlDesignation != null)
			{
				Debug.Assert(ReferenceEquals(xmlDesignation, m_xmlDesignation) == true);
				if(ReferenceEquals(xmlDesignation, m_xmlDesignation) == false) return false;
			}
			
			return false;
		}
		
		/// <summary>This method is called to determine if modifications have been made to the active link</summary>
		/// <param name="xmlLink">The active link</param>
		///	<param name="aModifications">An array in which to put the description of all modifications</param>
		/// <returns>true if modified</returns>
		public virtual bool IsModified(CXmlLink xmlLink, ArrayList aModifications)
		{
			if(m_xmlLink != null)
			{
				Debug.Assert(ReferenceEquals(xmlLink, m_xmlLink) == true);
				if(ReferenceEquals(xmlLink, m_xmlLink) == false) return false;
			}
			
			return false;
		}
		
		/// <summary>This method is called to see if any changes have been made to the active objects</summary>
		///	<param name="aModifications">An array in which to put the description of all modifications</param>
		/// <returns>true if changes have been made</returns>
		public virtual bool IsModified(ArrayList aModifications)
		{
			if((m_xmlDesignation != null) && (IsModified(m_xmlDesignation, aModifications) == true))
				return true;
			else if((m_xmlLink != null) && (IsModified(m_xmlLink, aModifications) == true))
				return true;
			else
				return false;		
		}
			
		/// <summary>This method is called to determine if it is OK to change the active designation and/or link</summary>
		/// <returns>true if OK to continue</returns>
		public virtual bool CanContinue()
		{
			return CanContinue(true,true);	
		}

		/// <summary>This method is called to set the value of the ClassicLinks property</summary>
		public virtual void SetClassicLinks(bool bClassicLinks)
		{
			m_bClassicLinks = bClassicLinks;
		}

		/// <summary>This method is called to determine if it is OK to change the active designation and/or link</summary>
		///	<param name="bCheckDesignation">true to check active designation</param>
		/// <param name="bCheckLink">true to check the active link</param>
		/// <returns>true if OK to continue</returns>
		public virtual bool CanContinue(bool bCheckDesignation, bool bCheckLink)
		{
			//	Construct the argument object
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			Args.EventId = TmaxVideoCtrlEvents.QueryCanContinue;
			Args.CheckDesignation = bCheckDesignation;
			Args.CheckLink = bCheckLink;
				
			//	Fire the event
			FireTmaxVideoCtrlEvent(Args);
				
			//	Was it processed?
			if(Args.QueryHandled == true)
			{
				return Args.CanContinue;
			}
			else
			{
				//	Nobody cares so go ahead and continue
				return true;
			}		
		
		}// public virtual bool CanContinue()
			
		/// <summary>This method is called to play the specified collection of designations</summary>
		///	<param name="xmlDesignations">the collection of designations that define the script</param>
		///	<param name="iFirst">the index of the designation to start with</param>
		/// <param name="bPlayToEnd">true to play to end</param>
		/// <returns>true if successful</returns>
		public virtual bool SetScript(CXmlDesignations xmlDesignations, int iFirst, bool bPlayToEnd)
		{
			//	Update the local members
			m_xmlScriptDesignations = xmlDesignations;
			m_iScriptIndex          = iFirst < 0 ? 0 : iFirst;
			m_bPlayScriptToEnd      = bPlayToEnd;
			
			//	Make sure the start index is within range
			if(m_xmlScriptDesignations != null)
			{ 
				if(iFirst >= m_xmlScriptDesignations.Count)
					iFirst = 0;
			}

			return true;
			
		}// public virtual bool SetScript(CXmlDesignations xmlDesignations, int iFirst, bool bPlayToEnd)
		
		/// <summary>This method is called when the user wants to start playing a script</summary>
		/// <returns>true if successful</returns>
		public virtual bool StartScript()
		{
			m_bPlayingScript = true;
			return true;
							
		}// public virtual bool StartScript()
		
		/// <summary>This method is called when the user wants to stop playing a script</summary>
		/// <returns>true if successful</returns>
		public virtual bool StopScript()
		{
			m_bPlayingScript = false;
			return true;
			
		}// public virtual bool StopScript()
		
		/// <summary>This method is called to compare two playback positions</summary>
		/// <param name="dPos1">The first playback position</param>
		/// <param name="dPos2">The second playback position</param>
		/// <returns>-1 if dPos1 less than dPos2, 0 if equal, 1 if dPos1 greater than dPos2</returns>
		///	<remarks>This method applies the allowable tolerance to determine equality</remarks>
		public int ComparePositions(double dPos1, double dPos2)
		{
			//	Are the positions within tolerance?
			if(System.Math.Abs(dPos2 - dPos1) <= TMAXVIDEO_MAX_POSITION_TOLERANCE)
				return 0;
			else
				return (dPos1 < dPos2) ? -1 : 1;
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to determine if the active designation is associated with a movie clip</summary>
		/// <returns>true if the active designation represents a movie clip</returns>
		protected virtual bool IsMovieClip()
		{
			//	Is this a clip as opposed to a transcript designation?
			if((m_xmlDesignation != null) && (m_xmlDesignation.HasText == false))
				return true;
			else
				return false;
				
		}// protected bool IsMovieClip()
		
		/// <summary>This method is called to determine if the active designation contains synchronized text</summary>
		/// <returns>true if the active designation is synchronized</returns>
		protected virtual bool IsSynchronized()
		{
			//	We only worry about synchronization for depositions
			if(IsMovieClip() == false)
			{
				if(m_xmlDesignation != null)
					return m_xmlDesignation.GetSynchronized(false);
				else
					return false;
			}
			else
			{
				return true;
			}
				
		}// protected virtual bool IsSynchronized()
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing == true)
			{
				if(m_tmaxErrorBuilder != null)
				{
					if(m_tmaxErrorBuilder.FormatStrings != null)
						m_tmaxErrorBuilder.FormatStrings.Clear();
						
					m_tmaxErrorBuilder = null;
				}
			
			}// if(disposing == true)
				
		}// protected override void Dispose(bool disposing)

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected virtual void InitializeComponent()
		{
			// 
			// CTmaxVideoBaseCtrl
			// 
			this.Name = "CTmaxVideoBaseCtrl";
			this.Size = new System.Drawing.Size(196, 188);

		}
		
		/// <summary>This function handles all Resize events</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnResize(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnResize(e);
			
			//	Make sure the controls are properly sized
			RecalcLayout();
			
		}// protected override void OnResize(System.EventArgs e)

		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);
			
			//	Make sure the controls are properly sized
			RecalcLayout();
		}

		/// <summary>This function is notify the control that the parent window has been moved</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		public virtual void OnParentMoved()
		{
		}
		
		/// <summary>This method is called to fire a TrialMax video event</summary>
		/// <param name="Args">The event arguments</param>
		///	<returns>true if successful</returns>
		protected bool FireTmaxVideoCtrlEvent(CTmaxVideoCtrlEventArgs Args)
		{
			if(TmaxVideoCtrlEvent != null)
			{
				try
				{
					TmaxVideoCtrlEvent(this, Args);
					return true;
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "FireTmaxVideoCtrlEvent", "An exception was raised while firing the TrialMax video event: event = " + Args.EventId.ToString(), Ex);
				}
				
			}
			
			//	Something went wrong
			return false;
		
		}// protected bool FireTmaxVideoCtrlEvent(CTmaxVideoCtrlEventArgs Args)

		/// <summary>This method is called to handle a TrialMax video playback (TmxVideoEvent) event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">event arguments</param>
		public virtual void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
		{
			//	Which event?
			switch(e.EventId)
			{
				case TmaxVideoCtrlEvents.PlayerPositionChanged:
				case TmaxVideoCtrlEvents.SetMode:
				case TmaxVideoCtrlEvents.SetPreviewPeriod:
				case TmaxVideoCtrlEvents.Apply:
				case TmaxVideoCtrlEvents.QueryPlayerPosition:
				default:
				
					break;
					
			}
			
		}// protected virtual void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
	
		/// <summary>This method is called to bubble (propagate) the video event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">event arguments</param>
		protected virtual void BubbleTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
		{
			if(TmaxVideoCtrlEvent != null)
				TmaxVideoCtrlEvent(sender, e);
			
		}// protected virtual void BubbleTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
	
		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected virtual void RecalcLayout()
		{
		}
			
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

		}// protected void SetErrorStrings()

		/// <summary>This method is called to get the position associated with the current tune mode</summary>
		/// <returns>The associated position</returns>
		protected virtual double GetModePosition(TmaxVideoCtrlTuneModes eMode)
		{
			switch(eMode)
			{
				case TmaxVideoCtrlTuneModes.Start:
					return StartPosition;
				case TmaxVideoCtrlTuneModes.Stop:
					return StopPosition;
				case TmaxVideoCtrlTuneModes.Link:
					return LinkPosition;
				default:
					return -1;
			}
			
		}// protected double GetModePosition()
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
			
		}// EventSource property
		
		/// <summary>Fully qualified path to the active video file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		
		}// FileSpec

		/// <summary>Flag to indicate if classic video deposition links are in use</summary>
		public bool ClassicLinks
		{
			get { return m_bClassicLinks; }
			set { SetClassicLinks(value); }

		}// ClassicLinks

		/// <summary>The active XML designation</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return m_xmlDesignation; }
		
		}// XmlDesignation
		
		/// <summary>The active XML link</summary>
		public CXmlLink XmlLink
		{
			get { return m_xmlLink; }
		
		}// XmlLink
		
		/// <summary>Flag to indicate if script is being played</summary>
		public bool PlayingScript
		{
			get { return m_bPlayingScript; }
		
		}// PlayingScript
		
		/// <summary>Position from which to start the playback</summary>
		public double StartPosition
		{
			get
			{
				if(m_xmlDesignation != null)
					return m_xmlDesignation.Start; 
				else
					return -1;
			}
			set 
			{ 
				if(m_xmlDesignation != null)
					m_xmlDesignation.Start = value;
			}
		
		}// StartPosition
		
		/// <summary>Position from which to start the playback</summary>
		public double StopPosition
		{
			get
			{
				if(m_xmlDesignation != null)
				{
					//	It's possible (i.e. no sync information in the log file) for the
					//	stop and start positions to be the same. This test makes an
					//	the designation play like a normal synchronized designation
					if(m_xmlDesignation.Stop > m_xmlDesignation.Start)
						return m_xmlDesignation.Stop; 
					else
						return (m_xmlDesignation.Start + TMAXVIDEO_MAX_POSITION_TOLERANCE);
				}
				else
				{
					return -1;
				}
			
			}
			set 
			{ 
				if(m_xmlDesignation != null)
					m_xmlDesignation.Stop = value;
			}
		
		}// StopPosition
		
		/// <summary>Position at which the link occurs</summary>
		public double LinkPosition
		{
			get
			{
				if(m_xmlLink != null)
					return m_xmlLink.Start;
				else
					return -1;
			}
			set 
			{ 
				if(m_xmlLink != null)
					m_xmlLink.Start = value;
			}
		
		}// LinkPosition
		
		#endregion Properties
		
	}// public class CTmaxVideoBaseCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls
