using System;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

using FTI.Shared;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class serves as the base class for all data exchange objects used by
	/// the Trialmax application. Data exchange objects are used to transfer between
	/// the application and it's associated database.
	/// </summary>
	public class CDxUser : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Private Members
		
		/// <summary>Local member bound to LastTime property</summary>
		protected DateTime m_tsLastTime;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxUser()
		{
		}
	
		#endregion Public Methods

		#region Properties
		
		/// <summary>
		/// This property exposes the last time that this user opened the database
		/// </summary>
		public DateTime LastTime
		{
			get
			{
				return m_tsLastTime;
			}
			set
			{
				m_tsLastTime = value;
			}
		}
		
		#endregion Properties
		
	}// class CDxUser

	/// <summary>
	/// This class is used to manage a ArrayList of CDxUser objects
	/// </summary>
	public class CDxUsers : CDxMediaRecords
	{
		public enum eFields
		{
			AutoId = 0,
			Name,
			Description,
			LastTime,
		}

		public const string TABLE_NAME = "Users";
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxUsers() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxUsers(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxUsers Dispose()
		{
			return (CDxUsers)base.Dispose();
			
		}// Dispose()

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxUser">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxUser Add(CDxUser dxUser)
		{
			return (CDxUser)base.Add(dxUser);
			
		}// Add(CDxUser dxUser)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxUser Find(long lAutoId, bool bBarcode)
		{
			return (CDxUser)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxUser Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Called to locate the object with the specified Name value
		/// </summary>
		/// <param name="strName">The Name value of the desired object</param>
		/// <returns>The object with the specified Name</returns>
		public CDxUser Find(string strName)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(CDxUser obj in m_aList)
				{
					if(String.Compare(strName, obj.Name, true) == 0)
					{
						return obj;
					}
				}
			}

			return null;
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxUser this[int iIndex]
		{
			get 
			{ 
				return (CDxUser)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxUser GetAt(int iIndex)
		{
			return (CDxUser)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxUser	dxUser = (CDxUser)dxRecord;
			string	strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.Description.ToString() + ",");
			strSQL += (eFields.LastTime.ToString() + ")");
			
			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(dxUser.Name) + "',");
			strSQL += ("'" + SQLEncode(dxUser.Description) + "',");
			strSQL += ("'" + dxUser.LastTime.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxUser	dxUser = (CDxUser)dxRecord;
			string	strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.Name.ToString() + " = '" + dxUser.Name + "',");
			strSQL += (eFields.Description.ToString() + " = '" + dxUser.Description + "',");
			strSQL += (eFields.LastTime.ToString() + " = '" + dxUser.LastTime.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxUser.AutoId.ToString();
			strSQL += ";";
			
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
			return ((CBaseRecord)(new CDxUser()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxUser dxUser = (CDxUser)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value = dxUser.AutoId;
					m_dxFields[(int)eFields.Name].Value = dxUser.Name;
					m_dxFields[(int)eFields.Description].Value = dxUser.Description;
					m_dxFields[(int)eFields.LastTime].Value = dxUser.LastTime;
				}
				else
				{
					dxUser.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxUser.Name = m_dxFields[(int)eFields.Name].Value.ToString();
					dxUser.Description = m_dxFields[(int)eFields.Description].Value.ToString();
					dxUser.LastTime = (DateTime)(m_dxFields[(int)eFields.LastTime].Value);
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
		}
		
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
				
					dxField.Value = 0;
					break;
					
				case eFields.LastTime:
				
					dxField.Value = System.DateTime.Now;
					break;
					
				case eFields.Name:
				case eFields.Description:
				
					dxField.Value = "";
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods

	}//	CDxUsers
		
}// namespace FTI.Trialmax.Database
