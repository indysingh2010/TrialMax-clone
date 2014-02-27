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
	/// <summary>This class is used to define a record in the Rulings table of the Objections database</summary>
	public class COxRuling : FTI.Trialmax.Database.CBaseRecord, ITmaxBaseObjectionRecord
	{
		#region Private Members

		/// <summary>Local member bound to UniqueId property</summary>
		private System.Guid m_guidUniqueId = System.Guid.Empty;

		/// <summary>Local member bound to Label property</summary>
		private string m_strLabel = "";

		/// <summary>Local member bound to CreatedOn property</summary>
		private DateTime m_dtCreatedOn = System.DateTime.Now;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public COxRuling() : base()
		{
		}

		/// <summary>This method retrieves the identifier assigned by the database</summary>
		///	<returns>The record id</returns>
		string ITmaxBaseObjectionRecord.GetUniqueId()
		{
			return this.UniqueId.ToString();
		}

		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		/// <remarks>This member MUST be overridden by the derived class</remarks>
		public override string GetKeyId()
		{
			return ("{" + this.UniqueId.ToString() + "}");
		}

		/// <summary>This method retrieves the text used to display this record</summary>
		///	<returns>The display text</returns>
		string ITmaxBaseObjectionRecord.GetText()
		{
			return this.Label;
		}

		/// <summary>Overridden base class member to provide default text representation</summary>
		///	<returns>The default text</returns>
		public override string ToString()
		{
			return this.Label;
		}

		#endregion Public Methods

		#region Properties

		/// <summary>The unique identifier assigned by the database</summary>
		public System.Guid UniqueId
		{
			get { return m_guidUniqueId; }
			set { m_guidUniqueId = value; }
		}

		/// <summary>The type description</summary>
		public string Label
		{
			get { return m_strLabel; }
			set { m_strLabel = value; }
		}


		/// <summary>The date the record was created</summary>
		public System.DateTime CreatedOn
		{
			get { return m_dtCreatedOn; }
			set { m_dtCreatedOn = value; }
		}

		#endregion Properties

	}// public class COxRuling : FTI.Trialmax.Database.CBaseRecord

	/// <summary>This class is used to manage a ArrayList of COxRuling objects</summary>
	public class COxRulings : CBaseRecords
	{
		public enum eFields
		{
			UniqueId = 0,
			Label,
			CreatedOn,
		}

		public const string TABLE_NAME = "Rulings";

		#region Public Members

		/// <summary>Default constructor</summary>
		public COxRulings()	: base()
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public COxRulings(CObjectionsDatabase tmaxDatabase)	: base(tmaxDatabase)
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="oxRuling">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public COxRuling Add(COxRuling oxRuling)
		{
			COxRuling oxAdded = null;

			if((oxAdded = (COxRuling)base.Add(oxRuling)) != null)
			{
				oxAdded.UniqueId = this.Database.GetAutoGuid(TABLE_NAME);
			}

			return oxAdded;

		}// public COxRuling Add(COxRuling oxRuling)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new COxRulings Dispose()
		{
			return (COxRulings)base.Dispose();

		}// public new COxRulings Dispose()

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new COxRuling this[int iIndex]
		{
			get { return (COxRuling)base[iIndex]; }

		}// public new COxRuling this[int iIndex]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new COxRuling GetAt(int iIndex)
		{
			return (COxRuling)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to select the desired records</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string strSQL = "SELECT * FROM " + m_strTableName;

			strSQL += " ORDER BY ";
			strSQL += eFields.CreatedOn.ToString();
			strSQL += ";";

			return strSQL;

		}// public override string GetSQLSelect()
		
		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			COxRuling oxRuling = (COxRuling)dxRecord;
			string strSQL = "INSERT INTO " + TableName + "(";

			strSQL += (eFields.Label.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(oxRuling.Label) + "',");
			strSQL += ("'" + oxRuling.CreatedOn.ToString() + "')");

			return strSQL;

		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			COxRuling oxRuling = (COxRuling)dxRecord;
			string strSQL = "UPDATE " + TableName + " SET ";

			strSQL += (eFields.Label.ToString() + " = '" + SQLEncode(oxRuling.Label) + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + oxRuling.CreatedOn.ToString() + "'");

			strSQL += " WHERE UniqueId = '{";
			strSQL += oxRuling.UniqueId.ToString();
			strSQL += "}';";

			return strSQL;

		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>Called to locate the object with the specified UniqueId value</summary>
		/// <param name="uniqueId">The UniqueId value of the desired object</param>
		/// <returns>The object with the specified UniqueId</returns>
		public COxRuling Find(System.Guid uniqueId)
		{
			return Find(uniqueId.ToString(), false);
		}

		/// <summary>Called to locate the object with the specified UniqueId value</summary>
		/// <param name="strValue">The value to search for</param>
		/// <param name="bLabel">true to check the Label instead of UniqueId</param>
		/// <returns>The object with the specified value</returns>
		public COxRuling Find(string strValue, bool bLabel)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(COxRuling O in m_aList)
				{
					if(bLabel == true)
					{
						if(String.Compare(strValue, O.Label, true) == 0)
							return O;
					}
					else
					{
						if(String.Compare(strValue, O.UniqueId.ToString(), true) == 0)
							return O;
					}

				}// foreach(COxRuling O in m_aList)

			}// if(m_aList != null)

			return null;

		}//	public COxRuling Find(string strValue , bool bLabel)

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
			return ((CBaseRecord)(new COxRuling()));
		}

		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			COxRuling oxRuling = (COxRuling)dxRecord;

			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;

			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.UniqueId].Value = oxRuling.UniqueId;
					m_dxFields[(int)eFields.Label].Value = oxRuling.Label;
					m_dxFields[(int)eFields.CreatedOn].Value = oxRuling.CreatedOn;
				}
				else
				{
					oxRuling.UniqueId = (System.Guid)(m_dxFields[(int)eFields.UniqueId].Value);
					oxRuling.Label = (string)(m_dxFields[(int)eFields.Label].Value);
					oxRuling.CreatedOn = (System.DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
				}

				return true;
			}
			catch(OleDbException oleEx)
			{
				FireError("Exchange", this.ExBuilder.Message(CObjectionsDatabase.ERROR_OBJECTIONS_DATABASE_RECORDS_EXCHANGE, TableName, bSetFields), oleEx, GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
				FireError("Exchange", this.ExBuilder.Message(CObjectionsDatabase.ERROR_OBJECTIONS_DATABASE_RECORDS_EXCHANGE, TableName, bSetFields), Ex, GetErrorItems(dxRecord));
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
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.UniqueId:

					dxField.Value = System.Guid.Empty;
					break;

				case eFields.Label:

					dxField.Value = "";
					break;

				case eFields.CreatedOn:

					dxField.Value = System.DateTime.Now;
					break;

				default:

					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;

			}// switch(eField)

		}// private void SetValue(eFields eField, CDxField dxField)

		#endregion Private Methods

	}//	public class COxRulings : CBaseRecords

}// namespace FTI.Trialmax.Database
