using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition node</summary>
	public class CXmlORESetup : CXmlFile
	{
		#region Constants

		public const string XML_ROOT_NAME = "reportconfiguration";

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to OREOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxOREOptions m_tmaxOREOptions = new CTmaxOREOptions();

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CXmlORESetup()
		{
			m_strRoot = XML_ROOT_NAME;
			Extension = GetExtension();

			//	Assign the default filename
			Filename = "_tmax_ore_setup_.xml";

			//	Populate the error builder
			SetErrorStrings();

		}// public CXmlORESetup()

		/// <summary>This method is called to open the specified file</summary>
		/// <param name="bCreate">true to create the file if it doesn't exist</param>
		/// <returns>true if successful</returns>
		public override bool Open(bool bCreate)
		{
			bool bSuccessful = false;

			//	Do the base class processing first
			if(base.Open(bCreate) == false) return false;

			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlRoot != null);

			try
			{
				while(bSuccessful == false)
				{
					//	Use the XML root node to set the properties for this object
					//if(SetProperties(m_xmlRoot) == false)
						//break;

					//	We're done 
					bSuccessful = true;

				}//	while(bSuccessful == false)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_FILE_EX, m_strFileSpec), Ex);
			}

			if(bSuccessful == false)
			{
				Close();
				return false;
			}
			else
			{
				return true;
			}

		}//	Open(bool bCreate)

		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <returns>true if successful</returns>
		public override bool Open()
		{
			//	We don't create Adobe conversion result files
			return Open(false);
		}

		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <param name="strFileSpec">Fully qualified path to the file</param>
		/// <returns>true if successful</returns>
		public override bool Open(string strFileSpec)
		{
			//	We don't create Adobe conversion result files
			return Open(strFileSpec, false);
		}

		/// <summary>This method is called to save the script to file</summary>
		/// <returns>true if successful</returns>
		public override bool Save()
		{
			XmlTextWriter xmlWriter = null;

			//	Construct the full path specification
			GetFileSpec();
			if(m_strFileSpec.Length == 0) return false;

			//	Delete the existing file
			if(System.IO.File.Exists(m_strFileSpec) == true)
			{
				try { System.IO.File.Delete(m_strFileSpec); }
				catch { }
			}

			try
			{
				//	Open the file
				if(Open(true) == false) return false;

				Debug.Assert(m_xmlDocument != null);
				Debug.Assert(m_xmlRoot != null);

				//	Add the options to the root node
				//
				//	NOTE:	We don't normally make the root node the primary element
				//			but the designers of this file format wanted to keep it
				//			as simple as possible
				AddOREOptions(m_xmlRoot as XmlElement);

				//	Save the XML document to file	
				if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
				{
					xmlWriter.Formatting = System.Xml.Formatting.Indented;
					xmlWriter.Indentation = 4;

					m_xmlDocument.Save(xmlWriter);
					xmlWriter.Close();

					Close();

					return true;

				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
			}

			return false;

		}//	public override bool Save()

		/// <summary>This method is called to populate the Options collection</summary>
		/// <param name="tmaxORETemplate">The ORE template containing the option values</param>
		public void Fill(CTmaxORETemplate tmaxORETemplate)
		{
			//	Clear the existing options
			m_tmaxOREOptions.Clear();
			
			//	Use the template collections to fill the local collection
			if(m_tmaxOREOptions != null)
			{
				Add(tmaxORETemplate.Constants);
				Add(tmaxORETemplate.Runtime);
				Add(tmaxORETemplate.Common);
				Add(tmaxORETemplate.Flags);
				Add(tmaxORETemplate.Colors);
				Add(tmaxORETemplate.Margins);
				Add(tmaxORETemplate.Columns);
			}

		}// public void Fill(CTmaxORETemplate tmaxORETemplate)

		/// <summary>This method is called to add options the Options collection</summary>
		/// <param name="tmaxOREOptions">The collection of options to be added</param>
		public void Add(CTmaxOREOptions tmaxOREOptions)
		{
			if(tmaxOREOptions != null)
			{
				foreach(CTmaxOREOption O in tmaxOREOptions)
					m_tmaxOREOptions.Add(O);
			}

		}// public void Add(CTmaxOREOptions tmaxOREOptions)

		#endregion Public Methods

		#region Protected Methods

		/// <summary>Called to add the ORE options to the specified elment's child collection</summary>
		/// <param name="xmlElement">Xml parent element</param>
		///	<returns>true if successful</returns>
		protected bool AddOREOptions(XmlElement xmlElement)
		{
			bool	bSuccessful = true;
			XmlNode	xmlOption = null;
			
			if((m_tmaxOREOptions != null) && (m_tmaxOREOptions.Count > 0))
			{
				foreach(CTmaxOREOption O in m_tmaxOREOptions)
				{
					if((xmlOption = O.ToXmlNode(m_xmlDocument)) != null)
						xmlElement.AppendChild(xmlOption);

				}// foreach(CTmaxOREOption O in m_tmaxOREOptions)

			}// if((m_tmaxOREOptions != null) && (m_tmaxOREOptions.Count > 0))

			return bSuccessful;

		}// protected bool AddOREOptions(XmlElement xmlElement)

		#endregion Protected Methods

		#region Properties

		/// <summary>The collection of configuration options to be written to the file</summary>
		public CTmaxOREOptions OREOptions
		{
			get { return m_tmaxOREOptions; }
		}

		#endregion Properties

	}// public class CXmlORESetup

}// namespace FTI.Shared.Xml
