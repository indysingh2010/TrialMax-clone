using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with a system message</summary>
	public class CTmaxPickItem : ITmaxSortable, ITmaxListViewCtrl
	{
		#region Private Members
		
		/// <summary>Private member bound to UniqueId property</summary>
		private long m_lUniqueId = 0;
		
		/// <summary>Private member bound to ParentId property</summary>
		private long m_lParentId = 0;
		
		/// <summary>Private member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Private member bound to Attributes property</summary>
		private long m_lAttributes = 0;
		
		/// <summary>Private member bound to Type property</summary>
		private TmaxPickItemTypes m_eType = TmaxPickItemTypes.Unknown;
		
		/// <summary>Private member bound to SortOrder property</summary>
		private int m_iSortOrder = 0;
		
		/// <summary>Private member bound to Parent property</summary>
		CTmaxPickItem m_tmaxParent = null;
		
		/// <summary>Private member bound to Children property</summary>
		private CTmaxPickItems m_tmaxChildren = null;
		
		/// <summary>Private member bound to DxRecord property</summary>
		private object m_dxRecord = null;
		
		/// <summary>This member is bounded to the Modified property</summary>
		private bool m_bModified = false;		
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxPickItem()
		{
			//	Set default attributes
			this.CaseSensitive = false;
			this.UserAdditions = true;
		}
	
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">Source object to be copied</param>
		public CTmaxPickItem(CTmaxPickItem tmaxSource) : base()
		{
			if(tmaxSource != null)
			{
				Copy(tmaxSource);
			}
			else
			{
				this.CaseSensitive = false;
				this.UserAdditions = true;
			}
			
		}// public CTmaxPickItem(CTmaxPickItem tmaxSource) : base()
		
		/// <summary>Called to clear this object's child collection</summary>
		public void Clear()
		{
			try
			{
				//	Flush the children if they exist
				if(this.Children != null)
					this.Children.Clear();
			}
			catch
			{
			}
		
		}// public void Clear()
		
		/// <summary>Called to copy the properties of the specified source object</summary>
		/// <param name="tmaxSource">The object to be copied</param>
		/// <param name="bChildren">true to copy the child collection</param>
		public void Copy(CTmaxPickItem tmaxSource, bool bChildren)
		{
			//	Clear the existing child collection
			if(this.Children != null)
				this.Children.Clear();
				
			this.UniqueId	= tmaxSource.UniqueId;
			this.ParentId	= tmaxSource.ParentId;
			this.Type		= tmaxSource.Type;
			this.Name		= tmaxSource.Name;
			this.SortOrder	= tmaxSource.SortOrder;
			this.DxRecord	= tmaxSource.DxRecord;
			this.Modified	= tmaxSource.Modified;
			this.Parent		= tmaxSource.Parent;
			this.Attributes = tmaxSource.Attributes;
			
			if((bChildren == true) && (this.Children != null))
			{
				this.Children.Clear();
				
				if(tmaxSource.Children != null)
				{
					this.Children.Copy(tmaxSource.Children);
				}
				
			}// if((bChildren == true) && (this.Children != null))
			
		}// public void Copy(CTmaxPickItem tmaxSource, bool bChildren)
		
		/// <summary>Called to copy the properties of the specified source object</summary>
		/// <param name="tmaxSource">The object to be copied</param>
		public void Copy(CTmaxPickItem tmaxSource)
		{
			//	Copy the children by default
			Copy(tmaxSource, true);
		}
		
		/// <summary>Called to get access to the object's child collection</summary>
		///	<returns>Retrieves the child collection if it exists</returns>
		public CTmaxPickItems GetChildren()
		{
			//	Is this a pick list?
			if(m_eType != TmaxPickItemTypes.Value)
			{
				//	Make sure we have a valid collection
				if(m_tmaxChildren == null)
				{
					m_tmaxChildren = new CTmaxPickItems();
					m_tmaxChildren.Parent = this;
					
					if(this.Collection != null)
						this.Collection.EventSource.Attach(m_tmaxChildren.EventSource);
				}
			
			}
			else
			{
				//	Make sure the collection has been reset
				if(m_tmaxChildren != null)
				{
					m_tmaxChildren.Clear();
					m_tmaxChildren = null;
				}
			
			}// if(this.IsList == true)
			
			return m_tmaxChildren;
			
		}// public CTmaxPickItems GetChildren()
		
		/// <summary>Called to set the parent object</summary>
		/// <param name="tmaxParent">The object to be assigned as the parent</param>
		public void SetParent(CTmaxPickItem tmaxParent)
		{
			m_tmaxParent = tmaxParent;
			m_lParentId = (tmaxParent != null) ? tmaxParent.UniqueId : 0;
		}
		
		/// <summary>Called to get the unique id of the parent object</summary>
		/// <returns>The id of the parent object</returns>
		public long GetParentId()
		{
			if(m_tmaxParent != null)
			{
				//	Just in case
				if(m_lParentId != m_tmaxParent.UniqueId)
					m_lParentId = m_tmaxParent.UniqueId;
			}
			
			return m_lParentId;

		}// public long GetParentId()
		
		/// <summary>Called to add an item to the child collection</summary>
		/// <param name="tmaxChild">The object to add to the child collection</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxPickItem tmaxChild)
		{
			bool bSuccessful = false;
			
			if((tmaxChild != null) && (this.GetChildren() != null))
			{
				this.GetChildren().Add(tmaxChild);
				tmaxChild.Parent = this;
				bSuccessful = true;
			}
			
			return bSuccessful;

		}// public bool Add(CTmaxPickItem tmaxChild)
		
		/// <summary>Called to get the path to this item</summary>
		/// <param name="bIncludeName">True to include the name of this item</param>
		public string GetPath(bool bIncludeName)
		{
			string			strPath = "";
			CTmaxPickItem	tmaxParent = null;
			
			//	Should we include the name of this item?
			if(bIncludeName == true)
				strPath = this.Name;
				
			tmaxParent = this.Parent;
			
			//	Stop when we run out of parents or hit the root
			while((tmaxParent != null) && (tmaxParent.UniqueId > 0))
			{
				if(strPath.Length > 0)
					strPath = ("\\" + strPath);
				strPath = (tmaxParent.Name + strPath);
				
				tmaxParent = tmaxParent.Parent;
				
			}// while((tmaxParent != null) && (tmaxParent.UniqueId > 0))
			
			return strPath;
				
		}// public string GetPath(bool bIncludeName)
		
		/// <summary>Called to find the pick list with the specified id</summary>
		/// <param name="lUniqueId">The id of the value list</param>
		/// <returns>The value list with the specified id</returns>
		public CTmaxPickItem FindList(long lUniqueId)
		{
			CTmaxPickItem tmaxValueList = null;
			
			//	Is this the item the caller is looking for?
			if(this.UniqueId == lUniqueId) return this;
			
			//	Could one of this object's children be the value list?
			if((this.Type == TmaxPickItemTypes.MultiLevel) && (this.Children != null))
			{
				foreach(CTmaxPickItem O in this.Children)
				{
					if((tmaxValueList = O.FindList(lUniqueId)) != null)
						return tmaxValueList;
				}
				
			}

			return null; //	Not found...
				
		}// public CTmaxPickItem FindList(long lUniqueId)
		
		/// <summary>Called to find the pick list item with the specified id</summary>
		/// <param name="lUniqueId">The id of the value item</param>
		/// <returns>The value item with the specified id</returns>
		public CTmaxPickItem FindValue(long lUniqueId)
		{
			CTmaxPickItem tmaxValueItem = null;
			
			//	Is this the item the caller is looking for?
			if(this.UniqueId == lUniqueId) return this;
			
			//	Check the child collection
			if(this.Children != null)
			{
				foreach(CTmaxPickItem O in this.Children)
				{
					if((tmaxValueItem = O.FindValue(lUniqueId)) != null)
						return tmaxValueItem;
					
				}// foreach(CTmaxPickItem O in this.Children)
				
			}// if(this.Children != null)

			return null; //	Not found...
				
		}// public CTmaxPickItem FindValue(long lUniqueId)
		
		/// <summary>Called to find the pick list item with the specified id</summary>
		/// <param name="strName">The name of the value item</param>
		/// <returns>The value item with the specified name</returns>
		public CTmaxPickItem FindValue(string strName)
		{
			CTmaxPickItem	tmaxValueItem = null;
			bool			bIgnoreCase = true;
			
			//	Is this a value type item?
			if(this.Type == TmaxPickItemTypes.Value)
			{
				if(this.Parent != null)
					bIgnoreCase = !this.Parent.CaseSensitive;
					
				if(String.Compare(this.Name, strName, bIgnoreCase) == 0)
					return this;
			}
			else
			{
				//	Check the child collection
				if(this.Children != null)
				{
					foreach(CTmaxPickItem O in this.Children)
					{
						if((tmaxValueItem = O.FindValue(strName)) != null)
							return tmaxValueItem;
					
					}// foreach(CTmaxPickItem O in this.Children)
				
				}// if(this.Children != null)

			}
			
			return null; //	Not found...
				
		}// public CTmaxPickItem FindValue(string strName)
		
		/// <summary>Called to find the child item with the specified Id</summary>
		/// <param name="lUniqueId">The id of the desired child</param>
		/// <returns>The child item with the specified id</returns>
		public CTmaxPickItem FindChild(long lUniqueId)
		{
			if(this.Children != null)
			{
				foreach(CTmaxPickItem O in this.Children)
				{
					if(O.UniqueId == lUniqueId)
						return O;
				}
				
			}
			
			return null; //	Not found...
				
		}// public CTmaxPickItem FindChild(long lUniqueId)
		
		/// <summary>Called to find the child item with the specified name</summary>
		/// <param name="strName">The name of the desired child</param>
		/// <param name="bCaseSensitive">True if the search is case sensitive</param>
		/// <returns>The child item with the specified name</returns>
		public CTmaxPickItem FindChild(string strName, bool bCaseSensitive)
		{
			if(this.Children != null)
			{
				foreach(CTmaxPickItem O in this.Children)
				{
					if(String.Compare(O.Name, strName, !bCaseSensitive) == 0)
						return O;
				}
				
			}
			
			return null; //	Not found...
				
		}// public CTmaxPickItem FindChild(string strName, bool bCaseSensitive)
		
		/// <summary>Called to find the child item with the specified name</summary>
		/// <param name="strName">The name of the desired child</param>
		/// <returns>The child item with the specified name</returns>
		public CTmaxPickItem FindChild(string strName)
		{
			//	Use this item's CaseSensitive property
			return FindChild(strName, this.CaseSensitive);
		}
		
		/// <summary>Called to get the pick list values associated with this object</summary>
		/// <param name="tmaxValueItems">The array in which to store the value items</param>
		/// <param name="bRecurse">True to recurse child lists</param>
		/// <returns>The number of items added to the collection</returns>
		public int GetValueItems(CTmaxPickItems tmaxValueItems, bool bRecurse)
		{
			int iExisting = 0;
			
			Debug.Assert(tmaxValueItems != null);
			if(tmaxValueItems == null) return 0;
			
			//	How may items are in the collection
			iExisting = tmaxValueItems.Count;
			
			//	Is this item a value item?
			if(this.Type == TmaxPickItemTypes.Value)
			{
				tmaxValueItems.Add(this);
			}
			else if(this.Children != null)
			{
				foreach(CTmaxPickItem O in this.Children)
				{
					if(O.Type == TmaxPickItemTypes.Value)
					{
						tmaxValueItems.Add(O);
					}
					else if(bRecurse == true)
					{
						GetValueItems(tmaxValueItems, true);
					}
				
				}// foreach(CTmaxPickItem O in this.Children)

			}// if(this.Type == TmaxPickItemTypes.Value)
			
			return (tmaxValueItems.Count - iExisting);
				
		}// public int GetValueItems(CTmaxPickItems tmaxValueItems, bool bRecurse)
		
		/// <summary>Called to get the pick list values associated with this object</summary>
		/// <param name="bRecurse">True to recurse child lists</param>
		/// <returns>The collection of pick item values</returns>
		public CTmaxPickItems GetValueItems(bool bRecurse)
		{
			CTmaxPickItems tmaxValueItems = null;
			
			try
			{
				tmaxValueItems = new CTmaxPickItems();
				GetValueItems(tmaxValueItems, bRecurse);
			}
			catch
			{
			}
			
			if((tmaxValueItems != null) && (tmaxValueItems.Count > 0))
				return tmaxValueItems;
			else
				return null;
				
		}// public CTmaxPickItems GetValueItems(bool bRecurse)
		
		/// <summary>Called to get the pick list values associated with this object</summary>
		/// <returns>The collection of pick item values</returns>
		public CTmaxPickItems GetValueItems()
		{
			//	Recurse the children by default
			return GetValueItems(true);
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxPickItem">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxPickItem, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxPickItem tmaxPickItem, long lMode)
		{
			//	Are these the same objects?
			if(ReferenceEquals(this, tmaxPickItem) == true)
			{
				return 0;
			}
			else
			{
				//	Are we sorting on order?
				if(lMode == CTmaxPickItems.SORT_ON_ORDER)
				{
					if(this.SortOrder == tmaxPickItem.SortOrder)
						return 0;
					else if(this.SortOrder > tmaxPickItem.SortOrder)
						return 1;
					else
						return -1;
				}
				else
				{						
					return CTmaxToolbox.Compare(this.Name, tmaxPickItem.Name, !this.CaseSensitive);
				}
			
			}// if(ReferenceEquals(this, tmaxPickItem) == true)
					
		}// public int Compare(CTmaxPickItem tmaxPickItem, long lMode)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxPickItem, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxPickItem)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method builds the text representation of the message</summary>
		/// <returns>The default text representation</returns>
		public override string ToString()
		{
			if(m_strName.Length > 0)
				return m_strName;
			else
				return base.ToString();
		
		}// public override string ToString()

		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			string[] aNames = { "Name" };
			return aNames;
		
		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[1];
			aValues[0] = this.Name;
			
			return aValues;
			
		}// string[] ITmaxListViewCtrl.GetValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			//	The values returned here assume an image list has been attached
			//	to the control with images in the order defined here
			switch(this.Type)
			{
				case TmaxPickItemTypes.MultiLevel:	return 1;
				case TmaxPickItemTypes.StringList:	return 2;
				case TmaxPickItemTypes.Value:		return 3;
				case TmaxPickItemTypes.Unknown:		
				default:							return 0;
					
			}// switch(this.Type)

		}// int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The unique identifier assigned to this pick item</summary>
		public long UniqueId
		{
			get { return m_lUniqueId; }
			set { m_lUniqueId = value; }
		}
		
		/// <summary>The parent item that owns this item</summary>
		public CTmaxPickItem Parent
		{
			get { return m_tmaxParent; }
			set { SetParent(value); }
		}
		
		/// <summary>The unique identifier assigned to this pick item's parent</summary>
		public long ParentId
		{
			get { return GetParentId(); }
			set { m_lParentId = value; }
		}
		
		/// <summary>The path from the root list to this item</summary>
		public string Path
		{
			get { return GetPath(false); }
		}
		
		/// <summary>Enumerated item type identifier</summary>
		public TmaxPickItemTypes Type
		{
			get { return m_eType; }
			set { m_eType = value; }
		}
		
		/// <summary>User defined sort order identifier</summary>
		public int SortOrder
		{
			get { return m_iSortOrder; }
			set { m_iSortOrder = value; }
		}
		
		/// <summary>The packed flags that define the attributes</summary>
		public long Attributes
		{
			get { return m_lAttributes; }
			set { m_lAttributes = value; }
		}
		
		/// <summary>The name associated with the pick list or value</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>The collection of child items</summary>
		public FTI.Shared.Trialmax.CTmaxPickItems Children
		{
			get { return GetChildren(); }
		}
		
		/// <summary>This is a reference to the data exchange object associated with this object</summary>
		public object DxRecord
		{
			get { return m_dxRecord; }
			set { m_dxRecord = value; }
		}
		
		//	True if object has been modified by the user
		public bool Modified
		{
			get{ return m_bModified; }
			set{ m_bModified = value; }
		}
		
		//	The collection that owns this object
		public CTmaxPickItems Collection
		{
			get { return this.Parent != null ? this.Parent.Children : null; }
		}
		
		/// <summary>Flag to indicate that the names of children are case sensitive</summary>
		public bool CaseSensitive
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxPickItemAttributes.CaseSensitive) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxPickItemAttributes.CaseSensitive;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxPickItemAttributes.CaseSensitive);
				}
			
			}
		
		}// public bool CaseSensitive
		
		/// <summary>Flag to indicate if users are allowed to add to the list when defined fielded data</summary>
		public bool UserAdditions
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxPickItemAttributes.UserAdditions) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxPickItemAttributes.UserAdditions;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxPickItemAttributes.UserAdditions);
				}
			
			}
		
		}// public bool UserAdditions
		
		#endregion Properties
		
	}// public class CTmaxPickItem : ITmaxSortable

	/// <summary>This class manages a list of system messages</summary>
	public class CTmaxPickItems : CTmaxSortedArrayList
	{
		#region Constants
		
		public const int SORT_ON_NAME	= 0;
		public const int SORT_ON_ORDER	= 1;
		
		private const string XMLINI_PICK_ITEM_SECTION_NAME				= "pickLists";
		private const string XMLINI_PICK_ITEM_ATTRIBUTE_ID				= "id";
		private const string XMLINI_PICK_ITEM_ATTRIBUTE_NAME			= "name";
		private const string XMLINI_PICK_ITEM_ATTRIBUTE_TYPE			= "type";
		private const string XMLINI_PICK_ITEM_ATTRIBUTE_PARENT_ID		= "parentId";
		private const string XMLINI_PICK_ITEM_ATTRIBUTE_CASE_SENSITIVE	= "caseSensitive";
		private const string XMLINI_PICK_ITEM_ATTRIBUTE_USER_ADDITIONS	= "userAdditions";
		private const string XMLINI_PICK_ITEM_ATTRIBUTE_SORT_ORDER		= "sortOrder";

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Private member bound to Parent property</summary>
		CTmaxPickItem m_tmaxParent = null;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxPickItems() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			((CTmaxSorter)Comparer).Mode = SORT_ON_NAME;
			
			this.KeepSorted = false;
			
			//	Initialize the event source
			m_tmaxEventSource.Name = "Pick Items";
			
		}// public CTmaxPickItems() : base()

		/// <summary>Called to empty the collection</summary>
		public override void Clear()
		{
			//	Clear each of the child items by default
			Clear(true);
		}
		
		/// <summary>Called to empty the collection</summary>
		/// <param name="bChildren">true to clear the child items also</param>
		public void Clear(bool bChildren)
		{
			try
			{
				//	Clear each child object before flushing the collection
				if(bChildren == true)
				{
					foreach(CTmaxPickItem O in this)
						O.Clear();
				}
					
				//	Empty the collection
				base.Clear();
			}
			catch
			{
			}
		
		}// public void Clear()
		
		/// <summary>Called to copy the properties of the specified source object</summary>
		/// <param name="tmaxSource">The object to be copied</param>
		public void Copy(CTmaxPickItems tmaxSource)
		{
			if(tmaxSource.Comparer != null)
			{
				this.Comparer = new CTmaxSorter();
				((CTmaxSorter)(this.Comparer)).Mode = ((CTmaxSorter)(tmaxSource.Comparer)).Mode;
			}
			else
			{
				this.Comparer = null;
			}
			
			this.Parent = tmaxSource.Parent;
			this.KeepSorted = tmaxSource.KeepSorted;
			
			foreach(CTmaxPickItem O in tmaxSource)
				Add(new CTmaxPickItem(O));
			
		}// public void Copy(CTmaxPickItems tmaxSource)
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxPickItem">CTmaxPickItem object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxPickItem Add(CTmaxPickItem tmaxPickItem)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxPickItem as object);
					
				return tmaxPickItem;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}
			
		}// public CTmaxPickItem Add(CTmaxPickItem tmaxPickItem)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxPickItem">The filter object to be removed</param>
		public void Remove(CTmaxPickItem tmaxPickItem)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxPickItem as object);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Remove", Ex);
			}
		
		}// public void Remove(CTmaxPickItem tmaxPickItem)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxPickItem">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxPickItem tmaxPickItem)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxPickItem as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxPickItem this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxPickItem);
			}
		}

		/// <summary>This method is called to locate the object with the specified unique id</summary>
		/// <param name="strName">The id to search for</param>
		/// <returns>The object with the specified Id</returns>
		public CTmaxPickItem Find(long lUniqueId)
		{
			foreach(CTmaxPickItem O in this)
			{
				if(O.UniqueId == lUniqueId)
					return O;
			}
			
			return null; // Not found...
			
		}// public CTmaxPickItem Find(long lUniqueId)

		/// <summary>This method is called to locate the object with the specified Name</summary>
		/// <param name="strName">The name to search for</param>
		/// <param name="bIgnoreCase">True to ignore case</param>
		/// <returns>The object with the specified Name</returns>
		public CTmaxPickItem Find(string strName, bool bIgnoreCase)
		{
			foreach(CTmaxPickItem O in this)
			{
				if(String.Compare(strName, O.Name, bIgnoreCase) == 0)
					return O;
			}
			
			return null; // Not found...
			
		}// public CTmaxPickItem Find(string strName, bool bIgnoreCase)

		/// <summary>This method is called to locate the object with the specified Name</summary>
		/// <param name="strName">The name to search for</param>
		/// <returns>The object with the specified Name</returns>
		public CTmaxPickItem Find(string strName)
		{
			return Find(strName, true);
		}

		/// <summary>This method is called to get the next sort order identifier</summary>
		/// <returns>The next sort order value to assign for new items</returns>
		public int GetNextSortOrder()
		{
			int iMaxOrder = 0;
			
			//	We can't assume the collection is sorted on the SortOrder field
			foreach(CTmaxPickItem O in this)
			{
				if(O.SortOrder > iMaxOrder)
					iMaxOrder = O.SortOrder;
			}
			
			return (iMaxOrder + 1);
		
		}// public int GetNextSortOrder()

		/// <summary>This method is called to determine if one or more objects has been modified</summary>
		/// <returns>True if one or more objects is modified</returns>
		public bool GetModified()
		{
			foreach(CTmaxPickItem O in this)
			{
				if(O.Modified == true)
					return true;
			}
			
			//	None have been modified
			return false;
			
		}// public bool GetModified()

		/// <summary>This method is called to clear the modified status of the objects in the collection</summary>
		public void ClearModified()
		{
			foreach(CTmaxPickItem O in this)
				O.Modified = false;
		}

		/// <summary>This method converts the numeric line identifier to a unique key for storing the object</summary>
		/// <param name="iLine">numeric line identifier</param>
		/// <returns>The appropriate XML Ini key</returns>
		string GetIniKey(int iLine)
		{
			return String.Format("PL{0}", iLine);
		}
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strSection">The name of the section to use to store the values</param>
		/// <returns>True if successful</returns>
		public bool Save(CXmlIni xmlIni, string strSection)
		{
			bool	bSuccessful = true;
			int		iLine = 1;
			
			//	Line up on the requested section
			if(xmlIni.SetSection(strSection, true, true) == false)
				return false; 
			
			//	Write the objects to file
			foreach(CTmaxPickItem O in this)
			{
				if(Save(xmlIni, GetIniKey(iLine++), O) == false)
					bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool Save(CXmlIni xmlIni, string strSection)
		
		/// <summary>This method is called to store the options in default section of the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the information should be stored</param>
		/// <returns>True if successful</returns>
		public bool Save(CXmlIni xmlIni)
		{
			return Save(xmlIni, XMLINI_PICK_ITEM_SECTION_NAME);	
		}
		
		/// <summary>This method is called to load the application pick list items from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the items</param>
		/// <param name="strSection">The name of the section where the items are stored</param>
		public bool Load(CXmlIni xmlIni, string strSection)
		{
			int				iLine = 1;
			CTmaxPickItem	tmaxPickItem = null;
			bool			bContinue = true;
			
			if(xmlIni.SetSection(strSection) == false)
				return false;
			
			//	The parent should be assigned
			Debug.Assert(this.Parent != null);
			if(this.Parent == null) return false;
			
			//	Load all the codes
			this.Clear();
			while(bContinue == true)
			{
				tmaxPickItem = new CTmaxPickItem();
				
				if((bContinue = Load(xmlIni, GetIniKey(iLine++), tmaxPickItem)) == true)
					this.Parent.Add(tmaxPickItem); // Use the parent item to add the child
			}
			
			return true;

		}// public void Load(CXmlIni xmlIni, string strSection)
		
		/// <summary>This method is called to load the pick list items in the default section of the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the values should be stored</param>
		/// <returns>True if successful</returns>
		public bool Load(CXmlIni xmlIni)
		{
			return Load(xmlIni, XMLINI_PICK_ITEM_SECTION_NAME);	
		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to store the specified pick list item in the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxPickItem">The item to be stored in the file</param>
		public bool Save(CXmlIni xmlIni, string strKey, CTmaxPickItem tmaxPickItem)
		{
			string	strPath = "";
			string	strSection = "";
			try
			{
				//	Write the common attributes
				xmlIni.Write(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_NAME, tmaxPickItem.Name);
				xmlIni.Write(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_ID, tmaxPickItem.UniqueId);
				xmlIni.Write(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_TYPE, (int)(tmaxPickItem.Type));
				xmlIni.Write(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_PARENT_ID, tmaxPickItem.ParentId);
				xmlIni.Write(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_SORT_ORDER, tmaxPickItem.SortOrder);

				//	Is this a list or multilevel?
				if(tmaxPickItem.Type != TmaxPickItemTypes.Value)
				{
					//	Write the list specific attributes
					xmlIni.Write(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_CASE_SENSITIVE, tmaxPickItem.CaseSensitive);
					xmlIni.Write(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_USER_ADDITIONS, tmaxPickItem.UserAdditions);
				
					//	Does this list have any children?
					if((tmaxPickItem.Children != null) && (tmaxPickItem.Children.Count > 0))
					{
						//	Get the unique path for this list
						strPath = tmaxPickItem.GetPath(true);
						if((strPath != null) && (strPath.Length > 0))
						{
							//	Save the current section
							strSection = xmlIni.Section;
							
							//	Save the children to file
							tmaxPickItem.Children.Save(xmlIni, strPath);
							
							//	Restore the section
							xmlIni.SetSection(strSection, false, false);
							
						}// if((strPath != null) && (strPath.Length > 0))
					
					}// if((tmaxPickItem.Children != null) && (tmaxPickItem.Children.Count > 0))
					
				}// if(tmaxPickItem.Type != TmaxPickItemTypes.Value)
				
				return true;
				
			}
			catch
			{
				return false;
			}
			
		}// public bool Save(CXmlIni xmlIni, string strKey, CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method is called to load the properties for the specified pick list item from the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the values should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxPickItem">The item to be initialized</param>
		///	<returns>true to continue loading</returns>
		private bool Load(CXmlIni xmlIni, string strKey, CTmaxPickItem tmaxPickItem)
		{
			string	strPath = "";
			string	strSection = "";
			
			try
			{
				//	Read the common attributes
				tmaxPickItem.Name = xmlIni.Read(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_NAME, "");
				tmaxPickItem.UniqueId = xmlIni.ReadLong(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_ID, 0);
				
				//	Have we run out of items?
				if((tmaxPickItem.UniqueId == 0) && (tmaxPickItem.Name.Length == 0)) return false;
				
				try { tmaxPickItem.Type = (TmaxPickItemTypes)(xmlIni.ReadInteger(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_TYPE, 0)); }
				catch {}
				
				tmaxPickItem.ParentId = xmlIni.ReadLong(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_PARENT_ID, 0);
				tmaxPickItem.SortOrder = xmlIni.ReadInteger(strKey, XMLINI_PICK_ITEM_ATTRIBUTE_SORT_ORDER, 0);
				
				//	Is this a list or multilevel?
				if(tmaxPickItem.Type != TmaxPickItemTypes.Value)
				{
					//	Get the list specific attributes
					tmaxPickItem.CaseSensitive = xmlIni.ReadBool(XMLINI_PICK_ITEM_ATTRIBUTE_CASE_SENSITIVE, tmaxPickItem.CaseSensitive);
					tmaxPickItem.UserAdditions = xmlIni.ReadBool(XMLINI_PICK_ITEM_ATTRIBUTE_USER_ADDITIONS, tmaxPickItem.UserAdditions);
					
					//	Does this item have a child collection
					if(tmaxPickItem.Children != null)
					{
						//	Get the unique path for this list
						tmaxPickItem.Parent = this.Parent;
						strPath = tmaxPickItem.GetPath(true);
						if((strPath != null) && (strPath.Length > 0))
						{
							//	Save the current section
							strSection = xmlIni.Section;
							
							//	Save the children to file
							tmaxPickItem.Children.Load(xmlIni, strPath);
							
							//	Restore the section
							xmlIni.SetSection(strSection, false, false);
							
						}// if((strPath != null) && (strPath.Length > 0))
					
					}// if((tmaxPickItem.Children != null) && (tmaxPickItem.Children.Count > 0))
					
				}// if(tmaxPickItem.Type != TmaxPickItemTypes.Value)
				
				return true;
			}
			catch
			{
				//	NOTE: We don't return FALSE because the rest of the codes might be OK
			}
			
			return true;
			
		}// private bool Load(CXmlIni xmlIni, string strKey, CTmaxPickItem tmaxPickItem)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The parent item that owns this item</summary>
		public CTmaxPickItem Parent
		{
			get { return m_tmaxParent; }
			set { m_tmaxParent = value; }
		}
		
		//	Controls the field used to sort objects in the collection
		public int SortOn
		{
			get { return (this.Comparer != null ? (int)(((CTmaxSorter)(this.Comparer)).Mode) : CTmaxPickItems.SORT_ON_NAME); }
			set { if(this.Comparer != null) ((CTmaxSorter)(this.Comparer)).Mode = value; }
		}
		
		//	True if one or more objects have been modified
		public bool Modified
		{
			get{ return GetModified(); }
		}
		
		#endregion Properties
		
	}//	public class CTmaxPickItems : CTmaxSortedArrayList
		
}// namespace FTI.Shared.Trialmax
