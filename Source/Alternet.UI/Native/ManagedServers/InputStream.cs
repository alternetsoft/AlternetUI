using System;

namespace Alternet.UI.Native.ManagedServers
{
    internal partial class InputStream
    {
        public long Length => throw new Exception();

        public bool IsOK => throw new Exception();

        public bool IsSeekable => throw new Exception();

        public long Position { get; set; }

        public IntPtr Read(byte[] buffer, IntPtr length) => throw new Exception();
    }
}