using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using WMEncoderLib;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Encode
{
	/// <summary>This class contains information associated with a system message</summary>
	public class CWMProfile : ITmaxSortable
	{
		#region Private Members
		
		/// <summary>Local member bound to DeviceIndex property</summary>
		private int m_iDeviceIndex = -1;
		
		/// <summary>Local member bound to Preferred property</summary>
		private bool m_bPreferred = true;
		
		/// <summary>Local member bound to IWMProfile property</summary>
		private IWMEncProfile m_IWMProfile = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CWMProfile()
		{
		}
	
		/// <summary>Constructor</summary>
		/// <param name="IProfile">The interface for the profile</param>
		/// <param name="iDeviceIndex">The device index for the profile</param>
		public CWMProfile(IWMEncProfile IProfile, int iDeviceIndex)
		{
			Initialize(IProfile, iDeviceIndex);
		}
	
		/// <summary>Called to initialize the object properties</summary>
		/// <param name="IProfile">The interface for the profile</param>
		/// <param name="iDeviceIndex">The device index for the profile</param>
		public void Initialize(IWMEncProfile IProfile, int iDeviceIndex)
		{
			m_IWMProfile = IProfile;
			m_iDeviceIndex = iDeviceIndex;
			
		}// public void Initialize(IWMEncProfile IProfile, int iDeviceIndex)
	
		/// <summary>This method is called to get the extended I2 interface</summary>
		/// <returns>The extended profile interface</returns>
		public WMEncProfile2 GetI2()
		{
			try
			{
				WMEncProfile2 wmp2 = new WMEncProfile2();
				wmp2.LoadFromIWMProfile(IProfile);
				return wmp2;
			}
			catch
			{
				return null;
			}
			
		}// public WMEncProfile2 GetI2()
		
		/// <summary>Called to get the name of this profile</summary>
		/// <returns>The name of the profile</returns>
		public string GetName()
		{
			string strName = "Unknown";
			
			if((m_IWMProfile != null) && (m_IWMProfile.Name != null))
			{
				strName = m_IWMProfile.Name;
			}
				
			return strName;
			
		}// public string GetName()
	
		/// <summary>Called to determine if the profile is capable of video encoding</summary>
		/// <returns>True if capable</returns>
		public bool GetVideoCapable()
		{
			bool bCapable = false;
			
			try
			{
				//	Do we have the COM interface
				if(m_IWMProfile != null)
				{
					if(m_IWMProfile.get_MediaCount(WMENC_SOURCE_TYPE.WMENC_VIDEO) > 0)
						bCapable = true;
				}
			
			}
			catch
			{
			}	
			
			return bCapable;
			
		}// public bool GetVideoCapable()
	
		/// <summary>Called to determine if the profile is capable of audio encoding</summary>
		/// <returns>True if capable</returns>
		public bool GetAudioCapable()
		{
			bool bCapable = false;
			
			try
			{
				//	Do we have the COM interface
				if(m_IWMProfile != null)
				{
					if(m_IWMProfile.get_MediaCount(WMENC_SOURCE_TYPE.WMENC_AUDIO) > 0)
						bCapable = true;
				}
			
			}
			catch
			{
			}	
			
			return bCapable;
			
		}// public bool GetAudioCapable()
	
		/// <summary>Called to determine if the profile is capable of video encoding</summary>
		/// <returns>True if capable</returns>
		public string GetVideoCodec()
		{
			WMEncProfile2		IProfile2 = null;
			IWMEncAudienceObj	IAudience = null;
			int					iIndex = 0;
			int					iCodec4CC = 0;
			object				oCodecName = null;
			string				strCodec = "";
			
			try
			{
				if(m_IWMProfile == null) return "";
					
				//	Do we have the COM interface
				if((IProfile2 = GetI2()) != null)
				{
					// Get the first audience in this profile
					if((IAudience = IProfile2.get_Audience(0)) != null)
					{
						//	Get the index of the codec being used by this audience
						if((iIndex = IAudience.get_VideoCodec(0)) >= 0)
						{
							iCodec4CC = IProfile2.EnumVideoCodec(iIndex, out oCodecName);
							
							if(oCodecName != null)
								strCodec = oCodecName.ToString();
						}
							
					}// if((IAudience = IProfile2.get_Audience(0)) != null)

				}
			
			}
			catch
			{
			}	
			
			return strCodec;
			
		}// public string GetVideoCodec()
	
		/// <summary>Called to determine if the profile is capable of video encoding</summary>
		/// <returns>True if capable</returns>
		public string GetAudioCodec()
		{
			WMEncProfile2		IProfile2 = null;
			IWMEncAudienceObj	IAudience = null;
			int					iIndex = 0;
			int					iCodec4CC = 0;
			object				oCodecName = null;
			string				strCodec = "";
			
			try
			{
				if(m_IWMProfile == null) return "";
					
				//	Do we have the COM interface
				if((IProfile2 = GetI2()) != null)
				{
					// Get the first audience in this profile
					if((IAudience = IProfile2.get_Audience(0)) != null)
					{
						//	Get the index of the codec being used by this audience
						if((iIndex = IAudience.get_AudioCodec(0)) >= 0)
						{
							iCodec4CC = IProfile2.EnumAudioCodec(iIndex, out oCodecName);
							
							if(oCodecName != null)
								strCodec = oCodecName.ToString();
						}
							
					}// if((IAudience = IProfile2.get_Audience(0)) != null)

				}
			
			}
			catch
			{
			}	
			
			return strCodec;
			
		}// public string GetAudioCodec()
	
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than O, 0 if equal, 1 if greater than</returns>
		public int Compare(CWMProfile O, long lMode)
		{
			if(ReferenceEquals(O, this) == true)
				return 0;
			else
				return String.Compare(this.Name, O.Name, true);
					
		}// public int Compare(CWMProfile O, long lMode)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than O, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CWMProfile)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		/// <summary>This method builds the text representation of the message</summary>
		/// <returns>The default text representation</returns>
		public override string ToString()
		{
			if(this.Name.Length > 0)
				return this.Name;
			else
				return base.ToString();

		}// public override string ToString()

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The interface to the profile</summary>
		public IWMEncProfile IProfile
		{
			get { return m_IWMProfile; }
			set { m_IWMProfile = value; }
		}
		
		/// <summary>The name of this encoder profile</summary>
		public string Name
		{
			get { return GetName(); }
		}
		
		/// <summary>Is the profile capable of encoding video</summary>
		public bool VideoCapable
		{
			get { return GetVideoCapable(); }
		}
		
		/// <summary>Is the profile capable of encoding audio</summary>
		public bool AudioCapable
		{
			get { return GetAudioCapable(); }
		}
		
		/// <summary>The profile device index when enumerated</summary>
		public int DeviceIndex
		{
			get { return m_iDeviceIndex; }
			set { m_iDeviceIndex = value; }
		}
		
		/// <summary>This profile is recommended for use with TrialMax</summary>
		public bool Preferred
		{
			get { return m_bPreferred; }
			set { m_bPreferred = value; }
		}
		
		#endregion Properties
		
	}// public class CWMProfile : ITmaxSortable

	/// <summary>This class manages a list of encoder profiles</summary>
	public class CWMProfiles : CTmaxSortedArrayList
	{
		#region Priviate Members
		
		/// <summary>Local member bound to IEncoder property</summary>
		public WMEncoderLib.IWMEncProfileCollection m_IWMProfiles = null;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CWMProfiles() : base()
		{
			//	Assign a default sorter
			base.Comparer = new CTmaxSorter();
			this.KeepSorted = false;
		}

		/// <summary>This method is called to fill the collection</summary>
		/// <param name="IWMProfiles">COM interface to the profiles collection</param>
		/// <param name="bFilter">True to filter out invalid profiles</param>
		/// <returns>The number of profiles</returns>
		public int Fill(IWMEncProfileCollection IWMProfiles, bool bFilter)
		{
			CWMProfile wmProfile = null;
			
			//	Clear the existing values
			this.Clear();
			
			//	Save the new interface
			if((m_IWMProfiles = IWMProfiles) == null)
				return 0;
			
			//	Retrieve each of the profiles from the collection
			for(int i = 0; i < m_IWMProfiles.Count; i++)
			{
				wmProfile = new CWMProfile(m_IWMProfiles.Item(i), i);
				if(bFilter == false)
				{
					this.Add(wmProfile);
				}
				else
				{
					if((wmProfile.VideoCapable == true) && (wmProfile.AudioCapable == true))
					{
						this.Add(wmProfile);
					}
					else
					{
						wmProfile = null;
					}
				
				}
				
			}// for(int i = 0; i < m_IWMProfiles.Count; i++)
			
			return this.Count;
		
		}// public int Fill(IWMEncProfileCollection IWMProfiles)
		
		/// <summary>This method is called to find the specified profile</summary>
		/// <param name="strName">The name of the profile</param>
		/// <param name="iDeviceIndex">The profile's device index</param>
		/// <returns>The specified profile if found</returns>
		public CWMProfile Find(string strName, int iDeviceIndex)
		{
			foreach(CWMProfile O in this)
			{
				//	Do we need to match the name?
				if((strName != null) && (strName.Length > 0))
				{
					if(strName == O.Name)
					{
						//	Do we need to match the device index
						if(iDeviceIndex >= 0)
						{
							if(O.DeviceIndex == iDeviceIndex)
								return O;
						}
						else
						{
							return O; 
						}
					
					}
					
				}
				else
				{
					//	Do we need to match the device index
					if(iDeviceIndex >= 0)
					{
						if(O.DeviceIndex == iDeviceIndex)
							return O;
					}
					else
					{
						return O; // this doesn't really make sense but...
					}
					
				}
				
			}// foreach(CWMProfile O in this)
			
			return null;	
		
		}// public CWMProfile Find(string strName, int iDeviceIndex)
		
		/// <summary>This method is called to find the specified profile</summary>
		/// <param name="strName">The name of the profile</param>
		/// <returns>The specified profile if found</returns>
		public CWMProfile Find(string strName)
		{
			return Find(strName, -1);	
		}
		
		/// <summary>This method is called to find the specified profile</summary>
		/// <param name="iDeviceIndex">The profile's device index</param>
		/// <returns>The specified profile if found</returns>
		public CWMProfile Find(int iDeviceIndex)
		{
			return Find("", iDeviceIndex);	
		}
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="O">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMProfile Add(CWMProfile O)
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
			
		}// public CWMProfile Add(CWMProfile O)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="O">The object to be removed</param>
		public void Remove(CWMProfile O)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(O as object);
			}
			catch
			{
			}
		
		}// public void Remove(CWMProfile O)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="O">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CWMProfile O)
		{
			// Use base class to process actual collection operation
			return base.Contains(O as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CWMProfile this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CWMProfile);
			}
		}

		/// <summary>This method is called to use the specified collection to set the preferred profiles</summary>
		/// <param name="aPreferred">The names of all preferred profiles</param>
		public void SetPreferred(ArrayList aPreferred)
		{
			CWMProfile wmPreferred = null;
			
			//	Start by clearing all preferred profiles
			foreach(CWMProfile O in this)
				O.Preferred = false;
				
			//	Now set the flag for those in the caller's collection
			if((aPreferred != null) && (aPreferred.Count > 0))
			{
				foreach(string O in aPreferred)
				{
					if((wmPreferred = this.Find(O, -1)) != null)
						wmPreferred.Preferred = true;
				}
			}
		
		}// public void SetPreferred(ArrayList aPreferred)
		
		/// <summary>This method is called to get the names of all preferred profiles</summary>
		/// <param name="aPreferred">The names of all preferred profiles</param>
		///	<returns>the number of preferred profiles</returns>
		public int GetPreferred(ArrayList aPreferred)
		{
			//	Start by clearing the caller's collection
			if(aPreferred == null) return 0;
			else aPreferred.Clear();
			
			//	Now add the name of each preferred profile
			foreach(CWMProfile O in this)
			{
				if(O.Preferred == true)
					aPreferred.Add(O.Name);
			}
			
			return aPreferred.Count;
				
		}// public int GetPreferred(ArrayList aPreferred
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>COM interface to WM Profile Collection object</summary>
		public WMEncoderLib.IWMEncProfileCollection IWMProfiles
		{
			get { return m_IWMProfiles; }
		}
		
		#endregion Properties
		
	}//	public class CWMProfiles : CTmaxSortedArrayList
		
}// namespace FTI.Trialmax.Encode
