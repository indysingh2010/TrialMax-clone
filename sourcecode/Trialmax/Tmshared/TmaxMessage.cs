using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with a system message</summary>
	public class CTmaxMessage : ITmaxSortable
	{
		#region Private Members
		
		/// <summary>Local member bound to Text property</summary>
		private string m_strText = "";
		
		/// <summary>Local member bound to Level property</summary>
		private TmaxMessageLevels m_eLevel = TmaxMessageLevels.Text;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxMessage()
		{
		}
	
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxMessage">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxMessage, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxMessage tmaxMessage, long lMode)
		{
			return -1;
					
		}// public int Compare(CTmaxMessage tmaxMessage, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxMessage, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxMessage)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method builds the text representation of the message</summary>
		/// <returns>The default text representation</returns>
		public override string ToString()
		{
			if(m_strText.Length == 0) return base.ToString();

			switch(m_eLevel)
			{
				case TmaxMessageLevels.Warning:
				case TmaxMessageLevels.CriticalError:
				case TmaxMessageLevels.FatalError:

					return (m_eLevel.ToString() + ": " + m_strText);

				case TmaxMessageLevels.Text:
				case TmaxMessageLevels.Information:
				default:
				
					return m_strText;
			}
		
		}// public override string ToString()

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The Text associated with the message</summary>
		public string Text
		{
			get { return m_strText; }
			set { m_strText = value; }
		}
		
		/// <summary>The enumerated error level identifier</summary>
		public TmaxMessageLevels Level
		{
			get { return m_eLevel; }
			set { m_eLevel = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxMessage : ITmaxSortable

	/// <summary>This class manages a list of system messages</summary>
	public class CTmaxMessages : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxMessages() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxMessage">CTmaxMessage object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxMessage Add(CTmaxMessage tmaxMessage)
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
			
		}// public CTmaxMessage Add(CTmaxMessage tmaxMessage)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxMessage">The filter object to be removed</param>
		public void Remove(CTmaxMessage tmaxMessage)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxMessage as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxMessage tmaxMessage)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxMessage">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxMessage tmaxMessage)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxMessage as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxMessage this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxMessage);
			}
		}

		#endregion Public Methods
		
	}//	public class CTmaxMessages : CTmaxSortedArrayList
		
}// namespace FTI.Shared.Trialmax
