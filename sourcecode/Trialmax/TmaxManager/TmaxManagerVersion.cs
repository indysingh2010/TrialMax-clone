using System;

namespace FTI.Trialmax.TmaxManager
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmaxManagerVersion : FTI.Shared.CBaseVersion
	{
		public CTmaxManagerVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
