using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the information associated with an ORE (Objections Report Engine) template</summary>
	public class CTmaxORETemplate
	{
		#region Constants

		/// <summary>Names of option collections managed by the template</summary>
		public const string ORE_TEMPLATE_COLLECTION_COMMON		= "Common";
		public const string ORE_TEMPLATE_COLLECTION_CONSTANTS	= "Constants";
		public const string ORE_TEMPLATE_COLLECTION_FLAGS		= "Flags";
		public const string ORE_TEMPLATE_COLLECTION_RUNTIME		= "Runtime";
		public const string ORE_TEMPLATE_COLLECTION_COLORS		= "Colors";
		public const string ORE_TEMPLATE_COLLECTION_MARGINS		= "Margins";
		public const string ORE_TEMPLATE_COLLECTION_COLUMNS		= "Columns";
		public const string ORE_TEMPLATE_COLLECTION_MANAGER		= "TmaxManager";

		/// <summary>Predefined ORE option identifiers</summary>
		public const int ORE_COMMON_TITLE				= 0;
		public const int ORE_COMMON_SHORT_CASE_NAME		= 1;
		public const int ORE_COMMON_LONG_CASE_NAME		= 2;
		public const int ORE_CONSTANT_FORMAT			= 3;
		public const int ORE_CONSTANT_VISIBLE			= 4;
		public const int ORE_MANAGER_EXTENSION			= 5;
		public const int ORE_MANAGER_SINGLE_DEPOSITION	= 6;
		public const int ORE_MANAGER_PREFIX				= 7;
		public const int ORE_RUNTIME_XMLS_PATH			= 8;
		public const int ORE_RUNTIME_SAVE_AS			= 9;
		public const int ORE_RUNTIME_SELECTED_DEPONENT	= 10;
		public const int ORE_RUNTIME_PROGRESS_FILESPEC	= 11;
		public const int ORE_RUNTIME_DEFENDANT_COLOR	= 12;
		public const int ORE_RUNTIME_PLAINTIFF_COLOR	= 13;

		/// <summary>Names of properties contained in the Common collection</summary>
		public const string ORE_COMMON_NAME_TITLE = "title";
		public const string ORE_COMMON_NAME_SHORT_CASE_NAME = "shortCaseName";
		public const string ORE_COMMON_NAME_LONG_CASE_NAME = "longCaseName";

		/// <summary>Names of properties contained in the Constants collection</summary>
		public const string ORE_CONSTANT_NAME_FORMAT = "reportFormat";
		public const string ORE_CONSTANT_NAME_VISIBLE = "visible";

		/// <summary>Names of properties contained in the Manager collection</summary>
		public const string ORE_MANAGER_NAME_EXTENSION = "extension";
		public const string ORE_MANAGER_NAME_SINGLE_DEPOSITION = "singleDeposition";
		public const string ORE_MANAGER_NAME_PREFIX = "prefix";

		/// <summary>Names of properties contained in the Runtime collection</summary>
		public const string ORE_RUNTIME_NAME_XMLS_PATH = "xmlsPath";
		public const string ORE_RUNTIME_NAME_SAVE_AS = "saveAs";
		public const string ORE_RUNTIME_NAME_SELECTED_DEPONENT = "selectedDeponent";
		public const string ORE_RUNTIME_NAME_PROGRESS_FILESPEC = "progressFile";
		public const string ORE_RUNTIME_NAME_DEFENDANT_COLOR = "defColor";
		public const string ORE_RUNTIME_NAME_PLAINTIFF_COLOR = "pltfColor";

		/// <summary>Prefix for color descriptor names</summary>
		public const string ORE_COLOR_PREFIX = "DesigColor";

		/// <summary>Error identifiers</summary>
		private const int ERROR_LOAD_EX = 0;
		private const int ERROR_SAVE_EX = 1;

		#endregion Constants

		#region Private Members

		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Local member bound to Common property</summary>
		private CTmaxOREOptions m_tmaxCommon = new CTmaxOREOptions();

		/// <summary>Local member bound to Constants property</summary>
		private CTmaxOREOptions m_tmaxConstants = new CTmaxOREOptions();

		/// <summary>Local member bound to Runtime property</summary>
		private CTmaxOREOptions m_tmaxRuntime = new CTmaxOREOptions();

		/// <summary>Local member bound to Flags property</summary>
		private CTmaxOREOptions m_tmaxFlags = new CTmaxOREOptions();

		/// <summary>Local member bound to Colors property</summary>
		private CTmaxOREOptions m_tmaxColors = new CTmaxOREOptions();

		/// <summary>Local member bound to Margins property</summary>
		private CTmaxOREOptions m_tmaxMargins = new CTmaxOREOptions();

		/// <summary>Local member bound to Columns property</summary>
		private CTmaxOREOptions m_tmaxColumns = new CTmaxOREOptions();

		/// <summary>Local member bound to Manager property</summary>
		private CTmaxOREOptions m_tmaxManager = new CTmaxOREOptions();

		/// <summary>Name used to identify the template</summary>
		private string m_strName = "";

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxORETemplate()
		{
			Initialize("");
		}

		/// <summary>Constructor</summary>
		/// <param name="strName">Name used to identify the template</param>
		public CTmaxORETemplate(string strName)
		{
			Initialize(strName);
		}

		/// <summary>This method is called to get the appropriate extension for the specified report format</summary>
		///	<param name="strFormat">the desired report format</param>
		/// <returns>The associated file extension</returns>
		static public string GetOREExtension(string strFormat)
		{
			switch(strFormat)
			{
				case "Script-Excel": return "xls";
				case "Transcript-Excel": return "xls";
				case "Designation-Word": return "doc";
				default: return "xls";
			}

		}// public string GetOREExtension(string strFormat)

		/// <summary>This method is called to get the appropriate extension for this template</summary>
		/// <returns>The associated file extension</returns>
		public string GetOREExtension()
		{
			//	Use the format to determine the extension
			return GetOREExtension(GetValue(ORE_CONSTANT_FORMAT));
		}
		
		/// <summary>Called to clear the collections and reset the object properties</summary>
		public void Clear()
		{
			m_tmaxCommon.Clear();
			m_tmaxConstants.Clear();
			m_tmaxFlags.Clear();
			m_tmaxManager.Clear();
			m_tmaxColors.Clear();
			m_tmaxMargins.Clear();
			m_tmaxColumns.Clear();
			
			//	Reset the runtime values
			foreach(CTmaxOREOption O in m_tmaxRuntime)
				O.Value = "";
				
			m_strFileSpec = "";

		}// public void Clear()

		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="strFileSpec">The path to the configuration file</param>
		public bool Load(string strFileSpec)
		{
			CXmlIni xmlIni = null;
			bool	bSuccessful = false;
			
			try
			{
				xmlIni = new CXmlIni();
				
				//	Open the file to retrieve the values
				if(xmlIni.Open(strFileSpec) == true)
				{
					//	Save the path to the file
					m_strFileSpec = xmlIni.FileSpec;

					//	Load the options stored in the file
					Load(m_tmaxCommon, xmlIni);
					Load(m_tmaxConstants, xmlIni);
					Load(m_tmaxManager, xmlIni);
					Load(m_tmaxFlags, xmlIni);
					Load(m_tmaxMargins, xmlIni);
					Load(m_tmaxColumns, xmlIni);
					
					bSuccessful = true;

				}// if(xmlIni.Open(strFileSpec) == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Load", m_tmaxErrorBuilder.Message(ERROR_LOAD_EX, strFileSpec), Ex);
			}
			finally
			{
				if(xmlIni != null)
				{
					xmlIni.Close();
					xmlIni = null;
				}

			}

			return bSuccessful;

		}// public bool Load(string strFileSpec)

		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="strFileSpec">The path to the configuration file</param>
		public bool Save(string strFileSpec)
		{
			CXmlIni xmlIni = null;
			bool	bSuccessful = false;

			try
			{
				xmlIni = new CXmlIni();

				//	Should we use our previously assigned file path?
				if((strFileSpec == null) || (strFileSpec.Length == 0))
					strFileSpec = m_strFileSpec;
				if(strFileSpec.Length == 0) return false;

				//	Save the file
				if(xmlIni.Open(strFileSpec) == true)
				{
					//	Save the path to the file
					m_strFileSpec = xmlIni.FileSpec;

					//	Store the options in the file
					Save(m_tmaxCommon, xmlIni);
					Save(m_tmaxConstants, xmlIni);
					Save(m_tmaxManager, xmlIni);
					Save(m_tmaxFlags, xmlIni);
					Save(m_tmaxMargins, xmlIni);
					Save(m_tmaxColumns, xmlIni);

					//	Save the file
					bSuccessful = xmlIni.Save();

				}// if(xmlIni.Open(strFileSpec) == true)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_EX, strFileSpec), Ex);
			}
			finally
			{
				if(xmlIni != null)
				{
					xmlIni.Close();
					xmlIni = null;
				}
				
			}

			return bSuccessful;

		}// public bool Save(string strFileSpec)

		/// <summary>This method is called to determine if the specified Common option is enabled</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <returns>True if enabled</returns>
		public string GetName(int iOption)
		{
			switch(iOption)
			{
				case ORE_COMMON_TITLE:					return ORE_COMMON_NAME_TITLE;
				case ORE_COMMON_SHORT_CASE_NAME:		return ORE_COMMON_NAME_SHORT_CASE_NAME;
				case ORE_COMMON_LONG_CASE_NAME:			return ORE_COMMON_NAME_LONG_CASE_NAME;
				case ORE_CONSTANT_FORMAT:				return ORE_CONSTANT_NAME_FORMAT;
				case ORE_CONSTANT_VISIBLE:				return ORE_CONSTANT_NAME_VISIBLE;
				case ORE_MANAGER_EXTENSION:				return ORE_MANAGER_NAME_EXTENSION;
				case ORE_MANAGER_SINGLE_DEPOSITION:		return ORE_MANAGER_NAME_SINGLE_DEPOSITION;
				case ORE_MANAGER_PREFIX:				return ORE_MANAGER_NAME_PREFIX;
				case ORE_RUNTIME_XMLS_PATH:				return ORE_RUNTIME_NAME_XMLS_PATH;
				case ORE_RUNTIME_SAVE_AS:				return ORE_RUNTIME_NAME_SAVE_AS;
				case ORE_RUNTIME_SELECTED_DEPONENT:		return ORE_RUNTIME_NAME_SELECTED_DEPONENT;
				case ORE_RUNTIME_PROGRESS_FILESPEC:		return ORE_RUNTIME_NAME_PROGRESS_FILESPEC;
				case ORE_RUNTIME_DEFENDANT_COLOR:		return ORE_RUNTIME_NAME_DEFENDANT_COLOR;
				case ORE_RUNTIME_PLAINTIFF_COLOR:		return ORE_RUNTIME_NAME_PLAINTIFF_COLOR;
				default:		
				
					Debug.Assert(false, "Unhandled option identifier: " + iOption.ToString());
					return ("Unknown" + iOption.ToString());

			}// switch(iOption)
	
		}// public string GetName(int iOption)

		/// <summary>Called to get the collection where the specified option is stored</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <returns>The owner collection</returns>
		public CTmaxOREOptions GetCollection(int iOption)
		{
			switch(iOption)
			{
				case ORE_COMMON_TITLE:
				case ORE_COMMON_SHORT_CASE_NAME:
				case ORE_COMMON_LONG_CASE_NAME: 
					return m_tmaxCommon;
					
				case ORE_CONSTANT_FORMAT:
				case ORE_CONSTANT_VISIBLE: 
					return m_tmaxConstants;
					
				case ORE_MANAGER_EXTENSION:
				case ORE_MANAGER_SINGLE_DEPOSITION:
				case ORE_MANAGER_PREFIX:
					return m_tmaxManager;
					
				case ORE_RUNTIME_XMLS_PATH:
				case ORE_RUNTIME_SAVE_AS:
				case ORE_RUNTIME_SELECTED_DEPONENT:
				case ORE_RUNTIME_PROGRESS_FILESPEC:
				case ORE_RUNTIME_DEFENDANT_COLOR:
				case ORE_RUNTIME_PLAINTIFF_COLOR:
					return m_tmaxRuntime;
					
				default:

					Debug.Assert(false, "Unhandled option identifier: " + iOption.ToString());
					return null;

			}// switch(iOption)

		}// public CTmaxOREOptions GetCollection(int iOption)

		/// <summary>Called to get the ORE label for the specified option</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <returns>The ORELabel for the specified option</returns>
		public string GetORELabel(int iOption)
		{
			return GetName(iOption);

		}// public string GetORELabel(int iOption)

		/// <summary>This method is called to get the option with the specified name</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <param name="bAdd">True if OK to add the option to the collection if not found</param>
		/// <returns>The option if found</returns>
		public CTmaxOREOption Find(CTmaxOREOptions tmaxOREOptions, string strName, bool bAdd)
		{
			CTmaxOREOption tmaxOREOption = null;

			if(tmaxOREOptions != null)
			{
				if((tmaxOREOption = tmaxOREOptions.Find(strName, false)) == null)
				{
					if(bAdd == true)
						tmaxOREOption = tmaxOREOptions.Add(strName);
				}

			}// if(tmaxOREOptions != null)

			return tmaxOREOption;

		}// public CTmaxOREOption Find(CTmaxOREOptions tmaxOREOptions, string strName, bool bAdd)

		/// <summary>This method is called to get the option with the specified name</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <returns>The option if found</returns>
		public CTmaxOREOption Find(CTmaxOREOptions tmaxOREOptions, string strName)
		{
			return Find(tmaxOREOptions, strName, false);
		}

		/// <summary>This method is called to get the option with the specified name</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <param name="bAdd">True if OK to add the option to the collection if not found</param>
		/// <returns>The option if found</returns>
		public CTmaxOREOption Find(int iOption, bool bAdd)
		{
			CTmaxOREOptions	tmaxCollection = GetCollection(iOption);
			
			if(tmaxCollection != null)
				return Find(tmaxCollection, GetName(iOption), bAdd);
			else
				return null;

		}// public CTmaxOREOption Find(int iOption, bool bAdd)

		/// <summary>This method is called to get the option with the specified name</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <returns>The option if found</returns>
		public CTmaxOREOption Find(int iOption)
		{
			return Find(iOption, false);
		}

		/// <summary>This method is called to read the value of an option stored in the specified collection</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <param name="strDefault">Value to be returned if option not found</param>
		/// <returns>The value of the requested option</returns>
		public string GetValue(CTmaxOREOptions tmaxOREOptions, string strName, string strDefault)
		{
			CTmaxOREOption tmaxOREOption = null;

			if((tmaxOREOption = Find(tmaxOREOptions, strName, false)) != null)
			{
				return tmaxOREOption.Value;
			}
			else
			{
				return (strDefault != null ? strDefault : ""); 
			}

		}// public string GetValue(CTmaxOREOptions tmaxOREOptions, string strName, string strDefault)

		/// <summary>This method is called to read the value of an option stored in the specified collection</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <returns>The value of the requested option</returns>
		public string GetValue(CTmaxOREOptions tmaxOREOptions, string strName)
		{
			return GetValue(tmaxOREOptions, strName);
		}

		/// <summary>This method is called to read the value of the specified option</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <param name="strDefault">Value to be returned if option not found</param>
		/// <returns>The value of the requested option</returns>
		public string GetValue(int iOption, string strDefault)
		{
			CTmaxOREOptions	tmaxCollection = GetCollection(iOption);
			
			if(tmaxCollection != null)
			{
				return GetValue(tmaxCollection, GetName(iOption), strDefault);
			}
			else
			{
				return (strDefault != null ? strDefault : "");
			}

		}// public string GetValue(int iOption, string strDefault)

		/// <summary>This method is called to read the value of the specified option</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <returns>The value of the requested option</returns>
		public string GetValue(int iOption)
		{
			return GetValue(iOption, "");
		}

		/// <summary>This method is called to write the value of a property stored in the specified collection</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <param name="oValue">The value to be written to the option</param>
		/// <param name="bAdd">True if OK to add the option to the collection if not found</param>
		/// <returns>true if successful</returns>
		public bool SetValue(CTmaxOREOptions tmaxOREOptions, string strName, object oValue, bool bAdd)
		{
			CTmaxOREOption tmaxOREOption = null;

			if((tmaxOREOption = Find(tmaxOREOptions, strName, bAdd)) != null)
				tmaxOREOption.Value = (oValue != null ? oValue.ToString() : "");

			return (tmaxOREOption != null);

		}// public bool SetValue(CTmaxOREOptions tmaxOREOptions, string strName, object oValue, bool bAdd)

		/// <summary>This method is called to write the value of a property stored in the specified collection</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <param name="oValue">The value to be written to the option</param>
		/// <returns>true if successful</returns>
		public bool SetValue(CTmaxOREOptions tmaxOREOptions, string strName, object oValue)
		{
			return SetValue(tmaxOREOptions, strName, oValue, true);
		}
		
		/// <summary>This method is called to write the value of a property stored in the specified collection</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <param name="oValue">The value to be written to the option</param>
		/// <param name="bAdd">True if OK to add the option to the collection if not found</param>
		/// <returns>true if successful</returns>
		public bool SetValue(int iOption, object oValue, bool bAdd)
		{
			CTmaxOREOption tmaxOREOption = null;

			if((tmaxOREOption = Find(iOption, bAdd)) != null)
				tmaxOREOption.Value = (oValue != null ? oValue.ToString() : "");

			return (tmaxOREOption != null);

		}// public bool SetValue(int iOption, object oValue, bool bAdd)

		/// <summary>This method is called to write the value of a property stored in the specified collection</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <param name="oValue">The value to be written to the option</param>
		/// <returns>true if successful</returns>
		public bool SetValue(int iOption, object oValue)
		{
			return SetValue(iOption, oValue, true);
		}

		/// <summary>This method is called to determine if the specified option is enabled</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <param name="bDefault">Return value if option does not exist in the collection</param>
		/// <returns>True if enabled</returns>
		public bool GetEnabled(CTmaxOREOptions tmaxOREOptions, string strName, bool bDefault)
		{
			CTmaxOREOption tmaxOREOption = null;

			if((tmaxOREOption = Find(tmaxOREOptions, strName, false)) != null)
			{
				return tmaxOREOption.Enabled;
			}
			else
			{
				return bDefault;
			}

		}// public bool GetEnabled(CTmaxOREOptions tmaxOREOptions, string strName, bool bDefault)

		/// <summary>This method is called to determine if the specified option is enabled</summary>
		/// <param name="tmaxOREOptions">The collection of ORE options</param>
		/// <param name="strName">Name of the desired option</param>
		/// <returns>True if enabled</returns>
		public bool GetEnabled(CTmaxOREOptions tmaxOREOptions, string strName)
		{
			return GetEnabled(tmaxOREOptions, strName, true);
		}
		
		/// <summary>This method is called to determine if the specified option is enabled</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <param name="bDefault">Return value if option does not exist in the collection</param>
		/// <returns>True if enabled</returns>
		public bool GetEnabled(int iOption, bool bDefault)
		{
			CTmaxOREOption tmaxOREOption = null;

			if((tmaxOREOption = Find(iOption, false)) != null)
			{
				return tmaxOREOption.Enabled;
			}
			else
			{
				return bDefault;
			}

		}// public bool GetEnabled(int iOption, bool bDefault)

		/// <summary>This method is called to determine if the specified option is enabled</summary>
		/// <param name="iOption">The identifier for the desired option</param>
		/// <returns>True if enabled</returns>
		public bool GetEnabled(int iOption)
		{
			return GetEnabled(iOption, true);
		}
		
		/// <summary>Called to set the name assigned to the collection</summary>
		/// <param name="strName">The name to be assigned to the collection</param>
		public void SetName(string strName)
		{
			m_strName = strName;
			m_tmaxEventSource.Name = ("ORE " + m_strName + " Template");
		}

		/// <summary>Called to convert the system color to an ORE string</summary>
		/// <param name="sysColor">The color to be converted to a string</param>
		/// <returns>The string equivalent of the specified color</returns>
		static public string ToString(System.Drawing.Color sysColor)
		{
			return String.Format("{0},{1},{2}", sysColor.R, sysColor.G, sysColor.B);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>Called to load the options for the specified collection</summary>
		/// <param name="tmaxOREOptions">The collection to be populated</param>
		/// <param name="xmlIni">The file containing the values</param>
		private void Load(CTmaxOREOptions tmaxOREOptions, CXmlIni xmlIni)
		{
			//	Set the section and retrieve the values
			xmlIni.SetSection(tmaxOREOptions.Name, true, false);
			tmaxOREOptions.Load(xmlIni);

		}// private bool Load(CTmaxOREOptions tmaxOREOptions, CXmlIni xmlIni)

		/// <summary>Called to store the options for the specified collection</summary>
		/// <param name="tmaxOREOptions">The collection to be saved</param>
		/// <param name="xmlIni">The file containing the values</param>
		private void Save(CTmaxOREOptions tmaxOREOptions, CXmlIni xmlIni)
		{
			//	Set the section and retrieve the values
			//
			//	NOTE: We flush the section before saving the options
			xmlIni.SetSection(tmaxOREOptions.Name, true, true);
			tmaxOREOptions.Save(xmlIni);

		}// private bool Save(CTmaxOREOptions tmaxOREOptions, CXmlIni xmlIni)

		/// <summary>Called to initialize the object and its collections</summary>
		/// <param name="strName">The name to be assigned to the template</param>
		private void Initialize(string strName)
		{
			//	Set the name if provided
			if((strName != null) && (strName.Length > 0))
				SetName(strName);
				
			//	Set the collection names
			m_tmaxCommon.Name = ORE_TEMPLATE_COLLECTION_COMMON;
			m_tmaxConstants.Name = ORE_TEMPLATE_COLLECTION_CONSTANTS;
			m_tmaxFlags.Name = ORE_TEMPLATE_COLLECTION_FLAGS;
			m_tmaxRuntime.Name = ORE_TEMPLATE_COLLECTION_RUNTIME;
			m_tmaxColors.Name = ORE_TEMPLATE_COLLECTION_COLORS;
			m_tmaxMargins.Name = ORE_TEMPLATE_COLLECTION_MARGINS;
			m_tmaxColumns.Name = ORE_TEMPLATE_COLLECTION_COLUMNS;
			m_tmaxManager.Name = ORE_TEMPLATE_COLLECTION_MANAGER;
				
			//	Propagate the collection events
			m_tmaxEventSource.Attach(m_tmaxCommon.EventSource);
			m_tmaxEventSource.Attach(m_tmaxConstants.EventSource);
			m_tmaxEventSource.Attach(m_tmaxFlags.EventSource);
			m_tmaxEventSource.Attach(m_tmaxRuntime.EventSource);
			m_tmaxEventSource.Attach(m_tmaxColors.EventSource);
			m_tmaxEventSource.Attach(m_tmaxMargins.EventSource);
			m_tmaxEventSource.Attach(m_tmaxColumns.EventSource);
			m_tmaxEventSource.Attach(m_tmaxManager.EventSource);
			
			//	Populate the runtime collection
			m_tmaxRuntime.Add(ORE_RUNTIME_NAME_XMLS_PATH);
			m_tmaxRuntime.Add(ORE_RUNTIME_NAME_SAVE_AS);
			m_tmaxRuntime.Add(ORE_RUNTIME_NAME_SELECTED_DEPONENT);
			m_tmaxRuntime.Add(ORE_RUNTIME_NAME_PROGRESS_FILESPEC);
			m_tmaxRuntime.Add(ORE_RUNTIME_NAME_DEFENDANT_COLOR);
			m_tmaxRuntime.Add(ORE_RUNTIME_NAME_PLAINTIFF_COLOR);

			//	Populate the error builder
			SetErrorStrings();

		}// private void Initialize(string strName)

		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;

			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;

			if(aStrings == null) return;

			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to load the template: filename = %1");
			aStrings.Add("An exception was raised while attempting to save the template: filename = %1");
		}
		
		#endregion Private Methods
		
		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>Name assigned to the template</summary>
		public string Name
		{
			get { return m_strName; }
			set { SetName(value); }
		}

		/// <summary>The collection of options common to all report formats</summary>
		public CTmaxOREOptions Common
		{
			get { return m_tmaxCommon; }
		}

		/// <summary>The collection of boolean options defined in the template file</summary>
		public CTmaxOREOptions Flags
		{
			get { return m_tmaxFlags; }
		}

		/// <summary>The collection of constants defined in the template file</summary>
		public CTmaxOREOptions Constants
		{
			get { return m_tmaxConstants; }
		}

		/// <summary>The collection of Runtime options constructed by the application</summary>
		public CTmaxOREOptions Runtime
		{
			get { return m_tmaxRuntime; }
		}

		/// <summary>The collection of Colors stored in the active database</summary>
		public CTmaxOREOptions Colors
		{
			get { return m_tmaxColors; }
		}

		/// <summary>The collection of Margins stored in the active database</summary>
		public CTmaxOREOptions Margins
		{
			get { return m_tmaxMargins; }
		}

		/// <summary>The collection of Columns stored in the active database</summary>
		public CTmaxOREOptions Columns
		{
			get { return m_tmaxColumns; }
		}

		/// <summary>The collection of options used by TmaxManager</summary>
		public CTmaxOREOptions Manager
		{
			get { return m_tmaxManager; }
		}

		/// <summary>The fully qualified path to the template configuration file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		}

		#endregion Properties

	}// public class CTmaxORETemplate

	/// <summary>This class is used to manage a dynamic array of CTmaxORETemplate objects</summary>
	public class CTmaxORETemplates : System.Collections.ArrayList
	{
		#region Private Members

		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxORETemplates() : base()
		{
		}

		/// <summary>Adds a new option to the collection</summary>
		/// <param name="tmaxORETemplate">The object to be added</param>
		public CTmaxORETemplate Add(CTmaxORETemplate tmaxORETemplate)
		{
			try
			{
				Debug.Assert((tmaxORETemplate != null), "Attempt to add NULL ORE template");

				m_tmaxEventSource.Attach(tmaxORETemplate.EventSource);

				this.Add(tmaxORETemplate as object);

				return tmaxORETemplate;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return null;
			}

		}// public CTmaxORETemplate Add(CTmaxORETemplate tmaxORETemplate)

		/// <summary>Called to find the template with the specified Name</summary>
		/// <param name="strName">The name of the desired template</param>
		/// <returns>The template with the specified name if found</returns>
		public CTmaxORETemplate Find(string strName)
		{
			foreach(CTmaxORETemplate O in this)
			{
				if(String.Compare(O.Name, strName, true) == 0)
					return O;
			}

			return null;

		}// public CTmaxORETemplate Find(string strName)

		/// <summary>Called to remove the specified object from the collection</summary>
		/// <param name="O">The object to be removed</param>
		public void Remove(CTmaxORETemplate O)
		{
			try { base.Remove(O as object); }
			catch {}
		}

		/// <summary>Called to determine if the specified object exists in the collection</summary>
		/// <param name="O">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxORETemplate O)
		{
			return base.Contains(O as object);
		}

		/// <summary>Overloaded [] operator to locate the object with the specified name</summary>
		/// <param name="O">The name of the desired object</param>
		/// <returns>The object with the specified name</returns>
		public CTmaxORETemplate this[string strName]
		{
			get { return Find(strName); }
		}

		/// <summary>Overloaded version of [] operator to return the object at the specified index</summary>
		/// <param name="index">The index of the desired object</param>
		/// <returns>The object at the specified index</returns>
		new public CTmaxORETemplate this[int index]
		{
			get { return (base[index] as CTmaxORETemplate); }
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="O">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxORETemplate O)
		{
			return base.IndexOf(O as object);
		}

		#endregion Public Methods

		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		#endregion Properties

	}//	public class CTmaxORETemplates

}// namespace FTI.Shared.Trialmax
