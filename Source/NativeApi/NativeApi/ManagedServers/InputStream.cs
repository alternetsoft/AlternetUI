using ApiCommon;
using System;

namespace NativeApi.Api.ManagedServers
{
    public class InputStream
    {
        public long Length => throw new Exception();
        public bool IsOK => throw new Exception();
        public bool IsSeekable => throw new Exception();
        
        public IntPtr Read(
            [PInvokeAttributes("[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]")]byte[] buffer,
            IntPtr length) => throw new Exception();

        public long Position { get; set; }
    }
}