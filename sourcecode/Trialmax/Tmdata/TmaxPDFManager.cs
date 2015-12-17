using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using FTI.Shared;
using FTI.Shared.Win32;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Shared.Database;

using FTI.Trialmax.Database;

namespace FTI.Shared.Database
{
    /// <summary>
    ///          This class is the Manager that does the PDF to Image conversion for all 3 categories
    ///          i.e. Force Black and White, Force Color, Auto Detect
    ///          For Color conversion, we are using Leadtools Imaging SDK Technology
    ///          For Black and White, we are using Ghostscript v9.14 and wrapper for C# i.e. Ghostscript.NET
    ///          For Color Detection, we are using MuPDF (MuDraw) .exe process
    ///          Incase the PDF Conversion is failed, perform Cleanup of any files that were already
    ///          converted but needs to deleted now as the process was cancelled or there was any error.
    /// </summary>
    public class CTmaxPDFManager
    {
        #region Public Members

        /// <summary>Notify the RegOptionsForm to update the statusbar</summary>
        public event EventHandler notifyRegOptionsForm;

        #endregion Public Members

        #region Private Members

        /// <summary>Local member to store the PDF file name and location for conversion</summary>
        private string m_InputFile = string.Empty;

        /// <summary>Local member to store the output directory where the converted files will be stored</summary>
        private string m_OutputPath = string.Empty;

        /// <summary>Local member to store Output Type</summary>
        private TmaxPDFOutputType m_OutputType = TmaxPDFOutputType.Autodetect;

        /// <summary>Local member to store CustomDPI </summary>
        private short m_CustomDPI = 0;

        /// <summary>Local member that will use MuManager</summary>
        private CTmaxMuPdfManager MuManager = null;

        ///// <summary>Local member that will use LtManager</summary>
        //private CTmaxLtPdfManager LtManager = null;

        /// <summary>Local member that will use GsManager</summary>
        private CTmaxGsPdfManager GsManager = null;

        /// <summary>Local member that will store if Custom Dither should be disabled</summary>
        private bool m_DisableCustomDither = false;

        /// <summary>Local variable to log detail errors with stacktrace</summary>
        private static readonly log4net.ILog logDetailed = log4net.LogManager.GetLogger("DetailedLog");

        /// <summary>Local variable to log user level details</summary>
        private static readonly log4net.ILog logUser = log4net.LogManager.GetLogger("UserLog");

        #endregion Private Members

        #region Public Methods

        /// <summary>Constructor</summary>
        public CTmaxPDFManager(string _inputFile, string _ouputPath, TmaxPDFOutputType _outputType, short _customDPI, bool _disableCustomDither)
        {
            m_InputFile = _inputFile;
            m_OutputPath = _ouputPath;
            m_OutputType = _outputType;
            m_CustomDPI = _customDPI;
            m_DisableCustomDither = _disableCustomDither;
        }// public TmaxPDFConversion()

        /// <summary>Main process which will start the conversion</summary>
        public bool StartConversion()
        {
            try
            {
                switch (m_OutputType)
                {
                    case TmaxPDFOutputType.Autodetect: return ConvertAutoDetect();
                    case TmaxPDFOutputType.ForceColor: 
                    case TmaxPDFOutputType.ForceBW: return ConvertGS();
                    default: return ConvertAutoDetect();
                }
            }
            catch (ThreadAbortException Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
            return false;
        }// public bool StartConversion()

        /// <summary>Stop any running conversion process immediatelly</summary>
        public void StopConversionProcess()
        {
            try
            {
                switch (m_OutputType)
                {
                    case TmaxPDFOutputType.Autodetect:
                        if (MuManager != null)
                            MuManager.StopProcess();
                        break;
                    case TmaxPDFOutputType.ForceColor:
                        //if (LtManager != null)
                        //    LtManager.StopProcess();
                        //break;
                    case TmaxPDFOutputType.ForceBW:
                        if (GsManager != null)
                            GsManager.StopProcess();
                        break;
                }
                CleanUp();
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
        }// public void StopConversionProcess()

        ///<summary>Release all resources used by this class</summary>
        public void Dispose()
        {
            try
            {
                switch (m_OutputType)
                {
                    case TmaxPDFOutputType.Autodetect:
                        if (MuManager != null)
                        {
                            MuManager.Dispose();
                            MuManager = null;
                        }
                        break;
                    case TmaxPDFOutputType.ForceColor:
                        //if (LtManager != null)
                        //{
                        //    LtManager.Dispose();
                        //    LtManager = null;
                        //}
                        //break;
                    case TmaxPDFOutputType.ForceBW:
                        if (GsManager != null)
                        {
                            GsManager.Dispose();
                            GsManager = null;
                        }
                        break;
                }
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
        }// public void Dispose()

        #endregion Public Methods

        #region Private Methods

        /// <summary>Clean any temporary files if PDF was stored in Temp folder and exported files since the conversion was stopped</summary>
        private void CleanUp()
        {
            try
            {
                //  We will check if the path contains the path of windows temp folder.
                //  If it does, we will assume that the pdf file was copied previously to the temp folder and needs to be deleted.
                string temporaryDirectory = @System.IO.Path.GetTempPath();
                if (m_InputFile.Contains(temporaryDirectory.ToLower()))
                {
                    int index = m_InputFile.IndexOf(temporaryDirectory.ToLower());
                    string cleanPath = (index < 0)
                        ? m_InputFile
                        : m_InputFile.Remove(index, temporaryDirectory.Length);

                    try
                    {
                        string parentPath = cleanPath;   // This will store the parent folder name in which the file was store if a folder was selected while importing
                        while (true)
                        {
                            string temp = Path.GetDirectoryName(parentPath);
                            if (String.IsNullOrEmpty(temp))
                                break;
                            parentPath = temp;
                        }
                        if (!string.IsNullOrEmpty(parentPath) && Directory.Exists(temporaryDirectory + parentPath))
                        {
                            Console.WriteLine("Deleting temporary file = " + temporaryDirectory + parentPath);
                            Directory.Delete(temporaryDirectory + parentPath, true);
                        }
                        else
                        {
                            File.Delete(m_InputFile);
                        }
                    }
                    catch (IOException Ex)
                    {
                        logDetailed.Error(Ex.ToString());
                        //  Do Nothing
                    }
                    catch (Exception Ex)
                    {
                        logDetailed.Error(Ex.ToString());
                    }
                }
            }
            catch (IOException Ex)
            {
                logDetailed.Error(Ex.ToString());
                //  Do Nothing
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
        }// private void CleanUp()

        /// <summary>This is called if AutoDetect is selected for conversion</summary>
        private bool ConvertAutoDetect()
        {
            MuManager = new CTmaxMuPdfManager(m_InputFile, m_OutputPath, m_CustomDPI, m_DisableCustomDither);
            if (MuManager == null)
                return false;
            MuManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return MuManager.Process(m_CustomDPI);
        }// private bool ConvertAutoDetect()

        private bool ConvertGS()
        {
            GsManager = new CTmaxGsPdfManager(m_InputFile, m_OutputPath, m_OutputType, m_CustomDPI, m_DisableCustomDither);
            if (GsManager == null)
                return false;
            GsManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return GsManager.Process();
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>This method notifies the parent to update the registration progress bar</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateRegStatusBar(object sender, EventArgs e)
        {
            if (notifyRegOptionsForm != null)
                notifyRegOptionsForm(sender, e);
        }// protected void UpdateRegStatusBar(object sender, EventArgs e)

        #endregion Protected Methods
    }
}
