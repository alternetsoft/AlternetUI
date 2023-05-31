using Alternet.UI.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    
    internal class WebBrowserMemoryFS : IWebBrowserMemoryFS
    {
        
        private readonly WebBrowser FBrowser;
        
        public WebBrowserMemoryFS(WebBrowser browser) :base()
        {
            FBrowser = browser;
        }
        
        public void Init(string schemeName) 
        {
            FBrowser.DoCommand("MemoryScheme.Init", "memory");
        }
        
        public void RemoveFile(string filename) 
        {
            Native.MemoryFSHandler.RemoveFile(filename);
        }
        
        public void AddTextFileWithMimeType(string filename, String textdata,
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
        
        public void AddFileWithMimeType(string filename,
            IntPtr binarydata, int size, string mimetype)
        {
            Native.MemoryFSHandler.AddFileWithMimeType(filename,
                binarydata, size, mimetype);
        }
        
        class AutoPinner : IDisposable
        {
            GCHandle _pinnedArray;
            public AutoPinner(Object obj)
            {
                _pinnedArray = GCHandle.Alloc(obj, GCHandleType.Pinned);
            }
            public static implicit operator IntPtr(AutoPinner ap)
            {
                return ap._pinnedArray.AddrOfPinnedObject();
            }
            public void Dispose()
            {
                _pinnedArray.Free();
            }
        }
        
        public static byte[] ReadFully(Stream input)
        {
            using MemoryStream memoryStream = new();
            input.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063", 
            Justification = "<Pending>")]
        public void AddStreamWithMimeType(string filename, Stream stream, string mimetype)
        {
            byte[] data = ReadFully(stream);

            using (AutoPinner ap = new(data))
            {
                IntPtr UnmanagedIntPtr = ap;
                AddFileWithMimeType(filename, UnmanagedIntPtr, data.Length, mimetype);
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
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063", 
            Justification = "<Pending>")]
        public void AddStream(string filename, Stream stream)
        {
            byte[] data = ReadFully(stream);

            using (AutoPinner ap = new(data))
            {
                IntPtr UnmanagedIntPtr = ap;
                AddFile(filename, UnmanagedIntPtr, data.Length);
            }
        }
        
    }
    
}

