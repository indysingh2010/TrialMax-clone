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
	/// <summary>This class encapsulates the information used to define a Trialmax case</summary>
	public class CDxDetail : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Private Members

		/// <summary>Local member bound to TmaxCase property</summary>
		private FTI.Shared.Trialmax.CTmaxCase m_tmaxCase = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CDxDetail() : base()
		{
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
		new public string Name
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

		#endregion Properties
	
	}// class CDxDetail

	/// <summary>
	/// This class is used to manage a ArrayList of CDxDetail objects
	/// </summary>
	public class CDxDetails : CDxMediaRecords
	{
		#region Constants

		public enum eFields
		{
			AutoId = 0,
			MasterId,
			DbMajor,
			DbMinor,
			DbBuild,
			Name,
			Description,
			CreatedBy,
			CreatedOn,
			ShortName,
		}

		public const string TABLE_NAME = "Details";

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Private member bound to AddedShortName property</summary>
		private bool m_bAddedShortName = false;

		#endregion Constants

		#region Public Members

		/// <summary>Default constructor</summary>
		public CDxDetails() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxDetails(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxDetail">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxDetail Add(CDxDetail dxDetail)
		{
			return (CDxDetail)base.Add(dxDetail);
			
		}// Add(CDxDetail dxDetail)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxDetails Dispose()
		{
			return (CDxDetails)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <param name="bBarcode">true to check the barcode id instead of the auto id</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxDetail Find(long lAutoId, bool bBarcode)
		{
			return (CDxDetail)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxDetail Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxDetail this[int iIndex]
		{
			get
			{
				return (CDxDetail)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxDetail GetAt(int iIndex)
		{
			return (CDxDetail)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxDetail	dxDetail = (CDxDetail)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.MasterId.ToString() + ",");
			strSQL += (eFields.DbMajor.ToString() + ",");
			strSQL += (eFields.DbMinor.ToString() + ",");
			strSQL += (eFields.DbBuild.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.Description.ToString() + ",");
			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString());

			if(m_dxFields[(int)(eFields.ShortName)].Index >= 0)
				strSQL += ("," + eFields.ShortName.ToString());

			strSQL += ")";

			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(dxDetail.UniqueId) + "',");
			strSQL += ("'" + dxDetail.TmaxCase.VerMajor.ToString() + "',");
			strSQL += ("'" + dxDetail.TmaxCase.VerMinor.ToString() + "',");
			strSQL += ("'" + dxDetail.TmaxCase.VerQEF.ToString() + "',");
			strSQL += ("'" + SQLEncode(dxDetail.Name) + "',");
			strSQL += ("'" + SQLEncode(dxDetail.Description) + "',");
			strSQL += ("'" + dxDetail.CreatedBy.ToString() + "',");
			strSQL += ("'" + dxDetail.CreatedOn.ToString() + "'");

			if(m_dxFields[(int)(eFields.ShortName)].Index >= 0)
				strSQL += (",'" + dxDetail.ShortName + "'");

			strSQL += ")";

			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxDetail	dxDetail = (CDxDetail)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.MasterId.ToString() + " = '" + SQLEncode(dxDetail.UniqueId) + "',");
			strSQL += (eFields.DbMajor.ToString() + " = '" + dxDetail.TmaxCase.VerMajor.ToString() + "',");
			strSQL += (eFields.DbMinor.ToString() + " = '" + dxDetail.TmaxCase.VerMinor.ToString() + "',");
			strSQL += (eFields.DbBuild.ToString() + " = '" + dxDetail.TmaxCase.VerQEF.ToString() + "',");
			strSQL += (eFields.Name.ToString() + " = '" + dxDetail.Name + "',");
			strSQL += (eFields.Description.ToString() + " = '" + dxDetail.Description + "',");
			strSQL += (eFields.CreatedBy.ToString() + " = '" + dxDetail.CreatedBy.ToString() + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + dxDetail.CreatedOn.ToString() + "'");

			if(m_dxFields[(int)(eFields.ShortName)].Index >= 0)
				strSQL += ("," + eFields.ShortName.ToString() + " = '" + dxDetail.ShortName + "'");

			strSQL += " WHERE AutoId = ";
			strSQL += dxDetail.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		}

		/// <summary>This method is called to get the SQL statement required to create the specified column</summary>
		/// <param name="iColumn">Identifies the desired column (derived class specific)</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLCreateColumn(int iColumn)
		{
			string strSQL = "";

			try
			{
				if(iColumn == ((int)(eFields.ShortName)))
				{
					strSQL = "ALTER TABLE ";
					strSQL += TABLE_NAME;

					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.ShortName.ToString() + " ");
					strSQL += "TEXT(255)";

					strSQL += " ;";

				}

			}
			catch
			{
			}

			return strSQL;

		}// public virtual string GetSQLCreateColumn(int iColumn)

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
			return ((CBaseRecord)(new CDxDetail()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxDetail dxDetail = (CDxDetail)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxDetail.AutoId;
					m_dxFields[(int)eFields.MasterId].Value = dxDetail.UniqueId;
					m_dxFields[(int)eFields.DbMajor].Value = (long)(dxDetail.TmaxCase.VerMajor);
					m_dxFields[(int)eFields.DbMinor].Value = (long)(dxDetail.TmaxCase.VerMinor);
					m_dxFields[(int)eFields.DbBuild].Value = (long)(dxDetail.TmaxCase.VerQEF);
					m_dxFields[(int)eFields.Name].Value = dxDetail.Name;
					m_dxFields[(int)eFields.ShortName].Value = dxDetail.ShortName;
					m_dxFields[(int)eFields.Description].Value = dxDetail.Description;
					m_dxFields[(int)eFields.CreatedBy].Value = dxDetail.CreatedBy;
					m_dxFields[(int)eFields.CreatedOn].Value = dxDetail.CreatedOn;
				}
				else
				{
					dxDetail.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxDetail.UniqueId = (string)(m_dxFields[(int)eFields.MasterId].Value);
					dxDetail.TmaxCase.VerMajor = (int)(m_dxFields[(int)eFields.DbMajor].Value);
					dxDetail.TmaxCase.VerMinor = (int)(m_dxFields[(int)eFields.DbMinor].Value);
					dxDetail.TmaxCase.VerQEF = (int)(m_dxFields[(int)eFields.DbBuild].Value);
					dxDetail.Name = m_dxFields[(int)eFields.Name].Value.ToString();
					dxDetail.ShortName = m_dxFields[(int)eFields.ShortName].Value.ToString();
					dxDetail.Description = m_dxFields[(int)eFields.Description].Value.ToString();
					dxDetail.CreatedBy = (int)(m_dxFields[(int)eFields.CreatedBy].Value);
					dxDetail.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
				
					//	Just in case
					dxDetail.TmaxCase.VerBuild = 0;
					dxDetail.Version = "";
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

		/// <summary>This function is called to populate the Fields collection</summary>
		protected override void SetFields()
		{
			//	Do the base class processing first
			base.SetFields();

			//	Make sure the ShortName column exist
			AddShortNameColumn();

		}// protected override void SetFields()

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
				case eFields.MasterId:
				case eFields.DbMajor:
				case eFields.DbMinor:
				case eFields.DbBuild:
				case eFields.CreatedBy:
				
					dxField.Value = 0;
					break;
					
				case eFields.Name:
				case eFields.Description:
				
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

		/// <summary>Called to add the column to store the Short Name assigned to the case</summary>
		/// <returns>True if successful</returns>
		public bool AddShortNameColumn()
		{
			m_bAddedShortName = false;
			
			if(this.Database != null)
			{
				//	Does the column already exists in the database?
				m_dxFields[(int)(eFields.ShortName)].Index = GetColumnIndex(TABLE_NAME, eFields.ShortName.ToString());
				if(m_dxFields[(int)(eFields.ShortName)].Index < 0)
				{
					CreateColumn((int)(eFields.ShortName), true);
					m_dxFields[(int)(eFields.ShortName)].Index = GetColumnIndex(TABLE_NAME, eFields.ShortName.ToString());
					
					if(m_dxFields[(int)(eFields.ShortName)].Index >= 0)
						m_bAddedShortName = true;
				}
			
			}
			else
			{
				m_dxFields[(int)(eFields.ShortName)].Index = -1;
			}

			return (m_dxFields[(int)(eFields.ShortName)].Index >= 0);

		}// public bool AddShortNameColumn()

		#endregion Private Methods
		
		#region Properties

		/// <summary>True if short name column was added when the record set connects to the database</summary>
		public bool AddedShortName
		{
			get { return m_bAddedShortName; }
		}
		
		#endregion Properties

	}//	CDxDetails
		
}// namespace FTI.Trialmax.Database
