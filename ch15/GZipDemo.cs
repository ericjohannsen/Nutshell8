using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ch15
{
    public class GZipDemo
    {
        static public async Task RunAsync()
        {
            var textfile = Path.Combine(TempDirectory, "myfile.txt");
            File.WriteAllText(textfile, RandomString(4096));
            var backup = textfile.Replace(".txt", "-Copy.txt");
            File.Copy(textfile, backup);

            await GZip(textfile);
            await GUnzip(textfile + ".gz", false); // Don't delete it so we can list it for comparison
            foreach (var fi in Directory
              .EnumerateFiles(TempDirectory, "myfile*")
              .Select(f => new FileInfo(f)))
                Console.WriteLine($"{fi.Name} {fi.Length} bytes");

            File.Delete(textfile);
            File.Delete(textfile + ".gz");
            File.Delete(backup);
        }

        static async Task GZip(string sourcefile, bool deleteSource = true)
        {
            var gzipfile = $"{sourcefile}.gz";
            if (File.Exists(gzipfile)) throw new Exception("Gzip file already exists");

            // Compress
            using (FileStream inStream = File.Open(sourcefile, FileMode.Open))
            using (FileStream outStream = new FileStream(gzipfile, FileMode.CreateNew))
            using (GZipStream gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                await inStream.CopyToAsync(gzipStream); // Or .CopyTo() for non-async code

            if (deleteSource) File.Delete(sourcefile);
        }

        static async Task GUnzip(string gzipfile, bool deleteGzip = true)
        {
            if (Path.GetExtension(gzipfile) != ".gz") throw new Exception("Not a gzip file");
            var uncompressedFile = gzipfile.Substring(0, gzipfile.Length - 3);
            if (File.Exists(uncompressedFile)) throw new Exception("Destination file already exists");

            // Uncompress
            using (FileStream uncompressToStream = File.Open(uncompressedFile, FileMode.Create))
            using (FileStream zipfileStream = File.Open(gzipfile, FileMode.Open))
            using (var unzipStream = new GZipStream(zipfileStream, CompressionMode.Decompress))
                await unzipStream.CopyToAsync(uncompressToStream); // Or .CopyTo() for non-async code

            if (deleteGzip) File.Delete(gzipfile);
        }

        static string TempDirectory
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                    @"C:\Temp" : "/tmp";
        }

        private static Random rnd = new Random();
        // https://stackoverflow.com/a/1344242/141172
        static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}