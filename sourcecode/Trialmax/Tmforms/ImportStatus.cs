using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form displays status information during an import operation</summary>
	public class CFImportStatus : CFTmaxBaseForm
	{
		#region Private Members

		/// <summary>Component collection required by forms designer</summary>
		private IContainer components;

		/// <summary>Group box for error messages</summary>
		private System.Windows.Forms.GroupBox m_ctrlErrorsGroup;
		
		/// <summary>Message control to display error messages</summary>
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlErrorMessages;
		
		/// <summary>Button to abort the current operation</summary>
		private System.Windows.Forms.Button m_ctrlAbort;
		
		/// <summary>Local member bound to Aborted property</summary>
		private bool m_bAborted = false;
		
		/// <summary>Local member bound to Status property</summary>
		private string m_strStatus = "";
		
		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";
		
		/// <summary>Local member bound to Line property</summary>
		private int m_iLine = 0;
		
		/// <summary>Status text label for status message control</summary>
		private System.Windows.Forms.Label m_ctrlStatusLabel;

		/// <summary>Static text control to display the form's status message</summary>
		private System.Windows.Forms.Label m_ctrlStatusMessage;
		
		/// <summary>Static text control to display the name of the active import file</summary>
		private System.Windows.Forms.Label m_ctrlFilename;
		
		/// <summary>Static text label for the Filename control</summary>
		private System.Windows.Forms.Label m_ctrlFilenameLabel;
		
		/// <summary>Static text control to display the active line number</summary>
		private System.Windows.Forms.Label m_ctrlLine;
		
		/// <summary>Static text label for the Line control</summary>
		private System.Windows.Forms.Label m_ctrlLineLabel;
		
		/// <summary>Pushbutton to allow user to save errors to file</summary>
		private System.Windows.Forms.Button m_ctrlSaveErrors;

		/// <summary>Group box for results list</summary>
		private GroupBox m_ctrlResultsGroup;
		
		/// <summary>Local member bound to Finished property</summary>
		private bool m_bFinished = false;

		/// <summary>List view to display operation results</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlResults;

		/// <summary>Images used for results list</summary>
		private ImageList m_ctrlResultImages;

		/// <summary>Pushbutton to save results to file</summary>
		private Button m_ctrlSaveResults;

		/// <summary>Local member bound to ShowResults property</summary>
		private bool m_bShowResults = false;

		/// <summary>Local member bound to ResultsMode property</summary>
		private TmaxImportMessageModes m_eResultsMode = TmaxImportMessageModes.AsciiMedia;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFImportStatus()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			m_tmaxEventSource.Name = "Import Status Form";
		}
		
		/// <summary>Called to add a new message to the errors list box</summary>
		/// <param name="strFilename">the file associated with the message</param>
		/// <param name="iLine">the line number associated with the message</param>
		/// <param name="strMessage">text associated with the message</param>
		/// <param name="eType">enumerated error level</param>
		public void AddError(string strFilename, int iLine, string strMessage, TmaxMessageLevels eLevel)
		{
			try
			{
				//	Create a new import message
				CTmaxImportMessage tmaxMessage = new CTmaxImportMessage();
				
				tmaxMessage.Filename   = strFilename;
				tmaxMessage.LineNumber = iLine;
				tmaxMessage.Message    = strMessage;
				tmaxMessage.Level	   = eLevel;
				
				m_ctrlErrorMessages.Add(tmaxMessage);

			}
			catch
			{
			}
			
		}// public void AddError(string strFilename, int iLine, string strMessage)

		/// <summary>Called to add a new result to the list box</summary>
		/// <param name="tmaxMessage">the message to be added to the results list</param>
		/// <param name="iLine">the line number associated with the message</param>
		/// <param name="eResult">the enumerated result identifier</param>
		/// <param name="strSummary">text associated with the summary</param>
		/// <param name="tmaxRecord">The database record associated with the result</param>
		public void AddResult(CTmaxImportMessage tmaxMessage)
		{
			try
			{
			    
				//	Set the image index if not already set by the caller
				if(tmaxMessage.ImageIndex < 0)
					SetImageIndex(tmaxMessage);
					
				//	Add it to the list
				m_ctrlResults.Add(tmaxMessage);
			}
			catch
			{
			}

		}// public void AddResult(CTmaxImportMessage tmaxMessage)

		/// <summary>Called to add a new result to the list box</summary>
		/// <param name="strFilename">the file associated with the message</param>
		/// <param name="iLine">the line number associated with the message</param>
		/// <param name="eResult">the enumerated result identifier</param>
		/// <param name="strSummary">text associated with the summary</param>
		/// <param name="tmaxRecord">The database record associated with the result</param>
		public void AddResult(string strFilename, int iLine, TmaxImportResults eResult, string strSummary, ITmaxBaseRecord tmaxRecord)
		{
			try
			{
				//	Create a new import message
				CTmaxImportMessage tmaxMessage = new CTmaxImportMessage();

				tmaxMessage.Filename = strFilename;
				tmaxMessage.LineNumber = iLine;
				tmaxMessage.Message = strSummary;
				tmaxMessage.Result = eResult;
				tmaxMessage.Level = TmaxMessageLevels.Information;
				tmaxMessage.TmaxRecord = tmaxRecord;
				
				AddResult(tmaxMessage);
			}
			catch
			{
			}

		}// public void AddResult(string strFilename, int iLine, TmaxImportResults eResult, string strSummary, ITmaxBaseRecord tmaxRecord)

		/// <summary>Called to add a new result to the list box</summary>
		/// <param name="strFilename">the file associated with the message</param>
		/// <param name="eResult">the enumerated result identifier</param>
		/// <param name="strSummary">text associated with the summary</param>
		/// <param name="tmaxRecord">The database record associated with the result</param>
		public void AddResult(string strFilename, TmaxImportResults eResult, string strSummary, ITmaxBaseRecord tmaxRecord)
		{
			AddResult(strFilename, 0, eResult, strSummary, tmaxRecord);	
		}

		/// <summary>Called to add a new result to the list box</summary>
		/// <param name="strFilename">the file associated with the message</param>
		/// <param name="iLine">the line number associated with the message</param>
		/// <param name="eResult">the enumerated result identifier</param>
		/// <param name="strSummary">text associated with the summary</param>
		/// <param name="eDataType">The data type associated with the result</param>
		/// <param name="eMediaType">The media type associated with the result</param>
		public void AddResult(string strFilename, int iLine, TmaxImportResults eResult, string strSummary, TmaxDataTypes eDataType, TmaxMediaTypes eMediaType)
		{
			try
			{
				//	Create a new import message
				CTmaxImportMessage tmaxMessage = new CTmaxImportMessage();

				tmaxMessage.Filename = strFilename;
				tmaxMessage.LineNumber = iLine;
				tmaxMessage.Message = strSummary;
				tmaxMessage.Result = eResult;
				tmaxMessage.Level = TmaxMessageLevels.Information;
				tmaxMessage.DataType = eDataType;
				tmaxMessage.MediaType = eMediaType;

				AddResult(tmaxMessage);
			}
			catch
			{
			}

		}// public void AddResult(string strFilename, int iLine, TmaxImportResults eResult, string strSummary, TmaxDataTypes eDataType, TmaxMediaTypes eMediaType)

		/// <summary>Called to add a new result to the list box</summary>
		/// <param name="strFilename">the file associated with the message</param>
		/// <param name="iLine">the line number associated with the message</param>
		/// <param name="eResult">the enumerated result identifier</param>
		/// <param name="strSummary">text associated with the summary</param>
		/// <param name="tmaxObjection">The application objection associated with the result</param>
		public void AddResult(string strFilename, int iLine, TmaxImportResults eResult, string strSummary, CTmaxObjection tmaxObjection)
		{
			try
			{

                if ((tmaxObjection != null) && (tmaxObjection.IOxObjection != null))
                {
                    AddResult(strFilename, iLine, eResult, strSummary, tmaxObjection.IOxObjection);
                }
                else
                {
                    AddResult(strFilename, iLine, eResult, strSummary, TmaxDataTypes.Objection, TmaxMediaTypes.Unknown);
                }
			}
			catch
			{
			}

		}// public void AddResult(string strFilename, int iLine, TmaxImportResults eResult, string strSummary, CTmaxObjection tmaxObjection)

		/// <summary>Called to set the status message displayed by the form</summary>
		/// <param name="strStatus"></param>
		private delegate void SetStatusDelegate(string strStatus);
		public void SetStatus(string strStatus)
		{
			try
			{
				//	Is this being called from another thread?
				//
				//	NOTE:	This is going to return false if the window is not yet visible
				if(this.InvokeRequired == true)
				{
					this.BeginInvoke(new SetStatusDelegate(SetStatus), new object[] { strStatus });
				}
				else
				{
					//	Only update the status if not aborted by the user
					if((m_bAborted == false) && (this.IsDisposed == false))
					{
						m_strStatus = strStatus;
						
						if(this.Visible == true)
						{
							m_ctrlStatusMessage.Text = m_strStatus;
							m_ctrlStatusMessage.Refresh();
						}
						
					}

				}// if(this.InvokeRequired == true)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetStatus", Ex);
			}

		}// public void SetStatus(string strStatus)

		/// <summary>Called before displaying the form to make the results visible</summary>
		/// <param name="bShowResults">true to make the results visible</param>
		public void SetShowResults(bool bShowResults)
		{
			try
			{
				//	Are we going to show the results?
				if((m_bShowResults = bShowResults) == true)
				{
					//	Initialize the columns in the results list
					m_ctrlResults.OwnerImages = m_ctrlResultImages;
					m_ctrlResults.DisplayMode = (int)m_eResultsMode;
					m_ctrlResults.Initialize(new CTmaxImportMessage());		
				
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetShowResults", Ex);
			}

		}// public void SetShowResults(bool bShowResults)

		/// <summary>Called by the worker thread to indicate the operation is finished</summary>
		/// <param name="strMessage">the summary message to be displayed</param>
		public void OnThreadFinished(string strSummary)
		{
			try
			{
				//	Give the window some time to become visible
				//
				//	NOTE:	In rare instances (like an empty import file) this method may
				//			be getting called before the window actually becomes visible
				for(int i = 1; ((i <= 5) && (this.Visible == false)); i++)
				{
					Thread.Sleep(1000);
					m_tmaxEventSource.FireDiagnostic(this, "OnThreadFinished", "Delay - " + i.ToString());
				}

				//	The window must be created to invoke the method asynchronously
				if(this.Visible == true)
				{
					this.BeginInvoke(new SetFinishedDelegate(SetFinished), new object[] { strSummary });
					m_tmaxEventSource.FireDiagnostic(this, "OnThreadFinished", "Invoke SetFinished()");
				}
				else
				{
					SetFinished(strSummary); // Just in case
					m_tmaxEventSource.FireDiagnostic(this, "OnThreadFinished", "Call SetFinished()");
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnThreadFinished", Ex);
			}

		}// public void OnThreadFinished(string strSummary)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Overridden base class member called when the form gets loaded</summary>
		/// <param name="e">System event items</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Initialize the controls used to display the results
			InitializeResultCtrls();

			if(m_ctrlStatusMessage != null)
				m_ctrlStatusMessage.Text = m_strStatus;
			if(m_ctrlFilename != null)
				m_ctrlFilename.Text = m_strFilename;
			if(m_ctrlLine != null)
				m_ctrlLine.Text = (m_iLine > 0) ? m_iLine.ToString() : "";
			
			base.OnLoad(e);

		}// protected override void OnLoad(EventArgs e)

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
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFImportStatus));
			this.m_ctrlErrorsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlErrorMessages = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.m_ctrlAbort = new System.Windows.Forms.Button();
			this.m_ctrlStatusLabel = new System.Windows.Forms.Label();
			this.m_ctrlStatusMessage = new System.Windows.Forms.Label();
			this.m_ctrlFilename = new System.Windows.Forms.Label();
			this.m_ctrlFilenameLabel = new System.Windows.Forms.Label();
			this.m_ctrlLine = new System.Windows.Forms.Label();
			this.m_ctrlLineLabel = new System.Windows.Forms.Label();
			this.m_ctrlSaveErrors = new System.Windows.Forms.Button();
			this.m_ctrlResultsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlResults = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlResultImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlSaveResults = new System.Windows.Forms.Button();
			this.m_ctrlErrorsGroup.SuspendLayout();
			this.m_ctrlResultsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlErrorsGroup
			// 
			this.m_ctrlErrorsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlErrorsGroup.Controls.Add(this.m_ctrlErrorMessages);
			this.m_ctrlErrorsGroup.Location = new System.Drawing.Point(8, 254);
			this.m_ctrlErrorsGroup.Name = "m_ctrlErrorsGroup";
			this.m_ctrlErrorsGroup.Size = new System.Drawing.Size(366, 123);
			this.m_ctrlErrorsGroup.TabIndex = 1;
			this.m_ctrlErrorsGroup.TabStop = false;
			this.m_ctrlErrorsGroup.Text = "Errors:";
			// 
			// m_ctrlErrorMessages
			// 
			this.m_ctrlErrorMessages.AddTop = false;
			this.m_ctrlErrorMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlErrorMessages.ClearOnDblClick = false;
			this.m_ctrlErrorMessages.Format = FTI.Trialmax.Controls.TmaxMessageFormats.ImportMessage;
			this.m_ctrlErrorMessages.Location = new System.Drawing.Point(8, 16);
			this.m_ctrlErrorMessages.MaxRows = 0;
			this.m_ctrlErrorMessages.Name = "m_ctrlErrorMessages";
			this.m_ctrlErrorMessages.SelectedIndex = -1;
			this.m_ctrlErrorMessages.ShowHeaders = true;
			this.m_ctrlErrorMessages.ShowImage = true;
			this.m_ctrlErrorMessages.Size = new System.Drawing.Size(350, 95);
			this.m_ctrlErrorMessages.TabIndex = 0;
			// 
			// m_ctrlAbort
			// 
			this.m_ctrlAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAbort.Location = new System.Drawing.Point(281, 387);
			this.m_ctrlAbort.Name = "m_ctrlAbort";
			this.m_ctrlAbort.Size = new System.Drawing.Size(89, 23);
			this.m_ctrlAbort.TabIndex = 2;
			this.m_ctrlAbort.Text = "&Abort";
			this.m_ctrlAbort.Click += new System.EventHandler(this.OnClickAbort);
			// 
			// m_ctrlStatusLabel
			// 
			this.m_ctrlStatusLabel.Location = new System.Drawing.Point(16, 56);
			this.m_ctrlStatusLabel.Name = "m_ctrlStatusLabel";
			this.m_ctrlStatusLabel.Size = new System.Drawing.Size(60, 16);
			this.m_ctrlStatusLabel.TabIndex = 3;
			this.m_ctrlStatusLabel.Text = "Status:";
			// 
			// m_ctrlStatusMessage
			// 
			this.m_ctrlStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStatusMessage.Location = new System.Drawing.Point(82, 56);
			this.m_ctrlStatusMessage.Name = "m_ctrlStatusMessage";
			this.m_ctrlStatusMessage.Size = new System.Drawing.Size(292, 24);
			this.m_ctrlStatusMessage.TabIndex = 4;
			// 
			// m_ctrlFilename
			// 
			this.m_ctrlFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFilename.Location = new System.Drawing.Point(82, 12);
			this.m_ctrlFilename.Name = "m_ctrlFilename";
			this.m_ctrlFilename.Size = new System.Drawing.Size(292, 16);
			this.m_ctrlFilename.TabIndex = 6;
			// 
			// m_ctrlFilenameLabel
			// 
			this.m_ctrlFilenameLabel.Location = new System.Drawing.Point(16, 12);
			this.m_ctrlFilenameLabel.Name = "m_ctrlFilenameLabel";
			this.m_ctrlFilenameLabel.Size = new System.Drawing.Size(60, 16);
			this.m_ctrlFilenameLabel.TabIndex = 5;
			this.m_ctrlFilenameLabel.Text = "Filename:";
			// 
			// m_ctrlLine
			// 
			this.m_ctrlLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlLine.Location = new System.Drawing.Point(82, 32);
			this.m_ctrlLine.Name = "m_ctrlLine";
			this.m_ctrlLine.Size = new System.Drawing.Size(292, 16);
			this.m_ctrlLine.TabIndex = 8;
			// 
			// m_ctrlLineLabel
			// 
			this.m_ctrlLineLabel.Location = new System.Drawing.Point(16, 32);
			this.m_ctrlLineLabel.Name = "m_ctrlLineLabel";
			this.m_ctrlLineLabel.Size = new System.Drawing.Size(60, 16);
			this.m_ctrlLineLabel.TabIndex = 7;
			this.m_ctrlLineLabel.Text = "Line:";
			// 
			// m_ctrlSaveErrors
			// 
			this.m_ctrlSaveErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSaveErrors.Enabled = false;
			this.m_ctrlSaveErrors.Location = new System.Drawing.Point(184, 387);
			this.m_ctrlSaveErrors.Name = "m_ctrlSaveErrors";
			this.m_ctrlSaveErrors.Size = new System.Drawing.Size(89, 23);
			this.m_ctrlSaveErrors.TabIndex = 9;
			this.m_ctrlSaveErrors.Text = "Save &Errors";
			this.m_ctrlSaveErrors.Click += new System.EventHandler(this.OnClickSaveErrors);
			// 
			// m_ctrlResultsGroup
			// 
			this.m_ctrlResultsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlResultsGroup.Controls.Add(this.m_ctrlResults);
			this.m_ctrlResultsGroup.Location = new System.Drawing.Point(8, 83);
			this.m_ctrlResultsGroup.Name = "m_ctrlResultsGroup";
			this.m_ctrlResultsGroup.Size = new System.Drawing.Size(366, 163);
			this.m_ctrlResultsGroup.TabIndex = 10;
			this.m_ctrlResultsGroup.TabStop = false;
			this.m_ctrlResultsGroup.Text = "Results";
			// 
			// m_ctrlResults
			// 
			this.m_ctrlResults.AddTop = false;
			this.m_ctrlResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlResults.AutoResizeColumns = true;
			this.m_ctrlResults.ClearOnDblClick = false;
			this.m_ctrlResults.DisplayMode = 0;
			this.m_ctrlResults.HideSelection = false;
			this.m_ctrlResults.Location = new System.Drawing.Point(8, 18);
			this.m_ctrlResults.MaxRows = 0;
			this.m_ctrlResults.Name = "m_ctrlResults";
			this.m_ctrlResults.OwnerImages = null;
			this.m_ctrlResults.PaneId = 0;
			this.m_ctrlResults.SelectedIndex = -1;
			this.m_ctrlResults.ShowHeaders = true;
			this.m_ctrlResults.ShowImage = true;
			this.m_ctrlResults.Size = new System.Drawing.Size(350, 135);
			this.m_ctrlResults.TabIndex = 0;
			// 
			// m_ctrlResultImages
			// 
			this.m_ctrlResultImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlResultImages.ImageStream")));
			this.m_ctrlResultImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlResultImages.Images.SetKeyName(0, "result_objection_added.bmp");
			this.m_ctrlResultImages.Images.SetKeyName(1, "result_objection_updated.bmp");
			this.m_ctrlResultImages.Images.SetKeyName(2, "result_objection_conflict.bmp");
			this.m_ctrlResultImages.Images.SetKeyName(3, "result_objection_failed.bmp");
			this.m_ctrlResultImages.Images.SetKeyName(4, "result_objection_ignored.bmp");
			this.m_ctrlResultImages.Images.SetKeyName(5, "result_media_added.bmp");
			this.m_ctrlResultImages.Images.SetKeyName(6, "result_media_updated.bmp");
			this.m_ctrlResultImages.Images.SetKeyName(7, "result_media_failed.bmp");
			// 
			// m_ctrlSaveResults
			// 
			this.m_ctrlSaveResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSaveResults.Enabled = false;
			this.m_ctrlSaveResults.Location = new System.Drawing.Point(87, 387);
			this.m_ctrlSaveResults.Name = "m_ctrlSaveResults";
			this.m_ctrlSaveResults.Size = new System.Drawing.Size(89, 23);
			this.m_ctrlSaveResults.TabIndex = 11;
			this.m_ctrlSaveResults.Text = "Save &Results";
			this.m_ctrlSaveResults.Click += new System.EventHandler(this.OnClickSaveResults);
			// 
			// CFImportStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(382, 416);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlSaveResults);
			this.Controls.Add(this.m_ctrlResultsGroup);
			this.Controls.Add(this.m_ctrlSaveErrors);
			this.Controls.Add(this.m_ctrlLine);
			this.Controls.Add(this.m_ctrlLineLabel);
			this.Controls.Add(this.m_ctrlFilename);
			this.Controls.Add(this.m_ctrlFilenameLabel);
			this.Controls.Add(this.m_ctrlStatusMessage);
			this.Controls.Add(this.m_ctrlStatusLabel);
			this.Controls.Add(this.m_ctrlAbort);
			this.Controls.Add(this.m_ctrlErrorsGroup);
			this.Name = "CFImportStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ImportStatus";
			this.m_ctrlErrorsGroup.ResumeLayout(false);
			this.m_ctrlResultsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>Called to initialize the controls used to display the import results</summary>
		private void InitializeResultCtrls()
		{
			try
			{
				//	Are we showing the results?
				if(m_bShowResults == true)
				{
					m_ctrlResults.ResizeColumns();
				}
				else
				{
					m_ctrlResultsGroup.Visible = false;
					m_ctrlResults.Visible = false;
					m_ctrlSaveResults.Visible = false;
					m_ctrlSaveErrors.Text = "&Save As";

					this.Height -= m_ctrlResultsGroup.Height;

					m_ctrlErrorsGroup.SetBounds(m_ctrlResultsGroup.Left,
												m_ctrlResultsGroup.Top,
												0, 0, BoundsSpecified.Location);
					m_ctrlErrorsGroup.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
					m_ctrlErrorMessages.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);

				}
					
			}
			catch
			{
			}

		}// private void HideResults()
		
		/// <summary>This method is called when the user clicks on Abort</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickAbort(object sender, System.EventArgs e)
		{
			//	Should we close the form?
			if((m_bAborted == true) || (m_bFinished == true))
			{
				DialogResult = (m_bFinished == true) ? DialogResult.OK : DialogResult.Abort;
				this.Close();
			}
			else
			{
				//	NOTE: Must do this BEFORE setting the aborted flag
				SetStatus("Operation Cancelled");

				//	User has aborted the operation
				m_bAborted = true;
				
				//	Allow the user to close the form
				SwitchToClose();
			}
			
		}// private void OnClickAbort(object sender, System.EventArgs e)
		
		/// <summary>This method is called when the user clicks on Save As</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSaveErrors(object sender, System.EventArgs e)
		{
			string strFilename = "";
			
			if(m_ctrlErrorMessages == null) return;
			if(m_ctrlErrorMessages.IsDisposed == true) return;
			if(m_ctrlErrorMessages.Count == 0) return;

			//	Build a default filename
			if((m_strFilename != null) && (m_strFilename.Length > 0))
			{
				strFilename = System.IO.Path.GetDirectoryName(m_strFilename);
				if((strFilename.Length > 0) && (strFilename.EndsWith("\\") == false))
					strFilename += "\\";
				strFilename += System.IO.Path.GetFileNameWithoutExtension(m_strFilename);
				strFilename += "_errors.txt";
			}
			
			m_ctrlErrorMessages.Save(strFilename, true, true);
			
		}// private void OnClickSaveErrors(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Save Results</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSaveResults(object sender, System.EventArgs e)
		{
			string strFilename = "";

			if(m_ctrlResults == null) return;
			if(m_ctrlResults.IsDisposed == true) return;
			if(m_ctrlResults.Count == 0) return;

			//	Build a default filename
			if((m_strFilename != null) && (m_strFilename.Length > 0))
			{
				strFilename = System.IO.Path.GetDirectoryName(m_strFilename);
				if((strFilename.Length > 0) && (strFilename.EndsWith("\\") == false))
					strFilename += "\\";
				strFilename += System.IO.Path.GetFileNameWithoutExtension(m_strFilename);
				strFilename += "_results.txt";
			}

			m_ctrlResults.Save(strFilename, true, true);

		}// private void OnClickSaveResults(object sender, System.EventArgs e)

		/// <summary>Called to finish the operation</summary>
		/// <param name="strMessage">the summary message to be displayed</param>
		private delegate void SetFinishedDelegate(string strSummary);
		private void SetFinished(string strSummary)
		{
			try
			{
				FTI.Shared.Win32.User.MessageBeep(0);

				if(m_bAborted == false)
				{
					m_bFinished = true;

					SwitchToClose();

					SetStatus("Import operation complete");
				}
				
				//	Should we display a summary message?
				if((this.Visible == true) && (strSummary != null) && (strSummary.Length > 0))
					MessageBox.Show(this, strSummary, "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);

				m_tmaxEventSource.FireDiagnostic(this, "SetFinished", "Finish Operation");
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetFinished", Ex);
			}

		}// public void SetFinished(string strSummary)

		/// <summary>Called to set the image to be associated with the result message</summary>
		/// <param name="tmaxMessage">the result message to be displayed</param>
		private void SetImageIndex(CTmaxImportMessage tmaxMessage)
		{
			try
			{
				switch(tmaxMessage.Result)
				{
					case TmaxImportResults.Added:
						tmaxMessage.ImageIndex = (tmaxMessage.DataType == TmaxDataTypes.Objection) ? 0 : 5;
						break;

					case TmaxImportResults.Updated:
						tmaxMessage.ImageIndex = (tmaxMessage.DataType == TmaxDataTypes.Objection) ? 1 : 6;
						break;

					case TmaxImportResults.Conflict:
						tmaxMessage.ImageIndex = 2;
						break;

					case TmaxImportResults.AddFailed:
					case TmaxImportResults.UpdateFailed:
						tmaxMessage.ImageIndex = (tmaxMessage.DataType == TmaxDataTypes.Objection) ? 3 : 7;
						break;

					case TmaxImportResults.Ignored:
						tmaxMessage.ImageIndex = 4;
						break;

					default:
						tmaxMessage.ImageIndex = -1;
						break;

				}// switch(tmaxMessage.Result)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetImageIndex", Ex);
			}

		}// private void SetImageIndex(CTmaxImportMessage tmaxMessage)

		/// <summary>This method is called to reset the controls to allow the user to close the form</summary>
		private void SwitchToClose()
		{
			Application.DoEvents();

			//	Change the text on the button
			if((m_ctrlAbort != null) && (m_ctrlAbort.IsDisposed == false))
				m_ctrlAbort.Text = "&OK";
			
			//	Allow user to save the results and errors
			if((m_ctrlResults != null) && (m_ctrlResults.Count > 0))
			{
				if((m_ctrlSaveResults != null) && (m_ctrlSaveResults.IsDisposed == false))
					m_ctrlSaveResults.Enabled = true;
			}
			if((m_ctrlErrorMessages != null) && (m_ctrlErrorMessages.Count > 0))
			{
				if((m_ctrlSaveErrors != null) && (m_ctrlSaveErrors.IsDisposed == false))
					m_ctrlSaveErrors.Enabled = true;
			}
			
		}// private void SwitchToClose()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Title to be displayed in the form caption</summary>
		public string Title
		{
			get { return this.Text; }
			set { this.Text = value; }
		}
		
		/// <summary>The form's status message</summary>
		public string Status
		{
			get { return m_strStatus; }
			set { SetStatus(value); }
		}
		
		/// <summary>The active file</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set 
			{ 
				m_strFilename = value;

                SetCtrlFilename(value);
			}
		
		}

        delegate void SetCtrlFilenameCallback(string value);
        private void SetCtrlFilename(string value)
        {
            if (m_ctrlFilename != null)
            {
                if (m_ctrlFilename.InvokeRequired)
                {
                    this.Invoke(new SetCtrlFilenameCallback(SetCtrlFilename), new object[] { value });
                }
                else
                {
                    m_ctrlFilename.Text = value;
                }
            }
        }


		
		/// <summary>The active line number</summary>
		public int Line
		{
			get { return m_iLine; }
			set 
			{ 
				m_iLine = value;

                if (m_ctrlLine != null)
                    SetLine();
			}
		
		}

        delegate void SetLineCallback();
        private void SetLine()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.

            if (this.m_ctrlLine.InvokeRequired)
            {
                SetLineCallback d = new SetLineCallback(SetLine);
                this.m_ctrlLine.Invoke(d, new object[] { });

            }
            else
            {
                m_ctrlLine.Text = (m_iLine > 0) ? m_iLine.ToString() : ""; 
            }
        }
		
		/// <summary>True if the operation has been aborted by the user</summary>
		public bool Aborted
		{
			get { return m_bAborted; }
		}

		/// <summary>True to show the Results list</summary>
		public bool ShowResults
		{
			get { return m_bShowResults; }
			set { SetShowResults(value); }
		}

		/// <summary>The display mode for the form's Results list</summary>
		public TmaxImportMessageModes ResultsMode
		{
			get { return m_eResultsMode; }
			set { m_eResultsMode = value; }
		}

		/// <summary>True if the operation has finished normally</summary>
		public bool Finished
		{
			get { return m_bFinished; }
		}
		
		#endregion Properties
		
	}// public class CFImportStatus : System.Windows.Forms.Form 
	
}// namespace FTI.Trialmax.Forms
