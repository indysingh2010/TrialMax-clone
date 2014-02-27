using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to implement an objections database filter</summary>
	public class CTmaxObjectionsFilter
	{
		#region Constants

		private const string XMLINI_DEPOSITION_ID_KEY = "DepositionId";

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";

		/// <summary>Local member bound to DepositionId property</summary>
		private string m_strDepositionId = "";

		#endregion Private Members

		#region Public Members

		/// <summary>Constructor</summary>
		public CTmaxObjectionsFilter()
		{

			//	Attach the terms collection events
			m_tmaxEventSource.Name = "Objections Filter";

		}// public CTmaxObjectionsFilter()

		/// <summary>This method will copy the properties and terms of the specified source filter</summary>
		public void Copy(CTmaxObjectionsFilter tmaxSource)
		{
			this.Name = tmaxSource.Name;
			this.DepositionId = tmaxSource.DepositionId;

		}// public voidd Copy(CTmaxObjectionsFilter tmaxSource)

		/// <summary>This method is called to get the name of the section in the XML configuration file for this filter</summary>
		/// <returns>The name of the section containing the configuration information for this filter</returns>
		public string GetXmlSectionName()
		{
			if(m_strName.Length > 0)
				return ("trialMax/station/objections/filter/" + m_strName);
			else
				return "";

		}// public string GetXmlSectionName()

		/// <summary>This method will reset the class members</summary>
		public void Clear()
		{
			this.DepositionId = "";
		}

		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;

			//	Read the filter properties from file
			m_strDepositionId = xmlIni.Read(XMLINI_DEPOSITION_ID_KEY, m_strDepositionId);

		}// public void Load(CXmlIni xmlIni)

		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(GetXmlSectionName(), true, true) == false) return;

			//	Write the filter properties to file
			xmlIni.Write(XMLINI_DEPOSITION_ID_KEY, this.DepositionId);

		}// public void Save(CXmlIni xmlIni)


		#endregion Private Methods

		#region Properties

		/// <summary>The EventSource for this object</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>The Name used to store the information for this filter in a TrialMax XML configuration file</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}

		/// <summary>The MediaId of the deposition that owns the objections</summary>
		public string DepositionId
		{
			get { return m_strDepositionId; }
			set { m_strDepositionId = value; }
		}

		#endregion Properties

	}//	public class CTmaxObjectionsFilter

}// namespace FTI.Shared.Trialmax
