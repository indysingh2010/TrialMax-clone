using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML transcript node</summary>
	public class CXmlTranscript : CXmlBase
	{
		#region Constants
		
		public const string XML_TRANSCRIPT_ELEMENT_NAME = "transcript";

		public const string XML_TRANSCRIPT_ATTRIBUTE_SEGMENT = "segment";
		public const string XML_TRANSCRIPT_ATTRIBUTE_PL = "pl";
		public const string XML_TRANSCRIPT_ATTRIBUTE_PAGE = "page";
		public const string XML_TRANSCRIPT_ATTRIBUTE_LINE = "line";
		public const string XML_TRANSCRIPT_ATTRIBUTE_QA = "qa";
		public const string XML_TRANSCRIPT_ATTRIBUTE_TEXT = "text";
		public const string XML_TRANSCRIPT_ATTRIBUTE_START = "start";
		public const string XML_TRANSCRIPT_ATTRIBUTE_STOP = "stop";
		public const string XML_TRANSCRIPT_ATTRIBUTE_EDITED = "edited";
		public const string XML_TRANSCRIPT_ATTRIBUTE_SYNCHRONIZED = "sync";
		
		#endregion Constants
		
		#region Protected Members

		/// <summary>This member is bounded to the Segment property</summary>
		protected string m_strSegment = "0";		
		
		/// <summary>This member is bounded to the PL property</summary>
		protected long m_lPL = 0;		
		
		/// <summary>This member is bounded to the Start property</summary>
		protected double m_dStart = 0;
		
		/// <summary>This member is bounded to the Stop property</summary>
		protected double m_dStop = 0;
		
		/// <summary>This member is bounded to the Page property</summary>
		protected long m_lPage = 0;
		
		/// <summary>This member is bounded to the Line property</summary>
		protected int m_iLine = 0;
		
		/// <summary>This member is bounded to the QA property</summary>
		protected string m_strQA = "";	
		
		/// <summary>This member is bounded to the Text property</summary>
		protected string m_strText = "";	
		
		/// <summary>This member is bounded to the Edited property</summary>
		protected bool m_bEdited = false;	
		
		/// <summary>This member is bounded to the Synchronized property</summary>
		protected bool m_bSynchronized = true;	
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlTranscript()
		{
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source node to be copied</param>
		public CXmlTranscript(CXmlTranscript xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
			
			if(xmlSource != null)
				Copy(xmlSource);
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				if(PL == ((CXmlTranscript)xmlBase).PL)
					return 0;
				else if(PL > ((CXmlTranscript)xmlBase).PL)
					return 1;
				else
					return -1;
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the properties of the source node</summary>
		public void Copy(CXmlTranscript xmlSource)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
			
			Edited = xmlSource.Edited;
			Line = xmlSource.Line;
			Page = xmlSource.Page;
			PL = xmlSource.PL;
			QA = xmlSource.QA;
			Segment = xmlSource.Segment;
			Start = xmlSource.Start;
			Stop = xmlSource.Stop;
			Text = xmlSource.Text;
			Synchronized = xmlSource.Synchronized;
			
		}// public void Copy(CXmlTranscript xmlSource)
		
		/// <summary>This method sets the properties to their default values</summary>
		/// <returns>true if successful</returns>
		public bool SetProperties()
		{
			m_strSegment = "0";
			m_lPL = 0;
			m_lPage = 0;
			m_iLine = 0;
			m_strQA = "";
			m_strText = "";
			m_dStart = 0;
			m_dStop = 0;
			m_bEdited = false;
			m_bSynchronized = true;
			
			return true;
			
		}// public bool SetProperties()
		
		/// <summary>This method uses the specified navigator to set the object properties</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			if(xpNavigator == null) return false;

			try
			{
				//	Make sure defaults are assigned
				SetProperties();
				
				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_SEGMENT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strSegment = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_PL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_PAGE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lPage = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_LINE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_iLine = System.Convert.ToInt32(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_QA,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strQA = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_TEXT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strText = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_START,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_dStart = System.Convert.ToDouble(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_STOP,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_dStop = System.Convert.ToDouble(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_EDITED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bEdited = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_TRANSCRIPT_ATTRIBUTE_SYNCHRONIZED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bSynchronized = XmlToBool(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML transcript line properties", Ex);
				return false;
			}
			
		}// protected bool SetProperties()
		
		/// <summary>This method uses the specified node to set the transcript text properties</summary>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlNode xmlNode)
		{
			XPathNavigator xpNavigator = null;
			
			Debug.Assert(xmlNode != null);
			
			try
			{
				if((xpNavigator = xmlNode.CreateNavigator()) != null)
					return SetProperties(xpNavigator);
				else
					return false;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML transcript line properties", Ex);
				return false;
			}
			
		}// protected bool SetProperties()

		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement  = null;
			bool		bSuccessful = false;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_TRANSCRIPT_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_SEGMENT, Segment) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_PL, PL) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_PAGE, Page) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_LINE, Line) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_PAGE, Page) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_LINE, Line) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_QA, QA) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_START, DoubleToXml(Start)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_STOP, DoubleToXml(Stop)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_TEXT, Text) == false)
						break;
					
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_EDITED, BoolToXml(m_bEdited)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_TRANSCRIPT_ATTRIBUTE_SYNCHRONIZED, BoolToXml(m_bSynchronized)) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>Called to determine if the specifed position is within this element's start/stop boundries</summary>
		/// <param name="dPosition">The desired position</param>
		/// <param name="dTolerance">The tolerance on either end to consider the position within range</param>
		/// <returns>0 if within bounds, negative if before, positive if after</returns>
		public double CheckPosition(double dPosition, double dTolerance)
		{
			//	Is the specified position within the bounds of this element?
			if((dPosition >= (m_dStart - dTolerance)) && (dPosition <= (m_dStop + dTolerance)))
			{
				return 0;	//	Within the bounds of this element
			}
			else if(dPosition < m_dStart)
			{
				return (dPosition - m_dStart); // Negative to indicate before this element
			}
			else
			{
				return (dPosition - m_dStop); // Positive to indicate after this element
			}
			
		}
		
		/// <summary>Called to determine if the specifed position is within this element's start/stop boundries</summary>
		/// <param name="dPosition">The desired position</param>
		/// <returns>0 if within bounds, negative if before, positive if after</returns>
		public double CheckPosition(double dPosition)
		{
			return CheckPosition(dPosition, 0.0);
		}
		
		/// <summary>Called to get the fully assembled line of text</summary>
		/// <returns>The complete line of text from the transcript</returns>
		public string GetFormattedText()
		{
			string strText = "";
			
			//	Is there a Q/A value?
			if((m_strQA != null) && (m_strQA.Length > 0))
			{
				strText = m_strQA;
				
				//	Add a period if necessary
				if(strText.EndsWith(".") == false)
					strText += ".";
					
				//	Add the actual text
				if(m_strText.Length > 0)
				{
					//	Make sure there is a space between the period and the text
					if(m_strText.StartsWith(" ") == false)
						strText += (" " + m_strText);
					else
						strText += m_strText;
				}
				
			}
			else
			{
				strText = m_strText;
			}
			
			return strText;
			
		}// public string GetFormattedText()


        //since this feature was implemented in 69 and reverted in 72, this code is being kept as commented

        //Part of the duplicate designations on merge script feature. Kept for future use.
        ///// <summary>
        ///// Compare if the provided transcript object has same properties as this
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public override bool Equals(Object obj)
        //{
        //    try
        //    {
        //        return this.DisplayString == ((CXmlTranscript)obj).DisplayString &&
        //           this.Line == ((CXmlTranscript)obj).Line &&
        //           this.Page == ((CXmlTranscript)obj).Page &&
        //           this.PL == ((CXmlTranscript)obj).PL &&
        //           this.QA == ((CXmlTranscript)obj).QA &&
        //           this.Segment == ((CXmlTranscript)obj).Segment &&
        //           this.Start == ((CXmlTranscript)obj).Start &&
        //           this.Stop == ((CXmlTranscript)obj).Stop &&
        //           this.Text == ((CXmlTranscript)obj).Text &&
        //           this.Edited == ((CXmlTranscript)obj).Edited &&
        //           this.Synchronized == ((CXmlTranscript)obj).Synchronized;
        //    }
        //    catch {
        //        return false;
        //    }   
        //}

		#endregion Public Methods
		
		#region Protected Methods
		
		
		#endregion Protected Methods
		
		#region Properties
		
		//	Key identifier of the owner segment
		public string Segment
		{
			get{ return m_strSegment; }
			set{ m_strSegment = value; }
			
		}// Segment Property
		
		//	Text of the file associated with the line
		public string Text
		{
			get{ return m_strText; }
			set{ m_strText = value; }
			
		}// Text Property
		
		//	Question / Answer indicator
		public string QA
		{
			get{ return m_strQA; }
			set{ m_strQA = value; }
			
		}// QA Property
		
		//	Page/Line of the line
		public long PL
		{
			get{ return m_lPL; }
			set{ m_lPL = value; }
			
		}// PL Property
		
		//	Page number of the line
		public long Page
		{
			get{ return m_lPage; }
			set{ m_lPage = value; }
			
		}// Page Property
		
		//	Line number
		public int Line
		{
			get{ return m_iLine; }
			set{ m_iLine = value; }
			
		}// Line Property
		
		//	Start frame / segment
		public double Start
		{
			get{ return m_dStart; }
			set{ m_dStart = value; }
			
		}// Start Property
		
		//	Stop frame / segment
		public double Stop
		{
			get{ return m_dStop; }
			set{ m_dStop = value; }
			
		}// Stop Property
		
		//	Flag to indicate if text has been edited
		public bool Edited
		{
			get{ return m_bEdited; }
			set{ m_bEdited = value; }
			
		}// Edited Property
		
		//	Flag to indicate if text has synchronized (time stamped) in the source log file
		public bool Synchronized
		{
			get{ return m_bSynchronized; }
			set{ m_bSynchronized = value; }
			
		}// Synchronized Property
		
		#endregion Properties
		
	}// public class CXmlTranscript

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CXmlTranscript objects
	/// </summary>
	public class CXmlTranscripts : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlTranscripts() : base()
		{
			//	Assign a default files sorter
			//base.Comparer = new CxmlTranscriptsSorter();
			//((CxmlTranscriptsSorter)base.Comparer).Ascending = true;
			//((CxmlTranscriptsSorter)base.Comparer).CaseSensitive = false;
			
			this.KeepSorted = false;
		}

		/// <summary>Overloaded constructor</summary>
		//		public CXmlTranscripts(CxmlTranscriptsSorter tmaxSorter) : base(tmaxSorter as IComparer)
		//		{
		//			//((CxmlTranscriptsSorter)base.Comparer).Ascending = true;
		//			//((CxmlTranscriptsSorter)base.Comparer).CaseSensitive = false;
		//			
		//			this.AllowDuplicates = true;
		//			this.KeepSorted = false;
		//		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="xmlTranscript">CXmlTranscript object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlTranscript Add(CXmlTranscript xmlTranscript)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlTranscript as object);

				return xmlTranscript;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlTranscript xmlTranscript)

		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(int iStart, double dPosition, bool bExact)
		{
			int		iPrevious = iStart;
			int		i;
			double	dOffset = 0;
			double	dPrevious = 0;
			
			try
			{
				//	Make sure we have a non-empty collection
				if((m_aList == null) || (m_aList.Count == 0)) return -1;
				
				//	Set the start index
				if((iStart < 0) || (iStart >= m_aList.Count))
					iStart = 0;
					
				//	Is the start element at the desired position?
				if((dOffset = this[iStart].CheckPosition(dPosition)) == 0)
					return iStart;

				dPrevious = dOffset;
				iPrevious = iStart;
					
				//	Do we need to iterate forward?
				if(dOffset > 0)
				{
					for(i = iStart + 1; i < Count; i++)
					{
						if((dOffset = this[i].CheckPosition(dPosition)) == 0)
							return i;

						//	Have we gone to far?
						if(dOffset < 0)
							break;

						dPrevious = dOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				}
				else 
				{
					//	Go backwards in search of the closest element
					for(i = iStart - 1; i >= 0; i--)
					{
						if((dOffset = this[i].CheckPosition(dPosition)) == 0)
							return i;

						//	Have we gone to far?
						if(dOffset > 0)
							break;

						dPrevious = dOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				
				}
				
				//	Is the caller looking for an exact match?
				if(bExact == true)
					return -1; // Not found
					
				//	Did we break out of the loop without finding a match?
				if(dPrevious == dOffset)
				{
					Debug.Assert(iPrevious >= 0);
					Debug.Assert(iPrevious < Count);
					return iPrevious;
				}
				else
				{
					//	Which is closer
					if(System.Math.Abs(dPrevious) <= System.Math.Abs(dOffset))
					{
						Debug.Assert(iPrevious >= 0);
						Debug.Assert(iPrevious < Count);
						return iPrevious;
					}
					else
					{
						Debug.Assert(i >= 0);
						Debug.Assert(i < Count);
						return i;
					}
				
				}
				
			}
			catch
			{
				Debug.Assert(false);
				return -1;
			}
			
		}// public int Locate(int iStart, double dPosition, bool bExact)

		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(double dPosition, bool bExact)
		{
			return Locate(0, dPosition, bExact);
		}
		
		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <returns>The index of the object at the specified position</returns>
		public int Locate(int iStart, double dPosition)
		{
			return Locate(iStart, dPosition, true);
		}
		
		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="dPosition">The time that indicates the desired element</param>
		/// <returns>The index of the object at the specified position</returns>
		public int Locate(double dPosition)
		{
			return Locate(0, dPosition, true);	
		}
			
		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="lPL">The PL value that indicates the desired element</param>
		/// <param name="bExact">true to return only elements that actually match the specified PL value</param>
		/// <returns>The index of the object at or closest to the specified PL value</returns>
		public int Locate(int iStart, long lPL, bool bExact)
		{
			int		iPrevious = iStart;
			int		i;
			long	lOffset = 0;
			long	lPrevious = 0;
			
			try
			{
				//	Make sure we have a non-empty collection
				if((m_aList == null) || (m_aList.Count == 0)) return -1;
				
				//	Set the start index
				if((iStart < 0) || (iStart >= m_aList.Count))
					iStart = 0;
					
				//	Is the start element at the desired position?
				if((lOffset = (lPL - this[iStart].PL)) == 0)
					return iStart;

				lPrevious = lOffset;
				iPrevious = iStart;
					
				//	Do we need to iterate forward?
				if(lOffset > 0)
				{
					for(i = iStart + 1; i < Count; i++)
					{
						if((lOffset = (lPL - this[i].PL)) == 0)
							return i;

						//	Have we gone to far?
						if(lOffset < 0)
							break;

						lPrevious = lOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				}
				else 
				{
					//	Go backwards in search of the closest element
					for(i = iStart - 1; i >= 0; i--)
					{
						if((lOffset = (lPL - this[i].PL)) == 0)
							return i;

						//	Have we gone to far?
						if(lOffset > 0)
							break;

						lPrevious = lOffset;
						iPrevious = i;

					}// for(int i = iIndex + 1; i < Count; i++)
				
				}
				
				//	Is the caller looking for an exact match?
				if(bExact == true)
					return -1; // Not found
					
				//	Did we break out of the loop without finding a match?
				if(lPrevious == lOffset)
				{
					Debug.Assert(iPrevious >= 0);
					Debug.Assert(iPrevious < Count);
					return iPrevious;
				}
				else
				{
					//	Which is closer
					if(System.Math.Abs(lPrevious) <= System.Math.Abs(lOffset))
					{
						Debug.Assert(iPrevious >= 0);
						Debug.Assert(iPrevious < Count);
						return iPrevious;
					}
					else
					{
						Debug.Assert(i >= 0);
						Debug.Assert(i < Count);
						return i;
					}
				
				}
				
			}
			catch
			{
				Debug.Assert(false);
				return -1;
			}
			
		}// public int Locate(int iStart, long lPL, bool bExact)

		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="lPL">The PL value that indicates the desired element</param>
		/// <param name="bExact">true to return only elements that actually map to the time</param>
		/// <returns>The index of the object at or closest to the specified position</returns>
		public int Locate(long lPL, bool bExact)
		{
			return Locate(0, lPL, bExact);
		}
		
		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="iStart">The index at which to start the search</param>
		/// <param name="lPL">The PL value that indicates the desired element</param>
		/// <returns>The index of the object at the specified position</returns>
		public int Locate(int iStart, long lPL)
		{
			return Locate(iStart, lPL, true);
		}
		
		/// <summary>Locates the transcript element corresponding to the desired position</summary>
		/// <param name="lPL">The PL value that indicates the desired element</param>
		/// <returns>The index of the object at the specified position</returns>
		public int Locate(long lPL)
		{
			return Locate(0, lPL, true);	
		}
			
		/// <summary>
		/// This method is called to remove the requested filter from the collection
		/// </summary>
		/// <param name="xmlTranscript">The object to be removed</param>
		public void Remove(CXmlTranscript xmlTranscript)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlTranscript as object);
			}
			catch
			{
			}
		}

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="xmlTranscript">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlTranscript xmlTranscript)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlTranscript as object);
		}

		/// <summary>
		/// Overloaded version of [] operator to return the object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CXmlTranscript this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlTranscript);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlTranscript value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>Called to check to see if the lines in the collection are synchronized</summary>
		/// <param name="bAll">true if all lines in the collection must be synchronized for the collection to be synchronized</param>
		/// <returns>true if the collection is synchronized</returns>
		public bool GetSynchronized(bool bAll)
		{
			//	Assume not synchronized if no text in the collection
			if(this.Count == 0) return false;
			
			//	Check each of the objects in the collection
			foreach(CXmlTranscript O in this)
			{
				//	Do all lines have to be synchronized?
				if(bAll == true)
				{
					if(O.Synchronized == false)
						return false;
				}
				else 
				{
					//	Just one or more line has to be synchronized
					if(O.Synchronized == true)
						return true;
				}
				
			}// foreach(CXmlTranscript O in this)
			
			//	Did all lines have to be synchronized?
			if(bAll == true)
				return true;
			else
				return false;
		
		}// public bool GetSynchronized(bool bAll)
		
		#endregion Public Methods
		
	}//	public class CXmlTranscripts
		
}// namespace FTI.Shared.Xml
