using System;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class encapsulates the information required to highlight a highlighter in a transcript grid control</summary>
	public class CTmaxHighlighter : ITmaxSortable
	{
		#region Private Members
		
		/// <summary>Private member bound to Id property</summary>
		private long m_lId = 0;
			
		/// <summary>Private member bound to Label property</summary>
		private string m_strLabel = "";
			
		/// <summary>Private member bound to Color property</summary>
		private System.Drawing.Color m_sysColor = System.Drawing.Color.Yellow;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxHighlighter()
		{
		}
	
		/// <summary>Constructor</summary>
		public CTmaxHighlighter(long lId)
		{
			m_lId = lId;
		}
	
		/// <summary>Overloaded member to retrieve default text representation</summary>
		/// <returns>The label assigned to the highlighter</returns>
		public override string ToString()
		{
			if(m_strLabel.Length > 0)
				return m_strLabel;
			else
				return "Unassigned";
		}

		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="tmaxHighlighter">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxHighlighter, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxHighlighter tmaxHighlighter, long lMode)
		{
			return -1;					
			
		}// public int Compare(CTmaxHighlighter tmaxHighlighter, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxResult, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxHighlighter)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method is called to get the string used to identify the line in the configuration file</summary>
		/// <returns>The configuration file line identifier</returns>
		public string GetIniKey()
		{
			return ("H" + m_lId.ToString());				
		}
		
		/// <summary>This method is called to get the string used to define this highlighter in an INI file</summary>
		/// <returns>The string descriptor for this highlighter</returns>
		public string GetIniString()
		{
			if(m_strLabel.Length > 0)
				return String.Format("{0},{1}", m_strLabel, m_sysColor.ToArgb().ToString("X"));
			else
				return "";
			
		}// public string GetIniString()
		
		/// <summary>This method is called to initialize the object using the specified configuration string</summary>
		/// <param name="strString">The configuration string</param>
		/// <returns>true if successful</returns>
		public bool InitFromString(string strString)
		{
			int			iArgb = 0;
			string []	aSubstrings = null;
			
			//	Clear the existing label
			m_strLabel = "";
			
			try
			{
				//	Separate the label and color fields
				if(strString.Length > 0)
					aSubstrings = strString.Split(',');
				
				if((aSubstrings != null) && (aSubstrings.GetUpperBound(0) >= 0))
				{
					//	Get the label
					m_strLabel = aSubstrings[0];
					
					//	Get the color
					if(aSubstrings.GetUpperBound(0) > 0)
					{
						try
						{
							iArgb = int.Parse(aSubstrings[1], System.Globalization.NumberStyles.HexNumber);
							m_sysColor = System.Drawing.Color.FromArgb(iArgb);
						}
						catch
						{
						}
						
					}// if(aSubstrings.GetUpperBound(0) > 0)
					
				}// if((aSubstrings != null) && (aSubstrings.GetUpperBound(0) >= 0))
				
			}
			catch
			{
			}
			
			return (m_strLabel.Length > 0);
				
		}// public bool InitFromString(string strString)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Unique Id assigned to the highlighter</summary>
		public long Id
		{
			get { return m_lId; }
			set { m_lId = value; }
		}
		
		/// <summary>Label assigned to the highlighter</summary>
		public string Label
		{
			get { return m_strLabel; }
			set { m_strLabel = value; }
		}
		
		/// <summary>Color assigned to the highlighter</summary>
		public System.Drawing.Color Color
		{
			get { return m_sysColor; }
			set { m_sysColor = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxHighlighter
		

	/// <summary>Objects of this class are used to manage a dynamic array of CTmaxHighlighter objects</summary>
	public class CTmaxHighlighters : CTmaxSortedArrayList
	{
		#region Constants
		
		const long HIGHLIGHTERS_DEFAULT_COUNT = 7;
		const string HIGHLIGHTERS_SECTION_NAME = "Highlighters";
				
		#endregion Constants
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxHighlighters() : base()
		{
			this.KeepSorted = false;
			
			m_tmaxEventSource.Name = "Transcript Highlighters";
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="tmaxHighlighter">CTmaxHighlighter object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxHighlighter Add(CTmaxHighlighter tmaxHighlighter)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxHighlighter as object);

				return tmaxHighlighter;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxHighlighter tmaxHighlighter)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="tmaxHighlighter">The object to be removed</param>
		public void Remove(CTmaxHighlighter tmaxHighlighter)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxHighlighter as object);
			}
			catch
			{
			}
		}
		
		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxHighlighter">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxHighlighter tmaxHighlighter)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxHighlighter as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxHighlighter this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxHighlighter);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxHighlighter value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>This method is called to populate the collection with the specified number of objects</summary>
		/// <param name="lCount">The number of objects to be added</param>
		/// <returns>tru if successful</returns>
		public bool SetCount(long lCount)
		{
			try
			{
				// Add objects?
				while(this.Count < lCount)
					Add(new CTmaxHighlighter(this.Count + 1));
					
				//	Remove objects?
				while(this.Count > lCount)
					RemoveAt(this.Count - 1);
					
				return true;

			}
			catch
			{
				return false;
			}
			
		}// public bool SetCount(long lCount)

		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		public void Load(CXmlIni xmlIni)
		{
			string strIniString;
			
			if(xmlIni.SetSection(HIGHLIGHTERS_SECTION_NAME) == false) return;
			
			//	Populate the collection if necessary
			if(this.Count == 0)
				SetCount(HIGHLIGHTERS_DEFAULT_COUNT);
				
			//	Initialize each object
			foreach(CTmaxHighlighter O in this)
			{
				strIniString = xmlIni.Read(O.GetIniKey());

				if(strIniString.Length > 0)
					O.InitFromString(strIniString);
			}
			
		}// public void Load(CXmlIni xmlIni)

		/// <summary>This method is called to store the application highlighters in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the application highlighter values</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(HIGHLIGHTERS_SECTION_NAME, true, true) == false) return;

			foreach(CTmaxHighlighter O in this)
			{
				xmlIni.Write(O.GetIniKey(), O.GetIniString());
			}
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to find the highlighter with the specified ID</summary>
		/// <param name="lId">The ID of the desired highlighter</param>
		/// <returns>The associated highlighter</returns>
		public CTmaxHighlighter Find(long lId)
		{
			foreach(CTmaxHighlighter O in this)
			{
				if(O.Id == lId)
					return O;
			}
			
			return null;
			
		}// public CTmaxHighlighter Find(long lId)
		
		#endregion Public Methods
		
	}//	public class CTmaxHighlighters : CTmaxSortedArrayList
		
}// namespace FTI.Shared.Trialmax
