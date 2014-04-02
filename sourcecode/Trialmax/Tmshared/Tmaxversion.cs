using System;
using System.Reflection;
using System.Diagnostics;

namespace FTI.Shared
{
	/// <summary>
	///	This class serves as the base class from which all Trialmax assemblies
	///	derive their respective version reporting classes. 
	/// </summary>
	public class CBaseVersion
	{
		#region Protected Members
		
		/// <summary>Local member bound to Major property</summary>
		private int	m_iMajor = 0;
		
		/// <summary>Local member bound to Minor property</summary>
		private int m_iMinor = 0;
		
		/// <summary>Local member bound to Build property</summary>
		private int m_iBuild = 0;
		
		/// <summary>Local member bound to QEF property</summary>
		private int m_iQEF = 0;
		
		/// <summary>Local member bound to Location property</summary>
		private string m_strLocation = "";
		
		/// <summary>Local member bound to Description property</summary>
		private string m_strDescription = "";
		
		/// <summary>Local member bound to Title property</summary>
		private string m_strTitle = "";
		
		/// <summary>Local member bound to Version property</summary>
		private string m_strVersion = "";
		
		/// <summary>Local member bound to ShortVersion property</summary>
		private string m_strShortVersion = "";
		
		/// <summary>Local member bound to BuildDate property</summary>
		private string m_strBuildDate = "";
		
		#endregion Protected Methods
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		/// <remarks>Derived classes should call SetLocation() from their constructor</remarks>
		public CBaseVersion()
		{
		}
		
		/// <summary>This method converts the version string to a packed numeric value suitable for comparison</summary>
		/// <param name="strVersion">The text version identifier</param>
		/// <param name="strDelimiters">Characters used to parse the version string</param>
		/// <returns>The packed numeric equivalent</returns>
		public static long GetPackedVersion(string strVersion, string strDelimiters)
		{
			int iVerMajor = 0;
			int iVerMinor = 0;
			int iVerUpdate = 0;
			
			if(SplitVersion(strVersion, ref iVerMajor, ref iVerMinor, ref iVerUpdate, strDelimiters) == true)
				return GetPackedVersion(iVerMajor, iVerMinor, iVerUpdate);
			else
				return 0;

		}// public static long GetPackedVersion(string strVersion)

		/// <summary>This method converts the version string to a packed numeric value suitable for comparison</summary>
		/// <param name="strVersion">The text version identifier</param>
		/// <returns>The packed numeric equivalent</returns>
		public static long GetPackedVersion(string strVersion)
		{
			return GetPackedVersion(strVersion, "");
		}

		/// <summary>This method converts the numeric version identifiers into a single numeric value suitable for comparison of versions</summary>
		/// <param name="iVerMajor">Major version identifier</param>
		/// <param name="iVerMinor">Minor version identifier</param>
		/// <param name="iVerUpdate">Update version identifier</param>
		/// <returns>The packed numeric equivalent</returns>
		public static long GetPackedVersion(int iVerMajor, int iVerMinor, int iVerUpdate)
		{
			return ((iVerMajor * 1000) + (iVerMinor * 100) + iVerUpdate);
			
		}// public static long GetPackedVersion(int iVerMajor, int iVerMinor, int iVerUpdate)

		/// <summary>This method is called to extract the version string from the resource in the specified file</summary>
		/// <param name="strFileSpec">The fully qualified path to the executable file</param>
		/// <param name="bProduct">True to retrieve the product version value instead of the file version</param>
		/// <returns>The requested version string stored in the executable's resources</returns>
		public static string GetVersion(string strFileSpec, bool bProduct)
		{
			string strVersion = "";
			
			//	Does the file exist?
			if(System.IO.File.Exists(strFileSpec) == true)
			{
				//	Extract the version information from the executable file
				FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(strFileSpec);
				
				if(bProduct == true)
					strVersion = fvi.ProductVersion;
				else
					strVersion = fvi.FileVersion;
			
			}// if(System.IO.File.Exists(strFileSpec) == true)
			
			return strVersion;
							
		}// public static string GetVersion(string strFileSpec, bool bProduct)

		/// <summary>This method is called to extract the FileVersion string from the resource in the specified file</summary>
		/// <param name="strFileSpec">The fully qualified path to the executable file</param>
		/// <returns>The requested version string stored in the executable's resources</returns>
		public static string GetVersion(string strFileSpec)
		{
			return GetVersion(strFileSpec, false);
							
		}// public static string GetVersion(string strFileSpec)
		
