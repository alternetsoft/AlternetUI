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
        /// Loads the state of this instance from the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag from which the state will be loaded.</param>
        /// <param name="prefix">An optional prefix to prepend to property names.</param>
        /// <returns>True if the state was successfully loaded; otherwise, false.</returns>
        bool LoadFrom(IPropertyBag? propertyBag, string? prefix = null);

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag to which the state will be saved.</param>
        /// <param name="prefix">An optional prefix to prepend to property names.</param>
        void SaveTo(IPropertyBag propertyBag, string? prefix = null);
    }
}
