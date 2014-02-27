using System;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmpanesVersion : FTI.Shared.CBaseVersion
	{
		public CTmpanesVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
