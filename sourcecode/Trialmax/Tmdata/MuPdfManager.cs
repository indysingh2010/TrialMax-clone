using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ghostscript.NET;
using Leadtools;
using Leadtools.Codecs;

namespace FTI.Trialmax.Database
{
    public class CTmaxMuPdfManager
    {

        #region Constants

        #endregion Constants

        #region Private Members

        /// <summary>Local member to conversion using Leadtools</summary>
        private CTmaxLtPdfManager m_LtManager = null;

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

        #endregion Private Members

        #region Public Members

        /// <summary> Notify Parent to update Progress bar on a single task completion </summary>
        public event EventHandler notifyPDFManager;

        #endregion Public Members

        #region Public Methods

        /// <summary>Constructor</summary>
        public CTmaxMuPdfManager(string docPath, string outPath, short outResolution = 0, short totThreads = 0)
        {
            m_documentNameWithPath  = docPath;
            m_outputPath            = outPath;
            m_resolution            = outResolution;
            m_totalThreads          = totThreads;
            InitializeLeadtools();
            InitializeGhostscript();
            m_TotalPages            = m_LtManager.GetTotalPages();
        }

        ///<summary>Start the conversion process using MuPdf/Leadtools/Ghostscript</summary>
        public bool Process()
        {
            for (int i = 1; i <= m_TotalPages; i++)
            {
                if (isColor(i))
                {
                    if (!m_LtManager.ProcessPage(i))
                        return false;
                }
                else
                {
                    if (!m_GsManager.ProcessPage(i))
                        return false;
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Members

        ///<summary>Check if the page is colored or not using MuDraw.exe</summary>
        private bool isColor(int pageNum)
        {
            bool result = true;
            string parameters = "-T \"" + m_documentNameWithPath + "\" " + pageNum;
            System.Diagnostics.ProcessStartInfo oInfo = new System.Diagnostics.ProcessStartInfo("PDFManager/mudraw.exe", parameters);                
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            using (System.Diagnostics.Process proc = System.Diagnostics.Process.Start(oInfo))
            {
                // allow for reading asynhcronous Output
                proc.BeginErrorReadLine();

                // Blocking untilt the encoding done
                proc.WaitForExit();
                result = proc.StandardOutput.ReadToEnd().Contains("color");

                proc.Close();
                proc.Dispose();
            }
            return result;
        }

        ///<summary>Initialize Leadtools object for color conversions</summary>
        private bool InitializeLeadtools()
        {
            m_LtManager = new CTmaxLtPdfManager(m_documentNameWithPath, m_outputPath, m_resolution);
            m_LtManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return (m_LtManager != null);
        }

        ///<summary>Initialize Ghostscript object for color conversions</summary>
        private bool InitializeGhostscript()
        {
            m_GsManager = new CTmaxGsPdfManager(m_documentNameWithPath, m_outputPath, m_resolution);
            m_GsManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return (m_GsManager != null);
        }

        ///<summary>Notify PDFManager to update Statusbar</summary>
        protected void UpdateRegStatusBar(object sender, EventArgs e)
        {
            if (notifyPDFManager != null)
                notifyPDFManager(sender, e);
        }
        #endregion Private Members
    }
}
