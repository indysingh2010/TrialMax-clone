using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Win32;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class manages a form that allows the user to add a value to a pick list</summary>
	public class CFAddPickValue : System.Windows.Forms.Form
	{
		/// <summary>This event is fired by a form to issue a command</summary>
		public event FTI.Shared.Trialmax.TmaxCommandHandler TmaxCommandEvent;
		
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_FIRE_COMMAND_EX	= 0;
		private const int ERROR_ADD_EX			= 1;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Edit box to enter the name of the new value</summary>
		private System.Windows.Forms.TextBox m_ctrlValue;
		
		/// <summary>Static text label for the Value edit box</summary>
		private System.Windows.Forms.Label m_ctrlValueLabel;
		
		/// <summary>Static text label for the pick list name</summary>
		private System.Windows.Forms.Label m_ctrlListLabel;
		
		/// <summary>Static text control to display the name of the pick list</summary>
		private System.Windows.Forms.Label m_ctrlList;
		
		/// <summary>Local member bound to Value property</summary>
		private string m_strValue = "";
		
		/// <summary>Local member bound to PickList property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickList = null;
		
		/// <summary>Local member bound to Added property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxAdded = null;
		
		/// <summary>Local member bound to PaneId property</summary>
		private int m_iPaneId = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFAddPickValue() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			//	Populate the error builder collection
			SetErrorStrings();
		}
		
		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>Called when the form window gets loaded<.
		/// <param name="e">The system event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Do we have a parent pick list?
			if(m_tmaxPickList != null)
			{
				m_ctrlList.Text = m_tmaxPickList.Name;
			}
			else
			{
				m_ctrlListLabel.Enabled = false;
				m_ctrlList.Enabled = false;
				m_ctrlValueLabel.Enabled = false;
				m_ctrlValue.Enabled = false;
			}
			
			//	Perform the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(m_tmaxErrorBuilder != null)
				{
					if(m_tmaxErrorBuilder.FormatStrings != null)
						m_tmaxErrorBuilder.FormatStrings.Clear();
					m_tmaxErrorBuilder = null;
				}
			}
			base.Dispose(disposing);
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the command event: command = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add %1 to %2");

		}// private override void SetErrorStrings()

		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		private bool Warn(string strMsg, System.Windows.Forms.Control ctrlSelect)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation);
			
			if(ctrlSelect != null)
				ctrlSelect.Focus();	
				
			return false; // allows for cleaner code						
		
		}// private void Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>Called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			CTmaxPickItem	tmaxValue = null;
			bool			bSuccessful = false;
			string			strMsg;
			
			Debug.Assert(m_tmaxPickList != null);
			Debug.Assert(m_tmaxPickList.Children != null);
			
			while(bSuccessful == false)
			{
				if(m_tmaxPickList == null) break;
				if(m_tmaxPickList.Children == null) break;
			
				//	Get the value name specified by the user
				m_strValue = m_ctrlValue.Text;
				if(m_strValue.Length == 0)
				{
					Warn("You must supply a name for the new value", m_ctrlValue);
					break;
				}
				
				//	Make sure there is not a duplicate value
				if((tmaxValue = m_tmaxPickList.Children.Find(m_strValue, !(m_tmaxPickList.CaseSensitive))) != null)
				{
					strMsg = String.Format("A value named {0} already exists in the pick list.", m_strValue);
					Warn(strMsg, m_ctrlValue);
					break;
				}
				
				//	Create an item to represent the new value
				if((m_tmaxAdded = Add(m_strValue)) == null)
					break;
				else
					m_strValue = m_tmaxAdded.Name; // Just in case...
				
				//	All done
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			//	Can we close the dialog
			if(bSuccessful == true)
			{
				DialogResult = DialogResult.OK;
				this.Close();
			}
			
		}// private void OnClickOK(object sender, System.EventArgs e)

		/// <summary>This method is called to add the specified value to the pick list</summary>
		/// <param name="tmaxPickList">The parent pick list</param>
		/// <param name="strValue">The value to be added to the list</param>
		/// <returns>The new pick list value</returns>
		public CTmaxPickItem Add(string strValue)
		{
			CTmaxCommandArgs	Args = null;
			CTmaxItem			tmaxParent = null;
			CTmaxPickItem		tmaxValue = null;
			
			Debug.Assert(m_tmaxPickList != null);
			if(m_tmaxPickList == null) return null;
			
			try
			{
				//	Create a new pick item value 
				tmaxValue = new CTmaxPickItem();
				tmaxValue.ParentId = m_tmaxPickList.UniqueId;
				tmaxValue.Parent = m_tmaxPickList;
				tmaxValue.Type = TmaxPickItemTypes.Value;
				tmaxValue.Name = strValue;
		
				//	Should we notify the database?
				if(TmaxCommandEvent != null)
				{
					//	Create an event item to identify the parent list
					tmaxParent = new CTmaxItem(m_tmaxPickList);
					
					//	Add an event item to identify the new value
					tmaxParent.SourceItems.Add(new CTmaxItem(tmaxValue));
					
					Args = FireCommand(TmaxCommands.Add, tmaxParent);
				
					if(Args.Successful == false)
						tmaxValue = null;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX, m_tmaxPickList.Name, strValue), Ex);
			}
			
			return tmaxValue;
			
		}// public CTmaxPickItem Add(string strValue)

		
		/// <summary>Called by form designer to initialize the child controls</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFAddPickValue));
			this.m_ctrlValue = new System.Windows.Forms.TextBox();
			this.m_ctrlValueLabel = new System.Windows.Forms.Label();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlListLabel = new System.Windows.Forms.Label();
			this.m_ctrlList = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlValue
			// 
			this.m_ctrlValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlValue.Location = new System.Drawing.Point(56, 40);
			this.m_ctrlValue.Name = "m_ctrlValue";
			this.m_ctrlValue.Size = new System.Drawing.Size(200, 20);
			this.m_ctrlValue.TabIndex = 0;
			this.m_ctrlValue.Text = "";
			// 
			// m_ctrlValueLabel
			// 
			this.m_ctrlValueLabel.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlValueLabel.Name = "m_ctrlValueLabel";
			this.m_ctrlValueLabel.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlValueLabel.TabIndex = 1;
			this.m_ctrlValueLabel.Text = "Name:";
			this.m_ctrlValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(176, 72);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.Location = new System.Drawing.Point(88, 72);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 1;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlListLabel
			// 
			this.m_ctrlListLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlListLabel.Name = "m_ctrlListLabel";
			this.m_ctrlListLabel.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlListLabel.TabIndex = 3;
			this.m_ctrlListLabel.Text = "List:";
			this.m_ctrlListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlList
			// 
			this.m_ctrlList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlList.Location = new System.Drawing.Point(56, 8);
			this.m_ctrlList.Name = "m_ctrlList";
			this.m_ctrlList.Size = new System.Drawing.Size(200, 20);
			this.m_ctrlList.TabIndex = 4;
			this.m_ctrlList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFAddPickValue
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(264, 101);
			this.Controls.Add(this.m_ctrlList);
			this.Controls.Add(this.m_ctrlListLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Controls.Add(this.m_ctrlValueLabel);
			this.Controls.Add(this.m_ctrlValue);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddPickValue";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Value";
			this.ResumeLayout(false);

		}// private void InitializeComponent()

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <returns>The argument object used to fire the command event</returns>
		private CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)
		{
			CTmaxCommandArgs	Args = null;
			CTmaxItems			tmaxItems = null;
			
			try
			{
				//	Is anybody registered?
				if(TmaxCommandEvent != null)
				{
					//	Create the items collection for the event
					tmaxItems = new CTmaxItems();
					tmaxItems.Add(tmaxItem);
				
					// Get the command arguments
					if((Args = new CTmaxCommandArgs(eCommand, m_iPaneId, tmaxItems, null)) != null)
					{
						Args.Successful = false;
					
						//	Fire the event
						TmaxCommandEvent(this, Args);
					}
			
				
				}// if(TmaxCommandEvent != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireCommand", m_tmaxErrorBuilder.Message(ERROR_FIRE_COMMAND_EX, eCommand), Ex);
				Args = null;
			}
			
			return Args;
		
		}//	private CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }

		}// EventSource property
		
		/// <summary>The ID of the pane that should be used for command events</summary>
		public int PaneId
		{
			get { return m_iPaneId; }
			set { m_iPaneId = value; }
		}
		
		/// <summary>The value added by the user</summary>
		public string Value
		{
			get { return m_strValue; }
			set { m_strValue = value; }
		}
		
		/// <summary>The pick list that owns the new value</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickList
		{
			get { return m_tmaxPickList; }
			set { m_tmaxPickList = value; }
		}
		
		/// <summary>The pick item added to the list</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem Added
		{
			get { return m_tmaxAdded; }
			set { m_tmaxAdded = value; }
		}
		
		#endregion Properties
		
	}// public class CFAddPickValue : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Controls
