using System;

namespace NativeApi.Api
{
    public class Clipboard
    {
        public UnmanagedDataObject GetDataObject() => throw new Exception();

        public void SetDataObject(UnmanagedDataObject value) => throw new Exception();

        public bool Flush() => default;

        public bool IsIntFormatSupported(int format) => default;

        public bool IsStrFormatSupported(string format) => default;
    }
}