using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>Objects that can be sorted using a CTmaxSorter comparer must support this interface</summary>
	public interface ITmaxSortable
	{
		/// <summary>This method is called to compare two sortable objects</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">The user defined mode identifier</param>
		/// <returns>-1 if this less than O, 0 if equal, 1 if greater than</returns>
		int Compare(ITmaxSortable O, long lMode);
	
	}// public interface ITmaxSortable

	/// <summary>This class implements a generic comparator for objects that support the ITmaxSortable interface</summary>
	public class CTmaxSorter : IComparer
	{
		#region Protected Members
		
		/// <summary>Local member bound to Mode property</summary>
		protected long m_lMode = 0;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxSorter()
		{
		}
		
		/// <summary>Constructor</summary>
		/// <param name="iMode">User defined identifier passed to comparison method</param>
		public CTmaxSorter(long lMode)
		{
			m_lMode = lMode;
		}
		
		/// <summary>This function is called to compare two objects</summary>
		/// <param name="O1">First object to be compared</param>
		/// <param name="O2">Second object to be compared</param>
		/// <returns>-1 if O1 less than O2, 0 if equal, 1 if O1 greater than O2</returns>
		int IComparer.Compare(object O1, object O2) 
		{
			try
			{
				//	Are these the same object?
				if(ReferenceEquals(O1, O2) == true)
					return 0;
					
				//	Objects using this sorter MUST support this interface
				ITmaxSortable S1 = (ITmaxSortable)O1;
				ITmaxSortable S2 = (ITmaxSortable)O2;
			
				return S1.Compare(S2, Mode);
			}
			catch
			{
				return -1;
			}

		}// int IComparer.Compare(object O1, object O2)

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>User defined value passed to comparison method</summary>
		public long Mode
		{
			get { return m_lMode; }
			set { m_lMode = value; }
		}
		
		#endregion Properties
		
	}// class CTmaxSorter

}// namespace FTI.Shared.Trialmax
