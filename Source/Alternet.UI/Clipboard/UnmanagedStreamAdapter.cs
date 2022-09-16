using Alternet.UI.Native;
using System;
using System.IO;

namespace Alternet.UI
{
    internal class UnmanagedStreamAdapter : Stream
    {
        private UnmanagedStream stream;

        public UnmanagedStreamAdapter(UnmanagedStream stream)
        {
            this.stream = stream;
        }

        public override bool CanRead => true;

        public override bool CanSeek => stream.IsSeekable;

        public override bool CanWrite => false;

        public override long Length => stream.Length;

        public override long Position
        {
            get => stream.Position;
            set => Seek(value, SeekOrigin.Begin);
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var b = new byte[count - offset];
            var read = stream.Read(b, new IntPtr(count)).ToInt32();
            if (read > 0)
                Array.Copy(b, buffer, read);
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!stream.IsSeekable)
                throw new NotSupportedException();

            stream.Position = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length - offset,
                _ => throw new Exception(),
            };

            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}