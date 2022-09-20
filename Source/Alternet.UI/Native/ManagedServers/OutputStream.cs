using System;
using System.IO;

namespace Alternet.UI.Native
{
    internal partial class OutputStream
    {
        Stream stream;

        public OutputStream(Stream stream)
        {
            this.stream = stream;
        }

        public long Length => stream.Length;

        public bool IsOK => true; // todo

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
                stream.Dispose();
                stream = null!;
            }

            base.Dispose(disposing);
        }
    }
}