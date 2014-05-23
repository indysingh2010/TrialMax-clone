﻿using System;
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

        // bit rate on which the video will be encoded
        // yet we have fixed it to 768k
        private string m_strBitrate = "100k";

        // used to store end time of current video that is in encoding
        private long m_lEndTime = 0;

        /// <summary>Local member bound to Status property</summary>
        private CFEncoderStatus m_encodingStatus = null;

        /// <summary>Local member to store status of active source while encoding is taking place</summary>
        private string m_strSourceStatus;

        /// <summary>Local member to store current source while encoding is taking place</summary>
        private string m_strSourceFile;

        // number of files complete encoding
        private long m_lCompleted = 0;

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
                    if (!m_bIsEncodingInProgress)
                    {
                        PrepareAndStartEncode(Sources);
                    }                    
                }
                else
                    m_bIsEncodingCompleted = true;

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

        /// <summary>This method is an event handler, it will be called when encoding process ends</summary>
        private void proc_Exited(object sender, EventArgs e)
        {           
            // handle event when process is exited                        
            if (GetCancelled()) 
            {
                DeleteFiles();
                DeleteEncodedFile();
            }
        }

        /// <summary>This method isused to calculated percentage of encoding progress</summary>
        private int getPercentage(TimeSpan initialProgress)
        {
            TimeSpan totalTime = TimeSpan.FromSeconds(m_lEndTime);
            double prgress = (initialProgress.TotalMilliseconds / totalTime.TotalMilliseconds);
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

        /// <summary>This method is called to prepare encoding for merge prerequisites</summary>        
        private void MergeFile()
        {
            string inputFiles = string.Empty;
            
            // endtime of current script
            m_lEndTime = 0;

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
                {
                    inputFiles += filename + "|";                    
                }
            }

            // when merging is happen it means there are more than one file in the source list
            // get each source end time to calculate the length of total merging
            if (Sources != null && Sources.Count > 0)
            {
                foreach (CFFMpegSource ffmpegSource in Sources)
                {
                    m_lEndTime += (long)ffmpegSource.m_dEndTime;
                }
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

        /// <summary>This method is called to delete the temporary encoding files that was created for merging</summary>        
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
