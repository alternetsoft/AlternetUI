using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    //-------------------------------------------------
    public interface IWebBrowserMemoryFS
    {
        //-------------------------------------------------
        void Init(string schemeName);
        //-------------------------------------------------
        /// <summary>
        ///     Removes a file from memory file system and frees the occupied memory.
        /// </summary>
        /// <param name="filename">
        ///     Name of the file to remove.
        /// </param>
        void RemoveFile(string filename);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename" or @name="binarydata" or @name="size" or @name="mimetype"]'/>
        void AddFileWithMimeType(string filename, IntPtr binarydata, int size, string mimetype);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename" or @name="textdata" or @name="mimetype"]'/>
        void AddTextFileWithMimeType(string filename, String textdata, string mimetype);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename"]'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="textdata"]'/>
        void AddTextFile(string filename, string textdata);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename" or @name="binarydata" or @name="size"]'/>
        void AddFile(string filename, IntPtr binarydata, int size);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename" or @name="stream" or @name="mimetype"]'/>
        void AddStreamWithMimeType(string filename, Stream stream, string mimetype);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename" or @name="stream"]'/>
        void AddStream(string filename, Stream stream);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename" or @name="osFilename"]'/>
        void AddOSFile(string filename, string osFilename);
        //-------------------------------------------------
        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml" 
        ///     path='doc/AddFile/param[@name="filename" or @name="osFilename" or @name="mimetype"]'/>
        void AddOSFileWithMimeType(string filename, string osFilename, string mimetype);
        //-------------------------------------------------
    }
    //-------------------------------------------------
}
