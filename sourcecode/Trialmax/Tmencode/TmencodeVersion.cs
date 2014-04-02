using System;

namespace FTI.Trialmax.Encode
{
	/// <summary>This class is used to expose the version information for this assembly</summary>
	public class CTmencodeVersion : FTI.Shared.CBaseVersion
	{
		public CTmencodeVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	
	}// public class CTmencodeVersion : FTI.Shared.CBaseVersion

}// namespace FTI.Trialmax.Encode
