using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with a search result</summary>
	public class CTmaxSearchResult : ITmaxSortable, ITmaxListViewCtrl
	{
		#region Private Members
		
		/// <summary>Local member bound to UserData1 property</summary>
		private object m_userData1 = null;
		
		/// <summary>Local member bound to UserData2 property</summary>
		private object m_userData2 = null;
		
		/// <summary>Local member bound to IDeponent property</summary>
		private ITmaxMediaRecord m_IDeposition = null;
		
		/// <summary>Local member bound to IScene property</summary>
		private ITmaxMediaRecord m_IScene = null;
		
		/// <summary>Local member bound to Transcript property</summary>
		private string m_strTranscript = "";
		
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
		public CTmaxSearchResult()
		{
		}
	
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="tmaxResult">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxResult, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxSearchResult tmaxResult, long lMode)
		{
			int iReturn = 0;
			
			//	Are both of these objects scenes?
			if((m_IScene != null) && (tmaxResult.IScene != null))
			{
				return CTmaxToolbox.Compare(m_IScene.GetBarcode(false), tmaxResult.IScene.GetBarcode(false), true);
			}
			else
			{
				//	Are they both referencing the same transcript?
				if((iReturn = CTmaxToolbox.Compare(m_strTranscript, tmaxResult.Transcript, true)) == 0)
				{
					//	Compare the PL values
					if(m_lPL < tmaxResult.PL)
						return -1;
					else
						return 1;
				}
				else
				{
					return iReturn;
				}
			
			}// if((m_IScene != null) && (tmaxResult.IScene != null))
					
		}// public int Compare(CTmaxSearchResult tmaxResult)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxResult, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxSearchResult)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			string[] aNames = { "Scene", "Transcript", "Page", "Line", "Text" };
			return aNames;

		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[5];
			if(this.IScene != null)
				aValues[0] = this.IScene.GetBarcode(false);
			else
				aValues[0] = "";
			aValues[1] = this.Transcript;
			aValues[2] = this.Page.ToString();
			aValues[3] = this.Line.ToString();
			aValues[4] = this.Text;
			
			return aValues;
						
		}// string[] ITmaxListViewCtrl.GetValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			return -1;
		}

		/// <summary>This function is called to get the text used to store the search result in a file</summary>
		/// <returns>The text string for file storage</returns>
		public string ToFileString()
		{
			string strResult = "";

			if(this.IScene != null)
				strResult = this.IScene.GetBarcode(false);
			strResult += "\t";

			strResult += this.Transcript;
			strResult += "\t";

			strResult += this.Page.ToString();
			strResult += "\t";

			strResult += this.Line.ToString();
			strResult += "\t";

			strResult += this.Text.ToString();
			
			return strResult;

		}// public string ToFileString()

		/// <summary>This function is called to get the text used to copy the result to the clipboard</summary>
		/// <returns>The text string for the clipboard</returns>
		public string ToClipboard()
		{
			//	Use the same format as for file storage
			return ToFileString();
		}

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The record exchange interface for the deposition associated with this result</summary>
		public ITmaxMediaRecord IDeposition
		{
			get { return m_IDeposition; }
			set { m_IDeposition = value; }
		}
		
		/// <summary>The record exchange interface for the scene associated with this result</summary>
		public ITmaxMediaRecord IScene
		{
			get { return m_IScene; }
			set { m_IScene = value; }
		}
		
		/// <summary>Name of the transcript associated with this result</summary>
		public string Transcript
		{
			get { return m_strTranscript; }
			set { m_strTranscript = value; }
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
		
		/// <summary>User specified data object</summary>
		public object UserData1
		{
			get { return m_userData1; }
			set { m_userData1 = value; }
		}
		
		/// <summary>User specified data object</summary>
		public object UserData2
		{
			get { return m_userData2; }
			set { m_userData2 = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxSearchResult

	/// <summary>This class manages a list of search results</summary>
	public class CTmaxSearchResults : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxSearchResults() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxResult">CTmaxSearchResult object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxSearchResult Add(CTmaxSearchResult tmaxResult)
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
			
		}// public CTmaxSearchResult Add(CTmaxSearchResult tmaxResult)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxResult">The filter object to be removed</param>
		public void Remove(CTmaxSearchResult tmaxResult)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxResult as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxSearchResult tmaxResult)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxResult">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxSearchResult tmaxResult)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxResult as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxSearchResult this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxSearchResult);
			}
		}

		/// <summary>Called to locate the search result with the specified record interface</summary>
		///	<param name="iIndex">The index at which to start the search</param>
		///	<param name="IRecord">The record exchange interface</param>
		/// <returns>The object bound to the specified record</returns>
		public CTmaxSearchResult Find(int iIndex, ITmaxMediaRecord IRecord)
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
					if(ReferenceEquals(this[i].IDeposition, IRecord) == true)
						return this[i];
					else if(ReferenceEquals(this[i].IScene, IRecord) == true)
						return this[i];
				}
				
			}
				
			return null;
			
		}// public CTmaxSearchResult Find(int iIndex, ITmaxMediaRecord IRecord)

		/// <summary>Called to locate the search results with the specified record interface</summary>
		///	<param name="IRecord">The record exchange interface</param>
		/// <param name="ICollection">The collection in which to store the objects</param>
		/// <returns>The number of objects located</returns>
		public long FindAll(ITmaxMediaRecord IRecord, IList IFound)
		{
			long lPrevious = IFound.Count;
			
			foreach(CTmaxSearchResult O in this)
			{
				if(ReferenceEquals(O.IDeposition, IRecord) == true)
					IFound.Add(O);
				else if(ReferenceEquals(O.IScene, IRecord) == true)
					IFound.Add(O);
			}
				
			return (IFound.Count - lPrevious);
			
		}// public long FindAll(ITmaxMediaRecord IRecord, IList IFound)

		/// <summary>Called to locate the search results with the specified record interface</summary>
		///	<param name="IRecord">The record exchange interface</param>
		/// <returns>The collection of search results bound to the specified record</returns>
		public CTmaxSearchResults FindAll(ITmaxMediaRecord IRecord)
		{
			CTmaxSearchResults tmaxResults = new CTmaxSearchResults();
			
			FindAll(IRecord, tmaxResults);
			
			if(tmaxResults.Count > 0)
				return tmaxResults;
			else
				return null;

		}// public CTmaxSearchResults FindAll(ITmaxMediaRecord IRecord)

		#endregion Public Methods
		
	}//	public class CTmaxSearchResults
		
}// namespace FTI.Shared.Trialmax
