using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Threading;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;


namespace FTI.Trialmax.TmaxInstaller
{
	/// <summary>This is the main form for the TrialMax updates installer application</summary>
	public class CTmaxInstallerForm : System.Windows.Forms.Form
	{
		#region Constants
		
		const string DEFAULT_XML_UPDATE_FILENAME  = "_tmax_updates.xml";
		const string DEFAULT_APPLICATION_FILENAME = "tmaxManager.exe";
		
		const int ERROR_OPEN_XML_PRODUCT_UPDATE_EX	= 0;
		const int ERROR_FILL_UPDATES_EX				= 1;
		const int ERROR_INSTALLATION_NOT_FOUND		= 2;
		const int ERROR_INSTALLATION_EXECUTE_FAILED	= 3;
		const int ERROR_INSTALL_UPDATE_EX			= 4;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>The pushbutton used to close the application</summary>
		private System.Windows.Forms.Button m_ctrlClose;
		
		/// <summary>The pushbutton used to Install the selected updates</summary>
		private System.Windows.Forms.Button m_ctrlInstall;
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Application's error message builder</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member to store the fully qualified path to the XML product update file</summary>
		private string m_strXmlFileSpec = "";

		/// <summary>Local member to store the fully qualified path to the updates folder</summary>
		private string m_strUpdatesFolder = "";

		/// <summary>Local member to store the fully qualified path to the application folder</summary>
		private string m_strApplicationFolder = "";

		/// <summary>Local member to store the fully qualified path to the TmaxManager application</summary>
		private string m_strAppFileSpec = "";

		/// <summary>Local member to store the command line passed to the application</summary>
		private string [] m_aCommandLine = null;

		/// <summary>List view control to display all available updates</summary>
		private System.Windows.Forms.ListView m_ctrlUpdates;

		/// <summary>Text label for the updates list</summary>
		private System.Windows.Forms.Label m_ctrlUpdatesLabel;

		/// <summary>Description column in the updates list</summary>
		private System.Windows.Forms.ColumnHeader m_colDescription;
		private System.Windows.Forms.Label m_ctrlStatus;
		private System.Windows.Forms.ProgressBar m_ctrlProgressBar;

		/// <summary>Local member to store a reference to the active XML product update</summary>
		private FTI.Shared.Xml.CXmlProductUpdate m_xmlProductUpdate = null;

		/// <summary>Local flag to request launching of TmaxManager on exit</summary>
		private bool m_bRestart = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>The main entry point for the application</summary>
		[STAThread]
		static void Main(string[] args) 
		{
			Application.Run(new CTmaxInstallerForm(args));
		}

		/// <summary>Constructor</summary>
		public CTmaxInstallerForm(string[] args)
		{
			//	Save the command line
			m_aCommandLine = args;
			
			//	Connect to the local event source
			m_tmaxEventSource.Name = "TmaxInstaller Application";
			m_tmaxEventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			m_tmaxEventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
			
			//	Populate the error builder
			SetErrorStrings();
			
			//	Initialize the child controls
			InitializeComponent();
			
		}// public CTmaxInstallerForm(string[] args)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		
		}// protected override void Dispose( bool disposing )

		/// <summary>Overloaded base class member called when the form gets loaded</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			while(true)
			{
				//	Set the path to the XML product update
				if(SetXmlFileSpec() == false)
					break;
					
				//	Open the product update
				if(OpenXmlProductUpdate() == false)
					break;

				//	Populate the list view control
				FillUpdates();
				
				//	We're done
				break;
				
			}// while(true)
			
