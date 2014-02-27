using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the paths used to access the media registered in a TrialMax database</summary>
	public class CTmaxPathMap : ITmaxSortable
	{
		#region Constants
		
		private const string XMLINI_NAME		= "Name";
		private const string XMLINI_DELETED_BY	= "Deleted";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Id property</summary>
		private int m_iId = 0;
		
		/// <summary>Local member bound to DeletedBy property</summary>
		private string m_strDeletedBy = "";
		
		/// <summary>Local member bound to CasePaths property</summary>
		private CTmaxOptions m_tmaxCasePaths = new FTI.Shared.Trialmax.CTmaxOptions();

		#endregion Private Members
	
		#region Public Methods
		
		/// <summary>Default Constructor</summary>
		public CTmaxPathMap()
		{
			Initialize(0, "");
		}
		
		/// <summary>Constructor</summary>
		/// <param name="iId">Id to be assigned to the map</param>
		public CTmaxPathMap(int iId)
		{
			Initialize(iId, "");
		}
		
		/// <summary>Constructor</summary>
		/// <param name="iId">Id to be assigned to the map</param>
		/// <param name="strName">Name to be assigned to the map</param>
		public CTmaxPathMap(int iId, string strName)
		{
			Initialize(iId, strName);
		}
		
		/// <summary>This method is called to get the name of the section in the XML configuration file for this machine</summary>
		/// <returns>The name of the section containing the configuration information for this machine</returns>
		public string GetXmlSectionName()
		{
			return (GetXmlSectionPrefix() + m_iId.ToString());
		
		}// public string GetXmlSectionName()
		
		/// <summary>This method is called to get the prefix of the section in the XML configuration file for this object</summary>
		/// <returns>The prefix of the section containing the configuration information for this object</returns>
		static public string GetXmlSectionPrefix()
		{
			return ("trialMax/pathMap/");
		
		}// static public string GetXmlSectionPrefix()
		
		/// <summary>This method is called to initialize the class members</summary>
		/// <param name="iId">Id to be assigned to the map</param>
		/// <param name="strName">Name to be assigned to the map</param>
		public void Initialize(int iId, string strName)
		{
			CTmaxOption tmaxOption = null;
			
			Debug.Assert(strName != null);
			Debug.Assert(m_tmaxCasePaths != null);
			
			//	Store the caller specified values
			m_iId = iId;
			m_strName = strName != null ? strName : "";
		
			//	Initialize the CasePaths collection
			m_tmaxCasePaths.Name = "CasePaths";
			foreach(string O in Enum.GetNames(typeof(TmaxCaseFolders)))
			{
				tmaxOption = new CTmaxOption();
				tmaxOption.Text = O;
				tmaxOption.Value = "";
				tmaxOption.Selectable = false;
				
				m_tmaxCasePaths.Add(tmaxOption);

			}// foreach(string O in Enum.GetNames(typeof(TmaxCaseFolders)))
				
		}// public void Initialize(string strName)
		
		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		///	<returns>true if successful</returns>
		public bool Load(CXmlIni xmlIni)
		{
			string strOldSection = "";
			
			if(GetXmlSectionName().Length == 0) return false;
			
			//	Line up on the section that has the same name
			strOldSection = xmlIni.Section;
			
			if(xmlIni.SetSection(GetXmlSectionName(), false) == true)
			{
				//	Get the map's attributes
				m_strName = xmlIni.Read(XMLINI_NAME, "");
				m_strDeletedBy = xmlIni.Read(XMLINI_DELETED_BY, "");
			
				//	Is this a valid map section?
				if(m_strName.Length > 0)
				{
					//	Read each of the folder paths
					foreach(CTmaxOption O in m_tmaxCasePaths)
					{
						O.Value = xmlIni.Read(O.Text, O.Value);
					}
					
				}
			
				//	Restore the section
				if(strOldSection.Length > 0)
					xmlIni.SetSection(strOldSection);
				
			}// if(xmlIni.SetSection(GetXmlSectionName()) == true)
				
			return (m_strName.Length > 0);
				
		}// public bool Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the option values</param>
		public void Save(CXmlIni xmlIni)
		{
			string strOldSection = "";
			
			if(GetXmlSectionName().Length == 0) return;
			
			//	Line up on the section that has the same name
			strOldSection = xmlIni.Section;
			
			if(xmlIni.SetSection(GetXmlSectionName()) == true)
			{
				xmlIni.Write(XMLINI_NAME, m_strName);
				xmlIni.Write(XMLINI_DELETED_BY, m_strDeletedBy);

				//	Save the path information
				foreach(CTmaxOption O in m_tmaxCasePaths)
				{
					xmlIni.Write(O.Text, O.Value);
				}
				
			}// if(xmlIni.SetSection(GetXmlSectionName()) == true)
			
			//	Restore the section
			if(strOldSection.Length > 0)
				xmlIni.SetSection(strOldSection);

		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to get the path assigned to the specified folder</summary>
		/// <param name="eFolder">The enumerator that identifies the desired folder</param>
		/// <returns>The path assigned to the folder</returns>
		public string GetCasePath(TmaxCaseFolders eFolder)
		{
			string		strPath = "";
			CTmaxOption	tmaxOption = null;
			
			if(m_tmaxCasePaths != null)
			{
				if((tmaxOption = m_tmaxCasePaths.Find(eFolder.ToString())) != null)
				{
					if(tmaxOption.Value != null)
						strPath = tmaxOption.Value.ToString();
				}
				
			}	
			
			return strPath;
			
		}// public string GetCasePath(TmaxCaseFolders eFolder)
		
		/// <summary>This method is called to set the path assigned to the specified folder</summary>
		/// <param name="eFolder">The enumerator that identifies the desired folder</param>
		/// <param name="strPath">The path to be assigned to the folder</param>
		public void SetCasePath(TmaxCaseFolders eFolder, string strPath)
		{
			CTmaxOption	tmaxOption = null;
			
			if(m_tmaxCasePaths != null)
			{
				if((tmaxOption = m_tmaxCasePaths.Find(eFolder.ToString())) == null)
				{
					//	Add an option for this folder
					tmaxOption = new CTmaxOption();
					tmaxOption.Text = eFolder.ToString();
					tmaxOption.Value = "";
					tmaxOption.Selectable = false;

					m_tmaxCasePaths.Add(tmaxOption);
				}
				
				Debug.Assert(tmaxOption != null);
				
				//	Set the specified path to the folder
				if(strPath != null)
					tmaxOption.Value = strPath;	
				else
					tmaxOption.Value = "";

			}	
			
		}// public void SetCasePath(TmaxCaseFolders eFolder, string strPath)
		
		/// <summary>This function is called to compare the specified file to this file</summary>
		/// <param name="tmaxFile">The file to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this file less than tmaxFile, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxPathMap tmaxPathMap, long lMode)
		{
			if(this.Id < tmaxPathMap.Id) 
				return -1;
			else if(this.Id > tmaxPathMap.Id) 
				return 1;
			else
				return 0;
		
		}// public int Compare(CTmaxPathMap tmaxPathMap, long lMode)
		
		/// <summary>This method is required to support the ITmaxSortable interface</summary>
		/// <param name="tmaxFile">The file to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this file less than tmaxFile, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try
			{
				return Compare((CTmaxPathMap)O, lMode);
			}
			catch
			{
				return -1;
			}
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Identifier assigned to the map</summary>
		public int Id
		{
			get { return m_iId; }
			set { m_iId = value; }
		}
		
		/// <summary>Name used to identify the map</summary>
		public string Name
		{
			get { return m_strName; } 
			set { m_strName = value; }
		}
		
		/// <summary>Collection of case path specifications</summary>
		public CTmaxOptions CasePaths
		{
			get { return m_tmaxCasePaths; }
		}

		/// <summary>Name of user that deleted this map</summary>
		public string DeletedBy
		{
			get { return m_strDeletedBy; } 
			set { m_strDeletedBy = value; }
		}
		
		/// <summary>True if the map has been deleted</summary>
		public bool IsDeleted
		{
			get { return (m_strDeletedBy.Length > 0); }
		}

		#endregion Properties
	
	}// public class CTmaxPathMap

	/// <summary>This class manages a list of search results</summary>
	public class CTmaxPathMaps : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxPathMaps() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxPathMap">CTmaxPathMap object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxPathMap Add(CTmaxPathMap tmaxPathMap)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxPathMap as object);

				return tmaxPathMap;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxPathMap Add(CTmaxPathMap tmaxPathMap)

		/// <summary>This method allocates a new object and adds it to the collection</summary>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxPathMap Add()
		{
			int iId = 1;
			CTmaxPathMap tmaxPathMap = null;
			
			try
			{
				//	Get the highest identifier in this collection
				foreach(CTmaxPathMap O in this)
				{
					if(O.Id > iId) 
						iId = O.Id;
				}
					
				//	Allocate and initialize a new map
				tmaxPathMap = new CTmaxPathMap(iId + 1);
				tmaxPathMap.Name = ("PATH MAP " + tmaxPathMap.Id.ToString());	
			
				//	Add it to the collection
				return Add(tmaxPathMap);
				
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxPathMap Add()

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxPathMap">The filter object to be removed</param>
		public void Remove(CTmaxPathMap tmaxPathMap)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxPathMap as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxPathMap tmaxPathMap)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxPathMap">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxPathMap tmaxPathMap)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxPathMap as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxPathMap this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxPathMap);
			}
		}

		/// <summary>This method is called to populate the collection using values stored in the specified XML configuration file</summary>
		/// <param name="xmlIni">The initialization file containing the aliases</param>
		public void Load(CXmlIni xmlIni)
		{
			CTmaxPathMap tmaxPathMap = null;
			
			//	Clear the existing objects
			this.Clear();
			
			//	Add the maps defined in the file
			for(int i = 1; i < 100000; i++)
			{
				//	Allocate a new map
				tmaxPathMap = new CTmaxPathMap(i, "");
				
				//	Read this map
				if(tmaxPathMap.Load(xmlIni) == false)
					break; // We've run out of maps
				else
					this.Add(tmaxPathMap);
					
			}// for(int i = 1; i < 100000; i++)
			
			//	Add a default if no map is defined in the file
			if(this.Count == 0)
				this.Add(new CTmaxPathMap(1, "WAR ROOM"));
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the objects in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the values</param>
		public void Save(CXmlIni xmlIni)
		{
			//	Save each object to the file
			foreach(CTmaxPathMap O in this)
			{
				try	  { O.Save(xmlIni); }
				catch {}
			}
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to get the map with the specified id</summary>
		/// <param name="iId">The unique map identifier</param>
		/// <returns>The map with the specified id</returns>
		public CTmaxPathMap Find(int iId)
		{
			foreach(CTmaxPathMap O in this)
			{
				if(O.Id == iId)
					return O;
			}
			return null;
		
		}// public CTmaxPathMap Find(int iId)
		
		/// <summary>This method is called to get the map with the specified name</summary>
		/// <param name="iId">The name of the map to be located</param>
		/// <returns>The map with the specified name</returns>
		public CTmaxPathMap Find(string strName)
		{
			foreach(CTmaxPathMap O in this)
			{
				if(String.Compare(O.Name, strName, true) == 0)
					return O;
			}
			return null;
		
		}// public CTmaxPathMap Find(string strName)
		
		#endregion Public Methods
		
	}//	public class CTmaxPathMaps
		
}// namespace FTI.Shared.Trialmax
