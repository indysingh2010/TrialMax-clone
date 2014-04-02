using System;
using System.Diagnostics;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>This is the delegate used to handle all viewer events</summary>
	/// <param name="objSender">Object firing the event</param>
	/// <param name="Args">Object containing the event arguments</param>
	public delegate void TmxEventHandler(object objSender, CTmxEventArgs Args);
		
	/// <summary>This is the delegate used to handle all viewer events</summary>
	/// <param name="objSender">Object firing the event</param>
	public delegate void TmxShareEventHandler(object objSender);
		
	/// <summary>This class contains the arguments for a TrialMax ActiveX event</summary>
	public class CTmxEventArgs
	{
		#region Private Members
		
		/// <summary>Local member bound to Event property</summary>
		private TmxEvents m_eEvent = TmxEvents.Action;

		/// <summary>Local member bound to State property</summary>
		private TmxStates m_eState = TmxStates.Invalid;

		/// <summary>Local member bound to Action property</summary>
		private TmxActions m_eAction = TmxActions.None;

		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";

		/// <summary>Local member bound to PageFilename property</summary>
		private string m_strPageFilename = "";

		/// <summary>Local member bound to CalloutCount property</summary>
		private int m_iCalloutCount = 0;

		/// <summary>Local member bound to Position property</summary>
		private double m_dPosition = 0.0;

		/// <summary>Local member accessed by the Start property</summary>
		protected double m_dStart = -1.0;
		
		/// <summary>Local member accessed by the Stop property</summary>
		protected double m_dStop = -1.0;
		
		/// <summary>Local member accessed by the Duration property</summary>
		protected double m_dDuration = 0.0;
		
		/// <summary>Local member accessed by the Continue property</summary>
		protected bool m_bContinue = false;
		
		/// <summary>Local member accessed by the GoTo property</summary>
		protected int m_iGoTo = 0;
		
		/// <summary>Local member accessed by the PageNumber property</summary>
		protected int m_iPageNumber = 0;
		
		/// <summary>Local member accessed by the TotalPages property</summary>
		protected int m_iTotalPages = 0;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmxEventArgs()
		{
			
		}// public CTmxEventArgs()

		/// <summary>Overloaded constructor</summary>
		/// <param name="eEvent">Enumerated event identifier</param>
		/// <param name="tmxBase">Base pane used to initialize the properties</param>
		public CTmxEventArgs(TmxEvents eEvent, CTmxBase tmxBase)
		{
			m_eEvent = eEvent;
			
			if(tmxBase != null)
				SetProperties(tmxBase);
			
		}// public CTmxEventArgs()

		/// <summary>Overloaded constructor</summary>
		public CTmxEventArgs(TmxEvents eEvent)
		{
			m_eEvent = eEvent;
			
		}// public CTmxEventArgs()

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmxBase">Base pane used to initialize the properties</param>
		public CTmxEventArgs(CTmxBase tmxBase)
		{
			if(tmxBase != null)
				SetProperties(tmxBase);
			
		}// public CTmxEventArgs()

		/// <summary>Sets the property values using the ActiveX object</summary>
		public void SetProperties(CTmxBase tmxBase)
		{
			Debug.Assert(tmxBase != null);
			if(tmxBase == null) return;
			
			Filename = tmxBase.Filename;
			Position = tmxBase.TmxPosition;
			Start = tmxBase.TmxStart;
			Stop = tmxBase.TmxStop;
			Duration = tmxBase.TmxDuration;
			State = tmxBase.TmxState;
			
		}// public SetProperties()

		#endregion Public Methods

		#region Properties
		
		/// <summary>This is the event identifier</summary>
		public TmxEvents Event
		{
			get	{ return m_eEvent; }
			set { m_eEvent = value; }
		
		}// Event Property
		
		/// <summary>This is the event identifier</summary>
		public TmxActions Action
		{
			get	{ return m_eAction; }
			set { m_eAction = value; }
		
		}// Action Property
		
		/// <summary>This is the playback position</summary>
		public double Position
		{
			get	{ return m_dPosition; }
			set { m_dPosition = value; }
		
		}// Position Property
		
		/// <summary>This is the playback start position</summary>
		public double Start
		{
			get { return m_dStart; }
			set { m_dStart = value; }
		
		}// Start Property
		
		/// <summary>This is the playback stop position</summary>
		public double Stop
		{
			get	{ return m_dStop; }
			set { m_dStop = value; }
		
		}// Stop Property
		
		/// <summary>This is the playback duration</summary>
		public double Duration
		{
			get	{ return m_dDuration; }
			set	{ m_dDuration = value; }
		
		}// Duration Property
		
		/// <summary>This is the position for GoTo actions</summary>
		public int GoTo
		{
			get	{ return m_iGoTo; }
			set	{ m_iGoTo = value; }
		
		}// GoTo Property
		
		public TmxStates State
		{
			get	{ return m_eState; }
			set { m_eState = value; }
		
		}// State Property
		
		/// <summary>Name of the file associated with the event</summary>
		public string Filename
		{
			get	{ return m_strFilename; }
			set	{ m_strFilename = value; }
		
		}// Filename Property
		
		/// <summary>Name of the output file associated with the SavedPage event</summary>
		public string PageFilename
		{
			get	{ return m_strPageFilename; }
			set	{ m_strPageFilename = value; }
		
		}// PageFilename Property
		
		/// <summary>Page number associated with the SavedPage event</summary>
		public int PageNumber
		{
			get	{ return m_iPageNumber; }
			set	{ m_iPageNumber = value; }
		
		}// PageNumber Property
		
		/// <summary>Page count associated with the SavedPage event</summary>
		public int TotalPages
		{
			get	{ return m_iTotalPages; }
			set	{ m_iTotalPages = value; }
		
		}// TotalPages Property
		
		/// <summary>Number of callouts associated with active file</summary>
		public int CalloutCount
		{
			get	{ return m_iCalloutCount; }
			set	{ m_iCalloutCount = value; }
		
		}// CalloutCount Property
		
		/// <summary>Response to a QueryContinue event</summary>
		public bool Continue
		{
			get	{ return m_bContinue; }
			set	{ m_bContinue = value; }
		
		}// Continue Property
		
		#endregion Properties

	}// public class CTmxEventArgs

}// namespace FTI.Trialmax.ActiveX
