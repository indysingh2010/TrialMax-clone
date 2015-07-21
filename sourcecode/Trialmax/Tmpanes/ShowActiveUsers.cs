using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Panes
{
	/// <summary>This form implements a wizard for importing load files from non-TrialMax applications</summary>
	public class CFShowActiveUsers : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
        private System.ComponentModel.IContainer components;

		/// <summary>Local member associated with the WizardOptions property</summary>
		protected CTmaxImportWizard m_tmaxWizardOptions = null;
		
		/// <summary>Local member associated with the RegisterOptions property</summary>
		protected CTmaxRegOptions m_tmaxRegisterOptions = null;
        
        /// <summary>Data grid for displaying recordset</summary>
        private DataGridView gvActiveUsers;

        /// <summary>Label for the form</summary>
        private Label lblTitle;

        /// <summary>Refresh button to update the datagrid</summary>
        private Button btnRefresh;

		///	<summary>Local member bound to Database property</summary>
		protected CTmaxCaseDatabase m_tmaxDatabase = null;
				
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFShowActiveUsers() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		
		}// public CFShowActiveUsers()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>Overridden base class member called when the form gets shown the first time</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Perform the base class processing
			base.OnLoad (e);

            LoadActiveUsersList();

		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFShowActiveUsers));
            this.gvActiveUsers = new System.Windows.Forms.DataGridView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvActiveUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // gvActiveUsers
            // 
            this.gvActiveUsers.AllowUserToAddRows = false;
            this.gvActiveUsers.AllowUserToDeleteRows = false;
            this.gvActiveUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvActiveUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvActiveUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvActiveUsers.Location = new System.Drawing.Point(12, 63);
            this.gvActiveUsers.Name = "gvActiveUsers";
            this.gvActiveUsers.ReadOnly = true;
            this.gvActiveUsers.Size = new System.Drawing.Size(600, 367);
            this.gvActiveUsers.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(111, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(316, 33);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Current Trialmax Case Users";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRefresh.Location = new System.Drawing.Point(438, 24);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // CFShowActiveUsers
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.gvActiveUsers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "CFShowActiveUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Show Active Users";
            ((System.ComponentModel.ISupportInitialize)(this.gvActiveUsers)).EndInit();
            this.ResumeLayout(false);

		}// private void InitializeComponent()

        /// <summary>Load list of current database connected users
        /// https://support.microsoft.com/en-us/kb/287655
        /// </summary>
        private void LoadActiveUsersList()
        {
            // Connect to the database
            if (Database == null || string.IsNullOrEmpty(Database.FileSpec)) return;

            List<ActiveUser> dbUsers = GetActiveUsersOfDatabase();

            // Group the list so that each machine appears only once in the list with their count
            var sortedData = (from user in dbUsers
                              group user by new
                             {
                                 user.ComputerName,
                                 user.Connected,
                             } into groupedData
                              select new ActiveUser()
                              {
                                  ComputerName = groupedData.Key.ComputerName,
                                  Connected = groupedData.Key.Connected,
                                  NumberOfConnections = groupedData.ToList().Count,
                              }).ToList();

            // There will be one additional entry with current Machine Name as this task will itself create a connection and we need to eliminate
            // this so that the user does not get confused with the extra counting.
            var currentUserInList = sortedData.FirstOrDefault(x => x.ComputerName.Equals(Environment.MachineName));
            if (currentUserInList != null)
            {
                currentUserInList.NumberOfConnections--;
            }

            // Setup the datagrid
            gvActiveUsers.DataSource = sortedData;
            gvActiveUsers.Columns[0].HeaderText = "Computer Name";
            gvActiveUsers.Columns[1].HeaderText = "Connected";
            gvActiveUsers.Columns[2].HeaderText = "Connections Count";
            gvActiveUsers.AutoResizeColumns();
        } // private void LoadActiveUsersList()

        /// <summary>
        /// Get Active Users connected to the Database
        /// </summary>
        /// <returns></returns>
        private List<ActiveUser> GetActiveUsersOfDatabase()
        {
            List<ActiveUser> dbUsers = new List<ActiveUser>();

            ADODB.Connection connection = new ADODB.Connection();
            ADODB.Recordset recordSet = null;

            try
            {
                connection.Provider = "Microsoft.Jet.OLEDB.4.0";
                connection.Open("Data Source=" + Database.FileSpec);
                try
                {
                    // Get records of current connected users
                    recordSet = connection.OpenSchema(ADODB.SchemaEnum.adSchemaProviderSpecific, Type.Missing, "{947bb102-5d43-11d1-bdbf-00c04fb92675}");

                    // Read the recordset and fill the list dbUsers
                    while (!recordSet.EOF)
                    {
                        dbUsers.Add(new ActiveUser
                        {
                            ComputerName = ((string)(recordSet.Fields[0].Value)).Trim().Replace("\0", string.Empty),
                            Connected = (bool)recordSet.Fields[2].Value
                        });
                        recordSet.MoveNext();
                    }
                }
                finally
                {
                    // Close open connection
                    if (recordSet != null)
                        recordSet.Close();
                }
            }
            finally
            {
                // Close open connection
                connection.Close();
            }

            return dbUsers;
        } // private List<ActiveUser> GetActiveUsersOfDatabase()

        /// <summary>
        /// Called when Refresh button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            gvActiveUsers.Columns.Clear();
            gvActiveUsers.DataSource = null;
            LoadActiveUsersList();
        } // private void BtnRefresh_Click(object sender, EventArgs e)

		#endregion Private Methods

        #region Public Properties

        /// <summary>Property to store the refernce of the opened Trialmax Databse</summary>
        public CTmaxCaseDatabase Database { get; set; }

        #endregion

	}// public class CFImportWizard : System.Windows.Forms.Form

    public class ActiveUser
    {
        public string ComputerName { get; set; }
        public bool Connected { get; set; }
        public int NumberOfConnections { get; set; }
    }

}// namespace FTI.Trialmax.Forms
