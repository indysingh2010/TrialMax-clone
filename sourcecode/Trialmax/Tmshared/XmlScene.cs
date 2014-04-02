using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition scene</summary>
	public class CXmlScene : CXmlBase
	{
		#region Constants
		
		public const string XML_SCENE_ROOT_NAME = "trialMax";
		public const string XML_SCENE_ELEMENT_NAME = "scene";

		public const string XML_SCENE_ATTRIBUTE_SOURCE_TYPE		= "sourceType";
		public const string XML_SCENE_ATTRIBUTE_SOURCE_ID		= "sourceId";
		public const string XML_SCENE_ATTRIBUTE_AUTO_TRANSITION = "autoTransition";
		public const string XML_SCENE_ATTRIBUTE_TRANSITION_TIME = "transitionTime";
		public const string XML_SCENE_ATTRIBUTE_HIDDEN			= "hidden";
		public const string XML_SCENE_ATTRIBUTE_BARCODE_ID		= "barcodeId";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bounded to the SourceId property</summary>
		private string m_strSourceId = "";		
		
		/// <summary>This member is bounded to the SourceType property</summary>
		private TmaxMediaTypes m_eSourceType = TmaxMediaTypes.Unknown;
		
		/// <summary>This member is bounded to the Hidden property</summary>
		private bool m_bHidden = false;
		
		/// <summary>This member is bounded to the AutoTransition property</summary>
		private bool m_bAutoTransition = false;
		
		/// <summary>This member is bounded to the TransitionTime property</summary>
		private int m_iTransitionTime = 0;
		
		/// <summary>This member is bounded to the BarcodeId property</summary>
		private long m_lBarcodeId = 0;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlScene() : base()
		{
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source node to be copied</param>
		public CXmlScene(CXmlScene xmlSource) : base()
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
			return -1;
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the properties of the source node</summary>
		///	<param name="xmlSource">the source node to be copied</param>
		public void Copy(CXmlScene xmlSource)
		{
			this.SourceId = xmlSource.SourceId;
			this.SourceType = xmlSource.SourceType;
			this.Hidden = xmlSource.Hidden;
			this.AutoTransition = xmlSource.AutoTransition;
			this.TransitionTime = xmlSource.TransitionTime;
			this.BarcodeId = xmlSource.BarcodeId;
		}
			
		/// <summary>This method will set the scene properties using the specified node</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML scene properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the scene properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_SCENE_ATTRIBUTE_SOURCE_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.SourceId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_SCENE_ATTRIBUTE_SOURCE_TYPE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.SourceType = CTmaxToolbox.GetTypeFromString(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SCENE_ATTRIBUTE_HIDDEN,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Hidden = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SCENE_ATTRIBUTE_AUTO_TRANSITION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.AutoTransition = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SCENE_ATTRIBUTE_TRANSITION_TIME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.TransitionTime = System.Convert.ToInt32(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SCENE_ATTRIBUTE_BARCODE_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.BarcodeId = System.Convert.ToInt64(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML scene properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
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
				strElementName = XML_SCENE_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_SCENE_ATTRIBUTE_BARCODE_ID, this.BarcodeId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCENE_ATTRIBUTE_SOURCE_ID, this.SourceId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCENE_ATTRIBUTE_SOURCE_TYPE, this.SourceType.ToString()) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCENE_ATTRIBUTE_HIDDEN, this.Hidden) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCENE_ATTRIBUTE_AUTO_TRANSITION, this.AutoTransition) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCENE_ATTRIBUTE_TRANSITION_TIME, this.TransitionTime) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
		#region Properties
		
		//	Id of the source media
		public string SourceId
		{
			get{ return m_strSourceId; }
			set{ m_strSourceId = value; }
		}
		
		//	Media type of the source media
		public TmaxMediaTypes SourceType
		{
			get{ return m_eSourceType; }
			set{ m_eSourceType = value; }
		}
		
		//	True if the scene is hidden
		public bool Hidden
		{
			get{ return m_bHidden; }
			set{ m_bHidden = value; }
		}
		
		//	True to automatically transition to the next scene
		public bool AutoTransition
		{
			get{ return m_bAutoTransition; }
			set{ m_bAutoTransition = value; }
		}
		
		//	Transition time in seconds
		public int TransitionTime
		{
			get{ return m_iTransitionTime; }
			set{ m_iTransitionTime = value; }
		}
		
		//	Barcode identifier assigned by the database
		public long BarcodeId
		{
			get{ return m_lBarcodeId; }
			set{ m_lBarcodeId = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlScene

	/// <summary>Objects of this class are used to manage a dynamic array of CXmlScene objects</summary>
	public class CXmlScenes : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlScenes() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="xmlScene">CXmlScene object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlScene Add(CXmlScene xmlScene)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlScene as object);

				return xmlScene;
			}
			catch
			{
				return null;
			}
			
		}// public CXmlScene Add(CXmlScene xmlScene)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="xmlScene">The object to be removed</param>
		public void Remove(CXmlScene xmlScene)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlScene as object);
			}
			catch
			{
			}
		
		}// public void Remove(CXmlScene xmlScene)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlScene">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlScene xmlScene)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlScene as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CXmlScene this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlScene);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlScene value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CXmlScenes
		
}// namespace FTI.Shared.Xml
