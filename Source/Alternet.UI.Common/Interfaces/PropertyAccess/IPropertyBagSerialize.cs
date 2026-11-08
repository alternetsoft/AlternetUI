using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines an interface for serializing and deserializing objects to and from a property bag.
    /// </summary>
    public interface IPropertyBagSerialize
    {
        /// <summary>
        /// Loads the state of this instance from the specified <see cref="ISerializableObjectContext"/>.
        /// </summary>
        /// <param name="context">The context containing the property bag and optional prefix for property names.</param>
        /// <returns>True if the state was successfully loaded; otherwise, false.</returns>
        bool LoadFrom(ISerializableObjectContext? context);

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="ISerializableObjectContext"/>.
        /// </summary>
        /// <param name="context">The context to which the state will be saved.</param>
        bool SaveTo(ISerializableObjectContext? context);
    }
}
