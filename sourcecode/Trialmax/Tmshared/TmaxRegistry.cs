using System;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.Security.Permissions;
using Microsoft.Win32;

[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
ViewAndModify = "HKEY_LOCAL_MACHINE")]

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to manage the application's registry entries</summary>
	public class CTmaxRegistry
	{
		#region Constants
		
		private const int ERROR_OPEN_SUBKEY_EX		= 0;
		private const int ERROR_GET_KEY_VALUE_EX	= 1;
		private const int ERROR_SET_KEY_VALUE_EX	= 2;
		private const int ERROR_SET_COMPONENT_EX	= 3;
		private const int ERROR_GET_COMPONENT_EX	= 4;
		private const int ERROR_GET_COMPONENTS_EX	= 5;
		
		private const string TMAX_REGISTRY_LAST_CASE_VALUE_NAME	= "LastCase01";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		protected CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		protected CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to UseLocalMachine property</summary>
		private bool m_bUseLocalMachine = true;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxRegistry()
		{
			SetErrorStrings();
			
			m_tmaxEventSource.Name = "TmaxRegistry Events";
		}
		
		/// <summary>This method will initialize the connection to the system registry</summary>
		/// <returns>true if successful</returns>
		public bool Initialize()
		{
			return true;
		
		}// public bool Initialize()
		
		/// <summary>This method will get the collection of registered components</summary>
		///	<param name="tmaxComponents">An option collection in which to store the components</param>
		/// <returns>The collection of components</returns>
		public FTI.Shared.Trialmax.CTmaxComponents GetComponents(CTmaxComponents tmaxComponents)
		{
			RegistryKey		rkProduct = null;
			CTmaxComponent	tmaxComponent = null;
			
			try
			{
				//	Open the Product subkey
				if((rkProduct = OpenSubKey(TmaxRegistryKeys.Product, false, true)) != null)
				{
					if(rkProduct.SubKeyCount > 0)
					{
						string [] aSubKeys = rkProduct.GetSubKeyNames();
						
						if((aSubKeys != null) && (aSubKeys.GetUpperBound(0) >= 0))
						{
							//	Do we need to allocate the collection?
							if(tmaxComponents == null)
								tmaxComponents = new CTmaxComponents();
								
							//	Add each of the component subkeys
							for(int i = 0; i <= aSubKeys.GetUpperBound(0); i++)
							{
								//	Create a new component object
								tmaxComponent = new CTmaxComponent();
								tmaxComponent.Name = aSubKeys[i];
								
								//	Read the values for this component
								GetComponent(tmaxComponent);
								
								//	Add to the collection
								tmaxComponents.Add(tmaxComponent);
							
							}// foreach(string O in aSubKeys)
							
						}// if((aSubKeys != null) && (aSubKeys.GetUpperBound(0) >= 0))
				
					}// if(rkProduct.SubKeyCount > 0)

				}// if((rkProduct = OpenSubKey(PATH_PRODUCT_KEY, false, true)) != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetComponents", m_tmaxErrorBuilder.Message(ERROR_GET_COMPONENTS_EX), Ex);
			}
			finally
			{
				if(rkProduct != null)
					rkProduct.Close();
			}
			
			return tmaxComponents;
		
		}// public FTI.Shared.Trialmax.CTmaxComponents GetComponents(CTmaxComponents tmaxComponents)
		
		/// <summary>This method will store the component descriptor in the registry</summary>
		///	<param name="tmaxComponent">The component to be stored in the registry</param>
		/// <returns>True if successful</returns>
		public bool SetComponent(CTmaxComponent tmaxComponent)
		{
			RegistryKey	rkComponent = null;
			string		strPath = "";
			bool		bSuccessful = false;
			
			//	The component MUST have a name
			if((tmaxComponent == null) || (tmaxComponent.Name.Length == 0))
				return false;
			
			try
			{
				//	Build the path to the component key
				strPath = GetKeyPath(tmaxComponent);
				
				//	Open the component subkey
				if((rkComponent = OpenSubKey(strPath, true, false)) != null)
				{
					//	Assume we are going to be successful
					bSuccessful = true;
					
					//	Set the component values
					try { rkComponent.SetValue("Description", tmaxComponent.Description); }
					catch { bSuccessful = false; }
				
					try { rkComponent.SetValue("Version", tmaxComponent.Version); }
					catch { bSuccessful = false; }
				
					try { rkComponent.SetValue("Folder", tmaxComponent.Folder); }
					catch { bSuccessful = false; }
				
					try { rkComponent.SetValue("Filename", tmaxComponent.Filename); }
					catch { bSuccessful = false; }
				
				}// if((rkComponent = OpenSubKey(strPath, true, false)) != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetComponent", m_tmaxErrorBuilder.Message(ERROR_SET_COMPONENT_EX, tmaxComponent.Name), Ex);
			}
			finally
			{
				if(rkComponent != null)
					rkComponent.Close();
			}
			
			return bSuccessful;
		
		}// public bool SetComponent(CTmaxComponent tmaxComponent)
		
		/// <summary>This method will retrieve the component descriptor stored in the registry</summary>
		///	<param name="tmaxComponent">The component stored in the registry</param>
		/// <returns>True if successful</returns>
		public bool GetComponent(CTmaxComponent tmaxComponent)
		{
			RegistryKey	rkComponent = null;
			string		strPath = "";
			bool		bSuccessful = false;
			
			//	The component MUST have a name
			if((tmaxComponent == null) || (tmaxComponent.Name.Length == 0))
				return false;
			
			try
			{
				//	Build the path to the component key
				strPath = GetKeyPath(tmaxComponent);
				
				//	Open the component subkey
				if((rkComponent = OpenSubKey(strPath, false, true)) != null)
				{
					//	Assume we are going to be successful
					bSuccessful = true;
					
					//	Set the component values
					try { tmaxComponent.Description = rkComponent.GetValue("Description").ToString(); }
					catch { bSuccessful = false; }
				
					try { tmaxComponent.Version = rkComponent.GetValue("Version").ToString(); }
					catch { bSuccessful = false; }

					try { tmaxComponent.Folder = rkComponent.GetValue("Folder").ToString(); }
					catch { bSuccessful = false; }

					try { tmaxComponent.Filename = rkComponent.GetValue("Filename").ToString(); }
					catch { bSuccessful = false; }

				}// if((rkComponent = OpenSubKey(strPath, false, true)) != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetComponent", m_tmaxErrorBuilder.Message(ERROR_GET_COMPONENT_EX, tmaxComponent.Name), Ex);
			}
			finally
			{
				if(rkComponent != null)
					rkComponent.Close();
			}
			
			return bSuccessful;
		
		}// public bool GetComponent(CTmaxComponent tmaxComponent)
		
		/// <summary>This method will delete the component descriptor stored in the registry</summary>
		///	<param name="tmaxComponent">The component stored in the registry</param>
		/// <returns>True if successful</returns>
		public bool RemoveComponent(CTmaxComponent tmaxComponent)
		{
			RegistryKey	rkProduct = null;
			bool		bSuccessful = true;
			
			//	The component MUST have a name
			if((tmaxComponent == null) || (tmaxComponent.Name.Length == 0))
				return false;
			
			try
			{
				//	Open the product subkey
				if((rkProduct = OpenSubKey(TmaxRegistryKeys.Product, false, false)) != null)
					rkProduct.DeleteSubKey(tmaxComponent.Name, false);
			}
			catch
			{
				//	We intentionally do not report an error here because the
				//	user may not have admin rights
				
				bSuccessful = false;
			}
			finally
			{
				if(rkProduct != null)
					rkProduct.Close();
			}
			
			return bSuccessful;
		
		}// public bool RemoveComponent(CTmaxComponent tmaxComponent)
		
		/// <summary>This method will delete the specified subkey in the registry</summary>
		///	<param name="strParent">The path to the parent key</param>
		///	<param name="strSubKey">The name of the subkey to be deleted</param>
		/// <returns>True if successful</returns>
		public bool RemoveSubKey(string strParent, string strSubKey)
		{
			RegistryKey	rkParent = null;
			bool		bSuccessful = true;
			
			Debug.Assert(strParent != null);
			Debug.Assert(strParent.Length != 0);
			if((strParent == null) || (strParent.Length == 0))
				return false;
			
			Debug.Assert(strSubKey != null);
			Debug.Assert(strSubKey.Length != 0);
			if((strSubKey == null) || (strSubKey.Length == 0))
				return false;
			
			try
			{
				//	Open the parent subkey
				if((rkParent = OpenSubKey(strParent, false, false)) != null)
					rkParent.DeleteSubKey(strSubKey, false);
			}
			catch
			{
				bSuccessful = false;
			}
			finally
			{
				if(rkParent != null)
					rkParent.Close();
			}
			
			return bSuccessful;
		
		}// public bool RemoveSubKey(string strParent, string strSubKey)
		
		/// <summary>This method will terminate the connection to the system registry</summary>
		public void Terminate()
		{
		
		}
		
		/// <summary>This method will set the activation code</summary>
		/// <param name="strCode">The activation code to be stored in the registry</param>
		/// <returns>true if successful</returns>
		public bool SetActivationCode(string strCode)
		{
			string	strActivate = "";
			bool	bSuccessful = false;
			
			//	Get the path to the key used to store the activation code
			strActivate = GetKeyPath(TmaxRegistryKeys.Activate);
			
			if(strActivate.Length > 0)
			{
				//	Update the registry
				if(SetKeyValue(strActivate, "", strCode) == true)
				{
					bSuccessful = true;
				}
			
			}// if(strActivate.Length > 0)
			
			return bSuccessful;
		
		}// public bool SetActivationCode(string strCode)
		
		/// <summary>This method will get the activation code stored in the registry</summary>
		/// <returns>The activation code if it exists</returns>
		public string GetActivationCode()
		{
			string	strActivate = "";
			string	strCode = "";
			
			//	Get the path to the key used to store the activation code
			strActivate = GetKeyPath(TmaxRegistryKeys.Activate);
			
			if(strActivate.Length > 0)
			{
				//	Get the value
				strCode = GetKeyValue(strActivate, "");
			
			}// if(strActivate.Length > 0)
			
			return strCode;
		
		}// public string GetActivationCode()
		
		/// <summary>This method is called to get the "last case" value stored in the registry</summary>
		/// <returns>The path to the last case</returns>
		/// <remarks>For demo purposes, the installation will write the last case value to the registry</remarks>
		public string GetLastCase()
		{
			return GetKeyValue(TmaxRegistryKeys.LastCase, TMAX_REGISTRY_LAST_CASE_VALUE_NAME);
			
		}// public string GetLastCase()
		
		/// <summary>This method is called to set the "last case" value stored in the registry</summary>
		/// <param name="strLastCase">The path to the last case</param>
		/// <returns>True if successful</returns>
		public bool SetLastCase(string strLastCase)
		{
			if(strLastCase != null)
				return SetKeyValue(TmaxRegistryKeys.LastCase, TMAX_REGISTRY_LAST_CASE_VALUE_NAME, strLastCase);
			else
				return SetKeyValue(TmaxRegistryKeys.LastCase, TMAX_REGISTRY_LAST_CASE_VALUE_NAME, "");
				
		}// public bool SetLastCase(string strLastCase)
		
		/// <param name="strPath">The path (relative to the local machine key) to the desired subkey</param>
		///	<param name="bCreate">True to create the subkey if it does not exist</param>
		///	<param name="bReadOnly">true to open the key read-only</param>
		/// <returns>The subkey if successful</returns>
		/// <remarks>The caller is responsible for closing the key</remarks>
		public RegistryKey OpenSubKey(string strPath, bool bCreate, bool bReadOnly)
		{
			RegistryKey rkSystem = null;
			RegistryKey	rkUser = null;
			
			try
			{
				//	Get the top level system key
				if((rkSystem = GetSystemKey()) != null)
				{
					//	Open the subkey requested by the user
					if(bCreate == true)
						rkUser = rkSystem.CreateSubKey(strPath);
					else
						rkUser = rkSystem.OpenSubKey(strPath, !bReadOnly);
				
				}// if(rkSystem != null)
				
			}
			catch(System.Exception)
			{
				//	Do not report this as an error because the user may not have sufficient 
				//	rights. It's up to the caller to determine if a failure to open the key 
				//	should result in an error
				//m_tmaxEventSource.FireError(this, "OpenSubKey", m_tmaxErrorBuilder.Message(ERROR_OPEN_SUBKEY_EX, "[HKEY_LOCAL_MACHINE]\\" + strPath, bReadOnly, bCreate), Ex);
			
			}
			
			//	Clean up
			if(rkSystem != null)
				rkSystem.Close();
				
			return rkUser;
			
		}// public RegistryKey OpenSubKey(string strPath, bool bReadOnly)

		/// <summary>This method is called to get the subkey at the specified path</summary>
		/// <param name="eKey">The enumerated TrialMax key identifier</param>
		///	<param name="bCreate">True to create the subkey if it does not exist</param>
		///	<param name="bReadOnly">true to open the key read-only</param>
		/// <returns>The subkey if successful</returns>
		public RegistryKey OpenSubKey(TmaxRegistryKeys eKey, bool bCreate, bool bReadOnly)
		{
			return OpenSubKey(GetKeyPath(eKey), bCreate, bReadOnly);
		}

		/// <summary>This method is called to get the value of the specified key/name pair</summary>
		///	<param name="strPath">The path to the desired key</param>
		/// <param name="strName">The name of the key value being retrieved</param>
		/// <returns>The current value as a string</returns>
		public string GetKeyValue(string strPath, string strName)
		{
			RegistryKey rkUser = null;
			string		strValue = "";
			
			try
			{
				//	Get the key requested by the user
				if((rkUser = OpenSubKey(strPath, false, true)) != null)
				{
					//	Retrieve the path to the executable
					if(rkUser.GetValue(strName) != null)
					{
						strValue = rkUser.GetValue(strName).ToString();
					}
							
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetKeyValue", m_tmaxErrorBuilder.Message(ERROR_GET_KEY_VALUE_EX, "[HKEY_LOCAL_MACHINE]\\" + strPath, strName), Ex);
			}
			
			//	Clean up
			if(rkUser != null)
				rkUser.Close();
				
			return strValue;
			
		}// public string GetKeyValue(string strPath, string strName)
		
		/// <summary>This method is called to get the requested value from the specified key/name pair</summary>
		///	<param name="eKey">The enumerated TrialMax registry key</param>
		/// <param name="strName">The name of the value being retrieved</param>
		/// <returns>The current value as a string</returns>
		public string GetKeyValue(TmaxRegistryKeys eKey, string strName)
		{
			string strPath = "";
			string strValue = "";
			
			try
			{
				//	Translate the enumerator to it's registry path
				strPath = GetKeyPath(eKey);
				
				//	Read the value
				strValue = GetKeyValue(strPath, strName);				
			}
			catch
			{
				strValue = "";
			}
			
			return strValue;
			
		}// public string GetKeyValue(TmaxRegistryKeys eKey, string strName)
		
		/// <summary>This method is called to set the value of the specified key/name pair</summary>
		///	<param name="strPath">The path to the desired key</param>
		/// <param name="strName">The name of the key value being set</param>
		/// <param name="oValue">the value to be assigned to the key/name pair</param>
		/// <returns>true if successful</returns>
		public bool SetKeyValue(string strPath, string strName, object oValue)
		{
			RegistryKey rkUser = null;
			bool		bSuccessful = false;
			
			try
			{
				//	Get the key requested by the user
				if((rkUser = OpenSubKey(strPath, true, false)) != null)
				{
					//	Set the requested value
					rkUser.SetValue(strName, oValue);

					bSuccessful = true;							
				}
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetKeyValue", m_tmaxErrorBuilder.Message(ERROR_SET_KEY_VALUE_EX, "[HKEY_LOCAL_MACHINE]\\" + strPath, strName), Ex);
			}
			
			//	Clean up
			if(rkUser != null)
				rkUser.Close();
				
			return bSuccessful;
			
		}// public bool SetKeyValue(string strPath, string strName, object oValue)
		
		/// <summary>This method is called to set the requested value in the specified registry key</summary>
		///	<param name="eKey">The enumerated TrialMax registry key</param>
		/// <param name="strName">The name of the key value being set</param>
		/// <param name="oValue">the value to be assigned to the key/name pair</param>
		/// <returns>true if successful</returns>
		public bool SetKeyValue(TmaxRegistryKeys eKey, string strName, object oValue)
		{
			string strPath = "";
			
			try
			{
				//	Translate the enumerator to it's registry path
				strPath = GetKeyPath(eKey);
				
				//	Set the value
				return SetKeyValue(strPath, strName, oValue);				
			}
			catch
			{
			}
			
			return false;
			
		}// public bool SetKeyValue(TmaxRegistryKeys eKey, string strName, object oValue)
		
		/// <summary>This method will assemble the path to the key for the specified component</summary>
		///	<param name="tmaxComponent">The component to be located</param>
		/// <returns>The path to the component key</returns>
		public string GetKeyPath(CTmaxComponent tmaxComponent)
		{
			string strPath = "";
			
			//	The component MUST have a name
			if((tmaxComponent != null) && (tmaxComponent.Name.Length > 0))
			{
				strPath = String.Format("{0}\\{1}", GetKeyPath(TmaxRegistryKeys.Product), tmaxComponent.Name);
			}
			
			return strPath;
		
		}// public string GetKeyPath(CTmaxComponent tmaxComponent)
		
		/// <summary>This method will assemble the path to the specified TrialMax registry key</summary>
		///	<param name="eKey">The desired TrialMax key</param>
		/// <returns>The path to the specified key</returns>
		public string GetKeyPath(TmaxRegistryKeys eKey)
		{
			//	Which key?
			switch(eKey)
			{
				case TmaxRegistryKeys.Root:
				
					return "Software\\FTI Consulting\\TrialMax 7";
					
				case TmaxRegistryKeys.Product:
				
					return (GetKeyPath(TmaxRegistryKeys.Root) + "\\Product");
					
				case TmaxRegistryKeys.Activate:
				
					//	Use the default value assigned to the Product key to store the activation code
					return GetKeyPath(TmaxRegistryKeys.Product);
					
				case TmaxRegistryKeys.LastCase:

					return "Software\\FTI Consulting\\TrialMax";
					
				default:
				
					Debug.Assert(false, eKey.ToString() + " is an unhandled registry key");
					return "";
			
			}// switch(eKey)
		
		}// public string GetKeyPath(TmaxRegistryKeys eKey)
		
		/// <summary>This method is called to get the system key used as the root for all TrialMax entries in the registry</summary>
		/// <returns>The system subkey if successful</returns>
		public RegistryKey GetSystemKey()
		{
			////	Are we running on 64-bit machine?
			//if(IntPtr.Size > 4)
			//{
			//Registry.
			//}
			//else
			//{
				//	Are we using the local machine?
				if(this.UseLocalMachine)
					return Registry.LocalMachine;
				else
					return Registry.CurrentUser;
			//}
						
		}// public RegistryKey GetSystemKey()

		/// <summary>Called to get the path to the ActiveX control or COM library with the specified class id</summary>
		/// <param name="strClassId">The registered class id</param>
		/// <returns>the path to the specified control or library</returns>
		public string GetRegisteredPath(string strClassId)
		{
			RegistryKey rkSubKey = null;
			string		strPath = "";
			string		strSubKey = "";
			
			try
			{
				//	Do we have a valid Classes key?
				if(Registry.ClassesRoot == null) return "";
				
				//	Make sure the class id is bounded by the brackets
				if(strClassId.StartsWith("{") == false)
					strClassId = ("{" + strClassId);
				if(strClassId.EndsWith("}") == false)
					strClassId += "}";
					
				//	Build the path to the subkey
				strSubKey  = String.Format("CLSID\\{0}\\InprocServer32", strClassId);

				if((rkSubKey = Registry.ClassesRoot.OpenSubKey(strSubKey)) != null)
				{
					strPath = rkSubKey.GetValue("").ToString();
				}
				
			}
			catch
			{
			}
			
			return strPath;
			
		}// public string GetRegisteredPath(string strClassId)
		
		/// <summary>Called to get the path to the ActiveX control or COM library with the specified class id</summary>
		/// <param name="classId">The registered class id</param>
		/// <returns>the path to the specified control or library</returns>
		public string GetRegisteredPath(System.Guid classId)
		{
			return GetRegisteredPath(classId.ToString());
		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to retrieve the registry subkey: path = %1  read-only = %2 create = %3");
			aStrings.Add("An exception was raised while attempting to read the registry key value: path = %1  name = %2");
			aStrings.Add("An exception was raised while attempting to set the registry key value: path = %1  name = %2");
			aStrings.Add("An exception was raised while attempting to set the values for the product component: name = %1");
			aStrings.Add("An exception was raised while attempting to get the values for the product component: name = %1");
			aStrings.Add("An exception was raised while attempting to read the product components from the registry");

		}// protected virtual void SetErrorStrings()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source used to fire system events</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>True to use HKE_LOCAL_MACHINE instead of HKEY_CURRENT_USER as the root key</summary>
		public bool UseLocalMachine
		{
			get { return m_bUseLocalMachine; }
			set { m_bUseLocalMachine = value; }
		}
		
		#endregion Properties
	
	}// public class CTmaxRegistry

}// namespace FTI.Shared.Trialmax

