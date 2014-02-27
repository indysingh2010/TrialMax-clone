using System;

namespace FTI.Trialmax.Reports
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmreportsVersion : FTI.Shared.CBaseVersion
	{
		public CTmreportsVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
