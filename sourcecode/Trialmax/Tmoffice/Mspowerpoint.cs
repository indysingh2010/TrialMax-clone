using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Office;
using PowerPoint;

using FTI.Shared;
using FTI.Trialmax.MSOffice;

namespace FTI.Trialmax.MSOffice.MSPowerPoint
{
	/// <summary>Enumerations of Presentation events</summary>
	public enum MSPowerPointEvents
	{
		OpenPresentation,
		ClosePresentation,
		SavePresentation,
		NewPresentation,
		NewSlide,
	}
	
	/// <summary>This is the delegate used to handle all Presentation events</summary>
	/// <param name="objSender">Object firing the event</param>
	/// <param name="Args">Object containing the event arguments</param>
	public delegate void MSPowerPointEventHandler(object objSender, CMSPowerPointArgs Args);
		
	/// <summary>
	/// This class encapsulates a PowerPoint presentation
	/// </summary>
	public class CMSPowerPoint : FTI.Trialmax.MSOffice.CMSOfficeApp
	{
		#region Constants
		
		protected const int MAX_INITIALIZE_ATTEMPTS	= 4;
		
		protected const int ERROR_OPEN_EX				= 0;
		protected const int ERROR_GET_SLIDE_COUNT_EX	= 1;
		protected const int ERROR_GET_SLIDE_ID_EX		= 2;
		protected const int ERROR_GET_SLIDE_FROM_ID_EX	= 3;
		protected const int ERROR_EXPORT_SLIDE_EX		= 4;
		protected const int ERROR_GET_SLIDE_NAME_EX		= 5;
		protected const int ERROR_GET_SLIDE_NOTES_EX	= 6;
		protected const int ERROR_GET_SLIDE_TITLE_EX	= 7;
		protected const int ERROR_GET_SLIDE_HEADER_EX	= 8;
		protected const int ERROR_GET_SLIDE_FOOTER_EX	= 9;
		protected const int ERROR_GET_NOTES_HEADER_EX	= 10;
		protected const int ERROR_GET_NOTES_FOOTER_EX	= 11;
		protected const int ERROR_INITIALIZE_EX			= 12;
		protected const int ERROR_NOT_INITIALIZED		= 13;
		protected const int ERROR_ADVISE_EX				= 14;
		protected const int ERROR_EXECUTE_EX			= 15;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>PowerPoint application object</summary>
		private PowerPoint._Application m_ppApplication = null;
		
		private PowerPoint.Presentations m_ppPresentations = null;
		
		/// <summary>PowerPoint application object</summary>
		private PowerPoint.Presentation m_ppPresentation = null;
		
        ///// <summary>COM connection point for event handling</summary>
        //private System.Runtime.InteropServices.ComTypes.IConnectionPointContainer m_comConnectionPoint = null;
		
		/// <summary>COM connection point container application</summary>
        //private UCOMIConnectionPointContainer m_comPointContainer = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This event is fired to bubble PowerPoint events</summary>
		public event MSPowerPointEventHandler PowerPointEvent;
		
		public CMSPowerPoint() : base()
		{
			m_strProduct = "PowerPoint";
			m_tmaxEventSource.Name = m_strProduct;
		}
		
