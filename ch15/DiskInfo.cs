using System;
using System.IO;
using System.Linq;

namespace ch15
{
    public class DiskInfo
    {
        static public void Run()
        {
            foreach (DriveInfo d in DriveInfo.GetDrives().OrderBy(d => d.Name))    // All defined drives.
            {
                Console.WriteLine(d.Name);             // C:\
                Console.WriteLine(d.DriveType);        // Fixed
                Console.WriteLine(d.RootDirectory);    // C:\

                if (d.IsReady)   // If the drive is not ready, the following two
                                 // properties will throw exceptions:
                {
                    Console.WriteLine($"Size: {d.TotalSize} Total free: {d.TotalFreeSpace} Available free: {d.AvailableFreeSpace}");
                    Console.WriteLine(d.VolumeLabel);    // The Sea Drive
                    Console.WriteLine(d.DriveFormat);    // NTFS
                }
                Console.WriteLine();
            }
        }
    }
}