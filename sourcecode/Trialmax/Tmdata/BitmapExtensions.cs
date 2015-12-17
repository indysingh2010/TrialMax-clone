using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Tmdata
{
    public static class BitmapExtensions
    {
        #region P-Invoke
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hdcDst, int xDst, int yDst, int w, int h, IntPtr hdcSrc, int xSrc, int ySrc, int rop);
        static int SRCCOPY = 0x00CC0020;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO bmi, uint Usage, out IntPtr bits, IntPtr hSection, uint dwOffset);
        static uint BI_RGB = 0;
        static uint DIB_RGB_COLORS = 0;
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public uint biSize;
            public int biWidth, biHeight;
            public short biPlanes, biBitCount;
            public uint biCompression, biSizeImage;
            public int biXPelsPerMeter, biYPelsPerMeter;
            public uint biClrUsed, biClrImportant;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 256)]
            public uint[] cols;
        }

        static uint MAKERGB(int r, int g, int b)
        {
            return ((uint)(b & 255)) | ((uint)((r & 255) << 8)) | ((uint)((g & 255) << 16));
        }
        #endregion

        /// <summary>
        /// Converts the given bitmap to a monochrome tiff bitmap.
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap.</param>
        /// <returns></returns>
        public static Bitmap ConvertToMonochromeTiff(this Bitmap sourceBitmap)
        {
            Bitmap destination = null;
            int bpp = 1;                                                            // Amount of bits to use in pallet, for monochrome use 1 bit (0 = black, 1 = white).
            uint ncols = (uint)1 << bpp;                                            // Use 2 colours (black and white) for monochrome tiff.
            int width = sourceBitmap.Width;
            int height = sourceBitmap.Height;
            uint size = (uint)(((width + 7) & 0xFFFFFFF8) * height / 8);
            uint[] pallet = new uint[256];                                          // Pallet has a fixed size 256, even when we are using fewer colours.
            pallet[0] = MAKERGB(0, 0, 0);                                           // Create black pixel.
            pallet[1] = MAKERGB(255, 255, 255);                                     // Create white pixel.
            BITMAPINFO bmi = new BITMAPINFO()                                       // Create unmanaged monochrome bitmapinfo.
            {
                biSize = 40,                                                        // The size of the BITMAPHEADERINFO struct.
                biWidth = width,
                biHeight = height,
                biPlanes = 1,
                biBitCount = (short)bpp,                                            // Amount of bits per pixel (1 for monochrome).
                biCompression = BI_RGB,
                biSizeImage = size,
                biXPelsPerMeter = 1000000,
                biYPelsPerMeter = 1000000,
                biClrUsed = ncols,
                biClrImportant = ncols,
                cols = pallet
            };

            IntPtr sourceHbitmap = sourceBitmap.GetHbitmap();                       // Convert bitmap to unmanaged HBitmap.
            IntPtr bits0;                                                           // Pointer to the raw bits that make up the bitmap.
            IntPtr destinationHbitmap = CreateDIBSection
            (
                IntPtr.Zero,
                ref bmi,
                DIB_RGB_COLORS,
                out bits0,
                IntPtr.Zero,
                0
            );                                                                      // Create the indexed bitmap.
            IntPtr screenDC = GetDC(IntPtr.Zero);                                   // Obtain the DC (= GDI equivalent of "Graphics" in GDI+) for the screen.
            IntPtr sourceDC = CreateCompatibleDC(screenDC);                         // Create a DC for the original hbitmap.
            SelectObject(sourceDC, sourceHbitmap);
            IntPtr destinationDC = CreateCompatibleDC(screenDC);                    // Create a DC for the monochrome hbitmap.
            SelectObject(destinationDC, destinationHbitmap);
            BitBlt(destinationDC, 0, 0, width, height, sourceDC, 0, 0, SRCCOPY);    // Use GDI's BitBlt function to copy from original hbitmap into monocrhome bitmap.
            destination = System.Drawing.Bitmap.FromHbitmap(destinationHbitmap);    // Convert this monochrome hbitmap back into a Bitmap.

            // Cleanup.
            DeleteDC(sourceDC);
            DeleteDC(destinationDC);
            ReleaseDC(IntPtr.Zero, screenDC);
            DeleteObject(sourceHbitmap);
            DeleteObject(destinationHbitmap);

            return destination;
        }
    }
}
