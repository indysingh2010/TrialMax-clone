using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Panes
{
	/// <summary>This pane displays the results of a search operation</summary>
	public class CHelpPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants
		
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Tool tips for help links</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTip;

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Panel control to provide background appearance</summary>
		private System.Windows.Forms.Panel m_ctrlBackPanel;

		/// <summary>Link label to invoke the application's About box</summary>
		private System.Windows.Forms.LinkLabel m_ctrlAbout;

		/// <summary>Link label to invoke the application's contact information</summary>
		private System.Windows.Forms.LinkLabel m_ctrlContactInformation;

		/// <summary>Link label to invoke the application's user's manual</summary>
		private System.Windows.Forms.LinkLabel m_ctrlUsersManual;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CHelpPane() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}// public CHelpPane()
		
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

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_ctrlToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlAbout = new System.Windows.Forms.LinkLabel();
			this.m_ctrlContactInformation = new System.Windows.Forms.LinkLabel();
			this.m_ctrlUsersManual = new System.Windows.Forms.LinkLabel();
			this.m_ctrlBackPanel = new System.Windows.Forms.Panel();
			this.m_ctrlBackPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlAbout
			// 
			this.m_ctrlAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlAbout.Location = new System.Drawing.Point(10, 56);
			this.m_ctrlAbout.Name = "m_ctrlAbout";
			this.m_ctrlAbout.Size = new System.Drawing.Size(172, 20);
			this.m_ctrlAbout.TabIndex = 5;
			this.m_ctrlAbout.TabStop = true;
			this.m_ctrlAbout.Text = "About TrialMax";
			this.m_ctrlAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlToolTip.SetToolTip(this.m_ctrlAbout, "Show About");
			this.m_ctrlAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// m_ctrlContactInformation
			// 
			this.m_ctrlContactInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlContactInformation.Location = new System.Drawing.Point(10, 32);
			this.m_ctrlContactInformation.Name = "m_ctrlContactInformation";
			this.m_ctrlContactInformation.Size = new System.Drawing.Size(172, 20);
			this.m_ctrlContactInformation.TabIndex = 4;
			this.m_ctrlContactInformation.TabStop = true;
			this.m_ctrlContactInformation.Text = "Contact Information";
			this.m_ctrlContactInformation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlToolTip.SetToolTip(this.m_ctrlContactInformation, "View FTI Contact Information");
			this.m_ctrlContactInformation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// m_ctrlUsersManual
			// 
			this.m_ctrlUsersManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlUsersManual.Location = new System.Drawing.Point(10, 8);
			this.m_ctrlUsersManual.Name = "m_ctrlUsersManual";
			this.m_ctrlUsersManual.Size = new System.Drawing.Size(172, 20);
			this.m_ctrlUsersManual.TabIndex = 3;
			this.m_ctrlUsersManual.TabStop = true;
			this.m_ctrlUsersManual.Text = "User\'s Manual";
			this.m_ctrlUsersManual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlToolTip.SetToolTip(this.m_ctrlUsersManual, "Open the user\'s manual");
			this.m_ctrlUsersManual.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// m_ctrlBackPanel
			// 
			this.m_ctrlBackPanel.BackColor = System.Drawing.SystemColors.Window;
			this.m_ctrlBackPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_ctrlBackPanel.Controls.Add(this.m_ctrlAbout);
			this.m_ctrlBackPanel.Controls.Add(this.m_ctrlContactInformation);
			this.m_ctrlBackPanel.Controls.Add(this.m_ctrlUsersManual);
			this.m_ctrlBackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlBackPanel.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlBackPanel.Name = "m_ctrlBackPanel";
			this.m_ctrlBackPanel.Size = new System.Drawing.Size(192, 84);
			this.m_ctrlBackPanel.TabIndex = 0;
			// 
			// CHelpPane
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.m_ctrlBackPanel);
			this.Name = "CHelpPane";
			this.Size = new System.Drawing.Size(192, 84);
			this.m_ctrlBackPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		#endregion Protected Methods

		#region Private Members
		
		/// <summary>Called when one of the links is clicked</summary>
		/// <param name="sender">The link firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnLinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			CTmaxParameters tmaxParameters = null;
			
			try
			{
				LinkLabel linkLabel = (LinkLabel)sender;
				
				tmaxParameters = new CTmaxParameters();
				
				//	Which link is this?
				if(ReferenceEquals(linkLabel, m_ctrlUsersManual) == true)
				{
					tmaxParameters.Add(TmaxCommandParameters.HelpManual, true);
				}
				else if(ReferenceEquals(linkLabel, m_ctrlContactInformation) == true)
				{
					tmaxParameters.Add(TmaxCommandParameters.HelpContact, true);
				}	
				else if(ReferenceEquals(linkLabel, m_ctrlAbout) == true)
				{
					tmaxParameters.Add(TmaxCommandParameters.HelpAbout, true);
				}	
			
				//	Fire the Help command
				if(tmaxParameters.Count > 0)
				{
					FireCommand(TmaxCommands.Help, (CTmaxItems)null, tmaxParameters);
				}
				
				linkLabel.LinkVisited = true;
			
			}
			catch
			{
			
			}
			
		}// private void OnLinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		
		#endregion Private Members
		
	}// public class CHelpPane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
