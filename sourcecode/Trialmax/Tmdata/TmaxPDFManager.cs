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

        /// <summary>Local member that will use LtManager</summary>
        private CTmaxLtPdfManager LtManager = null;

        /// <summary>Local member that will use GsManager</summary>
        private CTmaxGsPdfManager GsManager = null;

        #endregion Public Members
        
        #region Public Methods
        
        /// <summary>Constructor</summary>
        public CTmaxPDFManager(string _inputFile, string _ouputPath, TmaxPDFOutputType _outputType, short _customDPI)
        {
            m_InputFile = _inputFile;
            m_OutputPath = _ouputPath;
            m_OutputType = _outputType;
            m_CustomDPI = _customDPI;
        }// public TmaxPDFConversion()

        /// <summary>Main process which will start the conversion</summary>
        public bool StartConversion()
        {
            try
            {
                switch (m_OutputType)
                {
                    case TmaxPDFOutputType.Autodetect: return ConvertAutoDetect();
                    case TmaxPDFOutputType.ForceColor: return ConvertColor();
                    case TmaxPDFOutputType.ForceBW: return ConvertBW();
                    default: return ConvertAutoDetect();
                }
            }
            catch (ThreadAbortException Ex)
            {
                return false;
            }
        }// public bool StartConversion()

        /// <summary>Stop any running conversion process immediatelly</summary>
        public void StopConversionProcess()
        {
            switch (m_OutputType)
            {
                case TmaxPDFOutputType.Autodetect:
                    if (MuManager != null)
                        MuManager.StopProcess();
                    break;
                case TmaxPDFOutputType.ForceColor:
                    if (LtManager != null)
                        LtManager.StopProcess();
                    break;
                case TmaxPDFOutputType.ForceBW:
                    if (GsManager != null)
                        GsManager.StopProcess();
                    break;
                default: ConvertAutoDetect(); break;
            }
            CleanUp();
        }// public void StopConversionProcess()

        ///<summary>Return the list of errors if occured any</summary>
        public List<Exception> GetConversionErrorList()
        {
            List<Exception> ErrorList = new List<Exception>();
            switch (m_OutputType)
            {
                case TmaxPDFOutputType.Autodetect:
                    if (MuManager != null && MuManager.GetConversionErrorList() != null)
                        ErrorList.AddRange(MuManager.GetConversionErrorList());
                    break;
                case TmaxPDFOutputType.ForceColor:
                    if (LtManager != null && LtManager.GetConversionErrorList() != null)
                        ErrorList.AddRange(LtManager.GetConversionErrorList());
                    break;
                case TmaxPDFOutputType.ForceBW:
                    if (GsManager != null && GsManager.GetConversionErrorList() != null)
                        ErrorList.AddRange(GsManager.GetConversionErrorList());
                    break;
            }
            return ErrorList;
        }// public List<Exception> GetConversionErrorList()

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
                        Console.WriteLine("Exception while deleting temporary file = " + Ex.ToString());
                        //  Do Nothing
                    }
                }
            }
            catch (IOException Ex)
            {
                //  Do Nothing
            }
        }// private void CleanUp()

        /// <summary>This is called if AutoDetect is selected for conversion</summary>
        private bool ConvertAutoDetect()
        {
            MuManager = new CTmaxMuPdfManager(m_InputFile, m_OutputPath, m_CustomDPI);
            if (MuManager == null || (MuManager != null && MuManager.GetConversionErrorList() != null && MuManager.GetConversionErrorList().Count != 0))
                return false;
            MuManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return MuManager.Process();
        }// private bool ConvertAutoDetect()

        /// <summary>This is called if Color is selected for conversion</summary>
        private bool ConvertColor()
        {
            LtManager = new CTmaxLtPdfManager(m_InputFile, m_OutputPath, m_CustomDPI);
            if (LtManager == null || (LtManager != null && LtManager.GetConversionErrorList() != null && LtManager.GetConversionErrorList().Count != 0))
                return false;
            LtManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return LtManager.Process();
        }// private bool ConvertColor()

        /// <summary>This is called if BW is selected for conversion</summary>
        private bool ConvertBW()
        {
            GsManager = new CTmaxGsPdfManager(m_InputFile, m_OutputPath, m_CustomDPI);
            if (GsManager == null || (GsManager != null && GsManager.GetConversionErrorList() != null && GsManager.GetConversionErrorList().Count != 0))
                return false;
            GsManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return GsManager.Process();
        }// private bool ConvertBW()

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
