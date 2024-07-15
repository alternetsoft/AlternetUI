using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the read/write events in <see cref="StreamOverStream"/> class.
    /// </summary>
    public class StreamReadWriteEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamReadWriteEventArgs"/> class.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which data storing begins.</param>
        /// <param name="count">The maximum number of bytes.</param>
        /// <param name="readCount">The number of bytes which was read.</param>
        public StreamReadWriteEventArgs(byte[] buffer, int offset, int count, int? readCount = null)
        {
            Buffer = buffer;
            Offset = offset;
            Count = count;
            ReadCount = readCount;
        }

        /// <summary>
        /// Gets or sets an array of bytes used in read/write operation.
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// Gets or sets the zero-based byte offset in buffer at which data storing begins.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of bytes for the read/write operation.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes which was read.
        /// </summary>
        public int? ReadCount { get; set; }

        /// <summary>
        /// Gets <see cref="Buffer"/> as UTF8 string.
        /// </summary>
        public virtual string BufferAsString
        {
            get
            {
                var result = System.Text.Encoding.UTF8.GetString(Buffer, Offset, ReadCount ?? Count);
                return result;
            }
        }
    }
}
