using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using Ghostscript.NET;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using Tmdata;
using System.Drawing.Imaging;

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

        private bool isExtracted = false;

        private ArrayList arrList = new ArrayList();

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
        public bool Process(short m_CustomDPI)
        {
            if (m_TotalPages == 0)
                return false;
            try
            {
                if (!DoConvert)
                    return false;
                string path = m_outputPath + "\\";
                var task1 = Task.Factory.StartNew(() => extractPNG());
                var task2 = Task.Factory.StartNew(() => ProcessPage(path,m_CustomDPI));
                Task.WaitAll(task1, task2);
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
            
            return true;
        }// public bool Process()

        private void extractPNG() 
        {
            try
            {
                m_GsManager.ExtractPNG();
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
            finally 
            {
                isExtracted = true;
            }
        }

        public void ProcessPage(string directory,short m_CustomDPI)
        {
            Dictionary<string, PNGFile> dic = new Dictionary<string, PNGFile>();
            bool runProcess = true;
            while (runProcess)
            {
                if (isExtracted)
                    runProcess = false;
                
                var files = Directory.GetFiles(directory,"*.png");
                if (files != null)
                {
                    //Console.WriteLine("Currently {0} files Extracted", files.Length);
                    foreach (var file in files)
                    {
                        if (!dic.ContainsKey(file))
                        {
                            PNGFile fl = new PNGFile();
                            fl.fileName = file;
                            dic.Add(file, fl);
                        }
                    }

                    var notProcessedFiles = dic.Values.Where(x => x.isProcessed == false).ToList();

                    if (notProcessedFiles.Count != 0)
                    {
                        if (runProcess) 
                        {
                            if (notProcessedFiles.Count > 1)
                            {
                                notProcessedFiles = notProcessedFiles.Take(notProcessedFiles.Count - 1).ToList();
                            }
                            else 
                            {
                                notProcessedFiles = new List<PNGFile>();
                            }
                        }
                        foreach (var png in notProcessedFiles)
                        {
                            string fileName = ProcessPNG(png.fileName, m_CustomDPI);
                            png.isProcessed = true;
                        }
                    }
                    else 
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
        }

        private string ProcessPNG(string fileName, short m_CustomDPI)
        {
            bool isColor = false;
            string fileNamePNG = fileName;
            string fileNameTiff= fileName.Replace(".png",".tif");

            try
            {
                // Copy as a dummy from original image
                using (System.Drawing.Image dummy = System.Drawing.Image.FromFile(fileNamePNG))
                {
                    // Initialize new bitmap
                    using (Bitmap bitmap = new Bitmap(dummy))
                    {
                        isColor = ColorPNG(bitmap);
                        if (!isColor)
                        {
                            // Convert it into a binary Tiff image
                            using (Bitmap destinationBitmap = bitmap.ConvertToMonochromeTiff())
                            {
                                //Check if Custom DPI has not been set 
                                if (m_CustomDPI == 0)
                                {
                                    destinationBitmap.SetResolution(300, 300);
                                }
                                else
                                {
                                    destinationBitmap.SetResolution(m_CustomDPI, m_CustomDPI);
                                }
                                destinationBitmap.Save(fileNameTiff, ImageFormat.Tiff);
                            }
                        }
                    }
                    // Call Garbage Collector to dispose of any unused handles.
                    GC.Collect();
                }
                // Delete the originals of converted images
                if (!isColor && System.IO.File.Exists(fileNamePNG))
                {
                    System.IO.File.Delete(fileNamePNG);
                }
                // Update status bar
                if (notifyPDFManager != null)
                {
                    notifyPDFManager(null, null);
                }
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }

            if (isColor)
            {
                return fileNamePNG;
            }
            else
            {
                return fileNameTiff;
            }
        
        }

        /// <summary>
        /// This function accepts a bitmap and then performs a delta
        /// comparison on all the pixels to find the highest delta
        /// color in the image. This calculation only works for images
        /// which have a field of similar color and some grayscale or
        /// near-grayscale outlines. The result ought to be that the
        /// calculated color is a sample of the "field". From this we
        /// can infer which color in the image actualy represents a
        /// contiguous field in which we're interested.
        /// See the documentation of GetRgbDelta for more information.
        /// </summary>
        /// <param name="bmp">A bitmap for sampling</param>
        /// <returns>The highest delta color</returns>
        public static bool ColorPNG(Bitmap bmp)
        {
            bool Color = false;
            int highestRgbDelta = 3;

            try
            {
                for (int x = 0; x < bmp.Width; x += 32)
                {
                    for (int y = 0; y < bmp.Height; y += 32)
                    {
                        if (GetRgbDelta(bmp.GetPixel(x, y)) >= highestRgbDelta)
                        {
                            Color = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }

            return Color;
        }

        /// <summary>
        /// Utility method that encapsulates the RGB Delta calculation:
        /// delta = abs(R-G) + abs(G-B) + abs(B-R) 
        /// So, between the color RGB(50,100,50) and RGB(128,128,128)
        /// The first would be the higher delta with a value of 100 as compared
        /// to the secong color which, being grayscale, would have a delta of 0
        /// </summary>
        /// <param name="color">The color for which to calculate the delta</param>
        /// <returns>An integer in the range 0 to 510 indicating the difference
        /// in the RGB values that comprise the color</returns>
        private static int GetRgbDelta(Color color)
        {
            return
                Math.Abs(color.R - color.G) +
                Math.Abs(color.G - color.B) +
                Math.Abs(color.B - color.R);
        }

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
            bool bresult;

            bresult = (bool)arrList[pageNum - 1];

            return bresult;
        }// private bool isColor(int pageNum)

        //MUPDF Obselete with current 64 implementation
        ///<summary>Redirects standard output of MuPDF to populate list of color detection</summary>
        private bool GetColor(int pageNum)
        {
            bool result = false;
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
                    proc.BeginErrorReadLine();

                    // Blocking until the process exits
                    proc.WaitForExit();
                    result = proc.StandardOutput.ReadToEnd().Contains("color");

                    if (notifyPDFManager != null)
                    {
                            notifyPDFManager(null, null);
                    }
                    //proc.WaitForExit();
                    proc.Close();
                    proc.Dispose();
                }
            }

            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }
            return result;
        }

        ///<summary>Initialize Leadtools object for color conversions</summary>
        private void PopulateColorArray()
        {
            bool MUresult;
            for (int i = 1; i <= m_TotalPages; i++)
            {
                MUresult = GetColor(i);
                arrList.Add(MUresult);
            }

        }

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
