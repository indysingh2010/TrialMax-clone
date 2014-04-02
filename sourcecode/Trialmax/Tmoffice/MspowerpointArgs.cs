using System;

namespace FTI.Trialmax.MSOffice.MSPowerPoint
{
	/// <summary>This class is used to pass arguments in a Presentation event</summary>
	public class CMSPowerPointArgs
	{
		#region Private Members
		
		/// <summary>Local member bound to EventId property</summary>
		private MSPowerPointEvents m_eEventId;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		public CMSPowerPointArgs()
		{
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Presentation Event Identifier</summary>
		public MSPowerPointEvents EventId
		{
			get { return m_eEventId; }
			set { m_eEventId = value; }
			
		}
		
		/// <summary>Presentation fully qualified file specification</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { m_strFileSpec = value; }
		}
		
		#endregion Properties
	
	}// public class CMSPowerPointArgs

}// namespace FTI.Trialmax.MSOffice.MSPowerPoint
