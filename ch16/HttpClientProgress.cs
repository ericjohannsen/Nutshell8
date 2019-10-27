using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Net.Http;

namespace ch16
{
    public class HttpClientProgress
    {
        // https://github.com/dotnet/corefx/issues/21793
        static HttpClientHandler handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback  = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        

        // Only use a single instance for DNS caching and to avoid socket exhaustion        
        static HttpClient client = new HttpClient(handler); 

        static public async Task RunAsync()
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, value) => Console.Write("\r%{0:N0}", value);

            var cancellationToken = new CancellationTokenSource();

            using var destination = File.OpenWrite(Path.Combine(TempDirectory, "LINQPad6Setup.exe"));
            await DownloadFileAsync("https://www.linqpad.net/GetFile.aspx?LINQPad6Setup.exe", destination, progress, cancellationToken.Token);
            // Cleanup the test
            destination.Dispose(); // Release lock on the file
            File.Delete(Path.Combine(TempDirectory, "LINQPad6Setup.exe"));
        }

        // Based on: https://stackoverflow.com/q/21169573/141172
        //           https://stackoverflow.com/q/230128/141172

        
        static async Task CopyStreamWithProgressAsync(Stream input, Stream output, long total, IProgress<double> progress, CancellationToken token)
        {
            const int IO_BUFFER_SIZE = 8 * 1024; // Optimal size depends on your scenario

            // Expected size of input stream may be known from an HTTP header when reading from HTTP. Other streams may have their
            // own protocol for pre-reporting expected size.

            var canReportProgress = total != -1 && progress != null;
            var totalRead = 0L;
            byte[] buffer = new byte[IO_BUFFER_SIZE];
            int read;

            while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                token.ThrowIfCancellationRequested();
                await output.WriteAsync(buffer, 0, read);
                totalRead += read;
                if (canReportProgress)
                    progress.Report((totalRead * 1d) / (total * 1d) * 100);
            }
        }

        static async Task DownloadFileAsync(string url, Stream destination, IProgress<double> progress, CancellationToken token)
        {
            try
            {
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));

            var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;

            using var source = await response.Content.ReadAsStreamAsync();

            await CopyStreamWithProgressAsync(source, destination, total, progress, token);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string TempDirectory
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                    @"C:\Temp" : "/tmp";
        }

    }
}