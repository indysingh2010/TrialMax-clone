using System;

namespace FTI.Trialmax.TmaxInstaller
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmaxInstallerVersion : FTI.Shared.CBaseVersion
	{
		public CTmaxInstallerVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
