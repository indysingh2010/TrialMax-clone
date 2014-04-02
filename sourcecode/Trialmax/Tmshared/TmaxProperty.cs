using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>Objects of this class are used to expose properties associated with a media object</summary>
	public class CTmaxProperty : ITmaxPropGridCtrl
	{
		#region Private Members
		
		/// <summary>Local member accessed by Name property</summary>
		private string m_strName = "";
			
		/// <summary>Local member accessed by Value property</summary>
		private object m_oValue = null;
			
		/// <summary>Local member accessed by Id property</summary>
		private int m_iId = 0;
			
		/// <summary>Local member accessed by Category property</summary>
		private TmaxPropertyCategories m_eCategory = TmaxPropertyCategories.Media;
			
		/// <summary>Local member accessed by Editor property</summary>
		private TmaxPropGridEditors m_eEditor = TmaxPropGridEditors.Text;

		/// <summary>Local member accessed by Visible property</summary>
		private bool m_bVisible = true;

		#endregion Private Members
		
		#region Public Methods
			
		/// <summary>Default constructor</summary>
		public CTmaxProperty()
		{
		}
		
		/// <summary>Constructor</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name assigned to property</param>
		public CTmaxProperty(int iId, string strName)
		{
			Initialize(iId, strName, null, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name assigned to property</param>
		/// <param name="O">Value assigned to the property</param>
		public CTmaxProperty(int iId, string strName, object O)
		{
			Initialize(iId, strName, O, TmaxPropertyCategories.Media, TmaxPropGridEditors.Text);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name assigned to property</param>
		/// <param name="O">Value assigned to the property</param>
		/// <param name="eCategory">Category assigned to this property</param>
		public CTmaxProperty(int iId, string strName, object O, TmaxPropertyCategories eCategory)
		{
			Initialize(iId, strName, O, eCategory, TmaxPropGridEditors.Text);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name assigned to property</param>
		/// <param name="O">Value assigned to the property</param>
		/// <param name="eCategory">Category assigned to this property</param>
		/// <param name="eEditor">Editor assigned to this property</param>
		public CTmaxProperty(int iId, string strName, object O, TmaxPropertyCategories eCategory, TmaxPropGridEditors eEditor)
		{
			Initialize(iId, strName, O, eCategory, eEditor);
		}
		
		/// <summary>This method will initialize all property values</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name of this property</param>
		/// <param name="oValue">Value of this property</param>
		/// <param name="eCategory">Category assigned to this property</param>
		/// <param name="eEditor">Editor assigned to this property</param>
		public void Initialize(int iId, string strName, object oValue, TmaxPropertyCategories eCategory, TmaxPropGridEditors eEditor)
		{
			m_iId = iId;
			m_oValue = oValue;
			m_eCategory = eCategory;
			m_eEditor = eEditor;
			m_strName = strName;
		}
		
		/// <summary>This method is called to read the property value as a string</summary>
		/// <returns>The current value as a string</returns>
		public string AsString()
		{
			if(m_oValue != null)
				return m_oValue.ToString();
			else
				return "NULL";
		}
		
		/// <summary>This method is called to get the string representation of this property</summary>
		/// <returns>The current value as a string</returns>
		public override string ToString()
		{
			return (Name + " = " + AsString());
		}
				
		/// <summary>This method creates an xml node that represents the property</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			XmlElement xmlProperty = null;
				
			if((xmlProperty = xmlDocument.CreateElement(m_strName.Replace(' ', '_'))) != null)
			{
				xmlProperty.SetAttribute("Value", AsString());
			}
			
			return xmlProperty;

		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		/// <summary>This function is called to get the name used to display this object in the property grid</summary>
		/// <returns>The tag name</returns>
		string ITmaxPropGridCtrl.GetName()
		{
			return this.Name;
		}
		
		/// <summary>This function is called to get the value of this object in the property grid</summary>
		/// <returns>The current value</returns>
		string ITmaxPropGridCtrl.GetValue()
		{
			return this.AsString();
		}
		
		/// <summary>This function is called to get the category this object belongs to in the property grid</summary>
		/// <returns>null - tags don't use categories</returns>
		object ITmaxPropGridCtrl.GetCategory()
		{
			return this.Category;
		}
		
		/// <summary>This function is called to get the option tag the property grid uses to identify this object in events if fires</summary>
		/// <returns>the reference to this object</returns>
		object ITmaxPropGridCtrl.GetTag()
		{
			return this;
		}

		/// <summary>This function is called to get the visibility of the property in the grid</summary>
		/// <returns>true if visible</returns>
		bool ITmaxPropGridCtrl.GetVisible()
		{
			return this.Visible;
		}

		/// <summary>This function is called to compare the specified object to this object for sorting</summary>
		/// <param name="ICompare">The object to be compared</param>
		/// <param name="lSortOn">The sort mode identifier assigned to the property grid</param>
		/// <returns>0 if equal, -1 if this object is less, 1 if greater</returns>
		int ITmaxPropGridCtrl.Compare(ITmaxPropGridCtrl ICompare, long lSortOn)
		{
			//	Not sorted
			return -1;

		}// int ITmaxPropGridCtrl.Compare(ITmaxPropGridCtrl ICompare)
		
		/// <summary>This function is called to get the id assigned to the property</summary>
		/// <returns>the object id</returns>
		long ITmaxPropGridCtrl.GetId()
		{
			return this.Id;
		}
		
		/// <summary>This function is called to set the object's value</summary>
		/// <param name="newValue">The new value of the object</param>
		/// <returns>true if successful</returns>
		bool ITmaxPropGridCtrl.SetValue(string strValue)
		{
			return true;
		}
		
		/// <summary>This method is called to set the value of the multi-level property</summary>
		/// <param name="tmaxParent">The new parent pick list</param>
		/// <param name="strValue">The new value to be assigned to the property</param>
		/// <returns>True if successful</returns>
		bool ITmaxPropGridCtrl.SetValue(CTmaxPickItem tmaxParent, string strValue)
		{
			return true;
		}

		/// <summary>This method is called to set the visibility of the property in the grid</summary>
		/// <param name="bVisible">True if visible</param>
		void ITmaxPropGridCtrl.SetVisible(bool bVisible)
		{
			this.Visible = bVisible;
		}

		/// <summary>This function is called to get the editor best suited for this property</summary>
		/// <returns>The enumerated editor identifier</returns>
		TmaxPropGridEditors ITmaxPropGridCtrl.GetEditor()
		{
			return this.Editor;		
		}
		
		/// <summary>This method is called to get the collection of drop list values for the property</summary>
		/// <returns>True if successful</returns>
		System.Collections.ICollection ITmaxPropGridCtrl.GetDropListValues()
		{
			return null;		
		}
		
		/// <summary>This method is called to get the case code bound to this property</summary>
		/// <returns>True if successful</returns>
		FTI.Shared.Trialmax.CTmaxCaseCode ITmaxPropGridCtrl.GetCaseCode()
		{
			return null;		
		}

		/// <summary>This method is called to get the pick item bound to this property</summary>
		/// <returns>True if successful</returns>
		FTI.Shared.Trialmax.CTmaxPickItem ITmaxPropGridCtrl.GetPickItem()
		{
			return null;		
		}

		#endregion Public Methods
	
		#region Protected Methods
		
		
		#endregion Protected Methods
			
		#region Properties
		
		/// <summary>This property contains the property name</summary>
		public string Name
		{
			get
			{
				if((m_strName != null) && (m_strName.Length > 0))
				{
					return m_strName;
				}
				else
				{
					return m_iId.ToString();
				}

			}
			set
			{
				m_strName = value;
			}
			
		} // Name property

		/// <summary>The identifier associated with this property</summary>
		public int Id
		{
			get { return m_iId; }
			set { m_iId = value; }
		}

		/// <summary>The current value of this property</summary>
		public object Value
		{
			get { return m_oValue; }
			set { m_oValue = value; }
		
		}

		/// <summary>True if the property should be visible in the grid</summary>
		public bool Visible
		{
			get { return m_bVisible; }
			set { m_bVisible = value; }
		}

		/// <summary>Enumerated category of this property</summary>
		public TmaxPropertyCategories Category
		{
			get { return m_eCategory; }
			set { m_eCategory = value; }
		}

		/// <summary>Enumerated editor for this property</summary>
		public TmaxPropGridEditors Editor
		{
			get { return m_eEditor; }
			set { m_eEditor = value; }
		}

		#endregion Properties

	}//	public class CTmaxProperty : ITmaxPropGridCtrl
		
	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxProperty objects
	/// </summary>
	public class CTmaxProperties : CollectionBase
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxProperties()
		{
		}

		/// <summary>This method allows the caller to add a new property to the list</summary>
		/// <param name="Property">CTmaxProperty object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxProperty Add(CTmaxProperty tmaxProperty)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(tmaxProperty as object);

				return tmaxProperty;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxProperty tmaxProperty)

		/// <summary>This method will add a new property with the specified values to the collection</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name of this property</param>
		/// <param name="oValue">Value of this property</param>
		/// <param name="eCategory">Category assigned to this property</param>
		/// <param name="eEditor">Editor assigned to this property</param>
		///	<returns>The property added to the collection</returns>
		public CTmaxProperty Add(int iId, string strName, object oValue, TmaxPropertyCategories eCategory, TmaxPropGridEditors eEditor)
		{
			return Add(new CTmaxProperty(iId, strName, oValue, eCategory, eEditor));
		}
		
		/// <summary>This method will add a new property with the specified values to the collection</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name of this property</param>
		/// <param name="oValue">Value of this property</param>
		/// <param name="eCategory">Category assigned to this property</param>
		///	<returns>The property added to the collection</returns>
		public CTmaxProperty Add(int iId, string strName, object oValue, TmaxPropertyCategories eCategory)
		{
			return Add(new CTmaxProperty(iId, strName, oValue, eCategory));
		}
		
		/// <summary>This method will add a new property with the specified values to the collection</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name of this property</param>
		/// <param name="oValue">Value of this property</param>
		///	<returns>The property added to the collection</returns>
		public CTmaxProperty Add(int iId, string strName, object oValue)
		{
			return Add(new CTmaxProperty(iId, strName, oValue));
		}
		
		/// <summary>This method will add a new property with the specified values to the collection</summary>
		/// <param name="iId">Identifier assigned to this property</param>
		/// <param name="strName">Name of this property</param>
		///	<returns>The property added to the collection</returns>
		public CTmaxProperty Add(int iId, string strName)
		{
			return Add(new CTmaxProperty(iId, strName));
		}
		
		/// <summary>
		/// This method is called to remove the requested property from the collection
		/// </summary>
		/// <param name="tmaxProperty">The property object to be removed</param>
		public void Remove(CTmaxProperty tmaxProperty)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(tmaxProperty as object);
			}
			catch
			{
			}
		}

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="tmaxProperty">The property object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxProperty tmaxProperty)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(tmaxProperty as object);
		}

		/// <summary>
		/// Overloaded [] operator to locate the property object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxProperty this[string strName]
		{
			get 
			{
				return Find(strName);
			}
		}

		/// <summary>
		/// Called to locate the object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxProperty Find(string strName)
		{
			// Search for the object with the same name
			foreach(CTmaxProperty obj in base.List)
			{
				if(String.Compare(obj.Name, strName, true) == 0)
				{
					return obj;
				}
			}
			return null;

		}//	Find(string strName)

		/// <summary>
		/// Called to locate the object with the specified identifier
		/// </summary>
		/// <returns>The object with the specified identifier</returns>
		public CTmaxProperty Find(int iId)
		{
			// Search for the object with the same identifier
			foreach(CTmaxProperty O in base.List)
			{
				if(O.Id == iId)
				{
					return O;
				}
			}
			return null;

		}//	Find(int iId)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public CTmaxProperty this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (base.List[index] as CTmaxProperty);
			}
		}

		/// <summary>
		/// This method is called to retrieve the index of the specified object
		/// </summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxProperty value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		/// <summary>This method creates an xml node that represents the properties in this collection</summary>
		/// <param name="xmlDocument">XML document object to which the node will be added</param>
		/// <param name="strName">Name to be assigned to the node</param>
		///	<returns>An XML node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlProperties  = null;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = "tmaxProperties";
				 
			//	Create the top-level node
			if((xmlProperties = xmlDocument.CreateElement(strElementName)) == null)
				return null;

			foreach(CTmaxProperty O in this)
			{
				xmlProperties.SetAttribute(O.Name.Replace(' ', '_'), O.AsString());
			}
			
			return xmlProperties;
		
		}// public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		
		#endregion Public Methods
		
	}//	CTmaxProperties
	
	/// <summary>Objects of this class are used to set the value of a TrialMax property</summary>
	public class CTmaxSetPropertyArgs
	{
		#region Private Members
		
		/// <summary>Local member accessed by Property property</summary>
		private CTmaxProperty m_tmaxProperty = null;
			
		/// <summary>Local member accessed by NewValue property</summary>
		private string m_strNewValue = "";
		
		/// <summary>Local member accessed by Message property</summary>
		private string m_strMessage = ""; 
			
		/// <summary>Local member bound to Confirmed property</summary>
		private bool m_bConfirmed = false;
			
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxSetPropertyArgs()
		{
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxProperty">The property to be set</param>
		/// <param name="strNewValue">The new value to be assigned to the property</param>
		public CTmaxSetPropertyArgs(CTmaxProperty tmaxProperty, string strNewValue)
		{
			m_tmaxProperty = tmaxProperty;
			m_strNewValue  = strNewValue;
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="tmaxProperty">The property to be set</param>
		public CTmaxSetPropertyArgs(CTmaxProperty tmaxProperty)
		{
			m_tmaxProperty = tmaxProperty;
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>This is the Property object provided by the record exchange object</summary>
		public CTmaxProperty Property
		{
			get
			{
				return m_tmaxProperty;
			}
			set
			{
				m_tmaxProperty = value;
			}
			
		} // Property property

		/// <summary>This is the new value to be assigned to the property</summary>
		public string NewValue
		{
			get
			{
				return m_strNewValue;
			}
			set
			{
				m_strNewValue = value;
			}
			
		} // NewValue property
		
		/// <summary>This is the message to be displayed if unable to set the property to the new value</summary>
		public string Message
		{
			get
			{
				return m_strMessage;
			}
			set
			{
				m_strMessage = value;
			}
			
		} // Message property
		
		/// <summary>True if the operation has already been confirmed by the caller</summary>
		public bool Confirmed
		{
			get
			{
				return m_bConfirmed;
			}
			set
			{
				m_bConfirmed = value;
			}
			
		} // Confirmed property
		
		#endregion Properties
		
	}// public class CTmaxSetPropertyArgs
		
}// namespace FTI.Shared.Trialmax
