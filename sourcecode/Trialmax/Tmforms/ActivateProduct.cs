using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to activate the installation</summary>
	public class CFActivateProduct : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
        private System.ComponentModel.Container components = null;
		
		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Link label control to jump to activation site</summary>
		private System.Windows.Forms.LinkLabel m_ctrlSite;
		
		/// <summary>Label for control to display url of activation site</summary>
		private System.Windows.Forms.Label m_ctrlSiteLabel;

		/// <summary>Label for control to display activation seed</summary>
		private System.Windows.Forms.Label m_ctrlSeedLabel;

		/// <summary>Static text control to display activation seed value</summary>
		private System.Windows.Forms.Label m_ctrlSeed;

		/// <summary>Label for control to enter the activation key</summary>
		private System.Windows.Forms.Label m_ctrlKeyLabel;

		/// <summary>Control that allows user to enter the activation key</summary>
		private System.Windows.Forms.TextBox m_ctrlKey;

		/// <summary>Local member bound to Registry property</summary>
		private FTI.Shared.Trialmax.CTmaxRegistry m_tmaxRegistry = null;
		
		/// <summary>Local member bound to the Product property</summary>
		private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;
		
		/// <summary>Static text label to display application name and version</summary>
		private System.Windows.Forms.Label m_ctrlApplication;
		
		/// <summary>Label for application name and version control</summary>
		private System.Windows.Forms.Label m_ctrlApplicationLabel;
        private Button button1;
        private Button m_ctrlCancel;
        private Label lblExpirationMessage;

		/// <summary>Local member bound to ManagerOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxManagerOptions m_tmaxManagerOptions = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFActivateProduct()
		{
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)

		/// <summary>Overloaded member called when form window is created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if((m_tmaxRegistry != null) && (m_tmaxProductManager != null))
			{
                try
                {
                    string[] TmManagerVer = m_tmaxProductManager.TmaxManagerVersion.Split('.');
                    m_ctrlApplication.Text = String.Format("{0} ver. {1}", m_tmaxProductManager.Name,
                                            string.IsNullOrEmpty(m_tmaxProductManager.TmaxManagerVersion) ?
                                            m_tmaxProductManager.ShortVersion : (TmManagerVer.GetValue(0) + "." + TmManagerVer.GetValue(1) + "." + TmManagerVer.GetValue(2)));
                }
                catch
                {
                    m_ctrlApplication.Text = String.Format("{0} ver. {1}", m_tmaxProductManager.Name, m_tmaxProductManager.ShortVersion);
                }
                
				m_ctrlSeed.Text = m_tmaxProductManager.GetActivationSeed();
				m_ctrlSite.Text = m_tmaxProductManager.ActivateSite;
				m_ctrlSiteLabel.Text = "Click here to request a product activation key";

			}
			else
			{
				m_ctrlOk.Enabled = false;
				m_ctrlKeyLabel.Enabled = false;
				m_ctrlKey.Enabled = false;
			}
			
			base.OnLoad (e);
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFActivateProduct));
            this.m_ctrlOk = new System.Windows.Forms.Button();
            this.m_ctrlSite = new System.Windows.Forms.LinkLabel();
            this.m_ctrlSeedLabel = new System.Windows.Forms.Label();
            this.m_ctrlSeed = new System.Windows.Forms.Label();
            this.m_ctrlSiteLabel = new System.Windows.Forms.Label();
            this.m_ctrlKeyLabel = new System.Windows.Forms.Label();
            this.m_ctrlKey = new System.Windows.Forms.TextBox();
            this.m_ctrlApplication = new System.Windows.Forms.Label();
            this.m_ctrlApplicationLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.m_ctrlCancel = new System.Windows.Forms.Button();
            this.lblExpirationMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_ctrlOk
            // 
            this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlOk.Location = new System.Drawing.Point(184, 143);
            this.m_ctrlOk.Name = "m_ctrlOk";
            this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlOk.TabIndex = 1;
            this.m_ctrlOk.Text = "&OK";
            this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
            // 
            // m_ctrlSite
            // 
            this.m_ctrlSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSite.Location = new System.Drawing.Point(8, 87);
            this.m_ctrlSite.Name = "m_ctrlSite";
            this.m_ctrlSite.Size = new System.Drawing.Size(336, 20);
            this.m_ctrlSite.TabIndex = 9;
            this.m_ctrlSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickSite);
            // 
            // m_ctrlSeedLabel
            // 
            this.m_ctrlSeedLabel.Location = new System.Drawing.Point(7, 41);
            this.m_ctrlSeedLabel.Name = "m_ctrlSeedLabel";
            this.m_ctrlSeedLabel.Size = new System.Drawing.Size(84, 16);
            this.m_ctrlSeedLabel.TabIndex = 10;
            this.m_ctrlSeedLabel.Text = "Machine Name:";
            this.m_ctrlSeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlSeed
            // 
            this.m_ctrlSeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSeed.Location = new System.Drawing.Point(96, 41);
            this.m_ctrlSeed.Name = "m_ctrlSeed";
            this.m_ctrlSeed.Size = new System.Drawing.Size(248, 16);
            this.m_ctrlSeed.TabIndex = 11;
            this.m_ctrlSeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlSiteLabel
            // 
            this.m_ctrlSiteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSiteLabel.Location = new System.Drawing.Point(8, 67);
            this.m_ctrlSiteLabel.Name = "m_ctrlSiteLabel";
            this.m_ctrlSiteLabel.Size = new System.Drawing.Size(336, 16);
            this.m_ctrlSiteLabel.TabIndex = 12;
            this.m_ctrlSiteLabel.Text = "Site";
            // 
            // m_ctrlKeyLabel
            // 
            this.m_ctrlKeyLabel.Location = new System.Drawing.Point(8, 120);
            this.m_ctrlKeyLabel.Name = "m_ctrlKeyLabel";
            this.m_ctrlKeyLabel.Size = new System.Drawing.Size(32, 16);
            this.m_ctrlKeyLabel.TabIndex = 13;
            this.m_ctrlKeyLabel.Text = "Key:";
            this.m_ctrlKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlKey
            // 
            this.m_ctrlKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlKey.Location = new System.Drawing.Point(44, 116);
            this.m_ctrlKey.Name = "m_ctrlKey";
            this.m_ctrlKey.Size = new System.Drawing.Size(300, 20);
            this.m_ctrlKey.TabIndex = 0;
            this.m_ctrlKey.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.m_ctrlKey_PreviewKeyDown);
            this.m_ctrlKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_ctrlKey_KeyDown);
            // 
            // m_ctrlApplication
            // 
            this.m_ctrlApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlApplication.Location = new System.Drawing.Point(95, 21);
            this.m_ctrlApplication.Name = "m_ctrlApplication";
            this.m_ctrlApplication.Size = new System.Drawing.Size(248, 16);
            this.m_ctrlApplication.TabIndex = 15;
            this.m_ctrlApplication.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlApplicationLabel
            // 
            this.m_ctrlApplicationLabel.Location = new System.Drawing.Point(7, 21);
            this.m_ctrlApplicationLabel.Name = "m_ctrlApplicationLabel";
            this.m_ctrlApplicationLabel.Size = new System.Drawing.Size(84, 16);
            this.m_ctrlApplicationLabel.TabIndex = 14;
            this.m_ctrlApplicationLabel.Text = "Activate:";
            this.m_ctrlApplicationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(269, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = " &Cancel";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_ctrlCancel
            // 
            this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlCancel.Location = new System.Drawing.Point(268, 143);
            this.m_ctrlCancel.Name = "m_ctrlCancel";
            this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlCancel.TabIndex = 2;
            this.m_ctrlCancel.Text = " &Cancel";
            this.m_ctrlCancel.Visible = false;
            this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
            // 
            // lblExpirationMessage
            // 
            this.lblExpirationMessage.AutoSize = true;
            this.lblExpirationMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.lblExpirationMessage.ForeColor = System.Drawing.Color.Firebrick;
            this.lblExpirationMessage.Location = new System.Drawing.Point(7, 5);
            this.lblExpirationMessage.Name = "lblExpirationMessage";
            this.lblExpirationMessage.Size = new System.Drawing.Size(193, 17);
            this.lblExpirationMessage.TabIndex = 19;
            this.lblExpirationMessage.Text = "Your TrialMax activation has expired.";
            this.lblExpirationMessage.UseCompatibleTextRendering = true;
            this.lblExpirationMessage.Visible = false;
            this.lblExpirationMessage.Click += new System.EventHandler(this.lblExpirationMessage_Click);
            // 
            // CFActivateProduct
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(350, 174);
            this.Controls.Add(this.lblExpirationMessage);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.m_ctrlApplication);
            this.Controls.Add(this.m_ctrlApplicationLabel);
            this.Controls.Add(this.m_ctrlKey);
            this.Controls.Add(this.m_ctrlKeyLabel);
            this.Controls.Add(this.m_ctrlSiteLabel);
            this.Controls.Add(this.m_ctrlSeed);
            this.Controls.Add(this.m_ctrlSeedLabel);
            this.Controls.Add(this.m_ctrlSite);
            this.Controls.Add(this.m_ctrlCancel);
            this.Controls.Add(this.m_ctrlOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFActivateProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activate TrialMax";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void m_ctrlKey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
               VerifyActivationKey();
        }
        // private void InitializeComponent()

        /// <summary>This method handles the Cancel button's Click event</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event arguments</param>
        private void OnClickCancel(object sender, System.EventArgs e)
        {
            this.Close();
        }

	    /// <summary>This method handles the OK button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
	        VerifyActivationKey();
		
		}
        
        private void VerifyActivationKey()
        {
            //	Did the caller provide an activation key
            if (m_ctrlKey.Text.Length == 0)
            {
                MessageBox.Show("You must provide an activation key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //	Is this the correct key?
            if (m_tmaxProductManager.VerifyActivationCode(m_ctrlKey.Text) == false)
            {
                MessageBox.Show(m_ctrlKey.Text + " is not a valid activation key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                m_ctrlKey.Focus();
                m_ctrlKey.SelectAll();
                return;
            }

            //	Update the registry
            if(m_tmaxProductManager.SetActivationCode(m_ctrlKey.Text, false)==false)
            {
                MessageBox.Show("You must have Administrative privilege to run this applicatoin.","Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              //  return;
            }

            //	Close the dialog
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        // private void OnClickOK(object sender, System.EventArgs e)
		
		/// <summary>This method handles events fired when the user clicks on the activation site control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event arguments</param>
		private void OnClickSite(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(m_tmaxProductManager.ActivationLink);
				m_ctrlSite.LinkVisited = true;
			}
			catch
			{
			}
		
		}// private void OnClickSite(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The application's connection to the system registry</summary>
		public FTI.Shared.Trialmax.CTmaxRegistry Registry
		{
			get { return m_tmaxRegistry; }
			set { m_tmaxRegistry = value; }
		}
		
		/// <summary>The application's product descriptor</summary>
		public FTI.Shared.Trialmax.CTmaxProductManager Product
		{
			get { return m_tmaxProductManager; }
			set { m_tmaxProductManager = value; }
		}
		
		/// <summary>The options for TmaxManager application</summary>
		public FTI.Shared.Trialmax.CTmaxManagerOptions ManagerOptions
		{
			get { return m_tmaxManagerOptions; }
			set { m_tmaxManagerOptions = value; }
		}
		
		#endregion Properties

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void m_ctrlKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                m_ctrlKey.SelectAll();
            }
        }

        private void lblExpirationMessage_Click(object sender, EventArgs e)
        {

        }

	}// public class CFActivateProduct : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
