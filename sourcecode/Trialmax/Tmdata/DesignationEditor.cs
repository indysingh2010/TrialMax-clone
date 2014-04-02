using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class manages the export operations for the database</summary>
	public class CTmaxDesignationEditor
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_INITIALIZE_EX					= 0;
		protected const int ERROR_SET_SOURCE_EX					= 1;
		protected const int ERROR_SET_SCENE_EX					= 2;
		protected const int ERROR_SET_DEPOSITION_EX				= 3;
		protected const int ERROR_EDIT_EX						= 4;
		protected const int ERROR_NO_DEPOSITION_RECORD			= 5;
		protected const int ERROR_NO_TRANSCRIPT_RECORD			= 6;
		protected const int ERROR_XML_DEPOSTION_INVALID_PATH	= 7;
		protected const int ERROR_XML_DEPOSTION_NOT_FOUND		= 8;
		protected const int ERROR_XML_DEPOSTION_OPEN_FAILED		= 9;
		protected const int ERROR_CHECK_RANGE_EX				= 10;
		protected const int ERROR_INVALID_EDIT_RANGE			= 11;
		protected const int ERROR_EDIT_EXTENTS_EX				= 12;
		protected const int ERROR_NO_HIGHLIGHTER_RECORD			= 13;
		protected const int ERROR_INVALID_HIGHLIGHTER_ID		= 14;
		protected const int ERROR_SET_HIGHLIGHTER_EX			= 15;
		protected const int ERROR_CREATE_DESIGNATIONS_EX		= 16;
		protected const int ERROR_XML_DESIGNATION_INVALID_PATH	= 17;
		protected const int ERROR_XML_DESIGNATION_OPEN_FAILED	= 18;
		protected const int ERROR_UPDATE_EX						= 19;
		protected const int ERROR_ADD_DESIGNATION_EX			= 20;
		protected const int ERROR_ADD_DESIGNATIONS_EX			= 21;
		protected const int ERROR_ADD_SCENES_EX					= 22;
		protected const int ERROR_XML_SEGMENT_NOT_FOUND			= 23;
		protected const int ERROR_SET_XML_EXTENTS_EX			= 24;
		protected const int ERROR_DELETE_EX						= 25;
		protected const int ERROR_EXCLUDE_EX					= 26;
		protected const int ERROR_SAVE_DESIGNATION_FAILED		= 27;
		protected const int ERROR_SPLIT_EX						= 28;
		protected const int ERROR_MOVE_LINK_EX					= 29;
		protected const int ERROR_UPDATE_LINKS_EX				= 30;
	
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to Database property</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member bound to Source property</summary>
		private CTmaxItems m_tmaxSource = null;
		
		/// <summary>Caller supplies results container</summary>
		private CTmaxDatabaseResults m_tmaxResults = null;
		
		/// <summary>Local member to keep track of the active scene</summary>
		private CDxSecondary m_dxScene = null;
		
		/// <summary>Local member to keep track of the active designation</summary>
		private CDxTertiary m_dxDesignation = null;
		
		/// <summary>Local member to keep track of the active deposition</summary>
		private CDxPrimary m_dxDeposition = null;
		
		/// <summary>Local member to keep track of the active highlighter</summary>
		private CDxHighlighter m_dxHighlighter = null;
		
		/// <summary>Local member to keep track of the active transcript</summary>
		private CDxTranscript m_dxTranscript = null;
		
		/// <summary>Local member to keep track of the links owned by the active designation</summary>
		private CDxQuaternaries m_dxLinks = new CDxQuaternaries();
		
		/// <summary>Local member to keep track of the active XML deposition</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = null;
		
		/// <summary>Local member to keep track of the active XML designation</summary>
		private FTI.Shared.Xml.CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member to keep track of the designation method</summary>
		private TmaxDesignationEditMethods m_eMethod = TmaxDesignationEditMethods.Unknown;
		
		/// <summary>Local member to keep track of the operation's start position</summary>
		private long m_lStartPL = -1;
		
		/// <summary>Local member to keep track of the operation's stop position</summary>
		private long m_lStopPL = -1;
		
		/// <summary>Local member to keep track of the highlighter id to use for the operation</summary>
		private long m_lHighlighterId = -1;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxDesignationEditor()
		{
			//	Initialize the event source and error builder
			SetErrorStrings();
			m_tmaxEventSource.Name = "Designation Editor";
			
		}// public CTmaxDesignationEditor()
		
		/// <summary>This method uses the specified parameters to set the associated properties</summary>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <param name="tmaxSource">Collection of event items that identify the source records</param>
		/// <param name="tmaxResults">The object to store the results</param>
		/// <returns>true if successful</returns>
		public bool Initialize(CTmaxItems tmaxSource, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
		{
			CTmaxParameter	tmaxParameter = null;

			//	Reset the current values to their defaults
			Reset(true);
			
			try
			{
				//	Save the results object supplied by the caller
				m_tmaxResults = tmaxResults;
				
				//	Get the edit method
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.EditMethod)) != null)
				{
					try	  { m_eMethod = (TmaxDesignationEditMethods)(tmaxParameter.AsInteger()); }
					catch {}
				}

				//	Get the edit range
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.StartPL)) != null)
					m_lStartPL = tmaxParameter.AsLong();
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.StopPL)) != null)
					m_lStopPL = tmaxParameter.AsLong();
				
				//	Get the highlighter id
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Highlighter)) != null)
					m_lHighlighterId = tmaxParameter.AsLong();
					
				//	Set the source records
				return SetSource(tmaxSource);		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
				return false;
			}
			
		}// public bool Initialize(CTmaxParameters tmaxParameters)
		
		/// <summary>This method is called to execute the edit operation</summary>
		/// <returns>true if successful</returns>
		public bool Edit()
		{
			bool bSuccessful = false;
			
			try
			{
				
				//	Iterate the source collection
				foreach(CTmaxItem O in m_tmaxSource)
				{
					//	Set the scene and designation
					if(SetScene(O) == true)
					{
						//	Make sure the range and highlighters are valid
						if((CheckRange() == true) && (SetHighlighter() == true))
						{
							//	What method?
							switch(m_eMethod)
							{
								case TmaxDesignationEditMethods.Extents:
								
									EditExtents();
									break;
									
								case TmaxDesignationEditMethods.Exclude:
								
									Exclude();
									break;
									
								case TmaxDesignationEditMethods.SplitBefore:
								case TmaxDesignationEditMethods.SplitAfter:
								
									Split(m_eMethod == TmaxDesignationEditMethods.SplitBefore);
									break;
									
								default:
								
									break;
									
							}// switch(m_eMethod)
							
						}// if(CheckRange() == true)

					}// if(SetScene(O) == true)
					
				}// foreach(CTmaxItem O in m_tmaxSource)
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Edit", m_tmaxErrorBuilder.Message(ERROR_EDIT_EX), Ex);
				return false;
			}
			finally
			{
				Reset(true);
			}
			
			return bSuccessful;
			
		}// public bool Edit()
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method sets the active scene</summary>
		/// <param name="tmaxItem">The event item that identifies the scene</param>
		/// <returns>true if successful</returns>
		private bool SetScene(CTmaxItem tmaxItem)
		{
			string	strFileSpec = "";
			bool	bSuccessful = false;
			
			//	Reset the scene specific members
			Reset(false);
				
			try
			{
				Debug.Assert(tmaxItem != null);
				
				//	The caller's item should be bound to a scene
				if(tmaxItem.GetMediaRecord() == null) return false;
				if(tmaxItem.GetMediaRecord().GetMediaType() != TmaxMediaTypes.Scene) return false;
				
				m_dxScene = (CDxSecondary)(tmaxItem.GetMediaRecord());
					
				//	The scene should be bound to a designation
				if(m_dxScene.GetSource() == null) return false;
				if(m_dxScene.GetSource().MediaType != TmaxMediaTypes.Designation) return false;

				m_dxDesignation = (CDxTertiary)(m_dxScene.GetSource());
				
				//	Get the path to the XML designation
				strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxDesignation);
				if((strFileSpec == null) || (strFileSpec.Length == 0))
				{
					m_tmaxEventSource.FireError(this, "SetScene", m_tmaxErrorBuilder.Message(ERROR_XML_DESIGNATION_INVALID_PATH, m_dxDesignation.GetBarcode(false)));
					return false;
				}
				
				//	Make sure the XML file exists
				if(System.IO.File.Exists(strFileSpec) == false)
				{
					//	Attempt to create the file
					if(m_tmaxDatabase.CreateXmlDesignation(m_dxDesignation, strFileSpec) == false)
						return false;
				}
				
				//	Open the file
				m_xmlDesignation = new CXmlDesignation();
				if(m_xmlDesignation.FastFill(strFileSpec, false, true) == false)
				{
					m_tmaxEventSource.FireError(this, "SetScene", m_tmaxErrorBuilder.Message(ERROR_XML_DESIGNATION_OPEN_FAILED, strFileSpec));
					return false;
				}
				
				//	Store the links off to the side while we edit the designation
				m_dxLinks.Clear(); // Should be necessary but just in case...
				if(m_dxDesignation.Quaternaries.Count == 0)
					m_dxDesignation.Fill();
				foreach(CDxQuaternary O in m_dxDesignation.Quaternaries)
					m_dxLinks.AddList(O);
					
				//	Set the deposition information
				bSuccessful = SetDeposition(m_dxDesignation);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScene", m_tmaxErrorBuilder.Message(ERROR_SET_SCENE_EX), Ex);
			}
			
			return bSuccessful;
				
		}// private bool SetSource(CTmaxItems tmaxSource)
		
		/// <summary>This method sets the active deposition</summary>
		/// <param name="dxDesignation">The exchange interface for the active designation</param>
		/// <returns>true if successful</returns>
		private bool SetDeposition(CDxTertiary dxDesignation)
		{
			CDxPrimary	dxDeposition = null;
			string		strFileSpec  = "";
			bool		bSuccessful  = false;
			
			try
			{
				Debug.Assert(dxDesignation != null);
				
				//	Get the source deposition record
				if(dxDesignation.Secondary != null)
					dxDeposition = dxDesignation.Secondary.Primary;
					
				if(dxDeposition == null)
				{
					m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_NO_DEPOSITION_RECORD, dxDesignation.GetBarcode(false)));
					return false;
				}
				
				//	Has the source deposition changed?
				if(ReferenceEquals(dxDeposition, m_dxDeposition) == false)
				{
					//	Update the class member
					m_dxDeposition = dxDeposition;
					
					//	These members are no longer valid
					m_dxTranscript = null;
					if(m_xmlDeposition != null)
					{
						m_xmlDeposition.Clear();
						m_xmlDeposition = null;
					}
					
				}// if(ReferenceEquals(dxDeposition, m_dxDeposition) == false)
				
				//	Do we need to get the transcript record?
				if(m_dxTranscript == null)
				{
					if((m_dxTranscript = m_dxDeposition.GetTranscript()) == null)
					{
						m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_NO_TRANSCRIPT_RECORD, dxDesignation.GetBarcode(false)));
						return false;
					}
					
				}
				
				//	Do we need to get the XML deposition?
				if(m_xmlDeposition == null)
				{
					//	Get the path to the XML file
					strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxTranscript);
					
					if((strFileSpec == null) || (strFileSpec.Length == 0))
					{
						m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_XML_DEPOSTION_INVALID_PATH, dxDesignation.GetBarcode(false)));
						return false;
					}
					else if(System.IO.File.Exists(strFileSpec) == false)
					{
						m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_XML_DEPOSTION_NOT_FOUND, strFileSpec));
						return false;
					}
					else
					{
						m_xmlDeposition = new CXmlDeposition();
						if(m_xmlDeposition.FastFill(strFileSpec, true, true, false) == false)
						{
							m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_XML_DEPOSTION_OPEN_FAILED, strFileSpec));
							return false;
						}
						
					}
					
				}// if(m_xmlDeposition == null)
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDeposition", m_tmaxErrorBuilder.Message(ERROR_SET_DEPOSITION_EX), Ex);
			}
			
			return bSuccessful;
				
		}// private bool SetDeposition(CDxPrimary dxDeposition)
		
		/// <summary>This method sets the source collection for the operation</summary>
		/// <param name="tmaxSource">Collection of event items that identify the source records</param>
		/// <returns>true if successful</returns>
		private bool SetSource(CTmaxItems tmaxSource)
		{
			bool bSuccessful = false;
			
			try
			{
				m_tmaxSource = tmaxSource;
				
				//	Did the caller provide a valid source collection?
				if((m_tmaxSource != null) && (m_tmaxSource.Count > 0))
				{
					bSuccessful = true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSource", m_tmaxErrorBuilder.Message(ERROR_SET_SOURCE_EX), Ex);
			}
			
			return bSuccessful;
				
		}// private bool SetSource(CTmaxItems tmaxSource)
		
		/// <summary>This method sets the active highlighter</summary>
		/// <returns>true if successful</returns>
		private bool SetHighlighter()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Did the caller specify a highlighter?
				if(m_lHighlighterId > 0)
				{
					//	Get the requested highlighter
					if((m_dxHighlighter = m_tmaxDatabase.Highlighters.Find(m_lHighlighterId, false)) == null)
					{
						m_tmaxEventSource.FireError(this, "SetHighlighter", m_tmaxErrorBuilder.Message(ERROR_INVALID_HIGHLIGHTER_ID, m_lHighlighterId));
						return false;
					}
				
				}
				else if(m_dxDesignation != null)
				{
					//	Use the highlighter assigned to the active designation
					if(m_dxDesignation.GetExtent() != null)
						m_dxHighlighter = m_tmaxDatabase.Highlighters.Find(m_dxDesignation.GetExtent().HighlighterId);
				}
				
				//	Do we have a highlighter?
				if(m_dxHighlighter != null)
				{
					bSuccessful = true;
				}
				else
				{
					m_tmaxEventSource.FireError(this, "SetHighlighter", m_tmaxErrorBuilder.Message(ERROR_NO_HIGHLIGHTER_RECORD));
				}
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetHighlighter", m_tmaxErrorBuilder.Message(ERROR_SET_HIGHLIGHTER_EX, m_lHighlighterId), Ex);
			}
			
			return bSuccessful;
				
		}// private bool SetHighlighter()
		
		/// <summary>This method will reset the local members to their default values</summary>
		/// <param name="bComplete">True to do a complete reset of all members</param>
		private void Reset(bool bComplete)
		{
			//	These members get reset for each source item
			m_dxScene		= null;
			m_dxDesignation	= null;
			m_dxHighlighter = null;
			if(m_xmlDesignation != null)
			{
				m_xmlDesignation.Close(true);
				m_xmlDesignation = null;
			}
			
			//	Are we doing a complete reset?
			if(bComplete == true)
			{
				if(m_xmlDeposition != null)
				{
					m_xmlDeposition.Clear();
					m_xmlDeposition = null;
				}
				
				if(m_dxLinks != null)
					m_dxLinks.Clear();
					
				m_dxDeposition		= null;
				m_dxTranscript		= null;
				m_tmaxSource		= null;
				m_lStartPL			= -1;
				m_lStopPL			= -1;
				m_lHighlighterId	= -1;
				m_eMethod			= TmaxDesignationEditMethods.Unknown;
				m_tmaxResults		= null;
			
			}// if(bComplete == true)
				
		}// private void Reset()
		
		/// <summary>This method validates the start and stop positions for the operation</summary>
		/// <returns>true if the positions are valid</returns>
		private bool CheckRange()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_dxTranscript != null);
			
			try
			{
				//	Make sure the start position is valid
				if(m_lStartPL < m_dxTranscript.FirstPL)
					m_lStartPL = m_dxTranscript.FirstPL;
					
				//	Make sure the stop position is valid
				if(m_lStopPL > m_dxTranscript.LastPL)
					m_lStopPL = m_dxTranscript.LastPL;
					
				//	Make sure the range is not reversed
				if(m_lStopPL < m_lStartPL)
				{
					m_tmaxEventSource.FireError(this, "CheckRange", m_tmaxErrorBuilder.Message(ERROR_INVALID_EDIT_RANGE, m_lStartPL, m_lStopPL));
					return false;
				}
				
				bSuccessful = true;	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckRange", m_tmaxErrorBuilder.Message(ERROR_CHECK_RANGE_EX), Ex);
			}
			
			return bSuccessful;
				
		}// private bool CheckRange()
		
		/// <summary>This method is called to determine if any links are within the specified range</summary>
		/// <param name="lFirstPL">The page/line of the first position</param>
		/// <param name="lLastPL">The page/line of the last position</param>
		/// <param name="bInside">true if test to see if inside the specified range</param>
		/// <returns>the number of links that fall in/out of the specified range</returns>
		private int CheckLinks(long lFirstPL, long lLastPL, bool bInside)
		{
			int iLinks = 0;
			
			if(m_dxLinks == null) return 0;
			if(m_dxLinks.Count == 0) return 0;
			
			//	Test each of the links
			foreach(CDxQuaternary O in m_dxLinks)
			{
				//	Are we looking for links inside the specified range?
				if(bInside == true)
				{
					if(O.StartPL >= lFirstPL && O.StartPL <= lLastPL)
						iLinks++;
				}
				else
				{
					if(O.StartPL < lFirstPL || O.StartPL > lLastPL)
						iLinks++;
				}
			
			}// foreach(CDxQuaternary O in m_dxLinks)
			
			return iLinks;
				
		}// private int CheckLinks(long lFirstPL, long lLastPL, bool bInside)
		
		/// <summary>This method uses the current setup to edit the extents of the active designation</summary>
		/// <returns>true if successful</returns>
		private bool EditExtents()
		{
			CXmlDesignations	xmlDesignations = null;
			CXmlDesignation		xmld = null;
			bool				bSuccessful = false;
			long				lBarcode = 0;
			CDxSecondary		dxFirstScene = null;
			CTmaxItems			tmaxDesignations = null;
			int					iLinks = 0;
			string				strMsg = "";

			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_dxDesignation != null);
			Debug.Assert(m_dxDeposition != null);
			Debug.Assert(m_dxTranscript != null);
			Debug.Assert(m_xmlDeposition != null);
			
			try
			{
				//	Do we need to modify the extents?
				if((m_dxDesignation.GetExtent().StartPL != m_lStartPL) || (m_dxDesignation.GetExtent().StopPL != m_lStopPL))
				{
					//	Check for out of range links unless we are creating a whole new designation
					if((m_dxLinks != null) && (m_dxLinks.Count > 0))
					{
						if((iLinks = CheckLinks(m_lStartPL, m_lStopPL, false)) > 0)
						{
							strMsg = String.Format("{0} links will be outside the playback range after changing the extents. Do you want to continue?", iLinks);
							if(MessageBox.Show(strMsg, "Links", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
								return false;
						}
						
					}// if((m_dxLinks != null) && (m_dxLinks.Count > 0))
					
					//	Create a temporary collection to store designations
					if((xmlDesignations = CreateDesignations(true)) == null)
						return false;
						
					//	Does the segment for the first designation match the segment of the active designation?
					if(xmlDesignations[0].Segment == m_xmlDesignation.Segment)
					{
						//	Update the active segment to match the new extents
						m_xmlDesignation.EditExtents(xmlDesignations[0]);
						m_xmlDesignation.SetNameFromExtents(m_xmlDeposition);
						
						//	Add the new scenes
						if(xmlDesignations.Count > 1)
						{
							tmaxDesignations = new CTmaxItems();
							
							xmlDesignations.RemoveAt(0);
							AddScenes(xmlDesignations, false, tmaxDesignations);
						}
						
						//	Update the active scene
						Update();
						
					}
					else
					{
						//	Find the new designation that references the same segment as the active designation
						if((xmld = xmlDesignations.FindFromSegment(m_xmlDesignation.Segment)) != null)
						{
							//	Copy tuning and edited text from the active designation
							SetXmlExtents(xmld);					
						}
						
						//	Add the entire collection to the script
						tmaxDesignations = new CTmaxItems();
						dxFirstScene = AddScenes(xmlDesignations, false, tmaxDesignations);
						
						//	Get the barcode of the active scene
						lBarcode = m_dxScene.BarcodeId;
						
						//	Delete the active scene
						Delete();
						
						//	Change the barcode and select the first scene
						if(dxFirstScene != null)
						{
							if(m_tmaxResults.Selection != null)
								m_tmaxResults.Selection.SetRecord(dxFirstScene);
								
							dxFirstScene.BarcodeId = lBarcode;
							m_tmaxDatabase.Update(new CTmaxItem(dxFirstScene), null, null);
						}
					
					}// if(xmlDesignations[0].Segment == m_xmlDesignation.Segment)
				
				}// if((m_dxDesignation.GetExtent().StartPL != m_lStartPL) || (m_dxDesignation.GetExtent().Stop != m_lStopPL))
				
				//	Has the highlighter changed?
				else if((m_dxHighlighter != null) && (m_dxDesignation.GetExtent().HighlighterId != m_dxHighlighter.AutoId))
				{
					//	Update the active designation
					if(Update() == false)
						return false;
				}	
								
				bSuccessful = true;	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "EditExtents", m_tmaxErrorBuilder.Message(ERROR_EDIT_EXTENTS_EX), Ex);
			}
			
			return bSuccessful;
				
		}// private bool EditExtents()
		
		/// <summary>This method uses the current setup to edit the extents of the active designation</summary>
		/// <returns>true if successful</returns>
		private bool Exclude()
		{
			CXmlDesignations	xmlDesignations = null;
			CXmlDesignation		xmlSplit = null;
			bool				bSuccessful = false;
			CTmaxItems			tmaxDesignations = null;
			CTmaxItem			tmaxOriginal = null;
			int					iLinks = 0;
			string				strMsg = "";

			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_dxDesignation != null);
			Debug.Assert(m_dxDeposition != null);
			Debug.Assert(m_dxTranscript != null);
			Debug.Assert(m_xmlDeposition != null);
			
			try
			{
				//	Are we going to be putting any links out of range?
				if((iLinks = CheckLinks(m_lStartPL, m_lStopPL, true)) > 0)
				{
					strMsg = String.Format("{0} links will be outside the playback range after performing the exclusion. Do you want to continue?", iLinks);
					if(MessageBox.Show(strMsg, "Links", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						return false;
				}
				
				//	Are we chopping off the start of the designation?
				if(m_lStartPL <= m_xmlDesignation.FirstPL)
				{
					//	Make sure the stop position is valid
					if(m_lStopPL < m_xmlDesignation.LastPL)
					{
						//	Split off the leading portion
						m_xmlDesignation = m_xmlDesignation.Split(m_lStopPL, true, m_xmlDeposition);
					}
					else
					{
						//	Can't exclude the entire designation
						return false;
					}
					
				}
				else
				{
					//	Split off the trailing portion
					xmlSplit = m_xmlDesignation.Split(m_lStartPL, false, m_xmlDeposition);

					//	Are we cutting a piece out of the middle?
					if(m_lStopPL < xmlSplit.LastPL)
					{
						//	Split off the leading portion
						xmlSplit = xmlSplit.Split(m_lStopPL, true, m_xmlDeposition);

						//	We need to add this designation
						xmlDesignations = new CXmlDesignations();
						xmlDesignations.Add(xmlSplit);
						
						tmaxDesignations = new CTmaxItems();
						AddScenes(xmlDesignations, false, tmaxDesignations);
						
					}
					
				}
						
				//	Update the active record
				Update();
						
				//	Do we need to update links?
				if((tmaxDesignations != null) && (m_dxLinks != null) && (m_dxLinks.Count > 0))
				{
					//	Insert the original designation
					tmaxOriginal = new CTmaxItem(m_dxDesignation);
					tmaxOriginal.XmlDesignation = m_xmlDesignation;
					tmaxDesignations.Insert(tmaxOriginal, 0);

					//	Update the links
					UpdateLinks(tmaxDesignations);
						
				}// if((m_dxLinks != null) && (m_dxLinks.Count > 0))
												
				bSuccessful = true;	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exclude", m_tmaxErrorBuilder.Message(ERROR_EXCLUDE_EX), Ex);
			}
			
			return bSuccessful;
				
		}// private bool Exclude()
		
		/// <summary>This method uses the current setup to split the active designation into two parts</summary>
		/// <param name="bBeforeSelection">True to split before the selection</param>
		/// <returns>true if successful</returns>
		private bool Split(bool bBeforeSelection)
		{
			CXmlDesignations	xmlDesignations = null;
			CXmlDesignation		xmlSplit = null;
			CTmaxItems			tmaxDesignations = null;
			CTmaxItem			tmaxOriginal = null;
			bool				bSuccessful = false;

			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_dxDesignation != null);
			Debug.Assert(m_dxDeposition != null);
			Debug.Assert(m_dxTranscript != null);
			Debug.Assert(m_xmlDeposition != null);
			
			try
			{
				//	Are we splitting before the specified selection range?
				if(bBeforeSelection == true)
				{
					//	Make sure the start position is valid
					if(m_lStartPL > m_xmlDesignation.FirstPL)
					{
						//	Split off the portion from the start line to the end
						xmlSplit = m_xmlDesignation.Split(m_lStartPL, false, m_xmlDeposition);
					}
					else
					{
						//	Nothing to split
						return false;
					}
					
				}
				else
				{
					//	Is the stop position valid?
					if(m_lStopPL < m_xmlDesignation.LastPL)
					{
						//	Split off the leading portion
						xmlSplit = m_xmlDesignation.Split(m_lStopPL, true, m_xmlDeposition);
					}
					else
					{
						//	Nothing to split
						return false;
					}
					
				}
						
				//	We need to add this designation
				if(xmlSplit != null)
				{
					xmlDesignations = new CXmlDesignations();
					xmlDesignations.Add(xmlSplit);
							
					tmaxDesignations = new CTmaxItems();
					AddScenes(xmlDesignations, false, tmaxDesignations);

				}// if(xmlSplit != null)
				
				//	Update the active record
				Update();
						
				//	Do we need to update links?
				if((tmaxDesignations != null) && (m_dxLinks != null) && (m_dxLinks.Count > 0))
				{
					//	Insert the original designation
					tmaxOriginal = new CTmaxItem(m_dxDesignation);
					tmaxOriginal.XmlDesignation = m_xmlDesignation;
					tmaxDesignations.Insert(tmaxOriginal, 0);

					//	Update the links
					UpdateLinks(tmaxDesignations);
						
				}// if((m_dxLinks != null) && (m_dxLinks.Count > 0))
												
				bSuccessful = true;	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Split", m_tmaxErrorBuilder.Message(ERROR_SPLIT_EX, bBeforeSelection), Ex);
			}
			
			return bSuccessful;
				
		}// private bool Split(bool bBeforeSelection)
		
		/// <summary>This method sets the extents and text in the specified designation using the active designation</summary>
		/// <param name="xmlTarget">The XML designation to be set</param>
		/// <returns>true if successful</returns>
		private bool SetXmlExtents(CXmlDesignation xmlTarget)
		{
			CXmlTranscripts		xmlEdited = null;
			int					iIndex = -1;
			bool				bSuccessful = false;

			Debug.Assert(xmlTarget != null);
			Debug.Assert(m_xmlDesignation != null);
			
			try
			{
				//	We don't want to loose any tuning information
				if(m_xmlDesignation.FirstPL == xmlTarget.FirstPL)
				{
					if(m_xmlDesignation.StartTuned == true)
					{
						xmlTarget.StartTuned = true;
						xmlTarget.Start = m_xmlDesignation.Start;
					}
				}
			
				if(m_xmlDesignation.LastPL == xmlTarget.LastPL)
				{
					if(m_xmlDesignation.StopTuned == true)
					{
						xmlTarget.StopTuned = true;
						xmlTarget.Stop = m_xmlDesignation.Stop;
					}
				}
			
				//	Do we need to check for edited text?
				if((xmlTarget.LastPL >= m_xmlDesignation.FirstPL) && (xmlTarget.FirstPL <= m_xmlDesignation.LastPL))
				{
					//	Allocate a container to hold the edited text
					xmlEdited = new CXmlTranscripts();
					
					//	Locate all edited text in the source designation
					foreach(CXmlTranscript O in m_xmlDesignation.Transcripts)
					{
						if(O.Edited == true)
							xmlEdited.Add(O);
					}
					
					//	Do we have any edited text?
					if(xmlEdited.Count > 0)
					{
						foreach(CXmlTranscript O in xmlEdited)
						{
							if((iIndex = xmlTarget.Transcripts.Locate(iIndex, O.PL)) >= 0)
							{
								xmlTarget.Transcripts[iIndex].Text = O.Text;
								xmlTarget.Transcripts[iIndex].Edited = true;
							}
						
						}// foreach(CXmlTranscript O in xmlEdited)
						
						xmlEdited.Clear();
					
					}// if(xmlEdited.Count > 0)
					
					xmlEdited = null;
					
				}// if((xmlTarget.LastPL >= m_xmlDesignation.FirstPL) && (xmlTarget.FirstPL <= m_xmlDesignation.LastPL))				
								
				bSuccessful = true;	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetXmlExtents", m_tmaxErrorBuilder.Message(ERROR_SET_XML_EXTENTS_EX, xmlTarget.Name), Ex);
			}
			
			return bSuccessful;
				
		}// private bool SetXmlExtents(CXmlDesignation xmlTarget)
		
		/// <summary>This method creates a collection of designations that cover the current range</summary>
		/// <param name="bConfirmMultiple">true to confirm if more than one designation is needed</param>
		/// <returns>true if successful</returns>
		private CXmlDesignations CreateDesignations(bool bConfirmMultiple)
		{
			CXmlDesignations	xmlDesignations = null;
			string				strMsg = "";
			bool				bSuccessful = false;

			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_dxDesignation != null);
			Debug.Assert(m_dxDeposition != null);
			Debug.Assert(m_dxTranscript != null);
			Debug.Assert(m_xmlDeposition != null);
			
			try
			{
				while(bSuccessful == false)
				{
					//	Create a temporary collection to store designations
					xmlDesignations = new CXmlDesignations();
					
					//	Use the parent deposition to create desingations for the new range
					if(m_xmlDeposition.CreateDesignations(xmlDesignations, m_lStartPL, m_lStopPL, (int)(m_dxHighlighter.AutoId)) == false)
						break;
					
					//	This shouldn't happen but just in case ...
					if(xmlDesignations.Count == 0)
						break;
						
					//	should we prompt for confirmation?
					if((xmlDesignations.Count > 1) && (bConfirmMultiple == true))
					{
						strMsg = String.Format("The current selection will result in {0} designations\n\n", xmlDesignations.Count);
							
						foreach(CXmlDesignation O in xmlDesignations)
							strMsg += (O.Name + "\n");
								
						strMsg += "\nDo you want to continue?";
							
						//	Prompt the user for confirmation before continuing
						if(MessageBox.Show(strMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							break;
				
					}// if((xmlDesignations.Count > 1) && (bConfirm == true))
				
					//	It's all good
					bSuccessful = true;
					
				}// while(bSuccessful == false)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDesignations", m_tmaxErrorBuilder.Message(ERROR_CREATE_DESIGNATIONS_EX, m_lStartPL, m_lStopPL), Ex);
			}
			
			//	Did the attempt fail?
			if(bSuccessful == false)
			{
				if(xmlDesignations != null)
				{
					xmlDesignations.Clear();
					xmlDesignations = null;
				}
			}
			
			return xmlDesignations;
				
		}// private CXmlDesignations CreateDesignations(bool bConfirmMultiple)
		
		/// <summary>This method is called to update the active designation</summary>
		/// <returns>true if successful</returns>
		private bool Update()
		{
			CTmaxItem	tmaxItem = null;
			bool		bSuccessful = false;
			string		strFileSpec = "";
			
			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_xmlDesignation != null);
			Debug.Assert(m_dxDesignation != null);
			Debug.Assert(m_dxDesignation.GetExtent() != null);
			if(m_dxScene == null) return false;
			if(m_xmlDesignation == null) return false;
			if(m_dxDesignation == null) return false;
			if(m_dxDesignation.GetExtent() == null) return false;
			
			try
			{
				//	Make sure the highlighter assignment is correct
				if(m_dxHighlighter != null)
				{
					m_xmlDesignation.Highlighter = (int)(m_dxHighlighter.AutoId);
					m_dxDesignation.GetExtent().HighlighterId = m_dxHighlighter.AutoId;
				}
				
				//	Make sure we use the correct file path 
				strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxDesignation);
				
				//	Save the designation first
				if(m_xmlDesignation.Save(strFileSpec) == false)
				{
					m_tmaxEventSource.FireError(this, "Update", m_tmaxErrorBuilder.Message(ERROR_SAVE_DESIGNATION_FAILED, strFileSpec));
					return false;
				}
				
				//	Refresh the designation record
				m_dxDesignation.GetExtent().StartPL = m_xmlDesignation.FirstPL;
				m_dxDesignation.GetExtent().Start = m_xmlDesignation.Start;
				m_dxDesignation.GetExtent().StartTuned = m_xmlDesignation.StartTuned;
				m_dxDesignation.GetExtent().StopPL = m_xmlDesignation.LastPL;
				m_dxDesignation.GetExtent().Stop = m_xmlDesignation.Stop;
				m_dxDesignation.GetExtent().StopTuned = m_xmlDesignation.StopTuned;
				m_dxDesignation.GetExtent().MaxLineTime = m_xmlDesignation.GetMaxLineTime();
				m_dxDesignation.Name = m_xmlDesignation.Name;
				
				//	Update the database
				tmaxItem = new CTmaxItem(m_dxDesignation);
				m_tmaxDatabase.Update(tmaxItem, null, null);
				if(m_tmaxResults != null)
					m_tmaxResults.Updated.Add(tmaxItem);
				
				tmaxItem = new CTmaxItem(m_dxScene);
				m_tmaxDatabase.Update(tmaxItem, null, null);
				if(m_tmaxResults != null)
					m_tmaxResults.Updated.Add(tmaxItem);
				
				//	Set the Edited information in the results
				if((m_tmaxResults != null) && (m_tmaxResults.Edited != null))
				{
					m_tmaxResults.Edited.SetRecord(m_dxScene);

					foreach(CTmaxItem O in m_tmaxResults.Added)
					{
						//	Items are stored in Parent/Child heirarchy
						foreach(CTmaxItem subItem in O.SubItems)
							m_tmaxResults.Edited.SubItems.Add(subItem);
					}
				}

				bSuccessful = true;
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Update", m_tmaxErrorBuilder.Message(ERROR_UPDATE_EX, m_dxScene.GetBarcode(false), m_dxDesignation.GetBarcode(false)), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Update()

		/// <summary>This method is called to delete the active scene</summary>
		/// <returns>true if successful</returns>
		private bool Delete()
		{
			CTmaxItem	tmaxItem = null;
			bool		bSuccessful = false;
			
			Debug.Assert(m_dxScene != null);
			
			try
			{

				//	Update the database
				tmaxItem = new CTmaxItem(m_dxScene.Primary);
				tmaxItem.SubItems.Add(new CTmaxItem(m_dxScene));

				m_tmaxDatabase.Delete(tmaxItem, null, null);
				if(m_tmaxResults != null)
					m_tmaxResults.Deleted.Add(tmaxItem);
				
				bSuccessful = true;
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Delete", m_tmaxErrorBuilder.Message(ERROR_DELETE_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Delete()

		/// <summary>This method is called to add the designations to the database</summary>
		/// <param name="bBefore">true if records are to be added before the active scene</param>
		/// <param name="tmaxDesignations">Optional collection to store designation record items</param>
		/// <returns>The first new scene if successful</returns>
		private CDxSecondary AddScenes(CXmlDesignations xmlDesignations, bool bBefore, CTmaxItems tmaxDesignations)
		{
			CTmaxDatabaseResults	tmaxResults = null;
			CTmaxItem				tmaxItem = null;
			CTmaxParameters			tmaxParameters = null;
			CDxSecondary			dxScene = null;
			bool					bSuccessful = false;
			
			Debug.Assert(xmlDesignations != null);
			Debug.Assert(xmlDesignations.Count > 0);
			Debug.Assert(m_dxScene != null);
			Debug.Assert(m_xmlDesignation != null);
			Debug.Assert(m_dxDesignation != null);
			Debug.Assert(m_dxDesignation.GetExtent() != null);
			if(xmlDesignations == null) return null;
			if(xmlDesignations.Count == 0) return null;
			if(m_dxScene == null) return null;
			if(m_xmlDesignation == null) return null;
			if(m_dxDesignation == null) return null;
			if(m_dxDesignation.GetExtent() == null) return null;
			
			try
			{
				//	Make sure we have a collection to store the results
				if(tmaxDesignations == null)
					tmaxDesignations = new CTmaxItems();
					
				//	Add each of the designations to the database
				if(AddDesignations(xmlDesignations, tmaxDesignations) == null)
					return null;
					
				//	Create the item that represents the parent script
				tmaxItem = new CTmaxItem(m_dxScene.Primary);
				
				//	Use the new records as the source items for the new scenes
				tmaxItem.SourceItems = tmaxDesignations;
					
				//	Make the active scene the insertion point
				tmaxItem.SubItems.Add(new CTmaxItem(m_dxScene));
				
				//	Set up the parameters for the request
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
				
				//	Create an object to store the results
				tmaxResults = new CTmaxDatabaseResults();
				
				//	Add the scenes to the database
				bSuccessful = m_tmaxDatabase.Add(tmaxItem, tmaxParameters, tmaxResults);
							
				if((bSuccessful == true) && (m_tmaxResults != null) && (tmaxResults.Added.Count > 0))
				{
					if(tmaxResults.Added[0].SubItems.Count > 0)
						dxScene = (CDxSecondary)(tmaxResults.Added[0].SubItems[0].GetMediaRecord());
					
					foreach(CTmaxItem O in tmaxResults.Added)
						m_tmaxResults.Added.Add(O);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddScenes", m_tmaxErrorBuilder.Message(ERROR_ADD_SCENES_EX), Ex);
			}
			
			return dxScene;
			
		}// private CDxSecondary AddScenes(CXmlDesignations xmlDesignations, bool bBefore, CTmaxItems tmaxDesignations)

		/// <summary>This method is called to add the designations to the database</summary>
		/// <param name="xmlDesignations">The collection of XML designations used to create the new records</param>
		/// <param name="tmaxAdded">Optional collection to store items representing records added to the database</param>
		/// <returns>The collection of new designations if successful</returns>
		private CDxTertiaries AddDesignations(CXmlDesignations xmlDesignations, CTmaxItems tmaxAdded)
		{
			CDxTertiaries	dxDesignations = null;
			CDxTertiary		dxDesignation = null;
			CTmaxItem		tmaxItem = null;
			bool			bSuccessful = true;
			
			Debug.Assert(xmlDesignations != null);
			Debug.Assert(xmlDesignations.Count > 0);
			if(xmlDesignations == null) return null;
			if(xmlDesignations.Count == 0) return null;
			
			try
			{
				//	Allocate the collection to store the new records
				dxDesignations = new CDxTertiaries();
					
				//	Add a record for each XML designation
				foreach(CXmlDesignation O in xmlDesignations)
				{
					//	Add the designation to the database
					if((dxDesignation = AddDesignation(O)) != null)
					{
						//	Add to the holding collection
						dxDesignations.AddList(dxDesignation);
						
						if(tmaxAdded != null)
						{
							tmaxItem = new CTmaxItem(dxDesignation);
							tmaxItem.XmlDesignation = O;
							tmaxAdded.Add(tmaxItem);
						}
						
					}
					else
					{
						bSuccessful = false;
						break;

					}// if((dxDesignation = AddDesignation(O)) == null)
					
				}// foreach(CXmlDesignation O in xmlDesignations)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddDesignations", m_tmaxErrorBuilder.Message(ERROR_ADD_DESIGNATIONS_EX), Ex);
				bSuccessful = false;
			}
			
			//	Was there a problem?
			if(bSuccessful == false)
			{
				if((dxDesignations != null) && (dxDesignations.Count > 0))
				{
					//	Delete the records we added to the database
					foreach(CDxTertiary O in dxDesignations)
					{
						try
						{
							tmaxItem = new CTmaxItem(O.Secondary);
							tmaxItem.SubItems.Add(new CTmaxItem(O));
							m_tmaxDatabase.Delete(tmaxItem, null, null);
						}
						catch
						{
						}
							
					}// foreach(CDxTertiary O in dxDesignations)
					
					dxDesignations = null;
				
				}// if(dxDesignations != null)
				
			}// if(bSuccessful == false)
				
			return dxDesignations;
					
		}// private CDxTertiaries AddDesignations(CXmlDesignations xmlDesignations)

		/// <summary>This method is called to add a designation record to the database</summary>
		/// <param name="xmlDesignation">the XML designation descriptor</param>
		/// <returns>The exchange interface to the new designation record</returns>
		private CDxTertiary AddDesignation(CXmlDesignation xmlDesignation)
		{
			CDxSecondary			dxSegment = null;
			CDxTertiary				dxTertiary = null;
			CTmaxDatabaseResults	tmaxResults = null;
			CTmaxItem				tmaxItem = null;
			CTmaxItem				xmlItem = null;
			
			Debug.Assert(xmlDesignation != null);
			Debug.Assert(m_dxDeposition != null);

			if(xmlDesignation == null) return null;
			if(m_dxDeposition == null) return null;
			
			try
			{
				//	Locate the segment record that owns the new designation
				if((dxSegment = m_dxDeposition.GetSegment(xmlDesignation.Segment)) == null)
				{
					m_tmaxEventSource.FireError(this, "AddDesignation", m_tmaxErrorBuilder.Message(ERROR_XML_SEGMENT_NOT_FOUND, xmlDesignation.Segment));
					return null;
				}
					
				//	Create the item that represents the parent segment
				tmaxItem = new CTmaxItem(dxSegment);
				
				//	Create the item to represent the XML designation
				xmlItem = new CTmaxItem();
				xmlItem.XmlDesignation = xmlDesignation;
				
				//	Add to the Segment item's Source collection
				if(tmaxItem.SourceItems == null)
					tmaxItem.SourceItems = new CTmaxItems();
				tmaxItem.SourceItems.Add(xmlItem);
				
				//	Add to the database
				tmaxResults = new CTmaxDatabaseResults();
				if(m_tmaxDatabase.Add(tmaxItem, null, tmaxResults) == true)
				{
					if((tmaxResults.Added.Count > 0) && (tmaxResults.Added[0].SubItems.Count > 0))
					{
						dxTertiary = (CDxTertiary)(tmaxResults.Added[0].SubItems[0].GetMediaRecord());
					}

				}// if(m_tmaxDatabase.Add(tmaxItem, tmaxAdded, null) == true)
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddDesignation", m_tmaxErrorBuilder.Message(ERROR_ADD_DESIGNATION_EX), Ex);
			}
			
			if((tmaxResults != null) && (tmaxResults.Added != null))
			{
				tmaxResults.Clear();
				tmaxResults = null;
			}
			
			return dxTertiary;
			
		}// private CDxTertiary AddDesignation(CXmlDesignation xmlDesignation)

		/// <summary>Called to move the specified link from its original owner to a new owner designation</summary>
		/// <param name="dxTarget">The new owner</param>
		/// <param name="dxLink">The link to be transferred</param>
		/// <returns>true if successful</returns>
		private bool MoveLink(CTmaxItem tmaxTarget, CDxQuaternary dxLink)
		{
			CTmaxDatabaseResults	tmaxResults = null;
			CTmaxItem				tmaxSource = null;
			CTmaxItem				tmaxDelete = null;
			bool					bSuccessful = true;
			CDxTertiary				dxTarget = null;
			CXmlLink				xmlLink = null;
			
			Debug.Assert(tmaxTarget != null);
			Debug.Assert(tmaxTarget.GetMediaRecord() != null);
			Debug.Assert(tmaxTarget.XmlDesignation != null);

			try
			{
				dxTarget = (CDxTertiary)(tmaxTarget.GetMediaRecord());
				
				//	Don't bother if the target and original are the same
				if((m_dxDesignation != null) && (ReferenceEquals(dxTarget, m_dxDesignation) == true))
					return true; // Nothing to do in this case

				//	Delete from the original owner's collection
				if(m_dxDesignation != null)
				{
					tmaxDelete = new CTmaxItem(m_dxDesignation);
					tmaxDelete.SubItems.Add(new CTmaxItem(dxLink));

					m_tmaxDatabase.Delete(tmaxDelete, null, null);

					if(m_tmaxResults != null)
						m_tmaxResults.Deleted.Add(tmaxDelete);
											
				}// if(m_dxDesignation != null)
				
				//	Create a new XML link
				xmlLink = new CXmlLink();
				dxLink.SetAttributes(xmlLink);
				
				//	Now add to the requested target
				if(tmaxTarget.SourceItems == null)
					tmaxTarget.SourceItems = new CTmaxItems();
				else
					tmaxTarget.SourceItems.Clear();
				
				tmaxSource = new CTmaxItem();
				tmaxSource.XmlLink = xmlLink;
				tmaxTarget.SourceItems.Add(tmaxSource);
					
				//	Add to the database
				tmaxResults = new CTmaxDatabaseResults();
				if(m_tmaxDatabase.Add(tmaxTarget, null, tmaxResults) == true)
				{
					if((tmaxResults.Added.Count > 0) && (m_tmaxResults != null))
					{
						foreach(CTmaxItem O in tmaxResults.Added)
							m_tmaxResults.Added.Add(O);
					}
					
				}// if(m_tmaxDatabase.Add(tmaxItem, tmaxAdded, null) == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "MoveLink", m_tmaxErrorBuilder.Message(ERROR_MOVE_LINK_EX, dxLink.GetBarcode(false)), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private bool MoveLink(CDxTertiary dxTarget, CDxQuaternary dxLink)
		
		/// <summary>Called to transfer the links for each designation in the collection</summary>
		/// <param name="tmaxDesignations">The designations where the links are to be moved</param>
		/// <returns>true if successful</returns>
		private bool UpdateLinks(CTmaxItems tmaxDesignations)
		{
			bool		bSuccessful = true;
			CTmaxItem	tmaxTarget = null;
			int			iIndex = 1;
			bool		bRewind = false;
			
			Debug.Assert(tmaxDesignations != null);
			Debug.Assert(tmaxDesignations.Count > 0);
			
			//	Don't bother if no original links 
			if(m_dxLinks == null) return true;
			if(m_dxLinks.Count == 0) return true;
			
			try
			{
				//	Make sure each of the original links is transferred to the
				//	appropriate designation
				foreach(CDxQuaternary O in m_dxLinks)
				{
					tmaxTarget = null;
					bRewind = true;

					//	Add to the first designation?
					if(O.StartPL <= ((CDxTertiary)(tmaxDesignations[0].GetMediaRecord())).Extent.StopPL)
					{
						tmaxTarget = tmaxDesignations[0];
					}
					else if(O.StartPL >= ((CDxTertiary)(tmaxDesignations[tmaxDesignations.Count - 1].GetMediaRecord())).Extent.StartPL)
					{
						tmaxTarget = tmaxDesignations[tmaxDesignations.Count - 1];
					}	
					else
					{
						//	Find the designation that owns this link now
						while(iIndex < tmaxDesignations.Count)
						{
							if(O.StartPL <= ((CDxTertiary)(tmaxDesignations[iIndex].GetMediaRecord())).Extent.StopPL)
							{
								tmaxTarget = tmaxDesignations[iIndex - 1];
								break;
							}
							else
							{
								//	Adjust the search index. We're doing this to optimize the
								//	lookup since everything is in sorted order
								if((iIndex = iIndex + 1) == tmaxDesignations.Count)
								{
									//	Should we rewind?
									if(bRewind == true)
									{
										bRewind = false;
										iIndex = 1;
									}
									else
									{
										//	Theoretically we never reach this point
										Debug.Assert(false, "Link target not found");
										break;
									}
									
								}// if((iIndex = iIndex + 1) == dxDesignations.Count)
								
							}
							
						}// for(int i = 1; i < dxDesignations.Count; i++)						
						
					}
					
					if(tmaxTarget != null)
					{
						MoveLink(tmaxTarget, O);
					}
					
				}// foreach(CDxQuaternary O in m_dxLinks)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "UpdateLinks", m_tmaxErrorBuilder.Message(ERROR_UPDATE_LINKS_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private bool UpdateLinks(CTmaxItems tmaxDesignations)
			
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while assigning the source records for the edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while assigning the active scene for the edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active deposition for the edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the parent deposition record to edit the designation: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the source transcript record to edit the designation: barcode = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve the path to the XML deposition required to edit the designation: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the XML deposition required to edit the designation: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open the XML deposition required to edit the designation: filename = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to check the range for the edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("The range defined for the edit operation is not valid: Start = %1 Stop = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the extents of the specified designation.");

			m_tmaxErrorBuilder.FormatStrings.Add("No highlighter identified to perform the edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the highlighter specified for the edit operation: HighlighterId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the highlighter for the edit operation: HighlighterId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the XML designations for the new range:  Start = %1 Stop = %2");
		
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve the path to the XML designation required to perform the edit operation: designation = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open the XML designation to perform the edit operation: filename = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the selected designation during the edit operation:  scene = %1 designation = %2");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new designation record during the edit operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add new designation records during the edit operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add new script scene records during the edit operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the deposition segment with the specified XML identifier: XmlId = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the XML extents for the designation to match the source: designation = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the active scene.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the Exclude Text edit operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to save the updated XML designation: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to split the designation: SplitBefore = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to transfer the link's ownership: link = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to transfer links to a new designation.");

		}// private void SetErrorStrings()

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}

		#endregion Properties	
	
	}// class CTmaxDesignationEditor
	
}// namespace FTI.Trialmax.Database

