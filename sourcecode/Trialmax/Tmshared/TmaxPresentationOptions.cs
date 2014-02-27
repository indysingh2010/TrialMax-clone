using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the global options used by the TmaxPresentation application</summary>
	public class CTmaxPresentationOptions
	{
		#region Constants

		private const string INI_SECTION_PRESENTATION = "PRESENTATION";

		private const string INI_LINE_CLASSIC_LINKS = "ClassicLinks";
		private const string INI_LINE_CALLOUT_FRAME_COLOR = "CallFrameColor";

		private const bool DEFAULT_CLASSIC_LINKS = true;
		private const int DEFAULT_CALLOUT_FRAME_COLOR = 3;

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to ClassicLinks property</summary>
		private bool m_bClassicLinks = DEFAULT_CLASSIC_LINKS;

		/// <summary>Local member bound to CalloutFrameColor property</summary>
		private int m_iCalloutFrameColor = DEFAULT_CALLOUT_FRAME_COLOR;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CTmaxPresentationOptions()
		{

		}// CTmaxPresentationOptions()

		/// <summary>This method is called to load the options from the configuration file</summary>
		/// <param name="strFileSpec">The fully qualified path to the configuration file</param>
		/// <returns>true if successful</returns>
		public bool Load(string strFileSpec)
		{
			CWinIni winIni = null;
			bool	bSuccessful = false;
			
			try
			{
				Debug.Assert(strFileSpec != null);
				Debug.Assert(strFileSpec.Length > 0);
				
				//	Does the file exist?
				if(System.IO.File.Exists(strFileSpec) == true)
				{
					winIni = new CWinIni();
					winIni.Open(strFileSpec, INI_SECTION_PRESENTATION);
				}
				
				//	Were we able to open the file?
				if((winIni != null) && (winIni.FileFound == true))
				{
					m_bClassicLinks = winIni.ReadBool(INI_LINE_CLASSIC_LINKS, DEFAULT_CLASSIC_LINKS);
					m_iCalloutFrameColor = winIni.ReadInteger(INI_LINE_CALLOUT_FRAME_COLOR, DEFAULT_CALLOUT_FRAME_COLOR);
	
					bSuccessful = true;
				}
				else
				{
					m_bClassicLinks = DEFAULT_CLASSIC_LINKS;
					m_iCalloutFrameColor = DEFAULT_CALLOUT_FRAME_COLOR;
				}
				
			}
			catch
			{
			}
			
			return bSuccessful;

		}// public bool Load(string strFileSpec)

		#endregion Public Methods

		#region Properties

		/// <summary>Flag to enable classic deposition video links</summary>
		public bool ClassicLinks
		{
			get { return m_bClassicLinks; }
			set { m_bClassicLinks = value; }
		}

		/// <summary>Color identifier for callout frames</summary>
		public int CalloutFrameColor
		{
			get { return m_iCalloutFrameColor; }
			set { m_iCalloutFrameColor = value; }
		}

		#endregion Properties

	}//	public class CTmaxPresentationOptions

}// namespace FTI.Shared.Trialmax
