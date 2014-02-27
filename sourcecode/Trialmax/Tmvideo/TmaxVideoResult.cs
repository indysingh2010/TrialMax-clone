using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class contains information associated with a search result</summary>
	public class CTmaxVideoResult : ITmaxSortable, ITmaxListViewCtrl
	{
		#region Private Members
		
		/// <summary>Local member bound to XmlScript property</summary>
		private CXmlScript m_xmlScript = null;
		
		/// <summary>Local member bound to XmlDesignation property</summary>
		private CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member bound to PL property</summary>
		private long m_lPL = 0;
		
		/// <summary>Local member bound to Page property</summary>
		private long m_lPage = 0;
		
		/// <summary>Local member bound to Line property</summary>
		private int m_iLine = 0;
		
		/// <summary>Local member bound to Text property</summary>
		private string m_strText = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoResult()
		{
		}
	
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="tmaxResult">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxResult, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxVideoResult tmaxResult, long lMode)
		{
			if(ReferenceEquals(this, tmaxResult) == true)
			{
				return 0;
			}
			else if(this.PL == tmaxResult.PL)
			{
				return 0;
			}
			else
			{
				return (this.PL < tmaxResult.PL ? -1 : 1);
			}
					
		}// public int Compare(CTmaxVideoResult tmaxResult)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxResult, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxVideoResult)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			string[] aNames = { "Designation", "Page", "Line", "Text" };
			return aNames;

		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[4];
			
			//	Do we have a designation bound to this result?
			if(this.XmlDesignation != null)
			{
				aValues[0] = String.Format("{0} - {1}",
					CTmaxToolbox.PLToString(this.XmlDesignation.FirstPL),
					CTmaxToolbox.PLToString(this.XmlDesignation.LastPL));
			}
			else
			{
				aValues[0] = "";
			}
			
			aValues[1] = this.Page.ToString();
			aValues[2] = this.Line.ToString();
			aValues[3] = this.Text;
			
			return aValues;
						
		}// string[] ITmaxListViewCtrl.GetValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			return -1;
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The XML script that owns the text</summary>
		public CXmlScript XmlScript
		{
			get { return m_xmlScript; }
			set { m_xmlScript = value; }
		}
		
		/// <summary>The XML designation that owns the text</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return m_xmlDesignation; }
			set { m_xmlDesignation = value; }
		}
		
		/// <summary>Text associated with this result</summary>
		public string Text
		{
			get { return m_strText; }
			set { m_strText = value; }
		}
		
		/// <summary>PL value associated with this result</summary>
		public long PL
		{
			get { return m_lPL; }
			set { m_lPL = value; }
		}
		
		/// <summary>Page number associated with this result</summary>
		public long Page
		{
			get { return m_lPage; }
			set { m_lPage = value; }
		}
		
		/// <summary>Line number associated with this result</summary>
		public int Line
		{
			get { return m_iLine; }
			set { m_iLine = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxVideoResult

	/// <summary>This class manages a list of search results</summary>
	public class CTmaxVideoResults : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoResults() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
			this.EventSource.Name = "Search Results";
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxResult">CTmaxVideoResult object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxVideoResult Add(CTmaxVideoResult tmaxResult)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxResult as object);

				return tmaxResult;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxVideoResult Add(CTmaxVideoResult tmaxResult)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxResult">The filter object to be removed</param>
		public void Remove(CTmaxVideoResult tmaxResult)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxResult as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxVideoResult tmaxResult)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxResult">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxVideoResult tmaxResult)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxResult as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxVideoResult this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxVideoResult);
			}
		}

		/// <summary>Called to locate the search result with the specified owner designation</summary>
		///	<param name="iIndex">The index at which to start the search</param>
		///	<param name="xmlDesignation">The owner designation</param>
		/// <returns>The object bound to the specified record</returns>
		public CTmaxVideoResult Find(int iIndex, CXmlDesignation xmlDesignation)
		{
			if(this.Count == 0) return null;
			
			//	Is the index within range?
			if(iIndex < 0)
				iIndex = 0;
			else if(iIndex >= this.Count)
				return null;
				
			for(int i = iIndex; i < this.Count; i++)
			{
				if(this[i] != null)
				{
					if(ReferenceEquals(this[i].XmlDesignation, xmlDesignation) == true)
						return this[i];
				}
				
			}
				
			return null;
			
		}// public CTmaxVideoResult Find(int iIndex, CXmlDesignation xmlDesignation)

		/// <summary>Called to locate the search results with the specified owner designation</summary>
		///	<param name="xmlDesignation">The owner designation</param>
		/// <param name="ICollection">The collection in which to store the objects</param>
		/// <returns>The number of objects located</returns>
		public long FindAll(CXmlDesignation xmlDesignation, IList IFound)
		{
			long lPrevious = IFound.Count;
			
			foreach(CTmaxVideoResult O in this)
			{
				if(ReferenceEquals(O.XmlDesignation, xmlDesignation) == true)
					IFound.Add(O);
			}
				
			return (IFound.Count - lPrevious);
			
		}// public long FindAll(CXmlDesignation xmlDesignation, IList IFound)

		/// <summary>Called to locate the search results with the specified owner designation</summary>
		///	<param name="xmlDesignation">The owner designation</param>
		/// <returns>The collection of search results bound to the specified record</returns>
		public CTmaxVideoResults FindAll(CXmlDesignation xmlDesignation)
		{
			CTmaxVideoResults tmaxResults = new CTmaxVideoResults();
			
			FindAll(xmlDesignation, tmaxResults);
			
			if(tmaxResults.Count > 0)
				return tmaxResults;
			else
				return null;

		}// public CTmaxVideoResults FindAll(CXmlDesignation xmlDesignation)

		#endregion Public Methods
		
	}//	public class CTmaxVideoResults

}// namespace FTI.Trialmax.TMVV.Tmvideo
