#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class StreamOverStream : Stream
    {
        private Stream inner;

        public StreamOverStream(Stream? inner = null)
        {
            this.inner = inner ?? new MemoryStream();
        }

        public static event EventHandler<StreamReadWriteEventArgs>? GlobalAfterRead;

        public static event EventHandler<StreamReadWriteEventArgs>? GlobalAfterWrite;

        public event EventHandler<StreamReadWriteEventArgs>? AfterRead;

        public event EventHandler<StreamReadWriteEventArgs>? AfterWrite;

        public Stream BaseStream
        {
            get => inner;
            set => inner = value;
        }

        public override bool CanRead => inner.CanRead;

        public override bool CanSeek => inner.CanSeek;

        public override bool CanWrite => inner.CanWrite;

        public override long Length => inner.Length;

        public override long Position
        {
            get => inner.Position;

            set => inner.Position = value;
        }

        public override void Flush()
        {
            inner.Flush();
        }

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

        public override long Seek(long offset, SeekOrigin loc)
        {
            return inner.Seek(offset, loc);
        }

        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }

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
