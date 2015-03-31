using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class encapsulates the options used to register media in a Trialmax database</summary>
	public class CTmaxRegOptions
	{
		#region Constants
		
		private string XMLINI_SECTION_NAME						= "RegistrationOptions";
		private string XMLINI_FILE_TRANSFER_KEY					= "FileTransfer";
		private string XMLINI_MEDIA_CREATION_KEY				= "MediaCreation";
		private string XMLINI_MORPH_METHOD_KEY					= "MorphMethod";
		private string XMLINI_CONFLICT_RESOLUTION_KEY			= "ConflictResolution";
		private string XMLINI_FLAGS_KEY							= "Flags";
		private string XMLINI_FOLDER_ASSIGNMENTS_KEY			= "FolderAssignments";
		private string XMLINI_MORPH_METHOD_TEXT_KEY				= "MorphMethodText";
		private string XMLINI_CONFLICT_RESOLUTION_TEXT_KEY		= "ConflictResolutionText";
		private string XMLINI_CONFLICT_AUTO_PREFIX_KEY			= "ConflictAutoPrefix";
		private string XMLINI_ALLOW_REGISTER_LINKED_KEY			= "AllowRegisterLinked";
		private string XMLINI_MEDIA_ID_ADJUSTMENTS_KEY			= "MediaIDAdjustments";
		private string XMLINI_FOREIGN_BARCODE_ADJUSTMENTS_KEY	= "ForeignBarcodeAdjustments";
		private string XMLINI_USE_SHERROD_SPLITTER_KEY			= "UseSherrodSplitter";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to MediaCreations property</summary>
		private CTmaxOptions m_aMediaCreations = new CTmaxOptions();
		
		/// <summary>Local member bound to FileTransfers property</summary>
		private CTmaxOptions m_aFileTransfers = new CTmaxOptions();
		
		/// <summary>Local member bound to MorphMethods property</summary>
		private CTmaxOptions m_aMorphMethods = new CTmaxOptions();
		
		/// <summary>Local member bound to MorphMethodText property</summary>
		private string m_strMorphMethodText = "";
		
		/// <summary>Local member bound to FolderAssignments property</summary>
		private CTmaxOptions m_aFolderAssignments = new CTmaxOptions();
		
		/// <summary>Local member bound to ConflictResolutions property</summary>
		private CTmaxOptions m_aConflictResolutions = new CTmaxOptions();
		
		/// <summary>Local member bound to MediaIdAdjustments property</summary>
		private CTmaxOptions m_aMediaIdAdjustments = new CTmaxOptions();
		
		/// <summary>Local member bound to ForeignBarcodeAdjustments property</summary>
		private CTmaxOptions m_aForeignBarcodeAdjustments = new CTmaxOptions();
		
		/// <summary>Local member bound to ConflictResolutions property</summary>
		private CTmaxOptions m_aFlags = new CTmaxOptions();
		
		/// <summary>Local member bound to ConflictResolutionText property</summary>
		private string m_strConflictResolutionText = "";
		
		/// <summary>Local member bound to ConflictAutoPrefix property</summary>
		private string m_strConflictAutoPrefix = "_Auto";
		
		/// <summary>Local member bound to AllowRegisterLinked property</summary>
		private bool m_bAllowRegisterLinked = false;
		
		/// <summary>Local member bound to UseSherrodSplitter property</summary>
		private bool m_bUseSherrodSplitter = true;

        /// <summary>Local member bound to Output Type property</summary>
        private TmaxPDFOutputType m_bOutputType = TmaxPDFOutputType.Autodetect;

        /// <summary>Local member bound to Custom DPI property</summary>
        private short m_bCustomDPI = 0;

        /// <summary>Local member bound to Use Custom DPI property</summary>
        private bool m_bUseCustomDPI = false;

        /// <summary>Local member bound to Disable Custom Dither property</summary>
        private bool m_bDisableCustomDither = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxRegOptions()
		{
			m_aMediaCreations.Add(RegMediaCreations.OnePerFolder, "One media per folder", true);
			m_aMediaCreations.Add(RegMediaCreations.Split, "One media per file");
			m_aMediaCreations.Add(RegMediaCreations.Merge, "Merge files into one");

			m_aFileTransfers.Add(RegFileTransfers.Copy, "Copy To Case Folder", true);
			m_aFileTransfers.Add(RegFileTransfers.Move, "Move To Case Folder");

			m_aMorphMethods.Add(RegMorphMethods.None, "None", true);
			m_aMorphMethods.Add(RegMorphMethods.Offset, "Offset");
			m_aMorphMethods.Add(RegMorphMethods.Prefix, "Prefix");
			m_aMorphMethods.Add(RegMorphMethods.Suffix, "Suffix");

			m_aFolderAssignments.Multiple = true;
			m_aFolderAssignments.Add(RegFolderAssignments.MediaId, "Media Id", true);
			m_aFolderAssignments.Add(RegFolderAssignments.MediaName, "Media Name");
//			m_aFolderAssignments.Add(RegFolderAssignments.Exhibit, "Exhibit");
			m_aFolderAssignments.Add(RegFolderAssignments.Description, "Description");

			m_aConflictResolutions.Add(RegConflictResolutions.Prompt, "Prompt");
			m_aConflictResolutions.Add(RegConflictResolutions.Prefix, "Prefix");
			m_aConflictResolutions.Add(RegConflictResolutions.Suffix, "Suffix");
			m_aConflictResolutions.Add(RegConflictResolutions.Automatic, "Automatic", true);
			 
			m_aMediaIdAdjustments.Add(RegMediaIdAdjustments.TruncateOnSpace, "Truncate on space", true);
			m_aMediaIdAdjustments.Add(RegMediaIdAdjustments.TruncateOnHyphen, "Truncate on hyphen", true);
			m_aMediaIdAdjustments.Add(RegMediaIdAdjustments.StripZerosFirst, "Remove first zero padding", true);
			m_aMediaIdAdjustments.Add(RegMediaIdAdjustments.StripZerosAll, "Remove all zero padding", false);
			 
			m_aForeignBarcodeAdjustments.Add(RegForeignBarcodeAdjustments.AssignFromFilename, "Assign from filename", false);
			m_aForeignBarcodeAdjustments.Add(RegForeignBarcodeAdjustments.TruncateOnSpace, "Truncate on space", false);
			m_aForeignBarcodeAdjustments.Add(RegForeignBarcodeAdjustments.TruncateOnHyphen, "Truncate on hyphen", false);
			m_aForeignBarcodeAdjustments.Add(RegForeignBarcodeAdjustments.StripZerosFirst, "Remove first zero padding", false);
			m_aForeignBarcodeAdjustments.Add(RegForeignBarcodeAdjustments.StripZerosAll, "Remove all zero padding", false);
			 
			m_aFlags.Multiple = true;
			m_aFlags.Add(RegFlags.IncludeSubfolders, "Include all subfolders", true);
			m_aFlags.Add(RegFlags.PauseOnError, "Pause on error", true);
			m_aFlags.Add(RegFlags.UpdateSuperNodes, "Update super nodes", false);
			m_aFlags.Add(RegFlags.ShowOnDrop, "Show this form on drop", true);
			m_aFlags.Add(RegFlags.AssignFilenameToBarcode, "Treat filename as page number", false);
			m_aFlags.Add(RegFlags.MultiPageTiff, "Treat TIFF as multipage doc", false);
			 
		}// CTmaxRegOptions()
		
		/// <summary>
		/// This method is called to check the current value of the specified flag
		/// </summary>
		/// <param name="eFlag">Flag to be checked</param>
		/// <returns>true if selected</returns>
		public bool GetFlag(RegFlags eFlag)
		{
			if(m_aFlags != null)
			{
				return m_aFlags.GetSelected(eFlag);
			}
			else
			{
				return false;
			}
		
		}// GetFlag(RegFlags eFlag)
		
		/// <summary>
		/// This method is called to check the current value of the specified flag
		/// </summary>
		/// <param name="eFlag">Folder assignment option to be checked</param>
		/// <returns>true if selected</returns>
		public bool GetFolderAssignment(RegFolderAssignments eFolderAssignment)
		{
			if(m_aFolderAssignments != null)
			{
				return m_aFolderAssignments.GetSelected(eFolderAssignment);
			}
			else
			{
				return false;
			}
		
		}// GetFolderAssignment(RegFolderAssignments eFolderAssignment)
		
		/// <summary>
		/// This method is called to load the registration options from the specified XML ini file
		/// </summary>
		/// <param name="xmlIni">The initialization file containing the registration option values</param>
		public void Load(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			//	Are we going to allow the user to register linked documents
			m_bAllowRegisterLinked = xmlIni.ReadBool(XMLINI_ALLOW_REGISTER_LINKED_KEY, m_bAllowRegisterLinked);
			if(m_bAllowRegisterLinked == true)
			{
				m_aFileTransfers.Add(RegFileTransfers.Link, "Link To Source Folder");
			}

			FileTransfer = (RegFileTransfers)xmlIni.ReadEnum(XMLINI_FILE_TRANSFER_KEY, FileTransfer);
			MediaCreation = (RegMediaCreations)xmlIni.ReadEnum(XMLINI_MEDIA_CREATION_KEY, MediaCreation);
			MorphMethod = (RegMorphMethods)xmlIni.ReadEnum(XMLINI_MORPH_METHOD_KEY, MorphMethod);
			MorphMethodText = xmlIni.Read(XMLINI_MORPH_METHOD_TEXT_KEY, MorphMethodText);
			ConflictResolution = (RegConflictResolutions)xmlIni.ReadEnum(XMLINI_CONFLICT_RESOLUTION_KEY, ConflictResolution);
			ConflictResolutionText = xmlIni.Read(XMLINI_CONFLICT_RESOLUTION_TEXT_KEY, ConflictResolutionText);
			ConflictAutoPrefix = xmlIni.Read(XMLINI_CONFLICT_AUTO_PREFIX_KEY, ConflictAutoPrefix);
			m_bUseSherrodSplitter = xmlIni.ReadBool(XMLINI_USE_SHERROD_SPLITTER_KEY, m_bUseSherrodSplitter);
			
			if(m_aFlags != null)
			{
				foreach(CTmaxOption regOption in m_aFlags)
				{
					regOption.Selected = xmlIni.ReadBool(XMLINI_FLAGS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
			if(m_aFolderAssignments != null)
			{
				foreach(CTmaxOption regOption in m_aFolderAssignments)
				{
					regOption.Selected = xmlIni.ReadBool(XMLINI_FOLDER_ASSIGNMENTS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
			if(m_aMediaIdAdjustments != null)
			{
				foreach(CTmaxOption regOption in m_aMediaIdAdjustments)
				{
					regOption.Selected = xmlIni.ReadBool(XMLINI_MEDIA_ID_ADJUSTMENTS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
			if(m_aForeignBarcodeAdjustments != null)
			{
				foreach(CTmaxOption regOption in m_aForeignBarcodeAdjustments)
				{
					regOption.Selected = xmlIni.ReadBool(XMLINI_FOREIGN_BARCODE_ADJUSTMENTS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>
		/// This method is called to store the registration options in the specified XML ini file
		/// </summary>
		/// <param name="xmlIni">The initialization file to store the registration option values</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			xmlIni.Write(XMLINI_FILE_TRANSFER_KEY, FileTransfer);	
			xmlIni.Write(XMLINI_MEDIA_CREATION_KEY, MediaCreation);	
			xmlIni.Write(XMLINI_MORPH_METHOD_KEY, MorphMethod);	
			xmlIni.Write(XMLINI_MORPH_METHOD_TEXT_KEY, MorphMethodText);	
			xmlIni.Write(XMLINI_CONFLICT_RESOLUTION_KEY, ConflictResolution);	
			xmlIni.Write(XMLINI_CONFLICT_RESOLUTION_TEXT_KEY, ConflictResolutionText);	
			xmlIni.Write(XMLINI_CONFLICT_AUTO_PREFIX_KEY, ConflictAutoPrefix);	
			xmlIni.Write(XMLINI_ALLOW_REGISTER_LINKED_KEY, m_bAllowRegisterLinked);	
			xmlIni.Write(XMLINI_USE_SHERROD_SPLITTER_KEY, m_bUseSherrodSplitter);	

			if(m_aFlags != null)
			{
				foreach(CTmaxOption regOption in m_aFlags)
				{
					xmlIni.Write(XMLINI_FLAGS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
			if(m_aFolderAssignments != null)
			{
				foreach(CTmaxOption regOption in m_aFolderAssignments)
				{
					xmlIni.Write(XMLINI_FOLDER_ASSIGNMENTS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
			if(m_aMediaIdAdjustments != null)
			{
				foreach(CTmaxOption regOption in m_aMediaIdAdjustments)
				{
					xmlIni.Write(XMLINI_MEDIA_ID_ADJUSTMENTS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
			if(m_aForeignBarcodeAdjustments != null)
			{
				foreach(CTmaxOption regOption in m_aForeignBarcodeAdjustments)
				{
					xmlIni.Write(XMLINI_FOREIGN_BARCODE_ADJUSTMENTS_KEY, regOption.Value.ToString(), regOption.Selected);
				}
			}
		
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>Called to get the text used for conflict resolution</summary>
		/// <param name="tmaxRecord">The record being registered</param>
		/// <param name="strConflict">The conflicting value</param>
		/// <param name="eResolution">The method to use for the resolution</param>
		/// <returns>The resolved string</returns>
		public string Resolve(ITmaxMediaRecord tmaxRecord, string strConflict, RegConflictResolutions eResolution)
		{
			string strResolution = "";
			
			//	What resolution are we supposed to attempt?
			switch(eResolution)
			{
				case RegConflictResolutions.Automatic:
				
					if(this.ConflictAutoPrefix.Length == 0)
						this.ConflictAutoPrefix = "Auto";
						
					if(strConflict.Length > 0)
						strResolution = String.Format("{0}_{1}_{2}", this.ConflictAutoPrefix, tmaxRecord.GetAutoId(), strConflict);
					else
						strResolution = String.Format("{0}_{1}", this.ConflictAutoPrefix, tmaxRecord.GetAutoId());
					break;
					
				case RegConflictResolutions.Suffix:
							
					if(this.ConflictResolutionText.Length == 0)
						this.ConflictResolutionText = "xx";
						
					strResolution = String.Format("{0}_{1}", strConflict, this.ConflictResolutionText);
					break;
								
				case RegConflictResolutions.Prefix:
							
					if(this.ConflictResolutionText.Length == 0)
						this.ConflictResolutionText = "xx";
						
					strResolution = String.Format("{0}_{1}", this.ConflictResolutionText, strConflict);
					break;
								
				case RegConflictResolutions.Prompt:
				default:
				
					//	This should be handled by the caller
					break;
								
			}// switch(m_tmaxRegisterOptions.ConflictResolution)
			
			return strResolution;
			
		}// public string Resolve(ITmaxMediaRecord tmaxRecord, string strConflict)
		
		/// <summary>Called to get the text used for conflict resolution</summary>
		/// <param name="tmaxRecord">The record being registered</param>
		/// <param name="strConflict">The conflicting value</param>
		/// <returns>The resolved string</returns>
		public string Resolve(ITmaxMediaRecord tmaxRecord, string strConflict)
		{
			return Resolve(tmaxRecord, strConflict, this.ConflictResolution);
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>This property provides access to the list of Media Creation options</summary>
		public CTmaxOptions MediaCreations
		{
			get { return m_aMediaCreations; }
		}
		
		/// <summary>This property provides access to the current media creation selection</summary>
		public RegMediaCreations MediaCreation
		{
			get 
			{ 
				CTmaxOption tmaxOption = null;
				
				if(m_aMediaCreations != null)
					tmaxOption = m_aMediaCreations.GetSelection();
					
				if(tmaxOption != null)
					return (RegMediaCreations)tmaxOption.Value;
				else
					return RegMediaCreations.OnePerFolder;
			}
			set
			{
				if(m_aMediaCreations != null)
					m_aMediaCreations.SetSelected(value);
			}
			
		}// MediaCreation property
		
		/// <summary>This property provides access to the list of File Transfer options</summary>
		public CTmaxOptions FileTransfers
		{
			get { return m_aFileTransfers; }
		}
		
		/// <summary>This property provides access to the current file transfer selection</summary>
		public RegFileTransfers FileTransfer
		{
			get 
			{ 
				CTmaxOption tmaxOption = null;
				
				if(m_aFileTransfers != null)
					tmaxOption = m_aFileTransfers.GetSelection();
					
				if(tmaxOption != null)
					return (RegFileTransfers)tmaxOption.Value;
				else
					return RegFileTransfers.Copy;
			}
			set
			{
				if(m_aFileTransfers != null)
					m_aFileTransfers.SetSelected(value);
			}
			
		}// FileTransfer property
		
		/// <summary>This property provides access to the list of Morph Methods</summary>
		public CTmaxOptions MorphMethods
		{
			get { return m_aMorphMethods; }
		}
		
		/// <summary>This property provides access to the current morph method selection</summary>
		public RegMorphMethods MorphMethod
		{
			get 
			{ 
				CTmaxOption tmaxOption = null;
				
				if(m_aMorphMethods != null)
					tmaxOption = m_aMorphMethods.GetSelection();
					
				if(tmaxOption != null)
					return (RegMorphMethods)tmaxOption.Value;
				else
					return RegMorphMethods.None;
			}
			set
			{
				if(m_aMorphMethods != null)
					m_aMorphMethods.SetSelected(value);
			}
			
		}// MorphMethod property
		
		/// <summary>This property provides access to the text used for conflict resolution</summary>
		public string MorphMethodText
		{
			get 
			{ 
				return m_strMorphMethodText;
			}
			set
			{
				m_strMorphMethodText = value;
			}
			
		}// m_strMorphMethodText property
		
		/// <summary>This property provides access to the list of Conflict Resolution methods</summary>
		public CTmaxOptions ConflictResolutions
		{
			get { return m_aConflictResolutions; }
		}
		
		/// <summary>This property provides access to the current conflict resolution selection</summary>
		public RegConflictResolutions ConflictResolution
		{
			get 
			{ 
				CTmaxOption tmaxOption = null;
				
				if(m_aConflictResolutions != null)
					tmaxOption = m_aConflictResolutions.GetSelection();
					
				if(tmaxOption != null)
					return (RegConflictResolutions)tmaxOption.Value;
				else
					return RegConflictResolutions.Prompt;
			}
			set
			{
				if(m_aConflictResolutions != null)
					m_aConflictResolutions.SetSelected(value);
			}
			
		}// ConflictResolution property
		
		/// <summary>This property provides access to the text used for conflict resolution</summary>
		public string ConflictResolutionText
		{
			get 
			{ 
				return m_strConflictResolutionText;
			}
			set
			{
				m_strConflictResolutionText = value;
			}
			
		}// ConflictResolutionText property
		
		/// <summary>This property provides access to the text used to prefix the database id for automatic conflict resolution</summary>
		public string ConflictAutoPrefix
		{
			get 
			{ 
				return m_strConflictAutoPrefix;
			}
			set
			{
				m_strConflictAutoPrefix = value;
			}
			
		}// ConflictAutoPrefix property
		
		/// <summary>This property provides access to the list of Folder Assignment options</summary>
		public CTmaxOptions FolderAssignments
		{
			get { return m_aFolderAssignments; }
		}
		
		/// <summary>This property provides access to the list of MediaId adjustment options</summary>
		public CTmaxOptions MediaIdAdjustments
		{
			get { return m_aMediaIdAdjustments; }
		}
		
		/// <summary>This property provides access to the list of ForeignBarcode adjustment options</summary>
		public CTmaxOptions ForeignBarcodeAdjustments
		{
			get { return m_aForeignBarcodeAdjustments; }
		}
		
		/// <summary>This property provides access to the list of flags</summary>
		public CTmaxOptions Flags
		{
			get { return m_aFlags; }
		}
		
		/// <summary>True to allow user to register linked media</summary>
		public bool AllowRegisterLinked
		{
			get { return m_bAllowRegisterLinked; }
		}
		
		/// <summary>Hidden flag to request use of Sherrod library for TIFF splitting instead of LeadTools</summary>
		public bool UseSherrodSplitter
		{
			get { return m_bUseSherrodSplitter; }
		}

        /// <summary>This property provides access to the Output type for PDF</summary>
        public TmaxPDFOutputType OutputType
        {
            get
            {
                return m_bOutputType;
            }
            set
            {
                m_bOutputType = value;
            }

        }// m_bOutputType property

        /// <summary>This property provides access to the Custom DPI for PDF</summary>
        public short CustomDPI
        {
            get
            {
                return m_bCustomDPI;
            }
            set
            {
                m_bCustomDPI = value;
            }

        }// m_bCustomDPI property

        /// <summary>This property provides access to the Use Custom DPI for PDF</summary>
        public bool UseCustomDPI
        {
            get
            {
                return m_bUseCustomDPI;
            }
            set
            {
                m_bUseCustomDPI = value;
            }

        }// m_bUseCustomDPI property

        /// <summary>This property provides access to the Use Custom DPI for PDF</summary>
        public bool DisableCustomDither
        {
            get
            {
                return m_bDisableCustomDither;
            }
            set
            {
                m_bDisableCustomDither = value;
            }

        }// m_bUseCustomDPI property

        #endregion Properties

	}//	CTmaxRegOptions

}//namespace FTI.Shared.Trialmax
