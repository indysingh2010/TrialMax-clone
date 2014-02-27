using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.Database
{
	/// <summary>This class manages the export operations for the database</summary>
	public class CTmaxCodesDatabase
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX					= 0;
		
		private const string FIELDED_DATA_TABLE_NAME = "FieldedData";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>OLE DB data provider connection object</summary>
		private System.Data.OleDb.OleDbConnection m_oleConnection = null;
		
		/// <summary>OLE DB data adapter object</summary>
		private System.Data.OleDb.OleDbDataAdapter m_oleAdapter = null;
		
		/// <summary>The data set used to retrieve the database records</summary>
		private System.Data.DataSet m_dataSet = null;
		
		/// <summary>The data table that contains the records</summary>
		private System.Data.DataTable m_dataTable = null;
		
		/// <summary>Private member bound to Info property</summary>
		private CCxCodesInfo m_cxInfo = null;
		
		/// <summary>Local member bound to ExportOptions property</summary>
		private CTmaxExportOptions m_tmaxExportOptions = null;
		
		/// <summary>Local member bound to CaseDatabase property</summary>
		private CTmaxCaseDatabase m_tmaxCaseDatabase = null;
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxCodesDatabase()
		{
			//	Initialize the event source and error builder
			SetErrorStrings();
			m_tmaxEventSource.Name = "Codes Database";
			
		}// public CTmaxCodesDatabase()
		
		/// <summary>This method is called to open the connection to the database</summary>
		/// <param name="strFileSpec">The path to the database</param>
		/// <returns>true if successful</returns>
		public bool Open(string strFileSpec)
		{
			bool	bSuccessful = false;
			string	strConnection = "";
			
			try
			{
				//	Make sure the existing connection is closed
				Close();
				
				//	Build the connection string
				strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;";
				strConnection += ("Data Source=" + strFileSpec);
			
				//	Create the connection object
				m_oleConnection = new OleDbConnection(strConnection);
							
				//	Make sure the connection is open
				if(m_oleConnection.State != ConnectionState.Open)
					m_oleConnection.Open();
						
				m_strFileSpec = strFileSpec;
				
				if((bSuccessful = (m_oleConnection.State == ConnectionState.Open)) == true)
				{
					//	Does this database contain an Info table?
					if(HasTable(CCxCodesInfo.TABLE_NAME) == true)
					{
						m_cxInfo = new CCxCodesInfo();
						Fill(m_cxInfo);
					}
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Open", Ex);
			}

			return bSuccessful;
			
		}// public bool Open(string strFileSpec)
		
		/// <summary>This method is called to add the codes for the specified record to the database</summary>
		/// <param name="dxRecord">The record that owns the fielded data (codes)</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxMediaRecord dxRecord, CTmaxExportColumns tmaxColumns)
		{
			bool	bSuccessful = false;
//			bool	bConcatenate = false;
			int		iMaxRows = 0;
			
			if(tmaxColumns == null) return false;
			if(tmaxColumns.Count == 0) return false;
			
			try
			{
				//	Make sure we disable concatenation since we are dealing with a database
//				bConcatenate = m_tmaxExportOptions.Concatenate;
//				m_tmaxExportOptions.Concatenate = false;
				
				//	Get the values to be exported
				if((iMaxRows = tmaxColumns.GetValues(dxRecord, m_tmaxExportOptions)) > 0)
				{
					for(int i = 0; i < iMaxRows; i++)
					{
						Add(dxRecord, tmaxColumns, i);
				
					}// for(int i = 0; i < iMaxRows; i++)
			
					//	We don't need the values any more
					tmaxColumns.ClearValues();
							
				}// if((iMaxRows = GetValues(tmaxRecord, tmaxOptions)) > 0)
			
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
			}
			finally
			{
				//	Restore the concatenate option
//				m_tmaxExportOptions.Concatenate = bConcatenate;
			}
			
			return bSuccessful;
			
		}// public bool Add(CDxMediaRecord dxRecord)
		
		/// <summary>This method will close the connection</summary>
		public void Close()
		{
			m_dataTable = null;
			m_strFileSpec = "";
			m_cxInfo = null;
			
			try
			{
				if(m_dataSet != null)
				{
					m_dataSet.Dispose();
					m_dataSet = null;
				}
			
			}
			catch
			{
			}
			
			try
			{
				if(m_oleAdapter != null)
				{
					m_oleAdapter.Dispose();
					m_oleAdapter = null;
				}
			
			}
			catch
			{
			}
			
			try
			{
				//	Close the provider connection
				if(m_oleConnection != null)
				{
					m_oleConnection.Close();
					m_oleConnection.Dispose();
					m_oleConnection = null;
				}
			
			}
			catch
			{
			}
				
		}// public void Close()
					
		/// <summary>This method is called to create the fielded data table</summary>
		/// <returns>true if successful</returns>
		public bool CreateTable(CTmaxExportColumns tmaxColumns)
		{
			string			strSQL = "";
			OleDbCommand	oleCmd = null;
			bool			bSuccessful = false;
			
			if(m_oleConnection == null) return false;
			if(tmaxColumns == null) return false;
			if(tmaxColumns.Count == 0) return false;
			
			try
			{
				while(bSuccessful == false)
				{
					//	Get the SQL statement to create the table					
					strSQL = GetSQLCreate(tmaxColumns);
					if(strSQL.Length == 0)
						break;
						
					//	Create the command object
					oleCmd = new OleDbCommand(strSQL, m_oleConnection);
				
					//	Execute the command
					oleCmd.ExecuteNonQuery();	
				
					bSuccessful = true;
				
				}// while(bSuccessful == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "CreateTable", Ex);
			}
					
			return bSuccessful;
		
		}// public bool CreateTable()
		
		/// <summary>This method is called to get the array of names that define the columns in the fielded data table</summary>
		/// <returns>The array of column names</returns>
		public string [] GetColumns()
		{
			string	  strSQL = "";
			string [] aColumns = null;
			
			try
			{
				m_dataSet = new DataSet(FIELDED_DATA_TABLE_NAME);
						
				strSQL = String.Format("SELECT * FROM {0}", FIELDED_DATA_TABLE_NAME);
					
				m_oleAdapter = new OleDbDataAdapter(strSQL, m_oleConnection);
				m_oleAdapter.Fill(m_dataSet, FIELDED_DATA_TABLE_NAME);
						
				//	Should only be 1 table in the data set but we'll play it safe
				if((m_dataSet.Tables != null) && (m_dataSet.Tables.Count > 0))
				{
					foreach(DataTable table in m_dataSet.Tables)
					{
						//	Is this the requested table?
						if(String.Compare(table.TableName, FIELDED_DATA_TABLE_NAME, true) == 0)
						{
							m_dataTable = table;
							
							//	Get the array of columns
							if((m_dataTable.Columns != null) && (m_dataTable.Columns.Count > 0))
							{
								//	Create the column names array
								aColumns = new string[m_dataTable.Columns.Count];
								
								for(int i = 0; i < m_dataTable.Columns.Count; i++)
								{
									aColumns[i] = m_dataTable.Columns[i].ColumnName;
								}
							
							}// if((m_dataTable.Columns != null) && (m_dataTable.Columns.Count > 0))
							
							break;
							
						}// if(String.Compare(table.TableName, FIELDED_DATA_TABLE_NAME, true) == 0)
						
					}// foreach(DataTable table in dataSet.Tables)
					
				}//	if((dataSet.Tables != null) && (dataSet.Tables.Count > 0))
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetColumns", Ex);
			}
			
			if((aColumns != null) && (aColumns.GetUpperBound(0) >= 0))
				return aColumns;
			else
				return null;
		
		}// public string [] GetColumns()
		
		/// <summary>This method is called to get the array of names that define the columns in the fielded data table</summary>
		/// <param name="iIndex">The index of the row containing the values</param>
		/// <returns>The array of column names</returns>
		public string [] GetValues(int iIndex)
		{
			string [] aValues = null;
			
			try
			{
				if(m_dataTable == null) return null;
				if(m_dataTable.Columns == null) return null;
				if(m_dataTable.Columns.Count == 0) return null;
				if(iIndex < 0) return null;
				if(iIndex >= m_dataTable.Rows.Count) return null;
				
				//	Allocate an array to store the values
				aValues = new string[m_dataTable.Columns.Count];
						
				for(int i = 0; i <= aValues.GetUpperBound(0); i++)
				{
					if(m_dataTable.Rows[iIndex].IsNull(i) == false)
						aValues[i] = m_dataTable.Rows[iIndex].ItemArray[i].ToString();
					else
						aValues[i] = "";

				}// for(int i = 0; i <= aValues.GetUpperBound(0); i++)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetColumns", Ex);
			}
			
			if((aValues != null) && (aValues.GetUpperBound(0) >= 0))
				return aValues;
			else
				return null;
		
		}// public string [] GetValues(int iIndex)
		
		/// <summary>Called to get the number of rows in the FieldedData table</summary>
		/// <returns>The total number of rows</returns>
		public int GetRowCount()
		{
			if((m_dataTable != null) && (m_dataTable.Rows != null))
				return m_dataTable.Rows.Count;
			else
				return 0;
		
		}// public int GetRowCount()
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to add the codes at the specified index to the database</summary>
		/// <param name="dxRecord">The record that owns the fielded data (codes)</param>
		/// <param name="tmaxColumns">The columns being supported</param>
		/// <param name="iIndex">The index into the Values owned by each column</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxMediaRecord dxRecord, CTmaxExportColumns tmaxColumns, int iIndex)
		{
			bool			bSuccessful = false;
			string			strSQL = "";
			OleDbCommand	oleCmd = null;
			
			if(tmaxColumns == null) return false;
			if(tmaxColumns.Count == 0) return false;
			if(iIndex < 0) return false;
		
			try
			{
				while(bSuccessful == false)
				{
					//	Get the SQL statement to create the table					
					strSQL = GetSQLInsert(dxRecord, tmaxColumns, iIndex);
					if(strSQL.Length == 0)
						break;
						
					//	Create the command object
					oleCmd = new OleDbCommand(strSQL, m_oleConnection);
				
					//	Execute the command
					oleCmd.ExecuteNonQuery();	
				
					bSuccessful = true;
				
				}// while(bSuccessful == false)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
			}
			
			return bSuccessful;
			
		}// public bool Add(CDxMediaRecord dxRecord, CTmaxExportColumns tmaxColumns, int iIndex)
		
		/// <summary>This method is called to get the SQL statement required to create the table</summary>
		/// <returns>The appropriate SQL statement</returns>
		private string GetSQLCreate(CTmaxExportColumns tmaxColumns)
		{
			string	strSQL = "";
			string	strColumn = "";

			if((tmaxColumns != null) && (tmaxColumns.Count > 0))
			{
				strSQL = "CREATE TABLE " + FIELDED_DATA_TABLE_NAME;
				strSQL += "(SortOrder AUTOINCREMENT";

				foreach(CTmaxExportColumn O in tmaxColumns)
				{
					strColumn = GetSQLCreate(O);
					
					if(strColumn.Length > 0)
					{
						//	Add the separator
						strSQL += ", ";
						
						strSQL += strColumn;
						
					}// if(strColumn.Length > 0)
				
				}// foreach(CTmaxExportColumn O in tmaxColumns)

				if(strSQL.Length > 0)
					strSQL += ")";
				
			}// if((tmaxColumns != null) && (tmaxColumns.Count > 0))
			
			return strSQL;

		}// private string GetSQLCreate()
		
		/// <summary>This method is called to get the SQL statement required to include the column in the create statement</summary>
		/// <returns>The appropriate SQL statement</returns>
		private string GetSQLCreate(CTmaxExportColumn tmaxColumn)
		{
			//	Columns know how to create the SQL statement
			return tmaxColumn.GetSQLCreate(this.ExportOptions);

		}// private string GetSQLCreate(CTmaxExportColumn tmaxColumn)
		
		/// <summary>This method is called to get the SQL statement to add the codes at the specified index to the database</summary>
		/// <param name="dxRecord">The record that owns the fielded data (codes)</param>
		/// <param name="tmaxColumns">The columns being supported</param>
		/// <param name="iIndex">The index into the Values owned by each column</param>
		/// <returns>true if successful</returns>
		private string GetSQLInsert(CDxMediaRecord dxRecord, CTmaxExportColumns tmaxColumns, int iIndex)
		{
			string	strSQL = "";
			string	strColumns = "";
			string	strValues = "";
			string	strValue = "";
			
			foreach(CTmaxExportColumn O in tmaxColumns)
			{
				//	Do we have a value for this column?
				if((O.Values != null) && (iIndex < O.Values.Count))
				{
					//	Have to convert the strings if boolean
					if((O.CaseCode != null) && (O.CaseCode.Type == TmaxCodeTypes.Boolean))
					{
						//	Only do the conversion if not concatenating this field
						if((O.CaseCode.AllowMultiple == false) || (m_tmaxExportOptions.Concatenate == false))
							strValue = CTmaxToolbox.BoolToSQL(CTmaxToolbox.StringToBool(O.Values[iIndex].ToString()));
						else
							strValue = CTmaxToolbox.SQLEncode(O.Values[iIndex].ToString());
					}
					else
					{
						strValue = CTmaxToolbox.SQLEncode(O.Values[iIndex].ToString());
					}

				}
				else if(O.TmaxEnumId == TmaxExportColumns.Barcode)
				{
					strValue = dxRecord.GetBarcode(false);
				}
				else if(O.TmaxEnumId == TmaxExportColumns.Name)
				{
					strValue = dxRecord.GetName();
				}
				
				//	Do we have a value for this column?
				if(strValue.Length > 0)
				{
					//	Get the name of this column
					if(strColumns.Length > 0)
						strColumns += ",";
					strColumns += ("[" + O.Name + "]");
					
					//	Set the value
					if(strValues.Length > 0)
						strValues += ",";
					strValues += ("'" + strValue + "'");
			
					//	Clear for next column
					strValue = "";
				}
				
			}// foreach(CTmaxExportColumn O in tmaxColumns)
						
			//	Build the SQL statement
			if((strColumns.Length > 0) && (strValues.Length > 0))
			{
				strSQL = "INSERT INTO " + FIELDED_DATA_TABLE_NAME + "(";
				strSQL += strColumns;
				strSQL += ") VALUES(";
				strSQL += strValues;
				strSQL += ");";
			}

			return strSQL;
		
		}// private string GetSQLInsert(CDxMediaRecord dxRecord, CTmaxExportColumns tmaxColumns, int iIndex)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the export operation");

		}// private void SetErrorStrings()

		/// <summary>This method is called to determine if the specified table exists in the database</summary>
		/// <param name="strName">The name of the table</param>
		/// <returns>true if the table exists</returns>
		public bool HasTable(string strName)
		{
			DataTable dbTables = null;
			
			try
			{
				//	Do we have a valid connection?
				if(m_oleConnection == null) return false;
				
				//	Make sure the connection is open
				if(m_oleConnection.State != ConnectionState.Open)
				{
					m_oleConnection.Open();
				
					if(m_oleConnection.State != ConnectionState.Open)
						return false;
				}
					
				//	Get the tables in the database
				dbTables = m_oleConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
					new object[] {null, null, null, "TABLE"});
				foreach(DataRow O in dbTables.Rows)
				{
					if(String.Compare((O["TABLE_NAME"]).ToString(), strName, true) == 0)
						return true;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "HasTable", Ex);
			}
			
			return false;
		
		}// public bool HasTable(string strName)

		/// <summary>This method is called to get the data stored in the Info table</summary>
		/// <param name="cxInfo">The record to be populated</param>
		/// <returns>True if successful</returns>
		private bool Fill(CCxCodesInfo cxInfo)
		{
			string		strSQL = "";
			DataSet		dataSet = null;
			DataTable	dataTable = null;
			bool		bSuccessful = false;
			
			try
			{
				dataSet = new DataSet(CCxCodesInfo.TABLE_NAME);
						
				strSQL = String.Format("SELECT * FROM {0}", CCxCodesInfo.TABLE_NAME);
					
				m_oleAdapter = new OleDbDataAdapter(strSQL, m_oleConnection);
				m_oleAdapter.Fill(dataSet, CCxCodesInfo.TABLE_NAME);
						
				//	Should only be 1 table in the data set but we'll play it safe
				if((dataSet.Tables != null) && (dataSet.Tables.Count > 0))
				{
					foreach(DataTable table in dataSet.Tables)
					{
						//	Is this the requested table?
						if(String.Compare(table.TableName, CCxCodesInfo.TABLE_NAME, true) == 0)
						{
							dataTable = table;
							
							//	Has the table been populated
							if(dataTable.Rows.Count > 0)
							{
								cxInfo.SetProps(dataTable.Rows[0]);
							}
							else
							{
								//	Add the info to the database
								Add(cxInfo);
							}

							bSuccessful = true;
							break;
							
						}// if(String.Compare(table.TableName, FIELDED_DATA_TABLE_NAME, true) == 0)
						
					}// foreach(DataTable table in dataSet.Tables)
					
				}//	if((dataSet.Tables != null) && (dataSet.Tables.Count > 0))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Fill", Ex);
			}
			
			return bSuccessful;
			
		}// private bool Fill(CCxCodesInfo cxInfo)

		/// <summary>This method is called to add the data stored in the Info table</summary>
		/// <param name="cxInfo">The record exchange object</param>
		/// <returns>True if successful</returns>
		private bool Add(CCxCodesInfo cxInfo)
		{
			CTmdataVersion	dbVer = new CTmdataVersion();
			bool			bSuccessful = false;
			string			strSQL = "";
			OleDbCommand	oleCmd = null;
			
			try
			{
				//	Set the information
				cxInfo.Version = dbVer.Version;
				cxInfo.CreatedOn = System.DateTime.Now;
				
				if(m_tmaxExportOptions != null)
				{
					cxInfo.Concatenated = m_tmaxExportOptions.Concatenate;
				
					if(cxInfo.Concatenated == true)
					{
						if(m_tmaxExportOptions.Concatenator != TmaxExportConcatenators.User)
							cxInfo.ConcatenationChars = m_tmaxExportOptions.Concatenator.ToString();
						else
							cxInfo.ConcatenationChars = m_tmaxExportOptions.GetConcatenator();
					}
					
				}
				
				if(m_tmaxCaseDatabase != null)
				{
					if(m_tmaxCaseDatabase.User != null)
						cxInfo.CreatedBy = m_tmaxCaseDatabase.User.Name;
					if(m_tmaxCaseDatabase.Detail != null)
						cxInfo.CaseId = m_tmaxCaseDatabase.Detail.UniqueId;
						
					cxInfo.CaseName = System.IO.Path.GetFileNameWithoutExtension(m_tmaxCaseDatabase.Folder);
					
				}
				
				//	Get the SQL statement to create the table					
				strSQL = cxInfo.GetSQLInsert();

				//	Create the command object
				oleCmd = new OleDbCommand(strSQL, m_oleConnection);
				
				//	Execute the command
				oleCmd.ExecuteNonQuery();	
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "cxInfo", Ex);
			}
			
			return bSuccessful;
			
		}// private bool Add(CCxCodesInfo cxInfo)

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The user defined set of export options</summary>
		public CTmaxExportOptions ExportOptions
		{
			get { return m_tmaxExportOptions; }
			set { m_tmaxExportOptions = value; }
		}

		/// <summary>The active case database</summary>
		public CTmaxCaseDatabase CaseDatabase
		{
			get { return m_tmaxCaseDatabase; }
			set { m_tmaxCaseDatabase = value; }
		}

		/// <summary>The application's collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
			set { m_tmaxCaseCodes = value; }
		}

		/// <summary>Fully qualified path to the active file stream</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		}

		/// <summary>The information retrieved from the database Info table</summary>
		public CCxCodesInfo Info
		{
			get { return m_cxInfo; }
		}

		#endregion Properties	
	
	}// class CTmaxCodesDatabase
	
}// namespace FTI.Trialmax.Database

