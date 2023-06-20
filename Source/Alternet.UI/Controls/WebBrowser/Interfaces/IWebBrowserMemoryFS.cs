using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    ///     Interface for access to memory protocol scheme.
    /// </summary>
	/// <remarks>
    ///     Implementations of this interface can store arbitrary data in memory
    ///     stream and make them accessible via an URL in
    ///     <see cref="WebBrowser"/> control.
	/// </remarks>
    public interface IWebBrowserMemoryFS
    {
        /// <summary>
        ///     Initializes memory protocol scheme for use with the
        ///     <see cref="WebBrowser"/> control. 
        /// </summary>
		/// <remarks>
		/// 	You need to call this method before any other methods of the interface.
		/// </remarks>
        /// <param name="schemeName">Name of the url protocol that will be used instead of http.</param>
        /// <example>
        ///     <code>
        ///         WebBrowser1.MemoryFS.Init("memory");
        ///         WebBrowser1.MemoryFS.AddTextFile("myFolder/index.html",
        ///             "<html><body><b>sample html file</b></body></html>");
        ///         WebBrowser1.LoadURL("memory:myFolder/index.html");
        ///     </code>
        /// </example>
        void Init(string schemeName);

        /// <summary>
        ///     Removes a file from memory file system and frees the occupied memory.
        /// </summary>
        /// <param name="filename">
        ///     Name of the file to remove.
        /// </param>
        void RemoveFile(string filename);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename" or @name="binarydata" or @name="size" or @name="mimetype"]'/>
        void AddFileWithMimeType(string filename, IntPtr binarydata, int size, string mimetype);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename" or @name="textdata" or @name="mimetype"]'/>
        void AddTextFileWithMimeType(string filename, string textdata, string mimetype);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename"]'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="textdata"]'/>
        void AddTextFile(string filename, string textdata);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename" or @name="binarydata" or @name="size"]'/>
        void AddFile(string filename, IntPtr binarydata, int size);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename" or @name="stream" or @name="mimetype"]'/>
        void AddStreamWithMimeType(string filename, Stream stream, string mimetype);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename" or @name="stream"]'/>
        void AddStream(string filename, Stream stream);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename" or @name="osFilename"]'/>
        void AddOSFile(string filename, string osFilename);

        /// <include file="IWebBrowserMemoryFS.xml" path='doc/AddFile/summary'/>
        /// <include file="IWebBrowserMemoryFS.xml"
        ///     path='doc/AddFile/param[@name="filename" or @name="osFilename" or @name="mimetype"]'/>
        void AddOSFileWithMimeType(string filename, string osFilename, string mimetype);
    }
}
