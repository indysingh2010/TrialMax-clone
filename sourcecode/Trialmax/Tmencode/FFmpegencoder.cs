using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FTI.Trialmax.Encode
{
    /// <summary>This class is used to as a wrapper for FFMPEG Encoder services</summary>
    public class CFFMpegEncoder
    {
        public enum SupportedExportFormats { WMV,MPEG,MPG,MP4,AVI,MOV,M2V };
        
        // this file should in the Bin folder
        private string m_FFMpegEncoder = @"ffmpeg.exe";

        // destination file property
        private string m_strFileSpec = "";

        // bit rate on which the video will be encoded
        private string m_strBitrate = "768k";

        // used to stroe end time of current video that is in encoding
        private long m_lEndTime = 0;

        /// <summary>Local member bound to Status property</summary>
        private CFEncoderStatus m_encodingStatus = null;

        /// <summary>Local member to store status of active source while encoding is taking place</summary>
        private string m_strSourceStatus;
        private string m_strSourceFile;

        private long m_lCompleted = 0;
        private bool m_bIsEncodingCompleted = false;
        private bool m_bIsEncodingInProgress = false;

        private bool m_bShowCancel = true;
        private bool m_bIsFinalizing = false;
        private bool m_bIsCancelled = false;

        // local memeber bound to hold the sources that will be encoded in a single file
        public List<CFFMpegSource> Sources;


        public delegate bool EncoderStatusHandler(object sender, string strStatus);

        // event to notify the changing in encoding status
        public event EncoderStatusHandler EncoderStatusUpdate;

        public CFFMpegEncoder() { }

        public bool ShowCancel
        {
            get { return m_bShowCancel; }
            set { m_bShowCancel = value; }
        }

        public bool Initialize(string strSourceFile)
        {
            bool bSuccessful = false;
            m_lCompleted = 0;
            m_strSourceFile = string.Empty;
            m_lEndTime = 0;
            m_bIsCancelled = false;
            m_bIsFinalizing = false;

            try
            {
                //	Does the user want to encode a new file?
                if ((strSourceFile != null) && (strSourceFile.Length > 0))
                {
                    //	Set the name of the encoder's output file
                    string outputFolder = string.Empty;

                    if(strSourceFile.Contains("\\"))
                        outputFolder = strSourceFile.Substring(0,strSourceFile.LastIndexOf("\\"));

                    m_strFileSpec = strSourceFile;
                }

                bSuccessful = true;

            }
            catch (Exception ex)
            { }
            return bSuccessful;
        }

        public bool AddSource(string strDestinationFile, string strSourceFileName, double dStartTime, double dEndTime)
        {
            if (this.Sources == null) 
                this.Sources = new List<CFFMpegSource>();

            this.Sources.Add(new CFFMpegSource(strSourceFileName,strDestinationFile,dStartTime,dEndTime));
            return true;
            
        }

        public void Clear()
        {
            try
            {
                //	Destroy the status form
                if (m_encodingStatus != null)
                {
                    if (m_encodingStatus.IsDisposed == false)
                        m_encodingStatus.Dispose();
                    m_encodingStatus = null;
                }
                
                
            }
            catch
            {

            }
        }

        public void Cancel()
        {
            m_bIsCancelled = true;
        }

        private bool GetCancelled()
        {
            if (m_bIsCancelled == false)
            {
                System.Windows.Forms.Application.DoEvents();
                if (m_encodingStatus != null)
                {
                    // this property will be true if the use cancel's the encoding from EncoderStatus Form
                    m_bIsCancelled = m_encodingStatus.Cancelled;
                }
            }
            
            return m_bIsCancelled;
        }

        public bool Encode() 
        {
            if (!CreateStatusForm(m_strFileSpec) || !System.IO.File.Exists(m_FFMpegEncoder))
                return false;

            while (!m_bIsEncodingCompleted)
            {
                if (GetCancelled())
                {
                    SetStatus("Encoding Cancelled", 0);                    
                    return false;
                }

                if (Sources != null && Sources.Count() > 0 && Sources.Count() > m_lCompleted)
                {
                    if (!m_bIsEncodingInProgress)
                    {
                        PrepareAndStartEncode(Sources);
                    }                    
                }
                else
                    m_bIsEncodingCompleted = true;

                //System.Threading.Thread.Sleep(500);
            }

            // merging is only in the case of more than one script/source
            if (m_lCompleted > 0 && Sources != null && Sources.Count() > 1 && !GetCancelled())
            {
                m_bIsFinalizing = true;                
                MergeFile();
                while (m_bIsFinalizing) 
                {
                    if (GetCancelled())
                    {
                        SetStatus("Finalizing Cancelled", 0);                        
                        return false;
                    }

                    //System.Threading.Thread.Sleep(500);
                }
            }
            return true;
        }


        private void RunProcess(object param)
        {
            try
            {                            
                string paramerters = (string)param;
                ProcessStartInfo oInfo = new ProcessStartInfo(m_FFMpegEncoder, paramerters);                
                oInfo.UseShellExecute = false;
                oInfo.CreateNoWindow = true;
                oInfo.RedirectStandardOutput = true;
                oInfo.RedirectStandardError = true;                                

                using (Process proc = Process.Start(oInfo))
                {

                    //Hook up events
                    proc.EnableRaisingEvents = true;
                    proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
                    proc.Exited += new EventHandler(proc_Exited);

                    // allow for reading asynhcronous Output
                    proc.BeginErrorReadLine();

                    // Blocking untilt the encoding done
                    proc.WaitForExit();
                                        
                    proc.Close();
                    proc.Dispose();
                    
                    // mark that encoding is done and no more in progress now
                    m_bIsEncodingInProgress = false;

                    // if it was merging
                    if (m_bIsFinalizing)
                    {
                        // if merging is complete than mark it false so above loop can break
                        m_bIsFinalizing = false;

                        // delete the temp files when merging complete
                        DeleteFiles();
                    }
                    else // if it was encoding than increment the complete counter
                        m_lCompleted++;
                }
                return;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Exception in Process " + Environment.NewLine + ex.Message);
                return;
            }
        }

        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {            
            // output metadata of process during encoding
            if (e.Data != null && e.Data.StartsWith("frame"))
            {
                //Split the raw string
                string[] parts = e.Data.Split(new string[] { " ", "=" }, StringSplitOptions.RemoveEmptyEntries);

                //Parse current frame
                long lCurrentFrame = 0L;
                Int64.TryParse(parts[1], out lCurrentFrame);                

                //Parse FPS
                short sFPS = 0;
                Int16.TryParse(parts[3], out sFPS);
                
                //Calculate percentage
                double dCurrentFrame = (double)lCurrentFrame;

                // get the duration encoded from the metadata output of process
                TimeSpan durationEncoded = TimeSpan.FromSeconds(0);
                TimeSpan.TryParse(parts[9], out durationEncoded);
                int encodedDuration = getPercentage(durationEncoded);

                // set the status
                SetStatus(m_bIsFinalizing == true ? "Finalizing" : "Encoding " + m_strSourceFile.Substring(m_strSourceFile.LastIndexOf("\\")+1), encodedDuration);
                
                if (EncoderStatusUpdate != null)
                {
                    // this event return true if the encoding is aborted by User from ExportStatus Form
                    if (!EncoderStatusUpdate(this, m_bIsFinalizing == true ? "Finalizing" : "Encoding") || 
                        GetCancelled())
                    {
                        Process proc = sender as Process;
                        if (proc != null)
                        {
                            try
                            {
                                Cancel();
                                SetStatus("Encoding Cancelled", 0);
                                if ((m_encodingStatus != null) && (m_encodingStatus.IsDisposed == false))
                                {
                                    m_encodingStatus.Cancelled = true;
                                    m_encodingStatus.Status = "Cancelled";
                                }
                                                              
                                EncoderStatusUpdate(this, "Cancelled");
                                m_lCompleted++; // increment so current file can be deleted                        
                                proc.Kill();
                                proc.Close();                                
                                proc.Dispose();                                
                                //System.Threading.Thread.CurrentThread.Abort();
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                }
            }
        }

        void proc_Exited(object sender, EventArgs e)
        {           
            // handle event when process is exited                        
            if (GetCancelled()) 
            {
                DeleteFiles();
                DeleteEncodedFile();
            }
        }

        private int getPercentage(TimeSpan initialProgress)
        {
            TimeSpan totalTime = TimeSpan.FromSeconds(m_lEndTime);
            double prgress = (initialProgress.TotalMilliseconds / totalTime.TotalMilliseconds);
            return (int)(prgress * 100);
        }

        private bool CreateStatusForm(string strFileSpec)
        {
            try
            {
                //	Make sure the previous instance is disposed
                if (m_encodingStatus != null)
                {
                    if (m_encodingStatus.IsDisposed == false)
                        m_encodingStatus.Dispose();
                    m_encodingStatus = null;
                }

                //	Create a new instance
                m_encodingStatus = new CFEncoderStatus();

                // show the cancel button on Encoder Status Form
                m_encodingStatus.ShowCancel = m_bShowCancel;

                //	Set the initial status message
                m_encodingStatus.FileSpec = strFileSpec;
                SetStatus("Initializing encoder ...", 0);

                // Show the encoder status form
                m_encodingStatus.Show();

            }
            catch (System.Exception Ex)
            {
                //m_tmaxEventSource.FireError(this, "CreateStatusForm", m_tmaxErrorBuilder.Message(ERROR_CREATE_STATUS_FORM_EX, strFileSpec), Ex);
                m_encodingStatus = null;
            }

            return (m_encodingStatus != null);

        }

        private void SetStatus(string strStatus, int iProgress)
        {
            try
            {                
                if ((m_encodingStatus != null) && (m_encodingStatus.IsDisposed == false))
                {
                    m_encodingStatus.Status = strStatus;
                    m_encodingStatus.SetProgress(iProgress);
                    m_encodingStatus.Refresh();

                    
                }

            }
            catch
            {
            }

        }

        private void PrepareAndStartEncode(List<CFFMpegSource> Source)
        {
            // if source is available and not cancelled by the user
            if (Source != null && Source.Count > 0 && !GetCancelled())
            {
                // We are using FFMPEG Encoder to encode videos, so it is limitation in concat or merge different scripts
                // in single video to mutiple formats it is only possible in mpg
                // so if there are multiple scripts than we first cut and encode it in mpg
                // so we can add or concat them at last. (as we have done in MergeFile() method)
                string param = string.Empty;
                CFFMpegSource source = Sources[(int)m_lCompleted];

                // if there are single script so we do not need to merge, we only need to encode directly
                if (Source.Count == 1) 
                {
                    // encoding parameters
                    param = "-i \"" + source.m_strSourceFile + "\" -ss " + TimeSpan.FromSeconds(source.m_dStartTime) + " -t " + TimeSpan.FromSeconds(source.m_dEndTime) + " -b:v " + m_strBitrate + " " + getCodec(m_strFileSpec) + " \"" + m_strFileSpec + "\"";
                }
                else
                {
                    string destinationFileName = m_strFileSpec;

                    // get the actual destination filename and change it to convert in mpg first
                    string extension = m_strFileSpec.Substring(m_strFileSpec.LastIndexOf("."));
                    destinationFileName = destinationFileName.Replace(extension, "");
                    destinationFileName = destinationFileName + "_" + m_lCompleted + ".mpg";

                    // encoding parameters
                    param = "-i \"" + source.m_strSourceFile + "\" -ss " + TimeSpan.FromSeconds(source.m_dStartTime) + " -t " + TimeSpan.FromSeconds(source.m_dEndTime) + " " + getCodec(destinationFileName) + " \"" + destinationFileName + "\"";
                }

                // endtime of current script
                m_lEndTime = (long)source.m_dEndTime;

                // mark that encoding is in progress
                m_bIsEncodingInProgress = true;

                // current source file
                m_strSourceFile = source.m_strSourceFile;

                // start encoding in a seperate thread, so we can cancel the encoding in UI thread
                System.Threading.ParameterizedThreadStart threadStart = new System.Threading.ParameterizedThreadStart(RunProcess);
                System.Threading.Thread thread = new System.Threading.Thread(threadStart);
                thread.Start(param);                
            }
        }

        private void MergeFile()
        {
            string inputFiles = string.Empty;

            // get the mpg files that is encoded to merge in a single file
            // m_lCompleted is a property that tell us how many number of files encoded for merging
            for (int f = 0; f < m_lCompleted; f++)
            {
                string filename = m_strFileSpec;
                string extension = m_strFileSpec.Substring(m_strFileSpec.LastIndexOf("."));
                filename = filename.Replace(extension, "");
                filename = filename + "_" + f + ".mpg";

                // check if the encoded file exist that pick it and add in the concat list
                // after each file name append the pipe "|" sign which is the requirment for FFMPEG Encoder
                if (System.IO.File.Exists(filename))
                    inputFiles += filename + "|";
            }

            // after appending all filenames remove the last pipe sign it is appended extra at the end
            if (inputFiles.Length > 0 && inputFiles.Contains("|"))
            {
                inputFiles = inputFiles.Substring(0, inputFiles.LastIndexOf("|"));
            }

            // if we got some files to merge
            if (!string.IsNullOrEmpty(inputFiles))
            {
                // encoding and merging parameters
                string concatParam = @"-i ""concat:" + inputFiles + "\" -b:v " + m_strBitrate + " " + getCodec(m_strFileSpec) +" \"" + m_strFileSpec + "\"";                
                
                // mark that we start finalizing
                m_bIsFinalizing = true;

                // start encoding in a seperate thread, so we can cancel the encoding in UI thread
                System.Threading.ParameterizedThreadStart threadStart = new System.Threading.ParameterizedThreadStart(RunProcess);
                System.Threading.Thread thread = new System.Threading.Thread(threadStart);
                thread.Start(concatParam);
                
            }
        }

        private void DeleteFiles()
        {            
            for (int f = 0; f < m_lCompleted; f++)
            {
                string filename = m_strFileSpec;
                string extension = m_strFileSpec.Substring(m_strFileSpec.LastIndexOf("."));
                filename = filename.Replace(extension, "");
                filename = filename + "_" + f + ".mpg";

                try
                {
                    if (System.IO.File.Exists(filename))
                        System.IO.File.Delete(filename);
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(filename + " unable to delete" + Environment.NewLine + ex.Message);
                }
            }
        }

        private void DeleteEncodedFile()
        {
            string filename = m_strFileSpec;

            try
            {
                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);
            }
            catch (Exception ex)
            {
                
            }            
        }

        private string getCodec(string sourceFile)
        {
            string codec = string.Empty;
            string extension = sourceFile.Substring(sourceFile.LastIndexOf("."));
                        

            if (extension.ToUpper().Contains(
                Convert.ToString(SupportedExportFormats.WMV))
                )
            {
                codec = "-vcodec msmpeg4 -acodec wmav2";
            }
            else if (extension.ToUpper().Contains(
                Convert.ToString(SupportedExportFormats.MPEG)) ||
                extension.ToUpper().Contains(
                Convert.ToString(SupportedExportFormats.MPG))
                )
            {
                codec = "-qscale:v 1";
            }            
            else if (
               (extension.ToUpper().Contains(
               Convert.ToString(SupportedExportFormats.MP4))) ||
               (extension.ToUpper().Contains(
               Convert.ToString(SupportedExportFormats.AVI))) ||
               (extension.ToUpper().Contains(
               Convert.ToString(SupportedExportFormats.MOV))) ||
                (extension.ToUpper().Contains(
               Convert.ToString(SupportedExportFormats.M2V))) 
                )
            {
                // right now 
                codec = "";//"-acodec libfaac -vcodec mpeg4";
            }
            else 
            {
                codec = "";
            }

            return codec;
        }
    }
}
