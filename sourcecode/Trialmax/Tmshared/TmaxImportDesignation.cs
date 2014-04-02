using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to process a line that contains a designation descriptor</summary>
	public class CTmaxImportDesignation
	{
		#region Private Members
			
		/// <summary>Local member bound to StartPage property</summary>
		private	long m_lStartPage = 0;
			
		/// <summary>Local member bound to StartLine property</summary>
		private	int m_iStartLine = 0;
			
		/// <summary>Local member bound to StopPage property</summary>
		private	long m_lStopPage = 0;
			
		/// <summary>Local member bound to StopLine property</summary>
		private	int m_iStopLine = 0;
			
		/// <summary>Local member bound to StartPL property</summary>
		private	long m_lStartPL = 0;
			
		/// <summary>Local member bound to StopPL property</summary>
		private	long m_lStopPL = 0;
			
		/// <summary>Local member bound to Deposition property</summary>
		private	string m_strDepositionId = "";
			
		/// <summary>Local member bound to Highlighter property</summary>
		private	int m_iHighlighterId = 0;
			
		/// <summary>Local member bound to StartTime property</summary>
		private	double m_dStartTime = -1.0;
			
		/// <summary>Local member bound to StopTime property</summary>
		private	double m_dStopTime = -1.0;
			
		/// <summary>Local class member bound to XmlDeposition property</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = null;
		
		/// <summary>Local class member bound to DxDeposition property</summary>
		private FTI.Shared.Trialmax.ITmaxMediaRecord m_dxDeposition = null;

		/// <summary>Local class member bound to XmlIgnoreLines property</summary>
		private FTI.Shared.Xml.CXmlTranscripts m_xmlIgnoreLines = null;
		
		/// <summary>Local class member bound to ErrorMessage property</summary>
		private string m_strErrorMessage = "";
		
		#endregion Private Members
			
		#region Public Methods
			
		/// <summary>Constructor</summary>
		public CTmaxImportDesignation()
		{
			//	Set the default values
			Initialize();
		}
			
		/// <summary>This method is called to determine if the specified designation covers one or more full pages</summary>
		/// <param name="bSinglePage">true to allow coverage only of a single page</param>
		/// <returns>true if the two are adjacent</returns>
		public bool IsFullPage(bool bSinglePage)
		{
			//	Can't tell unless we can get to the XML deposition
			if(this.XmlDeposition == null) return false;
				
			//	Must start on the first line of the transcript or first line
			//	of any other page
			if((this.StartPL != this.XmlDeposition.GetFirstPL()) && (this.StartLine != 1))
				return false;
					
			//	Must end on the last line of the transcript or the last line of any other page
			if((this.StopPL != this.XmlDeposition.GetLastPL()) && (this.StopLine != GetLinesPerPage()))
				return false;
					
			//	Are we confined to a single page?
			if(bSinglePage == true)
				return (this.StartPage == this.StopPage);
			else
				return true;
			
		}// public bool IsFullPage(bool bSinglePage)
		
		/// <summary>This method is called to get the PL of the next line in the transcript</summary>
		/// <returns>The PL value of the next line</returns>
		public long GetNextPL()
		{
			long	lPage = 0;
			int		iLine = 0;
				
			//	Are we on the last line of the page?
			if(this.StopLine == GetLinesPerPage())
			{
				lPage = this.StopPage + 1;
				iLine = 1;
			}
			else
			{
				lPage = this.StopPage;
				iLine = this.StopLine + 1;
			}
				
			return CTmaxToolbox.GetPL(lPage, iLine);
			
		}// public long GetNextPL()
		
		/// <summary>This method is called to get the maximum number of lines per page</summary>
		/// <returns>The number of lines per page</returns>
		public int GetLinesPerPage()
		{
			if((m_xmlDeposition != null) && (m_xmlDeposition.LinesPerPage > 0))
				return m_xmlDeposition.LinesPerPage;
			else
				return 25;
			
		}// public int GetLinesPerPage()
		
		/// <summary>This method sets the properties using the strings parsed from a line in an import file</summary>
		/// <param name="aFields">The collection of strings used to initialize the designation</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(string [] aFields)
		{
			string	strStartPage = "";
			string	strStartLine = "";
			string	strStopPage  = "";
			string	strStopLine  = "";
				
			Debug.Assert(aFields != null);
			if(aFields == null) return false;
			
			//	Set the default property values
			Initialize();
				
			//	Get the strings used to set the page/line extents
			if((aFields.GetUpperBound(0) >= 0) && (aFields[0] != null))
				strStartPage = aFields[0];
			if((aFields.GetUpperBound(0) >= 1) && (aFields[1] != null))
				strStartLine = aFields[1];
			if((aFields.GetUpperBound(0) >= 2) && (aFields[2] != null))
				strStopPage = aFields[2];
			if((aFields.GetUpperBound(0) >= 3) && (aFields[3] != null))
				strStopLine = aFields[3];
					
			//	At the very least we need a start page / line
			if((strStartPage.Length == 0) || (strStartLine.Length == 0))
				return OnError("Start page/line not defined");

			//	Get the start page
			try { m_lStartPage = System.Convert.ToInt64(strStartPage); }
			catch {}
			if(m_lStartPage <= 0) return OnError("Invalid start page");
				
			//	Get the start line
			try { m_iStartLine = System.Convert.ToInt32(strStartLine); }
			catch {}
			if(m_iStartLine <= 0) return OnError("Invalid start line");
				
			//	Did the user provide a stop page value?
			//
			//	This test allows for special case where the user is
			//	specifying a stop line on the same page
			if(strStopPage.Length > 0)
			{
				//	Treat the page as a line identifier if no line
				//	was specified
				if(strStopLine.Length == 0)
				{
					strStopLine = strStopPage;
					strStopPage = "";
				}
					
			}
				
			//	Get the stop page
			if(strStopPage.Length > 0)
			{
				try { m_lStopPage = System.Convert.ToInt64(strStopPage); }
				catch {}
				if(m_lStopPage <= 0) return OnError("Invalid stop page");
			}
			else
			{
				m_lStopPage = m_lStartPage;
			}

			//	Get the stop line
			if(strStopLine.Length > 0)
			{
				try { m_iStopLine = System.Convert.ToInt32(strStopLine); }
				catch {}
				if(m_iStopLine <= 0) return OnError("Invalid stop line");
			}
			else
			{
				m_iStopLine = m_iStartLine;
			}
				
			//	Convert to PL values
			m_lStartPL = CTmaxToolbox.GetPL(m_lStartPage, m_iStartLine);
			m_lStopPL = CTmaxToolbox.GetPL(m_lStopPage, m_iStopLine);

			//	Get the MediaId of the owner deposition
			if((aFields.GetUpperBound(0) >= 4) && (aFields[4] != null))
				m_strDepositionId = CTmaxToolbox.StripQuotes(aFields[4], true);
							
			//	Did the user specify a highlighter
			if((aFields.GetUpperBound(0) >= 5) && (aFields[5] != null) && (aFields[5].Length > 0))
			{
				try { m_iHighlighterId = System.Convert.ToInt32(aFields[5]); }
				catch { return OnError("Invalid highlighter id"); }
			}
								
			//	Did the user specify a start time
			if((aFields.GetUpperBound(0) >= 6) && (aFields[6] != null) && (aFields[6].Length > 0))
			{
				try { m_dStartTime = System.Convert.ToDouble(aFields[6]); }
				catch { return OnError("Invalid start time"); }
			}
							
			//	Did the user specify a stop time
			if((aFields.GetUpperBound(0) >= 7) && (aFields[7] != null) && (aFields[7].Length > 0))
			{
				try { m_dStopTime = System.Convert.ToDouble(aFields[7]); }
				catch { return OnError("Invalid stop time"); }
			}

			return true;
			
		}// private bool SetProperties(string [] aFields)

		/// <summary>Called to get the collection of transcript lines to be ignored</summary>
		/// <returns>The IgnoreLines collection</returns>
		public CXmlTranscripts GetXmlIgnoreLines()
		{
			try
			{
				//	Allocate the collection if necessary
				if(m_xmlIgnoreLines == null)
					m_xmlIgnoreLines = new CXmlTranscripts();
			}
			catch
			{
			}
				
			return m_xmlIgnoreLines;

		}// public CXmlTranscripts GetXmlIgnoreLines()

		/// <summary>Called to determine if there are any lines to be ignored</summary>
		/// <returns>true if there is at least one line to be ignored</returns>
		public bool GetHasIgnoreLines()
		{
			return ((m_xmlIgnoreLines != null) && (m_xmlIgnoreLines.Count > 0));
		}

		/// <summary>Called to ignore the specified line</summary>
		/// <param name="lPL">The composite page/line number of the line to be ignored</param>
		/// <returns>The associated XML transcript if successful</returns>
		public CXmlTranscript IgnoreLine(long lPL)
		{
			CXmlTranscript	xmlIgnored = null;
			int				iIndex = -1;
			
			try
			{
				//	Make sure the collection has been allocated
				if(this.XmlIgnoreLines != null)
				{
					//	Make sure the line is not already in the collection
					if((iIndex = this.XmlIgnoreLines.Locate(lPL, true)) >= 0)
					{
						xmlIgnored = this.XmlIgnoreLines[iIndex];
					}
					else
					{
						//	Create a new line and add it to the collection
						xmlIgnored = new CXmlTranscript();
						xmlIgnored.PL = lPL;
						xmlIgnored.Page = CTmaxToolbox.PLToPage(lPL);
						xmlIgnored.Line = CTmaxToolbox.PLToLine(lPL);
						
						this.XmlIgnoreLines.Add(xmlIgnored);
					}

				}// if(this.XmlIgnoreLines != null)
				
			}
			catch
			{
				xmlIgnored = null;
			}
			
			return xmlIgnored;

		}// public CXmlTranscript IgnoreLine(long PL)

		/// <summary>Called to ignore the specified line</summary>
		/// <param name="lPage">The page number that owns the line</param>
		/// <param name="iLine">The number of the line to be ignored</param>
		/// <returns>The associated XML transcript if successful</returns>
		public CXmlTranscript IgnoreLine(long lPage, int iLine)
		{
			long lPL = 0;

			if((lPL = CTmaxToolbox.GetPL(lPage, iLine)) > 0)
				return IgnoreLine(lPL);
			else
				return null;

		}// public CXmlTranscript IgnoreLine(long lPage, int iLine)

		#endregion Public Methods
			
		#region Private Members
			
		/// <summary>This method initializes the properties to their default values</summary>
		private void Initialize()
		{
			m_lStartPage = 0;
			m_lStopPage = 0;
			m_iStartLine = 0;
			m_iStopLine = 0;
			m_lStartPL = 0;
			m_lStopPL = 0;
			m_dStartTime = -1.0;
			m_dStopTime = -1.0;
			m_iHighlighterId = -1;
			m_strDepositionId = "";
			m_strErrorMessage = "";
			m_xmlDeposition = null;
			m_xmlIgnoreLines = null;
			m_dxDeposition = null;
				
			//	NOTE:	We purposely do NOT Clear() the deposition because
			//			the object may be shared by another designation
			
		}// private void Initialize()
			
		/// <summary>Called when an error is encountered</summary>
		/// <param name="strMessage">The error message</param>
		/// <returns>false</returns>
		private bool OnError(string strMessage)
		{
			m_strErrorMessage = strMessage;
			return false;
		}
			
		#endregion Private Members
			
		#region Properties
			
		/// <summary>The start page</summary>
		public long StartPage
		{
			get { return m_lStartPage; }
			set { m_lStartPage = value; }
		}
			
		/// <summary>The start line</summary>
		public int StartLine
		{
			get { return m_iStartLine; }
			set { m_iStartLine = value; }
		}
			
		/// <summary>The start page</summary>
		public long StopPage
		{
			get { return m_lStopPage; }
			set { m_lStopPage = value; }
		}
			
		/// <summary>The start line</summary>
		public int StopLine
		{
			get { return m_iStopLine; }
			set { m_iStopLine = value; }
		}
			
		/// <summary>The packed start position</summary>
		public long StartPL
		{
			get { return m_lStartPL; }
			set { m_lStartPL = value; }
		}
			
		/// <summary>The packed stop position</summary>
		public long StopPL
		{
			get { return m_lStopPL; }
			set { m_lStopPL = value; }
		}

		/// <summary>The Highlighter Id</summary>
		public int HighlighterId
		{
			get { return m_iHighlighterId; }
			set { m_iHighlighterId = value; }
		}

		/// <summary>The Deposition Id</summary>
		public string DepositionId
		{
			get { return m_strDepositionId; }
			set { m_strDepositionId = value; }
		}

		/// <summary>The playback start time</summary>
		public double StartTime
		{
			get { return m_dStartTime; }
			set { m_dStartTime = value; }
		}

		/// <summary>The playback stop time</summary>
		public double StopTime
		{
			get { return m_dStopTime; }
			set { m_dStopTime = value; }
		}

		/// <summary>The active XML deposition</summary>
		public FTI.Shared.Xml.CXmlDeposition XmlDeposition
		{
			get { return m_xmlDeposition; }
			set { m_xmlDeposition = value; }
		}

		/// <summary>The collection of transcript lines to be ignored when creating the XML designation</summary>
		public FTI.Shared.Xml.CXmlTranscripts XmlIgnoreLines
		{
			get { return GetXmlIgnoreLines(); }
		}

		/// <summary>True if there is at least one line to be ignored</summary>
		public bool HasIgnoreLines
		{
			get { return GetHasIgnoreLines(); }
		}

		/// <summary>The deposition record exchange interface</summary>
		public FTI.Shared.Trialmax.ITmaxMediaRecord DxDeposition
		{
			get { return m_dxDeposition; }
			set { m_dxDeposition = value; }
		}

		/// <summary>The error message to be reported</summary>
		public string ErrorMessage
		{
			get { return m_strErrorMessage; }
		}

		#endregion Properties
			
	}// public class CTmaxImportDesignation

}// namespace FTI.Shared.Trialmax

