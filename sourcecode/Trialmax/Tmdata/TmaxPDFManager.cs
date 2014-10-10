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
    public class CTmaxPDFManager
    {
        /// <summary>Notify the RegOptionsForm to update the statusbar</summary>
        public event EventHandler notifyRegOptionsForm;

        /// <summary>Local member to store the PDF file name and location for conversion</summary>
        private string m_InputFile = string.Empty;

        /// <summary>Local member to store the output directory where the converted files will be stored</summary>
        private string m_OutputPath = string.Empty;
            
        /// <summary>Local member to store Output Type i.e. 0 = auto ; 1 = color ; 2 = black&white </summary>
        private TmaxPDFOutputType m_OutputType = TmaxPDFOutputType.Autodetect;

        /// <summary>Local member to store CustomDPI </summary>
        private short m_CustomDPI = 0;

        private CTmaxMuPdfManager MuManager = null;

        private CTmaxLtPdfManager LtManager = null;

        private CTmaxGsPdfManager GsManager = null;

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
                CleanUp();
                return false;
            }
        }// public bool StartConversion()

        private void CleanUp()
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
            try
            {
                Directory.Delete(m_OutputPath, true);

                //  We will check if the path contains the path of windows temp folder.
                //  If it does, we will assume that the pdf file was copied previously to the temp folder and needs to be deleted.
                string temporaryDirectory = @System.IO.Path.GetTempPath();
                if (m_InputFile.Contains(temporaryDirectory.ToLower()))
                {
                    File.Delete(m_InputFile);
                }
            }
            catch (IOException Ex)
            {
                //  Do Nothing
            }
        }

        /// <summary>This is called if AutoDetect is selected for conversion</summary>
        private bool ConvertAutoDetect()
        {
            MuManager = new CTmaxMuPdfManager(m_InputFile, m_OutputPath, m_CustomDPI);
            MuManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return MuManager.Process();
        }// private bool ConvertAutoDetect()

        /// <summary>This is called if Color is selected for conversion</summary>
        private bool ConvertColor()
        {
            LtManager = new CTmaxLtPdfManager(m_InputFile, m_OutputPath, m_CustomDPI);
            LtManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return LtManager.Process();
        }// private bool ConvertColor()

        /// <summary>This is called if BW is selected for conversion</summary>
        private bool ConvertBW()
        {
            GsManager = new CTmaxGsPdfManager(m_InputFile, m_OutputPath, m_CustomDPI);
            GsManager.notifyPDFManager += new EventHandler(UpdateRegStatusBar);
            return GsManager.Process();
        }// private bool ConvertBW()

        protected void UpdateRegStatusBar(object sender, EventArgs e)
        {
            if (notifyRegOptionsForm != null)
                notifyRegOptionsForm(sender, e);
        }
    }
}
