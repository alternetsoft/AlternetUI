using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the format in which a <see cref="SerializableObject"/> should be saved.
    /// </summary>
    public enum SerializableSaveFormat
    {
        /// <summary>
        /// Specifies that the object should be serialized to a JSON format.
        /// </summary>
        Json,

        /// <summary>
        /// Specifies that the object should be serialized to an XML format.
        /// </summary>
        Xml,
    }
}
