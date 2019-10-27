using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace ch15
{
    public class Zip
    {
        static public void Run()
        {
            AddFilesToFolder();
            
            var zipPath = Path.Combine(DirectoryToZip, "..", "archive.zip");
            ZipFile.CreateFromDirectory (DirectoryToZip, zipPath);
            
            Directory.CreateDirectory(DirectoryToExtractTo);
            ZipFile.ExtractToDirectory (zipPath, DirectoryToExtractTo);
            
            Console.WriteLine("Extracted files:");
            foreach (var file in Directory.EnumerateFiles(DirectoryToExtractTo))
            {
                Console.WriteLine(file);
                File.Delete(file); // Clean up
            }

            Directory.Delete(DirectoryToExtractTo);
            foreach (var file in Directory.EnumerateFiles(DirectoryToZip)) File.Delete(file);
            Directory.Delete(DirectoryToZip);
            File.Delete(zipPath);
        }

        static string DirectoryToZip
        {
            get
            {
                return RuntimeInformation.IsOSPlatform (OSPlatform.Windows) ?
                    @".\MyFolder" : "./MyFolder";
            }
        }

        static string DirectoryToExtractTo
        {
            get
            {
                return RuntimeInformation.IsOSPlatform (OSPlatform.Windows) ?
                    @".\Extracted" : "./Extracted";
            }
        }

        static void AddFilesToFolder()
        {
            Directory.CreateDirectory (DirectoryToZip);
            foreach (var c in new char[] { 'A', 'B', 'C' })
                File.WriteAllText (Path.Combine (DirectoryToZip, $"{c}.txt"), $"This is {c}");
        }

    

    }
}