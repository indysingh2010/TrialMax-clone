using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// This class implements a Trialmax pane that can be used to display
	///	system messages
	/// </summary>
	public class CMessagePane : FTI.Trialmax.Panes.CBasePane
	{
		#region Error Identifiers
		
		const int ERROR_ADD_EX = (ERROR_BASE_PANE_MAX + 1);
		
		#endregion Error Identifiers
		
		#region Private Members

		/// <summary>Local member bound to Format property</summary>
		private TmaxMessageFormats m_eFormat = TmaxMessageFormats.UserDefined;
		
		/// <summary>Maximum number of rows contained in the list box</summary>
		private int m_iMaxRows = 0;
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlList;
		
		/// <summary>True to add messages to the top of the list</summary>
		private bool m_bAddTop = false;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.m_ctrlList = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlList
			// 
			this.m_ctrlList.AddTop = false;
			this.m_ctrlList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlList.Format = FTI.Trialmax.Controls.TmaxMessageFormats.UserDefined;
			this.m_ctrlList.MaxRows = 0;
			this.m_ctrlList.Name = "m_ctrlList";
			this.m_ctrlList.TabIndex = 0;

			// 
			// CMessagePane
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlList});
			this.Name = "CMessagePane";
			this.ResumeLayout(false);

		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CMessagePane() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();
			
			m_tmaxEventSource.Attach(m_ctrlList.EventSource);
		}
		
		/// <summary>
		/// This method allows the caller to add a row to the list box using the specified CBaseVersion object
		/// </summary>
		/// <param name="tmVersion">Version object containing the information needed to populate the row</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Version</remarks>
		public bool Add(FTI.Shared.CBaseVersion tmVersion)
		{	
			if(m_ctrlList == null) return false;
			
			try
			{
				return m_ctrlList.Add(tmVersion);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
				return false;
			}
		
		}// Add(FTI.Shared.CBaseVersion tmVersion)
		
		/// <summary>This method allows the caller to update a row to the list box using the specified CBaseVersion object</summary>
		/// <param name="tmVersion">Version object used to create the row in the list box</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Version</remarks>
		public bool Update(FTI.Shared.CBaseVersion tmVersion)
		{	
			if(m_ctrlList == null) return false;
			
			try
			{
				return m_ctrlList.Update(tmVersion);
			}
			catch
			{
				return false;
			}
		
		}// public bool Update(FTI.Shared.CBaseVersion tmVersion)
		
		/// <summary>
		/// This method allows the caller to add a row to the list box using the specified CTmaxErrorArgs object
		/// </summary>
		/// <param name="Args">Error argument object containing the error information</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Error</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		{	
			if(m_ctrlList == null) return false;
			
			try
			{
				return m_ctrlList.Add(Args);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
				return false;
			}
		
		}// Add(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		
		/// <summary>
		/// This method allows the caller to add a row to the list box using the specified CTmaxDiagnosticArgs object
		/// </summary>
		/// <param name="Args">Diagnostic event argument object</param>
		/// <returns>true if successful</returns>
		///	<remarks>It only makes sense to use this method if Format == Diagnostics</remarks>
		public bool Add(FTI.Shared.Trialmax.CTmaxDiagnosticArgs Args)
		{	
			if(m_ctrlList == null) return false;
			
			try
			{
				return m_ctrlList.Add(Args);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
				return false;
			}
		
		}// Add(FTI.Shared.Trialmax.CTmaxErrorArgs Args)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method traps events fired by the message control when the user clicks on an error message</summary>
		/// <param name="objSender">The object sending the events</param>
		/// <param name="Args">The original error message argument object</param>
		protected void OnDblClickError(object objSender, object Args)
		{
			CFErrorMessage cfErrorMsg = new CFErrorMessage();
		
			Debug.Assert(Args != null);
			Debug.Assert(Args.GetType() == typeof(FTI.Shared.Trialmax.CTmaxErrorArgs));
			if(Args == null) return;
			if(Args.GetType() != typeof(FTI.Shared.Trialmax.CTmaxErrorArgs)) return;
			
			try
			{
				//	Initialize the controls
				cfErrorMsg.SetControls((CTmaxErrorArgs)Args);
				
				cfErrorMsg.ShowDialog();
			}
			catch
			{
			}
		
		}

		/// <summary>Clean up all resources being used</BR></summary>
		protected override void Dispose(bool disposing)
		{
			if(m_ctrlList != null)
				m_ctrlList.Dispose();
				
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is called to populate the error builder's format string collection
		/// </summary>
		/// <remarks>The strings should be added to the collection in the same order in which they are enumerated</remarks>
		protected override void SetErrorStrings()
		{
			// Do the base class first
			base.SetErrorStrings();
			
			if((m_tmaxErrorBuilder != null) && (m_tmaxErrorBuilder.FormatStrings != null))
			{
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding a row");
			}
			
		}// SetErrorStrings()

		#endregion Protected Methods

		#region Properties

		/// <summary>This property indicates the maximum number of rows allowed</summary>
		public int MaxRows
		{
			get
			{
				return m_iMaxRows;
			}
			set
			{
				m_iMaxRows = value;
				
				if(m_ctrlList != null)
					m_ctrlList.MaxRows = m_iMaxRows;
			}
		
		}//	MaxRows property

		/// <summary>This property indicates if new items should be added to the top of the list
		///	instead of the bottom
		///</summary>
		public bool AddTop
		{
			get
			{
				return m_bAddTop;
			}
			set
			{
				m_bAddTop = value;
				
				if(m_ctrlList != null)
					m_ctrlList.AddTop = m_bAddTop;
			}
		
		}//	AddTop property

		/// <summary>
		///	This is the collection of columns used to build the pane
		///</summary>
		public FTI.Trialmax.Controls.CTmaxMessageCtrlColumns Columns
		{
			get
			{
				if(m_ctrlList != null)
					return m_ctrlList.Columns;
				else
					return null;
			}
		}//	Columns property

		/// <summary>
		///	This property sets up the columns in the local collection
		///</summary>
		public TmaxMessageFormats Format
		{
			get
			{
				return m_eFormat;
			}
			set
			{
				//	Has the value changed?
				if(value != m_eFormat)
				{
					m_eFormat = value;
				
					if(m_ctrlList != null)
						m_ctrlList.Format = m_eFormat;
				}
				
			}
			
		}//	Format property

		/// <summary>True to clear the contents on a double-click</summary>
		public bool ClearOnDblClick
		{
			get { return m_ctrlList.ClearOnDblClick; }
			set { m_ctrlList.ClearOnDblClick = value; }
		}
		
		#endregion Properties
		
	}
}
