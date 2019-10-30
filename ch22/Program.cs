using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ch22
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
            {"--mutex", new CmdInfo() { Command = (o) => { OneAtATimePlease.Run(); }}},
            {"--sem", new CmdInfo() { Command = (o) => { SemDemo.TryToGetIn(); }}},
            {"--eventwait", new CmdInfo() { Command = (o) => { EventWait.AwaitSignal(); }}},
            {"--eventsignal", new CmdInfo() { Command = (o) => { EventWait.SendSignal(); }}},
            //{"--downprog", new CmdInfo() { CommandAsync = async (o) => { await HttpClientProgress.RunAsync(); }}},
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
                if (cmdInfo.IsWindowsOnly && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
