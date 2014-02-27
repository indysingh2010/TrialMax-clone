using System;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Win32;
using FTI.Shared.Xml;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.TmaxManager
{
	/// <summary>This class creates a simulator for TmaxManager</summary>
	public class CTmaxManagerSimulator
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_DATABASE_EX		= 0;
		private const int ERROR_INITIALIZE_EX		= 1;
		private const int ERROR_START_EX			= 2;
		private const int ERROR_STOP_EX				= 3;

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member used to implement the simulator's timer</summary>
		private System.Timers.Timer m_sysTimer = null;
		
		/// <summary>Local member bound to Database property</summary>
		private FTI.Trialmax.Database.CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member to store the records used in the simulation</summary>
		private CDxPrimaries m_dxPrimaries = null;
		
		/// <summary>Local member to store the timer interval</summary>
		private int m_iInterval = 500;
		
		/// <summary>Local member to store the index of the active record</summary>
		private int m_iIndex = -1;
		
		/// <summary>Local member bound to AppId property</summary>
		private int m_iAppId = 0;
		
		/// <summary>Local member bound to Active property</summary>
		private bool m_bActive = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This event is fired by the simulator to issue a command</summary>
		public event FTI.Shared.Trialmax.TmaxCommandHandler Command;
		
		/// <summary>Constructor</summary>
		public CTmaxManagerSimulator()
		{
			m_tmaxEventSource.Name = "Simulator";

			//	Populate the error builder's format string collection
			SetErrorStrings();
			
		}// public CTmaxManagerSimulator()
		
		/// <summary>Called to set the active database</summary>
		/// <param name="tmaxDatabase">the database to be used for the simulation</param>
		/// <returns>true if successful</returns>
		public bool SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Stop the active simulation
				Stop();
			
				//	Reset the record index
				m_iIndex = -1;
				m_tmaxDatabase = null;
				m_dxPrimaries = null;
				
				if((tmaxDatabase != null) && (tmaxDatabase.Primaries != null))
				{
					m_dxPrimaries = new CDxPrimaries(tmaxDatabase);
					
					foreach(CDxPrimary O in tmaxDatabase.Primaries)
					{
						if(O.MediaType == TmaxMediaTypes.Document)
							m_dxPrimaries.AddList(O);
					}
					
					if(m_dxPrimaries.Count > 0)
					{
						//	Store the new database reference
						m_tmaxDatabase = tmaxDatabase;
					}
					else
					{
						MessageBox.Show("No records available for simulation");
						m_dxPrimaries = null;
						return false;
					}
				
				}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDatabase", m_tmaxErrorBuilder.Message(ERROR_SET_DATABASE_EX), Ex);
			}
			
			return bSuccessful;
		
		}// public bool SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		
		/// <summary>This method is called by the application to initialize the simulator</summary>
		/// <returns>true if successful</returns>
		public bool Initialize(CXmlIni xmlINI)
		{
			bool bSuccessful = false;
			
			try
			{
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDatabase", m_tmaxErrorBuilder.Message(ERROR_SET_DATABASE_EX), Ex);
			}
			
			return bSuccessful;
		
		}// public bool Initialize(CXmlIni xmlINI)
		
		/// <summary>This method is called by the application when it is about to terminate</summary>
		public void Terminate(CXmlIni xmlINI)
		{
			SetDatabase(null);
		}
		
		/// <summary>This method is called by the application to start the simulation</summary>
		/// <returns>true if successful</returns>
		public bool Start()
		{
			bool bSuccessful = false;
			
			try
			{
				if(m_bActive == false)
				{
					//	Go active
					m_bActive = true;
					
					//	Activate the next record
					Next();
				
					//	Start the timer
					m_sysTimer = new System.Timers.Timer();
					m_sysTimer.Interval = m_iInterval;
					m_sysTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerElapsed);
					m_sysTimer.Start();
					
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Start", m_tmaxErrorBuilder.Message(ERROR_START_EX), Ex);
			}
			
			return bSuccessful;
		
		}
		
		/// <summary>This method is called by the application to stop the simulation</summary>
		public void Stop()
		{
			try
			{
				//	Simulator is no longer active
				m_bActive = false;

				//	Kill the timer
				if(m_sysTimer != null)
				{
					m_sysTimer.Stop();
					m_sysTimer = null;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Stop", m_tmaxErrorBuilder.Message(ERROR_STOP_EX), Ex);
			}
		
		}// public void Stop()
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="Args">Command argument object</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(CTmaxCommandArgs Args)
		{
			try
			{
				//	Is anybody registered?
				if(Command != null)
				{
					Command(this, Args);
					return Args.Successful;
				}
				else
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
			
		}// private bool FireCommand(CTmaxCommandArgs Args)
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxCommandArgs Args = null;
			
			try
			{
				// Get the command arguments
				if((Args = new CTmaxCommandArgs(eCommand, m_iAppId, tmaxItems, tmaxParameters)) != null)
				{
					return FireCommand(Args);
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireCommand", "Fire " + eCommand.ToString() + " command Ex: " + Ex.ToString());
			}
			
			return false;
		
		}//	private bool FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)
		{
			return FireCommand(eCommand, tmaxItems, null);
		}
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)
		{
			CTmaxItems tmaxItems = new CTmaxItems();
			
			tmaxItems.Add(tmaxItem);
			
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
		}
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)
		{
			return FireCommand(eCommand, tmaxItem, null);
		}

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand)
		{
			return FireCommand(eCommand, (CTmaxItems)null, (CTmaxParameters)null);
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the application's simulator.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to start the application's simulator.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to stop the application's simulator.");

		}// private void SetErrorStrings()

		/// <summary>This method is called to activate the next record</summary>
		public void Next()
		{
			CTmaxParameters tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
			
			if((m_bActive == true) && (m_dxPrimaries != null) && (m_dxPrimaries.Count > 0))
			{
				//	Increment the active index
				if((m_iIndex = m_iIndex + 1) >= m_dxPrimaries.Count)
					m_iIndex = 0;
					
				//	Create an event item for the record
				tmaxItem = new CTmaxItem(m_dxPrimaries[m_iIndex]);
				
				//	Create the parameter to request activation
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.SyncMediaTree, true));
				
				//	Fire the command event
				if(m_bActive == true)
					FireCommand(TmaxCommands.Activate, tmaxItem, tmaxParameters);
			}
			
		}// public void Next()
		
		/// <summary>Handles Elapsed events fired by the timer</summary>
		/// <param name="sender">the timer object firing the event</param>
		/// <param name="e">the event arguments</param>
		private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			//	Activate the next record
			if(m_bActive == true)
				Next();
		}

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source for TrialMax error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
			
		}
		
		/// <summary>The database used for the simulation</summary>
		public FTI.Trialmax.Database.CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { SetDatabase(value); }	
		}
		
		/// <summary>The Id used to fire application events</summary>
		public int AppId
		{
			get { return m_iAppId; }
			set { m_iAppId = value; }	
		}
		
		/// <summary>True if the simulator is active</summary>
		public bool Active
		{
			get { return m_bActive; }
		}
		
		#endregion Properties
	
	}// public class CTmaxManagerSimulator

}// namespace FTI.Trialmax.TmaxManager
