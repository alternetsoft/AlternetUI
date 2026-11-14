using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a context for serialization and deserialization operations,
    /// including the property bag and an optional prefix for property names.
    /// </summary>
    public interface ISerializableObjectContext
    {
        /// <summary>
        /// Gets or sets the <see cref="IPropertyBag"/> used for serialization and deserialization.
        /// </summary>
        public IPropertyBag? PropertyBag { get; set; }

        /// <summary>
        /// Gets or sets an optional prefix for property names in the property bag.
        /// </summary>
        public string? Prefix { get; set; }
    }

    /// <summary>
    /// Represents the context for serialization and deserialization operations,
    /// including the property bag and an optional prefix for property names.
    /// </summary>
    public struct SerializableObjectContext : ISerializableObjectContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableObjectContext"/> struct.
        /// </summary>
        public SerializableObjectContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableObjectContext"/>
        /// </summary>
        /// <param name="createPropertyBag">Indicates whether to create a new property bag.</param>
        public SerializableObjectContext(bool createPropertyBag)
        {
            PropertyBag = createPropertyBag ? UI.PropertyBag.Create() : null;
            Prefix = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableObjectContext"/>
        /// struct with the specified property bag.
        /// </summary>
        /// <param name="propertyBag">The property bag to be used for serialization and deserialization.</param>
        public SerializableObjectContext(IPropertyBag propertyBag)
        {
            PropertyBag = propertyBag;
            Prefix = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableObjectContext"/>
        /// struct with the specified property bag and an optional prefix for property names.
        /// </summary>
        /// <param name="propertyBag">The property bag to be used for serialization and deserialization.</param>
        /// <param name="prefix">An optional prefix for property names in the property bag.</param>
        public SerializableObjectContext(IPropertyBag propertyBag, string? prefix)
        {
            PropertyBag = propertyBag;
            Prefix = prefix;
        }

        /// <summary>
        /// Gets or sets the <see cref="IPropertyBag"/> used for serialization and deserialization.
        /// </summary>
        public IPropertyBag? PropertyBag { get; set; }

        /// <summary>
        /// Gets or sets an optional prefix for property names in the property bag.
        /// </summary>
        public string? Prefix { get; set; }
    }
}
