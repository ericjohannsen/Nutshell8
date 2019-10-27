using System.IO;
using System.Management;
using System.Runtime.InteropServices;

namespace ch15
{
    public class Compress
    {
        static public void Run()
        {
            CreateCompressStructure();

            CompressFolder(DirectoryToCompress, true);

            CleanupCompressStructure();
        }

        static void CreateCompressStructure()
        {
            Directory.CreateDirectory(DirectoryToCompress);
            File.WriteAllText(Path.Combine(DirectoryToCompress, "MyFile.txt"), "C# is fun!");
            var subfolder = Path.Combine(DirectoryToCompress, "Subfolder");
            Directory.CreateDirectory(subfolder);
            File.WriteAllText(Path.Combine(subfolder, "FileInSubfolder.txt"), ".NET Core rocks!");
        }

        static void CleanupCompressStructure()
        {
            var subfolder = Path.Combine(DirectoryToCompress, "Subfolder");
            File.Delete(Path.Combine(subfolder, "FileInSubfolder.txt"));
            Directory.Delete(subfolder);

            File.Delete(Path.Combine(DirectoryToCompress, "MyFile.txt"));
            Directory.Delete(DirectoryToCompress);
        }

        static uint CompressFolder(string folder, bool recursive)
        {
            string path = "Win32_Directory.Name='" + folder + "'";

            using (ManagementObject dir = new ManagementObject(path))
            using (ManagementBaseObject p = dir.GetMethodParameters("CompressEx"))
            {
                p["Recursive"] = recursive;
                using (ManagementBaseObject result = dir.InvokeMethod("CompressEx",
                                                                        p, null))
                    return (uint)result.Properties["ReturnValue"].Value;
            }
        }

        static string DirectoryToCompress
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                @".\CompressMe" : "/tmp/CompressMe";
            }
        }
    }
}