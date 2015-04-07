using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace TrialMaxDependencies
{
    public class CreateLstFile
    {
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
            FileInfo fI = new FileInfo(path);
            //string line = Path.GetFileName(path)+","+(double)(fI.Length) / 1024 / 1024+","+ GetMD5HashFromFile(path);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".lst", true))
            {
                file.WriteLine("{0},{1},{2}", path.Remove(0, @"D:\Dependencies\Dependencies\".Length), fI.Length, GetMD5HashFromFile(path));
            }
            //System.IO.File.WriteAllLines(@".lst", line);
            //Console.WriteLine("'{0}','{1}','{2}'", Path.GetFileName(path),(double)(fI.Length)/1024/1024, GetMD5HashFromFile(path));
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
