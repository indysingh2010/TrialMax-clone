using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Configuration;
using System.Net;
using System.Diagnostics;
using Ionic.Zip;

namespace TmaxDependencyDownloader
{
    public class Program
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
        private static int TotalFilesOnServer = 0;
        private static int TotalFilesToDownload = 0;
        private static int TotalFilesDownloadSuccess = 0;
        private static int TotalFilesDownloadFail = 0;
        private static int TotalFilesUncompressSuccess = 0;
        private static int TotalFilesUncompressFail = 0;
        private static int MaxTotalDownloadTries = 3;
        private static DownladFileStatus CurrentFileDownloadStatus = DownladFileStatus.NothingToDownload;
        private static ManualResetEvent resetDependency;
        private static List<FileDetails> fileListInfoWeb;
        private static string logFile = string.Empty;
        private static string currentFileDownload = string.Empty;
        private static string currentFileUnCompress = string.Empty;
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static DateTime lastProgressUpdate;
        private static TimeSpan timeoutSpan = TimeSpan.FromMilliseconds(30000);

        static Program()
        {
            XmlConfigurator.Configure();
        }

        static void Main(string[] args)
        {
            if (args.Length > 0 && args.FirstOrDefault().Equals("-p"))
            {
                PasswordEncrypter();
                return;
            }
            StartDownloader();

            Console.WriteLine("\nPress enter to exit.");
            Console.Read();
            return;
        }

        private static void PasswordEncrypter()
        {
            Console.WriteLine("Enter Password to encrypt:");
            string pass = "";
            char chr = (char)0;
            const int ENTER = 13;

            do
            {
                chr = System.Console.ReadKey(true).KeyChar;
                if (chr == '\b')
                {
                    if (pass.Length == 0) continue;
                    pass = pass.Remove(pass.Length - 1);
                    System.Console.Write("\b \b");
                }
                else if (chr == ENTER)
                {

                }
                else
                {
                    pass += (char)chr;
                    System.Console.Write("*");
                }
            } while (chr != ENTER);
            SimpleAES encryptDecrypt = new SimpleAES();
            string encryptedPass = encryptDecrypt.EncryptToString(pass);
            Console.WriteLine("Encrypted pass: {0}", encryptedPass);
            System.Console.WriteLine();
        }

        private static void StartDownloader()
        {
            log.Info("===================== Tmax Dependency Downloader Starting =====================");
            log.Info("Reading Config values...");
            if (!ReadConfiguration())
            {
                log.Error("Reading Config values failed.");
                return;
            }
            if (!IntializeFileList())
            {
                log.Error("Initializing FileList failed.");
                return;
            }
            if (!DownloadDependencies())
            {
                log.Error("Downloading file(s) failed.");
            }

            log.Info("========== Summary Start==========");
            log.Info("Total Dependencies           : " + TotalFilesOnServer);
            log.Info("To be Downloaded From Server : " + TotalFilesToDownload);
            log.Info("Already downloaded           : " + (TotalFilesOnServer - TotalFilesToDownload));
            log.Info("Download(s) Success          : " + TotalFilesDownloadSuccess);
            log.Info("Download(s) Failes           : " + TotalFilesDownloadFail);
            log.Info("Files to Uncompress          : " + TotalFilesDownloadSuccess);
            log.Info("Files to Uncompress Success  : " + TotalFilesUncompressSuccess);
            log.Info("Files to Uncompress Failes   : " + TotalFilesUncompressFail);
            log.Info("========== Summary End==========");
            log.Info("===================== Tmax Dependency Downloader Ending =====================");
        }
        
