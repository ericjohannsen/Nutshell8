using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ch15
{
    public class FileInfoUse
    {
        static public void Run()
        {
            Directory.CreateDirectory(TempDirectory);

            FileInfo fi = new FileInfo(Path.Combine(TempDirectory, "FileInfo.txt"));

            Console.WriteLine(fi.Exists);         // false

            using (TextWriter w = fi.CreateText())
                w.Write("Some text");

            Console.WriteLine(fi.Exists);         // false (still)
            fi.Refresh();
            Console.WriteLine(fi.Exists);         // true

            Console.WriteLine(fi.Name);           // FileInfo.txt
            Console.WriteLine(fi.FullName);       // c:\temp\FileInfo.txt (Windows)
                                                  // /tmp/FileInfo.txt (Unix)
            Console.WriteLine(fi.DirectoryName);  // c:\temp (Windows)
                                                  // /tmp (Unix)
            Console.WriteLine(fi.Directory.Name); // temp
            Console.WriteLine(fi.Extension);      // .txt
            Console.WriteLine(fi.Length);         // 9

            try
            {
                fi.Encrypt();
            }
            catch (PlatformNotSupportedException)
            {
                Console.WriteLine("Encryption not supported on this platform.");
                
            }
            fi.Attributes ^= FileAttributes.Hidden;   // (Toggle hidden flag)
            fi.IsReadOnly = true;

            Console.WriteLine(fi.Attributes);    // ReadOnly,Archive,Hidden,Encrypted
            Console.WriteLine(fi.CreationTime);  // 3/09/2019 1:24:05 PM

            fi.MoveTo(Path.Combine(TempDirectory, "FileInfoX.txt"));

            DirectoryInfo di = fi.Directory;
            Console.WriteLine(di.Name);             // temp or tmp
            Console.WriteLine(di.FullName);         // c:\temp or /tmp
            Console.WriteLine(di.Parent.FullName);  // c:\
            di.CreateSubdirectory("SubFolder");

            Cleanup();
        }


        static string TempDirectory
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                  @"C:\Temp" : "/tmp";
            }
        }
        // Clean up
        static void Cleanup()
        {
            var subfolder = Path.Combine(TempDirectory, "SubFolder");
            if (Directory.Exists(subfolder)) Directory.Delete(subfolder);
            var fi = new FileInfo(Path.Combine(TempDirectory, "FileInfo.txt"));
            if (fi.Exists)
            {
                fi.Attributes &= ~FileAttributes.Hidden;   // (Toggle hidden flag)
                fi.IsReadOnly = false;
                fi.Delete();
            }
            var fiX = new FileInfo(Path.Combine(TempDirectory, "FileInfoX.txt"));
            if (fiX.Exists)
            {
                fiX.Attributes &= ~FileAttributes.Hidden;   // (Toggle hidden flag)
                fiX.IsReadOnly = false;
                fiX.Delete();
            }
        }
    }
}