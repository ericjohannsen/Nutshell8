using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace ch15
{
    struct Data { public int X, Y; }
    public class MemMap
    {
        static public void Write()
        {
            var file = Path.Combine(TempDirectory, "interprocess.bin");
            File.WriteAllBytes(file, new byte[100]);

            using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fs, null, fs.Length, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
            using MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

            accessor.Write(0, 12345);

            Console.WriteLine("Press a key to delete the memory mapped file.");
            Console.ReadLine();   // Keep shared memory alive until user hits Enter.

            File.Delete(file);
        }

        static public void Read()
        {
            // This can run in a separate executable:
            var file = Path.Combine(TempDirectory, "interprocess.bin");
            using FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fs, null, fs.Length, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
            using MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

            Console.WriteLine(accessor.ReadInt32(0));   // 12345
        }

        static public void Unsafe()
        {
            File.WriteAllBytes("unsafe.bin", new byte[100]);

            var data = new Data { X = 123, Y = 456 };

            using MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile("unsafe.bin");
            using MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

            accessor.Write(0, ref data);
            accessor.Read(0, out data);
            Console.WriteLine(data.X + " " + data.Y);   // 123 456

            unsafe
            {
                byte* pointer = null;
                try
                {
                    accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
                    int* intPointer = (int*)pointer;
                    Console.WriteLine(*intPointer);               // 123
                }
                finally
                {
                    if (pointer != null)
                        accessor.SafeMemoryMappedViewHandle.ReleasePointer();

                    File.Delete("unsafe.bin");
                }
            }
        }

        static string TempDirectory
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                    @"C:\Temp" : "/tmp";
        }


    }
}