			//	Do we have any updates to be installed?
			if(m_ctrlUpdates.Items.Count > 0)
			{
				m_ctrlInstall.Enabled = (GetSelected() > 0);
			}
			else
			{
				MessageBox.Show("No product updates available for installation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				m_ctrlInstall.Enabled = false;
			}
			
			// Do the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method is called to determine how many items in the list view are checked</summary>
		/// <returns>the number of checked items</returns>
		private int GetSelected()
		{
			int iChecked = 0;
			
			if(m_ctrlUpdates == null) return 0;
			if(m_ctrlUpdates.IsDisposed == true) return 0;
			if(m_ctrlUpdates.Items == null) return 0;
			if(m_ctrlUpdates.Items.Count == 0) return 0;
			
			//	Get the selected updates
			foreach(ListViewItem O in m_ctrlUpdates.Items)
			{
				try
				{
					if(O.Checked == true)
						iChecked++;
						
					try { ((CXmlUpdate)(O.Tag)).Selected = O.Checked; }
					catch {};
					
				}
				catch
				{
				}
						
			}// foreach(ListViewItem O in m_ctrlUpdates.Items)
			
			return iChecked;
		
		}// private int GetSelected()
		
		/// <summary>This method is called when the user checks or unchecks one of the available updates in the list view</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the event argument object</param>
		private void OnUpdateItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			// Is the item being checked!
			//
			//	NOTE: We have to do it this way because the control doesn't actually change
			//		  the item's check state until after this event gets fired
			if(e.NewValue == CheckState.Checked)
			{
				m_ctrlInstall.Enabled = true;
			}
			else
			{
				//	We have to have more than one item checked
				if(GetSelected() > 1)
				{
					m_ctrlInstall.Enabled = true;
				}
				else
				{
					m_ctrlInstall.Enabled = false;
				}
			
			}// if(e.NewValue == CheckState.Checked)
			
		}// private void OnUpdateItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.m_ctrlClose = new System.Windows.Forms.Button();
			this.m_ctrlInstall = new System.Windows.Forms.Button();
			this.m_ctrlUpdates = new System.Windows.Forms.ListView();
			this.m_colDescription = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlUpdatesLabel = new System.Windows.Forms.Label();
			this.m_ctrlStatus = new System.Windows.Forms.Label();
			this.m_ctrlProgressBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// m_ctrlClose
			// 
			this.m_ctrlClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlClose.Location = new System.Drawing.Point(348, 192);
			this.m_ctrlClose.Name = "m_ctrlClose";
			this.m_ctrlClose.TabIndex = 2;
			this.m_ctrlClose.Text = "&Close";
			this.m_ctrlClose.Click += new System.EventHandler(this.OnClickClose);
			// 
			// m_ctrlInstall
			// 
			this.m_ctrlInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlInstall.Location = new System.Drawing.Point(264, 192);
			this.m_ctrlInstall.Name = "m_ctrlInstall";
			this.m_ctrlInstall.TabIndex = 1;
			this.m_ctrlInstall.Text = "&Install";
			this.m_ctrlInstall.Click += new System.EventHandler(this.OnClickInstall);
			// 
			// m_ctrlUpdates
			// 
			this.m_ctrlUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUpdates.CheckBoxes = true;
			this.m_ctrlUpdates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.m_colDescription});
			this.m_ctrlUpdates.FullRowSelect = true;
			this.m_ctrlUpdates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.m_ctrlUpdates.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlUpdates.Name = "m_ctrlUpdates";
			this.m_ctrlUpdates.Size = new System.Drawing.Size(424, 112);
			this.m_ctrlUpdates.TabIndex = 0;
			this.m_ctrlUpdates.View = System.Windows.Forms.View.Details;
			this.m_ctrlUpdates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnUpdateItemCheck);
			// 
			// m_colDescription
			// 
			this.m_colDescription.Text = "Description";
			this.m_colDescription.Width = 272;
			// 
			// m_ctrlUpdatesLabel
			// 
			this.m_ctrlUpdatesLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlUpdatesLabel.Name = "m_ctrlUpdatesLabel";
			this.m_ctrlUpdatesLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlUpdatesLabel.TabIndex = 3;
			this.m_ctrlUpdatesLabel.Text = "Available Updates";
			// 
			// m_ctrlStatus
			// 
			this.m_ctrlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStatus.Location = new System.Drawing.Point(8, 144);
			this.m_ctrlStatus.Name = "m_ctrlStatus";
			this.m_ctrlStatus.Size = new System.Drawing.Size(424, 16);
			this.m_ctrlStatus.TabIndex = 4;
			this.m_ctrlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlProgressBar
			// 
			this.m_ctrlProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlProgressBar.Location = new System.Drawing.Point(4, 168);
			this.m_ctrlProgressBar.Name = "m_ctrlProgressBar";
			this.m_ctrlProgressBar.Size = new System.Drawing.Size(428, 16);
			this.m_ctrlProgressBar.Step = 5;
			this.m_ctrlProgressBar.TabIndex = 5;
			// 
			// CTmaxInstallerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlClose;
			this.ClientSize = new System.Drawing.Size(440, 221);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlProgressBar);
			this.Controls.Add(this.m_ctrlStatus);
			this.Controls.Add(this.m_ctrlUpdatesLabel);
			this.Controls.Add(this.m_ctrlUpdates);
			this.Controls.Add(this.m_ctrlInstall);
			this.Controls.Add(this.m_ctrlClose);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CTmaxInstallerForm";
			this.Text = "TrialMax Update Installer";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.OnClosing);
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>Handles Click events fired by the Close button</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnClickClose(object sender, System.EventArgs e)
		{
			//	Close the dialog box
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickClose(object sender, System.EventArgs e)
	
		/// <summary>Called when all updates have been installed</summary>
		private void OnInstallComplete()
		{
			string strFileSpec = "";
			
			//	Check each of the product updates
			foreach(CXmlUpdate O in m_xmlProductUpdate.Updates)
			{
				//	Has this update been installed?
				if(O.Installed == true)
				{
					//	Build the path to this update's installation
					strFileSpec = GetInstallationFileSpec(O);
			
					//	Delete the installation file if it exists
					if(System.IO.File.Exists(strFileSpec) == true)
					{
						try { System.IO.File.Delete(strFileSpec); }
						catch {};
					}
			
				}// if(O.Installed == true)
				
			}// foreach(CXmlUpdate O in m_xmlProductUpdate.Updates)
			
			//	Rebuild the updates list (hopefully there won't be any)
			FillUpdates();
			
			m_ctrlStatus.Text = "Operation complete";
			try { m_ctrlProgressBar.Value = m_ctrlProgressBar.Maximum; }
			catch {}
			
			//	Enable the buttons
			m_ctrlClose.Enabled = true;
			m_ctrlInstall.Enabled = (GetSelected() > 0);
			
			//	Should we relaunch manager?
			if((m_strAppFileSpec.Length > 0) && (System.IO.File.Exists(m_strAppFileSpec) == true))
			{
				if(MessageBox.Show("The selected installations are complete. Would you like to restart TrialMax now?", "Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					m_bRestart = true;
					OnClickClose(m_ctrlClose, System.EventArgs.Empty);
				}
				
			}
			
		}// private void OnInstallComplete()
	
		/// <summary>Handles Click events fired by the Install button</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnClickInstall(object sender, System.EventArgs e)
		{
			//	Get the selected installations
			if(GetSelected() == 0)
			{
				MessageBox.Show("You must select one or more updates to install", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			else
			{
				//	Save the selected states to file in case we have to reboot
				SaveXmlProductUpdate();
			}
			
			m_ctrlInstall.Enabled = false;
			m_ctrlClose.Enabled = false;
			
			try
			{
				//	Run the registration in it's own thread
				Thread thread = new Thread(new ThreadStart(this.InstallThreadProc));
				thread.Start();
			}
			catch(System.Exception Ex)
			{
				MessageBox.Show(Ex.ToString());
			}
			
		}// private void OnClickInstall(object sender, System.EventArgs e)
	
		/// <summary>This method handles all Error events received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Error event arguments</param>
		private void OnError(object objSender, CTmaxErrorArgs Args)
		{
//			FTI.Trialmax.Forms.CFErrorMessage cfErrorMessage = null;
//			
//			//	Should we display the popup?
//			if(Args.Show == true)
//			{
//				User.MessageBeep(User.MB_ICONEXCLAMATION);
//				
//				cfErrorMessage = new FTI.Trialmax.Forms.CFErrorMessage();
//				cfErrorMessage.SetControls(Args);
//				cfErrorMessage.ShowDialog(this);
//			}

		}// private void OnError(object objSender, CTmaxErrorArgs Args)
		
		/// <summary>This method handles all Diagnostic events received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Diagnostic event arguments</param>
		private void OnDiagnostic(object objSender, CTmaxDiagnosticArgs Args)
		{
			MessageBox.Show(Args.Message, "Diagnostics");
		}
		
		/// <summary>Called to set the fully qualified path to the XML product update file</summary>
		/// <returns>true if successful</returns>
		private bool SetXmlFileSpec()
		{
			bool bSuccessful = false;
			
			//	Set the application folder
			m_strApplicationFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			if((m_strApplicationFolder.Length > 0) && (m_strApplicationFolder.EndsWith("\\") == false))
				m_strApplicationFolder += "\\";
				
			//	Did the user specify a filename on the command line?
			if((m_aCommandLine != null) && (m_aCommandLine.GetUpperBound(0) >= 0))
			{
				m_strXmlFileSpec = m_aCommandLine[0];
				
				//	Does the file exist?
				bSuccessful = System.IO.File.Exists(m_strXmlFileSpec);
			}
			else
			{
				//	Check for the default file
				m_strUpdatesFolder = (m_strApplicationFolder + "_tmax_updates\\");
				
				m_strXmlFileSpec = (m_strUpdatesFolder + DEFAULT_XML_UPDATE_FILENAME);
				
				if(System.IO.File.Exists(m_strXmlFileSpec) == false)
				{
					//	Try the default subfolder
					m_strUpdatesFolder += "_tmax_updates\\";
					m_strXmlFileSpec = (m_strUpdatesFolder + DEFAULT_XML_UPDATE_FILENAME);
					
					bSuccessful = System.IO.File.Exists(m_strXmlFileSpec);
				}
				else
				{
					bSuccessful = true;
				
				}// if(System.IO.File.Exists(m_strXmlFileSpec) == false)
				
			}// if((m_aCommandLine != null) && (m_aCommandLine.GetUpperBound(0) >= 0))	
					
			if(bSuccessful == true)
			{
				//	Set the path to the folder containing the updates
				m_strUpdatesFolder = System.IO.Path.GetDirectoryName(m_strXmlFileSpec);
				if((m_strUpdatesFolder.Length > 0) && (m_strUpdatesFolder.EndsWith("\\") == false))
					m_strUpdatesFolder += "\\";
			
			}// if(bSuccessful == true)
			else
			{
				//	Warn the user if unable to locate the configuration file
				MessageBox.Show("Unable to locate " + m_strXmlFileSpec + " to perform initialization", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			
			return bSuccessful;
			
		}// private bool SetXmlFileSpec()
		
		/// <summary>Called to open the XML product update</summary>
		/// <returns>true if successful</returns>
		private bool OpenXmlProductUpdate()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Allocate and intialize a new product update
				m_xmlProductUpdate = new CXmlProductUpdate();
				m_tmaxEventSource.Attach(m_xmlProductUpdate.EventSource);
				
				//	Open the file
				bSuccessful = m_xmlProductUpdate.FastFill(m_strXmlFileSpec, true);
				
				//	Make sure the updates are in sorted order
				if(m_xmlProductUpdate.Updates != null)
					m_xmlProductUpdate.Updates.Sort(true);
					
				if(m_xmlProductUpdate.AppFilename.Length > 0)
					m_strAppFileSpec = (m_strApplicationFolder + m_xmlProductUpdate.AppFilename);
				else
					m_strAppFileSpec = (m_strApplicationFolder + DEFAULT_APPLICATION_FILENAME);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenXmlProductUpdate", m_tmaxErrorBuilder.Message(ERROR_OPEN_XML_PRODUCT_UPDATE_EX, m_strXmlFileSpec), Ex);
			}
			
			return bSuccessful;
			
		}// private bool OpenXmlProductUpdate()
		
		/// <summary>Called to save the XML product update</summary>
		/// <returns>true if successful</returns>
		private bool SaveXmlProductUpdate()
		{
			bool bSuccessful = false;
			
			try
			{
				if(m_xmlProductUpdate != null)
					bSuccessful = m_xmlProductUpdate.Save();
				
			}
			catch
			{
			}
			
			return bSuccessful;
			
		}// private bool SaveXmlProductUpdate()
		
		/// <summary>This method is called to display an error message that we don't want to treat as a system error</summary>
		private void OnLocalError(string strMessage)
		{
			//	Display an error message
			MessageBox.Show(strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			
		}// private void OnLocalError(string strMessage)

		/// <summary>This method will populate the list of available updates</summary>
		/// <returns>true if at least one update is added to the list</returns>
		private bool FillUpdates()
		{
			ListViewItem	lvi = null;
			bool			bSuccessful = true;
			
			if(m_ctrlUpdates == null) return false;
			if(m_ctrlUpdates.IsDisposed == true) return false;
			if(m_ctrlUpdates.Items == null) return false;
			if(m_xmlProductUpdate == null) return false;
			if(m_xmlProductUpdate.Updates == null) return false;
			
			try
			{
				//	Clear the existing items
				m_ctrlUpdates.Items.Clear();
				
				foreach(CXmlUpdate O in m_xmlProductUpdate.Updates)
				{
					//	Should this update be made available for installation?
					if((O.Downloaded == true) && (O.Installed == false))
					{					
						//	Add a row to the list box
						lvi = new ListViewItem();
						lvi.Text = O.Description;
						lvi.Tag = O;
						lvi.Checked = O.Selected;
								
						m_ctrlUpdates.Items.Add(lvi);
					
					}// if(O.Downloaded == true)
					else
					{
						O.Selected = false;
					}

				}// foreach(CXmlUpdate O in m_xmlProductUpdate.Updates)
				
				if(m_ctrlUpdates.Items.Count > 0)
				{
					bSuccessful = true;

					//	Automatically resize the columns to fit the text
					SuspendLayout();
					m_ctrlUpdates.Columns[0].Width = -2;
					ResumeLayout();

				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillUpdates", m_tmaxErrorBuilder.Message(ERROR_FILL_UPDATES_EX), Ex);
			}
			
			//	Save the update file
			SaveXmlProductUpdate();
			
			return bSuccessful;
		
		}// private bool FillUpdates()
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while opening the XML product update: filename = %1");
			aStrings.Add("An exception was raised while populating the list of downloaded updates.");
			aStrings.Add("Unable to locate %1 to install %2");			
			aStrings.Add("Unable to execute %1 to install %2");
			aStrings.Add("An exception was raised while executing the installation for %1");

		}
		
		/// <summary>This method is called to install the specified update</summary>
		/// <param name="xmlUpdate">The XML update to be installed</param>
		/// <returns>true if successful</returns>
		private bool Install(FTI.Shared.Xml.CXmlUpdate xmlUpdate)
		{
			System.Diagnostics.Process	install = null;
			string						strFileSpec = "";
			bool						bSuccessful = false;
			
			//	Build the path to this update's installation
			strFileSpec = GetInstallationFileSpec(xmlUpdate);
			
			//	Make sure the file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				OnLocalError(m_tmaxErrorBuilder.Message(ERROR_INSTALLATION_NOT_FOUND, strFileSpec, xmlUpdate.Description));
				return false;
			}
			
			try
			{
				//	Reset the progress bar
				try { m_ctrlProgressBar.Value = m_ctrlProgressBar.Minimum; }
				catch {}
				
				//	Set the status bar text
				m_ctrlStatus.Text = ("Installing " + xmlUpdate.Description);
				m_ctrlStatus.Refresh();
				
				//	Allocate a new process to start the installation
				install = new System.Diagnostics.Process();
				
				//	Initialize the process block
				install.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
				install.StartInfo.FileName = strFileSpec;
				
				//	Start the process
				if(install.Start() == true)
				{
					bSuccessful = true;
					
					//	Update the XML file
					xmlUpdate.Installed = true;
					SaveXmlProductUpdate();
					
					//	Wait for the process to complete
					while(install.HasExited == false)
					{
						//	Step the progress bar
						try
						{
							if(m_ctrlProgressBar.Value + m_ctrlProgressBar.Step > m_ctrlProgressBar.Maximum)
								m_ctrlProgressBar.Value = m_ctrlProgressBar.Minimum;
							else
								m_ctrlProgressBar.PerformStep();
						
							m_ctrlProgressBar.Refresh();
							Application.DoEvents();
						}
						catch
						{
						}
						
							
						Thread.Sleep(500);
					}

					//	Operation is complete
					try { m_ctrlProgressBar.Value = m_ctrlProgressBar.Maximum; }
					catch {}
					m_ctrlStatus.Text = (xmlUpdate.Description + " complete");
					m_ctrlStatus.Refresh();
				
				}
				else
				{
					OnLocalError(m_tmaxErrorBuilder.Message(ERROR_INSTALLATION_EXECUTE_FAILED, strFileSpec, xmlUpdate.Description));
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Install", m_tmaxErrorBuilder.Message(ERROR_INSTALL_UPDATE_EX, xmlUpdate.Description), Ex);
			}
			
			return bSuccessful;
		
		}// private bool Install(CXmlUpdate xmlUpdate)
		
		/// <summary>This method is called to get the path to the installation for the specified update</summary>
		/// <param name="xmlUpdate">The XML update to be installed</param>
		/// <returns>the fully qualified path to the update's installation</returns>
		private string GetInstallationFileSpec(FTI.Shared.Xml.CXmlUpdate xmlUpdate)
		{
			string strFileSpec = "";
			
			//	Build the path to this update's installation
			strFileSpec = m_strUpdatesFolder;
			if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
				strFileSpec += "\\";
			strFileSpec += xmlUpdate.LocalFilename;
			
			return strFileSpec;
		
		}// private string GetInstallationFileSpec(CXmlUpdate xmlUpdate)
		
		private void InstallThreadProc()
		{
			//	Install each of the selected updates
			foreach(CXmlUpdate O in m_xmlProductUpdate.Updates)
			{
				if((O.Selected == true) && (O.Downloaded == true))
					Install(O);
			}
			
			//	Clean up
			OnInstallComplete();
			
		}
		
		/// <summary>This method is called to restart the host application</summary>
		private bool Restart()
		{
			System.Diagnostics.Process	manager = null;
			bool						bSuccessful = false;

			Debug.Assert(m_strAppFileSpec.Length > 0);
			if(m_strAppFileSpec.Length == 0) return false;
			if(System.IO.File.Exists(m_strAppFileSpec) == false) return false;
			
			try
			{
				//	Create the process for launching the converter
				manager = new Process();
				
				//	Initialize the startup information
				manager.StartInfo.FileName = m_strAppFileSpec;
				manager.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

				//	Start the process
				bSuccessful = manager.Start();

			}
			catch
			{
			}
			
			return bSuccessful;
			
		}// private bool Restart()
		
		/// <summary>Handles events fired when the form is closing</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//	Should we launch manager on exit
			if(m_bRestart == true)
			{
				Restart();
			}
			
		}// private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)

		#endregion Private Methods

	}// public class CTmaxInstallerForm : System.Windows.Forms.Form

}// namespace FTI.Trialmax.TmaxInstaller
