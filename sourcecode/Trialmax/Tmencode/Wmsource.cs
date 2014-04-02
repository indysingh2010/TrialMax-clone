using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using WMEncoderLib;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Encode
{
	/// <summary>This class contains information that identifies an encoder source</summary>
	public class CWMSourceGroup
	{
		#region Private Members
		
		/// <summary>Local member bound to IWMGroup property</summary>
		private IWMEncSourceGroup2 m_IWMGroup = null;			
		
		/// <summary>Local member bound to IWMAudio property</summary>
		public IWMEncSource m_IWMAudio = null;
		
		/// <summary>Local member bound to IWMVideo property</summary>
		public IWMEncVideoSource2 m_IWMVideo = null;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		/// <summary>The unique name assigned to the group</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Start property</summary>
		private double m_dStart = 0;
		
		/// <summary>Local member bound to Stop property</summary>
		private double m_dStop = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CWMSourceGroup()
		{
		}
	
		/// <summary>Constructor</summary>
		/// <param name="strName">The unique name used to identify the group</param>
		/// <param name="strFileSpec">Fully qualified path to the video file</param>
		/// <param name="dStartTime">Time in seconds of start position</param>
		/// <param name="dStopTime">Time in seconds of stop position</param>
		public CWMSourceGroup(string strName, string strFileSpec, double dStartTime, double dStopTime)
		{
			SetProps(strName, strFileSpec, dStartTime, dStopTime);
		}
	
		/// <summary>Called to set the video properties</summary>
		/// <param name="strName">The unique name used to identify the group</param>
		/// <param name="strFileSpec">Fully qualified path to the video file</param>
		/// <param name="dStartTime">Time in seconds of start position</param>
		/// <param name="dStopTime">Time in seconds of stop position</param>
		public void SetProps(string strName, string strFileSpec, double dStartTime, double dStopTime)
		{
			m_strName = strName;
			m_strFileSpec = strFileSpec;
			m_dStart = dStartTime;
			m_dStop = dStopTime;
			
		}// public void SetProps(string strName, string strFileSpec, double dStartTime, double dStopTime)
		
		/// <summary>Called to initialize the COM interfaces required to manage the group</summary>
		/// <param name="IWMGroup">The COM interface that identifies the group</param>
		/// <returns>true if successful</returns>
		public bool Initialize(IWMEncSourceGroup2 IWMGroup)
		{
			int iStart = 0;
			int iStop = 0;
			
			try
			{
				//	Set the group interface
				m_IWMGroup = IWMGroup;
				
				// Create the audio and video encoder streams
				m_IWMAudio = m_IWMGroup.AddSource(WMENC_SOURCE_TYPE.WMENC_AUDIO);
				m_IWMVideo = (IWMEncVideoSource2)m_IWMGroup.AddSource(WMENC_SOURCE_TYPE.WMENC_VIDEO);
    
				//	Set the source file
				m_IWMAudio.SetInput(this.FileSpec, "", "");
				m_IWMVideo.SetInput(this.FileSpec, "", "");
				
				//	Set the start and stop times in milliseconds
				iStart = (int)(m_dStart * 1000.0);
				iStop = (int)(m_dStop * 1000.0);
				
				m_IWMAudio.MarkIn  = iStart;
				m_IWMAudio.MarkOut = iStop;
				
				m_IWMVideo.MarkIn  = iStart;
				m_IWMVideo.MarkOut = iStop;
				
				return true;				
			}
			catch
			{
				return false;
			}
			
		}// public bool Initialize(IWMEncSourceGroup2 IWMGroup)
		
		/// <summary>Called to set the next group in the encoder sequence</summary>
		/// <param name="nextGroup">The next group in the encoder sequence</param>
		/// <returns>true if successful</returns>
		public bool Cascade(CWMSourceGroup nextGroup)
		{
			if(IWMGroup == null) return false;
			if(nextGroup.IWMGroup == null) return false;
			
			try
			{
				IWMGroup.SetAutoRollover(-1, nextGroup.IWMGroup.Name);
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public void SetProps(string strFileSpec, double dStartTime, double dStopTime)
		
		/// <summary>Called to set the profile to be applied to the group</summary>
		/// <param name="wmProfile">The profile to be applied to the group</param>
		/// <returns>true if successful</returns>
		public bool SetProfile(CWMProfile wmProfile)
		{
			if(m_IWMGroup == null) return false;
			if(wmProfile.IProfile == null) return false;
			
			try
			{
				m_IWMGroup.set_Profile(wmProfile.IProfile);
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public void SetProps(string strFileSpec, double dStartTime, double dStopTime)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Interface to the WM Encoder stream group</summary>
		public IWMEncSourceGroup2 IWMGroup
		{
			get { return m_IWMGroup; }
		}
		
		/// <summary>Interface to the WM Encoder audio stream</summary>
		public IWMEncSource IWMAudio
		{
			get { return m_IWMAudio; }
		}
		
		/// <summary>Interface to the WM Encoder video stream</summary>
		public IWMEncVideoSource2 IWMVideo
		{
			get { return m_IWMVideo; }
		}
		
		/// <summary>The path to the source file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { m_strFileSpec = value; }
		}
		
		/// <summary>The unique group name</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>The playback start time</summary>
		public double Start
		{
			get { return m_dStart; }
			set { m_dStart = value; }
		}
		
		/// <summary>The playback stop time</summary>
		public double Stop
		{
			get { return m_dStop; }
			set { m_dStop = value; }
		}
		
		#endregion Properties
		
	}// public class CWMSourceGroup : ITmaxSortable

	/// <summary>This class manages a list of encoder profiles</summary>
	public class CWMSourceGroups : CTmaxSortedArrayList
	{
		#region Priviate Members
		
		/// <summary>The encoder that owns this collection</summary>
		public CWMEncoder m_WMEncoder = null;
		
		/// <summary>Local member bound to IEncoder property</summary>
		public WMEncoderLib.IWMEncSourceGroupCollection m_IWMSourceGroups = null;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CWMSourceGroups() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
		}

		/// <summary>Called to set the encoder and intialize the object</summary>
		public bool Initialize(CWMEncoder WMEncoder)
		{
			Debug.Assert(WMEncoder != null, "Invalid encoder");
			
			//	Clear the existing contents
			Clear();
			
			//	Save the new owner
			m_WMEncoder = WMEncoder;
			
			//	Get the groups interface
			if(m_WMEncoder != null)
			{
				m_IWMSourceGroups = WMEncoder.m_IWMEncoder.SourceGroupCollection;
			}
			else
			{
				m_IWMSourceGroups = null;
			}
			
			return (m_IWMSourceGroups != null);
			
		}// public bool Initialize(CWMEncoder WMEncoder)

		/// <summary>Called to add a new source group</summary>
		/// <param name="strName">The unique name used to identify the group</param>
		/// <param name="strFileSpec">The fully qualified path to the source file</param>
		/// <param name="dStart">The start time in seconds</param>
		/// <param name="dStop">The stop time in seconds</param>
		/// <returns>true if successful</returns>
		public CWMSourceGroup Add(string strName, string strFileSpec, double dStart, double dStop)
		{
			IWMEncSourceGroup2	IWMGroup = null;
			CWMSourceGroup		WMGroup = null;
			
			Debug.Assert(m_IWMSourceGroups != null, "NO SOURCE GROUPS COLLECTION");
			if(m_IWMSourceGroups == null) return null;
			
			try
			{
				//	We have to have a unique name
				if((strName == null) || (strName.Length == 0))
					strName = ("WMGROUP_" + (this.Count + 1).ToString());
					
				// Create an IWMEncSourceGroup object.
				IWMGroup = (IWMEncSourceGroup2)m_IWMSourceGroups.Add(strName);

				//	Create a new group object
				WMGroup = new CWMSourceGroup(strName, strFileSpec, dStart, dStop);
				
				//	Initialize the group object
				if(WMGroup.Initialize(IWMGroup) == false)
					return null;
				
				//	Add to the collection
				return Add(WMGroup);
			}
			catch
			{
				return null;
			}
			
		}// public CWMSourceGroup Add(string strFileSpec, double dStart, double dStop)
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="O">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSourceGroup Add(CWMSourceGroup O)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(O as object);
				return O;
			}
			catch
			{
				return null;
			}
			
		}// public CWMSourceGroup Add(CWMSourceGroup O)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="O">The object to be removed</param>
		public void Remove(CWMSourceGroup O)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(O as object);
			}
			catch
			{
			}
		
		}// public void Remove(CWMSourceGroup O)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="O">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CWMSourceGroup O)
		{
			// Use base class to process actual collection operation
			return base.Contains(O as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CWMSourceGroup this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CWMSourceGroup);
			}
		}

		/// <summary>Called to get the total time required to play the encoded file</summary>
		/// <returns>The total time in seconds</returns>
		public double GetTotalTime()
		{
			double dTotal = 0;
			
			foreach(CWMSourceGroup O in this)
				dTotal += O.Stop - O.Start;
				
			return dTotal;
			
		}// public double GetTotalTime()
			
		/// <summary>Called to get the group with the specified name</summary>
		/// <returns>The group with the specified name</returns>
		public CWMSourceGroup Find(string strName)
		{
			foreach(CWMSourceGroup O in this)
			{
				if(O.Name == strName)
					return O;
			}
			
			return null;
			
		}// public CWMSourceGroup Find(string strName)
			
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>COM interface to WM SourceGroup Collection object</summary>
		public WMEncoderLib.IWMEncSourceGroupCollection IWMSourceGroups
		{
			get { return m_IWMSourceGroups; }
		}
		
		#endregion Properties
		
	}//	public class CWMSourceGroups : CTmaxSortedArrayList
		
}// namespace FTI.Trialmax.Encode
