using System;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class encapsulates the information required for a single row in a transcript grid control</summary>
	public class CTmaxTransGridRow
	{
		#region Constants
		
		public const int IMAGE_NONE							= -1;
		public const int IMAGE_SEGMENT_BREAK				=  0;
		public const int IMAGE_SELECTED_DESIGNATION			=  1;
		public const int IMAGE_NO_SYNC						=  2;
		public const int IMAGE_EXTENDED_PAUSE				=  3;
		public const int IMAGE_SEGMENT_BREAK_PAUSE			=  4;
		
		public const int MAX_OBJECTION_IMAGE_INDEX = 4;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>XML transcript bound to the row</summary>
		private CXmlTranscript m_xmlTranscript = null;
			
		/// <summary>XML segment key</summary>
		private int m_iSegment = 0;
		
		//	True to show the pause indicator
		private bool m_bShowPause = false;
		
		//	Local member bounded to SegmentBreak property
		private bool m_bSegmentBreak = false;
		
		//	Local member bounded to GP property
		private string m_strGridPage = "";
		
		//	Local member bounded to GLI property
		private string m_strGridLineImage = "";

		//	Local member bounded to PO property
		private string m_strPlaintiffObjectionsImage = "";

		//	Local member bounded to DO property
		private string m_strDefendantObjectionsImage = "";

		//	Local member bounded to GAD property
		private string m_strGridActiveDesignation = "";

		//	Local member to store objections associated with this row
		private CTmaxObjections m_tmaxObjections = null;

		//	Index of this row in the grid
		private int m_iGridIndex = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxTransGridRow()
		{
		}
	
		/// <summary>This method is called to set the XML transcript member</summary>
		/// <param name="xmlTranscript">The XML transcript used to initialize this object</param>
		public void SetXmlTranscript(CXmlTranscript xmlTranscript)
		{
			//	Update the local member
			m_xmlTranscript = xmlTranscript;
			Debug.Assert(m_xmlTranscript != null);
				
			//	Update the object properties
			try { m_iSegment = System.Convert.ToInt32(xmlTranscript.Segment); }
			catch { m_iSegment = 0; }
			
		}// public void SetXmlTranscript(CXmlTranscript xmlTranscript)

		/// <summary>This method is called to get the XmlTranscript member</summary>
		/// <returns>The XML transcript used to initialize this object</returns>
		public CXmlTranscript GetXmlTranscript()
		{
			return m_xmlTranscript;
		}

		/// <summary>This method is called to set the SegmentBreak flag</summary>
		/// <param name="bSegmentBreak">True if this row starts a new segment</param>
		public void SetSegmentBreak(bool bSegmentBreak)
		{
			m_bSegmentBreak = bSegmentBreak;
		}

		/// <summary>This method is called to get the SegmentBreak flag</summary>
		/// <returns>True if this row starts a new segment</returns>
		public bool GetSegmentBreak()
		{
			return m_bSegmentBreak;
		}

		/// <summary>This method is called to get the Synchronized flag</summary>
		/// <returns>True if the line in this row was time stamped in the log file</returns>
		public bool GetSynchronized()
		{
			return (m_xmlTranscript != null ? m_xmlTranscript.Synchronized : false);
		}

		/// <summary>This method is called to get the time required to play the video for this line of text</summary>
		/// <returns>The playback duration in seconds</returns>
		public double GetDuration()
		{	
			double dDuration = 0;
			
			if((dDuration = (this.GetStop() - this.GetStart())) < 0)
				dDuration = 0;
				
			return dDuration;
		}

		/// <summary>This method is called to set the page number</summary>
		/// <param name="lPage">The desired page number</param>
		/// <summary>This method is called to get the page number</summary>
		/// <returns>The owner row's page number</returns>
		public long GetPage()
		{
			return (m_xmlTranscript != null ? m_xmlTranscript.Page : 0);
		}
			
		/// <summary>This method is called to get the segment number</summary>
		/// <returns>The owner row's segment number</returns>
		public int GetSegment()
		{
			return m_iSegment;
		}
			
		/// <summary>This method is called to get the PL number</summary>
		/// <returns>The owner row's PL number</returns>
		public long GetPL()
		{
			return (m_xmlTranscript != null ? m_xmlTranscript.PL : 0);
		}
			
		/// <summary>This method is called to set the start time</summary>
		/// <param name="dStart">The desired start time</param>
		/// <summary>This method is called to get the start time</summary>
		/// <returns>The owner row's start time</returns>
		public double GetStart()
		{
			return (m_xmlTranscript != null ? m_xmlTranscript.Start : 0);
		}
			
		/// <summary>This method is called to get the stop time</summary>
		/// <returns>The owner row's stop time</returns>
		public double GetStop()
		{
			return (m_xmlTranscript != null ? m_xmlTranscript.Stop : 0);
		}
			
		/// <summary>This method is called to set the grid index</summary>
		/// <param name="iGridIndex">The index of the row in the grid</param>
		public void SetGridIndex(int iGridIndex)
		{
			m_iGridIndex = iGridIndex;
		}
			
		/// <summary>This method is called to get the grid index</summary>
		/// <returns>The owner row's index number</returns>
		public int GetGridIndex()
		{
			return m_iGridIndex;
		}
			
		/// <summary>This method is called to set the show pause flag</summary>
		/// <param name="bShowPause">True to show the pause indicator</param>
		public void SetShowPause(bool bShowPause)
		{
			m_bShowPause = bShowPause;
		}
			
		/// <summary>This method is called to get the show pause flag</summary>
		/// <returns>The show pause flag</returns>
		public bool GetShowPause()
		{
			return m_bShowPause;
		}

		/// <summary>Called to get the string representation of the text displayed in this row</summary>
		/// <returns>The text representation of the row</returns>
		public override string ToString()
		{
			try
			{
				return String.Format("{0,6} {1,-2} {2,1} {3}", this.GP, this.LN, this.QA, this.Text.Trim());
			}
			catch
			{
				return this.Text;
			}

		}// public override string ToString()

		/// <summary>This method is called to set the collection of objections associated with this row</summary>
		///	<param name="tmaxObjections">The objections collection</param>
		public void SetObjections(CTmaxObjections tmaxObjections)
		{
			m_tmaxObjections = tmaxObjections;
		}

		/// <summary>This method is called to get the collection of objections</summary>
		/// <returns>The collection of objections for this row</returns>
		public CTmaxObjections GetObjections()
		{
			return m_tmaxObjections;
		}

		/// <summary>This method is called to add an objection to the collection</summary>
		///	<param name="tmaxObjections">The objection to be added</param>
		public void AddObjection(CTmaxObjection tmaxObjection)
		{
			//	Do we have to allocate a collection
			if(m_tmaxObjections == null)
				m_tmaxObjections = new CTmaxObjections();
			m_tmaxObjections.Add(tmaxObjection);

		}// public void AddObjection(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to remove an objection from the collection</summary>
		///	<param name="tmaxObjections">The objection to be remove</param>
		public void RemoveObjection(CTmaxObjection tmaxObjection)
		{
			if(m_tmaxObjections != null)
				m_tmaxObjections.Remove(tmaxObjection);

		}// public void RemoveObjection(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to get the total number of objections</summary>
		/// <returns>The number of objections in the collection</returns>
		public int GetObjectionsCount()
		{
			if(m_tmaxObjections != null)
				return m_tmaxObjections.Count;
			else
				return 0;

		}// public int GetObjectionsCount()

		/// <summary>This method is called to get the total number of objections</summary>
		/// <param name="iPlaintiff">the number associated with the plaintiff</param>
		/// <param name="iDefendant">the number associated with the defendant</param>
		/// <returns>The total number of objections in the collection</returns>
		public int GetObjectionsCount(ref int iPlaintiff, ref int iDefendant)
		{
			int	iTotal = 0;
			
			iPlaintiff = 0;
			iDefendant = 0;
			
			if(m_tmaxObjections != null)
			{
				if((iTotal = m_tmaxObjections.Count) > 0)
				{
					foreach(CTmaxObjection O in m_tmaxObjections)
					{
						if(O.Plaintiff == true)
							iPlaintiff += 1;
						else
							iDefendant += 1;

					}// foreach(CTmaxObjection O in m_tmaxObjections)

				}// if((iTotal = m_tmaxObjections.Count) > 0)

			}// if(m_tmaxObjections != null)
				
			return iTotal;

		}// public int GetObjectionsCount()

		/// <summary>This method is called to determine if the specified objection is in this row's collection</summary>
		/// <param name="tmaxObjection">The objection to search for</param>
		/// <returns>true if in this row's collection</returns>
		public bool ContainsObjection(CTmaxObjection tmaxObjection)
		{
			if(m_tmaxObjections != null)
				return m_tmaxObjections.Contains(tmaxObjection);
			else
				return false;

		}// public bool ContainsObjection(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to get the objection text displayed in the cell's tooltip</summary>
		/// <returns>The text to be displayed in the tool tip</returns>
		public string GetObjectionsToolTipText()
		{
			string	strText = "";
			int		iIndex = 1;
			
			if(m_tmaxObjections != null)
			{
				foreach(CTmaxObjection O in m_tmaxObjections)
				{
					if(strText.Length > 0)
						strText += "\n";

					strText += String.Format("{0}. {1} [{2} - {3}]", iIndex, O.Argument, CTmaxToolbox.PLToString(O.FirstPL), CTmaxToolbox.PLToString(O.LastPL));

					iIndex += 1;

				}// foreach(CTmaxObjection O in m_tmaxObjections)

			}// if(m_tmaxObjections != null)
			
			return strText;

		}// public string GetObjectionsToolTipText()

		/// <summary>This method is called to get the index of the image to be displayed in the Objections count column</summary>
		/// <param name="bPlaintiff">true if plaintiff objections</param>
		/// <param name="iCount">the total number of objections</param>
		/// <returns>The index of the image to be displayed</returns>
		public int GetObjectionsImageIndex(bool bPlaintiff, int iCount)
		{
			int iIndex = 0;

			if(bPlaintiff == false)
			{
				if(iCount >= 4)
					iIndex = 4;
				else
					iIndex = iCount;					
			}
			else
			{
				if(iCount >= 4)
					iIndex = 9;
				else
					iIndex = 5 + iCount;
			}
			
			return iIndex;

		}// public int GetObjectionsImageIndex(bool bPlaintiff, int iCount)

		/// <summary>This method is called to get the index of the image to be displayed in this row</summary>
		/// <returns>The image index if required</returns>
		public int GetImageIndex()
		{
			int iIndex = IMAGE_NONE;
				
			if(this.GetSynchronized() == false)
			{
				iIndex = IMAGE_NO_SYNC;
			}
			else if(this.GetSegmentBreak() == true)
			{
				if(this.GetShowPause() == true)
				{
					iIndex = IMAGE_SEGMENT_BREAK_PAUSE;
				}
				else
				{
					iIndex = IMAGE_SEGMENT_BREAK;
				}
					
			}
			else if(this.GetShowPause() == true)
			{
				iIndex = IMAGE_EXTENDED_PAUSE;
			}
				
			return iIndex;		
		}

		#endregion Public Methods
		
		#region Properties
		
		public int LN
		{
			get { return (m_xmlTranscript != null ? m_xmlTranscript.Line : 0); }
		}
		
		public string QA
		{
			get { return (m_xmlTranscript != null ? m_xmlTranscript.QA : ""); }
		}
		
		public string Text
		{
			get { return (m_xmlTranscript != null ? m_xmlTranscript.Text : ""); }
		}

		public string GP
		{
			get { return m_strGridPage; }
			set { m_strGridPage = value; }
		}

		public string GLI
		{
			get { return m_strGridLineImage; }
			set { m_strGridLineImage = value; }
		}

		public string GAD
		{
			get { return m_strGridActiveDesignation; }
			set { m_strGridActiveDesignation = value; }
		}

		public string PO
		{
			get { return ""; }
			set { m_strPlaintiffObjectionsImage = value; }
		}

		public string DO
		{
			get { return ""; }
			set { m_strDefendantObjectionsImage = value; }
		}
		
		#endregion Properties
		
		
	}// public class CTmaxTransGridRow
		
}
