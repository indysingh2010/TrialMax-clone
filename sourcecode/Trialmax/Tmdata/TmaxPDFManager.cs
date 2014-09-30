using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FTI.Shared;
using FTI.Shared.Win32;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Shared.Database;

using Ghostscript.NET;
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
            switch (m_OutputType)
            {
                case TmaxPDFOutputType.Autodetect:  return ConvertAutoDetect();
                case TmaxPDFOutputType.ForceColor:  return ConvertColor();
                case TmaxPDFOutputType.ForceBW:     return ConvertBW();
                default:                            return ConvertAutoDetect();
            }
        }// public bool StartConversion()

        /// <summary>This is called if AutoDetect is selected for conversion</summary>
        private bool ConvertAutoDetect()
        {
            return true;
        }// private bool ConvertAutoDetect()

        /// <summary>This is called if Color is selected for conversion</summary>
        private bool ConvertColor()
        {
            return true;
        }// private bool ConvertColor()

        /// <summary>This is called if BW is selected for conversion</summary>
        private bool ConvertBW()
        {
            CTmaxGsPdfManager GsManager = new CTmaxGsPdfManager(m_InputFile, m_OutputPath, m_OutputType, m_CustomDPI);
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
