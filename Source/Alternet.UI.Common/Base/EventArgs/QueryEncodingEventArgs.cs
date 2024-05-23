using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the query encoding events.
    /// </summary>
    public class QueryEncodingEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryEncodingEventArgs"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file for which encoding is requested.</param>
        public QueryEncodingEventArgs(string? fileName)
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryEncodingEventArgs"/> class.
        /// </summary>
        /// <param name="stream">Stream with data for which encoding is requested.</param>
        public QueryEncodingEventArgs(Stream? stream)
        {
            this.Stream = stream;
        }

        /// <summary>
        /// Gets name of the file for which encoding is requested.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Gets stream with data for which encoding is requested.
        /// </summary>
        public Stream? Stream { get; set; }

        /// <summary>
        /// Gets or sets encoding of the file.
        /// </summary>
        public Encoding? Result { get; set; }
    }
}
