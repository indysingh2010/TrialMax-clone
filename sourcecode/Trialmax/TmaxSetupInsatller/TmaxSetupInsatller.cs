using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

namespace TmaxSetupInsatller
{
    public class TmaxSetupInsatller
    {
        static void Main(string[] args)
        {
            try
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                InstallWMEncoder();
                System.Windows.Forms.Application.Run(new InstallerForm());
            }
            catch (Exception)
            {
            }
        }

        private static void InstallCrystalReport()
        {
            string fileName = Config.CRRedist;
            if (!File.Exists(fileName)) return;
           Process _CRProcess = new Process();
            _CRProcess.StartInfo.FileName = fileName;
            _CRProcess.Start();
            _CRProcess.EnableRaisingEvents = true;
            _CRProcess.Close();
        }

        private static void InstallWMEncoder()
        {
            string fileName = Config.WMEncoder;
            Console.Write(fileName);
            if (!File.Exists(fileName)) return;
            Process _CRProcess = new Process();
            _CRProcess.StartInfo.FileName = fileName;
            _CRProcess.Start();
            _CRProcess.EnableRaisingEvents = true;
            _CRProcess.Close();
        }

        private static void InstallKLiteCodec()
        {
            string fileName = Config.KLiteCodec;
            Console.Write(fileName);
            if (!File.Exists(fileName)) return;
            Process _CRProcess = new Process();
            _CRProcess.StartInfo.FileName = fileName;
            _CRProcess.Start();
            _CRProcess.EnableRaisingEvents = true;
            _CRProcess.Close();
        }
    }
}
