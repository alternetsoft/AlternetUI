using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static partial class CommonUtils
    {
        public static async Task DownloadFileWithConsoleProgress(
            string docUrl,
            string filePath)
        {
            reportedProgress = -1;

            const string TempFileName = "4FE5F306CAB247D3877EE8193E031430";

            Console.WriteLine("Download started");
            Console.WriteLine($"Url: {docUrl}");
            Console.WriteLine($"Path: {filePath.Replace("\\","/").Replace("//","/")}");

            string tempFilePath = PathAddBackslash(Path.GetDirectoryName(filePath))+
                TempFileName;
            File.Delete(tempFilePath);
            File.Delete(filePath);

            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                await DownloadFile(docUrl, tempFilePath, DownloadFileProgressChangedToConsole);

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value. 
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                    ts.Hours, ts.Minutes, ts.Seconds);
                Console.WriteLine("Download Time: " + elapsedTime);

                File.Move(tempFilePath, filePath);
            }
            catch (Exception)
            {
                File.Delete(filePath);
                throw;
            }
        }

        public static async Task DownloadFile(string docUrl, string filePath, EventHandler<float> progressChanged)
        {
            var client = new HttpClient();

            var progress = new Progress<float>();
            progress.ProgressChanged += progressChanged;

            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                await client.DownloadDataAsync(docUrl, file, progress);
        }

        private static int reportedProgress = -1;
        public static void DownloadFileProgressChangedToConsole(object sender, float progress)
        {
            int p = (int)progress;
            if (p == reportedProgress)
                return;
            reportedProgress = p;
            Console.Write("\rDownloading {0}%   ", p.ToString());
        }
    }

}
