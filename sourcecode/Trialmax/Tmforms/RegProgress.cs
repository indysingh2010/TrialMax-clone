using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Timers;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows users to show progress during time consuming operations</summary>
	public class CFRegProgress : System.Windows.Forms.Form
	{
		#region Private members

		/// <summary>Description text display</summary>
		private System.Windows.Forms.Label m_ctrlDescription;

		/// <summary>Status text display</summary>
		private System.Windows.Forms.Label m_ctrlStatus;

		/// <summary>Error message list label</summary>
		private System.Windows.Forms.Label m_ctrlErrorsLabel;
		
		/// <summary>Local timer used to close the form</summary>
		private System.Timers.Timer m_sysTimer = new System.Timers.Timer(500);
		
		/// <summary>Error message list</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlErrorMessages;
		
		/// <summary>Local member used to blocking loop in OnError() method</summary>
		private bool m_bIgnoreError = false;
		
		/// <summary>Local member bound to PauseOnError property</summary>
		private bool m_bPauseOnError = true;

		/// <summary>Local member bound to Finished property</summary>
		private bool m_bFinished = false;

		/// <summary>Local flag to indicate we are in a blocking state</summary>
		private bool m_bBlocking = false;

		/// <summary>Local member bound to Title property</summary>
		private string m_strTitle = "Progress";

		/// <summary>Local member bound to Errors property</summary>
		private long m_lErrors = 0;

		/// <summary>Local member bound to Conflicts property</summary>
		private long m_lConflicts = 0;

		/// <summary>Local member bound to Completed property</summary>
		private long m_lCompleted = 0;

		/// <summary>Local member bound to Maximum property</summary>
		private long m_lMaximum = 100;

		/// <summary>Column used to display database id in conflicts message control</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrlColumn m_tmaxAutoIdColumn = new CTmaxMessageCtrlColumn("Id");
		
		/// <summary>Column used to display desired MediaId in conflicts message control</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrlColumn m_tmaxDesiredColumn = new CTmaxMessageCtrlColumn("Desired");
		
		/// <summary>Column used to display resolved MediaId in conflicts message control</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrlColumn m_tmaxResolvedColumn = new CTmaxMessageCtrlColumn("Resolved");
		
		/// <summary>Local member bound to Description property</summary>
		private string m_strDescription = "";
		private Infragistics.Win.UltraWinProgressBar.UltraProgressBar m_ctrlProgressBar;
		private System.Windows.Forms.Button m_ctrlClose;
		private System.Windows.Forms.Button m_ctrlIgnore;
		private System.Windows.Forms.Label m_ctrlConflictsLabel;
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlConflicts;
		private System.Windows.Forms.Button m_ctrlIgnoreAll;
		private System.Windows.Forms.Label m_ctrlErrorPending;

		/// <summary>Local member bound to Status property</summary>
		private string m_strStatus = "";

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFRegProgress()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			Initialize();			
		}

		/// <summary>Overloaded constructor</summary>
		public CFRegProgress(string strTitle)
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			Title = strTitle;
			
			Initialize();			
		}

		/// <summary>This method handles all Error events received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Error event arguments</param>
		public void OnError(object objSender, CTmaxErrorArgs Args)
		{
			string	strOldStatus = "";
			bool	bResetStatus = false;
			
			try
			{
				//	Add the error message to the list
				if(m_ctrlErrorMessages != null)
					m_ctrlErrorMessages.Add(Args);
					
				//	Update the counter
				Errors = m_lErrors + 1;
				
				//	Show the Ignore button if we are going to pause
				if(m_bPauseOnError == true)
				{
					if((m_ctrlIgnore != null) && (m_ctrlIgnore.IsDisposed == false))
						m_ctrlIgnore.Enabled = true;
					if((m_ctrlIgnoreAll != null) && (m_ctrlIgnoreAll.IsDisposed == false))
						m_ctrlIgnoreAll.Enabled = true;
					if((m_ctrlErrorPending != null) && (m_ctrlErrorPending.IsDisposed == false))
						m_ctrlErrorPending.Visible = true;
						
					//	Write the error message to the status control
					strOldStatus = m_ctrlStatus.Text;
					m_ctrlStatus.Text = "Error: " + Args.Message;
					bResetStatus = true;

					FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONEXCLAMATION);
				}
				
				//	Block the calling thread if required
				while((m_bPauseOnError == true) && (m_bIgnoreError == false))
				{
					m_bBlocking = true;
					System.Threading.Thread.Sleep(250);
					Application.DoEvents();
				}
				
				//	Disable the Ignore buttons if we are going to pause
				if((m_ctrlIgnore != null) && (m_ctrlIgnore.IsDisposed == false)&& (m_ctrlIgnore.Visible == true))
				{
					m_ctrlIgnore.Enabled = false;
				}
				if((m_ctrlIgnoreAll != null) && (m_ctrlIgnoreAll.IsDisposed == false)&& (m_ctrlIgnoreAll.Visible == true))
				{
					m_ctrlIgnoreAll.Enabled = false;
				}
				if((m_ctrlErrorPending != null) && (m_ctrlErrorPending.IsDisposed == false)&& (m_ctrlErrorPending.Visible == true))
				{
					m_ctrlErrorPending.Visible = false;
				}
				
				if(bResetStatus == true)
					m_ctrlStatus.Text = strOldStatus;
				
			}
			catch (Exception exc)
			{
                Console.WriteLine(exc.ToString());
			}
			finally
			{
				//	Make sure the local flags get reset
				m_bBlocking = false;
				m_bIgnoreError = false;
			}
			
		}// OnError
		
		/// <summary>This method is called to make it appear as though the user canceled the operation</summary>
		public virtual void Cancel()
		{
			OnClickClose(this, null);			
		}
		
		/// <summary>This method is called to enable/disable the cancel button</summary>
		public void EnableCancel(bool bEnabled)
		{
			if((m_ctrlClose != null) && (m_ctrlClose.IsDisposed == false))
				m_ctrlClose.Enabled = bEnabled;		
		}
		
		/// <summary>This method is used to report a media id conflict</summary>
		/// <param name="lId">The record's database identifier</param>
		/// <param name="strDesired">The desired media id</param>
		/// <param name="strResolved">The resolved media id</param>
		public void OnConflict(long lId, string strDesired, string strResolved)
		{
			try
			{
				if(m_tmaxAutoIdColumn != null)
					m_tmaxAutoIdColumn.Text = lId.ToString();
				if(m_tmaxDesiredColumn != null)
					m_tmaxDesiredColumn.Text = strDesired;
				if(m_tmaxResolvedColumn != null)
					m_tmaxResolvedColumn.Text = strResolved;
					
				//	Add the error message to the list
                if (m_ctrlConflicts != null)
                    AddRec();
					
				//	Update the counter
				Conflicts = m_lConflicts + 1;				
				
			}
			catch(Exception exc) 
			{
                Console.WriteLine(exc.ToString());
			}
			
		}

        delegate void AddRecCallback();
        private void AddRec()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.m_ctrlConflicts.InvokeRequired)
            {
                AddRecCallback d = new AddRecCallback(AddRec);
                this.Invoke(d, new object[] {});
            }
            else
            {
                m_ctrlConflicts.Add();
            }
        }

		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>
		/// This method will close the form when the timer event is trapped
		/// </summary>
		/// <param name="source">The timer object firing the event</param>
		/// <param name="e">the event arguments</param>
		protected void OnCloseTimer(object source, ElapsedEventArgs e)
		{
			// Kill the timer
			m_sysTimer.Enabled = false;
				
			//	Set the appropriate dialog result
			if(m_bFinished == true)
				DialogResult = DialogResult.OK;
			else
				DialogResult = DialogResult.Cancel;
				
			//	Close the form
			this.Close();
		}	
			
		/// <summary>Called to initilize local class members</summary>
		protected void Initialize()
		{
			//	Add the columns to the Media Id conflicts message box
            m_ctrlConflicts.Columns.Add(m_tmaxAutoIdColumn);
			m_ctrlConflicts.Columns.Add(m_tmaxDesiredColumn);
			m_ctrlConflicts.Columns.Add(m_tmaxResolvedColumn);

            m_ctrlConflicts.Rebuild();

			m_sysTimer.Interval  = 500;
			m_sysTimer.AutoReset = true;
			m_sysTimer.Enabled   = false;
			m_sysTimer.Elapsed += new ElapsedEventHandler(OnCloseTimer);
		}

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		protected virtual void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFRegProgress));
			this.m_ctrlErrorMessages = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.m_ctrlDescription = new System.Windows.Forms.Label();
			this.m_ctrlStatus = new System.Windows.Forms.Label();
			this.m_ctrlErrorsLabel = new System.Windows.Forms.Label();
			this.m_ctrlProgressBar = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
			this.m_ctrlClose = new System.Windows.Forms.Button();
			this.m_ctrlIgnore = new System.Windows.Forms.Button();
			this.m_ctrlConflictsLabel = new System.Windows.Forms.Label();
			this.m_ctrlConflicts = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.m_ctrlIgnoreAll = new System.Windows.Forms.Button();
			this.m_ctrlErrorPending = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlErrorMessages
			// 
			this.m_ctrlErrorMessages.AddTop = false;
			this.m_ctrlErrorMessages.ClearOnDblClick = false;
			this.m_ctrlErrorMessages.Format = FTI.Trialmax.Controls.TmaxMessageFormats.ErrorArgs;
			this.m_ctrlErrorMessages.Location = new System.Drawing.Point(8, 228);
			this.m_ctrlErrorMessages.MaxRows = 0;
			this.m_ctrlErrorMessages.Name = "m_ctrlErrorMessages";
			this.m_ctrlErrorMessages.Size = new System.Drawing.Size(420, 88);
			this.m_ctrlErrorMessages.TabIndex = 1;
			// 
			// m_ctrlDescription
			// 
			this.m_ctrlDescription.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlDescription.Name = "m_ctrlDescription";
			this.m_ctrlDescription.Size = new System.Drawing.Size(420, 20);
			this.m_ctrlDescription.TabIndex = 2;
			this.m_ctrlDescription.Text = "Description";
			this.m_ctrlDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlDescription.UseMnemonic = false;
			// 
			// m_ctrlStatus
			// 
			this.m_ctrlStatus.Location = new System.Drawing.Point(8, 68);
			this.m_ctrlStatus.Name = "m_ctrlStatus";
			this.m_ctrlStatus.Size = new System.Drawing.Size(420, 36);
			this.m_ctrlStatus.TabIndex = 3;
			this.m_ctrlStatus.Text = "Status";
			this.m_ctrlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlStatus.UseMnemonic = false;
			// 
			// m_ctrlErrorsLabel
			// 
			this.m_ctrlErrorsLabel.Location = new System.Drawing.Point(8, 216);
			this.m_ctrlErrorsLabel.Name = "m_ctrlErrorsLabel";
			this.m_ctrlErrorsLabel.Size = new System.Drawing.Size(420, 12);
			this.m_ctrlErrorsLabel.TabIndex = 4;
			this.m_ctrlErrorsLabel.Text = "Errors:";
			this.m_ctrlErrorsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlErrorsLabel.UseMnemonic = false;
			// 
			// m_ctrlProgressBar
			// 
			this.m_ctrlProgressBar.Location = new System.Drawing.Point(8, 36);
			this.m_ctrlProgressBar.Name = "m_ctrlProgressBar";
			this.m_ctrlProgressBar.Size = new System.Drawing.Size(420, 20);
			this.m_ctrlProgressBar.TabIndex = 6;
			this.m_ctrlProgressBar.Text = "[Formatted]";
			// 
			// m_ctrlClose
			// 
			this.m_ctrlClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlClose.Location = new System.Drawing.Point(348, 324);
			this.m_ctrlClose.Name = "m_ctrlClose";
			this.m_ctrlClose.TabIndex = 0;
			this.m_ctrlClose.Text = "&Cancel";
			this.m_ctrlClose.Click += new System.EventHandler(this.OnClickClose);
			// 
			// m_ctrlIgnore
			// 
			this.m_ctrlIgnore.Enabled = false;
			this.m_ctrlIgnore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlIgnore.Location = new System.Drawing.Point(168, 324);
			this.m_ctrlIgnore.Name = "m_ctrlIgnore";
			this.m_ctrlIgnore.TabIndex = 5;
			this.m_ctrlIgnore.Text = "&Ignore";
			this.m_ctrlIgnore.Click += new System.EventHandler(this.OnClickIgnore);
			// 
			// m_ctrlConflictsLabel
			// 
			this.m_ctrlConflictsLabel.Location = new System.Drawing.Point(8, 112);
			this.m_ctrlConflictsLabel.Name = "m_ctrlConflictsLabel";
			this.m_ctrlConflictsLabel.Size = new System.Drawing.Size(392, 12);
			this.m_ctrlConflictsLabel.TabIndex = 8;
			this.m_ctrlConflictsLabel.Text = "MediaId Conflicts:";
			this.m_ctrlConflictsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlConflictsLabel.UseMnemonic = false;
			// 
			// m_ctrlConflicts
			// 
			this.m_ctrlConflicts.AddTop = false;
			this.m_ctrlConflicts.ClearOnDblClick = false;
			this.m_ctrlConflicts.Format = FTI.Trialmax.Controls.TmaxMessageFormats.UserDefined;
			this.m_ctrlConflicts.Location = new System.Drawing.Point(4, 124);
			this.m_ctrlConflicts.MaxRows = 0;
			this.m_ctrlConflicts.Name = "m_ctrlConflicts";
			this.m_ctrlConflicts.Size = new System.Drawing.Size(424, 88);
			this.m_ctrlConflicts.TabIndex = 7;
			// 
			// m_ctrlIgnoreAll
			// 
			this.m_ctrlIgnoreAll.Enabled = false;
			this.m_ctrlIgnoreAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlIgnoreAll.Location = new System.Drawing.Point(258, 324);
			this.m_ctrlIgnoreAll.Name = "m_ctrlIgnoreAll";
			this.m_ctrlIgnoreAll.TabIndex = 9;
			this.m_ctrlIgnoreAll.Text = "Ignore &All";
			this.m_ctrlIgnoreAll.Click += new System.EventHandler(this.OnClickIgnoreAll);
			// 
			// m_ctrlErrorPending
			// 
			this.m_ctrlErrorPending.BackColor = System.Drawing.Color.Red;
			this.m_ctrlErrorPending.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlErrorPending.ForeColor = System.Drawing.Color.White;
			this.m_ctrlErrorPending.Location = new System.Drawing.Point(12, 324);
			this.m_ctrlErrorPending.Name = "m_ctrlErrorPending";
			this.m_ctrlErrorPending.Size = new System.Drawing.Size(148, 23);
			this.m_ctrlErrorPending.TabIndex = 10;
			this.m_ctrlErrorPending.Text = "ERROR PENDING";
			this.m_ctrlErrorPending.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_ctrlErrorPending.Visible = false;
			// 
			// CFRegProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(434, 353);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlErrorPending);
			this.Controls.Add(this.m_ctrlIgnoreAll);
			this.Controls.Add(this.m_ctrlConflictsLabel);
            this.Controls.Add(this.m_ctrlConflicts);
			this.Controls.Add(this.m_ctrlProgressBar);
			this.Controls.Add(this.m_ctrlIgnore);
			this.Controls.Add(this.m_ctrlErrorsLabel);
			this.Controls.Add(this.m_ctrlStatus);
			this.Controls.Add(this.m_ctrlDescription);
			this.Controls.Add(this.m_ctrlErrorMessages);
			this.Controls.Add(this.m_ctrlClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(300, 248);
			this.Name = "CFRegProgress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Registration Progress";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}

		/// <summary>
		/// This method traps the event fired when the user clicks on the Close button
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		protected virtual void OnClickClose(object sender, System.EventArgs e)
		{
			//	Make sure we don't continue to block on error
			m_bPauseOnError = false;
			m_bIgnoreError = true;
			
			//	Hold up return until we unblock the OnError() caller
			if(m_bBlocking == true)
			{
				m_sysTimer.Enabled = true;
			}
			else
			{
				OnCloseTimer(null, null);
			}
			
		}
		
		/// <summary>
		/// This method traps the event fired when the user clicks on the Ignore button
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event argument object</param>
		protected virtual void OnClickIgnore(object sender, System.EventArgs e)
		{
			//	Set the local flag
			m_bIgnoreError = true;
		}
		
		/// <summary>
		/// This method traps the event fired when the user clicks on the Ignore All button
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event argument object</param>
		protected virtual void OnClickIgnoreAll(object sender, System.EventArgs e)
		{
			//	Set the local flags
			m_bIgnoreError  = true;
			m_bPauseOnError = false;
			
			//	Hide the ignore button since we are not going to pause on error
			if(m_ctrlIgnore != null)
				m_ctrlIgnore.Visible = false;
				
			//	Hide the ignore All button since we are not going to pause on error
			if(m_ctrlIgnoreAll != null)
				m_ctrlIgnoreAll.Visible = false;
		}
		
		/// <summary>This method is called when there is a change in progress</summary>
		public void OnProgressChanged()
		{
			try
			{
				//	Do we have a valid progress bar?
				if((m_ctrlProgressBar == null) || (m_ctrlProgressBar.IsDisposed == true)) return;
				
				//	Calculate the current progress
				if(m_lMaximum > 0)
                    UpdateProgress((Int32)((m_lCompleted * 100) / m_lMaximum));
				else
					UpdateProgress(0);
					
			}
			catch(Exception exc)
			{
                Console.WriteLine(exc.ToString());
			}
			
		}// OnProgressChanged()

        delegate void UpdateProgressCallback(int progress);
        private void UpdateProgress(int progress)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.m_ctrlProgressBar.InvokeRequired)
            {
                UpdateProgressCallback d = new UpdateProgressCallback(UpdateProgress);
                this.Invoke(d, new object[] {  progress });
            }
            else
            {
                m_ctrlProgressBar.Value = progress;
            }
        }

		
		/// <summary>
		/// This method traps the Load event 
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event arguments</param>
		protected virtual void OnLoad(object sender, System.EventArgs e)
		{
			//	Hide the ignore button if we are not going to pause on error
			if((m_ctrlIgnore != null) && (m_bPauseOnError == false))
				m_ctrlIgnore.Visible = false;
				
			//	Hide the ignore All button if we are not going to pause on error
			if((m_ctrlIgnoreAll != null) && (m_bPauseOnError == false))
				m_ctrlIgnoreAll.Visible = false;
				
			if((m_ctrlErrorPending != null) && (m_ctrlErrorPending.IsDisposed == false))
				m_ctrlErrorPending.Visible = false;
		}

		#endregion Protected Methods

		#region Properties
		
		/// <summary>Flag to indicate the operation has finished</summary>
		public bool Finished
		{
			get 
			{ 
				return m_bFinished; 
			}
			set 
			{ 
				m_bFinished = value;
				
				//	Change the text on the Close button
				if(m_ctrlClose != null)
				{
                    UpdateCloseButton();
				}
				
				//	Update the progress bar
				m_lCompleted = m_lMaximum;
				OnProgressChanged();
				
				FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_OK);
				
			}
		}

        delegate void UpdateCloseButtonCallback();
        private void UpdateCloseButton()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.m_ctrlClose.InvokeRequired)
            {
                UpdateCloseButtonCallback d = new UpdateCloseButtonCallback(UpdateCloseButton);
                this.Invoke(d, new object[] { });
            }
            else
            {
                m_ctrlClose.ImageIndex = m_bFinished ? 1 : 0;
                try
                {
                    m_ctrlClose.Text = m_bFinished ? "&OK" : "&Cancel";

                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
            }
        }
		
		/// <summary>Flag to indicate that the operation should be blocked when an error occurs</summary>
		public bool PauseOnError
		{
			get 
			{ 
				return m_bPauseOnError; 
			}
			set 
			{ 
				m_bPauseOnError = value;
			}
		}
		
		/// <summary>Description of the operation</summary>
		public string Description
		{
			get 
			{ 
				return m_strDescription; 
			}
			set 
			{ 
				m_strDescription = value;
				
				if(m_ctrlDescription != null)
					m_ctrlDescription.Text = m_strDescription;
			}
		}
		
		/// <summary>Status of the operation</summary>
		public string Status
		{
			get 
			{ 
				return m_strStatus; 
			}
			set 
			{ 
				m_strStatus = value;
                UpdateStatus();
                //	Make sure all use input is being processed
                Application.DoEvents();
				
			}
		}

        delegate void UpdateStatusCallback();
        private void UpdateStatus()
        {
            if (m_ctrlStatus.InvokeRequired)
            {
                UpdateStatusCallback d = new UpdateStatusCallback(UpdateStatus);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (m_ctrlStatus != null)
                {
                    //	Set the text and force the redraw
                    m_ctrlStatus.Text = m_strStatus;
                    m_ctrlStatus.Refresh();
                }
            }
        }
		
		/// <summary>Title of the operation</summary>
		public string Title
		{
			get 
			{ 
				return m_strTitle; 
			}
			set 
			{ 
				m_strTitle = value;
				this.Text = m_strTitle;
			}
		}
		
		/// <summary>Total number of errors</summary>
		public long Errors
		{
			get 
			{ 
				return m_lErrors; 
			}
			set 
			{ 
				m_lErrors = value;
				
				if(m_ctrlErrorsLabel != null)
				{
					if(m_lErrors == 0)
						m_ctrlErrorsLabel.Text = "No errors:";
					else if(m_lErrors == 1)
						m_ctrlErrorsLabel.Text = "1 error:";
					else
						m_ctrlErrorsLabel.Text = m_lErrors.ToString() + " errors:";
				
					if(m_lErrors > 0)
					{
						m_ctrlErrorsLabel.BackColor = System.Drawing.Color.Red;
						m_ctrlErrorsLabel.ForeColor = System.Drawing.Color.White;
					}
					else
					{
						m_ctrlErrorsLabel.BackColor = System.Drawing.SystemColors.Control;
						m_ctrlErrorsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
					}
				
				}
			
			}
		}
		
		/// <summary>Total number of errors</summary>
		public long Conflicts
		{
			get 
			{ 
				return m_lConflicts; 
			}
			set 
			{ 
				m_lConflicts = value;
				
				if(m_ctrlConflictsLabel != null)
				{
                    UpdateConflict();
				}
			
			}
		}

        delegate void UpdateConflictCallback();
        void UpdateConflict()
        {
            if (m_ctrlConflictsLabel.InvokeRequired)
            {
                UpdateConflictCallback d = new UpdateConflictCallback(UpdateConflict);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (m_lConflicts == 0)
                    m_ctrlConflictsLabel.Text = "MediaId Conflicts:";
                else if (m_lConflicts == 1)
                    m_ctrlConflictsLabel.Text = "1 MediaId Conflict:";
                else
                    m_ctrlConflictsLabel.Text = m_lConflicts.ToString() + " MediaId Conflicts:";
            }
        }
		
		/// <summary>Maximum value used to calculate percent complete</summary>
		public long Maximum
		{
			get 
			{ 
				return m_lMaximum; 
			}
			set 
			{ 
				m_lMaximum = value;
				
				//	Update the progress bar
				OnProgressChanged();
			
			}
		}
		
		/// <summary>Completed value used to calculate percent complete</summary>
		public long Completed
		{
			get 
			{ 
				return m_lCompleted; 
			}
			set 
			{ 
				m_lCompleted = value;
				
				//	Update the progress bar
				OnProgressChanged();
			
			}
		}
		
		#endregion Properties
		
	}// class CFProgress
	
}// namespace FTI.Trialmax.Forms
