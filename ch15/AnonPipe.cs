using System;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;

namespace ch15
{
    public class AnonPipe
    {
        static public void Client(string rxID, string txID)
        {
            using (var rx = new AnonymousPipeClientStream (PipeDirection.In, rxID))
            using (var tx = new AnonymousPipeClientStream (PipeDirection.Out, txID))
            {
                Console.WriteLine ("Client received: " + rx.ReadByte());
                tx.WriteByte (200);
            }

        }
        static public void Server()
        {
            HandleInheritability inherit = HandleInheritability.Inheritable;

            using var tx = new AnonymousPipeServerStream (PipeDirection.Out, inherit);
            using var rx = new AnonymousPipeServerStream (PipeDirection.In, inherit);

            string txID = tx.GetClientHandleAsString();
            string rxID = rx.GetClientHandleAsString();

            var args = $"--anonpipe client {txID} {rxID}";
            var startInfo = new ProcessStartInfo (Util.ExecutablePath, args);

            startInfo.UseShellExecute = false;      // Required for child process
            Process p = Process.Start (startInfo);

            tx.DisposeLocalCopyOfClientHandle();    // Release unmanaged
            rx.DisposeLocalCopyOfClientHandle();    // handle resources.

            tx.WriteByte (100);
            Console.WriteLine ("Server received: " + rx.ReadByte());

            p.WaitForExit();

        }

    }
}