using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Xml;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using MediaToolkit;
using MediaToolkit.Model;
using System.Threading;
using System.IO;

namespace FTI.Trialmax.Encode
{
    public static class AudioWaveformGenerator
    {
        #region Private Members

        /// <summary>Location of the converter to be used for video to audio conversion</summary>
        private const string m_converter = "//ffmpeg.exe";

        /// <summary>Format for the converted audio</summary>
        private const string m_convertToFormat = "wav";

        /// <summary>Desired latency to be used for NAudio sampling</summary>
        private const int m_desiredLatency = 100;

        /// <summary>Number of buffers to be used for NAudio sampling</summary>
        private const int m_numberOfBuffers = 2;

        public static TimeSpan m_duration;

        public volatile static int m_lVideoConversionProgress = 0;

        private static CFEncoderStatus cfEncoderStatus = null;

        #endregion Private Members

        #region Public Methods


        public static TimeSpan GetMediaDuration(string strFileSpec)
        {
            var inputFile = new MediaFile { Filename = strFileSpec };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
            }
            return inputFile.Metadata.Duration;
        }

        /// <summary>This method is called to generate the audio wave form</summary>
        public static bool GenerateAudioWave(string strFileSpec, double startPosition = 0, double stopPosition = 0)
        {
            // Check if a valid video file is assigned to the player
            if (string.IsNullOrEmpty(strFileSpec)) return false;

            // Setup FFMPEG location
            string ffmpeg = System.IO.Directory.GetCurrentDirectory() + m_converter;

            // Verify that ffmpeg exists
            if (!System.IO.File.Exists(ffmpeg)) return false;

            try
            {
                if (System.IO.File.Exists(System.IO.Path.ChangeExtension(strFileSpec, m_convertToFormat)))
                    System.IO.File.Delete(System.IO.Path.ChangeExtension(strFileSpec, m_convertToFormat));


                // Execute Video to Audio conversion
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.FileName = ffmpeg;
                startInfo.Arguments = " -y -i ";
                startInfo.Arguments += "\"" + strFileSpec + "\"";
                if (stopPosition != 0 && startPosition < stopPosition)
                {
                    startInfo.Arguments += " -ss " + startPosition;
                    startInfo.Arguments += " -to " + stopPosition;
                }
                startInfo.Arguments += " \"" + System.IO.Path.ChangeExtension(strFileSpec, m_convertToFormat) + "\"";
                process.StartInfo = startInfo;

                Cursor.Current = Cursors.WaitCursor;

                process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
                process.EnableRaisingEvents = true;

                if (cfEncoderStatus == null)
                {
                    cfEncoderStatus = new CFEncoderStatus();
                    cfEncoderStatus.FileSpec = strFileSpec;
                    cfEncoderStatus.ShowCancel = false;
                    cfEncoderStatus.Show();
                }

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                cfEncoderStatus.SetStatus("Converting video to audio ...");

                while (!process.HasExited)
                {
                    cfEncoderStatus.SetProgress(m_lVideoConversionProgress);
                    cfEncoderStatus.Refresh();
                }

                cfEncoderStatus.SetStatus("Generating audio waveform ...");
                cfEncoderStatus.Refresh();

                CreateAudioWave(System.IO.Path.ChangeExtension(strFileSpec, m_convertToFormat));
                System.IO.File.Delete(System.IO.Path.ChangeExtension(strFileSpec, m_convertToFormat));

                cfEncoderStatus.Close();
                cfEncoderStatus.Dispose();
                cfEncoderStatus = null;

                Cursor.Current = Cursors.Default;                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }// private void GenerateAudioWave()

        static void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.Contains("Duration"))
            {
                m_duration = TimeSpan.Parse(e.Data.Split(',')[0].Replace("Duration: ", ""));
                return;
            }
            if (e.Data != null && e.Data.Contains("time"))
            {
                TimeSpan currentProgress = TimeSpan.Parse(e.Data.Split('=')[2].Replace(" bitrate", ""));
                m_lVideoConversionProgress = (Convert.ToInt32(currentProgress.Ticks * 100 / m_duration.Ticks));
                return;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>Generate the wave from the audio file.</summary>
        /// <param name="fileName">Location of the audio file for which the wave form is to be generated</param>
        /// <returns></returns>
        private static void CreateAudioWave(string fileName)
        {
            if (!System.IO.File.Exists(fileName)) return;

            var fileStream = new WaveFileReader(fileName);

            var sampleProvider = fileStream.ToSampleProvider();
            var Values = GetWaveFormValues(sampleProvider, fileStream.WaveFormat, fileStream.Length, fileStream.WaveFormat.SampleRate / 10);
            fileStream.Close();

            //File.WriteAllLines(Path.ChangeExtension(fileName, "txt"), Values.Select(x=>x.ToString()).ToArray());

            //return;

            // Generating the wave form image
            using (Bitmap bim = new Bitmap(Values.Count, 200))
            {
                using (System.Drawing.Graphics g = Graphics.FromImage(bim))
                {
                    Pen pen = new Pen(Color.White, 2.0f);
                    g.Clear(Color.Black);

                    var mid = 100;
                    var yScale = 100;

                    List<Point> p = new List<Point>();

                    for (int i = 0; i < Values.Count; i++)
                    {
                        float Y1 = mid + (Values[i] * yScale);
                        float Y2 = mid - (Values[i] * yScale);
                        p.Add(new Point(i, Convert.ToInt32(Math.Ceiling(Y1))));
                        p.Add(new Point(i, Convert.ToInt32(Math.Ceiling(Y2))));
                    }
                    g.DrawLines(pen, p.ToArray());
                }

                bim.Save(Path.ChangeExtension(fileName, "bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }// public Bitmap CreateAudioWave(string fileName)


        /// <summary>
        /// This function is called to generate the Y-values for the waveform
        /// Source: https://naudio.codeplex.com/discussions/649903
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="format"></param>
        /// <param name="length"></param>
        /// <param name="notificationCount"></param>
        /// <returns></returns>
        private static List<float> GetWaveFormValues(ISampleProvider provider, WaveFormat format, long length, int notificationCount)
        {
            try
            {
                int bufferSize = format.ConvertLatencyToByteSize((m_desiredLatency + m_numberOfBuffers - 1) / m_numberOfBuffers);
                var buf = new float[bufferSize];
                int samplesRead = 0;
                int count = 0;

                List<float> Values = new List<float>((int)(length / notificationCount));

                float maxValue = 0;
                while ((samplesRead = provider.Read(buf, 0, buf.Length)) > 0)
                {
                    for (int n = 0; n < samplesRead; n++)
                    {
                        maxValue = Math.Max(maxValue, buf[n]);
                        count++;
                        if (count >= notificationCount && notificationCount > 0)
                        {
                            Values.Add(maxValue);
                            maxValue = count = 0;
                        }
                    }
                }
                return Values;
            }
            catch (Exception ex) { }
            return new List<float>();

        }// private List<float> GetWaveForm

        #endregion
    }
}
