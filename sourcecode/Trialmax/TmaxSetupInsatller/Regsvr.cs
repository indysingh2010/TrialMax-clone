using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TmaxSetupInsatller
{
    public enum Register
    {
        Install,UnInsatll
    }

    public sealed class Regsvr
    {

        public void Register_Dlls(string filePath)
        {
            RunRegsvr(Register.Install, filePath);
        }

        public void UnRegister_Dlls(string filePath)
        {
            RunRegsvr(Register.UnInsatll, filePath);
        }

        private void RunRegsvr(Register register, string filePath)
        {
            try
            {
                 string fileinfo = string.Empty;
                if (register == Register.Install)
                {
                   

                    fileinfo = "/s" + " " + "\"" + filePath + "\"";
                    
                }
                else
                    fileinfo = "/u /s" + " " + "\"" + filePath + "\"";


                using (Process reg = new Process())
                {
                    reg.StartInfo.FileName = "regsvr32.exe";
                    reg.StartInfo.Arguments = fileinfo;
                    reg.StartInfo.UseShellExecute = false;
                    reg.StartInfo.CreateNoWindow = true;
                    reg.StartInfo.RedirectStandardOutput = true;
                    reg.Start();
                    reg.WaitForExit();
                    reg.Close();
                }
            }
            catch (Exception ex)
            {
            }
           
        }
    }
}
