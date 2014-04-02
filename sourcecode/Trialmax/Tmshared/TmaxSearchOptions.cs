using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class encapsulates the options used to register media in a Trialmax database</summary>
	public class CTmaxSearchOptions
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "SearchOptions";
		private const string XMLINI_INCLUDE_SUB_BINDERS_KEY		= "IncludeSubBinders";
		private const string XMLINI_ALLOW_PARTIAL_WORD_KEY		= "AllowPartialWord";
		private const string XMLINI_FIND_ALL_WORDS_KEY			= "FindAllWords";
		private const string XMLINI_CASE_SENSITIVE_KEY			= "CaseSensitive";
		private const string XMLINI_WHOLE_WORDS_KEY				= "WholeWords";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to PreviousItems property</summary>
		private CTmaxItems m_tmaxPreviousItems = null;
		
		/// <summary>Local member bound to SearchStrings property</summary>
		private ArrayList m_aSearchStrings = new ArrayList();
		
		/// <summary>Local member bound to MaxSearchStrings property</summary>
		private int m_iMaxSearchStrings = 10;
		
		/// <summary>Local member bound to IncludeSubBinders property</summary>
		private bool m_bIncludeSubBinders = false;
		
		/// <summary>Local member bound to AllowPartialWord property</summary>
		private bool m_bAllowPartialWord = false;
		
		/// <summary>Local member bound to FindAllWords property</summary>
		private bool m_bFindAllWords = false;

		/// <summary>Local member bound to CaseSensitive property</summary>
		private bool m_bCaseSensitive = false;

		/// <summary>Local member bound to WholeWords property</summary>
		private bool m_bWholeWords = false;

		/// <summary>Local member bound to UseXPath property</summary>
		private bool m_bUseXPath = true;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxSearchOptions()
		{
		
		}// CTmaxSearchOptions()
		
		/// <summary>This method is called to add a case folder to the collection of recently used folders</summary>
		/// <param name="strFolder">The folder to be added to the local collection</param>
		/// <returns>true if successful</returns>
		public bool AddSearchString(string strSearch)
		{
			string strNewSearch = null;
			
			try
			{
				//	See if this string already exists in the collection
				for(int i = 0; i < m_aSearchStrings.Count; i++)
				{
					if(String.Compare(m_aSearchStrings[i].ToString(), strSearch, false) == 0)
					{
						//	Is it already at the top of the list?
						if(i == 0)
						{
							//	Nothing to do
							return true;
						}
						else
						{
							//	Remove from the collection 
							m_aSearchStrings.RemoveAt(i);
						}
					
					}
					
				}// for(int i = 0; i < m_aSearchStrings.Count; i++)
				
				//	Do we need to make room?
				while(m_aSearchStrings.Count > m_iMaxSearchStrings)
					m_aSearchStrings.RemoveAt(m_aSearchStrings.Count - 1);
					
				strNewSearch = new string(strSearch.ToCharArray());
				
				//	Insert at the top of the collection
				if(m_aSearchStrings.Count > 0)
					m_aSearchStrings.Insert(0, strNewSearch);
				else
					m_aSearchStrings.Add(strNewSearch);
					
				return true;
			
			}
			catch
			{
				return false;
			}
		
		}// public bool AddSearchString(string strSearch)
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			string strKey;
			string strSearch;
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_bAllowPartialWord = xmlIni.ReadBool(XMLINI_ALLOW_PARTIAL_WORD_KEY);
			m_bFindAllWords = xmlIni.ReadBool(XMLINI_FIND_ALL_WORDS_KEY);
			m_bIncludeSubBinders = xmlIni.ReadBool(XMLINI_INCLUDE_SUB_BINDERS_KEY);
			m_bCaseSensitive = xmlIni.ReadBool(XMLINI_CASE_SENSITIVE_KEY);
			m_bWholeWords = xmlIni.ReadBool(XMLINI_WHOLE_WORDS_KEY);
			
			//	Retrieve the search strings
			for(int i = 1; i <= m_iMaxSearchStrings; i++)
			{
				strKey = ("SearchString" + i.ToString());
				strSearch = xmlIni.Read(strKey);
				
				if(strSearch != null && (strSearch.Length > 0))
				{
					m_aSearchStrings.Add(new string(strSearch.ToCharArray()));
				}
			}
		
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			string strKey = "";
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_ALLOW_PARTIAL_WORD_KEY, m_bAllowPartialWord);
			xmlIni.Write(XMLINI_FIND_ALL_WORDS_KEY, m_bFindAllWords);
			xmlIni.Write(XMLINI_INCLUDE_SUB_BINDERS_KEY, m_bIncludeSubBinders);
			xmlIni.Write(XMLINI_CASE_SENSITIVE_KEY, m_bCaseSensitive);
			xmlIni.Write(XMLINI_WHOLE_WORDS_KEY, m_bWholeWords);
			
			//	Write the recently used search strings
			for(int i = 0; i < m_iMaxSearchStrings; i++)
			{
				strKey = ("SearchString" + (i + 1).ToString());
				
				if(i < m_aSearchStrings.Count)
					xmlIni.Write(strKey, m_aSearchStrings[i].ToString());
				else
					xmlIni.Write(strKey, "");
			}
		
		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <ssummary>Collection of items most recently searched items</summary>
		public CTmaxItems PreviousItems
		{
			get { return m_tmaxPreviousItems; }
			set { m_tmaxPreviousItems = value; }
		}
		
		/// <summary>Allow partial words to satisfy search criteria</summary>
		public bool AllowPartialWord
		{
			get { return m_bAllowPartialWord; }
			set { m_bAllowPartialWord = value; }
		}
		
		/// <summary>Find all words when using multiple word search string</summary>
		public bool FindAllWords
		{
			get { return m_bFindAllWords; }
			set { m_bFindAllWords = value; }
		}
		
		/// <summary>Include subbinders in the search range</summary>
		public bool IncludeSubBinders
		{
			get { return m_bIncludeSubBinders; }
			set { m_bIncludeSubBinders = value; }
		}
		
		/// <summary>Perform case sensitive searching</summary>
		public bool CaseSensitive
		{
			get { return m_bCaseSensitive; }
			set { m_bCaseSensitive = value; }
		}
		
		/// <summary>Perform whole word matching</summary>
		public bool WholeWords
		{
			get { return m_bWholeWords; }
			set { m_bWholeWords = value; }
		}
		
		/// <summary>List of most recently used search strings</summary>
		public ArrayList SearchStrings
		{
			get { return m_aSearchStrings; }
		}
		
		/// <summary>This property used for development and testing</summary>
		public bool UseXPath
		{
			get { return m_bUseXPath; }
			set { m_bUseXPath = value; }
		}
		
		#endregion Properties

	}//	public class CTmaxSearchOptions

}// namespace FTI.Shared.Trialmax
