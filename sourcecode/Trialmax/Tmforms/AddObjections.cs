using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Shared.Win32;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFAddObjections : CFTmaxBaseForm
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_DEPOSITIONS_EX		= ERROR_TMAX_FORM_MAX + 1;
		private const int ERROR_SET_STATES_EX			= ERROR_TMAX_FORM_MAX + 2;
		private const int ERROR_CREATE_OBJECTION_EX		= ERROR_TMAX_FORM_MAX + 3;
		private const int ERROR_FIRE_ADD_COMMAND_EX		= ERROR_TMAX_FORM_MAX + 4;
		private const int ERROR_GET_LAST_DEPOSITION_EX	= ERROR_TMAX_FORM_MAX + 5;
		private const int ERROR_SET_RULINGS_EX			= ERROR_TMAX_FORM_MAX + 6;

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Control used to edit the objection properties</summary>
		private CTmaxObjectionEditorCtrl m_ctrlEditor;

		/// <summary>Local member bound to States property</summary>
		private IList m_IStates = null;

		/// <summary>Local member bound to Rulings property</summary>
		private IList m_IRulings = null;

		/// <summary>Private member bound to AllowMultiple property</summary>
		private bool m_bAllowMultiple = true;

		/// <summary>Private member bound to UseLastArgument property</summary>
		private bool m_bUseLastArgument = false;

		/// <summary>Private member bound to Objection property</summary>
		private CTmaxObjection m_tmaxObjection = null;

		/// <summary>Private member bound to StationOptions property</summary>
		private CTmaxStationOptions m_tmaxStationOptions = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CFAddObjections() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			m_tmaxEventSource.Name = "Add Objections";
			m_tmaxEventSource.Attach(m_ctrlEditor.EventSource);

		}// public CFAddObjections() : base()

		#endregion Public Methods

		#region Protected Methods

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Let the base class add its strings first
			base.SetErrorStrings();

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the list of depositions.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the list of status identifiers.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create an objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the command to add the objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the last deposition used to create an objection");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the list of ruling identifiers.");

		}// protected override void SetErrorStrings()

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

		}// protected override void Dispose( bool disposing )

		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;

			//	Initialize all the child controls
			while(bSuccessful == false)
			{
				//	Did the owner supply an objection?
				if(this.Objection != null)
				{
					if(m_tmaxStationOptions != null)
					{
						//	Always assign the last Side
						this.Objection.Plaintiff = m_tmaxStationOptions.AddObjectionPlaintiff;
						
						//	Should we assign a deposition
						if(this.Objection.ICaseDeposition == null)
							this.Objection.ICaseDeposition = GetLastDeposition();

						//	Should we assign the argument used to create the last objection?
						if((this.UseLastArgument == true) && (this.Objection.Argument.Length == 0))
							this.Objection.Argument = m_tmaxStationOptions.AddObjectionArgument;
							
					}// if(m_tmaxStationOptions != null)
						
					m_ctrlEditor.Objection = this.Objection;
				}
				else
				{
					//	Create and assign a new objection to be edited
					if(CreateObjection() == false)
						break;
				}

				//	Are we allowing multiple objections?
				if(m_bAllowMultiple == true)
				{
					m_ctrlOk.Text = "&Add";
					m_ctrlCancel.Text = "&Done";
				}
				else
				{
					m_ctrlOk.Text = "&OK";
					m_ctrlCancel.Text = "&Cancel";
				}

				bSuccessful = true;

			}// while(bSuccessful == false)

			base.OnLoad(e);

			if(bSuccessful == true)
			{
				m_ctrlEditor.SetFocus(false);
			}
			else
			{
				m_ctrlOk.Enabled = false;
				m_ctrlEditor.Enabled = false;
			}

		}// private void OnLoad(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Private Methods

		/// <summary>This method is called to set the editor's list of depositions</summary>
		/// <param name="IStates">The list of objection depositions</param>
		/// <returns>true if successful</returns>
		private bool SetStates(IList IStates)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlEditor.States = IStates;
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetStates", m_tmaxErrorBuilder.Message(ERROR_SET_STATES_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetStates(IList IStates)

		/// <summary>This method is called to set the editor's list of depositions</summary>
		/// <param name="IRulings">The list of objection depositions</param>
		/// <returns>true if successful</returns>
		private bool SetRulings(IList IRulings)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlEditor.Rulings = IRulings;
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetRulings", m_tmaxErrorBuilder.Message(ERROR_SET_RULINGS_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetRulings(IList IRulings)

		/// <summary>This method is called to set the editor's list of depositions</summary>
		/// <param name="IDepositions">The list of objection depositions</param>
		/// <returns>true if successful</returns>
		private bool SetDepositions(IList IDepositions)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlEditor.Depositions = IDepositions;
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDepositions", m_tmaxErrorBuilder.Message(ERROR_SET_DEPOSITIONS_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetDepositions(IList IDepositions)

		/// <summary>Called to get the last deposition used to create an objection</summary>
		/// <returns>The last deposition used to create an objection</returns>
		private ITmaxDeposition GetLastDeposition()
		{
			ITmaxDeposition tmaxDeposition = null;
			
			try
			{
				//	Do we have the station options and depositions?
				if((m_tmaxStationOptions != null) && (m_ctrlEditor.Depositions != null))
				{
					//	Locate the last deposition
					if(m_tmaxStationOptions.AddObjectionDeposition.Length > 0)
					{
						foreach(ITmaxDeposition O in m_ctrlEditor.Depositions)
						{
							if(String.Compare(O.GetMediaId(), m_tmaxStationOptions.AddObjectionDeposition, true) == 0)
							{
								tmaxDeposition = O;
								break;
							}
							
						}

					}// if(m_tmaxStationOptions.AddObjectionDeposition.Length > 0)
	
				}// if((m_tmaxStationOptions != null) && (m_IDepositions != null))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetLastDeposition", m_tmaxErrorBuilder.Message(ERROR_GET_LAST_DEPOSITION_EX), Ex);
			}
			
			return tmaxDeposition;

		}// private ITmaxDeposition GetLastDeposition()
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFAddObjections));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlEditor = new FTI.Trialmax.Controls.CTmaxObjectionEditorCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(488, 219);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "&Done";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(398, 219);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 1;
			this.m_ctrlOk.Text = "&Add";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlEditor
			// 
			this.m_ctrlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlEditor.Cancelled = false;
			this.m_ctrlEditor.Depositions = null;
			this.m_ctrlEditor.LastState = "2ADC3C97-D667-4E56-A84F-B9353B46F27E";
			this.m_ctrlEditor.Location = new System.Drawing.Point(2, 4);
			this.m_ctrlEditor.Name = "m_ctrlEditor";
			this.m_ctrlEditor.Objection = null;
			this.m_ctrlEditor.PaneId = 0;
			this.m_ctrlEditor.Rulings = null;
			this.m_ctrlEditor.Size = new System.Drawing.Size(577, 215);
			this.m_ctrlEditor.States = null;
			this.m_ctrlEditor.TabIndex = 0;
			// 
			// CFAddObjections
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(578, 252);
			this.Controls.Add(this.m_ctrlEditor);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddObjections";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Add Objections";
			this.ResumeLayout(false);

		}

		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			if(m_ctrlEditor.Objection != null)
			{
				//	Set the properties
				if(m_ctrlEditor.SetProps(m_ctrlEditor.Objection, false, false) == true)
				{
					//	Add to the database
					if(FireAddCommand(m_ctrlEditor.Objection) == true)
					{
						//	This is the latest objection
						m_tmaxObjection = m_ctrlEditor.Objection;
						
						//	Store the values for the next time
						if(m_tmaxStationOptions != null)
						{
							if(m_tmaxObjection.ICaseDeposition != null)
								m_tmaxStationOptions.AddObjectionDeposition = m_tmaxObjection.ICaseDeposition.GetMediaId();
							m_tmaxStationOptions.AddObjectionPlaintiff = m_tmaxObjection.Plaintiff;
							m_tmaxStationOptions.AddObjectionArgument = m_tmaxObjection.Argument;
						}
						
						//	Are we allowing multiple objections?
						if(m_bAllowMultiple == true)
						{
							//	Create and assign the next objection
							CreateObjection();
							
							m_ctrlEditor.SetFocus(true);
						}
						else
						{
							DialogResult = DialogResult.OK;
							this.Close();
						}

					}// if(FireAddCommand(m_ctrlEditor.Objection) == true)

				}// if(m_ctrlEditor.SetProps(m_ctrlEditor.Objection, false) == true)

			}// if(m_ctrlEditor.Objection != null)			

		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();

		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called to create a new objection to be edited</summary>
		private bool CreateObjection()
		{
			CTmaxObjection	tmaxObjection = null;
			bool			bSuccessful = false;

			try
			{
				tmaxObjection = new CTmaxObjection();

				//	Keep some of the values
				if(m_ctrlEditor.Objection != null)
				{
					tmaxObjection.ICaseDeposition = m_ctrlEditor.Objection.ICaseDeposition;
					tmaxObjection.IOxState = m_ctrlEditor.Objection.IOxState;
					tmaxObjection.Plaintiff = m_ctrlEditor.Objection.Plaintiff;
					tmaxObjection.FirstPL = m_ctrlEditor.Objection.FirstPL;
					tmaxObjection.LastPL = m_ctrlEditor.Objection.LastPL;
					tmaxObjection.Argument = m_ctrlEditor.Objection.Argument;
				}
				else
				{
					//	Are we supposed to be using the same argument as the last objection
					if((m_tmaxStationOptions != null) && (this.UseLastArgument == true))
						tmaxObjection.Argument = m_tmaxStationOptions.AddObjectionArgument;
				}
				
				if(m_tmaxStationOptions != null)
				{
					if(tmaxObjection.ICaseDeposition == null)
						tmaxObjection.ICaseDeposition = GetLastDeposition();
					tmaxObjection.Plaintiff = m_tmaxStationOptions.AddObjectionPlaintiff;
				}
				
				m_ctrlEditor.Objection = tmaxObjection;
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateObjection", m_tmaxErrorBuilder.Message(ERROR_CREATE_OBJECTION_EX), Ex);
			}

			return bSuccessful;

		}// private bool CreateObjection()

		/// <summary>This method is called to fire the command to add the objection to the database</summary>
		/// <param name="tmaxObjection">the objection to be added</param>
		/// <returns>true if the record was added successfully</returns>
		private bool FireAddCommand(CTmaxObjection tmaxObjection)
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;

			try
			{
				//	Allocate the event item to represent the new objection
				tmaxItem = new CTmaxItem();
				tmaxItem.DataType = TmaxDataTypes.Objection;
				if(tmaxItem.SourceItems == null)
					tmaxItem.SourceItems = new CTmaxItems();
				tmaxItem.SourceItems.Add(new CTmaxItem(tmaxObjection));
				
				//	Add the Objections parameter
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Objections, true);
				
				FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireAddCommand", m_tmaxErrorBuilder.Message(ERROR_FIRE_ADD_COMMAND_EX), Ex);
			}

			return (tmaxObjection.IOxObjection != null);

		}// private bool FireAddCommand(CTmaxObjection tmaxObjection)

		#endregion Private Methods

		#region Properties

		/// <summary>The application's station options</summary>
		public CTmaxStationOptions StationOptions
		{
			get { return m_tmaxStationOptions; }
			set { m_tmaxStationOptions = value; }
		}

		/// <summary>The active objection</summary>
		public CTmaxObjection Objection
		{
			get { return m_tmaxObjection; }
			set { m_tmaxObjection = value; }
		}

		/// <summary>The list of available depositions</summary>
		public IList Depositions
		{
			get { return m_ctrlEditor.Depositions; }
			set { SetDepositions(value); }
		}

		/// <summary>The list of available states</summary>
		public IList States
		{
			get { return m_IStates; }
			set { SetStates(value); }
		}

		/// <summary>The list of available rulings</summary>
		public IList Rulings
		{
			get { return m_IRulings; }
			set { SetRulings(value); }
		}

		/// <summary>true to allow multiple objections</summary>
		public bool AllowMultiple
		{
			get { return m_bAllowMultiple; }
			set { m_bAllowMultiple = value; }
		}

		/// <summary>true to initialize with the last argument</summary>
		public bool UseLastArgument
		{
			get { return m_bUseLastArgument; }
			set { m_bUseLastArgument = value; }
		}

		#endregion Properties

	}// public class CFAddObjections : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
