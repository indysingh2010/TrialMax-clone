using System;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Shared.Text;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the information associated with an XML deposition link</summary>
	public class CTmaxCaseCode : ITmaxSortable, ITmaxListViewCtrl
	{
		#region Constants

		public const string INVALID_CODE_NAME_CHARACTERS = "/\\\"[]'";

		#endregion Constants
		#region Private Members

		/// <summary>This member is bounded to the DxRecord property</summary>
		private object m_dxRecord = null;
		
		/// <summary>This member is bounded to the Id property</summary>
		private long m_lUniqueId = 0;
		
		/// <summary>This member is bounded to the PickListId property</summary>
		private long m_lPickListId = 0;
		
		/// <summary>This member is bounded to the PickList property</summary>
		private CTmaxPickItem m_tmaxPickList = null;
		
		/// <summary>This member is bounded to the Name property</summary>
		private string m_strName = "";		
		
		/// <summary>This member is bounded to the Type property</summary>
		private TmaxCodeTypes m_eType = TmaxCodeTypes.Unknown;		
		
		/// <summary>This member is bounded to the CodedProperty property</summary>
		private TmaxCodedProperties m_eCodedProperty = TmaxCodedProperties.Invalid;		
		
		/// <summary>Private member bound to Attributes property</summary>
		private long m_lAttributes = 0;
		
		/// <summary>Private member bound to SortOrder property</summary>
		private long m_lSortOrder = 0;
		
		/// <summary>Private member bound to Imported property</summary>
		private ArrayList m_aImported = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxCaseCode() : base()
		{
			//	Set default attributes
			this.AllowMultiple = true;
			this.Hidden = false;
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxCaseCode">Source object to be copied</param>
		public CTmaxCaseCode(CTmaxCaseCode tmaxCaseCode) : base()
		{
			Debug.Assert(tmaxCaseCode != null);
			
			if(tmaxCaseCode != null)
			{
				Copy(tmaxCaseCode);
			}
			else
			{
				//	Set default attributes
				this.AllowMultiple = true;
				this.Hidden = false;
			}
			
		}// public CTmaxCaseCode(CTmaxCaseCode tmaxCaseCode) : base()
		
		/// <summary>Constructor</summary>
		/// <param name="eProperty">Enumerated coded property to bind to this code</param>
		public CTmaxCaseCode(TmaxCodedProperties eProperty) : base()
		{
			if(eProperty != TmaxCodedProperties.Invalid)
			{
				this.UniqueId		= CTmaxCaseCodes.GetCodeId(eProperty);
				this.Name			= CTmaxCaseCodes.GetCodeName(eProperty);
				this.Type			= CTmaxCaseCodes.GetCodeType(eProperty);
				this.CodedProperty	= eProperty;
				this.AllowMultiple  = false;
				this.PickListId		= 0;
			}
			
		}// public CTmaxCaseCode(TmaxCodedProperties eProperty) : base()
		
		/// <summary>Called to copy the properties of the source node</summary>
		/// <param name="tmaxCaseCode">The primary object to be copied</param>
		public void Copy(CTmaxCaseCode tmaxCaseCode)
		{
			this.UniqueId		= tmaxCaseCode.UniqueId;
			this.Name			= tmaxCaseCode.Name;
			this.Type			= tmaxCaseCode.Type;
			this.SortOrder		= tmaxCaseCode.SortOrder;
			this.CodedProperty	= tmaxCaseCode.CodedProperty;
			this.PickListId		= tmaxCaseCode.PickListId;
			this.PickList		= tmaxCaseCode.PickList;
			this.DxRecord		= tmaxCaseCode.DxRecord;
			this.Attributes		= tmaxCaseCode.Attributes; // Copies all flags (Hidden, AllowMultiple, ...)
			
		}// public void Copy(CTmaxCaseCode tmaxCaseCode)
		
		/// <summary>Uses the specified string to set the object's data type</summary>
		/// <param name="strType">The string equivalent of an enumerated code data type</param>
		/// <returns>true if successful</returns>
		public bool SetType(string strType)
		{
			m_eType = StringToType(strType);
			return (m_eType != TmaxCodeTypes.Unknown);
		
		}// public bool SetType(string strType)
		
		/// <summary>Sets the pick list of values allowed for this code</summary>
		/// <param name="tmaxPickList">The collection of allowable values</param>
		/// <returns>true if successful</returns>
		public bool SetPickList(CTmaxPickItem tmaxPickList)
		{
			bool bSuccessful = false;
			
			//	Is this a pick list code?
			if(this.Type == TmaxCodeTypes.PickList)
			{
				//	Is the user setting or clearing the pick list?
				if(tmaxPickList != null)
				{
					//	This pick list item should be a value list
					switch(tmaxPickList.Type)
					{
						case TmaxPickItemTypes.Unknown:
						case TmaxPickItemTypes.Value:
						
							break;
							
						case TmaxPickItemTypes.StringList:
						case TmaxPickItemTypes.MultiLevel:
						default:
						
							m_tmaxPickList = tmaxPickList;
							m_lPickListId = m_tmaxPickList.UniqueId;
							bSuccessful = true;
							break;
							
					}// switch(tmaxPickList.Type)
				
				}
				else
				{
					m_tmaxPickList = null;
				
				}// if(tmaxPickList != null)
				
			}// if(this.Type == TmaxCodeTypes.PickList)
			
			return bSuccessful;
		
		}// public bool SetPickList(CTmaxPickItem tmaxPickList)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxCaseCode">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxCaseCode, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxCaseCode tmaxCaseCode, long lMode)
		{
			//	Are these the same objects?
			if(ReferenceEquals(this, tmaxCaseCode) == true)
			{
				return 0;
			}
			else
			{
				//	Are we sorting on order?
				if(lMode == CTmaxCaseCodes.SORT_ON_ORDER)
				{
					if(this.SortOrder == tmaxCaseCode.SortOrder)
						return 0;
					else if(this.SortOrder > tmaxCaseCode.SortOrder)
						return 1;
					else
						return -1;
				}
				else
				{						
					return CTmaxToolbox.Compare(this.Name, tmaxCaseCode.Name, true);
				}
			
			}// if(ReferenceEquals(this, tmaxCaseCode) == true)
					
		}// public int Compare(CTmaxCaseCode tmaxCaseCode, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxCaseCode, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxCaseCode)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			string[] aNames = { "Name", "Type" };
			return aNames;
		
		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[2];
			aValues[0] = m_strName;
			aValues[1] = m_eType.ToString();
			
			return aValues;
			
		}// string[] ITmaxListViewCtrl.GetValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			//	No images used for this type
			return -1;
		}
		
		/// <summary>This method gets the name of the column in the database table that is bound to this code type</summary>
		/// <returns>The database column bound to this code type</returns>
		public string GetDbColumn()
		{
			return CTmaxCaseCode.GetDbColumn(m_eType);
		}
		
		/// <summary>This method gets the default value for a code of this type</summary>
		/// <returns>The default value</returns>
		public string GetDefault()
		{
			string strValue = "";
			
			//	What type of code is this?
			switch(m_eType)
			{
				case TmaxCodeTypes.Boolean:
				
					strValue = "false";
					break;
					
				case TmaxCodeTypes.Integer:
				
					strValue = "0";
					break;
					
				case TmaxCodeTypes.Decimal:
				
					strValue = "0.0";
					break;
					
				case TmaxCodeTypes.PickList:
				
					strValue = "0";
					if(this.PickList != null)
					{
						if(this.PickList.Children != null)
						{
							if(this.PickList.Children.Count > 0)
								strValue = this.PickList.Children[0].UniqueId.ToString();
						}
						
					}
					break;
					
				default:
				
					break;
					
			}// switch(m_eType)
			
			return strValue;
					
		}// public string GetDefault()
		
		/// <summary>This method is called to determine if the specified string represents a valid value</summary>
		/// <param name="strValue">The value as a string</param>
		/// <returns>true if valid</returns>
		public bool IsValid(string strValue)
		{
			return IsValid(this.Type, strValue, this.PickList);
		}
		
		/// <summary>This method converts the string value to an object of the type appropriate for the specified code type</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsValid">Flag to indicate if the value string is valid</param>
		/// <returns>the converted value</returns>
		public object ToObject(string strValue, ref bool bIsValid)
		{
			return ToObject(m_eType, strValue, ref bIsValid, this.PickList);
		}
		
		/// <summary>This method converts the string value to an object of the type appropriate for this code</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <returns>the converted value</returns>
		public object ToObject(string strValue)
		{
			bool bIsValid = false;
			
			return ToObject(m_eType, strValue, ref bIsValid, this.PickList);
		}
		
		/// <summary>This method is called to determine if the specified string represents a valid value for the code</summary>
		/// <param name="eType">The enumerated code type identifier</param>
		/// <param name="strValue">The value as a string</param>
		/// <param name="tmaxPickList">The pick list of valid values</param>
		/// <returns>true if valid</returns>
		static public bool IsValid(TmaxCodeTypes eType, string strValue, CTmaxPickItem tmaxPickList)
		{
			bool bIsValid = true;
			
			//	Is this a null value?
			if(IsNull(strValue) == true) return false;
			
			//	What is the data type for this code
			switch(eType)
			{
				case TmaxCodeTypes.Boolean:
					
					ToBool(strValue, ref bIsValid);
					break;
						
				case TmaxCodeTypes.Decimal:
					
					ToDecimal(strValue, ref bIsValid);
					break;
						
				case TmaxCodeTypes.Integer:
					
					ToInteger(strValue, ref bIsValid);
					break;
						
				case TmaxCodeTypes.Date:
					
					ToDate(strValue, ref bIsValid);
					break;
						
				case TmaxCodeTypes.PickList:
				
					//	First convert to the numeric identifier
					int iId = ToInteger(strValue, ref bIsValid);
					
					//	Now verify that the object is in the list
					if(bIsValid == true)
					{
						//	If no pick list specified assume it is valid
						if(tmaxPickList != null)
						{
							if(tmaxPickList.Children != null)
								bIsValid = (tmaxPickList.Children.Find(iId) != null);
							else
								bIsValid = false;
						}
					
					}// if(bValid == true)
					break;
					
				case TmaxCodeTypes.Text:
				case TmaxCodeTypes.Memo:
				case TmaxCodeTypes.Unknown:
				default:
					
					return true; //	Assume OK if we don't know specific type
			
			}// switch(eType)

			return bIsValid;
			
		}// static public bool IsValid(string strValue)
		
		/// <summary>This method is called to determine if the specified string represents a valid value for the code</summary>
		/// <param name="eType">The enumerated code type identifier</param>
		/// <param name="strValue">The value as a string</param>
		/// <returns>true if valid</returns>
		static public bool IsValid(TmaxCodeTypes eType, string strValue)
		{
			return IsValid(eType, strValue, null);
		}

		/// <summary>Called to determine if the specified name is valid</summary>
		/// <param name="strName">The name to be checked</param>
		/// <param name="bWarnUser">true if user should be warned if not valid</param>
		/// <returns>true if valid</returns>
		static public bool IsValidName( string strName, bool bWarnUser)
		{
			bool	bValid = false;
			string	strMsg = "";
			
			if((strName != null) && (strName.Length > 0))
			{
				if(strName.IndexOfAny(INVALID_CODE_NAME_CHARACTERS.ToCharArray()) >= 0)
				{
					//	Should we warn the user?
					if(bWarnUser == true)
					{
						strMsg = String.Format("{0} contains one or more invalid characters. Code names can not include any of the these characters: {1}", strName, INVALID_CODE_NAME_CHARACTERS);
						MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}

				}
				else
				{
					bValid = true;
				}

			}// if(strName.IndexOfAny(INVALID_CODE_NAME_CHARACTERS.ToCharArray()) >= 0)
			
			return bValid;

		}// static public bool IsValidName( string strName)

		/// <summary>This method is called to determine if the specified value represents a null tag</summary>
		/// <param name="strValue">The text to check</param>
		/// <returns>true if null</returns>
		static public bool IsNull(string strValue)
		{
			if((strValue == null) || (strValue.Length == 0))
				return true;
			else
				return false;
				
		}// static public bool IsNull(string strValue)
		
		/// <summary>This method converts the string value to an integer</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsValid">true if the value is a valid integer</param>
		/// <returns>the converted value</returns>
		static public int ToInteger(string strValue, ref bool bIsValid)
		{
			try
			{
				int iValue = System.Convert.ToInt32(strValue);
				bIsValid = true;
				return iValue;
			}
			catch
			{
				bIsValid = false;
				return 0;
			}
		
		}// static public int ToInteger(string strValue, ref bool bIsValid)
		
		/// <summary>This method converts the string value to an integer</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <returns>the converted value</returns>
		static public int ToInteger(string strValue)
		{
			bool bIsValid = true;
			return ToInteger(strValue, ref bIsValid);
		}
		
		/// <summary>This method converts the string value to a double precision floating point value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsValid">true if the value is a valid integer</param>
		/// <returns>the converted value</returns>
		static public double ToDecimal(string strValue, ref bool bIsValid)
		{
			try
			{
				double dValue = System.Convert.ToDouble(strValue);
				bIsValid = true;
				return dValue;
			}
			catch
			{
				bIsValid = false;
				return 0;
			}
		
		}// static public double ToDecimal(string strValue, ref bool bIsValid)
		
		/// <summary>This method converts the string value to a double precision floating point value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <returns>the converted value</returns>
		static public double ToDecimal(string strValue)
		{
			bool bIsValid = true;
			return ToDecimal(strValue, ref bIsValid);
		}
		
		/// <summary>This method converts the string value to a system DateTime value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsValid">true if the value is valid</param>
		/// <returns>the converted value</returns>
		static public DateTime ToDate(string strValue, ref bool bIsValid)
		{
			try
			{
				DateTime dtValue = System.Convert.ToDateTime(strValue);
				bIsValid = true;
				return dtValue;
			}
			catch
			{
				bIsValid = false;
				return System.DateTime.MinValue;
			}
		
		}// static public DateTime ToDate(string strValue, ref bool bIsValid)
		
		/// <summary>This method converts the string value to a DateTime precision floating point value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <returns>the converted value</returns>
		static public DateTime ToDate(string strValue)
		{
			bool bIsValid = true;
			return ToDate(strValue, ref bIsValid);
		}
		
		/// <summary>This method converts the string value to a system boolean value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsValid">true if the value is valid</param>
		/// <returns>the converted value</returns>
		static public bool ToBool(string strValue, ref bool bIsValid)
		{
			return CTmaxToolbox.StringToBool(strValue, ref bIsValid);
		}
		
		/// <summary>This method converts the string value to a DateTime precision floating point value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <returns>the converted value</returns>
		static public bool ToBool(string strValue)
		{
			bool bIsValid = true;
			return ToBool(strValue, ref bIsValid);
		}
		
		/// <summary>This method converts the string value to an object of the type appropriate for the specified code type</summary>
		/// <param name="eType">The enumerated code type identifier</param>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsValid">true if the value is valid</param>
		/// <param name="tmaxPickList">The pick list against which to validate the value</param>
		/// <returns>the converted value</returns>
		static public object ToObject(TmaxCodeTypes eType, string strValue, ref bool bIsValid, CTmaxPickItem tmaxPickList)
		{
			object O = null;
			
			//	What is the data type for this code
			switch(eType)
			{
				case TmaxCodeTypes.Boolean:
					
					bool bValue = ToBool(strValue, ref bIsValid);
					if(bIsValid == true) O = bValue;
					break;
						
				case TmaxCodeTypes.Decimal:
					
					double dValue = ToDecimal(strValue, ref bIsValid);
					if(bIsValid == true) O = dValue;
					break;
						
				case TmaxCodeTypes.Integer:
					
					int iValue = ToInteger(strValue, ref bIsValid);
					if(bIsValid == true) O = iValue;
					break;
						
				case TmaxCodeTypes.PickList:
					
					if((bIsValid = IsValid(eType, strValue, tmaxPickList)) == true)
					{
						O = ToInteger(strValue, ref bIsValid);
					}
					break;
						
				case TmaxCodeTypes.Date:
					
					System.DateTime dtValue = ToDate(strValue, ref bIsValid);
					if(bIsValid == true) O = dtValue;
					break;
						
				case TmaxCodeTypes.Text:
				case TmaxCodeTypes.Memo:
				case TmaxCodeTypes.Unknown:
				default:
					
					//	Treat as string by default
					bIsValid = (IsNull(strValue) == false);
					if(bIsValid == true) O = strValue;
					break;
			
			}// switch(eType)
			
			return O;
			
		}// static public object ToObject(TmaxCodeTypes eType, string strValue, ref bool bIsValid)
		
		/// <summary>This method converts the string value to an object of the type appropriate for the specified code type</summary>
		/// <param name="eType">The enumerated code type identifier</param>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsValid">true if the value is valid</param>
		/// <returns>the converted value</returns>
		static public object ToObject(TmaxCodeTypes eType, string strValue, ref bool bIsValid)
		{
			return ToObject(eType, strValue, ref bIsValid, null);
		}
		
		/// <summary>This method converts the string value to an object of the type appropriate for the specified code type</summary>
		/// <param name="eType">The enumerated code type identifier</param>
		/// <param name="strValue">The value to be converted</param>
		/// <returns>the converted value</returns>
		static public object ToObject(TmaxCodeTypes eType, string strValue)
		{
			bool bIsValid = true;
			return ToObject(eType, strValue, ref bIsValid, null);
		}
		
		/// <summary>This method gets the name of the column in the database table that is associated with the specified code type</summary>
		/// <param name="eType">The case code type identifier</param>
		/// <returns>The database column bound to the specified code type</returns>
		static public string GetDbColumn(TmaxCodeTypes eType)
		{
			switch(eType)
			{
				case TmaxCodeTypes.Integer:
					return "valueInteger";
				case TmaxCodeTypes.Decimal:
					return "valueDecimal";
				case TmaxCodeTypes.Boolean:
					return "valueBoolean";
				case TmaxCodeTypes.Date:
					return "valueDateTime";
				case TmaxCodeTypes.Memo:
					return "valueMemo";
				case TmaxCodeTypes.PickList:
					return "valuePickList";
				case TmaxCodeTypes.Text:
				default:
					return "valueText";
			
			}// switch(eType)
			
		}// static public string GetDbColumn(TmaxCodeTypes eType)
		
		/// <summary>This method is called to get the SQL statement to search this code for the specified text</summary>
		/// <returns>The SQL statement for this term</returns>
		public string GetSQL(string strText)
		{
			string	strSQL = "";
			string	strSelect = "";
			string	strColumn = "";
			
			//	Only works for text-based codes
			if((this.Type != TmaxCodeTypes.Text) &&
			   (this.Type != TmaxCodeTypes.Memo)) return "";
			   
			//	Get the name of the column to query (valueText or valueMemo)
			strColumn = this.GetDbColumn();
			if(strColumn.Length == 0) return "";
			
			//	All statements start with this
			strSelect = String.Format("SELECT PrimaryId FROM Codes WHERE (CaseCodeId = {0} AND {1} LIKE '%{2}%')", this.UniqueId, strColumn, CTmaxToolbox.SQLEncode(strText));
			
			strSQL = String.Format("AutoId IN ({0})", strSelect);
				
			return strSQL;
					
		}// public string GetSQL(string strText)
		
		#endregion Public Methods
		
		#region Protected Members
		
		/// <summary>Overridden base member to display the code as a string</summary>
		/// <returns>The code name if available</returns>
		public override string ToString()
		{
			if(this.Name.Length > 0)
				return this.Name;
			else
				return base.ToString();
				
		}// public override string ToString()

		#endregion Protected Members
		
		#region Private Methods
		
		/// <summary>Called to retrieve the associated enumeration</summary>
		/// <param name="strType">The enumerated name</param>
		/// <returns>The equivalent enumerator</returns>
		private TmaxCodeTypes StringToType(string strType)
		{
			foreach(int i in Enum.GetValues(typeof(TmaxCodeTypes)))
			{
				if(String.Compare(((TmaxCodeTypes)i).ToString(), strType, true) == 0)
					return ((TmaxCodeTypes)i);
			}
			
			return TmaxCodeTypes.Unknown;
		
		}// private TmaxCodeTypes StringToType(string strType)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Unique identifier assigned to the code</summary>
		public long UniqueId
		{
			get{ return m_lUniqueId; }
			set{ m_lUniqueId = value; }
		}
		
		/// <summary>The unique id of the pick list bound to this code</summary>
		public long PickListId
		{
			get{ return m_lPickListId; }
			set{ m_lPickListId = value; }
		}
		
		/// <summary>Collection of values allowed for this code</summary>
		public CTmaxPickItem PickList
		{
			get{ return m_tmaxPickList; }
			set{ SetPickList(value); }
		}
		
		/// <summary>Name used to identify the code</summary>
		public string Name
		{
			get{ return m_strName; }
			set{ m_strName = value; }
		}
		
		/// <summary>The enumerated case code type identifier</summary>
		public TmaxCodeTypes Type
		{
			get{ return m_eType; }
			set{ m_eType = value; }
		}
		
		/// <summary>Enumerated coded property identifier associated with this code</summary>
		public TmaxCodedProperties CodedProperty
		{
			get{ return m_eCodedProperty; }
			set{ m_eCodedProperty = value; }
		}
		
		/// <summary>True if this code is bound to a coded property</summary>
		public bool IsCodedProperty
		{
			get{ return (m_eCodedProperty != TmaxCodedProperties.Invalid); }
		}
		
		/// <summary>The packed flags that define the attributes</summary>
		public long Attributes
		{
			get { return m_lAttributes; }
			set { m_lAttributes = value; }
		}
		
		/// <summary>True to allow multiple instances of this code for each record</summary>
		public bool AllowMultiple
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxCaseCodeAttributes.AllowMultiple) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxCaseCodeAttributes.AllowMultiple;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxCaseCodeAttributes.AllowMultiple);
				}
			
			}
		
		}// public bool AllowMultiple
		
		/// <summary>True to hide this code from the user</summary>
		public bool Hidden
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxCaseCodeAttributes.Hidden) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxCaseCodeAttributes.Hidden;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxCaseCodeAttributes.Hidden);
				}
			
			}
		
		}// public bool Hidden
		
		/// <summary>This is a reference to the data exchange object associated with this object</summary>
		public object DxRecord
		{
			get { return m_dxRecord; }
			set { m_dxRecord = value; }
		}
		
		/// <summary>User defined sort order identifier</summary>
		public long SortOrder
		{
			get { return m_lSortOrder; }
			set { m_lSortOrder = value; }
		}
		
		/// <summary>Collection used to manage fielded data during an import operation</summary>
		public ArrayList Imported
		{
			get { return m_aImported; }
			set { m_aImported = value; }
		}

		/// <summary>true if bound to a multi-level pick list</summary>
		public bool IsMultiLevel
		{
			get
			{
				if(this.PickList != null)
					return (this.PickList.Type == TmaxPickItemTypes.MultiLevel);
				else
					return false;
			}
		}
		
		#endregion Properties
		
	}// public class CTmaxCaseCode

	/// <summary>Objects of this class are used to manage a dynamic array of CTmaxCaseCode objects</summary>
	public class CTmaxCaseCodes : CTmaxSortedArrayList
	{
		#region Constants
		
		public const int SORT_ON_NAME	= 0;
		public const int SORT_ON_ORDER	= 1;
		
		private const string XMLINI_SECTION_NAME						= "caseCodes";
		private const string XMLINI_CASE_CODE_ATTRIBUTE_ID				= "id";
		private const string XMLINI_CASE_CODE_ATTRIBUTE_NAME			= "name";
		private const string XMLINI_CASE_CODE_ATTRIBUTE_TYPE			= "type";
		private const string XMLINI_CASE_CODE_ATTRIBUTE_CODED_PROPERTY	= "codedProperty";
		private const string XMLINI_CASE_CODE_ATTRIBUTE_ALLOW_MULTIPLE	= "allowMultiple";
		private const string XMLINI_CASE_CODE_ATTRIBUTE_PICK_LIST_ID	= "pickListId";
		private const string XMLINI_CASE_CODE_ATTRIBUTE_SORT_ORDER		= "sortOrder";

        //Added constant to get case codes from the text file format on 28June2012.
        private const string TXT_CASE_CODE_HEADER_NAME = "Name";
        private const string TXT_CASE_CODE_HEADER_TYPE = "Type";
        private const string TXT_CASE_CODE_HEADER_CODEDPROPERTY = "CodedProperty";
        private const string TXT_CASE_CODE_HEADER_ALLOWMULTIPLE = "AllowMultiple";
        private const string TXT_CASE_CODE_HEADER_UNIQUEID = "UniqueId";

        private const string TXT_CASE_CODE_INSTRUCTIONS = @"



//---Information below this line will not be use in the import funcionality,
//this is only generated for the user assistance for manual insertion.

Note: Above value are tab seperated

/* Types
0 (Unknown),
1 (Integer),
2 (Decimal),
3 (Text),
4 (Memo),
5 (Boolean),
6 (Date),
7 (PickList),
*/

/* Coded Properties
0 (Invalid),
1 (Description),
2 (Exhibit),
3 (Admitted),
*/

/* Allow Multiple
true,
false
*/

*/ UniqueId can be empty */";
   

		private const int ERROR_FIND_EX						= 0;
		private const int ERROR_ADD_EX						= 1;
		private const int ERROR_REMOVE_EX					= 2;
		private const int ERROR_ADD_CONSTANT_EX				= 3;
		private const int ERROR_INITIALIZE_PICK_LISTS_EX	= 4;
		
		public const int CASE_CODES_MAX_TEXT_LENGTH = 255;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		protected CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxCaseCodes() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			((CTmaxSorter)Comparer).Mode = SORT_ON_ORDER;
			this.KeepSorted = false;// Application will do the sorting
			
			this.EventSource.Name = "Case Codes";
			
			//	Populate the error builder collection
			SetErrorStrings();
			
		}// public CTmaxCaseCodes() : base()

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="tmaxCaseCode">CTmaxCaseCode object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxCaseCode Add(CTmaxCaseCode tmaxCaseCode)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxCaseCode as object);

				return tmaxCaseCode;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
				return null;
			}
			
		}// Add(CTmaxCaseCode tmaxCaseCode)

		/// <summary>This method add the constant codes to the collection</summary>
		/// <returns>True if successful</returns>
		public bool AddCodedProps()
		{
			bool bSuccessful = true;
			
			foreach(TmaxCodedProperties O in Enum.GetValues(typeof(TmaxCodedProperties)))
			{
				if(O != TmaxCodedProperties.Invalid)
				{
					if(Add(O) == null)
						bSuccessful = false;
				
				}// if(O != TmaxCodedProperties.Invalid)
				
			}// foreach(TmaxCodedProperties O in Enum.GetValues(typeof(TmaxCodedProperties)))
			
			return bSuccessful;
			
		}// public bool AddCodedProps()

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="tmaxCaseCode">The object to be removed</param>
		public void Remove(CTmaxCaseCode tmaxCaseCode)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxCaseCode as object);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Remove", m_tmaxErrorBuilder.Message(ERROR_REMOVE_EX), Ex);
			}
		
		}//	public void Remove(CTmaxCaseCode tmaxCaseCode)
		
		/// <returns>The object with the specified Name</returns>
		public CTmaxCaseCode Find(string strName)
		{
			try
			{
				// Search for the object with the same Name
				foreach(CTmaxCaseCode O in this)
				{
					if(String.Compare(O.Name, strName, true) == 0)
					{
						return O;
					}
				
				}// foreach(CTmaxCaseCode O in this)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Find", m_tmaxErrorBuilder.Message(ERROR_FIND_EX), Ex);
			}
			
			return null;

		}//	public CTmaxCaseCode Find(string strName)

		/// <summary>This method locates the object with the specified Id</summary>
		/// <param name="lId">The id of the desired object</param>
		/// <returns>The object with the specified Id</returns>
		public CTmaxCaseCode Find(long lId)
		{
			try
			{
				// Search for the object with the same Id
				foreach(CTmaxCaseCode O in this)
				{
					if(lId == O.UniqueId)
					{
						return O;
					}
				
				}// foreach(CTmaxCaseCode O in this)
				
			}
			catch
			{
			}
			
			return null;

		}//	public CTmaxCaseCode Find(long lId)

		/// <summary>This method locates the object that represents the coded property</summary>
		/// <param name="e">The enumerated coded property</param>
		/// <returns>The code used to represent the specified property</returns>
		public CTmaxCaseCode Find(TmaxCodedProperties e)
		{
			try
			{
				// Search for the object with the same property code
				foreach(CTmaxCaseCode O in this)
				{
					if(O.CodedProperty == e)
					{
						return O;
					}
				
				}// foreach(CTmaxCaseCode O in this)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Find", m_tmaxErrorBuilder.Message(ERROR_FIND_EX), Ex);
			}
			
			return null;

		}
		
		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			try 
			{ 
				base.Clear(); 
			}
			catch(System.Exception Ex) 
			{
				m_tmaxEventSource.FireDiagnostic(this, "Clear", Ex);
			}
		
		}// public override void Clear()

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxCaseCode">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxCaseCode tmaxCaseCode)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxCaseCode as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxCaseCode this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxCaseCode);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxCaseCode value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>This method is called to get the id to be assigned to the next code to be added to the collection</summary>
		/// <returns>The next id</returns>
		public long GetNextId()
		{
			long lNextId = 0;
			
			foreach(CTmaxCaseCode O in this)
			{
				if(lNextId < O.UniqueId)
					lNextId = O.UniqueId;
			}
			
			return (lNextId + 1);
			
		}// public long GetNextId()

		
		/// <summary>This method is called to copy the objects in the specified collection</summary>
		/// <param name="tmaxCaseCodes">The collection of objects to be copied</param>
		/// <param name="bClear">True to clear the collection before copying the objects</param>
		/// <returns>true if no error occurs</returns>
		public bool Copy(CTmaxCaseCodes tmaxCaseCodes, bool bClear)
		{
			bool bSuccessful = true;
			
			if(tmaxCaseCodes != null)
			{
				foreach(CTmaxCaseCode O in tmaxCaseCodes)
				{
					try { this.Add(new CTmaxCaseCode(O)); }
					catch { bSuccessful = false; }
					
				}// foreach(CTmaxCaseCode O in tmaxCaseCodes)
				
				return bSuccessful;
			}
			else
			{
				return false;
			}

		}// public bool Copy(CTmaxCaseCodes tmaxCaseCodes, bool bClear)

		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			int				iLine = 1;
			CTmaxCaseCode	tmaxCode = null;
			bool			bContinue = true;
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			//	Load all the codes
			this.Clear();
			while(bContinue == true)
			{
				tmaxCode = new CTmaxCaseCode();
				
				if((bContinue = Load(xmlIni, GetIniKey(iLine++), tmaxCode)) == true)
					this.Add(tmaxCode);
			}

			//	Make sure we have the coded properties
			this.AddCodedProps();
				
			//	Were sort orders stored in the file?
			//
			//	NOTE:	Sort orders did not get stored until ver 6.2.1
			if((this.Count > 0) && (this.GetNextSortOrder() == 1))
			{
				for(int i = 0; i < this.Count; i++)
					this[i].SortOrder = i + 1;
			}
			
			//	Make sure everything is properly sorted
			this.Sort(true);
				
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			int	iLine = 1;
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME, true, true) == false) return;
			
			//	Write the case codes to file
			foreach(CTmaxCaseCode O in this)
			{
				Save(xmlIni, GetIniKey(iLine++), O);
			}
			
		}// public void Save(CXmlIni xmlIni)

        /// <summary>
        /// This method is called to store the options in the specified Text ini file
        /// </summary>
        /// <param name="TextIni"></param>
        public void SaveToTextFile(CTextIni textIni)
        {
            try
            {
                textIni.Write(TXT_CASE_CODE_HEADER_NAME + "\t" + TXT_CASE_CODE_HEADER_TYPE + "\t" +
                              TXT_CASE_CODE_HEADER_CODEDPROPERTY +
                              "\t" + TXT_CASE_CODE_HEADER_ALLOWMULTIPLE + "\t" + TXT_CASE_CODE_HEADER_UNIQUEID);
                //	Write the case codes to file
                foreach (CTmaxCaseCode O in this)
                {
                    SaveToTextFile(textIni, O);
                }
                textIni.Write(TXT_CASE_CODE_INSTRUCTIONS);
                
            }
            catch(Exception e)
            {

            }
        }


		/// <summary>Called to get the case codes that reference the specified pick list or one of its children</summary>
		/// <param name="tmaxPickItem">The pick item referenced by the codes</param>
		/// <param name="tmaxCaseCodes">The array in which to store the codes</param>
		/// <param name="bRecurse">True to recurse child pick lists</param>
		/// <returns>The number of codes added to the collection</returns>
		public int GetReferences(CTmaxPickItem tmaxPickItem, CTmaxCaseCodes tmaxCaseCodes, bool bRecurse)
		{
			int iExisting = 0;
			
			Debug.Assert(tmaxPickItem != null);
			if(tmaxPickItem == null) return 0;
			Debug.Assert(tmaxCaseCodes != null);
			if(tmaxCaseCodes == null) return 0;
			
			try
			{
				//	How may items are in the collection
				iExisting = tmaxCaseCodes.Count;
				
				//	Is this item a value list?
				//
				//	NOTE:	Case codes are bound only to value lists
				if(tmaxPickItem.Type == TmaxPickItemTypes.StringList)
				{
					//	Search for codes that reference the list
					foreach(CTmaxCaseCode O in this)
					{
						if((O.Type == TmaxCodeTypes.PickList) && (O.PickListId == tmaxPickItem.UniqueId))
							tmaxCaseCodes.Add(O);
							
					}// foreach(CTmaxCaseCode O in this)
					
				}
				else if(tmaxPickItem.Children != null)
				{
					foreach(CTmaxPickItem O in tmaxPickItem.Children)
					{
						if(O.Type == TmaxPickItemTypes.StringList)
						{
							tmaxCaseCodes.Add(O);
						}
						else if((bRecurse == true) && (O.Type == TmaxPickItemTypes.MultiLevel))
						{
							GetReferences(O, tmaxCaseCodes, true);
						}
					
					}// foreach(CTmaxPickItem O in tmaxPickItem.Children)

				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetReferences", Ex);
			}
			
			return (tmaxCaseCodes.Count - iExisting);
				
		}// public int GetReferences(CTmaxPickItem tmaxPickItem, CTmaxCaseCodes tmaxCaseCodes, bool bRecurse)
		
		/// <summary>Called to get the case codes that reference the specified pick list or one of its children</summary>
		/// <param name="tmaxPickItem">The pick item referenced by the codes</param>
		/// <param name="bRecurse">True to recurse child pick lists</param>
		/// <returns>The collection of codes that reference the specified pick list</returns>
		public CTmaxCaseCodes GetReferences(CTmaxPickItem tmaxPickItem, bool bRecurse)
		{
			CTmaxCaseCodes tmaxCaseCodes = null;
			
			Debug.Assert(tmaxPickItem != null);
			if(tmaxPickItem == null) return null;
			
			tmaxCaseCodes = new CTmaxCaseCodes();
			GetReferences(tmaxPickItem, tmaxCaseCodes, bRecurse);
			
			if((tmaxCaseCodes != null) && (tmaxCaseCodes.Count > 0))
				return tmaxCaseCodes;
			else
				return null;
				
		}// public CTmaxCaseCodes GetReferences(CTmaxPickItem tmaxPickItem, bool bRecurse)
		
		/// <summary>Called to get the case codes that reference the specified pick list or one of its children</summary>
		/// <param name="tmaxPickItem">The pick item referenced by the codes</param>
		/// <returns>The collection of codes that reference the specified pick list</returns>
		public CTmaxCaseCodes GetReferences(CTmaxPickItem tmaxPickItem)
		{
			return GetReferences(tmaxPickItem, true);
		}
		
		/// <summary>This method is called to get the code name for the specified coded property</summary>
		/// <param name="e">The enumerated coded property identifier</param>
		/// <returns>The name of the associated code</returns>
		static public string GetCodeName(TmaxCodedProperties e)
		{
			return e.ToString();
		}
		
		/// <summary>This method is called to get the maximum length allowed for a text field</summary>
		/// <returns>The maximum number of characters</returns>
		static public int GetMaxTextLength()
		{
			return CASE_CODES_MAX_TEXT_LENGTH;
		}
		
		/// <summary>This method is called to get the description of the specified type</summary>
		/// <returns>The description of the data type</returns>
		static public string GetTypeDescription(TmaxCodeTypes eType)
		{
			string strDescription = "Unknown";
			
			switch(eType)
			{
				case TmaxCodeTypes.Boolean:		strDescription = "Two state (true/false)";
												break;
				case TmaxCodeTypes.Date:		strDescription = "Formatted date MM/DD/YYYY";
												break;
				case TmaxCodeTypes.Decimal:		strDescription = "+/- Decimal Number";
												break;
				case TmaxCodeTypes.Integer:		strDescription = "+/- Whole Number";
												break;
				case TmaxCodeTypes.Text:		strDescription = "Single line <= 255 characters";
												break;
				case TmaxCodeTypes.Memo:		strDescription = "Multi-line text";
												break;
				case TmaxCodeTypes.PickList:	strDescription = "Predefined value group";
					break;
			}
			
			return strDescription;
		
		}// static public string GetTypeDescription(TmaxCodeTypes eType)
		
		/// <summary>This method is called to get the code id for the specified coded property</summary>
		/// <param name="e">The enumerated coded property identifier</param>
		/// <returns>The id of the associated code</returns>
		static public long GetCodeId(TmaxCodedProperties e)
		{
			return (-1 * (long)e);	
		}
		
		/// <summary>This method is called to get the code type for the specified coded property</summary>
		/// <param name="e">The enumerated coded property identifier</param>
		/// <returns>The type of the associated code</returns>
		static public TmaxCodeTypes GetCodeType(TmaxCodedProperties e)
		{
			switch(e)
			{
				case TmaxCodedProperties.Admitted:
				
					return TmaxCodeTypes.Boolean;
				
				case TmaxCodedProperties.Description:
				
					return TmaxCodeTypes.Memo;
				
				case TmaxCodedProperties.Exhibit:
				default:
				
					return TmaxCodeTypes.Text;
					
			}	
		
		}// static public TmaxCodeTypes GetCodeType(TmaxCodedProperties e)
		
		/// <summary>This method is called to get the next sort order identifier</summary>
		/// <returns>The next sort order value to assign for new items</returns>
		public long GetNextSortOrder()
		{
			long lMaxOrder = 0;
			
			//	We can't assume the collection is sorted on the SortOrder field
			foreach(CTmaxCaseCode O in this)
			{
				if(O.SortOrder > lMaxOrder)
					lMaxOrder = O.SortOrder;
			}
			
			return (lMaxOrder + 1);
		
		}// public long GetNextSortOrder()

		/// <summary>This method is called to create a case code to represent the specified import property</summary>
		/// <returns>The case code to identify the property</returns>
		private CTmaxCaseCode CreateCode(TmaxImportProperties e)
		{
			CTmaxCaseCode tmaxCode = null;
			
			try
			{
				if(e != TmaxImportProperties.Invalid)
				{
					//	Allocate and initialize the new code
					tmaxCode = new CTmaxCaseCode();
					tmaxCode.Name = e.ToString();
					tmaxCode.UniqueId = (int)e;
					tmaxCode.Type = TmaxCodeTypes.Unknown;
				}
					
			}
			catch
			{
			}
			
			return tmaxCode;
		
		}// private CTmaxCaseCode CreateCode(TmaxImportProperties e)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method converts the numeric line identifier to a unique key for storing the term descriptor</summary>
		/// <param name="iCode">unique numeric identifier for the case code</param>
		/// <returns>The appropriate XML Ini key</returns>
		string GetIniKey(int iCode)
		{
			return String.Format("CC{0}", iCode);
		}
		
		/// <summary>This method is called to load the information for the specified filter term from the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxCode">The term to be initialized</param>
		///	<returns>true to continue loading</returns>
		public bool Load(CXmlIni xmlIni, string strKey, CTmaxCaseCode tmaxCode)
		{
			try
			{
				//	Read the name and ID of this code
				tmaxCode.Name = xmlIni.Read(strKey, XMLINI_CASE_CODE_ATTRIBUTE_NAME, "");
				tmaxCode.UniqueId = xmlIni.ReadLong(strKey, XMLINI_CASE_CODE_ATTRIBUTE_ID, 0);
				
				//	Have we run out of codes?
				if((tmaxCode.UniqueId == 0) && (tmaxCode.Name.Length == 0)) return false;
				
				try { tmaxCode.CodedProperty = (TmaxCodedProperties)(xmlIni.ReadInteger(strKey, XMLINI_CASE_CODE_ATTRIBUTE_CODED_PROPERTY, 0)); }
				catch {}
				
				try { tmaxCode.Type = (TmaxCodeTypes)(xmlIni.ReadInteger(strKey, XMLINI_CASE_CODE_ATTRIBUTE_TYPE, 0)); }
				catch {}
				
				tmaxCode.SortOrder = xmlIni.ReadLong(strKey, XMLINI_CASE_CODE_ATTRIBUTE_SORT_ORDER, 0);
				tmaxCode.PickListId = xmlIni.ReadLong(strKey, XMLINI_CASE_CODE_ATTRIBUTE_PICK_LIST_ID, 0);
				tmaxCode.AllowMultiple = xmlIni.ReadBool(strKey, XMLINI_CASE_CODE_ATTRIBUTE_ALLOW_MULTIPLE, true);

				//	NOTE:	We intentionally override the Hidden property. That will be stored
				//			and retrieved from the local station options
				tmaxCode.Hidden = false;
				
				return true;
			}
			catch
			{
				//	NOTE: We don't return FALSE because the rest of the codes might be OK
			}
			
			return true;
			
		}// public bool Load(CXmlIni xmlIni, string strKey, CTmaxCaseCode tmaxCode)
		
		/// <summary>This method is called to store the specified case code in the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxCode">The code to be stored in the file</param>
		public bool Save(CXmlIni xmlIni, string strKey, CTmaxCaseCode tmaxCode)
		{
			try
			{
				xmlIni.Write(strKey, XMLINI_CASE_CODE_ATTRIBUTE_NAME, tmaxCode.Name);
				xmlIni.Write(strKey, XMLINI_CASE_CODE_ATTRIBUTE_ID, tmaxCode.UniqueId);
				xmlIni.Write(strKey, XMLINI_CASE_CODE_ATTRIBUTE_TYPE, (int)(tmaxCode.Type));
				xmlIni.Write(strKey, XMLINI_CASE_CODE_ATTRIBUTE_CODED_PROPERTY, (int)(tmaxCode.CodedProperty));
				xmlIni.Write(strKey, XMLINI_CASE_CODE_ATTRIBUTE_ALLOW_MULTIPLE, tmaxCode.AllowMultiple);
				xmlIni.Write(strKey, XMLINI_CASE_CODE_ATTRIBUTE_SORT_ORDER, tmaxCode.SortOrder);
				xmlIni.Write(strKey, XMLINI_CASE_CODE_ATTRIBUTE_PICK_LIST_ID, tmaxCode.PickListId);
				
				//	NOTE:	We intentionally skip the Hidden property. That will be stored
				//			and retrieved from the local station options
				
				return true;
				
			}
			catch
			{
				return false;
			}
			
		}// public bool Save(CXmlIni xmlIni)
		
        /// <summary>
		/// This method is called to write case codes information to the text file.
		/// </summary>
		/// <param name="textIni"></param>
		/// <param name="tmaxCode"></param>
		/// <returns></returns>
        public bool SaveToTextFile(CTextIni textIni, CTmaxCaseCode tmaxCode)
        {
            try
            {
                textIni.Write("\r\n" + tmaxCode.Name +"\t"+ ((int)tmaxCode.Type).ToString() + "\t" + ((int)tmaxCode.CodedProperty).ToString() + "\t" 
                    + tmaxCode.AllowMultiple + "\t" + tmaxCode.UniqueId);
                return true;
            }
            catch
            {
                return false;
            }
        }//public bool SaveToTextFile(CTextIni textIni, CTmaxCaseCode tmaxCode)

		/// <summary>This method adds the specified coded property to the collection</summary>
		/// <param name="e">The enumerated coded property value</param>
		/// <returns>The code object</returns>
		private CTmaxCaseCode Add(TmaxCodedProperties e)
		{
			CTmaxCaseCode	tmaxCode = null;
			ArrayList		aRemove = null;
			string			strName = "";
			long			lUniqueId = 0;

			try
			{
				//	Must be a valid coded property
				if(e == TmaxCodedProperties.Invalid) return null;
				
				//	Get the name and unique id of this code
				strName = CTmaxCaseCodes.GetCodeName(e);
				lUniqueId = CTmaxCaseCodes.GetCodeId(e);
				
				//	Make sure there are no other codes with the same name
				aRemove = new ArrayList();
				foreach(CTmaxCaseCode O in this)
				{
					//	Do the names match?
					if(String.Compare(O.Name, strName, true) == 0)
					{
						//	Is this the coded property?
						if(lUniqueId == O.UniqueId)
						{
							if(tmaxCode != null)
								aRemove.Add(O);	//	Can't have more than one
							else
								tmaxCode = O;
						}
						else
						{
							//	Can't have a user code with the same name
							aRemove.Add(O);
						}
					
					}// if(String.Compare(O.Name, strName) == 0)
					
				}// foreach(CTmaxCode O in this)
					
				//	Do we have any to remove?
				if((aRemove != null) && (aRemove.Count > 0))
				{
					foreach(CTmaxCaseCode O in aRemove)
						this.Remove(O);
				}
				
				//	Do we need to add a new code?
				if(tmaxCode == null)
				{
					//	Add to the collection
					tmaxCode = new CTmaxCaseCode(e);
					this.Add(tmaxCode);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddCodedProp", m_tmaxErrorBuilder.Message(ERROR_ADD_CONSTANT_EX, strName), Ex);
				tmaxCode = null;
			}
			
			return tmaxCode;
		
		}// private CTmaxCaseCode Add(TmaxCodedProperties e)
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to locate an object in the collection");
			aStrings.Add("An exception was raised while attempting to add a new object to the collection");
			aStrings.Add("An exception was raised while attempting to remove an object from the collection");
			aStrings.Add("An exception was raised while attempting to add a constant code to the collection: Name = %1");
			aStrings.Add("An exception was raised while attempting to add a initialize the pick lists collecton.");

		}// private virtual void SetErrorStrings()
		
		#endregion Private Methods
		
		#region Properties
		
		//	Controls the field used to sort objects in the collection
		public int SortOn
		{
			get { return (this.Comparer != null ? (int)(((CTmaxSorter)(this.Comparer)).Mode) : CTmaxPickItems.SORT_ON_NAME); }
			set { if(this.Comparer != null) ((CTmaxSorter)(this.Comparer)).Mode = value; }
		}
		
		#endregion Properties
		
	}//	public class CTmaxCaseCodes
		
}// namespace FTI.Shared.Xml
