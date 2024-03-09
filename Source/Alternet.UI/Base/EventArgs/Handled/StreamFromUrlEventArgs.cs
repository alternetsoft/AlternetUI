using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for url to <see cref="Stream"/> convertion events.
    /// </summary>
    public class StreamFromUrlEventArgs : ValueConvertEventArgs<string, Stream?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamFromUrlEventArgs"/> class.
        /// </summary>
        /// <param name="url">Url with data.</param>
        public StreamFromUrlEventArgs(string? url)
            : base(url)
        {
        }
    }
}