		/// <summary>This method is called to initialize the control</summary>
		/// <returns>True if successful</returns>
		public override bool Initialize()
		{
			//	Have we already initialized?
			if(m_ppPresentations != null) 
				return true;
			
			for(int i = 0; i < MAX_INITIALIZE_ATTEMPTS; i++)
			{
				try
				{
					if(m_ppApplication == null)
						m_ppApplication = new PowerPoint.Application();
						
					if((m_ppPresentations = m_ppApplication.Presentations) != null)					
						break;
					else
						System.Threading.Thread.Sleep(500);
				}
				catch(System.Exception Ex)
				{
					//	Is this our last attempt?
					if(i + 1 == MAX_INITIALIZE_ATTEMPTS)
					{
						m_tmaxEventSource.FireError(this, "Initilize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
						m_ppApplication = null;
						m_ppPresentations = null;
					
						return false;
					}
					else
					{
						System.Threading.Thread.Sleep(500);
					}
						
				}
			
			}// for(int i = 0; i < MAX_INITIALIZE_ATTEMPTS; i++)

			return (m_ppPresentations != null);
			
		}// public bool Initialize()
			
		/// <summary>This method is called to terminate the interfaces</summary>
		public override void Terminate()
		{
			try
			{
				//	Close the active presentation
				Close();
					
				m_ppApplication = null;
				m_ppPresentations = null;
				
				//	Notify the garbage collection
				GC.Collect();				
			}
			catch
			{
			}
		
		}// Close()
			
		/// <summary>This method is called to open the specified presentation</summary>
		/// <param name="strFilename">The fully qualified path to the presentation file</param>
		/// <returns>true if successful</returns>
		public override bool Open(string strFilename)
		{
			try
			{
				//	Make sure the control has been initialized
				if(Initialize() == false)
					return false;

				//	Open the presentation
				m_ppPresentation = m_ppPresentations.Open(strFilename, 
														  MsoTriState.msoTrue,	// Read-only
														  MsoTriState.msoTrue,	// Untitled
														  MsoTriState.msoFalse);  // With window
				m_strFilename = strFilename;
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_EX, strFilename), Ex);
				Close();
				return false;
			}
		
		}// public override bool Open(string strFilename)
			
		/// <summary>This method is called to close the active presentation</summary>
		public override void Close()
		{
			try
			{
				if(m_ppPresentation != null)
					m_ppPresentation.Close();
					
				m_ppPresentation = null;
				m_strFilename = "";

				//GC.Collect();
			}
			catch
			{
			}
		
		}// Close()
			
        ///// <summary>This method is called to attach to the PowerPoint event source</summary>
        ///// <returns>true if successful</returns>
        //public bool Advise()
        //{
        //    //	Are we already listening to events?
        //    if(m_comConnectionPoint != null)
        //        return true;
				
        //    try
        //    {
        //        //	Make sure the control has been initialized
        //        if(Initialize() == false)
        //            return false;

        //        m_comPointContainer = (UCOMIConnectionPointContainer)m_ppApplication;
				
        //        //	Establish the connection point for event sinking
        //        Guid guid = typeof(PowerPoint.EApplication).GUID;
        //        m_comPointContainer.FindConnectionPoint(ref guid, out m_comConnectionPoint);
				
        //        //	Request event notification
        //        m_comConnectionPoint.Advise(this, out m_iCookie);
				
        //        return true;
        //    }
        //    catch(System.Exception Ex)
        //    {
        //        m_tmaxEventSource.FireError(this, "Advise", m_tmaxErrorBuilder.Message(ERROR_ADVISE_EX), Ex);
        //        UnAdvise();
        //        return false;
        //    }
        //}
			
        ///// <summary>This method is called to terminate the PowerPoint event interfaces</summary>
        //public void UnAdvise()
        //{
        //    try
        //    {
        //        if(m_comConnectionPoint != null)
        //        {
        //            m_comConnectionPoint.Unadvise(m_iCookie);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(m_ppApplication);
					
        //            m_comConnectionPoint = null;
        //            m_iCookie = 0;
					
        //        }
				
        //        m_comPointContainer = null;
				
        //        GC.Collect();
				
        //    }
        //    catch
        //    {
        //    }
		
        //}// public void UnAdvise()
		
