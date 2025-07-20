using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the value of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public interface IValueSource<T>
    {
        /// <summary>
        /// Occurs when the value is changed.
        /// </summary>
        event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <returns></returns>
        T? Value { get; set; }
    }
}
