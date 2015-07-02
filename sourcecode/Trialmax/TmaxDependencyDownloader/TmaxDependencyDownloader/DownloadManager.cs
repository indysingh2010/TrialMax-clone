using Ionic.Zip;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TmaxDependencyDownloader
{
    public class DownloadManager
    {
        #region Private Members
        //-------------------App.Config Variables-------------------//
        private string ftpAddress = ConfigurationManager.AppSettings["FTPAddress"];
        private int ftpPort = Convert.ToInt32(ConfigurationManager.AppSettings["FTPPort"]);
        private string ftpUserName = ConfigurationManager.AppSettings["FTPUserName"];
        private string ftpPassword = new SimpleAES().DecryptString(ConfigurationManager.AppSettings["FTPPassword"]);
        private string dependencyFilePath = ConfigurationManager.AppSettings["DependencyFilePath"];
        private string dependencyFileName = ConfigurationManager.AppSettings["DependencyFileName"];
        private string latestBuildFilePath = ConfigurationManager.AppSettings["LatestBuildFilePath"];
        private string latestBuildFileName = ConfigurationManager.AppSettings["LatestBuildFileName"];
        private string lstFile = ConfigurationManager.AppSettings["FileList"];
        private string sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
        private string targetFolder = ConfigurationManager.AppSettings["TargetFolder"];
        //-------------------App.Config Variables-------------------//

        //-------------------Local Variables-------------------//
        private List<DependencyFile> fileListInfoWeb = new List<DependencyFile>();
        private TimeSpan timeoutSpan = TimeSpan.FromSeconds(30);
        private DateTime lastProgressUpdate = DateTime.Now;
        private DownladFileStatus currentFileDownloadStatus = DownladFileStatus.NothingToDownload;
        private const short maxTotalDownloadTries = 3;
        private bool quitDownloader = false;
        private Downloader downloader;
        private string currentFileDownload { get; set; }
        //-------------------App.Config Variables-------------------//

        //-------------------Statistics Variables-------------------//
        private int totalFilesUncompressSuccess = 0;
        private int totalFilesUncompressFail = 0;
        private int currentFileNumber = 0;
        private int totalFilesToDownload = 0;
        private int totalFilesOnServer = 0;
        private int totalFilesDownloadFail = 0;
        private int totalFilesDownloadSuccess = 0;
        private static string currentFileUnCompress = string.Empty;
        //-------------------Statistics Variables-------------------//

        #endregion

        #region Private Methods
        private bool IntializeFileList()
        {
            try
            {
                CreateOutputFolder();
                LoggerManager.LogInfo("Intiailizing File List.");
                fileListInfoWeb = new List<DependencyFile>();
                long lstFileSize = 0;
                string lstFilePathFtp = ftpAddress + dependencyFilePath + sourceFolder + lstFile;
                bool isLstFileDownloadSuccess = false;
                for (int i = 0; i < maxTotalDownloadTries; i++)
                {
                    using (WebClient request = new WebClient())
                    {
                        lstFileSize = GetFileSize(lstFilePathFtp);
                        request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                        request.DownloadFile(new Uri(ftpAddress + dependencyFilePath + sourceFolder + lstFile), sourceFolder + lstFile);
                    }
                    if ((System.IO.File.Exists(sourceFolder + lstFile) && lstFileSize == new FileInfo(sourceFolder + lstFile).Length))
                    {
                        isLstFileDownloadSuccess = true;
                        break;
                    }
                }
                if (isLstFileDownloadSuccess)
                {
                    using (var sr = File.OpenText(sourceFolder + lstFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            DependencyFile fd = new DependencyFile(line, downloader);
                            fd.NotifyUnCompressProgress += zip1_ExtractProgress;
                            fileListInfoWeb.Add(fd);
                        }
                        totalFilesOnServer = fileListInfoWeb.Count;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LoggerManager.LogFatal(ex);
                LoggerManager.LogError("Downloading file " + lstFile + " failed.");
                return false;
            }
        }

        private long GetFileSize(string source)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(source));
            request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            long size = response.ContentLength;
            response.Close();
            return size;
        }

        private void CreateOutputFolder()
        {
            if (!Directory.Exists(sourceFolder))
                Directory.CreateDirectory(sourceFolder);
        }
        /// <summary>
        /// Intializes the FileList and process it
        /// </summary>
        /// <returns></returns>
        private bool ProcessFileList()
        {
            try
            {
                downloader.notifyDownloadProgress += proc_ErrorDataReceived;
                for (int i = 0; i < fileListInfoWeb.Count; i++)
                {
                    currentFileNumber = i + 1;
                    ProcessFile(fileListInfoWeb[i]);
                    if (quitDownloader) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.LogFatal(ex);
                LoggerManager.LogError(ex);
                return false;
            }
            finally {
                downloader.notifyDownloadProgress -= proc_ErrorDataReceived;
            }
        }

        private void ProcessFile(DependencyFile fd)
        {
            currentFileDownloadStatus = DownladFileStatus.Failed;
            string source = ftpAddress + dependencyFilePath + sourceFolder + fd.FileName;
            string destination = sourceFolder + fd.FileName;
            currentFileDownload = Path.GetFileName(fd.FileName);
            currentFileUnCompress = Path.GetFileName(fd.FileName);
            if (!fd.ExistOnDisk)
            {
                totalFilesToDownload++;
                fd.DownloadFile();
            }
            if (fd.CheckSumMatch)
            {
                if (fd.UnCompressFile())
                    totalFilesUncompressSuccess++;
                else
                    totalFilesUncompressFail++;
                Console.WriteLine();
                return;
            }
            totalFilesDownloadFail++;
            IsUserWantDownloadContinued();
        }

        private void IsUserWantDownloadContinued()
        {
            while (true)
            {
                Console.Write("Previous attempts to downloading has failed consecutively. Do you want to try again? (Y/N): ");
                string userEntered = Console.ReadLine();
                if (userEntered.ToLower().Equals("n"))
                {
                    quitDownloader = true;
                    break;
                }
                else if (userEntered.ToLower().Equals("y"))
                {
                    break;
                }
                Console.WriteLine("Invalid input.");
            }
        }

        private void StartDownloadManager()
        {
            if (!IntializeFileList())
            {
                LoggerManager.LogInfo("Download Manager Failed to Initialize File List.");
                return;
            }
            if (!ProcessFileList())
            {
                LoggerManager.LogInfo("Download Manager Failed to Process the File List.");
                return;
            }
        }

        private void ShowStats()
        {
            LoggerManager.LogInfo("========== Summary Start==========");
            LoggerManager.LogInfo("Total Dependencies             : " + totalFilesOnServer);
            LoggerManager.LogInfo("To be Downloaded From Server   : " + totalFilesToDownload);
            LoggerManager.LogInfo("Already downloaded             : " + (totalFilesOnServer - totalFilesToDownload));
            LoggerManager.LogInfo("Download(s) Succeeded          : " + totalFilesDownloadSuccess);
            LoggerManager.LogInfo("Download(s) Failed             : " + totalFilesDownloadFail);
            LoggerManager.LogInfo("Files to Uncompress            : " + totalFilesOnServer);
            LoggerManager.LogInfo("Files to Uncompress Succeeded  : " + totalFilesUncompressSuccess);
            LoggerManager.LogInfo("Files to Uncompress Failed     : " + (totalFilesOnServer - totalFilesUncompressSuccess));
            LoggerManager.LogInfo("========== Summary End==========");
            LoggerManager.LogInfo("===================== Tmax Dependency Downloader Ending =====================");
        }

        private void CopyPackagesFolder()
        {
            try
            {
                string path = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v7.0A", "InstallationFolder", "Not Exist").ToString();
                path += "Bootstrapper\\Packages\\";

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories("..\\..\\Packages\\", "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace("..\\..\\Packages\\", path));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles("..\\..\\Packages\\", "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace("..\\..\\Packages\\", path), true);

            }
            catch (Exception ex)
            {
                LoggerManager.LogError("Copying Packages folder failed.");
                LoggerManager.LogFatal(ex);
            }
        }
        
        #endregion

        #region Progress Events
        private void zip1_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            try
            { Console.Write("\r{0} Uncompressing: {1} {2}%", currentFileNumber.ToString() + "/" + fileListInfoWeb.Count, currentFileUnCompress, e.BytesTransferred * 100 / e.TotalBytesToTransfer); }
            catch { }
        }

        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lastProgressUpdate = DateTime.Now;
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }
            string[] info = e.Data.Split(' ');
            string percent = info.Where(x => x.Contains("%")).FirstOrDefault();
            string eta = info.Where(x => x.Contains("s")).FirstOrDefault();
            if (string.IsNullOrEmpty(percent))
                return;
            if (currentFileDownloadStatus != DownladFileStatus.Success && info.Length >= 3)
                if (percent == "100%")
                {
                    currentFileDownloadStatus = DownladFileStatus.Success;
                    Console.Write("\r{0} Downloading: {1} {2}        \n", currentFileNumber.ToString() + "/" + fileListInfoWeb.Count,
                    currentFileDownload, percent);
                }
                else
                {
                    eta = eta.Split('=').LastOrDefault();
                    currentFileDownloadStatus = DownladFileStatus.Downloading;
                    Console.Write("\r{0} Downloading: {1} {2} {3}       ", currentFileNumber.ToString() + "/" + fileListInfoWeb.Count,
                    currentFileDownload, percent, eta);
                }

        }
        #endregion

        #region Public Members

        #endregion

        #region Public Methods
        public DownloadManager()
        {
            downloader = new Downloader(ftpAddress, ftpUserName, ftpPassword, timeoutSpan, maxTotalDownloadTries);
            downloader.notifyDownloadProgress += proc_ErrorDataReceived;
        }

        public void StartProcess()
        {
            StartDownloadManager();
            CopyPackagesFolder();
            ShowStats();
        }
        #endregion

        #region Enum
        public enum DownladFileStatus
        {
            NothingToDownload,
            Success,
            Failed,
            Downloading
        }
        #endregion
    }
}
