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

namespace FTI.Trialmax.Controls
{
    /// <summary>Control used to tune video clips and designations</summary>
    public class CTmaxVideoTunerCtrl : CTmaxVideoBaseCtrl
    {
        #region Private Members

        /// <summary>Required designer variable</summary>
        private System.ComponentModel.Container components = null;

        /// <summary>Custom tune bar control for managing tune states</summary>
        private FTI.Trialmax.Controls.CTmaxVideoTuneBarCtrl m_ctrlTuneBar;

        /// <summary>Custom video player for playing TrialMax designations</summary>
        private FTI.Trialmax.Controls.CTmaxVideoPlayerCtrl m_ctrlPlayer;

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
                m_strFileSpec = m_ctrlPlayer.FileSpec;
            else
                bSuccessful = false;

            if (m_ctrlTuneBar.SetProperties(strFileSpec, xmlDesignation) == true)
                m_xmlDesignation = m_ctrlTuneBar.XmlDesignation;
            else
                bSuccessful = false;

            return bSuccessful;

        }// public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)

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
            this.m_ctrlTuneBar = new FTI.Trialmax.Controls.CTmaxVideoTuneBarCtrl();
            this.m_ctrlPlayer = new FTI.Trialmax.Controls.CTmaxVideoPlayerCtrl();
            this.SuspendLayout();
            // 
            // m_ctrlTuneBar
            // 
            this.m_ctrlTuneBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_ctrlTuneBar.LinkPosition = -1;
            this.m_ctrlTuneBar.Location = new System.Drawing.Point(0, 248);
            this.m_ctrlTuneBar.Name = "m_ctrlTuneBar";
            this.m_ctrlTuneBar.PlayerPosition = -1;
            this.m_ctrlTuneBar.PreviewPeriod = 1.5;
            this.m_ctrlTuneBar.Size = new System.Drawing.Size(300, 72);
            this.m_ctrlTuneBar.StartPosition = -1;
            this.m_ctrlTuneBar.StopPosition = -1;
            this.m_ctrlTuneBar.TabIndex = 0;
            this.m_ctrlTuneBar.TuneMode = FTI.Trialmax.Controls.TmaxVideoCtrlTuneModes.None;
            // 
            // m_ctrlPlayer
            // 
            this.m_ctrlPlayer.AllowApply = true;
            this.m_ctrlPlayer.BackColor = System.Drawing.SystemColors.Control;
            this.m_ctrlPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ctrlPlayer.LinkPosition = -1;
            this.m_ctrlPlayer.Location = new System.Drawing.Point(0, 0);
            this.m_ctrlPlayer.Name = "m_ctrlPlayer";
            this.m_ctrlPlayer.PlayerPosition = -1;
            this.m_ctrlPlayer.PlayOnLoad = false;
            this.m_ctrlPlayer.PreviewPeriod = 2;
            this.m_ctrlPlayer.ShowPosition = true;
            this.m_ctrlPlayer.ShowTranscript = true;
            this.m_ctrlPlayer.Size = new System.Drawing.Size(300, 248);
            this.m_ctrlPlayer.StartPosition = -1;
            this.m_ctrlPlayer.StopPosition = -1;
            this.m_ctrlPlayer.TabIndex = 1;
            this.m_ctrlPlayer.TuneMode = FTI.Trialmax.Controls.TmaxVideoCtrlTuneModes.None;
            // 
            // CTmaxVideoTunerCtrl
            // 
            this.Controls.Add(this.m_ctrlPlayer);
            this.Controls.Add(this.m_ctrlTuneBar);
            this.Name = "CTmaxVideoTunerCtrl";
            this.Size = new System.Drawing.Size(300, 320);
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
