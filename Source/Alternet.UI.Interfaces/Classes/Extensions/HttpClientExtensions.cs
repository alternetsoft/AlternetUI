using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="HttpClient"/>.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Downloads data async.
        /// </summary>
        /// <param name="client"><see cref="HttpClient"/> object used to download the data.</param>
        /// <param name="requestUrl">Url to download from.</param>
        /// <param name="destination">Destination stream.</param>
        /// <param name="progress">Progress notification provider. Optional.</param>
        /// <param name="cancellationToken">Cancellation Token. Optional.</param>
        /// <returns></returns>
        public static async Task DownloadDataAsync(
            this HttpClient client,
            string requestUrl,
            Stream destination,
            IProgress<float>? progress = null,
            CancellationToken cancellationToken = default)
        {
            using var response =
                await client.GetAsync(
                    requestUrl,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);
            var contentLength = response.Content.Headers.ContentLength;
            using var download = await response.Content.ReadAsStreamAsync();

            // no progress... no contentLength... very sad
            if (progress is null || !contentLength.HasValue)
            {
                await download.CopyToAsync(destination);
                return;
            }

            // Such progress and contentLength much reporting Wow!
            var progressWrapper = new Progress<long>(
                totalBytes => progress.Report(GetProgressPercentage(totalBytes, contentLength.Value)));
            await download.CopyToAsync(destination, 81920, progressWrapper, cancellationToken);

            static float GetProgressPercentage(float totalBytes, float currentBytes)
                => (totalBytes / currentBytes) * 100f;
        }
    }
}