		/// <summary>This method is called to open the specified presentation</summary>
		/// <param name="strFilename">The fully qualified path to the presentation file</param>
		/// <returns>true if successful</returns>
		public bool Execute(string strFilename, int iSlideId)
		{
			PowerPoint.Presentation ppPresentation = null;
			PowerPoint.Slides ppSlides = null;
			PowerPoint.Slide ppSlide = null;
			bool bSuccessful = false;
			
			try
			{
				//	Make sure the control has been initialized
				if(Initialize() == false)
					return false;

				m_ppApplication.Visible = Office.MsoTriState.msoTrue;
				
				//	Make sure PowerPoint has not been shrunk down by TrialMax Presentation
				if((m_ppApplication.Width < 100) || (m_ppApplication.Height < 100))
					m_ppApplication.WindowState = PowerPoint.PpWindowState.ppWindowMaximized;
				
				//	Check to see if this presentation is already active
				foreach(Presentation O in m_ppPresentations)
				{
					if(String.Compare(O.FullName, strFilename, true) == 0)
					{
						ppPresentation = O;
						
					}
				}
					
				//	Do we need to open the specified presentation?
				if(ppPresentation == null)
				{
					ppPresentation = m_ppPresentations.Open(strFilename, 
															MsoTriState.msoFalse,	// Read-only
															MsoTriState.msoFalse,	// Untitled
															MsoTriState.msoTrue);  // With window
				}
				else
				{
					//	Make sure this is the active presentation
					foreach(DocumentWindow O in m_ppApplication.Windows)
					{
						if(ReferenceEquals(O.Presentation, ppPresentation) == true)
						{
							O.Activate();
						}
						
					}
				}

				//	Do we need to go to a specific slide?
				if(iSlideId > 0)
				{
					try
					{
						if((ppSlides = ppPresentation.Slides) != null)
						{
							if((ppSlide = ppSlides.FindBySlideID(iSlideId)) != null)
							{
								ppSlide.Select();
							}
						}
					}
					catch
					{
					}
					
				}
				
				//	Bring the application to the front
				try
				{
					m_ppApplication.Visible = MsoTriState.msoTrue;
					m_ppApplication.Activate();
					FTI.Shared.Win32.User.SetForegroundWindow((IntPtr)(m_ppApplication.HWND));
				}
				catch
				{
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Execute", m_tmaxErrorBuilder.Message(ERROR_EXECUTE_EX, strFilename), Ex);
			}
			finally
			{
				ppPresentation = null;
				ppSlides = null;
				ppSlide = null;
				
				GC.Collect();
			}
			
			return bSuccessful;
		
		}// public bool Execute(string strFilename, long lSlideId)
			
		/// <summary>This method is called to get the number of slides in the presentation</summary>
		/// <returns>The number of slides if successful, -1 on failure</returns>
		public long GetSlideCount()
		{
			long				lSlides = -1;
			PowerPoint.Slides	ppSlides = null;
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return -1;
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					lSlides = ppSlides.Count;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSlideCount", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_COUNT_EX, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
			}
					
			return lSlides;
		
		}// GetSlideCount(string strFilename)
		
		/// <summary>This method is called to get the unique ID assigned to the slide at the specified index</summary>
		/// <param name="iIndex">The index of the slide in the presentation's slide collection</param>
		/// <returns>The ID if found, -1 on failure</returns>
		public long GetSlideId(int iIndex)
		{
			long lId = -1;
			PowerPoint.Slides ppSlides = null;
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return -1;
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					lId = (ppSlides.Item(iIndex)).SlideID;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSlideId", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_COUNT_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
			}
					
