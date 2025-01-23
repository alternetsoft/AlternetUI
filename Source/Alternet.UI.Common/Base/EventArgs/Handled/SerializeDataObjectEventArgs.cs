using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Event arguments for <see cref="DataObject.GlobalSerializeDataObject"/> and
    /// <see cref="DataObject.GlobalDeserializeDataObject"/> events.
    /// </summary>
    public class SerializeDataObjectEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeDataObjectEventArgs"/> class
        /// with the specified parameters.
        /// </summary>
        /// <param name="data">Data object.</param>
        /// <param name="stream">Stream where serialization is performed.</param>
        public SerializeDataObjectEventArgs(object? data, Stream stream)
        {
            Data = data;
            Stream = stream;
        }

        /// <summary>
        /// Gets or sets data object.
        /// </summary>
        public virtual object? Data { get; set; }

        /// <summary>
        /// Gets or sets data stream where serialization is performed.
        /// </summary>
        public virtual Stream Stream { get; set; }
    }
}
