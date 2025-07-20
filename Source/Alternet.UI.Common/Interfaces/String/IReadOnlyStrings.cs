using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to access array of strings in read only mode.
    /// </summary>
    public interface IReadOnlyStrings
    {
        /// <summary>
        /// Gets the number of elements.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="IReadOnlyStrings"/>.
        /// </returns>
        int Count { get; }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        string? this[int index] { get; }
    }
}