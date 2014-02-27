using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition node</summary>
	public class CXmlDeposition : CXmlFile
	{
		#region Constants
		
		public const string XML_DEPOSITION_ROOT_NAME = "trialmax";
		public const string XML_DEPOSITION_ELEMENT_NAME = "deposition";

		public const string XML_DEPOSITION_ATTRIBUTE_MEDIA_ID		= "mediaId";
		public const string XML_DEPOSITION_ATTRIBUTE_NAME			= "name";
		public const string XML_DEPOSITION_ATTRIBUTE_DEPONENT		= "deponent";
		public const string XML_DEPOSITION_ATTRIBUTE_DATE			= "date";
		public const string XML_DEPOSITION_ATTRIBUTE_LINES_PER_PAGE = "linesPerPage";
		
		protected const int ERROR_OPEN_DEPOSITION_EX				= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_NO_DEPOSITION_NODE				= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX					= (LAST_XML_FILE_ERROR + 3);
		protected const int	ERROR_FAST_FILL_EX						= (LAST_XML_FILE_ERROR + 4);
		protected const int	ERROR_GET_SEGMENTS_EX					= (LAST_XML_FILE_ERROR + 5);
		protected const int	ERROR_GET_TRANSCRIPTS_EX				= (LAST_XML_FILE_ERROR + 6);
		protected const int	ERROR_CREATE_DESIGNATIONS_EX			= (LAST_XML_FILE_ERROR + 7);
		protected const int ERROR_GET_DESIGNATION_TRANSCRIPTS_EX	= (LAST_XML_FILE_ERROR + 8);
		protected const int ERROR_GET_CASE_OBJECTIONS_EX			= (LAST_XML_FILE_ERROR + 9);
		protected const int ERROR_ADD_OBJECTION_EX					= (LAST_XML_FILE_ERROR + 10);
		protected const int ERROR_ADD_OBJECTIONS_EX					= (LAST_XML_FILE_ERROR + 11);
		protected const int ERROR_GET_OBJECTIONS_EX					= (LAST_XML_FILE_ERROR + 12);
		
		#endregion Constants
		
		#region Protected Members

		/// <summary>Local member bound to Segments property</summary>
		protected CXmlSegments m_xmlSegments = new CXmlSegments();

		/// <summary>Local member bound to Transcripts property</summary>
		protected CXmlTranscripts m_xmlTranscripts = new CXmlTranscripts();

		/// <summary>Local member bound to Objections property</summary>
		protected ArrayList m_aObjections = new ArrayList();

		/// <summary>This member is bounded to the Key property</summary>
		protected string m_strMediaId = "";		
		
		/// <summary>This member is bounded to the Name property</summary>
		protected string m_strName = "";		
		
		/// <summary>This member is bounded to the Deponent property</summary>
		protected string m_strDeponent = "";		
		
		/// <summary>This member is bounded to the Date property</summary>
		protected string m_strDate = "";		
		
		/// <summary>This member is bounded to the LinePerPage property</summary>
		protected int m_iLinesPerPage = 25;	
		
		/// <summary>Local member accessed by the Converted property</summary>
		protected bool m_bConverted = false;	
		
		#endregion Protected Members

		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlDeposition()
		{
			m_strRoot = XML_DEPOSITION_ROOT_NAME;
			Extension = GetExtension();
			
			m_xmlSegments.Comparer = new CXmlBaseSorter();
			m_xmlSegments.KeepSorted = false;

			m_xmlTranscripts.Comparer = new CXmlBaseSorter();
			m_xmlTranscripts.KeepSorted = false;

			//	Populate the error builder
			SetErrorStrings();
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source object to be copied</param>
		public CXmlDeposition(CXmlDeposition xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
					
			m_strRoot = XML_DEPOSITION_ROOT_NAME;
			Extension = GetExtension();
			
			m_xmlSegments.Comparer = new CXmlBaseSorter();
			m_xmlSegments.KeepSorted = false;

			m_xmlTranscripts.Comparer = new CXmlBaseSorter();
			m_xmlTranscripts.KeepSorted = false;
				
			if(xmlSource != null)
				Copy(xmlSource);
		}
			
		/// <summary>Called to copy the properties of the source object</summary>
		///	<param name="xmlSource">the source object to be copied</param>
		/// <param name="bSegments">true to copy the segments</param>
		/// <param name="bTranscripts">true to copy the transcripts</param>
		/// <param name="bObjections">true if the objections should be copied</param>
		public void Copy(CXmlDeposition xmlSource, bool bSegments, bool bTranscripts, bool bObjections)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
					
			this.MediaId = xmlSource.MediaId;
			this.Name = xmlSource.Name;
			this.Deponent = xmlSource.Deponent;
			this.Date = xmlSource.Date;
			this.LinesPerPage = xmlSource.LinesPerPage;
			this.Converted = xmlSource.Converted;
			
			//	Should we copy the segments?
			m_xmlSegments.Clear();
			if(bSegments == true)
			{
				if(xmlSource.Segments != null)
				{
					foreach(CXmlSegment O in xmlSource.Segments)
						m_xmlSegments.Add(new CXmlSegment(O));
				}
					
			}// if(bSegments == true)
					
			//	Should we copy the transcripts?
			m_xmlTranscripts.Clear();
			if(bTranscripts == true)
			{
				//	Did we copy the segments?
				if(bSegments == true)
				{
					foreach(CXmlSegment O in this.Segments)
					{
						foreach(CXmlTranscript T in O.Transcripts)
							m_xmlTranscripts.Add(T);
					}
				}
				else
				{
					if(xmlSource.Transcripts != null)
					{
						foreach(CXmlTranscript O in xmlSource.Transcripts)
							m_xmlTranscripts.Add(new CXmlTranscript(O));
					}
				
				}// if(bSegments == true)
					
			}// if(bTranscripts == true)

			//	Should we copy the objections?
			m_aObjections.Clear();
			if(bObjections == true)
			{
				if(xmlSource.Objections != null)
				{
					foreach(CXmlObjections O in xmlSource.Objections)
						m_aObjections.Add(new CXmlObjections(O));
				}

			}// if(bObjections == true)

		}// public void Copy(CXmlTranscript xmlSource)
		
		/// <summary>This method is called to close the XML file associated with the deposition</summary>
		/// <param name="bClear">true to clear the local collections</param>
		public void Close(bool bClear)
		{
			//	Do the base class processing
			base.Close();
			
			//	Should we clear the collections?
			if(bClear == true)
				Clear();
			
			// NOTE:	We do not clear the FileSpec value so that the user can set it
			//			once and then read / write at will
		}
			
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			return -1;
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>This method is called to get the filter string used to initialize a file selection form</summary>
		/// <param name="bAllowAll">true to allow All Files option</param>
		/// <returns>The appropriate filter string</returns>
		new static public string GetFilter(bool bAllowAll)
		{
			if(bAllowAll == true)
				return "Depositions (*.xmlt)|*.xmlt|All Files (*.*)|*.*";
			else
				return "Depositions (*.xmlt)|*.xmlt";
		}
		
		/// <summary>This method is called to get the default file extension</summary>
		/// <returns>The default file extension</returns>
		new static public string GetExtension()
		{
			return "xmlt";
		}
		
		/// <summary>This method is called to get the default lines per page</summary>
		/// <returns>The default extension</returns>
		static public int GetDefaultLinesPerPage()
		{
			return 25;
		}
		
		/// <summary>Called to clear the depositions child collections</summary>
		public void Clear()
		{
			if(m_xmlTranscripts != null)
			{
				m_xmlTranscripts.Clear();
			}
			
			if(m_xmlSegments != null)
			{
				m_xmlSegments.Clear();
			}

			if(m_aObjections != null)
			{
				m_aObjections.Clear();
			}

		}// public void Clear()
		
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
					//	Get the deposition descriptor node
					if((xmlNode = m_xmlRoot.SelectSingleNode(XML_DEPOSITION_ELEMENT_NAME)) == null)
					{
						//	Is this a new file?
						if(bCreate == true)
						{
							//	We don't expect the deposition node to 
							//	appear in a new file
							bSuccessful = true;
						}
						else
						{
							m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_DEPOSITION_NODE, m_strFileSpec));
						}
						break;
					}
					
					//	Get the deposition properties
					if(SetProperties(xmlNode) == false)
						break;
						
					//	We're done 
					bSuccessful = true;
					
				}//	while(bSuccessful == false)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_DEPOSITION_EX, m_strFileSpec), Ex);
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
			
		}//	public override bool Open(bool bCreate)
		
		/// <summary>This method is called to populate the segments collection</summary>
		/// <param name="xpDocument">The XPath document containing the segment nodes</param>
		/// <param name="strDepositionPath">The path to the owner deposition node</param>
		/// <param name="bTranscripts">true to load the segment's transcript text</param>
		/// <returns>true if successful</returns>
		public bool GetSegments(XPathDocument xpDocument, string strDepositionPath, bool bTranscripts)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			CXmlSegment			xmlSegment = null;
			string				strXPath = "";
			
			try
			{
				//	Make sure we have a valid segments collection
				Debug.Assert(m_xmlSegments != null);
				m_xmlSegments.Clear();
					
				//	Construct the path to use for the XPath query
				if((strDepositionPath != null) && (strDepositionPath.Length > 0))
					strXPath = String.Format("{0}/{1}", strDepositionPath, CXmlSegment.XML_SEGMENT_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}/{2}", XML_DEPOSITION_ROOT_NAME, XML_DEPOSITION_ELEMENT_NAME, CXmlSegment.XML_SEGMENT_ELEMENT_NAME);
				
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				
				//	Add an object for each segment
				while(xpIterator.MoveNext() == true)
				{
					xmlSegment = new CXmlSegment();
					xmlSegment.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
					xmlSegment.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
					
					if(xmlSegment.SetProperties(xpIterator.Current) == true)
					{
						m_xmlSegments.Add(xmlSegment);
						
						//	Should we get the transcript text?
						if(bTranscripts == true)
							xmlSegment.GetTranscripts(xpDocument, strDepositionPath);
					}
						
				}// while(xpIterator.MoveNext() == true)

				m_xmlSegments.Sort(true);
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSegments", m_tmaxErrorBuilder.Message(ERROR_GET_SEGMENTS_EX), Ex);
				return false;
			}
			
		}// public bool GetSegments(XPathDocument xpDocument)
		
		/// <summary>This method is called to populate the segments collection</summary>
		/// <param name="xpIterator">The XPath iterator containing the segment nodes</param>
		/// <param name="bTranscripts">true to load the segment's transcript text</param>
		/// <returns>true if successful</returns>
		public bool GetSegments(XPathNodeIterator xpIterator, bool bTranscripts)
		{
			XPathNodeIterator	xpTranscripts = null;
			CXmlSegment			xmlSegment = null;
			
			try
			{
				//	Make sure we have a valid segments collection
				Debug.Assert(m_xmlSegments != null);
				m_xmlSegments.Clear();
					
				if(bTranscripts == true)
				{
					Debug.Assert(m_xmlTranscripts != null);
					m_xmlTranscripts.Clear();
				}
				
				//	Add an object for each segment
				while(xpIterator.MoveNext() == true)
				{
					xmlSegment = new CXmlSegment();
					m_tmaxEventSource.Attach(xmlSegment.EventSource);
					
					if(xmlSegment.SetProperties(xpIterator.Current) == true)
					{
						m_xmlSegments.Add(xmlSegment);
						
						//	Should we get the transcript text?
						if(bTranscripts == true)
						{
							//	Get the transcript text for this designation
							if((xpTranscripts = xpIterator.Current.Select(CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME)) != null)
								xmlSegment.GetTranscripts(xpTranscripts);
						
							//	Add to the master collection
							foreach(CXmlTranscript O in xmlSegment.Transcripts)
								m_xmlTranscripts.Add(O);
						}
						
					}// if(xmlSegment.SetProperties(xpIterator.Current) == true)
						
				}// while(xpIterator.MoveNext() == true)

				//	Make sure the collections are sorted
				m_xmlSegments.Sort(true);
				if(bTranscripts == true)
					m_xmlTranscripts.Sort(true);
					
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSegments", m_tmaxErrorBuilder.Message(ERROR_GET_SEGMENTS_EX), Ex);
				return false;
			}
			
		}// public bool GetSegments(XPathDocument xpDocument)

		/// <summary>This method is called to populate the segments collection</summary>
		/// <param name="xpDocument">The XPath document containing the segment nodes</param>
		/// <param name="strDepositionPath">The path to the owner deposition node</param>
		/// <returns>true if successful</returns>
		public bool GetObjections(XPathDocument xpDocument, string strDepositionPath)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator = null;
			CXmlObjections		xmlObjections = null;
			string				strXPath = "";

			try
			{
				//	Make sure we have a valid segments collection
				Debug.Assert(m_aObjections != null);
				m_aObjections.Clear();

				//	Construct the path to use for the XPath query
				if((strDepositionPath != null) && (strDepositionPath.Length > 0))
					strXPath = String.Format("{0}/{1}", strDepositionPath, CXmlObjections.XML_OBJECTIONS_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}/{2}", XML_DEPOSITION_ROOT_NAME, XML_DEPOSITION_ELEMENT_NAME, CXmlObjections.XML_OBJECTIONS_ELEMENT_NAME);

				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;

				//	Add an object for each segment
				while(xpIterator.MoveNext() == true)
				{
					xmlObjections = new CXmlObjections();
					this.EventSource.Attach(xmlObjections.EventSource);

					if(xmlObjections.SetProperties(xpIterator.Current) == true)
					{
						m_aObjections.Add(xmlObjections);

						//	Fill the collection
						xmlObjections.GetObjections(xpDocument, strDepositionPath);
					}

				}// while(xpIterator.MoveNext() == true)

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetObjections", m_tmaxErrorBuilder.Message(ERROR_GET_OBJECTIONS_EX), Ex);
				return false;
			}

		}// public bool GetObjections(XPathDocument xpDocument, string strDepositionPath)

		/// <summary>This method is called to populate the objections collection</summary>
		/// <param name="xpIterator">The XPath iterator containing the objections nodes</param>
		/// <returns>true if successful</returns>
		public bool GetObjections(XPathNodeIterator xpIterator)
		{
			CXmlObjections		xmlObjections = null;
			XPathNodeIterator	xpObjections = null;

			try
			{
				//	Make sure we have a valid segments collection
				Debug.Assert(m_aObjections != null);
				m_aObjections.Clear();


				//	Add an object for each objection collection
				while(xpIterator.MoveNext() == true)
				{
					xmlObjections = new CXmlObjections();
					m_tmaxEventSource.Attach(xmlObjections.EventSource);

					if(xmlObjections.SetProperties(xpIterator.Current) == true)
					{
						m_aObjections.Add(xmlObjections);

						//	Fill the collection
						if((xpObjections = xpIterator.Current.Select(CXmlObjection.XML_OBJECTION_ELEMENT_NAME)) != null)
							xmlObjections.GetObjections(xpObjections);

					}// if(xmlObjections.SetProperties(xpIterator.Current) == true)

				}// while(xpIterator.MoveNext() == true)

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetObjections", m_tmaxErrorBuilder.Message(ERROR_GET_OBJECTIONS_EX), Ex);
				return false;
			}

		}// public bool GetObjections(XPathNodeIterator xpIterator)

		/// <summary>This method is called to get the first PL value</summary>
		/// <returns>The first PL value associated with the segments</returns>
		public long GetFirstPL()
		{
			long lPL = -1;
			
			if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
			{
				//	The list should be sorted but we'll play it safe just in case
				//	we ever changed the sort criteria
				lPL = m_xmlSegments[0].FirstPL;
				
				for(int i = 1; i < m_xmlSegments.Count; i++)
				{
					if(m_xmlSegments[i].FirstPL < lPL)
						lPL = m_xmlSegments[i].FirstPL;
				}
				
			}// if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
			
			return lPL;
					
		}// public long GetFirstPL()
		
		/// <summary>This method is called to get the last PL value</summary>
		/// <returns>The last PL value associated with the segments</returns>
		public long GetLastPL()
		{
			long lPL = -1;
			
			if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
			{
				//	The list should be sorted but we'll play it safe just in case
				//	we ever changed the sort criteria
				lPL = m_xmlSegments[0].LastPL;
				
				for(int i = 1; i < m_xmlSegments.Count; i++)
				{
					if(m_xmlSegments[i].LastPL > lPL)
						lPL = m_xmlSegments[i].LastPL;
				}
				
			}// if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
			
			return lPL;
					
		}// public long GetFirstPL()
		
		/// <summary>This method is called to get the first PL value</summary>
		/// <returns>The first PL value associated with the segments</returns>
		public bool GetPLRange(ref long lFirstPL, ref long lLastPL)
		{
			bool bSuccessful = false;
			
			try
			{
				if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
				{
					//	The list should be sorted but we'll play it safe just in case
					//	we ever changed the sort criteria
					lFirstPL = m_xmlSegments[0].FirstPL;
					lLastPL  = m_xmlSegments[0].LastPL;
					
					for(int i = 1; i < m_xmlSegments.Count; i++)
					{
						if(m_xmlSegments[i].FirstPL < lFirstPL)
							lFirstPL = m_xmlSegments[i].FirstPL;
						if(m_xmlSegments[i].LastPL > lLastPL)
							lLastPL = m_xmlSegments[i].LastPL;
					}
					
				}// if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
			
			}
			catch
			{
			}
			
			return bSuccessful;
					
		}// public bool GetPLRange(ref long lFirstPL, ref long lLastPL)
		
		/// <summary>This method is called to populate the transcripts collection</summary>
		/// <param name="xpDocument">The XPath document containing the transcript nodes</param>
		/// <param name="strDepositionPath">The path to the owner deposition node</param>
		/// <returns>true if successful</returns>
		public bool GetTranscripts(XPathDocument xpDocument, string strDepositionPath)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			CXmlTranscript		xmlTranscript = null;
			string				strXPath = "";
			
			try
			{
				//	Make sure we have a valid transcripts collection
				Debug.Assert(m_xmlTranscripts != null);
				m_xmlTranscripts.Clear();
					
				//	Have we populated the segments collection?
				if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
				{
					//	Retrieve the transcripts on a segment by segment basis
					foreach(CXmlSegment O in m_xmlSegments)
					{
						//	Do we need to populate the transcripts for this segment?
						if((O.Transcripts == null) || (O.Transcripts.Count == 0))
						{
							O.GetTranscripts(xpDocument, strDepositionPath);
						}
						
						//	Add each transcript to our local collection
						if((O.Transcripts != null) && (O.Transcripts.Count > 0))
						{
							foreach(CXmlTranscript xmlt in O.Transcripts)
								m_xmlTranscripts.Add(xmlt);
						}
					
					}// foreach(CXmlSegment O in m_xmlSegments)
					
				}
				else
				{
					//	Construct the path to use for the XPath query
					if((strDepositionPath != null) && (strDepositionPath.Length > 0))
						strXPath = String.Format("{0}/{1}/{2}", strDepositionPath, CXmlSegment.XML_SEGMENT_ELEMENT_NAME, CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME);
					else
						strXPath = String.Format("{0}/{1}/{2}/{3}", XML_DEPOSITION_ROOT_NAME, XML_DEPOSITION_ELEMENT_NAME, CXmlSegment.XML_SEGMENT_ELEMENT_NAME, CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME);
				
					if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
					if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
					
					//	Add an object for each transcript
					while(xpIterator.MoveNext() == true)
					{
						xmlTranscript = new CXmlTranscript();
						xmlTranscript.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
						xmlTranscript.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
						
						if(xmlTranscript.SetProperties(xpIterator.Current) == true)
							m_xmlTranscripts.Add(xmlTranscript);
							
					}// while(xpIterator.MoveNext() == true)
				
				}
				
				//	Make sure all are sorted
				m_xmlTranscripts.Sort();

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", m_tmaxErrorBuilder.Message(ERROR_GET_TRANSCRIPTS_EX), Ex);
				return false;
			}
			
		}// public bool GetTranscripts(XPathDocument xpDocument)
		
		/// <summary>This method is called to populate the transcripts collection</summary>
		/// <param name="xpIterator">The iterator for the collection of XML transcript nodes</param>
		/// <returns>true if successful</returns>
		public bool GetTranscripts(XPathNodeIterator xpIterator)
		{
			CXmlTranscript xmlTranscript = null;

			try
			{
				//	Make sure we have a valid transcripts collection
				Debug.Assert(m_xmlTranscripts != null);
				m_xmlTranscripts.Clear();
					
				//	Add an object for each transcript
				while(xpIterator.MoveNext() == true)
				{
					xmlTranscript = new CXmlTranscript();
					m_tmaxEventSource.Attach(xmlTranscript.EventSource);
						
					if(xmlTranscript.SetProperties(xpIterator.Current) == true)
						m_xmlTranscripts.Add(xmlTranscript);
							
				}// while(xpIterator.MoveNext() == true)
				
				//	Make sure all are sorted
				m_xmlTranscripts.Sort();

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", m_tmaxErrorBuilder.Message(ERROR_GET_TRANSCRIPTS_EX), Ex);
				return false;
			}
			
		}// public bool GetTranscripts(XPathNodeIterator xpIterator)
		
		/// <summary>This method is called to get the segment with the specified key</summary>
		/// <returns>The segment with the key value specified by the caller</returns>
		public CXmlSegment GetSegment(string strKey)
		{
			if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
			{
				for(int i = 0; i < m_xmlSegments.Count; i++)
				{
					if(m_xmlSegments[i].Key == strKey)
						return m_xmlSegments[i];
				}
				
			}// if((m_xmlSegments != null) && (m_xmlSegments.Count > 0))
			
			return null;
					
		}// public CXmlSegment GetSegment(string strKey)
		
		/// <summary>This method is called to initialize the object using the specified XPath document</summary>
		/// <param name="xpDocument">The XPath document containing the desposition</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <param name="bSegments">true to fill the segments collection</param>
		/// <param name="bTranscripts">true to fill the transcripts collection</param>
		/// <param name="bObjections">true to fill the objections collection</param>
		/// <returns>true if successful</returns>
		public bool FastFill(XPathDocument xpDocument, string strParentPath, bool bSegments, bool bTranscripts, bool bObjections)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			string				strXPath = "";

			if(xpDocument == null) return false;
			
			try
			{
				//	Build the path for the XPath query
				if((strParentPath == null) || (strParentPath.Length == 0))
					strXPath = String.Format("{0}/{1}", XML_DEPOSITION_ROOT_NAME, XML_DEPOSITION_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}", strParentPath, XML_DEPOSITION_ELEMENT_NAME);

				//	Get the deposition properties
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				if(xpIterator.Count > 0)
				{
					xpIterator.MoveNext();
					SetProperties(xpIterator.Current);
				}
				else
				{
					m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_NO_DEPOSITION_NODE, m_strFileSpec));
					return false;
				}

				//	Get the segments
				if(bSegments == true)
					GetSegments(xpDocument, strXPath, bTranscripts);
				
				//	Get the transcript text
				if(bTranscripts == true)
					GetTranscripts(xpDocument, strXPath);

				//	Get the objections
				if(bObjections == true)
					GetObjections(xpDocument, strXPath);

				return true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FAST_FILL_EX, m_strFileSpec), Ex);
			}
			
			//	Must have been a problem
			return false;
			
		}
		
		/// <summary>The method uses XPath queries against the specified file to initialize the objec</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <param name="bSegments">true to fill the segments collection</param>
		/// <param name="bTranscripts">true to fill the transcripts collection</param>
		/// <param name="bObjections">true to fill the objections collection</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec, string strParentPath, bool bSegments, bool bTranscripts, bool bObjections)
		{
			XPathDocument xpDocument = null;

			//	Make sure the file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FILE_NOT_FOUND, strFileSpec));
				return false;
			}
			
			try
			{
				//	Load the file
				xpDocument = new System.Xml.XPath.XPathDocument(strFileSpec);
				
				//	Fill the collections
				SetFileProps(strFileSpec);
				
				return FastFill(xpDocument, strParentPath, bSegments, bTranscripts, bObjections);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FAST_FILL_EX, strFileSpec), Ex);
			}
			
			//	Must have been a problem
			return false;
			
		}
		
		/// <summary>The method uses XPath queries against the specified file to initialize the objec</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="bSegments">true to fill the segments collection</param>
		/// <param name="bTranscripts">true to fill the transcripts collection</param>
		/// <param name="bObjections">true to fill the objections collection</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec, bool bSegments, bool bTranscripts, bool bObjections)
		{
			return FastFill(strFileSpec, "", bSegments, bTranscripts, bObjections);
		}
		
		/// <summary>The method uses XPath queries against the specified file to initialize the objec</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec, string strParentPath)
		{
			return FastFill(strFileSpec, strParentPath, true, true, true);
		}

		/// <summary>The method uses XPath queries against the specified file to initialize the objec</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec)
		{
			return FastFill(strFileSpec, "");
		}

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
				strElementName = XML_DEPOSITION_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_DEPOSITION_ATTRIBUTE_MEDIA_ID, MediaId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DEPOSITION_ATTRIBUTE_NAME, Name) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DEPOSITION_ATTRIBUTE_DEPONENT, Deponent) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DEPOSITION_ATTRIBUTE_DATE, Date) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_DEPOSITION_ATTRIBUTE_LINES_PER_PAGE, LinesPerPage) == false)
						break;
						
					if((Segments != null) && (Segments.Count > 0))
					{
						foreach(CXmlSegment O in Segments)
						{
							if((xmlChild = O.ToXmlNode(xmlDocument)) != null)
							{
								xmlElement.AppendChild(xmlChild);
							}
						}
					}

					if((Objections != null) && (Objections.Count > 0))
					{
						foreach(CXmlObjections O in Objections)
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
		
		/// <summary>This method is called to save the deposition to file</summary>
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

					//	Get the node for the deposition
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
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
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
		
		/// <summary>This method is called to create designations that cover the specified range</summary>
		/// <param name="xmlDesignations">The collection in which to place the designations</param>
		/// <param name="lStartPL">The start position</param>
		/// <param name="lStopPL">The stop position</param>
		/// <param name="iHighlighter">The highlighter id assigned to the new designations</param>
		/// <returns>true if successful</returns>
		public bool CreateDesignations(CXmlDesignations xmlDesignations, long lStartPL, long lStopPL, int iHighlighter)
		{
			CXmlDesignation xmlDesignation = null;
			CXmlTranscript	xmlTranscript = null;
			int				i = 0;
			bool			bFinished = false;
			
			Debug.Assert(xmlDesignations != null);
			Debug.Assert(lStartPL > 0);
			Debug.Assert(lStopPL > 0);
			Debug.Assert(lStopPL >= lStartPL);
			
			//	Do we have any transcript lines?
			if((m_xmlTranscripts == null) || (m_xmlTranscripts.Count == 0))
				return false;

			try
			{
				//	Find the first line
				for(i = 0; i < m_xmlTranscripts.Count; i++)
				{
					//	Have we reached the start point
					if(m_xmlTranscripts[i].PL == lStartPL)
					{
						break;
					}
					else if(m_xmlTranscripts[i].PL > lStartPL)
					{
						//	Back up one line if possible
						if(i > 0) i--;
						break;
					}
					
				}
				
				//	We're we unable to find the first line?
				if(i == m_xmlTranscripts.Count) return false;

				//	Now add the lines that fall within the range
				for(; ((i < m_xmlTranscripts.Count) && (bFinished == false)); i++)
				{
					//	Is this the last line to be added?
					if(m_xmlTranscripts[i].PL >= lStopPL)
					{
						//	Break after adding this line
						bFinished = true;
					}
					
					//	Do we need a new designation?
					if((xmlDesignation == null) || 
					   (String.Compare(xmlDesignation.Segment, m_xmlTranscripts[i].Segment, true) != 0))
					{
						//	Have we already created this designation?
						if((xmlDesignation = xmlDesignations.FindFromSegment(m_xmlTranscripts[i].Segment)) == null)
						{
							//	Create a designation
							xmlDesignation = new CXmlDesignation();
							xmlDesignation.Segment = m_xmlTranscripts[i].Segment;
							xmlDesignation.Highlighter = iHighlighter;
							xmlDesignation.PrimaryId = this.MediaId;
							
							//	Add this designation to the collection
							xmlDesignations.Add(xmlDesignation);
						}
						else
						{
							Debug.Assert(false); // this shouldn't happen
						}
						
					}
					
					//	Add this line to the designation
					xmlTranscript = new CXmlTranscript(m_xmlTranscripts[i]);
					xmlDesignation.Transcripts.Add(xmlTranscript);
				
				}// for(; i < m_xmlTranscripts.Count; i++)
				
				//	Set the extents for each new designation
				foreach(CXmlDesignation O in xmlDesignations)
				{
					O.HasText = true;
					O.ScrollText = true;
					O.SetExtents();
					O.SetNameFromExtents(this);
				}
				
				return true;	
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDesignations", m_tmaxErrorBuilder.Message(ERROR_CREATE_DESIGNATIONS_EX, lStartPL, lStopPL), Ex);
				return false;
			}
			
		}// public xmlDesignations CreateDesignations(CXmlDesignations xmlDesignations, long lStartPL, long lStopPL)
		
		/// <summary>This method is called to populate the transcripts text for the specified designation</summary>
		/// <param name="xmlDesignation">The designation that needs to be populated</param>
		/// <returns>true if successful</returns>
		public bool GetTranscripts(CXmlDesignation xmlDesignation)
		{
			CXmlTranscript	xmlTranscript = null;
			int				i = 0;
			bool			bFinished = false;
			
			Debug.Assert(xmlDesignation != null);
			
			//	Do we have any transcript lines?
			if((m_xmlTranscripts == null) || (m_xmlTranscripts.Count == 0))
				return false;
				
			try
			{
				//	Find the first line
				for(i = 0; i < m_xmlTranscripts.Count; i++)
				{
					//	Have we reached the start point
					if(m_xmlTranscripts[i].PL == xmlDesignation.FirstPL)
					{
						break;
					}
					else if(m_xmlTranscripts[i].PL > xmlDesignation.FirstPL)
					{
						//	Back up one line if possible
						if(i > 0) i--;
						break;
					}
					
				}
				
				//	We're we unable to find the first line?
				if(i == m_xmlTranscripts.Count) return false;
			
				//	Now add the lines that fall within the range
				for(; ((i < m_xmlTranscripts.Count) && (bFinished == false)); i++)
				{
					//	Is this the last line to be added?
					if(m_xmlTranscripts[i].PL >= xmlDesignation.LastPL)
					{
						//	Break after adding this line
						bFinished = true;
					}
					
					//	Add this line to the designation
					xmlTranscript = new CXmlTranscript(m_xmlTranscripts[i]);
					xmlDesignation.Transcripts.Add(xmlTranscript);
				
				}// for(; i < m_xmlTranscripts.Count; i++)
				
				return true;	
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetTranscripts", m_tmaxErrorBuilder.Message(ERROR_GET_DESIGNATION_TRANSCRIPTS_EX, xmlDesignation.Name), Ex);
				return false;
			}
			
		}// public bool GetTranscripts(CXmlDesignation xmlDesignation)

		/// <summary>This method is called to get the objections collection owned by the specified case</summary>
		/// <param name="strName">The case name</param>
		/// <param name="strId">The unique id assigned to the case</param>
		/// <param name="strVersion">The case version descriptor</param>
		/// <param name="bAdd">True if ok to create the objection collection and add it to the local list</param>
		/// <returns>The specified collection</returns>
		public CXmlObjections GetCaseObjections(string strName, string strId, string strVersion, bool bAdd)
		{
			CXmlObjections xmlObjections = null;

			Debug.Assert(m_aObjections != null);
			if(m_aObjections == null)
				m_aObjections = new ArrayList();

			try
			{
				//	Are we grouping the objects by name?
				if(strName.Length > 0)
				{
					//	Search for the requested collection
					foreach(CXmlObjections O in m_aObjections)
					{
						if(String.Compare(O.CaseName, strName, true) == 0)
						{
							xmlObjections = O;
							break;
						}

					}// foreach(CXmlObjections O in m_aObjections)

				}
				else
				{
					//	Search for the requested collection
					foreach(CXmlObjections O in m_aObjections)
					{
						if(String.Compare(O.CaseId, strId, true) == 0)
						{
							xmlObjections = O;
							break;
						}

					}// foreach(CXmlObjections O in m_aObjections)

				}// if(strName.Length > 0)
				
				//	Do we need to allocate a new collection?
				if((xmlObjections == null) && (bAdd == true))
				{
					xmlObjections = new CXmlObjections();
					xmlObjections.CaseId      = strId;
					xmlObjections.CaseName	  = strName;
					xmlObjections.CaseVersion = strVersion;
					m_aObjections.Add(xmlObjections);

				}// if((xmlObjections == null) && (bAdd == true))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetCaseObjections", m_tmaxErrorBuilder.Message(ERROR_GET_CASE_OBJECTIONS_EX, strName), Ex);
			}
			
			return xmlObjections;

		}// public CXmlObjections GetCaseObjections(string strName, string strId, string strVersion, bool bAdd)

		/// <summary>This method is called to get the objections collection owned by the specified case</summary>
		/// <param name="strName">The case name</param>
		/// <param name="strId">The unique id assigned to the case</param>
		/// <param name="strVersion">The case version descriptor</param>
		/// <returns>The specified collection</returns>
		public CXmlObjections GetCaseObjections(string strName, string strId, string strVersion)
		{
			return GetCaseObjections(strName, strId, strVersion, true);
		}

		/// <summary>This method is called to get the objections collection owned by the specified case</summary>
		/// <param name="tmaxCase">The case descriptor</param>
		/// <param name="bAdd">True if ok to create the objection collection and add it to the local list</param>
		/// <returns>The specified collection</returns>
		public CXmlObjections GetCaseObjections(CTmaxCase tmaxCase, bool bAdd)
		{
			Debug.Assert(tmaxCase != null);
			if(tmaxCase == null) return null;
			
			return GetCaseObjections(tmaxCase.Name, tmaxCase.UniqueId, tmaxCase.Version, bAdd);

		}// public CXmlObjections GetCaseObjections(CTmaxCase tmaxCase, bool bAdd)

		/// <summary>This method is called to get the objections collection owned by the specified case</summary>
		/// <param name="tmaxCase">The case descriptor</param>
		/// <returns>The specified collection</returns>
		public CXmlObjections GetCaseObjections(CTmaxCase tmaxCase)
		{
			Debug.Assert(tmaxCase != null);
			if(tmaxCase == null) return null;

			return GetCaseObjections(tmaxCase, true);

		}// public CXmlObjections GetCaseObjections(CTmaxCase tmaxCase)

		/// <summary>This method is called to add the specified objections</summary>
		/// <param name="tmaxCase">The case descriptor</param>
		/// <param name="tmaxObjections">The application objections collection</param>
		/// <param name="bVerify">True to verify the deposition id</param>
		/// <param name="bClear">True to clear the existing Objections</param>
		/// <returns>true if successful</returns>
		public bool AddObjections(CTmaxCase tmaxCase, CTmaxObjections tmaxObjections, bool bVerify, bool bClear)
		{
			CXmlObjections	xmlObjections = null;
			bool			bSuccessful = false;

			Debug.Assert(tmaxObjections != null);
			if(tmaxObjections == null) return false;

			Debug.Assert(m_aObjections != null);
			if(m_aObjections == null) 
				m_aObjections = new ArrayList();

			try
			{
				//	Should we clear out the existing objections?
				if(bClear == true)
					m_aObjections.Clear();
					
				//	Add each of the application objections
				foreach(CTmaxObjection O in tmaxObjections)
				{
					//	Should we verify the MediaId ?
					if(bVerify == true)
					{
						if(String.Compare(this.MediaId, O.Deposition, true) != 0)
							continue;
					}
					
					//	Has a case been assigned to this objection?
					if((O.Case != null) && (O.CaseName.Length > 0))
					{
						if((xmlObjections == null) || (String.Compare(xmlObjections.CaseName, O.CaseName, true) != 0))
						{
							xmlObjections = GetCaseObjections(O.Case);
						}
					}
					else if(tmaxCase != null)
					{
						if((xmlObjections == null) || (String.Compare(xmlObjections.CaseName, tmaxCase.Name, true) != 0))
						{
							xmlObjections = GetCaseObjections(tmaxCase);
						}

					}// if((O.Case != null) && (O.CaseName.Length > 0))
						
					if(xmlObjections != null)
						xmlObjections.Add(new CXmlObjection(O));
						
				}// foreach(CTmaxObjection O in tmaxObjections)
				
				//	Make sure all the collections are in sorted order
				if((m_aObjections != null) && (m_aObjections.Count > 0))
				{
					foreach(CXmlObjections O in m_aObjections)
					{
						try { O.Sort(true); }
						catch {}
					}

				}// if((m_aObjections != null) && (m_aObjections.Count > 0))
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddObjections", m_tmaxErrorBuilder.Message(ERROR_ADD_OBJECTIONS_EX), Ex);
			}

			return bSuccessful;

		}// public bool AddObjections(CTmaxCase tmaxCase, CTmaxObjections tmaxObjections, bool bVerify, bool bClear)

		/// <summary>This method is called to add the specified objections</summary>
		/// <param name="tmaxCase">The case descriptor</param>
		/// <param name="tmaxObjections">The application objections collection</param>
		/// <param name="bVerify">True to verify the deposition id</param>
		/// <returns>true if successful</returns>
		public bool AddObjections(CTmaxCase tmaxCase, CTmaxObjections tmaxObjections, bool bVerify)
		{
			return AddObjections(tmaxCase, tmaxObjections, true);
		}

		/// <summary>This method is called to add the specified objections</summary>
		/// <param name="tmaxCase">The case descriptor</param>
		/// <param name="tmaxObjections">The application objections collection</param>
		/// <returns>true if successful</returns>
		public bool AddObjections(CTmaxCase tmaxCase, CTmaxObjections tmaxObjections)
		{
			return AddObjections(tmaxCase, tmaxObjections, true, true);
		}

		/// <summary>This method uses the current document to set the deposition properties</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			if(xpNavigator == null) return false;

			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_DEPOSITION_ATTRIBUTE_MEDIA_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strMediaId = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_DEPOSITION_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DEPOSITION_ATTRIBUTE_DEPONENT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strDeponent = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DEPOSITION_ATTRIBUTE_DATE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strDate = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_DEPOSITION_ATTRIBUTE_LINES_PER_PAGE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_iLinesPerPage = System.Convert.ToInt32(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method uses the specified node to set the deposition properties</summary>
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
			aStrings.Add("An exception was raised while attempting to open the deposition: filename = %1");
			aStrings.Add("%1 is not a valid XML transcript. It does not contain a valid deposition node.");
			aStrings.Add("An exception was raised while attempting to set the deposition properties");
			aStrings.Add("An exception was raised while attempting to fast fill the deposition: filename = %1");
			aStrings.Add("An exception was raised while attempting to retrieve the deposition segments");
			
			aStrings.Add("An exception was raised while attempting to retrieve the deposition transcript text");
			aStrings.Add("An exception was raised while attempting to create the new designations: startPL = %1 stopPL = %2");
			aStrings.Add("An exception was raised while attempting to retrieve the text for a designation: Name = %1");
			aStrings.Add("An exception was raised while attempting to retrieve the objections for the specified case: Id = %1");
			aStrings.Add("An exception was raised while attempting to add an objection");

			aStrings.Add("An exception was raised while attempting to add a collection of objections");
			aStrings.Add("An exception was raised while attempting to fill the objections collection");
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		//	Media ID assigned to the deposition by the database
		public string MediaId
		{
			get{ return m_strMediaId; }
			set{ m_strMediaId = value; }
		}
		
		//	Flag to indicated that the file was converted from a legacy log
		public bool Converted
		{
			get{ return m_bConverted; }
			set{ m_bConverted = value; }
		}
		
		//	Name assigned to the deposition node
		public string Name
		{
			get{ return m_strName; }
			set{ m_strName = value; }
		}
		
		//	Deponent associated with the deposition
		public string Deponent
		{
			get{ return m_strDeponent; }
			set{ m_strDeponent = value; }
		}
		
		//	Date of the the deposition
		public string Date
		{
			get{ return m_strDate; }
			set{ m_strDate = value; }
		}
		
		//	Number of lines per transcript page
		public int LinesPerPage
		{
			get{ return ((m_iLinesPerPage > 0) ? m_iLinesPerPage : GetDefaultLinesPerPage()); }
			set{ m_iLinesPerPage = value; }
		}
		
		//	Collection of segments that belong to the deposition
		public CXmlSegments Segments
		{
			get{ return m_xmlSegments; }
		}
		
		//	Collection of transcript text associated with the segment
		public CXmlTranscripts Transcripts
		{
			get{ return m_xmlTranscripts; }
		}

		//	Collection of objection collections grouped by CaseId
		public ArrayList Objections
		{
			get { return m_aObjections; }
		}

		#endregion Properties
		
	}// public class CXmlDeposition

	/// <summary>Objects of this class are used to manage a dynamic array of CXmlDeposition objects</summary>
	public class CXmlDepositions : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlDepositions() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="xmlDeposition">CXmlDeposition object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlDeposition Add(CXmlDeposition xmlDeposition)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlDeposition as object);

				return xmlDeposition;
			}
			catch
			{
				return null;
			}
			
		}// public CXmlDeposition Add(CXmlDeposition xmlDeposition)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="xmlDeposition">The object to be removed</param>
		public void Remove(CXmlDeposition xmlDeposition)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlDeposition as object);
			}
			catch
			{
			}
		
		}// public void Remove(CXmlDeposition xmlDeposition)

		/// <summary>This method is called to locate the object with the matching media ID</summary>
		/// <param name="strMediaId">The desired media ID</param>
		/// <returns>The object with the matching media ID</returns>
		public CXmlDeposition Find(string strMediaId)
		{
			try
			{
				foreach(CXmlDeposition O in this)
				{
					if(String.Compare(O.MediaId, strMediaId, true) == 0)
						return O;
				}
				
			}
			catch
			{
			}
			
			return null;
		
		}// public CXmlDeposition Find(string strMediaId)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlDeposition">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlDeposition xmlDeposition)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlDeposition as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CXmlDeposition this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlDeposition);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlDeposition value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CXmlScenes
		
}// namespace FTI.Shared.Xml
