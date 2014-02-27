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
	public class CCxCodesInfo : FTI.Trialmax.Database.CBaseRecord
	{
		#region Constants

		public const string TABLE_NAME = "Info";

		//	Enumerated field (column) identifiers
		public enum eFields
		{
			Version = 0,
			CaseId,
			CaseName,
			Concatenated,
			ConcatenationChars,
			CreatedBy,
			CreatedOn,
		}

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to Version property</summary>
		private string m_strVersion = "";

		/// <summary>Local member bound to CaseId property</summary>
		private string m_strCaseId = "";

		/// <summary>Local member bound to CaseName property</summary>
		private string m_strCaseName = "";

		/// <summary>Local member bound to Concatenated property</summary>
		private bool m_bConcatenated = false;

		/// <summary>Local member bound to ConcatenationChars property</summary>
		private string m_strConcatenationChars = "";

		/// <summary>Local member bound to CreatedBy property</summary>
		private string m_strCreatedBy = "";

		/// <summary>Local member bound to CreatedOn property</summary>
		protected DateTime m_tsCreatedOn = System.DateTime.Now;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CCxCodesInfo()
			: base()
		{
		}

		/// <summary>This method is called to get the SQL statement required to insert this record</summary>
		/// <returns>The appropriate SQL statement</returns>
		public string GetSQLInsert()
		{
			string strSQL = "INSERT INTO " + TABLE_NAME + "(";

			strSQL += (eFields.Version.ToString() + ",");
			strSQL += (eFields.CaseId.ToString() + ",");
			strSQL += (eFields.CaseName.ToString() + ",");
			strSQL += (eFields.Concatenated.ToString() + ",");
			strSQL += (eFields.ConcatenationChars.ToString() + ",");
			strSQL += (eFields.CreatedBy.ToString() + ",");
			strSQL += (eFields.CreatedOn.ToString());

			strSQL += ")";

			strSQL += " VALUES(";

			strSQL += ("'" + CTmaxToolbox.SQLEncode(this.Version) + "',");
			strSQL += ("'" + CTmaxToolbox.SQLEncode(this.CaseId) + "',");
			strSQL += ("'" + CTmaxToolbox.SQLEncode(this.CaseName) + "',");
			strSQL += ("'" + CTmaxToolbox.BoolToSQL(this.Concatenated) + "',");
			strSQL += ("'" + CTmaxToolbox.SQLEncode(this.ConcatenationChars) + "',");
			strSQL += ("'" + CTmaxToolbox.SQLEncode(this.CreatedBy) + "',");
			strSQL += ("'" + this.CreatedOn.ToString() + "'");

			strSQL += ")";

			return strSQL;

		}// public override string GetSQLInsert(CDxBaseRecord dxRecord)

		/// <summary>Called to use the data set row to set the object properties</summary>
		/// <param name="dxRow"></param>
		/// <returns></returns>
		public bool SetProps(System.Data.DataRow dxRow)
		{
			bool bSuccessful = false;

			try
			{
				m_strVersion = dxRow.ItemArray[(int)eFields.Version].ToString();
				m_strCaseId = dxRow.ItemArray[(int)eFields.CaseId].ToString();
				m_strCaseName = dxRow.ItemArray[(int)eFields.CaseName].ToString();
				m_strVersion = dxRow.ItemArray[(int)eFields.Version].ToString();
				m_bConcatenated = CTmaxToolbox.StringToBool(dxRow.ItemArray[(int)eFields.Concatenated].ToString());
				m_strConcatenationChars = dxRow.ItemArray[(int)eFields.ConcatenationChars].ToString();
				m_strCreatedBy = dxRow.ItemArray[(int)eFields.CreatedBy].ToString();
				m_tsCreatedOn = (DateTime)(dxRow.ItemArray[(int)eFields.CreatedOn]);
				bSuccessful = true;
			}
			catch
			{
			}

			return bSuccessful;

		}// public bool SetProps(System.Data.DataRow dxRow)

		#endregion Public Methods

		#region Properties

		/// <summary>The version of the TrialMax assembly that created the database</summary>
		public string Version
		{
			get { return m_strVersion; }
			set { m_strVersion = value; }
		}

		/// <summary>The id of the TrialMax case that created the database</summary>
		public string CaseId
		{
			get { return m_strCaseId; }
			set { m_strCaseId = value; }
		}

		/// <summary>The name of the TrialMax case that created the database</summary>
		public string CaseName
		{
			get { return m_strCaseName; }
			set { m_strCaseName = value; }
		}

		/// <summary>True if the database is designed for concatenated codes</summary>
		public bool Concatenated
		{
			get { return m_bConcatenated; }
			set { m_bConcatenated = value; }
		}

		/// <summary>The characters used to concatenate multiple codes</summary>
		public string ConcatenationChars
		{
			get { return m_strConcatenationChars; }
			set { m_strConcatenationChars = value; }
		}

		/// <summary>Name of the user that created the database</summary>
		public string CreatedBy
		{
			get { return m_strCreatedBy; }
			set { m_strCreatedBy = value; }
		}

		/// <summary>The date the database was created</summary>
		public DateTime CreatedOn
		{
			get { return m_tsCreatedOn; }
			set { m_tsCreatedOn = value; }
		}

		#endregion Properties

	}// public class CCxCodesInfo : FTI.Trialmax.Database.CDxBaseRecord

}// namespace FTI.Trialmax.Database
