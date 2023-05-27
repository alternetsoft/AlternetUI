using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    //-------------------------------------------------
    public class MemoryFSHandler
    {
        //-------------------------------------------------
        public static void RemoveFile(string filename) => throw new Exception();
        public static void AddTextFileWithMimeType(string filename, String textdata,
            string mimetype) => throw new Exception();
        public static void AddTextFile(string filename, string textdata) => throw new Exception();
        public static void AddFile(string filename, IntPtr binarydata, int size) => throw new Exception();
        public static void AddFileWithMimeType(string filename, 
            IntPtr binarydata, int size, string mimetype) => throw new Exception();
        //-------------------------------------------------
    }
    //-------------------------------------------------
}
//-------------------------------------------------
