using NativeApi.Api.ManagedServers;
using System;

namespace NativeApi.Api
{
    public class UnmanagedDataObject
    {
        public string[] Formats => throw new Exception();

        public string GetStringData(string format) => throw new Exception();

        public string GetFileNamesData(string format) => throw new Exception();

        public UnmanagedStream GetStreamData(string format) => throw new Exception();

        public void SetStringData(string format, string value) => throw new Exception();

        public void SetFileNamesData(string format, string value) => throw new Exception();

        public void SetStreamData(string format, InputStream value) => throw new Exception();

        public bool GetDataPresent(string format) => throw new Exception();
    }
}