using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Reports
{
	/// <summary>This class acts as an interface between the application and the TrialMax report engine</summary>
	public class CTmaxReportManager
	{
		#region Constants
		
		private const string TRANSCRIPT_REPORT_BASE_FILENAME = "_transcript_report";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member bound to Database property</summary>
		private FTI.Trialmax.Database.CTmaxCaseDatabase m_tmaxDatabase = null;

		/// <summary>Local member associated with the TmaxProductManager property</summary>
		private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;

		/// <summary>Local member associated with the AppOptions property</summary>
		protected CTmaxManagerOptions m_tmaxAppOptions = null;

		/// <summary>Path to the folder containing the report templates and source databases</summary>
		private string m_strSourceFolder = "";
		
		/// <summary>Scripts report options</summary>
		private CROScripts m_roScripts = new CROScripts();
		
		/// <summary>Transcript report options</summary>
		private CROTranscript m_roTranscript = new CROTranscript();
		
		/// <summary>Exhibits report options</summary>
		private CROExhibits m_roExhibits = new CROExhibits();

		/// <summary>Objections report options</summary>
		private CROObjections m_roObjections = new CROObjections();

		#endregion Private Members
		
		#region Public Methods

		/// <summary>Constructor</summary>
		public CTmaxReportManager()
		{
			m_tmaxEventSource.Name = "Report Manager";
			
			//	Assume the source folder is the same in which the application is stored
			m_strSourceFolder = Process.GetCurrentProcess().MainModule.FileName;
			m_strSourceFolder = System.IO.Path.GetDirectoryName(m_strSourceFolder);
			if(m_strSourceFolder.EndsWith("\\") == false)
				m_strSourceFolder += "\\";
				
		}// public CTmaxReportManager()

		/// <summary>This method is called to execute a report</summary>
		/// <param name="eReport">Enumerated report identifier</param>
		/// <param name="tmaxItems">The collection of items that represent the records to be included in the report</param>
		/// <returns>true if successful</returns>
		public bool Execute(TmaxReports eReport, CTmaxItems tmaxItems)
		{
			//	Do we have a valid database?
			if((m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null))
			{
				m_tmaxEventSource.FireError(this, "Execute", "Unable to execute the " + eReport.ToString() + " report without a valid database");
				return false;
			}
			
			try
			{
				switch(eReport)
				{
					
					case TmaxReports.Scripts:
					
						return ExecuteScripts(tmaxItems);
						
					case TmaxReports.Transcript:
					
						return ExecuteTranscript(tmaxItems);
						
					case TmaxReports.Exhibits:
					
						return ExecuteExhibits(tmaxItems);

					case TmaxReports.Objections:

						return ExecuteObjections(tmaxItems);

					default:
					
						Debug.Assert(false);
						return false;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Execute", "Exception raised while executing " + eReport.ToString() + " report.", Ex);
				return false;
			}
			
		}// public bool Execute(TmaxReports eReport, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			try { m_roScripts.Load(xmlIni); }		catch{};
			try { m_roTranscript.Load(xmlIni); }	catch{};
			try { m_roExhibits.Load(xmlIni); }		catch{};
			try { m_roObjections.Load(xmlIni); }	catch{};
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			try { m_roScripts.Save(xmlIni); }		catch{};
			try { m_roTranscript.Save(xmlIni); }	catch{};
			try { m_roExhibits.Save(xmlIni); }		catch{};
			try { m_roObjections.Save(xmlIni); }	catch{};
			
		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to execute a transcript report</summary>
		/// <param name="tmaxItems">The collection of items that represent the records to be included in the report</param>
		/// <returns>true if successful</returns>
		private bool ExecuteTranscript(CTmaxItems tmaxItems)
		{
			CRFTranscript crfTranscript = new CRFTranscript();
			
			crfTranscript.Options = m_roTranscript;
			crfTranscript.Database = m_tmaxDatabase;
			crfTranscript.Items = tmaxItems;
			crfTranscript.SourceFolder = m_strSourceFolder;
			crfTranscript.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
			crfTranscript.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
			
			//	Does the form has what it needs to execute the report?
			if(crfTranscript.CanExecute() == true)
			{
				if(crfTranscript.ShowDialog() != DialogResult.Abort)
					return true;
			}
			
			//	Must have been a problem
			return false;
		
		}// private bool ExecuteTranscript(CTmaxCaseDatabase tmaxDatabase, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to execute a playlist summary report</summary>
		/// <param name="tmaxItems">The collection of items that represent the records to be included in the report</param>
		/// <returns>true if successful</returns>
		private bool ExecuteScripts(CTmaxItems tmaxItems)
		{
			CRFScripts crfScripts = new CRFScripts();
			
			crfScripts.Options = m_roScripts;
			crfScripts.Database = m_tmaxDatabase;
			crfScripts.Items = tmaxItems;
			crfScripts.SourceFolder = m_strSourceFolder;
			crfScripts.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
			crfScripts.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
			
			//	Does the form has what it needs to execute the report?
			if(crfScripts.CanExecute() == true)
			{
				if(crfScripts.ShowDialog() != DialogResult.Abort)
					return true;
			}
			
			//	Must have been a problem
			return false;

		}// private bool ExecuteScripts(CTmaxCaseDatabase tmaxDatabase, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to execute an exhibits list report</summary>
		/// <param name="tmaxItems">The collection of items that represent the records to be included in the report</param>
		/// <returns>true if successful</returns>
		public bool ExecuteExhibits(CTmaxItems tmaxItems)
		{
			CRFExhibits crfExhibits = new CRFExhibits();
			
			crfExhibits.Options = m_roExhibits;
			crfExhibits.Database = m_tmaxDatabase;
			crfExhibits.Items = tmaxItems;
			crfExhibits.SourceFolder = m_strSourceFolder;
			crfExhibits.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
			crfExhibits.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
			
			//	Does the form has what it needs to execute the report?
			if(crfExhibits.CanExecute() == true)
			{
				if(crfExhibits.ShowDialog() != DialogResult.Abort)
					return true;
			}
			
			//	Must have been a problem
			return false;

		}// private bool ExecuteExhibits(CTmaxCaseDatabase tmaxDatabase, CTmaxItems tmaxItems)

		/// <summary>This method is called to execute an objections report</summary>
		/// <param name="tmaxItems">The collection of items that represent the records to be included in the report</param>
		/// <returns>true if successful</returns>
		private bool ExecuteObjections(CTmaxItems tmaxItems)
		{
			CRFObjections crfObjections = new CRFObjections();

			crfObjections.Options = m_roObjections;
			crfObjections.Database = m_tmaxDatabase;
			crfObjections.ProductManager = m_tmaxProductManager;
			crfObjections.Items = tmaxItems;
			crfObjections.SourceFolder = m_strSourceFolder;
			crfObjections.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
			crfObjections.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);

			//	Does the form has what it needs to execute the report?
			if(crfObjections.CanExecute() == true)
			{
				if(crfObjections.ShowDialog() != DialogResult.Abort)
					return true;
			}

			//	Must have been a problem
			return false;

		}// private bool ExecuteObjections(CTmaxCaseDatabase tmaxDatabase, CTmaxItems tmaxItems)

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }

		}// EventSource property
		
		/// <summary>The active database</summary>
		public FTI.Trialmax.Database.CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
			
		}

		/// <summary>TrialMax application options</summary>
		public FTI.Shared.Trialmax.CTmaxManagerOptions AppOptions
		{
			get { return m_tmaxAppOptions; }
			set { m_tmaxAppOptions = value; }
		}

		/// <summary>TrialMax application product descriptor</summary>
		public FTI.Shared.Trialmax.CTmaxProductManager ProductManager
		{
			get { return m_tmaxProductManager; }
			set { m_tmaxProductManager = value; }
		}

		/// <summary>The folder containing the report source files</summary>
		public string SourceFolder
		{
			get { return m_strSourceFolder; }
			set { m_strSourceFolder = value; }
			
		}
		
		#endregion Properties
		
	}// public class CTmaxReportManager

}// namespace FTI.Trialmax.Reports
