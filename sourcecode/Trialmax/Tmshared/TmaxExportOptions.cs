using System;
using System.Collections;
using System.Windows.Forms;
using System.Text;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with an individual filter term</summary>
	public class CTmaxExportColumn : ITmaxSortable
	{
		#region Private Members
		
		/// <summary>Local member bound to TmaxEnumId property</summary>
		private TmaxExportColumns m_eTmaxEnumId = TmaxExportColumns.Invalid;
		
		/// <summary>Local member bound to CaseCode property</summary>
		private CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to CaseCodeId property</summary>
		private long m_lCaseCodeId = 0;
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Values property</summary>
		private ArrayList m_aValues = new ArrayList();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxExportColumn()
		{
		}
	
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">the source to be copied</param>
		public CTmaxExportColumn(CTmaxExportColumn tmaxSource)
		{
			if(tmaxSource != null) 
				Copy(tmaxSource);
		}
	
		/// <summary>This method will copy the properties of the specified source object</summary>
		public void Copy(CTmaxExportColumn tmaxSource)
		{
			this.Name = tmaxSource.Name;
			this.CaseCodeId = tmaxSource.CaseCodeId;
			this.CaseCode = tmaxSource.CaseCode;
			this.TmaxEnumId = tmaxSource.TmaxEnumId;
			
			this.Values.Clear();
			foreach(object O in tmaxSource.Values)
				this.Values.Add(O);
		
		}// public void Copy(CTmaxExportColumn tmaxColumn)
		
		/// <summary>Called to get text representation of this object</summary>
		public override string ToString()
		{
			if(this.GetName().Length > 0)
				return this.GetName();
			else
				return base.ToString();
				
		}// public override string ToString()

		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxColumn">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxColumn, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxExportColumn tmaxColumn, long lMode)
		{
			return -1;
					
		}// public int Compare(CTmaxExportColumn tmaxColumn, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxColumn, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxExportColumn)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method is called to get the name of the search field associated with this term</summary>
		/// <returns>The name of the field containing the data</returns>
		public string GetName()
		{
			//	Always give priority to the case code
			if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.Name.Length > 0))
				return m_tmaxCaseCode.Name;
			else if(m_eTmaxEnumId != TmaxExportColumns.Invalid)
				return m_eTmaxEnumId.ToString();
			else if((m_strName != null) && (m_strName.Length > 0))
				return m_strName;
			else
				return "";
		
		}// public string GetName()
		
		/// <summary>This method is called to set the name of the search field associated with this term</summary>
		/// <param name="strName">The name to assign to this term</param>
		public void SetName(string strName)
		{
			if(strName != null)
				m_strName = strName;
			else
				m_strName = "";
		
		}// public void SetName(string strName)
		
		/// <summary>This method is called to get the id of the case code associated with this term</summary>
		/// <returns>The id of the case code if applicable</returns>
		public long GetCaseCodeId()
		{
			//	Has a id been explicitly assigned?
			if(m_tmaxCaseCode != null)
				return m_tmaxCaseCode.UniqueId;
			else
				return m_lCaseCodeId ;
		
		}// public long GetCaseCodeId()
		
		/// <summary>This method is called to set the id of the case code bound to this term</summary>
		/// <param name="lId">The unique id of the case code</param>
		public void SetCaseCodeId(long lId)
		{
			m_lCaseCodeId = lId;
			
			//	Make sure they match if assigned
			if(m_tmaxCaseCode != null)
			{
				//	The code must match the id
				if(m_tmaxCaseCode.UniqueId != lId)
					m_tmaxCaseCode = null;
			}
		
		}// public void SetCaseCodeId(long lId)
		
		/// <summary>This method is called to get the case code associated with this term</summary>
		/// <returns>The case code if applicable</returns>
		public FTI.Shared.Trialmax.CTmaxCaseCode GetCaseCode()
		{
			return m_tmaxCaseCode;
		}
		
		/// <summary>This method is called to set the case code bound to this term</summary>
		/// <param name="tmaxCode">The case code</param>
		public void SetCaseCode(CTmaxCaseCode tmaxCode)
		{
			m_tmaxCaseCode = tmaxCode;
			
			//	Make sure the id matches if provided a valid code
			if(m_tmaxCaseCode != null)
			{
				//	The id must match the code
				m_lCaseCodeId = m_tmaxCaseCode.UniqueId;
				
				//	We'll use the name assigned to the code
				m_strName = "";
				
				//	Can't be an enumerated column at the same time
				m_eTmaxEnumId = TmaxExportColumns.Invalid;
			}
			else
			{
				// NOTE: We purposely do not clear the code id
			}
		
		}// public void SetCaseCode(CTmaxCaseCode tmaxCode)
		
		/// <summary>This method is called to set the enumerated identifier for the predefined column</summary>
		/// <param name="eId">The enumerated column identifier</param>
		public void SetTmaxEnumId(TmaxExportColumns eId)
		{
			m_eTmaxEnumId = eId;
			
			//	Can't be a case code if this is an enumerated id
			if(m_eTmaxEnumId != TmaxExportColumns.Invalid)
			{
				SetCaseCode(null);
			}
		
		}// public void SetTmaxEnumId(TmaxExportColumns eId)
		
		/// <summary>This method is called to get the enumerated identifier for the predefined column</summary>
		/// <returns>The enumerated column identifier</returns>
		public TmaxExportColumns GetTmaxEnumId()
		{
			return m_eTmaxEnumId;
		}

		/// <summary>This method is called to get the SQL statement required to create this column in a database</summary>
		/// <param name="tmaxOptions">The options being used for the export operation</param>
		/// <returns>The appropriate SQL statement</returns>
		public string GetSQLCreate(CTmaxExportOptions tmaxOptions)
		{
			string strSQL = "";
			string strName = "";

			//	Is this column bound to a case code?
			if(this.CaseCode != null)
			{
				strName = "[" + this.Name + "]";

				//	Is this a concatentated field?
				if((CaseCode.AllowMultiple == true) && (tmaxOptions.Concatenate == true))
				{
					strSQL = (strName + " MEMO");
				}
				else
				{
					//	What type of code is this
					switch(this.CaseCode.Type)
					{

						case TmaxCodeTypes.Date:

							strSQL = (strName + " DATETIME");
							break;

						case TmaxCodeTypes.Decimal:

							strSQL = (strName + " FLOAT");
							break;

						case TmaxCodeTypes.Boolean:
						case TmaxCodeTypes.Integer:

							strSQL = (strName + " LONG");
							break;

						case TmaxCodeTypes.Memo:

							strSQL = (strName + " MEMO");
							break;

						case TmaxCodeTypes.PickList:
						case TmaxCodeTypes.Text:
						default:

							strSQL = (strName + " TEXT(255)");
							break;

					}// switch(tmaxColumn.CaseCode.Type)

				}// if((CaseCode.AllowMultiple == true) && (tmaxOptions.Concatenate == true))

			}
			//	Is this the enumerated barcode column?
			else if(this.TmaxEnumId == TmaxExportColumns.Barcode)
			{
				strSQL = "Barcode TEXT(255)";
			}
			//	Is this the enumerated Name column?
			else if(this.TmaxEnumId == TmaxExportColumns.Name)
			{
				strSQL = "Name TEXT(255)";
			}

			return strSQL;

		}// public string GetSQLCreate()

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The Name that identifies the search field when the filter gets applied</summary>
		public string Name
		{
			get { return GetName(); }
			set { SetName(value); }
		}
		
		/// <summary>The case code that defines the search field when term is applied to the Codes table</summary>
		public CTmaxCaseCode CaseCode
		{
			get { return GetCaseCode(); }
			set { SetCaseCode(value); }
		}
		
		/// <summary>The id of the case code that defines the search field when term is applied to the Codes table</summary>
		public long CaseCodeId
		{
			get { return GetCaseCodeId(); }
			set { SetCaseCodeId(value); }
		}
		
		/// <summary>The enumerated id when associated with a predefined export column</summary>
		public TmaxExportColumns TmaxEnumId
		{
			get { return GetTmaxEnumId(); }
			set { SetTmaxEnumId(value); }
		}
		
		/// <summary>The collection of values (usually just 1) to be written to the export file</summary>
		/// <remarks>This allows us to support exporting of multiple codes of the same type assigned to a single record</remarks>
		public ArrayList Values
		{
			get { return m_aValues; }
		}
		
		#endregion Properties
		
	}// public class CTmaxExportColumn : ITmaxSortable

	/// <summary>This class manages a collection of filter columns</summary>
	public class CTmaxExportColumns : CTmaxSortedArrayList
	{
		#region Constants
		
		private const int ERROR_GET_LINES_EX	= 0;
		private const int ERROR_GET_LINE_EX		= 1;
		private const int ERROR_GET_HEADER_EX	= 2;

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;

		/// <summary>Local member bound to SortOrder property</summary>
		private long m_lSortOrder = 0;

		/// <summary>Local member bound to UseSortOrder property</summary>
		private bool m_bUseSortOrder = false;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxExportColumns() : base()
		{
			//	Assign a default sorter
			//base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
			
			m_tmaxEventSource.Name = "Export Columns Events";
			SetErrorStrings();
		}

		/// <summary>This method will copy the columns in the source collection to this collection</summary>
		/// <param name="tmaxSource">The source collection of columns</param>
		public void Copy(CTmaxExportColumns tmaxSource)
		{
			//	Clear the existing objects
			this.Clear();
			
			m_lSortOrder = tmaxSource.SortOrder;
			m_bUseSortOrder = tmaxSource.UseSortOrder;
			
			//	Copy each of the source columns
			if((tmaxSource != null) && (tmaxSource.Count > 0))
			{
				foreach(CTmaxExportColumn O in tmaxSource)
				{
					Add(new CTmaxExportColumn(O));
				}
				
			}// if((tmaxSource != null) && (tmaxSource.Count > 0))
			
		}// public void Copy(CTmaxExportColumns tmaxSource)
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxColumn">CTmaxExportColumn object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxExportColumn Add(CTmaxExportColumn tmaxColumn)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxColumn as object);

				return tmaxColumn;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxExportColumn Add(CTmaxExportColumn tmaxColumn)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxColumn">The filter object to be removed</param>
		public void Remove(CTmaxExportColumn tmaxColumn)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxColumn as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxExportColumn tmaxColumn)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxColumn">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxExportColumn tmaxColumn)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxColumn as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxExportColumn this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxExportColumn);
			}
		}

		/// <summary>Called to set the active collection of case codes</summary>
		/// <param name="tmaxCodes">The active collection of case codes</param>
		public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		{
			m_tmaxCaseCodes = tmaxCodes;
			
			//	Update each term in the collection
			foreach(CTmaxExportColumn O in this)
			{
				if(O.CaseCodeId != 0)
				{
					if(m_tmaxCaseCodes != null)
						O.SetCaseCode(m_tmaxCaseCodes.Find(O.CaseCodeId));
					else
						O.SetCaseCode(null);
				}
				
			}// foreach(CTmaxExportColumn O in this)
			
		}// public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		
		/// <summary>This method is called to get the get the values for each column supplied by the specified record</summary>
		/// <param name="tmaxRecord">The record being exported</param>
		/// <param name="tmaxOptions">The options used to format the lines</param>
		/// <returns>The maximum number of entries in any one column</returns>
		public int GetValues(ITmaxMediaRecord tmaxRecord, CTmaxExportOptions tmaxOptions)
		{
			int iMax = 0;
			
			//	Make sure we have columns being exported
			if(this.Count == 0) return 0;
			
			//	Make sure we have a record
			if(tmaxRecord == null) return 0;
			
			try
			{
				//	Start by iterating the list of columns and retrieving the values for each column
				foreach(CTmaxExportColumn O in this)
				{
					O.Values.Clear();
					
					//	Get the values for the specified record
					//
					//	NOTE:	We assume the caller has refreshed the fielded data if necessary
					tmaxRecord.GetExportValues(O, false, tmaxOptions);
					
					//	Keep track of the maximum number of rows
					if(O.Values.Count > iMax)
						iMax = O.Values.Count;
				
				}// foreach(CTmaxExportColumn O in this)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetValues", Ex);
			}
			
			return iMax;
				
		}// public int GetValues(ITmaxRecord tmaxRecord, CTmaxExportOptions tmaxOptions)

		/// <summary>The method constructs an export line for the columns header</summary>
		/// <param name="tmaxOptions">The formatting options to be applied</param>
		/// <returns>The formatted header line</returns>
		public string GetHeader(CTmaxExportOptions tmaxOptions)
		{
			string strLine = "";
			string strDelimiter = "";

			try
			{
				//	Get the delimiter text
				strDelimiter = tmaxOptions.GetDelimiter();

				//	Should we include the sort order column?
				if(this.UseSortOrder == true)
				{
					strLine = FormatString("SortOrder", tmaxOptions);
				}
				
				//	Use the column names to build the header line
				foreach(CTmaxExportColumn O in this)
				{
					//	Should we add the delimiter?
					if(strLine.Length > 0)
						strLine += strDelimiter;

					//	Add the value
					strLine += FormatString(O.Name, tmaxOptions);

				}// foreach(CTmaxExportColumn O in m_tmaxExportOptions.Columns)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetHeader", m_tmaxErrorBuilder.Message(ERROR_GET_HEADER_EX), Ex);
			}

			return strLine;

		}// public string GetHeader(CTmaxExportOptions tmaxOptions)

		/// <summary>This method is called to get the list of lines to be written to the export file for the specified record</summary>
		/// <param name="tmaxRecord">The record being exported</param>
		/// <param name="tmaxOptions">The options used to format the lines</param>
		/// <returns>The collection of export lines</returns>
		public ArrayList GetLines(ITmaxMediaRecord tmaxRecord, CTmaxExportOptions tmaxOptions)
		{
			int			iMaxRows = 0;
			ArrayList	aLines = null;
			
			//	Make sure we have columns being exported
			if(this.Count == 0) return null;
			
			//	Make sure we have a record
			if(tmaxRecord == null) return null;
			
			try
			{
				//	Get the values to be exported
				if((iMaxRows = GetValues(tmaxRecord, tmaxOptions)) > 0)
				{
					//	Now that we have all the values and we know how many
					//	rows will be required, we can build the lines
					aLines = new ArrayList();
				
					for(int i = 0; i < iMaxRows; i++)
					{
						string strLine = GetLine(tmaxRecord, i, tmaxOptions);
					
						if(strLine.Length > 0)
							aLines.Add(strLine);
				
					}// for(int i = 0; i < iMaxRows; i++)
							
				}// if((iMaxRows = GetValues(tmaxRecord, tmaxOptions)) > 0)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetLines", m_tmaxErrorBuilder.Message(ERROR_GET_LINES_EX, tmaxRecord.GetBarcode(false)), Ex);
			}
			
			//	We don't need the values any more
			ClearValues();
			
			//	Return the lines to the caller if we have any
			if((aLines != null) && (aLines.Count > 0))
				return aLines;
			else
				return null;
				
		}// public ArrayList GetLines(ITmaxRecord tmaxRecord, CTmaxExportOptions tmaxOptions)
		
		/// <summary>This method is called to clear the Values associated with each column in the collection</summary>
		public void ClearValues()
		{
			try
			{
				foreach(CTmaxExportColumn O in this)
					O.Values.Clear();
			}
			catch
			{
			
			}
			
		}// public void ClearValues()
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>The method constructs an export line for the values at the specified index</summary>
		/// <param name="tmaxRecord">The record being exported</param>
		/// <param name="iIndex">The index at which to retrieve the value being exported</param>
		/// <param name="tmaxOptions">The formatting options to be applied</param>
		/// <returns>The formatted export line</returns>
		private string GetLine(ITmaxMediaRecord tmaxRecord, int iIndex, CTmaxExportOptions tmaxOptions)
		{
			string	strLine = "";
			string	strDelimiter = "";
			string	strValue = "";
			
			try
			{
				//	Get the delimiter text
				strDelimiter = tmaxOptions.GetDelimiter();

				//	Should we include the sort order column?
				if(this.UseSortOrder == true)
				{
					strLine = FormatString(m_lSortOrder.ToString(), tmaxOptions);
					m_lSortOrder += 1;
				}

				//	Build the line to be written to the file
				foreach(CTmaxExportColumn O in this)
				{
					//	Do we have a value for this column?
					if(iIndex < O.Values.Count)
					{
						strValue = O.Values[iIndex].ToString();
					}
					else
					{
						//	Is this column bound to a case code?
						if(O.CaseCode != null)
						{
							//	Should we assign a default value?
							if(tmaxOptions.UseDefaults == true)
								strValue = O.CaseCode.GetDefault();
							else
								strValue = "";
						}
						//	Is this the enumerated barcode column?
						else if(O.TmaxEnumId == TmaxExportColumns.Barcode)
						{
							//	Always provide the barcode
							strValue = tmaxRecord.GetBarcode(false);
						}
						else
						{
							strValue = "";
						}
					
					}// if(iIndex < O.Values.Count)
					
					//	Should we format this field for text output?
					if(O.CaseCode != null)
					{
						if((O.CaseCode.Type == TmaxCodeTypes.Text) ||
						   (O.CaseCode.Type == TmaxCodeTypes.Memo))
						{
							strValue = FormatString(strValue, tmaxOptions);
						}
						
					}
					else if(O.TmaxEnumId != TmaxExportColumns.Invalid)
					{
						//	All enumerated columns are text based
						strValue = FormatString(strValue, tmaxOptions);
					}
	
					//	Should we add the delimiter?
					if(strLine.Length > 0)
						strLine += strDelimiter;
						
					//	Add the value
					strLine += strValue;
					
				}// foreach(CTmaxExportColumn O in m_tmaxExportOptions.Columns)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetLine", m_tmaxErrorBuilder.Message(ERROR_GET_LINE_EX, iIndex), Ex);
			}
			
			return strLine;
			
		}// private string GetLine(int iIndex, CTmaxExportOptions tmaxOptions)
		
		/// <summary>Formats the specified string for output as a text field</summary>
		/// <param name="strString">The string to be formatted</param>
		/// <param name="tmaxOptions">The user defined options to be applied</param>
		/// <returns>the formatted text string</returns>
		private string FormatString(string strString, CTmaxExportOptions tmaxOptions)
		{
			string strReplace = "";
			
			if(strString == null) return null;
			
			//	Are we supposed to add enclosing quotes?
			if(tmaxOptions.AddQuotes == true)
			{
				//	Do not quote zero-length strings
				if(strString.Length > 0)
					strString = String.Format("\"{0}\"", strString);
						
			}// if(tmaxOptions.AddQuotes == true)
	
			//	Should we replace CR/LF 
			if(tmaxOptions.CRLFReplacement != TmaxExportCRLF.None)
			{
				strReplace = tmaxOptions.GetCRLFReplacement();
				
				if(strReplace.Length > 0)
				{
					strString = strString.Replace("\r\n", strReplace);
					strString = strString.Replace("\n", strReplace);
					strString = strString.Replace("\r", strReplace);
				}
				
			}// if(tmaxOptions.CRLFReplacement != TmaxExportCRLF.None)
			
			return strString;
		
		}// private string FormatString(string strString)
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to construct the export lines for: %1");
			aStrings.Add("An exception was raised while attempting to construct the export line at index %1");
			aStrings.Add("An exception was raised while attempting to construct the export header line");

		}// private void SetErrorStrings()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The active collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
			set { SetCaseCodes(value); }
		}

		/// <summary>The value to be written to the sort order column</summary>
		public long SortOrder
		{
			get { return m_lSortOrder; }
			set { m_lSortOrder = value; }
		}

		/// <summary>True to include a sort order column in the export string</summary>
		public bool UseSortOrder
		{
			get { return m_bUseSortOrder; }
			set { m_bUseSortOrder = value; }
		}

		#endregion Properties
		
	}//	public class CTmaxExportColumns : CTmaxSortedArrayList

	/// <summary>This class encapsulates the options used to export records</summary>
	public class CTmaxExportOptions
	{
		#region Constants
		
		private const string XMLINI_DELIMITER_KEY					= "Delimiter";
		private const string XMLINI_ADD_QUOTES_KEY					= "AddQuotes";
		private const string XMLINI_SUB_BINDERS_KEY					= "SubBinders";
		private const string XMLINI_USE_DEFAULTS_KEY				= "UseDefaults";
		private const string XMLINI_AUTO_FILENAMES_KEY				= "AutoFilenames";
		private const string XMLINI_CONFIRM_OVERWRITE_KEY			= "ConfirmOverwrite";
		private const string XMLINI_COLUMN_HEADERS_KEY				= "ColumnHeaders";
		private const string XMLINI_CRLF_REPLACEMENT_KEY			= "CRLFReplacement";
		private const string XMLINI_USER_CRLF_REPLACEMENT_KEY		= "UserCRLF";
		private const string XMLINI_VIDEO_WMV_KEY					= "VideoWMV";
		private const string XMLINI_VIDEO_EDL_KEY					= "VideoEDL";
		private const string XMLINI_VIDEO_SAMI_KEY					= "VideoSAMI";
		private const string XMLINI_SAMI_LINES_KEY					= "SAMILines";
		private const string XMLINI_SAMI_FONT_FAMILY_KEY			= "SAMIFontFamily";
		private const string XMLINI_SAMI_FONT_SIZE_KEY				= "SAMIFontSize";
		private const string XMLINI_SAMI_FONT_COLOR_KEY				= "SAMIFontColor";
		private const string XMLINI_SAMI_FONT_HIGHLIGHTER_KEY		= "SAMIFontHighlighter";
		private const string XMLINI_SAMI_PAGE_NUMBERS_KEY			= "SAMIPageNumbers";
		private const string XMLINI_XML_SCRIPT_FORMAT_KEY			= "XmlScriptFormat";
		private const string XMLINI_INCLUDE_DEPOSITIONS_KEY			= "IncludeDepositions";
		private const string XMLINI_REFRESH_SOURCE_KEY				= "refreshSource";
		private const string XMLINI_CONCATENATOR_KEY				= "Concatenator";
		private const string XMLINI_CONCATENATE_KEY					= "Concatenate";
		private const string XMLINI_USER_CONCATENATOR_KEY			= "UserConcatenator";
		private const string XMLINI_INCLUDE_OBJECTIONS_KEY			= "IncludeObjections";

		private const string XMLINI_COLUMN_NAME_ATTRIBUTE			= "name";
		private const string XMLINI_COLUMN_CASE_CODE_ID_ATTRIBUTE	= "caseCodeId";
		private const string XMLINI_COLUMN_TMAX_ENUM_ID_ATTRIBUTE	= "tmaxEnumId";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member bound to Columns property</summary>
		private CTmaxExportColumns m_tmaxColumns = new CTmaxExportColumns();
		
		/// <summary>Local member bound to Delimiter property</summary>
		private TmaxExportDelimiters m_eDelimiter = TmaxExportDelimiters.Tab;
		
		/// <summary>Local member bound to CRLFReplacement property</summary>
		private TmaxExportCRLF m_eCRLFReplacement = TmaxExportCRLF.Pipe;
		
		/// <summary>Local member bound to Concatenator property</summary>
		private TmaxExportConcatenators m_eConcatenator = TmaxExportConcatenators.Comma;
		
		/// <summary>Local member bound to AddQuotes property</summary>
		private bool m_bAddQuotes = false;
		
		/// <summary>Local member bound to ColumnHeaders property</summary>
		private bool m_bColumnHeaders = true;
		
		/// <summary>Local member bound to SubBinders property</summary>
		private bool m_bSubBinders = false;
		
		/// <summary>Local member bound to UseDefaults property</summary>
		private bool m_bUseDefaults = false;
		
		/// <summary>Local member bound to AutoFilenames property</summary>
		private bool m_bAutoFilenames = true;
		
		/// <summary>Local member bound to ConfirmOverwrite property</summary>
		private bool m_bConfirmOverwrite = false;
		
		/// <summary>Local member bound to VideoWMV property</summary>
		private bool m_bVideoWMV = true;
		
		/// <summary>Local member bound to VideoEDL property</summary>
		private bool m_bVideoEDL = false;
		
		/// <summary>Local member bound to VideoSAMI property</summary>
		private bool m_bVideoSAMI = false;
		
		/// <summary>Local member bound to SAMIPageNumbers property</summary>
		private bool m_bSAMIPageNumbers = false;
		
		/// <summary>Local member bound to RefreshSource property</summary>
		private bool m_bRefreshSource = true;
		
		/// <summary>Local member bound to SAMILines property</summary>
		private int m_iSAMILines = 3;
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to UserCRLF property</summary>
		private string m_strUserCRLF = "";
		
		/// <summary>Local member bound to UserConcatenator property</summary>
		private string m_strUserConcatenator = "";
		
		/// <summary>Local member bound to SAMIFontFamily property</summary>
		private string m_strSAMIFontFamily = "Arial";
		
		/// <summary>Local member bound to SAMIFontHighlighter property</summary>
		private bool m_bSAMIFontHighlighter = true;
		
		/// <summary>Local member bound to SAMIFontSize property</summary>
		private int m_iSAMIFontSize = 12;
		
		/// <summary>Local member bound to SAMIFontColor property</summary>
		private System.Drawing.Color m_crSAMIFontColor = System.Drawing.Color.White;
		
		/// <summary>Local member bound to XmlScriptFormat property</summary>
		private TmaxXmlScriptFormats m_eXmlScriptFormat = TmaxXmlScriptFormats.Manager;
		
		/// <summary>Local member bound to IncludeDepositions property</summary>
		private bool m_bIncludeDepositions = false;

		/// <summary>Local member bound to IncludeObjections property</summary>
		private bool m_bIncludeObjections = false;

		/// <summary>Local member bound to Concatenate property</summary>
		private bool m_bConcatenate = false;

        /// <summary> To store video bitrate, default is 789 kbps</summary>
        private int m_iBitRate = 786;

		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxExportOptions()
		{
			//	Peform one time initialization
			Initialize();
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxOptions">The source object to copy</param>
		public CTmaxExportOptions(CTmaxExportOptions tmaxSource)
		{
			//	Peform one time initialization
			Initialize();

			if(tmaxSource != null)
				Copy(tmaxSource);
		
		}// public CTmaxExportOptions(CTmaxExportOptions tmaxSource)
		
		/// <summary>This method copies the source object</summary>
		/// <param name="tmaxOptions">The source object to copy</param>
		public void Copy(CTmaxExportOptions tmaxSource)
		{
			this.Name = tmaxSource.Name;
			this.AddQuotes = tmaxSource.AddQuotes;
			this.Delimiter = tmaxSource.Delimiter;
			this.SubBinders = tmaxSource.SubBinders;
			this.UseDefaults = tmaxSource.UseDefaults;
			this.ColumnHeaders = tmaxSource.ColumnHeaders;
			this.CRLFReplacement = tmaxSource.CRLFReplacement;
			this.UserCRLF = tmaxSource.UserCRLF;
			this.XmlScriptFormat = tmaxSource.XmlScriptFormat;
			this.IncludeDepositions = tmaxSource.IncludeDepositions;
			this.IncludeObjections = tmaxSource.IncludeObjections;
			this.SAMILines = tmaxSource.SAMILines;
			this.SAMIFontFamily = tmaxSource.SAMIFontFamily;
			this.SAMIFontSize = tmaxSource.SAMIFontSize;
			this.SAMIFontColor = tmaxSource.SAMIFontColor;
			this.SAMIFontHighlighter = tmaxSource.SAMIFontHighlighter;
			this.AutoFilenames = tmaxSource.AutoFilenames;
			this.ConfirmOverwrite = tmaxSource.ConfirmOverwrite;
			this.Columns.Copy(tmaxSource.Columns);
			this.SAMIPageNumbers = tmaxSource.SAMIPageNumbers;
			this.Concatenate = tmaxSource.Concatenate;
			this.Concatenator = tmaxSource.Concatenator;
			this.UserConcatenator = tmaxSource.UserConcatenator;
			
		}
		
		/// <summary>This method resets the object to its original state</summary>
		public void Clear()
		{
			m_tmaxColumns.Clear();
			m_eDelimiter = TmaxExportDelimiters.Tab;
			m_bAddQuotes = false;
			m_bSubBinders = false;
			m_bUseDefaults = false;
			m_bColumnHeaders = true;
			m_bIncludeDepositions = false;
			m_bIncludeObjections = false;
			m_bConcatenate = false;
			m_eConcatenator = TmaxExportConcatenators.Comma;
			m_eXmlScriptFormat = TmaxXmlScriptFormats.Manager;
			m_strUserConcatenator = "";
			m_strUserCRLF = "";
		
		}// public void Clear()
		
		/// <summary>This method is called to get the name of the section in the XML configuration file for this filter</summary>
		/// <returns>The name of the section containing the configuration information for this filter</returns>
		public string GetXmlSectionName()
		{
			if(m_strName.Length > 0)
				return ("trialMax/station/export/" + m_strName);
			else
				return "";
		
		}// public string GetXmlSectionName()
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			int					iLine = 1;
			CTmaxExportColumn	tmaxColumn = null;
			bool				bContinue = true;
			int					iColor = 0;
			
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;
		
			//	Read the properties from file
			m_eDelimiter = (TmaxExportDelimiters)(xmlIni.ReadInteger(XMLINI_DELIMITER_KEY, 0));
			m_bAddQuotes = xmlIni.ReadBool(XMLINI_ADD_QUOTES_KEY, m_bAddQuotes);
			m_bSubBinders = xmlIni.ReadBool(XMLINI_SUB_BINDERS_KEY, m_bSubBinders);
			m_bUseDefaults = xmlIni.ReadBool(XMLINI_USE_DEFAULTS_KEY, m_bUseDefaults);
			m_bAutoFilenames = xmlIni.ReadBool(XMLINI_AUTO_FILENAMES_KEY, m_bAutoFilenames);
			m_bConfirmOverwrite = xmlIni.ReadBool(XMLINI_CONFIRM_OVERWRITE_KEY, m_bConfirmOverwrite);
			m_bColumnHeaders = xmlIni.ReadBool(XMLINI_COLUMN_HEADERS_KEY, m_bColumnHeaders);
			m_eCRLFReplacement = (TmaxExportCRLF)(xmlIni.ReadInteger(XMLINI_CRLF_REPLACEMENT_KEY, (int)m_eCRLFReplacement));
			m_strUserCRLF = xmlIni.Read(XMLINI_USER_CRLF_REPLACEMENT_KEY, m_strUserCRLF);
			m_bVideoWMV = xmlIni.ReadBool(XMLINI_VIDEO_WMV_KEY, m_bVideoWMV);
			m_bVideoEDL = xmlIni.ReadBool(XMLINI_VIDEO_EDL_KEY, m_bVideoEDL);
			m_bVideoSAMI = xmlIni.ReadBool(XMLINI_VIDEO_SAMI_KEY, m_bVideoSAMI);
			m_iSAMILines = xmlIni.ReadInteger(XMLINI_SAMI_LINES_KEY, m_iSAMILines);
			m_strSAMIFontFamily = xmlIni.Read(XMLINI_SAMI_FONT_FAMILY_KEY, m_strSAMIFontFamily);
			m_iSAMIFontSize = xmlIni.ReadInteger(XMLINI_SAMI_FONT_SIZE_KEY, m_iSAMIFontSize);
			m_bSAMIFontHighlighter = xmlIni.ReadBool(XMLINI_SAMI_FONT_HIGHLIGHTER_KEY, m_bSAMIFontHighlighter);
			m_bSAMIPageNumbers = xmlIni.ReadBool(XMLINI_SAMI_PAGE_NUMBERS_KEY, m_bSAMIPageNumbers);
			m_eXmlScriptFormat = CTmaxToolbox.GetFormatFromString(xmlIni.Read(XMLINI_XML_SCRIPT_FORMAT_KEY, m_eXmlScriptFormat.ToString()));
			m_bIncludeDepositions = xmlIni.ReadBool(XMLINI_INCLUDE_DEPOSITIONS_KEY, m_bIncludeDepositions);
			m_bIncludeObjections = xmlIni.ReadBool(XMLINI_INCLUDE_OBJECTIONS_KEY, m_bIncludeObjections);
			m_bRefreshSource = xmlIni.ReadBool(XMLINI_REFRESH_SOURCE_KEY, m_bRefreshSource);
			m_bConcatenate = xmlIni.ReadBool(XMLINI_CONCATENATE_KEY, m_bConcatenate);
			m_eConcatenator = (TmaxExportConcatenators)(xmlIni.ReadInteger(XMLINI_CONCATENATOR_KEY, (int)m_eConcatenator));
			m_strUserConcatenator = xmlIni.Read(XMLINI_USER_CONCATENATOR_KEY, m_strUserConcatenator);

			try   
			{ 
				iColor = xmlIni.ReadInteger(XMLINI_SAMI_FONT_COLOR_KEY, m_crSAMIFontColor.ToArgb());
				m_crSAMIFontColor = System.Drawing.Color.FromArgb(iColor); 
			}
			catch 
			{ 
				m_crSAMIFontColor = System.Drawing.Color.White; 
			}

			//	Load all the columns
			m_tmaxColumns.Clear();
			while(bContinue == true)
			{
				tmaxColumn = new CTmaxExportColumn();
				
				if((bContinue = Load(xmlIni, GetIniKey(iLine++), tmaxColumn)) == true)
					m_tmaxColumns.Add(tmaxColumn);
			}
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			int iLine = 1;
			
			if(xmlIni.SetSection(GetXmlSectionName(), true, true) == false) return;
			
			//	Write the properties to file
			xmlIni.Write(XMLINI_DELIMITER_KEY, (int)(this.Delimiter));
			xmlIni.Write(XMLINI_ADD_QUOTES_KEY, m_bAddQuotes);
			xmlIni.Write(XMLINI_SUB_BINDERS_KEY, m_bSubBinders);
			xmlIni.Write(XMLINI_USE_DEFAULTS_KEY, m_bUseDefaults);
			xmlIni.Write(XMLINI_AUTO_FILENAMES_KEY, m_bAutoFilenames);
			xmlIni.Write(XMLINI_CONFIRM_OVERWRITE_KEY, m_bConfirmOverwrite);
			xmlIni.Write(XMLINI_COLUMN_HEADERS_KEY, m_bColumnHeaders);
			xmlIni.Write(XMLINI_CRLF_REPLACEMENT_KEY, (int)(m_eCRLFReplacement));
			xmlIni.Write(XMLINI_USER_CRLF_REPLACEMENT_KEY, m_strUserCRLF);
			xmlIni.Write(XMLINI_XML_SCRIPT_FORMAT_KEY, m_eXmlScriptFormat.ToString());
			xmlIni.Write(XMLINI_INCLUDE_DEPOSITIONS_KEY, m_bIncludeDepositions);
			xmlIni.Write(XMLINI_INCLUDE_OBJECTIONS_KEY, m_bIncludeObjections);
			xmlIni.Write(XMLINI_REFRESH_SOURCE_KEY, m_bRefreshSource);
			xmlIni.Write(XMLINI_VIDEO_WMV_KEY, m_bVideoWMV);
			xmlIni.Write(XMLINI_VIDEO_EDL_KEY, m_bVideoEDL);
			xmlIni.Write(XMLINI_VIDEO_SAMI_KEY, m_bVideoSAMI);
			xmlIni.Write(XMLINI_SAMI_LINES_KEY, m_iSAMILines);
			xmlIni.Write(XMLINI_SAMI_FONT_FAMILY_KEY, m_strSAMIFontFamily);
			xmlIni.Write(XMLINI_SAMI_FONT_SIZE_KEY, m_iSAMIFontSize);
			xmlIni.Write(XMLINI_SAMI_FONT_HIGHLIGHTER_KEY, m_bSAMIFontHighlighter);
			xmlIni.Write(XMLINI_SAMI_FONT_COLOR_KEY, m_crSAMIFontColor.ToArgb());
			xmlIni.Write(XMLINI_SAMI_PAGE_NUMBERS_KEY, m_bSAMIPageNumbers);
			xmlIni.Write(XMLINI_CONCATENATE_KEY, m_bConcatenate);
			xmlIni.Write(XMLINI_CONCATENATOR_KEY, (int)(m_eConcatenator));
			xmlIni.Write(XMLINI_USER_CONCATENATOR_KEY, m_strUserConcatenator);

			//	Now write the columns
			if(m_tmaxColumns != null)
			{
				foreach(CTmaxExportColumn O in m_tmaxColumns)
				{
					Save(xmlIni, GetIniKey(iLine++), O);
				}

			}
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method get the delimiter text requested by the user</summary>
		/// <returns>the delimiter text</returns>
		public string GetDelimiter()
		{
			return GetDelimiter(this.Delimiter);	
		}
		
		/// <summary>This method converts the enumerated delimiter to the appropriate text</summary>
		/// <param name="eDelimiter">the enumerated delimiter identifier</param>
		/// <returns>the delimiter text</returns>
		static public string GetDelimiter(TmaxExportDelimiters eDelimiter)
		{
			switch(eDelimiter)
			{
				case TmaxExportDelimiters.Comma:
				
					return ",";
				
				case TmaxExportDelimiters.Pipe:
				
					return "|";
					
				case TmaxExportDelimiters.Tab:
				default:
				
					return "\t";
					
			}// switch(eDelimiter)
			
		}// static public string GetDelimiter(TmaxExportDelimiters eDelimiter)
		
		/// <summary>This method converts the enumerated substitution to the appropriate text</summary>
		/// <returns>the replacement text</returns>
		public string GetCRLFReplacement()
		{
			if(this.CRLFReplacement == TmaxExportCRLF.User)
				return m_strUserCRLF;
			else
				return GetCRLFReplacement(m_eCRLFReplacement);
		}
		
		/// <summary>This method converts the enumerated replacement to the appropriate text</summary>
		/// <param name="eReplacement">the enumerated replacement identifier</param>
		/// <returns>the replacement text</returns>
		static public string GetCRLFReplacement(TmaxExportCRLF eReplacement)
		{
			switch(eReplacement)
			{
				case TmaxExportCRLF.HTML:
				
					return "<BR>";
				
				case TmaxExportCRLF.Summation:
				
					char cSummation = (char)0x0b;
					return cSummation.ToString();
				
				case TmaxExportCRLF.Space:
				
					return " ";
					
				case TmaxExportCRLF.Pipe:
				
					return "|";
					
				case TmaxExportCRLF.User:
				case TmaxExportCRLF.None:
				default:
				
					return "";
					
			}// switch(eReplacement)
			
		}// static public string GetCRLFReplacement(TmaxExportCRLF eReplacement)
		
		/// <summary>This method converts the enumerated substitution to the appropriate text</summary>
		/// <returns>the replacement text</returns>
		public string GetConcatenator()
		{
			if(this.Concatenator == TmaxExportConcatenators.User)
				return m_strUserConcatenator;
			else
				return GetConcatenator(m_eConcatenator);
		}
		
		/// <summary>This method converts the enumerated concatenator to the appropriate text</summary>
		/// <param name="eConcatenator">the enumerated concatenator identifier</param>
		/// <returns>the concatenator text</returns>
		static public string GetConcatenator(TmaxExportConcatenators eConcatenator)
		{
			switch(eConcatenator)
			{
				case TmaxExportConcatenators.Comma:
				
					return ",";
				
				case TmaxExportConcatenators.Semicolon:
				
					return ";";
				
				case TmaxExportConcatenators.Pipe:
				
					return "|";
					
				case TmaxExportConcatenators.Tilde:
				
					return "~";

				case TmaxExportConcatenators.HardReturn:

					return "\r\n";

				case TmaxExportConcatenators.User:
				default:
				
					return "";
					
			}// switch(eConcatenator)
			
		}// static public string GetConcatenator(TmaxExportConcatenators eConcatenator)
		
		/// <summary>This method get the text used to display the replacement option</summary>
		/// <param name="eReplacement">the enumerated replacement identifier</param>
		/// <returns>the display text</returns>
		static public string GetDisplayText(TmaxExportCRLF eReplacement)
		{
			switch(eReplacement)
			{
				case TmaxExportCRLF.HTML:
				
					return "<BR>";
				
				case TmaxExportCRLF.Summation:
				
					return "Summation [0xB]";
				
				case TmaxExportCRLF.Space:
				case TmaxExportCRLF.Pipe:
				case TmaxExportCRLF.User:
				case TmaxExportCRLF.None:
				default:
				
					return eReplacement.ToString();
					
			}// switch(eReplacement)
			
		}// static public string GetCRLFReplacement(TmaxExportCRLF eReplacement)

		/// <summary>This method get the text used to display the concatenator option</summary>
		/// <param name="eConcatenator">the enumerated replacement identifier</param>
		/// <returns>the display text</returns>
		static public string GetDisplayText(TmaxExportConcatenators eConcatenator)
		{
			switch(eConcatenator)
			{
				case TmaxExportConcatenators.HardReturn:

					return "Hard Return";

				case TmaxExportConcatenators.Comma:
				case TmaxExportConcatenators.Semicolon:
				case TmaxExportConcatenators.Pipe:
				case TmaxExportConcatenators.Tilde:
				case TmaxExportConcatenators.User:
				default:

					return eConcatenator.ToString();

			}// switch(eConcatenator)

		}// sstatic public string GetDisplayText(TmaxExportConcatenators eConcatenator)

		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method converts the numeric line identifier to a unique key for storing the column</summary>
		/// <param name="iTerm">unique numeric identifier for the column</param>
		/// <returns>The appropriate XML Ini key</returns>
		private string GetIniKey(int iColumn)
		{
			return String.Format("C{0}", iColumn);
		}
		
		/// <summary>This method is called to store the specified filter term in the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxColumn">The term to be stored in the file</param>
		private bool Save(CXmlIni xmlIni, string strKey, CTmaxExportColumn tmaxColumn)
		{
			try
			{
				xmlIni.Write(strKey, XMLINI_COLUMN_NAME_ATTRIBUTE, tmaxColumn.Name);
				xmlIni.Write(strKey, XMLINI_COLUMN_CASE_CODE_ID_ATTRIBUTE, tmaxColumn.CaseCodeId);
				xmlIni.Write(strKey, XMLINI_COLUMN_TMAX_ENUM_ID_ATTRIBUTE, (int)(tmaxColumn.TmaxEnumId));
				return true;
			}
			catch
			{
				return false;
			}
			
		}// private bool Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to load the information for the specified column from the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxColumn">The column to be initialized</param>
		///	<returns>true to continue loading</returns>
		private bool Load(CXmlIni xmlIni, string strKey, CTmaxExportColumn tmaxColumn)
		{
			try
			{
				//	Read the name of this term
				tmaxColumn.Name = xmlIni.Read(strKey, XMLINI_COLUMN_NAME_ATTRIBUTE, "");
				
				//	Have we run out of columns?
				if(tmaxColumn.Name.Length == 0) return false;
				
				//	Read the rest of the property values
				tmaxColumn.CaseCodeId = xmlIni.ReadLong(strKey, XMLINI_COLUMN_CASE_CODE_ID_ATTRIBUTE, 0);
				
				try { tmaxColumn.TmaxEnumId = (TmaxExportColumns)(xmlIni.ReadInteger(strKey, XMLINI_COLUMN_TMAX_ENUM_ID_ATTRIBUTE, 0)); }
				catch{}
				
				return true;
			}
			catch
			{
				//	NOTE: We don't return FALSE because the rest of the columns might be OK
			}
			
			return true;
			
		}// private bool Save(CXmlIni xmlIni)
		
		/// <summary>Called to initialize the object at construction</summary>
		private void Initialize()
		{
			//	Set up the event source
			SetErrorStrings();			
			m_tmaxEventSource.Name = "Export Options Events";
			m_tmaxEventSource.Attach(m_tmaxColumns.EventSource);
		
		}// private void Initialize()
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			//			aStrings.Add("An exception was raised while attempting to open the XML case options: filename = %1");
			//			aStrings.Add("Unable to save the case options. The XML file has not been opened.");
			//			aStrings.Add("An exception was raised while attempting to save the XML case options: filename = %1");
			//			aStrings.Add("An exception was raised while attempting to initialize the case options: filename = %1");

		}// private void SetErrorStrings()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The EventSource for this object</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <ssummary>Collection of columns to be exported</summary>
		public CTmaxExportColumns Columns
		{
			get { return m_tmaxColumns; }
		}
		
		/// <summary>The Name used to store the information for this filter in a TrialMax XML configuration file</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>Enumerated delimiter identifier</summary>
		public TmaxExportDelimiters Delimiter
		{
			get { return m_eDelimiter; }
			set { m_eDelimiter = value; }
		}
		
		/// <summary>Enumerated CRLF Replacement identifier</summary>
		public TmaxExportCRLF CRLFReplacement
		{
			get { return m_eCRLFReplacement; }
			set { m_eCRLFReplacement = value; }
		}
		
		/// <summary>Enumerated Concatenator identifier</summary>
		public TmaxExportConcatenators Concatenator
		{
			get { return m_eConcatenator; }
			set { m_eConcatenator = value; }
		}
		
		/// <summary>User defined CRLF Replacement</summary>
		public string UserCRLF
		{
			get { return m_strUserCRLF; }
			set { m_strUserCRLF = value; }
		}
		
		/// <summary>User defined Concatenator</summary>
		public string UserConcatenator
		{
			get { return m_strUserConcatenator; }
			set { m_strUserConcatenator = value; }
		}
		
		/// <summary>True to concatenate multiple fields</summary>
		public bool Concatenate
		{
			get { return m_bConcatenate; }
			set { m_bConcatenate = value; }
		}
		
		/// <summary>True to add quotes around string values</summary>
		public bool AddQuotes
		{
			get { return m_bAddQuotes; }
			set { m_bAddQuotes = value; }
		}
		
		/// <summary>True to use default values if actual values to not exist</summary>
		public bool UseDefaults
		{
			get { return m_bUseDefaults; }
			set { m_bUseDefaults = value; }
		}
		
		/// <summary>True to drill into sub binders</summary>
		public bool SubBinders
		{
			get { return m_bSubBinders; }
			set { m_bSubBinders = value; }
		}
		
		/// <summary>True to automatically assign filenames on multiple export</summary>
		public bool AutoFilenames
		{
			get { return m_bAutoFilenames; }
			set { m_bAutoFilenames = value; }
		}
		
		/// <summary>True to confirm before overwriting an existing file</summary>
		public bool ConfirmOverwrite
		{
			get { return m_bConfirmOverwrite; }
			set { m_bConfirmOverwrite = value; }
		}
		
		/// <summary>True to add column headers</summary>
		public bool ColumnHeaders
		{
			get { return m_bColumnHeaders; }
			set { m_bColumnHeaders = value; }
		}
		
		/// <summary>True to export to WMV video file</summary>
		public bool VideoWMV
		{
			get { return m_bVideoWMV; }
			set { m_bVideoWMV = value; }
		}
		
		/// <summary>True to export to EDL video file</summary>
		public bool VideoEDL
		{
			get { return m_bVideoEDL; }
			set { m_bVideoEDL = value; }
		}
		
		/// <summary>True to export to SAMI video file</summary>
		public bool VideoSAMI
		{
			get { return m_bVideoSAMI; }
			set { m_bVideoSAMI = value; }
		}
		
		/// <summary>The number of visible lines in the SAMI file</summary>
		public int SAMILines
		{
			get { return m_iSAMILines; }
			set { m_iSAMILines = value; }
		}
		
		/// <summary>The font to use for SAMI text</summary>
		public string SAMIFontFamily
		{
			get { return m_strSAMIFontFamily; }
			set { m_strSAMIFontFamily = value; }
		}
		
		/// <summary>The color to use for SAMI text</summary>
		public System.Drawing.Color SAMIFontColor
		{
			get { return m_crSAMIFontColor; }
			set { m_crSAMIFontColor = value; }
		}
		
		/// <summary>True to use highlighter color for text color</summary>
		public bool SAMIFontHighlighter
		{
			get { return m_bSAMIFontHighlighter; }
			set { m_bSAMIFontHighlighter = value; }
		}
		
		/// <summary>Point size for SAMI text</summary>
		public int SAMIFontSize
		{
			get { return m_iSAMIFontSize; }
			set { m_iSAMIFontSize = value; }
		}
		
		/// <summary>Include Page and Line numbers in SAMI text</summary>
		public bool SAMIPageNumbers
		{
			get { return m_bSAMIPageNumbers; }
			set { m_bSAMIPageNumbers = value; }
		}
		
		/// <summary>Format to be used when exporting scripts to XML</summary>
		public TmaxXmlScriptFormats XmlScriptFormat
		{
			get { return m_eXmlScriptFormat; }
			set { m_eXmlScriptFormat = value; }
		}
		
		/// <summary>Include deposition in the output file</summary>
		public bool IncludeDepositions
		{
			get { return m_bIncludeDepositions; }
			set { m_bIncludeDepositions = value; }
		}

		/// <summary>Include objections in the output file</summary>
		public bool IncludeObjections
		{
			get { return m_bIncludeObjections; }
			set { m_bIncludeObjections = value; }
		}

		/// <summary>True to refresh the source before export</summary>
		public bool RefreshSource
		{
			get { return m_bRefreshSource; }
			set { m_bRefreshSource = value; }
		}

        public int VideoBitRate
        {
            get 
            {
                return m_iBitRate;
            }
            set
            {
                m_iBitRate = value;
            }
        }
		#endregion Properties

	}//	public class CTmaxExportOptions

}// namespace FTI.Shared.Trialmax
