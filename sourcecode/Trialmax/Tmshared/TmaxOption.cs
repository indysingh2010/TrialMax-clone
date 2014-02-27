using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to manage a user configurable option</summary>
	public class CTmaxOption : CXmlBase
	{
		#region Constants
		
		public const string XML_OPTION_ATTRIBUTE_VALUE = "value";
		public const string XML_OPTION_ATTRIBUTE_SELECTED = "selected";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Value property</summary>
		private object m_oValue;
		
		/// <summary>Local member bound to Text property</summary>
		private string m_strText;
		
		/// <summary>Local member bound to Selected property</summary>
		private bool m_bSelected = false;
		
		/// <summary>Local member bound to Selectable property</summary>
		private bool m_bSelectable = true;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxOption() : base()
		{
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">Source object to be copied</param>
		public CTmaxOption(CTmaxOption tmaxSource) : base()
		{
			if(tmaxSource != null)
				Copy(tmaxSource);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="oValue">Value assigned to this object</param>
		/// <param name="strText">Display text for this option</param>
		public CTmaxOption(object oValue, string strText)
		{
			m_oValue   = oValue;
			m_strText  = strText;
		}
		
		/// <summary>Called to copy the properties of the source object</summary>
		/// <param name="tmaxSource">The object whose properties are to be copied</param>
		public void Copy(CTmaxOption tmaxSource)
		{
			//	Copy the base class members
			base.Copy(tmaxSource as CXmlBase);
			
			Selectable = tmaxSource.Selectable;
			Selected = tmaxSource.Selected;
			Text = tmaxSource.Text;
			Value = tmaxSource.Value;
			
		}// public void Copy(CTmaxOption tmaxSource)
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="oValue">Value assigned to this object</param>
		/// <param name="strText">Display text for this option</param>
		/// <param name="bSelected">Flag to set the Selected state</param>
		public CTmaxOption(object oValue, string strText, bool bSelected)
		{
			m_oValue    = oValue;
			m_strText   = strText;
			m_bSelected = bSelected;
		}
		
		/// <summary>Overloaded base class method to return the string representation of this item</summary>
		/// <returns>The string representation of this item</returns>
		public override string ToString()
		{
			if(m_strText.Length > 0)
				return m_strText;
			else if(m_oValue != null)
				return m_oValue.ToString();
			else
				return "Tmax Option";
		}
		
		/// <summary>This method will set the object properties using the specified node</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the tmaxOption properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the link properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				m_strText = xpNavigator.Name;
				
				strAttribute = xpNavigator.GetAttribute(XML_OPTION_ATTRIBUTE_VALUE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_oValue = strAttribute;
				else
					m_oValue = "";

				strAttribute = xpNavigator.GetAttribute(XML_OPTION_ATTRIBUTE_SELECTED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					m_bSelected = XmlToBool(strAttribute);
					m_bSelectable = true;
				}
				else
				{
					m_bSelected = false;
					m_bSelectable = false;
				}

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the tmaxOption properties", Ex);
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
				strElementName = m_strText;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(m_oValue != null)
					{
						if(AddAttribute(xmlElement, XML_OPTION_ATTRIBUTE_VALUE, m_oValue.ToString()) == false)
							break;
					}
					else
					{
						if(AddAttribute(xmlElement, XML_OPTION_ATTRIBUTE_VALUE, "") == false)
							break;
					}
						
					if(m_bSelectable == true)
					{
						if(AddAttribute(xmlElement, XML_OPTION_ATTRIBUTE_SELECTED, BoolToXml(m_bSelected)) == false)
							break;
					}
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Object assigned to this item</summary>
		public object Value
		{
			get { return m_oValue; }
			set { m_oValue = value; }
		}

		/// <summary>Text displayed for this item</summary>
		public string Text
		{
			get { return m_strText; }
			set { m_strText = value; }
		}

		/// <summary>Indicates if this option is selected by the user</summary>
		public bool Selected
		{
			get { return m_bSelected; }
			set { m_bSelected = value; }
		}

		/// <summary>True if this is a selectable property</summary>
		public bool Selectable
		{
			get { return m_bSelectable; }
			set { m_bSelectable = value; }
		}

		#endregion Properties
		
	}//	public class CTmaxOption
	
	/// <summary>This class is used to manage a dynamic array of CTmaxOption objects</summary>
	public class CTmaxOptions : System.Collections.ArrayList, IXmlObject
	{
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		protected CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Multiple property</summary>
		private bool m_bMultiple = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxOptions() : base()
		{
		}
		
		/// <summary>Adds a new option to the array</summary>
		/// <param name="oValue">Value assigned to the new option</param>
		/// <param name="strText">Display text for the new option</param>
		public CTmaxOption Add(object oValue, string strText)
		{
			CTmaxOption tmaxOption = new CTmaxOption(oValue, strText);
			Add(tmaxOption);
			return tmaxOption;
		}
		
		/// <summary>Adds a new option to the array</summary>
		/// <param name="oValue">Value assigned to the new option</param>
		/// <param name="strText">Display text for the new option</param>
		/// <param name="bSelected">Flag to set the Selected state</param>
		public CTmaxOption Add(object oValue, string strText, bool bSelected)
		{
			CTmaxOption tmaxOption = new CTmaxOption(oValue, strText, bSelected);
			Add(tmaxOption);
			return tmaxOption;
		}
		
		/// <summary>Called to find the object with the specified value</summary>
		/// <param name="oValue">The value assigned to the option</param>
		/// <returns>The object with the specified value if found</returns>
		public CTmaxOption Find(object oValue)
		{
			foreach(CTmaxOption tmaxOption in this)
			{
				//	Compare the strings because these should be enumerations
				if(tmaxOption.Value.ToString() == oValue.ToString())
					return tmaxOption;
				
			}// foreach(CTmaxOption tmaxOption in this)
			
			return null;
			
		}// public CTmaxOption Find(object oValue)
		
		/// <summary>Called to find the object with the specified text</summary>
		/// <param name="strText">The text assigned to the option</param>
		/// <returns>The object with the specified text if found</returns>
		public CTmaxOption Find(string strText)
		{
			foreach(CTmaxOption tmaxOption in this)
			{
				if(tmaxOption.Text == strText)
					return tmaxOption;
				
			}// foreach(CTmaxOption tmaxOption in this)
			
			return null;
			
		}// public CTmaxOption Find(string strText)
		
		/// <summary>Called to set the selection state of the specified object</summary>
		/// <param name="oValue">The value of the option to be selected</param>
		/// <param name="bSelected">true to select the object</param>
		public void SetSelected(object oValue, bool bSelected)
		{
			foreach(CTmaxOption tmaxOption in this)
			{
				//	Compare the strings because these should be enumerations
				if(tmaxOption.Value.ToString() == oValue.ToString())
				{
					tmaxOption.Selected = bSelected;
					
					//	Stop here if we don't have to clear the other objects
					if((bSelected == false) || (m_bMultiple == true))
						break;
				}
				else
				{
					//	Should we clear this selection?
					if((bSelected == true) && (m_bMultiple == false))
						tmaxOption.Selected = false;
				}
				
			}// foreach(CTmaxOption tmaxOption in this)
			
		}// public void SetSelected(object oValue, bool bSelected)
		
		/// <summary>Called to set the selection state of the specified object</summary>
		/// <param name="oValue">The value of the option to be selected</param>
		public void SetSelected(object oValue)
		{
			SetSelected(oValue, true);
			
		}// public void SetSelected(object oValue)
		
		/// <summary>Called to determine if the specified object has been selected</summary>
		/// <param name="oValue">The value of the option to be checked</param>
		public bool GetSelected(object oValue)
		{
			foreach(CTmaxOption tmaxOption in this)
			{
				//	Compare the strings because these should be enumerations
				if(tmaxOption.Value.ToString() == oValue.ToString())
				{
					return tmaxOption.Selected;
				}
				
			}// foreach(CTmaxOption tmaxOption in this)
			
			return false;
			
		}// public bool GetSelected(object oValue)
		
		/// <summary>Called to locate the selected object</summary>
		/// <returns>The value of the selected object</returns>
		public CTmaxOption GetSelection()
		{
			foreach(CTmaxOption tmaxOption in this)
			{
				//	Compare the strings because these should be enumerations
				if(tmaxOption.Selected == true)
				{
					return tmaxOption;
				}
				
			}// foreach(CTmaxOption tmaxOption in this)
			
			return null;
			
		}// public CTmaxOption GetSelection()
		
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
			XmlElement	xmlOptions  = null;
			XmlElement	xmlOption = null;
			string		strElementName = "";
			
			//	What name are we going to use for this element?
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = m_strName;
			
			//	Create the top-level node
			if((xmlOptions = xmlDocument.CreateElement(strElementName)) == null)
				return null;

			//	Add each of the options
			foreach(CTmaxOption O in this)
			{
				if((xmlOption = (XmlElement)(O.ToXmlNode(xmlDocument))) != null)
					xmlOptions.AppendChild(xmlOption);
			}
			
			return xmlOptions;
		
		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		/// <summary>This method initializes the collection using the specified node</summary>
		/// <param name="xmlNode">The XML node containing the options</param>
		///	<returns>true if successful</returns>
		public virtual bool FromXmlNode(System.Xml.XmlNode xmlNode)
		{
			CTmaxOption	tmaxOption = null;
			
			//	Use the element name to name this option collection
			m_strName = xmlNode.Name;
			
			//	Now get each child option
			foreach(XmlNode O in xmlNode.ChildNodes)
			{
				if(tmaxOption == null)
				{
					tmaxOption = new CTmaxOption();
					tmaxOption.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
					tmaxOption.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
				}
				
				//	Use the node to set the option properties
				if(tmaxOption.SetProperties(O) == true)
				{
					this.Add(tmaxOption);	//	Add to this collection
					tmaxOption = null;		//	Allocate a new object
				}
				
			}// foreach(XmlNode O in xmlNode.ChildNodes)
			
			return true;
		
		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Name assigned to the collection</summary>
		public string Name
		{
			get	{ return m_strName; }
			set { m_strName = value; }
		}

		/// <summary>Allow multiple selections</summary>
		public bool Multiple
		{
			get	{ return m_bMultiple; }
			set { m_bMultiple = value; }
		}

		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource;	}
		}
		
		#endregion Properties
		
	}//	public class CTmaxOptions
	
	
}// namespace FTI.Shared.Trialmax
