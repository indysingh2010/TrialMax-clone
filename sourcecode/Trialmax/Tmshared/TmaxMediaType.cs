using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information used to define a TrialMax media type</summary>
	public class CTmaxMediaType
	{
		#region Private Members
		
		/// <summary>Local member accessed by PrimaryType property</summary>
		private FTI.Shared.Trialmax.TmaxMediaTypes m_ePrimaryType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;
			
		/// <summary>Local member accessed by SecondaryType property</summary>
		private FTI.Shared.Trialmax.TmaxMediaTypes m_eSecondaryType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;
			
		/// <summary>Local member accessed by TertiaryType property</summary>
		private FTI.Shared.Trialmax.TmaxMediaTypes m_eTertiaryType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;
			
		/// <summary>Local member accessed by Name property</summary>
		private string m_strName = "";
			
		/// <summary>Local member accessed by HasFiles property</summary>
		private bool m_bHasFiles = true;
			
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxMediaType()
		{
		}// CTmaxMediaType()
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eMediaType">The primary media type value</param>
		/// <param name="strName">The media type name</param>
		public CTmaxMediaType(TmaxMediaTypes ePrimaryType, string strName)
		{
			Initialize(ePrimaryType, strName);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="ePrimaryType">The primary media type value</param>
		public CTmaxMediaType(TmaxMediaTypes ePrimaryType)
		{
			Initialize(ePrimaryType);
		}
		
		/// <summary>This method will initialize the object properties using the specified values</summary>
		/// <param name="ePrimaryType">The primary media type value</param>
		/// <param name="strName">The media type name</param>
		public void Initialize(TmaxMediaTypes ePrimaryType, string strName)
		{
			m_ePrimaryType = ePrimaryType;
				
			//	Set the secondary and tertiary types
			switch(ePrimaryType)
			{
				case TmaxMediaTypes.Document:	
				
					m_eSecondaryType = TmaxMediaTypes.Page;
					m_eTertiaryType  = TmaxMediaTypes.Treatment;
					m_bHasFiles		 = true;
					break;				

				case TmaxMediaTypes.Powerpoint:	
				
					m_eSecondaryType = TmaxMediaTypes.Slide;
					m_bHasFiles		 = true;
					break;				

				case TmaxMediaTypes.Recording:	
				
					m_eSecondaryType = TmaxMediaTypes.Segment;
					m_eTertiaryType  = TmaxMediaTypes.Clip;
					m_bHasFiles		 = true;
					break;				

				case TmaxMediaTypes.Deposition:	
				
					m_eSecondaryType = TmaxMediaTypes.Segment;
					m_eTertiaryType  = TmaxMediaTypes.Designation;
					m_bHasFiles		 = true;
					break;				

				case TmaxMediaTypes.Script:		
				
					m_eSecondaryType = TmaxMediaTypes.Scene;
					m_bHasFiles		 = false;
					break;				

				case TmaxMediaTypes.Unknown:	
				default:						
				
					Debug.Assert(false, "CTmaxMediaTypes::Initialize() -> Unknown media type");
					break;
					
			}// switch(eMediaType)
			
			if((strName != null) && (strName.Length > 0))
				m_strName = strName;
				
		}// public void Initialize(TmaxMediaTypes ePrimaryType, string strName)
		
		/// <summary>This method will initialize the object properties using the specified values</summary>
		/// <param name="ePrimaryType">The primary media type value</param>
		public void Initialize(TmaxMediaTypes ePrimaryType)
		{
			Initialize(ePrimaryType, null);
		}
		
		#endregion Public Methods
			
		#region Properties
		
		/// <summary>This property indicates whether or not external files are associated with this media type</summary>
		public bool HasFiles
		{
			get	{ return m_bHasFiles; }
		}

		/// <summary>This property contains the name of the media type</summary>
		public string Name
		{
			get
			{
				if((m_strName != null) && (m_strName.Length > 0))
					return m_strName;
				else
					return m_ePrimaryType.ToString();
			}
			set
			{
				m_strName = value;
			}
			
		} // Name property

		/// <summary>This is the enumerated TrialMax primary media type identifier</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes PrimaryType
		{
			get
			{
				return m_ePrimaryType;
			}
			set
			{
				if(value != m_ePrimaryType)
					Initialize(value, m_strName);
			}
			
		} // PrimaryType property

		/// <summary>This is the enumerated TrialMax secondary media type identifier</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes SecondaryType
		{
			get { return m_eSecondaryType; }
		}

		/// <summary>This is the enumerated TrialMax tertiary media type identifier</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes TertiaryType
		{
			get	{ return m_eTertiaryType; }
			
		} // TertiaryType property

		#endregion Properties
		
	}//	CTmaxMediaType
		
	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxMediaType objects
	/// </summary>
	public class CTmaxMediaTypes : CollectionBase
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxMediaTypes()
		{
			// Add an object for each TrialMax primary media type
			Add(new CTmaxMediaType(TmaxMediaTypes.Document, "Documents"));
			Add(new CTmaxMediaType(TmaxMediaTypes.Powerpoint, "PowerPoint Presentations"));
			Add(new CTmaxMediaType(TmaxMediaTypes.Recording, "Recordings"));
			Add(new CTmaxMediaType(TmaxMediaTypes.Deposition, "Depositions"));
			Add(new CTmaxMediaType(TmaxMediaTypes.Script, "Scripts"));
		
		}// public CTmaxMediaTypes()
		
		/// <summary>This method allows the caller to add a new item to the list</summary>
		/// <param name="tmaxMediaType">CTmaxMediaType object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxMediaType Add(CTmaxMediaType tmaxMediaType)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(tmaxMediaType as object);

				return tmaxMediaType;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxMediaType tmaxMediaType)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="tmaxMediaType">The object to be removed</param>
		public void Remove(CTmaxMediaType tmaxMediaType)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(tmaxMediaType as object);
			}
			catch
			{
			}
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxItem">The parameter object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxMediaType tmaxMediaType)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(tmaxMediaType as object);
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxMediaType value)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(value);
		}

		/// <summary>Called to locate the object with the specified primary TrialMax media type identifier</summary>
		/// <param name="eMediaType">The desired TrialMax media type</param>
		/// <returns>The object with the specified identifier</returns>
		public CTmaxMediaType Find(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)
		{
			// Search for the object with the same primary media type
			foreach(CTmaxMediaType tmaxType in base.List)
			{
				if((tmaxType.PrimaryType == eMediaType) || (tmaxType.SecondaryType == eMediaType))
				{
					return tmaxType;
				}
			}
			return null;

		}//	Find(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public CTmaxMediaType this[int index]
		{
			get 
			{ 
				return (base.List[index] as CTmaxMediaType);
			}
		}

		/// <summary>Overloaded version of [] operator to return the object with the specified media type</summary>
		/// <returns>The object with the specified media type</returns>
		public CTmaxMediaType this[FTI.Shared.Trialmax.TmaxMediaTypes eMediaType]
		{
			get 
			{ 
				return Find(eMediaType);
			}
		}

		/// <summary>This method is called to determine if the specified id represents a composite media format</summary>
		/// <param name="eMediaType">The MediaType value</param>
		/// <returns>true if composite media</returns>
		/// <remarks>Composite media means one primary source file contains multiple secondary media objects</remarks>
		public static bool IsCompositeMedia(TmaxMediaTypes eMediaType)
		{
			switch(eMediaType)
			{
				case TmaxMediaTypes.Powerpoint:	
				case TmaxMediaTypes.Slide:
				
					return true;				

				case TmaxMediaTypes.Document:	
				case TmaxMediaTypes.Recording:	
				case TmaxMediaTypes.Page:		
				case TmaxMediaTypes.Segment:	
				case TmaxMediaTypes.Treatment:	
				case TmaxMediaTypes.Clip:		
				case TmaxMediaTypes.Script:		
				case TmaxMediaTypes.Scene:		
				case TmaxMediaTypes.Unknown:	
				default:						
				
					return false;
			
			}// switch(eMediaType)
			
		}// IsCompositeMedia(TmaxMediaTypes eMediaType)
		
		#endregion Public Methods
		
	}//	CTmaxMediaTypes
		
}// namespace FTI.Shared.Trialmax
