using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class creates a grid-style control for viewing the terms in a TrialMax database filter</summary>
	public class CTmaxObjectionEditorCtrl : CTmaxBaseCtrl
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_OBJECTION_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_SET_DEPOSITIONS_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_SET_STATES_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_SET_CONTROL_STATES_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_SET_STATE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_SET_PROPS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 6;
		private const int ERROR_GET_STATE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 7;
		private const int ERROR_SET_DEPOSITION_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 8;
		private const int ERROR_GET_DEPOSITION_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 9;
		private const int ERROR_SET_RULINGS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 10;
		private const int ERROR_SET_RULING_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 11;
		private const int ERROR_GET_RULING_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 12;

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		protected System.ComponentModel.IContainer components = null;

		/// <summary>Drop list of available objection types</summary>
		private ComboBox m_ctrlStates;

		/// <summary>Static label for States drop list</summary>
		private Label m_ctrlStateLabel;

		/// <summary>Edit box for LastLine value</summary>
		private TextBox m_ctrlLastLine;

		/// <summary>Edit box for LastPage value</summary>
		private TextBox m_ctrlLastPage;

		/// <summary>Static label for Last page/line text entry boxes</summary>
		private Label m_ctrlLastPLLabel;

		/// <summary>Edit box for FirstLine value</summary>
		private TextBox m_ctrlFirstLine;

		/// <summary>Edit box for FirstPage value</summary>
		private TextBox m_ctrlFirstPage;

		/// <summary>Static label for First page/line text entry boxes</summary>
		private Label m_ctrlFirstPLLabel;

		/// <summary>Static label for Depositions drop list</summary>
		private Label m_ctrlDepositionLabel;

		/// <summary>Drop list of available depositions</summary>
		private ComboBox m_ctrlDepositions;

		/// <summary>Static label for deposition Id</summary>
		private Label m_ctrlDeposition;

		/// <summary>Edit box for Argument value</summary>
		private TextBox m_ctrlArgument;

		/// <summary>Static label for Argument text entry</summary>
		private Label m_ctrlArgumentLabel;

		/// <summary>Private member bound to Objection property</summary>
		private CTmaxObjection m_tmaxObjection = null;

		/// <summary>Private member bound to States property</summary>
		private IList m_aStates = null;

		/// <summary>Private member bound to Rulings property</summary>
		private IList m_aRulings = null;

		/// <summary>Radio button to set Plaintiff as the source of the objection</summary>
		private RadioButton m_ctrlPlaintiff;

		/// <summary>Radio button to set Defendant as the source of the objection</summary>
		private RadioButton m_ctrlDefendant;

		/// <summary>Edit box to enter the Response</summary>
		private TextBox m_ctrlResponse1;

		/// <summary>Static label for Response edit box</summary>
		private Label m_ctrlResponse1Label;

		/// <summary>Private member bound to Depositions property</summary>
		private IList m_aDepositions = null;

		/// <summary>Private member bound to LastState property</summary>
		private string m_strLastState = "";

		/// <summary>Drop list of available objection rulings</summary>
		private ComboBox m_ctrlRulings;

		/// <summary>Static label for rulings drop list</summary>
		private Label m_ctrlRulingsLabel;
		private SplitContainer splitContainer1;

		/// <summary>Private member bound to Cancelled property</summary>
		private bool m_bCancelled = false;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CTmaxObjectionEditorCtrl() : base()
		{
			m_tmaxEventSource.Name = "Objection Editor";
			
			// This call is required to initialize the child controls
			InitializeComponent();
		}

		/// <summary>Called to set the active objection</summary>
		/// <param name="tmaxObjection">The objection to be activated</param>
		/// <returns>true if successful</returns>
		public bool SetObjection(CTmaxObjection tmaxObjection)
		{
			bool bSuccessful = false;
			
			try
			{
				m_tmaxObjection = tmaxObjection;
				
				//	Update the child controls
				SetControlStates();

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetObjection", m_tmaxErrorBuilder.Message(ERROR_SET_OBJECTION_EX), Ex);
			}
			
			return bSuccessful;

		}// public bool SetObjection(CTmaxObjection tmaxObjection)

		/// <summary>Called to determine if the properties of the specified object match those of this object</summary>
		/// <param name="tmaxCompare">The object to be compared</param>
		public bool IsModified()
		{
			CTmaxObjection tmaxUser = null;

			//	Don't bother if nothing to modify
			if(m_tmaxObjection == null) return false;

			//	Allocate an objection to retrieve the user supplied values
			tmaxUser = new CTmaxObjection(m_tmaxObjection);

			//	Get the user supplied values
			if(SetProps(tmaxUser, true, false) == false)
				return true; // Something must have been modified

			//	Compare the user supplied values
			if(m_tmaxObjection.State != tmaxUser.State)
				return true;
			if(m_tmaxObjection.Ruling != tmaxUser.Ruling)
				return true;
			if(m_tmaxObjection.Plaintiff != tmaxUser.Plaintiff)
				return true;
			if(m_tmaxObjection.FirstPL != tmaxUser.FirstPL)
				return true;
			if(m_tmaxObjection.LastPL != tmaxUser.LastPL)
				return true;
			if(m_tmaxObjection.Argument != tmaxUser.Argument)
				return true;
			if(m_tmaxObjection.Response1 != tmaxUser.Response1)
				return true;
			if(m_tmaxObjection.Response2 != tmaxUser.Response2)
				return true;
			if(m_tmaxObjection.Response3 != tmaxUser.Response3)
				return true;
			if(m_tmaxObjection.Comments != tmaxUser.Comments)
				return true;
			if(m_tmaxObjection.RulingText != tmaxUser.RulingText)
				return true;
			if(m_tmaxObjection.WorkProduct != tmaxUser.WorkProduct)
				return true;

			//	Are we setting the deposition?
			if(m_ctrlDepositions.Visible == true)
			{
				if(m_tmaxObjection.ICaseDeposition != tmaxUser.ICaseDeposition)
					return true;
			}

			return false;

		}// public bool IsModified()

		/// <summary>Called to set the properties of the specified objection</summary>
		/// <param name="tmaxObjection">The objection to be updated</param>
		/// <param name="bSilent">true to suppress warning messages</param>
		/// <param name="bCanCancel">true if the user can cancel the operation</param>
		/// <returns>true if successful</returns>
		public bool SetProps(CTmaxObjection tmaxObjection, bool bSilent, bool bCanCancel)
		{
			bool bSuccessful = false;
			CTmaxObjection tmaxUser = null;
			long lPage = 0;
			int iLine = 0;

			try
			{
				Debug.Assert(tmaxObjection != null);

				//	Clear the flag that indicates cancelling the update
				m_bCancelled = false;
				
				//	Allocate an object to store the user values
				tmaxUser = new CTmaxObjection();

				while(bSuccessful == false)
				{
					//	Are we setting the deposition?
					if(m_ctrlDepositions.Visible == true)
					{
						//	Get the selected deposition
						if((tmaxUser.ICaseDeposition = GetDepositionSelection()) == null)
						{
							if(bSilent == false)
								OnPropError("You must select a deposition from the list", bCanCancel, null);
							break;
						}

					}// if(m_ctrlDepositions.Visible == true)
					
					//	Get the objection type
					if((tmaxUser.IOxState = GetStateSelection()) == null)
					{
						if(bSilent == false)
							OnPropError("You must assign the appropriate state", bCanCancel, null);
						break;
					}

					//	Get the objection ruling
					if((tmaxUser.IOxRuling = GetRulingSelection()) == null)
					{
						if(bSilent == false)
							OnPropError("You must assign the appropriate ruling", bCanCancel, null);
						break;
					}

					//	First page/line
					if(m_ctrlFirstPage.Text.Length == 0)
					{
						if(bSilent == false)
							OnPropError("You must set the First Page value", bCanCancel, m_ctrlFirstPage);
						break;
					}

					try { lPage = System.Convert.ToInt64(m_ctrlFirstPage.Text); }
					catch { }
					if(lPage <= 0)
					{
						if(bSilent == false)
							OnPropError("The First Page value is not valid", bCanCancel, m_ctrlFirstPage);
						break;
					}

					//	First line
					if(m_ctrlFirstLine.Text.Length == 0)
					{
						if(bSilent == false)
							OnPropError("You must set the First Line value", bCanCancel, m_ctrlFirstLine);
						break;
					}

					try { iLine = System.Convert.ToInt32(m_ctrlFirstLine.Text); }
					catch { }
					if(iLine <= 0)
					{
						if(bSilent == false)
							OnPropError("The First Line value is not valid", bCanCancel, m_ctrlFirstLine);
						break;
					}

					tmaxUser.FirstPL = CTmaxToolbox.GetPL(lPage, iLine);

					//	Last page/line
					if(m_ctrlLastPage.Text.Length == 0)
					{
						if(bSilent == false)
							OnPropError("You must set the Last Page value", bCanCancel, m_ctrlLastPage);
						break;
					}

					try { lPage = System.Convert.ToInt64(m_ctrlLastPage.Text); }
					catch { }
					if(lPage <= 0)
					{
						if(bSilent == false)
							OnPropError("The Last Page value is not valid", bCanCancel, m_ctrlLastPage);
						break;
					}

					//	Last line
					if(m_ctrlLastLine.Text.Length == 0)
					{
						if(bSilent == false)
							OnPropError("You must set the Last Line value", bCanCancel, m_ctrlLastLine);
						break;
					}

					try { iLine = System.Convert.ToInt32(m_ctrlLastLine.Text); }
					catch { }
					if(iLine <= 0)
					{
						if(bSilent == false)
							OnPropError("The Last Line value is not valid", bCanCancel, m_ctrlLastLine);
						break;
					}

					tmaxUser.LastPL = CTmaxToolbox.GetPL(lPage, iLine);

					if(tmaxUser.LastPL < tmaxUser.FirstPL)
					{
						if(bSilent == false)
							OnPropError("The First and Last page/line values are reversed", bCanCancel, m_ctrlFirstPage);
						break;
					}

					//	Get the Argument
					if(m_ctrlArgument.Text.Length > 0)
						tmaxUser.Argument = m_ctrlArgument.Text;
					else
					{
						if(bSilent == false)
							OnPropError("You must provide a Argument to describe the objection", bCanCancel, m_ctrlArgument);
						break;
					}

					//	Check the page/line range
					if(tmaxUser.ICaseDeposition != null)
					{
						if(tmaxUser.ICaseDeposition.GetFirstPL() > 0)
						{
							if(tmaxUser.FirstPL < tmaxUser.ICaseDeposition.GetFirstPL())
							{
								if(bSilent == false)
									OnPropError("The first page/line is outside the range for the deposition", bCanCancel, m_ctrlFirstPage);
								break;
							}
							
						}// if(tmaxUser.ITmaxDeposition.GetFirstPL() > 0)

						if(tmaxUser.ICaseDeposition.GetLastPL() > 0)
						{
							if(tmaxUser.LastPL > tmaxUser.ICaseDeposition.GetLastPL())
							{
								if(bSilent == false)
									OnPropError("The first page/line is outside the range for the deposition", bCanCancel, m_ctrlFirstPage);
								break;
							}

						}// if(tmaxUser.ITmaxDeposition.GetLastPL() > 0)

					}// if(tmaxUser.ITmaxDeposition != null)
					
					//	Copy the values now that they've been validated
					tmaxObjection.ICaseDeposition = tmaxUser.ICaseDeposition;
					tmaxObjection.IOxState = tmaxUser.IOxState;
					tmaxObjection.IOxRuling = tmaxUser.IOxRuling;
					tmaxObjection.FirstPL = tmaxUser.FirstPL;
					tmaxObjection.LastPL = tmaxUser.LastPL;
					tmaxObjection.Argument = tmaxUser.Argument;

					//	These values do not need to be validated
					tmaxObjection.Plaintiff = (m_ctrlPlaintiff.Checked == true);
					tmaxObjection.Response1 = m_ctrlResponse1.Text;

					bSuccessful = true;

				}// while(bSuccessful == false)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProps", m_tmaxErrorBuilder.Message(ERROR_SET_PROPS_EX), Ex);
			}

			return bSuccessful;

		}// public bool SetProps(CTmaxObjection tmaxObjection)

		/// <summary>Called to set the collection of depositions</summary>
		/// <param name="aDepositions">The collection of depositions</param>
		/// <returns>true if successful</returns>
		public bool SetDepositions(IList aDepositions)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlDepositions.Items.Clear();

				if((m_aDepositions = aDepositions) != null)
				{
					foreach(ITmaxDeposition O in m_aDepositions)
						m_ctrlDepositions.Items.Add(O);

				}

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDepositions", m_tmaxErrorBuilder.Message(ERROR_SET_DEPOSITIONS_EX), Ex);
			}

			return bSuccessful;

		}// public bool SetDepositions(ArrayList aDepositions)

		/// <summary>Called to set the collection of objection types</summary>
		/// <param name="aStates">The collection of types</param>
		/// <returns>true if successful</returns>
		public bool SetStates(IList aStates)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlStates.Items.Clear();
				
				if((m_aStates = aStates) != null)
				{
					foreach(ITmaxBaseObjectionRecord O in aStates)
					{
						m_ctrlStates.Items.Add(O.GetText());
					}
						
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetStates", m_tmaxErrorBuilder.Message(ERROR_SET_STATES_EX), Ex);
			}

			return bSuccessful;

		}// public bool SetStates(ArrayList aStates)

		/// <summary>Called to set the collection of objection types</summary>
		/// <param name="aRulings">The collection of types</param>
		/// <returns>true if successful</returns>
		public bool SetRulings(IList aRulings)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlRulings.Items.Clear();

				if((m_aRulings = aRulings) != null)
				{
					foreach(ITmaxBaseObjectionRecord O in aRulings)
					{
						m_ctrlRulings.Items.Add(O.GetText());
					}

				}

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetRulings", m_tmaxErrorBuilder.Message(ERROR_SET_RULINGS_EX), Ex);
			}

			return bSuccessful;

		}// public bool SetRulings(ArrayList aRulings)

		#endregion Public Methods

		#region Protected Methods

		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);
			
			//	Are we allowing the user to change the deposition?
			m_ctrlDeposition.Visible = (m_aDepositions == null);
			m_ctrlDepositions.Visible = (m_aDepositions != null);
			
			//	Initialize the controls
			SetControlStates();
			
			//	Set the initial focus to the first page
			//m_ctrlFirstPage.Focus();

		}// protected override void OnLoad(System.EventArgs e)
		
		/// <summary>Called by the parent form to set focus to the control</summary>
		///	<param name="bExtents">true to set focus to page/line extents controls</param>
		public void SetFocus(bool bExtents)
		{
			try
			{
				this.Focus();
				
				//	Should we set focus to the first page control?
				if((bExtents == true) || (m_tmaxObjection == null) || (m_tmaxObjection.FirstPL <= 0))
				{
					m_ctrlFirstPage.Focus();
					m_ctrlFirstPage.Select();
					m_ctrlFirstPage.SelectAll();
				}
				else
				{
					m_ctrlArgument.Focus();
					m_ctrlArgument.Select();
					m_ctrlArgument.SelectAll();
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetFocus", Ex);
			}

		}// public void SetFocus()

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		override protected void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Let the base class add it's strings first
			base.SetErrorStrings();

			//	Now add the strings for this class
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the active objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the objection depositions");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the objection states");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the control states.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the objection state: state = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the objection properties");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the state selection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the deposition selection to: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the deposition selection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the objection rulings");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the objection ruling: ruling = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the ruling selection");

		}// override protected void SetErrorStrings()

		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			this.m_ctrlDepositionLabel = new System.Windows.Forms.Label();
			this.m_ctrlArgument = new System.Windows.Forms.TextBox();
			this.m_ctrlArgumentLabel = new System.Windows.Forms.Label();
			this.m_ctrlDepositions = new System.Windows.Forms.ComboBox();
			this.m_ctrlDeposition = new System.Windows.Forms.Label();
			this.m_ctrlStates = new System.Windows.Forms.ComboBox();
			this.m_ctrlLastLine = new System.Windows.Forms.TextBox();
			this.m_ctrlStateLabel = new System.Windows.Forms.Label();
			this.m_ctrlLastPage = new System.Windows.Forms.TextBox();
			this.m_ctrlLastPLLabel = new System.Windows.Forms.Label();
			this.m_ctrlFirstLine = new System.Windows.Forms.TextBox();
			this.m_ctrlFirstPage = new System.Windows.Forms.TextBox();
			this.m_ctrlFirstPLLabel = new System.Windows.Forms.Label();
			this.m_ctrlPlaintiff = new System.Windows.Forms.RadioButton();
			this.m_ctrlDefendant = new System.Windows.Forms.RadioButton();
			this.m_ctrlResponse1 = new System.Windows.Forms.TextBox();
			this.m_ctrlResponse1Label = new System.Windows.Forms.Label();
			this.m_ctrlRulings = new System.Windows.Forms.ComboBox();
			this.m_ctrlRulingsLabel = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlDepositionLabel
			// 
			this.m_ctrlDepositionLabel.AutoSize = true;
			this.m_ctrlDepositionLabel.Location = new System.Drawing.Point(6, 10);
			this.m_ctrlDepositionLabel.Name = "m_ctrlDepositionLabel";
			this.m_ctrlDepositionLabel.Size = new System.Drawing.Size(57, 13);
			this.m_ctrlDepositionLabel.TabIndex = 0;
			this.m_ctrlDepositionLabel.Text = "Deposition";
			// 
			// m_ctrlArgument
			// 
			this.m_ctrlArgument.AcceptsReturn = true;
			this.m_ctrlArgument.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlArgument.Location = new System.Drawing.Point(64, 3);
			this.m_ctrlArgument.Multiline = true;
			this.m_ctrlArgument.Name = "m_ctrlArgument";
			this.m_ctrlArgument.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_ctrlArgument.Size = new System.Drawing.Size(466, 33);
			this.m_ctrlArgument.TabIndex = 0;
			// 
			// m_ctrlArgumentLabel
			// 
			this.m_ctrlArgumentLabel.AutoSize = true;
			this.m_ctrlArgumentLabel.Location = new System.Drawing.Point(3, 2);
			this.m_ctrlArgumentLabel.Name = "m_ctrlArgumentLabel";
			this.m_ctrlArgumentLabel.Size = new System.Drawing.Size(52, 13);
			this.m_ctrlArgumentLabel.TabIndex = 12;
			this.m_ctrlArgumentLabel.Text = "Objection";
			// 
			// m_ctrlDepositions
			// 
			this.m_ctrlDepositions.DisplayMember = "ShowAs";
			this.m_ctrlDepositions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlDepositions.FormattingEnabled = true;
			this.m_ctrlDepositions.Location = new System.Drawing.Point(81, 8);
			this.m_ctrlDepositions.Name = "m_ctrlDepositions";
			this.m_ctrlDepositions.Size = new System.Drawing.Size(121, 21);
			this.m_ctrlDepositions.TabIndex = 0;
			// 
			// m_ctrlDeposition
			// 
			this.m_ctrlDeposition.AutoEllipsis = true;
			this.m_ctrlDeposition.Location = new System.Drawing.Point(81, 11);
			this.m_ctrlDeposition.Name = "m_ctrlDeposition";
			this.m_ctrlDeposition.Size = new System.Drawing.Size(121, 13);
			this.m_ctrlDeposition.TabIndex = 1;
			this.m_ctrlDeposition.Text = "MediaId";
			// 
			// m_ctrlStates
			// 
			this.m_ctrlStates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlStates.FormattingEnabled = true;
			this.m_ctrlStates.Location = new System.Drawing.Point(422, 9);
			this.m_ctrlStates.Name = "m_ctrlStates";
			this.m_ctrlStates.Size = new System.Drawing.Size(111, 21);
			this.m_ctrlStates.TabIndex = 7;
			// 
			// m_ctrlLastLine
			// 
			this.m_ctrlLastLine.Location = new System.Drawing.Point(338, 37);
			this.m_ctrlLastLine.Name = "m_ctrlLastLine";
			this.m_ctrlLastLine.Size = new System.Drawing.Size(35, 20);
			this.m_ctrlLastLine.TabIndex = 6;
			// 
			// m_ctrlStateLabel
			// 
			this.m_ctrlStateLabel.AutoSize = true;
			this.m_ctrlStateLabel.Location = new System.Drawing.Point(379, 13);
			this.m_ctrlStateLabel.Name = "m_ctrlStateLabel";
			this.m_ctrlStateLabel.Size = new System.Drawing.Size(37, 13);
			this.m_ctrlStateLabel.TabIndex = 18;
			this.m_ctrlStateLabel.Text = "Status";
			// 
			// m_ctrlLastPage
			// 
			this.m_ctrlLastPage.Location = new System.Drawing.Point(280, 37);
			this.m_ctrlLastPage.Name = "m_ctrlLastPage";
			this.m_ctrlLastPage.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlLastPage.TabIndex = 5;
			// 
			// m_ctrlLastPLLabel
			// 
			this.m_ctrlLastPLLabel.AutoSize = true;
			this.m_ctrlLastPLLabel.Location = new System.Drawing.Point(210, 37);
			this.m_ctrlLastPLLabel.Name = "m_ctrlLastPLLabel";
			this.m_ctrlLastPLLabel.Size = new System.Drawing.Size(62, 13);
			this.m_ctrlLastPLLabel.TabIndex = 23;
			this.m_ctrlLastPLLabel.Text = "Last PG:LN";
			// 
			// m_ctrlFirstLine
			// 
			this.m_ctrlFirstLine.Location = new System.Drawing.Point(338, 9);
			this.m_ctrlFirstLine.Name = "m_ctrlFirstLine";
			this.m_ctrlFirstLine.Size = new System.Drawing.Size(35, 20);
			this.m_ctrlFirstLine.TabIndex = 4;
			// 
			// m_ctrlFirstPage
			// 
			this.m_ctrlFirstPage.Location = new System.Drawing.Point(280, 9);
			this.m_ctrlFirstPage.Name = "m_ctrlFirstPage";
			this.m_ctrlFirstPage.Size = new System.Drawing.Size(52, 20);
			this.m_ctrlFirstPage.TabIndex = 3;
			// 
			// m_ctrlFirstPLLabel
			// 
			this.m_ctrlFirstPLLabel.AutoSize = true;
			this.m_ctrlFirstPLLabel.Location = new System.Drawing.Point(210, 11);
			this.m_ctrlFirstPLLabel.Name = "m_ctrlFirstPLLabel";
			this.m_ctrlFirstPLLabel.Size = new System.Drawing.Size(61, 13);
			this.m_ctrlFirstPLLabel.TabIndex = 20;
			this.m_ctrlFirstPLLabel.Text = "First PG:LN";
			// 
			// m_ctrlPlaintiff
			// 
			this.m_ctrlPlaintiff.Location = new System.Drawing.Point(5, 36);
			this.m_ctrlPlaintiff.Name = "m_ctrlPlaintiff";
			this.m_ctrlPlaintiff.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.m_ctrlPlaintiff.Size = new System.Drawing.Size(68, 20);
			this.m_ctrlPlaintiff.TabIndex = 1;
			this.m_ctrlPlaintiff.TabStop = true;
			this.m_ctrlPlaintiff.Text = "Plaintiff";
			this.m_ctrlPlaintiff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlPlaintiff.UseVisualStyleBackColor = true;
			// 
			// m_ctrlDefendant
			// 
			this.m_ctrlDefendant.Location = new System.Drawing.Point(81, 36);
			this.m_ctrlDefendant.Name = "m_ctrlDefendant";
			this.m_ctrlDefendant.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_ctrlDefendant.Size = new System.Drawing.Size(76, 20);
			this.m_ctrlDefendant.TabIndex = 2;
			this.m_ctrlDefendant.TabStop = true;
			this.m_ctrlDefendant.Text = "Defendant";
			this.m_ctrlDefendant.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlDefendant.UseVisualStyleBackColor = true;
			// 
			// m_ctrlResponse1
			// 
			this.m_ctrlResponse1.AcceptsReturn = true;
			this.m_ctrlResponse1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlResponse1.Location = new System.Drawing.Point(64, 3);
			this.m_ctrlResponse1.Multiline = true;
			this.m_ctrlResponse1.Name = "m_ctrlResponse1";
			this.m_ctrlResponse1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_ctrlResponse1.Size = new System.Drawing.Size(466, 34);
			this.m_ctrlResponse1.TabIndex = 0;
			// 
			// m_ctrlResponse1Label
			// 
			this.m_ctrlResponse1Label.AutoSize = true;
			this.m_ctrlResponse1Label.Location = new System.Drawing.Point(3, 6);
			this.m_ctrlResponse1Label.Name = "m_ctrlResponse1Label";
			this.m_ctrlResponse1Label.Size = new System.Drawing.Size(55, 13);
			this.m_ctrlResponse1Label.TabIndex = 27;
			this.m_ctrlResponse1Label.Text = "Response";
			// 
			// m_ctrlRulings
			// 
			this.m_ctrlRulings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlRulings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlRulings.FormattingEnabled = true;
			this.m_ctrlRulings.Location = new System.Drawing.Point(422, 37);
			this.m_ctrlRulings.Name = "m_ctrlRulings";
			this.m_ctrlRulings.Size = new System.Drawing.Size(111, 21);
			this.m_ctrlRulings.TabIndex = 8;
			// 
			// m_ctrlRulingsLabel
			// 
			this.m_ctrlRulingsLabel.AutoSize = true;
			this.m_ctrlRulingsLabel.Location = new System.Drawing.Point(379, 40);
			this.m_ctrlRulingsLabel.Name = "m_ctrlRulingsLabel";
			this.m_ctrlRulingsLabel.Size = new System.Drawing.Size(37, 13);
			this.m_ctrlRulingsLabel.TabIndex = 29;
			this.m_ctrlRulingsLabel.Text = "Ruling";
			this.m_ctrlRulingsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(3, 68);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.m_ctrlArgument);
			this.splitContainer1.Panel1.Controls.Add(this.m_ctrlArgumentLabel);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.m_ctrlResponse1);
			this.splitContainer1.Panel2.Controls.Add(this.m_ctrlResponse1Label);
			this.splitContainer1.Size = new System.Drawing.Size(533, 86);
			this.splitContainer1.SplitterDistance = 39;
			this.splitContainer1.TabIndex = 30;
			this.splitContainer1.TabStop = false;
			// 
			// CTmaxObjectionEditorCtrl
			// 
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.m_ctrlRulings);
			this.Controls.Add(this.m_ctrlRulingsLabel);
			this.Controls.Add(this.m_ctrlDefendant);
			this.Controls.Add(this.m_ctrlPlaintiff);
			this.Controls.Add(this.m_ctrlStates);
			this.Controls.Add(this.m_ctrlLastLine);
			this.Controls.Add(this.m_ctrlStateLabel);
			this.Controls.Add(this.m_ctrlLastPage);
			this.Controls.Add(this.m_ctrlLastPLLabel);
			this.Controls.Add(this.m_ctrlFirstLine);
			this.Controls.Add(this.m_ctrlFirstPage);
			this.Controls.Add(this.m_ctrlFirstPLLabel);
			this.Controls.Add(this.m_ctrlDeposition);
			this.Controls.Add(this.m_ctrlDepositions);
			this.Controls.Add(this.m_ctrlDepositionLabel);
			this.Name = "CTmaxObjectionEditorCtrl";
			this.Size = new System.Drawing.Size(539, 157);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}// protected void InitializeComponent()

		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

		}// protected override void Dispose(bool disposing)

		#endregion Protected Methods

		#region Private Methods

		/// <summary>Called to set the selection in the depositions list</summary>
		/// <param name="strMediaId">The deposition to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetDepositionSelection(string strMediaId)
		{
			int iIndex = -1;

			try
			{
				if((strMediaId != null) && (strMediaId.Length > 0))
				{
					for(iIndex = 0; iIndex < m_ctrlDepositions.Items.Count; iIndex++)
					{
						if(String.Compare(((ITmaxDeposition)(m_ctrlDepositions.Items[iIndex])).GetMediaId(), strMediaId, true) == 0)
							break;
					}
					
				}
				else if(m_ctrlDepositions.Items.Count > 0)
				{
					//	Select the first deposition if not already a selection
					if(m_ctrlDepositions.SelectedIndex < 0)
						iIndex = 0;
				}

				if(iIndex >= 0)
					m_ctrlDepositions.SelectedIndex = iIndex;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDepositionSelection", m_tmaxErrorBuilder.Message(ERROR_SET_DEPOSITION_EX, strMediaId), Ex);
			}

			return (iIndex >= 0);

		}// private bool SetDepositionSelection(string strMediaId)

		/// <summary>Called to get the selection in the depositions list</summary>
		/// <returns>the deposition selected by the user</returns>
		private ITmaxDeposition GetDepositionSelection()
		{
			ITmaxDeposition ITmaxDeposition = null;

			try
			{
				if((m_aDepositions != null) && (m_ctrlDepositions.SelectedIndex >= 0))
				{
					ITmaxDeposition = (ITmaxDeposition)(m_ctrlDepositions.Items[m_ctrlDepositions.SelectedIndex]);

				}// if((m_aDepositions != null) && (m_ctrlDepositions.Text.Length > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetDepositionSelection", m_tmaxErrorBuilder.Message(ERROR_GET_DEPOSITION_EX), Ex);
			}

			return ITmaxDeposition;

		}// private ITmaxDeposition GetDepositionSelection()

		/// <summary>Called to set the selection in the types list</summary>
		/// <param name="strState">The type to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetStateSelection(string strState)
		{
			int iIndex = -1;

			try
			{
				//	Do we have a valid state label?
				if((strState != null) && (strState.Length > 0))
				{
					iIndex = m_ctrlStates.FindStringExact(strState);
				}
				else if(m_ctrlStates.Items.Count > 0)
				{
					//	Select the default state if not already a selection
					if(m_ctrlStates.SelectedIndex < 0)
						iIndex = 0;
				}

				if(iIndex >= 0)
					m_ctrlStates.SelectedIndex = iIndex;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetState", m_tmaxErrorBuilder.Message(ERROR_SET_STATE_EX, strState), Ex);
			}
			
			return (iIndex >= 0);

		}// private bool SetStateSelection(string strState)

		/// <summary>Called to get the selection in the types list</summary>
		/// <returns>the type selected by the user</returns>
		private ITmaxBaseObjectionRecord GetStateSelection()
		{
			ITmaxBaseObjectionRecord IState = null;

			try
			{
				if((m_aStates != null) && (m_ctrlStates.Text.Length > 0))
				{
					//	Search for the type with this description
					foreach(ITmaxBaseObjectionRecord O in m_aStates)
					{
						if(String.Compare(O.GetText(), m_ctrlStates.Text, true) == 0)
						{
							IState = O;
							break;
						}
						
					}// foreach(ITmaxBaseObjectionRecord O in m_aStates)
						
				}// if((m_aStates != null) && (m_ctrlStates.Text.Length > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetState", m_tmaxErrorBuilder.Message(ERROR_GET_STATE_EX), Ex);
			}

			return IState;

		}// private ITmaxBaseObjectionRecord GetStateSelection()

		/// <summary>Called to set the selection in the types list</summary>
		/// <param name="strRuling">The type to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetRulingSelection(string strRuling)
		{
			int iIndex = -1;

			try
			{
				//	Do we have a valid state label?
				if((strRuling != null) && (strRuling.Length > 0))
				{
					iIndex = m_ctrlRulings.FindStringExact(strRuling);
				}
				else if(m_ctrlRulings.Items.Count > 0)
				{
					//	Select the default state if not already a selection
					if(m_ctrlRulings.SelectedIndex < 0)
						iIndex = 0;
				}

				if(iIndex >= 0)
					m_ctrlRulings.SelectedIndex = iIndex;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetRuling", m_tmaxErrorBuilder.Message(ERROR_SET_RULING_EX, strRuling), Ex);
			}

			return (iIndex >= 0);

		}// private bool SetRulingSelection(string strRuling)

		/// <summary>Called to get the selection in the types list</summary>
		/// <returns>the type selected by the user</returns>
		private ITmaxBaseObjectionRecord GetRulingSelection()
		{
			ITmaxBaseObjectionRecord IRuling = null;

			try
			{
				if((m_aRulings != null) && (m_ctrlRulings.Text.Length > 0))
				{
					//	Search for the type with this description
					foreach(ITmaxBaseObjectionRecord O in m_aRulings)
					{
						if(String.Compare(O.GetText(), m_ctrlRulings.Text, true) == 0)
						{
							IRuling = O;
							break;
						}

					}// foreach(ITmaxBaseObjectionRecord O in m_aRulings)

				}// if((m_aRulings != null) && (m_ctrlRulings.Text.Length > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetRuling", m_tmaxErrorBuilder.Message(ERROR_GET_RULING_EX), Ex);
			}

			return IRuling;

		}// private ITmaxBaseObjectionRecord GetRulingSelection()

		/// <summary>Called to set enable/disable the controls</summary>
		private void SetControlStates()
		{
			try
			{
				if(m_tmaxObjection != null)
				{
					if(m_tmaxObjection.FirstPL > 0)
					{
						m_ctrlFirstPage.Text = CTmaxToolbox.PLToPage(m_tmaxObjection.FirstPL).ToString();
						m_ctrlFirstLine.Text = CTmaxToolbox.PLToLine(m_tmaxObjection.FirstPL).ToString();
					}
					else
					{
						m_ctrlFirstPage.Text = "";
						m_ctrlFirstLine.Text = "";
					}

					if(m_tmaxObjection.LastPL > 0)
					{
						m_ctrlLastPage.Text = CTmaxToolbox.PLToPage(m_tmaxObjection.LastPL).ToString();
						m_ctrlLastLine.Text = CTmaxToolbox.PLToLine(m_tmaxObjection.LastPL).ToString();
					}
					else
					{
						m_ctrlLastPage.Text = "";
						m_ctrlLastLine.Text = "";
					}
					
					m_ctrlPlaintiff.Checked = m_tmaxObjection.Plaintiff;
					m_ctrlDefendant.Checked = !m_ctrlPlaintiff.Checked;
					m_ctrlArgument.Text = m_tmaxObjection.Argument;
					m_ctrlResponse1.Text = m_tmaxObjection.Response1;
					SetStateSelection(m_tmaxObjection.State);
					SetRulingSelection(m_tmaxObjection.Ruling);

					m_ctrlDeposition.Text = m_tmaxObjection.Deposition;
						
					if(m_ctrlDepositions.Visible == true)
					{
						SetDepositionSelection(m_tmaxObjection.Deposition);
					}
				
				}
				else
				{
					m_ctrlDeposition.Text = "";
					m_ctrlFirstPage.Text = "";
					m_ctrlFirstLine.Text = "";
					m_ctrlLastPage.Text = "";
					m_ctrlLastLine.Text = "";
					m_ctrlArgument.Text = "";
					m_ctrlResponse1.Text = "";
					SetStateSelection("");
					SetRulingSelection("");
					SetDepositionSelection("");
				}

				m_ctrlFirstPLLabel.Enabled = (m_tmaxObjection != null);
				m_ctrlFirstPage.Enabled = (m_tmaxObjection != null);
				m_ctrlDepositionLabel.Enabled = (m_tmaxObjection != null);
				m_ctrlDeposition.Enabled = (m_tmaxObjection != null);
				m_ctrlDepositions.Enabled = (m_tmaxObjection != null);
				m_ctrlFirstLine.Enabled = (m_tmaxObjection != null);
				m_ctrlLastPLLabel.Enabled = (m_tmaxObjection != null);
				m_ctrlLastPage.Enabled = (m_tmaxObjection != null);
				m_ctrlStateLabel.Enabled = (m_tmaxObjection != null);
				m_ctrlStates.Enabled = (m_tmaxObjection != null);
				m_ctrlRulings.Enabled = (m_tmaxObjection != null);
				m_ctrlRulingsLabel.Enabled = (m_tmaxObjection != null);
				m_ctrlLastLine.Enabled = (m_tmaxObjection != null);
				m_ctrlArgumentLabel.Enabled = (m_tmaxObjection != null);
				m_ctrlArgument.Enabled = (m_tmaxObjection != null);
				m_ctrlResponse1.Enabled = (m_tmaxObjection != null);
				m_ctrlResponse1Label.Enabled = (m_tmaxObjection != null);
				m_ctrlPlaintiff.Enabled = (m_tmaxObjection != null);
				m_ctrlDefendant.Enabled = (m_tmaxObjection != null);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetControlStates", m_tmaxErrorBuilder.Message(ERROR_SET_CONTROL_STATES_EX), Ex);
			}

		}// private void SetControlStates()

		/// <summary>This method is called to warn the user when an invalid property value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="bCanCancel">true if the user can cancel the operation</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		private void OnPropError(string strMsg, bool bCanCancel, System.Windows.Forms.Control ctrlSelect)
		{
			string strPrompt = "";
			
			if(bCanCancel == false)
			{
				MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				//	Set focus to the requested control
				if(ctrlSelect != null)
				{
					ctrlSelect.Focus();
					ctrlSelect.Select();
				}

			}
			else
			{
				strPrompt = "Unable to set the objection properties:\n\n";
				strPrompt += (strMsg + "\n\n");
				strPrompt += "Do you want to cancel the changes?";
				
				if(MessageBox.Show(strPrompt, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					//	Put the original values back
					SetObjection(m_tmaxObjection);
					
					//	Set the property flag
					m_bCancelled = true;
				}
				else
				{
					//	Set focus to the requested control
					//
					//	NOTE:	We only do this if the user keeps the changes because
					//			it will take away the focus from the parent form's
					//			active control
					//if(ctrlSelect != null)
					//{
					//    ctrlSelect.Focus();
					//    ctrlSelect.Select();
					//}

				}

			}// if(bCanCancel == false)

		}// private void OnPropError(string strMsg, bool bCanCancel, System.Windows.Forms.Control ctrlSelect)
		
		#endregion Private Methods

		#region Properties

		/// <summary>The active objection</summary>
		public CTmaxObjection Objection
		{
			get { return m_tmaxObjection; }
			set { SetObjection(value); }
		}

		/// <summary>The collection of available depositions</summary>
		public IList Depositions
		{
			get { return m_aDepositions; }
			set { SetDepositions(value); }
		}

		/// <summary>The collection of available objection types</summary>
		public IList States
		{
			get { return m_aStates; }
			set { SetStates(value); }
		}

		/// <summary>The collection of available objection types</summary>
		public IList Rulings
		{
			get { return m_aRulings; }
			set { SetRulings(value); }
		}

		/// <summary>The last state selection</summary>
		public string LastState
		{
			get { return m_strLastState; }
			set { m_strLastState = value; }
		}

		/// <summary>True if user cancels attempt to set the property values</summary>
		public bool Cancelled
		{
			get { return m_bCancelled; }
			set { m_bCancelled = value; }
		}

		#endregion Properties

	}// class CTmaxObjectionEditorCtrl

}// namespace FTI.Trialmax.Controls

