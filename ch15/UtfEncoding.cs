using System;
using System.IO;
using System.Text;

namespace ch15
{
    public class UtfEncoding
    {
        static public void Run()
        {
            Console.WriteLine ("Default UTF-8 encoding");

            using (TextWriter w = File.CreateText ("but.txt"))    // Use default UTF-8 encoding.
            w.WriteLine ("but–");                               // Emdash, not the "minus" character

            using (Stream s = File.OpenRead ("but.txt"))
            for (int b; (b = s.ReadByte()) > -1;)
                Console.WriteLine (b);

            Console.WriteLine("Unicode a.k.a. UTF-16 encoding");

            using (Stream s = File.Create ("but.txt"))
            using (TextWriter w = new StreamWriter (s, Encoding.Unicode))
            w.WriteLine ("but–");

            foreach (byte b in File.ReadAllBytes ("but.txt"))
            Console.WriteLine (b);

            File.Delete("but.txt");
        }

    }
}