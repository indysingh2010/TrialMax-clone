using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TmaxDependencyDownloader
{
    public class DependencyFile
    {
        #region Private Members
        private string sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
        private string targetFolder = ConfigurationManager.AppSettings["TargetFolder"];
        private string dependencyFilePath = ConfigurationManager.AppSettings["DependencyFilePath"];
        private Downloader downloader;
        #endregion

        #region Private Methods
        /// <summary>
        /// Calculates and returns the checksum of the file at the given path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private int GetCheckSum(string filePath)
        {
            int checkSum = 0;
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "sum.exe",
                        Arguments = "\"" + filePath + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                StreamReader sOutput = proc.StandardOutput;
                string output = sOutput.ReadLine();
                proc.WaitForExit();
                proc.Close();
                checkSum = Convert.ToInt32(output.Split(' ').FirstOrDefault());
            }
            catch (Exception ex)
            {
                LoggerManager.LogFatal(ex);
            }
            return checkSum;
        }

        private void zip1_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (NotifyUnCompressProgress != null)
                NotifyUnCompressProgress(sender, e);
        }

        #endregion

        #region Public Properties
        public string FileName { get; set; }
        public int CheckSum { get; set; }
        public long FileSize {get; set;}
        public bool CheckSumMatch { get; set; }
        #endregion

        #region Public Methods
        public DependencyFile(string _line, Downloader _downloader)
        {
            var fields = _line.Split(',');
            FileName = fields[0];
            CheckSum = Convert.ToInt32(fields[2]);
            FileSize = Convert.ToInt64(fields[1]);
            CheckSumMatch = false;
            downloader = _downloader;
        }

        public static bool operator ==(DependencyFile file1, DependencyFile file2)
        {
            return (file1.FileName.Equals(file2.FileName) && file1.FileSize == file2.FileSize && file1.CheckSum.Equals(file2.CheckSum));
        }

        public static bool operator !=(DependencyFile file1, DependencyFile file2)
        {
            return (!file1.FileName.Equals(file2.FileName) || file1.FileSize != file2.FileSize || !file1.CheckSum.Equals(file2.CheckSum));
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            DependencyFile file = obj as DependencyFile;
            if ((System.Object)file == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this == file;
        }

        public override int GetHashCode()
        {
            return (this.FileName + this.FileSize + this.CheckSum).GetHashCode();
        }


        /// <summary>
        /// Extracts the provided file
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public bool UnCompressFile()
        {
            try
            {
                using (ZipFile zip1 = ZipFile.Read(sourceFolder + FileName))
                {
                    zip1.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zip1_ExtractProgress);
                    zip1.ExtractAll(targetFolder + Path.GetDirectoryName(FileName), ExtractExistingFileAction.OverwriteSilently);
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.LogFatal(ex);
                return false;
            }
        }

        public bool ExistOnDisk
        {
            get
            {
                string outputFile = sourceFolder + FileName;
                if (File.Exists(outputFile))
                {
                    if (FileSize == new FileInfo(outputFile).Length)
                    {
                        if (CheckSum == GetCheckSum(outputFile))
                        {
                            CheckSumMatch = true;
                            return true;
                        }
                    }
                    try
                    {
                        File.Delete(outputFile);
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.LogError(ex);
                    }
                }
                return false;
            }
        }

        public bool DownloadFile()
        {
            string source = dependencyFilePath + sourceFolder + FileName;
            string destination = sourceFolder + FileName;
            return (downloader.DownloadFile(source, destination) && ExistOnDisk);
        }
        #endregion

        #region Event Handlers
        public EventHandler<ExtractProgressEventArgs> NotifyUnCompressProgress { get; set; }
        #endregion
    }
}
