using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the format in which a <see cref="SerializableObject"/> should be deserialized.
    /// </summary>
    public enum SerializableLoadFormat
    {
        /// <summary>
        /// Specifies that the object should be deserialized from a JSON format.
        /// </summary>
        Json,

        /// <summary>
        /// Specifies that the object should be deserialized from an XML format.
        /// </summary>
        Xml,
    }
}
