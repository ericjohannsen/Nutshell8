using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace ch15
{
    public class SpecFolders
    {
        static public void Run()
        {
            foreach (var val in Enum.GetValues(typeof(Environment.SpecialFolder)).Cast<Environment.SpecialFolder>().Distinct().OrderBy(v => v.ToString()))
            {
                Console.WriteLine($"{val}: {Environment.GetFolderPath(val)}");
            }
            Console.WriteLine($".NET Core directory: {RuntimeEnvironment.GetRuntimeDirectory()}");
        }
    }
}