		/// <summary>This method is called to get the string representation of the version identifiers</summary>
		/// <param name="iVerMajor">The major version identifier</param>
		/// <param name="iVerMinor">The minor version identifier</param>
		/// <param name="iVerQEF">The QEF (Update) version identifier</param>
		/// <param name="iVerBuild">The Build version identifier</param>
		/// <param name="bShortString">true to construct the short 3-part string</param>
		/// <returns>The string representation of the version identifiers</returns>
		public static string GetVersion(int iVerMajor, int iVerMinor, int iVerQEF, int iVerBuild, bool bShortString)
		{
			string strVersion = "";
			
			//	Build the short version
			strVersion = String.Format("{0}.{1}.{2}", iVerMajor, iVerMinor, iVerQEF);
			
			//	Should we add the build?
			if(bShortString == false)
				strVersion += ("." + iVerBuild.ToString());
				
			return strVersion;
							
		}// public static string GetVersion(int iVerMajor, int iVerMinor, int iVerQEF, int iVerBuild, bool bShortString)

		/// <summary>This method is called to get the string representation of the version identifiers</summary>
		/// <param name="iVerMajor">The major version identifier</param>
		/// <param name="iVerMinor">The minor version identifier</param>
		/// <param name="iVerQEF">The QEF (Update) version identifier</param>
		/// <param name="iVerBuild">The Build version identifier</param>
		/// <returns>The string representation of the version identifiers</returns>
		public static string GetVersion(int iVerMajor, int iVerMinor, int iVerQEF, int iVerBuild)
		{
			return GetVersion(iVerMajor, iVerMinor, iVerQEF, iVerBuild, false);
		}

		/// <summary>This method is called to get the string representation of the version identifiers</summary>
		/// <param name="iVerMajor">The major version identifier</param>
		/// <param name="iVerMinor">The minor version identifier</param>
		/// <param name="iVerQEF">The QEF (Update) version identifier</param>
		/// <returns>The string representation of the version identifiers</returns>
		public static string GetVersion(int iVerMajor, int iVerMinor, int iVerQEF)
		{
			return GetVersion(iVerMajor, iVerMinor, iVerQEF, 0, true);
		}

		/// <summary>This method splits the version string into its numeric version identifiers</summary>
		/// <param name="strVer">The version string to be parsed</param>
		/// <param name="iVerMajor">Major version identifier</param>
		/// <param name="iVerMinor">Minor version identifier</param>
		/// <param name="iVerUpdate">Update version identifier</param>
		/// <param name="iVerBuild">Build version identifier</param>
		/// <param name="strDelimiters">Delimiters used to parse the string</param>
		/// <returns>True if successful</returns>
		public static bool SplitVersion(string strVer, ref int iVerMajor, ref int iVerMinor, ref int iVerUpdate, ref int iVerBuild, string strDelimiters)
		{
			bool bSuccessful = true;
			
			//	Initialize the caller's values
			iVerMajor  = 0;
			iVerMinor  = 0;
			iVerUpdate = 0;
			iVerBuild  = 0;
			
			if(strVer == null) return false;
			if(strVer.Length == 0) return false;
			
			if((strDelimiters == null) || (strDelimiters.Length == 0))
				strDelimiters = ".";
				
			string[] aIdentifiers = strVer.Split(strDelimiters.ToCharArray());

			if(aIdentifiers.GetUpperBound(0) >= 0)
			{
				try { iVerMajor = System.Convert.ToInt32(aIdentifiers[0]); }
				catch { bSuccessful = false; }
			}
			if(aIdentifiers.GetUpperBound(0) >= 1)
			{
				try { iVerMinor = System.Convert.ToInt32(aIdentifiers[1]); }
				catch { bSuccessful = false; }
			}
			if(aIdentifiers.GetUpperBound(0) >= 2)
			{
				try { iVerUpdate = System.Convert.ToInt32(aIdentifiers[2]); }
				catch { bSuccessful = false; }
			}
			if(aIdentifiers.GetUpperBound(0) >= 3)
			{
				try { iVerBuild = System.Convert.ToInt32(aIdentifiers[3]); }
				catch { bSuccessful = false; }
			}
				
			return bSuccessful;
		
		}// public bool SplitVersion(string strVer, ref int iVerMajor, ref int iVerMinor, ref int iVerUpdate, ref int iVerBuild, string strDelimiters)

