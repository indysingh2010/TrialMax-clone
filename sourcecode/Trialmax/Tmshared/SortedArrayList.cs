using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages a sorted array of objects</summary>
	public class CTmaxSortedArrayList : IList
	{
		#region Protected Members
		
		/// <summary>Local member bound to EventSource property</summary>
		protected CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local list member used to manage the objects in the collection</summary>
		protected System.Collections.Generic.List<object> m_aList = null;
		
		/// <summary>Local comparision interface used for sorting</summary>
		protected IComparer m_IComparer = null;
		
		/// <summary>Local member bounded to Owner property</summary>
		protected object m_oOwnerAssigned = null;
		
		/// <summary>Local member bounded to IsSorted property</summary>
		protected bool m_bIsSorted = true;

		/// <summary>Local member bounded to KeepSorted property</summary>
		protected bool m_bKeepSorted = false;

		#endregion Protected Members

		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxSortedArrayList()
		{ 
			Initialize(null, 0); 
		}

		/// <summary>Overloaded constructor to set the capacity on creation</summary>
		/// <param name="iCapacity">Capacity of the list</param>
		public CTmaxSortedArrayList(int iCapacity)
		{ 
			Initialize(null, iCapacity); 
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="Comparer">Will be used to compare added elements for sort and search operations</param>
		public CTmaxSortedArrayList(IComparer Comparer)
		{ 
			Initialize(Comparer, 0); 
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="Comparer">Will be used to compare added elements for sort and search operations</param>
		/// <param name="iCapacity">Capacity of the list</param>
		public CTmaxSortedArrayList(IComparer Comparer, int iCapacity)
		{ 
			Initialize(Comparer, iCapacity); 
		}

		/// <summary>Called to locate the index of specified object in the collection</summary>
		/// <param name="O">The object to locate</param>
		/// <returns>the zero-based index of the object if found</returns>
		public virtual int IndexOf(object O)
		{
			int iIndex = -1;

			try
			{
				return m_aList.IndexOf(O);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IndexOf", Ex);
			}
			
			return iIndex;

		}// public int IndexOf(object O)

		/// <summary>Called to retrieve the object at the specified index</summary>
		/// <param name="iIndex">The desired index</param>
		/// <returns>The object at the specified index</returns>
		public virtual object GetAt(int iIndex)
		{
			if((iIndex >= 0) && (iIndex < Count))
			{
				return m_aList[iIndex];
			}
			else
			{
				return null;
			}

		}// public object GetAt(int iIndex)
		
		/// <summary>Called to get the next object in the list</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The object that appears next in the list</returns>
		public virtual object GetNext(object O)
		{
			int iIndex = -1;
			
			iIndex = IndexOf(O);
			
			if((iIndex >= 0) && (iIndex < (this.Count - 1)))
				return GetAt(iIndex + 1);
			else
				return null;

		}// public virtual object GetNext(object O)
		
		/// <summary>Called to get the previous object in the list</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The object that appears prior in the list</returns>
		public virtual object GetPrevious(object O)
		{
			int iIndex = -1;
			
			iIndex = IndexOf(O);
			
			if(iIndex > 0)
				return GetAt(iIndex - 1);
			else
				return null;

		}// public virtual object GetPrevious(object O)
		
		/// <summary>Called to get the first object in the list</summary>
		/// <returns>The object that appears first in the list</returns>
		public virtual object GetFirst()
		{
			if(this.Count > 0)
				return GetAt(0);
			else
				return null;

		}// public virtual object GetFirst()
		
		/// <summary>Called to get the last object in the list</summary>
		/// <returns>The object that appears last in the list</returns>
		public virtual object GetLast()
		{
			if(this.Count > 0)
				return GetAt(this.Count - 1);
			else
				return null;

		}// public virtual object GetLast()
		
		/// <summary>Called to set the object at the specified index</summary>
		/// <param name="iIndex">The desired index</param>
		/// <param name="O">The new object</param>
		public virtual void SetAt(int iIndex, object O)
		{
			if(m_bKeepSorted == true) throw new InvalidOperationException("[] operator cannot be used to set a value if KeepSorted property is set to true.");
			
			if((iIndex >= 0) && (iIndex < this.Count))
			{
				//	Check to see if still in sorted state
				if(m_bIsSorted == true)
				{
					//	Should be greater than the previous object
					if((iIndex > 0) && (m_aList[iIndex - 1] != null))
					{
						if(Compare(m_aList[iIndex - 1], O) > 0)
							m_bIsSorted = false;
					}
				
					//	Should be less than the next object
					if((iIndex < Count - 1) && (m_aList[iIndex + 1] != null))
					{
						if(Compare(O, m_aList[iIndex + 1]) > 0)
							m_bIsSorted = false;
					}

				}// if(m_bIsSorted == true)

				m_aList[iIndex] = O;
			}
		
		}// public void SetAt(int iIndex, object O)

		/// <summary>Overloaded array operator</summary>
		public virtual object this[int iIndex]
		{
			get { return GetAt(iIndex); }
			set { SetAt(iIndex, value); }
		}

		/// <summary>Called to add an object to the list</summary>
		/// <param name="O">The object to add</param>
		/// <returns>The index where the object has been added.</returns>
		public virtual int Add(object O)
		{
			int iIndex = -1;

			try
			{
				//	Add to the list
				m_aList.Add(O);
				
				//	Do we need to make sure the list is sorted?
				if(m_bKeepSorted == true)
				{
					Sort();
					iIndex = IndexOf(O);
				}
				else
				{
					m_bIsSorted = false;
					iIndex = this.Count - 1;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", "Exception on add", Ex);
			}

			return iIndex;

		}// public int Add(object O)

		/// <summary>This method is called to add a collection of objects to the list</summary>
		/// <param name="OL">The list of objects to add</param>
		public virtual void AddRange(IList OL)
		{
			try
			{
				m_aList.AddRange(OL as IEnumerable<object>);
				
				if(m_bKeepSorted == true)
					Sort();
				else
					m_bIsSorted = false;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddRange", "Exception on add range", Ex);
			}

		}// public virtual void AddRange(IList OL)

		/// <summary>Inserts the object into the list at the specified index</summary>
		/// <param name="Index">The index before which the object must be added.</param>
		/// <param name="O">The object to inserted</param>
		public virtual void Insert(int iIndex, object O)
		{
			try
			{
				m_aList.Insert(iIndex, O);

				if(m_bKeepSorted == true)
					Sort();
				else
					m_bIsSorted = false;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Insert", "Exception on insert", Ex);
			}

		}// public void Insert(int iIndex, object O)

		/// <summary>This method is called to insert a collection of objects into the list</summary>
		/// <param name="Index">The index before which the objects must be added.</param>
		/// <param name="OL">The list of objects to insert</param>
		public virtual void InsertRange(int iIndex, IList OL)
		{
			//	Make sure the index is in range
			if(iIndex < 0) iIndex = 0;
				
			if(iIndex >= this.Count)
			{
				AddRange(OL);
			}
			else
			{
				try
				{
					m_aList.InsertRange(iIndex, OL as IEnumerable<object>);

					if(m_bKeepSorted == true)
						Sort();
					else
						m_bIsSorted = false;
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "InsertRange", "Exception on insert range", Ex);
				}

			}// if(iIndex >= this.Count)
			
		}// public void InsertRange(int iIndex, IList OL)

		/// <summary>Removes the object at the specified index</summary>
		/// <param name="iIndex">Index of object to remove</param>
		/// <remarks>This method is required to support the IList interface</remarks>
		public virtual void RemoveAt(int iIndex) 
		{	
			m_aList.RemoveAt(iIndex); 
		}

		/// <summary>Removes the specified object</summary>
		/// <param name="O">The object to be removed</param>
		/// <remarks>This method is required to support the IList interface</remarks>
		public virtual void Remove(object O) 
		{ 
			m_aList.Remove(O); 
		}

		/// <summary>Removes all objects in the list</summary>
		public virtual void Clear() 
		{	
			m_aList.Clear();
			m_bIsSorted = true; 
			
		}// public void Clear()

		/// <summary>This method is called to determine if an equivalent object exists in the list</summary>
		/// <param name="O">The object to look for</param>
		/// <returns>true if the object is in the list, otherwise false.</returns>
		public virtual bool Contains(object O)
		{
			return m_aList.Contains(O);

		}// public bool Contains(object O)

		/// <summary>Constructs a string that represents all items in the collection</summary>
		/// <returns>The string refecting the list objects</returns>
		public override string ToString()
		{
			string strObjects = "{";
			
			for(int i = 0; i < Count; i++)
			{
				strObjects += m_aList[i].ToString();
				
				if(i != Count - 1)
					strObjects += "; ";
					
			}
			
			strObjects += "}";
			
			return strObjects;

		}// public override string ToString()

		/// <summary>Copies object from this array to the specified target array</summary>
		/// <param name="aTarget">The target array</param>
		/// <param name="iIndex">The index of the object to be copied</param>
		/// <remarks>This method is required to support the ICollection interface</remarks>
		public virtual void CopyTo(Array aTarget, int iIndex) 
		{ 
			m_aList.CopyTo(aTarget as object[], iIndex); 
		}
		
		/// <summary>Called to get an enumerator for the collection</summary>
		/// <returns>Enumerator for the collection</returns>
		/// <remarks>This method is required to support the ICollection interface</remarks>
		public virtual IEnumerator GetEnumerator()
		{ 
			return m_aList.GetEnumerator(); 
		}

		/// <summary>Called to sort the objects in the list</summary>
		/// <param name="bAlways">Ignore IsSorted flag if true</param>
		public virtual void Sort(bool bAlways)
		{
			if(this.Count > 1)
			{
				if((bAlways == true) || (m_bIsSorted == false))
				{
					m_aList.Sort(delegate(object O1, object O2)
					{
						return Compare(O1, O2);
					});
				}

			}// if(this.Count > 1)
			
			m_bIsSorted = true;

		}// public virtual void Sort(bool bAlways)

		/// <summary>Called to sort the objects in the list</summary>
		public virtual void Sort()
		{
			//	Sort without regard for IsSorted flag
			Sort(true);
			
		}// public virtual void Sort()

		#endregion Public Methods
		
		#region Protected Methods
		
		protected virtual void Initialize(IComparer Comparer, int iCapacity)
		{
			m_IComparer = Comparer;

			m_aList = iCapacity > 0 ? new List<object>(iCapacity) : new List<object>();
		}

		/// <summary>This function is called to compare two objects</summary>
		/// <param name="O1">First object to be compared</param>
		/// <param name="O2">Second object to be compared</param>
		/// <returns>-1 if O1 less than O2, 0 if equal, 1 if O1 greater than O2</returns>
		protected virtual int Compare(object O1, object O2)
		{
			try
			{
				//	Are these the same object?
				if(ReferenceEquals(O1, O2) == true)
					return 0;

				//	Are we using a custom Comparer?
				if(m_IComparer != null)
				{
					return m_IComparer.Compare(O1, O2);
				}
				else
				{
					//	Objects using this sorter MUST support this interface
					ITmaxSortable S1 = (ITmaxSortable)O1;
					ITmaxSortable S2 = (ITmaxSortable)O2;

					return S1.Compare(S2, 0);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Compare", Ex);
				return -1;
			}

		}// protected virtual int Compare(object O1, object O2)

		#endregion Protected Members
		
		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>The number of objects in the list</summary>
		public int Count
		{
			get { return m_aList.Count; }
		}

		/// <summary>Current capacity of the list</summary>
		public int Capacity 
		{
			get { return m_aList != null ? m_aList.Capacity : 0; }
			set { if(m_aList != null) m_aList.Capacity = value; } 
		}

		/// <summary>Flag to indicate the list is to be kept in sorted order</summary>
		public bool KeepSorted
		{
			get { return m_bKeepSorted; }
			set { m_bKeepSorted = value; }
		}

		/// <summary>Indicates if the list is properly sorted</summary>
		public bool IsSorted 
		{ 
			get { return m_bIsSorted; }
			set { m_bIsSorted = value; } 			
		}
		
		/// <summary>The object assigned by the owner for its own use</summary>
		public object OwnerAssigned 
		{ 
			get { return m_oOwnerAssigned; } 
			set { m_oOwnerAssigned = value; }
		}
		
		/// <summary>The IComparer interface used for sorting</summary>
		public IComparer Comparer
		{ 
			get { return m_IComparer; } 
			set { m_IComparer = value; }			
		}

		/// <summary>true if list is read-only</summary>
		public bool IsReadOnly
		{
			get { return ((IList)(m_aList)).IsReadOnly; }
		}

		/// <summary>true if list is fixed size</summary>
		public bool IsFixedSize
		{
			get { return ((IList)(m_aList)).IsFixedSize; }
		}

		/// <summary>true if list is synchronized for thread-safe operation</summary>
		public bool IsSynchronized
		{
			get { return ((IList)(m_aList)).IsSynchronized; }
		}

		/// <summary>Returns an object that can be used to synchronize access to the collection</summary>
		public object SyncRoot
		{
			get { return ((ICollection)(m_aList)).SyncRoot; }
		}
		
		#endregion Properties

	}// public class CTmaxSortedArrayList : IList, ICloneable 
	
}// namespace FTI.Shared
