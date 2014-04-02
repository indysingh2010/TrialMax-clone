using System;
using System.Collections;
using System.Xml;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>This is the delegate used to handle all error events</summary>
	/// <param name="objSender">Object firing the event</param>
	/// <param name="Args">Object containing the error event arguments</param>
	public delegate void DiagnosticEventHandler(object objSender, CTmaxDiagnosticArgs Args);
		
	/// <summary>
	/// This class encapsulates the information required to report an error
	/// </summary>
	public class CTmaxDiagnosticArgs : IXmlObject, ITmaxListViewCtrl
	{
		#region Protected Members
		
		/// <summary>Local member accessed by Items property</summary>
		protected CDiagnosticItems m_aItems = new CDiagnosticItems();
			
		/// <summary>Local member accessed by Message property</summary>
		protected string m_strMessage = "";
			
		/// <summary>Local member accessed by Source property</summary>
		protected string m_strSource = "";
			
		/// <summary>Local member accessed by Name property</summary>
		protected string m_strName = "";
			
		/// <summary>Local member accessed by Date property</summary>
		protected string m_strDate = "";
			
		/// <summary>Local member accessed by Time property</summary>
		protected string m_strTime = "";
		
		/// <summary>Local member accessed by Exception property</summary>
		protected string m_strException = "";
		
		/// <summary>Local member accessed by Details property</summary>
		protected string m_strDetails = "";
		
		#endregion Private Members
			
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxDiagnosticArgs()
		{
			//	Initilize the time stamp
			SetTimeStamp();
		}
		
		/// <summary>Constructor</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		/// <param name="strException">The message associated with an exception</param>
		/// <param name="strDetails">The details associated with an exception</param>
		public CTmaxDiagnosticArgs(string strSource, string strMessage, string strName, CDiagnosticItems aItems, string strException, string strDetails)
		{
			Initialize(strSource, strMessage, strName, aItems, strException, strDetails);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		/// <param name="strException">The message associated with an exception</param>
		public CTmaxDiagnosticArgs(string strSource, string strMessage, string strName, CDiagnosticItems aItems, string strException)
		{
			Initialize(strSource, strMessage, strName, aItems, strException);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		/// <param name="Ex">The system exception associated with the message</param>
		public CTmaxDiagnosticArgs(string strSource, string strMessage, string strName, CDiagnosticItems aItems, System.Exception Ex)
		{
			Initialize(strSource, strMessage, strName, aItems, Ex);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		/// <param name="strException">The message associated with an exception</param>
		/// <param name="strDetails">The details associated with an exception</param>
		public CTmaxDiagnosticArgs(string strSource, string strMessage, string strName, CDiagnosticItems aItems)
		{
			Initialize(strSource, strMessage, strName, aItems);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		public CTmaxDiagnosticArgs(string strSource, string strMessage, string strName)
		{
			Initialize(strSource, strMessage, strName);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="aItems">The list of items associated with the event</param>
		public CTmaxDiagnosticArgs(string strSource, string strMessage, CDiagnosticItems aItems)
		{
			Initialize(strSource, strMessage, aItems);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		public CTmaxDiagnosticArgs(string strSource, string strMessage)
		{
			Initialize(strSource, strMessage);
		}

		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		/// <param name="strException">The message associated with an exception</param>
		/// <param name="strDetails">The details associated with an exception</param>
		public void Initialize(string strSource, string strMessage, string strName, CDiagnosticItems aItems, string strException, string strDetails)
		{
			if((strMessage != null) && (strMessage.Length > 0))
				m_strMessage = strMessage;
			else
				m_strMessage = "No diagnostic message available";
				
			if((strSource != null) && (strSource.Length > 0))
				m_strSource = strSource;
			
			if((strName != null) && (strName.Length > 0))
				m_strName = strName;
			
			if((strException != null) && (strException.Length > 0))
				m_strException = strException;
			
			if((strDetails != null) && (strDetails.Length > 0))
				m_strDetails = strDetails;
			
			if(aItems != null)
			{
				foreach(CTmaxDiagnosticArgs.CDiagnosticItem objItem in aItems)
				{
					m_aItems.Add(objItem);
				}
			}
			
			//	Set the time stamp
			SetTimeStamp();			
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		/// <param name="strException">The message associated with an exception</param>
		public void Initialize(string strSource, string strMessage, string strName, CDiagnosticItems aItems, string strException)
		{
			Initialize(strSource, strMessage, strName, aItems, strException, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		/// <param name="Ex">The exception associated with the message</param>
		public void Initialize(string strSource, string strMessage, string strName, CDiagnosticItems aItems, System.Exception Ex)
		{
			if(Ex != null)
				Initialize(strSource, strMessage, strName, aItems, Ex.Message, Ex.ToString());
			else
				Initialize(strSource, strMessage, strName, aItems);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		/// <param name="aItems">The list of items associated with the event</param>
		public void Initialize(string strSource, string strMessage, string strName, CDiagnosticItems aItems)
		{
			Initialize(strSource, strMessage, strName, aItems, null, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="strName">The name associated with the event</param>
		public void Initialize(string strSource, string strMessage, string strName)
		{
			Initialize(strSource, strMessage, strName, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="aItems">The list of items associated with the event</param>
		public void Initialize(string strSource, string strMessage, CDiagnosticItems aItems)
		{
			Initialize(strSource, strMessage, null, aItems);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strSource">The source of the diagnostic event</param>
		/// <param name="strMessage">The diagnostic message</param>
		public void Initialize(string strSource, string strMessage)
		{
			Initialize(strSource, strMessage, null, null);
		}
		
		/// <summary>This method will set the time stamp using the current system time</summary>
		public virtual void SetTimeStamp()
		{
			DateTime TimeStamp = DateTime.Now;
			m_strDate = TimeStamp.ToString("MM-dd-yyyy");
			m_strTime = TimeStamp.ToString("HH:mm:ss.fff");
		}

		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		///	<returns>An Xml node that represents the object</returns>
		XmlNode IXmlObject.ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument);
		}
			
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		///	<returns>An Xml node that represents the object</returns>
		XmlNode IXmlObject.ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			return ToXmlNode(xmlDocument, strName);
		}
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument, null);	
		}
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement  = null;
			XmlElement	xmlItems    = null;
			XmlElement	xmlItem		= null;
			bool		bSuccessful = false;
			CXmlFile	xmlFile = new CXmlFile();
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = "Diagnostic";
				
			if((xmlElement = xmlDocument.CreateElement("diag", strElementName, "fticonsulting.com/xsd/xmldiag")) != null)
			{
				while(bSuccessful == false)
				{
					if(xmlFile.AddAttribute(xmlElement, "Message", m_strMessage) == false)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Date", m_strDate) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Time", m_strTime) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Source", m_strSource) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Name", m_strName) == null)
						break;
						
					if((m_strException != null) && (m_strException.Length > 0))
					{
						if(xmlFile.AddElement(xmlDocument, xmlElement, "Exception", m_strException) == null)
							break;
					}
						
					if((m_strDetails != null) && (m_strDetails.Length > 0))
					{
						if(xmlFile.AddElement(xmlDocument, xmlElement, "Details", m_strDetails) == null)
							break;
					}
						
					if((xmlItems = xmlFile.AddElement(xmlDocument, xmlElement, "Items", null)) != null)
					{
						foreach(CDiagnosticItem objItem in m_aItems)
						{
							if((xmlItem = xmlFile.AddElement(xmlDocument, xmlItems, "Item", null)) != null)
							{
								xmlFile.AddAttribute(xmlItem, "Name", objItem.Name);
								xmlFile.AddAttribute(xmlItem, "Value", objItem.Value);
							}
						}
					}
					
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;
		}
		
		/// <summary>This function is called to add the names of the columns that appear in a TrialMax list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of column names</returns>
		string[] ITmaxListViewCtrl.GetColumnNames(int iDisplayMode)
		{
			string[] aNames = { "Time", "Message", "Source" };
			return aNames;

		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[3];
			aValues[0] = this.Time;
			aValues[1] = this.Message.Replace("\n", " ");
			aValues[2] = this.Source;
			
			return aValues;
						
		}// string[] ITmaxListViewCtrl.GetValues()
		
		/// <summary>This function is called to get the index of the image to be displayed in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The image index</returns>
		int ITmaxListViewCtrl.GetImageIndex(int iDisplayMode)
		{
			return -1;
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>This is the diagnostic message</summary>
		public string Message
		{
			get { return m_strMessage; }
			set { m_strMessage = value; }
		}

		/// <summary>This is the source of the event</summary>
		public string Source
		{
			get { return m_strSource; }
			set { m_strSource = value; }
		}

		/// <summary>This is the object name provided with the event</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		/// <summary>This is the array of items associated with this event</summary>
		public CDiagnosticItems Items
		{
			get { return m_aItems; }
		}

		/// <summary>The exception text reported with the event</summary>
		public string Exception
		{
			get { return m_strException; }
			set { m_strException = value; }
		}

		/// <summary>The details reported with an exception</summary>
		public string Details
		{
			get { return m_strDetails; }
			set { m_strDetails = value; }
		}

		/// <summary>This property exposes the Date associated with the event</summary>
		public string Date
		{
			get { return m_strDate; }
			set { m_strDate = value; }
		}

		/// <summary>This property exposes the Time associated with the event</summary>
		public string Time
		{
			get { return m_strTime; }
			set { m_strTime = value; }
		}

		#endregion Properties
	
		#region Items
		
		/// <summary>
		/// Objects of this class are used to define an item to be stored in the error
		/// </summary>
		public class CDiagnosticItem
		{
			/// <summary>Local member accessed by Name property</summary>
			protected string m_strName;
			
			/// <summary>Local member accessed by Value property</summary>
			protected string m_strValue;
			
			/// <summary>Local member accessed by the Delimiter property</summary>
			protected string m_strDelimiter = "=";			
		
			/// <summary>Default constructor</summary>
			public CDiagnosticItem()
			{
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			public CDiagnosticItem(string strName)
			{
				m_strName = strName;
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strValue">Value assigned to the item</param>
			public CDiagnosticItem(string strName, string strValue)
			{
				m_strName = strName;
				m_strValue = strValue;
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strDelimiter">Delimiter assigned to the item</param>
			/// <param name="strValue">Value assigned to the item</param>
			public CDiagnosticItem(string strName, string strDelimiter, string strValue)
			{
				m_strName = strName;
				m_strDelimiter = strDelimiter;
				m_strValue = strValue;
			}
			
			/// <summary>Called to convert the error item to a text string</summary>
			/// <returns>The string representation of the item</returns>
			public override string ToString()
			{
				string strItem = m_strName + m_strDelimiter + m_strValue;
				return strItem;
			}
			
			/// <summary>This property contains the full path specification of the item</summary>
			public string Name
			{
				get { return m_strName; }
				set { m_strName = value; }
			}

			/// <summary>This property contains the value written to the log</summary>
			public string Value
			{
				get { return m_strValue; }
				set { m_strValue = value; }
			}

			/// <summary>Delimiter added to the end of this item in the log file</summary>
			public string Delimiter
			{
				get { return m_strDelimiter; }
				set { m_strDelimiter = value; }
			}

		}//	CDiagnosticItem
		
		/// <summary> Objects of this class are used to manage a dynamic array of CDiagnosticItem objects</summary>
		public class CDiagnosticItems : CollectionBase
		{
			/// <summary>Local member accessed by the Delimiter property</summary>
			protected string m_strDelimiter = "  ";			
		
			/// <summary>Default constructor</summary>
			public CDiagnosticItems()
			{
			}

			/// <summary>This method allows the caller to add a new item to the list</summary>
			/// <param name="objItem">CDiagnosticItem object to be added to the list</param>
			/// <returns>The object just added if successful, null otherwise</returns>
			public virtual CDiagnosticItem Add(CDiagnosticItem objItem)
			{
				try
				{
					// Use base class to perform actual collection operation
					base.List.Add(objItem as object);

					return objItem;
				}
				catch
				{
					return null;
				}
			
			}// Add(CDiagnosticItem objItem)

			/// <summary>This method allows the caller to add a new item to the list</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strValue">Value assigned to the item</param>
			public virtual CDiagnosticItem Add(string strName, string strValue)
			{
				CDiagnosticItem diagItem = null;
				
				try
				{
					if((diagItem = new CDiagnosticItem(strName, strValue)) != null)
						return Add(diagItem);
				}
				catch
				{
				}
				
				//	Must have been an error
				return null;
			}
			
			/// <summary>This method allows the caller to add a new item to the list</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strDelimiter">Delimiter assigned to the item</param>
			/// <param name="strValue">Value assigned to the item</param>
			public virtual CDiagnosticItem Add(string strName, string strDelimiter, string strValue)
			{
				CDiagnosticItem diagItem = null;
				
				try
				{
					if((diagItem = new CDiagnosticItem(strName, strDelimiter, strValue)) != null)
						return Add(diagItem);
				}
				catch
				{
				}
				
				//	Must have been an error
				return null;
			}
			
			public virtual bool SetValue(string strName, string strValue)
			{
				try
				{
					foreach(CDiagnosticItem item in base.List)
					{
						if(item.Name == strName)
						{
							item.Value = strValue;
							return true;
						}
					}
				}
				catch
				{
				}
				
				return false;
			
			}// SetValue(string strName, string strValue)

			/// <summary>
			/// Overloaded version of [] operator to return the object at the desired index
			/// </summary>
			/// <returns>Object at the specified index</returns>
			public CDiagnosticItem this[int index]
			{
				// Use base class to process actual collection operation
				get 
				{ 
					return (base.List[index] as CDiagnosticItem);
				}
			}

			/// <summary>Called to assemble a string to represent all items in the collection</summary>
			/// <returns>The string representation of all items in the collection</returns>
			public override string ToString()
			{
				string strItems = "";
				
				if(Count > 0)
				{
					strItems = "[";
					
					for(int i = 0; i < Count; i++)
					{
						if(this[i] != null)
						{
							strItems += this[i].ToString();
							
							if(i < (Count - 1))
								strItems += m_strDelimiter;
						}
						
					}//for(int i = 0; i < Count; i++)
					
					strItems += "]";
				}
				
				return strItems;
			}
	
			/// <summary>Delimiter added to the end of this item in the log file</summary>
			public string Delimiter
			{
				get
				{
					return m_strDelimiter;
				}
				set
				{
					m_strDelimiter = value;
				}
			}

		
		}//	CDiagnosticItems
		
		#endregion Items
		
	}
}
