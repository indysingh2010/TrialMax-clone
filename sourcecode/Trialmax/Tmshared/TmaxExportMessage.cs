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
	public class CTmaxExportMessage : ITmaxSortable, ITmaxListViewCtrl
	{
		#region Private Members
		
		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";
		
		/// <summary>Local member bound to Message property</summary>
		private string m_strMessage = "";

		/// <summary>Local member bound to Level property</summary>
		private TmaxMessageLevels m_eLevel = TmaxMessageLevels.Text;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxExportMessage()
		{
		}
	
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="tmaxMessage">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxMessage, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxExportMessage tmaxMessage, long lMode)
		{
			return -1;

		}// public int Compare(CTmaxExportMessage tmaxMessage)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxMessage, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxExportMessage)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			string[] aNames = { "Message", "Filename" };
			return aNames;

		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[2];
			aValues[0] = this.Message;
			aValues[1] = this.Filename;

			return aValues;
						
		}// string[] ITmaxListViewCtrl.GetValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			//	The values returned here are consistent with
			//	the image list assigned to a CTmaxListCtrl object
			switch(this.Level)
			{
				case TmaxMessageLevels.CriticalError:
				case TmaxMessageLevels.FatalError:
				
					return 0;
					
				case TmaxMessageLevels.Warning:
				
					return 1;
					
				case TmaxMessageLevels.Information:
				case TmaxMessageLevels.Text:
				default:
				
					return 2;
					
			}// switch(this.Type)

		}// int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The filename associated with this message</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
		}
		
		/// <summary>The text associated with this message</summary>
		public string Message
		{
			get { return m_strMessage; }
			set { m_strMessage = value; }
		}
		
		/// <summary>The enumerated error level identifier</summary>
		public TmaxMessageLevels Level
		{
			get { return m_eLevel; }
			set { m_eLevel = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxExportMessage

	/// <summary>This class manages a list of search results</summary>
	public class CTmaxExportMessages : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxExportMessages() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxMessage">CTmaxExportMessage object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxExportMessage Add(CTmaxExportMessage tmaxMessage)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxMessage as object);

				return tmaxMessage;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxExportMessage Add(CTmaxExportMessage tmaxMessage)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxMessage">The filter object to be removed</param>
		public void Remove(CTmaxExportMessage tmaxMessage)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxMessage as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxExportMessage tmaxMessage)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxMessage">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxExportMessage tmaxMessage)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxMessage as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxExportMessage this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxExportMessage);
			}
		}

		#endregion Public Methods
		
	}//	public class CTmaxExportMessages
		
}// namespace FTI.Shared.Trialmax
