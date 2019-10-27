using System;
using System.Threading;

namespace ch22
{
    public class EventWait
    {
        static EventWaitHandle wh = new EventWaitHandle (false, EventResetMode.AutoReset,
                                        @"Global\MyCompany.MyApp.SomeName");        
        static public void AwaitSignal()
        {
            Console.WriteLine("Waiting for the signal.");
            wh.WaitOne();
            Console.WriteLine("I'm free...");
        }

        static public void SendSignal()
        {
            Console.WriteLine("Be free!");
            wh.Set();
        }
    }
}