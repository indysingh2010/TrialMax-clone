using System;

using FTI.Shared;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class extracts and exposes the assembly's version identifiers</summary>
	public class CTmvideoVersion : FTI.Shared.CBaseVersion
	{
		public CTmvideoVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	
	}// public class CTmvideoVersion : FTI.Shared.CBaseVersion

}// namespace FTI.Trialmax.TMVV
