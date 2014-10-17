using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Leadtools;
using Leadtools.Codecs;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Database
{
    /// <summary>This class manages the import operations using Leadtools</summary>
    public class CTmaxLtPdfManager
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

        /// <summary>Codecs that will load the pages </summary>
        private RasterCodecs m_codecs = null;

        ///// <summary>Information about the input PDF</summary>
        private CodecsImageInfo m_pdfInfo = null;

        /// <summary>Resolution for Output images</summary>
        private short m_resolution;

        /// <summary>List containing errors if occured</summary>
        private List<Exception> m_conversionErrors = null;

        /// <summary>Flag that tells whether to convert files or not</summary>
        private volatile bool DoConvert = true;

        #endregion Private Members

        #region Public Members

        /// <summary> Notify Parent to update Progress bar on a single task completion </summary>
        public event EventHandler notifyPDFManager;
        
        #endregion Public Members

        #region Public Methods

        ///<summary>Constructor</summary>
        public CTmaxLtPdfManager(string docPath, string outPath, short outResolution = 0, short totThreads = 0)
        {
            m_documentNameWithPath = docPath;
            m_outputPath = outPath;
            m_totalThreads = totThreads;
            m_resolution = outResolution;
            InitializeLeadtools();
        }
        
        ///<summary>Start the conversion process using Leadtools</summary>
        public bool Process()
        {
                int pageCount = m_pdfInfo.TotalPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    if (!DoConvert || !ProcessPage(i))
                        return false;
                }
                return true;
        }

        ///<summary>Process a single page</summary>
        public bool ProcessPage(int pageNum)
        {
            try
            {
                RasterImage image = m_codecs.Load(m_documentNameWithPath, pageNum);
                m_codecs.Save(image, m_outputPath+ "\\\\" + pageNum.ToString("D4") + ".png", RasterImageFormat.Png, 0);
                return true;
            }
            catch (Exception Ex)
            {
                m_conversionErrors.Add(Ex);
                return false;
            }
        }

        /// <summary>Number of pages in the current loaded PDF</summary>
        public int GetTotalPages()
        {
            return m_pdfInfo.TotalPages;
        }

        /// <summary>Stop the current conversion process</summary>
        public void StopProcess()
        {
            DoConvert = false;
        }

        /// <summary>Returns the list of errors</summary>
        public List<Exception> GetConversionErrorList()
        {
            return m_conversionErrors;
        }

        #endregion Public Methods

        #region Private Methods

        private bool InitializeLeadtools()
        {
            m_conversionErrors = new List<Exception>();
            if (InitializeRasterCodecs())
            {
                if (InitializePdfInfo())
                {
                    return true;
                }
                else
                {
                    // Already handled in InitializePdfInfo()
                }
            }
            else
            {
                // Already handled in InitializeRasterCodecs()
            }
            return false;
        }

        private bool InitializeRasterCodecs()
        {
            try
            {
                m_codecs = new RasterCodecs();
                if (m_resolution == 0)
                {
                    m_codecs.Options.RasterizeDocument.Load.XResolution = 300;
                    m_codecs.Options.RasterizeDocument.Load.YResolution = 300;
                }
                else
                {
                    m_codecs.Options.RasterizeDocument.Load.XResolution = m_resolution;
                    m_codecs.Options.RasterizeDocument.Load.YResolution = m_resolution;
                }
                m_codecs.Options.Pdf.Load.DisplayDepth = 8;
                m_codecs.SavePage += new EventHandler<CodecsPageEventArgs>(codecs_SavePage);
            }
            catch (RasterException Ex)
            {
                m_conversionErrors.Add(Ex);
            }
            return (m_codecs != null);
        }

        private bool InitializePdfInfo()
        {
            try
            {
                m_pdfInfo = m_codecs.GetInformation(m_documentNameWithPath, true);
            }
            catch (RasterException Ex)
            {
                m_conversionErrors.Add(Ex);
                return false;
            }
            return (m_pdfInfo != null);
        }
 
        private void codecs_SavePage(object sender, CodecsPageEventArgs e)    
        {
            if (notifyPDFManager != null && e!= null && e.State == CodecsPageEventState.After)
                notifyPDFManager(sender, e);
        }

        #endregion Private Methods

        #region Properties

        #endregion Properties

    }// class CTmaxLeadtoolsManager

}// namespace FTI.Trialmax.Database
