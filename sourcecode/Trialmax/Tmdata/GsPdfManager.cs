using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using FTI.Shared;
using FTI.Shared.Win32;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;

using Ghostscript.NET;
using Ghostscript.NET.Processor;

namespace FTI.Trialmax.Database
{
    /// <summary>This class manages the import operations using GhostScript.NET wrapper for Ghostscript</summary>
    public class CTmaxGsPdfManager
    {
        #region Constants

        #endregion Constants

        #region Private Members

        /// <summary>Local path where to output files</summary>
        private string m_outputPath = string.Empty;

        /// <summary>Local path where the document is present</summary>
        private string m_documentNameWithPath = string.Empty;

        /// <summary>Number of threads to be used. If set to 0, auto manage</summary>
        private short m_totalThreads;

        /// <summary>Reference to the Ghostscript.dll to use</summary>
        private GhostscriptVersionInfo m_gvi = null;

        /// <summary>List of switches to be passed for GS Processing</summary>
        private List<string> m_switches = null;

        /// <summary>Resolution for Output images</summary>
        private short m_resolution;

        /// <summary>List containing errors if occured</summary>
        private List<Exception> m_conversionErrors = null;

        /// <summary>Flag that tells whether to convert files or not</summary>
        private volatile bool DoConvert = true;

        /// <summary>Local variable for GhostScriptProcessor that processes files so we can stop it any time</summary>
        private GhostscriptProcessor processor = null;

        #endregion Private Members

        #region Public Members

        /// <summary> Notify Parent to update Progress bar on a single task completion </summary>
        public event EventHandler notifyPDFManager;

        #endregion Public Members

        #region Public Methods

        ///<summary>Constructor</summary>
        public CTmaxGsPdfManager(string docPath, string outPath, short outResolution = 0, short totThreads = 0)
        {
            m_documentNameWithPath = docPath;
            m_outputPath = outPath;
            m_totalThreads = totThreads;
            m_resolution = outResolution;
            m_switches = new List<string>();
            InitializeGhostScript();
            AddRequiredSwitches();
        }// public CTmaxGsPdfManager(string docPath, string outPath, short outResolution = 0, short totThreads = 0)

        ///<summary>Start the conversion process using Ghostscript</summary>
        public bool Process()
        {
            processor = new GhostscriptProcessor(m_gvi, true);
            processor.Processing += new GhostscriptProcessorProcessingEventHandler(processor_Processing);
            try
            {
                processor.StartProcessing(m_switches.ToArray(), null);
            }
            catch (Exception ex)
            {
                if (m_conversionErrors == null)
                    m_conversionErrors = new List<Exception>();
                m_conversionErrors.Add(ex);
                return false;
            }
            return true;
        }// public bool Process()

        ///<summary>Process a single page</summary>
        public bool ProcessPage(int pageNum)
        {
            processor = new GhostscriptProcessor(m_gvi, true);
            AddPageSwitch(pageNum);
            processor.Processing += new GhostscriptProcessorProcessingEventHandler(processor_Processing);
            try
            {
                processor.StartProcessing(m_switches.ToArray(), null);
            }
            catch (Exception ex)
            {
                if (m_conversionErrors == null)
                    m_conversionErrors = new List<Exception>();
                m_conversionErrors.Add(ex);
                return false;
            }
            RemovePageSwitch();
            return true;
        }// public bool ProcessPage(int pageNum)

        ///<summary>Return the list of errors if occured any</summary>
        public List<Exception> GetConversionErrorList()
        {
            return m_conversionErrors;
        }// public List<Exception> GetConversionErrorList()

        ///<summary>TmaxPdfManager signalled to stop the conversion process</summary>
        public void StopProcess()
        {
            DoConvert = false;
            if (processor != null)
            {
                processor.StopProcessing();
                while (processor.IsRunning) // Wait until processor stops and then proceed
                {
                    Thread.Sleep(1000);
                }
                processor.Dispose();
            }
        }// public void StopProcess()

        #endregion Public Methods

        #region Private Methods

        ///<summary>Initializes the Ghostscript dll that will be used</summary>
        private bool InitializeGhostScript()
        {
            //m_gvi = GhostscriptVersionInfo.GetLastInstalledVersion();
            m_gvi = new GhostscriptVersionInfo("PDFManager/gsdll32.dll");
            return !(m_gvi == null);
        }// private bool InitializeGhostScript()

        ///<summary>Add all required switch for output</summary>
        private void AddRequiredSwitches()
        {
            AddDefaultSwitches();
            AddColorSwitch();
            AddResolutionSwitch();
            AddOutputSwitch();
            AddPdfOpenSwitch();
            //AddErrorLogSwitch();
        }// private void AddRequiredSwitches()

        ///<summary>Add default switch for output</summary>
        private void AddDefaultSwitches()
        {
            m_switches.Add("-dSAFER");
            m_switches.Add("-dNumRenderingThreads=2");
            m_switches.Add("-dBATCH");
            m_switches.Add("-dUseCropBox");
            m_switches.Add("-dNOPAUSE");
            m_switches.Add("-sPDFPassword=\"\"");
        }// private void AddDefaultSwitches()

        ///<summary>Add color switch for output</summary>
        private void AddColorSwitch()
        {
            m_switches.Add("-sDEVICE=tiffg4");
        }// private void AddColorSwitch()

        ///<summary>Add resolution switch for output</summary>
        private void AddResolutionSwitch()
        {
            if (m_resolution == 0)
            {
                m_switches.Add("-r300");
                return;
            }
            m_switches.Add("-r" + m_resolution);
        }// private void AddResolutionSwitch()

        ///<summary>Add output target switch</summary>
        private void AddOutputSwitch()
        {
            m_switches.Add(@"-sOutputFile=" + m_outputPath + "\\%04d.tif");
        }// private void AddOutputSwitch()

        ///<summary>Add PDF open switch</summary>
        private void AddPdfOpenSwitch()
        {
            m_switches.Add(m_documentNameWithPath);
        }// private void AddPdfOpenSwitch()

        ///<summary>Add error logging switch</summary>
        private void AddErrorLogSwitch()
        {
            m_switches.Add(">> \"" + m_outputPath + "\\conv.tmp");
            m_switches.Add("2>> \"" + m_outputPath + "\\conv.err");
        }// private void AddErrorLogSwitch()

        ///<summary>Progress of the current task if performing any</summary>
        private void processor_Processing(object sender, GhostscriptProcessorProcessingEventArgs e)
        {
            if (notifyPDFManager != null)
                notifyPDFManager(sender, e);
            //Console.WriteLine(e.CurrentPage.ToString() + " / " + e.TotalPages.ToString());
        }// private void processor_Processing(object sender, GhostscriptProcessorProcessingEventArgs e)

        ///<summary>Add PDF open switch</summary>
        private void AddPageSwitch(int pageNum)
        {
            m_switches.Insert(8,"-dFirstPage=" + pageNum);
            m_switches.Insert(9,"-dLastPage=" + pageNum);
            m_switches[10] = @"-sOutputFile=" + m_outputPath + "\\"+pageNum.ToString("D4")+".tif";
        }// private void AddPageSwitch(int pageNum)

        ///<summary>Remove Page Switch</summary>
        private void RemovePageSwitch()
        {
            if (m_switches[8].Contains("-dFirstPage="))
            {
                m_switches.RemoveAt(8);
                m_switches.RemoveAt(8);
            }
        }// private void RemovePageSwitch()

        #endregion Private Methods

        #region Properties

        #endregion Properties

    }// public class CTmaxGsPdfManager
}// namespace FTI.Trialmax.Database
