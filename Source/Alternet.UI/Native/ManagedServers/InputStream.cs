using System;
using System.IO;

namespace Alternet.UI.Native.ManagedServers
{
    internal partial class InputStream
    {
        Stream stream;

        public InputStream(Stream stream)
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

        public IntPtr Read(byte[] buffer, IntPtr length)
        {
            return new IntPtr(stream.Read(buffer, 0, length.ToInt32()));
        }
    }
}