		/// <summary>This method splits the version string into its numeric version identifiers</summary>
		/// <param name="strVer">The version string to be parsed</param>
		/// <param name="iVerMajor">Major version identifier</param>
		/// <param name="iVerMinor">Minor version identifier</param>
		/// <param name="iVerUpdate">Update version identifier</param>
		/// <param name="strDelimiters">Delimiters used to parse the string</param>
		/// <returns>True if successful</returns>
		public static bool SplitVersion(string strVer, ref int iVerMajor, ref int iVerMinor, ref int iVerUpdate, string strDelimiters)
		{
			int iVerBuild = 0;

			return SplitVersion(strVer, ref iVerMajor, ref iVerMinor, ref iVerUpdate, ref iVerBuild, "");
		}

		/// <summary>This method splits the version string into its numeric version identifiers</summary>
		/// <param name="strVer">The version string to be parsed</param>
		/// <param name="iVerMajor">Major version identifier</param>
		/// <param name="iVerMinor">Minor version identifier</param>
		/// <param name="strDelimiters">Delimiters used to parse the string</param>
		/// <returns>True if successful</returns>
		public static bool SplitVersion(string strVer, ref int iVerMajor, ref int iVerMinor, string strDelimiters)
		{
			int iVerUpdate = 0;
			int iVerBuild = 0;
			
			return SplitVersion(strVer, ref iVerMajor, ref iVerMinor, ref iVerUpdate, ref iVerBuild, "");
		}

		/// <summary>This method splits the version string into its numeric version identifiers</summary>
		/// <param name="strVer">The version string to be parsed</param>
		/// <param name="iVerMajor">Major version identifier</param>
		/// <param name="iVerMinor">Minor version identifier</param>
		/// <param name="iVerUpdate">Update version identifier</param>
		/// <param name="iVerBuild">Build version identifier</param>
		/// <returns>True if successful</returns>
		public static bool SplitVersion(string strVer, ref int iVerMajor, ref int iVerMinor, ref int iVerUpdate, ref int iVerBuild)
		{
			return SplitVersion(strVer, ref iVerMajor, ref iVerMinor, ref iVerUpdate, ref iVerBuild, "");
		}
		
		/// <summary>This method splits the version string into its numeric version identifiers</summary>
		/// <param name="strVer">The version string to be parsed</param>
		/// <param name="iVerMajor">Major version identifier</param>
		/// <param name="iVerMinor">Minor version identifier</param>
		/// <param name="iVerUpdate">Update version identifier</param>
		/// <returns>True if successful</returns>
		public static bool SplitVersion(string strVer, ref int iVerMajor, ref int iVerMinor, ref int iVerUpdate)
		{
			return SplitVersion(strVer, ref iVerMajor, ref iVerMinor, ref iVerUpdate, "");
		}

		/// <summary>This method splits the version string into its numeric version identifiers</summary>
		/// <param name="strVer">The version string to be parsed</param>
		/// <param name="iVerMajor">Major version identifier</param>
		/// <param name="iVerMinor">Minor version identifier</param>
		/// <returns>True if successful</returns>
		public static bool SplitVersion(string strVer, ref int iVerMajor, ref int iVerMinor)
		{
			return SplitVersion(strVer, ref iVerMajor, ref iVerMinor, "");
		}

		/// <summary>This method gets the build identifier assoicated with the specified version string</summary>
		///	<param name="strVersion">The four-part version string containing the build identifier</param>
		/// <param name="strDelimiters">Delimiters used to parse the string</param>
		///	<returns>the build identifier contained in the string</returns>
		public static int GetBuild(string strVersion, string strDelimiters)
		{
			int	iMajor = 0;
			int	iMinor = 0;
			int	iUpdate = 0;
			int	iBuild = -1;
			
			try
			{
				//	Extract the version components
				if(!SplitVersion(strVersion, ref iMajor, ref iMinor, ref iUpdate, ref iBuild, strDelimiters))
					iBuild = -1;
			}
			catch
			{
				iBuild = -1;
			}
						
			return iBuild;
			
		}// public static int GetBuild(string strVersion, string strDelimiters)
		
		/// <summary>This method gets the build identifier assoicated with the specified version string</summary>
		///	<param name="strVersion">The four-part version string containing the build identifier</param>
		///	<returns>the build identifier contained in the string</returns>
		public static int GetBuild(string strVersion)
		{
			return GetBuild(strVersion, null);
			
		}// public static int GetBuild(string strVersion)
		
