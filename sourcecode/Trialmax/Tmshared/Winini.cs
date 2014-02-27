using System;
using System.Windows.Forms;
/// <summary>
/// Namespace consisting of utility classes developed for Trialmax
/// </summary>
namespace FTI.Shared
{
	/// <summary>
	/// Utility class used to manage WIN32 INI files in a .NET assembly
	/// </summary>
	public class CWinIni
	{
		/// <summary>
		/// Initial size of the buffer used when calling the Win32 API functions
		/// </summary>
		const int INITIAL_BUFFER_SIZE = 1024;

		#region Private class members
		
		/// <summary><BR> Path to the active INI file </BR></summary>
		private string m_strFolder;
		
		/// <summary><BR> Full path specification for the INI file </BR></summary>
		private string m_strFilespec;
		
		/// <summary><BR> Name of current section within the INI file </BR></summary>
		private string m_strSection;
		
		/// <summary><BR> Flag to indicate if file was found when object was opened </BR></summary>
		private bool m_bFileFound;

		#endregion Private Class Members
		
		#region Private class methods
		
		/// <summary>
		/// Extracts a token from an ascii delimited string
		/// </summary>
		/// <param name="strText">Delimited text string</param>
		/// <param name="delimiter">The delimiter</param>
		/// <param name="intIndex">The zero-based index of the token to return</param>
		/// <returns>Returns the nth token from a string.</returns>
		private string GetToken(string strText, char[] acDelimiter, int iIndex)
		{
			string strReturn = "";

			string[] aTokens = strText.Split(acDelimiter);

			if(aTokens.GetUpperBound(0) >= iIndex)
				strReturn = aTokens[iIndex];

			return strReturn;
		} 
		
		/// <summary>
		/// Encodes special characters that XML has problems with:
		///       Character       Encoded to
		///       &               &amp
		///       "               &quot
		///       <               &lt
		///       >               &gt
		/// </summary>
		/// <param name="strText">Text that needs encoding</param>
		/// <returns>Encoded Xml text</returns>
		private string XmlEncode(string strText)
		{
			string strReturn = strText;

			strReturn = strReturn.Replace( "&", "&amp;" );
			strReturn = strReturn.Replace( "\"", "&quot;");
			strReturn = strReturn.Replace( "<", "&lt;");
			strReturn = strReturn.Replace( ">", "&gt;");

			return strReturn;
		}
	
		#endregion Private class methods
		
		#region Properties
		
		/// <summary><BR> Default object constructor </BR></summary>
		public CWinIni()
		{
			m_strFolder = System.IO.Directory.GetCurrentDirectory();
			m_strFilespec = "";
			m_strSection = "";
			m_bFileFound = false;		
		}

		/// <summary><BR> Flag to indicate if file was found when caller set the
		/// Filename property or called Open() </BR></summary>
		///	<value>	FileFound accesses the m_bFileFound class member </value>
		public bool FileFound
		{
			get
			{
				return m_bFileFound;
			}
		}

		/// <summary><BR> Full path specification to active INI file </BR></summary>
		///	<value>	Filename accesses the m_strFilespec class member </value>
		public string Filename
		{
			get
			{
				return m_strFilespec;
			}
			set
			{
				Open(value, "");
			}
		}

		/// <summary><BR> Name of the active section within the file </BR></summary>
		///	<value>	Section accesses the m_strSection class member </value>
		public string Section
		{
			get
			{
				return m_strSection;
			}
			set
			{
				m_strSection = value;
			}
		}
		
		/// <summary><BR> Path that contains the INI files </BR></summary>
		///	<value>	Folder accesses the m_strFolder class member </value>
		public string Folder
		{
			get
			{
				return m_strFolder;
			}
			set
			{
				m_strFolder = value;
				if(m_strFolder.Length == 0)
					m_strFolder = System.IO.Directory.GetCurrentDirectory();
			}
		}

		#endregion Properties
	
		#region File Methods
		
		/// <summary><BR> Call to open the specified INI file </BR></summary>
		///	<param name="strFilename">Name of file to open (path optional)</param>
		/// <returns>true if file exists</returns>
		/// <remarks>If no path is provided with strFilename the current working folder is used</remarks>
		public bool Open(string strFilename)
		{
			return Open(strFilename, "");
		}
		
