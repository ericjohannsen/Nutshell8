using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ch15
{
    class Program
    {
        class CmdInfo
        {
            public Action<string> Command { get; set; }
            public Func<string, Task> CommandAsync { get; set; }
            public bool IsWindowsOnly { get; set; }
        }

        static Dictionary<string, CmdInfo> commands = new Dictionary<string, CmdInfo>()
            {
                {"--acl", new CmdInfo() { Command = (o) => { AccessControl.Run(); }, IsWindowsOnly = true }},
                {"--compress", new CmdInfo() { Command = (o) => { Compress.Run(); }, IsWindowsOnly = true }},
                {"--deflate", new CmdInfo() { Command = (o) => { Deflate.Run(); } }},
                {"--disk", new CmdInfo() { Command = (o) => { DiskInfo.Run(); } }},
                {"--fileinfo", new CmdInfo() { Command = (o) => { FileInfoUse.Run(); } }},
                {"--zip", new CmdInfo() { Command = (o) => { Zip.Run(); } }},
                {"--gzip", new CmdInfo() { CommandAsync = async (o) => { await GZipDemo.RunAsync(); } }},
                {"--open", new CmdInfo() { Command = (o) => { OpenAndWrite.Run(); } }},
                {"--encoding", new CmdInfo() { Command = (o) => { UtfEncoding.Run(); } }},
                {"--async", new CmdInfo() { Command = (o) => { AsyncDemo.Run(); } }},
                {"--streamrw", new CmdInfo() { Command = (o) => { StreamReaderWriter.Run(); } }},
                {"--filesec", new CmdInfo() { Command = (o) => { CheckFileSecurity.Run(); } }},
                {"--special", new CmdInfo() { Command = (o) => { SpecFolders.Run(); } }},
                {"--watch", new CmdInfo() { Command = (o) => { Watcher.Run(); } }},
                {"--memmap", new CmdInfo() { Command = (o) =>
                    { 
                    // To run the byte version from the command line, go to the project directory then:
                    // dotnet run -p ch15.csproj --memmap write
                    // dotnet run -p ch15.csproj --memmap read
                    // OR go to the build output directory and run the executable there with appropriate switches
                    if (o == "write")
                    {
                        MemMap.Write();
                    }
                    else if (o == "read")
                    {
                        MemMap.Read();
                    }
                    else if (o == "unsafe")
                    {
                        MemMap.Unsafe();
                    }
                    else throw new Exception($"Not supported: --pipe '{o}'");
                    }}},
                {"--pipe", new CmdInfo() { Command = (o) =>
                    { 
                    // To run the byte version from the command line, go to the project directory then:
                    // dotnet run -p ch15.csproj --pipe client
                    // dotnet run -p ch15.csproj --pipe server
                    // OR go to the build output directory and run the executable there with appropriate switches
                    if (o == "client")
                    {
                        NamedPipe.Client();
                    }
                    else if (o == "server")
                    {
                        NamedPipe.Server();
                    }
                    else throw new Exception($"Not supported: --pipe '{o}'");
                    }}},
                {"--msgpipe", new CmdInfo()  { Command = (o) =>
                    { 
                        // NOTE: Message transmission mode is only supported on Windows.
                        // Other platforms throw PlatformNotSupportedException.
                        // To run the message version from the command line, go to the project directory then:
                        // dotnet run -p ch15.csproj --msgpipe client
                        // dotnet run -p ch15.csproj --msgpipe server
                        // OR go to the build output directory and run the executable there with appropriate switches
                        if (o == "client")
                        {
                            NamedPipe.MessageClient();
                        }
                        else if (o == "server")
                        {
                            NamedPipe.MessageServer();
                        }
                        else throw new Exception($"Not supported: --msgpipe '{o}'");
                    }, IsWindowsOnly = true }},
                {"--anonpipe", new CmdInfo() { Command = (o) =>
                    {
                        if (string.IsNullOrEmpty(o) || o == "server")
                        {
                            AnonPipe.Server();
                        }
                        else if (o.StartsWith("client ")) // The server automatically starts the client
                        {
                            var opts = o.Split(' ');
                            AnonPipe.Client(opts[1], opts[2]);
                        }
                        else throw new Exception($"Not supported: --anonpipe '{o}'");
                    }}},
            };

        static void Help()
        {
            Console.WriteLine(string.Join(Environment.NewLine, commands.Keys.OrderBy(c => c)));
        }

        static async Task Main(string[] args)
        {
            string cmd = ""; // You can hard-code a command and option while testing
            string opt = "";

            if (cmd == "")
            {
                if (args.Length == 0)
                {
                    bool seekingHelp = false;
                    do
                    {
                        seekingHelp = false;
                        Console.WriteLine("Please enter an argument and option to specify which code to run.");
                        var input = Console.ReadLine().Split(' ');
                        cmd = input[0];
                        if (cmd == "--help" || cmd == "-h")
                        {
                            seekingHelp = true;
                            Help();
                        }
                        if (input.Length > 1) opt = string.Join(' ', input.Skip(1));
                    } while (seekingHelp);
                }
                else
                {
                    cmd = args[0];
                    if (args.Length > 1) opt = string.Join(' ', args.Skip(1));
                }
            }

            if (commands.TryGetValue(cmd, out CmdInfo cmdInfo))
            {                
                if (cmdInfo.IsWindowsOnly && !RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
                {
                    Console.WriteLine("This command is Windows-only.");
                    return;
                }
                if (cmdInfo.CommandAsync != null)
                    await cmdInfo.CommandAsync(opt);
                else
                    cmdInfo.Command(opt);
            }
            else Help();
        }
    }
}

