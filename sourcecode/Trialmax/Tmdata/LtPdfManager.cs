using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Leadtools;
//using Leadtools.Codecs;
//using Leadtools.ImageProcessing;
//using Leadtools.ImageProcessing.Color;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Trialmax
{
    /// <summary>This class manages the import operations using Leadtools</summary>
    public class CLtPdfManager
    {
        #region Constants

        #endregion Constants

        #region Private Members
        
        /// <summary> Notify Parent to update Progress bar on a single task completion </summary>
        public event EventHandler notifyExited;

        /// <summary>Local member used display registration progress form</summary>
        private CFRegProgress m_cfRegisterProgress = null;

        /// <summary>Local path where to output files</summary>
        private string m_outputPath = string.Empty;

        /// <summary>Local path where the document is present</summary>
        private string m_documentPath = string.Empty;

        /// <summary>Name of the document that is to be loaded</summary>
        private string m_documentName = string.Empty;

        /// <summary>Indicate if auto detect color, grayscale or colored for output files /// </summary>
        private TmaxPDFOutputType m_outputMode;

        /// <summary>Number of threads to be used. If set to 0, auto manage</summary>
        private short m_totalThreads;

        /// <summary>Codecs that will load the pages </summary>
        //private RasterCodecs m_codecs = null;

        ///// <summary>Information about the input PDF</summary>
        //private CodecsImageInfo m_pdfInfo = null;

        ///// <summary>ByteOrder in which the PDF is to be loaded</summary>
        //private CodecsLoadByteOrder m_byteOrder;

        ///// <summary>BitsPerPixel in which the PDF is to be loaded</summary>
        //private short m_bitsPerPixel;

        //private bool InitializeRasterCodecs()
        //{
        //    m_codecs = new RasterCodecs();
        //    m_codecs.SaveImage += 
        //    if (m_codecs != null)
        //        return true;

        //    return false;
        //}

        //private bool InitializePdfInfo()
        //{
        //    if (string.IsNullOrEmpty(m_documentPath) && string.IsNullOrEmpty(m_documentName) )
        //        m_pdfInfo = m_codecs.GetInformation(m_documentPath, true);

        //    if (m_pdfInfo != null)
        //        return true;
            
        //    return false;
        //}

        //#endregion Private Members

        //#region Public Methods

        //public CLtPdfManager(string docPath, string docName, string outPath, short totThreads = 0, TmaxPDFOutputType outMode = TmaxPDFOutputType.Autodetect)
        //{
        //    m_documentPath = docPath;
        //    m_documentName = docName;
        //    m_outputPath = outPath;
        //    m_outputMode = outMode;
        //}

        //private bool Initialize()
        //{
        //    if (InitializeRasterCodecs() && InitializePdfInfo())
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public bool IsColor(RasterImage img)
        //{
        //    ColorCountCommand command = new ColorCountCommand();
        //    command.Run(img);

        //    if (command.ColorCount > 2 && command.ColorCount <= 256) //could be gray
        //    {
        //        ColorResolutionCommand colorRes = new ColorResolutionCommand(ColorResolutionCommandMode.InPlace, 8,
        //           RasterByteOrder.Bgr, RasterDitheringMethod.None, ColorResolutionCommandPaletteFlags.Optimized, null);
        //        colorRes.Run(img);
        //        if (img.GrayscaleMode == RasterGrayscaleMode.None)
        //            return false;//MessageBox.Show("image is NOT grayscale");
        //        else
        //            return true;//MessageBox.Show("image is grayscale, its mode is: " + image.GrayscaleMode);
        //    }
        //    else
        //    {
        //        return false;// MessageBox.Show("image is NOT grayscale");
        //    }  
        //}

        //public void ConvertPdf()
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        switch (m_outputMode)
        //        {
        //            case 0:             // auto detect color
        //                m_bitsPerPixel = 24;
        //                m_byteOrder = CodecsLoadByteOrder.BgrOrGray;
        //                ConvertAutoDetect();
        //                break;
        //            case 1:             // force colored
        //                m_bitsPerPixel = 24;
        //                m_byteOrder = CodecsLoadByteOrder.Bgr;
        //                ConvertToColor();
        //                break;
        //            case 2:             // force gray
        //                m_bitsPerPixel = 8;
        //                m_byteOrder = CodecsLoadByteOrder.Gray;
        //                ConvertToGrayscale();
        //                break;
        //            default:
        //                break;
        //        }
        //    });
        //}

        //public bool ConvertToColor()
        //{
        //    ParallelOptions po = new ParallelOptions();
        //    if (m_totalThreads != 0)
        //        po.MaxDegreeOfParallelism = m_totalThreads;
        //    int count = 0;
        //    Parallel.For(1, m_pdfInfo.TotalPages, po, i =>
        //    {
        //        RasterImage image = m_codecs.Load(m_documentPath, 8, CodecsLoadByteOrder.Gray, i, i);
        //        m_codecs.Save(image, m_outputPath + "/" + m_documentName + (i) + ".png", RasterImageFormat.Png, 24, i, i, 1, CodecsSavePageMode.Overwrite);
        //        count++;
        //    });
        //    return (count == m_pdfInfo.TotalPages);
        //}

        //public bool ConvertToGrayscale()
        //{
        //    ParallelOptions po = new ParallelOptions();
        //    if (m_totalThreads != 0)
        //        po.MaxDegreeOfParallelism = m_totalThreads;
        //    int count = 0;
        //    Parallel.For(1, m_pdfInfo.TotalPages, po, i =>
        //    {
        //        RasterImage image = m_codecs.Load(m_documentPath, 8, CodecsLoadByteOrder.Gray, i, i);
        //        if (IsColor(image))
        //            m_codecs.Save(image, m_outputPath + "/" + m_documentName + (i) + ".png", RasterImageFormat.Png, 24, i, i, 1, CodecsSavePageMode.Overwrite);
        //        else
        //            m_codecs.Save(image, m_outputPath + "/" + m_documentName + (i) + ".tif", RasterImageFormat.TifJpeg411, 8, i, i, 1, CodecsSavePageMode.Overwrite);
        //        count++;
        //    });
        //    return (count == m_pdfInfo.TotalPages);
        //}

        //public bool ConvertAutoDetect()
        //{
        //    ParallelOptions po = new ParallelOptions();
        //    if (m_totalThreads != 0)
        //        po.MaxDegreeOfParallelism = m_totalThreads;
        //    int count = 0;
        //    Parallel.For(1, m_pdfInfo.TotalPages, po, i =>
        //    {
        //        RasterImage image = m_codecs.Load(m_documentPath, 8, m_byteOrder, i, i);
        //        if (IsColor(image))
        //            m_codecs.Save(image, m_outputPath + "/"+ m_documentName + (i) + ".png", RasterImageFormat.Png, 24, i, i, 1, CodecsSavePageMode.Overwrite);
        //        else
        //            m_codecs.Save(image, m_outputPath + "/" + m_documentName + (i) + ".tif", RasterImageFormat.TifJpeg411, 8, i, i, 1, CodecsSavePageMode.Overwrite);
        //        count++;
        //    });
        //    return (count == m_pdfInfo.TotalPages);
        //}

        #endregion Public Methods

        //#region Private Methods

        //#endregion Private Methods

        //#region Properties

        //#endregion Properties

    }// class CTmaxLeadtoolsManager

}// namespace FTI.Trialmax.Database