		/// <summary><BR> Call to open the specified INI file </BR></summary>
		///	<param name="strFilename">Name of file to open (path optional)</param>
		/// <param name="strSection">Name of section to line up on (optional)</param>
		/// <returns>true if file exists</returns>
		/// <remarks>If no path is provided with strFilename the current working folder is used</remarks>
		public bool Open(string strFilename, string strSection)
		{
			string strFolder;

			//	First flush the cache buffer if a file is already open
			Close();

			//	Construct the complete file specification
			if(strFilename.Length > 0)
			{
				//	Separate the folder
				strFolder = System.IO.Path.GetDirectoryName(strFilename);
				
				//  Did the caller provide a path?
				if(strFolder.Length > 0)
				{
					m_strFilespec = strFilename;
				}
				else
				{
					//	Build the file specification using the Folder property
					m_strFilespec = m_strFolder;
					if(m_strFilespec.EndsWith("\\") == false)
						m_strFilespec += "\\";
					m_strFilespec += strFilename;
				}	
			}

			//	Set the active section
			m_strSection = strSection;

			// 	See if the file exists. This gives the caller a means of checking
			// 	before calling a read function
			if(System.IO.File.Exists(m_strFilespec) == true)
				m_bFileFound = true;

			return m_bFileFound;
		}
	
		/// <summary><BR> Call to close the active INI file </BR></summary>
		public void Close()
		{
			//	Commit all changes before closing
			Commit();
			
			//	Reset the class members
			m_bFileFound = false;
			m_strFilespec = "";
			m_strFolder = "";
			m_strSection = "";
		}
	
		/// <summary><BR> Call to commit all changes made to the file </BR></summary>
		/// <remarks>Windows does not normally commit changes until the file is closed</remarks>
		public void Commit()
		{
			//	Flush the cache buffer
			Win32.IniApi.WritePrivateProfileString(null, null, null, m_strFilespec);
		}
		
		/// <summary><BR> Call to delete the specified line in the active section </BR></summary>
		///	<param name="strLine">Name of line (key) to be deleted</param>
		public void DeleteLine(string strLine)
		{
			Win32.IniApi.WritePrivateProfileString(m_strSection, strLine, null, m_strFilespec);
		}
		
		/// <summary><BR> Call to delete the specified section in the active file </BR></summary>
		///	<param name="strSection">Name of section to be deleted</param>
		public void DeleteSection(string strSection)
		{
			Win32.IniApi.WritePrivateProfileString(m_strSection, null, null, m_strFilespec);
		}
		
		/// <summary><BR> Call to delete the active section </BR></summary>
		public void DeleteSection()
		{
			DeleteSection(m_strSection);
		}
		
		/// <summary><BR> Call to save the file as an XML formatted file </BR></summary>
		///	<param name="strSection">Name of XML file</param>
		public bool SaveAsXml(string strFilename)
		{
			char[]	charEquals = {'='};
			string	strSections;
			int		iSize;
			int		iMaxSize;
			string	strSection;
			int		iSection;
			int		iNameValue;
			string	strName;
			string	strValue;
			string	strNameValue;
			string	strNameValues;
			byte[]	abAPI = new byte[1];
			char[]	cNull = {'\0'};
			System.IO.StreamWriter fsXml;
			
			// Get all sections names
			// Making sure allocate enough space for data returned
			for(iMaxSize = INITIAL_BUFFER_SIZE / 2,	iSize = iMaxSize;
				iSize != 0 && iSize >= (iMaxSize-2);
				iMaxSize *= 2)
			{
				abAPI = new byte[iMaxSize];
				iSize = Win32.IniApi.GetPrivateProfileSectionNames(abAPI, iMaxSize, m_strFilespec);
			}

			// Convert the byte array into a .NET string
			strSections = System.Text.Encoding.ASCII.GetString(abAPI);
		
			// Create XML File
			try
			{
				fsXml = System.IO.File.CreateText(strFilename);
			}
			catch
			{
				return false;
			}

			// Write the opening xml
			fsXml.WriteLine( "<?xml version=\"1.0\"?><configuration>" );
		
			// Loop through each section
			for(iSection = 0, strSection = GetToken(strSections, cNull, iSection);
				strSection.Length > 0;
				strSection = GetToken(strSections, cNull, ++iSection))
			{
				// Write a Node for the Section
				fsXml.WriteLine( "<section name=\"" + strSection + "\">" );
			
				// Get all values in this section, making sure to allocate enough space
				for(iMaxSize = INITIAL_BUFFER_SIZE,	iSize = iMaxSize;
					iSize != 0 && iSize >= (iMaxSize-2);
					iMaxSize *= 2)
				{
					abAPI = new Byte[iMaxSize];
					iSize = Win32.IniApi.GetPrivateProfileSection(strSection, abAPI, iMaxSize, m_strFilespec);
				}

				// convert the byte array into a .NET string
				strNameValues = System.Text.Encoding.ASCII.GetString(abAPI);
			
				// Loop through each Name/Value pair
				for(iNameValue = 0,
					strNameValue = GetToken(strNameValues, cNull, iNameValue);
					strNameValue.Length > 0;
					strNameValue = GetToken(strNameValues, cNull, ++iNameValue) )
				{
					// Get the name and value from the entire null separated string of name/value pairs
					// Also escape out the special characters, (ie. &"<> )
					strName = XmlEncode(GetToken( strNameValue, charEquals, 0));
					strValue = XmlEncode(strNameValue.Substring( strName.Length + 1));

					// Write the XML Name/Value Node to the xml file
					fsXml.WriteLine( "<setting name=\"" + strName + "\" value=\"" + strValue + "\"/>");
				}
				
				// Close the section node
				fsXml.WriteLine( "</section>" );
			}
		
			// Thats it
			fsXml.WriteLine( "</configuration>" );
			fsXml.Close();
			return true;
		}
		
