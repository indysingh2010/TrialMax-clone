using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class encapsulates the information required to highlight a objection in a transcript grid control</summary>
	public class CTmaxObjection : ITmaxSortable
	{
		#region Constants
		
		public const string TMAX_OBJECTION_ASCII_HEADER_UNIQUE_ID		= "UniqueId";
		public const string TMAX_OBJECTION_ASCII_HEADER_CASE_NAME		= "CaseName";
		public const string TMAX_OBJECTION_ASCII_HEADER_CASE_ID			= "CaseId";
		public const string TMAX_OBJECTION_ASCII_HEADER_DEPOSITION		= "Deposition";
		public const string TMAX_OBJECTION_ASCII_HEADER_PLAINTIFF		= "Plaintiff";
		public const string TMAX_OBJECTION_ASCII_HEADER_STATE			= "Status";
		public const string TMAX_OBJECTION_ASCII_HEADER_RULING			= "Ruling";
		public const string TMAX_OBJECTION_ASCII_HEADER_FIRST_PAGE		= "FirstPage";
		public const string TMAX_OBJECTION_ASCII_HEADER_FIRST_LINE		= "FirstLine";
		public const string TMAX_OBJECTION_ASCII_HEADER_LAST_PAGE		= "LastPage";
		public const string TMAX_OBJECTION_ASCII_HEADER_LAST_LINE		= "LastLine";
		public const string TMAX_OBJECTION_ASCII_HEADER_ARGUMENT		= "Objection";
		public const string TMAX_OBJECTION_ASCII_HEADER_RESPONSE_1		= "Response";
		public const string TMAX_OBJECTION_ASCII_HEADER_RESPONSE_2		= "Response2";
		public const string TMAX_OBJECTION_ASCII_HEADER_RESPONSE_3		= "Response3";
		public const string TMAX_OBJECTION_ASCII_HEADER_RULING_TEXT		= "RulingText";
		public const string TMAX_OBJECTION_ASCII_HEADER_WORK_PRODUCT	= "WorkProduct";
		public const string TMAX_OBJECTION_ASCII_HEADER_COMMENTS		= "Comments";

		public const int TMAX_OBJECTION_PROP_UNIQUE_ID		= 0;
		public const int TMAX_OBJECTION_PROP_DEPOSITION		= 1;
		public const int TMAX_OBJECTION_PROP_PARTY			= 2;
		public const int TMAX_OBJECTION_PROP_STATE			= 3;
		public const int TMAX_OBJECTION_PROP_RULING			= 4;
		public const int TMAX_OBJECTION_PROP_FIRST_PAGE		= 5;
		public const int TMAX_OBJECTION_PROP_FIRST_LINE		= 6;
		public const int TMAX_OBJECTION_PROP_LAST_PAGE		= 7;
		public const int TMAX_OBJECTION_PROP_LAST_LINE		= 8;
		public const int TMAX_OBJECTION_PROP_ARGUMENT		= 9;
		public const int TMAX_OBJECTION_PROP_RESPONSE_1		= 10;
		public const int TMAX_OBJECTION_PROP_RESPONSE_2		= 11;
		public const int TMAX_OBJECTION_PROP_RESPONSE_3		= 12;
		public const int TMAX_OBJECTION_PROP_RULING_TEXT	= 13;
		public const int TMAX_OBJECTION_PROP_WORK_PRODUCT	= 14;
		public const int TMAX_OBJECTION_PROP_COMMENTS		= 15;
		public const int TMAX_OBJECTION_PROP_CASE_NAME		= 16;
		public const int TMAX_OBJECTION_PROP_CASE_ID		= 17;

		//	Party identifiers
		public const string TMAX_OBJECTION_PARTY_PLAINTIFF = "Plaintiff";
		public const string TMAX_OBJECTION_PARTY_DEFENDANT = "Defendant";
		
		//	Predefined ruling identifiers
		public const string TMAX_OBJECTION_RULING_NONE = "Pending";

		//	Predefined state identifiers
		public const string TMAX_OBJECTION_STATE_PENDING_RESPONSE = "Pending Response";
		public const string TMAX_OBJECTION_STATE_PENDING_RULING = "Pending Ruling";

		#endregion Constants
		
		#region Private Members

		/// <summary>Private member bound to UniqueId property</summary>
		private string m_strUniqueId = "";

		/// <summary>Private member bound to DepositionId property</summary>
		private string m_strDepositionId = "";

		/// <summary>Local member bound to Case property</summary>
		private FTI.Shared.Trialmax.CTmaxCase m_tmaxCase = null;

		/// <summary>Private member bound to StateId property</summary>
		private string m_strStateId = "";

		/// <summary>Private member bound to RulingId property</summary>
		private string m_strRulingId = "";

		/// <summary>Private member bound to ModifiedById property</summary>
		private string m_strModifiedById = "";

		/// <summary>This member is bounded to the Plaintiff property</summary>
		private bool m_bPlaintiff = false;

		/// <summary>This member is bounded to the FirstPL property</summary>
		private long m_lFirstPL = 0;

		/// <summary>This member is bounded to the LastPL property</summary>
		private long m_lLastPL = 0;

		/// <summary>Private member bound to Deposition property</summary>
		private string m_strDeposition = "";

		/// <summary>Private member bound to State property</summary>
		private string m_strState = "";

		/// <summary>Private member bound to Ruling property</summary>
		private string m_strRuling = "";

		/// <summary>Private member bound to Argument property</summary>
		private string m_strArgument = "";

		/// <summary>Private member bound to Response1 property</summary>
		private string m_strResponse1 = "";

		/// <summary>Private member bound to Response2 property</summary>
		private string m_strResponse2 = "";

		/// <summary>Private member bound to Response3 property</summary>
		private string m_strResponse3 = "";

		/// <summary>Private member bound to WorkProduct property</summary>
		private string m_strWorkProduct = "";

		/// <summary>Private member bound to Comments property</summary>
		private string m_strComments = "";

		/// <summary>Private member bound to RulingText property</summary>
		private string m_strRulingText = "";

		/// <summary>Private member bound to ModifiedOn property</summary>
		private System.DateTime m_dtModifiedOn = System.DateTime.Now;

		/// <summary>Private member bound to ModifiedBy property</summary>
		private string m_strModifiedBy = "";

		/// <summary>Private member bound to IOxObjection property</summary>
		private ITmaxObjectionRecord m_IOxObjection = null;

		/// <summary>Private member bound to IOxDeposition property</summary>
		private ITmaxBaseObjectionRecord m_IOxDeposition = null;

		/// <summary>Private member bound to ICaseDeposition property</summary>
		private ITmaxDeposition m_ICaseDeposition = null;

		/// <summary>Private member bound to IOxState property</summary>
		private ITmaxBaseObjectionRecord m_IOxState = null;

		/// <summary>Private member bound to IOxRuling property</summary>
		private ITmaxBaseObjectionRecord m_IOxRuling = null;

		/// <summary>Private member bound to IOxModifiedBy property</summary>
		private ITmaxBaseObjectionRecord m_IOxModifiedBy = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CTmaxObjection()
		{
		}

		/// <summary>Constructor</summary>
		/// <param name="strUniqueId">The unique identifer assigned by the database</param>
		public CTmaxObjection(string strUniqueId)
		{
			m_strUniqueId = strUniqueId;
		}

		/// <summary>Copy Constructor</summary>
		/// <param name="tmaxObjection">The object to be copied</param>
		public CTmaxObjection(CTmaxObjection tmaxObjection)
		{
			if(tmaxObjection != null)
				Copy(tmaxObjection);
		}

		/// <summary>Called to determine if the specified value is a valid party</summary>
		/// <param name="strParty">The party identifier</param>
		/// <returns>true if a valid party identifier</returns>
		public bool IsParty(string strParty)
		{
			if(String.Compare(strParty, TMAX_OBJECTION_PARTY_DEFENDANT, true) == 0)
				return true;
			else if(String.Compare(strParty, TMAX_OBJECTION_PARTY_PLAINTIFF, true) == 0)
				return true;
			else
				return false;

		}// public bool IsParty(string strParty)

		/// <summary>Called to set the active party</summary>
		/// <param name="strParty">The party identifier</param>
		/// <returns>true if successful</returns>
		public bool SetParty(string strParty)
		{
			bool bSuccessful = true;

			if(String.Compare(strParty, TMAX_OBJECTION_PARTY_DEFENDANT, true) == 0)
				this.Plaintiff = false;
			else if(String.Compare(strParty, TMAX_OBJECTION_PARTY_PLAINTIFF, true) == 0)
				this.Plaintiff = true;
			else
				bSuccessful = false;
				
			return bSuccessful;

		}// public bool SetParty(string strParty)

		/// <summary>Called to set the active party identifier</summary>
		/// <returns>The current party</returns>
		public string GetParty()
		{
			if(this.Plaintiff == true)
				return TMAX_OBJECTION_PARTY_PLAINTIFF;
			else
				return TMAX_OBJECTION_PARTY_DEFENDANT;

		}// public string GetParty()

		/// <summary>Called to copy the property values of the specified object</summary>
		/// <param name="tmaxSource">The source object to be copied</param>
		public void Copy(CTmaxObjection tmaxSource)
		{
			this.m_strUniqueId = tmaxSource.m_strUniqueId;
			this.m_IOxObjection = tmaxSource.m_IOxObjection;
			this.m_strDepositionId = tmaxSource.m_strDepositionId;
			this.m_IOxDeposition = tmaxSource.m_IOxDeposition;
			this.m_ICaseDeposition = tmaxSource.m_ICaseDeposition;
			this.m_strState = tmaxSource.m_strState;
			this.m_strStateId = tmaxSource.m_strStateId;
			this.m_IOxState = tmaxSource.m_IOxState;
			this.m_strRuling = tmaxSource.m_strRuling;
			this.m_strRulingId = tmaxSource.m_strRulingId;
			this.m_IOxRuling = tmaxSource.m_IOxRuling;
			this.m_tmaxCase = tmaxSource.m_tmaxCase;
			this.m_bPlaintiff = tmaxSource.m_bPlaintiff;
			this.m_lFirstPL = tmaxSource.m_lFirstPL;
			this.m_lLastPL = tmaxSource.m_lLastPL;
			this.m_strComments = tmaxSource.m_strComments;
			this.m_strArgument = tmaxSource.m_strArgument;
			this.m_strResponse1 = tmaxSource.m_strResponse1;
			this.m_strResponse2 = tmaxSource.m_strResponse2;
			this.m_strResponse3 = tmaxSource.m_strResponse3;
			this.m_strRulingText = tmaxSource.m_strRulingText;
			this.m_strWorkProduct = tmaxSource.m_strWorkProduct;

		}// public void Copy(CTmaxObjection tmaxSource)

		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="tmaxCompare">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxCompare, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxObjection tmaxCompare, long lMode)
		{
			//	NOTE:	We are assuming all objections in a sorted collection
			//			belong to the same deposition
			
			//	NOTE:	Do NOT change this code without checking it's impact on the
			//			CTmaxTransGridCtrl::FillObjections() method

			//	Are the start positions equal?
			if(this.FirstPL == tmaxCompare.FirstPL)
			{
				//	Are the end positions equal?
				if(this.LastPL == tmaxCompare.LastPL)
					return 0;
				else
					return ((this.LastPL < tmaxCompare.LastPL) ? -1 : 1);
			}
			else
			{
				return ((this.FirstPL < tmaxCompare.FirstPL) ? -1 : 1);
			}

		}// public int Compare(CTmaxObjection tmaxCompare, long lMode)

		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxResult, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxObjection)O, lMode); }
			catch { return -1; }

		}// public int ITmaxSortable.Compare(ITmaxSortable O)

		/// <summary>Called to get the exchange interface for the Deposition record</summary>
		/// <returns>The exchange interface</returns>
		public ITmaxBaseObjectionRecord GetIOxDeposition()
		{
			//	Do we need to get the interface?
			if((m_IOxDeposition == null) && (m_IOxObjection != null) && (m_strDepositionId.Length > 0))
				m_IOxDeposition = m_IOxObjection.GetIOxDeposition();
			
			return m_IOxDeposition;

		}// public ITmaxBaseObjectionRecord GetOxDeposition()			

		/// <summary>Called to set the exchange interface for the Objection record</summary>
		/// <param name="IOxDeposition">The exchange interface</param>
		public void SetIOxObjection(ITmaxObjectionRecord IOxObjection)
		{
			if((m_IOxObjection = IOxObjection) != null)
			{
				if((m_IOxObjection.GetCase() != null) && (m_IOxObjection.GetCase().UniqueId.Length > 0))
					this.Case = m_IOxObjection.GetCase();
			}

		}// public void SetIOxObjection(ITmaxObjectionRecord IOxObjection)			

		/// <summary>Called to set the exchange interface for the Deposition record</summary>
		/// <param name="IOxDeposition">The exchange interface</param>
		public void SetIOxDeposition(ITmaxBaseObjectionRecord IOxDeposition)
		{
			if((m_IOxDeposition = IOxDeposition) != null)
			{
				m_strDepositionId = IOxDeposition.GetUniqueId();
				m_strDeposition = IOxDeposition.GetText();
			}
			else
			{
				m_strDepositionId = "";

				//	NOTE: We don't clear m_strDeposition because it may be set by CaseDeposition interface
			}

		}// public void SetOxDeposition(ITmaxBaseObjectionRecord IOxDeposition)			

		/// <summary>Called to set the exchange interface for the Deposition record in the case database</summary>
		/// <param name="ICaseDeposition">The exchange interface</param>
		public void SetICaseDeposition(ITmaxDeposition ICaseDeposition)
		{
			if((m_ICaseDeposition = ICaseDeposition) != null)
				m_strDeposition = ICaseDeposition.GetMediaId();

			//	NOTE: We don't clear m_strDeposition because it may be set by OxDeposition interface

		}// public void SetICaseDeposition(ITmaxDeposition ICaseDeposition)		

		/// <summary>Called to get the exchange interface for the State record</summary>
		/// <returns>The exchange interface</returns>
		public ITmaxBaseObjectionRecord GetIOxState()
		{
			//	Do we need to get the interface?
			if((m_IOxState == null) && (m_IOxObjection != null) && (m_strStateId.Length > 0))
				m_IOxState = m_IOxObjection.GetIOxState();

			return m_IOxState;

		}// public ITmaxBaseObjectionRecord GetOxState()			

		/// <summary>Called to set the exchange interface for the Deposition record</summary>
		/// <param name="IOxState">The exchange interface</param>
		public void SetIOxState(ITmaxBaseObjectionRecord IOxState)
		{
			if((m_IOxState = IOxState) != null)
			{
				m_strStateId = IOxState.GetUniqueId();
				m_strState = IOxState.GetText();
			}
			else
			{
				m_strStateId = "";
				m_strState = "";
			}

		}// public void SetOxState(ITmaxBaseObjectionRecord IOxState)			

		/// <summary>Called to get the exchange interface for the Ruling record</summary>
		/// <returns>The exchange interface</returns>
		public ITmaxBaseObjectionRecord GetIOxRuling()
		{
			//	Do we need to get the interface?
			if((m_IOxRuling == null) && (m_IOxObjection != null) && (m_strRulingId.Length > 0))
				m_IOxRuling = m_IOxObjection.GetIOxRuling();

			return m_IOxRuling;

		}// public ITmaxBaseObjectionRecord GetIOxRuling()			

		/// <summary>Called to set the exchange interface for the Deposition record</summary>
		/// <param name="IOxRuling">The exchange interface</param>
		public void SetIOxRuling(ITmaxBaseObjectionRecord IOxRuling)
		{
			if((m_IOxRuling = IOxRuling) != null)
			{
				m_strRulingId = IOxRuling.GetUniqueId();
				m_strRuling = IOxRuling.GetText();
			}
			else
			{
				m_strRulingId = "";
				m_strRuling = "";
			}

		}// public void SetIOxRuling(ITmaxBaseObjectionRecord IOxRuling)			

		/// <summary>Called to get the exchange interface for the ModifiedBy record</summary>
		/// <returns>The exchange interface</returns>
		public ITmaxBaseObjectionRecord GetIOxModifiedBy()
		{
			//	Do we need to get the interface?
			if((m_IOxModifiedBy == null) && (m_IOxObjection != null) && (m_strModifiedById.Length > 0))
				m_IOxModifiedBy = m_IOxObjection.GetIOxModifiedBy();

			return m_IOxModifiedBy;

		}// public ITmaxBaseObjectionRecord GetIOxModifiedBy()			

		/// <summary>Called to set the exchange interface for the Deposition record</summary>
		/// <param name="IOxModifiedBy">The exchange interface</param>
		public void SetIOxModifiedBy(ITmaxBaseObjectionRecord IOxModifiedBy)
		{
			if((m_IOxModifiedBy = IOxModifiedBy) != null)
			{
				m_strModifiedById = IOxModifiedBy.GetUniqueId();
				m_strModifiedBy = IOxModifiedBy.GetText();
			}
			else
			{
				m_strModifiedById = "";
				m_strModifiedBy = "";
			}

		}// public void SetIOxModifiedBy(ITmaxBaseObjectionRecord IOxModifiedBy)			

		/// <summary>Called to get the MediaId of the deposition the objection references</summary>
		/// <returns>The MediaId of the associated deposition</returns>
		public string GetDeposition()
		{
			if(m_strDeposition.Length == 0)
			{
				if(this.ICaseDeposition != null)
					m_strDeposition = this.ICaseDeposition.GetMediaId();
				else if(this.IOxDeposition != null)
					m_strDeposition = this.IOxDeposition.GetText();
			}
			
			return m_strDeposition;

		}// public string GetDeposition()		

		/// <summary>Called to get the unique id of the deposition record in the Objections database</summary>
		/// <returns>The unique id of the deposition record</returns>
		public string GetDepositionId()
		{
			if(m_strDepositionId.Length == 0)
			{
				if(this.IOxDeposition != null)
					m_strDepositionId = this.IOxDeposition.GetUniqueId();
			}

			return m_strDepositionId;

		}// public string GetDepositionId()	


		/// <summary>This method is called to get the application case object</summary>
		/// <returns>The application case descriptor object</returns>
		public CTmaxCase GetCase()
		{
			//	Do we need to allocate the object?
			if(m_tmaxCase == null)
			{
				if(m_IOxObjection != null)
					m_tmaxCase = m_IOxObjection.GetCase();
					
				if(m_tmaxCase == null)
					m_tmaxCase = new CTmaxCase();
			}

			return m_tmaxCase;

		}// public CTmaxCase GetCase()

		/// <summary>Called to get the current state</summary>
		/// <returns>The current state</returns>
		public string GetState()
		{
			if(m_strState.Length == 0)
			{
				if(this.IOxState != null)
					m_strState = this.IOxState.GetText();
			}

			return m_strState;

		}// public string GetState()		

		/// <summary>Called to get the unique id of the current state</summary>
		/// <returns>The unique id of the current state</returns>
		public string GetStateId()
		{
			if(m_strStateId.Length == 0)
			{
				if(this.IOxState != null)
					m_strStateId = this.IOxState.GetUniqueId();
			}

			return m_strStateId;

		}// public string GetStateId()	

		/// <summary>Called to get the current state</summary>
		/// <returns>The current state</returns>
		public string GetRuling()
		{
			if(m_strRuling.Length == 0)
			{
				if(this.IOxRuling != null)
					m_strRuling = this.IOxRuling.GetText();
			}

			return m_strRuling;

		}// public string GetRuling()		

		/// <summary>Called to get the unique id of the current state</summary>
		/// <returns>The unique id of the current state</returns>
		public string GetRulingId()
		{
			if(m_strRulingId.Length == 0)
			{
				if(this.IOxRuling != null)
					m_strRulingId = this.IOxRuling.GetUniqueId();
			}

			return m_strRulingId;

		}// public string GetRulingId()	

		/// <summary>Called to get the unique id of the ModifiedBy user record</summary>
		/// <returns>The unique id of the ModifiedBy user record</returns>
		public string GetModifiedById()
		{
			if(m_strModifiedById.Length == 0)
			{
				if(this.IOxModifiedBy != null)
					m_strModifiedById = this.IOxModifiedBy.GetUniqueId();
			}

			return m_strModifiedById;

		}// public string GetModifiedById()	

		/// <summary>Called to get the name of the user that last modified the objection</summary>
		/// <returns>The user name</returns>
		public string GetModifiedBy()
		{
			if(m_strModifiedBy.Length == 0)
			{
				if(this.IOxModifiedBy != null)
					m_strModifiedBy = this.IOxModifiedBy.GetText();
			}

			return m_strModifiedBy;

		}// public string GetModifiedBy()		

		/// <summary>Called to get the text used to identify the objection in an error message</summary>
		/// <returns>The error message text</returns>
		public string GetErrorId()
		{
			if(this.Deposition.Length > 0)
				return String.Format("{0} - [{1} - {2}]", this.Deposition, CTmaxToolbox.PLToString(this.FirstPL), CTmaxToolbox.PLToString(this.LastPL));
			else if(this.ICaseDeposition != null)
				return String.Format("{0} - [{1} - {2}]", this.ICaseDeposition.GetMediaId(), CTmaxToolbox.PLToString(this.FirstPL), CTmaxToolbox.PLToString(this.LastPL));
			else
				return String.Format("? - [{0} - {1}]", CTmaxToolbox.PLToString(this.FirstPL), CTmaxToolbox.PLToString(this.LastPL));

		}// public string GetErrorId()	

		/// <summary>Called to get the headers descriptor when exporting objections to ASCII text files</summary>
		/// <param name="tmaxOptions">The user defined options for the export operation</param>
		/// <returns>The header descriptor to be written to file</returns>
		static public string GetAsciiHeaders(CTmaxExportObjections tmaxOptions)
		{
			string strHeaders = "";
			string strDelimiter = tmaxOptions.GetDelimiter();
			
			//	Build the header line
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_PLAINTIFF, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_DEPOSITION, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_FIRST_PAGE, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_FIRST_LINE, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_LAST_PAGE, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_LAST_LINE, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_STATE, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_RULING, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_ARGUMENT, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_RESPONSE_1, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_RESPONSE_2, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_RESPONSE_3, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_RULING_TEXT, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_WORK_PRODUCT, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_COMMENTS, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_UNIQUE_ID, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_CASE_NAME, tmaxOptions) + strDelimiter);
			strHeaders += (ApplyAsciiOptions(TMAX_OBJECTION_ASCII_HEADER_CASE_ID, tmaxOptions));
			
			return strHeaders;
			
		}// static public string GetAsciiHeaders(string strDelimiter)

		/// <summary>Called to apply the options for formatting the ASCII text</summary>
		/// <param name="strSource">The source string</param>
		/// <param name="tmaxOptions">The user defined options for the export operation</param>
		/// <returns>The ASCII formatted string</returns>
		static public string ApplyAsciiOptions(string strSource, CTmaxExportObjections tmaxOptions)
		{
			string strApplied = strSource;
			string strReplacement = "";
			
			//	Should we replace CR/LF pairs?
			if(tmaxOptions.CRLFReplacement != TmaxExportCRLF.None)
			{
				strReplacement = tmaxOptions.GetCRLFReplacement();
				strApplied = strApplied.Replace("\r\n", strReplacement);
				strApplied = strApplied.Replace("\r", strReplacement);
				strApplied = strApplied.Replace("\n", strReplacement);
			}
			
			//	Should we add bounding quotes?
			if(tmaxOptions.AddQuotes == true)
			{
				strApplied = String.Format("\"{0}\"", strApplied);
			}
				
			return strApplied;

		}// static public string ApplyAsciiOptions(string strSource, CTmaxExportObjections tmaxOptions)

		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public void GetProperties(CTmaxProperties tmaxProperties)
		{
			tmaxProperties.Add(TMAX_OBJECTION_PROP_UNIQUE_ID, "UniqueId", this.UniqueId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_DEPOSITION, "Deposition", this.Deposition, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_PARTY, "Party", this.Party, TmaxPropertyCategories.Database, TmaxPropGridEditors.DropList);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_FIRST_PAGE, "First PG", CTmaxToolbox.PLToPage(this.FirstPL), TmaxPropertyCategories.Database, TmaxPropGridEditors.Integer);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_FIRST_LINE, "First LN", CTmaxToolbox.PLToLine(this.FirstPL), TmaxPropertyCategories.Database, TmaxPropGridEditors.Integer);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_LAST_PAGE, "Last PG", CTmaxToolbox.PLToPage(this.LastPL), TmaxPropertyCategories.Database, TmaxPropGridEditors.Integer);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_LAST_LINE, "Last LN", CTmaxToolbox.PLToLine(this.LastPL), TmaxPropertyCategories.Database, TmaxPropGridEditors.Integer);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_STATE, "Status", this.State, TmaxPropertyCategories.Database, TmaxPropGridEditors.DropList);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_RULING, "Ruling", this.Ruling, TmaxPropertyCategories.Database, TmaxPropGridEditors.DropList);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_ARGUMENT, "Objection", this.Argument, TmaxPropertyCategories.Database, TmaxPropGridEditors.Memo);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_RESPONSE_1, "Response", this.Response1, TmaxPropertyCategories.Database, TmaxPropGridEditors.Memo);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_RESPONSE_2, "Response 2", this.Response2, TmaxPropertyCategories.Database, TmaxPropGridEditors.Memo);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_RESPONSE_3, "Response 3", this.Response3, TmaxPropertyCategories.Database, TmaxPropGridEditors.Memo);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_RULING_TEXT, "Ruling Text", this.RulingText, TmaxPropertyCategories.Database, TmaxPropGridEditors.Memo);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_WORK_PRODUCT, "Work Product", this.WorkProduct, TmaxPropertyCategories.Database, TmaxPropGridEditors.Memo);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_COMMENTS, "Comments", this.Comments, TmaxPropertyCategories.Database, TmaxPropGridEditors.Memo);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_CASE_NAME, "Case Name", this.CaseName, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);
			tmaxProperties.Add(TMAX_OBJECTION_PROP_CASE_ID, "Case Id", this.CaseId, TmaxPropertyCategories.Database, TmaxPropGridEditors.None);

		}// public virtual void GetProperties(CTmaxProperties tmaxProperties)

		/// <summary>This method will refresh the value of all properties in the specified collection</summary>
		/// <param name="tmaxProperty">The properties to be refreshed</param>
		public virtual void RefreshProperties(CTmaxProperties tmaxProperties)
		{
			foreach(CTmaxProperty O in tmaxProperties)
			{
				try
				{
					RefreshProperty(O);
				}
				catch
				{
				}

			}// foreach(CTmaxProperty O in tmaxProperties)

		}// public virtual void RefreshProperties(CTmaxProperties tmaxProperties)

		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case TMAX_OBJECTION_PROP_UNIQUE_ID:
					tmaxProperty.Value = this.UniqueId;
					break;

				case TMAX_OBJECTION_PROP_DEPOSITION:
					tmaxProperty.Value = this.Deposition;
					break;

				case TMAX_OBJECTION_PROP_PARTY:
					tmaxProperty.Value = this.Party;
					break;

				case TMAX_OBJECTION_PROP_FIRST_PAGE:
					tmaxProperty.Value = CTmaxToolbox.PLToPage(this.FirstPL);
					break;

				case TMAX_OBJECTION_PROP_FIRST_LINE:
					tmaxProperty.Value = CTmaxToolbox.PLToLine(this.FirstPL);
					break;

				case TMAX_OBJECTION_PROP_LAST_PAGE:
					tmaxProperty.Value = CTmaxToolbox.PLToPage(this.LastPL);
					break;

				case TMAX_OBJECTION_PROP_LAST_LINE:
					tmaxProperty.Value = CTmaxToolbox.PLToLine(this.LastPL);
					break;

				case TMAX_OBJECTION_PROP_STATE:
					tmaxProperty.Value = this.State;
					break;

				case TMAX_OBJECTION_PROP_RULING:
					tmaxProperty.Value = this.Ruling;
					break;

				case TMAX_OBJECTION_PROP_ARGUMENT:
					tmaxProperty.Value = this.Argument;
					break;

				case TMAX_OBJECTION_PROP_RESPONSE_1:
					tmaxProperty.Value = this.Response1;
					break;

				case TMAX_OBJECTION_PROP_RESPONSE_2:
					tmaxProperty.Value = this.Response2;
					break;

				case TMAX_OBJECTION_PROP_RESPONSE_3:
					tmaxProperty.Value = this.Response3;
					break;

				case TMAX_OBJECTION_PROP_RULING_TEXT:
					tmaxProperty.Value = this.RulingText;
					break;

				case TMAX_OBJECTION_PROP_WORK_PRODUCT:
					tmaxProperty.Value = this.WorkProduct;
					break;

				case TMAX_OBJECTION_PROP_COMMENTS:
					tmaxProperty.Value = this.Comments;
					break;

				case TMAX_OBJECTION_PROP_CASE_NAME:
					tmaxProperty.Value = this.CaseName;
					break;

				case TMAX_OBJECTION_PROP_CASE_ID:
					tmaxProperty.Value = this.CaseId;
					break;

				default:

					Debug.Assert(false, "Unhandled property update");
					break;

			}

		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)

		/// <summary>Called to get the descriptor when exporting objections to ASCII text files</summary>
		/// <param name="tmaxOptions">The user defined options for the export operation</param>
		/// <returns>The objection descriptor to be written to file</returns>
		public string GetAsciiExport(CTmaxExportObjections tmaxOptions)
		{
			string strAscii = "";
			string strDelimiter = tmaxOptions.GetDelimiter();
			//	Assembly the properties in the same order as defined in GetAsciiHeader()
			strAscii += (ApplyAsciiOptions(((this.Plaintiff == true) ? "yes" : "no"), tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.Deposition, tmaxOptions) + strDelimiter);
			strAscii += (CTmaxToolbox.PLToPage(this.FirstPL).ToString() + strDelimiter);
			strAscii += (CTmaxToolbox.PLToLine(this.FirstPL).ToString() + strDelimiter);
			strAscii += (CTmaxToolbox.PLToPage(this.LastPL).ToString() + strDelimiter);
			strAscii += (CTmaxToolbox.PLToLine(this.LastPL).ToString() + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.State, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.Ruling, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.Argument, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.Response1, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.Response2, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.Response3, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.RulingText, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.WorkProduct, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.Comments, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.UniqueId, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.CaseName, tmaxOptions) + strDelimiter);
			strAscii += (ApplyAsciiOptions(this.CaseId, tmaxOptions));

			return strAscii;

		}// public string GetAsciiExport(CTmaxExportObjections tmaxOptions)

		/// <summary>Called to automatically change the state if required</summary>
		/// <param name="IOxStates">The collection of available states</param>
		/// <returns>True if the state was changed</returns>
		public bool SetAutoState(ICollection IOxStates)
		{
			bool						bStateChanged = false;
			ITmaxBaseObjectionRecord	IOxState = null;
			
			//	Has there been a response to this objection?
			if((this.Response1.Length > 0) || (this.Response2.Length > 0) || (this.Response3.Length > 0))
			{
				//	Assign default ruling if necessary
				if(this.Ruling.Length == 0)
					this.Ruling = TMAX_OBJECTION_RULING_NONE;
					
				//	Are we still waiting for a ruling?
				if(String.Compare(this.Ruling, TMAX_OBJECTION_RULING_NONE, true) == 0)
				{
					//	Do we need to set the state?
					if(String.Compare(this.State, TMAX_OBJECTION_STATE_PENDING_RULING, true) != 0)
					{
						//	Find the matching state record
						if(IOxStates != null)
						{
							foreach(ITmaxBaseObjectionRecord O in IOxStates)
							{
								if(String.Compare(O.GetText(), TMAX_OBJECTION_STATE_PENDING_RULING, true) == 0)
								{
									IOxState = O;
									break;
								}

							}// foreach(ITmaxBaseObjectionRecord O in IOxStates)

						}// if(IOxStates != null)


					}// if(String.Compare(this.Ruling, TMAX_OBJECTION_RULING_NONE, true) == 0)
					
					if(IOxState != null)
						this.IOxState = IOxState;
					else
						this.State = TMAX_OBJECTION_STATE_PENDING_RULING;
						
					bStateChanged = true;

				}// if(String.Compare(this.Ruling, TMAX_OBJECTION_RULING_NONE, true) == 0)

			}// if((this.Response1.Length > 0) || (this.Response2.Length > 0) || (this.Response3.Length > 0))
			
			return bStateChanged;

		}// public bool SetAutoState()

		/// <summary>Called to determine if this objection matches the caller's objection</summary>
		/// <param name="tmaxObjection">The objection to be compared</param>
		/// <param name="bIgnoreCase">True to ignore the case identifiers</param>
		/// <returns>true if the two objections match</returns>
		public bool IsDuplicate(CTmaxObjection tmaxObjection, bool bIgnoreCase)
		{
			//	Has the specified objection been assigned an identifier?
			if(tmaxObjection.UniqueId.Length > 0)
			{
				return (String.Compare(tmaxObjection.UniqueId, this.UniqueId, true) == 0);
			}
			else
			{
				if((bIgnoreCase == false) && (this.CaseName.Length > 0) && (tmaxObjection.CaseName.Length > 0))
				{
					if(String.Compare(this.CaseName, tmaxObjection.CaseName, true) != 0)
						return false;
				}

				if(String.Compare(this.Deposition, tmaxObjection.Deposition, true) != 0)
					return false;

				if(this.Plaintiff != tmaxObjection.Plaintiff)
					return false;
					
				if(this.FirstPL != tmaxObjection.FirstPL)
					return false;
					
				if(this.LastPL != tmaxObjection.LastPL)
					return false;
					
				return true;
			}

		}// public bool IsDuplicate(CTmaxObjection tmaxObjection)

		#endregion Public Methods

		#region Properties

		/// <summary>Unique Id assigned by the database</summary>
		public string UniqueId
		{
			get { return m_strUniqueId; }
			set { m_strUniqueId = value; }
		}

		/// <summary>UniqueId assigned to the Deposition in the objection database</summary>
		public string DepositionId
		{
			get { return GetDepositionId(); }
			set { m_strDepositionId = value; }
		}

		/// <summary>UniqueId assigned to the type in the objection database</summary>
		public string StateId
		{
			get { return GetStateId(); }
			set { m_strStateId = value; }
		}

		/// <summary>UniqueId assigned to the ruling in the objection database</summary>
		public string RulingId
		{
			get { return GetRulingId(); }
			set { m_strRulingId = value; }
		}

		/// <summary>UniqueId assigned to the ModifiedBy user in the objection database</summary>
		public string ModifiedById
		{
			get { return GetModifiedById(); }
			set { m_strModifiedById = value; }
		}

		/// <summary>Exchange interface to the record in the objections database</summary>
		public ITmaxObjectionRecord IOxObjection
		{
			get { return m_IOxObjection; }
			set { SetIOxObjection(value); }
		}

		/// <summary>Exchange interface to the record in the case database</summary>
		public ITmaxDeposition ICaseDeposition
		{
			get { return m_ICaseDeposition; }
			set { SetICaseDeposition(value); }
		}

		/// <summary>Exchange interface to the record in the objections database</summary>
		public ITmaxBaseObjectionRecord IOxDeposition
		{
			get { return GetIOxDeposition(); }
			set { SetIOxDeposition(value); }
		}

		/// <summary>Exchange interface to the record in the objections database</summary>
		public ITmaxBaseObjectionRecord IOxState
		{
			get { return GetIOxState(); }
			set { SetIOxState(value); }
		}

		/// <summary>Exchange interface to the record in the objections database</summary>
		public ITmaxBaseObjectionRecord IOxRuling
		{
			get { return GetIOxRuling(); }
			set { SetIOxRuling(value); }
		}

		/// <summary>Exchange interface to the record in the objections database</summary>
		public ITmaxBaseObjectionRecord IOxModifiedBy
		{
			get { return GetIOxModifiedBy(); }
			set { SetIOxModifiedBy(value); }
		}

		/// <summary>Name of the Deposition associated with the objection</summary>
		public string Deposition
		{
			get { return GetDeposition(); }
			set { m_strDeposition = value; }
		}

		/// <summary>Name of the type associated with the objection</summary>
		public string State
		{
			get { return GetState(); }
			set { m_strState = value; }
		}

		/// <summary>Name of the ruling associated with the objection</summary>
		public string Ruling
		{
			get { return GetRuling(); }
			set { m_strRuling = value; }
		}

		//	The application wrapper object for the associated case descriptor
		public CTmaxCase Case
		{
			get { return GetCase(); }
			set { m_tmaxCase = value; }
		}

		//	The Name assigned to the associated case
		public string CaseName
		{
			get { return this.Case.Name; }
			set { this.Case.Name = value; }
		}

		//	Uniqued ID assigned to the associated case
		public string CaseId
		{
			get { return this.Case.UniqueId; }
			set { this.Case.UniqueId = value; }
		}

		//	The version of the associated case
		public string CaseVersion
		{
			get { return this.Case.Version; }
			set { this.Case.Version = value; }
		}

		//	True if objection filed by the Plaintiff
		public bool Plaintiff
		{
			get { return m_bPlaintiff; }
			set { m_bPlaintiff = value; }
		}

		//	The string identifier plaintiff / defendant
		public string Party
		{
			get { return GetParty(); }
			set { SetParty(value); }
		}

		//	First Page/Line of the objection
		public long FirstPL
		{
			get { return m_lFirstPL; }
			set { m_lFirstPL = value; }
		}

		//	Last Page/Line of the objection
		public long LastPL
		{
			get { return m_lLastPL; }
			set { m_lLastPL = value; }
		}

		/// <summary>The description of the objection</summary>
		public string Argument
		{
			get { return m_strArgument; }
			set { m_strArgument = value; }
		}

		/// <summary>The first response</summary>
		public string Response1
		{
			get { return m_strResponse1; }
			set { m_strResponse1 = value; }
		}

		/// <summary>The second response</summary>
		public string Response2
		{
			get { return m_strResponse2; }
			set { m_strResponse2 = value; }
		}

		/// <summary>The third response</summary>
		public string Response3
		{
			get { return m_strResponse3; }
			set { m_strResponse3 = value; }
		}

		/// <summary>Supplemental text associated with the official ruling</summary>
		public string RulingText
		{
			get { return m_strRulingText; }
			set { m_strRulingText = value; }
		}

		/// <summary>The work product description</summary>
		public string WorkProduct
		{
			get { return m_strWorkProduct; }
			set { m_strWorkProduct = value; }
		}

		/// <summary>The user comments</summary>
		public string Comments
		{
			get { return m_strComments; }
			set { m_strComments = value; }
		}

		/// <summary>The name of the user that last modified by record</summary>
		public string ModifiedBy
		{
			get { return GetModifiedBy(); }
			set { m_strModifiedBy = value; }
		}

		/// <summary>The date/time of last modification</summary>
		public System.DateTime ModifiedOn
		{
			get { return m_dtModifiedOn; }
			set { m_dtModifiedOn = value; }
		}

		#endregion Properties

	}// public class CTmaxObjection


	/// <summary>Objects of this class are used to manage a dynamic array of CTmaxObjection objects</summary>
	public class CTmaxObjections : CTmaxSortedArrayList
	{
		#region Constants


		#endregion Constants

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxObjections() : base()
		{
			this.KeepSorted = false;

			m_tmaxEventSource.Name = "Objections";
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="tmaxObjection">CTmaxObjection object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxObjection Add(CTmaxObjection tmaxObjection)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxObjection as object);

				return tmaxObjection;
			}
			catch
			{
				return null;
			}

		}// Add(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="tmaxObjection">The object to be removed</param>
		public void Remove(CTmaxObjection tmaxObjection)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxObjection as object);
			}
			catch
			{
			}
		}

		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxObjection">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxObjection tmaxObjection)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxObjection as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxObjection this[int index]
		{
			// Use base class to process actual collection operation
			get
			{
				return (GetAt(index) as CTmaxObjection);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxObjection value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>This method is called to find the objection with the specified ID</summary>
		/// <param name="lId">The ID of the desired objection</param>
		/// <returns>The associated objection</returns>
		public CTmaxObjection Find(string strUniqueId)
		{
			foreach(CTmaxObjection O in this)
			{
				if(O.UniqueId == strUniqueId)
					return O;
			}

			return null;

		}// public CTmaxObjection Find(string strUniqueId)

		/// <summary>This method is called to find all objections associated with the specified Deposition record</summary>
		/// <param name="tmaxObjections">The collection in which to store the objections</param>
		/// <param name="strMediaId">The mediaID of the source deposition record in the case database</param>
		/// <param name="lFirstPL">The first page/line in the allowable range</param>
		/// <param name="lLastPL">The last page/line in the allowable range</param>
		/// <returns>The number of objections added to the collection</returns>
		public int FindAll(CTmaxObjections tmaxObjections, string strMediaId, long lFirstPL, long lLastPL)
		{
			int iInitial = 0;

			Debug.Assert(tmaxObjections != null);
			if(tmaxObjections == null) return 0;

			try
			{
				//	Store the initial number of objections in the caller's collection
				iInitial = tmaxObjections.Count;

				foreach(CTmaxObjection O in this)
				{
					//	Do the depositions match?
					if(String.Compare(O.Deposition, strMediaId, true) == 0)
					{
						//	Are we checking the range?
						if((lFirstPL > 0) && (lLastPL > 0))
						{
							if((O.FirstPL <= lLastPL) && (O.LastPL >= lFirstPL))
								tmaxObjections.Add(O);
						}
						else
						{
							tmaxObjections.Add(O);
						}

					}// if(String.Compare(O.Deposition, strMediaId, true) == 0)

				}// foreach(CTmaxObjection O in this)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FindAll", Ex);
			}

			return (tmaxObjections.Count - iInitial);

		}// public int FindAll(CTmaxObjections tmaxObjections, string strMediaId)

		/// <summary>This method is called to find all objections associated with the specified Deposition record</summary>
		/// <param name="strMediaId">The mediaID of the source deposition record in the case database</param>
		/// <param name="lFirstPL">The first page/line in the allowable range</param>
		/// <param name="lLastPL">The last page/line in the allowable range</param>
		/// <returns>The collection of objections if any</returns>
		public CTmaxObjections FindAll(string strMediaId, long lFirstPL, long lLastPL)
		{
			CTmaxObjections tmaxObjections = null;

			tmaxObjections = new CTmaxObjections();

			FindAll(tmaxObjections, strMediaId, lFirstPL, lLastPL);

			if((tmaxObjections != null) && (tmaxObjections.Count > 0))
				return tmaxObjections;
			else
				return null;

		}// public CTmaxObjections FindAll(string strMediaId, long lFirstPL, long lLastPL)

		/// <summary>This method is called to find all objections associated with the specified Deposition</summary>
		/// <param name="strMediaId">The MediaId of the desired deposition</param>
		/// <returns>The collection of objections if any</returns>
		public CTmaxObjections FindAll(string strMediaId)
		{
			return FindAll(strMediaId, 0, 0);
		}

		/// <summary>This method is called to find all objections associated with the specified Deposition</summary>
		/// <param name="tmaxObjections">The collection in which to store the objections</param>
		/// <param name="strMediaId">The MediaId of the desired deposition</param>
		/// <returns>The number of objections added to the collection</returns>
		public int FindAll(CTmaxObjections tmaxObjections, string strMediaId)
		{
			return FindAll(tmaxObjections, strMediaId, 0, 0);
		}

		/// <summary>This method is called to find all objections associated with the specified Deposition record</summary>
		/// <param name="tmaxObjections">The collection in which to store the objections</param>
		/// <param name="ICaseDeposition">The exchange interface for the deposition record in the case database</param>
		/// <param name="lFirstPL">The first page/line in the allowable range</param>
		/// <param name="lLastPL">The last page/line in the allowable range</param>
		/// <returns>The number of objections added to the collection</returns>
		public int FindAll(CTmaxObjections tmaxObjections, ITmaxDeposition ICaseDeposition, long lFirstPL, long lLastPL)
		{
			if((ICaseDeposition != null) && (ICaseDeposition.GetMediaId().Length > 0))
				return FindAll(tmaxObjections, ICaseDeposition.GetMediaId(), lFirstPL, lLastPL);
			else
				return 0;
		}

		/// <summary>This method is called to find all objections associated with the specified Deposition record</summary>
		/// <param name="tmaxObjections">The collection in which to store the objections</param>
		/// <param name="ICaseDeposition">The exchange interface for the deposition record in the case database</param>
		/// <returns>The number of objections added to the collection</returns>
		public int FindAll(CTmaxObjections tmaxObjections, ITmaxDeposition ICaseDeposition)
		{
			return FindAll(tmaxObjections, ICaseDeposition.GetMediaId(), 0, 0);
		}

		/// <summary>This method is called to find all objections associated with the specified Deposition record</summary>
		/// <param name="ICaseDeposition">The exchange interface for the deposition record in the case database</param>
		/// <param name="lFirstPL">The first page/line in the allowable range</param>
		/// <param name="lLastPL">The last page/line in the allowable range</param>
		/// <returns>The collection of objections if any</returns>
		public CTmaxObjections FindAll(ITmaxDeposition ICaseDeposition, long lFirstPL, long lLastPL)
		{
			if((ICaseDeposition != null) && (ICaseDeposition.GetMediaId().Length > 0))
				return FindAll(ICaseDeposition.GetMediaId(), lFirstPL, lLastPL);
			else
				return null;
		}

		/// <summary>This method is called to find all objections associated with the specified Deposition record</summary>
		/// <param name="ICaseDeposition">The exchange interface for the deposition record in the case database</param>
		/// <returns>The collection of objections if any</returns>
		public CTmaxObjections FindAll(ITmaxDeposition ICaseDeposition)
		{
			return FindAll(ICaseDeposition.GetMediaId(), 0, 0);
		}

		#endregion Public Methods

	}//	public class CTmaxObjections : CTmaxSortedArrayList

}// namespace FTI.Shared.Trialmax
