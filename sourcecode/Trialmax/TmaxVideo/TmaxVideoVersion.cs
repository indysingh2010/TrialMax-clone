using System;

using FTI.Shared;

namespace FTI.Trialmax.TMVV.TmaxVideo
{
	/// <summary>This class extracts and exposes the TmaxVideo application's version identifiers</summary>
	public class CTmaxVideoVersion : FTI.Shared.CBaseVersion
	{
		public CTmaxVideoVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		
		}
	
	}// public class CTmaxVideoVersion : FTI.Shared.CBaseVersion

}// namespace FTI.Trialmax.TmaxVideo
