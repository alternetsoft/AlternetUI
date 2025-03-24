using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Stream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Copies contents of the stream to another stream using async operations.
        /// </summary>
        /// <param name="source">Source stream.</param>
        /// <param name="destination">Destination stream.</param>
        /// <param name="bufferSize">Buffer size.</param>
        /// <param name="progress">Progress notification provider. Optional.</param>
        /// <param name="cancellationToken">Cancellation Token. Optional.</param>
        /// <returns></returns>
        public static async Task CopyToAsync(
            this Stream source,
            Stream destination,
            int bufferSize,
            IProgress<long>? progress = null,
            CancellationToken cancellationToken = default)
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new InvalidOperationException($"'{nameof(source)}' is not readable.");
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new InvalidOperationException($"'{nameof(destination)}' is not writable.");

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer, 0, bufferSize, cancellationToken)
                .ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(
                    buffer,
                    0,
                    bytesRead,
                    cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}
