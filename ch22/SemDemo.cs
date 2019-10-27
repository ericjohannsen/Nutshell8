using System;
using System.Threading;
using System.Diagnostics;

namespace ch22
{
    public class SemDemo
    {
        static Semaphore globalSem = new Semaphore(2, 2, @"Global\oreilly.com SemDemo");
        public static void TryToGetIn()
        {
            var pid = Process.GetCurrentProcess().Id;
            
            try
            {
                Console.WriteLine($"PID {pid} wants to get in.");
                globalSem.WaitOne();
                Console.WriteLine($"PID {pid} is in! Press ENTER to leave.");
                Console.ReadLine();
            }
            finally 
            {
                globalSem.Release();
            }
        }
    }
}