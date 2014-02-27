using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML objection node</summary>
	public class CXmlObjection : CXmlBase
	{
		#region Constants

		public const string XML_OBJECTION_ELEMENT_NAME = "objection";

		public const string XML_OBJECTION_ATTRIBUTE_UNIQUE_ID		= "id";
		public const string XML_OBJECTION_ATTRIBUTE_FIRST_PL		= "firstPL";
		public const string XML_OBJECTION_ATTRIBUTE_LAST_PL			= "lastPL";
		public const string XML_OBJECTION_ATTRIBUTE_PLAINTIFF		= "byPlaintiff";
		public const string XML_OBJECTION_ATTRIBUTE_STATE			= "status";
		public const string XML_OBJECTION_ATTRIBUTE_RULING			= "ruling";
		public const string XML_OBJECTION_ATTRIBUTE_ARGUMENT		= "objection";
		public const string XML_OBJECTION_ATTRIBUTE_RESPONSE_1		= "response";
		public const string XML_OBJECTION_ATTRIBUTE_RESPONSE_2		= "response2";
		public const string XML_OBJECTION_ATTRIBUTE_RESPONSE_3		= "response3";
		public const string XML_OBJECTION_ATTRIBUTE_RULING_TEXT		= "rulingText";
		public const string XML_OBJECTION_ATTRIBUTE_WORK_PRODUCT	= "workProduct";
		public const string XML_OBJECTION_ATTRIBUTE_COMMENTS		= "comment";
		public const string XML_OBJECTION_ATTRIBUTE_MODIFIED_ON		= "modifiedOn";
		public const string XML_OBJECTION_ATTRIBUTE_MODIFIED_BY		= "modifiedBy";

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to TmaxObjection property</summary>
		private FTI.Shared.Trialmax.CTmaxObjection m_tmaxObjection = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CXmlObjection()
		{
		}

		/// <summary>Constructor</summary>
		/// <param name="tmaxObjection">Application objection used to initialize the object</param>
		public CXmlObjection(CTmaxObjection tmaxObjection)
		{
			if(tmaxObjection != null)
				SetTmaxObjection(tmaxObjection);
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source node to be copied</param>
		public CXmlObjection(CXmlObjection xmlSource) : base()
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
				CXmlObjection xmlObjection = (CXmlObjection)xmlBase;

				if(this.FirstPL == xmlObjection.FirstPL)
				{
					if(this.LastPL == xmlObjection.LastPL)
						return 0;
					else
						return ((this.LastPL < xmlObjection.LastPL) ? -1 : 1);
				}
				else
				{
					return ((this.FirstPL < xmlObjection.FirstPL) ? -1 : 1);
				}

			}
			catch
			{
				return -1;
			}

		}// virtual public int Compare(CXmlBase xmlBase)

		/// <summary>Called to copy the properties of the source node</summary>
		public void Copy(CXmlObjection xmlSource)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
			
			if(xmlSource.m_tmaxObjection != null)
				m_tmaxObjection = new CTmaxObjection(xmlSource.m_tmaxObjection);
			else
				m_tmaxObjection = null;
			
		}// public void Copy(CXmlObjection xmlSource)

		/// <summary>This method is called to get the application object bound to this record</summary>
		/// <returns>The application object</returns>
		public FTI.Shared.Trialmax.CTmaxObjection GetTmaxObjection()
		{
			//	Do we need to allocate the object?
			if(m_tmaxObjection == null)
				m_tmaxObjection = new CTmaxObjection();

			return m_tmaxObjection;

		}// public FTI.Shared.Trialmax.CTmaxObjection GetTmaxObjection()

		/// <summary>This method is called to set the application object bound to this record</summary>
		/// <param name="tmaxObjection">The application object</param>
		public void SetTmaxObjection(FTI.Shared.Trialmax.CTmaxObjection tmaxObjection)
		{
			m_tmaxObjection = tmaxObjection;

		}// public void SetTmaxObjection(FTI.Shared.Trialmax.CTmaxObjection tmaxObjection)

		/// <summary>This method uses the specified navigator to set the object properties</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string	strAttribute = "";
			bool	bSuccessful = false;

			Debug.Assert(xpNavigator != null);
			if(xpNavigator == null) return false;

			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_UNIQUE_ID, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.UniqueId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_FIRST_PL, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.FirstPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_LAST_PL, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.LastPL = System.Convert.ToInt64(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_PLAINTIFF, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Plaintiff = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_STATE, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.State = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_RULING, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Ruling = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_ARGUMENT, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Argument = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_RESPONSE_1, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Response1 = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_RESPONSE_2, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Response2 = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_RESPONSE_3, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Response3 = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_RULING_TEXT, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.RulingText = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_WORK_PRODUCT, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.WorkProduct = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_COMMENTS, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Comments = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_MODIFIED_ON, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					try { this.ModifiedOn = System.Convert.ToDateTime(strAttribute); }
					catch { this.ModifiedOn = System.DateTime.Now; }
				}

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTION_ATTRIBUTE_MODIFIED_BY, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.ModifiedBy = strAttribute;

				//	Just in case ...
				if(this.LastPL < this.FirstPL)
					this.LastPL = this.FirstPL;
						
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML objection line properties", Ex);
			}
			
			return bSuccessful;

		}// public bool SetProperties(XPathNavigator xpNavigator)

		/// <summary>This method uses the specified node to set the objection properties</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML objection properties", Ex);
				return false;
			}

		}// public bool SetProperties(XmlNode xmlNode)

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
				strElementName = XML_OBJECTION_ELEMENT_NAME;

			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_UNIQUE_ID, this.UniqueId) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_FIRST_PL, this.FirstPL) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_LAST_PL, this.LastPL) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_PLAINTIFF, BoolToXml(this.Plaintiff)) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_STATE, this.State) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_RULING, this.Ruling) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_ARGUMENT, this.Argument) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_RESPONSE_1, this.Response1) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_RESPONSE_2, this.Response2) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_RESPONSE_3, this.Response3) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_RULING_TEXT, this.RulingText) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_WORK_PRODUCT, this.WorkProduct) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_COMMENTS, this.Comments) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_MODIFIED_ON, this.ModifiedOn) == false)
						break;

					if(AddAttribute(xmlElement, XML_OBJECTION_ATTRIBUTE_MODIFIED_BY, this.ModifiedBy) == false)
						break;

					//	We're done
					bSuccessful = true;

				}// while(bSuccessful == false)

			}// if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)

			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)

		#endregion Public Methods

		#region Properties

		/// <summary>The application object bound to this object</summary>
		public FTI.Shared.Trialmax.CTmaxObjection TmaxObjection
		{
			get { return GetTmaxObjection(); }
			set { SetTmaxObjection(value); }
		}

		/// <summary>The unique identifier assigned by the database</summary>
		public string UniqueId
		{
			get { return this.TmaxObjection.UniqueId; }
			set { this.TmaxObjection.UniqueId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Cases table</summary>
		public string CaseId
		{
			get { return this.TmaxObjection.CaseId; }
			set { this.TmaxObjection.CaseId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Depositions table</summary>
		public string DepositionId
		{
			get { return this.TmaxObjection.DepositionId; }
			set { this.TmaxObjection.DepositionId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the States table</summary>
		public string StateId
		{
			get { return this.TmaxObjection.StateId; }
			set { this.TmaxObjection.StateId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Rulings table</summary>
		public string RulingId
		{
			get { return this.TmaxObjection.RulingId; }
			set { this.TmaxObjection.RulingId = value; }
		}

		/// <summary>The unique identifier assigned by the associated record in the Users table</summary>
		public string ModifiedById
		{
			get { return this.TmaxObjection.ModifiedById; }
			set { this.TmaxObjection.ModifiedById = value; }
		}

		/// <summary>The MediaId of the deposition</summary>
		public string Deposition
		{
			get { return this.TmaxObjection.Deposition; }
			set { this.TmaxObjection.Deposition = value; }
		}

		/// <summary>The State of the objection</summary>
		public string State
		{
			get { return this.TmaxObjection.State; }
			set { this.TmaxObjection.State = value; }
		}

		/// <summary>The Ruling of the objection</summary>
		public string Ruling
		{
			get { return this.TmaxObjection.Ruling; }
			set { this.TmaxObjection.Ruling = value; }
		}

		/// <summary>True if objection raised by the plaintiff</summary>
		public bool Plaintiff
		{
			get { return this.TmaxObjection.Plaintiff; }
			set { this.TmaxObjection.Plaintiff = value; }
		}

		/// <summary>The description of the objection</summary>
		public string Argument
		{
			get { return this.TmaxObjection.Argument; }
			set { this.TmaxObjection.Argument = value; }
		}

		/// <summary>The first response</summary>
		public string Response1
		{
			get { return this.TmaxObjection.Response1; }
			set { this.TmaxObjection.Response1 = value; }
		}

		/// <summary>The second response</summary>
		public string Response2
		{
			get { return this.TmaxObjection.Response2; }
			set { this.TmaxObjection.Response2 = value; }
		}

		/// <summary>The third response</summary>
		public string Response3
		{
			get { return this.TmaxObjection.Response3; }
			set { this.TmaxObjection.Response3 = value; }
		}

		/// <summary>The text that accompanies the official ruling</summary>
		public string RulingText
		{
			get { return this.TmaxObjection.RulingText; }
			set { this.TmaxObjection.RulingText = value; }
		}

		/// <summary>The work product description</summary>
		public string WorkProduct
		{
			get { return this.TmaxObjection.WorkProduct; }
			set { this.TmaxObjection.WorkProduct = value; }
		}

		/// <summary>The user comments</summary>
		public string Comments
		{
			get { return this.TmaxObjection.Comments; }
			set { this.TmaxObjection.Comments = value; }
		}

		/// <summary>The first page/line of transcript text</summary>
		public long FirstPL
		{
			get { return this.TmaxObjection.FirstPL; }
			set { this.TmaxObjection.FirstPL = value; }
		}

		/// <summary>The last page/line of transcript text</summary>
		public long LastPL
		{
			get { return this.TmaxObjection.LastPL; }
			set { this.TmaxObjection.LastPL = value; }
		}

		/// <summary>The date the record was last modified</summary>
		public System.DateTime ModifiedOn
		{
			get { return this.TmaxObjection.ModifiedOn; }
			set { this.TmaxObjection.ModifiedOn = value; }
		}

		/// <summary>The name of the user to last modify this record</summary>
		public string ModifiedBy
		{
			get { return this.TmaxObjection.ModifiedBy; }
			set { this.TmaxObjection.ModifiedBy = value; }
		}

		#endregion Properties

	}// public class CXmlObjection

	/// <summary>Objects of this class are used to manage a dynamic array of CXmlObjection objects</summary>
	public class CXmlObjections : CTmaxSortedArrayList, IXmlObject
	{
		#region Constants

		public const string XML_OBJECTIONS_ELEMENT_NAME = "objections";

		public const string XML_OBJECTIONS_ATTRIBUTE_CASE_NAME		= "case";
		public const string XML_OBJECTIONS_ATTRIBUTE_CASE_ID		= "caseID";
		public const string XML_OBJECTIONS_ATTRIBUTE_CASE_VERSION	= "caseVersion";

		#endregion Constants

		#region Private Members

		/// <summary>Class member bound to the CaseName property</summary>
		private string m_strCaseName = "";

		/// <summary>Class member bound to the CaseId property</summary>
		private string m_strCaseId = "";

		/// <summary>Class member bound to the CaseVersion property</summary>
		private string m_strCaseVersion = "";

		#endregion Private Members

		#region Public Members

		/// <summary>Default constructor</summary>
		public CXmlObjections() : base()
		{
			//	Assign a default sorter
			this.Comparer = new CXmlBaseSorter();
			this.KeepSorted = false;
		}

		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">The collection to be copied</param>
		public CXmlObjections(CXmlObjections xmlSource) : base()
		{
			//	Assign a default sorter
			this.Comparer = new CXmlBaseSorter();
			this.KeepSorted = false;
			
			if(xmlSource != null)
				Copy(xmlSource);

		}// public CXmlObjections(CXmlObjections xmlSource) : base()		
		
		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="xmlObjection">CXmlObjection object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlObjection Add(CXmlObjection xmlObjection)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlObjection as object);

				return xmlObjection;
			}
			catch
			{
				return null;
			}

		}// Add(CXmlObjection xmlObjection)

		/// <summary>This method is called to remove the requested filter from the collection</summary>
		/// <param name="xmlObjection">The object to be removed</param>
		public void Remove(CXmlObjection xmlObjection)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlObjection as object);
			}
			catch
			{
			}

		}// public void Remove(CXmlObjection xmlObjection)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlObjection">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlObjection xmlObjection)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlObjection as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CXmlObjection this[int index]
		{
			// Use base class to process actual collection operation
			get
			{
				return (GetAt(index) as CXmlObjection);
			}

		}// public new CXmlObjection this[int index]

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlObjection value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>This method is called to copy the source collection specified by the caller</summary>
		/// <param name="xmlSource">The source collection to be copied</param>
		public void Copy(CXmlObjections xmlSource)
		{
			//	Flush the existing contents
			this.Clear();
			
			//	Copy the case information
			this.CaseName = xmlSource.CaseName;
			this.CaseId = xmlSource.CaseId;
			this.CaseVersion = xmlSource.CaseVersion;

			//	Copy the source collection
			foreach(CXmlObjection O in xmlSource)
				this.Add(new CXmlObjection(O));

		}// public void Copy(CXmlObjections xmlSource)

		/// <summary>Called to reset the properties and clear the collection</summary>
		public void Reset()
		{
			this.CaseName = "";
			this.CaseId = "";
			this.CaseVersion = "";

			//	Flush the collection
			Clear();

		}// public override void Clear()

		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		///	<returns>An Xml node that represents the object</returns>
		public XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument, "");
		}
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement = null;
			XmlNode		xmlChild = null;
			CXmlBase	xmlBase = null;
			bool		bSuccessful = false;
			string		strElementName = "";

			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_OBJECTIONS_ELEMENT_NAME;

			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					//	Allocate a base class XML object to provide access to methods for document management
					xmlBase = new CXmlBase();

					if(this.CaseName.Length > 0)
					{
						if(xmlBase.AddAttribute(xmlElement, XML_OBJECTIONS_ATTRIBUTE_CASE_NAME, this.CaseName) == false)
							break;
					}

					if(this.CaseId.Length > 0)
					{
						if(xmlBase.AddAttribute(xmlElement, XML_OBJECTIONS_ATTRIBUTE_CASE_ID, this.CaseId) == false)
							break;
					}

					if(this.CaseVersion.Length > 0)
					{
						if(xmlBase.AddAttribute(xmlElement, XML_OBJECTIONS_ATTRIBUTE_CASE_VERSION, this.CaseVersion) == false)
							break;
					}

					foreach(CXmlObjection O in this)
					{
						if((xmlChild = O.ToXmlNode(xmlDocument)) != null)
						{
							xmlElement.AppendChild(xmlChild);
						}
					}

					//	We're done
					bSuccessful = true;

				}// while(1)

			}

			return (bSuccessful == true) ? xmlElement : null;

		}// public XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)

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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML objection collection properties", Ex);
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
				strAttribute = xpNavigator.GetAttribute(XML_OBJECTIONS_ATTRIBUTE_CASE_NAME, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCaseName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTIONS_ATTRIBUTE_CASE_ID, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCaseId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_OBJECTIONS_ATTRIBUTE_CASE_VERSION, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCaseVersion = strAttribute;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML objection collection properties", Ex);
				return false;
			}

		}// public bool SetProperties(XPathNavigator xpNavigator)

		/// <summary>This method is called to populate the collection</summary>
		/// <param name="xpDocument">The XPath document containing the objection nodes</param>
		/// <param name="strDepositionPath">The path to the parent deposition node</param>
		/// <returns>true if successful</returns>
		public bool GetObjections(XPathDocument xpDocument, string strDepositionPath)
		{
			XPathNavigator xpNavigator = null;
			XPathNodeIterator xpIterator = null;
			string strPath = "";

			try
			{
				//	Clear the collection
				this.Clear();

				if((strDepositionPath != null) && (strDepositionPath.Length > 0))
					strPath = String.Format("{0}/{1}[@{2}=\"{3}\"]/{4}", strDepositionPath, XML_OBJECTIONS_ELEMENT_NAME, XML_OBJECTIONS_ATTRIBUTE_CASE_ID, this.CaseId, CXmlObjection.XML_OBJECTION_ELEMENT_NAME);
				else
					strPath = String.Format("{0}/{1}/{2}[@{3}=\"{4}\"]/{5}", CXmlDeposition.XML_DEPOSITION_ROOT_NAME, CXmlDeposition.XML_DEPOSITION_ELEMENT_NAME, XML_OBJECTIONS_ELEMENT_NAME, XML_OBJECTIONS_ATTRIBUTE_CASE_ID, this.CaseId, CXmlObjection.XML_OBJECTION_ELEMENT_NAME);

				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strPath)) == null) return false;

				return GetObjections(xpIterator);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetObjections", "An exception was raised while attempting to populate the objections collection", Ex);
				return false;
			}

		}// public bool GetObjections(XPathDocument xpDocument, string strDepositionPath)

		/// <summary>This method is called to populate the transcripts collection</summary>
		/// <param name="xpIterator">The XPath iterator containing the transcript nodes</param>
		/// <returns>true if successful</returns>
		public bool GetObjections(XPathNodeIterator xpIterator)
		{
			CXmlObjection xmlObjection = null;

			try
			{
				//	Clear the collection
				this.Clear();
				this.KeepSorted = false;

				//	Add an object for each node
				while(xpIterator.MoveNext() == true)
				{
					xmlObjection = new CXmlObjection();
					m_tmaxEventSource.Attach(xmlObjection.EventSource);

					if(xmlObjection.SetProperties(xpIterator.Current) == true)
						this.Add(xmlObjection);

				}// while(xpIterator.MoveNext() == true)

				this.Sort();

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetObjections", "An exception was raised while attempting to populate the objections collection", Ex);
				return false;
			}

		}// public bool GetObjections(XPathNodeIterator xpIterator)

		#endregion Public Methods

		#region Properties

		/// <summary>The name of the TrialMax case associated with the objection</summary>
		public string CaseName
		{
			get { return m_strCaseName; }
			set { m_strCaseName = value; }
		}

		/// <summary>The unique identifier of the TrialMax case associated with the objection</summary>
		public string CaseId
		{
			get { return m_strCaseId; }
			set { m_strCaseId = value; }
		}

		/// <summary>The version of the TrialMax case associated with the objection</summary>
		public string CaseVersion
		{
			get { return m_strCaseVersion; }
			set { m_strCaseVersion = value; }
		}

		#endregion Properties
		
	}//	public class CXmlObjections

}// namespace FTI.Shared.Xml
