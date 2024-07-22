using System;
using System.IO;

namespace Alternet.UI.Native
{
    internal partial class InputStream
    {
        private readonly bool disposeStream;
        private Stream stream;

        public InputStream(Stream stream, bool disposeStream = false)
        {
            this.stream = stream;
            this.disposeStream = disposeStream;
        }

        public long Length => stream.Length;

        public bool IsOK => true;

        public bool IsSeekable => stream.CanSeek;

        public long Position
        {
            get => stream.Position;
            set => stream.Position = value;
        }

        public IntPtr Read(byte[] buffer, IntPtr length)
        {
            return new IntPtr(stream.Read(buffer, 0, length.ToInt32()));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(disposeStream)
                    stream.Dispose();
                stream = null!;
            }

            base.Dispose(disposing);
        }
    }
}