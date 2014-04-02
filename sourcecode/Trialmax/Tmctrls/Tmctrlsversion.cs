using System;

namespace FTI.Trialmax.Controls
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmctrlsVersion : FTI.Shared.CBaseVersion
	{
		public CTmctrlsVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
