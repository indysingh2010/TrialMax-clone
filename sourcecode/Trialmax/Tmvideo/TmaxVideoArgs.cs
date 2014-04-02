using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This is the delegate used to handle all TrialMax Video Viewer events</summary>
	/// <param name="O">Object firing the event</param>
	/// <param name="Args">Object containing the event arguments</param>
	public delegate void TmaxVideoHandler(object O, CTmaxVideoArgs Args);
		
	/// <summary>This class is used to pass arguements in a TrialMax Video Viewer event</summary>
	public class CTmaxVideoArgs : System.EventArgs
	{
		#region Private Members
			
		/// <summary>Collection of CTmaxItems objects associated with the event</summary>
		private CTmaxItems m_aItems = null;
			
		/// <summary>Event item to represent the results of the operation</summary>
		private CTmaxItem m_tmaxResult = null;
			
		/// <summary>Collection of CTmaxCommandParamater objects associated with the event</summary>
		private CTmaxParameters m_aParameters = null;
			
		/// <summary>Local member accessed by View property</summary>
		private TmaxVideoViews m_eView = TmaxVideoViews.MaxViews;
			
		/// <summary>Local member accessed by Successful property</summary>
		private bool m_bSuccessful = false;
			
		/// <summary>Local member accessed by Command property</summary>
		private TmaxVideoCommands m_eCommand;
			
		#endregion Private Members
			
		#region Public Methods
			
		/// <summary>Default constructor</summary>
		public CTmaxVideoArgs()
		{
		}
			
		/// <summary>Overloaded constructor</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="eView">The id of the view sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public CTmaxVideoArgs(TmaxVideoCommands eCommand, TmaxVideoViews eView, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			Initialize(eCommand, eView, tmaxItems, tmaxParameters);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="eView">The id of the view sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		public CTmaxVideoArgs(TmaxVideoCommands eCommand, TmaxVideoViews eView, CTmaxItems tmaxItems)
		{
			Initialize(eCommand, eView, tmaxItems);
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="eView">The id of the view sending the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public CTmaxVideoArgs(TmaxVideoCommands eCommand, TmaxVideoViews eView, CTmaxParameters tmaxParameters)
		{
			Initialize(eCommand, eView, tmaxParameters);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="eView">The id of the view sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public void Initialize(TmaxVideoCommands eCommand, TmaxVideoViews eView, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			m_eCommand    = eCommand;
			m_eView       = eView;
			m_aItems      = tmaxItems;
			m_aParameters = tmaxParameters;
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="eView">The id of the source sending the event</param>
		/// <param name="tmaxItems">The collection of items assocaited with the event</param>
		public void Initialize(TmaxVideoCommands eCommand, TmaxVideoViews eView, CTmaxItems tmaxItems)
		{
			Initialize(eCommand, eView, tmaxItems, null);
		}
		
		/// <summary>Called to initialize the object properties using the specified values</summary>
		/// <param name="eCommand">The enumerated TrialMax command identifier</param>
		/// <param name="eView">The id of the source sending the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		public void Initialize(TmaxVideoCommands eCommand, TmaxVideoViews eView, CTmaxParameters tmaxParameters)
		{
			Initialize(eCommand, eView, null, tmaxParameters);
		}
		
		/// <summary>This method is called to get the parameter with the specified name</summary>
		/// <param name="strName">The name of the parameter</param>
		/// <returns>The parameter with the specified name if found</returns>
		public CTmaxParameter GetParameter(string strName)
		{
			CTmaxParameter tmaxParameter = null;
			
			if(m_aParameters != null)
				tmaxParameter = m_aParameters[strName];
				
			return tmaxParameter;
		}
		
		/// <summary>This method is called to get the parameter with the specified command enumeration</summary>
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
			
		/// <summary>The id assigned to the view firing the event</summary>
		public TmaxVideoViews View
		{
			get { return m_eView; }
			set { m_eView = value; }
		}

		/// <summary>The enumerated command identifier</summary>
		public TmaxVideoCommands Command
		{
			get { return m_eCommand; }
			set { m_eCommand = value; }
		}

		/// <summary>The property is set to indicate that the request was successfully processed</summary>
		public bool Successful
		{
			get { return m_bSuccessful; }
			set { m_bSuccessful = value; }
		}

		/// <summary>The collection of items representing the data associated with the event</summary>
		public CTmaxItems Items
		{
			get { return m_aItems; }
			set { m_aItems = value; }
		}
			
		/// <summary>The collection of parameters bound to the event</summary>
		public CTmaxParameters Parameters
		{
			get { return m_aParameters; }
			set { m_aParameters = value; }
		}
			
		/// <summary>An item to store the results of the operation</summary>
		public CTmaxItem Result
		{
			get { return m_tmaxResult; }
			set { m_tmaxResult = value; }
		}
			
		#endregion Properties
	
	}// public class CTmaxVideoArgs : System.EventArgs

}// namespace FTI.Trialmax.TMVV.Tmvideo
