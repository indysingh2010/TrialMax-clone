using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Database.Legacy
{
	/// <summary>This class is used to convert a TrialMax depolog database to an XML deposition</summary>
	public class CTmaxDepoLog
	{
		#region Constants
		
		//	Table names
		private string SEGMENTS_TABLE				= "Info";
		private string TRANSCRIPT_TEXT_TABLE		= "Transcript";
		private string SYNC_TIMES_TABLE				= "Log";
		
		private string START_TIME_COLUMN			= "StartTime";
		private string STOP_TIME_COLUMN				= "EndTime";
		
		// Segment table field indexes
		private int SEGMENTS_KEY					= 0;
//		private int SEGMENTS_DRIVE					= 1;
		private int SEGMENTS_FILENAME				= 2;
//		private int SEGMENTS_SOURCE					= 3;
//		private int SEGMENTS_PARTY					= 4;
//		private int SEGMENTS_WITNESSID				= 5;
		private int SEGMENTS_WITNESS				= 6;
//		private int SEGMENTS_VOLUMEID				= 7;
		private int SEGMENTS_LINESPERPAGE			= 8;
		private int SEGMENTS_STARTPAGE				= 9;
		private int SEGMENTS_STARTLINE				= 10;
//		private int SEGMENTS_ENDPAGE				= 11;
		private int SEGMENTS_STARTFRAME				= 12;
		private int SEGMENTS_ENDFRAME				= 13;

		//	Sync times table field identifiers
//		private int SYNC_TIMES_KEY					= 0;
		private int SYNC_TIMES_PAGE					= 1;
		private int SYNC_TIMES_LINE					= 2;
		private int SYNC_TIMES_HOURS				= 3;
		private int SYNC_TIMES_MINUTES				= 4;
		private int SYNC_TIMES_SECONDS				= 5;
		private int SYNC_TIMES_FRAMES				= 6;
//		private int SYNC_TIMES_FRAMECNT				= 7;
//		private int SYNC_TIMES_PAUSE				= 8;
//		private int SYNC_TIMES_PLAYBACK				= 9;
		private int SYNC_TIMES_START_TIME			= 10;
		private int SYNC_TIMES_STOP_TIME			= 11;

		//	Transcript text table field identifiers
//		private int TRANSCRIPTS_KEY					= 0;
		private int TRANSCRIPTS_PAGE				= 1;
		private int TRANSCRIPTS_LINE				= 2;
		private int TRANSCRIPTS_TEXT				= 3;

		protected const float FRAMES_PER_SECOND		= 30.00f;
		protected const double	SECONDS_PER_LINE	= 2.0;
		
		protected const int ERROR_OPEN_OLE_EX				= 0;
		protected const int	ERROR_OPEN_EX					= 1;
		protected const int ERROR_GET_READER_OLE_EX			= 2;
		protected const int	ERROR_GET_READER_EX				= 3;
		protected const int ERROR_GET_SEGMENTS_OLE_EX		= 4;
		protected const int	ERROR_GET_SEGMENTS_EX			= 5;
		protected const int ERROR_GET_TRANSCRIPTS_OLE_EX	= 6;
		protected const int	ERROR_GET_TRANSCRIPTS_EX		= 7;
		protected const int ERROR_GET_SYNC_TIMES_OLE_EX		= 8;
		protected const int	ERROR_GET_SYNC_TIMES_EX			= 9;
		protected const int ERROR_CHECK_TABLES_OLE_EX		= 10;
		protected const int	ERROR_CHECK_TABLES_EX			= 11;
		protected const int ERROR_GET_SEGMENT_INVALID		= 12;
		protected const int ERROR_GET_SYNC_TIME_INVALID		= 13;
		protected const int ERROR_GET_TRANSCRIPT_INVALID	= 14;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Fully qualified path to the log file</summary>
		private string m_strFileSpec = "";
		
		/// <summary>OLE DB data provider connection object</summary>
		private System.Data.OleDb.OleDbConnection m_oleDbConnection = null;
		
		/// <summary>Local member bound XmlDeposition property</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = new CXmlDeposition();
		
		/// <summary>Local member to indicate that the deposition object requires initialization</summary>
		private bool m_bInitializeDeposition = true;
		
		/// <summary>Local member to indicate that the sync time columns are present in the log file</summary>
		private bool m_bSyncTimeColumns = false;
		
		/// <summary>Local collection for storage of non-fatal errors</summary>
		private ArrayList m_aErrorMessages = new ArrayList();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This is the delegate used to handle all progress events</summary>
		/// <param name="objSender">Object firing the event</param>
		/// <param name="strProgress">Progress text</param>
		public delegate void DepoLogProgressHandler(object objSender, string strProgress);
		
		/// <summary>This event is fired to report conversion progress</summary>
		public event DepoLogProgressHandler DepoLogProgressEvent;
		
		/// <summary>Default constructor</summary>
		public CTmaxDepoLog()
		{
			m_tmaxEventSource.Name = "DepoLog Converter";
			
			SetErrorStrings();
		}
		
		/// <summary>Called to convert the specified database to an XML deposition</summary>
		/// <param name="strFileSpec">Fully qualified path to the depo log</param>
		/// <returns>true if successful</returns>
		public bool Convert(string strFileSpec)
		{
			string strMsg = "";
			
			//	Close the existing database connection
			Close();
			
			m_strFileSpec = strFileSpec;
			
			strMsg = String.Format("Converting {0} ...", m_strFileSpec);
			FireProgress(strMsg);
			
			//	Connect to the data provider
			if(Open() == false) return false;
			
			FireProgress("Checking tables ...");
			
			//	Reset this flag to force initialization of the deposition object
			m_bInitializeDeposition = true;
			
			//	Verify that all the tables are present
			if(CheckTables() == false) return false;
					
			//	Check to see if sync time columns are present
			m_bSyncTimeColumns = CheckTimeColumns();

			FireProgress("Converting segment information ...");
			
			//	Get the segment information
			if(GetSegments() == false) return false;
					
			FireProgress("Converting transcript text records ...");
			
			//	Get the transcript text
			if(GetTranscripts() == false) return false;
			
			FireProgress("Converting transcript sync times ...");
			
			//	Syncronize the transcript text for each segment
			GetSyncTimes();

			FireProgress("Resolving unsynchronized text ...");
			
			//	Store any unsynchronized transcript text in the appropriate segment
			ResolveUnSynchronized();

			if(m_xmlDeposition.Name.Length == 0)
			{
				m_xmlDeposition.Name = System.IO.Path.GetFileNameWithoutExtension(m_strFileSpec);
				m_xmlDeposition.Deponent = m_xmlDeposition.Name;
			}
			
			FireProgress("Conversion complete");
			
			return true;
		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to open a connection with the database</summary>
		/// <returns>true if successful</returns>
		private bool Open()
		{
			string strConnection = "";
			
			try
			{
				//	Build the connection string
				strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;";
				strConnection += ("Data Source=" + m_strFileSpec);
			
				//	Create the connection object
				m_oleDbConnection = new OleDbConnection(strConnection);
							
				//	Make sure the connection is open
				if(m_oleDbConnection.State != ConnectionState.Open)
					m_oleDbConnection.Open();
						
				return true;
			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_OLE_EX, strConnection), oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_EX, strConnection), Ex);
			}
			
			return false;
		
		}// private bool Open()
			
		/// <summary>Call to close the database</summary>
		private void Close()
		{
			try
			{
				//	Close the provider connection
				if(m_oleDbConnection != null)
				{
					m_oleDbConnection.Close();
					m_oleDbConnection.Dispose();
					m_oleDbConnection = null;
				}
				
				if(m_aErrorMessages != null)
					m_aErrorMessages.Clear();
			
			}
			catch
			{
			}
			
		}// private void Close()
		
		/// <summary>This method is called to determine if the required tables are present</summary>
		/// <returns>true if successful</returns>
		private bool CheckTables()
		{
			DataTable	dbTables = null;
			string		strMsg = "";
			bool		bSuccessful = false;
			
			try
			{
				//	Get the tables in the database
				dbTables = m_oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
															     new object[] {null, null, null, "TABLE"});
				while(bSuccessful == false)
				{
					//	Initialize the error message
					strMsg = (m_strFileSpec.ToLower() + " is not a valid TrialMax log file.");
					
					//	Check for tables found in all TrialMax log files
					if(FindTable(dbTables, SEGMENTS_TABLE) == false)
						break;
					if(FindTable(dbTables, SYNC_TIMES_TABLE) == false)
						break;
						
					//	Check for the transcript text table
					strMsg = (m_strFileSpec.ToLower() + " is an outdated TrialMax log file. Contact the Houston office for a newer version."); 	
					if(FindTable(dbTables, TRANSCRIPT_TEXT_TABLE) == false)
						break;
						
					bSuccessful = true;
				}
				
				//	Fire an error if necessary
				if(bSuccessful == false)
					m_tmaxEventSource.FireError(this, "CheckTables", strMsg);
					
				return bSuccessful;
								
			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this, "CheckTables", m_tmaxErrorBuilder.Message(ERROR_CHECK_TABLES_OLE_EX, m_strFileSpec), oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckTables", m_tmaxErrorBuilder.Message(ERROR_CHECK_TABLES_EX, m_strFileSpec), Ex);
			}
			
			return false;
		
		}// private bool CheckTables()
			
		/// <summary>This method is called to determine if the start/stop time columns are present in the log file</summary>
		/// <returns>true if the time columns exist</returns>
		public bool CheckTimeColumns()
		{
			DataSet				dataSet = null;
			string				strSQL = "";
			OleDbDataAdapter	oleDbAdapter = null;
			bool				bStartTime = false;
			bool				bStopTime = false;
			
			try
			{
				dataSet = new DataSet("Depolog_conversion");
						
				strSQL = String.Format("SELECT * FROM {0}", SYNC_TIMES_TABLE);
					
				oleDbAdapter = new OleDbDataAdapter(strSQL, m_oleDbConnection);
				oleDbAdapter.Fill(dataSet, SYNC_TIMES_TABLE);
						
				//	Should only be 1 table in the data set
				if((dataSet.Tables != null) && (dataSet.Tables.Count == 1))
				{
					//	Search for the requested columns
					if((dataSet.Tables[0].Columns != null) && (dataSet.Tables[0].Columns.Count > 0))
					{
						foreach(DataColumn O in dataSet.Tables[0].Columns)
						{
							if(String.Compare(O.ColumnName, START_TIME_COLUMN, true) == 0)
							{
								bStartTime = true;
								if(bStopTime == true) break;
							}
							else if(String.Compare(O.ColumnName, STOP_TIME_COLUMN, true) == 0)
							{
								bStopTime = true;
								if(bStartTime == true) break;
							}
							
						}// foreach(DataColumn O in dataSet.Tables[0].Columns)
							
					}// if((table.Columns != null) && (table.Columns.Count > 0))
					
				}//	if((dataSet.Tables != null) && (dataSet.Tables.Count == 1))
				
			}
			catch
			{
			}
			
			return ((bStartTime == true) && (bStopTime == true));
		
		}// public bool HasColumn(string strTable, string strColumn)

		/// <summary>This method is called to determine if the specified table exists in the schema</summary>
		/// <param name="dtSchema">The schema containing the table names</param>
		/// <param name="strTable">The name of the table to be located</param>
		/// <returns>True if the table exists</returns>
		private bool FindTable(DataTable dtSchema, string strTable)
		{
			foreach(DataRow dr in dtSchema.Rows)
			{
				if(dr["TABLE_NAME"] != null)
				{
					if(String.Compare(dr["TABLE_NAME"].ToString(), strTable, true) == 0)
						return true;
				}
			}
			
			return false;
		
		}// private bool FindTable(DataTable dtSchema, string strTable)

		/// <summary>This method is called to create a data reader for the appropriate provider</summary>
		/// <param name="strTable">The name of the table being opened</param>
		/// <param name="strSQL">The SQL Select state used to create the reader</param>
		/// <returns>true if successful</returns>
		private OleDbDataReader GetOleReader(string strTable, string strSQL)
		{
			OleDbDataReader oleReader = null;
			
			Debug.Assert(strSQL != null);
			Debug.Assert(strSQL.Length > 0);
			
			try
			{
				//	Create the reader object
				OleDbCommand oleCmd = new OleDbCommand(strSQL, m_oleDbConnection);
				oleReader = oleCmd.ExecuteReader();	
				
				return oleReader;
			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this, "GetOleReader", m_tmaxErrorBuilder.Message(ERROR_GET_READER_OLE_EX, strSQL, m_strFileSpec), oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetOleReader", m_tmaxErrorBuilder.Message(ERROR_GET_READER_EX, strSQL, m_strFileSpec), Ex);
			}
			
			return null;
		
		}// private OleDbDataReader GetOleReader(string strTable, string strSQL)
		
		/// <summary>This method will retrieve the transcript text stored in the database</summary>
		/// <returns>true if successful</returns>
		private bool GetTranscripts()
		{
			OleDbDataReader oleReader = null;
			string			strSQL = "";
			CXmlTranscript	xmlTranscript = null;
			bool			bSuccessful = false;
			int				iRow = 0;
			
			Debug.Assert(m_xmlDeposition != null);

			try
			{
				//	Create the SQL SELECT statement
				strSQL = "SELECT * FROM ";
				strSQL += TRANSCRIPT_TEXT_TABLE;
			
				//	Load the transcript records
				if((oleReader = GetOleReader(TRANSCRIPT_TEXT_TABLE, strSQL)) == null)
					return false;
					
				//	Make sure error message collection is empty
				m_aErrorMessages.Clear();

				while((oleReader != null) && (oleReader.Read() == true))
				{
					//	Increment the row counter
					iRow = iRow + 1;
					
					if((xmlTranscript = GetTranscript(oleReader, iRow)) != null)
					{
						m_xmlDeposition.Transcripts.Add(xmlTranscript);
					}
					
				}// while((oleReader != null) && (oleReader.Read() == true))
				
				//	Fire pending error events. These are considered non-fatal
				//	but they should be reported to the user
				FireErrors("GetTranscript");
				
				//	Make sure they are sorted
				m_xmlDeposition.Transcripts.Sort();
				
				bSuccessful = true;				

			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", m_tmaxErrorBuilder.Message(ERROR_GET_TRANSCRIPTS_OLE_EX, m_strFileSpec), oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", m_tmaxErrorBuilder.Message(ERROR_GET_TRANSCRIPTS_EX, m_strFileSpec), Ex);
			}
			finally
			{
				if(oleReader != null)
					oleReader.Close();
			}
			
			return bSuccessful;
		
		}// private bool GetTranscripts()
		
		/// <summary>This method will use the current record to initialize a new XML transcript</summary>
		/// <param name="oleReader">The OLE database reader positioned on the record</param>
		/// <param name="iRow">The index of the active row</param>
		/// <returns>The new XML transcript if successful</returns>
		private CXmlTranscript GetTranscript(OleDbDataReader oleReader, int iRow)
		{
			int				iPage;
			int				iLine;
			string			strText;
			CXmlTranscript	xmlTranscript = null;
			bool			bValid = true;
			string			strSpaces = " ";
			
			Debug.Assert(m_xmlDeposition != null);

			// Read all the required fields
			try { iPage = System.Convert.ToInt32(oleReader.GetValue(TRANSCRIPTS_PAGE).ToString()); }
			catch { iPage = -1; bValid = false; }
			
			try { iLine = System.Convert.ToInt32(oleReader.GetValue(TRANSCRIPTS_LINE).ToString()); }
			catch { iLine = -1; bValid = false; }
			
			try { strText = oleReader.GetValue(TRANSCRIPTS_TEXT).ToString(); }
			catch { strText = ""; bValid = false; }
			
			//	Stop here if not a valid record
			if(bValid == false)
			{
				//	Add an error to the local collection
				if(m_aErrorMessages != null)
					m_aErrorMessages.Add(m_tmaxErrorBuilder.Message(ERROR_GET_TRANSCRIPT_INVALID, iRow, TRANSCRIPT_TEXT_TABLE));
				
				return null;
			}
			
			//	Allocate and initialize a new transcript object
			xmlTranscript      = new CXmlTranscript();
			xmlTranscript.Page = iPage;
			xmlTranscript.Line = iLine;
			xmlTranscript.PL   = CTmaxToolbox.GetPL(iPage, iLine);
				
			strText = strText.TrimStart(strSpaces.ToCharArray());
					
			if(strText.StartsWith("Q. ") == true)
			{
				xmlTranscript.QA = "Q";
				xmlTranscript.Text = strText.Remove(0, 3);
			}
			else if(strText.StartsWith("Q ") == true)
			{
				xmlTranscript.QA = "Q";
				xmlTranscript.Text = strText.Remove(0, 2);
			}
			else if(strText.StartsWith("A. ") == true)
			{
				xmlTranscript.QA = "A";
				xmlTranscript.Text = strText.Remove(0, 3);
			}
			else if(strText.StartsWith("A ") == true)
			{
				xmlTranscript.QA = "A";
				xmlTranscript.Text = strText.Remove(0, 2);
			}
			else
			{
				xmlTranscript.Text = oleReader.GetValue(3).ToString();
			}
					
			return xmlTranscript;
		
		}// private CXmlTranscript GetTranscript(OleDbDataReader oleReader, int iRow)
		
		/// <summary>This method will retrieve the times for each transcript in the specified segment</summary>
		/// <param name="xmlSegment">The segment that owns the transcripts</param>
		/// <returns>true if successful</returns>
		private bool GetSyncTimes(CXmlSegment xmlSegment)
		{
			OleDbDataReader oleReader = null;
			string			strSQL = "";
			bool			bSuccessful = false;
			int				iRow = 0;
			
			Debug.Assert(m_xmlDeposition != null);
			Debug.Assert(xmlSegment != null);
			
			try
			{
				//	Create the SQL SELECT statement
				strSQL  = "SELECT * FROM ";
				strSQL += SYNC_TIMES_TABLE;
				strSQL += " WHERE Key = ";
				strSQL += xmlSegment.Key;
				strSQL += " ORDER BY Page,Line";
				strSQL += ";";
			
				//	Get the lines for this segment
				if((oleReader = GetOleReader(SYNC_TIMES_TABLE, strSQL)) == null)
					return false;
					
				//	Make sure error message collection is empty
				m_aErrorMessages.Clear();
				
				while((oleReader != null) && (oleReader.Read() == true))
				{
					//	Increment the row counter
					iRow = iRow + 1;
					
					//	Read the database fields
					GetSyncTime(xmlSegment, oleReader, iRow);
				}
				
				//	Fire pending error events. These are considered non-fatal
				//	but they should be reported to the user
				FireErrors("GetSycTime");

				bSuccessful = true;
				
			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this, "GetSyncTimes", m_tmaxErrorBuilder.Message(ERROR_GET_SYNC_TIMES_OLE_EX, m_strFileSpec), oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSyncTimes", m_tmaxErrorBuilder.Message(ERROR_GET_SYNC_TIMES_EX, m_strFileSpec), Ex);
			}
			finally
			{
				if(oleReader != null)
					oleReader.Close();
			}
			
			return bSuccessful;
		
		}// private bool GetSyncTimes(CXmlSegment xmlSegment)
		
		/// <summary>This method will get the synchronized time for the transcript referenced by the current recordr</summary>
		/// <param name="oleReader">The database reader used to get the time information</param>
		/// <param name="xmlSegment">The segment that owns the transcript</param>
		/// <param name="iRow">The active row identifier</param>
		/// <returns>true if successful</returns>
		private bool GetSyncTime(CXmlSegment xmlSegment, OleDbDataReader oleReader, int iRow)
		{
			long			lPL;
			int				iIndex = -1;
			int				iPage;
			int				iLine;
			int				iHours;
			int				iMinutes;
			int				iSeconds;
			int				iFrames;
			double			dStart = -1.0;
			double			dStop = -1.0;
			CXmlTranscript	xmlTranscript = null;
			bool			bValid = true;
			
			Debug.Assert(m_xmlDeposition != null);
			Debug.Assert(xmlSegment != null);
			
			// Read all the required fields
			try	{ iPage = System.Convert.ToInt32(oleReader.GetValue(SYNC_TIMES_PAGE).ToString()); }
			catch { iPage = -1; bValid = false; }

			try	{ iLine = System.Convert.ToInt32(oleReader.GetValue(SYNC_TIMES_LINE).ToString()); }
			catch { iLine = -1; bValid = false; }
			
			try	{ iHours = System.Convert.ToInt32(oleReader.GetValue(SYNC_TIMES_HOURS).ToString()); }
			catch { iHours = -1; bValid = false; }
			
			try	{ iMinutes = System.Convert.ToInt32(oleReader.GetValue(SYNC_TIMES_MINUTES).ToString()); }
			catch { iMinutes = -1; bValid = false; }

			try	{ iSeconds = System.Convert.ToInt32(oleReader.GetValue(SYNC_TIMES_SECONDS).ToString()); }
			catch { iSeconds = -1; bValid = false; }

			try	{ iFrames = System.Convert.ToInt32(oleReader.GetValue(SYNC_TIMES_FRAMES).ToString()); }
			catch { iFrames = 0; }  // Let this slide - they can tune it out
					
			//	Is this one of the newer log files containing start/stop times?
			if(m_bSyncTimeColumns == true)
			{
				try { dStart = System.Convert.ToDouble(oleReader.GetValue(SYNC_TIMES_START_TIME).ToString()); }
				catch {};
				try { dStop = System.Convert.ToDouble(oleReader.GetValue(SYNC_TIMES_STOP_TIME).ToString()); }
				catch {};
			}
			
			//	Stop here if this is not a valid sync time record
			if(bValid == false) 
			{
				//	Add an error to the local collection
				if(m_aErrorMessages != null)
					m_aErrorMessages.Add(m_tmaxErrorBuilder.Message(ERROR_GET_SYNC_TIME_INVALID, iRow, SYNC_TIMES_TABLE));
				
				return false;
			}
			
			//	Locate the transcript
			lPL = CTmaxToolbox.GetPL(iPage, iLine);
			if((iIndex = m_xmlDeposition.Transcripts.Locate(-1, lPL, true)) >= 0)
			{
				xmlTranscript = m_xmlDeposition.Transcripts[iIndex];
						
				//	Do we need to calculate the start time?
				if(dStart < 0)
				{
					dStart = ((double)iHours * 3600.0);
					dStart += ((double)iMinutes * 60.0);
					dStart += (double)iSeconds;
					dStart += ((double)iFrames / FRAMES_PER_SECOND);
				}
				
				xmlTranscript.Segment = xmlSegment.Key;
				xmlTranscript.Start = dStart;
				xmlTranscript.Synchronized = true;
								
				//	Did we get a stop time from the file?
				if((dStop >= 0) && (dStop >= dStart))
					xmlTranscript.Stop = dStop;
					
				xmlSegment.Transcripts.Add(xmlTranscript);
				m_xmlDeposition.Transcripts.Remove(xmlTranscript);
			
				return true;			
			}
			else
			{
				//	Don't bother with error message				
				return false;
			}
		
		}// private bool GetSyncTime(CXmlSegment xmlSegment, OleDbDataReader oleReader)
		
		/// <summary>This method will resolve all transcript lines that were not synchronized</summary>
		/// <returns>true if successful</returns>
		private bool ResolveUnSynchronized()
		{
			bool		bSuccessful = false;
			CXmlSegment	xmlSegment = null;
			
			Debug.Assert(m_xmlDeposition != null);
			if(m_xmlDeposition == null) return false;
			Debug.Assert(m_xmlDeposition.Transcripts != null);
			if(m_xmlDeposition.Transcripts == null) return false;
			
			try
			{
				//	Don't bother if no transcripts remain after retrieving the timing information
				if(m_xmlDeposition.Transcripts.Count == 0) return true;
				
				//	Resolve each transcript line remaining in the collection
				foreach(CXmlTranscript O in m_xmlDeposition.Transcripts)
				{
					//	Get the segment where this line should be stored
					if((xmlSegment = GetXmlSegment(O)) != null)
					{
						//	Set the playback times
						if(SetTimes(O, xmlSegment) == true)
						{
							//	Add it to the collection
							O.Segment = xmlSegment.Key;
							xmlSegment.Transcripts.Add(O);
							
							//	Mark this line as not having been synchronized
							O.Synchronized = false;
						}
						
					}// if((xmlSegment = GetXmlSegment(O.PL)) != null)
				
				}// foreach(CXmlTranscript O in m_xmlDeposition.Transcripts)
				
				//	Make sure each segment is sorted and the extents are updated
				foreach(CXmlSegment O in m_xmlDeposition.Segments)
				{
					SetExtents(O, false);
				
				}// foreach(CXmlSegment O in m_xmlDeposition.Segments)
			
				//	Flush the transcripts collection now that all have been 
				//	transferred to their appropriate sections
				m_xmlDeposition.Transcripts.Clear();
				
				bSuccessful = true;
				
			}
			catch
			{
			}
			
			return bSuccessful;
		
		}// private bool ResolveUnSynchronized()
		
		/// <summary>This method is called to locate the XML segment where the specified transcript should be stored</summary>
		/// <param name="xmlTranscript">The transcript to be stored</param>
		/// <returns>The segment that should contain the specified transcript</returns>
		private CXmlSegment GetXmlSegment(CXmlTranscript xmlTranscript)
		{
			CXmlSegment xmlPrevious = null;
			
			Debug.Assert(m_xmlDeposition != null);
			if(m_xmlDeposition == null) return null;
			Debug.Assert(m_xmlDeposition.Segments != null);
			if(m_xmlDeposition.Segments == null) return null;
			
			if(m_xmlDeposition.Segments.Count == 0) return null;
			
			//	Locate the appropriate segment
			foreach(CXmlSegment O in m_xmlDeposition.Segments)
			{
				//	We use FirstPL for intial test because that's what's
				//	actually stored in the depolog
				if(xmlTranscript.PL < O.FirstPL)
				{
					if(xmlPrevious != null)
						return xmlPrevious;
					else
						return O;
				}
				else
				{
					xmlPrevious = O;
				}
				
			}// foreach(CXmlSegment O in m_xmlDeposition.Segments)
			
			//	If we made it this far we must want the last segment
			return m_xmlDeposition.Segments[m_xmlDeposition.Segments.Count - 1];
		
		}// private CXmlSegment GetXmlSegment(CXmlTranscript xmlTranscript)

		/// <summary>This method is called to set the start and stop times for the unsynchronized transcript object specified by the caller</summary>
		/// <param name="xmlTranscript">The unsynchronized transcript</param>
		/// <param name="xmlSegment">The segment that owns the transcript line</param>
		/// <returns>True if successful</returns>
		private bool SetTimes(CXmlTranscript xmlTranscript, CXmlSegment xmlSegment)
		{
			bool bSuccessful = false;
			
			Debug.Assert(xmlTranscript != null);
			if(xmlTranscript == null) return false;
			Debug.Assert(xmlSegment.Transcripts != null);
			if(xmlSegment.Transcripts == null) return false;

			try
			{
				if(xmlSegment.Transcripts.Count >= 0)
				{
					if(xmlTranscript.PL <= xmlSegment.Transcripts[0].PL)
					{
						xmlTranscript.Start = xmlSegment.Transcripts[0].Start;
					}
					else if(xmlTranscript.PL >= xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].PL)
					{
						xmlTranscript.Start = xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].Stop;
					}
					else
					{
						//	Locate the closest transcript
						foreach(CXmlTranscript O in xmlSegment.Transcripts)
						{
							if(xmlTranscript.PL <= O.PL)
							{
								xmlTranscript.Start = O.Start;
								break;
							}
							
						}
				
					}
					
					//	Set the stop time to match the start
					xmlTranscript.Stop = xmlTranscript.Start;
				}
				else
				{
					//	This shouldn't happen but just in case...
					xmlTranscript.Start = xmlSegment.Start;
					xmlTranscript.Stop = xmlSegment.Stop;
				}	
				
				bSuccessful = true;
				
			}
			catch
			{
			}
		
			return bSuccessful;
					
		}// private bool SetTimes(CXmlTranscript xmlTranscript, CXmlSegment xmlSegment)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			m_tmaxErrorBuilder.FormatStrings.Add("An OLE exception was raised while attempting to open the DepoLog database connection. connection string = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("A system exception was raised while attempting to open the DepoLog database connection. connection string = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An OLE exception was raised while attempting to open the OLE data reader for the DepoLog: SQL = %1 Filename = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("A system exception was raised while attempting to open the OLE data reader for the DepoLog: SQL = %1 Filename = %2");

			m_tmaxErrorBuilder.FormatStrings.Add("An OLE exception was raised while attempting to populate the segments collection from the DepoLog: Filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("A system exception was raised while attempting to populate the segments collection from the DepoLog: Filename = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An OLE exception was raised while attempting to populate the transcripts collection from the DepoLog: Filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("A system exception was raised while attempting to populate the transcripts collection from the DepoLog: Filename = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An OLE exception was raised while attempting to retrieve the transcript times from the DepoLog: Filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("A system exception was raised while attempting to retrieve the transcript times from the DepoLog: Filename = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An OLE exception was raised while attempting to verify the tables contained in the DepoLog: Filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("A system exception was raised while attempting to verify the tables contained in the DepoLog: Filename = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An invalid record was encountered on row %1 in the %2 table - The Key or Filename is not valid");
			m_tmaxErrorBuilder.FormatStrings.Add("An invalid record was encountered on row %1 in the %2 table - The Page/Line or Time is not valid");
			m_tmaxErrorBuilder.FormatStrings.Add("An invalid record was encountered on row %1 in the %2 table - One or more of the fields is not valid");

		}// private void SetErrorStrings()

		/// <summary>This method will fire a progress event</summary>
		/// <param name="strMsg">The progress message</param>
		private void FireProgress(string strMsg)
		{
			try
			{
				if(DepoLogProgressEvent != null)
					DepoLogProgressEvent(this, strMsg);
			}
			catch
			{
			}
			
		}
		
		/// <summary>This method set the start and stop extents for the XML segment</summary>
		/// <param name="xmlSegment">The segment to be updated</param>
		/// <param name="bSetTranscripts">true to update transcript stop positions at the same time</param>
		private void SetExtents(CXmlSegment xmlSegment, bool bSetTranscripts)
		{
			long lFirstPL = 0;
			long lLastPL = 0;
			
			Debug.Assert(xmlSegment != null);
			if(xmlSegment == null) return;
			
			if((xmlSegment.Transcripts != null) && (xmlSegment.Transcripts.Count > 0))
			{
				//	Make sure the transcripts are sorted by PL value
				xmlSegment.Transcripts.Sort();
					
				//	Keep them sorted for ongoing operations
				//
				//	NOTE:	This is required for resolving unsynchronized transcript text properly
				xmlSegment.Transcripts.KeepSorted = true;
				
				//	Should we set the stop position for the transcripts first?
				if(bSetTranscripts == true)
				{
					for(int i = 1; i < xmlSegment.Transcripts.Count; i++)
					{
						//	Only if stop time not read directly from the file
						if(xmlSegment.Transcripts[i - 1].Stop <= 0)
							xmlSegment.Transcripts[i - 1].Stop = xmlSegment.Transcripts[i].Start;
					}
							
					if(xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].Stop <= 0)
						xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].Stop = xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].Start + SECONDS_PER_LINE;
				
				}// if(bSetTranscripts == true)
				
				//	Set the segment extents
				lFirstPL = CTmaxToolbox.GetPL(xmlSegment.Transcripts[0].Page, xmlSegment.Transcripts[0].Line);
				lLastPL  = CTmaxToolbox.GetPL(xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].Page, xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].Line);
				
				if(lFirstPL < xmlSegment.FirstPL)
					xmlSegment.FirstPL = lFirstPL;
				if(lLastPL > xmlSegment.LastPL)
					xmlSegment.LastPL = lLastPL;
				if(xmlSegment.Stop <= 0)
					xmlSegment.Stop = xmlSegment.Transcripts[xmlSegment.Transcripts.Count - 1].Stop;
			}
			else
			{
				//	Assign default extents
				xmlSegment.FirstPL = 0;
				xmlSegment.LastPL = 0;
				if(xmlSegment.Start < 0)
					xmlSegment.Start = 0;
				if(xmlSegment.Stop < 0)
					xmlSegment.Stop = 0;
			}
		
		}// private void SetExtents(CXmlSegment xmlSegment)
		
		/// <summary>This method is called to get the sync times for all transcript records</summary>
		private void GetSyncTimes()
		{
			//	Get the sync times stored in the Log table of the database
			foreach(CXmlSegment O in m_xmlDeposition.Segments)
			{
				//	Get the sync times logged in the database and transfer the
				//	transcript lines to their owner segment
				GetSyncTimes(O);
				
				//	Use the transcripts to set the extents for this segment
				SetExtents(O, true);
			
			}// foreach(CXmlSegment O in m_xmlDeposition.Segments)

			//	Make sure that the segments are sorted now that their extents have been set
			m_xmlDeposition.Segments.Sort(true);

		}// private void GetSyncTimes()
		
		/// <summary>This method will construct the collection of segments belonging to the deposition</summary>
		/// <returns>true if successful</returns>
		private bool GetSegments()
		{
			OleDbDataReader oleReader = null;
			CXmlSegment		xmlSegment = null;
			string			strSQL = "";
			bool			bSuccessful = false;
			int				iRow = 0;
			
			Debug.Assert(m_xmlDeposition != null);
			
			try
			{
				//	Create the SQL SELECT statement
				strSQL  = "SELECT * FROM ";
				strSQL += SEGMENTS_TABLE;
				strSQL += " ORDER BY Key";
				strSQL += ";";
			
				//	Load the segment records
				if((oleReader = GetOleReader(SEGMENTS_TABLE, strSQL)) == null)
					return false;
					
				//	Make sure error message collection is empty
				m_aErrorMessages.Clear();
				
				while((oleReader != null) && (oleReader.Read() == true))
				{
					iRow = iRow + 1;
					
					if((xmlSegment = GetSegment(oleReader, iRow)) != null)
					{
						m_xmlDeposition.Segments.Add(xmlSegment);
					}
					
				}// while((oleReader != null) && (oleReader.Read() == true))
				
				//	Fire pending error events. These are considered non-fatal
				//	but they should be reported to the user
				FireErrors("GetSegment");

				bSuccessful = true;
				
			}
			catch(OleDbException oleEx)
			{
				m_tmaxEventSource.FireError(this, "GetSegments", m_tmaxErrorBuilder.Message(ERROR_GET_SEGMENTS_OLE_EX, m_strFileSpec), oleEx);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSegments", m_tmaxErrorBuilder.Message(ERROR_GET_SEGMENTS_EX, m_strFileSpec), Ex);
			}
			finally
			{
				if(oleReader != null)
					oleReader.Close();
			}
			
			return bSuccessful;
		
		}// private bool GetSegments()
		
		/// <summary>This method will use the current record to initialize a new XML segment</summary>
		/// <param name="oleReader">The OLE database reader positioned on the record</param>
		/// <param name="iRow">The index of the active row</param>
		/// <returns>The new XML segment if successful</returns>
		private CXmlSegment GetSegment(OleDbDataReader oleReader, int iRow)
		{
			string			strKey;
			string			strFilename;
			string			strWitness;
			long			lStartFrame;
			long			lEndFrame;
			int				iLinesPerPage;
			int				iStartPage;
			int				iStartLine;
			CXmlSegment		xmlSegment = null;
			char[]			acDelimiters = {'\t',' '};
			
			Debug.Assert(m_xmlDeposition != null);
			
			// Read all the required fields
			try	{ strKey = oleReader.GetValue(SEGMENTS_KEY).ToString(); }
			catch { strKey = "-1"; }

			try { strFilename = oleReader.GetValue(SEGMENTS_FILENAME).ToString(); }
			catch { strFilename = ""; }
			
			try { iStartPage = System.Convert.ToInt32(oleReader.GetValue(SEGMENTS_STARTPAGE).ToString()); }
			catch { iStartPage = -1; }
			
			try { iStartLine = System.Convert.ToInt32(oleReader.GetValue(SEGMENTS_STARTLINE).ToString()); }
			catch { iStartLine = -1; }
			
			try { lStartFrame = System.Convert.ToInt64(oleReader.GetValue(SEGMENTS_STARTFRAME).ToString()); }
			catch { lStartFrame = 0; }
			
			try { lEndFrame = System.Convert.ToInt64(oleReader.GetValue(SEGMENTS_ENDFRAME).ToString()); }
			catch { lEndFrame = 0; }
			
			//	Do we need to initialize the deposition?
			if(m_bInitializeDeposition == true)
			{
				//	Get the values we need to initialize the deposition
				try { strWitness = oleReader.GetValue(SEGMENTS_WITNESS).ToString(); }
				catch { strWitness = ""; }
			
				try { iLinesPerPage = System.Convert.ToInt32(oleReader.GetValue(SEGMENTS_LINESPERPAGE).ToString()); }
				catch { iLinesPerPage = 0; }
			
				//	Do we have the witness information?
				if(strWitness.Length > 0)
				{
					m_xmlDeposition.Name = strWitness;

					try
					{
						string[] aFields = strWitness.Split(acDelimiters);
						if((aFields != null) && (aFields.GetUpperBound(0) >= 0))
							m_xmlDeposition.Deponent = aFields[0];
						if((aFields != null) && (aFields.GetUpperBound(0) >= 2))
							m_xmlDeposition.Date = aFields[2];
						
						if(iLinesPerPage > 0)
							m_xmlDeposition.LinesPerPage = iLinesPerPage;
							
						m_bInitializeDeposition = false;
					
					}
					catch
					{
					}
					
				}// if(strWitness.Length > 0)
				
			}// if(m_bInitializeDeposition == true)
					
			//	Create and initialize a new XML segment
			xmlSegment = new CXmlSegment();
			xmlSegment.Key = strKey;
			xmlSegment.Filename = strFilename;
			xmlSegment.FramesPerSecond = FRAMES_PER_SECOND;
			if((iStartPage > 0) && (iStartLine > 0))
				xmlSegment.FirstPL = CTmaxToolbox.GetPL(iStartPage, iStartLine);
			if(lStartFrame > 0)
				xmlSegment.Start = ((float)lStartFrame / FRAMES_PER_SECOND);
			else
				xmlSegment.Start = 0;
			if(lEndFrame > 0)
				xmlSegment.Stop = ((float)lEndFrame / FRAMES_PER_SECOND);
			else
				xmlSegment.Stop = 0;

			//	Were we able to get the required values?
			if((strKey == "-1") || (strFilename.Length == 0))
			{
				//	Add an error to the local collection
				if(m_aErrorMessages != null)
					m_aErrorMessages.Add(m_tmaxErrorBuilder.Message(ERROR_GET_SEGMENT_INVALID, iRow, SEGMENTS_TABLE));
			}
			
			return xmlSegment;
		
		}// private CXmlSegment GetSegment(OleDbDataReader oleReader, int iRow)
		
		/// <summary>This method fires system error events for each pending error</summary>
		/// <param name="strMethod">The method associated with the errors</param>
		private void FireErrors(string strMethod)
		{
			//	Are there any pending errors?
			//
			//	NOTE:	We postpone firing error events during iteration of
			//			an oleReader because firing the event breaks the reader
			if((m_aErrorMessages != null) && (m_aErrorMessages.Count > 0))
			{
				try
				{
					foreach(string O in m_aErrorMessages)
					{
						m_tmaxEventSource.FireError(this, strMethod, "converting " + System.IO.Path.GetFileName(m_strFileSpec) + " : "+ O);
					}
				
				}
				catch
				{
				}
				
				m_aErrorMessages.Clear();
				
			}
			
		}// private void FireErrors(string strMethod)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
			
		}// EventSource property
		
		/// <summary>Fully qualified path to the active video file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		
		}// FileSpec
		
		/// <summary>The converted XML deposition</summary>
		public CXmlDeposition XmlDeposition
		{
			get { return m_xmlDeposition; }
		
		}// XmlDeposition
		
		#endregion Properties
		
		
	}// public class CTmaxDepoLog

}// namespace FTI.Trialmax.Database.Legacy
