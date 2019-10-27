using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
namespace ch21
{
    public class UserInfo
    {
        [DllImport("libc")]
        public static extern uint getuid();

        static public void AmIAdmin()
        {
            Console.WriteLine($"Admin: {RunningAsAdmin()}");
        }
        static bool RunningAsAdmin()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using var identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            else return getuid() == 0;
        }
    }
}