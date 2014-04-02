using System;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.MSOffice
{
	/// <summary>
	///	This is the base class for classes defined in this assembly that
	///	manage Microsoft Office applications 
	/// </summary>
	public class CMSOfficeApp
	{
		#region Protected Members
		
		/// <summary>Local member bound to Filename property</summary>
		protected string m_strFilename = "";
		
		/// <summary>Local member bound to Product property</summary>
		protected string m_strProduct = "";
		
		/// <summary>Local member used to fire diagnostic and error events</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CMSOfficeApp()
		{
			m_strProduct = "Microsoft Office Application";
			m_tmaxEventSource.Name = m_strProduct;
			
			//	Populate the error builder's format string collection
			SetErrorStrings();
		}
		
		/// <summary>This method is called to initialize the Office interfaces</summary>
		/// <returns>true if successful</returns>
		public virtual bool Initialize()
		{
			return true;
		}
			
		/// <summary>This method is called to terminate the Office interfaces</summary>
		public virtual void Terminate()
		{
		}
			
		/// <summary>This method is called to open the specified document</summary>
		/// <param name="strFilename">The fully qualified path to the document file</param>
		/// <returns>true if successful</returns>
		public virtual bool Open(string strFilename)
		{
			m_strFilename = strFilename;
			return true;
		}
			
		/// <summary>This method is called to close the document</summary>
		public virtual void Close()
		{
			m_strFilename = "";
		
		}// Close()
			
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>
		/// This method is called to populate the error builder's format string collection
		/// </summary>
		protected virtual void SetErrorStrings()
		{
		}

		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get
			{
				return m_tmaxEventSource;
			}
			
		}// EventSource property

		/// <summary>The document filename</summary>
		public string Filename
		{
			get
			{
				return m_strFilename;
			}
		}
		
		/// <summary>The MS Office product name</summary>
		public string Product
		{
			get
			{
				return m_strProduct;
			}
		}
		
		#endregion Properties
	
	}
}