			return lId;
		
		}// GetSlideCount(int iIndex)
		
		/// <summary>This method is called to get the Name assigned to the slide at the specified index</summary>
		/// <param name="iIndex">The name of the slide in the presentation's slide collection</param>
		/// <returns>The name if found, Empty string on failure</returns>
		public string GetSlideName(int iIndex)
		{
			string strName = "";
			PowerPoint.Slides ppSlides = null;
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return "";
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					strName = (ppSlides.Item(iIndex)).Name;
			}
			catch(System.Exception)
			{
				//m_tmaxEventSource.FireError(this, "GetSlideName", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_NAME_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
			}
					
			return strName;
		
		}// GetSlideName(int iIndex)
		
		/// <summary>This method is called to get the Notes assigned to the slide at the specified index</summary>
		/// <param name="iIndex">The notes added to the slide</param>
		/// <returns>The notes text if found, Empty string on failure</returns>
		public string GetSlideNotes(int iIndex)
		{
			PowerPoint.Slides	ppSlides = null;
			PowerPoint.Slide	ppSlide = null;
			string				strNotes = "";

			//	Is the presentation open?
			if(m_ppPresentation == null) return "";
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					ppSlide = ppSlides.Item(iIndex);
					
				if((ppSlide.NotesPage != null) && (ppSlide.NotesPage.Shapes != null))
				{
					if(ppSlide.NotesPage.Shapes.Placeholders != null)
					{
						if(ppSlide.NotesPage.Shapes.Placeholders.Item(2) != null)
						{
							if(ppSlide.NotesPage.Shapes.Placeholders.Item(2).TextFrame != null)
							{
								strNotes = ppSlide.NotesPage.Shapes.Placeholders.Item(2).TextFrame.TextRange.Text;
							}
							
						}
						
					}
					
					//	This actually pulls more than just notes
					//
					//	It also grabs headers and footers
//					foreach(PowerPoint.Shape oShape in oSlide.NotesPage.Shapes)
//					{
//						if((oShape.HasTextFrame == Office.MsoTriState.msoTrue) && 
//							(oShape.TextFrame.HasText == Office.MsoTriState.msoTrue))
//						{
//							if(strNotes.Length > 0) 
//								strNotes += "\n";
//							
//							strNotes += oShape.TextFrame.TextRange.Text;
//						}
//
//					}
				
				}
				
			}
			catch(System.Exception)
			{
				//m_tmaxEventSource.FireError(this, "GetSlideNotes", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_NOTES_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
				ppSlide = null;
			}
					
			return strNotes;
		
		}// GetSlideNotes(int iIndex)
		
		/// <summary>This method is called to get the Notes assigned to the slide at the specified index</summary>
		/// <param name="iIndex">The notes added to the slide</param>
		/// <returns>The notes text if found, Empty string on failure</returns>
		public string GetSlideTitle(int iIndex)
		{
			PowerPoint.Slides	ppSlides = null;
			PowerPoint.Slide	ppSlide = null;
			string				strTitle = "";
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return "";
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					ppSlide = ppSlides.Item(iIndex);
					
				for(int i = 1; i <= ppSlide.Shapes.Placeholders.Count; i++)
				{
					switch(ppSlide.Shapes.Placeholders.Item(i).PlaceholderFormat.Type)
					{
						case PowerPoint.PpPlaceholderType.ppPlaceholderCenterTitle:
						case PowerPoint.PpPlaceholderType.ppPlaceholderTitle:
						case PowerPoint.PpPlaceholderType.ppPlaceholderVerticalTitle:
						
							strTitle = ppSlide.Shapes.Placeholders.Item(i).TextFrame.TextRange.Text;
							break;
							
					}
					
					if(strTitle.Length > 0)
						break;
				}
				
			}
			catch(System.Exception)
			{
				//m_tmaxEventSource.FireError(this, "GetSlideTitle", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_TITLE_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
				ppSlide = null;
			}
					
			return strTitle;
		
		}// GetSlideTitle(int iIndex)
		
		/// <summary>This method is called to get the header assigned to the slide at the specified index</summary>
		/// <param name="iIndex">The notes added to the slide</param>
		/// <returns>The notes text if found, Empty string on failure</returns>
		public string GetSlideHeader(int iIndex)
		{
			PowerPoint.Slides	ppSlides = null;
			PowerPoint.Slide	ppSlide = null;
			string				strHeader = "";
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return "";
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					ppSlide = ppSlides.Item(iIndex);
					
				for(int i = 1; i <= ppSlide.Shapes.Placeholders.Count; i++)
				{
					switch(ppSlide.Shapes.Placeholders.Item(i).PlaceholderFormat.Type)
					{
						case PowerPoint.PpPlaceholderType.ppPlaceholderHeader:
						
							strHeader = ppSlide.Shapes.Placeholders.Item(i).TextFrame.TextRange.Text;
							break;
							
					}
					
					if(strHeader.Length > 0)
						break;
				}
				
			}
			catch(System.Exception)
			{
				//m_tmaxEventSource.FireError(this, "GetSlideHeader", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_HEADER_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
				ppSlide = null;
			}
					
			return strHeader;
		
		}// GetSlideHeader(int iIndex)
		
		/// <summary>This method is called to get the header assigned to the slide at the specified index</summary>
		/// <param name="iIndex">The notes added to the slide</param>
		/// <returns>The notes text if found, Empty string on failure</returns>
		public string GetSlideFooter(int iIndex)
		{
			PowerPoint.Slides	ppSlides = null;
			PowerPoint.Slide	ppSlide = null;
			string				strFooter = "";
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return "";
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					ppSlide = ppSlides.Item(iIndex);
					
				for(int i = 1; i <= ppSlide.Shapes.Placeholders.Count; i++)
				{
					switch(ppSlide.Shapes.Placeholders.Item(i).PlaceholderFormat.Type)
					{
						case PowerPoint.PpPlaceholderType.ppPlaceholderFooter:
						
							strFooter = ppSlide.Shapes.Placeholders.Item(i).TextFrame.TextRange.Text;
							break;
							
					}
					
					if(strFooter.Length > 0)
						break;
				}
				
			}
			catch(System.Exception)
			{
				//m_tmaxEventSource.FireError(this, "GetSlideFooter", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_FOOTER_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
				ppSlide = null;
			}
					
			return strFooter;
		
		}// GetSlideFooter(int iIndex)
		
		/// <summary>This method is called to get the footer assigned to the slide notes at the specified index</summary>
		/// <param name="iIndex">The index of the desired slide</param>
		/// <returns>The footer text if found</returns>
		public string GetNotesFooter(int iIndex)
		{
			PowerPoint.Slides	ppSlides = null;
			PowerPoint.Slide	ppSlide = null;
			string				strFooter = "";
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return "";
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					ppSlide = ppSlides.Item(iIndex);
					
				for(int i = 1; i <= ppSlide.NotesPage.Shapes.Placeholders.Count; i++)
				{
					switch(ppSlide.NotesPage.Shapes.Placeholders.Item(i).PlaceholderFormat.Type)
					{
						case PowerPoint.PpPlaceholderType.ppPlaceholderFooter:
						
							strFooter = ppSlide.NotesPage.Shapes.Placeholders.Item(i).TextFrame.TextRange.Text;
							break;
							
					}
					
					if(strFooter.Length > 0)
						break;
				}
				
			}
			catch(System.Exception)
			{
				//m_tmaxEventSource.FireError(this, "GetNotesFooter", m_tmaxErrorBuilder.Message(ERROR_GET_NOTES_FOOTER_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
				ppSlide = null;
			}
					
			return strFooter;
		
		}// GetNotesFooter(int iIndex)
		
		/// <summary>
		/// This method is called to get the header assigned to the slide notes at the specified index
		/// </summary>
		/// <param name="iIndex">The index of the desired slide</param>
		/// <returns>The header text if found</returns>
		public string GetNotesHeader(int iIndex)
		{
			PowerPoint.Slides	ppSlides = null;
			PowerPoint.Slide	ppSlide = null;
			string				strHeader = "";
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return "";
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					ppSlide = ppSlides.Item(iIndex);
					
				for(int i = 1; i <= ppSlide.NotesPage.Shapes.Placeholders.Count; i++)
				{
					switch(ppSlide.NotesPage.Shapes.Placeholders.Item(i).PlaceholderFormat.Type)
					{
						case PowerPoint.PpPlaceholderType.ppPlaceholderHeader:
						
							strHeader = ppSlide.NotesPage.Shapes.Placeholders.Item(i).TextFrame.TextRange.Text;
							break;
							
					}
					
					if(strHeader.Length > 0)
						break;
				}
				
			}
			catch(System.Exception)
			{
				//m_tmaxEventSource.FireError(this, "GetNotesHeader", m_tmaxErrorBuilder.Message(ERROR_GET_NOTES_HEADER_EX, iIndex, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
				ppSlide = null;
			}
					
			return strHeader;
		
		}// GetNotesHeader(int iIndex)
		
		/// <summary>This method is called to get the slide with the specified PowerPoint id</summary>
		/// <param name="iId">The id assigned to the slide by PowerPoint</param>
		/// <returns>The requested slide if found</returns>
		public PowerPoint.Slide GetSlideFromId(int iId)
		{
			PowerPoint.Slides ppSlides = null;
			PowerPoint.Slide  ppSlide = null;
			
			//	Is the presentation open?
			if(m_ppPresentation == null) return null;
				
			try
			{
				if((ppSlides = m_ppPresentation.Slides) != null)
					ppSlide = ppSlides.FindBySlideID(iId);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSlideFromId", m_tmaxErrorBuilder.Message(ERROR_GET_SLIDE_FROM_ID_EX, iId, m_strFilename), Ex);
			}
			finally
			{
				ppSlides = null;
			}
			
			return ppSlide;
		}// public PowerPoint.Slide GetSlideFromId(int iId)
		
		/// <summary>This method is called to export the specified slide</summary>
		/// <param name="iSlideId">The PowerPoint slide identifier</param>
		/// <returns>true if successful</returns>
		public bool Export(int iSlideId, string strFilename)
		{
			PowerPoint.Slide	ppSlide = GetSlideFromId(iSlideId);
			string				strFilter = "";
			
			if(ppSlide != null)
			{
				//	Use the file extension to get the filter
				if((strFilter = System.IO.Path.GetExtension(strFilename)) != null)
				{
					if(strFilter.StartsWith(".") == true)
						strFilter = strFilter.Remove(0, 1);
						
					if(strFilter.Length > 0)
					{
						try
						{
							ppSlide.Export(strFilename, strFilter, 0, 0);
							return true;
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireError(this, "Export", m_tmaxErrorBuilder.Message(ERROR_EXPORT_SLIDE_EX, iSlideId, strFilename), Ex);
						}
					
					}// if(strFilter.Length > 0)
					
				}
			
			}// if(ppSlide != null)
					
			return false;
		
		}// public bool Export(int iSlideId, string strFilename)
		
		
		/// <summary>This handles PresentationClose events fired by PowerPoint</summary>
		/// <param name="pres">The presentation being closed</param>
		[DispId(2004)]
		public void OnPresentationClose(Presentation pres)
		{
			if(pres != null)
			{
				FirePowerPointEvent(MSPowerPointEvents.ClosePresentation,
									  pres.Path);
			}
		}
		
		/// <summary>This handles PresentationSave events fired by PowerPoint</summary>
		/// <param name="pres">The presentation being saved</param>
		[DispId(2005)]
		public void OnPresentationSave(Presentation pres)
		{
			if(pres != null)
			{
				FirePowerPointEvent(MSPowerPointEvents.SavePresentation,
					pres.Path);
			}
		}
		
		/// <summary>This handles PresentationOpen events fired by PowerPoint</summary>
		/// <param name="pres">The presentation being opened</param>
		[DispId(2006)]
		public void OnPresentationOpen(Presentation pres)
		{
			if(pres != null)
			{
				FirePowerPointEvent(MSPowerPointEvents.OpenPresentation,
									  pres.FullName);
			}
		}
		
		/// <summary>This handles NewPresentation events fired by PowerPoint</summary>
		/// <param name="pres">The new presentation</param>
		[DispId(2007)]
		public void OnNewPresentation(Presentation pres)
		{
			if(pres != null)
			{
				FirePowerPointEvent(MSPowerPointEvents.NewPresentation,
									  pres.Path);
			}
		}
		
		/// <summary>This handles NewSlide events fired by PowerPoint</summary>
		/// <param name="slide">The new slide</param>
		[DispId(2008)]
		public void OnNewSlide(Slide slide)
		{
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method will fire an event using the specified parameters</summary>
		/// <param name="eId">The event identifier</param>
		/// <param name="strFileSpec">Fully qualified file specification</param>
		protected void FirePowerPointEvent(MSPowerPointEvents eId, string strFileSpec)
		{
			if(PowerPointEvent != null)
			{
				try
				{
					CMSPowerPointArgs Args = new CMSPowerPointArgs();
					
					Args.EventId = eId;
					Args.FileSpec = strFileSpec;
					
					PowerPointEvent(this, Args);
				}
				catch
				{
				}
				
			}
		}
		
		/// This method is called to populate the error builder's format string collection
		/// </summary>
		/// <remarks>The strings should be added to the collection in the same order in which they are enumerated</remarks>
		protected override void SetErrorStrings()
		{
			if((m_tmaxErrorBuilder != null) && (m_tmaxErrorBuilder.FormatStrings != null))
			{
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to open %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the slide count for %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the slide id for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the slide with id = #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to export the slide with id = #%1 to %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the slide name for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the notes for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the title for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the header for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the footer for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the notes header for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to retrieve the notes footer for slide #%1 in %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to initialize the PowerPoint interfaces");
				m_tmaxErrorBuilder.FormatStrings.Add("An attempt was made to use the PowerPoint control before it was initialized");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to attach to the PowerPoint event interfaces");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to execute PowerPoint with this presentation: %1");
			}
			
		}// SetErrorStrings()
		
		#endregion Protected Methods
		
		#region Properties
		
		#endregion Properties
	
	}// public class CPresentation

}// namespace FTI.Trialmax.Office.PowerPoint
