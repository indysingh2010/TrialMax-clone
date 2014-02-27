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
	public class CWMSAMISource
	{
		#region Constants
			
		/// <summary>Error message identifiers</summary>
		private const int ERROR_FILL_LINES_EX		= 0;
		private const int ERROR_RESOLVE_TUNING_EX	= 1;
		
		private const double MINIMUM_DURATION = 0.001;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";		
		
		/// <summary>Local member bound to HighlighterColor property</summary>
		private System.Drawing.Color m_highlighterColor = System.Drawing.Color.Yellow;
				
		/// <summary>Local member bound to Start property</summary>
		private double m_dStart = 0;
		
		/// <summary>Local member bound to Stop property</summary>
		private double m_dStop = 0;
		
		/// <summary>Local member bound to XmlDesignation property</summary>
		private CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member bound to SamiLines property</summary>
		private CWMSAMILines m_aLines = new CWMSAMILines();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CWMSAMISource()
		{
			m_tmaxEventSource.Name = "SAMI Source";
			m_tmaxEventSource.Attach(m_aLines.EventSource);
			
			SetErrorStrings();
		}
	
		/// <summary>Constructor</summary>
		/// <param name="strFileSpec">Fully qualified path to the video file</param>
		/// <param name="dStart">Start time of source relative to beingging of it's parent video</param>
		/// <param name="dStop">Stop time of source relative to beingging of it's parent video</param>
		/// <param name="highlighterColor">Time in seconds of start position</param>
		public CWMSAMISource( string strFileSpec, double dStart, double dStop, System.Drawing.Color highlighterColor)
		{
			m_tmaxEventSource.Name = "SAMI Source";
			m_tmaxEventSource.Attach(m_aLines.EventSource);

			SetErrorStrings();
			
			SetProps(strFileSpec, dStart, dStop, highlighterColor);
		}
		
		/// <summary>Called to set the video properties</summary>
		/// <param name="strFileSpec">Fully qualified path to the video file</param>
		/// <param name="dStart">Start time of source relative to beginning of it's parent video</param>
		/// <param name="dStop">Stop time of source relative to beginning of it's parent video</param>
		/// <param name="highlighterColor">Time in seconds of start position</param>
		public void SetProps(string strFileSpec, double dStart, double dStop, System.Drawing.Color highlighterColor)
		{
			m_xmlDesignation = null;
			m_highlighterColor = highlighterColor;
			m_strFileSpec = strFileSpec;
			m_dStart = dStart;
			m_dStop = dStop;
			
			FillLines();
			
		}// public void SetProps(string strFileSpec, System.Drawing.Color highlighterColor)
		
		/// <summary>Called to set the video properties</summary>
		/// <param name="xmlDesignation">The XML designation containing the source text</param>
		/// <param name="highlighterColor">Time in seconds of start position</param>
		public void SetProps(CXmlDesignation xmlDesignation, System.Drawing.Color highlighterColor)
		{
			m_xmlDesignation   = xmlDesignation;
			m_highlighterColor = highlighterColor;
			
			//	Use the designation to set the extents
			if(m_xmlDesignation != null)
			{
				m_dStart = m_xmlDesignation.Start;
				m_dStop = m_xmlDesignation.Stop;
				m_strFileSpec = xmlDesignation.FileSpec;
			}
			else
			{
				m_dStart = 0;
				m_dStop = 0;
				m_strFileSpec = "";
			}
			
			//	Fill the lines collection
			FillLines();
		
		}// public void SetProps(string strFileSpec, System.Drawing.Color highlighterColor)
		
		/// <summary>Called to get the time in seconds required for this source</summary>
		/// <returns>The time in seconds required to play this source</returns>
		public double GetDuration()
		{
			if((m_dStop > 0) && (m_dStop > m_dStart))
				return (m_dStop - m_dStart);
			else if(m_xmlDesignation != null)
				return (m_xmlDesignation.Stop - m_xmlDesignation.Start);
			else
				return 0.0;
				
		}// public double GetDuration()
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while assigning the XML designation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while resolving the tuning information.");

		}// private void SetErrorStrings()

		/// <summary>Called to populate the lines collection</summary>
		/// <param name="xmlDesignation">The XML designation containing the source text</param>
		private void FillLines()
		{
			CWMSAMILine	wmsLine = null;
			
			try
			{
				//	Clear the existing contents
				m_aLines.Clear();
			
				if((m_xmlDesignation != null) && (m_xmlDesignation.Transcripts != null))
				{
					for(int i = 0; i < m_xmlDesignation.Transcripts.Count; i++)
					{
						wmsLine = new CWMSAMILine(this,  m_xmlDesignation.Transcripts[i]);
						
						//	Set the duration
						//
						//	NOTE:	The total time is actually the time between the start of this
						//			line and the start of the next line
						if(i < m_xmlDesignation.Transcripts.Count - 1)
						{
							wmsLine.Duration = m_xmlDesignation.Transcripts[i + 1].Start - m_xmlDesignation.Transcripts[i].Start;
						}
						else
						{
							//	Use the stop time of the designation
							//
							//	NOTE:	This also accounts for Stop position tuning
							if(m_xmlDesignation.Stop > m_xmlDesignation.Transcripts[i].Start)
								wmsLine.Duration = m_xmlDesignation.Stop - m_xmlDesignation.Transcripts[i].Start;
							else
								wmsLine.Duration = m_xmlDesignation.Transcripts[i].Stop - m_xmlDesignation.Transcripts[i].Start;
						}

						//	Add to the collection for this source
						m_aLines.Add(wmsLine);
					
					}// for(int i = 0; i < m_xmlDesignation.Transcripts.Count; i++)
				
					//	Make sure we account for tuning information
					ResolveTuning();
				
				}// if((m_xmlDesignation != null) && (m_xmlDesignation.Transcripts != null))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillLines", m_tmaxErrorBuilder.Message(ERROR_FILL_LINES_EX), Ex);
			}
			
		}// private void FillLines()
		
		/// <summary>Called to adjust the lines to allow for tuning of the bounded designation</summary>
		/// <returns>True if successful</returns>
		private bool ResolveTuning()
		{
			bool	bSuccessful = true;
			double	dAverage = 0;
			double	dAdjustment = 0;
			
			//	Make sure we have something to resolve
			if(this.XmlDesignation == null) return true;
			if(this.XmlDesignation.GetDuration() <= 0) return true;
			if(m_aLines == null) return true;
			if(m_aLines.Count == 0) return true;
			
			try
			{
				//	Check for special case where user tuned to position
				//	totally outside the range defined by the text
				if((m_aLines[0].GetStart() >= this.XmlDesignation.Stop) ||
				   (m_aLines[m_aLines.Count - 1].GetStop() <= this.XmlDesignation.Start))
				{
					//	Best we can do is divide the time equally
					dAverage = (this.XmlDesignation.GetDuration() / ((double)(m_aLines.Count)));
					
					foreach(CWMSAMILine O in m_aLines)
						O.Duration = dAverage;
					
					return true;
				}
				
				//	Has the start position been adjusted?
				if(this.XmlDesignation.StartTuned == true)
				{
					//	How much adjustment is required?
					if((dAdjustment = (m_aLines[0].GetStart() - this.XmlDesignation.Start)) >= 0)
					{
						//	Let the first line stay on screen longer than normal
						m_aLines[0].Duration = m_aLines[0].Duration + dAdjustment;
					}
					else
					{
						//	Get the absolute value
						dAdjustment *= -1.0;
						
						//	Adjust as many lines as required
						for(int i = 0; ((i < m_aLines.Count) && (dAdjustment > 0)); i++)
						{
							if(m_aLines[i].Duration > dAdjustment)
							{
								m_aLines[i].Duration = m_aLines[i].Duration - dAdjustment;
								break;
							}
							else
							{
								dAdjustment -= (m_aLines[i].Duration - MINIMUM_DURATION);
								m_aLines[i].Duration = MINIMUM_DURATION;
								
							}// if(m_aLines.Duration > dAdjustment)	
						
						}// for(int i = 0; i < m_aLines.Count; i++)
						
					}// if((dAdjustment = (m_aLines[0].GetStart() - this.XmlDesignation.Start)) >= 0)
						
				}// if(this.XmlDesignation.StartTuned == true)
				
				//	Just a final check to make sure the total duration matches 
				//	that of the video. This at least ensures that errors in
				//	the output file will not be cumulative
				//
				//	NOTE:	The duration of the last line was already adjusted
				//			once in the call to FillLines()
				if((dAdjustment = this.Duration - m_aLines.Duration) > 0)
				{
					//	Increase the time for the last line
					m_aLines[m_aLines.Count - 1].Duration = m_aLines[m_aLines.Count - 1].Duration + dAdjustment;
				}
				else
				{
					//	Get the absolute value
					dAdjustment *= -1.0;
						
					//	Adjust as many lines as required
					for(int i = m_aLines.Count - 1; ((i >= 0) && (dAdjustment > 0)); i--)
					{
						if(m_aLines[i].Duration > dAdjustment)
						{
							m_aLines[i].Duration = m_aLines[i].Duration - dAdjustment;
							break;
						}
						else
						{
							dAdjustment -= (m_aLines[i].Duration - MINIMUM_DURATION);
							m_aLines[i].Duration = MINIMUM_DURATION;
								
						}// if(m_aLines.Duration > dAdjustment)	
						
					}// for(int i = m_aLines.Count - 1; ((i >= 0) && (dAdjustment > 0)); i--)
						
				}// if((dAdjustment = this.Duration - m_aLines.Duration) > 0)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ResolveTuning", m_tmaxErrorBuilder.Message(ERROR_RESOLVE_TUNING_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
				
		}// private bool ResolveTuning()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The transcript lines to be written to the file</summary>
		public CWMSAMILines Lines
		{
			get { return m_aLines; }
		}
		
		/// <summary>Fully qualified path to the source file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { m_strFileSpec = value; }
		}
		
		/// <summary>Highlighter color to be applied to the text in this file</summary>
		public System.Drawing.Color HighlighterColor
		{
			get { return m_highlighterColor; }
			set { m_highlighterColor = value; }
		}
		
		/// <summary>Start time of this source relative to beginning of it's source video</summary>
		public double Start
		{
			get { return m_dStart; }
			set { m_dStart = value; }
		}
		
		/// <summary>Stop time of this source relative to beginning of it's source video</summary>
		public double Stop
		{
			get { return m_dStop; }
			set { m_dStop = value; }
		}
		
		/// <summary>Total time required to play this source</summary>
		public double Duration
		{
			get { return GetDuration(); }
		}
		
		/// <summary>XML designation containing the text to be written to file</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return m_xmlDesignation; }
			set { SetProps(value, this.HighlighterColor); }
		}
		
		#endregion Properties
		
	}// public class CWMSAMISource : ITmaxSortable

	/// <summary>This class manages a list of encoder profiles</summary>
	public class CWMSAMISources : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CWMSAMISources() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="strFileSpec">Fully qualified path to the video file</param>
		/// <param name="dStart">Start time of source relative to beingging of it's parent video</param>
		/// <param name="dStop">Stop time of source relative to beingging of it's parent video</param>
		/// <param name="highlighterColor">Time in seconds of start position</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSAMISource Add(string strFileSpec, double dStart, double dStop, System.Drawing.Color highlighterColor)
		{
			CWMSAMISource samiSource = null;
			
			try
			{
				//	Allocate a new source object
				samiSource = new CWMSAMISource();
				m_tmaxEventSource.Attach(samiSource.EventSource);
				
				//	Set its properties
				samiSource.SetProps(strFileSpec, dStart, dStop, highlighterColor);
				
				//	Add to the collection
				return Add(samiSource);
			}
			catch
			{
				return null;
			}
			
		}// public CWMSAMISource Add(string strFileSpec, System.Drawing.Color highlighterColor)

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="xmlDesignation">The XML designation bound to the source</param>
		/// <param name="highlighterColor">Time in seconds of start position</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSAMISource Add(CXmlDesignation xmlDesignation, System.Drawing.Color highlighterColor)
		{
			CWMSAMISource samiSource = null;
			
			try
			{
				//	Allocate a new source object
				samiSource = new CWMSAMISource();
				m_tmaxEventSource.Attach(samiSource.EventSource);
				
				//	Set its properties
				samiSource.SetProps(xmlDesignation, highlighterColor);
				
				//	Add to the collection
				return Add(samiSource);
			}
			catch
			{
				return null;
			}
			
		}// public CWMSAMISource Add(CXmlDesignation xmlDesignation, System.Drawing.Color highlighterColor)

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="O">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSAMISource Add(CWMSAMISource O)
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
			
		}// public CWMSAMISource Add(CWMSAMISource O)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="O">The object to be removed</param>
		public void Remove(CWMSAMISource O)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(O as object);
			}
			catch
			{
			}
		
		}// public void Remove(CWMSAMISource O)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="O">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CWMSAMISource O)
		{
			// Use base class to process actual collection operation
			return base.Contains(O as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CWMSAMISource this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CWMSAMISource);
			}
		}

		#endregion Public Methods
		
	}//	public class CWMSAMISources : CTmaxSortedArrayList
		
}// namespace FTI.Trialmax.Encode
