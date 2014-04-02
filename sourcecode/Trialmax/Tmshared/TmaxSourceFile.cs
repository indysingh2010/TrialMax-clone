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
		private ITmaxMediaRecord m_IPrimary = null;
		
		/// <summary>Local member bound to ISecondary property</summary>
		private ITmaxMediaRecord m_ISecondary = null;
		
		/// <summary>Local member bound to ITertiary property</summary>
		private ITmaxMediaRecord m_ITertiary = null;
		
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
			return CTmaxToolbox.Compare(this.Name, tmaxFile.Name, true);
		
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
		public ITmaxMediaRecord IPrimary
		{
			get { return m_IPrimary;	}
			set { m_IPrimary = value; }
		}
		
		/// <summary>Interface to secondary data exchange object associated with this source</summary>
		public ITmaxMediaRecord ISecondary
		{
			get { return m_ISecondary; }
			set { m_ISecondary = value; }
		}
		
		/// <summary>Interface to tertiary data exchange object associated with this source</summary>
		public ITmaxMediaRecord ITertiary
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
	
}// namespace FTI.Shared.Trialmax
