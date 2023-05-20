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
    internal class WebBrowserMemoryFS : IWebBrowserMemoryFS
    {
        private readonly WebBrowser browser;

        public WebBrowserMemoryFS(WebBrowser browser)
            : base()
        {
            this.browser = browser;
        }

        public static byte[] ReadFully(Stream input)
        {
            using MemoryStream memoryStream = new ();
            input.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public void Init(string schemeName)
        {
            browser.DoCommand("MemoryScheme.Init", "memory");
        }

        public void RemoveFile(string filename)
        {
            Native.MemoryFSHandler.RemoveFile(filename);
        }

        public void AddTextFileWithMimeType(
            string filename,
            string textdata,
            string mimetype)
        {
            Native.MemoryFSHandler.AddTextFileWithMimeType(filename, textdata, mimetype);
        }

        public void AddTextFile(string filename, string textdata)
        {
            Native.MemoryFSHandler.AddTextFile(filename, textdata);
        }

        public void AddFile(string filename, IntPtr binarydata, int size)
        {
            Native.MemoryFSHandler.AddFile(filename, binarydata, size);
        }

        public void AddFileWithMimeType(string filename, IntPtr binarydata, int size, string mimetype)
        {
            Native.MemoryFSHandler.AddFileWithMimeType(filename, binarydata, size, mimetype);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063", Justification = "ok")]
        public void AddStreamWithMimeType(string filename, Stream stream, string mimetype)
        {
            byte[] data = ReadFully(stream);

            using (AutoPinner ap = new (data))
            {
                IntPtr unmanagedIntPtr = ap;
                AddFileWithMimeType(filename, unmanagedIntPtr, data.Length, mimetype);
            }
        }

        public void AddOSFileWithMimeType(string fsFilename, string osFilename, string mimetype)
        {
            using FileStream stream = File.OpenRead(osFilename);
            AddStreamWithMimeType(fsFilename, stream, mimetype);
        }

        public void AddOSFile(string fsFilename, string osFilename)
        {
            using FileStream stream = File.OpenRead(osFilename);
            AddStream(fsFilename, stream);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063", Justification = "ok")]
        public void AddStream(string filename, Stream stream)
        {
            byte[] data = ReadFully(stream);

            using (AutoPinner ap = new (data))
            {
                IntPtr unmanagedIntPtr = ap;
                AddFile(filename, unmanagedIntPtr, data.Length);
            }
        }

        private class AutoPinner : IDisposable
        {
            private GCHandle pinnedArray;

            public AutoPinner(object obj)
            {
                pinnedArray = GCHandle.Alloc(obj, GCHandleType.Pinned);
            }

            public static implicit operator IntPtr(AutoPinner ap)
            {
                return ap.pinnedArray.AddrOfPinnedObject();
            }

            public void Dispose()
            {
                pinnedArray.Free();
            }
        }
    }
}