using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class encapsulates the information associated with a transcript
	/// </summary>
	public class CDxTranscript : FTI.Trialmax.Database.CDxMediaRecord
	{
		#region Constants
		
		protected const int DXTRANSCRIPT_PROP_FIRST_ID			= 13000; // Ensures a unique id
		
		protected const int DXTRANSCRIPT_PROP_DEPONENT			= (DXTRANSCRIPT_PROP_FIRST_ID + 1);
		protected const int	DXTRANSCRIPT_PROP_DEPOSED_ON		= (DXTRANSCRIPT_PROP_FIRST_ID + 2);
		protected const int DXTRANSCRIPT_PROP_FIRST_PL			= (DXTRANSCRIPT_PROP_FIRST_ID + 3);
		protected const int DXTRANSCRIPT_PROP_LAST_PL			= (DXTRANSCRIPT_PROP_FIRST_ID + 4);
		protected const int DXTRANSCRIPT_PROP_LINES_PER_PAGE	= (DXTRANSCRIPT_PROP_FIRST_ID + 5);
		protected const int DXTRANSCRIPT_PROP_FILENAME			= (DXTRANSCRIPT_PROP_FIRST_ID + 6);
		protected const int DXTRANSCRIPT_PROP_PATH				= (DXTRANSCRIPT_PROP_FIRST_ID + 7);

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to XmlDeposition property</summary>
		protected CXmlDeposition m_xmlDeposition = null;
		
		/// <summary>Local member bound to Primary property</summary>
		protected CDxPrimary m_dxPrimary = null;
		
		/// <summary>Local member bound to PrimaryId property</summary>
		private long m_lPrimaryId = 0;
		
		/// <summary>Local member bound to Deponent property</summary>
		private string m_strDeponent = "";
		
		/// <summary>Local member bound to DeposedOn property</summary>
		private string m_strDeposedOn = "";
		
		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";
		
		/// <summary>Local member bound to LinesPerPage property</summary>
		protected int m_iLinesPerPage = CXmlDeposition.GetDefaultLinesPerPage();
		
		/// <summary>Local member bound to FirstPL property</summary>
		protected long m_lFirstPL = -1;
		
		/// <summary>Local member bound to LastPL property</summary>
		protected long m_lLastPL = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxTranscript() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		public CDxTranscript(CDxPrimary dxPrimary) : base()
		{
			m_dxPrimary = dxPrimary;
			if(m_dxPrimary != null)
				m_lPrimaryId = m_dxPrimary.AutoId;
		}
		
		/// <summary>This function is called to get the media level</summary>
		public override FTI.Shared.Trialmax.TmaxMediaLevels GetMediaLevel()
		{
			return FTI.Shared.Trialmax.TmaxMediaLevels.Primary;
		}
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Media;
		}
		
		/// <summary>This function is called to populate the caller's collection with the properties associated with this record</summary>
		public override void GetProperties(CTmaxProperties tmaxProperties)
		{
			//tmaxProperties.Add(DXTRANSCRIPT_PROP_DEPONENT, "Deponent", Deponent, TmaxPropertyCategories.Media, TmaxPropertyEditors.None);
			//tmaxProperties.Add(DXTRANSCRIPT_PROP_DEPOSED_ON, "Deposed On", DeposedOn, TmaxPropertyCategories.Media, TmaxPropertyEditors.None);
			tmaxProperties.Add(DXTRANSCRIPT_PROP_FIRST_PL, "First PG:LN", CTmaxToolbox.PLToString(FirstPL), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXTRANSCRIPT_PROP_LAST_PL, "Last PG:LN", CTmaxToolbox.PLToString(LastPL), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXTRANSCRIPT_PROP_LINES_PER_PAGE, "Lines Per Page", LinesPerPage, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			tmaxProperties.Add(DXTRANSCRIPT_PROP_FILENAME, "Transcript Filename", Filename, TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
		
			//	Can we get to the path?
			if((m_dxPrimary != null) && (this.Database != null))
				tmaxProperties.Add(DXTRANSCRIPT_PROP_PATH, "Transcript Path", this.Database.GetFileSpec(this), TmaxPropertyCategories.Media, TmaxPropGridEditors.None);
			
		}// public virtual void GetProperties(CTmaxProperties tmaxProperties)
		
		/// <summary>This method will refresh the value of the property specified by the caller</summary>
		/// <param name="tmaxProperty">The property to be refreshed</param>
		public override void RefreshProperty(CTmaxProperty tmaxProperty)
		{
			//	Which property do we have to refresh
			switch(tmaxProperty.Id)
			{
				case DXTRANSCRIPT_PROP_DEPONENT:
				
					tmaxProperty.Value = Deponent;
					break;
					
				case DXTRANSCRIPT_PROP_DEPOSED_ON:
				
					tmaxProperty.Value = DeposedOn;
					break;
					
				case DXTRANSCRIPT_PROP_FIRST_PL:
				
					tmaxProperty.Value = CTmaxToolbox.PLToString(FirstPL);
					break;
					
				case DXTRANSCRIPT_PROP_LAST_PL:
				
					tmaxProperty.Value = CTmaxToolbox.PLToString(LastPL);
					break;
					
				case DXTRANSCRIPT_PROP_LINES_PER_PAGE:
				
					tmaxProperty.Value = LinesPerPage;
					break;
					
				case DXTRANSCRIPT_PROP_FILENAME:
				
					tmaxProperty.Value = Filename;
					break;
					
				case DXTRANSCRIPT_PROP_PATH:
				
					if(this.Database != null)
						tmaxProperty.Value = this.Database.GetFileSpec(this);
					break;
					
				default:
				
					//	We don't use base class properties so no need to call
					//	the base class
					
					break;
					
			}// switch(tmaxProperty.Id)
		
		}// public virtual void RefreshProperty(CTmaxProperty tmaxProperty)
		
		/// <summary>Called to get the DeposedOn value</summary>
		/// <returns>The DeposedOn date</returns>
		public string GetDeposedOn()
		{
			//	Do we have to try to get the value?
			if(m_strDeposedOn.Length == 0)
			{
				if(this.Primary != null)
				{
					m_strDeposedOn = CTmaxToolbox.ParseDateFromString(this.Primary.MediaId);
					if(m_strDeposedOn.Length == 0)
						m_strDeposedOn = CTmaxToolbox.ParseDateFromString(this.Primary.Name);
				}

			}// if(m_strDeposedOn.Length == 0)
			
			return m_strDeposedOn;

		}// public string GetDeposedOn()
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The AutoId of the primary record that owns this record</summary>
		public long PrimaryId
		{
			get
			{
				return m_lPrimaryId;
			}
			set
			{
				m_lPrimaryId = value;
			}
		}
		
		/// <summary>The name of the deponent</summary>
		public string Deponent
		{
			get
			{
				return m_strDeponent;
			}
			set
			{
				m_strDeponent = value;
			}
		}
		
		/// <summary>The date of the transcript</summary>
		public string DeposedOn
		{
			get
			{
				return GetDeposedOn();
			}
			set
			{
				m_strDeposedOn = value;
			}
		}
		
		/// <summary>The source XML deposition file</summary>
		public CXmlDeposition XmlDeposition
		{
			get
			{
				return m_xmlDeposition;
			}
			set
			{
				m_xmlDeposition = value;
			}
		}
		
		/// <summary>The name of the XML deposition file</summary>
		public string Filename
		{
			get
			{
				return m_strFilename;
			}
			set
			{
				m_strFilename = value;
			}
		}
		
		/// <summary>The number of lines per transcript page</summary>
		public int LinesPerPage
		{
			get
			{
				return m_iLinesPerPage;
			}
			set
			{
				m_iLinesPerPage = value;
			}
		}
		
		/// <summary>The PL value of the first line of text</summary>
		public long FirstPL
		{
			get
			{
				return m_lFirstPL;
			}
			set
			{
				m_lFirstPL = value;
			}
		}
		
		/// <summary>The PL value of the last line of text</summary>
		public long LastPL
		{
			get
			{
				return m_lLastPL;
			}
			set
			{
				m_lLastPL = value;
			}
		}
		
		/// <summary>The primary object that owns this record</summary>
		public CDxPrimary Primary
		{
			get
			{
				return m_dxPrimary;
			}
			set
			{
				m_dxPrimary = value;
			}
		}
		
		#endregion Properties
	
	}// class CDxTranscript

	/// <summary>
	/// This class is used to manage a ArrayList of CDxTranscript objects
	/// </summary>
	public class CDxTranscripts : CDxMediaRecords
	{
		#region Constants
		
		public enum eFields
		{
			AutoId = 0,
			PrimaryId,
			Deponent,
			DeposedOn,
			Filename,
			FirstPL,
			LastPL,
			LinesPerPage,
		}

		public const string TABLE_NAME = "Transcripts";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Primary property</summary>
		protected CDxPrimary m_dxPrimary = null;
		
		#endregion
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxTranscripts() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxTranscripts(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxTranscript">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxTranscript Add(CDxTranscript dxTranscript)
		{
			return (CDxTranscript)base.Add(dxTranscript);
			
		}// Add(CDxTranscript dxTranscript)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxTranscripts Dispose()
		{
			return (CDxTranscripts)base.Dispose();
			
		}// Dispose()

		/// <summary>
		/// Called to locate the object with the specified AutoId value
		/// </summary>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxTranscript Find(long lAutoId, bool bBarcode)
		{
			return (CDxTranscript)base.Find(lAutoId, bBarcode);
			
		}//	Find(long lAutoId)

		/// <summary>Called to locate the object with the specified AutoId or BarcodeId value</summary>
		/// <param name="lAutoId">The id to be located</param>
		/// <returns>The object with the specified AutoId</returns>
		public new CDxTranscript Find(long lAutoId)
		{
			return Find(lAutoId, false);
			
		}//	Find(long lAutoId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CDxTranscript this[int iIndex]
		{
			get
			{
				return (CDxTranscript)base[iIndex];
			}
		}

		/// <summary>
		/// This is the primary media object that owns the collection
		/// </summary>
		public CDxPrimary Primary
		{
			get
			{
				return m_dxPrimary;
			}
			set
			{
				m_dxPrimary = value;
			}
			
		}// Primary property
		
		/// <summary>
		/// Gets the object located at the specified index
		/// </summary>
		/// <returns>Object at the specified index</returns>
		public new CDxTranscript GetAt(int iIndex)
		{
			return (CDxTranscript)base.GetAt(iIndex);
		}

		/// <summary>
		/// This method is called to get the SQL statement required to insert the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxTranscript	dxTranscript = (CDxTranscript)dxRecord;
			string			strSQL = "INSERT INTO " + TableName + "(";
			
			strSQL += (eFields.PrimaryId.ToString() + ",");
			strSQL += (eFields.Deponent.ToString() + ",");
			strSQL += (eFields.DeposedOn.ToString() + ",");
			strSQL += (eFields.Filename.ToString() + ",");
			strSQL += (eFields.FirstPL.ToString() + ",");
			strSQL += (eFields.LastPL.ToString() + ",");
			strSQL += (eFields.LinesPerPage.ToString() + ")");
			
			strSQL += " VALUES(";
			strSQL += ("'" + dxTranscript.PrimaryId.ToString() + "',");
			strSQL += ("'" + SQLEncode(dxTranscript.Deponent) + "',");
			strSQL += ("'" + SQLEncode(dxTranscript.DeposedOn) + "',");
			strSQL += ("'" + SQLEncode(dxTranscript.Filename) + "',");
			strSQL += ("'" + dxTranscript.FirstPL.ToString() + "',");
			strSQL += ("'" + dxTranscript.LastPL.ToString() + "',");
			strSQL += ("'" + dxTranscript.LinesPerPage.ToString() + "')");
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to select the desired records
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string strSQL = "SELECT * FROM " + m_strTableName;
			
			if(m_dxPrimary != null)
			{
				strSQL += " WHERE PrimaryId = ";
				strSQL += m_dxPrimary.AutoId.ToString();
				strSQL += ";";
			}
			else
			{
				Debug.Assert(false);
			}
			
			return strSQL;
		}

		/// <summary>
		/// This method is called to get the SQL statement required to update the specified record
		/// </summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxTranscript	dxTranscript = (CDxTranscript)dxRecord;
			string			strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.PrimaryId.ToString() + " = '" + dxTranscript.PrimaryId.ToString() + "',");
			strSQL += (eFields.Deponent.ToString() + " = '" + SQLEncode(dxTranscript.Deponent) + "',");
			strSQL += (eFields.DeposedOn.ToString() + " = '" + SQLEncode(dxTranscript.DeposedOn) + "',");
			strSQL += (eFields.Filename.ToString() + " = '" + SQLEncode(dxTranscript.Filename) + "',");
			strSQL += (eFields.FirstPL.ToString() + " = '" + dxTranscript.FirstPL.ToString() + "',");
			strSQL += (eFields.LastPL.ToString() + " = '" + dxTranscript.LastPL.ToString() + "',");
			strSQL += (eFields.LinesPerPage.ToString() + " = '" + dxTranscript.LinesPerPage.ToString() + "'");
			
			strSQL += " WHERE AutoId = ";
			strSQL += dxTranscript.AutoId.ToString();
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
			CDxTranscript dxTranscript = new CDxTranscript();
			
			if(dxTranscript != null)
			{
				dxTranscript.Collection = this;
				dxTranscript.Primary = m_dxPrimary;
				
				if(m_dxPrimary != null)
					dxTranscript.PrimaryId = m_dxPrimary.AutoId;
			}
			
			return ((CBaseRecord)dxTranscript);
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxTranscript dxTranscript = (CDxTranscript)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.AutoId].Value  = dxTranscript.AutoId;
					m_dxFields[(int)eFields.PrimaryId].Value  = dxTranscript.PrimaryId;
					m_dxFields[(int)eFields.Deponent].Value = dxTranscript.Deponent;
					m_dxFields[(int)eFields.DeposedOn].Value = dxTranscript.DeposedOn;
					m_dxFields[(int)eFields.Filename].Value = dxTranscript.Filename;
					m_dxFields[(int)eFields.FirstPL].Value  = dxTranscript.FirstPL;
					m_dxFields[(int)eFields.LastPL].Value  = dxTranscript.LastPL;
					m_dxFields[(int)eFields.LinesPerPage].Value = dxTranscript.LinesPerPage;
				}
				else
				{
					dxTranscript.AutoId = (int)(m_dxFields[(int)eFields.AutoId].Value);
					dxTranscript.PrimaryId = (int)(m_dxFields[(int)eFields.PrimaryId].Value);
					dxTranscript.Deponent = (string)(m_dxFields[(int)eFields.Deponent].Value);
					dxTranscript.DeposedOn = (string)(m_dxFields[(int)eFields.DeposedOn].Value);
					dxTranscript.Filename = (string)(m_dxFields[(int)eFields.Filename].Value);
					dxTranscript.FirstPL = (int)(m_dxFields[(int)eFields.FirstPL].Value);
					dxTranscript.LastPL = (int)(m_dxFields[(int)eFields.LastPL].Value);
					dxTranscript.LinesPerPage = (short)(m_dxFields[(int)eFields.LinesPerPage].Value);

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
				case eFields.PrimaryId:
				case eFields.FirstPL:
				case eFields.LastPL:
				case eFields.LinesPerPage:
				
					dxField.Value = 0;
					break;
					
				case eFields.Deponent:
				case eFields.DeposedOn:
				case eFields.Filename:
				
					dxField.Value = "";
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods

	}//	CDxTranscripts
		
}// namespace FTI.Trialmax.Database
