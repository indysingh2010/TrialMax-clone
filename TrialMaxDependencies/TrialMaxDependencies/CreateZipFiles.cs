﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Ionic.Zip;

namespace TrialMaxDependencies
{
    public class CreateZipFiles
    {
        public CreateZipFiles(string directory)
        {
            try
            { File.Delete(".lst"); }
            catch { }
            ProcessDirectory(directory);
        }

        // Process all files in the directory passed in, recurse on any directories  
        // that are found, and process the files they contain. 
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here. 
        public static void ProcessFile(string path)
        {
            try
            {
                //string line = Path.GetFileName(path)+","+(double)(fI.Length) / 1024 / 1024+","+ GetMD5HashFromFile(path);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".lst", true))
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddFile(path, "");
                        zip.Save(Path.GetFileNameWithoutExtension(path) + ".zip");
                        string newFile = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ".zip";
                        if (File.Exists(newFile))
                        { File.Delete(newFile); }
                        File.Move(Path.GetFileNameWithoutExtension(path) + ".zip", newFile);


                        FileInfo fI = new FileInfo(newFile);
                        file.WriteLine("{0},{1},{2}", newFile.Remove(0, @"D:\Dependencies\Dependencies\".Length), fI.Length, GetMD5HashFromFile(newFile));
                        try
                        {
                            File.Delete(path);
                        }
                        catch { }
                    }
                }

            }
            catch (Exception ex)
            { }
        }

        // Generate MD5 of a file
        public static string GetMD5HashFromFile(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }
    }
}
