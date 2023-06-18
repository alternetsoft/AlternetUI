using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static partial class CommonUtils
    {
        public static async Task DownloadFile(string docUrl, string filePath, EventHandler<float> progressChanged)
        {
            var client = new HttpClient();

            var progress = new Progress<float>();
            progress.ProgressChanged += progressChanged;

            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                await client.DownloadDataAsync(docUrl, file, progress);
        }

        public static void DownloadFileProgressChangedToConsole(object sender, float progress)
        {
            Console.Write("\r{0}%   ", progress.ToString("0.00"));
        }
    }

}
