using System;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;

using FTI.Shared.Win32;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains generic utility methods</summary>
	public class CTmaxToolbox
	{
		#region Constants
		
		public const string INVALID_FILENAME_CHARACTERS = "/\\:*?\"<>|";
		
		#endregion Constants
		
		#region Public Members
		
		#endregion Public Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxToolbox()
		{
		}
		
		/// <summary>This method is called to determine if the specified character is a digit</summary>
		/// <param name="cChar">The character to be evaluated</param>
		/// <returns>true if the character is a digit</returns>
		static public bool IsDigit(char cChar)
		{
			switch(cChar)
			{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				
					return true;
					
				default:
				
					return false;
			}
			
		}// IsDigit(char cChar)
		
		/// <summary>This method performs a smart Ascii/Numeric comparison to determine if one string is greater than the other</summary>
		/// <param name="str1">First string to be compared</param>
		/// <param name="str2">Second string to be compared</param>
		/// <returns>-1 if strString1 less than strString2, 0 if equal, 1 if strString1 greater than strString2</returns>
		static public int Compare(string strString1, string strString2, bool bIgnoreCase)
		{
			try
			{
				//	First try to use our newer faster method
				return FastCompare(strString1, strString2);
			}
			catch
			{
				//	Fall back on our original method
				return SmartCompare(strString1, strString2, bIgnoreCase);
			}

		}// static public int Compare(string strString1, string strString2, bool bIgnoreCase)
		
		/// <summary>This method is called to convert the decimal seconds to a string</summary>
		/// <param name="dSeconds">The decimal seconds</param>
		/// <returns>The string representation</returns>
		static public string SecondsToString(double dSeconds)
		{
			return SecondsToString(dSeconds, 1);	
		}
			
		/// <summary>This method is called to convert the decimal seconds to a string</summary>
		/// <param name="dSeconds">The decimal seconds</param>
		/// <param name="iPrecision">The desired precision</param>
		/// <returns>The string representation</returns>
		static public string SecondsToString(double dSeconds, int iPrecision)
		{
			string	strSeconds = "";
			string	strMilliSeconds = "";
			int		iIndex = 0;

			try
			{
				System.TimeSpan Pos = System.TimeSpan.FromSeconds(dSeconds);

				strSeconds = String.Format("{0:00}:{1:00}:{2:00}", (Pos.Hours + (Pos.Days * 24)), Pos.Minutes, Pos.Seconds);
				
				if((iPrecision > 0) && (Pos.Milliseconds > 0))
				{
					strMilliSeconds = ((double)Pos.Milliseconds / 1000.0).ToString();
				
					//	Get the portion that follows the decimal point
					if((iIndex = strMilliSeconds.IndexOf('.')) >= 0)
						strMilliSeconds = strMilliSeconds.Substring(iIndex + 1);
					
					//	Do we need to add any characters?
					while(strMilliSeconds.Length < iPrecision)
						strMilliSeconds += "0";
					
					//	Do we need to strip any characters?
					if(strMilliSeconds.Length > iPrecision)
						strMilliSeconds = strMilliSeconds.Substring(0, iPrecision);
						
					strSeconds += ("." + strMilliSeconds);
				}
				
			}
			catch
			{
				strSeconds = ("Invalid : " + dSeconds.ToString());
			}
			
			return strSeconds;
			
		}// static public string SecondsToString(double dSeconds, int iPrecision)
		
		/// <summary>This method is called to extract the line number from the specified PL value</summary>
		/// <param name="lPL">The composite page/line value</param>
		/// <returns>The line number</returns>
		static public int PLToLine(long lPL)
		{
			try
			{
				return (int)(lPL % 100);
			}
			catch
			{
				return -1;
			}
			
		}// static public int PLToLine(long lPL)
		
		/// <summary>This method is called to extract the page number from the specified PL value</summary>
		/// <param name="lPL">The composite page/line value</param>
		/// <returns>The page number</returns>
		static public long PLToPage(long lPL)
		{
			try
			{
				return ((lPL - PLToLine(lPL)) / 100);
			}
			catch
			{
				return -1;
			}
			
		}// static public int PLToPage(long lPL)
		
		/// <summary>This method is called to convert the composite PageLine value to a string</summary>
		/// <param name="lPL">The composite page/line value</param>
		/// <returns>The string representation</returns>
		static public string PLToString(long lPL)
		{
			long lPage = PLToPage(lPL);
			int	 iLine = PLToLine(lPL);
			
			if((lPage >= 0) && (iLine >= 0))
				return (lPage.ToString() + ":" + iLine.ToString());
			else
				return ("Invalid PL : " + lPL.ToString());
			
		}// static public string PLToString(long lPL)
		
		/// <summary>This method is called to convert the composite PageLine value to a string</summary>
		/// <param name="lPL">The composite page/line value</param>
		/// <summary>This method converts the page/line pair to a PL composite value</summary>
		/// <param name="lPage">The page number</param>
		/// <param name="iLine">The line number</param>
		/// <returns>The combined PL value</returns>
		static public long GetPL(long lPage, int iLine)
		{
			return ((lPage * 100) + iLine);
		}

		/// <summary>This method strips bounding qoutes from the specified string</summary>
		/// <param name="strString">The string to be stripped</param>
		/// <param name="bTrimEmbedded">true to trim whitespace after stripping quotes</param>
		/// <returns>the stripped string</returns>
		static public string StripQuotes(string strString, bool bTrimEmbedded)
		{
			if((strString != null) && (strString.Length > 1))
			{
				//	Check for bounding quotes
				if((strString.StartsWith("\"") == true) && (strString.EndsWith("\"") == true))
				{
					strString = strString.Substring(1, strString.Length - 2);

					//	Replace pairs of quotes
					strString = strString.Replace("\"\"", "\"");

					if(bTrimEmbedded == true)
						strString = strString.Trim();
				}

			}// if((strString != null) && (strString.Length > 1))

			return strString;

		}// static public string StripQuotes(string strString, bool bTrimEmbedded)

		/// <summary>This method strips bounding qoutes from the specified string</summary>
		/// <param name="strString">The string to be stripped</param>
		/// <returns>the stripped string</returns>
		static public string StripQuotes(string strString)
		{
			return StripQuotes(strString, false);

		}// static public string StripQuotes(string strString)

		/// <summary>This method strips High-Ascii characters put in a text file by Microsoft Office products</summary>
		/// <param name="strString">The string to be stripped</param>
		/// <param name="bReplace">True to replace the special characters</param>
		/// <returns>the modified string</returns>
		static public string StripMSChars(string strString, bool bReplace)
		{
			string strStripped = "";
			
			try
			{
				for(int i = 0; i < strString.Length; i++)
				{
					switch(strString[i])
					{
						case '\x82':

							if(bReplace == true)
								strStripped += ",";
							break;

						case '\x85':

							if(bReplace == true)
								strStripped += "...";
							break;

						case '\x88':

							if(bReplace == true)
								strStripped += "^";
							break;

						case '\x91':
						case '\x92':

							if(bReplace == true)
								strStripped += "'";
							break;

						case '\x93':
						case '\x94':

							if(bReplace == true)
								strStripped += "\"";
							break;

						case '\x97':

							if(bReplace == true)
								strStripped += "--";
							break;

						case '\x99':

							if(bReplace == true)
								strStripped += "(TM)";
							break;

						case '\xA9':
							
							if(bReplace == true)
								strStripped += "(C)";
							break;

						case '\xAE':

							if(bReplace == true)
								strStripped += "(R)";
							break;

						case '\xBC':

							if(bReplace == true)
								strStripped += "1/4";
							break;

						case '\xBD':

							if(bReplace == true)
								strStripped += "1/2";
							break;

						case '\xBE':

							if(bReplace == true)
								strStripped += "3/4";
							break;

						default:
							strStripped += strString[i];
							break;
					}

				}// for(int i = 0; i < strString.Length; i++)
			}
			catch
			{
				strStripped = strString;
			}
			
			return strStripped;

		}// static public string StripQuotes(string strString)

		/// <summary>This method checks the specified filename to make sure it contains no invalid characters</summary>
		/// <param name="strFilename">The filename to be checked</param>
		/// <param name="bPrompt">true to prompt the user if the name is invalid</param>
		/// <returns>true if successful</returns>
		static public bool CheckFilename(string strFilename, bool bPrompt)
		{
			string strMsg = "";

			if(strFilename == null) return false;
			if(strFilename.Length == 0) return false;

			if(strFilename.IndexOfAny(INVALID_FILENAME_CHARACTERS.ToCharArray()) >= 0)
			{
				//	Should we prompt the user?
				if(bPrompt == true)
				{
					strMsg = String.Format("{0} contains one or more invalid characters. Filenames can not include any of the following characters:\n\n{1}", strFilename, INVALID_FILENAME_CHARACTERS);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}

				return false;
			}
			else
			{
				return true;
			}

		}// public bool CheckFilename(string strFilename)

		/// <summary>This method checks the specified filename to make sure it contains no invalid characters</summary>
		/// <param name="strFilename">The filename to be checked</param>
		/// <param name="bPrompt">true to prompt the user if the name is invalid</param>
		/// <returns>true if successful</returns>
		static public string CleanFilename(string strFilename, bool bConfirm)
		{
			string strMsg = "";

			if(strFilename == null) return strFilename;
			if(strFilename.Length == 0) return strFilename;

			//	Is this already a valid filename?
			if(CheckFilename(strFilename, false) == true) return strFilename;

			//	Do we need to prompt before continuing?
			if(bConfirm == true)
			{
				strMsg = String.Format("{0} contains one or more invalid characters: {1}\n\nThey will be replaced with underscore characters (_)", strFilename, INVALID_FILENAME_CHARACTERS);
				if(MessageBox.Show(strMsg, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return strFilename;
			}

			//	Replace each invalid character
			for(int i = 0; i < INVALID_FILENAME_CHARACTERS.Length; i++)
				strFilename = strFilename.Replace(INVALID_FILENAME_CHARACTERS[i], '_');

			return strFilename;

		}// public bool CheckFilename(string strFilename)

		/// <summary>
		/// This method is called to extract a substring from the start of the specified string.
		/// If the first character is a digit, the substring consists of all digits. If the first
		///	character is not a digit, the substring consists off all characters until a digit is found.
		/// </summary>
		/// <param name="cChar">The string to be parsed</param>
		/// <returns>The extracted substring</returns>
		static public string Extract(ref string strString)
		{
			string strSubString = "";

			while(strString.Length > 0)
			{
				if(strSubString.Length > 0)
				{
					//	Should we stop here?
					if(IsDigit(strSubString[0]) != IsDigit(strString[0]))
						break;
				}

				//	Transfer the next character to the substring
				strSubString += strString[0];
				strString = strString.Remove(0, 1);
			}

			return strSubString;

		}// static public string Extract(ref string strString)

		/// <summary>Called to get the date that appears in the specified string if it exists</summary>
		/// <param name="strString">The string to be parsed</param>
		/// <returns>The extracted date</returns>
		static public string ParseDateFromString(string strString)
		{
			DateTime dateTime = System.DateTime.Now;
			char[] acDelimiters = { ' ', '\t' };

			//	Make sure we have a valid string
			if((strString == null) || (strString.Length == 0))
				return "";

			//	Split the string into its component parts
			string[] aSubStrings = strString.Split(acDelimiters);
			if((aSubStrings != null) && (aSubStrings.GetUpperBound(0) >= 0))
			{
				foreach(string O in aSubStrings)
				{
					try
					{
						Convert.ToDateTime(O);
						return O;
					}
					catch
					{
					}

				}// foreach(string O in aSubStrings)

			}// if((aSubStrings != null) && (aSubStrings.GetUpperBound(0) >= 0))

			return "";

		}// static public string ParseDateFromString(string strString)

		/// <summary>This method is called to remove the first (or all) occurances of zero padding of numerical values in a string</summary>
		/// <param name="strString">The string to be stripped</param>
		/// <param name="bAllInstances">True to strip all instances of zero padding</param>
		/// <returns>The modified string</returns>
		static public string StripZeroPadding(string strString, bool bAllInstances)
		{
			string strSubstring = "";
			string strStripped = "";
			long lNumber = 0;

			while(strString.Length > 0)
			{
				//	Get the next alpha or numeric substring
				strSubstring = Extract(ref strString);

				//	Is this substring numeric?
				if(IsDigit(strSubstring[0]) == true)
				{
					try
					{
						//	Convert to an actual number and then add to the return string
						lNumber = Convert.ToInt64(strSubstring);
						strStripped += lNumber.ToString();
					}
					catch
					{
						//	Put the text back on an error
						strStripped += strSubstring;
					}

					//	Are we stopping on the first instance?
					if(bAllInstances == false)
					{
						//	Add the remaining string
						strStripped += strString;
						break;
					}

				}
				else
				{
					strStripped += strSubstring;
				}

			}// while(strString.Length > 0)

			return strStripped;

		}// static public string StripZeroPadding(string strString)

		/// <summary>This method uses ellipses to shorten the path to fit within the specified control</summary>
		/// <param name="strPath">The path to be shortened</param>
		/// <param name="wndControl">The control used to display the path</param>
		/// <returns>the adjusted path</returns>
		static public string FitPathToWidth(string strPath, System.Windows.Forms.Control wndControl)
		{
			string strAdjusted = "";

			try
			{
				StringBuilder strBuilder = new StringBuilder(strPath, FTI.Shared.Win32.Kernel.MAX_PATH);
				System.IntPtr hdc = new IntPtr();

				System.Drawing.Graphics wndGraphics = System.Drawing.Graphics.FromHwnd(wndControl.Handle);

				hdc = wndGraphics.GetHdc();

				FTI.Shared.Win32.Shell.PathCompactPath(hdc, strBuilder, wndControl.Width);
				strAdjusted = strBuilder.ToString();

				wndGraphics.ReleaseHdc(hdc);

			}
			catch
			{
				strAdjusted = strPath;
			}

			return strAdjusted;

		}// static public string FitPathToWidth(string strPath, System.Windows.Forms.Control wndControl)

		/// <summary>This method returns the folder where the application executable resides</summary>
		/// <returns>The folder where the application is stored</returns>
		static public string GetApplicationFolder()
		{
			try
			{
				Process appProcess = Process.GetCurrentProcess();
				return System.IO.Path.GetDirectoryName(appProcess.MainModule.FileName);
			}
			catch
			{
				return "";
			}

		}// static public string GetApplicationFolder()

		/// <summary>This method converts the string value to a system boolean value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <param name="bIsBoolean">true if the value is recognized as a boolean value</param>
		/// <returns>the converted value</returns>
		static public bool StringToBool(string strValue, ref bool bIsBoolean)
		{
			bool bValue = false;

			//	We want a somewhat more flexible interpretation of booleans than normal system bool
			switch(strValue.ToLower())
			{
				case "true":
				case "yes":
				case "1":
				case "-1": // ADO default for TRUE

					bIsBoolean = true;
					bValue = true;
					break;

				case "false":
				case "no":
				case "0":

					bIsBoolean = true;
					bValue = false;
					break;

				default:

					bIsBoolean = false;
					bValue = false;
					break;

			}// switch(strValue.ToLower())

			return bValue;

		}// static public bool StringToBool(string strValue, ref bool bIsBoolean)

		/// <summary>This method converts the string value to a boolean value</summary>
		/// <param name="strValue">The value to be converted</param>
		/// <returns>the converted value</returns>
		static public bool StringToBool(string strValue)
		{
			bool bIsBoolean = true;
			return StringToBool(strValue, ref bIsBoolean);
		}

		/// <summary>This method is called to determine if the string is a valid boolean value</summary>
		/// <param name="strValue">The boolean text</param>
		/// <returns>true if the text represents a boolean value</returns>
		static public bool IsBoolean(string strValue)
		{
			bool bIsBoolean = true;
			StringToBool(strValue, ref bIsBoolean);
			return bIsBoolean;
		}

		/// <summary>This method will replace special characters in the specified string to make it appropriate for a SQL statement</summary>
		/// <param name="strSQL">The SQL text</param>
		/// <returns>The encoded SQL string</returns>
		static public string SQLEncode(string strSQL)
		{
			string strEncoded = strSQL.Replace("'", "''");
			return strEncoded;
		}

		/// <summary>This method will convert a boolean value to an appropriate SQL value</summary>
		/// <param name="bValue">The boolean value</param>
		/// <returns>The corresponding SQL string</returns>
		static public string BoolToSQL(bool bValue)
		{
			return (bValue == true) ? "-1" : "0";
		}

		/// <summary>This method is called to determine if the specified string is a coded property</summary>
		/// <returns>The associated coded property</returns>
		static public TmaxCodedProperties GetCodedProperty(string strName)
		{
			foreach(TmaxCodedProperties O in Enum.GetValues(typeof(TmaxCodedProperties)))
			{
				if(String.Compare(O.ToString(), strName, true) == 0)
					return O;

			}// foreach(TmaxCodedProperties O in Enum.GetValues(typeof(TmaxCodedProperties)))

			return TmaxCodedProperties.Invalid;

		}// static public TmaxCodedProperties GetCodedProperty(string strName)

		/// <summary>This method is called to determine if the specified string is an Import Property</summary>
		/// <returns>The associated import property</returns>
		static public TmaxImportProperties GetImportProperty(string strName)
		{
			foreach(TmaxImportProperties O in Enum.GetValues(typeof(TmaxImportProperties)))
			{
				if(String.Compare(O.ToString(), strName, true) == 0)
					return O;

			}// foreach(TmaxImportProperties O in Enum.GetValues(typeof(TmaxImportProperties)))

			return TmaxImportProperties.Invalid;

		}// static public TmaxImportProperties GetImportProperty(string strName)

		/// <summary>This method is called to convert the text label to its equivalent media type</summary>
		/// <param name="strType">The text equivalent of the enumerated type identifier</param>
		/// <returns>The enumerated type identifier</returns>
		static public TmaxMediaTypes GetTypeFromString(string strType)
		{
			TmaxMediaTypes eType = TmaxMediaTypes.Unknown;

			try
			{
				if((strType != null) && (strType.Length > 0))
				{
					Array aTypes = Enum.GetValues(typeof(TmaxMediaTypes));

					foreach(TmaxMediaTypes O in aTypes)
					{
						if(String.Compare(O.ToString(), strType, true) == 0)
						{
							eType = O;
							break;
						}

					}// foreach(TmaxMediaTypes O in aTypes)

				}// if((strType != null) && (strType.Length > 0))

			}
			catch
			{
			}

			return eType;

		}// static public TmaxMediaTypes GetTypeFromString(string strType)

		/// <summary>This method is called to set the XmlScriptFormat value</summary>
		/// <param name="strFormat">The text equivalent of the enumerated format identifier</param>
		/// <returns>The enumerated format identifier</returns>
		static public TmaxXmlScriptFormats GetFormatFromString(string strFormat)
		{
			TmaxXmlScriptFormats eFormat = TmaxXmlScriptFormats.Unknown;

			try
			{
				if((strFormat != null) && (strFormat.Length > 0))
				{
					Array aFormats = Enum.GetValues(typeof(TmaxXmlScriptFormats));

					foreach(TmaxXmlScriptFormats O in aFormats)
					{
						if(String.Compare(O.ToString(), strFormat, true) == 0)
						{
							eFormat = O;
							break;
						}

					}// foreach(TmaxXmlScriptFormats O in aFormats)

				}// if((strFormat != null) && (strFormat.Length > 0))

			}
			catch
			{
			}

			return eFormat;

		}// static public TmaxXmlScriptFormats GetFormatFromString(string strFormat)

		/// <summary>This method is called to determine if the user is pressing the Control key</summary>
		/// <returns>True if Control key is pressed</returns>
		static public bool IsControlKeyPressed()
		{
			//	Is the user pressing the control key?
			if((Shared.Win32.User.GetKeyState(Shared.Win32.User.VK_CONTROL) & 0x8000) != 0)
				return true;
			else
				return false;

		}// static public bool IsControlKeyPressed()

		/// <summary>This method is called to determine if the user is pressing the Shift key</summary>
		/// <returns>True if Shift key is pressed</returns>
		static public bool IsShiftKeyPressed()
		{
			//	Is the user pressing the control key?
			if((Shared.Win32.User.GetKeyState(Shared.Win32.User.VK_SHIFT) & 0x8000) != 0)
				return true;
			else
				return false;

		}// static public bool IsShiftKeyPressed()

		/// <summary>This method performs a smart Ascii/Numeric comparison to determine if one string is greater than the other</summary>
		/// <param name="str1">First string to be compared</param>
		/// <param name="str2">Second string to be compared</param>
		/// <returns>-1 if strString1 less than strString2, 0 if equal, 1 if strString1 greater than strString2</returns>
		/// <remarks>This is the original implementation of the TrialMax smart string sorting</remarks>
		static public int SmartCompare(string strString1, string strString2, bool bIgnoreCase)
		{
			int iReturn = 0;
			string str1;
			string str2;
			string strSubString1;
			string strSubString2;
			long lNumber1;
			long lNumber2;

			str1 = strString1;
			str2 = strString2;

			//	Continue while strings evaluate as equal
			while(iReturn == 0)
			{
				//	Get the next pair of substrings
				strSubString1 = Extract(ref str1);
				strSubString2 = Extract(ref str2);

				//	Compare the substrings
				if(strSubString1.Length == 0)
				{
					//	Are both strings empty?
					if(strSubString2.Length == 0)
					{
						return 0;	//	They must be equal
					}
					else
					{
						return -1;	//	String2 has more characters
					}

				}
				else
				{
					if(strSubString2.Length == 0)
					{
						return 1;	//	String1 has more characters
					}
					else
					{
						//	Is substring1 numerical?
						if(IsDigit(strSubString1[0]) == true)
						{
							//	Is substring2 numerical?
							if(IsDigit(strSubString2[0]) == true)
							{
								//	Convert to numbers
								try
								{
									lNumber1 = System.Convert.ToInt64(strSubString1);
									lNumber2 = System.Convert.ToInt64(strSubString2);
								}
								catch
								{
									return 1;
								}

								//	Are the substrings numerically equal?
								if(lNumber1 == lNumber2)
								{
									//	Use the string lengths if numerically equal
									if(strSubString1.Length < strSubString2.Length)
										iReturn = 1;
									else if(strSubString1.Length > strSubString2.Length)
										iReturn = -1;
								}
								else
								{
									if(lNumber1 < lNumber2)
										iReturn = -1;
									else
										iReturn = 1;
								}

							}
							else
							{
								//	strSubString1 has numbers, strSubString2 has characters
								//if(bIgnoreCase == true)
								//    iReturn = String.Compare(strSubString1, strSubString2, StringComparison.OrdinalIgnoreCase);
								//else
								//    iReturn = String.Compare(strSubString1, strSubString2, StringComparison.Ordinal);

								if(bIgnoreCase == true)
									iReturn = Win32.Kernel.lstrcmpi(strSubString1, strSubString2);
								else
									iReturn = Win32.Kernel.lstrcmp(strSubString1, strSubString2);

								//	Must make one or the other so that we break out
								if(iReturn == 0)
									iReturn = 1;
							}

						}
						else
						{
							//	Is substring2 numerical?
							if(IsDigit(strSubString2[0]) == true)
							{
								//	strSubString2 has numbers, strSubString1 has characters
								if(bIgnoreCase == true)
									iReturn = Win32.Kernel.lstrcmpi(strSubString1, strSubString2);
								else
									iReturn = Win32.Kernel.lstrcmp(strSubString1, strSubString2);

								//	Must make one or the other so that we break out
								if(iReturn == 0)
									iReturn = 1;
							}
							else
							{
								//	Both substrings are non-numeric
								if(bIgnoreCase == true)
									iReturn = Win32.Kernel.lstrcmpi(strSubString1, strSubString2);
								else
									iReturn = Win32.Kernel.lstrcmp(strSubString1, strSubString2);
							}

						}// if(IsDigit(strSubString1[0]) == true) 

					}// if(strSubString2.Length == 0)

				}// if(strSubString1.Length == 0)

			}// while(iReturn == 0) 

			return iReturn;

		}// static public int SmartCompare(string strString1, string strString2, bool bIgnoreCase)

		/// <summary>This is a faster version of the smart string comparison (SmartCompare)</summary>
		/// <param name="str1">First string to be compared</param>
		/// <param name="str2">Second string to be compared</param>
		/// <returns>-1 if strString1 less than strString2, 0 if equal, 1 if strString1 greater than strString2</returns>
		/// <remarks>This was copied from an article written by Vesian Cepa and published on the Code Project web site</remarks>
		public static int FastCompare(string str1, string str2)
		{
			//	Check for special cases
			if((str1 == null) && (str2 == null)) return 0;
			else if(str1 == null) return -1;
			else if(str2 == null) return 1;

			if((str1.Equals(string.Empty) && (str2.Equals(string.Empty)))) return 0;
			else if(str1.Equals(string.Empty)) return -1;
			else if(str2.Equals(string.Empty)) return -1;

			//	Window's Explorer style, special case
			bool sp1 = Char.IsLetterOrDigit(str1, 0);
			bool sp2 = Char.IsLetterOrDigit(str2, 0);
			if(sp1 && !sp2) return 1;
			if(!sp1 && sp2) return -1;

			int i1 = 0, i2 = 0; //current index
			int r = 0; // temp result
			while(true)
			{
				bool c1 = Char.IsDigit(str1, i1);
				bool c2 = Char.IsDigit(str2, i2);
				if(!c1 && !c2)
				{
					bool letter1 = Char.IsLetter(str1, i1);
					bool letter2 = Char.IsLetter(str2, i2);
					if((letter1 && letter2) || (!letter1 && !letter2))
					{
						if(letter1 && letter2)
						{
							r = Char.ToLower(str1[i1]).CompareTo(Char.ToLower(str2[i2]));
						}
						else
						{
							r = str1[i1].CompareTo(str2[i2]);
						}
						if(r != 0) return r;
					}
					else if(!letter1 && letter2) return -1;
					else if(letter1 && !letter2) return 1;
				}
				else if(c1 && c2)
				{
					r = CompareNumeric(str1, ref i1, str2, ref i2);
					if(r != 0) return r;
				}
				else if(c1)
				{
					return -1;
				}
				else if(c2)
				{
					return 1;
				}
				i1++;
				i2++;
				if((i1 >= str1.Length) && (i2 >= str2.Length))
				{
					return 0;
				}
				else if(i1 >= str1.Length)
				{
					return -1;
				}
				else if(i2 >= str2.Length)
				{
					return -1;
				}
			}

		}// public static int FastCompare(string str1, string str2)

		#endregion Public Methods

		#region Private Methods
		
		/// <summary>Compares the numerical portion of the specified strings</summary>
		/// <param name="str1">First string to be compared</param>
		/// <param name="i1">Index where the number starts in str1</param>
		/// <param name="str2">Second string to be compared</param>
		/// <param name="i1">Index where the number starts in str1</param>
		/// <returns>-1 if str1 less than str2, 0 if equal, 1 if str1 greater than str2</returns>
		private static int CompareNumeric(string str1, ref int idx1, string str2, ref int idx2)
		{
			int nzStart1 = idx1; 
			int	nzStart2 = idx2; // nz = non zero
			int end1 = idx1; 
			int	end2 = idx2;

			//	Locate the index of the end of the number
			ScanNumEnd(str1, idx1, ref end1, ref nzStart1);
			ScanNumEnd(str2, idx2, ref end2, ref nzStart2);
			
			int start1 = idx1; idx1 = end1 - 1;
			int start2 = idx2; idx2 = end2 - 1;

			int nzLength1 = end1 - nzStart1;
			int nzLength2 = end2 - nzStart2;

			//	Compare the non-zero string lengths
			if(nzLength1 < nzLength2) return -1;
			else if(nzLength1 > nzLength2) return 1;

			//	Numbers must be of equal length if we reached this point
			for(int j1 = nzStart1, j2 = nzStart2; j1 <= idx1; j1++, j2++)
			{
				int r = str1[j1].CompareTo(str2[j2]);
				if(r != 0) return r;
			}
			
			//	Account for any zeroes that may be present since the non-zero
			//	portions are equal
			int length1 = end1 - start1;
			int length2 = end2 - start2;
			
			if(length1 == length2) return 0;
			if(length1 > length2) return -1;	//	Reverse this to make 0001 > 01
			return 1;

		}// private static int CompareNumeric(string str1, ref int idx1, string str2, ref int idx2)

		/// <summary>Called to find the index of the last digit in the numerical portion of the string</summary>
		/// <param name="s">the source string</param>
		/// <param name="start">the index to where the number starts</param>
		/// <param name="end">returns the index where the number ends</param>
		/// <param name="nzStart">returns the position of the first non-zero digit</param>
		private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)
		{
			bool countZeros = true;

			nzStart = start;
			end = start;

			while(Char.IsDigit(s, end))
			{
				if(countZeros && s[end].Equals('0'))
				{
					nzStart++;
				}
				else countZeros = false;
				end++;
				if(end >= s.Length) break;
			
			}

		}// private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)

		#endregion Private Methods

	}// public class CTmaxToolbox

}// namespace FTI.Shared
