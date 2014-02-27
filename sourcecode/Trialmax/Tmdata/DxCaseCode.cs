using System;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class is used to manage the exchange of data between the application and individual records in the PickLists table</summary>
	public class CDxCaseCode : FTI.Trialmax.Database.CBaseRecord
	{
		#region Private Members
		
		/// <summary>Local member bound to TmaxCaseCode property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to ModifiedBy property</summary>
		protected long m_lModifiedBy = 0;
		
		/// <summary>Local member bound to ModifiedOn property</summary>
		protected DateTime m_tsModifiedOn = System.DateTime.Now;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CDxCaseCode() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		public CDxCaseCode(CTmaxCaseCode tmaxCaseCode) : base()
		{
			m_tmaxCaseCode = tmaxCaseCode;
		}
		
		/// <summary>Performs cleanup</summary>
		/// <returns>Null</returns>
		public override CBaseRecord Dispose()
		{
			m_tmaxCaseCode = null;
			
			//	This allows use to dispose and reset the reference in one line of code
			return null;
			
		}// public override void Dispose()
	
		/// <summary>This method is called to get the application case code bound to this exchange object</summary>
		/// <returns>The application case code object bound to this object</returns>
		public FTI.Shared.Trialmax.CTmaxCaseCode GetTmaxCaseCode()
		{
			//	Do we need to allocate the object?
			if(m_tmaxCaseCode == null)
				m_tmaxCaseCode = new CTmaxCaseCode();
				
			return m_tmaxCaseCode;
			
		}// public FTI.Shared.Trialmax.CTmaxCaseCode GetTmaxCaseCode()

		/// <summary>This method is called to set the application case code bound to this exchange object</summary>
		/// <param name="tmaxCaseCode">The application item to be bound to this object</param>
		public void SetTmaxCaseCode(FTI.Shared.Trialmax.CTmaxCaseCode tmaxCaseCode)
		{
			m_tmaxCaseCode = tmaxCaseCode;
			
		}// public void SetTmaxCaseCode(FTI.Shared.Trialmax.CTmaxCaseCode tmaxCaseCode)
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.CaseCode;
		}
		
		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		public override string GetKeyId()
		{
			return this.UniqueId.ToString();
		}
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The database collection that owns this record</summary>
		/// <remarks>This overrides the base class property to cast the collection to the appropriate type</remarks>
		new public CDxCaseCodes Collection
		{
			get { return ((CDxCaseCodes)m_dxCollection); }
			set { m_dxCollection = value; }
		}
		
		/// <summary>The application CaseCode bound to this object</summary>
		public FTI.Shared.Trialmax.CTmaxCaseCode TmaxCaseCode
		{
			get { return GetTmaxCaseCode(); }
			set { SetTmaxCaseCode(value); }
		}
		
		/// <summary>The unique identifier assigned to this code</summary>
		public long UniqueId
		{
			get { return this.TmaxCaseCode.UniqueId; }
			set { this.TmaxCaseCode.UniqueId = value;  }
		}
		
		/// <summary>The unique identifier the pick list bound to this code</summary>
		public long PickListId
		{
			get { return this.TmaxCaseCode.PickListId; }
			set { this.TmaxCaseCode.PickListId = value;  }
		}
		
		/// <summary>The application pick lists collection bound to this case code</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickList
		{
			get { return this.TmaxCaseCode.PickList; }
			set { this.TmaxCaseCode.PickList = value;  }
		}
		
		/// <summary>The name assigned to this code</summary>
		public string Name
		{
			get { return this.TmaxCaseCode.Name; }
			set { this.TmaxCaseCode.Name = value;  }
		}
		
		/// <summary>Enumerated type identifier bound to this code</summary>
		public FTI.Shared.Trialmax.TmaxCodeTypes Type
		{
			get { return this.TmaxCaseCode.Type; }
			set { this.TmaxCaseCode.Type = value;  }
		}
		
		/// <summary>Identifier used to sort the collection</summary>
		public long SortOrder
		{
			get { return this.TmaxCaseCode.SortOrder; }
			set { this.TmaxCaseCode.SortOrder = value;  }
		}
		
		/// <summary>The enumerated coded property identifier</summary>
		public FTI.Shared.Trialmax.TmaxCodedProperties CodedProperty
		{
			get { return this.TmaxCaseCode.CodedProperty; }
			set { this.TmaxCaseCode.CodedProperty = value;  }
		}
		
		/// <summary>True if this code represents a coded property value</summary>
		public bool IsCodedProperty
		{
			get { return this.TmaxCaseCode.IsCodedProperty; }
		}
		
		/// <summary>True to allow multiple instances</summary>
		public bool AllowMultiple
		{
			get { return this.TmaxCaseCode.AllowMultiple; }
			set { this.TmaxCaseCode.AllowMultiple = value;  }
		}
		
		/// <summary>True to hide this code from the user</summary>
		public bool Hidden
		{
			get { return this.TmaxCaseCode.Hidden; }
			set { this.TmaxCaseCode.Hidden = value;  }
		}
		
		/// <summary>Value used to pack and unpack boolean properties for storage in the database</summary>
		public long Attributes
		{
			get { return this.TmaxCaseCode.Attributes; }
			set { this.TmaxCaseCode.Attributes = value;  }
		}
		
		/// <summary>The identifier of the user that last modified the media entry</summary>
		public long ModifiedBy
		{
			get { return m_lModifiedBy; }
			set { m_lModifiedBy = value; }
		}
		
		/// <summary>The date and time the media was last modified</summary>
		public DateTime ModifiedOn
		{
			get { return m_tsModifiedOn; }
			set { m_tsModifiedOn = value; }
		}
		
		#endregion Properties
	
	}// public class CDxCaseCode : FTI.Trialmax.Database.CBaseRecord

	/// <summary>This class is used to manage a ArrayList of CDxCaseCode objects</summary>
	public class CDxCaseCodes : CBaseRecords
	{
		#region Constants
		
		//	Enumerated field (column) identifiers
		public enum eFields
		{
			UniqueId = 0,
			Name,
			Type,
			CodedProperty,
			PickListId,
			SortOrder,
			Attributes,
			SpareNumber,
			SpareText,
			ModifiedBy,
			ModifiedOn,
		}

		public const string TABLE_NAME = "CaseCodes";
		private const string KEY_FIELD_NAME = "UniqueId";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to AutoIdEnabled property</summary>
		private bool m_bAutoIdEnabled = true;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxCaseCodes() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxCaseCodes(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxCaseCode">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxCaseCode Add(CDxCaseCode dxCaseCode)
		{
			CDxCaseCode dxAdded = null;

            dxCaseCode.ModifiedBy = this.Database.GetUserId();
			dxCaseCode.ModifiedOn = System.DateTime.Now;

			if((dxAdded = (CDxCaseCode)(base.Add(dxCaseCode))) != null)
			{
				//	Get the unique id assigned by the database
				if(this.AutoIdEnabled == true)
                    dxAdded.UniqueId = this.Database.GetAutoNumber();
			}
			
			return dxAdded;

		}// public CDxCaseCode Add(CDxCaseCode dxCaseCode)

		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public override bool Update(CBaseRecord dxRecord)
		{
			//	Make sure the user information is updated
            ((CDxCaseCode)dxRecord).ModifiedBy = this.Database.GetUserId();
			((CDxCaseCode)dxRecord).ModifiedOn = System.DateTime.Now;
				
			return base.Update(dxRecord);
			
		}// public override bool Update(CBaseRecord dxRecord)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxCaseCodes Dispose()
		{
			//	Make sure each object in this collection is disposed
			foreach(CDxCaseCode O in this)
			{
				try { O.Dispose(); }
				catch {}
			}
			
			return (CDxCaseCodes)base.Dispose();
		
		}// public new CDxCaseCodes Dispose()

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Object at the specified index</returns>
		public new CDxCaseCode this[int iIndex]
		{
			get { return (CDxCaseCode)base[iIndex]; }
		}

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new CDxCaseCode GetAt(int iIndex)
		{
			return (CDxCaseCode)base.GetAt(iIndex);
		}

        /// <summary>This method is called to populate the collection with codes that reference the specified case code</summary>
        /// <param name="tmaxCaseCode">The case code referenced by the desired codes</param>
        /// <returns>true if successful</returns>
        public bool GetCaseCode(long uniquieId)
        {
            string strSQL = "";
            bool bSuccessful = false;

            if (this.Database == null) return false;
            if (this.Database.IsConnected == false) return false;

                strSQL = GetSQLSearch(uniquieId);

                if (strSQL.Length > 0)
                    bSuccessful = Fill(strSQL);

            return bSuccessful;

        }// public virtual bool Fill(CTmaxPickItem tmaxPickItem)

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxCaseCode	dxCaseCode = (CDxCaseCode)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";

			//	Insert the UniqueId if not being assigned by the database
			if(this.AutoIdEnabled == false)
				strSQL += (eFields.UniqueId.ToString() + ",");
			
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.Type.ToString() + ",");
			strSQL += (eFields.CodedProperty.ToString() + ",");
			strSQL += (eFields.PickListId.ToString() + ",");
			strSQL += (eFields.SortOrder.ToString() + ",");
			strSQL += (eFields.Attributes.ToString());

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += ("," + eFields.ModifiedBy.ToString());
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += ("," + eFields.ModifiedOn.ToString());

			strSQL += ")";

			strSQL += " VALUES(";

			if(this.AutoIdEnabled == false)
				strSQL += ("'" + dxCaseCode.UniqueId.ToString() + "',");
			
			strSQL += ("'" + SQLEncode(dxCaseCode.Name) + "',");
			strSQL += ("'" + ((int)(dxCaseCode.Type)).ToString() + "',");
			strSQL += ("'" + ((int)(dxCaseCode.CodedProperty)).ToString() + "',");
			strSQL += ("'" + dxCaseCode.PickListId.ToString() + "',");
			strSQL += ("'" + dxCaseCode.SortOrder.ToString() + "',");
			strSQL += ("'" + dxCaseCode.Attributes.ToString() + "'");

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += (",'" + dxCaseCode.ModifiedBy.ToString() + "'");
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += (",'" + dxCaseCode.ModifiedOn.ToString() + "'");

			strSQL += ")";

			return strSQL;
		
		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to flush all records belonging to the collection</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLFlush()
		{
			string strSQL = "DELETE * FROM " + m_strTableName;
			return strSQL;
		
		}// public override string GetSQLFlush()
		
		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxCaseCode	dxCaseCode = (CDxCaseCode)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxCaseCode.Name) + "',");
			strSQL += (eFields.Type.ToString() + " = '" + ((int)(dxCaseCode.Type)).ToString() + "',");
			strSQL += (eFields.CodedProperty.ToString() + " = '" + ((int)(dxCaseCode.CodedProperty)).ToString() + "',");
			strSQL += (eFields.PickListId.ToString() + " = '" + dxCaseCode.PickListId.ToString() + "',");
			strSQL += (eFields.SortOrder.ToString() + " = '" + dxCaseCode.SortOrder.ToString() + "',");
			strSQL += (eFields.Attributes.ToString() + " = '" + dxCaseCode.Attributes.ToString() + "'");

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += ("," + eFields.ModifiedBy.ToString() + " = '" + dxCaseCode.ModifiedBy.ToString() + "'");
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += ("," + eFields.ModifiedOn.ToString() + " = '" + dxCaseCode.ModifiedOn.ToString() + "'");

			strSQL += (" WHERE " + this.KeyFieldName + " = ");
			strSQL += dxCaseCode.GetKeyId();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to select the desired records</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string strSQL = "SELECT * FROM " + m_strTableName;
			
			strSQL += " ORDER BY ";
			strSQL += eFields.SortOrder.ToString();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLSelect()

        /// <summary>This method is called to get the SQL statement required to get codes that reference the specified case code</summary>
        /// <param name="tmaxCaseCode">The case code being referenced</param>
        /// <returns>The appropriate SQL statement</returns>
        public string GetSQLSearch(long uniqueId)
        {
            string strSQL = "";

            strSQL = "SELECT * FROM " + m_strTableName;
            strSQL += (" WHERE " + eFields.UniqueId.ToString() + " = " + uniqueId.ToString() + ";");

            return strSQL;

        }// public string GetSQLSearch(CTmaxCaseCode tmaxCaseCode)

		/// <summary>This method is called to get the SQL statement required to create the table</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLCreate()
		{
			string strSQL = "CREATE TABLE " + TABLE_NAME;
			
			strSQL += "(";
			
			strSQL += "UniqueId AUTOINCREMENT, ";
			strSQL += "Name TEXT(255), ";
			strSQL += "Type SHORT, ";
			strSQL += "CodedProperty SHORT, ";
			strSQL += "PickListId LONG, ";
			strSQL += "SortOrder LONG, ";
			strSQL += "Attributes LONG, ";
			strSQL += "SpareNumber LONG, ";
			strSQL += "SpareText TEXT(255), ";
			strSQL += "ModifiedBy LONG, ";
			strSQL += "ModifiedOn DATETIME";

			strSQL += ")";
			
			return strSQL;

		}// public override string GetSQLCreate()
		
		/// <summary>This method is called to get the SQL statement required to create the specified column</summary>
		/// <param name="iColumn">Identifies the desired column (derived class specific)</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLCreateColumn(int iColumn)
		{
			string strSQL = "";

			try
			{
				if(iColumn == ((int)(eFields.ModifiedBy)))
				{
					strSQL = "ALTER TABLE ";
					strSQL += TABLE_NAME;
			
					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.ModifiedBy.ToString() + " ");
					strSQL += "LONG ";
					strSQL += "DEFAULT 0";
				
					strSQL += " ;";

				}
				else if(iColumn == ((int)(eFields.ModifiedOn)))
				{
					strSQL = "ALTER TABLE ";
					strSQL += TABLE_NAME;
			
					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.ModifiedOn.ToString() + " ");
					strSQL += "DATETIME;";

				}
				
			}
			catch
			{
			}
			
			return strSQL;
		
		}// public virtual string GetSQLCreateColumn(int iColumn)
		
		/// <summary>This method is called to get the SQL statement required to enable/disable the autoincrement attribute of the specified column</summary>
		/// <param name="iColumn">Identifies the desired column (derived class specific)</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSetAutoIncrement(int iColumn, bool bEnabled)
		{
			string strSQL = "";
		
			//	The only column we auto-increment in this table is UniqueId
			if(iColumn == ((int)(eFields.UniqueId)))
			{
				//	Format the SQL statement to modify the column				
				strSQL = "ALTER TABLE ";
				strSQL += this.TableName;
			
				strSQL += "\nALTER COLUMN ";
				strSQL += (eFields.UniqueId.ToString() + " ");
				
				if(bEnabled == true)
				{
					//	BOTH OF THESE FAIL BUT IT TURNS OUT WE DON'T NEED IT ANYWAY
					//	JUST LEAVING THE CODE IN PLACE IN CASE I HAVE TO COME BACK TO IT
					
					//strSQL += "AUTOINCREMENT";
					strSQL += "IDENTITY";
				}
				else
				{
					strSQL += "LONG";
					//strSQL += "DEFAULT 0";
				}
				
				// JUST IN CASE ........
				Debug.Assert(false, "SHOULDN'T REACH HERE");
				
				strSQL += " ;";

			}// if(iColumn == ((int)(eFields.UniqueId)))
			
			return strSQL;
				
		}// public override string GetSQLSetAutoIncrement(int iColumn, bool bEnabled)
		
		/// <summary>Called to locate the object with the specified identifier</summary>
		/// <param name="lUniqueId">The id to be located</param>
		/// <returns>The object with the specified identifier</returns>
		public CDxCaseCode Find(long lUniqueId)
		{
			CDxCaseCode dxCaseCode = null;
			
			foreach(CDxCaseCode O in this)
			{
				if(O.UniqueId == lUniqueId)
				{
					dxCaseCode = O;
					break;
				}
				
			}// foreach(CDxCaseCode O in this)
			
			return dxCaseCode;
		
		}// public CDxCaseCode Find(long lUniqueId)
			
		/// <summary>Called to locate the object with the specified name</summary>
		/// <param name="strName">The name to be located</param>
		/// <returns>The object with the specified name</returns>
		public CDxCaseCode Find(string strName)
		{
			CDxCaseCode dxCaseCode = null;
			
			foreach(CDxCaseCode O in this)
			{
				if(strName == O.Name)
				{
					dxCaseCode = O;
					break;
				}
				
			}// foreach(CDxCaseCode O in this)
			
			return dxCaseCode;
		
		}// public CDxCaseCode Find(string strName)
			
		/// <summary>Turns AUTOINCREMENT on and off on the UniqueId column</summary>
		/// <returns>True if successful</returns>
		public bool SetAutoIdEnabled(bool bEnabled)
		{
			//	Has the value changed?
			if(bEnabled == m_bAutoIdEnabled) return true;
			
			//	NOTE:	It turns out with Access that we don't have to modify the
			//			column to be able to set the UniqueId value. All we have to
			//			do is include the value in the SQL INSERT statement
			
			//	Set the AUTOINCREMENT feature of the UniqueId column
//			if(SetAutoIncrement((int)(eFields.UniqueId), bEnabled) == true)
//			{
				m_bAutoIdEnabled = bEnabled;
				return true;
//			}
//			else
//			{
//				return false;
//			}
			
		}// public bool SetAutoIdEnabled(bool bEnabled)

		/// <summary>Called to add the columns for ModifiedOn and ModifiedBy</summary>
		/// <returns>True if successful</returns>
		public bool AddModifiedColumns()
		{
            if(this.Database != null)
			{
				CreateColumn((int)(eFields.ModifiedBy), true);
				m_dxFields[(int)(eFields.ModifiedBy)].Index = GetColumnIndex(TABLE_NAME, eFields.ModifiedBy.ToString());

				CreateColumn((int)(eFields.ModifiedOn), true);
				m_dxFields[(int)(eFields.ModifiedOn)].Index = GetColumnIndex(TABLE_NAME, eFields.ModifiedOn.ToString());
			}
			else
			{
				m_dxFields[(int)(eFields.ModifiedBy)].Index = -1;
				m_dxFields[(int)(eFields.ModifiedOn)].Index = -1;
			}
			
			return ((m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0) && (m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0));
			
		}// public bool AddModifiedColumns()
			
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected override string[] GetFieldNames()
		{
			return Enum.GetNames(typeof(eFields));
		}
		
		/// <summary>This function is called to populate the Fields collection</summary>
		protected override void SetFields()
		{
			//	Do the base class processing first
			base.SetFields();
			
			//	This prevents attempts to retrieve the spare values
			if((m_dxFields != null) && ((int)eFields.SpareNumber < m_dxFields.Count))
				m_dxFields[(int)(eFields.SpareNumber)].Index = -1;
			if((m_dxFields != null) && ((int)eFields.SpareText < m_dxFields.Count))
				m_dxFields[(int)(eFields.SpareText)].Index = -1;
			
			//	Make sure the ModifiedOn and ModifiedBy columns exist
			AddModifiedColumns();
			
		}// protected override void SetFields()
		
		/// <summary>This function is called to set the database interface</summary>
		protected void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		{
			//	Do the base class processing first
			base.SetDatabase(tmaxDatabase as CBaseDatabase);
			
			//	Make sure the ModifiedOn and ModifiedBy columns exist
			AddModifiedColumns();
			
		}// protected override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		
		/// <summary>This function is called to get a new record object</summary>
		/// <returns>A new object of the collection type</returns>
		protected override CBaseRecord GetNewRecord()
		{
			return ((CBaseRecord)(new CDxCaseCode()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxCaseCode	dxCaseCode = (CDxCaseCode)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.UniqueId].Value  = dxCaseCode.UniqueId;
					m_dxFields[(int)eFields.Name].Value = dxCaseCode.Name;
					m_dxFields[(int)eFields.Type].Value = dxCaseCode.Type;
					m_dxFields[(int)eFields.CodedProperty].Value = dxCaseCode.CodedProperty;
					m_dxFields[(int)eFields.PickListId].Value = dxCaseCode.PickListId;
					m_dxFields[(int)eFields.SortOrder].Value = dxCaseCode.SortOrder;
					m_dxFields[(int)eFields.Attributes].Value = dxCaseCode.Attributes;
				}
				else
				{
					dxCaseCode.UniqueId = (int)(m_dxFields[(int)eFields.UniqueId].Value);
					dxCaseCode.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxCaseCode.Type = (FTI.Shared.Trialmax.TmaxCodeTypes)((short)(m_dxFields[(int)eFields.Type].Value));
					dxCaseCode.CodedProperty = (FTI.Shared.Trialmax.TmaxCodedProperties)((short)(m_dxFields[(int)eFields.CodedProperty].Value));
					dxCaseCode.PickListId = (int)(m_dxFields[(int)eFields.PickListId].Value);
					dxCaseCode.SortOrder = (int)(m_dxFields[(int)eFields.SortOrder].Value);
					dxCaseCode.Attributes = (int)(m_dxFields[(int)eFields.Attributes].Value);
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
			
		}// protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected override void SetNames()
		{
			m_strTableName = TABLE_NAME;
			m_strKeyFieldName = KEY_FIELD_NAME;
		}
		
		/// <summary>This method will set the default value for the specified field</summary>
		/// <param name="eField">The enumerated field identifier</param>
		/// <param name="dxField">The field object to be set</param>
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.UniqueId:
				case eFields.PickListId:
				case eFields.SortOrder:
				case eFields.Attributes:
				case eFields.SpareNumber:
				case eFields.ModifiedBy:
				
					dxField.Value = 0;
					break;
					
				case eFields.Type:
				
					dxField.Value = TmaxCodeTypes.Unknown;
					break;
					
				case eFields.CodedProperty:
				
					dxField.Value = TmaxCodedProperties.Invalid;
					break;
					
				case eFields.Name:
				case eFields.SpareText:
				
					dxField.Value = "";
					break;
					
				case eFields.ModifiedOn:
				
					dxField.Value = System.DateTime.Now;
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>True to automatically assign the UniqueId value for each record</summary>
		public bool AutoIdEnabled
		{
			get { return m_bAutoIdEnabled; }
			set { SetAutoIdEnabled(value); }
		}

        /// <summary>The active database</summary>
        new public CTmaxCaseDatabase Database
        {
            get { return (CTmaxCaseDatabase)(base.Database); }
            set { SetDatabase(value); }
        }

        #endregion Properties
		
	}//	public class CDxCaseCodes : CBaseRecords
		
}// namespace FTI.Trialmax.Database
