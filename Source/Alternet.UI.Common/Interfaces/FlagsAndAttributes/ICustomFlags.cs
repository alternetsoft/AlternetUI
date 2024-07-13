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
    public interface ICustomFlags : IReadOnlyCustomFlags
    {
        /// <summary>
        /// Calls <see cref="IReadOnlyCustomFlags.HasFlag"/> and <see cref="SetFlag"/>.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <returns></returns>
        new bool this[string name] { get; set; }

        /// <summary>
        /// Adds flag with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Flag name.</param>
        void AddFlag(string name);

        /// <summary>
        /// Removes flag with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Flag name.</param>
        void RemoveFlag(string name);

        /// <summary>
        /// If flag with <paramref name="name"/> was previously added, it is removed.
        /// And if there is no such flag, it is added.
        /// </summary>
        /// <param name="name">Flag name.</param>
        void ToggleFlag(string name);

        /// <summary>
        /// Adds flag if <paramref name="value"/> is true, removes flag if its false.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <param name="value">Add or remove flag.</param>
        void SetFlag(string name, bool value);
    }
}
