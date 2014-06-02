using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace TmaxSetupInsatller
{
    public struct Config
    {
        internal static string FTIPath =  !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROGRAMFILES(X86)"))
                                    ? Environment.GetEnvironmentVariable("PROGRAMFILES(X86)") + "\\FTI"
                                    : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                                      "\\FTI";

        internal static string TrialMaxPath = FTIPath + "\\TrialMax 7";
        internal static string CommonPath = FTIPath + "\\Common";
        internal static string UtilityInstaller = "TmaxUtilityInsatller.exe";
        internal static string CRRedist = "CRRedist2008_x86.msi";
        internal static string FTIORE = "ftiore_update.exe";
        internal static string WMEncoder = "WMEncoder.exe";
        internal static string KLiteCodec = "K-Lite_Codec_Pack_720_Full.exe";

    }
}
