using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with flags.
    /// </summary>
    /// <typeparam name="TKey">Type of the flag identifier.</typeparam>
    public interface ICustomFlags<TKey> : IReadOnlyFlags<TKey>
    {
        /// <summary>
        /// Calls <see cref="IReadOnlyFlags{T}.HasFlag"/> and <see cref="SetFlag"/>.
        /// </summary>
        /// <param name="id">Flag identifier.</param>
        /// <returns></returns>
        new bool this[TKey id] { get; set; }

        /// <summary>
        /// Adds flag with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Flag identifier.</param>
        void AddFlag(TKey id);

        /// <summary>
        /// Removes flag with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Flag identifier.</param>
        void RemoveFlag(TKey id);

        /// <summary>
        /// If flag with <paramref name="id"/> was previously added, it is removed.
        /// And if there is no such flag, it is added.
        /// </summary>
        /// <param name="id">Flag identifier.</param>
        void ToggleFlag(TKey id);

        /// <summary>
        /// Adds flag if <paramref name="value"/> is true, removes flag if its false.
        /// </summary>
        /// <param name="id">Flag identifier.</param>
        /// <param name="value">Add or remove flag.</param>
        void SetFlag(TKey id, bool value);
    }
}
