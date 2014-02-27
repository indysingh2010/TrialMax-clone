using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Encode
{
	/// <summary>This class contains information that identifies a SAMI source file</summary>
	public class CWMSAMILine
	{
		#region Private Members
		
		/// <summary>Local member bound to Source property</summary>
		private CWMSAMISource m_samiSource = null;
		
		/// <summary>Local member bound to XmlTranscript property</summary>
		private CXmlTranscript m_xmlTranscript = null;
		
		/// <summary>Local member bound to Duration property</summary>
		private double m_dDuration = -1.0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CWMSAMILine()
		{
		}
	
		/// <summary>Constructor</summary>
		/// <param name="samiSource">The SAMI source that owns this line</param>
		/// <param name="xmlTranscript">The transcript object bound to this line</param>
		public CWMSAMILine(CWMSAMISource samiSource, CXmlTranscript xmlTranscript)
		{
			m_samiSource = samiSource;
			m_xmlTranscript = xmlTranscript;
		}
		
		/// <summary>Called to get the XML designation bound to this line</summary>
		/// <returns>The XML transcript bound to this line</returns>
		public CXmlDesignation GetXmlDesignation()
		{
			if(m_samiSource != null)
				return m_samiSource.XmlDesignation;
			else
				return null;
				
		}// public XmlDesignation GetXmlDesignation()
		
		/// <summary>Called to get the time required for the line in seconds</summary>
		/// <returns>The time in fractional seconds</returns>
		public double GetDuration()
		{
			if(m_dDuration >= 0)
			{
				return m_dDuration;
			}
			else
			{
				if(m_xmlTranscript != null)
					return (m_xmlTranscript.Stop - m_xmlTranscript.Start);
				else
					return 0.0;
			}
				
		}// public double GetDuration()
		
		/// <summary>Called to get the playback start position of the bounded XML transcript</summary>
		/// <returns>The start position in fractional seconds</returns>
		public double GetStart()
		{
			if(m_xmlTranscript != null)
				return m_xmlTranscript.Start;
			else
				return -1.0;
				
		}// public double GetStart()
		
		/// <summary>Called to get the playback stop position of the bounded XML transcript</summary>
		/// <returns>The stop position in fractional seconds</returns>
		public double GetStop()
		{
			if(m_xmlTranscript != null)
				return m_xmlTranscript.Stop;
			else
				return -1.0;
				
		}// public double GetStop()
		
		/// <summary>Called to get the color used to display this line</summary>
		/// <param name="crDefault">The default color</param>
		/// <returns>The color to display this line</returns>
		public System.Drawing.Color GetColor(System.Drawing.Color crDefault)
		{
			if(m_samiSource != null)
				return m_samiSource.HighlighterColor;
			else
				return crDefault;
				
		}// public System.Drawing.Color GetColor(System.Drawing.Color crDefault)
		
		/// <summary>Called to get text used to write this line to the SYNC block</summary>
		/// <param name="bPageLine">True to include page/line number</param>
		/// <param name="crColor">The color to use for the line</param>
		/// <returns>The text to be written to file</returns>
		public string GetSyncText(bool bPageLine, System.Drawing.Color crColor)
		{
			string	strSubString = "";
			string	strQA = "";
			string	strText = "";
			
			//	No text if no transcript is bound to this line
			if(this.XmlTranscript != null)
			{
				if(this.XmlTranscript.QA.Length > 0)
					strQA = this.XmlTranscript.QA;
				else
					strQA = "";
					
				//	Format the line in accordance with SAMI specification
				if(bPageLine == true)
				{
					strSubString = String.Format("{0,4:0}:{1:00}&nbsp;{2}&nbsp;</FONT>{3}", 
						this.XmlTranscript.Page,
						this.XmlTranscript.Line,
						strQA,
						this.XmlTranscript.Text);
				}
				else
				{
					strSubString = String.Format("{0}&nbsp;</FONT>{1}", 
												 strQA, this.XmlTranscript.Text);
				
				}// if(bPageLine == true)
			
			}
			else
			{
				strSubString = " ";
			}
										
			strSubString = strSubString.Replace(" ", "&nbsp;");
				
			strText = String.Format("<Font Color={0}><font face=courier>{1}</Font><BR>",
									ColorTranslator.ToHtml(crColor),
									strSubString);
										 
			return strText;
				
		}// public string GetSyncText(bool bPageLine, System.Drawing.Color crColor)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Time in seconds required for the line</summary>
		public double Duration
		{
			get { return GetDuration(); }
			set { m_dDuration = value; }
		}
		
		/// <summary>The source that owns the line</summary>
		public CWMSAMISource Source
		{
			get { return m_samiSource; }
			set { m_samiSource = value; }
		}
		
		/// <summary>The transcript bound to this line</summary>
		public CXmlTranscript XmlTranscript
		{
			get { return m_xmlTranscript; }
			set { m_xmlTranscript = value; }
		}
		
		/// <summary>The XML designation that owns the transcript object bound to this line</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return GetXmlDesignation(); }
		}
		
		#endregion Properties
		
	}// public class CWMSAMILine : ITmaxSortable

	/// <summary>This class manages a list of encoder profiles</summary>
	public class CWMSAMILines : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CWMSAMILines() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
			
			this.EventSource.Name = "SAMI Lines";
		}

		/// <summary>This method allows the caller to add a new object to the collection</summary>
		/// <param name="samiSource">The source to which the line belongs</param>
		/// <param name="xmlDesignation">The XML transcript to bind to the new SAMI line</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSAMILine Add(CWMSAMISource samiSource, CXmlTranscript xmlTranscript)
		{
			CWMSAMILine samiLine = null;
			
			try
			{
				//	Allocate a new source object
				samiLine = new CWMSAMILine(samiSource, xmlTranscript);

				//	Add to the collection
				return Add(samiLine);
			}
			catch
			{
				return null;
			}
			
		}// public CWMSAMILine Add(CWMSAMISource samiSource)

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="O">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSAMILine Add(CWMSAMILine O)
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
			
		}// public CWMSAMILine Add(CWMSAMILine O)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="O">The object to be removed</param>
		public void Remove(CWMSAMILine O)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(O as object);
			}
			catch
			{
			}
		
		}// public void Remove(CWMSAMILine O)

		/// <summary>Called to get the time in seconds required all lines in this collection</summary>
		/// <returns>The time in seconds required to play all the lines</returns>
		public double GetDuration()
		{
			double dDuration = 0.0;
			
			foreach(CWMSAMILine O in this)
				dDuration += O.Duration;
				
			return dDuration;

		}// public double GetDuration()
		
		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="O">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CWMSAMILine O)
		{
			// Use base class to process actual collection operation
			return base.Contains(O as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CWMSAMILine this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CWMSAMILine);
			}
		}

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Total time required to play this source</summary>
		public double Duration
		{
			get { return GetDuration(); }
		}
		
		#endregion Properties
		
	}//	public class CWMSAMILines : CTmaxSortedArrayList
		
}// namespace FTI.Trialmax.Encode
