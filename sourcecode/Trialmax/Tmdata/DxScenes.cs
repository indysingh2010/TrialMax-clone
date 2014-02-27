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
	/// <summary>This class is used to manage an array of scene records</summary>
	public class CDxScenes : CDxSecondaries
	{
		#region Private Members
		
		/// <summary>Local member bound to Source property</summary>
		CDxMediaRecord m_dxSource = null;
		
		/// <summary>Local member to temporarily store connected records</summary>
		private CDxSecondaries m_dxConnected = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxScenes() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxScenes(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>
		/// This method is called to get the SQL statement required to flush all records belonging to the collection
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLFlush()
		{
			string strSQL = ("DELETE FROM " + TableName);
			string strId = "";

            if((m_dxSource == null) || (this.Database == null))
				return "";
			
			//	Get the PSTQ id for the source media
            strId = this.Database.GetUniqueId(m_dxSource);
			if((strId == null) || (strId.Length == 0)) 
				return "";
				
			strSQL += " WHERE " + eFields.MediaType.ToString() + " = " + ((int)TmaxMediaTypes.Scene).ToString();
			strSQL += " AND (";
			strSQL += (eFields.SourceId.ToString() + " = '" + strId + "'");
			strSQL += " OR ";
			strSQL += (eFields.SourceId.ToString() + " LIKE '" + strId + ".%')");
			strSQL += ";";

			return strSQL;
		}
		
		/// <summary>
		/// This method is called to get the SQL statement required to select the desired records
		/// </summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string	strSQL = "SELECT * FROM " + m_strTableName;
			string	strId = "";
			
			Debug.Assert(m_dxSource != null);
            Debug.Assert(this.Database != null);

            if((m_dxSource == null) || (this.Database == null))
				return "";
			
			//	Get the PSTQ id for the source media
            strId = this.Database.GetUniqueId(m_dxSource);
			if((strId == null) || (strId.Length == 0)) 
				return "";
				
			strSQL += " WHERE " + eFields.MediaType.ToString() + " = " + ((int)TmaxMediaTypes.Scene).ToString();
			strSQL += " AND (";
			strSQL += (eFields.SourceId.ToString() + " = '" + strId + "'");
			strSQL += " OR ";
			strSQL += (eFields.SourceId.ToString() + " LIKE '" + strId + ".%')");
			strSQL += " ORDER BY " + eFields.PrimaryMediaId.ToString() + "," + eFields.DisplayOrder.ToString();
			strSQL += ";";
			
			return strSQL;
		}

		/// <summary>This method is called to populate the collection</summary>
		/// <returns>true if successful</returns>
		public override bool Fill()
		{
			CDxSecondary dxConnected = null;
			
			//	Don't bother if we don't have an active database
            if(this.Database == null) return false;
            if(this.Database.Primaries == null) return false;
			
			//	Do the normal base class processing first
			if(base.Fill() == false) return false;
		
			//	Do have any more to do if nothing found
			if(this.Count == 0) return true;
			
			//	Allocate a collection to store the connected records 
            m_dxConnected = new CDxSecondaries(this.Database);
			
			//	Walk the parent chain of each record get to the actual application records
			foreach(CDxSecondary O in this)
			{
				if((dxConnected = Connect(O, true)) != null)
					m_dxConnected.AddList(dxConnected);
			}
			
			//	Clear the collection and transfer the connected
			//	records
			this.Clear();
			
			foreach(CDxSecondary O in m_dxConnected)
				this.AddList(O);
				
			m_dxConnected.Clear();
			return true;
			
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method will walk the parent chain to get the actual application record</summary>
		/// <param name="dxRecord">The source record</param>
		/// <returns>The actual application record</returns>
		protected CDxSecondary Connect(CDxSecondary dxRecord, bool bSilent)
		{
			string strId = "";

			//	Format this record's unique id
			strId = (dxRecord.PrimaryMediaId.ToString() + ".");
			strId += dxRecord.AutoId.ToString();

            return (CDxSecondary)(this.Database.GetRecordFromId(strId,bSilent));
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Media record that is the source for these scenes</summary>
		public CDxMediaRecord Source
		{
			get
			{
				return m_dxSource;
			}
			set
			{
				m_dxSource = value;
			}
			
		}//	Source Property
		
		#endregion Properties
		
	}//	CDxScenes
		
}// namespace FTI.Trialmax.Database
