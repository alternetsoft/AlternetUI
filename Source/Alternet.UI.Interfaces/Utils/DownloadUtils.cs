using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to file download using http protocol.
    /// </summary>
    public static class DownloadUtils
    {
        private static int reportedDownloadProgress = -1;

        /// <summary>
        /// Downloads file using async operation.
        /// </summary>
        /// <param name="docUrl">Url to download from.</param>
        /// <param name="filePath">Path to file.</param>
        /// <param name="progressChanged">Event to call on download progress.</param>
        /// <returns></returns>
        public static async Task DownloadFile(
            string docUrl,
            string filePath,
            EventHandler<float> progressChanged)
        {
            var client = new HttpClient();

            var progress = new Progress<float>();
            progress.ProgressChanged += progressChanged;

            using var file = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);
            await client.DownloadDataAsync(docUrl, file, progress);
        }

        /// <summary>
        /// Downloads file from the specified url and outputs progress to console
        /// or to callback actions.
        /// </summary>
        /// <param name="docUrl"></param>
        /// <param name="filePath"></param>
        /// <param name="writeLine">Callback action to use instead
        /// of <see cref="Console.WriteLine(string)"/></param>
        /// <param name="writeProgress">Callback action to use for progress outrut instead of
        /// <see cref="Console.WriteLine(string)"/></param>
        /// <returns></returns>
        public static async Task DownloadFileWithConsoleProgress(
            string docUrl,
            string filePath,
            Action<string>? writeLine = null,
            Action<string>? writeProgress = null)
        {
            void WriteLine(string s)
            {
                if (writeLine is not null)
                    writeLine(s);
                else
                    Console.WriteLine(s);
            }

            void WriteProgress(string s)
            {
                if (writeProgress is not null)
                    writeProgress(s);
                else
                    Console.Write($"\r{s}");
            }

            reportedDownloadProgress = -1;

            const string TempFileName = "4FE5F306CAB247D3877EE8193E031430";

            WriteLine("Download started");
            WriteLine($"Url: {docUrl}");
            WriteLine($"Path: {filePath.Replace("\\", "/").Replace("//", "/")}");

            string tempFilePath = PathUtils.AddDirectorySeparatorChar(Path.GetDirectoryName(filePath)) +
                TempFileName;
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                Stopwatch stopWatch = new();
                stopWatch.Start();

                await DownloadFile(docUrl, tempFilePath, DownloadFileProgressChangedToConsole);

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = string.Format(
                    "{0:00}:{1:00}:{2:00}",
                    ts.Hours,
                    ts.Minutes,
                    ts.Seconds);
                WriteLine("Download Time: " + elapsedTime);

                File.Move(tempFilePath, filePath);

                void DownloadFileProgressChangedToConsole(object? sender, float progress)
                {
                    int p = (int)progress;
                    if (p == reportedDownloadProgress)
                        return;
                    reportedDownloadProgress = p;
                    WriteProgress(string.Format("Downloading {0}%   ", p.ToString()));
                }
            }
            catch (Exception)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                throw;
            }
        }
    }
}
