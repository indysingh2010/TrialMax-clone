using System;

namespace FTI.Shared
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmsharedVersion : FTI.Shared.CBaseVersion
	{
		public CTmsharedVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	
	}// public class CTmsharedVersion : FTI.Shared.CBaseVersion

}// namespace FTI.Shared
