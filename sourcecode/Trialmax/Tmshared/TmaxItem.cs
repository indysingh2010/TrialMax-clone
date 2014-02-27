using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>
	/// Objects of this class are used to pass information about selected items
	/// in events fired by this control
	/// </summary>
	public class CTmaxItem : IXmlObject
	{
		#region Private Members
		
		/// <summary>Local member accessed by Primary property</summary>
		private ITmaxMediaRecord m_IPrimary = null;
			
		/// <summary>Local member accessed by Secondary property</summary>
		private ITmaxMediaRecord m_ISecondary = null;
			
		/// <summary>Local member accessed by Tertiary property</summary>
		private ITmaxMediaRecord m_ITertiary = null;
			
		/// <summary>Local member accessed by Quaternary property</summary>
		private ITmaxMediaRecord m_IQuaternary = null;
			
		/// <summary>Local member accessed by IBinderEntry property</summary>
		private ITmaxMediaRecord m_IBinderEntry = null;
			
		/// <summary>Local member accessed by ICode property</summary>
		private ITmaxMediaRecord m_ICode = null;

		/// <summary>Local member accessed by IObjection property</summary>
		private ITmaxBaseRecord m_IObjection = null;

		/// <summary>Local member accessed by IAppNotification property</summary>
		private ITmaxAppNotification m_IAppNotification = null;
			
		/// <summary>Local member accessed by PickItem property</summary>
		private CTmaxPickItem m_tmaxPickItem = null;
			
		/// <summary>Local member accessed by CaseCode property</summary>
		private CTmaxCaseCode m_tmaxCaseCode = null;
			
		/// <summary>Local member accessed by SourceFolder property</summary>
		private CTmaxSourceFolder m_tmaxSourceFolder = null;
			
		/// <summary>Local member accessed by SourceFile property</summary>
		private CTmaxSourceFile m_tmaxSourceFile = null;
			
		/// <summary>Local member accessed by SubItems property</summary>
		private CTmaxItems m_aSubItems = new CTmaxItems();
			
		/// <summary>Local member accessed by SourceItems property</summary>
		private CTmaxItems m_aSourceItems = null;
			
		/// <summary>Local member accessed by ParentItem property</summary>
		private CTmaxItem m_tmaxParentItem = null;
			
		/// <summary>Local member accessed by ReturnItem property</summary>
		private CTmaxItem m_tmaxReturnItem = null;
			
		/// <summary>Local member accessed by State property</summary>
		private TmaxItemStates m_eState = TmaxItemStates.Pending;
			
		/// <summary>Local member accessed by MediaType property</summary>
		private TmaxMediaTypes m_eMediaType = TmaxMediaTypes.Unknown;			
		
		/// <summary>Local member accessed by DataType property</summary>
		private TmaxDataTypes m_eDataType = TmaxDataTypes.Unknown;			
		
		/// <summary>Local member accessed by UserData1 property</summary>
		private object m_userData1 = null;
			
		/// <summary>Local member accessed by UserData2 property</summary>
		private object m_userData2 = null;

		/// <summary>Local member accessed by Reload property</summary>
		private bool m_bReload = false;

		/// <summary>Private member bound to XmlScript property</summary>
		private CXmlScript m_xmlScript = null;
			
		/// <summary>Private member bound to XmlDesignation property</summary>
		private CXmlDesignation m_xmlDesignation = null;
			
		/// <summary>Private member bound to XmlLink property</summary>
		private CXmlLink m_xmlLink = null;
			
		/// <summary>Private member bound to XmlTranscript property</summary>
		private CXmlTranscript m_xmlTranscript = null;
			
		/// <summary>Private member bound to XmlScene property</summary>
		private CXmlScene m_xmlScene = null;

		/// <summary>Private member bound to TmaxObjection property</summary>
		private CTmaxObjection m_tmaxObjection = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxItem()
		{
		}
		
		/// <summary>Overloaded constructor</summary>
		public CTmaxItem(ITmaxBaseRecord IRecord)
		{
			SetRecord(IRecord);
		}
		
		/// <summary>Copy constructor</summary>
		public CTmaxItem(CTmaxItem objSource)
		{
			Copy(objSource);
		}
		
		/// <summary>Constructor (used by TmaxVideo application)</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		/// <param name="xmlLink">The XML link bound to the item</param>
		/// <param name="xmlTranscript">The XML transcript bound to the item</param>
		public CTmaxItem(CXmlScript xmlScript, CXmlDesignation xmlDesignation, CXmlLink xmlLink, CXmlTranscript xmlTranscript)
		{
			Initialize(xmlScript, xmlDesignation, xmlLink, xmlTranscript);
		}
		
		/// <summary>Constructor (used by TmaxVideo application)</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		/// <param name="xmlTranscript">The XML transcript bound to the item</param>
		public CTmaxItem(CXmlScript xmlScript, CXmlDesignation xmlDesignation, CXmlTranscript xmlTranscript)
		{
			Initialize(xmlScript, xmlDesignation, xmlTranscript);
		}
		
		/// <summary>Constructor (used by TmaxVideo application)</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		/// <param name="xmlLink">The XML link bound to the item</param>
		public CTmaxItem(CXmlScript xmlScript, CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		{
			Initialize(xmlScript, xmlDesignation, xmlLink);
		}
		
		/// <summary>Constructor (used by TmaxVideo application)</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		public CTmaxItem(CXmlScript xmlScript, CXmlDesignation xmlDesignation)
		{
			Initialize(xmlScript, xmlDesignation);
		}
		
		/// <summary>Constructor (used by TmaxVideo application)</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		public CTmaxItem(CXmlScript xmlScript)
		{
			Initialize(xmlScript);
		}
		
		/// <summary>Constructor (used by TmaxVideo application)</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlScene">The XML scene bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		public CTmaxItem(CXmlScript xmlScript, CXmlScene xmlScene, CXmlDesignation xmlDesignation)
		{
			Initialize(xmlScript, xmlScene, xmlDesignation);
		}
		
		/// <summary>Constructor (used by TmaxVideo application)</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlScene">The XML scene bound to the event item</param>
		public CTmaxItem(CXmlScript xmlScript, CXmlScene xmlScene)
		{
			Initialize(xmlScript, xmlScene);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="tmaxPickItem">The application pick list item bound to the event item</param>
		public CTmaxItem(CTmaxPickItem tmaxPickItem)
		{
			Initialize(tmaxPickItem);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="tmaxCaseCode">The application case code bound to the event item</param>
		public CTmaxItem(CTmaxCaseCode tmaxCaseCode)
		{
			Initialize(tmaxCaseCode);
		}

		/// <summary>Constructor</summary>
		/// <param name="tmaxObjection">The application objection bound to the event item</param>
		public CTmaxItem(CTmaxObjection tmaxObjection)
		{
			Initialize(tmaxObjection);
		}

		/// <summary>This method is called to get a unique id for the item</summary>
		///	<returns>The unique id for the item</returns>
		public string GetUniqueId()
		{
			string strId = "";
			
			if(GetMediaRecord() != null)
				strId = GetMediaRecord().GetUniqueId();

			return strId;
						
		}// public string GetUniqueId()
		
		/// <summary>This method is called to get the IRecord interface associated with this item</summary>
		///	<returns>The associated IRecord interface</returns>
		public ITmaxBaseRecord GetRecord()
		{
			//	Is this a media record?
			if(GetMediaRecord() != null)
				return GetMediaRecord();
			else if(m_IBinderEntry != null)
				return m_IBinderEntry;
			else if(m_ICode != null)
				return m_ICode;
			else if(m_IObjection != null)
				return m_IObjection;
			else
				return null;

		}// public ITmaxBaseRecord GetRecord()
		
		/// <summary>This method is called to get the IRecord interface associated with this item</summary>
		///	<returns>The associated IRecord interface</returns>
		public ITmaxMediaRecord GetMediaRecord()
		{
			//	What is the media level?
			switch(MediaLevel)
			{
				case TmaxMediaLevels.Primary:
					
					Debug.Assert(m_IPrimary != null);
					return m_IPrimary;
							
				case TmaxMediaLevels.Secondary:
					
					Debug.Assert(m_ISecondary != null);
					return m_ISecondary;
							
				case TmaxMediaLevels.Tertiary:
					
					Debug.Assert(m_ITertiary != null);
					return m_ITertiary;
							
				case TmaxMediaLevels.Quaternary:
					
					Debug.Assert(m_IQuaternary != null);
					return m_IQuaternary;
							
				default:
					
					return null;
			}
						
		}// public ITmaxMediaRecord GetMediaRecord()
		
		/// <summary>This method is called to set the record interfaces using the specified record</summary>
		///	<param name="IRecord">The record to be associated with this item</param>
		public void SetRecord(ITmaxBaseRecord IRecord)
		{
			//	Initialize the related members
			m_IBinderEntry = null;
			m_IPrimary = null;
			m_ISecondary = null;
			m_ITertiary = null;
			m_IQuaternary = null;
			m_ICode = null;
			m_IObjection = null;
			m_eMediaType = TmaxMediaTypes.Unknown;
			m_eDataType = TmaxDataTypes.Unknown;

			//	NOTE:	We intentionally do not clear the XML members
			//			here because they may still be valid for the event
			
			if(IRecord != null)
			{
				m_eDataType = IRecord.GetDataType();
				
				switch(m_eDataType)
				{
					case TmaxDataTypes.Media:
					
						m_eMediaType = ((ITmaxMediaRecord)(IRecord)).GetMediaType();
						
						//	What is the media level?
						switch(((ITmaxMediaRecord)(IRecord)).GetMediaLevel())
						{
							case TmaxMediaLevels.Primary:

								m_IPrimary = (ITmaxMediaRecord)IRecord;
								break;
									
							case TmaxMediaLevels.Secondary:

								m_ISecondary = (ITmaxMediaRecord)IRecord;
								m_IPrimary   = m_ISecondary.GetParent();
								break;
									
							case TmaxMediaLevels.Tertiary:

								m_ITertiary = (ITmaxMediaRecord)IRecord;
								if((m_ISecondary = m_ITertiary.GetParent()) != null)
								{
									m_IPrimary = m_ISecondary.GetParent();
								}
								break;
									
							case TmaxMediaLevels.Quaternary:

								m_IQuaternary = (ITmaxMediaRecord)IRecord;
								if((m_ITertiary = m_IQuaternary.GetParent()) != null)
								{
									if((m_ISecondary = m_ITertiary.GetParent()) != null)
									{
										m_IPrimary = m_ISecondary.GetParent();
									}
								}
								break;
									
						}// switch(IRecord.GetMediaLevel())
						break;
						
					case TmaxDataTypes.Binder:

						m_IBinderEntry = (ITmaxMediaRecord)IRecord;
						break;
				
					case TmaxDataTypes.Code:

						m_ICode = (ITmaxMediaRecord)IRecord;
						break;

					case TmaxDataTypes.Objection:

						if((m_IObjection = IRecord) != null)
						{
							m_tmaxObjection = ((ITmaxObjectionRecord)m_IObjection).GetTmaxObjection();
						}
						break;

				}// switch(m_eDataType)
						
			}// if(IRecord != null)
			
		}// public void SetRecord(ITmaxMediaRecord IRecord)
		
		/// <summary>This method is called to make a copy of a source event</summary>
		/// <param name="objSource">The object whose members are to be copied</param>
		/// <param name="bSubItems">true to copy subitems collection</param>
		/// <param name="bSourceItems">true to copy source items collection</param>
		public void Copy(CTmaxItem objSource, bool bSubItems, bool bSourceItems)
		{
			Debug.Assert(objSource != null);
			if(objSource == null) return;
			
			m_tmaxSourceFolder	= objSource.m_tmaxSourceFolder;
			m_eDataType			= objSource.DataType;
			m_IBinderEntry		= objSource.m_IBinderEntry;
			m_IPrimary			= objSource.m_IPrimary;
			m_ISecondary		= objSource.m_ISecondary;
			m_ITertiary			= objSource.m_ITertiary;
			m_IQuaternary		= objSource.m_IQuaternary;
			m_ICode				= objSource.m_ICode;
			m_IObjection		= objSource.m_IObjection;
			m_IAppNotification	= objSource.m_IAppNotification;
			m_xmlScript			= objSource.m_xmlScript;
			m_xmlDesignation	= objSource.m_xmlDesignation;
			m_xmlLink			= objSource.m_xmlLink;
			m_xmlTranscript		= objSource.m_xmlTranscript;
			m_xmlScene			= objSource.m_xmlScene;
			m_tmaxPickItem		= objSource.m_tmaxPickItem;
			m_tmaxCaseCode		= objSource.m_tmaxCaseCode;
			m_tmaxObjection		= objSource.m_tmaxObjection;
			m_bReload			= objSource.m_bReload;
			
			//	Copy the subitems if requested
			if(bSubItems == true)
			{
				if(m_aSubItems != null)
					m_aSubItems.Clear();
				else
					m_aSubItems = new CTmaxItems();
				
				if((objSource.SubItems != null) && (objSource.SubItems.Count > 0))
					m_aSubItems.Copy(objSource.SubItems);
			}
		
			//	Copy the source items if requested
			if(bSourceItems == true)
			{
				if(m_aSourceItems != null)
				{
					m_aSourceItems.Clear();
					m_aSourceItems = null; // We'll recreate if we have to
				}
				
				if(objSource.SubItems != null) 
					m_aSourceItems = new CTmaxItems(objSource.SubItems);
			}
		
		}// public void Copy(CTmaxItem objSource, bool bSubItems, bool bSourceItems)
		
		/// <summary>This method is called to make a copy of a source event</summary>
		/// <param name="objSource">The object whose members are to be copied</param>
		public void Copy(CTmaxItem objSource)
		{
			Copy(objSource, true, true);
					
		}// public void Copy(CTmaxItem objSource)
		
		/// <summary>This method is called to reset the class members and clear the child collections</summary>																																																			</summary>
		public void Clear()
		{
			m_tmaxSourceFolder	= null;
			m_aSourceItems		= null;
			m_IBinderEntry		= null;
			m_IPrimary			= null;
			m_ISecondary		= null;
			m_ITertiary			= null;
			m_IQuaternary		= null;
			m_ICode				= null;
			m_IObjection		= null;
			m_IAppNotification	= null;
			m_xmlScript			= null;
			m_xmlDesignation	= null;
			m_xmlLink			= null;
			m_xmlTranscript		= null;
			m_xmlScene			= null;
			m_tmaxPickItem		= null;
			m_tmaxCaseCode		= null;
			m_tmaxObjection		= null;
			m_userData1			= null;
			m_userData2			= null;
			m_bReload			= false;
			m_tmaxParentItem	= null;
			m_eMediaType		= TmaxMediaTypes.Unknown;
						
			if(m_aSubItems != null)
			{
				m_aSubItems.Clear();
			}
		
			if(m_aSourceItems != null)
			{
				m_aSourceItems.Clear();
				m_aSourceItems = null;
			}
		
		}// public void Clear()
		
		/// <summary>This method will make a copy of the specified item but subitems will be put in source items collection</summary>
		/// <param name="tmaxItem">The item to be copied</param>
		/// <returns>The rearranged item</returns>
		public CTmaxItem SubToSource()
		{
			CTmaxItem	tmaxSource = new CTmaxItem();
			CTmaxItem	tmaxSub = null;
			
			//	Copy the item without copying the collections
			tmaxSource.Copy(this, false, false);
			
			//	Transfer subitems to the local source items collection
			if(this.SubItems != null)
			{
				tmaxSource.SourceItems = new CTmaxItems();
				
				foreach(CTmaxItem O in this.SubItems)
				{
					if((tmaxSub = O.SubToSource()) != null)
						tmaxSource.SourceItems.Add(tmaxSub);
				}
				
			}
			
			return tmaxSource;
			
		}// public CTmaxItem SubToSource(CTmaxItem tmaxItem)
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		///	<returns>An Xml node that represents the object</returns>
		XmlNode IXmlObject.ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument);
		}
		
		/// <summary>This method implements the IXmlObject.ToXmlNode() method</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">Name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		XmlNode IXmlObject.ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			return ToXmlNode(xmlDocument, strName);
		}
		
		/// <summary>This method creates an xml node that represents the item</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument, null);
		}
			
		/// <summary>This method creates an xml node that represents the item</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		/// <param name="strName">Name to be assigned to the element</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlItem  = null;
			XmlElement	xmlSubItems = null;
			XmlElement	xmlSourceItems = null;
			XmlElement  xmlInterface = null;
			string		strElementName = "";
			
			//	What name are we going to use for this element?
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = "tmaxItem";
			
			//	Create the top-level node
			if((xmlItem = xmlDocument.CreateElement(strElementName)) == null)
				return null;

			//	Add the item attribute
			xmlItem.SetAttribute("MediaLevel", MediaLevel.ToString());
			xmlItem.SetAttribute("MediaType", MediaType.ToString());
				
			//	Add each of the interfaces
			if((xmlInterface = (XmlElement)ToXmlNode(xmlDocument, ICode, "ICode")) != null)
				xmlItem.AppendChild(xmlInterface);
			if((xmlInterface = (XmlElement)ToXmlNode(xmlDocument, IBinderEntry, "IBinderEntry")) != null)
				xmlItem.AppendChild(xmlInterface);
			if((xmlInterface = (XmlElement)ToXmlNode(xmlDocument, IPrimary, "IPrimary")) != null)
				xmlItem.AppendChild(xmlInterface);
			if((xmlInterface = (XmlElement)ToXmlNode(xmlDocument, ISecondary, "ISecondary")) != null)
				xmlItem.AppendChild(xmlInterface);
			if((xmlInterface = (XmlElement)ToXmlNode(xmlDocument, ITertiary, "ITertiary")) != null)
				xmlItem.AppendChild(xmlInterface);
			if((xmlInterface = (XmlElement)ToXmlNode(xmlDocument, IQuaternary, "IQuaternary")) != null)
				xmlItem.AppendChild(xmlInterface);
			
			//	Add a child node for the SubItems collection
			if(m_aSubItems != null)
			{
				xmlSubItems = (XmlElement)m_aSubItems.ToXmlNode(xmlDocument, "SubItems");
			}
			else
			{
				if((xmlSubItems = xmlDocument.CreateElement("SubItems")) != null)
				{
					xmlSubItems.SetAttribute("Ref", "NULL");
				}
			
			}
			
			if(xmlSubItems != null)
				xmlItem.AppendChild(xmlSubItems);
			
			//	Add a child node for the SourceItems collection
			if(m_aSourceItems != null)
			{
				xmlSourceItems = (XmlElement)m_aSourceItems.ToXmlNode(xmlDocument, "SourceItems");
			}
			else
			{
				if((xmlSourceItems = xmlDocument.CreateElement("SourceItems")) != null)
				{
					xmlSourceItems.SetAttribute("Ref", "NULL");
				}
			
			}
			
			if(xmlSourceItems != null)
				xmlItem.AppendChild(xmlSourceItems);
			
			return xmlItem;
		
		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		/// <summary>This method creates an xml node that represents the specified record interface</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		/// <param name="strName">Name to be assigned to the element</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, ITmaxMediaRecord tmaxRecord, string strName)
		{
			XmlElement	xmlRecord  = null;
			string		strElementName = "";
			
			try
			{
				if(tmaxRecord != null)
				{
					return tmaxRecord.GetXmlNode(xmlDocument, strName);
				}
				else
				{
					//	What name are we going to use for this element?
					if((strName != null) && (strName.Length > 0))
						strElementName = strName;
					else
						strElementName = "ITmaxMediaRecord";
				
					//	Create the top-level node
					if((xmlRecord = xmlDocument.CreateElement(strElementName)) != null)
					{
						xmlRecord.SetAttribute("ref", "NULL");
					}
				
					return xmlRecord;
					
				}
			}
			catch
			{
				return null;
			}
		
		}// public virtual XmlNode ToXmlNode(ITmaxMediaRecord tmaxRecord, XmlDocument xmlDocument, string strName)
		
		/// <summary>Builds the string representation of the item</summary>
		/// <returns>A string that identifies the record referenced by the item</returns>
		public override string ToString()
		{
			string strString = "";

			if(GetMediaRecord() != null)
			{
				strString = GetMediaRecord().GetBarcode(false);
			}
			else if(IBinderEntry != null)
			{
				strString = IBinderEntry.GetName();
			}
			else if(ICode != null)
			{
				strString = ICode.GetName();
			}
			else if(IObjection != null)
			{
				strString = "Objection";
			}
			else if(m_xmlScript != null)
			{
				if(m_xmlLink != null)
					strString = m_xmlLink.GetDisplayString();
				else if(m_xmlDesignation != null)
					strString = m_xmlDesignation.GetDisplayString();
				else
					strString = m_xmlScript.GetDisplayString();
			}
			else
			{
				strString = MediaType.ToString();
				if(strString.EndsWith("s") == false)
					strString += "s";
			}
			
			return strString;
			
		}// public override string ToString()

		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		/// <param name="xmlLink">The XML link bound to the item</param>
		/// <param name="xmlTranscript">The XML transcript bound to the item</param>
		public void Initialize(CXmlScript xmlScript, CXmlDesignation xmlDesignation, CXmlLink xmlLink, CXmlTranscript xmlTranscript)
		{
			Clear();
			m_xmlScript = xmlScript;
			m_xmlDesignation = xmlDesignation;
			m_xmlLink = xmlLink;
			m_xmlTranscript = xmlTranscript;
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		/// <param name="xmlTranscript">The XML transcript bound to the item</param>
		public void Initialize(CXmlScript xmlScript, CXmlDesignation xmlDesignation, CXmlTranscript xmlTranscript)
		{
			Initialize(xmlScript, xmlDesignation, null, xmlTranscript);
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		/// <param name="xmlLink">The XML link bound to the item</param>
		public void Initialize(CXmlScript xmlScript, CXmlDesignation xmlDesignation, CXmlLink xmlLink)
		{
			Initialize(xmlScript, xmlDesignation, xmlLink, null);
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		public void Initialize(CXmlScript xmlScript, CXmlDesignation xmlDesignation)
		{
			Initialize(xmlScript, xmlDesignation, null, null);
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		public void Initialize(CXmlScript xmlScript)
		{
			Initialize(xmlScript, null, null, null);
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlScene">The XML scene bound to the item</param>
		/// <param name="xmlDesignation">The XML designation bound to the item</param>
		public void Initialize(CXmlScript xmlScript, CXmlScene xmlScene, CXmlDesignation xmlDesignation)
		{
			Clear();
			m_xmlScript = xmlScript;
			m_xmlScene = xmlScene;
			m_xmlDesignation = xmlDesignation;
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="xmlScript">The XML script bound to the event item</param>
		/// <param name="xmlScene">The XML scene bound to the item</param>
		public void Initialize(CXmlScript xmlScript, CXmlScene xmlScene)
		{
			Clear();
			m_xmlScript = xmlScript;
			m_xmlScene = xmlScene;
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="tmaxPickItem">The application pick list item bound to the event item</param>
		public void Initialize(CTmaxPickItem tmaxPickItem)
		{
			Clear();
			m_eDataType = TmaxDataTypes.PickItem;
			m_tmaxPickItem = tmaxPickItem;
		}
		
		/// <summary>This method initializes the item using the specified objects</summary>
		/// <param name="tmaxPickItem">The application case code object bound to the event item</param>
		public void Initialize(CTmaxCaseCode tmaxCaseCode)
		{
			Clear();
			m_eDataType = TmaxDataTypes.CaseCode;
			m_tmaxCaseCode = tmaxCaseCode;
		}

		/// <summary>This method initializes the item using the specified objection</summary>
		/// <param name="tmaxPickItem">The application objection object bound to the event item</param>
		public void Initialize(CTmaxObjection tmaxObjection)
		{
			Clear();
			m_eDataType = TmaxDataTypes.Objection;
			if((m_tmaxObjection = tmaxObjection) != null)
				m_IObjection = m_tmaxObjection.IOxObjection;
		}

		#endregion Public Methods
			
		#region Properties
		
		/// <summary>This property contains the collection of subitems</summary>
		public CTmaxItems SubItems
		{
			get { return m_aSubItems; }
		}   	
		
		/// <summary>This property contains the collection of items that can be used as the source for an event</summary>
		public CTmaxItems SourceItems
		{
			get 
			{ 
				//	Allocate when referenced
				if(m_aSourceItems == null) 
					m_aSourceItems = new CTmaxItems(); 
				return m_aSourceItems; 
			}
			set { m_aSourceItems = value; }
		}

		/// <summary>This property exposes an item that us to explicitly define the parent when the normal parental relationship won't work</summary>
		public CTmaxItem ParentItem
		{
			get { return m_tmaxParentItem; }                                                                                                                                                                                                                                                                                                         
			set { m_tmaxParentItem = value; }
		} 
		
		/// <summary>This property exposes an item that allows the event processor to return a result</summary>
		public CTmaxItem ReturnItem
		{
			get { return m_tmaxReturnItem; }
			set { m_tmaxReturnItem = value; }
		}
		
		/// <summary>User defined data #1</summary>
		public object UserData1
		{
			get { return m_userData1; }
			set { m_userData1 = value; }
		}

		/// <summary>User defined data #2</summary>
		public object UserData2
		{
			get { return m_userData2; }
			set { m_userData2 = value; }
		}

		/// <summary>True to force reloading of the specified record</summary>
		public bool Reload
		{
			get { return m_bReload; }
			set { m_bReload = value; }
		}

		/// <summary>This is the data type associated with this item</summary>
		public FTI.Shared.Trialmax.TmaxDataTypes DataType
		{
			get
			{
				ITmaxBaseRecord IRecord = GetRecord();
				
				//	Use the record interface if available
				if(IRecord != null)
					return IRecord.GetDataType();
				else if(m_xmlScript != null)
					return TmaxDataTypes.Media;
				else if(m_tmaxPickItem != null)
					return TmaxDataTypes.PickItem;
				else if(m_tmaxCaseCode != null)
					return TmaxDataTypes.CaseCode;
				else if(m_tmaxObjection != null)
					return TmaxDataTypes.Objection;
				else
					return m_eDataType;
			}
			set
			{
				//	This allows non-record nodes to associate themselves
				//	with a data type
				m_eDataType = value;
			}
			
		}// DataType property
		
		/// <summary>This is the media type associated with this item</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes MediaType
		{
			get
			{
				ITmaxMediaRecord IRecord = GetMediaRecord();
				
				//	Use the record interface if available
				if(IRecord != null)
				{
					return IRecord.GetMediaType();
				}
				else if(m_xmlScript != null)
				{
					if(m_xmlLink != null)
						m_eMediaType = TmaxMediaTypes.Link;
					else if(m_xmlDesignation != null)
						m_eMediaType = TmaxMediaTypes.Designation;
					else
						m_eMediaType = TmaxMediaTypes.Script;
						
					return m_eMediaType;
				}
				else
				{
					return m_eMediaType;
				}
			}
			set
			{
				//	This allows non-record nodes to associate themselves
				//	with a media type
				m_eMediaType = value;
			}
			
		}// MediaType property
		
		/// <summary>This is the media level associated with this item</summary>
		public FTI.Shared.Trialmax.TmaxMediaLevels MediaLevel
		{
			get
			{
				//	The media level is determined by the valid interfaces
				if(m_IPrimary != null)
				{
					if(m_ISecondary != null)
					{
						if(m_ITertiary != null)
						{
							if(m_IQuaternary != null)
								return TmaxMediaLevels.Quaternary;
							else
								return TmaxMediaLevels.Tertiary;
						}
						else
						{
							return TmaxMediaLevels.Secondary;
						}
						
					}
					else
					{
						return TmaxMediaLevels.Primary;
					
					}// if(m_ISecondary != null) 
					
				}
				else
				{
					return TmaxMediaLevels.None;
				
				}// if(m_iPrimary != null)
				
			}              
			
		} // MediaLevel property

		/// <summary>This is the registration source associated with this item</summary>
		public CTmaxSourceFolder SourceFolder
		{
			get { return m_tmaxSourceFolder; }
			set { m_tmaxSourceFolder = value; }
		}

		/// <summary>This is the source file associated with some TrialMax commands (eg. Import)</summary>
		public CTmaxSourceFile SourceFile
		{
			get { return m_tmaxSourceFile; }
			set { m_tmaxSourceFile = value;}
		}
		/// <summary>This is the current state of the event item</summary>
		public TmaxItemStates State
		{
			get{ return m_eState; }
			set { m_eState = value; }
		}

		/// <summary>This is the primary database record associated with this item</summary>
		public ITmaxMediaRecord IPrimary
		{
			get { return m_IPrimary; }
			set { m_IPrimary = value; }
		}
		/// <summary>This is the secondary database record associated with this item</summary>
		public ITmaxMediaRecord ISecondary
		{
			get { return m_ISecondary; }
			set { m_ISecondary = value; }
		}

		/// <summary>This is the tertiary database record associated with this item</summary>
		public ITmaxMediaRecord ITertiary
		{
			get { return m_ITertiary; }
			set { m_ITertiary = value; }
		} 
		/// <summary>This is the quaternary database record associated with this item</summary>
		public ITmaxMediaRecord IQuaternary
		{
			get { return m_IQuaternary; }
			set { m_IQuaternary = value; }
		} 

		/// <summary>This is the record exchange interface associated with a binder entry record</summary>
		public ITmaxMediaRecord IBinderEntry
		{
			get { return m_IBinderEntry; }
			set { m_IBinderEntry = value; }
		}

		/// <summary>This is the record exchange interface associated with a code record</summary>
		public ITmaxMediaRecord ICode
		{
			get { return m_ICode; }
			set { m_ICode = value; }
		}

		/// <summary>This is the record exchange interface associated with an objection record</summary>
		public ITmaxBaseRecord IObjection
		{
			get { return m_IObjection; }
			set { m_IObjection = value; }
		}

		/// <summary>This is the application notification interface bound to the event</summary>
		public ITmaxAppNotification IAppNotification
		{
			get { return m_IAppNotification; }
			set { m_IAppNotification = value; }
		}

		/// <summary>The XML script bound to the item (used by TmaxVideo application)</summary>
		public CXmlScript XmlScript
		{
			get { return m_xmlScript; }
			set { m_xmlScript = value; }
		}

		/// <summary>The XML designation bound to the item (used by TmaxVideo application)</summary>
		public CXmlDesignation XmlDesignation
		{
			get { return m_xmlDesignation; }
			set { m_xmlDesignation = value; }
		}

		/// <summary>The XML link bound to the item (used by TmaxVideo application)</summary>
		public CXmlLink XmlLink
		{
			get { return m_xmlLink; }
			set { m_xmlLink = value; }
		}

		/// <summary>The XML transcript bound to the item (used by TmaxVideo application)</summary>
		public CXmlTranscript XmlTranscript
		{
			get { return m_xmlTranscript; }
			set { m_xmlTranscript = value; }
		}

		/// <summary>The XML scene bound to the item (used by TmaxVideo application)</summary>
		public CXmlScene XmlScene
		{
			get { return m_xmlScene; }
			set { m_xmlScene = value; }
		}

		/// <summary>The application pick item bound to this event</summary>
		public CTmaxPickItem PickItem
		{
			get { return m_tmaxPickItem; }
			set { m_tmaxPickItem = value; }
		}

		/// <summary>The application case code bound to this event</summary>
		public CTmaxCaseCode CaseCode
		{
			get { return m_tmaxCaseCode; }
			set { m_tmaxCaseCode = value; }
		}

		/// <summary>The application objection bound to this event</summary>
		public CTmaxObjection Objection
		{
			get 
			{ 
				if(m_tmaxObjection != null)
					return m_tmaxObjection;
				else if(m_IObjection != null)
					return ((ITmaxObjectionRecord)m_IObjection).GetTmaxObjection();
				else
					return null;
			}
			set 
			{ 
				if((m_tmaxObjection = value) != null)
					m_IObjection = m_tmaxObjection.IOxObjection;
				else
					m_IObjection = null;
			}
		
		}

		#endregion Properties
		
	}//	CTmaxItem
		
	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxItem objects
	/// </summary>
	public class CTmaxItems : CollectionBase, IXmlObject
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxItems()
		{
		}
		
		/// <summary>Copy constructor</summary>
		public CTmaxItems(CTmaxItems tmaxItems)
		{
			if(tmaxItems != null)
				Copy(tmaxItems);
		
		}// CTmaxItems(CTmaxItems tmaxItems)
		
		/// <summary>
		/// This method will copy all items in the source collection and add them to this collection
		/// </summary>
		/// <param name="tmaxItems">The collection of items to be copied</param>
		public void Copy(CTmaxItems tmaxItems)
		{
			if(tmaxItems != null)
			{
				foreach(CTmaxItem tmaxItem in tmaxItems)
				{
					Add(new CTmaxItem(tmaxItem));
				}
			}
		
		}// Copy(CTmaxItems tmaxItems)

		/// <summary>This method allows the caller to add a new item to the list</summary>
		/// <param name="expItem">CTmaxItem object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxItem Add(CTmaxItem expItem)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(expItem as object);

				return expItem;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxItem expItem)

		/// <summary>
		/// This method is called to remove the requested parameter from the collection
		/// </summary>
		/// <param name="tmaxItem">The parameter object to be removed</param>
		public void Remove(CTmaxItem tmaxItem)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(tmaxItem as object);
			}
			catch
			{
			}
		}

		/// <summary>This method allows the caller to insert an item in the list</summary>
		/// <param name="expItem">CTmaxItem object to be added to the list</param>
		/// <param name="iIndex">Index where object is to be inserted</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxItem Insert(CTmaxItem expItem, int iIndex)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Insert(iIndex, expItem as object);

				return expItem;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxItem Insert(CTmaxItem expItem, int iIndex)

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="tmaxItem">The parameter object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxItem tmaxItem)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(tmaxItem as object);
		}

		/// <summary>
		/// This method is called to determine if the collection contains any items that represent TrialMax media
		/// </summary>
		/// <param name="bSubItems">True to include the subitems in the search</param>
		/// <returns>true if media items found in the collection</returns>
		public bool ContainsMedia(bool bSubItems)
		{
			foreach(CTmaxItem tmaxItem in this)
			{
				if(tmaxItem.MediaLevel != TmaxMediaLevels.None)
					return true;
					
				//	Are we checking subitems?
				if((bSubItems == true) && (tmaxItem.SubItems != null))
				{
					if(tmaxItem.SubItems.ContainsMedia(true) == true)
						return true;
				}
				
			}
			
			//	No media found
			return false;
		}

		/// <summary>
		/// This method is called to locate the item that references the specified record
		/// </summary>
		/// <returns>The associated event item if found</returns>
		public CTmaxItem Find(ITmaxMediaRecord tmaxRecord)
		{
			TmaxDataTypes eDataType = TmaxDataTypes.Unknown;
			
			Debug.Assert(tmaxRecord != null);
			Debug.Assert(tmaxRecord.GetDataType() != TmaxDataTypes.Unknown);
			
			//	Must have a valid data type
			if((eDataType = tmaxRecord.GetDataType()) == TmaxDataTypes.Unknown) 
				return null;
			
			foreach(CTmaxItem tmaxItem in this)
			{
				switch(eDataType)
				{
					case TmaxDataTypes.Media:
					
						if(tmaxItem.MediaLevel == tmaxRecord.GetMediaLevel())
						{
							if(tmaxItem.GetMediaRecord().GetAutoId() == tmaxRecord.GetAutoId())
								return tmaxItem;
						}
						break;
						
					case TmaxDataTypes.Binder:
					
						if((tmaxItem.IBinderEntry != null) &&
						   (tmaxItem.IBinderEntry.GetAutoId() == tmaxRecord.GetAutoId()))
						{
							return tmaxItem;
						}
						break;
						
				}// switch(eDataType)
				
			}

			return null;
			
		}// public CTmaxItem Find(ITmaxMediaRecord tmaxRecord)

		/// <summary>This method is called to locate the item that references the specified binder entry</summary>
		/// <param name="tmaxBinderEntry">Exchange interface to the desired binder entry</param>
		/// <returns>The associated event item if found</returns>
		public CTmaxItem FindBinderEntry(ITmaxMediaRecord tmaxBinderEntry)
		{
			Debug.Assert(tmaxBinderEntry != null);
			
			foreach(CTmaxItem tmaxItem in this)
			{
				if(ReferenceEquals(tmaxItem.IBinderEntry, tmaxBinderEntry) == true)
					return tmaxItem;
			}
			return null;
			
		}// public CTmaxItem FindBinderEntry(ITmaxMediaRecord tmaxBinderEntry)

		/// <summary>
		/// This method is called to locate the index of the item that references the specified record
		/// </summary>
		/// <returns>The item index if found, -1 otherwise</returns>
		public int IndexOf(ITmaxMediaRecord tmaxRecord)
		{
			CTmaxItem tmaxItem = null;
			
			Debug.Assert(tmaxRecord != null);
			
			if((tmaxItem = Find(tmaxRecord)) != null)
				return IndexOf(tmaxItem);
			else
				return -1;
			
		}// public int IndexOf(ITmaxMediaRecord tmaxRecord)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public CTmaxItem this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (base.List[index] as CTmaxItem);
			}
		}

		/// <summary>
		/// This method is called to retrieve the index of the specified object
		/// </summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxItem value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		/// <summary>This method is called to locate the index of the item that references the specified binder entry</summary>
		/// <returns>The item index if found, -1 otherwise</returns>
		public int IndexOfBinderEntry(ITmaxMediaRecord tmaxBinderEntry)
		{
			CTmaxItem tmaxItem = null;
			
			Debug.Assert(tmaxBinderEntry != null);
			
			if((tmaxItem = FindBinderEntry(tmaxBinderEntry)) != null)
				return IndexOf(tmaxItem);
			else
				return -1;
			
		}// public int IndexOfBinderEntry(ITmaxMediaRecord tmaxBinderEntry)

		/// <summary>This method implements the IXmlObject.ToXmlNode() method</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		///	<returns>An Xml node that represents the object</returns>
		XmlNode IXmlObject.ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument);
		}
		
		/// <summary>This method implements the IXmlObject.ToXmlNode() method</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">Name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		XmlNode IXmlObject.ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			return ToXmlNode(xmlDocument, strName);
		}
		
		/// <summary>This method creates an xml node that represents the collection</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument, null);

		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		/// <summary>This method creates an xml node that represents the collection</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		/// <param name="strName">Name assigned to the node</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlItems = null;
			XmlNode		xmlItem	= null;
			string		strElementName = "";
				
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = "tmaxItems";

			if((xmlItems = xmlDocument.CreateElement(strElementName)) != null)
			{
				//	Assign the attributes
				xmlItems.SetAttribute("Count", this.Count.ToString());
				
				//	Iterate the collection and append each item as a child
				foreach(CTmaxItem O in this)
				{
					if((xmlItem = O.ToXmlNode(xmlDocument)) != null)
					{
						xmlItems.AppendChild(xmlItem);
					}
				}

			}
			
			return xmlItems;

		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>This function will write the contents of the collection to the specified XML file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <returns>true if successful</returns>
		public virtual bool WriteToXml(string strFileSpec)
		{
			return WriteToXml(strFileSpec, "", true);
		}
			
		/// <summary>This function will write the contents of the collection to the specified XML file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <param name="bAppend">True to append to the existing file</param>
		/// <returns>true if successful</returns>
		public virtual bool WriteToXml(string strFileSpec, string strName)
		{
			return WriteToXml(strFileSpec, strName, true);
		}
			
		/// <summary>This function will write the contents of the collection to the specified XML file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <param name="strName">The name used to identify this collection in the file</param>
		/// <returns>true if successful</returns>
		public virtual bool WriteToXml(string strFileSpec, bool bAppend)
		{
			return WriteToXml(strFileSpec, "", bAppend);
		}
			
		/// <summary>This function will write the contents of the collection to the specified XML file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <param name="strName">The name used to identify this collection in the file</param>
		/// <param name="bAppend">True to append to the existing file</param>
		/// <returns>true if successful</returns>
		public virtual bool WriteToXml(string strFileSpec, string strName, bool bAppend)
		{
			try
			{
				CXmlFile xmlFile = new CXmlFile();
				
				xmlFile.AddDateToFilename = false;

				//	Should we delete the existing file?
				if(bAppend == false)
				{
					try
					{
						System.IO.File.Delete(strFileSpec);
					}
					catch
					{
					}
					
				}
				
				//	Open the XML file
				if(xmlFile.Open(strFileSpec) == true)
				{
					//	Write this collection to file
					xmlFile.Write(this, strName, true);
					
					xmlFile.Close();
					
					return true;
				}
				
			}
			catch
			{
			}
			
			return false;
			
		}// public virtual bool WriteToXml(string strFilespec, string strName)
				
		/// <summary>Builds the string representation of the collection</summary>
		/// <param name="strDelimiter">String used to delimit items in the composite string</param>
		/// <param name="strIfEmpty">String returned if the collection is empty</param>
		/// <returns>The string representation of the collection contents</returns>
		public string ToString(string strDelimiter, string strIfEmpty)
		{
			string	strString = "";
			string	strSeparate = ", ";
			
			if((strDelimiter != null) && (strDelimiter.Length > 0))
				strSeparate = strDelimiter;
				
			foreach(CTmaxItem O in this)
			{
				if(strString.Length > 0)
					strString += strSeparate;
				
				strString += O.ToString();

			}// foreach(CTmaxItem O in this)
			
			if((strString.Length == 0) && (strIfEmpty != null))
				strString = strIfEmpty;
				
			return strString;
			
		}// public string ToString(string strDelimiter, string strIfEmpty)

		/// <summary>Builds the string representation of the collection</summary>
		/// <param name="strDelimiter">String used to delimit items in the composite string</param>
		/// <returns>The string representation of the collection contents</returns>
		public string ToString(string strDelimiter)
		{
			return ToString(strDelimiter, "Empty");
			
		}// public string ToString(string strDelimiter)

		/// <summary>Builds the string representation of the collection</summary>
		/// <returns>The string representation of the collection contents</returns>
		public override string ToString()
		{
			return ToString(", ");
			
		}// public override string ToString()

		#endregion Public Methods
		
	}//	CTmaxItems
		
}
