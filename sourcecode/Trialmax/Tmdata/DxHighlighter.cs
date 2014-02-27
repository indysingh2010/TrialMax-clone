using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class encapsulates the information used to exchange a SecondaryMedia record
	/// </summary>
	public class CDxHighlighter : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Constants
		
		protected const int DXHIGHLIGHTER_PROP_FIRST_ID		= 10000; // Ensures unique property id
		
		protected const int DXHIGHLIGHTER_ID				= DXHIGHLIGHTER_PROP_FIRST_ID + 1;
		protected const int DXHIGHLIGHTER_GROUP				= DXHIGHLIGHTER_PROP_FIRST_ID + 2;
		protected const int DXHIGHLIGHTER_NAME				= DXHIGHLIGHTER_PROP_FIRST_ID + 3;
		protected const int DXHIGHLIGHTER_COLOR				= DXHIGHLIGHTER_PROP_FIRST_ID + 4;
	
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Color property</summary>
		private long m_lColor = System.Drawing.Color.Black.ToArgb();
				
		/// <summary>Local member bound to Group property</summary>
		protected FTI.Shared.Trialmax.TmaxHighlighterGroups m_eGroup = TmaxHighlighterGroups.Defendant;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxHighlighter() : base()
		{
		}
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Highlighter;
		}
		
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			return FTI.Shared.Trialmax.TmaxMediaLevels.None;
		}
		
		/// <summary>This method is called to get the color as a system color value</summary>
		/// <returns>The highlighter's system color value</returns>
		public System.Drawing.Color GetSysColor()
		{
			try
			{
				return System.Drawing.Color.FromArgb((int)m_lColor);
			}
			catch
			{
				return System.Drawing.Color.Black;
			}
			
		}// public System.Drawing.Color GetSysColor()
		
		/// <summary>This method is called to get the color as an OLE_COLOR value</summary>
		/// <returns>The highlighter's OLE color value</returns>
		public int GetOleColor()
		{
			return System.Drawing.ColorTranslator.ToOle(SysColor);
			
		}// public int GetOleColor()
		
		/// <summary>This method is called to set the highlighter color using the system color value</summary>
		/// <param name="sysColor">The desired system color</param>
		/// <returns>true if successful</returns>
		public bool SetSysColor(System.Drawing.Color sysColor)
		{
			try
			{
				m_lColor = sysColor.ToArgb();
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public BOOL SetSysColor(System.Drawing.Color sysColor)
		
		/// <summary>This method is called to set the highlighter color using an OLE_COLOR value</summary>
		/// <param name="sysColor">The desired OLE color</param>
		/// <returns>true if successful</returns>
		public bool SetOleColor(int iColor)
		{
			return SetSysColor(System.Drawing.ColorTranslator.FromOle(iColor));
		}
		
		/// <summary>This function is called to get the default text descriptor for the record</summary>
		public override string GetText()
		{
			if(m_strName.Length == 0)
				return "Unassigned";
			else
				return m_strName;
		}
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		/// <param name="tmaxProperties">The collection in which to place the properties</param>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			tmaxProperties.Add(DXHIGHLIGHTER_ID, "HL Id", AutoId, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXHIGHLIGHTER_GROUP, "HL Group", m_eGroup, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXHIGHLIGHTER_NAME, "HL Name", Name, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXHIGHLIGHTER_COLOR, "HL Color", SysColor, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			
		}// public override void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public override void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case DXHIGHLIGHTER_GROUP:
				
					tmaxProperty.Value = m_eGroup;
					break;
					
				case DXHIGHLIGHTER_NAME:
				
					tmaxProperty.Value = Name;
					break;
					
				case DXHIGHLIGHTER_COLOR:
				
					tmaxProperty.Value = SysColor;
					break;
					
				//	These properties are read-only
				case DXHIGHLIGHTER_ID:
				
					break;
					
				default:
				
					// No need to call base class because we don't put base class
					// properties in the caller's collection
					
					break;
					
			}// switch(tmaxProperty.Id)
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The highlighter argb color</summary>
		public long Color
		{
			get { return m_lColor; }
			set { m_lColor = value; }
		}
		
		/// <summary>The highlighter system color</summary>
		public System.Drawing.Color SysColor
		{
			get { return GetSysColor(); }
			set { SetSysColor(value); }
		}
		
		/// <summary>The highlighter OLE color</summary>
		public int OleColor
		{
			get { return GetOleColor(); }
			set { SetOleColor(value); }
		}
		
		/// <summary>The highlighter group</summary>
		public FTI.Shared.Trialmax.TmaxHighlighterGroups Group
		{
			get	{ return m_eGroup; }
			set { m_eGroup = value; }
		}
		
		#endregion Properties
	
	}// class CDxHighlighter

	/// <summary>
	/// This class is used to manage a ArrayList of CDxHighlighter objects
	/// </summary>
	public class CDxHighlighters : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			Color,
			GroupId,
			Name,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
		}

		public const string TABLE_NAME = "Highlighters";
		
		#endregion Constants
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxHighlighters() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxHighlighters(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxHighlighter">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxHighlighter Add(CDxHighlighter dxHighlighter)
		{
			return (CDxHighlighter)base.Add(dxHighlighter);
			
		}// Add(CDxHighlighter dxHighlighter)

		/// <summary>This method is called to add a highlighter to the database</summary>
		/// <param name="color">The highlighter color</param>
		/// <param name="eGroup">The group that owns the highlighter</param>
		/// <param name="strName">The name assigned to the highlighter</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxHighlighter Add(System.Drawing.Color color, TmaxHighlighterGroups eGroup, string strName)
		{
			try
			{
				CDxHighlighter dxHighlighter = new CDxHighlighter();
				
				dxHighlighter.Color = color.ToArgb();
				dxHighlighter.Group = eGroup;
				
				if((strName != null) && (strName.Length > 0))
					dxHighlighter.Name = strName;
					
				return Add(dxHighlighter);
			}
			catch(System.Exception Ex)
			{
                FireError("Add",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_HIGHLIGHTER_ADD_EX),Ex);
				return null;
			}
			
		}// Add(CDxHighlighter dxHighlighter)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxHighlighters Dispose()
		{
			return (CDxHighlighters)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxHighlighter Find(long lAutoId, bool bBarcode)
		{
			return (CDxHighlighter)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxHighlighter Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxHighlighter this[int iIndex]
		{
			get
			{
				return (CDxHighlighter)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxHighlighter GetAt(int iIndex)
		{
			return (CDxHighlighter)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxHighlighter	dxHighlighter = (CDxHighlighter)dxRecord;
			string			strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.Color.ToString() + ",");
			strSQL += (eFields.GroupId.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ",");
			strSQL += (eFields.ModifiedBy.ToString() + ",");
			strSQL += (eFields.ModifiedOn.ToString() + ")");
			
			strSQL += " VALUES(";
			strSQL += ("'" + dxHighlighter.Color.ToString() + "',");
			strSQL += ("'" + ((int)(dxHighlighter.Group)).ToString() + "',");
			strSQL += ("'" + SQLEncode(dxHighlighter.Name) + "',");
			strSQL += ("'" + dxHighlighter.CreatedBy.ToString() + "',");
			strSQL += ("'" + dxHighlighter.CreatedOn.ToString() + "',");
			strSQL += ("'" + dxHighlighter.ModifiedBy.ToString() + "',");
			strSQL += ("'" + dxHighlighter.ModifiedOn.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxHighlighter	dxHighlighter = (CDxHighlighter)dxRecord;
			string			strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.Color.ToString() + " = '" + dxHighlighter.Color.ToString() + "',");
			strSQL += (eFields.GroupId.ToString() + " = '" + ((int)(dxHighlighter.Group)).ToString() + "',");
			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxHighlighter.Name) + "',");
			strSQL += (eFields.CreatedBy.ToString() + " = '" + dxHighlighter.CreatedBy.ToString() + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + dxHighlighter.CreatedOn.ToString() + "',");
			strSQL += (eFields.ModifiedBy.ToString() + " = '" + dxHighlighter.ModifiedBy.ToString() + "',");
			strSQL += (eFields.ModifiedOn.ToString() + " = '" + dxHighlighter.ModifiedOn.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxHighlighter.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to select the desired records
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string strSQL = ("SELECT * FROM " + m_strTableName + ";");
			return strSQL;
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected override string[] GetFieldNames()
		{
			return Enum.GetNames(typeof(eFields));
		}
		
		/// <summary>This function is called to get a new record object</summary>
		/// <returns>A new object of the collection type</returns>
		protected override CBaseRecord GetNewRecord()
		{
			CDxHighlighter dxHighlighter = new CDxHighlighter();
			
			return ((CBaseRecord)dxHighlighter);
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxHighlighter dxHighlighter = (CDxHighlighter)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxHighlighter.AutoId;
					m_dxFields[(int)eFields.Color].Value  = dxHighlighter.Color;
					m_dxFields[(int)eFields.GroupId].Value  = dxHighlighter.Group;
					m_dxFields[(int)eFields.Name].Value = dxHighlighter.Name;
					m_dxFields[(int)eFields.CreatedBy].Value = dxHighlighter.CreatedBy;
					m_dxFields[(int)eFields.CreatedOn].Value = dxHighlighter.CreatedOn;
					m_dxFields[(int)eFields.ModifiedBy].Value = dxHighlighter.ModifiedBy;
					m_dxFields[(int)eFields.ModifiedOn].Value = dxHighlighter.ModifiedOn;
				}
				else
				{
					dxHighlighter.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxHighlighter.Color = (int)(m_dxFields[(int)eFields.Color].Value);
					dxHighlighter.Group = (FTI.Shared.Trialmax.TmaxHighlighterGroups)((short)(m_dxFields[(int)eFields.GroupId].Value));
					dxHighlighter.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxHighlighter.CreatedBy = (int)(m_dxFields[(int)eFields.CreatedBy].Value);
					dxHighlighter.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
					dxHighlighter.ModifiedBy = (int)(m_dxFields[(int)eFields.ModifiedBy].Value);
					dxHighlighter.ModifiedOn = (DateTime)(m_dxFields[(int)eFields.ModifiedOn].Value);
				}
				return true;
			}
			catch(OleDbException oleEx)
			{
                FireError("Exchange",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_COMMON_EXCHANGE_FIELDS_EX,TableName,bSetFields),oleEx,GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
                FireError("Exchange",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_COMMON_EXCHANGE_FIELDS_EX,TableName,bSetFields),Ex,GetErrorItems(dxRecord));
			}
			
			return false;
			
		}// Exchange()
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected override void SetNames()
		{
			m_strTableName = TABLE_NAME;
		}
		
		/// <summary>This method will set the default value for the specified field</summary>
		/// <param name="eField">The enumerated field identifier</param>
		/// <param name="dxField">The field object to be set</param>
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.AutoId:
				case eFields.Color:
				case eFields.CreatedBy:
				case eFields.ModifiedBy:
				
					dxField.Value = 0;
					break;
					
				case eFields.GroupId:
				
					dxField.Value = TmaxHighlighterGroups.Defendant;
					break;
					
				case eFields.Name:
				
					dxField.Value = "";
					break;
					
				case eFields.CreatedOn:
				case eFields.ModifiedOn:
				
					dxField.Value = System.DateTime.Now;
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods
		
	}//	CDxHighlighters
		
}// namespace FTI.Trialmax.Database
