using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition segment</summary>
	public class CXmlSegment : CXmlBase
	{
		#region Constants
		
		public const string XML_SEGMENT_ELEMENT_NAME = "segment";

		public const string XML_SEGMENT_ATTRIBUTE_KEY = "key";
		public const string XML_SEGMENT_ATTRIBUTE_FILENAME = "filename";
		public const string XML_SEGMENT_ATTRIBUTE_FIRST_PL = "firstPL";
		public const string XML_SEGMENT_ATTRIBUTE_LAST_PL = "lastPL";
		public const string XML_SEGMENT_ATTRIBUTE_START = "start";
		public const string XML_SEGMENT_ATTRIBUTE_STOP = "stop";
		public const string XML_SEGMENT_ATTRIBUTE_START_TUNED = "startTuned";
		public const string XML_SEGMENT_ATTRIBUTE_STOP_TUNED = "stopTuned";
		public const string XML_SEGMENT_ATTRIBUTE_FRAMES_PER_SECOND = "fps";
		
		#endregion Constants
		
		#region Protected Members

		/// <summary>Local member bound to Transcripts property</summary>
		protected CXmlTranscripts m_xmlTranscripts = new CXmlTranscripts();

		/// <summary>This member is bounded to the Key property</summary>
		protected string m_strKey = "0";		
		
		/// <summary>This member is bounded to the Filename property</summary>
		protected string m_strFilename = "";		
		
		/// <summary>This member is bounded to the FirstPL property</summary>
		protected long m_lFirstPL = -1;		
		
		/// <summary>This member is bounded to the LastPl property</summary>
		protected long m_lLastPL = -1;		
		
		/// <summary>This member is bounded to the Start property</summary>
		protected double m_dStart = 0;
		
		/// <summary>This member is bounded to the Stop property</summary>
		protected double m_dStop = 0;
		
		/// <summary>This member is bounded to the StartTuned property</summary>
		protected bool m_bStartTuned = false;
		
		/// <summary>This member is bounded to the StopTuned property</summary>
		protected bool m_bStopTuned = false;
		
		/// <summary>This member is bounded to the FramesPerSecond property</summary>
		protected float m_fFramesPerSecond = 0;	
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlSegment() : base()
		{
			m_xmlTranscripts.Comparer = new CXmlBaseSorter();
			m_xmlTranscripts.KeepSorted = false;
			
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source node to be copied</param>
		public CXmlSegment(CXmlSegment xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
				
			m_xmlTranscripts.Comparer = new CXmlBaseSorter();
			m_xmlTranscripts.KeepSorted = false;
			
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
				if(FirstPL >= 0)
				{
					if(FirstPL == ((CXmlSegment)xmlBase).FirstPL)
						return 0;
					else if(FirstPL > ((CXmlSegment)xmlBase).FirstPL)
						return 1;
					else
						return -1;
				}
				else
				{
					return -1;
				}
				
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the properties of the source node</summary>
		///	<param name="xmlSource">the source node to be copied</param>
		public void Copy(CXmlSegment xmlSource)
		{
			Copy(xmlSource, true);
		}
			
		/// <summary>Called to copy the properties of the source node</summary>
		///	<param name="xmlSource">the source node to be copied</param>
		/// <param name="bTranscripts">true to copy the transcripts</param>
		public void Copy(CXmlSegment xmlSource, bool bTranscripts)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
					
			Filename = xmlSource.Filename;
			FirstPL = xmlSource.FirstPL;
			FramesPerSecond = xmlSource.FramesPerSecond;
			Key = xmlSource.Key;
			LastPL = xmlSource.LastPL;
			Start = xmlSource.Start;
			StartTuned = xmlSource.StartTuned;
			Stop = xmlSource.Stop;
			StopTuned = xmlSource.StopTuned;
			
			//	Should we copy the transcript nodes?
			m_xmlTranscripts.Clear();
			if(bTranscripts == true)
			{
				if(xmlSource.Transcripts != null)
				{
					foreach(CXmlTranscript O in xmlSource.Transcripts)
						m_xmlTranscripts.Add(new CXmlTranscript(O));
				}
					
			}
					
		}// public void Copy(CXmlTranscript xmlSource)
		
		/// <summary>Called to clear the segment's child collections</summary>
		public void Clear()
		{
			m_xmlTranscripts.Clear();
			
		}// public void Clear()
		
		/// <summary>This method will set the segment properties using the specified node</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML segment properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the segment properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_KEY,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strKey = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_FILENAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strFilename = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_FIRST_PL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lFirstPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_LAST_PL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lLastPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_START,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_dStart = System.Convert.ToDouble(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_STOP,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_dStop = System.Convert.ToDouble(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_START_TUNED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					if(String.Compare(strAttribute, "Yes", true) == 0)
						m_bStartTuned = true;
				}

				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_STOP_TUNED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					if(String.Compare(strAttribute, "Yes", true) == 0)
						m_bStopTuned = true;
				}

				strAttribute = xpNavigator.GetAttribute(XML_SEGMENT_ATTRIBUTE_FRAMES_PER_SECOND,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					try
					{
						m_fFramesPerSecond = System.Convert.ToSingle(strAttribute);
					}
					catch
					{
					}
				}

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML segment properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method is called to populate the transcripts collection</summary>
		/// <param name="xpDocument">The XPath document containing the transcript nodes</param>
		/// <returns>true if successful</returns>
		public bool GetTranscripts(XPathDocument xpDocument, string strDepositionPath)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			string				strPath = "";
			
			try
			{
				//	Make sure we have a valid collection
				Debug.Assert(m_xmlTranscripts != null);
				m_xmlTranscripts.Clear();
					
				if((strDepositionPath != null) && (strDepositionPath.Length > 0))
					strPath = String.Format("{0}/{1}/{2}[@{1}=\"{3}\"]", strDepositionPath, CXmlSegment.XML_SEGMENT_ELEMENT_NAME, CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME, m_strKey);
				else
					strPath = String.Format("{0}/{1}/{2}/{3}[@{2}=\"{4}\"]", CXmlDeposition.XML_DEPOSITION_ROOT_NAME, CXmlDeposition.XML_DEPOSITION_ELEMENT_NAME, CXmlSegment.XML_SEGMENT_ELEMENT_NAME, CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME, m_strKey);
				
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strPath)) == null) return false;
				
				return GetTranscripts(xpIterator);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", "An exception was raised while attempting to populate the segment's transcript text", Ex);
				return false;
			}
			
		}// public bool GetTranscripts(XPathDocument xpDocument)
		
		/// <summary>This method is called to populate the transcripts collection</summary>
		/// <param name="xpIterator">The XPath iterator containing the transcript nodes</param>
		/// <returns>true if successful</returns>
		public bool GetTranscripts(XPathNodeIterator xpIterator)
		{
			CXmlTranscript	xmlTranscript = null;
			
			try
			{
				//	Make sure we have a valid collection
				Debug.Assert(m_xmlTranscripts != null);
				m_xmlTranscripts.Clear();
					
				//	Add an object for each node
				while(xpIterator.MoveNext() == true)
				{
					xmlTranscript = new CXmlTranscript();
					m_tmaxEventSource.Attach(xmlTranscript.EventSource);
					
					if(xmlTranscript.SetProperties(xpIterator.Current) == true)
						m_xmlTranscripts.Add(xmlTranscript);
						
				}// while(xpIterator.MoveNext() == true)

				m_xmlTranscripts.Sort();
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", "An exception was raised while attempting to populate the segment's transcript text", Ex);
				return false;
			}
			
		}// public bool GetTranscripts(XPathNodeIterator xpIterator)
		
		/// <summary>This method retrieves the default display text for the node</summary>
		/// <returns>The default display text string</returns>
		override public string ToString()
		{
			string strText = "";
			
			if(Filename.Length > 0)
			{
				strText = (Filename + " ");
			}
			
			if(FirstPL >= 0)
			{
				strText += (FirstPL.ToString() + " - " + LastPL.ToString());
			}
			else
			{
				strText += (CTmaxToolbox.SecondsToString(Start) + " - " + CTmaxToolbox.SecondsToString(Stop));
			}
			
			return strText;
			
		}// virtual public string GetDisplayString()
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement  = null;
			XmlNode		xmlChild	= null;
			bool		bSuccessful = false;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_SEGMENT_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_KEY, Key) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_FILENAME, Filename) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_FIRST_PL, FirstPL) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_LAST_PL, LastPL) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_START, DoubleToXml(Start)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_STOP, DoubleToXml(Stop)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_START_TUNED, BoolToXml(StartTuned)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_STOP_TUNED, BoolToXml(StopTuned)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SEGMENT_ATTRIBUTE_FRAMES_PER_SECOND, FramesPerSecond) == false)
						break;
						
					if((Transcripts != null) && (Transcripts.Count > 0))
					{
						foreach(CXmlTranscript O in Transcripts)
						{
							if((xmlChild = O.ToXmlNode(xmlDocument)) != null)
							{
								xmlElement.AppendChild(xmlChild);
							}
						}
					}
					
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
		#region Properties
		
		//	Collection of transcript text associated with the segment
		public CXmlTranscripts Transcripts
		{
			get{ return m_xmlTranscripts; }
		}
		
		//	XML key identifier assigned to the deposition node
		public string Key
		{
			get{ return m_strKey; }
			set{ m_strKey = value; }
		}
		
		//	Name of the file associated with the segment
		public string Filename
		{
			get{ return m_strFilename; }
			set{ m_strFilename = value; }
		}
		
		//	First Page/Line of the segment
		public long FirstPL
		{
			get{ return m_lFirstPL; }
			set{ m_lFirstPL = value; }
		}
		
		//	Last Page/Line of the segment
		public long LastPL
		{
			get{ return m_lLastPL; }
			set{ m_lLastPL = value; }
		}
		
		//	Start frame / segment
		public double Start
		{
			get{ return m_dStart; }
			set{ m_dStart = value; }
		}
		
		//	Stop frame / segment
		public double Stop
		{
			get{ return m_dStop; }
			set{ m_dStop = value; }
		}
		
		//	Start position is tuned
		public bool StartTuned
		{
			get{ return m_bStartTuned; }
			set{ m_bStartTuned = value; }
		}
		
		//	Stop position is tuned
		public bool StopTuned
		{
			get{ return m_bStopTuned; }
			set{ m_bStopTuned = value; }
		}
		
		//	Frames per second
		public float FramesPerSecond
		{
			get{ return m_fFramesPerSecond; }
			set{ m_fFramesPerSecond = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlSegment

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CXmlSegment objects
	/// </summary>
	public class CXmlSegments : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlSegments() : base()
		{
			//	Assign a default files sorter
			//base.Comparer = new CxmlSegmentsSorter();
			//((CxmlSegmentsSorter)base.Comparer).Ascending = true;
			//((CxmlSegmentsSorter)base.Comparer).CaseSensitive = false;
			
			this.KeepSorted = false;
		}

		/// <summary>Overloaded constructor</summary>
//		public CXmlSegments(CxmlSegmentsSorter tmaxSorter) : base(tmaxSorter as IComparer)
//		{
//			//((CxmlSegmentsSorter)base.Comparer).Ascending = true;
//			//((CxmlSegmentsSorter)base.Comparer).CaseSensitive = false;
//			
//			this.AllowDuplicates = true;
//			this.KeepSorted = false;
//		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="xmlSegment">CXmlSegment object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlSegment Add(CXmlSegment xmlSegment)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlSegment as object);

				return xmlSegment;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlSegment xmlSegment)

		/// <summary>
		/// This method is called to remove the requested filter from the collection
		/// </summary>
		/// <param name="xmlSegment">The filter object to be removed</param>
		public void Remove(CXmlSegment xmlSegment)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlSegment as object);
			}
			catch
			{
			}
		}

		/// <summary>Called to locate the object with the specified key</summary>
		/// <returns>The object with the specified key</returns>
		public CXmlSegment Locate(string strKey)
		{
			// Search for the object with the same name
			foreach(CXmlSegment O in this)
			{
				if(String.Compare(O.Key, strKey, true) == 0)
				{
					return O;
				}
			}
			return null;

		}//	Locate(string strKey)

		/// <summary>
		/// This method is called to clear the collection
		/// </summary>
		public override void Clear()
		{
			//	Clear the transcripts that belong to each segment
			foreach(CXmlSegment O in this)
			{
				O.Clear();
			}
			base.Clear();
		}

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="xmlSegment">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlSegment xmlSegment)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlSegment as object);
		}

		/// <summary>
		/// Overloaded version of [] operator to return the object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CXmlSegment this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlSegment);
			}
		}

		/// <summary>
		/// Overloaded version of [] operator to return the object with the specified key
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public CXmlSegment this[string strKey]
		{
			get 
			{ 
				return Locate(strKey);
			}
		}

		/// <summary>
		/// This method is called to retrieve the index of the specified object
		/// </summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlSegment value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CXmlSegments
		
}// namespace FTI.Shared.Xml
