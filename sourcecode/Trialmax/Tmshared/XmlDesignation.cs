using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition designation</summary>
	public class CXmlDesignation : CXmlFile
	{
		#region Constants
		
		public const int XML_DESIGNATION_SORT_ORDER		= 0;
		public const int XML_DESIGNATION_SORT_POSITION	= 1;
		
		public const string XML_DESIGNATION_ROOT_NAME    = "trialMax";
		public const string XML_DESIGNATION_ELEMENT_NAME = "designation";

		public const string XML_DESIGNATION_ATTRIBUTE_NAME			= "name";
		public const string XML_DESIGNATION_ATTRIBUTE_PRIMARY_ID	= "primaryId";
		public const string XML_DESIGNATION_ATTRIBUTE_SEGMENT		= "segment";
		public const string XML_DESIGNATION_ATTRIBUTE_START			= "start";
		public const string XML_DESIGNATION_ATTRIBUTE_STOP			= "stop";
		public const string XML_DESIGNATION_ATTRIBUTE_START_TUNED	= "startTuned";
		public const string XML_DESIGNATION_ATTRIBUTE_STOP_TUNED	= "stopTuned";
		public const string XML_DESIGNATION_ATTRIBUTE_HAS_TEXT		= "hasText";
		public const string XML_DESIGNATION_ATTRIBUTE_SCROLL_TEXT	= "scrollText";
		public const string XML_DESIGNATION_ATTRIBUTE_FIRST_PL		= "firstPL";
		public const string XML_DESIGNATION_ATTRIBUTE_LAST_PL		= "lastPL";
		public const string XML_DESIGNATION_ATTRIBUTE_CREATED_BY	= "createdBy";
		public const string XML_DESIGNATION_ATTRIBUTE_CREATED_ON	= "createdOn";
		public const string XML_DESIGNATION_ATTRIBUTE_MODIFIED_BY	= "modifiedBy";
		public const string XML_DESIGNATION_ATTRIBUTE_MODIFIED_ON	= "modifiedOn";
		public const string XML_DESIGNATION_ATTRIBUTE_HIGHLIGHTER	= "highlighter";

		protected const int ERROR_OPEN_DESIGNATION_EX	= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_NO_DESIGNATION_NODE	= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX		= (LAST_XML_FILE_ERROR + 3);
		protected const int	ERROR_FAST_FILL_EX			= (LAST_XML_FILE_ERROR + 4);
		protected const int	ERROR_GET_TRANSCRIPTS_EX	= (LAST_XML_FILE_ERROR + 5);
		protected const int	ERROR_GET_LINKS_EX			= (LAST_XML_FILE_ERROR + 6);
		
		#endregion Constants
		
		#region Protected Members

		/// <summary>Local member bound to Transcripts property</summary>
		protected CXmlTranscripts m_xmlTranscripts = new CXmlTranscripts();

		/// <summary>Local member bound to Links property</summary>
		protected CXmlLinks m_xmlLinks = new CXmlLinks();

		/// <summary>This member is bounded to the Name property</summary>
		protected string m_strName = "";		
		
		/// <summary>This member is bounded to the PrimaryId property</summary>
		protected string m_strPrimaryId = "";		
		
		/// <summary>This member is bounded to the Segment property</summary>
		protected string m_strSegment = "";		
		
		/// <summary>This member is bounded to the FirstPL property</summary>
		protected long m_lFirstPL = 0;		
		
		/// <summary>This member is bounded to the LastPl property</summary>
		protected long m_lLastPL = 0;		
		
		/// <summary>This member is bounded to the Start property</summary>
		protected double m_dStart = 0;
		
		/// <summary>This member is bounded to the Stop property</summary>
		protected double m_dStop = 0;
		
		/// <summary>This member is bounded to the StartTuned property</summary>
		protected bool m_bStartTuned = false;
		
		/// <summary>This member is bounded to the StopTuned property</summary>
		protected bool m_bStopTuned = false;
		
		/// <summary>This member is bounded to the HasText property</summary>
		protected bool m_bHasText = true;
		
		/// <summary>This member is bounded to the ScrollText property</summary>
		protected bool m_bScrollText = true;
		
		/// <summary>This member is bounded to the CreatedBy property</summary>
		protected string m_strCreatedBy = "";
		
		/// <summary>This member is bounded to the CreatedOn property</summary>
		protected string m_strCreatedOn = "";
		
		/// <summary>This member is bounded to the ModifiedBy property</summary>
		protected string m_strModifiedBy = "";
		
		/// <summary>This member is bounded to the ModifiedOn property</summary>
		protected string m_strModifiedOn = "";
		
		/// <summary>This member is bounded to the Highlighter property</summary>
		protected int m_iHighlighter = 0;
		
		/// <summary>This member is bounded to the XmlSortOrder property</summary>
		protected int m_iXmlSortOrder = 0;
		
		/// <summary>This member is bounded to the XmlCase property</summary>
		protected CXmlCase m_xmlCase = null;
		
		/// <summary>This member is bounded to the Recording property</summary>
		protected string m_strRecording = "";		
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlDesignation() : base()
		{
			m_strRoot = XML_DESIGNATION_ROOT_NAME;
			Extension = GetExtension();
			
			m_xmlTranscripts.Comparer = new CXmlBaseSorter();
			m_xmlTranscripts.KeepSorted = false;
			
			m_xmlLinks.Comparer = m_xmlTranscripts.Comparer;
			m_xmlLinks.KeepSorted = false;
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source node to be copied</param>
		public CXmlDesignation(CXmlDesignation xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
					
			if(xmlSource != null)
				Copy(xmlSource);
		}
		
		/// <summary>Called to copy the properties of the source node</summary>
		///	<param name="xmlSource">the source node to be copied</param>
		public void Copy(CXmlDesignation xmlSource)
		{
			Copy(xmlSource, true, true);
		}
			
		/// <summary>Called to copy the properties of the source node</summary>
		///	<param name="xmlSource">the source node to be copied</param>
		/// <param name="bTranscripts">true to copy the transcripts</param>
		/// <param name="bLinks">true to copy the links</param>
		public void Copy(CXmlDesignation xmlSource, bool bTranscripts, bool bLinks)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
							
			PrimaryId = xmlSource.PrimaryId;
			Segment = xmlSource.Segment;
			FirstPL = xmlSource.FirstPL;
			HasText = xmlSource.HasText;
			Highlighter = xmlSource.Highlighter;
			LastPL = xmlSource.LastPL;
			Name = xmlSource.Name;
			ScrollText = xmlSource.ScrollText;
			Start = xmlSource.Start;
			StartTuned = xmlSource.StartTuned;
			Stop = xmlSource.Stop;
			StopTuned = xmlSource.StopTuned;
			XmlSortOrder = xmlSource.XmlSortOrder;
			CreatedBy = xmlSource.CreatedBy;
			CreatedOn = xmlSource.CreatedOn;
			ModifiedBy = xmlSource.ModifiedBy;
			ModifiedOn = xmlSource.ModifiedOn;
					
			//	Copy the case information
			if(xmlSource.Case != null)
			{
				if(m_xmlCase != null)
					m_xmlCase.Copy(xmlSource.Case);
				else
					m_xmlCase = new CXmlCase(xmlSource.Case);
			}
			else
			{
				m_xmlCase = null;
			}
			
			//	Should we copy the transcript nodes?
			if(bTranscripts == true)
			{
				m_xmlTranscripts.Clear();
								
				if(xmlSource.Transcripts != null)
				{
					foreach(CXmlTranscript O in xmlSource.Transcripts)
						m_xmlTranscripts.Add(new CXmlTranscript(O));
				}
						
			}
					
			//	Should we copy the link nodes?
			if(bLinks == true)
			{
				m_xmlLinks.Clear();
								
				if(xmlSource.Links != null)
				{
					foreach(CXmlLink O in xmlSource.Links)
						m_xmlLinks.Add(new CXmlLink(O));
				}
						
			}
					
		}// public void Copy(CXmlTranscript xmlSource)
		
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				if(iMode == XML_DESIGNATION_SORT_POSITION)
				{
					if(((CXmlDesignation)(xmlBase)).FirstPL == this.FirstPL)
					{
						if(((CXmlDesignation)(xmlBase)).Start == this.Start)
							return 0;
						else
							return (((CXmlDesignation)(xmlBase)).Start > this.Start) ? -1 : 1;
					
					}
					else
					{
						return (((CXmlDesignation)(xmlBase)).FirstPL > this.FirstPL) ? -1 : 1;
					}
					
				}
				else
				{
					if(((CXmlDesignation)(xmlBase)).XmlSortOrder == m_iXmlSortOrder)
						return 0;
					else
						return (((CXmlDesignation)(xmlBase)).XmlSortOrder > m_iXmlSortOrder) ? -1 : 1;
			
				}
				
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to edit the extents of this node using the specified source node</summary>
		///	<param name="xmlSource">the designation source node</param>
		public bool EditExtents(CXmlDesignation xmlSource)
		{
			CXmlTranscripts xmlHolding = new CXmlTranscripts();
			int				iIndex = -1;
			
			//	Has the start PL changed?
			if(FirstPL != xmlSource.FirstPL)
			{
				FirstPL = xmlSource.FirstPL;
				Start = xmlSource.Start;
				StartTuned = false;
			}
			
			//	Has the last PL changed?
			if(LastPL != xmlSource.LastPL)
			{
				LastPL = xmlSource.LastPL;
				Stop = xmlSource.Stop;
				StopTuned = false;
			}
			
			//	Transfer all our transcript text to a temporary collection
			Debug.Assert(m_xmlTranscripts != null);
			foreach(CXmlTranscript O in m_xmlTranscripts)
				xmlHolding.Add(O);
			m_xmlTranscripts.Clear();
			
			//	Now rebuild the transcript text using the source
			Debug.Assert(xmlSource.Transcripts != null);
			foreach(CXmlTranscript O in xmlSource.Transcripts)
			{
				//	Does this line exist in our holding collection?
				if((iIndex = xmlHolding.Locate(iIndex, O.PL)) >= 0)
				{
					//	Put back in our local collection
					m_xmlTranscripts.Add(xmlHolding[iIndex]);
					xmlHolding.RemoveAt(iIndex);
				}
				else
				{
					//	Add a copy to our local collection
					m_xmlTranscripts.Add(new CXmlTranscript(O));
				}
			
			}
			
			//	Make sure we are sorted
			m_xmlTranscripts.Sort(true);
						
			//	Clear the holding collection
			xmlHolding.Clear();
			
			return true;
				
		}// public void EditExtents(CXmlDesignation xmlSource)
		
		/// <summary>This method is called to get the filter string used to initialize a file selection form</summary>
		/// <param name="bAllowAll">true to allow All Files option</param>
		/// <returns>The appropriate filter string</returns>
		new static public string GetFilter(bool bAllowAll)
		{
			if(bAllowAll == true)
				return "Designations (*.xmld)|*.xmld|All Files (*.*)|*.*";
			else
				return "Designations (*.xmld)|*.xmld";
		}
		
		/// <summary>This method is called to get the default file extension</summary>
		/// <returns>The default file extension</returns>
		new static public string GetExtension()
		{
			return "xmld";
		}
		
		/// <summary>This method retrieves the default display text for the node</summary>
		/// <returns>The default display text string</returns>
		override public string GetDisplayString()
		{
			return this.Name;
			
		}// override public string GetDisplayString()
		
		/// <summary>Called to check to see if the transcript lines in the designation are synchronized</summary>
		/// <param name="bAll">true if all lines in the designation must be synchronized for the designation to be synchronized</param>
		/// <returns>true if the designation is synchronized</returns>
		public bool GetSynchronized(bool bAll)
		{
			if(m_xmlTranscripts != null)
				return m_xmlTranscripts.GetSynchronized(bAll);
			else
				return false;
		
		}// public bool GetSynchronized(bool bAll)
		
		/// <summary>Called to get the playback time for the designation</summary>
		/// <returns>The total playback time</returns>
		public double GetDuration()
		{
			double dSeconds = 0;
			
			if((dSeconds = this.Stop - this.Start) < 0)
				dSeconds = 0;
				
			return dSeconds;
		
		}// public double GetDuration()
		
		/// <summary>Called to get the time required to play the specified line</summary>
		/// <param name="lPL">the packed page/line identifier for the desired line</param>
		/// <returns>the player time for the line</returns>
		public double GetPlayTime(long lPL)
		{
			int		iIndex = -1;
			double	dTime = -1;
			
			if(m_xmlTranscripts != null)
			{
				if((iIndex = m_xmlTranscripts.Locate(lPL, true)) >= 0)
					dTime = (m_xmlTranscripts[iIndex].Stop - m_xmlTranscripts[iIndex].Start);
			}
			
			return dTime;
		
		}// public double GetPlayTime(long lPL)

		/// <summary>This method is called to get the position defined by the PL value</summary>
		/// <returns>The position at which the page/line occurs</returns>
		public double GetPosition(long lPL)
		{
			double dPosition = -1;
			int i;

			if(m_xmlTranscripts != null)
			{
				for(i = 0; i < m_xmlTranscripts.Count; i++)
				{
					if(lPL <= m_xmlTranscripts[i].PL)
					{
						dPosition = m_xmlTranscripts[i].Start;
						break;
					}
				}

				//	Did we run out of lines?
				if((i > 0) && (i == m_xmlTranscripts.Count))
				{
					dPosition = m_xmlTranscripts[i - 1].Start;
				}

			}

			return dPosition;

		}// public double GetPosition(long lPL)

		/// <summary>Called to get the time required to play the longest line in the trascript</summary>
		/// <param name="lLastInSegment">the packed page/line identifier for the last line in the parent segment</param>
		/// <returns>the player time for the line</returns>
		public double GetMaxLineTime()
		{
			int		iStartIndex = -1;
			int		iStopIndex = -1;
			double	dTime = -1;
			double	dMaxTime = -1;
			
			//	Do we have any transcripts?
			if(m_xmlTranscripts == null) return -1;
			if(m_xmlTranscripts.Count == 0) return -1;
			
			//	Is there only one line?
			if(m_xmlTranscripts.Count == 1)
			{
				dMaxTime = (this.Stop - this.Start);
			}
			else
			{
				if(this.StartTuned == true)
					iStartIndex = 1;
				else
					iStartIndex = 0;
					
				if(this.StopTuned == true)
					iStopIndex = m_xmlTranscripts.Count - 2;
				else
					iStopIndex = m_xmlTranscripts.Count - 1;
				
				for(int i = iStartIndex; i <= iStopIndex; i++)
				{
					if((dTime = (m_xmlTranscripts[i].Stop - m_xmlTranscripts[i].Start)) > dMaxTime)
						dMaxTime = dTime;
				}
			
			}
			
			return dMaxTime;
		
		}// public double GetMaxLineTime()
		
		/// <summary>Called to clear the designation's child collections</summary>
		public void Clear()
		{
			m_xmlCase = null;
			
			if(m_xmlTranscripts != null)
			{
				m_xmlTranscripts.Clear();
			}
			
			if(m_xmlLinks != null)
			{
				m_xmlLinks.Clear();
			}
			
		}// public void Clear()
		
		/// <summary>This method is called to close the XML file associated with the deposition</summary>
		/// <param name="bClear">true to clear the local collections</param>
		public void Close(bool bClear)
		{
			//	Do the base class processing
			base.Close();
			
			//	Should we clear the collections?
			if(bClear == true)
				Clear();
		}
			
		/// <summary>This method is called to get open the specified file</summary>
		/// <param name="bCreate">true to create the file if it doesn't exist</param>
		/// <returns>true if successful</returns>
		public override bool Open(bool bCreate)
		{
			XmlNode xmlNode = null;
			bool	bSuccessful = false;
		
			//	Do the base class processing first
			if(base.Open(bCreate) == false) return false;
			
			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlRoot != null);
			
			try
			{
				while(bSuccessful == false)
				{
					//	Get the designation descriptor node
					if((xmlNode = m_xmlRoot.SelectSingleNode(XML_DESIGNATION_ELEMENT_NAME)) == null)
					{
						//	Is this a new file?
						if(bCreate == true)
						{
							//	We don't expect the designation node to 
							//	appear in a new file
							bSuccessful = true;
						}
						else
						{
							m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_DESIGNATION_NODE, GetFileSpec()));
						}
						break;
					}
					
					//	Get the designation properties
					if(SetProperties(xmlNode) == false)
						break;
						
					//	We're done 
					bSuccessful = true;
					
				}//	while(bSuccessful == false)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_DESIGNATION_EX, GetFileSpec()), Ex);
			}
			
			if(bSuccessful == false)
			{
				Close(true);
				return false;
			}
			else
			{
				return true;
			}
			
		}//	Open(bool bCreate)
		
		/// <summary>This method is called to use an XPath document to initialize the object</summary>
		/// <param name="xpDocument">The XPath document containing the object's XML node</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <param name="bLinks">true to fill the links collection</param>
		/// <param name="bTranscripts">true to fill the transcripts collection</param>
		/// <returns>true if successful</returns>
		public bool FastFill(XPathDocument xpDocument, string strParentPath, bool bLinks, bool bTranscripts)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			string				strXPath	= "";

			try
			{
				//	Build the path to the object's XML node
				if((strParentPath == null) || (strParentPath.Length == 0))
					strXPath = String.Format("{0}/{1}", XML_DESIGNATION_ROOT_NAME, XML_DESIGNATION_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}", strParentPath, XML_DESIGNATION_ELEMENT_NAME);
				
				//	Get the object node
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				if(xpIterator.Count > 0)
				{
					xpIterator.MoveNext();
					SetProperties(xpIterator.Current);
				}
				else
				{
					m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_NO_DESIGNATION_NODE, m_strFileSpec, strXPath));
					return false;
				}
				
				//	Does this file have a case node?
				if((xpIterator = xpNavigator.Select("trialMax/case")) == null) return false;
				if(xpIterator.Count > 0)
				{
					xpIterator.MoveNext();
					
					if(m_xmlCase == null)
						m_xmlCase = new CXmlCase();
						
					m_xmlCase.SetProperties(xpIterator.Current);
				}
				
				//	Get the links
				if(bLinks == true)
					GetLinks(xpDocument, strXPath);

				//	Get the transcript text
				if(bTranscripts == true)
					GetTranscripts(xpDocument, strXPath);

				return true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FAST_FILL_EX, GetFileSpec()), Ex);
			}
			
			//	Must have been a problem
			return false;
		}
		
		/// <summary>This method is called to use the specified XML file to intialize the object</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <param name="bLinks">true to fill the links collection</param>
		/// <param name="bTranscripts">true to fill the transcripts collection</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec, string strParentPath, bool bLinks, bool bTranscripts)
		{
			XPathDocument xpDocument = null;

			//	Make sure the file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FILE_NOT_FOUND, GetFileSpec()));
				return false;
			}

			try
			{
				//	Load the file
				xpDocument = new System.Xml.XPath.XPathDocument(strFileSpec);
				
				//	Update the base class file properties
				//
				//	NOTE:	This MUST be done in order to do subsequent calls
				//			to Save()
				SetFileProps(strFileSpec);
				
				//	Initialize the object
				return FastFill(xpDocument, strParentPath, bLinks, bTranscripts);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FAST_FILL_EX, GetFileSpec()), Ex);
			}
			
			//	Must have been a problem
			return false;
		}
		
		/// <summary>This method is called to use the specified XML file to intialize the object</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="bLinks">true to fill the links collection</param>
		/// <param name="bTranscripts">true to fill the transcripts collection</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec, bool bLinks, bool bTranscripts)
		{
			return FastFill(strFileSpec, "", bLinks, bTranscripts);
		}
		
		/// <summary>This method will set the designation properties using the specified node</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the designation properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_PRIMARY_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strPrimaryId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_SEGMENT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strSegment = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_FIRST_PL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lFirstPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_LAST_PL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_lLastPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_START,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_dStart = System.Convert.ToDouble(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_STOP,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_dStop = System.Convert.ToDouble(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_START_TUNED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bStartTuned = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_STOP_TUNED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bStopTuned = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_HAS_TEXT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bHasText = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_SCROLL_TEXT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bScrollText = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_CREATED_BY,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCreatedBy = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_CREATED_ON,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCreatedOn = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_MODIFIED_BY,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strModifiedBy = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_MODIFIED_ON,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strModifiedOn = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DESIGNATION_ATTRIBUTE_HIGHLIGHTER,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_iHighlighter = System.Convert.ToInt32(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method is called to set the start/stop extents using the contents of the transcripts collection</summary>
		/// <returns>true if successful</returns>
		public bool SetExtents()
		{
			//	Do we have any transcripts?
			if((m_xmlTranscripts == null) || (m_xmlTranscripts.Count == 0))
			{
				Start = -1;
				FirstPL = -1;
				LastPL = -1;
				Stop = -1;
			}
			else
			{
				//	Make sure the transcripts are in sorted order
				m_xmlTranscripts.Sort();
				
				Start = m_xmlTranscripts[0].Start;
				FirstPL = m_xmlTranscripts[0].PL;
				Stop = m_xmlTranscripts[m_xmlTranscripts.Count - 1].Stop;
				LastPL = m_xmlTranscripts[m_xmlTranscripts.Count - 1].PL;
				
				//	Do we have an invalid stop time?
				//
				//	NOTE:	This happens sometimes if the last line of text is
				//			where the segment breaks
				if(Stop <= 0)
					Stop = m_xmlTranscripts[m_xmlTranscripts.Count - 1].Start + XMLBASE_DEFAULT_TRANSCRIPT_TIME;

			}
			
			return true;
				
		}// public bool SetExtents()
		
		/// <summary>This method is called to split this designation at the specified position</summary>
		/// <param name="lPL">The position at which to perform the split</param>
		/// <param name="bIncludeFirst">True to include lPL in the first portion</param>
		/// <param name="xmlDeposition">Source deposition required for updating the designation names</param>
		/// <returns>A designation containing the text that appears after the specified position</returns>
		/// <remarks>The specified line is included in the first half</remarks>
		public CXmlDesignation Split(long lPL, bool bIncludeFirst, CXmlDeposition xmlDeposition)
		{
			CXmlDesignation xmlSecond = null;
			
			//	Do we have anything to split?
			if(bIncludeFirst == true)
			{
				if(lPL >= LastPL) return null;
			}
			else
			{
				if(lPL > LastPL) return null;
			}
		
			//	Do we have any transcripts?
			if(m_xmlTranscripts == null) return null;
			if(m_xmlTranscripts.Count == 0) return null;

			//	Allocate a designation for the second half
			xmlSecond = new CXmlDesignation();
			
			//	Copy this object except for transcripts and links
			xmlSecond.Copy(this, false, false);
			
			//	Transfer the text to the second half
			foreach(CXmlTranscript O in m_xmlTranscripts)
			{
				if(bIncludeFirst == true)
				{
					if(O.PL > lPL)
						xmlSecond.Transcripts.Add(O);
				}
				else
				{
					if(O.PL >= lPL)
						xmlSecond.Transcripts.Add(O);
				}
				
			}// foreach(CXmlTranscript O in m_xmlTranscripts)
			
			//	Remove the text from the first half
			foreach(CXmlTranscript O in xmlSecond.Transcripts)
			{
				m_xmlTranscripts.Remove(O);
			}
			
			//	Transfer the links to the second half
			foreach(CXmlLink O in m_xmlLinks)
			{
				if(bIncludeFirst == true)
				{
					if(O.PL > lPL)
						xmlSecond.Links.Add(O);
				}
				else
				{
					if(O.PL >= lPL)
						xmlSecond.Links.Add(O);
				}
				
			}// foreach(CXmlLink O in m_xmlLinks)
			
			//	Remove the links from the first half
			foreach(CXmlLink O in xmlSecond.Links)
			{
				m_xmlLinks.Remove(O);
			}
			
			//	Make sure the transcripts are sorted
			m_xmlTranscripts.Sort(true);
			xmlSecond.Transcripts.Sort(true);
			
			//	Update the extents
			if(xmlSecond.Transcripts.Count > 0)
			{
				//	Set the extents of the new designation
				xmlSecond.SetExtents();
				xmlSecond.StartTuned = false;
				if(this.StopTuned == true)
				{
					xmlSecond.Stop = this.Stop;
					xmlSecond.StopTuned = true;
				}
				
				//	Only the stop position should have changed for this designation
				if(m_xmlTranscripts.Count > 0)
				{
					Stop = m_xmlTranscripts[m_xmlTranscripts.Count - 1].Stop;
					LastPL = m_xmlTranscripts[m_xmlTranscripts.Count - 1].PL;
					if(Stop <= 0)
						Stop = m_xmlTranscripts[m_xmlTranscripts.Count - 1].Start + XMLBASE_DEFAULT_TRANSCRIPT_TIME;
					StopTuned = false;
				}
				else
				{
					SetExtents();
				}
				
				//	Update the names if provided with the parent deposition
				if(xmlDeposition != null)
				{
					this.SetNameFromExtents(xmlDeposition);
					xmlSecond.SetNameFromExtents(xmlDeposition);
				}
				
				
			}// if(xmlSecond.Transcripts.Count > 0)
			
			return xmlSecond;
				
		}// public CXmlDesignation Split(long lPL)
		
		/// <summary>This method is called to populate the transcripts collection</summary>
		/// <param name="xpDocument">The XPath document containing the transcript nodes</param>
		/// <param name="strDesignationPath">The path to the parent designation node</param>
		/// <returns>true if successful</returns>
		public bool GetTranscripts(XPathDocument xpDocument, string strDesignationPath)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			string				strXPath = "";
			
			try
			{
				//	Clear the collection
				m_xmlTranscripts.Clear();
					
				//	Construct the path to use for the XPath query
				if((strDesignationPath != null) && (strDesignationPath.Length > 0))
					strXPath = String.Format("{0}/{1}", strDesignationPath, CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}/{2}", XML_DESIGNATION_ROOT_NAME, XML_DESIGNATION_ELEMENT_NAME, CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME);

				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
			
				return GetTranscripts(xpIterator);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", m_tmaxErrorBuilder.Message(ERROR_GET_TRANSCRIPTS_EX, GetFileSpec()), Ex);
				return false;
			}
			
		}// public bool GetTranscripts(XPathDocument xpDocument)
		
		/// <summary>This method is called to populate the transcripts collection</summary>
		/// <param name="xpIterator">The iterator for a collection of transcript nodes returned by an XPath query</param>
		/// <returns>true if successful</returns>
		public bool GetTranscripts(XPathNodeIterator xpIterator)
		{
			CXmlTranscript xmlTranscript = null;
			
			try
			{
				//	Clear the collection
				m_xmlTranscripts.Clear();
					
				//	Add an object for each node
				while(xpIterator.MoveNext() == true)
				{
					xmlTranscript = new CXmlTranscript();
					xmlTranscript.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
					xmlTranscript.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
					
					if(xmlTranscript.SetProperties(xpIterator.Current) == true)
						m_xmlTranscripts.Add(xmlTranscript);
						
				}// while(xpIterator.MoveNext() == true)

				//	Make sure everything is properly sorted
				m_xmlTranscripts.Sort(true);
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", m_tmaxErrorBuilder.Message(ERROR_GET_TRANSCRIPTS_EX, GetFileSpec()), Ex);
				return false;
			}
			
		}// public bool GetTranscripts(XPathNodeIterator xpIterator)
		
		/// <summary>This method is called to populate the links collection</summary>
		/// <param name="xpDocument">The XPath document containing the link nodes</param>
		/// <param name="strDesignationPath">The path to the parent designation node</param>
		/// <returns>true if successful</returns>
		public bool GetLinks(XPathDocument xpDocument, string strDesignationPath)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			string				strXPath = "";
			
			try
			{
				//	Clear the collection
				m_xmlLinks.Clear();
					
				//	Construct the path to use for the XPath query
				if((strDesignationPath != null) && (strDesignationPath.Length > 0))
					strXPath = String.Format("{0}/{1}", strDesignationPath, CXmlLink.XML_LINK_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}/{2}", XML_DESIGNATION_ROOT_NAME, XML_DESIGNATION_ELEMENT_NAME, CXmlLink.XML_LINK_ELEMENT_NAME);
				
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				
				return GetLinks(xpIterator);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetLinks", m_tmaxErrorBuilder.Message(ERROR_GET_LINKS_EX, GetFileSpec()), Ex);
				return false;
			}
			
		}// public bool GetLinks(XPathDocument xpDocument, string strDesignationPath)
		
		/// <summary>This method is called to populate the links collection</summary>
		/// <param name="xpIterator">The iterator for a collection of link nodes returned by an XPath query</param>
		/// <returns>true if successful</returns>
		public bool GetLinks(XPathNodeIterator xpIterator)
		{
			CXmlLink xmlLink = null;
			
			try
			{
				//	Clear the collection
				m_xmlLinks.Clear();
					
				//	Add an object for each node
				while(xpIterator.MoveNext() == true)
				{
					xmlLink = new CXmlLink();
					xmlLink.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
					xmlLink.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
					
					if(xmlLink.SetProperties(xpIterator.Current) == true)
						m_xmlLinks.Add(xmlLink);
						
				}// while(xpIterator.MoveNext() == true)

				//	Now make sure they are properly sorted
				if(m_xmlLinks.Count > 0)
					m_xmlLinks.Sort();
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetLinks", m_tmaxErrorBuilder.Message(ERROR_GET_LINKS_EX, GetFileSpec()), Ex);
				return false;
			}
			
		}// public bool GetLinks(XPathNodeIterator xpIterator)
		
		/// <summary>Called to determine if all links are within the playback range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <param name="xmlOutOfRange">The collection in which to store links that are out of range</param>
		/// <returns>true if all in range</returns>
		public bool CheckLinkPositions(double dStart, double dStop, CXmlLinks xmlOutOfRange)
		{
			bool bInRange = true;
			
			if((m_xmlLinks != null) && (m_xmlLinks.Count > 0))
				bInRange = m_xmlLinks.CheckRange(dStart, dStop, xmlOutOfRange);
			
			return bInRange;
			
		}// public bool CheckLinkPositions(CXmlLinks xmlOutOfRange)
		
		/// <summary>Called to determine if all links are within the playback range</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <returns>true if all in range</returns>
		public bool CheckLinkPositions(double dStart, double dStop)
		{
			return CheckLinkPositions(dStart, dStop, null);
		}
		
		/// <summary>Called to determine if all links are within the playback range</summary>
		/// <param name="xmlOutOfRange">The collection in which to store links that are out of range</param>
		/// <returns>true if all in range</returns>
		public bool CheckLinkPositions(CXmlLinks xmlOutOfRange)
		{
			return CheckLinkPositions(m_dStart, m_dStop, xmlOutOfRange);
		}
		
		/// <summary>Called to determine if all links are within the playback range</summary>
		/// <returns>true if all in range</returns>
		public bool CheckLinkPositions()
		{
			return CheckLinkPositions(null);
		}
		
		/// <summary>This method is called to save the designation to file</summary>
		/// <returns>The Xml writer object if successful</returns>
		public override bool Save()
		{
			XmlTextWriter	xmlWriter = null;
			string			strBackUp = "";
			XmlNode			xmlChild = null;
			
			//	Construct the full path specification
			GetFileSpec();
			if(m_strFileSpec.Length == 0) return false;

			//	Should we back up the existing file?
			if(System.IO.File.Exists(m_strFileSpec) == true)
			{
				strBackUp = System.IO.Path.ChangeExtension(m_strFileSpec, "_xml_");
				try
				{
					if(System.IO.File.Exists(strBackUp) == true)
						System.IO.File.Delete(strBackUp);
						
					System.IO.File.Move(m_strFileSpec, strBackUp);
				}
				catch
				{
					strBackUp = "";
				}
			}
			
			try
			{

				//	Open the file
				if(Open(true) == true)
				{
					Debug.Assert(m_xmlDocument != null);
					Debug.Assert(m_xmlRoot != null);
					
					//	Do we have a case node?
					if(m_xmlCase != null)
					{
						if((xmlChild = m_xmlCase.ToXmlNode(m_xmlDocument)) != null)
							m_xmlRoot.AppendChild(xmlChild);
					}
			
					//	Get the node for the designation itself
					if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					{
						m_xmlRoot.AppendChild(xmlChild);
						
						if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
						{
							xmlWriter.Formatting = System.Xml.Formatting.Indented;
							xmlWriter.Indentation = 4;
							
							m_xmlDocument.Save(xmlWriter);
							xmlWriter.Close();
							
							//	Close the document without destroying the local collections
							Close(false);
							
							//	Delete the backup
							if(strBackUp.Length > 0)
							{
								try { System.IO.File.Delete(strBackUp); }
								catch { }
							}
							
							return true;
						}
						
					}// if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, GetFileSpec()), Ex);
			}
			
			//	Restore the backup
			if(strBackUp.Length > 0)
			{
				try
				{
					if(System.IO.File.Exists(m_strFileSpec) == true)
						System.IO.File.Delete(m_strFileSpec);
						
					System.IO.File.Move(strBackUp, m_strFileSpec);
				}
				catch
				{
				}
					
			}

			return false;
		
		}//	Save()
		
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
				strElementName = XML_DESIGNATION_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_NAME, Name) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_PRIMARY_ID, PrimaryId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_SEGMENT, Segment) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_START, DoubleToXml(Start)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_STOP, DoubleToXml(Stop)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_START_TUNED, BoolToXml(StartTuned)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_STOP_TUNED, BoolToXml(StopTuned)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_FIRST_PL, FirstPL) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_LAST_PL, LastPL) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_HAS_TEXT, BoolToXml(HasText)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_SCROLL_TEXT, BoolToXml(ScrollText)) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_CREATED_BY, CreatedBy) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_CREATED_ON, CreatedOn) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_MODIFIED_BY, ModifiedBy) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_MODIFIED_ON, ModifiedOn) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DESIGNATION_ATTRIBUTE_HIGHLIGHTER, Highlighter) == false)
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
					
					if((Links != null) && (Links.Count > 0))
					{
						foreach(CXmlLink O in Links)
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
		
		/// <summary>This method will construct the designation name using the specified name and transcript extents</summary>
		/// <param name="strName">The text that preceeds the extents</param>
		public void SetNameFromExtents(string strName)
		{
			//	Initialize
			Name = "";
			
			//	Add the extents
			if((Transcripts != null) && (Transcripts.Count > 0))
			{
				Name += Transcripts[0].Page.ToString();
				Name += ":";
				Name += Transcripts[0].Line.ToString();
				Name += " - ";
				Name += Transcripts[Transcripts.Count - 1].Page.ToString();
				Name += ":";
				Name += Transcripts[Transcripts.Count - 1].Line.ToString();
			}
			else
			{
				//	Use the time positions
				Name += CTmaxToolbox.SecondsToString(Start, 0);
				Name += " - ";
				Name += CTmaxToolbox.SecondsToString(Stop, 0);
			}
		
			if((strName != null) && (strName.Length > 0))
			{
				Name += (" " + strName);
			}
						
		}// public void SetNameFromExtents(string strName)
		
		/// <summary>This method will construct the designation name using the specified values and transcript extents</summary>
		/// <param name="strDeponent">The deponent name</param>
		/// <param name="strDate">The deposition date</param>
		public void SetNameFromExtents(string strDeponent, string strDate)
		{
			string strName = "";
			
			if((strDeponent != null) && (strDeponent.Length > 0))
			{
				strName = strDeponent;
			}
			if((strDate != null) && (strDate.Length > 0))
			{
				if(strName.Length > 0)
					strName += " ";
					
				strName += strDate;
			}
			
			SetNameFromExtents(strName);
		
		}// public void SetNameFromExtents(string strName)
		
		/// <summary>This method will construct the designation name using the specified deposition and transcript extents</summary>
		/// <param name="xmlDeposition">The source deposition</param>
		public void SetNameFromExtents(CXmlDeposition xmlDeposition)
		{
			if(xmlDeposition != null)
			{
				if((xmlDeposition.Name != null) && (xmlDeposition.Name.Length > 0))
					SetNameFromExtents(xmlDeposition.Name);
				else
					SetNameFromExtents(xmlDeposition.Deponent, xmlDeposition.Date);
			}
			else
			{
				SetNameFromExtents("");
			}
		
		}// public void SetNameFromExtents(CXmlDeposition xmlDeposition)

		/// <summary>Called to remove the specified line of transcript text</summary>
		public void RemoveLine(long lPL)
		{
			int iIndex = -1;
			
			if(m_xmlTranscripts != null)
			{
				//	Locate the specified line of text
				if((iIndex = m_xmlTranscripts.Locate(lPL, true)) >= 0)
				{
					m_xmlTranscripts.RemoveAt(iIndex);
				}

			}// if(m_xmlTranscripts != null)

		}// public void RemoveLine(long lPL)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			//	Do the base class processing first
			base.SetErrorStrings();
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to open the designation: filename = %1");
			aStrings.Add("%1 is not a valid XML designation. It does not contain a valid designation node: path = %2");
			aStrings.Add("An exception was raised while attempting to set the designation properties");
			aStrings.Add("An exception was raised while attempting to fast fill the designation: filename = %1");
			aStrings.Add("An exception was raised while attempting to retrieve the designation transcript text: filename = %1");
			aStrings.Add("An exception was raised while attempting to retrieve the designation links: filename = %1");
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		//	MediaId of the primary source
		public string PrimaryId
		{
			get{ return m_strPrimaryId; }
			set{ m_strPrimaryId = value; }
		}
		
		//	Segment identifier associated with this designation
		public string Segment
		{
			get{ return m_strSegment; }
			set{ m_strSegment = value; }
		}
		
		//	Fully qualified path to the recording associated with the designation
		//
		//	NOTE: This is not an attribute
		public string Recording
		{
			get{ return m_strRecording; }
			set{ m_strRecording = value; }
		}
		
		//	Case node associated with this designation
		public CXmlCase Case
		{
			get{ return m_xmlCase; }
			set{ m_xmlCase = value; }
		}
		
		//	Collection of transcript text associated with the designation
		public CXmlTranscripts Transcripts
		{
			get{ return m_xmlTranscripts; }
		}
		
		//	Collection of links associated with the designation
		public CXmlLinks Links
		{
			get{ return m_xmlLinks; }
		}
		
		//	Name of the designation
		public string Name
		{
			get{ return m_strName; }
			set{ m_strName = value; }
		}
		
		//	First Page/Line of the designation
		public long FirstPL
		{
			get{ return m_lFirstPL; }
			set{ m_lFirstPL = value; }
		}
		
		//	Last Page/Line of the designation
		public long LastPL
		{
			get{ return m_lLastPL; }
			set{ m_lLastPL = value; }
		}
		
		//	Start frame / designation
		public double Start
		{
			get{ return m_dStart; }
			set{ m_dStart = value; }
		}
		
		//	Stop frame / designation
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
		
		//	Designation has text 
		public bool HasText
		{
			get{ return m_bHasText; }
			set{ m_bHasText = value; }
		}
		
		//	Scroll text on playback
		public bool ScrollText
		{
			get{ return m_bScrollText; }
			set{ m_bScrollText = value; }
		}
		
		//	Name of the user that created the designation
		public string CreatedBy
		{
			get{ return m_strCreatedBy; }
			set{ m_strCreatedBy = value; }
		}
		
		//	Date the designation was created
		public string CreatedOn
		{
			get{ return m_strCreatedOn; }
			set{ m_strCreatedOn = value; }
		}
		
		//	Name of the user that last modified the designation
		public string ModifiedBy
		{
			get{ return m_strModifiedBy; }
			set{ m_strModifiedBy = value; }
		}
		
		//	Date the designation was last modified
		public string ModifiedOn
		{
			get{ return m_strModifiedOn; }
			set{ m_strModifiedOn = value; }
		}
		
		//	Highlighter assigned to the designation
		public int Highlighter
		{
			get{ return m_iHighlighter; }
			set{ m_iHighlighter = value; }
		}
		
		/// </summary>Value used to keep designations sorted within an XML script object</summary>
		public int XmlSortOrder
		{
			get{ return m_iXmlSortOrder; }
			set{ m_iXmlSortOrder = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlDesignation

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CXmlDesignation objects
	/// </summary>
	public class CXmlDesignations : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlDesignations() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="xmlDesignation">CXmlDesignation object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlDesignation Add(CXmlDesignation xmlDesignation)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlDesignation as object);

				return xmlDesignation;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlDesignation xmlDesignation)

		/// <summary>This method is called to remove the requested filter from the collection</summary>
		/// <param name="xmlDesignation">The object to be removed</param>
		public void Remove(CXmlDesignation xmlDesignation)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlDesignation as object);
			}
			catch
			{
			}
		}

		/// <summary>Called to locate the object with the specified segment id</summary>
		/// <returns>The first object with the specified segment</returns>
		public CXmlDesignation FindFromSegment(string strSegment)
		{
			// Search for the object with the same name
			foreach(CXmlDesignation O in this)
			{
				if(String.Compare(strSegment, O.Segment, true) == 0)
				{
					return O;
				}
			}
			return null;

		}//	FindFromSegment(string strSegment)

		/// <summary>Called to locate the designation that contains the specified line</summary>
		/// <param name="lPL">The composite page/line identifier</param>
		/// <returns>The first object where the specified PL is within the range</returns>
		public CXmlDesignation FindFromPL(long lPL)
		{
			foreach(CXmlDesignation O in this)
			{
				if((lPL >= O.FirstPL) && (lPL <= O.LastPL))
					return O;
			}
			return null;

		}//	public CXmlDesignation FindFromPL(long lPL)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlDesignation">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlDesignation xmlDesignation)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlDesignation as object);
		}

		/// <summary>Called to get the playback time for all designations in the collection</summary>
		/// <returns>The total playback time for all designations</returns>
		public double GetDuration()
		{
			double dSeconds = 0;
			
			// Search for the object with the same name
			foreach(CXmlDesignation O in this)
			{
				dSeconds += O.GetDuration();
			}
				
			return dSeconds;
		
		}// public double GetDuration()
		
		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CXmlDesignation this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlDesignation);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlDesignation value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CXmlDesignations
		
}// namespace FTI.Shared.Xml
