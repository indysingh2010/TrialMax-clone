using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML product update descriptor</summary>
	public class CXmlUpdate : CXmlBase
	{
		#region Constants
		
		private const string XML_UPDATE_ELEMENT_NAME = "tmaxUpdate";

		private const string XML_UPDATE_ATTRIBUTE_COMPONENT				= "component";
		private const string XML_UPDATE_ATTRIBUTE_DESCRIPTION			= "description";
		private const string XML_UPDATE_ATTRIBUTE_VERSION				= "version";
		private const string XML_UPDATE_ATTRIBUTE_URL					= "url";
		private const string XML_UPDATE_ATTRIBUTE_LOCAL_FILENAME		= "localFilename";
		private const string XML_UPDATE_ATTRIBUTE_AVAILABLE				= "available";
		private const string XML_UPDATE_ATTRIBUTE_SELECTED				= "selected";
		private const string XML_UPDATE_ATTRIBUTE_DOWNLOADED			= "downloaded";
		private const string XML_UPDATE_ATTRIBUTE_INSTALLED				= "installed";
		private const string XML_UPDATE_ATTRIBUTE_USERNAME				= "userName";
		private const string XML_UPDATE_ATTRIBUTE_PASSWORD				= "password";
		private const string XML_UPDATE_ATTRIBUTE_SEQUENCE				= "sequence";
		private const string XML_UPDATE_ATTRIBUTE_GROUP_ID				= "groupId";
		private const string XML_UPDATE_ATTRIBUTE_IGNORE_VERSION		= "ignoreVersion";
		private const string XML_UPDATE_ATTRIBUTE_REQUIRED_PRODUCT_VER	= "requiredProductVer";
		private const string XML_UPDATE_ATTRIBUTE_INSTALLED_KEY_PATH	= "installedKeyPath";
		private const string XML_UPDATE_ATTRIBUTE_INSTALLED_VALUE		= "installedValue";
		private const string XML_UPDATE_ATTRIBUTE_INSTALLED_VALUE_NAME	= "installedValueName";

		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bound to the Component property</summary>
		private string m_strComponent = "";	
		
		/// <summary>This member is bound to the Description property</summary>
		private string m_strDescription = "";	
		
		/// <summary>This member is bound to the Version property</summary>
		private string m_strVersion = "";
		
		/// <summary>This member is bound to the Url property</summary>
		private string m_strUrl = "";	
		
		/// <summary>This member is bound to the LocalFilename property</summary>
		private string m_strLocalFilename = "";
		
		/// <summary>This member is bound to the UserName property</summary>
		private string m_strUserName = "";
		
		/// <summary>This member is bound to the GroupId property</summary>
		private string m_strGroupId = "";
		
		/// <summary>This member is bound to the Password property</summary>
		private string m_strPassword = "";

		/// <summary>This member is bound to the RequiredProductVer property</summary>
		private string m_strRequiredProductVer = "";

		/// <summary>This member is bound to the InstalledKeyPath property</summary>
		private string m_strInstalledKeyPath = "";

		/// <summary>This member is bound to the InstalledValueName property</summary>
		private string m_strInstalledValueName = "";

		/// <summary>This member is bound to the InstalledValue property</summary>
		private string m_strInstalledValue = "";

		/// <summary>This member is bound to the Available property</summary>
		private bool m_bAvailable = false;
		
		/// <summary>This member is bound to the Selected property</summary>
		private bool m_bSelected = false;
		
		/// <summary>This member is bound to the Downloaded property</summary>
		private bool m_bDownloaded = false;
		
		/// <summary>This member is bound to the Installed property</summary>
		private bool m_bInstalled = false;
		
		/// <summary>This member is bound to the IgnoreVersion property</summary>
		private bool m_bIgnoreVersion = false;
		
		/// <summary>This member is bound to the Sequence property</summary>
		private int m_iSequence = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlUpdate() : base()
		{
			//	Initialize the event source
			m_tmaxEventSource.Name = "TmaxComponent Events";
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlUpdate">Source object to be copied</param>
		public CXmlUpdate(CXmlUpdate xmlUpdate) : base()
		{
			Debug.Assert(xmlUpdate != null);
			
			if(xmlUpdate != null)
				Copy(xmlUpdate);
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				CXmlUpdate xmlCompare = (CXmlUpdate)xmlBase;
				
				if(ReferenceEquals(this, xmlCompare) == true)
					return 0;
				else if(this.Sequence == xmlCompare.Sequence)
					return 0;
				else if(this.Sequence > xmlCompare.Sequence)
					return 1;
				else
					return -1;
					
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the properties of the source node</summary>
		/// <param name="xmlSource">The source object to be copied</param>
		public void Copy(CXmlUpdate xmlSource)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
			
			this.Component = xmlSource.Component;
			this.Description = xmlSource.Description;
			this.Version = xmlSource.Version;
			this.Url = xmlSource.Url;
			this.LocalFilename = xmlSource.LocalFilename;
			this.Available = xmlSource.Available;
			this.Selected = xmlSource.Selected;
			this.Downloaded = xmlSource.Downloaded;
			this.Installed = xmlSource.Installed;
			this.UserName = xmlSource.UserName;
			this.Password = xmlSource.Password;
			this.Sequence = xmlSource.Sequence;
			this.GroupId = xmlSource.GroupId;
			this.IgnoreVersion = xmlSource.IgnoreVersion;
			this.RequiredProductVer = xmlSource.RequiredProductVer;
			this.InstalledKeyPath = xmlSource.InstalledKeyPath;
			this.InstalledValue = xmlSource.InstalledValue;
			this.InstalledValueName = xmlSource.InstalledValueName;
			
		}// public void Copy(CXmlUpdate xmlSource)
		
		/// <summary>This method will set the object'properties using the specified node</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML update properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the object properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_COMPONENT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strComponent = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_DESCRIPTION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strDescription = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_VERSION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strVersion = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_URL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strUrl = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_USERNAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strUserName = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_GROUP_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strGroupId = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_PASSWORD,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strPassword = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_LOCAL_FILENAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strLocalFilename = strAttribute;
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_AVAILABLE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bAvailable = XmlToBool(strAttribute);
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_SELECTED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bSelected = XmlToBool(strAttribute);
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_DOWNLOADED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bDownloaded = XmlToBool(strAttribute);
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_INSTALLED,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bInstalled = XmlToBool(strAttribute);
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_SEQUENCE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					try { m_iSequence = System.Convert.ToInt32(strAttribute); }
					catch { m_iSequence = 0; }
				}
				
				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_IGNORE_VERSION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bIgnoreVersion = XmlToBool(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_REQUIRED_PRODUCT_VER, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strRequiredProductVer = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_INSTALLED_KEY_PATH, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strInstalledKeyPath = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_INSTALLED_VALUE_NAME, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strInstalledValueName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_UPDATE_ATTRIBUTE_INSTALLED_VALUE, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strInstalledValue = strAttribute;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML update properties", Ex);
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
				strElementName = XML_UPDATE_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_COMPONENT, this.Component) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_DESCRIPTION, this.Description) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_VERSION, this.Version) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_URL, this.Url) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_USERNAME, this.UserName) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_GROUP_ID, this.GroupId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_PASSWORD, this.Password) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_LOCAL_FILENAME, this.LocalFilename) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_AVAILABLE, this.Available) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_SELECTED, this.Selected) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_DOWNLOADED, this.Downloaded) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_INSTALLED, this.Installed) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_SEQUENCE, this.Sequence) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_IGNORE_VERSION, this.IgnoreVersion) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_REQUIRED_PRODUCT_VER, this.RequiredProductVer) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_INSTALLED_KEY_PATH, this.InstalledKeyPath) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_INSTALLED_VALUE, this.InstalledValue) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_UPDATE_ATTRIBUTE_INSTALLED_VALUE_NAME, this.InstalledValueName) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>This method is called to construct a fully qualified path to the local file for this update</summary>
		/// <param name="strFolder">The folder where the local file is stored</param>
		/// <returns>The fully qualified path to the local file</returns>
		public string GetLocalFileSpec(string strFolder)
		{
			string strFileSpec = "";
			
			//	Did the caller specify a folder?
			if((strFolder != null) && (strFolder.Length > 0))
				strFileSpec = strFolder;
			else
				strFileSpec = CTmaxToolbox.GetApplicationFolder();
			
			if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
				strFileSpec += "\\";
			
			//	Use the component name if we don't have a local filename
			if(m_strLocalFilename.Length == 0)
				strFileSpec += (m_strComponent + ".exe");
			else
				strFileSpec += m_strLocalFilename;
			
			return strFileSpec;
			
		}// public string GetLocalFileSpec(string strFolder)
		
		/// <summary>This method is called to get the fully qualified URL to the file on the server</summary>
		/// <param name="strServer">The url to the server to use if the URL assigned to this object is a relative path</param>
		/// <returns>The fully qualified URL</returns>
		public string GetServerFileSpec(string strServer)
		{
			string strFileSpec = "";
			string	strUrl;
			
			//	If we don't have a URL assume the local and remote filenames are the same
			if(this.Url.Length == 0)
				strUrl = this.LocalFilename;
			else
				strUrl = this.Url;
			
			if(strUrl.Length == 0) return "";

			//	Should we treat the URL as a relative path?
			if(strUrl.IndexOf("://") < 0)
			{
				//	Use the server specified by the caller
				if((strServer != null) && (strServer.Length > 0))
				{
					strFileSpec = strServer;
					
					//	Build the complete URL
					if((strFileSpec.EndsWith("\\") == true) || (strFileSpec.EndsWith("/") == true))
					{
						if((strUrl.StartsWith("\\") == true) || (strUrl.StartsWith("/") == true))
						{
							strFileSpec += strUrl.Substring(1, strUrl.Length - 1);
						}
						else
						{
							strFileSpec += strUrl;
						}
				
					}
					else
					{
						if((strUrl.StartsWith("\\") == true) || (strUrl.StartsWith("/") == true))
						{
							strFileSpec += strUrl;
						}
						else
						{
							strFileSpec += ("/" + strUrl);
						}
				
					}// if((strFileSpec.EndsWith("\\") == true) || (strFileSpec.EndsWith("/") == true))
				
				}// if((strFileSpec.EndsWith("\\") == true) || (strFileSpec.EndsWith("/") == true))
				
			}
			else
			{
				strFileSpec = strUrl;
				
				//	Do we need to add a filename?
				if((strFileSpec.EndsWith("/") == true) || (strFileSpec.EndsWith("\\") == true))
					strFileSpec += m_strLocalFilename;
				
			}// if(strUrl.IndexOf("://") < 0)
			
			return strFileSpec;
				
		}// public string GetServerFileSpec(string strServer)

		/// <summary>Called to display the object's property values in a standard MessageBox<\summary>
		/// <param name="strTitle">The title to be displayed in the message box</param>
		/// <returns>OK or Cancel</returns>
		public DialogResult MsgBox(string strTitle)
		{
			string strMsg = "";
			
			strMsg += ("Available - " + this.Available.ToString() + "\n");
			strMsg += ("Component - " + this.Component + "\n");
			strMsg += ("Description - " + this.Description + "\n");
			strMsg += ("Downloaded - " + this.Downloaded.ToString() + "\n");
			strMsg += ("GroupId - " + this.GroupId + "\n");

			strMsg += ("IgnoreVersion - " + this.IgnoreVersion.ToString() + "\n");
			strMsg += ("Installed - " + this.Installed.ToString() + "\n");
			strMsg += ("InstalledKeyPath - " + this.InstalledKeyPath + "\n");
			strMsg += ("InstalledValue - " + this.InstalledValue + "\n");
			strMsg += ("InstalledValueName - " + this.InstalledValueName + "\n");

			strMsg += ("LocalFilename - " + this.LocalFilename + "\n");
			strMsg += ("RequiredProductVer - " + this.RequiredProductVer + "\n");
			strMsg += ("Password - " + this.Password + "\n");
			strMsg += ("Selected - " + this.Selected.ToString() + "\n");
			strMsg += ("Sequence - " + this.Sequence.ToString() + "\n");

			strMsg += ("Url - " + this.Url + "\n");
			strMsg += ("UserName - " + this.UserName + "\n");
			strMsg += ("Version - " + this.Version + "\n");

			return MessageBox.Show(strMsg, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

		}// public DialogResult MsgBox(string strTitle)
					
		#endregion Public Methods
		
		#region Properties
		
		//	The name of the product component being updated
		public string Component
		{
			get{ return m_strComponent; }
			set{ m_strComponent = value; }
		}
		
		//	The update description presented to the user
		public string Description
		{
			get{ return m_strDescription; }
			set{ m_strDescription = value; }
		}
		
		//	The version identifier
		public string Version
		{
			get{ return m_strVersion; }
			set { m_strVersion = value; }
		}
		
		//	The address of the file to be downloaded
		public string Url
		{
			get{ return m_strUrl; }
			set{ m_strUrl = value; }
		}
		
		//	The user name used to connect to the site
		public string UserName
		{
			get{ return m_strUserName; }
			set{ m_strUserName = value; }
		}
		
		//	The group id used to filter visible updates
		public string GroupId
		{
			get{ return m_strGroupId; }
			set{ m_strGroupId = value; }
		}
		
		//	The password used to connect to the site
		public string Password
		{
			get{ return m_strPassword; }
			set{ m_strPassword = value; }
		}
		
		//	The name assigned to the file on the local machine
		public string LocalFilename
		{
			get{ return m_strLocalFilename; }
			set{ m_strLocalFilename = value; }
		}
		
		//	True if available to be selected for installation
		public bool Available
		{
			get{ return m_bAvailable; }
			set{ m_bAvailable = value; }
		}
		
		//	True if user selected for installation
		public bool Selected
		{
			get{ return m_bSelected; }
			set{ m_bSelected = value; }
		}
		
		//	True if the file has been downloaded
		public bool Downloaded
		{
			get{ return m_bDownloaded; }
			set{ m_bDownloaded = value; }
		}
		
		//	True if the update has been installed
		public bool Installed
		{
			get{ return m_bInstalled; }
			set{ m_bInstalled = value; }
		}
		
		//	True to ignore version information
		public bool IgnoreVersion
		{
			get{ return m_bIgnoreVersion; }
			set{ m_bIgnoreVersion = value; }
		}

		//	The version identifier of the product installation required
		public string RequiredProductVer
		{
			get { return m_strRequiredProductVer; }
			set { m_strRequiredProductVer = value; }
		}

		//	Sequence number used for ordered updates
		public int Sequence
		{
			get{ return m_iSequence; }
			set{ m_iSequence = value; }
		}

		//	The path to key to be checked for previous installations of the component
		public string InstalledKeyPath
		{
			get { return m_strInstalledKeyPath; }
			set { m_strInstalledKeyPath = value; }
		}

		//	The name of the value to be checked for previous installations of the component
		public string InstalledValueName
		{
			get { return m_strInstalledValueName; }
			set { m_strInstalledValueName = value; }
		}

		//	The alue to be checked for previous installations of the component
		public string InstalledValue
		{
			get { return m_strInstalledValue; }
			set { m_strInstalledValue = value; }
		}

		#endregion Properties
		
	}// public class CXmlUpdate

	/// <summary>Objects of this class are used to manage a dynamic array of CXmlUpdate objects</summary>
	public class CXmlUpdates : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlUpdates() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="xmlUpdate">CXmlUpdate object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlUpdate Add(CXmlUpdate xmlUpdate)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlUpdate as object);

				return xmlUpdate;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlUpdate xmlUpdate)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="xmlUpdate">The object to be removed</param>
		public void Remove(CXmlUpdate xmlUpdate)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlUpdate as object);
			}
			catch
			{
			}
		}
		
		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlUpdate">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlUpdate xmlUpdate)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlUpdate as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CXmlUpdate this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlUpdate);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlUpdate value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CXmlUpdates
		
}// namespace FTI.Shared.Xml
