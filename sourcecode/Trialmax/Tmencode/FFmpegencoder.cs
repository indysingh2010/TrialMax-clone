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
        
        // this file should be in the Bin folder
        private string m_FFMpegEncoder = @"ffmpeg.exe";

        // destination file property
        private string m_strFileSpec = "";

        // this property hold the text filename for encoder
        private string m_strInputFiles = "inputFiles.txt";

        // bit rate on which the video will be encoded
        // yet we have fixed it to 768k
        private string m_strBitrate = "768k";

        // used to store end time of current video that is in encoding
        private long m_lEndTime = 0;

        // used to store total end time of all videos that is going to be encoded
        private long m_lEndTimeTotal = 0;

        // used to store total end time of all videos that is going to be encoded
        private double m_lProgressTimeTotal = 0;

        /// <summary>Local member bound to Status property</summary>
        private CFEncoderStatus m_encodingStatus = null;

        /// <summary>Local member to store status of active source while encoding is taking place</summary>
        private string m_strSourceStatus;

        /// <summary>Local member to store current source while encoding is taking place</summary>
        private string m_strSourceFile;

        // number of files complete encoding
        private volatile int m_lCompleted = 0;

        // this property define is the encoding complete
        private bool m_bIsEncodingCompleted = false;

        // this property define is the encoding in progress
        private bool m_bIsEncodingInProgress = false;

        // this property will show/hide cancel button on status form        
        private bool m_bShowCancel = true;

        // this peroperty define is merging happening 
        private bool m_bIsFinalizing = false;

        // this peroperty define is encoding or merging cancelled 
        private bool m_bIsCancelled = false;

        private volatile int m_nProcessStarted = 0;

        private int m_nMaximumEncodingThreads = 5;

        public bool m_bIsMpeg2Selected = false;

        // local memeber bound to hold the sources that will be encoded in a single file
        public List<CFFMpegSource> Sources;

        // delegate for encoding status
        public delegate bool EncoderStatusHandler(object sender, string strStatus);

        // event to notify the changing in encoding status
        public event EncoderStatusHandler EncoderStatusUpdate;

        // contstructor
        public CFFMpegEncoder() { }

        // Show or hide cancel button on status form
        public bool ShowCancel
        {
            get { return m_bShowCancel; }
            set { m_bShowCancel = value; }
        }

        // this method will initialize and prepare the initial requirment for encoder
        /// <summary>This method is called to initialize the object</summary>
        /// <param name="strFileSpec">Fully qualified path to the output file to be encoded</param>
        /// <returns>true if successful</returns>
        public bool Initialize(string strSourceFile)
        {
            // reset the properties
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

        /// <summary>Called to add a new source descriptor to the encoder project</summary>
        /// <param name="strName">The unique name used to identify the group</param>
        /// <param name="strFileSpec">The fully qualified path to the source file</param>
        /// <param name="dStart">The start time in seconds</param>
        /// <param name="dStop">The stop time in seconds</param>
        /// <returns>true if successful</returns>
        public bool AddSource(string strDestinationFile, string strSourceFileName, double dStartTime, double dEndTime)
        {
            if (this.Sources == null) 
                this.Sources = new List<CFFMpegSource>();

            this.Sources.Add(new CFFMpegSource(strSourceFileName,strDestinationFile,dStartTime,dEndTime));
            return true;
            
        }

        /// <summary>This method is called to clear the existing interfaces and reset the object</summary>
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
            { }
        }

        /// <summary>This method is called to cancel the encoding operation</summary>
        public void Cancel()
        {
            m_bIsCancelled = true;
        }

        /// <summary>Called to check the flag to see if the user has cancelled</summary>
        /// <returns>true if cancelled</returns>
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

        /// <summary>This method is called to encode the specified source groups</summary>
        /// <method name="GetCancelled()">Flag to indicate if the operation was cancelled by the user</param>
        /// <returns>true if successful</returns>
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
                    // if initiated threads are less than the maximum threads.
                    if (m_nProcessStarted < m_nMaximumEncodingThreads)
                    {
                        if (m_lCompleted == 0)
                            CalculateTotalLength();

                        PrepareAndStartEncode(Sources);
                    }
                }
                else
                {
                    if(Sources.Count() == NumberOfEncodedFilesFound() && !m_bIsEncodingInProgress)
                        m_bIsEncodingCompleted = true;
                }

                System.Threading.Thread.Sleep(100);
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

                    System.Threading.Thread.Sleep(100);
                }
            }
            return true;
        }

        /// <summary>This method is called to start the process of encoding</summary>
        /// <param name="param">The parameters for encoding</param>
        /// <returns>true if successful</returns>
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
                    {
                        //m_lCompleted++;

                        // calculate the total initial progress of encoded video
                        CalculateTotalEncodingProgress();
                    }
                }
                return;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Exception in Process " + Environment.NewLine + ex.Message);
                return;
            }
        }

        /// <summary>This method is an event handler of data recieved from the encoding process</summary>
        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
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

                // mark that encoding is in progress
                m_bIsEncodingInProgress = true;

                // set the status
                SetStatus(m_bIsFinalizing == true ? "Finalizing" : "Encoding " + m_strSourceFile.Substring(m_strSourceFile.LastIndexOf("\\") + 1), encodedDuration);
                
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

        /// <summary>This method is an event handler, it will be called when encoding process ends</summary>
        private void proc_Exited(object sender, EventArgs e)
        {           
            // handle event when process is exited                        
            if (GetCancelled()) 
            {
                DeleteFiles();
                DeleteEncodedFile();
            }

            m_lCompleted++;
            m_nProcessStarted--;
        }

        /// <summary>This method isused to calculated percentage of encoding progress</summary>
        private int getPercentage(TimeSpan initialProgress)
        {
            TimeSpan totalTime = TimeSpan.FromSeconds(m_lEndTimeTotal);
            //m_lProgressTimeTotal += initialProgress.TotalMilliseconds;
            double prgress = (TimeSpan.FromSeconds(m_lProgressTimeTotal).TotalMilliseconds / totalTime.TotalMilliseconds);
            return (int)(prgress * 100);
        }

        /// <summary>This method is called to create the status form for the operation</summary>
        /// <param name="strFileSpec">The path to the output file</param>
        /// <returns>true if successful</returns>
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

        /// <summary>This method is called to update the status text on the status form</summary>
        /// <param name="strStatus">The new status message</param>
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
            { }

        }

        /// <summary>This method is called to prepare encoding prerequisites</summary>
        /// <param name="Source">This contain the sources of encoding</param>
        private void PrepareAndStartEncode(List<CFFMpegSource> Source)
        {
            // Initiate no. of threads as defined inm_nMaximumEncodingThreads
            for (int t = m_nProcessStarted; t <= m_nMaximumEncodingThreads; t++)
            {
                // get the total no of files that is in processed and completed
                int totalEncodingProcess = m_nProcessStarted + (int)m_lCompleted;

                // if there are remaining files to start
                if (Source.Count() > totalEncodingProcess)
                {
                    // if source is available and not cancelled by the user
                    if (Source != null && Source.Count > 0 && !GetCancelled())
                    {
                        /*
                         We are using FFMPEG Encoder to encode videos, so it is limitation in concat or merge different scripts
                         in single video to mutiple formats it is only possible in mpg
                         so if there are multiple scripts than we first cut and encode it in mpg
                         so we can add or concat them at last. (as we have done in MergeFile() method)
                         ********************************************************************************
                         There is another way to encode and merge multiple videos in a single file using FFMPEG
                         we first encode all files with their time duration in the required format
                         than we add all those files name in a text file named here "inputfiles.txt"
                         and than using this file in ffmpeg command to join all files
                         before we was following the 1st approach define above now we are using this approach.
                        */

                        string param = string.Empty;
                        CFFMpegSource source = Sources[(int)totalEncodingProcess];
                        double differenceTime = source.m_dEndTime - source.m_dStartTime;

                        // if there are single script so we do not need to merge, we only need to encode directly
                        if (Source.Count == 1)
                        {
                            
                            // encoding parameters
                            param = "-i \"" + source.m_strSourceFile + "\" -ss " + TimeSpan.FromSeconds(source.m_dStartTime) + " -t " + TimeSpan.FromSeconds(differenceTime) + " -b:v " + m_strBitrate + " " + getCodec(m_strFileSpec) + " \"" + m_strFileSpec + "\"";
                        }
                        else
                        {
                            // if there is a need to merge the file than 
                            // we first encode the file with changed the name by appending number in the filename
                            string destinationFileName = m_strFileSpec;

                            // get the actual destination filename and change it to convert in mpg first
                            string extension = m_strFileSpec.Substring(m_strFileSpec.LastIndexOf("."));
                            destinationFileName = destinationFileName.Replace(extension, "");
                            destinationFileName = destinationFileName + "_" + totalEncodingProcess + extension;

                            // encoding parameters
                            param = "-i \"" + source.m_strSourceFile + "\" -ss " + TimeSpan.FromSeconds(source.m_dStartTime) + " -t " + TimeSpan.FromSeconds(differenceTime) + " " + getCodec(destinationFileName) + " \"" + destinationFileName + "\"";
                        }

                        // endtime of current script
                        m_lEndTime = (long)(source.m_dEndTime - source.m_dStartTime);

                        // mark that encoding is in progress
                        m_bIsEncodingInProgress = true;

                        // current source file
                        m_strSourceFile = source.m_strSourceFile;

                        // Increment the no of thread in process
                        m_nProcessStarted++;

                        // start encoding in a seperate thread, so we can cancel the encoding in UI thread
                        System.Threading.ParameterizedThreadStart threadStart = new System.Threading.ParameterizedThreadStart(RunProcess);
                        System.Threading.Thread thread = new System.Threading.Thread(threadStart);                        
                        thread.Start(param);
                    }
                }
                else
                {
                    break;
                }
            }                
        }

        /// <summary>This method is called to prepare encoding for merge prerequisites</summary>        
        private void MergeFile()
        {            
            string inputFileNames = string.Empty;
            
            // reset the endtime property
            m_lEndTime = 0;

            // get the pieces of files that is encoded to merge in a single file
            // m_lCompleted is a property that tell us how many number of files encoded for merging
            for (int f = 0; f < m_lCompleted; f++)
            {
                string filename = m_strFileSpec;
                string extension = m_strFileSpec.Substring(m_strFileSpec.LastIndexOf("."));
                filename = filename.Replace(extension, "");
                filename = filename + "_" + f + extension;

                // check if the encoded file exist than pick it and add in the concat list                
                if (System.IO.File.Exists(filename))
                {                    
                    inputFileNames += "file '" + filename + "'" + Environment.NewLine;
                }
            }

            // when merging is happen it means there are more than one file in the source list
            // get each source end time to calculate the length of total merging
            if (Sources != null && Sources.Count > 0)
            {
                foreach (CFFMpegSource ffmpegSource in Sources)
                {
                    m_lEndTime += (long)(ffmpegSource.m_dEndTime - ffmpegSource.m_dStartTime);
                }
            }

            // delete the txt file if exists
            if (System.IO.File.Exists(m_strInputFiles))
            {
                try
                {
                    System.IO.File.Delete(m_strInputFiles);
                }
                catch { }
            }

            // create a file stream to add filename that will encode for merge
            using (System.IO.FileStream fStream = new System.IO.FileStream(m_strInputFiles, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
            {
                byte[] fileBytes = System.Text.Encoding.ASCII.GetBytes(inputFileNames);
                fStream.Write(fileBytes, 0, fileBytes.Count());
                fStream.Close();
                fStream.Dispose();
            }

            // if we got some files to merge
            if (!string.IsNullOrEmpty(inputFileNames))
            {
                // encoding and merging parameters
                //string concatParam = @"-i ""concat:" + inputFiles + "\" -b:v " + m_strBitrate + " " + getCodec(m_strFileSpec) +" \"" + m_strFileSpec + "\"";
                string concatParam = @"-f concat -i " + m_strInputFiles + "  -c copy " + " \"" + m_strFileSpec + "\"";
                
                // mark that we start finalizing
                m_bIsFinalizing = true;

                // start encoding in a seperate thread, so we can cancel the encoding in UI thread
                System.Threading.ParameterizedThreadStart threadStart = new System.Threading.ParameterizedThreadStart(RunProcess);
                System.Threading.Thread thread = new System.Threading.Thread(threadStart);
                thread.Start(concatParam);
                
            }
        }

        /// <summary>This method is called to delete the temporary encoding files that was created for merging</summary>        
        private void DeleteFiles()
        {            
            for (int f = 0; f < m_lCompleted; f++)
            {
                string filename = m_strFileSpec;
                string extension = m_strFileSpec.Substring(m_strFileSpec.LastIndexOf("."));
                filename = filename.Replace(extension, "");
                filename = filename + "_" + f + extension;

                try
                {
                    if (System.IO.File.Exists(filename))
                        System.IO.File.Delete(filename);
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(filename + " unable to delete" + Environment.NewLine + ex.Message);
                }

                // delete txt file
                if (System.IO.File.Exists(m_strInputFiles))
                {
                    try
                    {
                        System.IO.File.Delete(m_strInputFiles);
                    }
                    catch { }
                }
            }
        }

        /// <summary>This method is called to delete the permanent encoding file when user cancelled encoding</summary>
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

        /// <summary>This method is called to calculate the total length/time of video that is going to be encoded </summary>
        private void CalculateTotalLength()
        {
            if (Sources != null && Sources.Count > 0)
            {
                foreach (CFFMpegSource source in Sources)
                {
                    double differenceTime = source.m_dEndTime - source.m_dStartTime;
                    m_lEndTimeTotal += (int)Math.Ceiling(differenceTime);
                }
            }
        }

        /// <summary>This method is called to calculate the total progress of video that is encoded </summary>
        private void CalculateTotalEncodingProgress()
        {
            if (Sources != null && Sources.Count > 0 && m_lCompleted > 0)
            {
                CFFMpegSource source = Sources[(int)m_lCompleted - 1];                
                 double differenceTime = source.m_dEndTime - source.m_dStartTime;
                 m_lProgressTimeTotal += (int)Math.Ceiling(differenceTime);
                
            }
        }

        /// <summary>This method is used to get codec for particular file for encoding</summary>
        /// <param name="sourceFile">This contain the File based on which codec is decided</param>
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
                if(m_bIsMpeg2Selected)
                    codec = "-vcodec mpeg2video";
                else
                    codec = "-qscale:v 1";
            }            
            else if (
               (extension.ToUpper().Contains(
               Convert.ToString(SupportedExportFormats.MP4))) ||
               (extension.ToUpper().Contains(
               Convert.ToString(SupportedExportFormats.MOV))) 
                )
            {
                // right now 
                codec = "";//"-acodec libfaac -vcodec mpeg4";
            }
            else if (          
          (extension.ToUpper().Contains(
          Convert.ToString(SupportedExportFormats.AVI)))
           )
            {
                // right now 
                codec = "-acodec copy -vcodec copy ";//"-acodec libfaac -vcodec mpeg4";
            }
            else if (               
                (extension.ToUpper().Contains(
               Convert.ToString(SupportedExportFormats.M2V)))
                )
            {
                codec = "-vcodec mpeg2video";
            }            
            else 
            {
                codec = "";
            }

            return codec;
        }

        private int NumberOfEncodedFilesFound()
        {
            int encodedFiles = 0;
            for (int f = 0; f < m_lCompleted; f++)
            {
                string filename = m_strFileSpec;
                string extension = m_strFileSpec.Substring(m_strFileSpec.LastIndexOf("."));
                filename = filename.Replace(extension, "");
                filename = filename + "_" + f + extension;

                // check if the encoded file exist than pick it and add in the concat list                
                if (System.IO.File.Exists(filename))
                {
                    encodedFiles++;
                }
            }
            return encodedFiles;
        }
    }
}
