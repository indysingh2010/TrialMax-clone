using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ghostscript.NET;

namespace FTI.Trialmax.Database
{
    /// <summary>
    ///          This class is the Manager that does the PDF to Image conversion for AutoDetect
    /// </summary>
    public class CTmaxMuPdfManager
    {

        #region Constants

        #endregion Constants

        #region Private Members

        ///// <summary>Local member to conversion using Leadtools</summary>
        //private CTmaxLtPdfManager m_LtManager = null;

        /// <summary>Local member to conversion using Ghostscript</summary>
        private CTmaxGsPdfManager m_GsManager = null;

        /// <summary>Local path where to output files</summary>
        private string m_outputPath = string.Empty;

        /// <summary>Local path where the document is present</summary>
        private string m_documentNameWithPath = string.Empty;

        /// <summary>Number of threads to be used. If set to 0, auto manage</summary>
        private short m_totalThreads;

        /// <summary>Resolution for Output images</summary>
        private short m_resolution;

        /// <summary>Total number of Pages in the current loaded PDF</summary>
        private int m_TotalPages;

        /// <summary>Flag that tells whether to convert files or not</summary>
        private volatile bool DoConvert = true;

        /// <summary>Local variable to log detail errors with stacktrace</summary>
        private static readonly log4net.ILog logDetailed = log4net.LogManager.GetLogger("DetailedLog");

        /// <summary>Local variable to log user level details</summary>
        private static readonly log4net.ILog logUser = log4net.LogManager.GetLogger("UserLog");

        /// <summary>Local member that will store if Custom Dither should be disabled</summary>
        private bool m_DisableCustomDither = false;

        #endregion Private Members

        #region Public Members

        /// <summary> Notify Parent to update Progress bar on a single task completion </summary>
        public event EventHandler notifyPDFManager;

        #endregion Public Members

        #region Public Methods

        /// <summary>Constructor</summary>
        public CTmaxMuPdfManager(string docPath, string outPath, short outResolution = 0, bool disableCustomDither = false, short totThreads = 0)
        {
            m_documentNameWithPath  = docPath;
            m_outputPath            = outPath;
            m_resolution            = outResolution;
            m_totalThreads          = totThreads;
            m_DisableCustomDither   = disableCustomDither;
            try
            {
                if (InitializeGhostscript())
                    m_TotalPages = m_GsManager.GetTotalPages();
                else
                    m_TotalPages = 0;
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
        }// public CTmaxMuPdfManager(string docPath, string outPath, short outResolution = 0, short totThreads = 0)

        ///<summary>Start the conversion process using MuPdf/Leadtools/Ghostscript</summary>
        public bool Process()
        {
            if (m_TotalPages == 0)
                return false;
            for (int i = 1; i <= m_TotalPages; i++)
            {
                if (!DoConvert)
                    return false;

                if (!m_GsManager.ProcessPage(i, isColor(i)))
                    return false;
                //if (isColor(i))
                //{
                //    if (!m_LtManager.ProcessPage(i))
                //        return false;
                //}
                //else
                //{
                //    if (!m_GsManager.ProcessPage(i))
                //        return false;
                //}
            }
            return true;
        }// public bool Process()

        /// <summary>Stop any running conversion process immediatelly</summary>
        public void StopProcess()
        {
            DoConvert = false;
            if (m_GsManager != null)
                m_GsManager.StopProcess();
            //if (m_LtManager != null)
            //    m_LtManager.StopProcess();
        }// public void StopProcess()

        ///<summary>Release all resources used by this class</summary>
        public void Dispose()
        {
            //try
            //{
            //    if (m_LtManager != null)
            //        m_LtManager.Dispose();
            //}
            //catch (Exception Ex)
            //{
            //    logDetailed.Error(Ex.ToString());
            //}
            try
            {
                if (m_GsManager != null)
                    m_GsManager.Dispose();
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
        }// public void Dispose()

        #endregion Public Methods

        #region Private Methods

        ///<summary>Check if the page is colored or not using MuDraw.exe</summary>
        private bool isColor(int pageNum)
        {
            bool result = true;
            try
            {
                string parameters = "-T \"" + m_documentNameWithPath + "\" " + pageNum;
                System.Diagnostics.ProcessStartInfo oInfo = new System.Diagnostics.ProcessStartInfo(@"PDFManager\mudraw.exe", parameters);
                oInfo.UseShellExecute = false;
                oInfo.CreateNoWindow = true;
                oInfo.RedirectStandardOutput = true;
                oInfo.RedirectStandardError = true;

                using (System.Diagnostics.Process proc = System.Diagnostics.Process.Start(oInfo))
                {
                    // allow for reading asynhcronous Output
                    proc.BeginErrorReadLine();

                    // Blocking until the process exits
                    proc.WaitForExit();
                    result = proc.StandardOutput.ReadToEnd().Contains("color");

                    proc.Close();
                    proc.Dispose();
                }
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
            return result;
        }// private bool isColor(int pageNum)

        ///<summary>Initialize Leadtools object for color conversions</summary>
        private bool InitializeLeadtools()
        {
            //m_LtManager = new CTmaxLtPdfManager(m_documentNameWithPath, m_outputPath, m_resolution);
            //m_LtManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            //return (m_LtManager != null);
            return false;
        }// private bool InitializeLeadtools()

        ///<summary>Initialize Ghostscript object for black and white conversions</summary>
        private bool InitializeGhostscript()
        {
            m_GsManager = new CTmaxGsPdfManager(m_documentNameWithPath, m_outputPath, FTI.Shared.Trialmax.TmaxPDFOutputType.ForceColor, m_resolution);
            m_GsManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return (m_GsManager != null);
        }// private bool InitializeGhostscript()

        #endregion Private Methods

        #region Protected Method

        ///<summary>Notify PDFManager to update Statusbar</summary>
        protected void UpdateRegStatusBar(object sender, EventArgs e)
        {
            if (notifyPDFManager != null)
                notifyPDFManager(sender, e);
        }// protected void UpdateRegStatusBar(object sender, EventArgs e)

        #endregion Protected Method
    }
}
