using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Configuration;
using System.Net;
using System.Diagnostics;
using Ionic.Zip;
using Microsoft.Win32;

namespace TmaxDependencyDownloader
{
    public class Program
    {

        private DownloadManager _downloadManager = new DownloadManager();

        public DownloadManager DownloadManager { get { return _downloadManager; } }
        public static void Main(string[] args)
        {
            Program p = new Program();

            if (args.Length > 0 && args.FirstOrDefault().Equals("-p"))
            {
                p.GenerateEncryptedPassword();
                return;
            }
            
            LoggerManager.LogInfo("===================== Tmax Dependency Downloader Starting =====================");
            p.DownloadManager.StartProcess();

            Console.WriteLine("\nPress enter to exit.");
            Console.Read();
            return;
        }

        private void GenerateEncryptedPassword()
        {
            Console.WriteLine("Enter Password to encrypt:");
            string pass = "";
            char chr = (char)0;
            const int ENTER = 13;

            do
            {
                chr = System.Console.ReadKey(true).KeyChar;
                if (chr == '\b')
                {
                    if (pass.Length == 0) continue;
                    pass = pass.Remove(pass.Length - 1);
                    System.Console.Write("\b \b");
                }
                else if (chr == ENTER)
                {

                }
                else
                {
                    pass += (char)chr;
                    System.Console.Write("*");
                }
            } while (chr != ENTER);
            SimpleAES encryptDecrypt = new SimpleAES();
            string encryptedPass = encryptDecrypt.EncryptToString(pass);
            Console.WriteLine("\nEncrypted pass: {0}", encryptedPass);
            System.Console.WriteLine();
        }
    }
}
