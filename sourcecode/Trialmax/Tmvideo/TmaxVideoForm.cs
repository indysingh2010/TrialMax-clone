using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Shared.Win32;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class manages a form that allows the user to add new video designations</summary>
	public class CFTmaxVideoForm : System.Windows.Forms.Form
	{
		#region Constants
		
		protected const int ERROR_TMAX_VIDEO_FORM_RESERVED_1	= 1;
		protected const int ERROR_TMAX_VIDEO_FORM_RESERVED_2	= 2;
		protected const int ERROR_TMAX_VIDEO_FORM_RESERVED_3	= 3;
		protected const int ERROR_TMAX_VIDEO_FORM_RESERVED_4	= 4;
		protected const int ERROR_TMAX_VIDEO_FORM_RESERVED_5	= 5;
		protected const int ERROR_TMAX_VIDEO_FORM_RESERVED_6	= 6;
		protected const int ERROR_TMAX_VIDEO_FORM_RESERVED_7	= 7;
		protected const int ERROR_TMAX_VIDEO_FORM_MAX			= 7;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to the View property</summary>
		private TmaxVideoViews m_eView = TmaxVideoViews.MaxViews;
		
		#endregion Protected Members
		
		#region Public Methods
		
		public event TmaxVideoHandler TmaxVideoCommandEvent;
		
		public CFTmaxVideoForm()
		{
			//	Populate the error builder's format strings
			m_tmaxEventSource.Name = "TmaxVideo Form";
			SetErrorStrings();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(m_tmaxErrorBuilder != null)
				{
					if(m_tmaxErrorBuilder.FormatStrings != null)
						m_tmaxErrorBuilder.FormatStrings.Clear();
					m_tmaxErrorBuilder = null;
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 1");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 2");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 3");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 4");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 5");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 6");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 7");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxVideoForm error string 8");

		}// protected virtual void SetErrorStrings()

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="Args">Command argument object</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(CTmaxVideoArgs Args)
		{
			try
			{
				//	Is anybody registered?
				if(TmaxVideoCommandEvent != null)
					TmaxVideoCommandEvent(this, Args);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireCommand", Ex);
			}
			
			return Args;
		}
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)
		{
			CTmaxItems tmaxItems = new CTmaxItems();
			
			tmaxItems.Add(tmaxItem);
			
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
		}

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxVideoArgs Args = null;
			
			try
			{
				// Get the command arguments
				if((Args = new CTmaxVideoArgs(eCommand, m_eView, tmaxItems, tmaxParameters)) != null)
				{
					return FireCommand(Args);
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireCommand", "Fire " + eCommand.ToString() + " command Ex: " + Ex.ToString());
			}
			
			return Args;
		
		}

		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		protected virtual bool Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation);
			
			if(ctrlSelect != null)
				Select(ctrlSelect);	
				
			return false; // allows for cleaner code						
		
		}// protected virtual void Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>This method is called to select the specified control</summary>
		/// <param name="ctrlTextBox">the desired text box control to be selected</param>
		protected virtual void Select(System.Windows.Forms.TextBox ctrlTextBox)
		{
			ctrlTextBox.Focus();
			ctrlTextBox.SelectAll();
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>The id assigned to the owner view</summary>
		public TmaxVideoViews View
		{
			get { return m_eView; }
			set { m_eView = value; }
		}
		
		#endregion Properties
		
	}// public class CFTmaxVideoForm : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.TMVV.Tmvideo
