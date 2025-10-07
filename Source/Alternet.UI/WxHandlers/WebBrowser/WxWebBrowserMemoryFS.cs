using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alternet.UI.Native;

namespace Alternet.UI
{
    internal class WxWebBrowserMemoryFS : IWebBrowserMemoryFS
    {
        private readonly WebBrowser browser;

        public WxWebBrowserMemoryFS(WebBrowser browser)
        {
            this.browser = browser;
        }

        public static byte[] ReadFully(Stream input)
        {
            using MemoryStream memoryStream = new ();
            input.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public bool Init(string schemeName)
        {
            browser.DoCommand("MemoryScheme.Init", "memory");
            return true;
        }

        public bool Remove(string filename)
        {
            Native.MemoryFSHandler.RemoveFile(filename);
            return true;
        }

        public bool AddString(
            string filename,
            string textdata,
            string? mimetype = null)
        {
            if(mimetype is null)
                Native.MemoryFSHandler.AddTextFile(filename, textdata);
            else
                Native.MemoryFSHandler.AddTextFileWithMimeType(filename, textdata, mimetype);
            return true;
        }

        public bool Add(string filename, IntPtr binarydata, int size, string? mimetype = null)
        {
            if(mimetype is null)
                Native.MemoryFSHandler.AddFile(filename, binarydata, size);
            else
                Native.MemoryFSHandler.AddFileWithMimeType(filename, binarydata, size, mimetype);
            return true;
        }

        public bool Add(string filename, Stream stream, string? mimetype = null)
        {
            byte[] data = ReadFully(stream);

            using AutoPinner ap = new(data);
            IntPtr unmanagedIntPtr = ap;
            return Add(filename, unmanagedIntPtr, data.Length, mimetype);
        }

        public bool Add(string fsFilename, string osFilename, string? mimetype = null)
        {
            using FileStream stream = File.OpenRead(osFilename);
            return Add(fsFilename, stream, mimetype);
        }
    }
}