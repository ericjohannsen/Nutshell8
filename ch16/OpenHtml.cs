using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ch16
{
    public class OpenHtml
    {
        static public void Run()
        {
            WebClient wc = new WebClient { Proxy = null };
            wc.DownloadFile("http://www.albahari.com/nutshell/code.aspx", "code.htm");

            OpenHtmlFile("code.htm");

        }
        static void OpenHtmlFile(string location)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo("cmd", $"/c start {location}"));
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", location); // Desktop Linux
            else throw new Exception("Platform-specific code needed to open URL.");
        }
    }
}