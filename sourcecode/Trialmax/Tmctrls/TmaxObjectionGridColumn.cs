using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class contains information associated with a column in the Objections grid</summary>
	public class CTmaxObjectionGridColumn : ITmaxSortable
	{
		#region Private Members

		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";

		/// <summary>Local member bound to Visible property</summary>
		private bool m_bVisible = true;

		/// <summary>Local member bound to Configurable property</summary>
		private bool m_bConfigurable = true;

		/// <summary>Local member bound to Width property</summary>
		private int m_iWidth = -1;

		/// <summary>Local member bound to Position property</summary>
		private int m_iPosition = 0;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CTmaxObjectionGridColumn()
		{
		}

		/// <summary>This function is called to compare this object to the caller's object</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this is less than the caller's, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxObjectionGridColumn O, long lMode)
		{
			return -1;

		}// public int Compare(CTmaxObjectionGridColumn O, long lMode)

		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this is less than the caller's, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxObjectionGridColumn)O, lMode); }
			catch { return -1; }

		}// int ITmaxSortable.Compare(ITmaxSortable O, long lMode)

		/// <summary>Called to get the text representation of the object</summary>
		/// <returns>The text associated with the column</returns>
		public override string ToString()
		{
			return m_strName;
		}
		
		#endregion Public Methods

		#region Properties

		/// <summary>The Name of this column in the grid</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}

		/// <summary>true if this column is visible</summary>
		public bool Visible
		{
			get { return m_bVisible; }
			set { m_bVisible = value; }
		}

		/// <summary>true if this column can be selected by the user</summary>
		public bool Configurable
		{
			get { return m_bConfigurable; }
			set { m_bConfigurable = value; }
		}

		/// <summary>The width of this column</summary>
		public int Width
		{
			get { return m_iWidth; }
			set { m_iWidth = value; }
		}

		/// <summary>The visible position of this column</summary>
		public int Position
		{
			get { return m_iPosition; }
			set { m_iPosition = value; }
		}

		#endregion Properties

	}// public class CTmaxObjectionGridColumn

	/// <summary>This class manages a list of search results</summary>
	public class CTmaxObjectionGridColumns : CTmaxSortedArrayList
	{
		#region Public Members

		/// <summary>Default constructor</summary>
		public CTmaxObjectionGridColumns() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();

			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="O">CTmaxObjectionGridColumn object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxObjectionGridColumn Add(CTmaxObjectionGridColumn O)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(O as object);

				return O;
			}
			catch
			{
				return null;
			}

		}// public CTmaxObjectionGridColumn Add(CTmaxObjectionGridColumn O)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="O">The object to be removed</param>
		public void Remove(CTmaxObjectionGridColumn O)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(O as object);
			}
			catch
			{
			}

		}// public void Remove(CTmaxObjectionGridColumn O)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="O">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxObjectionGridColumn O)
		{
			// Use base class to process actual collection operation
			return base.Contains(O as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxObjectionGridColumn this[int index]
		{
			// Use base class to process actual collection operation
			get
			{
				return (GetAt(index) as CTmaxObjectionGridColumn);
			}
		}

		#endregion Public Methods

	}//	public class CTmaxObjectionGridColumns

}// namespace FTI.Trialmax.Controls
