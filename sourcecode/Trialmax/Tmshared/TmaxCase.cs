using System;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the information that defines a TrialMax case</summary>
	public class CTmaxCase
	{
		#region Private Members

		/// <summary>Private member bound to Name property</summary>
		private string m_strName = "";

		/// <summary>Private member bound to ShortName property</summary>
		private string m_strShortName = "";

		/// <summary>Private member bound to UniqueId property</summary>
		private string m_strUniqueId = "";

		/// <summary>Local member bound to VerMajor property</summary>
		private int m_iVerMajor = 0;

		/// <summary>Local member bound to VerMinor property</summary>
		private int m_iVerMinor = 0;

		/// <summary>Local member bound to VerBuild property</summary>
		private int m_iVerBuild = 0;

		/// <summary>Local member bound to VerQEF property</summary>
		private int m_iVerQEF = 0;

		/// <summary>Private member bound to Version property</summary>
		private string m_strVersion = "";

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxCase()
		{
		}

		/// <summary>Copy constructor</summary>
		/// <param name="tmaxCase">Source object to be copied</param>
		public CTmaxCase(CTmaxCase tmaxCase)
		{
			Debug.Assert(tmaxCase != null);

			if(tmaxCase != null)
			{
				Copy(tmaxCase);
			}

		}// public CTmaxCase(CTmaxCase tmaxCase)

		/// <summary>Called to copy the properties of the specified source object</summary>
		/// <param name="tmaxCase">The object to be copied</param>
		public void Copy(CTmaxCase tmaxCase)
		{
			this.Name = tmaxCase.Name;
			this.ShortName = tmaxCase.ShortName;
			this.UniqueId = tmaxCase.UniqueId;
			this.Version = tmaxCase.Version;
			this.VerMajor = tmaxCase.VerMajor;
			this.VerMinor = tmaxCase.VerMinor;
			this.VerQEF = tmaxCase.VerQEF;
			this.VerBuild = tmaxCase.VerBuild;

		}// public void Copy(CTmaxCase tmaxCase)

		/// <summary>Overridden base member to display the object as a string</summary>
		/// <returns>The case name if available</returns>
		public override string ToString()
		{
			if(this.Name.Length > 0)
				return this.Name;
			else
				return this.ShortName;

		}// public override string ToString()

		/// <summary>Called to set the version</summary>
		/// <param name="strVersion">The version descriptor</param>
		public void SetVersion(string strVersion)
		{
			m_strVersion = strVersion;
			
			if(m_strVersion.Length > 0)
				CBaseVersion.SplitVersion(m_strVersion, ref m_iVerMajor, ref m_iVerMinor, ref m_iVerQEF, ref m_iVerBuild);

		}// public void SetVersion(string strVersion)

		/// <summary>Called to get the version descriptor</summary>
		/// <returns>The case version descriptor</returns>
		public string GetVersion()
		{
			//	Should we construct the version descriptor?
			if((m_strVersion.Length == 0) && (m_iVerMajor > 0))
				m_strVersion = CBaseVersion.GetVersion(m_iVerMajor, m_iVerMinor, m_iVerQEF, m_iVerBuild, (m_iVerBuild <= 0));

			return m_strVersion;

		}// public string GetVersion()

		#endregion Public Methods

		#region Properties

		/// <summary>Name used to identify the case</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}

		/// <summary>Short name used to identify the case</summary>
		public string ShortName
		{
			get { return m_strShortName; }
			set { m_strShortName = value; }
		}
		
		/// <summary>Unique identifier assigned to the code</summary>
		public string UniqueId
		{
			get { return m_strUniqueId; }
			set { m_strUniqueId = value; }
		}

		/// <summary>The major version identifier</summary>
		public int VerMajor
		{
			get { return m_iVerMajor; }
			set { m_iVerMajor = value; }
		}

		/// <summary>The minor version identifier</summary>
		public int VerMinor
		{
			get { return m_iVerMinor; }
			set { m_iVerMinor = value; }
		}

		/// <summary>This build identifier</summary>
		/// <remarks>In TrialMax we use the fourth part (normally the QEF or Private) as the build identifier</remarks>
		public int VerBuild
		{
			get { return m_iVerBuild; }
			set { m_iVerBuild = value; }
		}

		/// <summary>The quick engineering fix (QEF) identifier</summary>
		/// <remarks>In TrialMax we use the third part (normally the Build) as the QEF identifier</remarks>
		public int VerQEF
		{
			get { return m_iVerQEF; }
			set { m_iVerQEF = value; }
		}

		/// <summary>The case version descriptor</summary>
		public string Version
		{
			get { return GetVersion(); }
			set { SetVersion(value); }
		}

		#endregion Properties

	}// public class CTmaxCase

}// namespace FTI.Shared.Trialmax
