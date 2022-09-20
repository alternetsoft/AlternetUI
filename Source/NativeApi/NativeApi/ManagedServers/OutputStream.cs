using ApiCommon;
using System;

namespace NativeApi.Api.ManagedServers
{
    public class OutputStream
    {
        public long Length => throw new Exception();
        public bool IsOK => throw new Exception();
        public bool IsSeekable => throw new Exception();
        
        public IntPtr Write(
            [PInvokeAttributes("[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]")]byte[] buffer,
            IntPtr length) => throw new Exception();

        public long Position { get; set; }
    }
}