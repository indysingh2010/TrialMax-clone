using System;

namespace FTI.Trialmax.Forms
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmformsVersion : FTI.Shared.CBaseVersion
	{
		public CTmformsVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
