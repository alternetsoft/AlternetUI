using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class SlicedStream : Stream
    {
        private readonly Stream baseStream;
        private readonly int from;

        public SlicedStream(Stream baseStream, int from, int length)
        {
            Length = length;
            this.baseStream = baseStream;
            this.from = from;
            this.baseStream.Position = from;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return baseStream.Read(buffer, offset, (int)Math.Min(count, Length - Position));
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                Position = offset;
            if (origin == SeekOrigin.End)
                Position = from + Length + offset;
            if (origin == SeekOrigin.Current)
                Position += offset;
            return Position;
        }

        public override void SetLength(long value) =>
            throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException();

        public override bool CanRead => true;

        public override bool CanSeek => baseStream.CanRead;

        public override bool CanWrite => false;

        public override long Length { get; }

        public override long Position
        {
            get => baseStream.Position - from;
            set => baseStream.Position = value + from;
        }

        public override void Close() => baseStream.Close();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                baseStream.Dispose();
        }
    }
}