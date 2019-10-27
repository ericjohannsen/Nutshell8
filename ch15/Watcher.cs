using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ch15
{
    public class Watcher
    {
        static public void Run()
        {
            Watch(TempDirectory, "*.txt", true);
        }

        static void Watch(string path, string filter, bool includeSubDirs)
        {
            using (var watcher = new FileSystemWatcher(path, filter))
            {
                watcher.Created += FileCreatedChangedDeleted;
                watcher.Changed += FileCreatedChangedDeleted;
                watcher.Deleted += FileCreatedChangedDeleted;
                watcher.Renamed += FileRenamed;
                watcher.Error += FileError;

                watcher.IncludeSubdirectories = includeSubDirs;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Listening for events - press <enter> to end");
                Console.ReadLine();
            }
            // Disposing the FileSystemWatcher stops further events from firing.
        }

        static void FileCreatedChangedDeleted(object o, FileSystemEventArgs e)
          => Console.WriteLine("File {0} has been {1}", e.FullPath, e.ChangeType);

        static void FileRenamed(object o, RenamedEventArgs e)
          => Console.WriteLine("Renamed: {0}->{1}", e.OldFullPath, e.FullPath);

        static void FileError(object o, ErrorEventArgs e)
          => Console.WriteLine("Error: " + e.GetException().Message);

        static string TempDirectory
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                    @"C:\Temp" : "/tmp";
        }
    }
}