using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the options used for printing</summary>
	public class CTmaxCleanOptions
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "CleanOptions";
		private const string XMLINI_PROMPT_BEFORE_SAVE_KEY		= "PromptBeforeSave";
		private const string XMLINI_DESKEW_KEY					= "Deskew";
		private const string XMLINI_DESPECKLE_KEY				= "Despeckle";
		private const string XMLINI_REMOVE_HOLE_PUNCHES_KEY		= "RemoveHolePunches";
		private const string XMLINI_MIN_HOLE_PUNCH_COUNT_KEY	= "MinimumHolePunchCount";
		private const string XMLINI_MAX_HOLE_PUNCH_COUNT_KEY	= "MaximumHolePunchCount";
		private const string XMLINI_HOLE_PUNCH_LOCATIONS_KEY	= "HolePunchLocations";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Deskew property</summary>
		private bool m_bDeskew = true;
		
		/// <summary>Local member bound to Despeckle property</summary>
		private bool m_bDespeckle = true;
		
		/// <summary>Local member bound to RemoveHolePunches property</summary>
		private bool m_bRemoveHolePunches = false;
		
		/// <summary>Local member bound to PromptBeforeSave property</summary>
		private bool m_bPromptBeforeSave = false;
		
		/// <summary>Local member bound to MinimumHolePunchCount property</summary>
		private int m_iMinimumHolePunchCount = 3;
		
		/// <summary>Local member bound to MaximumHolePunchCount property</summary>
		private int m_iMaximumHolePunchCount = 3;
		
		/// <summary>Local member bound to HolePunchLocations property</summary>
		private int m_iHolePunchLocations = (int)TmaxHolePunchLocations.Left;
		
		/// <summary>Local member bound to Rotation property</summary>
		private TmaxRotations m_eRotation = TmaxRotations.None;
		
		#endregion Private Members
	
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxCleanOptions()
		{
		}
		
		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		public void Load(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_bPromptBeforeSave = xmlIni.ReadBool(XMLINI_PROMPT_BEFORE_SAVE_KEY, m_bPromptBeforeSave);
			m_bDeskew = xmlIni.ReadBool(XMLINI_DESKEW_KEY, m_bDeskew);
			m_bDespeckle = xmlIni.ReadBool(XMLINI_DESPECKLE_KEY, m_bDespeckle);
			m_bRemoveHolePunches = xmlIni.ReadBool(XMLINI_REMOVE_HOLE_PUNCHES_KEY, m_bRemoveHolePunches);
			m_iMinimumHolePunchCount = xmlIni.ReadInteger(XMLINI_MIN_HOLE_PUNCH_COUNT_KEY, m_iMinimumHolePunchCount);
			m_iMaximumHolePunchCount = xmlIni.ReadInteger(XMLINI_MAX_HOLE_PUNCH_COUNT_KEY, m_iMaximumHolePunchCount);
			m_iHolePunchLocations = xmlIni.ReadInteger(XMLINI_HOLE_PUNCH_LOCATIONS_KEY, m_iHolePunchLocations);
			
			//	NOTE:	We intentionally do NOT persist the Rotation property
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the option values</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_PROMPT_BEFORE_SAVE_KEY, m_bPromptBeforeSave);
			xmlIni.Write(XMLINI_DESKEW_KEY, m_bDeskew);
			xmlIni.Write(XMLINI_DESPECKLE_KEY, m_bDespeckle);
			xmlIni.Write(XMLINI_REMOVE_HOLE_PUNCHES_KEY, m_bRemoveHolePunches);
			xmlIni.Write(XMLINI_MIN_HOLE_PUNCH_COUNT_KEY, m_iMinimumHolePunchCount);
			xmlIni.Write(XMLINI_MAX_HOLE_PUNCH_COUNT_KEY, m_iMaximumHolePunchCount);
			xmlIni.Write(XMLINI_HOLE_PUNCH_LOCATIONS_KEY, m_iHolePunchLocations);

			//	NOTE:	We intentionally do NOT persist the Rotation property
			
		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Prompt for confirmation before saving</summary>
		public bool PromptBeforeSave
		{
			get { return m_bPromptBeforeSave; }
			set { m_bPromptBeforeSave = value; }
		}
		
		/// <summary>Deskew page when cleaning</summary>
		public bool Deskew
		{
			get { return m_bDeskew; }
			set { m_bDeskew = value; }
		}
		
		/// <summary>Despeckle when cleaning</summary>
		public bool Despeckle
		{
			get { return m_bDespeckle; }
			set { m_bDespeckle = value; }
		}
		
		/// <summary>Remove Hole Punches</summary>
		public bool RemoveHolePunches
		{
			get { return m_bRemoveHolePunches; }
			set { m_bRemoveHolePunches = value; }
		}
		
		/// <summary>Minumum number of hole punches to remove</summary>
		public int MinimumHolePunchCount
		{
			get { return m_iMinimumHolePunchCount; }
			set { m_iMinimumHolePunchCount = value; }
		}
		
		/// <summary>Maximum number of hole punches to remove</summary>
		public int MaximumHolePunchCount
		{
			get { return m_iMaximumHolePunchCount; }
			set { m_iMaximumHolePunchCount = value; }
		}
		
		/// <summary>Location of hole punches</summary>
		public int HolePunchLocations
		{
			get { return m_iHolePunchLocations; }
			set { m_iHolePunchLocations = value; }
		}
		
		/// <summary>Despeckle when cleaning</summary>
		public TmaxRotations Rotation
		{
			get { return m_eRotation; }
			set { m_eRotation = value; }
		}
		
		#endregion Properties
	
	}// public class CTmaxCleanOptions

}// namespace FTI.Shared.Trialmax
