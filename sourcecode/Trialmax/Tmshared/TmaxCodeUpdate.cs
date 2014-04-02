using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with an individual filter term</summary>
	public class CTmaxCodeUpdate : ITmaxSortable, ITmaxListViewCtrl
	{
		#region Private Members
		
		/// <summary>Local member bound to CaseCode property</summary>
		private CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to MultiLevelSelection property</summary>
		private CTmaxPickItem m_tmaxMultiLevelSelection = null;
		
		/// <summary>Local member bound to Value property</summary>
		private string m_strValue = "";
		
		/// <summary>Local member bound to Action property</summary>
		private TmaxCodeActions m_eAction = TmaxCodeActions.Unknown;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxCodeUpdate()
		{
		}
	
		/// <summary>Constructor</summary>
		/// <param name="tmaxCode">The case code</param>
		public CTmaxCodeUpdate(CTmaxCaseCode tmaxCaseCode)
		{
			SetCaseCode(tmaxCaseCode);
		}
	
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">the source object to be copied</param>
		public CTmaxCodeUpdate(CTmaxCodeUpdate tmaxSource)
		{
			if(tmaxSource != null) 
				Copy(tmaxSource);
		}
	
		/// <summary>This method will copy the properties of the specified source object</summary>
		public void Copy(CTmaxCodeUpdate tmaxSource)
		{
			this.CaseCode = tmaxSource.CaseCode;
			this.MultiLevelSelection = tmaxSource.MultiLevelSelection;
			this.Value = tmaxSource.Value;
			this.Action = tmaxSource.Action;
		
		}// public void Copy(CTmaxCodeUpdate tmaxSource)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxCompare">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxCompare, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxCodeUpdate tmaxCompare, long lMode)
		{
			return -1;
					
		}// public int Compare(CTmaxCodeUpdate tmaxCompare, long lMode)
		
		/// <summary>This function is called to compare the specified object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxUpdate, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxCodeUpdate)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method is called to get the case code associated with this term</summary>
		/// <returns>The case code if applicable</returns>
		public FTI.Shared.Trialmax.CTmaxCaseCode GetCaseCode()
		{
			return m_tmaxCaseCode;
		}
		
		/// <summary>This method is called to set the case code bound to this term</summary>
		/// <param name="tmaxCode">The case code</param>
		public void SetCaseCode(CTmaxCaseCode tmaxCaseCode)
		{
			m_tmaxCaseCode = tmaxCaseCode;
		
		}// public void SetCaseCode(CTmaxCaseCode tmaxCaseCode)
		
		
		/// <summary>This method is called to get the multilevel selection associated with this term</summary>
		/// <returns>The multilevel pick list selection if applicable</returns>
		public FTI.Shared.Trialmax.CTmaxPickItem GetMultiLevelSelection()
		{
			return m_tmaxMultiLevelSelection;
		}
		
		/// <summary>This method is called to set the multilevel pick list selection bound to this term</summary>
		/// <param name="tmaxCode">The multilevel pick list selection</param>
		public void SetMultiLevelSelection(CTmaxPickItem tmaxSelection)
		{
			m_tmaxMultiLevelSelection = tmaxSelection;
		
		}// public void SetMultiLevelSelection(CTmaxPickItem tmaxSelection)
		
		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			string[] aNames = { "Code", "Assignment" };
			return aNames;
		
		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues    = new string[2];
			aValues[0] = this.CaseCode.Name;
			aValues[1] = this.Value;
			
			return aValues;
			
		}// string[] ITmaxListViewCtrl.GetValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			switch(this.Action)
			{
				case TmaxCodeActions.Delete:		return 0;
				case TmaxCodeActions.Add:			
				case TmaxCodeActions.Update:		return 1;
				case TmaxCodeActions.Unknown:		
				default:							return -1;
					
			}// switch(this.Action)

		}// int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The case code that defines the search field when term is applied to the Codes table</summary>
		public CTmaxCaseCode CaseCode
		{
			get { return GetCaseCode(); }
			set { SetCaseCode(value); }
		}
		
		/// <summary>The user selection when the case code is bound to a multi-level pick list</summary>
		public CTmaxPickItem MultiLevelSelection
		{
			get { return GetMultiLevelSelection(); }
			set { SetMultiLevelSelection(value); }
		}
		
		/// <summary>The action to be performed when the update is executed</summary>
		public TmaxCodeActions Action
		{
			get { return m_eAction; }
			set { m_eAction = value; }
		}
		
		/// <summary>The Value assigned to this term</summary>
		public string Value
		{
			get { return m_strValue; }
			set { m_strValue = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxCodeUpdate : ITmaxSortable

	/// <summary>This class manages a collection of filter terms</summary>
	public class CTmaxCodeUpdates : CTmaxSortedArrayList
	{
		#region Private Members
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to PickLists property</summary>
		private CTmaxPickItem m_tmaxPickLists = null;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxCodeUpdates() : base()
		{
			//	Assign a default sorter
			//base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
			
			m_tmaxEventSource.Name = "Code Updates";
		}

		/// <summary>This method will copy the terms in the source collection to this collection</summary>
		/// <param name="tmaxSource">The source collection of terms</param>
		public void Copy(CTmaxCodeUpdates tmaxSource)
		{
			//	Clear the existing objects
			this.Clear();
			
			m_tmaxCaseCodes = tmaxSource.CaseCodes;
			m_tmaxPickLists = tmaxSource.PickLists;
			
			//	Copy each of the source objects
			if((tmaxSource != null) && (tmaxSource.Count > 0))
			{
				foreach(CTmaxCodeUpdate O in tmaxSource)
				{
					Add(new CTmaxCodeUpdate(O));
				}
				
			}// if((tmaxSource != null) && (tmaxSource.Count > 0))
			
		}// public void Copy(CTmaxCodeUpdates tmaxSource)
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxUpdate">CTmaxCodeUpdate object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxCodeUpdate Add(CTmaxCodeUpdate tmaxUpdate)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxUpdate as object);

				return tmaxUpdate;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxCodeUpdate Add(CTmaxCodeUpdate tmaxUpdate)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxUpdate">The filter object to be removed</param>
		public void Remove(CTmaxCodeUpdate tmaxUpdate)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxUpdate as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxCodeUpdate tmaxUpdate)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxUpdate">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxCodeUpdate tmaxUpdate)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxUpdate as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxCodeUpdate this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxCodeUpdate);
			}
		}

		/// <summary>Called to set the active collection of case codes</summary>
		/// <param name="tmaxCodes">The active collection of case codes</param>
		public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		{
			m_tmaxCaseCodes = tmaxCodes;
			
		}// public void SetCaseCodes(CTmaxCaseCodes tmaxCodes)
		
		/// <summary>Called to set the active collection of pick lists</summary>
		/// <param name="tmaxPickLists">The active collection of pick lists</param>
		public void SetPickLists(CTmaxPickItem tmaxPickLists)
		{
			m_tmaxPickLists = tmaxPickLists;
			
		}// public void SetPickLists(CTmaxPickItem tmaxPickLists)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The active collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
			set { SetCaseCodes(value); }
		}
		
		/// <summary>The active collection of pick lists</summary>
		public CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
			set { SetPickLists(value); }
		}
		
		#endregion Properties
		
	}//	public class CTmaxCodeUpdates : CTmaxSortedArrayList
		
}// namespace FTI.Shared.Trialmax
