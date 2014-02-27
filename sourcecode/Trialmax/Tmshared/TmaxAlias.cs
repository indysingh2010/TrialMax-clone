using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information required to register a source file</summary>
	public class CTmaxAlias : CXmlBase, ITmaxSortable
	{
		#region Constants
		
		private const string XML_ALIAS_ATTRIBUTE_ID			= "id";
		private const string XML_ALIAS_ATTRIBUTE_CURRENT	= "current";
		private const string XML_ALIAS_ATTRIBUTE_ORIGINAL	= "original";
		private const string XML_ALIAS_ATTRIBUTE_PREVIOUS	= "previous";

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Id property</summary>
		private long m_lId = 0;
		
		/// <summary>Local member bound to Original property</summary>
		private string m_strOriginal = "";
		
		/// <summary>Local member bound to Previous property</summary>
		private string m_strPrevious = "";
		
		/// <summary>Local member bound to Current property</summary>
		private string m_strCurrent = "";
		
		/// <summary>Local member bound to Editor property</summary>
		private string m_strEditor = "";
		
		/// <summary>Local member bound to UserDefined property</summary>
		private object m_userDefined = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxAlias() : base()
		{
		}
	
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">Source object to be copied</param>
		public CTmaxAlias(CTmaxOption tmaxSource) : base()
		{
			if(tmaxSource != null)
				Copy(tmaxSource);
		}
		
	/// <summary>Default constructor</summary>
		/// <param name="lId">The alias identifier</param>
		/// <param name="strOriginal">The drive/server specification used to create the alias</param>
		public CTmaxAlias(string strOriginal)
		{
			Initialize(strOriginal);
			
		}// public CTmaxAlias(string strOriginal)
	
		/// <summary>Default constructor</summary>
		/// <param name="lId">The alias identifier</param>
		public CTmaxAlias(long lId)
		{
			Initialize(lId);
			
		}// public CTmaxAlias(long lId, string strOriginal)
	
		/// <summary>Default constructor</summary>
		/// <param name="lId">The alias identifier</param>
		/// <param name="strOriginal">The drive/server specification used to create the alias</param>
		public CTmaxAlias(long lId, string strOriginal)
		{
			Initialize(lId, strOriginal);
			
		}// public CTmaxAlias(long lId, string strOriginal)
	
		/// <summary>Called to copy the properties of the source object</summary>
		/// <param name="tmaxSource">The object whose properties are to be copied</param>
		public void Copy(CTmaxAlias tmaxSource)
		{
			//	Copy the base class members
			base.Copy(tmaxSource as CXmlBase);
			
			//	Copy the object properties
			Original = tmaxSource.Original;
			Current = tmaxSource.Current;
			Previous = tmaxSource.Previous;
			Editor = tmaxSource.Editor;
			Modified = tmaxSource.Modified;
			Id = tmaxSource.Id;
			UserDefined = tmaxSource.UserDefined;
			
		}// public void Copy(CTmaxAlias tmaxSource)
		
		/// <summary>This method initializes the alias using the specified values</summary>
		/// <param name="lId">The alias identifier</param>
		/// <param name="strOriginal">The drive/server specification used to create the alias</param>
		public void Initialize(long lId, string strOriginal)
		{
			m_lId = lId;
			
			if(strOriginal != null)
				m_strOriginal = strOriginal;
			else
				m_strOriginal = "";
				
			m_strCurrent = m_strOriginal;
			m_strPrevious = m_strOriginal;

		}// public void Initialize(long lId, string strOriginal)
	
		/// <summary>This method initializes the alias using the specified values</summary>
		/// <param name="lId">The alias identifier</param>
		public void Initialize(long lId)
		{
			Initialize(lId, "");

		}// public void Initialize(long lId)
	
		/// <summary>This method initializes the alias using the specified values</summary>
		/// <param name="strOriginal">The drive/server specification used to create the alias</param>
		public void Initialize(string strOriginal)
		{
			Initialize(0, strOriginal);

		}// public void Initialize(string strOriginal)
	
		/// <summary>This method gets the default text representation of the alias</summary>
		/// <returns>The text representation of the alias</returns>
		public override string ToString()
		{
			string strString = "";
			
			//	Strip the trailing backslash for display purposes
			if(m_strCurrent.EndsWith("\\"))
				strString = m_strCurrent.Substring(0, m_strCurrent.Length - 1);
			else
				strString = m_strCurrent;
				
			return strString;

		}// public override string ToString()
	
		/// <summary>This function is called to compare the specified file to this file</summary>
		/// <param name="tmaxAlias">The alias object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this alias less than O, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxAlias tmaxAlias, long lMode)
		{
			if(lMode != 0)
			{
				return String.Compare(Current, tmaxAlias.Current, true);
			}
			else
			{
				//	Compare the identifiers
				if(this.Id > tmaxAlias.Id)
					return 1;
				else if(this.Id == tmaxAlias.Id)
					return 0;
				else
					return -1;
			}
		
		}// public int Compare(CTmaxAlias tmaxAlias)
		
		/// <summary>This method is required to support the ITmaxSortable interface</summary>
		/// <param name="O">The alias object to be compared</param>
		/// <param name="lMode">User defined sort mode identifier</param>
		/// <returns>-1 if this alias less than O, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try
			{
				return Compare((CTmaxAlias)O, lMode);
			}
			catch
			{
				return -1;
			}
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method will convert the specified path to an alias</summary>
		/// <param name="strPath">The path to be converted</param>
		/// <param name="rAlias">The alias to be returned</param>
		/// <returns>true if successful</returns>
		static public bool PathToAlias(string strPath, ref string rAlias)
		{
			string	strAlias = "";
			char[]	aDrives = {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
			bool	bReturn = false;
			
			//	Is this a rooted path?
			try
			{
				if((strPath == null) || (strPath.Length == 0)) return false;
				
				//	Is the user attempting to specify a drive
				if(strPath.Length == 1)
				{
					//	Must be a valid drive letter
					if(strPath.IndexOfAny(aDrives) >= 0)
					{
						strAlias = (strPath + ":\\");
						bReturn = true;
					}
					
				}
				else
				{
					//	Must be a rooted path
					if(System.IO.Path.IsPathRooted(strPath) == false) return false;
				
					//	Get the path root
					strAlias = (System.IO.Path.GetPathRoot(strPath)).ToLower();
					if(strAlias.Length == 0) return false;
						
					if(strAlias.EndsWith("\\") == false)
						strAlias += "\\";
					
					bReturn = true;
				}
				
			}
			catch
			{
			}
			
			if(bReturn == true)
				rAlias = strAlias.ToLower();
				
			return bReturn;

		}// static public bool PathToAlias(string strPath, ref string rAlias)
		
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the TmaxAlias properties", Ex);
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
				strAttribute = xpNavigator.GetAttribute(XML_ALIAS_ATTRIBUTE_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					Id = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_ALIAS_ATTRIBUTE_CURRENT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					Current = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_ALIAS_ATTRIBUTE_ORIGINAL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					Original = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_ALIAS_ATTRIBUTE_PREVIOUS,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					Previous = strAttribute;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the TmaxAlias properties", Ex);
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
				strElementName = "Alias";
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_ALIAS_ATTRIBUTE_ID, Id.ToString()) == false)
						break;

					if(AddAttribute(xmlElement, XML_ALIAS_ATTRIBUTE_CURRENT, Current) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_ALIAS_ATTRIBUTE_ORIGINAL, Original) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_ALIAS_ATTRIBUTE_PREVIOUS, Previous) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>This method is called to initialize the properties from the specified XML file</summary>
		/// <param name="xmlIni">The initialization file containing the alias descriptor</param>
		///	<param name="strKey">The key that identifies the line containing the descriptor</param>
		public void Load(CXmlIni xmlIni, string strKey)
		{
			Id = xmlIni.ReadLong(strKey, XML_ALIAS_ATTRIBUTE_ID, Id);
			Current = xmlIni.Read(strKey, XML_ALIAS_ATTRIBUTE_CURRENT, Current);
			Original = xmlIni.Read(strKey, XML_ALIAS_ATTRIBUTE_ORIGINAL, Original);
			Previous = xmlIni.Read(strKey, XML_ALIAS_ATTRIBUTE_PREVIOUS, Previous);

		}// public void Load(CXmlIni xmlIni, string strKey)
		
		/// <summary>This method is called to store the aliases in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the values</param>
		///	<param name="strKey">The key that identifies the line containing the descriptor</param>
		public void Save(CXmlIni xmlIni, string strKey)
		{
			xmlIni.Write(strKey, XML_ALIAS_ATTRIBUTE_ID, Id);
			xmlIni.Write(strKey, XML_ALIAS_ATTRIBUTE_CURRENT, Current);
			xmlIni.Write(strKey, XML_ALIAS_ATTRIBUTE_ORIGINAL, Original);
			xmlIni.Write(strKey, XML_ALIAS_ATTRIBUTE_PREVIOUS, Previous);

		}// public void Save(CXmlIni xmlIni, string strKey)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Identifier assigned to this alias</summary>
		public long Id
		{
			get { return m_lId; }
			set { m_lId = value; }
		
		}// Id property
		
		/// <summary>Original value for this alias</summary>
		public string Original
		{
			get { return m_strOriginal; }
			set { m_strOriginal = value; }
		
		}// Original property
		
		/// <summary>Previous value for this alias</summary>
		public string Previous
		{
			get { return m_strPrevious; }
			set { m_strPrevious = value; }
		
		}// Previous property
		
		/// <summary>Value assigned by the editor</summary>
		public string Editor
		{
			get { return m_strEditor; }
			set { m_strEditor = value; }
		
		}// Editor property
		
		/// <summary>Current value for this alias</summary>
		public string Current
		{
			get { return m_strCurrent; }
			set { m_strCurrent = value; }
		
		}// Current property
		
		/// <summary>User defined value</summary>
		public object UserDefined
		{
			get { return m_userDefined; }
			set { m_userDefined = value; }
		
		}// UserDefined property
		
		#endregion Properties
				
	}// public class CTmaxAlias

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxAlias objects
	/// </summary>
	public class CTmaxAliases : CTmaxSortedArrayList, IXmlObject
	{
		#region Constants
		
		private const int TMAX_ALIAS_SORT_ON_ID = 0;
		private const int TMAX_ALIAS_SORT_ON_CURRENT = 1;
		
		private const string XMLINI_SECTION_NAME = "trialMax/case/aliases";
		private const string XMLINI_ALIASES = "Aliases";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to the SortOnId property</summary>
		private bool m_bSortOnId = true;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxAliases() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter(TMAX_ALIAS_SORT_ON_CURRENT);
			this.KeepSorted = true;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxAlias">CTmaxAlias object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxAlias Add(CTmaxAlias tmaxAlias)
		{
			try
			{
				base.Add(tmaxAlias as object);
				return tmaxAlias;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxAlias tmaxAlias)

		/// <summary>This method will allocate, initialize, and add a new alias to the list</summary>
		/// <param name="strOriginal">The original value assigned to the alias</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxAlias Add(string strOriginal)
		{
			try
			{
				return Add(new CTmaxAlias(GetMaxId() + 1, strOriginal));
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxAlias Add(string strOriginal)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="tmaxAlias">The filter object to be removed</param>
		public void Remove(CTmaxAlias tmaxAlias)
		{
			try
			{
				base.Remove(tmaxAlias as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxAlias tmaxAlias)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxAlias">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxAlias tmaxAlias)
		{
			return base.Contains(tmaxAlias as object);
		
		}// public bool Contains(CTmaxAlias tmaxAlias)

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CTmaxAlias this[int index]
		{
			get { return (GetAt(index) as CTmaxAlias);	}
		
		}// public new CTmaxAlias this[int index]

		/// <summary>This method is called to locate the object with the specified id</summary>
		/// <returns>The specified object if found</returns>
		public CTmaxAlias Find(long lId)
		{
			foreach(CTmaxAlias O in this)
			{
				if(O.Id == lId)
					return O;
			}
			return null;
		
		}// public CTmaxAlias Find(long lId)

		/// <summary>This method is called to determine if any alias in the list has been modified</summary>
		/// <returns>true if modified</returns>
		public bool IsModifed()
		{
			foreach(CTmaxAlias O in this)
			{
				if(O.Modified == true)
					return true;
			}
			return false;
		
		}// public bool IsModifed()

		/// <summary>This method is called to locate the object with the specified current value</summary>
		/// <returns>The specified object if found</returns>
		public CTmaxAlias Find(string strCurrent)
		{
			foreach(CTmaxAlias O in this)
			{
				if(String.Compare(O.Current, strCurrent, true) == 0)
					return O;
			}
			return null;
		
		}// public CTmaxAlias Find(string strCurrent)

		/// <summary>This method is called to get the maximum Id value for all objects in the list</summary>
		/// <returns>The maximum Id value for all objects</returns>
		public long GetMaxId()
		{
			long lId = 0;
			
			foreach(CTmaxAlias O in this)
			{
				if(lId < O.Id)
					lId = O.Id;
			}
			
			return lId;
		
		}// public long GetMaxId()

		/// <summary>This method is called to get construct the complete path using the specifid alias identifier</summary>
		/// <param name="lAlias">The identifier of the drive/server alias</param>
		/// <param name="strPath">The path to append to the alias</param>
		/// <returns>The complete path</returns>
		public string GetAliasedPath(long lAlias, string strAppend)
		{
			string		strPath = "";
			CTmaxAlias	tmaxAlias = null;
			
			//	Locate the specified alias
			if((tmaxAlias = Find(lAlias)) != null)
			{
				strPath = tmaxAlias.Current;
				
				if(strPath.EndsWith("\\") == true)
				{
					if(strAppend.StartsWith("\\") == true)
						strPath += strAppend.Substring(1, strAppend.Length - 1);
					else
						strPath += strAppend;
				}
				else
				{
					if(strAppend.StartsWith("\\") == true)
						strPath += strAppend;
					else
						strPath += ("\\" + strAppend);
				}
			
			}// if((tmaxAlias = Find(lAlias)) != null)
			
			//	Make sure we have the trailing backslash
			if((strPath.Length > 0) && (strPath.EndsWith("\\") == false))
				strPath += "\\";
				
			return strPath;
		
		}// public string GetPath(long lAlias, string strAppend)

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
			XmlElement	xmlAliases  = null;
			XmlElement	xmlAlias = null;
			string		strElementName = "";
			
			//	What name are we going to use for this element?
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = "Aliases";
			
			//	Create the top-level node
			if((xmlAliases = xmlDocument.CreateElement(strElementName)) == null)
				return null;

			//	Now add child nodes for each alias in the collection
			foreach(CTmaxAlias O in this)
			{
				if((xmlAlias = (XmlElement)(O.ToXmlNode(xmlDocument))) != null)
				{
					xmlAliases.AppendChild(xmlAlias);
				}
				
			}
			
			return xmlAliases;
		
		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		/// <summary>This method initializes the collection using the specified node</summary>
		/// <param name="xmlNode">The XML node containing the options</param>
		///	<returns>true if successful</returns>
		public virtual bool FromXmlNode(System.Xml.XmlNode xmlNode)
		{
			CTmaxAlias tmaxAlias = null;
			
			//	Now get each child alias
			foreach(XmlNode O in xmlNode.ChildNodes)
			{
				if(tmaxAlias == null)
				{
					tmaxAlias = new CTmaxAlias();
					tmaxAlias.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
					tmaxAlias.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
				}
				
				//	Use the node to set the alias properties
				if(tmaxAlias.SetProperties(O) == true)
				{
					this.Add(tmaxAlias);	//	Add to this collection
					tmaxAlias = null;		//	Allocate a new object
				}
				
			}// foreach(XmlNode O in xmlNode.ChildNodes)
			
			return true;
		
		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		/// <summary>This method is called to populate the collection using values stored in the specified XML configuration file</summary>
		/// <param name="xmlIni">The initialization file containing the aliases</param>
		public void Load(CXmlIni xmlIni)
		{
			CTmaxAlias	tmaxAlias = null;
			string		strOldSection = "";
			string		strKey = "";
			long		lAliases = 0;
			
			//	Make sure the collection is empty
			this.Clear();
			
			//	Line up on the section that has the same name
			strOldSection = xmlIni.Section;
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == true)
			{
				//	How many descriptors are stored in the file
				lAliases = xmlIni.ReadLong(XMLINI_ALIASES, 0);
				
				//	Retrieve each alias stored in the file
				for(int i = 0; i < lAliases; i++)
				{
					//	Format the key for this alias
					strKey = String.Format("Alias{0}", (i + 1));
					
					//	Initialize a new alias
					tmaxAlias = new CTmaxAlias();
					tmaxAlias.Load(xmlIni, strKey);
					
					//	Add it to the collection
					this.Add(tmaxAlias);
					
				}// for(int i = 0; i < 10000; i++)
					
			}// if(xmlIni.SetSection(XMLINI_SECTION_NAME) == true)

			//	Restore the section
			if(strOldSection.Length > 0)
				xmlIni.SetSection(strOldSection);
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the aliases in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the values</param>
		public void Save(CXmlIni xmlIni)
		{
			string	strOldSection = "";
			string	strKey = "";
			
			//	Line up on the section that has the same name
			strOldSection = xmlIni.Section;
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == true)
			{
				//	Write the total number of aliases to the file
				xmlIni.Write(XMLINI_ALIASES, this.Count);
				
				//	Save each alias to the file
				for(int i = 0; i < this.Count; i++)
				{
					//	Format the key for this alias
					strKey = String.Format("Alias{0}", (i + 1));
					
					//	Save this alias
					this[i].Save(xmlIni, strKey);
				}
					
			}// if(xmlIni.SetSection(XMLINI_SECTION_NAME) == true)
			
			//	Restore the section
			if(strOldSection.Length > 0)
				xmlIni.SetSection(strOldSection);
			
		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>True to sort on id, False to sort on current value</summary>
		public bool SortOnId
		{
			get
			{
				return m_bSortOnId;
			}
			set
			{
				if(m_bSortOnId != value)
				{
					m_bSortOnId = value;
					
					if(m_bSortOnId == true)
						((CTmaxSorter)base.Comparer).Mode = TMAX_ALIAS_SORT_ON_ID;
					else
						((CTmaxSorter)base.Comparer).Mode = TMAX_ALIAS_SORT_ON_ID;
				
					//	Make sure the objects have been sorted
					if(m_bKeepSorted == true)
						this.Sort(true);
				
				}// if(m_bSortOnId != value)
				
			}
			
		}// SortOnId property						
		#endregion Properties
		
	}//	public class CTmaxAliases
		
}// namespace FTI.Shared.Trialmax