		/// <summary><BR> Call to save the file as an XML formatted file </BR></summary>
		///	<remarks>The filename is constructed by replacing the INI file extension with .xml</remarks>
		public bool SaveAsXml()
		{
			string strFolder;
			string strFilename;
			
			if(m_strFilespec.Length > 0)
			{
				strFolder = System.IO.Path.GetDirectoryName(m_strFilespec);
				strFilename = System.IO.Path.GetFileNameWithoutExtension(m_strFilespec);
				
				if(strFolder.Length > 0)
					strFilename = strFolder + "\\" + strFilename;
				strFilename += ".xml";
				
				return SaveAsXml(strFilename);
			}
			else
			{
				return false;
			}
		}
		
		#endregion File Methods
		
		#region Read Methods
		
		/// <summary><BR> Call to read the specified line in the active section as a string value </BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="strDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public string ReadString(string strLine, string strDefault)
		{
			byte[]  abString = new byte[512];
			string  strString = "";
			int		iChars;
			
			//	Read the string from the ini file
			iChars = Win32.IniApi.GetPrivateProfileString(m_strSection, strLine, strDefault, abString, 512, m_strFilespec);
			
			//	Convert from a byte array to a string
			if(iChars > 0)
				strString = System.Text.Encoding.ASCII.GetString(abString, 0, iChars);
			
			return strString;
		}
		/// <summary><BR> Call to read the specified line in the active section as a string value </BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public string ReadString(string strLine)
		{
			return ReadString(strLine, "");
		}			
		/// <summary><BR> Call to read the specified line in the active section as a string value </BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public string ReadString(int iLine)
		{
			string strLine = iLine.ToString();
			return ReadString(strLine);
		}			
		/// <summary><BR> Call to read the specified line in the active section as a string value </BR></summary>
		///	<param name="strLine">Number of line (key) in the active section</param>
		///	<param name="strDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public string ReadString(int iLine, string strDefault)
		{
			string strLine = iLine.ToString();
			return ReadString(strLine, strDefault);
		}
		
		/// <summary><BR> Call to read the specified line in the active section as an integer</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="iDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public int ReadInteger(string strLine, int iDefault)
		{
			string	strDefault = iDefault.ToString();
			string	strValue;
			int		iReturn;
			
			strValue = ReadString(strLine, strDefault);
			
			try
			{
				//	Is the value stored as a hexidecimal number?
				if((strValue.StartsWith("0x") == true) || (strValue.StartsWith("0X") == true))
				{
					iReturn = Int32.Parse(strValue.Substring(2), System.Globalization.NumberStyles.HexNumber);
				}
				else
				{
					//	This allows us to handle values that may be stored as a floating point
					iReturn = System.Convert.ToInt32(Single.Parse(strValue));
				}
			}
			catch
			{
				iReturn = System.Convert.ToInt32(strDefault);
			}
				
			return iReturn;			
		}
		/// <summary><BR> Call to read the specified line in the active section as an integer</BR></summary>
		///	<param name="strLine">Number of line (key) in the active section</param>
		///	<param name="iDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public int ReadInteger(int iLine, int iDefault)
		{
			string	strLine = iLine.ToString();
			return ReadInteger(strLine, iDefault);
		}
		/// <summary><BR> Call to read the specified line in the active section as an integer</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public int ReadInteger(string strLine)
		{	
			return ReadInteger(strLine, 0);
		}			
		/// <summary><BR> Call to read the specified line in the active section as a string value </BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public int ReadInteger(int iLine)
		{
			return ReadInteger(iLine, 0);
		}			

