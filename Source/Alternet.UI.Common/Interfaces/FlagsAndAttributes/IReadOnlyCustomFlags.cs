using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to get readonly access to flags.
    /// </summary>
    public interface IReadOnlyCustomFlags
    {
        /// <summary>
        /// Calls <see cref="HasFlag"/>.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <returns></returns>
        bool this[string name] { get; }

        /// <summary>
        /// Flag with <paramref name="name"/> was added.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <returns><c>true</c> if flag was added; <c>false</c> otherwise.</returns>
        bool HasFlag(string name);
    }
}
