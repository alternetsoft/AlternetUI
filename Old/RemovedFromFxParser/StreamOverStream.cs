#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Syntax.Parsers.Lsp.Fx
{
    public class StreamOverStream : Stream
    {
        private readonly Stream inner;

        public StreamOverStream(Stream? inner = null)
        {
            this.inner = inner ?? new MemoryStream();
        }

        public event EventHandler<StreamReadWriteEventArgs>? AfterRead;

        public event EventHandler<StreamReadWriteEventArgs>? AfterWrite;

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
            var result = inner.Read(buffer, offset, count);
            AfterRead?.Invoke(this, new StreamReadWriteEventArgs(buffer, offset, count, result));
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var result = inner.Seek(offset, origin);
            return result;
        }

        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            inner.Write(buffer, offset, count);
            AfterWrite?.Invoke(this, new StreamReadWriteEventArgs(buffer, offset, count));
        }
    }
}
