using System;
using System.Linq;
using System.IO;

namespace ch15
{
    public class StreamReaderWriter
    {
        static public void Run()
        {
            using (FileStream fs = File.Create("test.txt"))
            using (TextWriter writer = new StreamWriter(fs))
            {
                var nl = string.Join("", writer.NewLine.Select(c => $"0x{((int)c).ToString("X2")} "));
                Console.WriteLine($"Newline is {nl} ");
                writer.WriteLine("Line1");
                writer.WriteLine("Line2");
            }

            using (FileStream fs = File.OpenRead("test.txt"))
            using (TextReader reader = new StreamReader(fs))
            {
                Console.WriteLine(reader.ReadLine());       // Line1
                Console.WriteLine(reader.ReadLine());       // Line2
            }

            File.Delete("test.txt");
        }

    }

}