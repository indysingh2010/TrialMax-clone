using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Trialmax.ActiveX;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections.Generic;

namespace FTI.Trialmax.Controls
{
    /// <summary>Control used to tune video clips and designations</summary>
    public class CTmaxVideoTunerCtrl : CTmaxVideoBaseCtrl
    {
        #region Private Members

        /// <summary>Required designer variable</summary>
        private System.ComponentModel.Container components = null;

        /// <summary>The object for accessing the video player</summary>
        private CTmaxVideoPlayerCtrl m_ctrlPlayer;

        /// <summary>Panel used to group Tune Bar and the wave form picture box</summary>
        private Panel m_ctrlPanel;

        /// <summary>The picture box that will show the wave form</summary>
        private PictureBox m_picWave;

        /// <summary>The bitmap object which will store the orignal wave form for the loaded file</summary>
        private Bitmap m_orignalWave;

        /// <summary></summary>
        private WaveOut playbackDevice = new WaveOut() { DesiredLatency = 100, NumberOfBuffers = 2 };

        /// <summary>Location of the converter to be used for video to audio conversion</summary>
        private const string m_converter = "//ffmpeg.exe";

        /// <summary>Format for the converted audio</summary>
        private const string m_convertToFormat = "wav";

        /// <summary>Custom tune bar control for managing tune states</summary>
        private FTI.Trialmax.Controls.CTmaxVideoTuneBarCtrl m_ctrlTuneBar;

        /// <summary></summary>
        private class MinMax
        {
            public float minValue { get; set; }
            public float maxValue { get; set; }
            public MinMax(float min, float max)
            {
                minValue = min;
                maxValue = max;
            }
        }// public class MinMax

        #endregion Private Members

        #region Public Methods

        public CTmaxVideoTunerCtrl()
            : base()
        {
            //	Set the default event source name
            m_tmaxEventSource.Name = "Video Tuner Control";

            //	Initialize the child controls
            InitializeComponent();

            //	Attach to the child controls' event sources
            m_ctrlTuneBar.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
            m_ctrlTuneBar.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
            m_ctrlTuneBar.PreviewPeriod = m_ctrlPlayer.PreviewPeriod;
            m_ctrlPlayer.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);
            m_ctrlPlayer.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);

