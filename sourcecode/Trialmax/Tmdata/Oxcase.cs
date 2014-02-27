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
	public class COxCase : FTI.Trialmax.Database.CBaseRecord, ITmaxBaseObjectionRecord
	{
		#region Private Members

		/// <summary>Local member bound to TmaxCase property</summary>
		private FTI.Shared.Trialmax.CTmaxCase m_tmaxCase = null;

		/// <summary>Local member bound to CreatedOn property</summary>
		private DateTime m_dtCreatedOn = System.DateTime.Now;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public COxCase() : base()
		{
		}

		/// <summary>This method retrieves the identifier assigned by the database</summary>
		///	<returns>The record id</returns>
		string ITmaxBaseObjectionRecord.GetUniqueId()
		{
			//	Expose the CaseId to the application
			return this.UniqueId;
		}

		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		/// <remarks>This member MUST be overridden by the derived class</remarks>
		public override string GetKeyId()
		{
			return ("{" + this.UniqueId + "}");
		}
		
		/// <summary>This method retrieves the text used to display this record</summary>
		///	<returns>The display text</returns>
		string ITmaxBaseObjectionRecord.GetText()
		{
			if(this.Name.Length > 0)
				return this.Name;
			else
				return this.UniqueId;
		}

		/// <summary>This method is called to get the application case object</summary>
		/// <returns>The application object</returns>
		public CTmaxCase GetTmaxCase()
		{
			//	Do we need to allocate the object?
			if(m_tmaxCase == null)
				m_tmaxCase = new CTmaxCase();

			return m_tmaxCase;

		}// public CTmaxCase GetTmaxCase()

		#endregion Public Methods

		#region Properties

		//	The application wrapper object for the case descriptor
		public CTmaxCase TmaxCase
		{
			get { return GetTmaxCase(); }
			set { m_tmaxCase = value; }
		}

		//	The Name assigned to the case
		public string Name
		{
			get { return this.TmaxCase.Name; }
			set { this.TmaxCase.Name = value; }
		}

		//	The short name assigned to the case
		public string ShortName
		{
			get { return this.TmaxCase.ShortName; }
			set { this.TmaxCase.ShortName = value; }
		}

		//	GUID of master database
		public string UniqueId
		{
			get { return this.TmaxCase.UniqueId; }
			set { this.TmaxCase.UniqueId = value; }
		}

		//	Trialmax version identifier
		public string Version
		{
			get { return this.TmaxCase.Version; }
			set { this.TmaxCase.Version = value; }
		}

		/// <summary>The date the record was created</summary>
		public System.DateTime CreatedOn
		{
			get { return m_dtCreatedOn; }
			set { m_dtCreatedOn = value; }
		}

		#endregion Properties

	}// public class COxCase : FTI.Trialmax.Database.CBaseRecord

	/// <summary>This class is used to manage a ArrayList of COxCase objects</summary>
	public class COxCases : CBaseRecords
	{
		public enum eFields
		{
			TmaxManagerId,
			Name,
			Version,
			CreatedOn,
		}

		public const string TABLE_NAME = "Cases";

		#region Public Members

		/// <summary>Default constructor</summary>
		public COxCases() : base()
		{
			m_strKeyFieldName = eFields.TmaxManagerId.ToString();
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public COxCases(CObjectionsDatabase tmaxDatabase) : base(tmaxDatabase)
		{
			m_strKeyFieldName = eFields.TmaxManagerId.ToString();
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="oxCase">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public COxCase Add(COxCase oxCase)
		{
			COxCase	oxAdded = null;
			
			//	Make sure this id does not already exist in the database
			//
			//	NOTE:	This could happen if the user changes the name of the
			//			original case
			if(oxCase.UniqueId.Length > 0)
			{
				if(this.Find(oxCase.UniqueId, false) != null)
					oxCase.UniqueId = ""; // Force a new id
			}
			
			//	Create an id if one not provided
			if(oxCase.UniqueId.Length == 0)
				oxCase.UniqueId = System.Guid.NewGuid().ToString();

			//	Must supply a version identifier
			if(oxCase.Version.Length == 0)
				oxCase.Version = "0.0.0";
				
			oxAdded = (COxCase)(base.Add(oxCase));
			
			return oxAdded;

		}// public COxCase Add(COxCase oxCase)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new COxCases Dispose()
		{
			return (COxCases)base.Dispose();

		}// public new COxCases Dispose()

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new COxCase this[int iIndex]
		{
			get { return (COxCase)base[iIndex]; }

		}// public new COxCase this[int iIndex]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new COxCase GetAt(int iIndex)
		{
			return (COxCase)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			COxCase oxCase = (COxCase)dxRecord;
			string	strSQL = "INSERT INTO " + TableName + "(";

			strSQL += (eFields.TmaxManagerId.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.Version.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(oxCase.UniqueId) + "',");
			strSQL += ("'" + SQLEncode(oxCase.Name) + "',");
			strSQL += ("'" + SQLEncode(oxCase.Version) + "',");
			strSQL += ("'" + oxCase.CreatedOn.ToString() + "')");

			return strSQL;

		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			COxCase oxCase = (COxCase)dxRecord;
			string	strSQL = "UPDATE " + TableName + " SET ";

			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(oxCase.Name) + "',");
			strSQL += (eFields.Version.ToString() + " = '" + SQLEncode(oxCase.Version) + "'");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + oxCase.CreatedOn.ToString() + "'");

			strSQL += " WHERE TmaxManagerId = '";
			strSQL += oxCase.UniqueId;
			strSQL += "';";

			return strSQL;

		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>Called to locate the object with the specified CaseId or Name value</summary>
		/// <param name="strSearch">The ID or Name to search for</param>
		/// <returns>The object with the specified search value</returns>
		public COxCase Find(string strSearch, bool bName)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(COxCase O in m_aList)
				{
					if(bName == true)
					{
						if(String.Compare(strSearch, O.Name, true) == 0)
							return O;
					}
					else
					{
						if(String.Compare(strSearch, O.UniqueId, true) == 0)
							return O;
					}

				}// foreach(COxCase O in m_aList)

			}// if(m_aList != null)

			return null;

		}//	public COxCase Find(string strSearch, bool bName)

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
			return ((CBaseRecord)(new COxCase()));
		}

		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord,bool bSetFields)
		{
			COxCase oxCase = (COxCase)dxRecord;

			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;

			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.TmaxManagerId].Value = oxCase.UniqueId;
					m_dxFields[(int)eFields.Name].Value = oxCase.Name;
					m_dxFields[(int)eFields.Version].Value = oxCase.Version;
					m_dxFields[(int)eFields.CreatedOn].Value = oxCase.CreatedOn;
				}
				else
				{
					oxCase.UniqueId = (string)(m_dxFields[(int)eFields.TmaxManagerId].Value);
					oxCase.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					oxCase.Version = (string)(m_dxFields[(int)eFields.Version].Value);
					oxCase.CreatedOn = (System.DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
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
				case eFields.TmaxManagerId:
				case eFields.Name:
				case eFields.Version:

					dxField.Value = "";
					break;

				case eFields.CreatedOn:

					dxField.Value = System.DateTime.Now;
					break;

				default:

					Debug.Assert(false,"SetValue() - unknown field identifier - " + eField.ToString());
					break;

			}// switch(eField)

		}// private void SetValue(eFields eField, CDxField dxField)

		#endregion Private Methods

	}//	public class COxCases : CBaseRecords

}// namespace FTI.Trialmax.Database
