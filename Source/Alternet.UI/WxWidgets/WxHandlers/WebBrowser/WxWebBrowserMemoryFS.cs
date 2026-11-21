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
            NativeStringSpan.Invoke(filename, Native.MemoryFSHandler.RemoveFile);
            return true;
        }

        public bool AddString(
            string filename,
            string textdata,
            string? mimetype = null)
        {
            if(mimetype is null)
                NativeStringSpan.Invoke(filename, textdata, Native.MemoryFSHandler.AddTextFile);
            else
            {
                NativeUtils.Invoke(filename, textdata, mimetype, (s1, s2, s3) =>
                {
                    Native.MemoryFSHandler.AddTextFileWithMimeType(s1, s2, s3);
                });
            }
            return true;
        }

        public bool Add(string filename, IntPtr binarydata, int size, string? mimetype = null)
        {
            if(mimetype is null)
            {
                NativeStringSpan.Invoke(filename, span => Native.MemoryFSHandler.AddFile(span, binarydata, size));
            }
            else
            {
                NativeStringSpan.Invoke(filename, mimetype ,(span1, span2) =>
                {
                    Native.MemoryFSHandler.AddFileWithMimeType(span1, binarydata, size, span2);
                });
            }
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