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
	/// <summary>This class is used to manage a record in the Depositions table of the Objections database</summary>
	public class COxDeposition : FTI.Trialmax.Database.CBaseRecord, ITmaxBaseObjectionRecord, ITmaxDeposition
	{
		#region Private Members

		/// <summary>Local member bound to UniqueId property</summary>
		private System.Guid m_guidUniqueId = System.Guid.Empty;

		/// <summary>Local member bound to MediaId property</summary>
		private string m_strMediaId = "";

		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";

		/// <summary>Local member bound to Deponent property</summary>
		private string m_strDeponent = "";

		/// <summary>Local member bound to DeposedOn property</summary>
		private DateTime m_dtDeposedOn = System.DateTime.Now;

		/// <summary>Local member bound to FirstPL property</summary>
		private long m_lFirstPL = 0;

		/// <summary>Local member bound to LastPL property</summary>
		private long m_lLastPL = 0;

		/// <summary>Local member bound to CreatedOn property</summary>
		private DateTime m_dtCreatedOn = System.DateTime.Now;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public COxDeposition() : base()
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
			return this.MediaId;
		}

		/// <summary>This method retrieves the text used to display this record</summary>
		///	<returns>The display text</returns>
		string ITmaxDeposition.ShowAs()
		{
			return this.MediaId;
		}

		/// <summary>This method retrieves the FirstPL value</summary>
		/// <returns>The transcript's FirstPL value</returns>
		public long GetFirstPL()
		{
			return m_lFirstPL;
		}

		/// <summary>This method retrieves the LastPL value</summary>
		/// <returns>The transcript's LastPL value</returns>
		public long GetLastPL()
		{
			return m_lLastPL;
		}

		/// <summary>This method retrieves the MediaId value</summary>
		/// <returns>The transcript's MediaId value</returns>
		public string GetMediaId()
		{
			return m_strMediaId;
		}

		#endregion Public Methods

		#region Properties

		/// <summary>The unique identifier assigned by the database</summary>
		public System.Guid UniqueId
		{
			get { return m_guidUniqueId; }
			set { m_guidUniqueId = value; }
		}

		/// <summary>The MediaId assigned to the deposition in the TrialMax case database</summary>
		public string MediaId
		{
			get { return GetMediaId(); }
			set { m_strMediaId = value; }
		}

		/// <summary>The name of the TrialMax XMLT file containing the deposition</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
		}

		/// <summary>The name used to identify the deponent</summary>
		public string Deponent
		{
			get { return m_strDeponent; }
			set { m_strDeponent = value; }
		}

		/// <summary>The date of the deposition</summary>
		public System.DateTime DeposedOn
		{
			get { return m_dtDeposedOn; }
			set { m_dtDeposedOn = value; }
		}

		/// <summary>The first page/line of transcript text</summary>
		public long FirstPL
		{
			get { return GetFirstPL(); }
			set { m_lFirstPL = value; }
		}

		/// <summary>The last page/line of transcript text</summary>
		public long LastPL
		{
			get { return GetLastPL(); }
			set { m_lLastPL = value; }
		}

		/// <summary>The date the record was created</summary>
		public System.DateTime CreatedOn
		{
			get { return m_dtCreatedOn; }
			set { m_dtCreatedOn = value; }
		}

		#endregion Properties

	}// public class COxDeposition : FTI.Trialmax.Database.CBaseRecord

	/// <summary>This class is used to manage a ArrayList of COxDeposition objects</summary>
	public class COxDepositions : CBaseRecords
	{
		public enum eFields
		{
			UniqueId = 0,
			MediaId,
			Filename,
			Deponent,
			DeposedOn,
			FirstPL,
			LastPL,
			CreatedOn,
		}

		public const string TABLE_NAME = "Depositions";

		#region Public Members

		/// <summary>Default constructor</summary>
		public COxDepositions() : base()
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public COxDepositions(CObjectionsDatabase tmaxDatabase) : base(tmaxDatabase)
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="OxDeposition">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public COxDeposition Add(COxDeposition OxDeposition)
		{
			COxDeposition oxAdded = null;

			oxAdded = (COxDeposition)base.Add(OxDeposition);

			return oxAdded;

		}// public COxDeposition Add(COxDeposition OxDeposition)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new COxDepositions Dispose()
		{
			return (COxDepositions)base.Dispose();

		}// public new COxDepositions Dispose()

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new COxDeposition this[int iIndex]
		{
			get { return (COxDeposition)base[iIndex]; }

		}// public new COxDeposition this[int iIndex]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new COxDeposition GetAt(int iIndex)
		{
			return (COxDeposition)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			COxDeposition oxDeposition = (COxDeposition)dxRecord;
			string strSQL = "INSERT INTO " + TableName + "(";

			//	Make sure the deposition has been assigned a unique id
			if(oxDeposition.UniqueId == System.Guid.Empty)
				oxDeposition.UniqueId = System.Guid.NewGuid();

			strSQL += (eFields.UniqueId + ",");
			strSQL += (eFields.MediaId.ToString() + ",");
			strSQL += (eFields.Filename.ToString() + ",");
			strSQL += (eFields.Deponent.ToString() + ",");
			strSQL += (eFields.DeposedOn.ToString() + ",");
			strSQL += (eFields.FirstPL.ToString() + ",");
			strSQL += (eFields.LastPL.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString() + ")");

			strSQL += " VALUES(";

			strSQL += ("'" + oxDeposition.UniqueId.ToString() + "',");
			strSQL += ("'" + SQLEncode(oxDeposition.MediaId) + "',");
			strSQL += ("'" + SQLEncode(oxDeposition.Filename) + "',");
			strSQL += ("'" + SQLEncode(oxDeposition.Deponent) + "',");
			strSQL += ("'" + oxDeposition.DeposedOn.ToString() + "',");
			strSQL += ("'" + oxDeposition.FirstPL.ToString() + "',");
			strSQL += ("'" + oxDeposition.LastPL.ToString() + "',");
			strSQL += ("'" + oxDeposition.CreatedOn.ToString() + "')");

			return strSQL;

		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			COxDeposition oxDeposition = (COxDeposition)dxRecord;
			string strSQL = "UPDATE " + TableName + " SET ";

			strSQL += (eFields.MediaId.ToString() + " = '" + SQLEncode(oxDeposition.MediaId) + "',");
			strSQL += (eFields.Filename.ToString() + " = '" + SQLEncode(oxDeposition.Filename) + "',");
			strSQL += (eFields.Deponent.ToString() + " = '" + oxDeposition.Deponent + "',");
			strSQL += (eFields.DeposedOn.ToString() + " = '" + oxDeposition.DeposedOn.ToString() + "'");
			strSQL += (eFields.FirstPL.ToString() + " = '" + oxDeposition.FirstPL.ToString() + "',");
			strSQL += (eFields.LastPL.ToString() + " = '" + oxDeposition.LastPL.ToString() + "',");
			strSQL += (eFields.CreatedOn.ToString() + " = '" + oxDeposition.CreatedOn.ToString() + "'");

			strSQL += " WHERE UniqueId = '{";
			strSQL += oxDeposition.UniqueId.ToString();
			strSQL += "}';";

			return strSQL;

		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>Called to locate the object with the specified value</summary>
		/// <param name="strValue">The value of the desired object</param>
		/// <param name="bMediaId">True for MediaId, false for UniqueId</param>
		/// <returns>The object with the specified MediaId or UniqueId</returns>
		public COxDeposition Find(string strValue, bool bMediaId)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(COxDeposition O in m_aList)
				{
					if(bMediaId == true)
					{
						if(String.Compare(strValue, O.MediaId, true) == 0)
							return O;
					}
					else
					{
						if(String.Compare(strValue, O.UniqueId.ToString(), true) == 0)
							return O;
					}

				}// foreach(COxDeposition O in m_aList)

			}// if(m_aList != null)

			return null;

		}//	public COxDeposition Find(string strValue, bool bMediaId)

		/// <summary>Called to locate the object with the specified UniqueId value</summary>
		/// <param name="uniqueId">The UniqueId value of the desired object</param>
		/// <returns>The object with the specified UniqueId</returns>
		public COxDeposition Find(System.Guid uniqueId)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(COxDeposition O in m_aList)
				{
					if(O.UniqueId == uniqueId)
						return O;

				}// foreach(COxDeposition O in m_aList)

			}// if(m_aList != null)
			
			return null;
			
		}// public COxDeposition Find(System.Guid uniqueId)

		/// <summary>Called to locate the object with the specified Deponent and DeposedOn values</summary>
		/// <param name="strDeponent">The deponent name</param>
		/// <param name="dtDeposedOn">The date of the deposition</param>
		/// <returns>The object with the specified deponent name and date</returns>
		public COxDeposition Find(string strDeponent, System.DateTime dtDeposedOn)
		{
			if(m_aList != null)
			{
				foreach(COxDeposition O in m_aList)
				{
					if(String.Compare(strDeponent,O.Deponent,true) == 0)
					{
						if(O.DeposedOn == dtDeposedOn)
							return O;
					}

				}// foreach(COxDeposition O in m_aList)

			}// if(m_aList != null)

			return null;

		}//	public COxDeposition Find(string strDeponent, System.DateTime dtDeposedOn)

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
			return ((CBaseRecord)(new COxDeposition()));
		}

		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord,bool bSetFields)
		{
			COxDeposition oxDeposition = (COxDeposition)dxRecord;

			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;

			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.UniqueId].Value = oxDeposition.UniqueId;
					m_dxFields[(int)eFields.MediaId].Value = oxDeposition.MediaId;
					m_dxFields[(int)eFields.Filename].Value = oxDeposition.Filename;
					m_dxFields[(int)eFields.Deponent].Value = oxDeposition.Deponent;
					m_dxFields[(int)eFields.DeposedOn].Value = oxDeposition.DeposedOn;
					m_dxFields[(int)eFields.FirstPL].Value = oxDeposition.FirstPL;
					m_dxFields[(int)eFields.LastPL].Value = oxDeposition.LastPL;
					m_dxFields[(int)eFields.CreatedOn].Value = oxDeposition.CreatedOn;
				}
				else
				{
					oxDeposition.UniqueId = (System.Guid)(m_dxFields[(int)eFields.UniqueId].Value);
					oxDeposition.MediaId = m_dxFields[(int)eFields.MediaId].Value.ToString();
					oxDeposition.Filename = m_dxFields[(int)eFields.Filename].Value.ToString();
					oxDeposition.Deponent = m_dxFields[(int)eFields.Deponent].Value.ToString();
					oxDeposition.DeposedOn = (DateTime)(m_dxFields[(int)eFields.DeposedOn].Value);
					oxDeposition.FirstPL = (int)(m_dxFields[(int)eFields.FirstPL].Value);
					oxDeposition.LastPL = (int)(m_dxFields[(int)eFields.LastPL].Value);
					oxDeposition.CreatedOn = (DateTime)(m_dxFields[(int)eFields.CreatedOn].Value);
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
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.UniqueId:

					dxField.Value = System.Guid.Empty;
					break;

				case eFields.DeposedOn:
				case eFields.CreatedOn:

					dxField.Value = System.DateTime.Now;
					break;

				case eFields.MediaId:
				case eFields.Filename:
				case eFields.Deponent:

					dxField.Value = "";
					break;

				case eFields.FirstPL:
				case eFields.LastPL:

					dxField.Value = 0;
					break;

				default:

					Debug.Assert(false,"SetValue() - unknown field identifier - " + eField.ToString());
					break;

			}// switch(eField)

		}// private void SetValue(eFields eField, CDxField dxField)

		#endregion Private Methods

	}//	COxDepositions

}// namespace FTI.Trialmax.Database
