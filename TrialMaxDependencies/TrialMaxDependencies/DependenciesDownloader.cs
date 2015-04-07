using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Ionic.Zip;
using System.Security.Cryptography;

namespace TrialMaxDependencies
{
    public class FileDetails
    {
        public string fileName { get; set;}
        public string md5Hash { get; set; }
        public long fileSize { get; set; }
        public bool status { get; set; }
        public FileDetails(string _fileName, long _fileSize, string _md5Hash)
        {
            fileName = _fileName;
            md5Hash = _md5Hash;
            fileSize = _fileSize;
            status = false;
        }

        public static bool operator ==(FileDetails c1, FileDetails c2)
        {
            return (c1.fileName.Equals(c2.fileName) && c1.fileSize == c2.fileSize && c1.md5Hash.Equals(c2.md5Hash));
        }

        public static bool operator !=(FileDetails c1, FileDetails c2)
        {
            return (!c1.fileName.Equals(c2.fileName) || c1.fileSize != c2.fileSize || !c1.md5Hash.Equals(c2.md5Hash));
        }

    }

    public class DependenciesDownloader
    {
        private static string FTPAddress = string.Empty;
        private static int FTPPort = 0;
        private static string DependencyFilePath = string.Empty;
        private static string DependencyFileName = string.Empty;
        private static string LatestBuildFilePath = string.Empty;
        private static string LatestBuildFileName = string.Empty;
        private static string FTPUserName = string.Empty;
        private static string FTPPassword = string.Empty;
        private static string FileList = string.Empty;
        private static string SourceFolder = string.Empty;
        private static string TargetFolder = string.Empty;
        private static long dependencyDownloadSize = 0;
        private static long latestBuildDownloadSize = 0;
        private static int CurrentFileNumber = 0;
        private static ManualResetEvent resetDependency;
        private static ManualResetEvent resetLatestBuild;
        private static List<FileDetails> fileListInfoWeb;
        private static List<FileDetails> fileListInfoLocal;
        private static List<FileDetails> filesToDownload;
        private static string currentFileDownload = string.Empty;
        static void Main(string[] args)
        {
            Console.WriteLine("TrialMax Dependencies Downloader");
            
            Console.WriteLine("Reading Config values...");
            if (!ReadConfiguration())
            {
                Console.WriteLine("Reading Config values failed.");
                return;
            }

            //CreateLstFile.ProcessDirectory(@"D:\Dependencies\Dependencies");
            if (!IntializeFileList() || !CompareFileList())
            {
                Console.WriteLine("Intializing FileList failed.");
                return;
            }

            if (!DownloadDependencies())// || !DownloadLatestBuild())
            {
                Console.WriteLine("Downloading file(s) failed.");
                return;
            }
            //if (!UncompressDependencies() || !UncompressLatestBuild())
            //{
            //    Console.WriteLine("Uncompressing file(s) failed.");
            //    return;
            //}

            //if (!InstallLatestBuild())
            //{
            //    Console.WriteLine("Installation of Latest Build unsuccessfull.");
            //    return;
            //}

            Console.WriteLine("TrialMax Dependencies downloaded and uncompressed successfully.\nPress enter to exit.");

            Console.Read();
            return;
        }

        // Process all files in the directory passed in, recurse on any directories  
        // that are found, and process the files they contain. 
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here. 
        public static void ProcessFile(string path)
        {
            FileInfo fI = new FileInfo(path);
            fileListInfoLocal.Add(new FileDetails(path.Remove(0,DependencyFilePath.Length), fI.Length, GetMD5HashFromFile(path)));
        }

