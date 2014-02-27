using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information required to register a source file</summary>
	public class CTmaxSourceFile : ITmaxSortable
	{
		/// <summary>Local member bound to UserData property</summary>
		private object m_objUserData = null;
		
		/// <summary>Local member bound to XmlSecondary property</summary>
		private CXmlSecondary m_xmlSecondary = null;
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Path property</summary>
		private string m_strPath = "";
		
		/// <summary>Local member bound to Registered property</summary>
		private bool m_bRegistered = false;
		
		/// <summary>Local member bound to Temporary property</summary>
		private bool m_bTemporary = false;
		
		/// <summary>Local member bound to IPrimary property</summary>
		private ITmaxRecord m_IPrimary = null;
		
		/// <summary>Local member bound to ISecondary property</summary>
		private ITmaxRecord m_ISecondary = null;
		
		/// <summary>Local member bound to ITertiary property</summary>
		private ITmaxRecord m_ITertiary = null;
		
		/// <summary>Default constructor</summary>
		public CTmaxSourceFile()
		{
		}
	
		/// <summary>Default constructor</summary>
		/// <param name="strPath">The fully qualified path of the folder</param>
		public CTmaxSourceFile(string strPath)
		{
			Initialize(strPath);
		}
	
		/// <summary>
		/// This method will initialize the object using the specified path
		/// </summary>
		/// <param name="strPath">The fully qualified path of the folder</param>
		public void Initialize(string strPath)
		{
			//	Make sure we have a valid path
			if((strPath == null) || (strPath.Length == 0)) return;
			
			//	Save the path
			m_strPath = strPath;
			
			//	Extract the filename
			m_strName = System.IO.Path.GetFileName(strPath);

		}// Initialize(string strPath)
	
		/// <summary>This function is called to compare the specified file to this file</summary>
		/// <param name="tmaxFile">The file to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this file less than tmaxFile, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxSourceFile tmaxFile, long lMode)
		{
			return FTI.Shared.CTmaxToolbox.Compare(this.Name, tmaxFile.Name, true);
		
		}// public int Compare(CTmaxSourceFile tmaxFile)
		
		/// <summary>This method is required to support the ITmaxSortable interface</summary>
		/// <param name="tmaxFile">The file to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this file less than tmaxFile, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try
			{
				return Compare((CTmaxSourceFile)O, lMode);
			}
			catch
			{
				return -1;
			}
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>Name of this file (without the path)</summary>
		public string Name
		{
			get	{ return m_strName;	}
			set	{ m_strName = value; }
		}
		
		/// <summary>Path of this file</summary>
		public string Path
		{
			get { return m_strPath; }
			set { m_strPath = value; }
		}
		
		/// <summary>Flag to indicate that the object was processed and registered</summary>
		public bool Registered
		{
			get{ return m_bRegistered; }
			set { m_bRegistered = value; }
		}
		
		/// <summary>User specified data object</summary>
		public object UserData
		{
			get { return m_objUserData; }
			set { m_objUserData = value; }
		}
		
		/// <summary>Flag to indicate that the file is temporary and should be deleted after it's no longer needed</summary>
		public bool Temporary
		{
			get { return m_bTemporary; }
			set { m_bTemporary = value; }
		}
		
		/// <summary>Interface to primary data exchange object that owns this source</summary>
		public ITmaxRecord IPrimary
		{
			get { return m_IPrimary;	}
			set { m_IPrimary = value; }
		}
		
		/// <summary>Interface to secondary data exchange object associated with this source</summary>
		public ITmaxRecord ISecondary
		{
			get { return m_ISecondary; }
			set { m_ISecondary = value; }
		}
		
		/// <summary>Interface to tertiary data exchange object associated with this source</summary>
		public ITmaxRecord ITertiary
		{
			get { return m_ITertiary; }
			set { m_ITertiary = value; }
		}
		
		/// <summary>XML secondary node used to create the file object when importing from a load file</summary>
		public CXmlSecondary XmlSecondary
		{
			get { return m_xmlSecondary; }
			set { m_xmlSecondary = value; }
		}
		
	}// public class CTmaxSourceFile

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxSourceFile objects
	/// </summary>
	public class CTmaxSourceFiles : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxSourceFiles() : base()
		{
			//	Assign a default files sorter
			base.Comparer = new CTmaxSorter();
			
			this.AllowDuplicates = true;
			this.KeepSorted = true;
		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="tmaxFile">CTmaxSourceFile object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxSourceFile Add(CTmaxSourceFile tmaxFile)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxFile as object);

				return tmaxFile;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxSourceFile tmaxFile)

		/// <summary>
		/// This method is called to remove the requested filter from the collection
		/// </summary>
		/// <param name="tmaxFile">The filter object to be removed</param>
		public void Remove(CTmaxSourceFile tmaxFile)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxFile as object);
			}
			catch
			{
			}
		}

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="tmaxFile">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxSourceFile tmaxFile)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxFile as object);
		}

		/// <summary>
		/// Called to locate the object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxSourceFile GetFileFromName(string strName)
		{
			// Search for the object with the same name
			if(m_aList != null)
			{
				foreach(CTmaxSourceFile O in m_aList)
				{
					if(String.Compare(O.Name, strName, true) == 0)
					{
						return O;
					}
				}
			}
			return null;

		}//	GetFileFromName(string strName)

		/// <summary>
		/// Called to locate the object with the specified path
		/// </summary>
		/// <returns>The object with the specified path</returns>
		public CTmaxSourceFile GetFileFromPath(string strPath)
		{
			// Search for the object with the same path
			if(m_aList != null)
			{
				foreach(CTmaxSourceFile O in this)
				{
					if(String.Compare(O.Path, strPath, true) == 0)
					{
						return O;
					}
				}
			}
			return null;

		}//	GetFileFromPath(string strPath)

		/// <summary>
		/// Called to get the index of the file with the specified name
		/// </summary>
		/// <returns>The index if found</returns>
		public int GetIndexFromName(string strName)
		{
			CTmaxSourceFile tmaxFile = null;
			
			if((tmaxFile = GetFileFromName(strName)) != null)
				return IndexOf(tmaxFile);
			else
				return -1;

		}//	GetIndexFromName(string strName)

		/// <summary>
		/// Called to get the index of the file with the specified path
		/// </summary>
		/// <returns>The index if found</returns>
		public int GetIndexFromPath(string strPath)
		{
			CTmaxSourceFile tmaxFile = null;
			
			if((tmaxFile = GetFileFromPath(strPath)) != null)
				return IndexOf(tmaxFile);
			else
				return -1;

		}//	GetIndexFromPath(string strName)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public new CTmaxSourceFile this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxSourceFile);
			}
		}

		/// <summary>
		/// Overloaded [] operator to locate the parameter object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxSourceFile this[string strName]
		{
			get 
			{
				return GetFileFromName(strName);
			}
		}

		#endregion Public Methods
		
	}//	public class CTmaxSourceFiles
		
	/// <summary>This class contains information required to register a source folder</summary>
	public class CTmaxSourceFolder
	{
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Path property</summary>
		private string m_strPath = "";
		
		/// <summary>Local member bound to Suffix property</summary>
		private string m_strSuffix = "";
		
		/// <summary>Local member bound to PrimaryAttributes property</summary>
		private long m_lPrimaryAttributes = 0;
		
		/// <summary>Local member accessed by MediaType property</summary>
		private FTI.Shared.Trialmax.TmaxMediaTypes m_eMediaType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;
			
		/// <summary>Local member bound to Registered property</summary>
		private bool m_bRegistered = false;
		
		/// <summary>Local member bound to IPrimary property</summary>
		private ITmaxRecord m_IPrimary = null;
		
		/// <summary>Local member bound to Files property</summary>
		private CTmaxSourceFiles m_aFiles = new CTmaxSourceFiles();
		
		/// <summary>Local member bound to Subfolders property</summary>
		private CTmaxSourceFolders m_aSubFolders = new CTmaxSourceFolders();
		
		/// <summary>Local member bound to UserData property</summary>
		private object m_objUserData = null;
		
		/// <summary>Local member bound to XmlPrimary property</summary>
		private FTI.Shared.Xml.CXmlPrimary m_xmlPrimary = null;
		
		/// <summary>Default constructor</summary>
		public CTmaxSourceFolder()
		{
		}
	
		/// <summary>Overloaded constructor</summary>
		/// <param name="strPath">The fully qualified path of the folder</param>
		public CTmaxSourceFolder(string strPath)
		{
			Initialize(strPath);
		}
	
		/// <summary>Overloaded constructor</summary>
		/// <param name="aFilenames">An array of fully qualifed filenames</param>
		public CTmaxSourceFolder(Array aFilenames)
		{
			Initialize(aFilenames);
		}
	
		/// <summary>Counts all files in this folder and all subfolders</summary>
		/// <return>The total number of files</return>
		public long GetTotalFiles()
		{
			long lFiles = 0;
			
			//	Start by iterating the subfolders
			if((m_aSubFolders != null) && (m_aSubFolders.Count > 0))
			{
				foreach(CTmaxSourceFolder tmaxFolder in m_aSubFolders)
				{
					lFiles += tmaxFolder.GetTotalFiles();
				}
				
			}
			
			//	Now add the files
			if(m_aFiles != null)
				lFiles += m_aFiles.Count;
				
			return lFiles;
		}
	
		/// <summary>This method will initialize the object using the specified path</summary>
		/// <param name="strPath">The fully qualified path of the folder</param>
		public void Initialize(string strPath)
		{
			char[]	acDelimiters = {'\\','/'};
			
			//	Make sure we have a valid path
			if((strPath == null) || (strPath.Length == 0)) return;
			
			//	Strip the trailing separator if it exists
			m_strPath = strPath.TrimEnd(acDelimiters);
			//m_strPath = strPath;
			m_strName = "";
			
			//	Parse the path into it's component parts
			string[] aFolders = m_strPath.Split(acDelimiters);
			if((aFolders != null) && (aFolders.GetUpperBound(0) >= 0))
			{
				m_strName = aFolders[aFolders.GetUpperBound(0)];
			}
			
			if(m_strName.Length == 0)
				m_strName = m_strPath;			
		}
	
		/// <summary>This method will initialize the object using the array of fully qualified file paths</summary>
		/// <param name="strPath">The array of files to be put in the folder</param>
		/// <remarks>This method assumes all files belong to the same folder</remarks>
		public void Initialize(Array aFilenames)
		{
			//	Add each file to the local collection
			foreach(string strFilename in aFilenames)
			{
				try
				{
					Files.Add(new CTmaxSourceFile(strFilename));
				}
				catch
				{
				}
				
			}// foreach(string strFilename in aFilenames)
			
			//	Set the path information for this folder
			if(Files.Count > 0)
			{
				Initialize(System.IO.Path.GetDirectoryName(Files[0].Path));
			}
		
		}// public void Initialize(Array aFilenames)
	
		/// <summary>
		/// This method will reset the local members and clear the collections
		/// </summary>
		public void Reset()
		{
			m_strName = "";
			m_strPath = "";
			IPrimary  = null;
			m_lPrimaryAttributes = 0;
			
			if(m_aFiles != null)
				m_aFiles.Clear();
				
			if(m_aSubFolders != null)
				m_aSubFolders.Clear();
		}
		
		/// <summary>This method will set the specified primary attribute to the desired value</summary>
		public void SetPrimaryAttribute(TmaxPrimaryAttributes eAttribute, bool bTrue)
		{
			//	Are we setting the bit to true?
			if(bTrue)
				m_lPrimaryAttributes |= (uint)eAttribute;
			else
				m_lPrimaryAttributes &= (~((uint)eAttribute));
		}

		/// <summary>Called to get the relative portion of the path</summary>
		/// <returns>The path relative to it's own drive specification</returns>
		public string GetRelativePath()
		{
			string	strRelative = m_strPath;
			int		iDriveLength = 0;
			
			if(m_strPath != null && (m_strPath.Length > 0))
			{
				if((iDriveLength = GetDrive().Length) > 0)
				{
					if(strRelative.Length > iDriveLength)
						strRelative = strRelative.Remove(0, iDriveLength);
					else
						strRelative = ""; //	Path is nothing but drive
				}
				
			}// if(m_strPath != null && (m_strPath.Length > 0))
			
			if(strRelative.Length > 0)
			{
				if(strRelative.StartsWith("\\") == true)
					strRelative = strRelative.Remove(0,1);
				
				if(strRelative.EndsWith("\\") == false)
					strRelative += "\\";
			}
			
			return strRelative;
		}
		
		/// <summary>Called to get the portion of this path relative to the specified root</summary>
		/// <param name="strRoot">The path to be treated as the root</param>
		/// <param name="strRelative">Variable in which to store the relative portion of this path</param>
		/// <returns>True if this path is relative</returns>
		public bool GetRelativePath(string strRoot, ref string strRelative)
		{
			string	strPath = "";
			bool	bRelative = false;
			
			//	Make sure the paths are formatted the same
			strRoot = strRoot.ToLower();
			if(strRoot.EndsWith("\\") == false)
				strRoot += "\\";
			
			strPath = m_strPath.ToLower();
			if(strPath.EndsWith("\\") == false)
				strPath += "\\";
				
			//	Are the paths equal?
			if(String.Compare(strRoot, strPath, true) == 0)
			{
				bRelative = true;
				strRelative = "";
			}
			
			//	Is this path relative to the root?
			else if(strPath.StartsWith(strRoot) == true)
			{
				bRelative = true;
				strRelative = strPath.Substring(strRoot.Length);
			}
			
			return bRelative;
		
		}// public bool GetRelativePath(string strRoot, ref string strRelative)
		
		/// <summary>Retrieve the drive portion of the path</summary>
		/// <returns>The drive (volumn) if not a relative path</returns>
		public string GetDrive()
		{
			string strDrive = "";
			
			if(m_strPath != null && (m_strPath.Length > 0))
			{
				if(System.IO.Path.IsPathRooted(m_strPath) == true)
				{
					strDrive = System.IO.Path.GetPathRoot(m_strPath);
					
					if((strDrive.Length > 0) && (strDrive.EndsWith("\\") == false))
						strDrive += "\\";
				}
			
			}
			
			return strDrive;
			
		}// public string GetDrive()
		
		/// <summary>This method is called to check the value of the specified attribute</summary>
		/// <param name="eAttribute">The attribute in question</param>
		/// <returns>true if the attribute is set</returns>
		public bool GetPrimaryAttribute(TmaxPrimaryAttributes eAttribute)
		{
			return ((m_lPrimaryAttributes & (uint)eAttribute) == (uint)eAttribute);
		}
		
		/// <summary>Name of this folder (without the path)</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		
		}// Name property
		
		/// <summary>Fully qualified path for this folder</summary>
		public string Path
		{
			get { return m_strPath; }
			set { m_strPath = value; }
		}
		
		/// <summary>Suffix that gets added to Name when automatically assigning folder name to other primary values such as the MediaId</summary>
		public string Suffix
		{
			get { return m_strSuffix; }
			set { m_strSuffix = value; }
		}
		
		/// <summary>User specified data object</summary>
		public object UserData
		{
			get { return m_objUserData; }
			set { m_objUserData = value; }
		}
		
		/// <summary>XML primary node used to create the folder object when importing from a load file</summary>
		public CXmlPrimary XmlPrimary
		{
			get { return m_xmlPrimary; }
			set { m_xmlPrimary = value; }
		}
		
		/// <summary>Flag to indicate that the object was processed and registered</summary>
		public bool Registered
		{
			get { return m_bRegistered; }
			set { m_bRegistered = value; }
		}
		
		/// <summary>Collection of files belonging to this folder</summary>
		public CTmaxSourceFiles Files
		{
			get { return m_aFiles; }
		}
		
		/// <summary>Collection of subfolders belonging to this folder</summary>
		public CTmaxSourceFolders SubFolders
		{
			get { return m_aSubFolders; }
		}
		
		/// <summary>Primary data exchange object associated with this source</summary>
		public ITmaxRecord IPrimary
		{
			get { return m_IPrimary; }
			set { m_IPrimary = value; }
		}
		
		/// <summary>This is the enumerated TrialMax media type identifier</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes MediaType
		{
			get { return m_eMediaType; }
			set { m_eMediaType = value; }
		}

		/// <summary>The packed primary media attributes</summary>
		public long PrimaryAttributes
		{
			get { return m_lPrimaryAttributes; }
			set { m_lPrimaryAttributes = value; }
		}

	}// public class CTmaxSourceFolder

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxSourceFolder objects
	/// </summary>
	public class CTmaxSourceFolders : CollectionBase
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxSourceFolders()
		{
		}

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="tmaxFolder">CTmaxSourceFolder object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxSourceFolder Add(CTmaxSourceFolder tmaxFolder)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(tmaxFolder as object);

				return tmaxFolder;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxSourceFolder tmaxFolder)

		/// <summary>This method allows the caller to add a new column to the list</summary>
		/// <param name="tmaxFolder">CTmaxSourceFolder object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxSourceFolder Insert(CTmaxSourceFolder tmaxFolder, int iIndex)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Insert(iIndex, tmaxFolder as object);

				return tmaxFolder;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxSourceFolder tmaxFolder)

		/// <summary>
		/// This method is called to remove the requested filter from the collection
		/// </summary>
		/// <param name="tmaxFolder">The folder object to be removed</param>
		public void Remove(CTmaxSourceFolder tmaxFolder)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(tmaxFolder as object);
			}
			catch
			{
			}
		}

		/// <summary>
		/// This method is called to determine if the specified object exists in the collection
		/// </summary>
		/// <param name="tmaxFolder">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxSourceFolder tmaxFolder)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(tmaxFolder as object);
		}

		/// <summary>
		/// Called to locate the object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxSourceFolder Find(string strName)
		{
			// Search for the object with the same name
			foreach(CTmaxSourceFolder obj in base.List)
			{
				if(String.Compare(obj.Name, strName, true) == 0)
				{
					return obj;
				}
			}
			return null;

		}//	Find(string strName)

		/// <summary>Called to locate the object with the specified primary TrialMax media type identifier</summary>
		/// <param name="eMediaType">The desired TrialMax media type</param>
		/// <returns>The object with the specified identifier</returns>
		public CTmaxSourceFolder Find(TmaxMediaTypes eMediaType)
		{
			// Search for the object with the same primary media type
			foreach(CTmaxSourceFolder regFolder in base.List)
			{
				if(regFolder.MediaType == eMediaType)
				{
					return regFolder;
				}
			}
			return null;

		}//	Find(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)

		/// <summary>
		/// Overloaded version of [] operator to return the filter object at the desired index
		/// </summary>
		/// <returns>Filter object at the specified index</returns>
		public CTmaxSourceFolder this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (base.List[index] as CTmaxSourceFolder);
			}
		}

		/// <summary>
		/// Overloaded [] operator to locate the parameter object with the specified name
		/// </summary>
		/// <returns>The object with the specified name</returns>
		public CTmaxSourceFolder this[string strName]
		{
			get 
			{
				// Search for the object with the same name
				foreach(CTmaxSourceFolder obj in base.List)
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
		public int IndexOf(CTmaxSourceFolder value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CTmaxSourceFolders
	
}// namespace FTI.Shared.Trialmax
