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
	/// <summary>This class is used to define a record in the Versions table of the Objections database</summary>
	public class COxUser : FTI.Trialmax.Database.CBaseRecord, ITmaxBaseObjectionRecord
	{
		#region Private Members

		/// <summary>Local member bound to AutoId property</summary>
		private System.Guid m_guidUniqueId = System.Guid.Empty;

		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";

		/// <summary>Local member bound to LastTime property</summary>
		protected DateTime m_dtLastTime = System.DateTime.Now;
		
		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public COxUser() : base()
		{
		}

		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		/// <remarks>This member MUST be overridden by the derived class</remarks>
		public override string GetKeyId()
		{
			return ("{" + this.UniqueId.ToString() + "}");
		}

		/// <summary>This method retrieves the identifier assigned by the database</summary>
		///	<returns>The record id</returns>
		string ITmaxBaseObjectionRecord.GetUniqueId()
		{
			return this.UniqueId.ToString();
		}

		/// <summary>This method retrieves the text used to display this record</summary>
		///	<returns>The display text</returns>
		string ITmaxBaseObjectionRecord.GetText()
		{
			return this.Name;
		}

		#endregion Public Methods

		#region Properties

		/// <summary>The unique identifier assigned by the database</summary>
		public System.Guid UniqueId
		{
			get { return m_guidUniqueId; }
			set { m_guidUniqueId = value; }
		}

		/// <summary>The user's name</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}

		/// <summary>The last time the user accessed the database</summary>
		public System.DateTime LastTime
		{
			get { return m_dtLastTime; }
			set { m_dtLastTime = value; }
		}

		#endregion Properties

	}// public class COxUser : FTI.Trialmax.Database.CBaseRecord

	/// <summary>This class is used to manage a ArrayList of COxUser objects</summary>
	public class COxUsers : CBaseRecords
	{
		public enum eFields
		{
			UniqueId = 0,
			Name,
			LastTime,
		}

		public const string TABLE_NAME = "Users";

		#region Public Members

		/// <summary>Default constructor</summary>
		public COxUsers() : base()
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public COxUsers(CObjectionsDatabase tmaxDatabase) : base(tmaxDatabase)
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="OxUser">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public COxUser Add(COxUser oxUser)
		{
			COxUser oxAdded = null;

			if((oxAdded = (COxUser)base.Add(oxUser)) != null)
			{
				oxAdded.UniqueId = this.Database.GetAutoGuid(TABLE_NAME,eFields.LastTime.ToString());
			}

			return oxAdded;

		}// public COxUser Add(COxUser OxUser)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new COxUsers Dispose()
		{
			return (COxUsers)base.Dispose();

		}// public new COxUsers Dispose()

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new COxUser this[int iIndex]
		{
			get { return (COxUser)base[iIndex]; }

		}// public new COxUser this[int iIndex]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new COxUser GetAt(int iIndex)
		{
			return (COxUser)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			COxUser oxUser = (COxUser)dxRecord;
			string	strSQL = "INSERT INTO " + TableName + "(";

			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.LastTime.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(oxUser.Name) + "',");
			strSQL += ("'" + oxUser.LastTime.ToString() + "')");

			return strSQL;

		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			COxUser oxUser = (COxUser)dxRecord;
			string	strSQL = "UPDATE " + TableName + " SET ";

			strSQL += (eFields.Name.ToString() + " = '" + oxUser.Name + "',");
			strSQL += (eFields.LastTime.ToString() + " = '" + oxUser.LastTime.ToString() + "'");

			strSQL += " WHERE UniqueId = '{";
			strSQL += oxUser.UniqueId.ToString();
			strSQL += "}';";

			return strSQL;
			
		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>Called to locate the object with the specified UniqueId value</summary>
		/// <param name="strValue">The value to search for</param>
		/// <param name="bName">true to check the Name instead of UniqueId</param>
		/// <returns>The object with the specified value</returns>
		public COxUser Find(string strValue, bool bName)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(COxUser O in m_aList)
				{
					if(bName == true)
					{
						if(String.Compare(strValue, O.Name, true) == 0)
							return O;
					}
					else
					{
						if(String.Compare(strValue, O.UniqueId.ToString(), true) == 0)
							return O;
					}

				}// foreach(COxUser O in m_aList)

			}// if(m_aList != null)

			return null;

		}//	public COxUser Find(string strValue , bool bName)

		/// <summary>Called to locate the object with the specified UniqueId value</summary>
		/// <param name="uniqueId">The UniqueId value of the desired object</param>
		/// <returns>The object with the specified UniqueId</returns>
		public COxUser Find(System.Guid uniqueId)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(COxUser O in m_aList)
				{
					if(uniqueId == O.UniqueId)
					{
						return O;
					}
				}
			}

			return null;

		}//	public COxUser Find(System.Guid uniqueId)

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
			return ((CBaseRecord)(new COxUser()));
		}

		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord,bool bSetFields)
		{
			COxUser oxUser = (COxUser)dxRecord;

			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;

			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.UniqueId].Value = oxUser.UniqueId;
					m_dxFields[(int)eFields.Name].Value = oxUser.Name;
					m_dxFields[(int)eFields.LastTime].Value = oxUser.LastTime;
				}
				else
				{
					oxUser.UniqueId = (System.Guid)(m_dxFields[(int)eFields.UniqueId].Value);
					oxUser.Name = m_dxFields[(int)eFields.Name].Value.ToString();
					oxUser.LastTime = (DateTime)(m_dxFields[(int)eFields.LastTime].Value);
				}

				return true;
			}
			catch(OleDbException oleEx)
			{
				FireError("Exchange",this.ExBuilder.Message(CObjectionsDatabase.ERROR_OBJECTIONS_DATABASE_RECORDS_EXCHANGE,TableName,bSetFields),oleEx,GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
				FireError("Exchange",this.ExBuilder.Message(CObjectionsDatabase.ERROR_OBJECTIONS_DATABASE_RECORDS_EXCHANGE,TableName,bSetFields),Ex,GetErrorItems(dxRecord));
			}

			return false;

		}// protected override bool Exchange(CBaseRecord dxRecord,bool bSetFields)

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
		private void SetValue(eFields eField,CDxField dxField)
		{
			switch(eField)
			{
				case eFields.UniqueId:

					dxField.Value = System.Guid.Empty;
					break;

				case eFields.LastTime:

					dxField.Value = System.DateTime.Now;
					break;

				case eFields.Name:

					dxField.Value = "";
					break;

				default:

					Debug.Assert(false,"SetValue() - unknown field identifier - " + eField.ToString());
					break;

			}// switch(eField)

		}// private void SetValue(eFields eField, CDxField dxField)

		#endregion Private Methods

	}//	COxUsers

}// namespace FTI.Trialmax.Database
