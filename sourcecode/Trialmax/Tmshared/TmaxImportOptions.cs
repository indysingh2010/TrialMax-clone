using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class encapsulates the options used to import records</summary>
	public class CTmaxImportOptions
	{
		#region Constants
		
		private const string XMLINI_DELIMITER_KEY					= "Delimiter";
		private const string XMLINI_EXPRESSION_KEY					= "Expression";
		private const string XMLINI_COMMENT_CHARACTERS_KEY			= "CommentCharacters";
		private const string XMLINI_USE_REGISTRATION_OPTIONS_KEY	= "UseRegistrationOptions";
		private const string XMLINI_OVERWRITE_CODES_KEY				= "OverwriteCodes";
		private const string XMLINI_CRLF_SUBSTITUTION_KEY			= "CRLFSubstitution";
		private const string XMLINI_USER_CRLF_SUBSTITUTION_KEY		= "UserCRLF";
		private const string XMLINI_MERGE_DESIGNATIONS_KEY			= "MergeDesignations";
		private const string XMLINI_CREATE_BACKUP_KEY				= "CreateBackup";
		private const string XMLINI_UPDATE_SCRIPTS_KEY				= "UpdateScripts";
		private const string XMLINI_CONCATENATOR_KEY				= "Concatenator";
		private const string XMLINI_SPLIT_CONCATENATED_KEY			= "SplitConcatenated";
		private const string XMLINI_USER_CONCATENATOR_KEY			= "UserConcatenator";
		private const string XMLINI_IGNORE_FIRST_LINE_KEY			= "IgnoreFirstLine";

		private const string XMLINI_ADD_OBJECTION_STATES_KEY		= "AddObjectionStates";
		private const string XMLINI_ADD_OBJECTION_RULINGS_KEY		= "AddObjectionRulings";
		private const string XMLINI_OBJECTIONS_METHOD_KEY			= "ObjectionsMethod";

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to Delimiter property</summary>
		private TmaxImportDelimiters m_eDelimiter = TmaxImportDelimiters.Tab;
		
		/// <summary>Local member bound to CRLFSubstitution property</summary>
		private TmaxImportCRLF m_eCRLFSubstitution = TmaxImportCRLF.Pipe;
		
		/// <summary>Local member bound to Concatenator property</summary>
		private TmaxImportConcatenators m_eConcatenator = TmaxImportConcatenators.Comma;
		
		/// <summary>Local member bound to DesignationMergeMethod property</summary>
		private TmaxDesignationMergeMethods m_eMergeDesignations = TmaxDesignationMergeMethods.None;
		
		/// <summary>Local member bound to UseRegistrationOptions property</summary>
		private bool m_bUseRegistrationOptions = true;
		
		/// <summary>Local member bound to OverwriteCodes property</summary>
		private bool m_bOverwriteCodes = false;
		
		/// <summary>Local member bound to UpdateScripts property</summary>
		private bool m_bUpdateScripts = false;
		
		/// <summary>Local member bound to CreateBackup property</summary>
		private bool m_bCreateBackup = true;
		
		/// <summary>Local member bound to SplitConcatenated property</summary>
		private bool m_bSplitConcatenated = false;

		/// <summary>Local member bound to IgnoreFirstLine property</summary>
		private bool m_bIgnoreFirstLine = false;

		/// <summary>Local member bound to Expression property</summary>
		private string m_strExpression = "";
		
		/// <summary>Local member bound to CommentCharacters property</summary>
		private string m_strCommentCharacters = "";
		
		/// <summary>Local member bound to UserCRLF property</summary>
		private string m_strUserCRLF = "";
		
		/// <summary>Local member bound to UserConcatenator property</summary>
		private string m_strUserConcatenator = "";

		/// <summary>Local member bound to ObjectionsMethod property</summary>
		private TmaxImportObjectionMethods m_eObjectionsMethod = TmaxImportObjectionMethods.UpdateExisting;

		/// <summary>Local member bound to AddObjectionStates property</summary>
		private bool m_bAddObjectionStates = false;

		/// <summary>Local member bound to AddObjectionRulings property</summary>
		private bool m_bAddObjectionRulings = false;

		/// <summary>Local member bound to DiscardObjectionId property</summary>
		private bool m_bDiscardObjectionsId = false;

		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxImportOptions()
		{
			//	Perform one-time initialization
			Initialize();
			
			//	Make sure the properties are properly initialized
			Clear();
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxOptions">The source object to copy</param>
		public CTmaxImportOptions(CTmaxImportOptions tmaxSource)
		{
			//	Perform one-time initialization
			Initialize();
			
			if(tmaxSource != null)
				Copy(tmaxSource);
		}
		
		/// <summary>This method copies the source object</summary>
		/// <param name="tmaxOptions">The source object to copy</param>
		public void Copy(CTmaxImportOptions tmaxSource)
		{
			this.Name = tmaxSource.Name;
			this.Delimiter = tmaxSource.Delimiter;
			this.OverwriteCodes = tmaxSource.OverwriteCodes;
			this.CommentCharacters = tmaxSource.CommentCharacters;
			this.UseRegistrationOptions = tmaxSource.UseRegistrationOptions;
			this.CRLFSubstitution = tmaxSource.CRLFSubstitution;
			this.Concatenator = tmaxSource.Concatenator;
			this.UserCRLF = tmaxSource.UserCRLF;
			this.UserConcatenator = tmaxSource.UserConcatenator;
			this.Expression = tmaxSource.Expression;
			this.MergeDesignations = tmaxSource.MergeDesignations;
			this.SplitConcatenated = tmaxSource.SplitConcatenated;
			this.IgnoreFirstLine = tmaxSource.IgnoreFirstLine;
			this.AddObjectionStates = tmaxSource.AddObjectionStates;
			this.AddObjectionRulings = tmaxSource.AddObjectionRulings;
			this.ObjectionsMethod = tmaxSource.ObjectionsMethod;
			this.DiscardObjectionsId = tmaxSource.DiscardObjectionsId;
			
		}// public void Copy(CTmaxImportOptions tmaxSource)
		
		/// <summary>This method resets the object to its original state</summary>
		public void Clear()
		{
			m_eDelimiter = TmaxImportDelimiters.Tab;
			m_bOverwriteCodes = false;
			m_bUseRegistrationOptions = true;
			m_strCommentCharacters = "";
			m_strExpression = "";
			m_eMergeDesignations = TmaxDesignationMergeMethods.None;
			m_bAddObjectionStates = true;
			m_bAddObjectionRulings = true;
			m_bIgnoreFirstLine = false;
			m_eObjectionsMethod = TmaxImportObjectionMethods.UpdateExisting;
			m_bDiscardObjectionsId = false;
			
			//	NOTE:	DO NOT clear the Name property. 
			//			It only gets set once by the database
		
		}// public void Clear()
		
		/// <summary>This method is called to get the name of the section in the XML configuration file for this filter</summary>
		/// <returns>The name of the section containing the configuration information for this filter</returns>
		public string GetXmlSectionName()
		{
			if(m_strName.Length > 0)
				return ("trialMax/station/import/" + m_strName);
			else
				return "";
		
		}// public string GetXmlSectionName()
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;
		
			//	Read the properties from file
			m_eDelimiter = (TmaxImportDelimiters)(xmlIni.ReadInteger(XMLINI_DELIMITER_KEY, (int)m_eDelimiter));
			m_strExpression = xmlIni.Read(XMLINI_EXPRESSION_KEY, m_strExpression);
			m_bUseRegistrationOptions = xmlIni.ReadBool(XMLINI_USE_REGISTRATION_OPTIONS_KEY, m_bUseRegistrationOptions);
			m_bOverwriteCodes = xmlIni.ReadBool(XMLINI_OVERWRITE_CODES_KEY, m_bOverwriteCodes);
			m_bUpdateScripts = xmlIni.ReadBool(XMLINI_UPDATE_SCRIPTS_KEY, m_bUpdateScripts);
			m_bCreateBackup = xmlIni.ReadBool(XMLINI_CREATE_BACKUP_KEY, m_bCreateBackup);
			m_strCommentCharacters = xmlIni.Read(XMLINI_COMMENT_CHARACTERS_KEY, m_strCommentCharacters);
			m_eCRLFSubstitution = (TmaxImportCRLF)(xmlIni.ReadInteger(XMLINI_CRLF_SUBSTITUTION_KEY, (int)m_eCRLFSubstitution));
			m_strUserCRLF = xmlIni.Read(XMLINI_USER_CRLF_SUBSTITUTION_KEY, m_strUserCRLF);
			m_eMergeDesignations = (TmaxDesignationMergeMethods)(xmlIni.ReadInteger(XMLINI_MERGE_DESIGNATIONS_KEY, (int)m_eMergeDesignations));
			m_bSplitConcatenated = xmlIni.ReadBool(XMLINI_SPLIT_CONCATENATED_KEY, m_bSplitConcatenated);
			m_bIgnoreFirstLine = xmlIni.ReadBool(XMLINI_IGNORE_FIRST_LINE_KEY, m_bIgnoreFirstLine);
			m_eConcatenator = (TmaxImportConcatenators)(xmlIni.ReadInteger(XMLINI_CONCATENATOR_KEY, (int)m_eConcatenator));
			m_strUserConcatenator = xmlIni.Read(XMLINI_USER_CONCATENATOR_KEY, m_strUserConcatenator);

			m_eObjectionsMethod = (TmaxImportObjectionMethods)(xmlIni.ReadInteger(XMLINI_OBJECTIONS_METHOD_KEY, (int)m_eObjectionsMethod));
			m_bAddObjectionStates = xmlIni.ReadBool(XMLINI_ADD_OBJECTION_STATES_KEY, m_bAddObjectionStates);
			m_bAddObjectionRulings = xmlIni.ReadBool(XMLINI_ADD_OBJECTION_RULINGS_KEY, m_bAddObjectionRulings);

		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(GetXmlSectionName(), true, true) == false) return;
			
			//	Write the properties to file
			xmlIni.Write(XMLINI_DELIMITER_KEY, (int)(this.Delimiter));
			xmlIni.Write(XMLINI_EXPRESSION_KEY, m_strExpression);
			xmlIni.Write(XMLINI_USE_REGISTRATION_OPTIONS_KEY, m_bUseRegistrationOptions);
			xmlIni.Write(XMLINI_OVERWRITE_CODES_KEY, m_bOverwriteCodes);
			xmlIni.Write(XMLINI_UPDATE_SCRIPTS_KEY, m_bUpdateScripts);
			xmlIni.Write(XMLINI_CREATE_BACKUP_KEY, m_bCreateBackup);
			xmlIni.Write(XMLINI_COMMENT_CHARACTERS_KEY, m_strCommentCharacters);
			xmlIni.Write(XMLINI_CRLF_SUBSTITUTION_KEY, (int)(m_eCRLFSubstitution));
			xmlIni.Write(XMLINI_USER_CRLF_SUBSTITUTION_KEY, m_strUserCRLF);
			xmlIni.Write(XMLINI_MERGE_DESIGNATIONS_KEY, (int)(this.MergeDesignations));
			xmlIni.Write(XMLINI_SPLIT_CONCATENATED_KEY, m_bSplitConcatenated);
			xmlIni.Write(XMLINI_IGNORE_FIRST_LINE_KEY, m_bIgnoreFirstLine);
			xmlIni.Write(XMLINI_CONCATENATOR_KEY, (int)(m_eConcatenator));
			xmlIni.Write(XMLINI_USER_CONCATENATOR_KEY, m_strUserConcatenator);

			xmlIni.Write(XMLINI_OBJECTIONS_METHOD_KEY, (int)(m_eObjectionsMethod));
			xmlIni.Write(XMLINI_ADD_OBJECTION_STATES_KEY, m_bAddObjectionStates);
			xmlIni.Write(XMLINI_ADD_OBJECTION_RULINGS_KEY, m_bAddObjectionRulings);

		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method get the delimiter text requested by the user</summary>
		/// <returns>the delimiter text</returns>
		public string GetDelimiter()
		{
			return GetDelimiter(this.Delimiter);	
		}
		
		/// <summary>This method get the expression associated with the specified delimiter</summary>
		/// <param name="e">The import delimiter</param>
		/// <returns>the associated regular expression</returns>
		public string GetExpression(TmaxImportDelimiters e)
		{
			switch(e)
			{
				case TmaxImportDelimiters.Comma:
				
					return ",(?!(?<=(?:^|,)\\s*\"(?:[^\"]|\"\"|\\\\\")*,)(?:[^\"]|\"\"|\\\\\")*\"\\s*(?:,|$))";
					//return ",(?=(?:[^\"\"]*\"\"[^\"\"]*\"\")*(?![^\"\"]*\"\"))";
				
				case TmaxImportDelimiters.Pipe:
				
					return "|(?!(?<=(?:^|,)\\s*\"(?:[^\"]|\"\"|\\\\\")*,)(?:[^\"]|\"\"|\\\\\")*\"\\s*(?:,|$))";
					//return "|(?=(?:[^\"\"]*\"\"[^\"\"]*\"\")*(?![^\"\"]*\"\"))";
					
				case TmaxImportDelimiters.Expression:
				
					return m_strExpression;
					
				case TmaxImportDelimiters.Tab:
				default:
				
					return "";
					
			}// switch(eDelimiter)
		}
		
		/// <summary>This method converts the enumerated delimiter to the appropriate text</summary>
		/// <param name="eDelimiter">the enumerated delimiter identifier</param>
		/// <returns>the delimiter text</returns>
		static public string GetDelimiter(TmaxImportDelimiters eDelimiter)
		{
			switch(eDelimiter)
			{
				case TmaxImportDelimiters.Comma:
				
					return ",";
				
				case TmaxImportDelimiters.Pipe:
				
					return "|";
					
				case TmaxImportDelimiters.Expression:
				case TmaxImportDelimiters.Tab:
				default:
				
					return "\t";
					
			}// switch(eDelimiter)
			
		}// static public string GetDelimiter(TmaxImportDelimiters eDelimiter)
		
		/// <summary>This method converts the enumerated substitution to the appropriate text</summary>
		/// <returns>the substitution text</returns>
		public string GetCRLFSubstitution()
		{
			if(this.CRLFSubstitution == TmaxImportCRLF.User)
				return m_strUserCRLF;
			else
				return GetCRLFSubstitution(m_eCRLFSubstitution);
		}
		
		/// <summary>This method converts the enumerated substitution to the appropriate text</summary>
		/// <param name="eSubstitution">the enumerated substitution identifier</param>
		/// <returns>the substitution text</returns>
		static public string GetCRLFSubstitution(TmaxImportCRLF eSubstitution)
		{
			switch(eSubstitution)
			{
				case TmaxImportCRLF.HTML:
				
					return "<BR>";
				
				case TmaxImportCRLF.Summation:
				
					char cSummation = (char)0x0b;
					return cSummation.ToString();
				
				case TmaxImportCRLF.Space:
				
					return " ";
					
				case TmaxImportCRLF.Pipe:
				
					return "|";
					
				case TmaxImportCRLF.User:
				case TmaxImportCRLF.None:
				default:
				
					return "";
					
			}// switch(eSubstitution)
			
		}// static public string GetCRLFSubstitution(TmaxImportCRLF eSubstitution)
		
		/// <summary>This method converts the enumerated substitution to the appropriate text</summary>
		/// <returns>the replacement text</returns>
		public string GetConcatenator()
		{
			if(this.Concatenator == TmaxImportConcatenators.User)
				return m_strUserConcatenator;
			else
				return GetConcatenator(m_eConcatenator);
		}
		
		/// <summary>This method converts the enumerated concatenator to the appropriate text</summary>
		/// <param name="eConcatenator">the enumerated concatenator identifier</param>
		/// <returns>the concatenator text</returns>
		static public string GetConcatenator(TmaxImportConcatenators eConcatenator)
		{
			switch(eConcatenator)
			{
				case TmaxImportConcatenators.Comma:
				
					return ",";
				
				case TmaxImportConcatenators.Semicolon:
				
					return ";";
				
				case TmaxImportConcatenators.Pipe:
				
					return "|";
					
				case TmaxImportConcatenators.Tilde:
				
					return "~";
					
				case TmaxImportConcatenators.User:
				default:
				
					return "";
					
			}// switch(eConcatenator)
			
		}// static public string GetConcatenator(TmaxImportConcatenators eConcatenator)
		
		/// <summary>This method get the text used to display the replacement option</summary>
		/// <param name="eReplacement">the enumerated replacement identifier</param>
		/// <returns>the display text</returns>
		static public string GetDisplayText(TmaxImportCRLF eSubstitution)
		{
			switch(eSubstitution)
			{
				case TmaxImportCRLF.HTML:
				
					return "<BR>";
				
				case TmaxImportCRLF.Summation:
				
					return "Summation [0xB]";
				
				case TmaxImportCRLF.Space:
				case TmaxImportCRLF.Pipe:
				case TmaxImportCRLF.User:
				case TmaxImportCRLF.None:
				default:
				
					return eSubstitution.ToString();
					
			}// switch(eSubstitution)
			
		}// static public string GetDisplayText(TmaxImportCRLF eSubstitution)
		
		/// <summary>This method get the text used to display the designation merge method</summary>
		/// <param name="eReplacement">the enumerated merge method identifier</param>
		/// <returns>the display text</returns>
		static public string GetDisplayText(TmaxDesignationMergeMethods eMethod)
		{
			switch(eMethod)
			{
				case TmaxDesignationMergeMethods.AdjacentLines:
				
					return "Adjacent Lines";
				
				case TmaxDesignationMergeMethods.AdjacentPages:
				
					return "Adjacent Pages";
				
				case TmaxDesignationMergeMethods.None:
				default:
				
					return eMethod.ToString();
					
			}// switch(eMethod)
			
		}// static public string GetDisplayText(TmaxDesignationMergeMethods eMethod)
		
		/// <summary>This method get the tool tip text for the designation merge method</summary>
		/// <param name="eReplacement">the enumerated merge method identifier</param>
		/// <returns>the tool tip text</returns>
		static public string GetToolTip(TmaxDesignationMergeMethods eMethod)
		{
			switch(eMethod)
			{
				case TmaxDesignationMergeMethods.AdjacentLines:
				
					return (GetDisplayText(eMethod) + " eg: 1:10-1:14 and 1:15-1:20");
				
				case TmaxDesignationMergeMethods.AdjacentPages:
				
					return (GetDisplayText(eMethod) + " eg: 2:1-2:25 and 3:1-3:25");
				
				case TmaxDesignationMergeMethods.None:
				
					return "No merging";
					
				default:
				
					return eMethod.ToString();
					
			}// switch(eMethod)
			
		}// static public string GetToolTip(TmaxDesignationMergeMethods eMethod)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			//			aStrings.Add("An exception was raised while attempting to save the XML case options: filename = %1");
			//			aStrings.Add("An exception was raised while attempting to initialize the case options: filename = %1");

		}// private void SetErrorStrings()
		
		/// <summary>Called to initialize the object at construction</summary>
		private void Initialize()
		{
			//	Set up the event source
			SetErrorStrings();			
			m_tmaxEventSource.Name = "Import Options Events";
		
		}// private void Initialize()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The EventSource for this object</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The Name used to store the information for this filter in a TrialMax XML configuration file</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>Enumerated delimiter identifier</summary>
		public TmaxImportDelimiters Delimiter
		{
			get { return m_eDelimiter; }
			set { m_eDelimiter = value; }
		}
		
		/// <summary>Enumerated CRLF Substitution identifier</summary>
		public TmaxImportCRLF CRLFSubstitution
		{
			get { return m_eCRLFSubstitution; }
			set { m_eCRLFSubstitution = value; }
		}
		
		/// <summary>Enumerated designation merge method identifier</summary>
		public TmaxDesignationMergeMethods MergeDesignations
		{
			get { return m_eMergeDesignations; }
			set { m_eMergeDesignations = value; }
		}
		
		/// <summary>Enumerated Concatenator identifier</summary>
		public TmaxImportConcatenators Concatenator
		{
			get { return m_eConcatenator; }
			set { m_eConcatenator = value; }
		}
		
		/// <summary>User defined Concatenator</summary>
		public string UserConcatenator
		{
			get { return m_strUserConcatenator; }
			set { m_strUserConcatenator = value; }
		}
		
		/// <summary>User defined CRLF substitution</summary>
		public string UserCRLF
		{
			get { return m_strUserCRLF; }
			set { m_strUserCRLF = value; }
		}
		
		/// <summary>Regular expression used to parse the input line</summary>
		public string Expression
		{
			get { return m_strExpression; }
			set { m_strExpression = value; }
		}
		
		/// <summary>True to split concatenated fields into multiple entries</summary>
		public bool SplitConcatenated
		{
			get { return m_bSplitConcatenated; }
			set { m_bSplitConcatenated = value; }
		}

		/// <summary>True to ignore first line if missing when merging designations using PageMerge option</summary>
		public bool IgnoreFirstLine
		{
			get { return m_bIgnoreFirstLine; }
			set { m_bIgnoreFirstLine = value; }
		}

		/// <summary>True to overwrite existing codes</summary>
		public bool OverwriteCodes
		{
			get { return m_bOverwriteCodes; }
			set { m_bOverwriteCodes = value; }
		}
		
		/// <summary>True to update scripts instead of create new</summary>
		public bool UpdateScripts
		{
			get { return m_bUpdateScripts; }
			set { m_bUpdateScripts = value; }
		}
		
		/// <summary>True to create backup on update</summary>
		public bool CreateBackup
		{
			get { return m_bCreateBackup; }
			set { m_bCreateBackup = value; }
		}
		
		/// <summary>True to apply registration options to barcode</summary>
		public bool UseRegistrationOptions
		{
			get { return m_bUseRegistrationOptions; }
			set { m_bUseRegistrationOptions = value; }
		}
		
		/// <summary>Characters used to identify comment lines</summary>
		public string CommentCharacters
		{
			get { return m_strCommentCharacters; }
			set { m_strCommentCharacters = value; }
		}

		/// <summary>Enumerated objections updates method identifier</summary>
		public TmaxImportObjectionMethods ObjectionsMethod
		{
			get { return m_eObjectionsMethod; }
			set { m_eObjectionsMethod = value; }
		}

		/// <summary>True to add new objection states when found in the import file</summary>
		public bool AddObjectionStates
		{
			get { return m_bAddObjectionStates; }
			set { m_bAddObjectionStates = value; }
		}

		/// <summary>True to add new objection rulings when found in the import file</summary>
		public bool AddObjectionRulings
		{
			get { return m_bAddObjectionRulings; }
			set { m_bAddObjectionRulings = value; }
		}

		/// <summary>True to ignore objection GUID values stored in the import file</summary>
		public bool DiscardObjectionsId
		{
			get { return m_bDiscardObjectionsId; }
			set { m_bDiscardObjectionsId = value; }
		}

		#endregion Properties

	}//	public class CTmaxImportOptions

}// namespace FTI.Shared.Trialmax
