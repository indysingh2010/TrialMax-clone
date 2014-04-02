using System;
using System.Collections;

namespace FTI.Shared
{
	/// <summary>This class is used to create and manage a user configurable log file </summary>
	public class CEventLog
	{
		#region Private Members
		
		/// <summary>Collection of CEventLogItems objects associated with the event</summary>
		private CEventLogItems m_aItems;
			
		/// <summary>Local member accessed by the AddTimeStamp property</summary>
		private bool m_bAddTimeStamp;			
		
		/// <summary>Local member accessed by the AddDateToFilename property</summary>
		private bool m_bAddDateToFilename;			
		
		/// <summary>Local member accessed by the Folder property</summary>
		private string m_strFolder;			
		
		/// <summary>Local member accessed by the Filename property</summary>
		private string m_strFilename;			
		
		/// <summary>Local member accessed by the Extension property</summary>
		private string m_strExtension = ".txt";			
		
		/// <summary>Local member accessed by the FileSpec property</summary>
		private string m_strFileSpec;			
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CEventLog()
		{
			m_aItems = new CEventLogItems();
		}
		
		/// <summary>
		/// This method is called to add a new entry to the log
		/// </summary>
		/// <returns>true if successful</returns>
		public virtual bool Add()
		{
			string strEntry;
			System.IO.StreamWriter fsLogFile;
			
			//	First make sure we have something to add
			strEntry = GetLogEntry();
			if(strEntry.Length == 0) return false;
			
			//	Construct the full path specification
			SetFileSpec();
			if(m_strFileSpec.Length == 0) return false;
			
			try
			{
				fsLogFile = System.IO.File.AppendText(m_strFileSpec);
				fsLogFile.WriteLine(strEntry);
				fsLogFile.Close();
			}
			catch
			{
				return false;
			}
			
			return true;
		
		}//	Add()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to get the current date as a string</summary>
		/// <returns>The string representation of the current date</returns>
		protected virtual string GetDateAsString()
		{
			DateTime Date = DateTime.Now;
			return Date.ToString("MM-dd-yyyy");
		}

		/// <summary>This method is called to get the current time as a string</summary>
		/// <returns>The string representation of the current time</returns>
		protected virtual string GetTimeAsString()
		{
			DateTime Time = DateTime.Now;
			return Time.ToString("HH:mm:ss.fff");
		}

		/// <summary>
		/// This method is called to construct a log entry using the current value assigned to each item
		/// </summary>
		/// <returns>The string to be logged to file</returns>
		protected virtual string GetLogEntry()
		{
			string strEntry = "";
			
			//	Should we add a time stamp?
			if(m_bAddTimeStamp == true)
			{
				strEntry = GetDateAsString() + "  " + GetTimeAsString() + " : ";
			}
			
			//	Add the value for each item in the collection
			foreach(CEventLogItem item in m_aItems)
			{
				strEntry += item.Value;
				strEntry += item.Delimiter;
			}
			
			return strEntry;
		}

		/// <summary>
		/// This method is called to construct the full path specification of the log file
		/// </summary>
		protected virtual void SetFileSpec()
		{
			//	Set the folder
			m_strFileSpec = m_strFolder;
			if(m_strFileSpec.EndsWith("\\") == false)
				m_strFileSpec += "\\";
			
			//	Add the filename
			m_strFileSpec += m_strFilename;
			
			//	Append the date if requested
			if(m_bAddDateToFilename)
				m_strFileSpec += GetDateAsString();
				
			//	Now add the extension
			if(m_strExtension.Length > 0)
			{
				if((m_strFileSpec.EndsWith(".") == false) && (m_strExtension.StartsWith(".") == false))
				{
					m_strFileSpec += ".";
				}
				
				m_strFileSpec += m_strExtension;
			}
			
		}// SetFileSpec()

		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>
		/// Property that exposes the CEventLogItems collection
		/// </summary>
		public CEventLogItems Items
		{
			get
			{
				return m_aItems;
			}
		}

		/// <summary>Controls whether or not a time stamp is added to the log entry</summary>
		public bool AddTimeStamp
		{
			get
			{
				return m_bAddTimeStamp;
			}
			set
			{
				m_bAddTimeStamp = value;
			}
		}

