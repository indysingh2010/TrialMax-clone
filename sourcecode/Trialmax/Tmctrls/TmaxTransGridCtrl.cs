using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class creates a grid-style control for viewing the text in a deposition transcript</summary>
	public class CTmaxTransGridCtrl : CTmaxBaseCtrl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_DEPOSITION_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_CREATE_DATA_SOURCE_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_ADD_EX						= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_INITIALIZE_LAYOUT_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_SET_DATA_SOURCE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_SET_TEXT_COLOR_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 6;
		private const int ERROR_ACTIVATE_EX					= ERROR_TMAX_BASE_CONTROL_MAX + 7;
		private const int ERROR_GET_SELECTED_ROWS_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 8;
		private const int ERROR_GET_SELECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 9;
		private const int ERROR_GET_SELECTION_RANGE_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 10;
		private const int ERROR_SET_SELECTION_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 11;
		private const int ERROR_SET_SELECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 12;
		private const int ERROR_GET_CURSOR_TRANSCRIPT_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 13;
		private const int ERROR_SET_OBJECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 14;
		private const int ERROR_GET_SOURCE_ROWS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 15;
		private const int ERROR_GET_ROWS_OBJECTION_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 16;
		private const int ERROR_ADD_OBJECTION_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 17;
		private const int ERROR_ADD_OBJECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 18;
		private const int ERROR_REMOVE_OBJECTION_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 19;
		private const int ERROR_UPDATE_OBJECTION_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 20;
		private const int ERROR_FILL_OBJECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 21;
		private const int ERROR_SELECT_OBJECTION_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 22;
		private const int ERROR_GET_TEXT_EX					= ERROR_TMAX_BASE_CONTROL_MAX + 23;
		private const int ERROR_GET_SELECTED_TEXT_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 24;
		private const int ERROR_COPY_SELECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 25;
		private const int ERROR_PRINT_SELECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 26;
		private const int ERROR_SAVE_SELECTIONS_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 27;
		private const int ERROR_OPEN_FILE_STREAM_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 28;
		
		private const string GRID_COLUMN_PAGE					= "GP";
		private const string GRID_COLUMN_DEFENDANT_OBJECTIONS	= "DO";
		private const string GRID_COLUMN_PLAINTIFF_OBJECTIONS	= "PO";
		private const string GRID_COLUMN_ACTIVE_DESG			= "GAD";
		private const string GRID_COLUMN_LINE_IMAGE				= "GLI";
		private const string GRID_COLUMN_LINE					= "LN";
		private const string GRID_COLUMN_QA						= "QA";
		private const string GRID_COLUMN_TEXT					= "TEXT";
		
		private const int GRID_MAX_OBJECTION_IMAGES = 10;

		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>The child grid used to display the transcript text</summary>
		private Infragistics.Win.UltraWinGrid.UltraGrid m_ctrlUltraGrid;
		
		/// <summary>Private member bound to XmlDeposition property</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = null;

		/// <summary>Private member bound to Objections property</summary>
		private FTI.Shared.Trialmax.CTmaxObjections m_tmaxObjections = new CTmaxObjections();

		/// <summary>Private member bound to PlaintiffColor property</summary>
		private System.Drawing.Color m_plaintiffColor = Color.Red;

		/// <summary>Private member bound to DefendantColor property</summary>
		private System.Drawing.Color m_defendantColor = Color.Blue;

		/// <summary>Array of transcript lines used as source for the grid control</summary>
		private Array m_aDataSource = null;
		
		/// <summary>The threshold (sec) used to control the display of Pause indicators</summary>
		private double m_dPauseThreshold = 0;

		/// <summary>Image list bound to the grid control</summary>
		private System.Windows.Forms.ImageList m_ctrlUltraGridImages;
		
		/// <summary>Local flag to indicate that we are processing the transcript's prolog</summary>
		private bool m_bPrologue = false;
		
		/// <summary>The index of the DefendantObjections column in the grid</summary>
		private int m_iDefendantObjectionsIndex = -1;
		
		/// <summary>The index of the PlaintiffObjections column in the grid</summary>
		private int m_iPlaintiffObjectionsIndex = -1;

		/// <summary>The document used to send text to the printer</summary>
		private System.Drawing.Printing.PrintDocument m_docPrinter = new PrintDocument();

		/// <summary>The text being sent to the printer</summary>
		private string m_strPrinterText = "";

		/// <summary>The fixed-font used for print jobs</summary>
		private System.Drawing.Font m_printerFont = null;

		#endregion Private Members

		#region Public Methods
		
		/// <summary>Fired by the control when the user clicks on the Edit button</summary>
		public event System.EventHandler SelChanged;	
		public event System.EventHandler DblClick;	
	
		/// <summary>Constructor</summary>
		public CTmaxTransGridCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();

			m_docPrinter.PrintPage += new PrintPageEventHandler(this.OnPrintPage);

			//	Set the default event source name
			m_tmaxEventSource.Name = "Transcript Grid";
			
			m_tmaxEventSource.Attach(m_tmaxObjections.EventSource);
			
		}// public CTmaxTransGridCtrl() : base()
		
		/// <summary>This method is called to set the active deposition</summary>
		/// <param name="xmlDeposition">The deposition to be displayed</param>
		/// <param name="tmaxObjections">The collection of objections to be displayed</param>
		/// <returns>true if successful</returns>
		public bool SetDeposition(CXmlDeposition xmlDeposition, CTmaxObjections tmaxObjections)
		{
			bool	bSuccessful = false;
			Array	aSource = null;
			int		iIndex = 0;
			
			//	Is the caller actually clearing the grid?
			if(xmlDeposition == null)
			{
				Clear();
				return true;
			}
			
			try
			{
				//	Update the class reference
				m_xmlDeposition = xmlDeposition;
				
				//	Initialize the objections
				SetObjections(tmaxObjections);
				
				//	Nothing to do if no transcript text
				if((m_xmlDeposition.Transcripts != null) && (m_xmlDeposition.Transcripts.Count > 0))
				{
					//	Create a new data set
					if((aSource = CreateDataSource(m_xmlDeposition.Transcripts.Count)) == null) return false;

					//	Set the flag to indicate we are processing the prolog
					//
					//	NOTE:	The prologue is the block of unsynchronized text that may
					//			or may not appear at the top of the transcript
					m_bPrologue = true;
				
					//	Add a row for each transcript node
					foreach(CXmlTranscript xmlTranscript in m_xmlDeposition.Transcripts)
					{
						//	Add the row to the grid
						Add(xmlTranscript, aSource, iIndex);
					
						iIndex++;
						
					}// foreach(CXmlTranscript xmlTranscript in m_xmlDeposition.Transcripts)
				
					//	Assign the new data source
					SetDataSource(aSource);

					//	Post process the rows and columns now that the grid is loaded
					InitializeLayout();
					
					return true;
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_SET_DEPOSITION_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool SetDeposition(CXmlDeposition xmlDeposition)
		
		/// <summary>Called to clear the grid</summary>
		public void Clear()
		{
			try
			{
				//	Detach the data source from the grid
				if(m_aDataSource != null)
				{
					m_ctrlUltraGrid.DataSource = null;
					m_aDataSource = null;
				}
				
				m_tmaxObjections.Clear();
				
			}
			catch
			{
			}
			finally
			{
				m_xmlDeposition = null;
				m_aDataSource = null;
			}
			
		}// public void Clear()
		
		/// <summary>This method is called to highlight the specified lines of text</summary>
		/// <param name="lStartPL">The composite page/line to start with</param>
		/// <param name="lStopPL">The composite page/line to stop with</param>
		/// <param name="scHighlighter">The system drawing color</param>
		/// <returns>true if successful</returns>
		public bool Highlight(long lStartPL, long lStopPL, System.Drawing.Color scHighlighter)
		{
			return SetTextColor(lStartPL, lStopPL, scHighlighter);
		
		}// public bool Highlight(long lStartPL, long lStopPL, System.Drawing.Color scHighlighter)
		
		/// <summary>This method is called to remove the highlight from the specified lines</summary>
		/// <param name="lStartPL">The composite page/line to start with</param>
		/// <param name="lStopPL">The composite page/line to stop with</param>
		/// <returns>true if successful</returns>
		public bool Erase(long lStartPL, long lStopPL)
		{
			return SetTextColor(lStartPL, lStopPL, System.Drawing.Color.Black);
		
		}// public bool Erase(long lStartPL, long lStopPL)
		
		/// <summary>This method is called to erase all highlights</summary>
		/// <returns>true if successful</returns>
		public bool Erase()
		{
			return SetTextColor(0, 0, System.Drawing.Color.Black);
		
		}// public bool Erase()
		
		/// <summary>This method is called to activate the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be activated</param>
		/// <param name="bEnsureVisible">true to make sure the designation is visible</param>
		/// <returns>true if successful</returns>
		public bool Activate(CXmlDesignation xmlDesignation, bool bEnsureVisible)
		{
			long	lStartPL = -1;
			long	lStopPL = -1;
			bool	bSuccessful = false;
			
			try
			{
				//	Do we have a valid designation?
				if(xmlDesignation != null)
				{
					lStartPL = xmlDesignation.FirstPL;
					lStopPL = xmlDesignation.LastPL;
				}
				
				//	Set the image
				bSuccessful = SetActive(lStartPL, lStopPL, bEnsureVisible);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Activate", m_tmaxErrorBuilder.Message(ERROR_ACTIVATE_EX, (xmlDesignation != null ? xmlDesignation.Name : "NULL")), Ex);
			}
			
			return bSuccessful;
		
		}// public bool Activate(CXmlDesignation xmlDesignation)

		/// <summary>This method is called to set the active rows</summary>
		/// <returns>true if successful</returns>
		public bool SetActive(long lStartPL, long lStopPL, bool bEnsureVisible)
		{
			long lPL = -1;
			int iStartPLIndex = -1;
			bool bSuccessful = false;

			Debug.Assert(m_ctrlUltraGrid != null);
			Debug.Assert(m_ctrlUltraGrid.IsDisposed == false);
			if(m_ctrlUltraGrid == null) return false;
			if(m_ctrlUltraGrid.IsDisposed == true) return false;
			if(m_ctrlUltraGrid.Rows == null) return false;

			if(m_aDataSource == null) return false;

			try
			{
				for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
				{
					lPL = ((CTmaxTransGridRow)m_aDataSource.GetValue(i)).GetPL();

					if((lStartPL >= 0) && (lPL >= lStartPL) && (lPL <= lStopPL))
					{
						(m_ctrlUltraGrid.Rows[i]).Cells[GRID_COLUMN_ACTIVE_DESG].Appearance.Image = CTmaxTransGridRow.IMAGE_SELECTED_DESIGNATION;

						//	Are we supposed to make sure the designation is visible?
						if(bEnsureVisible == true)
						{
							//	Keep track of the index for the first line
							if(iStartPLIndex < 0)
								iStartPLIndex = i;

							//	Is this row visible?
							if(IsVisible(m_ctrlUltraGrid.Rows[i]) == true)
								bEnsureVisible = false; // Prevent further processing since already visible

						}// if(bEnsureVisible == true)

					}
					else
					{
						(m_ctrlUltraGrid.Rows[i]).Cells[GRID_COLUMN_ACTIVE_DESG].Appearance.Image = CTmaxTransGridRow.IMAGE_NONE;
					}

				}// for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)	

				//	Do we need to make the first row visible?
				if((bEnsureVisible == true) && (iStartPLIndex >= 0))
				{
					EnsureVisible(iStartPLIndex);
				}

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetActive", Ex);
			}

			return bSuccessful;

		}// public bool SetActive(long lStartPL, long lStopPL, bool bEnsureVisible)

		/// <summary>This method is called to make the specified row visible</summary>
		/// <param name="lPL">The composite PL value of the row</param>
		/// <param name="bExact">true to set selection only if exact PL is found</param>
		/// <returns>True if successful</returns>
		public bool EnsureVisible(long lPL, bool bExact)
		{
			int iIndex = -1;
			bool bSuccessful = false;

			//	Get the index of the desired row
			if((iIndex = GetRowIndex(lPL, bExact)) >= 0)
			{
				bSuccessful = EnsureVisible(iIndex);
			}

			return bSuccessful;

		}// public bool EnsureVisible(long lPL, bool bExact)

		/// <summary>This method is called to ensure that the specified row is visible</summary>
		/// <param name="iRow">The index of the row to make visible</param>
		/// <returns>true if successful</returns>
		public bool EnsureVisible(int iRow)
		{
			bool bSuccessful = false;
			bool bScroll = false;

			if(m_ctrlUltraGrid == null) return false;
			if(m_ctrlUltraGrid.IsDisposed == true) return false;
			if(m_ctrlUltraGrid.Rows == null) return false;
			if(iRow < 0) return false;
			if(iRow >= m_ctrlUltraGrid.Rows.Count) return false;

			try
			{
				//	Are we going to have to scroll the view?
				//
				//	NOTE:	We make this check before Activate() because that call will
				//			scroll the row to the last line in the grid if it's not
				//			visible
				bScroll = (IsVisible(m_ctrlUltraGrid.Rows[iRow]) == false);

				//	Make the row active 
				m_ctrlUltraGrid.Rows[iRow].Activate();

				//	Do we need to scroll to the top?
				if(bScroll == true)
					ScrollToTop(m_ctrlUltraGrid.Rows[iRow]);

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "EnsureVisible", Ex);
			}

			return bSuccessful;

		}// public void EnsureVisible(int iRow)

		/// <summary>This method will get the start and stop positions of the current selection</summary>
		///	<param name="rStartPL">Location where start position is to be stored</param>
		///	<param name="rStopPL">Location where stop position is to be stored</param>
		/// <returns>the number of selected lines</returns>
		public long GetSelectionRange(ref long rStartPL, ref long rStopPL)
		{
			CXmlTranscripts xmlTranscripts = null;
			long			lSelected = 0;
			
			try
			{
				//	Get the selected lines
				if((xmlTranscripts = GetSelections()) != null)
				{
					Debug.Assert(xmlTranscripts.Count > 0);
					if((lSelected = xmlTranscripts.Count) > 0)
					{
						//	Make sure the transcripts are in sorted order
						xmlTranscripts.Sort(true);
						
						//	Get the positions
						rStartPL = xmlTranscripts[0].PL;
						rStopPL  = xmlTranscripts[xmlTranscripts.Count - 1].PL;
					}
					
				}// if((xmlTranscripts = GetSelections()) != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSelectionRange", m_tmaxErrorBuilder.Message(ERROR_GET_SELECTION_RANGE_EX), Ex);
			}
			
			return lSelected;
		
		}// public long GetSelectionRange(ref long rStartPL, ref long rStopPL)
			
		/// <summary>This method will retrieve an the number of rows selected in the grid</summary>
		/// <returns>The number of selected rows</returns>
		public long GetSelectedCount()
		{
			Debug.Assert(m_ctrlUltraGrid != null);
			Debug.Assert(m_ctrlUltraGrid.IsDisposed == false);
			
			if(m_ctrlUltraGrid == null) return 0;
			if(m_ctrlUltraGrid.IsDisposed == true) return 0;
			if(m_ctrlUltraGrid.Selected == null) return 0;
			if(m_ctrlUltraGrid.Selected.Rows == null) return 0;
			
			return (m_ctrlUltraGrid.Selected.Rows.Count);
		
		}// public long GetSelectedCount()
			
		/// <summary>This method will retrieve an array of rows that represent the current selection</summary>
		/// <returns>The array of rows</returns>
		public Array GetSelectedRows()
		{
			int		iStart = -1;
			int		iStop = -1;
			int		iIndex = 0;
			int		i;
			
			//	Are the objects valid and are there some selections?
			if(GetSelectedCount() == 0) return null;
			
			try
			{
				//	Initialize the indexes
				iStart = m_ctrlUltraGrid.Selected.Rows[0].Index;
				iStop = iStart;
				
				//	Get the indexes of the first and last selected row
				for(i = 1; i < m_ctrlUltraGrid.Selected.Rows.Count; i++)
				{
					iIndex = m_ctrlUltraGrid.Selected.Rows[i].Index;
					
					if(iIndex < iStart)
						iStart = iIndex;
					if(iIndex > iStop)
						iStop = iIndex;
				}

                //	Build the array of rows that are the current selection
				if((iStart >= 0) && (iStop >= iStart))
				{
					Debug.Assert(iStop <= m_aDataSource.GetUpperBound(0));
					if(iStop > m_aDataSource.GetUpperBound(0)) return null;
					
					CTmaxTransGridRow [] aClip = new CTmaxTransGridRow[iStop - iStart + 1];

					for(i = 0, iIndex = iStart; iIndex <= iStop; i++, iIndex++)
					{
						aClip.SetValue(m_aDataSource.GetValue(iIndex), i);
					}
					
					return aClip;
				
				}// if((iStart >= 0) && (iStop >= iStart))
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSelectedRows", m_tmaxErrorBuilder.Message(ERROR_GET_SELECTED_ROWS_EX), Ex);
			}
			
			return null;
		
		}// public Array GetSelectedRows()

        /// <summary>This method will retrieve an array of rows that represent the current selection and 4 lines before and 4 lines after selection</summary>
        /// <returns>The array of rows</returns>
        public Array GetSelectedRowsForResultPane()
        {
            int iStart = -1;
            int iStop = -1;
            int iIndex = 0;
            int i;

            //	Are the objects valid and are there some selections?
            if (GetSelectedCount() == 0) return null;

            try
            {
                //	Initialize the indexes
                iStart = m_ctrlUltraGrid.Selected.Rows[0].Index;
                iStop = iStart;

                //	Get the indexes of the first and last selected row
                for (i = 1; i < m_ctrlUltraGrid.Selected.Rows.Count; i++)
                {
                    iIndex = m_ctrlUltraGrid.Selected.Rows[i].Index;

                    if (iIndex < iStart)
                        iStart = iIndex;
                    if (iIndex > iStop)
                        iStop = iIndex;
                }
                
                // Adding 4 after the selected index(selection)
                if ((iStop + 4) > m_aDataSource.GetUpperBound(0))
                {
                    iStop = m_aDataSource.GetUpperBound(0);
                }
                else if ((iStop + 4) <= m_aDataSource.GetUpperBound(0))
                {
                    iStop = iStop + 4;
                }

                // Subtracting 4 before the selected index(selection)
                if ((iStart - 4) < m_aDataSource.GetLowerBound(0))
                {
                    iStart = m_aDataSource.GetLowerBound(0);
                }
                else if ((iStart - 4) >= m_aDataSource.GetLowerBound(0))
                {
                    iStart = iStart - 4;
                }

                //	Build the array of rows that are the current selection
                if ((iStart >= 0) && (iStop >= iStart))
                {
                    Debug.Assert(iStop <= m_aDataSource.GetUpperBound(0));
                    if (iStop > m_aDataSource.GetUpperBound(0)) return null;

                    CTmaxTransGridRow[] aClip = new CTmaxTransGridRow[iStop - iStart + 1];

                    for (i = 0, iIndex = iStart; iIndex <= iStop; i++, iIndex++)
                    {
                        aClip.SetValue(m_aDataSource.GetValue(iIndex), i);
                    }

                    return aClip;

                }// if((iStart >= 0) && (iStop >= iStart))

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "GetSelectedRows", m_tmaxErrorBuilder.Message(ERROR_GET_SELECTED_ROWS_EX), Ex);
            }

            return null;

        } //public Array GetSelectedRowsForResultPane()
			
		/// <summary>This method will add the selected transcript lines to the caller's collection</summary>
		///	<param name="xmlTranscripts">The collection in which to store the selected lines</param>
		/// <returns>The total number of lines added to the collection</returns>
		public int GetSelections(CXmlTranscripts xmlTranscripts)
		{
			CTmaxTransGridRow	row = null;
			int					iFirst = -1;
			int					iLast = -1;
			int					iAdded = 0;
			int					i = 0;
			
			try
			{
				//	Get the indexes of the current selection
				if(GetUltraSelectionRange(ref iFirst, ref iLast) == true)
				{
					Debug.Assert(iLast <= m_aDataSource.GetUpperBound(0));
					if(iLast > m_aDataSource.GetUpperBound(0)) return 0;

					for(i = iFirst; i <= iLast; i++)
					{
						if((row = (CTmaxTransGridRow)(m_aDataSource.GetValue(i))) != null)
						{
							if(row.GetXmlTranscript() != null)
							{
								xmlTranscripts.Add(row.GetXmlTranscript());
								iAdded++;
							}

						}

					}// for(i = iFirst; i <= iLast; i++)

				}// if(GetUltraSelectionRange(ref iFirst, ref iLast) == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSelections", m_tmaxErrorBuilder.Message(ERROR_GET_SELECTIONS_EX), Ex);
			}
			
			return iAdded;
		
		}// public int GetSelections(CXmlTranscripts xmlTranscripts)
			
		/// <summary>This method will get a collection of the selected lines</summary>
		/// <returns>The collection of selected lines</returns>
		public CXmlTranscripts GetSelections()
		{
			CXmlTranscripts xmlTranscripts = null;
			
			try
			{
				xmlTranscripts = new CXmlTranscripts();
				GetSelections(xmlTranscripts);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetSelections", Ex);
			}
			
			if((xmlTranscripts != null) && (xmlTranscripts.Count > 0))
				return xmlTranscripts;
			else
				return null;
		
		}// public CXmlTranscripts GetSelections()

		/// <summary>This method is called to select the specified row</summary>
		/// <param name="lPL">The composite PL value of the row</param>
		/// <param name="bClear">true to clear existing selections</param>
		/// <param name="bExact">true to set selection only if exact PL is found</param>
		/// <returns>True if successful</returns>
		public bool SetSelection(long lPL, bool bClear, bool bExact)
		{
			int		iIndex = GetRowIndex(lPL, bExact);
			bool	bSuccessful = false;
			
			try
			{
				//	Get the index of the desired row
				if((iIndex = GetRowIndex(lPL, bExact)) >= 0)
				{
					if((bSuccessful = SetSelection((CTmaxTransGridRow)m_aDataSource.GetValue(iIndex), bClear)) == true)
						EnsureVisible(iIndex);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}
			
			return bSuccessful;
		
		}// public void SetSelection(long lPL, bool bClear, bool bExact)
		
		/// <summary>This method will select the specified row</summary>
		/// <param name="transRow">The row to be selected</param>
		/// <param name="bClear">true to clear the existing selection</param>
		/// <returns>True if successful</returns>
		public bool SetSelection(CTmaxTransGridRow transRow, bool bClear)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Clear the current selections if requested
				if(bClear == true)
				{
					if((m_ctrlUltraGrid.Selected != null) && (m_ctrlUltraGrid.Selected.Rows != null))
					{
						if(m_ctrlUltraGrid.Selected.Rows.Count > 0)
						{
							m_ctrlUltraGrid.Selected.Rows.Clear();
						}
						
					}
				
				}// if(bClear == true)
				
				//	Does the caller want to select a row?
				if(transRow != null)
				{
					if(transRow.GetGridIndex() >= 0)
						m_ctrlUltraGrid.Rows[transRow.GetGridIndex()].Selected = true;
					
				}// if(transRow != null)
				
				//	We're done
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}
			
			return bSuccessful;
		
		}// public bool SetSelection(CTmaxTransGridRow transRow, bool bClear)
			
		/// <summary>This method will set the rows selected in the grid</summary>
		/// <param name="aSelections">The array of rows to be selected</param>
		/// <returns>True if successful</returns>
		public bool SetSelections(Array aSelections)
		{
			CTmaxTransGridRow	transRow = null;
			bool				bSuccessful = false;
			
			try
			{
				//	Clear the current selections
				if((m_ctrlUltraGrid.Selected != null) && (m_ctrlUltraGrid.Selected.Rows != null))
				{
					if(m_ctrlUltraGrid.Selected.Rows.Count > 0)
					{
						m_ctrlUltraGrid.Selected.Rows.Clear();
					}
					
				}
				
				if(m_ctrlUltraGrid.ActiveRow != null)
					m_ctrlUltraGrid.ActiveRow = null;
					
				//	Does the caller want to select some rows?
				if((aSelections != null) && (aSelections.GetUpperBound(0) >= 0))
				{
					for(int i = 0; i <= aSelections.GetUpperBound(0); i++)
					{
						if((transRow = (CTmaxTransGridRow)(aSelections.GetValue(i))) != null)
						{
							if(transRow.GetGridIndex() >= 0)
								m_ctrlUltraGrid.Rows[transRow.GetGridIndex()].Selected = true;
						}
						
					}
					
				}// if((aSelections != null) && (aSelections.GetUpperBound(0) >= 0))
				
				//	We're done
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelections", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTIONS_EX), Ex);
			}
			
			return bSuccessful;
		
		}// public bool SetSelections(Array aSelections)
			
		/// <summary>This method will get the transcript line at the current cursor position</summary>
		/// <returns>The line at the current position</returns>
		public CXmlTranscript GetCursorTranscript()
		{
			CXmlTranscript		xmlTranscript = null;
			UltraGridRow		ultraRow = null;
			CTmaxTransGridRow	transRow = null;
			int					iIndex = 0;
			
			try
			{
				if(m_aDataSource == null) return null;
				if(m_aDataSource.GetUpperBound(0) < 0) return null;
				
				//	Get the row at the current position
				if((ultraRow = GetUltraRow()) != null)
				{
					//	What is the index of this row in the data source?
					iIndex = ultraRow.Index;
					Debug.Assert(iIndex <= m_aDataSource.GetUpperBound(0));
					if(iIndex > m_aDataSource.GetUpperBound(0)) return null;
				
					//	Get the source object bound to this row
					if((transRow = ((CTmaxTransGridRow)m_aDataSource.GetValue(iIndex))) != null)
					{
						xmlTranscript = transRow.GetXmlTranscript();
					}
				
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetCursorTranscript", m_tmaxErrorBuilder.Message(ERROR_GET_CURSOR_TRANSCRIPT_EX), Ex);
			}
			
			return xmlTranscript;
		
		}// public CXmlTranscript GetCursorTranscript()

		/// <summary>Called to get the text associated with the rows selected by the caller</summary>
		/// <returns>The text that is currently selected by the user</returns>
		public string GetSelectedText()
		{
			int		iFirst = -1;
			int		iLast = -1;
			string	strText = "";

			try
			{
				//	Get the indexes of the current selection
				if(GetUltraSelectionRange(ref iFirst, ref iLast) == true)
				{
					strText = GetText(iFirst, iLast);

				}// if(GetUltraSelectionRange(ref iFirst, ref iLast) == true)	
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSelectedText", m_tmaxErrorBuilder.Message(ERROR_GET_SELECTED_TEXT_EX), Ex);
			}

			return strText;

		}// public string GetSelectedText()

		/// <summary>Called to save the current selections to the clipboard</summary>
		/// <returns>True if successful</returns>
		public bool CopySelections()
		{
			string	strSelections = "";
			bool	bSuccessful = false;

			try
			{
				if(GetSelectedCount() > 0)
				{
					//	Get the current selections as a text string
					strSelections = GetSelectedText();
				
					if(strSelections.Length > 0)
					{
						Clipboard.SetText(strSelections);
						bSuccessful = true;
					}

				}// if(GetSelectedCount() > 0)
	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CopySelections", m_tmaxErrorBuilder.Message(ERROR_COPY_SELECTIONS_EX), Ex);
			}

			return bSuccessful;

		}// public bool CopySelections()

		/// <summary>Called to print the current selections</summary>
		/// <returns>True if successful</returns>
		public bool PrintSelections()
		{
			bool bSuccessful = false;
			PrintDialog wndPrintSetup = null;

			try
			{
				if(GetSelectedCount() > 0)
				{
					//	Make sure we have a printer font
					if(m_printerFont == null)
					{
						try { m_printerFont = new Font("Courier New", 12); }
						catch(System.Exception Ex) { m_tmaxEventSource.FireDiagnostic(this, "PrintSelections", Ex); }
						
						if(m_printerFont == null)
							m_printerFont = this.Font;

					}// if(m_printerFont == null)
	
					//	Get the current selections as a text string
					m_strPrinterText = GetSelectedText();

					if(m_strPrinterText.Length > 0)
					{
						try
						{
							wndPrintSetup = new PrintDialog();
							wndPrintSetup.Document = m_docPrinter;

							if(wndPrintSetup.ShowDialog() == DialogResult.OK)
							{
								m_docPrinter.Print();

							}

							bSuccessful = true;
						}
						catch
						{
							Warn("Unable to start the print job. Verify that the printer is attached and operational");
						}

					}// if(strSelections.Length > 0)

				}// if(GetSelectedCount() > 0)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "PrintSelections", m_tmaxErrorBuilder.Message(ERROR_PRINT_SELECTIONS_EX), Ex);
			}

			return bSuccessful;

		}// public bool PrintSelections()

		/// <summary>Called to save the current selections</summary>
		/// <param name="strFileSpec">The target file specification (optional)</param>
		/// <returns>True if successful</returns>
		public bool SaveSelections(string strFileSpec)
		{
			CTmaxTransGridRow	row = null;
			StreamWriter		streamWriter = null;
			int					iFirst = -1;
			int					iLast = -1;
			bool				bSuccessful = false;

			try
			{
				//	Do we have anything to send to the file?
				if(GetUltraSelectionRange(ref iFirst, ref iLast) == true)
				{
					//	Open the output file
					if((streamWriter = OpenFileStream(strFileSpec)) != null)
					{
						for(int i = iFirst; i <= iLast; i++)
						{
							if((row = (CTmaxTransGridRow)(m_aDataSource.GetValue(i))) != null)
							{
								streamWriter.WriteLine(row.ToString());

							}// if((row = (CTmaxTransGridRow)(m_aDataSource.GetValue(i)))

						}// for(i = iFirst; i <= iLast; i++)
						
						bSuccessful = true;

					}// if((streamWriter = OpenFileStream(strFileSpec)) != null)

				}// if((xmlSelections != null) && (xmlSelections.Count > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SaveSelections", m_tmaxErrorBuilder.Message(ERROR_SAVE_SELECTIONS_EX), Ex);
			}
			finally
			{
				if(streamWriter != null)
				{
					try { streamWriter.Close(); }
					catch { }

					streamWriter = null;
				}
			
			}

			return bSuccessful;

		}// public bool SaveSelections(string strFileSpec)

		/// <summary>Called to open a file stream using the specified path</summary>
		/// <param name="strFileSpec">The target file specification (optional)</param>
		/// <returns>The associated file stream</returns>
		public System.IO.StreamWriter OpenFileStream(string strFileSpec)
		{
			System.IO.FileStream	fs = null;
			System.IO.StreamWriter	streamWriter = null;
			SaveFileDialog			saveFile = null;
			string					strMsg = "";

			try
			{
				//	Are we supposed to prompt the user for the filename?
				if((strFileSpec == null) || (strFileSpec.Length == 0))
				{
					//	Initialize the file selection dialog
					saveFile = new SaveFileDialog();
					saveFile.AddExtension = true;
					saveFile.CheckPathExists = true;
					saveFile.OverwritePrompt = true;
					saveFile.FileName = "";
					saveFile.DefaultExt = "txt";
					saveFile.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

					//	Open the dialog box
					if(saveFile.ShowDialog() == DialogResult.OK)
						strFileSpec = saveFile.FileName;
					else
						return null; // User cancelled

					//	Open the file
					try
					{
						if((fs = new FileStream(strFileSpec, FileMode.Create)) != null)
						{
							streamWriter = new StreamWriter(fs, System.Text.Encoding.Default);
						}

					}
					catch
					{
						//	Just use a message box to notify the user
						strMsg = String.Format("Unable to open {0} to save the selections", strFileSpec);
						MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					}

				}// if((strFileSpec == null) || (strFileSpec.Length == 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenFileStream", m_tmaxErrorBuilder.Message(ERROR_OPEN_FILE_STREAM_EX, strFileSpec), Ex);
				streamWriter = null;
			}

			return streamWriter;

		}// public System.IO.StreamWriter OpenFileStream(string strFileSpec)

		/// <summary>This method is called to add all objections in the specified collection</summary>
		/// <param name="tmaxObjections">The collection of objections to be added</param>]
		/// <returns>true if successful</returns>
		public bool AddObjections(CTmaxObjections tmaxObjections)
		{
			bool bSuccessful = false;

			try
			{
				if(m_tmaxObjections != null)
				{
					m_ctrlUltraGrid.SuspendLayout();
					
					foreach(CTmaxObjection O in tmaxObjections)
						AddObjection(O);
						
					bSuccessful = true;
					
					m_ctrlUltraGrid.ResumeLayout();

				}// if(m_tmaxObjections != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddObjections", m_tmaxErrorBuilder.Message(ERROR_ADD_OBJECTIONS_EX), Ex);
			}

			return bSuccessful;

		}// public bool AddObjections(CTmaxObjections tmaxObjections)

		/// <summary>This method is called to add an objection to the collection</summary>
		/// <param name="tmaxObjection">The objection to be added</param>]
		/// <returns>true if successful</returns>
		public bool AddObjection(CTmaxObjection tmaxObjection)
		{
			ArrayList	aRows = null;
			bool		bSuccessful = false;

			try
			{
				if(m_tmaxObjections != null)
				{
					//	Get the data source rows within the range of this objection
					if((aRows = GetSourceRows(tmaxObjection.FirstPL, tmaxObjection.LastPL)) != null)
					{
						//	Add to the row's collection
						foreach(CTmaxTransGridRow O in aRows)
						{
							O.AddObjection(tmaxObjection);

							SetObjectionProps(O);

							//Invalidate(O);
						}

					}// if((aRows = GetRowsInRange(tmaxObjection.FirstPL, tmaxObjection.LastPL)) != null)
					
					//	Add this objection to the collection
					m_tmaxObjections.Add(tmaxObjection);
					
					bSuccessful = true;
					
				}// if(m_tmaxObjections != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddObjection", m_tmaxErrorBuilder.Message(ERROR_ADD_OBJECTION_EX, tmaxObjection.GetErrorId()), Ex);
			}

			return bSuccessful;

		}// public bool AddObjection(CTmaxObjections tmaxObjection)

		/// <summary>This method is called to remove an objection</summary>
		/// <param name="tmaxObjection">The objection to be removed</param>]
		/// <returns>true if successful</returns>
		public bool RemoveObjection(CTmaxObjection tmaxObjection)
		{
			CTmaxTransGridRow tgRow = null;
			bool			  bSuccessful = false;

			try
			{
				if((m_tmaxObjections != null) && (m_aDataSource != null))
				{
					//	Get the rows within the range of this objection
					for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
					{
						if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)
						{
							if(tgRow.ContainsObjection(tmaxObjection) == true)
							{
								tgRow.GetObjections().Remove(tmaxObjection);

								SetObjectionProps(tgRow);

								Invalidate(tgRow);

							}// if(tgRow.GetObjections() != null)
							
						}// if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)

					}// for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)

					//	Remove this objection from the collection
					m_tmaxObjections.Remove(tmaxObjection);

					bSuccessful = true;

				}// if(m_tmaxObjections != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "RemoveObjection", m_tmaxErrorBuilder.Message(ERROR_REMOVE_OBJECTION_EX, tmaxObjection.GetErrorId()), Ex);
			}

			return bSuccessful;

		}// public bool RemoveObjection(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to update the rows associated with an objection</summary>
		/// <param name="tmaxObjection">The objection to be removed</param>]
		/// <returns>true if successful</returns>
		public bool UpdateObjection(CTmaxObjection tmaxObjection)
		{
			bool bSuccessful = false;

			try
			{
				if((m_tmaxObjections != null) && (m_aDataSource != null))
				{
					foreach(CTmaxTransGridRow O in m_aDataSource)
					{
						//	Is this row within the objection range?
						if((O.GetPL() >= tmaxObjection.FirstPL) && (O.GetPL() <= tmaxObjection.LastPL))
						{
							//	Should we add the objection?
							if(O.ContainsObjection(tmaxObjection) == false)
							{
								O.AddObjection(tmaxObjection);
							}
							
							//	The plaintiff/defendant assignment may have changed
							SetObjectionProps(O);
							Invalidate(O);

						}
						else
						{
							//	Should we remove the objection?
							if(O.ContainsObjection(tmaxObjection) == true)
							{
								O.RemoveObjection(tmaxObjection);
								SetObjectionProps(O);
								Invalidate(O);
							}

						}// if((O.GetPL() >= tmaxObjection.FirstPL) && (O.GetPL() <= tmaxObjection.LastPL))

					}// foreach(CTmaxTransGridRow O in m_aDataSource)

					bSuccessful = true;

				}// if(m_tmaxObjections != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "UpdateObjection", m_tmaxErrorBuilder.Message(ERROR_UPDATE_OBJECTION_EX, tmaxObjection.GetErrorId()), Ex);
			}

			return bSuccessful;

		}// public bool UpdateObjection(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to select the specified objection</summary>
		/// <param name="tmaxObjection">The objection to be selected</param>]
		/// <returns>true if successful</returns>
		public bool SelectObjection(CTmaxObjection tmaxObjection)
		{
			bool bSuccessful = false;

			try
			{
				if((m_tmaxObjections != null) && (m_tmaxObjections.Contains(tmaxObjection) == true))
				{
					EnsureVisible(tmaxObjection.FirstPL, false);

					bSuccessful = true;

				}// if((m_tmaxObjections != null) && (m_tmaxObjections.Contains(tmaxObjection) == true))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SelectObjection", m_tmaxErrorBuilder.Message(ERROR_SELECT_OBJECTION_EX, tmaxObjection.GetErrorId()), Ex);
			}

			return bSuccessful;

		}// public bool SelectObjection(CTmaxObjection tmaxObjection)

		/// <summary>Called to set the objection properties for the specified row</summary>
		/// <param name="tgRow">The row to be set</param>]
		/// <returns>true if successful</returns>
		public bool SetObjectionProps(CTmaxTransGridRow tgRow)
		{
			bool	bSuccessful = false;
			int		iIndex = 0;

			try
			{
				Debug.Assert(tgRow != null);
				
				if(m_ctrlUltraGrid.Rows != null)
				{
					iIndex = tgRow.GetGridIndex();

					if((iIndex >= 0) && (iIndex < m_ctrlUltraGrid.Rows.Count))
					{
						bSuccessful = SetObjectionProps(tgRow, m_ctrlUltraGrid.Rows[iIndex]);

					}

				}// if(m_ctrlUltraGrid.Rows != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetObjectionProps", Ex);
			}

			return bSuccessful;

		}// public bool SetObjectionProps(CTmaxTransGridRow tgRow)

		/// <summary>Called to set the row's properties that are related to objections</summary>
		/// <param name="tgRow">The row retrieved from the data set</param>
		/// <param name="ugRow">The row in the grid bound to the data row</param>
		/// <returns>true if successful</returns>
		public bool SetObjectionProps(CTmaxTransGridRow tgRow, UltraGridRow ugRow)
		{
			bool	bSuccessful = false;
			string	strToolTip = "";

			try
			{
				Debug.Assert(tgRow != null);
				Debug.Assert(ugRow != null);
				
				if(ugRow.Cells != null)
				{
					//	Set the tool-tip information
					strToolTip = tgRow.GetObjectionsToolTipText();
					ugRow.Cells[m_iPlaintiffObjectionsIndex].ToolTipText = strToolTip;
					ugRow.Cells[m_iDefendantObjectionsIndex].ToolTipText = strToolTip;

					//	Set the objection indicators
					SetObjectionMarkers(tgRow, ugRow);
					
					bSuccessful = true;
				
				}// if(ugRow.Cells != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetObjectionProps", Ex);
			}

			return bSuccessful;

		}// public bool SetObjectionProps(CTmaxTransGridRow tgRow, UltraGridRow ugRow)

		/// <summary>Called to set the row's colors for the objection highlight cells</summary>
		/// <param name="tgRow">The row retrieved from the data set</param>
		/// <param name="ugRow">The row in the grid bound to the data row</param>
		/// <returns>true if successful</returns>
		public bool SetObjectionMarkers(CTmaxTransGridRow tgRow, UltraGridRow ugRow)
		{
			bool bSuccessful = false;
			int iPlaintiffCount = 0;
			int iDefendantCount = 0;

			try
			{
				Debug.Assert(tgRow != null);
				Debug.Assert(ugRow != null);

				if(ugRow.Cells != null)
				{
					//	Get the total number of objections for each party
					tgRow.GetObjectionsCount(ref iPlaintiffCount, ref iDefendantCount);

					if(iPlaintiffCount > 0)
					{
						ugRow.Cells[m_iPlaintiffObjectionsIndex].Appearance.BackColor = this.PlaintiffColor;
						ugRow.Cells[m_iPlaintiffObjectionsIndex].SelectedAppearance.BackColor = this.PlaintiffColor;
					}
					else
					{
						ugRow.Cells[m_iPlaintiffObjectionsIndex].Appearance.BackColor = System.Drawing.Color.White;
						ugRow.Cells[m_iPlaintiffObjectionsIndex].SelectedAppearance.BackColor = System.Drawing.SystemColors.Highlight;
					}

					if(iDefendantCount > 0)
					{
						ugRow.Cells[m_iDefendantObjectionsIndex].Appearance.BackColor = this.DefendantColor;
						ugRow.Cells[m_iDefendantObjectionsIndex].SelectedAppearance.BackColor = this.DefendantColor;
					}
					else
					{
						ugRow.Cells[m_iDefendantObjectionsIndex].Appearance.BackColor = System.Drawing.Color.White;
						ugRow.Cells[m_iDefendantObjectionsIndex].SelectedAppearance.BackColor = System.Drawing.SystemColors.Highlight;
					}

					bSuccessful = true;

				}// if(ugRow.Cells != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetObjectionMarkers", Ex);
			}

			return bSuccessful;

		}// public bool SetObjectionMarkers(CTmaxTransGridRow tgRow, UltraGridRow ugRow)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);
			
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Do the base class processing first
			base.SetErrorStrings();
			
			//	Add placeholders for the reserved strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active deposition.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create a new data source.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new row to the transcript grid.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the transcript grid layout.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the transcript grid's data source.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting set the text color from StartPL = %1 to StopPL = %2 to %3");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to activate the designation: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the selected rows from the grid.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the selected transcript lines.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the selected transcript range.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to select the specified row.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to select the specified rows.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the transcript at the cursor position.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active objections.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the rows in the range: FirstPL=%1 LastPL=%2");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the rows from an objection: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding an objection: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding add a collection of objections.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while removing an objection: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while updating an objection: %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the grid row's Objections collection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while selecting an objection: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the text in the range: %1 to %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the selected text.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to copy the selected rows to the clipboard.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to print the selected rows.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to save the selected rows.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the file: filename = %1");

		}// protected void SetErrorStrings()

		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTmaxTransGridCtrl));
			this.m_ctrlUltraGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.m_ctrlUltraGridImages = new System.Windows.Forms.ImageList(this.components);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlUltraGrid
			// 
			this.m_ctrlUltraGrid.AllowDrop = true;
			appearance1.BackColor = System.Drawing.Color.White;
			this.m_ctrlUltraGrid.DisplayLayout.Appearance = appearance1;
			ultraGridBand1.ColHeadersVisible = false;
			ultraGridBand1.GroupHeadersVisible = false;
			ultraGridBand1.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.BorderStyleSummaryFooter = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.BorderStyleSummaryFooterCaption = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.BorderStyleSummaryValue = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			appearance2.BackColor = System.Drawing.Color.White;
			ultraGridBand1.Override.RowAppearance = appearance2;
			this.m_ctrlUltraGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.m_ctrlUltraGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlUltraGrid.DisplayLayout.MaxColScrollRegions = 1;
			this.m_ctrlUltraGrid.DisplayLayout.MaxRowScrollRegions = 1;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowGroupSwapping = Infragistics.Win.UltraWinGrid.AllowGroupSwapping.NotAllowed;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraGrid.DisplayLayout.Override.BorderStyleCardArea = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlUltraGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlUltraGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlUltraGrid.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
			this.m_ctrlUltraGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraGrid.DisplayLayout.Override.TipStyleRowConnector = Infragistics.Win.UltraWinGrid.TipStyle.Hide;
			this.m_ctrlUltraGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.m_ctrlUltraGrid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlUltraGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlUltraGrid.ImageList = this.m_ctrlUltraGridImages;
			this.m_ctrlUltraGrid.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlUltraGrid.Name = "m_ctrlUltraGrid";
			this.m_ctrlUltraGrid.Size = new System.Drawing.Size(200, 168);
			this.m_ctrlUltraGrid.TabIndex = 2;
			this.m_ctrlUltraGrid.Visible = false;
			this.m_ctrlUltraGrid.DragLeave += new System.EventHandler(this.OnUltraDragLeave);
			this.m_ctrlUltraGrid.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.OnUltraQueryContinueDrag);
			this.m_ctrlUltraGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnUltraMouseDown);
			this.m_ctrlUltraGrid.DoubleClick += new System.EventHandler(this.OnUltraDblClick);
			this.m_ctrlUltraGrid.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.OnUltraAfterSelChanged);
			this.m_ctrlUltraGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.OnUltraDragOver);
			this.m_ctrlUltraGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnUltraDragDrop);
			this.m_ctrlUltraGrid.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnUltraDragEnter);
			// 
			// m_ctrlUltraGridImages
			// 
			this.m_ctrlUltraGridImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlUltraGridImages.ImageStream")));
			this.m_ctrlUltraGridImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlUltraGridImages.Images.SetKeyName(0, "");
			this.m_ctrlUltraGridImages.Images.SetKeyName(1, "");
			this.m_ctrlUltraGridImages.Images.SetKeyName(2, "");
			this.m_ctrlUltraGridImages.Images.SetKeyName(3, "");
			this.m_ctrlUltraGridImages.Images.SetKeyName(4, "");
			this.m_ctrlUltraGridImages.Images.SetKeyName(5, "blue_tall_stripe.bmp");
			// 
			// CTmaxTransGridCtrl
			// 
			this.Controls.Add(this.m_ctrlUltraGrid);
			this.Name = "CTmaxTransGridCtrl";
			this.Size = new System.Drawing.Size(200, 168);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraGrid)).EndInit();
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This gets the row at the specified location within the client area</summary>
		/// <param name="iX">The X position in client coordinates</param>
		/// <param name="iY">The Y position in client coordinates</param>
		private UltraGridRow GetUltraRow(int iX, int iY)
		{
			UIElement		uiElement = null;
			UltraGridCell	cell = null;
				
			try
			{
				//	Retrieve the UIElement at the specified location
				uiElement = m_ctrlUltraGrid.DisplayLayout.UIElement.ElementFromPoint(new Point(iX, iY));
					
				//	Use the UI element to retrieve the cell
				if(uiElement != null)
					cell = (UltraGridCell)(uiElement.GetContext(typeof(UltraGridCell)));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetUltraRow", Ex);
			}

			if(cell != null)
				return cell.Row;
			else
				return null;

		}// private UltraGridRow GetUltraRow(int iX, int iY)

		/// <summary>This gets the row at the current mouse position</summary>
		private UltraGridRow GetUltraRow()
		{
			if((m_ctrlUltraGrid != null) && (m_ctrlUltraGrid.IsDisposed == false))
			{
				//	Get the current mouse position and convert to client coordinates
				Point Pos = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
				Pos = m_ctrlUltraGrid.PointToClient(Pos);

				return GetUltraRow(Pos.X, Pos.Y);
			
			}
			else
			{
				return null;
			}

		}// private UltraGridRow GetUltraRow()

		/// <summary>This gets grid row assocaited with the specified source row</summary>
		/// <param name="tgRow">The source row</param>
		/// <returns>The associated grid row</returns>
		private UltraGridRow GetUltraRow(CTmaxTransGridRow tgRow)
		{
			UltraGridRow row = null;
			
			Debug.Assert(tgRow != null);

			if((m_ctrlUltraGrid != null) && (m_ctrlUltraGrid.Rows != null))
			{
				if((tgRow.GetGridIndex() >= 0) && (tgRow.GetGridIndex() < m_ctrlUltraGrid.Rows.Count))
					row = m_ctrlUltraGrid.Rows[tgRow.GetGridIndex()];
			}

			return row;

		}// private UltraGridRow GetUltraRow(CTmaxTransGridRow tgRow)

		/// <summary>This gets grid cell assocaited with the specified source row and column</summary>
		/// <param name="tgRow">The source row</param>
		/// <returns>The associated grid cell</returns>
		private UltraGridCell GetUltraCell(CTmaxTransGridRow tgRow, string strColumn)
		{
			UltraGridRow  row = null;
			UltraGridCell cell = null;

			Debug.Assert(tgRow != null);
			Debug.Assert(strColumn != null);

			//	Get the specified row
			if((row = GetUltraRow(tgRow)) != null)
			{
				if(row.Cells != null)
					cell = row.Cells[strColumn];
			}

			return cell;

		}// private UltraGridCell GetUltraCell(CTmaxTransGridRow tgRow, string strColumn)

		/// <summary>This method will get the index of the first and last row selected by the user</summary>
		///	<param name="rFirstIndex">Location where first selected index is to be stored</param>
		///	<param name="rLastIndex">Location where last selected index is to be stored</param>
		/// <returns>the number of selected lines</returns>
		public bool GetUltraSelectionRange(ref int rFirstIndex, ref int rLastIndex)
		{
			int	iIndex = 0;
			int	i = 0;

			try
			{
				//	Initialize the indexes
				rFirstIndex = -1;
				rLastIndex = -1;

				//	Have any rows been selected by the user?
				if((m_ctrlUltraGrid.Selected != null) && (m_ctrlUltraGrid.Selected.Rows != null) && (m_ctrlUltraGrid.Selected.Rows.Count > 0))
				{
					//	Initialize the indexes
					rFirstIndex = m_ctrlUltraGrid.Selected.Rows[0].Index;
					rLastIndex  = rFirstIndex;

					//	Get the indexes of the first and last selected row
					for(i = 1; i < m_ctrlUltraGrid.Selected.Rows.Count; i++)
					{
						iIndex = m_ctrlUltraGrid.Selected.Rows[i].Index;

						if(iIndex < rFirstIndex)
							rFirstIndex = iIndex;
						if(iIndex > rLastIndex)
							rLastIndex = iIndex;
					}

				}// if((m_ctrlUltraGrid.Selected.Rows != null) && (m_ctrlUltraGrid.Selected.Rows.Count > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetUltraSelectionRange", Ex);
				rFirstIndex = -1;
				rLastIndex = -1;
			}

			return ((rFirstIndex >= 0) && (rLastIndex >= 0));

		}// public long GetUltraSelectionRange(ref int rFirstIndex, ref int rLastIndex)

		/// <summary>Called to invalidate the specified column to force a refresh of the display</summary>
		/// <param name="tgRow">The source row</param>
		private void Invalidate(CTmaxTransGridRow tgRow, string strColumn)
		{
			UltraGridRow row = null;
			UltraGridCell cell = null;

			Debug.Assert(tgRow != null);

			try
			{
				//	Did the caller specify a specific column?
				if((strColumn != null) && (strColumn.Length > 0))
					cell = GetUltraCell(tgRow, strColumn);
					
				//	Are we invalidating a column or the whole row?
				if(cell != null)
				{
					cell.Refresh();
				}
				else
				{
					if((row = GetUltraRow(tgRow)) != null)
						row.Refresh();
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Invalidate", Ex);
			}

		}// private void Invalidate(CTmaxTransGridRow tgRow, string strColumn)

		/// <summary>This method is called to invalidate the specified row</summary>
		/// <param name="tgRow">The row to be invalidated</param>]
		private void Invalidate(CTmaxTransGridRow tgRow)
		{
			Invalidate(tgRow, "");
		}

		/// <summary>This method is called to get the index of the row with the specified PL value</summary>
		/// <param name="lPL">The desired composite PageLine value</param>
		/// <param name="bExact">true if exact match required</param>
		/// <returns></returns>
		private int GetRowIndex(long lPL, bool bExact)
		{
			CTmaxTransGridRow	tgRow = null;

			Debug.Assert(m_aDataSource != null);
			if(m_aDataSource == null) return -1;
			
			for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
			{
				if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)
				{
					if(tgRow.GetPL() == lPL)
					{
						return i;
					}
					else if(tgRow.GetPL() > lPL) // Gone too far
					{
						//	Are we looking for an exact match?
						if(bExact == true)
						{
							return -1;
						}
						else
						{
							if(i > 0)
								return (i - 1); // Previous row
							else
								return i;
						}
					
					}
					
				}// if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)
			
			}// for(int i = 0; i <= ds.GetUpperBound(0); i++)
			
			return -1;
			
		}// private int GetRowIndex(long lPL, bool bExact)

		/// <summary>This method is called to get the collection of source rows that fall within the specified range</summary>
		/// <param name="lFirstPL">The first page/line in the desired range</param>
		/// <param name="lLastPL">The last page/line in the desired range</param>
		/// <returns>the collection of rows</returns>
		private ArrayList GetSourceRows(long lFirstPL, long lLastPL)
		{
			ArrayList			aRows = new ArrayList();
			CTmaxTransGridRow	tgRow = null;

			Debug.Assert(m_aDataSource != null);
			if(m_aDataSource == null) return null;

			try
			{
				for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
				{
					if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)
					{
						if(tgRow.GetPL() >= lFirstPL)
						{
							//	Is it within range?
							if(tgRow.GetPL() <= lLastPL)
							{
								//	Add to the collection
								aRows.Add(tgRow);
							}
							else
							{
								//	Nothing more to do
								break;
							}

						}// if(tgRow.GetPL() >= lFirstPL)

					}// if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)

				}// for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceRows", m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_ROWS_EX, lFirstPL, lLastPL), Ex);
			}
			
			//	Did we find any rows within the specified range?
			if((aRows != null) && (aRows.Count > 0))
				return aRows;
			else
				return null;

		}// private ArrayList GetSourceRows(long lFirstPL, long lLastPL)

		/// <summary>Called to get the text in the range specified by the caller</summary>
		///	<param name="iStart">The first line to be included in the result</param>
		///	<param name="iLast">The last line to be included in the result</param>
		/// <returns>The text in the specified range</returns>
		public string GetText(int iFirst, int iLast)
		{
			CTmaxTransGridRow	row = null;
			int					i = 0;
			string				strText = "";

			try
			{
				//	Do we have a valid transcript?
				if((m_aDataSource != null) && (m_aDataSource.GetUpperBound(0) >= 0))
				{
					//	Make sure the indexes are within range
					if(iFirst < 0) iFirst = 0;
					if(iLast > m_aDataSource.GetUpperBound(0)) iLast = m_aDataSource.GetUpperBound(0);
					if(iFirst > iLast) iFirst = iLast;

					//	Get the text in the first specified row
					if((row = (CTmaxTransGridRow)(m_aDataSource.GetValue(iFirst))) != null)
						strText = row.ToString();

					//	Get the text in the remaining rows in the specified range
					for(i = (iFirst + 1); i <= iLast; i++)
					{
						strText += "\r\n";
						
						if((row = (CTmaxTransGridRow)(m_aDataSource.GetValue(i))) != null)
							strText += row.ToString();

					}// for(i = iStart; i <= iStop; i++)

				}// if((m_aDataSource != null) && (m_aDataSource.GetUpperBound(0) >= 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetText", m_tmaxErrorBuilder.Message(ERROR_GET_TEXT_EX, iFirst, iLast), Ex);
			}

			return strText;

		}// public string GetText(int iFirst, int iLast)
		
		/// <summary>This method is called to see if the specified row is visible</summary>
		/// <param name="iRow">The index of the row to be checked</param>
		/// <returns>true if visibile</returns>
		private bool IsVisible(UltraGridRow row)
		{
			Infragistics.Win.UIElement	rowElement = null;
			bool						bVisible = false;

			try
			{
				rowElement = m_ctrlUltraGrid.DisplayLayout.UIElement.GetDescendant(typeof(Infragistics.Win.UltraWinGrid.RowUIElement), row);
				
				if(rowElement != null)
				{ 
					if (Rectangle.Intersect(m_ctrlUltraGrid.ClientRectangle, rowElement.Rect).IsEmpty == false) 
						bVisible = true; 
				} 
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IsVisible", Ex);
			}
 
			return bVisible; 
			
		}// private bool IsVisible(int iRow)
		
		/// <summary>This method is called to scroll the specified row to the top of the grid</summary>
		/// <param name="row">The row to move to the top of the grid</param>
		private void ScrollToTop(UltraGridRow row)
		{
			Debug.Assert(row != null);
			if(row == null) return;
			
			try
			{
				//	Make sure the row is in the visible portion of the grid
				m_ctrlUltraGrid.DisplayLayout.RowScrollRegions[0].ScrollRowIntoView(row);
				
				//	Prevent updates while we scroll
				m_ctrlUltraGrid.BeginUpdate();

				// Scroll the top row to the top
				while(m_ctrlUltraGrid.DisplayLayout.RowScrollRegions[0].VisibleRows[0].Row != row)
				{
					m_ctrlUltraGrid.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineDown);
				}

				// Turn display refresh back on 
				m_ctrlUltraGrid.EndUpdate();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ScrollToTop", Ex);
			}

		}// private void ScrollToTop(UltraGridRow row)
		
		/// <summary>This method is called to create a data source for the grid</summary>
		/// <param name="iRows">The number of rows in the new data source</param>
		/// <returns>A reference to the source array object</returns>
		private Array CreateDataSource(int iRows)
		{
			try
			{
				return new CTmaxTransGridRow[iRows];
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDataSource", m_tmaxErrorBuilder.Message(ERROR_CREATE_DATA_SOURCE_EX), Ex);
				return null;
			}
			
		}// private Array CreateDataSource(int iRows)
		
		/// <summary>This method is called to assign a new data source</summary>
		/// <param name="aSource">The data source to be assigned to the grid</param>
		/// <returns>true if successful</returns>
		private bool SetDataSource(Array aSource)
		{
			Debug.Assert(aSource != null);
			Debug.Assert(m_ctrlUltraGrid != null);
			Debug.Assert(m_ctrlUltraGrid.IsDisposed == false);
			
			if(aSource == null) return false;
			if(m_ctrlUltraGrid == null) return false;
			if(m_ctrlUltraGrid.IsDisposed == true) return false;
			
			try
			{
				//	Assign the new source to the grid
				m_ctrlUltraGrid.DataSource = aSource;
				m_aDataSource = aSource;
				
				//	Make sure the grid is visible
				if(m_ctrlUltraGrid.Visible == false)
					m_ctrlUltraGrid.Visible = true;
			
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDataSource", m_tmaxErrorBuilder.Message(ERROR_SET_DATA_SOURCE_EX), Ex);
				return false;
			}
			
		}// private bool SetDataSource(Array aSource)

		/// <summary>This method is called to set the active collection of objections</summary>
		/// <param name="tmaxObjections">The new collection</param>]
		/// <returns>true if successful</returns>
		private bool SetObjections(CTmaxObjections tmaxObjections)
		{
			bool bSuccessful = false;

			try
			{
				Debug.Assert(m_tmaxObjections != null);
				
				//	Clear the existing collection
				m_tmaxObjections.Clear();

				//m_tmaxEventSource.InitElapsed();
				
				//	Transfer all objections to our local collection and make sure they've been sorted
				if(tmaxObjections != null)
				{
					foreach(CTmaxObjection O in tmaxObjections)
						m_tmaxObjections.Add(O);

					//	Make sure they are in PL order. This helps speed up the
					//	initialization of the rows as they are added to the grid
					m_tmaxObjections.Sort();

				}// if(tmaxObjections != null)

				//m_tmaxEventSource.FireElapsed(this, "SetObjections", "Time to sort " + m_tmaxObjections.Count.ToString() + " objections: ");

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetObjections", m_tmaxErrorBuilder.Message(ERROR_SET_OBJECTIONS_EX), Ex);
			}

			return bSuccessful;

		}// public bool SetObjections(CTmaxObjections tmaxObjections)

		/// <summary>This method is called to populate the Objections collection for the specified row</summary>
		/// <param name="tgRow">The row being populated</param>
		/// <param name="iStart">The index at which to start searching the Objections collection</param>
		/// <returns>the index of the first objection associated with the row</returns>
		private int FillObjections(CTmaxTransGridRow tgRow, int iStart)
		{
			int	iFirst = -1;

			//	NOTE:	For this to work the Objections collection MUST 
			//			be sorted by FirstPL
			if((iStart < 0) || (iStart >= m_tmaxObjections.Count))
				iStart = 0;
			
			try
			{
				Debug.Assert(tgRow != null);

				for(int i = iStart; i < m_tmaxObjections.Count; i++)
				{
					if(tgRow.GetPL() >= m_tmaxObjections[i].FirstPL)
					{
						if(tgRow.GetPL() <= m_tmaxObjections[i].LastPL)
						{
							//	Add this objection to the row
							tgRow.AddObjection(m_tmaxObjections[i]);
							
							//	Keep track of the first objection that this row falls in
							if(iFirst < 0)
								iFirst = i;

						}// if(tgRow.GetPL() <= m_tmaxObjections[i].LastPL)
						
					}
					else
					{
						//	We've gone past any possible objections for this row
						//	because we know the list is sorted by FirstPL
						break;

					}// if(tgRow.GetPL() >= m_tmaxObjections[i].FirstPL)

				}//	for(i = iStart; i < m_tmaxObjections.Count; i++)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillObjections", m_tmaxErrorBuilder.Message(ERROR_FILL_OBJECTIONS_EX), Ex);
				iFirst = 0;
			}

			return ((iFirst >= 0) ? iFirst : 0);

		}// private int FillObjections(CTmaxTransGridRow tgRow, int iStart)

		/// <summary>This method is called to add the specified transcript to the grid</summary>
		/// <param name="xmlTranscript">The XML transcript bounded to the row to be added</param>
		/// <param name="aSource">The data source array where the new row descriptor should be stored</param>
		/// <param name="iIndex">The index into the data source array</param>
		/// <returns>true if successful</returns>
		private bool Add(CXmlTranscript xmlTranscript, Array aSource, int iIndex)
		{
			CTmaxTransGridRow	row	= null;
			CTmaxTransGridRow	previous = null;
			bool				bSuccessful = false;
			bool				bEndPrologue = false;
			
			Debug.Assert(aSource != null);
			Debug.Assert(iIndex >= 0);
			Debug.Assert(iIndex <= aSource.GetUpperBound(0));

			try
			{
				//	Get the previous row
				if(iIndex > 0)
					previous = aSource.GetValue(iIndex - 1) as CTmaxTransGridRow;
					
				//	Allocate the row and transcript objects
				row = new CTmaxTransGridRow();
				
				//	Set the row properties
				row.SetXmlTranscript(xmlTranscript);
				
				//	Set the pause indicator (except for last line)
				if((m_dPauseThreshold > 0) && (iIndex < aSource.GetUpperBound(0)))
				{
					row.SetShowPause(row.GetDuration() > m_dPauseThreshold);
				}
				else
				{
					row.SetShowPause(false);
				}
				
				//	Is this a page break
				if((previous == null) || (previous.GetPage() != row.GetPage()))
					row.GP = row.GetPage().ToString();
				
				//	Are we processing the prologue?
				if(m_bPrologue == true)
				{
					//	The first line of synchronized text ends the prologue
					if(xmlTranscript.Synchronized == true)
					{
						m_bPrologue = false;
						
						//	There may not have been a prologue
						if(iIndex > 0)
							bEndPrologue = true;
					}
				
				}
				
				//	Have we reached the end of the prologue?
				if(bEndPrologue == true)
				{
					//	Show the segment break indicator even though the
					//	prologue actually belongs to the first segment
					row.SetSegmentBreak(true);
				}
				else if((previous != null) && (previous.GetSegment() != row.GetSegment()))
				{
					row.SetSegmentBreak(true);
					
					//	The last row in a segment never shows the Pause indicator
					if(previous != null)
						previous.SetShowPause(false);
				}
				
				//	Add the row to the data source
				aSource.SetValue(row, iIndex);
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Add(CXmlTranscript xmlTranscript, Array aSource, int iIndex)
		
		/// <summary>This method is called initialize the grid layout after loading a new data source</summary>
		private void InitializeLayout()
		{
			UltraGridRow		ultra = null;
			CTmaxTransGridRow	row = null;
			int					iIndex = 0;

			try
			{
				//	This shouldn't be necessary but it is
				m_ctrlUltraGrid.DisplayLayout.Bands[0].ColHeadersVisible = false;
				m_ctrlUltraGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;

				//m_ctrlUltraGrid.DisplayLayout.Bands[0].Override.BorderStyleCell = UIElementBorderStyle.Solid;
				//m_ctrlUltraGrid.DisplayLayout.Bands[0].Override.RowAppearance.BorderColor = System.Drawing.Color.White;
				//m_ctrlUltraGrid.DisplayLayout.Bands[0].Override.CellAppearance.BorderAlpha = Alpha.Transparent;

				//ultraGridBand1.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
				//m_ctrlUltraGrid.DisplayLayout.Override.BorderStyleCell = UIElementBorderStyle.Solid;
				//m_ctrlUltraGrid.DisplayLayout.Override.CellAppearance.BorderColor = System.Drawing.Color.Black;

				//	Set the column order
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_LINE_IMAGE].Header.VisiblePosition = 0;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_ACTIVE_DESG].Header.VisiblePosition = 1;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_PAGE].Header.VisiblePosition = 2;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_LINE].Header.VisiblePosition = 3;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_QA].Header.VisiblePosition = 4;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_TEXT].Header.VisiblePosition = 5;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_DEFENDANT_OBJECTIONS].Header.VisiblePosition = 6;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_PLAINTIFF_OBJECTIONS].Header.VisiblePosition = 7;

				//	Set the fixed width for image columns
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_LINE_IMAGE].Width = 16;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_DEFENDANT_OBJECTIONS].Width = 4;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_PLAINTIFF_OBJECTIONS].Width = 4;
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_ACTIVE_DESG].Width = 16;

				//	Size the variable columns to match the text they contain
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_PAGE].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_LINE].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_QA].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
				m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_TEXT].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);

				m_iDefendantObjectionsIndex = m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_DEFENDANT_OBJECTIONS].Index;
				m_iPlaintiffObjectionsIndex = m_ctrlUltraGrid.DisplayLayout.Bands[0].Columns[GRID_COLUMN_PLAINTIFF_OBJECTIONS].Index;
				

				//	Set the column images
				for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
				{
					if((row = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)
					{
						//	Set the index to allow for fast lookups
						row.SetGridIndex(i);
						
						if((ultra = m_ctrlUltraGrid.Rows[i]) != null)
						{
							ultra.Cells[GRID_COLUMN_LINE_IMAGE].Appearance.Image = row.GetImageIndex();

							if(m_tmaxObjections.Count > 0)
							{
								iIndex = FillObjections(row, iIndex);

								if(row.GetObjectionsCount() > 0)
									SetObjectionProps(row, ultra);

							}// if(m_tmaxObjections.Count > 0)

						}// if((ultra = m_ctrlUltraGrid.Rows[i]) != null)
						
					}// if((row = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)
					
				}// for(int i = 0; i <= m_aDataSource.GetUpperBound(); i++)

				//m_ctrlUltraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeLayout", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_LAYOUT_EX), Ex);
			}

		}// private void InitializeLayout()
		
		/// <summary>This method is called to set the pause indicator threshold</summary>
		/// <param name="dThreshold">The new threshold in seconds</param>
		/// <returns>true if successful</returns>
		private bool SetPauseThreshold(double dThreshold)
		{
			UltraGridRow		ultra = null;
			CTmaxTransGridRow	row = null;
			
			//	Has the treshold changed?
			if(m_dPauseThreshold != dThreshold)
				m_dPauseThreshold = dThreshold;
			else
				return true;
		
			//	Must have a valid grid control
			Debug.Assert(m_ctrlUltraGrid != null);
			Debug.Assert(m_ctrlUltraGrid.IsDisposed == false);
			if(m_ctrlUltraGrid == null) return false;
			if(m_ctrlUltraGrid.IsDisposed == true) return false;
			
			//	Nothing more to do if no data source
			if(m_aDataSource == null) return true;
			
			try
			{
				//	Update each row in the grid
				for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
				{
					if((row = (CTmaxTransGridRow)(m_aDataSource.GetValue(i))) != null)
					{
						//	Is this the last in the deposition?
						if((i == m_aDataSource.GetUpperBound(0)) || (m_dPauseThreshold <= 0))
						{
							row.SetShowPause(false);
						}
						else
						{
							//	Is this the last row in the segment?
							if(((CTmaxTransGridRow)(m_aDataSource.GetValue(i + 1))).GetSegmentBreak() == true)
								row.SetShowPause(false);
							else
								row.SetShowPause(row.GetDuration() > m_dPauseThreshold);
								
						}// if(i == m_aDataSource.GetUpperBound(0))
						
						if((ultra = m_ctrlUltraGrid.Rows[row.GetGridIndex()]) != null)
						{
							ultra.Cells[GRID_COLUMN_LINE_IMAGE].Appearance.Image = row.GetImageIndex();
						}
					
					}// if((row = (CTmaxTransGridRow)(m_aDataSource.GetValue(i))) != null)
					
				}// for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)	
				
				return true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetPauseThreshold", Ex);
				return false;
			}
		
		}// private bool SetPauseThreshold(double dThreshold)
		
		/// <summary>This method is called to set the color of the specified lines of text</summary>
		/// <param name="lStartPL">The composite page/line to start with</param>
		/// <param name="lStopPL">The composite page/line to stop with</param>
		/// <param name="sysColor">The system drawing color</param>
		/// <returns>true if successful</returns>
		public bool SetTextColor(long lStartPL, long lStopPL, System.Drawing.Color sysColor)
		{
			long lPL = 0;
			bool bSuccessful = false;
			
			Debug.Assert(m_ctrlUltraGrid != null);
			Debug.Assert(m_ctrlUltraGrid.IsDisposed == false);
			if(m_ctrlUltraGrid == null) return false;
			if(m_ctrlUltraGrid.IsDisposed == true) return false;

			try
			{
				//	Nothing to do if the grid is not loaded
				if(m_aDataSource == null) return true;
				if(m_ctrlUltraGrid.Rows == null) return true;
				
				for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
				{
					//	Get the page/line for this row
					lPL = ((CTmaxTransGridRow)m_aDataSource.GetValue(i)).GetPL();
					
					//	Is this row within the caller's range?
					if((lPL >= lStartPL) && ((lPL <= lStopPL) || (lStopPL <= 0)))
					{
						try 
						{ 
							m_ctrlUltraGrid.Rows[i].Appearance.ForeColor = sysColor; 
							bSuccessful = true;
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireDiagnostic(this, "SetTextColor", Ex);
						}	
					
					}// if((lPL >= lStartPL) && (lPL <= lStopPL))
					else if((lStopPL > 0) && (lPL > lStopPL))
					{
						//	We've gone beyond the stop point
						break;
					}
					
				}// for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)	
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetTextColor", m_tmaxErrorBuilder.Message(ERROR_SET_TEXT_COLOR_EX, lStartPL, lStopPL, sysColor), Ex);
			}
			
			return bSuccessful;
		
		}// public bool SetTextColor(long lStartPL, long lStopPL, System.Drawing.Color sysColor)

		/// <summary>This method is called to select the specified row</summary>
		/// <param name="row">The row being selected</param>
		/// <returns>true if successful</returns>
		private bool SetSelection(UltraGridRow row)
		{
			bool bSuccessful = false;

			try
			{
				m_ctrlUltraGrid.Selected.Rows.Clear();
				m_ctrlUltraGrid.ActiveRow = null;

				if(row != null)
				{
					row.Selected = true;
					m_ctrlUltraGrid.ActiveRow = row;
				}

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetSelection(UltraGridRow row)

		/// <summary>This method will bubble the selection changed event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnUltraAfterSelChanged(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
			if(SelChanged != null)
				SelChanged(this, System.EventArgs.Empty);
		}

		/// <summary>This method will handle DoubleClick events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnUltraDblClick(object sender, System.EventArgs e)
		{
			if(DblClick != null)
				DblClick(this, System.EventArgs.Empty);
		}

		/// <summary>This method will handle DragDrop events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnUltraDragDrop(object sender, DragEventArgs e)
		{
			try
			{
				//	Raise the control's DragDrop event
				OnDragDrop(e);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnUltraDragDrop", Ex);
			}

		}// private void OnUltraDragDrop(object sender, DragEventArgs e)

		/// <summary>This method will handle DragEnter events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnUltraDragEnter(object sender, DragEventArgs e)
		{
			try
			{
				//	Raise the control's DragEnter event
				OnDragEnter(e);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnUltraDragEnter", Ex);
			}

		}// private void OnUltraDragEnter(object sender, DragEventArgs e)

		/// <summary>This method will handle DragLeave events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnUltraDragLeave(object sender, System.EventArgs e)
		{
			try
			{
				//	Raise the control's DragLeave event
				OnDragLeave(e);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnUltraDragLeave", Ex);
			}

		}// private void OnUltraDragLeave(object sender, DragEventArgs e)

		/// <summary>This method will handle DragOver events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnUltraDragOver(object sender, DragEventArgs e)
		{
			try
			{
				//	Raise the control's DragOver event
				OnDragOver(e);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnUltraDragOver", Ex);
			}

		}// private void OnUltraDragOver(object sender, DragEventArgs e)

		/// <summary>This method will handle QueryContinueDrag events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnUltraQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			try
			{
				//	Raise the control's QueryContinueDrag event
				OnQueryContinueDrag(e);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnUltraQueryContinueDrag", Ex);
			}

		}// private void OnUltraQueryContinueDrag(object sender, QueryContinueDragEventArgs e)

		/// <summary>This method handles MouseDown events fired by the grid control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnUltraMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UltraGridRow row = null;

			//	Is this the right mouse button?
			if(e.Button == MouseButtons.Right)
			{
				if((row = GetUltraRow(e.X, e.Y)) != null)
				{
					if(row.Selected == false)
					{
						SetSelection(row);
					}

				}// if((row = GetUltraRow(e.X, e.Y)) != null)

			}// if(e.Button == MouseButtons.Right)	

			//	Bubble the event
			try { OnMouseDown(e); }
			catch(System.Exception Ex) { m_tmaxEventSource.FireDiagnostic(this, "OnMouseDown", Ex); }

		}// private void OnUltraMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method will handle QueryContinueDrag events fired by the grid control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event argument object</param>
		private void OnPrintPage(object sender, PrintPageEventArgs e)
		{
			int		iCharsPerPage = 0;
			int		iLinesPerPage = 0;
		
			try
			{
				//	Do we have text to be printed?
				if(m_strPrinterText.Length > 0)
				{
					//	Set the bounds to be used for printing the text
					//
					//	NOTE:	The default bounds is set up for 1" boarders all around
					Rectangle rcMargins = e.MarginBounds;
					rcMargins.X = e.MarginBounds.X / 2; //	reduce to 1/2"

					//	How many characters can we print on this page?
					e.Graphics.MeasureString(m_strPrinterText, 
											 m_printerFont,
											 rcMargins.Size, 
											 StringFormat.GenericTypographic,
											 out iCharsPerPage, 
											 out iLinesPerPage);

					//	Draw the text
					e.Graphics.DrawString(m_strPrinterText, 
										  m_printerFont, 
										  Brushes.Black,
										  rcMargins, 
										  StringFormat.GenericTypographic);

					// Remove the portion of the string that has been printed.
					m_strPrinterText = m_strPrinterText.Substring(iCharsPerPage);

				}// if(m_strPrinterText.Length > 0)
			
			}
			catch(System.Exception Ex)
			{
				Warn("An error occurred while attempting to print the selected text:\n\n" + Ex.Message);
				m_strPrinterText = ""; // Stop further attempts to print
			}
			finally
			{
				//	Do we still have text to be printed?
				e.HasMorePages = (m_strPrinterText.Length > 0);
			}

		}// private void OnPrintPage(object sender, PrintPageEventArgs e)

		/// <summary>This method will perform the processing required when one of the objection color changes</summary>
		private void OnObjectionColorChanged()
		{
			CTmaxTransGridRow	tgRow = null;
			int					iIndex = -1;

			try
			{
				//	Have we got a valid data source?
				if((m_aDataSource != null) && (m_aDataSource.GetUpperBound(0) >= 0))
				{
					for(int i = 0; i <= m_aDataSource.GetUpperBound(0); i++)
					{
						if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)
						{
							//	Does this row have any objections
							if(tgRow.GetObjectionsCount() > 0)
							{
								iIndex = tgRow.GetGridIndex();

								if((iIndex >= 0) && (iIndex < m_ctrlUltraGrid.Rows.Count))
								{
									SetObjectionMarkers(tgRow, m_ctrlUltraGrid.Rows[iIndex]);
								}

							}// if(tgRow.GetObjectionsCount() > 0)

						}// if((tgRow = (CTmaxTransGridRow)m_aDataSource.GetValue(i)) != null)

					}// for(int i = 0; i <= ds.GetUpperBound(0); i++)

				}// if((m_aDataSource != null) && (m_aDataSource.GetUpperBound(0) >= 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnObjectionColorChanged", Ex);
			}

		}// private void OnObjectionColorChanged()

		#endregion Private Methods

		#region Properties

		/// <summary>The child grid control</summary>
		public Infragistics.Win.UltraWinGrid.UltraGrid UltraGrid
		{
			get { return m_ctrlUltraGrid; }
		}
		
		/// <summary>The active deposition</summary>
		public FTI.Shared.Xml.CXmlDeposition XmlDeposition
		{
			get { return m_xmlDeposition; }
			set { SetDeposition(value, null); }
		}

		/// <summary>The active collection of objections</summary>
		public FTI.Shared.Trialmax.CTmaxObjections Objections
		{
			get { return m_tmaxObjections; }
		}

		/// <summary>The time in ms required to display the Pause indicator</summary>
		public double PauseThreshold
		{
			get { return m_dPauseThreshold; }
			set { SetPauseThreshold(value); }
		}

		/// <summary>The color used to highlight defendant objections</summary>
		public System.Drawing.Color DefendantColor
		{
			get { return m_defendantColor; }
			set 
			{ 
				if(m_defendantColor != value)
				{
					m_defendantColor = value;
					OnObjectionColorChanged();
				}
			}

		}// public System.Drawing.Color DefendantColor

		/// <summary>The color used to highlight plaintiff objections</summary>
		public System.Drawing.Color PlaintiffColor
		{
			get { return m_plaintiffColor; }
			set
			{
				if(m_plaintiffColor != value)
				{
					m_plaintiffColor = value;
					OnObjectionColorChanged();
				}
			}

		}

		#endregion Properties

	}// public class CTmaxTransGridCtrl : CTmaxBaseCtrl
	
}// namespace FTI.Trialmax.Controls

