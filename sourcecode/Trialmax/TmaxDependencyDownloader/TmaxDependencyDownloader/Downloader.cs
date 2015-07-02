using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TmaxDependencyDownloader
{
    public class Downloader
    {
        #region Private Members
        private string ftpUserName = string.Empty;
        private string ftpPassword = string.Empty;
        private string ftpAddress = string.Empty;
        private short maxTries = 0;
        private TimeSpan timeoutSpan;
        #endregion

        #region Private Methods
        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (notifyDownloadProgress != null)
                notifyDownloadProgress(sender, e);
        }
        #endregion

        #region Public Methods
        public Downloader(string _ftpAddress, string _ftpUserName, string _ftpPassword, TimeSpan _timeoutSpan, short _maxTries)
        {
            ftpAddress = _ftpAddress;
            ftpUserName = _ftpUserName;
            ftpPassword = _ftpPassword;
            timeoutSpan = _timeoutSpan;
            maxTries = _maxTries;
        }

        public bool DownloadFile(string source, string destination)
        {
            try
            {
                source = ftpAddress + source;
                string paramerters = "--ftp-user=\"" + ftpUserName + "\" --ftp-password=\"" + ftpPassword + "\" --tries=" + maxTries.ToString() + " --timeout=" + timeoutSpan.Seconds + " --show-progress -q -O \"" + destination + "\" \"" + source.Replace("\\.", "") + "\"";
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
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.LogFatal(ex);
                return false;
            }
        }
        #endregion

        #region Event Handlers
        public event EventHandler<DataReceivedEventArgs> notifyDownloadProgress;
        #endregion
    }
}
