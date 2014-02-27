using System;

namespace FTI.Trialmax.MSOffice
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmofficeVersion : FTI.Shared.CBaseVersion
	{
		public CTmofficeVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
