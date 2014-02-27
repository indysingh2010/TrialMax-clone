using System;
using System.Collections;
using System.Xml;
using System.Windows.Forms;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This is the delegate used to handle all error events</summary>
	/// <param name="objSender">Object firing the event</param>
	/// <param name="Args">Object containing the error event arguments</param>
	public delegate void ErrorEventHandler(object objSender, CTmaxErrorArgs Args);
		
	/// <summary>
	/// This class encapsulates the information required to report an error
	/// </summary>
	public class CTmaxErrorArgs : IXmlObject, ITmaxListViewCtrl
	{
		#region Protected Members
		
		/// <summary>Local member accessed by Items property</summary>
		protected CErrorItems m_aItems = new CErrorItems();
			
		/// <summary>Local member accessed by Message property</summary>
		protected string m_strMessage = "";
			
		/// <summary>Local member accessed by Source property</summary>
		protected string m_strSource = "TrialMax";
			
		/// <summary>Local member accessed by Title property</summary>
		protected string m_strTitle = "TrialMax Error";
			
		/// <summary>Local member accessed by Exception property</summary>
		protected string m_strException = "";
		
		/// <summary>Local member accessed by Details property</summary>
		protected string m_strDetails = "";
		
		/// <summary>Local member accessed by Date property</summary>
		protected string m_strDate = "";
			
		/// <summary>Local member accessed by Time property</summary>
		protected string m_strTime = "";
		
		/// <summary>Local member accessed by Show property</summary>
		protected bool m_bShow = true;
		
		#endregion Private Members
			
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxErrorArgs()
		{
			//	Initilize the time stamp
			SetTimeStamp();
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		/// <param name="strException">The exception message</param>
		/// <param name="strDetails">The error details</param>
		public CTmaxErrorArgs(string strTitle, string strSource, string strMessage, CErrorItems aItems, string strException, string strDetails)
		{
			Initialize(strTitle, strSource, strMessage, aItems, strException, strDetails);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		public CTmaxErrorArgs(string strTitle, string strSource, string strMessage, CErrorItems aItems)
		{
			Initialize(strTitle, strSource, strMessage, aItems);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		public CTmaxErrorArgs(string strTitle, string strSource, string strMessage)
		{
			Initialize(strTitle, strSource, strMessage);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strMessage">The error message</param>
		public CTmaxErrorArgs(string strTitle, string strMessage)
		{
			Initialize(strTitle, strMessage);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strMessage">The error message</param>
		public CTmaxErrorArgs(string strMessage)
		{
			Initialize(strMessage);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		///	<param name="Ex">System exception assocaited with the error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		public CTmaxErrorArgs(string strTitle, string strSource, string strMessage, System.Exception Ex, CErrorItems aItems)
		{
			Initialize(strTitle, strSource, strMessage, Ex, aItems);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		///	<param name="Ex">System exception assocaited with the error message</param>
		public CTmaxErrorArgs(string strTitle, string strSource, string strMessage, System.Exception Ex)
		{
			Initialize(strTitle, strSource, strMessage, Ex);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		/// <param name="strException">The exception message</param>
		/// <param name="strDetails">The error details</param>
		public void Initialize(string strTitle, string strSource, string strMessage, CErrorItems aItems, string strException, string strDetails)
		{
			if((strTitle != null) && (strTitle.Length > 0))
				Title = strTitle;
			else
				Title = "Error";

			if((strMessage != null) && (strMessage.Length > 0))
				Message = strMessage;
			else
				Message = "No error message available";
				
			if((strSource != null) && (strSource.Length > 0))
				Source = strSource;
			else
				Source = "Source unknown";
			
			if(strDetails != null)
				Details = strDetails;
			
			if(strException != null)
				Exception = strException;
				
			if(aItems != null)
			{
				foreach(CTmaxErrorArgs.CErrorItem objItem in aItems)
				{
					Items.Add(objItem);
				}
			}
			
			//	Set the time stamp
			SetTimeStamp();			
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		public void Initialize(string strTitle, string strSource, string strMessage, CErrorItems aItems)
		{
			Initialize(strTitle, strSource, strMessage, aItems, null, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		public void Initialize(string strTitle, string strSource, string strMessage)
		{
			Initialize(strTitle, strSource, strMessage, null, null, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strMessage">The error message</param>
		public void Initialize(string strTitle, string strMessage)
		{
			Initialize(strTitle, null, strMessage, null, null, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strMessage">The error message</param>
		public void Initialize(string strMessage)
		{
			Initialize(null, null, strMessage, null, null, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		///	<param name="Ex">System exception assocaited with the error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		public void Initialize(string strTitle, string strSource, string strMessage, System.Exception Ex, CErrorItems aItems)
		{
			Initialize(strTitle, strSource, strMessage, aItems, Ex.Message, Ex.ToString());
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="strTitle">Title of the error</param>
		/// <param name="strSource">The source of the error</param>
		/// <param name="strMessage">The error message</param>
		///	<param name="Ex">System exception assocaited with the error message</param>
		public void Initialize(string strTitle, string strSource, string strMessage, System.Exception Ex)
		{
			Initialize(strTitle, strSource, strMessage, null, Ex.Message, Ex.ToString());
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
			return ToXmlNode(xmlDocument, null);
		}
			
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">Name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		XmlNode IXmlObject.ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			return ToXmlNode(xmlDocument, strName);
		}
			
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		///	<returns>An Xml node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument, null);
		}
			
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">Name assigned to the node</param>
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
				strElementName = "Error";
			
			if((xmlElement = xmlDocument.CreateElement("err", strElementName, "fticonsulting.com/xsd/xmlerror")) != null)
			{
				while(bSuccessful == false)
				{
					if(xmlFile.AddAttribute(xmlElement, "Message", m_strMessage) == false)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Title", m_strTitle) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Date", m_strDate) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Time", m_strTime) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Source", m_strSource) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Exception", m_strException) == null)
						break;
						
					if(xmlFile.AddElement(xmlDocument, xmlElement, "Details", m_strDetails) == null)
						break;
						
					if((xmlItems = xmlFile.AddElement(xmlDocument, xmlElement, "Items", null)) != null)
					{
						foreach(CErrorItem objItem in m_aItems)
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
			string[] aNames = { "Message", "Time", "Source" };
			return aNames;

		}// string[] ITmaxListViewCtrl.GetColumnNames()
		
		/// <summary>This function is called to get the values that appear in the list view control</summary>
		/// <param name="iDisplayMode">User defined display mode identifier</param>
		/// <returns>The array of values</returns>
		string[] ITmaxListViewCtrl.GetValues(int iDisplayMode)
		{
			string[] aValues = null;
			
			aValues = new string[3];
			aValues[0] = this.Message.Replace("\n", " ");
			aValues[1] = this.Time;
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
		
		/// <summary>This is the message displayed with the error</summary>
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

		/// <summary>This property exposes the source of the error</summary>
		public string Source
		{
			get
			{
				return m_strSource;
			}
			set
			{
				m_strSource = value;
			}
			
		} // Source property

		/// <summary>This is the title to be displayed with the error message</summary>
		public string Title
		{
			get
			{
				return m_strTitle;
			}
			set
			{
				m_strTitle = value;
			}
			
		} // Title property

		/// <summary>This is the array of items associated with this error</summary>
		public CErrorItems Items
		{
			get
			{
				return m_aItems;
			}
			
		} // Items property

		/// <summary>This property contains the message text included with the exception</summary>
		public string Exception
		{
			get
			{
				return m_strException;
			}
			set
			{
				m_strException = value;
			}
			
		} // Exception property
		
		/// <summary>This property contains details about the error</summary>
		public string Details
		{
			get
			{
				return m_strDetails;
			}
			set
			{
				m_strDetails = value;
			}
			
		} // Details property
		
		/// <summary>he property exposes the Date associated with the event</summary>
		public string Date
		{
			get
			{
				return m_strDate;
			}
			set
			{
				m_strDate = value;
			}
			
		} // Date property

		/// <summary>he property exposes the Time associated with the event</summary>
		public string Time
		{
			get
			{
				return m_strTime;
			}
			set
			{
				m_strTime = value;
			}
			
		} // Time property

		/// <summary>True if the error message should be shown in the application popup window</summary>
		public bool Show
		{
			get
			{
				return m_bShow;
			}
			set
			{
				m_bShow = value;
			}
			
		} // Show property

		#endregion Properties
	
		#region Items
		
		/// <summary>
		/// Objects of this class are used to define an item to be stored in the error
		/// </summary>
		public class CErrorItem
		{
			/// <summary>Local member accessed by Name property</summary>
			protected string m_strName;
			
			/// <summary>Local member accessed by Value property</summary>
			protected string m_strValue;
			
			/// <summary>Local member accessed by the Delimiter property</summary>
			protected string m_strDelimiter = "=";			
		
			/// <summary>Default constructor</summary>
			public CErrorItem()
			{
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			public CErrorItem(string strName)
			{
				m_strName = strName;
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strValue">Value assigned to the item</param>
			public CErrorItem(string strName, string strValue)
			{
				m_strName = strName;
				m_strValue = strValue;
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strDelimiter">Delimiter assigned to the item</param>
			/// <param name="strValue">Value assigned to the item</param>
			public CErrorItem(string strName, string strDelimiter, string strValue)
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
				get
				{
					return m_strName;
				}
				set
				{
					m_strName = value;
				}
			
			} // Name property

			/// <summary>This property contains the value written to the log</summary>
			public string Value
			{
				get
				{
					return m_strValue;
				}
				set
				{
					m_strValue = value;
				}
			
			} // Value property

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

		}//	CErrorItem
		
		/// <summary>
		/// Objects of this class are used to manage a dynamic array of CErrorItem objects
		/// </summary>
		public class CErrorItems : CollectionBase
		{
			/// <summary>Local member accessed by the Delimiter property</summary>
			protected string m_strDelimiter = "  ";			
		
			/// <summary>Default constructor</summary>
			public CErrorItems()
			{
			}

			/// <summary>This method allows the caller to add a new item to the list</summary>
			/// <param name="objItem">CErrorItem object to be added to the list</param>
			/// <returns>The object just added if successful, null otherwise</returns>
			public virtual CErrorItem Add(CErrorItem objItem)
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
			
			}// Add(CErrorItem objItem)

			public virtual bool SetValue(string strName, string strValue)
			{
				try
				{
					foreach(CErrorItem item in base.List)
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
			public CErrorItem this[int index]
			{
				// Use base class to process actual collection operation
				get 
				{ 
					return (base.List[index] as CErrorItem);
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

		
		}//	CErrorItems
		
		#endregion Items
		
	}
}
