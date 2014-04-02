using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the import wizard options</summary>
	public class CTmaxImportWizard
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME = "ImportWizard";
		
		private const string XMLINI_KEY_LAST_USED							= "LastUsed";
		private const string XMLINI_KEY_SOURCE_FILESPEC						= "Source";
		private const string XMLINI_KEY_CROSS_REFERENCE_FILESPEC			= "CrossReference";
		private const string XMLINI_KEY_ALTERNATE_FILESPEC					= "Alternate";
		private const string XMLINI_KEY_ROOT_FOLDER							= "RootFolder";
		private const string XMLINI_KEY_RENAME_FILES						= "RenameFiles";
		private const string XMLINI_KEY_MOVE_FILES							= "MoveFiles";
		private const string XMLINI_KEY_MAX_DOCS_PER_FOLDER					= "MaxDocsPerFolder";
		private const string XMLINI_KEY_DEVELOPMENT							= "Development";
		
		private const string XMLINI_CONVERTER_ATTRIBUTE_NAME				= "Name";
		private const string XMLINI_CONVERTER_ATTRIBUTE_CONFIGURATION_FILE	= "ConfigurationFile";
		private const string XMLINI_CONVERTER_ATTRIBUTE_USE_CROSS_REFERENCE	= "UseCrossReference";
		private const string XMLINI_CONVERTER_ATTRIBUTE_EXTENSIONS			= "Extensions";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to LastUsed property</summary>
		private string m_strLastUsed = "";
		
		/// <summary>Local member bound to CrossReferenceFileSpec property</summary>
		private string m_strCrossReferenceFileSpec = "";
		
		/// <summary>Local member bound to SourceFileSpec property</summary>
		private string m_strSourceFileSpec = "";
		
		/// <summary>Local member bound to RootFolder property</summary>
		private string m_strRootFolder = "";
		
		/// <summary>Local member bound to AlternateFileSpec property</summary>
		private string m_strAlternateFileSpec = "";
		
		/// <summary>Local member bound to MoveFiles property</summary>
		private bool m_bMoveFiles = false;
		
		/// <summary>Local member bound to RenameFiles property</summary>
		private bool m_bRenameFiles = true;
		
		/// <summary>Local member bound to Development property</summary>
		private bool m_bDevelopment = false;

		/// <summary>Local member bound to MaxDocsPerFolder property</summary>
		private long m_lMaxDocsPerFolder = 1000;

		/// <summary>Local member bound to Converters property</summary>
		private CTmaxLoadFileConverters m_tmaxConverters = new CTmaxLoadFileConverters();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxImportWizard()
		{
		}
		
		/// <summary>This method is called to initialize the object using values stored in the specified configuration file</summary>
		/// <param name="xmlIni">The initialization file containing the initialization information</param>
		public void Load(CXmlIni xmlIni)
		{
			int						iConverter = 1;
			string					strKey = "";
			string					strName = "";
			string					strConfigurationFile = "";
			string					strExtensions = "";
			bool					bUseCrossReference = false;
			CTmaxLoadFileConverter	tmaxConverter = null;
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_strLastUsed = xmlIni.Read(XMLINI_KEY_LAST_USED);
			m_strSourceFileSpec = xmlIni.Read(XMLINI_KEY_SOURCE_FILESPEC);
			m_strRootFolder = xmlIni.Read(XMLINI_KEY_ROOT_FOLDER);
			m_strCrossReferenceFileSpec = xmlIni.Read(XMLINI_KEY_CROSS_REFERENCE_FILESPEC);
			m_strAlternateFileSpec = xmlIni.Read(XMLINI_KEY_ALTERNATE_FILESPEC);
			m_lMaxDocsPerFolder = xmlIni.ReadLong(XMLINI_KEY_MAX_DOCS_PER_FOLDER, m_lMaxDocsPerFolder);
			m_bRenameFiles = xmlIni.ReadBool(XMLINI_KEY_RENAME_FILES, m_bRenameFiles);
			m_bMoveFiles = xmlIni.ReadBool(XMLINI_KEY_MOVE_FILES, m_bMoveFiles);
			m_bDevelopment = xmlIni.ReadBool(XMLINI_KEY_DEVELOPMENT, m_bDevelopment);
			
			//	Clear the existing converters
			m_tmaxConverters.Clear();
			
			//	Read the converter descriptors
			while(true)
			{
				//	Construct the key
				strKey = String.Format("Converter{0}", iConverter);
				iConverter++;
				
				//	Read the attributes stored in the file
				strName = xmlIni.Read(strKey, XMLINI_CONVERTER_ATTRIBUTE_NAME, "");
				strConfigurationFile = xmlIni.Read(strKey, XMLINI_CONVERTER_ATTRIBUTE_CONFIGURATION_FILE, "");
				bUseCrossReference = xmlIni.ReadBool(strKey, XMLINI_CONVERTER_ATTRIBUTE_USE_CROSS_REFERENCE, false);
				strExtensions = xmlIni.Read(strKey, XMLINI_CONVERTER_ATTRIBUTE_EXTENSIONS, "");
				
				if((strName.Length > 0) && (strConfigurationFile.Length > 0))
				{
					//	Allocate a new converter
					tmaxConverter = new CTmaxLoadFileConverter();
					
					//	Set the property values
					tmaxConverter.Name = strName;
					tmaxConverter.ConfigurationFilename = strConfigurationFile;
					tmaxConverter.UseCrossReference = bUseCrossReference;
					tmaxConverter.Extensions = strExtensions;
					
					//	Add to the collection
					m_tmaxConverters.Add(tmaxConverter);
				}
				else
				{
					break;
				}
				
			}// while(true)
			
			//	Do we need to put the defaults in the collection
			if(m_tmaxConverters.Count == 0)
				GetDefaultConverters();
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the wizard options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file in which to store the options</param>
		public void Save(CXmlIni xmlIni)
		{
			int		iConverter = 1;
			string	strKey = "";
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_KEY_LAST_USED, m_strLastUsed);
			xmlIni.Write(XMLINI_KEY_SOURCE_FILESPEC, m_strSourceFileSpec);
			xmlIni.Write(XMLINI_KEY_ROOT_FOLDER, m_strRootFolder);
			xmlIni.Write(XMLINI_KEY_CROSS_REFERENCE_FILESPEC, m_strCrossReferenceFileSpec);
			xmlIni.Write(XMLINI_KEY_ALTERNATE_FILESPEC, m_strAlternateFileSpec);
			xmlIni.Write(XMLINI_KEY_MAX_DOCS_PER_FOLDER, m_lMaxDocsPerFolder);
			xmlIni.Write(XMLINI_KEY_RENAME_FILES, m_bRenameFiles);
			xmlIni.Write(XMLINI_KEY_MOVE_FILES, m_bMoveFiles);
			xmlIni.Write(XMLINI_KEY_DEVELOPMENT, m_bDevelopment);
			
			//	Store each of the converters to file
			foreach(CTmaxLoadFileConverter O in m_tmaxConverters)
			{
				//	Construct the key
				strKey = String.Format("Converter{0}", iConverter);
				iConverter++;
				
				xmlIni.Write(strKey, XMLINI_CONVERTER_ATTRIBUTE_NAME, O.Name);
				xmlIni.Write(strKey, XMLINI_CONVERTER_ATTRIBUTE_CONFIGURATION_FILE, O.ConfigurationFilename);
				xmlIni.Write(strKey, XMLINI_CONVERTER_ATTRIBUTE_USE_CROSS_REFERENCE, O.UseCrossReference);
				xmlIni.Write(strKey, XMLINI_CONVERTER_ATTRIBUTE_EXTENSIONS, O.Extensions);

			}// foreach(CTmaxLoadFileConverter O in m_tmaxConverters)

		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>Adds the default load file converters to the collection</summary>
		private void GetDefaultConverters()
		{
			CTmaxLoadFileConverter tmaxConverter = null;
			
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "Summation DII";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_summation_dii.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "dii,sum";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "Summation SUM";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_summation_dat.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "dat,sum";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "Opticon";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_opticon.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "oll";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "IPRO";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_ipro.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "Ringtail";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_ringtail.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "Trial Director";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_trialdirector.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "Visionary";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_visionary.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "TrialMax 4.x";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_fti4x.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "fti";
			m_tmaxConverters.Add(tmaxConverter);
		
			tmaxConverter = new CTmaxLoadFileConverter();
			tmaxConverter.Name = "TrialMax 6.x";
			tmaxConverter.ConfigurationFilename = "_tmax_importwiz_fti6x.cfg";
			tmaxConverter.UseCrossReference = false;
			tmaxConverter.Extensions = "fti";
			m_tmaxConverters.Add(tmaxConverter);
		
		}// private void GetDefaultConverters()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The collection of known converters</summary>
		public CTmaxLoadFileConverters Converters
		{
			get { return m_tmaxConverters; }
		}
		
		/// <summary>Fully qualified path to the Source file used for the last operation</summary>
		public string SourceFileSpec
		{
			get { return m_strSourceFileSpec; }
			set { m_strSourceFileSpec = value; }
		}
		
		/// <summary>Fully qualified path to the root folder used for the last operation</summary>
		public string RootFolder
		{
			get { return m_strRootFolder; }
			set { m_strRootFolder = value; }
		}
		
		/// <summary>Fully qualified path to the Cross Reference file used for the last operation</summary>
		public string CrossReferenceFileSpec
		{
			get { return m_strCrossReferenceFileSpec; }
			set { m_strCrossReferenceFileSpec = value; }
		}
		
		/// <summary>Fully qualified path to the Cross Reference file used for the last operation</summary>
		public string AlternateFileSpec
		{
			get { return m_strAlternateFileSpec; }
			set { m_strAlternateFileSpec = value; }
		}
		
		/// <summary>Name of converter used in the last operation</summary>
		public string LastUsed
		{
			get { return m_strLastUsed; }
			set { m_strLastUsed = value; }
		}

		/// <summary>Maximum number of documents allowed per folder</summary>
		public long MaxDocsPerFolder
		{
			get { return m_lMaxDocsPerFolder; }
			set { m_lMaxDocsPerFolder = value; }
		}

		/// <summary>True to rename files to match page sequence</summary>
		public bool RenameFiles
		{
			get { return m_bRenameFiles; }
			set { m_bRenameFiles = value; }
		}
		
		/// <summary>True to move files (instead of copy) when transferred</summary>
		public bool MoveFiles
		{
			get { return m_bMoveFiles; }
			set { m_bMoveFiles = value; }
		}
		
		/// <summary>True to operate the wizard in development mode</summary>
		public bool Development
		{
			get { return m_bDevelopment; }
			set { m_bDevelopment = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxImportWizard

}// namespace FTI.Shared.Trialmax
