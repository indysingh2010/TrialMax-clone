using System;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmactxVersion : FTI.Shared.CBaseVersion
	{
		public CTmactxVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
