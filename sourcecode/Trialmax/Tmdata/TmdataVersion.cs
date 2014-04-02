using System;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;


namespace FTI.Trialmax.Database
{
	/// <summary>
	/// This class is used to expose the version information for this assembly
	/// </summary>
	public class CTmdataVersion : FTI.Shared.CBaseVersion
	{
		public CTmdataVersion() : base()
		{
			SetTmaxLocation(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}
	}
}
