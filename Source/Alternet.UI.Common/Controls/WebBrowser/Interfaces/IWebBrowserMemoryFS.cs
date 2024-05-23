using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Interface for access to memory protocol scheme.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface can store arbitrary data in memory
    /// stream and make them accessible via an URL in <see cref="WebBrowser"/> control.
    /// </remarks>
    /// <remarks>
    /// If you know MIME type of the file, use it when adding file to the memory file system.
    /// It makes accessing the files faster.
    /// </remarks>
    /// <remarks>
    /// When files are added to the memory file system, stored data (bitmap, text, raw data) are
    /// copied into private memory stream
    /// and become available in <see cref="WebBrowser"/> under name "memory:" + filename.
    /// Before using memory file system, you need to initialize it with <see cref="Init"/>.
    /// </remarks>
    public interface IWebBrowserMemoryFS
    {
        /// <summary>
        /// Initializes memory protocol scheme for use with the
        /// <see cref="WebBrowser"/> control.
        /// </summary>
        /// <remarks>
        /// You need to call this method before any other methods of the interface.
        /// </remarks>
        /// <param name="schemeName">Name of the url protocol that will be used
        /// instead of http.</param>
        /// <example>
        ///     <code>
        ///         WebBrowser1.MemoryFS.Init("memory");
        ///         WebBrowser1.MemoryFS.AddString("myFolder/index.html",
        ///             "<html><body><b>sample html file</b></body></html>");
        ///         WebBrowser1.LoadURL("memory:myFolder/index.html");
        ///     </code>
        /// </example>
        bool Init(string schemeName);

        /// <summary>
        /// Removes a file from memory file system and frees the occupied memory.
        /// </summary>
        /// <param name="filename">Name of the file to remove.</param>
        bool Remove(string filename);

        /// <summary>
        /// Adds the file to the list of the files stored in memory.
        /// </summary>
        /// <param name = "filename" > Specifies name of the file in the memory file system.</param>
        /// <param name = "binarydata" > Specifies pointer to the file's raw data.</param>
        /// <param name = "size" > Specifies size of the file.</param>
        /// <param name = "mimetype" > Specifies added file's MIME type.</param>
        bool Add(string filename, IntPtr binarydata, int size, string? mimetype = null);

        /// <summary>
        /// Adds the file to the list of the files stored in memory.
        /// </summary>
        /// <param name = "filename" > Specifies name of the file in the memory file system.</param>
        /// <param name = "textdata" > Specifies string with text file data.</param>
        /// <param name = "mimetype" > Specifies added file's MIME type.</param>
        bool AddString(string filename, string textdata, string? mimetype = null);

        /// <summary>
        /// Adds the file to the list of the files stored in memory.
        /// </summary>
        /// <param name = "filename" > Specifies name of the file in the memory file system.</param>
        /// <param name = "stream" > Specifies stream with the file data.</param>
        /// <param name = "mimetype" > Specifies added file's MIME type.</param>
        bool Add(string filename, Stream stream, string? mimetype = null);

        /// <summary>
        /// Adds the file to the list of the files stored in memory.
        /// </summary>
        /// <param name = "filename" > Specifies name of the file in the memory file system.</param>
        /// <param name = "osFilename" > Specifies filename on the hard drive to read the data from.</param>
        /// <param name = "mimetype" > Specifies added file's MIME type.</param>
        bool Add(string filename, string osFilename, string? mimetype = null);
    }
}
