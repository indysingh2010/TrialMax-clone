using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains the information required to manage a load file converter</summary>
	public class CTmaxLoadFileConverter
	{
		#region Private Members
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to ConfigurationFilename property</summary>
		private string m_strConfigurationFilename = "";
		
		/// <summary>Local member bound to UseCrossReference property</summary>
		protected bool m_bUseCrossReference = true;

		/// <summary>Local member bound to Extensions property</summary>
		protected string m_strExtensions = "";

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxLoadFileConverter()
		{
		
		}// CTmaxLoadFileConverter()
		
		/// <summary>Overridden base class member to get default text representation</summary>
		/// <returns>The converter name</returns>
		public override string ToString()
		{
			if(m_strName.Length > 0)
				return m_strName;
			else
				return "No-name Converter";
		
		}// public override string ToString()

		/// <summary>This method is called to get the filter string appropriate for this converter's source file</summary>
		/// <returns>The filter string for this converter</returns>
		public string GetFilterString()
		{
			char[]	acDelimiters = {'|',',',';'};
			string	strFilter = "";
			string	str1 = "";
			bool	bIsFirst = true;
			
			if((m_strExtensions != null) && (m_strExtensions.Length > 0))
			{
				//	Parse the string into individual extensions
				string[] aExtensions = m_strExtensions.Split(acDelimiters);

				if((aExtensions != null) && (aExtensions.Length > 0))
				{
					strFilter = (m_strName + " Files ");
					foreach(string O in aExtensions)
					{
						//	Is this the first extension?
						if(bIsFirst == true)
							bIsFirst = false;
						else
							str1 += ";";
							
						str1 += ("*." + O.ToLower());
						
					}// foreach(string O in aExtensions)
					
					strFilter = String.Format("{0} Files ({1})|{1}|All files (*.*)|*.*", m_strName, str1);
					
				}// if((aExtensions != null) && (aExtensions.Length > 0))

			}// if((m_strExtensions != null) && (m_strExtensions.Length > 0))
				
			if(strFilter.Length == 0)
				strFilter = "All files (*.*)|*.*";

			return strFilter;
		
		}// public string GetFilterString()

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Name assigned to the load file descriptor</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>Name of the configuration file used to process the load file</summary>
		public string ConfigurationFilename
		{
			get { return m_strConfigurationFilename; }
			set { m_strConfigurationFilename = value; }
		}
		
		/// <summary>True if the load file requires a cross reference</summary>
		public bool UseCrossReference
		{
			get { return m_bUseCrossReference; }
			set { m_bUseCrossReference = value; }
		}
		
		/// <summary>Default file extensions for source files</summary>
		public string Extensions
		{
			get { return m_strExtensions; }
			set { m_strExtensions = value; }
		}
		
		#endregion Properties

	}//	public class CTmaxLoadFileConverter

	/// <summary>This class manages a list of source filter objects</summary>
	public class CTmaxLoadFileConverters : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxLoadFileConverters() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxConverter">CTmaxLoadFileConverter object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxLoadFileConverter Add(CTmaxLoadFileConverter tmaxConverter)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxConverter as object);

				return tmaxConverter;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxLoadFileConverter Add(CTmaxLoadFileConverter tmaxConverter)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxConverter">The filter object to be removed</param>
		public void Remove(CTmaxLoadFileConverter tmaxConverter)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxConverter as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxLoadFileConverter tmaxConverter)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxConverter">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxLoadFileConverter tmaxConverter)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxConverter as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxLoadFileConverter this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxLoadFileConverter);
			}
		}

		/// <summary>This method is called to find the converter with the specified name</summary>
		/// <param name="strName">The name of the converter</param>
		/// <returns>The converter object if found</returns>
		public CTmaxLoadFileConverter Find(string strName)
		{
			foreach(CTmaxLoadFileConverter O in this)
			{
				if(String.Compare(O.Name, strName, true) == 0)
					return O;
			}
			return null;
		
		}// public CTmaxLoadFileConverter Find(string strName)
		
		/// <summary>This method is called to get the index of the converter with the specified name</summary>
		/// <param name="strName">The name of the converter</param>
		/// <returns>The zero-based index if found</returns>
		public int IndexOf(string strName)
		{
			CTmaxLoadFileConverter tmaxConverter = Find(strName);
			
			if(tmaxConverter != null)
				return IndexOf(tmaxConverter);
			else
				return -1;
			
		}// public int IndexOf(string strName)
		
		#endregion Public Methods
		
	}//	public class CTmaxLoadFileConverters : CTmaxSortedArrayList
		
}// namespace FTI.Shared.Trialmax
