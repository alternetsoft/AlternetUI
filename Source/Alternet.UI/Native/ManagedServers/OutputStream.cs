using System;
using System.IO;

namespace Alternet.UI.Native
{
    internal partial class OutputStream
    {
        private Stream stream;
        private readonly bool disposeStream;

        public OutputStream(Stream stream, bool disposeStream = false)
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

        public IntPtr Write(byte[] buffer, IntPtr length)
        {
            stream.Write(buffer, 0, length.ToInt32());
            return length;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (disposeStream)
                    stream.Dispose();
                stream = null!;
            }

            base.Dispose(disposing);
        }
    }
}