		/// <summary><BR> Call to read the specified line in the active section as a floating point value</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="iDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public Single ReadFloat(string strLine, Single fDefault)
		{
			string	strDefault = fDefault.ToString();
			string	strValue;
			Single  fReturn;
			
			strValue = ReadString(strLine, strDefault);
			
			try
			{
				//	Is the value stored as a hexidecimal number?
				if((strValue.StartsWith("0x") == true) || (strValue.StartsWith("0X") == true))
				{
					fReturn = Single.Parse(strValue.Substring(2), System.Globalization.NumberStyles.HexNumber);
				}
				else
				{
					//	This allows us to handle values that may be stored as a floating point
					fReturn = Single.Parse(strValue);
				}
			}
			catch
			{
				fReturn = Single.Parse(strDefault);
			}
				
			return fReturn;			
		}
		/// <summary><BR> Call to read the specified line in the active section as a floating point value</BR></summary>
		///	<param name="strLine">Number of line (key) in the active section</param>
		///	<param name="iDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public Single ReadFloat(int iLine, Single fDefault)
		{
			string	strLine = iLine.ToString();
			return ReadFloat(strLine, fDefault);
		}
		/// <summary><BR> Call to read the specified line in the active section as a floating point value</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public Single ReadFloat(string strLine)
		{	
			return ReadFloat(strLine, (Single)0);
		}			
		/// <summary><BR> Call to read the specified line in the active section as a floating point value </BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public Single ReadFloat(int iLine)
		{
			return ReadFloat(iLine, (Single)0);
		}			

		/// <summary><BR> Call to read the specified line in the active section as a boolean value</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="iDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public bool ReadBool(string strLine, bool bDefault)
		{
			string	strDefault;
			string	strValue;
			
			strDefault = bDefault == true? "true" : "false";
			
			strValue = ReadString(strLine, strDefault);
			strValue = strValue.ToLower();

			if(strValue.CompareTo("true") == 0)
				return true;
			else
				return false;
		}
		/// <summary><BR> Call to read the specified line in the active section as a boolean value</BR></summary>
		///	<param name="strLine">Number of line (key) in the active section</param>
		///	<param name="iDefault">Default return value</param>
		/// <returns>Value at specified line (key)</returns>
		public bool ReadBool(int iLine, bool bDefault)
		{
			string	strLine = iLine.ToString();
			return ReadBool(strLine, bDefault);
		}
		/// <summary><BR> Call to read the specified line in the active section as a boolean value</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public bool ReadBool(string strLine)
		{	
			return ReadBool(strLine, false);
		}			
		/// <summary><BR> Call to read the specified line in the active section as a boolean value </BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		/// <returns>Value at specified line (key)</returns>
		public bool ReadBool(int iLine)
		{
			return ReadBool(iLine, false);
		}			
		
		#endregion Read Methods
		
		#region Write Methods
		
		/// <summary><BR> Call to write the specified line in the active section as a string value </BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="strValue">Value to be written</param>
		public void WriteString(string strLine, string strValue)
		{
			Win32.IniApi.WritePrivateProfileString(m_strSection, strLine, strValue, m_strFilespec);
		}
		/// <summary><BR> Call to write the specified line in the active section as a string value </BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		///	<param name="strValue">Value to be written</param>
		public void WriteString(int iLine, string strValue)
		{
			string strLine = iLine.ToString();
			WriteString(strLine, strValue);
		}
		
		/// <summary><BR> Call to write the specified line in the active section as an integer</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="iValue">Value to be written</param>
		public void WriteInteger(string strLine, int iValue)
		{
			string strValue = System.Convert.ToString(iValue);
			WriteString(strLine, strValue);
		}
		/// <summary><BR> Call to write the specified line in the active section as an integer</BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		///	<param name="iValue">Value to be written</param>
		public void WriteInteger(int iLine, int iValue)
		{
			string strLine = iLine.ToString();
			WriteInteger(strLine, iValue);
		}
		
		/// <summary><BR> Call to write the specified line in the active section as a float point value</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="iValue">Value to be written</param>
		public void WriteFloat(string strLine, Single fValue)
		{
			string strValue = System.Convert.ToString(fValue);
			WriteString(strLine, strValue);
		}
		/// <summary><BR> Call to write the specified line in the active section as a float point value</BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		///	<param name="iValue">Value to be written</param>
		public void WriteFloat(int iLine, Single fValue)
		{
			string strLine = iLine.ToString();
			WriteFloat(strLine, fValue);
		}
		
		/// <summary><BR> Call to write the specified line in the active section as a boolean value</BR></summary>
		///	<param name="strLine">Name of line (key) in the active section</param>
		///	<param name="iValue">Value to be written</param>
		public void WriteBool(string strLine, bool bValue)
		{
			string strValue;
			
			if(bValue == true)
				strValue = "true";
			else
				strValue = "false";
				
			WriteString(strLine, strValue);
		}
		/// <summary><BR> Call to write the specified line in the active section as a boolean value</BR></summary>
		///	<param name="iLine">Number of line (key) in the active section</param>
		///	<param name="iValue">Value to be written</param>
		public void WriteBool(int iLine, bool bValue)
		{
			string strLine = iLine.ToString();
			WriteBool(strLine, bValue);
		}
		
		#endregion Write Methods
	
	}// public class CTMIni


}// namespace FTI.Trialmax.Utilities
