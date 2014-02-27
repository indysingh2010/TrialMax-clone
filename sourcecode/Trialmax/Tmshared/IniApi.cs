using System;
using System.Data;
using System.Runtime.InteropServices;

namespace FTI.Shared.Win32
{
	/// <summary>INI API Wrapper class </summary>
	public abstract class IniApi
	{
		/// <summary>WritePrivateProfileString API call</summary>
		[ DllImport("kernel32.dll", EntryPoint="WritePrivateProfileStringA") ]
		public extern static int WritePrivateProfileString(
			string lpAppName,
			string lpKeyName,
			string lpValue,
			string lpFileName);

		/// <summary>GetPrivateProfileString API call</summary>
		[ DllImport("kernel32.dll", EntryPoint="GetPrivateProfileStringA") ]
		public extern static int GetPrivateProfileString(
			string lpAppName,// section name
			string lpKeyName,// key name
			string lpDefault,// default string
			[MarshalAs(UnmanagedType.LPArray)] byte[] lpReturnedString,// destination buffer
			int nSize,// size of destination buffer
			string lpFileName);// initialization file name
	
		/// <summary>
		/// Get all the section names from an INI file
		/// </summary>
		[ DllImport("kernel32.dll", EntryPoint="GetPrivateProfileSectionNamesA") ]
		public extern static int GetPrivateProfileSectionNames(
			[MarshalAs(UnmanagedType.LPArray)] byte[] lpReturnedString,
			int nSize,
			string lpFileName);

		/// <summary>
		/// Get all the settings from a section in a INI file
		/// </summary>
		[ DllImport("kernel32.dll", EntryPoint="GetPrivateProfileSectionA") ]
		public extern static int GetPrivateProfileSection(
			string lpAppName,
			[MarshalAs(UnmanagedType.LPArray)] byte[] lpReturnedString,
			int nSize,
			string lpFileName);
	
	}
			
}// namespace FTI
