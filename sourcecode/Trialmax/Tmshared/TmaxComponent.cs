using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information that identifies a component of the TrialMax product</summary>
	public class CTmaxComponent : ITmaxSortable
	{
		#region Private Members
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Folder property</summary>
		private string m_strFolder = "";
		
		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";
		
		/// <summary>Local member bound to Description property</summary>
		private string m_strDescription = "";
		
		/// <summary>Local member bound to Version property</summary>
		private string m_strVersion = "";
		
		/// <summary>Local member bound to InstallDirectory property</summary>
		private string m_strInstallDirectory = "";

		/// <summary>Local member bound to ActivationCode property</summary>
		private string m_strActivationCode = "";

        /// <summary>Local member bound to ActivationDate property</summary>
        private string m_dateActivation = "";

		/// <summary>This member is bound to the VerMajor property</summary>
		private int m_iVerMajor = -1;
		
		/// <summary>This member is bound to the VerMinor property</summary>
		private int m_iVerMinor = -1;
		
		/// <summary>This member is bound to the VerUpdate property</summary>
		private int m_iVerUpdate = -1;
		
		/// <summary>This member is bound to the VerBuild property</summary>
		private int m_iVerBuild = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxComponent()
		{
		}
	
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxComponent">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxComponent, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxComponent tmaxComponent, long lMode)
		{
			//	Are these the same objects?
			if(ReferenceEquals(this, tmaxComponent) == true)
			{
				return 0;
			}
			
			//	Are we comparing different versions of the same component?
			else if(String.Compare(this.Name, tmaxComponent.Name, true) == 0)
			{
				long lVerPacked = CBaseVersion.GetPackedVersion(this.Version);
				long lVerComponent = CBaseVersion.GetPackedVersion(tmaxComponent.Version);
				
				//	Do the Major.Minor.Update values match?
				if(lVerPacked == lVerComponent)
				{
					//	Do we have valid build identifiers?
					if((this.VerBuild > 0) && (tmaxComponent.VerBuild > 0))
					{
						//	Are the build identifiers equal?
						if(this.VerBuild == tmaxComponent.VerBuild)
							return 0;
						else if(this.VerBuild < tmaxComponent.VerBuild)
							return -1;
						else
							return 1;
					}
					else
					{
						return 0;
					}
					
				}
				else if(lVerPacked < lVerComponent)
				{
					return -1;
				}
				else
				{
					return 1;	
				}
							
			}
			
			else
			{
				return String.Compare(this.Name, tmaxComponent.Name, true);
			}
					
		}// public int Compare(CTmaxComponent tmaxComponent, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxComponent, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxComponent)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method builds the text representation of the message</summary>
		/// <returns>The default text representation</returns>
		public override string ToString()
		{
			return GetDescription();
		
		}// public override string ToString()

		/// <summary>Called to get the description of this component</summary>
		/// <returns>The description of this component</returns>
		public string GetDescription()
		{
			//	Do we have a valid description?
			if(m_strDescription.Length > 0)
			{
				return m_strDescription;
			}
			else
			{
				//	Use a default description
				if(String.Compare(m_strName, TmaxComponents.TrialMax.ToString(), true) == 0)
				{
					return "TrialMax Presentation System";
				}
				else if(String.Compare(m_strName, TmaxComponents.FTIP2I.ToString(), true) == 0)
				{
					return "TrialMax PDF Converter";
				}
				else if(String.Compare(m_strName, TmaxComponents.FTIORE.ToString(), true) == 0)
				{
					return "TrialMax Objections Report Engine";
				}
				else if(String.Compare(m_strName, TmaxComponents.VideoViewer.ToString(), true) == 0)
				{
					return "TrialMax Video Viewer";
				}
				else if(String.Compare(m_strName, TmaxComponents.WMEncoder.ToString(), true) == 0)
				{
					return "Windows Media Encoder";
				}
				else
				{
					return m_strName;
				}
			
			}// if(m_strDescription.Length > 0)
			
		}// public string GetDescription()
		
		/// <summary>Called to get the name of the file associated with this component</summary>
		/// <returns>The name of this component's executable file</returns>
		public string GetFilename()
		{
			//	Do we have a valid description?
			if(m_strFilename.Length > 0)
			{
				return m_strFilename;
			}
			else
			{
				//	Use a default description
				if(String.Compare(m_strName, TmaxComponents.TrialMax.ToString(), true) == 0)
				{
					return "tmaxManager.exe";
				}
				else if(String.Compare(m_strName, TmaxComponents.VideoViewer.ToString(), true) == 0)
				{
					return "tmaxVideo.exe";
				}
				else if(String.Compare(m_strName, TmaxComponents.FTIP2I.ToString(), true) == 0)
				{
					return "ftip2i.exe";
				}
				else if(String.Compare(m_strName, TmaxComponents.FTIORE.ToString(), true) == 0)
				{
					return "ftiore.exe";
				}
				else if(String.Compare(m_strName, TmaxComponents.WMEncoder.ToString(), true) == 0)
				{
					return "wmencloc.dll";
				}
				else
				{
					return "";
				}
			
			}// if(m_strFilename.Length > 0)
			
		}// public string GetFilename()
		
		/// <summary>Called to get the fully qualified path to this component's executable file</summary>
		/// <returns>The fully qualified path of this component's executable file</returns>
		public string GetFileSpec()
		{
			string strFileSpec = "";
			string strFilename = "";
			
			//	Must have a filename
			strFilename = GetFilename();
			if(strFilename.Length > 0)
			{
				strFileSpec = this.Folder;
				if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
					strFileSpec += "\\";
				strFileSpec += strFilename;
			}
			
			return strFileSpec;
			
		}// public string GetFileSpec()
		
		/// <summary>Called to get a CBaseVersion object that represents this component</summary>
		///	<param name="bBuildAsDate">true to treat the build identifier as a date</param>
		/// <returns>An equivalent base version object</returns>
		public CBaseVersion GetBaseVersion(bool bBuildAsDate)
		{
			CBaseVersion baseVersion = new CBaseVersion();
			
			if(GetFilename().Length > 0)
				baseVersion.Location = this.GetFileSpec();
			else
				baseVersion.Location = this.Folder;
			
			baseVersion.Major = this.VerMajor;
			baseVersion.Minor = this.VerMinor;
			baseVersion.QEF = this.VerUpdate;
			baseVersion.Build = this.VerBuild;
			baseVersion.SetVersionText(bBuildAsDate);
					
			baseVersion.Description = this.GetDescription();
			baseVersion.Title = this.Name;
			
			return baseVersion;
				
		}// public CBaseVersion GetBaseVersion()
		
		/// <summary>Called to get a CBaseVersion object that represents this component</summary>
		/// <returns>An equivalent base version object</returns>
		public CBaseVersion GetBaseVersion()
		{
			return GetBaseVersion(true);
		}

		/// <summary>Called to show the component values in a standard windows message box</summary>
		public void Show()
		{
			string strMsg = "";

			strMsg += "Version: ";
			strMsg += this.Version;
			strMsg += "\n";

			strMsg += "Description: ";
			strMsg += this.Description;
			strMsg += "\n";

			strMsg += "FileSpec: ";
			strMsg += this.FileSpec;
			strMsg += "\n";

			strMsg += "Folder: ";
			strMsg += this.Folder;
			strMsg += "\n";

			strMsg += "Filename: ";
			strMsg += this.Filename;
			strMsg += "\n";

			strMsg += "InstallDir: ";
			strMsg += this.InstallDirectory;
			strMsg += "\n";

			strMsg += "ActivationCode: ";
			strMsg += this.ActivationCode;
			strMsg += "\n";
			
			MessageBox.Show(strMsg, this.Name);
		}

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The unique name that identifies the component</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>The folder where the component is installed</summary>
		public string Folder
		{
			get { return m_strFolder; }
			set { m_strFolder = value; }
		}
		
		/// <summary>The file that can be used to get the version information</summary>
		public string Filename
		{
			get { return GetFilename(); }
			set { m_strFilename = value; }
		}
		
		/// <summary>The fully qualified path to the executable program associated with this component</summary>
		public string FileSpec
		{
			get { return GetFileSpec(); }
		}
		
		/// <summary>The description of the component</summary>
		public string Description
		{
			get { return GetDescription(); }
			set { m_strDescription = value; }
		}

		/// <summary>The activation code associated with this component</summary>
		public string ActivationCode
		{
			get { return m_strActivationCode; }
			set { m_strActivationCode = value; }
		}

        /// <summary>The activation Date associated with this component</summary>
        public string ActivationDate
        {
            get { return m_dateActivation; }
            set { m_dateActivation = value; }
        }

		/// <summary>The directory where the component is installed</summary>
		public string InstallDirectory
		{
			get { return m_strInstallDirectory; }
			set { m_strInstallDirectory = value; }
		}
		
		/// <summary>The text (1.1.1.1) version identifier</summary>
		public string Version
		{
			get{ return m_strVersion; }
			set
			{ 
				if(value != m_strVersion)
				{
					//	Store the new version string
					m_strVersion = value;
					
					//	Split the string into it's numerical components
					CBaseVersion.SplitVersion(m_strVersion, ref m_iVerMajor, ref m_iVerMinor, ref m_iVerUpdate, ref m_iVerBuild);
				}
				
			}
		
		}
		
		//	Major version identifier
		public int VerMajor
		{
			get{ return m_iVerMajor; }
		}
		
		//	Minor version identifier
		public int VerMinor
		{
			get{ return m_iVerMinor; }
		}
		
		//	Update version identifier
		public int VerUpdate
		{
			get{ return m_iVerUpdate; }
		}
		
		//	Build version identifier
		public int VerBuild
		{
			get{ return m_iVerBuild; }
		}
		
		#endregion Properties
		
	}// public class CTmaxComponent : ITmaxSortable

	/// <summary>This class manages a list of system messages</summary>
	public class CTmaxComponents : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxComponents() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
			
			this.EventSource.Name = "TrialMax Components Collection";
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxComponent">CTmaxComponent object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxComponent Add(CTmaxComponent tmaxComponent)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxComponent as object);

				return tmaxComponent;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxComponent Add(CTmaxComponent tmaxComponent)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxComponent">The filter object to be removed</param>
		public void Remove(CTmaxComponent tmaxComponent)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxComponent as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxComponent tmaxComponent)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxComponent">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxComponent tmaxComponent)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxComponent as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxComponent this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxComponent);
			}
		}

		/// <summary>Called to find the component with the specified name</summary>
		/// <param name="strName">The name of the component to be located</param>
		/// <returns>The component with the specified name if found</returns>
		public CTmaxComponent Find(string strName)
		{
			foreach(CTmaxComponent O in this)
			{
				if(String.Compare(O.Name, strName, true) == 0)
					return O;
			}
			
			//	Must not be in the list
			return null;
			
		}// public CTmaxComponent Find(string strName)

		/// <summary>Called to find the component with the specified key</summary>
		/// <param name="eKey">The component's key identifier</param>
		/// <returns>The component with the specified key</returns>
		public CTmaxComponent Find(FTI.Shared.Trialmax.TmaxComponents eKey)
		{
			//	The key should be the Name
			return Find(eKey.ToString());
			
		}// public CTmaxComponent Find(FTI.Shared.Trialmax.TmaxComponents eKey)

		#endregion Public Methods
		
	}//	public class CTmaxComponents : CTmaxSortedArrayList
		
}// namespace FTI.Shared.Trialmax
