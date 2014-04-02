using System;
using System.Diagnostics;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>This class contains the arguments for a TrialMax ActiveX event</summary>
	public class CTmxRequest
	{
		#region Private Members
		
		/// <summary>Local member bound to Action property</summary>
		private TmxActions m_eAction = TmxActions.None;

		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";

		/// <summary>Local member bound to Page property</summary>
		private long m_lPage = 0;

		/// <summary>Local member bound to Start property</summary>
		private double m_dStart = (double)-1;

		/// <summary>Local member bound to Stop property</summary>
		private double m_dStop = (double)-1;

		/// <summary>Local member bound to Position property</summary>
		private long m_lPosition = 0;

		#endregion Private Members

		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmxRequest()
		{
			
		}// public CTmxRequest()

		/// <summary>Overloaded constructor</summary>
		public CTmxRequest(TmxActions eAction)
		{
			m_eAction = eAction;
			
		}// public CTmxRequest()

		#endregion Public Methods

		#region Properties
		
		/// <summary>This is the action identifier</summary>
		public TmxActions Action
		{
			get
			{
				return m_eAction;
			}
			set
			{
				m_eAction = value;
			}
		
		}// Action Property
		
		/// <summary>This is the Page page</summary>
		public long Page
		{
			get
			{
				return m_lPage;
			}
			set
			{
				m_lPage = value;
			}
		
		}// Page Property
		
		/// <summary>This is the Start time</summary>
		public double Start
		{
			get
			{
				return m_dStart;
			}
			set
			{
				m_dStart = value;
			}
		
		}// Start Property
		
		/// <summary>This is the Stop time</summary>
		public double Stop
		{
			get
			{
				return m_dStop;
			}
			set
			{
				m_dStop = value;
			}
		
		}// Stop Property
		
		/// <summary>This is the Position position</summary>
		public long Position
		{
			get
			{
				return m_lPosition;
			}
			set
			{
				m_lPosition = value;
			}
		
		}// Position Property
		
		/// <summary>Name of the file associated with the event</summary>
		public string Filename
		{
			get
			{
				return m_strFilename;
			}
			set
			{
				m_strFilename = value;
			}
		
		}// Filename Property
		
		#endregion Properties

	}// public class CTmxRequest

}// namespace FTI.Trialmax.ActiveX