            //	Connect the video event handlers/triggers
            m_ctrlTuneBar.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrlEvent);
            m_ctrlTuneBar.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(m_ctrlPlayer.OnTmaxVideoCtrlEvent);
            m_ctrlPlayer.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrlEvent);
            m_ctrlPlayer.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(m_ctrlTuneBar.OnTmaxVideoCtrlEvent);

        }// CTmaxVideoTunerCtrl()

        /// <summary>This method handles all video events fired by the player and tune bar</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">event arguments</param>
        public override void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
        {
            //	Propagate the events
            BubbleTmaxVideoCtrlEvent(sender, e);

            switch (e.EventId)
            {
                case TmaxVideoCtrlEvents.PlayerPositionChanged:
                    UpdateLocation(e.Position);
                    break;
                default:
                    break;

            }

        }// protected void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)

        /// <summary>This method is called when the attributes associated with the active designation have changed</summary>
        /// <param name="xmlDesignation">The designation to be updated with the current property values</param>
        /// <returns>true if successful</returns>
        public override bool OnAttributesChanged(CXmlDesignation xmlDesignation)
        {
            bool bSuccessful = true;

            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
            {
                if (m_ctrlTuneBar.OnAttributesChanged(xmlDesignation) == false)
                    bSuccessful = false;
            }

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
            {
                if (m_ctrlPlayer.OnAttributesChanged(xmlDesignation) == false)
                    bSuccessful = false;
            }

            return bSuccessful;

        }// public override bool OnAttributesChanged()

        /// <summary>This method is called when the attributes associated with the active designation have changed</summary>
        /// <param name="xmlLink">The link to be updated with the current property values</param>
        /// <returns>true if successful</returns>
        public override bool OnAttributesChanged(CXmlLink xmlLink)
        {
            bool bSuccessful = true;

            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
            {
                if (m_ctrlTuneBar.OnAttributesChanged(xmlLink) == false)
                    bSuccessful = false;
            }

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
            {
                if (m_ctrlPlayer.OnAttributesChanged(xmlLink) == false)
                    bSuccessful = false;
            }

            return bSuccessful;

        }// public override bool OnAttributesChanged()

        /// <summary>This method is called to determine if modifications have been made to the active designation</summary>
        /// <param name="xmlDesignation">The active designation</param>
        ///	<param name="aModifications">An array in which to put the description of all modifications</param>
        /// <returns>true if modified</returns>
        public override bool IsModified(CXmlDesignation xmlDesignation, ArrayList aModifications)
        {
            bool bModified = false;

            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
            {
                if (m_ctrlTuneBar.IsModified(xmlDesignation, aModifications) == true)
                    bModified = true;
            }

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
            {
                if (m_ctrlPlayer.IsModified(xmlDesignation, aModifications) == true)
                    bModified = true;
            }

            return bModified;

        }// public override bool IsModified(CXmlDesignation xmlDesignation)

        /// <summary>This method is called to determine if modifications have been made to the active link</summary>
        /// <param name="xmlLink">The active link</param>
        ///	<param name="aModifications">An array in which to put the description of all modifications</param>
        /// <returns>true if modified</returns>
        public override bool IsModified(CXmlLink xmlLink, ArrayList aModifications)
        {
            bool bModified = false;

            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
            {
                if (m_ctrlTuneBar.IsModified(xmlLink, aModifications) == true)
                    bModified = true;
            }

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
            {
                if (m_ctrlPlayer.IsModified(xmlLink, aModifications) == true)
                    bModified = true;
            }

            return bModified;

        }// public override bool IsModified(CXmlLink xmlLink)

        /// <summary>This method is called to get the derived class property values and use them to set the designation attributes</summary>
        /// <param name="xmlDesignation">The designation to be updated with the current property values</param>
        /// <returns>true if successful</returns>
        public override bool SetAttributes(CXmlDesignation xmlDesignation)
        {
            bool bSuccessful = true;

            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
            {
                if (m_ctrlTuneBar.SetAttributes(xmlDesignation) == false)
                    bSuccessful = false;
            }

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
            {
                if (m_ctrlPlayer.SetAttributes(xmlDesignation) == false)
                    bSuccessful = false;
            }

            return bSuccessful;

        }// public override bool SetAttributes(CXmlDesignation xmlDesignation)

        /// <summary>This method is called to get the derived class property values and use them to set the link attributes</summary>
        /// <param name="xmlLink">The link to be updated with the current property values</param>
        /// <returns>true if successful</returns>
        public override bool SetAttributes(CXmlLink xmlLink)
        {
            bool bSuccessful = true;

            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
            {
                if (m_ctrlTuneBar.SetAttributes(xmlLink) == false)
                    bSuccessful = false;
            }

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
            {
                if (m_ctrlPlayer.SetAttributes(xmlLink) == false)
                    bSuccessful = false;
            }

            return bSuccessful;

        }// public override bool SetAttributes(CXmlLink xmlLink)

        /// <summary>This method is called to set the control properties</summary>
        /// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
        /// <param name="xmlDesignation">The designation used to set property values</param>
        /// <returns>true if successful</returns>
        public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
        {
            bool bSuccessful = true;

            //	Can't set the properties unless we have the child controls
            Debug.Assert(m_ctrlPlayer != null);
            Debug.Assert(m_ctrlPlayer.IsDisposed == false);
            Debug.Assert(m_ctrlTuneBar != null);
            Debug.Assert(m_ctrlTuneBar.IsDisposed == false);

            if (m_ctrlPlayer.SetProperties(strFileSpec, xmlDesignation) == true)
            {
                m_strFileSpec = m_ctrlPlayer.FileSpec;
                GenerateAudioWave();
            }
            else
                bSuccessful = false;

            if (m_ctrlTuneBar.SetProperties(strFileSpec, xmlDesignation) == true)
                m_xmlDesignation = m_ctrlTuneBar.XmlDesignation;
            else
                bSuccessful = false;

            return bSuccessful;

        }// public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)

        /// <summary>This method is called to generate the audio wave form</summary>
        private void GenerateAudioWave()
        {
            // Check if a valid video file is assigned to the player
            if (string.IsNullOrEmpty(m_ctrlPlayer.FileSpec)) return;

            // Setup FFMPEG location
            string ffmpeg = System.IO.Directory.GetCurrentDirectory() + m_converter;

            // Verify that ffmpeg exists
            if (!System.IO.File.Exists(ffmpeg)) return;

            if (System.IO.File.Exists(System.IO.Path.ChangeExtension(m_ctrlPlayer.FileSpec, m_convertToFormat)))
                System.IO.File.Delete(System.IO.Path.ChangeExtension(m_ctrlPlayer.FileSpec, m_convertToFormat));

            // Execute Video to Audio conversion
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = ffmpeg;
            startInfo.Arguments = " -y -i ";
            startInfo.Arguments += "\"" + m_ctrlPlayer.FileSpec + "\"";
            startInfo.Arguments += " -ss " + m_ctrlPlayer.StartPosition;
            startInfo.Arguments += " -to " + m_ctrlPlayer.StopPosition;
            startInfo.Arguments += " \"" + System.IO.Path.ChangeExtension(m_ctrlPlayer.FileSpec, m_convertToFormat) + "\"";
            process.StartInfo = startInfo;

            Cursor.Current = Cursors.WaitCursor;

            process.Start();
            process.WaitForExit();

            m_orignalWave = CreateAudioWave(System.IO.Path.ChangeExtension(m_ctrlPlayer.FileSpec, m_convertToFormat));
            System.IO.File.Delete(System.IO.Path.ChangeExtension(m_ctrlPlayer.FileSpec, m_convertToFormat));
            m_picWave.Image = (Bitmap)m_orignalWave.Clone();

            Cursor.Current = Cursors.Default;
        }// private void GenerateAudioWave()

        /// <summary>Generate the wave from the audio file.</summary>
        /// <param name="fileName">Location of the audio file for which the wave form is to be generated</param>
        /// <returns></returns>
        public Bitmap CreateAudioWave(string fileName)
        {
            if (!System.IO.File.Exists(fileName)) return null;

            var fileStream = new WaveFileReader(fileName);

            var fileProvider = new WaveFileReader(fileName);
            playbackDevice.Init(fileProvider);

            var sampleProvider = fileStream.ToSampleProvider();
            var Values = GetWaveFormValues(sampleProvider, fileStream.WaveFormat, fileStream.Length, fileStream.WaveFormat.SampleRate / 10);

            // Generating the wave form image
            Bitmap bim = new Bitmap(Values.Count, 200);
            System.Drawing.Graphics g = Graphics.FromImage(bim);

            Pen pen = new Pen(Color.Red, 2.0f);
            g.Clear(Color.Black);

            var mid = 100;
            var yScale = 100;

            for (int i = 0; i < Values.Count; i++)
            {
                float Y1 = mid + (Values[i] * yScale);
                float Y2 = mid - (Values[i] * yScale);

                g.DrawLine(pen, i, Y1, i, Y2);
            }
            fileStream.Close();
            fileProvider.Close();
            return bim;
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
        private List<float> GetWaveFormValues(ISampleProvider provider, WaveFormat format, long length, int notificationCount)
        {
            int bufferSize = format.ConvertLatencyToByteSize((((WaveOut)playbackDevice).DesiredLatency + ((WaveOut)playbackDevice).NumberOfBuffers - 1) / ((WaveOut)playbackDevice).NumberOfBuffers);
            var buf = new float[bufferSize];
            int samplesRead = 0;
            int count = 0;

            List<float> Values = new List<float>((int)(length / notificationCount));

            float maxValue = 0;
            while ((samplesRead = provider.Read(buf, 0, buf.Length)) > 0)
            {
                for (int n = 0; n < samplesRead; n += format.Channels)
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
        }// private List<MinMax> GetWaveForm

        /// <summary>
        /// This function is called to draw a vertical bar on the waveform to show the current position of the video
        /// </summary>
        /// <param name="position">The time at which the video is.</param>
        public void UpdateLocation(double position)
        {
            if (m_orignalWave == null) return;
            double length = m_ctrlPlayer.StopPosition - m_ctrlPlayer.StartPosition;
            position = position - m_ctrlPlayer.StartPosition;

            Bitmap bmp = (Bitmap)m_orignalWave.Clone();
            Pen blackPen = new Pen(Color.Blue, m_orignalWave.Width / 10000);

            float x1 = (float)(position / length * m_orignalWave.Width);
            float y1 = 0;
            float x2 = x1;
            float y2 = 200;
            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawLine(blackPen, x1, y1, x2, y2);
            }
            m_picWave.Image = bmp;
        }// public void UpdateLocation(double position)

        /// <summary>This method is called to set the control properties</summary>
        /// <param name="xmlLink">The link used to set property values</param>
        /// <returns>true if successful</returns>
        public override bool SetProperties(CXmlLink xmlLink)
        {
            bool bSuccessful = true;

            m_xmlLink = xmlLink;

            //	Can't set the properties unless we have the child controls
            Debug.Assert(m_ctrlPlayer != null);
            Debug.Assert(m_ctrlPlayer.IsDisposed == false);
            Debug.Assert(m_ctrlTuneBar != null);
            Debug.Assert(m_ctrlTuneBar.IsDisposed == false);

            if (m_ctrlPlayer.SetProperties(xmlLink) == false)
                bSuccessful = false;

            if (m_ctrlTuneBar.SetProperties(xmlLink) == false)
                bSuccessful = false;

            return bSuccessful;

        }// public override bool SetProperties(CXmlLink xmlLink)

        /// <summary>This method is called to play the specified collection of designations</summary>
        ///	<param name="xmlDesignations">the collection of designations that define the script</param>
        ///	<param name="iFirst">the index of the designation to start with</param>
        /// <param name="bPlayToEnd">true to play to end</param>
        /// <returns>true if successful</returns>
        public override bool SetScript(CXmlDesignations xmlDesignations, int iFirst, bool bPlayToEnd)
        {
            bool bSuccessful = true;

            //	Can't set the properties unless we have the child controls
            Debug.Assert(m_ctrlPlayer != null);
            Debug.Assert(m_ctrlPlayer.IsDisposed == false);
            Debug.Assert(m_ctrlTuneBar != null);
            Debug.Assert(m_ctrlTuneBar.IsDisposed == false);

            if (m_ctrlPlayer.SetScript(xmlDesignations, iFirst, bPlayToEnd) == false)
                bSuccessful = false;

            if (m_ctrlTuneBar.SetScript(xmlDesignations, iFirst, bPlayToEnd) == false)
                bSuccessful = false;

            return bSuccessful;

        }// public virtual bool SetScript(CXmlDesignations xmlDesignations, int iFirst, bool bPlayToEnd)

        /// <summary>This method is called when the user wants to start playing a script</summary>
        /// <returns>true if successful</returns>
        public override bool StartScript()
        {
            bool bSuccessful = true;

            //	Can't set the properties unless we have the child controls
            Debug.Assert(m_ctrlPlayer != null);
            Debug.Assert(m_ctrlPlayer.IsDisposed == false);
            Debug.Assert(m_ctrlTuneBar != null);
            Debug.Assert(m_ctrlTuneBar.IsDisposed == false);

            if (m_ctrlTuneBar.StartScript() == false)
                bSuccessful = false;

            // NOTE: Do the player last because it's going to be firing the runtime events
            if (m_ctrlPlayer.StartScript() == false)
                bSuccessful = false;

            return bSuccessful;

        }// public virtual bool StartScript()

        /// <summary>This method is called when the user wants to stop playing a script</summary>
        /// <returns>true if successful</returns>
        public override bool StopScript()
        {
            bool bSuccessful = true;

            //	Can't set the properties unless we have the child controls
            Debug.Assert(m_ctrlPlayer != null);
            Debug.Assert(m_ctrlPlayer.IsDisposed == false);
            Debug.Assert(m_ctrlTuneBar != null);
            Debug.Assert(m_ctrlTuneBar.IsDisposed == false);

            // NOTE: Do the player first because it's going to be firing the runtime events
            if (m_ctrlPlayer.StopScript() == false)
                bSuccessful = false;

            if (m_ctrlTuneBar.StopScript() == false)
                bSuccessful = false;

            return bSuccessful;

        }// public virtual bool StopScript()

        /// <summary>This method is called to set the video position</summary>
        /// <param name="dPosition">The new position in seconds</param>
        /// <returns>true if successful</returns>
        public bool SetPosition(double dPosition)
        {
            //	Can't set the properties unless we have the child controls
            Debug.Assert(m_ctrlPlayer != null);
            Debug.Assert(m_ctrlPlayer.IsDisposed == false);

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
                return m_ctrlPlayer.SetPosition(dPosition);
            else
                return false;

        }// public bool SetPosition(double dPosition)

        /// <summary>This method is called to set the active tune mode</summary>
        /// <param name="eMode">The desired mode</param>
        /// <param name="bSilent">true to inhibit mode change events</param>
        /// <returns>The new tune mode</returns>
        public TmaxVideoCtrlTuneModes SetTuneMode(TmaxVideoCtrlTuneModes eMode, bool bSilent)
        {
            Debug.Assert(m_ctrlTuneBar != null);
            Debug.Assert(m_ctrlTuneBar.IsDisposed == false);

            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
                return m_ctrlTuneBar.SetTuneMode(eMode, bSilent);
            else
                return TmaxVideoCtrlTuneModes.None;

        }// public TmaxVideoCtrlTuneModes SetTuneMode(TmaxVideoCtrlTuneModes eMode, bool bSilent)

        /// <summary>This method is called to set the active tune mode</summary>
        /// <param name="eMode">The desired mode</param>
        /// <returns>The new tune mode</returns>
        public TmaxVideoCtrlTuneModes SetTuneMode(TmaxVideoCtrlTuneModes eMode)
        {
            //	Set the mode and fire the event
            return SetTuneMode(eMode, false);
        }

        /// <summary>This method is called to force an update of the active tune mode</summary>
        /// <returns>The new tune mode</returns>
        public TmaxVideoCtrlTuneModes SetTuneMode()
        {
            //	Set the mode and fire the event
            return SetTuneMode(TuneMode, false);
        }

        /// <summary>This function is notify the control that the parent window has been moved</summary>
        /// <remarks>It is expected that derived classes will override this function</remarks>
        public override void OnParentMoved()
        {
            if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
            {
                m_ctrlTuneBar.OnParentMoved();
            }

            if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
            {
                m_ctrlPlayer.OnParentMoved();
            }

        }// public override void OnParentMoved()

        #endregion Public Methods

        #region Protected Methods

        /// <summary>Clean up any resources being used</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            //	Do the base class cleanup
            base.Dispose(disposing);

        }// protected override void Dispose(bool disposing)

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        protected new void InitializeComponent()
        {
            this.m_ctrlPlayer = new FTI.Trialmax.Controls.CTmaxVideoPlayerCtrl();
            this.m_ctrlPanel = new System.Windows.Forms.Panel();
            this.m_ctrlTuneBar = new FTI.Trialmax.Controls.CTmaxVideoTuneBarCtrl();
            this.m_picWave = new System.Windows.Forms.PictureBox();
            this.m_ctrlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_picWave)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ctrlPlayer
            // 
            this.m_ctrlPlayer.AllowApply = true;
            this.m_ctrlPlayer.BackColor = System.Drawing.SystemColors.Control;
            this.m_ctrlPlayer.ClassicLinks = true;
            this.m_ctrlPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ctrlPlayer.EnableSimulation = false;
            this.m_ctrlPlayer.LinkPosition = -1D;
            this.m_ctrlPlayer.Location = new System.Drawing.Point(0, 0);
            this.m_ctrlPlayer.Name = "m_ctrlPlayer";
            this.m_ctrlPlayer.PlayerPosition = -1D;
            this.m_ctrlPlayer.PlayOnLoad = false;
            this.m_ctrlPlayer.PreviewPeriod = 2D;
            this.m_ctrlPlayer.ShowPosition = true;
            this.m_ctrlPlayer.ShowTranscript = true;
            this.m_ctrlPlayer.SimulationText = "";
            this.m_ctrlPlayer.Size = new System.Drawing.Size(300, 303);
            this.m_ctrlPlayer.StartPosition = -1D;
            this.m_ctrlPlayer.StopPosition = -1D;
            this.m_ctrlPlayer.TabIndex = 1;
            this.m_ctrlPlayer.TuneMode = FTI.Trialmax.Controls.TmaxVideoCtrlTuneModes.None;
            // 
            // m_ctrlPanel
            // 
            this.m_ctrlPanel.Controls.Add(this.m_ctrlTuneBar);
            this.m_ctrlPanel.Controls.Add(this.m_picWave);
            this.m_ctrlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_ctrlPanel.Location = new System.Drawing.Point(0, 303);
            this.m_ctrlPanel.Name = "m_ctrlPanel";
            this.m_ctrlPanel.Size = new System.Drawing.Size(300, 135);
            this.m_ctrlPanel.TabIndex = 2;
            // 
            // m_ctrlTuneBar
            // 
            this.m_ctrlTuneBar.ClassicLinks = true;
            this.m_ctrlTuneBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ctrlTuneBar.EnableLinks = true;
            this.m_ctrlTuneBar.LinkPosition = -1D;
            this.m_ctrlTuneBar.Location = new System.Drawing.Point(0, 0);
            this.m_ctrlTuneBar.Name = "m_ctrlTuneBar";
            this.m_ctrlTuneBar.PlayerPosition = -1D;
            this.m_ctrlTuneBar.PreviewPeriod = 1.5D;
            this.m_ctrlTuneBar.Size = new System.Drawing.Size(300, 83);
            this.m_ctrlTuneBar.StartPosition = -1D;
            this.m_ctrlTuneBar.StopPosition = -1D;
            this.m_ctrlTuneBar.TabIndex = 0;
            this.m_ctrlTuneBar.TuneMode = FTI.Trialmax.Controls.TmaxVideoCtrlTuneModes.None;
            // 
            // m_picWave
            // 
            this.m_picWave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_picWave.Location = new System.Drawing.Point(0, 83);
            this.m_picWave.Name = "m_picWave";
            this.m_picWave.Size = new System.Drawing.Size(300, 52);
            this.m_picWave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_picWave.TabIndex = 0;
            this.m_picWave.TabStop = false;
            // 
            // CTmaxVideoTunerCtrl
            // 
            this.Controls.Add(this.m_ctrlPlayer);
            this.Controls.Add(this.m_ctrlPanel);
            this.Name = "CTmaxVideoTunerCtrl";
            this.Size = new System.Drawing.Size(300, 438);
            this.m_ctrlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_picWave)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion Protected Methods

        #region Properties

        /// <summary>Video Player control</summary>
        public FTI.Trialmax.Controls.CTmaxVideoPlayerCtrl Player
        {
            get { return m_ctrlPlayer; }

        }// Player property

        /// <summary>Tune bar control</summary>
        public FTI.Trialmax.Controls.CTmaxVideoTuneBarCtrl TuneBar
        {
            get { return m_ctrlTuneBar; }

        }// TuneBar property


        /// <summary>Current playback position</summary>
        public double PlayerPosition
        {
            get
            {
                if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
                    return m_ctrlPlayer.PlayerPosition;
                else
                    return -1;
            }

        }// PlayerPosition

        /// <summary>Time for previewing video from current position</summary>
        public double PreviewPeriod
        {
            get
            {
                if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
                    return m_ctrlTuneBar.PreviewPeriod;
                else
                    return -1.0;
            }
            set
            {
                if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
                    m_ctrlTuneBar.PreviewPeriod = value;
            }

        }// PreviewPeriod

        /// <summary>Enable simulated playback if unable to find the video file</summary>
        public bool EnableSimulation
        {
            get
            {
                if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
                    return m_ctrlPlayer.EnableSimulation;
                else
                    return false;
            }
            set
            {
                if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
                    m_ctrlPlayer.EnableSimulation = value;
            }

        }// EnableSimulation

        /// <summary>Text displayed by the player when simulating playback</summary>
        public string SimulationText
        {
            get
            {
                if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
                    return m_ctrlPlayer.SimulationText;
                else
                    return "";
            }
            set
            {
                if ((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
                    m_ctrlPlayer.SimulationText = value;
            }

        }// SimulationText

        /// <summary>Current tune mode</summary>
        public TmaxVideoCtrlTuneModes TuneMode
        {
            get
            {
                if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
                    return m_ctrlTuneBar.TuneMode;
                else
                    return TmaxVideoCtrlTuneModes.None;
            }

        }// TuneMode

        /// <summary>True to enable tuning of links</summary>
        public bool EnableLinks
        {
            get
            {
                if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
                    return m_ctrlTuneBar.EnableLinks;
                else
                    return true;
            }
            set
            {
                if ((m_ctrlTuneBar != null) && (m_ctrlTuneBar.IsDisposed == false))
                    m_ctrlTuneBar.EnableLinks = value;
            }

        }// EnableLinks

        #endregion Properties

    }// public class CTmaxVideoTunerCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls
