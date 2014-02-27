using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

namespace FTI.Trialmax.Database
{
	/// <summary>This class is used to define a record in the Versions table of the Objections database</summary>
	public class COxVersion : FTI.Trialmax.Database.CBaseRecord
	{
		#region Private Members

		/// <summary>Local member bound to VerMajor property</summary>
		private long m_lVerMajor = 0;

		/// <summary>Local member bound to VerMinor property</summary>
		private long m_lVerMinor = 0;

		/// <summary>Local member bound to VerBuild property</summary>
		private long m_lVerQEF = 0;

		/// <summary>Local member bound to VerBuild property</summary>
		private long m_lVerBuild = 0;

		/// <summary>Local member bound to UniqueId property</summary>
		private string m_strUniqueId = "";

		/// <summary>Local member bound to CaseId property</summary>
		private string m_strCaseId = "";

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public COxVersion() : base()
		{
		}

		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		/// <remarks>This member MUST be overridden by the derived class</remarks>
		public override string GetKeyId()
		{
			return ("{" + this.UniqueId.ToString() + "}");
		}

		#endregion Public Methods

		#region Properties

		/// <summary>The database's major version identifier</summary>
		public long VerMajor
		{
			get { return m_lVerMajor; }
			set { m_lVerMajor = value; }
		}

		/// <summary>The database's minor version identifier</summary>
		public long VerMinor
		{
			get { return m_lVerMinor; }
			set { m_lVerMinor = value; }
		}

		/// <summary>The database's QEF identifier</summary>
		public long VerQEF
		{
			get { return m_lVerQEF; }
			set { m_lVerQEF = value; }
		}

		/// <summary>The database's Build identifier</summary>
		public long VerBuild
		{
			get { return m_lVerBuild; }
			set { m_lVerBuild = value; }
		}

		/// <summary>The unique identifier for this database</summary>
		public string UniqueId
		{
			get { return m_strUniqueId; }
			set { m_strUniqueId = value; }
		}

		/// <summary>The unique identifier assigned to the case that created this database</summary>
		public string CaseId
		{
			get { return m_strCaseId; }
			set { m_strCaseId = value; }
		}

		#endregion Properties

	}// class COxVersion

	/// <summary>This class is used to manage a ArrayList of COxVersion objects</summary>
	public class COxVersions:CBaseRecords
	{
		public enum eFields
		{
			UniqueId = 0,
			VerMajor,
			VerMinor,
			VerQEF,
			VerBuild,
		}

		public const string TABLE_NAME = "Versions";

		#region Public Members

		/// <summary>Default constructor</summary>
		public COxVersions() : base()
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public COxVersions(CObjectionsDatabase tmaxDatabase) : base(tmaxDatabase)
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="oxVersion">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public COxVersion Add(COxVersion oxVersion)
		{
			return (COxVersion)base.Add(oxVersion);

		}// Add(COxVersion oxVersion)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new COxVersions Dispose()
		{
			return (COxVersions)base.Dispose();

		}// Dispose()

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new COxVersion this[int iIndex]
		{
			get
			{
				return (COxVersion)base[iIndex];
			}
		}

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new COxVersion GetAt(int iIndex)
		{
			return (COxVersion)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			COxVersion oxVersion = (COxVersion)dxRecord;
			string strSQL = "INSERT INTO " + TableName + "(";

			strSQL += (eFields.UniqueId.ToString() + ",");
			strSQL += (eFields.VerMajor.ToString() + ",");
			strSQL += (eFields.VerMinor.ToString() + ",");
			strSQL += (eFields.VerQEF.ToString() + ",");
			strSQL += (eFields.VerBuild.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(oxVersion.UniqueId) + "',");
			strSQL += ("'" + oxVersion.VerMajor.ToString() + "',");
			strSQL += ("'" + oxVersion.VerMinor.ToString() + "',");
			strSQL += ("'" + oxVersion.VerQEF.ToString() + "',");
			strSQL += ("'" + oxVersion.VerBuild.ToString() + "')");

			return strSQL;
			
		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			COxVersion oxVersion = (COxVersion)dxRecord;
			string strSQL = "UPDATE " + TableName + " SET ";

			strSQL += (eFields.VerMajor.ToString() + " = '" + oxVersion.VerMajor.ToString() + "',");
			strSQL += (eFields.VerMinor.ToString() + " = '" + oxVersion.VerMinor.ToString() + "',");
			strSQL += (eFields.VerQEF.ToString() + " = '" + oxVersion.VerQEF.ToString() + "',");
			strSQL += (eFields.VerBuild.ToString() + " = '" + oxVersion.VerBuild.ToString() + "'");

			strSQL += " WHERE UniqueId = ";
			strSQL += oxVersion.UniqueId;
			strSQL += ";";

			return strSQL;

		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

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
			return ((CBaseRecord)(new COxVersion()));
		}

		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord,bool bSetFields)
		{
			COxVersion oxVersion = (COxVersion)dxRecord;

			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;

			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.UniqueId].Value = oxVersion.UniqueId;
					m_dxFields[(int)eFields.VerMajor].Value = oxVersion.VerMajor;
					m_dxFields[(int)eFields.VerMinor].Value = oxVersion.VerMinor;
					m_dxFields[(int)eFields.VerQEF].Value = oxVersion.VerQEF;
					m_dxFields[(int)eFields.VerBuild].Value = oxVersion.VerBuild;
				}
				else
				{
					oxVersion.UniqueId = (string)(m_dxFields[(int)eFields.UniqueId].Value);
					oxVersion.VerMajor = (int)(m_dxFields[(int)eFields.VerMajor].Value);
					oxVersion.VerMinor = (int)(m_dxFields[(int)eFields.VerMinor].Value);
					oxVersion.VerQEF = (int)(m_dxFields[(int)eFields.VerQEF].Value);
					oxVersion.VerBuild = (int)(m_dxFields[(int)eFields.VerBuild].Value);
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
				case eFields.VerMajor:
				case eFields.VerMinor:
				case eFields.VerQEF:
				case eFields.VerBuild:

					dxField.Value = 0;
					break;

				case eFields.UniqueId:

					dxField.Value = "";
					break;

				default:

					Debug.Assert(false,"SetValue() - unknown field identifier - " + eField.ToString());
					break;

			}// switch(eField)

		}// private void SetValue(eFields eField, CDxField dxField)

		#endregion Private Methods

	}//	COxVersions

}// namespace FTI.Trialmax.Database
