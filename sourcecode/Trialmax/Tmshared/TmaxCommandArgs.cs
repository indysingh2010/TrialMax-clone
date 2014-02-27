using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTI.Shared.Trialmax
{
	/// <summary>This is the delegate used to handle all command events</summary>
	/// <param name="objSender">Object firing the event</param>
	/// <param name="Args">Object containing the command event arguments</param>
	public delegate void TmaxCommandHandler(object objSender, CTmaxCommandArgs Args);
		
	/// <summary>This class is used to pass arguements in a TrialMax command event</summary>
	public class CTmaxCommandArgs : System.EventArgs
	{
		#region Private Members
			
		/// <summary>Collection of CTmaxItems objects associated with the event</summary>
		private CTmaxItems m_aItems = null;
			
		/// <summary>Collection of CTmaxCommandParamater objects associated with the event</summary>
		private CTmaxParameters m_aParameters = null;
			
		/// <summary>Local member accessed by Source property</summary>
		private int m_iSource = -1;
			
		/// <summary>Local member accessed by Successful property</summary>
		private bool m_bSuccessful = false;
			
		/// <summary>Local member accessed by Command property</summary>
		private TmaxCommands m_eCommand;
			
		#endregion Private Members
			
		#region Public Methods
			
		/// <summary>Default constructor</summary>
		public CTmaxCommandArgs()
		{
		}
			
		/// <summary>Overloaded constructor</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="iSource">The id of the source sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public CTmaxCommandArgs(TmaxCommands eCommand, int iSource, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			Initialize(eCommand, iSource, tmaxItems, tmaxParameters);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="iSource">The id of the source sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		public CTmaxCommandArgs(TmaxCommands eCommand, int iSource, CTmaxItems tmaxItems)
		{
			Initialize(eCommand, iSource, tmaxItems);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="iSource">The id of the source sending the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public CTmaxCommandArgs(TmaxCommands eCommand, int iSource, CTmaxParameters tmaxParameters)
		{
			Initialize(eCommand, iSource, tmaxParameters);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="iSource">The id of the source sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public void Initialize(TmaxCommands eCommand, int iSource, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			m_eCommand    = eCommand;
			m_iSource     = iSource;
			m_aItems      = tmaxItems;
			m_aParameters = tmaxParameters;
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="iSource">The id of the source sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		public void Initialize(TmaxCommands eCommand, int iSource, CTmaxItems tmaxItems)
		{
			Initialize(eCommand, iSource, tmaxItems, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="iSource">The id of the source sending the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public void Initialize(TmaxCommands eCommand, int iSource, CTmaxParameters tmaxParameters)
		{
			Initialize(eCommand, iSource, null, tmaxParameters);
		}
		
		/// <summary>
		/// This method is called to get the parameter with the specified name
		/// </summary>
		/// <param name="strName">The name of the parameter</param>
		/// <returns>The parameter with the specified name if found</returns>
		public CTmaxParameter GetParameter(string strName)
		{
			CTmaxParameter tmaxParameter = null;
			
			if(m_aParameters != null)
				tmaxParameter = m_aParameters[strName];
				
			return tmaxParameter;
		}
		
		/// <summary>
		/// This method is called to get the parameter with the specified command enumeration
		/// </summary>
		/// <param name="strName">The name of the parameter as determined by the command enumeration</param>
		/// <returns>The parameter with the specified name if found</returns>
		public CTmaxParameter GetParameter(TmaxCommandParameters eName)
		{
			CTmaxParameter tmaxParameter = null;
			
			if(m_aParameters != null)
				tmaxParameter = m_aParameters[eName];
				
			return tmaxParameter;
		}
		
		#endregion Public Methods

		#region Properties
			
		/// <summary>The property exposes the id of the event source
		public int Source
		{
			get
			{
				return m_iSource;
			}
			set
			{
				m_iSource = value;
			}
			
		} // Source property

		/// <summary>The property exposes the event command identifier</summary>
		public TmaxCommands Command
		{
			get
			{
				return m_eCommand;
			}
			set
			{
				m_eCommand = value;
			}
			
		} // Command property

		/// <summary>The property is set to indicate that the request was successfully processed</summary>
		public bool Successful
		{
			get
			{
				return m_bSuccessful;
			}
			set
			{
				m_bSuccessful = value;
			}
			
		} // Successful property

		/// <summary>
		/// Property that exposes the CExplorerPaneItems collection
		/// </summary>
		public CTmaxItems Items
		{
			get
			{
				return m_aItems;
			}
			set
			{
				m_aItems = value;
			}
		}
			
		/// <summary>This property exposes the parameters collection assocaited with the event</summary>
		public CTmaxParameters Parameters
		{
			get
			{
				return m_aParameters;
			}
			set
			{
				m_aParameters = value;
			}
			
		} // Parameters property
			
		#endregion Properties
	
	}// public class CTmaxCommandArgs : System.EventArgs

}// namespace FTI.Shared.Trialmax
