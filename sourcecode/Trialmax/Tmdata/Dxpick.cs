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
	public class CDxPickItem : FTI.Trialmax.Database.CBaseRecord
	{
		#region Private Members
		
		/// <summary>Local member bound to TmaxPickItem property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickItem = null;
		
		/// <summary>Local member bound to Parent property</summary>
		private CDxPickItem m_dxParent = null;
		
		/// <summary>Local member bound to Children property</summary>
		private CDxPickItems m_dxChildren = null;
		
		/// <summary>Local member bound to ModifiedBy property</summary>
		protected long m_lModifiedBy = 0;
		
		/// <summary>Local member bound to ModifiedOn property</summary>
		protected DateTime m_tsModifiedOn = System.DateTime.Now;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CDxPickItem() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		public CDxPickItem(CDxPickItem dxParent) : base()
		{
			m_dxParent = dxParent;
		}
		
		/// <summary>Called to clear this object's child collection</summary>
		public void Clear()
		{
			try
			{
				//	Flush the children if they exist
//				if(this.Children != null)
//					this.Children.Clear();
			}
			catch
			{
			}
		
		}// public void Clear()
		
		/// <summary>Performs cleanup</summary>
		/// <returns>Null</returns>
		public override CBaseRecord Dispose()
		{
			if(this.Children != null)
				this.Children.Dispose();
				
			if(this.TmaxPickItem != null)
			{
				if(this.TmaxPickItem.Children != null)
					this.TmaxPickItem.Children.Clear();
				this.TmaxPickItem = null;
			}

			//	This allows use to dispose and reset the reference in one line of code
			return null;
			
		}// public override void Dispose()
	
		/// <summary>This method is called to get the application pick item bound to this exchange object</summary>
		/// <returns>The application pick item object bound to this object</returns>
		public FTI.Shared.Trialmax.CTmaxPickItem GetTmaxPickItem()
		{
			//	Do we need to allocate the object
			if(m_tmaxPickItem == null)
				m_tmaxPickItem = new CTmaxPickItem();
				
			return m_tmaxPickItem;
			
		}// public FTI.Shared.Trialmax.CTmaxPickItem GetTmaxPickItem()
		
		/// <summary>This method is called to set the application pick item bound to this exchange object</summary>
		/// <param name="tmaxPickItem">The application item to be bound to this object</param>
		public void SetTmaxPickItem(FTI.Shared.Trialmax.CTmaxPickItem tmaxPickItem)
		{
			m_tmaxPickItem = tmaxPickItem;
			
		}// public void SetTmaxPickItem(FTI.Shared.Trialmax.CTmaxPickItem tmaxPickItem)
		
		/// <summary>This function is called to get the data type of the record</summary>
		/// <returns>The enumerated data type</returns>
		public override TmaxDataTypes GetDataType()
		{
			return TmaxDataTypes.PickItem;
		}
		
		/// <summary>This function is called to get the key identifier of the record in the table</summary>
		/// <returns>The unique primary key identifier</returns>
		public override string GetKeyId()
		{
			return this.UniqueId.ToString();
		}
		
		/// <summary>This function is called to get the parent record</summary>
		/// <returns>The parent record if it exists</returns>
		public CDxPickItem GetParent()
		{
			return m_dxParent;
		}
		
		/// <summary>This method is called to assign the parent record</summary>
		/// <param name="dxParent">The exchange interface for the parent record</param>
		/// <returns>True if successful</returns>
		public bool SetParent(CDxPickItem dxParent)
		{
			this.m_dxParent = dxParent;
			
			if(dxParent != null)
				this.Collection = dxParent.Children;
			
			if(this.TmaxPickItem != null)
			{
				if(this.m_dxParent != null)
					this.TmaxPickItem.ParentId = this.m_dxParent.UniqueId;
				else
					this.TmaxPickItem.ParentId = 0;
					
			}// if(this.TmaxPickItem != null)
			
			return true;
			
		}// public bool SetParent(CDxPickItem dxParent)
		
		/// <summary>This function is called to get the collection of child records</summary>
		/// <returns>The child record collection if it exists</returns>
		public CDxPickItems GetChildren()
		{
			//	Should we allocate the collection?
			if(this.m_dxChildren == null)
			{
				//	Must have a bounded application object that also has children
				if((this.TmaxPickItem != null) && (this.TmaxPickItem.Children != null))
				{
					this.m_dxChildren = new CDxPickItems(this.Database as CTmaxCaseDatabase);
					this.m_dxChildren.Parent = this;
				}
			
			}// if(m_dxChildren == null)
			
			return this.m_dxChildren;
		
		}// public CDxPickItems GetChildren()
		
		/// <summary>This method is called to assign the collection for child records</summary>
		/// <param name="dxChildren">The child record collection</param>
		/// <returns>True if successful</returns>
		public bool SetChildren(CDxPickItems dxChildren)
		{
			this.m_dxChildren = dxChildren;
			
			if(this.m_dxChildren != null)
				this.m_dxChildren.Parent = this;
			
			return true;
			
		}// public bool SetChildren(CDxPickItems dxChildren)
		
		/// <summary>This method is called to fill the collection</summary>
		/// <param name="bChildren">True to fill each of the child items</param>
		/// <returns>true if successful</returns>
		public bool Fill(bool bChildren)
		{
			bool bSuccessful = false;
			
			//	We have to have child records and application child objects
			if(this.Children == null) return false;
			if(this.TmaxPickItem.Children == null) return false;
			
			//	Make sure the collection is using the correct database
			if(this.Collection != null)
				this.Children.Database = this.Collection.Database;
			if(this.Children.Database == null) return false;
						
			//	Clear the existing objects
			this.Children.Clear();
			this.TmaxPickItem.Children.Clear();
			
			//	Make sure the child collection's parent has been assigned
			this.Children.Parent = this;
				
			//	Get the child records from the database			
			if((bSuccessful = this.Children.Fill()) == true)
			{					
				foreach(CDxPickItem O in this.Children)
				{
					//	Assign each child's parent
					O.Parent = this;
						
					//	Add the application object to the list
					if(O.TmaxPickItem != null)
					{
						this.TmaxPickItem.Add(O.TmaxPickItem);						
					
						//	Cross link the exchange object to the application object
						O.TmaxPickItem.DxRecord = O;
						
						//	Should we fill the child's collection?
						if((bChildren == true) && (O.Children != null))
						{
							O.Fill(true);
						}
					
					}// if(O.TmaxPickItem != null)
					
				}// foreach(CDxPickItem O in this.Children)

			}// if((bSuccessful = m_dxChildren.Fill()) == true)

			return bSuccessful;
			
		}// public bool Fill(bool bChildren)
		
		/// <summary>This method is called to add a new child to the collection</summary>
		/// <param name="dxChild">The child to be added</param>
		/// <returns>true if successful</returns>
		public bool Add(CDxPickItem dxChild, bool bAutoId)
		{
			bool	bSuccessful = false;
			bool	bOldAutoId = true;
			
			try
			{	
				//	We have to have child records and application child objects
				if(this.Children == null) return false;
				if(this.TmaxPickItem.Children == null) return false;

				//	Make sure the collection is using the correct database
				if(this.Children.Database == null) return false;

				//	Make sure the child collection's parent has been assigned
				this.Children.Parent = this;
								
				bOldAutoId = this.Children.AutoIdEnabled;
				this.Children.AutoIdEnabled = bAutoId;
				
				//	Add the record to the database
				if(this.Children.Add(dxChild) != null)
				{
					//	Add to the application object's child collection
					this.TmaxPickItem.Add(dxChild.TmaxPickItem);
					
					//	Cross link the application object
					if(dxChild.TmaxPickItem != null)
						dxChild.TmaxPickItem.DxRecord = dxChild;
						
					bSuccessful = true;
				
				}// if(this.Children.Add(dxChild) != null)
			
			}
			catch
			{
			}
			
			//	Be sure to restore the AutoId mode
			this.Children.AutoIdEnabled = bOldAutoId;
			
			return bSuccessful;
		
		}// public bool Add(CDxPickItem dxChild, bool bAutoId)
		
		/// <summary>This method is called to update a child record</summary>
		/// <param name="dxItem">Record exchange object associated with the record being updated</param>
		/// <returns>true if successful</returns>
		public bool Update(CDxPickItem dxChild)
		{
			bool bSuccessful = false;
			
			try
			{
				//	We have to have child records and application child objects
				if(this.Children == null) return false;
				if(this.TmaxPickItem.Children == null) return false;
				
				//	Make sure the collection is using the correct database
				if(this.Collection != null)
					this.Children.Database = this.Collection.Database;
				if(this.Children.Database == null) return false;
							
				//	Make sure the child collection's parent has been assigned
				this.Children.Parent = this;
					
				//	Update this record
				bSuccessful = this.Children.Update(dxChild);
			
			}
			catch
			{
			}
			
			return bSuccessful;
		
		}// public bool Update(CDxPickItem dxChild)
		
		/// <summary>This method is called to delete a child record</summary>
		/// <param name="dxChild">The record to be deleted</param>
		/// <returns>true if successful</returns>
		public bool Delete(CDxPickItem dxChild)
		{
			bool	bSuccessful = false;
			int		iIndex = -1;
			
			try
			{
				//	We have to have child records and application child objects
				if(this.Children == null) return false;
				if(this.TmaxPickItem.Children == null) return false;
					
				//	Make sure the collection is using the correct database
				if(this.Collection != null)
					this.Children.Database = this.Collection.Database;
				if(this.Children.Database == null) return false;
								
				//	Make sure the child collection's parent has been assigned
				this.Children.Parent = this;
						
				//	Make sure this child exists in the collection
				if((iIndex = this.Children.IndexOf(dxChild)) < 0) return false;
				
				if(this.Children.Delete(dxChild) == true)
				{
					//	Remove the application object
					this.TmaxPickItem.Children.Remove(dxChild.TmaxPickItem);
							
					bSuccessful = true;
				
				}// if(this.Children.Delete(dxChild) == true)
			
			}
			catch
			{
			}

			return bSuccessful;
		
		}// public bool Delete(CDxPickItem dxChild)
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>The database collection that owns this record</summary>
		/// <remarks>This overrides the base class property to cast the collection to the appropriate type</remarks>
		new public CDxPickItems Collection
		{
			get { return ((CDxPickItems)m_dxCollection); }
			set { m_dxCollection = value; }
		}
		
		/// <summary>The exchange interface for the parent record</summary>
		public CDxPickItem Parent
		{
			get { return GetParent(); }
			set { SetParent(value); }
		}
		
		/// <summary>The collection of child records (may or may not exist)</summary>
		public CDxPickItems Children
		{
			get { return GetChildren(); }
			set { SetChildren(value); }
		}
		
		/// <summary>The application PickItem bound to this object</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem TmaxPickItem
		{
			get { return GetTmaxPickItem(); }
			set { SetTmaxPickItem(value); }
		}
		
		/// <summary>The unique identifier assigned to this item</summary>
		public long UniqueId
		{
			get { return this.TmaxPickItem.UniqueId; }
			set { this.TmaxPickItem.UniqueId = value;  }
		}
		
		/// <summary>The unique identifier assigned to this item's parent</summary>
		public long ParentId
		{
			get { return this.TmaxPickItem.ParentId; }
			set { this.TmaxPickItem.ParentId = value;  }
		}
		
		/// <summary>The name assigned to this item's parent</summary>
		public string Name
		{
			get { return this.TmaxPickItem.Name; }
			set { this.TmaxPickItem.Name = value;  }
		}
		
		/// <summary>The enumerated pick item type identifier</summary>
		public FTI.Shared.Trialmax.TmaxPickItemTypes Type
		{
			get { return this.TmaxPickItem.Type; }
			set { this.TmaxPickItem.Type = value;  }
		}
		
		/// <summary>The user defined sort order assigned to the item</summary>
		public int SortOrder
		{
			get { return this.TmaxPickItem.SortOrder; }
			set { this.TmaxPickItem.SortOrder = value;  }
		}
		
		/// <summary>True if children names are case sensitive</summary>
		public bool CaseSensitive
		{
			get { return this.TmaxPickItem.CaseSensitive; }
			set { this.TmaxPickItem.CaseSensitive = value;  }
		}
		
		/// <summary>True if users are allowed to add to the list</summary>
		public bool UserAdditions
		{
			get { return this.TmaxPickItem.UserAdditions; }
			set { this.TmaxPickItem.UserAdditions = value;  }
		}
		
		/// <summary>Packed flags used to store various attributes (property values)</summary>
		public long Attributes
		{
			get { return this.TmaxPickItem.Attributes; }
			set { this.TmaxPickItem.Attributes = value;  }
		}
		
		/// <summary>The identifier of the user that last modified the media entry</summary>
		public long ModifiedBy
		{
			get { return m_lModifiedBy; }
			set { m_lModifiedBy = value; }
		}
		
		/// <summary>The date and time the media was last modified</summary>
		public DateTime ModifiedOn
		{
			get { return m_tsModifiedOn; }
			set { m_tsModifiedOn = value; }
		}
		
		#endregion Properties
	
	}// public class CDxPickItem : FTI.Trialmax.Database.CBaseRecord

	/// <summary>This class is used to manage a ArrayList of CDxPickItem objects</summary>
	public class CDxPickItems : CBaseRecords
	{
		#region Constants
		
		//	Enumerated field (column) identifiers
		public enum eFields
		{
			UniqueId = 0,
			ParentId,
			Name,
			Type,
			Attributes,
			SortOrder,
			SpareNumber,
			SpareText,
			ModifiedBy,
			ModifiedOn,
		}

		public const string TABLE_NAME = "PickLists";
		private const string KEY_FIELD_NAME = "UniqueId";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Parent property</summary>
		private CDxPickItem m_dxParent = null;
		
		/// <summary>Local member bound to AutoIdEnabled property</summary>
		private bool m_bAutoIdEnabled = true;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxPickItems() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxDatabase">Database connection that owns this collection</param>
		public CDxPickItems(CTmaxCaseDatabase tmaxDatabase) : base(tmaxDatabase)
		{
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="dxItem">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxPickItem Add(CDxPickItem dxItem)
		{
			CDxPickItem dxAdded = null;

            dxItem.ModifiedBy = this.Database.GetUserId();
			dxItem.ModifiedOn = System.DateTime.Now;

			if((dxAdded = (CDxPickItem)(base.Add(dxItem))) != null)
			{
				//	Get the unique id assigned by the database
				if(this.AutoIdEnabled == true)
                    dxAdded.UniqueId = this.Database.GetAutoNumber();
			
				if(m_dxParent != null)
					dxAdded.Parent = m_dxParent;
			}
			
			return dxAdded;

		}// public CDxPickItem Add(CDxPickItem dxItem)

		/// <summary>This method allows the caller to update an object's information stored in the database</summary>
		/// <param name="dxRecord">Object to be updated</param>
		/// <returns>true if successful</returns>
		public override bool Update(CBaseRecord dxRecord)
		{
			//	Make sure the user information is updated
            ((CDxPickItem)dxRecord).ModifiedBy = this.Database.GetUserId();
			((CDxPickItem)dxRecord).ModifiedOn = System.DateTime.Now;
				
			return base.Update(dxRecord);
			
		}// public override bool Update(CBaseRecord dxRecord)

		/// <summary>This method will perform cleanup of local resources</summary>
		/// <returns>Always null</returns>
		///	<remarks>The null return allows the caller to dispose and reset the reference in one line of code</remarks>
		public new CDxPickItems Dispose()
		{
			//	Make sure each object in this collection is disposed
			foreach(CDxPickItem O in this)
			{
				try { O.Dispose(); }
				catch {}
			}
			
			return (CDxPickItems)base.Dispose();
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Object at the specified index</returns>
		public new CDxPickItem this[int iIndex]
		{
			get
			{
				return (CDxPickItem)base[iIndex];
			}
		}

		/// <summary>Gets the object located at the specified index</summary>
		/// <returns>Object at the specified index</returns>
		public new CDxPickItem GetAt(int iIndex)
		{
			return (CDxPickItem)base.GetAt(iIndex);
		}

		/// <summary>This method is called to get the SQL statement required to insert the specified record</summary>
		/// <param name="dxRecord">The object to be inserted</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLInsert(CBaseRecord dxRecord)
		{
			CDxPickItem	dxPickItem = (CDxPickItem)dxRecord;
			string		strSQL = "INSERT INTO " + TableName + "(";

			//	Insert the UniqueId if not being assigned by the database
			if(this.AutoIdEnabled == false)
				strSQL += (eFields.UniqueId.ToString() + ",");
			
			strSQL += (eFields.ParentId.ToString() + ",");
			strSQL += (eFields.Name.ToString() + ",");
			strSQL += (eFields.Type.ToString() + ",");
			strSQL += (eFields.Attributes.ToString() + ",");
			strSQL += (eFields.SortOrder.ToString());

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += ("," + eFields.ModifiedBy.ToString());
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += ("," + eFields.ModifiedOn.ToString());

			strSQL += ")";

			strSQL += " VALUES(";

			if(this.AutoIdEnabled == false)
				strSQL += ("'" + dxPickItem.UniqueId.ToString() + "',");
			
			strSQL += ("'" + dxPickItem.ParentId.ToString() + "',");
			strSQL += ("'" + SQLEncode(dxPickItem.Name) + "',");
			strSQL += ("'" + ((int)(dxPickItem.Type)).ToString() + "',");
			strSQL += ("'" + dxPickItem.Attributes.ToString() + "',");
			strSQL += ("'" + dxPickItem.SortOrder.ToString() + "'");

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += (",'" + dxPickItem.ModifiedBy.ToString() + "'");
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += (",'" + dxPickItem.ModifiedOn.ToString() + "'");

			strSQL += ")";

			return strSQL;
		
		}// public override string GetSQLInsert(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to flush all records belonging to the collection</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLFlush()
		{
			string strSQL = "";
			
			if((this.Parent != null) && (this.Parent.UniqueId > 0))
			{
				strSQL = "DELETE FROM ";
				strSQL += TableName;
				strSQL += (" WHERE " + this.KeyFieldName + " = ");
				strSQL += this.Parent.GetKeyId();
				strSQL += ";";
			}
			else
			{
				strSQL = "DELETE * FROM ";
				strSQL += TableName;
				strSQL += ";";
			}
				
			return strSQL;
		
		}// public override string GetSQLFlush()
		
		/// <summary>This method is called to get the SQL statement required to update the specified record</summary>
		/// <param name="dxRecord">The object to be updated</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLUpdate(CBaseRecord dxRecord)
		{
			CDxPickItem	dxPickItem = (CDxPickItem)dxRecord;
			string		strSQL = "UPDATE " + TableName + " SET ";
			
			strSQL += (eFields.ParentId.ToString() + " = '" + dxPickItem.ParentId.ToString() + "',");
			strSQL += (eFields.Name.ToString() + " = '" + SQLEncode(dxPickItem.Name) + "',");
			strSQL += (eFields.Type.ToString() + " = '" + ((int)(dxPickItem.Type)).ToString() + "',");
			strSQL += (eFields.Attributes.ToString() + " = '" + dxPickItem.Attributes.ToString() + "',");
			strSQL += (eFields.SortOrder.ToString() + " = '" + dxPickItem.SortOrder.ToString() + "'");

			if(m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0)
				strSQL += ("," + eFields.ModifiedBy.ToString() + " = '" + dxPickItem.ModifiedBy.ToString() + "'");
			if(m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0)
				strSQL += ("," + eFields.ModifiedOn.ToString() + " = '" + dxPickItem.ModifiedOn.ToString() + "'");

			strSQL += (" WHERE " + this.KeyFieldName + " = ");
			strSQL += dxPickItem.GetKeyId();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLUpdate(CBaseRecord dxRecord)

		/// <summary>This method is called to get the SQL statement required to select the desired records</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLSelect()
		{
			string strSQL = "SELECT * FROM " + m_strTableName;
			
			strSQL += (" WHERE " + eFields.ParentId.ToString() + " = ");
			strSQL += this.Parent != null ? m_dxParent.GetKeyId() : "0";
			strSQL += " ORDER BY ";
			strSQL += eFields.SortOrder.ToString();
			strSQL += ";";
			
			return strSQL;
		
		}// public override string GetSQLSelect()

		/// <summary>This method is called to get the SQL statement required to create the table</summary>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLCreate()
		{
			string strSQL = "CREATE TABLE " + TABLE_NAME;
			
			strSQL += "(";
			
			strSQL += "UniqueId AUTOINCREMENT, ";
			strSQL += "ParentId LONG, ";
			strSQL += "Name TEXT(255), ";
			strSQL += "Type SHORT, ";
			strSQL += "Attributes LONG, ";
			strSQL += "SortOrder LONG, ";
			strSQL += "SpareNumber LONG, ";
			strSQL += "SpareText TEXT(255), ";
			strSQL += "ModifiedBy LONG, ";
			strSQL += "ModifiedOn DATETIME";

			strSQL += ")";
			
			return strSQL;

		}// public override string GetSQLCreate()
		
		/// <summary>This method is called to get the SQL statement required to create the specified column</summary>
		/// <param name="iColumn">Identifies the desired column (derived class specific)</param>
		/// <returns>The appropriate SQL statement</returns>
		public override string GetSQLCreateColumn(int iColumn)
		{
			string strSQL = "";

			try
			{
				if(iColumn == ((int)(eFields.ModifiedBy)))
				{
					strSQL = "ALTER TABLE ";
					strSQL += TABLE_NAME;
			
					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.ModifiedBy.ToString() + " ");
					strSQL += "LONG ";
					strSQL += "DEFAULT 0";
				
					strSQL += " ;";

				}
				else if(iColumn == ((int)(eFields.ModifiedOn)))
				{
					strSQL = "ALTER TABLE ";
					strSQL += TABLE_NAME;
			
					strSQL += "\nADD COLUMN ";
					strSQL += (eFields.ModifiedOn.ToString() + " ");
					strSQL += "DATETIME;";

				}
				
			}
			catch
			{
			}
			
			return strSQL;
		
		}// public virtual string GetSQLCreateColumn(int iColumn)
		
		/// <summary>Called to locate the object with the specified identifier</summary>
		/// <param name="lUniqueId">The id to be located</param>
		/// <returns>The object with the specified identifier</returns>
		public CDxPickItem Find(long lUniqueId)
		{
			CDxPickItem dxPickItem = null;
			
			foreach(CDxPickItem O in this)
			{
				if(O.UniqueId == lUniqueId)
				{
					dxPickItem = O;
					break;
				}
				
			}// foreach(CDxPickItem O in this)
			
			return dxPickItem;
		
		}// public CDxPickItem Find(long lUniqueId)
			
		/// <summary>Called to locate the object with the specified name</summary>
		/// <param name="strName">The name to be located</param>
		/// <returns>The object with the specified name</returns>
		public CDxPickItem Find(string strName)
		{
			CDxPickItem dxPickItem = null;
			
			foreach(CDxPickItem O in this)
			{
				if(strName == O.Name)
				{
					dxPickItem = O;
					break;
				}
				
			}// foreach(CDxPickItem O in this)
			
			return dxPickItem;
		
		}// public CDxPickItem Find(string strName)
			
		/// <summary>Called to add the columns for ModifiedOn and ModifiedBy</summary>
		/// <returns>True if successful</returns>
		public bool AddModifiedColumns()
		{
            if(this.Database != null)
			{
				CreateColumn((int)(eFields.ModifiedBy), true);
				m_dxFields[(int)(eFields.ModifiedBy)].Index = GetColumnIndex(TABLE_NAME, eFields.ModifiedBy.ToString());

				CreateColumn((int)(eFields.ModifiedOn), true);
				m_dxFields[(int)(eFields.ModifiedOn)].Index = GetColumnIndex(TABLE_NAME, eFields.ModifiedOn.ToString());
			}
			else
			{
				m_dxFields[(int)(eFields.ModifiedBy)].Index = -1;
				m_dxFields[(int)(eFields.ModifiedOn)].Index = -1;
			}
			
			return ((m_dxFields[(int)(eFields.ModifiedBy)].Index >= 0) && (m_dxFields[(int)(eFields.ModifiedOn)].Index >= 0));
			
		}// public bool AddModifiedColumns()
			
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to set the database interface</summary>
		protected void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		{
			//	Do the base class processing first
			base.SetDatabase(tmaxDatabase as CBaseDatabase);
			
			//	Make sure the ModifiedOn and ModifiedBy columns exist
			AddModifiedColumns();
			
		}// protected override void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		
		/// <summary>This method MUST be overridden by derived classes to return the collection of field (column) names</summary>
		/// <returns>The collection of field (column) names</returns>
		/// <remarks>The collection should be sorted based on the order of columns in the table</remarks>
		protected override string[] GetFieldNames()
		{
			return Enum.GetNames(typeof(eFields));
		}
		
		/// <summary>This function is called to populate the Fields collection</summary>
		protected override void SetFields()
		{
			//	Do the base class processing first
			base.SetFields();
			
			//	This prevents attempts to retrieve the spare values
			if((m_dxFields != null) && ((int)eFields.SpareNumber < m_dxFields.Count))
				m_dxFields[(int)(eFields.SpareNumber)].Index = -1;
			if((m_dxFields != null) && ((int)eFields.SpareText < m_dxFields.Count))
				m_dxFields[(int)(eFields.SpareText)].Index = -1;
			
			//	Make sure the ModifiedOn and ModifiedBy columns exist
			AddModifiedColumns();
			
		}// protected override void SetFields()
		
		/// <summary>This function is called to get a new record object</summary>
		/// <returns>A new object of the collection type</returns>
		protected override CBaseRecord GetNewRecord()
		{
			return ((CBaseRecord)(new CDxPickItem()));
		}
		
		/// <summary>This method is called to exchange data between the field objects and their associated record properties</summary>
		/// <param name="dxRecord">The record exchange object</param>
		/// <param name="bSetFields">true to set the field values, false to set the record values</param>
		/// <remarks>Derived classes must override this function</remarks>
		/// <returns>true if successful</returns>
		protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		{
			CDxPickItem	dxPickItem = (CDxPickItem)dxRecord;
			
			if((m_dxFields == null) || (m_dxFields.Count == 0)) return false;
			
			try
			{
				//	Are we setting the field values?
				if(bSetFields)
				{
					m_dxFields[(int)eFields.UniqueId].Value  = dxPickItem.UniqueId;
					m_dxFields[(int)eFields.ParentId].Value = dxPickItem.ParentId;
					m_dxFields[(int)eFields.Name].Value = dxPickItem.Name;
					m_dxFields[(int)eFields.Type].Value = dxPickItem.Type;
					m_dxFields[(int)eFields.Attributes].Value = dxPickItem.Attributes;
					m_dxFields[(int)eFields.SortOrder].Value = dxPickItem.SortOrder;
				}
				else
				{
					dxPickItem.UniqueId = (int)(m_dxFields[(int)eFields.UniqueId].Value);
					dxPickItem.ParentId = (int)(m_dxFields[(int)eFields.ParentId].Value);
					dxPickItem.Name = (string)(m_dxFields[(int)eFields.Name].Value);
					dxPickItem.Type = (FTI.Shared.Trialmax.TmaxPickItemTypes)((short)(m_dxFields[(int)eFields.Type].Value));
					dxPickItem.Attributes = (int)(m_dxFields[(int)eFields.Attributes].Value);
					dxPickItem.SortOrder = (int)(m_dxFields[(int)eFields.SortOrder].Value);
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
			
		}// protected override bool Exchange(CBaseRecord dxRecord, bool bSetFields)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This function is called to set the table name and key field name</summary>
		protected override void SetNames()
		{
			m_strTableName = TABLE_NAME;
			m_strKeyFieldName = KEY_FIELD_NAME;
		}
		
		/// <summary>This method will set the default value for the specified field</summary>
		/// <param name="eField">The enumerated field identifier</param>
		/// <param name="dxField">The field object to be set</param>
		private void SetValue(eFields eField, CDxField dxField)
		{
			switch(eField)
			{
				case eFields.UniqueId:
				case eFields.ParentId:
				case eFields.SortOrder:
				case eFields.Attributes:
				case eFields.SpareNumber:
				case eFields.ModifiedBy:
				
					dxField.Value = 0;
					break;
					
				case eFields.Type:
				
					dxField.Value = TmaxPickItemTypes.Unknown;
					break;
					
				case eFields.Name:
				case eFields.SpareText:
				
					dxField.Value = "";
					break;
					
				case eFields.ModifiedOn:
				
					dxField.Value = System.DateTime.Now;
					break;
					
				default:
				
					Debug.Assert(false, "SetValue() - unknown field identifier - " + eField.ToString());
					break;
					
			}// switch(eField)
		
		}// private void SetValue(eFields eField, CDxField dxField)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The record exchange object associated with the Parent binder</summary>
		public CDxPickItem Parent
		{
			get { return m_dxParent; }
			set { m_dxParent = value; }
		}
		
		/// <summary>True to automatically assign the UniqueId value for each record</summary>
		public bool AutoIdEnabled
		{
			get { return m_bAutoIdEnabled; }
			set { m_bAutoIdEnabled = value; }
		}

        /// <summary>The active database</summary>
        new public CTmaxCaseDatabase Database
        {
            get { return (CTmaxCaseDatabase)(base.Database); }
            set { SetDatabase(value); }
        }

        #endregion Properties
		
	}//	public class CDxPickItems : CBaseRecords
		
}// namespace FTI.Trialmax.Database