		/// <summary>This method converts the specified build identifier to its equivalent date</summary>
		///	<param name="iBuild">The build identifier to be converted</param>
		///	<param name="rdtBuild">The date object to be initialized from the build identifier</param>
		///	<returns>the equivalent date identifier</returns>
		///	<remarks>This method is no longer valid after 12-31-2009</remarks>
		public static bool GetBuildAsDate(int iBuild, ref System.DateTime rdtBuild)
		{
			int		iMonth = 0;
			int		iDay = 0;
			int		iYear = 0;
			bool	bSuccessful = false;
			
			//	Do we have a valid build identifier?
			//
			//	NOTE:	Minimum valid build date is 01-01-2000
			if(iBuild < 01010) return false;
			
			try
			{
				//	The build identifier is formatted as a packed integer MMDDY
				iYear = iBuild % 10;
				iDay = ((iBuild - iYear) % 1000) / 10;
				iMonth = ((iBuild - (iDay * 10) - iYear) / 1000);
			
				System.DateTime dtNow = System.DateTime.Now;
				iYear += 2000;
				while ((iYear + 10) <= dtNow.Year)
					iYear += 10;
				
				System.DateTime dtBuild = new System.DateTime(iYear, iMonth, iDay);
				rdtBuild = dtBuild;
				
				bSuccessful = true;
			}
			catch
			{
			}
			
			return bSuccessful;
			
		}// public static bool GetBuildAsDate(int iBuild, ref System.DateTime rdtBuild)
		
		/// <summary>This method converts the build identifier in the specified version string to its equivalent date</summary>
		///	<param name="strVersion">The four-part version string containing the build identifier</param>
		///	<param name="rdtBuild">The date object to be initialized from the build identifier</param>
		/// <param name="strDelimiters">Delimiters used to parse the string</param>
		///	<returns>true if successful</returns>
		public static bool GetBuildAsDate(string strVersion, ref System.DateTime rdtBuild, string strDelimiters)
		{
			int		iBuild = 0;
			bool	bSuccessful = false;
			
			if((iBuild = GetBuild(strVersion, strDelimiters)) > 0)
			{
				bSuccessful = GetBuildAsDate(iBuild, ref rdtBuild);
			}
			
			return bSuccessful;
			
		}// public static bool GetBuildAsDate(string strVersion, ref System.DateTime rdtBuild, string strDelimiters)
		
		/// <summary>This method converts the build identifier in the specified version string to its equivalent date string</summary>
		///	<param name="iBuild">The numerical build identifier</param>
		///	<returns>the date string that represents the build identifier</returns>
		public static string GetBuildAsString(int iBuild)
		{
			string			strBuild = "";
			System.DateTime	dtBuild = System.DateTime.Now;
			
			if(GetBuildAsDate(iBuild, ref dtBuild) == true)
			{
				strBuild = dtBuild.ToShortDateString();
			}
			
			return strBuild;
			
		}// public static string GetBuildAsString(int iBuild)
		
		/// <summary>This method converts the build identifier in the specified version string to its equivalent date string</summary>
		///	<param name="strVersion">The four-part version string containing the build identifier</param>
		/// <param name="strDelimiters">Delimiters used to parse the string</param>
		///	<returns>true if successful</returns>
		public static string GetBuildAsString(string strVersion, string strDelimiters)
		{
			int		iBuild = 0;
			string	strBuild = "";
			
			if((iBuild = GetBuild(strVersion, strDelimiters)) > 0)
			{
				strBuild = GetBuildAsString(iBuild);
			}
			
			return strBuild;
			
		}// public static string GetBuildAsString(string strVersion, string strDelimiters)
		
		/// <summary>This method converts the build identifier in the specified version string to its equivalent date string</summary>
		///	<param name="strVersion">The four-part version string containing the build identifier</param>
		///	<returns>true if successful</returns>
		public static string GetBuildAsString(string strVersion)
		{
			int		iBuild = 0;
			string	strBuild = "";
			
			if((iBuild = GetBuild(strVersion)) > 0)
			{
				strBuild = GetBuildAsString(iBuild);
			}
			
			return strBuild;
			
		}// public static string GetBuildAsString(string strVersion)
		
