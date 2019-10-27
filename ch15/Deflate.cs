using System;
using System.IO;
using System.IO.Compression;

namespace ch15
{
    public class Deflate
    {
        static public void Run()
        {
            Console.WriteLine("Deflate with non-repetitive data:");
            Deflate_NoRepetition();

            Console.WriteLine("Deflate with repetitive (better compressable) data:");
            Deflate_Repetition();
        }
        static void Deflate_NoRepetition()
        {
            using (Stream s = File.Create("compressed.bin"))
            using (Stream ds = new DeflateStream(s, CompressionMode.Compress))
                for (byte i = 0; i < 100; i++)
                    ds.WriteByte(i);

            Console.WriteLine($"Size on disk: {new FileInfo("compressed.bin").Length}");

            using (Stream s = File.OpenRead("compressed.bin"))
            using (Stream ds = new DeflateStream(s, CompressionMode.Decompress))
                for (byte i = 0; i < 100; i++)
                    Console.WriteLine(ds.ReadByte());     // Writes 0 to 99

            File.Delete("compressed.bin");
        }

        static async void Deflate_Repetition()
        {
            string[] words = "The quick brown fox jumps over the lazy dog".Split();
            Random rand = new Random();

            using (Stream s = File.Create("compressed.bin"))
            using (Stream ds = new DeflateStream(s, CompressionMode.Compress))
            using (TextWriter w = new StreamWriter(ds))
                for (int i = 0; i < 1000; i++)
                    await w.WriteAsync(words[rand.Next(words.Length)] + " ");

            Console.WriteLine(new FileInfo("compressed.bin").Length);      // 1073

            using (Stream s = File.OpenRead("compressed.bin"))
            using (Stream ds = new DeflateStream(s, CompressionMode.Decompress))
            using (TextReader r = new StreamReader(ds))
                Console.Write(await r.ReadToEndAsync());

            File.Delete("compressed.bin");
        }
    }
}