using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
    /// <summary>This class serves as the base class for all TrialMax database classes</summary>
    public class CBaseDatabase
    {
        #region Constants

        /// <summary>Error message identifiers</summary>
        public const int ERROR_BASE_DATABASE_CONNECT_EX                 = 0;
        public const int ERROR_BASE_DATABASE_GET_DATA_READER_EX         = 1;
        public const int ERROR_BASE_DATABASE_ADVANCE_READER_EX          = 2;
        public const int ERROR_BASE_DATABASE_HAS_TABLE_EX               = 3;
        public const int ERROR_BASE_DATABASE_GET_COLUMN_INDEX_EX        = 4;
        public const int ERROR_BASE_DATABASE_GET_VAUES_EX               = 5;
        public const int ERROR_BASE_DATABASE_GET_AUTO_NUMBER_EX         = 6;
        public const int ERROR_BASE_RECORDS_FILL_EX                     = 7;
        public const int ERROR_BASE_RECORDS_FLUSH_EX                    = 8;
        public const int ERROR_BASE_RECORDS_CREATE_EX                   = 9;
        public const int ERROR_BASE_RECORDS_CREATE_COLUMN_EX            = 10;
        public const int ERROR_BASE_RECORDS_CREATE_COLUMN_NO_SQL        = 11;
        public const int ERROR_BASE_RECORDS_CREATE_NO_SQL               = 12;
        public const int ERROR_BASE_RECORDS_SET_AUTO_INCREMENT_EX       = 13;
        public const int ERROR_BASE_RECORDS_SET_AUTO_INCREMENT_NO_SQL   = 14;
        public const int ERROR_BASE_RECORDS_ADD_EX                      = 15;
        public const int ERROR_BASE_RECORDS_UPDATE_EX                   = 16;
        public const int ERROR_BASE_RECORDS_DELETE_EX                   = 17;
        public const int ERROR_BASE_DATABASE_OPEN_EX					= 18;
		public const int ERROR_BASE_DATABASE_GET_AUTO_GUID_EX			= 19;
 
        // Derived classes should start their error numbering with this value
		protected const int ERROR_BASE_DATABASE_LAST = 19;

        protected const string BASE_DATABASE_DEFAULT_GUID_ID_COLUMN = "UniqueId";
        protected const string BASE_DATABASE_DEFAULT_GUID_ORDER_COLUMN = "CreatedOn";
        
        #endregion Constants

        #region Protected Members

        /// <summary>Local member bounded to EventSource property</summary>
        protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

        /// <summary>Local member used to construct error messages</summary>
        protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

        /// <summary>OLE DB data provider connection object</summary>
        protected System.Data.OleDb.OleDbConnection m_oleDbConnection = null;

        /// <summary>OLE DB data provider connection object</summary>
        protected System.Data.OleDb.OleDbDataReader m_oleDbReader = null;

        /// <summary>Class member bound to FileSpec property</summary>
        protected string m_strFileSpec = "";

        #endregion Private Members

        #region Public Methods

        /// <summary>Constructor</summary>
        public CBaseDatabase()
        {
            m_tmaxEventSource.Name = "baseDatabase";
            
            //	Populate the error builder collection
            SetErrorStrings();

        }// public CBaseDatabase()

		/// <summary>This method is called to open the database</summary>
		/// <param name="strFileSpec">The path to the database file</param>
		/// <returns>true if successful</returns>
		public virtual bool Open(string strFileSpec)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Close the existing connection
				Close();

				//	Store the new file path
				m_strFileSpec = strFileSpec;

				//	Open the connection to the provider
				bSuccessful = Connect(m_strFileSpec);

			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this,"Open",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_OPEN_EX,m_strFileSpec),oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this,"Open",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_OPEN_EX,m_strFileSpec),Ex);
			}

			return bSuccessful;

		}// public virtual bool Open(string strFileSpec)

		/// <summary>Called to close the database</summary>
		public virtual void Close()
		{
			FreeDataReader();

			//	Close the provider connection
			Disconnect();

			m_strFileSpec = "";

		}//  public virtual void Close()

		/// <summary>This method is called to execute the requested SQL statement</summary>
		/// <param name="strSQL">SQL statement to be executed</param>
        /// <returns>true if successful</returns>
		public virtual bool Execute(string strSQL)
		{
            //	Create the command object
            OleDbCommand oleCmd = new OleDbCommand(strSQL,m_oleDbConnection);
            oleCmd.ExecuteNonQuery();
 			
			//	NOTE:	We make the caller trap exceptions so that error messages
			//			can be presented in the context in which this method is called
			
			return true;

        }// public virtual bool Execute(string strSQL)
		
		/// <summary>This method is called to create a data reader for the appropriate provider</summary>
		/// <param name="strSQL">The SQL Select state used to create the reader</param>
		/// <returns>true if successful</returns>
		public virtual bool GetDataReader(string strSQL)
		{
		    bool bSuccessful = false;
		    
			Debug.Assert(strSQL != null);
			Debug.Assert(strSQL.Length > 0);
			
			try
			{
                //	Create the reader object
                OleDbCommand oleCmd = new OleDbCommand(strSQL,m_oleDbConnection);
                m_oleDbReader = oleCmd.ExecuteReader();
                
                bSuccessful = true;
			}
			catch(OleDbException oleEx)
			{
                m_tmaxEventSource.FireError(this,"GetDataReader",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_DATA_READER_EX, strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
                m_tmaxEventSource.FireError(this,"GetDataReader",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_DATA_READER_EX, strSQL),Ex);
			}

            return bSuccessful;

        }// public virtual bool GetDataReader(string strSQL)
		
		/// <summary>This method is called to advance the data reader to the next record</summary>
		/// <returns>true if successful</returns>
		public virtual bool AdvanceReader()
		{
		    bool bSuccessful = false;
		    
			try
			{
				if(m_oleDbReader != null)
					bSuccessful = m_oleDbReader.Read();
			}
			catch(OleDbException oleEx)
			{
                m_tmaxEventSource.FireError(this,"AdvanceReader",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_ADVANCE_READER_EX),oleEx);
			}
			catch(System.Exception Ex)
			{
                m_tmaxEventSource.FireError(this,"AdvanceReader",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_ADVANCE_READER_EX),Ex);
			}

            return bSuccessful;

		}// public virtual bool AdvanceReader()

        /// <summary>This method is called to determine if the specified table exists in the database</summary>
        /// <param name="strName">The name of the table</param>
        /// <returns>true if the table exists</returns>
        public virtual bool HasTable(string strName)
        {
            DataTable dbTables = null;

            try
            {
                //	Do we have a valid connection?
                if(m_oleDbConnection == null) return false;

                //	Make sure the connection is open
                if(m_oleDbConnection.State != ConnectionState.Open)
                {
                    m_oleDbConnection.Open();

                    if(m_oleDbConnection.State != ConnectionState.Open)
                        return false;
                }

                //	Get the tables in the database
                dbTables = m_oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                                                                 new object[] { null,null,null,"TABLE" });
                foreach(DataRow O in dbTables.Rows)
                {
                    if(String.Compare((O["TABLE_NAME"]).ToString(),strName,true) == 0)
                        return true;
                }

            }
            catch(System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this,"HasTable",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_HAS_TABLE_EX,strName),Ex);
            }

            return false;

        }// public virtual bool HasTable(string strName)

        /// <summary>This method is called to get the index of the specified column within the table</summary>
        /// <param name="strTable">The name of the table</param>
        /// <param name="strColumn">The name of the column</param>
        /// <returns>The zero-based index of the specified column</returns>
        public virtual int GetColumnIndex(string strTable, string strColumn)
        {
            DataSet dataSet = null;
            string strSQL = "";
            OleDbDataAdapter oleDbAdapter = null;
            int iIndex = -1;

            try
            {
                //	Make sure the specified table exists
                //
                //	NOTE:	This also confirms that we have a valid connection
                if(HasTable(strTable) == false) return -1;

                dataSet = new DataSet(strTable + strColumn);

                strSQL = String.Format("SELECT * FROM {0}",strTable);

                oleDbAdapter = new OleDbDataAdapter(strSQL,m_oleDbConnection);
                oleDbAdapter.Fill(dataSet,strTable);

                //	Should only be 1 table in the data set but we'll play it safe
                if((dataSet.Tables != null) && (dataSet.Tables.Count > 0))
                {
                    foreach(DataTable table in dataSet.Tables)
                    {
                        //	Is this the requested table?
                        if(String.Compare(table.TableName,strTable,true) == 0)
                        {
                            //	Search for the requested column
                            if((table.Columns != null) && (table.Columns.Count > 0))
                            {
                                foreach(DataColumn O in table.Columns)
                                {
                                    iIndex += 1;

                                    if(String.Compare(O.ColumnName,strColumn,true) == 0)
                                        return iIndex;
                                }

                            }// if((table.Columns != null) && (table.Columns.Count > 0))

                        }

                    }// foreach(DataTable table in dataSet.Tables)

                }//	if((dataSet.Tables != null) && (dataSet.Tables.Count > 0))

            }
            catch(System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this,"GetColumnIndex",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_COLUMN_INDEX_EX, strColumn, strTable),Ex);
            }

            return -1; // Not found

        }// public virtual int GetColumnIndex(string strTable, string strColumn)

        /// <summary>This method is called to add the specified column to the table</summary>
        /// <param name="strTable">The name of the table to be added to</param>
        /// <param name="strName">The name of the new column</param>
        /// <param name="strSQLType">The SQL type descriptor</param>
        /// <returns>true if successful</returns>
        public virtual bool AddColumn(string strTable, string strName, string strSQLType,string strDefault)
        {
            string strSQL = "";

            try
            {
                //	Format the SQL statement to add the column				
                strSQL = "ALTER TABLE ";
                strSQL += strTable;

                strSQL += "\nADD COLUMN ";
                strSQL += (strName + " ");
                strSQL += strSQLType;

                if((strDefault != null) && (strDefault.Length > 0))
                    strSQL += (" DEFAULT " + strDefault);

                strSQL += " ;";

                //	Execute the statement
                if(Execute(strSQL) == true)
                {
                    //	Set the default value for records that already exist
                    if(strDefault != null)
                    {
                        strSQL = "UPDATE " + strTable + " SET ";
                        strSQL += (strName + " = '" + strDefault + "';");

                        try { Execute(strSQL); }
                        catch { }

                    }// if(strDefault != null)

                    return true;

                }// if(Execute(strSQL) == true)

            }
            catch
            {
            }

            //	Must have been an error
            return false;

        }// public virtual bool AddColumn(string strTable, string strName, string strSQLType)

        /// <summary>This method is called to get the values for all fields in the collection</summary>
        /// <param name="dxFields">The collection of fields to be retrieved</param>
        /// <returns>true if successful</returns>
        public virtual bool GetValues(CDxFields dxFields)
        {
            int     iIndex = -1;
            bool    bSuccessful = false;

            Debug.Assert(dxFields != null);
            Debug.Assert(dxFields.Count > 0);

            try
            {
                if(m_oleDbReader != null)
                {
                    for(int i = 0;((i < m_oleDbReader.FieldCount) && (i < dxFields.Count));i++)
                    {
                        if((iIndex = dxFields[i].Index) >= 0)
                            dxFields[i].Value = m_oleDbReader.GetValue(iIndex);
                    }

                }// if(m_oleDbReader != null)
                
                bSuccessful = true;
            }
            catch(OleDbException oleEx)
            {
                m_tmaxEventSource.FireError(this,"GetValues",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_VAUES_EX),oleEx);
            }
            catch(System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this,"GetValues",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_VAUES_EX),Ex);
            }

            return bSuccessful;

        }// public virtual bool GetValues(CDxFields dxFields)

        /// <summary>This method is called to get the specified value from the current data reader</summary>
        /// <param name="iIndex">The index of the value to be retrieved</param>
        /// <returns>true if successful</returns>
        public virtual object GetValue(int iIndex)
        {
            object O = null;
            
            try
            {
                if(m_oleDbReader != null)
                    O = m_oleDbReader.GetValue(iIndex);
            }
            catch
            {
            }

            return O;

        }// public virtual object GetValue(int iIndex)

        /// <summary>This method is called to get the AutoNumber value assigned by the provider on the last INSERT operation </summary>
        /// <returns>The number generated by the provider</returns>
        public virtual int GetAutoNumber()
        {
			OleDbCommand oleCmd = null;

			//	This method for retrieving the AutoId is for Microsoft
            //	Access 2000 and greater. If we add support for other
            //	providers we will have to implement their method for 
            //	retrieving the new AutoId

            try
            {
				if((oleCmd = new OleDbCommand("SELECT @@IDENTITY",m_oleDbConnection)) != null)
				{
					return (int)(oleCmd.ExecuteScalar());
                }

            }
            catch(OleDbException oleEx)
            {
                m_tmaxEventSource.FireError(this,"GetAutoNumber",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_AUTO_NUMBER_EX),oleEx);
            }
            catch(System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this,"GetAutoNumber",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_AUTO_NUMBER_EX),Ex);
            }

            //	NOTE:	We used to return -1 here but we found out that Access
            //			uses negative numbers for Auto fields if replication is turned ON
            return 0;

        }// public virtual int GetAutoNumber()

		/// <summary>This method is called to get the UniqueId (GUID) value assigned by the provider on the last INSERT operation </summary>
		/// <param name="strTable">The name of the table containing the new record</param>
		/// <param name="strSortColumn">The name of the column used to order in reverse to get to the last record</param>
		/// <param name="strIdColumn">The name of the column containing the GUID value</param>
		/// <returns>The GUID generated by the provider</returns>
		public virtual System.Guid GetAutoGuid(string strTable, string strSortColumn, string strIdColumn)
		{
			OleDbCommand	oleCmd = null;
			string			strSQL = "";
			System.Guid		autoGuid = System.Guid.Empty;

			//	Should we assign defaults?
			if(strSortColumn.Length == 0)
				strSortColumn = BASE_DATABASE_DEFAULT_GUID_ORDER_COLUMN;
			if(strIdColumn.Length == 0)
				strIdColumn = BASE_DATABASE_DEFAULT_GUID_ID_COLUMN;
				
			try
			{
				strSQL = String.Format("SELECT {0} FROM {1} ORDER BY {2} DESC;", strIdColumn, strTable, strSortColumn);
				
				if((oleCmd = new OleDbCommand(strSQL,m_oleDbConnection)) != null)
				{
					autoGuid = (System.Guid)(oleCmd.ExecuteScalar());
				}

			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this,"GetAutoGuid",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_AUTO_GUID_EX, strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this,"GetAutoGuid",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_GET_AUTO_GUID_EX, strSQL),Ex);
			}

			return autoGuid;

		}// public virtual System.Guid GetAutoGuid(string strTable, string strSortOn)

		/// <summary>This method is called to get the UniqueId (GUID) value assigned by the provider on the last INSERT operation </summary>
		/// <param name="strTable">The name of the table containing the new record</param>
		/// <param name="strSortColumn">The name of the column used to order in reverse to get to the last record</param>
		/// <returns>The GUID generated by the provider</returns>
		public virtual System.Guid GetAutoGuid(string strTable, string strSortColumn)
		{
			return GetAutoGuid(strTable, strSortColumn, BASE_DATABASE_DEFAULT_GUID_ID_COLUMN);
		}

		/// <summary>This method is called to get the UniqueId (GUID) value assigned by the provider on the last INSERT operation </summary>
		/// <param name="strTable">The name of the table containing the new record</param>
		/// <returns>The GUID generated by the provider</returns>
		public virtual System.Guid GetAutoGuid(string strTable)
		{
			return GetAutoGuid(strTable, BASE_DATABASE_DEFAULT_GUID_ORDER_COLUMN, BASE_DATABASE_DEFAULT_GUID_ID_COLUMN);
		}

		/// <summary>Called to get the current state of the connection</summary>
        /// <returns>The current state of the connection</returns>
        public virtual ConnectionState GetConnectionState()
        {
            if(m_oleDbConnection != null)
                return m_oleDbConnection.State;
            else
                return ConnectionState.Closed;

        }// public virtual ConnectionState GetConnectionState()

        /// <summary>Called to determine if the database is connectd to a source</summary>
        /// <returns>True if the database is connected and ready to execute</returns>
        public virtual bool GetIsConnected()
        {
            //	What is the current state
            switch(GetConnectionState())
            {
                case ConnectionState.Broken:
                case ConnectionState.Closed:

                    //  Should we attempt to connect?
                    if(m_strFileSpec.Length > 0)
                    {
                        return Connect(m_strFileSpec);
                    }
                    else
                    {
                        return false;
                    }

                case ConnectionState.Connecting:
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                case ConnectionState.Open:
                default:

                    return true;

            }// switch(GetConnectionState())

        }// public virtual bool GetIsConnected()

        /// <summary>This method is called to free the data reader</summary>
        public virtual void FreeDataReader()
        {
            try
            {
                if(m_oleDbReader != null)
                {
                    m_oleDbReader.Close();
                    m_oleDbReader = null;
                }

            }
            catch
            {
            }

        }// public virtual void FreeDataReader()

		/// <summary>Call to determine if the specified file exists</summary>
		/// <param name="strFilename">Full path of the file to be located</param>
		/// <returns>true if the file exists</returns>
		public bool FindFile(string strFilename)
		{
			return System.IO.File.Exists(strFilename);
		}

		/// <summary>This method is called to fire a diagnostic event</summary>
        /// <param name="Args">Argument object to be passed in the event</param>
        public virtual void FireDiagnostic(CTmaxDiagnosticArgs Args)
        {
            m_tmaxEventSource.FireDiagnostic(this, Args);
        }

        /// <summary>This method is called to fire a diagnostic event</summary>
        /// <param name="objSource">The object that's generating the event</param>
        /// <param name="strMethod">The calling method</param>
        /// <param name="strMessage">The diagnostic message</param>
        /// <param name="aItems">The collection of items assocaited with the event</param>
        public virtual void FireDiagnostic(object objSource,string strMethod,string strMessage,CTmaxDiagnosticArgs.CDiagnosticItems aItems)
        {
            m_tmaxEventSource.FireDiagnostic(objSource, strMethod, strMessage, aItems);
        }

        /// <summary>This method is called to fire a diagnostic event</summary>
        /// <param name="objSource">The object that's generating the error</param>
        /// <param name="strMethod">The calling method</param>
        /// <param name="strMessage">The diagnostic message</param>
        public virtual void FireDiagnostic(object objSource,string strMethod,string strMessage)
        {
            FireDiagnostic(objSource,strMethod,strMessage,null);
        }

        /// <summary>This method is called to fire an error event</summary>
        /// <param name="Args">Argument object to be passed in the event</param>
        public virtual void FireError(CTmaxErrorArgs Args)
        {
            if(Args != null)
                m_tmaxEventSource.FireError(this, Args);
        }

        /// <summary>Called to fire an error event using the specified values</summary>
        /// <param name="objSource">The object that's generating the error</param>
        /// <param name="strMethod">The method that encountered the error</param>
        /// <param name="strMessage">The error message</param>
        /// <param name="aItems">The list of items associated with the message</param>
        /// <param name="strException">The exception message</param>
        /// <param name="strDetails">The error details</param>
        public virtual void FireError(object objSource,string strMethod,string strMessage,CTmaxErrorArgs.CErrorItems aItems,string strException,string strDetails)
        {
            string strSource = (objSource.GetType().FullName + "::" + strMethod);
            CTmaxErrorArgs Args;

            try
            {
                if((Args = new CTmaxErrorArgs()) != null)
                {
                    Args.Initialize("TrialMax Database Error",strSource,strMessage,aItems,strException,strDetails);

                    FireError(Args);
                }
            }
            catch
            {
            }
        }

        /// <summary>This method is called to report an error</summary>
        /// <param name="objSource">The object generating the error</param>
        /// <param name="strMethod">The calling method that encountered the error</param>
        /// <param name="strMessage">The error message</param>
        /// <param name="aItems">The collection of items assocaited with the error</param>
        public virtual void FireError(object objSource,string strMethod,string strMessage,CTmaxErrorArgs.CErrorItems aItems)
        {
            FireError(objSource,strMethod,strMessage,aItems,null,null);
        }

        /// <summary>This method is called to report an error</summary>
        /// <param name="objSource">The object generating the error</param>
        /// <param name="strMethod">The calling method that encountered the error</param>
        /// <param name="strMessage">The error message</param>
        public virtual void FireError(object objSource,string strMethod,string strMessage)
        {
            FireError(objSource,strMethod,strMessage,null,null,null);
        }

        /// <summary>This method is called to report an error</summary>
        /// <param name="objSource">The object generating the error</param>
        /// <param name="strMethod">The calling method that encountered the error</param>
        /// <param name="strMessage">The error message</param>
        /// <param name="oleEx">The OLE data provider exception associated with the error</param>
        /// <param name="aItems">The collection of items assocaited with the error</param>
        public virtual void FireError(object objSource,string strMethod,string strMessage,OleDbException oleEx,CTmaxErrorArgs.CErrorItems aItems)
        {
            FireError(objSource,strMethod,strMessage,aItems,oleEx.Message,oleEx.ToString());
        }

        /// <summary>This method is called to report an error</summary>
        /// <param name="objSource">The object generating the error</param>
        /// <param name="strMethod">The calling method that encountered the error</param>
        /// <param name="strMessage">The error message</param>
        /// <param name="oleEx">The OLE data provider exception associated with the error</param>
        public virtual void FireError(object objSource,string strMethod,string strMessage,OleDbException oleEx)
        {
            FireError(objSource,strMethod,strMessage,null,oleEx.Message,oleEx.ToString());
        }

        /// <summary>This method is called to report an error</summary>
        /// <param name="objSource">The object generating the error</param>
        /// <param name="strMethod">The calling method that encountered the error</param>
        /// <param name="strMessage">The error message</param>
        /// <param name="Ex">The system exception associated with the error</param>
        /// <param name="aItems">The collection of items assocaited with the error</param>
        public virtual void FireError(object objSource,string strMethod,string strMessage,System.Exception Ex,CTmaxErrorArgs.CErrorItems aItems)
        {
            FireError(objSource,strMethod,strMessage,aItems,Ex.Message,Ex.ToString());
        }

        /// <summary>This method is called to report an error</summary>
        /// <param name="objSource">The object generating the error</param>
        /// <param name="strMethod">The calling method that encountered the error</param>
        /// <param name="strMessage">The error message</param>
        /// <param name="Ex">The system exception associated with the error</param>
        public virtual void FireError(object objSource,string strMethod,string strMessage,System.Exception Ex)
        {
            FireError(objSource,strMethod,strMessage,null,Ex.Message,Ex.ToString());
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>Called to close the active data provider connection</summary>
        protected virtual void Disconnect()
        {
            //	Close the provider connection
            if (m_oleDbConnection != null)
            {
                try { m_oleDbConnection.Close(); }
                catch { }
                try { m_oleDbConnection.Dispose(); }
                catch { }
                m_oleDbConnection = null;
            }

        }//  protected virtual void Disconnect()

        /// <summary>This method is called to open a connection to the data provider</summary>
        /// <param name="strFileSpec">The path to the database file</param>
        /// <returns>true if successful</returns>
        protected virtual bool Connect(string strFileSpec)
        {
            string strConnection = GetConnectionString(strFileSpec);

            try
            {
                //	Close the existing connection
                Disconnect();

                //	Create the connection object
                m_oleDbConnection = new OleDbConnection(strConnection);

                //	Make sure the connection is open
                if (m_oleDbConnection.State != ConnectionState.Open)
                    m_oleDbConnection.Open();

                return true;
            }
            catch(OleDbException oleEx)
            {
                m_tmaxEventSource.FireError(this,"Connect",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_CONNECT_EX,strConnection),oleEx);
            }
            catch(System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this,"Connect",m_tmaxErrorBuilder.Message(ERROR_BASE_DATABASE_CONNECT_EX,strConnection),Ex);
            }

            return false;

        }// protected virtual bool Connect(string strFileSpec)

        /// <summary>This method constructs the parameter string to open the connection</summary>
        /// <param name="strDataSource">The database to be connected</param>
        /// <returns>The connection string required by the provider</returns>
        protected virtual string GetConnectionString(string strDataSource)
        {
            string strConnect = "";

            //	Start with the provider
            strConnect = "Provider=Microsoft.Jet.OLEDB.4.0;";

            //	Now add the data source
            strConnect += ("Data Source=" + strDataSource);

            /*
                We would use this connection string if we wanted to
                use an OleDbConnection to connect to a SQL Server instead of
                an SqlDbConnection.
				
                Provider=DQLOLEDB.1;
                Integrated Security=SSPI;
                Persist Security Info=false;
                Initial Catalog=Northwind;	// Database name
                Data Source=G61LS;			// Server name
				
                See pg 288 - A Programmer's Guide to ADO.NET in C#
            */
 
            return strConnect;

        }// protected virtual string GetConnectionString()

        /// <summary>This method is called to populate the error builder's format string collection</summary>
        protected virtual void SetErrorStrings() 
        {
            if(m_tmaxErrorBuilder == null) return;
            if(m_tmaxErrorBuilder.FormatStrings == null) return;

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the connection to the database using: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get a data reader using this SQL statement: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to advance the data reader");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while searching for the table named %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while searching for the column named %1 in the table named %2");
            
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attemping to retrieve the values from a query");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the identifier automatically assigned by the database");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the records collection for the table named: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to flush the records collection for the table named %1 using SQL statement: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the the table named %1 using SQL statement: %2");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create column #%1 in the %2 table using SQL statement: %3");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to get SQL statement to add column #%1 to the table named %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to get SQL statement to add the table named %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the auto-increment attribute of column #%1 in the %2 table using SQL statement: %3");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to get SQL statement to set the auto-increment attribute of column #%1 in the table named %2");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a record to the table named %1 using SQL statement: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update a record in the table named %1 using SQL statement: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete a record in the table named %1 using SQL statement: %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the database: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the automatic GUID using: SQL = %1");

        }// protected virtual void SetErrorStrings() 

        #endregion Protected Methods

        #region Properties

        /// <summary>Full path specification for the database file</summary>
         public string FileSpec
        {
            get { return m_strFileSpec; }
        }

        /// <summary>Flag to indicate if the current connection is good</summary>
         public bool IsConnected
        {
            get { return GetIsConnected(); }
        }

        /// <summary>Event source interface for error and diagnostic events</summary>
        public FTI.Shared.Trialmax.CTmaxEventSource EventSource
        {
            get { return m_tmaxEventSource; }
        }

        /// <summary>Event source interface for error and diagnostic events</summary>
        public FTI.Shared.Trialmax.CTmaxErrorBuilder ExBuilder
        {
            get { return m_tmaxErrorBuilder; }
        }

        #endregion Properties

    }// public class CBaseDatabase

}// namespace FTI.Trialmax.Database

