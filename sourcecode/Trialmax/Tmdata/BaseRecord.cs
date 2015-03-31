using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.Xml;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This is the base class for all record exchange classes</summary>
	public class CBaseRecord : FTI.Shared.Trialmax.ITmaxSortable, FTI.Shared.Trialmax.ITmaxBaseRecord
	{
		#region Protected Members
		
		/// <summary>Local member bound to Collection property</summary>
		protected CBaseRecords m_dxCollection = null;

		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CBaseRecord()
		{
		}
	
		/// <summary>Constructor</summary>
		public CBaseRecord(CBaseRecords dxCollection)
		{
			m_dxCollection = dxCollection;
		}
	
		/// <summary>Performs cleanup</summary>
		public virtual CBaseRecord Dispose()
		{
			//	This allows use to dispose and reset the reference in one line of code
			return null;
		}
	
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public virtual TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Unknown;
		}
		
		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		/// <remarks>This member MUST be overridden by the derived class</remarks>
		public virtual string GetKeyId()
		{
			return "";
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxPickItem, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CBaseRecord)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="dxRecord">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier (specific to derived class)</param>
		/// <returns>-1 if this result less than, 0 if equal, 1 if greater than</returns>
		public virtual int Compare(CBaseRecord dxRecord, long lMode)
		{
			//	Are these the same objects?
			if(ReferenceEquals(this, dxRecord) == true)
			{
				return 0;
			}
			else
			{
				return -1;
			}
					
		}// public int Compare(CBaseRecord dxRecord, long lMode)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to alert the derived class when the owner changes</summary>
		protected virtual void OnCollectionChanged()
		{
		}
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>The collection that owns this record</summary>
		public CBaseRecords Collection
		{
			get { return m_dxCollection; }
			set
			{
				m_dxCollection = value;
				
				//	Notify the derived class
				OnCollectionChanged();
			}
			
		}// Collection property
		
		/// <summary>The active database</summary>
		public CBaseDatabase Database
		{
			get 
			{ 
				//	The database is accessed via the owner collection
				if(this.Collection != null)
					return this.Collection.Database;
				else
					return null;
			}

		}

        /// <summary>Event source interface for error and diagnostic events</summary>
        public FTI.Shared.Trialmax.CTmaxErrorBuilder ExBuilder
        {
            get
            {
                if(this.Collection != null)
                    return this.Collection.ExBuilder;
                else
                    return null;
            }

        }// public FTI.Shared.Trialmax.CTmaxErrorBuilder ExBuilder

		#endregion Properties
		
	}// class CBaseRecord

	/// <summary>This class is used to manage a dynamic array list of CBaseRecord objects</summary>
	public class CBaseRecords : CTmaxSortedArrayList
	{
		#region Protected Members

        /// <summary>Local member bound to Fields property</summary>
		protected CDxFields m_dxFields = null;
		
		/// <summary>Local class member bound to Database property</summary>
		protected FTI.Trialmax.Database.CBaseDatabase m_dataBase = null;
		
		/// <summary>Local member bound to TableName property</summary>
		protected string m_strTableName = "";
		
		/// <summary>Local member bound to KeyFieldName property</summary>
		protected string m_strKeyFieldName = "AutoId";
		
		#endregion Protected Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CBaseRecords() : base()
		{
			//	Initialize the property values
			Initialize();
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
        public CBaseRecords(CBaseDatabase baseDatabase) : base()
		{
            m_dataBase = baseDatabase;
			
			//	Initialize the property values
			Initialize();
		}

        /// <summary>This function is called to set the database interface</summary>
        public virtual void SetDatabase(CBaseDatabase dataBase)
        {
            m_dataBase = dataBase;

        }// public virtual void SetDatabase(CBaseDatabase dataBase)

        /// <summary>Performs cleanup of local resources</summary>
		public virtual CBaseRecords Dispose()
		{
			//	Clean up
			Close();
			
			//	Flush the fields collection
			if(m_dxFields != null)
				m_dxFields.Clear();

			//	This allows use to dispose and reset the reference in one line of code
			return null;
		
		}// public virtual CBaseRecords Dispose()

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="dxRecord">The object to be removed</param>
		public void RemoveList(CBaseRecord dxRecord)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(dxRecord as object);
			}
			catch
			{
			}
		
		}// public void RemoveList(CBaseRecord dxRecord)

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="dxRecord">The object to be added to the list</param>
		/// <returns>true if successful</returns>
		public bool AddList(CBaseRecord dxRecord)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(dxRecord as object);
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public bool AddList(CBaseRecord dxRecord)

		/// <summary>This method allows the caller to insert a new object into the list</summary>
		/// <param name="dxRecord">The object to be inserted in the list</param>
		/// <param name="iIndex">The index at which to insert the object</param>
		/// <returns>true if successful</returns>
		public bool InsertList(int iIndex, CBaseRecord dxRecord)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Insert(iIndex, dxRecord as object);
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public bool InsertList(int iIndex, CBaseRecord dxRecord)

		/// <summary>This method is called to open a data set and populate the collection with the records contained in the table</summary>
		/// <returns>true if successful</returns>
		public virtual bool Open()
		{
			//	Make sure the existing objects get released
			Close();

			//	Nothing to do since we are not using data sets. If we switch over
			//	to data sets we will need to enable the code to open the data adapter
			return true;
			
		}// public virtual bool Open()
		
		/// <summary>This method is called to close the collection's connection to the database</summary>
		public virtual void Close()
		{
			//	Remove all objects from the list
			base.Clear();
			
		}// public virtual void Close()
		
		/// <summary>This method is called to populate the collection</summary>
		/// <returns>true if successful</returns>
		public virtual bool Fill()
		{
			return Fill(0);
		}
		
		/// <summary>This method is called to populate the collection</summary>
		/// <param name="lId">The unique numeric identifier, zero for all</param>
		/// <returns>true if successful</returns>
		public virtual bool Fill(long lId)
		{
			string strSQL = "";

            if(m_dataBase == null) return false;
            if(m_dataBase.IsConnected == false) return false;
		
			if(lId > 0)
				strSQL = GetSQLSearch(lId.ToString());
			else
				strSQL = GetSQLSelect();

			return Fill(strSQL);
		
		}// public virtual bool Fill(long lId)
		
		/// <summary>This method is called to populate the collection using the specified SQL statement</summary>
		/// <param name="strSQL">The SQL statement to use to populate the collection</param>
		/// <returns>true if successful</returns>
		public virtual bool Fill(string strSQL)
		{
			CBaseRecord dxRecord = null;
			
			if(m_dataBase == null) return false;
            if(m_dataBase.IsConnected == false) return false;
			if((strSQL == null) || (strSQL.Length == 0)) return false;
				
			try
			{
				//	Create a data reader
                if(m_dataBase.GetDataReader(strSQL) == true)
				{
                    while(m_dataBase.AdvanceReader() == true)
					{
						//	Allocate a new record object
						if((dxRecord = GetNewRecord()) != null)
						{
							//	Read the current field values
                            if(m_dataBase.GetValues(m_dxFields) == true)
							{
								//	Set the object properties
								if(Exchange(dxRecord, false) == true)
								{								
									//	Set the ownership
									dxRecord.Collection = this;
									
									//	Add to the local list
									base.Add(dxRecord as object);
								}
							}
							
						}// if((dxRecord = GetNewRecord()) != null)
						
					}// while(m_dataBase.Connection.AdvanceReader() == true)
					
					//	Release the reader
					m_dataBase.FreeDataReader();
					
					return true;
				}
			}
			catch(OleDbException oleEx)
			{
                FireError("Fill",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_FILL_EX,TableName),oleEx);
			}
			catch(System.Exception Ex)
			{
                FireError("Fill",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_FILL_EX,TableName),Ex);
			}
			
			//	Release the reader
			m_dataBase.FreeDataReader();
					
			//	Must have been an error
			return false;
		
		}// public virtual bool Fill(string strSQL)
		
		/// <summary>This method is called to remove all records in the collection</summary>
		/// <returns>true if successful</returns>
		public virtual bool Flush()
		{
			string strSQL = "";
		
			if(m_dataBase == null) return false;
            if(m_dataBase.IsConnected == false) return false;
		
			try
			{
				//	Get the SQL statement from the object						
				strSQL = GetSQLFlush();
				Debug.Assert(strSQL.Length > 0);
				if(strSQL.Length == 0) return false;
				
				//	Execute the statement
				if(m_dataBase.Execute(strSQL) == true)
				{	
					base.Clear();
					
					return true;
				
				}//if(m_dataBase.Execute(strSQL)) == true)
				
			}
			catch(OleDbException oleEx)
			{
				FireError("Flush", this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_FLUSH_EX, TableName, strSQL), oleEx);
			}
			catch(System.Exception Ex)
			{
                FireError("Flush",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_FLUSH_EX,TableName,strSQL),Ex);
			}
					
			//	Must have been an error
			return false;
		
		}// public virtual bool Flush()
		
		/// <summary>This method is called to create the table</summary>
		/// <returns>true if successful</returns>
		public virtual bool Create()
		{
			string strSQL = "";
		
			if(m_dataBase == null) return false;

			try
			{
				//	Get the SQL statement to create the table					
				strSQL = GetSQLCreate();
				if(strSQL.Length == 0)
				{
                    FireError("Create",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_CREATE_NO_SQL,TableName));
					return false;
				}
				
				//	Execute the statement
				return m_dataBase.Execute(strSQL);
			}
			catch(OleDbException oleEx)
			{
                FireError("Create",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_CREATE_EX,TableName,strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
                FireError("Create",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_CREATE_EX,TableName,strSQL),Ex);
			}
					
			//	Must have been an error
			return false;
		
		}// public virtual bool Create()
		
		/// <summary>This method is called to create the specified column</summary>
		/// <param name="iColumn">The id (derived class specific) of the column to create</param>
		/// <param name="bSilent">true to suppress error messages</param>
		/// <returns>true if successful</returns>
		public virtual bool CreateColumn(int iColumn, bool bSilent)
		{
			string	strSQL = "";
			int		iIndex = -1;
		
			Debug.Assert(this.Database != null);
			if(this.Database == null) return false;
            if(this.Database.IsConnected == false) return false;

			try
			{
				Debug.Assert(m_dxFields != null);
				if(m_dxFields == null) return false;
				
				//	Don't bother if this column already exists
				if((iIndex = this.Database.GetColumnIndex(this.TableName, m_dxFields[iColumn].Name)) < 0)
				{
					//	Get the SQL statement to create the table					
					strSQL = GetSQLCreateColumn(iColumn);
					if(strSQL.Length > 0)
					{
						//	Execute the statement
						this.Database.Execute(strSQL);
						
						//	Make sure it exists
						iIndex = this.Database.GetColumnIndex(this.TableName, m_dxFields[iColumn].Name);
					}
					else
					{					
						//	Always display this error
                        FireError("CreateColumn",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_CREATE_COLUMN_NO_SQL,iColumn,TableName));
					
					}// if(strSQL.Length > 0)
				
				}// if(this.Database.Connection.HasColumn(this.TableName, m_dxFields[iColumn].Name) == false)
				
			}
			catch(OleDbException oleEx)
			{
				if(bSilent == false)
                    FireError("CreateColumn",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_CREATE_EX,iColumn,TableName,strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
				if(bSilent == false)
                    FireError("CreateColumn",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_CREATE_EX,iColumn,TableName,strSQL),Ex);
			}
					
			return (iIndex >= 0);
		
		}// public virtual bool CreateColumn(int iColumn)
		
		/// <summary>This method is called to set the auto increment attribute of the specified column</summary>
		/// <param name="iColumn">The id (derived class specific) of the column to be modified</param>
		/// <param name="bEnabled">True to enable auto increment</param>
		/// <returns>true if successful</returns>
		public virtual bool SetAutoIncrement(int iColumn, bool bEnabled)
		{
			string	strSQL = "";
			bool	bSuccessful = false;
		
			Debug.Assert(this.Database != null);
			if(this.Database == null) return false;
            if(this.Database.IsConnected == false) return false;

			try
			{
				//	Get the SQL statement to modify the column				
				strSQL = GetSQLSetAutoIncrement(iColumn, bEnabled);
				if(strSQL.Length > 0)
				{
					//	Execute the statement
					bSuccessful = this.Database.Execute(strSQL);
				}
				else
				{					
					FireError("SetAutoIncrement", this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_SET_AUTO_INCREMENT_NO_SQL, iColumn, TableName));
					
				}// if(strSQL.Length > 0)
				
			}
			catch(OleDbException oleEx)
			{
                FireError("SetAutoIncrement",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_SET_AUTO_INCREMENT_EX,iColumn,TableName,strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
                FireError("SetAutoIncrement",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_SET_AUTO_INCREMENT_EX,iColumn,TableName,strSQL),Ex);
			}
					
			return bSuccessful;
		
		}// public virtual bool SetAutoIncrement(int iColumn, bool bEnabled)
		
		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxRecord">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public virtual CBaseRecord Add(CBaseRecord dxRecord)
		{
			string strSQL = "";

			Debug.Assert(dxRecord != null);
			if(dxRecord == null) return null;
			if(m_dataBase == null) return null;
            if(m_dataBase.IsConnected == false) return null;
			
			try
			{
				//	Get the SQL statement					
				strSQL = GetSQLInsert(dxRecord);

				Debug.Assert(strSQL.Length > 0);

				//	Execute the statement
				if(m_dataBase.Execute(strSQL) == true)
				{	
					//	Set the ownership to be this collection
					dxRecord.Collection = this;
						
					//	Add it to the underlying array list
					base.Add(dxRecord as object);
						
					return dxRecord;
				
				}//if(m_dataBase.Connection.Execute(strSQL)) == true)
				
			}
			catch(OleDbException oleEx)
			{
				FireError("Add", this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_ADD_EX, TableName, strSQL), oleEx, GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
				FireError("Add", this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_ADD_EX, TableName, strSQL), Ex, GetErrorItems(dxRecord));
			}
			
			return null;
			
		}// public virtual CBaseRecord Add(CBaseRecord dxRecord)

		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public virtual bool Update(CBaseRecord dxRecord)
		{
			string strSQL = "";
			
			if(m_dataBase == null) return false;
            if(m_dataBase.IsConnected == false) return false;

			try
			{
				//	Get the SQL statement 					
				strSQL = GetSQLUpdate(dxRecord);

				Debug.Assert(strSQL.Length > 0);
				
				//	Execute the statement
				if(m_dataBase.Execute(strSQL) == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				
			}
			catch(OleDbException oleEx)
			{
                FireError("Update",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_UPDATE_EX,TableName,strSQL),oleEx,GetErrorItems(dxRecord));
			}
			catch(System.Exception Ex)
			{
                FireError("Update",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_UPDATE_EX,TableName,strSQL),Ex,GetErrorItems(dxRecord));
			}
			
			return false;
			
		}// public virtual bool Update(CBaseRecord dxRecord)

		/// <summary>This method is called to delete the requested record</summary>
		/// <param name="dxRecord">The object to be deleted</param>
		///	<returns>true if successful</returns>
		public virtual bool Delete(CBaseRecord dxRecord)
		{
			string	strSQL = "";
            lock (m_dataBase)
            {
                try
                {
                    //	Get the SQL statement from the object						
                    strSQL = GetSQLDelete(dxRecord);
                    Debug.Assert(strSQL.Length > 0);

                    //	Execute the statement
                    if (m_dataBase.IsConnected == false) return false;
                    if (m_dataBase.Execute(strSQL) == true)
                    {

                        base.Remove(dxRecord as object);
                        return true;

                    }//if(m_dataBase.Connection.Execute(strSQL)) == true)

                }
                catch (OleDbException oleEx)
                {
                    FireError("Delete", this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_DELETE_EX, TableName, strSQL), oleEx, GetErrorItems(dxRecord));
                }
                catch (System.Exception Ex)
                {
                    FireError("Delete", this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_DELETE_EX, TableName, strSQL), Ex, GetErrorItems(dxRecord));
                }

                //	Must have been some kind of problem
                return false;
            }
			
			
		}// public virtual bool Delete(CBaseRecord dxRecord)

		/// <summary>This method is called to delete the records</summary>
		/// <param name="strSQL">The SQL DELETE statement</param>
		///	<returns>true if successful</returns>
		public virtual bool Delete(string strSQL)
		{
			try
			{
				//	Execute the statement
				if(m_dataBase.IsConnected == false) return false;
				if(m_dataBase.Execute(strSQL) == true)
				{	
					return true;
				
				}//if(m_dataBase.Connection.Execute(strSQL)) == true)
				
			}
			catch(OleDbException oleEx)
			{
                FireError("Delete",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_DELETE_EX,TableName,strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
                FireError("Delete",this.ExBuilder.Message(CBaseDatabase.ERROR_BASE_RECORDS_DELETE_EX,TableName,strSQL),Ex);
			}
			
			//	Must have been some kind of problem
			return false;
			
		}// public virtual bool Delete(string strSQL)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="dxRecord">The parameter object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public virtual bool Contains(CBaseRecord dxRecord)
		{
			// Use base class to process actual collection operation
			return base.Contains(dxRecord as object);
		}

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CBaseRecord this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return GetAt(index);
			}
		
		}// public new CBaseRecord this[int index]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new CBaseRecord GetAt(int index)
		{
			// Use base class to process actual collection operation
			return (base.GetAt(index) as CBaseRecord);
		
		}// public new CBaseRecord GetAt(int index)

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public virtual int IndexOf(CBaseRecord value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value as object);
		
		}// public virtual int IndexOf(CBaseRecord value)
		
		/// <summary>This method is called to get the SQL statement required to select the desired records</summary>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLSelect()
		{
			string strSQL = "SELECT * FROM " + m_strTableName;
			return strSQL;
		}

		/// <summary>This method is called to get the SQL statement required to flush all records belonging to the collection</summary>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLFlush()
		{
			string strSQL = "";
			return strSQL;
		}
		
		/// <summary>This method is called to get the SQL statement required to locate a specific record</summary>
		/// <param name="strKeyId">The key id of the record to search for</param>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLSearch(string strKeyId)
		{
			string strSQL = "SELECT * FROM " + m_strTableName;
			
			strSQL += (" WHERE " + m_strKeyFieldName + " = ");
			strSQL += strKeyId;
			strSQL += ";";
			
			return strSQL;
		
		}// public virtual string GetSQLSearch(string strKeyId)

		/// <summary>This method is called to get the SQL statement required to delete this object's record in the database</summary>
		/// <param name="dxRecord">The object to be deleted</param>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLDelete(CBaseRecord dxRecord)
		{
			string strSQL = "DELETE FROM ";
			
			strSQL += TableName;
			strSQL += (" WHERE " + m_strKeyFieldName + " = ");
			strSQL += dxRecord.GetKeyId();
			strSQL += ";";
			
			return strSQL;
		
		}// public virtual string GetSQLDelete(CBaseRecord dxRecord)
		
		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLInsert(CBaseRecord dxRecord)
		{
			string strSQL = "";
			return strSQL;
		
		}// public virtual string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLUpdate(CBaseRecord dxRecord)
		{
			string strSQL = "";
			return strSQL;
		
		}// public virtual string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to create the table</summary>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLCreate()
		{
			string strSQL = "";
			return strSQL;
		
		}// public virtual string GetSQLCreate()
		
		/// <summary>This method is called to get the SQL statement required to create the specified column</summary>
		/// <param name="iColumn">Identifies the desired column (derived class specific)</param>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLCreateColumn(int iColumn)
		{
			string strSQL = "";
			return strSQL;
		
		}// public virtual string GetSQLCreateColumn(int iColumn)
		
		/// <summary>This method is called to get the SQL statement required to enable/disable the autoincrement attribute of the specified column</summary>
		/// <param name="iColumn">Identifies the desired column (derived class specific)</param>
		/// <returns>The appropriate SQL statement</returns>
		public virtual string GetSQLSetAutoIncrement(int iColumn, bool bEnabled)
		{
			string strSQL = "";
			return strSQL;
		
		}// public virtual string GetSQLSetAutoIncrement(int iColumn, bool bEnabled)
		
		/// <summary>This method is called to report the occurance of a TrialMax database error</summary>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="aItems">The collection of error specific items</param>
		protected virtual void FireError(string strMethod, string strMessage, CTmaxErrorArgs.CErrorItems aItems)
		{
			if(m_dataBase != null)
				m_dataBase.FireError(this, strMethod, strMessage, aItems);
		}
		
		/// <summary>This method is called to report the occurance of a TrialMax database error</summary>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		protected virtual void FireError(string strMethod, string strMessage)
		{
			if(m_dataBase != null)
				m_dataBase.FireError(this, strMethod, strMessage);
		}
		
		/// <summary>This method is called to report an error</summary>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="oleEx">The OLE data provider exception associated with the error</param>
		/// <param name="aItems">The collection of items assocaited with the error</param>
		public void FireError(string strMethod, string strMessage, OleDbException oleEx, CTmaxErrorArgs.CErrorItems aItems)
		{
			if(m_dataBase != null)
				m_dataBase.FireError(this, strMethod, strMessage, oleEx, aItems);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="oleEx">The OLE data provider exception associated with the error</param>
		public void FireError(string strMethod, string strMessage, OleDbException oleEx)
		{
			if(m_dataBase != null)
				m_dataBase.FireError(this, strMethod, strMessage, oleEx);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="Ex">The system exception associated with the error</param>
		/// <param name="aItems">The collection of items assocaited with the error</param>
		public void FireError(string strMethod, string strMessage, System.Exception Ex, CTmaxErrorArgs.CErrorItems aItems)
		{
			if(m_dataBase != null)
				m_dataBase.FireError(this, strMethod, strMessage, Ex, aItems);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="Ex">The system exception associated with the error</param>
		public void FireError(string strMethod, string strMessage, System.Exception Ex)
		{
			if(m_dataBase != null)
				m_dataBase.FireError(this, strMethod, strMessage, Ex);
		}

		/// <summary>
		/// This method will construct the list of error items using the specified record transfer object
		/// </summary>
		/// <param name="dxRecord">The record transfer object associated with the error</param>
		/// <returns>The full populated error items collection</returns>
		public virtual CTmaxErrorArgs.CErrorItems GetErrorItems(CBaseRecord dxRecord)
		{
			CTmaxErrorArgs.CErrorItems aItems = new CTmaxErrorArgs.CErrorItems();
			
			if(m_dxFields != null)
			{
				//	Update the fields collection
				//Exchange(dxRecord, true);
				
				foreach(CDxField dxField in m_dxFields)
				{
					if(dxField.Value != null)
						aItems.Add(new CTmaxErrorArgs.CErrorItem(dxField.Name, dxField.Value.ToString()));
					else
						aItems.Add(new CTmaxErrorArgs.CErrorItem(dxField.Name, "null"));
				}
				
			}
			return aItems;
		}
		
		/// <summary>This method is called to get the index of the specified column within the table</summary>
		/// <param name="strTable">The name of the table</param>
		/// <param name="strColumn">The name of the column</param>
		/// <returns>The zero-based index of the specified column</returns>
		public virtual int GetColumnIndex(string strTable, string strColumn)
		{
			int iIndex = -1;
			
			if(m_dataBase == null) return -1;
			if(m_dataBase.IsConnected == false) return -1;
			
			try
			{
				iIndex = m_dataBase.GetColumnIndex(this.TableName, strColumn);
			}
			catch
			{
			}
					
			return iIndex;
		
		}// public virtual int GetColumnIndex(string strTable, string strColumn)
		
		/// <summary>This method will construct the list of diagnostic items using the specified record transfer object</summary>
		/// <param name="dxRecord">The record transfer object associated with the message</param>
		/// <returns>The full populated diagnostic items collection</returns>
		public virtual CTmaxDiagnosticArgs.CDiagnosticItems GetDiagnosticItems(CBaseRecord dxRecord)
		{
			CTmaxDiagnosticArgs.CDiagnosticItems aItems = new CTmaxDiagnosticArgs.CDiagnosticItems();
			
			if(m_dxFields != null)
			{
				//	Update the fields collection
				Exchange(dxRecord, true);
				
				foreach(CDxField dxField in m_dxFields)
				{
					if(dxField.Value != null)
						aItems.Add(new CTmaxDiagnosticArgs.CDiagnosticItem(dxField.Name, dxField.Value.ToString()));
					else
						aItems.Add(new CTmaxDiagnosticArgs.CDiagnosticItem(dxField.Name, "null"));
				}
				
			}
			return aItems;
		}
		
		/// <summary>This method will construct the list of error items using the specified record transfer object</summary>
		/// <param name="dxRecord">The record transfer object associated with the error</param>
		/// <returns>The full populated error items collection</returns>
		public virtual string ToString(CBaseRecord dxRecord)
		{
			string strRecord = "";
			
			if(m_dxFields != null)
			{
				//	Update the fields collection
				Exchange(dxRecord, true);
				
				foreach(CDxField dxField in m_dxFields)
				{
					if(dxField.Value != null)
						strRecord += (dxField.Name + "=" + dxField.Value.ToString() + " ");
					else
						strRecord += (dxField.Name + "=null ");
			
				}
			
			}//if(m_dxFields != null)
			
			//	Just in case no fields are defined
			if(strRecord.Length == 0)
				strRecord = "<>";
				
			return strRecord;
		
		}// public virtual string ToString(CBaseRecord dxRecord)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to initilize the collection properties</summary>
		protected virtual void Initialize()
		{
			//	Set the name of the table bound to this collection
			SetNames();
			
			//	Allocate the local objects
			m_dxFields  = new CDxFields();
			
			//	Fill the fields collection
			SetFields();
			
			//	Set the default base class property values
			KeepSorted = false;
		
		}// protected virtual void Initialize()
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected virtual void SetNames()
		{
			//	Derived classes should set table names and key field names here
		}
		
		/// <summary>This function is called to populate the Fields collection</summary>
		protected virtual void SetFields()
		{
			CDxField dxField = null;
			
			if(m_dxFields != null)
			{
				string[] aFields = GetFieldNames();
				
				if(aFields != null)
				{
					for(int i = 0; i <= aFields.GetUpperBound(0); i++)
					{
						//	Create a new code object and set its default index and value
						dxField = new CDxField(aFields[i]);
						
						dxField.Index = i;
						SetValue(i, dxField);
					
						m_dxFields.Add(dxField);
					}
				
				}// if(aFields != null)
			
			}// if(m_dxFields != null)

		}// protected virtual void SetFields()
		
		/// <summary>This method will set the default value for the specified field</summary>
		/// <param name="iField">The enumerated field identifier</param>
		/// <param name="dxField">The field object to be set</param>
		protected virtual void SetValue(int iField, CDxField dxField)
		{
		}// protected virtual void SetValue(int iField, CDxField dxField)
		
		/// <summary>This function is called to get a new record object</summary>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>A new object of the collection type</returns>
		protected virtual CBaseRecord GetNewRecord()
		{
			return (new CBaseRecord());
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected virtual bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			if(bSetFields == true)
			{
				//	Exchange data from local class members to assocaited fields
			}
			else
			{
				//	Exchange data from field values to local class members
			}
			
			return true;		
		
		}// protected virtual bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		
		/// <summary>This method will convert a boolean value to an appropriate SQL value</summary>
		/// <param name="bValue">The boolean value</param>
		/// <returns>The corresponding SQL string</returns>
		protected virtual string BoolToSQL(bool bValue)
		{
			return CTmaxToolbox.BoolToSQL(bValue);
		}
		
		/// <summary>This method will replace special characters in the specified string to make it appropriate for a SQL statement</summary>
		/// <param name="strSQL">The SQL text</param>
		/// <returns>The encoded SQL string</returns>
		protected virtual string SQLEncode(string strSQL)
		{
			return CTmaxToolbox.SQLEncode(strSQL);
		}
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected virtual string[] GetFieldNames()
		{
			return null;
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>This property stores the CBaseDatabase object that owns this collection</summary>
		public CBaseDatabase Database
		{
			get { return m_dataBase; }
			set { SetDatabase(value); }
		}

        /// <summary>Event source interface for error and diagnostic events</summary>
        public FTI.Shared.Trialmax.CTmaxErrorBuilder ExBuilder
        {
            get 
            { 
                if(this.Database != null)
                    return this.Database.ExBuilder;
                else
                    return null;
            }

        }// public FTI.Shared.Trialmax.CTmaxErrorBuilder ExBuilder

		/// <summary>This property identifies the name of the table associated with this record collection</summary>
		public string TableName
		{
			get { return m_strTableName; }
			set { m_strTableName = value; }
		}
		
		/// <summary>This property identifies the name of the primary key column</summary>
		public string KeyFieldName
		{
			get { return m_strKeyFieldName; }
			set { m_strKeyFieldName = value; }
		}
		
		/// <summary>This property exposes the local CDxFields collection</summary>
		public CDxFields Fields
		{
			get { return m_dxFields; }
		}
		
		#endregion Properties
		
	}//	public class CBaseRecords : CTmaxSortedArrayList
		
}// namespace FTI.Trialmax.Database
