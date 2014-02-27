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
	public class CTmaxBinderItem : ITmaxSortable
	{
		#region Constants
		
		private const string XML_ATTRIBUTE_BARCODE			= "barcode";
		private const string XML_ATTRIBUTE_NAME				= "name";
		private const string XML_ATTRIBUTE_DESCRIPTION		= "description";
		private const string XML_ATTRIBUTE_ATTRIBUTES		= "attributes";
		private const string XML_ATTRIBUTE_DISPLAY_ORDER	= "displayOrder";

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Private member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Private member bound to Barcode property</summary>
		private string m_strBarcode = "";
		
		/// <summary>Private member bound to Attributes property</summary>
		private long m_lAttributes = 0;
		
		/// <summary>Private member bound to Description property</summary>
		private string m_strDescription = "";
		
		/// <summary>Private member bound to DisplayOrder property</summary>
		private int m_iDisplayOrder = 0;
		
		/// <summary>Private member bound to IBinder property</summary>
		private FTI.Shared.Trialmax.ITmaxMediaRecord m_IBinder = null;
		
		/// <summary>Private member bound to Children property</summary>
		private CTmaxBinderItems m_tmaxChildren = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxBinderItem()
		{
			//	Set default attributes
			this.IsMedia = false;
		}
	
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">Source object to be copied</param>
		public CTmaxBinderItem(CTmaxBinderItem tmaxSource) : base()
		{
			if(tmaxSource != null)
			{
				Copy(tmaxSource);
			}
			else
			{
				this.IsMedia = false;
			}
			
		}// public CTmaxBinderItem(CTmaxBinderItem tmaxSource) : base()
		
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
		public void Copy(CTmaxBinderItem tmaxSource, bool bChildren)
		{
			//	Clear the existing child collection
			Clear();
				
			this.Barcode		= tmaxSource.Barcode;
			this.Name			= tmaxSource.Name;
			this.Description	= tmaxSource.Description;
			this.DisplayOrder	= tmaxSource.DisplayOrder;
			this.Attributes		= tmaxSource.Attributes;
			
			if((bChildren == true) && (this.Children != null))
			{
				if(tmaxSource.Children != null)
				{
					this.Children.Copy(tmaxSource.Children);
				}
				
			}// if((bChildren == true) && (this.Children != null))
			
		}// public void Copy(CTmaxBinderItem tmaxSource, bool bChildren)
		
		/// <summary>Called to copy the properties of the specified source object</summary>
		/// <param name="tmaxSource">The object to be copied</param>
		public void Copy(CTmaxBinderItem tmaxSource)
		{
			//	Copy the children by default
			Copy(tmaxSource, true);
		}
		
		/// <summary>Called to get access to the object's child collection</summary>
		///	<returns>Retrieves the child collection if it exists</returns>
		public CTmaxBinderItems GetChildren()
		{
			//	Media items do not have children
			if(this.IsMedia == false)
			{
				//	Make sure we have a valid collection
				if(m_tmaxChildren == null)
				{
					m_tmaxChildren = new CTmaxBinderItems();
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
			
			}// if(this.IsMedia == false)
			
			return m_tmaxChildren;
			
		}// public CTmaxBinderItems GetChildren()
		
		/// <summary>Called to add an item to the child collection</summary>
		/// <param name="tmaxChild">The object to add to the child collection</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxBinderItem tmaxChild)
		{
			bool bSuccessful = false;
			
			if((tmaxChild != null) && (this.Children != null))
			{
				this.Children.Add(tmaxChild);
				bSuccessful = true;
			}
			
			return bSuccessful;

		}// public bool Add(CTmaxBinderItem tmaxChild)
		
		/// <summary>Called to set the properties using the specified record interface</summary>
		/// <param name="IBinder">The exchange interface for the binder record</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(ITmaxMediaRecord IBinder)
		{
			bool bSuccessful = false;
			
			try
			{
				if((m_IBinder = IBinder) != null)
				{
					m_lAttributes = m_IBinder.GetAttributes();
					m_iDisplayOrder = (int)(m_IBinder.GetDisplayOrder());
					m_strDescription = m_IBinder.GetDescription();
					
					if(this.IsMedia == true)
					{
						m_strBarcode = m_IBinder.GetBarcode(false);
						m_strName = "";
					}
					else
					{
						m_strName = m_IBinder.GetName();
						m_strBarcode = "";
					}
					
				}
				
				bSuccessful = true;
			}
			catch
			{
			}
			
			return bSuccessful;

		}// public bool SetProperties(ITmaxMediaRecord IBinder)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxBinderItem">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxBinderItem, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxBinderItem tmaxBinderItem, long lMode)
		{
			//	Are these the same objects?
			if(ReferenceEquals(this, tmaxBinderItem) == true)
			{
				return 0;
			}
			else
			{
				//	Are we sorting on order?
				if(lMode == CTmaxBinderItems.SORT_ON_ORDER)
				{
					if(this.DisplayOrder == tmaxBinderItem.DisplayOrder)
						return 0;
					else if(this.DisplayOrder > tmaxBinderItem.DisplayOrder)
						return 1;
					else
						return -1;
				}
				else
				{						
					return CTmaxToolbox.Compare(this.Name, tmaxBinderItem.Name, true);
				}
			
			}// if(ReferenceEquals(this, tmaxBinderItem) == true)
					
		}// public int Compare(CTmaxBinderItem tmaxBinderItem, long lMode)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxBinderItem, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxBinderItem)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The exchange interface for the binder record bound to this item</summary>
		public FTI.Shared.Trialmax.ITmaxMediaRecord IBinder
		{
			get { return m_IBinder; }
			set { SetProperties(IBinder); }
		}
		
		/// <summary>The barcode assigned to the media record bound to this item</summary>
		public string Barcode
		{
			get { return m_strBarcode; }
			set { m_strBarcode = value; }
		}
		
		/// <summary>User defined display order identifier</summary>
		public int DisplayOrder
		{
			get { return m_iDisplayOrder; }
			set { m_iDisplayOrder = value; }
		}
		
		/// <summary>The packed flags that define the attributes</summary>
		public long Attributes
		{
			get { return m_lAttributes; }
			set { m_lAttributes = value; }
		}
		
		/// <summary>The name associated with the item</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>The description associated with the item</summary>
		public string Description
		{
			get { return m_strDescription; }
			set { m_strDescription = value; }
		}
		
		/// <summary>The collection of child items</summary>
		public FTI.Shared.Trialmax.CTmaxBinderItems Children
		{
			get { return GetChildren(); }
		}
		
		/// <summary>Flag to indicate that the item is bound to a media record</summary>
		public bool IsMedia
		{
			get 
			{ 
				return ((m_lAttributes & (long)TmaxBinderAttributes.IsMedia) != 0); 
			}
			set 
			{ 
				if(value == true)
				{
					m_lAttributes |= (long)TmaxBinderAttributes.IsMedia;
				}
				else
				{
					m_lAttributes &= ~((long)TmaxBinderAttributes.IsMedia);
				}
			
			}
		
		}// public bool IsMedia
		
		#endregion Properties
		
	}// public class CTmaxBinderItem : ITmaxSortable

	/// <summary>This class manages a list of system messages</summary>
	public class CTmaxBinderItems : CTmaxSortedArrayList
	{
		#region Constants
		
		public const int SORT_ON_NAME	= 0;
		public const int SORT_ON_ORDER	= 1;
		
		#endregion Constants
		
		#region Private Members
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxBinderItems() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			((CTmaxSorter)Comparer).Mode = SORT_ON_ORDER;
			
			this.KeepSorted = false;
			
			//	Initialize the event source
			m_tmaxEventSource.Name = "Binder Items";
			
		}// public CTmaxBinderItems() : base()

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
					foreach(CTmaxBinderItem O in this)
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
		public void Copy(CTmaxBinderItems tmaxSource)
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
			
			this.KeepSorted = tmaxSource.KeepSorted;
			
			foreach(CTmaxBinderItem O in tmaxSource)
				Add(new CTmaxBinderItem(O));
			
		}// public void Copy(CTmaxBinderItems tmaxSource)
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxBinderItem">CTmaxBinderItem object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxBinderItem Add(CTmaxBinderItem tmaxBinderItem)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxBinderItem as object);
					
				return tmaxBinderItem;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}
			
		}// public CTmaxBinderItem Add(CTmaxBinderItem tmaxBinderItem)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxBinderItem">The filter object to be removed</param>
		public void Remove(CTmaxBinderItem tmaxBinderItem)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxBinderItem as object);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Remove", Ex);
			}
		
		}// public void Remove(CTmaxBinderItem tmaxBinderItem)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxBinderItem">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxBinderItem tmaxBinderItem)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxBinderItem as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxBinderItem this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxBinderItem);
			}
		}

		/// <summary>This method is called to locate the object with the specified Name</summary>
		/// <param name="strName">The name to search for</param>
		/// <returns>The object with the specified Name</returns>
		public CTmaxBinderItem SearchForName(string strName)
		{
			foreach(CTmaxBinderItem O in this)
			{
				if(String.Compare(strName, O.Name, true) == 0)
					return O;
			}
			
			return null; // Not found...
			
		}// public CTmaxBinderItem SearchForName(string strName)

		/// <summary>This method is called to locate the object with the specified Barcode</summary>
		/// <param name="strBarcode">The name to search for</param>
		/// <returns>The object with the specified Barcode</returns>
		public CTmaxBinderItem SearchForBarcode(string strBarcode)
		{
			foreach(CTmaxBinderItem O in this)
			{
				if(String.Compare(strBarcode, O.Barcode, true) == 0)
					return O;
			}
			
			return null; // Not found...
			
		}// public CTmaxBinderItem SearchForBarcode(string strBarcode)

		#endregion Public Methods
		
		#region Private Methods
		
		#endregion Private Methods
		
		#region Properties
		
		//	Controls the field used to sort objects in the collection
		public int SortOn
		{
			get { return (this.Comparer != null ? (int)(((CTmaxSorter)(this.Comparer)).Mode) : CTmaxBinderItems.SORT_ON_NAME); }
			set { if(this.Comparer != null) ((CTmaxSorter)(this.Comparer)).Mode = value; }
		}
		
		#endregion Properties
		
	}//	public class CTmaxBinderItems : CTmaxSortedArrayList
		
}// namespace FTI.Shared.Trialmax
