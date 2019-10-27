using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ch15
{
    public class Util
    {
        static public string CsprojPath
        {
            get
            {
                var entryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var sep = Path.DirectorySeparatorChar;
                var csproj = Path.GetFullPath($@"{entryPath}{sep}..{sep}..{sep}..{sep}ch15.csproj");

                if (!File.Exists(csproj)) throw new Exception($"Can't find the csproj file at {csproj}");
                
                return csproj;
            }
        }

        static public string ExecutablePath
        {
            get
            {
                var entryPath = Assembly.GetEntryAssembly().Location;
                var executablePath = Path.Combine(Path.GetDirectoryName(entryPath), Path.GetFileNameWithoutExtension(entryPath));
                if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows)) executablePath += ".exe";
                return executablePath;
            }
        }

    }
}