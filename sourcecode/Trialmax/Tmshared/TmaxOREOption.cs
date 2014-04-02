using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to manage a user configurable option</summary>
	public class CTmaxOREOption : CXmlBase
	{
		#region Constants

		public const string ORE_OPTION_XML_ELEMENT_NAME			= "configure";
		public const string ORE_OPTION_XML_ATTRIBUTE_NAME		= "name";
		public const string ORE_OPTION_XML_ATTRIBUTE_VALUE		= "value";
		public const string ORE_OPTION_XML_ATTRIBUTE_ORE_LABEL	= "oreLabel";
		public const string ORE_OPTION_XML_ATTRIBUTE_ENABLED	= "enabled";
		public const string ORE_OPTION_XML_ATTRIBUTE_PARTY		= "party";

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to Value property</summary>
		private string m_strValue = "";

		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";

		/// <summary>Local member bound to ORELabel property</summary>
		private string m_strORELabel = "";

		/// <summary>Local member bound to Party property</summary>
		private string m_strParty = "";

		/// <summary>Local member bound to Enabled property</summary>
		private bool m_bEnabled = true;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxOREOption() : base()
		{
		}

		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">Source object to be copied</param>
		public CTmaxOREOption(CTmaxOREOption tmaxSource) : base()
		{
			if(tmaxSource != null)
				Copy(tmaxSource);
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		/// <param name="strORELabel">Label used to identify this option in the ORE setup file</param>
		/// <param name="bEnabled">True if enabled for the report</param>
		public CTmaxOREOption(string strName, string strValue, string strORELabel, bool bEnabled)
		{
			Initialize(strName, strValue, strORELabel, bEnabled);
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		/// <param name="strORELabel">Label used to identify this option in the ORE setup file</param>
		public CTmaxOREOption(string strName, string strValue, string strORELabel)
		{
			Initialize(strName, strValue, strORELabel);
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		public CTmaxOREOption(string strName, string strValue)
		{
			Initialize(strName, strValue);
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Display text for this option</param>
		public CTmaxOREOption(string strName)
		{
			Initialize(strName);
		}

		/// <summary>Called to copy the properties of the source object</summary>
		/// <param name="tmaxSource">The object whose properties are to be copied</param>
		public void Copy(CTmaxOREOption tmaxSource)
		{
			//	Copy the base class members
			base.Copy(tmaxSource as CXmlBase);

			this.Name = tmaxSource.Name;
			this.Value = tmaxSource.Value;
			this.ORELabel = tmaxSource.ORELabel;
			this.Enabled = tmaxSource.Enabled;
			this.Party = tmaxSource.Party;

		}// public void Copy(CTmaxOREOption tmaxSource)

		/// <summary>Overloaded base class method to return the string representation of this item</summary>
		/// <returns>The string representation of this item</returns>
		public override string ToString()
		{
			if(m_strName.Length > 0)
				return m_strName;
			else if(m_strORELabel.Length > 0)
				return m_strORELabel;
			else if(m_strValue != null)
				return m_strValue.ToString();
			else
				return "Unidentified ORE Option";

		}// public override string ToString()

		/// <summary>Called to initialize the property values</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		/// <param name="strORELabel">Label used to identify this option in the ORE setup file</param>
		/// <param name="bEnabled">True if enabled for the report</param>
		public void Initialize(string strName, string strValue, string strORELabel, bool bEnabled)
		{
			m_strName = strName;
			m_strValue = strValue;
			m_strORELabel = strORELabel;
			m_bEnabled = bEnabled;
			m_strParty = "";

		}// public void Initialize(string strName, string strValue, string strORELabel, bool bEnabled)

		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		/// <param name="strORELabel">Label used to identify this option in the ORE setup file</param>
		public void Initialize(string strName, string strValue, string strORELabel)
		{
			Initialize(strName, strValue, strORELabel, true);
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		public void Initialize(string strName, string strValue)
		{
			Initialize(strName, strValue, "", true);
		}

		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Display text for this option</param>
		public void Initialize(string strName)
		{
			Initialize(strName, "", "", true);
		}

		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement = null;
			bool		bSuccessful = false;
			string		strElementName = "";

			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = ORE_OPTION_XML_ELEMENT_NAME;

			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					//	Add the ORE identifier
					if(AddAttribute(xmlElement, ORE_OPTION_XML_ATTRIBUTE_NAME, GetORELabel()) == false)
						break;
						
					//	Add the value
					if(AddAttribute(xmlElement, ORE_OPTION_XML_ATTRIBUTE_VALUE, m_strValue) == false)
						break;

					//	Add the party if defined
					if(this.Party.Length > 0)
					{
						if(AddAttribute(xmlElement, ORE_OPTION_XML_ATTRIBUTE_PARTY, m_strParty) == false)
							break;
					}
					
					//	We're done
					bSuccessful = true;

				}// while(1)

			}

			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)

		/// <summary>Called to get the name assigned to the option</summary>
		/// <returns>The name used to identify the option</returns>
		public string GetName()
		{
			if(m_strName.Length > 0)
				return m_strName;
			else
				return m_strORELabel;

		}// public string GetName()

		/// <summary>Called to get the ORE label assigned to the option</summary>
		/// <returns>The text written to the ORE setup file to identify the option</returns>
		public string GetORELabel()
		{
			if(m_strORELabel.Length > 0)
				return m_strORELabel;
			else
				return m_strName;

		}// public string GetORELabel()

		/// <summary>This method is called to store the property values in the configuration file</summary>
		/// <param name="xmlIni">The initialization file to store the values</param>
		/// <param name="strKey">The key used to identify the line in the file</param>
		/// <returns>true if successful</returns>
		public bool Save(CXmlIni xmlIni, string strKey)
		{
			bool bSuccessful = false;
			
			try
			{
				xmlIni.Write(strKey, ORE_OPTION_XML_ATTRIBUTE_NAME, m_strName);
				xmlIni.Write(strKey, ORE_OPTION_XML_ATTRIBUTE_VALUE, m_strValue);
				xmlIni.Write(strKey, ORE_OPTION_XML_ATTRIBUTE_ORE_LABEL, m_strORELabel);
				xmlIni.Write(strKey, ORE_OPTION_XML_ATTRIBUTE_ENABLED, m_bEnabled ? "1" : "0");

				if(m_strParty.Length > 0)
					xmlIni.Write(strKey, ORE_OPTION_XML_ATTRIBUTE_PARTY, m_strParty);
					
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Save", Ex);
			}
			
			return bSuccessful;

		}// public bool Save(CXmlIni xmlIni, string strKey)
			
		/// <summary>This method is called to retrieve the property values stored in the configuration file</summary>
		/// <param name="xmlIni">The initialization file to store the values</param>
		/// <param name="strKey">The key used to identify the line in the file</param>
		/// <returns>true if successful</returns>
		public bool Load(CXmlIni xmlIni, string strKey)
		{
			try
			{
				//	Read the rest of the attributes
				m_strName = xmlIni.Read(strKey, ORE_OPTION_XML_ATTRIBUTE_NAME, "");
				m_strValue = xmlIni.Read(strKey, ORE_OPTION_XML_ATTRIBUTE_VALUE, "");
				m_strORELabel = xmlIni.Read(strKey, ORE_OPTION_XML_ATTRIBUTE_ORE_LABEL, "");
				m_strParty = xmlIni.Read(strKey, ORE_OPTION_XML_ATTRIBUTE_PARTY, "");
				m_bEnabled = xmlIni.ReadBool(strKey, ORE_OPTION_XML_ATTRIBUTE_ENABLED, true);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Load", Ex);
			}
			
			return (this.Name.Length > 0);

		}// public bool Load(CXmlIni xmlIni, string strKey)

		#endregion Public Methods

		#region Properties

		/// <summary>Value assigned to this option</summary>
		public string Value
		{
			get { return m_strValue; }
			set { m_strValue = value; }
		}

		/// <summary>Text displayed for this item</summary>
		public string Name
		{
			get { return GetName(); }
			set { m_strName = value; }
		}

		/// <summary>Text written to ORE setup file</summary>
		public string ORELabel
		{
			get { return GetORELabel(); }
			set { m_strORELabel = value; }
		}

		/// <summary>Party identifier written to ORE setup file</summary>
		public string Party
		{
			get { return m_strParty; }
			set { m_strParty = value; }
		}

		/// <summary>Indicates if this option is enabled for the report</summary>
		public bool Enabled
		{
			get { return m_bEnabled; }
			set { m_bEnabled = value; }
		}

		#endregion Properties

	}//	public class CTmaxOREOption

	/// <summary>This class is used to manage a dynamic array of CTmaxOREOption objects</summary>
	public class CTmaxOREOptions : System.Collections.ArrayList
	{
		#region Private Members

		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Name used to identify the options collection</summary>
		private string m_strName = "";
		
		#endregion Private Members
		
		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxOREOptions() : base()
		{
		}

		/// <summary>Adds a new option to the collection</summary>
		/// <param name="tmaxOREOption">The option to be added</param>
		/// <param name="bEnabled">True if enabled for the report</param>
		public CTmaxOREOption Add(CTmaxOREOption tmaxOREOption)
		{
			try
			{
				Debug.Assert((tmaxOREOption != null), "Attempt to add NULL ORE option");

				m_tmaxEventSource.Attach(tmaxOREOption.EventSource);

				this.Add(tmaxOREOption as object);
				
				return tmaxOREOption;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}

		}// public CTmaxOREOption Add(CTmaxOREOption tmaxOREOption)

		/// <summary>Adds a new option to the collection</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		/// <param name="strORELabel">Label used to identify this option in the ORE setup file</param>
		/// <param name="bEnabled">True if enabled for the report</param>
		public CTmaxOREOption Add(string strName, string strValue, string strORELabel, bool bEnabled)
		{
			try
			{
				return Add(new CTmaxOREOption(strName, strValue, strORELabel, bEnabled));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}

		}// public CTmaxOREOption Add(string strName, string strValue, string strORELabel, bool bEnabled)

		/// <summary>Adds a new option to the collection</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		/// <param name="strORELabel">Label used to identify this option in the ORE setup file</param>
		public CTmaxOREOption Add(string strName, string strValue, string strORELabel)
		{
			try
			{
				return Add(new CTmaxOREOption(strName, strValue, strORELabel));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}

		}// public CTmaxOREOption Add(string strName, string strValue, string strORELabel)

		/// <summary>Adds a new option to the collection</summary>
		/// <param name="strName">Display text for this option</param>
		/// <param name="strValue">Value assigned to this object</param>
		public CTmaxOREOption Add(string strName, string strValue)
		{
			try
			{
				return Add(new CTmaxOREOption(strName, strValue));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}

		}// public CTmaxOREOption Add(string strName, string strValue)

		/// <summary>Adds a new option to the collection</summary>
		/// <param name="strName">Display text for this option</param>
		public CTmaxOREOption Add(string strName)
		{
			try
			{
				return Add(new CTmaxOREOption(strName));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}

		}// public CTmaxOREOption Add(string strName)

		/// <summary>Called to find the object with the specified Name or ORELabel</summary>
		/// <param name="strText">The text assigned to the option</param>
		/// <param name="bORELabel">Test the ORELabel property if true, Name property if false</param>
		/// <returns>The object with the specified text if found</returns>
		public CTmaxOREOption Find(string strText, bool bORELabel)
		{
			foreach(CTmaxOREOption O in this)
			{
				if(bORELabel == true)
				{
					if(String.Compare(O.ORELabel, strText, true) == 0)
						return O;
				}
				else
				{
					if(String.Compare(O.Name, strText, true) == 0)
						return O;
				}

			}// foreach(CTmaxOREOption O in this)

			return null;

		}// public CTmaxOREOption Find(string strText, bool bORELabel)

		/// <summary>Called to remove the specified object from the collection</summary>
		/// <param name="O">The object to be removed</param>
		public void Remove(CTmaxOREOption O)
		{
			try { base.Remove(O as object); }
			catch { }
		}

		/// <summary>Called to determine if the specified object exists in the collection</summary>
		/// <param name="O">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxOREOption O)
		{
			return base.Contains(O as object);
		}

		/// <summary>Overloaded [] operator to locate the object with the specified name</summary>
		/// <param name="O">The name of the desired object</param>
		/// <returns>The object with the specified name</returns>
		public CTmaxOREOption this[string strName]
		{
			get { return Find(strName, false); }
		}

		/// <summary>Overloaded version of [] operator to return the object at the specified index</summary>
		/// <param name="index">The index of the desired object</param>
		/// <returns>The object at the specified index</returns>
		new public CTmaxOREOption this[int index]
		{
			get { return (base[index] as CTmaxOREOption); }
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="O">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxOREOption O)
		{
			return base.IndexOf(O as object);
		}

		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		public void Load(CXmlIni xmlIni)
		{
			int				iLine = 1;
			CTmaxOREOption	tmaxOption = null;
			string			strKey = "";

			//	NOTE:	We assume the file is already on the correct section

			//	Clear the collection
			this.Clear();

			//	Load all options stored in the file
			while(true)
			{
				tmaxOption = new CTmaxOREOption();

				strKey = GetXmlKey(iLine++);

				if(tmaxOption.Load(xmlIni, strKey) == true)
				{
					Add(tmaxOption);
				}
				else
				{
					break; // No more left in the file
				}

			}// while(true)

		}// public void Load(CXmlIni xmlIni)

		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the option values</param>
		public void Save(CXmlIni xmlIni)
		{
			int iLine = 1;

			//	NOTE:	We assume the file is already on the correct section
			foreach(CTmaxOREOption O in this)
				O.Save(xmlIni, GetXmlKey(iLine++));

		}// public void Save(CXmlIni xmlIni)

		/// <summary>Called to set the name assigned to the collection</summary>
		/// <param name="strName">The name to be assigned to the collection</param>
		public void SetName(string strName)
		{
			m_strName = strName;
			m_tmaxEventSource.Name = ("ORE " + m_strName + " Options");
		}
		
		#endregion Public Methods

		#region Private Methods

		/// <summary>This method is called to get the XML INI key identifier</summary>
		/// <param name="iLine">The line number</param>
		public string GetXmlKey(int iLine)
		{
			string strKey = String.Format("L{0}", iLine);
			return strKey;
		}

		#endregion Private Methods
		
		#region Properties

		/// <summary>Name assigned to the collection</summary>
		public string Name
		{
			get { return m_strName; }
			set { SetName(value); }
		}

		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		#endregion Properties

	}//	public class CTmaxOREOptions

}// namespace FTI.Shared.Trialmax
