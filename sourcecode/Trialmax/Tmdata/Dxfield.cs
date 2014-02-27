using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;

using FTI.Shared.Xml;

namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class manages the information associated with a single field in a record
	/// </summary>
	public class CDxField
	{
		#region Private Members
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Value property</summary>
		private object m_objValue = null;
		
		/// <summary>Local member bound to Index property</summary>
		private int m_iIndex = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CDxField()
		{
		}
	
		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Name used to identify the field</param>
		public CDxField(string strName)
		{
			m_strName = strName;
		}
	
		/// <summary>Overloaded constructor</summary>
		/// <param name="strName">Name used to identify the field</param>
		/// <param name="objValue">Value assigned to the field</param>
		public CDxField(string strName, object objValue)
		{
			m_strName = strName;
			m_objValue = objValue;
		}
		
		/// <summary>This method creates an xml node that represents the field</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			XmlElement xmlField = null;
				
			if((xmlField = xmlDocument.CreateElement(m_strName)) != null)
			{
				xmlField.SetAttribute("Value", Value.ToString());
			}
			
			return xmlField;

		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		#endregion Public Methods
	
		#region Properties
		
		/// <summary>Name of this field in the database</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>Value assigned to this field</summary>
		public object Value
		{
			get { return m_objValue; }
			set { m_objValue = value; }
		}	
		
		/// <summary>Index of the field in the table</summary>
		public int Index
		{
			get { return m_iIndex; }
			set { m_iIndex = value; }
		}	
		
		#endregion Properties

	}// public class CDxField
	
	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CDxField objects
	/// </summary>
	public class CDxFields : CollectionBase
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CDxFields()
		{
		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="dxField">CDxField object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CDxField Add(CDxField dxField)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(dxField as object);

				return dxField;
			}
			catch
			{
				return null;
			}
			
		}// Add(CDxField dxField)

		/// <summary>
		/// This method is called to remove the requested filter from the collection
		/// </summary>
		/// <param name="dxField">The filter object to be removed</param>
		public void Remove(CDxField dxField)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(dxField as object);
			}
			catch
			{
			}
		}

		/// <summary>Called to remove the object with the specified name</summary>
		/// <returns>The object with the specified name</returns>
		public void Remove(string strName)
		{
			CDxField dxField = null;
			
			if((dxField = Find(strName)) != null)
				Remove(dxField);				

		}//	public void Remove(string strName)

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="dxField">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CDxField dxField)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(dxField as object);
		}

		/// <summary>Called to locate the object with the specified name</summary>
		/// <returns>The object with the specified name</returns>
		public CDxField Find(string strName)
		{
			// Search for the object with the same name
			foreach(CDxField O in base.List)
			{
				if(String.Compare(O.Name, strName, true) == 0)
				{
					return O;
				}
			}
			return null;

		}//	Find(string strName)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public CDxField this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (base.List[index] as CDxField);
			}
		}

		/// <summary>
		/// Overloaded [] operator to locate the parameter object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CDxField this[string strName]
		{
			get 
			{
				// Search for the object with the same name
				foreach(CDxField obj in base.List)
				{
					if(String.Compare(obj.Name, strName, true) == 0)
					{
						return obj;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// This method is called to retrieve the index of the specified object
		/// </summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CDxField value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		/// <summary>This method creates an xml node that represents the collection</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		/// <param name="strName">Name assigned to the node</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlFields = null;
			XmlNode		xmlField	= null;
			string		strElementName = "";
				
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = "dxFields";

			if((xmlFields = xmlDocument.CreateElement(strElementName)) != null)
			{
				//	Assign the attributes
				xmlFields.SetAttribute("Count", this.Count.ToString());
				
				//	Iterate the collection and append each item as a child
				foreach(CDxField O in this)
				{
					if((xmlField = O.ToXmlNode(xmlDocument)) != null)
					{
						xmlFields.AppendChild(xmlField);
					}
				}

			}
			
			return xmlFields;

		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
	}//	public class CDxFields
		
}// namespace FTI.Trialmax.Database
