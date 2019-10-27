using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace ch15
{
    public class NamedPipe
    {
        static public void Client()
        {
            using var s = new NamedPipeClientStream ("pipedream");

            s.Connect();
            Console.WriteLine (s.ReadByte());
            s.WriteByte (200);                 // Send the value 200 back.            
        }
        static public void MessageClient()
        {
            using var s = new NamedPipeClientStream ("pipedream");

            s.Connect();
            s.ReadMode = PipeTransmissionMode.Message;

            Console.WriteLine (Encoding.UTF8.GetString (ReadMessage (s)));

            byte[] msg = Encoding.UTF8.GetBytes ("Hello right back!");
            s.Write (msg, 0, msg.Length);
        }
        static public void Server()
        {
            using var s = new NamedPipeServerStream ("pipedream");

            Console.WriteLine ("Please start Named Pipe Client.");
            s.WaitForConnection();
            s.WriteByte (100);                // Send the value 100.
            Console.WriteLine ("Response from Named Pipe Client.");
            Console.WriteLine (s.ReadByte());            
        }

        static public void MessageServer()
        {
            using var s = new NamedPipeServerStream ("pipedream", PipeDirection.InOut,
                                                        1, PipeTransmissionMode.Message);

            s.WaitForConnection();

            byte[] msg = Encoding.UTF8.GetBytes ("Hello");
            s.Write (msg, 0, msg.Length);

            Console.WriteLine (Encoding.UTF8.GetString (ReadMessage (s)));

        }

        static byte[] ReadMessage (PipeStream s)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte [0x1000];      // Read in 4 KB blocks

            do { ms.Write (buffer, 0, s.Read (buffer, 0, buffer.Length)); }
            while (!s.IsMessageComplete);

            return ms.ToArray();
        }        
    }
}