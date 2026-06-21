using NativeApi.Api.ManagedServers;
using System;

namespace NativeApi.Api
{
    public class UnmanagedDataObject
    {
        public NativeStringSpan[] Formats => throw new Exception();

        public NativeStringSpan GetStringData(NativeStringSpan format) => throw new Exception();

        public NativeStringSpan GetFileNamesData(NativeStringSpan format) => throw new Exception();

        public UnmanagedStream GetStreamData(NativeStringSpan format) => throw new Exception();

        public void SetStringData(NativeStringSpan format, NativeStringSpan value) => throw new Exception();

        public void SetFileNamesData(NativeStringSpan format, NativeStringSpan value) => throw new Exception();

        public void SetStreamData(NativeStringSpan format, InputStream value) => throw new Exception();

        public bool GetDataPresent(NativeStringSpan format) => throw new Exception();

        public bool GetNativeDataPresent(int format) => throw new Exception();
    }
}