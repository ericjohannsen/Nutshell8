using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ch16
{
    public class TcpClientServer
    {
        static public void Run()
        {
            new Thread(Server).Start();       // Run server method concurrently.
            Thread.Sleep(500);                // Give server time to start.
            Client();
        }

        static void Client()
        {
            using (TcpClient client = new TcpClient("localhost", 51111))
            using (NetworkStream n = client.GetStream())
            {
                BinaryWriter w = new BinaryWriter(n);
                w.Write("Hello");
                w.Flush();
                Console.WriteLine(new BinaryReader(n).ReadString());
            }
        }

        static void Server()     // Handles a single client request, then exits.
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 51111);
            listener.Start();
            using (TcpClient c = listener.AcceptTcpClient())
            using (NetworkStream n = c.GetStream())
            {
                string msg = new BinaryReader(n).ReadString();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(msg + " right back!");
                w.Flush();                      // Must call Flush because we're not
            }                                 // disposing the writer.
            listener.Stop();
        }
    }
}
