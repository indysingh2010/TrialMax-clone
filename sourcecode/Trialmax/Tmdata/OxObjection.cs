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
	/// <summary>This class is used to manage a record in the Objections table of the Objections database</summary>
	public class COxObjection : FTI.Trialmax.Database.CBaseRecord, ITmaxObjectionRecord
	{
		#region Private Members

		/// <summary>Local member bound to TmaxObjection property</summary>
		private FTI.Shared.Trialmax.CTmaxObjection m_tmaxObjection = null;

		/// <summary>Local member bound to OxDeposition property</summary>
		private COxDeposition m_oxDeposition = null;

		/// <summary>Local member bound to OxState property</summary>
		private COxState m_oxState = null;

		/// <summary>Local member bound to OxRuling property</summary>
		private COxRuling m_oxRuling = null;

		/// <summary>Local member bound to OxModifiedBy property</summary>
		private COxUser m_oxModifiedBy = null;

		/// <summary>Local member bound to OxCase property</summary>
		private COxCase m_oxCase = null;

		/// <summary>Local member bound to DxDeposition property</summary>
		private CDxPrimary m_dxDeposition = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public COxObjection() : base()
		{
		}

		/// <summary>Constructor</summary>
		public COxObjection(CTmaxObjection tmaxObjection) : base()
		{
			m_tmaxObjection = tmaxObjection;
		}

		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.Objection;
		}

		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		/// <remarks>This member MUST be overridden by the derived class</remarks>
		public override string GetKeyId()
		{
			return ("{" + this.UniqueId + "}");
		}

		/// <summary>This method is called to get the application object bound to this record</summary>
		/// <returns>The application object</returns>
		public FTI.Shared.Trialmax.CTmaxObjection GetTmaxObjection()
		{
			//	Do we need to allocate the object?
			if(m_tmaxObjection == null)
				m_tmaxObjection = new CTmaxObjection();

			return m_tmaxObjection;

		}// public FTI.Shared.Trialmax.CTmaxObjection GetTmaxObjection()

		/// <summary>This method is called to set the application object bound to this record</summary>
		/// <param name="tmaxObjection">The application object</param>
		public void SetTmaxObjection(FTI.Shared.Trialmax.CTmaxObjection tmaxObjection)
		{
			m_tmaxObjection = tmaxObjection;

		}// public void SetTmaxObjection(FTI.Shared.Trialmax.CTmaxObjection tmaxObjection)

		/// <summary>Called to get the deposition associated with this objection</summary>
		/// <returns>The deposition record stored in the objections database</returns>
		public COxDeposition GetOxDeposition()
		{
		    COxDepositions oxDepositions = null;
		
		    //	Do we need to get the exchange interface?
			if((m_oxDeposition == null) && (this.Database != null))
		    {
				oxDepositions = ((CObjectionsDatabase)(this.Database)).OxDepositions;
				
		        if(oxDepositions != null)
					m_oxDeposition = oxDepositions.Find(this.DepositionId, false);

		    }// if((this.TmaxObjection.IOxDeposition == null) && (this.Database != null))

			return m_oxDeposition;

		}// public COxDeposition GetOxDeposition()
		
		/// <summary>Called to set the deposition that this objection is based on</summary>
		/// <param name="oxDeposition">The deposition bound to this objection</param>
		public void SetOxDeposition(COxDeposition oxDeposition)
		{
			m_oxDeposition = oxDeposition;
			
			this.TmaxObjection.SetIOxDeposition(m_oxDeposition);

		}// public void SetOxDeposition(COxDeposition oxDeposition)

		/// <summary>Called to get the associated deposition record from the active case database</summary>
		/// <returns>The deposition record stored in the case database</returns>
		public CDxPrimary GetDxDeposition()
		{
			CTmaxCaseDatabase tmaxCaseDatabase = null;

			//	Do we need to find the deposition?
			if(m_dxDeposition == null)
			{
				//	It may have been set via the application
				if(this.TmaxObjection.ICaseDeposition != null)
				{
					m_dxDeposition = (this.TmaxObjection.ICaseDeposition as CDxPrimary);
				}
				else if((this.OxDeposition != null) && (this.Database != null))
				{
					if((tmaxCaseDatabase = ((CObjectionsDatabase)(this.Database)).CaseDatabase) != null)
					{
						if((tmaxCaseDatabase != null) && (tmaxCaseDatabase.Primaries != null))
							m_dxDeposition = tmaxCaseDatabase.Primaries.Find(this.OxDeposition.MediaId);
					}
				
				}

			}// if((m_dxDeposition == null) && (this.Database != null))

			return m_dxDeposition;

		}// public CDxPrimary GetDxDeposition()

		/// <summary>Called to set the case deposition that this objection is based on</summary>
		/// <param name="dxDeposition">The deposition bound to this objection</param>
		public void SetDxDeposition(CDxPrimary dxDeposition)
		{
			m_dxDeposition = dxDeposition;
			this.TmaxObjection.ICaseDeposition = (dxDeposition as ITmaxDeposition);

		}// public void SetDxDeposition(CDxPrimary dxDeposition)

		/// <summary>Called to get the State associated with this objection</summary>
		/// <returns>The State record stored in the objections database</returns>
		public COxState GetOxState()
		{
			COxStates oxStates = null;

			//	Do we need to get the exchange interface?
			if((m_oxState == null) && (this.Database != null))
			{
				oxStates = ((CObjectionsDatabase)(this.Database)).OxStates;

				if(oxStates != null)
					m_oxState = oxStates.Find(this.StateId, false);

			}// if((m_oxState == null) && (this.Database != null))

			return m_oxState;

		}// public COxState GetOxState()

		/// <summary>Called to set the State that this objection is based on</summary>
		/// <param name="oxState">The State bound to this objection</param>
		public void SetOxState(COxState oxState)
		{
			m_oxState = oxState;
			this.TmaxObjection.SetIOxState(oxState);
		}

		/// <summary>Called to get the Ruling associated with this objection</summary>
		/// <returns>The Ruling record stored in the objections database</returns>
		public COxRuling GetOxRuling()
		{
			COxRulings oxRulings = null;

			//	Do we need to get the exchange interface?
			if((m_oxRuling == null) && (this.Database != null))
			{
				oxRulings = ((CObjectionsDatabase)(this.Database)).OxRulings;

				if(oxRulings != null)
					m_oxRuling = oxRulings.Find(this.RulingId, false);

			}// if((m_oxRuling == null) && (this.Database != null))

			return m_oxRuling;

		}// public COxRuling GetOxRuling()

		/// <summary>Called to set the Ruling that this objection is based on</summary>
		/// <param name="oxRuling">The Ruling bound to this objection</param>
		public void SetOxRuling(COxRuling oxRuling)
		{
			m_oxRuling = oxRuling;
			this.TmaxObjection.SetIOxRuling(oxRuling);
		}

		/// <summary>Called to get the ModifiedBy associated with this objection</summary>
		/// <returns>The ModifiedBy user record stored in the objections database</returns>
		public COxUser GetOxModifiedBy()
		{
			COxUsers oxUsers = null;
			
			//	Do we need to get the exchange interface?
			if((m_oxModifiedBy == null) && (this.Database != null))
			{
				oxUsers = ((CObjectionsDatabase)(this.Database)).OxUsers;

				if(oxUsers != null)
					m_oxModifiedBy = oxUsers.Find(this.ModifiedById, false);

			}// if((m_oxModifiedBy == null) && (this.Database != null))

			return m_oxModifiedBy;

		}// public COxUser GetOxModifiedBy()

		/// <summary>Called to set the ModifiedBy user that last modified the record</summary>
		/// <param name="oxState">The ModifiedBy user bound to this objection</param>
		public void SetOxModifiedBy(COxUser oxModifiedBy)
		{
			m_oxModifiedBy = oxModifiedBy;
			this.TmaxObjection.SetIOxModifiedBy(oxModifiedBy);
		}

		/// <summary>Called to get the Case associated with this objection</summary>
		/// <returns>The Case record stored in the objections database</returns>
		public COxCase GetOxCase()
		{
			COxCases oxCases = null;

			//	Do we need to get the exchange interface?
			if((m_oxCase == null) && (this.Database != null))
			{
				oxCases = ((CObjectionsDatabase)(this.Database)).OxCases;

				if(oxCases != null)
					m_oxCase = oxCases.Find(this.CaseId, false);

			}// if((m_oxCase == null) && (this.Database != null))

			return m_oxCase;

		}// public COxCase GetOxCase()

		/// <summary>Called to get the MediaId of the deposition associated with this objection</summary>
		/// <returns>The MediaId of the associated deposition</returns>
		public string GetDeposition()
		{
			return this.Deposition;	
		}

		/// <summary>Called to set the Case that this objection is based on</summary>
		/// <param name="oxCase">The Case bound to this objection</param>
		public void SetOxCase(COxCase oxCase)
		{
			if((m_oxCase = oxCase) != null)
				this.TmaxObjection.Case = oxCase.TmaxCase;
			else
				this.TmaxObjection.Case = null;

		}// public void SetOxCase(COxCase oxCase)

		/// <summary>This method retrieves the identifier assigned by the database</summary>
		///	<returns>The record id</returns>
		string ITmaxBaseObjectionRecord.GetUniqueId()
		{
			return this.UniqueId;
		}

		/// <summary>This method retrieves the text used to display this record</summary>
		///	<returns>The display text</returns>
		string ITmaxBaseObjectionRecord.GetText()
		{
			return this.Argument;
		}

		/// <summary>This method retrieves the interface to the Deposition record</summary>
		///	<returns>The record interface</returns>
		ITmaxBaseObjectionRecord ITmaxObjectionRecord.GetIOxDeposition()
		{
			return (GetOxDeposition() as ITmaxBaseObjectionRecord);
		}

		/// <summary>This method retrieves the interface to the State record</summary>
		///	<returns>The display text</returns>
		ITmaxBaseObjectionRecord ITmaxObjectionRecord.GetIOxState()
		{
			return (GetOxState() as ITmaxBaseObjectionRecord);
		}

		/// <summary>This method retrieves the interface to the Ruling record</summary>
		///	<returns>The display text</returns>
		ITmaxBaseObjectionRecord ITmaxObjectionRecord.GetIOxRuling()
		{
			return (GetOxRuling() as ITmaxBaseObjectionRecord);
		}

		/// <summary>This method retrieves the interface to the ModifiedBy user record</summary>
		///	<returns>The associated exchange interface</returns>
		ITmaxBaseObjectionRecord ITmaxObjectionRecord.GetIOxModifiedBy()
		{
			return (GetOxModifiedBy() as ITmaxBaseObjectionRecord);
		}

		/// <summary>This method retrieves the interface to the Case descriptor</summary>
		///	<returns>The display text</returns>
		CTmaxCase ITmaxObjectionRecord.GetCase()
		{
			if(this.OxCase != null)
				return this.OxCase.TmaxCase;
			else
				return null;
		}

		#endregion Public Methods

		#region Properties

		/// <summary>The application object bound to this record</summary>
		public FTI.Shared.Trialmax.CTmaxObjection TmaxObjection
		{
			get { return GetTmaxObjection(); }
			set { SetTmaxObjection(value); }
		}

		/// <summary>The unique identifier assigned by the database</summary>
		public string UniqueId
		{
			get { return this.TmaxObjection.UniqueId; }
			set { this.TmaxObjection.UniqueId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Cases table</summary>
		public string CaseId
		{
			get { return this.TmaxObjection.CaseId; }
			set { this.TmaxObjection.CaseId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Depositions table</summary>
		public string DepositionId
		{
			get { return this.TmaxObjection.DepositionId; }
			set { this.TmaxObjection.DepositionId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the States table</summary>
		public string StateId
		{
			get { return this.TmaxObjection.StateId; }
			set { this.TmaxObjection.StateId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Rulings table</summary>
		public string RulingId
		{
			get { return this.TmaxObjection.RulingId; }
			set { this.TmaxObjection.RulingId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Users table</summary>
		public string ModifiedById
		{
			get { return this.TmaxObjection.ModifiedById; }
			set { this.TmaxObjection.ModifiedById = value; }
		}

		/// <summary>The MediaId of the deposition</summary>
		public string Deposition
		{
			get { return this.TmaxObjection.Deposition; }
			set { this.TmaxObjection.Deposition = value; }
		}

		/// <summary>The State of the objection</summary>
		public string State
		{
			get { return this.TmaxObjection.State; }
			set { this.TmaxObjection.State = value; }
		}

		/// <summary>The Ruling of the objection</summary>
		public string Ruling
		{
			get { return this.TmaxObjection.Ruling; }
			set { this.TmaxObjection.Ruling = value; }
		}

		/// <summary>True if objection raised by the plaintiff</summary>
		public bool Plaintiff
		{
			get { return this.TmaxObjection.Plaintiff; }
			set { this.TmaxObjection.Plaintiff = value; }
		}

		/// <summary>The description of the objection</summary>
		public string Argument
		{
			get { return this.TmaxObjection.Argument; }
			set { this.TmaxObjection.Argument = value; }
		}

		/// <summary>The first response</summary>
		public string Response1
		{
			get { return this.TmaxObjection.Response1; }
			set { this.TmaxObjection.Response1 = value; }
		}

		/// <summary>The second response</summary>
		public string Response2
		{
			get { return this.TmaxObjection.Response2; }
			set { this.TmaxObjection.Response2 = value; }
		}

		/// <summary>The third response</summary>
		public string Response3
		{
			get { return this.TmaxObjection.Response3; }
			set { this.TmaxObjection.Response3 = value; }
		}

		/// <summary>The supplemental text for the current ruling</summary>
		public string RulingText
		{
			get { return this.TmaxObjection.RulingText; }
			set { this.TmaxObjection.RulingText = value; }
		}

		/// <summary>The work product description</summary>
		public string WorkProduct
		{
			get { return this.TmaxObjection.WorkProduct; }
			set { this.TmaxObjection.WorkProduct = value; }
		}

		/// <summary>The user comments</summary>
		public string Comments
		{
			get { return this.TmaxObjection.Comments; }
			set { this.TmaxObjection.Comments = value; }
		}

		/// <summary>The first page/line of transcript text</summary>
		public long FirstPL
		{
			get { return this.TmaxObjection.FirstPL; }
			set { this.TmaxObjection.FirstPL = value; }
		}

		/// <summary>The last page/line of transcript text</summary>
		public long LastPL
		{
			get { return this.TmaxObjection.LastPL; }
			set { this.TmaxObjection.LastPL = value; }
		}

		/// <summary>The date the record was last modified</summary>
		public System.DateTime ModifiedOn
		{
			get { return this.TmaxObjection.ModifiedOn; }
			set { this.TmaxObjection.ModifiedOn = value; }
		}

		/// <summary>The name of the user to last modify this record</summary>
		public string ModifiedBy
		{
			get { return this.TmaxObjection.ModifiedBy; }
			set { this.TmaxObjection.ModifiedBy = value; }
		}

		/// <summary>The exchange interface to the Deposition record in the objections database</summary>
		public COxDeposition OxDeposition
		{
			get { return GetOxDeposition(); }
			set { SetOxDeposition(value); }
		}

		/// <summary>The exchange interface to the Deposition record stored in the case database</summary>
		public CDxPrimary DxDeposition
		{
			get { return GetDxDeposition(); }
			set { SetDxDeposition(value); }
		}

		/// <summary>The exchange interface to the State record in the objections database</summary>
		public COxState OxState
		{
			get { return GetOxState(); }
			set { SetOxState(value); }
		}

		/// <summary>The exchange interface to the Ruling record in the objections database</summary>
		public COxRuling OxRuling
		{
			get { return GetOxRuling(); }
			set { SetOxRuling(value); }
		}

		/// <summary>The exchange interface to the ModifiedBy record in the objections database</summary>
		public COxUser OxModifiedBy
		{
			get { return GetOxModifiedBy(); }
			set { SetOxModifiedBy(value); }
		}

		/// <summary>The exchange interface to the Case record in the objections database</summary>
		public COxCase OxCase
		{
			get { return GetOxCase(); }
			set { SetOxCase(value); }
		}

		#endregion Properties

	}// public class COxObjection : FTI.Trialmax.Database.CBaseRecord

	/// <summary>This class is used to manage a ArrayList of COxObjection objects</summary>
	public class COxObjections:CBaseRecords
	{
		public enum eFields
		{
			UniqueId = 0,
			CaseId,
			DepositionId,
			StateId,
			RulingId,
			Plaintiff,
			FirstPL,
			LastPL,
			Argument,
			RulingText,
			Response1,
			Response2,
			Response3,
			WorkProduct,
			Comments,
			ModifiedBy,
			ModifiedOn,
		}

		public const string TABLE_NAME = "Objections";

		#region Public Members

		/// <summary>Default constructor</summary>
		public COxObjections() : base()
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public COxObjections(CObjectionsDatabase tmaxDatabase) : base(tmaxDatabase)
		{
			m_strKeyFieldName = eFields.UniqueId.ToString();
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="OxObjection">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public COxObjection Add(COxObjection OxObjection)
		{
			COxObjection oxAdded = null;
			CObjectionsDatabase dbObjections = null;

			//	Should we automatically assign the state?
			if((dbObjections = (CObjectionsDatabase)(this.Database)) != null)
				OxObjection.TmaxObjection.SetAutoState(dbObjections.OxStates);

			oxAdded = (COxObjection)base.Add(OxObjection);

			return oxAdded;

		}// public COxObjection Add(COxObjection OxObjection)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new COxObjections Dispose()
		{
			return (COxObjections)base.Dispose();

		}// public new COxObjections Dispose()

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new COxObjection this[int iIndex]
		{
			get { return (COxObjection)base[iIndex]; }

		}// public new COxObjection this[int iIndex]

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new COxObjection GetAt(int iIndex)
		{
			return (COxObjection)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			COxObjection oxObjection = (COxObjection)dxRecord;
			string strSQL = "INSERT INTO " + TableName + "(";

			//	Should we assign the UniqueId?
			if(oxObjection.UniqueId.Length == 0)
				oxObjection.UniqueId = System.Guid.NewGuid().ToString();

			strSQL += (eFields.UniqueId + ",");
			strSQL += (eFields.CaseId.ToString() + ",");
			strSQL += (eFields.DepositionId.ToString() + ",");
			strSQL += (eFields.StateId.ToString() + ",");
			strSQL += (eFields.RulingId.ToString() + ",");
			strSQL += (eFields.Plaintiff.ToString() + ",");
			strSQL += (eFields.FirstPL.ToString() + ",");
			strSQL += (eFields.LastPL.ToString() + ",");
			strSQL += (eFields.Argument.ToString() + ",");
			strSQL += (eFields.RulingText.ToString() + ",");
			strSQL += (eFields.Response1.ToString() + ",");
			strSQL += (eFields.Response2.ToString() + ",");
			strSQL += (eFields.Response3.ToString() + ",");
			strSQL += (eFields.WorkProduct.ToString() + ",");
			strSQL += (eFields.Comments.ToString() + ",");
			strSQL += (eFields.ModifiedBy.ToString() + ",");
			strSQL += (eFields.ModifiedOn.ToString() + ")");

			strSQL += " VALUES(";
			strSQL += ("'" + oxObjection.UniqueId + "',");
			strSQL += ("'" + SQLEncode(oxObjection.CaseId) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.DepositionId) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.StateId) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.RulingId) + "',");
			strSQL += ("'" + BoolToSQL(oxObjection.Plaintiff) + "',");
			strSQL += ("'" + oxObjection.FirstPL.ToString() + "',");
			strSQL += ("'" + oxObjection.LastPL.ToString() + "',");
			strSQL += ("'" + SQLEncode(oxObjection.Argument) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.RulingText) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.Response1) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.Response2) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.Response3) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.WorkProduct) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.Comments) + "',");
			strSQL += ("'" + SQLEncode(oxObjection.ModifiedById) + "',");
			strSQL += ("'" + oxObjection.ModifiedOn.ToString() + "')");

			return strSQL;

		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			COxObjection oxObjection = (COxObjection)dxRecord;
			string strSQL = "UPDATE " + TableName + " SET ";

			strSQL += (eFields.CaseId.ToString() + " = '" + SQLEncode(oxObjection.CaseId) + "',");
			strSQL += (eFields.DepositionId.ToString() + " = '" + SQLEncode(oxObjection.DepositionId) + "',");
			strSQL += (eFields.StateId.ToString() + " = '" + SQLEncode(oxObjection.StateId) + "',");
			strSQL += (eFields.RulingId.ToString() + " = '" + SQLEncode(oxObjection.RulingId) + "',");
			strSQL += (eFields.Plaintiff.ToString() + " = '" + BoolToSQL(oxObjection.Plaintiff) + "',");
			strSQL += (eFields.FirstPL.ToString() + " = '" + oxObjection.FirstPL.ToString() + "',");
			strSQL += (eFields.LastPL.ToString() + " = '" + oxObjection.LastPL.ToString() + "',");
			strSQL += (eFields.Argument.ToString() + " = '" + SQLEncode(oxObjection.Argument) + "',");
			strSQL += (eFields.RulingText.ToString() + " = '" + SQLEncode(oxObjection.RulingText) + "',");
			strSQL += (eFields.Response1.ToString() + " = '" + SQLEncode(oxObjection.Response1) + "',");
			strSQL += (eFields.Response2.ToString() + " = '" + SQLEncode(oxObjection.Response2) + "',");
			strSQL += (eFields.Response3.ToString() + " = '" + SQLEncode(oxObjection.Response3) + "',");
			strSQL += (eFields.WorkProduct.ToString() + " = '" + SQLEncode(oxObjection.WorkProduct) + "',");
			strSQL += (eFields.Comments.ToString() + " = '" + SQLEncode(oxObjection.Comments) + "',");
			strSQL += (eFields.ModifiedBy.ToString() + " = '" + SQLEncode(oxObjection.ModifiedById) + "',");
			strSQL += (eFields.ModifiedOn.ToString() + " = '" + oxObjection.ModifiedOn.ToString() + "'");

			strSQL += " WHERE UniqueId = '{";
			strSQL += oxObjection.UniqueId;
			strSQL += "}';";

			return strSQL;

		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public override bool Update(CBaseRecord dxRecord)
		{
			CObjectionsDatabase dbObjections = null;
			
			//	Make sure the user information is updated
			((COxObjection)dxRecord).ModifiedById = (((CObjectionsDatabase)(this.Database)).GetUserId()).ToString();
			((COxObjection)dxRecord).ModifiedOn   = System.DateTime.Now;

			//	Check to see the the state should transition
			if((dbObjections = (CObjectionsDatabase)(this.Database)) != null)
				((COxObjection)dxRecord).TmaxObjection.SetAutoState(dbObjections.OxStates);
			
			return base.Update(dxRecord);

		}// public override bool Update(CBaseRecord dxRecord)

		/// <summary>Called to locate the object with the specified UniqueId value</summary>
		/// <param name="uniqueId">The UniqueId value of the desired object</param>
		/// <returns>The object with the specified UniqueId</returns>
		public COxObjection Find(System.Guid uniqueId)
		{
			return Find(uniqueId.ToString());
		}

		/// <summary>Called to locate the object with the specified UniqueId value</summary>
		/// <param name="strUniqueId">The UniqueId value of the desired object</param>
		/// <returns>The object with the specified UniqueId</returns>
		public COxObjection Find(string strUniqueId)
		{
			// Search for the requested object
			if(m_aList != null)
			{
				foreach(COxObjection O in m_aList)
				{
					if(String.Compare(O.UniqueId, strUniqueId, true) == 0)
					{
						return O;
					}
				}
			}

			return null;

		}//	public COxObjection Find(System.Guid uniqueId)

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
			return ((CBaseRecord)(new COxObjection()));
		}

		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord,bool bSetFields)
		{
			COxObjection oxObjection = (COxObjection)dxRecord;

			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;

			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.UniqueId].Value = new System.Guid(oxObjection.UniqueId);
					m_dxFields[(int)eFields.CaseId].Value = new System.Guid(oxObjection.CaseId);
					m_dxFields[(int)eFields.DepositionId].Value = new System.Guid(oxObjection.DepositionId);
					m_dxFields[(int)eFields.StateId].Value = new System.Guid(oxObjection.StateId);
					m_dxFields[(int)eFields.RulingId].Value = new System.Guid(oxObjection.RulingId);
					m_dxFields[(int)eFields.Plaintiff].Value = oxObjection.Plaintiff;
					m_dxFields[(int)eFields.FirstPL].Value = oxObjection.FirstPL;
					m_dxFields[(int)eFields.LastPL].Value = oxObjection.LastPL;
					m_dxFields[(int)eFields.Argument].Value = oxObjection.Argument;
					m_dxFields[(int)eFields.RulingText].Value = oxObjection.RulingText;
					m_dxFields[(int)eFields.Response1].Value = oxObjection.Response1;
					m_dxFields[(int)eFields.Response2].Value = oxObjection.Response2;
					m_dxFields[(int)eFields.Response3].Value = oxObjection.Response3;
					m_dxFields[(int)eFields.WorkProduct].Value = oxObjection.WorkProduct;
					m_dxFields[(int)eFields.Comments].Value = oxObjection.Comments;
					m_dxFields[(int)eFields.ModifiedBy].Value = new System.Guid(oxObjection.ModifiedById);
					m_dxFields[(int)eFields.ModifiedOn].Value = oxObjection.ModifiedOn;
				}
				else
				{
					oxObjection.UniqueId = m_dxFields[(int)eFields.UniqueId].Value.ToString();
					oxObjection.CaseId = m_dxFields[(int)eFields.CaseId].Value.ToString();
					oxObjection.DepositionId = m_dxFields[(int)eFields.DepositionId].Value.ToString();
					oxObjection.StateId = m_dxFields[(int)eFields.StateId].Value.ToString();
					oxObjection.RulingId = m_dxFields[(int)eFields.RulingId].Value.ToString();
					oxObjection.Plaintiff = (bool)(m_dxFields[(int)eFields.Plaintiff].Value);
					oxObjection.FirstPL = (int)(m_dxFields[(int)eFields.FirstPL].Value);
					oxObjection.LastPL = (int)(m_dxFields[(int)eFields.LastPL].Value);
					oxObjection.Argument = m_dxFields[(int)eFields.Argument].Value.ToString();
					oxObjection.RulingText = m_dxFields[(int)eFields.RulingText].Value.ToString();
					oxObjection.Response1 = m_dxFields[(int)eFields.Response1].Value.ToString();
					oxObjection.Response2 = m_dxFields[(int)eFields.Response2].Value.ToString();
					oxObjection.Response3 = m_dxFields[(int)eFields.Response3].Value.ToString();
					oxObjection.WorkProduct = m_dxFields[(int)eFields.WorkProduct].Value.ToString();
					oxObjection.Comments = m_dxFields[(int)eFields.Comments].Value.ToString();
					oxObjection.ModifiedById = m_dxFields[(int)eFields.ModifiedBy].Value.ToString();
					oxObjection.ModifiedOn = (DateTime)(m_dxFields[(int)eFields.ModifiedOn].Value);
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
		private void SetValue(eFields eField,CDxField dxField)
		{
			switch(eField)
			{
				case eFields.UniqueId:
				case eFields.CaseId:
				case eFields.DepositionId:
				case eFields.StateId:
				case eFields.RulingId:
				case eFields.ModifiedBy:

					dxField.Value = System.Guid.Empty;
					break;

				case eFields.Plaintiff:

					dxField.Value = true;
					break;

				case eFields.FirstPL:
				case eFields.LastPL:

					dxField.Value = 0;
					break;

				case eFields.ModifiedOn:

					dxField.Value = System.DateTime.Now;
					break;

				case eFields.Argument:
				case eFields.RulingText:
				case eFields.Response1:
				case eFields.Response2:
				case eFields.Response3:
				case eFields.WorkProduct:
				case eFields.Comments:

					dxField.Value = "";
					break;

				default:

					Debug.Assert(false,"SetValue() - unknown field identifier - " + eField.ToString());
					break;

			}// switch(eField)

		}// private void SetValue(eFields eField, CDxField dxField)

		#endregion Private Methods

	}//	public class COxObjection : FTI.Trialmax.Database.CBaseRecord

}// namespace FTI.Trialmax.Database