        // Generate MD5 of a file
        public static string GetMD5HashFromFile(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        private static bool CompareFileList()
        {
            try
            {
                fileListInfoLocal = new List<FileDetails>();
                filesToDownload = new List<FileDetails>();
                if (!Directory.Exists(DependencyFilePath))
                    Directory.CreateDirectory(DependencyFilePath);
                ProcessDirectory(DependencyFilePath);
                foreach (FileDetails fd in fileListInfoWeb)
                {
                    var exists = fileListInfoLocal.Count(x => x == fd);
                    if (exists == 0) // download this file from web
                    {
                        filesToDownload.Add(fd);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private static bool IntializeFileList()
        {
            try
            {
                fileListInfoWeb = new List<FileDetails>();
                using (WebClient request = new WebClient())
                {

                    request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
                    request.DownloadFile(new Uri(FTPAddress + DependencyFilePath + FileList), FileList);
                }
                if (!System.IO.File.Exists(FileList))
                {
                    return false;
                }
                using (var sr = File.OpenText(FileList))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var fields = line.Split(',');
                        fileListInfoWeb.Add(new FileDetails(fields[0], Convert.ToInt64(fields[1]), fields[2]));
                    }
                }
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private static bool InstallLatestBuild()
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "./"+ Path.GetFileNameWithoutExtension(LatestBuildFileName) + "//setup.exe";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    if (exeProcess.ExitCode == 0)
                    {
                        Console.WriteLine("Trialmax was successfully installed.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Trialmax was not installed successfully.");
                        return false;
                    }
                }
            }
            catch
            {
                return false;
                // Log error.
            }
        }

        private static bool UncompressLatestBuild()
        {
            try
            {
                using (ZipFile zip1 = ZipFile.Read(LatestBuildFileName))
                {
                    zip1.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zip1_ExtractProgress);
                    zip1.ExtractAll("./", ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            Console.WriteLine();
            Console.WriteLine("Latest build uncompressed successfully.");
            return true;
        }

        private static bool DownloadLatestBuild()
        {
            Console.WriteLine("Downloading latest build from FTP ...");
            try
            {
                string source = FTPAddress + LatestBuildFilePath + LatestBuildFileName;

                // This example assumes the FTP site uses anonymous logon.
                string username = FTPUserName;
                string password = FTPPassword;


                // Querying download size
                // These lines can be removed if the FTP server gives Progress percentage.
                FtpWebRequest f = WebRequest.Create(source) as FtpWebRequest;
                f.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
                f.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse fr = f.GetResponse() as FtpWebResponse;
                latestBuildDownloadSize = fr.ContentLength;
                fr.Close();


                using (WebClient request = new WebClient())
                {

                    request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
                    resetLatestBuild = new ManualResetEvent(false);
                    request.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(request_DownloadProgressChanged);
                    request.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(request_DownloadFileCompleted);
                    request.DownloadFileAsync(new Uri(source), LatestBuildFileName);
                    //Block till download completes
                    resetLatestBuild.WaitOne();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private static bool UncompressDependencies()
        {
            ExtractZipFiles eZF = new ExtractZipFiles(Path.GetFullPath(SourceFolder), Path.GetFullPath(TargetFolder));
            Console.WriteLine();
            Console.WriteLine("Dependencies uncompressed successfully.");
            return true;
        }

        static void zip1_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            try
            { Console.Write("\r{0} Uncompressing: {1} {2}%", CurrentFileNumber.ToString() + "/" + filesToDownload.Count, currentFileDownload, e.EntriesExtracted * 100 / e.EntriesTotal); }
            catch { }
        }

        private static bool DownloadDependencies()
        {
            try
            {
                using (WebClient request = new WebClient())
                {

                    request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
                    request.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(request_DownloadProgressChanged);
                    request.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(request_DownloadFileCompleted);

                    foreach (FileDetails fd in filesToDownload)
                    {
                        try
                        {
                            CurrentFileNumber++;
                            string source = FTPAddress + DependencyFilePath + fd.fileName;
                            resetDependency = new ManualResetEvent(false);
                            dependencyDownloadSize = fd.fileSize;
                            currentFileDownload = Path.GetFileName(fd.fileName);
                            string destination = Directory.GetCurrentDirectory() + "\\" + DependencyFilePath + fd.fileName;
                            if (!Directory.Exists(Path.GetDirectoryName(destination)))
                                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                            request.DownloadFileAsync(new Uri(source), destination);
                            //Block till download completes
                            resetDependency.WaitOne();
                            Console.WriteLine();
                            fd.status = ExtractFile(fd);
                            
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            //Console.WriteLine("Downloading dependencies from FTP ...");
            //try
            //{
            //    string source = FTPAddress + DependencyFilePath + DependencyFileName;

            //    // This example assumes the FTP site uses anonymous logon.
            //    string username = FTPUserName;
            //    string password = FTPPassword;

            //    // Querying download size
            //    // These lines can be removed if the FTP server gives Progress percentage.
            //    FtpWebRequest f = WebRequest.Create(source) as FtpWebRequest;
            //    f.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
            //    FtpWebResponse fr;

            //    //if (System.IO.File.Exists(DependencyFileName))
            //    //{
            //    //    f.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            //    //    fr = f.GetResponse() as FtpWebResponse;
            //    //    string lastModified = fr.StatusDescription;
            //    //    fr.Close();
            //    //}

            //    f.Method = WebRequestMethods.Ftp.GetFileSize;
            //    fr = f.GetResponse() as FtpWebResponse;
            //    dependencyDownloadSize = fr.ContentLength;
            //    fr.Close();


            //    using (WebClient request = new WebClient())
            //    {
                
            //        request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
            //        resetDependency = new ManualResetEvent(false);
            //        request.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(request_DownloadProgressChanged);
            //        request.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(request_DownloadFileCompleted);
            //        request.DownloadFileAsync(new Uri(source), DependencyFileName);
            //        //Block till download completes
            //        resetDependency.WaitOne();
            //    }

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    return false;
            //}
        }

        private static bool ExtractFile(FileDetails fd)
        {
            try
            {
                using (ZipFile zip1 = ZipFile.Read(Directory.GetCurrentDirectory() +"\\"+ DependencyFilePath + fd.fileName))
                {
                    zip1.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zip1_ExtractProgress);
                    zip1.ExtractAll(TargetFolder+Path.GetDirectoryName(fd.fileName), ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            Console.WriteLine();
            //Console.WriteLine("Dependencies uncompressed successfully.");
            return true;
        }
        
        static void request_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            resetDependency.Set();
            // Console.WriteLine("\nDownload Completed successfully. \n");
        }

        static void request_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write("\r{0} Downloading: {1} {2}%", CurrentFileNumber.ToString() + "/" + filesToDownload.Count,
                currentFileDownload,e.BytesReceived*100 / ( latestBuildDownloadSize!= 0 ? latestBuildDownloadSize : dependencyDownloadSize));
            //Console.Write("\r{0}%", e.ProgressPercentage); This is not used currently as the current FTP server does not send this information while downloading
        }

        private static bool ReadConfiguration()
        {
            try
            {
                FTPAddress = ConfigurationManager.AppSettings["FTPAddress"];
                FTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["FTPPort"]);
                FTPUserName = ConfigurationManager.AppSettings["FTPUserName"];
                FTPPassword = ConfigurationManager.AppSettings["FTPPassword"];
                DependencyFilePath = ConfigurationManager.AppSettings["DependencyFilePath"];
                DependencyFileName = ConfigurationManager.AppSettings["DependencyFileName"];
                LatestBuildFilePath = ConfigurationManager.AppSettings["LatestBuildFilePath"];
                LatestBuildFileName = ConfigurationManager.AppSettings["LatestBuildFileName"];
                FileList = ConfigurationManager.AppSettings["FileList"];
                SourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
                TargetFolder = ConfigurationManager.AppSettings["TargetFolder"];
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return false;
            }
            if (string.IsNullOrEmpty(FileList))
            {
                Console.WriteLine("FileList value not defined in App.config");
                return false;
            } 
            if (FTPPort <= 0)
                FTPPort = 21;
            
            if (string.IsNullOrEmpty(FTPAddress))
            {
                Console.WriteLine("FTPAddress not defined in App.config");
                return false;
            }
            if (string.IsNullOrEmpty(DependencyFileName))
            {
                Console.WriteLine("FileName not defined in App.config");
                return false;
            }
            return true;
        }
    }
}
