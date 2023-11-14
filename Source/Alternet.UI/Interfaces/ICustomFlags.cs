using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which work with flags.
    /// </summary>
    public interface ICustomFlags
    {
        /// <summary>
        /// Flag with <paramref name="name"/> was added.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <returns><c>true</c> if flag was added; <c>false</c> otherwise.</returns>
        bool HasFlag(string name);

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
