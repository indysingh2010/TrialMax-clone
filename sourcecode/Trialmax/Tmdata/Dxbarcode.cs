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
	/// <summary>This class encapsulates the information used to manage records in the BarcodeMap table</summary>
	public class CDxBarcode : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Private Members
		
		/// <summary>Local member bound to PSTQ property</summary>
		private string m_strPSTQ = "";
		
		/// <summary>Local member bound to Ancestor property</summary>
		private string m_strAncestor = "";
		
		/// <summary>Local member bound to Descendant property</summary>
		private string m_strDescendant = "";
		
		/// <summary>Local member bound to Source property</summary>
		protected CDxMediaRecord m_dxSource = null;
		
		/// <summary>Local member bound to Inherited property</summary>
		private bool m_bInherited = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxBarcode() : base()
		{
		}
		
		/// <summary>This method is called to get the record exchange object for the scene's source media</summary>
		///	<returns>The interface to the source record</returns>
		public CDxMediaRecord GetSource()
		{
			//	Have we already located the source record?
			if(m_dxSource != null) return m_dxSource;
			
			//	Do we have a valid id
			if((PSTQ != null) && (PSTQ.Length > 0))
			{
				//	Do we have access to the database?
				if((Collection != null) && (Collection.Database != null))
					m_dxSource = Collection.Database.GetRecordFromId(PSTQ, true);
			}

			return m_dxSource;
			
		}// public CDxMediaRecord GetSource()
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The database's unique identifier for the record</summary>
		public string PSTQ
		{
			get	{ return m_strPSTQ; }
			set { m_strPSTQ = value; }
		}
		
		/// <summary>The source record exchange object</summary>
		public CDxMediaRecord Source
		{
			get { return GetSource(); }
			set { m_dxSource = value; }
		}
		
		/// <summary>The source record exchange object</summary>
		/// <remarks>We provide a new property to change the base class handling</remarks>
		public new string ForeignBarcode
		{
			get { return m_strForeignBarcode; }
			set { m_strForeignBarcode = value; }
		}
		
		/// <summary>The portion of the inherited foreign barcode that actually appears in the database table</summary>
		public string Ancestor
		{
			get	{ return m_strAncestor; }
			set { m_strAncestor = value; }
		}
		
		/// <summary>The portion of the inherited barcode that identifies the descendant record</summary>
		public string Descendant
		{
			get	{ return m_strDescendant; }
			set { m_strDescendant = value; }
		}
		
		/// <summary>Indicates if this is inherited from the foreign barcode 
		///	of a higher level media record: ForeignBarcode.BarcodeId(s)</summary>
		public bool Inherited
		{
			get	{ return m_bInherited; }
			set { m_bInherited = value; }
		}
		
		#endregion Properties
	
	}// class CDxBarcode

	/// <summary>This class is used to manage a ArrayList of CDxBarcode objects</summary>
	public class CDxBarcodes : CDxMediaRecords
	{
		public enum eFields
		{
			PSTQ = 0,
			ForeignCode,
		}

		public const string TABLE_NAME = "BarcodeMap";
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxBarcodes() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxBarcodes(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxBarcode">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxBarcode Add(CDxBarcode dxBarcode)
		{
			string strSQL = "";

			Debug.Assert(dxBarcode != null);
            Debug.Assert(this.Database != null);
			
			try
			{
				//	Get the SQL statement					
				strSQL = GetSQLInsert(dxBarcode);

				Debug.Assert(strSQL.Length > 0);

				//	Execute the statement
				if(this.Database.IsConnected == false) return null;
				if(this.Database.Execute(strSQL) == true)
				{	
					//	Set the ownership to be this collection
					dxBarcode.Collection = this;
						
					//	Add it to the underlying array list
					base.Add(dxBarcode as object);
			
					return dxBarcode;
				
				}//if(this.Database.Connection.Execute(strSQL)) == true)
				
			}
			catch(OleDbException oleEx)
			{
                FireError("Add",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_ADD_SQL_EX,TableName,strSQL),oleEx,GetErrorItems(dxBarcode));
			}
			catch(System.Exception Ex)
			{
                FireError("Add",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_MEDIA_RECORD_ADD_SQL_EX,TableName,strSQL),Ex,GetErrorItems(dxBarcode));
			}
			
			return null;

		}// Add(CDxBarcode dxBarcode)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxBarcodes Dispose()
		{
			return (CDxBarcodes)base.Dispose();
			
		}// Dispose()

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CDxBarcode this[int iIndex]
		{
			get
			{
				return (CDxBarcode)base[iIndex];
			}
		}

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new CDxBarcode GetAt(int iIndex)
		{
			return (CDxBarcode)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxBarcode	dxBarcode = (CDxBarcode)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.PSTQ.ToString() + ",");
			strSQL += (eFields.ForeignCode.ToString() + ")");
			
			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(dxBarcode.PSTQ) + "',");
			strSQL += ("'" + SQLEncode(dxBarcode.ForeignBarcode) + "')");
			
			return strSQL;
		}

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The record to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxBarcode	dxBarcode = (CDxBarcode)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.ForeignCode.ToString() + " = '" + SQLEncode(dxBarcode.ForeignBarcode) + "'");
			
			strSQL += " WHERE PSTQ = '";
			strSQL += SQLEncode(dxBarcode.PSTQ);
			strSQL += "';";
			
			return strSQL;
		}

		/// <summary>This method is called to get the SQL statement required to delete this object's record in the database</summary>
		/// <param name="dxRecord">The object to be deleted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLDelete(CBaseRecord dxRecord)
		{
			CDxBarcode	dxBarcode = (CDxBarcode)dxRecord;
			string strSQL = "DELETE FROM ";
			
			strSQL += TableName;
			strSQL += " WHERE PSTQ = '";
			strSQL += SQLEncode(dxBarcode.PSTQ);
			strSQL += "';";
			
			return strSQL;
		}
		
		/// <summary>This method is called to get the SQL statement required to delete the specified records</summary>
		/// <param name="dxBarcodes">The collection of records to be deleted</param>
		/// <returns>The appropriate SQL statement</returns>
		public string GetSQLDelete(CDxBarcodes dxBarcodes)
		{
			string	strSQL = "DELETE FROM ";
			string	strPSTQ = "";
			bool	bFirstTime = true;
			
			strSQL += TableName;
			strSQL += " WHERE PSTQ IN (";
			
			foreach(CDxBarcode O in dxBarcodes)
			{
				if(bFirstTime == true)
					bFirstTime = false;
				else
					strSQL += ",";
					
				strPSTQ = String.Format("'{0}'", SQLEncode(O.PSTQ));
				strSQL += strPSTQ;
			}
			
			strSQL += ")";
			
			return strSQL;
		}
		
		/// <param name="dxMatches">The collection in which to store records that meet the search criteria</param>
		/// <param name="strForeign">The desired ForeignBarcode value</param>
		/// <param name="strSuffix">The suffix to be added to the foreign barcode to test for a valid record</param>
		/// <param name="bBreakOnFirst">True to break on the first match</param>
		/// <returns>The total number of records located</returns>
		public long SearchForeign(CDxBarcodes dxMatches, string strForeign, string strSuffix, bool bBreakOnFirst)
		{
			long		lMatches = 0;
			string		strBarcode = "";
			CDxMediaRecord	dxSource = null;
			CDxBarcode	dxBarcode = null;
			
			Debug.Assert(dxMatches != null);
			if(dxMatches == null) return 0;
			Debug.Assert(strForeign != null);
			if(strForeign == null) return 0;
			
			//	Locate all objects that have the specified Foreign value
			foreach(CDxBarcode O in this)
			{
				if(String.Compare(strForeign, O.ForeignBarcode, true) == 0)
				{
					//	Are we supposed to attach a suffix?
					if((strSuffix != null) && (strSuffix.Length > 0) && (O.Source != null))
					{
						//	Construct the true barcode for the source record
						strBarcode = O.Source.GetBarcode(true);
						if(strBarcode.EndsWith(".0") == true)
							strBarcode = strBarcode.Substring(0, strBarcode.Length - 2);
						strBarcode += ("." + strSuffix);
							
						//	Now try to locate the record identified by this barcode
						if(Database != null)
							dxSource = Database.GetRecordFromBarcode(strBarcode, true, true);
							
						//	Did we find a source record for the composite barcode?
						//
						//	NOTE:	We check to make sure this record is not already in the
						//			caller's collection. It could be if it is assigned an FBC
						//			and it's parent also has an FBC
						if((dxSource != null) && (dxMatches.FindSource(dxSource) == null))
						{
							//	We have to create a barcode object to represent this
							//	record since it's not actually in the map
							dxBarcode = new CDxBarcode();
							dxBarcode.PSTQ = dxSource.GetUniqueId();
							dxBarcode.Source = dxSource;
							dxBarcode.ForeignBarcode = strForeign + "." + strSuffix;
							dxBarcode.Inherited = true;
							dxBarcode.Ancestor = strForeign;
							dxBarcode.Descendant = strSuffix;
							
							//	Add it to the caller's collection
							dxMatches.AddList(dxBarcode);
							lMatches++;
							
							//	Should we stop here?
							if(bBreakOnFirst == true)
								return lMatches;
							
						}// if(dxSource != null)
					
					}
					else
					{
						dxMatches.AddList(O);
						lMatches++;
							
						//	Should we stop here?
						if(bBreakOnFirst == true)
							return lMatches;
					
					}// if((strSuffix != null) && (strSuffix.Length > 0) && (O.GetSource() != null))
					
				}// if(String.Compare(strForeign, O.ForeignBarcode, true) == 0)
				
			}// foreach(CDxBarcode O in this)

			//	Should we attempt to split the foreign barcode?
			if(strSuffix != null)
			{
				try
				{
					//	Split the foreign barcode
					if(Split(ref strForeign, ref strSuffix) == true)
					{
						//	Locate all the matches with the new combination
						lMatches += SearchForeign(dxMatches, strForeign, strSuffix, bBreakOnFirst);
					}

				}
				catch
				{
				}
				
			}// if(strSuffix != null)
			
			//	How many matches were found?
			return lMatches;
		
		}// public long SearchForeign(CDxBarcodes dxMatches, string strForeign, string strSuffix, bool bBreakOnFirst)
		
		/// <summary>Called to locate the record(s) with the specified ForeignCode value</summary>
		/// <param name="dxMatches">The collection in which to store records that meet the search criteria</param>
		/// <param name="strForeign">The desired ForeignBarcode value</param>
		/// <param name="strSuffix">The suffix to be added to the foreign barcode to test for a valid record</param>
		/// <returns>The total number of records located</returns>
		public long SearchForeign(CDxBarcodes dxMatches, string strForeign, string strSuffix)
		{
			//	Locate all occurances by default
			return SearchForeign(dxMatches, strForeign, strSuffix, false);
		
		}// public long SearchForeign(CDxBarcodes dxMatches, string strForeign, string strSuffix)
		
		/// <summary>Called to locate the record(s) with the specified ForeignCode value</summary>
		/// <param name="strForeign">The desired ForeignBarcode value</param>
		/// <param name="strSuffix">The suffix to be added to the foreign barcode to test for a valid record</param>
		/// <param name="bBreakOnFirst">True to break on the first match</param>
		/// <returns>The collection of matching records, null if no matches found</returns>
		public CDxBarcodes SearchForeign(string strForeign, string strSuffix, bool bBreakOnFirst)
		{
			CDxBarcodes dxMatches = null;
			
			try
			{
				dxMatches = new CDxBarcodes();
				
				//	Locate the specified records
				SearchForeign(dxMatches, strForeign, strSuffix, bBreakOnFirst);
			}
			catch
			{
				dxMatches = null;
			}
			
			//	Only return a collection if we find a match
			if(dxMatches != null)
			{
				if(dxMatches.Count == 0)
					dxMatches = null;
			}
			
			return dxMatches;
		
		}// public CDxBarcodes SearchForeign(string strForeign, string strSuffix, bool bBreakOnFirst)
		
		/// <summary>Called to locate the record(s) with the specified ForeignCode value</summary>
		/// <param name="strForeign">The desired ForeignBarcode value</param>
		/// <param name="strSuffix">The suffix to be added to the foreign barcode to test for a valid record</param>
		/// <returns>The collection of matching records, null if no matches found</returns>
		public CDxBarcodes SearchForeign(string strForeign, string strSuffix)
		{
			return SearchForeign(strForeign, strSuffix, false);
		
		}// public CDxBarcodes SearchForeign(string strForeign, string strSuffix)
		
		/// <summary>Called to locate the record(s) with the specified ForeignCode value</summary>
		/// <param name="strForeign">The desired ForeignBarcode value</param>
		/// <param name="bBreakOnFirst">True to break on the first match</param>
		/// <returns>The collection of matching records, null if no matches found</returns>
		public CDxBarcodes SearchForeign(string strForeign, bool bBreakOnFirst)
		{
			string strSuffix = "";
			
			return SearchForeign(strForeign, strSuffix, bBreakOnFirst);
		
		}// public CDxBarcodes SearchForeign(string strForeign, bool bBreakOnFirst)
		
		/// <summary>Called to locate the record(s) with the specified ForeignCode value</summary>
		/// <param name="strForeign">The desired ForeignBarcode value</param>
		/// <returns>The collection of matching records, null if no matches found</returns>
		public CDxBarcodes SearchForeign(string strForeign)
		{
			//	Find all occurances
			return SearchForeign(strForeign, false);
		
		}// public CDxBarcodes SearchForeign(string strForeign)
		
		/// <summary>This method is called to delete the requested records from the collection</summary>
		/// <param name="dxBarcodes">The collection of records to be deleted</param>
		///	<returns>true if successful</returns>
		public bool Delete(CDxBarcodes dxBarcodes)
		{
			string	strSQL = "";
			
			//	Don't bother if nothing in the collection
			Debug.Assert(dxBarcodes != null);
			Debug.Assert(dxBarcodes.Count > 0);
			if((dxBarcodes == null) || (dxBarcodes.Count == 0)) return false;
			
			//	Is there only one record in the collection
			if(dxBarcodes.Count == 1)
				return Delete(dxBarcodes[0]);
				
			//	Make sure we have a valid connection
			if(this.Database.IsConnected == false) return false;

			try
			{
				//	Get the SQL statement from the object						
				strSQL = GetSQLDelete(dxBarcodes);
				Debug.Assert(strSQL.Length > 0);
				
				//	Execute the statement
				this.Database.Execute(strSQL);
				
				//	Remove each of the specified records
				foreach(CDxBarcode O in dxBarcodes)
					base.Remove(O as object);
					
				return true;

			}
			catch(OleDbException oleEx)
			{
                FireError("Delete",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_COMMON_DELETE_SQL_EX,TableName,strSQL),oleEx);
			}
			catch(System.Exception Ex)
			{
                FireError("Delete",this.ExBuilder.Message(CTmaxCaseDatabase.ERROR_COMMON_DELETE_SQL_EX,TableName,strSQL),Ex);
			}
			
			//	Must have been some kind of problem
			return false;
			
		}// public bool Delete(CDxBarcodes dxBarcodes)

		/// <summary>This method is called when the specified source record is deleted</summary>
		/// <param name="strPSTQ">The PSTQ of the source record that has been deleted</param>
		///	<returns>true if successful</returns>
		public bool OnSourceDeleted(string strPSTQ)
		{
			CDxBarcodes dxDelete = null;
			bool		bSuccessful = true;
			string		strDescendant = "";
			
			//	Don't bother if we have no records in the collection
			if(this.Count == 0) return true;
			
			//	Make sure we have a valid PSTQ identifier
			Debug.Assert(strPSTQ != null);
			Debug.Assert(strPSTQ.Length > 0);
			if((strPSTQ == null) || (strPSTQ.Length == 0)) return false;
			
			//	Allocate a temporary collection to store the barcodes to be deleted
			dxDelete = new CDxBarcodes();
			
			//	Check this collection to see if the source or any of it's
			//	descendants appear in the barcode map
			strDescendant = (strPSTQ + ".");
			foreach(CDxBarcode O in this)
			{
				if((O.PSTQ == strPSTQ) || (O.PSTQ.StartsWith(strDescendant) == true))
					dxDelete.AddList(O);
					
			}// foreach(CDxBarcode O in this)
			
			//	Do we have anything to delete?
			if(dxDelete.Count > 0)
			{
				//	Remove each of the barcodes from the database barcode map
				foreach(CDxBarcode O in dxDelete)
					this.Remove(O);
					
				//	Now delete all the entries in the table
				if(Delete(dxDelete) == false)	
					bSuccessful = false;
				
				//	Flush the temporary collection
				dxDelete.Clear();
				
			}// if(dxDelete.Count > 0)

			dxDelete = null;
			
			return bSuccessful;
						
		}// public bool OnSourceDeleted(string strPSTQ)

		/// <summary>This method is called when the specified source record is deleted</summary>
		/// <param name="dxSource">The source record's exchange interface</param>
		///	<returns>true if successful</returns>
		public bool OnSourceDeleted(CDxMediaRecord dxSource)
		{
			string strPSTQ = "";
			
			Debug.Assert(dxSource != null);
			if(dxSource == null) return false;
			
			strPSTQ = dxSource.GetUniqueId();
			Debug.Assert(strPSTQ != null);
			Debug.Assert(strPSTQ.Length > 0);
			if((strPSTQ == null) || (strPSTQ.Length == 0)) return false;
			
			return OnSourceDeleted(strPSTQ);
			
		}// public bool OnSourceDeleted(CDxMediaRecord dxSource)

		/// <summary>Called to locate the record with the specified ForeignCode value</summary>
		/// <param name="strForeign">The desired ForeignBarcode value</param>
		/// <returns>The matching record if found</returns>
		public CDxBarcode FindForeign(string strForeign)
		{
			CDxBarcodes dxBarcodes = null;
			
			//	Locate the first occurance of the specified foreign barcode
			if((dxBarcodes = SearchForeign(strForeign, true)) != null)
			{
				Debug.Assert(dxBarcodes.Count == 1);
				if(dxBarcodes.Count > 0)
					return dxBarcodes[0];
			}
			
			//	Must not have found a match
			return null;
		
		}// public CDxBarcode FindForeign(string strForeign)
		
		/// <summary>Called to locate the record with the specified media PSTQ value</summary>
		/// <param name="strPSTQ">The desired PSTQ value</param>
		/// <returns>The first record (hopefully only) with the specified PSTQ value</returns>
		public CDxBarcode FindMedia(string strPSTQ)
		{
			foreach(CDxBarcode O in this)
			{
				if(String.Compare(strPSTQ, O.PSTQ, true) == 0)
					return O;
			}
			
			//	Must not be a match
			return null;
		
		}// public CDxBarcode FindMedia(string strPSTQ)
		
		/// <summary>Called to locate the record(s) with the specified media PSTQ value</summary>
		/// <param name="dxMatches">The collection in which to store records that meet the search criteria</param>
		/// <param name="strPSTQ">The desired PSTQ value</param>
		/// <returns>The total number of records located</returns>
		public long SearchMedia(CDxBarcodes dxMatches, string strPSTQ)
		{
			long lMatches = 0;
			
			Debug.Assert(dxMatches != null);
			if(dxMatches == null) return 0;
			
			//	Locate all objects that have the specified PSTQ value
			foreach(CDxBarcode O in this)
			{
				if(String.Compare(strPSTQ, O.PSTQ, true) == 0)
				{
					dxMatches.AddList(O);
					lMatches++;
				}
				
			}// foreach(CDxBarcode O in this)
			
			//	How many matches were found?
			return lMatches;
		
		}// public long SearchMedia(CDxBarcodes dxMatches, string strPSTQ)
		
		/// <summary>Called to locate the record(s) with the specified media PSTQ value</summary>
		/// <param name="strPSTQ">The desired PSTQ value</param>
		/// <returns>The collection of matching records, null if no matches</returns>
		public CDxBarcodes SearchMedia(string strPSTQ)
		{
			CDxBarcodes dxMatches = null;
			
			try
			{
				dxMatches = new CDxBarcodes();
				
				//	Locate the specified records
				SearchMedia(dxMatches, strPSTQ);
			}
			catch
			{
				dxMatches = null;
			}
			
			//	Only return a collection if we find a match
			if(dxMatches != null)
			{
				if(dxMatches.Count == 0)
					dxMatches = null;
			}
			
			return dxMatches;
		
		}// public CDxBarcodes SearchMedia(string strPSTQ)
		
		/// <summary>Called to locate the record with the specified source record</summary>
		/// <param name="strPSTQ">The desired source record</param>
		/// <returns>The first record (hopefully only) with the specified source record</returns>
		public CDxBarcode FindSource(CDxMediaRecord dxSource)
		{
			Debug.Assert(dxSource != null);
			if(dxSource == null) return null;

			return FindMedia(dxSource.GetUniqueId());

		}// public CDxBarcode FindSource(CDxMediaRecord dxSource)
		
		/// <summary>Called to locate the record(s) with the specified source record</summary>
		/// <param name="dxMatches">The collection in which to store the matching records</param>
		/// <param name="dxSource">The desired source record</param>
		/// <returns>The total number of records located</returns>
		public long SearchSource(CDxBarcodes dxMatches, CDxMediaRecord dxSource)
		{
			Debug.Assert(dxMatches != null);
			if(dxMatches == null) return 0;
			Debug.Assert(dxSource != null);
			if(dxSource == null) return 0;

			return SearchMedia(dxMatches, dxSource.GetUniqueId());
		
		}// public long SearchSource(CDxBarcodes dxMatches, CDxMediaRecord dxSource)
		
		/// <summary>Called to locate the record(s) with the specified source record</summary>
		/// <param name="dxSource">The desired source record</param>
		/// <returns>The collection of matching records, null if no matches</returns>
		public CDxBarcodes SearchSource(CDxMediaRecord dxSource)
		{
			Debug.Assert(dxSource != null);
			if(dxSource == null) return null;

			return SearchMedia(dxSource.GetUniqueId());
		
		}// public CDxBarcodes SearchSource(string strPSTQ)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to get a new record object</summary>
		/// <returns>A new object of the collection type</returns>
		protected override CBaseRecord GetNewRecord()
		{
			return ((CBaseRecord)(new CDxBarcode()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxBarcode dxBarcode = (CDxBarcode)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.PSTQ].Value  = dxBarcode.PSTQ;
					m_dxFields[(int)eFields.ForeignCode].Value = dxBarcode.ForeignBarcode;
				}
				else
				{
					dxBarcode.PSTQ = (string)(m_dxFields[(int)eFields.PSTQ].Value);
					dxBarcode.ForeignBarcode = (string)(m_dxFields[(int)eFields.ForeignCode].Value);
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
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected override string[] GetFieldNames()
		{
			return Enum.GetNames(typeof(eFields));
		}
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected override void SetNames()
		{
			m_strTableName = TABLE_NAME;
		}
		
		/// <summary>This method will set the default value for the specified field</summary>
		/// <param name="iField">The enumerated field identifier</param>
		/// <param name="dxField">The field object to be set</param>
		protected override void SetValue(int iField, CDxField dxField)
		{
			switch(iField)
			{
				case ((int)(eFields.PSTQ)):
				
					dxField.Value = "";
					break;
					
				case ((int)(eFields.ForeignCode)):
				
					dxField.Value = "";
					break;
					
				default:
				
					//	Let the base class deal with it
					base.SetValue(iField, dxField);
					break;
					
			}// switch(eField)
		
		}// protected void SetValue(int iField, CDxField dxField)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method splits the components of an inherited barcode</summary>
		/// <param name="rstrForeignBarcode">The foreign barcode to be parsed</param>
		/// <param name="rstrSuffix">The suffix to which the BarcodeId value is to be added</param>
		/// <returns>True if the foreign barcode contains a trailing BarcodeId value</returns>
		private bool Split(ref string rstrForeignBarcode, ref string rstrSuffix)
		{
//			string	strNumbers = "";
//			string	strForeign = "";
//			int		iDelimiter = 0;
			
			Debug.Assert(rstrForeignBarcode != null);
			if(rstrForeignBarcode == null) return false;
			Debug.Assert(rstrSuffix != null);
			if(rstrSuffix == null) return false;

// INHERITED
//			//	Initialize the foreign barcode
//			strForeign = rstrForeignBarcode;
//			
//			//	Separate the foreign barcode and display order components
//			//
//			//	NOTE: We are looking for this format: ForeignBarcode.DisplayOrder
//			if((iDelimiter = strForeign.LastIndexOf('.')) > 0)
//			{
//				//	Make sure there is something after the delimiter
//				if(iDelimiter < (strForeign.Length - 1))
//				{
//					//	Separate the components
//					strNumbers = strForeign.Substring(iDelimiter + 1);
//					strForeign = strForeign.Substring(0, iDelimiter);
//					
//					//	All the characters in the trailing portion must be numeric
//					for(int i = 0; i < strNumbers.Length; i++)
//					{
//						if(CTmaxToolbox.IsDigit(strNumbers[i]) == false)
//							return false;
//					}
//					
//					//	Adjust the caller's strings
//					rstrForeignBarcode = strForeign;
//					
//					if(rstrSuffix.Length > 0)
//					{
//						if(rstrSuffix.StartsWith(".") == true)
//							rstrSuffix = (strNumbers + rstrSuffix);
//						else
//							rstrSuffix = (strNumbers + "." + rstrSuffix);
//					}
//					else
//					{
//						rstrSuffix = strNumbers;
//					}
//						
//					return true;
//					
//				}// if(iDelimiter < (strForeign.Length - 1))
//				
//			}// if((iDelimiter = strForeign.LastIndexOf('.')) > 0)
			
			//	Must not be a valid ForeignBarcode.DisplayOrder format
			return false;
		}
		
		#endregion Private Methods
		
		
	}//	CDxBarcodes
		
}// namespace FTI.Trialmax.Database
