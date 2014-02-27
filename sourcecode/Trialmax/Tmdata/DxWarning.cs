using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class encapsulates the information used to display a warning when an older version opens a newer database</summary>
	public class CDxWarning : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Private Members
		
		/// <summary>Local member bound to Priority property</summary>
		private long m_lPriority = 0;
		
		/// <summary>Local member bound to Version property</summary>
		private string m_strVersion = "";
		
		/// <summary>Local member bound to Message property</summary>
		protected string m_strMessage = "";
		
		/// <summary>Local member bound to SpareText1 property</summary>
		protected string m_strSpareText1 = "";
		
		/// <summary>Local member bound to SpareText2 property</summary>
		protected string m_strSpareText2 = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxWarning() : base()
		{
		}
		
		/// <summary>Gets the numerical packed version identifier</summary>
		/// <returns>the numerical packed version identifier</returns>
		public long GetPackedVersion()
		{
			return CBaseVersion.GetPackedVersion(this.Version);
		}
		
		/// <summary>This function is called to get the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		public override string[] GetListViewNames(int iDisplayMode)
		{
			string[] aNames = { "Version", "Message" };
			return aNames;

		}// public override string[] GetListViewNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		public override string[] GetListViewValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[2];
			aValues[0] = this.Version;
			aValues[1] = this.Message;
			
			return aValues;

		}// public override string[] GetListViewValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		public override int GetListViewImage(int iDisplayMode)
		{
			//	No images used for this type
			return (int)(this.Priority);
		}
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The database version that added the warning</summary>
		public string Version
		{
			get { return m_strVersion; }
			set { m_strVersion = value; }
		}
		
		/// <summary>The warning message</summary>
		public string Message
		{
			get { return m_strMessage; }
			set { m_strMessage = value; }
		}
		
		/// <summary>spare text field</summary>
		public string SpareText1
		{
			get { return m_strSpareText1; }
			set { m_strSpareText1 = value; }
		}
		
		/// <summary>spare text field</summary>
		public string SpareText2
		{
			get { return m_strSpareText2; }
			set { m_strSpareText2 = value; }
		}
		
		/// <summary>The priority level</summary>
		public long Priority
		{
			get { return m_lPriority; }
			set { m_lPriority = value; }
		}
		
		#endregion Properties
	
	}// class CDxWarning

	/// <summary>
	/// This class is used to manage a ArrayList of CDxWarning objects
	/// </summary>
	public class CDxWarnings : CDxMediaRecords
	{
		public enum eFields
		{
			AutoId = 0,
			Version,
			Message,
			SpareText1,
			SpareText2,
			Priority,
		}

		public const string TABLE_NAME = "Warnings";
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxWarnings() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxWarnings(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxWarning">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxWarning Add(CDxWarning dxWarning)
		{
			return (CDxWarning)base.Add(dxWarning);
			
		}// Add(CDxWarning dxWarning)

		/// <summary>This method uses the specified information to add a warning to the table</summary>
		/// <param name="strVersion">The version to be assigned to the warning</param>
		/// <param name="strMessage">The message to be assigned to the warning</param>
		/// <param name="lPriority">The priority to be assigned to the warning</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxWarning Add(string strVersion, string strMessage, long lPriority)
		{
			CDxWarning dxWarning = new CDxWarning();
			
			dxWarning.Version  = strVersion;
			dxWarning.Message  = strMessage;
			dxWarning.Priority = lPriority;
			
			return Add(dxWarning);
			
		}// public CDxWarning Add(string strVersion, string strMessage, long lPriority)

		/// <summary>This method uses the specified information to add a warning to the table</summary>
		/// <param name="strVersion">The version to be assigned to the warning</param>
		/// <param name="strMessage">The message to be assigned to the warning</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxWarning Add(string strVersion, string strMessage)
		{
			return Add(strVersion, strMessage, 0);
		}

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxWarnings Dispose()
		{
			return (CDxWarnings)base.Dispose();
			
		}// Dispose()

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxWarning Find(long lAutoId)
		{
			return (CDxWarning)Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxWarning this[int iIndex]
		{
			get
			{
				return (CDxWarning)base[iIndex];
			}
		}

		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxWarning GetAt(int iIndex)
		{
			return (CDxWarning)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxWarning	dxWarning = (CDxWarning)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.Version.ToString() + ",");
			strSQL += (eFields.Message.ToString() + ",");
			strSQL += (eFields.SpareText1.ToString() + ",");
			strSQL += (eFields.SpareText2.ToString() + ",");
			strSQL += (eFields.Priority.ToString() + ")");
			
			strSQL += " VALUES(";
			strSQL += ("'" + SQLEncode(dxWarning.Version) + "',");
			strSQL += ("'" + SQLEncode(dxWarning.Message) + "',");
			strSQL += ("'" + SQLEncode(dxWarning.SpareText1) + "',");
			strSQL += ("'" + SQLEncode(dxWarning.SpareText2) + "',");
			strSQL += ("'" + dxWarning.Priority.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxWarning	dxWarning = (CDxWarning)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.Version.ToString() + " = '" + SQLEncode(dxWarning.Version) + "',");
			strSQL += (eFields.Message.ToString() + " = '" + SQLEncode(dxWarning.Message) + "',");
			strSQL += (eFields.SpareText1.ToString() + " = '" + SQLEncode(dxWarning.SpareText1) + "',");
			strSQL += (eFields.SpareText2.ToString() + " = '" + SQLEncode(dxWarning.SpareText2) + "',");
			strSQL += (eFields.Priority.ToString() + " = '" + dxWarning.Priority.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxWarning.AutoId.ToString();
			strSQL += ";";
			
			return strSQL;
		}

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
			return ((CBaseRecord)(new CDxWarning()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxWarning dxWarning = (CDxWarning)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxWarning.AutoId;
					m_dxFields[(int)eFields.Version].Value = dxWarning.Version;
					m_dxFields[(int)eFields.Message].Value = dxWarning.Message;
					m_dxFields[(int)eFields.SpareText1].Value = dxWarning.SpareText1;
					m_dxFields[(int)eFields.SpareText2].Value = dxWarning.SpareText2;
					m_dxFields[(int)eFields.Priority].Value = dxWarning.Priority;
				}
				else
				{
					dxWarning.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxWarning.Version = (m_dxFields[(int)eFields.Version].Value).ToString();
					dxWarning.Message = (m_dxFields[(int)eFields.Message].Value).ToString();
					dxWarning.SpareText1 = (m_dxFields[(int)eFields.SpareText1].Value).ToString();
					dxWarning.SpareText2 = (m_dxFields[(int)eFields.SpareText2].Value).ToString();
					dxWarning.Priority = (int)(m_dxFields[(int)eFields.Priority].Value);
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
				case eFields.Priority:
				
					dxField.Value = 0;
					break;
					
				case eFields.Version:
				case eFields.Message:
				case eFields.SpareText1:
				case eFields.SpareText2:
				
					dxField.Value = "";
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods
		
	}//	CDxWarnings
		
}// namespace FTI.Trialmax.Database
