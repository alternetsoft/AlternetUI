using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to get readonly access to flags.
    /// </summary>
    /// <typeparam name="TKey">Type of the flag identifier.</typeparam>
    public interface IReadOnlyFlags<TKey>
    {
        /// <summary>
        /// Calls <see cref="HasFlag"/>.
        /// </summary>
        /// <param name="id">Flag identifier.</param>
        /// <returns></returns>
        bool this[TKey id] { get; }

        /// <summary>
        /// Flag with <paramref name="id"/> was added.
        /// </summary>
        /// <param name="id">Flag identifier.</param>
        /// <returns><c>true</c> if flag was added; <c>false</c> otherwise.</returns>
        bool HasFlag(TKey id);
    }
}
