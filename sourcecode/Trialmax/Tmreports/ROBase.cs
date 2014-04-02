using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Reports
{
	/// <summary>This is the base class for all report options classes</summary>
	public class CROBase
	{
		#region Constants
		
		protected const string XMLINI_ALTERNATE_KEY					= "Alternate";
		protected const string XMLINI_TEMPLATE_KEY					= "Template";
		protected const string XMLINI_SAVE_DATA_KEY					= "SaveData";
		protected const string XMLINI_SHOW_SAVE_DATA_KEY			= "ShowSaveData";
		protected const string XMLINI_EXPORT_FORMAT_KEY				= "ExportFormat";
		protected const string XMLINI_PREVIEW_EXPORTS_KEY			= "PreviewExports";
		protected const string XMLINI_SPLIT_EXPORTS_KEY				= "SplitExports";
		protected const string XMLINI_EXPORT_FOLDER_KEY				= "ExportFolder";
		protected const string XMLINI_ADD_MEDIA_NAME_KEY			= "AddMediaName";
	
		protected const string XMLINI_TEMPLATE_FILENAME_ATTRIBUTE	= "Filename";
		protected const string XMLINI_TEMPLATE_SCHEMA_ATTRIBUTE		= "Schema";
		protected const string XMLINI_TEMPLATE_NAME_ATTRIBUTE		= "Name";
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bound to Templates property</summary>
		protected ArrayList m_aTemplates = new ArrayList();
		
		/// <summary>Local member bound to ExportFormat property</summary>
		protected TmaxExportReportFormats m_eExportFormat = TmaxExportReportFormats.None;
		
		/// <summary>Local member bound to Template property</summary>
		protected int m_iTemplate = 1;
		
		/// <summary>Local member bound to Alternate property</summary>
		protected string m_strAlternate = "";

		/// <summary>Local member bound to SaveData property</summary>
		protected bool m_bSaveData = false;

		/// <summary>Local member bound to ShowSaveData property</summary>
		protected bool m_bShowSaveData = false;

		/// <summary>Local member bound to PreviewExports property</summary>
		protected bool m_bPreviewExports = false;

		/// <summary>Local member bound to SplitExports property</summary>
		protected bool m_bSplitExports = true;

		/// <summary>Local member bound to AppendMediaName property</summary>
		protected bool m_bAddMediaName = false;

		/// <summary>Local member bound to ExportFolder property</summary>
		protected string m_strExportFolder = "";

		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CROBase()
		{
		
		}// CROBase()
		
		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public virtual void Load(CXmlIni xmlIni)
		{
			int		iTemplate = 1;
			string	strKey = "";
			string	strFilename = "";
			string	strSchema = "";
			string	strName = "";
			
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;
			
			m_strAlternate = xmlIni.Read(XMLINI_ALTERNATE_KEY, m_strAlternate);
			m_iTemplate = xmlIni.ReadInteger(XMLINI_TEMPLATE_KEY, m_iTemplate);
			m_bSaveData = xmlIni.ReadBool(XMLINI_SAVE_DATA_KEY, m_bSaveData);
			m_bShowSaveData = xmlIni.ReadBool(XMLINI_SHOW_SAVE_DATA_KEY, m_bShowSaveData);
			m_bPreviewExports = xmlIni.ReadBool(XMLINI_PREVIEW_EXPORTS_KEY, m_bPreviewExports);
			m_bSplitExports = xmlIni.ReadBool(XMLINI_SPLIT_EXPORTS_KEY, m_bSplitExports);
			m_bAddMediaName = xmlIni.ReadBool(XMLINI_ADD_MEDIA_NAME_KEY, m_bAddMediaName);
			m_eExportFormat = GetExportFormat(xmlIni.Read(XMLINI_EXPORT_FORMAT_KEY, m_eExportFormat));
			m_strExportFolder = xmlIni.Read(XMLINI_EXPORT_FOLDER_KEY, m_strExportFolder);
			
			//	Read the template descriptors
			while(true)
			{
				//	Construct the key
				strKey = String.Format("Template{0}", iTemplate);
				iTemplate++;
				
				//	Read the template attributes
				strFilename = xmlIni.Read(strKey, XMLINI_TEMPLATE_FILENAME_ATTRIBUTE, "");
				strSchema = xmlIni.Read(strKey, XMLINI_TEMPLATE_SCHEMA_ATTRIBUTE, GetDefaultXmlSchema());
				strName = xmlIni.Read(strKey, XMLINI_TEMPLATE_NAME_ATTRIBUTE, "");
				
				if((strFilename.Length > 0) && (strSchema.Length > 0))
				{
					m_aTemplates.Add(new CROTemplate(strFilename, strSchema, strName));
				}
				else
				{
					break;
				}
				
			}// while(true)
			
			//	Get the default template if none found in the file
			if(m_aTemplates.Count == 0)
				GetDefaultTemplates();
				
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public virtual void Save(CXmlIni xmlIni)
		{
			int		iTemplate = 1;
			string	strKey = "";
			
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;

			xmlIni.Write(XMLINI_ALTERNATE_KEY, m_strAlternate);
			xmlIni.Write(XMLINI_TEMPLATE_KEY, m_iTemplate);
			xmlIni.Write(XMLINI_SAVE_DATA_KEY, m_bSaveData);
			xmlIni.Write(XMLINI_SHOW_SAVE_DATA_KEY, m_bShowSaveData);
			xmlIni.Write(XMLINI_EXPORT_FORMAT_KEY, m_eExportFormat.ToString());
			xmlIni.Write(XMLINI_PREVIEW_EXPORTS_KEY, m_bPreviewExports);
			xmlIni.Write(XMLINI_SPLIT_EXPORTS_KEY, m_bSplitExports);
			xmlIni.Write(XMLINI_ADD_MEDIA_NAME_KEY, m_bAddMediaName);
			xmlIni.Write(XMLINI_EXPORT_FOLDER_KEY, m_strExportFolder);
			
			//	Store each of the template descriptors
			foreach(CROTemplate O in m_aTemplates)
			{
				//	Construct the key
				strKey = String.Format("Template{0}", iTemplate);
				iTemplate++;
				
				xmlIni.Write(strKey, XMLINI_TEMPLATE_FILENAME_ATTRIBUTE, O.Filename);
				xmlIni.Write(strKey, XMLINI_TEMPLATE_SCHEMA_ATTRIBUTE, O.XmlSchema);
				xmlIni.Write(strKey, XMLINI_TEMPLATE_NAME_ATTRIBUTE, O.Name);
			}

		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to get the XML section name</summary>
		/// <returns>The section containing all the option values</returns>
		/// <remarks>Derived classes should override this method to make sure they use a unique section name</remarks>
		public virtual string GetXmlSectionName()
		{
			return "BaseReportOptions";
		}
		
		/// <summary>This method is called to get name of the default XML schema file</summary>
		/// <returns>The default schema filename</returns>
		/// <remarks>Derived classes should override this method to make sure they provide the appropriate name</remarks>
		public virtual string GetDefaultXmlSchema()
		{
			return "";
		}
		
		/// <summary>This method is called to get the export format with the matching text identifier</summary>
		/// <param name="strFormat">The format's text descriptor</param>
		/// <returns>The equivalent enumerated value</returns>
		protected virtual TmaxExportReportFormats GetExportFormat(string strFormat)
		{
			foreach(TmaxExportReportFormats O in Enum.GetValues(typeof(TmaxExportReportFormats)))
			{
				if(String.Compare(O.ToString(), strFormat, true) == 0)
					return O;
				
			}// foreach(TmaxExportReportFormats O in Enum.GetValues(typeof(TmaxExportReportFormats)))
		
			return TmaxExportReportFormats.None; // Not found
			
		}// protected virtual TmaxExportReportFormats GetExportFormat(string strFormat)
		
		/// <summary>Called to get the display string for the specified export format</summary>
		/// <param name="eFormat">The desired export format</param>
		/// <returns>The string that identifies the specified format</returns>
		static public string GetDisplayString(TmaxExportReportFormats eFormat)
		{
			switch(eFormat)
			{
				case TmaxExportReportFormats.None:	return "None";
				case TmaxExportReportFormats.Adobe:	return "Adobe PostScript (pdf)";
				case TmaxExportReportFormats.Word:	return "Microsoft Word (doc)";
				case TmaxExportReportFormats.Excel:	return "Microsoft Excel (xls)";
				case TmaxExportReportFormats.HTML:	return "HTML (htm)";
				default:							return eFormat.ToString();
			}
			
		}// static public string GetDisplayString(TmaxExportReportFormats eFormat)
		
		/// <summary>Called to get the default file extension for the specified export format</summary>
		/// <param name="eFormat">The desired export format</param>
		/// <returns>The file extension for the specified type</returns>
		static public string GetExtension(TmaxExportReportFormats eFormat)
		{
			switch(eFormat)
			{
				case TmaxExportReportFormats.None:	return "";
				case TmaxExportReportFormats.Adobe:	return "pdf";
				case TmaxExportReportFormats.Word:	return "doc";
				case TmaxExportReportFormats.Excel:	return "xls";
				case TmaxExportReportFormats.HTML:	return "htm";
				default:							
					Debug.Assert(false, "Unknown export format: " + eFormat.ToString());
					return "";
			}
			
		}// static public string GetExtension(TmaxExportReportFormats eFormat)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the templates collection with the defaults for the report</summary>
		protected virtual void GetDefaultTemplates()
		{
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Array of templates associated with this report</summary>
		public ArrayList Templates
		{
			get { return m_aTemplates; }
		}
		
		/// <summary>User specified alternate report template</summary>
		public string Alternate
		{
			get { return m_strAlternate; }
			set { m_strAlternate = value; }
		}
		
		/// <summary>Id of the last template used to generate a report</summary>
		public int Template
		{
			get { return m_iTemplate; }
			set { m_iTemplate = value; }
		}
		
		/// <summary>True to save the report data to file</summary>
		public bool SaveData
		{
			get { return m_bSaveData; }
			set { m_bSaveData = value; }
		}
		
		/// <summary>True to show the save data check box</summary>
		public bool ShowSaveData
		{
			get { return m_bShowSaveData; }
			set { m_bShowSaveData = value; }
		}
		
		/// <summary>True to add Media Name to export filename</summary>
		public bool AddMediaName
		{
			get { return m_bAddMediaName; }
			set { m_bAddMediaName = value; }
		}
		
		/// <summary>Format used to export the report to file</summary>
		public TmaxExportReportFormats ExportFormat
		{
			get { return m_eExportFormat; }
			set { m_eExportFormat = value; }
		}
		
		/// <summary>Folder where exported reports should be stored</summary>
		public string ExportFolder
		{
			get { return m_strExportFolder; }
			set { m_strExportFolder = value; }
		}
		
		/// <summary>True to preview the report before exporting</summary>
		public bool PreviewExports
		{
			get { return m_bPreviewExports; }
			set { m_bPreviewExports = value; }
		}
		
		/// <summary>True to merge exports into one file</summary>
		public bool SplitExports
		{
			get { return m_bSplitExports; }
			set { m_bSplitExports = value; }
		}
		
		#endregion Properties

	}//	public class CROBase

	/// <summary>This class encapsulates the information to define a template for an Exhibits report</summary>
	public class CROTemplate
	{
		#region Private Members
		
		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";
		
		/// <summary>Local member bound to XmlSchema property</summary>
		private string m_strXmlSchema = "";
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CROTemplate()
		{
		
		}// CROTemplate()
		
		/// <summary>Constructor</summary>
		/// <param name="strFilename">Filename of the report definition file</param>
		/// <param name="strXmlSchema">Name of the XML file containing the report data set schema</param>
		/// <param name="strName">Name used to display the template</param>
		public CROTemplate(string strFilename, string strXmlSchema, string strName)
		{
			SetProperties(strFilename, strXmlSchema, strName);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="strFilename">Filename of the report definition file</param>
		/// <param name="strXmlSchema">Name of the XML file containing the report data set schema</param>
		public CROTemplate(string strFilename, string strXmlSchema)
		{
			SetProperties(strFilename, strXmlSchema, "");
		}
		
		/// <summary>Overridded base class member to convert the object to a string</summary>
		/// <returns>The string representation of this object</returns>
		public override string ToString()
		{
			if(m_strName.Length > 0)
				return m_strName;
			else if(m_strFilename.Length > 0)
				return m_strFilename;
			else if(m_strXmlSchema.Length > 0)
				return m_strXmlSchema;
			else
				return base.ToString();
		
		}// public override string ToString()

		/// <summary>Called to set the object properties</summary>
		/// <param name="strFilename">Filename of the report definition file</param>
		/// <param name="strXmlSchema">Name of the XML file containing the report data set schema</param>
		/// <param name="strName">Name used to display the template</param>
		public void SetProperties(string strFilename, string strXmlSchema, string strName)
		{
			if((strFilename != null) && (strFilename.Length > 0))
				m_strFilename = strFilename;
			
			if((strXmlSchema != null) && (strXmlSchema.Length > 0))
				m_strXmlSchema = strXmlSchema;
			
			if((strName != null) && (strName.Length > 0))
				m_strName = strName;

		}// public void SetProperties(string strFilename, string strXmlSchema, string strName)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Name of the report template file</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
		}
		
		/// <summary>Name of the XML schema file</summary>
		public string XmlSchema
		{
			get { return m_strXmlSchema; }
			set { m_strXmlSchema = value; }
		}
		
		/// <summary>Name used to display the template</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}

		#endregion Properties

	}//	public class CROTemplate

}// namespace FTI.Trialmax.Reports
