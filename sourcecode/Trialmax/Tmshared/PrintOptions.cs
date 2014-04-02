using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class contains information associated with an individual field in the print job</summary>
	public class CTmaxPrintField : ITmaxSortable
	{
		#region Private Members
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to Text property</summary>
		private string m_strText = "";
		
		/// <summary>Local member bound to PrintText property</summary>
		private string m_strPrintText = "";
		
		/// <summary>Local member bound to Id property</summary>
		private int m_iId = 0;
		
		/// <summary>Local member bound to Print property</summary>
		private bool m_bPrint = true;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxPrintField()
		{
		}
	
		/// <summary>Copy constructor</summary>
		/// <param name="tmaxSource">the source object to be copied</param>
		public CTmaxPrintField(CTmaxPrintField tmaxSource)
		{
			if(tmaxSource != null) 
				Copy(tmaxSource);
		}
	
		/// <summary>Overloaded base class member for use in list boxes</summary>
		/// <returns>The text representation of this object</returns>
		public override string ToString()
		{
			return m_strName;
		}
	
		/// <summary>This method will copy the properties of the specified source object</summary>
		public void Copy(CTmaxPrintField tmaxSource)
		{
			this.Id = tmaxSource.Id;
			this.Name = tmaxSource.Name;
			this.Text = tmaxSource.Text;
			this.Print = tmaxSource.Print;
		
		}// public void Copy(CTmaxPrintField tmaxSource)
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="tmaxCompare">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxField, 0 if equal, 1 if greater than</returns>
		public int Compare(CTmaxPrintField tmaxCompare, long lMode)
		{
			return -1;
					
		}// public int Compare(CTmaxPrintField tmaxCompare, long lMode)
		
		/// <summary>This function is called to compare the specified result object to this result</summary>
		/// <param name="O">The object to be compared</param>
		/// <param name="lMode">Sort mode identifier</param>
		/// <returns>-1 if this result less than tmaxField, 0 if equal, 1 if greater than</returns>
		int ITmaxSortable.Compare(ITmaxSortable O, long lMode)
		{
			try { return Compare((CTmaxPrintField)O, lMode); }
			catch { return -1; }
			
		}// public int ITmaxSortable.Compare(ITmaxSortable O)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The template field identifier</summary>
		public int Id
		{
			get { return m_iId; }
			set { m_iId = value; }
		}
		
		/// <summary>The Name that identifies the field</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>The text format string</summary>
		public string Text
		{
			get { return m_strText; }
			set { m_strText = value; }
		}
		
		/// <summary>The text that gets printed</summary>
		public string PrintText
		{
			get { return m_strPrintText; }
			set { m_strPrintText = value; }
		}
		
		/// <summary>true if the field should be printed</summary>
		public bool Print
		{
			get { return m_bPrint; }
			set { m_bPrint = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxPrintField : ITmaxSortable

	/// <summary>This class manages a collection of filter fields</summary>
	public class CTmaxPrintFields : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxPrintFields() : base()
		{
			//	Assign a default sorter
			//base.Comparer = new CTmaxSorter();
			
			this.KeepSorted = false;
			
			m_tmaxEventSource.Name = "Print Fields";
		}

		/// <summary>This method will copy the fields in the source collection to this collection</summary>
		/// <param name="tmaxSource">The source collection of fields</param>
		public void Copy(CTmaxPrintFields tmaxSource)
		{
			//	Clear the existing objects
			this.Clear();
			
			//	Copy each of the source objects
			if((tmaxSource != null) && (tmaxSource.Count > 0))
			{
				foreach(CTmaxPrintField O in tmaxSource)
				{
					Add(new CTmaxPrintField(O));
				}
				
			}// if((tmaxSource != null) && (tmaxSource.Count > 0))
			
		}// public void Copy(CTmaxPrintFields tmaxSource)
		
		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="tmaxField">CTmaxPrintField object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CTmaxPrintField Add(CTmaxPrintField tmaxField)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(tmaxField as object);

				return tmaxField;
			}
			catch
			{
				return null;
			}
			
		}// public CTmaxPrintField Add(CTmaxPrintField tmaxField)

		/// <summary>This method is called to remove an object from the list</summary>
		/// <param name="tmaxField">The filter object to be removed</param>
		public void Remove(CTmaxPrintField tmaxField)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxField as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxPrintField tmaxField)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxField">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxPrintField tmaxField)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxField as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CTmaxPrintField this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CTmaxPrintField);
			}
		}

		/// <summary>Called to find the object with the specifed id and/or name</summary>
		/// <param name="iId">The id of the desired object</param>
		/// <param name="strName">The name of the desired object</param>
		/// <returns>The object if found</returns>
		public CTmaxPrintField Find(int iId, string strName)
		{
			foreach(CTmaxPrintField O in this)
			{
				//	Are we checking the Id?
				if(iId > 0)
				{
					if(iId == O.Id)
					{
						//	Are we also checking the name?
						if((strName != null) && (strName.Length > 0))
						{
							if(String.Compare(strName, O.Name, true) == 0)
								return O;
						}
						else
						{
							return O; // Only have to satisfy the Id
						}
						
					}// if(iId == O.Id)
					
				}
				else if(String.Compare(strName, O.Name, true) == 0)
				{
					return O;
				}
				
			}// foreach(CTmaxPrintField O in this)
			
			return null; // Not found
			
		}// public CTmaxPrintField Find(int iId, string strName)
		
		/// <summary>Called to find the object with the specifed id and/or name</summary>
		/// <param name="iId">The id of the desired object</param>
		/// <returns>The object if found</returns>
		public CTmaxPrintField Find(int iId)
		{
			return Find(iId, null);
		}
			
		/// <summary>Called to find the object with the specifed id and/or name</summary>
		/// <param name="strName">The name of the desired object</param>
		/// <returns>The object if found</returns>
		public CTmaxPrintField Find(string strName)
		{
			return Find(-1, strName);
		}
			
		#endregion Public Methods
		
	}//	public class CTmaxPrintFields : CTmaxSortedArrayList
		

	/// <summary>This class manages the options used for printing</summary>
	public class CTmaxPrintOptions
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "PrintOptions";
		private const string XMLINI_COPIES_KEY					= "Copies";
		private const string XMLINI_COLLATE_KEY					= "Collate";
		private const string XMLINI_INCLUDE_TREATMENTS_KEY		= "IncludeTreatments";
		private const string XMLINI_INCLUDE_SUBBINDERS_KEY		= "IncludeSubBinders";
		private const string XMLINI_INCLUDE_LINKS_KEY			= "IncludeLinks";
		private const string XMLINI_ONLY_FIRST_PAGE_KEY			= "OnlyFirstPage";
		private const string XMLINI_ONLY_TREATMENTS_KEY			= "OnlyTreatments";
		private const string XMLINI_ONLY_LINKS_KEY				= "OnlyLinks";
		private const string XMLINI_TEMPLATE_KEY				= "Template";
		private const string XMLINI_PRINTER_KEY					= "Printer";
		private const string XMLINI_PRINT_CALLOUTS_KEY			= "PrintCallouts";
		private const string XMLINI_PRINT_PATH_KEY				= "PrintPath";
		private const string XMLINI_PRINT_PAGE_TOTAL_KEY		= "PrintPageTotal";
		private const string XMLINI_FORCE_NEW_PAGE_KEY			= "ForceNewPage";
		private const string XMLINI_PRINT_CALLOUT_BORDERS_KEY	= "PrintCalloutBorders";
		private const string XMLINI_DOUBLE_SIDED_KEY			= "DoubleSided";
		private const string XMLINI_FIELDS_KEY					= "Fields";
		private const string XMLINI_INSERT_SLIP_SHEET_KEY		= "InsertSlipSheet";
		
		private const string XMLINI_FIELD_ID_ATTRIBUTE			= "id";
		private const string XMLINI_FIELD_NAME_ATTRIBUTE		= "name";
		private const string XMLINI_FIELD_TEXT_ATTRIBUTE		= "text";
		private const string XMLINI_FIELD_PRINT_ATTRIBUTE		= "print";

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to TextFields property</summary>
		private CTmaxPrintFields m_aTextFields = new CTmaxPrintFields();
		
		/// <summary>Local member bound to Copies property</summary>
		private int m_iCopies = 1;
		
		/// <summary>Local member bound to Collate property</summary>
		private bool m_bCollate = false;

		/// <summary>Local member bound to OnlyFirstPage property</summary>
		private bool m_bOnlyFirstPage = false;

		/// <summary>Local member bound to OnlyTreatments property</summary>
		private bool m_bOnlyTreatments = false;

		/// <summary>Local member bound to OnlyLinks property</summary>
		private bool m_bOnlyLinks = false;

		/// <summary>Local member bound to IncludeTreatments property</summary>
		private bool m_bIncludeTreatments = true;

		/// <summary>Local member bound to IncludeLinks property</summary>
		private bool m_bIncludeLinks = false;

		/// <summary>Local member bound to IncludeSubBinders property</summary>
		private bool m_bIncludeSubBinders = true;
		
		/// <summary>Local member bound to Template property</summary>
		private string m_strTemplate = "";
		
		/// <summary>Local member bound to Printer property</summary>
		private string m_strPrinter = "";
		
		/// <summary>Local member bound to Fields property</summary>
		private int m_iFields = 0;
		
		/// <summary>Local member bound to DoubleSided property</summary>
		private bool m_bDoubleSided = true;
		
		/// <summary>Local member bound to PrintCallouts property</summary>
		private bool m_bPrintCallouts = true;
		
		/// <summary>Local member bound to PrintCalloutBorders property</summary>
		private bool m_bPrintCalloutBorders = true;
		
		/// <summary>Local member bound to PrintPath property</summary>
		private bool m_bPrintPath = true;
		
		/// <summary>Local member bound to PrintPageTotal property</summary>
		private bool m_bPrintPageTotal = true;
		
		/// <summary>Local member bound to ForceNewPage property</summary>
		private bool m_bForceNewPage = false;

		/// <summary>Local member bound to InsertSlipSheet property</summary>
		private bool m_bInsertSlipSheet = false;

		#endregion Private Members
	
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxPrintOptions()
		{
		}
		
		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		public void Load(CXmlIni xmlIni)
		{
			int				iLine = 1;
			CTmaxPrintField	tmaxField = null;
			bool			bContinue = true;

			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_iCopies = xmlIni.ReadInteger(XMLINI_COPIES_KEY, m_iCopies);
			m_bDoubleSided = xmlIni.ReadBool(XMLINI_DOUBLE_SIDED_KEY, m_bDoubleSided);
			m_bCollate = xmlIni.ReadBool(XMLINI_COLLATE_KEY, m_bCollate);
			m_bOnlyTreatments = xmlIni.ReadBool(XMLINI_ONLY_TREATMENTS_KEY, m_bOnlyTreatments);
			m_bOnlyFirstPage = xmlIni.ReadBool(XMLINI_ONLY_FIRST_PAGE_KEY, m_bOnlyFirstPage);
			m_bOnlyTreatments = xmlIni.ReadBool(XMLINI_ONLY_TREATMENTS_KEY, m_bOnlyTreatments);
			m_bOnlyLinks = xmlIni.ReadBool(XMLINI_ONLY_LINKS_KEY, m_bOnlyLinks);
			m_bIncludeLinks = xmlIni.ReadBool(XMLINI_INCLUDE_LINKS_KEY, m_bIncludeLinks);
			m_bIncludeTreatments = xmlIni.ReadBool(XMLINI_INCLUDE_TREATMENTS_KEY, m_bIncludeTreatments);
			m_bIncludeSubBinders = xmlIni.ReadBool(XMLINI_INCLUDE_SUBBINDERS_KEY, m_bIncludeSubBinders);
			m_bPrintCallouts = xmlIni.ReadBool(XMLINI_PRINT_CALLOUTS_KEY, m_bPrintCallouts);
			m_bPrintCalloutBorders = xmlIni.ReadBool(XMLINI_PRINT_CALLOUT_BORDERS_KEY, m_bPrintCalloutBorders);
			m_bPrintPath = xmlIni.ReadBool(XMLINI_PRINT_PATH_KEY, m_bPrintPath);
			m_bPrintPageTotal = xmlIni.ReadBool(XMLINI_PRINT_PAGE_TOTAL_KEY, m_bPrintPageTotal);
			m_bForceNewPage = xmlIni.ReadBool(XMLINI_FORCE_NEW_PAGE_KEY, m_bForceNewPage);
			m_bInsertSlipSheet = xmlIni.ReadBool(XMLINI_INSERT_SLIP_SHEET_KEY, m_bInsertSlipSheet);
			m_strTemplate = xmlIni.Read(XMLINI_TEMPLATE_KEY, m_strTemplate);
			m_strPrinter = xmlIni.Read(XMLINI_PRINTER_KEY, m_strPrinter);
			m_iFields = xmlIni.ReadInteger(XMLINI_FIELDS_KEY, m_iFields);
			
			//	Load all the coded terms
			m_aTextFields.Clear();
			while(bContinue == true)
			{
				tmaxField = new CTmaxPrintField();
				
				if((bContinue = Load(xmlIni, GetTextFieldKey(iLine++), tmaxField)) == true)
					m_aTextFields.Add(tmaxField);
			}
		
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to load the information for the specified field from the XML configuration file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		/// <param name="strKey">The line identifier</param>
		/// <param name="tmaxField">The field to be initialized</param>
		///	<returns>true to continue loading</returns>
		public bool Load(CXmlIni xmlIni, string strKey, CTmaxPrintField tmaxField)
		{
			bool bSuccessful = true;
			
			try
			{
				//	Read the ID
				tmaxField.Id = xmlIni.ReadInteger(strKey, XMLINI_FIELD_ID_ATTRIBUTE, 0);

				//	Have we run out of fields?
				if(tmaxField.Id == 0) return false;

				//	Read the rest of the attributes
				tmaxField.Name = xmlIni.Read(strKey, XMLINI_FIELD_NAME_ATTRIBUTE, "");
				tmaxField.Text = xmlIni.Read(strKey, XMLINI_FIELD_TEXT_ATTRIBUTE, "");
				tmaxField.Print = xmlIni.ReadBool(strKey, XMLINI_FIELD_PRINT_ATTRIBUTE, true);
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool Load(CXmlIni xmlIni, string strKey, CTmaxPrintField tmaxField)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the option values</param>
		public void Save(CXmlIni xmlIni)
		{
			int iLine = 1;

			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_COPIES_KEY, m_iCopies);
			xmlIni.Write(XMLINI_DOUBLE_SIDED_KEY, m_bDoubleSided);
			xmlIni.Write(XMLINI_COLLATE_KEY, m_bCollate);
			xmlIni.Write(XMLINI_ONLY_TREATMENTS_KEY, m_bOnlyTreatments);
			xmlIni.Write(XMLINI_ONLY_FIRST_PAGE_KEY, m_bOnlyFirstPage);
			xmlIni.Write(XMLINI_ONLY_LINKS_KEY, m_bOnlyLinks);
			xmlIni.Write(XMLINI_INCLUDE_LINKS_KEY, m_bIncludeLinks);
			xmlIni.Write(XMLINI_INCLUDE_TREATMENTS_KEY, m_bIncludeTreatments);
			xmlIni.Write(XMLINI_INCLUDE_SUBBINDERS_KEY, m_bIncludeSubBinders);
			xmlIni.Write(XMLINI_PRINT_CALLOUTS_KEY, m_bPrintCallouts);
			xmlIni.Write(XMLINI_PRINT_CALLOUT_BORDERS_KEY, m_bPrintCalloutBorders);
			xmlIni.Write(XMLINI_PRINT_PATH_KEY, m_bPrintPath);
			xmlIni.Write(XMLINI_PRINT_PAGE_TOTAL_KEY, m_bPrintPageTotal);
			xmlIni.Write(XMLINI_FORCE_NEW_PAGE_KEY, m_bForceNewPage);
			xmlIni.Write(XMLINI_INSERT_SLIP_SHEET_KEY, m_bInsertSlipSheet);
			xmlIni.Write(XMLINI_TEMPLATE_KEY, m_strTemplate);
			xmlIni.Write(XMLINI_PRINTER_KEY, m_strPrinter);
			xmlIni.Write(XMLINI_FIELDS_KEY, m_iFields);
			
			foreach(CTmaxPrintField O in m_aTextFields)
				Save(xmlIni, GetTextFieldKey(iLine++), O);

		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the specified field in the configuration file</summary>
		/// <param name="xmlIni">The initialization file to store the option values</param>
		/// <param name="strKey">The key used to identify the field in the file</param>
		/// <param name="tmaxField">The field to be stored in the file</param>
		public void Save(CXmlIni xmlIni, string strKey, CTmaxPrintField tmaxField)
		{
			try
			{
				xmlIni.Write(strKey, XMLINI_FIELD_ID_ATTRIBUTE, tmaxField.Id);
				xmlIni.Write(strKey, XMLINI_FIELD_NAME_ATTRIBUTE, tmaxField.Name);
				xmlIni.Write(strKey, XMLINI_FIELD_TEXT_ATTRIBUTE, tmaxField.Text);
				xmlIni.Write(strKey, XMLINI_FIELD_PRINT_ATTRIBUTE, tmaxField.Print);
			}
			catch
			{
			}

		}// public void Save(CXmlIni xmlIni, string strKey, CTmaxPrintField tmaxField)
		
		#endregion Public Methods
		
		#region Private Methods
		
		
		/// <summary>This method converts the numeric line identifier to a unique key for storing the text field descriptor</summary>
		/// <param name="iField">unique numeric identifier for the text field</param>
		/// <returns>The appropriate XML Ini key</returns>
		string GetTextFieldKey(int iField)
		{
			return String.Format("TF{0}", iField);
		}
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Collection of text fields in the active template</summary>
		public CTmaxPrintFields TextFields
		{
			get { return m_aTextFields; }
		}
		
		/// <summary>Number of copies</summary>
		public int Copies
		{
			get { return m_iCopies; }
			set { m_iCopies = value; }
		}
		
		/// <summary>Printer configured for double sided printing</summary>
		public bool DoubleSided
		{
			get { return m_bDoubleSided; }
			set { m_bDoubleSided = value; }
		}
		
		/// <summary>True to collate multiple copies</summary>
		public bool Collate
		{
			get { return m_bCollate; }
			set { m_bCollate = value; }
		}
		
		/// <summary>Print treatments without the source page</summary>
		public bool OnlyTreatments
		{
			get { return m_bOnlyTreatments; }
			set { m_bOnlyTreatments = value; }
		}

		/// <summary>Print document links without the parent designation</summary>
		public bool OnlyLinks
		{
			get { return m_bOnlyLinks; }
			set { m_bOnlyLinks = value; }
		}

		/// <summary>Print only the first page of documents included in the print job</summary>
		public bool OnlyFirstPage
		{
			get { return m_bOnlyFirstPage; }
			set { m_bOnlyFirstPage = value; }
		}

		/// <summary>Include treatments in the print job</summary>
		public bool IncludeTreatments
		{
			get { return m_bIncludeTreatments; }
			set { m_bIncludeTreatments = value; }
		}
		
		/// <summary>Include subBinders in the print job</summary>
		public bool IncludeSubBinders
		{
			get { return m_bIncludeSubBinders; }
			set { m_bIncludeSubBinders = value; }
		}

		/// <summary>Include linked documents in the print job</summary>
		public bool IncludeLinks
		{
			get { return m_bIncludeLinks; }
			set { m_bIncludeLinks = value; }
		}

		/// <summary>Force new page for each document</summary>
		public bool ForceNewPage
		{
			get { return m_bForceNewPage; }
			set { m_bForceNewPage = value; }
		}

		/// <summary>Insert slip sheet (blank page) between documents</summary>
		public bool InsertSlipSheet
		{
			get { return m_bInsertSlipSheet; }
			set { m_bInsertSlipSheet = value; }
		}

		/// <summary>Print treatment callouts</summary>
		public bool PrintCallouts
		{
			get { return m_bPrintCallouts; }
			set { m_bPrintCallouts = value; }
		}
		
		/// <summary>Print border around callouts</summary>
		public bool PrintCalloutBorders
		{
			get { return m_bPrintCalloutBorders; }
			set { m_bPrintCalloutBorders = value; }
		}
		
		/// <summary>Print full path in Filename field callouts</summary>
		public bool PrintPath
		{
			get { return m_bPrintPath; }
			set { m_bPrintPath = value; }
		}
		
		/// <summary>Print page total with page number field</summary>
		public bool PrintPageTotal
		{
			get { return m_bPrintPageTotal; }
			set { m_bPrintPageTotal = value; }
		}
		
		/// <summary>Name of printer used for last job</summary>
		public string Printer
		{
			get { return m_strPrinter; }
			set { m_strPrinter = value; }
		}
		
		/// <summary>Name of template used for last job</summary>
		public string Template
		{
			get { return m_strTemplate; }
			set { m_strTemplate = value; }
		}
		
		/// <summary>Active template fields bitmask</summary>
		public int Fields
		{
			get { return m_iFields; }
			set { m_iFields = value; }
		}
		
		#endregion Properties
	
	}// public class CTmaxPrintOptions

}// namespace FTI.Shared.Trialmax
