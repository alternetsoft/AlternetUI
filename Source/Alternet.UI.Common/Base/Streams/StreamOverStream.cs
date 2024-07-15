#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements stream which calls events after read/write operations.
    /// This class can be used for the debug purposes.
    /// </summary>
    public class StreamOverStream : Stream
    {
        private Stream inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamOverStream"/> class.
        /// </summary>
        /// <param name="inner">Stream used to perform read/write operations.</param>
        public StreamOverStream(Stream? inner = null)
        {
            this.inner = inner ?? new MemoryStream();
        }

        /// <summary>
        /// Occurs after read operation is performed in any <see cref="StreamOverStream"/> object.
        /// </summary>
        public static event EventHandler<StreamReadWriteEventArgs>? GlobalAfterRead;

        /// <summary>
        /// Occurs after write operation is performed in any <see cref="StreamOverStream"/> object.
        /// </summary>
        public static event EventHandler<StreamReadWriteEventArgs>? GlobalAfterWrite;

        /// <summary>
        /// Occurs after read operation is performed in this object.
        /// </summary>
        public event EventHandler<StreamReadWriteEventArgs>? AfterRead;

        /// <summary>
        /// Occurs after write operation is performed in this object.
        /// </summary>
        public event EventHandler<StreamReadWriteEventArgs>? AfterWrite;

        /// <summary>
        /// Gets or sets stream used to perform read/write operations.
        /// </summary>
        public virtual Stream BaseStream
        {
            get => inner;
            set => inner = value;
        }

        /// <inheritdoc/>
        public override bool CanRead => inner.CanRead;

        /// <inheritdoc/>
        public override bool CanSeek => inner.CanSeek;

        /// <inheritdoc/>
        public override bool CanWrite => inner.CanWrite;

        /// <inheritdoc/>
        public override long Length => inner.Length;

        /// <inheritdoc/>
        public override long Position
        {
            get => inner.Position;

            set => inner.Position = value;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            inner.Flush();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                var result = inner.Read(buffer, offset, count);
                var args = new StreamReadWriteEventArgs(buffer, offset, count, result);
                GlobalAfterRead?.Invoke(this, args);
                AfterRead?.Invoke(this, args);
                return result;
            }
            finally
            {
            }
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin loc)
        {
            return inner.Seek(offset, loc);
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                inner.Write(buffer, offset, count);
                var args = new StreamReadWriteEventArgs(buffer, offset, count);
                GlobalAfterWrite?.Invoke(this, args);
                AfterWrite?.Invoke(this, args);
            }
            finally
            {
            }
        }
    }
}