        private static bool ReadConfiguration()
        {
            try
            {
                FTPAddress = ConfigurationManager.AppSettings["FTPAddress"];
                FTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["FTPPort"]);
                FTPUserName = ConfigurationManager.AppSettings["FTPUserName"];
                FTPPassword = new SimpleAES().DecryptString(ConfigurationManager.AppSettings["FTPPassword"]);
                DependencyFilePath = ConfigurationManager.AppSettings["DependencyFilePath"];
                DependencyFileName = ConfigurationManager.AppSettings["DependencyFileName"];
                LatestBuildFilePath = ConfigurationManager.AppSettings["LatestBuildFilePath"];
                LatestBuildFileName = ConfigurationManager.AppSettings["LatestBuildFileName"];
                FileList = ConfigurationManager.AppSettings["FileList"];
                SourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
                TargetFolder = ConfigurationManager.AppSettings["TargetFolder"];
                logFile = ConfigurationManager.AppSettings["logFile"];
            }
            catch (Exception Ex)
            {
                log.Fatal(Ex.ToString());
                return false;
            }
            if (string.IsNullOrEmpty(FileList))
            {
                log.Error("FileList value not defined in App.config");
                return false;
            }
            if (FTPPort <= 0)
                FTPPort = 21;

            if (string.IsNullOrEmpty(FTPAddress))
            {
                log.Error("FTPAddress not defined in App.config");
                return false;
            }
            if (string.IsNullOrEmpty(DependencyFileName))
            {
                log.Error("FileName not defined in App.config");
                return false;
            }
            try
            {
                if (!Directory.Exists(SourceFolder))
                    Directory.CreateDirectory(SourceFolder);
                else
                    File.Delete(SourceFolder + FileList); 
            }
            catch { }
            return true;
        }

        private static bool IntializeFileList()
        {
            try
            {
                fileListInfoWeb = new List<FileDetails>();
                using (WebClient request = new WebClient())
                {

                    request.Credentials = new NetworkCredential(FTPUserName, FTPPassword);
                    request.DownloadFile(new Uri(FTPAddress + DependencyFilePath + SourceFolder + FileList), SourceFolder + FileList);
                }
                if (!System.IO.File.Exists(SourceFolder + FileList))
                {
                    return false;
                }
                using (var sr = File.OpenText(SourceFolder + FileList))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var fields = line.Split(',');
                        FileDetails fd = new FileDetails(fields[0], Convert.ToInt64(fields[1]), Convert.ToInt32(fields[2]));
                        string outputFile = SourceFolder + fd.fileName;
                        if (File.Exists(outputFile))
                        {
                            fd.fileExists = true;
                            if (fd.fileSize == new FileInfo(outputFile).Length)
                            {
                                fd.fileSizeMatch = true;
                                if (fd.checkSum == GetFileCheckSum(outputFile))
                                {
                                    fd.downloadStatus = true;
                                    fd.checkSumMatch = true;
                                }
                            }
                        }
                        fileListInfoWeb.Add(fd);
                    }
                }
                TotalFilesOnServer = fileListInfoWeb.Count;
                return true;
            }
            catch (Exception ex)
            {
                log.Fatal(ex.ToString());
                log.Error(ex.Message);
                return false;
            }
        }

        private static int GetFileCheckSum(string outputFile)
        {
            int checkSum = 0;
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "sum.exe",
                        Arguments = "\"" + outputFile + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                StreamReader sOutput = proc.StandardOutput;
                StreamReader sError = proc.StandardError;
                string output = sOutput.ReadLine();
                proc.WaitForExit();
                proc.Close();
                checkSum = Convert.ToInt32(output.Split(' ').FirstOrDefault());
            }
            catch (Exception ex)
            {
                log.Fatal(ex.ToString());
            }
            return checkSum;
        }

        private static bool ExtractFile(FileDetails fd)
        {
            try
            {
                using (ZipFile zip1 = ZipFile.Read(@SourceFolder + fd.fileName))
                {
                    zip1.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zip1_ExtractProgress);
                    zip1.ExtractAll(TargetFolder + Path.GetDirectoryName(fd.fileName), ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex.ToString());
                TotalFilesUncompressFail++;
                return false;
            }
            TotalFilesUncompressSuccess++;
            return true;
        }

        private static bool DownloadDependencies()
        {
            try
            {
                try
                {
                    foreach (string f in Directory.GetFiles(@SourceFolder + "trash//"))
                        File.Delete(f);
                }
                catch { }
                TotalFilesToDownload = fileListInfoWeb.Count(x => x.downloadStatus == false);
                bool quit = false;
                foreach (FileDetails fd in fileListInfoWeb)
                {
                    if (quit) return false;
                    try
                    {
                        if (fd.fileExists)
                        {
                            if (fd.downloadStatus)
                            {
                                log.Info(fd.fileName + " already processed. File ingored.");
                                continue;
                            }
                            log.Info("Local copy of " + fd.fileName + " is different then on the server. Downloading again.");
                        }
                        CurrentFileNumber++;
                        int totalTries = 0;
                        while (totalTries < MaxTotalDownloadTries)
                        {
                            CurrentFileDownloadStatus = DownladFileStatus.Failed;
                            string source = FTPAddress + DependencyFilePath + @SourceFolder + fd.fileName;
                            resetDependency = new ManualResetEvent(false);
                            dependencyDownloadSize = fd.fileSize;
                            currentFileDownload = Path.GetFileName(fd.fileName);
                            string destination = @SourceFolder + fd.fileName;
                            if (File.Exists(destination))
                            {
                                try
                                {
                                    File.Delete(destination);
                                }
                                catch
                                {
                                    if (!Directory.Exists(@SourceFolder + "trash//"))
                                        Directory.CreateDirectory(@SourceFolder + "trash//");
                                    File.Move(destination, @SourceFolder + "trash//" + DateTime.Now.Ticks + Path.GetExtension(destination));
                                }                                
                            }
                            DownloadFile(source, destination);
                            if (File.Exists(destination))
                            {
                                if (fd.checkSum == GetFileCheckSum(destination) && fd.fileSize == new FileInfo(destination).Length)
                                {
                                    log.Info(fd.fileName + " downloaded successfully.");
                                    TotalFilesDownloadSuccess++;
                                    fd.downloadStatus = true;
                                    break;
                                }
                                else
                                {
                                    try
                                    { File.Delete(destination); }
                                    catch { }
                                    totalTries++;
                                    log.Error(fd.fileName + " downloaded but CHECKSUM/SIZE mismatch. Retry attempt " + totalTries.ToString() + "/" + MaxTotalDownloadTries + "...");
                                    continue;
                                }
                            }
                            else
                            {
                                try
                                { File.Delete(destination); }
                                catch { }
                                totalTries++;
                                log.Error("Download failed for file: " + fd.fileName + ". Retry attempt " + totalTries.ToString() + "/" + MaxTotalDownloadTries + "...");
                                continue;
                            }
                        }
                        if (fd.downloadStatus == true)
                        {
                            currentFileUnCompress = Path.GetFileName(fd.fileName);
                            fd.unCompressStatus = ExtractFile(fd);
                            Console.WriteLine();
                            if (!fd.unCompressStatus)
                                log.Info("Uncompressing " + fd.fileName + " failed.");
                            else
                                log.Info(fd.fileName + " uncompressed successfully.");
                        }
                        else
                        {
                            TotalFilesDownloadFail++;
                            while (true)
                            {
                                Console.Write("Previous attempts to downloading has failed consecutively. Do you want to try again? (Y/N): ");
                                string userEntered = Console.ReadLine();
                                if (userEntered.ToLower().Equals("n"))
                                {
                                    quit = true;
                                    break;
                                }
                                else if (userEntered.ToLower().Equals("y"))
                                {
                                    break;
                                }
                                Console.WriteLine("Invalid input.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Fatal(ex.ToString());
                        TotalFilesDownloadFail++;
                    }
                }
                return true;
            }
            catch (WebException webEx)
            {
                log.Fatal(webEx.ToString());
                log.Error(webEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                log.Fatal(ex.ToString());
                log.Error(ex.Message);
                return false;
            }
        }

        static void request_Disposed(object sender, EventArgs e)
        {
            resetDependency.Set();
        }

        static void request_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            CurrentFileDownloadStatus = DownladFileStatus.Failed;
            if (e.Error == null)
            {
                CurrentFileDownloadStatus = DownladFileStatus.Success;
                try
                {
                    Console.Write("\r{0} Downloading: {1} {2}%", CurrentFileNumber.ToString() + "/" + TotalFilesToDownload,
                    currentFileDownload, 100);
                }
                catch { }
            }
            else
            {
                try
                {
                    log.Fatal(e.Error.ToString());
                    File.Delete(SourceFolder+currentFileDownload);
                }
                catch { }
            }
            resetDependency.Set();
        }

        private static void request_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lastProgressUpdate = DateTime.Now;
            if (CurrentFileDownloadStatus != DownladFileStatus.Success)
                Console.Write("\r{0} Downloading: {1} {2}%", CurrentFileNumber.ToString() + "/" + TotalFilesToDownload,
                currentFileDownload, e.BytesReceived * 100 / (latestBuildDownloadSize != 0 ? latestBuildDownloadSize : dependencyDownloadSize));
        }

        private static void zip1_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            try
            { Console.Write("\r{0} Uncompressing: {1} {2}%", CurrentFileNumber.ToString() + "/" + TotalFilesToDownload, currentFileUnCompress, e.BytesTransferred * 100 / e.TotalBytesToTransfer); }
            catch { }
        }

        private static void DownloadFile(string source, string destination)
        {
            try
            {
                string paramerters = "--ftp-user=\"" + FTPUserName + "\" --ftp-password=\"" + FTPPassword + "\" --tries=1 --timeout=" + timeoutSpan.Seconds + " --show-progress -q -O \"" + destination + "\" \"" + source.Replace("\\.", "") + "\"";
                ProcessStartInfo oInfo = new ProcessStartInfo("wget.exe", paramerters);
                oInfo.UseShellExecute = false;
                oInfo.CreateNoWindow = true;
                oInfo.RedirectStandardOutput = true;
                oInfo.RedirectStandardError = true;

                using (Process proc = Process.Start(oInfo))
                {

                    //Hook up events
                    proc.EnableRaisingEvents = true;
                    proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);

                    // allow for reading asynhcronous Output
                    proc.BeginErrorReadLine();

                    // Blocking untilt the encoding done
                    proc.WaitForExit();

                    proc.Close();
                }
                if (CurrentFileDownloadStatus != DownladFileStatus.Success)
                {
                    if (CurrentFileDownloadStatus == DownladFileStatus.Downloading)
                        Console.WriteLine();
                    log.Error("Connection timed out.");
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex.ToString());
            }
        }

        static void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lastProgressUpdate = DateTime.Now;
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }
            string[] info = e.Data.Split(' ');
            string percent = info.Where(x=>x.Contains("%")).FirstOrDefault();
            string eta = info.Where(x => x.Contains("s")).FirstOrDefault();
            if (string.IsNullOrEmpty(percent))
                return;
            if (CurrentFileDownloadStatus != DownladFileStatus.Success && info.Length >= 3)
                if(percent == "100%")
                {
                    CurrentFileDownloadStatus = DownladFileStatus.Success;
                    Console.Write("\r{0} Downloading: {1} {2}        \n", CurrentFileNumber.ToString() + "/" + TotalFilesToDownload,
                    currentFileDownload, percent);
                }
                else
                {
                    eta = eta.Split('=').LastOrDefault();
                    CurrentFileDownloadStatus = DownladFileStatus.Downloading;
                    Console.Write("\r{0} Downloading: {1} {2} {3}       ", CurrentFileNumber.ToString() + "/" + TotalFilesToDownload,
                    currentFileDownload, percent, eta);
                }

        }

        static void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }

    public class SimpleAES
    {
        // Change these keys
        private byte[] Key = { 15, 157, 249, 33, 66, 174, 25, 216, 243, 225, 0, 103, 20, 193, 217, 1, 135, 97, 14, 92, 220, 179, 89, 43, 25, 198, 211, 218, 176, 44, 133, 52 };

        private byte[] Vector = { 182, 46, 217, 94, 195, 143, 254, 211, 57, 84, 23, 79, 194, 219, 240, 89 };


        private ICryptoTransform EncryptorTransform, DecryptorTransform;
        private System.Text.UTF8Encoding UTFEncoder;

        public SimpleAES()
        {
            //This is our encryption method
            RijndaelManaged rm = new RijndaelManaged();

            //Create an encryptor and a decryptor using our encryption method, key, and vector.
            EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
            DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);

            //Used to translate bytes to text and vice versa
            UTFEncoder = new System.Text.UTF8Encoding();
        }

        /// -------------- Two Utility Methods (not used but may be useful) -----------
        /// Generates an encryption key.
        static public byte[] GenerateEncryptionKey()
        {
            //Generate a Key.
            RijndaelManaged rm = new RijndaelManaged();
            rm.GenerateKey();
            return rm.Key;
        }

        /// Generates a unique encryption vector
        static public byte[] GenerateEncryptionVector()
        {
            //Generate a Vector
            RijndaelManaged rm = new RijndaelManaged();
            rm.GenerateIV();
            return rm.IV;
        }


        /// ----------- The commonly used methods ------------------------------    
        /// Encrypt some text and return a string suitable for passing in a URL.
        public string EncryptToString(string TextValue)
        {
            return ByteArrToString(Encrypt(TextValue));
        }

        /// Encrypt some text and return an encrypted byte array.
        public byte[] Encrypt(string TextValue)
        {
            //Translates our text value into a byte array.
            Byte[] bytes = UTFEncoder.GetBytes(TextValue);

            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            /*
             * We will have to write the unencrypted bytes to the stream,
             * then read the encrypted result back from the stream.
             */
            #region Write the decrypted value to the encryption stream
            CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            #endregion

            #region Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            #endregion

            //Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        /// The other side: Decryption methods
        public string DecryptString(string EncryptedString)
        {
            return Decrypt(StrToByteArray(EncryptedString));
        }

        /// Decryption when working with byte arrays.    
        public string Decrypt(byte[] EncryptedValue)
        {
            #region Write the encrypted value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
            decryptStream.FlushFinalBlock();
            #endregion

            #region Read the decrypted value from the stream.
            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();
            #endregion
            return UTFEncoder.GetString(decryptedBytes);
        }

        /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //      return encoding.GetBytes(str);
        // However, this results in character values that cannot be passed in a URL.  So, instead, I just
        // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        public byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }

        // Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
        //      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        //      return enc.GetString(byteArr);    
        public string ByteArrToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                val = byteArr[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }
    }

    enum DownladFileStatus
    {
        NothingToDownload,
        Success,
        Failed,
        Downloading
    }

    public class FileDetails
    {
        public string fileName { get; set; }
        public int checkSum { get; set; }
        public long fileSize { get; set; }
        public bool downloadStatus { get; set; }
        public bool unCompressStatus { get; set; }
        public bool checkSumMatch { get; set; }
        public bool fileSizeMatch { get; set; }
        public bool fileExists { get; set; }

        public FileDetails(string _fileName, long _fileSize, int _checkSum)
        {
            fileName = _fileName;
            checkSum = _checkSum;
            fileSize = _fileSize;
            downloadStatus = false;
            unCompressStatus = false;
            checkSumMatch = false;
            fileSizeMatch = false;
            fileExists = false;
        }

        public static bool operator ==(FileDetails c1, FileDetails c2)
        {
            return (c1.fileName.Equals(c2.fileName) && c1.fileSize == c2.fileSize && c1.checkSum.Equals(c2.checkSum));
        }

        public static bool operator !=(FileDetails c1, FileDetails c2)
        {
            return (!c1.fileName.Equals(c2.fileName) || c1.fileSize != c2.fileSize || !c1.checkSum.Equals(c2.checkSum));
        }
    }
}
