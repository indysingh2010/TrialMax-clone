using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;
using System.Threading;


namespace TmaxSetupInsatller
{
    [RunInstaller(true)]
    public partial class TmaxInsaller : Installer
    {
   
        public TmaxInsaller()
        {
            InitializeComponent();
        }

        #region Method Uninstall
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
          Regsvr regsvr = new Regsvr();
          string trialMaxPath = this.Context.Parameters["TargetDir"];
          string commonPath = trialMaxPath.Replace("TrialMax 7\\", "Common\\"); 
          string[] fileEntries = GetActivexFilePath(commonPath);
            if (fileEntries != null)
                foreach (string fileName in fileEntries)
                {
                    regsvr.UnRegister_Dlls(fileName);
                }
        }
        #endregion

        #region Method OnAfterInstall
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            //RegisterPDF();
        }

        

        #endregion

        #region OnCommit
        protected override void OnCommitted(IDictionary savedState)
        {
            base.OnCommitted(savedState);
            string trialMaxPath = this.Context.Parameters["TargetDir"];
            string commonPath = trialMaxPath.Replace("TrialMax 7\\", "Common\\"); 
            RegisterActivexControl(GetActivexFilePath(commonPath));
            SetFolderPermission(trialMaxPath);
            RegisterPDF();
            
        }

        private void DeleteCommonExtraFiles()
        {
            string trialMaxPath = this.Context.Parameters["TargetDir"];
            string commonPath = trialMaxPath.Replace("TrialMax 7\\", "Common\\"); 
            if (!Directory.Exists(commonPath)) return;
            string[] files = Directory.GetFiles(commonPath);
            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    if (DeleteFile(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        private bool DeleteFile(string file)
        {
            bool flag;
            flag= file.Contains(".ocx") || file.ToLower().Equals("fti_ado.dll") ||
                   file.ToLower().Equals("fti_gen.dll") || file.ToLower().Equals("licence.txt");
            return !flag;
        }
        
      /*  private void InstallUtilities(object obj)
        {
            try
            {
                string fileName = Config.TrialMaxPath + "\\Utilities\\" + Config.CRRedist;
                if (!File.Exists(fileName)) return;
               Process _CRProcess = new Process();
                _CRProcess.StartInfo.FileName = fileName;
                _CRProcess.Start();
                _CRProcess.EnableRaisingEvents = true;
                _CRProcess.Close();

               //// Directory.SetCurrentDirectory(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                Process helperProcess = Process.Start(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +"\\"+
                    Config.UtilityInstaller);
            }
            catch (Exception ex)
            {
                // Do nothing
            }
        }*/

        private void SetFolderPermission(string dirPath)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(dirPath);
                WindowsIdentity self = System.Security.Principal.WindowsIdentity.GetCurrent();
               // To assign rights to all users
                string userName = "Users";// self.Name;// "Users";
                DirectorySecurity ds = info.GetAccessControl();
                ds.AddAccessRule(new FileSystemAccessRule(
                                     userName,
                                     FileSystemRights.FullControl,
                                     InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                     PropagationFlags.None,
                                     AccessControlType.Allow
                                     )
                    );
                info.SetAccessControl(ds);
            }
            catch (Exception)
            {
                
               // do nothing
            }
           
        }


        private void RegisterActivexControl(string[] fileEntries)
        {
            Regsvr regsvr = new Regsvr();
            if (fileEntries != null)
            {
                foreach (string fileName in fileEntries)
                {
                        regsvr.Register_Dlls(fileName);
                }
            }
        }

        #endregion

        #region Helper Methods
        private void RegisterPDF()
        {
            string ORERegKey = "FTIORE";
            SetRegistryValue(ORERegKey);
        }

        private void SetRegistryValue(string key)
        {
            string trialMaxPath = this.Context.Parameters["TargetDir"];
            string ftiPath = trialMaxPath.Replace("TrialMax 7\\", ""); 
            RegistryKey ftiRegistry = Registry.LocalMachine.OpenSubKey
                ("SOFTWARE\\Wow6432Node\\"+key, true);
            if (ftiRegistry == null)
                ftiRegistry = Registry.LocalMachine.OpenSubKey
                    ("SOFTWARE\\"+key, true);
            if (ftiRegistry != null)
            {
                string filePath = ftiPath + key;
                ftiRegistry.SetValue("", filePath);
                ftiRegistry.Close();
            }
        }

        private static string[] GetActivexFilePath(string filesPath)
        {
            

            if (!Directory.Exists(filesPath)) return null;
            string[] files = Directory.GetFiles(filesPath);
            if (files != null && files.Length > 0)
            {
                int count = 0;
                foreach (string file in files)
                {
                    if (!file.ToLower().Equals("mfc70.dll") || file.Contains(".ocx") || file.Contains(".dll"))
                    {
                        files[count] = file;
                        count++;
                    }
                }
            }
            return files;
        }

        #endregion
    }
}
