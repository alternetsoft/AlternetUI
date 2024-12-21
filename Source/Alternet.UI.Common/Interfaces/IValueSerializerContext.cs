using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alternet.UI.Port
{
    /// <summary>
    /// Context provided to ValueSerializer that can be used to special case serialization
    /// for different users of the ValueSerializer or for modes of serialization.
    /// </summary>
    public interface IValueSerializerContext : ITypeDescriptorContext
    {
        /// <summary>
        /// Get the value serializer associated with the given type.
        /// </summary>
        /// <param name="type">The type of the value that is to be convert.</param>
        /// <returns>A value serializer for capable of serializing the given type.</returns>
        ValueSerializer? GetValueSerializerFor(Type type);

        /// <summary>
        /// Get a value serializer for the given property descriptor. A property can override
        /// the value serializer that
        /// is to be used to serialize the property by specifing either a
        /// ValueSerializerAttribute or a
        /// TypeConverterAttribute. This method takes these attributes into
        /// account when determining the value
        /// serializer.
        /// </summary>
        /// <param name="descriptor">The property descriptor for whose property
        /// value is being converted.</param>
        /// <returns>A value serializer capable of serializing the given property.</returns>
        ValueSerializer? GetValueSerializerFor(PropertyDescriptor descriptor);
    }
}