		/// <summary>This method extracts the version information contained in the specified file</summary>
		/// <param name="strFileSpec">The location (fully qualified path) to the file</param>
		///	<param name="bReverseBuild">True to reverse the QEF and Build values as used by TrialMax assemblies</param>
		/// <returns>true if successful</returns>
		public virtual bool InitFromFile(string strFileSpec, bool bReverseBuild)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Does the file exist?
				if(System.IO.File.Exists(strFileSpec) == false)
					return false;
					
				//	Save the location
				m_strLocation = strFileSpec;
				
				//	Get the version information
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(m_strLocation);
				
				//	Get the version information
				m_iMajor = fvi.FileMajorPart;
				m_iMinor = fvi.FileMinorPart;
				
				//	TrialMax assemblies use the 3rd value for QEF and 4th for build
				if(bReverseBuild == true)
				{
					m_iBuild = fvi.FilePrivatePart;
					m_iQEF   = fvi.FileBuildPart;
				}
				else
				{
					m_iBuild = fvi.FileBuildPart;
					m_iQEF   = fvi.FilePrivatePart;
				}
				
				m_strDescription = fvi.Comments;
				m_strTitle = fvi.FileDescription;
				
				//	Set the text descriptors
				SetVersionText(false);
				
				bSuccessful = true;
				
			}
			catch
			{
			}
			
			return bSuccessful;
			
		}// public virtual bool InitFromFile(string strFileSpec)
		
		/// <summary>This method sets the location of a TrialMax assembly</summary>
		/// <param name="strLocation">The location (fully qualified path) to the assembly</param>
		virtual public void SetTmaxLocation(string strLocation)
		{
			try
			{
				InitFromFile(strLocation, true);
				
				//	The TrialMax Build Id is actually a date
				if(m_iBuild > 0)
					m_strBuildDate = GetBuildAsString(m_iBuild);
			}
			catch
			{
			}
			
		}// virtual public void SetTmaxLocation(string strLocation)
		
		/// <summary>This method updates the text descriptors using the current identifiers</summary>
		///	<param name="bBuildAsDate">true to treat the build identifier as a date</param>
		virtual public void SetVersionText(bool bBuildAsDate)
		{
			m_strShortVersion = GetVersion(m_iMajor, m_iMinor, m_iQEF);
			m_strVersion = GetVersion(m_iMajor, m_iMinor, m_iQEF, m_iBuild, false);
		
			//	Is the build identifier is actually a date
			if((bBuildAsDate == true) && (m_iBuild > 0))
				m_strBuildDate = GetBuildAsString(m_iBuild);
			else
				m_strBuildDate = "";
				
		}// public virtual void SetVersionText(bool bBuildAsDate)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The major version identifier</summary>
		public int Major
		{
			get { return m_iMajor;  }
			set { m_iMajor = value; }
		}
		
		/// <summary>The minor version identifier</summary>
		public int Minor
		{
			get { return m_iMinor;  }
			set { m_iMinor = value; }
		}
		
		/// <summary>This build identifier</summary>
		/// <remarks>In TrialMax we use the fourth part (normally the QEF or Private) as the build identifier</remarks>
		public int Build
		{
			get { return m_iBuild; }
			set { m_iBuild = value; }
		}
		
		/// <summary>The quick engineering fix (QEF) identifier</summary>
		/// <remarks>In TrialMax we use the third part (normally the Build) as the QEF identifier</remarks>
		public int QEF
		{
			get { return m_iQEF;  }
			set { m_iQEF = value; }
		}
		
		/// <summary>This fully qualified path to the file containing this version information</summary>
		public string Location
		{
			get { return m_strLocation;  }
			set { m_strLocation = value; }
		}
		
		/// <summary>This file description</summary>
		public string Description
		{
			get { return m_strDescription;  }
			set { m_strDescription = value; }
		}
		
		/// <summary>This Title for the file</summary>
		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value; }
		}
		
		/// <summary>The text representation of the complete 4-part version descriptor</summary>
		public string Version
		{
			get { return m_strVersion; }
			set { m_strVersion = value; }
		}
		
		/// <summary>The text representation of the 3-part Major.Minor.QEF descriptor</summary>
		public string ShortVersion
		{
			get { return m_strShortVersion; }
			set { m_strShortVersion = value; }
		}
		
		/// <summary>The date that corresponds with the Build value</summary>
		public string BuildDate
		{
			get { return m_strBuildDate; }
			set { m_strBuildDate = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxVersion
	
}// namespace FTI.Shared.Trialmax