		/// <summary>Controls whether or not the current date is added to the specified filename</summary>
		public bool AddDateToFilename
		{
			get
			{
				return m_bAddDateToFilename;
			}
			set
			{
				m_bAddDateToFilename = value;
			}
		}

		/// <summary>Folder (path) used to contruct the full path specification of the log file</summary>
		public string Folder
		{
			get
			{
				return m_strFolder;
			}
			set
			{
				m_strFolder = value;
			}
		}

		/// <summary>Filename used to contruct the full path specification of the log file</summary>
		public string Filename
		{
			get
			{
				return m_strFilename;
			}
			set
			{
				m_strFilename = value;
			}
		}

		/// <summary>Extension used to contruct the full path specification of the log file</summary>
		public string Extension
		{
			get
			{
				return m_strExtension;
			}
			set
			{
				m_strExtension = value;
			}
		}

		/// <summary>Full file specification for the log file</summary>
		public string FileSpec
		{
			get
			{
				return m_strFileSpec;
			}
		}

		#endregion Properties
		
		#region Items
		
		/// <summary>
		/// Objects of this class are used to define an item to be stored in the event log
		/// </summary>
		public class CEventLogItem
		{
			/// <summary>Local member accessed by Name property</summary>
			private string m_strName;
			
			/// <summary>Local member accessed by Value property</summary>
			private string m_strValue;
			
			/// <summary>Local member accessed by the Delimiter property</summary>
			private string m_strDelimiter = "  ";			
		
			/// <summary>Default constructor</summary>
			public CEventLogItem()
			{
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			public CEventLogItem(string strName)
			{
				m_strName = strName;
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strDelimiter">Delimiter assigned to the item</param>
			public CEventLogItem(string strName, string strDelimiter)
			{
				m_strName = strName;
				m_strDelimiter = strDelimiter;
			}
			
			/// <summary>Constructor</summary>
			/// <param name="strName">Name assigned to the item</param>
			/// <param name="strDelimiter">Delimiter assigned to the item</param>
			/// <param name="strValue">Value assigned to the item</param>
			public CEventLogItem(string strName, string strDelimiter, string strValue)
			{
				m_strName = strName;
				m_strDelimiter = strDelimiter;
				m_strValue = strValue;
			}
			/// <summary>This property contains the full path specification of the item</summary>
			public string Name
			{
				get
				{
					return m_strName;
				}
				set
				{
					m_strName = value;
				}
			
			} // Name property

			/// <summary>This property contains the value written to the log</summary>
			public string Value
			{
				get
				{
					return m_strValue;
				}
				set
				{
					m_strValue = value;
				}
			
			} // Value property

			/// <summary>Delimiter added to the end of this item in the log file</summary>
			public string Delimiter
			{
				get
				{
					return m_strDelimiter;
				}
				set
				{
					m_strDelimiter = value;
				}
			}

		}//	CEventLogItem
		
		/// <summary>
		/// Objects of this class are used to manage a dynamic array of CEventLogItem objects
		/// </summary>
		public class CEventLogItems : CollectionBase
		{
			/// <summary>Default constructor</summary>
			public CEventLogItems()
			{
			}

			/// <summary>This method allows the caller to add a new item to the list</summary>
			/// <param name="elItem">CEventLogItem object to be added to the list</param>
			/// <returns>The object just added if successful, null otherwise</returns>
			public CEventLogItem Add(CEventLogItem elItem)
			{
				try
				{
					// Use base class to perform actual collection operation
					base.List.Add(elItem as object);

					return elItem;
				}
				catch
				{
					return null;
				}
			
			}// Add(CEventLogItem elItem)

			public bool SetValue(string strName, string strValue)
			{
				try
				{
					foreach(CEventLogItem item in base.List)
					{
						if(item.Name == strName)
						{
							item.Value = strValue;
							return true;
						}
					}
				}
				catch
				{
				}
				
				return false;
			
			}// SetValue(string strName, string strValue)

		}//	CEventLogItems
		
		#endregion Items
		
	}//	CEventLog
	
}//	FTI.Shared
