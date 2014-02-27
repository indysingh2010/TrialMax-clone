using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with an individual filter term</summary>
	public class CTmaxFilterTerm : ITmaxSortable
	{
		#region Private Members
		
		/// <summary>Local member bound to CaseCode property</summary>
		private CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to MultiLevelSelection property</summary>
		private CTmaxPickItem m_tmaxMultiLevelSelection = null;
		
		/// <summary>Local member bound to CaseCodeId property</summary>
		private long m_lCaseCodeId = 0;
		
		/// <summary>Local member bound to MultiLevelSelectionId property</summary>
		private long m_lMultiLevelSelectionId = 0;
		
		/// <summary>Local member bound to Comparison property</summary>
		private TmaxFilterComparisons m_eComparison = TmaxFilterComparisons.Equals;
		
		/// <summary>Local member bound to Modifier property</summary>
		private TmaxFilterModifiers m_eModifier = TmaxFilterModifiers.None;
		
		/// <summary>Local member bound to Value property</summary>
		private string m_strValue = "";
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxFilterTerm()
		{
		}
	
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">the source term to be copied</param>
		public CTmaxFilterTerm(CTmaxFilterTerm tmaxSource)
		{
			if(tmaxSource != null) 
				Copy(tmaxSource);
		}
	
		/// <summary>This method will copy the properties of the specified source object</summary>
		public void Copy(CTmaxFilterTerm tmaxSource)
		{
			this.CaseCode = tmaxSource.CaseCode;
			this.CaseCodeId = tmaxSource.CaseCodeId;
			this.MultiLevelSelection = tmaxSource.MultiLevelSelection;
			this.MultiLevelSelectionId = tmaxSource.MultiLevelSelectionId;
			this.Name = tmaxSource.Name;
			this.Comparison = tmaxSource.Comparison;
			this.Modifier = tmaxSource.Modifier;
			this.Value = tmaxSource.Value;
		
		}// public void Copy(CTmaxFilterTerm tmaxTerm)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxTerm">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxTerm, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxFilterTerm tmaxTerm, long lMode)
		{
			return -1;
					
		}// public int Compare(CTmaxFilterTerm tmaxTerm, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxTerm, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxFilterTerm)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method is called to get the name of the search field associated with this term</summary>
		/// <returns>The name of the field containing the data</returns>
		public string GetName()
		{
			//	Always give priority to the case code
			if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.Name.Length > 0))
				return m_tmaxCaseCode.Name;
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
			if(m_lCaseCodeId != 0)
				return m_lCaseCodeId;
			else if(m_tmaxCaseCode != null)
				return m_tmaxCaseCode.UniqueId;
			else
				return 0 ;
		
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
			}
			else
			{
				// NOTE: We purposely do not clear the code id
			}
		
		}// public void SetCaseCode(CTmaxCaseCode tmaxCode)
		
		/// <summary>This method is called to get the id of the selected pick list value when case code is a multilevel code</summary>
		/// <returns>The id of the case code if applicable</returns>
		public long GetMultiLevelSelectionId()
		{
			//	Has a id been explicitly assigned?
			if(m_lMultiLevelSelectionId != 0)
				return m_lMultiLevelSelectionId;
			else if(m_tmaxMultiLevelSelection != null)
				return m_tmaxMultiLevelSelection.UniqueId;
			else
				return 0 ;
		
		}// public long GetMultiLevelSelectionId()
		
		/// <summary>This method is called to set the id of the multilevel selection</summary>
		/// <param name="lId">The unique id of the multilevel selection</param>
		public void SetMultiLevelSelectionId(long lId)
		{
			m_lMultiLevelSelectionId = lId;
			
			//	Make sure they match if assigned
			if(m_tmaxMultiLevelSelection != null)
			{
				if(m_tmaxMultiLevelSelection.UniqueId != lId)
					m_tmaxMultiLevelSelection = null;
			}
		
		}// public void SetMultiLevelSelectionId(long lId)
		
		/// <summary>This method is called to get the multilevel selection associated with this term</summary>
		/// <returns>The multilevel pick list selection if applicable</returns>
		public FTI.Shared.Trialmax.CTmaxPickItem GetMultiLevelSelection()
		{
			return m_tmaxMultiLevelSelection;
		}
		
		/// <summary>This method is called to set the multilevel pick list selection bound to this term</summary>
		/// <param name="tmaxCode">The multilevel pick list selection</param>
		public void SetMultiLevelSelection(CTmaxPickItem tmaxSelection)
		{
			m_tmaxMultiLevelSelection = tmaxSelection;
			
			//	Make sure the id matches if provided a valid code
			if(m_tmaxMultiLevelSelection != null)
			{
				//	The id must match the code
				m_lMultiLevelSelectionId = m_tmaxMultiLevelSelection.UniqueId;
			}
			else
			{
				// NOTE: We purposely do not clear the id
			}
		
		}// public void SetMultiLevelSelection(CTmaxPickItem tmaxSelection)
		
		/// <summary>Called to get the values required to construct the SQL filter for the specified pick list item</summary>
		/// <param name="tmaxPickItem">The pick list item to be filtered</param>
		/// <returns>The SQL value statement</returns>
		public string GetSQLValue(CTmaxPickItem tmaxPickItem)
		{
			string strValue = "";
			string strChild = "";
			
			//	What type of pick item?
			switch(tmaxPickItem.Type)
			{
				case TmaxPickItemTypes.Value:
					
					strValue = tmaxPickItem.UniqueId.ToString();
					break;
					
				case TmaxPickItemTypes.MultiLevel:
				case TmaxPickItemTypes.StringList:
				
					if((tmaxPickItem.Children != null) && (tmaxPickItem.Children.Count > 0))
					{
						foreach(CTmaxPickItem O in tmaxPickItem.Children)
						{
							strChild = GetSQLValue(O);
							if(strChild.Length > 0)
							{
								if(strValue.Length > 0)
									strValue += ",";
								strValue += strChild;
							}
							
						}// foreach(CTmaxPickItem O in tmaxPickItem.Children)
						
					}// if((tmaxPickItem.Children != null) && (tmaxPickItem.Children.Count > 0))
					break;
					
				case TmaxPickItemTypes.Unknown:
				default:
					
					break;	
					
			}// switch(tmaxPickItem.Type)	
			
			return strValue;			
			
		}// public string GetSQLValue(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method is called to get the SQL statement for this term</summary>
		/// <returns>The SQL statement for this term</returns>
		public string GetSQL()
		{
			TmaxFilterModifiers	eModifier = TmaxFilterModifiers.None;
			string				strValue = "";
			string				strPickValue = "";
			string				strSQL = "";
			string				strSelect = "";
			string				strColumn = "";
			string				strCondition = "";
			string				strEval = "";
			CTmaxPickItem		tmaxPickValue = null;
			
			//	Got to have the case code to generate the SQL
			if(m_tmaxCaseCode == null) return "";
			
			//	Check for the special case where we don't have to be concerned with the value
			if(this.Comparison == TmaxFilterComparisons.AnyValue)
			{
				strSelect = String.Format("SELECT PrimaryId FROM Codes WHERE (CaseCodeId = {0})", m_tmaxCaseCode.UniqueId);
				
				if(this.Modifier == TmaxFilterModifiers.NOT)
					strSQL = String.Format("AutoId NOT IN ({0})", strSelect);
				else
					strSQL = String.Format("AutoId IN ({0})", strSelect);
				
				return strSQL;
			
			}// if(this.Comparison == TmaxFilterComparisons.AnyValue)
			
			strColumn = m_tmaxCaseCode.GetDbColumn();
			if(strColumn.Length == 0) return "";
			
			//	All statements start with this
			strSelect = String.Format("SELECT PrimaryId FROM Codes WHERE (CaseCodeId = {0} AND ", m_tmaxCaseCode.UniqueId);
			
			//	Get the modifier and value to be used for the statement
			eModifier = this.Modifier;
			
			//	Is the pick list necessary?
			if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
			{
				//	Is this a multilevel code?
				if(m_tmaxCaseCode.IsMultiLevel == true)
					tmaxPickValue = this.MultiLevelSelection;
				else if(m_tmaxCaseCode.PickList != null)
					tmaxPickValue = m_tmaxCaseCode.PickList.FindChild(this.Value);

				if(tmaxPickValue != null)
					strPickValue = GetSQLValue(tmaxPickValue);
				
				if(strPickValue.Length > 0)
					strValue = String.Format("({0})", strPickValue);
				else
					return "";
			}
			else
			{			
				strValue = this.Value;
			}
			
			//	Force booleans to always be TRUE or NOT TRUE instead of allowing FALSE
			if(m_tmaxCaseCode.Type == TmaxCodeTypes.Boolean)
			{
				if(CTmaxToolbox.StringToBool(strValue) == false)
				{
					if(eModifier == TmaxFilterModifiers.NOT)
					   eModifier = TmaxFilterModifiers.None;
					else
					   eModifier = TmaxFilterModifiers.NOT;
					
					strValue = "True"; // Always TRUE
				}
				
			}// if(m_tmaxCaseCode.Type == TmaxCodeTypes.Boolean)
			
			//	Construct the condition portion of the statement
			switch(this.Comparison)
			{
				case TmaxFilterComparisons.Contains:
				case TmaxFilterComparisons.StartsWith:
				case TmaxFilterComparisons.EndsWith:
				
					strCondition += "LIKE";
					break;
					
				case TmaxFilterComparisons.LessThan:
				
					strCondition += "<";
					break;
					
				case TmaxFilterComparisons.LessThanEquals:
				
					strCondition += "<=";
					break;
					
				case TmaxFilterComparisons.GreaterThan:
				
					strCondition += ">";
					break;
					
				case TmaxFilterComparisons.GreaterThanEquals:
				
					strCondition += ">=";
					break;
					
				case TmaxFilterComparisons.Equals:
				
					if((m_tmaxCaseCode.Type == TmaxCodeTypes.Text) ||
					   (m_tmaxCaseCode.Type == TmaxCodeTypes.Memo))
					{
						strCondition += "LIKE";
					}
					else if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
					{
						strCondition += "IN";
					}
					else
					{
						strCondition += "=";
					}
					break;
					
				default:
				
					Debug.Assert(false, this.Comparison.ToString() + " is not a valid comparison");
					return "";			
			}
			
			//	What type of data is bound to this term
			switch(m_tmaxCaseCode.Type)
			{
				case TmaxCodeTypes.Decimal:
				case TmaxCodeTypes.Integer:
				case TmaxCodeTypes.PickList:
				
					strEval = String.Format("{0} {1} {2})", strColumn, strCondition, strValue);
					break;
					
				case TmaxCodeTypes.Boolean:
				
					if(CTmaxToolbox.StringToBool(strValue) == true)
						strEval = String.Format("{0} {1} TRUE)", strColumn, strCondition);
					else
						strEval = String.Format("{0} {1} FALSE)", strColumn, strCondition);
					break;
					
				case TmaxCodeTypes.Date:
				
					strEval = String.Format("{0} {1} #{2}#)", strColumn, strCondition, strValue);
					break;
					
				case TmaxCodeTypes.Text:
				case TmaxCodeTypes.Memo:
				
					if(this.Comparison == TmaxFilterComparisons.Contains)
						strEval = String.Format("{0} {1} '%{2}%')", strColumn, strCondition, CTmaxToolbox.SQLEncode(strValue));
					else if(this.Comparison == TmaxFilterComparisons.StartsWith)
						strEval = String.Format("{0} {1} '{2}%')", strColumn, strCondition, CTmaxToolbox.SQLEncode(strValue));
					else if(this.Comparison == TmaxFilterComparisons.EndsWith)
						strEval = String.Format("{0} {1} '%{2}')", strColumn, strCondition, CTmaxToolbox.SQLEncode(strValue));
					else
						strEval = String.Format("{0} {1} '{2}')", strColumn, strCondition, CTmaxToolbox.SQLEncode(strValue));
					break;
				
				case TmaxCodeTypes.Unknown:
				default:
				
					break;
				
			}// switch(m_tmaxCaseCode.Type)
			
			if(strEval.Length > 0)
			{
				//	This completes the SELECT portion of the statement
				strSelect += strEval;
				
				//	This applies the modifier to build the complete SQL statement
				if(eModifier == TmaxFilterModifiers.NOT)
					strSQL = String.Format("AutoId NOT IN ({0})", strSelect);
				else
					strSQL = String.Format("AutoId IN ({0})", strSelect);
			}
				
			return strSQL;
					
		}// public string GetSQL()
		
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
		
		/// <summary>The user selection when the case code is bound to a multi-level pick list</summary>
		public CTmaxPickItem MultiLevelSelection
		{
			get { return GetMultiLevelSelection(); }
			set { SetMultiLevelSelection(value); }
		}
		
		/// <summary>The id of the user selection when the case code is bound to a multi-level pick list</summary>
		public long MultiLevelSelectionId
		{
			get { return GetMultiLevelSelectionId(); }
			set { SetMultiLevelSelectionId(value); }
		}
		
		/// <summary>The Value assigned to this term</summary>
		public string Value
		{
			get { return m_strValue; }
			set { m_strValue = value; }
		}
		
		/// <summary>The comparison applied to this Value and the value stored in the database</summary>
		public TmaxFilterComparisons Comparison
		{
			get { return m_eComparison; }
			set { m_eComparison = value; }
		}
		
		/// <summary>The modifier applied to this term in the filter expression</summary>
		public TmaxFilterModifiers Modifier
		{
			get { return m_eModifier; }
			set { m_eModifier = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxFilterTerm : ITmaxSortable

	/// <summary>This class manages a collection of filter terms</summary>
	public class CTmaxFilterTerms : CTmaxSortedArrayList
	{
		#region Private Members
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to PickLists property</summary>
		private CTmaxPickItem m_tmaxPickLists = null;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxFilterTerms() : base()
		{
			//	Assign a default sorter
			//base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
			
			m_tmaxEventSource.Name = "Filter Terms Events";
		}

		/// <summary>This method will copy the terms in the source collection to this collection</summary>
		/// <param name="tmaxSource">The source collection of terms</param>
		public void Copy(CTmaxFilterTerms tmaxSource)
		{
			//	Clear the existing objects
			this.Clear();
			
			m_tmaxCaseCodes = tmaxSource.CaseCodes;
			m_tmaxPickLists = tmaxSource.PickLists;
			
			//	Copy each of the source terms
			if((tmaxSource != null) && (tmaxSource.Count > 0))
			{
				foreach(CTmaxFilterTerm O in tmaxSource)
				{
					Add(new CTmaxFilterTerm(O));
				}
				
			}// if((tmaxSource != null) && (tmaxSource.Count > 0))
			
		}// public void Copy(CTmaxFilterTerms tmaxSource)
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxTerm">CTmaxFilterTerm object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxFilterTerm Add(CTmaxFilterTerm tmaxTerm)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxTerm as object);

				return tmaxTerm;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxFilterTerm Add(CTmaxFilterTerm tmaxTerm)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxTerm">The filter object to be removed</param>
		public void Remove(CTmaxFilterTerm tmaxTerm)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxTerm as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxFilterTerm tmaxTerm)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxTerm">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxFilterTerm tmaxTerm)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxTerm as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxFilterTerm this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxFilterTerm);
			}
		}

		/// <summary>Called to set the active collection of case codes</summary>
		/// <param name="tmaxCodes">The active collection of case codes</param>
		public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		{
			m_tmaxCaseCodes = tmaxCodes;

			//	Update each term in the collection
			foreach(CTmaxFilterTerm O in this)
			{
				if(O.CaseCodeId != 0)
				{
					if(m_tmaxCaseCodes != null)
						O.SetCaseCode(m_tmaxCaseCodes.Find(O.CaseCodeId));
					else
						O.SetCaseCode(null);
				}
				
			}// foreach(CTmaxFilterTerm O in this)
			
		}// public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		
		/// <summary>Called to set the active collection of pick lists</summary>
		/// <param name="tmaxPickLists">The active collection of pick lists</param>
		public void SetPickLists(CTmaxPickItem tmaxPickLists)
		{
			m_tmaxPickLists = tmaxPickLists;

			//	Update each term in the collection
			foreach(CTmaxFilterTerm O in this)
			{
				if(O.MultiLevelSelectionId != 0)
				{
					if(m_tmaxPickLists != null)
						O.SetMultiLevelSelection(m_tmaxPickLists.FindValue(O.MultiLevelSelectionId));
					else
						O.SetMultiLevelSelection(null);
				}
				
			}// foreach(CTmaxFilterTerm O in this)
			
		}// public void SetPickLists(CTmaxPickItem tmaxPickLists)
		
		/// <summary>This method is called to get the SQL statement for the terms in this collection</summary>
		/// <param name="eOperator">The operator used to join the terms</param>
		/// <returns>The SQL statement for this collection</returns>
		public string GetSQL(TmaxFilterOperators eOperator)
		{
			string	strSQL = "";
			string	strTerm = "";
			
			//	Use the logical operator to combine the terms
			foreach(CTmaxFilterTerm O in this)
			{
				//	Get the SQL for this term
				strTerm = O.GetSQL();
				if(strTerm.Length == 0) continue;
				
				//	Do we need to add the logical operator?
				if(strSQL.Length > 0)
					strSQL += (" " + eOperator.ToString() + " \r\n");
					
				strSQL += strTerm;
			
			}// foreach(CTmaxFilterTerm O in this)
			
			return strSQL;
			
		}// public string GetSQL(TmaxFilterOperators eOperator)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The active collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
			set { SetCaseCodes(value); }
		}
		
		/// <summary>The active collection of pick lists</summary>
		public CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
			set { SetPickLists(value); }
		}
		
		#endregion Properties
		
	}//	public class CTmaxFilterTerms : CTmaxSortedArrayList
		
	/// <summary>This class is used to implement a database filter</summary>
	public class CTmaxFilter
	{
		#region Constants
		
		private const string XMLINI_OPERATOR_KEY				 = "Operator";
		private const string XMLINI_IF_TREATED_KEY				 = "IfTreated";
		private const string XMLINI_ALL_TEXT_FIELDS_KEY			 = "AllTextFields";
				
		private const string XMLINI_TERM_NAME_ATTRIBUTE			 = "name";
		private const string XMLINI_TERM_CASE_CODE_ID_ATTRIBUTE  = "caseCodeId";
		private const string XMLINI_TERM_VALUE_ATTRIBUTE		 = "value";
		private const string XMLINI_TERM_MODIFIER_ATTRIBUTE		 = "modifier";
		private const string XMLINI_TERM_COMPARISON_ATTRIBUTE	 = "comparison";
		private const string XMLINI_TERM_MULTI_LEVEL_ID_ATTRIBUTE = "multiLevelId";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Terms property</summary>
		private CTmaxFilterTerms m_tmaxTerms = new CTmaxFilterTerms();
		
		/// <summary>Local member bound to NameTerm property</summary>
		private CTmaxFilterTerm m_tmaxNameTerm = new CTmaxFilterTerm();
		
		/// <summary>Local member bound to Operator property</summary>
		private TmaxFilterOperators m_eOperator = TmaxFilterOperators.AND;
		
		/// <summary>Local member bound to IfTreated property</summary>
		private bool m_bIfTreated = false;
		
		/// <summary>Local member bound to Advanced property</summary>
		private bool m_bAdvanced = true;
		
		/// <summary>Local member bound to AllTextFields property</summary>
		private bool m_bAllTextFields = false;
		
		/// <summary>Local member bound to FastCodeId property</summary>
		private long m_lFastCodeId = 0;
		
		/// <summary>Local member bound to FastPickItemId property</summary>
		private long m_lFastPickListId = 0;
		
		/// <summary>Local member bound to FastText property</summary>
		private string m_strFastText = "";
		
		/// <summary>Local member bound to FastFlags property</summary>
		private long m_lFastFlags = 0;
		
		/// <summary>Local member bound to FastFields property</summary>
		private TmaxFastFilterFields m_eFastFields = TmaxFastFilterFields.All;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Constructor</summary>
		public CTmaxFilter()
		{
			//	Initialize the Name term
			m_tmaxNameTerm.Name = "Name";
			m_tmaxNameTerm.Comparison = TmaxFilterComparisons.Contains;
			m_tmaxNameTerm.Modifier = TmaxFilterModifiers.None;
			m_tmaxNameTerm.Value = "";
			
			//	Attach the terms collection events
			m_tmaxEventSource.Attach(m_tmaxTerms.EventSource);
			
		}// public CTmaxFilter()

		/// <summary>This method will copy the properties and terms of the specified source filter</summary>
		public void Copy(CTmaxFilter tmaxSource)
		{
			this.Name = tmaxSource.Name;
			this.Advanced = tmaxSource.Advanced;
			this.FastCodeId = tmaxSource.FastCodeId;
			this.FastPickListId = tmaxSource.FastPickListId;
			this.FastText = tmaxSource.FastText;
			this.FastFlags = tmaxSource.FastFlags;	
			this.FastFields = tmaxSource.FastFields;	
			m_eOperator = tmaxSource.Operator;
			m_bIfTreated = tmaxSource.IfTreated;
			m_bAllTextFields = tmaxSource.AllTextFields;
			m_tmaxNameTerm.Copy(tmaxSource.NameTerm);
			m_tmaxTerms.Copy(tmaxSource.Terms);
			
		}// public voidd Copy(CTmaxFilter tmaxSource)
		
		/// <summary>This method is called to get the name of the section in the XML configuration file for this filter</summary>
		/// <returns>The name of the section containing the configuration information for this filter</returns>
		public string GetXmlSectionName()
		{
			if(m_strName.Length > 0)
				return ("trialMax/station/filter/" + m_strName);
			else
				return "";
		
		}// public string GetXmlSectionName()
		
		/// <summary>This method will clear the terms and reset the class members</summary>
		public void Clear()
		{
			m_tmaxTerms.Clear();
			m_tmaxNameTerm.Value = "";
			m_bIfTreated = false;
		}
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			int				iLine = 1;
			CTmaxFilterTerm	tmaxTerm = null;
			bool			bContinue = true;
			
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;
			
			//	Read the filter properties from file
			m_eOperator = (TmaxFilterOperators)(xmlIni.ReadInteger(XMLINI_OPERATOR_KEY, 0));
			m_bIfTreated = xmlIni.ReadBool(XMLINI_IF_TREATED_KEY, m_bIfTreated);
			m_bAllTextFields = xmlIni.ReadBool(XMLINI_ALL_TEXT_FIELDS_KEY, m_bAllTextFields);
			
			//	The first term should be the constant NameTerm
			Load(xmlIni, GetIniKey(iLine++), m_tmaxNameTerm);

			//	Load all the coded terms
			m_tmaxTerms.Clear();
			while(bContinue == true)
			{
				tmaxTerm = new CTmaxFilterTerm();
				
				if((bContinue = Load(xmlIni, GetIniKey(iLine++), tmaxTerm)) == true)
					m_tmaxTerms.Add(tmaxTerm);
			}
		
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			int	iLine = 1;
			
			if(xmlIni.SetSection(GetXmlSectionName(), true, true) == false) return;
			
			//	Write the filter properties to file
			xmlIni.Write(XMLINI_OPERATOR_KEY, (int)(this.Operator));
			xmlIni.Write(XMLINI_IF_TREATED_KEY, this.IfTreated);
			xmlIni.Write(XMLINI_ALL_TEXT_FIELDS_KEY, this.AllTextFields);
			
			//	Write the static terms to file
			Save(xmlIni, GetIniKey(iLine++), m_tmaxNameTerm);
			
			//	Now write those terms in the collection
			if(m_tmaxTerms != null)
			{
				foreach(CTmaxFilterTerm O in m_tmaxTerms)
				{
					Save(xmlIni, GetIniKey(iLine++), O);
				}
				
			}
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to set the parameters for fast filtering</summary>
		/// <param name="tmaxCode">The case code used to construct the filter</param>
		/// <param name="tmaxPickList">The pick list used to construct the filter</param>
		/// <param name="strText">The text used to build the query</param>
		/// <param name="lFlags">The flags used to build the query</param>
		public void SetFastFilter(CTmaxCaseCode tmaxCode, CTmaxPickItem tmaxPickList, string strText, long lFlags)
		{
			if(tmaxCode != null)
			{
				if(tmaxPickList != null)
					SetFastFilter(tmaxCode.UniqueId, tmaxPickList.UniqueId, strText, lFlags);
				else
					SetFastFilter(tmaxCode.UniqueId, 0, strText, lFlags);
			}
			else
			{
				if(tmaxPickList != null)
					SetFastFilter(0, tmaxPickList.UniqueId, strText, lFlags);
				else
					SetFastFilter(0, 0, strText, lFlags);
			
			}// if(tmaxCode != null)
		
		}// public void SetFilter(CTmaxCaseCode tmaxCode, string strText, long lFlags)
		
		/// <summary>This method is called to set the parameters for fast filtering</summary>
		/// <param name="lCaseCodeId">The id of the case code used to construct the filter</param>
		/// <param name="lPickListId">The id of the pick list used to construct the filter</param>
		/// <param name="strText">The text used to build the query</param>
		/// <param name="lFlags">The flags used to build the query</param>
		public void SetFastFilter(long lCaseCodeId, long lPickListId, string strText, long lFlags)
		{
			this.FastCodeId = lCaseCodeId;
			this.FastPickListId = lPickListId;
			this.FastText = strText;
			this.FastFlags = lFlags;
			this.Advanced = false;
		}
		
		/// <summary>This method allows the caller to add a new term to the filter</summary>
		/// <param name="tmaxTerm">The term to be added to the filter</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxFilterTerm Add(CTmaxFilterTerm tmaxTerm)
		{
			Debug.Assert(m_tmaxTerms != null);
			
			try
			{
				if(m_tmaxTerms.Add(tmaxTerm) != null)
					return tmaxTerm;
				else
					return null;
			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", "Exception");
				return null;
			}
			
		}// public CTmaxFilterTerm Add(CTmaxFilterTerm tmaxTerm)

		/// <summary>This method is called to remove a term from the list</summary>
		/// <param name="tmaxTerm">The filter term to be removed</param>
		public void Remove(CTmaxFilterTerm tmaxTerm)
		{
			Debug.Assert(m_tmaxTerms != null);
			
			try
			{
				m_tmaxTerms.Remove(tmaxTerm);
			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "Remove", "Exception");
			}
		
		}// public void Remove(CTmaxFilterTerm tmaxTerm)

		/// <summary>This method is called to get the display text for the specified comparison enumeration</summary>
		/// <param name="e">The enumerated comparison identifier</param>
		/// <returns>true if successful</returns>
		static public string GetDisplayText(TmaxFilterComparisons e)
		{
			string strComparison = "";
			
			//	Which comparison?
			switch(e)
			{
				case TmaxFilterComparisons.LessThan:
				
					strComparison = "Less Than";
					break;
				
				case TmaxFilterComparisons.LessThanEquals:
				
					strComparison = "Less Than or Equal";
					break;
				
				case TmaxFilterComparisons.GreaterThan:
				
					strComparison = "Greater Than";
					break;
				
				case TmaxFilterComparisons.GreaterThanEquals:
				
					strComparison = "Greater Than or Equal";
					break;
				
				case TmaxFilterComparisons.StartsWith:
				
					strComparison = "Starts With";
					break;
				
				case TmaxFilterComparisons.EndsWith:
				
					strComparison = "Ends With";
					break;
				
				case TmaxFilterComparisons.AnyValue:
				
					strComparison = "Any Value";
					break;
				
				case TmaxFilterComparisons.Contains:
				case TmaxFilterComparisons.Equals:
				default:
				
					strComparison = e.ToString();
					break;
				
			}// switch(e)
			
			return strComparison;
			
		}// static public string GetDisplayText(TmaxFilterComparisons e)
		
		/// <summary>This method gets the collection of codes assigned to this filter</summary>
		/// <returns>The active codes collection</returns>
		public CTmaxCaseCodes GetCaseCodes()
		{
			if(m_tmaxTerms != null)
				return m_tmaxTerms.CaseCodes;
			else
				return null;
				
		}// public CTmaxCaseCodes GetCaseCodes()
		
		/// <summary>This method sets the collection of codes assigned to this filter</summary>
		/// <param name="tmaxCodes">The collection of case codes</param>
		public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		{
			if(m_tmaxTerms != null)
				m_tmaxTerms.CaseCodes = tmaxCodes;
				
		}// public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		
		/// <summary>This method gets the collection of pick lists assigned to this filter</summary>
		/// <returns>The active codes collection</returns>
		public CTmaxPickItem GetPickLists()
		{
			if(m_tmaxTerms != null)
				return m_tmaxTerms.PickLists;
			else
				return null;
				
		}// public CTmaxPickItem GetPickLists()
		
		/// <summary>This method sets the collection of codes assigned to this filter</summary>
		/// <param name="tmaxCodes">The collection of case codes</param>
		public void SetPickLists(CTmaxPickItem tmaxPickLists)
		{
			if(m_tmaxTerms != null)
				m_tmaxTerms.PickLists = tmaxPickLists;
				
		}// public void SetPickLists(CTmaxPickItem tmaxPickLists)
		
		/// <summary>This method is called to get the SQL statement for the filter</summary>
		/// <param name="bAdvanced">True to apply the advanced options</param>
		/// <returns>The SQL statement for this filter</returns>
		public string GetSQL(bool bAdvanced)
		{
			if(bAdvanced == true)
				return GetAdvancedSQL();
			else
				return GetFastSQL();

		}// public string GetSQL(bool bAdvanced)
		
		/// <summary>This method is called to get the SQL statement for the filter</summary>
		/// <returns>The SQL statement for this filter</returns>
		public string GetSQL()
		{
			return GetSQL(this.Advanced);
		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method converts the numeric line identifier to a unique key for storing the term descriptor</summary>
		/// <param name="iTerm">unique numeric identifier for the term</param>
		/// <returns>The appropriate XML Ini key</returns>
		string GetIniKey(int iTerm)
		{
			return String.Format("T{0}", iTerm);
		}
		
		/// <summary>This method is called to store the specified filter term in the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxTerm">The term to be stored in the file</param>
		public bool Save(CXmlIni xmlIni, string strKey, CTmaxFilterTerm tmaxTerm)
		{
			try
			{
				xmlIni.Write(strKey, XMLINI_TERM_NAME_ATTRIBUTE, tmaxTerm.Name);
				xmlIni.Write(strKey, XMLINI_TERM_CASE_CODE_ID_ATTRIBUTE, tmaxTerm.CaseCodeId);
				xmlIni.Write(strKey, XMLINI_TERM_MULTI_LEVEL_ID_ATTRIBUTE, tmaxTerm.MultiLevelSelectionId);
				xmlIni.Write(strKey, XMLINI_TERM_MODIFIER_ATTRIBUTE, (int)(tmaxTerm.Modifier));
				xmlIni.Write(strKey, XMLINI_TERM_COMPARISON_ATTRIBUTE, (int)(tmaxTerm.Comparison));
				xmlIni.Write(strKey, XMLINI_TERM_VALUE_ATTRIBUTE, tmaxTerm.Value);
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public bool Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to load the information for the specified filter term from the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxTerm">The term to be initialized</param>
		///	<returns>true to continue loading</returns>
		public bool Load(CXmlIni xmlIni, string strKey, CTmaxFilterTerm tmaxTerm)
		{
			try
			{
				//	Read the name of this term if not already defined
				if(tmaxTerm.Name.Length == 0)
					tmaxTerm.Name = xmlIni.Read(strKey, XMLINI_TERM_NAME_ATTRIBUTE, "");
				
				//	Have we run out of terms?
				if(tmaxTerm.Name.Length == 0) return false;
				
				//	Read the rest of the property values
				tmaxTerm.CaseCodeId = xmlIni.ReadLong(strKey, XMLINI_TERM_CASE_CODE_ID_ATTRIBUTE, 0);
				tmaxTerm.MultiLevelSelectionId = xmlIni.ReadLong(strKey, XMLINI_TERM_MULTI_LEVEL_ID_ATTRIBUTE, 0);
				
				try { tmaxTerm.Modifier = (TmaxFilterModifiers)(xmlIni.ReadInteger(strKey, XMLINI_TERM_MODIFIER_ATTRIBUTE, 0)); }
				catch {}
				
				try { tmaxTerm.Comparison = (TmaxFilterComparisons)(xmlIni.ReadInteger(strKey, XMLINI_TERM_COMPARISON_ATTRIBUTE, 0)); }
				catch {}
				
				tmaxTerm.Value = xmlIni.Read(strKey, XMLINI_TERM_VALUE_ATTRIBUTE, "");

				return true;
			}
			catch
			{
				//	NOTE: We don't return FALSE because the rest of the terms might be OK
			}
			
			return true;
			
		}// public bool Save(CXmlIni xmlIni)
		
		/// <summary>This method applies the advanced options to contruct the filter's SQL statement</summary>
		/// <returns>The SQL statement for this filter's advanced options</returns>
		private string GetAdvancedSQL()
		{
			string	strSQL = "";
			string	strPreFilter = "";
			string	strField = "";
			
			//	Are we pre-filtering primary media?
			if((m_tmaxNameTerm != null) && (m_tmaxNameTerm.Value.Length > 0))
			{
				strPreFilter = String.Format("(Name LIKE '%{0}%') OR (MediaId LIKE '%{0}%')", CTmaxToolbox.SQLEncode(m_tmaxNameTerm.Value));
				
				//	Are we searching all text fields also?
				if((this.AllTextFields == true) && (this.CaseCodes != null))
				{
					foreach(CTmaxCaseCode O in this.CaseCodes)
					{
						strField = O.GetSQL(m_tmaxNameTerm.Value);
						if(strField.Length > 0)
						{
							strPreFilter += " OR ";
							strPreFilter += strField;
						}
					
					}// foreach(CTmaxCaseCode O in this.CaseCodes)
					
				}// if((this.AllTextFields == true) && (this.CaseCodes != null))
				
				strSQL = String.Format("SELECT DISTINCT AutoId FROM PrimaryMedia WHERE \r\n({0})", strPreFilter);
			}
			
			//	Do we have any terms in the collection?
			if((m_tmaxTerms != null) && (m_tmaxTerms.Count > 0))
			{
				//	Are we combining this with MediaID / Name?
				if(strSQL.Length > 0)
					strSQL += ("\r\nAND (" + m_tmaxTerms.GetSQL(this.Operator) + ")");
				else
					strSQL = String.Format("SELECT DISTINCT AutoId FROM PrimaryMedia WHERE \r\n({0})", m_tmaxTerms.GetSQL(this.Operator));
			}
			
			//	Do we only want treated documents?
			if(this.IfTreated == true)
			{
				//	Are we combining this with other criteria?
				if(strSQL.Length > 0)
				{
					strSQL += ("\r\nAND (AutoID in (SELECT PrimaryMediaID FROM SecondaryMedia WHERE ((MediaType = 5) AND (Children > 0))))");
				}
				else
				{
					//	NOTE:	BEWARE - THIS STATEMENT MUST BE MODIFIED IF WE ADD ANOTHER
					//			CRITERIA THAT WE MAY WANT TO TACK ONTO THIS ONE. THIS STATEMENT
					//			PULLS THE PRIMARY MEDIA IDENTIFIERS FROM THE SECONDARY TABLE
					strSQL = "SELECT DISTINCT PrimaryMediaID FROM SecondaryMedia WHERE ((MediaType = 5) AND (Children > 0))";
				}
				
			}// if(this.IfTreated == true)
			
			return strSQL;
			
		}// private string GetAdvancedSQL()
		
		/// <summary>This method applies the fast filtering options to contruct the required SQL statement</summary>
		/// <returns>The SQL statement for this object's fast filtering options</returns>
		private string GetFastSQL()
		{
			string			strSQL = "";
			string			strSearch = "";
			CTmaxCaseCode	tmaxCaseCode = null;
			CTmaxPickItem	tmaxPickValue = null;
			CTmaxPickItem	tmaxPickList = null;
			bool			bExclude = false;
			
			//	Must have text to search for
			if(this.FastText.Length > 0)
				strSearch = this.FastText;
			else
				return "";
				
			//	Get the case code if defined
			if((this.FastCodeId != 0) && (this.CaseCodes != null))
				tmaxCaseCode = this.CaseCodes.Find(this.FastCodeId);
			else if((this.FastFields == TmaxFastFilterFields.Descriptions) && (this.CaseCodes != null))
				tmaxCaseCode = this.CaseCodes.Find(TmaxCodedProperties.Description);
			
			//	Has the exclusion flag been set?
			bExclude = ((this.FastFlags & (long)(TmaxSetFilterFlags.Exclude)) != 0);
			
			//	Are we using a case code?
			if(tmaxCaseCode != null)
			{
				//	What type of code is this?
				switch(tmaxCaseCode.Type)
				{
					case TmaxCodeTypes.Boolean:
					
						if(CTmaxToolbox.StringToBool(strSearch) == true)
							strSearch = "TRUE";
						else
							strSearch = "FALSE";

						if(bExclude == true)
							strSQL  = "SELECT DISTINCT AutoId FROM PrimaryMedia WHERE AutoId NOT IN (";

						strSQL += "SELECT DISTINCT PrimaryId FROM Codes WHERE ";
						strSQL += String.Format("(CaseCodeId = {0} AND ", tmaxCaseCode.UniqueId);
						strSQL += String.Format("(valueBoolean = {0}))", strSearch);
						
						if(bExclude == true)
							strSQL += ");";
							
						break;
						
					case TmaxCodeTypes.PickList:
					
						//	Which pick list should we use?
						if(FastPickListId > 0)
						{
							Debug.Assert(this.PickLists != null);
							if(this.PickLists != null)
								tmaxPickList = this.PickLists.FindList(FastPickListId);
						}
						else
						{
							//	Use the pick list bound to the case code
							tmaxPickList = tmaxCaseCode.PickList;
						}
						
						//	Get the search value
						if(tmaxPickList != null)
							tmaxPickValue = tmaxPickList.FindChild(strSearch);
						if(tmaxPickValue == null) break; // Can't find the value
					
						if(bExclude == true)
							strSQL  = "SELECT DISTINCT AutoId FROM PrimaryMedia WHERE AutoId NOT IN (";

						strSQL += "SELECT DISTINCT PrimaryId FROM Codes WHERE ";
						strSQL += String.Format("(CaseCodeId = {0} AND ", tmaxCaseCode.UniqueId);
						strSQL += String.Format("(valuePickList = {0}))", tmaxPickValue.UniqueId.ToString());
						
						if(bExclude == true)
							strSQL += ");";
							
						break;
						
					case TmaxCodeTypes.Text:
					case TmaxCodeTypes.Memo:
					
						if(bExclude == true)
							strSQL  = "SELECT DISTINCT AutoId FROM PrimaryMedia WHERE AutoId NOT IN (";

						strSQL += "SELECT DISTINCT PrimaryId FROM Codes WHERE ";
						strSQL += String.Format("(CaseCodeId = {0} AND ", tmaxCaseCode.UniqueId);
						strSQL += String.Format("((valueText LIKE '%{0}%') OR (valueMemo LIKE '%{0}%')))", CTmaxToolbox.SQLEncode(strSearch));
						
						if(bExclude == true)
							strSQL += ");";
							
						break;
						
					default:
					
						break;
						
				}// switch(tmaxCaseCode.Type)
					
			}
			else
			{
				//	Simple text searching of all fields
				if(bExclude == true)
				{
					strSQL  = "SELECT DISTINCT AutoId FROM PrimaryMedia WHERE ";
					strSQL += String.Format("((Name NOT LIKE '%{0}%') OR (MediaId NOT LIKE '%{0}%')) ", CTmaxToolbox.SQLEncode(strSearch));
					
					//	Should fielded data be included in the operation?
					if(this.FastFields != TmaxFastFilterFields.Names)
					{
						strSQL += "OR (AutoId NOT IN (SELECT PrimaryId FROM Codes WHERE ";
						strSQL += String.Format("((valueText LIKE '%{0}%') OR (valueMemo LIKE '%{0}%'))));", CTmaxToolbox.SQLEncode(strSearch));
					}
				}
				else
				{
					strSQL  = "SELECT DISTINCT AutoId FROM PrimaryMedia WHERE ";
					strSQL += String.Format("((Name LIKE '%{0}%') OR (MediaId LIKE '%{0}%')) ", CTmaxToolbox.SQLEncode(strSearch));
					
					//	Should fielded data be included in the operation?
					if(this.FastFields != TmaxFastFilterFields.Names)
					{
						strSQL += "OR (AutoId IN (SELECT PrimaryId FROM Codes WHERE ";
						strSQL += String.Format("((valueText LIKE '%{0}%') OR (valueMemo LIKE '%{0}%'))));", CTmaxToolbox.SQLEncode(strSearch));
					}
					
				}
				
			}// if(tmaxCaseCode != null)

			m_tmaxEventSource.FireDiagnostic(this, "GetFastSQL", "FAST Filter: " + strSQL);
			return strSQL;
			
		}// private string GetFastSQL()
		
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
		
		/// <summary>True if this filter is set up for advanced filtering</summary>
		public bool Advanced
		{
			get { return m_bAdvanced; }
			set { m_bAdvanced = value; }
		}
		
		/// <summary>The collection of terms used to construct the filter</summary>
		public CTmaxFilterTerms Terms
		{
			get { return m_tmaxTerms; }
		}
		
		/// <summary>The Name term used to filter the PrimaryMedia table</summary>
		public CTmaxFilterTerm NameTerm
		{
			get { return m_tmaxNameTerm; }
		}
		
		/// <summary>The Operator used to combine the terms in the filter</summary>
		public TmaxFilterOperators Operator
		{
			get { return m_eOperator; }
			set { m_eOperator = value; }
		}
		
		/// <summary>The active collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return GetCaseCodes(); }
			set { SetCaseCodes(value); }
		}
		
		/// <summary>The active collection of pick lists</summary>
		public CTmaxPickItem PickLists
		{
			get { return GetPickLists(); }
			set { SetPickLists(value); }
		}
		
		/// <summary>True to include only documents that have been treated</summary>
		public bool IfTreated
		{
			get { return m_bIfTreated; }
			set { m_bIfTreated = value; }
		}
		
		/// <summary>True to include all text fields in Primary Name/MediaID search</summary>
		public bool AllTextFields
		{
			get { return m_bAllTextFields; }
			set { m_bAllTextFields = value; }
		}
		
		/// <summary>Identifier of case code used to generate fast filter SQL</summary>
		public long FastCodeId
		{
			get { return m_lFastCodeId; }
			set { m_lFastCodeId = value; }
		}
		
		/// <summary>Identifier of pick list used to generate fast filter SQL</summary>
		public long FastPickListId
		{
			get { return m_lFastPickListId; }
			set { m_lFastPickListId = value; }
		}
		
		/// <summary>Flags used to generate fast filter SQL</summary>
		public long FastFlags
		{
			get { return m_lFastFlags; }
			set { m_lFastFlags = value; }
		}
		
		/// <summary>Fields used for the fast filter if no code specified</summary>
		public TmaxFastFilterFields FastFields
		{
			get { return m_eFastFields; }
			set { m_eFastFields = value; }
		}
		
		/// <summary>Text used to generate fast filter SQL</summary>
		public string FastText
		{
			get { return m_strFastText; }
			set { m_strFastText = value; }
		}
		
		#endregion Properties
		
	}//	public class CTmaxFilter
		
}// namespace FTI.Shared.Trialmax
