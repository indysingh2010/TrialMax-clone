using System;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class is used to pass arguments and results in a TrialMax video event</summary>
	public class CTmaxVideoCtrlEventArgs
	{
		#region Private Members
		
		/// <summary>Local member bound to EventId property</summary>
		private TmaxVideoCtrlEvents m_eEventId = TmaxVideoCtrlEvents.Invalid;
		
		/// <summary>Local member bound to TuneMode property</summary>
		private TmaxVideoCtrlTuneModes m_eTuneMode = TmaxVideoCtrlTuneModes.None;
		
		/// <summary>Local member bound to State property</summary>
		private TmaxVideoCtrlStates m_eState = TmaxVideoCtrlStates.Invalid;
		
		/// <summary>Local member bound to Position property</summary>
		private double m_dPosition = -1.0;

		/// <summary>Local member bound to PreviewPeriod property</summary>
		private double m_dPreviewPeriod = -1.0;

		/// <summary>Local member bound to QueryHandled property</summary>
		private bool m_bQueryHandled = false;

		/// <summary>Local member bound to CanContinue property</summary>
		private bool m_bCanContinue = false;

		/// <summary>Local member bound to CheckDesignation property</summary>
		private bool m_bCheckDesignation = true;

		/// <summary>Local member bound to CheckLink property</summary>
		private bool m_bCheckLink = true;

		/// <summary>Local member bound to XmlDesignation property</summary>
		protected CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member bound to XmlLink property</summary>
		protected CXmlLink m_xmlLink = null;
		
		/// <summary>Local member bound to LinkIndex property</summary>
		protected int m_iLinkIndex = -1;
		
		/// <summary>Local member bound to XmlTranscript property</summary>
		protected CXmlTranscript m_xmlTranscript = null;
		
		/// <summary>Local member bound to TranscriptIndex property</summary>
		protected int m_iTranscriptIndex = -1;
		
		/// <summary>Local member bound to LinkDbId property</summary>
		protected string m_strLinkSourceDbId = "";
		
		/// <summary>Local member bound to LinkMediaId property</summary>
		protected string m_strLinkSourceMediaId = "";
		
		/// <summary>Local member bound to DropBarcode property</summary>
		protected string m_strLinkDropBarcode = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoCtrlEventArgs()
		{
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Event identifier</summary>
		public TmaxVideoCtrlEvents EventId
		{
			get { return m_eEventId; }
			set { m_eEventId = value; }
		}
		
		/// <summary>Playback state identifier</summary>
		public TmaxVideoCtrlStates State
		{
			get { return m_eState; }
			set { m_eState = value; }
		}
		
		/// <summary>Tune mode specified with the event</summary>
		public TmaxVideoCtrlTuneModes TuneMode
		{
			get { return m_eTuneMode; }
			set { m_eTuneMode = value; }
		}
		
		/// <summary>Position reported with the event</summary>
		public double Position
		{
			get { return m_dPosition; }
			set { m_dPosition = value; }
		}
		
		/// <summary>Duration of playback if previewing video while tuning</summary>
		public double PreviewPeriod
		{
			get { return m_dPreviewPeriod; }
			set { m_dPreviewPeriod = value; }
		}
		
		/// <summary>Flag to indicate if query event was handled</summary>
		public bool QueryHandled
		{
			get { return m_bQueryHandled; }
			set { m_bQueryHandled = value; }
		}
		
		/// <summary>Flag to indicate if OK to continue</summary>
		public bool CanContinue
		{
			get { return m_bCanContinue; }
			set { m_bCanContinue = value; }
		}
		
		/// <summary>True to check active designation when processing CanContinue event</summary>
		public bool CheckDesignation
		{
			get { return m_bCheckDesignation; }
			set { m_bCheckDesignation = value; }
		}
		
		/// <summary>True to check active designation when processing CanContinue event</summary>
		public bool CheckLink
		{
			get { return m_bCheckLink; }
			set { m_bCheckLink = value; }
		}
		
		/// <summary>Reference to XML designation object associated with the event</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return m_xmlDesignation; }
			set { m_xmlDesignation = value; }
		}
		
		/// <summary>Reference to XML transcript object associated with the event</summary>
		public CXmlTranscript XmlTranscript
		{
			get { return m_xmlTranscript; }
			set { m_xmlTranscript = value; }
		}
		
		/// <summary>Reference to XML link object associated with the event</summary>
		public CXmlLink XmlLink
		{
			get { return m_xmlLink; }
			set { m_xmlLink = value; }
		}
		
		/// <summary>Index of XML transcript object associated with the event</summary>
		public int TranscriptIndex
		{
			get { return m_iTranscriptIndex; }
			set { m_iTranscriptIndex = value; }
		}
		
		/// <summary>Index of XML link object associated with the event</summary>
		public int LinkIndex
		{
			get { return m_iLinkIndex; }
			set { m_iLinkIndex = value; }
		}
		
		/// <summary>Media ID assigned to the link's source object</summary>
		public string LinkSourceMediaId
		{
			get { return m_strLinkSourceMediaId; }
			set { m_strLinkSourceMediaId = value; }
		}
		
		/// <summary>Barcode of link for drag/drop operations</summary>
		public string LinkDropBarcode
		{
			get { return m_strLinkDropBarcode; }
			set { m_strLinkDropBarcode = value; }
		}
		
		/// <summary>PST database ID assigned to the link's source object</summary>
		public string LinkSourceDbId
		{
			get { return m_strLinkSourceDbId; }
			set { m_strLinkSourceDbId = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxVideoCtrlEventArgs
	
}// namespace FTI.Trialmax.Controls
