using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    public class MemoryFSHandler
    {
        public static void RemoveFile(NativeStringSpan filename) => throw new Exception();
        public static void AddTextFileWithMimeType(NativeStringSpan filename, NativeStringSpan textdata,
            NativeStringSpan mimetype) => throw new Exception();
        public static void AddTextFile(NativeStringSpan filename, NativeStringSpan textdata) => throw new Exception();
        public static void AddFile(NativeStringSpan filename, IntPtr binarydata, int size) => throw new Exception();
        public static void AddFileWithMimeType(NativeStringSpan filename, 
            IntPtr binarydata, int size, NativeStringSpan mimetype) => throw new Exception();
    }
}
