using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information used to define a TrialMax registration source type</summary>
	public class CTmaxSourceType
	{
		#region Private Members
		
		/// <summary>Local member accessed by Extensions property</summary>
		private ArrayList m_aExtensions = new ArrayList();
			
		/// <summary>Local member accessed by MediaType property</summary>
		private FTI.Shared.Trialmax.TmaxMediaTypes m_eMediaType = FTI.Shared.Trialmax.TmaxMediaTypes.Unknown;
			
		/// <summary>Local member accessed by RegSourceType property</summary>
		private FTI.Shared.Trialmax.RegSourceTypes m_eRegSourceType = FTI.Shared.Trialmax.RegSourceTypes.AllFiles;
			
		/// <summary>Local member accessed by Name property</summary>
		private string m_strName = "";
			
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxSourceType()
		{
		}// CTmaxSourceType()
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eMediaType">The MediaType value</param>
		/// <param name="eRegSourceType">The RegSourceType value</param>
		/// <param name="strExtensions">The delimited extensions string</param>
		/// <param name="strName">The name used to describe the source type</param>
		public CTmaxSourceType(TmaxMediaTypes eMediaType, RegSourceTypes eRegSourceType, string strExtensions, string strName)
		{
			Initialize(eMediaType, eRegSourceType, strExtensions, strName);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eMediaType">The MediaType value</param>
		/// <param name="eRegSourceType">The RegSourceType value</param>
		/// <param name="strExtensions">The delimited extensions string</param>
		public CTmaxSourceType(TmaxMediaTypes eMediaType, RegSourceTypes eRegSourceType, string strExtensions)
		{
			Initialize(eMediaType, eRegSourceType, strExtensions);
		}
		
		/// <summary>This method will initialize the object properties using the specified values</summary>
		/// <param name="eMediaType">The MediaType value</param>
		/// <param name="eRegSourceType">The RegSourceType value</param>
		/// <param name="strExtensions">The delimited extensions string</param>
		/// <param name="strName">The name used to describe the source type</param>
		public void Initialize(TmaxMediaTypes eMediaType, RegSourceTypes eRegSourceType, string strExtensions, string strName)
		{
			m_eMediaType   = eMediaType;
			m_eRegSourceType = eRegSourceType;
				
			if((strName != null) && (strName.Length > 0))
				m_strName = strName;
				
			if((strExtensions != null) && (strExtensions.Length > 0))
				SetExtensions(strExtensions);
				
		}// CTmaxSourceType()
		
		/// <summary>This method will initialize the object properties using the specified values</summary>
		/// <param name="eMediaType">The MediaType value</param>
		/// <param name="eRegSourceType">The RegSourceType value</param>
		/// <param name="strExtensions">The delimited extensions string</param>
		public void Initialize(TmaxMediaTypes eMediaType, RegSourceTypes eRegSourceType, string strExtensions)
		{
			Initialize(eMediaType, eRegSourceType, strExtensions, null);
		}
		
		/// <summary>This method will construct a file filter string appropriate for a Windows common dialog</summary>
		/// <param name="strDelimiter">The extension delimiter</param>
		/// <param name="bWildcard">True to include leading wildcard</param>
		/// <returns>The file filter string</returns>
		public string GetFileFilterString(string strDelimiter, bool bWildcard)
		{
			string strFilter = "";
			
			if((m_aExtensions != null) && (m_aExtensions.Count > 0))
			{
				for(int i = 0; i < m_aExtensions.Count; i++)
				{
					if(bWildcard == true)
						strFilter += ("*." + m_aExtensions[i].ToString());
					else
						strFilter += m_aExtensions[i].ToString();
					
					if(i < m_aExtensions.Count - 1)
						strFilter += strDelimiter;
				}
			}
			
			return strFilter;
			
		}// GetFileFilterString(string strDelimiter, bool bWildcard)
		
		/// <summary>This method will construct a file filter string appropriate for a Windows common dialog</summary>
		/// <param name="strDelimiter">The extension delimiter</param>
		/// <returns>The file filter string</returns>
		public string GetFileFilterString(string strDelimiter)
		{
			return GetFileFilterString(strDelimiter, true);
			
		}// GetFileFilterString(string strDelimiter)
		
		/// <summary>This method will construct a file filter string appropriate for a Windows common dialog</summary>
		/// <returns>The file filter string</returns>
		public string GetFileFilterString()
		{
			return GetFileFilterString(";", true);
		}
		
		/// <summary>This method will construct a string appropriate for a dropdown selection box</summary>
		/// <returns>The file selection string</returns>
		public string GetFileSelectionString()
		{
			string strSelection = Name + " ";
			
			if((m_aExtensions != null) && (m_aExtensions.Count > 0))
			{
				for(int i = 0; i < m_aExtensions.Count; i++)
				{
					strSelection += ("*." + m_aExtensions[i].ToString());
					
					if(i < m_aExtensions.Count - 1)
						strSelection += " ";
				}
			}
			
			return strSelection;
			
		}// GetFileSelectionString()
		
		/// <summary>This method will populate the extensions collection using the specified delimited string</summary>
		/// <returns>The number of extensions added to the collection</returns>
		public int SetExtensions(string strExtensions)
		{
			char[]	acDelimiters = {'|',',',';',' '};
			
			try
			{
				//	Make sure we have a valid collection
				if(m_aExtensions == null) return 0;

				//	Clear the existing extensions
				m_aExtensions.Clear();
				
				//	Parse the string into individual extensions
				string[] aExtensions = strExtensions.Split(acDelimiters);

				if(aExtensions != null)
				{
					foreach(string strExtension in aExtensions)
					{
						if(strExtension.StartsWith("*.") == true)
							m_aExtensions.Add(strExtension.Substring(2, strExtension.Length - 2));
						else if(strExtension.StartsWith(".") == true)
							m_aExtensions.Add(strExtension.Substring(1, strExtension.Length - 1));
						else
							m_aExtensions.Add(strExtension);
							
					}
					
				}
			
			}
			catch
			{
			}

			return m_aExtensions.Count;
			
		}// SetExtensions(string strExtensions)
		
		/// <summary>This method will check the specified extension to see if it belongs to the media type</summary>
		/// <param name="strExtension">The file extension to be tested</param>
		/// <returns>true if valid file extension</returns>
		public bool CheckExtension(string strExtension)
		{
			try
			{
				if(m_aExtensions != null)
				{
					foreach(string str in m_aExtensions)
					{
						if(String.Compare(strExtension, str, true) == 0)
						{
							return true;
						}
						
					}// foreach(string str in aExtensions)
					
				}// if(aExtensions != null)
			
			}
			catch
			{
			}

			return false;
			
		}// CheckFileExtension(string strExtension)
		
		/// <summary>This method will check the specified file to see if it belongs to the media type</summary>
		/// <param name="strFilename">The name of the file to be tested</param>
		/// <returns>true if valid file </returns>
		public bool CheckFile(string strFilename)
		{
			string strExtension;
			
			try
			{
				strExtension = System.IO.Path.GetExtension(strFilename);
				if(strExtension.StartsWith("."))
					strExtension = strExtension.Remove(0,1);
					
				if((strExtension != null) && (strExtension.Length > 0))
					return CheckExtension(strExtension);			
			}
			catch
			{
			}

			return false;
			
		}// CheckFile(string strFilename)
		
		#endregion Public Methods
			
		#region Properties
		
		/// <summary>This property contains the collection of file extensions assocaited with the media type</summary>
		public ArrayList Extensions
		{
			get
			{
				return m_aExtensions;
			}
			
		} // Extensions property

		/// <summary>This property contains the name of the source type</summary>
		public string Name
		{
			get
			{
				if((m_strName != null) && (m_strName.Length > 0))
					return m_strName;
				else
					return m_eRegSourceType.ToString();
			}
			set
			{
				m_strName = value;
			}
			
		} // Name property

		/// <summary>This is the enumerated TrialMax registration source type</summary>
		public FTI.Shared.Trialmax.RegSourceTypes RegSourceType
		{
			get
			{
				return m_eRegSourceType;
			}
			set
			{
				m_eRegSourceType = value;
			}
			
		} // RegSourceType property

		/// <summary>This is the enumerated TrialMax media type identifier</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes MediaType
		{
			get
			{
				return m_eMediaType;
			}
			set
			{
				m_eMediaType = value;
			}
			
		} // MediaType property

		#endregion Properties
		
	}//	CTmaxSourceType
		
	/// <summary>Objects of this class are used to manage a dynamic array of CTmaxSourceType objects</summary>
	public class CTmaxSourceTypes : CollectionBase
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME = "SourceTypes";
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Local member bound to UseMultiPageTIFF property</summary>
		private CTmaxSourceType m_tmaxMultiPageTIFF = null;
			
		/// <summary>Local member bound to UseMultiPageTIFF property</summary>
		private bool m_bUseMultiPageTIFF = false;
			
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxSourceTypes()
		{
			//	Add an object for each known source type
			Add(new CTmaxSourceType(TmaxMediaTypes.Document, RegSourceTypes.Document, "tif,tiff,png,pcx,bmp,jpg,jpeg,gif", "Documents"));
			Add(new CTmaxSourceType(TmaxMediaTypes.Document, RegSourceTypes.Adobe, "pdf", "Adobe PDF"));
			Add(new CTmaxSourceType(TmaxMediaTypes.Powerpoint, RegSourceTypes.Powerpoint, "ppt,pps", "PowerPoint Presentations"));
			Add(new CTmaxSourceType(TmaxMediaTypes.Recording, RegSourceTypes.Recording, "avi,mpg,mpeg,wmv,mp4,mp3,wma,wav", "Recordings"));
			Add(new CTmaxSourceType(TmaxMediaTypes.Deposition, RegSourceTypes.Deposition, "xmlt,log", "Depositions"));
			
			m_tmaxMultiPageTIFF = new CTmaxSourceType(TmaxMediaTypes.Document, RegSourceTypes.MultiPageTIFF, "tif,tiff,mpt", "MultiPageTIFF");
		
		}// public CTmaxSourceTypes()
		
		/// <summary>This method allows the caller to add a new item to the list</summary>
		/// <param name="tmaxSourceType">CTmaxSourceType object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxSourceType Add(CTmaxSourceType tmaxSourceType)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.List.Add(tmaxSourceType as object);

				return tmaxSourceType;
			}
			catch
			{
				return null;
			}
			
		}// Add(CTmaxSourceType tmaxSourceType)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="tmaxItem">The object to be removed</param>
		public void Remove(CTmaxSourceType tmaxSourceType)
		{
			try
			{
				// Use base class to process actual collection operation
				base.List.Remove(tmaxSourceType as object);
			}
			catch
			{
			}
		
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxSourceType">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxSourceType tmaxSourceType)
		{
			// Use base class to process actual collection operation
			return base.List.Contains(tmaxSourceType as object);
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="tmaxSourceType">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxSourceType tmaxSourceType)
		{
			// Find the 0 based index of the requested entry
			return base.List.IndexOf(tmaxSourceType);
		}

		/// <summary>Called to locate the first object that supports the specified extension</summary>
		/// <param name="strExtension">The desired file extension</param>
		/// <returns>The first object that supports the specified extension</returns>
		public CTmaxSourceType FindFromExtension(string strExtension)
		{
			//	Are we using the MultiPageTIFF type?
			if(this.UseMultiPageTIFF == true)
			{
				if(this.MultiPageTIFF.CheckExtension(strExtension) == true)
					return this.MultiPageTIFF;
			}
			
			// Search for the object with the specified extension
			foreach(CTmaxSourceType tmaxSourceType in base.List)
			{
				if(tmaxSourceType.CheckExtension(strExtension) == true)
				{
					return tmaxSourceType;
				}
			}
			return null;

		}//	FindFromExtension(string strExtension)

		/// <summary>Called to locate the first object that supports the specified file</summary>
		/// <param name="strFilename">The name of the file</param>
		/// <returns>The first object that supports the specified file</returns>
		public CTmaxSourceType FindFromFilename(string strFilename)
		{
			//	Are we using the MultiPageTIFF type?
			if(this.UseMultiPageTIFF == true)
			{
				if(this.MultiPageTIFF.CheckFile(strFilename) == true)
					return this.MultiPageTIFF;
			}
			
			// Search for the object with the specified extension
			foreach(CTmaxSourceType tmaxSourceType in base.List)
			{
				if(tmaxSourceType.CheckFile(strFilename) == true)
				{
					return tmaxSourceType;
				}
			}
			return null;

		}//	FindFromFilename(string strExtension)

		/// <summary>Called to locate the object with the specified TrialMax media type identifier</summary>
		/// <param name="eMediaType">The desired TrialMax media type</param>
		/// <returns>The object with the specified identifier</returns>
		public CTmaxSourceType Find(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)
		{
			//	NOTE:	Nothing I can do here for MultiPageTIFF because it is a
			//			standard document type
			
			// Search for the object with the same media type
			foreach(CTmaxSourceType tmaxSourceType in base.List)
			{
				if(tmaxSourceType.MediaType == eMediaType)
				{
					return tmaxSourceType;
				}
			}
			return null;

		}//	Find(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)

		/// <summary>Called to locate the object with the specified TrialMax registration source type identifier</summary>
		/// <param name="eRegSourceType">The desired TrialMax registration source type</param>
		/// <returns>The object with the specified identifier</returns>
		public CTmaxSourceType Find(FTI.Shared.Trialmax.RegSourceTypes eRegSourceType)
		{
			if(eRegSourceType == RegSourceTypes.MultiPageTIFF)
			{
				return this.MultiPageTIFF;
			}
			else
			{
				// Search for the object with the same name
				foreach(CTmaxSourceType tmaxSourceType in base.List)
				{
					if(tmaxSourceType.RegSourceType == eRegSourceType)
					{
						return tmaxSourceType;
					}
				}
				return null;
			}

		}//	Find(FTI.Shared.Trialmax.RegSourceTypes eRegSourceType)

		/// <summary>Called to determine if the file is of the specified source type</summary>
		/// <param name="strFilename">The name of the file to be checked</param>
		/// <param name="eSourceType">The enumerated source type identifier</param>
		/// <returns>The matching type descriptor if found to be a match</returns>
		public CTmaxSourceType CheckFile(string strFilename, RegSourceTypes eSourceType)
		{
			CTmaxSourceType tmaxSourceType = null;
			
			//	Get the source type descriptor for this file
			if((tmaxSourceType = FindFromFilename(strFilename)) != null)
			{
				if(tmaxSourceType.RegSourceType == eSourceType)
					return tmaxSourceType;
			}
			
			//	No match found
			return null;

		}//	public CTmaxSourceType CheckFile(string strFilename, RegSourceTypes eSourceType)

		/// <summary>Called to determine if the file is of the specified media type</summary>
		/// <param name="strFilename">The name of the file to be checked</param>
		/// <param name="eSourceType">The enumerated media type identifier</param>
		/// <returns>The matching type descriptor if found to be a match</returns>
		public CTmaxSourceType CheckFile(string strFilename, TmaxMediaTypes eMediaType)
		{
			CTmaxSourceType tmaxSourceType = null;
			
			//	Get the source type descriptor for this file
			if((tmaxSourceType = FindFromFilename(strFilename)) != null)
			{
				if(tmaxSourceType.MediaType == eMediaType)
					return tmaxSourceType;
			}
			
			//	No match found
			return null;

		}//	public CTmaxSourceType CheckFile(string strFilename, TmaxMediaTypes eMediaType)
		
		/// <summary>Called to determine if the file extension is of the specified source type</summary>
		/// <param name="strExtension">The extension of the file to be checked</param>
		/// <param name="eSourceType">The enumerated source type identifier</param>
		/// <returns>The matching type descriptor if found to be a match</returns>
		public CTmaxSourceType CheckExtension(string strExtension, RegSourceTypes eSourceType)
		{
			CTmaxSourceType tmaxSourceType = null;
			
			//	Get the source type descriptor for this file
			if((tmaxSourceType = FindFromExtension(strExtension)) != null)
			{
				if(tmaxSourceType.RegSourceType == eSourceType)
					return tmaxSourceType;
			}
			
			//	No match found
			return null;

		}//	public CTmaxSourceType CheckExtension(string strExtension, RegSourceTypes eSourceType)

		/// <summary>Called to determine if the file extension is of the specified media type</summary>
		/// <param name="strExtension">The extension of the file to be checked</param>
		/// <param name="eMediaType">The enumerated media type identifier</param>
		/// <returns>The matching type descriptor if found to be a match</returns>
		public CTmaxSourceType CheckExtension(string strExtension, TmaxMediaTypes eMediaType)
		{
			CTmaxSourceType tmaxSourceType = null;
			
			//	Get the source type descriptor for this file
			if((tmaxSourceType = FindFromExtension(strExtension)) != null)
			{
				if(tmaxSourceType.MediaType == eMediaType)
					return tmaxSourceType;
			}
			
			//	No match found
			return null;

		}//	public CTmaxSourceType CheckExtension(string strExtension, TmaxMediaTypes eMediaType)
		
		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public CTmaxSourceType this[int index]
		{
			get 
			{ 
				return (base.List[index] as CTmaxSourceType);
			}
		}

		/// <summary>Overloaded version of [] operator to return the object with the specified media type</summary>
		/// <returns>The object with the specified media type</returns>
		public CTmaxSourceType this[FTI.Shared.Trialmax.TmaxMediaTypes eMediaType]
		{
			get 
			{ 
				return Find(eMediaType);
			}
		}

		/// <summary>Overloaded version of [] operator to return the object with the specified registration source type</summary>
		/// <returns>The object with the specified registration source type</returns>
		public CTmaxSourceType this[FTI.Shared.Trialmax.RegSourceTypes eRegSourceType]
		{
			get 
			{ 
				return Find(eRegSourceType);
			}
		}

		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			foreach(CTmaxSourceType O in this)
			{
				if(O.RegSourceType != RegSourceTypes.NoSource)
					xmlIni.Write(O.RegSourceType.ToString(), O.GetFileFilterString(";", false));
			}
			
			if(this.MultiPageTIFF != null)
				xmlIni.Write(this.MultiPageTIFF.RegSourceType.ToString(), this.MultiPageTIFF.GetFileFilterString(";", false));
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		public void Load(CXmlIni xmlIni)
		{
			string strFilter = "";
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			foreach(CTmaxSourceType O in this)
			{
				if(O.RegSourceType != RegSourceTypes.NoSource)
				{
					//	Read the file filter for this source type
					strFilter = xmlIni.Read(O.RegSourceType.ToString());
					if(strFilter.Length > 0)
						O.SetExtensions(strFilter);
				}
			}
			
			//	Check for the MultiPageTIFF extensions
			if(this.MultiPageTIFF != null)
			{
				strFilter = xmlIni.Read(this.MultiPageTIFF.RegSourceType.ToString());
				if(strFilter.Length > 0)
					this.MultiPageTIFF.SetExtensions(strFilter);
			}
			
		}// public void Load(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The hidden MultiPageTIFF type identifier</summary>
		public CTmaxSourceType MultiPageTIFF
		{
			get { return m_tmaxMultiPageTIFF; }
		}
		
		/// <summary>True if multipage TIFF is enabled</summary>
		public bool UseMultiPageTIFF
		{
			get { return ((m_bUseMultiPageTIFF == true) && (this.MultiPageTIFF != null)); }
			set { m_bUseMultiPageTIFF = value; }
		}
		
		#endregion Properties
		
	}//	CTmaxSourceTypes
		
}// namespace FTI.Shared.Trialmax
